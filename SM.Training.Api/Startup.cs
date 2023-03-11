using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using SoapCore;

namespace SM.Training.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //phuc vu cho phan authen 
            services.AddSoapCore();

            ServiceCollection = services;
            services.AddControllers();
            services.AddControllers()
                  .AddNewtonsoftJson(options =>
                  {
                      // Sử dụng cấu hình này để phía client nhận được Object có dạng giống trên Server
                      // mà không phải theo dạng camelCase
                      options.SerializerSettings.ContractResolver = new DefaultContractResolver();

                      // Do cấu hình trên custom lại việc Serializer JSON nên sử dụng 
                      // options.JsonSerializerOptions.IgnoreNullValues = true
                      // không còn tác dụng mà phải sử dụng cấu hình bên dưới
                      options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                  });
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters();
            });
        }
        public IServiceCollection ServiceCollection { get; private set; }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Cấu hình cho phép CORS
            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders("Content-Disposition"));

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
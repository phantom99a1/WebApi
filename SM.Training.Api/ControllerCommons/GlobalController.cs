using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace SM.Training.Api.ControllerCommons
{
    [Route("api/Global")]
    [AllowAnonymous]
    public class GlobalController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "Api is running: " + DateTime.Now.ToString();
        }

        [HttpPost]
        public GlobalDTO Post([FromBody] GlobalDTO dtoResquest)
        {
            var dtoResponse = new GlobalDTO();
            dtoResponse.Transaction_Code = "Response:" + dtoResquest.Transaction_Code;

            return dtoResponse;
        }
    }

    public class GlobalDTO
    {
        public string Transaction_Code { get; set; }
    }
}

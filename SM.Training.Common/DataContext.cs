using SM.Training.Utils;

namespace SM.Training.Common
{
    public class DataContext : SoftMart.Core.Dao.EnterpriseService
    {
        public DataContext(int commandTimeout)
            : this()
        {
            this.CommandTimeOut = commandTimeout;
        }

        public DataContext()
            : base(ConfigUtils.ApplicationDataConnection)
        {
            this.CommandTimeOut = 30;
        }

        public DataContext(string connectionString, int commandTimeout)
          : base(connectionString)
        {
            this.CommandTimeOut = commandTimeout;
        }
    }
}

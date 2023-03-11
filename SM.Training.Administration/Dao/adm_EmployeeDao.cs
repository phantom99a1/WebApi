using SM.Training.Common;
using SM.Training.SharedComponent.Entities.Administrations;
using System.Linq;

namespace SM.Training.Administration.Dao
{
    class adm_EmployeeDao : BaseDao
    {
        public adm_Employee Test()
        {
            var query = @"select * from ADM_EMPLOYEE";
            using (var dataContext = new DataContext())
            {
                return dataContext.ExecuteSelect<adm_Employee>(query).FirstOrDefault();
            }
        }
    }
}

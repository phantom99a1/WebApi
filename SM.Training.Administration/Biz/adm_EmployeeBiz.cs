using SM.Training.Administration.Dao;
using SM.Training.SharedComponent.Entities.Administrations;

namespace SM.Training.Administration.Biz
{
    public class adm_EmployeeBiz
    {
        private adm_EmployeeDao _dao = new adm_EmployeeDao();
        public (adm_Employee employee, object obj) Test()
        {
            var employee = _dao.Test();

            return (employee, null);
        }
    }
}

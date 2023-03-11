using Oracle.ManagedDataAccess.Client;
using SM.Training.Common;
using SM.Training.SharedComponent.Entities.Administrations;
using System.Collections.Generic;
using System.Linq;

namespace SM.Training.Administration.Dao
{
    public class Duatpt_EmployeeDao : BaseDao
    {
        //todo: Tìm kiếm không phân biệt hoa thường
        public List<Duatpt_Employee> SearchEmployee(Filter f)
        {
            string query =@"Select * 
                            from DUATPT_EMPLOYEE 
                            where USER_NAME = :username 
	                            or NAME like :name
                            ";

            OracleCommand cmd = new OracleCommand(query);
            cmd.Parameters.Add("username", f.username);
            cmd.Parameters.Add("name", BuildLikeFilter(f.name));

            using (var dataContext = new DataContext())
                return dataContext.ExecuteSelect<Duatpt_Employee>(cmd);
        }

        public Duatpt_Employee GetEmployeeDetailByEmployeeID(Filter f)
        {
            string query = "Select * from DUATPT_EMPLOYEE where EMPLOYEE_ID= :employee_id";
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = query;
            cmd.Parameters.Add("employee_id", f.employee_id);
            using (var dataContext = new DataContext())
                return dataContext.ExecuteSelect<Duatpt_Employee>(cmd).FirstOrDefault();
        }

        //todo: Không xóa thẳng trong bảng, chỉ mark Deleted = true
        //update DELETE
        public bool? DeleteEmployee(Filter f)
        {
            string query = @"Update DUATPT_EMPLOYEE set DELETED=1 
                             where EMPLOYEE_ID= :employee_id and VERSION= :version";
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = query;
            cmd.Parameters.Add("employee_id", f.employee_id);
            cmd.Parameters.Add("version", f.version);
            Duatpt_Employee Delete = new Duatpt_Employee();
            using (var dataContext = new DataContext())
                if (dataContext.ExecuteNonQuery(cmd) != 0)
                    Delete.Deleted = true;
                else
                    Delete.Deleted = false;
            return Delete.Deleted;
        }

        //todo: Dùng InsertItem
        public bool isValidEmployee(Duatpt_Employee emp)
        {
            var isValid = false;            
            if (emp.Employee_ID is null||string.IsNullOrEmpty(emp.User_Name) || string.IsNullOrEmpty(emp.Name) || string.IsNullOrEmpty(emp.Address) ||
                string.IsNullOrEmpty(emp.Phone_Number) || string.IsNullOrEmpty(emp.Dob) || string.IsNullOrEmpty(emp.GENDER_NAME))
                isValid = false;
            else isValid = true;            
            return isValid;
        }
        
        public bool isDistinctUsername(Duatpt_Employee emp)
        {
            var isDistinct = false;
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = @"select USER_NAME from DUATPT_EMPLOYEE where USER_NAME= :username";
            cmd.Parameters.Add("username", emp.User_Name);
            using (var dataContext = new DataContext())
            {
                if (dataContext.ExecuteSelect<Duatpt_Employee>(cmd).Count() ==0)
                    isDistinct = true;
                else isDistinct = false;
            }
            return isDistinct;
        }
        public void InsertEmployee(Duatpt_Employee emp)
        {
            InsertItem(emp);
        }
        //todo: Dùng UpdateItem

        public bool isValidEmployee1(Duatpt_Employee emp)
        {
            var isValid = false;
            if (isValidEmployee(emp) == false)
                isValid = false;
            if (emp.Employee_ID is null || emp.Version == null)
                isValid = false;
            isValid = true;
            return isValid;
        }
        public void EmployeeUpdate(Duatpt_Employee emp)
        {           
            

        }

    }
}

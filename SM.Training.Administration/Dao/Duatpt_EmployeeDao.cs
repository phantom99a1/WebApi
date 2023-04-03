using System.Collections.Generic;
using System.Linq;
using Oracle.ManagedDataAccess.Client;
using SM.Training.Common;
using SM.Training.SharedComponent.Entities.Administrations;

namespace SM.Training.Administration.Dao
{
    public class Duatpt_EmployeeDao : BaseDao
    {
        //todo: Tìm kiếm không phân biệt hoa thường

        public bool CheckString(string s)
        {
            if (string.IsNullOrWhiteSpace(s) || string.Empty.Equals(s))
                return false;
            return true;
        }
        public List<ResultSearchEmployee> SearchEmployee(string User_Name, string Name, int pageIndex, int pageSize, out int totalRecord)
        {
            //todo: Update lại(done)
            string query = @"select e.employee_id,
                                e.user_name,e.name,e.Address,
                                e.phone_number,
                                e.gender,e.dob,e.VERSION,temp.Certificate
                                from duatpt_employee e 
                                left join (select employee_id, listagg(cert_name,';') within group (order by employee_id) as Certificate
                                    from duatpt_employee_cert 
                                    group by employee_id) temp on e.employee_id=temp.employee_id where deleted = 0";

            OracleCommand cmd = new OracleCommand();                         
            //Check nếu có User_Name thì cộng thêm query tìm kiếm User_Name
            if (CheckString(User_Name))
                query += " and USER_NAME collate binary_ci = :username";
            //Check nếu có Name thì cộng thêm query tìm kiếm Name
            if (CheckString(Name))
                    query += " and NAME collate binary_ci like :name";
            
            //Thêm order
            query += " order by EMPLOYEE_ID asc";

            cmd.CommandText = query;
            cmd.Parameters.Add("username", User_Name);
            cmd.Parameters.Add("name", BuildLikeFilter(Name));
            using (var dataContext = new DataContext())
            {
                return dataContext.ExecutePaging<ResultSearchEmployee>(cmd, pageIndex, pageSize, out totalRecord);
            }
        }

        //todo: Dao không xử lý nhiều query(done)
        public Duatpt_Employee  GetEmployeeDetailByEmployeeID(int? Employee_ID)
        {
            string query = @"Select 
                                e.EMPLOYEE_ID,
                                e.USER_NAME,
                                e.NAME,
                                e.ADDRESS,
                                e.PHONE_NUMBER,
                                e.DOB,
                                e.Gender,
                                e.VERSION                    
                            from DUATPT_EMPLOYEE e where EMPLOYEE_ID= :employee_id";

            
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = query;
            cmd.Parameters.Add("employee_id", Employee_ID);
            
            using (var dataContext = new DataContext())
                return dataContext.ExecuteSelect<Duatpt_Employee>(cmd).FirstOrDefault();
        }
        public List<Duatpt_Employee_Cert> GetDetailCertByEmployeeID(int? Employee_ID)
        {
            string query2 = @"Select CERT_ID,EMPLOYEE_ID, CERT_NAME,CERT_CODE from duatpt_employee_cert
            where Employee_id=:emp_id";
            OracleCommand cmd2 = new OracleCommand();
            cmd2.CommandText = query2;
            cmd2.Parameters.Add("emp_id", Employee_ID);
            using (var dataContext = new DataContext())
                return dataContext.ExecuteSelect<Duatpt_Employee_Cert>(cmd2);
        }

        public bool? DeleteEmployee(int? Employee_ID, int? Version)
        {
            string query = @"Update DUATPT_EMPLOYEE set DELETED = 1 
                             where EMPLOYEE_ID= :employee_id and VERSION = :version";
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = query;
            cmd.Parameters.Add("employee_id", Employee_ID);
            cmd.Parameters.Add("version", Version);
            using (var dataContext = new DataContext())
                return dataContext.ExecuteNonQuery(cmd) != 0;
        }        
        //Xóa nhiều bản ghi
        public bool? DeleteAllEmployee(int? Employee_ID, int? Version)
        {
            string query = @"Update DUATPT_EMPLOYEE set DELETED = 1 
                             where (EMPLOYEE_ID,VERSION) in ((:employee_id,:version))";
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = query;
            cmd.Parameters.Add("employee_id", Employee_ID);
            cmd.Parameters.Add("version", Version);
            using (var dataContext = new DataContext())
                return dataContext.ExecuteNonQuery(cmd) != 0;
        }


        //todo: Cho phép trùng các bản ghi đã xóa(done)
        public bool IsDistinctUsername(Duatpt_Employee emp)
        {
            bool isDistinct;
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = @"select USER_NAME from DUATPT_EMPLOYEE where USER_NAME= :username and deleted=0";
            cmd.Parameters.Add("username", emp.User_Name);
            using (var dataContext = new DataContext())
            {
                if (dataContext.ExecuteSelect<Duatpt_Employee>(cmd).Count() == 0)
                    isDistinct = true;
                else isDistinct = false;
            }
            return isDistinct;
        }

        public void Delete_Cert(int Cert_ID)
        {
            string query = @"Delete from DUATPT_EMPLOYEE_CERT
                            where CERT_ID= :Cert_ID";
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = query;
            cmd.Parameters.Add("Cert_ID", Cert_ID);
            using (DataContext dataContext=new DataContext())
            {
                dataContext.ExecuteNonQuery(cmd);
            }              
        }

        public void Delete_CertByEmployee_ID(int Employee_ID)
        {
            string query = @"Delete from DUATPT_EMPLOYEE_CERT
                            where EMPLOYEE_ID= :Employee_ID";
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = query;
            cmd.Parameters.Add("Employee_ID", Employee_ID);
            using (DataContext dataContext = new DataContext())
            {
                dataContext.ExecuteNonQuery(cmd);
            }
        }

        public void DeleteAll_CertByEmployee_ID(int Employee_ID)
        {
            string query = @"Delete from DUATPT_EMPLOYEE_CERT
                            where EMPLOYEE_ID in :Employee_ID";
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = query;
            cmd.Parameters.Add("Employee_ID", Employee_ID);
            using (DataContext dataContext = new DataContext())
            {
                dataContext.ExecuteNonQuery(cmd);
            }
        }
    }
}

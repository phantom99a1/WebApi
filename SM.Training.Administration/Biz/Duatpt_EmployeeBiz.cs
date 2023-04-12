using System;
using System.Collections.Generic;
using System.Transactions;
using SM.Training.Administration.Dao;
using SM.Training.SharedComponent.Constants;
using SM.Training.SharedComponent.Entities.Administrations;

namespace SM.Training.Administration.Biz
{
    public class Duatpt_EmployeeBiz
    {
        private Duatpt_EmployeeDao _dao = new Duatpt_EmployeeDao();
        public (List<ResultSearchEmployee> employees, object obj) SearchEmployee(Duatpt_Employee emp,int PageIndex, int PageSize, out int totalRecord)
        {
            //todo: Truyền cả obj filter xuống Dao
            var employees = _dao.SearchEmployee(emp.User_Name, emp.Name,PageIndex,PageSize, out totalRecord);
            if (employees == null)
                throw new Exception("Không thể tìm được bản ghi nào");

            return (employees, null);
        }  

        public ((Duatpt_Employee employee,List<Duatpt_Employee_Cert>emp_certs) emp, object obj) GetEmployeeDetail(Duatpt_Employee emp)
        {
            //Lấy thông tin bảng chính emp
            var employee = _dao.GetEmployeeDetailByEmployeeID(emp.Employee_ID);
            if (employee == null)
                throw new Exception("Không tìm thấy bản ghi nào phù hợp");

            //Lấy thông tin cert theo empId
            var employee_certs = _dao.GetDetailCertByEmployeeID(emp.Employee_ID);
            if (employee_certs == null)
                throw new Exception("Không có thông tin về giấy tờ");
            return ((employee,employee_certs), null);
        }

        public void Delete_Employee(Duatpt_Employee emp)
        {
            bool? isUpdated = false;
            using (TransactionScope scope = new TransactionScope())
            {
                isUpdated = _dao.DeleteEmployee(emp.Employee_ID, emp.VERSION);
                _dao.Delete_CertByEmployee_ID(emp.Employee_ID.Value);
                scope.Complete();
            }

            if (isUpdated == false)
                throw new Exception("Không có bản ghi phù hợp để xóa");
        }
        
        public int? DeleteAll_Employee(List<Duatpt_Employee> emp)
        {
            List<bool?> DeleteRecord = new List<bool?>();
            foreach (var element in emp)
            {
                element.Deleted = _dao.DeleteEmployee(element.Employee_ID, element.VERSION);
                _dao.DeleteAll_CertByEmployee_ID(element.Employee_ID.Value);
                if (element.Deleted == true)
                    DeleteRecord.Add(element.Deleted);
            }

            if (DeleteRecord.Count==0)
                throw new Exception("Không có bản ghi phù hợp để xóa");
            
            return DeleteRecord.Count;
        }

        public void InsertEmployee(Duatpt_Employee emp, List<Duatpt_Employee_Cert>emp_certs)
        {           
            if (_dao.IsDistinctUsername(emp) == false)
                throw new Exception("Đã trùng Username! Mời nhập lại Username");

            //Set extend prop
            emp.Deleted = SMX.IsNotDeleted;
            emp.Created_By = SMX.DefaultAccount;
            emp.VERSION = SMX.FirstVersion;
            emp.Created_Dtg = DateTime.Now;

            //Call dao
            using (TransactionScope scope = new TransactionScope())
            {                
                _dao.InsertItem(emp);

                foreach (var item in emp_certs)
                    item.Employee_ID = emp.Employee_ID;

                _dao.InsertItems(emp_certs);

                scope.Complete();
            }
        }

        public void UpdateEmployee(Duatpt_Employee emp,List<Duatpt_Employee_Cert> emp_certs, List<Duatpt_Employee_Cert> emp_certs_delete)
        {
            string[] columns = new string[2]
            {
                "CERT_NAME","CERT_CODE"
            };  
            //Set extend prop            
            emp.Updated_By = "System";
            emp.Updated_Dtg = DateTime.Now;

            var listUpdate = new List<Duatpt_Employee_Cert>();
            var listInsert = new List<Duatpt_Employee_Cert>();
            foreach(var item in emp_certs)
            {
                if (item.Cert_ID == null)
                {
                    item.Employee_ID = emp.Employee_ID;
                    listInsert.Add(item);
                }
                else listUpdate.Add(item);
            }

            //Call Dao
            using (TransactionScope scope = new TransactionScope())
            {
                _dao.UpdateItem(emp);

                _dao.UpdateItems(listUpdate, columns);

                _dao.InsertItems(listInsert);

                foreach(var item in emp_certs_delete)
                {
                    _dao.Delete_Cert(item.Cert_ID.Value);
                }

                scope.Complete();
            }           
        }

        public (List<KeyValuePair<int,string>> gender,object obj) GetGender()
        {
            List<KeyValuePair<int, string>> lst = new List<KeyValuePair<int, string>>();
           Dictionary<int, string> dic = new Dictionary<int, string>()
           {
               {
                   0,"Nhập vào giới tính"
               } ,
               {
                   1,"Nam"
               },
               {
                   2,"Nữ"
               },
               {
                   3,"Khác"
               }               
           };
            foreach (KeyValuePair<int, string> kvp in dic)
                lst.Add(kvp);
            if (lst == null)
                throw new Exception("Không có thông tin về giới tính");
            return (lst, null);
        }
    }
}

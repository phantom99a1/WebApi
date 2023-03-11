using SM.Training.Administration.Dao;
using SM.Training.SharedComponent.Constants;
using SM.Training.SharedComponent.Entities.Administrations;
using System;
using System.Collections.Generic;

namespace SM.Training.Administration.Biz
{
    public class Duatpt_EmployeeBiz
    {
        private Duatpt_EmployeeDao _dao = new Duatpt_EmployeeDao();
        public (List<Duatpt_Employee> employees, object obj) SearchEmployee(Filter f)
        {
            //todo: Truyền cả obj filter xuống Dao
            var employees = _dao.SearchEmployee(f);
            if (employees == null)
                throw new Exception("Không thể tìm được bản ghi nào");
            return (employees, null);
        }

        public (Duatpt_Employee employee, object obj) GetEmployeeDetail(Filter f)
        {
            var employee = _dao.GetEmployeeDetailByEmployeeID(f);
            if (employee == null)
                throw new Exception("Không tìm thấy bản ghi nào phù hợp");

            return (employee, null);
        }

        public bool? Delete_Employee(Filter f)
        {
            Duatpt_Employee emp = new Duatpt_Employee();
            emp.Deleted = _dao.DeleteEmployee(f);
            if (emp.Deleted == false)
                throw new Exception("Không có bản ghi phù hợp để xóa");
            return emp.Deleted;
        }

        public void InsertEmployee(Duatpt_Employee emp)
        {
            
            //Validate item
            // 01. Validate xem có đủ thông tin không
            // 02. Validate không được trùng user_name
            
            //_dao.InsertEmployee(emp);
            if (_dao.isValidEmployee(emp) == false)
                throw new Exception("Bạn nhập chưa đủ thông tin! Mời nhập lại");
            if (_dao.isDistinctUsername(emp) == false)
                throw new Exception("Đã trùng Username! Mời nhập lại Username");

            //Set extend prop
            emp.Deleted = SMX.IsDeleted;
            emp.Created_By = SMX.DefaultAccount;
            emp.Version = SMX.FirstVersion;
            emp.Created_Dtg = DateTime.Now;

            //Call dao
            _dao.InsertItem(emp);
        }

        public void UpdateEmployee(Duatpt_Employee emp)
        {
            //Validate item
            // 01. Validate xem có đủ thông tin không
            // 02. Validate không được trùng user_name
            
            if (_dao.isValidEmployee1(emp) == false)
                throw new Exception("Bạn nhập chưa đủ thông tin! Mời nhập lại");
            if (_dao.isDistinctUsername(emp) == false)
                throw new Exception("Đã trùng Username! Mời nhập lại Username");
            
            //Set extend prop
            emp.Updated_By = "System";
            emp.Updated_Dtg = DateTime.Now;

            //Call Dao
            _dao.UpdateItem(emp);
        }
    }
}

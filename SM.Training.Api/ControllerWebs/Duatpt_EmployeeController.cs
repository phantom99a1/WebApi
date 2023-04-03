using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SM.Training.Administration.Biz;
using SM.Training.SharedComponent.Entities.Administrations;
using System;
using System.Collections.Generic;

namespace SM.Training.Api.ControllerCommons
{
    [Route("api/duatpt_employee/")]
    [AllowAnonymous]
    public class Duatpt_EmployeeController : Controller
    {
        [HttpPost]
        [Route("search")]
        public EmployeeDTO Search([FromBody] EmployeeDTO dtoRequest)
        {
            EmployeeDTO dtoResponse = new EmployeeDTO();
            int totalRecord;
            var f = dtoRequest.Filter;
            var pageIndex = dtoRequest.PageIndex;
            var pageSize = dtoRequest.PageSize;
            try
            {              
                Duatpt_EmployeeBiz biz = new Duatpt_EmployeeBiz();
                var result = biz.SearchEmployee(f, pageIndex, pageSize, out totalRecord);
               
                dtoResponse.ResultEmployees = result.employees;                
                dtoResponse.TotalRecords = totalRecord;
                
            }
            catch (Exception ex)
            {
                dtoResponse.Code = "01";
                dtoResponse.Message = ex.Message;
            }

            return dtoResponse;
        }       
        
        [HttpPost]
        [Route("detail")]
        public EmployeeDTO Detail([FromBody] EmployeeDTO dtoRequest)
        {
            EmployeeDTO dtoResponse = new EmployeeDTO();
            try
            {
                var emp = dtoRequest.Employee;                
                Duatpt_EmployeeBiz biz = new Duatpt_EmployeeBiz();
                var result = biz.GetEmployeeDetail(emp);
                dtoResponse.Employee = result.emp.employee;
                dtoResponse.Employees_Cert = result.emp.emp_certs;
            }
            catch(Exception ex)
            {
                dtoResponse.Code = "01";
                dtoResponse.Message = ex.Message;
            }
            
            return dtoResponse;
        }
        [HttpPost]
        [Route("delete")]
        public EmployeeDTO Delete([FromBody] EmployeeDTO emp)
        {
            var Employee = emp.Employee;
            var result = new EmployeeDTO();
            try
            {
                Duatpt_EmployeeBiz biz = new Duatpt_EmployeeBiz();
                biz.Delete_Employee(Employee);
            }
            catch (Exception ex)
            {
                result.Code = "01";
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("deleteall")]
        public EmployeeDTO DeleteAll([FromBody] EmployeeDTO emp)
        {
            //todo: Không dùng qua Route, chuyển sang Body
            // Sử dụng try catch để bắt exception
            var Emps = emp.Employees;
            var result = new EmployeeDTO();
            try
            {
                Duatpt_EmployeeBiz biz = new Duatpt_EmployeeBiz();
                var ketqua=biz.DeleteAll_Employee(Emps);                  
            }
            catch(Exception ex)
            {
                result.Code = "01";
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("insert")]
        public EmployeeDTO Insert([FromBody] EmployeeDTO emp)
        {
            var Employee = emp.Employee;
            var result = new EmployeeDTO();
            try
            {
                Duatpt_EmployeeBiz biz = new Duatpt_EmployeeBiz();          
                biz.InsertEmployee(Employee.CloneToInsert(),emp.Employees_Cert);
            }
            catch(Exception ex)
            {
                result.Code = "01";
                result.Message = ex.Message;
            }            

            return result;
        }

        [HttpPost]
        [Route("update")]
        public EmployeeDTO Update([FromBody] EmployeeDTO emp)
        {
            var result = new EmployeeDTO();
            try
            {
                Duatpt_EmployeeBiz biz = new Duatpt_EmployeeBiz();
                biz.UpdateEmployee(emp.Employee.CloneToUpdate(),emp.Employees_Cert,emp.Employees_Cert_Delete);
            }
            catch (Exception ex)
            {
                result.Code = "01";
                result.Message = ex.Message;
            }

            return result;
        }
        [HttpPost]
        [Route("getgender")]
        public EmployeeDTO GetGender()
        {
            var result = new EmployeeDTO();
            try
            {
                Duatpt_EmployeeBiz biz = new Duatpt_EmployeeBiz();
                result.Gender = biz.GetGender().gender;
            }
            catch(Exception ex)
            {
                result.Code = "01";
                result.Message = ex.Message;
            }
            return result;
        }
    }
    //phân trang:ExcutePaging
    public class EmployeeDTO
    {
        public Duatpt_Employee Filter { get; set; }
        public string Code { get; set; } = "00";
        public string Message { get; set; } = "";
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public Duatpt_Employee Employee { get; set; }
        public List<Duatpt_Employee> Employees { get; set; }

        public List<ResultSearchEmployee> ResultEmployees { get; set; }
        public List<KeyValuePair<int, string>> Gender { get; set; }

        public List<Duatpt_Employee_Cert> Employees_Cert { get; set; }
        public List<Duatpt_Employee_Cert> Employees_Cert_Delete { get; set; }
 

    }
}

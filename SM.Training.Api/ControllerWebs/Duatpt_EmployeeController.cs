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
            try
            {
                var f = dtoRequest.Filter;

                //todo: Truyền cả object Filter xuống biz
                Duatpt_EmployeeBiz biz = new Duatpt_EmployeeBiz();
                var result = biz.SearchEmployee(f);
                List<Duatpt_Employee> rs = new List<Duatpt_Employee>(); 
                foreach (var item in result.employees)
                    rs.Add(item.CloneToInsert());
                dtoResponse.Employees = rs;
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
            //todo: Không dùng qua Route, chuyển sang Body
            // Sử dụng try catch để bắt exception
            EmployeeDTO dtoResponse = new EmployeeDTO();
            try
            {
                var f = dtoRequest.Filter;                
                Duatpt_EmployeeBiz biz = new Duatpt_EmployeeBiz();
                var result = biz.GetEmployeeDetail(f).employee;
                
                dtoResponse.Employee = result.CloneToInsert();
                
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
        public EmployeeDTO Delete([FromBody] EmployeeDTO dtoRequest)
        {
            //todo: Không dùng qua Route, chuyển sang Body
            // Sử dụng try catch để bắt exception
            var result = new EmployeeDTO();
            try
            {
                var f = dtoRequest.Filter;
                Duatpt_EmployeeBiz biz = new Duatpt_EmployeeBiz();
                var ketqua=biz.Delete_Employee(f);                
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
        public EmployeeDTO Insert([FromBody] Duatpt_Employee emp)
        {
            //todo: Không dùng qua Route, chuyển sang Body
            // Sử dụng try catch để bắt exception
            
            var result = new EmployeeDTO();
            try
            {
                Duatpt_EmployeeBiz biz = new Duatpt_EmployeeBiz();
                biz.InsertEmployee(emp);
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
        public EmployeeDTO Update([FromBody] Duatpt_Employee emp)
        {
            //todo: Không dùng qua Route, chuyển sang Body
            // Sử dụng try catch để bắt exception

            emp = new Duatpt_Employee();
            var result = new EmployeeDTO();
            try
            {
                Duatpt_EmployeeBiz biz = new Duatpt_EmployeeBiz();
                biz.UpdateEmployee(emp);
            }
            catch (Exception ex)
            {
                result.Code = "01";
                result.Message = ex.Message;
            }

            return result;
        }
    }

    public class EmployeeDTO
    {
        public Filter Filter { get; set; }
        public string Code { get; set; } = "00";
        public string Message { get; set; } = "";
        public Duatpt_Employee Employee { get; set; }
        public List<Duatpt_Employee> Employees { get; set; }
    }
}

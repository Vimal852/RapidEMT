using Microsoft.EntityFrameworkCore;
using RapidEMT.Models;
using RapidEMT.Models.Responses;
using Microsoft.Extensions.Logging;
using RapidEMT.Models.DTO;
using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace RapidEMT.Services
{
    public interface IEmployeeService
    {
        Task<GetEmployeesResponse> GetEmployees();
        Task<BaseResponse> AddEmployee(AddEmployeeForm form);
        Task<GetEmployeeResponse> GetEmployee(int id);
        Task<BaseResponse> DeleteEmployee(Employee employee);
        Task<BaseResponse> EditEmployee(Employee employee);

    }

    public class EmployeeService : IEmployeeService
    {
        private readonly DataContext _factory;

        public EmployeeService(DataContext factory)
        {
            _factory = factory;
        }

        public async Task<BaseResponse> AddEmployee(AddEmployeeForm form)
        {


            var response = new BaseResponse();
            try
            {

                _factory.Add(new Employee
                {
                    Name = form.Name,
                    Position = form.Position,
                    Salary = form.Salary,
                    Type = form.Type,
                    ImgUrl = "https://picsum.photos/200?random=random.Next()}"
                });

                var result = await _factory.SaveChangesAsync();

                if (result == 1)
                {
                    response.StatusCode = 200;
                    response.Message = "Employee added successfully";
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Error occurred while adding the employee.";
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "Error adding employee: " + ex.Message;
            }

            return response;

        }
        public async Task<GetEmployeesResponse> GetEmployees()
        {
            var response = new GetEmployeesResponse();
            try
            {


                var employees = _factory.Employees.ToList();
                response.StatusCode = 200;
                response.Message = "Success";
                response.Employees = employees;

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "Error retrieving employees: " + ex.Message;
                response.Employees = null;
            }

            return response;

        }

        public async Task<GetEmployeeResponse> GetEmployee(int employeeId)
        {
            var response = new GetEmployeeResponse(); // Use GetEmployeeResponse instead of GetEmployeesResponse
            try
            {
                // Fetch the single employee by ID
                var employee = await _factory.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);

                if (employee != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Success";
                    response.Employee = employee;  // Store the single employee
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Employee not found";
                    response.Employee = null;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "Error retrieving employee: " + ex.Message;
                response.Employee = null;
            }

            return response;
        }
        public async Task<BaseResponse> DeleteEmployee(Employee employee)
        {
            var response = new BaseResponse();
            try
            {
                using (var context = _factory)
                {
                    context.Remove(employee);

                    var result = await context.SaveChangesAsync();

                    if (result == 1)
                    {
                        response.StatusCode = 204;
                        response.Message = "Employee removed successfully";
                    }
                    else
                    {
                        response.StatusCode = 400;
                        response.Message = "Error occurred while removing the employee.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "Error removing employee: " + ex.Message;
            }

            return response;
        }

        public async Task<BaseResponse> EditEmployee(Employee employee)
        {
            var response = new BaseResponse();
            try
            {
                using (var context = _factory)
                {
                    context.Update(employee);

                    var result = await context.SaveChangesAsync();

                    if (result == 1)
                    {
                        response.StatusCode = 200;
                        response.Message = "Employee updated successfully";
                    }
                    else
                    {
                        response.StatusCode = 400;
                        response.Message = "Error occurred while updating the employee.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "Error updating employee: " + ex.Message;
            }

            return response;
        }


    }
}
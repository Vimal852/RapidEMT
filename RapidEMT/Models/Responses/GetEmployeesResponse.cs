
using RapidEMT.Models;
using RapidEMT.Services;


namespace RapidEMT.Models.Responses
{
   
        public class GetEmployeesResponse : BaseResponse
        {
            public List<Employee>? Employees { get; set; }
        }
    
}

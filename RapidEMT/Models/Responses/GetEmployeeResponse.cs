namespace RapidEMT.Models.Responses
{
    public class GetEmployeeResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Employee? Employee { get; set; } // Single employee instead of a list
    }
}

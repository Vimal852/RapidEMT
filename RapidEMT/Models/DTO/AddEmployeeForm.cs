using RapidEMT.Models; // Ensure this contains your EmployeeType and Position enums
using System.ComponentModel.DataAnnotations;

namespace RapidEMT.Models.DTO
{
    public class AddEmployeeForm
    {
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        // Default value for ImgUrl
        public string ImgUrl { get; set; } = "https://picsum.photos/200";

        [Required(ErrorMessage = "Salary is required.")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Employee Type is required.")]
        public EmployeeType Type { get; set; }

        [Required(ErrorMessage = "Position is required.")]
        public Position Position { get; set; }
    }
}

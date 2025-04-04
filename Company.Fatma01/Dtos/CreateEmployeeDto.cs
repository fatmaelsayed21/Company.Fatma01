using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.PL.Dtos
{
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage = "Name is required !")]
        public string Name { get; set; }
        [Range(22,60, ErrorMessage ="Age must be between 22 and 60 years ")]
        public int? Age { get; set; }

        [RegularExpression(@"[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$"
                    ,ErrorMessage ="Address Must be like 123-street-city-country")]
        public string Address { get; set; }
        [DataType(DataType.EmailAddress , ErrorMessage ="Email is not valid")]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        [DisplayName("Hiring Date")]
        public DateTime HiringDate { get; set; }

        [DisplayName("Date Of Creation")]
        public DateTime CreateAt { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Framework;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Company.PL.Dtos
{
    public class CreateDepartmentDto
    {
        [Required(ErrorMessage="Code is required !")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name is required !")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Create At is required !")]
        public DateTime CreateAt { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RingoMediaApplication.Dtos
{
    public class ViewModelDepartment
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int? ParentDepartmentId { get; set; } 
    }
}

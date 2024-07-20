using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RingoMediaApplication.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; } 
        [Required]
        public string Name { get; set; }
        public string Logo { get; set; } 
        public int? ParentDepartmentId { get; set; } 
        public Department? ParentDepartment { get; set; }
        public List<Department>? SubDepartments { get; set; }
    }
}

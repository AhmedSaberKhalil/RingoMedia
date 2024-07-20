using System.ComponentModel.DataAnnotations;

namespace RingoMediaApplication.Models
{
    public class Reminder
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public bool IsSent { get; set; } 

    }
}

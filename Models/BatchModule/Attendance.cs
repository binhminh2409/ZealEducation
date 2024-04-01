using System.ComponentModel.DataAnnotations;
using ZealEducation.Models.Users;

namespace ZealEducation.Models.BatchModule
{
    public class Attendance
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please assign a session id")]
        public BatchSession? BatchSession { get; set; }

        [Required(ErrorMessage = "Please assign an user id")]
        public User? User { get; set; }

        [Required(ErrorMessage = "Please assign a date")]
        public DateTime? Date { get; set; }
    }
}

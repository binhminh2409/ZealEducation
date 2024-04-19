using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ZealEducation.Models.Users;

namespace ZealEducation.Models.BatchModule
{
    public class Attendance
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please assign a session id"), JsonIgnore]
        public BatchSession? BatchSession { get; set; }

        [Required(ErrorMessage = "Please assign an user id"), JsonIgnore]
        public UserInfo? UserInfo { get; set; }

        [Required(ErrorMessage = "Please assign a status")]
        public string? Status { get; set; }

        [Required(ErrorMessage = "Please assign a date")]
        public DateTime? Date { get; set; }
    }
}

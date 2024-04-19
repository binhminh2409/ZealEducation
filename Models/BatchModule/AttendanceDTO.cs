using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZealEducation.Models.BatchModule
{
    public class AttendanceDTO
    {

        [Required(ErrorMessage = "Please assign a session id")]
        public string? BatchSession { get; set; }

        [Required(ErrorMessage = "Please assign a status")]
        public string? Status { get; set; }

        [Required(ErrorMessage = "Please assign a date")]
        public DateTime? Date { get; set; }
    }
}

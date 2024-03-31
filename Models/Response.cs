using System.ComponentModel.DataAnnotations.Schema;

namespace ZealEducation.Models
{
    [NotMapped]
    public class Response
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
    }
}

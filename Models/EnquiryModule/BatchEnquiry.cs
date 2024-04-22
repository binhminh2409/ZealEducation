using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZealEducation.Models.EnquiryModule
{
    public class BatchEnquiry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }

        [Required]public string BatchId { get; set; }

        [Required]public string Description { get; set; }

        [Required]public DateTime CreatedDate { get; set; }
    }
}

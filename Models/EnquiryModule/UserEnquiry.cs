using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZealEducation.Models.EnquiryModule
{
    public class UserEnquiry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }

        [Required] public string UserInfoId { get; set; }

        [Required] public string Description { get; set; }
    }
}

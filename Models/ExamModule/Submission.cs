using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ZealEducation.Models.Users;

namespace ZealEducation.Models.ExamModule
{
    public class Submission
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Please select a user"), JsonIgnore]
        public UserInfo? UserInfo { get; set; }

        [Required(ErrorMessage = "Please assign an exam"), JsonIgnore]
        public Exam? Exam { get; set; }

        [Required(ErrorMessage = "Please submit the file")]
        public string? FileName { get; set; }

        [Required(ErrorMessage = "Please enter the submission date")]
        public DateTime? SubmitDateTime { get; set; }

        public bool? Passed { get; set; }

        public int? Score { get; set; }

        public DateTime? MarkedDateTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZealEducation.Models.Users;

namespace ZealEducation.Models.ExamModule
{
    public class Submission
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please select a user")]
        public User? User { get; set; }

        [Required(ErrorMessage = "Please assign an exam")]
        public Exam? Exam { get; set; }

        [Required(ErrorMessage = "Please submit the file")]
        public string? FilePath { get; set; }

        [Required(ErrorMessage = "Please enter the submission date")]
        public DateTime? DateTime { get; set; }

        public ICollection<Result>? Results { get; set; }
    }
}

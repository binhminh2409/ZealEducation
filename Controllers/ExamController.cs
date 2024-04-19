
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZealEducation.Models;
using ZealEducation.Models.CourseModule;
using ZealEducation.Models.ExamModule;
using ZealEducation.Utils;

namespace ZealEducation.Controllers
{
    [Route("api/exam")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly IWebHostEnvironment _env;

        public ExamController(ApplicationDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllExams()
        {
            var exams = await _dbContext.Exams.ToListAsync();
            return Ok(exams);
        }

        [HttpGet]
        [Route("all/{batchId}")]
        public async Task<IActionResult> GetAllExamsOfBatch([FromRoute] string batchId)
        {
            var batch = await _dbContext.Batch
                .Include(b => b.Exams)
                .FirstOrDefaultAsync(b => b.Id == batchId);
            return Ok(batch);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> GetAllExamsOfBatch([FromBody] ExamDTO examDTO)
        {
            var batch = await _dbContext.Batch.FindAsync(examDTO.batchId);
            if (batch == null) { return NotFound("Course not found"); }
            Exam exam = new() { 
                Id = IdCreator.HashId(examDTO.batchId, "EX"),
                Batch = batch,
                StartDate = examDTO.StartDate,
                EndDate = examDTO.EndDate,
                Name = examDTO.Name,
                ScoreToPass = examDTO.ScoreToPass,
            };
            await _dbContext.AddAsync(exam);
            await _dbContext.SaveChangesAsync();

            return Created("Exam created",exam);
        }

        [HttpDelete]
        [Route("delete/{examId}")]
        public async Task<IActionResult> DeleteExam([FromRoute] string examId)
        {
            var exam = await _dbContext.Exams.FindAsync(examId);
            if (exam == null) { return NotFound(); }

            _dbContext.Exams.Remove(exam);
            await _dbContext.SaveChangesAsync();

            return Ok("Exam removed");
        }

        [HttpPut]
        [Route("update/{examId}")]
        public async Task<IActionResult> UpdateExam([FromBody] ExamDTO examDTO, [FromRoute] string examId)
        {
            // Check exam exist
            var exam = await _dbContext.Exams.FindAsync(examId);
            if (exam == null) { return NotFound(); }

            // Update exam
            exam.StartDate = examDTO.StartDate;
            exam.EndDate = examDTO.EndDate;
            exam.ScoreToPass = examDTO.ScoreToPass;

            await _dbContext.SaveChangesAsync();
            return Ok(exam);
        }

        [HttpPost]
        [Route("submit/{examId}")]
        public async Task<IActionResult> Submit(IFormFile file, [FromRoute] string examId)
        {
            // Check file validity
            if (file == null || file.Length <= 0) { return BadRequest("No file submitted"); }

            // Check exam id validity\
            var exam = await _dbContext.Exams.FindAsync(examId);
            if (exam == null) { return BadRequest("Exam doesn't exist"); }

            // Get user information
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) { return Unauthorized(); }
            var user = await _dbContext.Users.Include(u => u.UserInfo).FirstOrDefaultAsync(u => u.Id == userIdClaim.Value);
            var userInfo = user.UserInfo;
            string username = user.UserName;

            // Create unique file name 
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            string submissionFileName = IdCreator.ConcatenateId(username, "ex", examId) + fileExtension;


            // Store file in the server
            var folderPath = Path.Combine(_env.ContentRootPath, "Files", "Submissions");
            Directory.CreateDirectory(folderPath); // Create the folder if it doesn't exist
            // Create file Path with unique file name
            var filePath = Path.Combine(folderPath, submissionFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Create submission in DB
            Submission submission = new()
            {
                SubmitDateTime = DateTime.Now,
                Exam = exam,
                FileName = submissionFileName,
                Id = IdCreator.ConcatenateId(examId, username),
                UserInfo = userInfo,
                MarkedDateTime = null,
                Passed = null,
                Score = null
            };

            _dbContext.Submission.Add(submission);
            await _dbContext.SaveChangesAsync();    

            return Created($"Exam submitted successfully", submission);
        }

        [HttpPut]
        [Route("mark-submission")]
        public async Task<IActionResult> MarkSubmission([FromBody] MarkedSubmissionDTO markedSubmissionDTO)
        {
            var submission = await _dbContext.Submission
                .Include(s => s.Exam)
                .FirstOrDefaultAsync(s => s.Id == markedSubmissionDTO.SubmissionId);
            if (submission == null) { return NotFound("Submission not found"); }

            // Update marks of the submission
            submission.MarkedDateTime = DateTime.Now;
            submission.Score = markedSubmissionDTO.Score;
            submission.Passed = (submission.Exam!.ScoreToPass < submission.Score);

            await _dbContext.SaveChangesAsync();
            return Ok(submission);
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using ZealEducation.Models.BatchModule;
using ZealEducation.Models.CandidateModule;
using ZealEducation.Models.CourseModule;
using ZealEducation.Models.ExamModule;
using ZealEducation.Models.Users;

namespace ZealEducation.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserInfo> UserInfo { get; set; }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<Batch> Batch { get; set; }

        public DbSet<BatchSession> BatchSession { get; set; }

        public DbSet<Attendance> Attendance { get; set; }

        public DbSet<Course> Course { get; set; }

        public DbSet<CourseSession> CourseSession { get; set; }

        public DbSet<Resource> Resource { get; set; }

        public DbSet<Enrollment> Enrollment { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasOne(u => u.UserInfo)
                .WithOne(ui => ui.User)
                .HasForeignKey<UserInfo>(ui => ui.Id);
            SeedRoles(builder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
                (
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Name = "Faculty", ConcurrencyStamp = "2", NormalizedName = "Faculty" },
                new IdentityRole() { Name = "Candidate", ConcurrencyStamp = "3", NormalizedName = "Candidate" }
                );
        }
    }
}

using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {
        }

        public StudentSystemContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Course> Courses { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourse> StudentsCourses { get; set; }

        private const string ConnectionString =
                            @"Server=DESKTOP-9FL9J1C;Database=Bet368;Integrated Security=True;Encrypt=False";

       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(s => s.Name).IsUnicode(true);
                entity.Property(s => s.PhoneNumber).IsUnicode(false);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(s => s.Name).IsUnicode();
                entity.Property(s=>s.Description).IsUnicode();
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.Property(s => s.Name).IsUnicode();
                entity.Property(s => s.Url).IsUnicode(false);
            });

            modelBuilder.Entity<Homework>(entity =>
            {
                entity.Property(h => h.Content).IsUnicode(false);

            });
            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(sc => new { sc.StudentId, sc.CourseId });
            });

        }
    }
}
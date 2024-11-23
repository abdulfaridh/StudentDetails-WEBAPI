using Microsoft.EntityFrameworkCore;
using StudentDetails.Data.Configuration;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace StudentDetails.Data
{
    public class StudentDBContext : DbContext
    {
        public StudentDBContext(DbContextOptions<StudentDBContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.ApplyConfiguration(new StudentConfig()); // Foe Students Table 
        }
    }
}

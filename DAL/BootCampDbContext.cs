using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BootCampApp.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BootCampApp.DAL
{
    public class BootCampDbContext : DbContext
    {
        //DAL -  Data Access Layer
        public BootCampDbContext() : base("BootCampContext")    //connstring
        { }
        
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Person> Persons { get; set; }  //Table per Inheritance   


        
        protected override void OnModelCreating(DbModelBuilder modelBuilder) 
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
                        
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors)
                .WithMany(i => i.Courses)
                .Map(t => t.MapLeftKey("CourseID")  
                .MapRightKey("InstructorID")
                .ToTable("CourseInstructor"));

            modelBuilder.Entity<Department>().MapToStoredProcedures();  

        }
        
    }
}
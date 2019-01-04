using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BootCampApp.Models;

namespace BootCampApp.DAL
{
   
    public class BootCampInitializer : DropCreateDatabaseIfModelChanges<BootCampDbContext>
    {
        protected override void Seed(BootCampDbContext context)
        {
            var students = new List<Student>
            {
                new Student
                {
                    FirstName = "John",
                    LastName = "Karastatiras",
                    EnrollmentDate = DateTime.Parse("2018-09-01")
                },
                new Student
                {
                    FirstName = "Nick",
                    LastName = "Gkalis",
                    EnrollmentDate = DateTime.Parse("2018-09-01")
                },
                new Student
                {
                    FirstName = "Michael",
                    LastName = "Schumacher",
                    EnrollmentDate = DateTime.Parse("2018-10-01")
                },
                new Student
                {
                    FirstName = "Ayrton",
                    LastName = "Senna",
                    EnrollmentDate = DateTime.Parse("2018-11-01")
                },
                new Student
                {
                    FirstName = "Giannis",
                    LastName = "Antentokoumpo",
                    EnrollmentDate = DateTime.Parse("2018-04-01")
                },
                new Student
                {
                    FirstName = "Lebron",
                    LastName = "James",
                    EnrollmentDate = DateTime.Parse("2018-06-01")
                },
                new Student
                {
                    FirstName = "Vassilis",
                    LastName = "Spanoulis",
                    EnrollmentDate = DateTime.Parse("2018-02-01")
                }
            };            
            students.ForEach(s => context.Students.Add(s));           
            context.SaveChanges();

            var courses = new List<Course>
            {
                new Course
                {
                    CourseID = 1050,
                    Title = "OOP",
                    Hours = 654
                },
                new Course
                {
                    CourseID = 1070,
                    Title = "C#",
                    Hours = 876
                },
                new Course
                {
                    CourseID = 1080,
                    Title = ".NET",
                    Hours = 690
                },
                new Course
                {
                    CourseID = 1090,
                    Title = "Angular",
                    Hours = 654
                },
                new Course
                {
                    CourseID = 1100,
                    Title = "CSS",
                    Hours = 654
                },
                new Course
                {
                    CourseID = 1110,
                    Title = "HTML",
                    Hours = 654
                },
                new Course
                {
                    CourseID = 1120,
                    Title = "Jvascript",
                    Hours = 654
                }
            };
            
            courses.ForEach(c => context.Courses.Add(c));
            context.SaveChanges();

            var enrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    CourseID = 1050,
                    StudentID = 1,
                    Grade = Grade.A
                },
                new Enrollment
                {
                    CourseID = 1070,
                    StudentID = 1,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    CourseID = 1080,
                    StudentID = 1,
                    Grade = Grade.A
                },
                new Enrollment
                {
                    CourseID = 1050,
                    StudentID = 2,
                    Grade = Grade.C
                },
                new Enrollment
                {
                    CourseID = 1050,
                    StudentID = 3,
                    Grade = Grade.D
                },
                new Enrollment
                {
                    CourseID = 1100,
                    StudentID = 3,
                    Grade = Grade.D
                },
                new Enrollment
                {
                    CourseID = 1100,
                    StudentID = 2,
                    Grade = Grade.D
                },
                new Enrollment
                {
                    CourseID = 1100,
                    StudentID = 1,
                    Grade = Grade.A
                },
                new Enrollment
                {
                    CourseID = 1110,
                    StudentID = 3,
                    
                },
                new Enrollment
                {
                    CourseID = 1120,
                    StudentID = 3,
                    Grade = Grade.A
                },
            };

            enrollments.ForEach(e => context.Enrollments.Add(e));
            context.SaveChanges();
        }
    }
}
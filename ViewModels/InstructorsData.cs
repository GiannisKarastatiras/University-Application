using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BootCampApp.Models;

namespace BootCampApp.ViewModels
{
    public class InstructorsData    
    {
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }

    }
}
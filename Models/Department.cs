using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BootCampApp.Models
{
    public class Department 
    {
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }        
        public int? InstructorID { get; set; }  

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Navigation Properties
        //1-N 1 department has many Courses
        public virtual ICollection<Course> Courses { get; set; }

        //one-to-zero-or-one because instructor id is nullable
        public virtual Instructor Administrator { get; set; }
    }
}
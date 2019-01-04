using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BootCampApp.Models
{
    public enum Grade
    {
        A, B, C, D, E, F
    }

    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }           
        public int StudentID { get; set; }

        [DisplayFormat(NullDisplayText = "No Grade")] 
        public Grade? Grade { get; set; }   //?---nullable        

        //Navigation Properties
        public virtual Student Student { get; set; }    
        public virtual Course Course { get; set; }      
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootCampApp.Models
{
    //one-to-zero-or-one
    //1-0 relationship---dependent with instructor----
    public class OfficeAssignment 
    {
        [Key] 
        [ForeignKey("Instructor")] 
        public int InstructorID { get; set; }

        public string Location { get; set; }

        //Navigation Property
        public virtual Instructor Instructor { get; set; }
    }
}
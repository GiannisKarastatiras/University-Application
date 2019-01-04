using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace BootCampApp.Models
{
    public class Student : Person
    {               

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")] 
        public DateTime EnrollmentDate { get; set; }    //Date time is struct---value type---by default non-null
                                                       //string reference type nullable
                

        // Navigation Property (gia na kanei navigate se mia class)
        //virtual = lazy-loading        
        public virtual ICollection<Enrollment> Enrollments { get; set; }

    }
}
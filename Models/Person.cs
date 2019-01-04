using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BootCampApp.Models
{
    public class Person
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "LastName must be up to 50 characters")]   
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "FirstName must be up to 50 characters")]
        public string FirstName { get; set; }

        [Display(Name = "Full Name")] 
        public string FullName 
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }
    }
}
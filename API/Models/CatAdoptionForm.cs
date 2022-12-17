using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class CatAdoptionForm
    {
        public Guid Id { get; set; }    

        public FormStatus FormStatus {get; set; }

        [Required(ErrorMessage = "Please enter your name")]
        public string OwnerName {get; set;}

        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string OwnerEmail {get; set;}

        [Required(ErrorMessage = "Please select a cat")]
        public Cat Cat {get; set;}

    }
}
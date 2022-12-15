using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class CatPutUpForAdoptionForm
    {
        public Guid Id { get; set; }    

        public FormStatus FormStatus { get; set; }

        public string OwnerName { get; set; }   

        public string OwnerEmail {get; set; }

        public Cat Cat { get; set; }
    }
}
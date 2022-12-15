using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Cat
    {
        public Guid Id { get; set; }

        public CatStatus CatStatus {get; set;}

        public string Name {get; set;}

        public string Description {get; set;}
    }
}
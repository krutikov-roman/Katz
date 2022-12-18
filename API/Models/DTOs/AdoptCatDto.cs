using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.DTOs
{
    public class AdoptCatDto
    {
        // OwnerName: name,
        // OwnerEmail: email,
        // Cat: catId

        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }
        public Guid CatId { get; set; }
    }
}
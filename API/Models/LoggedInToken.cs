using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class LoggedInToken
    {
        public Guid Id { get; set; }

        public string NameIdentifier { get; set; }

        public string IpAddress {get; set;}

        public string HashedToken { get; set; }
    }
}
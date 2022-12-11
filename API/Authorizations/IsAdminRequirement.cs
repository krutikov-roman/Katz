using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace API.Authorizations
{
    // https://www.red-gate.com/simple-talk/development/dotnet-development/policy-based-authorization-in-asp-net-core-a-deep-dive/
    public class IsAdminRequirement : IAuthorizationRequirement
    {
        public bool _isAdmin { get; set; }
        public IsAdminRequirement(bool isAdmin)
        {
            _isAdmin = isAdmin;
        } 
    }
}
/*

By: Roman Krutikov

Description: This class is used alongside the IsAdminRequirement class is used to create an 
             authorization policy that would only allow Admin Users to access certain API endpoints.
             
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace API.Authorizations 
{
    // https://www.red-gate.com/simple-talk/development/dotnet-development/policy-based-authorization-in-asp-net-core-a-deep-dive/
    public class IsAdminAuthorizationHandler : AuthorizationHandler<IsAdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAdminRequirement requirement)
        {
            var claim = context.User.FindFirst("IsAdmin");
            if(claim != null)
            {
                var isAdminValue = (claim?.Value).ToString().Equals("True");
                if (isAdminValue){
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
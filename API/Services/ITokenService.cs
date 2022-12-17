/*

By: Roman Krutikov

Description: This interface is used by the TokenService and allows mocking the TokenService during testing.
             
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Services
{
    public interface ITokenService
    {
        public string CreateToken(AppUser user, string ipAddress);

        public void RevokeToken(string token);

        public bool isTokenActive(string token);

        public string ComputeSha256Hash(string rawData);
    }
}
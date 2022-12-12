/*

By: Roman Krutikov

Description: This class is used for issuing bearer tokens to users, as well as handling 
             hashing these tokens and handling logging in and logging out of the application.
             
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using API.Models;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace API.Services
{
    public class TokenService
    {
        private static readonly string KEY = "Katz Veryz Securez Keyz 123z";
        private DatabaseIdentities _databaseIdentities;

        public TokenService (DatabaseIdentities databaseIdentities){
            _databaseIdentities = databaseIdentities; 
        }

        public string CreateToken(AppUser user, string ipAddress)
        {
            var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("IsAdmin", user.IsAdmin.ToString()),
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            string tokenWritten = tokenHandler.WriteToken(token);
            
            string tokenHashed = ComputeSha256Hash(tokenWritten);
            LoggedInToken? databaseToken = _databaseIdentities.LoggedInTokens.FirstOrDefault(x => x.NameIdentifier.Equals(user.Id) && x.IpAddress.Equals(ipAddress));
            if (databaseToken == null){
                databaseToken = new LoggedInToken {
                    NameIdentifier = user.Id,
                    IpAddress = ipAddress,
                    HashedToken = tokenHashed
                };
                _databaseIdentities.LoggedInTokens.Add(databaseToken);
            }
            else {
                databaseToken.HashedToken = tokenHashed;
            }
            _databaseIdentities.SaveChanges();

            return tokenWritten;
        }

        public void RevokeToken(string token){
            string tokenHashed = ComputeSha256Hash(token.ToString());
            LoggedInToken? databaseToken = _databaseIdentities.LoggedInTokens.FirstOrDefault(x => x.HashedToken.Equals(tokenHashed));
            if (databaseToken != null){
                _databaseIdentities.LoggedInTokens.Remove(databaseToken);
                _databaseIdentities.SaveChanges();
            }
        }

        public bool isTokenActive(string token){
            if (string.IsNullOrEmpty(token)){
                return false;
            }
            if (token.StartsWith("Bearer ")){
                token = token.Substring(7);
            }
            string tokenHashed = ComputeSha256Hash(token);
            LoggedInToken? possibleToken = _databaseIdentities.LoggedInTokens.FirstOrDefault(x => x.HashedToken.Equals(tokenHashed));
            return (possibleToken != null);
        }

        // https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/
        private string ComputeSha256Hash(string rawData)  
        {    
            using (SHA256 sha256Hash = SHA256.Create())  
            {  
                string salt = KEY;
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData + salt));  
  
                StringBuilder builder = new StringBuilder();  
                for (int i = 0; i < bytes.Length; i++)  
                {  
                    builder.Append(bytes[i].ToString("x2"));  
                }  
                return builder.ToString();  
            }  
        } 
    }
}
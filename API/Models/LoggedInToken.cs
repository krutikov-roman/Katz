/*

By: Roman Krutikov

Description: This class is used for handling signing in and signing out. This is done by keeping a table 
             of these objects in a database, and only allowing users that match the hashedtoken to access 
             the authenticated pages without needing to type their username or password again. When a user signs 
             in, a copy of this object with the user's info is stored on the database, and anytime that
             user tries to use their saved token, it will be compared here to ensure the user has
             not yet signed out. When a user signs out, this type of entry would be removed from the table, 
             signifying that they can no longer use the previous token to access pages that require authentication.
             
*/
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
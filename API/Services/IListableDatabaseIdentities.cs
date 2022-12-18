/*

By: Roman Krutikov

Description: This interface is used by the DatabaseIdentities database to interact with the various tables through
             methods which can be mocked during testing.
             
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Services
{
    public interface IListableDatabaseIdentities
    {
        public List<LoggedInToken> GetListOfLoggedInTokens();

        public void AddTokenToLoggedInTokens(LoggedInToken token);

        public void RemoveTokenFromLoggedInTokens(LoggedInToken token);
    }
}
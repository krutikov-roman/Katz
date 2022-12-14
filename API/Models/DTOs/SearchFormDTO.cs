/*

By: Roman Krutikov

Description: This class is used as a template for API endpoints that accept
             or deny cat adoption forms or cat put up for adoption forms. Only
             an Id is needed, since all searching for if the form's information
             is valid can be done by querying just the Id of the form.
             
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.DTOs
{
    public class SearchFormDTO
    {
        public Guid Id { get; set; }
    }
}
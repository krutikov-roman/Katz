/*

By: Roman Krutikov

Description: This enum is used to define the state a cat object is in
             throughout the adoption process.
             
*/
namespace API.Models
{
    public enum CatStatus
    {
        New, WaitingForAdoption, Denied, Adopted
    }
}
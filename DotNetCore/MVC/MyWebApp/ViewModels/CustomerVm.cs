using Microsoft.AspNetCore.Mvc;
using MyWebApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyWebApp.ViewModels
{
    public class CustomerVm
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderType Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Mobile { get; set; }
    }
}


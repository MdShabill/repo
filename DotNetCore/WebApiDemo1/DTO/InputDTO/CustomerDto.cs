﻿using WebApiDemo1.DataModel;
using WebApiDemo1.Enums;

namespace WebApiDemo1.DTO.InputDTO
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public GenderTypes Gender { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public DateTime? LastFailedLoginDate { get; set; }
        public DateTime? LastSuccessfulLoginDate { get; set; }
        public int? LoginFailedCount { get; set; }
        public bool? IsLocked { get; set; }
        public byte[]? HashValuePassword { get; set; }

        public List<AddressDto> Addresses { get; set; }

        public CustomerDto()
        {

        }

        public CustomerDto(int id, string fullName, GenderTypes gender, int age, string email, 
            string country, string password, string mobileNumber)
        {
            FullName = fullName;
            Gender = gender;
            Age = age;
            Email = email;
            Password = password;
            MobileNumber = mobileNumber;
        }
    }
}
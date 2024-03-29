﻿using WebApiDemo1.Enums;

namespace WebApiDemo1.DataModel
{
    public class Customer
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public GenderTypes Gender { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? MobileNumber { get; set; }
        public DateTime? LastFailedLoginDate { get; set; }
        public DateTime? LastSuccessfulLoginDate { get; set; }
        public int? LoginFailedCount { get; set; }
        public bool? IsLocked { get; set; }
        public byte[]? HashValuePassword { get; set; }
    }
}

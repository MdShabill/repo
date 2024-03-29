﻿using ShopEase.Enums;

namespace ShopEase.DataModels.Customer
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public GenderType Gender { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public DateTime LastSuccessFulLoginDate { get; set; }
        public DateTime LastFailedLoginDate { get; set; }
        public int? LoginFailedCount { get; set; }
        public bool IsLocked { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int PinCode { get; set; }
        public int AddressTypeId { get; set; }
        public string AddressTypeName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}

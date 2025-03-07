﻿namespace ConstructionApplication.Core.DataModels.Address
{
    public class Address
    {
        public int Id { get; set; }
        public int ContractorId { get; set; }
        public int AddressTypeId { get; set; }
        public int CountryId { get; set; }
        public string AddressLine1 { get; set; }
        public int PinCode { get; set; }

        //Parameterized Constructor
        public Address(int contractorId, string addressLine1, int addressTypeId, int countryId, int pinCode)
        {
            ContractorId = contractorId;
            AddressLine1 = addressLine1;
            AddressTypeId = addressTypeId;
            CountryId = countryId;
            PinCode = pinCode;
        }

        //Default Constructor
        public Address() { }
    }
}

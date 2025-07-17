namespace ConstructionApplication.Core.DataModels.Address
{
    public class Address
    {
        public int Id { get; set; }
        public int ServiceProviderId { get; set; }
        public int AddressTypeId { get; set; }
        public int CountryId { get; set; }
        public string AddressLine1 { get; set; }
        public int PinCode { get; set; }
        public int SiteId { get; set; }

        //Parameterized Constructor
        public Address(int serviceProviderId, string addressLine1, int addressTypeId, int countryId, int pinCode, int siteId)
        {
            ServiceProviderId = serviceProviderId;
            AddressLine1 = addressLine1;
            AddressTypeId = addressTypeId;
            CountryId = countryId;
            PinCode = pinCode;
            SiteId = siteId;
        }

        //Default Constructor
        public Address() { }
    }
}

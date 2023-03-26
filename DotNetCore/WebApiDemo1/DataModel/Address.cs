using WebApiDemo1.Enums;

namespace WebApiDemo1.DataModel
{
    public class Address
    {
        public int CustomerId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int PinCode { get; set; }
        public string Country { get; set; }
        public AddressTypes AddressType { get; set; }
    }
}

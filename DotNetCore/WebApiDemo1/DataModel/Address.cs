using WebApiDemo1.Enums;

namespace WebApiDemo1.DataModel
{
    public class Address
    {
        public int? CustomerId { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public int? PinCode { get; set; }
        public string? Country { get; set; }
        public AddressType? AddressType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? LastEditedOn { get; set; }
    }
}

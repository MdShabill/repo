using ConstructionApplication.Core.Enums;

namespace ConstructionApplication.ViewModels.ContractorVm
{
    public class ContractorVm
    {
        public int ContractorId { get; set; }
        public string ContractorName { get; set; }
        public int JobCategoryId { get; set; }
        public string JobTypes { get; set; }
        public GenderTypes Gender { get; set; }
        public DateTime? DOB { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string MobileNumber { get; set; }
        public string ReferredBy { get; set; }

        public int? AddressTypeId { get; set; }
        public string AddressTypes { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public string? AddressLine1 { get; set; }
        public int? PinCode { get; set; }
    }
}

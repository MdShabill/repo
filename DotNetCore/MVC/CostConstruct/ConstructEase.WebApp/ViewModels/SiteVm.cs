using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConstructEase.WebApp.ViewModels
{
    public class SiteVm
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public string Name { get; set; }
        public DateTime StartedDate { get; set; }
        public int SiteStatusId { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }

        public int ServiceProviderId { get; set; }
        public string DisplayName { get; set; }
        public string AddressLine1 { get; set; }
        public int? AddressTypeId { get; set; }
        public string AddressTypes { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public int? PinCode { get; set; }

        public List<int> SelectedMasterMasonIds { get; set; } = new();
        public MultiSelectList? MasterMasons { get; set; }

        public List<int> SelectedElectricianIds { get; set; } = new List<int>();
        public MultiSelectList? Electrician { get; set; }
    }
}

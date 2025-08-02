using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConstructionApplication.ViewModels
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
        public MultiSelectList? Electricians { get; set; }

        public List<int> SelectedLabourIds { get; set; } = new List<int>();
        public MultiSelectList? Labours { get; set; }

        public List<int> SelectedPlumberIds { get; set; } = new List<int>();
        public MultiSelectList? Plumbers { get; set; }

        public List<int> SelectedPainterIds { get; set; } = new List<int>();
        public MultiSelectList? Painters { get; set; }

        public List<int> SelectedCarpenterIds { get; set; } = new List<int>();
        public MultiSelectList? Carpenters { get; set; }

        public List<int> SelectedTilerIds { get; set; } = new List<int>();
        public MultiSelectList? Tilers { get; set; }
    }
}

namespace ConstructEase.WebApp.APIControllers.APIViewModels
{
    public class SiteAPIDTO
    {
        public string Name { get; set; }

        public DateTime StartedDate { get; set; }

        public int SiteStatusId { get; set; }

        public string Note { get; set; }

        public string? AddressLine1 { get; set; }

        public int? AddressTypeId { get; set; }

        public int? CountryId { get; set; }

        public int? PinCode { get; set; }

        public List<int> SelectedMasterMasonIds { get; set; } = new();

        public List<int> SelectedElectricianIds { get; set; } = new();

        public List<int> SelectedLabourIds { get; set; } = new();

        public List<int> SelectedPlumberIds { get; set; } = new();

        public List<int> SelectedPainterIds { get; set; } = new();

        public List<int> SelectedCarpenterIds { get; set; } = new();

        public List<int> SelectedTilerIds { get; set; } = new();
    }
}

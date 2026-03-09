namespace ConstructEase.WebApp.APIControllers.APIViewModels
{
    public class SiteAPIVm
    {
        public int Id { get; set; }
        //public int SiteId { get; set; }
        public string Name { get; set; }
        public DateTime StartedDate { get; set; }
        //public int SiteStatusId { get; set; }
        public string Status { get; set; }
        //public string Note { get; set; }

        //public int? ServiceProviderId { get; set; }
        public string? DisplayName { get; set; }
        public string? AddressLine1 { get; set; }
        //public int? AddressTypeId { get; set; }
        public string? AddressTypes { get; set; }
        //public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? PinCode { get; set; }

        public List<int> LabourIds { get; set; }
        public List<int> MasterMasonIds { get; set; }
        public List<int> ElectricianIds { get; set; }
        public List<int> PlumberIds { get; set; }
        public List<int> PainterIds { get; set; }
        public List<int> CarpenterIds { get; set; }
        public List<int> TilerIds { get; set; }
    }
}

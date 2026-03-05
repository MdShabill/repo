namespace ConstructEase.WebApp.APIControllers.APIViewModels
{
    public class DropdownItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class SiteDropdownDTO
    {
        public List<DropdownItemDTO> Statuses { get; set; } = new();
        public List<DropdownItemDTO> AddressTypes { get; set; } = new();
        public List<DropdownItemDTO> Countries { get; set; } = new();
    }
}
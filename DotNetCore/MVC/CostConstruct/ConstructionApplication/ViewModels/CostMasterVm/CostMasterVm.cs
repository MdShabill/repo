namespace ConstructionApplication.ViewModels.CostMasterVm
{
    public class CostMasterVm
    {
        public int Id { get; set; }
        public int JobCategoryId { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }
}

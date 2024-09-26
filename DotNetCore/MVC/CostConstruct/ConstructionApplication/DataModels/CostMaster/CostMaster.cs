namespace ConstructionApplication.DataModels.CostMaster
{
    public class CostMaster
    {
        public int Id { get; set; }
        public decimal MasterMasonCost { get; set; }
        public decimal LabourCost { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }
}

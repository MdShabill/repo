namespace ConstructionApplication.Core.DataModels.CostMaster
{
    public class CostMaster
    {
        public int Id { get; set; }
        public int ServiceTypeId { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public DateTime Date { get; set; }
    }
}

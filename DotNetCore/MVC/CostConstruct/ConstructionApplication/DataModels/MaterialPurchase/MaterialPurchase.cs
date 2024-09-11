namespace ConstructionApplication.DataModels.MaterialPurchase
{
    public class MaterialPurchase
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public DateTime Date {  get; set; }
        public decimal MaterialCost { get; set; }
        public decimal DeliveryCharge { get; set; }
    }
}

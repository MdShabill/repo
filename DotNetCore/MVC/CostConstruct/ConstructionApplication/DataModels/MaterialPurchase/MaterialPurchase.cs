namespace ConstructionApplication.DataModels.MaterialPurchase
{
    public class MaterialPurchase
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }
        public int SupplirId { get; set; }
        public string SupplirName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime Date {  get; set; }
        public decimal MaterialCost { get; set; }
        public decimal DeliveryCharge { get; set; }
    }
}

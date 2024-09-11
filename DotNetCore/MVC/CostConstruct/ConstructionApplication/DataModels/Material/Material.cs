namespace ConstructionApplication.DataModels.Material
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int SupplierId {  get; set; }
        public string SupplierName {  get; set; }
    }
}

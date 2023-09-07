namespace UploadFile.ViewModels
{
    public class ProductImageVm
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public IFormFile ImageName { get; set; }
    }
}

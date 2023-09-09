namespace UploadFile.ViewModels
{
    public class ProductImageVm
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        
		public IFormFile ImageFile { get; set; }

		public string ImageName { get; set; }

	}
}

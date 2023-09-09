using UploadFile.Models;

namespace UploadFile.Repository
{
    public interface IUploadRepository
    {
        public int AddImage(ProductImage productImage);
        public ProductImage GetImageById(int id);
    }
}

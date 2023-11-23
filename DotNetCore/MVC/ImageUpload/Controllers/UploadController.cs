using AutoMapper;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using UploadFile.Models;
using UploadFile.Repository;
using UploadFile.ViewModels;

namespace UploadFile.Controllers
{
    public class UploadController : Controller
    {
        IUploadRepository _uploadRepository;
        private readonly IWebHostEnvironment _env;
        IMapper _imapper;

        public UploadController(IWebHostEnvironment env, IUploadRepository uploadRepository)
        {
            _env = env;
            _uploadRepository = uploadRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductImageVm, ProductImage>();
                cfg.CreateMap<ProductImage, ProductImageVm>();
            });

            _imapper = configuration.CreateMapper();
        }



		public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductImageVm productImageVm)
        {
			//upload image in folder
			
            string dir = Path.Combine(_env.WebRootPath, "UploadedFiles");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

			string uniqueFileName = GetUniqueFileName(productImageVm.ImageFile.FileName);
			string filePath = Path.Combine(dir, uniqueFileName);
            
            await productImageVm.ImageFile.CopyToAsync(new FileStream(filePath, FileMode.Create));

            // now call repo method to insert data in table
            
            ProductImage productImage = _imapper.Map<ProductImageVm, ProductImage>(productImageVm);
            productImage.ImageName = uniqueFileName;
            int affectedRowsCount = _uploadRepository.Add(productImage);
            if (affectedRowsCount > 0)
            {

            }

            return View();
		}

        //private void AddImage(ProductImageVm productImageVm, string uniqueFileName)
        //{
        //    ProductImage productImage = _imapper.Map<ProductImageVm, ProductImage>(productImageVm);
        //    productImage.ImageName = uniqueFileName;
        //    int affectedRowsCount = _uploadRepository.AddImage(productImage);
        //    if (affectedRowsCount > 0)
        //    {

        //    }
        //}

        private string GetUniqueFileName(string fileName)
        {                                   
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }

        public IActionResult View(int id)
        {
            ProductImage productImage = _uploadRepository.GetImageById(id);

            ProductImageVm productImageVm = _imapper.Map<ProductImage, ProductImageVm>(productImage);

            return View(productImageVm);
        }
    }
}

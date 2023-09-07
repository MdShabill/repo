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
        private readonly IHostEnvironment _env;
        IMapper _imapper;

        public UploadController(IHostEnvironment env, IUploadRepository uploadRepository)
        {
            _env = env;
            _uploadRepository = uploadRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductImageVm, ProductImage>();
            });

            _imapper = configuration.CreateMapper();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProductImageVm productImageVm)
        {
            //upload image in folder
            string uniqueFileName = GetUniqueFileName(productImageVm.ImageName.FileName);
            string dir = Path.Combine(_env.ContentRootPath, "UploadFile.Demo");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            
            string filePath = Path.Combine(dir, uniqueFileName);
            
            await productImageVm.ImageName.CopyToAsync(new FileStream(filePath, FileMode.Create));

            // now call repo method to insert data in table
            ProductImage productImage = _imapper.Map<ProductImageVm, ProductImage>(productImageVm);
            productImage.ImageName = uniqueFileName;
            int affectedRowsCount = _uploadRepository.AddImage(productImage);
            if(affectedRowsCount > 0)
            {

            }
            return View();
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }
    }
}

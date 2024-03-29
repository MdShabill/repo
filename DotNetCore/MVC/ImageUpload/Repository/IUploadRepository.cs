﻿using UploadFile.Models;

namespace UploadFile.Repository
{
    public interface IUploadRepository
    {
        public int Add(ProductImage productImage);
        public ProductImage GetImageById(int id);
    }
}

﻿namespace ShopEase.ViewModels
{
    public class ProductVm
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Title { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal ActualPrice { get; set; }
        //public decimal ActualPrice
        //{
        //    get 
        //    { 
        //        return Price - Discount; 
        //    }
        //}
        public string CategoryName { get; set; }
        public string SupplierName { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImageName { get; set; }
        public int Quantity { get; set; }
    }
}

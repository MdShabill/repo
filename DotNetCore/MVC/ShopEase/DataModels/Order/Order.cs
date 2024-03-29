﻿using ShopEase.DataModels.OderItem;

namespace ShopEase.DataModels.Order
{
    public class Order
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int PinCode { get; set; }
        public string CountryName { get; set; }
        public string AddressTypeName { get; set; }

        public OrderItem OrderItem { get; set; }
    }
}

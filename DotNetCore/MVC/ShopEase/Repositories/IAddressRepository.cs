﻿using ShopEase.DataModels;

namespace ShopEase.Repositories
{
    public interface IAddressRepository
    {
        public List<Address> GetAllAddress();
    }
}
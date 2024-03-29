﻿using ShopEase.DataModels.Customer;

namespace ShopEase.Repositories
{
    public interface ICustomerRepository
    {
        public List<Customer> GetAll();   
        public Customer GetCustomerById(int id);
        public int Register(Customer customer);
        public int Update(Customer customer);

        public Customer GetCustomerDetailByEmail(string email);
        public void UpdateOnLoginSuccessfull(string email);
        public void UpdateOnLoginFailed(string email);
        public void UpdateIsLocked(string email, bool isLocked = true);
    }
}

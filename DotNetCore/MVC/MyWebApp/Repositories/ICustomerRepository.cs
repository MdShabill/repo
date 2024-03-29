﻿using MyWebApp.DataModel;
using MyWebApp.Enums;
using MyWebApp.ViewModels;

namespace MyWebApp.Repositories
{
    public interface ICustomerRepository
    {
        public List<Customer> GetAll();
        public Customer Get(int id);
        public int Delete(int id);
        //public List<Customer> GetCustomers(string firstName, string lastName, int gender);
        
        //public List<Customer> GetCustomers(CustomerSearchVm vmFilter);
        
        public List<Customer> GetCustomers(CustomerSearch searchFilter);

        //public List<Customer> GetCustomersOptional(string firstName, string lastName, int gender);
        public List<Customer> GetCustomersOptional(CustomerSearchOptional optionalFilter);

        public Customer GetCustomerDetailsByEmail(string email);

        public void UpdateOnLoginSuccessful(string email);
        public void UpdateOnLoginFailed(string email);
        public void UpdateIsLocked(string email, bool isLocked = true);   
        
        public int Register(Customer customer);
        public int Update(Customer customer);
    }
}

﻿using Accounting_Software.Data.Entites;
using Accounting_Software.Date.Entites;

namespace Accounting_Software.Repository_Interfaces
{
    public interface IWareHouseRepositoryInterface
    {
        void Add(Product product);
        void Delete(int id);   
        List<Product> GetAll();
        Product GetById(int id);
        Product GetByName(string name);
        double GetAveragePrice();
    }
}

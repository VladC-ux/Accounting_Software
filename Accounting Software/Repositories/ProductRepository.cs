﻿using Accounting_Software.Data;
using Accounting_Software.Data.Entites;
using Accounting_Software.Repository_Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Accounting_Software.Repositories
{
    public class ProductRepository : IProductRepositoryInterface
    {
        private readonly DBContextAccounting _context;

        public ProductRepository(DBContextAccounting context)
        {
            _context = context;
        }

        public void Add(Product Product)
        {
            _context.Products.Add(Product);

        }



        public void Delete(int Product)
        {
            var querry = _context.Products.Find(Product);


            if (querry != null)
            {
                _context.Products.Remove(querry);
             
            }


        }

        public List<Product> GetAll()
        {
            return _context.Products.ToList();


        }

        public Product GetById(int Id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == Id);

        }


        public Product Update(Product product)
        {
            var entity = _context.Products.FirstOrDefault(p => p.Id == product.Id);

            entity.Name = product.Name;
            entity.Price = product.Price;
            entity.Mass = product.Mass;
            entity.Description = product.Description;
            entity.Unitofmass = product.Unitofmass;
            _context.Products.Update(entity);
            _context.SaveChanges();

            return entity;


        }
    }
}
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Accounting_Software.Data.Entites;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Accounting_Software.UnitOfWorkk;
using Accounting_Software.Date.Entites;
using Accounting_Software.Data;
using Accounting_Software.Repositories;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.ModelBinding;




namespace Accounting_Software.Service
{
    public class SellerService : ISellerServiceInterface
    {
        private readonly ISellerRepositoryInterface _sellerRepository;
        private readonly IUnitofWork _uow;
        private readonly IProductRepositoryInterface _productRepository;


        public SellerService(ISellerRepositoryInterface userRepositoryInterface, IUnitofWork unitofWork, IProductRepositoryInterface productRepository)
        {
            _sellerRepository = userRepositoryInterface;
            _uow = unitofWork;
            _productRepository = productRepository;

        }

        public void Add(SellerViewModel adduser)
        {
            Seller seller = new Seller
            {
                Name = adduser.Name,
            };

            _sellerRepository.Add(seller);
            _uow.SaveChanges();

        }

        public void Delete(SellerViewModel id)
        {
            throw new NotImplementedException();
        }

        public List<SellerViewModel> GetAll()
        {
            var data = _sellerRepository.GetAll();

            List<SellerViewModel> SellerViewModels = data.Select(seller => new SellerViewModel
            {
                Id = seller.Id,
                Name = seller.Name,

            }).ToList();

            return SellerViewModels;
        }

        public SellerViewModel GetById(int id)
        {
            var data = _sellerRepository.GetById(id);

            return new SellerViewModel
            {
                Id = data.Id,
                Name = data.Name,

            };
        }

        public void Update(SellerViewModel model)
        {
            Seller userToUpdate = new Seller
            {
                Id = model.Id,
                Name = model.Name,

            };

            var updatedUser = _sellerRepository.Update(userToUpdate);
            _uow.SaveChanges();

        }
    }





}

using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Accounting_Software.Data.Entites;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Accounting_Software.UnitOfWorkk;
using Accounting_Software.Date.Entites;
using Accounting_Software.Data;
using Accounting_Software.ViewModel;
using Accounting_Software.Repositories;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Accounting_Software.Service
{
    public class SellerService : ISellerServiceInterface
    {
        private readonly ISellerRepositoryInterface _userRepositoryInterface;
        private readonly IUnitofWork _uow;


        public SellerService(ISellerRepositoryInterface userRepositoryInterface, IUnitofWork unitofWork)
        {
            _userRepositoryInterface = userRepositoryInterface;
            _uow = unitofWork;

        }

         public void Add(AddEditViewModel adduser)
        {
            Seller seller = new Seller
            {
              
                Name = adduser.Name,
            };
            _userRepositoryInterface.Add(seller);
            _uow.SaveChanges();
        }

        public void Delete(SellerViewModel model)
        {
            Seller seller = new Seller
            {
                Name = model.Name,
            };
            _userRepositoryInterface.Delete(model.Id);
            _uow.SaveChanges();
        }

        public List<SellerViewModel> GetAll()
        {
            var data = _userRepositoryInterface.GetAll();

            List<SellerViewModel> SellerViewModels = data.Select(seller => new SellerViewModel
            {
                Id = seller.Id,
                Name = seller.Name,

            }).ToList();

            return SellerViewModels;
        }

        public SellerViewModel GetById(int id)
        {
            var data = _userRepositoryInterface.GetById(id);

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

            var updatedUser = _userRepositoryInterface.Update(userToUpdate);
            _uow.SaveChanges();

        }
    }





}

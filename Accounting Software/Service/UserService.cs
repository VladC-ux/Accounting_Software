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
    public class UserService : IUserServiceInterface
    {
        private readonly IUserRepositoryInterface _userRepositoryInterface;
        private readonly IUnitofWork _uow;


        public UserService(IUserRepositoryInterface userRepositoryInterface, IUnitofWork unitofWork, DBContextAccounting context)
        {
            _userRepositoryInterface = userRepositoryInterface;
            _uow = unitofWork;

        }

        public void Add(AddEditViewModel adduser)
        {
            User user = new User
            {
                Name = adduser.Name,
            };
            _userRepositoryInterface.Add(user);
            _uow.SaveChanges();
        }

        public void Delete(UserViewModel model)
        {
            User user = new User
            {
                Name = model.Name,
            };
            _userRepositoryInterface.Delete(model.Id);
            _uow.SaveChanges();
        }

        public List<UserViewModel> GetAll()
        {
            var data = _userRepositoryInterface.GetAll();

            List<UserViewModel> userViewModels = data.Select(user => new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,

            }).ToList();

            return userViewModels;
        }

        public UserViewModel GetById(int id)
        {
            var data = _userRepositoryInterface.GetById(id);

            return new UserViewModel
            {
                Id = data.Id,
                Name = data.Name,            

            };
        }

        public void Update(UserViewModel model)
        {
            User userToUpdate = new User
            {
                Id = model.Id,
                Name = model.Name,

            };

            var updatedUser = _userRepositoryInterface.Update(userToUpdate);
            _uow.SaveChanges();

        }
    }





}

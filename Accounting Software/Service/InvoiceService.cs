using Accounting_Software.Data.Entities;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.UnitOfWork;
using Accounting_Software.ViewModel;

namespace Accounting_Software.Service
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IStoreProductRepository _storeProductRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionHistoryRepository _transactionHistoryRepository;
        private readonly ITelegramNotifier _telegramNotifier;
        private readonly IUnitofWork _uow;

        public InvoiceService(
            IInvoiceRepository invoiceRepository,
            IStoreProductRepository storeProductRepository,
            IStoreRepository storeRepository,
            IUserRepository userRepository,
            ITransactionHistoryRepository transactionHistoryRepository,
            ITelegramNotifier telegramNotifier,
            IUnitofWork uow)
        {
            _invoiceRepository = invoiceRepository;
            _storeProductRepository = storeProductRepository;
            _storeRepository = storeRepository;
            _userRepository = userRepository;
            _transactionHistoryRepository = transactionHistoryRepository;
            _telegramNotifier = telegramNotifier;
            _uow = uow;
        }

        public InvoiceViewModel Create(InvoiceCreateViewModel model, int userId)
        {
            if (model.Items == null || model.Items.Count == 0)
                throw new InvalidOperationException("Invoice must contain at least one item.");

            var store = _storeRepository.GetById(model.StoreId)
                ?? throw new InvalidOperationException("Store not found.");

            var user = _userRepository.GetUserById(userId)
                ?? throw new InvalidOperationException("User not found.");

            var invoice = new Invoice
            {
                IssuedAt = DateTime.Now,
                UserId = userId,
                StoreId = store.Id,
                StoreName = store.StoreName,
                BuyerName = string.IsNullOrWhiteSpace(model.BuyerName) ? null : model.BuyerName.Trim(),
                BuyerEmail = string.IsNullOrWhiteSpace(model.BuyerEmail) ? null : model.BuyerEmail.Trim(),
                BuyerPhone = string.IsNullOrWhiteSpace(model.BuyerPhone) ? null : model.BuyerPhone.Trim(),
                TaxRate = model.TaxRate,
                Number = "PENDING"
            };

            var soldStoreProducts = new List<(StoreProduct sp, int count)>();

            foreach (var line in model.Items)
            {
                if (line.Count <= 0)
                    throw new InvalidOperationException("Quantity must be greater than zero.");

                var sp = _storeProductRepository.GetById(line.StoreProductId)
                    ?? throw new InvalidOperationException($"Store product #{line.StoreProductId} not found.");

                if (sp.StoreId != store.Id)
                    throw new InvalidOperationException($"Product '{sp.ProductName}' does not belong to store '{store.StoreName}'.");

                if (line.Count > sp.Count)
                    throw new InvalidOperationException($"Not enough stock for '{sp.ProductName}'. Available: {sp.Count}, requested: {line.Count}.");

                invoice.Items.Add(new InvoiceItem
                {
                    StoreProductId = sp.Id,
                    ProductId = sp.ProductId,
                    ProductName = sp.ProductName,
                    Description = sp.Description,
                    Price = sp.Price,
                    Count = line.Count,
                    Mass = sp.Mass,
                    UnitOfMass = sp.Unitofmass
                });

                sp.Count -= line.Count;
                soldStoreProducts.Add((sp, line.Count));
            }

            _invoiceRepository.Add(invoice);
            _uow.SaveChanges();

            invoice.Number = $"INV-{invoice.IssuedAt:yyyy}-{invoice.Id:D5}";

            decimal invoiceTotalForBalance = 0m;
            foreach (var (sp, count) in soldStoreProducts)
            {
                if (sp.Count <= 0)
                    _storeProductRepository.Delete(sp);
                else
                    _storeProductRepository.Update(sp);

                var lineTotal = sp.Unitofmass != Enums.Unit_of_mass.Pcs && sp.Mass > 0
                    ? sp.Price * count * sp.Mass
                    : sp.Price * count;
                invoiceTotalForBalance += lineTotal;

                _transactionHistoryRepository.Add(new TransactionHistory
                {
                    ProductName = sp.ProductName,
                    Price = sp.Price,
                    Description = sp.Description,
                    Mass = sp.Mass,
                    UserId = userId,
                    unitOfmass = sp.Unitofmass,
                    Count = count,
                    SoldDate = invoice.IssuedAt,
                    typeofAction = "Sale",
                    StoreName = sp.StoreName
                });

                _ = _telegramNotifier.NotifySaleAsync(userId, sp.ProductName, sp.Price, sp.StoreName);
            }

            user.Balance += invoiceTotalForBalance + Math.Round(invoiceTotalForBalance * invoice.TaxRate / 100m, 2);
            _userRepository.Update(user);

            _uow.SaveChanges();

            return ToViewModel(invoice, user.Name);
        }

        public InvoiceViewModel? GetById(int id)
        {
            var invoice = _invoiceRepository.GetById(id);
            return invoice == null ? null : ToViewModel(invoice, invoice.User?.Name);
        }

        public List<InvoiceViewModel> GetAll()
        {
            return _invoiceRepository.GetAll()
                .Select(i => ToViewModel(i, i.User?.Name))
                .ToList();
        }

        public List<InvoiceViewModel> GetByUserId(int userId)
        {
            return _invoiceRepository.GetByUserId(userId)
                .Select(i => ToViewModel(i, i.User?.Name))
                .ToList();
        }

        private static InvoiceViewModel ToViewModel(Invoice invoice, string? userName)
        {
            return new InvoiceViewModel
            {
                Id = invoice.Id,
                Number = invoice.Number,
                IssuedAt = invoice.IssuedAt,
                UserId = invoice.UserId,
                UserName = userName,
                StoreId = invoice.StoreId,
                StoreName = invoice.StoreName ?? invoice.Store?.StoreName,
                BuyerName = invoice.BuyerName,
                BuyerEmail = invoice.BuyerEmail,
                BuyerPhone = invoice.BuyerPhone,
                TaxRate = invoice.TaxRate,
                Items = invoice.Items.Select(it => new InvoiceItemViewModel
                {
                    Id = it.Id,
                    StoreProductId = it.StoreProductId,
                    ProductId = it.ProductId,
                    ProductName = it.ProductName,
                    Description = it.Description,
                    Price = it.Price,
                    Count = it.Count,
                    Mass = it.Mass,
                    UnitOfMass = it.UnitOfMass
                }).ToList()
            };
        }
    }
}

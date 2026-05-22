using Accounting_Software.Data;
using Accounting_Software.Data.Entities;
using Accounting_Software.Repository_Interfaces;

namespace Accounting_Software.Repositories
{
    public class TaxRepository : ITaxRepository
    {
        private readonly DBContextAccounting _context;

        public TaxRepository(DBContextAccounting context)
        {
            _context = context;
        }

        public void Add(TaxRule rule)
        {
            _context.TaxRules.Add(rule);
        }

        public void Delete(int id)
        {
            var entity = _context.TaxRules.Find(id);
            if (entity != null)
                _context.TaxRules.Remove(entity);
        }

        public List<TaxRule> GetByUserId(int userId)
        {
            return _context.TaxRules
                .Where(t => t.UserId == userId)
                .OrderBy(t => t.Id)
                .ToList();
        }

        public TaxRule? GetById(int id)
        {
            return _context.TaxRules.FirstOrDefault(t => t.Id == id);
        }

        public TaxRule? Update(TaxRule rule)
        {
            var entity = _context.TaxRules.FirstOrDefault(t => t.Id == rule.Id);
            if (entity == null)
                return null;

            entity.Name = rule.Name;
            entity.Rate = rule.Rate;
            entity.Description = rule.Description;
            _context.TaxRules.Update(entity);
            return entity;
        }
    }
}

using Microsoft.CodeAnalysis.CSharp;

namespace Accounting_Software.UnitOfWork
{
    public interface IUnitofWork
    {
        void SaveChanges();
        Task SaveChangesAsync();
    }
}

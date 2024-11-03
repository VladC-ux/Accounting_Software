using Microsoft.CodeAnalysis.CSharp;

namespace Accounting_Software.UnitOfWorkk
{
    public interface IUnitofWork
    {
        void SaveChanges();
        Task SaveChangesAsnyc();
    }
}

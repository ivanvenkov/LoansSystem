using LoansApi.DataAccess.Models;

namespace LoansApi.DataAccess.Interfaces
{
    public interface ILoansRepository
    {
        Task<IEnumerable<LoanWithInvoices>> GetLoansWithInvoicesAsync();
        Task<IEnumerable<LoansStatusShare>> GetLoansShareAsync();
    }
}
using LoansApi.Services.Models;

namespace LoansApi.Services.Interfaces
{
    public interface ILoansService
    {
        Task<IEnumerable<LoanWithInvoicesResponse>> GetLoansWithInvoicesAsync();
        Task<LoanStatusShareResponse> GetLoansShareByStatusAsync();
    }
}
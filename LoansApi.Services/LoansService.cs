using LoansApi.DataAccess.Enum;
using LoansApi.DataAccess.Interfaces;
using LoansApi.Services.Interfaces;
using LoansApi.Services.Models;

namespace LoansApi.Services
{
    public class LoansService : ILoansService
    {
        private readonly ILoansRepository loansRepository;

        public LoansService(ILoansRepository loansRepository)
        {
            this.loansRepository = loansRepository;
        }

        public async Task<IEnumerable<LoanWithInvoicesResponse>> GetLoansWithInvoicesAsync()
        {
            var data = await this.loansRepository.GetLoansWithInvoicesAsync();
            var loansWithInvoices = data.Select(x => new LoanWithInvoicesResponse
            {
                ApplicationAmount = x.ApplicationAmount,
                Status = x.Status,
                ApplicationDate = x.ApplicationDate,
                CustomerName = x.CustomerName,
                Invoices = x.Invoices,
                LoanId = x.LoanId,
            });
            return loansWithInvoices;
        }

        public async Task<LoanStatusShareResponse> GetLoansShareByStatusAsync()
        {
            var data = await this.loansRepository.GetLoansShareAsync();

            var totalAmountPaidAwaiting = data.Sum(s => s.TotalAmount);
            if (totalAmountPaidAwaiting <= 0)
            {
                return new LoanStatusShareResponse
                {
                    LoanSharesByStatus = Enumerable.Empty<LoanStatusShareSummary>(),
                    Errors = "The total amount of the loans with paid and awaiting payment statuses is 0."
                };
            }
            var result = data.Select(x => new LoanStatusShareSummary
            {
                Percentage = (double)Math.Round((x.TotalAmount / totalAmountPaidAwaiting), 2),
                Status = Enum.GetName(typeof(LoanStatus), x.Status),
                TotalAmount = x.TotalAmount,
            });

            return new LoanStatusShareResponse
            {
                LoanSharesByStatus = result,
            };
        }
    }
}

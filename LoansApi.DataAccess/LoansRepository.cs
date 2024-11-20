using Dapper;
using LoansApi.DataAccess.Interfaces;
using LoansApi.DataAccess.Models;
using System.Data;

namespace LoansApi.DataAccess
{
    public class LoansRepository : ILoansRepository
    {
        private readonly IDbConnection dbConnection;

        public LoansRepository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public async Task<IEnumerable<LoanWithInvoices>> GetLoansWithInvoicesAsync()
        {
            var sql = @"
                        SELECT l.LoanId, l.CustomerName, l.ApplicationAmount, l.ApplicationDate, l.Status, i.InvoiceId, i.LoanId, i.Number, i.Amount
                        FROM Loans l 
                        left join Invoices i on l.loanid=i.loanid";

            var loansWithInvoices = new List<LoanWithInvoices>();

            var _ = await this.dbConnection.QueryAsync<Loan, Invoice, LoanWithInvoices>(sql, (loan, invoice) =>
            {
                var loanWithInvoice = loansWithInvoices.FirstOrDefault(x => x.LoanId == loan.LoanId);
                if (loanWithInvoice is null)
                {
                    loanWithInvoice = new LoanWithInvoices 
                    {                       
                        ApplicationAmount =  loan.ApplicationAmount,
                        ApplicationDate = loan.ApplicationDate,
                        CustomerName = loan.CustomerName,
                        LoanId = loan.LoanId,
                        Status = loan.Status, 
                         
                    };
                    loansWithInvoices.Add(loanWithInvoice);
                }
                loanWithInvoice.Invoices.Add(invoice);
                return loanWithInvoice;

            }, splitOn: "InvoiceId");

            return loansWithInvoices;
        }

        public async Task<IEnumerable<LoansStatusShare>> GetLoansShareAsync()
        {
            var sql = @"
                SELECT Status, SUM(ApplicationAmount) AS TotalAmount
                FROM Loans
                WHERE Status IN ('Paid', 'AwaitingPayment')
                GROUP BY Status;";

            var loansShares = (await this.dbConnection.QueryAsync<LoansStatusShare>(sql)).ToList();
            return loansShares; 
        }
    }
}

using Dapper;
using LoansApi.DataAccess;
using LoansApi.DataAccess.Enum;
using Microsoft.Data.Sqlite;
using System.Data;

namespace LoansApi.Tests.Unit
{
    public class LoansRepositoryTests
    {
        private IDbConnection mockedDbConnection;
        private LoansRepository repository;

        [SetUp]
        public void Setup()
        {
            this.mockedDbConnection = new SqliteConnection("Data Source=:memory:");
            this.mockedDbConnection.Open();

            var createLoansTable = @"
       
                    CREATE TABLE IF NOT EXISTS Loans (
                        LoanId INTEGER PRIMARY KEY AUTOINCREMENT,
                        CustomerName TEXT NOT NULL,
                        ApplicationAmount DECIMAL(18, 2) NOT NULL,
                        ApplicationDate TEXT NOT NULL,
                        Status TEXT NOT NULL
                    );";

            var createInvoicesTable = @"
        
                    CREATE TABLE IF NOT EXISTS Invoices (
                        InvoiceId INTEGER PRIMARY KEY AUTOINCREMENT,                   
                        LoanId INTEGER,
                        Number TEXT NOT NULL,
                        Amount DECIMAL(18, 2) NOT NULL,
                        FOREIGN KEY (LoanId) REFERENCES Loans(LoanId)
                    );";

            mockedDbConnection.Execute(createLoansTable);
            mockedDbConnection.Execute(createInvoicesTable);

            this.repository = new LoansRepository(mockedDbConnection);
        }

        [TearDown]
        public void Cleanup()
        {
            this.mockedDbConnection.Dispose();
        }

        [Test]
        public async Task GetCreditsWithInvoicesAsync_ReturnsLoansWithInvoices()
        {
            // Arrange
            var insertLoans = @"
                    INSERT INTO Loans (LoanId, CustomerName, ApplicationAmount, ApplicationDate, Status)
                    VALUES (1, 'Test Customer', 10000, '2024-01-01', 'Paid');";

            var insertInvoices = @"
                    INSERT INTO Invoices (InvoiceId, LoanId, Number, Amount)
                    VALUES (1, 1, 'INV001', 1000);";

            this.mockedDbConnection.Execute(insertLoans);
            this.mockedDbConnection.Execute(insertInvoices);

            // Act
            var result = await this.repository.GetLoansWithInvoicesAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().CustomerName, Is.EqualTo("Test Customer"));
            Assert.That(result.First().Invoices.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetCreditSummaryAsync_ReturnsLoanSummary()
        {
            //Arrange
            var insertLoans = @"
                INSERT INTO Loans (LoanId, CustomerName, ApplicationAmount, ApplicationDate, Status)
                VALUES (1, 'Test Customer1', 5000, '2024-01-01', 'Paid');
                INSERT INTO Loans (LoanId, CustomerName, ApplicationAmount, ApplicationDate, Status)
                VALUES (2, 'Test Customer2', 150000, '2024-01-01', 'AwaitingPayment');
                INSERT INTO Loans (LoanId, CustomerName, ApplicationAmount, ApplicationDate, Status)
                VALUES (3, 'Test Customer3', 20000, '2024-01-01', 'Created');";

            this.mockedDbConnection.Execute(insertLoans);


            var sql = @"
                SELECT Status, SUM(ApplicationAmount) AS TotalAmount
                FROM Loans
                WHERE Status IN ('Paid', 'AwaitingPayment')
                GROUP BY Status;";

            //Act
            var result = (await this.repository.GetLoansShareAsync()).ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(x => x.Status == LoanStatus.Paid));
            Assert.That(result.Any(x=>x.TotalAmount==5000m));
            Assert.That(result.Any(x=>x.TotalAmount == 150000m));
            Assert.That(result.Any(x => x.Status == LoanStatus.AwaitingPayment));
            Assert.That(!result.Any(x=>x.Status== LoanStatus.Created));
        }
    }
}

using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace LoansApi.DataAccess
{
    public class DbInitializer
    {
        private readonly string connectionString = "Data Source=loansDb.db";

        public IDbConnection Connection => new SqliteConnection(this.connectionString);

        public void InitializeDatabase()
        {
            using (var connection = Connection)
            {
                connection.Open();
                connection.Execute("PRAGMA foreign_keys = ON;");

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

                connection.Execute(createLoansTable);
                connection.Execute(createInvoicesTable);

                if (connection.ExecuteScalar<int>("SELECT COUNT(1) FROM Loans;") > 0)
                {
                    return;
                }
                SeedDatabase(connection);
            }
        }

        private void SeedDatabase(IDbConnection connection)
        {
            var insertoans = @"
            INSERT INTO Loans (CustomerName, ApplicationAmount, ApplicationDate, Status)
            VALUES 
            ('John Doe', 10000, '2024-01-15T00:00:00', 'Paid'),
            ('Jane Smith', 5000, '2024-02-20T00:00:00', 'AwaitingPayment'),
            ('Tom Brown', 15000, '2024-03-10T00:00:00', 'Created');";

            connection.Execute(insertoans);

            var insertInvoices = @"
            INSERT INTO Invoices (LoanId, Number, Amount)
            VALUES
            (1, 'INV001', 1000),
            (1, 'INV002', 2000),
            (2, 'INV003', 1000),
            (2, 'INV004', 1500),
            (3, 'INV005', 5000);";

            connection.Execute(insertInvoices);
        }
    }
}


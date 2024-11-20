using LoansApi.DataAccess.Enum;
using LoansApi.DataAccess.Interfaces;
using LoansApi.DataAccess.Models;
using LoansApi.Services;
using NSubstitute;

namespace LoansApi.Tests.Unit
{
    public class LoansServiceTests
    {
        private LoansService service;
        private ILoansRepository mockedRepository;

        [SetUp]
        public void Setup()
        {
            this.mockedRepository = Substitute.For<ILoansRepository>();
            this.service = new(this.mockedRepository);
        }

        [Test]
        public async Task GetLoansWithInvoicesAsync_ReturnsCorerctResponse()
        {
            //Arrange
            var mockedLoandWithInvoices = new List<LoanWithInvoices>
            {
                new(){ LoanId = 1, CustomerName="TestCustomer1"},
                new(){ LoanId = 2, CustomerName="TestCustomer2"}
            };

            this.mockedRepository.GetLoansWithInvoicesAsync().Returns(mockedLoandWithInvoices);

            //Act
            var result = await this.service.GetLoansWithInvoicesAsync();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().CustomerName, Is.EqualTo("TestCustomer1"));
        }

        [Test]
        public async Task GetlLoansSummaryByStatusAsync_ReturnsCorrectPercentage()
        {
            //Arrange
            var mockedSumary = new List<LoansStatusShare>
            {
                new(){ Status = LoanStatus.Paid, TotalAmount = 5000m },
                new(){ Status = LoanStatus.AwaitingPayment, TotalAmount = 15000m },
            };

            this.mockedRepository.GetLoansShareAsync().Returns(mockedSumary);

            //Act
            var result = await this.service.GetLoansShareByStatusAsync();

            //Assert
            Assert.That(result.LoanSharesByStatus.Count(), Is.EqualTo(2));
            Assert.That(result.LoanSharesByStatus.First().Status, Is.EqualTo("Paid"));
            Assert.That(result.LoanSharesByStatus.First().Percentage, Is.EqualTo(25 / 100d));
            Assert.That(result.LoanSharesByStatus.First().TotalAmount, Is.EqualTo(5000m));
        }

        [Test]
        public async Task GetLoansShareByStatusAsync_SetsErrorMessage_WhenTotalAmountIsZero()
        {
            // Arrange
            var mockData = new List<LoansStatusShare>
            {
                new LoansStatusShare { Status = LoanStatus.Paid, TotalAmount = 0 },
                new LoansStatusShare { Status = LoanStatus.AwaitingPayment, TotalAmount = 0 }
            };

            this.mockedRepository.GetLoansShareAsync().Returns(mockData);

            // Act
            var result = (await this.service.GetLoansShareByStatusAsync());

            // Assert
            Assert.That(result.LoanSharesByStatus, Is.Empty);
            Assert.That(result.Errors, Is.EqualTo("The total amount of the loans with paid and awaiting payment statuses is 0."));
        }
    }
}

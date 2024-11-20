using LoansApi.Controllers;
using LoansApi.Services.Interfaces;
using LoansApi.Services.Models;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace LoansApi.Tests.Unit
{
    public class LoansControllerTests
    {
        private LoansController loanController;
        private ILoansService mockedLoansService;

        [SetUp]
        public void Setup()
        {
            this.mockedLoansService = Substitute.For<ILoansService>();
            this.loanController = new(this.mockedLoansService);
        }

        [Test]
        public async Task GetLoansWithInvoices_ReturnsOk()
        {
            //Arrange
            var mockedLoans = new List<LoanWithInvoicesResponse>
            {
                new(){LoanId = 1, CustomerName = "TestUser1"},
                new(){LoanId = 2, CustomerName = "TestUser2"},
            };

            this.mockedLoansService.GetLoansWithInvoicesAsync().Returns(mockedLoans);

            //Act
            var result = await this.loanController.GetLoansWithInvoices();

            //Assert
            Assert.That(result, Is.InstanceOf<ActionResult<IEnumerable<LoanWithInvoicesResponse>>>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));

            var loans = okResult.Value as IEnumerable<LoanWithInvoicesResponse>;
            Assert.That(loans, Is.Not.Null);
            Assert.That(loans.Count(), Is.EqualTo(2));
            Assert.That(loans.First().CustomerName, Is.EqualTo("TestUser1"));
        }

        [Test]
        public async Task GetLoansSummaryByStatus_ReturnsOk()
        {
            //Arrange
            var mockedSummary = new List<LoanStatusShareSummary>
            {
                new(){  Status = "Paid", TotalAmount=5000m, Percentage = 0.25},
                new(){  Status = "AwaitingPayment", TotalAmount=15000m, Percentage = 0.75}
            };

            var mockedResponse = new LoanStatusShareResponse { LoanSharesByStatus = mockedSummary };

            this.mockedLoansService.GetLoansShareByStatusAsync().Returns(mockedResponse);

            //Act
            var result = await this.loanController.GetLoansShareByStatus();

            //Assert
            Assert.That(result, Is.InstanceOf<ActionResult<LoanStatusShareResponse>>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));

            var summary = okResult.Value as LoanStatusShareResponse;
            Assert.That(summary, Is.Not.Null);
            Assert.That(summary.LoanSharesByStatus.Count, Is.EqualTo(2));
            Assert.That(summary.LoanSharesByStatus.First().Status, Is.EqualTo("Paid"));
        }
    }
}

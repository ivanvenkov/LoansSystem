using LoansApi.Services.Interfaces;
using LoansApi.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoansApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoansService loansService;

        public LoansController(ILoansService loansService)
        {
            this.loansService = loansService;
        }

        [HttpGet("loans-with-invoices")]        
        public async Task<ActionResult<IEnumerable<LoanWithInvoicesResponse>>> GetLoansWithInvoices()
        {
            var result = await this.loansService.GetLoansWithInvoicesAsync();
            return Ok(result);
        }  

        [HttpGet("statuses-paid-and-awaiting-share")]
        public async Task<ActionResult<LoanStatusShareResponse>> GetLoansShareByStatus()
        {
            var loansShareByStatus = await this.loansService.GetLoansShareByStatusAsync();
            return Ok(loansShareByStatus);
        }
    }
}

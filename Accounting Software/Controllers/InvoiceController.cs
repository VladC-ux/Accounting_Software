using Accounting_Software.Service_Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IInvoicePdfService _invoicePdfService;

        public InvoiceController(
            IInvoiceService invoiceService,
            IInvoicePdfService invoicePdfService)
        {
            _invoiceService = invoiceService;
            _invoicePdfService = invoicePdfService;
        }

        public IActionResult Index()
        {
            var invoices = _invoiceService.GetAll();
            return View(invoices);
        }

        public IActionResult Details(int id)
        {
            var invoice = _invoiceService.GetById(id);
            if (invoice == null)
                return NotFound("Invoice not found.");

            return View(invoice);
        }

        [HttpGet]
        public IActionResult Pdf(int id)
        {
            var invoice = _invoiceService.GetById(id);
            if (invoice == null)
                return NotFound("Invoice not found.");

            var pdf = _invoicePdfService.Generate(invoice);
            return File(pdf, "application/pdf", $"{invoice.Number}.pdf");
        }
    }
}

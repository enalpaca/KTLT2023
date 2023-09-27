using DoAn1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;


namespace DoAn1.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(ILogger<InvoiceController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();
            ViewBag.InvoiceList = ReadListInvoice.ToArray();
            return View();
        }

        public IActionResult CreateInvoice()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ActionName("Create")]

        public ActionResult Create_post(IFormCollection collection)
        {
            try
            {
                Invoice invoiceModel = new Invoice();
                invoiceModel.invoiceCode = collection["invoiceCode"];
                invoiceModel.invoiceCreateDate = collection["invoiceCreateDate"];
                IOFile.IOFile.SaveInvoice(invoiceModel);
                return Redirect("/Invoice");
            }
            catch
            {
                return View();
            }
        }
    }
}

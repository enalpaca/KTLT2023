using DoAn_KTLT.IOFile;
using DoAn_KTLT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
namespace DoAn_KTLT.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(ILogger<InvoiceController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string searchText)
        {
            List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();
            if (searchText != null && searchText != "")
            {
                ReadListInvoice = ReadListInvoice.FindAll(p => Utils.StringLike(p.invoiceCode, searchText) || Utils.StringLike(p.invoiceCustomerName, searchText));
            }/*
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();
            foreach (Product p in ReadListProduct)
            {
                Category? category = ReadListCategory.Find(cat => cat.categoryCode == p.productCategory);
                if (category != null)
                {
                    p.productCategoryName = category.categoryName;
                }
            }*/
            ViewBag.InvoiceList = ReadListInvoice.ToArray();
            return View();
        } //??

        public IActionResult EditInvoice(string invoiceCode)
        {
            List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();
            Invoice? invoice = ReadListInvoice.Find(x => x.invoiceCode == invoiceCode);
            List<Category> ReadListCategory = IOFile.IOFile.ReadCategory(); //??

            ViewBag.CategoryList = ReadListCategory.ToArray();//??
            ViewBag.invoice = invoice;
            return View();
        } //??

        public IActionResult CreateInvoice()
        {
            List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();//???
            ViewBag.CategoryList = ReadListCategory.ToArray();//???
            return View();
        } //??

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ActionName("CreateInvoice")]

        public ActionResult CreateInvoice(Invoice newInvoice)
        {
            try
            {
                List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();

                ReadListInvoice.Add(newInvoice);
                IOFile.IOFile.SaveInvoices(ReadListInvoice);
                return Redirect("/Invoice");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("EditInvoice")]
        public ActionResult EditInvoice(Invoice updatedInvoice)
        {
            try
            {
                List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();

                int invoiceIndex = ReadListInvoice.FindIndex(x => x.invoiceCode == updatedInvoice.invoiceCode);

                if (invoiceIndex >= 0)
                {
                    ReadListInvoice.RemoveAt(invoiceIndex);
                    ReadListInvoice.Insert(invoiceIndex, updatedInvoice);
                    IOFile.IOFile.SaveInvoices(ReadListInvoice);
                }

                return Redirect("/Invoice");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("DeleteInvoice")]
        public ActionResult DeleteInvoice(string invoiceCode)
        {
            try
            {
                List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();
                int invoiceIndex = ReadListInvoice.FindIndex(x => x.invoiceCode == invoiceCode);

                if (invoiceIndex >= 0)
                {
                    ReadListInvoice.RemoveAt(invoiceIndex);
                    IOFile.IOFile.SaveInvoices(ReadListInvoice);
                }

                return Redirect("/Invoice");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("SearchProduct")]
        public ActionResult SearchInvoice(string searchText)
        {
            var encodedLocationName = WebUtility.UrlEncode(searchText);
            return Redirect($"/Invoice?searchText={encodedLocationName}");
        }

    }
}

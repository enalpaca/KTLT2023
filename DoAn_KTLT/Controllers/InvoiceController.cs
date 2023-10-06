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
                ReadListInvoice = ReadListInvoice.FindAll(p => Utils.StringLike(p.InvoiceCode, searchText) || Utils.StringLike(p.InvoiceCustomerName, searchText));
            }

            ViewBag.InvoiceList = ReadListInvoice.ToArray();
            return View();
        }

        // https://www.telerik.com/blogs/how-to-pass-multiple-parameters-get-method-aspnet-core-mvc
        [HttpGet("Invoice/Details/{InvoiceCode}")]
        public ActionResult Details(string InvoiceCode)
        {
            List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();
            Invoice? invoice = ReadListInvoice.Find(x => x.InvoiceCode == InvoiceCode);
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();

            if (invoice != null)
            {
                foreach (InvoiceProduct item in invoice.ProductItems)
                {
                    Product? product = ReadListProduct.Find(x => x.ProductCode == item.InvoiceProductCode);
                    if (product != null)
                    {
                        item.InvoiceProductName = product.ProductName;
                        item.InvoiceProductPrice = product.ProductPrice;
                    }
                }
            }

            ViewBag.invoice = invoice;
            return View();
        }

        [HttpGet("Invoice/Edit/{InvoiceCode}")]
        public IActionResult EditInvoice(string InvoiceCode)
        {
            List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();
            Invoice? invoice = ReadListInvoice.Find(x => x.InvoiceCode == InvoiceCode);
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();

            if (invoice != null)
            {
                foreach (InvoiceProduct item in invoice.ProductItems)
                {
                    Product? product = ReadListProduct.Find(x => x.ProductCode == item.InvoiceProductCode);
                    if (product != null)
                    {
                        item.InvoiceProductName = product.ProductName;
                        item.InvoiceProductPrice = product.ProductPrice;
                    }
                }
            }

            ViewBag.invoice = invoice;
            ViewBag.ProductList = ReadListProduct.ToArray();
            return View();
        }

        public IActionResult CreateInvoice()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost("Invoice/Create")]
        [ActionName("CreateInvoice")]
        public ActionResult CreateInvoice(Invoice newInvoice)
        {
            try
            {
                List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();
                newInvoice.InvoiceCreateDate = DateTime.Now;
                ReadListInvoice.Add(newInvoice);
                IOFile.IOFile.SaveInvoices(ReadListInvoice);
                return Redirect("/Invoice/Edit/" + newInvoice.InvoiceCode);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost("Invoice/Update/{InvoiceCode}")]
        [ActionName("UpdateInvoice")]
        public ActionResult UpdateInvoice(Invoice updatedInvoice)
        {
            try
            {
                List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();

                int invoiceIndex = ReadListInvoice.FindIndex(x => x.InvoiceCode == updatedInvoice.InvoiceCode);

                if (invoiceIndex >= 0)
                {
                    updatedInvoice.ProductItems = ReadListInvoice[invoiceIndex].ProductItems;
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

        [HttpPost("Invoice/Edit/{InvoiceCode}/ProductItem")]
        [ActionName("CreateInvoiceProductItem")]
        public ActionResult CreateInvoiceProductItem(string InvoiceCode, InvoiceProduct newInvoiceProduct)
        {
            try
            {
                List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();

                int invoiceIndex = ReadListInvoice.FindIndex(x => x.InvoiceCode == InvoiceCode);

                if (invoiceIndex >= 0)
                {
                    int productItemsIndex = ReadListInvoice[invoiceIndex].ProductItems.FindIndex(y => y.InvoiceProductCode == newInvoiceProduct.InvoiceProductCode);
                    if (productItemsIndex >= 0)
                    {
                        ReadListInvoice[invoiceIndex].ProductItems[productItemsIndex].InvoiceProductQuantity += newInvoiceProduct.InvoiceProductQuantity;
                    }
                    else
                    {
                        ReadListInvoice[invoiceIndex].ProductItems.Add(newInvoiceProduct);
                    }
                    IOFile.IOFile.SaveInvoices(ReadListInvoice);

                }

                return Redirect("/Invoice/Edit/" + InvoiceCode);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost("Invoice/Delete/{InvoiceCode}/ProductItem/{ProductCode}")]
        [ActionName("DeleteInvoiceProductItem")]
        public ActionResult DeleteInvoiceProductItem(string InvoiceCode, string ProductCode)
        {
            try
            {
                List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();

                int invoiceIndex = ReadListInvoice.FindIndex(x => x.InvoiceCode == InvoiceCode);

                if (invoiceIndex >= 0)
                {
                    int invoiceProducItemIndex = ReadListInvoice[invoiceIndex].ProductItems.FindIndex(x => x.InvoiceProductCode == ProductCode);
                    if (invoiceProducItemIndex >= 0)
                    {
                        ReadListInvoice[invoiceIndex].ProductItems.RemoveAt(invoiceProducItemIndex);
                        IOFile.IOFile.SaveInvoices(ReadListInvoice);
                    }
                }

                return Redirect("/Invoice/Edit/" + InvoiceCode);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost("Invoice/Update/{InvoiceCode}/ProductItem/{ProductCode}")]
        [ActionName("UpdateInvoiceProductItem")]
        public ActionResult UpdateInvoiceProductItem(string InvoiceCode, string ProductCode, InvoiceProduct updatedInvoiceProduct)
        {
            try
            {
                List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();

                int invoiceIndex = ReadListInvoice.FindIndex(x => x.InvoiceCode == InvoiceCode);

                if (invoiceIndex >= 0)
                {
                    int invoiceProducItemIndex = ReadListInvoice[invoiceIndex].ProductItems.FindIndex(x => x.InvoiceProductCode == ProductCode);
                    if (invoiceProducItemIndex >= 0)
                    {
                        ReadListInvoice[invoiceIndex].ProductItems[invoiceProducItemIndex].InvoiceProductQuantity = updatedInvoiceProduct.InvoiceProductQuantity;
                        IOFile.IOFile.SaveInvoices(ReadListInvoice);
                    }
                }

                return Redirect("/Invoice/Edit/" + InvoiceCode);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost("Invoice/Delete/{InvoiceCode}")]
        [ActionName("DeleteInvoice")]
        public ActionResult DeleteInvoice(string InvoiceCode)
        {
            try
            {
                List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();
                int invoiceIndex = ReadListInvoice.FindIndex(x => x.InvoiceCode == InvoiceCode);

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
        [ActionName("SearchInvoice")]
        public ActionResult SearchInvoice(string searchText)
        {
            var encodedLocationName = WebUtility.UrlEncode(searchText);
            return Redirect($"/Invoice?searchText={encodedLocationName}");
        }

    }
}

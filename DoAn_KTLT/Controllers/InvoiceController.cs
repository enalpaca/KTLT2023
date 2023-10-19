using DoAn_KTLT.IOFile;
using DoAn_KTLT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
namespace DoAn_KTLT.Controllers
{
    public class InvoiceController : BaseController
    {
        private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(ILogger<InvoiceController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string searchText, int? page)
        {
            List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();
            if (searchText != null && searchText != "")
            {
                ReadListInvoice = ReadListInvoice.FindAll(p => Utils.StringLike(p.InvoiceCode, searchText) || Utils.StringLike(p.InvoiceCustomerName, searchText));
            }

            int totalPage = Utils.CalculateNumberOfPage(ReadListInvoice.Count, PAGE_SIZE);
            int currentPage = page ?? 1;

            //Paging => get data from currentPage - 1 * pageSize to (currentPage * pageSize) + pageSize
            IEnumerable<Invoice> listInvoice = ReadListInvoice.Skip((currentPage - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE);

            ViewBag.totalPage = totalPage;
            ViewBag.currentPage = currentPage;
            ViewBag.totalRow = ReadListInvoice.Count;
            ViewBag.InvoiceList = listInvoice.ToArray();
            ViewBag.searchText = searchText ?? "";
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
                List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();

                int currentProductIndex = ReadListProduct.FindIndex(p => p.ProductCode == newInvoiceProduct.InvoiceProductCode);
                if (currentProductIndex < 0)
                {
                    SetAlert("Sản phẩm không tồn tại", 3);
                    return Redirect("/Invoice/Edit/" + InvoiceCode);
                }

                if (ReadListProduct[currentProductIndex].ProductQuantity < newInvoiceProduct.InvoiceProductQuantity)
                {
                    SetAlert("Vượt quá số lượng tồn kho", 3);
                    return Redirect("/Invoice/Edit/" + InvoiceCode);
                }

                int invoiceIndex = ReadListInvoice.FindIndex(x => x.InvoiceCode == InvoiceCode);

                if (invoiceIndex < 0)
                {
                    SetAlert("Hóa đơn không tồn tại", 3);
                    return Redirect("/Invoice/Edit/" + InvoiceCode);
                }

                InvoiceProduct? invoiceProduct = ReadListInvoice[invoiceIndex].ProductItems.Find(p => p.InvoiceProductCode == newInvoiceProduct.InvoiceProductCode);
                if (invoiceProduct != null)
                {
                    SetAlert("Sản phẩm đã tồn tại. Vui lòng cập nhật số lượng bên dưới!", 3);
                    return Redirect("/Invoice/Edit/" + InvoiceCode);
                }

                ReadListInvoice[invoiceIndex].ProductItems.Add(newInvoiceProduct);
                ReadListProduct[currentProductIndex].ProductQuantity -= newInvoiceProduct.InvoiceProductQuantity;

                IOFile.IOFile.SaveInvoices(ReadListInvoice);

                SetAlert("Cập nhật thành công", 0);
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

            List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();

            int invoiceIndex = ReadListInvoice.FindIndex(x => x.InvoiceCode == InvoiceCode);

            if (invoiceIndex >= 0)
            {
                int invoiceProducItemIndex = ReadListInvoice[invoiceIndex].ProductItems.FindIndex(x => x.InvoiceProductCode == ProductCode);

                if (invoiceProducItemIndex >= 0)
                {
                    int productIndex = ReadListProduct.FindIndex(p => p.ProductCode == ReadListInvoice[invoiceIndex].ProductItems[invoiceProducItemIndex].InvoiceProductCode);

                    if (productIndex >= 0)
                    {
                        ReadListProduct[productIndex].ProductQuantity += ReadListInvoice[invoiceIndex].ProductItems[invoiceProducItemIndex].InvoiceProductQuantity;
                    }

                    ReadListInvoice[invoiceIndex].ProductItems.RemoveAt(invoiceProducItemIndex);

                    IOFile.IOFile.SaveProducts(ReadListProduct);
                    IOFile.IOFile.SaveInvoices(ReadListInvoice);
                    SetAlert("Xóa thành công", 0);
                }
            }

            return Redirect("/Invoice/Edit/" + InvoiceCode);

        }

        [HttpPost("Invoice/Update/{InvoiceCode}/ProductItem/{ProductCode}")]
        [ActionName("UpdateInvoiceProductItem")]
        public ActionResult UpdateInvoiceProductItem(string InvoiceCode, string ProductCode, InvoiceProduct updatedInvoiceProduct)
        {
            try
            {
                List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();
                List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();

                int currentProductIndex = ReadListProduct.FindIndex(p => p.ProductCode == ProductCode);
                int invoiceIndex = ReadListInvoice.FindIndex(x => x.InvoiceCode == InvoiceCode);

                if (currentProductIndex < 0)
                {
                    SetAlert("Sản phẩm không tồn tại", 3);
                    return Redirect("/Invoice/Edit/" + InvoiceCode);
                }
                if (invoiceIndex < 0)
                {
                    SetAlert("Hóa đơn không tồn tại", 3);
                    return Redirect("/Invoice/Edit/" + InvoiceCode);
                }


                int invoiceProducItemIndex = ReadListInvoice[invoiceIndex].ProductItems.FindIndex(x => x.InvoiceProductCode == ProductCode);

                if (invoiceProducItemIndex < 0)
                {
                    SetAlert("Sản phẩm không tồn tại", 3);
                    return Redirect("/Invoice/Edit/" + InvoiceCode);
                }

                int delta = updatedInvoiceProduct.InvoiceProductQuantity - ReadListInvoice[invoiceIndex].ProductItems[invoiceProducItemIndex].InvoiceProductQuantity;

                ReadListInvoice[invoiceIndex].ProductItems[invoiceProducItemIndex].InvoiceProductQuantity = updatedInvoiceProduct.InvoiceProductQuantity;

                // user tăng sl trong hóa đơn
                if (delta > 0)
                {
                    if (delta > ReadListProduct[currentProductIndex].ProductQuantity)
                    {
                        SetAlert("Vượt quá số lượng tồn kho", 3);
                        return Redirect("/Invoice/Edit/" + InvoiceCode);
                    }

                    // giảm sl tồn kho
                    ReadListProduct[currentProductIndex].ProductQuantity -= delta;
                }

                // user giảm sl trong hóa đơn
                if (delta < 0)
                {
                    // tăng sl tồn kho
                    ReadListProduct[currentProductIndex].ProductQuantity += Math.Abs(delta);
                }

                // user đã giảm sl tới 0, ta sẽ xóa sp khỏi hóa đơn
                if (ReadListInvoice[invoiceIndex].ProductItems[invoiceProducItemIndex].InvoiceProductQuantity == 0)
                {
                    ReadListInvoice[invoiceIndex].ProductItems.RemoveAt(invoiceProducItemIndex);
                }

                IOFile.IOFile.SaveProducts(ReadListProduct);
                IOFile.IOFile.SaveInvoices(ReadListInvoice);

                SetAlert("Cập nhật thành công", 0);
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

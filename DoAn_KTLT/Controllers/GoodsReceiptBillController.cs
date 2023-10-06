using DoAn_KTLT.IOFile;
using DoAn_KTLT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace DoAn_KTLT.Controllers
{
    public class GoodsReceiptBillController : Controller
    {
        private readonly ILogger<GoodsReceiptBillController> _logger;

        public GoodsReceiptBillController(ILogger<GoodsReceiptBillController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string searchText)
        {
            List<GoodsReceiptBill> ReadListGoodsReceiptBill = IOFile.IOFile.ReadGoodsReceiptBill();
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();
            foreach (GoodsReceiptBill item in ReadListGoodsReceiptBill)
            {
                Product? product = ReadListProduct.Find(x => x.ProductCode == item.goodsReceiptBillProductCode);
                if (product != null)
                {
                    item.goodsReceiptBillProductName = product.ProductName;
                }
            }
            if (searchText != null && searchText != "")
            {
              
                ReadListGoodsReceiptBill = ReadListGoodsReceiptBill.FindAll(p => Utils.StringLike(p.goodsReceiptBillCode, searchText) || Utils.StringLike(p.goodsReceiptBillProductName, searchText) || Utils.StringLike(p.goodsReceiptBillProductCompany, searchText));
            }

            ViewBag.GoodsReceiptBillList = ReadListGoodsReceiptBill.ToArray();
            return View();
        }

        public IActionResult EditGoodsReceiptBill(string goodsReceiptBillCode)
        {
            List<GoodsReceiptBill> ReadListGoodsReceiptBill = IOFile.IOFile.ReadGoodsReceiptBill();
            GoodsReceiptBill? goodsReceiptBill = ReadListGoodsReceiptBill.Find(x => x.goodsReceiptBillCode == goodsReceiptBillCode);
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();

            ViewBag.ProductList = ReadListProduct.ToArray();
            ViewBag.goodsReceiptBill = goodsReceiptBill;
            return View();
        }

        public IActionResult CreateGoodsReceiptBill()
        {
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();
            ViewBag.ProductList = ReadListProduct.ToArray();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ActionName("CreateGoodsReceiptBill")]
        public ActionResult CreateGoodsReceiptBill(GoodsReceiptBill newGoodsReceiptBill)
        {
            try
            {
                List<GoodsReceiptBill> ReadListGoodsReceiptBill = IOFile.IOFile.ReadGoodsReceiptBill();

                ReadListGoodsReceiptBill.Add(newGoodsReceiptBill);
                IOFile.IOFile.SaveGoodsReceiptBills(ReadListGoodsReceiptBill);
                return Redirect("/GoodsReceiptBill");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("EditGoodsReceiptBill")]
        public ActionResult EditGoodsReceiptBill(GoodsReceiptBill updatedGoodsReceiptBill)
        {
            try
            {
                List<GoodsReceiptBill> ReadListGoodsReceiptBill = IOFile.IOFile.ReadGoodsReceiptBill();

                int goodsReceiptBillIndex = ReadListGoodsReceiptBill.FindIndex(x => x.goodsReceiptBillCode == updatedGoodsReceiptBill.goodsReceiptBillCode);

                if (goodsReceiptBillIndex >= 0)
                {
                    ReadListGoodsReceiptBill.RemoveAt(goodsReceiptBillIndex);
                    ReadListGoodsReceiptBill.Insert(goodsReceiptBillIndex, updatedGoodsReceiptBill);
                    IOFile.IOFile.SaveGoodsReceiptBills(ReadListGoodsReceiptBill);
                }

                return Redirect("/GoodsReceiptBill");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("DeleteGoodsReceiptBill")]
        public ActionResult DeleteGoodsReceiptBill(string goodsReceiptBillCode)
        {
            try
            {
                List<GoodsReceiptBill> ReadListGoodsReceiptBill = IOFile.IOFile.ReadGoodsReceiptBill();
                int goodsReceiptBillIndex = ReadListGoodsReceiptBill.FindIndex(x => x.goodsReceiptBillCode == goodsReceiptBillCode);

                if (goodsReceiptBillIndex >= 0)
                {
                    ReadListGoodsReceiptBill.RemoveAt(goodsReceiptBillIndex);
                    IOFile.IOFile.SaveGoodsReceiptBills(ReadListGoodsReceiptBill);
                }

                return Redirect("/GoodsReceiptBill");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("SearchGoodsReceiptBill")]
        public ActionResult SearchGoodsReceiptBill(string searchText)
        {
            var encodedLocationName = WebUtility.UrlEncode(searchText);
            return Redirect($"/GoodsReceiptBill?searchText={encodedLocationName}");
        }
    }
}

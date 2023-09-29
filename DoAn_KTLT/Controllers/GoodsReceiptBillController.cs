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
            if (searchText != null && searchText != "")
            {
                ReadListGoodsReceiptBill = ReadListGoodsReceiptBill.FindAll(p => Utils.StringLike(p.goodsReceiptBillProductName, searchText) || Utils.StringLike(p.goodsReceiptBillProductCompany, searchText));
            }
/*            List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();
            foreach (Product p in ReadListProduct)
            {
                Category? category = ReadListCategory.Find(cat => cat.categoryCode == p.productCategory);
                if (category != null)
                {
                    p.productCategoryName = category.categoryName;
                }
            }*/
            ViewBag.GoodsReceiptBillList = ReadListGoodsReceiptBill.ToArray();
            return View();
        }//??

        public IActionResult EditGoodsReceiptBill(string goodsReceiptBillCode)
        {
            List<GoodsReceiptBill> ReadListGoodsReceiptBill = IOFile.IOFile.ReadGoodsReceiptBill();
            GoodsReceiptBill? goodsReceiptBill = ReadListGoodsReceiptBill.Find(x => x.goodsReceiptBillCode == goodsReceiptBillCode);
/*            List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();

            ViewBag.CategoryList = ReadListCategory.ToArray();*/
            ViewBag.goodsReceiptBill = goodsReceiptBill;
            return View();
        }

        public IActionResult CreateGoodsReceiptBill()
        {
/*            List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();
            ViewBag.CategoryList = ReadListCategory.ToArray();*/ 
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

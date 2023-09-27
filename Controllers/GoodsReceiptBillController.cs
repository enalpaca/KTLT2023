using DoAn1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;


namespace DoAn1.Controllers
{
    public class GoodsReceiptBillController : Controller
    {
        private readonly ILogger<GoodsReceiptBillController> _logger;

        public GoodsReceiptBillController(ILogger<GoodsReceiptBillController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            List<GoodsReceiptBill> ReadListGoodsReceiptBill = IOFile.IOFile.ReadGoodsReceiptBill();
            ViewBag.GoodsReceiptBillList = ReadListGoodsReceiptBill.ToArray();
            return View();
        }

        public IActionResult CreateGoodsReceiptBill()
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
                GoodsReceiptBill goodsReceiptBillModel = new GoodsReceiptBill();
                goodsReceiptBillModel.goodsReceiptBillCode = collection["goodsReceiptBillCode"];
                goodsReceiptBillModel.goodsReceiptBillCreateDate = collection["goodsReceiptBillCreateDate"];
                IOFile.IOFile.SaveGoodsReceiptBill(goodsReceiptBillModel);
                return Redirect("/GoodsReceiptBill");
            }
            catch
            {
                return View();
            }
        }
    }
}

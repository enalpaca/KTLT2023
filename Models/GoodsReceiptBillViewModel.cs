using System;
namespace DoAn1.Models
{
    public class GoodsReceiptBill
    {
        public string goodsReceiptBillCode { get; set; }
        public string goodsReceiptBillCreateDate { get; set; }
    }


    public class GoodsReceiptBillViewModel
    {
        public GoodsReceiptBill[] goodsReceiptBillList = {};
    }


}

using System;
namespace DoAn_KTLT.Models
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

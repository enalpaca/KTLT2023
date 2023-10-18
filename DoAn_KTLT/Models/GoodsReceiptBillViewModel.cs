using DoAn_KTLT.IOFile;

namespace DoAn_KTLT.Models
{

    [Serializable]
    public class GoodsReceiptBillProduct
    {
        public string GoodsReceiptBillProductName { get; set; } = "";
        public string GoodsReceiptBillProductProductCode { get; set; } = "";
    }
    [Serializable]
    public class GoodsReceiptBill
    {
        public string goodsReceiptBillCode { get; set; } = "";
        public string goodsReceiptBillProductName { get; set; } = "";
        public string goodsReceiptBillProductCode { get; set; } = "";
        public int goodsReceiptBillProductQuantity { get; set; } = 0;
        public int goodsReceiptBillProductPrice { get; set; } = 0;
        public string goodsReceiptBillProductCompany { get; set; } = "";
        public string goodsReceiptBillProductDeliver { get; set; } = "";
        public DateTime goodsReceiptBillCreateDate { get; set; } = DateTime.Now;
        public GoodsReceiptBill()
        {
            goodsReceiptBillCode = Utils.GenerateString();
        }
    }
}

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
        public string goodsReceiptBillProductQuantity { get; set; } = "";
        public string goodsReceiptProductUnit { get; set; } = "";
        public string goodsReceiptBillProductPrice { get; set; } = "";
        public string goodsReceiptBillProductCompany { get; set; } = "";
        public string goodsReceiptBillProductDeliver { get; set; } = "";
        public string goodsReceiptBillCreateDate { get; set; } = "";
        public GoodsReceiptBill()
        {
            goodsReceiptBillCode = Utils.GenerateString();
        }
    }
}

namespace DoAn_KTLT.Models
{
    public class Invoice
    {
        public string invoiceCode { get; set; }
        public string invoiceCreateDate { get; set; }
    }


    public class InvoiceViewModel
    {
        public Invoice[] invoiceList = {};
    }


}

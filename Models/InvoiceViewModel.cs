namespace DoAn1.Models
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

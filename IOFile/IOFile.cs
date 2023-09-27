using DoAn1.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DoAn1.IOFile
{
    public class IOFile
    {
        public static void SaveProduct(Product product)
        {
            FileStream fs = new FileStream("product.txt", FileMode.Append, FileAccess.Write);
            StreamWriter w = new StreamWriter(fs);
            string json = JsonSerializer.Serialize(product);
            w.WriteLine(json);
            w.Flush();
            fs.Close();

        }

        public static void SaveProducts(List<Product> products)
        {
            FileStream fs = new FileStream("product.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter w = new StreamWriter(fs);

            foreach (Product product in products)
            {
                string json = JsonSerializer.Serialize(product);
                w.WriteLine(json);
                w.Flush();
            }

            fs.Close();

        }
        public static List<Product> ReadProduct()
        {

            List<Product> readProductList = new List<Product>();
            var lines = File.ReadLines("product.txt");
            foreach (var line in lines)
            {
                Product product = JsonSerializer.Deserialize<Product>(line);
                readProductList.Add(product);
            }
            return readProductList;
        }

        public static void SaveCategory(Category category)
        {
            FileStream fs = new FileStream("category.txt", FileMode.Append, FileAccess.Write);
            StreamWriter w = new StreamWriter(fs);
            string json = JsonSerializer.Serialize(category);
            w.WriteLine(json);
            w.Flush();
            fs.Close();

        }
        public static List<Category> ReadCategory()
        {

            List<Category> readCategoryList = new List<Category>();
            var lines = File.ReadLines("category.txt");
            foreach (var line in lines)
            {
                Category category = JsonSerializer.Deserialize<Category>(line);
                readCategoryList.Add(category);
            }
            return readCategoryList;
        }

        public static void SaveInvoice(Invoice invoice)
        {
            FileStream fs = new FileStream("invoice.txt", FileMode.Append, FileAccess.Write);
            StreamWriter w = new StreamWriter(fs);
            string json = JsonSerializer.Serialize(invoice);
            w.WriteLine(json);
            w.Flush();
            fs.Close();
        }
        public static List<Invoice> ReadInvoice()
        {
            List<Invoice> readInvoiceList = new List<Invoice>();
            var lines = File.ReadLines("invoice.txt");
            foreach (var line in lines)
            {
                Invoice invoice = JsonSerializer.Deserialize<Invoice>(line);
                readInvoiceList.Add(invoice);
            }
            return readInvoiceList;
        }

        public static void SaveGoodsReceiptBill(GoodsReceiptBill goodsReceiptBill)
        {
            FileStream fs = new FileStream("goodsReceiptBill.txt", FileMode.Append, FileAccess.Write);
            StreamWriter w = new StreamWriter(fs);
            string json = JsonSerializer.Serialize(goodsReceiptBill);
            w.WriteLine(json);
            w.Flush();
            fs.Close();
        }
        public static List<GoodsReceiptBill> ReadGoodsReceiptBill()
        {
            List<GoodsReceiptBill> readGoodsReceiptBillList = new List<GoodsReceiptBill>();
            var lines = File.ReadLines("goodsReceiptBill.txt");
            foreach (var line in lines)
            {
                GoodsReceiptBill goodsReceiptBill = JsonSerializer.Deserialize<GoodsReceiptBill>(line);
                readGoodsReceiptBillList.Add(goodsReceiptBill);
            }
            return readGoodsReceiptBillList;
        }
    }
}

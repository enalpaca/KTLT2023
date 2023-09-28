﻿using DoAn_KTLT.Models;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DoAn_KTLT.IOFile
{
    public class IOFile
    {
        // https://learn.microsoft.com/en-us/dotnet/api/system.io.binarywriter?view=net-7.0
        public static void Save<T>(string fileName, List<T> list)
        {
            string json = JsonSerializer.Serialize(list, jsonSerializerOptions);
            File.WriteAllText(fileName, json);
        }

        public static List<T> Load<T>(string fileName)
        {
            var list = new List<T>();

            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                if (json != "")
                {
                    return JsonSerializer.Deserialize<List<T>>(json);
                }
            }
            return list;
        }

        // https://stackoverflow.com/questions/58003293/dotnet-core-system-text-json-unescape-unicode-string
        public static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };
        public static void SaveProduct(Product product)
        {
            FileStream fs = new FileStream("product.txt", FileMode.Append, FileAccess.Write);
            StreamWriter w = new StreamWriter(fs);
            string json = JsonSerializer.Serialize(product, jsonSerializerOptions);
            w.WriteLine(json);
            w.Flush();
            fs.Close();

        }

        public static void SaveProducts(List<Product> products)
        {

            Save<Product>("product.bin", products);
            /* FileStream fs = new FileStream("product.txt", FileMode.OpenOrCreate, FileAccess.Write);
             StreamWriter w = new StreamWriter(fs);

             foreach (Product product in products)
             {
                 string json = JsonSerializer.Serialize(product, jsonSerializerOptions);
                 w.WriteLine(json);
                 w.Flush();
             }

             fs.Close();*/

        }
        public static List<Product> ReadProduct()
        {
            return Load<Product>("product.bin");
            /*  List<Product> readProductList = new List<Product>();
              var lines = File.ReadLines("product.txt");
              foreach (var line in lines)
              {
                  Product product = JsonSerializer.Deserialize<Product>(line, jsonSerializerOptions);
                  readProductList.Add(product);
              }
              return readProductList;*/
        }

        public static void SaveCategory(Category category)
        {
            FileStream fs = new FileStream("category.txt", FileMode.Append, FileAccess.Write);
            StreamWriter w = new StreamWriter(fs);
            string json = JsonSerializer.Serialize(category, jsonSerializerOptions);
            w.WriteLine(json);
            w.Flush();
            fs.Close();

        }
        public static void SaveCategories(List<Category> categories)
        {

            Save<Category>("category.bin", categories);

        }
        public static List<Category> ReadCategory()
        {
            return Load<Category>("category.bin");
        }

        public static void SaveInvoice(Invoice invoice)
        {
            FileStream fs = new FileStream("invoice.txt", FileMode.Append, FileAccess.Write);
            StreamWriter w = new StreamWriter(fs);
            string json = JsonSerializer.Serialize(invoice, jsonSerializerOptions);
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
                Invoice invoice = JsonSerializer.Deserialize<Invoice>(line, jsonSerializerOptions);
                readInvoiceList.Add(invoice);
            }
            return readInvoiceList;
        }

        public static void SaveGoodsReceiptBill(GoodsReceiptBill goodsReceiptBill)
        {
            FileStream fs = new FileStream("goodsReceiptBill.txt", FileMode.Append, FileAccess.Write);
            StreamWriter w = new StreamWriter(fs);
            string json = JsonSerializer.Serialize(goodsReceiptBill, jsonSerializerOptions);
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
                GoodsReceiptBill goodsReceiptBill = JsonSerializer.Deserialize<GoodsReceiptBill>(line, jsonSerializerOptions);
                readGoodsReceiptBillList.Add(goodsReceiptBill);
            }
            return readGoodsReceiptBillList;
        }
    }
}
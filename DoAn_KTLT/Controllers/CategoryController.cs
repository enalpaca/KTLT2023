using DoAn_KTLT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace DoAn_KTLT.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string searchText, string confirmOnDelete, string deletedCategoryCode, int? page)
        {
            List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();

            if (searchText != null && searchText != "")
            {
                ReadListCategory = ReadListCategory.FindAll(p => p.CategoryCode.Contains(searchText) || p.CategoryName.Contains(searchText));
            }

            if (confirmOnDelete == "true")
            {
                ViewBag.ProductListOnDeletedCategory = ReadListProduct.FindAll(p => p.ProductCategory == deletedCategoryCode).ToArray();
            }

            int totalPage = IOFile.Utils.CalculateNumberOfPage(ReadListCategory.Count, PAGE_SIZE);
            int currentPage = page ?? 1;

            //Paging => get data from currentPage - 1 * pageSize to (currentPage * pageSize) + pageSize
            IEnumerable<Category> listCategory = ReadListCategory.Skip((currentPage - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE);

            ViewBag.totalPage = totalPage;
            ViewBag.currentPage = currentPage;
            ViewBag.totalRow = ReadListCategory.Count();
            ViewBag.CategoryList = listCategory.ToArray();
            ViewBag.searchText = searchText ?? "";

            return View();
        }

        public IActionResult EditCategory(string CategoryCode)
        {
            List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();
            Category? category = ReadListCategory.Find(x => x.CategoryCode == CategoryCode);
            ViewBag.category = category;
            return View();
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ActionName("CreateCategory")]
        public ActionResult CreateCategory(Category newCategory)
        {

            List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();

            ReadListCategory.Add(newCategory);
            IOFile.IOFile.SaveCategories(ReadListCategory);
            return Redirect("/Category");
        }

        [HttpPost]
        [ActionName("EditCategory")]
        public ActionResult EditCategory(Category updatedCategory)
        {

            List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();

            int categoryIndex = ReadListCategory.FindIndex(x => x.CategoryCode == updatedCategory.CategoryCode);

            if (categoryIndex >= 0)
            {
                ReadListCategory.RemoveAt(categoryIndex);
                ReadListCategory.Insert(categoryIndex, updatedCategory);
                IOFile.IOFile.SaveCategories(ReadListCategory);
            }

            return Redirect("/Category");
        }

        [HttpPost]
        [ActionName("DeleteCategory")]
        public ActionResult DeleteCategory(string CategoryCode, string comfirmed)
        {

            if (comfirmed == "true")
            {
                List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();
                int categoryIndex = ReadListCategory.FindIndex(x => x.CategoryCode == CategoryCode);

                if (categoryIndex >= 0)
                {
                    ReadListCategory.RemoveAt(categoryIndex);
                    IOFile.IOFile.SaveCategories(ReadListCategory);
                }

                return Redirect("/Category");
            }

            return Redirect("/Category?confirmOnDelete=true&deletedCategoryCode=" + CategoryCode);
        }

        [HttpPost]
        [ActionName("SearchCategory")]

        public ActionResult SearchCategory(string searchText)
        {
            var encodedLocationName = WebUtility.UrlEncode(searchText);
            return Redirect($"/Category?searchText={encodedLocationName}");
        }
    }
}

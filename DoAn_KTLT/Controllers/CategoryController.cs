﻿using DoAn_KTLT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;



namespace DoAn_KTLT.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string searchText)
        {
            List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();
            if (searchText != null && searchText != "")
            {
                ReadListCategory = ReadListCategory.FindAll(p => p.CategoryCode.Contains(searchText) || p.CategoryName.Contains(searchText));
            }
            ViewBag.CategoryList = ReadListCategory.ToArray();
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
            try
            {
                List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();

                ReadListCategory.Add(newCategory);
                IOFile.IOFile.SaveCategories(ReadListCategory);
                return Redirect("/Category");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("EditCategory")]
        public ActionResult EditCategory(Category updatedCategory)
        {
            try
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
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("DeleteCategory")]
        public ActionResult DeleteCategory(string CategoryCode)
        {
            try
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
            catch
            {
                return View();
            }
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

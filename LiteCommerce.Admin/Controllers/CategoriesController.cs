using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
  [Authorize]
  public class CategoriesController : Controller
  {
    // GET: Categories
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult List(int page = 1, string searchValue = "")
    {
      int rowCount = 0;
      int pageSize = 7;
      var listOfCategories = DataService.ListCategories(page, pageSize, searchValue, out rowCount);
      var model = new Models.CategoryPaginationQueryResult()
      {
        Page = page,
        PageSize = pageSize,
        RowCount = rowCount,
        SearchValue = searchValue,
        Data = listOfCategories
      };

      return View(model);
    }

    public ActionResult Add()
    {
      ViewBag.Title = "Tạo mới Danh Mục Loại Hàng";
      Category model = new Category()
      {
        CategoryID = 0
      };
      return PartialView(model);
    }

    public ActionResult Edit(int id)
    {
      ViewBag.Title = "Cập nhật Thông Tin Danh Mục";
      var model = DataService.GetCategory(id);
      if (model == null)
        return RedirectToAction("Index");
      return PartialView(model);
    }

    public ActionResult Delete(int id)
    {
      ViewBag.Title = "Xác nhận xóa mặt hàng";
      var model = DataService.GetCategory(id);
      if (model == null)
        return RedirectToAction("Index");
      if (Request.HttpMethod == "GET")
        return PartialView(model);
      DataService.DeleteCategory(id);
      return RedirectToAction("Index");
    }

    public ActionResult Save(Category data)
    {
      try
      {

        if (string.IsNullOrWhiteSpace(data.CategoryName))
          ModelState.AddModelError("CategoryName", "Vui lòng nhập tên của loại hàng!");
        if (data.ParentCategoryId.Equals(""))
          ModelState.AddModelError("ParentCategoryId", "Bạn chưa nhập ParentCategoryId!");

        if (string.IsNullOrEmpty(data.Description))
          data.Description = "";
        if (!ModelState.IsValid)
        {
          if (data.CategoryID == 0)
            ViewBag.Title = "Tạo mới danh mục loại hàng";
          else
            ViewBag.Title = "Thay đổi thông tin danh mục";
          return PartialView("Add", data);
        }

        if (data.CategoryID == 0)
        {
          DataService.AddCategory(data);
        }
        else
        {
          DataService.UpdateCategory(data);
        }
        return RedirectToAction("Index", "Categories");
        // return Json(data);
      }
      catch
      {
        return Content("Oops! Trang này hình như không tồn tại :)");
      }
    }
  }
}

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
  public class SupplierController : Controller
  {
    // GET: Suppliers
    /*    public ActionResult Index(int page = 1, string searchValue = "")
        {
          //int rowCount = 0;
          //int pageSize = 10;

          //var listOfSuppliers = DataService.ListSuppliers(page, pageSize, searchValue, out rowCount);
          //int pageCount = rowCount / pageSize;
          //if (rowCount % pageSize > 0)
          //  pageCount += 1;
          //// 3 cach đẩy du lieu từ controler ra view, viewbag, viewdata, model
          //ViewBag.Page = page;
          //ViewBag.RowCount = rowCount;
          //ViewBag.PageCount = pageCount;
          //ViewBag.SearchValue = searchValue;

          //return View(listOfSuppliers);

          int rowCount = 0;
          int pageSize = 7;
          var listOfSuppliers = DataService.ListSuppliers(page, pageSize, searchValue, out rowCount);
          var model = new Models.SupplierPaginationQueryResult()
          {
            Page = page,
            PageSize = pageSize,
            SearchValue = searchValue,
            RowCount = rowCount,
            Data = listOfSuppliers
          };
          return View(model);
        }*/


    /// <summary>
    /// dung ajax de show index va tim kiem de toi uu trang web
    /// </summary>
    /// <returns></returns>
    public ActionResult Index()
    {
      return View();
    }


    public ActionResult List(int page = 1, string searchValue = "")
    {
      try
      {
        int rowCount = 0;
        int pageSize = 7;
        var listOfSuppliers = DataService.ListSuppliers(page, pageSize, searchValue, out rowCount);
        var model = new Models.SupplierPaginationQueryResult()
        {
          Page = page,
          PageSize = pageSize,
          SearchValue = searchValue,
          RowCount = rowCount,
          Data = listOfSuppliers
        };
        return View(model);
      }
      catch (Exception e)
      {
        return Content(e.Message);
      }
    }


    public ActionResult Add()
    {
      ViewBag.Title = "Tạo mới Nhà Cung Cấp";

      Supplier model = new Supplier()
      {
        SupplierID = 0
      };
      return View("Edit", model);
    }

    public ActionResult Edit(int id)
    {
      ViewBag.Title = "Chỉnh sửa thông tin Nhà Cung Cấp";

      var model = DataService.GetSupplier(id);
      if (model == null)
        RedirectToAction("Index");

      return View(model);
    }

    public ActionResult delete(int id)
    {
      try
      {
        if (Request.HttpMethod == "POST")
        {
          //Xóa supplier có mã là id
          DataService.DeleteSupplier(id);
          // Quay về lại trang index
          return RedirectToAction("Index");
        }
        else
        {
          //lấy thông tin của supplier cần xóa
          var model = DataService.GetSupplier(id);
          if (model == null)
            return RedirectToAction("Index");
          //trả thông tin về cho view hiển thị
          return View(model);
        }
      }
      catch
      {
        return Content("Trang này không tồn tại");
      }

    }

    public ActionResult Save(Supplier data)
    {
      try
      {

        if (string.IsNullOrWhiteSpace(data.SupplierName))
          ModelState.AddModelError("SupplierName", "Vui lòng nhập tên của nhà cung cấp!");
        if (string.IsNullOrWhiteSpace(data.ContactName))
          ModelState.AddModelError("ContactName", "Bạn chưa nhập tên liên hệ của nhà cung cấp!");
        if (string.IsNullOrWhiteSpace(data.Address))
          ModelState.AddModelError("Address", "Bạn chưa nhập địa chỉ của nhà cung cấp!");
        if (string.IsNullOrEmpty(data.Country))
          data.Country = "";
        if (string.IsNullOrEmpty(data.City))
          data.City = "";
        if (string.IsNullOrEmpty(data.PostalCode))
          data.PostalCode = "";
        if (string.IsNullOrEmpty(data.Phone))
          data.Phone = "";
        if (!ModelState.IsValid)
        {
          if (data.SupplierID == 0)
            ViewBag.Title = "Tạo mới Nhà Cung Cấp";
          else
            ViewBag.Title = "Thay đổi thông tin nhà cung cấp";
          return View("Edit", data);
        }

        if (data.SupplierID == 0)
        {
          DataService.AddSupplier(data);
        }
        else
        {
          DataService.UpdateSupplier(data);
        }
        return RedirectToAction("Index");
        // return Json(data);
      }
      catch
      {
        return Content("Oops! Trang này hình như không tồn tại :)");
      }

    }
  }
}

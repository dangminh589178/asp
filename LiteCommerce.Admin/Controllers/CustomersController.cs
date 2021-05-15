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
  public class CustomersController : Controller
  {
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    // GET: Customers
    public ActionResult Index(int page = 1, string searchValue = "")
    {
     
      return View();
    }

    public ActionResult List(int page = 1, string searchValue = "")
    {
      int rowCount = 0;
      int pageSize = 7;
      var listOfCustomers = DataService.ListCustomers(page, pageSize, searchValue, out rowCount);
      var model = new Models.CustomerPaginationQueryResult()
      {
        Page = page,
        PageSize = pageSize,
        SearchValue = searchValue,
        RowCount = rowCount,
        Data = listOfCustomers
      };
      return View(model);
    }

    public ActionResult Add()
    {
      ViewBag.Title = "Tạo mới khách hàng";
      Customer model = new Customer()
      {
        CustomerID = 0
      };
      return View("Edit", model);
    }

    public ActionResult Edit(int id)
    {
      ViewBag.Title = "Chỉnh sửa thông tin khách hàng";
      var model = DataService.GetCustomer(id);
      if (model == null)
        RedirectToAction("Index");
      return View(model);
    }

    public ActionResult Delete(int id)
    {
      try {
         if(Request.HttpMethod =="POST")
        {
          DataService.DeleteCustomer(id);
          return RedirectToAction("Index");
        }
        else
        {
          var model = DataService.GetCustomer(id);
          if (model == null)
            return RedirectToAction("Index");
          return View(model);
        }
      } catch
      {
        return Content("Oops! Trang này hình như không tồn tại :)");
      }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>

    public ActionResult Save(Customer data)
    {
      try
      {

        if (string.IsNullOrWhiteSpace(data.CustomerName))
          ModelState.AddModelError("CustomerName", "Vui lòng nhập tên của nhà cung cấp!");
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
        if (string.IsNullOrEmpty(data.Email))
          data.Email = "";
        if (string.IsNullOrEmpty(data.Password))
          data.Password = "";
        if (!ModelState.IsValid)
        {
          if (data.CustomerID == 0)
            ViewBag.Title = "Tạo mới khách hàng";
          else
            ViewBag.Title = "Thay đổi thông tin khách hàng";
          return View("Edit", data);
        }

        if (data.CustomerID == 0)
        {
          DataService.AddCustomer(data);
        }
        else
        {
          DataService.UpdateCustomer(data);
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

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
  public class ShippersController : Controller
  {
    /// <summary>
    /// tìm kiếm và hiển thị danh sách nhà vận chuyển
    /// </summary>
    /// <returns></returns>
    // GET: Shippers
    public ActionResult Index()
    {
     
      return View();
    }

    public ActionResult List(int page = 1, string searchValue = "")
    {
      int rowCount = 0;
      int pageSize = 7;
      var listOfShippers = DataService.ListShippers(page, pageSize, searchValue, out rowCount);
      var model = new Models.ShipperPaginationQueryResult()
      {
        Page = page,
        PageSize = pageSize,
        SearchValue = searchValue,
        RowCount = rowCount,
        Data = listOfShippers
      };
      return View(model);
    }

    public ActionResult Add()
    {
      ViewBag.Title = "Tạo mới nhà vận chuyển";
      Shipper model = new Shipper()
      {
        ShipperID = 0
      };
      return PartialView(model);
    }

    public ActionResult Edit(int id)
    {
      ViewBag.Title = "Chỉnh sửa nhà vận chuyển";
      Shipper model = DataService.GetShipper(id);
      if (model == null)
        return RedirectToAction("Index");
      return PartialView(model);
    }

    public ActionResult Delete(int id)
    {
      ViewBag.Title = "Xác nhận xóa nhà vận chuyển";
      var model = DataService.GetShipper(id);
      if(model == null)
      {
        return RedirectToAction("Index");
      }
      if(Request.HttpMethod == "GET")
      {
        return PartialView(model);
      }
      DataService.DeleteShipper(id);
      return RedirectToAction("Index");
    }

    public ActionResult Save(Shipper data)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(data.ShipperName))
          ModelState.AddModelError("ShipperName", "Bạn chưa nhập tên nhà vận chuyển");
        if (string.IsNullOrWhiteSpace(data.Phone))
          ModelState.AddModelError("Phone", "Bạn chưa nhập số điện thoại");
        if (data.ShipperID == 0)
        {
          DataService.AddShipper(data);
        }
        else
        {
          DataService.UpdateShipper(data);
        }
        return RedirectToAction("Index", "Shippers");
      }
      catch
      {
        return Content ("Opp, Some things went wrong!, try again!");
      }
    }
  }
}

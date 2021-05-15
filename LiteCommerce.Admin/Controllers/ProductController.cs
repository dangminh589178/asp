using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
  /// <summary>
  /// 
  /// </summary>
  [Authorize]
  public class ProductController : Controller
  {
    // GET: Product
    public ActionResult Index()
    {
      return View();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ActionResult List(int CategoryID = 0, int SupplierID = 0, string searchValue = "", int page = 1)
    {
      try
      {
        int rowCount = 0;
        int pageSize = 6;
        var listOfProducts = ProductService.List(page, pageSize, CategoryID, SupplierID, searchValue, out rowCount);
        var model = new Models.ProductPaginationQueryResult()
        {
          Page = page,
          PageSize = pageSize,
          SearchValue = searchValue,
          RowCount = rowCount,
          Data = listOfProducts
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
      ViewBag.Title = "Tạo mới Sản Phẩm";
      Product model = new Product()
      {
        ProductID = 0
      };
      return View("Edit", model);
    }
    public ActionResult Edit(int id)
    {
      ViewBag.Title = "Chỉnh sửa thông tin Sản Phẩm";
      var model = ProductService.Get(id);
      if (model == null)
        RedirectToAction("Index");
      return View(model);
    }

        public ActionResult Editex(int id)
        {
            ViewBag.Title = "Chỉnh sửa thông tin Sản Phẩm";
            var model = ProductService.GetEx(id);
            if (model == null)
                RedirectToAction("Index");
            return View(model);
        }

        public string ProcessUpload(HttpPostedFileBase file)
    {
      file.SaveAs(Server.MapPath("~/Controllers/Content/ProductImages/" + file.FileName));
      return "/Controllers/Content/ProductImages/" + file.FileName;
    }


    public ActionResult Delete(int id)
    {
      if(Request.HttpMethod == "POST")
      {
        ProductService.Delete(id);
        return RedirectToAction("Index");
      }
      else
      {
        var model = ProductService.Get(id);
        if (model == null)
          return RedirectToAction("Index");
        return View(model);
      }


    }

    public ActionResult Save(Product data)
    {
     
/*      try {*/
        if (string.IsNullOrWhiteSpace(data.ProductName))
          ModelState.AddModelError("ProductName", "Vui lòng nhập tên sản phẩm");
        if (string.IsNullOrWhiteSpace(data.Unit))
          ModelState.AddModelError("Unit", "Bạn chưa nhập đơn vị tính!");
        if (string.IsNullOrWhiteSpace(data.Photo))
          ModelState.AddModelError("Photo", "Vui lòng chọn ảnh sản phẩm");
        if (!ModelState.IsValid)
        {
          if (data.ProductID == 0)
            ViewBag.Title = "Thêm mới Sản Phẩm";
          else
            ViewBag.Title = "Thay đổi thông tin sản phẩm";
          return View("Edit", data);
        }
        if (data.ProductID == 0)
        {
          ProductService.Add(data);
        }
        else
        {
          ProductService.Update(data);
        }
        return RedirectToAction("Index");
     /* }
      catch
      {
        return Content("Oops! Trang này hình như không tồn tại :)");
      }*/
    }
  }
}

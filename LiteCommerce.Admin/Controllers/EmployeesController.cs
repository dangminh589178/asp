using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;


namespace LiteCommerce.Admin.Controllers
{
  [Authorize]
  public class EmployeesController : Controller
  {
    // GET: Employees
    public ActionResult Index( )
    {
      return View();
    }

    public ActionResult List(int page = 1, string searchValue = "")
    {
      int rowCount = 0;
      int pageSize = 2;
      var listOfEmployee = DataService.ListEmployees(page, pageSize, searchValue, out rowCount);
      var model = new Models.EmployeePaginationQueryResult()
      {
        Page = page,
        PageSize = pageSize,
        SearchValue = searchValue,
        RowCount = rowCount,
        Data = listOfEmployee
      };
      return View(model);
    }

    public ActionResult Add()
    {
      ViewBag.Title = "Tạo mới nhân viên";
      Employee model = new Employee()
      {
        EmployeeID = 0
      };
      
      return View("Edit", model);
    }

    public ActionResult Edit(int id)
    {
      ViewBag.Title = "Chỉnh sửa nhân viên";
      var model = DataService.GetEmployee(id);
      if (model == null)
        return RedirectToAction("Index");

      return View(model);
    }

    /// <summary>
    /// Action xử lý upload ảnh
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public string ProcessUpload(HttpPostedFileBase file)
    {
      file.SaveAs(Server.MapPath("~/Controllers/Content/Images/" + file.FileName));
      return "/Controllers/Content/Images/" + file.FileName;
    }


    public ActionResult Delete(int id)
    {
      if (Request.HttpMethod == "POST")
      {
        DataService.DeleteEmployee(id);
        return RedirectToAction("Index");
      }
      else
      {
        var model = DataService.GetEmployee(id);
        if (model == null)
          return RedirectToAction("index");
        return View(model);
      }
    }

    public ActionResult Save(Employee data)
    {
      try {

        if (string.IsNullOrWhiteSpace(data.FirstName))
          ModelState.AddModelError("FirstName", "Vui lòng nhập họ của nhân viên!");
        if (string.IsNullOrWhiteSpace(data.LastName))
          ModelState.AddModelError("LastName", "Vui lòng nhập tên của nhân viên!");
        if (string.IsNullOrWhiteSpace(data.BirthDate))
          ModelState.AddModelError("BirthDate", "Bạn chưa nhập ngày sinh!");
        if (string.IsNullOrWhiteSpace(data.Photo))
          ModelState.AddModelError("Photo", "Bạn chưa chọn ảnh cho nhân viên !");
        if (string.IsNullOrEmpty(data.Photo)) data.Photo = @"Piture.png";
        if (string.IsNullOrEmpty(data.Notes))
          data.Notes = "";
        if (string.IsNullOrEmpty(data.Email))
          data.Email = "";
        if (string.IsNullOrEmpty(data.Password))
          data.Password = "";
        
        if (!ModelState.IsValid)
        {
          if (data.EmployeeID == 0)
            ViewBag.Title = "Tạo mới Nhân Viên";
          else
            ViewBag.Title = "Thay đổi thông tin nhân viên";
          return View("Edit", data);
        }

        if (data.EmployeeID == 0)
        {
          DataService.AddEmployee(data);
        }
        else
        {
          DataService.UpdateEmployee(data);
        }
        return RedirectToAction("Index");
      } catch
      {
        return Content("Oops! Trang này hình như không tồn tại :)");
      }
      
    }
  }
}

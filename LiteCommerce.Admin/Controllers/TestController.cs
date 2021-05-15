using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LiteCommerce.DataLayers;
using LiteCommerce.DataLayers.SQLServer;
using LiteCommerce.DomainModels;
namespace LiteCommerce.Admin.Controllers
{
  public class TestController : Controller
  {
    /// <summary>
    /// Hiển Thị Tên Các Thành Phố Của USA có trong CSDL
    /// </summary>
    /// <returns></returns>
    // GET: Test
    public ActionResult Index()
    {
      string connectionString = ConfigurationManager.ConnectionStrings["LiteCommerceDB"].ConnectionString;
      /* ICountryDAL dal = new DataLayers.SQLServer.CountryDAL(connectionString);
       var data = dal.List();*/
      /*CityDAL dal = new DataLayers.SQLServer.CityDAL(connectionString);
      var data = dal.List("USA");*/
      /*ISupplierDAL dal = new DataLayers.SQLServer.SupplierDAL(connectionString);
       var data = dal.List();*/

      ISupplierDAL dal = new SupplierDAL(connectionString);
      // var data = dal.Count("");// Count searchValue neu ko co gia tri thi count het
      // var data = dal.Get(1); // lay mot supplier khi id = 1;

      Supplier data = new Supplier()
      {
        SupplierID = 35,
        SupplierName = "Huỳnh Hải Hòa Dep Trai",
        ContactName = "HoaK41",
        Address = "41 Nguyễn Khoa Chiêm, phường An Tây, TP Huế",
        Country = "Việt Nam",
        City = "Thừa Thiên Huế",
        PostalCode = "5039",
        Phone = "0971197955"
      };
      return Json(dal.Update(data), JsonRequestBehavior.AllowGet); // cap nhat supplier
      /*var data = dal.Delete(33);// xoa du lieu supplier co id 33
      return Json(data, JsonRequestBehavior.AllowGet);*/
    }

    /// <summary>
    /// Hiển Thị List, Tìm Kiếm Và Phân Trang Của Trang Nhà Cung Cấp
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchValue"></param>
    /// <returns></returns>
    public ActionResult Pagination(int page, int pageSize, string searchValue)
    {
      string connectionString = ConfigurationManager.ConnectionStrings["LiteCommerceDB"].ConnectionString;
      ISupplierDAL dal = new SupplierDAL(connectionString);
      var data = dal.List(page, pageSize, searchValue);

      return Json(data, JsonRequestBehavior.AllowGet);
    }

    //Test URL: https://localhost:44398/Test/Pagination?page=1&pageSize=10&searchValue=

    /// <summary>
    /// Hiển Thị List ,Tìm kiếm , và phân trang của trang Danh Mục Loại Hàng
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchValue"></param>
    /// <returns></returns>
    public ActionResult PaginationCategory(int page, int pageSize, string searchValue)
    {
      string connectionString = ConfigurationManager.ConnectionStrings["LiteCommerceDB"].ConnectionString;
      ICategoryDAL dal = new CategoryDAL(connectionString);
      //var data = dal.Count("");
      //var data = dal.Get(1);
      var data = dal.Delelte(9);
     /* Category data = new Category()
      {
        CategoryID = 1, 
        CategoryName = "Beverages",
        Description = "Soft drinks, coffees, teas, beers, and ales",
        ParentCategoryId = 2
      };
      return Json(dal.Update(data), JsonRequestBehavior.AllowGet);*/
      return Json(data, JsonRequestBehavior.AllowGet);
    }

    //Test URL: https://localhost:44398/Test/PaginationCategory?page=1&pageSize=10&searchValue=

    /// <summary>
    /// Hiển thị list khách hàng, tìm kiếm và phân trang 
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchValue"></param>
    /// <returns></returns>
    public ActionResult PaginationCustomer(int page, int pageSize, string searchValue)
    {
      string connectionString = ConfigurationManager.ConnectionStrings["LiteCommerceDB"].ConnectionString;
      ICustomerDAL dal = new CustomerDAL(connectionString);
      //var data = dal.List(page, pageSize, searchValue);
      //var data = dal.Count("");
      //var data = dal.Delete(95);
     // var data = dal.Get(2);
      //return Json(data, JsonRequestBehavior.AllowGet);

      Customer data = new Customer()
      {
        CustomerID = 96,
        CustomerName = "Huỳnh Hải Hòa",
        ContactName = "Hòa Minzy hihi",
        Address = "Tam Sơn Núi Thành Quảng Nam",
        PostalCode = "5039",
        Country = "Việt Nam",
        City = "Quảng Nam",
        Email = "Huynhhaihoak41@gmail.com",
        Password = "123"
      };

      return Json(dal.Update(data), JsonRequestBehavior.AllowGet);
    }

    //Test URL: https://localhost:44398/Test/PaginationCustomer?page=1&pageSize=10&searchValue

    /// <summary>
    /// Hiển thị danh sách Shipper(nhà vận chuyển) Tìm kiếm, phân trang
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchValue"></param>
    /// <returns></returns>
    public ActionResult PaginationShipper(int page, int pageSize, string searchValue)
    {
      string connectionString = ConfigurationManager.ConnectionStrings["LiteCommerceDB"].ConnectionString;
      IShipperDAL dal = new ShipperDAL(connectionString);
      /*var data = dal.List(page, pageSize, searchValue);

      return Json(data, JsonRequestBehavior.AllowGet);*/

      Shipper data = new Shipper()
      {
        ShipperID = 4,
        ShipperName = "Lệ Quyên",
        Phone = "0988712"
      };
      //var data = dal.Count("");
      //var data = dal.Get(4);
      return Json(dal.Update(data), JsonRequestBehavior.AllowGet);
    }

    //Test URL: https://localhost:44398/Test/PaginationShipper?page=1&pageSize=10&searchValue=


    /// <summary>
    /// Hiển thị danh sách Employee(Nhaan Vien) Tìm kiếm, phân trang
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchValue"></param>
    /// <returns></returns>
    public ActionResult PaginationEmployee(int page, int pageSize, string searchValue)
    {
      string connectionString = ConfigurationManager.ConnectionStrings["LiteCommerceDB"].ConnectionString;
      IEmployeeDAL dal = new EmployeeDAL(connectionString);
      //var data = dal.List(page, pageSize, searchValue);
      var data = dal.Count("");
      return Json(data, JsonRequestBehavior.AllowGet);
 /*     Employee data = new Employee()
      {
        LastName = "Huynh",
        FirstName = "Hoa",
        BirthDate = "11/11/1999",
        Photo = "abc",
        Notes = "deptrai",
        Email = "employ@gmail.com",
        Password = "12"
      };
      return Json(dal.Add(data), JsonRequestBehavior.AllowGet);*/
    }

    //Test URL: https://localhost:44398/Test/PaginationEmployee?page=1&pageSize=10&searchValue=
  }
}

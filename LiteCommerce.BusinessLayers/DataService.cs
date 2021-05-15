
using System;
using System.Collections.Generic;
using LiteCommerce.DataLayers;
using LiteCommerce.DomainModels;

namespace LiteCommerce.BusinessLayers
{
  /// <summary>
  /// Cung cấp các chức năng nghiệp vụ liên quan đến quản lý dữ liệu chung
  /// </summary>
  public static class DataService
  {
    // 
    private static ICountryDAL CountryDB;
    private static ICityDAL CityDB;
    private static ISupplierDAL SupplierDB;
    private static ICategoryDAL CategoryDB;
    private static IShipperDAL ShipperDB;
    private static ICustomerDAL CustomerDB;
    private static IEmployeeDAL EmployeeDB;

    /// <summary>
    /// Khởi tạo tính năng tác nghiệp (hàm này phải được gọi nếu muốn sử dụng các tính năng của lớp)
    /// </summary>
    /// <param name="dbType"></param>
    /// <param name="connectionString">Chuỗi kết nối</param>
    public static void Init(DatabaseTypes dbType, string connectionString)
    {
      // factory (design parten)
      switch (dbType)
      {
        case DatabaseTypes.SQLServer:
          CountryDB = new DataLayers.SQLServer.CountryDAL(connectionString);
          CityDB = new DataLayers.SQLServer.CityDAL(connectionString);
          SupplierDB = new DataLayers.SQLServer.SupplierDAL(connectionString);
          CategoryDB = new DataLayers.SQLServer.CategoryDAL(connectionString);
          CustomerDB = new DataLayers.SQLServer.CustomerDAL(connectionString);
          ShipperDB = new DataLayers.SQLServer.ShipperDAL(connectionString);
          EmployeeDB = new DataLayers.SQLServer.EmployeeDAL(connectionString);
          break;
        default:
          throw new Exception("Database Type is not Supported");
      }
    }

    /// <summary>
    /// Danh sách các quốc gia
    /// </summary>
    /// <returns></returns>
    public static List<Country> ListCountries()
    {
      return CountryDB.List();
    }

    /// <summary>
    /// Danh sách các thành phố
    /// </summary>
    /// <returns></returns>
    public static List<City> ListCities()
    {
      return CityDB.List();
    }


    /// <summary>
    /// Danh sách các thành phố của một quốc gia cụ thể
    /// </summary>
    /// <param name="countryName">tên quốc gia</param>
    /// <returns></returns>
    public static List<City> ListCities(string countryName)
    {
      return CityDB.List(countryName);
    }
    /// <summary>
    /// Danh sách nhà cung cấp (phân trang, tìm kiếm)
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchValue"></param>
    /// <param name="rowCount">cho bieest</param>
    /// <returns></returns>
    public static List<Supplier> ListSuppliers(int page,
                                                int pageSize,
                                                string searchValue,
                                                out int rowCount)
    {
      rowCount = SupplierDB.Count(searchValue);
      return SupplierDB.List(page, pageSize, searchValue);
    }

    public static List<Supplier> ListOfSuppliers()
    {
      return SupplierDB.ListOfSuppliers();
    }

    /// <summary>
    /// Bổ sung nhà cung cấp
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static int AddSupplier(Supplier data)
    {
      return SupplierDB.Add(data);
    }

    /// <summary>
    /// Cập nhật thông tin nhà cung cấp
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool UpdateSupplier(Supplier data)
    {
      return SupplierDB.Update(data);
    }

    /// <summary>
    /// Xóa nhà cung cấp theo mã
    /// </summary>
    /// <param name="supplierId"></param>
    /// <returns></returns>
    public static bool DeleteSupplier(int supplierId)
    {
      return SupplierDB.Delete(supplierId);
    }

    /// <summary>
    /// Lấy thông tin nhà cung cấp để chỉnh sửa
    /// </summary>
    /// <param name="supplierId"></param>
    /// <returns></returns>
    public static Supplier GetSupplier(int supplierId)
    {
      return SupplierDB.Get(supplierId);
    }
/*Phan Loai Mat Hang*/
    /// <summary>
    /// Danh sách Loại mặt hàng.  (Tìm kiếm , phân trang)
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchValue"></param>
    /// <param name="rowCount">đếm số dòng có trong cơ sở dũ liệu</param>
    /// <returns></returns>
    public static List<Category> ListCategories(int page,
                                                int pageSize,
                                                string searchValue,
                                                out int rowCount)
    {
      rowCount = CategoryDB.Count(searchValue);
      return CategoryDB.List(page, pageSize, searchValue);
    }

    public static List<Category> ListOfCategories()
    {
      return CategoryDB.ListOfCategories();
    }

    public static Category GetCategory(int CategoryID)
    {
      return CategoryDB.Get(CategoryID);
    }

    public static int AddCategory(Category data)
    {
      return CategoryDB.Add(data);
    }
    public static bool UpdateCategory(Category data)
    {
      return CategoryDB.Update(data);
    }

    public static bool DeleteCategory(int CategoryID)
    {
      return CategoryDB.Delelte(CategoryID);
    }
/*Het Phan Loai Mat Hang*/
/* phan customer */
    /// <summary>
    /// Lấy danh sách khách hàng. (Tìm kiếm , phân trang)
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchValue"></param>
    /// <param name="rowCount">số dòng có trong csdl</param>
    /// <returns></returns>
    public static List<Customer> ListCustomers(int page,
                                                int pageSize,
                                                string searchValue,
                                                out int rowCount )
    {
      rowCount = CustomerDB.Count(searchValue);
      return CustomerDB.List(page, pageSize, searchValue);
    }
    public static Customer GetCustomer(int CustomerID)
    {
      return CustomerDB.Get(CustomerID);
    }

    public static int AddCustomer(Customer data)
    {
      return CustomerDB.Add(data);
    }

    public static bool UpdateCustomer(Customer data)
    {
      return CustomerDB.Update(data);
    }

    public static bool DeleteCustomer(int CustomerID)
    {
      return CustomerDB.Delete(CustomerID);
    }
 /*het phan Customer*/
 /*Phan Shipper*/
    /// <summary>
    /// Danh sách Nhà Vận Chuyển
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchValue"></param>
    /// <param name="rowCount"></param>
    /// <returns></returns>
    public static List<Shipper> ListShippers(int page, int pageSize, string searchValue, out int rowCount)
    {
      rowCount = ShipperDB.Count(searchValue);
      return ShipperDB.List(page, pageSize, searchValue);
    }
    public static Shipper GetShipper(int shipperID)
    {
      return ShipperDB.Get(shipperID);
    }
    public static int AddShipper(Shipper data)
    {
      return ShipperDB.Add(data);
    }

    public static bool UpdateShipper(Shipper data)
    {
      return ShipperDB.Update(data);
    }

    public static bool DeleteShipper(int shipperID)
    {
      return ShipperDB.Delete(shipperID);
    }
/*Het phan Shipper*/
/*Phan Nhan Vien (Employee)*/
    public static List<Employee> ListEmployees(int page, int pageSize, string searchValue, out int rowCount)
    {
      rowCount = EmployeeDB.Count(searchValue);
      return EmployeeDB.List(page, pageSize, searchValue);
    }

    public static Employee GetEmployee(int EmployeeID)
    {
      return EmployeeDB.Get(EmployeeID);
    }

    public static int AddEmployee(Employee data)
    {
      return EmployeeDB.Add(data);
    }

    public static bool UpdateEmployee(Employee data)
    {
      return EmployeeDB.Update(data);
    }

    public static bool DeleteEmployee(int EmployeeID)
    {
      return EmployeeDB.Delete(EmployeeID);
    }
/*Het Phan Nhan Vien (Employee)*/
  }
}

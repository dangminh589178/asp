using LiteCommerce.BusinessLayers;
using LiteCommerce.DataLayers.SQLServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//namespace LiteCommerce.Admin.Helpers.. nếu để vậy thì khi dùng phải using. bỏ helper đi ta dùng ko cần using
namespace LiteCommerce.Admin
{
  /// <summary>
  /// Cung cap cac ham tien ich lien quan den SelectListItem
  /// </summary>
  public static class SelectListHelper 
  {
    /// <summary>
    /// Danh sách các quốc gia dưới dạng value and text
    /// </summary>
    /// <returns></returns>
    public static List<SelectListItem> Countries()
    {
      List<SelectListItem> list = new List<SelectListItem>();

      foreach (var item in DataService.ListCountries())
      {
        list.Add(new SelectListItem()
        {
          Value = item.CountryName,
          Text = item.CountryName
        });
      }
      return list;
    }


    /// <summary>
    /// Danh  sách các thành phố dưới dạng value và text
    /// </summary>
    /// <returns></returns>
    public static List<SelectListItem> Cities()
    {
      List<SelectListItem> list = new List<SelectListItem>();

      foreach (var item in DataService.ListCities())
      {
        list.Add(new SelectListItem()
        {
          Value = item.CityName,
          Text = item.CityName
        });
      }
      return list;
    }

    /// <summary>
    /// Danh sách các loại hàng dưới dạng value và text
    /// </summary>
    /// <returns></returns>
    public static List<SelectListItem> Categories()
    {
      List<SelectListItem> list = new List<SelectListItem>();
      foreach (var item in DataService.ListOfCategories())
      {
        list.Add(new SelectListItem()
        {
          Value = Convert.ToString(item.CategoryID),
          Text = item.CategoryName
        });
      }
      return list;
    }


    public static List<SelectListItem> Suppliers()
    {
      List<SelectListItem> list = new List<SelectListItem>();
      foreach (var item in DataService.ListOfSuppliers())
      {
        list.Add(new SelectListItem()
        {
          Value = Convert.ToString(item.SupplierID),
          Text = item.SupplierName
        });
      }
      return list;
    }
  }
}

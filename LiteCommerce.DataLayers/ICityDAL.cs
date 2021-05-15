using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LiteCommerce.DataLayers
{
  /// <summary>
  /// Định nghĩa các phép xử lý dữ liệu liên quan đến thành phố
  /// </summary>
  public interface ICityDAL
  {
    /// <summary>
    /// Lấy danh sách các thành phố thuộc một quốc gia
    /// </summary>
    /// <returns></returns>
    List<City> List();

    /// <summary>
    /// Lấy danh sách các thành phố thuộc một quốc gia
    /// </summary>
    /// <param name="countryName">Tên của quốc gia cần lấy thành phố</param>
    /// <returns></returns>
    List<City> List(String countryName);
  }
}

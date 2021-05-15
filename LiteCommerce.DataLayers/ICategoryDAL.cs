using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
  /// <summary>
  /// Định Nghĩa Các Phép Xử Lý Liên Quan Đến Danh Mục Hàng Hóa
  /// </summary>
  public interface ICategoryDAL
  {
    /// <summary>
    /// Bổ Sung Một Danh Mục Hàng mới.Hàm trả về một danh mục Hàng
    /// nếu bổ sung thành công
    /// </summary>
    /// <param name="data">Đối tượng lưu thông tin của danh mục cần bổ sung.</param>
    /// <returns></returns>
    int Add(Category data);

    /// <summary>
    /// Lấy Danh Sách Các Danh Mục , Tìm kiếm, Phân trang
    /// </summary>
    /// <param name="page">Trang cần lấy dữ liệu</param>
    /// <param name="pageSize">số item cần hiển thị trên 1 trang</param>
    /// <param name="searchValue">giá trị cần tìm kiếm theo CategoryName, Description (Chuỗi rỗng nếu không có nhu cầu tìm kiếm)</param>
    /// <returns></returns>
    List<Category> List(int page, int pageSize, string searchValue);
 
    /// <summary>
    /// Đếm số lượng danh mục thỏa điều kiện tìm kiếm (để phân trang)
    /// </summary>
    /// <param name="searchValue">giá trị cần tìm kiếm theo CategoryName, Description (Chuỗi rỗng nếu không có nhu cầu tìm kiếm)</param>
    /// <returns></returns>
    int Count(string searchValue);

    /// <summary>
    /// Lấy Thông Tin của một danh mục hàng theo mã(dùng để edit , hoặc delete)
    /// </summary>
    /// <param name="CategoryID">Mã của danh mục hàng cần lấy thông tin</param>
    /// <returns></returns>
    Category Get(int CategoryID);
    /// <summary>
    /// Cập nhật thông tin của một danh mục hàm trả về giá trị boolen cho biết việc cập nhật có thành công hay không?
    /// </summary>
    /// <param name="data">đối tượng lưu thông tin của danh mục hàng</param>
    /// <returns></returns>
    bool Update(Category data);

    /// <summary>
    ///  Xóa Danh mục Dựa vào mã . hàm trả về giá trị boolen cho biết việc xóa có thành công hay không?
    /// </summary>
    /// <param name="CategoryID"></param>
    /// <returns></returns>
    bool Delelte(int CategoryID);

    List<Category> ListOfCategories();
  }
}

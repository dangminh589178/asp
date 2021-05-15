using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
  /// <summary>
  /// Định nghĩa các phép xử lý liên quan đến Nhà Cung Cấp
  /// </summary>
  public interface ISupplierDAL
  {
    /// <summary>
    /// Bổ sung một nhà cung cấp mới. Hàm trả về mã của nhà cung cấp
    /// nếu bổ sung thành công.
    /// </summary>
    /// <param name="data">Đối tượng lưu thông tin của nhà cung cấp cần bổ sung.</param>
    /// <returns></returns>
    int Add(Supplier data);

    /*    int[] Add(Supplier[] data) lấy thông tin nhiều nhà cung cấp*/
    /// <summary>
    /// Lấy Danh sách các nhà cung cấp(Tìm kiếm, Phân trang)
    /// </summary>
    /// <param name="page">Trang cần lấy dữ liệu</param>
    /// <param name="pageSize">Số dòng hiển thị trên một trang</param>
    /// <param name="searchValue">giá trị cần tìm kiếm theo SupplierName, ContactName, Address, Phone (Chuỗi rỗng nếu không có nhu cầu tìm kiếm)</param>
    /// <returns></returns>
    List<Supplier> List(int page, int pageSize, string searchValue);
    /// <summary>
    /// Đếm số lượng nhà cung cấp thỏa điều kiện tìm kiếm
    /// </summary>
    /// <param name="searchValue">giá trị cần tìm kiếm theo SupplierName, ContactName, Address, Phone (Chuỗi rỗng nếu không có nhu cầu tìm kiếm)</param>
    /// <returns></returns>
    int Count(string searchValue);
    /// <summary>
    /// Lấy thông tin của một nhà cung cấp theo mã. Trong trường hợp nhà cung cấp không tồn tại
    /// hàm trả về giá trị null
    /// </summary>
    /// <param name="supplierID">Mã của nhà cung cấp cần lấy thông tin.</param>
    /// <returns></returns>
    Supplier Get(int supplierID);
    /// <summary>
    /// Cập nhật thông tin của một nhà cung cấp . hàm trả về giá trị boolean cho
    /// biết việc cập nhật có thành công hay không?
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    bool Update(Supplier data);
    /// <summary>
    /// Xóa nhà cung cấp dựa vào mã. Hàm trả về giá trị bool cho biết việc xóa
    /// có thực hiện được hay không (Lưu ý: không được xóa nhà cung cấp nếu đang có mặt hàng tham chiếu
    /// đến nhà cung cấp)
    /// </summary>
    /// <param name="supplierID"></param>
    /// <returns></returns>
    bool Delete(int supplierID);

    List<Supplier> ListOfSuppliers();
  }
}

using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
  public interface ICustomerDAL
  {
    /// <summary>
    /// Bổ sung một nhà cung cấp mới. Hàm trả về mã của Khách hàng
    /// nếu bổ sung thành công.
    /// </summary>
    /// <param name="data">Đối tượng lưu thông tin của khách hàng cần bổ sung.</param>
    /// <returns></returns>
    int Add(Customer data);

    /*    int[] Add(Customer[] data) lấy thông tin nhiều khách hàng*/
    /// <summary>
    /// Lấy Danh sách các khách hàng(Tìm kiếm, Phân trang)
    /// </summary>
    /// <param name="page">Trang cần lấy dữ liệu</param>
    /// <param name="pageSize">Số dòng hiển thị trên một trang</param>
    /// <param name="searchValue">giá trị cần tìm kiếm theo CustomerName, ContactName, Country, City, Address, PostalCode (Chuỗi rỗng nếu không có nhu cầu tìm kiếm)</param>
    /// <returns></returns>
    List<Customer> List(int page, int pageSize, string searchValue);
    /// <summary>
    /// Đếm số lượng khách hàng thỏa điều kiện tìm kiếm
    /// </summary>
    /// <param name="searchValue">giá trị cần tìm kiếm theo CustomerName, ContactName, Country, City, Address, PostalCode (Chuỗi rỗng nếu không có nhu cầu tìm kiếm)</param>
    /// <returns></returns>
    int Count(string searchValue);
    /// <summary>
    /// Lấy thông tin của một khách hàng theo mã. Trong trường hợp nhà cung cấp không tồn tại
    /// hàm trả về giá trị null
    /// </summary>
    /// <param name="CustomerID">Mã của khách hàng cần lấy thông tin.</param>
    /// <returns></returns>
    Customer Get(int CustomerID);
    /// <summary>
    /// Cập nhật thông tin của một khách hàng . hàm trả về giá trị boolean cho
    /// biết việc cập nhật có thành công hay không?
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    bool Update(Customer data);
    /// <summary>
    /// Xóa khách hàng dựa vào mã. Hàm trả về giá trị bool cho biết việc xóa
    /// có thực hiện được hay không (Lưu ý: không được xóa khách hàng nếu đang có mặt hàng tham chiếu
    /// đến nhà cung cấp)
    /// </summary>
    /// <param name="CustomerID"></param>
    /// <returns></returns>
    bool Delete(int CustomerID);
  }
}

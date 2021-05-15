using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
  /// <summary>
  /// Định nghĩa các phép xử lý liên quan đến Nhân Viên
  /// </summary>
  public interface IEmployeeDAL
  {
    /// <summary>
    /// Bổ sung một Nhân Viên mới. Hàm trả về mã của Nhân Viên
    /// nếu bổ sung thành công.
    /// </summary>
    /// <param name="data">Đối tượng lưu thông tin của Nhân Viên cần bổ sung.</param>
    /// <returns></returns>
    int Add(Employee data);

    /*    int[] Add(Supplier[] data) lấy thông tin nhiều Nhân Viên*/
    /// <summary>
    /// Lấy Danh sách các Nhân Viên(Tìm kiếm, Phân trang)
    /// </summary>
    /// <param name="page">Trang cần lấy dữ liệu</param>
    /// <param name="pageSize">Số dòng hiển thị trên một trang</param>
    /// <param name="searchValue">giá trị cần tìm kiếm theo FirstName, LastName, BirthDate, Notes, Email (Chuỗi rỗng nếu không có nhu cầu tìm kiếm)</param>
    /// <returns></returns>
    List<Employee> List(int page, int pageSize, string searchValue);


    /// <summary>
    /// Đếm số lượng Nhân Viên thỏa điều kiện tìm kiếm
    /// </summary>
    /// <param name="searchValue">giá trị cần tìm kiếm theo FirstName, LastName, BirthDate, Notes, Email (Chuỗi rỗng nếu không có nhu cầu tìm kiếm)</param>
    /// <returns></returns>
    int Count(string searchValue);


    /// <summary>
    /// Lấy thông tin của một Nhân Viên theo mã. Trong trường hợp Nhân Viên không tồn tại
    /// hàm trả về giá trị null
    /// </summary>
    /// <param name="EmployeeID">Mã của Nhân Viên cần lấy thông tin.</param>
    /// <returns></returns>
    Employee Get(int EmployeeID);


    /// <summary>
    /// Cập nhật thông tin của một Nhân Viên . hàm trả về giá trị boolean cho
    /// biết việc cập nhật có thành công hay không?
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    bool Update(Employee data);


    /// <summary>
    /// Xóa Nhân Viên dựa vào mã. Hàm trả về giá trị bool cho biết việc xóa
    /// có thực hiện được hay không (Lưu ý: không được xóa Nhân Viên nếu đang có mặt hàng tham chiếu
    /// đến Nhân Viên)
    /// </summary>
    /// <param name="EmployeeID"></param>
    /// <returns></returns>
    bool Delete(int EmployeeID);
  }
}

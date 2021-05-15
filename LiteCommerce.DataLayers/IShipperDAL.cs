using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
  public interface IShipperDAL
  {
    /// <summary>
    /// Bổ sung một nhà vận chuyển mới. Hàm trả về mã của nhà vận chuyển
    /// nếu bổ sung thành công.
    /// </summary>
    /// <param name="data">Đối tượng lưu thông tin của nhà vận chuyển cần bổ sung.</param>
    /// <returns></returns>
    int Add(Shipper data);

    /*    int[] Add(Shipper[] data) lấy thông tin nhiều nhà vận chuyển*/
    /// <summary>
    /// Lấy Danh sách các nhà vận chuyển(Tìm kiếm, Phân trang)
    /// </summary>
    /// <param name="page">Trang cần lấy dữ liệu</param>
    /// <param name="pageSize">Số dòng hiển thị trên một trang</param>
    /// <param name="searchValue">giá trị cần tìm kiếm theo ShipperName, Phone (Chuỗi rỗng nếu không có nhu cầu tìm kiếm)</param>
    /// <returns></returns>
    List<Shipper> List(int page, int pageSize, string searchValue);
    /// <summary>
    /// Đếm số lượng nhà vận chuyển thỏa điều kiện tìm kiếm
    /// </summary>
    /// <param name="searchValue">giá trị cần tìm kiếm theo ShipperName, Phone (Chuỗi rỗng nếu không có nhu cầu tìm kiếm)</param>
    /// <returns></returns>
    int Count(string searchValue);
    /// <summary>
    /// Lấy thông tin của một nhà vận chuyển theo mã. Trong trường hợp nhà vận chuyển không tồn tại
    /// hàm trả về giá trị null
    /// </summary>
    /// <param name="ShipperID">Mã của nhà vận chuyển cần lấy thông tin.</param>
    /// <returns></returns>
    Shipper Get(int ShipperID);
    /// <summary>
    /// Cập nhật thông tin của một nhà vận chuyển . hàm trả về giá trị boolean cho
    /// biết việc cập nhật có thành công hay không?
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    bool Update(Shipper data);
    /// <summary>
    /// Xóa khách hàng dựa vào mã. Hàm trả về giá trị bool cho biết việc xóa
    /// có thực hiện được hay không (Lưu ý: không được xóa nhà vận chuyển nếu đang có mặt hàng tham chiếu
    /// đến nhà vận chuyển)
    /// </summary>
    /// <param name="ShipperID"></param>
    /// <returns></returns>
    bool Delete(int ShipperID);
  }
}

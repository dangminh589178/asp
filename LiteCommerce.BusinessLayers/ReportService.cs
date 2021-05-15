using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
  /// <summary>
  /// Cung cấp các chức năng tác nghiệp liên quan đến thống kê báo cáo
  /// </summary>
  //static khi dùng ko cần phải new
  public static class ReportService
  {
    /// <summary>
    /// Khởi tạo tính năng tác nghiệp (hàm này phải được gọi nếu muốn sử dụng các tính năng của lớp)
    /// </summary>
    /// <param name="dbType"></param>
    /// <param name="connectionString">Chuỗi kết nối</param>
    public static void Init(DatabaseTypes dbType, string connectionString)
    {

    }
  }
}

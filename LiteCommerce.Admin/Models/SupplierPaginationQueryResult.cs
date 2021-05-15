using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{

  /// <summary>
  /// Kết quả truy vấn dữ liệu có phân trang cho Supplier
  /// </summary>
  public class SupplierPaginationQueryResult : BasePaginationQueryResult
  {
    public List<Supplier> Data { get; set; }
  }
}

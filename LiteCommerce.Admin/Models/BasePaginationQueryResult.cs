using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{
  /// <summary>
  /// lop abstrac dung de ke thua. ko dung de tao ra doi tuong(ko new duoc)
  /// </summary>
  public abstract class BasePaginationQueryResult
  {
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string SearchValue { get; set; }

    public int RowCount { get; set; }

    public int PageCount {
      get
      {
        int p = RowCount / PageSize;
        if (RowCount % PageSize != 0)
          p += 1;
        return p;
      }
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LiteCommerce.DomainModels;

namespace LiteCommerce.Admin.Models
{
  public class ProductPaginationQueryResult : BasePaginationQueryResult
  {
    public List<Product> Data { get; set; }
  }
}

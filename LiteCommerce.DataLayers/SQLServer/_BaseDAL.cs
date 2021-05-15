using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers.SQLServer
{
  /// <summary>
  /// 
  /// </summary>
  public abstract class _BaseDAL
  {
    /// <summary>
    /// Chuổi tham số kết nối đến cơ sở dữ liệu
    /// </summary>
    protected string connectionString;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="connectionString"></param>
    public _BaseDAL (string connectionString)
    {
      this.connectionString = connectionString;
    }
    /// <summary>
    /// Tạo và mở kết nối đến cơ sở dữ liệu
    /// </summary>
    /// <returns></returns>
    protected SqlConnection GetConnection()
    {
      SqlConnection connection = new SqlConnection();
      connection.ConnectionString = this.connectionString;
      connection.Open();
      return connection;
    }
  }
}

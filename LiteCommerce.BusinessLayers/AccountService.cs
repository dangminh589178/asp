using LiteCommerce.DataLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
  /// <summary>
  /// 
  /// </summary>
  public static class AccountService
  {
    private static IAccountDAL AccountDB;
    public static void Init(DatabaseTypes dbType, string connectionString, AccountTypes accountType)
    {
      switch (dbType)
      {
        case DatabaseTypes.SQLServer:
          if (accountType == AccountTypes.Employee) {
            AccountDB = new DataLayers.SQLServer.EmployeeAccountDAL(connectionString);
          } else {
            AccountDB = new DataLayers.SQLServer.CustomerAccountDAL(connectionString);
          }
          break;
        default:
          throw new Exception("Database is not Supported");
      }
    }

    public static Account Authorize(string loginName, string password)
    {
      return AccountDB.Authorize(loginName, password);
    }

    public static bool ChangePassword(string accountId, string oldPassword, string newPassword, string confirmpassword)
    {
      return AccountDB.ChangePassword(accountId, oldPassword, newPassword, confirmpassword);
    }

    public static Account Get(string accountId)
    {
      return AccountDB.Get(accountId);
    }

    public static bool CheckOldPassword(string oldpassword, string accountId)
    {
      return AccountDB.CheckOldPassword(oldpassword, accountId);
    }
  }

  /// <summary>
  /// định nghĩa các loại tài khoản
  /// </summary>
  public enum AccountTypes
  {
    /// <summary>
    /// Nhân viên
    /// </summary>
    Employee,
    /// <summary>
    /// Khách hàng
    /// </summary>
    Customer
  }
}

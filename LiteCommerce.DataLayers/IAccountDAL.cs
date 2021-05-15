using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
  /// <summary>
  /// Khai báo các phép xử lý liên quan đến tài khoản của User
  /// </summary>
  public interface IAccountDAL
  {
    /// <summary>
    /// Kiếm tra thông tin đăng nhập của user (Hàm trà về null nếu thông tin bằng 0
    /// </summary>
    /// <param name="loginName">Tên đăng nhập</param>
    /// <param name="password">Mật khẩu </param>
    /// <returns></returns>
    Account Authorize(string loginName, string password);

    /// <summary>
    /// Đổi mật khẩu 
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="oldpassword"></param>
    /// <param name="newpassword"></param>
    /// <returns></returns>
    bool ChangePassword(string accountId, string oldpassword, string newpassword, string confirmpassword);

    /// <summary>
    /// Lấy thông tin của tài khoản theo id
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    Account Get(string accountId);

    bool CheckOldPassword(string oldpassword, string accountId);
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace LiteCommerce.Admin
{
  /// <summary>
  /// các hàm dùng để mã hóa/ giải mã
  /// </summary>
  public static class CryptHelper
  {

    /// <summary>
    /// Mã hóa Md5 chuỗi text
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Md5(string text)
    {
      MD5 md5 = new MD5CryptoServiceProvider();
      UTF8Encoding encoder = new UTF8Encoding();
      Byte[] originalBytes = encoder.GetBytes(text);
      Byte[] encodedBytes = md5.ComputeHash(originalBytes);
      text = BitConverter.ToString(encodedBytes).Replace("-", "");
      var result = text.ToLower();
      return result;
    }
  }
}

using LiteCommerce.DomainModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin
{
  /// <summary>
  /// 
  /// </summary>
  public class CookieHelper
  {
    public static string AccountToCookieString(Account data)
    {
      return JsonConvert.SerializeObject(data);
    }

    public static Account CookieStringToAccount(string cookie)
    {
      return JsonConvert.DeserializeObject<Account>(cookie);
    }
  }
}

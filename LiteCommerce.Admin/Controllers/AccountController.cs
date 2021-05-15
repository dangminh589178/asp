using CaptchaMvc.HtmlHelpers;
using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LiteCommerce.Admin.Controllers
{
  public class AccountController : Controller
  {


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    // GET: Account
    [HttpGet]
    public ActionResult Index (string accountId)
    {
      Account info = AccountService.Get(accountId);
      if (info == null)
        return RedirectToAction("Logout", "Account");
      return View(info);
    }

    public ActionResult Login(string loginName = "", string password = "")
    {
      ViewBag.LoginName = loginName;
      if (Request.HttpMethod == "POST")
      {
        var account = AccountService.Authorize(loginName, CryptHelper.Md5(password));
        if(account == null)
        {
          ModelState.AddModelError("", "Thông tin đăng nhập bị sai");
          return View();
        }
        // ghi nhan cookie cho phien dang nhap
        FormsAuthentication.SetAuthCookie(CookieHelper.AccountToCookieString(account), false);


        return RedirectToAction("Index", "Home");
      }
      else
      {
        return View();
      }
    }

    public ActionResult Logout()
    {
      Session.Clear();
      FormsAuthentication.SignOut();
      return RedirectToAction("Login", "Account");  
    }


    /// <summary>
    /// Đổi mật khẩu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public ActionResult ChangePassword(string accountId,  string oldpassword, string newpassword, string confirmpassword)
    {
     
      if (string.IsNullOrEmpty(oldpassword)) ModelState.AddModelError("oldpassword", "Mật khẩu cũ không được để trống !!! ");
      else
      {
        if (!AccountService.CheckOldPassword(oldpassword, accountId)) ModelState.AddModelError("oldpassword", "Mật khẩu cũ không Chính xác !!! ");
      }
      if (string.IsNullOrEmpty(newpassword)) ModelState.AddModelError("newpassword", "Mật khẩu mới không được để trống !!! ");
      if (string.IsNullOrEmpty(confirmpassword)) ModelState.AddModelError("confirmpassword", "Bạn chưa xác nhận lại mật khẩu !!! ");
      if (confirmpassword != newpassword && !string.IsNullOrEmpty(newpassword) && !string.IsNullOrEmpty(confirmpassword)) ModelState.AddModelError("confirmpassword", "Mật khẩu nhập lại không chính xác !!! ");
      if (!this.IsCaptchaValid(""))
      {
        ViewBag.ErrorMessage = "Captcha is not valid!";
        return View();
      }
      if (!ModelState.IsValid)
      {
        ViewBag.accountId = accountId;
        ViewBag.oldpassword = oldpassword;
        ViewBag.newpassword = newpassword;
        ViewBag.confirmpassword = confirmpassword;
        return View();
      }
      else
      {
        bool check = AccountService.ChangePassword(accountId, oldpassword, newpassword, confirmpassword);
        if (check == false) return RedirectToAction("Logout", "Account");
        return RedirectToAction("ChangePassword", "Account");
      }
    }
    [HttpGet]
    public ActionResult ChangePassword(string accountId)
    {
      ViewBag.accountId = accountId;
      return View();
    }
  }
}

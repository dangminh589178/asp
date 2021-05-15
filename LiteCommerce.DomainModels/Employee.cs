using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LiteCommerce.DomainModels
{
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Web;
  /// <summary>
  ///  Nhân Viên
  /// </summary>
  public class Employee
  {
    public int EmployeeID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string BirthDate { get; set; }
    public string Photo { get; set; }
    public string Notes { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    [NotMapped]
    public HttpPostedFileBase ImageUpload { get; set; }
  }
}

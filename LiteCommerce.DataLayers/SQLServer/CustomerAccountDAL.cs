using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers.SQLServer
{
  public class CustomerAccountDAL : _BaseDAL, IAccountDAL
  {
    public CustomerAccountDAL(string connectionString): base(connectionString)
    {

    }

    public Account Authorize(string loginName, string password)
    {
      Account data = null;
      using (SqlConnection connection = GetConnection())
      {
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = @"SELECT CustomerID, CustomerName, ContactName, Address, City, PostalCode, Country, Email, Password
                             FROM Customers
                              WHERE Email = @loginName AND Password = @password";
        cmd.CommandType = System.Data.CommandType.Text;
        cmd.Parameters.AddWithValue("@loginName", loginName);
        cmd.Parameters.AddWithValue("@password", password);

        using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        {
          if (dbReader.Read())
          {
            data = new Account()
            {
              UserName = dbReader["CustomerID"].ToString(),
              FullName = dbReader["CustomerName"].ToString(),
              Email = dbReader["Email"].ToString(),
              Password = dbReader["Password"].ToString()
            };
          }
        }
        connection.Close();
      }
      return data;
    }

    public bool ChangePassword(string accountId, string oldpassword, string newpassword,string confirmpassword)
    {
      throw new NotImplementedException();
    }

    public Account Get(string accountId)
    {
      throw new NotImplementedException();
    }

    public bool CheckOldPassword(string oldpassword, string accountId)
    {
      throw new NotImplementedException();
    }
  }
}

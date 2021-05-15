using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers.SQLServer
{
  public class EmployeeAccountDAL : _BaseDAL, IAccountDAL
  {
    /// <summary>
    /// Constructor cua employee
    /// </summary>
    /// <param name="connectionString"></param>
    public EmployeeAccountDAL(string connectionString): base(connectionString)
    {

    }

    /// <summary>
    /// phân quyền đăng nhập
    /// </summary>
    /// <param name="loginName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public Account Authorize(string loginName, string password)
    {
      Account data = null;
      using (SqlConnection connection = GetConnection())
      {
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = @"SELECT EmployeeID, FirstName, LastName, Email, Photo, Password
                             FROM Employees
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
              UserName = dbReader["EmployeeID"].ToString(),
              FullName = $"{dbReader["FirstName"]} {dbReader["LastName"]}",
              Email = dbReader["Email"].ToString(),
              Photo = dbReader["Photo"].ToString(),
              Password = dbReader["Password"].ToString()
            };
          }
        }
        connection.Close();
      }
      return data;
    }

    public bool ChangePassword(string accountId, string oldpassword, string newpassword, string confirmpassword)
    {
      bool result = false;
      if (!CheckOldPassword(oldpassword, accountId)) return false;
      else
      {
        using (SqlConnection connection = new SqlConnection(this.connectionString))
        {
          connection.Open();
          using (SqlCommand cmd = new SqlCommand())
          {
            cmd.CommandText = @"UPDATE Employees
                                        SET Password = @Password
                                        WHERE Email = @email";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue("@Password", GetMd5Hash(newpassword));
            cmd.Parameters.AddWithValue("@email", accountId);
            result = cmd.ExecuteNonQuery() > 0;
          }
          connection.Close();
          return result;
        }
      }
    }

    private string GetMd5Hash(string input)
    {
      using (MD5 md5Hash = MD5.Create())
      {
        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
          sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();

      }
    }


    public Account Get(string accountId)
    {

      Account data = null;
      
      using(SqlConnection connection = GetConnection())
      {
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = @"SELECT EmployeeID, FirstName, LastName, Email, Photo, Password
                             FROM Employees
                              WHERE Email = @email";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@email", accountId);
        using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        {
          if (dbReader.Read())
          {
            data = new Account()
            {
              UserName = dbReader["EmployeeID"].ToString(),
              FullName = $"{dbReader["FirstName"]} {dbReader["LastName"]}",
              Email = dbReader["Email"].ToString(),
              Photo = dbReader["Photo"].ToString(),
              Password = dbReader["Password"].ToString()
            };
          }
        }
        connection.Close();
      }
      return data;
    }

    public bool CheckOldPassword(string oldpassword, string accountId)
    {
      int id;
      using (SqlConnection connection = GetConnection())
      {
        using (SqlCommand cmd = new SqlCommand())
        {
          cmd.CommandText = @"select EmployeeID from Employees where Email=@Email and Password=@Password";
          cmd.CommandType = CommandType.Text;
          cmd.Connection = connection;
          cmd.Parameters.AddWithValue("@Email", accountId);
          cmd.Parameters.AddWithValue("@Password", GetMd5Hash(oldpassword));
          id = Convert.ToInt32(cmd.ExecuteScalar());
        }
        connection.Close();
      }
      return id > 0;
    }
  }
}

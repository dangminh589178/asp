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
  public class EmployeeDAL : _BaseDAL , IEmployeeDAL
  {
    public EmployeeDAL (string connectionString): base(connectionString)
    {

    }

    public int Add(Employee data)
    {
      int EmployeeId = 0;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"  INSERT INTO Employees(LastName,FirstName, BirthDate, Photo, Notes, Email, Password)
                            VALUES(@LastName,@FirstName, @BirthDate, @Photo, @Notes, @Email, @Password)
                           SELECT @@IDENTITY";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@LastName", data.LastName);
        cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
        cmd.Parameters.AddWithValue("@BirthDate", data.BirthDate);
        cmd.Parameters.AddWithValue("@Photo", data.Photo);
        cmd.Parameters.AddWithValue("@Notes", data.Notes);
        cmd.Parameters.AddWithValue("@Email", data.Email);
        cmd.Parameters.AddWithValue("@Password", GetMd5Hash(data.Password));


        // ExecuteScalar sử dụng trong trường hợp dữ liệu trả về là một cột, tối đa một dòng . 1 giá trị
        EmployeeId = Convert.ToInt32(cmd.ExecuteScalar());

        cn.Close();
      }
      return EmployeeId;
    }

    public int Count(string searchValue)
    {
      if (searchValue != "")
        searchValue = "%" + searchValue + "%";
      int count = 0;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"SELECT COUNT(*) FROM Employees 
	                             WHERE (@searchValue = '')
	                              OR (
		                              FirstName LIKE @searchValue
	                              OR	LastName LIKE @searchValue
                                OR	BirthDate LIKE @searchValue
                                OR	Notes LIKE @searchValue
	                              OR	Email LIKE @searchValue
	                              )";

        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@searchValue", searchValue);

        // executeScalar sử dụng trong trường hợp câu lệnh trả về là 1 dòng 1 cột, 1 giá trị
        count = Convert.ToInt32(cmd.ExecuteScalar());

        cn.Close();
      }
      return count;
    }

    public bool Delete(int EmployeeID)
    {
      bool result = false;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"  DELETE FROM Employees WHERE EmployeeID = @EmployeeID
                        AND NOT EXISTS( SELECT * FROM Orders WHERE EmployeeID = Employees.EmployeeID)";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
        result = cmd.ExecuteNonQuery() > 0;
        cn.Close();
      }
      return result;
    }

    public Employee Get(int EmployeeID)
    {
      Employee data = null;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Employees WHERE EmployeeID = @EmployeeID";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
        using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        {
          if (dbReader.Read())
          {
            data = new Employee()
            {
              EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
              FirstName = Convert.ToString(dbReader["FirstName"]),
              LastName = Convert.ToString(dbReader["LastName"]),
              BirthDate = Convert.ToString(dbReader["BirthDate"]),
              Photo = Convert.ToString(dbReader["Photo"]),
              Notes = Convert.ToString(dbReader["Notes"]),
              Email = Convert.ToString(dbReader["Email"]),
              Password = Convert.ToString(dbReader["Password"])
            };
          }
        }
        cn.Close();
      }
      return data;
    }

    public List<Employee> List(int page, int pageSize, string searchValue)
    {
      if (searchValue != "")
        searchValue = "%" + searchValue + "%";   //% tìm chuỗi kí tự với bất kì độ dài nào (bao gồm cả độ dài 0)
      List<Employee> data = new List<Employee>();
      using (SqlConnection cn = GetConnection()) 
      {

        SqlCommand cmd = new SqlCommand();
        /*cmd.CommandText = "SELECT * FROM Employees";*/
        cmd.CommandText = @"SELECT *
                            FROM
                              (
                              SELECT *, ROW_NUMBER() OVER(ORDER BY FirstName) AS RowNumber
                              FROM Employees
                              WHERE (@searchValue = '')
	                              OR (
		                              FirstName LIKE @searchValue
	                              OR	LastName LIKE @searchValue
                                OR	BirthDate LIKE @searchValue
                                OR	Notes LIKE @searchValue
	                              OR	Email LIKE @searchValue
	                              )
                              ) AS s
                              WHERE s.RowNumber BETWEEN (@page -1) * @pageSize + 1 And @page*@pageSize";
        cmd.CommandType = System.Data.CommandType.Text;
        // truyen tham so cho bien (bat buoc)
        cmd.Parameters.AddWithValue("@searchValue", searchValue);
        cmd.Parameters.AddWithValue("@page", page);
        cmd.Parameters.AddWithValue("@pageSize", pageSize);
        cmd.Connection = cn;
        // data reader doc du lieu nhanh. doc tu tren xuong
        //Câu lệnh using sẽ đảm bảo xử lý đúng đối tượng và giải phóng tài nguyên.
        using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        {
          while (dbReader.Read())
          {

            data.Add(new Employee()
            {
              EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
              FirstName = Convert.ToString(dbReader["FirstName"]),
              LastName = Convert.ToString(dbReader["LastName"]),
              BirthDate = Convert.ToString(dbReader["BirthDate"]),
              Photo = Convert.ToString(dbReader["Photo"]),
              Notes = Convert.ToString(dbReader["Notes"]),
              Email = Convert.ToString(dbReader["Email"]),
              Password = Convert.ToString(dbReader["Password"])
            });
          }
        }

        cn.Close();
      }

      return data;
    }


    public bool Update(Employee data)
    {
      bool result = false;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"  UPDATE Employees SET LastName = @LastName, FirstName = @FirstName, BirthDate = @BirthDate, Photo = @Photo, Notes = @Notes, Email = @Email, Password = @Password
                              WHERE EmployeeID = @EmployeeID";

        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
        cmd.Parameters.AddWithValue("@LastName", data.LastName);
        cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
        cmd.Parameters.AddWithValue("@BirthDate", data.BirthDate);
        cmd.Parameters.AddWithValue("@Notes", data.Notes);
        cmd.Parameters.AddWithValue("@Email", data.Email);
        cmd.Parameters.AddWithValue("@Password", GetMd5Hash(data.Password));
        cmd.Parameters.AddWithValue("@Photo", data.Photo);
        result = cmd.ExecuteNonQuery() > 0;
        cn.Close();
      }
      return result;
    }

    /// <summary>
    /// Hàm để đưa mật khẩu về dạng mã hóa MD5. mã hóa 1 chiều
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
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
  }
}

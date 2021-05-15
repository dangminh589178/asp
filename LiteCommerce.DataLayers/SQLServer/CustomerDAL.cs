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
  public class CustomerDAL : _BaseDAL, ICustomerDAL
  {
    public CustomerDAL(string connectionString) : base(connectionString)
    { }
    public int Add(Customer data)
    {
      int CustomerId = 0;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Customers(CustomerName,ContactName, Address, City, PostalCode, Country, Email, Password)
                            VALUES(@CustomerName,@ContactName, @Address, @City, @PostalCode, @Country, @Email, @Password)
                           SELECT @@IDENTITY";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("CustomerName", data.CustomerName);
        cmd.Parameters.AddWithValue("ContactName", data.ContactName);
        cmd.Parameters.AddWithValue("Address", data.Address);
        cmd.Parameters.AddWithValue("City", data.City);
        cmd.Parameters.AddWithValue("PostalCode", data.PostalCode);
        cmd.Parameters.AddWithValue("Country", data.Country);
        cmd.Parameters.AddWithValue("Email", data.Email);
        cmd.Parameters.AddWithValue("Password", data.Password);


        // ExecuteScalar sử dụng trong trường hợp dữ liệu trả về là một cột, tối đa một dòng . 1 giá trị
        CustomerId = Convert.ToInt32(cmd.ExecuteScalar());

        cn.Close();
      }
      return CustomerId;
    }

    public int Count(string searchValue)
    {
      if (searchValue != "")
        searchValue = "%" + searchValue + "%";
      int count = 0;
      using(SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"SELECT COUNT(*) FROM Customers WHERE (@searchValue = '')
	                              OR (
		                              CustomerName LIKE @searchValue
	                              OR	ContactName LIKE @searchValue
                                OR	Country LIKE @searchValue
                                OR	City LIKE @searchValue
	                              OR	Address LIKE @searchValue
                                OR	PostalCode LIKE @searchValue
	                              )";

        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@searchValue", searchValue);

        // executeScalar sử dụng trong trường hợp câu lệnh trả về là 1 dòng 1 cột, 1 giá trị
        count = Convert.ToInt32(cmd.ExecuteScalar());

        cn.Close();
      }
      return count;
    }

    public bool Delete(int CustomerID)
    {
      bool result = false;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"  DELETE FROM Customers WHERE CustomerID = @CustomerID
                        AND NOT EXISTS( SELECT * FROM Orders WHERE CustomerID = Customers.CustomerID)";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
        result = cmd.ExecuteNonQuery() > 0;
        cn.Close();
      }
      return result;
    }

    public Customer Get(int CustomerID)
    {
      Customer data = null;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Customers WHERE CustomerID = @CustomerID";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
        using(SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        {
          if(dataReader.Read())
          {
            data = new Customer()
            {
              CustomerID = Convert.ToInt32(dataReader["CustomerID"]),
              CustomerName = Convert.ToString(dataReader["CustomerName"]),
              ContactName = Convert.ToString(dataReader["ContactName"]),
              Country = Convert.ToString(dataReader["Country"]),
              City = Convert.ToString(dataReader["City"]),
              Address = Convert.ToString(dataReader["Address"]),
              PostalCode = Convert.ToString(dataReader["PostalCode"]),
              Email = Convert.ToString(dataReader["Email"]),
              Password = Convert.ToString(dataReader["Password"])
            };
          }
        }
        cn.Close();
      }
        return data;
    }

    public List<Customer> List(int page, int pageSize, string searchValue)
    {
      if (searchValue != "")
        searchValue = "%" + searchValue + "%";
      List<Customer> data = new List<Customer>();
      using (SqlConnection cn = GetConnection()) // mở kết nối
      {

        SqlCommand cmd = new SqlCommand();
        /*cmd.CommandText = "SELECT * FROM Suppliers";*/
        cmd.CommandText = @"SELECT *
                            FROM
                              (
                              SELECT *, ROW_NUMBER() OVER(ORDER BY CustomerName) AS RowNumber
                              FROM Customers
                              WHERE (@searchValue = '')
	                              OR (
		                              CustomerName LIKE @searchValue
	                              OR	ContactName LIKE @searchValue
                                OR	Country LIKE @searchValue
                                OR	City LIKE @searchValue
	                              OR	Address LIKE @searchValue
                                OR	PostalCode LIKE @searchValue
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
        using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        {
          while (dbReader.Read())
          {

            data.Add(new Customer()
            {
              CustomerID = Convert.ToInt32(dbReader["CustomerID"]),
              CustomerName = Convert.ToString(dbReader["CustomerName"]),
              ContactName = Convert.ToString(dbReader["ContactName"]),
              Country = Convert.ToString(dbReader["Country"]),
              City = Convert.ToString(dbReader["City"]),
              Address = Convert.ToString(dbReader["Address"]),
              PostalCode = Convert.ToString(dbReader["PostalCode"])
            });
          }
        }

        cn.Close();
      }

      return data;
    }

    public bool Update(Customer data)
    {
      bool result = false;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"  UPDATE Customers SET CustomerName = @CustomerName, ContactName = @ContactName, Address = @Address, City = @City, PostalCode = @PostalCode, Country = @Country,Email = @Email, Password = @Password
                              WHERE CustomerID = @CustomerID";

        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
        cmd.Parameters.AddWithValue("@CustomerName", data.CustomerName);
        cmd.Parameters.AddWithValue("@ContactName", data.ContactName);
        cmd.Parameters.AddWithValue("@Address", data.Address);
        cmd.Parameters.AddWithValue("@City", data.City);
        cmd.Parameters.AddWithValue("@PostalCode", data.PostalCode);
        cmd.Parameters.AddWithValue("@Country", data.Country);
        cmd.Parameters.AddWithValue("@Email", data.Email);
        cmd.Parameters.AddWithValue("@Password", data.Password);

        result = cmd.ExecuteNonQuery() > 0;
        cn.Close();
      }
      return result;
    }
  }
}

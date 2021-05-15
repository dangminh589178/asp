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
  public class SupplierDAL : _BaseDAL, ISupplierDAL
  {

    public SupplierDAL(string connectionString) : base(connectionString)
    { }
    public int Add(Supplier data)
    {
      int supplierID = 0;
      // mo ket noi
      using(SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Suppliers 
                                    (
	                                    SupplierName,
	                                    ContactName,
	                                    Address,
	                                    City,
	                                    PostalCode,
	                                    Country,
	                                    Phone
                                    ) 
                                    VALUES
                                    (
	                                    @SupplierName,
	                                    @ContactName,
	                                    @Address,
	                                    @City,
	                                    @PostalCode,
	                                    @Country,
	                                    @Phone
                                    )
                                    SELECT @@IDENTITY"; // khi thuc thi cau lenh tra ve 1 dong 1 cot 
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@SupplierName",data.SupplierName);
        cmd.Parameters.AddWithValue("@ContactName", data.ContactName);
        cmd.Parameters.AddWithValue("@Address", data.Address);
        cmd.Parameters.AddWithValue("@City", data.City);
        cmd.Parameters.AddWithValue("@PostalCode", data.PostalCode);
        cmd.Parameters.AddWithValue("@Country", data.Country);
        cmd.Parameters.AddWithValue("@Phone", data.Phone);

        // ExecuteScalar() Sử dụng trong trường hợp dữ liệu trả về là một cột và tối đa 1 dòng.(1 giá trị)
        supplierID = Convert.ToInt32(cmd.ExecuteScalar());
        cn.Close();
      }

      return supplierID;
    }

    public int Count(string searchValue)
    {
      if (searchValue != "")
        searchValue = "%" + searchValue + "%";
      int result = 0;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        /*SqlCommand cmd = new SqlCommand(); khong duoc dung vi phai tao ket noi cho sqlcommand*/
        cmd.CommandText = @"SELECT COUNT(*) FROM Suppliers
                            WHERE (@searchValue = '')
	                              OR (
		                                  SupplierName LIKE @searchValue
	                                  OR	ContactName LIKE @searchValue
	                                  OR	Address LIKE @searchValue
	                                  OR	Phone LIKE @searchValue
                                 )";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@searchValue", searchValue);

        // executescalar chi tra ve gia tri 1 dong 1 cot dau tien cua cau truy van
        // ham executescalar tra ve kieu object vi the phai convert
        result = Convert.ToInt32(cmd.ExecuteScalar());

        cn.Close();
        // ExecuteNonQuery dùng để thực thi các câu truy vấn như INSERT, DELETE, UPDATE.
        // nó trả về kiểu int chính là số dòng trong table của database bị thay đổi bởi 3 lệnh trên. nếu = -1 thì bị lỗi.
      }
      return result;
    }

    public bool Delete(int supplierID)
    {
      bool result = false;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"DELETE FROM Suppliers
                            WHERE SupplierId = @SupplierId
                            AND NOT EXISTS(SELECT *
                                               FROM Products
                                                WHERE SupplierID = Suppliers.SupplierID)";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@SupplierID", supplierID);

        //execute nonquery
        result = cmd.ExecuteNonQuery() > 0  ;
      }

      return result;
    }

    public Supplier Get(int supplierID)
    {
      Supplier data = null;

      using (SqlConnection connection = GetConnection())
      {
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM Suppliers WHERE SupplierID = @SupplierID";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@SupplierID", supplierID);
        // thực thi câu lệnh với ExecuteReader
        //using khi su dung xong doi tuong, khi chay xong no se tu giai phong
        using (SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        {
          if (dataReader.Read()) // nếu SQL data có dữ liệu , chir tra ve 1 dong nen dung if while no doc tung dong
          {
            data = new Supplier()
            {
              SupplierID = Convert.ToInt32(dataReader["SupplierID"]),
              SupplierName = Convert.ToString(dataReader["SupplierName"]),
              ContactName = Convert.ToString(dataReader["ContactName"]),
              Country = Convert.ToString(dataReader["Country"]),
              City = Convert.ToString(dataReader["City"]),
              Address = Convert.ToString(dataReader["Address"]),
              PostalCode = Convert.ToString(dataReader["PostalCode"]),
              Phone = Convert.ToString(dataReader["Phone"])
            };
          }
        }
        connection.Close();
      }
      return data;

    }

    public List<Supplier> List(int page, int pageSize, string searchValue)
    {
      if (searchValue != "")
        searchValue = "%" + searchValue + "%";
      List<Supplier> data = new List<Supplier>();
      using (SqlConnection cn = GetConnection()) // mở kết nối
      {

        SqlCommand cmd = new SqlCommand();
        /*cmd.CommandText = "SELECT * FROM Suppliers";*/
        cmd.CommandText = @"SELECT *
                            FROM
                              (
                              SELECT *, ROW_NUMBER() OVER(ORDER BY SupplierName) AS RowNumber
                              FROM Suppliers
                              WHERE (@searchValue = '')
	                              OR (
		                              SupplierName LIKE @searchValue
	                              OR	ContactName LIKE @searchValue
	                              OR	Address LIKE @searchValue
	                              OR	Phone LIKE @searchValue
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

            data.Add(new Supplier()
            {
              SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
              SupplierName = Convert.ToString(dbReader["SupplierName"]),
              ContactName = Convert.ToString(dbReader["ContactName"]),
              Country = Convert.ToString(dbReader["Country"]),
              City = Convert.ToString(dbReader["city"]),
              Address = Convert.ToString(dbReader["Address"]),
              PostalCode = Convert.ToString(dbReader["PostalCode"]),
              Phone = Convert.ToString(dbReader["Phone"])
            });
          }
        }

        cn.Close();
      }

      return data;
    }

    public bool Update(Supplier data)
    {
      bool result = false;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"UPDATE Suppliers
                            SET SupplierName = @SupplierName,
                                ContactName = @ContactName,
                                Address = @Address,
                                City = @City,
                                PostalCode = @PostalCode,
                                Country = @Country,
                                Phone = @Phone
                            WHERE SupplierID = @supplierID";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@supplierID", data.SupplierID);
        cmd.Parameters.AddWithValue("@SupplierName", data.SupplierName);
        cmd.Parameters.AddWithValue("@ContactName", data.ContactName);
        cmd.Parameters.AddWithValue("@Address", data.Address);
        cmd.Parameters.AddWithValue("@City", data.City);
        cmd.Parameters.AddWithValue("@PostalCode", data.PostalCode);
        cmd.Parameters.AddWithValue("@Country", data.Country);
        cmd.Parameters.AddWithValue("@Phone", data.Phone);

        result = cmd.ExecuteNonQuery() > 0;

        cn.Close();
      }
      return result;
      
    }

    public List<Supplier> ListOfSuppliers()
    {
      List<Supplier> data = new List<Supplier>();
      using (SqlConnection cn = GetConnection()) // mở kết nối
      {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT SupplierID, SupplierName FROM Suppliers";
        cmd.CommandType = System.Data.CommandType.Text;
        cmd.Connection = cn;
        // data reader doc du lieu nhanh. doc tu tren xuong
        using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        {
          while (dbReader.Read())
          {
            // theo cac tu tu
            //Country country = new Country();
            //country.CountryName = Convert.ToString(dbReader["CountryName"]);
            //data.Add(country);

            //cach 2
            //Country country = new Country()
            //{
            //  CountryName = Convert.ToString(dbReader["CountryName"])
            //};
            //data.Add(country);

            data.Add(new Supplier()
            {
              SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
              SupplierName = Convert.ToString(dbReader["SupplierName"])
            });
          }
        }

        cn.Close();
      }

      return data;
    }
  }
}

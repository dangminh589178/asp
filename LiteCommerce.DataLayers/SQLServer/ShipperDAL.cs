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
  public class ShipperDAL : _BaseDAL, IShipperDAL
  {
    public ShipperDAL(string connectionString) : base(connectionString)
    {

    }

    public int Add(Shipper data)
    {
      int ShipperID = 0;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Shippers (ShipperName,Phone)
                                        VALUES (@ShipperName,@Phone)
                                        SELECT @@IDENTITY";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@ShipperID", data.ShipperID);
        cmd.Parameters.AddWithValue("@ShipperName", data.ShipperName);
        cmd.Parameters.AddWithValue("@Phone", data.Phone);

        // ExecuteScalar() Sử dụng trong trường hợp dữ liệu trả về là một cột và tối đa 1 dòng.(1 giá trị)
        ShipperID = Convert.ToInt32(cmd.ExecuteScalar());
        cn.Close();
      }
      return ShipperID;
    }

    public int Count(string searchValue)
    {
      if (searchValue != "")
        searchValue = "%" + searchValue + "%";
      int result = 0;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"SELECT COUNT(*) FROM Shippers
                            WHERE (@searchValue = '')
	                              OR (
		                                 ShipperName LIKE @searchValue
	                              OR	Phone LIKE @searchValue
                                 )";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@searchValue", searchValue);

        result = Convert.ToInt32(cmd.ExecuteScalar());
        cn.Close();
      }
      return result;
    }

    public bool Delete(int ShipperID)
    {
      bool result = false;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"DELETE FROM Shippers WHERE ShipperID = @ShipperID
                                    AND NOT EXISTS (SELECT * FROM Orders WHERE ShipperID = Shippers.ShipperID)";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@ShipperID", ShipperID);

        result = cmd.ExecuteNonQuery() > 0;
        cn.Close();
      }
      return result;
    }

    public Shipper Get(int ShipperID)
    {
      Shipper data = null;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Shippers WHERE ShipperID = @ShipperID";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@ShipperID", ShipperID);
        using (SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        {
          if (dataReader.Read())
          {
            data = new Shipper()
            {
              ShipperID = Convert.ToInt32(dataReader["ShipperID"]),
              ShipperName = Convert.ToString(dataReader["ShipperName"]),
              Phone = Convert.ToString(dataReader["Phone"])
            };
          }
        }
        cn.Close();
      }
      return data;
    }

      public List<Shipper> List(int page, int pageSize, string searchValue)
      {
        if (searchValue != "")
          searchValue = "%" + searchValue + "%";
        List<Shipper> data = new List<Shipper>();
        using (SqlConnection cn = GetConnection()) // mở kết nối
        {

          SqlCommand cmd = new SqlCommand();
          /*cmd.CommandText = "SELECT * FROM Suppliers";*/
          cmd.CommandText = @"SELECT *
                            FROM
                              (
                              SELECT *, ROW_NUMBER() OVER(ORDER BY ShipperName) AS RowNumber
                              FROM Shippers
                              WHERE (@searchValue = '')
	                              OR (
		                              ShipperName LIKE @searchValue
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

              data.Add(new Shipper()
              {
                ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                ShipperName = Convert.ToString(dbReader["ShipperName"]),
                Phone = Convert.ToString(dbReader["Phone"])
              });
            }
          }

          cn.Close();
        }

        return data;
      }

      public bool Update(Shipper data)
      {
      bool result = false;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"UPDATE Shippers 
                            SET ShipperName = @ShipperName,
                                
                                Phone = @Phone
                            WHERE ShipperID = @ShipperID";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@ShipperID",data.ShipperID);
        cmd.Parameters.AddWithValue("@ShipperName", data.ShipperName);
        cmd.Parameters.AddWithValue("@Phone", data.Phone);

        result = cmd.ExecuteNonQuery() > 0;
        cn.Close();
      }
      return result;
      }
    }
  }

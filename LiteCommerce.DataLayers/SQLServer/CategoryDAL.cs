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
  public class CategoryDAL : _BaseDAL, ICategoryDAL
  {
    public CategoryDAL(string connectionString) : base(connectionString) {
    }
    public int Add(Category data)
    {
      int CategoryID = 0;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Categories (CategoryName, Description, ParentCategoryId)
                                      VALUES(@CategoryName, @Description, @ParentCategoryId)
                                      SELECT @@IDENTITY";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@CategoryName", data.CategoryName);
        cmd.Parameters.AddWithValue("@Description", data.Description);
        cmd.Parameters.AddWithValue("@ParentCategoryId", data.ParentCategoryId);
        CategoryID = Convert.ToInt32(cmd.ExecuteScalar());

        //cn.Close();
      }
      return CategoryID;
    }

    public int Count(string searchValue)
    {
      int result = 0;
      if (searchValue != "")
        searchValue = "%" + searchValue + "%";
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"SELECT COUNT(*) FROM Categories WHERE (@searchValue = '')
	                              OR (
		                              CategoryName LIKE @searchValue
                                     OR Description LIKE @searchValue
	                              )";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@searchValue", searchValue);

        // ExecuteScalar() . Sử dụng trong trường hợp dữ liệu trả về là một cột và tối đa một dòng. 1 giá trị 
        result = Convert.ToInt32(cmd.ExecuteScalar());

        cn.Close();
      }
      return result;
    }

    public bool Delelte(int CategoryID)
    {
      bool result = false;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"DELETE FROM Categories WHERE CategoryID = @CategoryID
                             AND NOT EXISTS(SELECT * FROM Products WHERE CategoryID = Categories.CategoryID)   ";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@CategoryID", CategoryID);

        result = cmd.ExecuteNonQuery() > 0;
      }
      return result;
    }

    public Category Get(int CategoryID)
    {
      Category data = null;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Categories WHERE CategoryID = @CategoryID";
        cmd.CommandType = CommandType.Text;
        // cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
        // Lỗi oject reference not set to an instance an object . Tham chiếu của đối tượng không được đặt thành một phiên bản của đối tượng
        cmd.Parameters.AddWithValue("@CategoryID", CategoryID);

        // ExecuteReader : Sử dụng trong trường hợp có dữ liệu trả về dưới dạng bảng (các dòng/ cột)
        using (SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        {
          if (dataReader.Read())// vi chi can get 1 dong nen dung if , khi list ra doc nhieu dong thi dung while
          {
            data = new Category()
            {
              CategoryID = Convert.ToInt32(dataReader["CategoryID"]),
              CategoryName = Convert.ToString(dataReader["CategoryName"]),
              Description = Convert.ToString(dataReader["Description"]),
              //  ParentCategoryId = Convert.ToInt32(dataReader["ParentCategoryId"])

            };
          }
        }
        cn.Close();
      }
      return data;
    }
    public List<Category> List(int page, int pageSize, string searchValue)
    {
      if (searchValue != "")
        searchValue = "%" + searchValue + "%";
      List<Category> data = new List<Category>();
      using (SqlConnection cn = GetConnection()) // mở kết nối
      {

        SqlCommand cmd = new SqlCommand();
        /*cmd.CommandText = "SELECT * FROM Suppliers";*/
        cmd.CommandText = @"SELECT *
                            FROM
                              (
                              SELECT *, ROW_NUMBER() OVER(ORDER BY CategoryName) AS RowNumber
                              FROM Categories
                              WHERE (@searchValue = '')
	                              OR (
		                              CategoryName LIKE @searchValue
                                     OR Description LIKE @searchValue
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

            data.Add(new Category()
            {
              CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
              CategoryName = Convert.ToString(dbReader["CategoryName"]),
              Description = Convert.ToString(dbReader["Description"]),
              // ParentCategoryId = Convert.ToInt32(dbReader["ParentCategoryId"])
            });
          }
        }

        cn.Close();
      }

      return data;
    }

    public bool Update(Category data)
    {
      bool result = false;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @" UPDATE Categories  SET
                                          CategoryName = @CategoryName,
                                          Description = @Description
                                          
                                    WHERE CategoryID = @CategoryID";
        /*ParentCategoryId = @ParentCategoryId*/
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
        cmd.Parameters.AddWithValue("@CategoryName", data.CategoryName);
        cmd.Parameters.AddWithValue("@Description", data.Description);
        /* cmd.Parameters.AddWithValue("@ParentCategoryId", data.ParentCategoryId);
 */


        // ExecuteNonquery sử dụng trong trường hợp câu lệnh không trả về dữ liệu (insert , update, delete) mà chỉ
        // trả về số dòng dữ liệu bị tác động bởi câu lệnh 
        result = cmd.ExecuteNonQuery() > 0;

        cn.Close();
      }
      return result;
    }

    public List<Category> ListOfCategories()
    {
      List<Category> data = new List<Category>();
      using (SqlConnection cn = GetConnection()) // mở kết nối
      {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT CategoryID, CategoryName FROM Categories";
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

            data.Add(new Category()
            {
              CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
              CategoryName = Convert.ToString(dbReader["CategoryName"])
            });
          }
        }

        cn.Close();
      }

      return data;
    }


  }
}

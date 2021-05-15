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
  /// <summary>
  /// 
  /// </summary>
  public class ProductDAL : _BaseDAL, IProductDAL
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionString"></param>
    public ProductDAL (string connectionString): base(connectionString)
    {

    }
    public int Count(int categoryId, int supplierId, string searchValue)
    {
      //TODO: phai viet Ham Count
      if (searchValue != "")
        searchValue = "%" + searchValue + "%";
      int result = 0;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        /*SqlCommand cmd = new SqlCommand(); khong duoc dung vi phai tao ket noi cho sqlcommand*/
        cmd.CommandText = @"SELECT COUNT(*) FROM Products
                            WHERE (@searchValue = '')
	                              OR (
		                                  ProductName LIKE @searchValue
	                                  OR	Unit LIKE @searchValue
	                                  OR	Price LIKE @searchValue
	                                  OR	SupplierID = @supplierId
                                    OR	CategoryID = @categoryId
                                 )";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@searchValue", searchValue);
        cmd.Parameters.AddWithValue("@categoryId", categoryId);
        cmd.Parameters.AddWithValue("@supplierId", supplierId);

        // executescalar chi tra ve gia tri 1 dong 1 cot dau tien cua cau truy van
        // ham executescalar tra ve kieu object vi the phai convert
        result = Convert.ToInt32(cmd.ExecuteScalar());

        cn.Close();
        // ExecuteNonQuery dùng để thực thi các câu truy vấn như INSERT, DELETE, UPDATE.
        // nó trả về kiểu int chính là số dòng trong table của database bị thay đổi bởi 3 lệnh trên. nếu = -1 thì bị lỗi.
      }
      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public int Add(Product data)
    {
      int productId = 0;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Products ( ProductName , SupplierID, CategoryID, Unit, Price, Photo)
                                        VALUES ( @ProductName , @SupplierID, @CategoryID, @Unit, @Price, @Photo)
                                        SELECT @@IDENTITY ";
        cmd.CommandType = CommandType.Text;
      
        cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
        cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
        cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
        cmd.Parameters.AddWithValue("@Unit", data.Unit);
        cmd.Parameters.AddWithValue("@Price", data.Price);
        cmd.Parameters.AddWithValue("@Photo", data.Photo);

        // ExecuteScalar su dung trong truong hop du lieu tra ve la mot cot, toi da 1 dong (1 gia tri)
        productId = Convert.ToInt32(cmd.ExecuteScalar());
        cn.Close();
      }
      return productId;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public long AddAttribute(ProductAttribute data)
    {
      int attributeId = 0;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"INSERT INTO ProductAttributes ( ProductID , AttributeName, AttributeValue, DisplayOrder)
                                        VALUES  ( @ProductID , @AttributeName, @AttributeValue, @DisplayOrder)
                                        SELECT @@IDENTITY ";
        cmd.CommandType = CommandType.Text;

        cmd.Parameters.AddWithValue("@ProductName", data.ProductID);
        cmd.Parameters.AddWithValue("@AttributeName", data.AttributeName);
        cmd.Parameters.AddWithValue("@AttributeValue", data.AttributeValue);
        cmd.Parameters.AddWithValue("@DisplayOrder", data.DisplayOrder);

        // ExecuteScalar su dung trong truong hop du lieu tra ve la mot cot, toi da 1 dong (1 gia tri)
        attributeId = Convert.ToInt32(cmd.ExecuteScalar());
        cn.Close();
      }
      return attributeId;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public long AddGallery(ProductGallery data)
    {
      int galleryId = 0;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"INSERT INTO ProductGallery ( ProductID , Photo, Description, DisplayOrder, IsHidden)
                                        VALUES  ( @ProductID , @Photo, @Description, @DisplayOrder, @IsHidden)
                                        SELECT @@IDENTITY ";
        cmd.CommandType = CommandType.Text;

        cmd.Parameters.AddWithValue("@ProductID", data.ProductID);
        cmd.Parameters.AddWithValue("@Photo", data.Photo);
        cmd.Parameters.AddWithValue("@Description", data.Description);
        cmd.Parameters.AddWithValue("@DisplayOrder", data.DisplayOrder);
        cmd.Parameters.AddWithValue("@IsHidden", data.IsHidden);

        // ExecuteScalar su dung trong truong hop du lieu tra ve la mot cot, toi da 1 dong (1 gia tri)
        galleryId = Convert.ToInt32(cmd.ExecuteScalar());
        cn.Close();
      }
      return galleryId;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    public bool Delete(int productId)
    {
      bool result = false;
      using(SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"DELETE FROM Products WHERE ProductID = @ProductID
                            DELETE FROM ProductGallery WHERE ProductGallery.ProductID = @ProductID
	                          DELETE FROM ProductAttributes WHERE ProductAttributes.ProductID = @ProductID
                            AND ProductID NOT IN ( SELECT ProductID FROM OrderDetails)";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@ProductID", productId);

        //execute noquery
        result = cmd.ExecuteNonQuery() > 0;
       }
      return result;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="attributeId"></param>
    /// <returns></returns>
    public bool DeleteAttribute(long attributeId)
    {
      bool result = false;
      using (SqlConnection connection = GetConnection())
      {
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = @"  DELETE FROM ProductAttributes WHERE AttributeID = @AttributeID )";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@AttributeID", attributeId);
        result = cmd.ExecuteNonQuery() > 0;
        connection.Close();
      }
      return result;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="galleryId"></param>
    /// <returns></returns>
    public bool DeleteGallery(long galleryId)
    {
      bool result = false;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"DELETE FROM ProductGallery
                            WHERE GalleryID = @GalleryID ";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@GalleryID", galleryId);

        //execute noquery
        result = cmd.ExecuteNonQuery() > 0;
      }
      return result;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    public Product Get(int productId)
    {
      Product data = null;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Products WHERE  ProductID = @ProductID";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@ProductID", productId);
        using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        {
          if (dbReader.Read())
          {
            data = new Product()
            {
              ProductID = Convert.ToInt32(dbReader["ProductID"]),
              ProductName = Convert.ToString(dbReader["ProductName"]),
              SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
              CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
              Unit = Convert.ToString(dbReader["Unit"]),
              Price = Convert.ToDecimal(dbReader["Price"]),
              Photo = Convert.ToString(dbReader["Photo"])

            };
          }
        }
        cn.Close();
      }
      return data;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="attributeId"></param>
    /// <returns></returns>
    public ProductAttribute GetAttribute(long attributeId)
    {
      ProductAttribute data = null;
      using(SqlConnection connection = GetConnection())
      {
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = @"SELECT * FROM ProductAttributes
                          WHERE AttributeID = @AttributeID";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@AttributeID",attributeId);
        using(SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        {
          if (dataReader.Read())
          {
            data = new ProductAttribute()
            {
              ProductID = Convert.ToInt32(dataReader["ProductID"]),
              AttributeID = Convert.ToInt32(dataReader["AttributeID"]),
              AttributeName = Convert.ToString(dataReader["AttributeName"]),
              AttributeValue = Convert.ToString(dataReader["AttributeValue"]),
              DisplayOrder = Convert.ToInt32(dataReader["DisplayOrder"])
            };
          }
        }
        connection.Close();
      }
      return data;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    public ProductEx GetEx(int productId)
    {
      ProductEx data = null;
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = cn.CreateCommand();
        cmd.CommandText = @"  SELECT *
                           FROM Products as p
                          left join ProductAttributes as pa on p.ProductID = pa.ProductID
                          left join ProductGallery as pg on p.ProductID = pg.ProductID 
                            WHERE  p.ProductID = @ProductID";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@ProductID", productId);
        using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        {
          if (dbReader.Read())
          {
            data = new ProductEx()
            {
              ProductID = Convert.ToInt32(dbReader["ProductID"]),
              ProductName = Convert.ToString(dbReader["ProductName"]),
              SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
              CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
              Unit = Convert.ToString(dbReader["Unit"]),
              Price = Convert.ToDecimal(dbReader["Price"]),
              Photo = Convert.ToString(dbReader["Photo"])
            };
          }
          
        }
        cn.Close();
      }
      return data;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="galleryId"></param>
    /// <returns></returns>
    public ProductGallery GetGallery(long galleryId)
    {
      throw new NotImplementedException();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="categoryId"></param>
    /// <param name="supplierId"></param>
    /// <param name="searchValue"></param>
    /// <returns></returns>
    public List<Product> List(int page, int pageSize, int categoryId, int supplierId, string searchValue)
    {
      if (searchValue != "")
        searchValue = "%" + searchValue + "%";
      List<Product> data = new List<Product>();
      using(SqlConnection connection = GetConnection())
      {
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = @"SELECT  *
                                    FROM
                                    (
                                        SELECT  *, ROW_NUMBER() OVER(ORDER BY ProductID) AS RowNumber
                                        FROM    Products 
                                        WHERE   (@categoryId = 0 OR CategoryId = @categoryId)
                                            AND  (@supplierId = 0 OR SupplierId = @supplierId)
                                            AND (@searchValue = '' OR ProductName LIKE @searchValue)
                                    ) AS s
                                    WHERE s.RowNumber BETWEEN (@page - 1)*@pageSize + 1 AND @page*@pageSize";
        cmd.CommandType = CommandType.Text;

        cmd.Parameters.AddWithValue("@page", page);
        cmd.Parameters.AddWithValue("@pageSize", pageSize);
        cmd.Parameters.AddWithValue("@categoryId", categoryId);
        cmd.Parameters.AddWithValue("@supplierId", supplierId);
        cmd.Parameters.AddWithValue("@searchValue", searchValue);

        using(SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        {
          while (dbReader.Read())
          {
            data.Add(new Product()
            {
              ProductID = Convert.ToInt32(dbReader["ProductID"]),
              ProductName = Convert.ToString(dbReader["ProductName"]),
              Unit = Convert.ToString(dbReader["Unit"]),
              Price = Convert.ToDecimal(dbReader["Price"]),
              Photo = Convert.ToString(dbReader["Photo"]),
              CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
              SupplierID = Convert.ToInt32(dbReader["SupplierID"])
            });
          }
        }

        connection.Close();
      }
      return data;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    public List<ProductAttribute> ListAttributes(int productId)
    {
      List<ProductAttribute> data = new List<ProductAttribute>();
      using(SqlConnection connection = GetConnection())
      {
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = @"SELECT *
                            FROM ProductAttributes
                            WHERE ProductID = @ProductID";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@ProductID", productId);
        using(SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        {
          while (dataReader.Read())
          {
            data.Add(new ProductAttribute()
            {
              ProductID = Convert.ToInt32(dataReader["ProductID"]),
              AttributeID = Convert.ToInt64(dataReader["AttributeID"]),
              AttributeName = Convert.ToString(dataReader["AttributeName"]),
              AttributeValue = Convert.ToString(dataReader["AttributeValue"]),
              DisplayOrder = Convert.ToInt32(dataReader["DisplayOrder"])
            }) ;
          }
        }
        connection.Close();
      }
      return data;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    public List<ProductGallery> ListGalleries(int productId)
    {
      List<ProductGallery> data = new List<ProductGallery>();
      using (SqlConnection connection = GetConnection())
      {
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = @"SELECT * FROM ProductGalleries
                            WHERE ProductID = @ProductID";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@ProductID", productId);
        using(SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        {
          while (dataReader.Read())
          {
            data.Add(new ProductGallery()
            {
              ProductID = Convert.ToInt32(dataReader["ProductID"]),
              GalleryID = Convert.ToInt64(dataReader["GalleryID"]),
              Photo = Convert.ToString(dataReader["Photo"]),
              Description = Convert.ToString(dataReader["Description"]),
              DisplayOrder = Convert.ToInt32(dataReader["DisplayOrder"]),
              IsHidden = Convert.ToBoolean(dataReader["IsHidden"]),
            });
          }
        }
        connection.Close();
      }
      return data;
    }

    public bool Update(Product data)
    {
      bool result = false;
      using(SqlConnection connection = GetConnection())
      {
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = @"UPDATE Products SET
                                            ProductName = @ProductName,
                                            SupplierID = @SupplierID,
                                            CategoryID = @CategoryID,
                                            Unit = @Unit,
                                            Price = @Price,
                                            Photo = @Photo
                                            WHERE ProductID = @ProductID";

        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@ProductID", data.ProductID);
        cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
        cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
        cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
        cmd.Parameters.AddWithValue("@Unit", data.Unit);
        cmd.Parameters.AddWithValue("@Price", data.Price);
        cmd.Parameters.AddWithValue("@Photo", data.Photo);

        result = cmd.ExecuteNonQuery() > 0;
        connection.Close();
      }
      return result;
    }

    public bool UpdateAttribute(ProductAttribute data)
    {
      bool result = false;
      using(SqlConnection connection = GetConnection())
      {
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = @"UPDATE ProductAttributes SET
                                    ProductID = @ProductID,
                                    AttributeName = @AttributeName,
                                    AttributeValue = @AttributeValue,
                                    DisplayOrder = @DisplayOrder
                            WHERE AttributeID = @AttributeID";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@ProductID", data.ProductID);
        cmd.Parameters.AddWithValue("@AttributeName", data.AttributeName);
        cmd.Parameters.AddWithValue("@AttributeValue", data.AttributeValue);
        cmd.Parameters.AddWithValue("@DisplayOrder", data.DisplayOrder);
        cmd.Parameters.AddWithValue("@AttributeID", data.AttributeID);

        result = cmd.ExecuteNonQuery() > 0;
        connection.Close();
      }
      return result;
    }

    public bool UpdateGallery(ProductGallery data)
    {
      bool result = false;
      using(SqlConnection connection = GetConnection())
      {
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = @"UPDATE ProductGallery SET
                                              ProductID = @ProductID,
                                              Photo =@Photo,
                                              Description =@Description,
                                              DisplayOrder = @DisplayOrder,
                                              IsHidden = @IsHidden
                           WHERE GalleryID = @GalleryID ";
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@ProductID", data.ProductID);
        cmd.Parameters.AddWithValue("@Photo", data.Photo);
        cmd.Parameters.AddWithValue("@Description", data.Description);
        cmd.Parameters.AddWithValue("@DisplayOrder", data.DisplayOrder);
        cmd.Parameters.AddWithValue("@IsHidden", data.IsHidden);
        cmd.Parameters.AddWithValue("@GalleryID", data.GalleryID);

        result = cmd.ExecuteNonQuery() > 1;
        connection.Close();
      }
      return result;
    }
  }
}

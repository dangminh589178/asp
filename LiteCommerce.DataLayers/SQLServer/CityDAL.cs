using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;
using System.Data.SqlClient;

namespace LiteCommerce.DataLayers.SQLServer
{
  public class CityDAL : _BaseDAL, ICityDAL
  {
    public CityDAL(string connectionString) : base(connectionString)
    { }

    public List<City> List()
    {
      return List("");
    }

    public List<City> List(string countryName)
    {
      List<City> data = new List<City>();
      using (SqlConnection cn = GetConnection())
      {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = @"SELECT * FROM Cities WHERE @CountryName = '' OR CountryName = @CountryName";
        cmd.CommandType = System.Data.CommandType.Text;
        cmd.Connection = cn;
        // truyen value de thuc hien cau lenh truy van
        cmd.Parameters.AddWithValue("@CountryName", countryName);

        using (SqlDataReader dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)) // ExecuteReader:
        {
          while (dbReader.Read())
          {
            // Cach 1
            //Country country = new Country();
            //country.CountryName = Convert.ToString(dbReader["CountryName"]);
            //data.Add(country);


            // Cach 2
            //Country country = new Country()
            //{
            //    CountryName = Convert.ToString(dbReader["CountryName"])
            //};
            //data.Add(country);

            // Cach 3
            data.Add(new City()
            {
              // Nhấn Ctrl + j hoặc Ctrl + Space để gợi ý các trường
              CityName = Convert.ToString(dbReader["CityName"]),
              CountryName = Convert.ToString(dbReader["CountryName"])
            });
          }
        }
        cn.Close();
      }
      return data;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using Population;

namespace Business_Logic.Cities
{
    /// <summary>
    /// CitiesService
    /// </summary>
    public static class CitiesService
    {
        /// <summary>
        /// Get all cities
        /// </summary>
        /// <returns>List(City) of all cities</returns>
        public static List<City> GetAll()
        {
            var dt = Connect.GetData("SELECT * FROM nhsCities", "nhsCities");
            var cities = (from DataRow dataRow in dt.Rows
                select new City()
                {
                    Id = int.Parse(dataRow["nhsCityID"].ToString().Trim()),
                    Name = dataRow["nhsCity"].ToString().Trim()
                }).ToList();
            return cities.OrderBy(x => x.Name).ToList();
        }
        /// <summary>
        /// Get all cities in datatable
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetAllDataTable()
        {
            var dt = Connect.GetData("SELECT * FROM nhsCities", "nhsCities");
            return dt;
        }
        /// <summary>
        /// Get all cities in DataSet
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetAllDataSet()
        {
            var dt = Connect.GetData("SELECT * FROM nhsCities", "nhsCities");
            var ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }
        /// <summary>
        /// Get city by id
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>City</returns>
        public static City GetCity(int cityId)
        {
            var dt = Connect.GetData("SELECT * FROM nhsCities WHERE nhsCityID=" + cityId, "nhsCities");
            return new City() { Id = int.Parse(dt.Rows[0]["nhsCityID"].ToString().Trim()), Name = dt.Rows[0]["nhsCity"].ToString().Trim() };
        }
        /// <summary>
        /// Add a new city
        /// </summary>
        /// <param name="c">City</param>
        /// <returns>success</returns>
        public static bool Add(City c)
        {
            return Connect.InsertUpdateDelete("INSERT INTO nhsCities (nhsCity) VALUES('" + c.Name + "')");
        }
        /// <summary>
        /// Updates city from service
        /// </summary>
        public static void UpdateCities()
        {
            UpdateDb();
        }
        /// <summary>
        /// Updates city from service 
        /// </summary>
        private static void UpdateDb()
        {
            try
            {
                //This method is using an external webservice -
                //http://www.loveburn.com/school/WebServiceCities.asmx - A service for getting all the cities in israel
                //Author of service: Meir Dahan
                //Namespace: Population
                //Service: WebServiceCities
                //Method: GetAllCitiesFromIsrael
                var cityService = new WebServiceCities();
                var dtWs = cityService.GetAllCitiesFromIsrael().Tables[0];
                var dtAll = Connect.GetData("SELECT * FROM nhsCities ORDER BY nhsCity", "nhsCities");
                var con = new OleDbConnection(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
                con.Open();
                var cmd = new OleDbCommand
                {
                    Connection = con,
                    CommandText = "INSERT INTO nhsCities(nhsCity) VALUES(@nhsCity)"
                };
                cmd.Parameters.Add("@nhsCity", OleDbType.VarWChar, 255);
                var transaction = con.BeginTransaction();
                cmd.Transaction = transaction;
                for (var i = 0; i < dtWs.Rows.Count; i++)
                {
                    var cityName = dtWs.Rows[i]["Heb"].ToString();
                    var exists = dtAll.AsEnumerable().Any(x => x.Field<string>("nhsCity").Equals(cityName));//Using lambda to check if exsits
                    if (exists) continue;
                    cmd.Parameters[0].Value = cityName;
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
                con.Close();
            }
            catch(Exception ex)
            {
                Problem.Log(ex);
            }
        }
    }
}
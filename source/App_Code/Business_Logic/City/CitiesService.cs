using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Configuration;
using Population;
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
        List<City> cities = new List<City>();
        DataTable dt = Connect.GetData("SELECT * FROM nhsCities", "nhsCities");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            City c = new City()
            {
                ID = int.Parse(dt.Rows[i]["nhsCityID"].ToString().Trim()),
                Name = dt.Rows[i]["nhsCity"].ToString().Trim()
            };
            cities.Add(c);
        }
        return cities.OrderBy(x => x.Name).ToList();
    }
    /// <summary>
    /// Get all cities in datatable
    /// </summary>
    /// <returns>DataTable</returns>
    public static DataTable GetAllDT()
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsCities", "nhsCities");
        return dt;
    }
    /// <summary>
    /// Get all cities in DataSet
    /// </summary>
    /// <returns>DataSet</returns>
    public static DataSet GetAllDS()
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsCities", "nhsCities");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        return ds;
    }
    /// <summary>
    /// Get city by id
    /// </summary>
    /// <param name="cityID">The id of the city</param>
    /// <returns>City</returns>
    public static City GetCity(int cityID)
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsCities WHERE nhsCityID=" + cityID, "nhsCities");
        return new City() { ID = int.Parse(dt.Rows[0]["nhsCityID"].ToString().Trim()), Name = dt.Rows[0]["nhsCity"].ToString().Trim() };
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
        updateDB();
    }
    /// <summary>
    /// Updates city from service 
    /// </summary>
    private static void updateDB()
    {
        try
        {
            //This method is using an external webservice -
            //http://www.loveburn.com/school/WebServiceCities.asmx - A service for getting all the cities in israel
            //Author of service: Meir Dahan
            //Namespace: Population
            //Service: WebServiceCities
            //Method: GetAllCitiesFromIsrael
            WebServiceCities cityService = new WebServiceCities();
            DataTable dtWS = cityService.GetAllCitiesFromIsrael().Tables[0];
            DataSet ds = cityService.GetAllCitiesFromIsrael();
            DataTable dtAll = Connect.GetData("SELECT * FROM nhsCities ORDER BY nhsCity", "nhsCities");
            OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
            con.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = "INSERT INTO nhsCities(nhsCity) VALUES(@nhsCity)";
            cmd.Parameters.Add("@nhsCity", OleDbType.VarWChar, 255);
            OleDbTransaction transaction = con.BeginTransaction();
            cmd.Transaction = transaction;
            for (int i = 0; i < dtWS.Rows.Count; i++)
            {
                string cityName = dtWS.Rows[i]["Heb"].ToString();
                bool exists = dtAll.AsEnumerable().Where(x => x.Field<string>("nhsCity").Equals(cityName)).Count() > 0;//Using lambda to check if exsits
                if (!exists)
                {
                    cmd.Parameters[0].Value = cityName;
                    cmd.ExecuteNonQuery();
                }
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
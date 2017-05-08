using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

/// <summary>
/// This class is connecting the application to the database,
/// mainly used in the business logic classes.
/// </summary>
public static class Connect
{
    /// <summary>
    /// Counts the queries that excuted for the page loaded
    /// </summary>
    public static int QueriesCount;
    /// <summary>
    ///  Return's Result For The SQL Query(DataTable Object)
    /// </summary>
    public static DataTable GetData(string sqlQuery, string tableName)
    {
        var con = new OleDbConnection(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
        var command = new OleDbCommand(sqlQuery, con);
        con.Open();
        var adp = new OleDbDataAdapter(command);
        var ds = new DataSet();
        
        adp.Fill(ds, tableName);
        con.Close();
        QueriesCount++;
        return ds.Tables[0];
    }
    /// <summary>
    ///  Return's Result For The SQL Query(DataSet Object)
    /// </summary>
    public static DataSet GetData(string sqlQuery, string tableName, bool wantFullDataSet)
    {
        var con = new OleDbConnection(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
        var command = new OleDbCommand(sqlQuery, con);
        var adp = new OleDbDataAdapter(command);
        var ds = new DataSet();
        adp.Fill(ds, tableName);
        QueriesCount++;
        return ds;
    }
    /// <summary>
    /// Return's Object Result (It's Mainly For Mathmatical Use)
    /// </summary>
    public static object GetObject(string sqlQuery)
    {
        try
        {
            var con = new OleDbConnection(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
            var command = new OleDbCommand(sqlQuery, con);
            con.Open();
            var myObj = command.ExecuteScalar();
            con.Close();
            QueriesCount++;
            return myObj;
        }
        catch(Exception ex)
        {
            Problem.Log(ex,sqlQuery);
            return null;
        }
    }
    /// <summary>
    /// Executes The SqlQuery And Return's Boolean For Success Of The Query Execution
    /// </summary>
    public static bool InsertUpdateDelete(string sqlQuery)
    {
        try
        {
            var con = new OleDbConnection(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
            var command = new OleDbCommand(sqlQuery, con);
            con.Open();
            command.ExecuteNonQuery();
            con.Close();
            QueriesCount++;
            return true;
        }
        catch (Exception ex)
        {
            Problem.Log(ex, sqlQuery);
            return false;
        }
    }
    /// <summary>
    /// Executes The SqlQuery And Return's Boolean For Success Of The Query Execution
    /// </summary>
    public static int InsertUpdateDeleteState(string sqlQuery)
    {
        try
        {
            var con = new OleDbConnection(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
            var command = new OleDbCommand(sqlQuery, con);
            con.Open();
            var rows = command.ExecuteNonQuery();
            con.Close();
            QueriesCount++;
            return rows;
        }
        catch (Exception ex)
        {
            Problem.Log(ex, sqlQuery);
        }
        return 0;
    }
}
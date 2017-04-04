using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Web.Configuration;
/// <summary>
/// This class is connecting the application to the database,
/// mainly used in the business logic classes.
/// </summary>
public static class Connect
{
    public static int QueriesCount;
    /// <summary>
    ///  Return's Result For The SQL Query(DataTable Object)
    /// </summary>
    public static DataTable GetData(string SqlQuery, string TableName)
    {
        OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["eduDB_Connection"].ConnectionString);
        OleDbCommand command = new OleDbCommand(SqlQuery, con);
        con.Open();
        OleDbDataAdapter adp = new OleDbDataAdapter(command);
        DataSet ds = new DataSet();
        
        adp.Fill(ds, TableName);
        con.Close();
        QueriesCount++;
        return ds.Tables[0];
    }
    /// <summary>
    ///  Return's Result For The SQL Query(DataSet Object)
    /// </summary>
    public static DataSet GetData(string SqlQuery, string TableName, bool WantFullDataSet)
    {
        OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["eduDB_Connection"].ConnectionString);
        OleDbCommand command = new OleDbCommand(SqlQuery, con);
        OleDbDataAdapter adp = new OleDbDataAdapter(command);
        DataSet ds = new DataSet();
        adp.Fill(ds, TableName);
        QueriesCount++;
        return ds;
    }
    /// <summary>
    /// Return's Object Result (It's Mainly For Mathmatical Use)
    /// </summary>
    public static object GetObject(string SqlQuery)
    {
        try
        {
            OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["eduDB_Connection"].ConnectionString);
            OleDbCommand command = new OleDbCommand(SqlQuery, con);
            con.Open();
            object MyObj = command.ExecuteScalar();
            con.Close();
            QueriesCount++;
            return MyObj;
        }
        catch(Exception ex)
        {
            //Problem.Log(ex);
            return null;
        }
    }
    /// <summary>
    /// Executes The SqlQuery And Return's Boolean For Success Of The Query Execution
    /// </summary>
    public static bool InsertUpdateDelete(string SqlQuery)
    {
        try
        {
            OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["eduDB_Connection"].ConnectionString);
            OleDbCommand command = new OleDbCommand(SqlQuery, con);
            con.Open();
            command.ExecuteNonQuery();
            con.Close();
            QueriesCount++;
            return true;
        }
        catch (Exception ex)
        {
            //Problem.Log(ex);
        }
        return false;  
    }
}
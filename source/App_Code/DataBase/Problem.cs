using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Logging errors and track to txt files
/// </summary>
public static class Problem
{
    /// <summary>
    /// Log an Exception
    /// </summary>
    /// <param name="ex">Exception occured in system</param>
    public static void Log(Exception ex)
    {
        if (ex is HttpException)
        {
            AddLogUI(ex.ToString());
        }
        else if (ex is System.Data.OleDb.OleDbException)
        {
            AddLogDB(ex.ToString());
        }
        else
        {
            AddLogApp(ex.ToString());
        }
    }
    public static void Log(Exception ex,string dataQuery)
    {

        if (ex is HttpException)
        {
            AddLogUI(ex.ToString()+ "\r\n QUERY= " + dataQuery);
        }
        else if (ex is System.Data.OleDb.OleDbException)
        {
            AddLogDB(ex.ToString() + "\r\n QUERY= " + dataQuery);
        }
        else
        {
            AddLogApp(ex.ToString() + "\r\n QUERY= " + dataQuery);
        }
    }
    /// <summary>
    /// Save the error to app log
    /// </summary>
    /// <param name="error">Error content</param>
    private static void AddLogApp(string error)
    {
        using (StreamWriter w = File.AppendText(HttpContext.Current.Server.MapPath("~/App_Data/Logs/app_log.txt")))
        {
            Log(error, w);
        }
    }
    /// <summary>
    /// Save the error to DB log
    /// </summary>
    /// <param name="error">Error content</param>
    private static void AddLogDB(string error)
    {
        using (StreamWriter w = File.AppendText(HttpContext.Current.Server.MapPath("~/App_Data/Logs/db_log.txt")))
        {
            Log(error, w);
        }
    }
    /// <summary>
    /// Save the error to UI log
    /// </summary>
    /// <param name="error">Error content</param>
    private static void AddLogUI(string error)
    {
        using (StreamWriter w = File.AppendText(HttpContext.Current.Server.MapPath("~/App_Data/Logs/ui_log.txt")))
        {
            Log(error, w);
        }
    }
    /// <summary>
    /// Logs to file
    /// </summary>
    /// <param name="logMessage">Log Message</param>
    /// <param name="w">TextWriter/StreamWriter</param>
    public static void Log(string logMessage, TextWriter w)
    {
        w.Write("\r\n AVIVNET Log Entry : ");
        w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
            DateTime.Now.ToLongDateString());
        w.WriteLine("  :");
        w.WriteLine("  :{0}", logMessage);
        w.WriteLine("-------------------------------");
    }

}
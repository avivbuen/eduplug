using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for State
/// </summary>
public static class State
{
    public static void SaveAction(string action)
    {
        //AddLogApp(action);
    }
    private static void AddLogApp(string error)
    {
        using (StreamWriter w = File.AppendText(HttpContext.Current.Server.MapPath("~/App_Data/Logs/app_log.txt")))
        {
            Log(error, w);
        }
    }
    /// <summary>
    /// Logs to file
    /// </summary>
    /// <param name="logMessage">Log Message</param>
    /// <param name="w">TextWriter/StreamWriter</param>
    private static void Log(string logMessage, TextWriter w)
    {
        w.Write("\r\n AVIVNET Log Entry : ");
        w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
            DateTime.Now.ToLongDateString());
        w.WriteLine("  :");
        w.WriteLine("  :{0}", logMessage);
        w.WriteLine("-------------------------------");
    }
}
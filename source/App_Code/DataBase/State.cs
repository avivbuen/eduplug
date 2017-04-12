using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Deals with txt log
/// </summary>
public static class State
{
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
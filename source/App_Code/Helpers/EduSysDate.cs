using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EduSysDate
/// </summary>
public static class EduSysDate
{
    public static DateTime GetStart()
    {
        if (HttpContext.Current.Session == null) return DateTimeExtensions.StartOfYear(DateTime.Now);
        if (HttpContext.Current.Session["eduSysDate"] == null)
            HttpContext.Current.Session["eduSysDate"] = DateTimeExtensions.StartOfYear(DateTime.Now);
        return DateTimeExtensions.StartOfYear((DateTime)HttpContext.Current.Session["eduSysDate"]);
    }
    public static DateTime GetEnd()
    {
        if (HttpContext.Current.Session == null) return DateTimeExtensions.EndOfYear(DateTime.Now);
        if (HttpContext.Current.Session["eduSysDate"] == null)
            HttpContext.Current.Session["eduSysDate"] = DateTimeExtensions.EndOfYear(DateTime.Now);
        return DateTimeExtensions.EndOfYear((DateTime)HttpContext.Current.Session["eduSysDate"]);
    }
    public static bool SetYear(int year)
    {
        if (HttpContext.Current.Session == null) return false;
        DateTime date = new DateTime(year, DateTime.Now.Month, DateTime.Now.Day);
        HttpContext.Current.Session["eduSysDate"] = date;
        return true;
    }
    public static int GetYear(DateTime date)
    {
        if (HttpContext.Current.Session["eduSysDate"] == null)
            HttpContext.Current.Session["eduSysDate"] = DateTime.Now;
        if (date.Month>=1&&date.Month<=8)
        {
            return GetEnd().Year;
        }
        else
        {
            return GetStart().Year;
        }
    }
    public static string GetYearPart(DateTime date)
    {
        if (date.Month>=1&&date.Month<8)
        {
            return "b";
        }
        else
        {
            return "a";
        }
    }
}
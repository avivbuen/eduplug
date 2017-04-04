using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// DateTime Extensions
/// </summary>
public static class DateTimeExtensions
{
    public static DateTime StartOfWeek(object obj, DayOfWeek startOfWeek)
    {
        if (obj == null)
            obj = DateTime.Now;
        return StartOfWeek((DateTime)obj, startOfWeek);
    }
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = startOfWeek - dt.DayOfWeek;
        return dt.AddDays(diff).Date;
    }
    public static DateTime StartOfYear(this DateTime dt)
    {
        if (dt.Month>=9)
        {
            return new DateTime(dt.Year, 9, 1);
        }
        return new DateTime(dt.Year - 1, 9, 1);
    }
    public static DateTime EndOfYear(this DateTime dt)
    {
        if (dt.Month >= 9)
        {
            return new DateTime(dt.Year + 1, 7, 20);
        }
        return new DateTime(dt.Year, 7, 20);
    }
}
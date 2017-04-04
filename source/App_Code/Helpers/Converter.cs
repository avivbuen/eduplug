﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Converter
/// </summary>
public static class Converter
{
    /// <summary>
    /// Prepares the current time for Access database
    /// </summary>
    /// <returns>DateTime.Now - Converted To DB string</returns>
    public static string GetFullTimeReadyForDataBase()
    {
        return GetTimeReadyForDataBase(DateTime.Now);
    }

    /// <summary>
    /// Prepares the time for Access database
    /// </summary>
    /// <param name="time">The time you want to convert</param>
    /// <returns>DateTime time - Converted To DB string</returns>
    public static string GetTimeReadyForDataBase(DateTime time)
    {
        return time.ToString("yyyy-MM-dd H:mm:ss");
    }
    /// <summary>
    /// Prepares the time for Access database
    /// </summary>
    /// <param name="time">The time you want to convert</param>
    /// <returns>DateTime time - Converted To DB string</returns>
    public static string GetTimeShortForDataBase(DateTime time)
    {
        return time.ToString("yyyy-MM-dd");
    }
    public static string GetClearnce(MemberClearance mem)
    {
        switch (mem)
        {
            case MemberClearance.Guest:
                return "g";
            case MemberClearance.Student:
                return "s";
            case MemberClearance.Teacher:
                return "t";
            case MemberClearance.Admin:
                return "a";
        }
        return "f";
    }
    public static string GetGender(MemberGender gen)
    {
        switch (gen)
        {
            case MemberGender.Male:
                return "m";
            case MemberGender.Female:
                return "f";
            case MemberGender.Unknown:
                return "u";
        }
        return "u";
    }
}
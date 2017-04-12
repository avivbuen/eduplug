using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Data;
/// <summary>
/// Gathers intelligence on client and system, also provides help to ui
/// </summary>
public static class Intel
{

    /// <summary>
    /// Returns the project's root url in order to use the master page in all the web directories
    /// </summary>
    /// <returns>Project's root url</returns>
    public static string GetFullRootUrl()
    {
        //The request object is not available in this portion of asp.net without adding assemblies so i got it from the current httpcontext object
        HttpRequest request = HttpContext.Current.Request;//Get the current request from current HttpContext
        string url = request.Url.AbsoluteUri.Replace(request.Url.AbsolutePath, String.Empty);//Get current root url from request
        if (url.Contains("?"))
        {
            url = url.Remove(url.IndexOf("?"));
        }
        return url;
    }
    /// <summary>
    /// Redirects the user to his panel
    /// </summary>
    public static void Redirect()
    {
        HttpResponse Response = HttpContext.Current.Response;
        switch (MemberService.GetCurrent().Auth)
        {
            case MemberClearance.Guest:
                Response.Redirect("~/User");
                break;
            case MemberClearance.Student:
                Response.Redirect("~/Panel/Student");
                break;
            case MemberClearance.Teacher:
                Response.Redirect("~/Panel/Teacher");
                break;
            case MemberClearance.Admin:
                Response.Redirect("~/Panel/Admin");
                break;
            default:
                Response.Redirect("~/User");
                break;
        }
    }
    /// <summary>
    /// Gets the ip address of the client
    /// </summary>
    /// <returns>String - IP Address of client</returns>
    public static string GetIpAddress()
    {
        string VisitorsIPAddr = string.Empty;
        if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        {
            VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        }
        else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
        {
            VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
        }
        if (VisitorsIPAddr == "::1")
            return "מקומי על מערכת " + GetUserPlatform();
        return VisitorsIPAddr;
    }
    /// <summary>
    /// Mark an entry as reviewed
    /// </summary>
    /// <param name="ip">IP of entry</param>
    /// <param name="state">Rev/NoRev</param>
    /// <returns>success</returns>
    public static bool MarkRev(string ip,bool state)
    {
        string dataDir = HttpContext.Current.Server.MapPath("~/App_Data");//The file directory
        string xmlFilePath = dataDir + @"/EduEntries.xml";//The xml file path
        DataSet ds = new DataSet("EduEntries");
        DataTable dt = new DataTable("EntryPoint");
        if (Directory.Exists(dataDir))
        {
            string[] files = Directory.GetFiles(dataDir);
            if (!File.Exists(xmlFilePath))
            {
                return false;
            }
            else
            {
                ds.ReadXml(xmlFilePath);//Reading the xml from the file
                dt = ds.Tables[0];//Reading the table from the DataSet object
            }
            foreach (DataRow d_r in dt.Rows)
            {
                if (ip == d_r["VisitorIP"].ToString())
                {
                    d_r["Rev"] = state;
                    ds.Tables.Clear();
                    ds.Tables.Add(dt);
                    ds.WriteXml(xmlFilePath);
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// Gets all the entries
    /// </summary>
    /// <returns>All the entries</returns>
    public static DataTable GetEduSense()
    {
        string dataDir = HttpContext.Current.Server.MapPath("~/App_Data");//The file directory
        string xmlFilePath = dataDir + @"/EduEntries.xml";//The xml file path
        DataSet ds = new DataSet("EduEntries");
        DataTable dt = new DataTable("EntryPoint");
        if (Directory.Exists(dataDir))
        {
            string[] files = Directory.GetFiles(dataDir);
            if (!File.Exists(xmlFilePath))
            {
                //Incase file does not exsit, we create a new table for a new file
                //Adding columns

                DataColumn dc;//Creating a DataColumn object for inserting the columns

                dc = new DataColumn("VisitorIP");//New column create
                dt.Columns.Add(dc);//New column add

                dc = new DataColumn("VisitorCountry");//New column create
                dt.Columns.Add(dc);//New column add

                dc = new DataColumn("VisitorISP");//New column create
                dt.Columns.Add(dc);//New column add

                dc = new DataColumn("VistServerTime");//New column create
                dt.Columns.Add(dc);//New column add

                dc = new DataColumn("VisitorDeviceFamily");//New column create
                dt.Columns.Add(dc);//New column add

                dc = new DataColumn("VisitorOS");//New column create
                dt.Columns.Add(dc);//New column add

                dc = new DataColumn("UserID");//New column create
                dt.Columns.Add(dc);//New column add

                dc = new DataColumn("Rev");//New column create
                dt.Columns.Add(dc);//New column add
            }
            else
            {
                ds.ReadXml(xmlFilePath);//Reading the xml from the file
                dt = ds.Tables[0];//Reading the table from the DataSet object
            }
        }
        return dt;
    }

    /// <summary>
    /// Gets the user device OS
    /// </summary>
    /// <returns>Client device OS</returns>
    public static string GetUserPlatform()
    {
        var ua = HttpContext.Current.Request.UserAgent;

        if (ua.Contains("Android"))
            return string.Format("{0} אנדרואיד", GetMobileVersion(ua, "Android"));

        if (ua.Contains("iPad"))
            return string.Format("{0} אייפד מערכת", GetMobileVersion(ua, "OS"));

        if (ua.Contains("iPhone"))
            return string.Format("{0} אייפון מערכת", GetMobileVersion(ua, "OS"));

        if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
            return "טאבלט אמאזון";

        if (ua.Contains("RIM Tablet") || (ua.Contains("BB") && ua.Contains("Mobile")))
            return "בלאקברי";

        if (ua.Contains("Windows Phone"))
            return string.Format("{0} טלפון חלונות", GetMobileVersion(ua, "Windows Phone"));

        if (ua.Contains("Mac OS"))
            return "מק-אפל";

        if (ua.Contains("Windows NT 5.1") || ua.Contains("Windows NT 5.2"))
            return "חלונות אקס-פי";

        if (ua.Contains("Windows NT 6.0"))
            return "חלונות ויסטה";

        if (ua.Contains("Windows NT 6.1"))
            return "חלונות 7";

        if (ua.Contains("Windows NT 6.2"))
            return "חלונות 8";

        if (ua.Contains("Windows NT 6.3"))
            return "חלונות 8.1";

        if (ua.Contains("Windows NT 10"))
            return "חלונות 10";

        //fallback to basic platform:
        return HttpContext.Current.Request.Browser.Platform + (ua.Contains("Mobile") ? " Mobile " : "");
    }
    /// <summary>
    /// Get the current device family of the client
    /// </summary>
    /// <returns>Client Device Family</returns>
    public static string GetDeviceFamily()
    {
        var ua = HttpContext.Current.Request.UserAgent;
        if (ua.Contains("Android"))
            return "Android";

        if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
            return "Amazon";

        if (ua.Contains("RIM Tablet") || (ua.Contains("BB") && ua.Contains("Mobile")))
            return "BlackBerry";

        if (ua.Contains("Windows Phone"))
            return "MS-Windows-Mobile";

        if (ua.Contains("Mac OS") || ua.Contains("iPad") || ua.Contains("iPhone"))
            return "Apple";

        if (ua.Contains("Windows"))
            return "MS-Windows";
        return "Unknown Family";
    }
    /// <summary>
    /// Gets the version of the mobile OS of client
    /// </summary>
    /// <param name="userAgent">The user agent of the request</param>
    /// <param name="device">The device of the client</param>
    /// <returns>The mobile version of the device</returns>
    public static string GetMobileVersion(string userAgent, string device)
    {
        var temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
        var version = string.Empty;

        foreach (var character in temp)
        {
            var validCharacter = false;
            int test = 0;

            if (Int32.TryParse(character.ToString(), out test))
            {
                version += character;
                validCharacter = true;
            }

            if (character == '.' || character == '_')
            {
                version += '.';
                validCharacter = true;
            }

            if (validCharacter == false)
                break;
        }

        return version;
    }
    /// <summary>
    /// Delete entry
    /// </summary>
    /// <param name="ip">IP</param>
    /// <returns>success</returns>
    public static bool DeleteVisit(string ip)
    {
        string dataDir = HttpContext.Current.Server.MapPath("~/App_Data");//The file directory
        string xmlFilePath = dataDir + @"/EduEntries.xml";//The xml file path
        DataSet ds = new DataSet("EduEntries");
        DataTable dt = new DataTable("EntryPoint");
        if (Directory.Exists(dataDir))
        {
            string[] files = Directory.GetFiles(dataDir);
            if (!File.Exists(xmlFilePath))
            {
                return false;
            }
            else
            {
                ds.ReadXml(xmlFilePath);//Reading the xml from the file
                dt = ds.Tables[0];//Reading the table from the DataSet object
            }
            foreach (DataRow d_r in dt.Rows)
            {
                if (ip == d_r["VisitorIP"].ToString())
                {
                    dt.Rows.Remove(d_r);
                    ds.Tables.Clear();
                    ds.Tables.Add(dt);
                    ds.WriteXml(xmlFilePath);
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// Saves the visitor entry in the DB
    /// </summary>
    public static void SaveVisit()
    {
        string ip = GetIpAddress();
        string dataDir = HttpContext.Current.Server.MapPath("~/App_Data");//The file directory
        string xmlFilePath = dataDir + @"/EduEntries.xml";//The xml file path
        DataSet ds = new DataSet("EduEntries");
        DataTable dt = new DataTable("EntryPoint");
        if (Directory.Exists(dataDir))
        {
            string[] files = Directory.GetFiles(dataDir);
            if (!File.Exists(xmlFilePath))
            {
                //Incase file does not exsit, we create a new table for a new file
                //Adding columns

                DataColumn dc;//Creating a DataColumn object for inserting the columns

                dc = new DataColumn("VisitorIP");//New column create
                dt.Columns.Add(dc);//New column add

                dc = new DataColumn("VisitorCountry");//New column create
                dt.Columns.Add(dc);//New column add

                dc = new DataColumn("VisitorISP");//New column create
                dt.Columns.Add(dc);//New column add

                dc = new DataColumn("VistServerTime");//New column create
                dt.Columns.Add(dc);//New column add

                dc = new DataColumn("VisitorDeviceFamily");//New column create
                dt.Columns.Add(dc);//New column add

                dc = new DataColumn("VisitorOS");//New column create
                dt.Columns.Add(dc);//New column add

                dc = new DataColumn("UserID");//New column create
                dt.Columns.Add(dc);//New column add

                dc = new DataColumn("Rev");//New column create
                dt.Columns.Add(dc);//New column add
            }
            else
            {
                ds.ReadXml(xmlFilePath);//Reading the xml from the file
                dt = ds.Tables[0];//Reading the table from the DataSet object
            }
            foreach (DataRow d_r in dt.Rows)
            {
                if (ip == d_r["VisitorIP"].ToString() && int.Parse(d_r["UserID"].ToString()) == -1 && MemberService.GetCurrent().UserID != 0)
                {
                    d_r["UserID"] = MemberService.GetCurrent().UserID;

                    ds.Tables.Clear();
                    ds.Tables.Add(dt);
                    ds.WriteXml(xmlFilePath);
                    return;
                }
                else if (ip == d_r["VisitorIP"].ToString()) return;
            }
            DataRow dr = dt.NewRow();
            dr["VisitorIP"] = ip;
            dr["VisitorCountry"] = GetCountry();
            dr["VisitorISP"] = GetISP();
            dr["VistServerTime"] = Converter.GetFullTimeReadyForDataBase();
            dr["VisitorDeviceFamily"] = GetDeviceFamily();
            dr["VisitorOS"] = GetUserPlatform();
            dr["Rev"] = false;
            if (MemberService.GetCurrent().Auth != MemberClearance.Guest)
                dr["UserID"] = MemberService.GetCurrent().UserID;
            else
                dr["UserID"] = -1;
            dt.Rows.Add(dr);
            ds.Tables.Clear();
            ds.Tables.Add(dt);
            ds.WriteXml(xmlFilePath);
        }
    }
    /// <summary>
    /// Uses an external API to resolve the current client country by the ip (The API - IP-API.COM)
    /// </summary>
    /// <returns>Client country</returns>
    public static string GetCountry()
    {
        if (GetIpAddress().Contains("מקומי") || GetIpAddress() == "127.0.0.1") return "מקומי";
        try
        {
            WebRequest request = WebRequest.Create("http://ip-api.com/json/" + GetIpAddress());
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string res = reader.ReadToEnd().ToString();
            return res.Substring(res.IndexOf("country") + 10, res.Substring(res.IndexOf("country") + 10).IndexOf(",") - 1);
        }
        catch
        {
            return "Unknown Location";
        }
    }
    /// <summary>
    ///  Uses an external API to resolve the current client ISP(Internet Service Provider) by the ip (The API - IP-API.COM)
    /// </summary>
    /// <returns>Client ISP</returns>
    public static string GetISP()
    {
        if (GetIpAddress().Contains("מקומי")) return "מקומי";
        try
        {
            WebRequest request = WebRequest.Create("http://ip-api.com/json/" + GetIpAddress());
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string res = reader.ReadToEnd().ToString();
            return res.Substring(res.IndexOf("isp") + 6, res.Substring(res.IndexOf("isp") + 6).IndexOf(",") - 1);
        }
        catch
        {
            return "Unknown ISP";
        }
    }
    /// <summary>
    /// Gets entries locations for graph
    /// </summary>
    /// <returns>Dictionary(string, object) of locations</returns>
    public static Dictionary<string, object> GetLocations()
    {
        DataTable dt = GetEduSense();
        Dictionary<string, object> data = new Dictionary<string, object>();
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["VisitorCountry"].ToString().Trim() != "מקומי")
            {
                if (!data.ContainsKey(dr["VisitorCountry"].ToString()))
                {
                    data.Add(dr["VisitorCountry"].ToString(), 1);
                }
                else
                {
                    data[dr["VisitorCountry"].ToString()] = int.Parse(data[dr["VisitorCountry"].ToString()].ToString()) + 1;
                }
            }
        }
        return data;
    }

}
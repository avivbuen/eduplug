<%@ WebHandler Language="C#" Class="Precent" %>

using System;
using System.Web;
using Business_Logic.Exams;

public class Precent : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        if (context.Request.Form["tgid"] == null || context.Request.Form["day"] == null || context.Request.Form["month"] == null || context.Request.Form["year"] == null)
        {
            context.Response.Write("0");
            return;//Cutting the method(No reason to do that now, but just to make sure)
        }
        int tgid = int.Parse(context.Request.Form["tgid"].ToString());
        int year = int.Parse(context.Request.Form["year"].ToString());
        int month = int.Parse(context.Request.Form["month"].ToString());
        int day = int.Parse(context.Request.Form["day"].ToString());
        DateTime date = new DateTime(year, month, day);
        context.Response.Write(ExamService.PrecentLeft(tgid, EduSysDate.GetYearPart(date)));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
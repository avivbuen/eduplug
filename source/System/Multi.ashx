<%@ WebHandler Language="C#" Class="Multi" %>

using System;
using System.Web;
using Business_Logic.Members;
using Newtonsoft.Json;

public class Multi : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        if (context.Request.Form["id"] != null)
        {
            string id = context.Request.Form["id"].ToString();//Getting the email from the 'POST'
            var schools = MemberService.GetSchools(id.Replace("'",""));
            var json = JsonConvert.SerializeObject(schools);
            context.Response.Write(json);
            return;
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
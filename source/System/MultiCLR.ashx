<%@ WebHandler Language="C#" Class="MultiCLR" %>

using System;
using System.Web;
using Business_Logic.Members;
using Newtonsoft.Json;

public class MultiCLR : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        try
        {
            if (context.Request.Form["id"] != null)
            {
                string id = context.Request.Form["id"].ToString(); //Getting the email from the 'POST'
                    string scid = context.Request.Form["scid"] == null ? "-1" :context.Request.Form["scid"].ToString(); //Getting the email from the 'POST'
                var clearances = MemberService.GetClearances(id.Replace("'", ""), int.Parse(scid));
                var json = JsonConvert.SerializeObject(clearances);
                context.Response.Write(json);
                return;
            }
        }
        catch (Exception ex)
        {
            Problem.Log(ex);
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}
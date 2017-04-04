﻿<%@ WebHandler Language="C#" Class="Current" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Net;
using System.Threading;

public class Current : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
{
    /// <summary>
    /// Gets the current user saved on session
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
        try
        {

            context.Response.ContentType = "text/plain";
            if (context.Request["logout"] != null)
            {
                MemberService.Logout();
                context.Response.Write("Bye Bye! <br/> YnllLCBieWUh");
                return;
            }
            if (context.Request.Form["id"] != null && context.Request.Form["pass"] != null)
            {
                string id = context.Request.Form["id"].ToString();//Getting the email from the 'POST'
                string pass = context.Request.Form["pass"].ToString();//Getting the password from the 'POST'
                if (MemberService.Login(id, pass, true))
                {
                    Member m = MemberService.GetCurrent();
                    string json = "{'fname':'" + m.FirstName + "','lname':'" + m.LastName + "','email':'" + m.Mail + "','pic':'" + m.PicturePath + "', 'clr':'" + ((char)m.Auth) + "','m_count':'" + MessagesService.GetUnreaed(m.UserID) + "'}";
                    context.Response.Write(json.Replace((char)39, (char)34));//Returning json to JavaScript on master page
                }
                else
                {
                    string emptyJson = "{'fname':'non','lname':'non'}";//Empty json user
                    context.Response.Write(emptyJson.Replace((char)39, (char)34));//Returning the empty json
                    context.Response.End();//Cutting the response and ending it
                    return;//Cutting the method(No reason to do that now, but just to make sure)
                }
                return;
            }
            string strTemplate = "{'fname':'{0}','lname':'{1}','email':'{2}','uid':'{3}','pic':'{4}','clr':'{5}','m_count':'{6}','grt':'{7}'}";
            string[] e = { "non", "non", "non", "non", "non", "non", "non", "non" };

            string eUser = FormStr(strTemplate, e);//Do not use (does not complie to json)
            string emptyUser = eUser.Replace((char)39, (char)34);
            if (ValidateSessions("Member", context))
            {
                Member m = MemberService.GetCurrent();
                string[] f = { m.FirstName, m.LastName, m.Mail, m.UserID.ToString(), m.PicturePath, ((char)m.Auth).ToString(), MessagesService.GetUnreaedCount(m.UserID).ToString(), MemberService.GetGreeting(MemberService.GetCurrent()) };
                context.Response.Write(FormStr(strTemplate, f).Replace((char)39, (char)34));
                context.Response.End();
                return;
            }
            else if (context.Request.Form["nada"] != null)
            {
                context.Response.Write(emptyUser);
                context.Response.End();
                return;
            }
            else
            {
                context.Response.Redirect("~/");
                context.Response.End();
                return;
            }
        }
        catch (ThreadAbortException ex)
        {
            return;
        }
    }
    private string FormStr(string str, string[] strs)//Was made because string.format didn't work - it does not except apostrophe near spacer
    {
        for (int i = 0; i < strs.Length; i++)
        {
            str = str.Replace("{" + i + "}", strs[i]);
        }
        return str;
    }
    private bool ValidateSessions(string sessionName, HttpContext c) { return ValidateSessions(new string[] { sessionName }, c); }
    private bool ValidateSessions(string[] sessionNames, HttpContext c)
    {
        foreach (string sName in sessionNames)
        {
            if (c == null || c.Session == null || c.Session[sName] == null || c.Session[sName].ToString().Trim() == "")
            {
                return false;
            }
        }
        return true;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
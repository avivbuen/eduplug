<%@ Application Language="C#" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="Business_Logic.Members" %>
<script runat="server">
    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        //State.SaveAction("APPLICATION START");
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
        //State.SaveAction("APPLICATION END");
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        Exception exc = Server.GetLastError();
        HttpContext con = HttpContext.Current;
        if (exc is ThreadAbortException)
        {
            return;
        }
        Exception ex = new Exception(exc.Message + " URL:" + HttpContext.Current.Request.Url.ToString());
        if (ex.ToString().Contains("does not exist"))
            return;
        Problem.Log(ex);
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
        //State.SaveAction("SESSION START: ID "+ Session.SessionID);
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
        MemberService.Logout();
        //State.SaveAction("SESSION END: ID "+ Session.SessionID);
    }
</script>

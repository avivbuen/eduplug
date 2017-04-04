<%@ Page Language="C#" AutoEventWireup="true" %>

<%-- 
This page is a dummy. it redirects the user to the required area:
    Guest: Login
    User: His control panel Admin/Teacher/Student
--%>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        Intel.Redirect();//Redirecting to the required area
    }
</script>

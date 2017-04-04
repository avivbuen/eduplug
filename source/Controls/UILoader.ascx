<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UILoader.ascx.cs" Inherits="Controls_UILoader" %>
<%-- Script Manager --%>
<asp:ScriptManager ID="MasterScriptManager" runat="server">
    <Scripts>
        <asp:ScriptReference Path="~/Content/js/jquery.js" />
        <asp:ScriptReference Path="~/Content/js/framework.js" />
        <asp:ScriptReference Path="~/Content/js/live.js" />
    </Scripts>
</asp:ScriptManager>
<!-- Initial script -->
<script type="text/javascript">
    var GlobalUser = 0;//To track connected user, might be removed in future
    var host = "<%= Intel.GetFullRootUrl()%>";//This line comes from the server. :)
    $(document).ready(function () {
        $("#ToggleMenu1").sideNav();
        $("#ToggleMenu2").sideNav();
    });
    LoadCurrentUser();
</script>


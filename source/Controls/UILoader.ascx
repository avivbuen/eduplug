<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UILoader.ascx.cs" Inherits="Controls_UILoader" %>
<%-- Script Manager --%>
<asp:ScriptManager ID="MasterScriptManager" runat="server">
    <Scripts>
        <asp:ScriptReference Path="~/Content/js/jquery.js" />
        <asp:ScriptReference Path="~/Content/js/framework.js" />
        <asp:ScriptReference Path="~/Content/js/live.js" />
        <asp:ScriptReference Path="~/Content/js/chartLoader.js" />
        <asp:ScriptReference Path="~/Content/js/jquery.dataTables.min.js" />
    </Scripts>
</asp:ScriptManager>
<!-- Initial script -->
<script type="text/javascript">
    var GlobalUser = 0;//To track connected user, might be removed in future
    var host = "<%= Intel.GetFullRootUrl()%>";//This line comes from the server. :)
    var preventLoginOffline = true;//Prevents login offline, disconnects also connected users - to disable change here and in connect.js to false
    $(document).ready(function () {
        $("#ToggleMenu1").sideNav();
        $("#ToggleMenu2").sideNav();
    });
    LoadCurrentUser();
    if ((!navigator.onLine)&&preventLoginOffline) {
        AvivnetFramework.toast('אין אינטרנט - חלקים עשויים לא לעבוד כראוי ולכן לא תוכל להשתמש במערכת, תנותק בעוד מספר שניות', 6000, 'rounded');
        setTimeout(Logout, 8000);
    }
</script>


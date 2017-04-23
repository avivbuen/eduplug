<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="InterTrack_Admin_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <a href="~/Panel/Admin/Allowed.aspx" runat="server" class="panelBox purple white-text">
        <h4><i class="material-icons">verified_user</i> הרשאת הרשמה</h4>
    </a>
    <a href="~/Panel/Admin/Tgrades.aspx" runat="server" class="panelBox red accent-3 white-text">
        <h4><i class="material-icons">domain</i> כיתות</h4>
    </a>
    <a href="~/Panel/Admin/Members.aspx" runat="server" class="panelBox teal white-text">
        <h4><i class="material-icons">supervisor_account</i> משתמשים</h4>
    </a>
    <a href="~/Panel/Admin/EduSense.aspx" runat="server" class="panelBox amber darken-2 white-text">
        <h4><i class="material-icons">settings_system_daydream</i> מעקב</h4>
    </a>
</asp:Content>


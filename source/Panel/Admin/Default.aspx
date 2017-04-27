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
     <ul class="collection with-header" style="direction: rtl; float: right; border: 0px; margin: 0px 0px 5px 5px; margin-top: 0px; width: 100%; max-width: 500px">
        <li class="collection-header blue-grey white-text" style="border-radius: 0px;">
            <h4>ממוצע כיתות:</h4>
        </li>
        <asp:ListView ID="ListViewScores" runat="server">
            <ItemTemplate>
                <li class="collection-item">
                    <div>- <%#Eval("Exam.Title") %> <span class="secondary-content" style="float: left"><%#CastVal(Eval("ScoreVal")) %></span></div>
                </li>
            </ItemTemplate>
        </asp:ListView>
        <li class="collection-item blue-grey white-text" style="height: 40px; text-align: center;">
            <a href="~/Panel/Admin/Tgrades.aspx" runat="server" class="white-text" style="float: left;">כל הכיתות <i class="material-icons">open_in_new</i></a>
            <div>
                <asp:Label ID="LabelEmpty" runat="server" Text=""></asp:Label>
            </div>
        </li>
    </ul>
</asp:Content>


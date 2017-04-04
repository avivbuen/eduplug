<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="InterTrack_Admin_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <a href="~/Panel/Admin/Allowed.aspx" runat="server">
            <div class="collection with-header" style="direction: rtl; float: right; border: 0px; margin: 0px 0px 5px 5px; width: 100%; max-width: 415px">
                <div class="collection-header purple white-text" style="border-radius: 0px;">
                    <h4 style="line-height: 90%; font-size: 37px; padding-top: 15px; padding-bottom: 20px;"><i class="material-icons" style="font-size:40px;">verified_user</i>&nbsp; &nbsp הרשאת הרשמה</h4>
                </div>
            </div>
        </a>
        <a href="~/Panel/Admin/Tgrades.aspx" runat="server">
            <div class="collection with-header" style="direction: rtl; float: right; border: 0px; margin: 0px 0px 5px 5px; width: 100%; max-width: 415px">
                <div class="collection-header red accent-3 white-text" style="border-radius: 0px;">
                    <h4 style="line-height: 90%; font-size: 37px; padding-top: 15px; padding-bottom: 20px;"><i class="material-icons" style="font-size:40px;">domain</i>&nbsp; &nbsp כיתות</h4>
                </div>
            </div>
        </a>
        <a href="~/Panel/Admin/Members.aspx" runat="server">
            <div class="collection with-header" style="direction: rtl; float: right; border: 0px; margin: 0px 0px 5px 5px; width: 100%; max-width: 415px">
                <div class="collection-header teal white-text" style="border-radius: 0px;">
                    <h4 style="line-height: 90%; font-size: 37px; padding-top: 15px; padding-bottom: 20px;"><i class="material-icons" style="font-size:40px;">supervisor_account</i>&nbsp; &nbsp;משתמשים</h4>
                </div>
            </div>
        </a>
        <a href="~/Panel/Admin/EduSense.aspx" runat="server">
            <div class="collection with-header" style="direction: rtl; float: right; border: 0px; margin: 0px 0px 5px 5px; width: 100%; max-width: 415px">
                <div class="collection-header amber darken-2 white-text" style="border-radius: 0px;">
                    <h4 style="line-height: 90%; font-size: 37px; padding-top: 15px; padding-bottom: 20px;"><i class="material-icons" style="font-size:40px;">settings_system_daydream</i>&nbsp; &nbsp;מעקב</h4>
                </div>
            </div>
        </a>
</asp:Content>


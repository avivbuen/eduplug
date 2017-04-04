<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Read.aspx.cs" Inherits="Messages_Read" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <script>var bringHomeAfterLogout = 0;</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div dir="rtl" style="max-width: 800px; text-align: center; margin: 0 auto; box-shadow: -1px 1px 5px 0px; background: #fff; padding: 30px;">
        <h1>
            <asp:Label ID="LabelTitle" runat="server" Text="Label"></asp:Label>
        </h1>
        <asp:Label ID="LabelSender" runat="server" Text="sender"></asp:Label>
        >
        <asp:Label ID="LabelReciver" runat="server" Text="rec"></asp:Label>
        {
        <asp:Label ID="LabelDate" runat="server" Text="rec"></asp:Label>
        }
        <div class="input-field col s6" dir="rtl" style="text-align:right;">
            <%= _messageHtml %>
        </div>
        <br />
        <button id="GoBackBtn" runat="server" onserverclick="GoBackBtn_ServerClick" class="btn waves-block" >
            <i class="material-icons">keyboard_arrow_right</i>
        </button>
    </div>
</asp:Content>


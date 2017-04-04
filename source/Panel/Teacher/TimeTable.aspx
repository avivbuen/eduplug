<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TimeTable.aspx.cs" Inherits="Panel_Teacher_TimeTable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 style="text-align: center;">מערכת שעות</h2>
    <Avivnet:TimeTableWeek runat="server" ID="TimeTableWeek" />
</asp:Content>
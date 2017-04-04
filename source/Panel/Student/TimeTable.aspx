<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TimeTable.aspx.cs" Inherits="Panel_Student_TimeTable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 style="text-align: center;">מערכת שעות</h2>
    <Avivnet:TimeTableWeek runat="server" id="TimeTableWeek" />
</asp:Content>


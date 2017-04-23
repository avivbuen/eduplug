<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="InterTrack_Student_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../../Content/css/college.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <a href="~/Panel/Student/Scores.aspx" runat="server" class="panelBox purple white-text">
        <h4><i class="material-icons">assignment</i> ציונים</h4>
    </a>
    <a href="~/Panel/Student/Disciplines.aspx" runat="server" class="panelBox red accent-3 white-text">
        <h4><i class="material-icons">warning</i> הערות</h4>
    </a>
    <a href="~/Panel/Student/TimeTable.aspx" runat="server" class="panelBox  cyan darken-1 white-text">
        <h4><i class="material-icons">access_time</i> מערכת שעות</h4>
    </a>
    <a href="~/Panel/Student/Adjustments.aspx" runat="server" class="panelBox light-green darken-2 white-text">
        <h4><i class="material-icons">face</i> התאמות</h4>
    </a>
    <Avivnet:TimeTableDay runat="server" ID="TimeTableDay" TableFor="Student" />
    <ul class="collection with-header" style="direction: rtl; float: right; border: 0px; margin: 0px 0px 5px 5px; margin-top: 0px; width: 100%; max-width: 500px">
        <li class="collection-header blue-grey white-text" style="border-radius: 0px;">
            <h4>ציונים אחרונים:</h4>
        </li>
        <asp:ListView ID="ListViewScores" runat="server">
            <ItemTemplate>
                <li class="collection-item">
                    <div>- <%#Eval("Exam.Title") %> <span class="secondary-content" style="float: left"><%#Eval("ScoreVal") %></span></div>
                </li>
            </ItemTemplate>
        </asp:ListView>
        <li class="collection-item blue-grey white-text" style="height: 40px; text-align: center;">
            <a href="~/Panel/Student/Scores.aspx" runat="server" class="white-text" style="float: left;">כל הציונים <i class="material-icons">open_in_new</i></a>
            <div>
                <asp:Label ID="LabelEmpty" runat="server" Text=""></asp:Label>
            </div>
        </li>
    </ul>
    <ul class="collection with-header" style="direction: rtl; float: right; border: 0px; margin: 0px 0px 5px 5px; margin-top: 0px; width: 100%; max-width: 500px">
        <li class="collection-header blue-grey white-text" style="border-radius: 0px;">
            <h4>אירועי משמעת אחרונים:</h4>
        </li>
        <asp:ListView ID="ListViewDisi" runat="server">
            <ItemTemplate>
                <li class="collection-item">
                    <div>- <%#Eval("dName") %> <span class="secondary-content"><%#Eval("dDate","{0:d}") %></span> <span class="secondary-content" style="float: left"><%#Eval("lName") %></span></div>
                </li>
            </ItemTemplate>
        </asp:ListView>
        <li class="collection-item blue-grey white-text" style="height: 40px; text-align: center;">
            <a href="~/Panel/Student/Disciplines.aspx" runat="server" class="white-text" style="float: left;">כל ההערות <i class="material-icons">open_in_new</i></a>
            <div>
                <asp:Label ID="LabelEmptyDisi" runat="server" Text=""></asp:Label>
            </div>
        </li>
    </ul>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="InterTrack_Student_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../../Content/css/college.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
           <a href="~/Panel/Student/Scores.aspx" runat="server">
            <div class="collection with-header" style="direction: rtl; float: right; border: 0px; margin: 0px 0px 5px 5px; width: 100%; max-width: 415px">
                <div class="collection-header purple white-text" style="border-radius: 0px;">
                    <h4 style="line-height: 90%; font-size: 37px; padding-top: 15px; padding-bottom: 20px;"><i class="material-icons" style="font-size:40px;">assignment</i>&nbsp; &nbsp ציונים</h4>
                </div>
            </div>
        </a>
        <a href="~/Panel/Student/Disciplines.aspx" runat="server">
            <div class="collection with-header" style="direction: rtl; float: right; border: 0px; margin: 0px 0px 5px 5px; width: 100%; max-width: 415px">
                <div class="collection-header red accent-3 white-text" style="border-radius: 0px;">
                    <h4 style="line-height: 90%; font-size: 37px; padding-top: 15px; padding-bottom: 20px;"><i class="material-icons" style="font-size:40px;">warning</i>&nbsp; &nbsp הערות</h4>
                </div>
            </div>
        </a>
        <a href="~/Panel/Student/TimeTable.aspx" runat="server">
            <div class="collection with-header" style="direction: rtl; float: right; border: 0px; margin: 0px 0px 5px 5px; width: 100%; max-width: 415px">
                <div class="collection-header cyan darken-1 white-text" style="border-radius: 0px;">
                    <h4 style="line-height: 90%; font-size: 37px; padding-top: 15px; padding-bottom: 20px;"><i class="material-icons" style="font-size:40px;">access_time</i>&nbsp; &nbsp;מערכת שעות</h4>
                </div>
            </div>
        </a>
        <a href="~/Panel/Student/Adjustments.aspx" runat="server">
            <div class="collection with-header" style="direction: rtl; float: right; border: 0px; margin: 0px 0px 5px 5px; width: 100%; max-width: 415px">
                <div class="collection-header light-green darken-2 white-text" style="border-radius: 0px;">
                    <h4 style="line-height: 90%; font-size: 37px; padding-top: 15px; padding-bottom: 20px;"><i class="material-icons" style="font-size:40px;">face</i>&nbsp; &nbsp;התאמות</h4>
                </div>
            </div>
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
                    <div> - <%#Eval("dName") %> <span class="secondary-content"><%#Eval("dDate","{0:d}") %></span> <span class="secondary-content" style="float: left"><%#Eval("lName") %></span></div>
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


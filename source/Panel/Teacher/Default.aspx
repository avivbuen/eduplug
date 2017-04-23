<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="InterTrack_Teacher_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../../Content/css/college.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <a href="~/Messages/Compose.aspx" runat="server" class="panelBox light-green darken-2 white-text">
        <h4><i class="material-icons">add</i> הודעה חדשה</h4>
    </a>
    <a href="~/Panel/Teacher/Students.aspx" runat="server" class="panelBox purple white-text">
        <h4><i class="material-icons">face</i> תלמידים</h4>
    </a>
    <a href="~/Messages" runat="server" class="panelBox red accent-3 white-text">
        <h4><i class="material-icons">speaker_notes</i> הודעות</h4>
    </a>
    <a href="~/Panel/Teacher/TimeTable.aspx" runat="server" class="panelBox cyan darken-1 white-text">
        <h4><i class="material-icons">access_time</i> מערכת שעות</h4>
    </a>
    <Avivnet:TimeTableDay runat="server" ID="TimeTableWeek" />
    <ul class="collection with-header" style="direction: rtl; float: right; border: 0px; margin-top: 0px; width: 100%; max-width: 500px">
        <li class="collection-header lime white-text" style="border-radius: 0px;">
            <h4>כיתות:</h4>
        </li>
        <asp:ListView ID="ListViewGrades" runat="server">
            <ItemTemplate>
                <li class="collection-item">
                    <div><%#Eval("Name") %><a href="Grade.aspx?gid=<%#Eval("ID") %>" class="secondary-content lime-text" style="float: left"><i class="material-icons">arrow_back</i></a></div>
                </li>
            </ItemTemplate>
        </asp:ListView>
        <li class="collection-item lime white-text"></li>
    </ul>
    <%--<div class="row">
        <div class="col s2">
            <div class="card-panel teal">
                <span class="white-text">
                    <span style="display: block; direction: rtl; text-align: right; font-size: 20px">מערכת שעות להיום():</span>
                    <script runat="server">
                        public int GetDay()
                        {
                            switch (DateTime.Now.DayOfWeek)
                            {
                                case DayOfWeek.Friday:
                                    return 5;
                                case DayOfWeek.Monday:
                                    return 1;
                                case DayOfWeek.Sunday:
                                    return 0;
                                case DayOfWeek.Thursday:
                                    return 4;
                                case DayOfWeek.Tuesday:
                                    return 2;
                                case DayOfWeek.Wednesday:
                                    return 2;
                                default:
                                    return 0;
                            }
                        }
                    </script>
                    <asp:ListView ID="ListViewTimeTable" runat="server">
                        <ItemTemplate>
                            <span style="display: block; direction: rtl; text-align: right; font-size: 15px"><%#Container.DataItemIndex+1 %> - 
                                <%#CastLessons(Eval("["+GetDay()+"]")) %>
                            </span>
                            <br />
                        </ItemTemplate>
                    </asp:ListView>
                    <a href="#" style="vertical-align: middle;" class="white-text"><i class="material-icons">open_in_new</i></a>
                </span>
            </div>
        </div>
    </div>--%>
    <%--    <h2 style="text-align:center;">מערכת שעות</h2>
    <div class="tab">
        <table border="0" class="college" style="font-size: 25px; margin:0 auto; border:solid; border-color:#3b6bab">
            <tr class="days">
                <th><span style="font-size:25px">#</span></th>
                <th>ראשון</th>
                <th>שני</th>
                <th>שלישי</th>
                <th>רביעי</th>
                <th>חמישי</th>
                <th>שישי</th>
            </tr>
            <asp:ListView ID="ListViewTimeTable" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class='time'><%#Container.DataItemIndex+1 %></td>
                        <td <%#CastClick(Eval("[0].TeacherGradeID")) %><%#CastColor(Eval("[0].Color")) %> ><%#Eval("[0].Name") %></td>
                        <td <%#CastClick(Eval("[1].TeacherGradeID")) %><%#CastColor(Eval("[1].Color")) %>><%#Eval("[1].Name") %></td>
                        <td <%#CastClick(Eval("[2].TeacherGradeID")) %><%#CastColor(Eval("[2].Color")) %>><%#Eval("[2].Name") %></td>
                        <td <%#CastClick(Eval("[3].TeacherGradeID")) %><%#CastColor(Eval("[3].Color")) %>><%#Eval("[3].Name") %></td>
                        <td <%#CastClick(Eval("[4].TeacherGradeID")) %><%#CastColor(Eval("[4].Color")) %>><%#Eval("[4].Name") %></td>
                        <td <%#CastClick(Eval("[5].TeacherGradeID")) %><%#CastColor(Eval("[5].Color")) %>><%#Eval("[5].Name") %></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </table>
        <div style="text-align:center">כדי לנהל כיתה מסוימת משמעת/ציונים עליך ללחוץ על אחת משעות המערכת של אותה כיתה</div>
    </div>--%>
    <%-- DO NOT DELETE! --%>
    <%--    <link href="../../Content/css/pShow.css" rel="stylesheet" />
    <h2 style="text-align: center">הכיתות שלי</h2>
    <div id="pShow" style="direction: rtl">
        <asp:DataList ID="DataLisTeacherGrades" runat="server" RepeatLayout="Table" RepeatColumns="3" OnItemCommand="DataLisTeacherGrades_ItemCommand" OnUpdateCommand="DataLisTeacherGrades_UpdateCommand" OnEditCommand="DataLisTeacherGrades_EditCommand" DataKeyField="ID" OnItemDataBound="DataLisTeacherGrades_ItemDataBound">
            <ItemTemplate>
                <div id="ccol" class="ccol_4 noselect">
                    <figure>
                        <div class="image">
                            <img src="../../Content/graphics/img/TeacherGrade.jpg">
                            <h2><%#TeacherGradeService.GetParTeacherGrade(int.Parse(Eval("ID").ToString())) %></h2>
                        </div>
                        <figcaption>
                            <span style="font-size: 20px"><%#Eval("Name") %></span>
                            <br />
                            כמות תלמידים: <%#TeacherGradeService.GetStudentCount(int.Parse(Eval("ID").ToString())) %> 
                        </figcaption>
                        <asp:LinkButton ID="LinkButtonEdit" CssClass="button" Style="width: 90%" runat="server" CommandName="Edit"><span style="font-size: 18px" class="noselect">הצג כיתה</span><i class="material-icons" style="vertical-align: middle;">keyboard_arrow_left</i></asp:LinkButton>
                    </figure>
                </div>
            </ItemTemplate>
            <EditItemTemplate>
                <div id="ccol" class="ccol_4 noselect">
                    <figure style="max-width: 600px">
                        <div class="image">
                            <img src="../../Content/graphics/img/TeacherGrade.jpg">
                            <h2><%#TeacherGradeService.GetParTeacherGrade(int.Parse(Eval("ID").ToString())) %></h2>
                        </div>
                        <figcaption>
                            <table>
                                <tr>
                                    <td>שם כיתה:</td>
                                    <td><span style="font-size: 20px"><%#Eval("Name") %></span></td>
                                </tr>
                                <tr>
                                    <td>כמות תלמידים:</td>
                                    <td><%#TeacherGradeService.GetStudentCount(int.Parse(Eval("ID").ToString())) %></td>
                                    <td>ממוצע כיתתי:</td>
                                    <td><%#TeacherGradeService.GetStudentCount(int.Parse(Eval("ID").ToString())) %></td>
                                </tr>
                                <tr>
                                    <td>תלמידים:</td>
                                    <td>
                                        <asp:ListBox ID="ListBoxStudents" runat="server" Style="display: inline; height: 130px;"></asp:ListBox>
                                    </td>
                                    <td>בחינות:</td>
                                    <td>
                                        <asp:ListBox ID="ListBoxExams" runat="server" Style="display: inline; height: 130px;"></asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                        </figcaption>
                        <div style="margin-top: 200px; float: left;">
                            <asp:LinkButton ID="LinkButtonCloseEdit" CssClass="button" runat="server" CommandName="Update" Style="width: 20%;"><i class="material-icons" style="vertical-align: middle;">close</i><span style="font-size: 18px" class="noselect">סגור</span></asp:LinkButton>
                            <asp:LinkButton ID="LinkButtonAddStudents" CssClass="button" runat="server" CommandName="AddStudents" Style="width: 20%;"><span style="font-size:16px;">+</span><br /><span style="font-size: 18px" class="noselect">תלמיד</span></asp:LinkButton>
                            <asp:LinkButton ID="LinkButtonGrades" CssClass="button" runat="server" CommandName="ManageExams" Style="width: 33%;"><i class="material-icons" style="vertical-align: middle;">assignment</i><span style="font-size: 18px" class="noselect"> בחינה חדשה</span></asp:LinkButton>
                        </div>
                    </figure>
                </div>
            </EditItemTemplate>
        </asp:DataList>
    </div>--%>
</asp:Content>


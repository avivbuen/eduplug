<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SideNav.ascx.cs" Inherits="Controls_SideNav" %>
<!-- Side Navigation -->
<ul id="slide-out" class="side-nav right-aligned right-align">
    <li style="background: #3b6bab; text-align: center;">
        <img class="background" src="~/Content/graphics/img/logo2.png" runat="server" style="width: 100%; margin-top: 10%;" />
        <!-- Dropdown Trigger -->
        <a class='dropdown-button white-text' href='#' data-activates='dropdown1'><%=EduSysDate.GetYear(DateTime.Now) %></a>

        <!-- Dropdown Structure -->
        <ul id='dropdown1' class='dropdown-content yearsSelectMP' style="width:300px; text-align:center;">
        </ul>
        <script>
            var currentTime = new Date();
            var year = currentTime.getFullYear()+1;
            for (var i = 0; i < 4; i++) {
                $(".yearsSelectMP").append("<li><a href='../../../Panel/SetYear.aspx?year="+year+"' style='text-align:center;'>"+(year-1)+" - "+year+"</a></li>");
                year--;
            }   
        </script>
    </li>
    <li><a class="waves-effect" id="HomeButton" href="~/" runat="server"><i class="material-icons">home</i>בית</a></li>
    <li class="logout-panel"><a class="waves-effect" id="EditButton" href="~/User/Edit.aspx" runat="server"><i class="material-icons">perm_identity</i>פרופיל</a></li>
    <li class="logout-panel">
        <ul class="collapsible collapsible-accordion">
            <li>
                <a class="collapsible-header waves-effect" id="MessagesButton">הודעות<i class="material-icons">mail_outline</i></a>
                <div class="collapsible-body">
                    <ul>
                        <li><a href="~/Messages/Compose.aspx" runat="server">הודעה חדשה<i class="material-icons">create</i></a></li>
                        <li><a href="~/Messages/" runat="server">הודעות<i class="material-icons">inbox</i></a></li>
                    </ul>
                </div>
            </li>
        </ul>
    </li>
    <li style="display: none" class="admin-panel"><a class="waves-effect" href="~/Admin/Default.aspx" runat="server" id="AdminPanelButton"><i class="material-icons">trending_up</i>ניהול</a></li>
    <li style="display: none" class="student-panel teacher-panel"><a class="waves-effect" href="~/Calendar.aspx" runat="server"><i class="material-icons">calendar</i>מערכת שעות</a></li>
    <%--Student--%>
    <%if (MemberService.GetCurrent().Auth == MemberClearance.Student)
      { %>
    <li class="student-panel">
        <ul class="collapsible collapsible-accordion">
            <li>
                <a class="collapsible-header waves-effect">נתוני תלמיד<i class="material-icons">face</i></a>
                <div class="collapsible-body">
                    <ul>
                        <li><a href="~/Panel/Student/Disciplines.aspx" runat="server">התנהגות<i class="material-icons">thumbs_up_down</i></a></li>
                        <li><a href="~/Panel/Student/Scores.aspx" runat="server">ציונים<i class="material-icons">grade</i></a></li>
                        <li><a href="~/Messages/Adjustments.aspx" runat="server">התאמות<i class="material-icons">insert_emoticon</i></a></li>
                    </ul>
                </div>
            </li>
        </ul>
    </li>
    <%} %>
    <%--Teacher--%>
    <%if (MemberService.GetCurrent().Auth == MemberClearance.Teacher)
      { %>


    <%} %>
    <%--Admin--%>
    <%if (MemberService.GetCurrent().Auth == MemberClearance.Admin)
      { %>
    <li class="admin-panel">
        <a class="collapsible-header waves-effect" href="~/Panel/Admin/AddTgrade.aspx" runat="server">כיתה<i class="material-icons">add</i></a>
    </li>
    <%} %>
    <li class="logout-panel"><a class="waves-effect" onclick="Logout()" id="LogoutButton"><i class="material-icons">play_for_work</i>צא</a></li>
</ul>

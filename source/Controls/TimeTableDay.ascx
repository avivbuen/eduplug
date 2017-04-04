<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TimeTableDay.ascx.cs" Inherits="Controls_TimeTableDay" %>
<%if (Session["StartDate1"] == null) Session["StartDate1"] = DateTime.Now; %>
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
                return 3;
            default:
                return 0;
        }
    }
</script>
<ul class="collection with-header" style="direction: rtl; float: right; border: 0px; margin: 0px 0px 5px 5px; margin-top: 0px; width: 100%; max-width: 500px">
    <li class="collection-header blue-grey white-text" style="border-radius: 0px;">
        <h4>מערכת שעות(<%=DateTimeExtensions.StartOfWeek(((DateTime)Session["StartDate1"]),(DayOfWeek)GetDay()).ToShortDateString() %>):</h4>
    </li>
    <asp:ListView ID="ListViewTimeTable" runat="server">
        <ItemTemplate>
            <li class="collection-item">
                <%#Container.DataItemIndex+1 %> - <%#CastLessons(Eval("["+GetDay()+"]"),(DayOfWeek)GetDay()) %>
            </li>
        </ItemTemplate>
    </asp:ListView>
    <li>
        <asp:Label ID="LabelEmpty" runat="server" Text=""></asp:Label>
    </li>
    <%if (tableFor == MemberClearance.Teacher)
        { %>
    <li class="collection-item blue-grey white-text" style="height: 40px;text-align:center;"><a href="~/Panel/Teacher/TimeTable.aspx" runat="server" class="white-text">מערכת מלאה <i class="material-icons">open_in_new</i></a></li>
    <%}
    else if (tableFor == MemberClearance.Student)
    { %>
    <li class="collection-item blue-grey white-text" style="height: 40px;text-align:center;"><a href="~/Panel/Student/TimeTable.aspx" runat="server" class="white-text" style="float:left">מערכת מלאה <i class="material-icons">open_in_new</i></a></li>
    <%} %>
</ul>

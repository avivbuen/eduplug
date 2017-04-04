<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TimeTableWeek.ascx.cs" Inherits="Controls_TimeTable" %>
<link href="../../Content/framework/Site.css" rel="stylesheet" />
<link href="../../Content/css/college.css" rel="stylesheet" runat="server" />
<asp:UpdatePanel ID="UpdatePanelTimeTable" runat="server">
    <ContentTemplate>
        <div class="tab">

            <style>
                .whiteLinkButton {
                    color: #fff;
                }
            </style>
            <table border="0" class="college" style="margin: 0 auto; border: solid; border-color: #3b6bab;">
                <tr class="days">
                    <th><span style="font-size: 25px">#</span></th>
                    <%if (Session["StartDate"] == null) Session["StartDate"] = DateTime.Now; %>
                    <th style="font-size: 20px">ראשון <%=DateTimeExtensions.StartOfWeek(((DateTime)Session["StartDate"]),DayOfWeek.Sunday).ToShortDateString() %></th>
                    <th style="font-size: 20px">שני <%=DateTimeExtensions.StartOfWeek(((DateTime)Session["StartDate"]),DayOfWeek.Monday).ToShortDateString() %></th>
                    <th style="font-size: 20px">שלישי <%=DateTimeExtensions.StartOfWeek(((DateTime)Session["StartDate"]),DayOfWeek.Tuesday).ToShortDateString() %></th>
                    <th style="font-size: 20px">רביעי <%=DateTimeExtensions.StartOfWeek(((DateTime)Session["StartDate"]),DayOfWeek.Wednesday).ToShortDateString() %></th>
                    <th style="font-size: 20px">חמישי <%=DateTimeExtensions.StartOfWeek(((DateTime)Session["StartDate"]),DayOfWeek.Thursday).ToShortDateString() %></th>
                    <th style="font-size: 20px">שישי <%=DateTimeExtensions.StartOfWeek(((DateTime)Session["StartDate"]),DayOfWeek.Friday).ToShortDateString() %></th>
                </tr>
                <asp:ListView ID="ListViewTimeTable" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td class='time' style="font-size: 15px"><%#Container.DataItemIndex+1 %></td>
                            <%#CastLessons(Eval("[0]"),DayOfWeek.Sunday)%>
                            <%#CastLessons(Eval("[1]"),DayOfWeek.Monday)%>
                            <%#CastLessons(Eval("[2]"),DayOfWeek.Tuesday)%>
                            <%#CastLessons(Eval("[3]"),DayOfWeek.Wednesday)%>
                            <%#CastLessons(Eval("[4]"),DayOfWeek.Thursday)%>
                            <%#CastLessons(Eval("[5]"),DayOfWeek.Friday)%>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <tfoot>
                    <tr class="days">
                        <td>
                            <asp:LinkButton ID="LinkButtonBack" CssClass="whiteLinkButton" runat="server" OnClick="LinkButtonBack_Click"><i class="material-icons">arrow_forward</i></asp:LinkButton>
                        </td>
                        <td></td>
                        <td colspan="4" style="text-align: center">
                            <asp:LinkButton ID="LinkButtonToday" CssClass="whiteLinkButton" runat="server" OnClick="LinkButtonToday_Click"><i class="material-icons">today</i></asp:LinkButton>
                        </td>
                        <td style="text-align: left">
                            <asp:LinkButton ID="LinkButtonForward" CssClass="whiteLinkButton" runat="server" OnClick="LinkButtonForward_Click"><i class="material-icons">arrow_back</i></asp:LinkButton>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

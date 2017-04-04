<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Scores.aspx.cs" Inherits="Track_Scores" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 style="text-align: center;">הציונים שלי</h2>
    <ul class="collapsible" data-collapsible="accordion" dir="rtl">
        <asp:ListView ID="DataListScores" runat="server">
            <ItemTemplate>
                <li>
                    <div class="collapsible-header">
                        <table>
                            <tr>
                                <td ><i class="material-icons"><%#GetScoreIcon(Eval("StudentScore").ToString())%></i></td>
                                <td ><%#Eval("ExamTitle")%>  </td>
                                <td >תאריך:<%#Eval("ExamDate","{0:dd/MM/yyyy}") %>  </td>
                                <td style="float: left">ציון:<%#Eval("StudentScore") %> </td>
                            </tr>
                        </table>
                    </div>
                    <div class="collapsible-body">
                        <p>קיבלת במבחן ב<%#Eval("GradeName") %> ציון <%#Eval("StudentScore") %></p>
                    </div>
                </li>
            </ItemTemplate>
        </asp:ListView>
    </ul>
    <h4 style="text-align: center;">
        <asp:Label ID="LabelError" runat="server" Text="Label"></asp:Label>
    </h4>
    <script> $('.collapsible').collapsible();</script>
</asp:Content>


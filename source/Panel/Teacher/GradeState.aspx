<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GradeState.aspx.cs" Inherits="Panel_Teacher_GradeState" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="direction: rtl;">
        <table>
            <thead>
                <tr>
                    <th style="width: 100px; margin-left: 100px; background-color: aliceblue">#תלמיד#</th>
                    <asp:ListView ID="ListViewExamsA" runat="server">
                        <ItemTemplate>
                            <th>*<%#Eval("Title") %></th>
                        </ItemTemplate>
                    </asp:ListView>
                    <th>- ציון מחצית א -</th>
                    <asp:ListView ID="ListViewExamsB" runat="server">
                        <ItemTemplate>
                            <th>*<%#Eval("Title") %></th>
                        </ItemTemplate>
                    </asp:ListView>
                    <th>- ציון מחצית ב -</th>
                    <th>- ממוצע -</th>
                    <th>- ממוצע סופי -</th>
                </tr>
            </thead>
            <tbody>
                <asp:ListView ID="ListViewStudents" runat="server" DataKeyNames="UserID" OnItemDataBound="ListViewStudents_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td><%#Eval("Name") %></td>
                            <asp:ListView ID="ListViewScoresA" runat="server" DataKeyNames="ID">
                                <ItemTemplate>
                                    <td>
                                        <%#CastScore(Eval("ScoreVal")) %>
                                    </td>
                                </ItemTemplate>
                            </asp:ListView>
                            <td><%#GetFAVG(Eval("UserID"),"a") %></td>
                            <asp:ListView ID="ListViewScoresB" runat="server" DataKeyNames="ID">
                                <ItemTemplate>
                                    <td>
                                        <%#CastScore(Eval("ScoreVal")) %>
                                    </td>
                                </ItemTemplate>
                            </asp:ListView>
                            <td><%#GetFAVG(Eval("UserID"),"b") %></td>
                            <td><%#GetAVG(Eval("UserID")) %></td>
                            <td><%#GetFAVG(Eval("UserID")) %></td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </tbody>
        </table>

    </div>
</asp:Content>


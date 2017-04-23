<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EduSense.aspx.cs" Inherits="Panel_Admin_EduSense" %>
<%@ Import Namespace="Business_Logic.Members" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        html {
            overflow-x: hidden;
        }

        table, th, td {
            border: solid 1px !important;
        }
    </style>
    <div dir="rtl" style="max-width: 800px; text-align: center; margin: 0 auto; box-shadow: -1px 1px 5px 0px; background: #fff; padding: 30px;">
        <Avivnet:Chart ID="EnterChart" runat="server" Title="כניסות" />
        <br />
        <asp:GridView ID="GridViewSense" runat="server" DataKeyNames="Rev" AutoGenerateColumns="False" OnRowDataBound="GridViewSense_RowDataBound" OnPageIndexChanging="GridViewSense_PageIndexChanging" OnRowCommand="GridViewSense_RowCommand" CssClass="highlight" style="margin:0 auto" AllowPaging="True">
            <Columns>
                <asp:TemplateField HeaderText="שם משתמש">
                    <ItemTemplate>
                        <%#MemberService.GetUserPart(int.Parse(Eval("UserID").ToString())).Name %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="VisitorOS" HeaderText="מערכת הפעלה"></asp:BoundField>
                <asp:BoundField DataField="VisitorIP" HeaderText="כתובת IP"></asp:BoundField>
                <asp:BoundField DataField="VisitorISP" HeaderText="ספק אינטרנט"></asp:BoundField>
                <asp:BoundField DataField="VisitorCountry" HeaderText="מדינה"></asp:BoundField>
                <asp:BoundField DataField="VistServerTime" HeaderText="שעה" DataFormatString="{0:d}"></asp:BoundField>
                <asp:TemplateField HeaderText="בקר">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButtonDel" runat="server" CommandName="EditT"
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"><i class="material-icons">done</i></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="מחיקה">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnDelete" runat="server" Style="color: black" CommandName="DeleteT" OnClientClick="return confirm('בטוח שברצונך למחוק')"
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"><i class="material-icons">delete</i></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerTemplate>
                <div style="text-align: center;">
                    <asp:LinkButton ID="First" CommandName="Page" CommandArgument="First" runat="server" Text="<<" />
                    <asp:LinkButton ID="Prev" CommandName="Page" CommandArgument="Prev" runat="server" Text="<" />
                    &nbsp;
                    <% Response.Write(GridViewSense.PageIndex + 1);%> מתוך <% Response.Write(GridViewSense.PageCount); %>
                    &nbsp;
                    <asp:LinkButton ID="Next" CommandName="Page" CommandArgument="Next" runat="server" Text=">" />
                    <asp:LinkButton ID="Last" CommandName="Page" CommandArgument="Last" runat="server" Text=">>" />
                </div>
            </PagerTemplate>
        </asp:GridView>
        <asp:Label ID="LabelEmpty" runat="server" Text=""></asp:Label>
    </div>
</asp:Content>


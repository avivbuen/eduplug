<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Allowed.aspx.cs" Inherits="Panel_Admin_Allowed" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
            <a href="~/Panel/Admin/AddAllow.aspx" runat="server">
            <div class="collection with-header" style="direction: rtl; float: right; border: 0px; margin: 0px 0px 5px 5px; width: 100%; max-width: 500px">
                <div class="collection-header green white-text" style="border-radius: 0px;">
                    <h4 style="line-height: 90%; font-size: 37px; padding-top: 15px; padding-bottom: 20px;"><i class="material-icons" style="font-size:40px;">add</i>&nbsp; &nbsp;הוספת הרשאת הרשמה</h4>
                </div>
            </div>
        </a>
    <div dir="rtl" style="max-width: 400px; text-align: center; margin: 0 auto; box-shadow: -1px 1px 5px 0px; background: #fff; padding: 30px;">
        <asp:GridView ID="ListViewUsers" PageSize="10" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="ListViewUsers_PageIndexChanging" OnRowCommand="ListViewUsers_RowCommand" CssClass="highlight" AllowPaging="True">
            <Columns>
                <asp:TemplateField HeaderText="שם">
                    <ItemTemplate>
                        <span><%#Eval("nhsFirstName")%> <%#Eval("nhsLastName")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="nhsID" HeaderText="ת.ז"></asp:BoundField>
                <asp:TemplateField HeaderText="נרשם">
                    <ItemTemplate>
                        <%#GetYesNo((bool)Eval("nhsActive")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="עריכה">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButtonDel" runat="server" CommandName="EditT"
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"><i class="material-icons">edit</i></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="מחיקה">
                    <ItemTemplate>

                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteT" OnClientClick="return confirm('בטוח שברצונך למחוק')"
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"><i class="material-icons">delete</i></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerTemplate>
                <div style="text-align: center;">
                    <asp:LinkButton ID="First" CommandName="Page" CommandArgument="First" runat="server" Text="<<" />
                    <asp:LinkButton ID="Prev" CommandName="Page" CommandArgument="Prev" runat="server" Text="<" />
                    &nbsp;
                    <% Response.Write(ListViewUsers.PageIndex + 1);%> מתוך <% Response.Write(ListViewUsers.PageCount); %>
                    &nbsp;
                    <asp:LinkButton ID="Next" CommandName="Page" CommandArgument="Next" runat="server" Text=">" />
                    <asp:LinkButton ID="Last" CommandName="Page" CommandArgument="Last" runat="server" Text=">>" />
                </div>
            </PagerTemplate>
        </asp:GridView>
        <asp:Label ID="LabelEmpty" runat="server" Text=""></asp:Label>
    </div>
</asp:Content>


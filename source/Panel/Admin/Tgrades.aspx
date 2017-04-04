<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Tgrades.aspx.cs" Inherits="Panel_Admin_Tgrades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <a href="~/Panel/Admin/AddTgrade.aspx" runat="server">
        <div class="collection with-header" style="direction: rtl; float: right; border: 0px; margin: 0px 0px 5px 5px; width: 100%; max-width: 500px">
            <div class="collection-header green white-text" style="border-radius: 0px;">
                <h4 style="line-height: 90%; font-size: 37px; padding-top: 15px; padding-bottom: 20px;"><i class="material-icons" style="font-size: 40px;">add</i>&nbsp; &nbsp;הוספת כיתה</h4>
            </div>
        </div>
    </a>
    <div dir="rtl" style="max-width: 400px; text-align: center; margin: 0 auto; box-shadow: -1px 1px 5px 0px; background: #fff; padding: 30px;">
        <asp:GridView ID="GridViewTgrades" PageSize="10" DataKeyNames="ID" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="GridViewTgrades_PageIndexChanging" OnRowCommand="GridViewTgrades_RowCommand" CssClass="highlight" AllowPaging="True">
            <Columns>
                <asp:TemplateField HeaderText="שם מקצוע">
                    <ItemTemplate>
                        <%#Eval("Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="מורה">
                    <ItemTemplate>
                        <%#MemberService.GetUser((int)Eval("TeacherID")).Name %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="שכבה">
                    <ItemTemplate>
                        <%#tGradeService.GetPartGrade((int)Eval("ID")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="עריכה">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButtonDel" runat="server" CommandName="EditT"
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Style="color: black"><i class="material-icons">edit</i></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="שיעורים">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButtonLes" runat="server" CommandName="LessonEdit"
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Style="color: black"><i class="material-icons">access_time</i></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="מחיקה">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteT" OnClientClick="return confirm('בטוח שברצונך למחוק')"
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Style="color: black"><i class="material-icons">delete</i></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerTemplate>
                <div style="text-align: center;">
                    <asp:LinkButton ID="First" CommandName="Page" CommandArgument="First" runat="server" Text="<<" />
                    <asp:LinkButton ID="Prev" CommandName="Page" CommandArgument="Prev" runat="server" Text="<" />
                    &nbsp;
                    <% Response.Write(GridViewTgrades.PageIndex + 1);%> מתוך <% Response.Write(GridViewTgrades.PageCount); %>
                    &nbsp;
                    <asp:LinkButton ID="Next" CommandName="Page" CommandArgument="Next" runat="server" Text=">" />
                    <asp:LinkButton ID="Last" CommandName="Page" CommandArgument="Last" runat="server" Text=">>" />
                </div>
            </PagerTemplate>
        </asp:GridView>
        <asp:Label ID="LabelEmpty" runat="server" Text=""></asp:Label>
    </div>
</asp:Content>


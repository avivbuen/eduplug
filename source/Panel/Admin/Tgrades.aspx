<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Tgrades.aspx.cs" Inherits="Panel_Admin_Tgrades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="../../Content/js/jquery.dataTables.min.js"></script>
    <h2 style="text-align:center; direction:rtl">כיתות:</h2>
    <a href="~/Panel/Admin/AddTgrade.aspx" runat="server">
        <div class="collection with-header waves-effect" style="direction: rtl; float: right; border: 0px; margin: 0px 0px 0px 0px; padding: 0 0 0 0; width: 100%; max-width: 200px; max-height: 100px;">
            <div class="collection-header green white-text" style="border-radius: 0px; margin: 0px 0px 0px 0px; padding: 0 0 0 0;">
                <h4 style="line-height: 90%; margin: 0px 0px 0px 0px; padding: 5px 0px 5px 0px; font-size: 37px;"><i class="material-icons" style="font-size: 40px;">add</i>&nbsp; &nbsp;חדש</h4>
            </div>
        </div>
    </a>
    <br />
    <link href="../../Content/css/table.css" rel="stylesheet" />
    <div class="table-responsive-vertical shadow-z-1" style="direction: rtl; text-align: center;">
        <br />
        <br />
        <asp:GridView ID="GridViewTgrades" DataKeyNames="ID" OnDataBound="GridViewTgrades_DataBinding" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="GridViewTgrades_PageIndexChanging" OnRowCommand="GridViewTgrades_RowCommand" CssClass="datatables-table table table-hover">
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
        </asp:GridView>
    </div>
    <asp:Label ID="LabelEmpty" runat="server" Text=""></asp:Label>
    
    <script>
        $('.datatables-table').DataTable({
            // Enable mark.js search term highlighting
            mark: true
        });
    </script>
</asp:Content>


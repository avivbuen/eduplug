<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Allowed.aspx.cs" Inherits="Panel_Admin_Allowed" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="../../Content/js/jquery.dataTables.min.js"></script>
    <a href="~/Panel/Admin/AddAllow.aspx" runat="server">
        <div class="collection with-header waves-effect" style="direction: rtl; float: right; border: 0px; margin: 0px 0px 0px 0px; padding: 0 0 0 0; width: 100%; max-width: 200px; max-height: 100px;">
            <div class="collection-header green white-text" style="border-radius: 0px; margin: 0px 0px 0px 0px; padding: 0 0 0 0;">
                <h4 style="line-height: 90%; margin: 0px 0px 0px 0px; padding: 5px 0px 5px 0px; font-size: 37px;"><i class="material-icons" style="font-size: 40px;">add</i>&nbsp; &nbsp;חדש</h4>
            </div>
        </div>
    </a>
    <a href="~/Panel/Admin/ImportAllowed.aspx" runat="server">
        <div class="collection with-header waves-effect" style="direction: rtl; float: right; border: 0px; margin: 0px 20px 0px 0px; padding: 0px 0px 0 0px; width: 100%; max-width: 200px; max-height: 100px;">
            <div class="collection-header orange white-text" style="border-radius: 0px; margin: 0px 0px 0px 0px; padding: 0 0 0 0;">
                <h4 style="line-height: 90%; margin: 0px 0px 0px 0px; padding: 5px 10px 5px 0px; font-size: 37px;"><i class="material-icons" style="font-size: 40px;">cloud_upload</i>&nbsp; &nbsp;יבא</h4>
            </div>
        </div>
    </a>
    <link href="../../Content/css/table.css" rel="stylesheet" />
    <div class="table-responsive-vertical shadow-z-1" style="direction: rtl; text-align: center;">
        <br />
        <br />
        <h4>רשימת מוזמנים להרשמה</h4>
        <asp:GridView ID="GridViewUsers" runat="server" OnDataBound="GridViewUsers_DataBinding" AutoGenerateColumns="False" OnPageIndexChanging="GridViewUsers_PageIndexChanging" OnRowCommand="GridViewUsers_RowCommand" CssClass="datatables-table table table-hover">
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

                <asp:TemplateField HeaderText="פעולות">
                    <ItemTemplate>

                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteT" Style="color: black" OnClientClick="return confirm('בטוח שברצונך למחוק')"
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"><i class="material-icons">delete</i></asp:LinkButton>
                        <asp:LinkButton ID="LinkButtonDel" runat="server" CommandName="EditT" Style="color: black"
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"><i class="material-icons">edit</i></asp:LinkButton>
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


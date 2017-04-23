<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Disciplines.aspx.cs" Inherits="Panel_Student_Disciplines" %>
<%@ Import Namespace="Business_Logic.Members" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <link href="../../Content/css/table.css" rel="stylesheet" />
    <script src="../../Content/js/jquery.dataTables.min.js"></script>
     <h2 style="text-align:center; direction:rtl">הערות משמעת</h2>

    <div class="table-responsive-vertical shadow-z-1" style="direction: rtl; text-align: center;">
            <asp:GridView ID="GridViewDiscplines" OnDataBound="GridViewDiscplines_DataBound" CssClass="datatables-table table table-hover" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="lName" HeaderText="שיעור"></asp:BoundField>
                    <asp:BoundField DataField="dName" HeaderText="הערה"></asp:BoundField>
                    <asp:BoundField DataField="dHour" HeaderText="שעה"></asp:BoundField>
                    <asp:TemplateField HeaderText="מורה">
                        <ItemTemplate>
                            <%#MemberService.GetUserPart((int)Eval("teacherId")).Name %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="dDate" DataFormatString="{0: dd/MM/yyyy}" HeaderText="תאריך"></asp:BoundField>
                </Columns>
            </asp:GridView>
            <div style="text-align: center;">
                <asp:Literal ID="LiteralEmptyDiscplines" runat="server"></asp:Literal>
            </div>
        </div>
        <script>
        $('.datatables-table').DataTable({
            // Enable mark.js search term highlighting
            mark: true
        });
    </script>
</asp:Content>


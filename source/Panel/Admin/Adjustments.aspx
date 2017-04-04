<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Adjustments.aspx.cs" Inherits="Panel_Admin_Adjustments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        table, th, td {
            border: solid 1px !important;
        }
    </style>

    <div id="GridViewDiscplines_tbl" style="border: groove; direction: rtl; float: right;text-align:center; margin-top: 0px; width: 100%;">
        <span style="font-weight: 600; text-align: center; font-size:xx-large; text-align:center;">כרטיס התאמות לתלמיד</span>
        <a id="link" href="~/Panel/Admin/AddAdjustment.aspx?uid=" runat="server" style="float:left;font-size:large; color:black">הוסף <i style="color:black;float:left; font-size:xx-large" class="material-icons">add</i></a>
        <asp:GridView ID="GridViewAdjustment" CssClass="bordered" runat="server" AutoGenerateColumns="False" Style="width: 100%">
            <Columns>
                <asp:TemplateField HeaderText="#">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Name" HeaderText="התאמה"></asp:BoundField>
            </Columns>
        </asp:GridView>
        <div style="text-align: center;">
            <asp:Literal ID="LiteralEmptyAdjust" runat="server"></asp:Literal>
        </div>
    </div>
    <script><%=script%></script>
</asp:Content>


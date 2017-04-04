<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Disciplines.aspx.cs" Inherits="Panel_Student_Disciplines" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
        <style>
            table, th, td {
                border: solid 1px !important;
            }
        </style>
        <div id="GridViewDiscplines_tbl" style="border: groove; direction: rtl; float: right; margin-top: 0px; width: 100%; background:#fff">
            <h5 style="font-weight: 600; text-align: center;">הערות משמעת</h5>
            <asp:GridView ID="GridViewDiscplines" CssClass="bordered" runat="server" AutoGenerateColumns="False" Style="width: 100%">
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
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Lesson.aspx.cs" Inherits="Panel_Teacher_Lesson" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="direction: rtl;">
        <asp:Label ID="LabelTitle" runat="server" Text="" Font-Size="XX-Large"></asp:Label><br />
        <asp:Label ID="LabelDate" runat="server" Text="" Font-Size="Larger"></asp:Label><br />
        <asp:Label ID="LabelHour" runat="server" Text="" Font-Size="Larger"></asp:Label>
        <style>
            .opc > [type="checkbox"] {
                opacity: 1;
                left: auto;
                padding-left: 50%;
                text-align: center;
                position: relative;
            }

            .opc {
                text-align: center;
                margin: 0 auto;
            }
        </style>
        <asp:Panel ID="PanelData" runat="server">
            <div style="overflow-y: scroll;">
                <table>
                    <thead>
                        <tr>
                            <th style="width: 100px; margin-left: 100px; background-color: aliceblue">#תלמיד#</th>
                            <asp:ListView ID="ListViewTypes" runat="server">
                                <ItemTemplate>
                                    <th>*<%#Eval("Name") %></th>
                                </ItemTemplate>
                            </asp:ListView>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:ListView ID="ListViewStudents" runat="server" DataKeyNames="UserID" OnItemDataBound="ListViewStudents_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td style="position: fixed; width: 100px; margin-left: 100px; background-color: aliceblue; z-index: 99; white-space: nowrap !important;"><%#Eval("Name") %></td>
                                    <asp:ListView ID="ListViewTypes" runat="server" DataKeyNames="ID">
                                        <ItemTemplate>
                                            <td>
                                                <asp:CheckBox ID="CheckBoxItem" runat="server" CssClass="opc" />
                                            </td>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </tbody>
                </table>
            </div>
        </asp:Panel>
        <script>
            $(".opc input:checkbox:checked").parent().parent().css("background", "red");
            $(".opc input:checkbox:checked").parent().parent().css("border", "black solid 1px !important;");
            $(".opc input:checkbox").click(function () {
                $(".opc input:checkbox").parent().parent().css("background", "");
                $(".opc input:checkbox:checked").parent().parent().css("background","red");
                $(".opc input:checkbox:checked").parent().parent().css("border", "black solid 1px !important;");
            });

        </script>
    </div>
    <div style="text-align: center; padding-top: 20px;">
        <asp:Label ID="LabelResponse" runat="server" Text=""></asp:Label>
        <asp:LinkButton ID="ButtonApply" CssClass="btn-flat grey" runat="server" OnClick="ButtonApply_Click">
            <i class="material-icons">save</i>
        </asp:LinkButton>
    </div>
    <div id="success-modal" class="modal" style="max-width: 250px;">
        <div class="modal-content center-block center-align center centered" dir="rtl">
            <div class="checkmark modal-close modal-action">
                <svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"
                    viewBox="0 0 161.2 161.2" enable-background="new 0 0 161.2 161.2" xml:space="preserve">
                    <path class="path" fill="none" stroke="#3b6bab" stroke-miterlimit="10" d="M425.9,52.1L425.9,52.1c-2.2-2.6-6-2.6-8.3-0.1l-42.7,46.2l-14.3-16.4
	c-2.3-2.7-6.2-2.7-8.6-0.1c-1.9,2.1-2,5.6-0.1,7.7l17.6,20.3c0.2,0.3,0.4,0.6,0.6,0.9c1.8,2,4.4,2.5,6.6,1.4c0.7-0.3,1.4-0.8,2-1.5
	c0.3-0.3,0.5-0.6,0.7-0.9l46.3-50.1C427.7,57.5,427.7,54.2,425.9,52.1z" />
                    <circle class="path" fill="none" stroke="#3b6bab" stroke-width="4" stroke-miterlimit="10" cx="80.6" cy="80.6" r="62.1" />
                    <polyline class="path" fill="none" stroke="#3b6bab" stroke-width="6" stroke-linecap="round" stroke-miterlimit="10" points="113,52.8
	74.1,108.4 48.2,86.4 " />

                    <circle class="spin" fill="none" stroke="#3b6bab" stroke-width="4" stroke-miterlimit="10" stroke-dasharray="12.2175,12.2175" cx="80.6" cy="80.6" r="73.9" />

                </svg>
            </div>
        </div>
    </div>
    <script><%= done %></script>
    <%=script %>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddAdjustment.aspx.cs" Inherits="Panel_Admin_AddAdjustment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanelAllow" runat="server">
        <ContentTemplate>
            <div dir="rtl" style="max-width: 500px; text-align: center; margin: 0 auto; box-shadow: -1px 1px 5px 0px; background: #fff; padding: 30px;">
                <asp:ValidationSummary ID="AllowValidationSummary" ForeColor="Red" Style="list-style-type: upper-roman;" ValidationGroup="AllowValidationGroup" runat="server" />
                <h1><i class="material-icons" style="font-size: 72px;">perm_identity</i></h1>
                <p>
                    דף זה נועד להוסיף התאמה לתלמיד                         <asp:Label ID="User_ID_Name" runat="server"></asp:Label>, יש לציין כי את/ה מחויב להוסיף אך ורק התאמות אשר אושרו על ידי משרד החינוך:
                </p>
                <div class="row" style="text-align: right;">
                    <div class="input-field col" style="direction: rtl; width: 100%">
                        <asp:DropDownList ID="User_Adjustment" runat="server" CssClass="browser-default">

                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfv_User_Adjustment" runat="server" ControlToValidate="User_Adjustment" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="AllowValidationGroup" InitialValue="-1" ErrorMessage="בחר רמת הרשאות">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                <asp:Button ID="AddButton" CssClass="btn waves-button-input btn-large" runat="server" Text="+" Font-Size="36" OnClick="AddButton_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


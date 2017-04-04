<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EditAllowed.aspx.cs" Inherits="Panel_Admin_EditAllowed" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

            <div dir="rtl" style="max-width: 500px; text-align: center; margin: 0 auto; box-shadow: -1px 1px 5px 0px; background: #fff; padding: 30px;">
                <asp:ValidationSummary ID="AllowValidationSummary" ForeColor="Red" Style="list-style-type: upper-roman;" ValidationGroup="AllowValidationGroup" runat="server" />
                <h1><i class="material-icons" style="font-size: 72px;">perm_identity</i></h1>
                <p>
                    דף זה נועד לאשר רישום של תלמידים חדשים אשר אינם נמצאים במערכת. הוספת תלמיד חדש אל המערכת על מנת שיוכל להרשם:
                </p>
                <div class="row" style="text-align: right;">
                    <div class="input-field col s6" dir="rtl">
                        <asp:TextBox ID="User_First_Name" runat="server"></asp:TextBox>
                        <label>
                            <asp:RequiredFieldValidator ID="rfv_User_First_Name" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="AllowValidationGroup" ControlToValidate="User_First_Name" runat="server" ErrorMessage="הכנס שם פרטי">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="refv_User_First_Name" Display="Dynamic" ValidationExpression="^([א-ת]{2,10})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="AllowValidationGroup" ControlToValidate="User_First_Name" runat="server" ErrorMessage="שם פרטי חייב להיות בעברית בלבד בין 2-10 תווים">*</asp:RegularExpressionValidator>
                            שם פרטי</label>
                    </div>
                    <div class="input-field col s6">
                        <asp:TextBox ID="User_Last_Name" runat="server"></asp:TextBox>
                        <label>
                            <asp:RequiredFieldValidator ID="rfv_User_Last_Name" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="AllowValidationGroup" ControlToValidate="User_Last_Name" runat="server" ErrorMessage="הכנס שם משפחה">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="refv_User_Last_Name" Display="Dynamic" ValidationExpression="^([א-ת]{2,10})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="AllowValidationGroup" ControlToValidate="User_Last_Name" runat="server" ErrorMessage="שם משפחה חייב להיות בעברית בלבד בין 2-10 תווים">*</asp:RegularExpressionValidator>
                            שם משפחה</label>
                    </div>
                    <div class="input-field col" style="width: 100%;">
                        <asp:TextBox ID="User_ID" runat="server"></asp:TextBox>
                        <label>
                            <asp:RequiredFieldValidator ID="rfv_User_ID" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="AllowValidationGroup" ControlToValidate="User_ID" runat="server" ErrorMessage="הכנס שם תעודת זהות">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="refv_User_ID" Display="Dynamic" ValidationExpression="^[0-9]{9}$" EnableClientScript="true" ForeColor="Red" ValidationGroup="AllowValidationGroup" ControlToValidate="User_ID" runat="server" ErrorMessage="תעודת זהות לא תקינה">*</asp:RegularExpressionValidator>
                             <asp:CustomValidator ID="cv_User_ID" runat="server" ErrorMessage="תעודת זהות לא תקינה על פי חוק" OnServerValidate="cv_User_ID_ServerValidate" ForeColor="Red" Display="Dynamic" ValidationGroup="AllowValidationGroup">*</asp:CustomValidator>
                            <asp:CustomValidator ID="cve_User_ID" runat="server" ErrorMessage="תעודת זהות קיימת כבר" OnServerValidate="cve_User_ID_ServerValidate" ForeColor="Red" Display="Dynamic" ValidationGroup="AllowValidationGroup">*</asp:CustomValidator>
                            תעודת זהות</label>
                    </div>
                    <div class="input-field col" style="direction: rtl; width: 100%">
                        <asp:DropDownList ID="User_Section" runat="server" CssClass="browser-default">
                            <asp:ListItem Text="בחר רמת הרשאות" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="מנהל" Value="a"></asp:ListItem>
                            <asp:ListItem Text="מורה" Value="t"></asp:ListItem>
                            <asp:ListItem Text="תלמיד" Value="s"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfv_User_Section" runat="server" ControlToValidate="User_Section" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="AllowValidationGroup" InitialValue="-1" ErrorMessage="בחר רמת הרשאות">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                <asp:Button ID="SaveButton" CssClass="btn waves-button-input btn-large" runat="server" Text="שמור" Font-Size="36" OnClick="SaveButton_Click" />
            </div>
</asp:Content>


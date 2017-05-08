<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddAllow.aspx.cs" Inherits="Admin_Tools_AddAllow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div dir="rtl" style="max-width: 500px; text-align: center; margin: 0 auto; box-shadow: -1px 1px 5px 0px; background: #fff; padding: 30px;">
        <asp:ValidationSummary ID="AllowValidationSummary" ForeColor="Red" Style="list-style-type: upper-roman;" ValidationGroup="AllowValidationGroup" runat="server" />
        <h1><i class="material-icons" style="font-size: 72px;">perm_identity</i></h1>
        <p>
            דף זה נועד לאשר רישום של תלמידים חדשים אשר אינם נמצאים במערכת. הוספת תלמיד חדש אל המערכת על מנת שיוכל להרשם:
        </p>
        <div class="row" style="text-align: right;">

            <div class="input-field col s6">
                <asp:TextBox ID="User_Last_Name" runat="server"></asp:TextBox>
                <label>
                    <asp:RequiredFieldValidator ID="rfv_User_Last_Name" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="AllowValidationGroup" ControlToValidate="User_Last_Name" runat="server" ErrorMessage="הכנס שם משפחה">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="refv_User_Last_Name" Display="Dynamic" ValidationExpression="^([א-ת]{2,10})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="AllowValidationGroup" ControlToValidate="User_Last_Name" runat="server" ErrorMessage="שם משפחה חייב להיות בעברית בלבד בין 2-10 תווים">*</asp:RegularExpressionValidator>
                    שם משפחה</label>
            </div>
            <div class="input-field col s6" dir="rtl">
                <asp:TextBox ID="User_First_Name" runat="server"></asp:TextBox>
                <label>
                    <asp:RequiredFieldValidator ID="rfv_User_First_Name" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="AllowValidationGroup" ControlToValidate="User_First_Name" runat="server" ErrorMessage="הכנס שם פרטי">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="refv_User_First_Name" Display="Dynamic" ValidationExpression="^([א-ת]{2,10})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="AllowValidationGroup" ControlToValidate="User_First_Name" runat="server" ErrorMessage="שם פרטי חייב להיות בעברית בלבד בין 2-10 תווים">*</asp:RegularExpressionValidator>

                    שם פרטי</label>
            </div>
            <div class="input-field col" style="width: 100%;">
                <asp:TextBox ID="User_ID" runat="server" MaxLength="9"></asp:TextBox>
                <label>
                    <asp:RequiredFieldValidator ID="rfv_User_ID" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="AllowValidationGroup" ControlToValidate="User_ID" runat="server" ErrorMessage="הכנס שם תעודת זהות">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="refv_User_ID" Display="Dynamic" ValidationExpression="^[0-9]{9}$" EnableClientScript="true" ForeColor="Red" ValidationGroup="AllowValidationGroup" ControlToValidate="User_ID" runat="server" ErrorMessage="תעודת זהות לא תקינה">*</asp:RegularExpressionValidator>
                    <asp:CustomValidator ID="cv_User_ID" runat="server" ErrorMessage="תעודת זהות לא תקינה על פי חוק" OnServerValidate="cv_User_ID_ServerValidate" ForeColor="Red" Display="Dynamic" ValidationGroup="AllowValidationGroup">*</asp:CustomValidator>
                    <asp:CustomValidator ID="cve_User_ID" runat="server" ErrorMessage="תעודת זהות קיימת כבר" OnServerValidate="cve_User_ID_ServerValidate" ForeColor="Red" Display="Dynamic" ValidationGroup="AllowValidationGroup">*</asp:CustomValidator>
                    תעודת זהות</label>
            </div>
            <div class="input-field col" style="direction: rtl;">
                <asp:DropDownList ID="User_City" runat="server" CssClass="browser-default e-form">
                    <asp:ListItem Text="בחר עיר" Value="-1"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfv_User_City" runat="server" ControlToValidate="User_City" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="UpdateValidationGroup" InitialValue="-1" ErrorMessage="בחר עיר">*</asp:RequiredFieldValidator>
            </div>
            <div class="input-field col" style="width: 50%; float: right">
                <asp:DropDownList ID="User_Section" runat="server" CssClass="browser-default">
                    <asp:ListItem Text="בחר אזור/כיתה" Value="-1"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="User_Section" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="AllowValidationGroup" InitialValue="-1" ErrorMessage="בחר כיתה/אזור">*</asp:RequiredFieldValidator>
            </div>

            <div class="input-field col" style="direction: rtl; width: 100%; ">
                <asp:DropDownList ID="User_Type" runat="server" CssClass="browser-default">
                    <asp:ListItem Text="בחר רמת הרשאות" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="מנהל" Value="a"></asp:ListItem>
                    <asp:ListItem Text="מורה" Value="t"></asp:ListItem>
                    <asp:ListItem Text="תלמיד" Value="s"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfv_User_Section" runat="server" ControlToValidate="User_Type" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="AllowValidationGroup" InitialValue="-1" ErrorMessage="בחר רמת הרשאות">*</asp:RequiredFieldValidator>
            </div>
            <div class="input-field col" style="float: right">
                <span>
                    <asp:RequiredFieldValidator ID="rfv_User_Gender" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="UpdateValidationGroup" ControlToValidate="User_Gender" runat="server" ErrorMessage="הכנס מגדר">*</asp:RequiredFieldValidator>
                    מגדר</span>
                <asp:RadioButtonList ID="User_Gender" runat="server" RepeatDirection="Vertical" RepeatColumns="2" CssClass="e-form">
                    <asp:ListItem Text="זכר" Value="m"></asp:ListItem>
                    <asp:ListItem Text="נקבה" Value="f"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <p dir="rtl" class="e-form">
                <asp:CustomValidator ID="cv_User_Majors" runat="server" ErrorMessage="אנא בחר מגמה אחת לפחות" OnServerValidate="cv_User_Majors_OnServerValidate" ForeColor="Red" ValidationGroup="UpdateValidationGroup">*</asp:CustomValidator>
                <span style="text-align: center;">מגמות</span>
                <asp:CheckBoxList ID="User_Majors" runat="server" RepeatDirection="Vertical" RepeatColumns="3" CssClass="e-form"></asp:CheckBoxList>
            </p>
            <asp:RequiredFieldValidator ID="rfv_User_BornDate" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="UpdateValidationGroup" ControlToValidate="User_BornDate" runat="server" ErrorMessage="הכנס תאריך לידה">*</asp:RequiredFieldValidator>
            <div class="col">
                <asp:TextBox ID="User_BornDate" runat="server" CssClass="datepicker e-form"></asp:TextBox>
                <label>
                    <asp:CompareValidator ID="cmv_User_BornDate" runat="server" ControlToValidate="User_BornDate" Display="Dynamic" ForeColor="Red" ValidationGroup="UpdateValidationGroup" ErrorMessage="גיל לא לא יכול להיות קטן מ - 11 או גדול מהתאריך הנוכחי" Type="Date" Operator="LessThanEqual">*</asp:CompareValidator>
                    <asp:CompareValidator ID="cmv_User_BornDate_g" runat="server" ControlToValidate="User_BornDate" Display="Dynamic" ForeColor="Red" ValidationGroup="UpdateValidationGroup" ErrorMessage="גיל לא לא יכול להיות גדול מ - 120" Type="Date" Operator="GreaterThanEqual">*</asp:CompareValidator>
                    תאריך לידה
                </label>
            </div>
        </div>
        <asp:Button ID="AddButton" CssClass="btn waves-button-input btn-large" runat="server" Text="+" Font-Size="36" OnClick="AddButton_Click" />
    </div>
    <script><%=script%></script>
</asp:Content>


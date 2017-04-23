<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EditTgrade.aspx.cs" Inherits="Panel_Admin_EditTeacherGrade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div dir="rtl" style="max-width: 500px; text-align: center; margin: 0 auto; box-shadow: -1px 1px 5px 0px; background: #fff; padding: 30px;">
        <asp:ValidationSummary ID="AllowValidationSummary" ForeColor="Red" Style="list-style-type: upper-roman;" ValidationGroup="AllowValidationGroup" runat="server" />
        <h1><i class="material-icons" style="font-size: 72px;">domain</i></h1>
        <p>
            דף זה נועד לעריכת כיתה למורים חדשים או קיימים:
        </p>
        <div class="row" style="text-align: right;">
            <div class="input-field col" style="direction: rtl; width: 100%">
                מורה הכיתה
                <asp:DropDownList ID="ListTeachers" runat="server" CssClass="browser-default">
                    <asp:ListItem Text="בחר מורה" Value="-1"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfv_ListTeachers" runat="server" ControlToValidate="ListTeachers" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="AllowValidationGroup" InitialValue="-1" ErrorMessage="בחר מורה">*</asp:RequiredFieldValidator>
            </div>
            <div class="input-field col" style="width: 100%;">
                <asp:TextBox ID="TeacherGradeName" runat="server"></asp:TextBox>
                <label>
                    <asp:RequiredFieldValidator ID="rfv_TeacherGradeName" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="AllowValidationGroup" ControlToValidate="TeacherGradeName" runat="server" ErrorMessage="הכנס שם כיתה">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="refv_TeacherGradeName" Display="Dynamic" ValidationExpression="^([\sא-ת]{2,30})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="AllowValidationGroup" ControlToValidate="TeacherGradeName" runat="server" ErrorMessage="שם כיתה חייב להיות בעברית בלבד בין 2-30 תווים">*</asp:RegularExpressionValidator>
                    שם כיתה</label>
            </div>
            <asp:UpdatePanel ID="UpdatePanelData" runat="server">
                <ContentTemplate>
                    <div class="input-field col" style="direction: rtl; width: 100%">
                        שכבת המקצוע
                        <asp:DropDownList ID="LisTeacherGrades" runat="server" CssClass="browser-default" OnSelectedIndexChanged="LisTeacherGrades_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="בחר שכבה" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="ז'" Value="ז'"></asp:ListItem>
                            <asp:ListItem Text="ח'" Value="ח'"></asp:ListItem>
                            <asp:ListItem Text="ט'" Value="ט'"></asp:ListItem>
                            <asp:ListItem Text="י'" Value="י'"></asp:ListItem>
                            <asp:ListItem Text="יא'" Value="יא'"></asp:ListItem>
                            <asp:ListItem Text="יב'" Value="יב'"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfv_LisTeacherGrades" runat="server" ControlToValidate="LisTeacherGrades" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="AllowValidationGroup" InitialValue="-1" ErrorMessage="בחר שכבה">*</asp:RequiredFieldValidator>
                    </div>
                    <asp:Panel ID="PanelStudents" runat="server" Visible="false">
                        <asp:CustomValidator ID="cv_StudentsToAdd" runat="server" ErrorMessage="אנא בחר תלמיד אחד לפחות" OnServerValidate="cv_StudentsToAdd_ServerValidate" ForeColor="Red" ValidationGroup="AllowValidationGroup">*</asp:CustomValidator>
                        <span style="text-align: center; width: 100%">תלמידי הכיתה</span>

                        <div style="overflow-y: scroll; height: 100px; width: 100%; padding-right: 15px">
                            <asp:CheckBoxList ID="StudentsToAdd" RepeatColumns="2" runat="server" CssClass="e-form" Visible="false"></asp:CheckBoxList>
                            <asp:Label ID="LabelNoStudents" runat="server" Text=""></asp:Label>
                        </div>
                    </asp:Panel>
                    <div class="input-field col" style="direction: rtl; width: 100%">
                        מגמת המקצוע
                        <asp:DropDownList ID="ListMajors" runat="server" CssClass="browser-default" OnSelectedIndexChanged="ListMajors_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    <asp:Panel ID="PanelNewMajor" runat="server" Visible="false">
                        <div class="input-field col" style="width: 100%;">
                            <asp:TextBox ID="MajorName" runat="server"></asp:TextBox>
                            <label>
                                <asp:RequiredFieldValidator ID="rfv_MajorName" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="AllowValidationGroup" ControlToValidate="MajorName" runat="server" ErrorMessage="הכנס שם מגמה">*</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="refv_MajorName" Display="Dynamic" ValidationExpression="^([\sא-ת]{2,30})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="AllowValidationGroup" ControlToValidate="MajorName" runat="server" ErrorMessage="שם מגמה חייב להיות בעברית בלבד בין 2-30 תווים">*</asp:RegularExpressionValidator>
                                שם מגמה חדשה</label>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="margin: 0 auto; text-align:center;">
                <asp:Button ID="AddButton" CssClass="btn waves-button-input btn-large" runat="server" Text="שמור" Font-Size="36" OnClick="AddButton_Click" />
            </div>
        </div>
</asp:Content>


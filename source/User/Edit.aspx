<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Edit.aspx.cs" Inherits="User_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="col " dir="rtl" style="max-width: 800px; text-align: center; margin: 0 auto; box-shadow: -1px 1px 5px 0px; background: #fff; padding: 30px;">
        <img id="ProfilePicture" src="#" style="border-radius: 50%; width: 200px; height: 200px; background: #1ec8ff" />
        <script>
            $(document).ready(function () {
                document.getElementById("ProfilePicture").src = host + "<%=picPath%>";
                <%if (!IsPostBack)
            { %>
                $(".e-form").attr("readonly", "readonly");
                $(".e-form .input-field").attr("disabled", "disabled");
                $(".e-form").removeClass("datepicker");
                $(".e-form input:radio").attr("disabled", "disabled");
                $(".e-form input:file").attr("disabled", "disabled");
                $(".e-form input:checkbox").attr("disabled", "disabled");
                $("#<%= User_City.ClientID %>").attr("disabled", "disabled");
                $("#<%= User_Section.ClientID %>").attr("disabled", "disabled");
                <%}
            else
            {%>
                EnableControls();
                <%}%>
            });
            var formActive = false;
            function EnableControls() {
                var active = formActive;
                formActive = !formActive;
                $(".e-form").removeAttr("readonly");
                $(".e-form .input-field").removeAttr("disabled");
                $("#<%= User_BornDate.ClientID %>").addClass("datepicker");
                $(".e-form input:radio").removeAttr("disabled");
                $(".e-form input:file").removeAttr("disabled");
                $(".e-form input:checkbox").removeAttr("disabled");
                $("#<%= User_City.ClientID %>").removeAttr("disabled");
                $("#<%= User_Section.ClientID %>").removeAttr("disabled");
                $('.datepicker').pickadate({
                    selectMonths: true, // Creates a dropdown to control month
                    selectYears: 150 // Creates a dropdown of 150 years to control year
                });
                $("#<%= UpdateButton.ClientID %>").val("שמור");
                return active;
            }
            var bringHomeAfterLogout = 0;
            function CheckFileUploaderSelectedFileType(source, args) {

                var fileupload = document.getElementById("<%=User_Picture.ClientID%>").value;
                var lastSlesh = fileupload.lastIndexOf(".");
                var newName = "";

                for (var i = lastSlesh; i < fileupload.length; i++) {
                    newName += fileupload.charAt(i);
                }
                if (newName != "") {
                    if (newName != ".png" && newName != ".jpg" && newName != ".bmp" && newName != ".jpeg") {
                        args.IsValid = false;
                    }
                }
                else {
                    args.IsValid = true;
                }
            }
        </script>


        <asp:ValidationSummary ID="UpadateValidationSummary" ForeColor="Red" Style="list-style-type: upper-roman;" ValidationGroup="UpdateValidationGroup" runat="server" />
        <div class="row" style="text-align: right;">
        </div>
        <div class="input-field col s6" dir="rtl">
            <asp:TextBox ID="User_First_Name" runat="server" CssClass="e-form"></asp:TextBox>
            <label>
                <asp:RequiredFieldValidator ID="rfv_User_First_Name" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="UpdateValidationGroup" ControlToValidate="User_First_Name" runat="server" ErrorMessage="הכנס שם פרטי">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="refv_User_First_Name" Display="Dynamic" ValidationExpression="^([א-ת]{2,10})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="UpdateValidationGroup" ControlToValidate="User_First_Name" runat="server" ErrorMessage="שם פרטי חייב להיות בעברית בלבד בין 2-10 תווים">*</asp:RegularExpressionValidator>
                שם פרטי</label>
        </div>
        <div class="input-field col s6">
            <asp:TextBox ID="User_Last_Name" runat="server" CssClass="e-form"></asp:TextBox>
            <label>
                <asp:RequiredFieldValidator ID="rfv_User_Last_Name" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="UpdateValidationGroup" ControlToValidate="User_Last_Name" runat="server" ErrorMessage="הכנס שם משפחה">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="refv_User_Last_Name" Display="Dynamic" ValidationExpression="^([א-ת]{2,10})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="UpdateValidationGroup" ControlToValidate="User_Last_Name" runat="server" ErrorMessage="שם משפחה חייב להיות בעברית בלבד בין 2-10 תווים">*</asp:RegularExpressionValidator>
                שם משפחה</label>
        </div>
        <div class="input-field col s6" dir="rtl">
            <asp:TextBox ID="User_Email" runat="server" CssClass="e-form"></asp:TextBox>
            <label>
                <asp:RequiredFieldValidator ID="rfv_User_Email" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="UpdateValidationGroup" ControlToValidate="User_Email" runat="server" ErrorMessage="הכנס כתובת אימייל">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="refv_User_Email" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" EnableClientScript="true" ForeColor="Red" ValidationGroup="UpdateValidationGroup" ControlToValidate="User_Email" runat="server" ErrorMessage="אמייל לא תקין">*</asp:RegularExpressionValidator>
                <asp:CustomValidator ID="cfv_User_Email" runat="server" Display="Dynamic" OnServerValidate="cfv_User_Email_ServerValidate" ErrorMessage="אימייל כבר קיים במערכת" ValidationGroup="UpdateValidationGroup" ControlToValidate="User_Email" ForeColor="Red">*</asp:CustomValidator>
                אימייל</label>
        </div>

        <div class="input-field col" dir="rtl">
            <asp:TextBox ID="User_ID" runat="server" ReadOnly="true" MaxLength="9"></asp:TextBox>
            <label>
                <asp:RequiredFieldValidator ID="rfv_User_ID" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="UpdateValidationGroup" ControlToValidate="User_ID" runat="server" ErrorMessage="הכנס תעודת זהות">*</asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cfv_User_ID" runat="server" Display="Dynamic" OnServerValidate="cfv_User_ID_ServerValidate" ErrorMessage="אינך קיים במערכת/תעודת הזהות או השם אינם תואמים את נתונינו פנה למנהל טכנולוגי" ValidationGroup="UpdateValidationGroup" ControlToValidate="User_ID" ForeColor="Red">*</asp:CustomValidator>
                <asp:RegularExpressionValidator ID="refv_User_ID" Display="Dynamic" ValidationExpression="^[0-9]{9}$" EnableClientScript="true" ForeColor="Red" ValidationGroup="UpdateValidationGroup" ControlToValidate="User_ID" runat="server" ErrorMessage="תעודת זהות לא תקינה">*</asp:RegularExpressionValidator>
                תעודת זהות</label>
        </div>
        <div class="input-field col" dir="rtl">
            <asp:TextBox ID="User_Password" runat="server" TextMode="Password" CssClass="e-form"></asp:TextBox>
            <label>
                <asp:RequiredFieldValidator ID="rfv_User_Password" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="UpdateValidationGroup" ControlToValidate="User_Password" runat="server" ErrorMessage="הכנס סיסמה">*</asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cmv_User_Password" runat="server" ErrorMessage="סיסמה בין 32-8 באנגלית וספרות בלבד" OnServerValidate="cmv_User_Password_ServerValidate" ForeColor="Red" ValidationGroup="UpdateValidationGroup" Display="Dynamic" ControlToValidate="User_Password">*</asp:CustomValidator>
                סיסמה</label>
        </div>
        <div class="input-field col" dir="rtl">
            <asp:TextBox ID="User_Password_c" runat="server" TextMode="Password" CssClass="e-form"></asp:TextBox>
            <label>
                <asp:RequiredFieldValidator ID="rfv_User_Password_c" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="UpdateValidationGroup" ControlToValidate="User_Password_c" runat="server" ErrorMessage="הכנס אישור סיסמה">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="ctv_User_Password_c" runat="server" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="UpdateValidationGroup" ControlToValidate="User_Password_c" ControlToCompare="User_Password" ErrorMessage="הסיסמאות אינם תואמות">*</asp:CompareValidator>
                אשר סיסמה</label>
        </div>
        <div class="input-field col" style="direction: rtl;">
            <asp:DropDownList ID="User_City" runat="server" CssClass="browser-default e-form">
                <asp:ListItem Text="בחר עיר" Value="-1"></asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfv_User_City" runat="server" ControlToValidate="User_City" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="UpdateValidationGroup" InitialValue="-1" ErrorMessage="בחר עיר">*</asp:RequiredFieldValidator>
        </div>
        <div class="input-field col" style="direction: rtl;">
            <asp:DropDownList ID="User_Section" runat="server" CssClass="browser-default e-form">
                <asp:ListItem Text="בחר אזור/כיתה" Value="-1"></asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfv_User_Section" runat="server" ControlToValidate="User_Section" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="UpdateValidationGroup" InitialValue="-1" ErrorMessage="בחר כיתה/אזור">*</asp:RequiredFieldValidator>
        </div>
        <div class="file-field input-field col e-form">
            <div class="btn">
                <span>תמונה</span>
                <asp:CustomValidator ID="cv_User_Picture" runat="server" ErrorMessage="קובץ זה אינו תמונה" OnServerValidate="cv_User_Picture_ServerValidate1" ForeColor="Red" ValidationGroup="UpdateValidationGroup">*</asp:CustomValidator>
                <asp:FileUpload ID="User_Picture" runat="server" accept=".png,.jpg" CssClass="e-form" />
            </div>
            <div class="file-path-wrapper">
                <input class="file-path validate e-form" type="text" />
            </div>
        </div>
        <div class="input-field col">
            <span>
                <asp:RequiredFieldValidator ID="rfv_User_Gender" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="UpdateValidationGroup" ControlToValidate="User_Gender" runat="server" ErrorMessage="הכנס מגדר">*</asp:RequiredFieldValidator>
                מגדר</span>
            <asp:RadioButtonList ID="User_Gender" runat="server" RepeatDirection="Vertical" RepeatColumns="2" CssClass="e-form">
                <asp:ListItem Text="זכר" Value="m"></asp:ListItem>
                <asp:ListItem Text="נקבה" Value="f"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <p dir="rtl" class="e-form">
            <asp:CustomValidator ID="cv_User_Majors" runat="server" ErrorMessage="אנא בחר מגמה אחת לפחות" OnServerValidate="cv_User_Majors_ServerValidate" ForeColor="Red" ValidationGroup="UpdateValidationGroup">*</asp:CustomValidator>
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
        <asp:Button ID="UpdateButton" CssClass="btn waves-button-input" runat="server" Text="עדכנ/י" OnClick="UpdateButton_Click" OnClientClick="return EnableControls()" />
        <asp:Button ID="DeleteButton" CssClass="btn waves-button-input red" runat="server" Text="כבה חשבון" OnClick="DeleteButton_Click" OnClientClick="return confirm('אתה בטוח?')" />
    </div>
    <div id="success-modal" class="modal" style="max-width: 500px;">
        <div class="modal-content center-block center-align center centered" dir="rtl">
            השינויים עברו בהצלחה!
            <br />
            <br />
            <br />
            <a href="#!" style="margin: 0 auto" class="modal-action modal-close waves-effect waves-green btn green center-align center">אישור</a>
        </div>
    </div>
    <script>
        $("#<%=User_Password.ClientID%>").focusin(function () {
            if ($("#<%=User_Password.ClientID%>").val() == "!@#$%^&*(8)(&%@#&$*(") {
                $("#<%=User_Password.ClientID%>").val("");
            }
        });
        $("#<%=User_Password.ClientID%>").focusout(function () {
            if ($("#<%=User_Password.ClientID%>").val().trim() == "") {
                $("#<%=User_Password.ClientID%>").val("!@#$%^&*(8)(&%@#&$*(");
            }
        });
        $("#<%=User_Password_c.ClientID%>").focusin(function () {
            if ($("#<%=User_Password_c.ClientID%>").val() == "!@#$%^&*(8)(&%@#&$*(") {
                $("#<%=User_Password_c.ClientID%>").val("");
            }
        });
        $("#<%=User_Password_c.ClientID%>").focusout(function () {
            if ($("#<%=User_Password_c.ClientID%>").val().trim() == "") {
                $("#<%=User_Password_c.ClientID%>").val("!@#$%^&*(8)(&%@#&$*(");
            }
        });
    </script>
    <script><%= done %></script>
</asp:Content>


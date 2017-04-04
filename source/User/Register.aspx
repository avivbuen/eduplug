<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="User_Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <Avivnet:Intersense runat="server" ID="Intersense" />
    <title>EduPlug</title>
    <link href="../Content/css/connect.css" rel="stylesheet" />
    <script src="../Content/js/jquery.js"></script>
    <script src="../Content/js/framework.js"></script>
    <link href="../Content/css/loader.css" rel="stylesheet" />
    <link href="../Content/css/picker.css" rel="stylesheet" />
    <link href="../Content/graphics/img/favicon.png" rel="icon" runat="server" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        label {
            white-space: nowrap !important;
        }
    </style>
</head>
<body onload="myFunction()">

    <div id="loader"></div>
    <div id="page">
        <form id="form1" runat="server">
            <div style="text-align: center; direction: rtl;" class="login-page">
                <div class="form">
                    <img src="../Content/graphics/img/logo.png" style="width: 250px" />
                    <br />
                    <br />
                    <div class="frm1">
                        <asp:RequiredFieldValidator ID="rfv_User_First_Name" EnableClientScript="true" SetFocusOnError="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_First_Name" runat="server" ErrorMessage="הכנס שם פרטי">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="refv_User_First_Name" Display="Dynamic" SetFocusOnError="true" ValidationExpression="^([א-ת]{2,10})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_First_Name" runat="server" ErrorMessage="שם פרטי חייב להיות בעברית בלבד בין 2-10 תווים">*</asp:RegularExpressionValidator>
                        <asp:TextBox ID="User_First_Name" runat="server" CssClass="tbbox pt1" placeholder="שם פרטי"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv_User_Last_Name" EnableClientScript="true" SetFocusOnError="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Last_Name" runat="server" ErrorMessage="הכנס שם משפחה">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="refv_User_Last_Name" Display="Dynamic" SetFocusOnError="true" ValidationExpression="^([א-ת]{2,10})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Last_Name" runat="server" ErrorMessage="שם משפחה חייב להיות בעברית בלבד בין 2-10 תווים">*</asp:RegularExpressionValidator>
                        <asp:TextBox ID="User_Last_Name" runat="server" CssClass="tbbox pt1" placeholder="שם משפחה"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv_User_ID" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_ID" runat="server" ErrorMessage="הכנס תעודת זהות">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="refv_User_ID" Display="Dynamic" ValidationExpression="^[0-9]{9}$" SetFocusOnError="true" EnableClientScript="true" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_ID" runat="server" ErrorMessage="תעודת זהות לא תקינה">*</asp:RegularExpressionValidator>
                        <asp:TextBox ID="User_ID" runat="server" CssClass="tbbox pt1" placeholder="תעודת זהות" MaxLength="9"></asp:TextBox>
                        <button class="sbmBtn" style="color: white; width: 100%; margin-top: 10px" onclick="NavigateForm(2);return false;">הבא</button>
                    </div>
                    <div class="frm2 register-form">
                        <asp:RequiredFieldValidator ID="rfv_User_Email" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Email" runat="server" ErrorMessage="הכנס כתובת אימייל">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="refv_User_Email" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" EnableClientScript="true" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Email" runat="server" ErrorMessage="אמייל לא תקין">*</asp:RegularExpressionValidator>
                        <asp:TextBox ID="User_Email" runat="server" CssClass="tbbox" placeholder="אימייל"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv_User_Password" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Password" runat="server" ErrorMessage="הכנס סיסמה">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="refv_User_Password" Display="Dynamic" ValidationExpression="^([A-Za-z0-9]{8,32})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Password" runat="server" ErrorMessage="סיסמה חייבת להיות בין 8-32 תווים באנגלית ובספרות">*</asp:RegularExpressionValidator>
                        <asp:TextBox ID="User_Password" runat="server" CssClass="tbbox" TextMode="Password" placeholder="סיסמה"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv_User_Password_c" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Password_c" runat="server" ErrorMessage="הכנס אישור סיסמה">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="ctv_User_Password_c" runat="server" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Password_c" ControlToCompare="User_Password" ErrorMessage="הסיסמאות אינם תואמות">*</asp:CompareValidator>
                        <asp:TextBox ID="User_Password_c" runat="server" TextMode="Password" CssClass="tbbox" placeholder="אשר סיסמה"></asp:TextBox>
                        <a class="sbmBtn" style="color: white; width: 50%; margin-top: 10px" onclick="NavigateForm(1)">הקודם</a>
                        <button class="sbmBtn" style="color: white; width: 74%; margin-top: 10px" onclick="NavigateForm(3);return false;">הבא</button>
                    </div>
                    <div class="frm3 register-form">
                        <asp:RequiredFieldValidator ID="rfv_User_City" runat="server" Style="width: 100%" ControlToValidate="User_City" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" InitialValue="-1" ErrorMessage="בחר עיר">*</asp:RequiredFieldValidator>
                        <asp:DropDownList ID="User_City" runat="server" CssClass="tbbox">
                            <asp:ListItem Text="בחר עיר" Value="-1"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfv_User_Section" runat="server" Style="width: 100%" ControlToValidate="User_Section" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" InitialValue="-1" ErrorMessage="בחר כיתה/אזור">*</asp:RequiredFieldValidator>
                        <asp:DropDownList ID="User_Section" runat="server" CssClass="tbbox">
                            <asp:ListItem Text="בחר אזור/כיתה" Value="-1"></asp:ListItem>
                        </asp:DropDownList>
                        <div class="tbbox">
                            <span id="PicMessage">
                                <h4>תמונת פרופיל</h4>
                            </span>
                            <table style="margin:0 auto">
                                <tbody>
                                    <tr>
                                        <td>
                                            <img id="PreviewImage" alt="your image" style="display: none; width: 70px; height: 70px; border: 1px solid black; border-radius: 50%;" />
                                        </td>
                                        <td>
                                            <img src="../Content/graphics/img/remove-icon.png" title="בטל תמונה" id="delPic" onclick="CheckMe(this);" style="display: none; width: 15px; height: 15px" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <asp:FileUpload ID="User_Picture" runat="server" accept=".png,.jpg" onchange="readURL(this);" />

                            <script>
                                function readURL(input) {
                                    if (input.files && input.files[0]) {

                                        var reader = new FileReader();

                                        reader.onload = function (e) {
                                            document.getElementById('PreviewImage').src = e.target.result;
                                            document.getElementById('PreviewImage').style.display = "inline";
                                            document.getElementById('PicMessage').style.display = "none";
                                            document.getElementById('delPic').style.display = "block";
                                        }

                                        reader.readAsDataURL(input.files[0]);
                                    }
                                }
                                function CheckMe(input) {
                                    var myPic = document.getElementById('<%=User_Picture.ClientID%>');
                                    if (myPic.files && myPic.files[0]) {
                                        document.getElementById('PreviewImage').src = "";
                                        document.getElementById('PreviewImage').style.display = "none";
                                        document.getElementById('PicMessage').style.display = "block";
                                        myPic.value = "";
                                        document.getElementById('delPic').style.display = "none";
                                    }

                                }

                            </script>
                        </div>

                        <a class="sbmBtn" style="color: white; width: 50%; margin-top: 25px" onclick="NavigateForm(2)">הקודם</a>
                        <button class="sbmBtn" style="color: white; width: 74%; margin-top: 10px" onclick="NavigateForm(4);return false;">הבא</button>
                    </div>
                    <div class="frm4 register-form" style="direction: rtl;">
                        <asp:RequiredFieldValidator ID="rfv_User_Gender" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Gender" runat="server" ErrorMessage="הכנס מגדר">*</asp:RequiredFieldValidator>
                        <asp:RadioButtonList ID="User_Gender" runat="server" RepeatDirection="Vertical" RepeatColumns="2" CssClass="tbbox">
                            <asp:ListItem Text="זכר" Value="m"></asp:ListItem>
                            <asp:ListItem Text="נקבה" Value="f"></asp:ListItem>
                        </asp:RadioButtonList>
                        <div class="tbbox">
                            <h4>מגמות</h4>
                            <asp:CheckBoxList ID="User_Majors" runat="server" TextAlign="Right" RepeatDirection="Vertical" RepeatColumns="3"></asp:CheckBoxList>
                        </div>
                        <asp:RequiredFieldValidator ID="rfv_User_BornDate" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_BornDate" runat="server" ErrorMessage="הכנס תאריך לידה">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cmv_User_BornDate" runat="server" ControlToValidate="User_BornDate" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ErrorMessage="גיל לא לא יכול להיות קטן מ - 11 או גדול מהתאריך הנוכחי" Type="Date" Operator="LessThanEqual">*</asp:CompareValidator>
                        <asp:CompareValidator ID="cmv_User_BornDate_g" runat="server" ControlToValidate="User_BornDate" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ErrorMessage="גיל לא לא יכול להיות גדול מ - 120" Type="Date" Operator="GreaterThanEqual">*</asp:CompareValidator>
                        <asp:TextBox ID="User_BornDate" runat="server" CssClass="tbbox datepicker" placeholder="תאריך לידה"></asp:TextBox>
                        <%--                        <asp:RequiredFieldValidator ID="rfv_User_Phone" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Phone" runat="server" ErrorMessage="הכנס טלפון">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="refv_User_Phone" Display="Dynamic" ValidationExpression="^(([0]([50|52|53|54|55|56|57|58|59])))[0-9]\d{7}$" EnableClientScript="true" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Phone" runat="server" ErrorMessage="מספר טלפון לא תקין">*</asp:RegularExpressionValidator>
                        <asp:TextBox ID="User_Phone" runat="server" CssClass="tbbox" placeholder="מס. טלפון" MaxLength="10"></asp:TextBox>--%>
                        <asp:CustomValidator ID="cv_User_Picture" runat="server" ErrorMessage="קובץ זה אינו תמונה" OnServerValidate="cv_User_Picture_ServerValidate1" ForeColor="Red" ValidationGroup="RegisterValidationGroup">*</asp:CustomValidator>
                        <asp:CustomValidator ID="cv_User_Majors" runat="server" Display="Dynamic" ErrorMessage="אנא בחר מגמה אחת לפחות" OnServerValidate="cv_User_Majors_ServerValidate" ValidationGroup="RegisterValidationGroup" ForeColor="Red">*</asp:CustomValidator>
                        <asp:CustomValidator ID="cfv_User_Email" runat="server" Display="Dynamic" OnServerValidate="cfv_User_Email_ServerValidate" ErrorMessage="אימייל כבר קיים במערכת" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Email" ForeColor="Red">*</asp:CustomValidator>
                        <asp:CustomValidator ID="cfv_User_ID" runat="server" Display="Dynamic" OnServerValidate="cfv_User_ID_ServerValidate" ErrorMessage="אינך קיים במערכת/תעודת הזהות או השם אינם תואמים את נתונינו פנה למנהל טכנולוגי / נרשמת בעבר" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_ID" ForeColor="Red">*</asp:CustomValidator>
                        <a class="sbmBtn" style="color: white; width: 50%; margin-top: 10px" onclick="NavigateForm(3)">הקודם</a>
                        <asp:Button ID="RegisterButton" CssClass="sbmBtn" runat="server" Text="הרשמה" OnClick="RegisterButton_Click" Style="margin-top: 10px; width: 60%;" OnClientClick="return ValidateForm();" />
                    </div>
                    <asp:ValidationSummary ID="RegisterValidationSummary" CssClass="reg-validate-sum" DisplayMode="BulletList" ForeColor="Red" ValidationGroup="RegisterValidationGroup" runat="server" />
                    <br />
                    <p class="message">רשום? <a href="~/User" runat="server">הכנס</a></p>
                </div>
            </div>
        </form>
    </div>
    <script src="../Content/js/create.js"></script>
    <script>
        $('.datepicker').pickadate({
            selectMonths: true, // Creates a dropdown to control month
            selectYears: 150 // Creates a dropdown of 150 years to control year
        });
        var myVar;

        function myFunction() {
            myVar = setTimeout(showPage, 500);
        }

        function showPage() {
            document.getElementById("loader").style.display = "none";
            document.getElementById("page").style.display = "block";
        }
    </script>
    <script><%= done %></script>
</body>
</html>

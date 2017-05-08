<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Activate.aspx.cs" Inherits="User_Activate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <Avivnet:Intersense runat="server" ID="Intersense" />
    <title>EduPlug - Interactive school online | אדופלאג בית ספר אינטראקטיבי אונליין</title>
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

            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <div style="text-align: center; direction: rtl;" class="login-page">
                <div class="form">
                    <img src="../Content/graphics/img/logo.png" style="width: 250px" />
                    <br />
                    <br />
                    אנא
                    השלם את הפרטים הבאים כדי שתוכל להשלים את הליך ההרשמה
                    <div class="frm1">
                        <asp:RequiredFieldValidator ID="rfv_User_Email" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Email" runat="server" ErrorMessage="הכנס כתובת אימייל">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="refv_User_Email" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" EnableClientScript="true" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Email" runat="server" ErrorMessage="אמייל לא תקין">*</asp:RegularExpressionValidator>
                        <asp:TextBox ID="User_Email" runat="server" CssClass="tbbox" placeholder="אימייל"></asp:TextBox>

                        <asp:RequiredFieldValidator ID="rfv_User_PasswordTemp" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_PasswordTemp" runat="server" ErrorMessage="הכנס סיסמה זמניה">*</asp:RequiredFieldValidator>
                        <asp:TextBox ID="User_PasswordTemp" runat="server" CssClass="tbbox" TextMode="Password" placeholder="סיסמה זמנית"></asp:TextBox>

                        <asp:RequiredFieldValidator ID="rfv_User_Password" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Password" runat="server" ErrorMessage="הכנס סיסמה">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="refv_User_Password" Display="Dynamic" ValidationExpression="^([A-Za-z0-9]{8,32})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Password" runat="server" ErrorMessage="סיסמה חייבת להיות בין 8-32 תווים באנגלית ובספרות">*</asp:RegularExpressionValidator>
                        <asp:TextBox ID="User_Password" runat="server" CssClass="tbbox" TextMode="Password" placeholder="סיסמה"></asp:TextBox>

                        <asp:RequiredFieldValidator ID="rfv_User_Password_c" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Password_c" runat="server" ErrorMessage="הכנס אישור סיסמה">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="ctv_User_Password_c" runat="server" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Password_c" ControlToCompare="User_Password" ErrorMessage="הסיסמאות אינם תואמות">*</asp:CompareValidator>
                        <asp:TextBox ID="User_Password_c" runat="server" TextMode="Password" CssClass="tbbox" placeholder="אשר סיסמה"></asp:TextBox>
                        <button class="sbmBtn" style="color: white; width: 60%; margin-top: 10px" onclick="NavigateForm(2);return false;">הבא</button>
                    </div>
                    <div class="frm2 register-form">
                        <div class="tbbox">
                            <span id="PicMessage">
                                <h4>תמונת פרופיל</h4>
                            </span>
                            <table style="margin: 0 auto">
                                <tbody>
                                    <tr>
                                        <td>
                                            <img id="PreviewImage" alt="your image" style="display: none; width: 70px; height: 70px; border: 1px solid black; border-radius: 50%;" />
                                        </td>
                                        <td>
                                            <img src="../Content/graphics/img/remove-icon.png" title="בטל תמונה" id="delPic" onclick="CheckMe1(this);" style="display: none; width: 15px; height: 15px" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <asp:FileUpload ID="User_Picture" runat="server" accept=".png,.jpg" onchange="readURL1(this);" />

                            <script>
                                function readURL1(input) {
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
                                function CheckMe1(input) {
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
                        <asp:CustomValidator ID="cv_TempPass" runat="server" ErrorMessage="סיסמה זמנית שגויה" OnServerValidate="cv_TempPass_OnServerValidate" ForeColor="Red" ValidationGroup="RegisterValidationGroup">*</asp:CustomValidator>
                        <asp:CustomValidator ID="cv_User_Picture" runat="server" ErrorMessage="קובץ זה אינו תמונה" OnServerValidate="cv_User_Picture_OnServerValidate" ForeColor="Red" ValidationGroup="RegisterValidationGroup">*</asp:CustomValidator>
                        <a class="sbmBtn" style="color: white; width: 50%; margin-top: 10px" onclick="NavigateForm(1)">הקודם</a>
                        <asp:Button ID="RegisterButton" CssClass="sbmBtn" runat="server" Text="הרשמה" OnClick="RegisterButton_OnClick" Style="margin-top: 10px; width: 60%;" OnClientClick="return ValidateForm();" />
                    </div>
                    <br />
                    <script src="../Content/js/create.js"></script>
                    <asp:ValidationSummary ID="RegisterValidationSummary" CssClass="reg-validate-sum" DisplayMode="BulletList" ForeColor="Red" ValidationGroup="RegisterValidationGroup" runat="server" />

                    <p class="message">
                        רשום? <a href="~/User" runat="server">הכנס</a> |
                    לא רשום? <a href="#">צור חשבון</a>
                    </p>
                </div>
            </div>
        </form>
    </div>
    <script>
        $('.datepicker').pickadate({
            selectMonths: true, // Creates a dropdown to control month
            selectYears: 150 // Creates a dropdown of 150 years to control year
        });
        function myFunction() {
            myVar = setTimeout(showPage, 500);
        }

        function showPage() {
            document.getElementById("loader").style.display = "none";
            document.getElementById("page").style.display = "block";
        }
        var timeOutvar;//Storing the time out in order to stop it when its done
        function loadPage() {
            timeOutvar = setTimeout(showPage, 500);//Showing the page
        }
        function showPage() {
            document.getElementById("loader").style.display = "none";//Hiding the preloader
            document.getElementById("page").style.display = "block";//Showing the page
        }
    </script>
    <script><%= done %></script>
</body>

</html>

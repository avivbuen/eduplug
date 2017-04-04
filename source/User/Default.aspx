﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="User_Default" %>
<!-- FROM EDUPLUG.CO.IL -->
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <Avivnet:Intersense runat="server" ID="Intersense" />
    <title>EduPlug</title>
    <!-- FROM EDUPLUG.CO.IL -->
    <link href="../Content/css/connect.css" rel="stylesheet" />
    <script src="../Content/js/jquery.js"></script>
    <link href="../favicon.png" rel="icon" />
    <link href="../Content/css/loader.css" rel="stylesheet" />
    <link href="../Content/framework/Icons.css" rel="stylesheet" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="../Content/graphics/img/favicon.png" rel="icon" runat="server" />
</head>
<body onload="loadPage()">
    <div id="loader"></div>
    <div id="page">
        <form id="Connect" runat="server">
            <div style="text-align: center; direction: rtl;" class="login-page">
                <div class="form">
                    <!-- FROM EDUPLUG.CO.IL -->
                    <div class="login-form frm">
                        <img src="../Content/graphics/img/logo.png" style="width: 250px" />
                        <br />
                        <br />
                        <asp:RequiredFieldValidator ID="rfv_User_ID" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="LoginValidationGroup" ControlToValidate="User_ID" runat="server" ErrorMessage="הכנס תעודת זהות">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="refv_User_ID" Display="Dynamic" ValidationExpression="^[0-9]{9}$" EnableClientScript="true" ForeColor="Red" ValidationGroup="LoginValidationGroup" ControlToValidate="User_ID" runat="server" ErrorMessage="תעודת זהות לא תקינה">*</asp:RegularExpressionValidator>
                        <asp:TextBox ID="User_ID" runat="server" placeholder="תעודת זהות" CssClass="tbbox disablecopypaste" MaxLength="9"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv_User_Password" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="LoginValidationGroup" ControlToValidate="User_Password" runat="server" ErrorMessage="הכנס סיסמה">*</asp:RequiredFieldValidator>
                        <asp:TextBox ID="User_Password" runat="server" placeholder="סיסמה" CssClass="tbbox" TextMode="Password"></asp:TextBox>
                        <input type="submit" class="sbmBtn log" value="כניסה" />
                        <p class="message">לא רשום? <a href="#">צור חשבון</a> | שכחת סיסמה? <a href="Reset.aspx">אפס</a></p>
                        <p class="errorCode message" style="color: #ff0000"></p>
                        <asp:ValidationSummary ForeColor="Red" ID="ValidationSummaryLogin" ValidationGroup="LoginValidationGroup" runat="server" DisplayMode="BulletList" />
                    </div>
                    <div class="checkmark" style="display: none">
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
                        <p class="success">ברוך הבא <span id="username"></span></p>
                    </div>
                </div>
            </div>
            <script>
                function getUserid() {
                    return document.getElementById("<%= User_ID.ClientID%>").value;
                }
                function getPass() {
                    return document.getElementById("<%= User_Password.ClientID%>").value;
                }
                var host = "<%= Intel.GetFullRootUrl()%>";
                $(document).ready(function () {
                    $('input.disablecopypaste').bind('copy paste', function (e) {
                        e.preventDefault();
                    });
                });
            </script>
            <script src="../Content/js/connect.js"></script>
            <script>LoadCurrentUser();</script>
        </form>
    </div>
    <!-- FROM EDUPLUG.CO.IL -->
    <script>
        var timeOutvar;//Storing the time out in order to stop it when its done
        function loadPage() {
            timeOutvar = setTimeout(showPage, 500);//Showing the page
        }
        function showPage() {
            document.getElementById("loader").style.display = "none";//Hiding the preloader
            document.getElementById("page").style.display = "block";//Showing the page
        }
    </script>
</body>
</html>
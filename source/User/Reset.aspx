<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Reset.aspx.cs" Inherits="User_Reset" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <Avivnet:Intersense runat="server" ID="Intersense" />
    <title>EduPlug - Interactive school online | אדופלאג בית ספר אינטראקטיבי אונליין</title>
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
<body onload="myFunction()">

    <div id="loader"></div>
    <div id="page">
        <form id="form1" runat="server">
            <div style="text-align: center; direction: rtl;" class="login-page">
                <div class="form">
                    <img src="../Content/graphics/img/logo.png" style="width: 250px" />
                    <br />
                    <br />
                    <asp:Panel ID="PanelStage1" runat="server">
                        <asp:RequiredFieldValidator ID="rfv_User_ID" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="LoginValidationGroup" ControlToValidate="User_ID" runat="server" ErrorMessage="הכנס תעודת זהות">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="refv_User_ID" Display="Dynamic" ValidationExpression="^[0-9]{9}$" EnableClientScript="true" ForeColor="Red" ValidationGroup="LoginValidationGroup" ControlToValidate="User_ID" runat="server" ErrorMessage="תעודת זהות לא תקינה">*</asp:RegularExpressionValidator>
                        <asp:TextBox ID="User_ID" runat="server" placeholder="תעודת זהות" CssClass="tbbox disablecopypaste" MaxLength="9"></asp:TextBox>
                        <asp:Button ID="ResetButton" CssClass="sbmBtn" runat="server" Text="קבל קוד" OnClick="ResetButton_Click" Style="margin-top: 10px; width: 60%;"/>
                    </asp:Panel>
                    <asp:Panel ID="PanelStage2" runat="server" Visible="false">
                        הזן קוד איפוס:
                         <asp:RequiredFieldValidator ID="rfv_User_Code" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="LoginValidationGroup" ControlToValidate="User_Code" runat="server" ErrorMessage="הכנס סיסמה">*</asp:RequiredFieldValidator>
                        <asp:TextBox ID="User_Code" runat="server" placeholder="קוד איפוס" CssClass="tbbox" TextMode="Password"></asp:TextBox>
                        <asp:Button ID="CodeButton" CssClass="sbmBtn" runat="server" Text="אפס סיסמה" OnClick="CodeButton_Click" Style="margin-top: 10px; width: 60%;"/>
                    </asp:Panel>
                    <asp:Panel ID="PanelStage3" runat="server" Visible="false">
                        <asp:RequiredFieldValidator ID="rfv_User_Password" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="LoginValidationGroup" ControlToValidate="User_Password" runat="server" ErrorMessage="הכנס סיסמה">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="refv_User_Password" Display="Dynamic" ValidationExpression="^([A-Za-z0-9]{8,32})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="LoginValidationGroup" ControlToValidate="User_Password" runat="server" ErrorMessage="סיסמה חייבת להיות בין 8-32 תווים באנגלית ובספרות">*</asp:RegularExpressionValidator>
                        <asp:TextBox ID="User_Password" runat="server" CssClass="tbbox" TextMode="Password" placeholder="סיסמה חדשה"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv_User_Password_c" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="LoginValidationGroup" ControlToValidate="User_Password_c" runat="server" ErrorMessage="הכנס אישור סיסמה">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="ctv_User_Password_c" runat="server" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="LoginValidationGroup" ControlToValidate="User_Password_c" ControlToCompare="User_Password" ErrorMessage="הסיסמאות אינם תואמות">*</asp:CompareValidator>
                        <asp:TextBox ID="User_Password_c" runat="server" TextMode="Password" CssClass="tbbox" placeholder="אשר סיסמה"></asp:TextBox>
                        <asp:Button ID="ChangeButton" CssClass="sbmBtn" runat="server" Text="אישור" OnClick="ChangeButton_Click" Style="margin-top: 10px; width: 60%;"/>
                    </asp:Panel>
                    <p class="message">
                        רשום? <a href="~/User" runat="server">הכנס</a> |
                    לא רשום? <a href="#">צור חשבון</a>
                    </p>
                </div>
            </div>
            <script><%=script%></script>
        </form>
    </div>
    <script>
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
</body>

</html>

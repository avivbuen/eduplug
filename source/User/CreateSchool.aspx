<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateSchool.aspx.cs" Inherits="User_CreateSchool" %>

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
                    <asp:Panel ID="PanelStage1" runat="server">
                        ברוך הבא להליך יצירת בית ספר.
                        <br />
                        בחר האם בית הספר שלך מוכר רשמית על ידי משרד החינוך:
                        <br />
                        <asp:Button ID="KnownBtn" CssClass="sbmBtn" runat="server" Text="מוכר" OnClick="Known_OnClick" Style="margin-top: 10px; width: 50%;" />
                        <asp:Button ID="UknownBtn" CssClass="sbmBtn" runat="server" Text="אינו מוכר" OnClick="UknownBtn_OnClick" Style="margin-top: 10px; width: 40%;" />
                    </asp:Panel>
                    <asp:Panel ID="PanelStageSchool" runat="server" Visible="False">

                        <asp:Panel ID="PanelKnown" runat="server" Visible="False">

                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Style="width: 100%" ControlToValidate="SchoolCity" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" InitialValue="-1" ErrorMessage="בחר עיר לימוד">*</asp:RequiredFieldValidator>
                                    <asp:DropDownList ID="SchoolCity" runat="server" CssClass="tbbox" OnSelectedIndexChanged="SchoolCity_OnSelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Text="בחר עיר לימוד" Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Style="width: 100%" ControlToValidate="SchoolName" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" InitialValue="-1" ErrorMessage="בחר מוסד לימוד">*</asp:RequiredFieldValidator>
                                    <asp:DropDownList ID="SchoolName" runat="server" CssClass="tbbox" Visible="False">
                                        <asp:ListItem Text="בחר מוסד לימוד" Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                        <asp:Panel ID="PanelUnKnown" runat="server" Visible="False">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" EnableClientScript="true" SetFocusOnError="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="School_Name" runat="server" ErrorMessage="הכנס שם בית ספר">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Display="Dynamic" SetFocusOnError="true" ValidationExpression="^([\sא-ת0-9\.]{2,50})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="School_Name" runat="server" ErrorMessage="שם בית ספר חייב להיות  בין 2-50 תווים">*</asp:RegularExpressionValidator>
                            <asp:TextBox ID="School_Name" runat="server" CssClass="tbbox pt1" placeholder="שם בית ספר"></asp:TextBox>
                        </asp:Panel>
                        <div class="tbbox">
                            <span id="PicMessage">
                                <h4>לוגו בית ספר</h4>
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
                            <asp:FileUpload ID="FileUploadLogo" runat="server" accept=".png,.jpg" onchange="readURL1(this);" />

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
                                    var myPic = document.getElementById('<%=FileUploadLogo.ClientID%>');
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

                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="בית ספר כבר רשום" OnServerValidate="CustomValidator2_OnServerValidate" ForeColor="White" ValidationGroup="RegisterValidationGroup"> * </asp:CustomValidator>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="קובץ זה אינו תמונה" OnServerValidate="PictureUpload_OnServerValidate" ForeColor="White" ValidationGroup="RegisterValidationGroup"> * </asp:CustomValidator>
                        <asp:Button ID="prev1Known" CssClass="sbmBtn" runat="server" Text="הקודם" OnClick="prev1Known_OnClick" Style="margin-top: 10px; width: 30%;" />
                        <asp:Button ID="nextKnown" CssClass="sbmBtn" runat="server" Text="הבא" OnClick="nextKnown_OnClick" Style="margin-top: 10px; width: 60%;" />
                    </asp:Panel>
                    <asp:Panel ID="PanelStage2" runat="server" Visible="False">
                        <div class="frm1">
                            <asp:RequiredFieldValidator ID="rfv_User_First_Name" EnableClientScript="true" SetFocusOnError="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_First_Name" runat="server" ErrorMessage="הכנס שם פרטי">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="refv_User_First_Name" Display="Dynamic" SetFocusOnError="true" ValidationExpression="^([\sא-ת]{2,40})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_First_Name" runat="server" ErrorMessage="שם פרטי חייב להיות בעברית בלבד בין 2-40 תווים">*</asp:RegularExpressionValidator>
                            <asp:TextBox ID="User_First_Name" runat="server" CssClass="tbbox pt1" placeholder="שם פרטי"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv_User_Last_Name" EnableClientScript="true" SetFocusOnError="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Last_Name" runat="server" ErrorMessage="הכנס שם משפחה">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="refv_User_Last_Name" Display="Dynamic" SetFocusOnError="true" ValidationExpression="^([א-ת]{2,10})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Last_Name" runat="server" ErrorMessage="שם משפחה חייב להיות בעברית בלבד בין 2-10 תווים">*</asp:RegularExpressionValidator>
                            <asp:TextBox ID="User_Last_Name" runat="server" CssClass="tbbox pt1" placeholder="שם משפחה"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv_User_ID" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_ID" runat="server" ErrorMessage="הכנס תעודת זהות">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="refv_User_ID" Display="Dynamic" ValidationExpression="^[0-9]{9}$" SetFocusOnError="true" EnableClientScript="true" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_ID" runat="server" ErrorMessage="תעודת זהות לא תקינה">*</asp:RegularExpressionValidator>
                            <asp:TextBox ID="User_ID" runat="server" CssClass="tbbox pt1" placeholder="תעודת זהות" MaxLength="9"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv_User_Password" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Password" runat="server" ErrorMessage="הכנס סיסמה">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="refv_User_Password" Display="Dynamic" ValidationExpression="^([A-Za-z0-9]{8,32})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Password" runat="server" ErrorMessage="סיסמה חייבת להיות בין 8-32 תווים באנגלית ובספרות">*</asp:RegularExpressionValidator>
                            <asp:TextBox ID="User_Password" runat="server" CssClass="tbbox" TextMode="Password" placeholder="סיסמה"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv_User_Password_c" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Password_c" runat="server" ErrorMessage="הכנס אישור סיסמה">*</asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="ctv_User_Password_c" runat="server" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Password_c" ControlToCompare="User_Password" ErrorMessage="הסיסמאות אינם תואמות">*</asp:CompareValidator>
                            <asp:TextBox ID="User_Password_c" runat="server" TextMode="Password" CssClass="tbbox" placeholder="אשר סיסמה"></asp:TextBox>
                            <asp:Button ID="prevKnown" CssClass="sbmBtn" runat="server" Text="הקודם" OnClick="prevKnown_OnClick" Style="margin-top: 10px; width: 30%;" />
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
                            <asp:RequiredFieldValidator ID="rfv_User_Gender" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_Gender" runat="server" ErrorMessage="הכנס מגדר">*</asp:RequiredFieldValidator>
                            <asp:RadioButtonList ID="User_Gender" runat="server" RepeatDirection="Vertical" RepeatColumns="2" CssClass="tbbox">
                                <asp:ListItem Text="זכר" Value="m"></asp:ListItem>
                                <asp:ListItem Text="נקבה" Value="f"></asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="rfv_User_BornDate" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="User_BornDate" runat="server" ErrorMessage="הכנס תאריך לידה">*</asp:RequiredFieldValidator>
                            <asp:TextBox ID="User_BornDate" runat="server" CssClass="tbbox datepicker" placeholder="תאריך לידה"></asp:TextBox>
                            <asp:CustomValidator ID="cv_User_Picture" runat="server" ErrorMessage="קובץ זה אינו תמונה" OnServerValidate="cv_User_Picture_OnServerValidate" ForeColor="Red" ValidationGroup="RegisterValidationGroup">*</asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="תעודת זהות קיימת במערכת" OnServerValidate="CustomValidator3_OnServerValidate" ForeColor="Red" ValidationGroup="RegisterValidationGroup">*</asp:CustomValidator>

                            <a class="sbmBtn" style="color: white; width: 50%; margin-top: 10px" onclick="NavigateForm(1)">הקודם</a>
                            <asp:Button ID="RegisterButton" CssClass="sbmBtn" runat="server" Text="הרשמה" OnClick="RegisterButton_OnClick" Style="margin-top: 10px; width: 60%;" OnClientClick="return ValidateForm();" />
                        </div>
                        <br />
                        <script src="../Content/js/create.js"></script>
                    </asp:Panel>
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

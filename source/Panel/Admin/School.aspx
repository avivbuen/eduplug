<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="School.aspx.cs" Inherits="Panel_Admin_School" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div dir="rtl" style="max-width: 500px; text-align: center; margin: 0 auto; box-shadow: -1px 1px 5px 0px; background: #fff; padding: 30px;">
        <asp:ValidationSummary ID="AllowValidationSummary" ForeColor="Red" Style="list-style-type: upper-roman;" ValidationGroup="AllowValidationGroup" runat="server" />
        <h1><i class="material-icons" style="font-size: 72px;">school</i></h1>
        <div class="row" style="text-align: right;">
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" EnableClientScript="true" SetFocusOnError="true" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="School_Name" runat="server" ErrorMessage="הכנס שם בית ספר">*</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Display="Dynamic" SetFocusOnError="true" ValidationExpression="^([\sא-ת0-9\.]{2,50})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="RegisterValidationGroup" ControlToValidate="School_Name" runat="server" ErrorMessage="שם בית ספר חייב להיות  בין 2-50 תווים">*</asp:RegularExpressionValidator>
            <asp:TextBox ID="School_Name" runat="server" CssClass="tbbox pt1" placeholder="שם בית ספר"></asp:TextBox>

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
        </div>
        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="קובץ זה אינו תמונה" OnServerValidate="PictureUpload_OnServerValidate" ForeColor="White" ValidationGroup="RegisterValidationGroup"> * </asp:CustomValidator>
        <asp:Button ID="applyBtn" CssClass="btn" runat="server" Text="שמור" OnClick="applyBtn_OnClick" Style="margin-top: 10px; width: 60%;" />
        <asp:ValidationSummary ID="RegisterValidationSummary" CssClass="reg-validate-sum" DisplayMode="BulletList" ForeColor="Red" ValidationGroup="RegisterValidationGroup" runat="server" />
    </div>
    

    <script><%= done %></script>
</asp:Content>


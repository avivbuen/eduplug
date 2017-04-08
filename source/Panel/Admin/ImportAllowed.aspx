<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ImportAllowed.aspx.cs" Inherits="Panel_Admin_ImportAllowed" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="direction: rtl">
        
        <h3><a href="Allowed.aspx"><i class="material-icons">navigate_next</i></a> הוראות להעלת קובץ: </h3>
        <ul style="list-style-type: circle;">
            <li>- על הקובץ להיות בעל טופס אחד </li>
            <li>- הקובץ יכיל שלושה עמודות - שם פרטי, שם משפחה, תעודת זהות</li>
            <li>- הקובץ חייב להיות בפורמט אקסל חדש(xlsx)</li>
        </ul>
        <a href="../../Content/ipi.xlsx">קובץ דוגמא</a>
        <br /><hr />
         <b>בחר מספר העמודה המכילה את שם המשפחה!</b>
        <asp:DropDownList ID="DropDownFam" runat="server" CssClass="browser-default drpdwnFam">
             <asp:ListItem Text="בחר מספר עמודה" Value="-1"></asp:ListItem>
            <asp:ListItem Text="1" Value="0"></asp:ListItem>
            <asp:ListItem Text="2" Value="1"></asp:ListItem>
            <asp:ListItem Text="3" Value="2"></asp:ListItem>
        </asp:DropDownList>
        <br /><br /><br />
    </div>
    <asp:Panel ID="PanelUpload" runat="server">
        <div class="wrapper" >
            <div class="file-upload">
                <asp:FileUpload ID="FileUploadExcel" runat="server" CssClass="fileLoader" disabled="disabled" accept=".xlsx" />
                <span style="font-size: 15px">העלה קובץ אקסל</span>
                <i class="material-icons text-accent-4" style="font-size: 50px">
                    <asp:Literal ID="LiteralRespIcon" runat="server" Text="file_upload"></asp:Literal></i>
            </div>
        </div>
        <div>
            <asp:Literal ID="LiteralResp" runat="server" Text=""></asp:Literal></div>
        <script>
            $(".fileLoader").change(function () { this.form.submit(); });
            $(function () {
                var val = $('.drpdwnFam option:selected').text();
                if (val != "בחר מספר עמודה") {
                    $(".fileLoader").removeAttr("disabled");
                }
                else {
                    $(".fileLoader").attr("disabled", true);
                }
            });
            $(".drpdwnFam").change(function () {
                var val = $('.drpdwnFam option:selected').text();
                if (val != "בחר מספר עמודה")
                {
                    $(".fileLoader").removeAttr("disabled");
                }
                else {
                    $(".fileLoader").attr("disabled",true);
                }
            });
        </script>
    </asp:Panel>
    <style>
        .wrapper {
            width: 100%;
            height: 100%;
            display: flex;
            align-items: center;
            justify-content: center;
        }

            .wrapper .file-upload {
                height: 200px;
                width: 200px;
                border-radius: 100px;
                position: relative;
                display: flex;
                justify-content: center;
                align-items: center;
                border: 4px solid #FFFFFF;
                overflow: hidden;
                background-image: linear-gradient(to bottom, #2590EB 50%, #FFFFFF 50%);
                background-size: 100% 200%;
                transition: all 1s;
                color: #FFFFFF;
                font-size: 100px;
            }

                .wrapper .file-upload input[type='file'] {
                    height: 200px;
                    width: 200px;
                    position: absolute;
                    top: 0;
                    left: 0;
                    opacity: 0;
                    cursor: pointer;
                }

                .wrapper .file-upload:hover {
                    background-position: 0 -100%;
                    color: #2590EB;
                }
    </style>
</asp:Content>


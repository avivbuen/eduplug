<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ImportAllowed.aspx.cs" Inherits="Panel_Admin_ImportAllowed" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="direction: rtl">
        <h3>הוראות להעלת קובץ: </h3>
        <ul style="list-style-type: circle;">
            <li>על הקובץ להיות בעל טופס אחד</li>
            <li>הקובץ יכיל שלושה עמודות - שם פרטי, שם משפחה, תעודת זהות</li>
            <li>(xlsx) הקובץ חייב להיות בפורמט אקסל חדש</li>
        </ul>
        <a href="../../Content/ipi.xlsx">קובץ דוגמא</a>
    </div>
    <asp:Panel ID="PanelUpload" runat="server">
        <div class="wrapper">
            <div class="file-upload">
                <asp:FileUpload ID="FileUploadExcel" runat="server" CssClass="fileLoader" />
                <span style="font-size: 15px">
                    <asp:Literal ID="LiteralResp" runat="server" Text="העלה קובץ אקסל"></asp:Literal></span>
                <i class="material-icons text-accent-4" style="font-size: 50px">
                    <asp:Literal ID="LiteralRespIcon" runat="server" Text="file_upload"></asp:Literal></i>
            </div>
        </div>
        <script>
            $(".fileLoader").change(function () { this.form.submit(); });
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


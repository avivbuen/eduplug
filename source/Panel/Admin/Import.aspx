<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Import.aspx.cs" Inherits="Panel_Admin_Import" %>

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
        <br />
        סוג:
        <br />
        a- מנהל,
        t- מורה,
        s- תלמיד,
        p- הורה
        <br />
        מגדר: זכר/נקבה
        <br />
        מזהי מגמות: מזהי מגמות יופרד באמצעות פסיק (1,2,3)
        <a href="../../Content/ipi.xlsx">קובץ דוגמא</a>
        <br />
        <hr />
        <b>אלו הם שמות העמודות הדרושים בסדר הבא!</b><br />
        <asp:ListBox ID="ListBoxReq" runat="server" Style="display: inline; height: 10rem; width: 33%">
            <asp:ListItem Text="שם פרטי" Value="0"></asp:ListItem>
            <asp:ListItem Text="שם משפחה" Value="1"></asp:ListItem>
            <asp:ListItem Text="תעודת זהות" Value="2"></asp:ListItem>
            <asp:ListItem Text="סוג" Value="3"></asp:ListItem>
            <asp:ListItem Text="כיתה" Value="4"></asp:ListItem>
            <asp:ListItem Text="פלאפון" Value="5"></asp:ListItem>
            <asp:ListItem Text="מגדר" Value="6"></asp:ListItem>
        </asp:ListBox>
        <asp:ListBox ID="ListBoxMajors" runat="server" Style="display: inline; height: 10rem; width: 33%"></asp:ListBox>
        <asp:ListBox ID="ListBoxCity" runat="server" Style="display: inline; height: 10rem; width: 33%"></asp:ListBox>


        <style>
            .uploadState {
                display: none
            }
        </style>
        מזהה עיר יוצג במספרו: 
    <div class="wrapper">
        <div class="file-upload">
            <input name="fileExcel" id="fileExcel" type="file" class="fileLoader" />
            <span class="respServer">
                <i class="material-icons text-accent-4" style="font-size: 50px">
                file_upload    
                </i>
            </span>
        </div>
    </div>
        <script>
            $(".fileLoader").change(function () { CalluploaderHandler(); });
            function CalluploaderHandler() {
                AvivnetFramework.toast('מעלה קובץ...', 6000);
                clearTimeout(serverContact);
                //$('.uploadState').css('display', 'block');
                var files = $("#fileExcel")[0].files;
                if (files.length > 0) {
                    var formData = new FormData();
                    for (var i = 0; i < files.length; i++) {
                        formData.append(files[i].name, files[i]);
                    }
                    $.ajax({
                        type: "POST",
                        url: host + "/System/Import.ashx",
                        data: formData,
                        contentType: false,
                        processData: false,
                        success: OnComplete,
                        error: OnFail
                    });
                }
                return false;
            }

            function OnComplete(result) {
                LoadCurrentUser();
                //$('.uploadState').css('display', 'none');
                if (result === "הועלה")
                    AvivnetFramework.toast('המשתמשים הועלו', 6000);
                else
                    AvivnetFramework.toast(result, 6000);
                //$("#fileopen")[0].val("");

                $("#fileopen").replaceWith($("#fileopen").clone());
            }

            function OnFail(result) {
                LoadCurrentUser();
                alert('Request failed');
            }
        </script>
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
        </style>
    </div>
</asp:Content>


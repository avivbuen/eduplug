<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Student.aspx.cs" Inherits="Panel_Teacher_Student" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="../../Content/js/JsBarcode.all.min.js"></script>
    <div id="studentDet">
        <div  style="float:right; width:100%; text-align:right;">
            <div id="logo-place"></div>
            <h2>
                <asp:Label ID="LabelName" runat="server" Text="Label"></asp:Label></h2>
            ת.ז: <asp:Label ID="LabelID" runat="server" Text="Label"></asp:Label>
        </div>
        <br />
        <div id="GridViewDiscplines_tbl" style="border: groove; direction: rtl; float: right; margin-top: 0px; width: 100%; max-width: 500px">
            <h5 style="font-weight: 600; text-align: center;">הערות משמעת</h5>
            <asp:GridView ID="GridViewDiscplines" CssClass="bordered" runat="server" AutoGenerateColumns="False" Style="width: 100%">
                <Columns>
                    <asp:BoundField DataField="lName" HeaderText="שיעור"></asp:BoundField>
                    <asp:BoundField DataField="dName" HeaderText="הערה"></asp:BoundField>
                    <asp:BoundField DataField="dHour" HeaderText="שעה"></asp:BoundField>
                    <asp:BoundField DataField="dDate" DataFormatString="{0: dd/MM/yyyy}" HeaderText="תאריך"></asp:BoundField>
                </Columns>
            </asp:GridView>
            <div style="text-align: center;">
                <asp:Literal ID="LiteralEmptyDiscplines" runat="server"></asp:Literal>
            </div>
        </div>
        <div id="GridViewScores_tbl" style="border: groove; direction: rtl; float: right; margin-top: 0px; width: 100%; max-width: 500px">
            <h5 style="font-weight: 600; text-align: center;">ציונים</h5>
            <asp:GridView ID="GridViewScores" CssClass="bordered" runat="server" AutoGenerateColumns="False" Style="width: 100%">
                <Columns>
                    <asp:BoundField DataField="Exam.Title" HeaderText="מבחן"></asp:BoundField>
                    <asp:BoundField DataField="ScoreVal" HeaderText="ציון"></asp:BoundField>
                    <asp:BoundField DataField="Exam.Date" DataFormatString="{0: dd/MM/yyyy}" HeaderText="תאריך"></asp:BoundField>
                </Columns>
            </asp:GridView>
            <div style="text-align: center;">
                <asp:Literal ID="LiteralEmptyScores" runat="server"></asp:Literal>
            </div>
        </div>
        <svg id="ean-13" style="text-align:center;"></svg>
    </div>
    <div class="fixed-action-btn" style="right:auto; left:23px">
        <a class="btn-floating btn-large red" onclick="PrintPage()">
            <i class="material-icons">print</i>
        </a>
    </div>
    <script>
        JsBarcode("#ean-13", "<%= idSTD%>");
        function PrintPage() {
            var prtContent = document.getElementById("studentDet");
            var WinPrint = window.open('', '', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');
            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.getElementById('logo-place').innerHTML = "<img src='../../Content/graphics/img/logo1.png' style='float:left; width:200px;filter: grayscale(100%);'/>";
            WinPrint.document.getElementById("GridViewScores_tbl").style.maxWidth = "100%";
            WinPrint.document.getElementById("GridViewDiscplines_tbl").style.maxWidth = "100%";
            setTimeout(Print(WinPrint),500);
            WinPrint.document.close();

        }
        function Print(WinPrint)
        {
            WinPrint.focus();

            WinPrint.print();
            WinPrint.close();
        }
    </script>
</asp:Content>


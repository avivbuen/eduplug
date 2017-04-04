<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Adjustments.aspx.cs" Inherits="Panel_Student_Adjustments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../../Content/js/JsBarcode.all.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        table, th, td {
            border: solid 1px !important;
        }
    </style>
    <div id="GridViewDiscplines_tbl" style="border: groove; direction: rtl; float: right; margin-top: 0px; width: 100%;background:#fff">
        <h5 style="font-weight: 600; text-align: center;">כרטיס התאמות לתלמיד</h5>
        <asp:GridView ID="GridViewAdjustment" CssClass="bordered" runat="server" AutoGenerateColumns="False" Style="width: 100%">
            <Columns>
                <asp:TemplateField HeaderText="#">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Name" HeaderText="התאמה"></asp:BoundField>
            </Columns>
        </asp:GridView>
        <div style="text-align: center;">
            <asp:Literal ID="LiteralEmptyAdjust" runat="server"></asp:Literal>
        </div>
    </div>
    <div id="barcode"></div>
        <div class="fixed-action-btn" style="right:auto; left:23px">
        <a class="btn-floating btn-large red" onclick="PrintPage()">
            <i class="material-icons">print</i>
        </a>
    </div>
    <script>
        function PrintPage() {
            var prtContent = document.getElementById("GridViewDiscplines_tbl");
            var WinPrint = window.open('', '', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');
            WinPrint.document.write(prtContent.innerHTML);
           // WinPrint.document.getElementById('logo-place').innerHTML = "<img src='../../Content/graphics/img/logo1.png' style='float:left; width:200px;filter: grayscale(100%);'/>";
            //WinPrint.document.getElementById("GridViewScores_tbl").style.maxWidth = "100%";
            //WinPrint.document.getElementById("GridViewDiscplines_tbl").style.maxWidth = "100%";
            setTimeout(Print(WinPrint), 500);
            WinPrint.document.close();

        }
        function Print(WinPrint) {
            WinPrint.focus();

            WinPrint.print();
            WinPrint.close();
        }
    </script>
</asp:Content>


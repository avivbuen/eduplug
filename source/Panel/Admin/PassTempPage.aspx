<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PassTempPage.aspx.cs" Inherits="Panel_Admin_PassTempPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="GridViewPass_tbl" class="table-responsive-vertical shadow-z-1" style="direction: rtl; text-align: center;">
        <asp:GridView ID="GridViewPass" CssClass="datatables-table table" OnDataBound="GridViewDiscplines_DataBound" runat="server" AutoGenerateColumns="False" Style="width: 100%">
            <Columns>
                <asp:BoundField DataField="eduFirstName" HeaderText="שם פרטי"></asp:BoundField>
                <asp:BoundField DataField="eduLastName" HeaderText="שם משפחה"></asp:BoundField>
                <asp:BoundField DataField="Pass" HeaderText="סיסמה"></asp:BoundField>
            </Columns>
        </asp:GridView>
        <div style="text-align: center;">
            <asp:Literal ID="LiteralEmptyDiscplines" runat="server"></asp:Literal>
        </div>
    </div>
    <div class="fixed-action-btn" style="right:auto; left:23px">
        <a class="btn-floating btn-large red" onclick="PrintPage()">
            <i class="material-icons">print</i>
        </a>
    </div>
    <script>
        //JsBarcode("#ean-13", "<%= MemberService.GetCurrent().School.Id%>");
        function PrintPage() {
            var prtContent = document.getElementById("GridViewPass_tbl");
            var WinPrint = window.open('', '', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0,dir=rtl');
            WinPrint.document.write(prtContent.innerHTML);
            //WinPrint.document.getElementById('logo-place').innerHTML = "<img src='../../Content/graphics/img/logo1.png' style='float:left; width:200px;filter: grayscale(100%);'/>";
           // WinPrint.document.getElementById("GridViewScores_tbl").style.maxWidth = "100%";
            WinPrint.document.getElementById("GridViewPass").style.maxWidth = "100%";
            setTimeout(Print(WinPrint),500);
            WinPrint.document.close();

        }
        function Print(WinPrint)
        {
            WinPrint.focus();

            WinPrint.print();
            WinPrint.close();
        }
        $('.datatables-table').DataTable({
            // Enable mark.js search term highlighting
            mark: true
        });
    </script>
</asp:Content>


<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Week.aspx.cs" Inherits="Week" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>מערכת שעות</title>
    <link href="../Content/framework/Site.css" rel="stylesheet" />
</head>
<body>
    <style>
        table.college {
            width: 100% !important;
            height:95vh;
            margin:0 !important;
            max-width:100% !important;
        }
        nav{
            height: 5vh !important;
            padding-bottom:15px !important;
        }
    </style>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <nav class="grey">
            <a href="../Default.aspx" class="center" style="width: 100%; text-align: center; margin: 0 auto;">
                <img src="../Content/graphics/img/logo1.png" class="center" style="height: 50px; margin: 0 auto;" />
            </a>
        </nav>
        <Avivnet:TimeTableWeek ID="WeekTimeTable" runat="server" />
    </form>
</body>
</html>

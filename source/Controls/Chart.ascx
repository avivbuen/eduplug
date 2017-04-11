<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Chart.ascx.cs" Inherits="Controls_Chart" %>
<%@ Import Namespace="System.Drawing" %>
<div id="piechart" class="card blue-grey" style="width: 500px; height: 200px;padding:20px 20px 20px 20px; margin:0 auto"></div>
<script>
    function buildChart() {
        document.getElementById("piechart").style.width = "700px";
        document.getElementById("piechart").style.height = "<%= Height%>px";
        google.charts.load('current', { 'packages': ['corechart']});
        google.charts.setOnLoadCallback(drawChart);
        function drawChart() {

            var data = google.visualization.arrayToDataTable([["<%=xAxisHeader%>","<%=yAxisHeader%>"],<%= jsonData%>]);

            var options = {
                title: '<%=Title%>',
                backgroundColor: { fill: "<%= ColorTranslator.ToHtml(BackColor) %>" },
                is3D: true
            };

            var chart = new google.visualization.<%=GetDesign()%>(document.getElementById('piechart'));

            chart.draw(data, options);
        }
    }
    buildChart();
</script>
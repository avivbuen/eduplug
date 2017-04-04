using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.Script.Serialization;
public partial class Controls_Chart : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public Controls_Chart()
    {
        Width = 600;
        Height = 350;
        Title = "Title";
        BackColor = ColorTranslator.FromHtml("#F4F4F4");
        ChartDesign = ChartType.Pie;
        xAxisHeader = "xAxis";
        yAxisHeader = "yAxis";
    }
    protected string jsonData;
    public Dictionary<string, object> DataSource
    {
        get
        {
            return new Dictionary<string, object>();
        }
        set
        {
            Dictionary<object,object> obj = value.ToDictionary(entry => (object)entry.Key, entry => (object)entry.Value);
            this.jsonData = Serialize(obj);
        }
    }
    protected string Serialize<T>(Dictionary<object, T> data)
    {
        string str = "";
        int count = 0;
        foreach (KeyValuePair<object, T> entry in data)
        {
            count++;
            if (count != data.Count)
                str += "['" + entry.Key.ToString() + "',  " + entry.Value.ToString() + "],";
            else
                str += "['" + entry.Key.ToString() + "',  " + entry.Value.ToString() + "]";
        }
        return str;
    }
    public string xAxisHeader { get; set; }
    public string yAxisHeader { get; set; }
    public uint Width { get; set; }
    public uint Height { get; set; }
    public string Title { get; set; }
    public Color BackColor { get; set; }
    public ChartType ChartDesign { get; set; }
    public string BackColorHEX
    {
        get { return ColorTranslator.ToHtml(BackColor); }
        set { this.BackColor = ColorTranslator.FromHtml(value); }
    }
    protected string GetDesign()
    {
        switch (ChartDesign)
        {
            case ChartType.Pie:
                return "PieChart";
            case ChartType.Line:
                return "LineChart";
            case ChartType.GeoChart:
                return "GeoChart";
            default:
                return "PieChart";
        }
    }
}
public enum ChartType
{
    Pie,
    Line,
    GeoChart
}

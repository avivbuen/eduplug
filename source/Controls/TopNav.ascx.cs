using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_TopNav : System.Web.UI.UserControl
{
    protected string style;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public Controls_TopNav()
    {
        this.backColor = ColorTranslator.FromHtml("#3b6bab");
        style = "style='background:" + ColorTranslator.ToHtml(this.backColor) + ";'";
    }

    private Color backColor;
    public string BackColorHTML
    {
        get
        {
            return ColorTranslator.ToHtml(backColor);
        }
    }
    public Color BackColor
    {
        get
        {
            return this.backColor;
        }
        set
        {
            this.backColor = value;
            style = "style='background:" + ColorTranslator.ToHtml(this.backColor) + ";'";
        }
    }
  

}
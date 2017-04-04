using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_SetYear : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["year"]!=null&& Request.QueryString["year"].ToString().Trim()!="")
        {
            try
            {
                int year = int.Parse(Request.QueryString["year"].ToString().Trim());
                EduSysDate.SetYear(year);
                Intel.Redirect();               
            }
            catch
            {
                Intel.Redirect();
            }
        }
        else
        {
            Intel.Redirect();
        }
    }
}
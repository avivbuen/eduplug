using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic.Lessons;

public partial class Week : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        WeekTimeTable.DataSource = LessonService.GetTimeTable("יא");
        WeekTimeTable.DataBind();
    }
}
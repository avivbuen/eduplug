using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Lessons;
using Business_Logic.Members;

public partial class Panel_Student_TimeTable : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth == MemberClearance.Student)
        {
            TimeTableWeek.DataSource = LessonService.GetTimeTable(MemberService.GetCurrent().UserID, MemberClearance.Student);
            TimeTableWeek.DataBind();
        }
        else
        {
            Response.Redirect("~/Default.aspx");
        }

    }
}
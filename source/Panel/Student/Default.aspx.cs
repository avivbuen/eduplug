using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Disciplines;
using Business_Logic.Lessons;
using Business_Logic.Members;
using Business_Logic.Scores;

public partial class InterTrack_Student_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Student)
            Response.Redirect("~/");

        TimeTableDay.DataSource = LessonService.GetTimeTable(MemberService.GetCurrent().UserID, MemberClearance.Student);
        TimeTableDay.DataBind();
        ListViewScores.DataSource = ScoreService.GetAllStudent(MemberService.GetCurrent().UserID).Where(x => x.Exam.Date >= DateTime.Now.AddDays(-30));
        ListViewScores.DataBind();
        ListViewDisi.DataSource = DisciplinesServices.GetStudent(MemberService.GetCurrent().UserID, DateTime.Now.AddDays(-2));
        ListViewDisi.DataBind();
        if (ListViewScores.Items.Count == 0)
        {
            LabelEmpty.Text = "אין ציונים חדשים";
        }
        else
        {
            LabelEmpty.Text = "";
        }
        if (ListViewDisi.Items.Count == 0)
        {
            LabelEmptyDisi.Text = "אין הערות חדשות";
        }
        else
        {
            LabelEmptyDisi.Text = "";
        }

    }
}
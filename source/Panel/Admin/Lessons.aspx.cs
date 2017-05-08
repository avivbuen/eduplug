using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Grades;
using Business_Logic.Lessons;
using Business_Logic.Members;
using Business_Logic.TeacherGrades;

public partial class Panel_Admin_Lessons : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Admin)
            Response.Redirect("~/");
        if (!IsPostBack)
            Fill();
    }
    protected void Fill()
    {
        if (Request.QueryString["tgid"] == null || Request.QueryString["tgid"].ToString() == "")
            Response.Redirect("~/");
        TeacherGrade t = TeacherGradeService.Get(int.Parse(Request.QueryString["tgid"].ToString().Trim()));
        if (t == null) Response.Redirect("~/");
        Session["ltgCur"] = t;
        LabelName.Text = t.Name;
        ListViewLessons.DataSource = LessonService.GetLessons(t.Id);
        ListViewLessons.DataBind();
        if (ListViewLessons.Items.Count == 0)
            LabelLessonsEmpty.Visible = true;
        else
            LabelLessonsEmpty.Visible = false;
    }
    protected string CastDate(object obj)
    {
        DayOfWeek day = (DayOfWeek)((int)obj-1);
        switch (day)
        {
            case DayOfWeek.Sunday:
                return "ראשון";
            case DayOfWeek.Monday:
                return "שני";
            case DayOfWeek.Tuesday:
                return "שלישי";
            case DayOfWeek.Wednesday:
                return "רביעי";
            case DayOfWeek.Thursday:
                return "חמישי";
            case DayOfWeek.Friday:
                return "שישי";
            case DayOfWeek.Saturday:
                return "שבת";
        }
        return "";
    }

    protected void ListViewLessons_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        int lid = int.Parse(ListViewLessons.DataKeys[e.ItemIndex]["ID"].ToString());
        LessonService.DeleteLesson(lid);
        Fill();
    }

    protected void Lesson_Day_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["ltgCur"] == null) Response.Redirect("~/");
        TeacherGrade tg = (TeacherGrade)Session["ltgCur"];
        Lesson_Hour.Items.Clear();
        List<int> hours = MemberService.GetFreeHours(tg.TeacherId,int.Parse(Lesson_Day.SelectedValue));
        Lesson_Hour.Items.Add(new ListItem("בחר שעה...", "-1"));
        foreach (int hour in hours)
        {
             Lesson_Hour.Items.Add(new ListItem(hour.ToString(), hour.ToString()));
        }
        Lesson_Hour.Visible = true;
    }

    protected void AddButtonExam_Click(object sender, EventArgs e)
    {
        Page.Validate("AllowValidationGroup");
        if (Page.IsValid)
        {
            if (Request.QueryString["tgid"] == null || Request.QueryString["tgid"].ToString() == "")
                Response.Redirect("~/");
            Lesson lesson = new Lesson()
            {
                TeacherGradeId = int.Parse(Request.QueryString["tgid"].ToString().Trim()),
                Day = int.Parse(Lesson_Day.SelectedValue),
                Hour = int.Parse(Lesson_Hour.SelectedValue)
            };
            LessonService.Add(lesson);
            Fill();
        }
    }
}
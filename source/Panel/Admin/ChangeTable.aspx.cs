using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_ChangeTable : System.Web.UI.Page
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
        tGrade t = tGradeService.Get(int.Parse(Request.QueryString["tgid"].ToString().Trim()));
        if (t == null) Response.Redirect("~/");
        Session["ltgCur"] = t;
        LabelName.Text = t.Name;
        List<LessonChange> changes = new List<LessonChange>();
        List<Lesson> lsns = LessonService.GetLessons(t.ID);
        foreach (Lesson les in lsns)
        {
            changes.AddRange(LessonService.GetChanges(les.ID));
        }
        ListViewChanges.DataSource = changes;
        ListViewChanges.DataBind();
        if (ListViewChanges.Items.Count == 0)
            LabelLessonsEmpty.Visible = true;
        else
            LabelLessonsEmpty.Visible = false;
    }
    public string CastHour(object ob)
    {
        return LessonService.GetLesson((int)ob).Hour.ToString();
    }
    protected void ListViewChanges_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        int CHANGE_id = int.Parse(ListViewChanges.DataKeys[e.ItemIndex]["ID"].ToString());
        if (LessonService.CancelChange(CHANGE_id))
        {
            Fill();
        }

    }


    protected void Change_Date_TextChanged(object sender, EventArgs e)
    {
        if (Request.QueryString["tgid"] == null || Request.QueryString["tgid"].ToString() == "")
            Response.Redirect("~/");
        if (Change_Date.Text != "")
        {
            tGrade t = tGradeService.Get(int.Parse(Request.QueryString["tgid"].ToString().Trim()));
            DayOfWeek day = DateTime.ParseExact(Change_Date.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).DayOfWeek;
            int daoy = (int)day + 1;
            List<Lesson> lessons = LessonService.GetLessons(t.ID).Where(x => x.Day == daoy).OrderBy(x => x.Hour).ToList();
            Lesson_Hour.Items.Clear();
            Lesson_Hour.Items.Add(new ListItem("בחר שעה...", "-1"));
            foreach (Lesson lesson in lessons)
            {
                Lesson_Hour.Items.Add(new ListItem(lesson.Hour.ToString(), lesson.Hour.ToString()));
            }
            Lesson_Hour.Visible = true;
        }
    }

    protected void AddButtonChange_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["tgid"] == null || Request.QueryString["tgid"].ToString() == "")
            Response.Redirect("~/");
        Page.Validate();
        if (Page.IsValid)
        {
            tGrade t = tGradeService.Get(int.Parse(Request.QueryString["tgid"].ToString().Trim()));
            int day = (int)DateTime.ParseExact(Change_Date.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).DayOfWeek+1;
            LessonChange change = new LessonChange()
            {
                ChangeType = (LessonChangeType)Change_Cause.SelectedValue.ToString()[0],
                Date = DateTime.ParseExact(Change_Date.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                LessonID = LessonService.GetLesson(t.ID,int.Parse(Lesson_Hour.SelectedValue), day).ID,
                Message = TextBoxDesc.Text
            };
            LessonService.AddChange(change);
            Fill();
        }
    }
    protected string GetLessonChangeType(object ob)
    {
        LessonChangeType change = (LessonChangeType)ob;
        switch (change)
        {
            case LessonChangeType.Cancel:
                return "ביטול";
            case LessonChangeType.Fill:
                return "מילויי מקום";
            case LessonChangeType.Test:
                return "מבחן";
            case LessonChangeType.FinalTest:
                return "מבחן בגרות";
            case LessonChangeType.Unknown:
                return "לא ידוע";
            default:
                return "לא ידוע";
        }
    }
    protected void Lesson_Hour_SelectedIndexChanged(object sender, EventArgs e)
    {
        PanelDet.Visible = (Lesson_Hour.SelectedValue != "-1");
    }
}
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

public partial class Panel_Teacher_Lesson : System.Web.UI.Page
{
    public string script;
    public string done;
    public static List<DisciplineType> dtypes;
    public static List<DisciplineEvent> selected;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Teacher)
            Response.Redirect("~/Default.aspx");
        if (Request.QueryString["lid"] == null || Request.QueryString["d"] == null || Request.QueryString["m"] == null || Request.QueryString["y"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        else
        {
            PanelData.Visible = true;
            ButtonApply.Visible = true;
            int lid = int.Parse(Request.QueryString["lid"].ToString());
            DateTime date = new DateTime(int.Parse(Request.QueryString["y"].ToString()), int.Parse(Request.QueryString["m"].ToString()), int.Parse(Request.QueryString["d"].ToString()));
            if (date>DateTime.Now)
            {
                PanelData.Visible = false;
                ButtonApply.Visible = false;
                script = "<script>alert('אין אפשרות להזין הערות טרם מועד השיעור');location='Default.aspx';</script>";
                return;
            }
            Lesson lesson = LessonService.GetLesson(lid);
            List<LessonChange> changes = lesson.Changes.Where(x => x.Date == date && x.LessonId == lesson.Id).ToList();
            if (changes.Count > 0)
            {
                foreach (LessonChange change in changes)
                {
                    if (change.ChangeType == LessonChangeType.Cancel)
                    {
                        LabelDate.Text = date.ToShortDateString();
                        LabelHour.Text = "שעה " + lesson.Hour.ToString();
                        LabelTitle.Text = lesson.Name+" - השיעור בוטל לא ניתן להזין הערות";
                        PanelData.Visible = false;
                        ButtonApply.Visible = false;
                        return;
                    }
                }
            }
            selected = DisciplinesServices.GetSelected(lesson.Id, date);
            if (lesson == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            Session["LessonDedit1"] = date;
            Session["LessonDedit2"] = lesson.Id;
            LabelDate.Text = date.ToShortDateString();
            LabelHour.Text = "שעה " + lesson.Hour.ToString();
            LabelTitle.Text = lesson.Name;
            dtypes = DisciplinesServices.GetAllTypes().OrderBy(x => x.Score).ToList();
            ListViewTypes.DataSource = dtypes;
            ListViewTypes.DataBind();
            if (!IsPostBack)
            {
                ListViewStudents.DataSource = LessonService.GetAllStudents(lesson.Id);
                ListViewStudents.DataBind();
            }
            LabelResponse.Text = "";
        }
    }

    protected void ButtonApply_Click(object sender, EventArgs e)
    {
        if (Session["LessonDedit1"] == null || Session["LessonDedit2"] == null) Response.Redirect("~/Default.aspx");
        DisciplinesServices.ResetLesson((int)Session["LessonDedit2"], (DateTime)Session["LessonDedit1"]);
        foreach (ListViewItem student in ListViewStudents.Items)
        {
            foreach (ListViewItem dtp in ((ListView)student.FindControl("ListViewTypes")).Items)
            {
                CheckBox cb = (CheckBox)dtp.FindControl("CheckBoxItem");
                if (cb.Checked)
                {
                    int DataTypeID = (int)((ListView)student.FindControl("ListViewTypes")).DataKeys[dtp.DataItemIndex].Value;
                    int UserID = (int)(ListViewStudents.DataKeys[student.DataItemIndex].Value);
                    DisciplinesServices.Add((int)Session["LessonDedit2"], UserID, DataTypeID, (DateTime)Session["LessonDedit1"]);
                }
            }
        }
        LabelResponse.Text = "נשמר";
        LabelResponse.ForeColor = System.Drawing.Color.Green;
        done = "$('#success-modal').openModal();";//Showing message
    }

    protected void ListViewStudents_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        ((ListView)e.Item.FindControl("ListViewTypes")).DataSource = dtypes;
        ((ListView)e.Item.FindControl("ListViewTypes")).DataBind();
        foreach (ListViewItem dtp in ((ListView)e.Item.FindControl("ListViewTypes")).Items)
        {
            if (selected == null) break;
            CheckBox cb = (CheckBox)dtp.FindControl("CheckBoxItem");
            int DataTypeID = (int)((ListView)e.Item.FindControl("ListViewTypes")).DataKeys[dtp.DataItemIndex].Value;
            int UserID = (int)(ListViewStudents.DataKeys[e.Item.DataItemIndex].Value);
            if (selected.Where(x => x.DisciplinesId == DataTypeID && x.StudentId == UserID).Count() >= 1)
            {
                cb.Checked = true;
            }
        }
    }
}
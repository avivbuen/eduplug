using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_Teacher_Grade : System.Web.UI.Page
{
    protected int maxPrecent;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (MemberService.GetCurrent().Auth != MemberClearance.Teacher)
                Response.Redirect("~/Default.aspx");
            if (Request.QueryString["gid"] == null)
                Response.Redirect("~/Default.aspx");
            int id = int.Parse(Request.QueryString["gid"].ToString().Trim());
            FillInit(id);
        }
        catch(Exception ex) { Response.Redirect("~/Default.aspx"); }
    }
    protected void FillInit(int id)
    {
        try
        {
            if (Request.QueryString["gid"] == null)
                Response.Redirect("~/Default.aspx");
            tGrade current = tGradeService.Get(id);
            Session["tgCur"] = current;
            LabelTitle.Text = current.Name;
            if (current == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            maxPrecent = ExamService.PrecentLeft(current.ID);
            FillExams(current.ID);
            FillStudents(current.ID);
        }
        catch (Exception ex) { Response.Redirect("~/Default.aspx"); }
    }
    public void FillStudents(int tgid)
    {
        ListViewStudents.DataSource = tGradeService.GetStudents(tgid);
        ListViewStudents.DataBind();
        if (ListViewStudents.Items.Count>0)
        {
            LabelStudentsEmpty.Text = "";
        }
    }
    public void FillExams(int tgid)
    {
        ListViewExams.DataSource = ExamService.GetExamsByTgradeID(tgid);
        ListViewExams.DataBind();
        if (ListViewExams.Items.Count > 0)
        {
            LabelExamsEmpty.Text = "";
        }
    }

    protected void AddButtonExam_Click(object sender, EventArgs e)
    {
        if (Session["tgCur"] == null)
            Response.Redirect("~/Default.aspx");

        Page.Validate("ExamValidationGroup");
        if (Page.IsValid)
        {

            Exam exm = new Exam()
            {
                Title = Exam_Name.Text.Trim(),
                TeacherID = ((tGrade)Session["tgCur"]).TeacherID,
                Precent = int.Parse(Exam_Precent.Text),
                Date= DateTime.ParseExact(Exam_Date.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            };
            ExamService.Add(exm, ((tGrade)Session["tgCur"]).ID);
            Exam_Name.Text = "";
            Exam_Date.Text = "";
            Exam_Precent.Text = "";
            FillInit(((tGrade)Session["tgCur"]).ID);
        }
    }

    protected void ListViewExams_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        if (Session["tgCur"] == null)
            Response.Redirect("~/Default.aspx");
        int id = int.Parse(ListViewExams.DataKeys[e.ItemIndex]["ID"].ToString());
        ExamService.Delete(id);
        FillInit(((tGrade)Session["tgCur"]).ID);
    }

    protected void CancelButton_Click(object sender, EventArgs e)
    {
        if (Session["tgCur"] == null)
            Response.Redirect("~/Default.aspx");
        Exam_Name.Text = "";
        Exam_Date.Text = "";
        Exam_Precent.Text = "";
        FillInit(((tGrade)Session["tgCur"]).ID);
    }
}
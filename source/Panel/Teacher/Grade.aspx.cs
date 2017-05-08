using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Exams;
using Business_Logic.Grades;
using Business_Logic.Members;
using Business_Logic.Scores;
using Business_Logic.TeacherGrades;

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
        catch(Exception ex) { Response.Redirect("~/Default.aspx"); ex.HelpLink = "http://avivnet.com"; }
    }
    protected void FillInit(int id)
    {
        try
        {
            if (Request.QueryString["gid"] == null)
                Response.Redirect("~/Default.aspx");
            TeacherGrade current = TeacherGradeService.Get(id);
            Session["tgCur"] = current;
            LabelTitle.Text = current.Name;
            if (current == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            maxPrecent = ExamService.PrecentLeft(current.Id);
            FillExams(current.Id);
            FillStudents(current.Id);
        }
        catch (Exception ex) { Response.Redirect("~/Default.aspx"); ex.HelpLink = "http://avivnet.com"; }
    }
    public void FillStudents(int tgid)
    {
        ListViewStudents.DataSource = TeacherGradeService.GetStudents(tgid);
        ListViewStudents.DataBind();
        if (ListViewStudents.Items.Count>0)
        {
            LabelStudentsEmpty.Text = "";
        }
    }
    public void FillExams(int tgid)
    {
        ListViewExams.DataSource = ExamService.GetExamsByTeacherGradeId(tgid);
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
                TeacherId = ((TeacherGrade)Session["tgCur"]).TeacherId,
                Precent = int.Parse(Exam_Precent.Text),
                Date= DateTime.ParseExact(Exam_Date.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            };
            ExamService.Add(exm, ((TeacherGrade)Session["tgCur"]).Id);
            Thread.Sleep(100);
            ScoreService.ResetScores(ExamService.GetExamId(exm));
            Exam_Name.Text = "";
            Exam_Date.Text = "";
            Exam_Precent.Text = "";
            FillInit(((TeacherGrade)Session["tgCur"]).Id);
        }
    }

    protected void ListViewExams_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        if (Session["tgCur"] == null)
            Response.Redirect("~/Default.aspx");
        int id = int.Parse(ListViewExams.DataKeys[e.ItemIndex]["ID"].ToString());
        ExamService.Delete(id);
        FillInit(((TeacherGrade)Session["tgCur"]).Id);
    }

    protected void CancelButton_Click(object sender, EventArgs e)
    {
        if (Session["tgCur"] == null)
            Response.Redirect("~/Default.aspx");
        Exam_Name.Text = "";
        Exam_Date.Text = "";
        Exam_Precent.Text = "";
        FillInit(((TeacherGrade)Session["tgCur"]).Id);
    }
}
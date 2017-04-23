using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Exams;
using Business_Logic.Grades;
using Business_Logic.Members;
using Business_Logic.Scores;
using Business_Logic.TeacherGrades;

public partial class Panel_Teacher_GradeState : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Teacher)
            Response.Redirect("~/Default.aspx");
        if (Request.QueryString["gid"] == null)
            Response.Redirect("~/Default.aspx");
        int id = int.Parse(Request.QueryString["gid"].ToString().Trim());
        TeacherGrade current = TeacherGradeService.Get(id);
        if (current == null)
            Response.Redirect("~/Default.aspx");
        Session["tgCur12"] = current;
        if (!IsPostBack)
        {
            Fill(current.Id);
        }
    }
    /// <summary>
    /// Filling the data list
    /// </summary>
    /// <param name="tgid">Teacher Grade ID</param>
    protected void Fill(int tgid)
    {
        ListViewExamsA.DataSource = ExamService.GetExamsByTeacherGradeId(tgid, "a").OrderBy(x => x.Id);
        ListViewExamsA.DataBind();
        ListViewExamsB.DataSource = ExamService.GetExamsByTeacherGradeId(tgid, "b").OrderBy(x => x.Id);
        ListViewExamsB.DataBind();
        ListViewStudents.DataSource = TeacherGradeService.GetStudents(tgid);
        ListViewStudents.DataBind();
    }
    public List<Score> scores = null;
    public List<Score> sA = null;
    public List<Score> sB = null;
    protected void ListViewStudents_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (Session["tgCur12"] == null)
            Response.Redirect("~/");
        TeacherGrade tg = (TeacherGrade)Session["tgCur12"];
        if (scores == null)
            scores = ScoreService.GetAllGradeScores(tg.Id);
        sA = scores.Where(x => x.Exam.YearPart == "a").ToList();
        sB = scores.Where(x => x.Exam.YearPart == "b").ToList();
        ((ListView)e.Item.FindControl("ListViewScoresA")).DataSource = sA.Where(x => x.Student.UserID == (int)(ListViewStudents.DataKeys[e.Item.DataItemIndex].Value)).OrderBy(x => x.Exam.Id);
        ((ListView)e.Item.FindControl("ListViewScoresA")).DataBind();
        ((ListView)e.Item.FindControl("ListViewScoresB")).DataSource = sB.Where(x => x.Student.UserID == (int)(ListViewStudents.DataKeys[e.Item.DataItemIndex].Value)).OrderBy(x => x.Exam.Id);
        ((ListView)e.Item.FindControl("ListViewScoresB")).DataBind();
    }
    protected string CastScore(object score)
    {
        int scoreVal = (int)score;
        if (scoreVal == -1)
        {
            return "אין ציון";
        }
        return scoreVal.ToString();
    }
    protected string GetAVG(object userID)
    {
        if (((TeacherGrade)Session["tgCur12"]) == null)
            Response.Redirect("~/Default.aspx");
        int UID = (int)userID;
        return ScoreService.GetStudentAvg(UID, ((TeacherGrade)Session["tgCur12"]).Id).ToString();
    }
    protected string GetFAVG(object userID)
    {
        if (((TeacherGrade)Session["tgCur12"]) == null)
            Response.Redirect("~/Default.aspx");
        int UID = (int)userID;
        return ScoreService.GetStudentAvgFinal(UID, ((TeacherGrade)Session["tgCur12"]).Id).ToString();
    }
    protected string GetFAVG(object userID, string yearPart)
    {
        if (((TeacherGrade)Session["tgCur12"]) == null)
            Response.Redirect("~/Default.aspx");
        int UID = (int)userID;
        return ScoreService.GetStudentAvgFinal(UID, ((TeacherGrade)Session["tgCur12"]).Id, yearPart).ToString();
    }
}
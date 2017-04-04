using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_Teacher_GradeState : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Teacher)
            Response.Redirect("~/Default.aspx");
        if (Request.QueryString["gid"] == null)
            Response.Redirect("~/Default.aspx");
        int id = int.Parse(Request.QueryString["gid"].ToString().Trim());
        tGrade current = tGradeService.Get(id);
        if(current==null)
            Response.Redirect("~/Default.aspx");
        Session["tgCur12"] = current;
        if (!IsPostBack)
        {
            Fill(current.ID);
        }
    }
    /// <summary>
    /// Filling the data list
    /// </summary>
    /// <param name="tgid">Teacher Grade ID</param>
    protected void Fill(int tgid)
    {
        ListViewExamsA.DataSource = ExamService.GetExamsByTgradeID(tgid,"a").OrderBy(x=>x.ID);
        ListViewExamsA.DataBind();
        ListViewExamsB.DataSource = ExamService.GetExamsByTgradeID(tgid, "b").OrderBy(x => x.ID);
        ListViewExamsB.DataBind();
        ListViewStudents.DataSource = tGradeService.GetStudents(tgid);
        ListViewStudents.DataBind();
    }

    protected void ListViewStudents_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (((tGrade)Session["tgCur12"]) == null)
            Response.Redirect("~/Default.aspx");
        ((ListView)e.Item.FindControl("ListViewScoresA")).DataSource = ScoreService.GetAllStudentWE((int)(ListViewStudents.DataKeys[e.Item.DataItemIndex].Value),((tGrade)Session["tgCur12"]).ID,"a").OrderBy(x=>x.Exam.ID);
        ((ListView)e.Item.FindControl("ListViewScoresA")).DataBind();
        ((ListView)e.Item.FindControl("ListViewScoresB")).DataSource = ScoreService.GetAllStudentWE((int)(ListViewStudents.DataKeys[e.Item.DataItemIndex].Value), ((tGrade)Session["tgCur12"]).ID, "b").OrderBy(x => x.Exam.ID);
        ((ListView)e.Item.FindControl("ListViewScoresB")).DataBind();
    }
    protected string CastScore(object score)
    {
        int scoreVal = (int)score;
        if (scoreVal==-1)
        {
            return "אין ציון";
        }
        return scoreVal.ToString();
    }
    protected string GetAVG(object userID)
    {
        if (((tGrade)Session["tgCur12"]) == null)
            Response.Redirect("~/Default.aspx");
        int UID = (int)userID;
        return ScoreService.GetStudentAvg(UID, ((tGrade)Session["tgCur12"]).ID).ToString();
    }
    protected string GetFAVG(object userID)
    {
        if (((tGrade)Session["tgCur12"]) == null)
            Response.Redirect("~/Default.aspx");
        int UID = (int)userID;
        return ScoreService.GetStudentAvgFinal(UID, ((tGrade)Session["tgCur12"]).ID).ToString();
    }
    protected string GetFAVG(object userID,string yearPart)
    {
        if (((tGrade)Session["tgCur12"]) == null)
            Response.Redirect("~/Default.aspx");
        int UID = (int)userID;
        return ScoreService.GetStudentAvgFinal(UID, ((tGrade)Session["tgCur12"]).ID,yearPart).ToString();
    }
}
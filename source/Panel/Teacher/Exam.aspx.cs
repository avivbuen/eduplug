using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_Teacher_Exam : System.Web.UI.Page
{
    protected int maxPrecent;
    protected string script;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (MemberService.GetCurrent().Auth != MemberClearance.Teacher)
                Response.Redirect("~/Default.aspx");
            if (Request.QueryString["eid"] == null)
                Response.Redirect("~/Default.aspx");
            int id = int.Parse(Request.QueryString["eid"].ToString().Trim());
            Exam exm = ExamService.GetExam(id);
            Session["curExamScoring"] = exm;
            if (!IsPostBack)
            {
                Fill(exm.TeacherGradeID);
                Exam_Name.Text = exm.Title;
                Exam_Date.Text = exm.Date.ToString("dd/MM/yyyy");
                Exam_Precent.Text = exm.Precent.ToString();
            }
            script = "";
            maxPrecent = ExamService.PrecentLeft(exm.TeacherGradeID) + exm.Precent;
        }
        catch (Exception ex) { Response.Redirect("~/Default.aspx"); }
    }
    protected void Fill(int eid)
    {
        DataListScores.DataSource = tGradeService.GetStudents(eid);
        DataListScores.DataBind();
    }
    protected void DataListScores_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (Session["curExamScoring"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        int sid = int.Parse(DataListScores.DataKeys[e.Item.ItemIndex].ToString());
        int eid = ((Exam)Session["curExamScoring"]).ID;
        Score scr = ScoreService.GetScore(sid, eid);
        if (scr == null||scr.ScoreVal==-1)
        {
            ((TextBox)e.Item.FindControl("TextBoxScoreVal")).Text = "";
        }
        else
        {
            ((TextBox)e.Item.FindControl("TextBoxScoreVal")).Text = scr.ScoreVal.ToString();
        }
    }

    protected void SaveButton_Click(object sender, EventArgs e)
    {
        if (Session["curExamScoring"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        Page.Validate("ExamValidationGroup");
        if (Page.IsValid)
        {
            Exam exmn = new Exam()
            {
                ID = ((Exam)Session["curExamScoring"]).ID,
                Title = Exam_Name.Text,
                Date = DateTime.ParseExact(Exam_Date.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                Precent = int.Parse(Exam_Precent.Text)
            };
            ExamService.Update(exmn);
            ScoreService.ResetScores(((Exam)Session["curExamScoring"]).ID);
            maxPrecent = ExamService.PrecentLeft(((Exam)Session["curExamScoring"]).TeacherGradeID) + exmn.Precent;
            int count = 0;
            foreach (DataListItem item in DataListScores.Items)
            {
                TextBox tbScore = (TextBox)item.FindControl("TextBoxScoreVal");
                if (tbScore.Text.Trim() != "")
                {
                    try
                    {
                        int val = int.Parse(tbScore.Text);
                        if (val > 100 || val < 0)
                        {
                            count++;
                            continue;
                        }
                        Score score = new Score()
                        {
                            ScoreVal = val,
                            Student = new Member() { UserID = int.Parse(DataListScores.DataKeys[item.ItemIndex].ToString()) },
                            Exam = new Exam() { ID = ((Exam)Session["curExamScoring"]).ID }
                        };
                        ScoreService.Add(score);
                    }
                    catch { count++; }
                }
            }
            Fill(((Exam)Session["curExamScoring"]).TeacherGradeID);
            LabelSaved.Text = "נשמר";
            script = "alert('נשמר')";
            if (count != 0)
                LabelSaved.Text = "נשמר פרט ל" + count + " ציונים ";
        }
    }

    protected void cv_Exam_Precent_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if(Session["curExamScoring"]==null)Response.Redirect("~/");
        Exam e = (Exam)Session["curExamScoring"];
        DateTime date =DateTime.ParseExact(Exam_Date.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        maxPrecent=ExamService.PrecentLeft(e.TeacherGradeID,EduSysDate.GetYearPart(date))+e.Precent;
        args.IsValid = (int.Parse(args.Value) <= maxPrecent && int.Parse(args.Value) >= 0);
        LabelLeft.Text = "האחוזים שנותרו לידעתך למחצית זו  - " + (maxPrecent + "%") + " (מבלי מבחן זה)";
    }
}
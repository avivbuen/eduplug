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
                Fill(exm.TeacherGradeId);
                ChartOfStats.DataSource = GetDataChart(exm.Id);
                ChartOfStats.Title = "כמות נכשלים/ עוברים מתוך רשומות שהוזנו";
                ChartOfStats.DataBind();
                Exam_Name.Text = exm.Title;
                Exam_Date.Text = exm.Date.ToString("dd/MM/yyyy");
                Exam_Precent.Text = exm.Precent.ToString();
            }
            script = "";
            maxPrecent = ExamService.PrecentLeft(exm.TeacherGradeId) + exm.Precent;
        }
        catch (Exception ex) { Response.Redirect("~/Default.aspx"); ex.HelpLink = "http://avivnet.com"; }
    }
    protected void Fill(int tgid)
    {
        DataListScores.DataSource = TeacherGradeService.GetStudents(tgid);
        DataListScores.DataBind();
        

    }
    protected Dictionary<string, object> GetDataChart(int eid)
    {
        var lsScores = ScoreService.GetAllExam(eid);
        ChartOfStats.Visible = (lsScores.Count != 0);
        var data = new Dictionary<string, object>();
        data.Add("עוברים", 0);
        data.Add("נכשל", 0);
        foreach (Score scr in lsScores)
        {
            if(scr.ScoreVal>55)
            {
                data["עוברים"] = (int)data["עוברים"]+1;
                continue;
            }
            data["נכשל"] = (int)data["נכשל"] + 1;
        }
        return data;
    }
    protected void DataListScores_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (Session["curExamScoring"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        int sid = int.Parse(DataListScores.DataKeys[e.Item.ItemIndex].ToString());
        int eid = ((Exam)Session["curExamScoring"]).Id;
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
                Id = ((Exam)Session["curExamScoring"]).Id,
                Title = Exam_Name.Text,
                Date = DateTime.ParseExact(Exam_Date.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                Precent = int.Parse(Exam_Precent.Text)
            };
            ExamService.Update(exmn);
            ScoreService.ResetScores(((Exam)Session["curExamScoring"]).Id);
            maxPrecent = ExamService.PrecentLeft(((Exam)Session["curExamScoring"]).TeacherGradeId) + exmn.Precent;
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
                            Exam = new Exam() { Id = ((Exam)Session["curExamScoring"]).Id }
                        };
                        ScoreService.Add(score);
                    }
                    catch { count++; }
                }
            }
            Fill(((Exam)Session["curExamScoring"]).TeacherGradeId);
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
        maxPrecent=ExamService.PrecentLeft(e.TeacherGradeId,EduSysDate.GetYearPart(date))+e.Precent;
        args.IsValid = (int.Parse(args.Value) <= maxPrecent && int.Parse(args.Value) >= 0);
        LabelLeft.Text = "האחוזים שנותרו לידעתך למחצית זו  - " + (maxPrecent + "%") + " (מבלי מבחן זה)";
    }
}
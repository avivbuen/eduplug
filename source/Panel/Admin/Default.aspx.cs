using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Exams;
using Business_Logic.Members;
using Business_Logic.Scores;
using Business_Logic.TeacherGrades;

public partial class InterTrack_Admin_Default : System.Web.UI.Page
{
    /// <summary>
    /// Page load event - Fires when page loads
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Admin)
            Response.Redirect("~/");
        if (!IsPostBack)
            Fill();
    }
    /// <summary>
    /// Fill method - used to fill up controls from the data 
    /// </summary>
    protected void Fill()
    {
        var tgrades = TeacherGradeService.GetAll();
        var scores = tgrades.Select(tg => new Score()
            {
                Exam = new Exam()
                {
                    Id = -1,
                    Precent = -1,
                    Title = tg.Name
                },
                ScoreVal = (int) ScoreService.GetAvgTgrade(tg.Id)
            })
            .ToList();
        ListViewScores.DataSource = scores;
        ListViewScores.DataBind();
    }
    protected string CastVal(object val)
    {
        return val.ToString() == "0" ? "לא נקבע" : val.ToString();
    }
}
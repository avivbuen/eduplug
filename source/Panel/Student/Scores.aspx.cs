using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Members;
using Business_Logic.Scores;

public partial class Track_Scores : System.Web.UI.Page
{
    public bool admin;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Student)
            Response.Redirect("~/");
        switch (MemberService.GetCurrent().Auth)
        {
            case MemberClearance.Student:
                DataTable dt = ScoreService.GetAllGrade(MemberService.GetCurrent().UserID);
                if (dt.Rows.Count==0)
                {
                    LabelError.Text = "טרם הוזנו לך ציונים";
                }
                else
                {
                    DataListScores.DataSource = dt;
                    DataListScores.DataBind();
                    LabelError.Text = "";
                }
                break;
            default:
                Response.Redirect("~/Default.aspx");
                break;
        }
    }
    public string GetScoreIcon(string ev)
    {
        int grade = int.Parse(ev);
        if (grade < 30)
        {
            return "sentiment_very_dissatisfied";
        }
        else if (grade < 56)
        {
            return "mood_bad";
        }
        else if (grade < 70)
        {
            return "sentiment_neutral";
        }
        else if (grade < 85)
        {
            return "sentiment_satisfied";
        }
        else if (grade < 90)
        {
            return "mood";
        }
        else
        {
            return "sentiment_very_satisfied";
        }
    }
}
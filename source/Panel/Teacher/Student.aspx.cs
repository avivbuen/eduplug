using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Disciplines;
using Business_Logic.Members;
using Business_Logic.Scores;

public partial class Panel_Teacher_Student : System.Web.UI.Page
{
    public string idSTD;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (MemberService.GetCurrent().Auth != MemberClearance.Teacher)
                Response.Redirect("~/Default.aspx");
            if (Request.QueryString["sid"] == null)
                Response.Redirect("~/Default.aspx");
            int id = int.Parse(Request.QueryString["sid"].ToString().Trim());
            int tgid = -1;
            if (Request.QueryString["tgid"] != null)
            {
                tgid = int.Parse(Request.QueryString["tgid"].ToString().Trim());
                GridViewDiscplines.DataSource = DisciplinesServices.GetStudent(id, tgid);
            }
            else
            {
                GridViewDiscplines.DataSource = DisciplinesServices.GetStudent(id);
            }

            idSTD = MemberService.GetUserPart(id).ID;
            
            GridViewDiscplines.DataBind();
            if (GridViewDiscplines.Rows.Count == 0)
                LiteralEmptyDiscplines.Text = "אין הערות משמעת";
            else
                LiteralEmptyDiscplines.Text = "";

            GridViewScores.DataSource = ScoreService.GetAllStudent(id);
            GridViewScores.DataBind();

            if (GridViewScores.Rows.Count == 0)
                LiteralEmptyScores.Text = "אין ציונים";
            else
                LiteralEmptyScores.Text = "";
            Member student = MemberService.GetUser(id);
            LabelName.Text = student.Name;
            LabelID.Text = student.ID;

        }
        catch (Exception ex) { Response.Redirect("~/Default.aspx"); ex.HelpLink = "http://avivnet.com"; }
    }
}
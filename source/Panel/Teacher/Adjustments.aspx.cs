using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Members;

public partial class Panel_Teacher_Adjustments : System.Web.UI.Page
{
    protected string script;
    public string idSTD;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Teacher)
            Response.Redirect("~/Default.aspx");
        try
        {
            if (Request.QueryString["uid"] == null || Request.QueryString["uid"].ToString().Trim() == "")
                Response.Redirect("~/Default.aspx");
            Member mem = MemberService.GetUser(int.Parse(Request.QueryString["uid"].ToString().Trim()));
            if(mem==null)
                Response.Redirect("~/Default.aspx");
            if (mem.Auth != MemberClearance.Student)
            {
                script = "alert('זה לא תלמיד. אתה מועבר חזרה. רק לתלמידים יש התאמות');location='Members.aspx'";//Showing message;
                return;
            }
            idSTD = mem.ID;
            Fill(mem.ID);
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Default.aspx");
            ex.HelpLink = "http://avivnet.com";
        }

        
    }
    protected void Fill(string id)
    {
        AdjustmentWS.EduAdjustmentsService ws = new AdjustmentWS.EduAdjustmentsService();
        GridViewAdjustment.DataSource = ws.GetAdjustmentsStudent(id);
        GridViewAdjustment.DataBind();
        if (GridViewAdjustment.Rows.Count == 0)
            LiteralEmptyAdjust.Text = "אין לתלמיד התאמות";
        else
            LiteralEmptyAdjust.Text = "";
    }
}
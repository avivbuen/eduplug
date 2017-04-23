using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Members;

public partial class Panel_Admin_Adjustments : System.Web.UI.Page
{
    protected string script;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Admin)
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
                
            link.HRef = "~/Panel/Admin/AddAdjustment.aspx?uid="+mem.UserID;
            Fill(mem.ID);
        }
        catch 
        {
            Response.Redirect("~/Default.aspx");
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
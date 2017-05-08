using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Members;

public partial class Panel_Student_Adjustments : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Student && MemberService.GetCurrent().Auth != MemberClearance.Parent)
            Response.Redirect("~/");
        Fill();
    }
    protected void Fill()
    {
        try
        {
            AdjustmentWS.EduAdjustmentsService ws = new AdjustmentWS.EduAdjustmentsService();
            if (MemberService.GetCurrent().Auth == MemberClearance.Student)
                GridViewAdjustment.DataSource = ws.GetAdjustmentsStudent(MemberService.GetCurrent().ID);
            else
                GridViewAdjustment.DataSource = ws.GetAdjustmentsStudent(MemberService.GetSelectedChild().ID);
            GridViewAdjustment.DataBind();
            if (GridViewAdjustment.Rows.Count == 0)
                LiteralEmptyAdjust.Text = "אין לך התאמות";
            else
                LiteralEmptyAdjust.Text = "";
        }
        catch
        {
            Response.Redirect("~/Default.aspx");
        }

    }
}
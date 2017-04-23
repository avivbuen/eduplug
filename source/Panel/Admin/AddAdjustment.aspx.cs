using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Members;

public partial class Panel_Admin_AddAdjustment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Admin)
            Response.Redirect("~/Default.aspx");
        if (Request.QueryString["uid"] == null || Request.QueryString["uid"].ToString().Trim()=="")
            Response.Redirect("~/Default.aspx");
        try
        {
            int uid = int.Parse(Request.QueryString["uid"].ToString().Trim());
            Member mem = MemberService.GetUserPart(uid);
            Session["emmAdj"] = mem;
            if (mem == null) Response.Redirect("~/Default.aspx");
            User_ID_Name.Text = mem.Name + " - ת.ז. " + mem.ID;
            if (!IsPostBack)
                Fill();
        }
        catch
        {
            Response.Redirect("~/Default.aspx");
        }

    }
    protected void Fill()
    {
        AdjustmentWS.EduAdjustmentsService ws = new AdjustmentWS.EduAdjustmentsService();
        User_Adjustment.DataSource = ws.GetAdjustmentsTypes();
        User_Adjustment.DataValueField = "ID";
        User_Adjustment.DataTextField = "Name";
        User_Adjustment.DataBind();
    }

    protected void AddButton_Click(object sender, EventArgs e)
    {
            AdjustmentWS.EduAdjustmentsService ws = new AdjustmentWS.EduAdjustmentsService();
            if(Session["emmAdj"] == null) Response.Redirect("~/Default.aspx");
            Member mem = (Member)Session["emmAdj"];
            ws.AddStudent(mem.FirstName, mem.LastName, mem.ID, User_Adjustment.SelectedValue);
            Response.Redirect("~/Panel/Admin/Adjustments.aspx?uid=" + mem.UserID);
        
    }
}
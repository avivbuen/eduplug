using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Members;

public partial class Panel_Admin_EduSense : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Admin)
            Response.Redirect("~/");
        if (!IsPostBack)
            Fill();
        //Adds THEAD and TBODY to GridView.
    }
    protected void Fill()
    {
        GridViewSense.DataSource = Intel.GetEduSense();
        GridViewSense.DataBind();
        if (GridViewSense.Rows.Count == 0)
            LabelEmpty.Text = "אין כניסות";
        else
            LabelEmpty.Text = "";
        EnterChart.DataSource = Intel.GetLocations();
    }
    protected void GridViewSense_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (e.NewPageIndex < 0 || e.NewPageIndex > GridViewSense.PageCount) return;
        GridViewSense.PageIndex = e.NewPageIndex;
        Fill();
    }
    public string Rev(object obj)
    {
        try
        {
            bool objcasted = (bool)obj;
            if (objcasted)
            {
                return "green";
            }
            return "red";
        }
        catch
        {
            return "red";
        }

    }
    protected void GridViewSense_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditT")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewSense.Rows[index];
            Intel.MarkRev(row.Cells[2].Text, true);
            Fill();
            //Response.Redirect("~/Panel/Admin/EditSense.aspx?ip=" + row.Cells[2].Text);
        }
        if (e.CommandName == "DeleteT")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewSense.Rows[index];
            string ip = row.Cells[2].Text;
            Intel.DeleteVisit(ip);
            Fill();
            Response.Redirect("Edusense.aspx");
        }
    }

    protected void GridViewSense_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string done = GridViewSense.DataKeys[e.Row.RowIndex].Value.ToString();
            LinkButton ln = (LinkButton)e.Row.FindControl("LinkButtonDel");
            bool d = bool.Parse(done);
            if (!d)
                ln.Attributes.Add("style", "color:red");
            else
                ln.Attributes.Add("style", "color:green");
        }

    }

}
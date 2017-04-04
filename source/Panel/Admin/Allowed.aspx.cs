﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_Admin_Allowed : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Admin)
            Response.Redirect("~/Default.aspx");
        if (!IsPostBack)
            Fill();
        //Adds THEAD and TBODY to GridView.
        
    }
    /// <summary>
    /// Fills the gridview with data from the DB
    /// </summary>
    protected void Fill()
    {
        DataTable dt = MemberService.GetAllowed();
        ListViewUsers.DataSource = dt;
        ListViewUsers.DataBind();
        if (dt.Rows.Count == 0)
            LabelEmpty.Text = "אין מורשים";
        else
            LabelEmpty.Text = "";
    }
    protected void ListViewUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (e.NewPageIndex < 0 || e.NewPageIndex > ListViewUsers.PageCount) return;
        ListViewUsers.PageIndex = e.NewPageIndex;
        Fill();
    }
    protected string GetYesNo(bool o)
    {
        if (o) return "כן";
        return "לא";
    }
    protected void ListViewUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditT")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = ListViewUsers.Rows[index];
            Response.Redirect("~/Panel/Admin/EditAllowed.aspx?uid=" + row.Cells[1].Text);
        }
        if (e.CommandName == "DeleteT")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = ListViewUsers.Rows[index];
            MemberService.RemoveFromAllowed(row.Cells[1].Text);
            Fill();
        }
    }
}
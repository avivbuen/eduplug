using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_Admin_Tgrades : System.Web.UI.Page
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
        GridViewTgrades.DataSource = tGradeService.GetAll().OrderBy(x=>x.TeacherID).ToList();
        GridViewTgrades.DataBind();
        if (GridViewTgrades.Rows.Count == 0)
            LabelEmpty.Text = "אין כיתות";
        else
            LabelEmpty.Text = "";
    }
    protected void GridViewTgrades_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (e.NewPageIndex < 0 || e.NewPageIndex > GridViewTgrades.PageCount) return;
        GridViewTgrades.PageIndex = e.NewPageIndex;
        Fill();
    }
    protected void GridViewTgrades_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditT")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewTgrades.Rows[index];
            Response.Redirect("~/Panel/Admin/EditTgrade.aspx?tgid=" + int.Parse(GridViewTgrades.DataKeys[row.RowIndex].Value.ToString()));
        }
        if (e.CommandName == "ChangeT")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewTgrades.Rows[index];
            Response.Redirect("~/Panel/Admin/ChangeTable.aspx?tgid=" + int.Parse(GridViewTgrades.DataKeys[row.RowIndex].Value.ToString()));
        }
        if (e.CommandName == "LessonEdit")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewTgrades.Rows[index];
            Response.Redirect("~/Panel/Admin/Lessons.aspx?tgid=" + int.Parse(GridViewTgrades.DataKeys[row.RowIndex].Value.ToString()));
        }
        if (e.CommandName == "DeleteT")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewTgrades.Rows[index];
            tGradeService.Remove(int.Parse(GridViewTgrades.DataKeys[row.RowIndex].Value.ToString()));
            Fill();
        }
    }

    protected void GridViewTgrades_DataBinding(object sender, EventArgs e)
    {
        //GridViewTgrades.HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        GridViewTgrades.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
}
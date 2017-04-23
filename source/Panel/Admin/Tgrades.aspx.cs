using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Grades;
using Business_Logic.Members;
using Business_Logic.TeacherGrades;

public partial class Panel_Admin_TeacherGrades : System.Web.UI.Page
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
        GridViewTeacherGrades.DataSource = TeacherGradeService.GetAll().OrderBy(x=>x.TeacherId).ToList();
        GridViewTeacherGrades.DataBind();
        if (GridViewTeacherGrades.Rows.Count == 0)
            LabelEmpty.Text = "אין כיתות";
        else
            LabelEmpty.Text = "";
    }
    protected void GridViewTeacherGrades_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (e.NewPageIndex < 0 || e.NewPageIndex > GridViewTeacherGrades.PageCount) return;
        GridViewTeacherGrades.PageIndex = e.NewPageIndex;
        Fill();
    }
    protected void GridViewTeacherGrades_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditT")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewTeacherGrades.Rows[index];
            Response.Redirect("~/Panel/Admin/EditTeacherGrade.aspx?tgid=" + int.Parse(GridViewTeacherGrades.DataKeys[row.RowIndex].Value.ToString()));
        }
        if (e.CommandName == "ChangeT")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewTeacherGrades.Rows[index];
            Response.Redirect("~/Panel/Admin/ChangeTable.aspx?tgid=" + int.Parse(GridViewTeacherGrades.DataKeys[row.RowIndex].Value.ToString()));
        }
        if (e.CommandName == "LessonEdit")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewTeacherGrades.Rows[index];
            Response.Redirect("~/Panel/Admin/Lessons.aspx?tgid=" + int.Parse(GridViewTeacherGrades.DataKeys[row.RowIndex].Value.ToString()));
        }
        if (e.CommandName == "DeleteT")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewTeacherGrades.Rows[index];
            TeacherGradeService.Remove(int.Parse(GridViewTeacherGrades.DataKeys[row.RowIndex].Value.ToString()));
            Fill();
        }
    }

    protected void GridViewTeacherGrades_DataBinding(object sender, EventArgs e)
    {
        //GridViewTeacherGrades.HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        GridViewTeacherGrades.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
}
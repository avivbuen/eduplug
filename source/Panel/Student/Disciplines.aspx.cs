using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Disciplines;
using Business_Logic.Members;

public partial class Panel_Student_Disciplines : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Student)
            Response.Redirect("~/");
        GridViewDiscplines.DataSource = DisciplinesServices.GetStudent(MemberService.GetCurrent().UserID);
        GridViewDiscplines.DataBind();
    }

    protected void GridViewDiscplines_DataBound(object sender, EventArgs e)
    {
        GridViewDiscplines.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
}
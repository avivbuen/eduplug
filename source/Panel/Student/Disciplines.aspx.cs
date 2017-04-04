using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_Student_Disciplines : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Student)
            Response.Redirect("~/");
        GridViewDiscplines.DataSource = DisciplinesServices.GetStudent(MemberService.GetCurrent().UserID);
        GridViewDiscplines.DataBind();
    }
}
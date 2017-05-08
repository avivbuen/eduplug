using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Members;

public partial class Panel_SetChild : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth!=MemberClearance.Parent)
        {
            Response.Redirect("~/");
        }
        if(Request.QueryString["cid"]==null)
            Response.Redirect("~/");
        int id;
        if (!int.TryParse(Request.QueryString["cid"].ToString(),out id))
        {
            Response.Redirect("~/");
        }
        MemberService.SetSelectedChild(id);
        Response.Redirect("~/");
    }
}
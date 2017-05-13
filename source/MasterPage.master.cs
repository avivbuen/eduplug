using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic.Members;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(MemberService.GetCurrent().Active=="Wait")
            Response.Redirect("~/User/Activate.aspx?uid="+MemberService.GetCurrent().UserID,false);
    }
}

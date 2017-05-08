using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Business_Logic;
using Business_Logic.Members;

public partial class Controls_SideNav : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PanelParrent.Visible = (MemberService.GetCurrent().Auth == MemberClearance.Parent);
    }
}
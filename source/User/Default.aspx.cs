using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Cities;
using Business_Logic.Members;

public partial class User_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (MemberService.GetCurrent().Auth != MemberClearance.Guest)
        {
            if(MemberService.GetCurrent().Active=="Wait")
                MemberService.Logout();
            Response.Redirect("~/Default.aspx",false);
        }
        User_ID.Attributes.Add("onkeypress", "return event.charCode >= 48 && event.charCode <= 57");
        CitiesService.UpdateCities();

    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Cities;
using Business_Logic.Majors;
using Business_Logic.Members;

public partial class Panel_Admin_Import : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(MemberService.GetCurrent().Auth!=MemberClearance.Admin)
            Response.Redirect("~/");
        if (!IsPostBack)
            Fill();
    }

    protected void Fill()
    {
        DataTable dt = MajorsService.GetAllDataTable();
        dt.Columns.Add("FullName", typeof(string), "eduMajorID + ' - ' + eduMajor");
        ListBoxMajors.DataSource = dt;
        ListBoxMajors.DataTextField = "FullName";
        ListBoxMajors.DataBind();
        DataTable dt1 = CitiesService.GetAllDataTable();
        dt1.Columns.Add("FullName", typeof(string), "eduCityID + ' - ' + eduCity");
        ListBoxCity.DataSource = dt1;
        ListBoxCity.DataTextField = "FullName";
        ListBoxCity.DataBind();
    }

}
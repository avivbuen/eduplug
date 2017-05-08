using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Members;

public partial class Panel_Admin_EditAllowed : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Admin)
            Response.Redirect("~/");
        if (!IsPostBack) Fill();
    }
    protected void Fill()
    {
        if(Request.QueryString["uid"]==null||Request.QueryString["uid"].ToString().Trim()=="")Response.Redirect("~/");
        string s = Request.QueryString["uid"].ToString().Trim();
        Member mem = MemberService.GetAllowed(s);
        User_First_Name.Text = mem.FirstName;
        User_Last_Name.Text = mem.LastName;
        User_ID.Text = mem.ID;
        Session["memAll9004"] = mem;

        User_Section.SelectedValue = ((char)mem.Auth).ToString();
    }
    protected void SaveButton_Click(object sender, EventArgs e)
    {
        Page.Validate();
        if (Page.IsValid)
        {
            //MemberService.RemoveFromAllowed(User_ID.Text.Trim());
            Member m = new Member()
            {
                FirstName = User_First_Name.Text,
                LastName = User_Last_Name.Text,
                ID = User_ID.Text,
                Auth = ((MemberClearance)User_Section.SelectedValue[0])
            };

            MemberService.AddAllowed(m);
            Response.Redirect("~/Panel/Admin/Allowed.aspx");
        }
    }
    protected void cve_User_ID_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Session["memAll9004"] == null) Response.Redirect("~/");
        Member mem = (Member)Session["memAll9004"];

        if (mem.ID!=User_ID.Text&&MemberService.ExsitsAllowed(User_ID.Text))
        {
            args.IsValid = false;
            return;
        }
        args.IsValid = true;
    }
    protected void cv_User_ID_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            if (!CheckIDNo(User_ID.Text))
            {
                args.IsValid = false;
                return;
            }
            args.IsValid = true;
        }
        catch
        {
            args.IsValid = false;
        }

    }
    static bool CheckIDNo(string strID)
    {
        int[] id_12_digits = { 1, 2, 1, 2, 1, 2, 1, 2, 1 };
        int count = 0;

        if (strID == null)
            return false;

        strID = strID.PadLeft(9, '0');

        for (int i = 0; i < 9; i++)
        {
            int num = int.Parse(strID.Substring(i, 1)) * id_12_digits[i];

            if (num > 9)
                num = (num / 10) + (num % 10);

            count += num;
        }

        return (count % 10 == 0);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Tools_AddAllow : System.Web.UI.Page
{
    public string script;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Admin)
            Response.Redirect("~/Default.aspx");
    }
    protected void AddButton_Click(object sender, EventArgs e)
    {
        Page.Validate("AllowValidationGroup");
        if (Page.IsValid)
        {
            Member m = new Member()
            {
                FirstName = User_First_Name.Text,
                LastName = User_Last_Name.Text,
                ID = User_ID.Text,
                Auth = ((MemberClearance)User_Section.SelectedValue[0])
            };
            MemberService.AddAllowed(m);
            script = "alert('נוסף!');location='Allowed.aspx';";//Showing message;
        }
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
    protected void cve_User_ID_ServerValidate(object source, ServerValidateEventArgs args)
    {
        
        if (MemberService.ExsitsAllowed(User_ID.Text))
        {
            args.IsValid = false;
            return;
        }
        args.IsValid = true;
    }
}
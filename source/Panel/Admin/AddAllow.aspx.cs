using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Cities;
using Business_Logic.Grades;
using Business_Logic.Members;

public partial class Admin_Tools_AddAllow : System.Web.UI.Page
{
    public string script;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Admin)
            Response.Redirect("~/Default.aspx");
        if (!IsPostBack)
            Fill();
    }

    protected void Fill()
    {
        Fill(User_City,"בחר עיר",CitiesService.GetAll(), "eduCity", "eduCityID");
        Fill(User_Section, "בחר/י כיתה/אזור", GradesService.GetAll(), "eduGradeName", "eduGradeID");

    }
    protected void Fill<T>(DropDownList drpdwnLST, string placeholder, List<T> data, string TextField, string ValueField)
    {
        drpdwnLST.Items.Clear();
        drpdwnLST.Items.Add(new ListItem(placeholder, "-1"));
        foreach (object c in data)
        {
            if (c is City)
            {
                City city = ((City)c);
                drpdwnLST.Items.Add(new ListItem(city.Name, city.Id.ToString()));
            }
            if (c is Grade)
            {
                Grade grade = ((Grade)c);
                drpdwnLST.Items.Add(new ListItem(grade.Name, grade.Id.ToString()));
            }
        }
    }
    protected void AddButton_Click(object sender, EventArgs e)
    {
        Page.Validate("AllowValidationGroup");
        if (Page.IsValid)
        {/*
            Member m = new Member()
            {
                FirstName = User_First_Name.Text,
                LastName = User_Last_Name.Text,
                ID = User_ID.Text,
                Auth = ((MemberClearance)User_Type.SelectedValue[0]),
                Gender = ((MemberGender)(char.Parse(User_Gender.SelectedValue.Trim()))),
                BornDate = DateTime.ParseExact(User_BornDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                GradeID = int.Parse(User_Section.SelectedValue.Trim()),
                City = CitiesService.GetCity(int.Parse(User_City.SelectedValue.Trim())),
                Majors = _Majors
            };
            MemberService.AddAllowed(m);*/
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

    protected void cv_User_Majors_OnServerValidate(object source, ServerValidateEventArgs args)
    {
        throw new NotImplementedException();
    }
}
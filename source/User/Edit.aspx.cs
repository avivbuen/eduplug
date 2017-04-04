using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_Edit : System.Web.UI.Page
{
    public string picPath = "";
    public string done = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (MemberService.GetCurrent().Auth == MemberClearance.Guest)
                Response.Redirect("~/Default.aspx");
            Member mem = MemberService.GetCurrent();
            if (Request.QueryString["uid"] != null && Request.QueryString["uid"].ToString().Trim() != "" && MemberService.GetCurrent().Auth == MemberClearance.Admin)
                picPath = MemberService.GetUser(int.Parse(Request.QueryString["uid"].ToString().Trim())).PicturePath;
            else
                picPath = MemberService.GetCurrent().PicturePath;
            if (!IsPostBack)
            {
                CitiesService.UpdateCities();
                FillAreas();
                if (Request.QueryString["uid"] != null && Request.QueryString["uid"].ToString().Trim() != "" && MemberService.GetCurrent().Auth == MemberClearance.Admin)
                    UpdateFill(Request.QueryString["uid"].ToString().Trim());
                else
                    UpdateFill(MemberService.GetCurrent().UserID.ToString());
                cmv_User_BornDate.ValueToCompare = DateTime.Today.AddYears(-11).ToShortDateString();
                cmv_User_BornDate_g.ValueToCompare = DateTime.Today.AddYears(-120).ToShortDateString();
            }
        }
        catch
        {
            Response.Redirect("~/");
        }
    }
    protected void FillAreas()
    {
        Fill(User_City, "בחר/י עיר", CitiesService.GetAll(), "nhsCity", "nhsCityID");
        Fill(User_Section, "בחר/י כיתה/אזור", GradesService.GetAll(), "nhsGradeName", "nhsGradeID");
        List<Major> mjrs = MajorsService.GetAll();
        User_Majors.DataSource = mjrs;
        User_Majors.DataTextField = "Title";
        User_Majors.DataValueField = "ID";
        User_Majors.DataBind();
    }
    protected void UpdateFill(string uid)
    {
        Session["uuid"] = uid;//For updating later
        Member mem = MemberService.GetUser(int.Parse(uid));
        if (mem == null) Response.Redirect("~/Default.aspx");
        picPath = mem.PicturePath;
        User_First_Name.Text = mem.FirstName;
        User_Last_Name.Text = mem.LastName;
        User_Email.Text = mem.Mail;
        User_ID.Text = mem.ID;
        User_Section.SelectedValue = mem.GradeID.ToString();
        User_City.SelectedValue = mem.City.ID.ToString();
        User_Gender.SelectedValue = ((char)mem.Gender).ToString();
        User_Password.Attributes.Add("value", "!@#$%^&*(8)(&%@#&$*(");
        User_Password_c.Attributes.Add("value", "!@#$%^&*(8)(&%@#&$*(");
        User_BornDate.Text = mem.BornDate.ToString("dd/MM/yyyy").Trim();
        string select = "";
        User_Majors.ClearSelection();
        Major[] mjrs = MajorsService.GetUserMajors(mem.UserID).ToArray();
        for (int i = 0; i < mjrs.Length; i++)
        {
            if (mjrs.Length - 1 != i)
            {
                select += mjrs[i].ID + ",";
            }
            else
            {
                select += mjrs[i].ID;
            }
        }
        SetSelectedInCheckBox(User_Majors, select);
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
                drpdwnLST.Items.Add(new ListItem(city.Name, city.ID.ToString()));
            }
            if (c is Grade)
            {
                Grade grade = ((Grade)c);
                drpdwnLST.Items.Add(new ListItem(grade.Name, grade.ID.ToString()));
            }
        }
    }
    static string EncryptID(string id)
    {
        id = id.Trim();
        string strNew = "";
        foreach (char cid in id)
        {
            int c = int.Parse(cid.ToString());
            char cc = (char)(c + 97);
            strNew += cid + cc.ToString();
        }
        return strNew;
    }
    static string DEncryptID(string s)
    {
        string news = "";
        foreach (char ca in s)
        {
            if (ca >= '0' && ca <= '9')
                news += ca;
        }
        return news;
    }
    public string CreateUserFolder(string uid)
    {
        string hash = EncryptID(uid);
        string path = "/User/Data/" + hash;
        string rel = Server.MapPath(path);
        Directory.CreateDirectory(rel);
        return path;
    }
    public string SaveImage(string folder)
    {
        string pic = "~/User/Data/" + folder.Replace("/User/Data/", "") + "/" + User_Picture.FileName;
        foreach (string file in Directory.GetFiles(Server.MapPath("~/User/Data/") + folder.Replace("/User/Data/", "")))
        {
            File.Delete(file);
        }
        User_Picture.SaveAs(Server.MapPath(pic));
        return "/User/" + pic.Substring(6);
    }
    protected void cfv_User_Email_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Session["uuid"] == null) return;
        if (User_Email.Text == MemberService.GetUser(int.Parse(Session["uuid"].ToString())).Mail)
        {
            args.IsValid = true;
            return;
        }
        args.IsValid = MemberService.Exsits(User_Email.Text);
    }
    protected void cfv_User_ID_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;//Temp - need attention
        //args.IsValid = MemberService.IsAllowed(User_First_Name.Text.Trim(), User_Last_Name.Text.Trim(), User_ID.Text.Trim());
    }
    protected void UpdateButton_Click(object sender, EventArgs e)
    {
        Page.Validate("UpdateValidationGroup");
        if (Page.IsValid)
        {
            //Create majors list
            List<Major> _Majors = new List<Major>();
            foreach (ListItem it in User_Majors.Items)
                if (it.Selected)
                    _Majors.Add(MajorsService.GetMajor(int.Parse(it.Value.Trim())));

            //Create a 'new' member
            Member m = new Member()
            {
                UserID = int.Parse(Session["uuid"].ToString()),
                ID = User_ID.Text.Trim(),
                FirstName = User_First_Name.Text.Trim().Replace("'", "''"),
                LastName = User_Last_Name.Text.Trim().Replace("'", "''"),
                Mail = User_Email.Text.Trim().Replace("'", "''"),
                Auth = MemberService.GetUser(int.Parse(Session["uuid"].ToString())).Auth,
                Gender = ((MemberGender)(char.Parse(User_Gender.SelectedValue.Trim()))),
                BornDate = DateTime.ParseExact(User_BornDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                GradeID = int.Parse(User_Section.SelectedValue.Trim()),
                City = CitiesService.GetCity(int.Parse(User_City.SelectedValue.Trim())),
                Majors = _Majors
            };

            if (User_Picture.HasFile)
            {
                string folder = CreateUserFolder(User_ID.Text);//Creating user directory
                string path = SaveImage(folder);//Saving the image
                m.PicturePath = path;//Adding picture path
                MemberService.Update(m);//Updating the user
            }
            else
                MemberService.Update(m);

            if (User_Password.Text != "!@#$%^&*(8)(&%@#&$*(")
            {
                MemberService.UpdatePassword(int.Parse(Session["uuid"].ToString()), User_Password.Text);
            }
            CitiesService.UpdateCities();
            FillAreas();
            if (Request.QueryString["uid"] != null && Request.QueryString["uid"].ToString().Trim() != "" && MemberService.GetCurrent().Auth == MemberClearance.Admin)
                UpdateFill(Request.QueryString["uid"].ToString().Trim());
            else
                UpdateFill(MemberService.GetCurrent().UserID.ToString());
            cmv_User_BornDate.ValueToCompare = DateTime.Today.AddYears(-11).ToShortDateString();
            cmv_User_BornDate_g.ValueToCompare = DateTime.Today.AddYears(-120).ToShortDateString();
            done = "$('#success-modal').openModal();";//Showing message
            if (Request.QueryString["uid"] != null && Request.QueryString["uid"].ToString().Trim() != "")
                Response.Redirect("~/User/Edit.aspx?uid=" + Request.QueryString["uid"].ToString().Trim());
        }
    }
    private void SetSelectedInCheckBox(CheckBoxList list, string listStr)
    {
        string[] values = listStr.Split(',');
        foreach (ListItem item in list.Items)
        {
            if (values.Contains(item.Value))
                item.Selected = true;
            else
                item.Selected = false;
        }
    }
    protected void cv_User_Majors_ServerValidate(object source, ServerValidateEventArgs args)
    {
        int acc = 0;
        foreach (ListItem item in User_Majors.Items)
            if (item.Selected) acc++;
        args.IsValid = (acc >= 1);
    }
    protected void DeleteButton_Click(object sender, EventArgs e)
    {
        int uid = MemberService.GetUID(User_ID.Text.Trim());
        MemberService.RemoveFromActive(uid.ToString());
        Response.Redirect("~/Default.aspx");//Taken care of task
    }

    protected void cmv_User_Password_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (User_Password.Text == "!@#$%^&*(8)(&%@#&$*(")
        {
            args.IsValid = true;
        }
        else
        {
            args.IsValid = Regex.IsMatch(User_Password.Text, "^([A-Za-z0-9]{8,32})$");
        }
    }
    protected void cv_User_Picture_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (User_Picture.HasFile)
        {
            string ending = Path.GetExtension(User_Picture.FileName);
            ending = ending.ToLower();
            args.IsValid = (ending == ".jpg" || ending == ".png");
            return;
        }
        args.IsValid = true;
    }
    protected void cv_User_Picture_ServerValidate1(object source, ServerValidateEventArgs args)
    {
        if(User_Picture.HasFile)
        {
            string ending = Path.GetExtension(User_Picture.FileName);
            string[] fileTypes = { ".png", ".bmp", ".jpg" };
            if (!fileTypes.Contains(ending.ToLower()))
            {
                args.IsValid = false;
                return;
            }
            args.IsValid = true;
            return;
        }
        args.IsValid = true;
    }
}
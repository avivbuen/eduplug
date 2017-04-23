using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Cities;
using Business_Logic.Grades;
using Business_Logic.Majors;
using Business_Logic.Members;

public partial class User_Register : System.Web.UI.Page
{
    public string done = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth!=MemberClearance.Guest)//Redirecting users that are logged in, not incl. admins because in they are able to register others.
        {
            Response.Redirect("~/Default.aspx");
        }
        else
        {
            if (!IsPostBack)
                FillAreas();
            cmv_User_BornDate.ValueToCompare = DateTime.Today.AddYears(-11).ToShortDateString();
            cmv_User_BornDate_g.ValueToCompare = DateTime.Today.AddYears(-120).ToShortDateString();
            //User_Phone.Attributes.Add("onkeypress", "return event.charCode >= 48 && event.charCode <= 57");
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
    protected void RegisterButton_Click(object sender, EventArgs e)
    {
        Page.Validate("RegisterValidationGroup");
        if (Page.IsValid)
        {
            Register();
        }
    }
    protected void Register()
    {
        string picture = "/Content/graphics/img/default.png";
        DateTime born = DateTime.ParseExact(User_BornDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        if (User_Gender.SelectedValue == "f")
        {
            picture = "/Content/graphics/img/default.png";
        }
        string folder = CreateUserFolder(User_ID.Text);
        if (User_Picture.HasFile)
        {
            SaveImage(folder);
            picture = folder + "/" + User_Picture.FileName;
        }
        //Majors cast
        List<Major> mjrs = new List<Major>();
        foreach (ListItem c in User_Majors.Items)
            if (c.Selected)
                mjrs.Add(new Major() { Id = int.Parse(c.Value), Title = c.Text });

        //Declaring a new member
        Member mem = new Member()
        {
            ID = User_ID.Text.Trim(),
            FirstName = User_First_Name.Text.Trim(),
            LastName = User_Last_Name.Text.Trim(),
            Mail = User_Email.Text.Trim(),
            Auth = MemberService.GetClearance(User_ID.Text.Trim()),
            Gender = ((MemberGender)(char.Parse(User_Gender.Text.Trim()))),
            BornDate = DateTime.ParseExact(User_BornDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
            RegisterationDate = DateTime.Now,
            PicturePath = picture,
            GradeID = int.Parse(User_Section.SelectedValue.Trim()),
            City = CitiesService.GetCity(int.Parse(User_City.SelectedValue.Trim())),
            Majors = mjrs
        };
        MemberService.Add(mem, User_Password.Text.Trim());//Adding the member with the password - it will be hashed by the business logic.
        MemberService.UpdateAllowed(int.Parse(User_ID.Text.Trim()));//Updating that the user have registered.
        done = "alert('ההרשמה עברה בהצלחה. אתה מועבר לדף כניסה');location='../'";//Showing message
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
    protected void cv_User_Picture_ServerValidate1(object source, ServerValidateEventArgs args)
    {
        if (User_Picture.HasFile)
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
        string pic = "~/User/Data" + folder.Replace("/User/Data", "") + "/" + User_Picture.FileName;
        User_Picture.SaveAs(Server.MapPath(pic));
        return pic.Substring(6);
    }
    public static string GetFullTimeReadyForDataBase()
    {
        return GetTimeReadyForDataBase(DateTime.Now);
    }
    public static string GetTimeReadyForDataBase(DateTime time)
    {
        return time.ToString("yyyy-MM-dd H:mm:ss");
    }
    protected void cfv_User_Email_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = MemberService.Exsits(User_Email.Text.Trim());
    }
    protected void cfv_User_ID_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = MemberService.IsAllowed(User_First_Name.Text, User_Last_Name.Text, User_ID.Text);
    }
    protected void cv_User_Majors_ServerValidate(object source, ServerValidateEventArgs args)
    {
        int acc = 0;
        foreach (ListItem item in User_Majors.Items)
            if (item.Selected) acc++;
        args.IsValid = (acc >= 1);
    }
}
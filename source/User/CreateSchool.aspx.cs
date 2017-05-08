using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Description;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Cities;
using Business_Logic.Grades;
using Business_Logic.Majors;
using Business_Logic.Members;
using Business_Logic.Schools;

public partial class User_CreateSchool : System.Web.UI.Page
{
    public string done = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Guest)
        {
            if (MemberService.GetCurrent().Active == "Wait")
                MemberService.Logout();
            Response.Redirect("~/Default.aspx", false);
        }
        if (!IsPostBack)
        {
            Thread t = new Thread(() => SchoolService.HandleDB());
            t.Start();
            FillAreas();
        }



    }
    protected void FillAreas()
    {
        List<City> cities = CitiesService.GetAll();

        //Fill(User_City, "בחר/י עיר", cities, "eduCity", "eduCityID");
        Fill(SchoolCity, "בחר עיר לימוד", cities, "eduCity", "eduCityID");
        //Fill(User_Section, "בחר/י כיתה/אזור", GradesService.GetAll(), "eduGradeName", "eduGradeID");
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
            if (c is School)
            {
                School school = (School)c;
                drpdwnLST.Items.Add(new ListItem(school.Name, school.Id.ToString()));
            }
        }
    }

    protected void Known_OnClick(object sender, EventArgs e)
    {
        Session["knkwnVal"] = true;
        PanelStage1.Visible = false;
        PanelStageSchool.Visible = true;
        PanelKnown.Visible = true;
        PanelUnKnown.Visible = false;
    }

    protected void UknownBtn_OnClick(object sender, EventArgs e)
    {
        Session["knkwnVal"] = false;
        PanelStage1.Visible = false;
        PanelStageSchool.Visible = true;
        PanelUnKnown.Visible = true;
        PanelKnown.Visible = false;
    }

    protected void cfv_User_ID_ServerValidate(object source, ServerValidateEventArgs args)
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

    protected void cv_User_Picture_OnServerValidate(object source, ServerValidateEventArgs args)
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



    protected void RegisterButton_OnClick(object sender, EventArgs e)
    {
        Page.Validate();
        if (Page.IsValid)
        {
            if (Session["knkwnVal"] == null)
                Response.Redirect("~/");
            string picture = "/Content/graphics/img/default.png";
            string folder = CreateUserFolder(User_ID.Text);
            if (User_Picture.HasFile)
            {
                SaveImage(folder);
                picture = folder + "/" + User_Picture.FileName;
            }
            School school = (School)Session["curSchoolCreate"];
            //Member member = MemberService.GetUser(MemberService.GetUID(User_ID.Text));

            Member mem = new Member()
            {
                ID = User_ID.Text.Trim(),
                FirstName = User_First_Name.Text.Trim(),
                LastName = User_Last_Name.Text.Trim(),
                Auth = MemberClearance.Admin,
                Gender = ((MemberGender)(char.Parse(User_Gender.Text.Trim()))),
                BornDate = DateTime.ParseExact(User_BornDate.Text, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture),
                RegisterationDate = DateTime.Now,
                PicturePath = picture,
                City = CitiesService.GetCity(school.City.Id),
                School = new School() { Id = school.Id },
                GradeID = 33
            };
            MemberService.Add(mem, User_Password.Text.Trim());
            done = "alert('השלמת הפרטים עברה בהצלחה. אתה מועבר לדף כניסה לצורך כניסה מחדש.');location='../User'";//Showing message
        }
    }

    protected void prevKnown_OnClick(object sender, EventArgs e)
    {
        PanelStageSchool.Visible = true;
        PanelStage2.Visible = false;
    }

    protected void prev1Known_OnClick(object sender, EventArgs e)
    {
        if (Session["curSchoolCreate"] != null)
        {
            SchoolService.Delete(((School)Session["curSchoolCreate"]).Id);
            Session["curSchoolCreate"] = null;
        }
        Session["SchoolRegistered"] = null;
        PanelStage1.Visible = true;
        PanelStageSchool.Visible = false;
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
    public string CreateSchoolFolder(string uid)
    {
        string encrypt = EncryptID(uid);
        string path = "/User/SchoolData/" + encrypt;
        string rel = Server.MapPath(path);
        Directory.CreateDirectory(rel);
        return path;
    }
    public string SaveLogo(string folder)
    {
        string pic = "~/User/SchoolData" + folder.Replace("/User/SchoolData", "") + "/" + FileUploadLogo.FileName;
        FileUploadLogo.SaveAs(Server.MapPath(pic));
        return pic.Substring(6);
    }
    protected void nextKnown_OnClick(object sender, EventArgs e)
    {
        if (Session["SchoolRegistered"] != null)
        {
            School school = (School)Session["curSchoolCreate"];
            bool knk = (bool)Session["knkwnVal"];
            string name = knk ? SchoolName.Items[SchoolName.SelectedIndex].Text : School_Name.Text;
            if (school.Name == name)
            {
                PanelStage2.Visible = true;
                PanelStageSchool.Visible = false;
                return;
            }
        }

        if (Session["knkwnVal"] == null)
            Response.Redirect("~/");
        Page.Validate();
        if (Page.IsValid)
        {
            bool knk = (bool)Session["knkwnVal"];
            int id = knk ? int.Parse(SchoolName.SelectedValue) : SchoolService.GetNID();
            string name = knk ? SchoolName.Items[SchoolName.SelectedIndex].Text : School_Name.Text;
            School school = new School()
            {
                Id = id,
                Name = name,
                City = new City() { Id = int.Parse(SchoolCity.SelectedValue) },
                Official = knk                
            };

            if (FileUploadLogo.HasFile)
            {
                string folder = CreateSchoolFolder(id.ToString());
                school.LogoPath = folder + "/" + FileUploadLogo.FileName;
                SaveLogo(folder);
            }
            if (Session["curSchoolCreate"] != null)
            {
                School schoolDel = (School)Session["curSchoolCreate"];
                SchoolService.Delete(schoolDel.Id);
            }
            if (SchoolService.Add(school))
            {
                PanelStage2.Visible = true;
                PanelStageSchool.Visible = false;
            }
            Session["curSchoolCreate"] = school;
            Session["SchoolRegistered"] = true;

        }

    }

    protected void SchoolCity_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (SchoolCity.SelectedValue != "-1")
        {
            List<School> schools = SchoolService.GetKnownCity(int.Parse(SchoolCity.SelectedValue));
            Fill(SchoolName, "בחר מוסד לימוד", schools, "Name", "Id");
            SchoolName.Visible = true;
        }
    }

    protected void PictureUpload_OnServerValidate(object source, ServerValidateEventArgs args)
    {
        if (FileUploadLogo.HasFile)
        {
            string ending = Path.GetExtension(FileUploadLogo.FileName);
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

    protected void CustomValidator2_OnServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Session["knkwnVal"] == null)
            Response.Redirect("~/");
        bool knk = (bool)Session["knkwnVal"];
        if (knk)
        {
            args.IsValid = !SchoolService.Exsits(int.Parse(SchoolName.SelectedValue));
            return;
        }
        args.IsValid = true;
    }

    protected void CustomValidator3_OnServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Session["curSchoolCreate"] == null)
            Response.Redirect("~/");
        School school = (School)Session["curSchoolCreate"];
        Member member = MemberService.GetUser(MemberService.GetUID(User_ID.Text, school.Id));
        if (member == null)
        {
            args.IsValid = true;
            return;
        }
        if (member.FirstName != User_First_Name.Text)
        {
            args.IsValid = false;
            return;
        }
        if (member.LastName != User_Last_Name.Text)
        {
            args.IsValid = false;
            return;
        }
        if (!MemberService.Validate(User_ID.Text, User_Password.Text))
        {
            args.IsValid = false;
            return;
        }

        args.IsValid = true;
    }
}
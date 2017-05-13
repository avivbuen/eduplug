using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic.Members;
using Business_Logic.Schools;

public partial class Panel_Admin_School : System.Web.UI.Page
{
    public string done = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            Fill();
    }

    protected void Fill()
    {
        School_Name.Text = MemberService.GetCurrent().School.Name;
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
    protected void applyBtn_OnClick(object sender, EventArgs e)
    {
        Page.Validate();
        if (Page.IsValid)
        {
            School currentSchool = MemberService.GetCurrent().School;
            School school = new School()
            {
                Id = currentSchool.Id,
                Name = School_Name.Text
            };
            if (FileUploadLogo.HasFile)
            {
                string folder = CreateSchoolFolder(currentSchool.Id.ToString());
                school.LogoPath = folder + "/" + FileUploadLogo.FileName;
                SaveLogo(folder);

            }
            Member m = (Member)Session["Member"];
            if (school.LogoPath != null && school.LogoPath.Trim() != "")
                m.School.LogoPath = school.LogoPath;
            m.School.Name = school.Name;
            Session["Member"] = m;
            SchoolService.Update(school);
            done = "AvivnetFramework.toast('המשתמשים הועלו', 6000);";//Showing message
            Response.Redirect("~/Panel/Admin/");
        }
    }
}
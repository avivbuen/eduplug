using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Members;

public partial class User_Activate : System.Web.UI.Page
{
    public string done = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Member mem = MemberService.GetCurrent();
            if (mem.Auth == MemberClearance.Guest || mem.Active != "Wait")
            {
                Response.Redirect("~/");
            }
        }

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

    protected void RegisterButton_OnClick(object sender, EventArgs e)
    {
        Page.Validate();
        if (Page.IsValid)
        {
            string picture = "/Content/graphics/img/default.png";
            string folder = CreateUserFolder(MemberService.GetCurrent().ID);
            if (User_Picture.HasFile)
            {
                SaveImage(folder);
                picture = folder + "/" + User_Picture.FileName;
            }
            Member m = new Member()
            {
                UserID = MemberService.GetCurrent().UserID,
                Mail = User_Email.Text,
                PicturePath = picture
            };
            MemberService.Complete(m, User_Password.Text.Trim());
            done = "alert('השלמת הפרטים עברה בהצלחה. אתה מועבר לדף כניסה לצורך כניסה מחדש.');location='../User'";//Showing message
        }
    }

    protected void cv_TempPass_OnServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = MemberService.Validate(MemberService.GetCurrent().UserID, User_PasswordTemp.Text);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Members;

public partial class User_Reset : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(MemberService.GetCurrent().Auth!=MemberClearance.Guest)
        {
            Response.Redirect("~/");
        }
    }

    protected string script;
    protected void ResetButton_Click(object sender, EventArgs e)
    {
        Page.Validate();
        if (Page.IsValid)
        {
            Member m = MemberService.GetUser(MemberService.GetUID(User_ID.Text));
            if (m==null)
            {
                SendToUser("תעודת זהות שגויה");
            }
            else
            {
                Random rnd = new Random();
                int code = rnd.Next(9999, 999999);
                Session["resetCode"] = code;
                Session["resetMem"] = m;
                SendMail(m.Mail, code.ToString());
                PanelStage1.Visible = false;
                PanelStage2.Visible = true;
                SendToUser("קוד האיפוס נשלח אל המייל שלך");
            }
        }
    }
    public static void SendMail(string to, string code)
    {
        MailMessage Message = new MailMessage("eduplug.website@gmail.com", to);
        Message.Subject = "איפוס סיסמת אדופלאג";
        Message.Body = "<center style='width: 100%; background: #222222; text-align: left;'> <table class='email-container' style='margin: auto;' border='0' width='600' cellspacing='0' cellpadding='0' align='center'><tr><td bgcolor='#ffffff'>&nbsp;</td></tr><tr><td style='padding: 40px; text-align: center; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;' bgcolor='#ffffff'><p>מישהו ביקש איפוס סיסמה לחשבון האדופלאג שלך</p><p>" + code + " קוד האיפוס לקבלת סיסמה חדשה הוא&nbsp; </p><table style='margin: auto; height: 64px;' border='0' width='136' cellspacing='0' cellpadding='0' align='center'><tbody></center>";
        Message.IsBodyHtml = true;
        SmtpClient client = new SmtpClient();
        client.Host = "smtp.googlemail.com";
        client.Port = 587;
        client.UseDefaultCredentials = false;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.EnableSsl = true;
        client.Credentials = new NetworkCredential("eduplug.website@gmail.com", "avivadi1");
        client.Send(Message);
    }
    protected void SendToUser(string message)
    {
        script = "alert('" + message + "');";
    }
    protected void SendToUser(string message,bool home)
    {
        script = "alert('" + message + "');location='../'";
    }

    protected void CodeButton_Click(object sender, EventArgs e)
    {
        Page.Validate();
        if (Page.IsValid)
        {
            if (Session["resetCode"] == null)
                Response.Redirect("~/");
            int code = (int)Session["resetCode"];
            if(code.ToString()!=User_Code.Text)
            {
                SendToUser("קוד שגויי");
            }
            else
            {
                PanelStage2.Visible = false;
                PanelStage3.Visible = true;
            }
        }
    }

    protected void ChangeButton_Click(object sender, EventArgs e)
    {
        Page.Validate();
        if (Page.IsValid)
        {
            if (Session["resetMem"] == null) Response.Redirect("~/");
            Member mem = (Member)Session["resetMem"];
            MemberService.UpdatePassword(mem.UserID, User_Password.Text.Trim());
            SendToUser("הסיסמה אופסה בהצלחה!",true);
        }
    }
}
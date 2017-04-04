using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Messages_Read : System.Web.UI.Page
{
    public string _messageHtml = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth == MemberClearance.Guest)
            Response.Redirect("~/Default.aspx");
        if (Request.QueryString["mid"] == null || Request.QueryString["mid"].ToString().Trim() == "")
            Response.Redirect("~/Default.aspx");
        Fill();
    }
    protected void Fill()
    {
        List<Message> messages = MessagesService.GetAllUser(MemberService.GetCurrent().UserID);
        List<Message> current = messages.Where(x => x.ID == int.Parse(Request.QueryString["mid"].ToString().Trim())).ToList();
        if (current.Count == 1)
        {
            Message m = current.First();
            _messageHtml = m.Content;
            LabelTitle.Text = m.Subject;
            LabelDate.Text = m.SentDate.ToString("dd/MM/yyyy");
            LabelReciver.Text = MemberService.GetUser(m.ReciverID).Name;
            LabelSender.Text = MemberService.GetUser(m.SenderID).Name;
            if (m.ReciverID == MemberService.GetCurrent().UserID)
                MessagesService.MarkAsRead(m.ID);
        }
        else
        {
            Response.Redirect("~/Default.aspx");
        }
    }

    protected void GoBackBtn_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/Messages");
    }
}
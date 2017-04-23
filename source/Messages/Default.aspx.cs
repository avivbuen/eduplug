using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic.Members;
using Business_Logic.Messages;

public partial class Messages_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            Fill();
    }
    protected void Fill()
    {
        List<Message> messages = MessagesService.GetAllUser(MemberService.GetCurrent().UserID);
        List<Message> all = messages.OrderByDescending(x => x.SentDate).OrderBy(x => x.Read).ToList();
        ListViewAll.DataSource = all;
        ListViewAll.DataBind();
        if (all.Count == 0)
            PanelEmptyAll.Visible = true;
        else
            PanelEmptyAll.Visible = false;

        List<Message> In = messages.Where(x => x.ReciverId == MemberService.GetCurrent().UserID).OrderByDescending(x => x.SentDate).OrderBy(x => x.Read).ToList();
        ListViewIn.DataSource = In;
        ListViewIn.DataBind();
        if (In.Count == 0)
            PanelEmptyIn.Visible = true;
        else
            PanelEmptyIn.Visible = false;

        List<Message> Out = messages.Where(x => x.SenderId == MemberService.GetCurrent().UserID && !x.Guest).OrderByDescending(x => x.SentDate).OrderBy(x => x.Read).ToList();
        ListViewOut.DataSource = Out;
        ListViewOut.DataBind();
        if (Out.Count == 0)
            PanelEmptyOut.Visible = true;
        else
            PanelEmptyOut.Visible = false;
    }
    protected string CastVisi(object val)
    {
        if (!(bool)val)
        {
            return "unreaded";
        }
        return "";
    }
    public string CastSenderReciver(object rec,object sender,object senderid)
    {
        string recName = (string)rec;
        string senderName = (string)sender;
        if (((int)senderid)==MemberService.GetCurrent().UserID)
        {
            return "אל: " + recName;
        }
        else
        {
            return "מאת: " + senderName;
        }
    }
    public string CastPicPath(object recID)
    {
        Member M = MemberService.GetUser(int.Parse(recID.ToString()));
        return M.PicturePath;
    }
    protected void ListViewAll_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        int ID = int.Parse(ListViewAll.DataKeys[e.ItemIndex]["ID"].ToString());
        MessagesService.Delete(MemberService.GetCurrent().UserID, ID);
        Fill();
    }

    protected void ListViewOut_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        int ID = int.Parse(ListViewOut.DataKeys[e.ItemIndex]["ID"].ToString());
        MessagesService.Delete(MemberService.GetCurrent().UserID, ID);
        Fill();
    }

    protected void ListViewIn_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        int ID = int.Parse(ListViewIn.DataKeys[e.ItemIndex]["ID"].ToString());
        MessagesService.Delete(MemberService.GetCurrent().UserID, ID);
        Fill();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Members;
using Business_Logic.Messages;

public partial class Messages_Compose : System.Web.UI.Page
{
    public string done = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth == MemberClearance.Guest)
            Response.Redirect("~/Default.aspx");
        if (!IsPostBack)
            Fill();

    }
    protected void Fill()
    {
        Member current = MemberService.GetCurrent();
        List<Member> mems = MemberService.GetNames();
        if (current.Auth == MemberClearance.Admin)
        {
            UsersToSend.DataSource = mems.AsEnumerable().Where(x => x.Active&&x.UserID!=current.UserID).ToList();
        }
        else if (current.Auth == MemberClearance.Teacher)
        {
            DropDownUsers.DataSource = mems.AsEnumerable().Where(x => x.Active && x.Auth != MemberClearance.Admin && x.UserID != current.UserID).ToList();
        }
        else
        {
            DropDownUsers.DataSource = mems.AsEnumerable().Where(x => x.Active && x.Auth == MemberClearance.Teacher && x.UserID != current.UserID).ToList();
        }
        UsersToSend.DataTextField = "Name";
        UsersToSend.DataValueField = "UserID";
        UsersToSend.DataBind();
        DropDownUsers.DataTextField = "Name";
        DropDownUsers.DataValueField = "UserID";
        DropDownUsers.DataBind();
        if (MemberService.GetCurrent().Auth == MemberClearance.Student || MemberService.GetCurrent().Auth == MemberClearance.Teacher)
        {
            PanelSelectorAll.Visible = true;
        }
        else
        {
            PanelSelectorAdmin.Visible = true;
        }
    }

    protected void cv_UsersToSend_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if(!UsersToSend.Visible)
        {
            if (DropDownUsers.SelectedValue=="-1")
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
            return;
        }
        int acc = 0;
        foreach (ListItem item in UsersToSend.Items)
            if (item.Selected) acc++;
        args.IsValid = (acc >= 1);
    }

    protected void SendButton_Click(object sender, EventArgs e)
    {
        Page.Validate("ComposeValidationGroup");
        if (Page.IsValid)
        {
            if(DropDownUsers.Visible)
            {
                Message m = new Message()
                {
                    Content = TextBoxMessageContent.Text.Trim(),
                    ReciverId = int.Parse(DropDownUsers.SelectedValue),
                    SenderId = MemberService.GetCurrent().UserID,
                    Subject = Message_Subject.Text.Trim()
                };
                MessagesService.SendMessage(m);
                done = "$('#success-modal').openModal();";//Showing message
            }
            else
            {
                foreach (ListItem lt in UsersToSend.Items)
                {
                    if (lt.Selected)
                    {
                        Message m = new Message()
                        {
                            Content = TextBoxMessageContent.Text.Trim(),
                            ReciverId = int.Parse(lt.Value),
                            SenderId = MemberService.GetCurrent().UserID,
                            Subject = Message_Subject.Text.Trim()
                        };
                        MessagesService.SendMessage(m);
                    }
                }
                done = "$('#success-modal').openModal();";//Showing message
            }
        }
    }
}
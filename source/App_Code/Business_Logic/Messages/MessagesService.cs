using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Message Service
/// </summary>
public static class MessagesService
{
    /// <summary>
    /// Sends a message
    /// </summary>
    /// <param name="m">Message Object</param>
    public static void SendMessage(Message m)
    {
        Connect.InsertUpdateDelete("INSERT INTO nhsMessages (nhsMessageSender,nhsMessageReciver,nhsMessageSubject,nhsMessageContent,nhsMessageRead,nhsDateSent,nhsGuestMessage) VALUES (" + m.SenderID + "," + m.ReciverID + ",'" + m.Subject.Replace("'","''") + "','" + m.Content.Replace("'", "''") + "',No,'" + Converter.GetFullTimeReadyForDataBase() + "'," + m.Guest.ToString() + ")");
    }
    /// <summary>
    /// Marks a message as read by message id
    /// </summary>
    /// <param name="mid">Message ID</param>
    public static void MarkAsRead(int mid)
    {
        Connect.InsertUpdateDelete("UPDATE nhsMessages SET nhsMessageRead=Yes WHERE nhsMessageID=" + mid);
    }
    /// <summary>
    /// Marks a message as read by message object(id)
    /// </summary>
    /// <param name="mid">Message</param>
    public static void MarkAsRead(Message m) { MarkAsRead(m.ID); }
    /// <summary>
    /// Gets the user messages
    /// </summary>
    /// <param name="uid">User ID</param>
    public static List<Message> GetAllUser(int uid)
    {
        List<Message> messages = new List<Message>();
        DataTable dt = Connect.GetData("SELECT m1.nhsFirstName +' '+ m1.nhsLastName AS nhsSenderName,m2.nhsFirstName + ' ' + m2.nhsLastName AS nhsReciverName,nhsMessages.nhsMessageID AS nhsMessageID,nhsMessages.nhsMessageSender AS nhsMessageSender,nhsMessages.nhsMessageReciver AS nhsMessageReciver,nhsMessages.nhsMessageSubject AS nhsMessageSubject,nhsMessages.nhsMessageContent AS nhsMessageContent,nhsMessages.nhsMessageRead AS nhsMessageRead,nhsMessages.nhsDateSent AS nhsDateSent,nhsMessages.nhsActive AS nhsActive FROM nhsMembers AS m1, nhsMembers AS m2, nhsMessages WHERE (m1.nhsUserID=nhsMessages.nhsMessageSender AND m2.nhsUserID=nhsMessages.nhsMessageReciver) AND (nhsMessages.nhsMessageReciver=" + uid + " OR nhsMessages.nhsMessageSender=" + uid + ") AND (nhsGuestMessage=No) AND (nhsMessages.nhsActive<> ':" + uid + ":' OR nhsMessages.nhsActive IS NULL)", "nhsMessages");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Message m = new Message()
            {
                ID = int.Parse(dt.Rows[i]["nhsMessageID"].ToString().Trim()),
                SenderID = int.Parse(dt.Rows[i]["nhsMessageSender"].ToString().Trim()),
                ReciverID = int.Parse(dt.Rows[i]["nhsMessageReciver"].ToString().Trim()),
                Subject = dt.Rows[i]["nhsMessageSubject"].ToString().Trim(),
                Content = dt.Rows[i]["nhsMessageContent"].ToString().Trim(),
                Read = Convert.ToBoolean(dt.Rows[i]["nhsMessageRead"].ToString().Trim()),
                SentDate = DateTime.Parse(dt.Rows[i]["nhsDateSent"].ToString().Trim()),
                State = dt.Rows[i]["nhsActive"].ToString().Trim(),
                SenderName = dt.Rows[i]["nhsSenderName"].ToString().Trim(),
                ReciverName = dt.Rows[i]["nhsReciverName"].ToString().Trim()
            };
            messages.Add(m);
        }
        return messages;
    }
    /// <summary>
    /// Gets all messages
    /// </summary>
    public static List<Message> GetAll()
    {
        List<Message> messages = new List<Message>();
        DataTable dt = Connect.GetData("SELECT * FROM nhsMessages", "nhsMessages");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Message m = new Message()
            {
                ID = int.Parse(dt.Rows[i]["nhsMessageID"].ToString().Trim()),
                SenderID = int.Parse(dt.Rows[i]["nhsMessageSender"].ToString().Trim()),
                ReciverID = int.Parse(dt.Rows[i]["nhsMessageReciver"].ToString().Trim()),
                Subject = dt.Rows[i]["nhsMessageSubject"].ToString().Trim(),
                Content = dt.Rows[i]["nhsMessageContent"].ToString().Trim(),
                Read = Convert.ToBoolean(dt.Rows[i]["nhsMessageRead"].ToString().Trim()),
                SentDate = DateTime.Parse(dt.Rows[i]["nhsDateSent"].ToString().Trim()),
                State = dt.Rows[i]["nhsActive"].ToString().Trim()
            };
            messages.Add(m);
        }
        return messages;
    }
    /// <summary>
    /// Deletes a message
    /// </summary>
    /// <param name="uid">User ID</param>
    /// <param name="mid">Message ID</param>
    public static void Delete(int uid, int mid)
    {
        List<Message> getALL = GetAll();
        if (getALL.Count == 0)
            return;

        string state = getALL.Single(x => x.ID == mid).State;
        if (state.Contains(":" + uid + ":")) return;
        if (state.Trim() == "")
        {
            Connect.InsertUpdateDelete("UPDATE nhsMessages SET nhsActive=':" + uid + ":' WHERE nhsMessageID=" + mid);
        }
        else
        {
            Connect.InsertUpdateDelete("DELETE FROM nhsMessages WHERE nhsMessageID=" + mid);
        }
    }
    /// <summary>
    /// Gets the unread count of the user
    /// </summary>
    /// <param name="uid">User ID</param>
    public static int GetUnreaedCount(int uid)
    {
        if (uid == 0) return 0;
        object obj = Connect.GetObject("SELECT COUNT(*) FROM nhsMessages WHERE nhsMessageReciver=" + uid + " AND nhsMessageRead=NO AND (nhsActive<> ':" + uid + ":' OR nhsActive IS NULL)");
        if (obj == null) return 0;
        return int.Parse(obj.ToString());
    }
    public static List<Message> GetUnreaed(int uid)
    {
        List<Message> messages = new List<Message>();
        DataTable dt = Connect.GetData("SELECT m1.nhsFirstName +' '+ m1.nhsLastName AS nhsSenderName,m2.nhsFirstName + ' ' + m2.nhsLastName AS nhsReciverName,nhsMessages.nhsMessageID AS nhsMessageID,nhsMessages.nhsMessageSender AS nhsMessageSender,nhsMessages.nhsMessageReciver AS nhsMessageReciver,nhsMessages.nhsMessageSubject AS nhsMessageSubject,nhsMessages.nhsMessageContent AS nhsMessageContent,nhsMessages.nhsMessageRead AS nhsMessageRead,nhsMessages.nhsDateSent AS nhsDateSent,nhsMessages.nhsActive AS nhsActive FROM nhsMembers AS m1, nhsMembers AS m2, nhsMessages WHERE (m1.nhsUserID=nhsMessages.nhsMessageSender AND m2.nhsUserID=nhsMessages.nhsMessageReciver) AND (nhsMessages.nhsMessageReciver=" + uid + " OR nhsMessages.nhsMessageSender=" + uid + ") AND (nhsGuestMessage=No) AND (nhsMessageRead=NO) AND (nhsMessages.nhsActive<> ':" + uid + ":' OR nhsMessages.nhsActive IS NULL)", "nhsMessages");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Message m = new Message()
            {
                ID = int.Parse(dt.Rows[i]["nhsMessageID"].ToString().Trim()),
                SenderID = int.Parse(dt.Rows[i]["nhsMessageSender"].ToString().Trim()),
                ReciverID = int.Parse(dt.Rows[i]["nhsMessageReciver"].ToString().Trim()),
                Subject = dt.Rows[i]["nhsMessageSubject"].ToString().Trim(),
                Content = dt.Rows[i]["nhsMessageContent"].ToString().Trim(),
                Read = Convert.ToBoolean(dt.Rows[i]["nhsMessageRead"].ToString().Trim()),
                SentDate = DateTime.Parse(dt.Rows[i]["nhsDateSent"].ToString().Trim()),
                State = dt.Rows[i]["nhsActive"].ToString().Trim(),
                SenderName = dt.Rows[i]["nhsSenderName"].ToString().Trim(),
                ReciverName = dt.Rows[i]["nhsReciverName"].ToString().Trim()
            };
            messages.Add(m);
        }
        return messages;
    }

}
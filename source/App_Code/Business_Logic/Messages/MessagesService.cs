using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Business_Logic.Messages
{
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
            Connect.InsertUpdateDelete("INSERT INTO eduMessages (eduMessageSender,eduMessageReciver,eduMessageSubject,eduMessageContent,eduMessageRead,eduDateSent) VALUES (" + m.SenderId + "," + m.ReciverId + ",'" + m.Subject.Replace("'","''") + "','" + m.Content.Replace("'", "''") + "',No,'" + Converter.GetFullTimeReadyForDataBase() + "')");
        }
        /// <summary>
        /// Marks a message as read by message id
        /// </summary>
        /// <param name="mid">Message ID</param>
        public static void MarkAsRead(int mid)
        {
            Connect.InsertUpdateDelete("UPDATE eduMessages SET eduMessageRead=Yes WHERE eduMessageID=" + mid);
        }

        /// <summary>
        /// Marks a message as read by message object(id)
        /// </summary>
        public static void MarkAsRead(Message m) { MarkAsRead(m.Id); }
        /// <summary>
        /// Gets the user messages
        /// </summary>
        /// <param name="uid">User ID</param>
        public static List<Message> GetAllUser(int uid)
        {
            List<Message> messages = new List<Message>();
            DataTable dt = Connect.GetData("SELECT m1.eduFirstName +' '+ m1.eduLastName AS eduSenderName,m2.eduFirstName + ' ' + m2.eduLastName AS eduReciverName,eduMessages.eduMessageID AS eduMessageID,eduMessages.eduMessageSender AS eduMessageSender,eduMessages.eduMessageReciver AS eduMessageReciver,eduMessages.eduMessageSubject AS eduMessageSubject,eduMessages.eduMessageContent AS eduMessageContent,eduMessages.eduMessageRead AS eduMessageRead,eduMessages.eduDateSent AS eduDateSent,'Yes' AS eduActive FROM eduMembers AS m1, eduMembers AS m2, eduMessages WHERE (m1.eduUserID=eduMessages.eduMessageSender AND m2.eduUserID=eduMessages.eduMessageReciver) AND (eduMessages.eduMessageReciver=" + uid + " OR eduMessages.eduMessageSender=" + uid + ") AND (eduMessages.eduActive<> ':" + uid + ":' OR eduMessages.eduActive IS NULL)", "eduMessages");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Message m = new Message()
                {
                    Id = int.Parse(dt.Rows[i]["eduMessageID"].ToString().Trim()),
                    SenderId = int.Parse(dt.Rows[i]["eduMessageSender"].ToString().Trim()),
                    ReciverId = int.Parse(dt.Rows[i]["eduMessageReciver"].ToString().Trim()),
                    Subject = dt.Rows[i]["eduMessageSubject"].ToString().Trim(),
                    Content = dt.Rows[i]["eduMessageContent"].ToString().Trim(),
                    Read = Convert.ToBoolean(dt.Rows[i]["eduMessageRead"].ToString().Trim()),
                    SentDate = DateTime.Parse(dt.Rows[i]["eduDateSent"].ToString().Trim()),
                    State = dt.Rows[i]["eduActive"].ToString().Trim(),
                    SenderName = dt.Rows[i]["eduSenderName"].ToString().Trim(),
                    ReciverName = dt.Rows[i]["eduReciverName"].ToString().Trim()
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
            var dt = Connect.GetData("SELECT * FROM eduMessages", "eduMessages");
            return (from DataRow dataRow in dt.Rows
                select new Message()
                {
                    Id = int.Parse(dataRow["eduMessageID"].ToString().Trim()),
                    SenderId = int.Parse(dataRow["eduMessageSender"].ToString().Trim()),
                    ReciverId = int.Parse(dataRow["eduMessageReciver"].ToString().Trim()),
                    Subject = dataRow["eduMessageSubject"].ToString().Trim(),
                    Content = dataRow["eduMessageContent"].ToString().Trim(),
                    Read = Convert.ToBoolean(dataRow["eduMessageRead"].ToString().Trim()),
                    SentDate = DateTime.Parse(dataRow["eduDateSent"].ToString().Trim()),
                    State = dataRow["eduActive"].ToString().Trim()
                }).ToList();
        }
        /// <summary>
        /// Deletes a message
        /// </summary>
        /// <param name="uid">User ID</param>
        /// <param name="mid">Message ID</param>
        public static void Delete(int uid, int mid)
        {
            var getAll = GetAll();
            if (getAll.Count == 0)
                return;

            var state = getAll.Single(x => x.Id == mid).State;
            if (state.Contains(":" + uid + ":")) return;
            if (state.Trim() == "")
            {
                Connect.InsertUpdateDelete("UPDATE eduMessages SET eduActive=':" + uid + ":' WHERE eduMessageID=" + mid);
            }
            else
            {
                Connect.InsertUpdateDelete("DELETE FROM eduMessages WHERE eduMessageID=" + mid);
            }
        }
        /// <summary>
        /// Gets the unread count of the user
        /// </summary>
        /// <param name="uid">User ID</param>
        public static int GetUnreaedCount(int uid)
        {
            if (uid == 0) return 0;
            var obj = Connect.GetObject("SELECT COUNT(*) FROM eduMessages WHERE eduMessageReciver=" + uid + " AND eduMessageRead=NO AND (eduActive<> ':" + uid + ":' OR eduActive IS NULL)");
            return obj == null ? 0 : int.Parse(obj.ToString());
        }
        /// <summary>
        /// Get unread messages of user
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static List<Message> GetUnreaed(int uid)
        {
            var dt = Connect.GetData("SELECT m1.eduFirstName +' '+ m1.eduLastName AS eduSenderName,m2.eduFirstName + ' ' + m2.eduLastName AS eduReciverName,eduMessages.eduMessageID AS eduMessageID,eduMessages.eduMessageSender AS eduMessageSender,eduMessages.eduMessageReciver AS eduMessageReciver,eduMessages.eduMessageSubject AS eduMessageSubject,eduMessages.eduMessageContent AS eduMessageContent,eduMessages.eduMessageRead AS eduMessageRead,eduMessages.eduDateSent AS eduDateSent,eduMessages.eduActive AS eduActive FROM eduMembers AS m1, eduMembers AS m2, eduMessages WHERE (m1.eduUserID=eduMessages.eduMessageSender AND m2.eduUserID=eduMessages.eduMessageReciver) AND (eduMessages.eduMessageReciver=" + uid + " OR eduMessages.eduMessageSender=" + uid + ") AND (eduMessageRead=NO) AND (eduMessages.eduActive<> ':" + uid + ":' OR eduMessages.eduActive IS NULL)", "eduMessages");
            return (from DataRow dataRow in dt.Rows
                select new Message()
                {
                    Id = int.Parse(dataRow["eduMessageID"].ToString().Trim()),
                    SenderId = int.Parse(dataRow["eduMessageSender"].ToString().Trim()),
                    ReciverId = int.Parse(dataRow["eduMessageReciver"].ToString().Trim()),
                    Subject = dataRow["eduMessageSubject"].ToString().Trim(),
                    Content = dataRow["eduMessageContent"].ToString().Trim(),
                    Read = Convert.ToBoolean(dataRow["eduMessageRead"].ToString().Trim()),
                    SentDate = DateTime.Parse(dataRow["eduDateSent"].ToString().Trim()),
                    State = dataRow["eduActive"].ToString().Trim(),
                    SenderName = dataRow["eduSenderName"].ToString().Trim(),
                    ReciverName = dataRow["eduReciverName"].ToString().Trim()
                }).ToList();
        }
    }
}
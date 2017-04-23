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
            Connect.InsertUpdateDelete("INSERT INTO nhsMessages (nhsMessageSender,nhsMessageReciver,nhsMessageSubject,nhsMessageContent,nhsMessageRead,nhsDateSent) VALUES (" + m.SenderId + "," + m.ReciverId + ",'" + m.Subject.Replace("'","''") + "','" + m.Content.Replace("'", "''") + "',No,'" + Converter.GetFullTimeReadyForDataBase() + "')");
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
        public static void MarkAsRead(Message m) { MarkAsRead(m.Id); }
        /// <summary>
        /// Gets the user messages
        /// </summary>
        /// <param name="uid">User ID</param>
        public static List<Message> GetAllUser(int uid)//TODO BIG PROBLEM HERE CODE DOES NOT RUN I GET AN ERROR ON QUERY RUN!
        {
            List<Message> messages = new List<Message>();
            DataTable dt = Connect.GetData("SELECT m1.nhsFirstName +' '+ m1.nhsLastName AS nhsSenderName,m2.nhsFirstName + ' ' + m2.nhsLastName AS nhsReciverName,nhsMessages.nhsMessageID AS nhsMessageID,nhsMessages.nhsMessageSender AS nhsMessageSender,nhsMessages.nhsMessageReciver AS nhsMessageReciver,nhsMessages.nhsMessageSubject AS nhsMessageSubject,nhsMessages.nhsMessageContent AS nhsMessageContent,nhsMessages.nhsMessageRead AS nhsMessageRead,nhsMessages.nhsDateSent AS nhsDateSent,YES AS nhsActive FROM nhsMembers AS m1, nhsMembers AS m2, nhsMessages WHERE (m1.nhsUserID=nhsMessages.nhsMessageSender AND m2.nhsUserID=nhsMessages.nhsMessageReciver) AND (nhsMessages.nhsMessageReciver=" + uid + " OR nhsMessages.nhsMessageSender=" + uid + ") AND (nhsMessages.nhsActive<> ':" + uid + ":' OR nhsMessages.nhsActive IS NULL)", "nhsMessages");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Message m = new Message()
                {
                    Id = int.Parse(dt.Rows[i]["nhsMessageID"].ToString().Trim()),
                    SenderId = int.Parse(dt.Rows[i]["nhsMessageSender"].ToString().Trim()),
                    ReciverId = int.Parse(dt.Rows[i]["nhsMessageReciver"].ToString().Trim()),
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
            var dt = Connect.GetData("SELECT * FROM nhsMessages", "nhsMessages");
            return (from DataRow dataRow in dt.Rows
                select new Message()
                {
                    Id = int.Parse(dataRow["nhsMessageID"].ToString().Trim()),
                    SenderId = int.Parse(dataRow["nhsMessageSender"].ToString().Trim()),
                    ReciverId = int.Parse(dataRow["nhsMessageReciver"].ToString().Trim()),
                    Subject = dataRow["nhsMessageSubject"].ToString().Trim(),
                    Content = dataRow["nhsMessageContent"].ToString().Trim(),
                    Read = Convert.ToBoolean(dataRow["nhsMessageRead"].ToString().Trim()),
                    SentDate = DateTime.Parse(dataRow["nhsDateSent"].ToString().Trim()),
                    State = dataRow["nhsActive"].ToString().Trim()
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
            var obj = Connect.GetObject("SELECT COUNT(*) FROM nhsMessages WHERE nhsMessageReciver=" + uid + " AND nhsMessageRead=NO AND (nhsActive<> ':" + uid + ":' OR nhsActive IS NULL)");
            return obj == null ? 0 : int.Parse(obj.ToString());
        }
        /// <summary>
        /// Get unread messages of user
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static List<Message> GetUnreaed(int uid)
        {
            var dt = Connect.GetData("SELECT m1.nhsFirstName +' '+ m1.nhsLastName AS nhsSenderName,m2.nhsFirstName + ' ' + m2.nhsLastName AS nhsReciverName,nhsMessages.nhsMessageID AS nhsMessageID,nhsMessages.nhsMessageSender AS nhsMessageSender,nhsMessages.nhsMessageReciver AS nhsMessageReciver,nhsMessages.nhsMessageSubject AS nhsMessageSubject,nhsMessages.nhsMessageContent AS nhsMessageContent,nhsMessages.nhsMessageRead AS nhsMessageRead,nhsMessages.nhsDateSent AS nhsDateSent,nhsMessages.nhsActive AS nhsActive FROM nhsMembers AS m1, nhsMembers AS m2, nhsMessages WHERE (m1.nhsUserID=nhsMessages.nhsMessageSender AND m2.nhsUserID=nhsMessages.nhsMessageReciver) AND (nhsMessages.nhsMessageReciver=" + uid + " OR nhsMessages.nhsMessageSender=" + uid + ") AND (nhsMessageRead=NO) AND (nhsMessages.nhsActive<> ':" + uid + ":' OR nhsMessages.nhsActive IS NULL)", "nhsMessages");
            return (from DataRow dataRow in dt.Rows
                select new Message()
                {
                    Id = int.Parse(dataRow["nhsMessageID"].ToString().Trim()),
                    SenderId = int.Parse(dataRow["nhsMessageSender"].ToString().Trim()),
                    ReciverId = int.Parse(dataRow["nhsMessageReciver"].ToString().Trim()),
                    Subject = dataRow["nhsMessageSubject"].ToString().Trim(),
                    Content = dataRow["nhsMessageContent"].ToString().Trim(),
                    Read = Convert.ToBoolean(dataRow["nhsMessageRead"].ToString().Trim()),
                    SentDate = DateTime.Parse(dataRow["nhsDateSent"].ToString().Trim()),
                    State = dataRow["nhsActive"].ToString().Trim(),
                    SenderName = dataRow["nhsSenderName"].ToString().Trim(),
                    ReciverName = dataRow["nhsReciverName"].ToString().Trim()
                }).ToList();
        }
    }
}
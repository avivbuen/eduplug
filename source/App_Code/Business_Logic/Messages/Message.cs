using System;

namespace Business_Logic.Messages
{
    /// <summary>
    /// City Database Structure
    /// </summary>
    public class Message
    {
        /// <summary>
        /// The id of the message
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The id of the reciver
        /// </summary>
        public int ReciverId { get; set; }
        /// <summary>
        /// The id of the sender
        /// </summary>
        public int SenderId { get; set; }
        /// <summary>
        /// The subject of the message
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// The content of the message
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// The read status of the message
        /// </summary>
        public bool Read { get; set; }
        /// <summary>
        /// The sent date
        /// </summary>
        public DateTime SentDate { get; set; }
        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// The name of the reciver
        /// </summary>
        public string ReciverName { get; set; }
        /// <summary>
        /// The name of the sender
        /// </summary>
        public string SenderName { get; set; }
        /// <summary>
        /// Guest
        /// </summary>
        public bool Guest { get; set; }
    }
}
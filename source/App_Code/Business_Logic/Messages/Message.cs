using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// City Database Structure
/// </summary>
public class Message
{
    /// <summary>
    /// The id of the message
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// The id of the reciver
    /// </summary>
    public int ReciverID { get; set; }
    /// <summary>
    /// The id of the sender
    /// </summary>
    public int SenderID { get; set; }
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
    public string ReciverName { get; set; }
    public string SenderName { get; set; }
    public bool Guest { get; set; }
}
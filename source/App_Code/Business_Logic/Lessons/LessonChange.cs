using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Lesson Change
/// </summary>
public class LessonChange
{
    /// <summary>
    /// The id of the lesson change
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// The id of the lesson
    /// </summary>
    public int LessonID { get; set; }
    /// <summary>
    /// The date of the change
    /// </summary>
    public DateTime Date { get; set; }
    /// <summary>
    /// The change type
    /// </summary>
    public LessonChangeType ChangeType { get; set; }
    /// <summary>
    /// The message for the change
    /// </summary>
    public string Message { get; set; }
}
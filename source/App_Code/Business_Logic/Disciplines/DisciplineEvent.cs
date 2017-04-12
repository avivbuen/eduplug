using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Discipline Event
/// </summary>
public class DisciplineEvent
{
    /// <summary>
    /// The id of the lesson
    /// </summary>
    public int LessonID { get; set; }
    /// <summary>
    /// The id of the disciplines type
    /// </summary>
    public int DisciplinesID { get; set; }
    /// <summary>
    /// The id of the student
    /// </summary>
    public int StudentID { get; set; }
    /// <summary>
    /// The date of the event
    /// </summary>
    public DateTime Date { get; set; }
}
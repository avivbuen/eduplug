using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LessonChange
/// </summary>
public class LessonChange
{
    public int LessonID { get; set; }
    public DateTime Date { get; set; }
    public LessonChangeType ChangeType { get; set; }
    public string Message { get; set; }
}
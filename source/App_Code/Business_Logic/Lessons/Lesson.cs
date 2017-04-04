using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Lesson
/// </summary>
public class Lesson
{
    public string Color { get; set; }
    public int ID { get; set; }
    public int Hour { get; set; }
    public int Day { get; set; }
    public int TeacherID { get; set; }
    public int TeacherGradeID { get; set; }
    public string Name { get; set; }
    public List<LessonChange> Changes { get; set; }
}
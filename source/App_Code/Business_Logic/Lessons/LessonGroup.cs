using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LessonGroup
/// </summary>
public class LessonGroup
{
    public List<Lesson> lessons;
    public LessonGroup()
    {
        lessons = new List<Lesson>();
    }
}
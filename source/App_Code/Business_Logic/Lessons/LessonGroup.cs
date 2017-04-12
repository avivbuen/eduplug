using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// LessonGroup
/// </summary>
public class LessonGroup
{
    /// <summary>
    /// The lessons
    /// </summary>
    public List<Lesson> lessons;
    /// <summary>
    /// CTOR
    /// </summary>
    public LessonGroup()
    {
        lessons = new List<Lesson>();
    }
}
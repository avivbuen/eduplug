using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Lesson
/// </summary>
public class Lesson
{
    /// <summary>
    /// The color in the table
    /// </summary>
    public string Color { get; set; }
    /// <summary>
    /// The id of the lesson
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// The hour of the lesson 1-12
    /// </summary>
    public int Hour { get; set; }
    /// <summary>
    /// The day of the lesson 1-6
    /// </summary>
    public int Day { get; set; }
    /// <summary>
    /// The id of the teacher
    /// </summary>
    public int TeacherID { get; set; }
    /// <summary>
    /// The id of the teacher grade
    /// </summary>
    public int TeacherGradeID { get; set; }
    /// <summary>
    /// The name of the grade
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// The changes in the system for the lesson
    /// </summary>
    public List<LessonChange> Changes { get; set; }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Exam
/// </summary>
public class Exam
{ 
    public int ID { get; set; }
    public string Title { get; set; }
    public int TeacherID { get; set; }
    public DateTime Date { get; set; }
    public int Precent { get; set; }
    public int TeacherGradeID { get; set; }
}
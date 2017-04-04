using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Score
/// </summary>
public class Score
{
    public int ID { get; set; }
    public Member Student { get; set; }
    public Exam Exam { get; set; }
    public int ScoreVal { get; set; }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Score
/// </summary>
public class Score
{
    /// <summary>
    /// The id of the score
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// The studetn
    /// </summary>
    public Member Student { get; set; }
    /// <summary>
    /// The exam
    /// </summary>
    public Exam Exam { get; set; }
    /// <summary>
    /// The value of the score
    /// </summary>
    public int ScoreVal { get; set; }

}
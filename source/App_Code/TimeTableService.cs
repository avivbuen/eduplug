using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Business_Logic;
using Business_Logic.Lessons;

/// <summary>
/// Summary description for TimeTableService
/// </summary>
[WebService(Namespace = "https://eduplug.co.il/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class TimeTableService : System.Web.Services.WebService
{

    public TimeTableService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    /// <summary>
    /// Gets all the weekly hours of the teacher
    /// </summary>
    /// <param name="teacherID">The id of the teacher</param>
    /// <returns>weekly hours of the teacher</returns>
    [WebMethod]
    public List<LessonGroup[]> GetByTeacherID(int teacherID)
    {
        return LessonService.GetTimeTable(teacherID, MemberClearance.Teacher);
    }
    /// <summary>
    /// Gets all the weekly hours of the student
    /// </summary>
    /// <param name="studentID">The id of the student</param>
    /// <returns></returns>
    [WebMethod]
    public List<LessonGroup[]> GetByStudentID(int studentID)
    {
        return LessonService.GetTimeTable(studentID, MemberClearance.Student);
    }
    /// <summary>
    /// Gets all the weekly hours of the grade part
    /// </summary>
    /// <param name="gradePart">Grade Part For e.g. יא'/יב'/י'/ט'/ח</param>
    /// <returns></returns>
    [WebMethod]
    public List<LessonGroup[]> GetByGradePart(string gradePart)
    {
        return LessonService.GetTimeTable(gradePart);
    }
    /// <summary>
    /// Creates a lesson for the teacher class with the teacher grade id
    /// </summary>
    /// <param name="day">Day 1-7</param>
    /// <param name="hour">Hour 1-12</param>
    /// <param name="tgid">Teacher Grade ID</param>
    [WebMethod]
    public void AddLesson(int day, int hour, int tgid)
    {
        
    }
}

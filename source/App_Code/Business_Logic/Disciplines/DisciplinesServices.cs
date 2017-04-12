using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// DisciplinesServices
/// </summary>
public static class DisciplinesServices
{
    /// <summary>
    /// Gets all the types
    /// </summary>
    /// <returns></returns>
    public static List<DisciplineType> GetAllTypes()
    {
        List<DisciplineType> types = new List<DisciplineType>();
        DataTable dt = Connect.GetData("SELECT * FROM nhsDisciplines", "nhsDisciplines");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DisciplineType c = new DisciplineType()
            {
                ID = int.Parse(dt.Rows[i]["nhsDisciplinesID"].ToString().Trim()),
                Name = dt.Rows[i]["nhsDisciplinesTitle"].ToString().Trim(),
                Score = int.Parse(dt.Rows[i]["nhsDisciplinesScore"].ToString().Trim())
            };
            types.Add(c);
        }
        return types;
    }
    /// <summary>
    /// Add new event
    /// </summary>
    /// <param name="lessonID">The id of the lesson</param>
    /// <param name="studentId">The user id of the student</param>
    /// <param name="disciplinesID">The disciplines type id</param>
    /// <param name="date">The date of the event</param>
    /// <returns></returns>
    public static bool Add(int lessonID, int studentId, int disciplinesID,DateTime date)
    {
        return Connect.InsertUpdateDelete("INSERT INTO nhsDisciplinesMembers (nhsLessonID,nhsDisciplinesID,nhsStudentID,nhsDate) VALUES(" + lessonID + "," + disciplinesID + "," + studentId + ",#" + Converter.GetTimeShortForDataBase(date) + "#)");
    }
    /// <summary>
    /// Get all the preselected items
    /// </summary>
    /// <param name="lessonID">The lesson id</param>
    /// <param name="date">The date of the lesson</param>
    /// <returns></returns>
    public static List<DisciplineEvent> GetSelected(int lessonID, DateTime date)
    {
        List<DisciplineEvent> events = new List<DisciplineEvent>();
        DataTable dt = Connect.GetData("SELECT * FROM nhsDisciplinesMembers WHERE nhsLessonID="+lessonID+" AND nhsDate=#"+Converter.GetTimeShortForDataBase(date)+"#" , "nhsDisciplinesMembers");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DisciplineEvent even = new DisciplineEvent()
            {
                StudentID = int.Parse(dt.Rows[i]["nhsStudentID"].ToString().Trim()),
                DisciplinesID = int.Parse(dt.Rows[i]["nhsDisciplinesID"].ToString().Trim())
            };

            events.Add(even);
        }
        return events;
    }
    /// <summary>
    /// Get student disciplines events by user id
    /// </summary>
    /// <param name="uid">User ID</param>
    /// <returns></returns>
    public static DataTable GetStudent(int uid)
    {
        DataTable dt = Connect.GetData("SELECT ds.nhsDisciplinesTitle AS dName, dsm.nhsDate AS dDate, tg.nhsTgradeName AS lName, tg.nhsTeacherID AS teacherId, ls.nhsHour AS dHour FROM nhsTeacherGrades AS tg, nhsDisciplinesMembers AS dsm,nhsDisciplines AS ds,nhsLessons AS ls WHERE dsm.nhsStudentID=" + uid+ " AND dsm.nhsDisciplinesID=ds.nhsDisciplinesID AND ls.nhsLessonID = dsm.nhsLessonID AND ls.nhsTgradeID=tg.nhsTgradeID ORDER BY dsm.nhsDate DESC", "nhsDisciplinesMembers");
        return dt;
    }
    /// <summary>
    /// Get student disciplines events by user id
    /// </summary>
    /// <param name="uid">User ID</param>
    /// <returns></returns>
    public static DataTable GetStudent(int uid,DateTime date)
    {
        DataTable dt = Connect.GetData("SELECT ds.nhsDisciplinesTitle AS dName, dsm.nhsDate AS dDate, tg.nhsTgradeName AS lName, tg.nhsTeacherID AS teacherId, ls.nhsHour AS dHour FROM nhsTeacherGrades AS tg, nhsDisciplinesMembers AS dsm,nhsDisciplines AS ds,nhsLessons AS ls WHERE dsm.nhsStudentID=" + uid + " AND dsm.nhsDisciplinesID=ds.nhsDisciplinesID AND ls.nhsLessonID = dsm.nhsLessonID AND ls.nhsTgradeID=tg.nhsTgradeID AND dsm.nhsDate > #"+Converter.GetTimeShortForDataBase(date)+ "# ORDER BY dsm.nhsDate DESC", "nhsDisciplinesMembers");
        return dt;
    }
    /// <summary>
    /// Get student disciplines events by user id
    /// </summary>
    /// <param name="uid">User ID</param>
    /// <returns></returns>
    public static DataTable GetStudent(int uid,int tgid)
    {
        DataTable dt = Connect.GetData("SELECT ds.nhsDisciplinesTitle AS dName, dsm.nhsDate AS dDate, tg.nhsTgradeName AS lName, tg.nhsTeacherID AS teacherId, ls.nhsHour AS dHour FROM nhsTeacherGrades AS tg, nhsDisciplinesMembers AS dsm,nhsDisciplines AS ds,nhsLessons AS ls WHERE dsm.nhsStudentID=" + uid + " AND dsm.nhsDisciplinesID=ds.nhsDisciplinesID AND ls.nhsLessonID = dsm.nhsLessonID AND ls.nhsTgradeID=tg.nhsTgradeID AND tg.nhsTgradeID="+tgid, "nhsDisciplinesMembers");
        return dt;
    }
    /// <summary>
    /// Reset lesson disciplines
    /// </summary>
    /// <param name="lessonID">LessonID</param>
    /// <param name="date">Date</param>
    public static void ResetLesson(int lessonID, DateTime date)
    {
        Connect.InsertUpdateDelete("DELETE FROM nhsDisciplinesMembers WHERE nhsLessonID=" + lessonID + " AND nhsDate =#" + Converter.GetTimeShortForDataBase(date) + "#");
    }
}
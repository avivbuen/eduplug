using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// Summary description for LessonService
/// </summary>
public static class LessonService
{
    public static int DaysInWeek = int.Parse(ConfigurationManager.AppSettings["DaysInWeek"].ToString());
    public static int LessonsInDay = int.Parse(ConfigurationManager.AppSettings["LessonsInDay"].ToString());
    /// <summary>
    /// Gets all the lessons for the following id.
    /// </summary>
    /// <param name="teacherId">The teacher id</param>
    /// <returns></returns>
    public static List<Lesson> GetAll(int teacherId)
    {
        DataTable dt = Connect.GetData("SELECT tg.nhsTgradeName AS LessonName, nhsLessonID AS LessonID, nhsLessons.nhsTgradeID AS TgradeID, nhsDay AS nhsDay, nhsHour AS nhsHour, tg.nhsTeacherID AS TeacherID FROM nhsTeacherGrades AS tg, nhsLessons WHERE tg.nhsTgradeID = nhsLessons.nhsTgradeID AND tg.nhsTeacherID=" + teacherId + " AND nhsActive=YES", "nhsLessons");
        List<Lesson> all = new List<Lesson>();
        foreach (DataRow dr in dt.Rows)
        {
            Lesson lesson = new Lesson()
            {
                Name = dr["LessonName"].ToString(),
                Day = int.Parse(dr["nhsDay"].ToString()),
                Hour = int.Parse(dr["nhsHour"].ToString()),
                ID = int.Parse(dr["LessonID"].ToString()),
                TeacherID = int.Parse(dr["TeacherID"].ToString())
            };
            all.Add(lesson);
        }
        return all;
    }
    /// <summary>
    /// Gets a lesson by lesson id
    /// </summary>
    /// <param name="lid">Lesson ID</param>
    /// <returns></returns>
    public static Lesson GetLesson(int lid)
    {
        DataTable dt = Connect.GetData("SELECT tg.nhsTgradeName AS LessonName, nhsLessonID AS LessonID, nhsLessons.nhsTgradeID AS TgradeID, nhsDay AS nhsDay, nhsHour AS nhsHour, tg.nhsTeacherID AS TeacherID FROM nhsTeacherGrades AS tg, nhsLessons WHERE tg.nhsTgradeID = nhsLessons.nhsTgradeID AND nhsLessonID=" + lid + " AND nhsActive=YES", "nhsLessons");
        if (dt.Rows.Count == 0)
            return null;
        DataRow dr = dt.Rows[0];
        Lesson lesson = new Lesson()
        {
            Name = dr["LessonName"].ToString(),
            Day = int.Parse(dr["nhsDay"].ToString()),
            Hour = int.Parse(dr["nhsHour"].ToString()),
            ID = int.Parse(dr["LessonID"].ToString()),
            TeacherID = int.Parse(dr["TeacherID"].ToString()),
            Changes = GetChanges(lid)
        };
        return lesson;
    }
    public static List<Member> GetAllStudents(int lid)
    {
        DataTable dt = Connect.GetData("SELECT nhsFirstName,nhsLastName,nhsUserID FROM nhsMembers AS student,nhsLearnGroups AS lgrps,nhsTeacherGrades AS tg, nhsLessons WHERE lgrps.nhsTgradeID = tg.nhsTgradeID AND lgrps.nhsStudentID=student.nhsUserID AND tg.nhsTgradeID = nhsLessons.nhsTgradeID AND nhsLessonID=" + lid, "nhsMembers");
        List<Member> students = new List<Member>();
        foreach (DataRow dr in dt.Rows)
        {
            Member m = new Member()
            {
                FirstName = dr["nhsFirstName"].ToString(),
                LastName = dr["nhsLastName"].ToString(),
                UserID = int.Parse(dr["nhsUserID"].ToString())
            };
            students.Add(m);
        }
        return students;
    }
    /// <summary>
    /// Helper method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="arrs"></param>
    /// <param name="rowIndex"></param>
    /// <returns></returns>
    private static T[] GetRow<T>(T[,] arrs, int rowIndex)
    {
        T[] row = new T[arrs.GetLength(1)];
        for (int i = 0; i < arrs.GetLength(1); i++)
        {
            row[i] = arrs[rowIndex, i];
        }
        return row;
    }
    /// <returns></returns>
    private static DataTable GetLessonsByTeacher(int tid)
    {
        return Connect.GetData("SELECT tg.nhsColor AS cellColor, tg.nhsTgradeName AS LessonName, nhsLessonID AS LessonID, nhsLessons.nhsTgradeID AS TgradeID, nhsDay AS nhsDay, nhsHour AS nhsHour, tg.nhsTeacherID AS TeacherID FROM nhsTeacherGrades AS tg, nhsLessons WHERE tg.nhsTgradeID = nhsLessons.nhsTgradeID AND tg.nhsTeacherID=" + tid + " AND tg.nhsDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND tg.nhsDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "# AND nhsActive=YES", "nhsLessons");
    }
    private static DataTable GetLessonsByStudent(int sid)
    {
        return Connect.GetData("SELECT tg.nhsColor AS cellColor, tg.nhsTgradeName AS LessonName, nhsLessonID AS LessonID, nhsLessons.nhsTgradeID AS TgradeID, nhsDay AS nhsDay, nhsHour AS nhsHour, tg.nhsTeacherID AS TeacherID FROM nhsTeacherGrades AS tg, nhsLessons, nhsLearnGroups WHERE tg.nhsTgradeID = nhsLessons.nhsTgradeID AND nhsLearnGroups.nhsTgradeID = tg.nhsTgradeID  AND nhsLearnGroups.nhsStudentID=" + sid + " AND tg.nhsDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND tg.nhsDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "# AND nhsActive=YES", "nhsLessons");
    }
    private static DataTable GetLessonsByGradePart(string gradePart)
    {
        return Connect.GetData("SELECT tg.nhsColor AS cellColor, tg.nhsTgradeName AS LessonName, lsons.nhsLessonID AS LessonID, lsons.nhsTgradeID AS TgradeID, lsons.nhsDay AS nhsDay, lsons.nhsHour AS nhsHour, tg.nhsTeacherID AS TeacherID FROM nhsLessons AS lsons INNER JOIN nhsTeacherGrades AS tg ON tg.nhsTgradeID = lsons.nhsTgradeID WHERE tg.nhsTgradeID IN( SELECT grp.nhsTgradeID FROM (nhsLearnGroups grp INNER JOIN nhsMembers AS mem ON mem.nhsUserID = grp.nhsStudentID) INNER JOIN nhsGrades AS grd ON mem.nhsGradeID = grd.nhsGradeID WHERE grd.nhsGradeName LIKE '%" + gradePart + "%' AND tg.nhsDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND tg.nhsDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#) AND nhsActive=YES", "nhsLessons");
    }
    /// <summary>
    /// Gets the time table of the grade that contains this value in its name
    /// </summary>
    /// <param name="gradePart">Value to check</param>
    /// <returns></returns>
    public static List<LessonGroup[]> GetTimeTable(string gradePart)
    {
        return GetDataTime(GetLessonsByGradePart(gradePart));
    }
    /// <summary>
    /// Gets the time table of the user
    /// </summary>
    /// <param name="uid">User ID</param>
    /// <param name="memTable">User Clearance</param>
    /// <returns></returns>
    public static List<LessonGroup[]> GetTimeTable(int uid, MemberClearance memTable)
    {
        DataTable dt = new DataTable();
        switch (memTable)
        {
            case MemberClearance.Student:
                dt = GetLessonsByStudent(uid);
                break;
            case MemberClearance.Teacher:
                dt = GetLessonsByTeacher(uid);
                break;
        }

        if (dt.Rows.Count == 0)
        {
            List<LessonGroup[]> lessData = new List<LessonGroup[]>();
            for (int i = 0; i < LessonsInDay; i++)
                lessData.Add(new LessonGroup[DaysInWeek]);
            return lessData;
        }
        return GetDataTime(dt);
    }
    private static List<LessonChange> GetChanges(int lid)
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsLessonChanges WHERE nhsLessonID=" + lid, "nhsLessonChanges");
        List<LessonChange> changeTable = new List<LessonChange>();
        foreach (DataRow dr in dt.Rows)
        {
            LessonChange change = new LessonChange()
            {
                ChangeType = ((LessonChangeType)(char.Parse(dr["nhsLessonChangeType"].ToString()))),
                Date = DateTime.Parse(dr["nhsDate"].ToString().Trim()),
                Message = dr["nhsMessage"].ToString().Trim(),
                LessonID = lid
            };
            changeTable.Add(change);
        }
        return changeTable;
    }
    private static List<LessonGroup[]> GetDataTime(DataTable dt)
    {
        LessonGroup[,] lessons = new LessonGroup[LessonsInDay, DaysInWeek];//Array that stores the time table
        for (int i = 0; i < lessons.GetLength(0); i++)//Array INIT
        {
            for (int j = 0; j < lessons.GetLength(1); j++)
                lessons[i, j] = null;
        }
        foreach (DataRow dr in dt.Rows)//Array Fill
        {
            Lesson lesson = new Lesson()
            {
                Name = dr["LessonName"].ToString(),
                Day = int.Parse(dr["nhsDay"].ToString()),
                Hour = int.Parse(dr["nhsHour"].ToString()),
                ID = int.Parse(dr["LessonID"].ToString()),
                TeacherID = int.Parse(dr["TeacherID"].ToString()),
                Color = dr["cellColor"].ToString(),
                TeacherGradeID = int.Parse(dr["TgradeID"].ToString()),
                Changes = GetChanges(int.Parse(dr["LessonID"].ToString()))
            };
            if (int.Parse(dr["nhsHour"].ToString()) > (LessonsInDay) || int.Parse(dr["nhsHour"].ToString()) <= 0 || int.Parse(dr["nhsDay"].ToString()) > DaysInWeek || int.Parse(dr["nhsDay"].ToString()) <= 0)
            {
                continue;
            }
            if (lessons[int.Parse(dr["nhsHour"].ToString()) - 1, int.Parse(dr["nhsDay"].ToString()) - 1] == null)
            {
                lessons[int.Parse(dr["nhsHour"].ToString()) - 1, int.Parse(dr["nhsDay"].ToString()) - 1] = new LessonGroup();
            }
            lessons[int.Parse(dr["nhsHour"].ToString()) - 1, int.Parse(dr["nhsDay"].ToString()) - 1].lessons.Add(lesson);
        }

        List<LessonGroup[]> lessData = new List<LessonGroup[]>();//Converting the 2D array to a list of 1D arrays(The conversion is made for the datalist)
        for (int i = 0; i < lessons.GetLength(0); i++)
            lessData.Add(GetRow(lessons, i));//Filling the new struct with the old one

        return lessData;//Returning the new struct
    }
    public static List<Lesson> GetLessons(int tgid)
    {
        DataTable dt = Connect.GetData("SELECT tg.nhsColor AS cellColor, tg.nhsTgradeName AS LessonName, nhsLessonID AS LessonID, nhsLessons.nhsTgradeID AS TgradeID, nhsDay AS nhsDay, nhsHour AS nhsHour, tg.nhsTeacherID AS TeacherID FROM nhsTeacherGrades AS tg, nhsLessons WHERE tg.nhsTgradeID = nhsLessons.nhsTgradeID AND tg.nhsTgradeID=" + tgid + " AND tg.nhsDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND tg.nhsDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "# AND nhsActive = YES", "nhsLessons");
        List<Lesson> lessons = new List<Lesson>();
        foreach (DataRow dr in dt.Rows)//Array Fill
        {
            Lesson lesson = new Lesson()
            {
                Name = dr["LessonName"].ToString(),
                Day = int.Parse(dr["nhsDay"].ToString()),
                Hour = int.Parse(dr["nhsHour"].ToString()),
                ID = int.Parse(dr["LessonID"].ToString()),
                TeacherID = int.Parse(dr["TeacherID"].ToString()),
                Color = dr["cellColor"].ToString(),
                TeacherGradeID = int.Parse(dr["TgradeID"].ToString()),
            };
            lessons.Add(lesson);
        }
        return lessons;
    }
    public static bool DeleteLesson(int tgid, int day, int hour)
    {
        return Connect.InsertUpdateDelete("UPDATE nhsLessons SET nhsActive=NO WHERE nhsTgradeID=" + tgid + " AND nhsDay=" + day + " AND nhsHour=" + hour);
    }
    public static bool DeleteLesson(int lid)
    {
        return Connect.InsertUpdateDelete("UPDATE nhsLessons SET nhsActive=NO WHERE nhsLessonID=" + lid);
    }
    public static bool Add(Lesson lsn)
    {
        tGrade t = tGradeService.Get(lsn.TeacherGradeID);
        if (t != null)
        {

            int rows = Connect.InsertUpdateDeleteState("UPDATE nhsLessons SET nhsActive=YES WHERE nhsTgradeID=" + lsn.TeacherGradeID + " AND nhsDay=" + lsn.Day + " AND nhsHour=" + lsn.Hour);
            if (rows == 0)
            {
                Connect.InsertUpdateDelete("INSERT INTO nhsLessons (nhsTgradeID,nhsDay,nhsHour,nhsActive) VALUES (" + lsn.TeacherGradeID + "," + lsn.Day + "," + lsn.Hour + ",YES)");
            }
            return true;
        }
        return false;  
    }
}
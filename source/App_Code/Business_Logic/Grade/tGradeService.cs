using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Drawing;
using System.Data.OleDb;
using System.Configuration;

/// <summary>
/// tGradeService
/// </summary>
public static class tGradeService
{
    /// <summary>
    /// Gets all the tgrades
    /// </summary>
    /// <returns></returns>
    public static List<tGrade> GetAll()
    {
        List<tGrade> grades = new List<tGrade>();
        DataTable dt = Connect.GetData("SELECT * FROM nhsTeacherGrades WHERE nhsDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND nhsDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "nhsTeacherGrades");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            tGrade c = new tGrade()
            {
                ID = int.Parse(dt.Rows[i]["nhsTgradeID"].ToString().Trim()),
                Name = dt.Rows[i]["nhsTgradeName"].ToString().Trim(),
                TeacherID = int.Parse(dt.Rows[i]["nhsTeacherID"].ToString().Trim())
            };
            grades.Add(c);
        }
        return grades;
    }
    /// <summary>
    /// Removes tgrade by id
    /// </summary>
    /// <param name="tgid"></param>
    /// <returns></returns>
    public static bool Remove(int tgid)
    {
        return Connect.InsertUpdateDelete("DELETE FROM nhsLearnGroups WHERE nhsTgradeID=" + tgid) && Connect.InsertUpdateDelete("DELETE FROM nhsExams WHERE nhsTgradeID=" + tgid) && Connect.InsertUpdateDelete("DELETE FROM nhsTeacherGrades WHERE nhsTgradeID=" + tgid);
    }
    /// <summary>
    /// Get id by obj
    /// </summary>
    /// <param name="grd">tGrade OBJ</param>
    /// <returns></returns>
    public static int GetID(tGrade grd)
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsTeacherGrades WHERE nhsTeacherID=" + grd.TeacherID + " AND nhsTgradeName='" + grd.Name + "' AND nhsDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND nhsDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "nhsTeacherGrades");
        if (dt.Rows.Count < 1)
        {
            return -1;
        }
        return int.Parse(dt.Rows[0]["nhsTgradeID"].ToString().Trim());
    }
    /// <summary>
    /// Add new tGrade to DB
    /// </summary>
    /// <param name="grd">tGrade</param>
    /// <returns></returns>
    public static bool Add(tGrade grd)
    {
        int count = (int)Connect.GetObject("SELECT COUNT(*) FROM nhsTeacherGrades WHERE nhsTeacherID=" + grd.TeacherID + " AND nhsTgradeName='" + grd.Name + "'");
        if (count >= 1)
        {
            return true;
        }
        Random rnd = new Random();
        Color a = Color.FromArgb(rnd.Next(50, 256), rnd.Next(50, 256), rnd.Next(50, 256));
        string str = ColorTranslator.ToHtml(a).Substring(1);
        int countColor = (int)Connect.GetObject("SELECT COUNT(*) FROM nhsTeacherGrades WHERE nhsColor='" + str + "'");
        while (countColor > 0)
        {
            a = Color.FromArgb(rnd.Next(50, 256), rnd.Next(50, 256), rnd.Next(50, 256));
            str = ColorTranslator.ToHtml(a).Substring(1);
            countColor = (int)Connect.GetObject("SELECT COUNT(*) FROM nhsTeacherGrades WHERE nhsColor='" + str + "'");
        }
        return Connect.InsertUpdateDelete("INSERT INTO nhsTeacherGrades(nhsTeacherID,nhsTgradeName,nhsColor, nhsDate) VALUES(" + grd.TeacherID + ",'" + grd.Name + "','" + str + "', #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "#)");
    }
    /// <summary>
    /// Add students to tgarde
    /// </summary>
    /// <param name="tgid">Tgrade ID</param>
    /// <param name="students">List of students</param>
    /// <returns></returns>
    public static bool AddStudents(int tgid, List<Member> students)
    {
        List<Member> all = GetStudents(tgid);
        Connect.InsertUpdateDelete("DELETE FROM nhsLearnGroups WHERE nhsTgradeID=" + tgid);

        foreach (Member student in students)
        {
            if (all.Where(x => x.UserID == student.UserID).Count() == 0)
            {
                ScoreService.ResetScoresStudent(student.UserID, tgid);
            }
            Connect.InsertUpdateDelete("INSERT INTO nhsLearnGroups(nhsTgradeID,nhsStudentID,nhsDate) VALUES(" + tgid + "," + student.UserID + ",#" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "#)");
        }
        return true;
    }
    /// <summary>
    /// Add student to tgarde
    /// </summary>
    public static bool AddStudent(int tgid, Member student)
    {
        Connect.InsertUpdateDelete("INSERT INTO nhsLearnGroups(nhsTgradeID,nhsStudentID,nhsDate) VALUES(" + tgid + "," + student.UserID + ",#" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "#)");
        return true;
    }
    /// <summary>
    /// Add student to tgarde
    /// </summary>
    public static bool AddStudent(int tgid, int uid)
    {
        Connect.InsertUpdateDelete("INSERT INTO nhsLearnGroups(nhsTgradeID,nhsStudentID,nhsDate) VALUES(" + tgid + "," + uid + ",#" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "#)");
        return true;
    }
    /// <summary>
    /// Update tgrade
    /// </summary>
    /// <param name="tg"></param>
    /// <returns></returns>
    public static bool Update(tGrade tg)
    {
        return Connect.InsertUpdateDelete("UPDATE nhsTeacherGrades SET nhsTeacherID=" + tg.TeacherID + ",nhsTgradeName='" + tg.Name + "',nhsDate = #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# WHERE nhsTgradeID=" + tg.ID);
    }
    /// <summary>
    /// Get part grade of tgarde
    /// </summary>
    /// <param name="tgid">tgrade id</param>
    /// <returns></returns>
    public static string GetPartGrade(int tgid)
    {
        DataTable dt = Connect.GetData("SELECT grade.nhsGradeName AS GradeName FROM nhsMembers AS m, nhsGrades AS grade, nhsLearnGroups AS lg WHERE lg.nhsStudentID = m.nhsUserID AND grade.nhsGradeID = m.nhsGradeID AND lg.nhsTgradeID=" + tgid, "nhsLearnGroup");
        if (dt.Rows.Count < 1)
        {
            return "";
        }
        string gname = dt.Rows[0]["GradeName"].ToString();
        return GetPartGrade(gname);
    }
    /// <summary>
    /// Get part grade
    /// </summary>
    /// <param name="gname">grade</param>
    /// <returns></returns>
    public static string GetPartGrade(string gname)
    {
        if (gname.Contains("ז'"))
        {
            return "ז'";
        }
        else if (gname.Contains("ח'"))
        {
            return "ח'";
        }
        else if (gname.Contains("ט'"))
        {
            return "ט'";
        }
        else if (gname.Contains("י'"))
        {
            return "י'";
        }
        else if (gname.Contains("יא'"))
        {
            return "יא'";
        }
        else if (gname.Contains("יב'"))
        {
            return "יב'";
        }
        return "";
    }
    /// <summary>
    /// Get tgrade by id
    /// </summary>
    /// <param name="tgid">tGrade id</param>
    /// <returns></returns>
    public static tGrade Get(int tgid)
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsTeacherGrades WHERE nhsTgradeID=" + tgid, "nhsTeacherGrades");
        if (dt.Rows.Count == 0)
        {
            return null;
        }
        tGrade c = new tGrade()
        {
            ID = int.Parse(dt.Rows[0]["nhsTgradeID"].ToString().Trim()),
            Name = dt.Rows[0]["nhsTgradeName"].ToString().Trim(),
            TeacherID = int.Parse(dt.Rows[0]["nhsTeacherID"].ToString().Trim())
        };
        return c;
    }
    /// <summary>
    /// Get tgrades of teacher
    /// </summary>
    /// <param name="tid"></param>
    /// <returns></returns>
    public static List<tGrade> GetTeacherTgrades(int tid)
    {
        List<tGrade> grades = new List<tGrade>();
        DataTable dt = Connect.GetData("SELECT m.nhsUserID AS TeacherID, nhsTgradeID AS TgradeID, nhsTgradeName AS TgradeName FROM  nhsMembers AS m, nhsTeacherGrades WHERE m.nhsUserID = nhsTeacherID AND nhsTeacherID=" + tid + " AND nhsTeacherGrades.nhsDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND nhsTeacherGrades.nhsDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "nhsTeacherGrades");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            tGrade c = new tGrade()
            {
                ID = int.Parse(dt.Rows[i]["TgradeID"].ToString().Trim()),
                Name = dt.Rows[i]["TgradeName"].ToString().Trim(),
                TeacherID = int.Parse(dt.Rows[i]["TeacherID"].ToString().Trim())
            };
            grades.Add(c);
        }
        return grades;
    }
    /// <summary>
    /// Get student count of tgrade
    /// </summary>
    /// <param name="tgid"></param>
    /// <returns></returns>
    public static int GetStudentCount(int tgid)
    {
        return (int)Connect.GetObject("SELECT COUNT(*) FROM nhsLearnGroups WHERE nhsTgradeID=" + tgid);
    }
    /// <summary>
    /// Get students of tgrade
    /// </summary>
    /// <param name="tgid"></param>
    /// <returns></returns>
    public static List<Member> GetStudents(int tgid)
    {
        List<Member> mems = new List<Member>();
        DataTable dt = Connect.GetData("SELECT m.nhsFirstName + ' ' + m.nhsLastName AS nhsStudentName,m.nhsUserID AS nhsStudentID  FROM nhsMembers AS m, nhsLearnGroups WHERE m.nhsUserID = nhsLearnGroups.nhsStudentID AND nhsTgradeID=" + tgid, "nhsLearnGroups");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Member c = new Member()
            {
                UserID = int.Parse(dt.Rows[i]["nhsStudentID"].ToString().Trim()),
                Name = dt.Rows[i]["nhsStudentName"].ToString().Trim()
            };
            mems.Add(c);
        }
        return mems;
    }
    /// <summary>
    /// Get the major of tgrade
    /// </summary>
    /// <param name="tgid"></param>
    /// <returns></returns>
    public static int GetMajor(int tgid)
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsMajorsTgrades WHERE nhsTgradeID=" + tgid, "nhsMajorsTgrades");
        if (dt.Rows.Count == 0)
            return -1;
        return int.Parse(dt.Rows[0]["nhsMajorID"].ToString().Trim());
    }
}
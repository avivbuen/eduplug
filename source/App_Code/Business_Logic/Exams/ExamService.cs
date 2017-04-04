using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ExamService
/// </summary>
public static class ExamService
{
    public static List<Exam> GetAll()
    {
        List<Exam> Exams = new List<Exam>();
        DataTable dt = Connect.GetData("SELECT * FROM nhsExams", "nhsExams");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Exam c = new Exam()
            {
                ID = int.Parse(dt.Rows[i]["nhsExamID"].ToString().Trim()),
                Title = dt.Rows[i]["nhsExamTitle"].ToString().Trim(),
                TeacherID = int.Parse(dt.Rows[i]["nhsTeacherID"].ToString().Trim()),
                Date = DateTime.Parse(dt.Rows[i]["nhsExamDate"].ToString().Trim()),
                TeacherGradeID = int.Parse(dt.Rows[i]["nhsTgradeID"].ToString().Trim()),
                Precent = int.Parse(dt.Rows[0]["nhsPrecent"].ToString().Trim())
            };
            Exams.Add(c);
        }
        return Exams;
    }
    public static bool Add(Exam exm, int tgid)
    {
        if (exm.Date < EduSysDate.GetStart() || exm.Date > EduSysDate.GetEnd())
            return false;
        if (MemberService.GetUser(exm.TeacherID) == null)
            return false;
        return Connect.InsertUpdateDelete("INSERT INTO nhsExams (nhsTeacherID,nhsExamDate,nhsPrecent,nhsTgradeID,nhsExamTitle,nhsYearPart) VALUES (" + exm.TeacherID + ",#" + Converter.GetTimeShortForDataBase(exm.Date) + "#," + exm.Precent + "," + tgid + ",'" + exm.Title + "','" + EduSysDate.GetYearPart(exm.Date) + "')");
    }
    public static Exam GetExam(int eid)
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsExams WHERE nhsExamID=" + eid + " AND nhsExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND nhsExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "nhsExams");
        if (dt.Rows.Count == 1)
        {
            Exam c = new Exam()
            {
                ID = int.Parse(dt.Rows[0]["nhsExamID"].ToString().Trim()),
                Title = dt.Rows[0]["nhsExamTitle"].ToString().Trim(),
                TeacherID = int.Parse(dt.Rows[0]["nhsTeacherID"].ToString().Trim()),
                Date = DateTime.Parse(dt.Rows[0]["nhsExamDate"].ToString().Trim()),
                TeacherGradeID = int.Parse(dt.Rows[0]["nhsTgradeID"].ToString().Trim()),
                Precent = int.Parse(dt.Rows[0]["nhsPrecent"].ToString().Trim())
            };
            return c;
        }
        return null;
    }
    public static bool Delete(int eid)
    {
        return Connect.InsertUpdateDelete("DELETE FROM nhsScores WHERE nhsExamID=" + eid) && Connect.InsertUpdateDelete("DELETE FROM nhsExams WHERE nhsExamID=" + eid);
    }
    public static int PrecentLeft(int tgid)
    {
        int count = int.Parse(Connect.GetObject("SELECT COUNT (*) FROM nhsExams WHERE nhsTgradeID=" + tgid + " AND nhsExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND nhsExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#").ToString());
        if (count == 0)
        {
            return 100;
        }
        DataTable dt = Connect.GetData("SELECT SUM(nhsPrecent) AS PSu FROM nhsExams WHERE nhsTgradeID=" + tgid + " AND nhsExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND nhsExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "# AND nhsYearPart='a'", "nhsExams");
        string str = dt.Rows[0]["PSu"].ToString();
        if (str == "") return 100;
        return 100 - int.Parse(str);
    }
    public static int PrecentLeft(int tgid,string yearPart)
    {
        object obj = Connect.GetObject("SELECT COUNT (*) FROM nhsExams WHERE nhsYearPart='" + yearPart + "' AND nhsTgradeID=" + tgid + " AND nhsExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND nhsExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#");
        if (obj == null) return 100;
        int count = int.Parse(obj.ToString());
        if (count == 0)
        {
            return 100;
        }
        DataTable dt = Connect.GetData("SELECT SUM(nhsPrecent) AS PSu FROM nhsExams WHERE nhsYearPart='" + yearPart + "' AND nhsTgradeID=" + tgid + " AND nhsExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND nhsExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "# ", "nhsExams");
        string str = dt.Rows[0]["PSu"].ToString();
        if (str == "") return 100;
        return 100 - int.Parse(str);
    }
    public static List<Exam> GetExamsByTgradeID(int tgid)
    {
        List<Exam> Exams = new List<Exam>();
        DataTable dt = Connect.GetData("SELECT * FROM nhsExams WHERE nhsTgradeID=" + tgid + " AND nhsExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND nhsExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "nhsExams");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Exam c = new Exam()
            {
                ID = int.Parse(dt.Rows[i]["nhsExamID"].ToString().Trim()),
                Title = dt.Rows[i]["nhsExamTitle"].ToString().Trim(),
                TeacherID = int.Parse(dt.Rows[i]["nhsTeacherID"].ToString().Trim()),
                Date = DateTime.Parse(dt.Rows[i]["nhsExamDate"].ToString().Trim()),
                TeacherGradeID = int.Parse(dt.Rows[i]["nhsTgradeID"].ToString().Trim()),
                Precent = int.Parse(dt.Rows[0]["nhsPrecent"].ToString().Trim())
            };
            Exams.Add(c);
        }
        return Exams;
    }
    public static List<Exam> GetExamsByTgradeID(int tgid,string yearPart)
    {
        List<Exam> Exams = new List<Exam>();
        DataTable dt = Connect.GetData("SELECT * FROM nhsExams WHERE nhsYearPart='"+yearPart+"' AND nhsTgradeID=" + tgid + " AND nhsExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND nhsExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "nhsExams");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Exam c = new Exam()
            {
                ID = int.Parse(dt.Rows[i]["nhsExamID"].ToString().Trim()),
                Title = dt.Rows[i]["nhsExamTitle"].ToString().Trim(),
                TeacherID = int.Parse(dt.Rows[i]["nhsTeacherID"].ToString().Trim()),
                Date = DateTime.Parse(dt.Rows[i]["nhsExamDate"].ToString().Trim()),
                TeacherGradeID = int.Parse(dt.Rows[i]["nhsTgradeID"].ToString().Trim()),
                Precent = int.Parse(dt.Rows[0]["nhsPrecent"].ToString().Trim())
            };
            Exams.Add(c);
        }
        return Exams;
    }
    public static bool Update(Exam exam)
    {
        return Connect.InsertUpdateDelete("UPDATE nhsExams SET nhsExamTitle='" + exam.Title + "',nhsExamDate=#" + Converter.GetTimeShortForDataBase(exam.Date) + "#,nhsPrecent=" + exam.Precent + " WHERE nhsExamID=" + exam.ID);
    }
}
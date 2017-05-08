using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Business_Logic.Members;

namespace Business_Logic.Exams
{
    /// <summary>
    /// ExamService
    /// </summary>
    public static class ExamService
    {
        /// <summary>
        /// Get all exams
        /// </summary>
        /// <returns>List of Exams</returns>
        public static List<Exam> GetAll()
        {
            var exams = new List<Exam>();
            var dt = Connect.GetData("SELECT * FROM eduExams", "eduExams");
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var c = new Exam()
                {
                    Id = int.Parse(dt.Rows[i]["eduExamID"].ToString().Trim()),
                    Title = dt.Rows[i]["eduExamTitle"].ToString().Trim(),
                    TeacherId = int.Parse(dt.Rows[i]["eduTeacherID"].ToString().Trim()),
                    Date = DateTime.Parse(dt.Rows[i]["eduExamDate"].ToString().Trim()),
                    TeacherGradeId = int.Parse(dt.Rows[i]["eduTgradeID"].ToString().Trim()),
                    Precent = int.Parse(dt.Rows[0]["eduPrecent"].ToString().Trim())
                };
                exams.Add(c);
            }
            return exams;
        }
        /// <summary>
        /// Add new exam
        /// </summary>
        /// <param name="exm">Exam obj</param>
        /// <param name="tgid">Teacher grade id</param>
        /// <returns>success</returns>
        public static bool Add(Exam exm, int tgid)
        {
            if (exm.Date < EduSysDate.GetStart() || exm.Date > EduSysDate.GetEnd())
                return false;
            return MemberService.GetUser(exm.TeacherId) != null && Connect.InsertUpdateDelete("INSERT INTO eduExams (eduTeacherID,eduExamDate,eduPrecent,eduTgradeID,eduExamTitle,eduYearPart) VALUES (" + exm.TeacherId + ",#" + Converter.GetTimeShortForDataBase(exm.Date) + "#," + exm.Precent + "," + tgid + ",'" + exm.Title + "','" + EduSysDate.GetYearPart(exm.Date) + "')");
        }
        /// <summary>
        /// Get exam by id
        /// </summary>
        /// <param name="eid">Exam id</param>
        /// <returns>Exam</returns>
        public static Exam GetExam(int eid)
        {
            var dt = Connect.GetData("SELECT * FROM eduExams WHERE eduExamID=" + eid + " AND eduExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND eduExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "eduExams");
            if (dt.Rows.Count != 1) return null;
            var c = new Exam()
            {
                Id = int.Parse(dt.Rows[0]["eduExamID"].ToString().Trim()),
                Title = dt.Rows[0]["eduExamTitle"].ToString().Trim(),
                TeacherId = int.Parse(dt.Rows[0]["eduTeacherID"].ToString().Trim()),
                Date = DateTime.Parse(dt.Rows[0]["eduExamDate"].ToString().Trim()),
                TeacherGradeId = int.Parse(dt.Rows[0]["eduTgradeID"].ToString().Trim()),
                Precent = int.Parse(dt.Rows[0]["eduPrecent"].ToString().Trim())
            };
            return c;
        }
        /// <summary>
        /// Get exam id by exam object
        /// </summary>
        /// <param name="exm">Exam</param>
        /// <returns></returns>
        public static int GetExamId(Exam exm)
        {
            var dt = Connect.GetData("SELECT * FROM eduExams WHERE eduExamTitle='" + exm.Title + "' AND eduTeacherID="+exm.TeacherId+" AND eduExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND eduExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "eduExams");
            if (dt.Rows.Count == 1)
            {
                return int.Parse(dt.Rows[0]["eduExamID"].ToString().Trim());
            }
            return -1;
        }
        /// <summary>
        /// Delete exam
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public static bool Delete(int eid)
        {
            return Connect.InsertUpdateDelete("DELETE FROM eduScores WHERE eduExamID=" + eid) && Connect.InsertUpdateDelete("DELETE FROM eduExams WHERE eduExamID=" + eid);
        }
        /// <summary>
        /// Get precent left of teacher grade - irelavent
        /// </summary>
        /// <param name="tgid"></param>
        /// <returns></returns>
        public static int PrecentLeft(int tgid)
        {
            int count = int.Parse(Connect.GetObject("SELECT COUNT (*) FROM eduExams WHERE eduTgradeID=" + tgid + " AND eduExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND eduExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#").ToString());
            if (count == 0)
            {
                return 100;
            }
            DataTable dt = Connect.GetData("SELECT SUM(eduPrecent) AS PSu FROM eduExams WHERE eduTgradeID=" + tgid + " AND eduExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND eduExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "# AND eduYearPart='a'", "eduExams");
            string str = dt.Rows[0]["PSu"].ToString();
            if (str == "") return 100;
            return 100 - int.Parse(str);
        }
        /// <summary>
        /// Get precent left of teacher grade 
        /// </summary>
        /// <param name="tgid">teacher grade id</param>
        /// /// <param name="yearPart">year part</param>
        /// <returns></returns>
        public static int PrecentLeft(int tgid,string yearPart)
        {
            object obj = Connect.GetObject("SELECT COUNT (*) FROM eduExams WHERE eduYearPart='" + yearPart + "' AND eduTgradeID=" + tgid + " AND eduExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND eduExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#");
            if (obj == null) return 100;
            int count = int.Parse(obj.ToString());
            if (count == 0)
            {
                return 100;
            }
            DataTable dt = Connect.GetData("SELECT SUM(eduPrecent) AS PSu FROM eduExams WHERE eduYearPart='" + yearPart + "' AND eduTgradeID=" + tgid + " AND eduExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND eduExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "# ", "eduExams");
            string str = dt.Rows[0]["PSu"].ToString();
            if (str == "") return 100;
            return 100 - int.Parse(str);
        }
        /// <summary>
        /// Get exams by TeacherGrade id
        /// </summary>
        /// <param name="tgid">Teacher grade id</param>
        /// <returns></returns>
        public static List<Exam> GetExamsByTeacherGradeId(int tgid)
        {
            var dt = Connect.GetData("SELECT * FROM eduExams WHERE eduTgradeID=" + tgid + " AND eduExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND eduExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "eduExams");
            return (from DataRow dataRow in dt.Rows
                select new Exam()
                {
                    Id = int.Parse(dataRow["eduExamID"].ToString().Trim()),
                    Title = dataRow["eduExamTitle"].ToString().Trim(),
                    TeacherId = int.Parse(dataRow["eduTeacherID"].ToString().Trim()),
                    Date = DateTime.Parse(dataRow["eduExamDate"].ToString().Trim()),
                    TeacherGradeId = int.Parse(dataRow["eduTgradeID"].ToString().Trim()),
                    Precent = int.Parse(dataRow["eduPrecent"].ToString().Trim())
                }).ToList();
        }
        /// <summary>
        /// Get exams by TeacherGrade id
        /// </summary>
        /// <param name="tgid">Teacher grade id</param>
        /// <param name="yearPart">Year part</param>
        /// <returns></returns>
        public static List<Exam> GetExamsByTeacherGradeId(int tgid,string yearPart)
        {
            var dt = Connect.GetData("SELECT * FROM eduExams WHERE eduYearPart='"+yearPart+"' AND eduTgradeID=" + tgid + " AND eduExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND eduExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "eduExams");
            return (from DataRow dataRow in dt.Rows
                select new Exam()
                {
                    Id = int.Parse(dataRow["eduExamID"].ToString().Trim()),
                    Title = dataRow["eduExamTitle"].ToString().Trim(),
                    TeacherId = int.Parse(dataRow["eduTeacherID"].ToString().Trim()),
                    Date = DateTime.Parse(dataRow["eduExamDate"].ToString().Trim()),
                    TeacherGradeId = int.Parse(dataRow["eduTgradeID"].ToString().Trim()),
                    Precent = int.Parse(dataRow["eduPrecent"].ToString().Trim())
                }).ToList();
        }
        /// <summary>
        /// Update exam in DB
        /// </summary>
        /// <param name="exam">Exam</param>
        public static bool Update(Exam exam)
        {
            return Connect.InsertUpdateDelete("UPDATE eduExams SET eduExamTitle='" + exam.Title + "',eduExamDate=#" + Converter.GetTimeShortForDataBase(exam.Date) + "#,eduPrecent=" + exam.Precent + " WHERE eduExamID=" + exam.Id);
        }
    }
}
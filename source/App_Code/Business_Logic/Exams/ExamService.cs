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
            var dt = Connect.GetData("SELECT * FROM nhsExams", "nhsExams");
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var c = new Exam()
                {
                    Id = int.Parse(dt.Rows[i]["nhsExamID"].ToString().Trim()),
                    Title = dt.Rows[i]["nhsExamTitle"].ToString().Trim(),
                    TeacherId = int.Parse(dt.Rows[i]["nhsTeacherID"].ToString().Trim()),
                    Date = DateTime.Parse(dt.Rows[i]["nhsExamDate"].ToString().Trim()),
                    TeacherGradeId = int.Parse(dt.Rows[i]["nhsTgradeID"].ToString().Trim()),
                    Precent = int.Parse(dt.Rows[0]["nhsPrecent"].ToString().Trim())
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
            return MemberService.GetUser(exm.TeacherId) != null && Connect.InsertUpdateDelete("INSERT INTO nhsExams (nhsTeacherID,nhsExamDate,nhsPrecent,nhsTgradeID,nhsExamTitle,nhsYearPart) VALUES (" + exm.TeacherId + ",#" + Converter.GetTimeShortForDataBase(exm.Date) + "#," + exm.Precent + "," + tgid + ",'" + exm.Title + "','" + EduSysDate.GetYearPart(exm.Date) + "')");
        }
        /// <summary>
        /// Get exam by id
        /// </summary>
        /// <param name="eid">Exam id</param>
        /// <returns>Exam</returns>
        public static Exam GetExam(int eid)
        {
            var dt = Connect.GetData("SELECT * FROM nhsExams WHERE nhsExamID=" + eid + " AND nhsExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND nhsExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "nhsExams");
            if (dt.Rows.Count != 1) return null;
            var c = new Exam()
            {
                Id = int.Parse(dt.Rows[0]["nhsExamID"].ToString().Trim()),
                Title = dt.Rows[0]["nhsExamTitle"].ToString().Trim(),
                TeacherId = int.Parse(dt.Rows[0]["nhsTeacherID"].ToString().Trim()),
                Date = DateTime.Parse(dt.Rows[0]["nhsExamDate"].ToString().Trim()),
                TeacherGradeId = int.Parse(dt.Rows[0]["nhsTgradeID"].ToString().Trim()),
                Precent = int.Parse(dt.Rows[0]["nhsPrecent"].ToString().Trim())
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
            var dt = Connect.GetData("SELECT * FROM nhsExams WHERE nhsExamTitle='" + exm.Title + "' AND nhsTeacherID="+exm.TeacherId+" AND nhsExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND nhsExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "nhsExams");
            if (dt.Rows.Count == 1)
            {
                return int.Parse(dt.Rows[0]["nhsExamID"].ToString().Trim());
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
            return Connect.InsertUpdateDelete("DELETE FROM nhsScores WHERE nhsExamID=" + eid) && Connect.InsertUpdateDelete("DELETE FROM nhsExams WHERE nhsExamID=" + eid);
        }
        /// <summary>
        /// Get precent left of teacher grade - irelavent
        /// </summary>
        /// <param name="tgid"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Get precent left of teacher grade 
        /// </summary>
        /// <param name="tgid">teacher grade id</param>
        /// /// <param name="yearPart">year part</param>
        /// <returns></returns>
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
        /// <summary>
        /// Get exams by TeacherGrade id
        /// </summary>
        /// <param name="tgid">Teacher grade id</param>
        /// <returns></returns>
        public static List<Exam> GetExamsByTeacherGradeId(int tgid)
        {
            var dt = Connect.GetData("SELECT * FROM nhsExams WHERE nhsTgradeID=" + tgid + " AND nhsExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND nhsExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "nhsExams");
            return (from DataRow dataRow in dt.Rows
                select new Exam()
                {
                    Id = int.Parse(dataRow["nhsExamID"].ToString().Trim()),
                    Title = dataRow["nhsExamTitle"].ToString().Trim(),
                    TeacherId = int.Parse(dataRow["nhsTeacherID"].ToString().Trim()),
                    Date = DateTime.Parse(dataRow["nhsExamDate"].ToString().Trim()),
                    TeacherGradeId = int.Parse(dataRow["nhsTgradeID"].ToString().Trim()),
                    Precent = int.Parse(dataRow["nhsPrecent"].ToString().Trim())
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
            var dt = Connect.GetData("SELECT * FROM nhsExams WHERE nhsYearPart='"+yearPart+"' AND nhsTgradeID=" + tgid + " AND nhsExamDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND nhsExamDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "nhsExams");
            return (from DataRow dataRow in dt.Rows
                select new Exam()
                {
                    Id = int.Parse(dataRow["nhsExamID"].ToString().Trim()),
                    Title = dataRow["nhsExamTitle"].ToString().Trim(),
                    TeacherId = int.Parse(dataRow["nhsTeacherID"].ToString().Trim()),
                    Date = DateTime.Parse(dataRow["nhsExamDate"].ToString().Trim()),
                    TeacherGradeId = int.Parse(dataRow["nhsTgradeID"].ToString().Trim()),
                    Precent = int.Parse(dataRow["nhsPrecent"].ToString().Trim())
                }).ToList();
        }
        /// <summary>
        /// Update exam in DB
        /// </summary>
        /// <param name="exam">Exam</param>
        public static bool Update(Exam exam)
        {
            return Connect.InsertUpdateDelete("UPDATE nhsExams SET nhsExamTitle='" + exam.Title + "',nhsExamDate=#" + Converter.GetTimeShortForDataBase(exam.Date) + "#,nhsPrecent=" + exam.Precent + " WHERE nhsExamID=" + exam.Id);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Business_Logic.Members;
using Business_Logic.Scores;

namespace Business_Logic.TeacherGrades
{
    /// <summary>
    /// TeacherGradeService
    /// </summary>
    public static class TeacherGradeService
    {
        /// <summary>
        /// Gets all the TeacherGrades
        /// </summary>
        /// <returns></returns>
        public static List<TeacherGrade> GetAll()
        {
            var dt = Connect.GetData("SELECT * FROM eduTeacherGrades WHERE eduSchoolID="+MemberService.GetCurrent().School.Id+" AND eduDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND eduDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "eduTeacherGrades");
            return (from DataRow dataRow in dt.Rows
                select new TeacherGrade()
                {
                    Id = int.Parse(dataRow["eduTgradeID"].ToString().Trim()),
                    Name = dataRow["eduTgradeName"].ToString().Trim(),
                    TeacherId = int.Parse(dataRow["eduTeacherID"].ToString().Trim())
                }).ToList();
        }
        /// <summary>
        /// Removes TeacherGrade by id
        /// </summary>
        /// <param name="tgid"></param>
        /// <returns></returns>
        public static bool Remove(int tgid)
        {
            return Connect.InsertUpdateDelete("DELETE FROM eduLearnGroups WHERE eduTgradeID=" + tgid)&& Connect.InsertUpdateDelete("DELETE FROM eduMajorsTgrades WHERE eduTgradeID=" + tgid) && Connect.InsertUpdateDelete("DELETE FROM eduExams WHERE eduTgradeID=" + tgid) && Connect.InsertUpdateDelete("DELETE FROM eduTeacherGrades WHERE eduTgradeID=" + tgid);
        }
        /// <summary>
        /// Get id by obj
        /// </summary>
        /// <param name="grd">TeacherGrade OBJ</param>
        /// <returns></returns>
        public static int GetId(TeacherGrade grd)
        {
            var dt = Connect.GetData("SELECT * FROM eduTeacherGrades WHERE eduTeacherID=" + grd.TeacherId + " AND eduTgradeName='" + grd.Name + "' AND eduSchoolID="+MemberService.GetCurrent().School.Id+" AND eduDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND eduDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "eduTeacherGrades");
            if (dt.Rows.Count < 1)
            {
                return -1;
            }
            return int.Parse(dt.Rows[0]["eduTgradeID"].ToString().Trim());
        }
        /// <summary>
        /// Add new TeacherGrade to DB
        /// </summary>
        /// <param name="grd">TeacherGrade</param>
        /// <returns></returns>
        public static bool Add(TeacherGrade grd)
        {
            var count = (int)Connect.GetObject("SELECT COUNT(*) FROM eduTeacherGrades WHERE eduTeacherID=" + grd.TeacherId + " AND eduTgradeName='" + grd.Name + "'");
            if (count >= 1)
            {
                return true;
            }
            var rnd = new Random();
            var a = Color.FromArgb(rnd.Next(50, 256), rnd.Next(50, 256), rnd.Next(50, 256));
            var str = ColorTranslator.ToHtml(a).Substring(1);
            var countColor = (int)Connect.GetObject("SELECT COUNT(*) FROM eduTeacherGrades WHERE eduColor='" + str + "'");
            while (countColor > 0)
            {
                a = Color.FromArgb(rnd.Next(50, 256), rnd.Next(50, 256), rnd.Next(50, 256));
                str = ColorTranslator.ToHtml(a).Substring(1);
                countColor = (int)Connect.GetObject("SELECT COUNT(*) FROM eduTeacherGrades WHERE eduColor='" + str + "'");
            }
            return Connect.InsertUpdateDelete("INSERT INTO eduTeacherGrades(eduTeacherID,eduTgradeName,eduColor, eduDate,eduSchoolID) VALUES(" + grd.TeacherId + ",'" + grd.Name + "','" + str + "', #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "#,"+MemberService.GetCurrent().School.Id+")");
        }
        /// <summary>
        /// Add students to tgarde
        /// </summary>
        /// <param name="tgid">TeacherGrade ID</param>
        /// <param name="students">List of students</param>
        /// <returns></returns>
        public static bool AddStudents(int tgid, List<Member> students)
        {
            var all = GetStudents(tgid);
            Connect.InsertUpdateDelete("DELETE FROM eduLearnGroups WHERE eduTgradeID=" + tgid);

            foreach (var student in students)
            {
                if (all.All(x => x.UserID != student.UserID))
                {
                    ScoreService.ResetScoresStudent(student.UserID, tgid);
                }
                Connect.InsertUpdateDelete("INSERT INTO eduLearnGroups(eduTgradeID,eduStudentID,eduDate) VALUES(" + tgid + "," + student.UserID + ",#" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "#)");
            }
            return true;
        }
        /// <summary>
        /// Add student to tgarde
        /// </summary>
        public static bool AddStudent(int tgid, Member student)
        {
            Connect.InsertUpdateDelete("INSERT INTO eduLearnGroups(eduTgradeID,eduStudentID,eduDate) VALUES(" + tgid + "," + student.UserID + ",#" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "#)");
            return true;
        }
        /// <summary>
        /// Add student to tgarde
        /// </summary>
        public static bool AddStudent(int tgid, int uid)
        {
            Connect.InsertUpdateDelete("INSERT INTO eduLearnGroups(eduTgradeID,eduStudentID,eduDate) VALUES(" + tgid + "," + uid + ",#" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "#)");
            return true;
        }
        /// <summary>
        /// Update TeacherGrade
        /// </summary>
        /// <param name="tg"></param>
        /// <returns></returns>
        public static bool Update(TeacherGrade tg)
        {
            return Connect.InsertUpdateDelete("UPDATE eduTeacherGrades SET eduTeacherID=" + tg.TeacherId + ",eduTgradeName='" + tg.Name + "',eduDate = #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# WHERE eduTgradeID=" + tg.Id);
        }
        /// <summary>
        /// Get part grade of tgarde
        /// </summary>
        /// <param name="tgid">TeacherGrade id</param>
        /// <returns></returns>
        public static string GetParTeacherGrade(int tgid)
        {
            var dt = Connect.GetData("SELECT grade.eduGradeName AS GradeName FROM eduMembers AS m, eduGrades AS grade, eduLearnGroups AS lg WHERE lg.eduStudentID = m.eduUserID AND grade.eduGradeID = m.eduGradeID AND lg.eduTgradeID=" + tgid, "eduLearnGroup");
            if (dt.Rows.Count < 1)
            {
                return "";
            }
            var gname = dt.Rows[0]["GradeName"].ToString();
            return GetParTeacherGrade(gname);
        }
        /// <summary>
        /// Get part grade
        /// </summary>
        /// <param name="gname">grade</param>
        /// <returns></returns>
        public static string GetParTeacherGrade(string gname)
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
        /// Get TeacherGrade by id
        /// </summary>
        /// <param name="tgid">TeacherGrade id</param>
        /// <returns></returns>
        public static TeacherGrade Get(int tgid)
        {
            var dt = Connect.GetData("SELECT * FROM eduTeacherGrades WHERE eduTgradeID=" + tgid, "eduTeacherGrades");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            var c = new TeacherGrade()
            {
                Id = int.Parse(dt.Rows[0]["eduTgradeID"].ToString().Trim()),
                Name = dt.Rows[0]["eduTgradeName"].ToString().Trim(),
                TeacherId = int.Parse(dt.Rows[0]["eduTeacherID"].ToString().Trim())
            };
            return c;
        }
        /// <summary>
        /// Get TeacherGrades of teacher
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public static List<TeacherGrade> GetTeacherTeacherGrades(int tid)
        {
            var dt = Connect.GetData("SELECT m.eduUserID AS TeacherID, eduTgradeID AS TeacherGradeID, eduTgradeName AS TeacherGradeName FROM  eduMembers AS m, eduTeacherGrades WHERE m.eduUserID = eduTeacherID AND eduTeacherID=" + tid + " AND m.eduSchoolID=" + MemberService.GetCurrent().School.Id+" AND eduTeacherGrades.eduDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND eduTeacherGrades.eduDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "eduTeacherGrades");
            return (from DataRow dataRow in dt.Rows
                select new TeacherGrade()
                {
                    Id = int.Parse(dataRow["TeacherGradeID"].ToString().Trim()),
                    Name = dataRow["TeacherGradeName"].ToString().Trim(),
                    TeacherId = int.Parse(dataRow["TeacherID"].ToString().Trim())
                }).ToList();
        }
        /// <summary>
        /// Get student count of TeacherGrade
        /// </summary>
        /// <param name="tgid"></param>
        /// <returns></returns>
        public static int GetStudentCount(int tgid)
        {
            return (int)Connect.GetObject("SELECT COUNT(*) FROM eduLearnGroups WHERE eduTgradeID=" + tgid);
        }
        /// <summary>
        /// Get students of TeacherGrade
        /// </summary>
        /// <param name="tgid"></param>
        /// <returns></returns>
        public static List<Member> GetStudents(int tgid)
        {
            var dt = Connect.GetData("SELECT m.eduFirstName + ' ' + m.eduLastName AS eduStudentName,m.eduUserID AS eduStudentID  FROM eduMembers AS m, eduLearnGroups WHERE m.eduUserID = eduLearnGroups.eduStudentID AND eduTgradeID=" + tgid, "eduLearnGroups");
            return (from DataRow dataRow in dt.Rows
                select new Member()
                {
                    UserID = int.Parse(dataRow["eduStudentID"].ToString().Trim()),
                    Name = dataRow["eduStudentName"].ToString().Trim()
                }).ToList();
        }
        /// <summary>
        /// Get the major of TeacherGrade
        /// </summary>
        /// <param name="tgid"></param>
        /// <returns></returns>
        public static int GetMajor(int tgid)
        {
            var dt = Connect.GetData("SELECT * FROM eduMajorsTgrades WHERE eduTgradeID=" + tgid, "eduMajorsTgrades");
            if (dt.Rows.Count == 0)
                return -1;
            return int.Parse(dt.Rows[0]["eduMajorID"].ToString().Trim());
        }
    }
}
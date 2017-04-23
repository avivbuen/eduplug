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
            var dt = Connect.GetData("SELECT * FROM nhsTeacherGrades WHERE nhsDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND nhsDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "nhsTeacherGrades");
            return (from DataRow dataRow in dt.Rows
                select new TeacherGrade()
                {
                    Id = int.Parse(dataRow["nhsTgradeID"].ToString().Trim()),
                    Name = dataRow["nhsTgradeName"].ToString().Trim(),
                    TeacherId = int.Parse(dataRow["nhsTeacherID"].ToString().Trim())
                }).ToList();
        }
        /// <summary>
        /// Removes TeacherGrade by id
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
        /// <param name="grd">TeacherGrade OBJ</param>
        /// <returns></returns>
        public static int GetId(TeacherGrade grd)
        {
            var dt = Connect.GetData("SELECT * FROM nhsTeacherGrades WHERE nhsTeacherID=" + grd.TeacherId + " AND nhsTgradeName='" + grd.Name + "' AND nhsDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND nhsDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "nhsTeacherGrades");
            if (dt.Rows.Count < 1)
            {
                return -1;
            }
            return int.Parse(dt.Rows[0]["nhsTgradeID"].ToString().Trim());
        }
        /// <summary>
        /// Add new TeacherGrade to DB
        /// </summary>
        /// <param name="grd">TeacherGrade</param>
        /// <returns></returns>
        public static bool Add(TeacherGrade grd)
        {
            var count = (int)Connect.GetObject("SELECT COUNT(*) FROM nhsTeacherGrades WHERE nhsTeacherID=" + grd.TeacherId + " AND nhsTgradeName='" + grd.Name + "'");
            if (count >= 1)
            {
                return true;
            }
            var rnd = new Random();
            var a = Color.FromArgb(rnd.Next(50, 256), rnd.Next(50, 256), rnd.Next(50, 256));
            var str = ColorTranslator.ToHtml(a).Substring(1);
            var countColor = (int)Connect.GetObject("SELECT COUNT(*) FROM nhsTeacherGrades WHERE nhsColor='" + str + "'");
            while (countColor > 0)
            {
                a = Color.FromArgb(rnd.Next(50, 256), rnd.Next(50, 256), rnd.Next(50, 256));
                str = ColorTranslator.ToHtml(a).Substring(1);
                countColor = (int)Connect.GetObject("SELECT COUNT(*) FROM nhsTeacherGrades WHERE nhsColor='" + str + "'");
            }
            return Connect.InsertUpdateDelete("INSERT INTO nhsTeacherGrades(nhsTeacherID,nhsTgradeName,nhsColor, nhsDate) VALUES(" + grd.TeacherId + ",'" + grd.Name + "','" + str + "', #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "#)");
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
            Connect.InsertUpdateDelete("DELETE FROM nhsLearnGroups WHERE nhsTgradeID=" + tgid);

            foreach (var student in students)
            {
                if (all.All(x => x.UserID != student.UserID))
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
        /// Update TeacherGrade
        /// </summary>
        /// <param name="tg"></param>
        /// <returns></returns>
        public static bool Update(TeacherGrade tg)
        {
            return Connect.InsertUpdateDelete("UPDATE nhsTeacherGrades SET nhsTeacherID=" + tg.TeacherId + ",nhsTgradeName='" + tg.Name + "',nhsDate = #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# WHERE nhsTgradeID=" + tg.Id);
        }
        /// <summary>
        /// Get part grade of tgarde
        /// </summary>
        /// <param name="tgid">TeacherGrade id</param>
        /// <returns></returns>
        public static string GetParTeacherGrade(int tgid)
        {
            var dt = Connect.GetData("SELECT grade.nhsGradeName AS GradeName FROM nhsMembers AS m, nhsGrades AS grade, nhsLearnGroups AS lg WHERE lg.nhsStudentID = m.nhsUserID AND grade.nhsGradeID = m.nhsGradeID AND lg.nhsTgradeID=" + tgid, "nhsLearnGroup");
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
            var dt = Connect.GetData("SELECT * FROM nhsTeacherGrades WHERE nhsTgradeID=" + tgid, "nhsTeacherGrades");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            var c = new TeacherGrade()
            {
                Id = int.Parse(dt.Rows[0]["nhsTgradeID"].ToString().Trim()),
                Name = dt.Rows[0]["nhsTgradeName"].ToString().Trim(),
                TeacherId = int.Parse(dt.Rows[0]["nhsTeacherID"].ToString().Trim())
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
            var dt = Connect.GetData("SELECT m.nhsUserID AS TeacherID, nhsTgradeID AS TeacherGradeID, nhsTgradeName AS TeacherGradeName FROM  nhsMembers AS m, nhsTeacherGrades WHERE m.nhsUserID = nhsTeacherID AND nhsTeacherID=" + tid + " AND nhsTeacherGrades.nhsDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND nhsTeacherGrades.nhsDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "nhsTeacherGrades");
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
            return (int)Connect.GetObject("SELECT COUNT(*) FROM nhsLearnGroups WHERE nhsTgradeID=" + tgid);
        }
        /// <summary>
        /// Get students of TeacherGrade
        /// </summary>
        /// <param name="tgid"></param>
        /// <returns></returns>
        public static List<Member> GetStudents(int tgid)
        {
            var dt = Connect.GetData("SELECT m.nhsFirstName + ' ' + m.nhsLastName AS nhsStudentName,m.nhsUserID AS nhsStudentID  FROM nhsMembers AS m, nhsLearnGroups WHERE m.nhsUserID = nhsLearnGroups.nhsStudentID AND nhsTgradeID=" + tgid, "nhsLearnGroups");
            return (from DataRow dataRow in dt.Rows
                select new Member()
                {
                    UserID = int.Parse(dataRow["nhsStudentID"].ToString().Trim()),
                    Name = dataRow["nhsStudentName"].ToString().Trim()
                }).ToList();
        }
        /// <summary>
        /// Get the major of TeacherGrade
        /// </summary>
        /// <param name="tgid"></param>
        /// <returns></returns>
        public static int GetMajor(int tgid)
        {
            var dt = Connect.GetData("SELECT * FROM nhsMajorsTeacherGrades WHERE nhsTgradeID=" + tgid, "nhsMajorsTeacherGrades");
            if (dt.Rows.Count == 0)
                return -1;
            return int.Parse(dt.Rows[0]["nhsMajorID"].ToString().Trim());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using Business_Logic.TeacherGrades;
using Business_Logic.Members;

namespace Business_Logic.Lessons
{
    /// <summary>
    /// LessonService
    /// </summary>
    public static class LessonService
    {
        //Days in week
        public static int DaysInWeek = int.Parse(ConfigurationManager.AppSettings["DaysInWeek"]);
        //Lessons in day
        public static int LessonsInDay = int.Parse(ConfigurationManager.AppSettings["LessonsInDay"]);

        /// <summary>
        /// Gets all the lessons for the following id.
        /// </summary>
        /// <param>The teacher id
        ///     <name>teacherId</name>
        /// </param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public static List<Lesson> GetAll(int teacherId)
        {
            var dt = Connect.GetData("SELECT tg.eduTgradeName AS LessonName, eduLessonID AS LessonID, eduLessons.eduTgradeID AS TeacherGradeID, eduDay AS eduDay, eduHour AS eduHour, tg.eduTeacherID AS TeacherID FROM eduTeacherGrades AS tg, eduLessons WHERE tg.eduTgradeID = eduLessons.eduTgradeID AND tg.eduTeacherID=" + teacherId + " AND eduActive='Yes'", "eduLessons");
            return (from DataRow dr in dt.Rows
                select new Lesson()
                {
                    Name = dr["LessonName"].ToString(),
                    Day = int.Parse(dr["eduDay"].ToString()),
                    Hour = int.Parse(dr["eduHour"].ToString()),
                    Id = int.Parse(dr["LessonID"].ToString()),
                    TeacherId = int.Parse(dr["TeacherID"].ToString())
                }).ToList();
        }
        /// <summary>
        /// Gets a lesson by lesson id
        /// </summary>
        /// <param name="lid">Lesson ID</param>
        /// <returns></returns>
        public static Lesson GetLesson(int lid)
        {
            DataTable dt = Connect.GetData("SELECT tg.eduTgradeName AS LessonName, eduLessonID AS LessonID, eduLessons.eduTgradeID AS TeacherGradeID, eduDay AS eduDay, eduHour AS eduHour, tg.eduTeacherID AS TeacherID FROM eduTeacherGrades AS tg, eduLessons WHERE tg.eduTgradeID = eduLessons.eduTgradeID AND eduLessonID=" + lid + " AND eduActive='Yes'", "eduLessons");
            if (dt.Rows.Count == 0)
                return null;
            DataRow dr = dt.Rows[0];
            var lesson = new Lesson()
            {
                Name = dr["LessonName"].ToString(),
                Day = int.Parse(dr["eduDay"].ToString()),
                Hour = int.Parse(dr["eduHour"].ToString()),
                Id = int.Parse(dr["LessonID"].ToString()),
                TeacherId = int.Parse(dr["TeacherID"].ToString()),
                Changes = GetChanges(lid)
            };
            return lesson;
        }
        /// <summary>
        /// Get all students in lesson
        /// </summary>
        /// <param name="lid">Lesson id</param>
        /// <returns></returns>
        public static List<Member> GetAllStudents(int lid)
        {
            DataTable dt = Connect.GetData("SELECT eduFirstName,eduLastName,eduUserID FROM eduMembers AS student,eduLearnGroups AS lgrps,eduTeacherGrades AS tg, eduLessons WHERE lgrps.eduTgradeID = tg.eduTgradeID AND lgrps.eduStudentID=student.eduUserID AND tg.eduTgradeID = eduLessons.eduTgradeID AND eduLessonID=" + lid, "eduMembers");
            List<Member> students = new List<Member>();
            foreach (DataRow dr in dt.Rows)
            {
                Member m = new Member()
                {
                    FirstName = dr["eduFirstName"].ToString(),
                    LastName = dr["eduLastName"].ToString(),
                    UserID = int.Parse(dr["eduUserID"].ToString())
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
        /// <summary>
        /// Gets lessons by teacher
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        private static DataTable GetLessonsByTeacher(int tid)
        {
            return Connect.GetData("SELECT tg.eduColor AS cellColor, tg.eduTgradeName AS LessonName, eduLessonID AS LessonID, eduLessons.eduTgradeID AS TeacherGradeID, eduDay AS eduDay, eduHour AS eduHour, tg.eduTeacherID AS TeacherID FROM eduTeacherGrades AS tg, eduLessons WHERE tg.eduTgradeID = eduLessons.eduTgradeID AND tg.eduTeacherID=" + tid + " AND tg.eduDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND tg.eduDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "# AND eduActive='Yes'", "eduLessons");
        }

        /// <summary>
        /// Gets lessons by student
        /// </summary>
        /// <returns></returns>
        private static DataTable GetLessonsByStudent(int sid)
        {
            return Connect.GetData("SELECT tg.eduColor AS cellColor, tg.eduTgradeName AS LessonName, eduLessonID AS LessonID, eduLessons.eduTgradeID AS TeacherGradeID, eduDay AS eduDay, eduHour AS eduHour, tg.eduTeacherID AS TeacherID FROM eduTeacherGrades AS tg, eduLessons, eduLearnGroups WHERE tg.eduTgradeID = eduLessons.eduTgradeID AND eduLearnGroups.eduTgradeID = tg.eduTgradeID  AND eduLearnGroups.eduStudentID=" + sid + " AND tg.eduDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND tg.eduDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "# AND eduActive='Yes'", "eduLessons");
        }

        /// <summary>
        /// Gets lessons by GradePart
        /// </summary>
        /// <returns></returns>
        private static DataTable GetLessonsByGradePart(string gradePart)
        {
            return Connect.GetData("SELECT tg.eduColor AS cellColor, tg.eduTgradeName AS LessonName, lsons.eduLessonID AS LessonID, lsons.eduTgradeID AS TeacherGradeID, lsons.eduDay AS eduDay, lsons.eduHour AS eduHour, tg.eduTeacherID AS TeacherID FROM eduLessons AS lsons INNER JOIN eduTeacherGrades AS tg ON tg.eduTgradeID = lsons.eduTgradeID WHERE tg.eduTgradeID IN( SELECT grp.eduTgradeID FROM (eduLearnGroups grp INNER JOIN eduMembers AS mem ON mem.eduUserID = grp.eduStudentID) INNER JOIN eduGrades AS grd ON mem.eduGradeID = grd.eduGradeID WHERE grd.eduGradeName LIKE '%" + gradePart + "%' AND tg.eduDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND tg.eduDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#) AND eduActive='Yes'", "eduLessons");
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
        /// <summary>
        /// Cancel change
        /// </summary>
        /// <param name="changeId"></param>
        /// <returns></returns>
        public static bool CancelChange(int changeId)
        {
            return Connect.InsertUpdateDelete("DELETE FROM eduLessonChanges WHERE eduLessonChangeID=" + changeId);
        }
        /// <summary>
        /// Get the changes of a lesson
        /// </summary>
        /// <param name="lid">Lesson id</param>
        /// <returns></returns>
        public static List<LessonChange> GetChanges(int lid)
        {
            
            DataTable dt = Connect.GetData("SELECT * FROM eduLessonChanges WHERE eduLessonID=" + lid, "eduLessonChanges");
            List<LessonChange> changeTable = new List<LessonChange>();
            foreach (DataRow dr in dt.Rows)
            {
                LessonChange change = new LessonChange()
                {
                    Id = int.Parse(dr["eduLessonChangeID"].ToString()),
                    ChangeType = ((LessonChangeType)(char.Parse(dr["eduLessonChangeType"].ToString()))),
                    Date = DateTime.Parse(dr["eduDate"].ToString().Trim()),
                    Message = dr["eduMessage"].ToString().Trim(),
                    LessonId = lid
                };
                changeTable.Add(change);
            }
            return changeTable;
        }
        /// <summary>
        /// Get time table - helper method
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        private static List<LessonGroup[]> GetDataTime(DataTable dt)
        {
            LessonGroup[,] lessons = new LessonGroup[LessonsInDay, DaysInWeek];//Array that stores the time table
            for (int i = 0; i < lessons.GetLength(0); i++)//Array INIT
            {
                for (int j = 0; j < lessons.GetLength(1); j++)
                    lessons[i, j] = null;
            }
            var con = new OleDbConnection(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
            con.Open();
            foreach (DataRow dr in dt.Rows)//Array Fill
            {
                var command = new OleDbCommand("SELECT * FROM eduLessonChanges WHERE eduLessonID=" + int.Parse(dr["LessonID"].ToString()), con);
                var adp = new OleDbDataAdapter(command);
                var ds = new DataSet();

                adp.Fill(ds, "eduLessonChanges");
                List<LessonChange> changeTable = new List<LessonChange>();
                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    LessonChange change = new LessonChange()
                    {
                        Id = int.Parse(dr1["eduLessonChangeID"].ToString()),
                        ChangeType = ((LessonChangeType)(char.Parse(dr1["eduLessonChangeType"].ToString()))),
                        Date = DateTime.Parse(dr1["eduDate"].ToString().Trim()),
                        Message = dr1["eduMessage"].ToString().Trim(),
                        LessonId = int.Parse(dr["LessonID"].ToString())
                    };
                    changeTable.Add(change);
                }
                Lesson lesson = new Lesson()
                {
                    Name = dr["LessonName"].ToString(),
                    Day = int.Parse(dr["eduDay"].ToString()),
                    Hour = int.Parse(dr["eduHour"].ToString()),
                    Id = int.Parse(dr["LessonID"].ToString()),
                    TeacherId = int.Parse(dr["TeacherID"].ToString()),
                    Color = dr["cellColor"].ToString(),
                    TeacherGradeId = int.Parse(dr["TeacherGradeID"].ToString()),
                    Changes = changeTable
                };
                if (int.Parse(dr["eduHour"].ToString()) > (LessonsInDay) || int.Parse(dr["eduHour"].ToString()) <= 0 || int.Parse(dr["eduDay"].ToString()) > DaysInWeek || int.Parse(dr["eduDay"].ToString()) <= 0)
                {
                    continue;
                }
                if (lessons[int.Parse(dr["eduHour"].ToString()) - 1, int.Parse(dr["eduDay"].ToString()) - 1] == null)
                {
                    lessons[int.Parse(dr["eduHour"].ToString()) - 1, int.Parse(dr["eduDay"].ToString()) - 1] = new LessonGroup();
                }
                lessons[int.Parse(dr["eduHour"].ToString()) - 1, int.Parse(dr["eduDay"].ToString()) - 1].Lessons.Add(lesson);
            }
            con.Close();
            List<LessonGroup[]> lessData = new List<LessonGroup[]>();//Converting the 2D array to a list of 1D arrays(The conversion is made for the datalist)
            for (int i = 0; i < lessons.GetLength(0); i++)
                lessData.Add(GetRow(lessons, i));//Filling the new struct with the old one

            return lessData;//Returning the new struct
        }
        /// <summary>
        /// Gets all the lessons of a TeacherGrade
        /// </summary>
        /// <param name="tgid">TeacherGrade id</param>
        /// <returns></returns>
        public static List<Lesson> GetLessons(int tgid)
        {
            DataTable dt = Connect.GetData("SELECT tg.eduColor AS cellColor, tg.eduTgradeName AS LessonName, eduLessonID AS LessonID, eduLessons.eduTgradeID AS TeacherGradeID, eduDay AS eduDay, eduHour AS eduHour, tg.eduTeacherID AS TeacherID FROM eduTeacherGrades AS tg, eduLessons WHERE tg.eduTgradeID = eduLessons.eduTgradeID AND tg.eduTgradeID=" + tgid + " AND tg.eduDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND tg.eduDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "# AND eduActive = YES", "eduLessons");
            List<Lesson> lessons = new List<Lesson>();
            foreach (DataRow dr in dt.Rows)//Array Fill
            {
                Lesson lesson = new Lesson()
                {
                    Name = dr["LessonName"].ToString(),
                    Day = int.Parse(dr["eduDay"].ToString()),
                    Hour = int.Parse(dr["eduHour"].ToString()),
                    Id = int.Parse(dr["LessonID"].ToString()),
                    TeacherId = int.Parse(dr["TeacherID"].ToString()),
                    Color = dr["cellColor"].ToString(),
                    TeacherGradeId = int.Parse(dr["TeacherGradeID"].ToString()),
                };
                lessons.Add(lesson);
            }
            return lessons;
        }
        /// <summary>
        /// Delete lesson
        /// </summary>
        /// <param name="tgid"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static bool DeleteLesson(int tgid, int day, int hour)
        {
            return Connect.InsertUpdateDelete("UPDATE eduLessons SET eduActive='No' WHERE eduTgradeID=" + tgid + " AND eduDay=" + day + " AND eduHour=" + hour);
        }
        /// <summary>
        /// Delete lesson
        /// </summary>
        public static bool DeleteLesson(int lid)
        {
            return Connect.InsertUpdateDelete("UPDATE eduLessons SET eduActive='No' WHERE eduLessonID=" + lid);
        }
        /// <summary>
        /// Add lesson to db
        /// </summary>
        public static bool Add(Lesson lsn)
        {
            var t = TeacherGradeService.Get(lsn.TeacherGradeId);
            if (t == null) return false;
            var rows = Connect.InsertUpdateDeleteState("UPDATE eduLessons SET eduActive='Yes' WHERE eduTgradeID=" + lsn.TeacherGradeId + " AND eduDay=" + lsn.Day + " AND eduHour=" + lsn.Hour);
            if (rows == 0)
            {
                Connect.InsertUpdateDelete("INSERT INTO eduLessons (eduTgradeID,eduDay,eduHour,eduActive) VALUES (" + lsn.TeacherGradeId + "," + lsn.Day + "," + lsn.Hour + ",'Yes')");
            }
            return true;
        }
        /// <summary>
        /// Add new change to lesson
        /// </summary>
        /// <param name="change"></param>
        /// <returns></returns>
        public static bool AddChange(LessonChange change)
        {
            return Connect.InsertUpdateDelete("INSERT INTO eduLessonChanges (eduLessonChangeType,eduLessonID,eduMessage,eduDate) VALUES('" + Converter.GetChangeType(change.ChangeType) + "'," + change.LessonId + ",'" + change.Message + "',#" + Converter.GetTimeShortForDataBase(change.Date) + "#)");
        }
        /// <summary>
        /// Get lesson by params
        /// </summary>
        /// <param name="tgid"></param>
        /// <param name="hour"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static Lesson GetLesson(int tgid,int hour,int day)
        {
            DataTable dt = Connect.GetData("SELECT tg.eduTgradeName AS LessonName, eduLessonID AS LessonID, eduLessons.eduTgradeID AS TeacherGradeID, eduDay AS eduDay, eduHour AS eduHour, tg.eduTeacherID AS TeacherID FROM eduTeacherGrades AS tg, eduLessons WHERE tg.eduTgradeID = eduLessons.eduTgradeID AND eduHour=" + hour + " AND eduDay="+day+ " AND tg.eduTgradeID="+tgid+" AND eduActive='Yes'", "eduLessons");
            if (dt.Rows.Count == 0)
                return null;
            DataRow dr = dt.Rows[0];
            Lesson lesson = new Lesson()
            {
                Name = dr["LessonName"].ToString(),
                Day = int.Parse(dr["eduDay"].ToString()),
                Hour = int.Parse(dr["eduHour"].ToString()),
                Id = int.Parse(dr["LessonID"].ToString()),
                TeacherId = int.Parse(dr["TeacherID"].ToString()),
                Changes = GetChanges(int.Parse(dr["LessonID"].ToString()))
            };
            return lesson;
        }
    }
}
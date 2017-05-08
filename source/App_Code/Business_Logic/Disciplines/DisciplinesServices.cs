using System;
using System.Collections.Generic;
using System.Data;

namespace Business_Logic.Disciplines
{
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
            DataTable dt = Connect.GetData("SELECT * FROM eduDisciplines", "eduDisciplines");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DisciplineType c = new DisciplineType()
                {
                    Id = int.Parse(dt.Rows[i]["eduDisciplinesID"].ToString().Trim()),
                    Name = dt.Rows[i]["eduDisciplinesTitle"].ToString().Trim(),
                    Score = int.Parse(dt.Rows[i]["eduDisciplinesScore"].ToString().Trim())
                };
                types.Add(c);
            }
            return types;
        }
        /// <summary>
        /// Add new event
        /// </summary>
        /// <param name="lessonId">The id of the lesson</param>
        /// <param name="studentId">The user id of the student</param>
        /// <param name="disciplinesId">The disciplines type id</param>
        /// <param name="date">The date of the event</param>
        /// <returns></returns>
        public static bool Add(int lessonId, int studentId, int disciplinesId,DateTime date)
        {
            return Connect.InsertUpdateDelete("INSERT INTO eduDisciplinesMembers (eduLessonID,eduDisciplinesID,eduStudentID,eduDate) VALUES(" + lessonId + "," + disciplinesId + "," + studentId + ",#" + Converter.GetTimeShortForDataBase(date) + "#)");
        }
        /// <summary>
        /// Get all the preselected items
        /// </summary>
        /// <param name="lessonId">The lesson id</param>
        /// <param name="date">The date of the lesson</param>
        /// <returns></returns>
        public static List<DisciplineEvent> GetSelected(int lessonId, DateTime date)
        {
            List<DisciplineEvent> events = new List<DisciplineEvent>();
            DataTable dt = Connect.GetData("SELECT * FROM eduDisciplinesMembers WHERE eduLessonID="+lessonId+" AND eduDate=#"+Converter.GetTimeShortForDataBase(date)+"#" , "eduDisciplinesMembers");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DisciplineEvent even = new DisciplineEvent()
                {
                    StudentId = int.Parse(dt.Rows[i]["eduStudentID"].ToString().Trim()),
                    DisciplinesId = int.Parse(dt.Rows[i]["eduDisciplinesID"].ToString().Trim())
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
            DataTable dt = Connect.GetData("SELECT ds.eduDisciplinesTitle AS dName, dsm.eduDate AS dDate, tg.eduTgradeName AS lName, tg.eduTeacherID AS teacherId, ls.eduHour AS dHour FROM eduTeacherGrades AS tg, eduDisciplinesMembers AS dsm,eduDisciplines AS ds,eduLessons AS ls WHERE dsm.eduStudentID=" + uid+ " AND dsm.eduDisciplinesID=ds.eduDisciplinesID AND ls.eduLessonID = dsm.eduLessonID AND ls.eduTgradeID=tg.eduTgradeID ORDER BY dsm.eduDate DESC", "eduDisciplinesMembers");
            return dt;
        }

        /// <summary>
        /// Get student disciplines events by user id
        /// </summary>
        /// <param name="uid">User ID</param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataTable GetStudent(int uid,DateTime date)
        {
            DataTable dt = Connect.GetData("SELECT ds.eduDisciplinesTitle AS dName, dsm.eduDate AS dDate, tg.eduTgradeName AS lName, tg.eduTeacherID AS teacherId, ls.eduHour AS dHour FROM eduTeacherGrades AS tg, eduDisciplinesMembers AS dsm,eduDisciplines AS ds,eduLessons AS ls WHERE dsm.eduStudentID=" + uid + " AND dsm.eduDisciplinesID=ds.eduDisciplinesID AND ls.eduLessonID = dsm.eduLessonID AND ls.eduTgradeID=tg.eduTgradeID AND dsm.eduDate > #"+Converter.GetTimeShortForDataBase(date)+ "# ORDER BY dsm.eduDate DESC", "eduDisciplinesMembers");
            return dt;
        }

        /// <summary>
        /// Get student disciplines events by user id
        /// </summary>
        /// <param name="uid">User ID</param>
        /// <param name="tgid"></param>
        /// <returns></returns>
        public static DataTable GetStudent(int uid,int tgid)
        {
            DataTable dt = Connect.GetData("SELECT ds.eduDisciplinesTitle AS dName, dsm.eduDate AS dDate, tg.eduTgradeName AS lName, tg.eduTeacherID AS teacherId, ls.eduHour AS dHour FROM eduTeacherGrades AS tg, eduDisciplinesMembers AS dsm,eduDisciplines AS ds,eduLessons AS ls WHERE dsm.eduStudentID=" + uid + " AND dsm.eduDisciplinesID=ds.eduDisciplinesID AND ls.eduLessonID = dsm.eduLessonID AND ls.eduTgradeID=tg.eduTgradeID AND tg.eduTgradeID="+tgid, "eduDisciplinesMembers");
            return dt;
        }
        /// <summary>
        /// Reset lesson disciplines
        /// </summary>
        /// <param name="lessonId">LessonID</param>
        /// <param name="date">Date</param>
        public static void ResetLesson(int lessonId, DateTime date)
        {
            Connect.InsertUpdateDelete("DELETE FROM eduDisciplinesMembers WHERE eduLessonID=" + lessonId + " AND eduDate =#" + Converter.GetTimeShortForDataBase(date) + "#");
        }
    }
}
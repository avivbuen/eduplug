using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Business_Logic.Exams;
using Business_Logic.Members;
using Business_Logic.TeacherGrades;

namespace Business_Logic.Scores
{
    /// <summary>
// ScoreService
    /// </summary>
    public static class ScoreService
    {
        /// <summary>
        /// Get all scores
        /// </summary>
        /// <returns></returns>
        public static List<Score> GetAll()
        {
            const string sqlGet = "SELECT score.eduScoreID AS eduScoreID, exam.eduExamID AS ExamID,TeacherGrade.eduTgradeName AS GradeName, TeacherGrade.eduTgradeID AS GradeID, teacher.eduFirstName +' '+ teacher.eduLastName AS TeacherName,teacher.eduUserID AS TeacherID,student.eduFirstName + ' ' + student.eduLastName AS StudentName,student.eduUserID AS StudentID,grade.eduGradeName AS StudenTeacherGrade,grade.eduGradeID AS StudenTeacherGradeID,exam.eduExamDate AS ExamDate,exam.eduExamTitle AS ExamTitle,score.eduScore AS StudentScore FROM eduScores AS score, eduGrades AS grade, eduExams AS exam,eduTeacherGrades AS TeacherGrade, eduMembers AS teacher, eduMembers AS student WHERE (score.eduExamID=exam.eduExamID AND teacher.eduUserID = exam.eduTeacherID AND student.eduUserID=score.eduStudentID AND student.eduGradeID=grade.eduGradeID AND exam.eduTgradeID=TeacherGrade.eduTgradeID)";
            var dt = Connect.GetData(sqlGet, "eduScores");
            return (from DataRow dataRow in dt.Rows
                select new Score()
                {
                    Id = int.Parse(dataRow["eduScoreID"].ToString().Trim()),
                    Student = new Member() {Name = dataRow["StudentName"].ToString().Trim(), UserID = int.Parse(dataRow["StudentID"].ToString().Trim())},
                    Exam = new Exam() {Id = int.Parse(dataRow["ExamID"].ToString().Trim()), Date = DateTime.Parse(dataRow["ExamDate"].ToString().Trim()), Title = dataRow["ExamTitle"].ToString().Trim(), TeacherId = int.Parse(dataRow["TeacherID"].ToString().Trim())},
                    ScoreVal = int.Parse(dataRow["StudentScore"].ToString().Trim())
                }).ToList();
        }
        /// <summary>
        /// Get all scores of student
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static List<Score> GetAllStudent(int sid)
        {
            if (MemberService.GetUserPart(sid) == null || MemberService.GetUserPart(sid).Auth != MemberClearance.Student)
                return new List<Score>();
            var sqlGet = "SELECT score.eduScoreID AS eduScoreID,exam.eduExamID AS ExamID,TeacherGrade.eduTgradeName AS GradeName, TeacherGrade.eduTgradeID AS GradeID, teacher.eduFirstName +' '+ teacher.eduLastName AS TeacherName,teacher.eduUserID AS TeacherID,student.eduFirstName + ' ' + student.eduLastName AS StudentName,student.eduUserID AS StudentID,grade.eduGradeName AS StudenTeacherGrade,grade.eduGradeID AS StudenTeacherGradeID,exam.eduExamDate AS ExamDate,exam.eduExamTitle AS ExamTitle,score.eduScore AS StudentScore FROM eduScores AS score, eduGrades AS grade, eduExams AS exam,eduTeacherGrades AS TeacherGrade, eduMembers AS teacher, eduMembers AS student WHERE (score.eduExamID=exam.eduExamID AND teacher.eduUserID = exam.eduTeacherID AND student.eduUserID=score.eduStudentID AND student.eduGradeID=grade.eduGradeID AND exam.eduTgradeID=TeacherGrade.eduTgradeID AND eduStudentID=" + sid + ")";
            var dt = Connect.GetData(sqlGet, "eduScores");
            var mem = MemberService.GetUserPart(sid);

            return (from DataRow dataRow in dt.Rows
                where int.Parse(dataRow["StudentScore"].ToString().Trim())!=-1
                    select new Score()
                {
                    Id = int.Parse(dataRow["eduScoreID"].ToString().Trim()),
                    Student = mem,
                    Exam = new Exam() { Id = int.Parse(dataRow["ExamID"].ToString().Trim()), Date = DateTime.Parse(dataRow["ExamDate"].ToString().Trim()), Title = dataRow["ExamTitle"].ToString().Trim(), TeacherId = int.Parse(dataRow["TeacherID"].ToString().Trim()) },
                    ScoreVal = int.Parse(dataRow["StudentScore"].ToString().Trim())
                }).ToList();
        }
        /// <summary>
        /// Get all scores of student in specific TeacherGrade
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="tgid"></param>
        /// <returns></returns>
        public static List<Score> GetAllStudent(int sid,int tgid)
        {
            Member mem = MemberService.GetUserPart(sid);
            if (mem == null || mem.Auth != MemberClearance.Student)
                return new List<Score>();
            string sqlGet = "SELECT exam.eduPrecent AS ePrecent, score.eduScoreID AS eduScoreID,exam.eduExamID AS ExamID,TeacherGrade.eduTgradeName AS GradeName, TeacherGrade.eduTgradeID AS GradeID, teacher.eduFirstName +' '+ teacher.eduLastName AS TeacherName,teacher.eduUserID AS TeacherID,student.eduFirstName + ' ' + student.eduLastName AS StudentName,student.eduUserID AS StudentID,grade.eduGradeName AS StudenTeacherGrade,grade.eduGradeID AS StudenTeacherGradeID,exam.eduExamDate AS ExamDate,exam.eduExamTitle AS ExamTitle,score.eduScore AS StudentScore FROM eduScores AS score, eduGrades AS grade, eduExams AS exam,eduTeacherGrades AS TeacherGrade, eduMembers AS teacher, eduMembers AS student WHERE (score.eduExamID=exam.eduExamID AND teacher.eduUserID = exam.eduTeacherID AND student.eduUserID=score.eduStudentID AND student.eduGradeID=grade.eduGradeID AND exam.eduTgradeID=TeacherGrade.eduTgradeID AND eduStudentID=" + sid + " AND TeacherGrade.eduTgradeID=" + tgid + ")";
            List<Score> scores = new List<Score>();
            DataTable dt = Connect.GetData(sqlGet, "eduScores");
        
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (int.Parse(dt.Rows[i]["StudentScore"].ToString().Trim()) == -1) continue;
                Score c = new Score()
                {
                    Id = int.Parse(dt.Rows[i]["eduScoreID"].ToString().Trim()),
                    Student = mem,
                    Exam = new Exam() { Id = int.Parse(dt.Rows[i]["ExamID"].ToString().Trim()), Date = DateTime.Parse(dt.Rows[i]["ExamDate"].ToString().Trim()), Title = dt.Rows[i]["ExamTitle"].ToString().Trim(), TeacherId = int.Parse(dt.Rows[i]["TeacherID"].ToString().Trim()), Precent= int.Parse(dt.Rows[i]["ePrecent"].ToString().Trim()) },
                    ScoreVal = int.Parse(dt.Rows[i]["StudentScore"].ToString().Trim())
                };
                scores.Add(c);
            }
            return scores;
        }
        /// <summary>
        /// Get all scores of student in specific TeacherGrade and year part
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="tgid"></param>
        /// <param name="yearPart"></param>
        /// <returns></returns>
        public static List<Score> GetAllStudent(int sid, int tgid,string yearPart)
        {
            Member mem = MemberService.GetUserPart(sid);
            if (mem == null || mem.Auth != MemberClearance.Student)
                return new List<Score>();
            string sqlGet = "SELECT exam.eduPrecent AS ePrecent, score.eduScoreID AS eduScoreID,exam.eduExamID AS ExamID,TeacherGrade.eduTgradeName AS GradeName, TeacherGrade.eduTgradeID AS GradeID, teacher.eduFirstName +' '+ teacher.eduLastName AS TeacherName,teacher.eduUserID AS TeacherID,student.eduFirstName + ' ' + student.eduLastName AS StudentName,student.eduUserID AS StudentID,grade.eduGradeName AS StudenTeacherGrade,grade.eduGradeID AS StudenTeacherGradeID,exam.eduExamDate AS ExamDate,exam.eduExamTitle AS ExamTitle,score.eduScore AS StudentScore FROM eduScores AS score, eduGrades AS grade, eduExams AS exam,eduTeacherGrades AS TeacherGrade, eduMembers AS teacher, eduMembers AS student WHERE (score.eduExamID=exam.eduExamID AND teacher.eduUserID = exam.eduTeacherID AND student.eduUserID=score.eduStudentID AND student.eduGradeID=grade.eduGradeID AND exam.eduTgradeID=TeacherGrade.eduTgradeID AND exam.eduYearPart='"+yearPart+"' AND eduStudentID=" + sid + " AND TeacherGrade.eduTgradeID=" + tgid + ")";
            List<Score> scores = new List<Score>();
            DataTable dt = Connect.GetData(sqlGet, "eduScores");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (int.Parse(dt.Rows[i]["StudentScore"].ToString().Trim()) != -1)
                {
                    Score c = new Score()
                    {
                        Id = int.Parse(dt.Rows[i]["eduScoreID"].ToString().Trim()),
                        Student = mem,
                        Exam = new Exam() { Id = int.Parse(dt.Rows[i]["ExamID"].ToString().Trim()), Date = DateTime.Parse(dt.Rows[i]["ExamDate"].ToString().Trim()), Title = dt.Rows[i]["ExamTitle"].ToString().Trim(), TeacherId = int.Parse(dt.Rows[i]["TeacherID"].ToString().Trim()), Precent = int.Parse(dt.Rows[i]["ePrecent"].ToString().Trim()) },
                        ScoreVal = int.Parse(dt.Rows[i]["StudentScore"].ToString().Trim())
                    };
                    scores.Add(c);
                }
            }
            return scores;
        }
        /// <summary>
        /// Get all scores of student in specific TeacherGrade with empty scores
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="tgid"></param>
        /// <returns></returns>
        public static List<Score> GetAllStudentWithEmpty(int sid,int tgid)
        {
            if (MemberService.GetUserPart(sid) == null || MemberService.GetUserPart(sid).Auth != MemberClearance.Student)
                return new List<Score>();
            var sqlGet = "SELECT score.eduScoreID AS eduScoreID,exam.eduExamID AS ExamID,TeacherGrade.eduTgradeName AS GradeName, TeacherGrade.eduTgradeID AS GradeID, teacher.eduFirstName +' '+ teacher.eduLastName AS TeacherName,teacher.eduUserID AS TeacherID,student.eduFirstName + ' ' + student.eduLastName AS StudentName,student.eduUserID AS StudentID,grade.eduGradeName AS StudenTeacherGrade,grade.eduGradeID AS StudenTeacherGradeID,exam.eduExamDate AS ExamDate,exam.eduExamTitle AS ExamTitle,score.eduScore AS StudentScore FROM eduScores AS score, eduGrades AS grade, eduExams AS exam,eduTeacherGrades AS TeacherGrade, eduMembers AS teacher, eduMembers AS student WHERE (score.eduExamID=exam.eduExamID AND teacher.eduUserID = exam.eduTeacherID AND student.eduUserID=score.eduStudentID AND student.eduGradeID=grade.eduGradeID AND exam.eduTgradeID=TeacherGrade.eduTgradeID AND eduStudentID=" + sid + " AND TeacherGrade.eduTgradeID=" + tgid+")";
            var dt = Connect.GetData(sqlGet, "eduScores");
            var mem = MemberService.GetUserPart(sid);
            return (from DataRow dataRow in dt.Rows
                select new Score()
                {
                    Id = int.Parse(dataRow["eduScoreID"].ToString().Trim()),
                    Student = mem,
                    Exam = new Exam() {Id = int.Parse(dataRow["ExamID"].ToString().Trim()), Date = DateTime.Parse(dataRow["ExamDate"].ToString().Trim()), Title = dataRow["ExamTitle"].ToString().Trim(), TeacherId = int.Parse(dataRow["TeacherID"].ToString().Trim())},
                    ScoreVal = int.Parse(dataRow["StudentScore"].ToString().Trim())
                }).ToList();
        }
        /// <summary>
        /// Get all scores of student in specific TeacherGrade and year part with empty scores
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="tgid"></param>
        /// <param name="yearPart"></param>
        /// <returns></returns>
        public static List<Score> GetAllStudentWithEmpty(int sid, int tgid,string yearPart)
        {
            var mem = MemberService.GetUserPart(sid);
            if (mem == null || mem.Auth != MemberClearance.Student)
                return new List<Score>();
            var sqlGet = "SELECT score.eduScoreID AS eduScoreID,exam.eduExamID AS ExamID,TeacherGrade.eduTgradeName AS GradeName, TeacherGrade.eduTgradeID AS GradeID, teacher.eduFirstName +' '+ teacher.eduLastName AS TeacherName,teacher.eduUserID AS TeacherID,student.eduFirstName + ' ' + student.eduLastName AS StudentName,student.eduUserID AS StudentID,grade.eduGradeName AS StudenTeacherGrade,grade.eduGradeID AS StudenTeacherGradeID,exam.eduExamDate AS ExamDate,exam.eduExamTitle AS ExamTitle,score.eduScore AS StudentScore FROM eduScores AS score, eduGrades AS grade, eduExams AS exam,eduTeacherGrades AS TeacherGrade, eduMembers AS teacher, eduMembers AS student WHERE (score.eduExamID=exam.eduExamID AND teacher.eduUserID = exam.eduTeacherID AND student.eduUserID=score.eduStudentID AND student.eduGradeID=grade.eduGradeID AND exam.eduTgradeID=TeacherGrade.eduTgradeID AND eduStudentID=" + sid + " AND exam.eduYearPart='"+yearPart+"' AND TeacherGrade.eduTgradeID=" + tgid + ")";
            var dt = Connect.GetData(sqlGet, "eduScores");
            return (from DataRow dataRow in dt.Rows
                select new Score()
                {
                    Id = int.Parse(dataRow["eduScoreID"].ToString().Trim()),
                    Student = mem,
                    Exam = new Exam() {Id = int.Parse(dataRow["ExamID"].ToString().Trim()), Date = DateTime.Parse(dataRow["ExamDate"].ToString().Trim()), Title = dataRow["ExamTitle"].ToString().Trim(), TeacherId = int.Parse(dataRow["TeacherID"].ToString().Trim())},
                    ScoreVal = int.Parse(dataRow["StudentScore"].ToString().Trim())
                }).ToList();
        }
        /// <summary>
        /// Get all the scores of an exam
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public static List<Score> GetAllExam(int eid)
        {
            var sqlGet = "SELECT score.eduScoreID AS eduScoreID,exam.eduExamID AS ExamID,TeacherGrade.eduTgradeName AS GradeName, TeacherGrade.eduTgradeID AS GradeID, teacher.eduFirstName +' '+ teacher.eduLastName AS TeacherName,teacher.eduUserID AS TeacherID,student.eduFirstName + ' ' + student.eduLastName AS StudentName,student.eduUserID AS StudentID,grade.eduGradeName AS StudenTeacherGrade,grade.eduGradeID AS StudenTeacherGradeID,exam.eduExamDate AS ExamDate,exam.eduExamTitle AS ExamTitle,score.eduScore AS StudentScore FROM eduScores AS score, eduGrades AS grade, eduExams AS exam,eduTeacherGrades AS TeacherGrade, eduMembers AS teacher, eduMembers AS student WHERE (score.eduExamID=exam.eduExamID AND teacher.eduUserID = exam.eduTeacherID AND student.eduUserID=score.eduStudentID AND student.eduGradeID=grade.eduGradeID AND exam.eduTgradeID=TeacherGrade.eduTgradeID AND exam.eduExamID=" + eid + ")";
            var dt = Connect.GetData(sqlGet, "eduScores");
            var exm = ExamService.GetExam(eid);
            return (from DataRow dataRow in dt.Rows
                where int.Parse(dataRow["StudentScore"].ToString().Trim()) != -1
                select new Score()
                {
                    Id = int.Parse(dataRow["eduScoreID"].ToString().Trim()),
                    Student = new Member() {Name = dataRow["StudentName"].ToString().Trim(), UserID = int.Parse(dataRow["StudentID"].ToString().Trim())},
                    Exam = exm,
                    ScoreVal = int.Parse(dataRow["StudentScore"].ToString().Trim())
                }).ToList();
        }
        /// <summary>
        /// Get score
        /// </summary>
        /// <param name="sid">Student ID</param>
        /// <param name="eid">Exam ID</param>
        /// <returns></returns>
        public static Score GetScore(int sid, int eid)
        {
            var sqlGet = "SELECT score.eduScoreID AS eduScoreID,exam.eduExamID AS ExamID,TeacherGrade.eduTgradeName AS GradeName, TeacherGrade.eduTgradeID AS GradeID, teacher.eduFirstName +' '+ teacher.eduLastName AS TeacherName,teacher.eduUserID AS TeacherID,student.eduFirstName + ' ' + student.eduLastName AS StudentName,student.eduUserID AS StudentID,grade.eduGradeName AS StudenTeacherGrade,grade.eduGradeID AS StudenTeacherGradeID,exam.eduExamDate AS ExamDate,exam.eduExamTitle AS ExamTitle,score.eduScore AS StudentScore FROM eduScores AS score, eduGrades AS grade, eduExams AS exam,eduTeacherGrades AS TeacherGrade, eduMembers AS teacher, eduMembers AS student WHERE (score.eduExamID=exam.eduExamID AND teacher.eduUserID = exam.eduTeacherID AND student.eduUserID=score.eduStudentID AND student.eduGradeID=grade.eduGradeID AND exam.eduTgradeID=TeacherGrade.eduTgradeID AND exam.eduExamID=" + eid + " AND score.eduStudentID=" + sid + ")";
            var dt = Connect.GetData(sqlGet, "eduScores");
            if (dt.Rows.Count == 0)
                return null;
            return new Score()
            {
                Id = int.Parse(dt.Rows[0]["eduScoreID"].ToString().Trim()),
                Student = new Member() { Name = dt.Rows[0]["StudentName"].ToString().Trim(), UserID = int.Parse(dt.Rows[0]["StudentID"].ToString().Trim()) },
                ScoreVal = int.Parse(dt.Rows[0]["StudentScore"].ToString().Trim())
            };
        }

        public static double GetAvgTgrade(int tgid)
        {
            var exams= ExamService.GetExamsByTeacherGradeId(tgid);
            var valsDoubles = exams.Select(exam => GetAvgExam(exam.Id)).Where(val => val > 0).ToList();
            return valsDoubles.Count == 0 ? 0 : valsDoubles.Average();
        }
        /// <summary>
        /// Get avg of exam
        /// </summary>
        /// <param name="eid">ExamID</param>
        /// <returns></returns>
        public static double GetAvgExam(int eid)
        {
            var scores = GetAllExam(eid);
            return scores.Count == 0 ? 0 : GetAllExam(eid).Average(x => x.ScoreVal);
        }
        /// <summary>
        /// Get all grade - DataTable
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static DataTable GetAllGrade(int sid)
        {
            var sqlGet = "SELECT TeacherGrade.eduTgradeName AS GradeName, TeacherGrade.eduTgradeID AS GradeID, teacher.eduFirstName +' '+ teacher.eduLastName AS TeacherName,teacher.eduUserID AS TeacherID,student.eduFirstName + ' ' + student.eduLastName AS StudentName,student.eduUserID AS StudentID,grade.eduGradeName AS StudenTeacherGrade,grade.eduGradeID AS StudenTeacherGradeID,exam.eduExamDate AS ExamDate,exam.eduExamTitle AS ExamTitle,score.eduScore AS StudentScore FROM eduScores AS score, eduGrades AS grade, eduExams AS exam,eduTeacherGrades AS TeacherGrade, eduMembers AS teacher, eduMembers AS student WHERE (score.eduExamID=exam.eduExamID AND teacher.eduUserID = exam.eduTeacherID AND student.eduUserID=score.eduStudentID AND student.eduGradeID=grade.eduGradeID AND exam.eduTgradeID=TeacherGrade.eduTgradeID) AND score.eduScore<>-1 AND student.eduUserID="+sid;
            return Connect.GetData(sqlGet, "eduMembers");
        }

        public static List<Score> GetAllGradeScores(int tgid)
        {
            var sqlGet = "SELECT exam.eduYearPart AS YP, exam.eduPrecent AS ePrecent, score.eduScoreID AS eduScoreID,exam.eduExamID AS ExamID,TeacherGrade.eduTgradeName AS GradeName, TeacherGrade.eduTgradeID AS GradeID, teacher.eduFirstName +' '+ teacher.eduLastName AS TeacherName,teacher.eduUserID AS TeacherID,student.eduFirstName + ' ' + student.eduLastName AS StudentName,student.eduUserID AS StudentID,grade.eduGradeName AS StudenTeacherGrade,grade.eduGradeID AS StudenTeacherGradeID,exam.eduExamDate AS ExamDate,exam.eduExamTitle AS ExamTitle,score.eduScore AS StudentScore FROM eduScores AS score, eduGrades AS grade, eduExams AS exam,eduTeacherGrades AS TeacherGrade, eduMembers AS teacher, eduMembers AS student WHERE (score.eduExamID=exam.eduExamID AND teacher.eduUserID = exam.eduTeacherID AND student.eduUserID=score.eduStudentID AND student.eduGradeID=grade.eduGradeID AND exam.eduTgradeID=TeacherGrade.eduTgradeID AND TeacherGrade.eduTgradeID=" + tgid + ")";
            var dt = Connect.GetData(sqlGet, "eduScores");
            return (from DataRow dataRow in dt.Rows
                select new Score()
                {
                    Id = int.Parse(dataRow["eduScoreID"].ToString().Trim()),
                    Student = new Member() {UserID = int.Parse(dataRow["StudentID"].ToString().Trim())},
                    Exam = new Exam() {Id = int.Parse(dataRow["ExamID"].ToString().Trim()), YearPart = dataRow["YP"].ToString().Trim(), Date = DateTime.Parse(dataRow["ExamDate"].ToString().Trim()), Title = dataRow["ExamTitle"].ToString().Trim(), TeacherId = int.Parse(dataRow["TeacherID"].ToString().Trim())},
                    ScoreVal = int.Parse(dataRow["StudentScore"].ToString().Trim())
                }).ToList();
        }
        /// <summary>
        /// Get student avg
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static double GetStudentAvg(int sid)
        {
            var mem = MemberService.GetUserPart(sid);
            if (mem == null || mem.Auth != MemberClearance.Student)
                return 0;
            var scores = GetAllStudent(sid);
            return scores.Count == 0 ? 0 : scores.Average(x => x.ScoreVal);
        }
        /// <summary>
        /// Get student avg
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="tgid"></param>
        /// <returns></returns>
        public static double GetStudentAvg(int sid,int tgid)
        {
            var mem = MemberService.GetUserPart(sid);
            if (mem == null || mem.Auth != MemberClearance.Student)
                return 0;
            var scores = GetAllStudent(sid, tgid);
            return scores.Count == 0 ? 0 : scores.Average(x => x.ScoreVal);
        }
        /// <summary>
        /// Get student avg final
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="tgid"></param>
        /// <returns></returns>
        public static double GetStudentAvgFinal(int sid, int tgid)
        {
            var mem = MemberService.GetUserPart(sid);
            if (mem == null || mem.Auth != MemberClearance.Student)
                return 0;
            var scores = GetAllStudent(sid, tgid);
            return scores.Count == 0 ? 0 : scores.Sum(scr => scr.ScoreVal * ((double) scr.Exam.Precent / 100));
        }
        /// <summary>
        /// Get student avg final
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="tgid"></param>
        /// <param name="yearPart"></param>
        /// <returns></returns>
        public static double GetStudentAvgFinal(int sid, int tgid,string yearPart)
        {
            var mem = MemberService.GetUserPart(sid);
            if (mem == null || mem.Auth != MemberClearance.Student)
                return 0;
            var scores = GetAllStudent(sid, tgid,yearPart);
            if (scores.Count == 0) return 0;
            return scores.Sum(scr => scr.ScoreVal * ((double) scr.Exam.Precent / 100));
        }
        /// <summary>
        /// Add new score
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public static bool Add(Score score)
        {
            var mem = MemberService.GetUserPart(score.Student.UserID);
            if (mem == null || mem.Auth != MemberClearance.Student)
                return false;
            return Exsits(score.Student.UserID, score.Exam.Id) ? Connect.InsertUpdateDelete("UPDATE eduScores SET eduStudentID=" + score.Student.UserID + ",eduExamID=" + score.Exam.Id + ",eduScore=" + score.ScoreVal + " WHERE  eduStudentID=" + score.Student.UserID + " AND eduExamID=" + score.Exam.Id + "") : Connect.InsertUpdateDelete("INSERT INTO eduScores (eduStudentID,eduExamID,eduScore) VALUES (" + score.Student.UserID + "," + score.Exam.Id + "," + score.ScoreVal + ")");
        }
        /// <summary>
        /// Check if exsit
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="eid"></param>
        /// <returns></returns>
        public static bool Exsits(int sid, int eid)
        {
            return int.Parse(Connect.GetObject("SELECT COUNT(*) FROM eduScores WHERE eduStudentID=" + sid + " AND eduExamID=" + eid + "").ToString()) > 0;
        }
        /// <summary>
        /// Reset scores
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public static bool ResetScores(int eid)
        {
            Connect.InsertUpdateDelete("DELETE FROM eduScores WHERE eduExamID=" + eid);

            foreach (var student in TeacherGradeService.GetStudents(ExamService.GetExam(eid).TeacherGradeId))
            {
                var score = new Score()
                {
                    Exam = new Exam() { Id = eid },
                    Student = new Member() { UserID = student.UserID },
                    ScoreVal = -1
                };
                Add(score);
            }
            return true;
        }

        /// <summary>
        /// Reset scores student
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="tgid"></param>
        /// <returns></returns>
        public static bool ResetScoresStudent(int sid,int tgid)
        {
            Connect.InsertUpdateDelete("DELETE FROM eduScores WHERE eduStudentID=" + sid);
            foreach (var exm in ExamService.GetExamsByTeacherGradeId(tgid))
            {
                var score = new Score()
                {
                    Exam = new Exam() { Id = exm.Id },
                    Student = new Member() { UserID = sid },
                    ScoreVal = -1
                };
                Add(score);
            }
            return true;
        }
    }
}
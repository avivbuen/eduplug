using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

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
        string sqlGet = "SELECT score.nhsScoreID AS nhsScoreID, exam.nhsExamID AS ExamID,tgrade.nhsTgradeName AS GradeName, tgrade.nhsTgradeID AS GradeID, teacher.nhsFirstName +' '+ teacher.nhsLastName AS TeacherName,teacher.nhsUserID AS TeacherID,student.nhsFirstName + ' ' + student.nhsLastName AS StudentName,student.nhsUserID AS StudentID,grade.nhsGradeName AS StudentGrade,grade.nhsGradeID AS StudentGradeID,exam.nhsExamDate AS ExamDate,exam.nhsExamTitle AS ExamTitle,score.nhsScore AS StudentScore FROM nhsScores AS score, nhsGrades AS grade, nhsExams AS exam,nhsTeacherGrades AS tgrade, nhsMembers AS teacher, nhsMembers AS student WHERE (score.nhsExamID=exam.nhsExamID AND teacher.nhsUserID = exam.nhsTeacherID AND student.nhsUserID=score.nhsStudentID AND student.nhsGradeID=grade.nhsGradeID AND exam.nhsTgradeID=tgrade.nhsTgradeID)";
        List<Score> scores = new List<Score>();
        DataTable dt = Connect.GetData(sqlGet, "nhsScores");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Score c = new Score()
            {
                ID = int.Parse(dt.Rows[i]["nhsScoreID"].ToString().Trim()),
                Student = new Member() { Name = dt.Rows[i]["StudentName"].ToString().Trim(), UserID = int.Parse(dt.Rows[i]["StudentID"].ToString().Trim()) },
                Exam = new Exam() { ID = int.Parse(dt.Rows[i]["ExamID"].ToString().Trim()), Date = DateTime.Parse(dt.Rows[i]["ExamDate"].ToString().Trim()), Title = dt.Rows[i]["ExamTitle"].ToString().Trim(), TeacherID = int.Parse(dt.Rows[i]["TeacherID"].ToString().Trim()) },
                ScoreVal = int.Parse(dt.Rows[i]["StudentScore"].ToString().Trim())
            };
            scores.Add(c);
        }
        return scores;
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
        string sqlGet = "SELECT score.nhsScoreID AS nhsScoreID,exam.nhsExamID AS ExamID,tgrade.nhsTgradeName AS GradeName, tgrade.nhsTgradeID AS GradeID, teacher.nhsFirstName +' '+ teacher.nhsLastName AS TeacherName,teacher.nhsUserID AS TeacherID,student.nhsFirstName + ' ' + student.nhsLastName AS StudentName,student.nhsUserID AS StudentID,grade.nhsGradeName AS StudentGrade,grade.nhsGradeID AS StudentGradeID,exam.nhsExamDate AS ExamDate,exam.nhsExamTitle AS ExamTitle,score.nhsScore AS StudentScore FROM nhsScores AS score, nhsGrades AS grade, nhsExams AS exam,nhsTeacherGrades AS tgrade, nhsMembers AS teacher, nhsMembers AS student WHERE (score.nhsExamID=exam.nhsExamID AND teacher.nhsUserID = exam.nhsTeacherID AND student.nhsUserID=score.nhsStudentID AND student.nhsGradeID=grade.nhsGradeID AND exam.nhsTgradeID=tgrade.nhsTgradeID AND nhsStudentID=" + sid + ")";
        List<Score> scores = new List<Score>();
        DataTable dt = Connect.GetData(sqlGet, "nhsScores");
        Member mem = MemberService.GetUserPart(sid);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (int.Parse(dt.Rows[i]["StudentScore"].ToString().Trim()) != -1)
            {
                Score c = new Score()
                {
                    ID = int.Parse(dt.Rows[i]["nhsScoreID"].ToString().Trim()),
                    Student = mem,
                    Exam = new Exam() { ID = int.Parse(dt.Rows[i]["ExamID"].ToString().Trim()), Date = DateTime.Parse(dt.Rows[i]["ExamDate"].ToString().Trim()), Title = dt.Rows[i]["ExamTitle"].ToString().Trim(), TeacherID = int.Parse(dt.Rows[i]["TeacherID"].ToString().Trim()) },
                    ScoreVal = int.Parse(dt.Rows[i]["StudentScore"].ToString().Trim())
                };
                scores.Add(c);
            }
        }
        return scores;
    }
    /// <summary>
    /// Get all scores of student in specific tgrade
    /// </summary>
    /// <param name="sid"></param>
    /// <param name="tgid"></param>
    /// <returns></returns>
    public static List<Score> GetAllStudent(int sid,int tgid)
    {
        Member mem = MemberService.GetUserPart(sid);
        if (mem == null || mem.Auth != MemberClearance.Student)
            return new List<Score>();
        string sqlGet = "SELECT exam.nhsPrecent AS ePrecent, score.nhsScoreID AS nhsScoreID,exam.nhsExamID AS ExamID,tgrade.nhsTgradeName AS GradeName, tgrade.nhsTgradeID AS GradeID, teacher.nhsFirstName +' '+ teacher.nhsLastName AS TeacherName,teacher.nhsUserID AS TeacherID,student.nhsFirstName + ' ' + student.nhsLastName AS StudentName,student.nhsUserID AS StudentID,grade.nhsGradeName AS StudentGrade,grade.nhsGradeID AS StudentGradeID,exam.nhsExamDate AS ExamDate,exam.nhsExamTitle AS ExamTitle,score.nhsScore AS StudentScore FROM nhsScores AS score, nhsGrades AS grade, nhsExams AS exam,nhsTeacherGrades AS tgrade, nhsMembers AS teacher, nhsMembers AS student WHERE (score.nhsExamID=exam.nhsExamID AND teacher.nhsUserID = exam.nhsTeacherID AND student.nhsUserID=score.nhsStudentID AND student.nhsGradeID=grade.nhsGradeID AND exam.nhsTgradeID=tgrade.nhsTgradeID AND nhsStudentID=" + sid + " AND tgrade.nhsTgradeID=" + tgid + ")";
        List<Score> scores = new List<Score>();
        DataTable dt = Connect.GetData(sqlGet, "nhsScores");
        
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (int.Parse(dt.Rows[i]["StudentScore"].ToString().Trim()) != -1)
            {
                Score c = new Score()
                {
                    ID = int.Parse(dt.Rows[i]["nhsScoreID"].ToString().Trim()),
                    Student = mem,
                    Exam = new Exam() { ID = int.Parse(dt.Rows[i]["ExamID"].ToString().Trim()), Date = DateTime.Parse(dt.Rows[i]["ExamDate"].ToString().Trim()), Title = dt.Rows[i]["ExamTitle"].ToString().Trim(), TeacherID = int.Parse(dt.Rows[i]["TeacherID"].ToString().Trim()), Precent= int.Parse(dt.Rows[i]["ePrecent"].ToString().Trim()) },
                    ScoreVal = int.Parse(dt.Rows[i]["StudentScore"].ToString().Trim())
                };
                scores.Add(c);
            }
        }
        return scores;
    }
    /// <summary>
    /// Get all scores of student in specific tgrade and year part
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
        string sqlGet = "SELECT exam.nhsPrecent AS ePrecent, score.nhsScoreID AS nhsScoreID,exam.nhsExamID AS ExamID,tgrade.nhsTgradeName AS GradeName, tgrade.nhsTgradeID AS GradeID, teacher.nhsFirstName +' '+ teacher.nhsLastName AS TeacherName,teacher.nhsUserID AS TeacherID,student.nhsFirstName + ' ' + student.nhsLastName AS StudentName,student.nhsUserID AS StudentID,grade.nhsGradeName AS StudentGrade,grade.nhsGradeID AS StudentGradeID,exam.nhsExamDate AS ExamDate,exam.nhsExamTitle AS ExamTitle,score.nhsScore AS StudentScore FROM nhsScores AS score, nhsGrades AS grade, nhsExams AS exam,nhsTeacherGrades AS tgrade, nhsMembers AS teacher, nhsMembers AS student WHERE (score.nhsExamID=exam.nhsExamID AND teacher.nhsUserID = exam.nhsTeacherID AND student.nhsUserID=score.nhsStudentID AND student.nhsGradeID=grade.nhsGradeID AND exam.nhsTgradeID=tgrade.nhsTgradeID AND exam.nhsYearPart='"+yearPart+"' AND nhsStudentID=" + sid + " AND tgrade.nhsTgradeID=" + tgid + ")";
        List<Score> scores = new List<Score>();
        DataTable dt = Connect.GetData(sqlGet, "nhsScores");

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (int.Parse(dt.Rows[i]["StudentScore"].ToString().Trim()) != -1)
            {
                Score c = new Score()
                {
                    ID = int.Parse(dt.Rows[i]["nhsScoreID"].ToString().Trim()),
                    Student = mem,
                    Exam = new Exam() { ID = int.Parse(dt.Rows[i]["ExamID"].ToString().Trim()), Date = DateTime.Parse(dt.Rows[i]["ExamDate"].ToString().Trim()), Title = dt.Rows[i]["ExamTitle"].ToString().Trim(), TeacherID = int.Parse(dt.Rows[i]["TeacherID"].ToString().Trim()), Precent = int.Parse(dt.Rows[i]["ePrecent"].ToString().Trim()) },
                    ScoreVal = int.Parse(dt.Rows[i]["StudentScore"].ToString().Trim())
                };
                scores.Add(c);
            }
        }
        return scores;
    }
    /// <summary>
    /// Get all scores of student in specific tgrade with empty scores
    /// </summary>
    /// <param name="sid"></param>
    /// <param name="tgid"></param>
    /// <returns></returns>
    public static List<Score> GetAllStudentWE(int sid,int tgid)
    {
        if (MemberService.GetUserPart(sid) == null || MemberService.GetUserPart(sid).Auth != MemberClearance.Student)
            return new List<Score>();
        string sqlGet = "SELECT score.nhsScoreID AS nhsScoreID,exam.nhsExamID AS ExamID,tgrade.nhsTgradeName AS GradeName, tgrade.nhsTgradeID AS GradeID, teacher.nhsFirstName +' '+ teacher.nhsLastName AS TeacherName,teacher.nhsUserID AS TeacherID,student.nhsFirstName + ' ' + student.nhsLastName AS StudentName,student.nhsUserID AS StudentID,grade.nhsGradeName AS StudentGrade,grade.nhsGradeID AS StudentGradeID,exam.nhsExamDate AS ExamDate,exam.nhsExamTitle AS ExamTitle,score.nhsScore AS StudentScore FROM nhsScores AS score, nhsGrades AS grade, nhsExams AS exam,nhsTeacherGrades AS tgrade, nhsMembers AS teacher, nhsMembers AS student WHERE (score.nhsExamID=exam.nhsExamID AND teacher.nhsUserID = exam.nhsTeacherID AND student.nhsUserID=score.nhsStudentID AND student.nhsGradeID=grade.nhsGradeID AND exam.nhsTgradeID=tgrade.nhsTgradeID AND nhsStudentID=" + sid + " AND tgrade.nhsTgradeID=" + tgid+")";
        List<Score> scores = new List<Score>();
        DataTable dt = Connect.GetData(sqlGet, "nhsScores");
        Member mem = MemberService.GetUserPart(sid);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
                Score c = new Score()
                {
                    ID = int.Parse(dt.Rows[i]["nhsScoreID"].ToString().Trim()),
                    Student = mem,
                    Exam = new Exam() { ID = int.Parse(dt.Rows[i]["ExamID"].ToString().Trim()), Date = DateTime.Parse(dt.Rows[i]["ExamDate"].ToString().Trim()), Title = dt.Rows[i]["ExamTitle"].ToString().Trim(), TeacherID = int.Parse(dt.Rows[i]["TeacherID"].ToString().Trim()) },
                    ScoreVal = int.Parse(dt.Rows[i]["StudentScore"].ToString().Trim())
                };
                scores.Add(c);
        }
        return scores;
    }
    /// <summary>
    /// Get all scores of student in specific tgrade and year part with empty scores
    /// </summary>
    /// <param name="sid"></param>
    /// <param name="tgid"></param>
    /// <param name="yearPart"></param>
    /// <returns></returns>
    public static List<Score> GetAllStudentWE(int sid, int tgid,string yearPart)
    {
        Member mem = MemberService.GetUserPart(sid);
        if (mem == null || mem.Auth != MemberClearance.Student)
            return new List<Score>();
        string sqlGet = "SELECT score.nhsScoreID AS nhsScoreID,exam.nhsExamID AS ExamID,tgrade.nhsTgradeName AS GradeName, tgrade.nhsTgradeID AS GradeID, teacher.nhsFirstName +' '+ teacher.nhsLastName AS TeacherName,teacher.nhsUserID AS TeacherID,student.nhsFirstName + ' ' + student.nhsLastName AS StudentName,student.nhsUserID AS StudentID,grade.nhsGradeName AS StudentGrade,grade.nhsGradeID AS StudentGradeID,exam.nhsExamDate AS ExamDate,exam.nhsExamTitle AS ExamTitle,score.nhsScore AS StudentScore FROM nhsScores AS score, nhsGrades AS grade, nhsExams AS exam,nhsTeacherGrades AS tgrade, nhsMembers AS teacher, nhsMembers AS student WHERE (score.nhsExamID=exam.nhsExamID AND teacher.nhsUserID = exam.nhsTeacherID AND student.nhsUserID=score.nhsStudentID AND student.nhsGradeID=grade.nhsGradeID AND exam.nhsTgradeID=tgrade.nhsTgradeID AND nhsStudentID=" + sid + " AND exam.nhsYearPart='"+yearPart+"' AND tgrade.nhsTgradeID=" + tgid + ")";
        List<Score> scores = new List<Score>();
        DataTable dt = Connect.GetData(sqlGet, "nhsScores");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Score c = new Score()
            {
                ID = int.Parse(dt.Rows[i]["nhsScoreID"].ToString().Trim()),
                Student = mem,
                Exam = new Exam() { ID = int.Parse(dt.Rows[i]["ExamID"].ToString().Trim()), Date = DateTime.Parse(dt.Rows[i]["ExamDate"].ToString().Trim()), Title = dt.Rows[i]["ExamTitle"].ToString().Trim(), TeacherID = int.Parse(dt.Rows[i]["TeacherID"].ToString().Trim()) },
                ScoreVal = int.Parse(dt.Rows[i]["StudentScore"].ToString().Trim())
            };
            scores.Add(c);
        }
        return scores;
    }
    /// <summary>
    /// Get all the scores of an exam
    /// </summary>
    /// <param name="eid"></param>
    /// <returns></returns>
    public static List<Score> GetAllExam(int eid)
    {
        string sqlGet = "SELECT score.nhsScoreID AS nhsScoreID,exam.nhsExamID AS ExamID,tgrade.nhsTgradeName AS GradeName, tgrade.nhsTgradeID AS GradeID, teacher.nhsFirstName +' '+ teacher.nhsLastName AS TeacherName,teacher.nhsUserID AS TeacherID,student.nhsFirstName + ' ' + student.nhsLastName AS StudentName,student.nhsUserID AS StudentID,grade.nhsGradeName AS StudentGrade,grade.nhsGradeID AS StudentGradeID,exam.nhsExamDate AS ExamDate,exam.nhsExamTitle AS ExamTitle,score.nhsScore AS StudentScore FROM nhsScores AS score, nhsGrades AS grade, nhsExams AS exam,nhsTeacherGrades AS tgrade, nhsMembers AS teacher, nhsMembers AS student WHERE (score.nhsExamID=exam.nhsExamID AND teacher.nhsUserID = exam.nhsTeacherID AND student.nhsUserID=score.nhsStudentID AND student.nhsGradeID=grade.nhsGradeID AND exam.nhsTgradeID=tgrade.nhsTgradeID AND nhsExamID=" + eid + ")";
        List<Score> scores = new List<Score>();
        DataTable dt = Connect.GetData(sqlGet, "nhsScores");
        Exam exm = ExamService.GetExam(eid);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (int.Parse(dt.Rows[i]["StudentScore"].ToString().Trim()) != -1)
            {
                Score c = new Score()
                {
                    ID = int.Parse(dt.Rows[i]["nhsScoreID"].ToString().Trim()),
                    Student = new Member() { Name = dt.Rows[i]["StudentName"].ToString().Trim(), UserID = int.Parse(dt.Rows[i]["StudentID"].ToString().Trim()) },
                    Exam = exm,
                    ScoreVal = int.Parse(dt.Rows[i]["StudentScore"].ToString().Trim())
                };
                scores.Add(c);
            }
        }
        return scores;
    }
    /// <summary>
    /// Get score
    /// </summary>
    /// <param name="sid">Student ID</param>
    /// <param name="eid">Exam ID</param>
    /// <returns></returns>
    public static Score GetScore(int sid, int eid)
    {
        string sqlGet = "SELECT score.nhsScoreID AS nhsScoreID,exam.nhsExamID AS ExamID,tgrade.nhsTgradeName AS GradeName, tgrade.nhsTgradeID AS GradeID, teacher.nhsFirstName +' '+ teacher.nhsLastName AS TeacherName,teacher.nhsUserID AS TeacherID,student.nhsFirstName + ' ' + student.nhsLastName AS StudentName,student.nhsUserID AS StudentID,grade.nhsGradeName AS StudentGrade,grade.nhsGradeID AS StudentGradeID,exam.nhsExamDate AS ExamDate,exam.nhsExamTitle AS ExamTitle,score.nhsScore AS StudentScore FROM nhsScores AS score, nhsGrades AS grade, nhsExams AS exam,nhsTeacherGrades AS tgrade, nhsMembers AS teacher, nhsMembers AS student WHERE (score.nhsExamID=exam.nhsExamID AND teacher.nhsUserID = exam.nhsTeacherID AND student.nhsUserID=score.nhsStudentID AND student.nhsGradeID=grade.nhsGradeID AND exam.nhsTgradeID=tgrade.nhsTgradeID AND exam.nhsExamID=" + eid + " AND score.nhsStudentID=" + sid + ")";
        DataTable dt = Connect.GetData(sqlGet, "nhsScores");
        if (dt.Rows.Count == 0)
        {
            return null;
        }
        Score c = new Score()
        {
            ID = int.Parse(dt.Rows[0]["nhsScoreID"].ToString().Trim()),
            Student = new Member() { Name = dt.Rows[0]["StudentName"].ToString().Trim(), UserID = int.Parse(dt.Rows[0]["StudentID"].ToString().Trim()) },
            ScoreVal = int.Parse(dt.Rows[0]["StudentScore"].ToString().Trim())
        };
        return c;
    }
    /// <summary>
    /// Get avg of exam
    /// </summary>
    /// <param name="eid">ExamID</param>
    /// <returns></returns>
    public static double GetAvgExam(int eid)
    {
        return GetAllExam(eid).Average(x => x.ScoreVal);
    }
    /// <summary>
    /// Get all grade - DataTable
    /// </summary>
    /// <param name="sid"></param>
    /// <returns></returns>
    public static DataTable GetAllGrade(int sid)
    {
        string sqlGet = "SELECT tgrade.nhsTgradeName AS GradeName, tgrade.nhsTgradeID AS GradeID, teacher.nhsFirstName +' '+ teacher.nhsLastName AS TeacherName,teacher.nhsUserID AS TeacherID,student.nhsFirstName + ' ' + student.nhsLastName AS StudentName,student.nhsUserID AS StudentID,grade.nhsGradeName AS StudentGrade,grade.nhsGradeID AS StudentGradeID,exam.nhsExamDate AS ExamDate,exam.nhsExamTitle AS ExamTitle,score.nhsScore AS StudentScore FROM nhsScores AS score, nhsGrades AS grade, nhsExams AS exam,nhsTeacherGrades AS tgrade, nhsMembers AS teacher, nhsMembers AS student WHERE (score.nhsExamID=exam.nhsExamID AND teacher.nhsUserID = exam.nhsTeacherID AND student.nhsUserID=score.nhsStudentID AND student.nhsGradeID=grade.nhsGradeID AND exam.nhsTgradeID=tgrade.nhsTgradeID) AND score.nhsScore<>-1 AND student.nhsUserID="+sid;
        return Connect.GetData(sqlGet, "nhsMembers");
    }
    /// <summary>
    /// Get student avg
    /// </summary>
    /// <param name="sid"></param>
    /// <returns></returns>
    public static double GetStudentAvg(int sid)
    {
        Member mem = MemberService.GetUserPart(sid);
        if (mem == null || mem.Auth != MemberClearance.Student)
            return 0;
        List<Score> scores = GetAllStudent(sid);
        if (scores.Count == 0) return 0;
        return scores.Average(x => x.ScoreVal);
    }
    /// <summary>
    /// Get student avg
    /// </summary>
    /// <param name="sid"></param>
    /// <param name="tgid"></param>
    /// <returns></returns>
    public static double GetStudentAvg(int sid,int tgid)
    {
        Member mem = MemberService.GetUserPart(sid);
        if (mem == null || mem.Auth != MemberClearance.Student)
            return 0;
        List<Score> scores = GetAllStudent(sid, tgid);
        if (scores.Count == 0) return 0;
        return scores.Average(x => x.ScoreVal);
    }
    /// <summary>
    /// Get student avg final
    /// </summary>
    /// <param name="sid"></param>
    /// <param name="tgid"></param>
    /// <returns></returns>
    public static double GetStudentAvgFinal(int sid, int tgid)
    {
        Member mem = MemberService.GetUserPart(sid);
        if (mem == null || mem.Auth != MemberClearance.Student)
            return 0;
        List<Score> scores = GetAllStudent(sid, tgid);
        if (scores.Count == 0) return 0;
        double avg = 0;
        foreach (Score scr in scores)
        {
            avg += scr.ScoreVal * ((double)scr.Exam.Precent / 100);
        }
        return avg;
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
        Member mem = MemberService.GetUserPart(sid);
        if (mem == null || mem.Auth != MemberClearance.Student)
            return 0;
        List<Score> scores = GetAllStudent(sid, tgid,yearPart);
        if (scores.Count == 0) return 0;
        double avg = 0;
        foreach (Score scr in scores)
        {
            avg += scr.ScoreVal * ((double)scr.Exam.Precent / 100);
        }
        return avg;
    }
    /// <summary>
    /// Add new score
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public static bool Add(Score score)
    {
        Member mem = MemberService.GetUserPart(score.Student.UserID);
        if (mem == null || mem.Auth != MemberClearance.Student)
            return false;
        if (Exsits(score.Student.UserID, score.Exam.ID))
            return Connect.InsertUpdateDelete("UPDATE nhsScores SET nhsStudentID=" + score.Student.UserID + ",nhsExamID=" + score.Exam.ID + ",nhsScore=" + score.ScoreVal + " WHERE  nhsStudentID=" + score.Student.UserID + " AND nhsExamID=" + score.Exam.ID + "");
        return Connect.InsertUpdateDelete("INSERT INTO nhsScores (nhsStudentID,nhsExamID,nhsScore) VALUES (" + score.Student.UserID + "," + score.Exam.ID + "," + score.ScoreVal + ")");
    }
    /// <summary>
    /// Check if exsit
    /// </summary>
    /// <param name="sid"></param>
    /// <param name="eid"></param>
    /// <returns></returns>
    public static bool Exsits(int sid, int eid)
    {
        return int.Parse(Connect.GetObject("SELECT COUNT(*) FROM nhsScores WHERE nhsStudentID=" + sid + " AND nhsExamID=" + eid + "").ToString()) > 0;
    }
    /// <summary>
    /// Reset scores
    /// </summary>
    /// <param name="eid"></param>
    /// <returns></returns>
    public static bool ResetScores(int eid)
    {
        Connect.InsertUpdateDelete("DELETE FROM nhsScores WHERE nhsExamID=" + eid);

        foreach (Member student in tGradeService.GetStudents(ExamService.GetExam(eid).TeacherGradeID))
        {
            Score score = new Score()
            {
                Exam = new Exam() { ID = eid },
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
        Connect.InsertUpdateDelete("DELETE FROM nhsScores WHERE nhsStudentID=" + sid);
        foreach (Exam exm in ExamService.GetExamsByTgradeID(tgid))
        {
            Score score = new Score()
            {
                Exam = new Exam() { ID = exm.ID },
                Student = new Member() { UserID = sid },
                ScoreVal = -1
            };
            Add(score);
        }
        return true;
    }
}
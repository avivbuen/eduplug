namespace Business_Logic.TeacherGrades
{
    /// <summary>
    /// Teacher Grade Structure
    /// </summary>
    public class TeacherGrade : Grades.Grade
    {
        public int TeacherId { get; set; }
    }
}
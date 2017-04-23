using System;

namespace Business_Logic.Exams
{
    /// <summary>
    /// Exam
    /// </summary>
    public class Exam
    { 
        /// <summary>
        /// The id of the exam
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The title of the exam
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The id of the teacher that conducts this exam
        /// </summary>
        public int TeacherId { get; set; }
        /// <summary>
        /// The date of the exam
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// The precent of the exam
        /// </summary>
        public int Precent { get; set; }
        /// <summary>
        /// The teacher grade id of exam
        /// </summary>
        public int TeacherGradeId { get; set; }
        /// <summary>
        /// The year part
        /// </summary>
        public string YearPart { get; set; }
    }
}
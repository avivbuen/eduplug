using System;

namespace Business_Logic.Disciplines
{
    /// <summary>
    /// Discipline Event
    /// </summary>
    public class DisciplineEvent
    {
        /// <summary>
        /// The id of the lesson
        /// </summary>
        public int LessonId { get; set; }
        /// <summary>
        /// The id of the disciplines type
        /// </summary>
        public int DisciplinesId { get; set; }
        /// <summary>
        /// The id of the student
        /// </summary>
        public int StudentId { get; set; }
        /// <summary>
        /// The date of the event
        /// </summary>
        public DateTime Date { get; set; }
    }
}
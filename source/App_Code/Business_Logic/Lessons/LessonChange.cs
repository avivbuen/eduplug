using System;

namespace Business_Logic.Lessons
{
    /// <summary>
    /// Lesson Change
    /// </summary>
    public class LessonChange
    {
        /// <summary>
        /// The id of the lesson change
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The id of the lesson
        /// </summary>
        public int LessonId { get; set; }
        /// <summary>
        /// The date of the change
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// The change type
        /// </summary>
        public LessonChangeType ChangeType { get; set; }
        /// <summary>
        /// The message for the change
        /// </summary>
        public string Message { get; set; }
    }
}
using System.Collections.Generic;

namespace Business_Logic.Lessons
{
    /// <summary>
    /// LessonGroup
    /// </summary>
    public class LessonGroup
    {
        /// <summary>
        /// The lessons
        /// </summary>
        public List<Lesson> Lessons;
        /// <summary>
        /// CTOR
        /// </summary>
        public LessonGroup()
        {
            Lessons = new List<Lesson>();
        }
    }
}
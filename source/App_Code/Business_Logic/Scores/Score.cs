using Business_Logic.Exams;
using Business_Logic.Members;

namespace Business_Logic.Scores
{
    /// <summary>
    /// Score
    /// </summary>
    public class Score
    {
        /// <summary>
        /// The id of the score
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The studetn
        /// </summary>
        public Member Student { get; set; }
        /// <summary>
        /// The exam
        /// </summary>
        public Exam Exam { get; set; }
        /// <summary>
        /// The value of the score
        /// </summary>
        public int ScoreVal { get; set; }

    }
}
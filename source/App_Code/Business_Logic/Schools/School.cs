using Business_Logic.Members;
using Business_Logic.Cities;

namespace Business_Logic.Schools
{
    /// <summary>
    /// Summary description for School
    /// </summary>
    public class School
    {
        /// <summary>
        /// The name of the school
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The id of the school
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The logo path of the school
        /// </summary>
        public string LogoPath { get; set; }
        /// <summary>
        /// Is the school officialy recognized by the department of education
        /// </summary>
        public bool Official { get; set; }
        /// <summary>
        /// The school manager
        /// </summary>
        public Member Manager { get; set; }
        /// <summary>
        /// The mid shool manager
        /// </summary>
        public Member MidManger { get; set; }
        /// <summary>
        /// The city of the school
        /// </summary>
        public City City { get; set; }
    }
}
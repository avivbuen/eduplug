using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Business_Logic.Cities;
using Business_Logic.Grades;
using Business_Logic.Majors;
using Business_Logic.Schools;

namespace Business_Logic.Members
{
    /// <summary>
    /// Member structure for business logic
    /// </summary>
    public class Member
    {
        //GET/SET
        /// <summary>
        /// The DB id of the user (A_I-PK) - DO NOT ENTER YOURSELF
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// The israeli/passport id of user
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// The first name of the user
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// The last name of the user
        /// </summary>
        public string LastName { get; set; }
        public string Name
        {
            get
            {
                return this.FirstName + ' ' + this.LastName;
            }
            set
            {
                string[] val = value.Split(' ');
                if (val.Length >= 2)
                {
                    FirstName = val[0];
                    string lname = "";
                    for (int i = 1; i < val.Length; i++)
                    {
                        lname += val[i] + " ";
                    }
                    LastName = lname.Trim();
                }
            }
        }
        /// <summary>
        /// The E-Mail of the user
        /// </summary>
        public string Mail { get; set; }
        /// <summary>
        /// The type of the user - The auth(Admin/Student/Teacher)
        /// </summary>
        public MemberClearance Auth { get; set; }
        /// <summary>
        /// The gender of the user
        /// </summary>
        public MemberGender Gender { get; set; }
        /// <summary>
        /// The date that the user was born in
        /// </summary>
        public DateTime BornDate { get; set; }
        /// <summary>
        /// The majors of the user(Computer Science, Movie Director, etc.)
        /// </summary>
        public List<Major> Majors { get; set; }
        /// <summary>
        /// The registeration time of the user
        /// </summary>
        public DateTime RegisterationDate { get; set; }
        /// <summary>
        /// The path that the user profile picture is located in
        /// </summary>
        public string PicturePath { get; set; }

        /// <summary>
        /// The id of the user grade
        /// </summary>
        public int GradeID {
            get
            {
                return Grade == null ? 0 : Grade.Id;
            }
            set
            {
                if(Grade==null) Grade=new Grade(){Id=value}; else Grade.Id = value;
            }
        }
        /// <summary>
        /// The grade of the user
        /// </summary>
        public Grade Grade { get; set; }
        /// <summary>
        /// The school of the user
        /// </summary>
        public School School { get; set; }
        /// <summary>
        /// The city of the user
        /// </summary>
        public City City { get; set; }
        /// <summary>
        /// The user state active/not active
        /// </summary>
        public string Active { get; set; }
        //END GET/SET 
        public string Phone { get; set; }
    }
}
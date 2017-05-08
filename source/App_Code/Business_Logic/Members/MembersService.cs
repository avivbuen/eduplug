using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Threading;
using System.Web;
using Business_Logic.Cities;
using Business_Logic.Grades;
using Business_Logic.Lessons;
using Business_Logic.Majors;
using Business_Logic.Schools;
using Business_Logic.TeacherGrades;
using OfficeOpenXml.FormulaParsing.Excel.Operators;

namespace Business_Logic.Members
{
    /// <summary>
    /// A service class for member class
    /// DB Connector
    /// </summary>
    public static class MemberService
    {
        /// <summary>
        /// Get allowed by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Member GetAllowed(string id)
        {
            var dt = Connect.GetData("SELECT * FROM eduMembers WHERE eduID='" + id + "' AND eduActive=", "eduMembers");
            if (dt.Rows.Count == 0) return new Member();
            var m = new Member()
            {
                FirstName = dt.Rows[0]["eduFirstName"].ToString().Trim(),
                LastName = dt.Rows[0]["eduLastName"].ToString().Trim(),
                ID = dt.Rows[0]["eduID"].ToString().Trim(),
                Auth = ((MemberClearance)dt.Rows[0]["eduType"].ToString().Trim()[0])
            };
            return m;
        }
        /// <summary>
        /// Gets all the members from the DB
        /// </summary>
        /// <returns>A list of member object</returns>
        public static List<Member> GetAll()
        {
            var dt = Connect.GetData("SELECT * FROM eduMembers", "eduMembers");
            var mems = new List<Member>();
            foreach (DataRow dr in dt.Rows)
            {
                var mem = new Member()
                {
                    UserID = int.Parse(dr["eduUserID"].ToString()),
                    ID = dr["eduID"].ToString(),
                    FirstName = dr["eduFirstName"].ToString(),
                    LastName = dr["eduLastName"].ToString(),
                    Mail = dr["eduMail"].ToString(),
                    Auth = ((MemberClearance)(char.Parse(dr["eduType"].ToString()))),
                    Gender = ((MemberGender)(char.Parse(dr["eduGender"].ToString()))),
                    BornDate = DateTime.Parse(dr["eduBorn"].ToString()),
                    PicturePath = dr["eduPicture"].ToString(),
                    GradeID = int.Parse(dr["eduGradeID"].ToString()),
                    City = CitiesService.GetCity(int.Parse(dr["eduCityID"].ToString().Trim())),
                    Majors = MajorsService.GetUserMajors(int.Parse(dr["eduUserID"].ToString())),
                    Active = dr["eduActive"].ToString().Trim(),
                    School = SchoolService.GetSchool(int.Parse(dr["eduSchoolID"].ToString()))
                };
                if (dr["eduDateRegister"].ToString() != "")
                    mem.RegisterationDate = DateTime.Parse(dr["eduDateRegister"].ToString());
                mems.Add(mem);
            }
            return mems;
        }
        /// <summary>
        /// Gets partially field member object for liter work loads that require the following:
        /// UserID, FirstName, LastName, Name, Auth, Active
        /// </summary>
        /// <returns></returns>
        public static List<Member> GetNames()
        {
            var dt = Connect.GetData("SELECT eduUserID,eduFirstName,eduLastName,eduType,eduActive FROM eduMembers WHERE eduSchoolID=" + GetCurrent().School.Id, "eduMembers");
            return (from DataRow dr in dt.Rows
                    select new Member()
                    {
                        UserID = int.Parse(dr["eduUserID"].ToString()),
                        FirstName = dr["eduFirstName"].ToString(),
                        LastName = dr["eduLastName"].ToString(),
                        Auth = ((MemberClearance)(char.Parse(dr["eduType"].ToString()))),
                        Active = dr["eduActive"].ToString().Trim()
                    }).ToList();
        }
        /// <summary>
        /// Gets all the members from the DB
        /// </summary>
        /// <returns>A DataTable of members</returns>
        public static DataTable GetAllDataTable()
        {
            var dt = Connect.GetData("SELECT * FROM eduMembers,eduCities WHERE eduCities.eduCityID=eduMembers.eduCityID AND eduMembers.eduActive='Yes' AND eduSchoolID=" + GetCurrent().School.Id, "eduMembers");
            return dt;
        }
        /// <summary>
        /// Get the current member from the DB
        /// </summary>
        /// <returns>A DataTable of members</returns>
        public static DataTable GetCurrentDataTable()
        {
            var dt = Connect.GetData("SELECT * FROM eduMembers,eduCities WHERE eduCities.eduCityID=eduMembers.eduCityID AND eduMembers.eduActive='Yes' AND eduMembers.eduUserID=" + GetCurrent().UserID, "eduMembers");
            return dt;
        }
        /// <summary>
        /// Login of the user - init for session - The session key is 'Member'
        /// </summary>
        /// <param name="email">Email to login</param>
        /// <param name="pass">Password to login</param>
        /// <returns>Whether the login was successful or not</returns>
        public static bool Login(string email, string pass)
        {
            email = email.Replace("'", "");
            pass = pass.Replace("'", "");
            var dt = Connect.GetData("SELECT * FROM eduMembers WHERE eduMail='" + email + "' AND eduActive='Yes'", "eduMembers");//Excuting a query to check up with the data base
            if (dt.Rows.Count == 0) return false;
            string dbHash = dt.Rows[0]["eduPass"].ToString();//Getting dbHash - The encrypted password that came from the data base for that user
            if (!Security.ValidatePassword(pass, dbHash)) return false;
            var m = new Member()
            {
                UserID = int.Parse(dt.Rows[0]["eduUserID"].ToString()),
                ID = dt.Rows[0]["eduID"].ToString(),
                FirstName = dt.Rows[0]["eduFirstName"].ToString(),
                LastName = dt.Rows[0]["eduLastName"].ToString(),
                Mail = dt.Rows[0]["eduMail"].ToString(),
                Auth = ((MemberClearance)(char.Parse(dt.Rows[0]["eduType"].ToString()))),
                Gender = ((MemberGender)(char.Parse(dt.Rows[0]["eduGender"].ToString()))),
                BornDate = DateTime.Parse(dt.Rows[0]["eduBorn"].ToString()),
                PicturePath = dt.Rows[0]["eduPicture"].ToString(),
                GradeID = int.Parse(dt.Rows[0]["eduGradeID"].ToString()),
                City = CitiesService.GetCity(int.Parse(dt.Rows[0]["eduCityID"].ToString().Trim())),
                Majors = MajorsService.GetUserMajors(int.Parse(dt.Rows[0]["eduUserID"].ToString())),
                Active = dt.Rows[0]["eduActive"].ToString().Trim(),
                School = SchoolService.GetSchool(int.Parse(dt.Rows[0]["eduSchoolID"].ToString()))
            };
            if (dt.Rows[0]["eduDateRegister"].ToString() != "")
                m.RegisterationDate = DateTime.Parse(dt.Rows[0]["eduDateRegister"].ToString());
            HttpContext.Current.Session["Member"] = m;
            if (HttpContext.Current.Application["Members"] == null)
            {
                var mems = new List<Member> { m };
                HttpContext.Current.Application["Members"] = mems;
            }
            else
            {
                var mems = (List<Member>)HttpContext.Current.Application["Members"];
                mems.Add(m);
                HttpContext.Current.Application["Members"] = mems;
            }
            return true;
        }
        /// <summary>
        /// Login of the user - init for session - The session key is 'Member'
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pass">Password to login</param>
        /// <param name="ids"></param>
        /// <returns>Whether the login was successful or not</returns>
        public static bool Login(string id, string pass, bool ids)
        {
            id = id.Replace("'", "");
            pass = pass.Replace("'", "");
            var dt = Connect.GetData("SELECT * FROM eduMembers WHERE eduID='" + id + "' AND eduActive='Yes'", "eduMembers");//Excuting a query to check up with the data base
            if (dt.Rows.Count == 0) return false;
            var dbHash = dt.Rows[0]["eduPass"].ToString();//Getting dbHash - The encrypted password that came from the data base for that user
            if (!Security.ValidatePassword(pass, dbHash)) return false;
            var m = new Member()
            {
                UserID = int.Parse(dt.Rows[0]["eduUserID"].ToString()),
                ID = dt.Rows[0]["eduID"].ToString(),
                FirstName = dt.Rows[0]["eduFirstName"].ToString(),
                LastName = dt.Rows[0]["eduLastName"].ToString(),
                Mail = dt.Rows[0]["eduMail"].ToString(),
                Auth = ((MemberClearance)(char.Parse(dt.Rows[0]["eduType"].ToString()))),
                Gender = ((MemberGender)(char.Parse(dt.Rows[0]["eduGender"].ToString()))),
                BornDate = DateTime.Parse(dt.Rows[0]["eduBorn"].ToString()),
                PicturePath = dt.Rows[0]["eduPicture"].ToString(),
                GradeID = int.Parse(dt.Rows[0]["eduGradeID"].ToString()),
                City = CitiesService.GetCity(int.Parse(dt.Rows[0]["eduCityID"].ToString().Trim())),
                Majors = MajorsService.GetUserMajors(int.Parse(dt.Rows[0]["eduUserID"].ToString())),
                Active = dt.Rows[0]["eduActive"].ToString().Trim(),
                School = SchoolService.GetSchool(int.Parse(dt.Rows[0]["eduSchoolID"].ToString()))
            };
            if (dt.Rows[0]["eduDateRegister"].ToString() != "")
                m.RegisterationDate = DateTime.Parse(dt.Rows[0]["eduDateRegister"].ToString());
            HttpContext.Current.Session["Member"] = m;
            if (HttpContext.Current.Application["Members"] == null)
            {
                var mems = new List<Member> { m };
                HttpContext.Current.Application["Members"] = mems;
            }
            else
            {
                var mems = (List<Member>)HttpContext.Current.Application["Members"];
                mems.Add(m);
                HttpContext.Current.Application["Members"] = mems;
            }
            return true;
        }

        public static bool Complete(Member m, string pass)
        {
            return Connect.InsertUpdateDelete("UPDATE eduMembers SET eduMail='" + m.Mail.Replace("'", "''") +
                                              "', eduPass='" + Security.CreateHash(pass) + "',eduPicture='" +
                                              m.PicturePath + "', eduActive='Yes',eduDateRegister=#" + Converter.GetFullTimeReadyForDataBase() + "#  WHERE eduUserID=" + m.UserID);
        }
        /// <summary>
        /// Login of the user - init for session - The session key is 'Member'
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pass">Password to login</param>
        /// <param name="scid">School ID</param>
        /// <returns>Whether the login was successful or not</returns>
        public static bool Login(string id, string pass, int scid)
        {
            id = id.Replace("'", "");
            pass = pass.Replace("'", "");
            var dt = Connect.GetData("SELECT * FROM eduMembers WHERE eduID='" + id + "' AND eduSchoolID=" + scid + " AND eduActive<>'No'", "eduMembers");//Excuting a query to check up with the data base
            if (dt.Rows.Count == 0) return false;
            var dbHash =
                dt.Rows[0]["eduPass"]
                    .ToString(); //Getting dbHash - The encrypted password that came from the data base for that user
            if (!Security.ValidatePassword(pass, dbHash)) return false;
            var m = new Member()
            {
                UserID = int.Parse(dt.Rows[0]["eduUserID"].ToString()),
                ID = dt.Rows[0]["eduID"].ToString(),
                FirstName = dt.Rows[0]["eduFirstName"].ToString(),
                LastName = dt.Rows[0]["eduLastName"].ToString(),
                Mail = dt.Rows[0]["eduMail"].ToString(),
                Auth = ((MemberClearance)(char.Parse(dt.Rows[0]["eduType"].ToString()))),
                Gender = ((MemberGender)(char.Parse(dt.Rows[0]["eduGender"].ToString()))),
                BornDate = DateTime.Parse(dt.Rows[0]["eduBorn"].ToString()),
                PicturePath = dt.Rows[0]["eduPicture"].ToString(),
                GradeID = int.Parse(dt.Rows[0]["eduGradeID"].ToString()),
                City = CitiesService.GetCity(int.Parse(dt.Rows[0]["eduCityID"].ToString().Trim())),
                Majors = MajorsService.GetUserMajors(int.Parse(dt.Rows[0]["eduUserID"].ToString())),
                Active = dt.Rows[0]["eduActive"].ToString().Trim(),
                School = SchoolService.GetSchool(int.Parse(dt.Rows[0]["eduSchoolID"].ToString()))
            };
            if (dt.Rows[0]["eduDateRegister"].ToString() != "")
                m.RegisterationDate = DateTime.Parse(dt.Rows[0]["eduDateRegister"].ToString());
            HttpContext.Current.Session["Member"] = m;
            if (HttpContext.Current.Application["Members"] == null)
            {
                var mems = new List<Member> { m };
                HttpContext.Current.Application["Members"] = mems;
            }
            else
            {
                var mems = (List<Member>)HttpContext.Current.Application["Members"];
                mems.Add(m);
                HttpContext.Current.Application["Members"] = mems;
            }

            return true;
        }
        /// <summary>
        /// Gets Clearances
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <param name="scid">The school id</param>
        /// <returns></returns>
        public static List<MemberClearance> GetClearances(string id, int scid)
        {
            DataTable dt;
            dt = scid == -1 ? Connect.GetData("SELECT * FROM eduMembers WHERE eduID='" + id + "'", "eduMembers") : Connect.GetData("SELECT * FROM eduMembers WHERE eduMembers.eduSchoolID=" + scid + " AND eduID='" + id + "'", "eduMembers");
            var clrs = (from DataRow dataRow in dt.Rows select ((MemberClearance)(char.Parse(dataRow["eduType"].ToString())))).ToList();
            return clrs;
        }
        /// <summary>
        /// Get's the current user from the session
        /// </summary>
        /// <returns>The current logged in user</returns>
        public static Member GetCurrent()
        {
            if (ValidateSessions(new string[1] { "Member" }, HttpContext.Current))
            {
                return ((Member)HttpContext.Current.Session["Member"]);
            }
            else
            {
                var m = new Member()
                {
                    FirstName = "non",
                    LastName = "non",
                    Auth = MemberClearance.Guest
                };
                return m;
            }
        }
        /// <summary>
        /// Gets all the children
        /// </summary>
        /// <param name="uid">User id</param>
        /// <param name="schoolId">Schoool id</param>
        /// <returns></returns>
        public static List<Member> GetChildren(int uid, int schoolId)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null &&
                HttpContext.Current.Session["kids"] != null)
                return (List<Member>)HttpContext.Current.Session["kids"];
            if (GetCurrent().Auth != MemberClearance.Parent) return new List<Member>();
            var dt = Connect.GetData("SELECT eduParentMember.eduChildID AS childID FROM eduParentMember,eduMembers WHERE eduMembers.eduUserID=eduParentMember.eduUserID AND eduMembers.eduSchoolID=" + schoolId + " AND eduParentMember.eduUserID=" + uid, "eduMembers");
            var mems = new List<Member>();
            foreach (DataRow dr in dt.Rows)
            {
                var mem = GetUser(int.Parse(dr["childID"].ToString()));
                mems.Add(mem);
            }
            if (HttpContext.Current == null) return mems;
            if (HttpContext.Current.Session != null) HttpContext.Current.Session["kids"] = mems;
            return mems;
        }
        /// <summary>
        /// Checks if the current parrent have a child with the id given
        /// </summary>
        /// <param name="childId">The id of the child</param>
        /// <returns></returns>
        public static bool HaveChild(int childId)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null &&
                HttpContext.Current.Session["kids"] != null)
                return ((List<Member>)HttpContext.Current.Session["kids"]).All(x => x.UserID == childId);
            return false;
        }

        /// <summary>
        /// Sets the current view selected child
        /// </summary>
        /// <param name="childId">The id of the child</param>
        /// <returns></returns>
        public static bool SetSelectedChild(int childId)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                HttpContext.Current.Session["sChildId"] = childId;
            return true;
        }
        /// <summary>
        /// Gets the current view selected child
        /// </summary>
        /// <returns></returns>
        public static Member GetSelectedChild()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null &&
                HttpContext.Current.Session["kids"] != null)
            {
                if (HttpContext.Current.Session["sChildId"] != null)
                {
                    return ((List<Member>)HttpContext.Current.Session["kids"]).SingleOrDefault(
                        x => x.UserID == int.Parse(HttpContext.Current.Session["sChildId"].ToString()));
                }

                return ((List<Member>)HttpContext.Current.Session["kids"]).First();
            }
            return new Member();
        }

        public static bool SetChildren(int uid, List<int> kidList)
        {
            if (uid == -1) return false;
            var con = new OleDbConnection(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
            var command = new OleDbCommand("", con);
            con.Open();
            var adp = new OleDbDataAdapter(command);
            foreach (int s in kidList)
            {
                command = new OleDbCommand("INSERT INTO eduParentMember (eduUserID,eduChildID) VALUES ("+uid+","+s+")",con);
                command.ExecuteNonQuery();
            }
            con.Close();
            return true;
        }
        public static List<School> GetSchools(string id)
        {
            var dt = Connect.GetData("SELECT eduMembers.eduSchoolID, eduSchools.eduSchoolName FROM eduMembers,eduSchools WHERE eduMembers.eduSchoolID=eduSchools.eduSchoolID AND eduID='" + id + "'", "eduMembers");
            var schools = (from DataRow dataRow in dt.Rows select new School() { Id = int.Parse(dataRow["eduSchoolID"].ToString()), Name = dataRow["eduSchoolName"].ToString() }).ToList();
            return schools;
        }
        /// <summary>
        /// Validates Session Keys (Their Values)
        /// </summary>
        /// <param name="sessionNames">The keys</param>
        /// <param name="c">The hyper text transfer protocol context</param>
        /// <returns>Whether the keys are empty or not(if one is empty then it is false)</returns>
        private static bool ValidateSessions(string[] sessionNames, HttpContext c)
        {
            return sessionNames.All(sName => c != null && c.Session != null && c.Session[sName] != null && c.Session[sName].ToString().Trim() != "");
        }
        /// <summary>
        /// Template for inserting a new member into the DB
        /// </summary>
        private const string FullInsertTemplate = "INSERT INTO eduMembers (eduFirstName,eduLastName,eduPass,eduType,eduMail,eduID,eduGender,eduGradeID,eduPicture,eduBorn,eduDateRegister,eduCityID,eduActive,eduSchoolID) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',{11},'Yes',{12})";
        /// <summary>
        /// Adds a user - ALL FIELDS MUST NOT BE EMPTY!
        /// </summary>
        /// <param name="m">The member to add</param>
        /// <param name="pass">The password of the member</param>
        public static void Add(Member m, string pass)
        {
            char status = Converter.GetClearnce(m.Auth)[0];
            char gen = Converter.GetGender(m.Gender)[0];
            //pass = pass.Replace("'", "''");
            Connect.InsertUpdateDelete(string.Format(FullInsertTemplate, m.FirstName, m.LastName, Security.CreateHash(pass), status, m.Mail, m.ID, gen, m.GradeID, m.PicturePath, Converter.GetTimeReadyForDataBase(m.BornDate), Converter.GetFullTimeReadyForDataBase(), m.City.Id, m.School.Id));

            if (m.Majors != null)
            {
                int userId = GetUID(m.ID, m.School.Id);
                foreach (Majors.Major maj in m.Majors)
                {
                    Connect.InsertUpdateDelete("INSERT INTO eduMajorsMembers (eduUserID,eduMajorID) VALUES (" + userId +
                                               "," + maj.Id + ")");
                    DataTable dt = Connect.GetData(
                        "SELECT * FROM eduMajorsTeacherGrades WHERE eduMajorID=" + maj.Id + " AND eduGradePart='" +
                        TeacherGradeService.GetParTeacherGrade(GradesService.Get(m.GradeID).Name).Replace("'", "''") +
                        "'", "eduMajorsTeacherGrades");
                    foreach (DataRow dr in dt.Rows)
                    {
                        int tgid = int.Parse(dr["eduTgradeID"].ToString());
                        TeacherGradeService.AddStudent(tgid, userId);
                    }
                }
            }
            UpdateAllowed(m.UserID);
        }

        /// <summary>
        /// Update the DB allowed table to active account -- just for tracking
        /// </summary>
        /// <param name="uid" param="">The user id</param>
        public static void UpdateAllowed(int uid)
        {
            Connect.InsertUpdateDelete("UPDATE eduMembers SET eduActive='Yes' WHERE eduID='" + uid + "'");
        }
        /// <summary>
        /// Updates the session to the following user
        /// </summary>
        /// <param name="m"></param>
        public static void UpdateCurrent(Member m)
        {
            HttpContext.Current.Session["Member"] = m;
        }
        /// <summary>
        /// Checks if the user is allowed to register - to prevent unwanted guests from registering
        /// </summary>
        /// <param name="fname">First Name</param>
        /// <param name="lname">Last Name</param>
        /// <param name="iuid">Israeli ID</param>
        /// <returns>UserAllowed</returns>
        public static bool IsAllowed(string fname, string lname, string iuid)
        {
            //DataTable dt1 = Connect.GetData("SELECT * FROM eduAllowed WHERE (eduFirstName='" + fname + "' AND eduLastName='" + lname + "' AND eduID='" + iuid + "')", "eduAllowed");
            DataTable dt2 = Connect.GetData("SELECT * FROM eduMembers WHERE (eduFirstName='" + fname + "' AND eduLastName='" + lname + "' AND eduID='" + iuid + "' AND eduActive='Wait')", "eduMembers");
            return (dt2.Rows.Count == 1);
        }
        /// <summary>
        /// Check if allowed exsit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool ExsitsAllowed(string id)
        {
            DataTable dt1 = Connect.GetData("SELECT * FROM eduMembers WHERE eduID='" + id + "' AND eduActive='Wait'", "eduMembers");
            return (dt1.Rows.Count != 0);
        }
        /// <summary>
        /// Gets the user auth
        /// </summary>
        /// <param name="uid">The user id</param>
        /// <returns></returns>
        public static MemberClearance GetClearance(string uid)
        {
            DataTable dt = Connect.GetData("SELECT * FROM eduMembers WHERE eduID='" + uid + "'", "eduMembers");
            if (dt.Rows.Count == 0)
                return MemberClearance.Guest;
            return ((MemberClearance)dt.Rows[0]["eduType"].ToString().ToCharArray()[0]);
        }
        /// <summary>
        /// Gets the user id with the id of the user
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static int GetUID(string ID)
        {
            DataTable dt = Connect.GetData("SELECT * FROM eduMembers WHERE eduID='" + ID + "'", "eduMembers");
            if (dt.Rows.Count == 0)
                return -1;
            return int.Parse(dt.Rows[0]["eduUserID"].ToString());
        }
        /// <summary>
        /// Gets the user id with the id of the user
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static int GetUID(string ID, int scid)
        {
            DataTable dt = Connect.GetData("SELECT * FROM eduMembers WHERE eduID='" + ID + "' AND eduSchoolID=" + scid, "eduMembers");
            if (dt.Rows.Count == 0)
                return -1;
            return int.Parse(dt.Rows[0]["eduUserID"].ToString());
        }
        /// <summary>
        /// Checks if the email already exsits in the database
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Whether the member exsit with the same email</returns>
        public static bool Exsits(string email)
        {
            int count = (int)Connect.GetObject("SELECT COUNT(*) FROM eduMembers WHERE eduMail='" + email + "'");
            return (count == 0);
        }
        /// <summary>
        /// Updates the user in the database - USER ID IS MUST
        /// </summary>
        /// <param name="m">The user</param>
        /// <returns>State</returns>
        public static bool Update(Member m)
        {
            int userID = GetUID(m.ID,GetCurrent().School.Id);
            Connect.InsertUpdateDelete("DELETE FROM eduMajorsMembers WHERE eduUserID=" + userID);
            var con = new OleDbConnection(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
            var command = new OleDbCommand("", con);
            con.Open();
            var adp = new OleDbDataAdapter(command);
            var ds = new DataSet();
            foreach (Majors.Major maj in m.Majors)
            {
                command= new OleDbCommand("INSERT INTO eduMajorsMembers (eduUserID,eduMajorID) VALUES (" + userID + "," + maj.Id + ")",con);
                command.ExecuteNonQuery();
            }
            con.Close();
            if (m.PicturePath != null)
                Connect.InsertUpdateDelete("UPDATE eduMembers SET eduFirstName='" + m.FirstName + "',eduLastName='" + m.LastName + "',eduType='" + ((char)m.Auth) + "',eduMail='" + m.Mail + "',eduID='" + m.ID + "',eduGender='" + ((char)m.Gender) + "',eduGradeID=" + m.GradeID + ",eduPicture='" + m.PicturePath + "',eduBorn='" + Converter.GetTimeReadyForDataBase(m.BornDate) + "',eduCityID='" + m.City.Id + "' WHERE eduUserID=" + m.UserID);
            else
                Connect.InsertUpdateDelete("UPDATE eduMembers SET eduFirstName='" + m.FirstName + "',eduLastName='" + m.LastName + "',eduType='" + ((char)m.Auth) + "',eduMail='" + m.Mail + "',eduID='" + m.ID + "',eduGender='" + ((char)m.Gender) + "',eduGradeID=" + m.GradeID + ",eduBorn='" + Converter.GetTimeReadyForDataBase(m.BornDate) + "',eduCityID='" + m.City.Id + "' WHERE eduUserID=" + m.UserID);
            if (m.UserID == GetCurrent().UserID)
                UpdateCurrent(GetUser(userID));
            return true;
        }
        /// <summary>
        /// Updates the user password in the database - USER ID IS MUST - PK
        /// </summary>
        /// <param name="uid">The user id</param>
        /// <param name="pass">The new password</param>
        /// <returns>State</returns>
        public static bool UpdatePassword(int uid, string pass)
        {
            return Connect.InsertUpdateDelete("UPDATE eduMembers SET eduPass='" + Security.CreateHash(pass) + "' WHERE eduUserID=" + uid);
        }
        /// <summary>
        /// Removes the user by the user id PK
        /// </summary>
        /// <param name="uid">User ID</param>
        /// <returns>Action State</returns>
        public static bool RemoveFromActive(string uid)
        {
            if (int.Parse(uid) == GetCurrent().UserID) HttpContext.Current.Session["Member"] = null;
            return Connect.InsertUpdateDelete("UPDATE eduMembers SET eduActive='No' WHERE eduUserID=" + uid);
        }
        /// <summary>
        /// Gets a user by his user id PK
        /// </summary>
        /// <param name="uid">User id</param>
        /// <returns></returns>
        public static Member GetUser(int uid)
        {
            DataTable dt = Connect.GetData("SELECT * FROM eduMembers WHERE eduUserID=" + uid, "eduMembers");
            if (dt.Rows.Count == 0)
                return null;
            Member m = new Member()
            {
                UserID = int.Parse(dt.Rows[0]["eduUserID"].ToString()),
                ID = dt.Rows[0]["eduID"].ToString(),
                FirstName = dt.Rows[0]["eduFirstName"].ToString(),
                LastName = dt.Rows[0]["eduLastName"].ToString(),
                Mail = dt.Rows[0]["eduMail"].ToString(),
                Auth = ((MemberClearance)(char.Parse(dt.Rows[0]["eduType"].ToString()))),
                Gender = ((MemberGender)(char.Parse(dt.Rows[0]["eduGender"].ToString()))),
                BornDate = DateTime.Parse(dt.Rows[0]["eduBorn"].ToString()),
                PicturePath = dt.Rows[0]["eduPicture"].ToString(),
                GradeID = int.Parse(dt.Rows[0]["eduGradeID"].ToString()),
                City = CitiesService.GetCity(int.Parse(dt.Rows[0]["eduCityID"].ToString().Trim())),
                Majors = MajorsService.GetUserMajors(int.Parse(dt.Rows[0]["eduUserID"].ToString())),
                Active = dt.Rows[0]["eduActive"].ToString().Trim(),
                School = SchoolService.GetSchool(int.Parse(dt.Rows[0]["eduSchoolID"].ToString()))
            };

            if (dt.Rows[0]["eduDateRegister"].ToString() != "")
                m.RegisterationDate = DateTime.Parse(dt.Rows[0]["eduDateRegister"].ToString());
            return m;
        }
        /// <summary>
        /// Gets a user by his user id PK
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static Member GetUserPart(int uid)
        {
            if (uid == -1) return new Member() { FirstName = "אורח", LastName = "חדש" };
            DataTable dt = Connect.GetData("SELECT * FROM eduMembers WHERE eduUserID=" + uid, "eduMembers");
            if (dt.Rows.Count == 0)
                return null;
            Member m = new Member()
            {
                UserID = int.Parse(dt.Rows[0]["eduUserID"].ToString()),
                ID = dt.Rows[0]["eduID"].ToString(),
                FirstName = dt.Rows[0]["eduFirstName"].ToString(),
                LastName = dt.Rows[0]["eduLastName"].ToString(),
                Mail = dt.Rows[0]["eduMail"].ToString(),
                Auth = ((MemberClearance)(char.Parse(dt.Rows[0]["eduType"].ToString()))),
                Gender = ((MemberGender)(char.Parse(dt.Rows[0]["eduGender"].ToString()))),
                BornDate = DateTime.Parse(dt.Rows[0]["eduBorn"].ToString()),
                PicturePath = dt.Rows[0]["eduPicture"].ToString(),
                GradeID = int.Parse(dt.Rows[0]["eduGradeID"].ToString()),
                Active = dt.Rows[0]["eduActive"].ToString().Trim()
            };
            if (dt.Rows[0]["eduDateRegister"].ToString() != "")
                m.RegisterationDate = DateTime.Parse(dt.Rows[0]["eduDateRegister"].ToString());
            return m;
        }
        /// <summary>
        /// Logs out the current connected user
        /// </summary>
        public static void Logout()
        {
            if (HttpContext.Current != null && HttpContext.Current.Application["Members"] != null)
            {
                var mems = (List<Member>)HttpContext.Current.Application["Members"];
                mems.Remove(GetCurrent());
                HttpContext.Current.Application["Members"] = mems;
            }
            HttpContext context = HttpContext.Current;
            if (context != null && context.Session != null)//Test if the session is empty - if not delete it's content
                context.Session.Clear();//Clearing the session without changing the session id
        }
        /// <summary>
        /// Returns a list of currently connected users
        /// </summary>
        /// <returns></returns>
        public static List<Member> GetConnected()
        {
            if (HttpContext.Current.Application["Members"] != null)
                return (List<Member>)HttpContext.Current.Application["Members"];
            return new List<Member>();
        }
        /// <summary>
        /// Adds a member to invite list(those who are allowed to register)
        /// </summary>
        /// <param name="m">Member to add</param>
        /// <returns></returns>
        public static bool AddAllowed(Member m)
        {
            return AddAllowed(m.FirstName, m.LastName, m.ID, ((char)m.Auth).ToString());
        }
        /// <summary>
        /// Adds a member to invite list(those who are allowed to register)
        /// </summary>
        /// <param name="fname">First Name</param>
        /// <param name="lname">Last Name</param>
        /// <param name="id">ID</param>
        /// <param name="type">Clearence</param>
        /// <returns></returns>
        public static bool AddAllowed(string fname, string lname, string id, string type)
        {
            return Connect.InsertUpdateDelete("INSERT INTO eduMembers (eduFirstName,eduLastName,eduID,eduActive,eduType) VALUES ('" + fname + "','" + lname + "','" + id + "','Wait','" + type + "')");

            //return Connect.InsertUpdateDelete("INSERT INTO eduAllowed (eduFirstName,eduLastName,eduID,eduActive,eduType) VALUES ('"+fname+"','"+lname+"','"+id+"','No','"+type+"')");
        }
        /// <summary>
        /// Adds a member to invite list(those who are allowed to register)
        /// </summary>
        /// <param name="dt">Data Table</param>
        /// <returns></returns>
        public static string AddAllowed(DataTable dt)
        {
            try
            {
                DataTable dtWs = dt;
                DataTable dtAll = Connect.GetData("SELECT * FROM eduMembers ORDER BY eduID", "eduMembers");
                var indId = dtWs.Columns.Cast<DataColumn>().TakeWhile(dc => dt.Rows[0][dc].ToString() != "תעודת זהות").Count();

                string[] state = new string[dt.Columns.Count];
                for (int i = 0; i < state.Length; i++)
                {
                    state[i] = dt.Rows[0][i].ToString();
                }
                var con = new OleDbConnection(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
                con.Open();
                List<Grade> grades = GradesService.GetAll();
                List<City> cities = CitiesService.GetAll();
                int scid = MemberService.GetCurrent().School.Id;
                Dictionary<string,List<int>> parList = new Dictionary<string, List<int>>();
                for (int i = 0; i < dtWs.Rows.Count; i++)
                {
                    if (i == 0) continue;
                    var id = dtWs.Rows[i][indId].ToString();

                    bool exists = dtAll.AsEnumerable().Any(x => x.Field<string>("eduID").Equals(id) && x.Field<int>("eduSchoolID").Equals(scid));//Using lambda to check if exsits
                    if (exists) continue;
                    Member member = new Member();
                    string[] majorStrings = { };
                    int index = 0;
                    foreach (DataColumn dc in dtWs.Columns)
                    {
                        switch (state[index])
                        {
                            case "שם פרטי":
                                if (i == 0) continue;
                                member.FirstName = dtWs.Rows[i][dc].ToString();
                                break;
                            case "שם משפחה":
                                if (i == 0) continue;
                                member.LastName = dtWs.Rows[i][dc].ToString();
                                break;
                            case "תעודת זהות":
                                if (i == 0) continue;
                                member.ID = dtWs.Rows[i][dc].ToString();
                                break;
                            case "סוג":
                                if (i == 0) continue;
                                MemberClearance clearance = MemberClearance.Student;
                                switch (dtWs.Rows[i][dc].ToString())
                                {
                                    case "מנהל":
                                        clearance = MemberClearance.Admin;
                                        break;
                                    case "תלמיד":
                                        clearance = MemberClearance.Student;
                                        break;
                                    case "הורה":
                                        clearance = MemberClearance.Parent;
                                        break;
                                    case "מורה":
                                        clearance = MemberClearance.Teacher;
                                        break;
                                }
                                member.Auth = clearance;
                                break;
                            case "כיתה":
                                if (i == 0) continue;
                                member.Grade = grades.Single(x => x.Name == dtWs.Rows[i][dc].ToString());
                                break;
                            case "תאריך לידה":
                                if (i == 0) continue;
                                member.BornDate = DateTime.ParseExact(dtWs.Rows[i][dc].ToString(), "dd/MM/yyyy",
                                    System.Globalization.CultureInfo.InvariantCulture);
                                break;
                            case "מגדר":
                                if (i == 0) continue;
                                MemberGender gender = MemberGender.Unknown;
                                switch (dtWs.Rows[i][dc].ToString())
                                {
                                    case "זכר":
                                        gender = MemberGender.Male;
                                        break;
                                    case "ז":
                                        gender = MemberGender.Male;
                                        break;
                                    case "נקבה":
                                        gender = MemberGender.Female;
                                        break;
                                    case "נ":
                                        gender = MemberGender.Female;
                                        break;
                                }
                                member.Gender = gender;
                                break;
                            case "מזהי מגמות":
                                if (i == 0) continue;
                                majorStrings = dtWs.Rows[i][dc].ToString().Split(',');
                                break;
                            case "מזהה עיר":
                                if (i == 0) continue;
                                member.City = cities.Single(x => x.Id == int.Parse(dtWs.Rows[i][dc].ToString()));
                                break;
                            case "פלאפון":
                                if (i == 0) continue;
                                member.Phone = dtWs.Rows[i][dc].ToString();
                                break;
                            case "מייל":
                                if (i == 0) continue;
                                if (dt.Rows[i][dc].ToString() == "") continue;
                                member.Mail = dt.Rows[i][dc].ToString().Trim();
                                break;
                            case "אימייל":
                                if (i == 0) continue;
                                if (dt.Rows[i][dc].ToString() == "") continue;
                                member.Mail = dt.Rows[i][dc].ToString().Trim();
                                break;
                            case "ילדים":
                                if (i == 0) continue;
                                if (dt.Rows[i][dc].ToString() == "") continue;
                                string[] strings = dt.Rows[i][dc].ToString().Split(',');
                                var kidList = strings.Select(str => GetUID(str, scid)).ToList();
                                if (parList.ContainsKey(dt.Rows[i][indId].ToString()))
                                {
                                    List<int> adList = parList[dt.Rows[i][indId].ToString()];
                                    adList.AddRange(kidList);
                                }
                                else
                                {
                                    parList.Add(dt.Rows[i][indId].ToString(), kidList);
                                }
                                break;
                        }
                        index++;
                    }
                    if (member.PicturePath == "")
                        member.PicturePath = "/Content/graphics/img/default.png";
                    string sqlQuery = "INSERT INTO eduMembers(eduFirstName,eduLastName,eduID,eduType,eduGradeID,eduPhone,eduBorn,eduGender,eduCityID,eduActive,eduSchoolID) VALUES('" + member.FirstName + "','" + member.LastName + "','" + member.ID + "','" + Converter.GetClearnce(member.Auth) + "'," + member.GradeID + ",'" + member.Phone + "',#" + Converter.GetTimeShortForDataBase(member.BornDate) + "#,'" + Converter.GetGender(member.Gender) + "'," + member.City.Id + ",'Wait'," + MemberService.GetCurrent().School.Id + ")";
                    var command = new OleDbCommand(sqlQuery, con);
                    command.ExecuteNonQuery();
                    command = new OleDbCommand("SELECT @@IDENTITY;", con);
                    int uid = (int)command.ExecuteScalar();
                    SetTempPass(uid);
                    if (member.Auth == MemberClearance.Student)
                    {
                        foreach (string major in majorStrings)
                        {
                            var command1 = new OleDbCommand(
                                "INSERT INTO eduMajorsMembers (eduUserID,eduMajorID) VALUES (" + uid + "," + major +
                                ")", con);
                            DataTable dt1 = Connect.GetData(
                                "SELECT * FROM eduMajorsTgrades WHERE eduMajorID=" + major + " AND eduGradePart='" +
                                TeacherGradeService.GetParTeacherGrade(GradesService.Get(member.Grade.Id).Name).Replace("'", "''") +
                                "'", "eduMajorsTeacherGrades");
                            foreach (DataRow dr in dt1.Rows)
                            {
                                int tgid = int.Parse(dr["eduTgradeID"].ToString());
                                TeacherGradeService.AddStudent(tgid, uid);
                            }
                            command1.ExecuteNonQuery();
                        }
                    }
                }
                foreach (KeyValuePair<string, List<int>> pair in parList)
                {
                    SetChildren(GetUID(pair.Key,scid), pair.Value);
                }
                con.Close();
                return "";
            }
            catch (Exception ex)
            {
                Problem.Log(ex);
                return "";
            }
        }

        /// <summary>
        /// Validates data over db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static bool Validate(string id, string pass)
        {
            id = id.Replace("'", "");
            pass = pass.Replace("'", "");
            var dt = Connect.GetData("SELECT * FROM eduMembers WHERE eduID='" + id + "' AND eduActive='Yes'", "eduMembers");//Excuting a query to check up with the data base
            if (dt.Rows.Count == 0) return false;
            var dbHash = dt.Rows[0]["eduPass"].ToString();//Getting dbHash - The encrypted password that came from the data base for that user
            return Security.ValidatePassword(pass, dbHash);
        }

        /// <summary>
        /// Validates data over db
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static bool Validate(int uid, string pass)
        {
            pass = pass.Replace("'", "");
            var dt = Connect.GetData("SELECT * FROM eduMembers WHERE eduUserID=" + uid, "eduMembers");//Excuting a query to check up with the data base
            if (dt.Rows.Count == 0) return false;
            var dbHash = dt.Rows[0]["eduPass"].ToString();//Getting dbHash - The encrypted password that came from the data base for that user
            return Security.ValidatePassword(pass, dbHash);
        }
        /// <summary>
        /// Gets all the users from the following grade
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static List<Member> GeTeacherGrade(int gid)
        {
            DataTable dt = Connect.GetData("SELECT * FROM eduMembers WHERE eduGradeID=" + gid, "eduMembers");
            List<Member> Members = new List<Member>();
            foreach (DataRow dr in dt.Rows)
            {
                Member c = new Member()
                {
                    UserID = int.Parse(dr["eduUserID"].ToString()),
                    ID = dr["eduID"].ToString(),
                    FirstName = dr["eduFirstName"].ToString(),
                    LastName = dr["eduLastName"].ToString(),
                    Mail = dr["eduMail"].ToString(),
                    Auth = ((MemberClearance)(char.Parse(dr["eduType"].ToString()))),
                    Gender = ((MemberGender)(char.Parse(dr["eduGender"].ToString()))),
                    BornDate = DateTime.Parse(dr["eduBorn"].ToString()),
                    RegisterationDate = DateTime.Parse(dr["eduDateRegister"].ToString()),
                    PicturePath = dr["eduPicture"].ToString(),
                    GradeID = int.Parse(dr["eduGradeID"].ToString()),
                    City = CitiesService.GetCity(int.Parse(dr["eduCityID"].ToString().Trim())),
                    Majors = MajorsService.GetUserMajors(int.Parse(dr["eduUserID"].ToString())),
                    Active = dr["eduActive"].ToString().Trim()
                };
                if (dr["eduDateRegister"].ToString() != "")
                    c.RegisterationDate = DateTime.Parse(dr["eduDateRegister"].ToString());
                Members.Add(c);
            }
            return Members;
        }
        /// <summary>
        /// Gets all the users that in that grade incl. Teachers
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        public static List<Member> GeTeacherGradePart(string grade)
        {
            DataTable dt = Connect.GetData("SELECT eduUserID AS StudentID, eduFirstName AS eduFirstName, eduLastName AS eduLastName FROM eduGrades AS grd , eduMembers WHERE grd.eduGradeName LIKE '%" + grade + "%' AND grd.eduGradeID = eduMembers.eduGradeID AND eduType='s' AND eduActive<>'No'", "eduMembers");
            List<Member> Members = new List<Member>();
            foreach (DataRow dr in dt.Rows)
            {
                Member c = new Member()
                {
                    UserID = int.Parse(dr["StudentID"].ToString()),
                    FirstName = dr["eduFirstName"].ToString(),
                    LastName = dr["eduLastName"].ToString()
                };
                Members.Add(c);
            }
            return Members;
        }

        public static string SetTempPass(int uid)
        {
            string id = uid.ToString();
            string strNew = "";
            foreach (char cid in id)
            {
                int c = int.Parse(cid.ToString());
                char cc = (char)(c + 97);
                strNew += cid + cc.ToString();
            }
            Connect.InsertUpdateDelete("UPDATE eduMembers SET eduPass='" + Security.CreateHash(strNew) + "' WHERE eduUserID=" + uid);
            return strNew;
        }
        /// <summary>
        /// Gets all the allowed to register list
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetAllowed()
        {
            return Connect.GetData("SELECT * FROM eduMembers WHERE eduActive='Wait' AND eduSchoolID=" + GetCurrent().School.Id, "eduMembers");
        }
        /// <summary>
        /// Gets the greeting for the panel
        /// </summary>
        /// <param name="m">Member</param>
        /// <returns>Greeting for the panel</returns>
        public static string GetGreeting(Member m)
        {
            return GetGreeting(m.Auth, m.Gender);
        }
        /// <summary>
        /// Gets the greeting for the panel
        /// </summary>
        /// <param name="auth">MemberClearance</param>
        /// <param name="gen">MemberGender</param>
        /// <returns>Greeting for the panel</returns>
        public static string GetGreeting(MemberClearance auth, MemberGender gen)
        {
            string[] female = { "התלמידה", "המורה", "המנהלת","אמא" };
            string[] male = { "התלמיד", "המורה", "המנהל","אבא" };
            switch (auth)
            {
                case MemberClearance.Student:
                    if (gen == MemberGender.Female) return female[0]; return male[0];
                case MemberClearance.Teacher:
                    if (gen == MemberGender.Female) return female[1]; return male[1];
                case MemberClearance.Admin:
                    if (gen == MemberGender.Female) return female[2]; return male[2];
                case MemberClearance.Parent:
                    if (gen == MemberGender.Female) return female[3]; return male[3];
            }
            return "";
        }
        /// <summary>
        /// Get free hours of a teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static List<int> GetFreeHours(int teacherId, int day)
        {
            var dt = Connect.GetData("SELECT eduHour FROM eduLessons,eduTeacherGrades AS tg WHERE tg.eduTgradeID=eduLessons.eduTgradeID AND tg.eduTeacherID=" + teacherId + " AND eduLessons.eduDay=" + day + "  AND eduActive='" +
                                     "Yes" +
                                     "" +
                                     "" +
                                     "" +
                                     "" +
                                     "'", "eduLessons");
            var hours = new List<int>();
            for (int i = 0; i < LessonService.LessonsInDay; i++)
            {
                hours.Add(i + 1);
            }
            foreach (DataRow dr in dt.Rows)
            {
                int givenValue = int.Parse(dr["eduHour"].ToString());
                if (hours.Contains(givenValue))
                {
                    hours.Remove(givenValue);
                }
            }
            return hours;
        }
        /// <summary>
        /// Get students of a teacher
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public static DataTable GetStudents(int tid)
        {
            var dt = Connect.GetData("SELECT DISTINCT eduUserID, * FROM eduMembers,eduCities,eduLearnGroups,eduTeacherGrades WHERE eduLearnGroups.eduStudentID=eduMembers.eduUserID AND eduCities.eduCityID=eduMembers.eduCityID AND eduMembers.eduActive<>'No' AND eduTeacherGrades.eduTgradeID=eduLearnGroups.eduTgradeID AND eduTeacherGrades.eduTeacherID=" + tid + "AND eduTeacherGrades.eduDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND eduTeacherGrades.eduDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "eduMembers");
            var dtn = dt.Clone();

            foreach (DataRow dr in dt.Rows)
            {
                if (!(dtn.AsEnumerable().Any(row => int.Parse(dr["eduUserID"].ToString()) == row.Field<int>("eduUserID"))))
                {
                    dtn.ImportRow(dr);
                }
            }
            return dtn;
        }
    }
}
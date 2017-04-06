using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;

/// <summary>
/// A service class for member class
/// DB Connector
/// </summary>
public static class MemberService
{
    public static Member GetAllowed(string id)
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsAllowed WHERE nhsID='" + id + "'","nhsAllowed");
        if (dt.Rows.Count == 0) return new Member();
        Member m = new Member()
        {
            FirstName = dt.Rows[0]["nhsFirstName"].ToString().Trim(),
            LastName = dt.Rows[0]["nhsLastName"].ToString().Trim(),
            ID = dt.Rows[0]["nhsID"].ToString().Trim(),
            Auth = ((MemberClearance)dt.Rows[0]["nhsType"].ToString().Trim()[0])
        };
        return m;
    }
    /// <summary>
    /// Removes the user from the invite list
    /// </summary>
    /// <param name="id">Civilian ID</param>
    /// <returns></returns>
    public static bool RemoveFromAllowed(string id)
    {
        return Connect.InsertUpdateDelete("DELETE FROM nhsAllowed WHERE nhsID='" + id + "'");
    }
    /// <summary>
    /// Gets all the members from the DB
    /// </summary>
    /// <returns>A list of member object</returns>
    public static List<Member> GetAll()
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsMembers", "nhsMembers");
        List<Member> Members = new List<Member>();
        foreach (DataRow dr in dt.Rows)
        {
            Member c = new Member()
            {
                UserID = int.Parse(dr["nhsUserID"].ToString()),
                ID = dr["nhsID"].ToString(),
                FirstName = dr["nhsFirstName"].ToString(),
                LastName = dr["nhsLastName"].ToString(),
                Mail = dr["nhsMail"].ToString(),
                Auth = ((MemberClearance)(char.Parse(dr["nhsType"].ToString()))),
                Gender = ((MemberGender)(char.Parse(dr["nhsGender"].ToString()))),
                BornDate = DateTime.Parse(dr["nhsBorn"].ToString()),
                RegisterationDate = DateTime.Parse(dr["nhsDateRegister"].ToString()),
                PicturePath = dr["nhsPicture"].ToString(),
                GradeID = int.Parse(dr["nhsGradeID"].ToString()),
                City = CitiesService.GetCity(int.Parse(dr["nhsCityID"].ToString().Trim())),
                Majors = MajorsService.GetUserMajors(int.Parse(dr["nhsUserID"].ToString())),
                Active= Convert.ToBoolean(dr["nhsActive"].ToString().Trim()),
                Phone = dr["nhsPhone"].ToString().Trim()
            };
            Members.Add(c);
        }
        return Members;
    }
    /// <summary>
    /// Gets partially field member object for liter work loads that require the following:
    /// UserID, FirstName, LastName, Name, Auth, Active
    /// </summary>
    /// <returns></returns>
    public static List<Member> GetNames()
    {
        DataTable dt = Connect.GetData("SELECT nhsUserID,nhsFirstName,nhsLastName,nhsType,nhsActive FROM nhsMembers", "nhsMembers");
        List<Member> Members = new List<Member>();
        foreach (DataRow dr in dt.Rows)
        {
            Member c = new Member()
            {
                UserID = int.Parse(dr["nhsUserID"].ToString()),
                FirstName = dr["nhsFirstName"].ToString(),
                LastName = dr["nhsLastName"].ToString(),
                Auth = ((MemberClearance)(char.Parse(dr["nhsType"].ToString()))),
                Active = Convert.ToBoolean(dr["nhsActive"].ToString().Trim())
            };
            Members.Add(c);
        }
        return Members;
    }
    /// <summary>
    /// Gets all the members from the DB
    /// </summary>
    /// <returns>A DataTable of members</returns>
    public static DataTable GetAllDT()
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsMembers,nhsCities WHERE nhsCities.nhsCityID=nhsMembers.nhsCityID AND nhsMembers.nhsActive=Yes", "nhsMembers");
        return dt;
    }
    /// <summary>
    /// Get the current member from the DB
    /// </summary>
    /// <returns>A DataTable of members</returns>
    public static DataTable GetCurrentDT()
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsMembers,nhsCities WHERE nhsCities.nhsCityID=nhsMembers.nhsCityID AND nhsMembers.nhsActive=Yes AND nhsMembers.nhsUserID=" + MemberService.GetCurrent().UserID, "nhsMembers");
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
        DataTable dt = Connect.GetData("SELECT * FROM nhsMembers WHERE nhsMail='" + email + "' AND nhsActive=Yes", "nhsMembers");//Excuting a query to check up with the data base
        if (dt.Rows.Count == 0) return false;
        string dbHash = dt.Rows[0]["nhsPass"].ToString();//Getting dbHash - The encrypted password that came from the data base for that user
        if (Security.ValidatePassword(pass, dbHash))
        {
            Member m = new Member()
            {
                UserID = int.Parse(dt.Rows[0]["nhsUserID"].ToString()),
                ID = dt.Rows[0]["nhsID"].ToString(),
                FirstName = dt.Rows[0]["nhsFirstName"].ToString(),
                LastName = dt.Rows[0]["nhsLastName"].ToString(),
                Mail = dt.Rows[0]["nhsMail"].ToString(),
                Auth = ((MemberClearance)(char.Parse(dt.Rows[0]["nhsType"].ToString()))),
                Gender = ((MemberGender)(char.Parse(dt.Rows[0]["nhsGender"].ToString()))),
                BornDate = DateTime.Parse(dt.Rows[0]["nhsBorn"].ToString()),
                RegisterationDate = DateTime.Parse(dt.Rows[0]["nhsDateRegister"].ToString()),
                PicturePath = dt.Rows[0]["nhsPicture"].ToString(),
                GradeID = int.Parse(dt.Rows[0]["nhsGradeID"].ToString()),
                City = CitiesService.GetCity(int.Parse(dt.Rows[0]["nhsCityID"].ToString().Trim())),
                Majors = MajorsService.GetUserMajors(int.Parse(dt.Rows[0]["nhsUserID"].ToString())),
                Active = Convert.ToBoolean(dt.Rows[0]["nhsActive"].ToString().Trim()),
                Phone = dt.Rows[0]["nhsPhone"].ToString().Trim()
            };
            HttpContext.Current.Session["Member"] = m;
            if (HttpContext.Current.Application["Members"]==null)
            {
                List<Member> mems = new List<Member>();
                mems.Add(m);
                HttpContext.Current.Application["Members"] = mems;
            }
            else
            {
                List<Member> mems = (List<Member>)HttpContext.Current.Application["Members"];
                mems.Add(m);
                HttpContext.Current.Application["Members"] = mems;
            }
            return true;
        }
        return false;
    }
    /// <summary>
    /// Login of the user - init for session - The session key is 'Member'
    /// </summary>
    /// <param name="email">Email to login</param>
    /// <param name="pass">Password to login</param>
    /// <returns>Whether the login was successful or not</returns>
    public static bool Login(string id, string pass,bool ids)
    {
        id = id.Replace("'", "");
        pass = pass.Replace("'", "");
        DataTable dt = Connect.GetData("SELECT * FROM nhsMembers WHERE nhsID='" + id + "' AND nhsActive=Yes", "nhsMembers");//Excuting a query to check up with the data base
        if (dt.Rows.Count == 0) return false;
        string dbHash = dt.Rows[0]["nhsPass"].ToString();//Getting dbHash - The encrypted password that came from the data base for that user
        if (Security.ValidatePassword(pass, dbHash))
        {
            Member m = new Member()
            {
                UserID = int.Parse(dt.Rows[0]["nhsUserID"].ToString()),
                ID = dt.Rows[0]["nhsID"].ToString(),
                FirstName = dt.Rows[0]["nhsFirstName"].ToString(),
                LastName = dt.Rows[0]["nhsLastName"].ToString(),
                Mail = dt.Rows[0]["nhsMail"].ToString(),
                Auth = ((MemberClearance)(char.Parse(dt.Rows[0]["nhsType"].ToString()))),
                Gender = ((MemberGender)(char.Parse(dt.Rows[0]["nhsGender"].ToString()))),
                BornDate = DateTime.Parse(dt.Rows[0]["nhsBorn"].ToString()),
                RegisterationDate = DateTime.Parse(dt.Rows[0]["nhsDateRegister"].ToString()),
                PicturePath = dt.Rows[0]["nhsPicture"].ToString(),
                GradeID = int.Parse(dt.Rows[0]["nhsGradeID"].ToString()),
                City = CitiesService.GetCity(int.Parse(dt.Rows[0]["nhsCityID"].ToString().Trim())),
                Majors = MajorsService.GetUserMajors(int.Parse(dt.Rows[0]["nhsUserID"].ToString())),
                Active = Convert.ToBoolean(dt.Rows[0]["nhsActive"].ToString().Trim()),
                Phone = dt.Rows[0]["nhsPhone"].ToString().Trim()
            };
            HttpContext.Current.Session["Member"] = m;
            if (HttpContext.Current.Application["Members"] == null)
            {
                List<Member> mems = new List<Member>();
                mems.Add(m);
                HttpContext.Current.Application["Members"] = mems;
            }
            else
            {
                List<Member> mems = (List<Member>)HttpContext.Current.Application["Members"];
                mems.Add(m);
                HttpContext.Current.Application["Members"] = mems;
            }
            return true;
        }
        return false;
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
            Member m = new Member()
            {
                FirstName = "non",
                LastName = "non",
                Auth = MemberClearance.Guest
            };
            return m;
        }
    }
    /// <summary>
    /// Validates Session Keys (Their Values)
    /// </summary>
    /// <param name="sessionNames">The keys</param>
    /// <param name="c">The hyper text transfer protocol context</param>
    /// <returns>Whether the keys are empty or not(if one is empty then it is false)</returns>
    private static bool ValidateSessions(string[] sessionNames, HttpContext c)
    {
        foreach (string sName in sessionNames)
        {
            if (c == null || c.Session == null || c.Session[sName] == null || c.Session[sName].ToString().Trim() == "")
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// Template for inserting a new member into the DB
    /// </summary>
    private const string FullInsertTemplate = "INSERT INTO nhsMembers (nhsFirstName,nhsLastName,nhsPass,nhsType,nhsMail,nhsID,nhsGender,nhsGradeID,nhsPicture,nhsBorn,nhsDateRegister,nhsCityID,nhsActive,nhsPhone) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',{11},Yes,'{12}')";
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
        Connect.InsertUpdateDelete(string.Format(FullInsertTemplate, m.FirstName, m.LastName, Security.CreateHash(pass), status, m.Mail, m.ID, gen, m.GradeID, m.PicturePath, Converter.GetTimeReadyForDataBase(m.BornDate), Converter.GetFullTimeReadyForDataBase(), m.City.ID,m.Phone));
        int userID = GetUID(m.ID);
        foreach (Major maj in m.Majors)
        {
            Connect.InsertUpdateDelete("INSERT INTO nhsMajorsMembers (nhsUserID,nhsMajorID) VALUES (" + userID + "," + maj.ID + ")");
            DataTable dt = Connect.GetData("SELECT * FROM nhsMajorsTgrades WHERE nhsMajorID="+maj.ID+" AND nhsGradePart='"+ tGradeService.GetPartGrade(GradesService.Get(m.GradeID).Name).Replace("'", "''")+"'", "nhsMajorsTgrades");
            foreach (DataRow dr in dt.Rows)
            {
                int tgid = int.Parse(dr["nhsTgradeID"].ToString());
                tGradeService.AddStudent(tgid, userID);
            }
        }
        UpdateAllowed(m.UserID);
    }
    /// <summary>
    /// Update the DB allowed table to active account -- just for tracking
    /// </summary>
    /// <param name="uid">The user id<
    /// /param>
    public static void UpdateAllowed(int uid)
    {
        Connect.InsertUpdateDelete("UPDATE nhsAllowed SET nhsActive=Yes WHERE nhsID='" + uid + "'");
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
        DataTable dt1 = Connect.GetData("SELECT * FROM nhsAllowed WHERE (nhsFirstName='" + fname + "' AND nhsLastName='" + lname + "' AND nhsID='" + iuid + "')", "nhsAllowed");
        DataTable dt2 = Connect.GetData("SELECT * FROM nhsMembers WHERE (nhsFirstName='" + fname + "' AND nhsLastName='" + lname + "' AND nhsID='" + iuid + "')", "nhsMembers");
        return (dt1.Rows.Count == 1)&& (dt2.Rows.Count == 0);
    }
    public static bool ExsitsAllowed(string id)
    {
        DataTable dt1 = Connect.GetData("SELECT * FROM nhsAllowed WHERE nhsID='" + id + "'", "nhsAllowed");
        return (dt1.Rows.Count != 0);
    }
    /// <summary>
    /// Gets the user auth
    /// </summary>
    /// <param name="uid">The user id</param>
    /// <returns></returns>
    public static MemberClearance GetClearance(string uid)
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsAllowed WHERE nhsID='" + uid + "'", "nhsAllowed");
        if (dt.Rows.Count == 0)
            return MemberClearance.Guest;
        return ((MemberClearance)dt.Rows[0]["nhsType"].ToString().ToCharArray()[0]);
    }
    /// <summary>
    /// Gets the user id with the id of the user
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public static int GetUID(string ID)
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsMembers WHERE nhsID='" + ID + "'", "nhsMembers");
        if (dt.Rows.Count == 0)
            return -1;
        return int.Parse(dt.Rows[0]["nhsUserID"].ToString());
    }
    /// <summary>
    /// Checks if the email already exsits in the database
    /// </summary>
    /// <param name="email">Email</param>
    /// <returns>Whether the member exsit with the same email</returns>
    public static bool Exsits(string email)
    {
        int count = (int)Connect.GetObject("SELECT COUNT(*) FROM nhsMembers WHERE nhsMail='" + email + "'");
        return (count == 0);
    }
    /// <summary>
    /// Updates the user in the database - USER ID IS MUST
    /// </summary>
    /// <param name="m">The user</param>
    /// <returns>State</returns>
    public static bool Update(Member m)
    {
        int userID = GetUID(m.ID);
        Connect.InsertUpdateDelete("DELETE FROM nhsMajorsMembers WHERE nhsUserID=" + userID);
        foreach (Major maj in m.Majors)
        {
            Connect.InsertUpdateDelete("INSERT INTO nhsMajorsMembers (nhsUserID,nhsMajorID) VALUES (" + userID + "," + maj.ID + ")");
        }
        if (m.PicturePath != null)
            Connect.InsertUpdateDelete("UPDATE nhsMembers SET nhsFirstName='" + m.FirstName + "',nhsLastName='" + m.LastName + "',nhsType='" + ((char)m.Auth) + "',nhsMail='" + m.Mail + "',nhsID='" + m.ID + "',nhsGender='" + ((char)m.Gender) + "',nhsGradeID=" + m.GradeID + ",nhsPicture='" + m.PicturePath + "',nhsBorn='" + Converter.GetTimeReadyForDataBase(m.BornDate) + "',nhsCityID='" + m.City.ID + "' WHERE nhsUserID=" + m.UserID);
        else
            Connect.InsertUpdateDelete("UPDATE nhsMembers SET nhsFirstName='" + m.FirstName + "',nhsLastName='" + m.LastName + "',nhsType='" + ((char)m.Auth) + "',nhsMail='" + m.Mail + "',nhsID='" + m.ID + "',nhsGender='" + ((char)m.Gender) + "',nhsGradeID=" + m.GradeID + ",nhsBorn='" + Converter.GetTimeReadyForDataBase(m.BornDate) + "',nhsCityID='" + m.City.ID + "' WHERE nhsUserID=" + m.UserID);
        if (m.UserID == GetCurrent().UserID)
            UpdateCurrent(GetAll().Where(x => x.UserID == m.UserID).First());
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
        return Connect.InsertUpdateDelete("UPDATE nhsMembers SET nhsPass='" + Security.CreateHash(pass) + "' WHERE nhsUserID=" + uid);
    }
    /// <summary>
    /// Removes the user by the user id PK
    /// </summary>
    /// <param name="uid">User ID</param>
    /// <returns>Action State</returns>
    public static bool RemoveFromActive(string uid)
    {
        if (int.Parse(uid) == GetCurrent().UserID) HttpContext.Current.Session["Member"] = null;
        return Connect.InsertUpdateDelete("UPDATE nhsMembers SET nhsActive=No WHERE nhsUserID=" + uid);
    }
    /// <summary>
    /// Gets a user by his user id PK
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public static Member GetUser(int uid)
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsMembers WHERE nhsUserID="+uid,"nhsMembers");
        if (dt.Rows.Count == 0)
            return null;
        Member m = new Member()
        {
            UserID = int.Parse(dt.Rows[0]["nhsUserID"].ToString()),
            ID = dt.Rows[0]["nhsID"].ToString(),
            FirstName = dt.Rows[0]["nhsFirstName"].ToString(),
            LastName = dt.Rows[0]["nhsLastName"].ToString(),
            Mail = dt.Rows[0]["nhsMail"].ToString(),
            Auth = ((MemberClearance)(char.Parse(dt.Rows[0]["nhsType"].ToString()))),
            Gender = ((MemberGender)(char.Parse(dt.Rows[0]["nhsGender"].ToString()))),
            BornDate = DateTime.Parse(dt.Rows[0]["nhsBorn"].ToString()),
            RegisterationDate = DateTime.Parse(dt.Rows[0]["nhsDateRegister"].ToString()),
            PicturePath = dt.Rows[0]["nhsPicture"].ToString(),
            GradeID = int.Parse(dt.Rows[0]["nhsGradeID"].ToString()),
            City = CitiesService.GetCity(int.Parse(dt.Rows[0]["nhsCityID"].ToString().Trim())),
            Majors = MajorsService.GetUserMajors(int.Parse(dt.Rows[0]["nhsUserID"].ToString())),
            Active = Convert.ToBoolean(dt.Rows[0]["nhsActive"].ToString().Trim()),
            Phone = dt.Rows[0]["nhsPhone"].ToString().Trim()
        };
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
        DataTable dt = Connect.GetData("SELECT * FROM nhsMembers WHERE nhsUserID=" + uid, "nhsMembers");
        if (dt.Rows.Count == 0)
            return null;
        Member m = new Member()
        {
            UserID = int.Parse(dt.Rows[0]["nhsUserID"].ToString()),
            ID = dt.Rows[0]["nhsID"].ToString(),
            FirstName = dt.Rows[0]["nhsFirstName"].ToString(),
            LastName = dt.Rows[0]["nhsLastName"].ToString(),
            Mail = dt.Rows[0]["nhsMail"].ToString(),
            Auth = ((MemberClearance)(char.Parse(dt.Rows[0]["nhsType"].ToString()))),
            Gender = ((MemberGender)(char.Parse(dt.Rows[0]["nhsGender"].ToString()))),
            BornDate = DateTime.Parse(dt.Rows[0]["nhsBorn"].ToString()),
            RegisterationDate = DateTime.Parse(dt.Rows[0]["nhsDateRegister"].ToString()),
            PicturePath = dt.Rows[0]["nhsPicture"].ToString(),
            GradeID = int.Parse(dt.Rows[0]["nhsGradeID"].ToString()),
            Active = Convert.ToBoolean(dt.Rows[0]["nhsActive"].ToString().Trim()),
            Phone = dt.Rows[0]["nhsPhone"].ToString().Trim()
        };
        return m;
    }
    /// <summary>
    /// Logs out the current connected user
    /// </summary>
    public static void Logout()
    {
        if (HttpContext.Current != null&& HttpContext.Current.Application != null && HttpContext.Current.Application["Members"] != null)
        {
            List<Member> mems = (List<Member>)HttpContext.Current.Application["Members"];
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
        return AddAllowed(m.FirstName, m.LastName, m.ID,((char)m.Auth).ToString());
    }
    /// <summary>
    /// Adds a member to invite list(those who are allowed to register)
    /// </summary>
    /// <param name="fname">First Name</param>
    /// <param name="lname">Last Name</param>
    /// <param name="id">ID</param>
    /// <param name="type">Clearence</param>
    /// <returns></returns>
    public static bool AddAllowed(string fname,string lname,string id,string type)
    {
        return Connect.InsertUpdateDelete("INSERT INTO nhsAllowed (nhsFirstName,nhsLastName,nhsID,nhsActive,nhsType) VALUES ('"+fname+"','"+lname+"','"+id+"',NO,'"+type+"')");
    }
    public static int AddAllowed(DataTable dt,int indID,int indFname,int indLname)
    {
        try
        {
            DataTable dtWS = dt;
            DataTable dtAll = Connect.GetData("SELECT * FROM nhsAllowed ORDER BY nhsID", "nhsAllowed");
            OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
            con.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = "INSERT INTO nhsAllowed(nhsFirstName,nhsLastName,nhsID,nhsActive,nhsType) VALUES(@FNAME,@LNAME,@ID,NO,'s')";
            cmd.Parameters.Add("@FNAME", OleDbType.VarWChar, 255);
            cmd.Parameters.Add("@LNAME", OleDbType.VarWChar, 255);
            cmd.Parameters.Add("@ID", OleDbType.VarWChar, 255);
            int count = dt.Rows.Count;
            OleDbTransaction transaction = con.BeginTransaction();
            cmd.Transaction = transaction;
            for (int i = 0; i < dtWS.Rows.Count; i++)
            {
                string id = dtWS.Rows[i][indID].ToString();
                bool exists = dtAll.AsEnumerable().Where(x => x.Field<string>("nhsID").Equals(id)).Count() > 0;//Using lambda to check if exsits
                if (!exists)
                {
                    count--;
                    cmd.Parameters[0].Value = dtWS.Rows[i][indFname].ToString();
                    cmd.Parameters[1].Value = dtWS.Rows[i][indLname].ToString();
                    cmd.Parameters[2].Value = dtWS.Rows[i][indID].ToString();
                    cmd.ExecuteNonQuery();
                }
            }
            transaction.Commit();
            con.Close();
            return count;
        }
        catch (Exception ex)
        {
            Problem.Log(ex);
            return 0;
        }
    }
    /// <summary>
    /// Gets all the users from the following grade
    /// </summary>
    /// <param name="gid"></param>
    /// <returns></returns>
    public static List<Member> GetGrade(int gid)
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsMembers WHERE nhsGradeID="+gid, "nhsMembers");
        List<Member> Members = new List<Member>();
        foreach (DataRow dr in dt.Rows)
        {
            Member c = new Member()
            {
                UserID = int.Parse(dr["nhsUserID"].ToString()),
                ID = dr["nhsID"].ToString(),
                FirstName = dr["nhsFirstName"].ToString(),
                LastName = dr["nhsLastName"].ToString(),
                Mail = dr["nhsMail"].ToString(),
                Auth = ((MemberClearance)(char.Parse(dr["nhsType"].ToString()))),
                Gender = ((MemberGender)(char.Parse(dr["nhsGender"].ToString()))),
                BornDate = DateTime.Parse(dr["nhsBorn"].ToString()),
                RegisterationDate = DateTime.Parse(dr["nhsDateRegister"].ToString()),
                PicturePath = dr["nhsPicture"].ToString(),
                GradeID = int.Parse(dr["nhsGradeID"].ToString()),
                City = CitiesService.GetCity(int.Parse(dr["nhsCityID"].ToString().Trim())),
                Majors = MajorsService.GetUserMajors(int.Parse(dr["nhsUserID"].ToString())),
                Active = Convert.ToBoolean(dr["nhsActive"].ToString().Trim()),
                Phone = dr["nhsPhone"].ToString().Trim()
            };
            Members.Add(c);
        }
        return Members;
    }
    /// <summary>
    /// Gets all the users that in that grade incl. Teachers
    /// </summary>
    /// <param name="grade"></param>
    /// <returns></returns>
    public static List<Member> GetGradePart(string grade)
    {
        DataTable dt = Connect.GetData("SELECT nhsUserID AS StudentID, nhsFirstName AS nhsFirstName, nhsLastName AS nhsLastName FROM nhsGrades AS grd , nhsMembers WHERE grd.nhsGradeName LIKE '%" + grade+"%' AND grd.nhsGradeID = nhsMembers.nhsGradeID AND nhsType='s'", "nhsMembers");
        List<Member> Members = new List<Member>();
        foreach (DataRow dr in dt.Rows)
        {
            Member c = new Member()
            {
                UserID = int.Parse(dr["StudentID"].ToString()),
                FirstName = dr["nhsFirstName"].ToString(),
                LastName = dr["nhsLastName"].ToString()
            };
            Members.Add(c);
        }
        return Members;
    }
    /// <summary>
    /// Gets all the allowed to register list
    /// </summary>
    /// <returns>DataTable</returns>
    public static DataTable GetAllowed()
    {
        return Connect.GetData("SELECT * FROM nhsAllowed ORDER BY nhsActive", "nhsAllowed");
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
    public static string GetGreeting(MemberClearance auth,MemberGender gen)
    {
        string[] female = { "התלמידה", "המורה", "המנהלת" };
        string[] male = { "התלמיד", "המורה", "המנהל" };
        switch (auth)
        {
            case MemberClearance.Student:
                if (gen == MemberGender.Female) return female[0]; return male[0];
            case MemberClearance.Teacher:
                if (gen == MemberGender.Female) return female[1]; return male[1];
            case MemberClearance.Admin:
                if (gen == MemberGender.Female) return female[2]; return male[2];
        }
        return "";
    }
    public static List<int> GetFreeHours(int teacherId,int day)
    {
        DataTable dt = Connect.GetData("SELECT nhsHour FROM nhsLessons,nhsTeacherGrades AS tg WHERE tg.nhsTgradeID=nhsLessons.nhsTgradeID AND tg.nhsTeacherID=" + teacherId + " AND nhsLessons.nhsDay=" + day + "  AND nhsActive=YES", "nhsLessons");
        List<int> hours = new List<int>();
        for (int i = 0; i < LessonService.LessonsInDay; i++)
        {
            hours.Add(i + 1);
        }
        foreach (DataRow dr in dt.Rows)
        {
            int givenValue = int.Parse(dr["nhsHour"].ToString());
            if (hours.Contains(givenValue))
            {
                hours.Remove(givenValue);
            }
        }
        return hours;
    }
    public static DataTable GetStudents(int tid)
    {
        DataTable dt = Connect.GetData("SELECT DISTINCT nhsUserID, * FROM nhsMembers,nhsCities,nhsLearnGroups,nhsTeacherGrades WHERE nhsLearnGroups.nhsStudentID=nhsMembers.nhsUserID AND nhsCities.nhsCityID=nhsMembers.nhsCityID AND nhsMembers.nhsActive=Yes AND nhsTeacherGrades.nhsTgradeID=nhsLearnGroups.nhsTgradeID AND nhsTeacherGrades.nhsTeacherID=" + tid + "AND nhsTeacherGrades.nhsDate >= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetStart()) + "# AND nhsTeacherGrades.nhsDate <= #" + Converter.GetTimeShortForDataBase(EduSysDate.GetEnd()) + "#", "nhsMembers");
        DataTable dtn = dt.Clone();

        foreach (DataRow dr in dt.Rows)
        {
            if (!(dtn.AsEnumerable().Any(row => int.Parse(dr["nhsUserID"].ToString()) == row.Field<int>("nhsUserID"))))
            {
                dtn.ImportRow(dr);
            }
        }
        return dtn;
    }
}
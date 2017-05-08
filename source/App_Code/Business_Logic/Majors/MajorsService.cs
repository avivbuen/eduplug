using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Business_Logic.Members;

namespace Business_Logic.Majors
{
    /// <summary>
    /// Major Service
    /// </summary>
    public static class MajorsService
    {
        /// <summary>
        /// Get all majors
        /// </summary>
        /// <returns></returns>
        public static List<Major> GetAll()
        {
            var dt = Connect.GetData("SELECT * FROM eduMajors", "eduMajors");
            return (from DataRow dataRow in dt.Rows
                select new Major()
                {
                    Id = int.Parse(dataRow["eduMajorID"].ToString().Trim()),
                    Title = dataRow["eduMajor"].ToString().Trim()
                }).ToList();
        }
        /// <summary>
        /// Get all majors - DataTable
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetAllDataTable()
        {
            DataTable dt = Connect.GetData("SELECT * FROM eduMajors", "eduMajors");
            return dt;
        }
        /// <summary>
        /// Get all majors - DataSet
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetAllDataSet()
        {
            DataTable dt = Connect.GetData("SELECT * FROM eduMajors", "eduMajors");
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }
        /// <summary>
        /// Get major by id
        /// </summary>
        /// <param name="majorId">Major id</param>
        /// <returns>Major</returns>
        public static Major Get(int majorId)
        {
            DataTable dt = Connect.GetData("SELECT * FROM eduMajors WHERE eduMajorID="+majorId, "eduMajors");
            return new Major() { Id = int.Parse(dt.Rows[0]["eduMajorID"].ToString().Trim()), Title = dt.Rows[0]["eduMajor"].ToString().Trim() };
        }
        /// <summary>
        /// Get connections of majors and members
        /// </summary>
        /// <returns></returns>
        public static List<MajorMember> GetConnection()
        {
            List<MajorMember> majors = new List<MajorMember>();
            DataTable dt = Connect.GetData("SELECT * FROM eduMajorsMembers", "eduMajorsMembers");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                MajorMember c = new MajorMember()
                {
                    UserId = int.Parse(dt.Rows[i]["eduUserID"].ToString().Trim()),
                    MajorId = int.Parse(dt.Rows[i]["eduMajorID"].ToString().Trim())
                };
                majors.Add(c);
            }
            return majors;
        }
        private static List<Major> _currentAll = GetAll();//All
        private static List<MajorMember> _currentConnections = GetConnection();//Connections
        /// <summary>
        /// Get majors of user
        /// </summary>
        /// <param name="uid">User ID</param>
        /// <returns></returns>
        public static List<Major> GetUserMajors(int uid)
        {
            List<Major> mjrs=new List<Major>();
            foreach (MajorMember connection in _currentConnections)
            {
                if (connection.UserId==uid)
                { 
                    mjrs.Add(_currentAll.Single(x => x.Id == connection.MajorId));
                }
            }
            Update();
            return mjrs;
        }
        /// <summary>
        /// Update majors and connections with multithreading
        /// </summary>
        /// <returns></returns>
        private static void Update()
        {
            Task.Factory.StartNew(UpdateDb);
        }
        /// <summary>
        /// Update majors and connections with multithreading
        /// </summary>
        /// <returns></returns>
        private static void UpdateDb()
        {
            _currentAll = GetAll();
            _currentConnections = GetConnection();
        }
        /// <summary>
        /// connect major to TeacherGrade for certain grade part
        /// </summary>
        /// <returns></returns>
        public static bool SetMajorTeacherGrade(int tgid,int mjrid,string gPart)
        {
            Connect.InsertUpdateDelete("DELETE FROM eduMajorsTgrades WHERE eduTgradeID=" + tgid);
            Connect.InsertUpdateDelete("INSERT INTO eduMajorsTgrades (eduMajorID,eduTgradeID,eduGradePart,eduSchoolID) VALUES (" + mjrid + "," + tgid + ",'" + gPart.Replace("'", "''") + "',"+MemberService.GetCurrent().School.Id+")");
            return true;
        }
        /// <summary>
        /// Add new major
        /// </summary>
        /// <param name="m">Major</param>
        /// <returns></returns>
        public static bool Add(Major m)
        {
            return Connect.InsertUpdateDelete("INSERT INTO eduMajors (eduMajor) VALUES ('" + m.Title.Replace("'", "''") + "')");
        }
        /// <summary>
        /// Get major id by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetMajorID(string name)
        {
            DataTable dt = Connect.GetData("SELECT * FROM eduMajors WHERE eduMajor='"+name+"'","eduMajors");
            if (dt.Rows.Count==0)
            {
                return -1;
            }
            return int.Parse(dt.Rows[0]["eduMajorID"].ToString());
        }
    }
}
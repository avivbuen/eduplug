using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Threading.Tasks;
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
        List<Major> majors = new List<Major>();
        DataTable dt = Connect.GetData("SELECT * FROM nhsMajors", "nhsMajors");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Major c = new Major()
            {
                ID = int.Parse(dt.Rows[i]["nhsMajorID"].ToString().Trim()),
                Title = dt.Rows[i]["nhsMajor"].ToString().Trim()
            };
            majors.Add(c);
        }
        return majors;
    }
    /// <summary>
    /// Get all majors - DataTable
    /// </summary>
    /// <returns>DataTable</returns>
    public static DataTable GetAllDT()
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsMajors", "nhsMajors");
        return dt;
    }
    /// <summary>
    /// Get all majors - DataSet
    /// </summary>
    /// <returns>DataSet</returns>
    public static DataSet GetAllDS()
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsMajors", "nhsMajors");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        return ds;
    }
    /// <summary>
    /// Get major by id
    /// </summary>
    /// <param name="majorID">Major id</param>
    /// <returns>Major</returns>
    public static Major Get(int majorID)
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsMajors WHERE nhsMajorID="+majorID, "nhsMajors");
        return new Major() { ID = int.Parse(dt.Rows[0]["nhsMajorID"].ToString().Trim()), Title = dt.Rows[0]["nhsMajor"].ToString().Trim() };
    }
    /// <summary>
    /// Get connections of majors and members
    /// </summary>
    /// <returns></returns>
    public static List<MajorMember> GetConnection()
    {
        List<MajorMember> majors = new List<MajorMember>();
        DataTable dt = Connect.GetData("SELECT * FROM nhsMajorsMembers", "nhsMajorsMembers");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            MajorMember c = new MajorMember()
            {
                UserID = int.Parse(dt.Rows[i]["nhsUserID"].ToString().Trim()),
                MajorID = int.Parse(dt.Rows[i]["nhsMajorID"].ToString().Trim())
            };
            majors.Add(c);
        }
        return majors;
    }
    private static List<Major> currentAll = GetAll();//All
    private static List<MajorMember> currentConnections = GetConnection();//Connections
    /// <summary>
    /// Get majors of user
    /// </summary>
    /// <param name="uid">User ID</param>
    /// <returns></returns>
    public static List<Major> GetUserMajors(int uid)
    {
        List<Major> mjrs=new List<Major>();
        foreach (MajorMember connection in currentConnections)
        {
            if (connection.UserID==uid)
            { 
                mjrs.Add(currentAll.Single(x => x.ID == connection.MajorID));
            }
        }
        Update();
        return mjrs;
    }
    /// <summary>
    /// Update majors and connections with multithreading
    /// </summary>
    /// <returns></returns>
    private static Task Update()
    {
        return Task.Factory.StartNew(() => updateDB());
    }
    /// <summary>
    /// Update majors and connections with multithreading
    /// </summary>
    /// <returns></returns>
    private static void updateDB()
    {
        currentAll = GetAll();
        currentConnections = GetConnection();
    }
    /// <summary>
    /// connect major to tgrade for certain grade part
    /// </summary>
    /// <returns></returns>
    public static bool SetMajorTgrade(int tgid,int mjrid,string gPart)
    {
        Connect.InsertUpdateDelete("DELETE FROM nhsMajorsTgrades WHERE nhsTgradeID=" + tgid);
        Connect.InsertUpdateDelete("INSERT INTO nhsMajorsTgrades (nhsMajorID,nhsTgradeID,nhsGradePart) VALUES (" + mjrid + "," + tgid + ",'" + gPart.Replace("'", "''") + "')");
        return true;
    }
    /// <summary>
    /// Add new major
    /// </summary>
    /// <param name="m">Major</param>
    /// <returns></returns>
    public static bool Add(Major m)
    {
        return Connect.InsertUpdateDelete("INSERT INTO nhsMajors (nhsMajor) VALUES ('" + m.Title.Replace("'", "''") + "')");
    }
    /// <summary>
    /// Get major id by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static int GetMajorID(string name)
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsMajors WHERE nhsMajor='"+name+"'","nhsMajors");
        if (dt.Rows.Count==0)
        {
            return -1;
        }
        return int.Parse(dt.Rows[0]["nhsMajorID"].ToString());
    }
}
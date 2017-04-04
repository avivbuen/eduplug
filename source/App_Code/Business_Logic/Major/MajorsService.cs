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
    public static DataTable GetAllDT()
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsMajors", "nhsMajors");
        return dt;
    }
    public static DataSet GetAllDS()
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsMajors", "nhsMajors");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        return ds;
    }
    public static Major GetMajor(int majorID)
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsMajors WHERE nhsMajorID="+majorID, "nhsMajors");
        return new Major() { ID = int.Parse(dt.Rows[0]["nhsMajorID"].ToString().Trim()), Title = dt.Rows[0]["nhsMajor"].ToString().Trim() };
    }
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
    private static List<Major> currentAll = GetAll();
    private static List<MajorMember> currentConnections = GetConnection();
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
    private static Task Update()
    {
        return Task.Factory.StartNew(() => updateDB());
    }
    private static void updateDB()
    {
        currentAll = GetAll();
        currentConnections = GetConnection();
    }
    public static bool SetMajorTgrade(int tgid,int mjrid,string gPart)
    {
        Connect.InsertUpdateDelete("DELETE FROM nhsMajorsTgrades WHERE nhsTgradeID=" + tgid);
        Connect.InsertUpdateDelete("INSERT INTO nhsMajorsTgrades (nhsMajorID,nhsTgradeID,nhsGradePart) VALUES (" + mjrid + "," + tgid + ",'" + gPart.Replace("'", "''") + "')");
        return true;
    }
    public static bool Add(Major m)
    {
        return Connect.InsertUpdateDelete("INSERT INTO nhsMajors (nhsMajor) VALUES ('" + m.Title.Replace("'", "''") + "')");
    }
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
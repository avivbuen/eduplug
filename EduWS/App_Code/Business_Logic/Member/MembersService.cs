using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// A service class for member class
/// DB Connector
/// </summary>
public static class MemberService
{
    /// <summary>
    /// Gets all the members from the DB
    /// </summary>
    /// <returns>A list of member object</returns>
    public static List<Member> GetAll()
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsStudents", "nhsStudents");
        List<Member> Members = new List<Member>();
        foreach (DataRow dr in dt.Rows)
        {
            Member c = new Member()
            {
                UserID = int.Parse(dr["nhsStudentID"].ToString()),
                ID = dr["nhsID"].ToString(),
                FirstName = dr["nhsFirstName"].ToString(),
                LastName = dr["nhsLastName"].ToString()
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
        DataTable dt = Connect.GetData("SELECT nhsStudentID,nhsFirstName,nhsLastName,nhsType,nhsActive FROM nhsStudents", "nhsStudents");
        List<Member> Members = new List<Member>();
        foreach (DataRow dr in dt.Rows)
        {
            Member c = new Member()
            {
                UserID = int.Parse(dr["nhsStudentID"].ToString()),
                ID = dr["nhsID"].ToString(),
                FirstName = dr["nhsFirstName"].ToString(),
                LastName = dr["nhsLastName"].ToString()
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
        DataTable dt = Connect.GetData("SELECT * FROM nhsStudents,nhsCities WHERE nhsCities.nhsCityID=nhsStudents.nhsCityID AND nhsStudents.nhsActive=Yes", "nhsStudents");
        return dt;
    }
    /// <summary>
    /// Template for inserting a new member into the DB
    /// </summary>
    private const string FullInsertTemplate = "INSERT INTO nhsStudents (nhsFirstName,nhsLastName,nhsID) VALUES('{0}','{1}','{2}')";
    public static string Add(Member mem)
    {
        if (mem.FirstName.Trim() == "" || mem.LastName.Trim() == "" || mem.ID == "")
            return "OP_FAILED_EMPTY_MEM_OBJECT";
        if (GetUID(mem.ID) == -1)
            Connect.InsertUpdateDelete(string.Format(FullInsertTemplate, mem.FirstName, mem.LastName, mem.ID));
        return "Student Added " + mem.ID;
    }
    /// <summary>
    /// Gets the user id with the id of the user
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public static int GetUID(string ID)
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsStudents WHERE nhsID='" + ID + "'", "nhsStudents");
        if (dt.Rows.Count == 0)
            return -1;
        return int.Parse(dt.Rows[0]["nhsStudentID"].ToString());
    }

}
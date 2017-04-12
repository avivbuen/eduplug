using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// AdjustmentService
/// </summary>
public static class AdjustmentService
{
    /// <summary>
    /// Get all adjustments
    /// </summary>
    /// <returns></returns>
    public static List<Adjustment> GetAll()
    {
        List<Adjustment> adjustments = new List<Adjustment>();
        DataTable dt = Connect.GetData("SELECT * FROM nhsAdjustments", "nhsAdjustments");
        foreach (DataRow dr in dt.Rows)
        {
            Adjustment adj = new Adjustment()
            {
                ID = int.Parse(dr["nhsStudentAdjusmentID"].ToString().Trim()),
                Name = dr["nhsStudentAdjustment"].ToString().Trim()
            };
            adjustments.Add(adj);
        }
        return adjustments.OrderBy(x => x.ID).ToList();
    }
    /// <summary>
    /// Get students adjustments
    /// </summary>
    /// <param name="sid"></param>
    /// <returns></returns>
    public static List<Adjustment> GetStudent(string sid)
    {
        List<Adjustment> adjustments = new List<Adjustment>();
        DataTable dt = Connect.GetData("SELECT nhsStudentAdjusmentID,nhsStudentAdjustment FROM nhsAdjustments,nhsStudents AS m, nhsAdjustmentsStudents AS adjMem WHERE adjMem.nhsAdjustmentID=nhsAdjustments.nhsStudentAdjusmentID AND m.nhsStudentID=adjMem.nhsStudentID AND m.nhsStudentID="+MemberService.GetUID(sid), "nhsAdjustments");
        foreach (DataRow dr in dt.Rows)
        {
            Adjustment adj = new Adjustment()
            {
                ID = int.Parse(dr["nhsStudentAdjusmentID"].ToString().Trim()),
                Name = dr["nhsStudentAdjustment"].ToString().Trim()
            };
            adjustments.Add(adj);
        }
        return adjustments.OrderBy(x => x.ID).ToList();
    }
    /// <summary>
    /// Add adjustment to student
    /// </summary>
    /// <param name="sid"></param>
    /// <param name="adid"></param>
    /// <returns></returns>
    public static string Add(string sid, int adid)
    {
        if (sid.Trim() == "" || sid.Trim().Length < 8)
            return "OP_FAILED_WRONG_ID";
        int id = MemberService.GetUID(sid);
        if (id == -1)
            return "OP_FAILED_WRONG_ID";
        Connect.InsertUpdateDelete("INSERT INTO nhsAdjustmentsStudents (nhsStudentID,nhsAdjustmentID) VALUES(" + id + "," + adid + ")");
        return "OP_ADJUSTMENT_ADD_PASSED";
    }
}
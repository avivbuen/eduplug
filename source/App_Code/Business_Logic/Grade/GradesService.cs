using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// GradesService
/// </summary>
public static class GradesService
{
    /// <summary>
    /// Gets all the grades
    /// </summary>
    /// <returns></returns>
    public static List<Grade> GetAll()
    {
        List<Grade> grades = new List<Grade>();
        DataTable dt = Connect.GetData("SELECT * FROM nhsGrades", "nhsGrades");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Grade c = new Grade()
            {
                ID = int.Parse(dt.Rows[i]["nhsGradeID"].ToString().Trim()),
                Name = dt.Rows[i]["nhsGradeName"].ToString().Trim()
            };
            grades.Add(c);
        }
        return grades;
    }
    /// <summary>
    /// Gets all the grades - DataTable
    /// </summary>
    /// <returns>DataTable</returns>
    public static DataTable GetAllDT()
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsGrades", "nhsGrades");
        return dt;
    }
    /// <summary>
    /// Gets all the grades - DataSet
    /// </summary>
    /// <returns>DataSet</returns>
    public static DataSet GetAllDS()
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsGrades", "nhsGrades");
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        return ds;
    }
    /// <summary>
    /// Get grade by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Grade Get(int id)
    {
        DataTable dt = Connect.GetData("SELECT * FROM nhsGrades WHERE nhsGradeID="+id, "nhsGrades");
        if (dt.Rows.Count == 0)
        {
            return null;
        }
        Grade c = new Grade()
        {
            ID = int.Parse(dt.Rows[0]["nhsGradeID"].ToString().Trim()),
            Name = dt.Rows[0]["nhsGradeName"].ToString().Trim()
        };
        return c;
    }
}
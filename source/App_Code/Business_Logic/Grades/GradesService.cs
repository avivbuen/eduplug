using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Business_Logic.Grades
{
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
            var dt = Connect.GetData("SELECT * FROM eduGrades", "eduGrades");
            return (from DataRow dataRow in dt.Rows
                select new Grade()
                {
                    Id = int.Parse(dataRow["eduGradeID"].ToString().Trim()),
                    Name = dataRow["eduGradeName"].ToString().Trim()
                }).ToList();
        }
        /// <summary>
        /// Gets all the grades - DataTable
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetAllDataTable()
        {
            var dt = Connect.GetData("SELECT * FROM eduGrades", "eduGrades");
            return dt;
        }
        /// <summary>
        /// Gets all the grades - DataSet
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetAllDataSet()
        {
            var dt = Connect.GetData("SELECT * FROM eduGrades", "eduGrades");
            var ds = new DataSet();
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
            var dt = Connect.GetData("SELECT * FROM eduGrades WHERE eduGradeID="+id, "eduGrades");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            var c = new Grade()
            {
                Id = int.Parse(dt.Rows[0]["eduGradeID"].ToString().Trim()),
                Name = dt.Rows[0]["eduGradeName"].ToString().Trim()
            };
            return c;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business_Logic.Cities;
using Business_Logic.Members;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;


namespace Business_Logic.Schools
{
    /// <summary>
    /// Summary description for SchoolService
    /// </summary>
    public static class SchoolService
    {
        /// <summary>
        /// Gets all schools
        /// </summary>
        /// <returns></returns>
        public static List<School> GetAll()
        {
            var dt = Connect.GetData("SELECT * FROM eduSchools", "eduSchools");
            var schools = (from DataRow dataRow in dt.Rows
                           select new School()
                           {
                               Id = int.Parse(dataRow["eduSchoolID"].ToString().Trim()),
                               Name = dataRow["eduSchoolName"].ToString().Trim(),
                               LogoPath = dataRow["eduSchoolLogo"].ToString().Trim(),
                               Official= Convert.ToBoolean(dataRow["eduOfficial"])

                           }).ToList();
            return schools.OrderBy(x => x.Name).ToList();
        }
        /// <summary>
        /// Update school 
        /// </summary>
        /// <param name="school"></param>
        /// <returns></returns>
        public static bool Update(School school)
        {
            if(school.LogoPath!=null)
            return Connect.InsertUpdateDelete("UPDATE eduSchools SET eduSchoolName='" + school.Name +
                                              "', eduSchoolLogo='" + school.LogoPath + "' WHERE eduSchoolID=" +
                                              school.Id);
            return Connect.InsertUpdateDelete("UPDATE eduSchools SET eduSchoolName='" + school.Name +
                                              "' WHERE eduSchoolID=" +
                                              school.Id);
        }
        /// <summary>
        /// Gets all known schools to israel (last updated 2-5-2017)
        /// </summary>
        /// <returns></returns>
        public static List<School> GetAllKnown()
        {
            var dt = Connect.GetData("SELECT * FROM eduKnownSchools", "eduKnownSchools");
            var cities = CitiesService.GetAll();
            var schools = (from DataRow dataRow in dt.Rows
                           select new School()
                           {
                               Id = int.Parse(dataRow["eduSchoolID"].ToString().Trim()),
                               Name = dataRow["eduSchoolName"].ToString().Trim(),
                               Manager = new Member() { Name = dataRow["eduSchoolManager"].ToString().Trim() },
                               MidManger = new Member() { Name = dataRow["eduMidSchoolManager"].ToString().Trim() },
                               City = cities.Where(x => x.Id == int.Parse(dataRow["eduSchoolCity"].ToString().Trim())).ToList().First()
                           }).ToList();
            return schools.OrderBy(x => x.Id).ToList();
        }
        /// <summary>
        /// Gets all known schools to israel (last updated 2-5-2017)
        /// </summary>
        /// <returns></returns>
        public static List<School> GetKnownCity(int cid)
        {
            var dt = Connect.GetData("SELECT * FROM eduKnownSchools WHERE eduSchoolCity=" + cid, "eduKnownSchools");
            var cities = CitiesService.GetAll();
            var schools = (from DataRow dataRow in dt.Rows
                           select new School()
                           {
                               Id = int.Parse(dataRow["eduSchoolID"].ToString().Trim()),
                               Name = dataRow["eduSchoolName"].ToString().Trim(),
                               Manager = new Member() { Name = dataRow["eduSchoolManager"].ToString().Trim() },
                               MidManger = new Member() { Name = dataRow["eduMidSchoolManager"].ToString().Trim() },
                               City = cities.Where(x => x.Id == int.Parse(dataRow["eduSchoolCity"].ToString().Trim())).ToList().First()
                           }).ToList();
            return schools.OrderBy(x => x.Id).ToList();
        }
        /// <summary>
        /// Gets all known schools to israel (last updated 2-5-2017)
        /// </summary>
        /// <returns></returns>
        public static School GetKnownById(int id)
        {
            var dt = Connect.GetData("SELECT * FROM eduKnownSchools WHERE eduSchoolID=" + id, "eduKnownSchools");
            if (dt.Rows.Count == 0) return null;
            var school = new School()
            {
                Id = int.Parse(dt.Rows[0]["eduSchoolID"].ToString().Trim()),
                Name = dt.Rows[0]["eduSchoolName"].ToString().Trim(),
                Manager = new Member() { Name = dt.Rows[0]["eduSchoolManager"].ToString().Trim() },
                MidManger = new Member() { Name = dt.Rows[0]["eduMidSchoolManager"].ToString().Trim() }
            };
            return school;
        }
        /// <summary>
        /// Add new school to db
        /// </summary>
        /// <param name="school"></param>
        /// <returns></returns>
        public static bool Add(School school)
        {
            if (school.Id == 0)
                school.Id = GetNID();
            string state = "Yes";
            if (!school.Official)
                state = "No";
            school.Name = school.Name.Replace("'", "''");
            return Connect.InsertUpdateDelete("INSERT INTO eduSchools (eduSchoolID,eduSchoolName,eduSchoolLogo,eduOfficial) VALUES (" +
                                       school.Id + ",'" + school.Name + "', '" + school.LogoPath + "','"+state+"')");
        }
        /// <summary>
        /// Check if the school exsits
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Exsits(int id)
        {
            int count = int.Parse(Connect
                .GetObject("SELECT COUNT(*) FROM eduSchools WHERE eduSchools.eduSchoolID=" + id)
                .ToString());
            return count == 1;
        }

        private static List<School> okSchools = new List<School>();
        /// <summary>
        /// Deletes empty schools
        /// </summary>
        /// <returns></returns>
        public static bool HandleDB()
        {

            List<School> schools = GetAll();
            List<Member> members = MemberService.GetAll();
            foreach (var school in schools)
            {
                int count = members.Count(x => x.School.Id == school.Id);

                if (count != 0)
                {
                    continue;

                }

                SchoolService.Delete(school.Id);
            }
            return true;
        }
        /// <summary>
        /// Delete schools
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Delete(int id)
        {
            return Connect.InsertUpdateDelete("DELETE FROM eduSchools WHERE eduSchools.eduSchoolID=" + id);
        }
        /// <summary>
        /// Get new id for school
        /// </summary>
        /// <returns></returns>
        public static int GetNID()
        {
            Random rnd = new Random();
            int id = rnd.Next();
            int count = int.Parse(Connect
                .GetObject("SELECT COUNT(*) FROM eduKnownSchools,eduSchools WHERE eduKnownSchools.eduSchoolID=" + id +
                           " OR eduSchools.eduSchoolID=" + id)
                .ToString());
            while (count != 0)
            {
                id = rnd.Next();
                count = int.Parse(Connect
                    .GetObject("SELECT COUNT(*) FROM eduKnownSchools,eduSchools WHERE eduKnownSchools.eduSchoolID=" + id +
                               " OR eduSchools.eduSchoolID=" + id)
                    .ToString());
            }
            return id;
        }

        /// <summary>
        /// Get school by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static School GetSchool(int id)
        {
            var dt = Connect.GetData("SELECT * FROM eduSchools WHERE eduSchoolID=" + id, "eduSchools");
            return new School() { Id = int.Parse(dt.Rows[0]["eduSchoolID"].ToString().Trim()), Name = dt.Rows[0]["eduSchoolName"].ToString().Trim(), LogoPath = dt.Rows[0]["eduSchoolLogo"].ToString().Trim(), Official = Convert.ToBoolean(dt.Rows[0]["eduOfficial"]) };
        }
    }
}
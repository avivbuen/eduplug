using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
/// <summary>
/// Summary description for EduAdjustmentsService
/// </summary>
[WebService(Namespace = "http://ws.eduplug.co.il/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class EduAdjustmentsService : System.Web.Services.WebService {
    /// <summary>
    /// Gets all the adjustments types
    /// </summary>
    /// <returns>List of adjustments</returns>
    [WebMethod]
    public List<Adjustment> GetAdjustmentsTypes() {
        return AdjustmentService.GetAll();
    }
    /// <summary>
    /// Gets the adjustment of the student with the following id
    /// </summary>
    /// <param name="id">ID of the student</param>
    /// <returns>List of adjustments the student have</returns>
    [WebMethod]
    public List<Adjustment> GetAdjustmentsStudent(string id)
    {
        return AdjustmentService.GetStudent(id);
    }
    /// <summary>
    /// Add a student to the system with an adjustment
    /// </summary>
    /// <param name="firstName">First Name</param>
    /// <param name="lastName">Last Name</param>
    /// <param name="id">ID</param>
    /// <param name="adid">Adjustment ID</param>
    /// <returns>The result of the action from the system</returns>
    [WebMethod]
    public string AddStudent(string firstName,string lastName,string id, string adid)
    {
        Member mem = new Member() { FirstName = firstName, ID = id, LastName = lastName };
        string result = MemberService.Add(mem);
        try
        {
            result += "%" + AdjustmentService.Add(mem.ID, int.Parse(adid));
        }
        catch (Exception ex)
        {
            result += "%OP_FAILED_WRONG_ADJUSTMENT_ID";
        }
        
        return result;
    }
    /// <summary>
    /// Adds an adjustment to a student that already exsits in the system
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="adid">Adjustment ID</param>
    /// <returns>The result of the action from the system</returns>
    [WebMethod]
    public string AddAdjustmentToStudent(string id, string adid)
    {
        string result = "";
        try
        {
            result += AdjustmentService.Add(id, int.Parse(adid));
        }
        catch (Exception ex)
        {
            result += "OP_FAILED_WRONG_ADJUSTMENT_ID";
        }
        return result;
    }
    /// <summary>
    /// Gets the adjustment of the student with the following id - in JSON
    /// </summary>
    /// <param name="id">ID of the student</param>
    /// <returns>List of adjustments the student have</returns>
    [WebMethod]
    public string GetAdjustmentsStudentJSON(string id)
    {
        return new JavaScriptSerializer().Serialize(AdjustmentService.GetStudent(id));
    }
    /// <summary>
    /// Gets all the students from the system
    /// </summary>
    /// <returns>List of students</returns>
    [WebMethod]
    public List<Member> GetAllStudents()
    {
        return MemberService.GetAll();
    }
    /// <summary>
    /// Gets all the students from the system - JSON
    /// </summary>
    /// <returns>List of students</returns>
    [WebMethod]
    public string GetAllStudentsJSON()
    {
        return new JavaScriptSerializer().Serialize(MemberService.GetAll());
    }
}

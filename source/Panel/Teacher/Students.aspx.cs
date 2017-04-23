using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Grades;
using Business_Logic.Majors;
using Business_Logic.Members;
using Business_Logic.TeacherGrades;

public partial class Panel_Teacher_Students : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Teacher)
            Response.Redirect("~/");
        if (!IsPostBack)
            Fill();
    }
    /// <summary>
    /// Fills the gridview with data from the DB
    /// </summary>
    protected void Fill()
    {
        DataTable dt = MemberService.GetStudents(MemberService.GetCurrent().UserID);
        ListViewUsers.DataSource = dt;
        ListViewUsers.DataBind();
    }
    /// <summary>
    /// Customizing fields during the row data bound event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ListViewUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Age
            DateTime date = DateTime.Parse(e.Row.Cells[4].Text);
            TimeSpan ts = DateTime.Now - date;
            double age = ts.Days / 365.0;
            e.Row.Cells[4].Text = Math.Round(age, 1).ToString();
            //Majors
            string mjrsStr = "";
            List<Major> all = MajorsService.GetUserMajors(MemberService.GetUID(e.Row.Cells[3].Text));
            Major last = all.Last();
            foreach (Major mjr in all)
            {
                if (mjr.Equals(last))
                {
                    mjrsStr += mjr.Title;
                }
                else
                {
                    mjrsStr += mjr.Title + ", ";
                }
            }
            e.Row.Cells[5].Text = mjrsStr;
        }
    }
    protected string CastType(object obj)
    {
        switch (obj.ToString())
        {
            case "a": return "מנהל";
            case "s": return "תלמיד";
            case "t": return "מורה";
        }
        return "לא ידוע";
    }
    protected string CastGPART(object obj, object objt)
    {
        string obt = objt.ToString();
        if (obt == "a" || obt == "t") return "*";
        return TeacherGradeService.GetParTeacherGrade(GradesService.Get((int)obj).Name);
    }
    protected string CastAge(object obj)
    {
        DateTime date = DateTime.Parse(obj.ToString());
        TimeSpan ts = DateTime.Now - date;
        double age = ts.Days / 365.0;
        return Math.Round(age, 1).ToString();
    }

    protected void ListViewUsers_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "AssessT")
        {
            int uid = int.Parse(e.CommandArgument.ToString());
            Response.Redirect("~/Panel/Teacher/Student.aspx?sid=" + uid);
        }

        if (e.CommandName == "AdjustT")
        {
            int uid = int.Parse(e.CommandArgument.ToString());
            Response.Redirect("~/Panel/Teacher/Adjustments.aspx?uid=" + uid);
        }
    }
    protected string CastGender(object obj)
    {
        string gen = obj.ToString();
        if (gen == "m") return "זכר";
        if (gen == "f") return "נקבה";
        return "לא ידוע";
    }
}
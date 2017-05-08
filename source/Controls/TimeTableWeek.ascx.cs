using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Grades;
using Business_Logic.Lessons;
using Business_Logic.TeacherGrades;

public partial class Controls_TimeTable : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["StartDate"] == null)
            Session["StartDate"] = DateTime.Now;
        if (!IsPostBack)
        {
            Session["StartDate"] = DateTime.Now;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                Session["StartDate"] = ((DateTime)Session["StartDate"]).AddDays(7);
            }
            ListViewTimeTable.DataSource = (List<LessonGroup[]>)data;
            ListViewTimeTable.DataBind();
        }

    }
    protected bool teacherTable;
    public bool FullScreen { get; set; }
    public bool TeacherTable
    {
        get
        {
            return this.teacherTable;
        }
        set
        {
            this.teacherTable = value;
        }
    }

    protected ICollection data;
    public ICollection DataSource
    {
        get
        {
            return this.data;
        }
        set
        {
            this.data = value;
            ListViewTimeTable.DataSource = value;
            ListViewTimeTable.DataBind();
        }
    }
    protected string CastLessons(object lsns, DayOfWeek day)
    {
        if (lsns == null) return "<td></td>";
        return GetLessons(((LessonGroup)lsns).Lessons, day);
    }

    protected string GetLink(Lesson lesson, DayOfWeek dayo)
    {
        DateTime day = DateTimeExtensions.StartOfWeek((Session["StartDate"]), dayo);
        if (teacherTable)
            return "onclick=location='Lesson.aspx?lid=" + lesson.Id + "&d=" + day.Day + "&m=" + day.Month + "&y=" + day.Year + "'; class='clickableCell' data-tooltip='שכבה " + TeacherGradeService.GetParTeacherGrade((int)lesson.TeacherGradeId) + "";
        return "";
    }
    protected string GetLessons(List<Lesson> lsns, DayOfWeek day)
    {
        if (lsns.Count == 0) return "";
        string str = "<td" + CastColor(lsns.First().Color) + " " + GetLink(lsns.First(), day) + ">";
        if (teacherTable)
        {
            List<LessonChange> changes = lsns.First().Changes.Where(x => x.Date == DateTimeExtensions.StartOfWeek((Session["StartDate"]), day) && x.LessonId == lsns.First().Id).ToList();
            if (lsns.First().Changes != null && lsns.First().Changes.Count != 0 && changes.Count() == 1)
            {
                string str1 = "";
                switch (changes.First().ChangeType)
                {
                    case LessonChangeType.Cancel:
                        str1 += "- בוטל ";
                        break;
                    case LessonChangeType.Fill:
                        str1 += "- שונה למילויי מקום ";
                        break;
                    case LessonChangeType.Test:
                        str1 += "- שונה למבחן";
                        break;
                    case LessonChangeType.FinalTest:
                        str1 += "- שונה למבחן בגרות ";
                        break;
                    case LessonChangeType.Unknown:
                        str1 += "- " + changes.First().Message + " ";
                        break;
                    default:
                        break;
                }
                return "<td" + CastColor(lsns.First().Color) + " " + GetLink(lsns.First(), day) + ">" + lsns.First().Name + " "+str1+"</td>";
            }
            return "<td" + CastColor(lsns.First().Color) + " " + GetLink(lsns.First(), day) + ">" + lsns.First().Name + "</td>";
        }
            
        if (teacherTable)
            return "<td" + CastColor(lsns.First().Color) + " " + GetLink(lsns.First(), day) + ">" + lsns.First().Name + "</td>";
        foreach (Lesson lsn in lsns)
        {
            List<LessonChange> changes = lsn.Changes.Where(x => x.Date == DateTimeExtensions.StartOfWeek((Session["StartDate"]), day) && x.LessonId == lsn.Id).ToList();

            if (lsn.Changes != null && lsn.Changes.Count != 0 && changes.Count()==1)
            {
                str += "השיעור " + lsn.Name;
                switch (changes.First().ChangeType)
                {
                    case LessonChangeType.Cancel:
                        str += "- בוטל ";
                        break;
                    case LessonChangeType.Fill:
                        str += "- שונה למילויי מקום ";
                        break;
                    case LessonChangeType.Test:
                        str += "- שונה למבחן";
                        break;
                    case LessonChangeType.FinalTest:
                        str += "- שונה למבחן בגרות ";
                        break;
                    case LessonChangeType.Unknown:
                        str += "- " + changes.First().Message + " ";
                        break;
                    default:
                        break;
                }
                str += "לתאריך זה ";
            }
            else
            {
                str += "-" + lsn.Name + "<br/>";
            }
        }
        str += "</td>";
        return str;
    }
    protected static string CastColor(object color)
    {
        if (color == null || color.ToString().Trim() == "")
        {
            return "";
        }
        else
        {
            return " style='background: #" + color.ToString() + ";'";
        }
    }

    protected void LinkButtonForward_Click(object sender, EventArgs e)
    {
        if (Session["StartDate"] == null)
        {
            Session["StartDate"] = DateTime.Now;
        }
        Session["StartDate"] = ((DateTime)Session["StartDate"]).AddDays(7);
        ListViewTimeTable.DataBind();
    }

    protected void LinkButtonBack_Click(object sender, EventArgs e)
    {
        if (Session["StartDate"] == null)
        {
            Session["StartDate"] = DateTime.Now;
        }
        Session["StartDate"] = ((DateTime)Session["StartDate"]).AddDays(-7);
        ListViewTimeTable.DataBind();
    }
    protected void LinkButtonToday_Click(object sender, EventArgs e)
    {
        Session["StartDate"] = DateTime.Now;
        if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
        {
            Session["StartDate"] = ((DateTime)Session["StartDate"]).AddDays(7);
        }
        ListViewTimeTable.DataBind();
    }
}

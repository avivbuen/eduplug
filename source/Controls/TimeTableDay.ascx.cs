using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Lessons;

public partial class Controls_TimeTableDay : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["StartDate1"] == null)
                Session["StartDate1"] = DateTime.Now;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                Session["StartDate1"] = ((DateTime)Session["StartDate1"]).AddDays(1);
            }
            ListViewTimeTable.DataSource = (List<LessonGroup[]>)data;
            ListViewTimeTable.DataBind();
        }
    }
    protected ICollection data;
    protected MemberClearance tableFor;
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
    public MemberClearance TableFor
    {
        get
        {
            return this.tableFor;
        }
        set
        {
            this.tableFor = value;
        }
    }
    protected string CastLessons(object lsns, DayOfWeek day)
    {
        if (lsns == null)
        {
            return "<span></span>";
        }
        return GetLessons(((LessonGroup)lsns).Lessons, day);
    }

    protected string GetLink(Lesson lesson, DayOfWeek dayo)
    {
        if (tableFor == MemberClearance.Teacher)
        {
            DateTime day = DateTimeExtensions.StartOfWeek((Session["StartDate1"]), dayo);
            return "<a href='Lesson.aspx?lid=" + lesson.Id + "&d=" + day.Day + "&m=" + day.Month + "&y=" + day.Year + "' class='secondary-content blue-grey-text' style='float:left'><i class='material-icons'>arrow_back</i></a>";
        }
        return "";
    }
    protected string GetLessons(List<Lesson> lsns, DayOfWeek day)
    {
        if (lsns.Count == 0) return "ריק";
        string str = GetLink(lsns.First(), day) + "<span>";
        if (tableFor == MemberClearance.Teacher)
        {
            List<LessonChange> changes = lsns.First().Changes.AsEnumerable().Where(x => x.Date == DateTimeExtensions.StartOfWeek((Session["StartDate1"]), day) && x.LessonId == lsns.First().Id).ToList();
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
                return GetLink(lsns.First(), day) + "<span>" + lsns.First().Name + " " + str1 + " " + GetLink(lsns.First(), day) + "</span>";
            }
            return GetLink(lsns.First(), day) + "<span>" + lsns.First().Name + "</span>";
        }

        foreach (Lesson lsn in lsns)
        {
            List<LessonChange> changes = lsn.Changes.AsEnumerable().Where(x => x.Date == DateTimeExtensions.StartOfWeek((Session["StartDate1"]), day) && x.LessonId == lsn.Id).ToList();
            if (lsn.Changes != null && lsn.Changes.Count != 0 && changes.Count() == 1)
            {
                str += lsn.Name;
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
            }
            else
            {
                str += lsn.Name + " ";
            }
        }
        str += "</span>";
        if (str == "") return "ריק";
        return str;
    }
}
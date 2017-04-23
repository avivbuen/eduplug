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
using Business_Logic.Members;
using Business_Logic.TeacherGrades;

public partial class InterTrack_Teacher_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Teacher)
            Response.Redirect("~/Default.aspx");
        TimeTableWeek.DataSource = LessonService.GetTimeTable(MemberService.GetCurrent().UserID, MemberClearance.Teacher);
        TimeTableWeek.TableFor = MemberClearance.Teacher;
        TimeTableWeek.DataBind();
        ListViewGrades.DataSource = TeacherGradeService.GetTeacherTeacherGrades(MemberService.GetCurrent().UserID);
        ListViewGrades.DataBind();
    }
   /* public string CastColor(object color)
    {
        if (color == null || color.ToString().Trim()=="")
        {
            return "";
        }
        else
        {
            return "style='background: #"+color.ToString()+";'";
        }
    }
    public string CastClick(object tgid)
    {
        if (tgid == null || tgid.ToString().Trim() == "")
        {
            return "";
        }
        else
        {
            return "onclick=location='GradeShow.aspx?gid="+tgid.ToString()+"';  class='clickableCell' data-tooltip='שכבה " + TeacherGradeService.GetParTeacherGrade((int)tgid) + "";
        }
    }*/
    //protected void DataLisTeacherGrades_EditCommand(object source, DataListCommandEventArgs e)
    //{
    //    DataLisTeacherGrades.EditItemIndex = e.Item.ItemIndex;
    //    DataLisTeacherGrades.DataBind();

    //}
    //protected void DataLisTeacherGrades_ItemDataBound(object sender, DataListItemEventArgs e)
    //{
    //    if (e.Item.ItemType == ListItemType.EditItem)
    //    {
    //        ListBox lbStudents = (ListBox)(e.Item.FindControl("ListBoxStudents"));//Get the students list control of the current item
    //        ListBox lbExams = (ListBox)(e.Item.FindControl("ListBoxExams"));//Get the student list control of the current item
    //        List<Member> students = TeacherGradeService.GetStudents(int.Parse(DataLisTeacherGrades.DataKeys[e.Item.ItemIndex].ToString()));//Gets all the students from DB
    //        List<Exam> exams = ExamService.GetExamByTeacherGradeID(int.Parse(DataLisTeacherGrades.DataKeys[e.Item.ItemIndex].ToString()));

    //        /* Filling the students */
    //        if (students.Count == 0)
    //        {
    //            lbStudents.Items.Add(new ListItem("אין תלמידים", "-1"));
    //            lbStudents.DataBind();
    //        }
    //        else
    //        {
    //            lbStudents.DataSource = TeacherGradeService.GetStudents(int.Parse(DataLisTeacherGrades.DataKeys[e.Item.ItemIndex].ToString()));
    //            lbStudents.DataTextField = "Name";
    //            lbStudents.DataValueField = "UserID";
    //            lbStudents.DataBind();
    //        }
    //        /* END */

    //        /* Filling the exams */
    //        if (exams.Count == 0)
    //        {
    //            lbExams.Items.Add(new ListItem("אין בחינות", "-1"));
    //            lbExams.DataBind();
    //        }
    //        else
    //        {
    //            lbExams.DataSource = TeacherGradeService.GetStudents(int.Parse(DataLisTeacherGrades.DataKeys[e.Item.ItemIndex].ToString()));
    //            lbExams.DataTextField = "Name";
    //            lbExams.DataValueField = "UserID";
    //            lbExams.DataBind();
    //        }
    //        /* END */
    //    }
    //}

    //protected void LinkButtonCloseEdit_Click(object sender, EventArgs e)
    //{

    //}

    //protected void DataLisTeacherGrades_UpdateCommand(object source, DataListCommandEventArgs e)
    //{
    //    DataLisTeacherGrades.EditItemIndex = -1;
    //    DataLisTeacherGrades.DataBind();
    //}

    //protected void DataLisTeacherGrades_ItemCommand(object source, DataListCommandEventArgs e)
    //{
    //    if (e.CommandName=="AddStudents")
    //    {
    //        int key = int.Parse(DataLisTeacherGrades.DataKeys[e.Item.ItemIndex].ToString());
    //        Response.Redirect("~/InterTrack/Teacher/AddStudents.aspx?id="+key);
    //    }
    //}
}
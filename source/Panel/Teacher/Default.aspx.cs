using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class InterTrack_Teacher_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Teacher)
            Response.Redirect("~/Default.aspx");
        TimeTableWeek.DataSource = LessonService.GetTimeTable(MemberService.GetCurrent().UserID, MemberClearance.Teacher);
        TimeTableWeek.TableFor = MemberClearance.Teacher;
        TimeTableWeek.DataBind();
        ListViewGrades.DataSource = tGradeService.GetTeacherTgrades(MemberService.GetCurrent().UserID);
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
            return "onclick=location='GradeShow.aspx?gid="+tgid.ToString()+"';  class='clickableCell' data-tooltip='שכבה " + tGradeService.GetPartGrade((int)tgid) + "";
        }
    }*/
    //protected void DataListGrades_EditCommand(object source, DataListCommandEventArgs e)
    //{
    //    DataListGrades.EditItemIndex = e.Item.ItemIndex;
    //    DataListGrades.DataBind();

    //}
    //protected void DataListGrades_ItemDataBound(object sender, DataListItemEventArgs e)
    //{
    //    if (e.Item.ItemType == ListItemType.EditItem)
    //    {
    //        ListBox lbStudents = (ListBox)(e.Item.FindControl("ListBoxStudents"));//Get the students list control of the current item
    //        ListBox lbExams = (ListBox)(e.Item.FindControl("ListBoxExams"));//Get the student list control of the current item
    //        List<Member> students = tGradeService.GetStudents(int.Parse(DataListGrades.DataKeys[e.Item.ItemIndex].ToString()));//Gets all the students from DB
    //        List<Exam> exams = ExamService.GetExamByTgradeID(int.Parse(DataListGrades.DataKeys[e.Item.ItemIndex].ToString()));

    //        /* Filling the students */
    //        if (students.Count == 0)
    //        {
    //            lbStudents.Items.Add(new ListItem("אין תלמידים", "-1"));
    //            lbStudents.DataBind();
    //        }
    //        else
    //        {
    //            lbStudents.DataSource = tGradeService.GetStudents(int.Parse(DataListGrades.DataKeys[e.Item.ItemIndex].ToString()));
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
    //            lbExams.DataSource = tGradeService.GetStudents(int.Parse(DataListGrades.DataKeys[e.Item.ItemIndex].ToString()));
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

    //protected void DataListGrades_UpdateCommand(object source, DataListCommandEventArgs e)
    //{
    //    DataListGrades.EditItemIndex = -1;
    //    DataListGrades.DataBind();
    //}

    //protected void DataListGrades_ItemCommand(object source, DataListCommandEventArgs e)
    //{
    //    if (e.CommandName=="AddStudents")
    //    {
    //        int key = int.Parse(DataListGrades.DataKeys[e.Item.ItemIndex].ToString());
    //        Response.Redirect("~/InterTrack/Teacher/AddStudents.aspx?id="+key);
    //    }
    //}
}
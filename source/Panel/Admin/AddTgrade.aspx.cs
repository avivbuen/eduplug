using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Grades;
using Business_Logic.Majors;
using Business_Logic.Members;
using Business_Logic.TeacherGrades;

public partial class InterTrack_Admin_AddTeacherGrade : System.Web.UI.Page
{
    public string Script = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Admin)
            Response.Redirect("~/");
        if (!IsPostBack)
            FillData();
    }
    public void FillData()
    {
        ListTeachers.DataSource = MemberService.GetNames().Where(x => x.Auth == MemberClearance.Teacher&& x.Active != "No");
        ListTeachers.DataTextField = "Name";
        ListTeachers.DataValueField = "UserID";
        ListTeachers.DataBind();
        ListMajors.DataSource = MajorsService.GetAll();
        ListMajors.DataTextField = "Title";
        ListMajors.DataValueField = "ID";
        ListMajors.DataBind();
        ListMajors.Items.Add(new ListItem("מגמה חדשה+", "-1"));
    }
    protected void AddButton_Click(object sender, EventArgs e)
    {
        Page.Validate();
        if (!Page.IsValid) return;
        var tg = new TeacherGrade()
        {
            TeacherId = int.Parse(ListTeachers.SelectedValue),
            Name = TeacherGradeName.Text
        };
        TeacherGradeService.Add(tg);
        var tgid = TeacherGradeService.GetId(tg);
        var students = (from ListItem check in StudentsToAdd.Items
            where check.Selected
            select new Member()
            {
                UserID = int.Parse(check.Value)
            }).ToList();
        var majorid = int.Parse(ListMajors.SelectedValue);
        if (majorid!=-1)
        {
            var gPart = TeacherGradeService.GetParTeacherGrade(LisTeacherGrades.SelectedValue);
            MajorsService.SetMajorTeacherGrade(tgid, majorid, gPart);
        }
        else
        {
            var m = new Major()
            {
                Title = MajorName.Text.Trim()
            };
            MajorsService.Add(m);
            majorid = MajorsService.GetMajorID(m.Title);
            var gPart = TeacherGradeService.GetParTeacherGrade(LisTeacherGrades.SelectedValue);
            MajorsService.SetMajorTeacherGrade(tgid, majorid, gPart);
        }
        TeacherGradeService.AddStudents(tgid, students);
        Script = "<script>alert('הכיתה נוספה למערכת.');location='/Default.aspx';</script>";
    }
    protected void LisTeacherGrades_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (LisTeacherGrades.SelectedValue != "-1")
        {
            StudentsToAdd.DataSource = MemberService.GeTeacherGradePart(LisTeacherGrades.SelectedValue.Replace("'", "''"));
            StudentsToAdd.DataTextField = "Name";
            StudentsToAdd.DataValueField = "UserID";
            StudentsToAdd.DataBind();
            if (StudentsToAdd.Items.Count > 0)
            {
                LabelNoStudents.Text = "";
                StudentsToAdd.Visible = true;
                PanelStudents.Visible = true;
            }
            else
            {
                LabelNoStudents.Text = "אין תלמידים בשכבה זו";
                StudentsToAdd.Visible = false;
                PanelStudents.Visible = true;
            }
        }
        else
        {
            StudentsToAdd.Visible = false;
            PanelStudents.Visible = false;
            LabelNoStudents.Text = "";
        }
    }

    protected void cv_StudentsToAdd_ServerValidate(object source, ServerValidateEventArgs args)
    {
        int acc = 0;
        foreach (ListItem item in StudentsToAdd.Items)
            if (item.Selected) acc++;
        args.IsValid = (acc >= 1);
    }

    protected void ListMajors_SelectedIndexChanged(object sender, EventArgs e)
    {
            PanelNewMajor.Visible = (ListMajors.SelectedValue == "-1");
    }

    protected void cvMegama_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if(PanelNewMajor.Visible)
        {
            List<Major> majors = MajorsService.GetAll();
            args.IsValid = (majors.Count(x => x.Title == MajorName.Text.Trim())==0);
            return;
        }
        args.IsValid = true;
    }

    protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
    {

    }
}
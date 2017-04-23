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

public partial class Panel_Admin_EditTeacherGrade : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Admin)
            Response.Redirect("~/");
        if (!IsPostBack)
            FillData();
    }
    public void FillData()
    {
        if (Request.QueryString["tgid"] == null || Request.QueryString["tgid"].ToString() == "")
            Response.Redirect("~/");
        ListTeachers.DataSource = MemberService.GetNames().Where(x => x.Auth == MemberClearance.Teacher && x.Active);
        ListTeachers.DataTextField = "Name";
        ListTeachers.DataValueField = "UserID";
        ListTeachers.DataBind();
        ListMajors.DataSource = MajorsService.GetAll();
        ListMajors.DataTextField = "Title";
        ListMajors.DataValueField = "ID";
        ListMajors.DataBind();
        ListMajors.Items.Add(new ListItem("מגמה חדשה+", "-1"));
        TeacherGrade t = TeacherGradeService.Get(int.Parse(Request.QueryString["tgid"].ToString().Trim()));
        if (t == null) Response.Redirect("~/");
        TeacherGradeName.Text = t.Name;
        ListTeachers.SelectedValue = t.TeacherId.ToString();
        LisTeacherGrades.SelectedValue = TeacherGradeService.GetParTeacherGrade(t.Id);
        ListMajors.SelectedValue = TeacherGradeService.GetMajor(t.Id).ToString();
        Session["kTG"] = t;
        vChanged();
    }
    private void SetSelectedInCheckBox(CheckBoxList list, string listStr)
    {
        string[] values = listStr.Split(',');
        foreach (ListItem item in list.Items)
        {
            if (values.Contains(item.Value))
                item.Selected = true;
            else
                item.Selected = false;
        }
    }
    protected void LisTeacherGrades_SelectedIndexChanged(object sender, EventArgs e)
    {
        vChanged();
    }
    protected void vChanged()
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
        if (Session["kTG"] == null) Response.Redirect("~/");
        TeacherGrade tg = (TeacherGrade)Session["kTG"];
        Member[] mem = TeacherGradeService.GetStudents(tg.Id).ToArray();
        string select = "";
        for (int i = 0; i < mem.Length; i++)
        {
            if (mem.Length - 1 != i)
            {
                select += mem[i].UserID + ",";
            }
            else
            {
                select += mem[i].UserID;
            }
        }
        SetSelectedInCheckBox(StudentsToAdd, select);
    }

    protected void cv_StudentsToAdd_ServerValidate(object source, ServerValidateEventArgs args)
    {
        int acc = 0;
        foreach (ListItem item in StudentsToAdd.Items)
            if (item.Selected) acc++;
        args.IsValid = (acc >= 1);
    }
    protected void AddButton_Click(object sender, EventArgs e)
    {
        Page.Validate();
        if (Page.IsValid)
        {
            if(((TeacherGrade)Session["kTG"])==null)Response.Redirect("~/");
            TeacherGrade tg = ((TeacherGrade)Session["kTG"]);
            int tgid = tg.Id;
            tg.Name = TeacherGradeName.Text;
            tg.TeacherId = int.Parse(ListTeachers.SelectedValue);
            TeacherGradeService.Update(tg);
            List<Member> students = new List<Member>();
            foreach (ListItem check in StudentsToAdd.Items)
            {
                if (check.Selected)
                {
                    Member student = new Member()
                    {
                        UserID = int.Parse(check.Value)
                    };
                    students.Add(student);
                }
            }
            int majorid = int.Parse(ListMajors.SelectedValue);
            if (majorid != -1)
            {
                string gPart = TeacherGradeService.GetParTeacherGrade(LisTeacherGrades.SelectedValue);
                MajorsService.SetMajorTeacherGrade(tgid, majorid, gPart);
            }
            else
            {
                Major m = new Major()
                {
                    Title = MajorName.Text.Trim()
                };
                MajorsService.Add(m);
                majorid = MajorsService.GetMajorID(m.Title);
                string gPart = TeacherGradeService.GetParTeacherGrade(LisTeacherGrades.SelectedValue);
                MajorsService.SetMajorTeacherGrade(tgid, majorid, gPart);
            }
            TeacherGradeService.AddStudents(tgid, students);
            Response.Redirect("~/Panel/Admin/Tgrades.aspx");
            //script = "<script>alert('הכיתה נוספה למערכת.');location='/Default.aspx';</script>";
        }
    }
    protected void ListMajors_SelectedIndexChanged(object sender, EventArgs e)
    {
        PanelNewMajor.Visible = (ListMajors.SelectedValue == "-1");
    }
}
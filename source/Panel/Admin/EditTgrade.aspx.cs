using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_Admin_EditTgrade : System.Web.UI.Page
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
        tGrade t = tGradeService.Get(int.Parse(Request.QueryString["tgid"].ToString().Trim()));
        if (t == null) Response.Redirect("~/");
        tGradeName.Text = t.Name;
        ListTeachers.SelectedValue = t.TeacherID.ToString();
        ListGrades.SelectedValue = tGradeService.GetPartGrade(t.ID);
        ListMajors.SelectedValue = tGradeService.GetMajor(t.ID).ToString();
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
    protected void ListGrades_SelectedIndexChanged(object sender, EventArgs e)
    {
        vChanged();
    }
    protected void vChanged()
    {
        if (ListGrades.SelectedValue != "-1")
        {
            StudentsToAdd.DataSource = MemberService.GetGradePart(ListGrades.SelectedValue.Replace("'", "''"));
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
        tGrade tg = (tGrade)Session["kTG"];
        Member[] mem = tGradeService.GetStudents(tg.ID).ToArray();
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
            if(((tGrade)Session["kTG"])==null)Response.Redirect("~/");
            tGrade tg = ((tGrade)Session["kTG"]);
            int tgid = tg.ID;
            tg.Name = tGradeName.Text;
            tg.TeacherID = int.Parse(ListTeachers.SelectedValue);
            tGradeService.Update(tg);
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
                string gPart = tGradeService.GetPartGrade(ListGrades.SelectedValue);
                MajorsService.SetMajorTgrade(tgid, majorid, gPart);
            }
            else
            {
                Major m = new Major()
                {
                    Title = MajorName.Text.Trim()
                };
                MajorsService.Add(m);
                majorid = MajorsService.GetMajorID(m.Title);
                string gPart = tGradeService.GetPartGrade(ListGrades.SelectedValue);
                MajorsService.SetMajorTgrade(tgid, majorid, gPart);
            }
            tGradeService.AddStudents(tgid, students);
            Response.Redirect("~/Panel/Admin/Tgrades.aspx");
            //script = "<script>alert('הכיתה נוספה למערכת.');location='/Default.aspx';</script>";
        }
    }
    protected void ListMajors_SelectedIndexChanged(object sender, EventArgs e)
    {
        PanelNewMajor.Visible = (ListMajors.SelectedValue == "-1");
    }
}
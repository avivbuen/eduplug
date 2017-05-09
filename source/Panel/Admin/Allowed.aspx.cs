using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic;
using Business_Logic.Members;

public partial class Panel_Admin_Allowed : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (MemberService.GetCurrent().Auth != MemberClearance.Admin)
            Response.Redirect("~/Default.aspx");
        if (!IsPostBack)
            Fill();
        //Adds THEAD and TBODY to GridView.



    }
    /// <summary>
    /// Fills the gridview with data from the DB
    /// </summary>
    protected void Fill()
    {
        var dt = MemberService.GetAllowed();
        dt.Columns.Add("Pass");

        DataTable dt1DataTable = dt.Clone();
        foreach (DataRow dataRow in dt.Rows)
        {
            string id = dataRow["eduUserID"].ToString();
            string strNew = "";
            foreach (char cid in id)
            {
                int c = int.Parse(cid.ToString());
                char cc = (char)(c + 97);
                strNew += cid + cc.ToString();
            }
            dataRow["Pass"] = strNew;
            dt1DataTable.ImportRow(dataRow);
        }
        if (dt1DataTable.Rows.Count != 0)
        {
            GridViewUsers.DataSource = dt1DataTable;
            GridViewUsers.DataBind();
            LabelEmpty.Text = "";
            return;
        }
        LabelEmpty.Text = "אין מורשים";
    }
    protected void GridViewUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (e.NewPageIndex < 0 || e.NewPageIndex > GridViewUsers.PageCount) return;
        GridViewUsers.PageIndex = e.NewPageIndex;
        Fill();
    }
    protected string GetYesNo(bool o)
    {
        if (o) return "כן";
        return "לא";
    }
    protected void GridViewUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditT")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewUsers.Rows[index];
            Response.Redirect("~/Panel/Admin/EditAllowed.aspx?uid=" + row.Cells[1].Text);
        }
        if (e.CommandName == "DeleteT")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridViewUsers.Rows[index];
            MemberService.RemoveFromActive(MemberService.GetUID(row.Cells[1].Text, MemberService.GetCurrent().School.Id));
            Fill();
        }
    }
    protected string CastAge(object obj)
    {
        DateTime date = DateTime.Parse(obj.ToString());
        TimeSpan ts = DateTime.Now - date;
        double age = ts.Days / 365.0;
        return Math.Round(age, 1).ToString();
    }
    protected string CastType(object obj)
    {
        switch (obj.ToString())
        {
            case "a": return "מנהל";
            case "s": return "תלמיד";
            case "t": return "מורה";
            case "p": return "הורה";
        }
        return "לא ידוע";
    }

    protected void GridViewUsers_DataBinding(object sender, EventArgs e)
    {
        if (GridViewUsers.HeaderRow != null)
            GridViewUsers.HeaderRow.TableSection = TableRowSection.TableHeader;

    }
}
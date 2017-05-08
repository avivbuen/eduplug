using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic.Members;

public partial class Panel_Admin_PassTempPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = MemberService.GetAllowed();
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

        GridViewPass.DataSource = dt;
        GridViewPass.DataBind();
    }
    protected void GridViewDiscplines_DataBound(object sender, EventArgs e)
    {
        if (GridViewPass.HeaderRow != null)
            GridViewPass.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
}
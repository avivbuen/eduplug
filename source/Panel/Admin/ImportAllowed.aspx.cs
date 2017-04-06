using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_Admin_ImportAllowed : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack && FileUploadExcel.PostedFile != null)
        {
            if (FileUploadExcel.PostedFile.FileName.Length > 0)
            {
                if (Path.GetExtension(FileUploadExcel.FileName) == ".xlsx")
                {
                    ExcelPackage package = new ExcelPackage(FileUploadExcel.FileContent);
                    DataTable dt = package.ToDataTable();
                    Tuple<string, bool, int> valFunc = ValidateExcel(dt);
                    LiteralResp.Text = valFunc.Item1;
                    if (valFunc.Item2)
                    {
                        LiteralRespIcon.Text = "done";
                        PanelUpload.Visible = false;
                    }
                    else
                    {
                        LiteralRespIcon.Text = "close";
                    }
                }
                else
                {
                    LiteralResp.Text = "xlsx תומך רק בקבצי";
                    LiteralRespIcon.Text = "close";
                }
            }


        }
    }

    public Tuple<string, bool, int> ValidateExcel(DataTable dt)
    {
        //Validate number of columns
        if (dt.Columns.Count != 3)
        {
            return new Tuple<string, bool, int>("כמות עמודות לא שווה 3", false, -1);
        }
        //Validate rows id
        string[] state = new string[3];
        for (int i = 0; i < state.Length; i++)
        {
            state[i] = "";
        }
        foreach (DataRow dr in dt.Rows)
        {
            int index = 0;
            foreach (DataColumn dc in dt.Columns)
            {
                if (state[index] == "")
                {
                    state[index] = GetCellType(dr[dc].ToString());
                }
                else if (state[index] != GetCellType(dr[dc].ToString()))
                {
                    return new Tuple<string, bool, int>("אין אחדות בנתונים", false, -1);
                }
                index++;
            }
        }
        int indNum = -1;
        for (int i = 0; i < state.Length; i++)
        {
            if (state[i] == "num")
            {
                indNum = i;
                break;
            }
        }
        if (indNum == -1)
            return new Tuple<string, bool, int>("לא מכיל תעודות זהות", false, -1);

        foreach (DataRow dr in dt.Rows)
        {
            if (!CheckIDNo(dr[indNum].ToString()))
            {
                return new Tuple<string, bool, int>("קיימות תעודות זהות לא תקינות", false, indNum);
            }
        }
        return new Tuple<string, bool, int>("תקין", true, indNum);
    }
    static bool CheckIDNo(string strID)
    {
        int[] id_12_digits = { 1, 2, 1, 2, 1, 2, 1, 2, 1 };
        int count = 0;

        if (strID == null)
            return false;

        strID = strID.PadLeft(9, '0');

        for (int i = 0; i < 9; i++)
        {
            int num = int.Parse(strID.Substring(i, 1)) * id_12_digits[i];

            if (num > 9)
                num = (num / 10) + (num % 10);

            count += num;
        }

        return (count % 10 == 0);
    }
    public string GetCellType(string data)
    {
        double n;
        bool isNumeric = double.TryParse(data, out n);
        if (isNumeric)
            return "num";
        return "str";
    }
}
public static class ExcelPackageExtensions
{
    public static DataTable ToDataTable(this ExcelPackage package)
    {
        ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
        DataTable table = new DataTable();
        foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
        {
            table.Columns.Add(firstRowCell.Text);
        }

        for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
        {
            var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
            var newRow = table.NewRow();
            foreach (var cell in row)
            {
                newRow[cell.Start.Column - 1] = cell.Text;
            }
            table.Rows.Add(newRow);
        }
        return table;
    }
}
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic.Members;

public partial class Panel_Admin_ImportAllowed : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HandleFile();
    }
    protected void HandleFile()
    {
        if (IsPostBack && FileUploadExcel.PostedFile != null && DropDownFam.SelectedValue != "-1")
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
                        int[] indexs = new int[3];
                        int indFname = 0;
                        for (int i = 0; i < 3; i++)
                        {
                            if (valFunc.Item3 != i && int.Parse(DropDownFam.SelectedValue) != i)
                            {
                                indFname = i;
                                break;
                            }
                        }
                        int count = MemberService.AddAllowed(dt, valFunc.Item3, indFname, int.Parse(DropDownFam.SelectedValue));
                        LiteralResp.Text += " " + (dt.Rows.Count - count) + " כבר קיימים " + count.ToString();
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
                return new Tuple<string, bool, int>("תעודות זהות לא תקינות", false, indNum);
            }
        }
        if (int.Parse(DropDownFam.SelectedValue) == indNum)
            return new Tuple<string, bool, int>("עמודת המשפחה מכילה מספרים", false, indNum);
        return new Tuple<string, bool, int>("הועלה", true, indNum);
    }
    static bool CheckIDNo(string strID)
    {
        strID = strID.Replace("'", "");
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
        data = data.Replace("'", "");
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
        int t = 0;
        foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
        {
            t++;
            table.Columns.Add("T" + t);
        }

        for (var rowNumber = 1; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
        {
            var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
            var newRow = table.NewRow();
            foreach (var cell in row)
            {
                try
                {
                    if ((cell.Start.Column - 1) < workSheet.Dimension.End.Column)
                        newRow[cell.Start.Column - 1] = cell.Text;
                    else
                        return new DataTable();
                }
                catch (System.IndexOutOfRangeException e)
                {
                    return new DataTable();
                    
                }

            }
            table.Rows.Add(newRow);
        }
        return table;
    }
}
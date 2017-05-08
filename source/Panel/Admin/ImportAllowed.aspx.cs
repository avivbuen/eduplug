using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using Business_Logic.Cities;
using Business_Logic.Grades;
using Business_Logic.Majors;
using Business_Logic.Members;

public partial class Panel_Admin_ImportAllowed : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HandleFile();
    }
    protected void HandleFile()
    {/*
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
                        int[] indexs = new int[3];
                        int indFname = 0;
                        for (int i = 0; i < 3; i++)
                        {
                            if (valFunc.Item3 != i)
                            {
                                indFname = i;
                                break;
                            }
                        }

                        string resAddAllowed = MemberService.AddAllowed(dt);
                        LiteralResp.Text += resAddAllowed;
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
        }*/
    }

    public Tuple<string, bool, int> ValidateExcel(DataTable dt)
    {
        //Validate number of columns
        if (dt.Columns.Count != 10)
        {
            return new Tuple<string, bool, int>("כמות עמודות לא שווה 10", false, -1);
        }
        //Validate rows id
        string[] state = new string[10];
        for (int i = 0; i < state.Length; i++)
        {
            state[i] = dt.Rows[0][i].ToString();
        }
        int rowNum = 0;
        foreach (DataRow dr in dt.Rows)
        {
            rowNum++;
            int index = 0;
            List<Grade> grades = GradesService.GetAll();
            DateTime dat;
            List<Major> majors = MajorsService.GetAll();
            List<City> cities = CitiesService.GetAll();
            foreach (DataColumn dc in dt.Columns)
            {
                switch (state[index])
                {
                    case "שם פרטי":
                        if(rowNum==1)continue;
                        if (!CheckName(dr[dc].ToString().Replace("`", "").Replace("-", "")))
                            return new Tuple<string, bool, int>("שם פרטי באחד או יותר מן הרשומות לא חוקי", false, -1);
                        break;
                    case "שם משפחה":
                        if (rowNum == 1) continue;
                        if (!CheckName(dr[dc].ToString().Replace("`", "").Replace("-", "")))
                            return new Tuple<string, bool, int>("שם משפחה באחד או יותר מן הרשומות לא חוקי", false, -1);
                        break;
                    case "תעודת זהות":
                        if (rowNum == 1) continue;
                        if (!CheckIDNo(dr[dc].ToString())) return new Tuple<string, bool, int>("תעודות זהות לא חוקיות קיימות", false, -1);
                        break;
                    case "סוג":
                        if (rowNum == 1) continue;
                        if (!CheckType(dr[dc].ToString())) return new Tuple<string, bool, int>("סוגים לא חוקיים", false, -1);
                        break;
                    case "כיתה":
                        if (rowNum == 1) continue;
                        if (grades.All(x=> x.Name!= dr[dc].ToString()))
                            return new Tuple<string, bool, int>("כיתות לא קיימות", false, -1);
                        break;
                    case "תאריך לידה":
                        if (rowNum == 1) continue;
                        if (!DateTime.TryParse(dr[dc].ToString(), out dat))
                            return new Tuple<string, bool, int>("תאריכי לידה לא תקינים", false, -1);
                        break;
                    case "מגדר":
                        if (rowNum == 1) continue;
                        if (dr[dc].ToString()!="זכר"&& dr[dc].ToString()!="נקבה") return new Tuple<string, bool, int>("מגדרים לא תקינים", false, -1);
                        break;
                    case "מזהי מגמות":
                        if (rowNum == 1) continue;
                        string[] idStrings = dr[dc].ToString().Split(',');
                        if (idStrings.Any(s => majors.All(x=>x.Id!=int.Parse(s))))
                        {
                            return new Tuple<string, bool, int>("מזהי מגמות אשר לא קיימות", false, -1);
                        }
                        break;
                    case "מזהה עיר":
                        if (rowNum == 1) continue;
                        if (cities.All(x=> x.Id!= int.Parse(dr[dc].ToString()))) return new Tuple<string, bool, int>("מזהה עיר אשר לא קיימ/ת/ות", false, -1);
                        break;
                    case "פלאפון": if (rowNum == 1) continue; break;
                    default:
                        return new Tuple<string, bool, int>("עמודות חסרות/שגויות", false, -1);
                }
                index++;
            }
        }
        return new Tuple<string, bool, int>("הועלה", true, -1);
    }

    static bool CheckName(string name)
    {
        if (name.Length < 2)
            return false;
        if (name.Length > 32)
            return false;
        return name.All(c => c >= 'א' && c <= 'ת');
    }
    static bool CheckIDNo(string strId)
    {
        strId = strId.Replace("'", "");
        int[] id12Digits = { 1, 2, 1, 2, 1, 2, 1, 2, 1 };
        int count = 0;

        strId = strId.PadLeft(9, '0');

        for (int i = 0; i < 9; i++)
        {
            int num = int.Parse(strId.Substring(i, 1)) * id12Digits[i];

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

    public bool CheckType(string str)
    {
        string[] strings = { "תלמיד", "הורה", "מורה", "מנהל" };
        return (strings.Contains(str));
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
            table.Columns.Add(firstRowCell.Text);
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
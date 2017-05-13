<%@ WebHandler Language="C#" Class="Import" %>

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using Business_Logic;
using Business_Logic.Cities;
using Business_Logic.Grades;
using Business_Logic.Majors;
using Business_Logic.Members;
using OfficeOpenXml;

public class Import : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string savedFileName = "";
        if (MemberService.GetCurrent().Auth != MemberClearance.Admin)
            return;
        foreach (string file in context.Request.Files)
        {
            HttpPostedFile hpf = context.Request.Files[file] as HttpPostedFile;
            if (hpf == null || hpf.ContentLength == 0)
                continue;
            Stream stream = hpf.InputStream;
            if (Path.GetExtension(hpf.FileName) == ".xlsx")
            {
                ExcelPackage package = new ExcelPackage(stream);
                DataTable dt = package.ToDataTable();
                Tuple<string, bool> valFunc = ValidateExcel(dt);
                if (valFunc.Item2)
                {
                    string resAddAllowed = MemberService.AddAllowed(dt);
                    context.Response.Write("הועלה");
                }
                else
                {
                    context.Response.Write(valFunc.Item1);
                }
            }
            else
            {
                context.Response.Write("קובץ לא מתאים");
            }
            context.Response.Write(savedFileName);
        }
    }
    public Tuple<string, bool> ValidateExcel(DataTable dt)
    {
        //Validate number of columns
        if (dt.Columns.Count < 10 || dt.Columns.Count > 12)
        {
            return new Tuple<string, bool>("כמות עמודות לא תקין", false);
        }
        //Validate rows id
        string[] state = new string[dt.Columns.Count];
        for (int i = 0; i < state.Length; i++)
        {
            state[i] = dt.Rows[0][i].ToString();
        }
        int rowNum = 0;
        List<Grade> grades = GradesService.GetAll();
        DateTime dat;
        List<Major> majors = MajorsService.GetAll();
        List<City> cities = CitiesService.GetAll();

        foreach (DataRow dr in dt.Rows)
        {
            rowNum++;
            int index = 0;
            foreach (DataColumn dc in dt.Columns)
            {
                switch (state[index])
                {
                    case "שם פרטי":
                        if (rowNum == 1) continue;
                        if (!CheckName(dr[dc].ToString().Replace("`", "").Replace("-", "")))
                            return new Tuple<string, bool>("שם פרטי באחד או יותר מן הרשומות לא חוקי"+ " שורה "+(rowNum-1), false);
                        break;
                    case "שם משפחה":
                        if (rowNum == 1) continue;
                        if (!CheckName(dr[dc].ToString().Replace("`", "").Replace("-", "")))
                            return new Tuple<string, bool>("שם משפחה באחד או יותר מן הרשומות לא חוקי"+ " שורה "+(rowNum-1), false);
                        break;
                    case "תעודת זהות":
                        if (rowNum == 1) continue;
                        if (!CheckIDNo(dr[dc].ToString())) return new Tuple<string, bool>("תעודות זהות לא חוקיות קיימות"+ " שורה "+(rowNum-1), false);
                        break;
                    case "סוג":
                        if (rowNum == 1) continue;
                        if (!CheckType(dr[dc].ToString())) return new Tuple<string, bool>("סוגים לא חוקיים"+ " שורה "+(rowNum-1), false);
                        break;
                    case "כיתה":
                        if (rowNum == 1) continue;
                        if (grades.All(x => x.Name != dr[dc].ToString()))
                            return new Tuple<string, bool>("כיתות לא קיימות"+ " שורה "+(rowNum-1), false);
                        break;
                    case "תאריך לידה":
                        if (rowNum == 1) continue;
                        if (!DateTime.TryParse(dr[dc].ToString(), out dat))
                            return new Tuple<string, bool>("תאריכי לידה לא תקינים"+ " שורה "+(rowNum-1), false);
                        break;
                    case "מגדר":
                        if (rowNum == 1) continue;
                        if (dr[dc].ToString() != "זכר" && dr[dc].ToString() != "נקבה") return new Tuple<string, bool>("מגדרים לא תקינים", false);
                        break;
                    case "מזהי מגמות":
                        if (rowNum == 1) continue;
                        string[] idStrings = dr[dc].ToString().Split(',');
                        if (idStrings.Any(s => majors.All(x => x.Id != int.Parse(s))))
                        {
                            return new Tuple<string, bool>("מזהי מגמות אשר לא קיימות"+ " שורה "+(rowNum-1), false);
                        }
                        break;
                    case "מזהה עיר":
                        if (rowNum == 1) continue;
                        if (cities.All(x => x.Id != int.Parse(dr[dc].ToString()))) return new Tuple<string, bool>("מזהה עיר אשר לא קיימ/ת/ות"+ " שורה "+(rowNum-1), false);
                        break;
                    case "פלאפון": if (rowNum == 1) continue; break;
                    case "מייל":
                        if (rowNum == 1) continue;
                        if (dr[dc].ToString() == "") continue;
                        if (!ValidateMail(dr[dc].ToString()))
                            return new Tuple<string, bool>("כתובת אימייל לא תקינה " + dr[dc].ToString()+ " שורה "+(rowNum-1), false);
                        break;
                    case "אימייל":
                        if (rowNum == 1) continue;
                        if (dr[dc].ToString() == "") continue;
                        if (!ValidateMail(dr[dc].ToString()))
                            return new Tuple<string, bool>("כתובת אימייל לא תקינה " + dr[dc].ToString()+ " שורה "+(rowNum-1), false);
                        break;
                    case "ילדים":
                        if (rowNum == 1) continue;
                        if (dr[dc].ToString().Trim() == "") continue;
                        string[] strings = dr[dc].ToString().Split(',');
                        int a;
                        if (strings.Any(s => s.Length!=9|| !int.TryParse(s,out a) || !CheckIDNo(s)))
                        {
                            return new Tuple<string, bool>("תעודות זהות של ילד/ים לא תקינות " + dr[dc].ToString()+ " שורה "+(rowNum-1), false);
                        }
                        break;
                    default:
                        return new Tuple<string, bool>("עמודות חסרות/שגויות", false);
                }
                index++;
            }
        }
        return new Tuple<string, bool>("הועלה", true);
    }

    static bool CheckName(string name)
    {
        if (name.Length < 2)
            return false;
        if (name.Length > 32)
            return false;
        return name.All(c => c >= 'א' && c <= 'ת');
    }

    static bool ValidateMail(string mail)
    {
        return Regex.IsMatch(mail, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
    }
    static bool CheckIDNo(string strId)
    {
        try
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
        catch
        {
            return false;
        }
    }
    public bool CheckType(string str)
    {
        string[] strings = { "תלמיד", "הורה", "מורה", "מנהל" };
        return (strings.Contains(str));
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
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
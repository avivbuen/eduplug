using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business_Logic.Cities;
using Business_Logic.Schools;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

public partial class Test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CitiesService.UpdateCities();
    }


}
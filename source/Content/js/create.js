//$('.message').click(function () {//Switching views Register/Login
//    $('.frm').animate({ height: "toggle", opacity: "toggle" }, "slow");
//});

var frmCount = 4;
function NavigateForm(frmNum)
{
    for (var i = 1; i < frmCount+1; i++) {
        $('.frm'+i).animate({ height: "hide", opacity: "hide" }, "slow");
    }
    $('.frm' + frmNum).animate({ height: "show", opacity: "show" }, "slow");
}
function ValidateForm()
{
    if (!Page_ClientValidate('RegisterValidationGroup')) {
        if ($("#RegisterValidationSummary").html().indexOf("שם") != -1 || $("#RegisterValidationSummary").html().indexOf("זהות") != -1)
            NavigateForm(1);
        else if ($("#RegisterValidationSummary").html().indexOf("אימייל") != -1 || $("#RegisterValidationSummary").html().indexOf("סיסמה") != -1)
            NavigateForm(2);
        else if($("#RegisterValidationSummary").html().indexOf("כיתה") != -1 || $("#RegisterValidationSummary").html().indexOf("עיר") != -1)
            NavigateForm(3);
        return false;
    }
    return true;
}
function load()
{
    if (window.location.hash) {
        var hash = window.location.hash.substring(1); //Puts hash in variable, and removes the # character
        NavigateForm(hash[hash.length-1]);
    } else {
        // Fragment doesn't exist
    }
}
load();
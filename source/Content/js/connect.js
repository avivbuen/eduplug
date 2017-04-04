
$('.message a').click(function () {//Switching views Register/Login
    location = "Register.aspx";
});
var wrong = ["טעות בתעודת זהות או סיסמה", "אופס כנראה טעית בסיסמה או בתעודת זהות", "תעודת הזהות ו/או הסיסמה שגוים"];
$("form").submit(function (event) {
    event.preventDefault();//Preventing Post-Back
    if (Page_ClientValidate("LoginValidationGroup")) {//Validating the basics
        var url = "../../System/SysAuth.ashx";//Server validation page path
        $.ajax({//Calling the server in order to validate
            url: url,
            data: { id: getUserid(), pass: getPass() },//Sending the credentials
            type: "POST",
            success: function (data) {//Got response
                var user = JSON.parse(data);
                if (user.fname == "non" && user.lname == "non") {
                    $(".errorCode").html(wrong[Math.floor((Math.random() * (wrong.length-1)))]);
                } else {
                    $("#username").html(user.fname + " " + user.lname);
                    $('.login-form').css("display", "none");//Hiding the login form
                    $('.checkmark').css("display", "block");//showing the success with name
                    window.setTimeout(function () {//Start of timeout message
                        window.location.href = '';//Redirect to area of the user
                    }, 3000);//The timeout is to wait 3 second before redirect to dashboard
                }
            },
            error: function (reponse) {//Did not get response or damaged one
                console.log("Avivnet System Error (Ajax) :" + reponse);//Log the error to the console for debug and easy troubleshooting
            }
        });
    }
});
function LoadCurrentUser() {
    var url = host + "/SYSTEM/SysAuth.ashx";
    $.ajax({//*
        url: url,
        data: { nada: Infinity },
        type: "POST",
        success: function (data) {
            var user = JSON.parse(data);
            GlobalUser = user;
            if (user.fname != "non" && user.lname != "non") {
                location = host;//Redirecting to login
            }
        },
        error: function (reponse) {
            console.log("Avivnet System Error (Ajax) - Loading User : " + reponse);
        }
    });
    setTimeout(LoadCurrentUser, 3000);
}

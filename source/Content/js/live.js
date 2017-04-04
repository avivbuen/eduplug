function Logout() {
    var url = host + "/SYSTEM/SysAuth.ashx";
    $.ajax({//*
        url: url,
        data: { logout: Infinity },
        type: "GET",
        success: function () {
            location = host;
        },
        error: function (reponse) {
            console.log("Avivnet System Error (Ajax) - Logout : " + reponse);
        }
    });
}
function LoadCurrentUser() {
    var url = host + "/SYSTEM/SysAuth.ashx";
    $.ajax({//*
        url: url,
        data: { nada: Infinity },
        type: "POST",
        success: function (data) {
            var user = JSON.parse(data);
            GlobalUser = user;
            if (user.fname == "non" && user.lname == "non") {
                location = host;//Redirecting to login
            } else {
                $(".user-name-view").html(user.grt+ " - " +user.fname + " " + user.lname);//Filling the name
                $(".user-email-view").html(user.email);//Filling the email
                $(".user-grt").html(user.grt);//Filling the email
                $(".user-pic-view").attr("src", host+user.pic);//Filling the user profile picture source
                if (user.m_count == 0) {
                    $(".m-count").hide();
                }
                else {
                    $(".m-count").show();
                    $(".m-count").html(user.m_count);
                }
                if (user.clr == "a") {
                    $(".user-admin-view").html("<i class='material-icons'>android</i>");
                }
                $(".visiLoad").show();
                $(".visiReady").hide();
            }
        },
        error: function (reponse) {
            console.log("Avivnet System Error (Ajax) - Loading User : " + reponse);
        }
    });
    setTimeout(LoadCurrentUser, 3000);
}


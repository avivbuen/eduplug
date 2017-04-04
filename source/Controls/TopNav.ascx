<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TopNav.ascx.cs" Inherits="Controls_TopNav" %>
<!-- Top Navigation -->
<nav class="green">
    <div id="MainNavbar" class="nav-wrapper" <%=style %>>
        <ul class="left">
            <li class="visiReady" style="margin-top: 5%; margin-left: 5%; height: 0.5em">
                <div class="preloader-wrapper large active">
                    <div class="spinner-layer spinner-white-only">
                        <div class="circle-clipper left">
                            <div class="circle"></div>
                        </div>
                        <div class="gap-patch">
                            <div class="circle"></div>
                        </div>
                        <div class="circle-clipper right">
                            <div class="circle"></div>
                        </div>
                    </div>
                </div>
            </li>
            <li class="waves-effect dropdown-button visiLoad" data-activates='dropdown-user' style="height: 100%; font-size: 25px; border-radius: 3px; padding-left: 10px; padding-right: 10px">
                <span class="user-name-view hide-on-small-and-down" style="margin: 2px; font-weight: 600;"></span>

                <img src="#" class="user-pic-view" style="width: 50px; height: 50px; vertical-align: middle; border-radius: 50%; background: #ffffff; image-rendering: crisp-edges;" />
            </li>
            <%--            <li class="waves-effect dropdown-button visiLoad" data-activates="dropdown-notfication">
                <a href="#" data-activates="slide-out" class="boxes flat-box waves-effect waves-block"><i class="material-icons">announcement</i></a>
            </li>--%>
            <li>
                <!-- User Dropdown Structure -->
                <ul id='dropdown-user' class='dropdown-content' style="width: 300px; text-align: center">
                    <li style="background-color: #6385b2; text-align: center;">
                        <img src="#" class="user-pic-view" style="border-radius: 50%; width: 170px; height: 170px; background: #ffffff; margin-top: 30px" /><span style="color: #fff; font-size: 30px" class="user-name-view"></span>

                    </li>
                    <li style="text-align: center" class="actionLink"><a href="#!" onclick="Logout()"><span style="font-size: 20px"></span><i class="material-icons">play_for_work</i></a></li>

                </ul>
                <%--                <!-- Notfication Dropdown Structure -->
                <ul id='dropdown-notfication' class='dropdown-content' style="width: 300px; text-align: center">
                    <li style="font-size: 20px; text-align: center; vertical-align: middle; padding-top: 10px; font-weight: 700;">הודעות</li>
                    <li class="divider"></li>
                    <li style="text-align: right">אין הודעות חדשות</li>
                </ul>--%>
            </li>
        </ul>
        <ul class="right">
            <li><a id="ToggleMenu1" href="#" style="vertical-align: middle" data-activates="slide-out" class="boxes flat-box waves-effect waves-block"><i class="material-icons">menu</i></a></li>
            <li><a id="HomeBtn" href="~/" runat="server" style="vertical-align: middle" data-activates="slide-out" class="boxes flat-box waves-effect waves-block hide-on-small-and-down"><i class="material-icons">home</i></a></li>
        </ul>
    </div>
</nav>

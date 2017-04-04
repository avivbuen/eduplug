<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Intersense.ascx.cs" Inherits="Controls_Intersense" %>
<!--
    ***************************************************
    Ѧ√їṽηℯt LTD

    This code is property of Ѧ√їṽηℯt
    using it or modifying it without permissions
    from system admin is illegal and the violator
    will be prosecuted in the court of law.

    Our website: http://www.avivnet.com
    - Code Base & UI by Aviv Bueno : aviv.buen@gmail.com
    <%-- MESSAGE: See readme for further information about the --%>

    Start Date: <%=EduSysDate.GetStart().ToShortDateString() %>
    End Date: <%=EduSysDate.GetEnd().ToShortDateString() %>

    Queries Run In Load: <%=Connect.QueriesCount%><%Connect.QueriesCount = 0; %>

    Device Operating System: <%= Intel.GetUserPlatform()%>

    IP: <%=Intel.GetIpAddress() %>

    Device Family: <%=Intel.GetDeviceFamily() %>
    <%if (Request.UrlReferrer != null)
    { %>
    Refer(The url you came from): <%= Request.UrlReferrer %>
    <%} %>
		   _    _____         _
		   \`,""   ,'7"r-..__/ \
		  ,'\ `, ,',','    _/   \
		 /   \  7 / /     (   \ |
		J     \/ j  L______\  / |
		L   __JF"""/""\"\_,    /
		L,-"| O|  f O |  L_  _/
		F   \_ /  \__/   `-  j|
			.-'    `"""    ,' |          _..====.._
			\__/         r"_  A        ,' _..---.._`,
			 `-.______,,-L// / \  ___,' ,'_..:::.. `,`,
					  j   / / / 7"    `-<""=:'  '':. \ \
					 /   <,' /  F  . i , \   `,    :A V I
					 |    \,'  /    >X<  |     \   :| | V
					 |     `._/    ' ! ` |      I  :| |  G
					  \           \     /       |  :B U  |
					 __>-.__   __,-\   |        |  E N   |
					/     /|   | \  \  \_       | 'O 0   |
				   /   __/ |   |  V  O   \      0./ /    L
				  /   |    |   4  E  L    |    0./ /   ,'
				 J    \    I    L R  D    |    // / _,'
		_________|     |   F    | Y  0    |   //_/-'
		\   __   |    /   J     |/  0     |  /="
		\\  \_\  \__,' \  F     |   0     |
		\\\_____________\/      F__/      F
		 \\| ASP.NET for |_____/  (______/
		  \/__Dummies___/
    ***************************************************
-->
<script>
    console.log("%cEduPlug v1.2 by Ѧ√їṽηℯt LTD", "color: blue; font-size: xx-large");
    console.log("%cStop!", "background: red; color: yellow; font-size: xx-large");
    console.log("%cIf someone told you to paste something here they might be trying to hack your account!", "color: red; font-size: large");
</script>
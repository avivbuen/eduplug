<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Messages_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="messages" class="mailbox section">
        <div class="row">
            <div class="col s12">
                <div class="z-depth-1">
                    <nav class="z-depth-0 blue-grey">
                        <div class="nav-wrapper">
                            <div class="col s10" style="width: 100%; white-space: nowrap !important;">
                                <a href="Compose.aspx"  style="font-size:2em;" class="left"><i class="material-icons">add</i></a>
                                <div class="center" style="font-size:2em">הודעות</div>
                            </div>
                        </div>
                    </nav>
                    <ul class="tabs tab-demo" style="width: 100%;">
                        <li class="tab col s4"><a href="#out-mailbox" class="blue-grey-text">יוצא</a></li>
                        <li class="tab col s4"><a href="#in-mailbox" class="active blue-grey-text">נכנס</a></li>
                        <li class="tab col s4"><a href="#main-mailbox" class="blue-grey-text">הכל</a></li>
                    </ul>
                    <div class="indicator blue-grey" style="right: 853px; left: 0px;"></div>
                </div>
            </div>
            <div class="col s12">
                <div class="card-panel no-padding">
                    <!--  MAIN mailbox START-->
                    <div id="main-mailbox" style="display: block; direction: rtl;">
                        <table class="list bordered highlight">
                            <thead>
                                <tr>
                                    <th colspan="4">הודעות</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:ListView ID="ListViewAll" runat="server" DataKeyNames="ID" OnItemDeleting="ListViewAll_ItemDeleting">
                                    <ItemTemplate>
                                        <tr class="<%#CastVisi(Eval("Read")) %>">
                                            <td class="check-col">
                                                <input type="checkbox" id="checkbox<%#Container.DataItemIndex+1 %>a" class="filled-in">
                                                <label for="checkbox<%#Container.DataItemIndex+1 %>a"></label>
                                            </td>
                                            <td><a href="Read.aspx?mid=<%#Eval("ID") %>&mailbox=all" class="cell-row">
                                                <div class="cell">
                                                    <img src="<%#CastPicPath(Eval("SenderID")) %>" alt="" class="simple-avatar small circle right pink darken-1" style="margin-left: 20PX"><h6><%#CastSenderReciver(Eval("ReciverName"),Eval("SenderName"),Eval("SenderID")) %></h6>
                                                    <p class="test"><%#Eval("Subject") %></p>
                                                </div>
                                                <div class="left">
                                                </div>
                                            </a></td>
                                            <td class="cell-row">
                                                <asp:LinkButton ID="LinkButtonDeleteAll" runat="server" Style="float: left; margin-left: 20%;" CommandName="Delete" OnClientClick="return confirm('האם אתה בטוח שברצונך למחוק הודעה זו?')"><i class="material-icons" style="font-size:30px;top:50%">delete</i></asp:LinkButton>
                                                <span style="float: left; margin-left: 30%; margin-top: 1em;"><%#Eval("SentDate") %></span>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                                <asp:Panel ID="PanelEmptyAll" runat="server" Visible="false">
                                    <tr style="text-align: center">
                                        <td colspan="3" style="text-align: center">
                                            <h2>אין הודעות</h2>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </tbody>
                        </table>
                    </div>
                    <!--  MAIN mailbox END -->
                    <!--  SOCIAL mailbox START -->
                    <div id="in-mailbox" style="display: block; direction: rtl;">
                        <table class="list bordered highlight">
                            <thead>
                                <tr>
                                    <th colspan="2">הודעות</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:ListView ID="ListViewIn" DataKeyNames="ID" runat="server" OnItemDeleting="ListViewIn_ItemDeleting">
                                    <ItemTemplate>
                                        <tr class="<%#CastVisi(Eval("Read")) %>">
                                            <td class="check-col">
                                                <input type="checkbox" id="checkbox<%#Container.DataItemIndex+1 %>b" class="filled-in">
                                                <label for="checkbox<%#Container.DataItemIndex+1 %>b"></label>
                                            </td>
                                            <td><a href="Read.aspx?mid=<%#Eval("ID") %>&mailbox=in" class="cell-row">
                                                <div class="cell">
                                                    <img src="<%#CastPicPath(Eval("SenderID")) %>" alt="" class="simple-avatar small circle right pink darken-1" style="margin-left: 20PX"><h6><%#CastSenderReciver(Eval("ReciverName"),Eval("SenderName"),Eval("SenderID")) %></h6>
                                                    <p class="test"><%#Eval("Subject") %></p>
                                                </div>
                                                <div class="left">
                                                </div>
                                            </a></td>
                                            <td class="cell-row">
                                                <asp:LinkButton ID="LinkButtonDeleteAll" runat="server" Style="float: left; margin-left: 20%;" CommandName="Delete" OnClientClick="return confirm('האם אתה בטוח שברצונך למחוק הודעה זו?')"><i class="material-icons" style="font-size:30px;top:50%">delete</i></asp:LinkButton>
                                                <span style="float: left; margin-left: 30%; margin-top: 1em;"><%#Eval("SentDate") %></span>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                                <asp:Panel ID="PanelEmptyIn" runat="server" Visible="false">
                                    <tr style="text-align: center">
                                        <td colspan="3" style="text-align: center">
                                            <h2>אין הודעות</h2>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </tbody>
                        </table>
                    </div>
                    <!--  SOCIAL mailbox END -->
                    <!--  UPDATES mailbox START -->
                    <!--  SOCIAL mailbox START -->
                    <div id="out-mailbox" style="display: block; direction: rtl;">
                        <table class="list bordered highlight">
                            <thead>
                                <tr>
                                    <th colspan="2">הודעות</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:ListView ID="ListViewOut" DataKeyNames="ID" runat="server" OnItemDeleting="ListViewOut_ItemDeleting">
                                    <ItemTemplate>
                                        <tr class="<%#CastVisi(Eval("Read")) %>">
                                            <td class="check-col">
                                                <input type="checkbox" id="checkbox<%#Container.DataItemIndex+1 %>c" class="filled-in">
                                                <label for="checkbox<%#Container.DataItemIndex+1 %>c"></label>
                                            </td>
                                            <td><a href="Read.aspx?mid=<%#Eval("ID") %>&mailbox=out" class="cell-row">
                                                <div class="cell">
                                                    <img src="<%#CastPicPath(Eval("SenderID")) %>" alt="" class="simple-avatar small circle right pink darken-1" style="margin-left: 20PX"><h6 style="white-space: nowrap !important;"><%#CastSenderReciver(Eval("ReciverName"),Eval("SenderName"),Eval("SenderID")) %></h6>
                                                    <p class="test" style="white-space: nowrap !important;"><%#Eval("Subject") %></p>
                                                </div>
                                                <div class="left">
                                                </div>
                                            </a></td>
                                            <td class="cell-row">
                                                <asp:LinkButton ID="LinkButtonDeleteAll" runat="server" Style="float: left; margin-left: 20%;" CommandName="Delete" OnClientClick="return confirm('האם אתה בטוח שברצונך למחוק הודעה זו?')"><i class="material-icons" style="font-size:30px;top:50%">delete</i></asp:LinkButton>
                                                <span style="float: left; margin-left: 30%; margin-top: 1em;"><%#Eval("SentDate") %></span>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                                <asp:Panel ID="PanelEmptyOut" runat="server" Visible="false">
                                    <tr style="text-align: center">
                                        <td colspan="3" style="text-align: center">
                                            <h2>אין הודעות</h2>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </tbody>
                        </table>
                    </div>
                    <!--  UPDATES mailbox END -->
                </div>
            </div>
        </div>
    </div>
</asp:Content>


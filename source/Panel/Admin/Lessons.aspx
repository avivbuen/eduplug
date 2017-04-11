<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Lessons.aspx.cs" Inherits="Panel_Admin_Lessons" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%-- Add Exam Of Grade --%>

    <div style="direction: rtl;">
        <style>
            table, th, td {
                border: solid 1px !important;
            }
        </style>
       
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                 <h2><asp:Label ID="LabelName" runat="server" Text="Label"></asp:Label></h2>
                <ul class="collection with-header" style="direction: rtl; float: right; border: 0px; margin-top: 0px; width: 100%; max-width: 500px">
                    <li class="collection-header orange white-text" style="border-radius: 0px;">
                        <h4><i class="material-icons">access_time</i> שיעורים</h4>
                    </li>
                    <asp:ListView ID="ListViewLessons" runat="server" DataKeyNames="ID" OnItemDeleting="ListViewLessons_ItemDeleting">
                        <ItemTemplate>
                            <li class="collection-item">
                                <div>| שעה <%#Eval("Hour") %> ביום <%#CastDate(Eval("Day")) %><asp:LinkButton ID="LinkButtonDelete" CommandName="Delete" CssClass="secondary-content red-text" OnClientClick="return confirm('האם אתה בטוח שאתה רוצה למחוק את השיעור?');" runat="server"><i class="material-icons">delete</i></asp:LinkButton></div>
                            </li>
                        </ItemTemplate>
                    </asp:ListView>
                    <li class="collection-item orange white-text" style="height: 35px; padding-top: 5px">
                        <asp:Label ID="LabelLessonsEmpty" runat="server" Text="אין שיעורים"></asp:Label>
                        <asp:Label ID="LabelAVG" runat="server" Text=""></asp:Label>
                        <a id="addExamClick" onclick="$('#newLessonModal').openModal()" class="white-text" style="float: left; cursor: pointer;"><i class="material-icons">add</i></a>
                    </li>
                </ul>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="newLessonModal" class="modal newLessonModal" style="overflow: hidden; overflow-y: auto; max-width: 500px;">
            <div class="modal-content">
                <div dir="rtl" style="text-align: center;">
                    <asp:UpdatePanel ID="UpdatePanelAddExam" runat="server">
                        <ContentTemplate>
                            <h1><i class="material-icons" style="font-size: 72px;">assignment</i></h1>
                            <p>
                                דף זה נועד להוספת שיעור למקצוע המבוקש:
                            </p>
                            <div class="row" style="text-align: right;">
                                <div class="input-field">
                                    <asp:RequiredFieldValidator ID="rfv_Lesson_Day" runat="server" ControlToValidate="Lesson_Day" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="AllowValidationGroup" InitialValue="-1" ErrorMessage="בחר יום">*</asp:RequiredFieldValidator>
                                    <asp:DropDownList ID="Lesson_Day" runat="server" CssClass="browser-default" OnSelectedIndexChanged="Lesson_Day_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="בחר יום..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="ראשון" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="שני" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="שלישי" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="רביעי" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="חמישי" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="שישי" Value="6"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row" style="text-align: right;">
                                <div class="input-field">
                                    <asp:RequiredFieldValidator ID="rfv_Lesson_Hour" runat="server" ControlToValidate="Lesson_Hour" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="AllowValidationGroup" InitialValue="-1" ErrorMessage="בחר שעה">*</asp:RequiredFieldValidator>
                                    <asp:DropDownList ID="Lesson_Hour" runat="server" CssClass="browser-default" Visible="false">
                                        <asp:ListItem Text="בחר שעה..." Value="-1"></asp:ListItem>

                                    </asp:DropDownList>

                                </div>
                            </div>
                            <asp:LinkButton ID="CancelButton" CssClass="btn btn-floating waves-button-input btn-large modal-close red" OnClientClick="$('#newLessonModal').closeModal()" runat="server"><i class="material-icons">cancel</i></asp:LinkButton>
                            <asp:LinkButton ID="AddButtonExam" OnClientClick="$('#newLessonModal').closeModal()" CssClass="btn btn-floating green waves-button-input btn-large" runat="server" OnClick="AddButtonExam_Click"><i class="material-icons">add</i></asp:LinkButton>

                            <asp:ValidationSummary ID="UpadateValidationSummary" ForeColor="Red" Style="list-style-type: upper-roman;" ValidationGroup="AllowValidationGroup" runat="server" />
                            <span class="precentLeft red-text"></span>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>
        </div>

    <a href="Tgrades.aspx">חזור לכל הכיתות מקצועות</a>

</asp:Content>


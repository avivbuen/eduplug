<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ChangeTable.aspx.cs" Inherits="Panel_ChangeTable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../../Content/css/picker.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="pageContent" style="direction: rtl">

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <h2>
                    <asp:Label ID="LabelName" runat="server" Text="Label"></asp:Label></h2>
                <ul class="collection with-header" style="direction: rtl; float: right; border: 0px; margin-top: 0px; width: 100%; max-width: 500px">
                    <li class="collection-header teal white-text" style="border-radius: 0px;">
                        <h4><i class="material-icons">access_time</i> שינויי מערכת</h4>
                    </li>
                    <asp:ListView ID="ListViewChanges" runat="server" DataKeyNames="ID" OnItemDeleting="ListViewChanges_ItemDeleting">
                        <ItemTemplate>
                            <li class="collection-item">
                                <div>| שעה <%#CastHour(Eval("LessonID"))%> בתאריך <%#(Eval("Date","{0:dd/MM/yyyy}")) %> <%#GetLessonChangeType(Eval("ChangeType")) %><asp:LinkButton ID="LinkButtonDelete" CommandName="Delete" CssClass="secondary-content red-text" OnClientClick="return confirm('האם אתה בטוח שאתה רוצה למחוק את השינויי?');" runat="server"><i class="material-icons">delete</i></asp:LinkButton></div>
                            </li>
                        </ItemTemplate>
                    </asp:ListView>
                    <li class="collection-item teal white-text" style="height: 35px; padding-top: 5px">
                        <asp:Label ID="LabelLessonsEmpty" runat="server" Text="אין שינוים"></asp:Label>
                        <asp:Label ID="LabelAVG" runat="server" Text=""></asp:Label>
                        <a id="addChangeClick" onclick="$('#newChangeModal').openModal()" class="white-text" style="float: left; cursor: pointer;"><i class="material-icons">add</i></a>
                    </li>
                </ul>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="newChangeModal" class="modal newChangeModal" style="overflow: hidden; overflow-y: auto; max-width: 500px;">
            <div class="modal-content">
                <div dir="rtl" style="text-align: center;">
                    <asp:UpdatePanel ID="UpdatePanelAddChange" runat="server">
                        <ContentTemplate>
                            <h1><i class="material-icons" style="font-size: 72px;">assignment</i></h1>
                            <p>
                                דף זה נועד להוספת שיעור למקצוע המבוקש:
                            </p>
                            <div class="row" style="text-align: right;">
                                <div class="input-field" dir="rtl">
                                    <asp:TextBox ID="Change_Date" placeholder="תאריך שיעור" runat="server" CssClass="datepicker brown-text" OnTextChanged="Change_Date_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <label>
                                        <asp:RequiredFieldValidator ID="rfv_Change_Date" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="ChangeValidationGroup" ControlToValidate="Change_Date" runat="server" ErrorMessage="הכנס תאריך מבחן/מטלה">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="refv_Change_Date" Display="Dynamic" ValidationExpression="^(?:[012]?[0-9]|3[01])[./-](?:0?[1-9]|1[0-2])[./-](?:[0-9]{2}){1,2}$" EnableClientScript="true" ForeColor="Red" ValidationGroup="ChangeValidationGroup" ControlToValidate="Change_Date" runat="server" ErrorMessage="תאריך לא תקין">*</asp:RegularExpressionValidator>
                                    </label>
                                </div>
                            </div>
                            <div class="row" style="text-align: right;">
                                <div class="input-field">
                                    <asp:RequiredFieldValidator ID="rfv_Lesson_Hour" runat="server" ControlToValidate="Lesson_Hour" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="AllowValidationGroup" InitialValue="-1" ErrorMessage="בחר שעה">*</asp:RequiredFieldValidator>
                                    <asp:DropDownList ID="Lesson_Hour" runat="server" CssClass="browser-default" Visible="false" OnSelectedIndexChanged="Lesson_Hour_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="בחר שעה..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <asp:Panel ID="PanelDet" runat="server" Visible="false">
                                <div class="row" style="text-align: right;">
                                    <div class="input-field">
                                        <asp:RequiredFieldValidator ID="rfv_Change_Cause" runat="server" ControlToValidate="Change_Cause" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="AllowValidationGroup" InitialValue="-1" ErrorMessage="בחר סיבה">*</asp:RequiredFieldValidator>
                                        <asp:DropDownList ID="Change_Cause" runat="server" CssClass="browser-default">
                                            <asp:ListItem Text="בחר סיבה..." Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="ביטול" Value="c"></asp:ListItem>
                                            <asp:ListItem Text="מילויי מקום" Value="f"></asp:ListItem>
                                            <asp:ListItem Text="מבחן" Value="t"></asp:ListItem>
                                            <asp:ListItem Text="מבחן בגרות" Value="b"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="input-field col s12">
                                    <asp:TextBox ID="TextBoxDesc" placeholder=" אופציונלי - תיאור שיעור" runat="server" data-length="120" CssClass="avivnet-textarea brown-text"></asp:TextBox>
                                    
                                    
                                </div>
                            </asp:Panel>
                            <asp:LinkButton ID="CancelButton" CssClass="btn btn-floating waves-button-input btn-large modal-close red" OnClientClick="$('#newChangeModal').closeModal()" runat="server"><i class="material-icons">cancel</i></asp:LinkButton>
                            <asp:LinkButton ID="AddButtonChange" CssClass="btn btn-floating green waves-button-input btn-large" runat="server" OnClick="AddButtonChange_Click" OnClientClick="validate()"><i class="material-icons">add</i></asp:LinkButton>

                            <asp:ValidationSummary ID="UpadateValidationSummary" ForeColor="Red" Style="list-style-type: upper-roman;" ValidationGroup="AllowValidationGroup" runat="server" />
                            <span class="precentLeft red-text"></span>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <script>
                        function validate()
                        {
                            if (Page_ClientValidate())
                                $('#newChangeModal').closeModal();
                        }
                        function picker() {
                            $('.datepicker').pickadate({
                                selectMonths: true, // Creates a dropdown to control month
                                selectYears: 3, // Creates a dropdown of 150 years to control year
                                container: '#pageContent'

                            });
                            setTimeout(picker, 200);
                        }
                        picker();
                    </script>
                </div>
            </div>
        </div>
    </div>
    <a href="Tgrades.aspx">חזור לכל הכיתות מקצועות</a>
</asp:Content>


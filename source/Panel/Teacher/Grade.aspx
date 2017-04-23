<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Grade.aspx.cs" Inherits="Panel_Teacher_Grade" %>
<%@ Import Namespace="Business_Logic.Grades" %>
<%@ Import Namespace="Business_Logic.TeacherGrades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="pageContent" dir="ltr">
        <div style="text-align: center;">
            <asp:Label ID="LabelTitle" runat="server" Text="Title" Font-Size="XX-Large"></asp:Label><br />
            <br />
        </div>
        <asp:UpdatePanel ID="UpdatePanelData" runat="server">
            <ContentTemplate>
                <ul class="collection with-header" style="direction: rtl; float: right; border: 0px; margin: 0px 0px 5px 5px; width: 100%; max-width: 500px">
                    <li class="collection-header cyan white-text" style="border-radius: 0px;">
                        <h4>תלמידים:</h4>
                    </li>
                    <asp:ListView ID="ListViewStudents" runat="server">
                        <ItemTemplate>
                            <li class="collection-item">
                                <div><%#Eval("Name") %><a href="Student.aspx?sid=<%#Eval("UserID") %>&tgid=<%=((TeacherGrade)Session["tgCur"]).Id %>" class="secondary-content cyan-text" style="float: left"><i class="material-icons">arrow_back</i></a></div>
                            </li>
                        </ItemTemplate>
                    </asp:ListView>
                    <li class="collection-item cyan white-text" style="height: 35px; padding-top: 5px">
                        <asp:Label ID="LabelStudentsEmpty" runat="server" Text="אין תלמידים"></asp:Label>
                        
                    </li>
                </ul>
                <ul class="collection with-header" style="direction: rtl; float: right; border: 0px; margin-top: 0px; width: 100%; max-width: 500px">
                    <li class="collection-header orange white-text" style="border-radius: 0px;">
                        <h4>מבחנים:</h4>
                    </li>
                    <asp:ListView ID="ListViewExams" runat="server" DataKeyNames="ID" OnItemDeleting="ListViewExams_ItemDeleting">
                        <ItemTemplate>
                            <li class="collection-item">
                                <div>| <%#Eval("Title") %><a href="Exam.aspx?eid=<%#Eval("ID") %>" class="secondary-content orange-text" style="float: left"><i class="material-icons">arrow_back</i></a><asp:LinkButton ID="LinkButtonDelete" CommandName="Delete" CssClass="secondary-content red-text" OnClientClick="return confirm('האם אתה בטוח שאתה רוצה למחוק את המבחן?');" runat="server"><i class="material-icons">delete</i></asp:LinkButton></div>
                            </li>
                        </ItemTemplate>
                    </asp:ListView>
                    <li class="collection-item orange white-text" style="height: 35px; padding-top: 5px">
                        <asp:Label ID="LabelExamsEmpty" runat="server" Text="אין מבחנים"></asp:Label>
                        <asp:Label ID="LabelAVG" runat="server" Text=""></asp:Label>
                        <a id="addExamClick" onclick="$('#examModal').openModal()" class="white-text" style="float: left; cursor: pointer;"><i class="material-icons">add</i></a>
                        <a id="stateClick" class="white-text" style="float: left; cursor: pointer;" href="GradeState.aspx?gid=<%=((TeacherGrade)Session["tgCur"]).Id %>"><i class="material-icons">trending_up</i></a>
                    </li>
                </ul>
            </ContentTemplate>
        </asp:UpdatePanel>

        <%-- Add Exam Of Grade --%>
        <asp:UpdatePanel ID="UpdatePanelAddExam" runat="server">
            <ContentTemplate>
                <div id="examModal" class="modal examModal" style="overflow: hidden; overflow-y: auto; max-width: 500px;">
                    <div class="modal-content">
                        <div dir="rtl" style="text-align: center;">
                            <h1><i class="material-icons" style="font-size: 72px;">assignment</i></h1>
                            <p>
                                דף זה נועד להוספת מבחן לכיתה המבוקשת:
                            </p>
                            <div class="row" style="text-align: right;">
                                <div class="input-field" dir="rtl">
                                    <asp:TextBox ID="Exam_Name" runat="server"></asp:TextBox>
                                    <label>
                                        <asp:RequiredFieldValidator ID="rfv_Exam_Name" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="ExamValidationGroup" ControlToValidate="Exam_Name" runat="server" ErrorMessage="הכנס שם מבחן/מטלה">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="refv_Exam_Name" Display="Dynamic" ValidationExpression="^([\sא-ת]{2,50})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="ExamValidationGroup" ControlToValidate="Exam_Name" runat="server" ErrorMessage="שם מבחן/מטלה חייב להיות בעברית בלבד בין 2-50 תווים">*</asp:RegularExpressionValidator>
                                        שם מבחן/מטלה</label>
                                </div>
                                <div class="input-field" dir="rtl">
                                    <asp:TextBox ID="Exam_Date" runat="server" CssClass="datepicker"></asp:TextBox>
                                    <label>
                                        <asp:RequiredFieldValidator ID="rfv_Exam_Date" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="ExamValidationGroup" ControlToValidate="Exam_Date" runat="server" ErrorMessage="הכנס תאריך מבחן/מטלה">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="refv_Exam_Date" Display="Dynamic" ValidationExpression="^(?:[012]?[0-9]|3[01])[./-](?:0?[1-9]|1[0-2])[./-](?:[0-9]{2}){1,2}$" EnableClientScript="true" ForeColor="Red" ValidationGroup="ExamValidationGroup" ControlToValidate="Exam_Date" runat="server" ErrorMessage="תאריך לא תקין">*</asp:RegularExpressionValidator>
                                        תאריך המטלה/המבחן
                                    </label>
                                </div>
                                <div class="input-field" dir="rtl">
                                    <asp:TextBox ID="Exam_Precent" runat="server" MaxLength="3"></asp:TextBox>
                                    <label>
                                        <asp:RequiredFieldValidator ID="rfv_Exam_Precent" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="ExamValidationGroup" ControlToValidate="Exam_Precent" runat="server" ErrorMessage="הכנס אחוז ציון המבחן/מטלה">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="refv_Exam_Precent" Display="Dynamic" ValidationExpression="^[0-9]+$" EnableClientScript="true" ForeColor="Red" ValidationGroup="ExamValidationGroup" ControlToValidate="Exam_Precent" runat="server" ErrorMessage="מספר בלבד">*</asp:RegularExpressionValidator>
                                        <asp:CustomValidator ID="cv_Exam_Precent" runat="server" Display="Dynamic" EnableClientScript="true" ForeColor="Red" ValidationGroup="ExamValidationGroup" MinimumValue="0" MaximumValue="50" ControlToValidate="Exam_Precent" ErrorMessage="האחוז חייב להיות קטן או שווה לסך האחוזים שנותרו" ClientValidationFunction="validateLength">*</asp:CustomValidator>
                                        אחוז ציון</label>
                                </div>
                            </div>
                            <asp:LinkButton ID="CancelButton" CssClass="btn btn-floating waves-button-input btn-large modal-close red" runat="server" OnClick="CancelButton_Click"><i class="material-icons">cancel</i></asp:LinkButton>
                            <asp:LinkButton ID="AddButtonExam" OnClientClick="return ExamValidate()" CssClass="btn btn-floating green waves-button-input btn-large" OnClick="AddButtonExam_Click" runat="server"><i class="material-icons">add</i></asp:LinkButton>

                            <asp:ValidationSummary ID="UpadateValidationSummary" ForeColor="Red" Style="list-style-type: upper-roman;" ValidationGroup="ExamValidationGroup" runat="server" />
                            <span class="precentLeft red-text"></span>
                        </div>
                    </div>
                </div>
                <script>
                    function validateLength(oSrc, args) {
                        if ((args.Value.length > 3)){
                            args.IsValid = false;
                            return false;
                        }
                        var txtDate= $("#<%=Exam_Date.ClientID%>").val();
                        if(txtDate=="") {
                            return;
                        }
                        var from = txtDate.split("/");
                        var maxPrc;
                        $.ajax({//Calling the server in order to validate
                            url: "../../../System/Precent.ashx",
                            data: { tgid: <%=((TeacherGrade)Session["tgCur"]).Id %>, day:from[0], month:from[1], year:from[2] },
                            type: "POST",
                            async: false,
                            cache: false,
                            success: function (data) {//Got response
                                maxPrc=data;
                            },
                            error: function (reponse) {//Did not get 
                                maxPrc=0;
                            }
                        });
                        if(args.Value<=maxPrc&&args.Value>=0)
                        {
                            args.IsValid = true;
                            return true;
                        }
                        
                        if(maxPrc==0)
                        {
                            $(".precentLeft").html('לא נותר אחוזי ציון');
                        }
                        else {
                            $(".precentLeft").html('האחוזים שנותרו '+maxPrc+'%');
                        }
                        
                        
                    }
                    function picker()
                    {
                        $('.datepicker').pickadate({
                            selectMonths: true, // Creates a dropdown to control month
                            selectYears: 3, // Creates a dropdown of 150 years to control year
                            container: '#pageContent',
                            min: new Date(<%= EduSysDate.GetStart().Year %>,<%= EduSysDate.GetStart().Month %>-1,<%= EduSysDate.GetStart().Day %>),
                            max: new Date(<%= EduSysDate.GetEnd().Year %>,<%= EduSysDate.GetEnd().Month %>-1,<%= EduSysDate.GetEnd().Day %>)
                        });
                        setTimeout(picker,200);
                    }
                    picker();

                    function ExamValidate() {
                        if (Page_ClientValidate("ExamValidationGroup")) {
                            $('#examModal').closeModal();
                            return true;
                        }
                        return false;
                    }
                    
                </script>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%-- END --%>
    </div>
    <style>
        input[type=submit] {
            background: none !important;
            border: none;
            padding: 0 !important;
            /*optional*/
            cursor: pointer;
        }
    </style>
</asp:Content>


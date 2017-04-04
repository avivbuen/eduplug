<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Exam.aspx.cs" Inherits="Panel_Teacher_Exam" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelData" runat="server">
        <ContentTemplate>
            <div dir="rtl" style="text-align: center; margin: 0 auto;">
                <div style="max-width: 500px; margin: 0 auto; text-align: center; background: #ffd800; border-radius: 4px 4px 0 0; font-family: sans-serif; font-size: 30px" class="white-text">
                    <div class="input-field">
                        <asp:TextBox ID="Exam_Name" runat="server" Style="text-align: center; font-size: 48px" placeholder="שם"></asp:TextBox>
                        <label>
                            <asp:RequiredFieldValidator ID="rfv_Exam_Name" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="ExamValidationGroup" ControlToValidate="Exam_Name" runat="server" ErrorMessage="הכנס שם מבחן/מטלה">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="refv_Exam_Name" Display="Dynamic" ValidationExpression="^([\sא-ת]{2,50})$" EnableClientScript="true" ForeColor="Red" ValidationGroup="ExamValidationGroup" ControlToValidate="Exam_Name" runat="server" ErrorMessage="שם מבחן/מטלה חייב להיות בעברית בלבד בין 2-50 תווים">*</asp:RegularExpressionValidator>
                        </label>
                    </div>
                    <div class="input-field" style="margin: 0 0 0 0">
                        <asp:TextBox ID="Exam_Date" runat="server" CssClass="datepicker" Style="text-align: center; border: 0px; margin: 0 0 0 0; font-size: 25px" placeholder="תאריך"></asp:TextBox>
                        <label>
                            <asp:RequiredFieldValidator ID="rfv_Exam_Date" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="ExamValidationGroup" ControlToValidate="Exam_Date" runat="server" ErrorMessage="הכנס תאריך מבחן/מטלה">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="refv_Exam_Date" Display="Dynamic" ValidationExpression="^(?:[012]?[0-9]|3[01])[./-](?:0?[1-9]|1[0-2])[./-](?:[0-9]{2}){1,2}$" EnableClientScript="true" ForeColor="Red" ValidationGroup="ExamValidationGroup" ControlToValidate="Exam_Date" runat="server" ErrorMessage="תאריך לא תקין">*</asp:RegularExpressionValidator>

                        </label>
                    </div>
                    <div style="font-size: 20px">אחוז</div>
                    <div class="input-field" style="margin: 0px">
                        <asp:TextBox ID="Exam_Precent" runat="server" MaxLength="3" Style="text-align: center; border: 0px; font-size: 25px" placeholder="אחוז מבחן/מטלה"></asp:TextBox>
                        <label>
                            <asp:RequiredFieldValidator ID="rfv_Exam_Precent" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="ExamValidationGroup" ControlToValidate="Exam_Precent" runat="server" ErrorMessage="הכנס אחוז ציון המבחן/מטלה">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="refv_Exam_Precent" Display="Dynamic" ValidationExpression="^[0-9]+$" EnableClientScript="true" ForeColor="Red" ValidationGroup="ExamValidationGroup" ControlToValidate="Exam_Precent" runat="server" ErrorMessage="מספר בלבד">*</asp:RegularExpressionValidator>
                            <asp:CustomValidator ID="cv_Exam_Precent" OnServerValidate="cv_Exam_Precent_ServerValidate" runat="server" Display="Dynamic" EnableClientScript="true" ForeColor="Red" ValidationGroup="ExamValidationGroup" MinimumValue="0" MaximumValue="50" ControlToValidate="Exam_Precent" ErrorMessage="האחוז חייב להיות קטן או שווה לסך האחוזים שנותרו">*</asp:CustomValidator>
                        </label>
                    </div>
                    ציונים
                </div>
                <div style="max-width: 500px; margin: 0 auto; text-align: center; overflow-y: scroll; background: #fff;">
                    <asp:DataList ID="DataListScores" RepeatLayout="Table" runat="server" DataKeyField="UserID" RepeatColumns="3" OnItemDataBound="DataListScores_ItemDataBound">
                        <ItemTemplate>
                            <%#Eval("Name") %>:
                            <asp:TextBox ID="TextBoxScoreVal" runat="server" MaxLength="3" CssClass="browser-default" Style="background-color: #fff; width: auto; margin: 0 0 0 0; height: 1.5rem; text-align: center; width: 40px;"></asp:TextBox>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
                <div style="max-width: 500px; margin: 0 auto; text-align: center; background: #fff;">
                    <asp:Button ID="SaveButton" runat="server" Text="שמור" OnClick="SaveButton_Click" OnClientClick="return ExamValidate()" Style="background: #ffd800; margin-bottom: 10px;" CssClass="btn" />
                    <asp:ValidationSummary ID="UpadateValidationSummary" ForeColor="Red" Style="list-style-type: upper-roman;" ValidationGroup="ExamValidationGroup" runat="server" />
                    <br />
                    <asp:Label ID="LabelLeft" runat="server" Text="" ForeColor="Green"></asp:Label>
                    <br />
                    <asp:Label ID="LabelSaved" runat="server" Text="" ForeColor="Green"></asp:Label>
                </div>


            </div>
            <script><%=script%></script>
            <script>

                function picker() {
                    $('.datepicker').pickadate({
                        selectMonths: true, // Creates a dropdown to control month
                        selectYears: 3, // Creates a dropdown of 150 years to control year
                        min: new Date(<%= EduSysDate.GetStart().Year %>,<%= EduSysDate.GetStart().Month %>-1,<%= EduSysDate.GetStart().Day %>),
                        max: new Date(<%= EduSysDate.GetEnd().Year %>,<%= EduSysDate.GetEnd().Month %>-1,<%= EduSysDate.GetEnd().Day %>)
                    });
                    setTimeout(picker, 200);
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
</asp:Content>


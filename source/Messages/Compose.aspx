<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Compose.aspx.cs" Inherits="Messages_Compose" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div dir="rtl" style="max-width: 800px; text-align: center; margin: 0 auto; box-shadow: -1px 1px 5px 0px; background: #fff; padding: 30px;">
        <h1>הודעה חדשה</h1>

        <div class="input-field col s6">
            <asp:TextBox ID="Message_Subject" runat="server"></asp:TextBox>
            <label>
                <asp:RequiredFieldValidator ID="rfv_Message_Subject" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="ComposeValidationGroup" ControlToValidate="Message_Subject" runat="server" ErrorMessage="הכנס שם נושא הודעה">*</asp:RequiredFieldValidator>
                נושא</label>
        </div>
        <div id="recipients">
            <asp:Panel ID="PanelSelectorAdmin" runat="server" Visible="false">
                <input name="CheckAll" id="check" type="checkbox" onclick="ToggleAll()" />
                <label for="check">כולם</label>
                <asp:CustomValidator ID="cv_UsersToSend" runat="server" ErrorMessage="אנא בחר מקבל הודעה" ForeColor="Red" ValidationGroup="ComposeValidationGroup" OnServerValidate="cv_UsersToSend_ServerValidate">*</asp:CustomValidator>
                <div style="overflow-y: scroll; height: 100px; width: 100%; padding-right: 25px">
                    <asp:CheckBoxList ID="UsersToSend" RepeatColumns="4" runat="server" CssClass="e-form"></asp:CheckBoxList>
                </div>
            </asp:Panel>
            <asp:Panel ID="PanelSelectorAll" runat="server"  Visible="false">
               <span style="width:10%">מקבל:</span>
                <asp:DropDownList ID="DropDownUsers" runat="server" CssClass="browser-default" style="width:90%; display:inline"></asp:DropDownList>
            </asp:Panel>
        </div>
        <div class="input-field col s6" dir="rtl">
            <asp:RequiredFieldValidator ID="refv_TextBoxMessageContent" EnableClientScript="true" Display="Dynamic" ForeColor="Red" ValidationGroup="ComposeValidationGroup" ControlToValidate="TextBoxMessageContent" runat="server" ErrorMessage="הכנס תוכן הודעה">*</asp:RequiredFieldValidator>
            <asp:TextBox ID="TextBoxMessageContent" TextMode="MultiLine" Height="300" placeholder="תוכן ההודעה" runat="server"></asp:TextBox>
        </div>
        <br />
        <button id="SendBtn" runat="server" class="btn" onserverclick="SendButton_Click">
            <i class="material-icons">send</i>
        </button>
    </div>
    <script>
        var checked = false;
        function ToggleAll() {
            if (checked)
                $('.e-form input:checkbox').prop('checked', false);
            else
                $('.e-form input:checkbox').prop('checked', true);
            checked = !checked;
        }
    </script>
    <div id="success-modal" class="modal" style="max-width: 500px;">
        <div class="modal-content center-block center-align center centered" dir="rtl">
            ההודעה נשלחה!
            <br />
            <br />
            <br />
            <a href="~/Messages" runat="server" style="margin: 0 auto" class="modal-action modal-close waves-effect waves-green btn green center-align center">אישור</a>
        </div>
    </div>
    <script><%= done %></script>
</asp:Content>

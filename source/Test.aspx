<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    <asp:GridView ID="GridView1" runat="server"></asp:GridView>
    <style>
        .select-dropdown li.disabled, .select-dropdown li.disabled > span, .select-dropdown li.optgroup {
            color: rgba(0,0,0,0.3);
            background-color: transparent;
        }

        .dropdown-content li > span {
            font-size: 16px;
            color: #26a69a;
            display: block;
            line-height: 22px;
            padding: 14px 16px;
        }
    </style>
    <div class="input-field">
        <select multiple>
            <option value="" disabled selected>Choose your option</option>
            <option value="1">Option 1</option>
            <option value="2">Option 2</option>
            <option value="3">Option 3</option>
        </select>
        <label>Materialize Multiple Select</label>
    </div>

    <script>
 $('select').material_select();
    </script>
</asp:Content>


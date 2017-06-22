<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DelayedRecipientListUI.ascx.cs" Inherits="DelayedRecipientList.DelayedRecipientListUI" %>
<table>
    <tr><td class="NormalBold">Select Zipcode:</td><td><asp:DropDownList ID="ddlZipCodes" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlZipCodes_SelectedIndexChanged"></asp:DropDownList></td></tr>
    <tr runat="server" id="trNumberToShow">
    <td class="NormalBold">Number to show:</td><td><asp:TextBox ID="txtNumberToShow" Width="40" runat="server"></asp:TextBox> of <asp:Label ID="lblNumberAvailable" runat="server" Text="Label"></asp:Label> recipients.</td></tr>
</table>
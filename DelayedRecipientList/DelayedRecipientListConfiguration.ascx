<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DelayedRecipientListConfiguration.ascx.cs" Inherits="DelayedRecipientList.DelayedRecipientListConfiguration" %>

<table class="SubSectionTable">
<tr>
    <td>List API Url:</td>
    <td><asp:TextBox ID="txtAPIUrl" runat="server"></asp:TextBox></td>    
</tr>
<tr>
    <td>Price Per Piece:</td>
    <td><asp:TextBox ID="txtPricePerPiece" runat="server"></asp:TextBox></td>    
</tr>    
<tr>
    <td>Mail Specs Data:</td>
    <td><asp:TextBox ID="txtMailPieceJSON" runat="server" Style="display:none" TextMode="MultiLine"></asp:TextBox></td>    
</tr>
</table>


<script src="https://cdnjs.cloudflare.com/ajax/libs/jsoneditor/5.6.0/jsoneditor.min.js"></script>
<link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/jsoneditor/5.6.0/jsoneditor.min.css" />

<div id="jsoneditor" style="width: 400px; height: 400px;"></div>

<script>
    $('#jsoneditor').css('width','100%');

    // create the editor
    var container = document.getElementById("jsoneditor");
    var options = {mode:'code'};
    var editor = new JSONEditor(container, options);

    // set json
    var json = document.getElementById('<%=txtMailPieceJSON.ClientID%>').innerHTML;

    editor.set(JSON.parse(json));

	// Update the json when clicking Save
	var btnSave = $("[id*=LinkButtonBack]");

	// Update the json
    btnSave.click(function (save) {
	    document.getElementById('<%=txtMailPieceJSON.ClientID%>').innerHTML = JSON.stringify(editor.get());
	    return true;	
    });
</script>
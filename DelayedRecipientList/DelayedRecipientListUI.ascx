<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DelayedRecipientListUI.ascx.cs" Inherits="DelayedRecipientList.DelayedRecipientListUI" %>

<script>
	
    /**
     * Update list params, ie: json data
     */
    var UpdateListParams = function()
    {
        var iframe  = document.getElementById('iframeListSelector');
        var json    = document.getElementById('<%=txtMailPieceJSON.ClientID%>').innerHTML;
        var itemId  = document.getElementById('<%=txtItemOrderProductId.ClientID%>').value;

        var data = {
            externalTrackingType: "OrderProductId",
            externalTrackingId: itemId,
            jsonData: json,
        };
    
        iframe.contentWindow.postMessage(JSON.stringify(data), iframe.src);
    };

	$(function(){
	    var url     = document.getElementById('<%=txtApiUrl.ClientID%>').value;
        var iframe  = document.getElementById('iframeListSelector');
	    var itemId  = document.getElementById('<%=txtItemOrderProductId.ClientID%>').value;
	
        iframe.src  = url + '?source=uStore&ExternalTrackingType=OrderProductId&ExternalTrackingId=' + itemId;
	});
</script>

<asp:TextBox id="txtMailPieceJSON" TextMode="MultiLine" runat="server" Style="display:none"></asp:TextBox>
<asp:HiddenField id="txtItemOrderProductId" runat="server"></asp:HiddenField>
<asp:HiddenField id="txtApiUrl" runat="server"></asp:HiddenField>

<iframe ID="iframeListSelector" frameborder="0" width="100%" height="500" src="" onLoad="UpdateListParams()"></iframe>



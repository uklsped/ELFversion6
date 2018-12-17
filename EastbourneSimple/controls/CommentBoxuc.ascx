<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CommentBoxuc.ascx.vb" Inherits="controls_CommentBoxuc" %>
<script type="text/javascript">
    function Count() {
                 var i = document.getElementById("CommentBox").value.length;
        if (i < 251) {
            document.getElementById("CommentWordCount").innerHTML = 250 - i;
            
        }
        else 
            document.getElementById("CommentWordCount").innerHTML = "You have exceeded the max text please delete some characters";
           
        
        return;
    }
    
</script>
<asp:TextBox ID="CommentBox" runat="server" TextMode="Multiline"  onkeyup="Count()" ClientIDMode="Static"  ViewStateMode="Disabled"></asp:TextBox>
                       <br/>
<asp:Literal ID="Literal1" runat="server">Characters Remaining: </asp:Literal>

<asp:Label ID="CommentWordCount" runat="server" Text="" ClientIDMode="Static"></asp:Label>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CommentBoxuc.ascx.vb" Inherits="controls_CommentBoxuc" %>
<script type="text/javascript">
  <%-- function Count() {
        alert("What the fuck")
        var txt = document.getElementById('<%=TextBox.ClientID%>');
                 var i = txt.value.length;
        if (i < 251) {
            document.getElementById('<%=CommentWordCount.ClientID%>').innerHTML = 250 - i;
            
        }
        else 
            document.getElementById('<%=CommentWordCount.ClientID%>').innerHTML = "You have exceeded the max text please delete some characters";
           
        
        return;
    }--%>
   

</script>

<asp:TextBox ID="TextBox" runat="server" TextMode="Multiline"    ViewStateMode="Disabled"></asp:TextBox>
<asp:RequiredFieldValidator ID="CommentValidation" ControlToValidate="TextBox"  runat="server" Display="Dynamic"></asp:RequiredFieldValidator>
                       <br/>
<asp:Literal ID="Literal1" runat="server">Characters Remaining: </asp:Literal>

<asp:Label ID="CommentWordCount" runat="server" Text="" ></asp:Label>






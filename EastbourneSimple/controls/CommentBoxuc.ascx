<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CommentBoxuc.ascx.vb" Inherits="controls_CommentBoxuc" %>

<asp:TextBox ID="TextBox" runat="server" TextMode="Multiline" style="overflow:auto;" ReadOnly="false" ViewStateMode="Disabled"></asp:TextBox>
<asp:RequiredFieldValidator ID="CommentValidation" ControlToValidate="TextBox"  runat="server" Display="Dynamic"></asp:RequiredFieldValidator>
                       <br/>
<asp:Literal ID="Literal1" runat="server">Characters Remaining: </asp:Literal>

<asp:Label ID="CommentWordCount" runat="server" Text="" ></asp:Label>






<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FaultTrackinguc.ascx.vb" Inherits="controls_FaultTrackinguc" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register src="../WriteDatauc.ascx" tagname="WriteDatauc" tagprefix="uc1" %>
<%@ Register src="CommentBoxuc.ascx" tagname="CommentBoxuc" tagprefix="uc2" %>

 <%--<%@ Register src="OriginalReportedfaultuc.ascx" tagname="OriginalReportedfaultuc" tagprefix="uc3" %>--%>

 <%--<%@ Register src="DeviceReportedfaultuc.ascx" tagname="DeviceReportedfaultuc" tagprefix="uc3" %>--%>


  <%@ Register src="ConcessionHistoryuc.ascx" tagname="ConcessionHistoryuc" tagprefix="uc4" %>


  <%@ Register src="../LockElfuc.ascx" tagname="LockElfuc" tagprefix="uc5" %>


  <%@ Register src="../ManyFaultGriduc.ascx" tagname="ManyFaultGriduc" tagprefix="uc6" %>


  <asp:Panel ID="Panel2" runat="server" BorderColor="#33CC33" 
    BorderStyle="Solid" style="margin-bottom: 5px" Width="960px">
                                 <uc1:writedatauc ID="WriteDatauc3" LinacName="" UserReason="4" Tabby="incident" WriteName="incident" Visible="false" runat="server" />
      <uc5:LockElfuc ID="LockElfuc1" LinacName="" UserReason="5" Tabby="99" visible="false" runat="server" />

   <asp:Table ID="Table1" runat="server" Width="682px">
     <asp:TableRow>
       <asp:TableCell>
                                           
        <table style="width: 100%;">
          <tr>
                         
         <td colspan="3">
             <asp:Label ID="ConcessiondescriptionLabel" runat="server" Text="Concession Description"></asp:Label><br />
             <asp:Panel ID="CDescriptionPanel" enabled=false runat="server">
                <asp:UpdatePanel ID="DescriptionUpdatePanel" runat="server"  UpdateMode="Conditional">
                    <ContentTemplate>   
                        <uc2:CommentBoxuc ID="ConcessiondescriptionBoxC" MaxCount="50" TextHeight="30" runat="server" />
                     </ContentTemplate>
                </asp:UpdatePanel>  
             </asp:Panel>
         </td>
                                                       
         <td>
                                                                <asp:Label ID="StatusLabel" runat="server" Text="Current Status:"></asp:Label><br />
                                                                <asp:Label ID="StatusLabel1" runat="server" Text=""></asp:Label>
                                                           </td>


                                                        <td>
                                                       <asp:Label ID="ProblemStatusLabel" runat="server" Text="New Status"></asp:Label><br />
                                                        <asp:DropDownList ID="FaultOptionList" AutoPostBack="True" DataValueField="Value" runat="server">
                                                        <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                                        <%--<asp:ListItem Text="Open" Value="Open"></asp:ListItem>--%>
                                                        <asp:ListItem Text="Concession" Value="Concession"></asp:ListItem>
                                                        <asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
                                                        </asp:DropDownList>
                                                            </td>

                                                        <td>
                                                            <asp:Label ID="AssignedLabel" runat="server" Text="Assigned To"></asp:Label><br />
                                                        <asp:DropDownList ID="AssignedToList" runat="server">
                                                        <asp:ListItem>Unassigned</asp:ListItem>
                                                        <asp:ListItem>Engineering</asp:ListItem>
                                                        <asp:ListItem>Physics</asp:ListItem>
                                                        <asp:ListItem>Software</asp:ListItem>
                                                        </asp:DropDownList>
                                                        </td>
                                                       
                                                        </tr>
          <tr>

        <td colspan="3">
                                                            <asp:Label ID="ActionLabel" runat="server" Text="Concession Action"></asp:Label><br />
                                                            <asp:Panel ID="CActionPanel" Enabled="false" runat="server">
                                                                <asp:UpdatePanel ID="ConcessionActionUpdatePanel" Updatemode="Conditional" runat="server"><ContentTemplate> 
                                                            <uc2:CommentBoxuc ID="ConcessionActionBox" TextHeight="60" runat="server" />
                                                               <asp:Button ID="ClearActionButton" runat="server" Text="Clear" CausesValidation="False" CssClass="buttonmargin" />
                                                                         </ContentTemplate></asp:UpdatePanel>                                                         
                                                                </asp:Panel>
                                                        </td>

                                                        <td colspan="3">
                                                            <asp:Label ID="commentLabel" runat="server" Text="Comment"></asp:Label><br />
                                                            <asp:Panel ID="CCommentPanel" Enabled="false" runat="server" >
                                                        <asp:UpdatePanel ID="CCommentUpdatePanel" runat="server" UpdateMode="Conditional"><ContentTemplate>
                                                            <uc2:CommentBoxuc ID="ConcessionCommentBox" TextHeight="60" runat="server" />
                                                                <asp:Button ID="ClearCommentButton" runat="server" Text="Clear" CausesValidation="False" CssClass="buttonmargin" />
                                                               </ContentTemplate>
                                                               
                                                                </asp:UpdatePanel>
                                                            </asp:Panel>

                                                        </td>
                                                        </tr>
          <tr>
                                                        <td>
                                                        <asp:Button ID="SaveAFault" runat="server" Text="Save" Enabled="false" CausesValidation="false" />
                                                        </td>
                                                        <td>
                                                        <asp:Button ID="CancelButton" runat="server" Text="Cancel/Close" CausesValidation="false" />
                                                        </td>
              <td>
                  <asp:Button ID="LogElf" runat="server" Text="Lock Elf/Switch User" CausesValidation="false" />
              </td>
                                                        </tr>                     
        </table>
       </asp:TableCell>

     </asp:TableRow>
       
     <asp:TableRow>
                                            <asp:TableCell>
  

  <asp:Panel ID="Panel6" runat="server" BorderStyle="Solid" BorderColor="Yellow" Width="945px">
      <uc4:ConcessionHistoryuc ID="ConcessionHistoryuc1" runat="server" />

  </asp:Panel>
                                            </asp:TableCell></asp:TableRow>
   </asp:Table>
                                 
  </asp:Panel>

















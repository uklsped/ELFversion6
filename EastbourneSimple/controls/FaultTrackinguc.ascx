<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FaultTrackinguc.ascx.vb" Inherits="controls_FaultTrackinguc" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register src="../WriteDatauc.ascx" tagname="WriteDatauc" tagprefix="uc1" %>
<%@ Register src="CommentBoxuc.ascx" tagname="CommentBoxuc" tagprefix="uc2" %>

 <%@ Register src="DeviceReportedfaultuc.ascx" tagname="DeviceReportedfaultuc" tagprefix="uc3" %>

<uc1:writedatauc ID="WriteDatauc3" LinacName="" UserReason="4" Tabby="incident" WriteName="incident" Visible="false" runat="server" />
                             <asp:Panel ID="Panel2" runat="server" BorderColor="#33CC33" BorderStyle="Solid">
                                 <asp:Table ID="Table2" runat="server">
                                    <asp:TableRow>
                                       <asp:TableCell>
                                           <asp:Label ID="Label3" runat="server" Text="Incident ID"></asp:Label>
                                       </asp:TableCell>
                                       <asp:TableCell>
                                           <asp:Label ID="IncidentNumber" runat="server" Text=""></asp:Label>
                                       </asp:TableCell>
                                       <asp:TableCell>
                                       </asp:TableCell>
                                       <asp:TableCell>
                                       </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                                <asp:Table ID="Table1" runat="server" Width="1500px">
                                   <asp:TableRow>
                                       <asp:TableCell>
                                           <fieldset style="width: 1000px;">
                                               <legend>Details</legend>
                                                    <table style="width: 250px;">
                                                        <tr>
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate></ContentTemplate></asp:UpdatePanel>
                                                            <td>
                                                                <asp:Label ID="StatusLabel" runat="server" Text="Current Status:"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="StatusLabel1" runat="server" Text=""></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                        <td>
                                                        <asp:Label ID="ProblemStatusLabel" runat="server" Text="New Status"></asp:Label>
                                                        </td>
                                                        <td>
                                                        <%--<asp:UpdatePanel ID="UpdatePanelFaultOptionlist" runat="server" UpdateMode="Conditional"><ContentTemplate>--%>
                                                        <asp:DropDownList ID="FaultOptionList" AutoPostBack="true" DataValueField="Value" runat="server">
                                                        <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                                        <asp:ListItem Text="Open" Value="Open"></asp:ListItem>
                                                        <asp:ListItem Text="Concession" Value="Concession"></asp:ListItem>
                                                        <asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
                                                        </asp:DropDownList>
                                                           <%-- </ContentTemplate>
                                                        </asp:UpdatePanel>--%>
                                                        <td>
                                                        <asp:Label ID="ConcessionLabel" runat="server" Text="Concession Number"></asp:Label>
                                                        </td>
                                                        <td>
                                                        <asp:TextBox ID="ConcessionNumberBox" runat="server" ReadOnly="True" Visible="True"></asp:TextBox>
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                        <td>
                                                        <asp:Label ID="AssignedLabel" runat="server" Text="Assigned To"></asp:Label>
                                                        </td>
                                                        <td>
                                                        <asp:DropDownList ID="AssignedToList" runat="server">
                                                        <asp:ListItem>Unassigned</asp:ListItem>
                                                        <asp:ListItem>Engineering</asp:ListItem>
                                                        <asp:ListItem>Physics</asp:ListItem>
                                                        <asp:ListItem>Software</asp:ListItem>
                                                        </asp:DropDownList>
                                                        </td>
                                                        <td>

                                                        <asp:Label ID="ConcessiondescriptionLabel" runat="server" Text="Concession Description"></asp:Label>
                                                        </td>
                                                        <td>
                                                        
                                                            
                                                            <asp:Panel ID="CDescriptionPanel" enabled=false runat="server">
                                                              <asp:UpdatePanel ID="DescriptionUpdatePanel" runat="server"  UpdateMode="Conditional"><ContentTemplate>   
                                                            <uc2:CommentBoxuc ID="ConcessiondescriptionBoxC" MaxCount="50" runat="server" />
                                                                    </ContentTemplate>
                                                               
                                                                </asp:UpdatePanel>  
                                                            </asp:Panel>
                                                               
                                                               
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                        <td>
                                                        <asp:Label ID="commentLabel" runat="server" Text="Comment"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Panel ID="CCommentPanel" Enabled="false" runat="server" >
                                                        <asp:UpdatePanel ID="CCommentUpdatePanel" runat="server" UpdateMode="Conditional"><ContentTemplate>
                                                            <uc2:CommentBoxuc ID="ConcessionCommentBox" runat="server" />
                                                                <asp:Button ID="ClearCommentButton" runat="server" Text="Clear" CausesValidation="False" CssClass="buttonmargin" />
                                                               </ContentTemplate>
                                                               
                                                                </asp:UpdatePanel>
                                                            </asp:Panel>

                                                        </td>
                                                        <td>
                                                        <asp:Label ID="ActionLabel" runat="server" Text="Concession Action"></asp:Label>
                                                        </td>
                                                        <td>

                                                            <asp:Panel ID="CActionPanel" Enabled="false" runat="server">
                                                                <asp:UpdatePanel ID="ConcessionActionUpdatePanel" Updatemode="Conditional" runat="server"><ContentTemplate> 
                                                            <uc2:CommentBoxuc ID="ConcessionActionBox" runat="server" />
                                                               <asp:Button ID="ClearActionButton" runat="server" Text="Clear" CausesValidation="False" CssClass="buttonmargin" />
                                                                         </ContentTemplate></asp:UpdatePanel>                                                         
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
                                                        </tr>                     
                                                        </table>
                                                        </fieldset>
                                                        </asp:TableCell>
                                                        <asp:TableCell>
                                                    <fieldset style="width: 300px;">
                                                        <asp:PlaceHolder ID="DeviceRepeatFaultPlaceHolder" runat="server"></asp:PlaceHolder>
                                                    </fieldset>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>

                                    
                                    </asp:Panel>

                                    <asp:Panel ID="Panel6" runat="server" BorderStyle="Solid" BorderColor="Yellow">
                                        <asp:GridView ID="GridView5" runat="server" AllowPaging="True"
                                            AutoGenerateColumns="False" CellPadding="4" DataKeyNames="TrackingID"
                                            EnableViewState="False" ForeColor="#333333" GridLines="None">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="TrackingID" HeaderText="TrackingID"
                                                    InsertVisible="False" ReadOnly="True" SortExpression="TrackingID" Visible="false" />
                                                <asp:BoundField DataField="TrackingComment" HeaderText="Comment"
                                                    SortExpression="Comment" />
                                                <asp:BoundField DataField="Action" HeaderText="Concession Action"
                                                    SortExpression="Action" />
                                                <asp:BoundField DataField="AssignedTo" HeaderText="AssignedTo"
                                                    SortExpression="AssignedTo" />
                                                <asp:BoundField DataField="Status" HeaderText="Status"
                                                    SortExpression="Status" />
                                                <asp:BoundField DataField="LastupdatedBy" HeaderText="LastupdatedBy"
                                                    SortExpression="LastupdatedBy" />
                                                <asp:BoundField DataField="LastupdatedOn" HeaderText="LastupdatedOn"
                                                    SortExpression="LastupdatedOn" />

                                            </Columns>
                                            <EditRowStyle BackColor="#99FF33" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </asp:Panel>





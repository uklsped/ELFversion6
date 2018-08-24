﻿<%@ Control Language="VB"  AutoEventWireup="false" CodeFile="ViewOpenFaults.ascx.vb" Inherits="ViewOpenFaults" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register src="WriteDatauc.ascx" tagname="WriteDatauc" tagprefix="uc1" %>

<%@ Register src="ManyFaultGriduc.ascx" tagname="ManyFaultGriduc" tagprefix="uc2" %>

<%@ Register src="controls/DeviceReportedfaultuc.ascx" tagname="DeviceReportedfaultuc" tagprefix="uc5" %>

<%@ Register src="controls/DeviceRepeatFaultuc.ascx" tagname="DeviceRepeatFaultuc" tagprefix="uc3" %>




<style type="text/css">
    .style2
    {
        width: 474px;
        height: 26px;
    }
    .style3
    {
        height: 26px;
    }
      
    .cssPager span { background-color:#4f6b72; font-size:18px;}    
      
</style>

       <br />
<br />

       <asp:UpdatePanel ID="UpdatePanel4"  runat="server" 
    ChildrenAsTriggers="False" UpdateMode="Conditional">
                <ContentTemplate>

               <asp:Label ID="FakeLabel" runat="server" style="display:none;" />
                    <asp:Table ID="Table3" runat="server">
                    <asp:TableRow>
                    <asp:TableCell>
                        
                    <uc2:ManyFaultGriduc ID="ManyFaultGriduc"  NewFault="false" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell>
                    <fieldset style="width:auto;">
                    <legend>Open Concessions</legend>
                                               
                    <asp:GridView ID="ConcessionGrid" runat="server" AutoGenerateColumns="False" CellPadding="4" 
                        DataKeyNames="incidentID" PageSize="5" 
                        style="top: 670px; left: 10px; height: 162px; width: 617px" 
                        AllowPaging="True" OnPageIndexChanging="ConcessionGrid_PageIndexChanging" 
                        onrowcommand="FaultGridView_RowCommand"
                        ForeColor="#333333" GridLines="None" EmptyDataText="No Data To Display" EmptyDataRowStyle-ForeColor="White" EmptyDataRowStyle-BackColor="Black" Font-Bold="True">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" />
                        <PagerStyle CssClass="cssPager" />
                        <Columns>
                            <asp:BoundField DataField="incidentID" HeaderText="incidentID" InsertVisible="False" 
                                ReadOnly="True" SortExpression="incidentID" />
                            <asp:BoundField DataField="ConcessionNumber" HeaderText="Concession Number" 
                                SortExpression="ConcessionNumber" ItemStyle-HorizontalAlign="Center" >
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ConcessionDescription" HeaderText="Concession Description" 
                                SortExpression="Description" />
                            <asp:BoundField DataField="Action" HeaderText="Concession Action" 
                                SortExpression="Action" />
                            <asp:BoundField DataField="DateInserted" HeaderText="Date Reported" 
                                SortExpression="DateReported" />

                                <asp:ButtonField ButtonType="Button" CommandName="View" Text="View Concession History" />
                                <asp:ButtonField ButtonType="Button" CommandName="Faults" Text="View Faults" />
                                <asp:ButtonField ButtonType="Button" CommandName="Log Fault" Text="Log Repeat Fault" />
                            <asp:TemplateField>
                            <ItemTemplate>
            
            </ItemTemplate>
            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                    
                    </fieldset>
                    <br />
                    </asp:TableCell>
                  </asp:TableRow>
                   </asp:Table>
                    
                    </ContentTemplate>
                 
                    </asp:UpdatePanel>
                    
<asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
<ContentTemplate>
                    <asp:Panel ID="Panel4" runat="server">
                    
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="UpdatefaultView" runat="server">
                       
                    <asp:Panel ID="UpdatePanelRepeatFault" runat="server" Visible="false">
                    <ContentTemplate>
                     <fieldset style="width:700px;">
                    <legend>Repeat Faults</legend>
                    <fieldset style="width:700px;">
                    <legend>Record Repeat Fault</legend>
                       
                     <asp:TableRow runat="server">
            <asp:TableCell>
                <asp:Label ID="Label1" runat="server" Text="Incident ID"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                
            </asp:TableCell>
            <asp:TableCell>
            </asp:TableCell>
            </asp:TableRow>
<asp:PlaceHolder ID="PlaceholderRepeatFault" runat="server"></asp:PlaceHolder>
        
                        <asp:Button ID="ViewExistingFaults" runat="server" Text="View Associated Faults" causesvalidation="false"/>
                        </fieldset>
                        <fieldset style="width:auto">
                       <legend>Associated Faults
                           </legend>
                       <asp:UpdatePanel ID="UpdatePanelVEF" runat="server" ChildrenAsTriggers="false">
                            
                        <ContentTemplate>
                            <asp:PlaceHolder ID="PlaceHolder3" runat="server"></asp:PlaceHolder>

                            </fieldset></fieldset>
                    </ContentTemplate>
                        </asp:UpdatePanel>
                        
                    </ContentTemplate>
                    </asp:Panel>
                    </asp:View>
                        <asp:View ID="View1" runat="server">
                            <asp:MultiView ID="MultiView2" runat="server">
                               <asp:View ID="statustech" runat="server">
                               <asp:Panel ID="statustechpanel" runat="server" visible="false" >
     <uc1:WriteDatauc ID="WriteDatauc3" LinacName="" UserReason="4"  Tabby="incident"  WriteName="incident" visible="false" runat="server" />

        <asp:Panel ID="Panel2" runat="server" BorderColor="#33CC33" BorderStyle="Solid">
            <asp:Table ID="Table2" runat="server" >
                
            <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="Label3" runat="server" Text="Problem ID"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
            </asp:TableCell>
            <asp:TableCell>
            </asp:TableCell>
            </asp:TableRow>
            </asp:Table>
            <asp:Table ID="Table1" runat="server" Width="943px">
            <asp:TableRow>
                
            <asp:TableCell>
                <fieldset style="width:550px;"></legend>
                        Details
                    <table style="width: 160px;">
                        <tr>
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
                               <%-- <asp:UpdatePanel ID="UpdatePanelStatuslist" runat="server">
                                    <ContentTemplate>--%>
                                        <asp:Panel ID="Panel5" runat="server">
                                            <asp:DropDownList ID="DropDownList1" autopostback="true" runat="server">
                                            <asp:ListItem>Select</asp:ListItem>
                                            <asp:ListItem>Open</asp:ListItem>
                                            <asp:ListItem>Concession</asp:ListItem>
                                            <asp:ListItem>Closed</asp:ListItem>
                                            </asp:DropDownList>
                                        </asp:Panel>
                              <%--  </ContentTemplate>--%>
<%--                                <Triggers><asp:AsyncPostBackTrigger ControlID="DropDownList1" EventName="SelectedIndexChanged" /></Triggers>--%>

                              <%--  </asp:UpdatePanel>--%>
                            <td>
                                <asp:Label ID="ConcessionLabel" runat="server" Text="Concession Number"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="ConcessionNumberBox" runat="server" ReadOnly="True" 
                            Visible="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="AssignedLabel" runat="server" Text="Assigned To"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownList2" runat="server"><asp:ListItem>Unassigned</asp:ListItem><asp:ListItem>Engineering</asp:ListItem><asp:ListItem>Physics</asp:ListItem><asp:ListItem>Software</asp:ListItem></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="ConcessiondescriptionLabel" runat="server" 
                                    Text="Concession Description"></asp:Label>
                                <td>
                                    <asp:TextBox ID="ConcessiondescriptionBox" runat="server" ReadOnly="True" TextMode="MultiLine"
                                Visible="True" MaxLength="250"></asp:TextBox>
                                </td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="commentLabel" runat="server" Text="Comment"></asp:Label>
                            </td>
                            <td>
                                <asp:PlaceHolder ID="CommentPlace" runat="server">
                                <asp:UpdatePanel ID="CommentBoxUpdatePanel" runat="server"><ContentTemplate>
                                <asp:TextBox ID="CommentBox1" runat="server" readonly="true" visible="false"
                            TextMode="MultiLine" MaxLength="250" EnableViewState="False"></asp:TextBox>
                                    </ContentTemplate><Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="DropDownList1" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="CommentBoxButton" EventName="Click" />
                                                      </Triggers></asp:UpdatePanel>
                                    </asp:PlaceHolder>
                            </td>
                            <td>
                                <asp:Label ID="ActionLabel" runat="server" Text="Concession Action"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="ActionBox"  TextMode="MultiLine" MaxLength="250" ReadOnly="true" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="SaveAFault" runat="server" Text="Save"  CausesValidation="false"  />
                                <td>
                                    <asp:Button ID="CancelButton" runat="server" Text="Cancel/Close" causesvalidation="false"/>
                                </td>
                        </td>
                        </tr>
                    </table>
                </fieldset>
    </asp:TableCell>
    <asp:TableCell>
   
            <fieldset style="width:300px;">
                

                      <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder></td>
                     
        </tr>
    </table>

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
                            InsertVisible="False" ReadOnly="True" SortExpression="TrackingID" visible="false"/>
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

    </asp:Panel>
</asp:View>       
<asp:View ID="statusother" runat="server">
                 <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="false" ChildrenAsTriggers="false">
<ContentTemplate>
              
    <asp:Panel ID="Panel1" runat="server">
                    <asp:GridView ID="GridView2" runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" CellPadding="4" DataKeyNames="TrackingID" 
                    EnableViewState="False" ForeColor="#333333" GridLines="None">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="TrackingID" HeaderText="TrackingID" 
                            InsertVisible="False" ReadOnly="True" SortExpression="TrackingID" visible="false"/>
                        <asp:BoundField DataField="TrackingComment" HeaderText="Comment" 
                            SortExpression="TrackingComment" />
                        <asp:BoundField DataField="AssignedTo" HeaderText="Assigned To" 
                            SortExpression="AssignedTo" />
                        <asp:BoundField DataField="Status" HeaderText="Status" 
                            SortExpression="Status" />
                        <asp:BoundField DataField="LastupdatedBy" HeaderText="Lastupdated By" 
                            SortExpression="LastupdatedBy" />
                        <asp:BoundField DataField="LastupdatedOn" HeaderText="Lastupdated On" 
                            SortExpression="LastupdatedOn" />
                            <asp:BoundField DataField="ConcessionNumber" HeaderText="Concession Number"
                            SortExpression="ConcessionNumber" ItemStyle-HorizontalAlign="Center" />

                    </Columns>
                    <EditRowStyle BackColor="#99FF33" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                </asp:GridView>
                
   </asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</asp:View>
</asp:MultiView>
</asp:View>

 <asp:View ID="View2" runat="server">
                        
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" Visible="false" ChildrenAsTriggers="false">
<ContentTemplate>
    <asp:Panel ID="Panel3" runat="server">
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>                 
   </asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</asp:View>
</asp:MultiView>
<asp:Button ID="Hidefaults" runat="server" CausesValidation="False" visible="false"
                        Text="Close" />
                        
                        </asp:Panel>
                </ContentTemplate>
                
            </asp:UpdatePanel>

<asp:Button ID="CommentBoxButton" runat="server" Text="" Style="display:none" CausesValidation="False" />


         

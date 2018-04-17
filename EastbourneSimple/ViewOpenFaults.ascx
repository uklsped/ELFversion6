<%@ Control Language="VB"  AutoEventWireup="false" CodeFile="ViewOpenFaults.ascx.vb" Inherits="ViewOpenFaults" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register src="WriteDatauc.ascx" tagname="WriteDatauc" tagprefix="uc1" %>

<%@ Register src="FaultGriduc.ascx" tagname="FaultGriduc" tagprefix="uc3" %>

<%@ Register src="UpDateIncidentuc.ascx" tagname="UpDateIncidentuc" tagprefix="uc4" %>

<%@ Register src="ManyFaultGriduc.ascx" tagname="ManyFaultGriduc" tagprefix="uc2" %>

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

<%--
                    <asp:Button ID="ViewFault" runat="server" 
                CausesValidation="False" Text="View Open Faults" />--%>    
       <br />
<br />
<uc1:WriteDatauc ID="WriteDatauc1"  LinacName ="" Tabby="Updatefault" UserReason="11" Visible="false" runat="server" />
       <asp:UpdatePanel ID="UpdatePanel4"  runat="server" 
    ChildrenAsTriggers="False" UpdateMode="Conditional">
                <ContentTemplate>
<%--                <uc1:WriteDatauc ID="WriteDatauc1"  LinacName ="LA1" Tabby="Updatefault" UserReason="11" Visible="false" runat="server" />
--%>                <asp:Label ID="FakeLabel" runat="server" style="display:none;" />
                    <asp:Table ID="Table3" runat="server">
                    <asp:TableRow>
                    <asp:TableCell>
                     <uc2:ManyFaultGriduc ID="ManyFaultGriduc"  NewFault="false" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell>
                    <fieldset style="width:auto;">
                    <legend>Open Concessions</legend>
                        <%--<uc3:FaultGriduc ID="FaultGriduc1"  Incident = "" NewFault="False" runat="server" />--%>
                       
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" 
                        DataKeyNames="incidentID" PageSize="5" 
                        style="top: 670px; left: 10px; height: 162px; width: 617px" 
                        AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging" 
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
                    <Triggers>
<%--<asp:AsyncPostBackTrigger ControlId="GridView1" EventName="SelectedIndexChanged" />--%>
</Triggers>
                  
                    </asp:UpdatePanel>
                    
                    <%--<asp:Panel ID="Panel2" runat="server">
                     <div >
               </div>
                    </asp:Panel>
                    <br />
                        <asp:ModalPopupExtender ID="Panel2_ModalPopupExtender" runat="server" 
        DynamicServicePath="" Enabled="True" TargetControlID="FakeLabel" PopupControlID="Panel2">
    </asp:ModalPopupExtender>--%>
<asp:UpdatePanel ID="UpdatePanel3" runat="server">
<ContentTemplate>
                    <asp:Panel ID="Panel4" runat="server">
                    
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="UpdatefaultView" runat="server">
                       
                    <asp:UpdatePanel ID="UpdatePanelRepeatFault" runat="server" Visible="false">
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
                     <table style="width:300px;">
        <tr>
            <td class="style1">
                Area:</td>
            <td>
    <asp:TextBox ID="AreaBox" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Energy:</td>
            <td class="style3">
               <asp:DropDownList ID="DropDownListEnergy" runat="server">
    </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="style1">
                Gantry Angle:</td>
            <td>
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style1">
                Collimator Angle:</td>
            <td>
    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
            </td>
            </tr>
            <tr>
        <td class="style1">
                Fault Description:</td>
            <td>
              <asp:TextBox ID="TextBox4" runat="server" MaxLength="250" TextMode="MultiLine"></asp:TextBox>
              </td> 
        </tr>
         <tr>
            <td class="style1">
                Patient ID:</td>
            <td>
    <asp:TextBox ID="PatientIDBox" Text="" runat="server"></asp:TextBox>
    <asp:RegularExpressionValidator ID="RegularExpressionPatient" runat="server" ControlToValidate="PatientIDBox" validationexpression="^\d{7}$" Display="Dynamic" ErrorMessage="Please enter a BSUH ID"></asp:RegularExpressionValidator>
            </td>
            </tr>

    </table>
    <table style="width:300px;">
        <tr>
            <td class="style1">
        <asp:Button ID="confirmfault" runat="server" Text="Confirm Repeat Fault" 
                    causesvalidation="false"/>
        </td>
        <td><asp:Button ID="Cancel" runat="server" Text="Cancel" CausesValidation="false" />
        </td>
        </tr>
        </table>
        
                        <asp:Button ID="ViewExistingFaults" runat="server" Text="View Associated Faults" causesvalidation="false"/>
                        </fieldset>
                        <%--<uc3:FaultGriduc ID="FaultGriduc1" runat="server" />--%>
                       <fieldset style="width:auto">
                       <legend>Associated Faults
                           </legend>
                       <asp:UpdatePanel ID="UpdatePanelVEF" runat="server">
                            
                        <ContentTemplate>
                        <asp:GridView ID="GridView4" AutoGenerateColumns="false" runat="server"
                        CellPadding="4" DataKeyNames="FaultID" 
        EnableViewState="False" ForeColor="#333333" GridLines="None" 
        AllowSorting="True">
        <RowStyle BackColor="#E3EAEB" />
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="FaultID" HeaderText="FaultID" InsertVisible="False" 
                ReadOnly="True"
                SortExpression="FaultID" />
            <asp:BoundField DataField="Description" HeaderText="User Description" 
                SortExpression="Description" />
            <asp:BoundField DataField="ReportedBy" HeaderText="Reported By" 
                SortExpression="ReportedBy" />
            <asp:BoundField DataField="DateReported" HeaderText="Date Reported" 
                SortExpression="DateReported" />
                <asp:BoundField DataField="Area" HeaderText="Area" SortExpression="Area" />
                <asp:BoundField DataField="Energy" HeaderText="Energy" SortExpression="Energy" />
                <asp:BoundField DataField="GantryAngle" HeaderText="GantryAngle" SortExpression="GantryAngle" />
                     <asp:BoundField DataField="CollimatorAngle" HeaderText="CollimatorAngle" SortExpression="CollimatorAngle" />      
                           <asp:BoundField DataField="Linac" HeaderText="Linac" 
                SortExpression="Linac" />
           
        </Columns>
        <EditRowStyle BackColor="#7C6F57" />
        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
    </asp:GridView></fieldset></fieldset>
                    </ContentTemplate>
                        </asp:UpdatePanel>
                        
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    </asp:View>
                        <asp:View ID="View1" runat="server">
                            <asp:MultiView ID="MultiView2" runat="server">
                               <asp:View ID="statustech" runat="server">
                               <asp:UpdatePanel ID="UpdatePanel5" runat="server" visible="false"><ContentTemplate>
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
                                <asp:UpdatePanel ID="UpdatePanelStatuslist" runat="server"><ContentTemplate><asp:Panel 
                                        ID="Panel5" runat="server"><asp:DropDownList ID="DropDownList1" 
                                        autopostback="true" runat="server"><asp:ListItem>Select</asp:ListItem><asp:ListItem>Open</asp:ListItem><asp:ListItem>Concession</asp:ListItem><asp:ListItem>Closed</asp:ListItem></asp:DropDownList>
                                        </asp:Panel>
                                </ContentTemplate>
                                <%--<Triggers><asp:AsyncPostBackTrigger ControlID="DropDownList1" EventName="SelectedIndexChanged" /></Triggers>--%>

                                </asp:UpdatePanel>
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
                                    <asp:TextBox ID="ConcessiondescriptionBox" runat="server" ReadOnly="True" 
                                Visible="True" MaxLength="20"></asp:TextBox>
                                </td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="commentLabel" runat="server" Text="Comment"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="CommentBox1" runat="server" readonly="false" 
                            TextMode="MultiLine" MaxLength="250"></asp:TextBox>
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
    <%--<asp:PlaceHolder ID="PlaceHolderDisplayfault" runat="server"></asp:PlaceHolder>--%>
            <fieldset style="width:300px;">
                

            <legend>Reported Fault Details</legend>
                <table style="width:300px;">                
        <tr>           
            <td class="style1">
                Area:</td>
            <td>
    <asp:TextBox ID="OriginalAreaBox" runat="server" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
         <tr>
            
            <td class="style1">

                Energy:</td>
            <td>      

    <asp:TextBox ID="OriginalEnergyBox" runat="server" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style1">
                Gantry Angle:</td>
            <td>
    <asp:TextBox ID="OriginalGantryBox" ReadOnly="True" runat="server"></asp:TextBox>

            </td>
        </tr>
        <tr>

            <td class="style1">
                
                Collimator Angle:</td>
            <td>
                
    <asp:TextBox ID="OriginalCollBox" runat="server" ReadOnly="True"></asp:TextBox>

            </td>
            </tr>
            <tr>
                
        <td class="style1">
                
                Fault Description:</td>
            <td>
                
              <asp:TextBox ID="OriginalDescriptionBox" runat="server" MaxLength="250" ReadOnly="True" 
                    Rows="5" TextMode="MultiLine"></asp:TextBox>
              </td>
              </tr>
               <tr>
                
        <td class="style1">
                
                Patient ID:</td>
            <td>
                
              <asp:TextBox ID="OriginalPatientIDBox" runat="server" ReadOnly="True"></asp:TextBox>
              </td>
              </tr>
              <tr>

              <td>
                  
              Reported By:</td>
              <td>
                  <asp:TextBox ID="OriginalReportedBox" runat="server" ReadOnly="True"></asp:TextBox>
                </td>
              </tr>
              <tr>
              <td>
                  
              Date Open:</td>
              <td>
                  <asp:TextBox ID="OriginalOpenDateBox" runat="server" ReadOnly="True"></asp:TextBox>
              </td>
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
</ContentTemplate>
    </asp:UpdatePanel>
</asp:View>       
<asp:View ID="statusother" runat="server">
                 <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="false">
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
                        <%--<asp:BoundField DataField="incidentID" HeaderText="incidentID" 
                            SortExpression="FaultID" />--%>
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
                        
                    <%--<asp:Panel ID="Panel3" runat="server" Height="200px">
                    </asp:Panel>--%>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" Visible="false">
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

            <div>
            <asp:UpdatePanel ID="UpdatePanelNewFault" runat="server" Visible="false">
                    <ContentTemplate>
                     <fieldset style="width:700px;">
                    <legend>New Fault</legend>
                    <fieldset style="width:700px;">
                    <legend>Record New Fault</legend>
                     <asp:TableRow ID="TableRow1" runat="server">
            <asp:TableCell>
                <asp:Label ID="Label5" runat="server" Text="Incident ID"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="Label6" runat="server" Text=""></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
            </asp:TableCell>
            <asp:TableCell>
            </asp:TableCell>
            </asp:TableRow>
                     <table style="width:300px;">
        <tr>
            <td class="style1">
                Area:</td>
            <td>
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Energy:</td>
            <td class="style3">
               <asp:DropDownList ID="DropDownList3" runat="server">
    </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="style1">
                Gantry Angle:</td>
            <td>
    <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style1">
                Collimator Angle:</td>
            <td>
    <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
            </td>
            </tr>
            <tr>
        <td class="style1">
                Fault Description:</td>
            <td>
              <asp:TextBox ID="TextBox7" runat="server" MaxLength="250" TextMode="MultiLine"></asp:TextBox>
              </td> 
        </tr>
         <tr>
            <td class="style1">
                Patient ID:</td>
            <td>
    <asp:TextBox ID="TextBox8" Text="" runat="server"></asp:TextBox>
    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="PatientIDBox" validationexpression="^\d{7}$" Display="Dynamic" ErrorMessage="Please enter a BSUH ID"></asp:RegularExpressionValidator>
            </td>
            </tr>

    </table>
    <table style="width:300px;">
        <tr>
            <td class="style1">
        <asp:Button ID="Button1" runat="server" Text="Confirm Repeat Fault" 
                    causesvalidation="false"/>
        </td>
        <td><asp:Button ID="Button2" runat="server" Text="Cancel" CausesValidation="false" />
        </td>
        </tr>
        </table>
        
  
                        
                    </ContentTemplate>
                    </asp:UpdatePanel>
            
            
            
            
            </div>
         
<%--<uc2:ManyFaultGriduc ID="ManyFaultGriduc1" runat="server" />--%>

         

﻿<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DeviceRepeatFaultuc.ascx.vb" Inherits="Controls_DeviceRepeatFaultuc" %>


<%@ Register src="CommentBoxuc.ascx" tagname="CommentBoxuc" tagprefix="uc1" %>
<%@ Register Src="~/controls/CommentBoxuc.ascx" TagPrefix="uc2" TagName="CommentBoxuc" %>

                    <%@ Register src="../ManyFaultGriduc.ascx" tagname="ManyFaultGriduc" tagprefix="uc3" %>

                    <fieldset style="width: 700px;">
                        <legend>Repeat Faults</legend>
                        <fieldset style="width: 700px;">
                            <legend>Record Repeat Fault</legend>
                            <asp:Table runat="server">
                            <asp:TableRow runat="server">
                                <asp:TableCell>
                                    <asp:Label ID="Label1" runat="server" Text="Concession: "></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                     <asp:Label ID="ConcessionNumber" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="Label3" runat="server" Text="" Visible="false"></asp:Label>
                                   </asp:TableCell>
                                <asp:TableCell>
                                </asp:TableCell>
                                <asp:TableCell>
                                </asp:TableCell>
                            </asp:TableRow>
                             </asp:Table>
                           

<asp:MultiView ID="MultiView1" runat="server">

    <asp:View ID="Linac" runat="server">
       
                     <table style="width:401px;">
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
               <asp:DropDownList ID="DropDownListEnergy" runat="server" >
    </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="style1">
                Gantry Angle:</td>
            <td>
    <asp:TextBox ID="GantryAngleBox" runat="server"></asp:TextBox>
                 <asp:CompareValidator ID="GantryAngleCheck"
        runat="server" ErrorMessage="Please enter angle as integer" 
                    ControlToValidate="GantryAngleBox" Operator="DataTypeCheck" SetFocusOnError="True" 
                    Type="Integer" Display="Static" ValidationGroup="incident"></asp:CompareValidator>
                <asp:RangeValidator ID="GantryRangeCheck" runat="server" 
                    ErrorMessage="Range is 0 to 360 degrees" Type="Integer" SetFocusOnError="True" 
                    MaximumValue="360" MinimumValue="0" ControlToValidate="GantryAngleBox" 
                    Display="Dynamic" ValidationGroup="Incident"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td class="style1">
                Collimator Angle:</td>
            <td>
    <asp:TextBox ID="CollimatorAngleBox" runat="server"></asp:TextBox>
                 <asp:CompareValidator ID="CollimatorAngleCheck"
        runat="server" ErrorMessage="Please enter angle as integer" 
                ControlToValidate="CollimatorAngleBox" Operator="DataTypeCheck" SetFocusOnError="True" 
                Type="Integer" Display="Static" ValidationGroup="defect"></asp:CompareValidator>
                <asp:RangeValidator ID="CollimatorRangeCheck" runat="server" 
                ErrorMessage="Range is 0 to 360 degrees" Type="Integer" SetFocusOnError="True" 
                MaximumValue="360" MinimumValue="0" ControlToValidate="CollimatorAngleBox" 
                Display="Dynamic" ValidationGroup="defect"></asp:RangeValidator>
                </td>
            </tr>
            <tr>
        <td class="style1">
                Fault Description:</td>
            <td>
                <asp:Panel ID="FaultPanel" runat="server" Enabled="True">
                    <uc1:CommentBoxuc ID="FaultDescription" runat="server" />
                </asp:Panel>
            </td> 
        </tr>
                         <tr>
          <td class="style2">
              Corrective Action:
          </td>
          <td>
              <asp:Panel ID="ActPanel" Enabled="False" runat="server">
                  <uc2:CommentBoxuc runat="server" ID="RadActC" />
              </asp:Panel>
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
    </asp:View>
    <asp:View ID="Tomo" runat="server">
         
                  <table style="width:401px;">                
      
         <tr>
             <td class="style1">
                Error code:</td>
            <td>      
    <asp:TextBox ID="ErrorTextBox" runat="server"></asp:TextBox>
            </td>
        </tr>
                       <tr>
            <td class="style2">
                Physicist/Accuray job number:</td>
            <td>
    <asp:TextBox ID="Accuray" runat="server" EnableViewState="false"></asp:TextBox>
               </td>
        </tr>
        <tr>
        <td class="style1">
                Fault Description:</td>
            <td>
                <asp:Panel ID="FaultPanelT" Enabled="true" runat="server">
                <uc2:CommentBoxuc runat="server" ID="FaultDescriptionT" />
                    </asp:Panel>
              </td>
              </tr>
                       <tr>
          <td class="style2">
              Corrective Action:
          </td>
          <td>
              <asp:Panel ID="ActPanelT" Enabled="true" runat="server">
                  <uc2:CommentBoxuc runat="server" ID="RadActCT" />
              </asp:Panel>
                </td>
      </tr>
               <tr>
                
        <td class="style1">
                
                Patient ID:</td>
            <td>
                
              <asp:TextBox ID="PatientIDBoxT" runat="server" ></asp:TextBox>
                 <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="PatientIDBoxT" validationexpression="^\d{7}$" Display="Dynamic" ErrorMessage="Please enter a BSUH ID"></asp:RegularExpressionValidator>
              </td>
                   </tr>
                              
    </table>
    </asp:View>

</asp:MultiView>
<div>
 <table style="width:300px;">
      <tr>
                   <td>
                   <asp:Label ID="Label2" runat="server" Text="Radiation Incident?"></asp:Label>
                       </td>
                          <td>
                                     <asp:RadioButtonList ID="RadioIncident" runat="server" AutoPostBack="false" ValidationGroup="Repeat">
                                        <asp:ListItem Text="No" Value="False"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                    </asp:RadioButtonList>
                              </td>
                          <td>
                        <asp:RequiredFieldValidator 
            ID="ReqiredFieldValidator1"
            runat="server"
            ControlToValidate="RadioIncident"
            ErrorMessage="Please complete Radiation Incident Selection"
                            Display="Dynamic"
                            validationgroup="Repeat"
            >
        </asp:RequiredFieldValidator>
                       </td>
              </tr>
        <tr>
            <td class="style1">
                <asp:Button ID="SaveRepeatFault" runat="server" Text="Save Repeat Fault" CausesValidation="false"  />
        </td>
        <td>
            <asp:Button ID="CancelFault" runat="server" Text="Cancel" CausesValidation="false"/>
            
        </td>
        </tr>
        </table>
    </div>
                             <asp:Button ID="ViewExistingFaults" runat="server" Text="View Associated Faults" CausesValidation="false" />
                                    
                                
                        </fieldset>
                        <fieldset style="width: auto">
                            <legend>Associated Faults
                            </legend>
                            <asp:Panel ID="UpdatePanelVEF" runat="server">
                                 <asp:PlaceHolder ID="PlaceHolder3" runat="server"></asp:PlaceHolder>
                            </asp:Panel>
                        </fieldset>
                    </fieldset>

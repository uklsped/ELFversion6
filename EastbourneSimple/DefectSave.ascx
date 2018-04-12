<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DefectSave.ascx.vb" Inherits="DefectSave" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%--No need now for WriteDatauc Analysis 23/11/16 --%>
<%--Added back in 26/03/18 --%>
                 <%@ Register src="WriteDatauc.ascx" tagname="WriteDatauc" tagprefix="uc1" %>
           

           

                         <style type="text/css">
                     .style1
                     {
                         width: 271px;
                     }
                     .style2
                     {
                         width: 124px;
                     }
                     .style3
                     {
                         width: 124px;
                         height: 26px;
                     }
                     .style4
                     {
                         height: 26px;
                     }
                     .redcolour
                     {
                        color: red;
                        font-weight: bold;
                        font-size: medium;
                     }
                 </style>
<asp:HiddenField ID="HiddenField1" Value="" runat="server" />
<asp:HiddenField ID="HiddenField2" Value="" runat="server" />
<%-- NO requirement 23/11/16 --%>
<%-- Added back in 26/03/18 --%>
<uc1:WriteDatauc ID="WriteDatauc1" LinacName="" UserReason="11"  Tabby="Defect"  WriteName="Defect" visible="false"  runat="server" />

                 <%--<asp:TableCell ID="c2" runat="server" HorizontalAlign="left" Width="250px">Radiographer Cleared Fault <br />--%>
                 
                       <div style ="width:401px">
                           Record Repeat Fault<br />
                           <span class = "redcolor"> * Mandatory Field </span><br />
                       <table style="width:401px;" >
                           <tr>
                               <td class="style1">
                                   <asp:UpdatePanel ID="UpdatePanelDefectlist" runat="server" UpdateMode="Conditional"><ContentTemplate>
                                       
                                   <asp:DropDownList ID="Defect" runat="server" AutoPostBack="true" 
                                       AppendDataBoundItems="True" DataValueField="IncidentID" DateTextField ="Fault">
                                    <asp:ListItem>Select</asp:ListItem>                           
                                   <%-- <asp:ListItem Value="-1">2T Error</asp:ListItem>
                                    <asp:ListItem Value="-2">2R Error</asp:ListItem>
                                    <asp:ListItem Value="-3">DOSE RATE MON</asp:ListItem>
                                    <asp:ListItem Value="-4">D1 RLY OFF</asp:ListItem>
                                    <asp:ListItem Value="-5">COMMUNICATION ERROR</asp:ListItem>                                
                                    <asp:ListItem Value="-6">LEAVES NOT READY</asp:ListItem>
                                    <asp:ListItem Value="-7">UNIFORMITY</asp:ListItem>                                   
                                    <asp:ListItem Value="-8">HT CON K</asp:ListItem>                                
                                    <asp:ListItem Value="-9">INTEGRITY NOT READY</asp:ListItem>                                 
                                    <asp:ListItem Value="-10">IVIEW IMAGE QUALITY</asp:ListItem>
                                    <asp:ListItem Value="-11">M.FIL V MON</asp:ListItem>
                                    <asp:ListItem Value="-12">DOSE DIFF</asp:ListItem>
                                    <asp:ListItem Value="-13">IVIEW FAULT</asp:ListItem>
                                    <asp:ListItem Value="-14">XVI FAULT</asp:ListItem>
                                    <asp:ListItem Value="-15">DAILY QA3 WARNING</asp:ListItem>--%>
                                    </asp:DropDownList>
                                    </ContentTemplate>
                                   </asp:UpdatePanel>
                                    </td>
                                    <td></td>
                                    </tr>

<%--
<tr>
<td class="style1">--%>
     </table>       
<%--                <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
            </td>
            <td></td>
            <td>
            </td>
            </tr>
            </table>--%>
  <table style="width:401px;" >
        <tr>
            <td class="style3">
                Area:
                <%--comment out 21/11/16 to replace with text box
                 <span class = "redcolor"> * </span><br /> </td>--%>
            <td class="style4">
                <%--<asp:UpdatePanel ID="UpdatePanelArea" runat="server" UpdateMode="Conditional" ><ContentTemplate>--%>
           <asp:DropDownList ID="DropDownListArea" runat="server"  enabled ="false" enableviewstate="false">
                <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                <asp:ListItem Text="Machine" Value="Machine"></asp:ListItem>
                <asp:ListItem Text="iView" Value="iView"></asp:ListItem>
                <asp:ListItem Text="XVI" Value="XVI"></asp:ListItem>
                <asp:ListItem Text="IT" Value="IT"></asp:ListItem>
                <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
            </asp:DropDownList>
                     <%--</ContentTemplate>
                                   </asp:UpdatePanel>--%>
    <%--<asp:TextBox ID="AreaBox" runat="server" Text="" readonly="true" EnableViewState="False"></asp:TextBox>--%>
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
            <td class="style2">
                Gantry Angle:</td>
            <td>
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
    <asp:CompareValidator ID="GantryAngleCheck"
        runat="server" ErrorMessage="Please enter angle as integer" 
                    ControlToValidate="TextBox2" Operator="DataTypeCheck" SetFocusOnError="True" 
                    Type="Integer" Display="Static" ValidationGroup="defect"></asp:CompareValidator>
                <asp:RangeValidator ID="GantryRangeCheck" runat="server" 
                    ErrorMessage="Range is 0 to 360 degrees" Type="Integer" SetFocusOnError="True" 
                    MaximumValue="360" MinimumValue="0" ControlToValidate="TextBox2" 
                    Display="Static" ValidationGroup="defect"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Collimator Angle:</td>
            <td>
    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
     <asp:CompareValidator ID="CollimatorAngleCheck"
        runat="server" ErrorMessage="Please enter angle as integer" 
                ControlToValidate="TextBox3" Operator="DataTypeCheck" SetFocusOnError="True" 
                Type="Integer" Display="Static" ValidationGroup="defect"></asp:CompareValidator>
                <asp:RangeValidator ID="CollimatorRangeCheck" runat="server" 
                ErrorMessage="Range is 0 to 360 degrees" Type="Integer" SetFocusOnError="True" 
                MaximumValue="360" MinimumValue="0" ControlToValidate="TextBox3" 
                Display="Static" ValidationGroup="defect"></asp:RangeValidator>
            </td>
            </tr>
            <tr>
        <td class="style2">
                Fault Description:</td>
            <td>
              <asp:TextBox ID="TextBox4" runat="server" MaxLength="250" TextMode="MultiLine"></asp:TextBox>
              </td> 
        </tr>
      <tr>
          <td class="style2">
              Corrective Action:
          </td>
          <td>
              <asp:TextBox ID="RadAct" runat="server" MaxLength="250" TextMode="MultiLine" Visible ="true"></asp:TextBox>
          </td>
      </tr>
         <tr>
            <td class="style2">
                Patient ID:</td>
            <td>
    <asp:TextBox ID="PatientIDBox" Text="" runat="server"></asp:TextBox>
    <asp:RegularExpressionValidator ID="RegularExpressionPatient" runat="server" ControlToValidate="PatientIDBox" validationexpression="^\d{7}$" Display="Static" ValidationGroup="defect" ErrorMessage="Please enter a BSUH ID"></asp:RegularExpressionValidator>
            </td>
            </tr>

<tr>
 <td class="style2">
 <asp:Button ID="SaveDefectButton" runat="server" Text="Save" ValidationGroup="defect" CausesValidation="true" />
                               

                               </td>
                               <td >
                                  
 <asp:Button ID="ClearButton" runat="server" Text="Clear" CausesValidation="False" CssClass="buttonmargin" />
                               

                               </td>
                              </tr>
                              </table>
<%--                              <table>
                           <tr>
                               <td class="style1">
                               
                                   <br />--%>
                                   Today's Repeat Faults
                                   <div style =" background-color:Green; 
        height:30px;width:400px; margin:0;padding:0">
        <table cellspacing="0" cellpadding = "0" rules="all" border="1" id="Table3" 
         style="font-family:Arial;font-size:10pt;width:400px;color:white;
         border-collapse:collapse;height:100%;">
            <tr>
               <td style ="width:150px;text-align:center">Repeat Fault</td>
               <td style ="width:80px;text-align:center">Time</td>
               <td style ="width:180px;text-align:center">Description</td>
               </tr></table></div>
                           <div style="width:400px"> <asp:UpdatePanel ID="UpdatePanel3" runat="server">
           <ContentTemplate>
           
        <div style ="height:150px; width:400px; overflow:auto;">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" showheader="false"
        DataKeyNames="ConcessionNumber" BackColor="White" Width="400px"
        BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4" 
        GridLines="Horizontal" Font-Size="smaller" >
        <RowStyle BackColor="White" ForeColor="#333333" />
        <Columns>
            <%--<asp:BoundField DataField="RRFId" HeaderText="RRFId" InsertVisible="False" 
                ReadOnly="True" SortExpression="RRFId" />--%>
            <asp:BoundField DataField="ConcessionNumber" HeaderText="Fault Identifier" ItemStyle-Width="150px" 
                SortExpression="ConcessionNumber" />
            <asp:BoundField DataField="DefectTime" HeaderText="DefectTime" ItemStyle-Width="80px" 
                SortExpression="DefectTime" />
                <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="160px" 
                SortExpression="Description" />
            <%--<asp:BoundField DataField="RadSig" HeaderText="RadSig" 
                SortExpression="RadSig" />
            <asp:BoundField DataField="CAuthId" HeaderText="CAuthId" 
                SortExpression="CAuthId" />--%></Columns><FooterStyle BackColor="White" ForeColor="#333333" />
        <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
    </asp:GridView>              
               </div>
               <%--</table>--%>
               </div>

               </ContentTemplate>
               </asp:UpdatePanel>
</div>

</div>

 



 

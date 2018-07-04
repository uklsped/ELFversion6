<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DeviceRepeatFaultuc.ascx.vb" Inherits="controls_DeviceRepeatFaultuc" %>


<asp:MultiView ID="MultiView1" runat="server">

    <asp:View ID="Linac" runat="server">
       
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
    <asp:TextBox ID="GantryAngleBox" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style1">
                Collimator Angle:</td>
            <td>
    <asp:TextBox ID="CollimatorAngleBox" runat="server"></asp:TextBox>
            </td>
            </tr>
            <tr>
        <td class="style1">
                Fault Description:</td>
            <td>
              <asp:TextBox ID="DescriptionBox" runat="server" MaxLength="250" TextMode="MultiLine"></asp:TextBox>
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
   <%-- <table style="width:300px;">
        <tr>
            <td class="style1">
                <asp:Button ID="SaveRepeatFault" runat="server" Text="Save Repeat Fault" CausesValidation="false"/>
        </td>
        <td>
            <asp:Button ID="CancelFault" runat="server" Text="Cancel" CausesValidation="false"/>
            
        </td>
        </tr>
        </table>--%>
    </asp:View>
    <asp:View ID="Tomo" runat="server">
         
                  <table style="width:300px;">                
      
         <tr>
             <td class="style1">
                Error code:</td>
            <td>      
    <asp:TextBox ID="ErrorTextBox" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
             
        <td class="style1">
                
                Fault Description:</td>
            <td>
                
              <asp:TextBox ID="DescriptionBoxT" runat="server" MaxLength="250"  
                    Rows="5" TextMode="MultiLine"></asp:TextBox>
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
 <table style="width:300px;">
        <tr>
            <td class="style1">
                <asp:Button ID="SaveRepeatFault" runat="server" Text="Save Repeat Fault" CausesValidation="false"/>
        </td>
        <td>
            <asp:Button ID="CancelFault" runat="server" Text="Cancel" CausesValidation="false"/>
            
        </td>
        </tr>
        </table>
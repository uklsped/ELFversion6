<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PhysicsQAuc.ascx.vb" Inherits="PhysicsQAuc" %>



 <%@ Register src="ViewOpenFaults.ascx" tagname="ViewOpenFaults" tagprefix="uc1" %>



 <%@ Register src="DefectSave.ascx" tagname="DefectSave" tagprefix="uc6" %>



 <%@ Register src="TodayClosedFault.ascx" tagname="TodayClosedFault" tagprefix="uc7" %>



 <%--<%@ Register src="Singlemachinefaultuc.ascx" tagname="Singlemachinefaultuc" tagprefix="uc1" %>--%>
<%@ Register src="AtlasEnergyViewuc.ascx" tagname="AtlasEnergyViewuc" tagprefix="uc2" %>
<%@ Register src="Modalitiesuc.ascx" tagname="Modalitiesuc" tagprefix="uc3" %>



 <%@ Register src="WriteDatauc.ascx" tagname="WriteDatauc" tagprefix="uc4" %>


<%@ Register src="ViewCommentsuc.ascx" tagname="ViewCommentsuc" tagprefix="uc5" %>


<uc4:WriteDatauc ID="WriteDatauc1" LinacName="" UserReason="6"  Tabby="6"  WriteName="Physics" visible="false" runat="server" />
 <div>
       <asp:Panel ID="Panel100" runat="server" BackColor="#99CCFF" BorderColor="#0033CC" 
        BorderStyle="Solid">
                     
           <asp:Table ID="Table1" runat="server" CellSpacing="20" GridLines="Both" 
               Width="1509px">
               <asp:TableRow ID="r1" runat="server">
                   <asp:TableCell ID="c1" runat="server" Width="250px" HorizontalAlign="Left">
                    
                       <asp:Label ID="StateLabel" runat="server" Text="Last State:"></asp:Label>
                       <asp:TextBox ID="StateTextBox" runat="server"></asp:TextBox>
                   
                    <asp:RadioButtonList ID="RadioButtonList1" CausesValidation="false"  
                         runat="server" AutoPostBack="True">
                       <asp:ListItem Value="1">Go To Engineering Run up</asp:ListItem>
                       <asp:ListItem Value="2" Enabled="false">Requires Pre-Clinical Run up</asp:ListItem>
                       <asp:ListItem Value="3" Enabled="False">Hand Back to Clinical</asp:ListItem>
                       <asp:ListItem Value="4">Go To Planned Maintenance</asp:ListItem>
                       <asp:ListItem Value="5">Go To Repair</asp:ListItem>
                       <asp:ListItem Value="8">Go To Training/Development</asp:ListItem>
                       <asp:ListItem Value="102">End of Day</asp:ListItem>

                    </asp:RadioButtonList>


</asp:TableCell>
                   <asp:TableCell ID="c2" runat="server" Width="50px">
                       
                   <asp:Button ID="LogOffButton" runat="server" Text="Log Off" causesvalidation="false" Enabled="False" Height="150px" />


</asp:TableCell>
                   <asp:TableCell ID="c3" runat="server" HorizontalAlign="right" Width="150px">
                   
                       <asp:TextBox ID="CommentBox" runat="server" Width="145px" Height="150px" TextMode="MultiLine" Text="">
                       </asp:TextBox>
                       
</asp:TableCell>
<asp:TableCell ID = "c4" runat="server" HorizontalAlign="Left" Width="375px">
                       <asp:UpdatePanel ID="UpdatePanel3" runat="server">
<ContentTemplate>
<asp:PlaceHolder ID="PlaceHolder4" runat="server">
</asp:PlaceHolder>
</ContentTemplate>

</asp:UpdatePanel>
                       </asp:TableCell>
                        <asp:TableCell VerticalAlign="Top">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
               <ContentTemplate>
                   <asp:Label ID="Label1" runat="server" Text="Major Faults Cleared Today:"></asp:Label>
                   <br></br>
               <asp:PlaceHolder ID="PlaceHolder5" runat="server">
                </asp:PlaceHolder>
                </ContentTemplate></asp:UpdatePanel>
               </asp:TableCell>
               </asp:TableRow>
               
           </asp:Table>
         <asp:Table ID="Table2" runat="server" CellSpacing="20" GridLines="Both" 
               Width="1069px">
         <asp:TableRow ID ="t2r1" runat="server">
         <asp:TableCell ID="t2c1" runat="server">
         <asp:Button ID="EditEnergiesButton" runat="server" Text="View Physics Energies"  CausesValidation="false"/>
               </asp:TableCell>
               <asp:TableCell ID="t2c2" runat="server">
               <asp:Button ID="Faultpanelbutton" runat="server" Text="View Open Faults" causesvalidation ="false"/>
               </asp:TableCell>
               <asp:TableCell ID="t2c3" runat="server">
                   <asp:Button ID="ViewAtlasButton" runat="server" Text="View Atlas Energies" causesvalidation="false"/>
               </asp:TableCell>

                        </asp:TableRow>
         
      </asp:Table> 
           
      <uc5:ViewCommentsuc ID="ViewCommentsuc1" linacName="" CommentSort="pqa" runat="server" />
        <asp:UpdatePanel ID="UpdatePanelQA" runat="server" Visible="false">
<ContentTemplate>
    <asp:PlaceHolder ID="PlaceHolder3" runat="server">
    </asp:PlaceHolder>
        </ContentTemplate>
        
</asp:UpdatePanel>
    </asp:Panel>
</div>


<asp:UpdatePanel ID="UpdatePanel2" runat="server" Visible="false">
<ContentTemplate>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
    </asp:PlaceHolder>
</ContentTemplate>
        
</asp:UpdatePanel>
<asp:UpdatePanel ID="UpdatePanelAtlas" runat="server" Visible="false">
<ContentTemplate>
    <asp:PlaceHolder ID="PlaceHolder2" runat="server">
    </asp:PlaceHolder>
    
    
    
</ContentTemplate>
        
</asp:UpdatePanel>





































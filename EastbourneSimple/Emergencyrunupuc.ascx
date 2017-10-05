<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Emergencyrunupuc.ascx.vb" Inherits="Emergencyrunupuc" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register src="WriteDatauc.ascx" tagname="WriteDatauc" tagprefix="uc1" %>

<%@ Register src="ViewOpenFaults.ascx" tagname="ViewOpenFaults" tagprefix="uc2" %>

<%@ Register src="AtlasEnergyViewuc.ascx" tagname="AtlasEnergyViewuc" tagprefix="uc3" %>

<link href="App_Themes/Blue/Elf.css" rel="stylesheet" type="text/css" />




<asp:Label ID="CheckUser" runat="server" Text=""  visible="true" display="none" causesvalidation="false"></asp:Label>
      

        
            

 <div>
       <asp:Panel ID="Panel100" runat="server" BackColor="#99CCFF" BorderColor="#0033CC" 
        BorderStyle="Solid">
        <asp:Table ID="Table1" runat="server" CellSpacing="20" GridLines="Both" 
               Width="1038px">
               <asp:TableRow ID="r1" runat="server">
                   <asp:TableCell ID="c1" runat="server" Width="250px" HorizontalAlign="Left">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" >
            <Columns>
            
                <asp:BoundField DataField="Energy" HeaderText="Select All Energies" 
                    SortExpression="Energy" />
                    <asp:TemplateField>  
            <HeaderTemplate>  
                <asp:CheckBox ID="chkSelectAll"   
                    runat="server" 
                    AutoPostBack="true"  
                    OncheckedChanged="checked"
                     />  
            </HeaderTemplate>
                    
            <ItemTemplate>
            <asp:CheckBox runat="server" ID="RowlevelCheckBox" />
            </ItemTemplate>
            </asp:TemplateField>
            </Columns>
        </asp:GridView>
</asp:TableCell>
                   <asp:TableCell ID="c2" runat="server" Width="50px">
                       
                   <asp:Button ID="engHandoverButton" runat="server" Text="Approve Energies" causesvalidation="false"  Height="150px" />


</asp:TableCell>
                   <asp:TableCell ID="c3" runat="server" HorizontalAlign="right" Width="625px">
                               <asp:TextBox ID="CommentBox" runat="server" MaxLength="250" Rows="5" 
                TextMode="MultiLine" Width="625px" Height="150px"></asp:TextBox>

                       </asp:TableCell>
               </asp:TableRow>
                          </asp:Table>
                          <asp:Table ID="Table2" runat="server" CellSpacing="20" GridLines="Both" Width="1038px">
         <asp:TableRow ID ="t2r1" runat="server">
         <asp:TableCell ID="t2c1" runat="server" Width="160px" HorizontalAlign="Left">
         <asp:Button ID="FaultPanelButton" runat="server" Text="View Open Faults" causesvalidation ="false"/>
               </asp:TableCell>
               <asp:TableCell ID="t2c2" runat="server" Width="50px">
               <asp:Button ID="AtlasPanelButton" runat="server" Text="View Atlas Energies" width = "160px" causesvalidation="false"/>
               </asp:TableCell>
               <asp:TableCell ID="t2c3" runat="server">
               <asp:Button ID="LogOff" runat="server" Text="Log Off Without Approving Energies" CausesValidation="False" />
               </asp:TableCell>
                        </asp:TableRow>
               </asp:Table>
                          
            </asp:Panel>
           
        </div>         

  <asp:UpdatePanel ID="UpdatePanel3" runat="server" Visible="false">
<ContentTemplate>
    <asp:PlaceHolder ID="PlaceHolder3" runat="server">
    </asp:PlaceHolder>

</ContentTemplate> 
</asp:UpdatePanel>  
<asp:UpdatePanel ID="UpdatePanelatlas" runat="server" Visible="false">
     <ContentTemplate>
         <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
     </ContentTemplate>
</asp:UpdatePanel>


 <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
            <asp:PlaceHolder ID="PlaceHolder4" runat="server">
            <uc1:WriteDatauc ID="WriteDatauc1" LinacName="LA1" UserReason="1"  Tabby="1"  WriteName="EngData"   Visible="false" runat="server" />
            </asp:PlaceHolder>
            </ContentTemplate>
            </asp:UpdatePanel>











<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Repairuc.ascx.vb" Inherits="Repairuc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="controls/CommentBoxuc.ascx" tagname="CommentBoxuc" tagprefix="uc1" %>
<%@ Register src="WriteDatauc.ascx" tagname="WriteDatauc" tagprefix="uc2" %>
<%@ Register src="AtlasEnergyViewuc.ascx" tagname="AtlasEnergyViewuc" tagprefix="uc3" %>
<%@ Register src="Modalitiesuc.ascx" tagname="Modalitiesuc" tagprefix="uc4" %>
<%@ Register src="ViewOpenFaults.ascx" tagname="ViewOpenFaults" tagprefix="uc5" %>
<%@ Register src="ViewCommentsuc.ascx" tagname="ViewCommentsuc" tagprefix="uc6" %>
<%@ Register src="DefectSave.ascx" tagname="DefectSave" tagprefix="uc7" %>
<%@ Register src="LockElfuc.ascx" tagname="LockElfuc" tagprefix="uc9" %>
<%@ Register src="TodayClosedFault.ascx" tagname="TodayClosedFault" tagprefix="uc10" %>
<%@ Register src="DefectSavePark.ascx" tagname="DefectSavePark" tagprefix="uc11" %>

<uc2:WriteDatauc ID="WriteDatauc1" LinacName="" UserReason="5"  Tabby="5"  WriteName="Repair" visible="false" runat="server" />
<uc9:LockElfuc ID="LockElfuc1" LinacName="" UserReason="5" Tabby="5" visible="false" runat="server" />

<div>
 <asp:Panel ID="Panel100" runat="server" BackColor="#99CCFF" BorderColor="#0033CC" BorderStyle="Solid">
   <asp:Table ID="Table1" runat="server" CellSpacing="20" GridLines="Both" Width="1875px">
      <asp:TableRow ID="r1" runat="server">
                  <asp:TableCell ID="c1" runat="server" Width="300px" HorizontalAlign="Left">
                       <asp:UpdatePanel ID="RadioListPanel" runat="server">
                       <ContentTemplate>
                       <asp:Label ID="StateLabel" runat="server" Text="Last State:"></asp:Label>
                       <asp:TextBox ID="StateTextBox" runat="server"></asp:TextBox>
                        <asp:RadioButtonList ID="RadioButtonList1" CausesValidation="false" runat="server" AutoPostBack="True">
                        </asp:RadioButtonList>
                       </ContentTemplate>
                       </asp:UpdatePanel>
                       <br />
                       <fieldset>
                        <legend style="font-family: Arial, Helvetica, sans-serif; font-weight: bold">Repair Comments</legend>
                           <uc1:CommentBoxuc ID="CommentBox" runat="server" />
                       </fieldset>
                    </asp:TableCell>
                  <asp:TableCell ID="c2" runat="server" Width="50px">
                       <asp:Button ID="LogOffButton" runat="server" Text="Log Off" causesvalidation="false" Enabled="false"  Height="150px" />
                  </asp:TableCell>
                  <asp:TableCell ID = "c4" runat="server" HorizontalAlign="Left" Width="500px">
                       <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                        <asp:PlaceHolder ID="PlaceHolder4" runat="server"></asp:PlaceHolder>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                  </asp:TableCell>
                  <asp:TableCell VerticalAlign="Top">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                        <asp:Label ID="Label1" runat="server" Text="Major Faults Cleared Today:"></asp:Label>
                            <br /><br />
                            <asp:PlaceHolder ID="PlaceHolder5" runat="server"></asp:PlaceHolder>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                  </asp:TableCell>
      </asp:TableRow>
   </asp:Table>
          
   <asp:Table ID="Table2" runat="server" CellSpacing="20" GridLines="Both" Width="1507px">
         <asp:TableRow ID ="t2r1" runat="server">
         <asp:TableCell ID="t2c1" runat="server">
            <asp:Button ID="LockElf" runat="server" Text="Lock Elf/Switch User" causesvalidation="false"/>
         </asp:TableCell>
         <asp:TableCell ID="t2c2" runat="server">
             <asp:Button ID="ViewAtlasButton" runat="server" Text="View Atlas Energies" causesvalidation="false"/>
         </asp:TableCell>
         <asp:TableCell ID="t2c3" runat="server">
             <asp:Button ID="PhysicsQA" runat="server" Text="View Physics Energies/Imaging"  CausesValidation="false"  />
         </asp:TableCell>
         </asp:TableRow>
   </asp:Table>

   <asp:Panel ID="Panel2" runat="server" Visible="true">
       <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
   </asp:Panel>
 </asp:Panel>
</div>
<asp:UpdatePanel ID="UpdatePanelQA" runat="server" Visible="false">
<ContentTemplate>
<asp:PlaceHolder ID="PlaceHolder3" runat="server"></asp:PlaceHolder>
</ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="UpdatePanelAtlas" runat="server" Visible="false">
<ContentTemplate>
<asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
</ContentTemplate>   
</asp:UpdatePanel>

<uc6:ViewCommentsuc ID="ViewCommentsuc1" LinacName="" CommentSort="rp" runat="server" />












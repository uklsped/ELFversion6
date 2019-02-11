<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ErunupUserControlCommon.ascx.vb" Inherits="ErunupUserControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register src="AtlasEnergyViewuc.ascx" tagname="AtlasEnergyViewuc" tagprefix="uc2" %>

<%@ Register src="controls/CommentBoxuc.ascx" tagname="CommentBoxuc" tagprefix="uc3" %>

<%@ Register src="ViewCommentsuc.ascx" tagname="ViewCommentsuc" tagprefix="uc5" %>

<%@ Register src="DefectSave.ascx" tagname="DefectSave" tagprefix="uc6" %>

<%@ Register src="TodayClosedFault.ascx" tagname="TodayClosedFault" tagprefix="uc7" %>

<%@ Register src="LockElfuc.ascx" tagname="LockElfuc" tagprefix="uc9" %>

<%@ Register src="Modalitiesuc.ascx" tagname="Modalitiesuc" tagprefix="uc10" %>

<%@ Register src="DefectSavePark.ascx" tagname="DefectSavePark" tagprefix="uc11" %>

<%@ Register src="AcceptLinac.ascx" tagname="AcceptLinac" tagprefix="uc12" %>

<%@ Register src="WriteDatauc.ascx" tagname="WriteDatauc" tagprefix="uc4" %>

<%@ Register src="ConfirmPage.ascx" tagname="ConfirmPage" tagprefix="uc1" %>


<link href="App_Themes/Blue/Elf.css" rel="stylesheet" type="text/css" />

<%@ Register src="ViewOpenFaults.ascx" tagname="ViewOpenFaults" tagprefix="uc1" %>

<uc9:LockElfuc ID="LockElfuc1" LinacName="" UserReason="1" Tabby="1" visible="false" runat="server" />

<asp:Label ID="CheckUser" runat="server" Text=""  visible="true" display="none" causesvalidation="false"></asp:Label>
 <asp:GridView ID="DummyGridView" runat="server">
        </asp:GridView>     
<div class="grid">
    <div class="col100 red">350 pixels</div>
    <div class="col200 grey">200 pixels</div>
    <div class="col300 red">300 pixels</div>
   
</div>
<div class="clear"></div>
 
<div class="grid">
 
       <%--<asp:Panel ID="Panel100" runat="server" BackColor="#99CCFF" BorderColor="#0033CC" 
        BorderStyle="Solid">--%>
    <asp:Panel ID="Panel100" runat="server"  BorderColor="#0033CC" 
        BorderStyle="Solid">
           <asp:Label ID="lblError" runat="server" Text="" EnableViewState="False"></asp:Label>
           
        <asp:Table ID="Table1" runat="server" CellSpacing="20" GridLines="Both" 
               Width="1875px">
               <asp:TableRow ID="r1" style="vertical-align:top" runat="server">
                   <asp:TableCell ID="c1" runat="server" HorizontalAlign="Left">
                       <table style="width: 350px;">
            <tr style="vertical-align:top;">
                <td  rowspan="2" class="auto-style1"><asp:GridView ID="GridView1" runat="server" 
                        AutoGenerateColumns="False" visible="false" Width="126px" >
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
        </asp:GridView></td>
                <td> <asp:GridView ID="GridViewImage" runat="server" AutoGenerateColumns="False" Visible="false" >
            <Columns>
            
                <asp:BoundField DataField="Energy" HeaderText="Select Imaging" 
                    SortExpression="Energy" />
                    <asp:TemplateField>  
                             
            <ItemTemplate>
            <asp:CheckBox runat="server" ID="RowlevelCheckBoxImage" />

            </ItemTemplate>
                      </asp:TemplateField>
            </Columns>
        </asp:GridView></td>
                
            </tr>
            <tr>
                
                <td>
                    <asp:Button ID="engHandoverButton" runat="server" BackColor="#FFCC00" 
                        causesvalidation="false" Height="100px" Text="" />
                </td>
               
            </tr>
            <tr>
                <td colspan="2"><table style="width: 350px;">
                           <tr>
                               <td><asp:Literal ID="Literal1" runat="server" Text="Runup Comments"></asp:Literal></td>
                                                          </tr>
                           <tr>
                               <td><uc3:CommentBoxuc ID="CommentBox" runat="server" /></td>
                               
                           </tr>
                       </table></td>
                              
            </tr><tr>
                <td colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
               <ContentTemplate>
                   <asp:Label ID="Label1" runat="server" Text="Major Faults Cleared Today:"></asp:Label>
                   
               <asp:PlaceHolder ID="PlaceHolderTodaysclosedfaults" runat="server">
                </asp:PlaceHolder>
                </ContentTemplate></asp:UpdatePanel>
                </td>
                
            </tr>
        </table>
                      <%-- <table style="width: 350px;">
                           <tr>
                               <td rowspan="2">
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" visible="false" >
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
                               </td>
                               <td>
                                   <asp:GridView ID="GridViewImage" runat="server" AutoGenerateColumns="False" Visible="false" >
            <Columns>
            
                <asp:BoundField DataField="Energy" HeaderText="Select Imaging" 
                    SortExpression="Energy" />
                    <asp:TemplateField>  
                             
            <ItemTemplate>
            <asp:CheckBox runat="server" ID="RowlevelCheckBoxImage" />

            </ItemTemplate>
                      </asp:TemplateField>
            </Columns>
        </asp:GridView>
                               </td>
                               </tr>
                           <tr>
                               <td>
                                   </td>
                           </tr>
                          
                       </table>
                       <table style="width: 350px;">
                           <tr>
                               <td><asp:Literal ID="Literal1" runat="server" Text="Runup Comments"></asp:Literal></td>
                                                          </tr>
                           <tr>
                               <td><uc3:CommentBoxuc ID="CommentBox" runat="server" /></td>
                               
                           </tr>
                       </table>--%>
                   </asp:TableCell>
                                  <%-- <asp:TableCell ID="c2" runat="server" Width="50px">--%>
                       
                   <%--<asp:Button ID="engHandoverButton" runat="server" Text="" causesvalidation="false"  Height="150px" BackColor="#FFCC00"/>--%>

<%--</asp:TableCell>--%>
                                          <asp:TableCell ID = "c4" runat="server" HorizontalAlign="Left" Width="500px">
                       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<asp:PlaceHolder ID="PlaceHolderDefectSave" runat="server">
</asp:PlaceHolder>
</ContentTemplate>
</asp:UpdatePanel>
                       </asp:TableCell>
                        <asp:TableCell VerticalAlign="Top">
                             <asp:UpdatePanel ID="UpdatePanel3" runat="server" Visible="true">
<ContentTemplate>
    <asp:PlaceHolder ID="PlaceHolderViewopenfaults" runat="server">
    </asp:PlaceHolder>

</ContentTemplate> 
</asp:UpdatePanel>
<%--<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
               <ContentTemplate>
                   <asp:Label ID="Label1" runat="server" Text="Major Faults Cleared Today:"></asp:Label>
                   
               <asp:PlaceHolder ID="PlaceHolderTodaysclosedfaults" runat="server">
                </asp:PlaceHolder>
                </ContentTemplate></asp:UpdatePanel>--%>
               </asp:TableCell>
               </asp:TableRow>
                          </asp:Table>
                          <asp:Table ID="Table2" runat="server" CellSpacing="20" GridLines="Both" Width="1507px">
         <asp:TableRow ID ="t2r1" runat="server">
         <asp:TableCell ID="lock" runat="server" Width="160px" HorizontalAlign="Left">
         <asp:Button ID="LockElf" runat="server" Text="Lock Elf/Switch User" causesvalidation="false"/>
         </asp:TableCell>
         <asp:TableCell ID="t2c1" runat="server" Width="160px" HorizontalAlign="Left">

               </asp:TableCell>
               <asp:TableCell ID="t2c2" runat="server" Width="50px">
               <asp:Button ID="ViewAtlasButton" runat="server" Text="View Atlas Energies" width = "160px" causesvalidation="false"/>
               </asp:TableCell>
               <asp:TableCell ID="TableCell1" runat="server">
                    <asp:Button ID="PhysicsQA" runat="server" Text="View Physics Energies/Imaging" CausesValidation="false" />
                </asp:TableCell>
               <asp:TableCell ID="t2c3" runat="server">
               <asp:Button ID="LogOffButton" runat="server" Text="Log Off Without Approving Energies" CausesValidation="False" />
               </asp:TableCell>
              
                        </asp:TableRow>
               </asp:Table>
               </asp:Panel>
           </div>         

 <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server" Visible="true">
<ContentTemplate>
    <asp:PlaceHolder ID="PlaceHolderViewopenfaults" runat="server">
    </asp:PlaceHolder>

</ContentTemplate> 
</asp:UpdatePanel>--%>
<asp:UpdatePanel ID="UpdatePanelQA" runat="server" Visible="false">
<ContentTemplate>
    <asp:PlaceHolder ID="PlaceHolderModalities" runat="server">
    </asp:PlaceHolder>
        </ContentTemplate>
        
</asp:UpdatePanel>
 
<asp:UpdatePanel ID="UpdatePanelatlas" runat="server" Visible="false">
     <ContentTemplate>
         <asp:PlaceHolder ID="PlaceHolderAtlas" runat="server"></asp:PlaceHolder>
     </ContentTemplate>
</asp:UpdatePanel>

 <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
            <asp:PlaceHolder ID="PlaceHolderWriteData" runat="server">
            <uc4:WriteDatauc ID="WriteDatauc1" LinacName="Linac" UserReason="0"  Tabby="TabNumber"  WriteName="EngData"   Visible="false" runat="server" />
            </asp:PlaceHolder>
            </ContentTemplate>
            </asp:UpdatePanel>
                        
 <div style="background-color: #FFFF66; background-repeat: no-repeat; border-style: solid; border-width: thin">  
   
 <uc5:ViewCommentsuc ID="ViewCommentsuc1"  LinacName="" CommentSort="er" runat="server" />
</div>
<asp:PlaceHolder ID="PlaceHolderConfirmPage" runat="server">
        
            <uc1:ConfirmPage ID="ConfirmPage1" Visible="false" runat="server" />
         </asp:PlaceHolder>



 






            
            



        
 

            
            



        
 

            
            



        
 

            
            



        
 






            
            



        
 

            
            



        
 

            
            



        
 

            
            



        
 
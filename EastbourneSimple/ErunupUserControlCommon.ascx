<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ErunupUserControlCommon.ascx.vb" Inherits="ErunupUserControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register src="AtlasEnergyViewuc.ascx" tagname="AtlasEnergyViewuc" tagprefix="uc2" %>


<%--<%@ Register src="UpDateIncidentuc.ascx" tagname="UpDateIncidentuc" tagprefix="uc3" %>--%>


<%@ Register src="ViewCommentsuc.ascx" tagname="ViewCommentsuc" tagprefix="uc5" %>



<%@ Register src="DefectSave.ascx" tagname="DefectSave" tagprefix="uc6" %>



<%@ Register src="TodayClosedFault.ascx" tagname="TodayClosedFault" tagprefix="uc7" %>



<%--<%@ Register src="Traininguc.ascx" tagname="Traininguc" tagprefix="uc8" %>--%>


<%@ Register src="LockElfuc.ascx" tagname="LockElfuc" tagprefix="uc9" %>


<%@ Register src="WebUserControl2.ascx" tagname="WebUserControl2" tagprefix="uc10" %>


<%@ Register src="DefectSavePark.ascx" tagname="DefectSavePark" tagprefix="uc11" %>


<%@ Register src="AcceptLinac.ascx" tagname="AcceptLinac" tagprefix="uc12" %>


<%--<%@ Register src="CommitData.ascx" tagname="CommitData" tagprefix="uc3" %>--%>


<%@ Register src="WriteDatauc.ascx" tagname="WriteDatauc" tagprefix="uc4" %>
<%@ Register src="ConfirmPage.ascx" tagname="ConfirmPage" tagprefix="uc1" %>


<link href="App_Themes/Blue/Elf.css" rel="stylesheet" type="text/css" />

<%@ Register src="ViewOpenFaults.ascx" tagname="ViewOpenFaults" tagprefix="uc1" %>


<%--<%@ Register src="Singlemachinefaultuc.ascx" tagname="Singlemachinefaultuc" tagprefix="uc2" %>--%>
<uc9:LockElfuc ID="LockElfuc1" LinacName="" UserReason="1" Tabby="1" visible="false" runat="server" />
<script type="text/javascript">
    function Count() {
         var i = document.getElementById("CommentBox").value.length;
        if (i < 21)
            document.getElementById("Label2").innerHTML = 20 - i;
        else
            document.getElementById("Label2").innerHTML = "You have exceeded the max text please delete some characters";
        return;
    }
    
   
//doesn't work
function checkLength(oObject) 
{
if (oObject.value.length<10) 
return true;
else  
{ 
if ((event.keyCode>=37 && event.keyCode<=40) || (event.keyCode==8) || (event.keyCode==46)) 
event.returnValue = true;

else
event.returnValue = false;  
}

}






</script>
<asp:Label ID="CheckUser" runat="server" Text=""  visible="true" display="none" causesvalidation="false"></asp:Label>
 <asp:GridView ID="DummyGridView" runat="server">
        </asp:GridView>     

 <div>
 
       <asp:Panel ID="Panel100" runat="server" BackColor="#99CCFF" BorderColor="#0033CC" 
        BorderStyle="Solid">
           <asp:Label ID="lblError" runat="server" Text="" EnableViewState="False"></asp:Label>
           
        <asp:Table ID="Table1" runat="server" CellSpacing="20" GridLines="Both" 
               Width="1500px">
               <asp:TableRow ID="r1" runat="server">
                   <asp:TableCell ID="c1" runat="server" Width="250px" HorizontalAlign="Left">
                   <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" visible="false">
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
                   <asp:TableCell ID="imagingcell" runat="server" Width =" 250px" HorizontalAlign="Left">
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
                   </asp:TableCell>
                        

                   
                   <asp:TableCell ID="c2" runat="server" Width="50px">
                       
                   <asp:Button ID="engHandoverButton" runat="server" Text="Approve Energies" causesvalidation="false"  Height="150px" BackColor="#FFCC00"/>


</asp:TableCell>
                   <asp:TableCell ID="c3" runat="server" HorizontalAlign="left" Width="250px">
                               <%--<legend align="top" style="font-family: Arial, Helvetica, sans-serif; font-weight: bold">Run-up Comments</legend>--%>
                             <asp:TextBox ID="CommentBox" runat="server" MaxLength="10" Rows="5"  
                TextMode="Multiline" Width="150px" Height="300px" onkeyup="Count()" ClientIDMode="Static"></asp:TextBox>
                       <br/> <asp:Label ID="Label2" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                      
<%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="CommentBox" ErrorMessage="Only 250 characters"> </asp:RequiredFieldValidator>--%>
                       </asp:TableCell>
                       <asp:TableCell ID = "c4" runat="server" HorizontalAlign="Left" Width="375px">
                       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<asp:PlaceHolder ID="PlaceHolderDefectSave" runat="server">
</asp:PlaceHolder>
</ContentTemplate>

</asp:UpdatePanel>
                       </asp:TableCell>
                        <asp:TableCell VerticalAlign="Top">
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
               <ContentTemplate>
                   <asp:Label ID="Label1" runat="server" Text="Major Faults Cleared Today:"></asp:Label>
                   
               <asp:PlaceHolder ID="PlaceHolderTodaysclosedfaults" runat="server">
                </asp:PlaceHolder>
                </ContentTemplate></asp:UpdatePanel>
               </asp:TableCell>
               </asp:TableRow>
                          </asp:Table>
                          <asp:Table ID="Table2" runat="server" CellSpacing="20" GridLines="Both" Width="1507px">
         <asp:TableRow ID ="t2r1" runat="server">
         <asp:TableCell ID="lock" runat="server" Width="160px" HorizontalAlign="Left">
         <asp:Button ID="LockElf" runat="server" Text="Lock Elf/Switch User" causesvalidation="false"/>
         </asp:TableCell>
         <asp:TableCell ID="t2c1" runat="server" Width="160px" HorizontalAlign="Left">
         <%--<asp:Button ID="FaultPanelButton" runat="server" Text="View Open Faults" causesvalidation ="false" Visible="false"/>--%>
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

  <asp:UpdatePanel ID="UpdatePanel3" runat="server" Visible="true">
<ContentTemplate>
    <asp:PlaceHolder ID="PlaceHolderViewopenfaults" runat="server">
    </asp:PlaceHolder>

</ContentTemplate> 
</asp:UpdatePanel>
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



 






            
            



        
 

            
            



        
 

            
            



        
 

            
            



        
 






            
            



        
 

            
            



        
 

            
            



        
 

            
            



        
 
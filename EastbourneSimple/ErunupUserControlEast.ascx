<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ErunupUserControlEast.ascx.vb" Inherits="ErunupUserControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register src="AtlasEnergyViewuc.ascx" tagname="AtlasEnergyViewuc" tagprefix="uc2" %>


<%@ Register src="UpDateIncidentuc.ascx" tagname="UpDateIncidentuc" tagprefix="uc3" %>


<%@ Register src="ViewCommentsuc.ascx" tagname="ViewCommentsuc" tagprefix="uc5" %>



<%@ Register src="DefectSave.ascx" tagname="DefectSave" tagprefix="uc6" %>



<%@ Register src="TodayClosedFault.ascx" tagname="TodayClosedFault" tagprefix="uc7" %>



<%@ Register src="Traininguc.ascx" tagname="Traininguc" tagprefix="uc8" %>


<%@ Register src="LockElfuc.ascx" tagname="LockElfuc" tagprefix="uc9" %>


<%@ Register src="WebUserControl2.ascx" tagname="WebUserControl2" tagprefix="uc10" %>


<%--<%@ Register src="CommitData.ascx" tagname="CommitData" tagprefix="uc3" %>--%>


<%@ Register src="WriteDatauc.ascx" tagname="WriteDatauc" tagprefix="uc4" %>
<%@ Register src="ConfirmPage.ascx" tagname="ConfirmPage" tagprefix="uc1" %>


<link href="App_Themes/Blue/Elf.css" rel="stylesheet" type="text/css" />

<%@ Register src="ViewOpenFaults.ascx" tagname="ViewOpenFaults" tagprefix="uc1" %>
<%@ Register src="Singlemachinefaultuc.ascx" tagname="Singlemachinefaultuc" tagprefix="uc2" %>
<uc9:LockElfuc ID="LockElfuc1" LinacName="" UserReason="1" Tabby="1" visible="false" runat="server" />

<asp:Label ID="CheckUser" runat="server" Text=""  visible="true" display="none" causesvalidation="false"></asp:Label>
      

        
            <script type="text/javascript">
                //    function pageLoad() {
                //    var popup = $find('check_ModalPopupExtender');
                //popup.add_showing(calledWhenShown);
                //        if (popup != null) {
                //            popup.show();
                //        }
                //     }

               
                function yaag() {

                    
                    pipup.add_showing(calledWhenShown);
                    return true;
                }
                


                function runup() {
                    var apopup = $find('check_ModalPopupExtender');

                    apopup.add_showing(rkunup);

                }
                function rkunup() {
                    alert('I am called!');
                }



                function calledWhenShown() {

                    alert('I am called when the ModalPopup is shown');

                }

                function ClearUI() {
                    $find("textvalidator1").hide();
                    $get("txtUsername").value = "";
                }

                function IsValid() {
                    var textbox = $get("txtUsername");
                    if (textbox.value == "") {
                        return false;
                    }
                    else
                        return true;
                }
                function ClosePopup() {
                    if (IsValid()) {
                        
                        alert("You have given your name");
                        ClearUI();
                    }
                }

                function DoClose() {
                    // close the modal popup
                    
                    
                    $find('Buggerit_ModalPopupExtender').hide();

                    // return true so that submit will happen
                    return true;
                }
                function CheckAllEmp(Checkbox) {
                    var GridView1 = document.getElementById("<%=GridView1.ClientID %>");
                    for (i = 1; i < GridView1.rows.length; i++) {
                        if (GridView1.rows[i].cells[1].getElementsByTagName("RowlevelCheckBox")[0].enabled) {
                            GridView1.rows[i].cells[1].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
                        }
                    }
                }



            </script>

 <div>
 
       <asp:Panel ID="Panel100" runat="server" BackColor="#99CCFF" BorderColor="#0033CC" 
        BorderStyle="Solid">
           <asp:Label ID="lblError" runat="server" Text="" EnableViewState="False"></asp:Label>
        <asp:Table ID="Table1" runat="server" CellSpacing="20" GridLines="Both" 
               Width="1500px">
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
                   <asp:TableCell ID="imagingcell" runat="server" Width =" 250px" HorizontalAlign="Left">
                       <asp:GridView ID="GridViewImage" runat="server" AutoGenerateColumns="False" >
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
                       
                   <asp:Button ID="engHandoverButton" runat="server" Text="Approve Energies" causesvalidation="false"  Height="150px" BackColor="#FFCC00" />


</asp:TableCell>
                   <asp:TableCell ID="c3" runat="server" HorizontalAlign="left" Width="250px">
                               <%--<legend align="top" style="font-family: Arial, Helvetica, sans-serif; font-weight: bold">Run-up Comments</legend>--%>
                             <asp:TextBox ID="CommentBox" runat="server" MaxLength="250" Rows="5" 
                TextMode="multiline"  Width="100px" Height="100px" AutoPostBack="true"></asp:TextBox>

                       </asp:TableCell>
                       <asp:TableCell ID = "c4" runat="server" HorizontalAlign="Left" Width="375px">
                       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<asp:PlaceHolder ID="PlaceHolder1" runat="server">
</asp:PlaceHolder>
</ContentTemplate>

</asp:UpdatePanel>
                       </asp:TableCell>
                        <asp:TableCell VerticalAlign="Top">
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
               <ContentTemplate>
                   <asp:Label ID="Label1" runat="server" Text="Major Faults Cleared Today:"></asp:Label>
                   <br></br>
               <asp:PlaceHolder ID="PlaceHolder5" runat="server">
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
         <asp:Button ID="FaultPanelButton" runat="server" Text="View Open Faults" causesvalidation ="false"/>
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

  <asp:UpdatePanel ID="UpdatePanel3" runat="server" Visible="false">
<ContentTemplate>
    <asp:PlaceHolder ID="PlaceHolder3" runat="server">
    </asp:PlaceHolder>

</ContentTemplate> 
</asp:UpdatePanel>
<asp:UpdatePanel ID="UpdatePanelQA" runat="server" Visible="false">
<ContentTemplate>
    <asp:PlaceHolder ID="PlaceHolder6" runat="server">
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
            <uc4:WriteDatauc ID="WriteDatauc1" LinacName="Linac" UserReason="0"  Tabby="TabNumber"  WriteName="EngData"   Visible="false" runat="server" />
            </asp:PlaceHolder>
            </ContentTemplate>
            </asp:UpdatePanel>
            
            
 <div style="background-color: #FFFF66; background-repeat: no-repeat; border-style: solid; border-width: thin">  
          
      
 <uc5:ViewCommentsuc ID="ViewCommentsuc1"  LinacName="" CommentSort="er" runat="server" />
</div>
<asp:PlaceHolder ID="PlaceHolder7" runat="server">
        
            <uc1:ConfirmPage ID="ConfirmPage1" Visible="false" runat="server" />
         </asp:PlaceHolder>
 






            
            



        
 

            
            



        
 

            
            



        
 

            
            



        
 






            
            



        
 

            
            



        
 

            
            



        
 

            
            



        
 
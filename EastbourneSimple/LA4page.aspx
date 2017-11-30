<%@ Page Title="" Language="VB" MasterPageFile="~/Elf.master"  AutoEventWireup="false" CodeFile="LA4page.aspx.vb" Inherits="LA4page"  %>
<%@ MasterType VirtualPath="~/Elf.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>



<%@ Register src="ErunupUserControl.ascx" tagname="ErunupUserControl" tagprefix="uc1" %>
<%@ Register src="Preclinusercontrol.ascx" tagname="Preclinusercontrol" tagprefix="uc2" %>
<%@ Register src="ClinicalUserControl.ascx" tagname="ClinicalUserControl" tagprefix="uc3" %>

<%@ Register src="AcceptLinac.ascx" tagname="AcceptLinac" tagprefix="uc4" %>

<%@ Register src="LinacStatusuc.ascx" tagname="LinacStatusuc" tagprefix="uc5" %>

<%@ Register src="PlannedMaintenanceuc.ascx" tagname="PlannedMaintenanceuc" tagprefix="uc6" %>

<%@ Register src="Repairuc.ascx" tagname="Repairuc" tagprefix="uc7" %>

<%@ Register src="WebUserControl2.ascx" tagname="WebUserControl2" tagprefix="uc8" %>

<%@ Register src="WriteDatauc.ascx" tagname="WriteDatauc" tagprefix="uc9" %>

<%@ Register src="PhysicsQAuc.ascx" tagname="PhysicsQAuc" tagprefix="uc10" %>

<%@ Register src="Emergencyrunupuc.ascx" tagname="Emergencyrunupuc" tagprefix="uc11" %>

<%@ Register src="Traininguc.ascx" tagname="Traininguc" tagprefix="uc12" %>




<%@ Register src="RegisterUseruc.ascx" tagname="RegisterUseruc" tagprefix="uc13" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<script type="text/javascript">
    // JScript File

    // finds a control that had the given server id, of a the given type
    // in the given parent.          

    function findControl(parent, tagName, serverId) {
        var items = parent.getElementsByTagName(tagName);
        // walk the items looking for the right guy
        for (var i = 0; i < items.length; i++) {
            var ctl = items[i];
            if (ctl && ctl.id) {
                // check the end of the name.
                //
                var subId = ctl.id.substring(ctl.id.length - serverId.length);
                if (subId == serverId) {
                    return ctl;
                }
            }
        }
        return null;
    }



    function loadTabPanel(sender, e) {
        
        var tabContainer = sender;

        if (tabContainer) {
            
            var updateControlId = "TabButton" + tabContainer.get_activeTabIndex();
            // get the active tab and find our button
            //
            var activeTab = tabContainer.get_activeTab();


            // check to see if we've already loaded
            //
            if (findControl(activeTab.get_element(), "div", "TabContent" + tabContainer.get_activeTabIndex())) return;


            var updateControl = findControl(activeTab.get_element(), "input", updateControlId);

            if (updateControl) {
               
                // fire the update

                updateControl.click();



            }

        }

    }

  </script>

    

    <asp:UpdatePanel ID="UpdatePanel8" runat="server"><ContentTemplate>
    <div>
    <%--<asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>--%>
         <table style="width: 100%;">
            <tr>
                <td>
    <asp:Label ID="CurrentStateLabel" runat="server" Text="Current Linac State: " 
            Height="50px" Width="1200px"  Font-Size="XX-Large" BackColor="White" 
            ForeColor="Black"></asp:Label>
<asp:Label ID="Statelabel" runat="server" Text="" Height="50px" Width="1200px"  Font-Size="XX-Large" BackColor="White" ForeColor="Black"></asp:Label>
        <asp:Label ID="CurrentActivityLabel" runat="server" Text="Current Activity: " Height="20px" Width="1200px"  Font-Size="Large" BackColor="#3399FF" ForeColor="#FFFF66"></asp:Label>
<asp:Label ID="ActivityLabel" runat="server" Text="" Height="25px" Width="1200px"  Font-Size="Large" BackColor="#3399FF" ForeColor="#FFFF66"></asp:Label>
        <asp:Label ID="CurrentUserGroupLabel" runat="server" Text="Current User: " Height="20px" Width="1200px"  Font-Size="Large" BackColor="#3399FF" ForeColor="#FFFF66"></asp:Label>
<asp:Label ID="UserGroupLabel" runat="server" Text="" Height="40px" Width="1200px"  Font-Size="Large" BackColor="#3399FF" ForeColor="#FFFF66"></asp:Label>
<asp:Label ID="Label4" runat="server" Text="ELF IP Address: " Height="20px" Width="1200px"  Font-Size="Large" BackColor="#3399FF" ForeColor="#FFFF66"></asp:Label>
<asp:Label ID="Label5" runat="server" Text="" Height="40px" Width="1200px"  Font-Size="Large" BackColor="#3399FF" ForeColor="#FFFF66"></asp:Label>
                     </td>
                <td>
                    
                </td>
                <td>
                    <asp:Image id="Image2" runat="server"
            ImageUrl="~/Images/bsuh_logo.gif" Width="250" Height="74"
            AlternateText="BSUH Linacs" />
                    <br>
            </br>
             <asp:Image id="Image1" runat="server"
            ImageUrl="~/Images/if_elf_62126.png" Width="100px" Height="100px"
            AlternateText="ELF" />
            <asp:Label ID="Label15" runat="server" Text="Software Version 4.0"></asp:Label>
                </td>
            </tr>
                   </table>
<%--This is an instrumentation label associated with updatehiddenLAfield--%>
<%--<asp:Label ID="Application1Label" runat="server" Text="Current Application value: " Height="20px" Width="1200px"  Font-Size="Large" BackColor="#3399FF" ForeColor="#FFFF66"></asp:Label>
--%>
<%--<asp:Label ID="Application1" runat="server" Text="" Height="40px" Width="1200px"  Font-Size="Large" BackColor="#3399FF" ForeColor="#FFFF66"></asp:Label>
--%>

<%--This is instrumentation code
<asp:Label ID="UserLabelText" runat="server" Text="Current User: " Height="20px" Width="1200px"  Font-Size="Large" BackColor="#3399FF" ForeColor="#FFFF66"></asp:Label>
<asp:Label ID="UserLabel" runat="server" Text="" Height="40px" Width="1200px"  Font-Size="Large" BackColor="#3399FF" ForeColor="#FFFF66"></asp:Label>--%>
</div>
    <asp:HiddenField ID="LAHiddenFieldcontrol" runat="server" />

    <%--<asp:TextBox ID="LAFieldcontrol" runat="server"></asp:TextBox>--%>

    </ContentTemplate>
    </asp:UpdatePanel>
      <asp:Timer ID="Timer1" runat="server" Interval="7200000">
      
    </asp:Timer>
    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
    </Triggers>
    <ContentTemplate>
    <div>
     <asp:Label ID="Label2" runat="server"></asp:Label><br />
     <asp:Label ID="Label1" runat="server"></asp:Label>
                <br />
     <asp:Label ID="Label3" runat="server" ></asp:Label><br />
     </div>
    </ContentTemplate>
        
       
    </asp:UpdatePanel>
    
   
   
<asp:Button ID="ReportFault" runat="server" BackColor="#FF3300" ForeColor="#FFFF66" 
        Height="25px" Text="Report Fault" Width="150px" CausesValidation="False" 
                Font-Bold="True" Font-Size="Medium" />

    
  <asp:Button ID="EndOfDay" runat="server" Text="End of Day"  causesvalidation="false"/>              
&nbsp;
<%--<asp:Button ID="RegisterUser" runat="server" Text="Register" causesvalidation="false"/>
    <asp:Button ID="Admin" runat="server" CausesValidation="False" Text="Admin" />
    
    <asp:Button ID="ViewFaultButton" runat="server" visible="true" CausesValidation="False" 
        style="height: 26px" Text="View Faults" />--%>

            <asp:Button ID="RestoreButton" runat="server" visible="true" CausesValidation="False" 
        style="height: 26px" Text="RESTORE ELF" />
                   <br />
    <input id="inpHide" type="hidden" runat="server" value="9" />
        <br />

    

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
             <asp:PlaceHolder ID="PlaceHolder4" runat="server">
            <uc9:WriteDatauc ID="WriteDatauc1" LinacName="LA4" UserReason="10"  Tabby="EndDay"  WriteName="EndDayData"   Visible="False" runat="server" />
            <%--<uc4:WriteDatauc ID="WriteDatauc1" LinacName="Linac" UserReason="0"  Tabby="TabNumber"  WriteName="EngData"   Visible="false" runat="server" />--%>
            </asp:PlaceHolder>
            </ContentTemplate>
            </asp:UpdatePanel>


            
    <asp:TabContainer ID="tcl" runat="server"  activetabindex="0" 
          OnClientActiveTabChanged="loadTabPanel"  height="930px" >
         
<asp:TabPanel runat="server" HeaderText="LA4 Status" ID="TabPanel0"><HeaderTemplate>
LA4 Status
</HeaderTemplate>
<ContentTemplate>
<asp:UpdatePanel ID="linacstatus" runat="server" >
    <ContentTemplate>
<%--<uc4:AcceptLinac ID="AcceptLinac0" LinacName= "LA4"  UserReason = "1"  Tabby="0" runat="server" />--%>
        <asp:UpdatePanel ID="UpdatePanel0" runat="server" >
            <ContentTemplate>
            <asp:Button ID="TabButton0" runat="server"  OnClick="TabButton_Click"  style="display:none;" CausesValidation="false"/>
            <asp:Panel ID="Panel0" runat="server" >
            <%--<asp:Button ID="EndOfDay" runat="server" Text="End of Day" visible="false" causesvalidation="false" Width="200px" Height="100px"/>--%>
                    <uc5:LinacStatusuc ID="LinacStatusuc1" LinacName="LA4" runat="server" />
                    </asp:Panel></ContentTemplate></asp:UpdatePanel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:TabPanel>

<%--This is the engineering run up tab--%>
        
<asp:TabPanel ID="TabPanel1" runat="server" HeaderText="LA4 Engineering Runup" DynamicContextKey='Engrunup' CssClass="ajax__tab_header"><ContentTemplate>
<asp:UpdatePanel ID="signin" runat="server"
><ContentTemplate>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" >
<ContentTemplate>
<asp:Button ID="TabButton1" runat="server"  OnClick="TabButton_Click"  style="display:none;" CausesValidation="false"/>
<asp:Panel ID="TabContent1" runat="server" Visible="False">
<uc4:AcceptLinac ID="AcceptLinac1" runat="server" LinacName= "LA4"  Tabby="1" UserReason = "1" visible="false" />
<uc1:ErunupUserControl ID="ErunupUserControl1" LinacName="LA4" Tabby = "1" UserReason = "1" DataName="EngData" visible="false" runat="server"  />
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
    





</asp:TabPanel>
        
<%--  This is the Pre-clinical tab  --%>    
        
        
<asp:TabPanel ID="TabPanel2" runat="server" HeaderText="LA4 Pre-clinical Runup" >
        <ContentTemplate>
<asp:UpdatePanel ID="updatemod2" runat="server" >
<ContentTemplate>
<asp:UpdatePanel ID="UpdatePaneln" updatemode ="Conditional"  runat="server">
<ContentTemplate><asp:Button ID="TabButton2" runat="server"  OnClick="TabButton_Click"  style="display:none;" CausesValidation="false"/>
<asp:Panel ID="TabContent2" runat="server" Visible="false"
><uc4:AcceptLinac ID="AcceptLinac2" LinacName="LA4" UserReason="2" Tabby="2" runat="server" visible="false"/>
<uc2:Preclinusercontrol ID="Preclinusercontrol1" LinacName = "LA4" DataName="PreData" runat="server"/>
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:TabPanel>

<%-- This is the Clinical Tab --%>
        
<asp:TabPanel ID="TabPanel3" runat="server" HeaderText="LA4 Clinical">
        <ContentTemplate>
<asp:UpdatePanel ID="UpdatePanel3" runat="server" >
<ContentTemplate><asp:UpdatePanel ID="UpdatePanelClinical" Updatemode="Conditional" runat="server">
<ContentTemplate>
<asp:Button ID="TabButton3" runat="server" OnClick="TabButton_click" Style="Display: none" CausesValidation="false"/>
<asp:Panel ID="TabContent3" runat="server" Visible="false">
<uc3:ClinicalUserControl ID="ClinicalUserControl1"  LinacName="LA4" DataName="ClinData" runat="server" visible="false"/>
<uc4:AcceptLinac ID="AcceptLinac3" LinacName="LA4" UserReason="3" Tabby="3" runat="server"  />
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:TabPanel>
        
        
<asp:TabPanel ID="TabPanel4" runat="server" HeaderText="LA4 Planned Maintenance" >
        <ContentTemplate>
<asp:UpdatePanel ID="UpdatePanel4" runat="server" >
<ContentTemplate>
<asp:UpdatePanel ID="UpdatePanelMaintenance" runat="server">
<ContentTemplate><asp:Button ID="TabButton4" runat="server" OnClick="TabButton_click" Style="Display: none" CausesValidation="false"/>
<asp:Panel ID="TabContent4" runat="server" Visible="false">
<uc4:AcceptLinac ID="AcceptLinac4" LinacName="LA4" UserReason="4" Tabby="4" runat="server" visible="false"/>
<uc6:PlannedMaintenanceuc ID="PlannedMaintenanceuc1" linacname="LA4" runat="server" Visible="false" />
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:TabPanel>
  
        
  <asp:TabPanel ID="TabPanel5" runat="server" HeaderText="LA4 Repair" >
        <ContentTemplate>
<asp:UpdatePanel ID="UpdatePanel5" runat="server" >
<ContentTemplate>
<asp:UpdatePanel ID="UpdateRepair" runat="server">
<ContentTemplate>
<asp:Button ID="TabButton5" runat="server" OnClick="TabButton_click" Style="Display: none" CausesValidation="false"/>
<asp:Panel ID="TabContent5" runat="server" Visible="false">
<uc4:AcceptLinac ID="AcceptLinac5" LinacName="LA4" UserReason="5" Tabby="5" runat="server" visible="false"/>
<uc7:Repairuc ID="Repairuc1" LinacName="LA4" runat="server" Visible="false" />
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:TabPanel>

 <asp:TabPanel ID="TabPanel6" runat="server" hidden="true" enabled="false" >
        <ContentTemplate>
<asp:UpdatePanel ID="UpdatePanel6" runat="server" >
<ContentTemplate>
<asp:UpdatePanel ID="UpdatePhysicsQA" runat="server">
<ContentTemplate>
<asp:Button ID="Button1" runat="server" OnClick="TabButton_click" Style="Display: none" CausesValidation="false"/>
<asp:Panel ID="TabContent6" runat="server" Visible="false"><uc4:AcceptLinac ID="AcceptLinac6" LinacName="LA4" UserReason="6" Tabby="6" runat="server" visible="false"/>
<%--<uc8:WebUserControl2 ID="WebUserControl21" LinacName="LA4" runat="server" Visible="false" />--%>
 <uc10:PhysicsQAuc ID="PhysicsQAuc1" LinacName="LA4"  Visible="false" runat="server" />
 </asp:Panel>
 </ContentTemplate>
 </asp:UpdatePanel>
 </ContentTemplate>
 </asp:UpdatePanel>
</ContentTemplate>
</asp:TabPanel>

<asp:TabPanel ID="TabPanel7" runat="server" HeaderText="LA4 Emergency Runup" DynamicContextKey='Emerunup' CssClass="ajax__tab_header">
     <ContentTemplate>
<asp:UpdatePanel ID="UpdatePanel7" runat="server">
<ContentTemplate>
<asp:UpdatePanel ID="UpdatePanelEmergency" runat="server" >
<ContentTemplate><asp:Button ID="TabButton7" runat="server"  OnClick="TabButton_Click"  style="display:none;" CausesValidation="false"/>
<asp:Panel ID="TabContent7" runat="server" Visible="False">
<uc4:AcceptLinac ID="AcceptLinac7" runat="server" LinacName= "LA4"  Tabby="7" UserReason = "9" visible="false" />
<uc1:ErunupUserControl ID="ErunupUserControl2" LinacName="LA4" Tabby = "7" UserReason = "9" DataName="EmeData" visible="false" runat="server"  />
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
 </asp:TabPanel>

<asp:TabPanel ID="TabPanel8" runat="server" HeaderText="LA4 Development/Training" DynamicContextKey='Devel' CssClass="ajax__tab_header"><ContentTemplate>
<asp:UpdatePanel ID="UpdateDevel" runat="server">
<ContentTemplate>
<asp:Button ID="TabButton8" runat="server"  OnClick="TabButton_Click"  style="display:none;" CausesValidation="false"/>
<asp:Panel ID="TabContent8" runat="server" Visible="False">
<uc4:AcceptLinac ID="AcceptLinac8" runat="server" LinacName= "LA4"  Tabby="8" UserReason = "8" visible="false" />
<uc12:Traininguc ID="Traininguc1" LinacName = "LA4" Tabby="8" UserReason="8" Visible="false" runat="server" />
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:TabPanel>      
    </asp:TabContainer>
</asp:Content>


﻿<%@ Page Title="" Language="VB" MasterPageFile="~/Elf.master"  AutoEventWireup="false" CodeFile="E2page.aspx.vb" Inherits="E2page"  %>
<%@ MasterType VirtualPath="~/Elf.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="ErunupUserControlCommon.ascx" tagname="ErunupUserControlCommon" tagprefix="uc1" %>
<%@ Register src="Preclinusercontrol.ascx" tagname="Preclinusercontrol" tagprefix="uc2" %>
<%@ Register src="ClinicalUserControl.ascx" tagname="ClinicalUserControl" tagprefix="uc3" %>

<%@ Register src="AcceptLinac.ascx" tagname="AcceptLinac" tagprefix="uc4" %>

<%@ Register src="LinacStatusuc.ascx" tagname="LinacStatusuc" tagprefix="uc5" %>

<%@ Register src="PlannedMaintenanceuc.ascx" tagname="PlannedMaintenanceuc" tagprefix="uc6" %>

<%@ Register src="Repairuc.ascx" tagname="Repairuc" tagprefix="uc7" %>

<%@ Register src="WriteDatauc.ascx" tagname="WriteDatauc" tagprefix="uc9" %>


<%@ Register src="Traininguc.ascx" tagname="Traininguc" tagprefix="uc12" %>

<%@ Register src="RegisterUseruc.ascx" tagname="RegisterUseruc" tagprefix="uc13" %>
<%@ Register src="controls/ModalityDisplayuc.ascx" tagname="ModalityDisplayuc" tagprefix="uc8" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="ContenE2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


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
 <div class="gridheader">
	

    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
        <ContentTemplate>
    <div>
	
        <table style="width: 100%;">
            <tr>
                <td style="width: 260px"> <asp:Label ID="CurrentStateLabel" runat="server" Text="Current Linac State: " 
																				  
                                Height="50px"  Font-Size="XX-Large" BackColor="White" ForeColor="Black">
                                 </asp:Label></td>
                <td style="width: 350px">
                    <asp:Label ID="Statelabel" runat="server" BackColor="White" 
                     Font-Size="XX-Large" ForeColor="Black" Height="50px" Width="339px"></asp:Label></td>
                <td style="width: 150px">
                    <asp:Label ID="CurrentActivityLabel" runat="server" BackColor="White" 
                     Font-Size="Large" ForeColor="Black" Height="30px" Text="Current Activity: " 
                     Width="130px"></asp:Label></td>
                <td style="width: 200px"><asp:Label ID="ActivityLabel" runat="server" BackColor="White" 
                     Font-Size="Large" ForeColor="Black" Height="30px" Text=""></asp:Label></td>
                <td style="width: 117px"><asp:Label ID="CurrentUserGroupLabel" runat="server" BackColor="White" 
                     Font-Size="Large" ForeColor="Black" Height="30px" Text="Current User: "></asp:Label></td>
                <td style="width: 149px"><asp:Label ID="UserGroupLabel" runat="server" BackColor="White" 
                     Font-Size="Large" ForeColor="Black" Height="30px" Text=""></asp:Label></td>
                <td><asp:Button ID="EndOfDay" runat="server" Text="End of Day"  causesvalidation="false"/><br /><br />
                    <asp:Button ID="RestoreButton" runat="server" visible="true" CausesValidation="False" style="height: 26px" Text="RESTORE ELF" />
                </td>
                <td rowspan="2"><asp:Image ID="ELFImage" runat="server" AlternateText="ELF" Height="80px" 
												  
                     ImageUrl="~/Images/if_elf_62126.png" Width="100px" />
								  
                 <asp:Label ID="SoftwareVersion" runat="server" Text="Software Version 6.0"></asp:Label></td>
            </tr>
           
        </table>
       
								
 <asp:HiddenField ID="LAHiddenFieldcontrol" runat="server" />
																																						 
</div>
															

      </ContentTemplate>
    </asp:UpdatePanel>
  </div>   
      <asp:Timer ID="Timer1" runat="server" Interval="7200000"></asp:Timer>

				
    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
    </Triggers>
    <ContentTemplate>
	   
    </ContentTemplate>
        
       </asp:UpdatePanel>


	
  <input id="inpHide" type="hidden" runat="server" value="9" />

        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
             <asp:PlaceHolder ID="PlaceHolder4" runat="server">
            <uc9:WriteDatauc ID="WriteDatauc1" LinacName="E2" UserReason="10"  Tabby="EndDay"  WriteName="EndDayData"   Visible="False" runat="server" />
           
            </asp:PlaceHolder>
            </ContentTemplate>
            </asp:UpdatePanel>


            
    <asp:TabContainer ID="tcl" runat="server"  activetabindex="0" 
          OnClientActiveTabChanged="loadTabPanel"  height="930px" >
         
<asp:TabPanel runat="server" HeaderText="E2 Status" ID="TabPanel0"><HeaderTemplate>
E2 Status
</HeaderTemplate>
<ContentTemplate>
<asp:UpdatePanel ID="linacstatus" runat="server" >
    <ContentTemplate>

        <asp:UpdatePanel ID="UpdatePanel0" runat="server" >
            <ContentTemplate>
            <asp:Button ID="TabButton0" runat="server"  OnClick="TabButton_Click"  style="display:none;" CausesValidation="false"/>
            <asp:Panel ID="Panel0" runat="server" >
           
                    <uc5:LinacStatusuc ID="LinacStatusuc1" LinacName="E2" runat="server" />
                    </asp:Panel></ContentTemplate></asp:UpdatePanel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:TabPanel>

<%--This is the engineering run up tab--%>
        
<asp:TabPanel ID="TabPanel1" runat="server" HeaderText="E2 Engineering Runup" DynamicContextKey='Engrunup' CssClass="ajax__tab_header"><ContentTemplate>
<asp:UpdatePanel ID="signin" runat="server"
><ContentTemplate>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" >
<ContentTemplate>
<asp:Button ID="TabButton1" runat="server"  OnClick="TabButton_Click"  style="display:none;" CausesValidation="false"/>
<asp:Panel ID="TabContent1" runat="server" Visible="False">
<uc4:AcceptLinac ID="AcceptLinac1" runat="server" LinacName= "E2"  Tabby="1" UserReason = "1" visible="false" />
    <uc1:ErunupUserControlCommon ID="ErunupUserControl1" LinacName="E2" Tabby = "1" UserReason = "1" DataName="EngData" visible="false" runat="server" />

</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
    

</asp:TabPanel>
        
<%--  This is the Pre-clinical tab  --%>    
        
        
<asp:TabPanel ID="TabPanel2" runat="server" hidden="true" Enabled="false">
        <ContentTemplate>
<asp:UpdatePanel ID="updatemod2" runat="server" >
<ContentTemplate>
<asp:UpdatePanel ID="UpdatePaneln" updatemode ="Conditional"  runat="server">
<ContentTemplate><asp:Button ID="TabButton2" runat="server"  OnClick="TabButton_Click"  style="display:none;" CausesValidation="false"/>
<asp:Panel ID="TabContenE2" runat="server" Visible="false"
><uc4:AcceptLinac ID="AcceptLinac2" LinacName="E2" UserReason="2" Tabby="2" runat="server" visible="false"/>
<uc2:Preclinusercontrol ID="Preclinusercontrol1" LinacName = "E2" DataName="PreData" runat="server"/>
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:TabPanel>

<%-- This is the Clinical Tab --%>
        
<asp:TabPanel ID="TabPanel3" runat="server" HeaderText="E2 Clinical">
        <ContentTemplate>
<asp:UpdatePanel ID="UpdatePanel3" runat="server" >
<ContentTemplate><asp:UpdatePanel ID="UpdatePanelClinical" Updatemode="Conditional" runat="server">
<ContentTemplate>
<asp:Button ID="TabButton3" runat="server" OnClick="TabButton_click" Style="Display: none" CausesValidation="false"/>
<asp:Panel ID="TabContent3" runat="server" Visible="false">
<uc4:AcceptLinac ID="AcceptLinac3" LinacName="E2" UserReason="3" Tabby="3" runat="server"  />
<uc3:ClinicalUserControl ID="ClinicalUserControl1"  LinacName="E2" DataName="ClinData" runat="server" visible="false"/>
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:TabPanel>
        
        
<asp:TabPanel ID="TabPanel4" runat="server" HeaderText="E2 Planned Maintenance" >
        <ContentTemplate>
<asp:UpdatePanel ID="UpdatePanel4" runat="server" >
<ContentTemplate>
<asp:UpdatePanel ID="UpdatePanelMaintenance" runat="server">
<ContentTemplate><asp:Button ID="TabButton4" runat="server" OnClick="TabButton_click" Style="Display: none" CausesValidation="false"/>
<asp:Panel ID="TabContent4" runat="server" Visible="false">
<uc4:AcceptLinac ID="AcceptLinac4" LinacName="E2" UserReason="4" Tabby="4" runat="server" visible="false"/>
<uc6:PlannedMaintenanceuc ID="PlannedMaintenanceuc1" linacname="E2" runat="server" Visible="false" />
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:TabPanel>
  
        
  <asp:TabPanel ID="TabPanel5" runat="server" HeaderText="E2 Repair" >
        <ContentTemplate>
<asp:UpdatePanel ID="UpdatePanel5" runat="server" >
<ContentTemplate>
<asp:UpdatePanel ID="UpdateRepair" runat="server">
<ContentTemplate>
<asp:Button ID="TabButton5" runat="server" OnClick="TabButton_click" Style="Display: none" CausesValidation="false"/>
<asp:Panel ID="TabContent5" runat="server" Visible="false">
<uc4:AcceptLinac ID="AcceptLinac5" LinacName="E2" UserReason="5" Tabby="5" runat="server" visible="false"/>
<uc7:Repairuc ID="Repairuc1" LinacName="E2" runat="server" Visible="false" />
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
<asp:Panel ID="TabContent6" runat="server" Visible="false"><uc4:AcceptLinac ID="AcceptLinac6" LinacName="E2" UserReason="6" Tabby="6" runat="server" visible="false"/>

 </asp:Panel>
 </ContentTemplate>
 </asp:UpdatePanel>
 </ContentTemplate>
 </asp:UpdatePanel>
</ContentTemplate>
</asp:TabPanel>

<asp:TabPanel ID="TabPanel7" runat="server" DynamicContextKey='Emerunup' CssClass="ajax__tab_header" hidden="true" Enabled="false">
     <ContentTemplate>
<asp:UpdatePanel ID="UpdatePanel7" runat="server">
<ContentTemplate>
<asp:UpdatePanel ID="UpdatePanelEmergency" runat="server" >
<ContentTemplate><asp:Button ID="TabButton7" runat="server"  OnClick="TabButton_Click"  style="display:none;" CausesValidation="false"/>
<asp:Panel ID="TabContent7" runat="server" Visible="False">
<uc4:AcceptLinac ID="AcceptLinac7" runat="server" LinacName= "E2"  Tabby="7" UserReason = "9" visible="false" />
 <uc1:ErunupUserControlCommon ID="ErunupUserControl2" LinacName="E2" Tabby = "7" UserReason = "9" DataName="EmeData" visible="false" runat="server" />

</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
 </asp:TabPanel>

<asp:TabPanel ID="TabPanel8" runat="server" HeaderText="E2 Development/Training" DynamicContextKey='Devel' CssClass="ajax__tab_header"><ContentTemplate>
<asp:UpdatePanel ID="UpdateDevel" runat="server">
<ContentTemplate>
<asp:Button ID="TabButton8" runat="server"  OnClick="TabButton_Click"  style="display:none;" CausesValidation="false"/>
<asp:Panel ID="TabContent8" runat="server" Visible="False">
<uc4:AcceptLinac ID="AcceptLinac8" runat="server" LinacName= "E2"  Tabby="8" UserReason = "8" visible="false" />
<uc12:Traininguc ID="Traininguc1" LinacName = "E2" Tabby="8" UserReason="8" Visible="false" runat="server" />
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</ContentTemplate>
</asp:TabPanel>      
    </asp:TabContainer>
    
</asp:Content>


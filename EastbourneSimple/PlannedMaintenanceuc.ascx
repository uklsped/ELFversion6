<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PlannedMaintenanceuc.ascx.vb" Inherits="Planned_Maintenanceuc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="LockElfuc.ascx" tagname="LockElfuc" tagprefix="uc1" %>
<%@ Register src="DefectSavePark.ascx" tagname="DefectSavePark" tagprefix="uc10" %>
<%@ Register src="controls/CommentBoxuc.ascx" tagname="CommentBoxuc" tagprefix="uc2" %>
<%@ Register Src="WriteDatauc.ascx" TagName="WriteDatauc" TagPrefix="uc3" %>
<%@ Register Src="AtlasEnergyViewuc.ascx" TagName="AtlasEnergyViewuc" TagPrefix="uc4" %>
<%@ Register Src="Modalitiesuc.ascx" TagName="Modalitiesuc" TagPrefix="uc5" %>
<%@ Register Src="ViewOpenFaults.ascx" TagName="ViewOpenFaults" TagPrefix="uc6" %>
<%@ Register Src="ViewCommentsuc.ascx" TagName="ViewCommentsuc" TagPrefix="uc7" %>
<%@ Register src="DefectSave.ascx" tagname="DefectSave" tagprefix="uc8" %>
<%@ Register src="TodayClosedFault.ascx" tagname="TodayClosedFault" tagprefix="uc9" %>


<%@ Register src="controls/MainFaultDisplayuc.ascx" tagname="MainFaultDisplayuc" tagprefix="uc11" %>


<%@ Register src="ManyFaultGriduc.ascx" tagname="ManyFaultGriduc" tagprefix="uc12" %>


<%@ Register src="controls/ReportFaultPopUpuc.ascx" tagname="ReportFaultPopUpuc" tagprefix="uc13" %>


<div class="clear" style="width:1863px"></div>
<div class="grid" >
    <div class="col100 grey" >
        <table id="Handover">
            <tr style="vertical-align:top">
                <td style="width: 300px">
                  <asp:Label ID="StateLabel" runat="server" Text="Last State:"></asp:Label><br />
                    <asp:TextBox ID="StateTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                      
                </td>
                <td style="width:145px">
                     <asp:Button ID="LogOffButton" runat="server" Text="Log Off" 
                         CausesValidation="false" Enabled="False" Height="50px" Width="183px" />
                </td>
                </tr>
            <tr><td rowspan="4" style="width:300px">
                <asp:RadioButtonList ID="RadioButtonList1" CausesValidation="false" runat="server" AutoPostBack="True"></asp:RadioButtonList>
                </td></tr>
           <%-- <tr>
                <td style="width:145px">
                     <asp:Button ID="LogOffButton" runat="server" Text="Log Off" 
                         CausesValidation="false" Enabled="False" Height="50px" Width="183px" />
                </td>
            </tr>--%>
            <tr>
                <td style="width: 145px">
            <asp:Button ID="LockElf" runat="server" Text="Lock Elf/Switch User" causesvalidation="false"/>
                                       
                </td>
            </tr>
            <tr>
                <td style="width: 145px">
                    <asp:Button ID="PhysicsQA" runat="server" Text="View Physics Energies/Imaging" 
                        CausesValidation="false" Width="200px" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="ViewAtlasButton" runat="server" Text="View Atlas Energies" CausesValidation="false" />
                </td>
            </tr>
            <tr>
                <td colspan="3" style="height:92px"><fieldset>
                    <legend style="font-family: Arial, Helvetica, sans-serif; font-weight: bold">
                        Maintenance Comments
                   </legend>
                    <%-- <br />--%>
                  <uc2:CommentBoxuc ID="CommentBox" runat="server" />
                   </fieldset></td>
                
            </tr>
            <tr>
              <%-- <td colspan="2"> <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                    <asp:Label ID="Label1" runat="server" Text="Major Faults Cleared Today:"></asp:Label>
                    <br /><br />
                   <asp:PlaceHolder ID="PlaceHolder5" runat="server"></asp:PlaceHolder>
                    </ContentTemplate>
                </asp:UpdatePanel></td>--%>
                
            </tr>
        </table>
    </div>
    <div class="col200 blue" ><asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="ReportFaultButton" runat="server" Text="Report Fault" CausesValidation="false"/>
                            <uc13:ReportFaultPopUpuc ID="ReportFaultPopUpuc1" LinacName="" Visible="false" ParentControl="4" runat ="server" />
                            <%--<asp:PlaceHolder ID="PlaceHolderReportFault" runat="server"></asp:PlaceHolder>--%>
                        <asp:PlaceHolder ID="PlaceHolderDefectSave" runat="server"></asp:PlaceHolder>
                        </ContentTemplate>
                   </asp:UpdatePanel></div>
    <div class="col300 green" ><asp:UpdatePanel ID="UpdatePanel2" runat="server" Visible="true" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:PlaceHolder ID="PlaceHolderFaults" runat="server"></asp:PlaceHolder>&nbsp;
        </ContentTemplate>
    </asp:UpdatePanel></div>
    
    
    
    
    
    
    
</div>
<%--<div>
    <asp:Panel ID="Panel100" runat="server" BackColor="#99CCFF" BorderColor="#0033CC"
        BorderStyle="Solid">--%>

        <uc3:WriteDatauc ID="WriteDatauc1" LinacName="" UserReason="4" Tabby="4" Visible="false" runat="server" />
        <uc1:LockElfuc ID="LockElfuc1" LinacName="" UserReason="4" Tabby="4"  visible="false" runat="server" />
<%--         </asp:Panel>
    </div>--%>
       <%-- <asp:Table ID="Table1" runat="server" CellSpacing="20" GridLines="Both" Width="1875px">
            <asp:TableRow ID="r1" runat="server">
                <asp:TableCell ID="c1" runat="server" Width="300px" HorizontalAlign="Left">

                   
                    <br />
                    
                

                    
                </asp:TableCell>
                <asp:TableCell ID="c2" runat="server" Width="50px">

                   
                </asp:TableCell>
                <asp:TableCell ID = "c4" runat="server" HorizontalAlign="Left" Width="500px">

                   
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Top">

               
               </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Table2" runat="server" CellSpacing="20" GridLines="Both" Width="1507px">
            <asp:TableRow ID="t2r1" runat="server">
            <asp:TableCell ID="t2c0" runat="server">

            <asp:Button ID="LockElf" runat="server" Text="Lock Elf/Switch User" causesvalidation="false"/>
                

            </asp:TableCell>
                <asp:TableCell ID="t2c1" runat="server">

                    <asp:Button ID="Faultpanelbutton" runat="server" Text="View Open Faults" CausesValidation="false" Visible="false" />
                

                </asp:TableCell>
                <asp:TableCell ID="t2c2" runat="server">

                    <asp:Button ID="ViewAtlasButton" runat="server" Text="View Atlas Energies" CausesValidation="false" />
                

                </asp:TableCell>
                <asp:TableCell ID="t2c3" runat="server">

                    <asp:Button ID="PhysicsQA" runat="server" Text="View Physics Energies/Imaging" CausesValidation="false" />
                

                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
   
</div>--%>
<div id="left">
   
    <asp:UpdatePanel ID="UpdatePanelAtlas" runat="server" Visible="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanelQA" runat="server" Visible="false">
        <ContentTemplate>
            <asp:PlaceHolder ID="PlaceHolder3" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div id="right">
</div>

<uc7:ViewCommentsuc ID="ViewCommentsuc1" LinacName="" CommentSort="pm" runat="server" />
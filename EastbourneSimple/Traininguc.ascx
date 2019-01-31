﻿<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Traininguc.ascx.vb"
    Inherits="Traininguc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="DefectSavePark.ascx" tagname="DefectSavePark" tagprefix="uc1" %>
<%@ Register Src="WriteDatauc.ascx" TagName="WriteDatauc" TagPrefix="uc3" %>
<%@ Register Src="AtlasEnergyViewuc.ascx" TagName="AtlasEnergyViewuc" TagPrefix="uc4" %>
<%@ Register Src="Modalitiesuc.ascx" TagName="Modalitiesuc" TagPrefix="uc5" %>
<%@ Register Src="ViewOpenFaults.ascx" TagName="ViewOpenFaults" TagPrefix="uc6" %>
<%@ Register Src="ViewCommentsuc.ascx" TagName="ViewCommentsuc" TagPrefix="uc7" %>
<%@ Register src="AcceptLinac.ascx" tagname="AcceptLinac" tagprefix="uc8" %>
<%@ Register src="DefectSave.ascx" tagname="DefectSave" tagprefix="uc9" %>
<%@ Register src="TodayClosedFault.ascx" tagname="TodayClosedFault" tagprefix="uc10" %>
<%@ Register src="controls/CommentBoxuc.ascx" tagname="CommentBoxuc" tagprefix="uc2" %>
<%--<link href="App_Themes/Blue/Elf.css" rel="stylesheet" type="text/css" />--%>

<div>
    <asp:Panel ID="Panel100" runat="server" BackColor="#99CCFF" BorderColor="#0033CC"
        BorderStyle="Solid">
        
        <uc3:WriteDatauc ID="WriteDatauc1" LinacName="" UserReason="8" Tabby="8" Visible="false"
            runat="server" />
        
        <asp:Table ID="Table1" runat="server" CellSpacing="20" GridLines="Both" Width="1875px">
            <asp:TableRow ID="r1" runat="server">
                <asp:TableCell ID="c1" runat="server" Width="300px" HorizontalAlign="Left">
                    <asp:Label ID="StateLabel" runat="server" Text="Last State:"></asp:Label>
                    <asp:TextBox ID="StateTextBox" runat="server"></asp:TextBox>
                    <asp:RadioButtonList ID="RadioButtonList1" CausesValidation="false" runat="server"
                        AutoPostBack="True">
                    </asp:RadioButtonList>
                    <br />
                     <legend align="left" style="font-family: Arial, Helvetica, sans-serif; font-weight: bold">
                       Training Comments</legend>
                    <uc2:CommentBoxuc ID="CommentBox" runat="server" />
                </asp:TableCell>
                <asp:TableCell ID="c2" runat="server" Width="50px">
                    <asp:Button ID="LogOffButton" runat="server" Text="Log Off" CausesValidation="false"
                        Enabled="False" Height="150px" />
                </asp:TableCell>
                
                <asp:TableCell ID = "c4" runat="server" HorizontalAlign="Left" Width="500px">
                       <asp:UpdatePanel ID="UpdatePanel3" runat="server">
<ContentTemplate>
<asp:PlaceHolder ID="PlaceHolder4" runat="server">
</asp:PlaceHolder>
</ContentTemplate>

</asp:UpdatePanel>
                       </asp:TableCell>
                        <asp:TableCell VerticalAlign="Top">
<asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
               <ContentTemplate>
                   <asp:Label ID="Label1" runat="server" Text="Major Faults Cleared Today:"></asp:Label>
                   <br></br>
               <asp:PlaceHolder ID="PlaceHolder5" runat="server">
                </asp:PlaceHolder>
                </ContentTemplate></asp:UpdatePanel>
               </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        
        <asp:Table ID="Table2" runat="server" CellSpacing="20" GridLines="Both" Width="1038px">
            <asp:TableRow ID="t2r1" runat="server">
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
        
    </asp:Panel>
</div>
<div id="left">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" Visible="true" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
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
<asp:UpdatePanel ID="confirmrunup" runat="server">
    <ContentTemplate>
        <asp:UpdatePanel ID="ru" runat="server" Visible="false">
            <ContentTemplate>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>







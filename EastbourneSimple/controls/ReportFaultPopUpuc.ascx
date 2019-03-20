<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ReportFaultPopUpuc.ascx.vb" Inherits="controls_ReportFaultPopUpuc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register src="../DefectSave.ascx" tagname="DefectSave" tagprefix="uc1" %>

<link href="App_Themes/Blue/Elf.css" rel="stylesheet" type="text/css" />

<asp:Label ID="Label1" runat="server" style="display:none" causesvalidation="false" Visible="true"></asp:Label>
<asp:HiddenField ID="ParentTabComment" runat="server" />
<asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="Panel1" runat="server" Height="600px" Width="970px" cssclass="modalPopup" style="display:none" >
  <div >
      <%--<asp:PlaceHolder ID="PlaceHolderDefectSave" runat="server"></asp:PlaceHolder>--%>
      <uc1:DefectSave ID="DefectSave1" runat="server" />
  </div>
  <asp:Label ID="LoginErrorDetails" runat="server" Font-Italic="True" ForeColor="Red"></asp:Label>
            
</asp:Panel>

</ContentTemplate>
</asp:UpdatePanel>


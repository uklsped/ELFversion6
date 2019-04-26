<%@ Page Title="" Language="VB" MasterPageFile="~/Elf.master" AutoEventWireup="false" CodeFile="testpopup.aspx.vb" Inherits="testpopup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div> 
        <asp:UpdatePanel ID="udpOutterUpdatePanel" runat="server"> 
             <ContentTemplate> 

                 <asp:Button ID="ButtonShow" runat="server" Text="Show Popup" OnClick="ButtonShow_Click" />                 


                <input id="dummy" type="button" style="display: none" runat="server" /> 
                <asp:ModalPopupExtender runat="server" 
                        ID="mpeThePopup" 
                        TargetControlID="dummy" 
                        PopupControlID="pnlModalPopUpPanel" 
                        BackgroundCssClass="modalBackground"                        
                        DropShadow="true"/>
                 <asp:Panel ID="pnlModalPopUpPanel" runat="server" CssClass="modalPopup"> 
                    <asp:UpdatePanel ID="udpInnerUpdatePanel" runat="Server" UpdateMode="Conditional"> 
                        <ContentTemplate> 
                            Message
                            <asp:Button ID="ButtonClose" runat="server" Text="Close Popup" OnClick="ButtonClose_Click" />                                                                  
                        </ContentTemplate>       
                    </asp:UpdatePanel> 
                 </asp:Panel> 

            </ContentTemplate> 
        </asp:UpdatePanel> 
    </div> 

</asp:Content>


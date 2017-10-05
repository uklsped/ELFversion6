<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Elf.master" CodeFile="AcknowledgeEnergies.aspx.vb" Inherits="AcknowledgeEnergies" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="EnergyDisplayuc.ascx" tagname="EnergyDisplayuc" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    <div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    
        <uc1:EnergyDisplayuc ID="EnergyDisplayuc1" LinacName="LA3" runat="server" />
    
    </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    
<div>
                    <table>
                        
 
                        
                       
                        <tr>
                            <td class="style1">
                                <asp:Button ID="AcceptOK" runat="server" Text="Acknowledge Energies" causesvalidation="false"/>
                                 
                                <asp:Button ID="btnchkcancel" runat="server" causesvalidation="false" 
                                    Text="Cancel" />
                            </td>
                        </tr>
                    </table>
                </div>
    
</asp:Content>

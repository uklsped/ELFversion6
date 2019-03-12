<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MainFaultDisplayuc.ascx.vb" Inherits="controls_MainFaultDisplayuc" %>


<%@ Register src="../ManyFaultGriduc.ascx" tagname="ManyFaultGriduc" tagprefix="uc1" %>


<%@ Register src="../ViewOpenFaults.ascx" tagname="ViewOpenFaults" tagprefix="uc2" %>


<%@ Register src="../TodayClosedFault.ascx" tagname="TodayClosedFault" tagprefix="uc3" %>


<div>
  <table style="width=100%">
    <tr>
        <td>
            <asp:PlaceHolder ID="PlaceHolderFaults" runat="server"></asp:PlaceHolder>
        </td>
    </tr>
    <tr>
        <td>

            <asp:PlaceHolder ID="PlaceHolderTodaysclosedfaults" runat="server"></asp:PlaceHolder>    
        </td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:PlaceHolder ID="PlaceHolderViewOpenFaults"  runat="server"></asp:PlaceHolder>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
  </table>  
</div>








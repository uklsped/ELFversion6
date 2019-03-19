<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MainFaultDisplayuc.ascx.vb" Inherits="controls_MainFaultDisplayuc" %>


<%@ Register src="../ManyFaultGriduc.ascx" tagname="ManyFaultGriduc" tagprefix="uc1" %>


<%@ Register src="../ViewOpenFaults.ascx" tagname="ViewOpenFaults" tagprefix="uc2" %>


<%@ Register src="../TodayClosedFault.ascx" tagname="TodayClosedFault" tagprefix="uc3" %>


<%@ Register src="NewFaultPopUpuc.ascx" tagname="NewFaultPopUpuc" tagprefix="uc4" %>


<div style="height:800px">
    

        <div style="height:250px;">
            <asp:PlaceHolder ID="PlaceHolderFaults" runat="server"></asp:PlaceHolder>

      </div>

    <div style="height:150px; ">
            <asp:PlaceHolder ID="PlaceHolderTodaysclosedfaults" runat="server"></asp:PlaceHolder>    

        </div>
<div style="height:400px; ">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:PlaceHolder ID="PlaceHolderViewOpenFaults"  runat="server"></asp:PlaceHolder><br />

                </ContentTemplate>
            </asp:UpdatePanel>
    
    </div>
</div>








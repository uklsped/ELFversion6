<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MainFaultDisplayuc.ascx.vb" Inherits="controls_MainFaultDisplayuc" %>

<%@ Register src="../ManyFaultGriduc.ascx" tagname="ManyFaultGriduc" tagprefix="uc1" %>

<%@ Register src="ViewOpenFaultsuc.ascx" tagname="ViewOpenFaultsuc" tagprefix="uc2" %>

<%@ Register src="../TodayClosedFault.ascx" tagname="TodayClosedFault" tagprefix="uc3" %>

<%@ Register src="NewFaultPopUpuc.ascx" tagname="NewFaultPopUpuc" tagprefix="uc4" %>

 <%--<div id="container">
   
    <div class="main">
        <section class="third"><asp:PlaceHolder ID="PlaceHolderFaults" runat="server"></asp:PlaceHolder></section>
        <section class="third"><asp:PlaceHolder ID="PlaceHolderTodaysclosedfaults" runat="server"></asp:PlaceHolder></section>
        <section class="third"><asp:PlaceHolder ID="PlaceHolderViewOpenFaults"  runat="server"></asp:PlaceHolder></section>
    </div>
</div>--%>

<div id="container">

<div id="top_div"><asp:PlaceHolder ID="PlaceHolderFaults" runat="server"></asp:PlaceHolder></div>
<div id="middle_div"><asp:PlaceHolder ID="PlaceHolderTodaysclosedfaults" runat="server"></asp:PlaceHolder></div>
<div id="bottom_div"><asp:PlaceHolder ID="PlaceHolderViewOpenFaults"  runat="server"></asp:PlaceHolder></div>

</div>








<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ClinicalUserControl.ascx.vb" Inherits="ClinicalUserControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="WriteDatauc.ascx" TagName="WriteDatauc" TagPrefix="uc2" %>

<%@ Register Src="AcceptLinac.ascx" TagName="AcceptLinac" TagPrefix="uc3" %>

<%@ Register Src="ViewCommentsuc.ascx" TagName="ViewCommentsuc" TagPrefix="uc4" %>

<%@ Register Src="DefectSave.ascx" TagName="DefectSave" TagPrefix="uc5" %>

<%@ Register Src="TodayClosedFault.ascx" TagName="TodayClosedFault" TagPrefix="uc6" %>


<%@ Register Src="DefectSavePark.ascx" TagName="DefectSavePark" TagPrefix="uc7" %>


<%@ Register Src="controls/CommentBoxuc.ascx" TagName="CommentBoxuc" TagPrefix="uc8" %>


<%@ Register src="controls/ReportFaultPopUpuc.ascx" tagname="ReportFaultPopUpuc" tagprefix="uc9" %>


<%@ Register src="controls/MainFaultDisplayuc.ascx" tagname="MainFaultDisplayuc" tagprefix="uc10" %>


<%@ Register src="controls/ReportAFaultuc.ascx" tagname="ReportAFaultuc" tagprefix="uc11" %>


<%@ Register src="controls/ModalityDisplayuc.ascx" tagname="ModalityDisplayuc" tagprefix="uc12" %>


<link href="App_Themes/Blue/Elf.css" rel="stylesheet" type="text/css" />
<%@ Register Src="ViewOpenFaults.ascx" TagName="ViewOpenFaults" TagPrefix="uc1" %>

<div class="grid">
    <div class="col100 grey">

        <table id="HandoverTable">
             <tr style="vertical-align:top">
            <td colspan="3">
                <asp:Panel ID="ModalityDisplayPanel" runat="server" Visible="false">
                       <asp:PlaceHolder ID="ModalityPlaceholder" runat="server">
                       </asp:PlaceHolder>
                   </asp:Panel>
            </td>
                </tr>
            <tr style="vertical-align: top">
                <td colspan="2">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="Tstart" runat="server"
                                    Text="Start Treatment" CausesValidation="false"
                                    BackColor="#FFCC00"
                                    Height="150px" Width="120px" />
                            </td>
                            <td>
                                <asp:Button ID="LogOffButton" runat="server" Text="Log Off" Height="150px"
                                    CausesValidation="false" />
                            </td>

                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <uc11:ReportAFaultuc ID="ReportAFaultuc1" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 92px">
                    <table>
                        <tr>
                            <td>
                                <asp:Literal ID="Literal1" runat="server" Text="Runup Comments"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="Panel1" Enabled="false" runat="server">
                                    <uc8:CommentBoxuc ID="RunUpCommentBox" TextHeight="60" runat="server" NoWrite="true" />
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>Clinical Comment
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanelcomments" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <uc8:CommentBoxuc ID="CommentBox" TextHeight="60" runat="server" />
                                                        <br />
                                                        <asp:Button ID="SaveText" runat="server" Text="Save" CausesValidation="False" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="SaveText" EventName="click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>

                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="background-color: Green; height: 30px; width: 355px; margin: 0; padding: 0">
                                                    <table cellspacing="0" cellpadding="0" rules="all" border="1" id="Table2"
                                                        style="font-family: Arial; font-size: 10pt; width: 355px; color: white; border-collapse: collapse;
                                                        height: 100%;">
                                                        <tr>
                                                            <td style="width: 60px; text-align: center">Time</td>
                                                            <td style="width: 240px; text-align: center">Clinical Comment</td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>

                                                        <div style="height: 175px; width: 355px; overflow: auto;">
                                                            <asp:GridView ID="GridViewComments" runat="server" AutoGenerateColumns="False" ShowHeader="false"
                                                                DataKeyNames="Clincomment" BackColor="White"
                                                                BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4"
                                                                GridLines="Horizontal">
                                                                <RowStyle BackColor="White" ForeColor="#333333" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="DateTime" ItemStyle-Width="60px"
                                                                        SortExpression="Time" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top"
                                                                        HtmlEncode="False" HtmlEncodeFormatString="True" />
                                                                    <asp:BoundField DataField="Clincomment" ItemStyle-Width="350px"
                                                                        SortExpression="Clinical" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top"
                                                                        HtmlEncode="False" HtmlEncodeFormatString="True" />

                                                                </Columns>
                                                                <FooterStyle BackColor="White" ForeColor="#333333" />
                                                                <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                                                                <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                                                <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                                                            </asp:GridView>
                                                        </div>

                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>

</div>
<%-- <div class="col200 blue" ><asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="ReportFaultButton" runat="server" Text="Report Fault" CausesValidation="false"/>
                            <asp:PlaceHolder ID="ReportFaultPopupPlaceHolder" runat="server"></asp:PlaceHolder>
                        </ContentTemplate>
                   </asp:UpdatePanel></div>--%>
    <div class="col300 green" ><asp:UpdatePanel ID="UpdatePanel5" runat="server" Visible="true" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:PlaceHolder ID="PlaceHolderFaults" runat="server"></asp:PlaceHolder>&nbsp;
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
 
    
</div>

<div>
    <asp:Panel ID="Panel100" runat="server" BackColor="#99CCFF" BorderColor="#0033CC"
        BorderStyle="Solid">


      <%--  <div>
            <asp:Label ID="StateLabel" runat="server" BackColor="#0066FF" Font-Bold="True" 
                ForeColor="Yellow"></asp:Label>
        </div>--%>
        <%--<div id="header_wrapper">
            <div id="temp_header">
                <asp:Table ID="Table4" runat="server">
                    <asp:TableRow ID="TableRow1" runat="server">
                        <asp:TableCell ID="TableCell1" runat="server">
                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False"
                                DataKeyNames="HandoverId"
                                BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px"
                                CellPadding="4" EditRowStyle-BorderStyle="NotSet" Width="906px">
                                <RowStyle BackColor="White" ForeColor="#330099" />
                                <Columns>
                                    <asp:TemplateField HeaderText="6 MV">
                                        <ItemTemplate>
                                            <img src="<%#FormatImage(Eval("MV6"))%>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="6 MV FFF">
                                        <ItemTemplate>
                                            <img src=" <%#FormatImage(Eval("MV6FFF")) %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="10 MV">
                                        <ItemTemplate>
                                            <img src="<%#FormatImage(Eval("MV10"))%>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="10 MV FFF">
                                        <ItemTemplate>
                                            <img src="<%#FormatImage(Eval("MV10FFF"))%>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="4 MeV">
                                        <ItemTemplate>
                                            <img src="<%#FormatImage(Eval("MeV4"))%>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="6 MeV">
                                        <ItemTemplate>
                                            <img src="<%#FormatImage(Eval("MeV6"))%>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="8 MeV">
                                        <ItemTemplate>
                                            <img src="<%#FormatImage(Eval("MeV8"))%>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="10 MeV">
                                        <ItemTemplate>
                                            <img src="<%#FormatImage(Eval("MeV10"))%>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="12 MeV">
                                        <ItemTemplate>
                                            <img src="<%#FormatImage(Eval("MeV12"))%>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="15 MeV">
                                        <ItemTemplate>
                                            <img src="<%#FormatImage(Eval("MeV15"))%>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="18 MeV">
                                        <ItemTemplate>
                                            <img src="<%#FormatImage(Eval("MeV18"))%>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="20 MeV">
                                        <ItemTemplate>
                                            <img src="<%#FormatImage(Eval("MeV20"))%>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="iView">
                                        <ItemTemplate>
                                            <img src="<%#FormatImage(Eval("iView"))%>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="XVI">
                                        <ItemTemplate>
                                            <img src="<%#FormatImage(Eval("XVI"))%>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                <HeaderStyle BackColor="#330000" Font-Bold="True" ForeColor="#FFFFCC" />
                            </asp:GridView>
                        </asp:TableCell>
                        <asp:TableCell runat="server">
                            <asp:PlaceHolder ID="PlaceHolder6" runat="server"></asp:PlaceHolder>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>

        </div>--%>

<%--        <asp:Table ID="Table1" runat="server" CellSpacing="20" GridLines="Both"
            Width="1875px" Height="509px">
            <asp:TableRow ID="r1" runat="server" Width="1100px" HorizontalAlign="Left" BorderColor="White">
                <asp:TableCell ID="c1" runat="server" Width="130px" BorderStyle="Solid" HorizontalAlign="Left">
                    <asp:Table runat="server">
                        <asp:TableRow runat="server">

                            <asp:TableCell>

                               <asp:Button ID="Tstart" runat="server"
                                    Text="Start Treatment" CausesValidation="false"
                                    BackColor="#FFCC00"
                                    Height="150px" Width="120px" />
                                <br />
                                <br />
                                <br />


                            </asp:TableCell>


                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell>
                               <%-- <asp:Button ID="LogOffButton" runat="server" Text="Log Off Linac"  CausesValidation="false" />



                            </asp:TableCell>

                        </asp:TableRow>
                        <asp:TableRow ID="TableRow2" runat="server">
                            <asp:TableCell>        
         
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>

                <asp:TableCell ID="Clinicalcomments" runat="server" HorizontalAlign="left" VerticalAlign="Top" Width="250px">
                    <table>
                        <asp:Panel ID="PanelPreclincomments" runat="server">
                        <tr>
                            
                            <td>
                                
                                <div style="background-color: Green; height: 30px; width: 355px; margin: 0; padding: 0" >
                                    <table cellspacing="0" cellpadding="0" rules="all" border="1" id="Table5" style="font-family: Arial; font-size: 10pt; width: 355px; color: white; border-collapse: collapse; height: 100%;">
                                        <tr>
                                            <td style="width: 60px; text-align: center">Time</td>
                                            <td style="width: 240px; text-align: center">Pre-Clinical Comment</td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="height:135px; width: 355px; overflow: auto;">
                                    <asp:GridView ID="GridViewPre" runat="server" AutoGenerateColumns="False" ShowHeader="false"
                                        DataKeyNames="Ccomment" BackColor="White"
                                        BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4"
                                        GridLines="Horizontal">
                                        <RowStyle BackColor="White" ForeColor="#333333" />
                                        <Columns>
                                            <asp:BoundField DataField="DateTime"  ItemStyle-Width="60px"
                                                SortExpression="Time" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top" HtmlEncode="False" HtmlEncodeFormatString="True" />
                                            <asp:BoundField DataField="Ccomment"  ItemStyle-Width="350px"
                                                SortExpression="PreClinical" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top" />

                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#333333" />
                                        <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                                    </asp:GridView>
                                </div>
                            </td>
                           
                        </tr>
                        </asp:Panel>
                        <tr>
                            <td>
                                <div style="background-color: Green; height: 30px; width: 355px; margin: 0; padding: 0">
                                    <table cellspacing="0" cellpadding="0" rules="all" border="1" id="Table6"
                                        style="font-family: Arial; font-size: 10pt; width: 355px; color: white; border-collapse: collapse; height: 100%;">
                                        <tr>
                                            <td style="width: 60px; text-align: center">Time</td>
                                            <td style="width: 240px; text-align: center">Engineering Comment</td>
                                        </tr>
                                    </table>
                                </div>

                                <div style="height:135px; width: 355px; overflow: auto;">
                                    <asp:GridView ID="GridViewEng" runat="server" AutoGenerateColumns="False" ShowHeader="false"
                                        DataKeyNames="Comment" BackColor="White"
                                        BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4"
                                        GridLines="Horizontal">
                                        <RowStyle BackColor="White" ForeColor="#333333" />
                                        <Columns>
                                            <asp:BoundField DataField="DateTime"  ItemStyle-Width="60px"
                                                SortExpression="Time" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top" HtmlEncode="False" HtmlEncodeFormatString="True" />
                                            <asp:BoundField DataField="comment"  ItemStyle-Width="350px"
                                                SortExpression="Engineering" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <FooterStyle BackColor="White" ForeColor="#333333" />
                                        <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                                    </asp:GridView>
                                </div>
                                
                            </td>
                        </tr>
                        
                    </table>
                    <%--<div>
                        <table style="width: 100%;">
                            <tr>
                                <td>Clinical Comment
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanelcomments" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc8:CommentBoxuc ID="CommentBox" runat="server" />
                                            <br />
                                            <asp:Button ID="SaveText" runat="server" Text="Save" CausesValidation="False" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="SaveText" EventName="click" />
                                        </Triggers>
                                    </asp:UpdatePanel>

                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="background-color: Green; height: 30px; width: 355px; margin: 0; padding: 0">
                                        <table cellspacing="0" cellpadding="0" rules="all" border="1" id="Table2"
                                            style="font-family: Arial; font-size: 10pt; width: 355px; color: white; border-collapse: collapse; height: 100%;">
                                            <tr>
                                                <td style="width: 60px; text-align: center">Time</td>
                                                <td style="width: 240px; text-align: center">Clinical Comment</td>
                                            </tr>
                                        </table>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>

                                            <div style="height: 175px; width: 355px; overflow: auto;">
                                                <asp:GridView ID="GridViewComments" runat="server" AutoGenerateColumns="False" ShowHeader="false"
                                                    DataKeyNames="Clincomment" BackColor="White"
                                                    BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4"
                                                    GridLines="Horizontal">
                                                    <RowStyle BackColor="White" ForeColor="#333333" />
                                                    <Columns>
                                                        <asp:BoundField DataField="DateTime"  ItemStyle-Width="60px"
                                                            SortExpression="Time" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top" HtmlEncode="False" HtmlEncodeFormatString="True" />
                                                        <asp:BoundField DataField="Clincomment"  ItemStyle-Width="350px"
                                                            SortExpression="Clinical" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top" HtmlEncode="False" HtmlEncodeFormatString="True" />

                                                    </Columns>
                                                    <FooterStyle BackColor="White" ForeColor="#333333" />
                                                    <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                                                </asp:GridView>
                                            </div>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:TableCell>

                <asp:TableCell HorizontalAlign="Left" Width="500px">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:PlaceHolder ID="PlaceHolder3" runat="server"></asp:PlaceHolder>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </asp:TableCell>

               


            </asp:TableRow>
        </asp:Table>--%>


        
       <%-- <asp:PlaceHolder ID="PlaceHolder4" runat="server">--%>
            <uc2:WriteDatauc ID="WriteDatauc2" LinacName="" UserReason="3" Tabby="3"  Visible="false" runat="server" />
        <%--</asp:PlaceHolder>--%>

    </asp:Panel>
</div>

<div>
    <br />
    <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible="true">
        <%--<uc2:WriteDatauc ID="WriteDatauc1" LinacName="" UserReason="3"  Tabby="3"  WriteName="ClinData" Visible="false" runat="server" />--%>
    </asp:PlaceHolder>
</div>
<asp:HiddenField ID="HiddenFieldLinacState" Visible="true" runat="server" />
<div style="background-color: #FFFF66; background-repeat: no-repeat; border-style: solid; border-width: thin">

    <uc4:ViewCommentsuc ID="ViewCommentsuc1" LinacName="" CommentSort="pcr" runat="server" />
</div>







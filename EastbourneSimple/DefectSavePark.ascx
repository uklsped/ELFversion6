﻿<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DefectSavePark.ascx.vb" Inherits="DefectSavePark" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="controls/CommentBoxuc.ascx" tagname="CommentBoxuc" tagprefix="uc2" %>
<%--No need now for WriteDatauc Analysis 23/11/16 --%><%--Added back in 26/03/18 --%>
<%@ Register Src="WriteDatauc.ascx" TagName="WriteDatauc" TagPrefix="uc1" %>

<style type="text/css">
    .style1 {
        width: 271px;
    }

    .style2 {
        width: 124px;
        vertical-align: top;
    }

    .style3 {
        width: 124px;
        height: 26px;
    }

    .style4 {
        height: 26px;
    }

    .redcolour {
        color: red;
        font-weight: bold;
        font-size: medium;
    }
</style>
<asp:HiddenField ID="SelectedIncidentID" Value="" runat="server" />
<asp:HiddenField ID="TimeFaultSelected" Value="" runat="server" />
<asp:HiddenField ID="AreaOrAccuray" Value="" runat="server" />
<%-- NO requirement 23/11/16 --%><%-- Added back in 26/03/18 --%>
<uc1:WriteDatauc ID="WriteDatauc1" LinacName="" UserReason="12" Tabby="Defect" WriteName="Defect" Visible="false" runat="server" />

<div style="width: 401px">
    <asp:UpdatePanel ID="UpdatePanelDefectlist" runat="server" UpdateMode="Conditional">
        <ContentTemplate>Record Fault<br />
            <span class="redcolor">* Mandatory Field </span>
            <br />
            <table style="width: 401px;">
                <tr>
                    <td class="style1">
                        <asp:DropDownList ID="Defect" runat="server" AutoPostBack="true"
                            AppendDataBoundItems="True" DataValueField="IncidentID" DateTextField="Fault">
                            <asp:ListItem>Select</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
            </table>
            <table style="width: 401px;">
                <tr>
                    <td class="style3">
                    Error Code:
              <td class="style4">
                  <asp:TextBox ID="ErrorCode" runat="server" Text="" ReadOnly="false" EnableViewState="False" Visible="true"></asp:TextBox>
              </td>
                </tr>
                <tr>
                    <td class="style2">Physicist/Accuray job number:</td>
                    <td>
                        <asp:TextBox ID="Accuray" runat="server" EnableViewState="false"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="AccurayValidation" runat="server" ControlToValidate="Accuray" ErrorMessage="Please enter a physicist name or Accuray job number" Display="Dynamic" validationgroup="Tomodefect" Enabled="false"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style2">Fault Description:</td>
                    <td>
                        <asp:Panel ID="FaultPanel" runat="server" Enabled ="false">
                            <uc2:CommentBoxuc ID="FaultDescription" runat="server" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="style2">Corrective Action:
                    </td>
                    <td>
                        <asp:Panel ID="ActPanel" Enabled="false" runat="server">
                            <uc2:CommentBoxuc ID="RadActC" runat="server" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="style2">Patient ID:</td>
                    <td>
                        <asp:TextBox ID="PatientIDBox" Text="" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionPatient" runat="server" ControlToValidate="PatientIDBox" ValidationExpression="^\d{7}$" Display="Dynamic"  ErrorMessage="Please enter a BSUH ID"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                 <tr>
                   <td class="style2">
                  <asp:Label ID="Label3" runat="server" Text="Radiation Incident?"></asp:Label>
                       </td>
                          <td>
                                     <asp:RadioButtonList ID="RadioIncident" runat="server" AutoPostBack="false" enabled="true">
                                        <asp:ListItem Text="No" Value="False"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                    </asp:RadioButtonList>
                              </td>
                          <td>
                        <asp:RequiredFieldValidator ID="RadioIncidentValidation" runat="server" ControlToValidate="RadioIncident" ErrorMessage="Please complete Radiation Incident Selection"
                            Display="Dynamic" validationgroup="Tomodefect" >
        </asp:RequiredFieldValidator>
                     </td>
              </tr>
            </table>
            <table>
                <tr>
                    <td class="style2">
                        <asp:Panel ID="Page1ViewPanel"
                            Height="85px"
                            HorizontalAlign="Left"
                            runat="Server">
                            <asp:MultiView ID="FaultTypeSave" ActiveViewIndex="-1" runat="server">
                                <asp:View ID="RecoverableView" runat="server">
                                   <asp:Button ID="SaveDefectButton" runat="server" Text="Save" Enabled="false" CausesValidation="false" />
                                </asp:View>
                                <asp:View ID="UnRecoverableView" runat="server">
                                    <asp:Label ID="Label1" runat="server" Text="Fault Closed?"></asp:Label>
                                    <asp:RadioButtonList ID="FaultOpenClosed" runat="server" AutoPostBack="True">
                                        <asp:ListItem Text="No" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <%--<asp:Label ID="Label2" runat="server" Text="Radiation Incident?"></asp:Label>
                                     <asp:RadioButtonList ID="RadioIncident" runat="server" AutoPostBack="false">
                                        <asp:ListItem Text="No" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>--%>
                                    <asp:Button ID="UnRecoverableSave" runat="server" CausesValidation="False" Enabled="False" Text="Save" />
                                </asp:View>
                            </asp:MultiView>
                        </asp:Panel>
                    </td>
                    <td class="style2">
                        <asp:Button ID="ClearButton" runat="server" Text="Clear" CausesValidation="False" CssClass="buttonmargin" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    Today's Repeat Faults
                                   <div style =" background-color:Green; 
        height:30px;width:400px; margin:0;padding:0">
        <table cellspacing="0" cellpadding = "0" rules="all" border="1" id="Table3" 
         style="font-family:Arial;font-size:10pt;width:400px;color:white;
         border-collapse:collapse;height:100%;">
            <tr>
               <td style ="width:150px;text-align:center">Repeat Fault</td>
               <td style ="width:80px;text-align:center">Time</td>
               <td style ="width:180px;text-align:center">Description</td>
               </tr></table></div>
    <div style="width: 400px">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
          
                <div style="height: 150px; width: 400px; overflow: auto;">

                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowHeader="false"
                        DataKeyNames="ConcessionNumber" BackColor="White" Width="400px"
                        BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4"
                        GridLines="Horizontal" Font-Size="smaller">
                        <RowStyle BackColor="White" ForeColor="#333333" />
                        <Columns>
                            <asp:BoundField DataField="ConcessionNumber" HeaderText="Fault" ItemStyle-Width="150px"
                                SortExpression="ConcessionNumber" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DefectTime" HeaderText="Time" ItemStyle-Width="80px"
                                SortExpression="DefectTime" >
                            <ItemStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="160px"
                                SortExpression="Description" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Width="160px" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="White" ForeColor="#333333" />
                        <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                    </asp:GridView>
                </div>
               <%-- </div>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>








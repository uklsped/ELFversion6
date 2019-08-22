﻿<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewOpenFaultsuc.ascx.vb" Inherits="controls_ViewOpenFaultsuc" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register src="ConcessionPopUpuc.ascx" tagname="ConcessionPopUpuc" tagprefix="uc1" %>

<%@ Register src="../ManyFaultGriduc.ascx" tagname="ManyFaultGriduc" tagprefix="uc2" %>


<%@ Register src="ConcessionHistoryuc.ascx" tagname="ConcessionHistoryuc" tagprefix="uc3" %>


<asp:Panel ID="ConcessionSelectionPanel" runat="server">
    <div>

                <fieldset style="width: auto;">
                    <legend>Open Concessions</legend>
                    <asp:GridView ID="ConcessionGrid" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        DataKeyNames="incidentID" PageSize="5"
                        
                        AllowPaging="True" OnPageIndexChanging="ConcessionGrid_PageIndexChanging"
                        OnRowCommand="FaultGridView_RowCommand"
                        ForeColor="#333333" GridLines="None" EmptyDataText="No Data To Display" EmptyDataRowStyle-ForeColor="White" EmptyDataRowStyle-BackColor="Black" Font-Bold="True">
                        <RowStyle CSSClass="grows" />
                         
                        <PagerStyle CssClass="cssPager" />
                        <Columns>
                            <asp:BoundField DataField="incidentID" HeaderText="incidentID" InsertVisible="False"
                                ReadOnly="True" SortExpression="incidentID" />
                            <asp:BoundField DataField="ConcessionNumber" HeaderText="Concession Number"
                                SortExpression="ConcessionNumber" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ConcessionDescription" HeaderText="Concession Description"
                                SortExpression="Description" />
                            <asp:BoundField DataField="Action" HeaderText="Concession Action"
                                SortExpression="Action" />
                            <asp:BoundField DataField="DateInserted" HeaderText="Date Reported"
                                SortExpression="DateReported" />

                            <asp:ButtonField ButtonType="Button" CommandName="View" Text="View Concession" />
                            <asp:ButtonField ButtonType="Button" CommandName="Faults" Text="View Faults" />

                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>

                </fieldset>
                <br />

</div>
</asp:Panel>

<asp:Panel ID="SelectedDisplaysPanel" runat="server">
    <asp:PlaceHolder ID="ConcessionPopupPlaceHolder" runat="server"></asp:PlaceHolder>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <table style="width: 100%;">
                <tr>
                    <td>

                        <%--<asp:GridView ID="GridView2" runat="server" AllowPaging="True"
                            AutoGenerateColumns="False" CellPadding="4" DataKeyNames="TrackingID"
                            EnableViewState="False" ForeColor="#333333" GridLines="None">
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="TrackingID" HeaderText="TrackingID"
                                    InsertVisible="False" ReadOnly="True" SortExpression="TrackingID" Visible="false" />
                                <asp:BoundField DataField="TrackingComment" HeaderText="Comment"
                                    SortExpression="TrackingComment" />
                                <asp:BoundField DataField="AssignedTo" HeaderText="Assigned To"
                                    SortExpression="AssignedTo" />
                                <asp:BoundField DataField="Status" HeaderText="Status"
                                    SortExpression="Status" />
                                <asp:BoundField DataField="LastupdatedBy" HeaderText="Lastupdated By"
                                    SortExpression="LastupdatedBy" />
                                <asp:BoundField DataField="LastupdatedOn" HeaderText="Lastupdated On"
                                    SortExpression="LastupdatedOn" />
                                <asp:BoundField DataField="ConcessionNumber" HeaderText="Concession Number"
                                    SortExpression="ConcessionNumber" ItemStyle-HorizontalAlign="Center" />

                            </Columns>
                            <EditRowStyle BackColor="#99FF33" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        </asp:GridView>--%>

                         <uc3:ConcessionHistoryuc ID="ConcessionHistoryuc1" runat="server" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="HideFaultsclinicalview" runat="server" Text="Hide Concession History" CausesValidation="false" />
                    </td>
                </tr>
            </table>
           
        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:PlaceHolder ID="PlaceHolderFaults" runat="server"></asp:PlaceHolder>
            <br />
            <asp:Button ID="Hidefaults" runat="server" CausesValidation="False" Visible="false" Text="Close" />
        </asp:View>
    </asp:MultiView>
</asp:Panel>







         









         

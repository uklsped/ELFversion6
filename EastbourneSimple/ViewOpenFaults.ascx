<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewOpenFaults.ascx.vb" Inherits="ViewOpenFaults" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="ManyFaultGriduc.ascx" TagName="ManyFaultGriduc" TagPrefix="uc2" %>

<%@ Register Src="controls/DeviceReportedfaultuc.ascx" TagName="DeviceReportedfaultuc" TagPrefix="uc5" %>

<%@ Register Src="controls/DeviceRepeatFaultuc.ascx" TagName="DeviceRepeatFaultuc" TagPrefix="uc3" %>

<%@ Register src="controls/CommentBoxuc.ascx" tagname="CommentBoxuc" tagprefix="uc4" %>

<%@ Register src="controls/FaultTrackinguc.ascx" tagname="FaultTrackinguc" tagprefix="uc6" %>
<%@ Register Src="~/controls/DeviceRepeatFaultuc.ascx" TagPrefix="uc2" TagName="DeviceRepeatFaultuc" %>


<style type="text/css">
    .style2 {
        width: 474px;
        height: 26px;
    }

    .style3 {
        height: 26px;
    }

    .cssPager span {
        background-color: #4f6b72;
        font-size: 18px;
    }
</style>

<br />
<br />

<asp:Panel ID="UpdatePanel4" runat="server">


    <asp:Label ID="FakeLabel" runat="server" Style="display: none;" />
    <asp:Table ID="Table3" runat="server">
        <asp:TableRow>
            <asp:TableCell>
                <uc2:ManyFaultGriduc ID="ManyFaultGriduc" NewFault="false" runat="server" />
            </asp:TableCell>
            <asp:TableCell>
                <fieldset style="width: auto;">
                    <legend>Open Concessions</legend>
                    <asp:GridView ID="ConcessionGrid" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        DataKeyNames="incidentID" PageSize="5"
                        Style="top: 670px; left: 10px; height: 162px; width: 617px"
                        AllowPaging="True" OnPageIndexChanging="ConcessionGrid_PageIndexChanging"
                        OnRowCommand="FaultGridView_RowCommand"
                        ForeColor="#333333" GridLines="None" EmptyDataText="No Data To Display" EmptyDataRowStyle-ForeColor="White" EmptyDataRowStyle-BackColor="Black" Font-Bold="True">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" />
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

                            <asp:ButtonField ButtonType="Button" CommandName="View" Text="View Concession History" />
                            <asp:ButtonField ButtonType="Button" CommandName="Faults" Text="View Faults" />
                            <asp:ButtonField ButtonType="Button" CommandName="Log Fault" Text="Log Repeat Fault" />
                            <asp:TemplateField>
                                <ItemTemplate></ItemTemplate>
                            </asp:TemplateField>
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
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>

</asp:Panel>

<asp:Panel ID="UpdatePanel3" runat="server">

    <asp:Panel ID="Panel4" runat="server">

       <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="UpdatefaultView" runat="server">
                <asp:Panel ID="UpdatePanelRepeatFault" runat="server" Visible="false">
                    <uc2:DeviceRepeatFaultuc runat="server" ID="DeviceRepeatFaultuc1" />
                    <fieldset style="width: 700px;">
                        <legend>Repeat Faults</legend>
                        <fieldset style="width: 700px;">
                            <legend>Record Repeat Fault</legend>
                            <asp:Table runat="server">
                            <asp:TableRow runat="server">
                                <asp:TableCell>
                                    <asp:Label ID="Label1" runat="server" Text="Concession: "></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                     <asp:Label ID="Label5" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="Label2" runat="server" Text="" Visible="false"></asp:Label>
                                   </asp:TableCell>
                                <asp:TableCell>
                                </asp:TableCell>
                                <asp:TableCell>
                                </asp:TableCell>
                            </asp:TableRow>
                             </asp:Table>  
                            
                            <asp:PlaceHolder ID="PlaceholderRepeatFault" runat="server"></asp:PlaceHolder>
                                   
                            <asp:Button ID="ViewExistingFaults" runat="server" Text="View Associated Faults" CausesValidation="false" />
                                    
                                
                        </fieldset>
                        <fieldset style="width: auto">
                            <legend>Associated Faults
                            </legend>
                            <asp:Panel ID="UpdatePanelVEF" runat="server">
                                 <asp:PlaceHolder ID="PlaceHolder3" runat="server"></asp:PlaceHolder>
                            </asp:Panel>
                        </fieldset>
                    </fieldset>
                </asp:Panel>
         </asp:View>
             
         <asp:View ID="View1" runat="server">
             <asp:MultiView ID="MultiView2" runat="server">
                 <asp:View ID="statustech" runat="server">
                     <asp:Panel ID="statustechpanel" runat="server" Visible="false">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server"><ContentTemplate>
                            <uc6:FaultTrackinguc ID="FaultTrackinguc1" runat="server" />
                         
                         </ContentTemplate></asp:UpdatePanel>
                         
                       </asp:Panel>
                        
                     </asp:View>
                 <asp:View ID="statusother" runat="server">
                     <asp:Panel ID="UpdatePanel1" runat="server" Visible="false">
                                    <asp:Panel ID="Panel1" runat="server">
                                        <asp:GridView ID="GridView2" runat="server" AllowPaging="True"
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
                                        </asp:GridView>

                                    </asp:Panel>

                                </asp:Panel>
                 </asp:View>
              </asp:MultiView>
         </asp:View>
         <asp:View ID="View2" runat="server">
             <asp:Panel ID="UpdatePanel2" runat="server" Visible="false">
                 <asp:Panel ID="Panel3" runat="server">
                     <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                 </asp:Panel>
             </asp:Panel>
         </asp:View>
   
         </asp:MultiView>
</asp:Panel>
    
</asp:Panel>
<asp:Button ID="Hidefaults" runat="server" CausesValidation="False" Visible="false" Text="Close" />




         

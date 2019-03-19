﻿<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ManyFaultGriduc.ascx.vb" Inherits="ManyFaultGriduc" %>

<fieldset style="width:auto">
               <asp:UpdatePanel ID="UpdatePanelVEF"  runat="server">
                                                    <ContentTemplate>         
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View id="Linacs" runat="server">
            <asp:GridView ID="GridViewLinac" AutoGenerateColumns="false" runat="server"
                        CellPadding="4" DataKeyNames="IncidentID" AllowPaging="True" PageSize="3"
        EnableViewState="False" ForeColor="#333333" GridLines="None" 
        onrowcommand="NewFaultGridView_RowCommand"
        AllowSorting="True">
        <RowStyle CSSClass="grows" BackColor="#E3EAEB" />
        <AlternatingRowStyle BackColor="White" />
        <Columns>
           <asp:BoundField DataField="IncidentID" HeaderText="Incident ID" InsertVisible="False" 
                ReadOnly="True"
                 />
           <%-- <asp:BoundField DataField="FaultID" HeaderText="Fault ID"
                SortExpression="FaultID" />--%>
           <%-- <asp:BoundField DataField="ConcessionNumber" HeaderText="Concession Number"
                SortExpression="ConcessionNumber" />--%>
             <asp:BoundField DataField="RadiationIncident" HeaderText="Radiation Incident"
                 />
            <asp:BoundField DataField="Description" HeaderText="User Description" 
                 HeaderStyle-Width="350px" ItemStyle-Width="350px" />
            <asp:BoundField DataField="ReportedBy" HeaderText="Reported By" 
                />
            <asp:BoundField DataField="DateReported" HeaderText="Date Reported" 
                />
                <asp:BoundField DataField="Area" HeaderText="Area"  />
                <asp:BoundField DataField="Energy" HeaderText="Energy" />
                <asp:BoundField DataField="GantryAngle" HeaderText="Gantry Angle"  />
                     <asp:BoundField DataField="CollimatorAngle" HeaderText="Collimator Angle"  />      
                           <%--<asp:BoundField DataField="Linac" HeaderText="Linac" 
                SortExpression="Linac" />--%>
         <asp:ButtonField ButtonType="Button" CommandName="View" Text="View Fault" Visible="False" />

        </Columns>
        <EditRowStyle BackColor="#7C6F57" />
        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
    </asp:GridView>
    
     </asp:View>
        <asp:View ID="Tomo" runat="server">
              <asp:GridView ID="GridViewTomo" AutoGenerateColumns="false" runat="server"
                        CellPadding="4" DataKeyNames="IncidentID" 
        EnableViewState="False" ForeColor="#333333" GridLines="None" 
        onrowcommand="NewFaultGridView_RowCommand"
        AllowSorting="True">
        <RowStyle BackColor="#E3EAEB" />
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="IncidentID" HeaderText="Incident ID" InsertVisible="False" ReadOnly="True" />
             <asp:BoundField DataField="FaultID" HeaderText="Fault ID" />
             <asp:BoundField DataField="RadiationIncident" HeaderText="Radiation Incident" />
            <asp:BoundField DataField="Description" HeaderText="User Description" />
            <asp:BoundField DataField="ReportedBy" HeaderText="Reported By" />
            <asp:BoundField DataField="DateReported" HeaderText="Date Reported" />
                <asp:BoundField DataField="Area" HeaderText="Physics/Accuray" />
                <asp:BoundField DataField="Energy" HeaderText="Error Code"  />   
                          <%-- <asp:BoundField DataField="Linac" HeaderText="Tomo" 
                SortExpression="Linac" />--%>
         <asp:ButtonField ButtonType="Button" CommandName="View" Text="View Fault" Visible="False" />

        </Columns>
        <EditRowStyle BackColor="#7C6F57" />
        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
    </asp:GridView>

        </asp:View>
        </asp:MultiView>
                            
                    </ContentTemplate>
                        </asp:UpdatePanel>
           
                           </fieldset>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ManyFaultGriduc.ascx.vb" Inherits="ManyFaultGriduc" %>


                       <fieldset style="width:auto">
                       
                       <asp:UpdatePanel ID="UpdatePanelVEF"  runat="server">
                            
                        <ContentTemplate>
                        <asp:GridView ID="GridView4" AutoGenerateColumns="false" runat="server"
                        CellPadding="4" DataKeyNames="IncidentID" 
        EnableViewState="False" ForeColor="#333333" GridLines="None" 
        onrowcommand="NewFaultGridView_RowCommand"
        AllowSorting="True">
        <RowStyle BackColor="#E3EAEB" />
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="IncidentID" HeaderText="Incident ID" InsertVisible="False" 
                ReadOnly="True"
                SortExpression="IncidentID" />
            <asp:BoundField DataField="Description" HeaderText="User Description" 
                SortExpression="Description" />
            <asp:BoundField DataField="ReportedBy" HeaderText="Reported By" 
                SortExpression="ReportedBy" />
            <asp:BoundField DataField="DateReported" HeaderText="Date Reported" 
                SortExpression="DateReported" />
                <asp:BoundField DataField="Area" HeaderText="Area" SortExpression="Area" />
                <asp:BoundField DataField="Energy" HeaderText="Energy" SortExpression="Energy" />
                <asp:BoundField DataField="GantryAngle" HeaderText="GantryAngle" SortExpression="GantryAngle" />
                     <asp:BoundField DataField="CollimatorAngle" HeaderText="CollimatorAngle" SortExpression="CollimatorAngle" />      
                           <asp:BoundField DataField="Linac" HeaderText="Linac" 
                SortExpression="Linac" />
         <asp:ButtonField ButtonType="Button" CommandName="View" Text="View Fault" Visible="False" />

        </Columns>
        <EditRowStyle BackColor="#7C6F57" />
        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
    </asp:GridView>
    </fieldset>
    </fieldset>
                            
                    </ContentTemplate>
                        </asp:UpdatePanel>
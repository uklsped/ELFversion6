<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AtlasEnergyViewuc.ascx.vb" Inherits="AtlasEnergyViewuc" %>

<%--<asp:Button ID="ViewAtlasEnergies" runat="server" CausesValidation="False" 
    Text="View Atlas Energies" />--%>

<asp:UpdatePanel ID="UpdatePanel3" runat="server"  
    Visible="true">
    <ContentTemplate>
        <asp:Table ID="Table1" runat="server">
        <asp:TableRow runat="server">
        <asp:TableCell runat="server">
        <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" 
            CellPadding="4" ForeColor="#333333" 
            GridLines="None" 
            style="top: 670px; left: 10px; height: 162px; width: 600px" Font-Size="Small" 
            HorizontalAlign="Left" UseAccessibleHeader="False" >
            <RowStyle BackColor="#E3EAEB" HorizontalAlign="Left" />
            <Columns>
                <%--<asp:BoundField DataField="machinename" HeaderText="machinename" 
                    SortExpression="machinename" />--%>
<%--                <asp:BoundField DataField="Beam" HeaderText="Beam" ReadOnly="True" 
                    SortExpression="Beam" />--%>
                <asp:BoundField DataField="Test Name" HeaderText="Test Name" 
                    SortExpression="Test Name" />
                <asp:BoundField DataField="result" HeaderText="result" 
                    SortExpression="result" />
                <asp:BoundField DataField="Test Date" HeaderText="Test Date" ReadOnly="True" 
                    SortExpression="Test Date" />
                <asp:BoundField DataField="comment" HeaderText="comment" 
                    SortExpression="comment" />
                <%--<asp:BoundField DataField="accountname" HeaderText="accountname" 
                    SortExpression="accountname" />--%>
                    
            </Columns>
<%--           <emptydatarowstyle backcolor="LightBlue"
          forecolor="Red"/>

        <emptydatatemplate>

                      No Records Found.  

        </emptydatatemplate>--%>
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="center" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
            <EditRowStyle BackColor="#7C6F57" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        </asp:TableCell>
        <%--<asp:TableCell>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" GridLines="None" ForeColor="#333333">
            <Columns>
            <asp:BoundField DataField="Energy" HeaderText="Last Runup" 
                    SortExpression="Energy" />
            <asp:TemplateField>
            <ItemTemplate>
            <asp:CheckBox runat="server" ID="RowlevelCheckBox" />
            </ItemTemplate>
            </asp:TemplateField>
            </Columns>
            </asp:GridView>
        
        </asp:TableCell>--%>
        </asp:TableRow>
</asp:Table>
        
<%--        <asp:Button ID="HideAtlas" runat="server" CausesValidation="false" 
            Text="Hide" />--%>
            
            
    </ContentTemplate>
</asp:UpdatePanel>
<br />



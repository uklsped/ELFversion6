<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EngApproveuc.ascx.vb" Inherits="controls_EngApproveuc" %>

         <div>
        <asp:Panel ID="Panel1" runat="server" BackColor="#99CCFF" BorderColor="#0033CC" 
        BorderStyle="Solid">
           
        <asp:Table ID="Table2" runat="server" CellSpacing="20" GridLines="Both" 
               Width="1500px">
               <asp:TableRow ID="TableRow1" runat="server">
                   <asp:TableCell ID="TableCell1" runat="server" Width="250px" HorizontalAlign="Left">
                   <asp:GridView ID="EnergyGridView" visible="false" runat="server" AutoGenerateColumns="False" >
            <Columns>
            
                <asp:BoundField DataField="Energy" HeaderText="Select All Energies" 
                    SortExpression="Energy" />
                    <asp:TemplateField>  
            <HeaderTemplate>  
                <asp:CheckBox ID="chkSelectAll"   
                    runat="server" 
                    AutoPostBack="true"  
                    OncheckedChanged="checked"
                     />  
            </HeaderTemplate>
                    
            <ItemTemplate>
            <asp:CheckBox runat="server" ID="RowlevelCheckBox" />
            </ItemTemplate>
            </asp:TemplateField>
            </Columns>
        </asp:GridView>
               
</asp:TableCell>
                   <asp:TableCell ID="imagingcell" runat="server" Width =" 250px" HorizontalAlign="Left">
                       <asp:GridView ID="GridViewImage" visible="false" runat="server" AutoGenerateColumns="False" >
            <Columns>
            
                <asp:BoundField DataField="Energy" HeaderText="Select Imaging" 
                    SortExpression="Energy" />
                    <asp:TemplateField>  
                             
            <ItemTemplate>
            <asp:CheckBox runat="server" ID="RowlevelCheckBoxImage" />

            </ItemTemplate>
                      </asp:TemplateField>
            </Columns>
        </asp:GridView>
                   </asp:TableCell>
                 
                   <asp:TableCell ID="TableCell2" runat="server" Width="50px">
                       
                   <asp:Button ID="engHandoverButton" runat="server" Text="Approve Energies" causesvalidation="false"  Height="150px" BackColor="#FFCC00"/>


</asp:TableCell>
                   <asp:TableCell ID="TableCell3" runat="server" HorizontalAlign="left" Width="250px">
                               <legend align="top" style="font-family: Arial, Helvetica, sans-serif; font-weight: bold">Run-up Comments</legend>
                             <asp:TextBox ID="CommentBox" runat="server" MaxLength="250" Rows="5" 
                TextMode="multiline"  Width="100px" Height="100px" AutoPostBack="true"></asp:TextBox>

                       </asp:TableCell>
                   </asp:TableRow>
            </asp:Table>
           </asp:Panel>
             <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
            <asp:PlaceHolder ID="PlaceHolder4" runat="server">
           <%-- <uc4:WriteDatauc ID="WriteDatauc1" LinacName="Linac" UserReason="0"  Tabby="TabNumber"  WriteName="EngData"   Visible="false" runat="server" />--%>
            </asp:PlaceHolder>
            </ContentTemplate>
            </asp:UpdatePanel>
             </div>
  
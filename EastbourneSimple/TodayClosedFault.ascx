<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TodayClosedFault.ascx.vb" Inherits="TodayClosedFault" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<link href="App_Themes/Blue/Elf.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">

    </script>

  


    <asp:HiddenField ID="HiddenField1" runat="server"/>
    
   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" ><ContentTemplate>
       
    <asp:Table ID="Table1" runat="server" HorizontalAlign="Left" Height="16px">
       <asp:TableRow HorizontalAlign="Left" VerticalAlign="Top">
       <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">   
       <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" 
                        DataKeyNames="incidentID"  
                        style="top: 550px; left: 10px; height: 162px; width: 550px;" 
                        ForeColor="#333333" GridLines="None" AllowPaging="True"  PageSize="10">
                        

<RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" />

                                                
<Columns>
                            
<asp:BoundField DataField="incidentID" HeaderText="incidentID" InsertVisible="False" 
                                ReadOnly="True" SortExpression="incidentID" />
                                
<asp:BoundField DataField="DateInserted" HeaderText="Date Reported" 
                                SortExpression="DateReported" />
                                
<asp:BoundField DataField="DateClosed" HeaderText="Date Closed" 
                                SortExpression="DateClosed" />
                                
                               
<asp:BoundField DataField="Description" HeaderText="Original Fault Description" 
                                SortExpression="Description" />
                            
<asp:BoundField DataField="ConcessionNumber" HeaderText="Concession Number" 
                                SortExpression="ConcessionNumber" ItemStyle-HorizontalAlign="Center" >
                            
<ItemStyle HorizontalAlign="Center" />
                            
</asp:BoundField>
                            
<asp:BoundField DataField="ConcessionDescription" HeaderText="Concession Description" 
                                SortExpression="Description" />
                            
<asp:BoundField DataField="Linac" HeaderText="Linac" 
                                SortExpression="Linac" />

                        
</Columns>
                        

<FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                        

           <PagerSettings Mode="NumericFirstLast" />
                        

<PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        

<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        

<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        

<EditRowStyle BackColor="#999999" />
                        
<AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    
</asp:GridView>

       
</asp:TableCell>
       <%-- <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
       <fieldset style="width:300px;">
                

          <legend>Reported Fault Details</legend>

    <asp:DataList ID="DatalistFaults" runat="server" CellPadding="4" ForeColor="#333333" 
                 
                 GridLines="Both" BorderColor="Blue" BorderWidth="1px">
        
<AlternatingItemStyle BackColor="White" ForeColor="#284775" HorizontalAlign="Left" VerticalAlign="Top" />
        

<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        

<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        

<ItemStyle BackColor="#F7F6F3" ForeColor="#333333" />
    
<ItemTemplate>
    Fault ID: <strong><%# Eval("FaultID")%></strong><br />
    Area: <strong><%# Eval("Area")%></strong><br />
    Energy: <strong><%# Eval("Energy")%></strong><br />
    Gantry Angle: <strong><%# Eval("GantryAngle")%></strong><br />
    Collimator Angle: <strong><%# Eval("CollimatorAngle")%></strong><br />
    Description: <strong><%# Eval("Description")%></strong><br />
    Patient ID: <strong><%# Eval("BSUHID")%></strong><br />
    Reported By: <strong><%# Eval("ReportedBy")%></strong><br />
    Date Reported: <strong><%# Eval("DateReported")%></strong><br />
    
</ItemTemplate>
        

<SelectedItemStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
     
</asp:DataList>
      
       
</asp:TableCell>
       <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
       <asp:DataList ID="trackingHistory" runat="server" CellPadding="4" ForeColor="#333333" >
        
<AlternatingItemStyle BackColor="White" ForeColor="#284775" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        

<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        

<ItemStyle BackColor="#F7F6F3" ForeColor="#333333" />
    
<ItemTemplate>
   Tracking ID: <strong><%# Eval("TrackingID")%></strong><br />
   Tracking Comment: <strong><%# Eval("trackingcomment")%></strong><br />
   Assigned To: <strong><%# Eval("AssignedTo")%></strong><br />
   Status To: <strong><%# Eval("Status")%></strong><br />
   Update By: <strong><%# Eval("LastUpDatedBy")%></strong><br />
   Updated On: <strong><%# Eval("LastUpDatedOn")%></strong><br />
   Linac: <strong><%# Eval("linac")%></strong><br />
   Concession Action: <strong><%# Eval("action")%></strong><br />
   
</ItemTemplate>


</asp:DataList>
       
</asp:TableCell>--%>
</asp:TableRow>
    </asp:Table>

</ContentTemplate>
    </asp:UpdatePanel>
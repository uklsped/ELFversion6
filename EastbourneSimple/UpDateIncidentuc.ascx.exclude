<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UpDateIncidentuc.ascx.vb" Inherits="UpDateIncidentuc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register src="WriteDatauc.ascx" tagname="WriteDatauc" tagprefix="uc1" %>

<%--<uc1:WriteDatauc ID="WriteDatauc1" LinacName="LA1" UserReason="4"  Tabby="Fault"  WriteName="Fault" visible="false" runat="server" />--%>
 <script language="javascript" type="text/javascript">
     var win = null;
     function OpenPopUp(mypage, myname, w, h, scroll, pos) {
         if (pos == "random") { LeftPosition = (screen.width) ? Math.floor(Math.random() * (screen.width - w)) : 100; TopPosition = (screen.height) ? Math.floor(Math.random() * ((screen.height - h) - 75)) : 100; }
         if (pos == "center") { LeftPosition = (screen.width) ? (screen.width - w) / 2 : 100; TopPosition = (screen.height) ? (screen.height - h) / 2 : 100; }
         else if ((pos != "center" && pos != "random") || pos == null) { LeftPosition = 0; TopPosition = 20 }
         settings = 'width=' + w + ',height=' + h + ',top=' + TopPosition + ',left=' + LeftPosition + ',scrollbars=' + scroll + ',location=no,directories=no,status=no,menubar=no,toolbar=no,resizable=yes';
         win = window.open(mypage, myname, settings);
     }
</script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ><ContentTemplate>
     <uc1:WriteDatauc ID="WriteDatauc3" LinacName="LA1" UserReason="4"  Tabby="incident"  WriteName="incident" visible="false" runat="server" />

        <asp:Panel ID="Panel3" runat="server" BorderColor="#33CC33" BorderStyle="Solid">
            <asp:Table ID="Table2" runat="server" >
                
            <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="Label1" runat="server" Text="Incident ID"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                
                
            </asp:TableCell>
            <asp:TableCell>
                
            </asp:TableCell>
            </asp:TableRow>
            </asp:Table>
            <asp:Table ID="Table1" runat="server" Width="943px">
            <asp:TableRow>
                
    <asp:TableCell>
        <fieldset style="width:550px;">
            <legend>Update Fault Details</legend>    
    <table style="width: 150px;">
        <tr>
        <td>
        <asp:Label ID="StatusLabel" runat="server" Text="Current Status:"></asp:Label>
        </td>
        <td>
        <asp:Label ID="StatusLabel1" runat="server" Text=""></asp:Label>
        </td></tr>
        <tr>
            <td>
                Update Fault Status:
            </td>
            <td>
               <asp:UpdatePanel ID="UpdatePanelStatuslist" runat="server"><ContentTemplate>
                    <asp:Panel ID="Panel1" runat="server">
                    
<asp:DropDownList ID="DropDownList1" autopostback="false" runat="server">
<asp:ListItem>Select</asp:ListItem>
<asp:ListItem>Open</asp:ListItem>
<asp:ListItem>Concession</asp:ListItem>
<asp:ListItem>Closed</asp:ListItem>
</asp:DropDownList>
</asp:Panel>
        </ContentTemplate>
        <%--<Triggers><asp:AsyncPostBackTrigger ControlID="DropDownList1" EventName="SelectedIndexChanged" /></Triggers>--%>
                </asp:UpdatePanel>

            </td>
            <td><asp:Label ID="ConcessionLabel" runat="server" Text="Concession Number"></asp:Label></td>
            <td>
                
            <asp:TextBox ID="ConcessionNumberBox" runat="server" ReadOnly="True" Visible="True"></asp:TextBox>
            </td>
            
                    </tr>
        <tr>
            <td>
                Assign To:
            </td>
            <td>
<asp:DropDownList ID="DropDownList2" runat="server">
<asp:ListItem>Unassigned</asp:ListItem>
<asp:ListItem>Engineering</asp:ListItem>
<asp:ListItem>Physics</asp:ListItem>
<asp:ListItem>Software</asp:ListItem>
</asp:DropDownList>
            

            </td>
            <td><asp:Label ID="ConcessiondescriptionLabel" runat="server" Text="Concession Description"></asp:Label>
            <td><asp:TextBox ID="ConcessiondescriptionBox" runat="server" ReadOnly="True" Visible="True" MaxLength="20"></asp:TextBox>
            </td>
            </td>
            </tr>
        <tr>
            <td>
                Comment:
                
            </td>
            <td>
                <asp:TextBox ID="CommentBox1" runat="server" readonly="false" TextMode="MultiLine"></asp:TextBox>
                

                   </td>
                   <td>
                   Action:</td>
                   <td>
                       <asp:TextBox ID="ActionBox" TextMode="MultiLine" runat="server"></asp:TextBox>
                   </td>
            </tr>
            <tr>
            <td><asp:Button ID="SaveAFault" runat="server" Text="Save"  CausesValidation="false"  />
                                             <td><asp:Button ID="CancelButton" runat="server" Text="Cancel" causesvalidation="false"/>
</td>
            </tr>
            
    </table>
    </fieldset>
    </asp:TableCell>
    </asp:TableRow>
    </asp:Table>
        </asp:Panel>
        <asp:Panel ID="Panel2" runat="server" BorderStyle="Solid" BorderColor="Yellow">

  

                <asp:GridView ID="GridView2" runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" CellPadding="4" DataKeyNames="TrackingID" 
                    EnableViewState="False" ForeColor="#333333" GridLines="None">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="TrackingID" HeaderText="TrackingID" 
                            InsertVisible="False" ReadOnly="True" SortExpression="TrackingID" />
                        <asp:BoundField DataField="TrackingComment" HeaderText="TrackingComment" 
                            SortExpression="TrackingComment" />
                        <asp:BoundField DataField="AssignedTo" HeaderText="AssignedTo" 
                            SortExpression="AssignedTo" />
                        <asp:BoundField DataField="Status" HeaderText="Status" 
                            SortExpression="Status" />
                        <asp:BoundField DataField="LastupdatedBy" HeaderText="LastupdatedBy" 
                            SortExpression="LastupdatedBy" />
                        <asp:BoundField DataField="LastupdatedOn" HeaderText="LastupdatedOn" 
                            SortExpression="LastupdatedOn" />

                    </Columns>
                    <EditRowStyle BackColor="#99FF33" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                </asp:GridView>
        

       </asp:Panel> 
</ContentTemplate>
<%--<Triggers><asp:AsyncPostBackTrigger ControlID="DropDownList1" EventName="SelectedIndexChanged" /></Triggers>
--%>
    </asp:UpdatePanel>

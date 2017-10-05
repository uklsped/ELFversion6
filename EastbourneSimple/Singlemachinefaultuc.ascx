<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Singlemachinefaultuc.ascx.vb" Inherits="Singlemachinefaultuc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register src="WriteDatauc.ascx" tagname="WriteDatauc" tagprefix="uc1" %>

<%@ Register src="ViewOpenFaults.ascx" tagname="ViewOpenFaults" tagprefix="uc2" %>

<uc1:WriteDatauc ID="WriteDatauc1" LinacName="LA1" UserReason="4"  Tabby="Fault"  WriteName="Fault" visible="false" runat="server" />
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
<%--<asp:Button ID="Viewopenfaultsbutton" runat="server" Text="View Open Faults" causesvalidation="false"/>--%>
<%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
<ContentTemplate>
    <asp:PlaceHolder ID="Placeholder1" visible="false" runat="server"></asp:PlaceHolder>
    </ContentTemplate>
</asp:UpdatePanel>--%>
<asp:UpdatePanel ID="newfaultUpdatePanel1" Visible="false" runat="server"><ContentTemplate >
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
        
<asp:Panel ID="Panel1" runat="server" Width="949px" BorderColor="#3399FF" 
    BorderStyle="Solid">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        CellPadding="4" DataKeyNames="incidentID" 
        EnableViewState="False" ForeColor="#333333" GridLines="None" 
        OnPageIndexChanging="GridView1_PageIndexChanging" AllowPaging="True" 
        AllowSorting="True">
        <RowStyle BackColor="#E3EAEB" />
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="incidentID" HeaderText="incidentID" InsertVisible="False" 
                ReadOnly="True"
                SortExpression="incidentID" />
            <asp:BoundField DataField="Description" HeaderText="User Description" 
                SortExpression="Description" />
            <asp:BoundField DataField="ReportedBy" HeaderText="Reported By" 
                SortExpression="ReportedBy" />
            <asp:BoundField DataField="DateReported" HeaderText="Date Reported" 
                SortExpression="DateReported" />
            <asp:BoundField DataField="Status" HeaderText="Fault Status" 
                SortExpression="Status" />
              <asp:BoundField DataField="ConcessionNumber" HeaderText="Concession Number" 
                SortExpression="ConcessionNumber" />
           <asp:BoundField DataField="Linac" HeaderText="Linac" 
                SortExpression="Linac" />
           <asp:BoundField DataField="FaultID" HeaderText="FaultID"  SortExpression="FaultID" />
            <asp:CommandField ButtonType="Button" HeaderText="Select" ShowHeader="true" 
                ShowSelectButton="true" visible="false"/>
        </Columns>
        <EditRowStyle BackColor="#7C6F57" />
        <EmptyDataTemplate>
            FaultID
        </EmptyDataTemplate>
        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
    </asp:GridView>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="false" ><ContentTemplate>
        <asp:Panel ID="Panel3" runat="server" BorderColor="#33CC33" BorderStyle="Solid">
            <asp:Table ID="Table2" runat="server" >
                
            <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="Label1" runat="server" Text="Fault ID"></asp:Label>
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
            <fieldset style="width:300px;">
                

            <legend>Reported Fault Details</legend>
                <table style="width:300px;">                
        <tr>           
            <td class="style1">
                Area:</td>
            <td>
    <asp:TextBox ID="AreaBox" runat="server"></asp:TextBox>
            </td>
        </tr>
         <tr>
            
            <td class="style1">

                Energy:</td>
            <td>      

    <asp:TextBox ID="EnergyBox" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style1">
                Gantry Angle:</td>
            <td>
    <asp:TextBox ID="GantryBox" runat="server"></asp:TextBox>

            </td>
        </tr>
        <tr>

            <td class="style1">
                
                Collimator Angle:</td>
            <td>
                
    <asp:TextBox ID="CollBox" runat="server"></asp:TextBox>

            </td>
            </tr>
            <tr>
                
        <td class="style1">
                
                Fault Description:</td>
            <td>
                
              <asp:TextBox ID="DescriptionBox" runat="server" MaxLength="250" ReadOnly="True" 
                    Rows="5" TextMode="MultiLine"></asp:TextBox>
              </td>
              </tr>
              <tr>

              <td>
                  
              Reported By:</td>
              <td>
                  <asp:TextBox ID="ReportedBox" runat="server" ReadOnly="True"></asp:TextBox>
                </td>
              </tr>
              <tr>
              <td>
                  
              Date Open:</td>
              <td>
                  <asp:TextBox ID="OpenDateBox" runat="server" ReadOnly="True"></asp:TextBox>
              </td>
        </tr>
    </table>
    </fieldset>
    </asp:TableCell>
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
                   </td>
            </tr>
        <tr>
            
        <td>
            
            Update Fault Status:
        </td>
        <td>
            
            <asp:UpdatePanel ID="UpdatePanelStatuslist" runat="server"><ContentTemplate>
<asp:DropDownList ID="DropDownList1" autopostback="true" runat="server">
<asp:ListItem>Select</asp:ListItem><asp:ListItem>Open</asp:ListItem><asp:ListItem>Concession</asp:ListItem><asp:ListItem>Closed</asp:ListItem></asp:DropDownList>
            </ContentTemplate></asp:UpdatePanel>
        </td>
            <td>
                <asp:Label ID="ConcessionLabel" runat="server" Text="Concession Number"></asp:Label>
            </td>
            <td>
                
                <asp:TextBox ID="ConcessionNumberBox" runat="server" ReadOnly="True" Visible="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            
            <td>
                Assign To: </td>
            <td>

                <asp:DropDownList ID="DropDownList2" runat="server">
<asp:ListItem>Unassigned</asp:ListItem><asp:ListItem>Engineering</asp:ListItem><asp:ListItem>Physics</asp:ListItem><asp:ListItem>Software</asp:ListItem></asp:DropDownList>

            </td>
            <td>

                <asp:Label ID="ConcessiondescriptionLabel" runat="server" Text="Concession Description"></asp:Label>
                <td>
                    <asp:TextBox ID="ConcessiondescriptionBox" runat="server" ReadOnly="True" Visible="True" MaxLength="20"></asp:TextBox>
                </td>
            </td>
                    </tr>
        <tr>
            <td>
                Comment:
            </td>
            <td> 
                <asp:TextBox ID="CommentBox1" runat="server" TextMode="MultiLine"></asp:TextBox>
            
            </td>
            </tr>
            <tr>
                
            <td><asp:Button ID="SaveFault" runat="server" Text="Save"  CausesValidation="false" />
                                                
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
    <caption>
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
<%--                            <asp:BoundField DataField="ConcessionNumber" HeaderText="Concession Number"
                            SortExpression="ConcessionNumber" />
                            <asp:BoundField DataField="ConcessionDescription" HeaderText="Concession Description"
                            SortExpression="ConcessionDescription" />
                        <asp:BoundField DataField="FaultID" HeaderText="FaultID" 
                            SortExpression="FaultID" />--%>
                    </Columns>
                    <EditRowStyle BackColor="#99FF33" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                </asp:GridView>
        </caption>
   <%-- <asp:Table ID="Table1" runat="server">
    <asp:TableRow>
    <asp:TableCell>
    <asp:Label ID="CauseLabel" runat="server" Text="Cause:"></asp:Label></asp:TableCell>
    <asp:TableCell>
        <asp:TextBox ID="CauseTextBox" runat="server" TextMode="MultiLine"></asp:TextBox></asp:TableCell></asp:TableRow>
<asp:TableRow>
    <asp:TableCell>
    <asp:Label ID="Remedy" runat="server" Text="Remedy:"></asp:Label></asp:TableCell>
    <asp:TableCell>
        <asp:TextBox ID="RemedyTextBox" runat="server" TextMode="MultiLine"></asp:TextBox></asp:TableCell></asp:TableRow>
</asp:Table>--%></asp:Panel>
       
</ContentTemplate>
<Triggers><asp:AsyncPostBackTrigger ControlID="DropDownList1" EventName="SelectedIndexChanged" /></Triggers>
    </asp:UpdatePanel>

</asp:Panel>
</asp:View>
            <asp:View ID="View2" runat="server">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" Visible="false"><ContentTemplate><asp:Panel ID="Panel4" runat="server"><asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder></asp:Panel></ContentTemplate></asp:UpdatePanel>
            </asp:View>
            <asp:view ID="DummyView" runat="server"></asp:view>
            </asp:MultiView>
            </ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlId="GridView1" EventName="SelectedIndexChanged" /></Triggers>
</asp:UpdatePanel>
                






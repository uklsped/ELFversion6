<%@ Page Title="" Language="VB" MasterPageFile="~/Elf.master" AutoEventWireup="false" CodeFile="FaultTracker.aspx.vb" Inherits="FaultTracker" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <script language="javascript" type="text/javascript">

     function characterCounter(controlId, countControlId, maxCharlimit) {
       
         if (controlId.value.length > maxCharlimit)
             controlId.value = controlId.value.substring(0, maxCharlimit);
         else
                           countControlId.value = maxCharlimit - controlId.value.length;
     }
     function LimtCharacters(txtMsg, CharLength, indicator) {
         chars = txtMsg.value.length;
             if (chars > CharLength) 
             txtMsg.value = txtMsg.value.substring(0, CharLength);
             else
             document.getElementById(indicator).innerHTML = CharLength - chars;
        
     }

</script>
    <%--<script language="javascript" type="text/javascript">
   document.getElementById("divGrid").style.width = screen.width + "px";
</script>--%>
<%--Enter Fault ID:--%><br />
<%--    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>--%>
    <fieldset style="width:300px;">
    <legend>Counting Remaining Characters example</legend>
    <label id="lblcount" >30</label> characters left
    <asp:TextBox ID="mytextbox" Width="280px" Rows="4" Columns="12" runat="server" 
     TextMode="MultiLine"    onkeyup="LimtCharacters(this,30,'lblcount');"
     MaxLength="30" />
        </fieldset>



        
        
        
        <asp:LinkButton id="lkCloseWindow" runat="server" Text="Close window" OnClientClick="window.close();"></asp:LinkButton>
      <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
        <asp:TabPanel ID="Linac1" runat="server" HeaderText="LA1" >
            <HeaderTemplate>
                LA1
            
            
</HeaderTemplate>
        
<ContentTemplate>
            <asp:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="1" 
                CssClass=""><asp:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1"></asp:TabPanel>
<asp:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2"><HeaderTemplate>
        TabPanel2
    
</HeaderTemplate>
</asp:TabPanel>
</asp:TabContainer>

            

        
        
    <asp:Label ID="Label1" runat="server" Text="Most Recent Fault"></asp:Label>

    <asp:CreateUserWizard ID="CreateUserWizard1" runat="server">
        <WizardSteps>
            <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                <ContentTemplate>
                    <table border="0">
                        <tr>
                            <td align="center" colspan="2">
                                Sign Up for Your New Account</td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                            </td>
                            <td>
                                
                                <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                    ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                    ToolTip="User Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                    ControlToValidate="Password" ErrorMessage="Password is required." 
                                    ToolTip="Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="ConfirmPasswordLabel" runat="server" 
                                    AssociatedControlID="ConfirmPassword">Confirm Password:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" 
                                    ControlToValidate="ConfirmPassword" 
                                    ErrorMessage="Confirm Password is required." 
                                    ToolTip="Confirm Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Email" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                                    ControlToValidate="Email" ErrorMessage="E-mail is required." 
                                    ToolTip="E-mail is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="RoleLabel" runat="server" AssociatedControlID="UserRole">User Role:</asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="UserRole"  runat="server"  Width="128px">
                    <asp:ListItem>Select</asp:ListItem>
                    <asp:ListItem>Engineer</asp:ListItem>
                    <asp:ListItem>Radiographer</asp:ListItem>
                    <asp:ListItem>Physicist</asp:ListItem>
                    <asp:ListItem>Administrator</asp:ListItem>
                </asp:DropDownList>
                                <%--<asp:TextBox ID="UserRole" runat="server"></asp:TextBox>--%>
                                <asp:RequiredFieldValidator ID="RoleRequired" runat="server" 
                                    ControlToValidate="UserRole" ErrorMessage="User Role is required." 
                                    ToolTip="User Role is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        
                        <tr>
                            <td align="center" colspan="2">
                                <asp:CompareValidator ID="PasswordCompare" runat="server" 
                                    ControlToCompare="Password" ControlToValidate="ConfirmPassword" 
                                    Display="Dynamic" 
                                    ErrorMessage="The Password and Confirmation Password must match." 
                                    ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2" style="color:Red;">
                                <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:CreateUserWizardStep>
            <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
            </asp:CompleteWizardStep>
        </WizardSteps>
    </asp:CreateUserWizard>

<asp:Panel ID="Panel1" runat="server"  
        Width="949px">
     <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="FaultID" DataSourceID="SqlDataSource3" 
        EnableViewState="False" CellPadding="4" ForeColor="#333333" 
            GridLines="None"><Columns>
<asp:BoundField DataField="FaultID" HeaderText="FaultID" InsertVisible="False" 
                 ReadOnly="True" SortExpression="FaultID" />
<asp:BoundField DataField="Description" HeaderText="Description" 
                 SortExpression="Description" />
<asp:BoundField DataField="ReportedBy" HeaderText="ReportedBy" 
                 SortExpression="ReportedBy" />
<asp:BoundField DataField="DateReported" HeaderText="DateReported" 
                 SortExpression="DateReported" />
<asp:BoundField DataField="FaultStatus" HeaderText="FaultStatus" 
                 SortExpression="FaultStatus" />
<asp:BoundField DataField="Energy" HeaderText="Energy" 
                 SortExpression="Energy" />
<asp:BoundField DataField="GantryAngle" HeaderText="GantryAngle" 
                 SortExpression="GantryAngle" />
<asp:BoundField DataField="CollimatorAngle" HeaderText="CollimatorAngle" 
                 SortExpression="CollimatorAngle" />
<asp:CommandField ButtonType="Button" HeaderText="Select" ShowHeader="True" 
                 ShowSelectButton="True" />
</Columns>

<AlternatingRowStyle BackColor="White" />

<EditRowStyle BackColor="#7C6F57" />
<EmptyDataTemplate>
             FaultID
         
</EmptyDataTemplate>

<FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />

<HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />

<PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />

<RowStyle BackColor="#E3EAEB" />

<SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
</asp:GridView>


        </asp:Panel>


    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    Fault Status:
    <asp:DropDownList ID="DropDownList1" runat="server">
        <asp:ListItem>Open</asp:ListItem>
<asp:ListItem>Closed</asp:ListItem>
<asp:ListItem>Concession</asp:ListItem>
<asp:ListItem>Physics</asp:ListItem>
<asp:ListItem Value=" Select">Select</asp:ListItem>
</asp:DropDownList>


    <br />
    
    <br />
    <asp:TextBox ID="TextBox1" runat="server" Visible="False"></asp:TextBox>


    <br />
    <br />
    <br />
    <br />
    <br />
    
    <br />
    Assigned To:&nbsp;&nbsp;     <asp:DropDownList ID="DropDownList2" runat="server"><asp:ListItem>Select</asp:ListItem>
<asp:ListItem>Rob Hughes</asp:ListItem>
<asp:ListItem>Andy Flower</asp:ListItem>
<asp:ListItem>Steve Morgan</asp:ListItem>
<asp:ListItem>John Reardon</asp:ListItem>
<asp:ListItem>Bob Marlow</asp:ListItem>
</asp:DropDownList>


    <br />
    <br />
    Comment:&nbsp;
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>


    <br />
     <asp:Button ID="Button2" runat="server" Text="Save" />


    
        <br />
    

   
    <br />
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
        SelectCommand="select * from reportfault where faultid=(select max(faultid) as mancount from reportfault)"></asp:SqlDataSource>


    <br />
    <br />
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="FaultID" DataSourceID="SqlDataSource1" AllowPaging="True" 
        EnableViewState="False"><Columns>
<asp:BoundField DataField="FaultID" HeaderText="FaultID" InsertVisible="False" 
                ReadOnly="True" SortExpression="FaultID" />
<asp:BoundField DataField="Description" HeaderText="Description" 
                SortExpression="Description" />
<asp:BoundField DataField="ReportedBy" HeaderText="ReportedBy" 
                SortExpression="ReportedBy" />
<asp:BoundField DataField="DateReported" HeaderText="DateReported" 
                SortExpression="DateReported" />
<asp:BoundField DataField="FaultStatus" HeaderText="FaultStatus" 
                SortExpression="FaultStatus" />
<asp:CommandField ButtonType="Button" HeaderText="Select" ShowHeader="True" 
                ShowSelectButton="True" />
</Columns>

<SelectedRowStyle BackColor="#66CCFF" />
</asp:GridView>


    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
        
        SelectCommand="SELECT [FaultID], [Description], [ReportedBy], [DateReported], [FaultStatus] FROM [ReportFault] ORDER BY [FaultID] DESC"></asp:SqlDataSource>


    <br />
    <br />
    
    <br />
    <br />
    <br />
    <br />
    <asp:GridView ID="GridView3" runat="server" 
        DataKeyNames="FaultID,TrackingID" DataSourceID="SqlDataSource2"></asp:GridView>


    
    
    <br />
    <br />
    <br />
    
    
   
    <br />
    
    <br />
    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="TrackingID" DataSourceID="SqlDataSourcet" CellPadding="4" 
        ForeColor="#333333" GridLines="None" EnableViewState="False" 
        AllowPaging="True"><Columns>
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
<asp:BoundField DataField="FaultID" HeaderText="FaultID" 
                SortExpression="FaultID" />
</Columns>

<AlternatingRowStyle BackColor="White" ForeColor="#284775" />

<EditRowStyle BackColor="#999999" />

<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />

<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />

<PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />

<RowStyle BackColor="#F7F6F3" ForeColor="#333333" />

<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
</asp:GridView>


    <asp:SqlDataSource ID="SqlDataSourcet" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
        SelectCommand="SELECT * FROM [FaultTracking]"></asp:SqlDataSource>


    
    <br />
    
    
    
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
        SelectCommand="Select ReportFault.FaultID, FaultTracking.TrackingComment, FaultTracking.AssignedTo, FaultTracking.Status, FaultTracking.LastUpDatedOn, FaultTracking.LastUpdatedBy, FaultTracking.TrackingID From ReportFault INNER JOIN FaultTracking ON ReportFault.FaultID = FaultTracking.FaultID Where (ReportFault.FaultID = @FaultID)"><SelectParameters>
<asp:ControlParameter ControlID="GridView1" Name="FaultID" 
                PropertyName="SelectedValue" />
</SelectParameters>
</asp:SqlDataSource>

    
   

    
   
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" ><ContentTemplate>
            <div>
            <asp:Table ID="Table1" runat="server" CellSpacing="20" GridLines="Both" 
               Width="1038px">
                           <asp:TableRow ID="r1" runat="server">
                   <asp:TableCell ID="c1" runat="server" Width="250px" HorizontalAlign="Left">
                       <asp:Label ID="FaultIDlabel" runat="server" Text="Fault ID"></asp:Label>
                   </asp:TableCell>
                   <asp:TableCell>
                   From database
                   </asp:TableCell>
                   </asp:TableRow>
                   <asp:TableRow ID="TableRow1" runat="server">
                   <asp:TableCell ID="TableCell1" runat="server" Width="250px" HorizontalAlign="Left">
                       <asp:Label ID="StartDate" runat="server" Text="Start Date"></asp:Label>
                   </asp:TableCell>
                   <asp:TableCell>
                   Closed Date
                   </asp:TableCell>
                   </asp:TableRow>
            </asp:Table>
            </div>
            <div>
            <table style="width:300px;">
        <tr>
            <td class="style1">
                Energy:</td>
            <td>
    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style1">
                Cause:</td>
            <td>
    <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style1">
                Remedy:</td>
            <td>
    <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
            </td>
            </tr>
            <tr>
        <td class="style1">
                Comment:</td>
            <td>
              <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
              </td> 
        </tr>
    </table>
            </div>
            
</ContentTemplate>
</asp:UpdatePanel>

    
   

    
   
</ContentTemplate>
        

</asp:TabPanel>
          
          
        <asp:TabPanel ID="Linac2" runat="server" HeaderText="LA2">
        </asp:TabPanel>
          
          
        <asp:TabPanel ID="Linac3" runat="server" HeaderText="LA3">
        </asp:TabPanel>
    
          
          
    </asp:TabContainer>
    <br />
&nbsp;<br />
    <br />  
    
   
    
    
</asp:Content>


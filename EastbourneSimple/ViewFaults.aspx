<%@ Page Title="" Language="VB" MasterPageFile="~/Elf.master" AutoEventWireup="false" CodeFile="ViewFaults.aspx.vb" Inherits="ViewFaults" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--From http://www.dotnetcurry.com/showarticle.aspx?ID=149--%>

    <link href="App_Themes/Blue/calendar.css" rel="stylesheet" type="text/css" />
       

 
 <script type="text/javascript">
     function showDate(sender, args) {

         if (sender._textbox.get_element().value == "") {

             var todayDate = new Date();

             sender._selectedDate = todayDate;

         }

     }
 
 
    function checkDate(sender,args)
{
 if (sender._selectedDate > new Date())
            {
                alert("You cannot select a day later than today!");
                sender._selectedDate = new Date(); 
                // set the date back to the current date
sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }
        }

        function SelectToday(sender, e) {
            var format = sender._todaysDateFormat;
            var date = new Date();
            var formateddate = date.format(format);
            var daycells = $get(sender.get_id() + "_body").getElementsByTagName("DIV");
            for (var i = 0; i < daycells.length; i++) {
                if (daycells[i].title.indexOf(formateddate) > -1) {
                    daycells[i].style.backgroundColor = "Grey";
                    daycells[i].style.color = "White";
                    break;
                }
            }
        }
 
//This function prevents return key usage from http://www.felgall.com/jstip43.htm
        function kH(e) {
           var pK = e ? e.which : window.event.keyCode;
            return pK != 13;
        }
        document.onkeypress = kH;
        if (document.layers) document.captureEvents(Event.KEYPRESS);

    </script>

  

     <div align="center">
         <asp:Button ID="ExitButton" runat="server" Text="Exit" CausesValidation="false" 
        Height="56px" Width="105px" />
        </div>
    <br />
    <div>

    <asp:Label ID="linacLabel" runat="server" Text="Please Select Linac:"></asp:Label> <br />
<asp:DropDownList id="dropLinac" AutoPostBack=true runat="server">
<asp:ListItem>Select</asp:ListItem>
<asp:ListItem>LA1</asp:ListItem>
<asp:ListItem>LA2</asp:ListItem>
<asp:ListItem>LA3</asp:ListItem>  
<asp:ListItem>LA4</asp:ListItem>
</asp:DropDownList>  
    <br />

    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" Visible = "false"><ContentTemplate>
    
    <asp:RadioButtonList ID="RadioButtonFault" runat="server" AutoPostBack="true">
        <asp:ListItem Text="View All Faults" Value="1"></asp:ListItem>
        <asp:ListItem Text="View All Closed Faults" Value="2"></asp:ListItem>
        <asp:ListItem Text="View All Concessions" Value="3"></asp:ListItem>
        <asp:ListItem Text="View Open Concessions" Value="4"></asp:ListItem>
        <asp:ListItem Text="View Faults Opened Between" Value="5"></asp:ListItem>
        <asp:ListItem Text="View Faults Closed Between" Value="6"></asp:ListItem>
        
    </asp:RadioButtonList>
        <br />
        
        <br />
        <asp:Label ID="StartLabel" runat="server" Text="Start Date:"></asp:Label>

    <asp:TextBox ID="StartDate" ReadOnly="true" runat="server" ></asp:TextBox>
<%--        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="StartDate" runat="server" ErrorMessage="Please Enter Start Date" ValidationExpression="^(?:(?:(?:0?[13578]|1[02])(\/|-|\.)31)\1|(?:(?:0?[1,3-9]|1[0-2])(\/|-|\.)(?:29|30)\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:0?2(\/|-|\.)29\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:(?:0?[1-9])|(?:1[0-2]))(\/|-|\.)(?:0?[1-9]|1\d|2[0-8])\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"></asp:RegularExpressionValidator>
--%>        
<asp:RequiredFieldValidator ID="RequiredFieldValidatorstart" runat="server" ErrorMessage="Please Enter Start Date" Display="dynamic" ControlToValidate="StartDate" Enabled="False" ForeColor="#FF3300"></asp:RequiredFieldValidator>
    
<asp:CalendarExtender ID="StartDate_CalendarExtender" runat="server" 
        Enabled="True" Format="dd/MM/yyyy" TargetControlID="StartDate" 
         CssClass="cal_Theme1">
    </asp:CalendarExtender>
        <asp:Label ID="EndLabel" runat="server" Text="End Date"></asp:Label>

    <asp:TextBox ID="EndDate" ReadOnly="true" runat="server" ></asp:TextBox>
<%--    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="EndDate" runat="server" ErrorMessage="Please Enter End Date" ValidationExpression="^(?:(?:(?:0?[13578]|1[02])(\/|-|\.)31)\1|(?:(?:0?[1,3-9]|1[0-2])(\/|-|\.)(?:29|30)\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:0?2(\/|-|\.)29\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:(?:0?[1-9])|(?:1[0-2]))(\/|-|\.)(?:0?[1-9]|1\d|2[0-8])\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"></asp:RegularExpressionValidator>
--%>
    <asp:RequiredFieldValidator ID="RequiredFieldValidatorstop" runat="server" 
            ErrorMessage="Please Enter End Date" Display="Dynamic" 
            ControlToValidate="EndDate" Enabled="false" ForeColor="#FF3300"></asp:RequiredFieldValidator>
   
<asp:CalendarExtender ID="EndDate_CalendarExtender" runat="server" 
        Enabled="True" TargetControlID="EndDate" Format="dd/MM/yyyy" 
         CssClass="cal_Theme1"  >
    </asp:CalendarExtender>
    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="StartDate"
        ControlToValidate="EndDate" ErrorMessage="Start Date must be before End Date"
        Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>     
        <br />
        
    <br />
    <br />
    
    <asp:Button ID="ViewFaultsButton" runat="server" Text="View" 
        CausesValidation="true" BackColor="#33CC33" Height="56px" Width="105px" Enabled ="false"/>
        
   </ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger  ControlID="RadioButtonFault" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

    
   
   


    <asp:HiddenField ID="HiddenField1" runat="server"/>
    
   <asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>
       
    <asp:Table ID="Table1" runat="server" HorizontalAlign="Left">
       <asp:TableRow HorizontalAlign="Left" VerticalAlign="Top">
       <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">   
       <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" 
                        DataKeyNames="incidentID"  
                        style="top: 670px; left: 10px; height: 162px; width: 617px;" 
                        ForeColor="#333333" GridLines="None" AllowPaging="True"  >
                        

<RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" />

                                                
<Columns>
                            
<asp:BoundField DataField="incidentID" HeaderText="incidentID" InsertVisible="False" 
                                ReadOnly="True" SortExpression="incidentID" />
                                
<asp:BoundField DataField="DateInserted" HeaderText="Date Reported" 
                                SortExpression="DateReported" />
                                
<asp:BoundField DataField="DateClosed" HeaderText="Date Closed" 
                                SortExpression="DateClosed" />
                                
<asp:BoundField DataField="Status" HeaderText="Status" 
                                SortExpression="Status" />
                                
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
                            

                                
<asp:ButtonField ButtonType="Button" CommandName="Select" Text="View Details" causesvalidation="false"/>

                        
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
        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
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
       
</asp:TableCell></asp:TableRow>
    </asp:Table>

</ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


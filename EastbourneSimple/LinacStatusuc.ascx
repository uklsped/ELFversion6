<%@ Control Language="VB" AutoEventWireup="false" CodeFile="LinacStatusuc.ascx.vb" Inherits="LinacStatusuc" %>

 <%@ Register src="WriteDatauc.ascx" tagname="WriteDatauc" tagprefix="uc1" %>

 <%@ Register src="ViewFaultsuc.ascx" tagname="viewfaultsuc" tagprefix="uc2" %>

 <%@ Register src="RegisterUseruc.ascx" tagname="RegisterUseruc" tagprefix="uc3" %>

<%@ Register src="Administrationuc.ascx" tagname="administrationuc" tagprefix="uc4" %>

<%@ Register src="controls/ModalityDisplayuc.ascx" tagname="ModalityDisplayuc" tagprefix="uc5" %>

<%-- <link href="App_Themes/Blue/Elf.css" rel="stylesheet" type="text/css" />--%>
<%--<link rel="stylesheet" type="text/css" href="css\twocol.css" />--%>
 <%--<asp:TextBox ID="TextBox1" runat="server" Width="168px"></asp:TextBox>--%>
<%--    <br />
    <br />
    <br />--%>
<asp:HiddenField ID="HiddenField1" runat="server" Visible="False" Value="False" />
<asp:HiddenField ID="HiddenField2" runat="server" Visible="False" Value="False" />
    
            <asp:PlaceHolder ID="PlaceHolder1" runat="server">
           <uc1:WriteDatauc ID="WriteDatauc1" LinacName = "" UserReason = "100" Tabby="0" WriteName="Status" Visible ="false" runat="server" />
            
         </asp:PlaceHolder>
&nbsp;<br />
        <asp:GridView ID="DummyGridView" runat="server">
    </asp:GridView>
    
       
   <br />
<asp:Panel ID="ModalityDisplayPanel" runat="server" Visible="false">
                       <asp:PlaceHolder ID="ModalityPlaceholder" runat="server">
                       </asp:PlaceHolder>
                   </asp:Panel>
   <div>
             <asp:Button id="Button1" runat="server" Text="View Fault History" causesvalidation="false" OnClick="Button1_Click" /> 
             &nbsp;
    <asp:Button id="Button2" runat="server" Text="Register" causesvalidation="false" OnClick="Button2_Click" /> &nbsp; 
    <asp:Button id="Button3" runat="server" Text="Administration" causesvalidation="false" OnClick="Button3_Click" />  &nbsp;
    <asp:Button ID="Reset" runat="server" Text="Reset ELF" causesvalidation="false"/>
            
       </div>    
            <%--<asp:DropDownList ID="DropDownList1" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
                runat="server" AutoPostBack="True">
                <asp:ListItem Value="0">View 0</asp:ListItem>
                <asp:ListItem Value="1">View 1</asp:ListItem>
                <asp:ListItem Value="2">View 2</asp:ListItem>
                <asp:ListItem Value="3">View 3</asp:ListItem>
            </asp:DropDownList><br />--%>
            <%--<hr />--%>
            <div id="container">
            
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="View0" runat="server">
            </asp:View>
                <asp:View ID="View1" runat="server">
                                
                    <uc2:ViewFaultsuc ID="ViewFaultsuc1" LinacName='' runat="server" />
                    </asp:View>
                   <asp:View ID="View2" runat="server">
                  <asp:PlaceHolder ID="PlaceHolder2" runat="server">
                     <uc3:RegisterUseruc ID="RegisterUseruc1" updatemode="conditional" runat="server" />
                    </asp:PlaceHolder>
                    
                     </asp:View>
                <asp:View ID="View3" runat="server" >
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                    <ContentTemplate>
                    <%--<asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>--%>
                   <uc4:Administrationuc ID="Administrationuc1" linac='' runat="server" />
                   </ContentTemplate>
                   </asp:UpdatePanel>
                </asp:View>
            </asp:MultiView>
            
            </div>
    <br />
   
   <%--<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
        SelectCommand="SELECT l.stateid, l.state, l.DateTime, u.username, d.reasondescription  FROM [LinacStatus]  l 
left outer join reasondescription d on d.reasonnumber=l.userreason
left outer join userinformation u on u.usergroup = l.usergroup
where l.stateID = (Select max(stateID) as lastrecord from [LinacStatus])"></asp:SqlDataSource>--%>
<%-- <asp:GridView ID="GridView2" runat="server">
        </asp:GridView>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="stateid" CellPadding="4" 
        ForeColor="#333333" GridLines="None">
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <Columns>
            <asp:BoundField DataField="stateid" HeaderText="stateid" InsertVisible="False" 
                ReadOnly="True" SortExpression="stateid" />
            <asp:BoundField DataField="state" HeaderText="state" SortExpression="state" />
            <asp:BoundField DataField="DateTime" HeaderText="DateTime" 
                SortExpression="DateTime" />
            <asp:BoundField DataField="username" HeaderText="username" 
                SortExpression="username" />
            <asp:BoundField DataField="reasondescription" HeaderText="reasondescription" 
                SortExpression="reasondescription" />
        </Columns>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>--%>





    

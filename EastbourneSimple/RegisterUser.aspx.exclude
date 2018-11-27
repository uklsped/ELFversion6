<%@ Page Title="" Language="VB" MasterPageFile="~/Elf.master" AutoEventWireup="false" CodeFile="RegisterUser.aspx.vb" Inherits="RegisterUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript"> 
//This function prevents return key usage from http://www.felgall.com/jstip43.htm
        function kH(e) {
           var pK = e ? e.which : window.event.keyCode;
            return pK != 13;
        }
        document.onkeypress = kH;
        if (document.layers) document.captureEvents(Event.KEYPRESS);
</script>
    <div>
<fieldset style="width:300px;">
            <legend>Register</legend>
            
<asp:CreateUserWizard ID="CreateUserWizard1" runat="server" disablecreateduser="true"
    DisplayCancelButton="false" FinishDestinationPageUrl="~/RegisterUser.aspx" CancelDestinationPageUrl="~/RegisterUser.aspx" 
     
     LoginCreatedUser="False" >
        <WizardSteps>
            <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                <ContentTemplate>
                    <table border="0">
                        <tr>
                            <td align="center" colspan="2">
                                </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="FirstNameLabel" runat="server" AssociatedControlID="FirstName">First Name:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="FirstName" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="FirstNameRequired" runat="server" 
                                    ControlToValidate="FirstName" ErrorMessage="First Name is required." 
                                    ToolTip="First Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="LastNameLabel" runat="server" AssociatedControlID="LastName">Last Name:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="LastName" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="LastNameRequired" runat="server" 
                                    ControlToValidate="LastName" ErrorMessage="Last Name is required." 
                                    ToolTip="Last Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
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
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:SecurityString %>" 
                                    SelectCommand="SELECT [RoleName] FROM [vw_aspnet_Roles]"></asp:SqlDataSource>
                                <asp:DropDownList ID="UserRole"  runat="server"  Width="128px" 
                                    DataSourceID="SqlDataSource1" DataTextField="RoleName" 
                                    DataValueField="RoleName">
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
                    <asp:SqlDataSource ID="InsertExtraInfo" runat="server" ConnectionString="<%$ ConnectionStrings:SecurityString %>"
                    InsertCommand="Insert into [FirstLastName] (UserID, FirstName, LastName) Values (@UserID, @FirstName, @LastName)"
                    ProviderName="<%$ ConnectionStrings:SecurityString.ProviderName %>">
                    <InsertParameters>
                    <asp:ControlParameter Name="FirstName" Type="String" ControlID="FirstName" PropertyName="Text" />
                    <asp:ControlParameter Name="LastName" Type="String" ControlID="LastName" PropertyName="Text" />
                    </InsertParameters>
                    </asp:SqlDataSource>
                    <table border="0" style="font-size: 100%; font-family: Verdana" id="TABLE1" >
            
            <tr>
                <td>
            </tr>
            <tr>

            
                <td align="right" colspan="2">
                    &nbsp;<asp:Button ID="CancelButton" runat="server" 
                        CausesValidation="False"
                        Text="Cancel"  onclick="cancel_click" />
               </td>
            </tr>
        </table>
                    
                    
                </ContentTemplate>
            </asp:CreateUserWizardStep>
            <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
             <ContentTemplate>
        <table border="0" style="font-size: 100%; font-family: Verdana" id="TABLE1" >
            <%--<tr>
                <td align="center" colspan="2" style="font-weight: bold; color: white; background-color: #5d7b9d; height: 18px;">
                    Complete</td>
            </tr>--%>
           <%-- <tr>
                <td>
            </tr>--%>
            <tr>
                <td align="right" colspan="2">
                    &nbsp;<asp:Button ID="ContinueButton" runat="server"  Text="Close" onclick="continue_click"/>
<%--                    <asp:Button ID="ContinueButton1" runat="server" BackColor="#FFFBFF" BorderColor="#CCCCCC"
                        BorderStyle="Solid" BorderWidth="1px" CausesValidation="False"
                        Font-Names="Verdana" ForeColor="#284775" Text="Close"  onclick="continue_click" />--%>
                </td>
            </tr>
        </table>
    </ContentTemplate>
            </asp:CompleteWizardStep>
        </WizardSteps>
    </asp:CreateUserWizard>
    </fieldset>
        
    </div>
    
    <div>
    <fieldset style="width:350px;">
    <legend>Recover Password</legend>
    <asp:PasswordRecovery ID="PasswordRecovery1" runat="server">
</asp:PasswordRecovery>
</fieldset>
    </div>
    <div>
    <fieldset style="width:350px;">
    <legend>Change Password</legend>
    
    <asp:ChangePassword ID="ChangePwd" runat="server" 
        ContinueDestinationPageUrl="~/RegisterUser.aspx" DisplayUserName="true" 
            CancelDestinationPageUrl="RegisterUser.aspx">
    </asp:ChangePassword>
    
    </fieldset>
    </div>
    <asp:HiddenField ID="HiddenField1" runat="server" />
   
<br />
<br />
        
</asp:Content>


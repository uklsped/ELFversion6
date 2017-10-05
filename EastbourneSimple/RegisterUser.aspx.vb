﻿Imports System.Net.Mail
Partial Class RegisterUser
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim myControl1 As Control = FindControl("TextBox2")
        'If (Not myControl1 Is Nothing) Then
        '    ' Get control's parent. 
        '    Dim myControl2 As Control = myControl1.Parent
        '    Response.Write("Parent of the text box is : " & myControl2.ID)
        'Else
        '    Response.Write("Control not found.....")
        'End If



        'Dim UserNameTextBox As TextBox
        'UserNameTextBox = ChangePwd.ChangePasswordTemplateContainer.FindControl("UserName")
        'UserNameTextBox.Text = Nothing
        'If Not IsPostBack Then
        'Dim machinename As String
        'from http://blog.davidsz.nl/2012/03/04/previous-page-in-asp-net/
        'Cancel.Attributes.Add("onClick", "javascript:history.back();   return false;")
        'leave  cancel_click empty if you use this
        'If Not IsPostBack Then
        'This next is if register opens as new page, but at the moment using
        'http://stackoverflow.com/questions/17582081/how-to-open-aspx-web-pages-on-a-pop-up-window


        'ViewState("RefUrl") = Request.UrlReferrer.ToString()
        'Dim viewrefurl As String = ViewState("RefUrl")
        'machinename = Request.QueryString("val")
        'HiddenField1.Value = Request.QueryString("Tabindex")

        'End If
        'End If




    End Sub
    'Private Sub BindUserGrid()
    'Dim allUsers As MembershipUserCollection = Membership.GetAllUsers()
    'UserGrid.DataSource = allUsers
    'UserGrid.DataBind()
    ' End Sub
    Protected Sub CreateUserWizard1_CreatedUser(ByVal sender As Object, ByVal e As System.EventArgs) Handles CreateUserWizard1.CreatedUser
        'Protected Sub CreateUser_click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim selectedrole As DropDownList = CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserRole")
        Roles.AddUserToRole(CreateUserWizard1.UserName, selectedrole.SelectedItem.Value)
        'this is from http://www.4guysfromrolla.com/articles/070506-1.aspx
        Dim UserNameTextBox As TextBox
        UserNameTextBox = CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName")
        Dim DataSource As SqlDataSource
        DataSource = CreateUserWizardStep1.ContentTemplateContainer.FindControl("InsertExtraInfo")
        Dim User As MembershipUser
        User = Membership.GetUser(UserNameTextBox.Text)
        Dim UserGuid As Object
        UserGuid = User.ProviderUserKey()
        Dim guid As String = UserGuid.ToString()

        DataSource.InsertParameters.Add("UserID", UserGuid.ToString())
        DataSource.Insert()

        Dim UserEmailtextbox As TextBox = CreateUserWizardStep1.ContentTemplateContainer.FindControl("Email")
        Dim UserEmail As String
        UserEmail = UserEmailtextbox.Text.ToString
        Dim smtpClient As SmtpClient = New SmtpClient()
        Dim message As MailMessage = New MailMessage()
        Try
            Dim fromAddress As New MailAddress("VISIRSERVER@VISIRSERVER.bsuh.nhs.uk", "ELF")
            Dim toAddress As New MailAddress("david.spendley@bsuh.nhs.uk")
            message.From = fromAddress
            message.To.Add(toAddress)
            message.Subject = "ELF registration"
            message.Body = "Someone has registered for ELF"
            smtpClient.Host = "10.216.8.19"
            smtpClient.Send(message)

            Dim regAddress As New MailAddress(UserEmail)
            message.From = fromAddress
            message.To.Add(regAddress)
            message.Subject = "ELF registration"
            message.Body = "Thank you for registering with ELF. You will be notified when your account has been approved"
            smtpClient.Host = "10.216.8.19"
            smtpClient.Send(message)

        Catch ex As Exception
            'should be an error action here
        End Try
    End Sub

    'Protected Sub ExitButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExitButton.Click
    '    'This if opening as full page not as popup page

    '    'Dim tab As String
    '    'tab = HiddenField1.Value
    '    'Response.Redirect("LA1page.aspx?pageref=RegisterUser&Tabindex=" & tab)

    '    ClientScript.RegisterClientScriptBlock(Page.GetType(), "script", "window.close();", True)
    'End Sub

    Protected Sub continue_click(ByVal sender As Object, ByVal e As System.EventArgs)

        ClientScript.RegisterClientScriptBlock(Page.GetType(), "script", "window.close();", True)
    End Sub
    Protected Sub cancel_click(ByVal sender As Object, ByVal e As System.EventArgs)

        ClientScript.RegisterClientScriptBlock(Page.GetType(), "script", "window.close();", True)
    End Sub
End Class

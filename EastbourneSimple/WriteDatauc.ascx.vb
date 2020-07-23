Imports AjaxControlToolkit
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.Page
Imports System.Text
Imports System.IO
Partial Class WriteDatauc
    Inherits System.Web.UI.UserControl
    Private Action As String
    Private actionstate As String
    Private appstate As String
    Dim modalpopupextendercom As New ModalPopupExtender
    Public Event UserApproved(ByVal Tab As String, ByVal UserName As String)
    Public Property Tabby() As String
    Public Property UserReason() As Integer

    Public Property LinacName() As String

    Public ReadOnly Property username() As String
        Get
            username = txtchkUserName.Text.Trim()
        End Get

    End Property
    Public ReadOnly Property userpassword() As String
        Get
            userpassword = txtchkPWD.Text.Trim()
        End Get
    End Property

    Protected Sub Page_load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        WaitButtons("Acknowledge")
        appstate = "LogOn" + LinacName
        actionstate = "ActionState" + LinacName
        Dim logerrorbox As Label = FindControl("LoginErrordetails")
        logerrorbox.Text = Nothing
        'Reference to defect removed 23/11/16 Added back in 26/03/18
        'If Application(appstate) = 1 Or tablabel = "3" Or tablabel = "Report" Or tablabel = "handover" Or tablabel = "EndDay" Or tablabel = "Admin" Or tablabel = "Updatefault" Or tablabel = "incident" Or tablabel = "0" Or tablabel = "Defect" Or tablabel = "recover" Or tablabel = "Image" Or tablabel = "Major" Then
        If Application(appstate) = 1 Or Tabby = "3" Or Tabby = "Report" Or Tabby = "EndDay" Or Tabby = "Admin" Or Tabby = "Updatefault" Or Tabby = "incident" Or Tabby = "0" Or Tabby = "Defect" Or Tabby = "recover" Or Tabby = "Image" Or Tabby = "Major" Then
            'If Application(appstate) = 1 Or tablabel = "3" Or tablabel = "Report" Or tablabel = "handover" Or tablabel = "EndDay" Or tablabel = "Admin" Or tablabel = "Updatefault" Or tablabel = "incident" Or tablabel = "0" Or tablabel = "recover" Or tablabel = "Image" Then

            Dim MyString As String
            Dim Tabnumber As String
            MyString = "ModalPopupextendercom"
            Tabnumber = Tabby
            MyString = MyString & Tabnumber
            modalpopupextendercom.ID = MyString
            modalpopupextendercom.BehaviorID = MyString
            modalpopupextendercom.TargetControlID = "label1"
            modalpopupextendercom.PopupControlID = "Panel1"
            modalpopupextendercom.BackgroundCssClass = "modalBackground"
            PlaceHolder1.Controls.Add(modalpopupextendercom)
            modalpopupextendercom.Show()

            'If (Not Page.ClientScript.IsStartupScriptRegistered("Startup")) Then
            '    Dim sb As StringBuilder = New StringBuilder()
            '    sb.Append("<script type=""text/javascript"">")
            '    sb.Append("Sys.Application.add_load(modalSetup);")
            '    sb.Append("function modalSetup() {")
            '    sb.Append(String.Format("var modalPopup = $find('{0}');", modalpopupextendercom.BehaviorID))
            '    sb.Append("modalPopup.add_shown(SetFocusOnControl); }")
            '    sb.Append("function SetFocusOnControl() {")
            '    sb.Append(String.Format("var textBox1 = $get('{0}');", txtchkUserName.ClientID))
            '    sb.Append("textBox1.focus();}")
            '    sb.Append("</script>")
            '    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Startup", sb.ToString())
            'End If
            'WaitButtons("Acknowledge")
        End If

    End Sub
    Protected Sub btnchkcancel_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnchkcancel.Click
        resetLogInscreen()

        Dim wctrl As WriteDatauc = CType(Me.Parent.FindControl("Writedatauc1"), WriteDatauc)
        If wctrl IsNot Nothing Then
            wctrl.Visible = False
            'added to deal with clearing defect form if wrong user type 19/03/18
            If (Tabby = "Admin") Or (Tabby = "Defect") Then
                Application(actionstate) = "Cancel"
                RaiseEvent UserApproved(Tabby, "dummy")
            End If
        End If
        Dim wctrl1 As WriteDatauc = CType(Me.Parent.FindControl("Writedatauc2"), WriteDatauc)
        If wctrl1 IsNot Nothing Then
            wctrl1.Visible = False
            If Tabby = "Major" Then
                Application(actionstate) = "Cancel"
                RaiseEvent UserApproved(Tabby, "dummy")
            End If
        End If
        Dim wctrl2 As WriteDatauc = CType(Me.Parent.FindControl("Writedatauc3"), WriteDatauc)
        If wctrl2 IsNot Nothing Then
            wctrl2.Visible = False
            If Tabby = "incident" Then
                Application(actionstate) = "Cancel"
                RaiseEvent UserApproved(Tabby, "dummy")
            End If
        End If
    End Sub
    Protected Sub resetLogInscreen()
        Dim textboxUser As TextBox = FindControl("txtchkUserName")
        Dim passwordUser As TextBox = FindControl("txtchkPWD")
        Dim logerrorbox As Label = FindControl("LoginErrordetails")
        textboxUser.Text = Nothing
        passwordUser.Text = Nothing
        logerrorbox.Text = Nothing
        modalpopupextendercom.Hide()
    End Sub
    Protected Sub AcceptOK_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AcceptOK.Click
        Dim logerrorbox As Label = FindControl("LoginErrordetails")
        Dim strScript As String = "<script>"
        Dim textboxUser As TextBox = FindControl("txtchkUserName")
        Dim passwordUser As TextBox = FindControl("txtchkPWD")
        Dim Activity As String = ""
        Dim modalpop As ModalPopupExtender
        Dim modalname As String = "Modalpopupextendercom" & Tabby
        modalpop = CType(FindControl(modalname), ModalPopupExtender)
        'We need to determine if the user is authenticated
        'Get the values entered by the user
        Dim loginUsername As String = username
        Dim loginPassword As String = userpassword
        'Dim Pager As Page = SPage

        Dim modalidentifier As String
        'If DavesCode.Reuse.SuccessfulLogin(loginUsername, loginPassword, UserReason, textboxUser, passwordUser, logerrorbox, modalpop) <> 0 Then
        If DavesCode.Reuse.SuccessfulLogin(loginUsername, loginPassword, UserReason, textboxUser, passwordUser, logerrorbox) <> 0 Then
            If modalpop IsNot Nothing Then
                'modalpop.Dispose()
                resetLogInscreen()
                'eg from http://dotnetbites.wordpress.com/2014/02/15/call-parent-page-method-from-user-control-using-reflection/

                RaiseEvent UserApproved(Tabby, loginUsername)
                'DavesCode.Reuse.writeLogFile(Reason, loginUsername, False)
                'activity = DavesCode.Reuse.ReturnActivity(Reason)
                'Me.Page.GetType.InvokeMember("UpdateDisplay", System.Reflection.BindingFlags.InvokeMethod, Nothing, Me.Page, New Object() {activity})

            End If
        Else
            'If it gets to here something has gone wrong with SuccessfulLogin()
            'Dim NoLoggin As String = "Failed"
            'RaiseEvent FaultFailed(NoLoggin)
            modalidentifier = modalpopupextendercom.ID
            modalpop.Show()
            'Dim mpContentPlaceHolder As ContentPlaceHolder
            'Dim wctrl As WriteDatauc

            'mpContentPlaceHolder = _
            'CType(Parent.FindControl("ContentPlaceHolder1"),  _
            'ContentPlaceHolder)
            'If Not mpContentPlaceHolder Is Nothing Then
            '    wctrl = CType(mpContentPlaceHolder.FindControl("Writedataucfr"), WriteDatauc)
            '    wctrl.Visible = True
            'End If


            'modalpopupextendercom.Show()
        End If

    End Sub
    Private Sub WaitButtons(ByVal WaitType As String)

        Select Case WaitType
            Case "Acknowledge"
                Dim Accept As Button = FindControl("AcceptOK")
                Dim Cancel As Button = FindControl("btnchkcancel")
                If Not FindControl("AcceptOK") Is Nothing Then
                    Accept.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(Accept, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not FindControl("btnchkcancel") Is Nothing Then
                    Cancel.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(Cancel, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If

            Case "Tech"
                Dim Eng As Button = FindControl("engHandoverButton")
                Dim LogOff As Button = FindControl("LogOffButton")
                Dim Lock As Button = FindControl("LockElf")
                Dim Physics As Button = FindControl("PhysicsQA")
                Dim Atlas As Button = FindControl("ViewAtlasButton")
                Dim FaultPanel As Button = FindControl("FaultPanelButton")
                If Not Eng Is Nothing Then
                    Eng.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(Eng, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not LogOff Is Nothing Then
                    LogOff.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(LogOff, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not Lock Is Nothing Then
                    Lock.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(Lock, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not Physics Is Nothing Then
                    Physics.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(Physics, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not Atlas Is Nothing Then
                    Atlas.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(Atlas, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not FaultPanel Is Nothing Then
                    FaultPanel.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(FaultPanel, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If

            Case "Rad"
                Dim clin As Button = FindControl("clinHandoverButton")
                Dim LogOff As Button = FindControl("LogOff")
                Dim TStart As Button = FindControl("TStart")
                If Not clin Is Nothing Then
                    clin.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(clin, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not LogOff Is Nothing Then
                    LogOff.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(LogOff, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not TStart Is Nothing Then
                    TStart.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(TStart, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If

        End Select

    End Sub

End Class

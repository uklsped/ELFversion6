Imports System.Transactions

Partial Public Class AcceptLinacuc
    Inherits System.Web.UI.UserControl
    Public Event AcceptHandler As EventHandler
    Private appstate As String
    Private suspstate As String
    'Dim modalpopupextendergen As New ModalPopupExtender
    Public Event ClinicalApproved(ByVal connectionString As String)
    Public Event AcknowledgeEnergies()
    Public Event UpdateReturnButtons()
    Public Event ShowName(ByVal LastUserGroup As Integer)
    'Public Event EngRunuploaded(ByVal connectionString As String)
    'Public Event PreRunuploaded(ByVal connectionString As String)
    'Public Event SetModalities(ByVal connectionString As String)
    Public Event Repairloaded(ByVal connectionString As String)

    Public Property Tabby() As String
    Public Property UserReason() As Integer
    Public Property LinacName() As String

    Public ReadOnly Property Username() As String
        Get
            Username = txtchkUserName.Text.Trim()
        End Get

    End Property
    Public ReadOnly Property Userpassword() As String
        Get
            Userpassword = txtchkPWD.Text.Trim()
        End Get
    End Property

    Public Sub AcceptOK_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AcceptOK.Click
        Dim output As String
        Dim strScript As String = "<script>"
        Dim textboxUser As TextBox = FindControl("txtchkUserName") 'This gets username textbox to pass to login
        Dim passwordUser As TextBox = FindControl("txtchkPWD")  'This gets password textbox to pass to login
        Dim logerrorbox As Label = FindControl("LoginErrordetails") 'This gets error label to pass to login

        Dim usergroupname As String = String.Empty

        'We need to determine if the user is authenticated
        'Get the values entered by the user
        Dim loginUsername As String = Username
        Dim loginPassword As String = Userpassword
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        Dim Activity As String

        appstate = "LogOn" & LinacName
        suspstate = "Suspended" & LinacName
        Dim reload As String
        Dim clinstate As String = "Clinical"

        reload = DavesCode.Reuse.GetLastState(LinacName, 0)
        'from http://spacetech.dk/vb-net-string-compare-not-equal.html
        If Not (reload.Equals(clinstate)) Then
            Dim myAppState As Integer = CInt(Application(appstate))
            'myAppState = 1
            'DavesCode.Reuse.RecordStates(LinacName, Tabby, "AcceptOK", 0)
            If myAppState <> 1 Then
                Activity = DavesCode.Reuse.ReturnActivity(UserReason)

                'This can only return here if there is a valid log in
                'Dim usergroupselected As Integer = DavesCode.Reuse.SuccessfulLogin(loginUsername, loginPassword, Reason, textboxUser, passwordUser, logerrorbox, modalpop)
                Dim usergroupselected As Integer = DavesCode.Reuse.SuccessfulLogin(loginUsername, loginPassword, UserReason, textboxUser, passwordUser, logerrorbox)

                If usergroupselected <> Nothing Then

                    Try
                        Using myscope As TransactionScope = New TransactionScope()

                            DavesCode.Reuse.MachineStateNew(loginUsername, usergroupselected, LinacName, UserReason, False, connectionString)
                            Select Case Tabby
                                Case 1, 7
                                    'RaiseEvent EngRunuploaded(connectionString)
                                    'RaiseEvent SetModalities(connectionString)

                                Case 2
                                    'RaiseEvent PreRunuploaded(connectionString)
                                Case 3
                                    'Me.Page.GetType.InvokeMember("LaunchTab", System.Reflection.BindingFlags.InvokeMethod, Nothing, Me.Page, New Object() {})
                                    output = "Clinical"
                                    'Me.Page.GetType.InvokeMember("Updatestatedisplay", System.Reflection.BindingFlags.InvokeMethod, Nothing, Me.Page, New Object() {output})

                                    'RaiseEvent ClinicalApproved(connectionString)
                                    'RaiseEvent SetModalities(connectionString)
                                Case 4, 8
                                    RaiseEvent UpdateReturnButtons()
                                    'RaiseEvent ShowName(usergroupselected)
                                Case 5
                                    'RaiseEvent Repairloaded(connectionString)
                                    'RaiseEvent Repairloaded(connectionString)
                            End Select
                            output = connectionString
                            myscope.Complete()
                        End Using

                        textboxUser.Text = String.Empty

                        'eg from http://dotnetbites.wordpress.com/2014/02/15/call-parent-page-method-from-user-control-using-reflection/
                        ' this is an instrumentation field that displays application number ie 0 or 1
                        'Me.Page.GetType.InvokeMember("UpdateHiddenLAField", System.Reflection.BindingFlags.InvokeMethod, Nothing, Me.Page, New Object() {output})
                        Me.Page.GetType.InvokeMember("UpdateDisplay", System.Reflection.BindingFlags.InvokeMethod, Nothing, Me.Page, New Object() {Activity})
                        'this is instrumentation code that displays current username
                        'Me.Page.GetType.InvokeMember("Updateuserlabel", System.Reflection.BindingFlags.InvokeMethod, Nothing, Me.Page, New Object() {loginUsername})

                        RaiseEvent ShowName(usergroupselected)

                        Dim returnstring As String = LinacName & "page.aspx?tabclicked=" & Tabby
                        Response.Redirect(returnstring, False)

                    Catch ex As Exception
                        DavesCode.NewFaultHandling.LogError(ex)
                        RaiseLoadError()
                    End Try
                Else

                    textboxUser.Text = String.Empty


                End If
            Else
                textboxUser.Text = String.Empty
                Application(appstate) = 0

            End If
        Else
            Try
                If Tabby = 3 Then
                    Using myscope As TransactionScope = New TransactionScope()
                        RaiseEvent ClinicalApproved(connectionString)
                        output = "Clinical"
                        Me.Page.GetType.InvokeMember("Updatestatedisplay", System.Reflection.BindingFlags.InvokeMethod, Nothing, Me.Page, New Object() {output})

                        myscope.Complete()
                    End Using
                End If
            Catch ex As Exception
                DavesCode.NewFaultHandling.LogError(ex)
                RaiseLoadError()
            End Try
        End If
    End Sub
    Protected Sub RaiseLoadError()
        Dim machinelabel As String = LinacName & "Page.aspx';"
        Dim strScript As String = "<script>"
        strScript += "alert('Problem Logging On. Please call Administrator');"
        strScript += "window.location='"
        strScript += machinelabel
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(AcceptOK, Me.GetType(), "JSCR", strScript.ToString(), False)
    End Sub


    Protected Sub Page_init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init

        appstate = "LogOn" & LinacName
        suspstate = "Suspended" & LinacName

    End Sub

    Protected Sub Page_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' DavesCode.Reuse.RecordStates("T1", "1", "AcceptucPageLoad", 0)

        ''from http://spacetech.dk/vb-net-string-compare-not-equal.html
        'If Not (reload.Equals(clinstate)) Then

        WaitButtons("Acknowledge")
        If Application(appstate) <> 1 Then

            AcceptTablabel.text = "Log on to " & DavesCode.Reuse.ReturnActivity(UserReason)

            If Tabby = 3 Then
                If Not LinacName Like "T#" Then
                    Dim objCon As UserControl = Page.LoadControl("EnergyDisplayuc.ascx")
                    CType(objCon, EnergyDisplayuc).LinacName = LinacName
                    PlaceHolder2.Controls.Add(objCon)
                    'PlaceHolder2.Visible = True
                    AcceptLinacDisplay.Width = 1000
                    AcceptLinacDisplay.Height = 200
                    AcceptOK.Text = "Acknowledge Energies and Accept Linac"
                End If
            End If

        End If


    End Sub

    Public Sub Btnchkcancel_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnchkcancel.Click
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        AcceptOK.Visible = False
        appstate = "LogOn" & LinacName
        suspstate = "Suspended" & LinacName
        Dim reload As String
        Dim clinstate As String = "Clinical"
        reload = DavesCode.Reuse.GetLastState(LinacName, 0)
        'from http://spacetech.dk/vb-net-string-compare-not-equal.html
        If Not (reload.Equals(clinstate)) Then
            Dim myAppState As Integer = CInt(Application(appstate))

            If myAppState <> 1 Then
                Dim loginUsername As String = Username
                Dim returnstring As String
                Application(appstate) = 0

                If Tabby = 3 Then
                    Application(suspstate) = 1
                End If
                returnstring = LinacName & "page.aspx"
                Response.Redirect(returnstring)
            Else
                'Write and error?
            End If
        Else
            RaiseEvent ClinicalApproved(connectionString)
        End If
    End Sub
    Private Sub WaitButtons(ByVal WaitType As String)

        Select Case WaitType
            Case "Acknowledge"
                Dim Accept As Button = FindControl("AcceptOK")
                Dim Cancel As Button = FindControl("btnchkcancel")
                If Not FindControl("AcceptOK") Is Nothing Then
                    Accept.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(Accept, "") & ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not FindControl("btnchkcancel") Is Nothing Then
                    Cancel.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(Cancel, "") & ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If

            Case "Tech"
                Dim Eng As Button = FindControl("engHandoverButton")
                Dim LogOff As Button = FindControl("LogOffButton")
                Dim Lock As Button = FindControl("LockElf")
                Dim Physics As Button = FindControl("PhysicsQA")
                Dim Atlas As Button = FindControl("ViewAtlasButton")
                Dim FaultPanel As Button = FindControl("FaultPanelButton")
                If Not Eng Is Nothing Then
                    Eng.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(Eng, "") & ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not LogOff Is Nothing Then
                    LogOff.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(LogOff, "") & ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not Lock Is Nothing Then
                    Lock.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(Lock, "") & ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not Physics Is Nothing Then
                    Physics.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(Physics, "") & ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not Atlas Is Nothing Then
                    Atlas.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(Atlas, "") & ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not FaultPanel Is Nothing Then
                    FaultPanel.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(FaultPanel, "") & ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If

            Case "Rad"
                Dim clin As Button = FindControl("clinHandoverButton")
                Dim LogOff As Button = FindControl("LogOff")
                Dim TStart As Button = FindControl("TStart")
                If Not clin Is Nothing Then
                    clin.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(clin, "") & ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not LogOff Is Nothing Then
                    LogOff.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(LogOff, "") & ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not TStart Is Nothing Then
                    TStart.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(TStart, "") & ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If

        End Select

    End Sub

End Class

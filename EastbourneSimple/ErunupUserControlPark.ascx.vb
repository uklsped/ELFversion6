Imports System.Data.SqlClient
Imports System.Data
Imports AjaxControlToolkit
Imports System.Web.UI.Page
Imports DavesCode

Partial Class ErunupUserControlPark
    Inherits System.Web.UI.UserControl

    Private mpContentPlaceHolder As ContentPlaceHolder
    Private appstate As String
    Private suspstate As String
    Private actionstate As String
    Private failstate As String
    Private repairstate As String
    'Private faultviewstate As String
    'Private atlasviewstate As String
    'Private qaviewstate As String
    Private LinacFlag As String
    Private objconToday As TodayClosedFault
    Private Todaydefect As DefectSavePark
    Private objconViewFaults As ViewOpenFaults
    Private BoxChanged As String
    Public Event BlankGroup(ByVal BlankUser As Integer)
    Private tabstate As String

    Public ReadOnly Property CurrentComment() As String
        Get
            Return CommentBox.Text
        End Get
    End Property

    Public Property DataName() As String

    Public Property LinacName() As String
    Public Property Tabby() As String
    Public Property UserReason() As Integer


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        AddHandler WriteDatauc1.UserApproved, AddressOf UserApprovedEvent
        appstate = "LogOn" + LinacName
        actionstate = "ActionState" + LinacName
        suspstate = "Suspended" + LinacName
        failstate = "FailState" + LinacName
        repairstate = "rppTab" + LinacName
        LinacFlag = "State" + LinacName
        BoxChanged = "EngBoxChanged" + LinacName
        tabstate = "ActTab" + LinacName
    End Sub

    Protected Sub Update_Today(ByVal EquipmentID As String, ByVal incidentID As String)
        If LinacName = EquipmentID Then
            Dim todayfault As TodayClosedFault = PlaceHolder5.FindControl("Todaysfaults")
            todayfault.SetGrid()
            Todaydefect = PlaceHolder1.FindControl("DefectDisplay")
            Todaydefect.ResetDefectDropDown(incidentID)
        End If
    End Sub

    Protected Sub Update_Defect(ByVal EquipmentID As String)
        If LinacName = EquipmentID Then
            Todaydefect = PlaceHolder1.FindControl("DefectDisplay")
            Todaydefect.UpDateDefectsEventHandler()
        End If
    End Sub
    Protected Sub Update_ViewOpenFaults(ByVal EquipmentID As String)
        If LinacName = EquipmentID Then
            objconViewFaults = FindControl("ViewOpenFaults")
            objconViewFaults.RebindViewFault()
        End If
    End Sub

    Protected Sub UserApprovedEvent(ByVal Tabset As String, ByVal Userinfo As String)
        Dim RunUp As EngRunup
        RunUp = GetEngRunup()
        Dim tabcontrol As String = Tabset
        Dim Action As String = Application(actionstate)
        Dim machinelabel As String = LinacName & "Page.aspx';"
        Dim username As String = Userinfo
        Dim Valid As Boolean = False
        Dim Activity As String = "Logged Off"
        Dim strScript As String = "<script>"

        'added for non-LA linac in MachineState
        Dim usergroupselected As Integer = 2
        Dim Reason As Integer = 2
        '1 or 7 is engineering tab or emergency run up tab
        If tabcontrol = "1" Or "7" Then

            Dim Textboxcomment As TextBox = FindControl("CommentBox")
            Dim Comment As String = Textboxcomment.Text
            If Action = "Confirm" Then
                'This app sets for repair, maintenance and physics tab to know that run up was done
                Application(repairstate) = 1
                Application(LinacFlag) = "Clinical"
                Valid = True
                DavesCode.ReusePC.CommitRunup(LinacName, Tabby, username, Comment, Valid, False, False)
                'inserted to make same as LA linac step without having to activate pre-clin tab
                DavesCode.Reuse.MachineState(username, usergroupselected, LinacName, Reason, False)
                DavesCode.Reuse.CommitPreClin(LinacName, username, Comment, False, False, Valid, False)
                Application(appstate) = Nothing
                HttpContext.Current.Application(BoxChanged) = Nothing
                Application(tabstate) = String.Empty
                Dim returnstring As String = LinacName + "page.aspx?tabref=3"

                Application(suspstate) = 1
                Response.Redirect(returnstring)

            Else

                Valid = False
                Application(LinacFlag) = "Linac Unauthorised"
                'added this line 27 jan 2016 to set repairstate
                Application(repairstate) = Nothing

                strScript += "alert('Not clinical Logging Off');"
                DavesCode.ReusePC.CommitRunup(LinacName, Tabby, username, Comment, Valid, False, False)
                Application(appstate) = Nothing
                HttpContext.Current.Application(BoxChanged) = Nothing
                Application(tabstate) = String.Empty

                strScript += "window.location='"
                strScript += machinelabel
                strScript += "</script>"
                ScriptManager.RegisterStartupScript(engHandoverButton, Me.GetType(), "JSCR", strScript.ToString(), False)
            End If


        End If
    End Sub

    Private Function GetEngRunup() As EngRunup
        Dim RunUp As EngRunup
        If Cache("EngRunup") Is Nothing Then
            'RunUp = DataListItem.GetEngRunup()
        End If
        Return RunUp
    End Function
    Protected Sub Page_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        WaitButtons("Tech")

        objconToday = Page.LoadControl("TodayClosedFault.ascx")
        objconToday.ID = "Todaysfaults"
        objconToday.LinacName = LinacName
        PlaceHolder5.Controls.Add(objconToday)
        'Dim websiteAlreadyAccessed As Boolean = False
        objconViewFaults = Page.LoadControl("ViewOpenFaults.ascx")
        CType(objconViewFaults, ViewOpenFaults).TabName = Tabby
        CType(objconViewFaults, ViewOpenFaults).LinacName = LinacName
        CType(objconViewFaults, ViewOpenFaults).ID = "ViewOpenFaults"
        PlaceHolder3.Controls.Add(objconViewFaults)
        AddHandler CType(objconViewFaults, ViewOpenFaults).UpDateDefect, AddressOf Update_Today
        AddHandler CType(objconViewFaults, ViewOpenFaults).UpDateDefectDisplay, AddressOf Update_Defect
        If Not Tabby = 1 Then
            LockElf.Visible = False
        End If

        Dim Vctrl As ViewCommentsuc = CType(FindControl("ViewCommentsuc1"), ViewCommentsuc)
        Vctrl.LinacName = LinacName
        Dim lockctrl As LockElfuc = CType(FindControl("LockElfuc1"), LockElfuc)
        lockctrl.LinacName = LinacName

        Dim objDefect As UserControl = Page.LoadControl("DefectSavePark.ascx")
        CType(objDefect, DefectSavePark).ID = "DefectDisplay"
        CType(objDefect, DefectSavePark).LinacName = LinacName
        CType(objDefect, DefectSavePark).ParentControl = Tabby
        PlaceHolder1.Controls.Add(objDefect)
        'To update concession and closed fault table
        AddHandler CType(objDefect, DefectSavePark).UpDateDefect, AddressOf Update_Today
        AddHandler CType(objDefect, DefectSavePark).UpdateViewFault, AddressOf Update_ViewOpenFaults


        'Wire up the event (UserApproved) to the event handler (UserApprovedEvent)
        'The solution of how to pass parameter to dynamically loaded user control is from here:
        'http://weblogs.asp.net/aghausman/archive/2009/04/15/how-to-pass-parameters-to-the-dynamically-added-user-control.aspx
        'Dim objConCommit As WriteDatauc1 = Page.LoadControl("WriteDatauc.ascx")
        'CType(objConCommit, WriteDatauc1).LinacName = MachineName
        'CType(objConCommit, WriteDatauc1).UserReason = 1
        'CType(objConCommit, WriteDatauc1).Tabby = 1
        'CType(objConCommit, WriteDatauc).Source = Page
        'CType(objConCommit, WriteDatauc1).Visible = False
        'CType(objConCommit, WriteDatauc1).ID = WriteName
        'PlaceHolder4.Controls.Add(objConCommit)
        PlaceHolder4.Visible = True
        Dim ControlName As String = DataName
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        wctrl.LinacName = LinacName
        wctrl.UserReason = UserReason
        wctrl.Tabby = Tabby
        Dim Textboxcomment As TextBox = FindControl("CommentBox")
        If Not IsPostBack Then
            If (Not HttpContext.Current.Application(BoxChanged) Is Nothing) Then
                Textboxcomment.Text = Application(BoxChanged).ToString
            Else
                'Textboxcomment.Text = comment
            End If

            Application(LinacFlag) = "Linac Unauthorised"

        End If
        'websiteAlreadyAccessed = True
    End Sub

    Protected Sub EngHandoverButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles engHandoverButton.Click
        Dim strScript As String = "<script>"
        'count if there are unacknowledged rad concessions first
        Dim Radcount As Boolean
        Radcount = ConfirmNoRadConcession()
        If Radcount Then
            ConfirmExitEvent()
        Else
            strScript += "alert('There are unacknowledged Rad Concessions');"
            strScript += "</script>"
            ScriptManager.RegisterStartupScript(engHandoverButton, Me.GetType(), "JSCR", strScript.ToString(), False)
        End If
    End Sub

    Protected Function ConfirmNoRadConcession() As Boolean
        Dim comm As SqlCommand
        Dim conn As SqlConnection
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        Dim reader As SqlDataReader
        Dim NumOpen As Integer
        conn = New SqlConnection(connectionString)
        comm = New SqlCommand("SELECT Count(*) as NumOpen From RadAckFault r Left outer join concessiontable c on r.Incidentid = c.Incidentid where r.Acknowledge = 'false' and linac=@linac", conn)
        comm.Parameters.AddWithValue("@linac", LinacName)

        conn.Open()
        reader = comm.ExecuteReader()

        If reader.Read() Then
            NumOpen = reader.Item("NumOpen")
            If NumOpen <> 0 Then 'there are open faults
                Return False
            Else
                Return True
            End If
        Else
            Return True

        End If
    End Function

    Protected Sub LogOff_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LogOffButton.Click


        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
        wcbutton.Text = "Log Off"
        Application(actionstate) = "Cancel"
        WriteDatauc1.Visible = True
        ForceFocus(wctext)

    End Sub

    Protected Sub CommentBox_TextChanged(sender As Object, e As System.EventArgs) Handles CommentBox.TextChanged
        Application(BoxChanged) = CommentBox.Text
        'DavesCode.Reuse.ReturnApplicationState(BoxChanged)
    End Sub

    '15 April Added this control as a result of Bug 11
    Protected Sub LockElf_Click(sender As Object, e As System.EventArgs) Handles LockElf.Click
        '15 April test mod next 6 lines
        Dim lockctrl As LockElfuc = CType(FindControl("LockElfuc1"), LockElfuc)
        Dim lockctrltext As TextBox = CType(lockctrl.FindControl("txtchkUserName"), TextBox)
        'Dim grdview As GridView = FindControl("Gridview1")
        Dim Textboxcomment As TextBox = FindControl("CommentBox")
        Dim Comment As String = Textboxcomment.Text
        'has to be tablable to cope with either tab 1 or 7 control
        DavesCode.ReusePC.CommitRunup(LinacName, Tabby, "Lockuser", Comment, False, False, True)
        RaiseEvent BlankGroup(0)
        lockctrl.Visible = True
        ForceFocus(lockctrltext)

    End Sub

    '15 April Comment added as a result of Bug 6
    Private Sub ForceFocus(ByVal ctrl As Control)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "FocusScript", "setTimeout(function(){$get('" +
        ctrl.ClientID + "').focus();}, 100);", True)
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
    Protected Sub ConfirmExitEvent()
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
        wcbutton.Text = "Confirm Clinical"
        Application(actionstate) = "Confirm" 'This should only happen if log in is ok move to writedatauc
        WriteDatauc1.Visible = True
        ForceFocus(wctext)
    End Sub
End Class

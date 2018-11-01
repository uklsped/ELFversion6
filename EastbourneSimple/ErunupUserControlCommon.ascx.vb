Imports System.Data.SqlClient
Imports AjaxControlToolkit

Partial Class ErunupUserControl
    Inherits UserControl

    Private mpContentPlaceHolder As ContentPlaceHolder
    Private appstate As String
    Private suspstate As String
    Private actionstate As String
    Private failstate As String
    Private repairstate As String
    Private faultviewstate As String
    Private atlasviewstate As String
    Private qaviewstate As String
    Private LinacFlag As String
    Private objconToday As TodayClosedFault
    Private Todaydefect As DefectSave
    Private BoxChanged As String
    Public Event BlankGroup(ByVal BlankUser As Integer)
    Private tabstate As String
    Private Objcon As ViewOpenFaults
    'Private EngLoad As String
    Dim accontrol1 As AcceptLinac
    Dim accontrol7 As AcceptLinac
    Private Obpage As Page

    Public ReadOnly Property CurrentComment() As String
        Get
            Return CommentBox.Text
        End Get
    End Property

    Public Property DataName() As String
    Public Property LinacName() As String
    Public Property Tabby() As String
    Public Property UserReason() As Integer


    Public Function FormatImage(ByVal energy As Boolean) As String
        Dim happyIcon As String = "Images/happy.gif"
        Dim sadIcon As String = "Images/sad.gif"
        If (energy) Then
            Return happyIcon
        Else
            Return sadIcon
        End If
    End Function

    Protected Sub Checked(ByVal sender As Object, ByVal e As EventArgs)
        Dim toggle As CheckBox = CType(GridView1.HeaderRow.FindControl("chkSelectAll"), CheckBox)

        If toggle.Checked = True Then
            For Each grv As GridViewRow In GridView1.Rows
                Dim cb As CheckBox = CType(grv.FindControl("RowlevelCheckBox"), CheckBox)
                If cb.Enabled = True Then
                    cb.Checked = True
                End If
            Next
        Else
            For Each grv As GridViewRow In GridView1.Rows
                Dim cb As CheckBox = CType(grv.FindControl("RowlevelCheckBox"), CheckBox)
                cb.Checked = False
            Next
        End If

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        Dim tabcontainer1 As TabContainer
        Page = Me.Page
        mpContentPlaceHolder =
        CType(Page.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        If Not mpContentPlaceHolder Is Nothing Then
            tabcontainer1 = CType(mpContentPlaceHolder.
                FindControl("tcl"), TabContainer)
            If Not tabcontainer1 Is Nothing Then
                If Tabby = 1 Then
                    Dim panelcontrol1 As TabPanel = tabcontainer1.FindControl("TabPanel1")
                    accontrol1 = panelcontrol1.FindControl("AcceptLinac1")
                    AddHandler accontrol1.EngRunuploaded, AddressOf EngLogOnEvent

                Else
                    Dim panelcontrol7 As TabPanel = tabcontainer1.FindControl("TabPanel7")
                    accontrol7 = panelcontrol7.FindControl("AcceptLinac7")
                    AddHandler accontrol7.EngRunuploaded, AddressOf EngLogOnEvent
                End If
            End If
        End If
        AddHandler WriteDatauc1.UserApproved, AddressOf UserApprovedEvent
        AddHandler ConfirmPage1.ConfirmExit, AddressOf ConfirmExitEvent ' this is if imaging wasn't selected

        appstate = "LogOn" + LinacName
        actionstate = "ActionState" + LinacName
        suspstate = "Suspended" + LinacName
        failstate = "FailState" + LinacName
        repairstate = "rppTab" + LinacName
        faultviewstate = "Faultsee" + LinacName
        atlasviewstate = "Atlassee" + LinacName
        qaviewstate = "QAsee" + LinacName
        LinacFlag = "State" + LinacName
        BoxChanged = "EngBoxChanged" + LinacName
        tabstate = "ActTab" + LinacName
        'EngLoad = "Loaded" + LinacName
    End Sub

    Public Sub EngLogOnEvent(connectionString As String)

        SetEnergies(connectionString)
        Select Case LinacName
            Case "E1", "E2", "B1"
                SetImaging(connectionString)
        End Select

        GridView1.Visible = True
        GridViewImage.Visible = True
        Application(atlasviewstate) = 1

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
            Objcon = FindControl("ViewOpenFaults")
            Objcon.RebindViewFault()
        End If
    End Sub

    Public Sub UserApprovedEvent(ByVal Tabset As String, ByVal Userinfo As String)

        If Tabset = "1" Or "7" Then
            Dim Action As String = Application(actionstate)
            Dim machinelabel As String = LinacName & "Page.aspx';"
            Dim Valid As Boolean = False
            Dim strScript As String = "<script>"

            'added for non-LA linac in MachineState
            'Dim usergroupselected As Integer = 2
            'Dim Reason As Integer = 2
            '1 or 7 is engineering tab or emergency run up tab
            'If Tabset = "1" Or "7" Then
            'DavesCode.Reuse.ReturnApplicationState(tabcontrol)
            Dim grdview As GridView = FindControl("Gridview1")
            Dim grdviewI As GridView = FindControl("GridViewImage")
            Dim Textboxcomment As TextBox = FindControl("CommentBox")
            Dim Comment As String = Textboxcomment.Text
            Dim Successful As Boolean = False
            If Action = "Confirm" Then
                Valid = True
                Successful = DavesCode.NewEngRunup.CommitRunup(grdview, grdviewI, LinacName, Tabby, Userinfo, Comment, Valid, False, False)
                If Successful Then
                    'This app sets for repair, maintenance and physics tab to know that run up was done
                    Application(repairstate) = 1
                    'Valid = True
                    Application(appstate) = Nothing
                    Application(tabstate) = String.Empty
                    HttpContext.Current.Application(BoxChanged) = Nothing
                    'Moved dal to newengrunup 19/9/18 to aid error handling
                    'Successful = DavesCode.NewEngRunup.CommitRunup(grdview, grdviewI, LinacName, Tabby, username, Comment, Valid, False, False)
                    'If Successful Then
                    If LinacName Like "LA?" Then
                        Application(LinacFlag) = "Engineering Approved"
                        strScript += "window.location='"
                        strScript += machinelabel
                        strScript += "</script>"
                        ScriptManager.RegisterStartupScript(engHandoverButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                    Else
                        Application(LinacFlag) = "Clinical"
                        Dim returnstring As String = LinacName + "page.aspx?tabref=3"
                        Application(suspstate) = 1
                        Response.Redirect(returnstring)
                    End If
                Else
                    RaiseError("WriteEnergies")
                End If

            Else
                Valid = False
                Successful = DavesCode.NewEngRunup.CommitRunup(grdview, grdviewI, LinacName, Tabby, Userinfo, Comment, Valid, False, False)
                If Successful Then
                    Application(LinacFlag) = "Linac Unauthorised"
                    Application(repairstate) = Nothing
                    Application(appstate) = Nothing
                    Application(tabstate) = String.Empty
                    HttpContext.Current.Application(BoxChanged) = Nothing
                    strScript += "alert('No Energies Approved Logging Off');"
                    strScript += "window.location='"
                    strScript += machinelabel
                    strScript += "</script>"
                    ScriptManager.RegisterStartupScript(engHandoverButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                Else
                    RaiseError("WriteEnergies")
                End If
            End If

        End If
    End Sub
    Protected Sub RaiseLoadError()
        Dim machinelabel As String = LinacName & "Page.aspx';"
        Application(LinacFlag) = "Linac Unauthorised"
        Application(repairstate) = Nothing
        Application(appstate) = Nothing
        HttpContext.Current.Application(BoxChanged) = Nothing
        Application(tabstate) = String.Empty
        Dim strScript As String = "<script>"
        strScript += "alert('Problem Loading Values. Please call Engineer');"
        strScript += "window.location='"
        strScript += machinelabel
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(engHandoverButton, Me.GetType(), "JSCR", strScript.ToString(), False)
    End Sub
    Protected Sub RaiseError(ByVal message As String)

        Select Case message
            Case "Images"
                message = "alert('Problem Loading Imaging. Logging off without Approving Energies');"
            Case "WriteEnergies"
                message = "alert('Problem Recording Energies. Logging off without Approving Energies');"
            Case "Radconcess"
                message = "alert('Problem checking Rad Concessions. Logging off without Approving Energies');"
        End Select

        Dim strScript As String = "<script>"
        Dim machinelabel As String = LinacName & "Page.aspx';"
        Application(LinacFlag) = "Linac Unauthorised"
        Application(repairstate) = Nothing
        Application(appstate) = Nothing
        HttpContext.Current.Application(BoxChanged) = Nothing
        Application(tabstate) = String.Empty
        strScript += message
        strScript += "window.location='"
        strScript += machinelabel
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(engHandoverButton, Me.GetType(), "JSCR", strScript.ToString(), False)

    End Sub
    Protected Sub RaiseLockError()
        Dim strScript As String = "<script>"
        'DavesCode.ReusePC.InsertReportFault("System Error", "System", Now(), EMPTYSTRING, EMPTYSTRING, EMPTYSTRING, EMPTYSTRING, LinacName, -1000, EMPTYSTRING, "System Error", False)
        'RaiseEvent UpDateDefectDisplay(LinacName)
        strScript += "alert('Problem Locking Elf. Please inform system administator');"
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(LockElf, Me.GetType(), "JSCR", strScript.ToString(), False)
    End Sub

    Protected Sub Page_load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        WaitButtons("Tech")
        Dim SuccessEnergy As Boolean = False
        Dim SuccessImage As Boolean = False
        objconToday = Page.LoadControl("TodayClosedFault.ascx")
        objconToday.ID = "Todaysfaults"
        objconToday.LinacName = LinacName
        PlaceHolder5.Controls.Add(objconToday)
        Dim websiteAlreadyAccessed As Boolean = False
        Dim objConAtlas As UserControl = Page.LoadControl("AtlasEnergyViewuc.ascx")
        CType(objConAtlas, AtlasEnergyViewuc).LinacName = LinacName
        PlaceHolder2.Controls.Add(objConAtlas)
        Objcon = Page.LoadControl("ViewOpenFaults.ascx")

        Dim strScript As String = "<script>"

        Dim Vctrl As ViewCommentsuc = CType(FindControl("ViewCommentsuc1"), ViewCommentsuc)
        Vctrl.LinacName = LinacName
        Dim lockctrl As LockElfuc = CType(FindControl("LockElfuc1"), LockElfuc)
        lockctrl.LinacName = LinacName
        If Tabby = 1 Then

            CType(Objcon, ViewOpenFaults).TabName = Tabby
            CType(Objcon, ViewOpenFaults).LinacName = LinacName
            PlaceHolder3.Controls.Add(Objcon)
        Else
            'Hide lock elf button if rad run up
            LockElf.Visible = False
            CType(Objcon, ViewOpenFaults).TabName = Tabby
            CType(Objcon, ViewOpenFaults).LinacName = LinacName
            PlaceHolder3.Controls.Add(Objcon)
        End If

        Dim objQA As UserControl = Page.LoadControl("WebUserControl2.ascx")
        CType(objQA, WebUserControl2).LinacName = LinacName
        CType(objQA, WebUserControl2).TabName = 1
        PlaceHolder6.Controls.Add(objQA)


        AddHandler CType(Objcon, ViewOpenFaults).UpDateDefect, AddressOf Update_Today
        AddHandler CType(Objcon, ViewOpenFaults).UpDateDefectDisplay, AddressOf Update_Defect
        Dim objDefect As UserControl
        If LinacName Like "T?" Then
            objDefect = Page.LoadControl("DefectSavePark.ascx")
            CType(objDefect, DefectSavePark).ID = "DefectDisplay"
            CType(objDefect, DefectSavePark).LinacName = LinacName
            CType(objDefect, DefectSavePark).ParentControl = 1
            AddHandler CType(objDefect, DefectSavePark).UpDateDefect, AddressOf Update_Today
            AddHandler CType(objDefect, DefectSavePark).UpdateViewFault, AddressOf Update_ViewOpenFaults

        Else
            objDefect = Page.LoadControl("DefectSave.ascx")
            CType(objDefect, DefectSave).ID = "DefectDisplay"
            CType(objDefect, DefectSave).LinacName = LinacName
        End If

        PlaceHolder1.Controls.Add(objDefect)



        'Wire up the event (UserApproved) to the event handler (UserApprovedEvent)
        'The solution of how to pass parameter to dynamically loaded user control is from here:
        'http://weblogs.asp.net/aghausman/archive/2009/04/15/how-to-pass-parameters-to-the-dynamically-added-user-control.aspx

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
        End If
        websiteAlreadyAccessed = True
    End Sub
    Protected Sub SetEnergies(ByVal connectionString As String)
        Dim SelCommand As String = ""
        If Tabby = 1 Then
            'added imaging
            SelCommand = "SELECT * FROM [physicsenergies] where linac= @linac and Energy not in ('iView','XVI')"
        Else
            SelCommand = "SELECT * FROM [physicsenergies] where linac= @linac and EnergyID in (1,2,10,11,19,27,28)"
        End If
        Dim SqlDataSource1 As New SqlDataSource With {
            .ID = "SqlDataSource1",
            .ConnectionString = connectionString,
            .SelectCommand = SelCommand
        }
        SqlDataSource1.SelectParameters.Add("@linac", Data.SqlDbType.NVarChar)
        SqlDataSource1.SelectParameters.Add("linac", LinacName)

        GridView1.DataSource = SqlDataSource1
        GridView1.DataBind()


        ''This makes visible checkboxes for those energies that are approved
        Dim conn As SqlConnection
        Dim comm As SqlCommand
        Dim reader As SqlDataReader
        Dim count As Integer = 0
        'Dim connectionString1 As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        conn = New SqlConnection(connectionString)
        If Tabby = 1 Then
            'added imaging
            comm = New SqlCommand("SELECT EnergyID, Approved FROM physicsenergies where linac=@linac and Energy not in ('iView','XVI')", conn)
        Else
            comm = New SqlCommand("SELECT EnergyID, Approved FROM physicsenergies where linac=@linac and EnergyID in (1,2,10,11,19,27,28)", conn)
        End If
        comm.Parameters.Add("@linac", Data.SqlDbType.NVarChar, 10)
        comm.Parameters("@linac").Value = LinacName
        'Try
        conn.Open()
        reader = comm.ExecuteReader()
        While reader.Read()
            'This will fall over if approved is null so needs error handling
            'Added handling 4/7/17
            If Not IsDBNull(reader.Item("Approved")) Then
                If Not reader.Item("Approved") Then
                    Dim cb As CheckBox = CType(GridView1.Rows(count).FindControl("RowLevelCheckBox"), CheckBox)
                    cb.Enabled = False
                    cb.Visible = False
                End If
            Else
                Dim cb As CheckBox = CType(GridView1.Rows(count).FindControl("RowLevelCheckBox"), CheckBox)
                cb.Enabled = False
                cb.Visible = False
            End If
            count = count + 1
        End While
        reader.Close()
        conn.Close()

    End Sub
    Protected Function SetImaging(ByVal connectionString As String) As Boolean
        Dim count As Integer = 0
        Dim conn As SqlConnection
        Dim Success As Boolean = False

        Dim SqlDataSource1 As New SqlDataSource With {
            .ID = "SqlDataSource1",
            .ConnectionString = connectionString,
            .SelectCommand = "SELECT * FROM [physicsenergies] where linac=@linac and Energy in ('iView','XVI') order by energy"
        }

        SqlDataSource1.SelectParameters.Add("@linac", Data.SqlDbType.NVarChar)
        SqlDataSource1.SelectParameters.Add("linac", LinacName)

        GridViewImage.DataSource = SqlDataSource1
        GridViewImage.DataBind()

        Dim comm As SqlCommand
        Dim reader As SqlDataReader

        conn = New SqlConnection(connectionString)
        comm = New SqlCommand("SELECT EnergyID, Approved FROM physicsenergies where linac=@linac and Energy in ('iView','XVI') order by energy", conn)

        comm.Parameters.Add("@linac", Data.SqlDbType.NVarChar, 10)
        comm.Parameters("@linac").Value = LinacName

        conn.Open()
        reader = comm.ExecuteReader()
        While reader.Read()
            'This will fall over if approved is null so needs error handling
            'Same fix as Engineering run up energies 4/7/17
            If Not IsDBNull(reader.Item("Approved")) Then
                If Not reader.Item("Approved") Then
                    Dim cb As CheckBox = CType(GridViewImage.Rows(count).FindControl("RowLevelCheckBoxImage"), CheckBox)
                    cb.Enabled = False
                    cb.Visible = False
                End If
            Else
                CType(GridViewImage.Rows(count).FindControl("RowLevelCheckBoxImage"), CheckBox).Enabled = False
                CType(GridViewImage.Rows(count).FindControl("RowLevelCheckBoxImage"), CheckBox).Visible = False
            End If

            count = count + 1
        End While
        reader.Close()

        conn.Close()

        Return Success

    End Function

    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        For Each grv As GridViewRow In GridView1.Rows
            Dim cb As CheckBox = CType(grv.FindControl("RowlevelCheckBox"), CheckBox)
            If cb.Enabled = True Then
                cb.Checked = True
            End If
        Next

    End Sub

    Protected Sub EngHandoverButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles engHandoverButton.Click
        Dim strScript As String = "<script>"
        'count if there are unacknowledged rad concessions first
        Dim Radcount As Boolean
        Radcount = ConfirmNoRadConcession()
        If Radcount Then
            If LinacName Like "T?" Then
                ConfirmExitEvent()
            Else
                Dim counter As Integer = 0
                For Each grv As GridViewRow In GridView1.Rows
                    Dim checktick As CheckBox = CType(grv.FindControl("RowlevelCheckBox"), CheckBox)
                    If checktick.Checked = True Then
                        counter = counter + 1
                    End If
                Next
                Select Case counter
                    Case Is <> 0

                        If LinacName Like "LA?" Then
                            ConfirmExitEvent()
                        Else
                            'Inserted imaging check from preclinicaluser control
                            Dim icounter As Integer = 0
                            'Dim tclcontainer As TabContainer
                            For Each grv As GridViewRow In GridViewImage.Rows

                                Dim checktick As CheckBox = CType(grv.FindControl("RowlevelCheckBoxImage"), CheckBox)
                                If checktick.Checked = True Then
                                    icounter = icounter + 1
                                End If
                            Next
                            If icounter <> 0 Then
                                ConfirmExitEvent()
                            Else
                                Dim cptrl As ConfirmPage = CType(FindControl("ConfirmPage1"), ConfirmPage)
                                Dim cpbutton As Button = CType(cptrl.FindControl("AcceptOK"), Button)
                                'Dim cptext As TextBox = CType(cptrl.FindControl("txtchkUserName"), TextBox)
                                cpbutton.Text = "Confirm No Imaging"
                                ConfirmPage1.Visible = True
                                'ForceFocus(cptext)

                            End If

                        End If

                    Case Else
                        strScript += "alert('Select at least one energy');"
                        strScript += "</script>"
                        ScriptManager.RegisterStartupScript(engHandoverButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                End Select
            End If

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
        Try
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
                'No need for this for if there is no record there has been a sql error
                'Else
                '    Return True

            End If
        Catch ex As Exception
            DavesCode.ReusePC.LogError(ex)
            RaiseError("Radconcess")
        Finally
            conn.Close()
        End Try

    End Function


    Protected Sub LogOff_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LogOffButton.Click

        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
        wcbutton.Text = "Log Off"
        Application(actionstate) = "Cancel"
        WriteDatauc1.Visible = True
        ForceFocus(wctext)

    End Sub

    Protected Sub AtlasPanelButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ViewAtlasButton.Click
        Dim updateAtlas As UpdatePanel = FindControl("updatepanelatlas")
        If Application(atlasviewstate) = 1 Then
            updateAtlas.Visible = True
            Application(atlasviewstate) = Nothing
            ViewAtlasButton.Text = "Hide Atlas Energies"
        Else
            updateAtlas.Visible = False
            Application(atlasviewstate) = 1
            ViewAtlasButton.Text = "View Atlas Energies"
        End If
    End Sub


    Protected Sub CommentBox_TextChanged(sender As Object, e As EventArgs) Handles CommentBox.TextChanged
        Application(BoxChanged) = CommentBox.Text
        'DavesCode.Reuse.ReturnApplicationState(BoxChanged)
    End Sub

    '15 April Added this control as a result of Bug 11
    Protected Sub LockElf_Click(sender As Object, e As EventArgs) Handles LockElf.Click
        '15 April test mod next 6 lines
        Dim lockctrl As LockElfuc = CType(FindControl("LockElfuc1"), LockElfuc)
        Dim lockctrltext As TextBox = CType(lockctrl.FindControl("txtchkUserName"), TextBox)
        Dim grdview As GridView = FindControl("Gridview1")
        Dim grdviewI As GridView = FindControl("GridViewImage")
        Dim Textboxcomment As TextBox = FindControl("CommentBox")
        Dim Comment As String = Textboxcomment.Text
        Dim success As Boolean = False
        'has to be tablable to cope with either tab 1 or 7 control
        success = DavesCode.NewEngRunup.CommitRunup(grdview, grdviewI, LinacName, Tabby, "Lockuser", Comment, False, False, True)
        'DavesCode.Reuse.CommitRunup(grdview, LinacName, Tabby, "Lockuser", Comment, False, False, True)
        If success Then
            RaiseEvent BlankGroup(0)
            lockctrl.Visible = True
            ForceFocus(lockctrltext)
        Else
            RaiseLockError()
        End If

    End Sub

    '15 April comment. Added this control as a result of Bug 11
    Protected Sub PhysicsQA_Click(ByVal sender As Object, ByVal e As EventArgs) Handles PhysicsQA.Click
        Dim updatepanelQA As UpdatePanel = FindControl("updatepanelQA")
        If Application(qaviewstate) = 1 Then
            updatepanelQA.Visible = True
            Application(qaviewstate) = Nothing
            PhysicsQA.Text = "Hide Physics Energies/Imaging"
        Else
            updatepanelQA.Visible = False
            Application(qaviewstate) = 1
            PhysicsQA.Text = "View Physics Energies/Imaging"
        End If
    End Sub

    '15 April Comment added as a result of Bug 6
    Private Sub ForceFocus(ByVal ctrl As Control)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "FocusScript", "setTimeout(function(){$get('" + _
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
        wcbutton.Text = "Confirm Energies"
        Application(actionstate) = "Confirm" 'This should only happen if log in is ok move to writedatauc
        WriteDatauc1.Visible = True
        ForceFocus(wctext)
    End Sub


End Class

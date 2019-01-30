Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports AjaxControlToolkit
Imports System.Transactions


Partial Class ClinicalUserControl
    Inherits UserControl

    Private mpContentPlaceHolder As ContentPlaceHolder
    Private CurrentCID As Integer
    Private colourstart As Color = Color.FromArgb(255, 204, 0)
    Private colourstop As Color = Color.FromArgb(102, 153, 153)
    Private StateLabel As Label
    Private ActivityLabel As Label
    Private appstate As String
    Private suspstate As String
    Private actionstate As String
    Private failstate As String
    Private repairstate As String
    Private faultviewstate As String
    Private treatmentstate As String
    Private clinicalstate As String
    Private returnclinical As String
    Private LinacFlag As String
    Private objconToday As TodayClosedFault
    Private Todaydefect As DefectSave
    Private TodaydefectPark As DefectSavePark
    Dim BoxChanged As String
    Dim accontrol As AcceptLinac
    Private tabstate As String
    Private TodayComment As controls_CommentBoxuc
    Const CLINICAL As String = "3"

    Public Property LinacName() As String

    Public Function FormatImage(ByVal energy As Boolean) As String

        Dim happyIcon As String = "Images/check_mark.jpg"

        Dim sadIcon As String = "Images/box_with_x.jpg"
        If energy Then
            Return happyIcon
        Else
            Return sadIcon
        End If
    End Function

    Protected Sub Update_FaultClosedDisplays(ByVal EquipmentID As String, ByVal incidentID As String)
        If LinacName = EquipmentID Then
            Dim todayfault As TodayClosedFault = PlaceHolder5.FindControl("Todaysfaults")
            todayfault.SetGrid()
            If LinacName Like "T?" Then
                TodaydefectPark = PlaceHolder1.FindControl("DefectDisplay")
                TodaydefectPark.ResetDefectDropDown(incidentID)
            Else
                Todaydefect = PlaceHolder1.FindControl("DefectDisplay")
                Todaydefect.ResetDefectDropDown(incidentID)
            End If

        End If
    End Sub

    ' This updates the defect display on defectsave etc when repeat fault from viewopenfaults
    Protected Sub Update_DefectDailyDisplay(ByVal EquipmentID As String)
        If LinacName = EquipmentID Then
            If LinacName Like "T?" Then
                TodaydefectPark = PlaceHolder1.FindControl("DefectDisplay")
                TodaydefectPark.UpDateDefectsEventHandler()
            Else
                Todaydefect = PlaceHolder1.FindControl("DefectDisplay")
                Todaydefect.UpDateDefectsEventHandler()
            End If

        End If
    End Sub

    Protected Sub Update_ViewOpenFaults(ByVal EquipmentID As String)
        If LinacName = EquipmentID Then
            Dim updatefault As ViewOpenFaults = PlaceHolder4.FindControl("ViewOpenFaults")
            updatefault.RebindViewFault()
        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'The method of finding acceptlinac3 started from here http://forums.asp.net/t/1380670.aspx?Access+Master+page+properties+from+User+Control

        Dim tabcontainer1 As TabContainer
        Page = Me.Page
        mpContentPlaceHolder =
        CType(Page.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        If Not mpContentPlaceHolder Is Nothing Then
            tabcontainer1 = CType(mpContentPlaceHolder.
                FindControl("tcl"), TabContainer)
            If Not tabcontainer1 Is Nothing Then
                Dim panelcontrol As TabPanel = tabcontainer1.FindControl("TabPanel3")
                accontrol = panelcontrol.FindControl("AcceptLinac3")
                AddHandler accontrol.ClinicalApproved, AddressOf ClinicalApprovedEvent
            End If
        End If

        AddHandler WriteDatauc2.UserApproved, AddressOf UserApprovedEvent

        If Not mpContentPlaceHolder Is Nothing Then
            StateLabel = CType(mpContentPlaceHolder.
                FindControl("StateLabel"), Label)
            ActivityLabel = CType(mpContentPlaceHolder.FindControl("ActivityLabel"), Label)
        End If

        appstate = "LogOn" + LinacName
        actionstate = "ActionState" + LinacName
        suspstate = "Suspended" + LinacName
        failstate = "FailState" + LinacName
        repairstate = "rppTab" + LinacName
        faultviewstate = "Faultsee" + LinacName
        clinicalstate = "ClinicalOn" + LinacName
        treatmentstate = "Treatment" + LinacName
        returnclinical = "ReturnClinical" + LinacName
        LinacFlag = "State" + LinacName
        BoxChanged = "ClinBoxChanged" + LinacName
        tabstate = "ActTab" + LinacName
    End Sub

    Public Sub ClinicalApprovedEvent(ByVal connectionString As String)
        'This is the point that the gridviews are displayed and Clinical Table Data is written
        BindEnergyData()

        'This looks to see if BoxChanged has a value. if it has the comment has not been saved.
        If Not HttpContext.Current.Application(BoxChanged) Is Nothing Then
            HiddenFieldLinacState.Value = DavesCode.NewCommitClinical.NewWriteClinicalTable(LinacName, HttpContext.Current.Application(BoxChanged), connectionString)
            CommentBox.ResetCommentBox(String.Empty)
        End If
        BindComments()
        Application(suspstate) = Nothing
    End Sub

    Public Sub UserApprovedEvent(ByVal tabused As String, ByVal Userinfo As String)

        Dim Action As String = Application(actionstate)
        Dim machinelabel As String = LinacName & "Page.aspx"
        Dim username As String = Userinfo
        'Dim linacstatusid As String = HiddenFieldLinacState.Value
        Dim Result As Boolean = False

        If tabused = CLINICAL Or tabused = "Recover" Then
            Dim FaultParams As DavesCode.FaultParameters = New DavesCode.FaultParameters()
            Result = DavesCode.NewCommitClinical.CommitClinical(LinacName, username, False, FaultParams)
            If Result Then
                If Action = "Confirm" Then
                    Dim activity As Integer = 7 'This will always be log off?
                    If Application(treatmentstate) = "No" Then
                        activity = 8
                    ElseIf Application(treatmentstate) = "Yes" Then
                        activity = 3
                    Else
                        'This has gone wrong
                    End If
                    CommentBox.ResetCommentBox(String.Empty)
                    Application(treatmentstate) = "Yes"
                    Application(appstate) = Nothing
                    Application(suspstate) = 1
                    Application(repairstate) = Nothing
                    StateLabel.Text = "Suspended"
                    ActivityLabel.Text = "Logged Off"
                    Application(LinacFlag) = "Suspended"
                    Response.Redirect(machinelabel)

                End If
            Else
                RaiseLogOffError()
                If tabused = "Recover" Then
                    'if this is from restore then don't want to go back to recovery
                    Response.Redirect("/Errorpages/Oops.aspx")
                End If
            End If
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SaveText.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(SaveText, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
        WaitButtons("Rad")
        objconToday = Page.LoadControl("TodayClosedFault.ascx")
        objconToday.ID = "Todaysfaults"
        objconToday.LinacName = LinacName
        PlaceHolder5.Controls.Add(objconToday)

        Dim objCon As ViewOpenFaults = Page.LoadControl("ViewOpenFaults.ascx")
        CType(objCon, ViewOpenFaults).LinacName = LinacName
        CType(objCon, ViewOpenFaults).ID = "ViewOpenFaults"
        CType(objCon, ViewOpenFaults).ParentControl = CLINICAL
        PlaceHolder1.Controls.Add(objCon)
        PlaceHolder2.Visible = True

        AddHandler CType(objCon, ViewOpenFaults).UpDateDefectDailyDisplay, AddressOf Update_DefectDailyDisplay

        Dim objDefect As UserControl
        If LinacName Like "T?" Then
            objDefect = Page.LoadControl("DefectSavePark.ascx")
            CType(objDefect, DefectSavePark).ID = "DefectDisplay"
            CType(objDefect, DefectSavePark).LinacName = LinacName
            CType(objDefect, DefectSavePark).ParentControl = CLINICAL
            AddHandler CType(objDefect, DefectSavePark).UpdateFaultClosedDisplays, AddressOf Update_FaultClosedDisplays
            AddHandler CType(objDefect, DefectSavePark).UpdateViewOpenFaults, AddressOf Update_ViewOpenFaults

        Else
            objDefect = Page.LoadControl("DefectSave.ascx")
            CType(objDefect, DefectSave).ID = "DefectDisplay"
            CType(objDefect, DefectSave).LinacName = LinacName
            CType(objDefect, DefectSave).ParentControl = CLINICAL
            AddHandler CType(objDefect, DefectSave).UpdateViewOpenFaults, AddressOf Update_ViewOpenFaults
        End If

        PlaceHolder3.Controls.Add(objDefect)

        Dim Vctrl As ViewCommentsuc = CType(FindControl("ViewCommentsuc1"), ViewCommentsuc)
        Vctrl.LinacName = LinacName
        Dim laststate As String
        Dim wctrl2 As WriteDatauc = CType(FindControl("Writedatauc2"), WriteDatauc)
        wctrl2.LinacName = LinacName
        CommentBox.BoxChanged = BoxChanged
        Dim conn As SqlConnection
        Dim connectionString1 As String = ConfigurationManager.ConnectionStrings(
        "connectionstring").ConnectionString
        conn = New SqlConnection(connectionString1)
        If Not IsPostBack Then
            If Not LinacName Like "LA?" Then
                PanelPreclincomments.Visible = False
            End If
            Dim treatval As String = Application(treatmentstate)

            Application(faultviewstate) = 1
            lastState = DavesCode.Reuse.GetLastState(LinacName, 0)
            Try
                Using myscope As TransactionScope = New TransactionScope()
                    If Application(returnclinical) = 1 Then


                        ClinicalApprovedEvent(connectionString1)

                        Select Case Application(treatmentstate)
                            Case "Yes"
                                Tstart.Text = "Start Treatment"
                                Tstart.BackColor = colourstart
                                ActivityLabel.Text = "Clinical - Not Treating"
                            Case "No"
                                Tstart.Text = "Stop Treatment"
                                Tstart.BackColor = colourstop
                                LogOffButton.Visible = True
                                ActivityLabel.Text = "Clinical - Treating"
                        End Select
                        Application(returnclinical) = Nothing
                    Else
                        SetButtonText()
                        myscope.Complete()

                    End If
                End Using
            Catch ex As Exception
                DavesCode.NewFaultHandling.LogError(ex)
                RaiseLoadError()
            End Try
        End If

    End Sub


    Protected Sub EnergyGridView_DataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GridView2.RowDataBound

        Dim headerRow As GridViewRow = e.Row
        Dim energy As Integer
        Dim query As String
        Dim SqlDataSource1 As New SqlDataSource()

        query = "Select distinct MV6,MV6FFF, MV10,MV10FFF,MeV4, MeV6, MeV8, " &
                  "MeV10, MeV12, MeV15, MeV18, MeV20, iView, XVI from HandoverEnergies e  left outer join clinicalhandover r on e.handoverid=r.ehandid where r.CHandID  = (Select max(CHandID) as lastrecord from ClinicalHandover where linac=@linac)"

        SqlDataSource1 = QuerySqlConnection(LinacName, query)

        If headerRow.RowType = DataControlRowType.Header Then
            Try
                Dim dv As DataView
                dv = CType(SqlDataSource1.Select(DataSourceSelectArguments.Empty), DataView)
                'added 16march
                Dim colnum As Integer = dv.Table.Columns.Count - 1
                For count As Integer = 0 To dv.Table.Columns.Count - 1
                    'This will fail if for some reason the value n dv.Table.Rows(0)(count) is null
                    'so check for null and if it is put energy = 0
                    If IsDBNull(dv.Table.Rows(0)(count)) Then
                        energy = 0
                    Else
                        energy = CType(dv.Table.Rows(0)(count), Integer)
                    End If

                    Select Case energy
                        Case -1
                            headerRow.Cells(count).BackColor = System.Drawing.Color.Green
                        Case 0
                            headerRow.Cells(count).BackColor = System.Drawing.Color.Red
                    End Select

                Next

            Finally

            End Try
        End If

    End Sub

    Private Sub BindComments()
        Dim SqlDateSourceComment As New SqlDataSource()

        Dim query As String = "select convert(Varchar(5),e.LogOutDate, 108) as DateTime, e.comment from handoverenergies e " &
         "where e.handoverid = (Select Max(handoverid) as mancount from [handoverenergies] where linac=@linac)"

        SqlDateSourceComment = QuerySqlConnection(LinacName, query)
        GridViewEng.DataSource = SqlDateSourceComment
        GridViewEng.DataBind()

        query = "select convert(Varchar(5),r.LogOutDate, 108) as DateTime, r.Ccomment from handoverenergies e left outer join clinicalhandover r On e.handoverid=r.ehandid " &
         "where e.handoverid = (Select Max(handoverid) as mancount from [handoverenergies] where linac=@linac)"

        SqlDateSourceComment = QuerySqlConnection(LinacName, query)
        GridViewPre.DataSource = SqlDateSourceComment
        GridViewPre.DataBind()

        query = "select convert(Varchar(5),c.DateTime, 108) as DateTime, c.ClinComment from handoverenergies e left outer join clinicalhandover r on e.handoverid=r.ehandid " &
        "Left outer join ClinicalTable c on c.PreClinID = r.CHandID where e.handoverid = (Select Max(handoverid) as mancount from [handoverenergies] where linac=@linac) and " &
        "c.PreClinID = (Select Max(CHandID) as mancount from [ClinicalHandover] where linac=@linac) order by c.datetime desc"

        SqlDateSourceComment = QuerySqlConnection(LinacName, query)

        GridViewComments.DataSource = SqlDateSourceComment
        GridViewComments.DataBind()

    End Sub

    Private Sub BindEnergyData()
        If LinacName IsNot "T1" Then
            Dim SqlDataSource1 As New SqlDataSource()
            'the distinct takes care of when suspended returns via pre-clinical because then there are two pre ids for one runup id
            'Dim query As String = "Select distinct handoverID, MV6, MV10, MeV6, MeV8, " & _
            '                          "MeV10, MeV12, MeV15, MeV18, MeV20, iView, XVI from HandoverEnergies e  left outer join clinicalhandover r On e.handoverid=r.ehandid where e.HandoverID  = (Select max(HandoverID) As lastrecord from HandoverEnergies where linac=@linac)"
            'Added isnull as per energy displayuc
            Dim query As String = "Select distinct handoverID, MV6, ISNULL(MV6FFF, 0) As ""MV6FFF"", MV10 ,ISNULL(MV10FFF, 0) As ""MV10FFF"",ISNULL(MeV4,0) As ""MeV4"", MeV6, MeV8, " &
                          "MeV10, MeV12, MeV15, MeV18, MeV20, iView, XVI from HandoverEnergies e  left outer join clinicalhandover r On e.handoverid=r.ehandid where r.CHandID  = (Select max(CHandID) As lastrecord from ClinicalHandover where linac=@linac)"

            SqlDataSource1 = QuerySqlConnection(LinacName, query)
            GridView2.DataSource = SqlDataSource1
            GridView2.DataBind()

            GridView2.Columns(13).Visible = False
            Select Case LinacName
                Case "LA1"
                    For index As Integer = 1 To 4
                        GridView2.Columns(index).Visible = False
                    Next

                Case "LA2", "LA3"
                    For index As Integer = 1 To 4
                        GridView2.Columns(index).Visible = False
                    Next
                    GridView2.Columns(2).Visible = True
                Case "LA4"

                    For index As Integer = 1 To 11
                        GridView2.Columns(index).Visible = False
                    Next
                    GridView2.Columns(2).Visible = True
                    GridView2.Columns(13).Visible = True
                Case "E1", "E2", "B1"
                    GridView2.Columns(13).Visible = True
                    For index As Integer = 10 To 11
                        GridView2.Columns(index).Visible = False
                    Next
                Case Else
                    'All columns are valid and are displayed

            End Select
        Else
            GridView2.Visible = False
        End If

    End Sub
    Private Sub SetButtonText()

        Dim treatment As String = Application(treatmentstate)
        Select Case treatment
            Case "No"
                Tstart.Text = "Stop Treatment"
                Application(treatmentstate) = "No"

            Case "Yes"
                Tstart.Text = "Start Treatment"
                Application(treatmentstate) = "Yes"

            Case Else
                Tstart.Text = "Start Treatment"
                Application(treatmentstate) = "Yes"
        End Select

    End Sub
    Protected Function QuerySqlConnection(ByVal MachineName As String, ByVal query As String) As SqlDataSource
        'This uses the sqldatasource instead of the individual conn definitions so reader can't be used
        'this solution is from http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.sqldatasource.select%28v=vs.90%29.aspx

        Dim SqlDataSource1 As New SqlDataSource With {
            .ID = "SqlDataSource1",
            .ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString,
            .SelectCommand = query
        }
        SqlDataSource1.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        SqlDataSource1.SelectParameters.Add("linac", MachineName)
        Return SqlDataSource1


    End Function

    Protected Sub LinacHandover_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LogOffButton.Click
        'This hands over linac to enable repair, planned maintenance or Physics QA to take place
        'But it needs to allow existing engineering and pre-clinical run up to be retained
        'It needs a log out as well+
        'WriteClinicalTable()
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc2"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
        wcbutton.Text = "Log off Linac"
        Application(actionstate) = "Confirm"
        wctrl.Visible = True
        ForceFocus(wctext)

    End Sub
    Private Sub ForceFocus(ByVal ctrl As Control)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "FocusScript", "setTimeout(Function(){$Get('" + ctrl.ClientID + "').focus();}, 100);", True)
    End Sub
    Protected Sub SaveText_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveText.Click
        Dim statusid As Integer
        Try
            Using myscope As TransactionScope = New TransactionScope()
                Dim connectionString1 As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
                statusid = DavesCode.NewCommitClinical.NewWriteClinicalTable(LinacName, HttpContext.Current.Application(BoxChanged), connectionString1)
                BindComments()
                myscope.Complete()
            End Using

            HiddenFieldLinacState.Value = statusid
            'GridViewComments.DataBind()
        Catch ex As Exception
            DavesCode.NewFaultHandling.LogError(ex, Application(BoxChanged))
            RaiseWriteError()
        End Try
        CommentBox.ResetCommentBox(String.Empty)
    End Sub

    Protected Sub Tstart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Tstart.Click
        Dim linacstatusid As String
        'http://www.javascripter.net/faq/hextorgb.htm to convert from hex to argb
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString

        If Not StateLabel Is Nothing Then
            linacstatusid = HiddenFieldLinacState.Value
            Dim tval As String = Application(treatmentstate)
            Try
                Using myscope As TransactionScope = New TransactionScope()
                    If tval = "Yes" Then

                        DavesCode.NewCommitClinical.SetTreatment("Treating", LinacName, linacstatusid, connectionString)
                        Tstart.Text = "Stop Treatment"
                        Tstart.BackColor = colourstop
                        Application(treatmentstate) = "No"
                        LogOffButton.Visible = True
                        ActivityLabel.Text = "Clinical - Treating"
                        BindEnergyData()
                    Else

                        DavesCode.NewCommitClinical.SetTreatment("Not Treating", LinacName, linacstatusid, connectionString)
                        Tstart.Text = "Start Treatment"
                        Tstart.BackColor = colourstart
                        Application(treatmentstate) = "Yes"
                        LogOffButton.Visible = True
                        ActivityLabel.Text = "Clinical - Not Treating"
                    End If

                    myscope.Complete()
                End Using
                StateLabel.Text = "Clinical"
            Catch ex As Exception
                DavesCode.NewFaultHandling.LogError(ex)
                RaiseStartError()
            End Try
        End If

    End Sub
    Protected Sub RaiseLogOffError()
        Dim machinelabel As String = LinacName & "Page.aspx';"
        Dim strScript As String = "<script>"
        strScript += "alert('Problem Logging Off. Please inform Engineering');"
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(Tstart, Me.GetType(), "JSCR", strScript.ToString(), False)
    End Sub
    Protected Sub RaiseStartError()
        Dim machinelabel As String = LinacName & "Page.aspx';"
        Dim strScript As String = "<script>"
        strScript += "alert('Problem setting Start Treatment. Please inform Engineering');"
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(Tstart, Me.GetType(), "JSCR", strScript.ToString(), False)
    End Sub
    Protected Sub RaiseLoadError()
        Dim machinelabel As String = LinacName & "Page.aspx';"
        Dim strScript As String = "<script>"
        strScript += "alert('Problem Logging On. Please call Administrator');"
        strScript += "window.location='"
        strScript += machinelabel
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(Tstart, Me.GetType(), "JSCR", strScript.ToString(), False)
    End Sub

    Protected Sub RaiseWriteError()
        Dim machinelabel As String = LinacName & "Page.aspx';"
        Dim strScript As String = "<script>"
        strScript += "alert('Problem Writing Comment. Please call Administrator');"
        'strScript += "window.location='"
        'strScript += machinelabel
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(Tstart, Me.GetType(), "JSCR", strScript.ToString(), False)
    End Sub

    'This happens on log on to create the comments table. Create TreatmentTable as well

    'Protected Function NewWriteClinicalTable(ByVal LinacName As String, ByVal connectionString As String) As Integer
    '    Dim time As DateTime
    '    time = Now()
    '    Dim reader As SqlDataReader
    '    Dim CID As Integer
    '    Dim CTID As Integer
    '    'Dim ClinD As Integer
    '    Dim StatusID As Integer = 0
    '    Dim UserName As String = String.Empty
    '    Dim conn As SqlConnection
    '    Dim Clinicalcomment As String = String.Empty
    '    Dim commCAuthID As SqlCommand
    '    Dim commclin As SqlCommand

    '    Clinicalcomment = Application(BoxChanged)
    '    conn = New SqlConnection(connectionString)

    '    commCAuthID = New SqlCommand("Select CHandID, LogOutStatusID from ClinicalHandover where CHandID  = (Select max(CHandID) as lastrecord from ClinicalHandover where linac=@linac)", conn)
    '    commCAuthID.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
    '    commCAuthID.Parameters("@linac").Value = LinacName
    '    conn.Open()
    '    CID = 0
    '    reader = commCAuthID.ExecuteReader()
    '    If reader.Read() Then
    '        CID = reader.Item("CHandID")
    '        CTID = reader.Item("LogOutStatusID")
    '        reader.Close()
    '        conn.Close()
    '    End If
    '    If Not CID = 0 Then
    '        'Remove user name as this is not the correct name anyway
    '        commCAuthID = New SqlCommand("Select StateID from LinacStatus where StateID  = (Select max(StateID) as lastrecord from LinacStatus where linac=@linac)", conn)
    '        commCAuthID.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
    '        commCAuthID.Parameters("@linac").Value = LinacName
    '        conn.Open()
    '        reader = commCAuthID.ExecuteReader()
    '        If reader.Read() Then
    '            StatusID = reader.Item("StateID")

    '            reader.Close()
    '            conn.Close()
    '        End If

    '        commclin = New SqlCommand("INSERT INTO ClinicalTable (PreClinID,LinacStatusID, clinComment,linac, DateTime, Username) " &
    '                            "VALUES ( @PreclinID, @LinacStatusID,@clincomment, @linac, @DateTime, @UserName)", conn)

    '        commclin.Parameters.Add("@PreClinID", System.Data.SqlDbType.Int)
    '        commclin.Parameters("@PreclinID").Value = CID
    '        commclin.Parameters.Add("@LinacStatusID", System.Data.SqlDbType.Int)
    '        commclin.Parameters("@LinacStatusID").Value = StatusID
    '        commclin.Parameters.Add("@clinComment", System.Data.SqlDbType.NVarChar, 250)
    '        commclin.Parameters("@clinComment").Value = Clinicalcomment
    '        commclin.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
    '        commclin.Parameters("@linac").Value = LinacName
    '        commclin.Parameters.Add("@DateTime", System.Data.SqlDbType.DateTime)
    '        commclin.Parameters("@DateTime").Value = time
    '        commclin.Parameters.Add("@UserName", System.Data.SqlDbType.NVarChar, 25)
    '        commclin.Parameters("@UserName").Value = String.Empty


    '        conn.Open()
    '        commclin.ExecuteNonQuery()

    '        conn.Close()



    '    End If

    '    Return StatusID

    '    'from http://msdn.microsoft.com/en-us/library/system.string.isnullorempty(v=vs.110).aspx

    'End Function


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
                Dim LogOff As Button = FindControl("LogOffButton")
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

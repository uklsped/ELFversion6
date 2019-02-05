'this disappears
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports DavesCode

Partial Class ViewOpenFaults

    Inherits UserControl

    Private FaultStatus As String
    Private ClinicalButton As Button
    Private suspstate As String
    Private failstate As String
    Private actionstate As String
    Private repairstate As String
    Private laststate As String
    Private faultviewstate As String
    Private technicalstate As String
    Private faultstate As String
    Private RadRow As DataTable
    Const REPEATFAULTSELECTED As String = "REPEAT"
    Const CONCESSIONSELECTED As String = "CSelected"
    Const VIEWSTATEKEY_DYNCONTROL As String = "DynamicControlSelection"
    Const EMPTYSTRING As String = ""
    Const CONCESSION As String = "Concession"
    Const CLOSED As String = "Closed"
    Const OPEN As String = "Open"

    Private ConcessionDescriptionChanged As String
    Private ConcessionActionChanged As String
    Private ConcessionCommentChanged As String
    Private ParamApplication As String
    Private ConcessParamsTrial As ConcessionParameters = New ConcessionParameters()
    Private FaultApplication As String
    Private FaultParams As FaultParameters = New FaultParameters()



    'from  
    Private Property DynamicControlSelection() As String
        Get
            Dim result As String = ViewState.Item(VIEWSTATEKEY_DYNCONTROL)
            If result Is Nothing Then
                'doing things like this lets us access this property without
                'worrying about this property returning null/Nothing
                Return String.Empty
            Else
                Return result
            End If
        End Get
        Set(ByVal value As String)
            ViewState.Item(VIEWSTATEKEY_DYNCONTROL) = value
        End Set
    End Property


    Public Event UpdateFaultClosedDisplays(ByVal EquipmentName As String, ByVal incidentID As String)
    Public Event UpDateDefectDailyDisplay(ByVal EquipmentName As String)
    Public Event AddConcessionToDefectDropDownList(ByVal EquipmentName As String, ByVal incidentID As String)
    Public Event ResetViewOpenFaults(ByVal EquipmentName As String)
    Public Property ParentControl() As String
    Public Property LinacName() As String
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init

        AddHandler ManyFaultGriduc.ShowFault, AddressOf NewFaultEvent
        AddHandler FaultTrackinguc1.CloseFaultTracking, AddressOf CloseTracking
        AddHandler FaultTrackinguc1.UpdateClosedDisplays, AddressOf CloseDisplays
        'AddHandler DeviceRepeatFaultuc1.CloseRepeatFault, AddressOf CloseTracking
        AddHandler DeviceRepeatFaultuc1.UpdateRepeatFault, AddressOf CloseRepeatFault

        suspstate = "Suspended" + LinacName
        failstate = "FailState" + LinacName
        actionstate = "ActionState" + LinacName
        repairstate = "rppTab" + LinacName
        laststate = "previousstate" + LinacName
        faultviewstate = "Faultsee" + LinacName
        technicalstate = "techstate" + LinacName
        faultstate = "OpenFault" + LinacName
        ConcessionDescriptionChanged = "ConcessionDescription" + LinacName
        ConcessionActionChanged = "ConcessionAction" + LinacName
        ConcessionCommentChanged = "ConcessionComment" + LinacName
        ParamApplication = "Params" + LinacName
        FaultApplication = "FaultParams" + LinacName
        'Application(techstate) = False


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load


        Select Case Me.DynamicControlSelection
            Case REPEATFAULTSELECTED
                LoadRepeatFaultTable(Label2.Text, Label5.Text)
                'Case CONCESSIONSELECTED
                'LoadFaultTable(Label2.Text)
                'ReloadTracking("1771")
            Case Else
                'no dynamic controls need to be loaded...yet
        End Select


        RadRow = HighlightRow()
        bindGridView()

        Dim loadIncidentNumber As Integer
        technicalstate = "techstate" + LinacName
        Dim previousstate As String
        Dim SqlDataSource2 As New SqlDataSource()

        'Dim loadfaultNumber As Integer = 0 ' this makes sure that if there isn't a new fault loadfaultnumber is zero
        If ParentControl = "5" Then
            loadIncidentNumber = 0
            Dim conn As SqlConnection
            Dim comm As SqlCommand
            Dim reader As SqlDataReader
            Dim connectionString1 As String = ConfigurationManager.ConnectionStrings(name:="connectionstring").ConnectionString
            conn = New SqlConnection(connectionString1)

            comm = New SqlCommand("select IncidentID from FaultIDTable where linac=@linac and Status in ('New', 'Open')", conn)

            comm.Parameters.AddWithValue(parameterName:="@linac", value:=LinacName)
            Try
                conn.Open()
                reader = comm.ExecuteReader() 'checks to see if there is a new fault - returns true or false if record read
                If reader.HasRows Then

                    Dim Manyfaultgrid As ManyFaultGriduc = CType(FindControl("ManyFaultGriduc"), ManyFaultGriduc)
                    Manyfaultgrid.MachineName = LinacName
                    Manyfaultgrid.NewFault = True

                    'This is to get the page that the fault was reported from - possibly a better way?
                    Application(laststate) = DavesCode.Reuse.GetLastState(LinacName, -1)
                    previousstate = Application(laststate)
                Else 'If there isn't a new fault then populate the grid with all of the open faults
                    'This could have come from a valid page or opened up again because fault wasn't closed. In that case
                    'The option on the parent page would still be to have LOOF
                    'This wouldn't be altered until concession or closed event
                    'bindGridView()

                End If
            Finally
                reader.Close()
                conn.Close()

            End Try
        End If

    End Sub


    '    'from http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.buttonfield.commandname%28v=vs.110%29.aspx?cs-save-lang=1&cs-lang=vb#code-snippet-2

    Protected Sub NewFaultEvent(ByVal incident As String) 'Triggers if new fault and is selected via Manyfaultgriduc1
        Dim IncidentID As String = incident
        Dim success As Boolean = False
        success = ConcessParamsTrial.CreateObject(IncidentID)

        If success Then
            Application(ParamApplication) = ConcessParamsTrial
            Dim FaultTracking As controls_FaultTrackinguc = CType(FindControl("FaultTrackinguc1"), controls_FaultTrackinguc)
            FaultTracking.LinacName = LinacName
            FaultTracking.IncidentID = IncidentID
            FaultTracking.InitialiseFaultTracking(ConcessParamsTrial)
            statustechpanel.Visible = True
            MultiView2.SetActiveView(statustech)
            MultiView1.SetActiveView(View1)
            Dim Manyfaultgrid As ManyFaultGriduc = CType(FindControl("ManyFaultGriduc"), ManyFaultGriduc)
            Manyfaultgrid.IncidentID = IncidentID
            Manyfaultgrid.NewFault = True
            Manyfaultgrid.MachineName = LinacName
            Application(technicalstate) = True
            'there are now 2 types of machine 28/06/18

            Manyfaultgrid.BindGridViewManyFault()
            ConcessionGrid.Columns(5).Visible = False
            ConcessionGrid.Columns(6).Visible = False
            ConcessionGrid.Columns(7).Visible = False
        Else
            RaiseError()
        End If

    End Sub

    Private Sub TabTech()
        Dim lastusergroup As Integer
        Dim lastuser As String = ""
        Dim NumOpen As Integer
        Dim Repairlist As RadioButtonList
        Dim StateTextBox As TextBox
        Dim comm As SqlCommand
        Dim reader As SqlDataReader

        Dim conn As SqlConnection
        Dim connectionString1 As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        conn = New SqlConnection(connectionString1)
        comm = New SqlCommand("select Count(*) as Numopen from FaultIDTable where Status in ('New','Open') and linac=@linac", conn)
        comm.Parameters.AddWithValue("@linac", LinacName)
        conn.Open()
        reader = comm.ExecuteReader()
        If Not Me.Parent.FindControl("RadioButtonlist1") Is Nothing And Not Me.Parent.FindControl("StateTextBox") Is Nothing Then
            Repairlist = Me.Parent.FindControl("RadioButtonlist1")
            StateTextBox = Me.Parent.FindControl("StateTextBox")
            If reader.Read() Then
                NumOpen = reader.Item("NumOpen")
                If NumOpen <> 0 Then 'there are open faults

                    ManyFaultGriduc.NewFault = True
                    ManyFaultGriduc.MachineName = LinacName
                    ManyFaultGriduc.BindGridViewManyFault()
                    ManyFaultGriduc.Visible = True
                    Application(technicalstate) = Nothing
                    Application(faultstate) = True
                Else
                    'But how does it know here to do this?
                    Dim lastState As String = ""
                    ConcessionGrid.Enabled = True
                    bindGridView()
                    Dim logbutton As Button
                    DavesCode.Reuse.GetLastTech(LinacName, 0, lastState, lastuser, lastusergroup)
                    lastState = DavesCode.Reuse.GetLastState(LinacName, 0)
                    Application(faultstate) = False
                    Repairlist.Items.FindByValue(1).Enabled = True
                    Repairlist.Items.FindByValue(4).Enabled = True

                    Repairlist.Items.FindByValue(8).Enabled = True
                    Repairlist.Items.FindByValue(102).Enabled = True

                    'This next if check if got here via clinical suspend
                    StateTextBox.Text = "Linac Unauthorised"

                    If Application(suspstate) = 1 Then
                        If LinacName Like "LA?" Then
                            Repairlist.Items.FindByValue(2).Enabled = True
                        End If
                        Repairlist.Items.FindByValue(3).Enabled = True
                        StateTextBox.Text = "Suspended"

                        Dim rtab As String = Application(repairstate)
                    ElseIf Application(repairstate) = 1 Then
                        If Application(failstate) = 3 Then
                            If LinacName Like "LA?" Then
                                Repairlist.Items.FindByValue(2).Enabled = True
                            End If
                            Repairlist.Items.FindByValue(3).Enabled = True
                            StateTextBox.Text = "Clinical - Not Treating"
                        Else
                            If LinacName Like "LA?" Then
                                Repairlist.Items.FindByValue(2).Enabled = True
                                StateTextBox.Text = "Engineering Approved"
                            Else
                                Repairlist.Items.FindByValue(3).Enabled = True
                                StateTextBox.Text = "Clinical - Not Treating"
                            End If
                        End If
                    Else
                        StateTextBox.Text = "Linac Unauthorised"
                    End If


                    If Not Me.Parent.FindControl("LogOffButton") Is Nothing Then
                        logbutton = Me.Parent.FindControl("LogOffButton")
                        logbutton.Enabled = False
                    End If
                End If
            End If
        End If
        ConcessionGrid.Enabled = True
        bindGridView()

    End Sub

    'Protected Sub CreateConcessParams(ByVal UserInfo As String)
    '    ConcessParamsTrial = Application(ParamApplication)
    '    Dim ConcessionDescription As String
    '    Dim ConcessionAction As String
    '    Dim ConcessionComment As String
    '    Dim FaultState As String
    '    Dim ConcessionNumber As String
    '    Dim AssignedTo As String = String.Empty
    '    If (Not HttpContext.Current.Application(ConcessionDescriptionChanged) Is Nothing) Then
    '        ConcessionDescription = HttpContext.Current.Application(ConcessionDescriptionChanged).ToString
    '    Else
    '        ConcessionDescription = String.Empty
    '    End If

    '    If (Not HttpContext.Current.Application(ConcessionActionChanged) Is Nothing) Then
    '        ConcessionAction = HttpContext.Current.Application(ConcessionActionChanged).ToString
    '    Else
    '        ConcessionAction = String.Empty
    '    End If

    '    If (Not HttpContext.Current.Application(ConcessionCommentChanged) Is Nothing) Then
    '        ConcessionComment = HttpContext.Current.Application(ConcessionCommentChanged).ToString
    '    Else
    '        ConcessionComment = String.Empty
    '    End If

    '    If AssignedTo = "Select" Then
    '        AssignedTo = "Unassigned"
    '    End If

    '    FaultState = "Concession"
    '    ConcessionNumber = "ELF000"
    '    ConcessParamsTrial.UserInfo = UserInfo
    '    ConcessParamsTrial.AssignedTo = AssignedTo
    '    ConcessParamsTrial.ConcessionAction = ConcessionAction

    '    ConcessParamsTrial.ConcessionComment = ConcessionComment
    '    ConcessParamsTrial.ConcessionDescription = ConcessionDescription

    'End Sub

    Protected Sub CloseDisplays(ByVal Linac As String, ByVal IncidentID As String)
        RaiseEvent UpdateFaultClosedDisplays(Linac, IncidentID)
        If ParentControl = "5" Then
            TabTech()
        End If

    End Sub
    Protected Sub CloseRepeatFault(ByVal Linac As String)

        ConcessionGrid.Enabled = True
        RaiseEvent UpDateDefectDailyDisplay(LinacName)
        bindGridView()
        UpdatePanelRepeatFault.Visible = False
        UpdatePanel4.Visible = True
        Me.DynamicControlSelection = String.Empty
    End Sub
    Protected Sub CloseTracking(ByVal Linac As String)

        If Linac = LinacName Then
            ConcessionGrid.Enabled = True
            bindGridView()
            statustechpanel.Visible = False
            UpdatePanel4.Visible = True

            ManyFaultGriduc.Visible = True
            Application(technicalstate) = Nothing
            ManyFaultGriduc.BindGridViewManyFault()
            SetViewFault(True)
            ConcessionGrid.Columns(5).Visible = True
            ConcessionGrid.Columns(6).Visible = True
            ConcessionGrid.Columns(7).Visible = True

        End If
    End Sub

    Protected Sub ConcessionGrid_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles ConcessionGrid.PageIndexChanging

        ConcessionGrid.PageIndex = e.NewPageIndex
        bindGridView()

    End Sub

    Sub FaultGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        'this can be changed to pass concession number to report repeat fault? 02/11/18
        Dim success As Boolean = False

        If Not e.CommandName = "Page" Then
            Dim IncidentID As String
            ' Convert the row index stored in the CommandArgument
            ' property to an Integer.
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            ' Retrieve the row that contains the button clicked 
            ' by the user from the Rows collection.
            Dim row As GridViewRow = ConcessionGrid.Rows(index)
            IncidentID = Server.HtmlDecode(row.Cells(0).Text)

            'This is where the concession number is found out
            Dim Concession As String = Server.HtmlDecode(row.Cells(1).Text)
            Dim Region As String
            Region = ""
            SetViewFault(False)
            ' If multiple buttons are used in a GridView control, use the
            ' CommandName property to determine which button was clicked.
            Select Case e.CommandName
                Case "View"
                    Select Case ParentControl
                        Case "1", "4", "5"
                            success = ConcessParamsTrial.CreateObject(IncidentID)
                            'success = DavesCode.ConcessionParameters.CreateObject(IncidentID, LinacName)
                            If success Then
                                Application(ParamApplication) = ConcessParamsTrial
                                statustechpanel.Visible = True
                                MultiView2.SetActiveView(statustech)
                                MultiView1.SetActiveView(View1)
                                ConcessionGrid.Enabled = False
                                UpdatePanel4.Visible = False
                                DynamicControlSelection = CONCESSIONSELECTED
                                Dim FaultTracking As controls_FaultTrackinguc = CType(FindControl("FaultTrackinguc1"), controls_FaultTrackinguc)
                                FaultTracking.LinacName = LinacName
                                FaultTracking.IncidentID = IncidentID
                                FaultTracking.InitialiseFaultTracking(ConcessParamsTrial)
                            Else
                                RaiseError()
                            End If

                        Case Else
                            BindTrackingGrid(IncidentID)
                            Hidefaults.Visible = True
                            UpdatePanel1.Visible = True
                            MultiView2.SetActiveView(statusother)
                            MultiView1.SetActiveView(View1)
                            ConcessionGrid.Enabled = False
                            UpdatePanel4.Visible = False
                    End Select

                Case "Log Fault"
                    success = FaultParams.CreateObject(IncidentID)
                    If success Then
                        Application(FaultApplication) = FaultParams
                        UpdatePanelRepeatFault.Visible = True
                        MultiView1.SetActiveView(UpdatefaultView)
                        ConcessionGrid.Enabled = False
                        Dim RepeatFault As Controls_DeviceRepeatFaultuc = CType(FindControl("DeviceRepeatFaultuc1"), Controls_DeviceRepeatFaultuc)
                        RepeatFault.LinacName = LinacName
                        RepeatFault.IncidentID = IncidentID
                        RepeatFault.InitialiseRepeatFault(FaultParams)
                        Label2.Text = IncidentID
                        Label5.Text = Concession
                        UpdatePanel4.Visible = False
                    Else
                        RaiseError()
                        CloseRepeatFault(LinacName)
                    End If

                    ''from http://www.sqlservercentral.com/Forums/Topic1416029-1292-1.aspx

                Case "Faults"
                    Dim objCon As UserControl = Page.LoadControl("ManyFaultGriduc.ascx")
                    CType(objCon, ManyFaultGriduc).NewFault = False
                    CType(objCon, ManyFaultGriduc).IncidentID = IncidentID
                    'to accomodate Tomo now need to pass equipment name?
                    CType(objCon, ManyFaultGriduc).MachineName = LinacName
                    PlaceHolder1.Controls.Add(objCon)
                    UpdatePanel2.Visible = True
                    MultiView1.SetActiveView(View2)
                    ConcessionGrid.Enabled = False
                    Hidefaults.Visible = True
                    UpdatePanel4.Visible = False
            End Select

        End If

    End Sub

    Protected Sub FormError()
        Dim strScript As String = "<script>"
        strScript += "alert('Please Correct Form Errors');"
        strScript += "</script>"

    End Sub
    Function CheckUniqueConcession(ByVal ConcessionNumber As String) As Boolean
        Dim NewNumber As String = ConcessionNumber
        Dim ncount As Integer = 1
        Dim unique As Boolean = False
        Dim conn As SqlConnection
        Dim comm As SqlCommand
        Dim reader1 As SqlDataReader
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        conn = New SqlConnection(connectionString)
        comm = New SqlCommand("select Count(ConcessionNumber) as 'Count' from ConcessionTable where linac=@linac and ConcessionNumber = @ConcessionNumber", conn)
        comm.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        comm.Parameters("@linac").Value = LinacName
        comm.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar)
        comm.Parameters("@ConcessionNumber").Value = NewNumber

        Try
            conn.Open()
            reader1 = comm.ExecuteReader()
            If reader1.Read() Then
                ncount = reader1.Item("Count")
                reader1.Close()
            End If
        Finally
            conn.Close()
        End Try
        If ncount = 0 Then
            unique = True
        End If
        Return unique
    End Function

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles ConcessionGrid.PageIndexChanging
        ConcessionGrid.PageIndex = e.NewPageIndex
        ConcessionGrid.DataBind()
    End Sub
    Sub bindGridView()
        Dim SqlDataSource2 As New SqlDataSource With {
            .ID = "SqlDataSource2",
            .ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString,
            .SelectCommand = "select distinct f.incidentID,  f.Dateinserted, c.ConcessionDescription, c.ConcessionNumber, c.Action, f.linac " _
        & "from FaultIDTable f left outer join ConcessionTable c on c.ConcessionNumber = f.ConcessionNumber where f.linac=@linac and f.Status in('Concession') order by incidentid desc"
        }
        'Open was added to f.status allow use with singlemachinefaultuc it will only be appropriate for the repair page. TAken out on 5/8/15

        'SqlDataSource2.SelectCommand = "select r.FaultID, r.Description, r.ReportedBy, r.DateReported, r.FaultStatus, t.ConcessionNumber, r.linac " & _
        '   "from reportfault r left outer join Faulttracking t on r.FaultID=t.FaultID where r.linac=@linac and r.faultstatus=t.status and FaultStatus in('Concession') order by faultid desc"
        'SqlDataSource2.SelectCommand = "select * from reportfault where linac=@linac and FaultStatus in('Concession')"
        SqlDataSource2.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        SqlDataSource2.SelectParameters.Add("linac", LinacName)
        ConcessionGrid.DataSource = SqlDataSource2
        ConcessionGrid.DataBind()
        CheckEmptyGrid(ConcessionGrid)

    End Sub
    Protected Sub BindTrackingGrid(ByVal incidentID As String)
        Dim incidentNumber As String = incidentID
        Dim SqlDataSource3 As New SqlDataSource With {
            .ID = "SqlDataSource3",
            .ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString,
            .SelectCommand = "select t.TrackingID, t.trackingcomment, t.AssignedTo, t.Status, t.LastUpDatedBy, t.LastUpDatedOn, c.ConcessionNumber, t.linac, t.incidentID " _
& "from FaultTracking t left outer join ConcessionTable c on c.incidentID=t.incidentID where t.linac=@linac and t.incidentID=@incidentID order by trackingid asc"
        }
        SqlDataSource3.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        SqlDataSource3.SelectParameters.Add("linac", LinacName)
        SqlDataSource3.SelectParameters.Add("@incidentID", System.Data.SqlDbType.Int)
        SqlDataSource3.SelectParameters.Add("incidentID", incidentNumber)
        GridView2.DataSource = SqlDataSource3
        GridView2.DataBind()
    End Sub
    Protected Sub BindTrackingGridTech(ByVal incidentID As String)
        Dim SqlDataSource3 As New SqlDataSource With {
            .ID = "SqlDataSource3",
            .ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString,
            .SelectCommand = "select t.TrackingID, t.trackingcomment, t.AssignedTo, t.Status, t.LastUpDatedBy, t.LastUpDatedOn, t.linac, t.action " _
        & "from FaultTracking t  where t.linac=@linac and t.incidentID=@incidentID order by t.TrackingID desc"
        }
        SqlDataSource3.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        SqlDataSource3.SelectParameters.Add("linac", LinacName)
        SqlDataSource3.SelectParameters.Add("@incidentID", System.Data.SqlDbType.Int)
        SqlDataSource3.SelectParameters.Add("incidentID", incidentID)

    End Sub

    Protected Sub Hidefaults_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Hidefaults.Click
        ConcessionGrid.Visible = True
        ConcessionGrid.Enabled = True
        ConcessionGrid.SelectedIndex = -1
        UpdatePanel2.Visible = False
        Hidefaults.Visible = False
        UpdatePanel4.Visible = True
        SetViewFault(True)
    End Sub
    'delete 3/7/18

    '    'from http://www.aspsnippets.com/Articles/Programmatically-add-items-to-DropDownList-on-Button-Click-in-ASPNet-using-C-and-VBNet.aspx
    '    'and http://www.yaldex.com/asp_tutorial/0596004877_progaspdotnet2-chp-5-sect-7.html

    Protected Sub Cancel()

        ConcessionGrid.Enabled = True
        UpdatePanelRepeatFault.Visible = False
        UpdatePanel4.Visible = True
        SetViewFault(True)
        Me.DynamicControlSelection = String.Empty
        Page_Load(Page, EventArgs.Empty)
    End Sub

    'Protected Sub ViewExistingFaults_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ViewExistingFaults.Click
    '    Dim incidentID As String
    '    incidentID = Label2.Text
    '    Dim objCon As UserControl = Page.LoadControl("ManyFaultGriduc.ascx")
    '    CType(objCon, ManyFaultGriduc).NewFault = False
    '    CType(objCon, ManyFaultGriduc).IncidentID = incidentID
    '    'to accomodate Tomo now need to pass equipment name?
    '    CType(objCon, ManyFaultGriduc).MachineName = LinacName
    '    PlaceHolder3.Controls.Add(objCon)

    'End Sub

    Protected Sub LoadRepeatFaultTable(ByVal incidentid As String, ByVal concessionnumber As String)
        Dim objcon As Object
        objcon = Page.LoadControl("~/controls/DeviceRepeatFaultuc.ascx")
        CType(objcon, Controls_DeviceRepeatFaultuc).IncidentID = incidentid
        CType(objcon, Controls_DeviceRepeatFaultuc).LinacName = LinacName
        CType(objcon, Controls_DeviceRepeatFaultuc).ConcessionN = concessionnumber
        AddHandler CType(objcon, Controls_DeviceRepeatFaultuc).UpdateRepeatFault, AddressOf CloseRepeatFault
        PlaceholderRepeatFault.Controls.Add(objcon)
        Me.DynamicControlSelection = REPEATFAULTSELECTED

    End Sub

    Protected Sub CheckEmptyGrid(ByVal grid As GridView)
        'If grid.Rows.Count = 0 And Not grid.DataSource Is Nothing Then
        '    Dim dt As Object = Nothing
        '    If grid.DataSource.GetType Is GetType(Data.DataSet) Then
        '        dt = New System.Data.DataSet
        '        dt = CType(grid.DataSource, System.Data.DataSet).Tables(0).Clone()
        '    ElseIf grid.DataSource.GetType Is GetType(Data.DataTable) Then
        '        dt = New System.Data.DataTable
        '        dt = CType(grid.DataSource, System.Data.DataTable).Clone()
        '    ElseIf grid.DataSource.GetType Is GetType(Data.DataView) Then
        '        dt = New System.Data.DataView
        '        dt = CType(grid.DataSource, System.Data.DataView).Table.Clone
        '    Else
        '        grid.DataSource
        '        dt = New System.Data.DataTable
        '        gr()
        '        dt = CType(grid.DataSource, System.Data.DataTable).Clone()
        '    End If
        '    dt.Rows.Add(dt.NewRow())
        '    grid.DataSource = dt
        '    grid.DataBind()
        '    Dim columnsCount As Integer
        '    Dim tCell As New TableCell()
        '    columnsCount = grid.Columns.Count
        '    grid.Rows(0).Cells.Clear()
        '    grid.Rows(0).Cells.Add(tCell)
        '    grid.Rows(0).Cells(0).ColumnSpan = columnsCount


        '    grid.Rows(0).Cells(0).Text = "No Records To Display"
        '    grid.Rows(0).Cells(0).HorizontalAlign = HorizontalAlign.Center
        '    grid.Rows(0).Cells(0).ForeColor = Drawing.Color.Black
        '    grid.Rows(0).Cells(0).Font.Bold = True


        '    'grid.Rows(0).Visible = False

        'End If
    End Sub

    Public Sub SetViewFault(ByVal OnOff As Boolean)
        If Not Me.Parent.FindControl("FaultpanelButton") Is Nothing Then
            Dim Fbutton As Button = Me.Parent.FindControl("FaultPanelButton")
            Fbutton.Enabled = OnOff
        End If
    End Sub

    Private Sub ForceFocus(ByVal ctrl As Control)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "FocusScript", "setTimeout(function(){$get('" + ctrl.ClientID + "').focus();}, 100);", True)
    End Sub

    Public Sub RebindViewFault()
        RadRow = HighlightRow()
        bindGridView()

    End Sub

    Protected Sub ConcessionGrid_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles ConcessionGrid.RowDataBound
        Select Case ParentControl
            Case "1", "4", "5"
                Dim nincid As String = ""
                If e.Row.RowType = DataControlRowType.DataRow Then
                    Dim SelectRow As String = e.Row.Cells(0).Text.ToString
                    If RadRow.Rows.Count > 0 Then
                        For Each dataRow As DataRow In RadRow.Rows
                            nincid = dataRow("incidentID")
                            If SelectRow = nincid Then
                                e.Row.BackColor = Color.FromName("#E56E94")
                            End If
                        Next
                    End If
                End If
        End Select
    End Sub
    Protected Function HighlightRow() As DataTable
        Dim conn As SqlConnection
        Dim comm As SqlCommand
        Dim connectionString1 As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        conn = New SqlConnection(connectionString1)
        comm = New SqlCommand("SELECT r.IncidentID From RadAckFault r Left outer join concessiontable c on r.Incidentid = c.Incidentid where r.Acknowledge = 'false' and linac=@linac", conn)
        comm.Parameters.AddWithValue("@linac", LinacName)
        conn.Open()
        Dim da As New SqlDataAdapter(comm)
        Dim dt As New DataTable()
        da.Fill(dt)
        Return dt

    End Function

    Protected Sub RaiseError()
        Dim strScript As String = "<script>"
        strScript += "alert('Problem Updating Fault. Please call Engineer');"
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(Hidefaults, Me.GetType(), "JSCR", strScript.ToString(), False)
    End Sub

End Class

Imports System.Data.SqlClient
Imports System.Data
Partial Class Singlemachinefaultuc
    Inherits System.Web.UI.UserControl
    Private MachineName As String
    Private TabName As String
    Private FaultStatus As String
    Private ClinicalButton As Button
    Private LogoffButton As Button
    Private suspstate As String
    Private failstate As String
    Private actionstate As String
    Private laststate As String


    Public Property LinacName() As String
        Get
            Return MachineName
        End Get
        Set(ByVal value As String)
            MachineName = value
        End Set
    End Property
    Public Property Tabs() As String
        Get
            Return TabName
        End Get
        Set(ByVal value As String)
            TabName = value
        End Set
    End Property

    Public Property PassButton() As Button
        Get
            Return ClinicalButton
        End Get
        Set(ByVal value As Button)
            ClinicalButton = value
        End Set
    End Property
    Public Property LogoffPassButton() As Button
        Get
            Return LogoffButton
        End Get
        Set(ByVal value As Button)
            LogoffButton = value
        End Set
    End Property

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        AddHandler WriteDatauc1.UserApproved, AddressOf UserApprovedEvent
        suspstate = "Suspended" + MachineName
        failstate = "FailState" + MachineName
        actionstate = "ActionState" + MachineName
        laststate = "previousstate" + MachineName
    End Sub
    Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridView1.SelectedIndexChanged
        'This sub gets record that is selected, completes the fault information and hides the gridview with all the faults

        Dim FaultDescription As New List(Of String)
        Dim faultID As Integer
        Dim incidentID As Integer
        'ConcessionNumberBox.ReadOnly = True
        'ConcessiondescriptionBox.ReadOnly = True
        'faultID = CInt(GridView1.SelectedDataKey.Values("FaultID"))
        incidentID = CInt(GridView1.SelectedDataKey.Values("incidentID"))
        BindTrackingGrid()
        UpdatePanel1.Visible = True
        Label2.Text = incidentID
        Dim conn As SqlConnection
        Dim comm As SqlCommand
        Dim connectionString1 As String = ConfigurationManager.ConnectionStrings( _
        "connectionstring").ConnectionString
        conn = New SqlConnection(connectionString1)
        'comm = New SqlCommand("select FaultStatus  from reportfault where linac=@linac and FaultID = @FaultID", conn)
        'comm = New SqlCommand("select distinct r.FaultID, r.Description, r.ReportedBy, r.DateReported, r.FaultStatus, r.Area, r.Energy, r.GantryAngle, r.CollimatorAngle,t.ConcessionNumber, t.concessiondescription, r.linac, t.IncidentID " & _
        '    "from reportfault r left outer join faulttracking t on r.FaultID=t.FaultID left outer join faultidtable f on f.incidentid=t.incidentid where r.linac=@linac and f.status=t.status and r.FaultID = @FaultID", conn)
        comm = New SqlCommand("select distinct r.FaultID, r.Description, r.ReportedBy, r.DateReported, f.Status, r.Area, r.Energy, r.GantryAngle, r.CollimatorAngle, ISNULL(c.ConcessionNumber, '') as ConcessionNumber , ISNULL(c.concessiondescription, '') as ConcessionDescription, f.linac, f.IncidentID " & _
        "from reportfault r left outer join FaultIDTable f on f.OriginalFaultID = r.FaultID left outer join ConcessionTable c on f.ConcessionNumber=c.ConcessionNumber where f.linac=@linac and f.incidentID = @incidentID", conn)
        comm.Parameters.AddWithValue("@linac", MachineName)
        comm.Parameters.AddWithValue("@incidentID", incidentID)
        Try
            conn.Open()
            Dim da As New SqlDataAdapter(comm)
            Dim dt As New DataTable()
            Dim nfaultid As String
            Dim ndescription As String
            Dim nrep As String
            Dim ndate As String
            Dim nen As String
            Dim nga As String
            Dim nca As String
            Dim nlin As String
            Dim narea As String
            Dim nconc As String
            Dim ncond As String
            Dim nincid As String


            da.Fill(dt)
            If dt.Rows.Count > 0 Then

                For Each dataRow As DataRow In dt.Rows
                    nfaultid = dataRow("FaultId")
                    ndescription = dataRow("Description")
                    nrep = dataRow("ReportedBy")
                    ndate = dataRow("DateReported")
                    FaultStatus = dataRow("Status")
                    narea = dataRow("Area")
                    nen = dataRow("Energy")
                    nga = dataRow("GantryAngle")
                    nca = dataRow("CollimatorAngle")
                    nconc = dataRow("ConcessionNumber")
                    ncond = dataRow("ConcessionDescription")
                    nlin = dataRow("Linac")
                    nincid = dataRow("incidentID")

                Next
            End If

            DescriptionBox.Text = ndescription
            AreaBox.Text = narea
            EnergyBox.Text = nen
            GantryBox.Text = nga
            CollBox.Text = nca
            ReportedBox.Text = nrep
            OpenDateBox.Text = ndate
            StatusLabel1.Text = FaultStatus
            'If nconc = "NULL" Then
            '    nconc = ""
            '    ncond = ""
            'End If
            ConcessionNumberBox.Text = nconc
            ConcessiondescriptionBox.Text = ncond
            'ConcessionNumberBox.ReadOnly = True
            'ConcessiondescriptionBox.ReadOnly = True
            If FaultStatus = "Concession" Then
                'ConcessionNumberBox.ReadOnly = True
                'ConcessiondescriptionBox.ReadOnly = True
                DropDownList1.Items.FindByValue("Open").Enabled = False

                'Else
                '    ConcessionNumberBox.ReadOnly = False
                '    ConcessiondescriptionBox.ReadOnly = False
                '    'This physically removes the item so keeps doing that
                '    'DropDownList1.Items.RemoveAt(1)
            End If


        Finally
            conn.Close()

        End Try
        GridView1.Visible = False
        If TabName = "Clin" Then
            'Dim handoverbutton As Button
            'If Not Me.Parent.FindControl("clinHandoverButton") Is Nothing Then
            '    handoverbutton = Me.Parent.FindControl("clinHandoverButton")
            '    handoverbutton.Enabled = False
            ClinicalButton.Enabled = False
            LogoffButton.Enabled = False
            'End If
        End If
        Application("GridSet") = True
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        'Reloads grid with the next page
        GridView1.PageIndex = e.NewPageIndex
        BindOpenfault()
    End Sub
    Protected Sub UserApprovedEvent(ByVal Tabused As String, ByVal Userinfo As String)
        'it can't get to here unless a fault has been selected and set to a value
        'It's not possible to get here with the fault still being new because the only way to do that is to
        'cancel
        If Tabused = "Fault" Then 'This checks that this is the correct event handler
            'This stops it popping up again when it shouldn't
            Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
            wctrl.Visible = False
            GridView1.Visible = True
            Dim Action As String = Application("ActionState")
            If Action = "Confirm" Then
                Dim username As String = Userinfo
                Dim strScript As String = "<script>"
                Dim selecttext As String
                Dim assignuser As String
                Dim time As DateTime
                time = Now()
                Dim faultNumber As Integer
                Dim ConcessionNumber As String
                Dim ConcessionDescription As String
                Dim ConcessionActive As Integer
                Dim incidentID As Integer
                Dim FaultStatus As String
                Dim exists As Integer

                'This should probably happen at the end
                newfaultUpdatePanel1.Visible = False
                'ConcessiondescriptionBox.ReadOnly = True
                'ConcessionNumberBox.ReadOnly = True
                Dim conn As SqlConnection
                Dim connectionString1 As String = ConfigurationManager.ConnectionStrings( _
                "connectionstring").ConnectionString
                conn = New SqlConnection(connectionString1)
                Try
                    'check that this is successful what happens if it isn't
                    incidentID = CInt(GridView1.SelectedDataKey.Values("incidentID"))
                    selecttext = DropDownList1.SelectedItem.Text
                    ConcessionNumber = ConcessionNumberBox.Text
                    ConcessionDescription = ConcessiondescriptionBox.Text
                    assignuser = DropDownList2.SelectedItem.Text
                    FaultStatus = StatusLabel1.Text
                    If assignuser = "Select" Then
                        assignuser = "Unassigned"
                    End If
                    If selecttext = "Concession" Then
                        ConcessionActive = 1
                    Else
                        ConcessionActive = 0
                    End If

                    'should check that one of the fields has been altered?
                Finally
                    '    conn.Close()

                End Try
                If incidentID <> 0 Then
                    Dim commtrack As SqlCommand
                    commtrack = New SqlCommand("Insert into FaultTracking (Trackingcomment, AssignedTo, Status, LastupdatedBy, Lastupdatedon,   linac,  incidentID) " & _
                                               "VALUES (@Trackingcomment, @AssignedTo, @Status, @LastupdatedBy, @Lastupdatedon,  @linac,  @IncidentID)", conn)
                    commtrack.Parameters.Add("@Trackingcomment", System.Data.SqlDbType.NVarChar, 250)
                    commtrack.Parameters("@Trackingcomment").Value = CommentBox1.Text
                    commtrack.Parameters.Add("@AssignedTo", Data.SqlDbType.NVarChar, 50)
                    commtrack.Parameters("@AssignedTo").Value = assignuser
                    commtrack.Parameters.Add("@Status", Data.SqlDbType.NVarChar, 50)
                    commtrack.Parameters("@Status").Value = DropDownList1.SelectedItem.Text
                    commtrack.Parameters.Add("@LastupdatedBy", System.Data.SqlDbType.NVarChar, 50)
                    commtrack.Parameters("@LastupdatedBy").Value = Userinfo
                    commtrack.Parameters.Add("@Lastupdatedon", System.Data.SqlDbType.DateTime)
                    commtrack.Parameters("@Lastupdatedon").Value = time
                    'commtrack.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 10)
                    'commtrack.Parameters("@ConcessionNumber").Value = ConcessionNumber
                    'commtrack.Parameters.Add("@ConcessionDescription", System.Data.SqlDbType.NVarChar, 20)
                    'commtrack.Parameters("@ConcessionDescription").Value = ConcessionDescription
                    'commtrack.Parameters.Add("@FaultID", System.Data.SqlDbType.Int)
                    'commtrack.Parameters("@FaultID").Value = -1
                    commtrack.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
                    commtrack.Parameters("@linac").Value = MachineName
                    'commtrack.Parameters.Add("@ConcessionActive", System.Data.SqlDbType.TinyInt)
                    'commtrack.Parameters("@ConcessionActive").Value = ConcessionActive
                    commtrack.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    commtrack.Parameters("@incidentID").Value = incidentID
                    'This updates the original fault table as well so faultstatus is in two places as is concession number now


                    'Dim commfault As SqlCommand
                    'commfault = New SqlCommand("Update ReportFault SET FaultStatus=@FaultStatus WHERE FaultID=@FaultID", conn)
                    'commfault.Parameters.Add("@FaultStatus", Data.SqlDbType.NVarChar, 50)
                    'commfault.Parameters("@FaultStatus").Value = DropDownList1.SelectedItem.Text
                    ''commfault.Parameters.Add("@ConcessionNumber", Data.SqlDbType.NVarChar, 10)
                    ''commfault.Parameters("@ConcessionNumber").Value = ConcessionNumber
                    'commfault.Parameters.Add("@FaultID", System.Data.SqlDbType.Int)
                    'commfault.Parameters("@FaultID").Value = faultNumber
                    Dim incidentfault As SqlCommand
                    incidentfault = New SqlCommand("Update FaultIDTable SET Status=@Status WHERE IncidentID=@incidentID", conn)
                    incidentfault.Parameters.Add("@Status", Data.SqlDbType.NVarChar, 50)
                    incidentfault.Parameters("@Status").Value = DropDownList1.SelectedItem.Text
                    incidentfault.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    incidentfault.Parameters("@incidentID").Value = incidentID
                    'incidentfault.Parameters.Add("@ConcessionNumber", Data.SqlDbType.NVarChar, 10)
                    'incidentfault.Parameters("@ConcessionNumber").Value = ConcessionNumber
                    'incidentfault.Parameters.Add("@ConcessionActive", System.Data.SqlDbType.TinyInt)
                    'incidentfault.Parameters("@ConcessionActive").Value = ConcessionActive

                    Dim commconcess As SqlCommand
                    commconcess = New SqlCommand("Insert into ConcessionTable (ConcessionNumber, ConcessionDescription, IncidentID, Linac, ConcessionActive) " & _
                    "VALUES (@ConcessionNumber, @ConcessionDescription, @IncidentID, @Linac, @ConcessionActive) Update FaultIDTable SET ConcessionNumber=@ConcessionNumber WHERE IncidentID=@incidentID ", conn)
                    commconcess.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 10)
                    commconcess.Parameters("@ConcessionNumber").Value = ConcessionNumber
                    commconcess.Parameters.Add("@ConcessionDescription", System.Data.SqlDbType.NVarChar, 20)
                    commconcess.Parameters("@ConcessionDescription").Value = ConcessionDescription
                    commconcess.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    commconcess.Parameters("@incidentID").Value = incidentID
                    commconcess.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                    commconcess.Parameters("@Linac").Value = MachineName
                    commconcess.Parameters.Add("@ConcessionActive", System.Data.SqlDbType.Bit)
                    commconcess.Parameters("@ConcessionActive").Value = ConcessionActive
                    'Dim faultstatus As String
                    'faultstatus = DropDownList1.SelectedItem.Text
                    'If faultstatus = "Concession" Then
                    '    commconcess.Parameters("@ConcessionActive").Value = True
                    'Else
                    '    commconcess.Parameters("@ConcessionActive").Value = False
                    'End If
                    Dim bcommand = New SqlCommand("select count(*) from Concessiontable where incidentID=@incidentID", conn)
                    bcommand.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    bcommand.Parameters("@incidentID").Value = incidentID
                    Try
                        conn.Open()
                        commtrack.ExecuteNonQuery()

                        'commfault.ExecuteNonQuery()

                        If selecttext = "Concession" Then
                            exists = -1

                            exists = bcommand.ExecuteScalar()

                            If exists = 0 Then
                                commconcess.ExecuteNonQuery()
                            End If
                        End If

                        incidentfault.ExecuteNonQuery()
                        'conn.Close()
                        'conn.Open()
                        'commconcess.ExecuteNonQuery()
                    Finally
                        conn.Close()

                    End Try
                    'This for when there is a closed statement
                    If selecttext = "Closed" Then

                        '    commcause = New SqlCommand("Insert into CauseRemedy (FaultID, Cause, Remedy, Linac)" & _
                        '                               "Values (@FaultID, @Cause, @Remedy, @Linac)", conn)
                        incidentfault = New SqlCommand("Update FaultIDTable SET DateClosed=@DateClosed WHERE IncidentID=@incidentID", conn)
                        incidentfault.Parameters.Add("@DateClosed", System.Data.SqlDbType.DateTime)
                        incidentfault.Parameters("@DateClosed").Value = time
                        'incidentfault.Parameters("@Status").Value = DropDownList1.SelectedItem.Text
                        incidentfault.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                        incidentfault.Parameters("@incidentID").Value = incidentID
                        '    commcause.Parameters.Add("@FaultID", System.Data.SqlDbType.Int)
                        '    commcause.Parameters("@FaultID").Value = faultNumber
                        '    commcause.Parameters.Add("@Cause", System.Data.SqlDbType.NVarChar, 250)
                        '    commcause.Parameters("@Cause").Value = CauseTextBox.Text
                        '    commcause.Parameters.Add("@Remedy", Data.SqlDbType.NVarChar, 250)
                        '    commcause.Parameters("@remedy").Value = RemedyTextBox.Text
                        '    commcause.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
                        '    commcause.Parameters("@linac").Value = MachineName
                        Try
                            conn.Open()
                            incidentfault.ExecuteNonQuery()
                            conn.Close()
                        Finally
                            conn.Close()

                        End Try

                    End If
                    GridView1.Enabled = True
                    BindOpenfault()

                    'This is where what happens depends on which control it's on
                    'if we've got to here then reset buttons on master so that open fault is now not available or selected and relevant buttons are available

                    If TabName = "Tech" Then

                        Dim NumOpen As Integer
                        Dim Repairlist As RadioButtonList
                        Dim comm As SqlCommand
                        Dim reader As SqlDataReader
                        comm = New SqlCommand("select Count(*) as Numopen from FaultIDTable where Status in ('Open') and linac=@linac", conn)
                        comm.Parameters.AddWithValue("@linac", MachineName)

                        conn.Open()
                        reader = comm.ExecuteReader()
                        If Not Me.Parent.FindControl("RadioButtonlist1") Is Nothing Then
                            Repairlist = Me.Parent.FindControl("RadioButtonlist1")

                            If reader.Read() Then
                                NumOpen = reader.Item("NumOpen")
                                If NumOpen <> 0 Then 'there are open faults

                                    Repairlist.Items(4).Enabled = True
                                    Repairlist.Items(4).Selected = True
                                Else
                                    'But how does it know here to do this?
                                    Dim lastState As String
                                    Dim fState As String
                                    Repairlist.Items(0).Enabled = True
                                    Repairlist.Items(3).Enabled = True
                                    Repairlist.Items(4).Enabled = False
                                    Repairlist.Items(4).Selected = False
                                    fState = Application(failstate)
                                    Select Case fState
                                        Case 2
                                            Repairlist.Items(1).Enabled = True
                                        Case 3
                                            Repairlist.Items(1).Enabled = True
                                            Repairlist.Items(2).Enabled = True
                                        Case Else
                                            If Application(suspstate) = 1 Then
                                                Repairlist.Items(1).Enabled = True
                                                Repairlist.Items(2).Enabled = True
                                            End If
                                    End Select
                                    '18 April Moved from here because want to be able to remember where came from if fault recorded after faults closed Moved to repair page.
                                    'Application(failstate) = Nothing
                                End If
                            End If
                        End If
                    Else
                        'Dim handoverbutton As Button
                        'If Not Me.Parent.FindControl("clinHandoverButton") Is Nothing Then
                        '    handoverbutton = Me.Parent.FindControl("clinHandoverButton")
                        '    handoverbutton.Enabled = True
                        'End If
                        ClinicalButton.Enabled = True
                        LogoffButton.Enabled = True
                    End If

                Else
                    'Linac status now set by repair or other parent container

                    strScript += "alert('Please select a fault or cancel action');"
                    strScript += "</script>"
                    ScriptManager.RegisterStartupScript(SaveFault, Me.GetType(), "JSCR", strScript.ToString(), False)
                End If



            End If
        End If

        Application("GridSet") = False
    End Sub

    Protected Sub CancelButton_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        GridView1.Visible = True
        GridView1.SelectedIndex = -1
        ConcessiondescriptionBox.ReadOnly = True
        ConcessionNumberBox.ReadOnly = True
        UpdatePanel1.Visible = False
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        wctrl.Visible = False
        'Dim handoverbutton As Button
        'If Not Me.Parent.FindControl("clinHandoverButton") Is Nothing Then
        '    handoverbutton = Me.Parent.FindControl("clinHandoverButton")
        '    handoverbutton.Enabled = True
        'End If
        If TabName = "Clin" Then
            ClinicalButton.Enabled = True
            LogoffButton.Enabled = True
        End If
        Application("GridSet") = False
        Page_Load(Page, e)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'just select command and select column based on fault status

        If Not IsPostBack Then
            MultiView1.SetActiveView(DummyView)
        End If

        Dim faultNumber As Integer
        Dim loadIncidentNumber As Integer

        Dim previousstate As String
        Dim SqlDataSource2 As New SqlDataSource()

        'Dim loadfaultNumber As Integer = 0 ' this makes sure that if there isn't a new fault loadfaultnumber is zero
        loadIncidentNumber = 0
        Dim conn As SqlConnection
        Dim comm As SqlCommand
        Dim reader As SqlDataReader
        Dim connectionString1 As String = ConfigurationManager.ConnectionStrings( _
        "connectionstring").ConnectionString
        conn = New SqlConnection(connectionString1)
        'comm = New SqlCommand("select faultid from reportfault where linac=@linac and FaultStatus = 'New'", conn)
        comm = New SqlCommand("select IncidentID from FaultIDTable where linac=@linac and Status = 'New'", conn)

        comm.Parameters.AddWithValue("@linac", MachineName)
        Try
            conn.Open()
            reader = comm.ExecuteReader() 'checks to see if there is a new fault - returns true or false if record read
            If reader.Read() Then
                'loadfaultNumber = reader.Item("faultid")
                loadIncidentNumber = reader.Item("IncidentID")
                reader.Close()
                'comm = New SqlCommand("select distinct faultid from FaultTracking where linac=@linac and FaultStatus = 'New'", conn)
                'reader = comm.ExecuteReader()
                'If reader.Read() Then
                'loadfaultNumber = reader.Item("faultid")
                'reader.Close()
                'End If
            End If

            'reader.Close()
        Finally
            conn.Close()

        End Try
        If loadIncidentNumber <> 0 Then 'If there is a new fault display just the new fault. Must have come from fault page
            newfaultUpdatePanel1.Visible = True
            MultiView1.SetActiveView(View1)
            DropDownList1.SelectedIndex = -1
            DropDownList2.SelectedIndex = -1
            CommentBox1.Text = ""
            Dim SqlDataSource1 As New SqlDataSource()
            SqlDataSource1.ID = "SqlDataSource1"
            SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            'SqlDataSource1.SelectCommand = "select * from reportfault where faultid=(select max(faultid) as mancount from reportfault where linac=@linac)"
            'SqlDataSource1.SelectCommand = "select * from reportfault where faultid=@faultid"
            'concession number will be NULL at this point
            'SqlDataSource1.SelectCommand = "select distinct r.faultID, r.Description, r.ReportedBy, r.DateReported, r.FaultStatus, t.ConcessionNumber, r.linac, t.incidentID " & _
            '"from reportfault r left outer join faulttracking t on r.FaultID=t.FaultID where t.incidentID=@incidentID"

            SqlDataSource1.SelectCommand = "select distinct r.faultID, r.Description, r.ReportedBy, r.DateReported, f.Status, f.ConcessionNumber, f.linac, f.incidentID " & _
            "from reportfault r left outer join FaultIDTable f on r.incidentID=f.incidentID where f.incidentID=@incidentID"
            SqlDataSource1.SelectParameters.Add("incidentID", loadIncidentNumber)
            'SqlDataSource1.SelectCommand = "select distinct r.FaultID, r.Description, r.ReportedBy, r.DateReported, r.FaultStatus, t.ConcessionNumber, r.linac " & _
            '"from reportfault r left outer join faulttracking t on r.FaultID=t.FaultID where r.faultid=@faultid and r.faultstatus=t.status"
            'SqlDataSource1.SelectParameters.Add("faultid", loadfaultNumber)


            'SqlDataSource1.SelectParameters.Add("linac", MachineName)
            GridView1.DataSource = SqlDataSource1
            GridView1.DataBind()
            GridView1.Columns(8).Visible = True 'This will have to go back to 10 if columns are added back
            'newfaultUpdatePanel1.Visible = True
            'Viewopenfaultsbutton.Visible = False
            ' Set Repair page - this will never be exposed for other parent pages
            Dim Repairlist As RadioButtonList 'Select log off with open fault if the page is closed with a new fault
            If Not Me.Parent.FindControl("RadioButtonlist1") Is Nothing Then
                Repairlist = Me.Parent.FindControl("RadioButtonlist1")
                'Repairlist.Items(0).Enabled = false
                'Repairlist.Items(3).Enabled = False
                Repairlist.Items(4).Enabled = True
                Repairlist.Items(4).Selected = True
            End If
            'This is to get the page that the fault was reported from - possibly a better way?
            Application("laststate") = DavesCode.Reuse.GetLastState(MachineName, -1)
            previousstate = Application(laststate)
        Else 'If there isn't a new fault then populate the grid with all of the open faults
            'This could have come from a valid page or opened up again because fault wasn't closed. In that case
            'The option on the parent page would still be to have LOOF
            'This wouldn't be altered until concession or closed event
            'Dim objCon As UserControl = Page.LoadControl("ViewOpenFaults.ascx")
            'CType(objCon, ViewOpenFaults).TabName = TabName
            'CType(objCon, ViewOpenFaults).LinacName = MachineName
            'Placeholder1.Controls.Add(objCon)
            newfaultUpdatePanel1.Visible = True
            Dim objCon As UserControl = Page.LoadControl("ViewOpenFaults.ascx")
            CType(objCon, ViewOpenFaults).TabName = TabName
            CType(objCon, ViewOpenFaults).LinacName = MachineName
            PlaceHolder2.Controls.Add(objCon)
            UpdatePanel3.Visible = True
            MultiView1.SetActiveView(View2)
            PlaceHolder2.Visible = True
            'DropDownList1.SelectedIndex = -1
            'DropDownList2.SelectedIndex = -1
            'CommentBox1.Text = ""
            'SqlDataSource2.ID = "SqlDataSource2"
            'SqlDataSource2.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            'SqlDataSource2.SelectCommand = "select distinct r.FaultID, r.Description, r.ReportedBy, r.DateReported, f.Status, f.ConcessionNumber, f.linac, f.incidentID " & _
            '"from reportfault r left outer join FaultIDTable f on f.originalfaultid = r.faultid where f.linac=@linac and f.status in('Open','Concession') order by incidentid desc"
            'SqlDataSource2.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
            'SqlDataSource2.SelectParameters.Add("linac", MachineName)
            'GridView1.DataSource = SqlDataSource2
            'GridView1.DataBind()
            'GridView1.Columns(8).Visible = True

            ''If UpdatePanel1 is Visible then a row must have been selected so the tracking grid will reload
            'If UpdatePanel1.Visible Then
            '    BindTrackingGrid()
            '    Label2.Text = faultNumber
            'End If
        End If




    End Sub

    Protected Sub BindOpenfault()

        'This is called when the new fault is changed to Open or Concession
        DropDownList1.SelectedIndex = -1
        DropDownList2.SelectedIndex = -1
        CommentBox1.Text = ""
        Dim SqlDataSource2 As New SqlDataSource()
        SqlDataSource2.ID = "SqlDataSource2"
        SqlDataSource2.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        'SqlDataSource2.SelectCommand = "select * from reportfault where linac=@linac and FaultStatus in('Open', 'Concession') order by faultid desc"
        SqlDataSource2.SelectCommand = "select distinct r.FaultID, r.Description, r.ReportedBy, r.DateReported, f.Status, f.ConcessionNumber, f.linac, f.incidentid " & _
            "from reportfault r left outer join FaultTracking t on r.incidentID=t.incidentID left outer join FaultIDTable f on f.incidentid=r.incidentid where f.status=t.status and f.Status in('Open', 'Concession') order by f.incidentid desc"
        SqlDataSource2.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        SqlDataSource2.SelectParameters.Add("linac", MachineName)
        GridView1.DataSource = SqlDataSource2
        GridView1.DataBind()
        GridView1.SelectedIndex = -1



        newfaultUpdatePanel1.Visible = True
        UpdatePanel1.Visible = False

        GridView1.Columns(8).Visible = True

    End Sub
    Protected Sub BindTrackingGrid()
        Dim incidentNumber As Integer
        incidentNumber = CInt(GridView1.SelectedDataKey.Values("incidentID"))

        Dim SqlDataSource3 As New SqlDataSource()
        SqlDataSource3.ID = "SqlDataSource3"
        SqlDataSource3.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        SqlDataSource3.SelectCommand = "select t.TrackingID, t.trackingcomment, t.AssignedTo, t.Status, t.LastUpDatedBy, t.LastUpDatedOn, t.linac " & _
            "from FaultTracking t  where t.linac=@linac and t.incidentID=@incidentID"
        SqlDataSource3.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        SqlDataSource3.SelectParameters.Add("linac", MachineName)
        SqlDataSource3.SelectParameters.Add("@incidentID", System.Data.SqlDbType.Int)
        SqlDataSource3.SelectParameters.Add("incidentID", incidentNumber)
        GridView2.DataSource = SqlDataSource3
        GridView2.DataBind()
    End Sub

    Protected Sub SaveFault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveFault.Click

        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
        If TabName = "Clin" Then
            wctrl.UserReason = "12"
        End If
        Dim faultstatus As String
        Dim ConcessionNumber As String
        Dim strScript As String = "<script>"

        faultstatus = DropDownList1.SelectedItem.Text
        ConcessionNumber = ConcessionNumberBox.Text
        If faultstatus = "Select" Then
            strScript += "alert('Please select a fault status or cancel action');"
            strScript += "</script>"
            ScriptManager.RegisterStartupScript(SaveFault, Me.GetType(), "JSCR", strScript.ToString(), False)
        ElseIf faultstatus = "Concession" And ConcessionNumber = "" Then
            strScript += "alert('Please Enter a Concession Number');"
            strScript += "</script>"
            ScriptManager.RegisterStartupScript(SaveFault, Me.GetType(), "JSCR", strScript.ToString(), False)

        Else
            wcbutton.Text = "Saving Fault Status"
            Application(actionstate) = "Confirm"
            wctrl.Visible = True
            ForceFocus(wctext)
        End If
    End Sub

    Protected Sub Dropdownlist1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        Dim updateFaultStatus As String
        FaultStatus = StatusLabel1.Text
        If FaultStatus = "Concession" Then
            ConcessionNumberBox.ReadOnly = True
            ConcessiondescriptionBox.ReadOnly = True
        Else
            updateFaultStatus = DropDownList1.SelectedItem.Text
            If updateFaultStatus = "Concession" Then
                ConcessionNumberBox.ReadOnly = False
                ConcessiondescriptionBox.ReadOnly = False
            ElseIf updateFaultStatus = "Open" Then
                ConcessionNumberBox.Text = ""
                ConcessiondescriptionBox.Text = ""
                ConcessionNumberBox.ReadOnly = True
                ConcessiondescriptionBox.ReadOnly = True
            ElseIf updateFaultStatus = "Closed" Then
                ConcessionNumberBox.ReadOnly = True
                ConcessiondescriptionBox.ReadOnly = True
            End If
        End If
        BindTrackingGrid()
    End Sub
    Private Sub ForceFocus(ByVal ctrl As Control)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "FocusScript", "setTimeout(function(){$get('" + _
        ctrl.ClientID + "').focus();}, 100);", True)
    End Sub
End Class

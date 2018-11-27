Imports System.Data.SqlClient
Imports System.Data
Partial Class UpDateIncidentuc
    Inherits System.Web.UI.UserControl
    Private MachineName As String
    Private TabName As String
    Private FaultStatus As String
    Private incidentID As String
    Private suspstate As String
    Private actionstate As String
    Private failstate As String

    Public Property Incident() As String
        Get
            Return incidentID
        End Get
        Set(ByVal value As String)
            incidentID = value
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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        AddHandler WriteDatauc3.UserApproved, AddressOf UserApprovedEvent
        actionstate = "ActionState" + MachineName
        suspstate = "Suspended" + MachineName
        failstate = "FailState" + MachineName
    End Sub


    Protected Sub UserApprovedEvent(ByVal Tabused As String, ByVal Userinfo As String)
        'it can't get to here unless a fault has been selected and set to a value
        'It's not possible to get here with the fault still being new because the only way to do that is to
        'cancel
        If Tabused = "incident" Then 'This checks that this is the correct event handler
            'This stops it popping up again when it shouldn't
            Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc3"), WriteDatauc)
            wctrl.Visible = False

            Dim Action As String = Application(actionstate)
            If Action = "Confirm" Then
                Dim username As String = Userinfo
                Dim strScript As String = "<script>"
                Dim selecttext As String
                Dim assignuser As String
                Dim time As DateTime
                time = Now()
                'Dim faultNumber As Integer
                Dim ConcessionNumber As String
                Dim ConcessionDescription As String
                Dim ConcessionActive As Integer
                Dim ConcessionAction As String

                Dim FaultStatus As String
                Dim exists As Integer

                'This should probably happen at the end

                'ConcessiondescriptionBox.ReadOnly = True
                'ConcessionNumberBox.ReadOnly = True
                Dim conn As SqlConnection
                Dim connectionString1 As String = ConfigurationManager.ConnectionStrings( _
                "connectionstring").ConnectionString
                conn = New SqlConnection(connectionString1)
                Try
                    'check that this is successful what happens if it isn't

                    selecttext = DropDownList1.SelectedItem.Text
                    ConcessionNumber = ConcessionNumberBox.Text
                    ConcessionDescription = ConcessiondescriptionBox.Text
                    assignuser = DropDownList2.SelectedItem.Text
                    FaultStatus = StatusLabel1.Text
                    ConcessionAction = ActionBox.Text
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
                    commtrack = New SqlCommand("Insert into FaultTracking (Trackingcomment, AssignedTo, Status, LastupdatedBy, Lastupdatedon,   linac,  action,incidentID) " & _
                                               "VALUES (@Trackingcomment, @AssignedTo, @Status, @LastupdatedBy, @Lastupdatedon,  @linac, @action, @IncidentID)", conn)
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
                    commtrack.Parameters.Add("@action", System.Data.SqlDbType.NVarChar, 250)
                    commtrack.Parameters("@action").Value = ConcessionAction
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
                    commconcess = New SqlCommand("Insert into ConcessionTable (ConcessionNumber, ConcessionDescription, IncidentID, Linac, ConcessionActive, Action) " & _
                    "VALUES (@ConcessionNumber, @ConcessionDescription, @IncidentID, @Linac, @ConcessionActive, @Action) Update FaultIDTable SET ConcessionNumber=@ConcessionNumber WHERE IncidentID=@incidentID ", conn)
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
                    commconcess.Parameters("@Action").Value = ConcessionAction
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
                                    Application(failstate) = Nothing
                                End If
                            End If
                        End If
                    Else
                        'Dim handoverbutton As Button
                        'If Not Me.Parent.FindControl("clinHandoverButton") Is Nothing Then
                        '    handoverbutton = Me.Parent.FindControl("clinHandoverButton")
                        '    handoverbutton.Enabled = True
                        'End If

                    End If

                Else
                    'Linac status now set by repair or other parent container

                    strScript += "alert('Please select a fault or cancel action');"
                    strScript += "</script>"
                    ScriptManager.RegisterStartupScript(SaveAFault, Me.GetType(), "JSCR", strScript.ToString(), False)
                End If



            End If
        End If


    End Sub

    Protected Sub CancelButton_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click
       
        ConcessiondescriptionBox.ReadOnly = True
        ConcessionNumberBox.ReadOnly = True

        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc3"), WriteDatauc)
        wctrl.Visible = False


        Page_Load(Page, e)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim FaultDescription As New List(Of String)

        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc3"), WriteDatauc)
        'wctrl.Visible = True
        'If Not IsPostBack Then
        BindTrackingGrid()

        Label2.Text = incidentID
        Dim conn As SqlConnection
        Dim comm As SqlCommand
        Dim connectionString1 As String = ConfigurationManager.ConnectionStrings( _
        "connectionstring").ConnectionString
        conn = New SqlConnection(connectionString1)
        comm = New SqlCommand("select distinct f.status, ISNULL(c.ConcessionNumber, '') as ConcessionNumber , ISNULL(c.concessiondescription, '') as ConcessionDescription,  f.IncidentID " & _
        "from FaultIDTable f left outer join ConcessionTable c on f.ConcessionNumber=c.ConcessionNumber where f.incidentID = @incidentID", conn)
        comm.Parameters.AddWithValue("@incidentID", incidentID)
        Try
            conn.Open()
            Dim da As New SqlDataAdapter(comm)
            Dim dt As New DataTable()
            Dim nconc As String
            Dim ncond As String
            Dim nincid As String

            da.Fill(dt)
            If dt.Rows.Count > 0 Then

                For Each dataRow As DataRow In dt.Rows

                    FaultStatus = dataRow("Status")
                    nconc = dataRow("ConcessionNumber")
                    ncond = dataRow("ConcessionDescription")
                    nincid = dataRow("incidentID")

                Next
            End If

            StatusLabel1.Text = FaultStatus

            ConcessionNumberBox.Text = nconc
            ConcessiondescriptionBox.Text = ncond

            If FaultStatus = "Concession" Then

                DropDownList1.Items.FindByValue("Open").Enabled = False

            End If


        Finally
            conn.Close()

        End Try


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





        'UpdatePanel1.Visible = False


    End Sub
    Protected Sub BindTrackingGrid()


        Dim SqlDataSource3 As New SqlDataSource()
        SqlDataSource3.ID = "SqlDataSource3"
        SqlDataSource3.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        SqlDataSource3.SelectCommand = "select t.TrackingID, t.trackingcomment, t.AssignedTo, t.Status, t.LastUpDatedBy, t.LastUpDatedOn, t.linac " & _
            "from FaultTracking t  where t.linac=@linac and t.incidentID=@incidentID"
        SqlDataSource3.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        SqlDataSource3.SelectParameters.Add("linac", MachineName)
        SqlDataSource3.SelectParameters.Add("@incidentID", System.Data.SqlDbType.Int)
        SqlDataSource3.SelectParameters.Add("incidentID", incidentID)
        GridView2.DataSource = SqlDataSource3
        GridView2.DataBind()
    End Sub

    Protected Sub SaveAFault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveAFault.Click

        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc3"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)

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
            ScriptManager.RegisterStartupScript(SaveAFault, Me.GetType(), "JSCR", strScript.ToString(), False)
        ElseIf faultstatus = "Concession" And ConcessionNumber = "" Then
            strScript += "alert('Please Enter a Concession Number');"
            strScript += "</script>"
            ScriptManager.RegisterStartupScript(SaveAFault, Me.GetType(), "JSCR", strScript.ToString(), False)

        Else
            wcbutton.Text = "Saving Fault Status"
            Application(actionstate) = "Confirm"
            wctrl.Visible = True
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
End Class



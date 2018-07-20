Imports System.Data.SqlClient
Imports System.Data

Partial Class DefectSavePark
    Inherits System.Web.UI.UserControl
    Private MachineName As String
    Private actionstate As String
    Private time As DateTime
    Const Unrecoverable As Integer = -23
    Public Event UpDateDefect(ByVal EquipmentName As String, ByVal incidentID As String)
    Public Event UpdateViewFault(ByVal EquipmentName As String)

    Public Property LinacName() As String
        Get
            Return MachineName
        End Get
        Set(ByVal value As String)
            MachineName = value
        End Set
    End Property
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Remove reference to this as no longer used after March 2016 done on 23/11/16
        'Added back in 26/3/18 see SPR
        AddHandler WriteDatauc1.UserApproved, AddressOf UserApprovedEvent


    End Sub

    Public Sub UpDateDefectsEventHandler()
        BindDefectData()
        'SetFaults()
    End Sub

    'No need to pass any references now or to have if statements. Analysis 23/11/16 Back in 29/03/18

    Protected Sub UserApprovedEvent(ByVal Tabused As String, ByVal Userinfo As String)
        Dim Action As String = Application(actionstate)
        'Dim Energy As String
        'Dim incidentID As String
        Dim ConcessionNumber As String = Defect.SelectedItem.ToString
        If ConcessionNumber.Contains("ELF") Then
            ConcessionNumber = Left(ConcessionNumber, 7)
        End If
        If Tabused = "Defect" Then
            Dim wctrl As WriteDatauc = CType(FindControl("WriteDatauc1"), WriteDatauc)
            wctrl.Visible = False
            If Action = "Confirm" Then
                BindDefectData()

                'Writeradreset(Userinfo, ConcessionNumber)
            Else
                ClearsForm()
            End If
        End If
    End Sub

    Protected Sub SaveDefectButton_Click(sender As Object, e As System.EventArgs) Handles SaveDefectButton.Click, FaultOpenClosed.SelectedIndexChanged
        'Dim b As Button = TryCast(sender, Button)
        'Dim i As RadioButtonList
        'If b Is Nothing Then
        '    i = CType(sender, RadioButtonList)
        'End If

        'No need for reference to WriteDatauc if no signature - March 2016
        'Back in 26/03/2108
        Dim wctrl As WriteDatauc = CType(FindControl("WriteDatauc1"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
        Dim strScript As String = "<script>"
        Page.Validate("defect")
        If Page.IsValid Then
            If Defect.SelectedItem.Text = "Select" Then
                FaultOpenClosed.SelectedIndex = -1
                wctrl.Visible = False
                strScript += "alert('Please select a fault');"
                strScript += "</script>"
                ScriptManager.RegisterStartupScript(SaveDefectButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                'comment out 21/11/16
                'ElseIf DropDownListArea.SelectedItem.Text = "Select" Then
                '    'wctrl.Visible = False
                '    strScript += "alert('Please select an Area');"
                '    strScript += "</script>"
                '    ScriptManager.RegisterStartupScript(SaveDefectButton, Me.GetType(), "JSCR", strScript.ToString(), False)
            ElseIf Defect.SelectedItem.Text = "UnRecoverable Fault" Then
                If Accuray.Text = "" Then
                    FaultOpenClosed.SelectedIndex = -1
                    wctrl.Visible = False
                    'AreaBox.Enabled = True
                    strScript += "alert('Please enter a name or Job number');"
                    strScript += "</script>"
                    ScriptManager.RegisterStartupScript(SaveDefectButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                ElseIf FaultDescription.Text = "" Then
                    FaultOpenClosed.SelectedIndex = -1
                    wctrl.Visible = False
                    'DropDownListArea.Enabled = True
                    strScript += "alert('Please complete the Fault Description');"
                    strScript += "</script>"
                    ScriptManager.RegisterStartupScript(SaveDefectButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                ElseIf RadAct.Text = "" Then
                    FaultOpenClosed.SelectedIndex = -1
                    wctrl.Visible = False
                    'DropDownListArea.Enabled = True
                    strScript += "alert('Please complete corrective action');"
                    strScript += "</script>"
                    ScriptManager.RegisterStartupScript(SaveDefectButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                Else
                    wcbutton.Text = "Saving Unrecoverable Fault"
                    Application(actionstate) = "Confirm"
                    wctrl.Visible = True
                    ForceFocus(wctext)
                End If
            Else
                'This is what will happen for recoverable fault and Concession
                'Now just call User Approved Even March 2016
                'UserApprovedEvent("Defect", "")
                Application(actionstate) = "Confirm"
                UserApprovedEvent("Defect", "")
            End If

        End If

    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        SaveDefectButton.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(SaveDefectButton, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
        ClearButton.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(ClearButton, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
        actionstate = "ActionState" + MachineName
        Dim newFault As Boolean
        If Not IsPostBack Then
            newFault = False
            SetFaults(newFault)
            'AddEnergyItem()

        End If
        'WriteDatauc1 no longer used 23/11/16
        'Added back in for RAD RESET 26/3/18 SEE SPR
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        wctrl.LinacName = MachineName
        BindDefectData()
    End Sub
    Public Sub ResetDefectDropDown(ByVal incidentid As String)

        Dim result As ListItem
        Dim newFault As Boolean

        result = Defect.Items.FindByValue(incidentid)
        Dim index As Integer
        index = Defect.Items.IndexOf(result)
        If index > 0 Then
            Defect.Items.RemoveAt(index)
            UpdatePanelDefectlist.Update()
        Else
            newFault = True
            SetFaults(newFault)
        End If
    End Sub
    Private Sub SetFaults(ByVal newfault As Boolean)

        Dim selectCommand As Boolean = newfault
        Dim Faults As New DataTable()
        Dim conn As SqlConnection
        Dim comm As SqlCommand
        'formatting has to change between vs versions
        Dim connectionString1 As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        conn = New SqlConnection(connectionString1)
        If newfault Then
            comm = New SqlCommand("SELECT ConcessionNumber + ' ' + ConcessionDescription As Fault, IncidentID FROM [ConcessionTable] where linac=@linac and ConcessionActive = 'TRUE' and IncidentID = (Select max(IncidentID) from  [ConcessionTable] where linac=@linac and ConcessionActive = 'TRUE') order by ConcessionNumber", conn)
            comm.Parameters.AddWithValue("@Linac", MachineName)
        Else
            ' Modified 10 / 11 / 17 because defects are now in DefectTable Not hard wired in to page
            comm = New SqlCommand("SELECT ConcessionNumber + ' ' + ConcessionDescription As Fault, IncidentID FROM [ConcessionTable] where linac=@linac and ConcessionActive = 'TRUE' order by ConcessionNumber", conn)
            comm.Parameters.AddWithValue("@Linac", MachineName)

            comm = New SqlCommand(" SELECT  Defect as Fault, IncidentID From [DefectTable] where linacType in('T') and Active = 'True' UNION SELECT ConcessionNumber + ' ' + ConcessionDescription As Fault, IncidentID FROM [ConcessionTable] where linac=@linac and ConcessionActive = 'TRUE' order by IncidentID", conn)
            comm.Parameters.AddWithValue("@Linac", MachineName)

        End If
        'Don't need to open https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/populating-a-dataset-from-a-dataadapter
        'conn.Open()
        Dim da As New SqlDataAdapter(comm)
        da.Fill(Faults)

        Defect.DataSource = Faults
        Defect.DataTextField = "Fault"
        Defect.DataValueField = "IncidentID"
        Defect.DataBind()


    End Sub
    'Not used - for if areas are in table
    'Protected Sub SetArea()

    '    Dim Areas As New DataTable()
    '    Dim conn As SqlConnection
    '    Dim comm As SqlCommand
    '    'vs formatting
    '    Dim connectionString1 As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
    '    conn = New SqlConnection(connectionString1)

    '    comm = New SqlCommand("SELECT AreaID, Area FROM [AreaTable] order by AreaID", conn)

    '    'Don't need to open https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/populating-a-dataset-from-a-dataadapter
    '    'conn.Open()
    '    Dim at As New SqlDataAdapter(comm)
    '    at.Fill(Areas)

    '    DropDownListArea.DataSource = Areas
    '    DropDownListArea.DataTextField = "Area"
    '    DropDownListArea.DataValueField = "AreaID"
    '    DropDownListArea.DataBind()
    'End Sub

    Private Sub BindDefectData()

        Dim SqlDataSource1 As New SqlDataSource()
        Dim query As String = "SELECT RIGHT(CONVERT(VARCHAR, DateReported, 100),7) as DefectTime, ConcessionNumber, Description FROM [ReportFault] where Cast(DateReported as Date) = Cast(GetDate() as Date) and linac=@linac and ConcessionNumber != '' order by DateReported desc"
        SqlDataSource1 = QuerySqlConnection(MachineName, query)
        GridView1.DataSource = SqlDataSource1
        GridView1.DataBind()
    End Sub

    Protected Function QuerySqlConnection(ByVal MachineName As String, ByVal query As String) As SqlDataSource
        'This uses the sqldatasource instead of the individual conn definitions so reader can't be used
        'this solution is from http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.sqldatasource.select%28v=vs.90%29.aspx

        Dim SqlDataSource1 As New SqlDataSource()
        SqlDataSource1.ID = "SqlDataSource1"
        SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        SqlDataSource1.SelectCommand = (query)
        SqlDataSource1.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        SqlDataSource1.SelectParameters.Add("linac", MachineName)
        Return SqlDataSource1


    End Function

    Protected Sub Defect_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles Defect.SelectedIndexChanged
        'Dim radreset As String = "Undefined"
        Dim incidentIDstring As String = ""
        Dim incidentID As Integer
        Dim conn1 As SqlConnection
        Dim comm1 As SqlCommand
        'Dim comm2 As SqlCommand
        'Dim Region As String = ""
        'Dim Queryreturn As String = ""
        'formatting vs
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString

        incidentIDstring = Defect.SelectedItem.Value
        Dim result As ListItem
        result = Defect.Items.FindByValue(incidentIDstring)
        Dim index As Integer
        index = Defect.Items.IndexOf(result)
        If Integer.TryParse(incidentIDstring, incidentID) Then

            HiddenField1.Value = incidentID

            conn1 = New SqlConnection(connectionString)
            If incidentID > 0 Then 'concession
                comm1 = New SqlCommand("SELECT Action FROM ConcessionTable where incidentID=@incidentID", conn1) 'Corrective action

                comm1.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                comm1.Parameters("@incidentID").Value = incidentID

                conn1.Open()
                'Dim sqlresult As Object = comm1.ExecuteScalar()
                'If sqlresult Is Nothing Then
                'do nothing
                'AreaBox.Text = String.Empty
                'DropDownListArea.SelectedValue = "Select"
                'Else
                'Queryreturn = sqlresult.ToString
                'If Queryreturn = radreset Then
                'DropDownListArea.Enabled = True
                'DropDownListArea.SelectedValue = "Select"
                'AreaBox.Enabled = True
                'Else
                'DropDownListArea.SelectedItem.Text = sqlresult.ToString
                'AreaBox.Text = sqlresult.ToString
                'HiddenField2.Value = sqlresult.ToString - this might be needed for energy or error code later
                'AreaBox.Enabled = False
                'DropDownListArea.Enabled = False
                'If incidentID > 0 Then
                'comm2 = New SqlCommand("SELECT Action FROM ConcessionTable where incidentID=@incidentID", conn1)
                'comm1.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                'comm1.Parameters("@incidentID").Value = incidentID
                Dim sqlresult1 As Object = comm1.ExecuteScalar()
                    RadAct.Text = sqlresult1.ToString
                        RadAct.ReadOnly = True


            End If


        conn1.Close()

        Else
            HiddenField1.Value = -1000
            'AreaBox.Text = String.Empty
            'DropDownListArea.SelectedIndex = -1
        End If

    End Sub
    'Protected Sub AddEnergyItem()
    '    'from http://www.aspsnippets.com/Articles/Programmatically-add-items-to-DropDownList-on-Button-Click-in-ASPNet-using-C-and-VBNet.aspx
    '    'and http://www.yaldex.com/asp_tutorial/0596004877_progaspdotnet2-chp-5-sect-7.html

    '    'This is a mad way of doing it but I don't know how to dim the energy list without constructing it at the same time

    '    Select Case MachineName
    '        Case "LA1"
    '            Dim Energy1() As String = {"Select", "6 MV", "6 MeV", "8 MeV", "10 MeV", "12 MeV", "15 MeV", "18 MeV", "20 MeV"}
    '            ConstructEnergylist(Energy1)
    '        Case "LA2", "LA3"
    '            Dim Energy1() As String = {"Select", "6 MV", "10 MV", "6 MeV", "8 MeV", "10 MeV", "12 MeV", "15 MeV", "18 MeV", "20 MeV"}
    '            ConstructEnergylist(Energy1)
    '        Case "LA4"
    '            Dim Energy1() As String = {"Select", "6 MV", "10 MV"}
    '            ConstructEnergylist(Energy1)
    '        Case "E1", "E2", "B1"
    '            Dim Energy1() As String = {"Select", "6 MV", "6 MV FFF", "10 MV", "10 MV FFF", "4 MeV", "6 MeV", "8 MeV", "10 MeV", "12 MeV", "15 MeV"}
    '            ConstructEnergylist(Energy1)
    '        Case Else
    '            'Don't show any energies
    '    End Select

    'End Sub
    'Protected Sub ConstructEnergylist(ByVal Energylist() As String)
    '    Dim energy() As String = Energylist
    '    Dim i As Integer
    '    For i = 0 To energy.GetLength(0) - 1
    '        DropDownListEnergy.Items.Add(New ListItem(energy(i)))
    '    Next
    '    DropDownListEnergy.SelectedIndex = -1
    'End Sub

    Protected Sub ClearButton_Click(sender As Object, e As System.EventArgs) Handles ClearButton.Click
        ClearsForm()
    End Sub

    Protected Sub ClearsForm()
        Defect.SelectedIndex = -1
        FaultOpenClosed.SelectedIndex = -1
        'DropDownListArea.SelectedIndex = -1
        'DropDownListEnergy.SelectedIndex = -1
        ErrorCode.Text = Nothing
        Accuray.Text = Nothing
        FaultDescription.Text = Nothing
        PatientIDBox.Text = Nothing
        RadAct.Text = Nothing
    End Sub

    'Protected Sub DropDownListArea_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownListArea.SelectedIndexChanged

    '    HiddenField2.Value = DropDownListArea.SelectedValue.ToString
    'End Sub

    Protected Sub Writeradreset(ByVal UserInfo As String, ByVal ConcessionNumber As String)
        'Dim time As DateTime

        Dim LastIncident As Integer
        Dim LastFault As Integer
        time = Now()
        Dim conn As SqlConnection
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        Dim incidentfault As SqlCommand

        Dim comm2 As SqlCommand

        conn = New SqlConnection(connectionString)


        LastIncident = HiddenField1.Value
        If LastIncident = Unrecoverable Then
            Dim logInStatusID As Integer = 0
            ConcessionNumber = ""
            Dim constateid As SqlCommand
            constateid = New SqlCommand("SELECT stateid FROM [LinacStatus] where stateID = (Select max(stateID) as lastrecord from [LinacStatus] where linac=@linac)", conn)
            constateid.Parameters.AddWithValue("@linac", MachineName)
            Dim readers As SqlDataReader
            conn.Open()
            readers = constateid.ExecuteReader()

            If readers.Read() Then
                logInStatusID = readers.Item("stateid")
            End If
            readers.Close()
            conn.Close()
            incidentfault = New SqlCommand("INSERT INTO FaultIDTable (DateInserted, Linac, Status, originalFaultID, ConcessionNumber, StatusID) VALUES (@DateInserted, @Linac, @Status, @originalFaultID, @ConcessionNumber, @StatusID)", conn)
            incidentfault.Parameters.Add("@DateInserted", System.Data.SqlDbType.DateTime)
            incidentfault.Parameters("@DateInserted").Value = time
            incidentfault.Parameters.Add("@Status", System.Data.SqlDbType.NVarChar, 20)
            incidentfault.Parameters("@Status").Value = "Concession"
            incidentfault.Parameters.Add("@originalFaultID", System.Data.SqlDbType.Int)
            incidentfault.Parameters("@originalFaultID").Value = 0
            incidentfault.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 10)
            incidentfault.Parameters("@ConcessionNumber").Value = ""
            incidentfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
            incidentfault.Parameters("@Linac").Value = MachineName
            incidentfault.Parameters.Add("@StatusID", System.Data.SqlDbType.Int)
            incidentfault.Parameters("@StatusID").Value = logInStatusID
            conn.Open()
            incidentfault.ExecuteNonQuery()
            conn.Close()

            Dim comm1 As SqlCommand
            Dim reader1 As SqlDataReader

            comm1 = New SqlCommand("Select IncidentID from FaultIDTable where IncidentId = (select max(IncidentId) from FaultIDTable where linac = @linac)", conn)
            comm1.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
            comm1.Parameters("@Linac").Value = MachineName
            conn.Open()
            reader1 = comm1.ExecuteReader()
            If reader1.Read() Then
                LastIncident = reader1.Item("IncidentID")
                reader1.Close()
                conn.Close()
            End If

            WriteReportFault(UserInfo, LastIncident, ConcessionNumber)

            'This reads the number of the newly created fault to put into the fault tracking database

            comm2 = New SqlCommand("Select FaultID from ReportFault where FaultId = (select max(FaultId) from ReportFault where linac = @linac)", conn)
            comm2.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
            comm2.Parameters("@Linac").Value = MachineName
            conn.Open()
            Dim reader2 As SqlDataReader
            reader2 = comm2.ExecuteReader()
            If reader2.Read() Then
                LastFault = reader2.Item("FaultID")
                reader2.Close()
                conn.Close()
            End If

            WriteTracking(UserInfo, "New", LastIncident, "")

            'update incident table with fault id then don't need it in fault tracking table?

            incidentfault = New SqlCommand("Update FaultIDTable SET originalFaultID=@originalFaultID where incidentID=@incidentID", conn)
            incidentfault.Parameters.Add("@originalFaultID", Data.SqlDbType.Int)
            incidentfault.Parameters("@originalFaultID").Value = LastFault
            incidentfault.Parameters.Add("@incidentID", Data.SqlDbType.Int)
            incidentfault.Parameters("@incidentID").Value = LastIncident
            conn.Open()
            incidentfault.ExecuteNonQuery()
            conn.Close()

            'from viewopenfaults. Now turn into concession
            Dim commconcess As SqlCommand
            commconcess = New SqlCommand("Insert into ConcessionTable (PreFix, ConcessionDescription, IncidentID, Linac, ConcessionActive, Action) VALUES (@PreFix, @ConcessionDescription, @IncidentID, @Linac, @ConcessionActive, @Action) SELECT SCOPE_IDENTITY()", conn)
            commconcess.Parameters.Add("@PreFix", System.Data.SqlDbType.NVarChar, 10)
            commconcess.Parameters("@PreFix").Value = "ELF"
            commconcess.Parameters.Add("@ConcessionDescription", System.Data.SqlDbType.NVarChar, 250)
            commconcess.Parameters("@ConcessionDescription").Value = FaultDescription.Text
            commconcess.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
            commconcess.Parameters("@incidentID").Value = LastIncident
            commconcess.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
            commconcess.Parameters("@Linac").Value = MachineName
            commconcess.Parameters.Add("@ConcessionActive", System.Data.SqlDbType.Bit)
            commconcess.Parameters("@ConcessionActive").Value = 1
            commconcess.Parameters.Add("@Action", System.Data.SqlDbType.NVarChar, 250)
            commconcess.Parameters("@Action").Value = RadAct.Text
            Dim bcommand = New SqlCommand("select count(*) from Concessiontable where incidentID=@incidentID", conn)
            bcommand.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
            bcommand.Parameters("@incidentID").Value = LastIncident

            conn.Open()

            Dim exists As Integer
            exists = bcommand.ExecuteScalar()

            If exists = 0 Then
                'commconcess.ExecuteNonQuery()
                'from http://www.dotnetperls.com/string-format-vbnet
                Dim obj As Object = commconcess.ExecuteScalar()
                Dim value As Integer
                value = CInt(obj)
                Dim concessionnum As String = value.ToString("0000")
                Dim builder As New StringBuilder
                Dim Prefix As String = "ELF"
                builder.Append(Prefix)
                builder.Append(concessionnum)
                Dim s As String = builder.ToString
                commconcess = New SqlCommand("Update FaultIDTable SET ConcessionNumber=@ConcessionNumber WHERE IncidentID=@incidentID ", conn)
                commconcess.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 10)
                commconcess.Parameters("@ConcessionNumber").Value = s
                commconcess.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                commconcess.Parameters("@incidentID").Value = LastIncident
                commconcess.ExecuteNonQuery()
                conn.Close()
                Dim TrackAction As String = RadAct.Text
                'need to write reportfault and faulttracking table again
                WriteTracking(UserInfo, "Concession", LastIncident, TrackAction)
                WriteRadAckFault(LastIncident, False)
                'From repair
                Dim updatefault As SqlCommand
                updatefault = New SqlCommand("UPDATE  FaultIDTable SET ReportClosed = @ReportClosed where IncidentID=@incidentID", conn)
                'updatefault = New SqlCommand("UPDATE  FaultIDTable SET ReportClosed = @ReportClosed where IncidentID  = (Select max(IncidentID) as lastrecord from FaultIDtable where linac=@linac)", conn)
                updatefault.Parameters.Add("@ReportClosed", System.Data.SqlDbType.DateTime)
                updatefault.Parameters("@ReportClosed").Value = Now()
                updatefault.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
                updatefault.Parameters("@linac").Value = MachineName
                updatefault.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                updatefault.Parameters("@incidentID").Value = LastIncident

                Try
                    conn.Open()
                    updatefault.ExecuteNonQuery()

                Finally
                    conn.Close()
                End Try
                BindDefectData()
                RaiseEvent UpDateDefect(MachineName, LastIncident)
                RaiseEvent UpdateViewFault(MachineName)
            End If
        Else
            WriteReportFault(UserInfo, LastIncident, ConcessionNumber)
            BindDefectData()
        End If


        ClearsForm()


    End Sub
    Protected Sub WriteReportFault(ByVal UserInfo As String, ByVal LastIncident As String, ConcessionNumber As String)
        Dim conn As SqlConnection
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString

        time = Now()

        conn = New SqlConnection(connectionString)
        Dim commfault As SqlCommand
        commfault = New SqlCommand("INSERT INTO ReportFault (Description, ReportedBy, DateReported, Area, Energy, Linac, IncidentID, BSUHID, ConcessionNumber ) " _
                                  & "VALUES (@Description, @ReportedBy, @DateReported, @Area, @Energy, @Linac, @IncidentID, @BSUHID, @ConcessionNumber )", conn)
        commfault.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar, 250)
        commfault.Parameters("@Description").Value = FaultDescription.Text
        commfault.Parameters.Add("@ReportedBy", System.Data.SqlDbType.NVarChar, 50)
        commfault.Parameters("@ReportedBy").Value = UserInfo
        commfault.Parameters.Add("@DateReported", System.Data.SqlDbType.DateTime)
        commfault.Parameters("@DateReported").Value = time
        commfault.Parameters.Add("@Area", System.Data.SqlDbType.NVarChar, 20)
        commfault.Parameters("@Area").Value = Accuray.Text
        commfault.Parameters.Add("@Energy", System.Data.SqlDbType.NVarChar, 10)
        commfault.Parameters("@Energy").Value = ErrorCode.Text
        commfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
        commfault.Parameters("@Linac").Value = MachineName
        commfault.Parameters.Add("@IncidentID", System.Data.SqlDbType.Int)
        commfault.Parameters("@IncidentID").Value = LastIncident
        commfault.Parameters.Add("@BSUHID", System.Data.SqlDbType.NVarChar, 7)
        commfault.Parameters("@BSUHID").Value = PatientIDBox.Text
        commfault.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 25)
        commfault.Parameters("@ConcessionNumber").Value = ConcessionNumber
        Try

            conn.Open()
            commfault.ExecuteNonQuery()

        Finally
            conn.Close()

        End Try
    End Sub
    Protected Sub WriteTracking(ByVal UserInfo As String, ByVal Status As String, ByVal LastIncident As Integer, ByVal Action As String)
        time = Now()
        Dim conn As SqlConnection
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        conn = New SqlConnection(connectionString)
        Dim commtrack As SqlCommand
        commtrack = New SqlCommand("Insert into FaultTracking (Trackingcomment, AssignedTo, Status, LastupdatedBy, Lastupdatedon, Linac, Action, IncidentID) " _
                                  & "VALUES (@Trackingcomment, @AssignedTo, @Status, @LastupdatedBy, @Lastupdatedon, @Linac, @Action, @IncidentID)", conn)
        commtrack.Parameters.Add("@Trackingcomment", System.Data.SqlDbType.NVarChar, 250)
        commtrack.Parameters("@Trackingcomment").Value = ""
        commtrack.Parameters.Add("@AssignedTo", Data.SqlDbType.NVarChar, 50)
        commtrack.Parameters("@AssignedTo").Value = "Unassigned"
        commtrack.Parameters.Add("@Status", Data.SqlDbType.NVarChar, 50)
        commtrack.Parameters("@Status").Value = Status
        commtrack.Parameters.Add("@LastupdatedBy", System.Data.SqlDbType.NVarChar, 50)
        commtrack.Parameters("@LastupdatedBy").Value = UserInfo
        commtrack.Parameters.Add("@Lastupdatedon", System.Data.SqlDbType.DateTime)
        commtrack.Parameters("@Lastupdatedon").Value = time
        commtrack.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
        commtrack.Parameters("@Linac").Value = MachineName
        commtrack.Parameters.Add("@Action", System.Data.SqlDbType.NVarChar, 250)
        commtrack.Parameters("@Action").Value = Action
        commtrack.Parameters.Add("@IncidentID", System.Data.SqlDbType.Int)
        commtrack.Parameters("@IncidentID").Value = LastIncident
        Try
            conn.Open()
            commtrack.ExecuteNonQuery()


        Finally
            conn.Close()

        End Try
    End Sub

    Protected Sub WriteRadAckFault(ByVal LastIncident As Integer, ByVal Ack As Boolean)
        Dim conn As SqlConnection
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        conn = New SqlConnection(connectionString)
        Dim commack As SqlCommand
        commack = New SqlCommand("Insert into RadAckFault (IncidentID,TrackingID, Acknowledge) VALUES (@IncidentID,@TrackingID,@Acknowledge)", conn)
        commack.Parameters.Add("@IncidentID", Data.SqlDbType.Int)
        commack.Parameters("@IncidentID").Value = LastIncident
        commack.Parameters.Add("@TrackingID", System.Data.SqlDbType.Int)
        commack.Parameters("@TrackingID").Value = 0
        commack.Parameters.Add("@Acknowledge", Data.SqlDbType.Bit)
        commack.Parameters("@Acknowledge").Value = Ack
        Try
            conn.Open()
            commack.ExecuteNonQuery()
        Finally
            conn.Close()

        End Try
    End Sub

    Private Sub ForceFocus(ByVal ctrl As Control)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "FocusScript", "setTimeout(function(){$get('" + ctrl.ClientID + "').focus();}, 100);", True)
    End Sub

End Class

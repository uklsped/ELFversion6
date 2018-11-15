Imports System.Data.SqlClient
Imports System.Data
Imports System.Transactions

Partial Class DefectSave
    Inherits System.Web.UI.UserControl
    Const EMPTYSTRING As String = ""
    Private actionstate As String
    Private time As DateTime
    Const RADRESET = -21
    Const MAJORFAULT = -24
    Public Event UpDateDefect(ByVal EquipmentName As String, ByVal incidentID As String)
    Public Event UpdateViewFault(ByVal EquipmentName As String)
    Public Property ParentControl() As String
    Public Property LinacName() As String
    Private appstate As String
    Private suspstate As String
    Private repairstate As String
    Private failstate As String
    Private Valid As Boolean = False
    Const RADIO As Integer = 103

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Remove reference to this as no longer used after March 2016 done on 23/11/16
        'Added back in 26/3/18 see SPR
        AddHandler WriteDatauc1.UserApproved, AddressOf UserApprovedEvent
        AddHandler WriteDatauc2.UserApproved, AddressOf UserApprovedEvent

        appstate = "LogOn" + LinacName
        actionstate = "ActionState" + LinacName
        suspstate = "Suspended" + LinacName
        failstate = "FailState" + LinacName
        repairstate = "rppTab" + LinacName
    End Sub

    Public Sub UpDateDefectsEventHandler()
        BindDefectData()
        'SetFaults()
    End Sub

    'No need to pass any references now or to have if statements. Analysis 23/11/16 Back in 29/03/18

    Protected Sub UserApprovedEvent(ByVal Tabused As String, ByVal Userinfo As String)
        Dim Action As String = Application(actionstate)
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        'Dim Energy As String
        'Dim incidentID As String
        Dim ConcessionNumber As String = Defect.SelectedItem.ToString
        If ConcessionNumber.Contains("ELF") Then
            ConcessionNumber = Left(ConcessionNumber, 7)
        End If
        If Tabused = "Defect" Or Tabused = "Major" Then
            Dim wctrl1 As WriteDatauc = CType(FindControl("WriteDatauc1"), WriteDatauc)
            wctrl1.Visible = False
            Dim wctrl2 As WriteDatauc = CType(FindControl("WriteDatauc2"), WriteDatauc)
            wctrl2.Visible = False
            If Action = "Confirm" Then
                'Writeradreset(Userinfo, ConcessionNumber)
                'Try
                '    Using myscope As TransactionScope = New TransactionScope()
                NewWriteradreset(Userinfo, ConcessionNumber, connectionString)
                '        myscope.Complete()
                '    End Using
                'Catch ex As Exception
                '    DavesCode.NewFaultHandling.LogError(ex)
                'End Try

            End If
            ClearsForm()
        End If
    End Sub

    Protected Sub SaveDefectButton_Click(sender As Object, e As System.EventArgs) Handles SaveDefectButton.Click
        'No need for reference to WriteDatauc if no signature - March 2016
        'Back in 26/03/2108

        Dim strScript As String = "<script>"
        Page.Validate("defect")
        If Page.IsValid Then
            'If Defect.SelectedItem.Text = "Select" Then
            '    wctrl.Visible = False
            '    strScript += "alert('Please select a fault');"
            '    strScript += "</script>"
            '    ScriptManager.RegisterStartupScript(SaveDefectButton, Me.GetType(), "JSCR", strScript.ToString(), False)
            '    'comment out 21/11/16
            '    'ElseIf DropDownListArea.SelectedItem.Text = "Select" Then
            '    '    'wctrl.Visible = False
            '    '    strScript += "alert('Please select an Area');"
            '    '    strScript += "</script>"
            '    '    ScriptManager.RegisterStartupScript(SaveDefectButton, Me.GetType(), "JSCR", strScript.ToString(), False)
            If Defect.SelectedItem.Value = RADRESET Then
                'If Defect.SelectedItem.Text = "RAD RESET" Then
                '    If DropDownListArea.SelectedItem.Text = "Select" Then
                '        wctrl.Visible = False
                '        DropDownListArea.Enabled = True
                '        strScript += "alert('Please select an Area');"
                '        strScript += "</script>"
                '        ScriptManager.RegisterStartupScript(SaveDefectButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                '    ElseIf FaultDescription.Text = "" Then
                '        wctrl.Visible = False
                '        DropDownListArea.Enabled = True
                '        strScript += "alert('Please complete the Fault Description');"
                '        strScript += "</script>"
                '        ScriptManager.RegisterStartupScript(SaveDefectButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                '    ElseIf RadAct.Text = "" Then
                '        wctrl.Visible = False
                '        DropDownListArea.Enabled = True
                '        strScript += "alert('Please complete corrective action');"
                '        strScript += "</script>"
                '        ScriptManager.RegisterStartupScript(SaveDefectButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                Dim wctrl As WriteDatauc = CType(FindControl("WriteDatauc1"), WriteDatauc)
                Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
                Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
                wcbutton.Text = "Saving RAD RESET"
                Application(actionstate) = "Confirm"
                wctrl.Visible = True
                ForceFocus(wctext)
            ElseIf Defect.SelectedItem.Value = MAJORFAULT Then
                Dim wctrl As WriteDatauc = CType(FindControl("WriteDatauc2"), WriteDatauc)
                Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
                Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
                wcbutton.Text = "Saving Major Fault"
                Application(actionstate) = "Confirm"

                wctrl.Visible = True
                ForceFocus(wctext)
            Else
                Application(actionstate) = "Confirm"
                UserApprovedEvent("Defect", "")
            End If
        Else
            'Makes sure Area is still available after failed validation
            If (Defect.SelectedItem.Value = RADRESET) Or (Defect.SelectedItem.Value = MAJORFAULT) Then
                DropDownListArea.Enabled = True
            End If

        End If

    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        SaveDefectButton.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(SaveDefectButton, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
        ClearButton.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(ClearButton, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
        actionstate = "ActionState" + LinacName
        Dim newFault As Boolean
        If Not IsPostBack Then
            newFault = False
            SetFaults(newFault)
            AddEnergyItem()
            RadioIncident.SelectedIndex = -1
        End If
        'WriteDatauc1 no longer used 23/11/16
        'Added back in for RAD RESET 26/3/18 SEE SPR
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        wctrl.LinacName = LinacName
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
        Dim LinacType As String
        Dim selectCommand As Boolean = newfault
        Dim Faults As New DataTable()
        Dim conn As SqlConnection
        Dim comm As SqlCommand
        'formatting has to change between vs versions
        Dim connectionString1 As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        conn = New SqlConnection(connectionString1)
        If newfault Then
            comm = New SqlCommand("SELECT ConcessionNumber + ' ' + ConcessionDescription As Fault, IncidentID FROM [ConcessionTable] where linac=@linac and ConcessionActive = 'TRUE' and IncidentID = (Select max(IncidentID) from  [ConcessionTable] where linac=@linac and ConcessionActive = 'TRUE') order by ConcessionNumber", conn)
            comm.Parameters.AddWithValue("@Linac", LinacName)
        Else
            'Modified 10/11/17 because defects are now in DefectTable not hard wired in to page
            'comm = New SqlCommand("SELECT ConcessionNumber + ' ' + ConcessionDescription As Fault, IncidentID FROM [ConcessionTable] where linac=@linac and ConcessionActive = 'TRUE' order by ConcessionNumber", conn)
            'comm.Parameters.AddWithValue("@Linac", MachineName)
            If LinacName Like "LA?" Then
                LinacType = "O"
            Else
                LinacType = "BE"
            End If
            comm = New SqlCommand(" SELECT  Defect as Fault, IncidentID From [DefectTable] where linacType in('A',@LinacType) and Active = 'True' UNION SELECT ConcessionNumber + ' ' + ConcessionDescription As Fault, IncidentID FROM [ConcessionTable] where linac=@linac and ConcessionActive = 'TRUE' order by IncidentID", conn)
            comm.Parameters.AddWithValue("@Linac", LinacName)
            comm.Parameters.AddWithValue("@LinacType", LinacType)
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
        SqlDataSource1 = QuerySqlConnection(LinacName, query)
        GridView1.DataSource = SqlDataSource1
        GridView1.DataBind()
    End Sub

    Protected Function QuerySqlConnection(ByVal MachineName As String, ByVal query As String) As SqlDataSource
        'This uses the sqldatasource instead of the individual conn definitions so reader can't be used
        'this solution is from http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.sqldatasource.select%28v=vs.90%29.aspx

        Dim SqlDataSource1 As New SqlDataSource With {
            .ID = "SqlDataSource1",
            .ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString,
            .SelectCommand = (query)
        }
        SqlDataSource1.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        SqlDataSource1.SelectParameters.Add("linac", MachineName)
        Return SqlDataSource1

    End Function

    Protected Sub Defect_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles Defect.SelectedIndexChanged
        'Dim radreset As String = "Undefined"
        Dim incidentIDstring As String = ""
        Dim incidentID As Integer
        Dim conn As SqlConnection
        Dim comm1 As SqlCommand
        'Dim comm2 As SqlCommand
        Dim Region As String = ""
        Dim Queryreturn As String = ""
        'formatting vs
        Dim reader As SqlDataReader
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        Dim Area As String = ""
        Dim Action As String = ""
        incidentIDstring = Defect.SelectedItem.Value
        If incidentIDstring = "Select" Then
            ClearsForm()
        Else
            Dim result As ListItem
            result = Defect.Items.FindByValue(incidentIDstring)
            Dim index As Integer
            index = Defect.Items.IndexOf(result)
            If Integer.TryParse(incidentIDstring, incidentID) Then
                DropDownListArea.SelectedIndex = -1
                HiddenField1.Value = incidentID
                HiddenField2.Value = Now().ToString
                conn = New SqlConnection(connectionString)
                conn.Open()
                If incidentID > 0 Then
                    comm1 = New SqlCommand("Select r.Area, c.Action from ReportFault r left outer join ConcessionTable c on r.incidentid = c.incidentid where r.incidentID=@incidentID", conn)
                    comm1.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    comm1.Parameters("@incidentID").Value = incidentID
                    reader = comm1.ExecuteReader()
                    If reader.Read() Then
                        Area = reader.Item("Area")
                        Action = reader.Item("Action")
                    End If
                    reader.Close()
                    conn.Close()
                    DropDownListArea.SelectedItem.Text = Area
                    HiddenField3.Value = Area
                    DropDownListArea.Enabled = False
                    RadAct.Text = Action
                    RadAct.ReadOnly = True

                    'ElseIf (incidentID = RADRESET) Or (incidentID = MAJORFAULT) Then
                ElseIf (incidentID = RADRESET) Then
                    DropDownListArea.Enabled = True
                    DropDownListArea.SelectedValue = "Select"
                    RadAct.ReadOnly = False
                    FaultDescriptionValidation.ValidationGroup = "defect"
                    RadActValidation.ValidationGroup = "defect"
                    AreaValidation.ValidationGroup = "defect"
                ElseIf (incidentID = MAJORFAULT) Then
                    DropDownListArea.Enabled = True
                    DropDownListArea.SelectedValue = "Select"
                    RadAct.ReadOnly = False
                    FaultDescriptionValidation.ValidationGroup = "defect"
                    AreaValidation.ValidationGroup = "defect"
                Else
                    comm1 = New SqlCommand("SELECT Area from DefectTable where incidentID=@incidentID", conn)
                    comm1.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    comm1.Parameters("@incidentID").Value = incidentID

                    Dim sqlresult As Object = comm1.ExecuteScalar()
                    conn.Close()
                    Area = sqlresult.ToString()
                    DropDownListArea.SelectedItem.Text = Area
                    HiddenField3.Value = Area
                    DropDownListArea.Enabled = False
                End If

                'comm1.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                'comm1.Parameters("@incidentID").Value = incidentID

                'conn.Open()
                ''Dim sqlresult As Object = comm1.ExecuteScalar()
                'If sqlresult Is Nothing Then
                '        'AreaBox.Text = String.Empty
                '        DropDownListArea.SelectedValue = "Select"
                '    Else
                '        Queryreturn = sqlresult.ToString
                '        If Queryreturn = radreset Then
                '            DropDownListArea.Enabled = True
                '            DropDownListArea.SelectedValue = "Select"
                '            FaultDescriptionValidation.ValidationGroup = "defect"
                '            RadActValidation.ValidationGroup = "defect"
                '            AreaValidation.ValidationGroup = "defect"

                '        Else
                '            DropDownListArea.SelectedItem.Text = sqlresult.ToString
                '            HiddenField3.Value = sqlresult.ToString
                '            DropDownListArea.Enabled = False
                '            If incidentID > 0 Then
                '                comm2 = New SqlCommand("SELECT Action FROM ConcessionTable where incidentID=@incidentID", conn1)
                '                comm2.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                '                comm2.Parameters("@incidentID").Value = incidentID
                '                Dim sqlresult1 As Object = comm2.ExecuteScalar()
                '                RadAct.Text = sqlresult1.ToString
                '                RadAct.ReadOnly = True
                '            End If

                '        End If
                '    End If

                '    conn1.Close()
                '    'RadioIncident.Enabled = True
                SaveDefectButton.Enabled = True
                SaveDefectButton.BackColor = Drawing.Color.Yellow
            Else
                HiddenField1.Value = -1000
                'AreaBox.Text = String.Empty
                DropDownListArea.SelectedIndex = -1
            End If
        End If
    End Sub
    Protected Sub AddEnergyItem()
        'from http://www.aspsnippets.com/Articles/Programmatically-add-items-to-DropDownList-on-Button-Click-in-ASPNet-using-C-and-VBNet.aspx
        'and http://www.yaldex.com/asp_tutorial/0596004877_progaspdotnet2-chp-5-sect-7.html

        'This is a mad way of doing it but I don't know how to dim the energy list without constructing it at the same time

        Select Case LinacName
            Case "LA1"
                Dim Energy1() As String = {"Select", "6 MV", "6 MeV", "8 MeV", "10 MeV", "12 MeV", "15 MeV", "18 MeV", "20 MeV"}
                ConstructEnergylist(Energy1)
            Case "LA2", "LA3"
                Dim Energy1() As String = {"Select", "6 MV", "10 MV", "6 MeV", "8 MeV", "10 MeV", "12 MeV", "15 MeV", "18 MeV", "20 MeV"}
                ConstructEnergylist(Energy1)
            Case "LA4"
                Dim Energy1() As String = {"Select", "6 MV", "10 MV"}
                ConstructEnergylist(Energy1)
            Case "E1", "E2", "B1"
                Dim Energy1() As String = {"Select", "6 MV", "6 MV FFF", "10 MV", "10 MV FFF", "4 MeV", "6 MeV", "8 MeV", "10 MeV", "12 MeV", "15 MeV"}
                ConstructEnergylist(Energy1)
            Case Else
                'Don't show any energies
        End Select

    End Sub
    Protected Sub ConstructEnergylist(ByVal Energylist() As String)
        Dim energy() As String = Energylist
        Dim i As Integer
        For i = 0 To energy.GetLength(0) - 1
            DropDownListEnergy.Items.Add(New ListItem(energy(i)))
        Next
        DropDownListEnergy.SelectedIndex = -1
    End Sub

    Protected Sub ClearButton_Click(sender As Object, e As System.EventArgs) Handles ClearButton.Click
        ClearsForm()
    End Sub

    Protected Sub ClearsForm()
        Defect.SelectedIndex = -1
        RadioIncident.SelectedIndex = -1
        'RadioIncident.Enabled = False
        DropDownListArea.SelectedIndex = -1
        DropDownListEnergy.SelectedIndex = -1
        GantryAngleBox.Text = Nothing
        CollimatorAngleBox.Text = Nothing
        FaultDescription.Text = Nothing
        PatientIDBox.Text = Nothing
        RadAct.Text = Nothing
        SaveDefectButton.BackColor = Drawing.Color.LightGray
        SaveDefectButton.Enabled = False

    End Sub

    Protected Sub DropDownListArea_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownListArea.SelectedIndexChanged

        HiddenField3.Value = DropDownListArea.SelectedValue.ToString
    End Sub



    Protected Sub NewWriteradreset(ByVal UserInfo As String, ByVal ConcessionNumber As String, ByVal connectionString As String)
        'Dim FaultP As DavesCode.FaultParameters = New DavesCode.FaultParameters()
        Dim Concession As String = "Concession"
        Dim Status As String = EMPTYSTRING
        Dim IncidentID As Integer
        Dim LastIncident As Integer
        time = Now()
        Dim FaultParams As DavesCode.FaultParameters = New DavesCode.FaultParameters()
        CreateFaultParams(UserInfo, FaultParams)
        LastIncident = HiddenField1.Value
        Select Case LastIncident
            Case RADRESET
                IncidentID = DavesCode.NewFaultHandling.InsertNewFault("Concession", FaultParams)
                Status = Concession
                BindDefectData()
                RaiseEvent UpDateDefect(LinacName, LastIncident)
                RaiseEvent UpdateViewFault(LinacName)
                'End If
            Case MAJORFAULT
                Dim result As String = False
                Dim susstate As String = Application(suspstate)
                Dim repstate As String = Application(repairstate)
                Dim DaTxtBox As TextBox = Me.Parent.FindControl("CommentBox")
                Dim Comment As String = DaTxtBox.Text
                Dim GridViewE As GridView = Me.Parent.FindControl("Gridview1")
                Dim grdviewI As GridView = Me.Parent.FindControl("GridViewImage")
                Dim iView As Boolean = False
                Dim XVI As Boolean = False
                'Dim Runup As DavesCode.NewEngRunup = New DavesCode.NewEngRunup("Tab")
                'Dim FaultParams As DavesCode.FaultParameters = New DavesCode.FaultParameters()
                'CreateFaultParams(UserInfo, FaultParams)
                Select Case ParentControl

                    Case 1, 7
                        result = DavesCode.NewEngRunup.CommitRunup(GridViewE, grdviewI, LinacName, ParentControl, UserInfo, Comment, Valid, True, False, FaultParams)

                    Case 2
                        DavesCode.Reuse.ReturnImaging(iView, XVI, grdviewI, LinacName)
                        result = DavesCode.NewPreClinRunup.CommitPreClin(LinacName, UserInfo, Comment, iView, XVI, Valid, True, FaultParams)

                    Case 3
                        result = DavesCode.NewCommitClinical.CommitClinical(LinacName, UserInfo, True, FaultParams)
                        Application(suspstate) = 1

                    Case 4, 5, 6, 8
                        result = DavesCode.NewWriteAux.WriteAuxTables(LinacName, UserInfo, Comment, RADIO, ParentControl, True, susstate, repstate, False, FaultParams)

                    Case Else
                        'Put up error message
                        'Application(failstate) = ParentControl
                        'DavesCode.Reuse.WriteAuxTables(LinacName, UserInfo, Comment, RADIO, ParentControl, True, susstate, repstate, False)

                End Select

                If result Then
                    Application(appstate) = Nothing
                    'CreateNewFault(UserInfo, "New", connectionString)
                    'https://support.microsoft.com/en-us/help/312629/prb-threadabortexception-occurs-if-you-use-response-end-response-redir
                    PopupAck()
                    Dim returnstring As String = LinacName + "page.aspx?pageref=Fault&Tabindex="
                    Response.Redirect(returnstring & ParentControl & "&comment=" & "", False)
                Else
                    RaiseError()
                End If
            Case Else
                'Else ' this is responding to repeat fault
                ' WriteReportFault(UserInfo, LastIncident, ConcessionNumber)
                Dim Energy As String
                Energy = DropDownListEnergy.SelectedItem.Text
                If Energy = "Select" Then
                    Energy = ""
                End If
                Dim RadioIncidentSelected As String
                Dim faultid As Integer = -1
                RadioIncidentSelected = RadioIncident.SelectedItem.Value
                faultid = DavesCode.NewFaultHandling.InsertRepeatFault(FaultDescription.Text, UserInfo, DateTime.Parse(HiddenField2.Value), HiddenField3.Value, Energy, GantryAngleBox.Text, CollimatorAngleBox.Text, LinacName, LastIncident, PatientIDBox.Text, ConcessionNumber, RadioIncidentSelected)
                'BindDefectData()
                If Not faultid = -1 Then
                    'RaiseEvent UpdateRepeatFault(RepeatFault, ReportedBy)
                    BindDefectData()
                Else
                    RaiseError()
                End If
                ' End If
        End Select

        'ClearsForm()


    End Sub
    Protected Sub PopupAck()
        Dim strScript As String = "<script>"
        strScript += "alert('Fault Logged');"
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(SaveDefectButton, Me.GetType(), "JSCR", strScript.ToString(), False)
    End Sub
    Protected Sub RaiseError()
        Dim strScript As String = "<script>"

        strScript += "alert('Problem Updating Fault. Please call Engineering');"
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(SaveDefectButton, Me.GetType(), "JSCR", strScript.ToString(), False)
    End Sub

    Protected Sub CreateFaultParams(ByVal UserInfo As String, ByRef FaultParams As DavesCode.FaultParameters)

        Dim Energy As String
        Energy = DropDownListEnergy.SelectedItem.Text
        If Energy = "Select" Then
            Energy = ""
        End If

        FaultParams.Linac = LinacName
        FaultParams.DateInserted = DateTime.Parse(HiddenField2.Value)
        FaultParams.UserInfo = UserInfo
        FaultParams.Area = HiddenField3.Value
        FaultParams.Energy = Energy
        FaultParams.GantryAngle = GantryAngleBox.Text
        FaultParams.CollimatorAngle = CollimatorAngleBox.Text
        FaultParams.PatientID = PatientIDBox.Text
        FaultParams.FaultDescription = FaultDescription.Text
        FaultParams.RadAct = RadAct.Text
        FaultParams.RadioIncident = RadioIncident.SelectedItem.Value

    End Sub
    'Protected Function CreateNewFault(ByVal State As String, ByVal FaultParameters As DavesCode.FaultParameters) As Integer
    '    Dim IncidentID As Integer = 0
    'Dim DateInserted As DateTime = DateTime.Parse(HiddenField2.Value)
    'Dim Description As String = FaultDescription.Text
    'Dim ConcessionNumber = EMPTYSTRING
    'Dim Assigned = EMPTYSTRING
    'Dim TrackingComment = EMPTYSTRING
    'Dim TrackAction = EMPTYSTRING
    'Dim RadioIncidentSelected As String = EMPTYSTRING
    'Dim Energy As String
    'Energy = DropDownListEnergy.SelectedItem.Text
    'If Energy = "Select" Then
    '    Energy = ""
    'End If
    'RadioIncidentSelected = RadioIncident.SelectedItem.Value
    '    IncidentID = DavesCode.NewFaultHandling.InsertNewFault(State, FaultParameters)
    '    Return IncidentID
    'End Function
    'Protected Sub WriteReportFault(ByVal UserInfo As String, ByVal LastIncident As String, ConcessionNumber As String)
    '    Dim conn As SqlConnection
    '    Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
    '    Dim Energy As String
    '    Energy = DropDownListEnergy.SelectedItem.Text
    '    If Energy = "Select" Then
    '        Energy = ""
    '    End If
    '    time = Now()

    '    conn = New SqlConnection(connectionString)
    '    Dim commfault As SqlCommand
    '    commfault = New SqlCommand("INSERT INTO ReportFault (Description, ReportedBy, DateReported, Area, Energy, GantryAngle, CollimatorAngle,Linac, IncidentID, BSUHID, ConcessionNumber ) " _
    '                              & "VALUES (@Description, @ReportedBy, @DateReported, @Area, @Energy,@GantryAngle,@CollimatorAngle, @Linac, @IncidentID, @BSUHID, @ConcessionNumber )", conn)
    '    commfault.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar, 250)
    '    commfault.Parameters("@Description").Value = FaultDescription.Text
    '    commfault.Parameters.Add("@ReportedBy", System.Data.SqlDbType.NVarChar, 50)
    '    commfault.Parameters("@ReportedBy").Value = UserInfo
    '    commfault.Parameters.Add("@DateReported", System.Data.SqlDbType.DateTime)
    '    commfault.Parameters("@DateReported").Value = time
    '    commfault.Parameters.Add("@Area", System.Data.SqlDbType.NVarChar, 20)
    '    commfault.Parameters("@Area").Value = HiddenField3.Value
    '    commfault.Parameters.Add("@Energy", System.Data.SqlDbType.NVarChar, 10)
    '    commfault.Parameters("@Energy").Value = Energy
    '    commfault.Parameters.Add("@GantryAngle", System.Data.SqlDbType.NVarChar, 3)
    '    commfault.Parameters("@GantryAngle").Value = GantryAngleBox.Text
    '    commfault.Parameters.Add("@CollimatorAngle", System.Data.SqlDbType.NVarChar, 3)
    '    commfault.Parameters("@CollimatorAngle").Value = CollimatorAngleBox.Text
    '    commfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
    '    commfault.Parameters("@Linac").Value = LinacName
    '    commfault.Parameters.Add("@IncidentID", System.Data.SqlDbType.Int)
    '    commfault.Parameters("@IncidentID").Value = LastIncident
    '    commfault.Parameters.Add("@BSUHID", System.Data.SqlDbType.NVarChar, 7)
    '    commfault.Parameters("@BSUHID").Value = PatientIDBox.Text
    '    commfault.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 25)
    '    commfault.Parameters("@ConcessionNumber").Value = ConcessionNumber
    '    Try

    '        conn.Open()
    '        commfault.ExecuteNonQuery()

    '    Finally
    '        conn.Close()

    '    End Try
    'End Sub
    'Protected Sub WriteTracking(ByVal UserInfo As String, ByVal Status As String, ByVal LastIncident As Integer, ByVal Action As String)
    '    time = Now()
    '    Dim conn As SqlConnection
    '    Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
    '    conn = New SqlConnection(connectionString)
    '    Dim commtrack As SqlCommand
    '    commtrack = New SqlCommand("Insert into FaultTracking (Trackingcomment, AssignedTo, Status, LastupdatedBy, Lastupdatedon, Linac, Action, IncidentID) " _
    '                              & "VALUES (@Trackingcomment, @AssignedTo, @Status, @LastupdatedBy, @Lastupdatedon, @Linac, @Action, @IncidentID)", conn)
    '    commtrack.Parameters.Add("@Trackingcomment", System.Data.SqlDbType.NVarChar, 250)
    '    commtrack.Parameters("@Trackingcomment").Value = ""
    '    commtrack.Parameters.Add("@AssignedTo", Data.SqlDbType.NVarChar, 50)
    '    commtrack.Parameters("@AssignedTo").Value = "Unassigned"
    '    commtrack.Parameters.Add("@Status", Data.SqlDbType.NVarChar, 50)
    '    commtrack.Parameters("@Status").Value = Status
    '    commtrack.Parameters.Add("@LastupdatedBy", System.Data.SqlDbType.NVarChar, 50)
    '    commtrack.Parameters("@LastupdatedBy").Value = UserInfo
    '    commtrack.Parameters.Add("@Lastupdatedon", System.Data.SqlDbType.DateTime)
    '    commtrack.Parameters("@Lastupdatedon").Value = time
    '    commtrack.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
    '    commtrack.Parameters("@Linac").Value = LinacName
    '    commtrack.Parameters.Add("@Action", System.Data.SqlDbType.NVarChar, 250)
    '    commtrack.Parameters("@Action").Value = Action
    '    commtrack.Parameters.Add("@IncidentID", System.Data.SqlDbType.Int)
    '    commtrack.Parameters("@IncidentID").Value = LastIncident
    '    Try
    '        conn.Open()
    '        commtrack.ExecuteNonQuery()


    '    Finally
    '        conn.Close()

    '    End Try
    'End Sub

    'Protected Sub WriteRadAckFault(ByVal LastIncident As Integer, ByVal Ack As Boolean)
    '    Dim conn As SqlConnection
    '    Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
    '    conn = New SqlConnection(connectionString)
    '    Dim commack As SqlCommand
    '    commack = New SqlCommand("Insert into RadAckFault (IncidentID,TrackingID, Acknowledge) VALUES (@IncidentID,@TrackingID,@Acknowledge)", conn)
    '    commack.Parameters.Add("@IncidentID", Data.SqlDbType.Int)
    '    commack.Parameters("@IncidentID").Value = LastIncident
    '    commack.Parameters.Add("@TrackingID", System.Data.SqlDbType.Int)
    '    commack.Parameters("@TrackingID").Value = 0
    '    commack.Parameters.Add("@Acknowledge", Data.SqlDbType.Bit)
    '    commack.Parameters("@Acknowledge").Value = Ack
    '    Try
    '        conn.Open()
    '        commack.ExecuteNonQuery()
    '    Finally
    '        conn.Close()

    '    End Try
    'End Sub

    Private Sub ForceFocus(ByVal ctrl As Control)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "FocusScript", "setTimeout(function(){$get('" + ctrl.ClientID + "').focus();}, 100);", True)
    End Sub
    'Protected Sub RadioIncident_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioIncident.SelectedIndexChanged
    '    Dim Selected As String = ""
    '    Selected = RadioIncident.SelectedItem.Text
    '    SaveDefectButton.Enabled = True
    '    SaveDefectButton.BackColor = Drawing.Color.Yellow
    '    'AccurayValidation.ValidationGroup = "defect"
    '    'FaultDescriptionValidation.ValidationGroup = "defect"
    '    'CorrectiveActionValidation.ValidationGroup = "defect"
    'End Sub
End Class

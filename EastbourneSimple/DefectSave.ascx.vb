Imports System.Data.SqlClient
Imports System.Data

Partial Class DefectSave
    Inherits System.Web.UI.UserControl
    Private MachineName As String
    Private actionstate As String

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

    'No need to pass any references now or to have if statements. Analysis 23/11/16

    Protected Sub UserApprovedEvent(ByVal Tabused As String, ByVal Userinfo As String)
        Dim Action As String = Application(actionstate)
        Dim Energy As String
        Dim incidentID As String

        If Tabused = "Defect" Then
            Dim wctrl As WriteDatauc = CType(FindControl("WriteDatauc1"), WriteDatauc)
            wctrl.Visible = False
            If Action = "Confirm" Then
                Dim time As DateTime
                time = Now

                Energy = DropDownListEnergy.SelectedItem.Text
                If Energy = "Select" Then
                    Energy = ""
                End If

                incidentID = HiddenField1.Value

                Dim conn As SqlConnection

                Dim connectionString As String = ConfigurationManager.ConnectionStrings(
                "connectionstring").ConnectionString

                Dim commfault As SqlCommand

                conn = New SqlConnection(connectionString)

                commfault = New SqlCommand("INSERT INTO ReportFault (Description, ReportedBy, DateReported, Area, Energy, GantryAngle, CollimatorAngle,Linac, IncidentID, BSUHID, ConcessionNumber) " &
                                           "VALUES (@Description, @ReportedBy, @DateReported, @Area, @Energy,@GantryAngle,@CollimatorAngle, @Linac, @IncidentID, @BSUHID, @ConcessionNumber )", conn)
                commfault.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar, 250)
                commfault.Parameters("@Description").Value = TextBox4.Text
                commfault.Parameters.Add("@ReportedBy", System.Data.SqlDbType.NVarChar, 50)
                'userinfo is redundant. Replace with string.empty 23/11/16
                'commfault.Parameters("@ReportedBy").Value = String.Empty
                commfault.Parameters("@ReportedBy").Value = Userinfo
                commfault.Parameters.Add("@DateReported", System.Data.SqlDbType.DateTime)
                commfault.Parameters("@DateReported").Value = time
                commfault.Parameters.Add("@Area", System.Data.SqlDbType.NVarChar, 20)
                'Area now is text box. 23/11/16
                'commfault.Parameters("@Area").Value = DropDownListArea.SelectedItem.ToString
                commfault.Parameters("@Area").Value = AreaBox.Text
                commfault.Parameters.Add("@Energy", System.Data.SqlDbType.NVarChar, 6)
                commfault.Parameters("@Energy").Value = Energy
                commfault.Parameters.Add("@GantryAngle", System.Data.SqlDbType.NVarChar, 3)
                commfault.Parameters("@GantryAngle").Value = TextBox2.Text
                commfault.Parameters.Add("@CollimatorAngle", System.Data.SqlDbType.NVarChar, 3)
                commfault.Parameters("@CollimatorAngle").Value = TextBox3.Text
                commfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                commfault.Parameters("@Linac").Value = MachineName
                commfault.Parameters.Add("@IncidentID", System.Data.SqlDbType.Int)
                commfault.Parameters("@IncidentID").Value = incidentID
                commfault.Parameters.Add("@BSUHID", System.Data.SqlDbType.VarChar, 7)
                commfault.Parameters("@BSUHID").Value = PatientIDBox.Text
                commfault.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 25)
                commfault.Parameters("@ConcessionNumber").Value = Defect.SelectedItem.ToString
                Try
                    conn.Open()
                    commfault.ExecuteNonQuery()
                    conn.Close()
                Finally
                    DropDownListEnergy.SelectedIndex = -1
                    AreaBox.Text = String.Empty
                    PatientIDBox.Text = String.Empty
                    TextBox2.Text = String.Empty
                    TextBox3.Text = String.Empty
                    TextBox4.Text = String.Empty
                    conn.Close()

                End Try


                'End If
                Defect.SelectedIndex = -1
                BindDefectData()
            End If
        End If
    End Sub
    Protected Sub UserApprovedEvent()
        'Doesn't get here via Writedatauc now but directly from save button March 2016
        'Dim Action As String = Application(actionstate)
        Dim Energy As String
        Dim incidentID As String

        'If Tabused = "Defect" Then


        ' Dim wctrl As WriteDatauc = CType(FindControl("WriteDatauc1"), WriteDatauc)
        ' wctrl.Visible = False


        'If Action = "Confirm" Then
        Dim time As DateTime
        time = Now

        Energy = DropDownListEnergy.SelectedItem.Text
        If Energy = "Select" Then
            Energy = ""
        End If

        incidentID = HiddenField1.Value

        Dim conn As SqlConnection

        Dim connectionString As String = ConfigurationManager.ConnectionStrings( _
        "connectionstring").ConnectionString

        Dim commfault As SqlCommand

        conn = New SqlConnection(connectionString)

        commfault = New SqlCommand("INSERT INTO ReportFault (Description, ReportedBy, DateReported, Area, Energy, GantryAngle, CollimatorAngle,Linac, IncidentID, BSUHID, ConcessionNumber) " & _
                                   "VALUES (@Description, @ReportedBy, @DateReported, @Area, @Energy,@GantryAngle,@CollimatorAngle, @Linac, @IncidentID, @BSUHID, @ConcessionNumber )", conn)
        commfault.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar, 250)
        commfault.Parameters("@Description").Value = TextBox4.Text
        commfault.Parameters.Add("@ReportedBy", System.Data.SqlDbType.NVarChar, 50)
        'userinfo is redundant. Replace with string.empty 23/11/16
        commfault.Parameters("@ReportedBy").Value = String.Empty
        'commfault.Parameters("@ReportedBy").Value = Userinfo
        commfault.Parameters.Add("@DateReported", System.Data.SqlDbType.DateTime)
        commfault.Parameters("@DateReported").Value = time
        commfault.Parameters.Add("@Area", System.Data.SqlDbType.NVarChar, 20)
        'Area now is text box. 23/11/16
        'commfault.Parameters("@Area").Value = DropDownListArea.SelectedItem.ToString
        commfault.Parameters("@Area").Value = AreaBox.Text
        commfault.Parameters.Add("@Energy", System.Data.SqlDbType.NVarChar, 6)
        commfault.Parameters("@Energy").Value = Energy
        commfault.Parameters.Add("@GantryAngle", System.Data.SqlDbType.NVarChar, 3)
        commfault.Parameters("@GantryAngle").Value = TextBox2.Text
        commfault.Parameters.Add("@CollimatorAngle", System.Data.SqlDbType.NVarChar, 3)
        commfault.Parameters("@CollimatorAngle").Value = TextBox3.Text
        commfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
        commfault.Parameters("@Linac").Value = MachineName
        commfault.Parameters.Add("@IncidentID", System.Data.SqlDbType.Int)
        commfault.Parameters("@IncidentID").Value = incidentID
        commfault.Parameters.Add("@BSUHID", System.Data.SqlDbType.VarChar, 7)
        commfault.Parameters("@BSUHID").Value = PatientIDBox.Text
        commfault.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 25)
        commfault.Parameters("@ConcessionNumber").Value = Defect.SelectedItem.ToString
        Try
            conn.Open()
            commfault.ExecuteNonQuery()
            conn.Close()
        Finally
            DropDownListEnergy.SelectedIndex = -1
            AreaBox.Text = String.Empty
            PatientIDBox.Text = String.Empty
            TextBox2.Text = String.Empty
            TextBox3.Text = String.Empty
            TextBox4.Text = String.Empty
            conn.Close()

        End Try


        'End If
        Defect.SelectedIndex = -1
        BindDefectData()
        'End If
    End Sub
    Protected Sub SaveDefectButton_Click(sender As Object, e As System.EventArgs) Handles SaveDefectButton.Click
        'No need for reference to WriteDatauc if no signature - March 2016
        'Back in 26/03/2108
        Dim wctrl As WriteDatauc = CType(FindControl("WriteDatauc1"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        Dim strScript As String = "<script>"
        Page.Validate("defect")
        If Page.IsValid Then
            If Defect.SelectedItem.Text = "Select" Then
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
            ElseIf Defect.SelectedItem.Text = "RAD RESET" Then
                If TextBox4.Text = "" Then
                    wctrl.Visible = False
                    strScript += "alert('Please complete the Fault Description');"
                    strScript += "</script>"
                    ScriptManager.RegisterStartupScript(SaveDefectButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                Else
                    wcbutton.Text = "Saving RAD RESET"
                    Application(actionstate) = "Confirm"
                    wctrl.Visible = True
                End If
            Else

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
        Dim newFault As Boolean
        If Not IsPostBack Then
            newFault = False
            SetFaults(newFault)
            AddEnergyItem()
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
        Dim LinacType As String
        Dim selectCommand As Boolean = newfault
        Dim Faults As New DataTable()
        Dim conn As SqlConnection
        Dim comm As SqlCommand
        Dim connectionString1 As String = ConfigurationManager.ConnectionStrings( _
        "connectionstring").ConnectionString
        conn = New SqlConnection(connectionString1)
        If newfault Then
            comm = New SqlCommand("SELECT ConcessionNumber + ' ' + ConcessionDescription As Fault, IncidentID FROM [ConcessionTable] where linac=@linac and ConcessionActive = 'TRUE' and IncidentID = (Select max(IncidentID) from  [ConcessionTable] where linac=@linac and ConcessionActive = 'TRUE') order by ConcessionNumber", conn)
            comm.Parameters.AddWithValue("@Linac", MachineName)
        Else
            'Modified 10/11/17 because defects are now in DefectTable not hard wired in to page
            'comm = New SqlCommand("SELECT ConcessionNumber + ' ' + ConcessionDescription As Fault, IncidentID FROM [ConcessionTable] where linac=@linac and ConcessionActive = 'TRUE' order by ConcessionNumber", conn)
            'comm.Parameters.AddWithValue("@Linac", MachineName)
            If MachineName Like "LA?" Then
                LinacType = "O"
            Else
                LinacType = "BE"
            End If
            comm = New SqlCommand(" SELECT  Defect as Fault, IncidentID From [DefectTable] where linacType in('A',@LinacType) and Active = 'True' UNION SELECT ConcessionNumber + ' ' + ConcessionDescription As Fault, IncidentID FROM [ConcessionTable] where linac=@linac and ConcessionActive = 'TRUE' order by IncidentID", conn)
            comm.Parameters.AddWithValue("@Linac", MachineName)
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
        Dim incidentIDstring As String = ""
        Dim incidentID As Integer
        Dim conn1 As SqlConnection
        Dim comm1 As SqlCommand
        Dim Region As String = ""
        Dim connectionString As String = ConfigurationManager.ConnectionStrings( _
        "connectionstring").ConnectionString

        incidentIDstring = Defect.SelectedItem.Value
        Dim result As ListItem
        result = Defect.Items.FindByValue(incidentIDstring)
        Dim index As Integer
        index = Defect.Items.IndexOf(result)
        If Integer.TryParse(incidentIDstring, incidentID) Then
            HiddenField1.Value = incidentID
            'Modified 10/1117 because of use of defect table
            'If incidentID > 0 Then
            '    conn1 = New SqlConnection(connectionString)
            '    'from http://www.sqlservercentral.com/Forums/Topic1416029-1292-1.aspx
            '    comm1 = New SqlCommand("SELECT Area from ReportFault where incidentID=@incidentID", conn1)
            '    comm1.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
            '    comm1.Parameters("@incidentID").Value = incidentID
            '    conn1.Open()
            '    Dim sqlresult As Object = comm1.ExecuteScalar()
            '    If sqlresult Is Nothing Then
            '        AreaBox.Text = String.Empty
            '        'DropDownListArea.SelectedIndex = -1

            '    Else
            '        AreaBox.Text = sqlresult.ToString
            '        'DropDownListArea.SelectedValue = sqlresult.ToString
            '        'DropDownListArea.Enabled = False
            '    End If
            '    conn1.Close()
            'Else
            '    
            '    'If incidentID = -10 Or incidentID = -13 Then
            '    '    AreaBox.Text = "iView"
            '    '    'DropDownListArea.SelectedValue = "iView"
            '    '    'DropDownListArea.Enabled = False
            '    '    'Modified 21/11/2016 to automatically add Area
            '    'ElseIf incidentID = -14 Then
            '    '    AreaBox.Text = "XVI"
            '    '    'DropDownListArea.SelectedValue = "XVI"
            '    '    'DropDownListArea.Enabled = False
            '    'Else
            '    '    AreaBox.Text = "Machine"
            '    '    'DropDownListArea.SelectedValue = "Machine"
            '    '    'DropDownListArea.Enabled = False
            '    'End If

            'End If
            conn1 = New SqlConnection(connectionString)
            If incidentID > 0 Then
                comm1 = New SqlCommand("SELECT Area from ReportFault where incidentID=@incidentID", conn1)
                Else
                comm1 = New SqlCommand("SELECT Area from DefectTable where incidentID=@incidentID", conn1)
            End If
            comm1.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
            comm1.Parameters("@incidentID").Value = incidentID
            conn1.Open()
            Dim sqlresult As Object = comm1.ExecuteScalar()
            If sqlresult Is Nothing Then
                AreaBox.Text = String.Empty
            Else

                AreaBox.Text = sqlresult.ToString
            End If
            conn1.Close()

        Else
            HiddenField1.Value = -1000
            AreaBox.Text = String.Empty
        End If

    End Sub
    Protected Sub AddEnergyItem()
        'from http://www.aspsnippets.com/Articles/Programmatically-add-items-to-DropDownList-on-Button-Click-in-ASPNet-using-C-and-VBNet.aspx
        'and http://www.yaldex.com/asp_tutorial/0596004877_progaspdotnet2-chp-5-sect-7.html

        'This is a mad way of doing it but I don't know how to dim the energy list without constructing it at the same time

        Select Case MachineName
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
        Defect.SelectedIndex = -1
        AreaBox.Text = String.Empty
        DropDownListEnergy.SelectedIndex = -1
        TextBox2.Text = Nothing
        TextBox3.Text = Nothing
        TextBox4.Text = Nothing
        PatientIDBox.Text = Nothing
        
    End Sub
End Class

Imports System.Data.SqlClient
Imports System.Data
Imports System.Transactions

Partial Class DefectSave
    Inherits System.Web.UI.UserControl
    Const EMPTYSTRING As String = ""
    Private actionstate As String
    Const RADRESET = -21
    Const MAJORFAULT = -24
    Public Event UpdateViewOpenFaults(ByVal EquipmentName As String)
    Public Property ParentControl() As String
    Public Property LinacName() As String
    Private appstate As String
    Private suspstate As String
    Private repairstate As String
    Private failstate As String
    Private Valid As Boolean = False
    Const RADIO As Integer = 103
    Dim ConcessionNumber As String = ""
    Dim SelectedIncident As Integer = 0


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
    'This updates the defect display for the day when a repeat fault is registered by viewopenfaults and then planned maintenance etc.
    Public Sub UpDateDefectsEventHandler()
        BindDefectData()
        'SetFaults()
    End Sub

    'No need to pass any references now or to have if statements. Analysis 23/11/16 Back in 29/03/18

    Protected Sub UserApprovedEvent(ByVal Tabused As String, ByVal Userinfo As String)
        Dim Action As String = Application(actionstate)
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString

        ConcessionNumber = Defect.SelectedItem.ToString
        If ConcessionNumber.Contains("ELF") Then
            ConcessionNumber = Left(ConcessionNumber, 7)
        End If
        If Tabused = "Defect" Or Tabused = "Major" Then
            Dim wctrl1 As WriteDatauc = CType(FindControl("WriteDatauc1"), WriteDatauc)
            wctrl1.Visible = False
            Dim wctrl2 As WriteDatauc = CType(FindControl("WriteDatauc2"), WriteDatauc)
            wctrl2.Visible = False
            If Action = "Confirm" Then
                'What happens if this fails
                NewWriteradreset(Userinfo, connectionString)

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
            If Defect.SelectedItem.Value = RADRESET Then
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
            'reset the validation controls
            FaultDescriptionValidation.ValidationGroup = ""
            RadActValidation.ValidationGroup = ""
            AreaValidation.ValidationGroup = ""
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

        Dim incidentIDstring As String = ""
        Dim conn As SqlConnection
        Dim comm1 As SqlCommand
        Dim Region As String = ""
        Dim Queryreturn As String = ""
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
            If Integer.TryParse(incidentIDstring, SelectedIncident) Then
                DropDownListArea.SelectedIndex = -1
                SelectedIncidentID.Value = SelectedIncident
                TimeFaultSelected.Value = Now().ToString
                conn = New SqlConnection(connectionString)
                conn.Open()
                If SelectedIncident > 0 Then
                    comm1 = New SqlCommand("Select r.Area, c.Action from ReportFault r left outer join ConcessionTable c on r.incidentid = c.incidentid where r.incidentID=@incidentID", conn)
                    comm1.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    comm1.Parameters("@incidentID").Value = SelectedIncident
                    reader = comm1.ExecuteReader()
                    If reader.Read() Then
                        Area = reader.Item("Area")
                        Action = reader.Item("Action")
                    End If
                    reader.Close()
                    conn.Close()
                    DropDownListArea.SelectedItem.Text = Area
                    AreaOrAccuray.Value = Area
                    DropDownListArea.Enabled = False
                    RadAct.Text = Action
                    RadAct.ReadOnly = True

                ElseIf (SelectedIncident = RADRESET) Then
                    DropDownListArea.Enabled = True
                    DropDownListArea.SelectedValue = "Select"
                    RadAct.ReadOnly = False
                    FaultDescriptionValidation.ValidationGroup = "defect"
                    RadActValidation.ValidationGroup = "defect"
                    AreaValidation.ValidationGroup = "defect"
                ElseIf (SelectedIncident = MAJORFAULT) Then
                    DropDownListArea.Enabled = True
                    DropDownListArea.SelectedValue = "Select"
                    RadAct.ReadOnly = False
                    FaultDescriptionValidation.ValidationGroup = "defect"
                    AreaValidation.ValidationGroup = "defect"
                Else
                    comm1 = New SqlCommand("SELECT Area from DefectTable where incidentID=@incidentID", conn)
                    comm1.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    comm1.Parameters("@incidentID").Value = SelectedIncident

                    Dim sqlresult As Object = comm1.ExecuteScalar()
                    conn.Close()
                    Area = sqlresult.ToString()
                    DropDownListArea.SelectedItem.Text = Area
                    AreaOrAccuray.Value = Area
                    DropDownListArea.Enabled = False
                End If

                SaveDefectButton.Enabled = True
                SaveDefectButton.BackColor = Drawing.Color.Yellow
            Else
                SelectedIncidentID.Value = -1000
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

        AreaOrAccuray.Value = DropDownListArea.SelectedValue.ToString
    End Sub



    Protected Sub NewWriteradreset(ByVal UserInfo As String, ByVal connectionString As String)

        Dim Concession As String = "Concession"
        Dim Status As String = EMPTYSTRING
        Dim Result As Boolean = False
        Dim FaultParams As DavesCode.FaultParameters = New DavesCode.FaultParameters()
        SelectedIncident = SelectedIncidentID.Value
        CreateFaultParams(UserInfo, FaultParams)
        Select Case SelectedIncident
            Case RADRESET
                Result = DavesCode.NewFaultHandling.InsertNewFault("Concession", FaultParams)
                If Result Then
                    Status = Concession
                    SetFaults(True)
                    BindDefectData()
                    'This updates the concession list
                    RaiseEvent UpdateViewOpenFaults(LinacName)
                Else
                    RaiseError()
                End If

            Case MAJORFAULT

                Dim susstate As String = Application(suspstate)
                Dim repstate As String = Application(repairstate)
                Dim DaTxtBox As TextBox = Me.Parent.FindControl("CommentBox")
                Dim Comment As String = DaTxtBox.Text
                Dim GridViewE As GridView = Me.Parent.FindControl("Gridview1")
                Dim grdviewI As GridView = Me.Parent.FindControl("GridViewImage")
                Dim iView As Boolean = False
                Dim XVI As Boolean = False

                Select Case ParentControl

                    Case 1, 7
                        Result = DavesCode.NewEngRunup.CommitRunup(GridViewE, grdviewI, LinacName, ParentControl, UserInfo, Comment, Valid, True, False, FaultParams)

                    Case 2
                        DavesCode.Reuse.ReturnImaging(iView, XVI, grdviewI, LinacName)
                        Result = DavesCode.NewPreClinRunup.CommitPreClin(LinacName, UserInfo, Comment, iView, XVI, Valid, True, FaultParams)

                    Case 3
                        Result = DavesCode.NewCommitClinical.CommitClinical(LinacName, UserInfo, True, FaultParams)
                        Application(suspstate) = 1

                    Case 4, 5, 6, 8
                        Result = DavesCode.NewWriteAux.WriteAuxTables(LinacName, UserInfo, Comment, RADIO, ParentControl, True, susstate, repstate, False, FaultParams)

                    Case Else
                        'Put up error message


                End Select

                If Result Then
                    Application(appstate) = Nothing
                    Application(failstate) = ParentControl
                    'https://support.microsoft.com/en-us/help/312629/prb-threadabortexception-occurs-if-you-use-response-end-response-redir
                    PopupAck()
                    Dim returnstring As String = LinacName + "page.aspx?pageref=Fault&Tabindex="
                    Response.Redirect(returnstring & ParentControl & "&comment=" & Comment, False)

                Else
                    RaiseError()
                End If
            Case Else
                Result = DavesCode.NewFaultHandling.InsertRepeatFault(FaultParams)
                If Result Then
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
        FaultParams.SelectedIncident = SelectedIncident
        FaultParams.Linac = LinacName
        FaultParams.DateInserted = DateTime.Parse(TimeFaultSelected.Value)
        FaultParams.UserInfo = UserInfo
        FaultParams.Area = AreaOrAccuray.Value
        FaultParams.Energy = Energy
        FaultParams.GantryAngle = GantryAngleBox.Text
        FaultParams.CollimatorAngle = CollimatorAngleBox.Text
        FaultParams.PatientID = PatientIDBox.Text
        FaultParams.FaultDescription = FaultDescription.Text
        FaultParams.ConcessionNumber = ConcessionNumber
        FaultParams.RadAct = RadAct.Text
        FaultParams.RadioIncident = RadioIncident.SelectedItem.Value

    End Sub


    Private Sub ForceFocus(ByVal ctrl As Control)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "FocusScript", "setTimeout(function(){$get('" + ctrl.ClientID + "').focus();}, 100);", True)
    End Sub

End Class

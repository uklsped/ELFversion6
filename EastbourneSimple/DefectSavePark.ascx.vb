Imports System.Data.SqlClient
Imports System.Data

Partial Class DefectSavePark
    Inherits System.Web.UI.UserControl
    Private appstate As String
    Private actionstate As String
    Private suspstate As String
    Private failstate As String
    Private repairstate As String
    Const RADIO As Integer = 103
    Private time As DateTime
    Const RecoverableFault As String = "Recoverable Fault"
    Const UnRecoverableFault As String = "UnRecoverable Fault"
    Const UnRecoverableID As Integer = -23
    Const FaultAnswerNo As String = "No"
    Const FaultAnswerYes As String = "Yes"
    Const Concession As String = "Concession"
    Const Closed As String = "Closed"
    Const Radiographer As Integer = 3
    Const EMPTYSTRING As String = ""
    Public Event UpDateDefect(ByVal EquipmentName As String, ByVal incidentID As String)
    Public Event UpdateViewFault(ByVal EquipmentName As String)
    Public Event UpdateUnrecoverableClosed(ByVal EquipmentName As String)
    Private Valid As Boolean = False

    Public Property LinacName() As String
    Public Property ParentControl() As String
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Remove reference to this as no longer used after March 2016 done on 23/11/16
        'Added back in 26/3/18 see SPR
        AddHandler WriteDatauc1.UserApproved, AddressOf UserApprovedEvent
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
        'Dim Energy As String

        Dim ConcessionNumber As String = Defect.SelectedItem.ToString
        If ConcessionNumber.Contains("ELF") Then
            ConcessionNumber = Left(ConcessionNumber, 7)
        End If
        If Tabused = "Defect" Then
            Dim wctrl As WriteDatauc = CType(FindControl("WriteDatauc1"), WriteDatauc)
            wctrl.Visible = False
            If Action = "Confirm" Then
                'BindDefectData()

                NewWriteRadReset(Userinfo, ConcessionNumber)
            Else
                Defect.SelectedIndex = -1
                ClearsForm()
            End If
        End If
    End Sub

    Protected Sub SaveDefectButton_Click(sender As Object, e As System.EventArgs) Handles SaveDefectButton.Click
        'Dim b As Button = TryCast(sender, Button)
        'Dim i As RadioButtonList
        'If b Is Nothing Then
        '    i = CType(sender, RadioButtonList)
        'End If

        Page.Validate("defect")
        If Page.IsValid Then
            'If (TypeOf sender Is Button) Then
            Application(actionstate) = "Confirm"
            UserApprovedEvent("Defect", "")

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

            comm = New SqlCommand(" SELECT  Defect as Fault, IncidentID From [DefectTable] where linacType in('T') and Active = 'True' UNION SELECT ConcessionNumber + ' ' + ConcessionDescription As Fault, IncidentID FROM [ConcessionTable] where linac=@linac and ConcessionActive = 'TRUE' order by IncidentID", conn)
            comm.Parameters.AddWithValue("@Linac", LinacName)

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
        Dim incidentID As Integer
        Dim conn1 As SqlConnection
        Dim comm1 As SqlCommand
        Dim DefectString As String = ""
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        ClearsForm()
        incidentIDstring = Defect.SelectedItem.Value
        DefectString = Defect.SelectedItem.Text
        Dim result As ListItem
        result = Defect.Items.FindByValue(incidentIDstring)
        Dim index As Integer
        index = Defect.Items.IndexOf(result)
        If Integer.TryParse(incidentIDstring, incidentID) Then

            HiddenField1.Value = incidentID
            HiddenField2.Value = Now().ToString

            conn1 = New SqlConnection(connectionString)
            If incidentID > 0 Then 'concession
                comm1 = New SqlCommand("SELECT Action FROM ConcessionTable where incidentID=@incidentID", conn1) 'Corrective action

                comm1.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                comm1.Parameters("@incidentID").Value = incidentID

                conn1.Open()

                Dim sqlresult1 As Object = comm1.ExecuteScalar()
                RadAct.Text = sqlresult1.ToString
                RadAct.ReadOnly = True
                FaultTypeSave.SetActiveView(RecoverableView)
                conn1.Close()

            ElseIf Not String.IsNullOrEmpty(DefectString) Then
                If DefectString = RecoverableFault Then
                    FaultTypeSave.SetActiveView(RecoverableView)
                ElseIf DefectString = UnRecoverableFault Then
                    FaultTypeSave.SetActiveView(UnRecoverableView)
                End If

            End If

        Else
            HiddenField1.Value = -1000
        End If

    End Sub


    Protected Sub ClearButton_Click(sender As Object, e As System.EventArgs) Handles ClearButton.Click
        Defect.SelectedIndex = -1
        ClearsForm()
    End Sub

    Protected Sub ClearsForm()
        FaultTypeSave.ActiveViewIndex = -1
        FaultOpenClosed.SelectedIndex = -1
        ErrorCode.Text = Nothing
        Accuray.Text = Nothing
        FaultDescription.Text = Nothing
        PatientIDBox.Text = Nothing
        RadAct.Text = Nothing
    End Sub


    'Protected Sub Writeradreset(ByVal UserInfo As String, ByVal ConcessionNumber As String)

    '    Dim usergroupselected As Integer = 0
    '    Dim LastIncident As Integer
    '    Dim IncidentID As Integer
    '    Dim Selected As String = EMPTYSTRING

    '    Dim Status As String = EMPTYSTRING

    '    LastIncident = HiddenField1.Value
    '    If LastIncident = UnRecoverableID Then

    '        Selected = FaultOpenClosed.SelectedItem.Text
    '        If Selected.Equals(FaultAnswerYes) Then
    '            'Open new fault
    '            IncidentID = CreateNewFault(UserInfo, Status)
    '            usergroupselected = DavesCode.Reuse.GetRole(UserInfo)
    '            'If usertype = rad then create concession else
    '            If usergroupselected.Equals(Radiographer) Then
    '                Status = Concession
    '                CreateNewConcession(UserInfo, IncidentID)
    '                BindDefectData()
    '                RaiseEvent UpDateDefect(LinacName, IncidentID)
    '                RaiseEvent UpdateViewFault(LinacName)
    '            Else
    '                'close fault
    '                CloseFault(IncidentID, UserInfo)
    '            End If

    '            'RaiseEvent UpDateDefect(LinacName, IncidentID)
    '            'RaiseEvent UpdateViewFault(LinacName)

    '            'Unrecoverable fault isn't closed
    '        ElseIf Selected.Equals(FaultAnswerNo) Then
    '            'Write equivalent of report fault assume only one fault at a time at the moment
    '            'Close current tab
    '            Dim result As String
    '            appstate = "LogOn" + LinacName
    '            Select Case ParentControl

    '                Case 1, 7
    '                    Dim DaTxtBox As TextBox = Me.Parent.FindControl("CommentBox")
    '                    Dim Comment As String = DaTxtBox.Text
    '                    result = DavesCode.ReusePC.CommitRunup(LinacName, ParentControl, UserInfo, Comment, Valid, True, False)
    '                    Application(appstate) = Nothing
    '                    CreateNewFault(UserInfo, Status)

    '                    Dim returnstring As String = LinacName + "page.aspx?pageref=Fault&Tabindex="
    '                    Response.Redirect(returnstring & ParentControl & "&comment=" & "")
    '            End Select

    '        Else
    '            'Error action
    '        End If

    '    Else 'This is a recoverable fault
    '        DavesCode.ReusePC.InsertReportFault(FaultDescription.Text, UserInfo, DateTime.Parse(HiddenField2.Value), Accuray.Text, ErrorCode.Text, EMPTYSTRING, EMPTYSTRING, LinacName, IncidentID, PatientIDBox.Text, ConcessionNumber)
    '        BindDefectData()
    '    End If

    '    Defect.SelectedIndex = -1
    '    ClearsForm()

    'End Sub

    Sub NewWriteRadReset(ByVal UserInfo As String, ByVal ConcessionNumber As String)
        Dim usergroupselected As Integer = 0
        Dim LastIncident As Integer
        Dim IncidentID As Integer
        Dim Selected As String = EMPTYSTRING

        Dim Status As String = EMPTYSTRING

        LastIncident = HiddenField1.Value
        If LastIncident = UnRecoverableID Then

            Selected = FaultOpenClosed.SelectedItem.Text
            If Selected.Equals(FaultAnswerYes) Then
                usergroupselected = DavesCode.Reuse.GetRole(UserInfo)
                If usergroupselected.Equals(Radiographer) Then
                    IncidentID = CreateNewFault(UserInfo, "Concession")
                    Status = Concession
                    BindDefectData()
                    RaiseEvent UpDateDefect(LinacName, IncidentID)
                    RaiseEvent UpdateViewFault(LinacName)
                Else
                    IncidentID = CreateNewFault(UserInfo, "Closed")
                End If

                'Unrecoverable fault isn't closed
            ElseIf Selected.Equals(FaultAnswerNo) Then
                'Write equivalent of report fault assume only one fault at a time at the moment
                'Close current tab
                Dim result As String
                Dim susstate As String = Application(suspstate)
                Dim repstate As String = Application(repairstate)
                Dim DaTxtBox As TextBox = Me.Parent.FindControl("CommentBox")
                Dim Comment As String = DaTxtBox.Text

                Select Case ParentControl

                    Case 1, 7
                        result = DavesCode.ReusePC.CommitRunup(LinacName, ParentControl, UserInfo, Comment, Valid, True, False)

                    Case 3
                        DavesCode.Reuse.CommitClinical(LinacName, UserInfo, True)
                        Application(suspstate) = 1

                    Case 4, 5, 6, 8
                        DavesCode.Reuse.WriteAuxTables(LinacName, UserInfo, Comment, RADIO, ParentControl, True, susstate, repstate, False)

                    Case Else
                        Application(failstate) = ParentControl
                        DavesCode.Reuse.WriteAuxTables(LinacName, UserInfo, Comment, RADIO, ParentControl, True, susstate, repstate, False)

                End Select
                Application(appstate) = Nothing
                CreateNewFault(UserInfo, "New")

                Dim returnstring As String = LinacName + "page.aspx?pageref=Fault&Tabindex="
                Response.Redirect(returnstring & ParentControl & "&comment=" & "")
            Else
                'Error Action
            End If

        Else 'This is a recoverable fault - So won't have concession number?
            IncidentID = HiddenField1.Value
            DavesCode.ReusePC.InsertReportFault(FaultDescription.Text, UserInfo, DateTime.Parse(HiddenField2.Value), Accuray.Text, ErrorCode.Text, EMPTYSTRING, EMPTYSTRING, LinacName, IncidentID, PatientIDBox.Text, ConcessionNumber)
            BindDefectData()
        End If

        Defect.SelectedIndex = -1
        ClearsForm()
    End Sub

    'Protected Sub CloseFault(ByVal IncidentID As Integer, UserInfo As String)
    '    Dim TrackAction = RadAct.Text
    '    Dim Status As String = Closed
    '    Dim TrackingComment As String = ""
    '    Dim Assigned As String = EMPTYSTRING

    '    DavesCode.ReusePC.UpdateTracking(TrackingComment, Assigned, Status, UserInfo, LinacName, TrackAction, IncidentID)
    '    DavesCode.ReusePC.UpdateFaultIDTable(IncidentID, Status, LinacName)
    'where should these be?
    'RaiseEvent UpdateUnrecoverableClosed()
    'RaiseEvent UpDateDefect(LinacName, IncidentID)
    'End Sub

    'Protected Sub CreateNewConcession(ByVal UserInfo As String, ByVal IncidentID As Integer)
    '    Dim Inserted As Integer
    '    Dim TrackAction As String = RadAct.Text
    '    Const concession = "Concession"
    'Inserted = DavesCode.ReusePC.InsertNewConcession(FaultDescription.Text, LinacName, IncidentID, RadAct.Text)
    'If Inserted = 0 Then
    'need to write faultidtable again and faulttracking table again and set rad concession flag
    'DavesCode.ReusePC.UpdateTracking(EMPTYSTRING, EMPTYSTRING, concession, UserInfo, LinacName, TrackAction, IncidentID)
    'DavesCode.ReusePC.UpdateFaultIDTable(IncidentID, concession, LinacName)
    'DavesCode.ReusePC.WriteRadAckFault(IncidentID, False)

    ' Else
    ' create concession has failed
    ' End If

    'End Sub

    Protected Function CreateNewFault(ByVal UserInfo As String, ByVal State As String) As Integer
        Dim IncidentID As Integer = 0
        Dim DateInserted As DateTime = DateTime.Parse(HiddenField2.Value)
        Dim Description As String = FaultDescription.Text
        Dim ConcessionNumber = EMPTYSTRING
        Dim Assigned = EMPTYSTRING
        Dim TrackingComment = EMPTYSTRING
        Dim TrackAction = EMPTYSTRING

        IncidentID = DavesCode.ReusePC.InsertNewFault(State, LinacName, DateInserted, Description, UserInfo, Accuray.Text, ErrorCode.Text, EMPTYSTRING, EMPTYSTRING, PatientIDBox.Text, FaultDescription.Text, RadAct.Text)
        Return IncidentID
    End Function

    Private Sub ForceFocus(ByVal ctrl As Control)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "FocusScript", "setTimeout(function(){$get('" + ctrl.ClientID + "').focus();}, 100);", True)
    End Sub

    Protected Sub FaultOpenClosed_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FaultOpenClosed.SelectedIndexChanged
        Dim Selected As String = ""
        Selected = FaultOpenClosed.SelectedItem.Text
        UnRecoverableSave.Enabled = True
        UnRecoverableSave.BackColor = Drawing.Color.Yellow
        AccurayValidation.ValidationGroup = "defect"
        FaultDescriptionValidation.ValidationGroup = "defect"
        CorrectiveActionValidation.ValidationGroup = "defect"
    End Sub
    Protected Sub UnRecoverableSave_Click(sender As Object, e As EventArgs) Handles UnRecoverableSave.Click
        Dim wctrl As WriteDatauc = CType(FindControl("WriteDatauc1"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)

        'If (TypeOf sender Is RadioButtonList) Then
        'AccurayValidation.ValidationGroup = "defect"
        '    FaultDescriptionValidation.ValidationGroup = "defect"
        '    CorrectiveActionValidation.ValidationGroup = "defect"
        'End If
        Page.Validate("defect")
        If Page.IsValid Then
            Dim Selected As String = ""
            UnRecoverableSave.BackColor = Drawing.Color.LightGray
            UnRecoverableSave.Enabled = False
            Selected = FaultOpenClosed.SelectedItem.Text
            If Selected.Equals(FaultAnswerNo) Then
                wcbutton.Text = "Saving New Fault"
                Application(actionstate) = "Confirm"
                wctrl.Visible = True
                ForceFocus(wctext)
                'Open equivalent of fault
            ElseIf Selected.Equals(FaultAnswerYes) Then
                'Write equivalent of radreset
                wcbutton.Text = "Closing Unrecoverable Fault"
                Application(actionstate) = "Confirm"
                wctrl.Visible = True
                ForceFocus(wctext)
            Else
                'Error action
            End If

        End If
    End Sub

End Class

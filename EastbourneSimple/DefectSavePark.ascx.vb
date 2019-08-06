﻿Imports System.Data.SqlClient
Imports System.Data
Imports System.Transactions

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
    Const RadioIncidentAnswerYes As String = "Yes"
    Const RadioIncidentAnswerNo As String = "No"
    Const Concession As String = "Concession"
    Const Closed As String = "Closed"
    Const Radiographer As Integer = 3
    Const EMPTYSTRING As String = ""
    'Public Event UpdateViewOpenFaults(ByVal EquipmentName As String)
    'Public Event UpdateFaultClosedDisplays(ByVal EquipmentName As String, ByVal IncidentID As String)
    Private Valid As Boolean = False
    Dim ConcessionNumber As String = ""
    Private FaultDescriptionChanged As String
    Private RadActDescriptionChanged As String
    Private Comment As String
    Private RadActComment As String
    Dim SelectedIncident As Integer = 0
    Public Property LinacName() As String
    Public Property ParentControl() As String
    Public Property ParentControlComment() As String
    Public Event UpDateDefectDailyDisplay(ByVal EquipmentName As String)
    Public Event CloseReportFaultPopUp(ByVal EquipmentName As String)
    Public Event UpdateViewOpenFaults(ByVal EquipmentName As String)
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Remove reference to this as no longer used after March 2016 done on 23/11/16
        'Added back in 26/3/18 see SPR
        AddHandler WriteDatauc1.UserApproved, AddressOf UserApprovedEvent
        appstate = "LogOn" + LinacName
        actionstate = "ActionState" + LinacName
        suspstate = "Suspended" + LinacName
        failstate = "FailState" + LinacName
        repairstate = "rppTab" + LinacName
        FaultDescriptionChanged = "defectFault" + LinacName
        RadActDescriptionChanged = "radact" + LinacName
    End Sub

    Public Sub InitialiseDefectPage()
        Dim newFault As Boolean
        FaultDescription.BoxChanged = FaultDescriptionChanged
        RadActC.BoxChanged = RadActDescriptionChanged
        newFault = False
        SetFaults(newFault)
        RadioIncident.SelectedIndex = -1
        Dim clear As Button = FindControl("ClearButton")

    End Sub

    'Public Sub UpDateDefectsEventHandler()
    '    BindDefectData()
    '    'SetFaults()
    'End Sub

    'No need to pass any references now or to have if statements. Analysis 23/11/16 Back in 29/03/18

    Protected Sub UserApprovedEvent(ByVal Tabused As String, ByVal Userinfo As String)
        Dim Action As String = Application(actionstate)
        'Dim Energy As String

        ConcessionNumber = Defect.SelectedItem.ToString
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

            End If
            ClearsForm()
            RaiseEvent UpdateViewOpenFaults(LinacName)
            RaiseEvent CloseReportFaultPopUp(LinacName)
        End If
    End Sub

    Protected Sub SaveDefectButton_Click(sender As Object, e As System.EventArgs) Handles SaveDefectButton.Click
        'Dim b As Button = TryCast(sender, Button)
        'Dim i As RadioButtonList
        'If b Is Nothing Then
        '    i = CType(sender, RadioButtonList)
        'End If
        If Defect.SelectedItem.Text = RecoverableFault Then
            FaultDescription.SetValidation("Tomodefect", "Please Enter a fault description")
            'RadActC.SetValidation("Tomodefect", "Please Enter the Corrective Action Taken")
        End If

        Page.Validate("Tomodefect")
        If Page.IsValid Then
            'If (TypeOf sender Is Button) Then
            Application(actionstate) = "Confirm"
            UserApprovedEvent("Defect", "")
        Else
            For Each validator In Page.Validators
                If (Not validator.IsValid) Then
                    'validator that failed found so set the focus to the control
                    'it validates and exit the loop
                    ForceFocus(validator.FindControl(validator.ControlToValidate))
                    Exit For
                End If
            Next validator

        End If

    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        SaveDefectButton.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(SaveDefectButton, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
        ClearButton.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(ClearButton, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
        actionstate = "ActionState" + LinacName
        'Dim newFault As Boolean
        'FaultDescription.BoxChanged = FaultDescriptionChanged
        'RadActC.BoxChanged = RadActDescriptionChanged
        'If Not IsPostBack Then
        '    newFault = False
        '    SetFaults(newFault)
        '    RadioIncident.SelectedIndex = -1
        'End If
        'WriteDatauc1 no longer used 23/11/16
        'Added back in for RAD RESET 26/3/18 SEE SPR
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        wctrl.LinacName = LinacName
        FaultDescription.BoxChanged = FaultDescriptionChanged
        RadActC.BoxChanged = RadActDescriptionChanged
        'BindDefectData()
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


    'Private Sub BindDefectData()

    '    Dim SqlDataSource1 As New SqlDataSource()
    '    Dim query As String = "SELECT RIGHT(CONVERT(VARCHAR, DateReported, 100),7) as DefectTime, ConcessionNumber, Description FROM [ReportFault] where Cast(DateReported as Date) = Cast(GetDate() as Date) and linac=@linac and ConcessionNumber != '' order by DateReported desc"
    '    SqlDataSource1 = QuerySqlConnection(LinacName, query)
    '    GridView1.DataSource = SqlDataSource1
    '    GridView1.DataBind()
    'End Sub

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
        'ClearsForm()
        incidentIDstring = Defect.SelectedItem.Value
        DefectString = Defect.SelectedItem.Text
        Dim result As ListItem
        result = Defect.Items.FindByValue(incidentIDstring)
        Dim index As Integer
        index = Defect.Items.IndexOf(result)
        If Integer.TryParse(incidentIDstring, incidentID) Then

            SelectedIncidentID.Value = incidentID
            TimeFaultSelected.Value = Now().ToString

            conn1 = New SqlConnection(connectionString)
            If incidentID > 0 Then 'concession
                comm1 = New SqlCommand("SELECT Action FROM ConcessionTable where incidentID=@incidentID", conn1) 'Corrective action

                comm1.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                comm1.Parameters("@incidentID").Value = incidentID

                conn1.Open()

                Dim sqlresult1 As Object = comm1.ExecuteScalar()
                'RadAct.Text = sqlresult1.ToString
                'RadAct.ReadOnly = True
                RadActC.ResetCommentBox(sqlresult1.ToString)
                'FaultTypeSave.SetActiveView(RecoverableView)
                FaultClosedLabel.Visible = False
                FaultOpenClosed.Visible = False
                UnRecoverableSave.Visible = False
                SaveDefectButton.Visible = True
                SaveDefectButton.Enabled = True
                SaveDefectButton.BackColor = Drawing.Color.Yellow
                conn1.Close()

            ElseIf Not String.IsNullOrEmpty(DefectString) Then
                If DefectString = RecoverableFault Then
                    'FaultTypeSave.SetActiveView(RecoverableView)
                    FaultClosedLabel.Visible = False
                    FaultOpenClosed.Visible = False
                    UnRecoverableSave.Visible = False
                    SaveDefectButton.Visible = True
                    SaveDefectButton.Enabled = True
                    SaveDefectButton.BackColor = Drawing.Color.Yellow
                ElseIf DefectString = UnRecoverableFault Then
                    FaultClosedLabel.Visible = True
                    FaultOpenClosed.Visible = True
                    SaveDefectButton.Visible = False

                    'FaultTypeSave.SetActiveView(UnRecoverableView)
                    ActPanel.Enabled = False
                End If

            End If
            FaultPanel.Enabled = True

        Else
            SelectedIncidentID.Value = -1000
        End If

    End Sub

    Protected Sub ClearButton_Click(sender As Object, e As System.EventArgs) Handles ClearButton.Click
        'Defect.SelectedIndex = -1
        'ClearsForm()
        RaiseEvent CloseReportFaultPopUp(LinacName)
    End Sub

    Protected Sub ClearsForm()
        'FaultTypeSave.ActiveViewIndex = -1
        FaultOpenClosed.SelectedIndex = -1
        RadioIncident.SelectedIndex = -1
        ErrorCode.Text = Nothing
        Accuray.Text = Nothing
        FaultDescription.ResetCommentBox(EMPTYSTRING)
        PatientIDBox.Text = Nothing
        RadActC.ResetCommentBox(EMPTYSTRING)
        SaveDefectButton.BackColor = Drawing.Color.LightGray
        SaveDefectButton.Enabled = False
        FaultPanel.Enabled = False
        ActPanel.Enabled = False
    End Sub

    Sub NewWriteRadReset(ByVal UserInfo As String, ByVal ConcessionNumber As String)
        appstate = "LogOn" + LinacName
        actionstate = "ActionState" + LinacName
        suspstate = "Suspended" + LinacName
        failstate = "FailState" + LinacName
        repairstate = "rppTab" + LinacName
        FaultDescriptionChanged = "defectFault" + LinacName
        RadActDescriptionChanged = "radact" + LinacName
        'FaultApplication = "FaultParams" + LinacName
        Dim Result As Boolean = False
        Dim usergroupselected As Integer = 0
        'Dim IncidentID As Integer
        Dim FaultSelected As String = EMPTYSTRING
        Dim GridViewEnergy As GridView
        Dim GridViewImage As GridView
        Dim grdviewI As GridView = Me.Parent.FindControl("GridViewImage")
        Dim Status As String = EMPTYSTRING
        Dim FaultParams As DavesCode.FaultParameters = New DavesCode.FaultParameters()
        SelectedIncident = SelectedIncidentID.Value
        CreateFaultParams(UserInfo, FaultParams)
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        'Try

        'Using myscope As TransactionScope = New TransactionScope()

        If SelectedIncident = UnRecoverableID Then

            FaultSelected = FaultOpenClosed.SelectedItem.Text
            'If FaultSelected.Equals(FaultAnswerYes) Then
            Select Case FaultSelected
                Case FaultAnswerYes
                    usergroupselected = DavesCode.Reuse.GetRole(UserInfo)
                    If usergroupselected.Equals(Radiographer) Then
                        Result = DavesCode.NewFaultHandling.InsertNewFault("Concession", FaultParams)
                        If Result Then
                            Status = Concession
                            SetFaults(True)
                            'BindDefectData()
                            RaiseEvent UpdateViewOpenFaults(LinacName)
                        Else
                            RaiseError()
                        End If
                    Else
                        Result = DavesCode.NewFaultHandling.InsertNewFault("Closed", FaultParams)
                        If Result Then
                            RaiseEvent UpDateDefectDailyDisplay(LinacName)
                            'RaiseEvent UpdateFaultClosedDisplays(LinacName, IncidentID)
                        Else
                            RaiseError()
                        End If
                    End If
                    'Unrecoverable fault isn't closed
                Case FaultAnswerNo
                    'ElseIf FaultSelected.Equals(FaultAnswerNo) Then
                    'Write equivalent of report fault assume only one fault at a time at the moment
                    'Close current tab

                    Dim susstate As String = Application(suspstate)
                    Dim repstate As String = Application(repairstate)
                    'This gets comment box from tab that defectsave is on
                    'Dim ParentCommentControl As controls_CommentBoxuc = Me.Parent.FindControl("CommentBox")
                    'Dim DaTxtBox As TextBox = ParentCommentControl.FindControl("TextBox")
                    'Dim Comment As String = DaTxtBox.Text
                    Dim ParentControlComment As String = Application("TabComment")
                    Dim iView As Boolean = False
                    Dim XVI As Boolean = False

                    Select Case ParentControl

                        Case 1, 7
                            Result = DavesCode.NewEngRunup.CommitRunup(GridViewEnergy, GridViewImage, LinacName, ParentControl, UserInfo, ParentControlComment, Valid, True, False, FaultParams)

                        Case 2
                            DavesCode.Reuse.ReturnImaging(iView, XVI, grdviewI, LinacName)
                            Result = DavesCode.NewPreClinRunup.CommitPreClin(LinacName, UserInfo, ParentControlComment, iView, XVI, Valid, True, FaultParams)
                        Case 3
                            Result = DavesCode.NewCommitClinical.CommitClinical(LinacName, UserInfo, True, FaultParams)
                            Application(suspstate) = 1

                        Case 4, 5, 6, 8
                            Result = DavesCode.NewWriteAux.WriteAuxTables(LinacName, UserInfo, ParentControlComment, RADIO, ParentControl, True, susstate, repstate, False, FaultParams)

                        Case Else
                            'Application(failstate) = ParentControl
                            'DavesCode.NewWriteAux.WriteAuxTables(LinacName, UserInfo, Comment, RADIO, ParentControl, True, susstate, repstate, False)

                    End Select
                    If Result Then
                        Application(appstate) = Nothing
                        Application(failstate) = ParentControl
                        'CreateNewFault(UserInfo, "New", connectionString)

                        Dim returnstring As String = LinacName + "page.aspx?pageref=Fault&Tabindex="
                        Response.Redirect(returnstring & ParentControl & "&comment=" & "")
                    Else
                        RaiseError()
                    End If
                Case Else
            End Select

        Else 'This is a recoverable fault - So won't have concession number?

            Result = DavesCode.NewFaultHandling.InsertRepeatFault(FaultParams)
            If Result Then
                'BindDefectData()
                RaiseEvent UpDateDefectDailyDisplay(LinacName)
            Else
                RaiseError()
            End If

            'End If
            'End If
        End If
        Defect.SelectedIndex = -1
        'ClearsForm()
    End Sub

    Protected Sub CreateFaultParams(ByVal UserInfo As String, ByRef FaultParams As DavesCode.FaultParameters)
        If (Not HttpContext.Current.Application(FaultDescriptionChanged) Is Nothing) Then
            Comment = HttpContext.Current.Application(FaultDescriptionChanged).ToString
        Else
            Comment = String.Empty
        End If

        If (Not HttpContext.Current.Application(RadActDescriptionChanged) Is Nothing) Then
            RadActComment = HttpContext.Current.Application(RadActDescriptionChanged).ToString
        Else
            RadActComment = String.Empty
        End If

        FaultParams.SelectedIncident = SelectedIncident
        FaultParams.Linac = LinacName
        FaultParams.DateInserted = DateTime.Parse(TimeFaultSelected.Value)
        FaultParams.UserInfo = UserInfo
        FaultParams.Area = Accuray.Text
        FaultParams.Energy = ErrorCode.Text
        FaultParams.GantryAngle = EMPTYSTRING
        FaultParams.CollimatorAngle = EMPTYSTRING
        FaultParams.PatientID = PatientIDBox.Text
        FaultParams.FaultDescription = Comment
        FaultParams.ConcessionNumber = ConcessionNumber
        FaultParams.RadAct = RadActComment
        FaultParams.RadioIncident = RadioIncident.SelectedItem.Value

    End Sub

    Private Sub ForceFocus(ByVal ctrl As Control)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "FocusScript", "setTimeout(function(){$get('" + ctrl.ClientID + "').focus();}, 100);", True)
    End Sub

    Protected Sub FaultOpenClosed_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FaultOpenClosed.SelectedIndexChanged
        Dim Selected As String = ""
        Selected = FaultOpenClosed.SelectedItem.Text
        Select Case Selected
            Case "Yes"
                ActPanel.Enabled = True
                UnRecoverableSave.Visible = True
                UnRecoverableSave.Enabled = True
                UnRecoverableSave.BackColor = Drawing.Color.Yellow
            Case "No"
                ActPanel.Enabled = False
                UnRecoverableSave.Visible = True
                UnRecoverableSave.Enabled = True
                UnRecoverableSave.BackColor = Drawing.Color.Yellow
            Case Else
        End Select
        'AccurayValidation.ValidationGroup = "Tomodefect"
        'FaultDescription.SetValidation = "defect"
        'CorrectiveActionValidation.ValidationGroup = "defect"
    End Sub
    Protected Sub UnRecoverableSave_Click(sender As Object, e As EventArgs) Handles UnRecoverableSave.Click
        Dim wctrl As WriteDatauc = CType(FindControl("WriteDatauc1"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
        AccurayValidation.Enabled = True
        FaultDescription.SetValidation("Tomodefect", "Please Enter a fault description")
        RadActC.SetValidation("Tomodefect", "Please Enter the Corrective Action Taken")

        Page.Validate("Tomodefect")
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
            FaultDescription.SetValidation("", "")
            RadActC.SetValidation("", "")
        End If
    End Sub
    Protected Sub RaiseError()
        Dim strScript As String = "<script>"

        strScript += "alert('Problem Updating Fault. Please call Engineering');"
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(UnRecoverableSave, Me.GetType(), "JSCR", strScript.ToString(), False)
    End Sub
End Class

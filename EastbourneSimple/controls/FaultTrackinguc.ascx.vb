Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing

Partial Class controls_FaultTrackinguc
    Inherits System.Web.UI.UserControl

    Private actionstate As String
    Private technicalstate As String
    Private RadRow As DataTable
    Const CONCESSIONSELECTED As String = "CSelected"
    Const VIEWSTATEKEY_DYNCONTROL As String = "DynamicControlSelection"
    Const EMPTYSTRING As String = ""
    Const CONCESSION As String = "Concession"
    Const CLOSED As String = "Closed"
    Const OPEN As String = "Open"
    Const TECH As String = "Tech"
    Private ConcessionDescriptionChanged As String
    Private ConcessionActionChanged As String
    Private ConcessionCommentChanged As String
    Private ParamApplication As String
    Private ConcessParamsTrial As DavesCode.ConcessionParameters = New DavesCode.ConcessionParameters()

    Public Event UpdateClosedDisplays(ByVal EquipmentName As String, ByVal incidentID As String)
    Public Event AddConcessionToDefectDropDownList(ByVal EquipmentName As String, ByVal incidentID As String)
    Public Event CloseFaultTracking(ByVal EquipmentName As String)
    'Public Property IsLoaded As Boolean
    'Public Property TabName() As String
    Public Property LinacName() As String
    Public Property IncidentID As String

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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init

        'AddHandler WriteDatauc1.UserApproved, AddressOf UserApprovedEvent
        AddHandler WriteDatauc3.UserApproved, AddressOf UserApprovedEvent

        actionstate = "ActionState" + LinacName
        technicalstate = "techstate" + LinacName

        ConcessionDescriptionChanged = "ConcessionDescription" + LinacName
        ConcessionActionChanged = "ConcessionAction" + LinacName
        ConcessionCommentChanged = "ConcessionComment" + LinacName
        ParamApplication = "Params" + LinacName

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        ConcessiondescriptionBoxC.BoxChanged = ConcessionDescriptionChanged
        ConcessionActionBox.BoxChanged = ConcessionActionChanged
        ConcessionCommentBox.BoxChanged = ConcessionCommentChanged
        Dim wrtctrl3 As WriteDatauc = CType(FindControl("WriteDatauc3"), WriteDatauc)
        wrtctrl3.LinacName = LinacName

    End Sub

    Public Sub InitialiseFaultTracking(ByVal ConcessObject As DavesCode.ConcessionParameters)

        Application(ParamApplication) = ConcessObject
        SetupStatusTech(ConcessObject.IncidentID)
        SaveAFault.Enabled = False
        CCommentPanel.Enabled = False
        CActionPanel.Enabled = False
        CDescriptionPanel.Enabled = False

        BindTrackingGridTech(ConcessObject.IncidentID)

    End Sub

    Protected Sub SetupStatusTech(ByVal incidentID As String)
        Dim FaultDescription As New List(Of String)
        ConcessParamsTrial = Application(ParamApplication)
        FaultOptionList.SelectedIndex = -1
        AssignedToList.SelectedIndex = -1

        IncidentNumber.Text = incidentID
        StatusLabel1.Text = ConcessParamsTrial.PresentFaultState
        ConcessionNumberBox.Text = ConcessParamsTrial.ConcessionNumber
        ConcessiondescriptionBoxC.ResetCommentBox(ConcessParamsTrial.ConcessionDescription)
        ConcessionActionBox.ResetCommentBox(ConcessParamsTrial.ConcessionAction)
        AssignedToList.SelectedIndex = AssignedToList.Items.IndexOf(AssignedToList.Items.FindByText(ConcessParamsTrial.AssignedTo))
        If ConcessParamsTrial.PresentFaultState = "Concession" Then
            FaultOptionList.Items.FindByValue("Open").Enabled = False
        End If
        LoadFaultTable(incidentID)

    End Sub


    Protected Sub LoadFaultTable(ByVal incidentid As String)
        Dim objcon As Object
        objcon = Page.LoadControl("~/controls/DeviceReportedfaultuc.ascx")
        CType(objcon, controls_DeviceReportedfaultuc).IncidentID = incidentid
        CType(objcon, controls_DeviceReportedfaultuc).Device = LinacName
        CType(objcon, controls_DeviceReportedfaultuc).ReportFaultID = incidentid
        DeviceReportedFaultPlaceHolder.Controls.Add(objcon)

    End Sub

    Protected Sub BindTrackingGridTech(ByVal incidentID As String)
        Dim SqlDataSource3 As New SqlDataSource With {
            .ID = "SqlDataSource3",
            .ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString,
            .SelectCommand = "select t.TrackingID, t.description,t.action, t.trackingcomment, t.AssignedTo, t.Status, t.LastUpDatedBy, t.LastUpDatedOn  " _
        & "from FaultTracking t  where t.incidentID=@incidentID order by t.TrackingID desc"
        }

        SqlDataSource3.SelectParameters.Add("@incidentID", System.Data.SqlDbType.Int)
        SqlDataSource3.SelectParameters.Add("incidentID", incidentID)
        GridView5.DataSource = SqlDataSource3
        GridView5.DataBind()
    End Sub

    Protected Sub FaultOptionList_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles FaultOptionList.SelectedIndexChanged
        Dim updateFaultStatus As String
        Dim incidentid As String
        ConcessParamsTrial = Application(ParamApplication)
        Dim CDescriptionstring As String = ConcessParamsTrial.ConcessionDescription

        updateFaultStatus = FaultOptionList.SelectedItem.Value
        incidentid = ConcessParamsTrial.IncidentID

        If Not updateFaultStatus = "Select" Then
            SaveAFault.Enabled = True
            SaveAFault.BackColor = Drawing.Color.Yellow
            CCommentPanel.Enabled = True
            ConcessParamsTrial.FutureFaultState = updateFaultStatus
            If updateFaultStatus = "Concession" Then

                CActionPanel.Enabled = True
                If String.IsNullOrEmpty(CDescriptionstring) Then
                    CDescriptionPanel.Enabled = True
                End If
            Else
                CActionPanel.Enabled = False

            End If

        Else
            CDescriptionPanel.Enabled = False
            CActionPanel.Enabled = False
            CCommentPanel.Enabled = False
            ConcessionNumberBox.ReadOnly = True
            SaveAFault.Enabled = False
            SaveAFault.BackColor = Drawing.Color.LightGray
        End If
        'Basically puts new selected value into concessparams and Application
        Application(ParamApplication) = ConcessParamsTrial

        LoadFaultTable(incidentid)
        BindTrackingGridTech(incidentid)

    End Sub

    Protected Sub SaveAFault_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SaveAFault.Click
        Dim validator As System.Web.UI.WebControls.BaseValidator
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc3"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        'what's this here for?
        'If TabName = "Clin" Then
        '    wctrl.UserReason = "12"
        'End If

        Dim ConcessionNumber As String

        Dim strScript As String = "<script>"
        Dim incidentid As String
        'Dim UniqueC As Boolean
        Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
        ConcessionNumber = ConcessionNumberBox.Text
        'UniqueC = CheckUniqueConcession(ConcessionNumber)
        'or this is concessparamstrial.futurestate
        Select Case FaultOptionList.SelectedItem.Text
            Case CONCESSION
                'Concessiondescription validator doesn't work if panel is disabled
                If CDescriptionPanel.Enabled = True Then
                    ConcessiondescriptionBoxC.SetValidation("faulttracking", "Please Enter a Concession description")
                End If
                ConcessionActionBox.SetValidation("faulttracking", "Please Enter the Corrective Action")
        End Select
        'this is concessparamstrial.incidentid
        incidentid = IncidentNumber.Text

        'This must be different validate key to others on the page!
        Page.Validate("faulttracking")
        If Page.IsValid Then

            wcbutton.Text = "Saving Fault Status"
            Application(actionstate) = "Confirm"
            wctrl.Visible = True
            ForceFocus(wctext)

            BindTrackingGridTech(incidentid)
        Else
            For Each validator In Page.Validators
                If (Not validator.IsValid) Then
                    'validator that failed found so set the focus to the control
                    'it validates and exit the loop
                    ForceFocus(validator.FindControl(validator.ControlToValidate))
                    Exit For
                End If
            Next validator

            Me.DynamicControlSelection = CONCESSIONSELECTED
            LoadFaultTable(incidentid)
            BindTrackingGridTech(incidentid)
            FormError()
        End If

    End Sub

    Protected Sub UserApprovedEvent(ByVal Tabused As String, ByVal Userinfo As String)
        Dim TRACKINGID As Integer = 0
        Dim Action As String = Application(actionstate)
        Dim incidentID As String
        Dim Machine As String
        Application(technicalstate) = Nothing
        CreateConcessParams(Userinfo)

        Machine = ConcessParamsTrial.Linac
        'Update fault is for recording repeat fault
        If Tabused = "Cancel" Then
            'Cancel()

            'This is all redundant now because of use of devicerepeatfaultuc

        Else Tabused = "incident"
            incidentID = ConcessParamsTrial.IncidentID
            'This stops it popping up again when it shouldn't

            Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc3"), WriteDatauc)
            wctrl.Visible = False

            If Action = "Confirm" Then
                Dim exists As Integer

                If incidentID <> 0 Then
                    'need to check if concession is new or not

                    If ConcessParamsTrial.FutureFaultState = "Concession" Then
                        'if new concession and everything works then exists = 0 so write new concession and tracking and skip to end
                        'if there is a problem then roll back and skip to end via -1 (what about rad row etc?)
                        'if exists = 1 or not 0 or -1 then concession already exists so update tracking. If it gets to there then insertnewconcession has worked
                        'ie returned exists = 1 or not 0 or -1 but hasn't done anything else. If concession updated ok but otherwise update tracking
                        'exists = DavesCode.NewFaultHandling.InsertNewConcession(ConcessionDescription, LinacName, incidentID, Userinfo, ConcessionAction)
                        If String.IsNullOrEmpty(ConcessParamsTrial.ConcessionNumber) Then

                            exists = DavesCode.NewFaultHandling.InsertNewConcession(ConcessParamsTrial)

                            If exists = -1 Then
                                RaiseError()
                            Else
                                RaiseEvent AddConcessionToDefectDropDownList(Machine, exists)
                                RaiseEvent CloseFaultTracking(Machine)
                            End If
                        Else
                            TRACKINGID = DavesCode.NewFaultHandling.UpdateTracking(ConcessParamsTrial)
                            If TRACKINGID = -1 Then
                                RaiseError()
                            Else
                                RaiseEvent CloseFaultTracking(Machine)
                            End If
                        End If

                    Else
                        TRACKINGID = DavesCode.NewFaultHandling.UpdateTracking(ConcessParamsTrial)

                        If TRACKINGID = -1 Then
                            RaiseError()
                        Else
                            '    RaiseEvent CloseFaultTracking(Machine)
                            'End If
                            If ConcessParamsTrial.FutureFaultState = "Closed" Then

                                RaiseEvent UpdateClosedDisplays(Machine, incidentID)

                            End If
                            RaiseEvent CloseFaultTracking(Machine)
                        End If

                    End If
                Else
                    RaiseError()
                    RaiseEvent CloseFaultTracking(Machine)
                End If
            End If
        End If
    End Sub

    Protected Sub RaiseError()
        Dim strScript As String = "<script>"

        strScript += "alert('Problem Updating Fault. Please call Engineer');"
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(SaveAFault, Me.GetType(), "JSCR", strScript.ToString(), False)
    End Sub

    Protected Sub CreateConcessParams(ByVal UserInfo As String)
        ConcessParamsTrial = Application(ParamApplication)
        Dim ConcessionDescription As String
        Dim ConcessionAction As String
        Dim ConcessionComment As String
        Dim FaultState As String
        Dim ConcessionNumber As String
        Dim AssignedTo As String
        If (Not HttpContext.Current.Application(ConcessionDescriptionChanged) Is Nothing) Then
            ConcessionDescription = HttpContext.Current.Application(ConcessionDescriptionChanged).ToString
        Else
            ConcessionDescription = String.Empty
        End If

        If (Not HttpContext.Current.Application(ConcessionActionChanged) Is Nothing) Then
            ConcessionAction = HttpContext.Current.Application(ConcessionActionChanged).ToString
        Else
            ConcessionAction = String.Empty
        End If

        If (Not HttpContext.Current.Application(ConcessionCommentChanged) Is Nothing) Then
            ConcessionComment = HttpContext.Current.Application(ConcessionCommentChanged).ToString
        Else
            ConcessionComment = String.Empty
        End If


        AssignedTo = AssignedToList.SelectedItem.Text
        If AssignedTo = "Select" Then
            AssignedTo = "Unassigned"
        End If

        FaultState = FaultOptionList.SelectedItem.Text
        ConcessionNumber = ConcessionNumberBox.Text
        ConcessParamsTrial.UserInfo = UserInfo
        ConcessParamsTrial.AssignedTo = AssignedTo
        ConcessParamsTrial.ConcessionAction = ConcessionAction

        ConcessParamsTrial.ConcessionComment = ConcessionComment
        ConcessParamsTrial.ConcessionDescription = ConcessionDescription


        Application(ParamApplication) = ConcessParamsTrial
    End Sub


    Protected Sub ClearActionButton_Click(sender As Object, e As EventArgs) Handles ClearActionButton.Click
        ConcessParamsTrial = Application(ParamApplication)
        ConcessionActionBox.ResetCommentBox(EMPTYSTRING)
        ConcessParamsTrial.ConcessionAction = EMPTYSTRING
        Application(ParamApplication) = ConcessParamsTrial
        BindTrackingGridTech(IncidentNumber.Text)
        LoadFaultTable(IncidentNumber.Text)
    End Sub
    Protected Sub ClearCommentButton_Click(sender As Object, e As EventArgs) Handles ClearCommentButton.Click
        ConcessParamsTrial = Application(ParamApplication)
        ConcessionCommentBox.ResetCommentBox(EMPTYSTRING)
        ConcessParamsTrial.ConcessionComment = EMPTYSTRING
        Application(ParamApplication) = ConcessParamsTrial
        LoadFaultTable(IncidentNumber.Text)
        BindTrackingGridTech(IncidentNumber.Text)
    End Sub

    Protected Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        ConcessParamsTrial = Application(ParamApplication)
        RaiseEvent CloseFaultTracking(ConcessParamsTrial.Linac)

    End Sub

    Private Sub ForceFocus(ByVal ctrl As Control)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "FocusScript", "setTimeout(function(){$get('" + ctrl.ClientID + "').focus();}, 100);", True)
    End Sub

    Protected Sub FormError()
        Dim strScript As String = "<script>"
        strScript += "alert('Please Correct Form Errors');"
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(SaveAFault, Me.GetType(), "JSCR", strScript.ToString(), False)
    End Sub
End Class

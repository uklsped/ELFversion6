Imports AjaxControlToolkit
Imports DavesCode
Partial Class controls_NewFaultPopUpuc
    Inherits System.Web.UI.UserControl

    Private IncidentID As String
    Dim modalpopupextendernf As New ModalPopupExtender
    Public Property ParentName() As String
    Public Property LinacName() As String
    Private ParamApplication As String
    Private ConcessParamsTrial As ConcessionParameters = New ConcessionParameters()
    Private FaultParamsApplication As String
    Public Event CloseFaultTracking(ByVal Linac As String)
    Public Event UpdateOpenConcessions(ByVal Linac As String)
    'Public Event UpdateClosedDisplays(ByVal Linac As String, ByVal IncidentID As String)
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        ParamApplication = "Params" + LinacName
        AddHandler FaultTrackinguc1.CloseFaultTracking, AddressOf CloseTracking
        'AddHandler FaultTrackinguc1.UpdateClosedDisplays, AddressOf CloseDisplays
        FaultParamsApplication = "FaultParams" + LinacName
    End Sub

    'Private Property DynamicControlSelection() As String - Do THIS On WEDNESDAY MORNING
    '    Get
    '        Dim result As String = ViewState.Item(VIEWSTATEKEY_DYNCONTROL)
    '        If result Is Nothing Then
    '            'doing things like this lets us access this property without
    '            'worrying about this property returning null/Nothing
    '            Return String.Empty
    '        Else
    '            Return result
    '        End If
    '    End Get
    '    Set(ByVal value As String)
    '        ViewState.Item(VIEWSTATEKEY_DYNCONTROL) = value
    '    End Set
    'End Property
    Private Sub CloseTracking(ByVal LinacName As String)
        RaiseEvent CloseFaultTracking(LinacName)
    End Sub

    Private Sub Update_OpenConcessions(ByVal LinacName As String)
        RaiseEvent UpdateOpenConcessions(LinacName)
    End Sub

    'Private Sub CloseDisplays(ByVal LinacName As String, ByVal IncidentID As String)
    '    RaiseEvent UpdateClosedDisplays(LinacName, IncidentID)
    'End Sub
    Protected Sub Page_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'WaitButtons("Acknowledge")
        'appstate = "LogOn" + MachineName
        'actionstate = "ActionState" + MachineName
        Dim success As Boolean = False
        Dim logerrorbox As Label = FindControl("LoginErrordetails")
        logerrorbox.Text = Nothing

        'Reference to defect removed 23/11/16 Added back in 26/03/18
        'If Application(appstate) = 1 Or tablabel = "3" Or tablabel = "Report" Or tablabel = "handover" Or tablabel = "EndDay" Or tablabel = "Admin" Or tablabel = "Updatefault" Or tablabel = "incident" Or tablabel = "0" Or tablabel = "Defect" Or tablabel = "recover" Or tablabel = "Image" Or tablabel = "Major" Then
        'If Application(appstate) = 1 Or tablabel = "3" Or tablabel = "Report" Or tablabel = "EndDay" Or tablabel = "Admin" Or tablabel = "Updatefault" Or tablabel = "incident" Or tablabel = "0" Or tablabel = "Defect" Or tablabel = "recover" Or tablabel = "Image" Or tablabel = "Major" Then
        'If Application(appstate) = 1 Or tablabel = "3" Or tablabel = "Report" Or tablabel = "handover" Or tablabel = "EndDay" Or tablabel = "Admin" Or tablabel = "Updatefault" Or tablabel = "incident" Or tablabel = "0" Or tablabel = "recover" Or tablabel = "Image" Then
        'IncidentID = Application(FaultParamsApplication)
        'success = ConcessParamsTrial.CreateObject(IncidentID)
        'success = DavesCode.ConcessionParameters.CreateObject(IncidentID, LinacName)
        'If Not IsPostBack Then
        ConcessParamsTrial = Application(ParamApplication)
            If Not ConcessParamsTrial Is Nothing Then
                Application(ParamApplication) = ConcessParamsTrial

                Dim FaultTracking As controls_FaultTrackinguc = CType(FindControl("FaultTrackinguc1"), controls_FaultTrackinguc)
                FaultTracking.LinacName = ConcessParamsTrial.Linac
                FaultTracking.IncidentID = ConcessParamsTrial.IncidentID
                FaultTracking.InitialiseFaultTracking(ConcessParamsTrial)
            AddHandler FaultTrackinguc1.UpdateOpenConcessions, AddressOf Update_OpenConcessions
            AddHandler FaultTracking.CloseFaultTracking, AddressOf CloseTracking

            Dim MyString As String
                Dim Tabnumber As String
                MyString = "ModalPopupextendernf"
                Tabnumber = ParentName
                MyString = MyString & Tabnumber
                modalpopupextendernf.ID = MyString
                modalpopupextendernf.BehaviorID = MyString
                modalpopupextendernf.TargetControlID = "label1"
                modalpopupextendernf.PopupControlID = "Panel1"
                modalpopupextendernf.BackgroundCssClass = "modalBackground"
                PlaceHolder1.Controls.Add(modalpopupextendernf)
                modalpopupextendernf.Show()
            Else
                RaiseError()
            End If
        ' End If
    End Sub

    Protected Sub RaiseError()
        Dim strScript As String = "<script>"
        strScript += "alert('Problem Updating Fault. Please call Engineer');"
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(Label1, Me.GetType(), "JSCR", strScript.ToString(), False)
    End Sub




    Protected Function GetNewIncident() As Short
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        Dim IncidentID As Short = 0
        IncidentID = NewFaultHandling.ReturnNewIncidentID(LinacName, connectionString)
        Return IncidentID
    End Function

End Class

Imports AjaxControlToolkit
Imports DavesCode
Partial Class controls_NewFaultPopUpuc
    Inherits System.Web.UI.UserControl
    Dim modalpopupextenderft As New ModalPopupExtender
    Public Property ParentName() As String
    Public Property LinacName() As String
    Private ParamApplication As String
    Private ConcessParamsTrial As ConcessionParameters = New ConcessionParameters()
    Public Event CloseFaultTracking(ByVal Linac As String)
    Public Event UpdateClosedDisplays(ByVal Linac As String, ByVal IncidentID As String)
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        ParamApplication = "Params" + LinacName
        AddHandler FaultTrackinguc1.CloseFaultTracking, AddressOf CloseTracking
        AddHandler FaultTrackinguc1.UpdateClosedDisplays, AddressOf CloseDisplays

    End Sub
    Private Sub CloseTracking(ByVal LinacName As String)
        RaiseEvent CloseFaultTracking(LinacName)
    End Sub
    Private Sub CloseDisplays(ByVal LinacName As String, ByVal IncidentID As String)
        RaiseEvent UpdateClosedDisplays(LinacName, IncidentID)
    End Sub
    Protected Sub Page_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'WaitButtons("Acknowledge")
        'appstate = "LogOn" + MachineName
        'actionstate = "ActionState" + MachineName
        Dim logerrorbox As Label = FindControl("LoginErrordetails")
        logerrorbox.Text = Nothing

        'Reference to defect removed 23/11/16 Added back in 26/03/18
        'If Application(appstate) = 1 Or tablabel = "3" Or tablabel = "Report" Or tablabel = "handover" Or tablabel = "EndDay" Or tablabel = "Admin" Or tablabel = "Updatefault" Or tablabel = "incident" Or tablabel = "0" Or tablabel = "Defect" Or tablabel = "recover" Or tablabel = "Image" Or tablabel = "Major" Then
        'If Application(appstate) = 1 Or tablabel = "3" Or tablabel = "Report" Or tablabel = "EndDay" Or tablabel = "Admin" Or tablabel = "Updatefault" Or tablabel = "incident" Or tablabel = "0" Or tablabel = "Defect" Or tablabel = "recover" Or tablabel = "Image" Or tablabel = "Major" Then
        'If Application(appstate) = 1 Or tablabel = "3" Or tablabel = "Report" Or tablabel = "handover" Or tablabel = "EndDay" Or tablabel = "Admin" Or tablabel = "Updatefault" Or tablabel = "incident" Or tablabel = "0" Or tablabel = "recover" Or tablabel = "Image" Then

        ConcessParamsTrial = Application(ParamApplication)
        If Not ConcessParamsTrial Is Nothing Then


            Dim FaultTracking As controls_FaultTrackinguc = CType(FindControl("FaultTrackinguc1"), controls_FaultTrackinguc)
            FaultTracking.LinacName = ConcessParamsTrial.Linac
            FaultTracking.IncidentID = ConcessParamsTrial.IncidentID
            FaultTracking.InitialiseFaultTracking(ConcessParamsTrial)


            Dim MyString As String
            Dim Tabnumber As String
            MyString = "ModalPopupextenderft"
            Tabnumber = ParentName
            MyString = MyString & Tabnumber
            modalpopupextenderft.ID = MyString
            modalpopupextenderft.BehaviorID = MyString
            modalpopupextenderft.TargetControlID = "label1"
            modalpopupextenderft.PopupControlID = "Panel1"
            modalpopupextenderft.BackgroundCssClass = "modalBackground"
            PlaceHolder1.Controls.Add(modalpopupextenderft)
            modalpopupextenderft.Show()
        End If

    End Sub
End Class

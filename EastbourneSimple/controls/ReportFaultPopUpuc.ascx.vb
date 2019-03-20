Imports AjaxControlToolkit
Imports DavesCode
Partial Class controls_ReportFaultPopUpuc
    Inherits System.Web.UI.UserControl
    Dim modalpopupextenderrf As New ModalPopupExtender
    Public Property ParentControl() As String
    Public Property LinacName() As String
    Public Property ParentCommentBox() As String
    Private ParamApplication As String
    Private ConcessParamsTrial As ConcessionParameters = New ConcessionParameters()
    Public Event CloseFaultTracking(ByVal Linac As String)
    Public Event UpdateClosedDisplays(ByVal Linac As String, ByVal IncidentID As String)
    Public Event UpDateDefectDailyDisplay(ByVal Linac As String)
    Public Event UpdateViewOpenFaults(ByVal Linac As String)
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        ParamApplication = "Params" + LinacName
        'AddHandler FaultTrackinguc1.CloseFaultTracking, AddressOf CloseTracking
        'AddHandler FaultTrackinguc1.UpdateClosedDisplays, AddressOf CloseDisplays

    End Sub
    Private Sub CloseTracking(ByVal LinacName As String)
        RaiseEvent CloseFaultTracking(LinacName)
    End Sub
    Private Sub CloseDisplays(ByVal LinacName As String, ByVal IncidentID As String)
        RaiseEvent UpdateClosedDisplays(LinacName, IncidentID)
    End Sub

    Private Sub Update_DefectDailyDisplay(ByVal linacName As String)
        RaiseEvent UpDateDefectDailyDisplay(linacName)
    End Sub
    Private Sub Update_ViewOpenFaults(ByVal linacName As String)
        RaiseEvent UpdateViewOpenFaults(linacName)
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
        'If Not ConcessParamsTrial Is Nothing Then


        Dim objDefect As DefectSave = CType(FindControl("DefectSave1"), DefectSave)
        'Dim objDefect As DefectSave
        'objDefect = Page.LoadControl("DefectSave.ascx")
        'CType(objDefect, DefectSave).ID = "DefectDisplay"
        CType(objDefect, DefectSave).LinacName = LinacName
        CType(objDefect, DefectSave).ParentControl = ParentControl
        CType(objDefect, DefectSave).Parentcontrolcomment = ParentCommentBox
        AddHandler CType(objDefect, DefectSave).CloseReportFaultPopUp, AddressOf Close_ReportFaultPopUp
        AddHandler CType(objDefect, DefectSave).UpDateDefectDailyDisplay, AddressOf Update_DefectDailyDisplay
        AddHandler CType(objDefect, DefectSave).UpdateViewOpenFaults, AddressOf Update_ViewOpenFaults
        'AddHandler CType(objDefect, DefectSave).UpDateDefectDailyDisplay, AddressOf Update_DefectDailyDisplay


        'PlaceHolderDefectSave.Controls.Add(objDefect)
        'FaultTracking.LinacName = ConcessParamsTrial.Linac
        'FaultTracking.IncidentID = ConcessParamsTrial.IncidentID
        'FaultTracking.InitialiseFaultTracking(ConcessParamsTrial)


        Dim MyString As String
        Dim Tabnumber As String
        MyString = "ModalPopupextenderrf"
        Tabnumber = ParentControl
        MyString = MyString & Tabnumber
        modalpopupextenderrf.ID = MyString
        modalpopupextenderrf.BehaviorID = MyString
        modalpopupextenderrf.TargetControlID = "Label1"
        modalpopupextenderrf.PopupControlID = "Panel1"
        modalpopupextenderrf.BackgroundCssClass = "modalBackground"
        PlaceHolder1.Controls.Add(modalpopupextenderrf)
        modalpopupextenderrf.Show()



    End Sub
    Protected Sub Close_ReportFaultPopUp()
        modalpopupextenderrf.Hide()
    End Sub

End Class

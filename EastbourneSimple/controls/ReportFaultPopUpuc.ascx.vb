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
    'Public Event CloseFaultTracking(ByVal Linac As String)
    'Public Event UpdateClosedDisplays(ByVal Linac As String, ByVal IncidentID As String)
    Public Event UpDateDefectDailyDisplay(ByVal Linac As String)
    Public Event UpdateViewOpenFaults(ByVal Linac As String)
    Public Event CloseReportFaultPopUpTab(ByVal Linac As String)
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        ParamApplication = "Params" + LinacName
        'AddHandler FaultTrackinguc1.CloseFaultTracking, AddressOf CloseTracking
        'AddHandler FaultTrackinguc1.UpdateClosedDisplays, AddressOf CloseDisplays


    End Sub
    'Private Sub CloseTracking(ByVal LinacName As String)
    '    RaiseEvent CloseFaultTracking(LinacName)
    'End Sub
    Private Sub Close_ReportPopUp(ByVal LinacName As String)
        RaiseEvent CloseReportFaultPopUpTab(LinacName)
    End Sub
    'Private Sub CloseDisplays(ByVal LinacName As String, ByVal IncidentID As String)
    '    RaiseEvent UpdateClosedDisplays(LinacName, IncidentID)
    'End Sub

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

        'ConcessParamsTrial = Application(ParamApplication)
        ''If Not ConcessParamsTrial Is Nothing Then

        'If Not LinacName Like "T?" Then
        '    Dim objDefect As DefectSave = CType(FindControl("DefectSave1"), DefectSave)
        '    'Dim objDefect As DefectSave
        '    'objDefect = Page.LoadControl("DefectSave.ascx")
        '    CType(objDefect, DefectSave).ID = "DefectDisplay"
        '    CType(objDefect, DefectSave).LinacName = LinacName
        '    CType(objDefect, DefectSave).ParentControl = ParentControl
        '    CType(objDefect, DefectSave).Parentcontrolcomment = ParentCommentBox
        '    AddHandler CType(objDefect, DefectSave).CloseReportFaultPopUp, AddressOf Close_ReportFaultPopUp
        '    AddHandler CType(objDefect, DefectSave).UpDateDefectDailyDisplay, AddressOf Update_DefectDailyDisplay
        '    AddHandler CType(objDefect, DefectSave).UpdateViewOpenFaults, AddressOf Update_ViewOpenFaults
        '    objDefect.InitialiseDefectPage()
        '    'AddHandler CType(objDefect, DefectSave).UpDateDefectDailyDisplay, AddressOf Update_DefectDailyDisplay


        '    'PlaceHolderDefectSave.Controls.Add(objDefect)
        '    'FaultTracking.LinacName = ConcessParamsTrial.Linac
        '    'FaultTracking.IncidentID = ConcessParamsTrial.IncidentID
        '    'FaultTracking.InitialiseFaultTracking(ConcessParamsTrial)
        'Else

        '    Dim objDefectPark As DefectSavePark = CType(FindControl("DefectSavePark1"), DefectSavePark)
        '    'Dim objDefect As DefectSave
        '    'objDefect = Page.LoadControl("DefectSave.ascx")
        '    CType(objDefectPark, DefectSavePark).ID = "DefectDisplaypark"
        '    CType(objDefectPark, DefectSavePark).LinacName = LinacName
        '    CType(objDefectPark, DefectSavePark).ParentControl = ParentControl
        '    'CType(objDefectPark, DefectSavePark).ParentControlComment = ParentCommentBox
        '    AddHandler CType(objDefectPark, DefectSavePark).CloseReportFaultPopUp, AddressOf Close_ReportFaultPopUp
        '    AddHandler CType(objDefectPark, DefectSavePark).UpDateDefectDailyDisplay, AddressOf Update_DefectDailyDisplay
        '    AddHandler CType(objDefectPark, DefectSavePark).UpdateViewOpenFaults, AddressOf Update_ViewOpenFaults
        '    objDefectPark.InitialiseDefectPage()
        '    DefectView.ActiveViewIndex = 1
        'End If


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

    Public Sub SetUpReportFault()
        ConcessParamsTrial = Application(ParamApplication)
        'If Not ConcessParamsTrial Is Nothing Then

        If Not LinacName Like "T?" Then
            Dim objDefect As DefectSave = Page.LoadControl("DefectSave.ascx")
            'Dim objDefect As DefectSave
            'objDefect = Page.LoadControl("DefectSave.ascx")
            CType(objDefect, DefectSave).ID = "DefectDisplay"
            CType(objDefect, DefectSave).LinacName = LinacName
            CType(objDefect, DefectSave).ParentControl = ParentControl
            CType(objDefect, DefectSave).ParentControlComment = ParentCommentBox
            AddHandler CType(objDefect, DefectSave).CloseReportFaultPopUp, AddressOf Close_ReportFaultPopUp
            AddHandler CType(objDefect, DefectSave).UpDateDefectDailyDisplay, AddressOf Update_DefectDailyDisplay
            AddHandler CType(objDefect, DefectSave).UpdateViewOpenFaults, AddressOf Update_ViewOpenFaults
            objDefect.InitialiseDefectPage()
            'AddHandler CType(objDefect, DefectSave).UpDateDefectDailyDisplay, AddressOf Update_DefectDailyDisplay
            PlaceHolderDefectSave.Controls.Add(objDefect)

            'PlaceHolderDefectSave.Controls.Add(objDefect)
            'FaultTracking.LinacName = ConcessParamsTrial.Linac
            'FaultTracking.IncidentID = ConcessParamsTrial.IncidentID
            'FaultTracking.InitialiseFaultTracking(ConcessParamsTrial)
        Else

            Dim objDefectPark As DefectSavePark = CType(FindControl("DefectSavePark1"), DefectSavePark)
            'Dim objDefect As DefectSave
            'objDefect = Page.LoadControl("DefectSave.ascx")
            CType(objDefectPark, DefectSavePark).ID = "DefectDisplaypark"
            CType(objDefectPark, DefectSavePark).LinacName = LinacName
            CType(objDefectPark, DefectSavePark).ParentControl = ParentControl
            'CType(objDefectPark, DefectSavePark).ParentControlComment = ParentCommentBox
            AddHandler CType(objDefectPark, DefectSavePark).CloseReportFaultPopUp, AddressOf Close_ReportFaultPopUp
            AddHandler CType(objDefectPark, DefectSavePark).UpDateDefectDailyDisplay, AddressOf Update_DefectDailyDisplay
            AddHandler CType(objDefectPark, DefectSavePark).UpdateViewOpenFaults, AddressOf Update_ViewOpenFaults
            objDefectPark.InitialiseDefectPage()
            DefectView.ActiveViewIndex = 1
        End If
    End Sub
    Protected Sub Close_ReportFaultPopUp(ByVal Linac As String)
        If LinacName = Linac Then
            RaiseEvent CloseReportFaultPopUpTab(Linac)
        End If
    End Sub

End Class

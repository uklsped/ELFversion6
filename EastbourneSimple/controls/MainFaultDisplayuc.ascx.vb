
Imports System.Data

Partial Class controls_MainFaultDisplayuc
    Inherits System.Web.UI.UserControl
    Public Property LinacName() As String
    Public Property ParentControl As String
    Public Property RepeatFault As Integer
    Private Property MainFaultID As String = "MainFaults"

    'Const PRECLIN As String = "2"
    'Const REPEATFAULT As Integer = 0


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim objMFG As UserControl = Page.LoadControl("ManyFaultGriduc.ascx")
        CType(objMFG, ManyFaultGriduc).NewFault = False
        CType(objMFG, ManyFaultGriduc).IncidentID = RepeatFault
        'to accomodate Tomo now need to pass equipment name?
        CType(objMFG, ManyFaultGriduc).MachineName = LinacName
        CType(objMFG, ManyFaultGriduc).ID = "MFG"
        PlaceHolderFaults.Controls.Add(objMFG)

        Dim objconToday As TodayClosedFault = Page.LoadControl("TodayClosedFault.ascx")
        objconToday = Page.LoadControl("TodayClosedFault.ascx")
        objconToday.ID = "Todaysfaults"
        objconToday.LinacName = LinacName
        PlaceHolderTodaysclosedfaults.Controls.Add(objconToday)

        Dim objCon As ViewOpenFaults = Page.LoadControl("ViewOpenFaults.ascx")
        CType(objCon, ViewOpenFaults).LinacName = LinacName
        CType(objCon, ViewOpenFaults).ParentControl = ParentControl
        CType(objCon, ViewOpenFaults).ID = "ViewOpenFaults"
        AddHandler CType(objCon, ViewOpenFaults).UpdateFaultClosedDisplays, AddressOf Update_FaultClosedDisplays
        'AddHandler CType(objCon, ViewOpenFaults).UpDateDefectDailyDisplay, AddressOf Update_DefectDailyDisplay

        PlaceHolderViewOpenFaults.Controls.Add(objCon)
    End Sub

    Protected Sub Update_FaultClosedDisplays(ByVal EquipmentID As String, ByVal incidentID As String)
        If LinacName = EquipmentID Then
            Dim todayfault As TodayClosedFault = PlaceHolderTodaysclosedfaults.FindControl("Todaysfaults")
            todayfault.SetGrid()
            'If MachineName Like "T?" Then
            '    Todaydefectpark = PlaceHolder1.FindControl("DefectDisplay")
            '    Todaydefectpark.ResetDefectDropDown(incidentID)
            'Else
            '    Todaydefect = PlaceHolder1.FindControl("DefectDisplay")
            '    Todaydefect.ResetDefectDropDown(incidentID)
            'End If

        End If
    End Sub
    Public Sub Update_defectsToday(ByVal EquipmentID As String)
        If LinacName = EquipmentID Then
            Dim TodayDefectClosed As ManyFaultGriduc = PlaceHolderFaults.FindControl("MFG")
            TodayDefectClosed.BindDefectData()
        End If
    End Sub

    Public Sub Update_OpenConcessions(ByVal EquipmentID As String)
        If LinacName = EquipmentID Then
            Dim OpenConcessions As ViewOpenFaults = PlaceHolderViewOpenFaults.FindControl("ViewOpenFaults")
            OpenConcessions.RebindViewFault()
        End If
    End Sub

End Class

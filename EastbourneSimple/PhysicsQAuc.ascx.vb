Imports AjaxControlToolkit
Imports System.Data.SqlClient
Partial Class PhysicsQAuc
    Inherits System.Web.UI.UserControl
    Private MachineName As String
    Private SelectCount As Boolean
    Private Radioselect As Integer
    Private appstate As String
    Private suspstate As String
    Private actionstate As String
    Private failstate As String
    Private repairstate As String
    Private faultviewstate As String
    Private atlasviewstate As String
    Private qaviewstate As String
    Private energyviewstate As String
    Private objconToday As TodayClosedFault
    Private Todaydefect As DefectSave
    Private laststate As String
    Private lastuser As String
    Private lastusergroup As String
    Private BoxChanged As String
    Private Event AutoApproved(ByVal Tab As String, ByVal UserName As String)

    Public Property LinacName() As String
        Get
            Return MachineName
        End Get
        Set(ByVal value As String)
            MachineName = value
        End Set
    End Property

    Protected Sub Update_Today(ByVal EquipmentID As String, ByVal incidentID As String)
        If MachineName = EquipmentID Then
            Dim todayfault As TodayClosedFault = PlaceHolder5.FindControl("Todaysfaults")
            todayfault.SetGrid()
            Todaydefect = PlaceHolder1.FindControl("DefectDisplay")
            Todaydefect.ResetDefectDropDown(incidentID)
        End If
    End Sub

    Protected Sub Update_DefectDailyDisplay(ByVal EquipmentID As String)
        If MachineName = EquipmentID Then
            'Todaydefect = PlaceHolder1.FindControl("DefectDisplay")
            'Todaydefect.UpDateDefectsEventHandler()
        End If
    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        AddHandler WriteDatauc1.UserApproved, AddressOf UserApprovedEvent
        AddHandler PhysicsQAuc.AutoApproved, AddressOf UserApprovedEvent
        appstate = "LogOn" + MachineName
        actionstate = "ActionState" + MachineName
        suspstate = "Suspended" + MachineName
        failstate = "FailState" + MachineName
        repairstate = "rppTab" + MachineName
        faultviewstate = "Faultsee" + MachineName
        atlasviewstate = "Atlassee" + MachineName
        qaviewstate = "QAsee" + MachineName
        energyviewstate = "Qenergies" + MachineName
        BoxChanged = "PQABoxChanged" + MachineName
    End Sub
    Protected Sub UserApprovedEvent(ByVal Tabused As String, ByVal Userinfo As String)
        Dim machinelabel As String = MachineName & "Page.aspx';"
        Dim username As String = Userinfo
        Dim suspendvalue As String
        Dim repairvalue As String
        Dim LinacStateID As String = ""
        If Tabused = "6" Then
            'DavesCode.Reuse.ReturnApplicationState(Tabused)
            Dim Action As String = Application(actionstate)
            If Action = "Confirm" Then
                Dim Textboxcomment As TextBox = FindControl("CommentBox")
                Dim comment As String = Textboxcomment.Text
                Dim strScript As String = "<script>"
                strScript += "window.location='"
                strScript += machinelabel
                strScript += "</script>"
                suspendvalue = Application(suspstate)
                repairvalue = Application(repairstate)
                Radioselect = RadioButtonList1.SelectedItem.Value
                DavesCode.Reuse.WriteAuxTables(MachineName, username, comment, Radioselect, Tabused, False, suspendvalue, repairvalue, False)
                Application(appstate) = Nothing
                HttpContext.Current.Application(BoxChanged) = Nothing
                Select Case Radioselect
                    Case 1
                        Application(failstate) = Nothing
                        Application(repairstate) = Nothing
                        Application(suspstate) = Nothing
                        Dim returnstring As String = MachineName + "page.aspx?tabref=" + Convert.ToString(Radioselect)
                        'DavesCode.Reuse.ReturnApplicationState(Tabused)
                        Response.Redirect(returnstring)
                    Case 2
                        Application(suspstate) = Nothing
                        Application(failstate) = Nothing
                        Application(repairstate) = 1
                        'DavesCode.Reuse.ReturnApplicationState(Tabused)
                        ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                    Case 3
                        Application(suspstate) = 1
                        Application(failstate) = Nothing
                        'DavesCode.Reuse.ReturnApplicationState(Tabused)
                        ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                    Case 4
                        Dim returnstring As String = MachineName + "page.aspx?tabref=" + Convert.ToString(Radioselect)
                        'DavesCode.Reuse.ReturnApplicationState(Tabused)
                        Response.Redirect(returnstring)
                    Case 5
                        Dim returnstring As String = MachineName + "page.aspx?tabref=" + Convert.ToString(Radioselect)
                        'DavesCode.Reuse.ReturnApplicationState(Tabused)
                        Response.Redirect(returnstring)
                    Case 102
                        Application(failstate) = Nothing
                        Application(repairstate) = Nothing
                        Application(suspstate) = Nothing
                        'DavesCode.Reuse.ReturnApplicationState(Tabused)
                        ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                    Case 8
                        Dim returnstring As String = MachineName + "page.aspx?tabref=" + Convert.ToString(Radioselect)
                        'DavesCode.Reuse.ReturnApplicationState(Tabused)
                        Response.Redirect(returnstring)
                End Select

                RadioButtonList1.SelectedIndex = -1
                LogOffButton.BackColor = Drawing.Color.AntiqueWhite
            Else
                Application(appstate) = Nothing
                Dim strScript As String = "<script>"
                strScript += "alert('How have we got here?');"
                strScript += "window.location='"
                strScript += machinelabel
                strScript += "</script>"
                ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)
            End If
        End If


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objconToday = Page.LoadControl("TodayClosedFault.ascx")
        objconToday.ID = "Todaysfaults"
        objconToday.LinacName = MachineName
        PlaceHolder5.Controls.Add(objconToday)

        Dim objCon As UserControl = Page.LoadControl("ViewOpenFaults.ascx")
        CType(objCon, ViewOpenFaults).LinacName = MachineName
        CType(objCon, ViewOpenFaults).ParentControl = "6"
        PlaceHolder1.Controls.Add(objCon)

        Dim objAtlas As UserControl = Page.LoadControl("AtlasEnergyViewuc.ascx")
        CType(objAtlas, AtlasEnergyViewuc).LinacName = MachineName
        PlaceHolder2.Controls.Add(objAtlas)

        Dim objQA As UserControl = Page.LoadControl("Modalitiesuc.ascx")
        CType(objQA, Modalitiesuc).LinacName = MachineName
        PlaceHolder3.Controls.Add(objQA)
        Dim Vctrl As ViewCommentsuc = CType(FindControl("ViewCommentsuc1"), ViewCommentsuc)
        Vctrl.LinacName = MachineName

        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        wctrl.LinacName = MachineName

        AddHandler CType(objCon, ViewOpenFaults).UpdateFaultClosedDisplays, AddressOf Update_Today
        AddHandler CType(objCon, ViewOpenFaults).UpDateDefectDailyDisplay, AddressOf Update_DefectDailyDisplay
        Dim objDefect As UserControl = Page.LoadControl("DefectSave.ascx")
        CType(objDefect, DefectSave).ID = "DefectDisplay"
        CType(objDefect, DefectSave).LinacName = MachineName
        PlaceHolder4.Controls.Add(objDefect)

        Dim Textboxcomment As TextBox = FindControl("CommentBox")
        If Not IsPostBack Then
            If (Not HttpContext.Current.Application(BoxChanged) Is Nothing) Then
                Textboxcomment.Text = Application(BoxChanged).ToString
            Else
                'Textboxcomment.Text = comment
            End If

            Application(faultviewstate) = 1
            Application(qaviewstate) = 1
            Application(energyviewstate) = 1

            'lastState = DavesCode.Reuse.GetLastState(MachineName, 0)
            'StateTextBox.Text = lastState
            'If lastState = ("Engineering Approved") Then
            '    RadioButtonList1.Items(1).Enabled = True
            'End If
            'If Application("Suspended") = 1 Then
            '    RadioButtonList1.Items(1).Enabled = True
            '    RadioButtonList1.Items(2).Enabled = True
            'End If
            StateTextBox.Text = "Linac Unauthorised"
            If Application(suspstate) = 1 Then
                RadioButtonList1.Items.FindByValue(2).Enabled = True
                RadioButtonList1.Items.FindByValue(3).Enabled = True
                StateTextBox.Text = "Suspended"
            End If

            'Dim rtab As String = Application("rppTab")
            If Application(repairstate) = 1 Then
                RadioButtonList1.Items.FindByValue(2).Enabled = True
                StateTextBox.Text = "Engineering Approved"
            End If
            Select Case Application(failstate)
                Case 2
                    RadioButtonList1.Items.FindByValue(2).Enabled = True
                    StateTextBox.Text = "Engineering Approved"
                Case 3
                    RadioButtonList1.Items.FindByValue(2).Enabled = True
                    RadioButtonList1.Items.FindByValue(3).Enabled = True
                    StateTextBox.Text = "Clinical - Not Treating"
                Case Else
                    'StateTextBox.Text = "Linac Unauthorised"
            End Select


        End If

    End Sub

    Protected Sub Faultpanelbutton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Faultpanelbutton.Click
        Dim updatepanel2 As UpdatePanel = FindControl("updatepanel2")
        If Application(faultviewstate) = 1 Then
            updatepanel2.Visible = True
            Application(faultviewstate) = Nothing
            Faultpanelbutton.Text = "Hide Open Faults"
        Else
            updatepanel2.Visible = False
            Application(faultviewstate) = 1
            Faultpanelbutton.Text = "View Open Faults"
        End If

    End Sub

    Protected Sub LogOffButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LogOffButton.Click
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        Dim wclabel As Label = CType(wctrl.FindControl("WarningLabel"), Label)

        DavesCode.Reuse.GetLastTech(MachineName, 0, laststate, lastuser, lastusergroup)
        Radioselect = RadioButtonList1.SelectedItem.Value
        Application(actionstate) = "Confirm"
        Select Case radioselect
            Case 1
                wcbutton.Text = "Go To Engineering Run up"
                RaiseEvent AutoApproved(6, lastuser)
            Case 2
                wcbutton.Text = "Needs Pre-clinical Run up"
                WriteDatauc1.Visible = True
            Case 3
                wcbutton.Text = "Return to clinical"
                WriteDatauc1.Visible = True
            Case 4
                wcbutton.Text = "Go To Planned Maintenance"
                RaiseEvent AutoApproved(6, lastuser)
            Case 5
                wcbutton.Text = "Go To Repair"
                RaiseEvent AutoApproved(6, lastuser)
            Case 102
                wclabel.Text = "Are you sure? This is the End of Day"
                wcbutton.Text = "End of Day"
                WriteDatauc1.Visible = True
            Case 8
                wcbutton.Text = "Go To Training/Development"
                RaiseEvent AutoApproved(6, lastuser)

        End Select

        'WritePQAComments()


    End Sub

    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonList1.SelectedIndexChanged
        If Not SelectCount Then
            LogOffButton.Enabled = True
            LogOffButton.BackColor = Drawing.Color.Yellow
            SelectCount = True
        End If
    End Sub

    Protected Sub ViewAtlasButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewAtlasButton.Click
        Dim updatepanelatlas As UpdatePanel = FindControl("updatepanelatlas")
        If Application(atlasviewstate) = 1 Then
            updatepanelatlas.Visible = True
            Application(atlasviewstate) = Nothing
            ViewAtlasButton.Text = "Hide Atlas Energies"
        Else
            updatepanelatlas.Visible = False
            Application(atlasviewstate) = 1
            ViewAtlasButton.Text = "View Atlas Energies"
        End If
    End Sub

    Protected Sub EditEnergiesButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EditEnergiesButton.Click
        Dim updatepanelenergies As UpdatePanel = FindControl("UpdatePanelQA")

        If Application(energyviewstate) = 1 Then
            updatepanelenergies.Visible = True
            EditEnergiesButton.Text = "Hide Energies"
            Application(energyviewstate) = Nothing
        Else
            updatepanelenergies.Visible = False
            EditEnergiesButton.Text = "View Energies"
            Application(energyviewstate) = 1
        End If

    End Sub
    Protected Sub CommentBox_TextChanged(sender As Object, e As System.EventArgs) Handles CommentBox.TextChanged
        Application(BoxChanged) = CommentBox.Text
        'DavesCode.Reuse.ReturnApplicationState(BoxChanged)
    End Sub

End Class

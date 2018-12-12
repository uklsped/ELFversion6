Imports AjaxControlToolkit
Imports System.Data.SqlClient
Partial Class Traininguc
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
    Private laststate As String
    Private lastuser As String
    Private lastusergroup As String
    Private objconToday As TodayClosedFault
    Private Todaydefect As DefectSave
    Private Todaydefectpark As DefectSavePark
    Dim Master As Object
    Dim BoxChanged As String
    Private Event AutoApproved(ByVal Tab As String, ByVal UserName As String)
    Private tabstate As String
    Private Objcon As ViewOpenFaults
    Dim comment As String


    Public Property LinacName() As String
        Get
            Return MachineName
        End Get
        Set(ByVal value As String)
            MachineName = value
        End Set
    End Property

    'Public Sub UpdateTrainingHandler(ByVal EquipmentID As String)
    Public Sub UpdateReturnButtonsHandler()
        'Now find which user group is logged on
        'removed references to physics QA tab 31 March 2016
        'Removed these commands for E1 and E2
        'If MachineName Like "LA?" Then
        '         RadioButtonList1.Items.FindByValue(2).Enabled = False
        '    End If
        ' RadioButtonList1.Items.FindByValue(1).Enabled = False
        ' RadioButtonList1.Items.FindByValue(3).Enabled = False
        'RadioButtonList1.Items.FindByValue(4).Enabled = False
        'RadioButtonList1.Items.FindByValue(5).Enabled = False
        'RadioButtonList1.Items.FindByValue(6).Enabled = False

        'This is what sets buttons now with addition of E1 and E2
        If Not IsPostBack Then
            If MachineName Like "LA?" Then
                RadioButtonList1.Items.Add(New ListItem("Go To Engineering Run up", "1", False))
                RadioButtonList1.Items.Add(New ListItem("Requires Pre-Clinical Run up", "2", False))
                RadioButtonList1.Items.Add(New ListItem("Hand Back to Clinical", "3", False))
                RadioButtonList1.Items.Add(New ListItem("Go to Planned Maintenance", "4", False))
                RadioButtonList1.Items.Add(New ListItem("Go To Repair", "5", False))
                RadioButtonList1.Items.Add(New ListItem("End of Day", "102", True))
            Else
                RadioButtonList1.Items.Add(New ListItem("Go To Engineering Run up", "1", False))
                RadioButtonList1.Items.Add(New ListItem("Hand Back to Clinical", "3", False))
                RadioButtonList1.Items.Add(New ListItem("Go to Planned Maintenance", "4", False))
                RadioButtonList1.Items.Add(New ListItem("Go To Repair", "5", False))
                RadioButtonList1.Items.Add(New ListItem("End of Day", "102", True))
            End If
        End If

        DavesCode.Reuse.GetLastTech(MachineName, 0, laststate, lastuser, lastusergroup)
        If Application(suspstate) = 1 Then
            RadioButtonList1.Items.FindByValue(3).Enabled = True
            If (lastusergroup = 2) Or (lastusergroup = 4) Then
                RadioButtonList1.Items.FindByValue(4).Enabled = True
                RadioButtonList1.Items.FindByValue(5).Enabled = True
                'If (lastusergroup = 4) Then
                '    RadioButtonList1.Items.FindByValue(6).Enabled = True
                'End If

            End If
            '5 caters for autosign from last tab
        Else
            If ((lastusergroup = 2) Or (lastusergroup = 4) Or (lastusergroup = 5)) And (laststate = "Linac Unauthorised") Then
                RadioButtonList1.Items.FindByValue(1).Enabled = True
                RadioButtonList1.Items.FindByValue(4).Enabled = True
                RadioButtonList1.Items.FindByValue(5).Enabled = True
                'If lastusergroup = 4 Then
                '    RadioButtonList1.Items.FindByValue(6).Enabled = True
                'End If

        ElseIf (laststate = "Engineering Approved") Then
                If MachineName Like "LA?" Then
                RadioButtonList1.Items.FindByValue(2).Enabled = True
                End If
                If (lastusergroup = 2) Or (lastusergroup = 4) Then
                    RadioButtonList1.Items.FindByValue(4).Enabled = True
                    RadioButtonList1.Items.FindByValue(5).Enabled = True
                    'If lastusergroup = 4 Then
                    '    RadioButtonList1.Items.FindByValue(6).Enabled = True
                    'End If
                End If
            End If
        End If
        StateTextBox.Text = laststate

    End Sub

    Protected Sub Update_FaultClosedDisplays(ByVal EquipmentID As String, ByVal incidentID As String)
        If LinacName = EquipmentID Then
            Dim todayfault As TodayClosedFault = PlaceHolder5.FindControl("Todaysfaults")
            todayfault.SetGrid()
            If LinacName Like "T?" Then
                Todaydefectpark = PlaceHolder1.FindControl("DefectDisplay")
                Todaydefectpark.ResetDefectDropDown(incidentID)
            Else
                Todaydefect = PlaceHolder1.FindControl("DefectDisplay")
                Todaydefect.ResetDefectDropDown(incidentID)
            End If

        End If
    End Sub

    ' This updates the defect display on defectsave etc when repeat fault from viewopenfaults
    Protected Sub Update_DefectDailyDisplay(ByVal EquipmentID As String)
        If LinacName = EquipmentID Then
            If LinacName Like "T?" Then
                Todaydefectpark = PlaceHolder1.FindControl("DefectDisplay")
                Todaydefectpark.UpDateDefectsEventHandler()
            Else
                Todaydefect = PlaceHolder1.FindControl("DefectDisplay")
                Todaydefect.UpDateDefectsEventHandler()
            End If

        End If
    End Sub
    Protected Sub Update_ViewOpenFaults(ByVal EquipmentID As String)
        If MachineName = EquipmentID Then
            Objcon = FindControl("ViewOpenFaults")
            Objcon.RebindViewFault()
        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        AddHandler WriteDatauc1.UserApproved, AddressOf UserApprovedEvent
        AddHandler AutoApproved, AddressOf UserApprovedEvent


        appstate = "LogOn" + MachineName
        actionstate = "ActionState" + MachineName
        suspstate = "Suspended" + MachineName
        failstate = "FailState" + MachineName
        repairstate = "rppTab" + MachineName
        faultviewstate = "Faultsee" + MachineName
        atlasviewstate = "Atlassee" + MachineName
        qaviewstate = "QAsee" + MachineName
        BoxChanged = "TABoxChanged" + MachineName
        tabstate = "ActTab" + MachineName
    End Sub

    Public Sub UserApprovedEvent(ByVal Tabused As String, ByVal Userinfo As String)
        Dim machinelabel As String = MachineName & "Page.aspx';"
        Dim username As String = Userinfo
        Dim LinacStateID As String = ""
        Dim suspendvalue As String
        Dim repairvalue As String
        Dim FaultParams As DavesCode.FaultParameters = New DavesCode.FaultParameters()
        Dim EndofDay As Integer = 102
        Dim Recovery As Integer = 101
        'DavesCode.Reuse.GetLastTech(MachineName, 0, laststate, lastuser, lastusergroup)
        'HttpContext.Current.Application(BoxChanged) = Nothing
        If Tabused = "8" Then
            Dim strScript As String = "<script>"
            strScript += "window.location='"
            strScript += machinelabel
            strScript += "</script>"
            Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
            wctrl.Visible = False
            Dim Action As String = Application(actionstate)
            'Dim Textboxcomment As TextBox = FindControl("CommentBox")
            'Dim comment As String = Textboxcomment.Text
            Dim result As Boolean = False

            suspendvalue = Application(suspstate)
            repairvalue = Application(repairstate)
            If (Not HttpContext.Current.Application(BoxChanged) Is Nothing) Then
                comment = HttpContext.Current.Application(BoxChanged).ToString
            Else
                comment = String.Empty
            End If
            If Action = "EndOfDay" Then
                Radioselect = EndofDay
                Action = "Confirm"
                Userinfo = "System"
            ElseIf Action = "False" Then
                Radioselect = Recovery
            Else
                Radioselect = RadioButtonList1.SelectedItem.Value
            End If

            result = DavesCode.NewWriteAux.WriteAuxTables(MachineName, username, comment, Radioselect, Tabused, False, suspendvalue, repairvalue, False, FaultParams)

            If result Then
                If Action = "Confirm" Then
                    'DavesCode.Reuse.ReturnApplicationState(Tabused)
                    'Dim Textboxcomment As TextBox = FindControl("CommentBox")
                    'Dim comment As String = Textboxcomment.Text
                    'suspendvalue = Application(suspstate)
                    'repairvalue = Application(repairstate)
                    'Radioselect = RadioButtonList1.SelectedItem.Value
                    'If this fails it writes an error to file but carries on.
                    'DavesCode.NewWriteAux.WriteAuxTables(MachineName, username, comment, Radioselect, Tabused, False, suspendvalue, repairvalue, False)
                    Application(appstate) = Nothing
                    Application(tabstate) = String.Empty
                    CommentBox.ResetCommentBox()
                    Select Case Radioselect
                        Case 1
                            'LinacStateID = DavesCode.Reuse.SetStatus(username, "Linac Unauthorised", 5, 7, MachineName, 5)
                            Application(failstate) = Nothing
                            Application(repairstate) = Nothing
                            Application(suspstate) = Nothing
                            'DavesCode.Reuse.ReturnApplicationState(Tabused)
                            If lastusergroup <> 3 Then
                                Dim returnstring As String = MachineName + "page.aspx?tabref=" + Convert.ToString(Radioselect)
                                Response.Redirect(returnstring)
                            Else
                                ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                            End If
                        Case 2
                            ' LinacStateID = DavesCode.Reuse.SetStatus(username, "Engineering Approved", 5, 7, MachineName, 5)
                            Application(suspstate) = Nothing
                            Application(failstate) = Nothing
                            Application(repairstate) = 1
                            'DavesCode.Reuse.ReturnApplicationState(Tabused)
                            If lastusergroup = 3 Then
                                Dim returnstring As String = MachineName + "page.aspx?tabref=" + Convert.ToString(Radioselect)
                                Response.Redirect(returnstring)
                            Else
                                ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                            End If

                        Case 3
                            Application(suspstate) = 1
                            Application(failstate) = Nothing
                            'DavesCode.Reuse.ReturnApplicationState(Tabused)
                            If lastusergroup = 3 Then
                                Dim returnstring As String = MachineName + "page.aspx?tabref=" + Convert.ToString(Radioselect)
                                Response.Redirect(returnstring)
                            Else
                                ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                            End If
                        Case 4
                            'DavesCode.Reuse.ReturnApplicationState(Tabused)
                            If lastusergroup <> 3 Then
                                Dim returnstring As String = MachineName + "page.aspx?tabref=" + Convert.ToString(Radioselect)
                                Response.Redirect(returnstring)
                            Else
                                ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                            End If
                        Case 5
                            'LinacStateID = DavesCode.Reuse.SetStatus(username, "Planned Maintenance", 5, 7, MachineName, 5)
                            'ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                            'DavesCode.Reuse.ReturnApplicationState(Tabused)
                            If lastusergroup <> 3 Then
                                Dim returnstring As String = MachineName + "page.aspx?tabref=" + Convert.ToString(Radioselect)
                                Response.Redirect(returnstring)
                            Else
                                ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                            End If
                        Case 6
                            'DavesCode.Reuse.ReturnApplicationState(Tabused)
                            If lastusergroup <> 3 Then
                                Dim returnstring As String = MachineName + "page.aspx?tabref=" + Convert.ToString(Radioselect)
                                Response.Redirect(returnstring)
                            Else
                                ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                            End If
                        Case 102
                            'DavesCode.Reuse.ReturnApplicationState(Tabused)
                            Application(failstate) = Nothing
                            Application(repairstate) = Nothing
                            Application(suspstate) = Nothing
                            ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                    End Select

                    RadioButtonList1.SelectedIndex = -1
                    LogOffButton.BackColor = Drawing.Color.AntiqueWhite
                    'SelectCount = False



                    'Else
                    '    'DavesCode.NewWriteAux.WriteAuxTables(MachineName, username, comment, Radioselect, Tabused, False, suspendvalue, repairvalue, False)
                    '    Application(appstate) = Nothing
                    '    strScript = "<script>"
                    '    strScript += "alert('How have you got to here?');"
                    '    strScript += "window.location='"
                    '    strScript += machinelabel
                    '    strScript += "</script>"
                    '    ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                End If
            End If
        End If


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        WaitButtons("Tech")
        objconToday = Page.LoadControl("TodayClosedFault.ascx")
        objconToday.ID = "Todaysfaults"
        objconToday.LinacName = MachineName
        PlaceHolder5.Controls.Add(objconToday)

        Dim objCon As UserControl = Page.LoadControl("ViewOpenFaults.ascx")
        CType(objCon, ViewOpenFaults).ID = "ViewOpenFaults"
        CType(objCon, ViewOpenFaults).LinacName = MachineName
        CType(objCon, ViewOpenFaults).TabName = "8"
        PlaceHolder1.Controls.Add(objCon)

        Dim objAtlas As UserControl = Page.LoadControl("AtlasEnergyViewuc.ascx")
        CType(objAtlas, AtlasEnergyViewuc).LinacName = MachineName
        PlaceHolder2.Controls.Add(objAtlas)

        Dim objQA As UserControl = Page.LoadControl("Modalitiesuc.ascx")
        CType(objQA, Modalitiesuc).LinacName = MachineName
        CType(objQA, Modalitiesuc).TabName = 8
        PlaceHolder3.Controls.Add(objQA)

        'AddHandler CType(objCon, ViewOpenFaults).UpdateFaultClosedDisplay, AddressOf Update_Today
        AddHandler CType(objCon, ViewOpenFaults).UpDateDefectDailyDisplay, AddressOf Update_DefectDailyDisplay

        Dim objDefect As UserControl

        If MachineName Like "T?" Then
            objDefect = Page.LoadControl("DefectSavePark.ascx")
            CType(objDefect, DefectSavePark).ID = "DefectDisplay"
            CType(objDefect, DefectSavePark).LinacName = MachineName
            CType(objDefect, DefectSavePark).ParentControl = 8
            AddHandler CType(objDefect, DefectSavePark).UpdateFaultClosedDisplays, AddressOf Update_FaultClosedDisplays
            AddHandler CType(objDefect, DefectSavePark).UpdateViewOpenFaults, AddressOf Update_ViewOpenFaults

        Else
            objDefect = Page.LoadControl("DefectSave.ascx")
            CType(objDefect, DefectSave).ID = "DefectDisplay"
            CType(objDefect, DefectSave).LinacName = MachineName
            CType(objDefect, DefectSave).ParentControl = 8
            AddHandler CType(objDefect, DefectSave).UpdateViewOpenFaults, AddressOf Update_ViewOpenFaults
        End If

        PlaceHolder4.Controls.Add(objDefect)
        CommentBox.BoxChanged = BoxChanged
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        'Dim Textboxcomment As TextBox = FindControl("CommentBox")

        wctrl.LinacName = MachineName
        'If Application("SelectCount") = "True" Then
        '    LogOffButton.Enabled = True
        'End If
        'Dim lastState As String
        'Not used?
        'DavesCode.Reuse.GetLastTech(MachineName, 8, laststate, lastuser, lastusergroup)
        If Not IsPostBack Then
            'If MachineName Like "LA?" Then
            '    RadioButtonList1.Items.Add(New ListItem("Go To Engineering Run up", "1", False))
            '    RadioButtonList1.Items.Add(New ListItem("Requires Pre-Clinical Run up", "2", False))
            '    RadioButtonList1.Items.Add(New ListItem("Hand Back to Clinical", "3", False))
            '    RadioButtonList1.Items.Add(New ListItem("Go to Planned Maintenance", "4", False))
            '    RadioButtonList1.Items.Add(New ListItem("Go To Repair", "5", False))
            '    RadioButtonList1.Items.Add(New ListItem("End of Day", "102", False))
            'Else
            '    RadioButtonList1.Items.Add(New ListItem("Go To Engineering Run up", "1", False))
            '    RadioButtonList1.Items.Add(New ListItem("Hand Back to Clinical", "3", False))
            '    RadioButtonList1.Items.Add(New ListItem("Go to Planned Maintenance", "4", False))
            '    RadioButtonList1.Items.Add(New ListItem("Go To Repair", "5", False))
            '    RadioButtonList1.Items.Add(New ListItem("End of Day", "102", False))
            'End If
            'If (Not HttpContext.Current.Application(BoxChanged) Is Nothing) Then
            '    Textboxcomment.Text = Application(BoxChanged).ToString
            'Else
            '    'Textboxcomment.Text = comment
            'End If
            Application(faultviewstate) = 1
            Application(atlasviewstate) = 1
            Application(qaviewstate) = 1
        End If
        'Application("SelectCount") = "False"
        'lastState = DavesCode.Reuse.GetLastState(MachineName, 0)
        'StateTextBox.Text = lastState
        'If lastState = ("Engineering Approved") Then
        '    RadioButtonList1.Items(1).Enabled = True
        'End If
        'Dim test As String = Application("Suspended")
        'If Application("Suspended") = 1 Then
        '    RadioButtonList1.Items(1).Enabled = True
        '    RadioButtonList1.Items(2).Enabled = True
        'End If
        ' Select Application("Failstate")
        '    Case 2
        '        RadioButtonList1.Items(1).Enabled = True
        '    Case 3
        '        RadioButtonList1.Items(1).Enabled = True
        '        RadioButtonList1.Items(2).Enabled = True
        'End Select
        'StateTextBox.Text = "Linac Unauthorised"
        'If Application(suspstate) = 1 Then
        '    RadioButtonList1.Items(1).Enabled = True
        '    RadioButtonList1.Items(2).Enabled = True
        '    StateTextBox.Text = "Suspended"
        'End If


        'If Application(repairstate) = 1 Then
        '    RadioButtonList1.Items(1).Enabled = True
        '    StateTextBox.Text = "Engineering Approved"
        'End If



        'End If

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
        Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
        DavesCode.Reuse.GetLastTech(MachineName, 0, laststate, lastuser, lastusergroup)
        Radioselect = RadioButtonList1.SelectedItem.Value
        Application(actionstate) = "Confirm"
        Select Case Radioselect
            Case 1
                wcbutton.Text = "Go To Engineering Run up"
                If lastusergroup <> 3 Then
                    RaiseEvent AutoApproved(8, lastuser)
                Else
                    WriteDatauc1.Visible = True
                    ForceFocus(wctext)
                End If
            Case 2
                wcbutton.Text = "Needs Pre-clinical Run up"
                If lastusergroup = 3 Then
                    RaiseEvent AutoApproved(8, lastuser)
                Else
                    WriteDatauc1.Visible = True
                    ForceFocus(wctext)
                End If

            Case 3
                wcbutton.Text = "Return to clinical"
                If lastusergroup = 3 Then
                    RaiseEvent AutoApproved(8, lastuser)
                Else
                    WriteDatauc1.Visible = True
                    ForceFocus(wctext)
                End If

            Case 4
                wcbutton.Text = "Go To Planned Maintenance"
                If lastusergroup <> 3 Then
                    RaiseEvent AutoApproved(8, lastuser)
                End If
            Case 5
                wcbutton.Text = "Go To Repair"
                If lastusergroup <> 3 Then
                    RaiseEvent AutoApproved(8, lastuser)
                Else
                    WriteDatauc1.Visible = True
                    ForceFocus(wctext)
                End If
            Case 6
                wcbutton.Text = "Go To Physics QA"
                If lastusergroup <> 3 Then
                    RaiseEvent AutoApproved(8, lastuser)
                Else
                    WriteDatauc1.Visible = True
                    ForceFocus(wctext)
                End If
            Case 102
                wclabel.Text = "Are you sure? This is the End of Day"
                wcbutton.Text = "End of Day"
                WriteDatauc1.Visible = True
                ForceFocus(wctext)
        End Select
        'Application("SelectCount") = "False"

        'WritepmComments()
        'WriteDatauc1.Visible = True

    End Sub


    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonList1.SelectedIndexChanged
        'If Not Application("SelectCount") = "True" Then
        LogOffButton.Enabled = True
        LogOffButton.BackColor = Drawing.Color.Yellow
        'Application("SelectCount") = "True"
        'End If
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

    Protected Sub PhysicsQA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PhysicsQA.Click
        Dim updatepanelQA As UpdatePanel = FindControl("updatepanelQA")
        If Application(qaviewstate) = 1 Then
            updatepanelQA.Visible = True
            Application(qaviewstate) = Nothing
            PhysicsQA.Text = "Hide Physics Energies/Imaging"
        Else
            updatepanelQA.Visible = False
            Application(qaviewstate) = 1
            PhysicsQA.Text = "View Physics Energies/Imaging"
        End If
    End Sub
    'Protected Sub CommentBox_TextChanged(sender As Object, e As System.EventArgs) Handles CommentBox.TextChanged
    '    Application(BoxChanged) = CommentBox.Text
    '    'DavesCode.Reuse.ReturnApplicationState(BoxChanged)
    'End Sub
    Private Sub ForceFocus(ByVal ctrl As Control)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "FocusScript", "setTimeout(function(){$get('" + _
        ctrl.ClientID + "').focus();}, 100);", True)
    End Sub
    Private Sub WaitButtons(ByVal WaitType As String)

        Select Case WaitType
            Case "Acknowledge"
                Dim Accept As Button = FindControl("AcceptOK")
                Dim Cancel As Button = FindControl("btnchkcancel")
                If Not FindControl("AcceptOK") Is Nothing Then
                    Accept.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(Accept, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not FindControl("btnchkcancel") Is Nothing Then
                    Cancel.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(Cancel, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If

            Case "Tech"
                Dim Eng As Button = FindControl("engHandoverButton")
                Dim LogOff As Button = FindControl("LogOffButton")
                Dim Lock As Button = FindControl("LockElf")
                Dim Physics As Button = FindControl("PhysicsQA")
                Dim Atlas As Button = FindControl("ViewAtlasButton")
                Dim FaultPanel As Button = FindControl("FaultPanelButton")
                If Not Eng Is Nothing Then
                    Eng.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(Eng, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not LogOff Is Nothing Then
                    LogOff.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(LogOff, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not Lock Is Nothing Then
                    Lock.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(Lock, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not Physics Is Nothing Then
                    Physics.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(Physics, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not Atlas Is Nothing Then
                    Atlas.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(Atlas, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not FaultPanel Is Nothing Then
                    FaultPanel.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(FaultPanel, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If

            Case "Rad"
                Dim clin As Button = FindControl("clinHandoverButton")
                Dim LogOff As Button = FindControl("LogOff")
                Dim TStart As Button = FindControl("TStart")
                If Not clin Is Nothing Then
                    clin.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(clin, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not LogOff Is Nothing Then
                    LogOff.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(LogOff, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If
                If Not TStart Is Nothing Then
                    TStart.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(TStart, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
                End If

        End Select

    End Sub
End Class


Partial Class Planned_Maintenanceuc
    Inherits System.Web.UI.UserControl

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
    Private objconToday As TodayClosedFault
    Private Todaydefect As DefectSave
    Private Todaydefectpark As DefectSavePark
    Private MainFaultPanel As controls_MainFaultDisplayuc
    Dim Master As Object
    Private laststate As String
    Private lastuser As String
    Private lastusergroup As String
    Private BoxChanged As String
    Private Event AutoApproved(ByVal Tab As String, ByVal UserName As String)
    Public Event BlankGroup(ByVal BlankUser As Integer)
    Private tabstate As String
    Private Objcon As ViewOpenFaults
    Dim FaultParams As DavesCode.FaultParameters = New DavesCode.FaultParameters()
    Public Property LinacName() As String
    Private comment As String
    Const PM As String = "4"
    Dim objReportFault As controls_ReportFaultPopUpuc
    Private WithEvents objdefect As DefectSave = New DefectSave
    Const FAULTPOPUPSELECTED As String = "faultpopupupselected"
    Const QASELECTED As String = "ModalityQApopupselected"
    Const VIEWSTATEKEY_DYNCONTROL As String = "DynamicControlSelection"
    Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
    Dim Modalities As controls_ModalityDisplayuc

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

    Protected Sub Close_ModalityQAPopUp(ByVal EquipmentId As String)
        If LinacName = EquipmentId Then
            DynamicControlSelection = String.Empty
            Dim ModalityQA As controls_ModalityQAPopUpuc = CType(FindControl("ModalityQAPopUpuc1"), controls_ModalityQAPopUpuc)
            PlaceHolderModalities.Controls.Remove(ModalityQA)
        End If
    End Sub

    Protected Sub Update_FaultClosedDisplays(ByVal EquipmentID As String)
        MainFaultPanel = PlaceHolderFaults.FindControl("MainFaultDisplay")
        MainFaultPanel.Update_FaultClosedDisplay(EquipmentID)

    End Sub


    ' This updates the defect display on defectsave etc when repeat fault from viewopenfaults
    Protected Sub Update_DefectDailyDisplay(ByVal EquipmentID As String)
        If LinacName = EquipmentID Then
            MainFaultPanel = PlaceHolderFaults.FindControl("MainFaultDisplay")
            MainFaultPanel.Update_defectsToday(LinacName)

        End If
    End Sub
    Protected Sub Update_ViewOpenFaults(ByVal EquipmentID As String)
        If LinacName = EquipmentID Then
            MainFaultPanel = PlaceHolderFaults.FindControl("MainFaultDisplay")
            MainFaultPanel.Update_OpenConcessions(LinacName)
        End If
    End Sub

    Public Sub UpdateReturnButtonsHandler()

        'getlasttech returns laststate by ref!!!!
        DavesCode.Reuse.GetLastTech(LinacName, 0, laststate, lastuser, lastusergroup)

        StateTextBox.Text = laststate

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        AddHandler WriteDatauc1.UserApproved, AddressOf UserApprovedEvent
        AddHandler AutoApproved, AddressOf UserApprovedEvent
        'AddHandler ReportFaultPopUp1.

        appstate = "LogOn" + LinacName
        actionstate = "ActionState" + LinacName
        suspstate = "Suspended" + LinacName
        failstate = "FailState" + LinacName
        repairstate = "rppTab" + LinacName
        faultviewstate = "Faultsee" + LinacName
        atlasviewstate = "Atlassee" + LinacName
        qaviewstate = "QAsee" + LinacName
        BoxChanged = "PMBoxChanged" + LinacName
        tabstate = "ActTab" + LinacName

    End Sub
    Public Sub UserApprovedEvent(ByVal Tabused As String, ByVal Userinfo As String)
        Dim machinelabel As String = LinacName & "Page.aspx';"
        Dim username As String = Userinfo
        Dim LinacStateID As String = ""
        Dim suspendvalue As String = Nothing
        Dim repairvalue As String = Nothing
        Dim EndofDay As Integer = 102
        Dim Recovery As Integer = 101
        Dim result As Boolean = False
        'This is here to cater for when system is recovering at end of day so no tab is actually loaded.
        Dim actionstate As String = "ActionState" + LinacName
        If Tabused = PM Then
            'Dim Textboxcomment As TextBox = FindControl("CommentBox")
            If (Not HttpContext.Current.Application(BoxChanged) Is Nothing) Then
                comment = HttpContext.Current.Application(BoxChanged).ToString
            Else
                comment = String.Empty
            End If
            Dim strScript As String = "<script>"
            strScript += "window.location='"
            strScript += machinelabel
            strScript += "</script>"
            Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
            wctrl.Visible = False
            Dim Action As String = Application(actionstate)
            suspendvalue = Application(suspstate)
            repairvalue = Application(repairstate)
            If Action = "EndOfDay" Then
                Radioselect = EndofDay
                Action = "Confirm"

            ElseIf Action = "False" Then
                Radioselect = Recovery
            Else
                Radioselect = RadioButtonList1.SelectedItem.Value
            End If

            result = DavesCode.NewWriteAux.WriteAuxTables(LinacName, username, comment, Radioselect, Tabused, False, suspendvalue, repairvalue, False, FaultParams)
            If result Then
                If Action = "Confirm" Then

                    'DavesCode.Reuse.ReturnApplicationState(Tabused)
                    'Dim Textboxcomment As TextBox = FindControl("CommentBox")
                    'Dim comment As String = Textboxcomment.Text


                    'result = DavesCode.NewWriteAux.WriteAuxTables(LinacName, username, comment, Radioselect, Tabused, False, suspendvalue, repairvalue, False, FaultParams)
                    Application(tabstate) = String.Empty
                    Application(appstate) = Nothing
                    CommentBox.ResetCommentBox(String.Empty)
                    Select Case Radioselect
                        Case 1
                            Application(failstate) = Nothing
                            Application(repairstate) = Nothing
                            Application(suspstate) = Nothing
                            Dim returnstring As String = LinacName + "page.aspx?tabref=" + Convert.ToString(Radioselect)
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
                        Case 5
                            Dim returnstring As String = LinacName + "page.aspx?tabref=" + Convert.ToString(Radioselect)
                            'DavesCode.Reuse.ReturnApplicationState(Tabused)
                            Response.Redirect(returnstring)
                        Case 6
                            Dim returnstring As String = LinacName + "page.aspx?tabref=" + Convert.ToString(Radioselect)
                            'DavesCode.Reuse.ReturnApplicationState(Tabused)
                            Response.Redirect(returnstring)
                        Case 102
                            Application(failstate) = Nothing
                            Application(repairstate) = Nothing
                            Application(suspstate) = Nothing
                            'DavesCode.Reuse.ReturnApplicationState(Tabused)
                            ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                        Case 8
                            Dim returnstring As String = LinacName + "page.aspx?tabref=" + Convert.ToString(Radioselect)
                            'DavesCode.Reuse.ReturnApplicationState(Tabused)
                            Response.Redirect(returnstring)
                    End Select
                    LogOffButton.BackColor = Drawing.Color.AntiqueWhite
                    RadioButtonList1.SelectedIndex = -1

                Else
                    'Nothing needs to be done here now. This is called from end of day and recovery 
                    'And they reset all of the application states when this returns to them.

                End If
            Else
                RaiseLogOffError()
            End If
        End If

    End Sub
    Protected Sub RaiseLogOffError()
        Dim machinelabel As String = LinacName & "Page.aspx';"
        Dim strScript As String = "<script>"
        strScript += "alert('Problem Logging Off. Please inform Engineering');"
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        WaitButtons("Tech")
        CommentBox.BoxChanged = BoxChanged

        Dim objMFG As controls_MainFaultDisplayuc = Page.LoadControl("controls\MainFaultDisplayuc.ascx")
        CType(objMFG, controls_MainFaultDisplayuc).LinacName = LinacName
        CType(objMFG, controls_MainFaultDisplayuc).ID = "MainFaultDisplay"
        CType(objMFG, controls_MainFaultDisplayuc).ParentControl = PM
        AddHandler objMFG.Mainfaultdisplay_UpdateClosedFaultDisplay, AddressOf Update_FaultClosedDisplays
        'AddHandler objMFG.Mainfaultdisplay_UpdateClosedFaultDisplay, AddressOf Update_ClosedFaultDisplay
        PlaceHolderFaults.Controls.Add(objMFG)

        Dim Vctrl As ViewCommentsuc = CType(FindControl("ViewCommentsuc1"), ViewCommentsuc)
        Vctrl.LinacName = LinacName
        Dim lockctrl As LockElfuc = CType(FindControl("LockElfuc1"), LockElfuc)
        lockctrl.LinacName = LinacName
        Dim ReportFault As controls_ReportAFaultuc = CType(FindControl("ReportAFaultuc1"), controls_ReportAFaultuc)
        ReportFault.LinacName = LinacName
        ReportFault.ParentControl = PM
        AddHandler ReportFault.ReportAFault_UpdateDailyDefectDisplay, AddressOf Update_DefectDailyDisplay
        AddHandler ReportFault.ReportAFault_UpDateViewOpenFaults, AddressOf Update_ViewOpenFaults

        Modalities = Page.LoadControl("controls/ModalityDisplayuc.ascx")
        CType(Modalities, controls_ModalityDisplayuc).LinacName = LinacName
        CType(Modalities, controls_ModalityDisplayuc).ID = "ModalityDisplay"
        If Application(suspstate) = 1 Then
            CType(Modalities, controls_ModalityDisplayuc).Mode = "Suspended"
        Else
            CType(Modalities, controls_ModalityDisplayuc).Mode = "Linac Unauthorised"
        End If
        CType(Modalities, controls_ModalityDisplayuc).ConnectionString = connectionString
        ModalityPlaceholder.Controls.Add(Modalities)
        ModalityDisplayPanel.Visible = True

        Select Case Me.DynamicControlSelection
        '    Case REPEATFAULTSELECTED
            '        LoadRepeatFaultTable(HiddenIncidentID.Value, HiddenConcessionNumber.Value)
            Case FAULTPOPUPSELECTED
                '        'LoadFaultTable(Label2.Text)
                'ReloadConcessionPopUp()

                'Dim objReportFault As controls_ReportFaultPopUpuc = Page.LoadControl("controls\ReportFaultPopUpuc.ascx")
                'objReportFault.LinacName = LinacName
                'objReportFault.ID = "ReportFaultPopupuc"
                'objReportFault.ParentControl = PM
                ''objReportFault.Visible = False
                'AddHandler CType(objReportFault, controls_ReportFaultPopUpuc).UpDateDefectDailyDisplay, AddressOf Update_DefectDailyDisplay
                'AddHandler CType(objReportFault, controls_ReportFaultPopUpuc).UpdateViewOpenFaults, AddressOf Update_ViewOpenFaults
                'AddHandler CType(objReportFault, controls_ReportFaultPopUpuc).CloseReportFaultPopUp, AddressOf Close_ReportFaultPopUp
                'ReportFaultPopupPlaceHolder.Controls.Add(objReportFault)
            Case QASELECTED
                Dim ObjQA As controls_ModalityQAPopUpuc = Page.LoadControl("controls\ModalityQAPopUpuc.ascx")
                ObjQA.LinacName = LinacName
                ObjQA.ParentControl = PM
                ObjQA.ID = "ModalityQAPopUpuc1"
                DynamicControlSelection = QASELECTED
                AddHandler ObjQA.CloseModalityQAPopUpTab, AddressOf Close_ModalityQAPopUp
                PlaceHolderModalities.Controls.Add(ObjQA)
            Case Else
                '        'no dynamic controls need to be loaded...yet
        End Select
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        wctrl.LinacName = LinacName
        If Not IsPostBack Then
            If Not LinacName Like "T?" Then
                PhysicsQA.Visible = True
            End If
            RadioButtonList1.Items.Add(New ListItem("Go To Engineering Run up", "1", True))
            RadioButtonList1.Items.Add(New ListItem("Hand Back to Clinical", "3", False))
            RadioButtonList1.Items.Add(New ListItem("Go To Repair", "5", True))
            RadioButtonList1.Items.Add(New ListItem("Go To Training/Development", "8", True))
            RadioButtonList1.Items.Add(New ListItem("End of Day", "102", True))
            'End If

            Application(faultviewstate) = 1
            Application(atlasviewstate) = 1
            Application(qaviewstate) = 1

            StateTextBox.Text = "Linac Unauthorised"
            If Application(suspstate) = 1 Then
                'remove la option
                'If LinacName Like "LA?" Then
                '    RadioButtonList1.Items.FindByValue(2).Enabled = True
                'End If
                RadioButtonList1.Items.FindByValue(3).Enabled = True
                StateTextBox.Text = "Suspended"

                'ElseIf Application(repairstate) = 1 Then
                '    If LinacName Like "LA?" Then
                '        RadioButtonList1.Items.FindByValue(2).Enabled = True
                '        StateTextBox.Text = "Engineering Approved"
                '    End If
            End If

        End If

    End Sub

    Protected Sub LogOffButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LogOffButton.Click
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        Dim wclabel As Label = CType(wctrl.FindControl("WarningLabel"), Label)
        Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
        Dim laststate As String = ""
        Dim lastusername As String = ""
        Dim lastusergroup As Integer

        DavesCode.Reuse.GetLastTech(LinacName, 0, laststate, lastusername, lastusergroup)

        Radioselect = RadioButtonList1.SelectedItem.Value
        Application(actionstate) = "Confirm"
        Select Case Radioselect
            Case 1
                wcbutton.Text = "Go To Engineering Run up"
                RaiseEvent AutoApproved(PM, lastusername)
            'Case 2
            '    wcbutton.Text = "Needs Pre-clinical Run up"
            '    WriteDatauc1.Visible = True
            '    ForceFocus(wctext)
            Case 3
                wcbutton.Text = "Return to clinical"
                WriteDatauc1.Visible = True
                ForceFocus(wctext)
            Case 5
                wcbutton.Text = "Go To Repair"
                RaiseEvent AutoApproved(PM, lastusername)
            Case 6
                If lastusergroup = 4 Then
                    RaiseEvent AutoApproved(PM, lastusername)
                Else
                    WriteDatauc1.Visible = True
                    ForceFocus(wctext)
                End If
            Case 102
                wclabel.Text = "Are you sure? This is the End of Day"
                wcbutton.Text = "End of Day"
                WriteDatauc1.Visible = True
                ForceFocus(wctext)
            Case 8
                wcbutton.Text = "Go To Training/Development"
                RaiseEvent AutoApproved(PM, lastusername)
        End Select

    End Sub


    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonList1.SelectedIndexChanged

        LogOffButton.Enabled = True
        LogOffButton.BackColor = Drawing.Color.Yellow

    End Sub

    Protected Sub PhysicsQA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PhysicsQA.Click
        Dim ObjQA As controls_ModalityQAPopUpuc = Page.LoadControl("controls\ModalityQAPopUpuc.ascx")
        ObjQA.LinacName = LinacName
        ObjQA.ParentControl = PM
        ObjQA.ID = "ModalityQAPopUpuc1"
        DynamicControlSelection = QASELECTED
        AddHandler ObjQA.CloseModalityQAPopUpTab, AddressOf Close_ModalityQAPopUp
        PlaceHolderModalities.Controls.Add(ObjQA)

    End Sub

    Protected Sub LockElf_Click(sender As Object, e As System.EventArgs) Handles LockElf.Click
        Dim lockctrl As LockElfuc = CType(FindControl("LockElfuc1"), LockElfuc)
        Dim lockctrltext As TextBox = CType(lockctrl.FindControl("txtchkUserName"), TextBox)
        Dim suspendvalue As String
        Dim repairvalue As String
        Dim username As String = "Lockuser"
        'Dim Textboxcomment As TextBox = FindControl("CommentBox")
        Dim comment As String
        comment = CommentBox.Currentcomment
        suspendvalue = Application(suspstate)
        repairvalue = Application(repairstate)
        Dim tabused As Integer = 4
        Dim radioselect As Integer = 101
        Dim success As Boolean = False
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        'has to be tablable to cope with either tab 1 or 7 control
        success = DavesCode.NewWriteAux.WriteAuxTables(LinacName, username, comment, radioselect, tabused, False, suspendvalue, repairvalue, True, FaultParams)

        If success Then
            RaiseEvent BlankGroup(0)
            lockctrl.Visible = True
            ForceFocus(lockctrltext)
        Else
            RaiseLockError()
        End If

    End Sub
    Protected Sub RaiseLockError()
        Dim strScript As String = "<script>"

        strScript += "alert('Problem Locking Elf. Please inform system administator');"
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(LockElf, Me.GetType(), "JSCR", strScript.ToString(), False)
    End Sub

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

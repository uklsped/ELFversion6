Imports AjaxControlToolkit
Imports System.Data.SqlClient
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

    Protected Sub Update_Today(ByVal EquipmentID As String, ByVal incidentID As String)
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

    'Protected Sub Update_TodayClosedFault(ByVal EquipmentID As String)
    '    If LinacName = EquipmentID Then

    '    End If
    'End Sub
    ' This updates the defect display on defectsave etc when repeat fault from viewopenfaults
    Protected Sub Update_Defect(ByVal EquipmentID As String)
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
        If LinacName = EquipmentID Then
            Objcon = FindControl("ViewOpenFaults")
            Objcon.RebindViewFault()
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
        Dim result As Boolean = False

        If Tabused = "4" Then
            Dim Textboxcomment As TextBox = FindControl("CommentBox")
            Dim comment As String = Textboxcomment.Text
            Dim strScript As String = "<script>"
            strScript += "window.location='"
            strScript += machinelabel
            strScript += "</script>"
            Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
            wctrl.Visible = False
            Dim Action As String = Application(actionstate)
            result = DavesCode.NewWriteAux.WriteAuxTables(LinacName, username, comment, Radioselect, Tabused, False, suspendvalue, repairvalue, False, FaultParams)
            If result Then
                If Action = "Confirm" Then
                    'DavesCode.Reuse.ReturnApplicationState(Tabused)
                    'Dim Textboxcomment As TextBox = FindControl("CommentBox")
                    'Dim comment As String = Textboxcomment.Text
                    suspendvalue = Application(suspstate)
                    repairvalue = Application(repairstate)
                    Radioselect = RadioButtonList1.SelectedItem.Value

                    'result = DavesCode.NewWriteAux.WriteAuxTables(LinacName, username, comment, Radioselect, Tabused, False, suspendvalue, repairvalue, False, FaultParams)
                    Application(tabstate) = String.Empty
                    Application(appstate) = Nothing
                    HttpContext.Current.Application(BoxChanged) = Nothing
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

                    'Else
                    '    'This now responds to end of day at midnight and on start up
                    '    DavesCode.NewWriteAux.WriteAuxTables(LinacName, username, comment, EndofDay, Tabused, False, suspendvalue, repairvalue, False)
                    'Application(appstate) = Nothing
                    'strScript = "<script>"
                    'strScript += "alert('How have you got to here?');"
                    'strScript += "window.location='"
                    'strScript += machinelabel
                    'strScript += "</script>"
                    'ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)

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
        objconToday = Page.LoadControl("TodayClosedFault.ascx")
        objconToday.ID = "Todaysfaults"
        objconToday.LinacName = LinacName
        PlaceHolder5.Controls.Add(objconToday)

        Objcon = Page.LoadControl("ViewOpenFaults.ascx")
        CType(Objcon, ViewOpenFaults).LinacName = LinacName
        CType(Objcon, ViewOpenFaults).TabName = "5"
        CType(Objcon, ViewOpenFaults).ID = "ViewOpenFaults"
        PlaceHolder1.Controls.Add(Objcon)

        Dim objAtlas As UserControl = Page.LoadControl("AtlasEnergyViewuc.ascx")
        CType(objAtlas, AtlasEnergyViewuc).LinacName = LinacName
        PlaceHolder2.Controls.Add(objAtlas)

        Dim objQA As UserControl = Page.LoadControl("WebUserControl2.ascx")
        CType(objQA, WebUserControl2).LinacName = LinacName
        CType(objQA, WebUserControl2).TabName = 4
        PlaceHolder3.Controls.Add(objQA)
        Dim Vctrl As ViewCommentsuc = CType(FindControl("ViewCommentsuc1"), ViewCommentsuc)
        Vctrl.LinacName = LinacName
        Dim lockctrl As LockElfuc = CType(FindControl("LockElfuc1"), LockElfuc)
        lockctrl.LinacName = LinacName
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        wctrl.LinacName = LinacName
        Dim objDefect As UserControl
        AddHandler CType(Objcon, ViewOpenFaults).UpDateFaultClosedDisplay, AddressOf Update_Today
        AddHandler CType(Objcon, ViewOpenFaults).UpDateDefectDisplay, AddressOf Update_Defect
        If LinacName Like "T?" Then
            objDefect = Page.LoadControl("DefectSavePark.ascx")
            CType(objDefect, DefectSavePark).ID = "DefectDisplay"
            CType(objDefect, DefectSavePark).LinacName = LinacName
            CType(objDefect, DefectSavePark).ParentControl = 4
            'AddHandler CType(objDefect, DefectSavePark).UpDateDefect, AddressOf Update_Today
            AddHandler CType(objDefect, DefectSavePark).UpdateViewFault, AddressOf Update_ViewOpenFaults

        Else
            objDefect = Page.LoadControl("DefectSave.ascx")
            CType(objDefect, DefectSave).ID = "DefectDisplay"
            CType(objDefect, DefectSave).LinacName = LinacName
            CType(objDefect, DefectSave).ParentControl = 4
            'AddHandler CType(objDefect, DefectSave).UpDateDefect, AddressOf Update_Today
            AddHandler CType(objDefect, DefectSave).UpdateViewFault, AddressOf Update_ViewOpenFaults
        End If

        PlaceHolder4.Controls.Add(objDefect)
        Dim Textboxcomment As TextBox = FindControl("CommentBox")

        If Not IsPostBack Then
            If LinacName Like "LA?" Then
                RadioButtonList1.Items.Add(New ListItem("Go To Engineering Run up", "1", True))
                RadioButtonList1.Items.Add(New ListItem("Requires Pre-Clinical Run up", "2", False))
                RadioButtonList1.Items.Add(New ListItem("Hand Back to Clinical", "3", False))
                RadioButtonList1.Items.Add(New ListItem("Go To Repair", "5", True))
                RadioButtonList1.Items.Add(New ListItem("Go To Training/Development", "8", True))
                RadioButtonList1.Items.Add(New ListItem("End of Day", "102", True))
            Else
                RadioButtonList1.Items.Add(New ListItem("Go To Engineering Run up", "1", True))
                RadioButtonList1.Items.Add(New ListItem("Hand Back to Clinical", "3", False))
                RadioButtonList1.Items.Add(New ListItem("Go To Repair", "5", True))
                RadioButtonList1.Items.Add(New ListItem("Go To Training/Development", "8", True))
                RadioButtonList1.Items.Add(New ListItem("End of Day", "102", True))
            End If

            If (Not HttpContext.Current.Application(BoxChanged) Is Nothing) Then
                Textboxcomment.Text = Application(BoxChanged).ToString
            Else
                'Textboxcomment.Text = comment
            End If

            Application(faultviewstate) = 1
            Application(atlasviewstate) = 1
            Application(qaviewstate) = 1

            StateTextBox.Text = "Linac Unauthorised"
            If Application(suspstate) = 1 Then
                If LinacName Like "LA?" Then
                    RadioButtonList1.Items.FindByValue(2).Enabled = True
                End If
                RadioButtonList1.Items.FindByValue(3).Enabled = True
                StateTextBox.Text = "Suspended"

            ElseIf Application(repairstate) = 1 Then
                If LinacName Like "LA?" Then
                    RadioButtonList1.Items.FindByValue(2).Enabled = True
                    StateTextBox.Text = "Engineering Approved"
                End If
            End If

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
                RaiseEvent AutoApproved(4, lastusername)
            Case 2
                wcbutton.Text = "Needs Pre-clinical Run up"
                WriteDatauc1.Visible = True
                ForceFocus(wctext)
            Case 3
                wcbutton.Text = "Return to clinical"
                WriteDatauc1.Visible = True
                ForceFocus(wctext)
            Case 5
                wcbutton.Text = "Go To Repair"
                RaiseEvent AutoApproved(4, lastusername)
            Case 6
                If lastusergroup = 4 Then
                    RaiseEvent AutoApproved(4, lastusername)
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
                RaiseEvent AutoApproved(4, lastusername)
        End Select

    End Sub


    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonList1.SelectedIndexChanged

        LogOffButton.Enabled = True
        LogOffButton.BackColor = Drawing.Color.Yellow

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

    Protected Sub LockElf_Click(sender As Object, e As System.EventArgs) Handles LockElf.Click
        Dim lockctrl As LockElfuc = CType(FindControl("LockElfuc1"), LockElfuc)
        Dim lockctrltext As TextBox = CType(lockctrl.FindControl("txtchkUserName"), TextBox)
        Dim suspendvalue As String
        Dim repairvalue As String
        Dim username As String = "Lockuser"
        Dim Textboxcomment As TextBox = FindControl("CommentBox")
        Dim comment As String = Textboxcomment.Text
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
    Protected Sub CommentBox_TextChanged(sender As Object, e As System.EventArgs) Handles CommentBox.TextChanged
        Application(BoxChanged) = CommentBox.Text
        'DavesCode.Reuse.ReturnApplicationState(BoxChanged)
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

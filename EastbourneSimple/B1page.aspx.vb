Imports System.Data.SqlClient
Imports System.Data
Imports AjaxControlToolkit
Imports System.Web.Services

Partial Public Class B1page
    Inherits System.Web.UI.Page
    Private EquipmentID As String = "B1"
    'Private IPaddress As String = "10.179.85.164"
    Private Reg As String
    Private refpage As String
    Private tabref As String
    Private tabIndex As String
    Private comment As String
    Private mpContentPlaceHolder As ContentPlaceHolder
    Private wctrl As WriteDatauc
    Private cusctrl As AcceptLinac
    Private LinacFlag As String = "StateB1"
    Private appstate As String = "LogOnB1"
    Private suspstate As String = "SuspendedB1"
    Private actionstate As String = "ActionStateB1"
    Private repairstate As String = "rppTabB1"
    Private failstate As String = "FailStateB1"
    Private clinicalstate As String = "ClinicalOnB1"
    Private treatmentstate As String = "TreatmentB1"
    Private activetabstate As String = "ActTabB1"
    Private runupcontrolId As String = "ERunupUserControl1"
    Private preclincontrolID As String = "PreclinUserControl1"
    Private ClinicalUserControlID As String = "ClinicalUserControl1"
    Private PlannedMaintenanceControlID As String = "PlannedMaintenanceuc1"
    Private repcontrolId As String = "repairuc1"
    Private webusercontrol21ID As String = "webUserControl21"
    Private physicscontrolID As String = "PhysicsQAuc1"
    Private writedatacontrolID As String = "Writedatauc1"
    Private emergencycontrolID As String = "ERunupUserControl2"
    Private trainingcontrolID As String = "Traininguc1"
    Public Event NoApprove()
    Public Event LAQA As EventHandler
    Private TodayTraining As Traininguc
    Private TodayPM As Planned_Maintenanceuc
    Private TodayRep As Repairuc
    Private faultstate As String = "faultstateB1"
    Private linacloaded As String = "B1loaded"
    Private returnclinical As String = "ReturnClinicalB1"
    Private technicalstate As String = "techstateB1"
    Private recover As String = Nothing
    Private lsctrl As LinacStatusuc
    Private RegistrationState As String = "regstateB1"
    Private loadup As String = Nothing
    Public Event EngRunuploaded(ByVal connectionString As String)
    Private Modalities As controls_ModalityDisplayuc
    Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
    'Public Event DayEnded(ByVal Tab As String, ByVal UserName As String)

    Protected Sub Update_ReturnButtons()

        'If EquipmentID = MachineName Then
        Dim tabActive As Integer
        tabActive = tcl.ActiveTabIndex
        Dim containerID As String = "TabContent" & tabActive
        Dim panel As Panel = tcl.ActiveTab.FindControl(containerID)
        If (Not panel Is Nothing) Then
            Select Case tabActive
                Case 4
                    TodayPM = tcl.ActiveTab.FindControl(PlannedMaintenanceControlID)
                    TodayPM.UpdateReturnButtonsHandler()
                Case 5
                    TodayRep = tcl.ActiveTab.FindControl(repcontrolId)
                    TodayRep.UpdateReturnButtonsHandler()
                Case 8
                    TodayTraining = tcl.ActiveTab.FindControl(trainingcontrolID)
                    TodayTraining.UpdateReturnButtonsHandler()
            End Select

        End If
        'End If

    End Sub

    'Protected Property ActiveTabIndex() As Integer
    '    Get
    '        Return If(ViewState("ActiveTabIndex") IsNot Nothing, Convert.ToInt32(ViewState("ActiveTabIndex")), 0)
    '    End Get
    '    Set(ByVal value As Integer)
    '        ViewState("ActiveTabIndex") = value
    '    End Set
    'End Property


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        mpContentPlaceHolder = CType(Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        If Not mpContentPlaceHolder Is Nothing Then
            wctrl = CType(mpContentPlaceHolder.FindControl("Writedatauc1"), WriteDatauc)
            AddHandler wctrl.UserApproved, AddressOf UserApprovedEvent
            'lsctrl = CType(mpContentPlaceHolder.FindControl("LinacStatusuc1"), LinacStatusuc)
            'AddHandler lsctrl.ResetTab, AddressOf LaunchTab
        End If
        AddHandler NoApprove, AddressOf ClinicalApprovedEvent


    End Sub

    Protected Sub UserApprovedEvent(ByVal Tabset As String, ByVal Userinfo As String)

        Dim tabcontrol As String = Tabset
        Dim Action As String = Application(actionstate)
        If tabcontrol = "EndDay" Then

            mpContentPlaceHolder = CType(Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
            If Not mpContentPlaceHolder Is Nothing Then
                wctrl = CType(mpContentPlaceHolder.FindControl("Writedatauc1"), WriteDatauc)
                wctrl.Visible = False
                If Action = "Confirm" Then
                    Try
                        Dim lastState As String
                        'tick looks at fault table make these consistent
                        lastState = DavesCode.Reuse.GetLastState(EquipmentID, 0)
                        Statelabel.Text = lastState
                        'Shouldn't allow end of day for fault. Remove this code at some point but disable elsewhere for now 28/4/16
                        Select Case lastState
                            Case "Repair", "Fault"
                                'This should be Fault because that's the only state that should be allowed to not be reset
                                DavesCode.Reuse.SetStatus("No User", "Fault", 5, 7, EquipmentID, 0)
                                Application(LinacFlag) = lastState
                            Case Else
                                DavesCode.Reuse.SetStatus(Userinfo, "Linac Unauthorised", 5, 102, EquipmentID, 0)
                                Application(LinacFlag) = "Linac Unauthorised"
                                Application(suspstate) = Nothing
                                Application(appstate) = Nothing
                                Application(failstate) = Nothing
                                Application(clinicalstate) = Nothing
                                Application(repairstate) = Nothing
                                Application(treatmentstate) = "Yes"
                                Application(activetabstate) = Nothing
                        End Select
                    Finally


                        Dim returnstring As String = EquipmentID + "page.aspx"
                        'DavesCode.Reuse.ReturnApplicationState(tabcontrol)
                        Response.Redirect(returnstring)

                    End Try

                End If
            End If
        End If
    End Sub

    Protected Sub ClinicalApprovedEvent()
        Dim tabActive As Integer
        tabActive = tcl.ActiveTabIndex
        Dim containerID As String = "TabContent" & tabActive
        Dim ClinicalUserControlid As String = "ClinicalUserControl1"
        Dim panel As Panel = tcl.ActiveTab.FindControl(containerID)
        Dim clincontrol As UserControl = tcl.ActiveTab.FindControl(ClinicalUserControlid)
        Dim Acceptcontrolid As String = "AcceptLinac3"
        Dim acceptcontrol As AcceptLinac = tcl.ActiveTab.FindControl(Acceptcontrolid)
        If (Not panel Is Nothing) Then
            'clincontrol.Visible = True
            acceptcontrol.Visible = False
        End If

    End Sub

    Protected Sub SetUser(ByVal usergroup As Integer)
        Dim usergroupname As String = ""
        Select Case usergroup
            Case 0
                usergroupname = String.Empty
            Case 2
                usergroupname = "Engineer"
            Case 3
                usergroupname = "Radiographer"
            Case 4
                usergroupname = "Physicist"
        End Select

        UserGroupLabel.Text = usergroupname

    End Sub

    Public Sub SetModalities(ByVal connectionString As String)
        'If Application(appstate) Then
        Modalities = Page.LoadControl("controls/ModalityDisplayuc.ascx")
        CType(Modalities, controls_ModalityDisplayuc).LinacName = EquipmentID
        CType(Modalities, controls_ModalityDisplayuc).ID = "ModalityDisplay"
        CType(Modalities, controls_ModalityDisplayuc).Mode = Statelabel.Text
        CType(Modalities, controls_ModalityDisplayuc).ConnectionString = connectionString
        ModalityPlaceholder.Controls.Add(Modalities)
        ModalityDisplayPanel.Visible = True
        'End If

    End Sub

    Protected Sub NullPhysics()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Reload As Boolean = False
        'Dim panelcontrol As TabPanel
        Dim Fcancel As String = ""
        AddHandler AcceptLinac1.ShowName, AddressOf SetUser
        AddHandler AcceptLinac4.ShowName, AddressOf SetUser
        AddHandler AcceptLinac5.ShowName, AddressOf SetUser
        'AddHandler AcceptLinac6.ShowName, AddressOf SetUser disable because of QA
        AddHandler AcceptLinac8.ShowName, AddressOf SetUser
        AddHandler AcceptLinac3.ClinicalApproved, AddressOf ClinicalApprovedEvent
        AddHandler AcceptLinac3.AcknowledgeEnergies, AddressOf AcknowledgeEnergies
        AddHandler AcceptLinac3.NullPhysics, AddressOf NullPhysics
        'AddHandler AcceptLinac8.UpdateTraining, AddressOf Update_Training
        AddHandler AcceptLinac4.UpdateReturnButtons, AddressOf Update_ReturnButtons
        AddHandler AcceptLinac5.UpdateReturnButtons, AddressOf Update_ReturnButtons
        AddHandler AcceptLinac8.UpdateReturnButtons, AddressOf Update_ReturnButtons
        AddHandler LinacStatusuc1.Resetstatus, AddressOf LaunchTab
        AddHandler PlannedMaintenanceuc1.BlankGroup, AddressOf SetUser
        AddHandler Repairuc1.BlankGroup, AddressOf SetUser
        AddHandler ErunupUserControl1.BlankGroup, AddressOf SetUser
        AddHandler AcceptLinac1.SetModalities, AddressOf SetModalities
        AddHandler AcceptLinac3.SetModalities, AddressOf SetModalities
        Dim ResetDay As String = Nothing
        Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString



        'Dim EndofDayWait As Button = FindControl("EndOfDay")
        'If Not FindControl("EndOfDayWait") Is Nothing Then
        EndOfDay.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(EndOfDay, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
        'End If


        'If PreviousPage IsNot Nothing Then
        refpage = Nothing
        tabIndex = Nothing
        comment = Nothing
        tabref = Nothing

        Dim lastState As String
        lastState = DavesCode.Reuse.GetLastState(EquipmentID, 0)
        Statelabel.Text = lastState
        'This block was wrong for Application names and anyway is unnecessary apart from setting statelabel to laststate
        'If Application("Suspended") = 1 Then
        '    Statelabel.Text = "Suspended"
        'ElseIf lastState = "Clinical" Then
        '    Statelabel.Text = lastState
        'ElseIf Application("rppTab") = 1 Then
        '    Statelabel.Text = "Engineering Approved"
        'Else
        '    'Statelabel.Text = "Linac Unauthorised"
        '    Statelabel.Text = lastState
        'End If


        'DavesCode.Reuse.ReturnApplicationState("First Start")

        If Not IsPostBack Then
            'Dim loadstate As String = Application("B1loaded")
            'If loadstate = "Albert" Then
            'Statelabel.Text = loadstate
            'End If

            'added 16/11/17 to check for end of day
            If Not Request.QueryString("loadup") Is Nothing Then

                ResetDay = DavesCode.Reuse.GetLastTime(EquipmentID, 0)

                Select Case ResetDay
                    Case "Ignore"
                        'Ignore
                    Case "EndDay"
                        EndofDayElf(ResetDay)
                    Case "Error"
                        'Do nothing
                End Select
            End If

            'Dim userIP As String = DavesCode.Reuse.GetIPAddress()
            'Label5.Text = userIP
            'Label5.Text = DavesCode.Reuse.GetLastTime(EquipmentID, 0)
            ' 20 April handle direct open to repair page

            If Not Request.QueryString("recovered") Is Nothing Then
                recover = Request.QueryString("recovered").ToString
            End If
            TabPanel0.Enabled = True
            EndOfDay.Visible = True
            'This is to check if request is coming from tab that doesn't need new sign in and automatically launches tab. Probably incorporate in stuff above
            tabref = Nothing
            Dim tabpicked As String = Nothing
            If Not Request.QueryString("tabref") Is Nothing Then
                tabref = Request.QueryString("tabref").ToString
                If tabref = 0 Then
                    tabpicked = Nothing
                Else
                    tcl.ActiveTabIndex = tabref
                    tabpicked = tabref
                End If
            ElseIf Not Request.QueryString("tabclicked") Is Nothing Then
                tabpicked = Request.QueryString("tabclicked").ToString
                tcl.ActiveTabIndex = tabpicked
                If tabpicked = 0 Then
                    tabpicked = Application(activetabstate)
                    Application(activetabstate) = tcl.ActiveTabIndex
                    'tabpicked = CType(Session.Item("ActiveTabIdx"), Integer)
                    'Session.Item("ActiveTabIdx") = tcl.ActiveTabIndex
                End If
            End If
            If Not tabpicked Is Nothing Then
                Select Case tabpicked
                    Case 1
                        TabPanel2.Enabled = False
                        TabPanel3.Enabled = False
                        TabPanel4.Enabled = "false"
                        TabPanel5.Enabled = "false"
                        'TabPanel6.Enabled = "false"
                        TabPanel7.Enabled = False
                        TabPanel8.Enabled = "false"
                    Case 2
                        TabPanel1.Enabled = "false"
                        TabPanel3.Enabled = False
                        TabPanel4.Enabled = False
                        TabPanel5.Enabled = "false"
                        'TabPanel6.Enabled = "false"
                        TabPanel7.Enabled = False
                        TabPanel8.Enabled = "false"
                    Case 3
                        TabPanel1.Enabled = "false"
                        TabPanel2.Enabled = False
                        TabPanel4.Enabled = False
                        TabPanel5.Enabled = "false"
                        'TabPanel6.Enabled = "false"
                        TabPanel7.Enabled = False
                        TabPanel8.Enabled = "false"
                    Case 4
                        TabPanel1.Enabled = "false"
                        TabPanel2.Enabled = False
                        TabPanel3.Enabled = False
                        TabPanel5.Enabled = "false"
                        'TabPanel6.Enabled = "false"
                        TabPanel7.Enabled = False
                        TabPanel8.Enabled = "false"
                    Case 5
                        TabPanel1.Enabled = "false"
                        TabPanel2.Enabled = False
                        TabPanel3.Enabled = False
                        TabPanel4.Enabled = "false"
                        'TabPanel6.Enabled = "false"
                        TabPanel7.Enabled = False
                        TabPanel8.Enabled = "false"
                    Case 6
                        TabPanel1.Enabled = "false"
                        TabPanel2.Enabled = False
                        TabPanel3.Enabled = False
                        TabPanel4.Enabled = "false"
                        TabPanel5.Enabled = "false"
                        TabPanel7.Enabled = False
                        TabPanel8.Enabled = "false"
                    Case 7
                        TabPanel1.Enabled = False
                        TabPanel2.Enabled = False
                        TabPanel3.Enabled = False
                        TabPanel4.Enabled = "false"
                        TabPanel5.Enabled = "false"
                        'TabPanel6.Enabled = "false"
                        TabPanel8.Enabled = False
                    Case 8
                        TabPanel1.Enabled = "false"
                        TabPanel2.Enabled = False
                        TabPanel3.Enabled = False
                        TabPanel4.Enabled = "false"
                        TabPanel5.Enabled = "false"
                        'TabPanel6.Enabled = False
                        TabPanel7.Enabled = "false"
                End Select
                LaunchTab()
            Else
                Select Case lastState
                    Case "Linac Unauthorised"
                        TabPanel1.Enabled = "true"
                        TabPanel2.Enabled = "false"
                        TabPanel3.Enabled = "false"
                        TabPanel4.Enabled = "true"
                        TabPanel5.Enabled = "true"
                        'TabPanel6.Enabled = "true"
                        'added 9/10/17
                        If EquipmentID Like "LA_" Then
                            TabPanel7.Enabled = "true"
                            TabPanel7.HeaderText = EquipmentID + " Emergency Runup"
                        End If
                        TabPanel8.Enabled = "True"


                    Case "Engineering Approved", "Radiographer Approved"
                        TabPanel1.Enabled = "false"
                        TabPanel2.Enabled = "true"
                        TabPanel3.Enabled = "false"
                        TabPanel4.Enabled = "true"
                        TabPanel5.Enabled = "true"
                        'TabPanel6.Enabled = "true"
                        TabPanel7.Enabled = "false"
                        TabPanel8.Enabled = "True"

                        'TabPanel2.OnClientPopulated
                        'This is the set if must move on to pre-clinical
                        'TabPanel3.Enabled = "false"
                        'TabPanel4.Enabled = "false"
                        'TabPanel5.Enabled = "false"
                        'TabPanel6.Enabled = "false"

                        '31 Octobe change
                    Case "Clinical"
                        TabPanel1.Enabled = "false"
                        TabPanel2.Enabled = "false"
                        TabPanel3.Enabled = "true"
                        TabPanel7.Enabled = "false"
                        If Application(suspstate) = 1 Then
                            TabPanel4.Enabled = "true"
                            TabPanel5.Enabled = "true"
                            'TabPanel6.Enabled = "true"
                            TabPanel8.Enabled = "true"
                        Else
                            TabPanel4.Enabled = "false"
                            TabPanel5.Enabled = "false"
                            'TabPanel6.Enabled = "false"
                            TabPanel7.Enabled = "false"
                            TabPanel8.Enabled = "false"
                        End If

                    Case "Fault"
                        TabPanel1.Enabled = "false"
                        TabPanel2.Enabled = "false"
                        TabPanel3.Enabled = "false"
                        TabPanel4.Enabled = "false"
                        TabPanel5.Enabled = "true"
                        'TabPanel6.Enabled = "false"
                        TabPanel7.Enabled = "false"
                        TabPanel8.Enabled = "false"
                        'EndOfDay.Visible = False
                        If Not Request.QueryString("Tabindex") Is Nothing Then
                            tabIndex = Request.QueryString("Tabindex").ToString
                        End If
                        Select Case tabIndex
                            Case 1, 4, 5, 6, 7
                                LaunchTab()
                            Case 0
                                If Application(appstate) = 1 Then
                                    LaunchTab()
                                End If
                        End Select

                    Case "Suspended"
                        TabPanel1.Enabled = "false"
                        TabPanel2.Enabled = "false"
                        TabPanel3.Enabled = "true"
                        TabPanel4.Enabled = "true"
                        TabPanel5.Enabled = "true"
                        'TabPanel6.Enabled = "true"
                        TabPanel7.Enabled = "false"
                        TabPanel8.Enabled = "true"


                    Case Else

                End Select
                'Added this to set up modality view

            End If

        End If
        'SetModalities(ConnectionString)
    End Sub


    Protected Sub TabButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim TabString As String
        Dim tabbutton As Button = sender
        'Dim linacstatusuc As LinacStatusuc
        'Dim tabcontainer1 As TabContainer
        TabString = tabbutton.ID
        'Get Tab number
        Dim returnstring As String = EquipmentID + "page.aspx?tabclicked=" + TabString.Substring(9)
        'This is important. Session("ActiveTabIdx") is not updated when clicking on Tab 0. Used particularly for reporting fault
        If TabString.Substring(9) <> 0 Then
            Application(activetabstate) = tcl.ActiveTabIndex
            'Dim field1 As Integer = Application(activetabstate)
            'Session("ActiveTabIdx") = tcl.ActiveTabIndex
            'Dim field1 As Integer = CType(Session.Item("ActiveTabIdx"), Integer)
            'Label2.Text = field1
            Response.Redirect(returnstring)
        Else
            LaunchTab()
            SetModalities(connectionString)
        End If
    End Sub

    Public Sub LaunchTab()
        Dim Reload As Boolean = False
        'Dim EndOfDaycontrol As Button
        Dim linacstatusuc As LinacStatusuc
        Dim tabcontainer1 As TabContainer
        Dim reguser As RegisterUseruc
        Dim hidfield2 As HiddenField
        Dim tabActive As Integer
        Dim lastState As String = ""
        Dim lastuser As String = ""
        Dim lastusergroup As Integer = 0
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        Page = Me.Page
        mpContentPlaceHolder = CType(Page.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        If Not mpContentPlaceHolder Is Nothing Then
            tabcontainer1 = CType(mpContentPlaceHolder.FindControl("tcl"), TabContainer)
            If Not tabcontainer1 Is Nothing Then
                Dim panelcontrol As TabPanel = tabcontainer1.FindControl("TabPanel0")
                linacstatusuc = panelcontrol.FindControl("Linacstatusuc1")
                reguser = linacstatusuc.FindControl("RegisterUseruc1")
                Dim statusmultiview As MultiView = linacstatusuc.FindControl("Multiview1")
                statusmultiview.Visible = False
                Dim setView As View = statusmultiview.FindControl("View0")
                statusmultiview.SetActiveView(setView)
                Dim B1 As Button = linacstatusuc.FindControl("Button1")
                Dim B2 As Button = linacstatusuc.FindControl("Button2")
                Dim B3 As Button = linacstatusuc.FindControl("Button3")
                B1.Visible = True
                B2.Visible = True
                B3.Visible = True
                Dim hidfield1 As HiddenField = linacstatusuc.FindControl("HiddenField1")
                hidfield1.Value = False
                hidfield2 = linacstatusuc.FindControl("HiddenField2")
            End If

        End If


        'Moved not ispostback 12 April to fix problem where end of day button is not disabled
        'Action removed to end of day button as a result of SPR 30

        'Page = Me.Page
        'mpContentPlaceHolder = _
        'CType(Page.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        'If Not mpContentPlaceHolder Is Nothing Then
        '    tabcontainer1 = CType(mpContentPlaceHolder. _
        '        FindControl("tcl"), TabContainer)
        '    If Not tabcontainer1 Is Nothing Then
        '        Dim panelcontrol As TabPanel = tabcontainer1.FindControl("TabPanel0")
        '        EndOfDaycontrol = panelcontrol.FindControl("EndOfDay")
        '    End If
        '    'If Application(appstate) = 1 Then
        '    '    EndOfDaycontrol.Visible = False
        '    'Else
        '    '    EndOfDaycontrol.Visible = True
        '    'End If
        'End If


        If Not IsPostBack Then

            'DavesCode.Reuse.GetLastTech(EquipmentID, 1, lastState, lastuser, lastusergroup)
            ' find the sibling with the name that starts with TabContainer
            If Application(appstate) <> 1 Then
                'If Application("Someoneisloggedin") <> 1 Then
                'have to cater for when getting here from fault
                'DavesCode.Reuse.GetLastTech(EquipmentID, 0, lastState, lastuser, lastusergroup)
                'If lastState = "Fault" Then
                '    EndOfDaycontrol.Visible = False
                'Else
                '    EndOfDaycontrol.Visible = True
                'End If

                Reload = True
                'lastState = DavesCode.Reuse.GetLastState(EquipmentID, 0)
                'DavesCode.Reuse.GetLastTech(EquipmentID, 1, lastState, lastuser, lastusergroup)
            ElseIf Application(appstate) = 1 Then
                'this will be true if coming from these pages or if someone clicks a tab that isn't the one they are working on.
                'EndOfDaycontrol.Visible = False
                tabActive = tcl.ActiveTabIndex
                If Not Request.QueryString("pageref") Is Nothing Then
                    refpage = Request.QueryString("pageref").ToString
                    '    tabIndex = Request.QueryString("tabref").ToString
                    '    'tabIndex = Request.QueryString("Tabindex").ToString
                    '    comment = Request.QueryString("comment").ToString
                End If
                'changed to tabref
                If refpage = "Fault" Or refpage = "ViewFault" Or (Application(faultstate) <> False) Then
                    Reload = True
                ElseIf tabActive = 0 Then


                    Reload = False
                Else
                    If Not Request.QueryString("Reg") Is Nothing Then
                        Reg = Request.QueryString("Reg").ToString
                        '    tabIndex = Request.QueryString("tabref").ToString
                        '    'tabIndex = Request.QueryString("Tabindex").ToString
                        '    comment = Request.QueryString("comment").ToString
                    End If
                    If Application("RegistrationState") = True Then
                        hidfield2.Value = False
                        Reload = True
                    Else
                        'This should autorecover
                        WriteRecovery()
                    End If

                End If
            End If

            If Reload = True Then

                'Dim containerId As String = "TabContent" & tcl.ActiveTabIndex

                If Application(appstate) = 1 Then
                    tabActive = Application(activetabstate)
                    'tabActive = CType(Session.Item("ActiveTabIdx"), Integer)
                    tcl.ActiveTabIndex = tabActive
                Else
                    DavesCode.Reuse.GetLastTech(EquipmentID, 1, lastState, lastuser, lastusergroup)
                    If Not Request.QueryString("pageref") Is Nothing Then
                        refpage = Request.QueryString("pageref").ToString
                        '    tabIndex = Request.QueryString("tabref").ToString
                        '    'tabIndex = Request.QueryString("Tabindex").ToString
                        '    comment = Request.QueryString("comment").ToString
                        tabActive = 5
                        tcl.ActiveTabIndex = tabActive
                    Else
                        tabActive = tcl.ActiveTabIndex
                    End If

                End If
                Dim containerId As String = "TabContent" & tabActive
                Application(activetabstate) = tabActive
                Dim logcontrolId As String = "AcceptLinac" & tcl.ActiveTabIndex
                Dim panel As Panel = tcl.ActiveTab.FindControl(containerId)
                Dim modalpopupextendername As String = "modalpopupextendergen"
                modalpopupextendername = modalpopupextendername & tabActive

                Dim logcontrol As AcceptLinac = tcl.ActiveTab.FindControl(logcontrolId)
                Dim rucontrol As ErunupUserControl = tcl.ActiveTab.FindControl(runupcontrolId)
                Dim preccontrol As Preclinusercontrol = tcl.ActiveTab.FindControl(preclincontrolID)
                Dim clincontrol As ClinicalUserControl = tcl.ActiveTab.FindControl(ClinicalUserControlID)
                Dim plancontrol As Planned_Maintenanceuc = tcl.ActiveTab.FindControl(PlannedMaintenanceControlID)
                Dim repcontrol As Repairuc = tcl.ActiveTab.FindControl(repcontrolId)
                Dim writecontrol As WriteDatauc = tcl.ActiveTab.FindControl(writedatacontrolID)
                Dim physicscontrol As UserControl = tcl.ActiveTab.FindControl(physicscontrolID)
                Dim emergencycontrol As ErunupUserControl = tcl.ActiveTab.FindControl(emergencycontrolID)
                Dim trainingcontrol As Traininguc = tcl.ActiveTab.FindControl(trainingcontrolID)
                'Dim activepanel As Control = tcl.FindControl(tabpanelId)

                'moved panel.visible 030316
                If (Not panel Is Nothing) Then
                    panel.Visible = True
                    If (Not clincontrol Is Nothing) Then
                        clincontrol.Visible = False
                    End If

                    If Application(appstate) <> 1 Then
                        Dim failingstate As String = Application(failstate)
                        If (refpage = "Fault") Then
                            Select Case failingstate
                                Case 1, 4, 5, 6
                                    AcceptOKnosigpass(5, lastuser, lastusergroup)
                                Case Else
                                    If (Not logcontrol Is Nothing) Then
                                        Dim modalid As ModalPopupExtender = logcontrol.FindControl(modalpopupextendername)
                                        Dim textboxmodal As TextBox = logcontrol.FindControl("txtchkUserName")
                                        logcontrol.Visible = True
                                        'added 50416
                                        ForceFocus(textboxmodal)
                                        If (Not modalid Is Nothing) Then
                                            modalid.Show()

                                        End If
                                    End If
                            End Select

                        Else
                            'If recover = 1 Then

                            '    If (Not logcontrol Is Nothing) Then
                            '        Dim modalid As ModalPopupExtender = logcontrol.FindControl(modalpopupextendername)
                            '        Dim textboxmodal As TextBox = logcontrol.FindControl("txtchkUserName")
                            '        logcontrol.Visible = True
                            '        'added 50416
                            '        ForceFocus(textboxmodal)
                            '        If (Not modalid Is Nothing) Then
                            '            modalid.Show()

                            '        End If
                            '    End If
                            'Else
                            'Move to top 18/7/19
                            'Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
                            Select Case tabref

                                Case 1
                                    'Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
                                    AcceptOKnosigpass(tabref, lastuser, lastusergroup)
                                    rucontrol.EngLogOnEvent(connectionString)
                                Case 4, 5, 6, 8
                                    AcceptOKnosigpass(tabref, lastuser, lastusergroup)
                                Case 2
                                    AcceptOKnosigpass(tabref, lastuser, lastusergroup)
                                    preccontrol.Preclinloaded(connectionString)
                                Case 3


                                Case Else
                                    If (Not logcontrol Is Nothing) Then
                                        Dim modalid As ModalPopupExtender = logcontrol.FindControl(modalpopupextendername)
                                        Dim textboxmodal As TextBox = logcontrol.FindControl("txtchkUserName")
                                        logcontrol.Visible = True
                                        'added 50416
                                        ForceFocus(textboxmodal)
                                        If (Not modalid Is Nothing) Then
                                            modalid.Show()
                                        End If
                                    End If
                            End Select
                            'End If
                        End If
                    End If
                    'Dim textbox As TextBox

                    'instrumentation code
                    'Dim userlabel As Label
                    Dim Activity As String = ""
                    Dim User As String = ""
                    'Label2.Text = "Active Tab " & tabActive
                    'Modalities = Page.LoadControl("controls/ModalityDisplayuc.ascx")
                    'CType(Modalities, controls_ModalityDisplayuc).LinacName = EquipmentID
                    'CType(Modalities, controls_ModalityDisplayuc).ID = "ModalityDisplay"
                    Select Case tabActive
                        Case 0

                        Case 1
                            Activity = "Engineering Run Up"
                            DavesCode.Reuse.GetLastTech(EquipmentID, 0, lastState, lastuser, lastusergroup)
                            SetUser(lastusergroup)
                            'User = "Engineer/Physicist"
                            'SetModalities(connectionstring)
                            rucontrol.Visible = True


                            'textbox = rucontrol.FindControl("commentbox")
                            ''020316
                            ''If there is something in application state write it to box. There will be if text change is activated.
                            ''Have to check it exists
                            'If (Not HttpContext.Current.Application("EngBoxChanged") Is Nothing) Then
                            '    textbox.Text = Application("EngBoxChanged").ToString
                            'Else
                            '    textbox.Text = comment
                            'End If
                            'writecontrol.Visible = True
                            Dim panelcontrol As TabPanel = tcl.FindControl("TabPanel5")
                            If (Not panelcontrol Is Nothing) Then
                                'panelcontrol.Enabled = False
                                panelcontrol.Dispose()
                            End If
                            'added 8th
                            'TabPanel1.Enabled = "false"
                            'TabPanel2.Enabled = "false"
                            'TabPanel3.Enabled = "false"
                            'TabPanel4.Enabled = "false"
                            'TabPanel5.Enabled = "false"
                            'TabPanel6.Enabled = "false"
                            'TabPanel7.Enabled = "false"
                            'TabPanel8.Enabled = "false"
                        Case 2
                            Activity = "Pre-clinical Run Up"
                            UserGroupLabel.Text = "Radiographer"
                            preccontrol.Visible = True
                            'textbox = preccontrol.FindControl("commentbox")
                            'textbox.Text = comment
                            tcl.ActiveTabIndex = 2
                            UpdatePaneln.Update()
                        Case 3
                            Activity = ActivityLabel.Text
                            UserGroupLabel.Text = "Radiographer"
                            clincontrol.Visible = True
                            Dim output As String = "Clinical"
                            Dim clinicalcontrol As ClinicalUserControl = tcl.ActiveTab.FindControl(ClinicalUserControlID)
                            Dim outputn As String = Application(appstate)
                            If outputn = 1 Then
                                'should have a transaction
                                'Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
                                clinicalcontrol.ClinicalApprovedEvent(connectionString)
                                'CType(Modalities, controls_ModalityDisplayuc).Mode = "Unauthorised"
                            End If

                        Case 4
                            Activity = "Planned Maintenance"
                            DavesCode.Reuse.GetLastTech(EquipmentID, 0, lastState, lastuser, lastusergroup)
                            SetUser(lastusergroup)
                            'User = "Engineer/Physicist"
                            plancontrol.Visible = True
                            'textbox = plancontrol.FindControl("commentbox")
                            'textbox.Text = comment
                            'instrumentation code
                            'userlabel = plancontrol.FindControl("userlabel")
                            If Not Application(suspstate) = 1 Then
                                If Application(repairstate) = 1 Then
                                    Statelabel.Text = "Engineering Approved"
                                Else
                                    Statelabel.Text = "Linac Unauthorised"
                                End If
                            Else
                                Statelabel.Text = "Suspended"

                            End If
                        Case 5
                            Activity = "Repair"
                            DavesCode.Reuse.GetLastTech(EquipmentID, 0, lastState, lastuser, lastusergroup)
                            SetUser(lastusergroup)
                            'User = "Engineer/Physicist"
                            repcontrol.Visible = True
                            'textbox = repcontrol.FindControl("commentbox")
                            'textbox.Text = comment
                            If (refpage <> "Fault") Then
                                'Commented out 1 April no need to disable this button
                                'If lastusergroup <> 4 Then
                                '    Dim physicsbutton As Button
                                '    physicsbutton = repcontrol.FindControl("PhysicsQA")
                                '    physicsbutton.Enabled = False
                                'End If
                                If Not Application(suspstate) = 1 Then
                                    If Application(repairstate) = 1 Then
                                        'Statelabel.Text = "Engineering Approved"
                                    Else
                                        Statelabel.Text = "Linac Unauthorised"
                                    End If
                                Else
                                    Statelabel.Text = "Suspended"
                                End If
                            Else
                                'added GetlastTech because for a fault this is not called earlier so statelabel and set user are blank
                                DavesCode.Reuse.GetLastTech(EquipmentID, 0, lastState, lastuser, lastusergroup)
                                Statelabel.Text = lastState

                            End If
                        Case 6
                            'webcontrol.Visible = True
                            Activity = "Physics QA"
                            DavesCode.Reuse.GetLastTech(EquipmentID, 0, lastState, lastuser, lastusergroup)
                            SetUser(lastusergroup)
                            'User = "Physicist"
                            physicscontrol.Visible = True
                            'textbox = physicscontrol.FindControl("commentbox")
                            'textbox.Text = comment
                            If Not Application(suspstate) = 1 Then
                                If Application(repairstate) = 1 Then
                                    Statelabel.Text = "Engineering Approved"
                                Else
                                    Statelabel.Text = "Linac Unauthorised"
                                End If
                            Else
                                Statelabel.Text = "Suspended"
                            End If
                        Case 7
                            Activity = "Emergency Run Up"
                            UserGroupLabel.Text = "Radiographer"
                            'User = "Radiographer"
                            emergencycontrol.Visible = True
                            'textbox = emergencycontrol.FindControl("commentbox")
                            'textbox.Text = comment
                        Case 8
                            Activity = "Training/Development"
                            DavesCode.Reuse.GetLastTech(EquipmentID, 0, lastState, lastuser, lastusergroup)
                            SetUser(lastusergroup)
                            'User = "Staff"
                            trainingcontrol.Visible = True

                            'DavesCode.Reuse.GetLastTech(EquipmentID, 1, lastState, lastuser, lastusergroup)
                            Update_ReturnButtons()
                            'Enable button for all groups otherwise have to do other checks elsewhere and not worth it as only phys can save changes.
                            'If (lastusergroup = 3) Then
                            '    Dim physicsbutton As Button
                            '    physicsbutton = trainingcontrol.FindControl("PhysicsQA")
                            '    physicsbutton.Enabled = False
                            'End If
                            If Not Application(suspstate) = 1 Then
                                If Application(repairstate) = 1 Then
                                    Statelabel.Text = "Engineering Approved"
                                Else
                                    Statelabel.Text = "Linac Unauthorised"
                                End If
                            Else
                                Statelabel.Text = "Suspended"
                            End If
                    End Select
                    'CType(Modalities, controls_ModalityDisplayuc).Mode = Statelabel.Text
                    'ModalityPlaceholder.Controls.Add(Modalities)
                    'ModalityDisplayPanel.Visible = True
                    ActivityLabel.Text = Activity
                    Select Case tabActive
                        Case 4, 5, 6, 8
                            SetUser(lastusergroup)
                    End Select
                    Application(activetabstate) = tcl.ActiveTabIndex
                    'Session("ActiveTabIdx") = tcl.ActiveTabIndex
                    'Dim field1 As Integer = Application(activetabstate)
                    'Dim field1 As Integer = CType(Session.Item("ActiveTabIdx"), Integer)
                    'DavesCode.Reuse.ReturnApplicationState("Tabload")
                    If recover = 1 Then
                        recoverbuttonscript()
                    End If
                    Application("RegistrationState") = False
                End If
            Else

                tabActive = tcl.ActiveTabIndex
                Dim containerId As String = "TabContent" & tabActive
                Application(activetabstate) = tabActive

            End If

        End If

    End Sub
    Public Sub AcceptOKnosigpass(ByVal Task As Integer, ByVal user As String, ByVal usergroup As Integer)
        Dim output As String
        Dim strScript As String = "<script>"
        Dim Activity As String
        Dim laststate As String
        Activity = DavesCode.Reuse.ReturnActivity(Task)
        'Don't want to write status if already on tab 5
        laststate = DavesCode.Reuse.GetLastState(EquipmentID, 0)
        If laststate = "Fault" And Application(failstate) = 5 Then
            DavesCode.Reuse.MachineState(user, usergroup, EquipmentID, Task, False)
        Else
            DavesCode.Reuse.MachineState(user, usergroup, EquipmentID, Task, False)
        End If
        Application(appstate) = 1
        output = Application(appstate)
        'eg from http://dotnetbites.wordpress.com/2014/02/15/call-parent-page-method-from-user-control-using-reflection/
        ' this is an instrumentation field that displays application number ie 0 or 1
        'Me.Page.GetType.InvokeMember("UpdateHiddenLAField", System.Reflection.BindingFlags.InvokeMethod, Nothing, Me.Page, New Object() {output})
        Me.Page.GetType.InvokeMember("UpdateDisplay", System.Reflection.BindingFlags.InvokeMethod, Nothing, Me.Page, New Object() {Activity})
        'Addded 31 March 2016
        SetUser(usergroup)
        If Task = 3 Then
            output = "Clinical"
            Me.Page.GetType.InvokeMember("Updatestatedisplay", System.Reflection.BindingFlags.InvokeMethod, Nothing, Me.Page, New Object() {output})
            RaiseEvent NoApprove()
        End If
        output = connectionString
        Me.Page.GetType.InvokeMember("SetModalities", System.Reflection.BindingFlags.InvokeMethod, Nothing, Me.Page, New Object() {output})
    End Sub

    Public Event MyEventB1 As System.EventHandler


    'Protected Sub ReportFault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ReportFault.Click
    '    'This takes account of if fault is reported while on status tab but logged in to another tab
    '    'If already logged in but on tab 0 then don't change session
    '    'Otherwise make sure session is uptodate?
    '    Dim Tabindex As String
    '    If tcl.ActiveTabIndex = 0 And Application(appstate) = 1 Then
    '        'user is already logged on and has clicked on tab 0 then reported a fault. Want to send live tab to fault page.
    '        'Tabindex = CType(Session.Item("ActiveTabIdx"), String)
    '        Tabindex = Application(activetabstate)
    '    Else
    '        Session("ActiveTabIdx") = tcl.ActiveTabIndex
    '        Tabindex = CType(Session.Item("ActiveTabIdx"), String)
    '    End If


    '    'Dim tabActive As Integer
    '    'tabActive = tcl.ActiveTabIndex
    '    'New stuff
    '    Dim UserControlid As String
    '    Dim comment As TextBox
    '    Dim textcomment As String
    '    Dim containerID As String = "TabContent" & Tabindex
    '    'if suspended there will be no active tab
    '    Dim sus As String = Application(suspstate)

    '    Select Case Tabindex
    '        Case 0, 3 'This takes care of a fault reported when system suspended although this shouldn't happen
    '            'only 0 if no one logged in or if suspended
    '            UserControlid = ""
    '            textcomment = Nothing
    '        Case 1
    '            UserControlid = "ErunupUserControl1"
    '        Case 2
    '            UserControlid = "Preclinusercontrol1"
    '            'Case 3
    '            'Don't need comment from clinicalusercontrol because it is saved
    '            'UserControlid = "ClinicalUserControl"

    '        Case 4
    '            UserControlid = "PlannedMaintenanceuc1"
    '        Case 5
    '            UserControlid = "Repairuc1"
    '        Case 6
    '            UserControlid = "PhysicsQAuc1"
    '        Case 7
    '            UserControlid = "Emergencyrunupuc1"
    '        Case 8
    '            UserControlid = "Traininguc1"
    '        Case Else
    '            'if it gets to here with no active tab then autorecover
    '            WriteRecovery()
    '    End Select
    '    'Why does it find panel?
    '    Dim panel As Panel = tcl.ActiveTab.FindControl(containerID)
    '    'Why is it called clincontrol?


    '    Dim clincontrol As UserControl = tcl.ActiveTab.FindControl(UserControlid)
    '    If Not clincontrol Is Nothing Then
    '        comment = CType(clincontrol.FindControl("CommentBox"), TextBox)
    '        If Not comment Is Nothing Then
    '            textcomment = comment.Text
    '        End If
    '    Else
    '        'Labelcomment.Text = "Cannot find comment box."
    '    End If
    '    'Response.Redirect("faultPage.aspx?val=B1&Tabindex=" & Tabindex & "&commentbox=" & textcomment)
    '    Dim returnstring As String = "faultPage.aspx?val=" + EquipmentID + "&Tabindex=" + Tabindex + "&commentbox=" & textcomment
    '    Response.Redirect(returnstring)

    'End Sub

    Protected Sub tcl_ActiveTabChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tcl.ActiveTabChanged
        'This doesn't do anything. just playing with the dynamiccontextkey
        Select Case (tcl.ActiveTab.DynamicContextKey)

            Case "Engrunup"


        End Select
        'From http://geekswithblogs.net/frankw/archive/2008/10/29/enable-back-button-support-in-asp.net-ajax-web-sites.aspx
        'ActiveTabIndex = tcl.ActiveTabIndex 'Update the ActiveTabIndex property
        'Dim scripmanager As ToolkitScriptManager
        'scripmanager = Page.Master.FindControl("ToolkitScriptManager1")
        'scripmanager.AddHistoryPoint("ActiveTabIndex", ActiveTabIndex.ToString())
    End Sub

    Protected Sub Navigate(ByVal sender As Object, ByVal e As HistoryEventArgs)
        tcl.ActiveTabIndex = Convert.ToInt32(e.State("ActiveTabIndex"))
    End Sub



    'Protected Sub RegisterUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RegisterUser.Click
    '    'http://stackoverflow.com/questions/17582081/how-to-open-aspx-web-pages-on-a-pop-up-window
    '    PopUpWindow("RegisterUser.aspx", "Register")

    '    'Session("ActiveTabIdx") = tcl.ActiveTabIndex
    '    'Dim Tabindex As String = CType(Session.Item("ActiveTabIdx"), String)
    '    'Response.Redirect("RegisterUser.aspx?val=B1&Tabindex=" & Tabindex)
    'End Sub

    'Protected Sub Admin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Admin.Click

    '    Dim returnstring As String = "Administration.aspx?val=" + EquipmentID
    '    PopUpWindow(returnstring, "Admin")

    '    'ChangePassword 23rdfeb What's this about?
    '    'Dim returnstring As String = "Administration.aspx?val=" + EquipmentID
    '    'Response.Redirect(returnstring)

    'End Sub

    'Protected Sub ViewFaultButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewFaultButton.Click
    '    'http://stackoverflow.com/questions/17582081/how-to-open-aspx-web-pages-on-a-pop-up-window
    '    Dim returnstring As String = "ViewFaults.aspx?id=" + EquipmentID
    '    PopUpWindow(returnstring, "Fault")

    'End Sub
    Protected Sub AcknowledgeEnergies()
        PopUpWindow("AcknowledgeEnergies.aspx", "Energies")
    End Sub
    Protected Sub PopUpWindow(ByVal Poppage As String, ByVal PageType As String)
        'http://www.aspsnippets.com/Articles/Open-New-Window-from-Server-Side-Code-Behind-in-ASPNet-using-C-and-VBNet.aspx
        Dim url As String = Poppage
        Dim PageName As String = PageType
        PageName = "popup_window_" & PageName
        Dim DiagResult As Integer
        'DiagResult = Integer.Parse(inpHide.Value)
        Dim path As String = HttpContext.Current.Request.Url.AbsolutePath
        'from http://www.codestore.net/store.nsf/unid/DOMM-4PYJ3S?OpenDocument
        'Dim s As String = "window.open('" & url + "', 'PageName', 'width=800,height=700,left=100,top=100,resizable=yes, scrollbars=yes');"
        Dim s As String = "windowOpener('" & url + "', 'PageName', 'width=800,height=700,left=100,top=100,resizable=yes, scrollbars=yes');"
        'Dim s As String = "windowOpener('" & url + "', 'PageName', 'fullscreen=yes');"

        ClientScript.RegisterStartupScript(path.GetType(), PageName, s, True)

    End Sub

    Protected Sub EndOfDay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EndOfDay.Click

        'Amended because a user could click button before it was hidden SPR 30

        EndOfDay.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(EndOfDay, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
        'EndofDayElf("EndDay")

        Dim mpContentPlaceHolder As ContentPlaceHolder
        mpContentPlaceHolder = CType(Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        If Not mpContentPlaceHolder Is Nothing Then
            Dim lastState As String
            lastState = DavesCode.Reuse.GetLastState(EquipmentID, 0)
            If (Application(appstate) = 1) Or (lastState = "Fault") Then
                'tell user it can't be done
                Dim strScript As String = "<script>"
                If Application(appstate) = 1 Then
                    strScript += "alert('Please complete current action first');"
                Else
                    strScript += "alert('Please Clear fault first');"
                End If
                strScript += "</script>"
                ScriptManager.RegisterStartupScript(EndOfDay, Me.GetType(), "JSCR", strScript.ToString(), False)
            Else
                wctrl = CType(mpContentPlaceHolder.FindControl("Writedatauc1"), WriteDatauc)
                Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
                wcbutton.Text = "End Of Day"
                Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
                Application(actionstate) = "Confirm"
                wctrl.Visible = True
                ForceFocus(wctext)
            End If
        End If

    End Sub

    ' this is an instrumentation field that displays application number ie 0 or 1
    'Public Sub UpdateHiddenLAField(ByVal message As String)
    '    LAHiddenFieldcontrol.Value = message
    '    Application1.Text = message
    '    'LAFieldcontrol.Text = message
    'End Sub
    Public Sub UpdateUserDisplay(ByVal message As Integer)
        SetUser(message)
    End Sub
    Public Sub UpdateDisplay(ByVal message As String)
        ActivityLabel.Text = message
    End Sub
    Public Sub UpdateButtons()
        Update_ReturnButtons()
    End Sub
    'instrumentation code
    'Public Sub Updateuserlabel(ByVal message As String)
    '    UserLabel.Text = message
    'End Sub
    Public Sub Updatestatedisplay(ByVal message As String)
        Statelabel.Text = message
    End Sub

    Public Shared Sub CloseMessage()
        'DavesCode.Reuse.CloseBrowser()
        'Need to put in a check here to see if is a fault because hidden field not refreshed in writeauc
        Try
            Dim lastState As String
            lastState = DavesCode.Reuse.GetLastState("B1", 0)
            Select Case lastState
                Case "Repair", "Fault"
                    DavesCode.Reuse.SetStatus("No User", "Repair", 5, 7, "B1", 0)
                Case Else
                    DavesCode.Reuse.SetStatus("No User", "Linac Unauthorised", 5, 7, "B1", 0)
            End Select
        Finally


        End Try

    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As System.EventArgs) Handles Timer1.Tick
        'modified to handle browser being closed without end of day or equivalent 16/11/17
        Dim HoursSinceMidnight As Double = Date.Now.Subtract(Date.Today).TotalHours
        'This falls over if + is used and is only instrumentation code
        'Label1.Text = "Hours since midnight " + HoursSinceMidnight
        If HoursSinceMidnight < 3 Then
            EndofDayElf("Timer")
        End If
    End Sub

        Protected Sub EndofDayElf(ByVal Caller As String)
        Dim returnstring As String = EquipmentID + "page.aspx"
        Dim mrucontrol As ErunupUserControl
        Dim mpreccontrol As Preclinusercontrol
        Dim mclincontrol As ClinicalUserControl
        Dim mplancontrol As Planned_Maintenanceuc
        Dim mrepcontrol As Repairuc
        'Dim mwebcontrol As UserControl = tcl.ActiveTab.FindControl(webusercontrol21ID)
        'Dim mwritecontrol As UserControl = tcl.ActiveTab.FindControl(writedatacontrolID)
        Dim mphysicscontrol As UserControl
        Dim mtrainingcontrol As Traininguc
        Dim grdview As GridView
        Dim Commentbox As TextBox
        Dim Comment As String
        Dim Logoffuser As String = "System"
        Dim Breakdown As Boolean
        Dim lastState As String
        Dim activetab As String
        Dim suspendnull As String = Nothing
        Dim repairstatenull As String = Nothing
        Dim NumOpen As Integer
        Dim conn As SqlConnection
        Dim comm As SqlCommand
        Dim reader As SqlDataReader
        Dim connectionString1 As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        Dim Successful As Boolean = False

        lastState = DavesCode.Reuse.GetLastState(EquipmentID, 0)
            conn = New SqlConnection(connectionString1)
            comm = New SqlCommand("select Count(*) as Numopen from FaultIDTable where Status in ('New','Open') and linac=@linac", conn)
            comm.Parameters.AddWithValue("@linac", EquipmentID)

            conn.Open()
            reader = comm.ExecuteReader()
            If reader.Read() Then
                NumOpen = reader.Item("NumOpen")
                If NumOpen <> 0 Then
                    Breakdown = True
                Else
                    Breakdown = False
                End If
            End If

        'Label2.Text = "Last state " + lastState

        If Application(appstate) = 1 Then
            'this forces active tab to be actual active tab. This isn't the case if the active tab is tab 0 so find controls fails.
            'Dim tabActive As String
            activetab = Application(activetabstate)
            'tabActive = CType(Session.Item("ActiveTabIdx"), Integer)
            'tcl.ActiveTabIndex = activetab
            'This is superfluous
            'If Not Breakdown Then and was also breaking application states
            '    suspstate = Nothing
            '    repairstate = Nothing
            'End If
            'Label3.Text = "Tab is " + activetab
            Application(actionstate) = False
            Select Case activetab

                Case 1, 7
                    If activetab = 1 Then
                        mrucontrol = tcl.ActiveTab.FindControl(runupcontrolId)
                    Else
                        mrucontrol = tcl.ActiveTab.FindControl(emergencycontrolID)
                    End If
                    If Not mrucontrol Is Nothing Then
                        grdview = mrucontrol.FindControl("Gridview1")
                        'Commentbox = mrucontrol.FindControl("CommentBox")
                        'Comment = Commentbox.Text
                        'blank grid view 17/11/17
                        'RaiseEvent DayEnded(activetab, Logoffuser)
                        'all of these commits removed because each control is called up directly
                        'At the moment if it fails it 
                        'Successful = DavesCode.NewEngRunup.CommitRunup(grdview, EquipmentID, 666, Logoffuser, Comment, False, Breakdown, False)
                        mrucontrol.UserApprovedEvent(activetab, Logoffuser)
                    Else

                    End If
                Case 2

                    mpreccontrol = tcl.ActiveTab.FindControl(preclincontrolID)
                    'Commentbox = mpreccontrol.FindControl("CommentBox")
                    'Comment = Commentbox.Text
                    'DavesCode.Reuse.CommitPreClin(EquipmentID, Logoffuser, Comment, False, False, False, Breakdown)
                    If Not mpreccontrol Is Nothing Then
                        mpreccontrol.UserApprovedEvent(activetab, Logoffuser)
                    Else
                    End If
                Case 3
                    mclincontrol = tcl.ActiveTab.FindControl(ClinicalUserControlID)
                    If Not mclincontrol Is Nothing Then
                        mclincontrol.UserApprovedEvent(activetab, Logoffuser)
                    Else
                    End If

                    'DavesCode.NewCommitClinical.CommitClinical(EquipmentID, Logoffuser, Breakdown)
                        'Next line not used because commitclinical modified to remove two step process of suspended then log off
                        'DavesCode.Reuse.SetStatus(Logoffuser, "Linac Unauthorised", 5, 102, EquipmentID, 0)
                Case 4
                    mplancontrol = tcl.ActiveTab.FindControl(PlannedMaintenanceControlID)
                    'Commentbox = mplancontrol.FindControl("CommentBox")
                    'Comment = Commentbox.Text
                    If Not mplancontrol Is Nothing Then
                        Application(actionstate) = "EndOfDay"
                        mplancontrol.UserApprovedEvent(activetab, Logoffuser)
                    Else
                    End If
                    'DavesCode.Reuse.WriteAuxTables(EquipmentID, Logoffuser, Comment, 102, 4, Breakdown, suspendnull, repairstatenull, False)

                Case 5
                    mrepcontrol = tcl.ActiveTab.FindControl(repcontrolId)
                    'Commentbox = mrepcontrol.FindControl("CommentBox")
                    'Comment = Commentbox.Text
                    'mrepcontrol.RemoteLockElf()
                    If Not mrepcontrol Is Nothing Then
                        If Breakdown Then
                            'This means there are still open faults
                            If Caller = "EndDay" Then
                                WriteRecovery()
                            Else
                                mrepcontrol.RemoteLockElf(False)
                            End If
                        Else
                            If lastState = "Fault" Then
                                'This means there were open faults but they have been closed so need to close them off.
                                mrepcontrol.WriteFaultIDTable()
                            End If
                            'DavesCode.Reuse.WriteAuxTables(EquipmentID, Logoffuser, Comment, 102, 5, Breakdown, suspendnull, repairstatenull, False)
                            Application(actionstate) = "EndOfDay"
                            mrepcontrol.UserApprovedEvent(activetab, Logoffuser)
                        End If
                    Else
                    End If

                Case 6
                    mphysicscontrol = tcl.ActiveTab.FindControl(physicscontrolID)
                    Commentbox = mphysicscontrol.FindControl("CommentBox")
                    Comment = Commentbox.Text
                    DavesCode.Reuse.WriteAuxTables(EquipmentID, Logoffuser, Comment, 102, 6, Breakdown, suspendnull, repairstatenull, False)
                Case 8
                    mtrainingcontrol = tcl.ActiveTab.FindControl(trainingcontrolID)
                    'Commentbox = mtrainingcontrol.FindControl("CommentBox")
                    'Comment = Commentbox.Text
                    If Not mtrainingcontrol Is Nothing Then
                        Application(actionstate) = "EndOfDay"
                        mtrainingcontrol.userapprovedevent(activetab, Logoffuser)
                        'DavesCode.Reuse.WriteAuxTables(EquipmentID, Logoffuser, Comment, 102, 8, Breakdown, suspendnull, repairstatenull, False)
                    Else
                    End If
            End Select
        Else
            If Breakdown = False Then
                'this is to make sure that equivalent of end of day happens
                'Only want this to happen if repairstate or suspended but no one is logged on.
                If Application(suspstate) = 1 Or Application(repairstate) = 1 Then
                    DavesCode.Reuse.SetStatus(Logoffuser, "Linac Unauthorised", 5, 102, EquipmentID, 10)
                End If
            End If
        End If
        If Not Breakdown Then
                Application(suspstate) = Nothing
                Application(appstate) = Nothing
                Application(failstate) = Nothing
                Application(clinicalstate) = Nothing
                Application(repairstate) = Nothing
                Application(treatmentstate) = "Yes"
                Application(activetabstate) = Nothing
                Response.Redirect(returnstring)
            End If
            'This is in the wrong place because it redirects even if there is a fault and this confuses the system
            'Response.Redirect(returnstring)

    End Sub

    Protected Sub RestoreButton_Click(sender As Object, e As System.EventArgs) Handles RestoreButton.Click

        WriteRecovery()

    End Sub

    'From http://www.pberblog.com/blog/set-focus-to-a-control-of-a-modalpopupextender-programmatically/
    Private Sub ForceFocus(ByVal ctrl As Control)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "FocusScript", "setTimeout(function(){$get('" + _
        ctrl.ClientID + "').focus();}, 100);", True)
    End Sub

    Private Sub recoverbuttonscript()
        Dim strScript As String = "<script>"
        strScript += "alert('Elf has been Restored');"
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(RestoreButton, Me.GetType(), "JSCR", strScript.ToString(), False)
    End Sub
    Sub WriteRecovery()
        Application(actionstate) = False
        Dim activetab As String
        Dim susstate As String = Nothing
        Dim repstate As String = Nothing
        'Dim Userinfo As String = "Restored"
        Dim Logoffuser As String = "Restored"
        Dim reader As SqlDataReader
        Dim Status As String = ""
        Dim Activity As Integer
        Dim Radio As String = "101"
        Dim conn As SqlConnection
        Dim conActivity As SqlCommand
        Dim connectionString As String = ConfigurationManager.ConnectionStrings(
        "connectionstring").ConnectionString
        Dim mpContentPlaceHolder As ContentPlaceHolder
        Dim grdview As GridView
        Dim breakdown As Boolean = False
        'Dim returnstring As String
        Dim MachineLabel As String = EquipmentID & "Page.aspx';"
        Dim strScript As String = "<script>"
        Dim Valid As Boolean = False
        Dim Comment As String = "Recovered"
        strScript += "window.location='"
        strScript += EquipmentID
        strScript += "</script>"
        Dim returnstring As String
        Dim mrucontrol As ErunupUserControl
        Dim mpreccontrol As Preclinusercontrol
        Dim mclincontrol As ClinicalUserControl
        Dim mplancontrol As Planned_Maintenanceuc
        Dim mrepcontrol As Repairuc
        Dim mphysicscontrol As UserControl
        Dim mtrainingcontrol As Traininguc
        activetab = Application(activetabstate)

        breakdown = DavesCode.Reuse.CheckForOpenFault(EquipmentID)
        mpContentPlaceHolder = CType(FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        If Not mpContentPlaceHolder Is Nothing Then
            grdview = CType(mpContentPlaceHolder.FindControl("DummyGridview"), GridView)
        End If
        If Not breakdown Then
            conn = New SqlConnection(connectionString)

            conActivity = New SqlCommand("SELECT state, userreason FROM [LinacStatus] where stateID = (Select max(stateID) as lastrecord from [LinacStatus] where linac=@linac)", conn)

            conActivity.Parameters.AddWithValue("@linac", EquipmentID)
            conn.Open()
            reader = conActivity.ExecuteReader()

            If reader.Read() Then
                Status = reader.Item("State")
                Activity = reader.Item("userreason")
            End If
            reader.Close()
            conn.Close()
        Else
            Status = "Fault"
            Activity = 5
            activetab = 5
        End If




        Select Case activetab
            'Case 7
            '        DavesCode.Reuse.SetStatus(Userinfo, "Linac Unauthorised", 5, 7, MachineName, 0)
            Case 1, 7
                'only need dummy gridview when passing to commit run up not when using runup control
                'tab 666 is for commit run up - same as for fault condition
                If activetab = 1 Then
                    mrucontrol = tcl.ActiveTab.FindControl(runupcontrolId)
                Else
                    mrucontrol = tcl.ActiveTab.FindControl(emergencycontrolID)
                End If

                'grdview = mrucontrol.FindControl("Gridview1")
                'Commentbox = mrucontrol.FindControl("CommentBox")
                'Comment = Commentbox.Text
                'blank grid view 17/11/17
                'RaiseEvent DayEnded(activetab, Logoffuser)
                'Successful = DavesCode.NewEngRunup.CommitRunup(grdview, EquipmentID, 666, Logoffuser, Comment, False, Breakdown, False)
                mrucontrol.UserApprovedEvent("666", Logoffuser)
                'DavesCode.NewEngRunup.CommitRunupNew(grdview, EquipmentID, 666, Userinfo, Comment, Valid, False, False) ' 666 means that blank gridview is written
                'Application(repairstate) = Nothing

            Case 2
                mpreccontrol = tcl.ActiveTab.FindControl(preclincontrolID)
                mpreccontrol.UserApprovedEvent(activetab, Logoffuser)
                Application(repairstate) = 1
            Case 3
                mclincontrol = tcl.ActiveTab.FindControl(ClinicalUserControlID)
                mclincontrol.UserApprovedEvent("Recover", Logoffuser)
                'DavesCode.Reuse.CommitClinical(EquipmentID, Userinfo, breakdown)
                Application(treatmentstate) = "Yes"
            Case 4, 5, 6, 8
                If (Not HttpContext.Current.Application(suspstate) Is Nothing) Then
                    susstate = HttpContext.Current.Application(suspstate).ToString
                    '        DavesCode.Reuse.WriteAuxTables(MachineName, Userinfo, comment, -1, Activity, breakdown, suspstate, repstate)
                Else
                End If
                If (Not HttpContext.Current.Application(repairstate) Is Nothing) Then
                    repstate = HttpContext.Current.Application(repairstate).ToString

                Else
                End If

                Select Case activetab
                    Case 4
                        If (Not tcl.ActiveTab.FindControl(PlannedMaintenanceControlID) Is Nothing) Then
                            mplancontrol = tcl.ActiveTab.FindControl(PlannedMaintenanceControlID)
                            mplancontrol.UserApprovedEvent(activetab, Logoffuser)
                        Else
                        End If
                    Case 5
                        If (Not tcl.ActiveTab.FindControl(repcontrolId) Is Nothing) Then
                            mrepcontrol = tcl.ActiveTab.FindControl(repcontrolId)
                            mrepcontrol.UserApprovedEvent(activetab, Logoffuser)
                        Else
                        End If

                    Case 8
                        If (Not tcl.ActiveTab.FindControl(trainingcontrolID) Is Nothing) Then
                            mtrainingcontrol = tcl.ActiveTab.FindControl(trainingcontrolID)
                            mtrainingcontrol.UserApprovedEvent(activetab, Logoffuser)
                        Else
                        End If

                End Select
                ' DavesCode.Reuse.WriteAuxTables(EquipmentID, Userinfo, Comment, Radio, Activity, breakdown, susstate, repstate, False)
            Case Else
                'This caters for when the system is already idling as it were.
                returnstring = EquipmentID + "page.aspx"
                Application(appstate) = Nothing
                Response.Redirect(returnstring)
        End Select
        returnstring = EquipmentID + "page.aspx?tabref=" + Convert.ToString(Activity) + "&recovered=1"
        Application(technicalstate) = Nothing
        Application(appstate) = Nothing

        Response.Redirect(returnstring)

    End Sub

End Class

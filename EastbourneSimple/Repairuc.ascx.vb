Imports System.Data.SqlClient
Imports AjaxControlToolkit
Partial Class Repairuc
    Inherits System.Web.UI.UserControl

    Private MachineName As String
    Private SelectCount As Boolean
    Private Radioselect As Integer
    Public LinName As String
    Private appstate As String
    Private suspstate As String
    Private actionstate As String
    Private failstate As String
    Private repairstate As String
    Private faultviewstate As String
    Private atlasviewstate As String
    Private qaviewstate As String
    Private faultstate As String
    Private objconToday As TodayClosedFault
    Private Todaydefect As DefectSave
    Private Todaydefectpark As DefectSavePark
    Private laststate As String
    Private lastuser As String
    Private lastusergroup As String
    Private isFault As Boolean
    Private BoxChanged As String
    Public Event BlankGroup(ByVal BlankUser As Integer)
    Private Event AutoApproved(ByVal Tab As String, ByVal UserName As String)
    Private tabstate As String
    Private Objcon As ViewOpenFaults

    Public Property LinacName() As String
        Get
            Return MachineName
        End Get
        Set(ByVal value As String)
            MachineName = value
            LinName = value
        End Set
    End Property
    Public Sub UpdateReturnButtonsHandler()
        'Now find which user group is logged on
        'disabled to test removal of physics QA button 31 march 2016
        If Application(faultstate) <> True Then
            DavesCode.Reuse.GetLastTech(MachineName, 0, laststate, lastuser, lastusergroup)
            '    If (lastusergroup = 4) Then
            '        RadioButtonList1.Items.FindByValue(6).Enabled = True
            '    Else
            '        RadioButtonList1.Items.FindByValue(6).Enabled = False
            '    End If
            StateTextBox.Text = laststate
        End If
    End Sub
    Protected Sub Update_Today(ByVal EquipmentID As String, ByVal incidentID As String)
        If MachineName = EquipmentID Then
            Dim todayfault As TodayClosedFault = PlaceHolder5.FindControl("Todaysfaults")
            todayfault.SetGrid()
            If MachineName Like "T?" Then
                Todaydefectpark = PlaceHolder1.FindControl("DefectDisplay")
                Todaydefectpark.ResetDefectDropDown(incidentID)
            Else
                Todaydefect = PlaceHolder1.FindControl("DefectDisplay")
                Todaydefect.ResetDefectDropDown(incidentID)
            End If

        End If
    End Sub

    Protected Sub Update_Defect(ByVal EquipmentID As String)
        If MachineName = EquipmentID Then
            If MachineName Like "T?" Then
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
        faultstate = "OpenFault" + MachineName
        BoxChanged = "RepBoxChanged" + MachineName
        tabstate = "ActTab" + MachineName
    End Sub
    Protected Sub UserApprovedEvent(ByVal Tabused As String, ByVal Userinfo As String)
        Dim username As String = Userinfo
        Dim machinelabel As String = MachineName & "Page.aspx';"
        Dim LinacStatusID As String = ""
        Dim LinacStateID As String = ""
        Dim Breakdown = False
        Dim suspendvalue As String
        Dim repairvalue As String

        If Tabused = "5" Then
            Dim Action As String = Application(actionstate)
            If Action = "Confirm" Then
                'DavesCode.Reuse.ReturnApplicationState(Tabused)
                Dim Textboxcomment As TextBox = FindControl("CommentBox")
                Dim comment As String = Textboxcomment.Text
                Dim strScript As String = "<script>"
                strScript += "window.location='"
                strScript += machinelabel
                strScript += "</script>"
                'This could probably be tidied up if clinical not 3,3
                suspendvalue = Application(suspstate)
                repairvalue = Application(repairstate)
                Radioselect = RadioButtonList1.SelectedItem.Value
                'If this fails it writes an error to file but carries on.
                DavesCode.NewWriteAux.WriteAuxTables(MachineName, username, comment, Radioselect, Tabused, False, suspendvalue, repairvalue, False)
                'DavesCode.Reuse.Writerep(MachineName, username, comment, LinacStatusID)
                Application(appstate) = Nothing
                Application(tabstate) = String.Empty
                ' this is an instrumentation field that displays application number ie 0 or 1
                'Dim output As String = Application(appstate)
                'Me.Page.GetType.InvokeMember("UpdateHiddenLAField", System.Reflection.BindingFlags.InvokeMethod, Nothing, Me.Page, New Object() {output})
                SelectCount = False
                HttpContext.Current.Application(BoxChanged) = Nothing
                Select Case Radioselect

                    Case 1
                        'LinacStateID = DavesCode.Reuse.SetStatus(username, "Linac Unauthorised", 5, 7, MachineName, 5)
                        Application(failstate) = Nothing
                        Application(repairstate) = Nothing
                        Application(suspstate) = Nothing
                        Dim returnstring As String = MachineName + "page.aspx?tabref=" + Convert.ToString(Radioselect)
                        'DavesCode.Reuse.ReturnApplicationState(Tabused)
                        Response.Redirect(returnstring)
                    Case 2
                        ' LinacStateID = DavesCode.Reuse.SetStatus(username, "Engineering Approved", 5, 7, MachineName, 5)
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
                        'LinacStateID = DavesCode.Reuse.SetStatus(username, "Clinical", 5, 7, MachineName, 5)
                    Case 4
                        Application(failstate) = Nothing
                        'LinacStateID = DavesCode.Reuse.SetStatus(username, "Planned Maintenance", 5, 7, MachineName, 5)
                        'ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)
                        Dim returnstring As String = MachineName + "page.aspx?tabref=" + Convert.ToString(Radioselect)
                        'DavesCode.Reuse.ReturnApplicationState(Tabused)
                        Response.Redirect(returnstring)
                    Case 6
                        Application(failstate) = Nothing
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
                        Application(failstate) = Nothing
                        Dim returnstring As String = MachineName + "page.aspx?tabref=" + Convert.ToString(Radioselect)
                        'DavesCode.Reuse.ReturnApplicationState(Tabused)
                        Response.Redirect(returnstring)
                End Select

                RadioButtonList1.SelectedIndex = -1
                LogOffButton.BackColor = Drawing.Color.AntiqueWhite
            Else
                'I don't think it should get here
                Application(appstate) = Nothing
                Dim strScript As String = "<script>"
                strScript += "alert('What? Logging Off');"
                strScript += "window.location='"
                strScript += machinelabel
                strScript += "</script>"
                ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)
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


        DavesCode.Reuse.GetLastTech(MachineName, 0, laststate, lastusername, lastusergroup)
        If laststate = "Fault" Then
            WriteFaultIDTable()
        End If
        If Not RadioButtonList1.SelectedItem Is Nothing Then
            Application(actionstate) = "Confirm"
            Radioselect = RadioButtonList1.SelectedItem.Value
            Select Case Radioselect
                Case 1
                    wcbutton.Text = "Go To Engineering Run up"
                    RaiseEvent AutoApproved(5, lastusername)
                Case 2
                    wcbutton.Text = "Needs Pre-clinical Run up"
                    WriteDatauc1.Visible = True
                    ForceFocus(wctext)
                Case 3
                    wcbutton.Text = "Return to clinical"
                    WriteDatauc1.Visible = True
                    ForceFocus(wctext)
                Case 4
                    wcbutton.Text = "Go To Planned Maintenance"
                    RaiseEvent AutoApproved(5, lastusername)
                Case 6
                    wcbutton.Text = "Go To Physics QA"
                    If lastusergroup = 4 Then
                        RaiseEvent AutoApproved(5, lastusername)
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
                    RaiseEvent AutoApproved(5, lastusername)
            End Select
            'Application("SelectCount") = "False"

            'WritepmComments()
            'WriteDatauc1.Visible = True
        Else
            'tell the user to select a value
            'or stop it getting to here anyway
        End If

    End Sub

    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonList1.SelectedIndexChanged
        'By this time the available buttons should have been set
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        WaitButtons("Tech")
        '    'The solution of how to pass parameter to dynamically loaded user control is from here:
        '    'http://weblogs.asp.net/aghausman/archive/2009/04/15/how-to-pass-parameters-to-the-dynamically-added-user-control.aspx
        Dim tabActive As Integer
        Dim lastusergroup As Integer
        'Dim previousstate As String
        Dim Repairlist As RadioButtonList


        'Dim objCon As UserControl = Page.LoadControl("singlemachinefaultuc.ascx")
        'CType(objCon, Singlemachinefaultuc).LinacName = MachineName
        'CType(objCon, Singlemachinefaultuc).Tabs = "Tech"
        'PlaceHolder1.Controls.Add(objCon)
        objconToday = Page.LoadControl("TodayClosedFault.ascx")
        objconToday.ID = "Todaysfaults"
        objconToday.LinacName = MachineName
        PlaceHolder5.Controls.Add(objconToday)

        Dim objCon As UserControl = Page.LoadControl("ViewOpenFaults.ascx")
        CType(objCon, ViewOpenFaults).LinacName = MachineName
        CType(objCon, ViewOpenFaults).TabName = "Tech"
        CType(objCon, ViewOpenFaults).ID = "ViewOpenFaults"
        PlaceHolder1.Controls.Add(objCon)

        'Dim objCon As UserControl = Page.LoadControl("ViewOpenConcessions.ascx")
        'CType(objCon, ViewOpenConcessions).LinacName = MachineName
        'CType(objCon, ViewOpenConcessions).TabName = "Tech"
        'PlaceHolder1.Controls.Add(objCon)

        Dim objAtlas As UserControl = Page.LoadControl("AtlasEnergyViewuc.ascx")
        CType(objAtlas, AtlasEnergyViewuc).LinacName = MachineName
        PlaceHolder2.Controls.Add(objAtlas)

        Dim objQA As UserControl = Page.LoadControl("WebUserControl2.ascx")
        CType(objQA, WebUserControl2).LinacName = MachineName
        CType(objQA, WebUserControl2).TabName = 5
        PlaceHolder3.Controls.Add(objQA)

        Dim Vctrl As ViewCommentsuc = CType(FindControl("ViewCommentsuc1"), ViewCommentsuc)
        Vctrl.LinacName = MachineName
        Dim wrtctrl As WriteDatauc = CType(FindControl("WriteDatauc1"), WriteDatauc)
        wrtctrl.LinacName = MachineName

        Dim lockctrl As LockElfuc = CType(FindControl("LockElfuc1"), LockElfuc)
        lockctrl.LinacName = MachineName


        AddHandler CType(objCon, ViewOpenFaults).UpDateDefect, AddressOf Update_Today
        AddHandler CType(objCon, ViewOpenFaults).UpDateDefectDisplay, AddressOf Update_Defect
        Dim objDefect As UserControl
        If MachineName Like "T?" Then
            objDefect = Page.LoadControl("DefectSavePark.ascx")
            CType(objDefect, DefectSavePark).ID = "DefectDisplay"
            CType(objDefect, DefectSavePark).LinacName = MachineName
            CType(objDefect, DefectSavePark).ParentControl = 4
            AddHandler CType(objDefect, DefectSavePark).UpDateDefect, AddressOf Update_Today
            AddHandler CType(objDefect, DefectSavePark).UpdateViewFault, AddressOf Update_ViewOpenFaults
            'AddHandler CType(objDefect, DefectSavePark).UpdateUnrecoverableClosed Address Of Update
        Else
            objDefect = Page.LoadControl("DefectSave.ascx")
            CType(objDefect, DefectSave).ID = "DefectDisplay"
            CType(objDefect, DefectSave).LinacName = MachineName
        End If

        PlaceHolder4.Controls.Add(objDefect)


        Dim Textboxcomment As TextBox = FindControl("CommentBox")
        If Not IsPostBack Then
            If MachineName Like "LA?" Then
                RadioButtonList1.Items.Add(New ListItem("Go To Engineering Run up","1", False))
                RadioButtonList1.Items.Add(New ListItem("Requires Pre-Clinical Run up","2", False))
                RadioButtonList1.Items.Add(New ListItem("Hand Back to Clinical","3", False))
                RadioButtonList1.Items.Add(New ListItem("Go To Planned Maintenance","4", False))
                RadioButtonList1.Items.Add(New ListItem("Go To Training/Development","8", False))
                RadioButtonList1.Items.Add(New ListItem("End of Day","102", False))
            Else
                RadioButtonList1.Items.Add(New ListItem("Go To Engineering Run up","1", False))
                RadioButtonList1.Items.Add(New ListItem("Hand Back to Clinical","3", False))
                RadioButtonList1.Items.Add(New ListItem("Go To Planned Maintenance","4", False))
                RadioButtonList1.Items.Add(New ListItem("Go To Training/Development","8", False))
                RadioButtonList1.Items.Add(New ListItem("End of Day","102", False))
            End If
            If (Not HttpContext.Current.Application(BoxChanged) Is Nothing) Then
                Textboxcomment.Text = Application(BoxChanged).ToString
            Else
                'Textboxcomment.Text = comment
            End If
            Application(atlasviewstate) = 1
            Application(qaviewstate) = 1
            Dim NumOpen As Integer
            Dim conn As SqlConnection
            Dim comm As SqlCommand
            Dim reader As SqlDataReader
            Dim connectionString1 As String = ConfigurationManager.ConnectionStrings( _
            "connectionstring").ConnectionString
            conn = New SqlConnection(connectionString1)
            comm = New SqlCommand("select Count(*) as Numopen from FaultIDTable where Status in ('New','Open') and linac=@linac", conn)
            comm.Parameters.AddWithValue("@linac", MachineName)

            conn.Open()
            reader = comm.ExecuteReader()
            If Not FindControl("RadioButtonlist1") Is Nothing Then
                Repairlist = FindControl("RadioButtonlist1")
                If reader.Read() Then
                    NumOpen = reader.Item("NumOpen")
                    If NumOpen <> 0 Then
                        'This sets only return option as log off with open fault
                        'after review meeting 6th august decided that should disable option to log off with open fault. Item 4 disabled in aspx file
                        'Repairlist.Items(4).Enabled = True
                        'Repairlist.Items(4).Selected = True
                        StateTextBox.Text = "Fault"
                        Application(faultstate) = True
                        isFault = Application(faultstate) = True
                        LogOffButton.Enabled = False
                    Else
                        DavesCode.Reuse.GetLastTech(MachineName, 0, laststate, lastuser, lastusergroup)
                        LogOffButton.Enabled = False
                        Application(faultstate) = False
                        isFault = False
                        Repairlist.Items.FindByValue(1).Enabled = True
                        Repairlist.Items.FindByValue(4).Enabled = True

                        Repairlist.Items.FindByValue(8).Enabled = True
                        Repairlist.Items.FindByValue(102).Enabled = True
                        'commented out next if because no Physics QA now 31 march 2016
                        'If lastusergroup = 4 Then
                        '    Repairlist.Items.FindByValue(6).Enabled = True
                        'End If
                        'Repairlist.Items(4).Selected = False
                        'This next if check if got here via clinical suspend
                        StateTextBox.Text = "Linac Unauthorised"
                        If Application(failstate) IsNot Nothing Then
                            Select Case Application(failstate)
                                Case 0
                                    If Application(repairstate) = 1 Then
                                        if MachineName Like "LA?" Then
                                        RadioButtonList1.Items.FindByValue(2).Enabled = True
                                        StateTextBox.Text = "Engineering Approved"
                                            Else
                                        RadioButtonList1.Items.FindByValue(3).Enabled = True
                                        StateTextBox.Text = "Clinical - Not Treating"
                                        End If
                                    End If
                                Case 2 ' this can only happen for LA machines
                                    RadioButtonList1.Items.FindByValue(2).Enabled = True
                                    StateTextBox.Text = "Engineering Approved"
                                Case 3
                                    if MachineName Like "LA?" Then
                                    RadioButtonList1.Items.FindByValue(2).Enabled = True
                                    End If
                                    RadioButtonList1.Items.FindByValue(3).Enabled = True
                                    StateTextBox.Text = "Clinical - Not Treating"
                                Case 4, 5, 8
                                    If Application(suspstate) = 1 Then
                                        if MachineName Like "LA?" Then
                                        RadioButtonList1.Items.FindByValue(2).Enabled = True
                                        End If
                                        RadioButtonList1.Items.FindByValue(3).Enabled = True
                                        StateTextBox.Text = "Suspended"
                                    ElseIf Application(repairstate) = 1 Then
                                        if MachineName Like "LA?" Then
                                        RadioButtonList1.Items.FindByValue(2).Enabled = True
                                        StateTextBox.Text = "Engineering Approved"
                                            Else
                                             RadioButtonList1.Items.FindByValue(3).Enabled = True
                                        StateTextBox.Text = "Clinical - Not Treating"
                                            End If
                                    End If
                                Case Else
                                    'StateTextBox.Text = "Linac Unauthorised"
                            End Select

                        ElseIf Application(suspstate) = 1 Then
                            if MachineName Like "LA?" Then
                            RadioButtonList1.Items.FindByValue(2).Enabled = True
                            End If
                            RadioButtonList1.Items.FindByValue(3).Enabled = True
                            StateTextBox.Text = "Suspended"
                            'End If
                            'Application("Failstate") = 0
                            Dim rtab As String = Application(repairstate)
                        ElseIf Application(repairstate) = 1 Then
                            if MachineName Like "LA?" Then
                                   RadioButtonList1.Items.FindByValue(2).Enabled = True
                                   StateTextBox.Text = "Engineering Approved"
                            Else
                                   RadioButtonList1.Items.FindByValue(3).Enabled = True
                                   StateTextBox.Text = "Clinical - Not Treating"
                            End If
                        End If

                    End If
                    conn.Close()
                Else
                    'Why have we got here

                End If
            End If

        End If

    End Sub


    'Sub BindConcessionGrid()
    '    Dim SqlDataSource2 As New SqlDataSource()
    '    SqlDataSource2.ID = "SqlDataSource2"
    '    SqlDataSource2.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
    '    SqlDataSource2.SelectCommand = "select distinct f.incidentID,  f.Dateinserted, c.ConcessionDescription, c.ConcessionNumber, c.Action, f.linac " & _
    '       "from FaultIDTable f left outer join ConcessionTable c on c.ConcessionNumber = f.ConcessionNumber where f.linac=@linac and f.Status in('Open','Concession') order by incidentid desc"
    '    'Open was added to allow use with singlemachinefaultuc it will only be appropriate for the repair page

    '    'SqlDataSource2.SelectCommand = "select r.FaultID, r.Description, r.ReportedBy, r.DateReported, r.FaultStatus, t.ConcessionNumber, r.linac " & _
    '    '   "from reportfault r left outer join Faulttracking t on r.FaultID=t.FaultID where r.linac=@linac and r.faultstatus=t.status and FaultStatus in('Concession') order by faultid desc"
    '    'SqlDataSource2.SelectCommand = "select * from reportfault where linac=@linac and FaultStatus in('Concession')"
    '    SqlDataSource2.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
    '    SqlDataSource2.SelectParameters.Add("linac", MachineName)
    '    GridView1.DataSource = SqlDataSource2
    '    GridView1.DataBind()
    'End Sub

    'Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
    '    GridView1.PageIndex = e.NewPageIndex
    '    GridView1.DataBind()
    'End Sub

    'Sub FaultGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

    'End Sub
    'Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging

    '    GridView1.PageIndex = e.NewPageIndex
    '    BindConcessionGrid()

    'End Sub

    Protected Sub LockElf_Click(sender As Object, e As System.EventArgs) Handles LockElf.Click
        RemoteLockElf(True)
        'Dim lockctrl As LockElfuc = CType(FindControl("LockElfuc1"), LockElfuc)
        'Dim lockctrltext As TextBox = CType(lockctrl.FindControl("txtchkUserName"), TextBox)
        'Dim suspendvalue As String
        'Dim repairvalue As String
        'Dim username As String = "Lockuser"
        'Dim Textboxcomment As TextBox = FindControl("CommentBox")
        'Dim comment As String = Textboxcomment.Text
        'suspendvalue = Application(suspstate)
        'repairvalue = Application(repairstate)
        'Dim tabused As Integer = 5
        'Dim radioselect As Integer = 5

        'DavesCode.Reuse.WriteAuxTables(MachineName, username, comment, radioselect, tabused, False, suspendvalue, repairvalue, True)
        'lockctrl.Visible = True
        'ForceFocus(lockctrltext)

    End Sub

    Public Sub RemoteLockElf(ByVal realbut As Boolean)
        Dim lockctrl As LockElfuc = CType(FindControl("LockElfuc1"), LockElfuc)
        Dim lockctrltext As TextBox = CType(lockctrl.FindControl("txtchkUserName"), TextBox)
        Dim suspendvalue As String
        Dim repairvalue As String
        Dim username As String = "Lockuser"
        Dim Textboxcomment As TextBox = FindControl("CommentBox")
        Dim comment As String = Textboxcomment.Text
        suspendvalue = Application(suspstate)
        repairvalue = Application(repairstate)
        Dim tabused As Integer = 5
        Dim radioselect As Integer = 101
        Dim breakdown As Boolean = False
        Dim faultstate As String = Nothing

        faultstate = DavesCode.Reuse.GetLastState(MachineName, 0)
        RaiseEvent BlankGroup(0)
        If faultstate = "Fault" Then
            'radioselect = 101
            breakdown = True
        End If
        If Not realbut Then
            username = "System lock"
            '    faultstate = DavesCode.Reuse.GetLastState(MachineName, 0)
            '    If faultstate = "Fault" Then
            '        'radioselect = 101
            '        breakdown = True
            '    End If
            'Else
            '    radioselect = 101
        End If
        Dim lock As Boolean
        lock = Not lockctrl.Visible
        If lock Then
            'If this fails it writes an error to file but carries on.
            DavesCode.NewWriteAux.WriteAuxTables(MachineName, username, comment, radioselect, tabused, breakdown, suspendvalue, repairvalue, lock)
            lockctrl.Visible = True
        End If
        ForceFocus(lockctrltext)
    End Sub
    Protected Sub CommentBox_TextChanged(sender As Object, e As System.EventArgs) Handles CommentBox.TextChanged
        Application(BoxChanged) = CommentBox.Text
        'DavesCode.Reuse.ReturnApplicationState(BoxChanged)
    End Sub
    Private Sub ForceFocus(ByVal ctrl As Control)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "FocusScript", "setTimeout(function(){$get('" + _
        ctrl.ClientID + "').focus();}, 100);", True)
    End Sub

    Public Sub WriteFaultIDTable()
        'There might have been multiple faults open while repair tab was opened so need to update all of them when leaving.
        Dim laststateid As Integer
        Dim conn As SqlConnection
        Dim connectionString As String = ConfigurationManager.ConnectionStrings( _
        "connectionstring").ConnectionString

        Dim updatefault As SqlCommand
        Dim getlaststateid As SqlCommand
        conn = New SqlConnection(connectionString)

        'need to close all faults that have been opened before repair page could be left. Find out what last stateid is then update all records with that time.
        'get last stateid
        getlaststateid = New SqlCommand(("SELECT TOP(1)  [StatusID] FROM FaultIDTable where Linac = @linac ORDER BY [IncidentID] DESC"), conn)
        getlaststateid.Parameters.AddWithValue("@linac", MachineName)

        Try
            conn.Open()
            'getlaststateid.ExecuteNonQuery()
            laststateid = DirectCast(getlaststateid.ExecuteScalar(), Integer)
            conn.Close()
        Catch ex As Exception

        End Try
        updatefault = New SqlCommand("UPDATE  FaultIDTable SET ReportClosed = @ReportClosed where StatusID  = @statusId", conn)
        'updatefault = New SqlCommand("UPDATE  FaultIDTable SET ReportClosed = @ReportClosed where IncidentID  = (Select max(IncidentID) as lastrecord from FaultIDtable where linac=@linac)", conn)
        updatefault.Parameters.Add("@ReportClosed", System.Data.SqlDbType.DateTime)
        updatefault.Parameters("@ReportClosed").Value = Now()
        updatefault.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
        updatefault.Parameters("@linac").Value = MachineName
        updatefault.Parameters.Add("@statusID", System.Data.SqlDbType.Int)
        updatefault.Parameters("@statusID").Value = CInt(laststateid)

        Try
            conn.Open()
            updatefault.ExecuteNonQuery()

        Finally
            conn.Close()
        End Try
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

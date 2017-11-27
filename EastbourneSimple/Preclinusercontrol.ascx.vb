Imports System.Data.SqlClient
Imports System.Data
Imports AjaxControlToolkit
Imports System.Web.UI.Page
Partial Class Preclinusercontrol
    Inherits System.Web.UI.UserControl
    Private MachineName As String
    Private WriteName As String
    Private appstate As String
    Private suspstate As String
    Private actionstate As String
    Private faultviewstate As String
    Private LinacFlag As String
    Private objconToday As TodayClosedFault
    Private Todaydefect As DefectSave
    Private BoxChanged As String
    Private tabstate As String
    
    Public Property LinacName() As String
        Get
            Return MachineName
        End Get
        Set(ByVal value As String)
            MachineName = value
        End Set
    End Property
    Public Property DataName() As String
        Get
            Return WriteName
        End Get
        Set(ByVal value As String)
            WriteName = value
        End Set
    End Property

    Public Function FormatImage(ByVal energy As Boolean) As String
        'Dim happyIcon As String = "Images/happy.gif"
        Dim happyIcon As String = "Images/check_mark.jpg"
        'Dim sadIcon As String = "Images/sad.gif"
        Dim sadIcon As String = "Images/box_with_x.jpg"

        If (energy) Then
                Return happyIcon
            Else
                Return sadIcon
            End If

    End Function
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        AddHandler WriteDatauc1.UserApproved, AddressOf UserApprovedEvent
        AddHandler ConfirmPage1.ConfirmExit, AddressOf ConfirmExitEvent ' this is if imaging wasn't selected
        appstate = "LogOn" + MachineName
        actionstate = "ActionState" + MachineName
        suspstate = "Suspended" + MachineName
        faultviewstate = "Faultsee" + MachineName
        LinacFlag = "State" + MachineName
        BoxChanged = "PreCBoxChanged" + MachineName
        tabstate = "ActTab" + MachineName
        
    End Sub

    Protected Sub Update_Today(ByVal EquipmentID As String, ByVal incidentID As String)
        If MachineName = EquipmentID Then
            Dim todayfault As TodayClosedFault = PlaceHolder5.FindControl("Todaysfaults")
            todayfault.SetGrid()
            Todaydefect = PlaceHolder1.FindControl("DefectDisplay")
            Todaydefect.ResetDefectDropDown(incidentID)
        End If
    End Sub

    Protected Sub Update_Defect(ByVal EquipmentID As String)
        If MachineName = EquipmentID Then
            Todaydefect = PlaceHolder1.FindControl("DefectDisplay")
            Todaydefect.UpDateDefectsEventHandler()
        End If
    End Sub

    Protected Sub ConfirmExitEvent()
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
        wcbutton.Text = "Confirm Pre-Clinical"
        Application(actionstate) = "Confirm"
        WriteDatauc1.Visible = True
        ForceFocus(wctext)
    End Sub

    Protected Sub UserApprovedEvent(ByVal Tabused As String, ByVal Userinfo As String)
        Dim machinelabel As String = MachineName & "Page.aspx';"
        Dim username As String = Userinfo
        'Set these specifically to false 2/12/16
        Dim Valid As Boolean = False
        Dim iView As Boolean = False
        Dim XVI As Boolean = False
        
        If Tabused = "2" Then
            'DavesCode.Reuse.ReturnApplicationState(Tabused)
            Dim Textboxcomment As TextBox = FindControl("CommentBox")
            Dim comment As String = Textboxcomment.Text
            Dim Imagecheck As CheckBoxList
            Imagecheck = FindControl("Imaging")
            Dim strScript As String = "<script>"
            Dim Action As String = Application(actionstate)
            Dim grdviewI As GridView = FindControl("GridViewImage")
            'this changed 21 aug to allow to move on to other states so suspstate is made to be suspended
            Application(appstate) = Nothing
            'Application(suspstate) = 1
            If Action = "Confirm" Then
                Application(LinacFlag) = "Clinical"
                Valid = True
                'Replaced by ReturnImaging so not repeated in two places - erunupusercontroleast
                'Dim cb As CheckBox
                '    Select MachineName

                '    Case "LA1", "LA2", "LA3"
                '        cb = CType(GridViewImage.Rows(0).FindControl("RowLevelCheckBox"), CheckBox)
                '        iView = cb.Checked
                '        'added E1 and LA6 for eastbourne 4/7/17
                '    Case "LA4", "E1", "E2"
                '        cb = CType(GridViewImage.Rows(0).FindControl("RowLevelCheckBox"), CheckBox)
                '        iView = cb.Checked
                '        cb = CType(GridViewImage.Rows(1).FindControl("RowLevelCheckBox"), CheckBox)
                '        XVI = cb.Checked
                'End Select
                DavesCode.Reuse.ReturnImaging(iView,XVI,grdviewI, LinacName)
                DavesCode.Reuse.CommitPreClin(LinacName, username, comment, iView, XVI, Valid, False)
                Dim returnstring As String = MachineName + "page.aspx?tabref=3"
                'DavesCode.Reuse.ReturnApplicationState(Tabused)
                Application(tabstate) = String.Empty
                HttpContext.Current.Application(BoxChanged) = Nothing
                'added application suspstate 31 march 2016
                Application(suspstate) = 1
                Response.Redirect(returnstring)
                'Dim returnstring As String = "AcknowledgeEnergies.aspx?val=" + MachineName
                'Response.Redirect(returnstring)
                'Dim alertstring As String
                'DavesCode.Reuse.CommitPreClin(LinacName, username, Textboxcomment, Imagecheck, Valid)
                'This should probably be a function
                'alertstring = "alert('Pre-clinical Approved Logging Off');"
                'Application("Someoneisloggedin") = Nothing
                'Application("Eng") = Nothing
                'strScript += alertstring
                'strScript += "window.location='"
                'strScript += machinelabel
                'strScript += "</script>"
                'ScriptManager.RegisterStartupScript(clinHandoverButton, Me.GetType(), "JSCR", strScript.ToString(), False)
            Else
                Application(LinacFlag) = "Engineering Approved"
                Valid = False
                HttpContext.Current.Application(BoxChanged) = Nothing
                'DavesCode.Reuse.SetStatus(username, "Engineering Approved", 5, 7, MachineName, 2)
                'Application("Someoneisloggedin") = Nothing
                'Dim strScript As String = "<script>"
                strScript += "alert('No Pre-clinical RunUp Logging Off');"
                DavesCode.Reuse.CommitPreClin(LinacName, username, comment, iView, XVI, Valid, False)
                strScript += "window.location='"
                strScript += machinelabel
                strScript += "</script>"
                ScriptManager.RegisterStartupScript(LogOff, Me.GetType(), "JSCR", strScript.ToString(), False)
            End If
            'this code moved to allow clinical tab to be opened automatically
            'DavesCode.Reuse.CommitPreClin(LinacName, username, comment, iView, XVI, Valid, False)
            'Application(appstate) = Nothing
            'strScript += "window.location='"
            'strScript += machinelabel
            'strScript += "</script>"
            'ScriptManager.RegisterStartupScript(LogOff, Me.GetType(), "JSCR", strScript.ToString(), False)
            HttpContext.Current.Application(BoxChanged) = Nothing
            'DavesCode.Reuse.ReturnApplicationState(Tabused)
        End If


    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        WaitButtons("Rad")
        objconToday = Page.LoadControl("TodayClosedFault.ascx")
        objconToday.ID = "Todaysfaults"
        objconToday.LinacName = MachineName
        PlaceHolder5.Controls.Add(objconToday)
        Dim objCon As UserControl = Page.LoadControl("ViewOpenFaults.ascx")
        CType(objCon, ViewOpenFaults).LinacName = MachineName
        CType(objCon, ViewOpenFaults).TabName = "2"

        Dim button1 As Button = FindControl("clinHandoverButton")
        Dim button2 As Button = FindControl("LogOff")
        'Dim objCon As UserControl = Page.LoadControl("Singlemachinefaultuc.ascx")
        'CType(objCon, Singlemachinefaultuc).LinacName = MachineName
        'CType(objCon, Singlemachinefaultuc).Tabs = "Clin"
        'CType(objCon, Singlemachinefaultuc).PassButton = button1
        'CType(objCon, Singlemachinefaultuc).LogoffPassButton = button2
        PlaceHolder1.Controls.Add(objCon)

        AddHandler CType(objCon, ViewOpenFaults).UpDateDefect, AddressOf Update_Today
        AddHandler CType(objCon, ViewOpenFaults).UpDateDefectDisplay, AddressOf Update_Defect
        Dim objDefect As UserControl = Page.LoadControl("DefectSave.ascx")
        CType(objDefect, DefectSave).ID = "DefectDisplay"
        CType(objDefect, DefectSave).LinacName = MachineName
        PlaceHolder3.Controls.Add(objDefect)

        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        wctrl.LinacName = MachineName
        Dim Vctrl As ViewCommentsuc = CType(FindControl("ViewCommentsuc1"), ViewCommentsuc)
        Vctrl.LinacName = MachineName



        'The solution of how to pass parameter to dynamically loaded user control is from here:
        'http://weblogs.asp.net/aghausman/archive/2009/04/15/how-to-pass-parameters-to-the-dynamically-added-user-control.aspx
        'Dim objConCommit As UserControl = Page.LoadControl("WriteDatauc.ascx")
        'CType(objConCommit, WriteDatauc).LinacName = MachineName
        'CType(objConCommit, WriteDatauc).UserReason = 2
        'CType(objConCommit, WriteDatauc).Tabby = 2
        'CType(objConCommit, WriteDatauc).Source = Page
        'CType(objConCommit, WriteDatauc).Visible = False
        'CType(objConCommit, WriteDatauc).ID = WriteName
        'PlaceHolder2.Controls.Add(objConCommit)
        PlaceHolder2.Visible = True
        Dim Textboxcomment As TextBox = FindControl("CommentBox")
        'If Application(appstate) <> 1 Then
        If Not IsPostBack Then
            'Application("Eng") = Nothing
            'This lot replaced by BindGridView2 on 6th Nov to put in first and last name instead of username
            'Dim SqlDataSource1 As New SqlDataSource()
            'SqlDataSource1.ID = "SqlDataSource1"
            'SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            ''Doesn't matter about valid flag because It can only get here if the flag is valid
            'SqlDataSource1.SelectCommand = "SELECT * FROM [HandoverEnergies] where handoverid = (Select Max(handoverid) as mancount from [handoverenergies] where linac=@linac)"
            'SqlDataSource1.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
            'SqlDataSource1.SelectParameters.Add("linac", MachineName)
            'GridView2.DataSource = SqlDataSource1
            'GridView2.DataBind()
            BindGridview2()
            BindComments()


            'removes engineer comments from display in grid
            GridView2.Columns(13).Visible = False
            'This next step probably isn't the best way of doing this
            'Dim cb As CheckBox
            'cb = CType(GridView1.Rows(0).FindControl("RowLevelCheckBox"), CheckBox)
            'cb.Enabled = True
            'cb.Visible = True
            'cb = CType(GridView1.Rows(1).FindControl("RowLevelCheckBox"), CheckBox)
            'If MachineName = "LA4" Then
            '    cb.Enabled = True
            '    cb.Visible = True
            'Else
            '    cb.Enabled = False
            '    cb.Visible = False
            'End If

            Select Case MachineName
                Case "LA1"
                    For index As Integer = 2 To 5
                        GridView2.Columns(index).Visible = False
                    Next

                Case "LA2", "LA3"
                    For index As Integer = 2 To 5
                        GridView2.Columns(index).Visible = False
                    Next
                    GridView2.Columns(3).Visible = True
                Case "LA4"

                    For index As Integer = 2 To 12
                        GridView2.Columns(index).Visible = False
                    Next
                    GridView2.Columns(3).Visible = True
                Case "E1", "E2", "B1"

                    For index As Integer = 11 To 12
                        GridView2.Columns(index).Visible = False
                    Next
                Case Else
                    'All columns are valid and are displayed

            End Select
            GridView2.Columns(0).Visible = False
            'Dim Textboxcomment As TextBox = FindControl("CommentBox")
            ''If Application(appstate) <> 1 Then
            'If Not IsPostBack Then
            If (Not HttpContext.Current.Application(BoxChanged) Is Nothing) Then
                Textboxcomment.Text = Application(BoxChanged).ToString
            Else
                'Textboxcomment.Text = comment
            End If
            Application(faultviewstate) = 1


            '2/12/16
            SetImaging()


        End If
        'This lot is probably redundant now

        '    Dim reason As Integer
        '    Dim status As String
        '    Dim conn1 As SqlConnection
        '    Dim comm1 As SqlCommand
        '    Dim comm2 As SqlCommand
        '    Dim reader1 As SqlDataReader
        '    Dim connectionString As String = ConfigurationManager.ConnectionStrings( _
        '"connectionstring").ConnectionString
        '    status = ""
        '    reason = 1
        '    conn1 = New SqlConnection(connectionString)

        '    comm2 = New SqlCommand("Select userreason from linacstatus where stateid = (select max(stateid) from linacstatus  where linac=@linac)", conn1)
        '    comm2.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
        '    comm2.Parameters("@linac").Value = MachineName
        '    conn1.Open()
        '    reader1 = comm2.ExecuteReader()
        '    If reader1.Read() Then
        '        reason = reader1.Item("userreason")
        '        reader1.Close()
        '        conn1.Close()
        '    End If


        '    If reason = 7 Then
        '        conn1 = New SqlConnection(connectionString)
        '        comm1 = New SqlCommand("Select state from LinacStatus where stateId = (select max(StateId) from Linacstatus where linac=@linac)", conn1)
        '        comm1.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
        '        comm1.Parameters("@linac").Value = MachineName
        '        conn1.Open()
        '        reader1 = comm1.ExecuteReader()
        '        If reader1.Read() Then
        '            status = reader1.Item("state")
        '            reader1.Close()
        '            conn1.Close()
        '        End If

        '        Select Case status
        '            Case "Fault"
        '                'do nothing
        '            Case "Linac Unauthorised"
        '                'do nothing
        '            Case Else

        '                'This is probably all that's needed I don't think even this is needed now
        '                If Not IsPostBack Then
        '                    Dim reader As SqlDataReader
        '                    Dim EHID As Integer
        '                    Dim LastEHID As Integer
        '                    Dim conn As SqlConnection

        '                    Dim connectionString1 As String = ConfigurationManager.ConnectionStrings( _
        '                    "connectionstring").ConnectionString
        '                    Dim commHAuthID As SqlCommand
        '                    Dim commLastHAuthID As SqlCommand
        '                    conn = New SqlConnection(connectionString1)

        '                    commHAuthID = New SqlCommand("Select HandoverID from HandoverEnergies where HandoverID  = (Select max(HandoverID) as lastrecord from HandoverEnergies where linac=@linac)", conn)
        '                    commHAuthID.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
        '                    commHAuthID.Parameters("@linac").Value = MachineName
        '                    conn.Open()
        '                    reader = commHAuthID.ExecuteReader()
        '                    If reader.Read() Then
        '                        EHID = reader.Item("HandoverId")
        '                        reader.Close()
        '                        conn.Close()
        '                    End If

        '                    commLastHAuthID = New SqlCommand("Select EHandID from ClinicalHandover where EHandID = (Select max(EHandID) as lastrecord from ClinicalHandover where linac=@linac)", conn)
        '                    commLastHAuthID.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
        '                    commLastHAuthID.Parameters("@linac").Value = MachineName
        '                    conn.Open()
        '                    reader = commLastHAuthID.ExecuteReader()
        '                    If reader.Read() Then
        '                        LastEHID = reader.Item("EHandId")
        '                        reader.Close()
        '                        conn.Close()
        '                    End If




        '                    If EHID > LastEHID Then
        '                        'ClinicalAccept.Enabled = True
        '                    Else
        '                        'ClinicalAccept.Enabled = False



        '                    End If

        '                    commHAuthID = New SqlCommand("Select Comment from HandoverEnergies where HandoverID  = (Select max(HandoverID) as lastrecord from HandoverEnergies where linac=@linac)", conn)
        '                    commHAuthID.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
        '                    commHAuthID.Parameters("@linac").Value = MachineName
        '                    conn.Close()
        '                    conn.Open()
        '                    'reader = commHAuthID.ExecuteReader()
        '                    'If reader.Read() Then
        '                    '    Ecom = reader.Item("Comment")
        '                    '    reader.Close()
        '                    '    conn.Close()
        '                    'End If
        '                    'TextBox2.Text = Ecom
        '                    'Repeater1.DataSource = reader
        '                    'Repeater1.DataBind()
        '                    'reader.Close()
        '                    conn.Close()
        '                    BindComments()
        '                End If
        '        End Select
        '        'checkitout_ModalPopupExtender.Show()
        '    Else
        '        'Server.Transfer("default.aspx")
        '    End If
        '    End If
        '    End If
    End Sub
    Private Sub BindComments()
        Dim SqlDateSourceComment As New SqlDataSource()
        Dim query As String = "select e.comment from handoverenergies e  where e.handoverid = (Select Max(handoverid) as mancount from [handoverenergies] where linac=@linac)"
        SqlDateSourceComment = QuerySqlConnection(MachineName, query)
        GridViewComments.DataSource = SqlDateSourceComment
        GridViewComments.DataBind()

    End Sub

    Private Sub BindGridview2()
        Dim SqlDateSourceGridView As New SqlDataSource()
        'Dim query As String = "CREATE TABLE #Users (Username varchar(25),name varchar(50)) insert into #users select u.UserName, (f.FirstName +' ' + f.LastName) as name from ElfMembers.[C:\20ELF\APP_DATA\ASPNETDB.MDF].dbo.aspnet_users u  join ElfMembers.[C:\20Elf\app_data\aspnetdb.mdf].dbo.FirstLastName f on u.UserId = f.UserID  select *, (select distinct u.name from #users u left outer join handoverenergies h1 on h1.LogInName = u.username where h1.LogInName = h.LogInName) as 'Accepted By', (select distinct u.name from #users u left outer join handoverenergies h1 on h1.LogOutName = u.username where h1.LogOutName = h.LogOutName) as 'Approved By' from [C:\20ELF\APP_DATA\PHYSICSENERGY.MDF].dbo.handoverenergies h where h.handoverid = (Select Max(handoverid) as mancount from [handoverenergies] where linac=@linac)"
        Dim query As String = "select HandoverId, MV6,ISNULL(MV6FFF, 0) as ""MV6FFF"",MV10,ISNULL(MV10FFF, 0) as ""MV10FFF"",ISNULL(MeV4,0) as ""MeV4"", MEV6" & _
        ", MEV8, MeV10, MeV12, MeV15, MeV18, MeV20, Comment, LogOutName, LogOutDate, linac, LogInDate, Duration, LogInStatusID, Approved, LogInName, LogOutStatusID" & _
        " From handoverenergies Where handoverid = (Select Max(handoverid) As mancount from [handoverenergies] where linac=@linac)"
        SqlDateSourceGridView = QuerySqlConnection(MachineName, query)
        GridView2.DataSource = SqlDateSourceGridView
        GridView2.DataBind()
    End Sub

    Protected Sub EnergyGridView_DataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GridView2.RowDataBound

        Dim headerRow As GridViewRow = e.Row
        Dim energy As Integer
        Dim conn As SqlConnection
        Dim comm As SqlCommand
        Dim reader As SqlDataReader
        Dim count As Integer
        Dim connectionString1 As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        conn = New SqlConnection(connectionString1)
        'This query has to be changed to accommodate E1 and LA6
        comm = New SqlCommand("Select  MV6, MV6FFF, MV10, MV10FFF, MeV4, MeV6, MeV8, " & _
                                  "MeV10, MeV12, MeV15, MeV18, MeV20 from HandoverEnergies where HandoverID  = (Select max(HandoverID) As lastrecord from HandoverEnergies where linac=@linac)", conn)
        comm.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
        comm.Parameters("@linac").Value = MachineName
        If headerRow.RowType = DataControlRowType.Header Then
            Try
                conn.Open()
                reader = comm.ExecuteReader()
                While reader.Read()
                    For count = 0 To reader.FieldCount - 1
                        'modified to account for dbnull 5/7/17
                        If Not reader.GetValue(count) Is System.DBNull.Value Then
                            energy = reader.GetValue(count)
                        Else
                            energy = 0
                        End If
                        Select Case energy
                            Case -1
                                headerRow.Cells(count + 1).BackColor = System.Drawing.Color.Green
                            Case 0
                                headerRow.Cells(count + 1).BackColor = System.Drawing.Color.Red
                        End Select

                    Next
                End While

                reader.Close()
            Finally
                conn.Close()
            End Try
        End If


    End Sub

    Protected Sub clinHandoverButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles clinHandoverButton.Click
        'Checks to see if imaging has been selected
        'Dim counter As Integer
        'For Each grv As GridViewRow In GridView1.Rows
        '    Dim checktick As CheckBox = CType(GridView1.Rows(0).FindControl("RowLevelCheckBox"), CheckBox)

        '    If checktick.Checked = True Then
        '        counter = counter + 1
        '    End If

        'Next

        'If Imaging.Items(0).Selected Or Imaging.Items(1).Selected Then
        Dim counter As Integer = 0
        'Dim tclcontainer As TabContainer
        For Each grv As GridViewRow In GridViewImage.Rows

            Dim checktick As CheckBox = CType(grv.FindControl("RowlevelCheckBoxImage"), CheckBox)
            If checktick.Checked = True Then
                counter = counter + 1
            End If
        Next
        If counter <> 0 Then
            Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
            Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
            Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
            wcbutton.Text = "Confirm Pre-Clinical"
            Application(actionstate) = "Confirm"
            WriteDatauc1.Visible = True
            ForceFocus(wctext)

        Else
            Dim cptrl As ConfirmPage = CType(FindControl("ConfirmPage1"), ConfirmPage)
            Dim cpbutton As Button = CType(cptrl.FindControl("AcceptOK"), Button)
            'Dim cptext As TextBox = CType(cptrl.FindControl("txtchkUserName"), TextBox)
            cpbutton.Text = "Confirm No Imaging"
            ConfirmPage1.Visible = True
            'ForceFocus(cptext)

        End If





    End Sub
    Private Sub ForceFocus(ByVal ctrl As Control)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "FocusScript", "setTimeout(function(){$get('" + _
        ctrl.ClientID + "').focus();}, 100);", True)
    End Sub

    Protected Sub LogOff_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LogOff.Click
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
        wcbutton.Text = "Log Off"
        Application(actionstate) = "Cancel"
        WriteDatauc1.Visible = True
        ForceFocus(wctext)
    End Sub

    'Protected Sub FaultPanelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FaultPanelButton.Click
    '    Dim panel1 As UpdatePanel = FindControl("FaultUpdatepanel")
    '    If Application("PCFaultsee") = 1 Then
    '        panel1.Visible = True
    '        Application("PCFaultsee") = Nothing
    '        FaultPanelButton.Text = "Hide Open Faults"
    '    Else
    '        panel1.Visible = False
    '        Application("PCFaultsee") = 1
    '        FaultPanelButton.Text = "View Open Faults"
    '    End If

    'End Sub
    Protected Function QuerySqlConnection(ByVal MachineName As String, ByVal query As String) As SqlDataSource
        'This uses the sqldatasource instead of the individual conn definitions so reader can't be used
        'this solution is from http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.sqldatasource.select%28v=vs.90%29.aspx

        Dim SqlDataSource1 As New SqlDataSource()
        SqlDataSource1.ID = "SqlDataSource1"
        SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        SqlDataSource1.SelectCommand = (query)
        SqlDataSource1.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        SqlDataSource1.SelectParameters.Add("linac", MachineName)

        Return SqlDataSource1


    End Function

    Protected Sub CommentBox_TextChanged(sender As Object, e As System.EventArgs) Handles CommentBox.TextChanged
        Application(BoxChanged) = CommentBox.Text
        'DavesCode.Reuse.ReturnApplicationState(BoxChanged)
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
    Protected Sub SetImaging()
        Dim SqlDataSource1 As New SqlDataSource()
        SqlDataSource1.ID = "SqlDataSource1"
        SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

        'This had to be changed to add the imaging modalities for E1 and E2 - added 44 and 45 and 56 and 57. Altered this 2/10 because energyids are not always
        'sequential. Changed to check energy instead which is what is used successfully elsewhere
        'SqlDataSource1.SelectCommand = "SELECT * FROM [physicsenergies] where linac= @linac and EnergyID in (29,30,31,32,33,44,45, 56,57)"

         SqlDataSource1.SelectCommand = "SELECT * FROM [physicsenergies] where linac=@linac and Energy in ('iView','XVI')"

        SqlDataSource1.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        SqlDataSource1.SelectParameters.Add("linac", MachineName)

        GridViewImage.DataSource = SqlDataSource1
        GridViewImage.DataBind()

        Dim conn As SqlConnection
        Dim comm As SqlCommand
        Dim reader As SqlDataReader
        Dim count As Integer = 0
        Dim connectionString1 As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        conn = New SqlConnection(connectionString1)
        'This had to be changed to add the imaging modalities for E1 and E2 - added 44 and 45 and 56 and 57. Altered this 2/10 because energyids are not always
        'sequential. Changed to check energy instead which is what is used successfully elsewhere
        'comm = New SqlCommand("SELECT EnergyID, Approved FROM physicsenergies where linac=@linac and EnergyID in (29,30,31,32,33, 44, 45, 56,57)", conn)
        comm = New SqlCommand("SELECT EnergyID, Approved FROM physicsenergies where linac=@linac and Energy in ('iView','XVI')", conn)
        

        comm.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
        comm.Parameters("@linac").Value = MachineName
        Try
            conn.Open()
            reader = comm.ExecuteReader()
            While reader.Read()
                'This will fall over if approved is null so needs error handling
                'Same fix as Engineering run up energies 4/7/17
                If Not IsDBNull(reader.Item("Approved")) Then
                    If Not reader.Item("Approved") Then
                        Dim cb As CheckBox = CType(GridViewImage.Rows(count).FindControl("RowLevelCheckBoxImage"), CheckBox)
                        cb.Enabled = False
                        cb.Visible = False
                    End If
                Else
                    Dim cb As CheckBox = CType(GridViewImage.Rows(count).FindControl("RowLevelCheckBoxImage"), CheckBox)
                    cb.Enabled = False
                    cb.Visible = False
                End If
                'If Not reader.Item("Approved") Then

                'Dim cb As CheckBox = CType(GridViewImage.Rows(count).FindControl("RowLevelCheckBox"), CheckBox)
                'cb.Enabled = False
                'cb.Visible = True
                'Dim strScript As String = "<script>"
                'strScript += "alert('This imaging modality is disabled');"
                'strScript += "</script>"


                'ScriptManager.RegisterStartupScript(LogOff, Me.GetType(), "JSCR", strScript.ToString(), False)
                'End If

                count = count + 1
            End While
            reader.Close()
        Finally
            conn.Close()

        End Try
    End Sub

    Protected Sub checked(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim check As CheckBox = sender

        'Dim SqlDataSource1 As New SqlDataSource()
        'SqlDataSource1.ID = "SqlDataSource1"
        'SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString


        'SqlDataSource1.SelectCommand = "SELECT * FROM [physicsenergies] where linac= @linac and EnergyID in (29,30,31,32,33)"

        'SqlDataSource1.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        'SqlDataSource1.SelectParameters.Add("linac", MachineName)

        'GridViewImage.DataSource = SqlDataSource1
        'GridViewImage.DataBind()

        Dim conn As SqlConnection
        Dim comm As SqlCommand
        Dim reader As SqlDataReader
        Dim count As Integer = 0
        Dim connectionString1 As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        conn = New SqlConnection(connectionString1)
        comm = New SqlCommand("SELECT Energy, Approved FROM physicsenergies where linac=@linac and Energy in ('iView','XVI')", conn)

        comm.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
        comm.Parameters("@linac").Value = MachineName
        Try
            conn.Open()
            reader = comm.ExecuteReader()
            While reader.Read()
                'This will fall over if approved is null so needs error handling
                If Not reader.Item("Approved") Then
                    Dim strScript As String
                    Dim cb As CheckBox = CType(GridViewImage.Rows(count).FindControl("RowLevelCheckBox"), CheckBox)
                    If cb.Checked Then
                        cb.Checked = False
                        If reader.Item("Energy") = "iview" Then

                            strScript = "<script>alert('iView is not available. See Concession');</script>"
                            'strScript += "alert('This imaging modality has been disabled by Physics. See Concession');"
                            'strScript += "</script>"
                        Else
                            strScript = "<script>alert('XVI is not available. See Concession');</script>"
                        End If

                        ScriptManager.RegisterStartupScript(LogOff, Me.GetType(), "JSCR", strScript.ToString(), False)
                    End If
                    Else
                        'check.Checked = True
                        'BindGridViewImage()
                    End If

                    count = count + 1
            End While
            reader.Close()
        Finally
            conn.Close()

        End Try
    End Sub
    'This Sub is no longer used Commented out 2/10/17
    'Private Sub BindGridViewImage()
    '    Dim SqlDataSource1 As New SqlDataSource()
    '    SqlDataSource1.ID = "SqlDataSource1"
    '    SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString


    '    SqlDataSource1.SelectCommand = "SELECT * FROM [physicsenergies] where linac= @linac and EnergyID in (29,30,31,32,33)"

    '    SqlDataSource1.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
    '    SqlDataSource1.SelectParameters.Add("linac", MachineName)

    '    GridViewImage.DataSource = SqlDataSource1
    '    GridViewImage.DataBind()
    'End Sub
End Class

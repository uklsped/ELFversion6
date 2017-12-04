Imports System.Data.SqlClient
Imports System.Data
Imports AjaxControlToolkit
Imports System.Web.UI.Page

Partial Class ErunupUserControl
    Inherits System.Web.UI.UserControl
    Private MachineName As String
    Private WriteName As String
    Private Reason As Integer
    Private tablabel As String
    Private mpContentPlaceHolder As ContentPlaceHolder
    Private appstate As String
    Private suspstate As String
    Private actionstate As String
    Private failstate As String
    Private repairstate As String
    Private faultviewstate As String
    Private atlasviewstate As String
    Private qaviewstate As String
    Private LinacFlag As String
    Private objconToday As TodayClosedFault
    Private Todaydefect As DefectSave
    Private BoxChanged As String
    Public Event BlankGroup(ByVal BlankUser As Integer)
    Private tabstate As String

    Public ReadOnly Property CurrentComment() As String
        Get
            Return CommentBox.Text
        End Get
    End Property

    Public Property DataName() As String
        Get
            Return WriteName
        End Get
        Set(ByVal value As String)
            WriteName = value
        End Set
    End Property

    Public Property LinacName() As String
        Get
            Return MachineName
        End Get
        Set(ByVal value As String)
            MachineName = value
        End Set
    End Property
    Public Property Tabby() As String
        Get
            Return tablabel
        End Get
        Set(ByVal value As String)
            tablabel = value
        End Set
    End Property
    Public Property UserReason() As Integer

        Get
            Return Reason
        End Get
        Set(ByVal value As Integer)
            Reason = value
        End Set
    End Property


    Public Function FormatImage(ByVal energy As Boolean) As String
        Dim happyIcon As String = "Images/happy.gif"
        Dim sadIcon As String = "Images/sad.gif"
        If (energy) Then
            Return happyIcon
        Else
            Return sadIcon
        End If
    End Function

    Protected Sub checked(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim toggle As CheckBox = CType(GridView1.HeaderRow.FindControl("chkSelectAll"), CheckBox)

        If toggle.Checked = True Then

            For Each grv As GridViewRow In GridView1.Rows
                Dim cb As CheckBox = CType(grv.FindControl("RowlevelCheckBox"), CheckBox)
                If cb.Enabled = True Then
                    cb.Checked = True
                End If
            Next
        Else
            For Each grv As GridViewRow In GridView1.Rows
                Dim cb As CheckBox = CType(grv.FindControl("RowlevelCheckBox"), CheckBox)
                cb.Checked = False
            Next
        End If

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        AddHandler WriteDatauc1.UserApproved, AddressOf UserApprovedEvent
        AddHandler ConfirmPage1.ConfirmExit, AddressOf ConfirmExitEvent ' this is if imaging wasn't selected
        appstate = "LogOn" + MachineName
        actionstate = "ActionState" + MachineName
        suspstate = "Suspended" + MachineName
        failstate = "FailState" + MachineName
        repairstate = "rppTab" + MachineName
        faultviewstate = "Faultsee" + MachineName
        atlasviewstate = "Atlassee" + MachineName
        qaviewstate = "QAsee" + MachineName
        LinacFlag = "State" + MachineName
        BoxChanged = "EngBoxChanged" + MachineName
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

    Protected Sub UserApprovedEvent(ByVal Tabset As String, ByVal Userinfo As String)

        Dim tabcontrol As String = Tabset
        Dim Action As String = Application(actionstate)
        Dim machinelabel As String = MachineName & "Page.aspx';"
        Dim username As String = Userinfo
        Dim Valid As Boolean = False
        Dim Activity As String = "Logged Off"
        Dim iView As Boolean = False
        Dim XVI As Boolean = False
        'Dim Imagecheck As CheckBoxList
        Dim strScript As String = "<script>"
        'Imagecheck = FindControl("Imaging")
        'added for non-LA linac in MachineState
        Dim usergroupselected As Integer = 2
        Dim Reason As Integer = 2
        '1 or 7 is engineering tab or emergency run up tab
        If tabcontrol = "1" Or "7" Then
            'DavesCode.Reuse.ReturnApplicationState(tabcontrol)
            Dim grdview As GridView = FindControl("Gridview1")
            Dim grdviewI As GridView = FindControl("GridViewImage")
            Dim Textboxcomment As TextBox = FindControl("CommentBox")
            Dim Comment As String = Textboxcomment.Text
            If Action = "Confirm" Then
                'This app sets for repair, maintenance and physics tab to know that run up was done
                Application(repairstate) = 1
                Application(LinacFlag) = "Clinical"
                Valid = True
                DavesCode.Reuse.CommitRunup(grdview, LinacName, tablabel, username, Comment, Valid, False, False)
                'inserted to make same as LA linac step without having to activate pre-clin tab
                DavesCode.Reuse.MachineState(Username, usergroupselected, MachineName, Reason, False)
                DavesCode.Reuse.ReturnImaging(iView,XVI,grdviewI, LinacName)
            DavesCode.Reuse.CommitPreClin(LinacName, username, comment, iView,XVI, Valid, False)
            Application(appstate) = Nothing
            HttpContext.Current.Application(BoxChanged) = Nothing
            Application(tabstate) = String.Empty
             Dim returnstring As String = MachineName + "page.aspx?tabref=3"
                
                Application(suspstate) = 1
                Response.Redirect(returnstring)

            Else
               
                Valid = False
                Application(LinacFlag) = "Linac Unauthorised"
                'added this line 27 jan 2016 to set repairstate
                Application(repairstate) = Nothing
               
                strScript += "alert('No Energies Approved Logging Off');"
               DavesCode.Reuse.CommitRunup(grdview, LinacName, tablabel, username, Comment, Valid, False, False)
            'DavesCode.Reuse.CommitPreClin(LinacName, username, comment, iView, XVI, Valid, False)
            Application(appstate) = Nothing
            HttpContext.Current.Application(BoxChanged) = Nothing
            Application(tabstate) = String.Empty
            
            strScript += "window.location='"
            strScript += machinelabel
            strScript += "</script>"
            ScriptManager.RegisterStartupScript(engHandoverButton, Me.GetType(), "JSCR", strScript.ToString(), False)
            End If
            
            
        End If
    End Sub
    Protected Sub Page_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        WaitButtons("Tech")

        objconToday = Page.LoadControl("TodayClosedFault.ascx")
        objconToday.ID = "Todaysfaults"
        objconToday.LinacName = MachineName
        PlaceHolder5.Controls.Add(objConToday)
        Dim websiteAlreadyAccessed As Boolean = False
        Dim objConAtlas As UserControl = Page.LoadControl("AtlasEnergyViewuc.ascx")
        CType(objConAtlas, AtlasEnergyViewuc).LinacName = MachineName
        PlaceHolder2.Controls.Add(objConAtlas)
        Dim objCon As UserControl = Page.LoadControl("ViewOpenFaults.ascx")

        Dim strScript As String = "<script>"

        Dim Vctrl As ViewCommentsuc = CType(FindControl("ViewCommentsuc1"), ViewCommentsuc)
        Vctrl.LinacName = MachineName
        Dim lockctrl As LockElfuc = CType(FindControl("LockElfuc1"), LockElfuc)
        lockctrl.LinacName = MachineName
        If tablabel = 1 Then

            CType(objCon, ViewOpenFaults).TabName = tablabel
            CType(objCon, ViewOpenFaults).LinacName = MachineName
            PlaceHolder3.Controls.Add(objCon)
        Else
            'Hide lock elf button if rad run up
            LockElf.Visible = False
            CType(objCon, ViewOpenFaults).TabName = tablabel
            CType(objCon, ViewOpenFaults).LinacName = MachineName
            PlaceHolder3.Controls.Add(objCon)
        End If

        Dim objQA As UserControl = Page.LoadControl("WebUserControl2.ascx")
        CType(objQA, WebUserControl2).LinacName = MachineName
        CType(objQA, WebUserControl2).TabName = 1
        PlaceHolder6.Controls.Add(objQA)


        AddHandler CType(objCon, ViewOpenFaults).UpDateDefect, AddressOf Update_Today
        AddHandler CType(objCon, ViewOpenFaults).UpDateDefectDisplay, AddressOf Update_Defect
        Dim objDefect As UserControl = Page.LoadControl("DefectSave.ascx")
        CType(objDefect, DefectSave).ID = "DefectDisplay"
        CType(objDefect, DefectSave).LinacName = MachineName
        PlaceHolder1.Controls.Add(objDefect)
        

        'Wire up the event (UserApproved) to the event handler (UserApprovedEvent)
        'The solution of how to pass parameter to dynamically loaded user control is from here:
        'http://weblogs.asp.net/aghausman/archive/2009/04/15/how-to-pass-parameters-to-the-dynamically-added-user-control.aspx
        'Dim objConCommit As WriteDatauc1 = Page.LoadControl("WriteDatauc.ascx")
        'CType(objConCommit, WriteDatauc1).LinacName = MachineName
        'CType(objConCommit, WriteDatauc1).UserReason = 1
        'CType(objConCommit, WriteDatauc1).Tabby = 1
        'CType(objConCommit, WriteDatauc).Source = Page
        'CType(objConCommit, WriteDatauc1).Visible = False
        'CType(objConCommit, WriteDatauc1).ID = WriteName
        'PlaceHolder4.Controls.Add(objConCommit)
        PlaceHolder4.Visible = True
        Dim ControlName As String = WriteName
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        wctrl.LinacName = MachineName
        wctrl.UserReason = Reason
        wctrl.Tabby = tablabel
        Dim comment As String
        Dim Textboxcomment As TextBox = FindControl("CommentBox")
        If Not IsPostBack Then
            If (Not HttpContext.Current.Application(BoxChanged) Is Nothing) Then
                Textboxcomment.Text = Application(BoxChanged).ToString
            Else
                'Textboxcomment.Text = comment
            End If

            'comment = Application("EngBoxChanged").ToString
            'Textboxcomment.Text = comment
            'strScript += "alert('load eng control');"
            'strScript += "</script>"
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "JSCR", strScript.ToString(), False)
            Application(faultviewstate) = 1
            Application(atlasviewstate) = 1
            Application(LinacFlag) = "Linac Unauthorised"
            'This sets up the gridview with all of the available energies
            Dim SqlDataSource1 As New SqlDataSource()
            SqlDataSource1.ID = "SqlDataSource1"
            SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

            If tablabel = 1 Then
                'added imaging
                SqlDataSource1.SelectCommand = "SELECT * FROM [physicsenergies] where linac= @linac and Energy not in ('iView','XVI')"
            Else
                SqlDataSource1.SelectCommand = "SELECT * FROM [physicsenergies] where linac= @linac and EnergyID in (1,2,10,11,19,27,28)"
            End If

            SqlDataSource1.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
            SqlDataSource1.SelectParameters.Add("linac", MachineName)

            GridView1.DataSource = SqlDataSource1
            GridView1.DataBind()

            'This makes visible checkboxes for those energies that are approved
            Dim conn As SqlConnection
            Dim comm As SqlCommand
            Dim reader As SqlDataReader
            Dim count As Integer = 0
            Dim connectionString1 As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
            conn = New SqlConnection(connectionString1)
            If tablabel = 1 Then
                'added imaging
                comm = New SqlCommand("SELECT EnergyID, Approved FROM physicsenergies where linac=@linac and Energy not in ('iView','XVI')", conn)
            Else
                comm = New SqlCommand("SELECT EnergyID, Approved FROM physicsenergies where linac=@linac and EnergyID in (1,2,10,11,19,27,28)", conn)
            End If
            comm.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
            comm.Parameters("@linac").Value = MachineName
            Try
                conn.Open()
                reader = comm.ExecuteReader()
                While reader.Read()
                    'This will fall over if approved is null so needs error handling
                    'Added handling 4/7/17
                    If Not IsDBNull(reader.Item("Approved")) Then
                        If Not reader.Item("Approved") Then
                            Dim cb As CheckBox = CType(GridView1.Rows(count).FindControl("RowLevelCheckBox"), CheckBox)
                            cb.Enabled = False
                            cb.Visible = False
                        End If
                    Else
                        Dim cb As CheckBox = CType(GridView1.Rows(count).FindControl("RowLevelCheckBox"), CheckBox)
                        cb.Enabled = False
                        cb.Visible = False
                    End If
                    count = count + 1
                End While
                reader.Close()
            Finally
                conn.Close()

            End Try
            SetImaging()
        End If
        
        'If websiteAlreadyAccessed = True Then
        '    Dim strScript As String = "<script>"
        '    strScript += "alert('load eng control');"
        '    strScript += "</script>"
        '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "JSCR", strScript.ToString(), False)
        'End If
        PlaceHolder2.Visible = True
        websiteAlreadyAccessed = True
    End Sub
     Protected Sub SetImaging()
        Dim SqlDataSource1 As New SqlDataSource()
        SqlDataSource1.ID = "SqlDataSource1"
        SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
               
         SqlDataSource1.SelectCommand = "SELECT * FROM [physicsenergies] where linac=@linac and Energy in ('iView','XVI') order by energy"

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
       
        comm = New SqlCommand("SELECT EnergyID, Approved FROM physicsenergies where linac=@linac and Energy in ('iView','XVI') order by energy", conn)
        

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
               
                count = count + 1
            End While
            reader.Close()
        Finally
            conn.Close()

        End Try
    End Sub
    'Private Sub BindGridData()
    '    Dim SqlDataSource1 As New SqlDataSource()
    '    SqlDataSource1.ID = "SqlDataSource1"

    '    SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
    '    SqlDataSource1.SelectCommand = "SELECT * FROM [PhysicsEnergies] where linac= @linac"
    '    SqlDataSource1.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
    '    SqlDataSource1.SelectParameters.Add("@linac", MachineName)
    '    GridView1.DataSource = SqlDataSource1
    '    GridView1.DataBind()
    'End Sub


    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For Each grv As GridViewRow In GridView1.Rows
            Dim cb As CheckBox = CType(grv.FindControl("RowlevelCheckBox"), CheckBox)
            If cb.Enabled = True Then
                cb.Checked = True
            End If
        Next
        


    End Sub

    Protected Sub engHandoverButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles engHandoverButton.Click
        'this shouldn't be here
        'Dim lapage As Page

        'lapage = CType(Me.Parent.FindControl("LA3Page"), Page)
        Dim counter As Integer = 0
        'Dim tclcontainer As TabContainer
        For Each grv As GridViewRow In GridView1.Rows

            Dim checktick As CheckBox = CType(grv.FindControl("RowlevelCheckBox"), CheckBox)
            If checktick.Checked = True Then
                counter = counter + 1
            End If
        Next
        If counter <> 0 Then
            'Inserted imaging check from preclinicaluser control
        Dim icounter As Integer = 0
        'Dim tclcontainer As TabContainer
        For Each grv As GridViewRow In GridViewImage.Rows

            Dim checktick As CheckBox = CType(grv.FindControl("RowlevelCheckBoxImage"), CheckBox)
            If checktick.Checked = True Then
                icounter = icounter + 1
            End If
        Next
            If icounter <> 0 Then
            ConfirmExitEvent()
            Else
            Dim cptrl As ConfirmPage = CType(FindControl("ConfirmPage1"), ConfirmPage)
            Dim cpbutton As Button = CType(cptrl.FindControl("AcceptOK"), Button)
            'Dim cptext As TextBox = CType(cptrl.FindControl("txtchkUserName"), TextBox)
            cpbutton.Text = "Confirm No Imaging"
            ConfirmPage1.Visible = True
            'ForceFocus(cptext)

        End If

        Else
            Dim strScript As String = "<script>"
            strScript += "alert('Select at least one energy');"
            strScript += "</script>"
            ScriptManager.RegisterStartupScript(engHandoverButton, Me.GetType(), "JSCR", strScript.ToString(), False)
        End If

        
        


        

    End Sub

    Protected Sub FaultPanelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FaultPanelButton.Click
        Dim updatepanel3 As UpdatePanel = FindControl("updatepanel3")
        If Application(faultviewstate) = 1 Then
            updatepanel3.Visible = True
            Application(faultviewstate) = Nothing
            FaultPanelButton.Text = "Hide Open Faults"
        Else
            updatepanel3.Visible = False
            Application(faultviewstate) = 1
            FaultPanelButton.Text = "View Open Faults"
        End If

    End Sub

    Protected Sub LogOff_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LogOffButton.Click


        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
        wcbutton.Text = "Log Off"
        Application(actionstate) = "Cancel"
        WriteDatauc1.Visible = True
        ForceFocus(wctext)

    End Sub

    Protected Sub AtlasPanelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewAtlasButton.Click
        Dim updateAtlas As UpdatePanel = FindControl("updatepanelatlas")
        If Application(atlasviewstate) = 1 Then
            updateAtlas.Visible = True
            Application(atlasviewstate) = Nothing
            ViewAtlasButton.Text = "Hide Atlas Energies"
        Else
            updateAtlas.Visible = False
            Application(atlasviewstate) = 1
            ViewAtlasButton.Text = "View Atlas Energies"
        End If
    End Sub

    
    Protected Sub CommentBox_TextChanged(sender As Object, e As System.EventArgs) Handles CommentBox.TextChanged
        Application(BoxChanged) = CommentBox.Text
        'DavesCode.Reuse.ReturnApplicationState(BoxChanged)
    End Sub

    '15 April Added this control as a result of Bug 11
    Protected Sub LockElf_Click(sender As Object, e As System.EventArgs) Handles LockElf.Click
        '15 April test mod next 6 lines
        Dim lockctrl As LockElfuc = CType(FindControl("LockElfuc1"), LockElfuc)
        Dim lockctrltext As TextBox = CType(lockctrl.FindControl("txtchkUserName"), TextBox)
        Dim grdview As GridView = FindControl("Gridview1")
        Dim Textboxcomment As TextBox = FindControl("CommentBox")
        Dim Comment As String = Textboxcomment.Text
        'has to be tablable to cope with either tab 1 or 7 control
        DavesCode.Reuse.CommitRunup(grdview, LinacName, tablabel, "Lockuser", Comment, False, False, True)
        RaiseEvent BlankGroup(0)
        lockctrl.Visible = True
        ForceFocus(lockctrltext)

    End Sub

    '15 April comment. Added this control as a result of Bug 11
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

    '15 April Comment added as a result of Bug 6
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
    Protected Sub ConfirmExitEvent()
       Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
            Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
            Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
            wcbutton.Text = "Confirm Energies"
            Application(actionstate) = "Confirm" 'This should only happen if log in is ok move to writedatauc
            WriteDatauc1.Visible = True
            ForceFocus(wctext)
    End Sub
   
End Class

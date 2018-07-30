Imports System.Data.SqlClient
Imports System.Data
Imports AjaxControlToolkit
Imports System.Web.UI.Page
Partial Class Preclinusercontrol
    Inherits System.Web.UI.UserControl
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
    Public Property DataName() As String

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
        appstate = "LogOn" + LinacName
        actionstate = "ActionState" + LinacName
        suspstate = "Suspended" + LinacName
        faultviewstate = "Faultsee" + LinacName
        LinacFlag = "State" + LinacName
        BoxChanged = "PreCBoxChanged" + LinacName
        tabstate = "ActTab" + LinacName

    End Sub

    Protected Sub Update_Today(ByVal EquipmentID As String, ByVal incidentID As String)
        If LinacName = EquipmentID Then
            Dim todayfault As TodayClosedFault = PlaceHolder5.FindControl("Todaysfaults")
            todayfault.SetGrid()
            Todaydefect = PlaceHolder1.FindControl("DefectDisplay")
            Todaydefect.ResetDefectDropDown(incidentID)
        End If
    End Sub

    Protected Sub Update_Defect(ByVal EquipmentID As String)
        If LinacName = EquipmentID Then
            Todaydefect = PlaceHolder1.FindControl("DefectDisplay")
            Todaydefect.UpDateDefectsEventHandler()
        End If
    End Sub

    Protected Sub Update_ViewOpenFaults(ByVal EquipmentID As String)
        If LinacName = EquipmentID Then
            Dim updatefault As ViewOpenFaults = FindControl("ViewOpenFaults")
            updatefault.RebindViewFault()
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
        Dim machinelabel As String = LinacName & "Page.aspx';"
        Dim username As String = Userinfo
        'Set these specifically to false 2/12/16
        Dim Valid As Boolean = False
        Dim iView As Boolean = False
        Dim XVI As Boolean = False

        If Tabused = "2" Then

            Dim Textboxcomment As TextBox = FindControl("CommentBox")
            Dim comment As String = Textboxcomment.Text
            Dim Imagecheck As CheckBoxList
            Imagecheck = FindControl("Imaging")
            Dim strScript As String = "<script>"
            Dim Action As String = Application(actionstate)
            Dim grdviewI As GridView = FindControl("GridViewImage")
            'this changed 21 aug to allow to move on to other states so suspstate is made to be suspended
            Application(appstate) = Nothing

            If Action = "Confirm" Then
                Application(LinacFlag) = "Clinical"
                Valid = True
                DavesCode.Reuse.ReturnImaging(iView, XVI, grdviewI, LinacName)
                DavesCode.Reuse.CommitPreClin(LinacName, username, comment, iView, XVI, Valid, False)
                Dim returnstring As String = LinacName + "page.aspx?tabref=3"
                Application(tabstate) = String.Empty
                HttpContext.Current.Application(BoxChanged) = Nothing
                'added application suspstate 31 march 2016
                Application(suspstate) = 1
                Response.Redirect(returnstring)

            Else
                Application(LinacFlag) = "Engineering Approved"
                Valid = False
                HttpContext.Current.Application(BoxChanged) = Nothing
                strScript += "alert('No Pre-clinical RunUp Logging Off');"
                DavesCode.Reuse.CommitPreClin(LinacName, username, comment, iView, XVI, Valid, False)
                strScript += "window.location='"
                strScript += machinelabel
                strScript += "</script>"
                ScriptManager.RegisterStartupScript(LogOff, Me.GetType(), "JSCR", strScript.ToString(), False)
            End If
            HttpContext.Current.Application(BoxChanged) = Nothing

        End If


    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        WaitButtons("Rad")
        objconToday = Page.LoadControl("TodayClosedFault.ascx")
        objconToday.ID = "Todaysfaults"
        objconToday.LinacName = LinacName
        PlaceHolder5.Controls.Add(objconToday)
        Dim objCon As UserControl = Page.LoadControl("ViewOpenFaults.ascx")
        CType(objCon, ViewOpenFaults).LinacName = LinacName
        CType(objCon, ViewOpenFaults).TabName = "2"
        CType(objCon, ViewOpenFaults).ID = "ViewOpenFaults"

        Dim button1 As Button = FindControl("clinHandoverButton")
        Dim button2 As Button = FindControl("LogOff")

        PlaceHolder1.Controls.Add(objCon)

        AddHandler CType(objCon, ViewOpenFaults).UpDateDefect, AddressOf Update_Today
        AddHandler CType(objCon, ViewOpenFaults).UpDateDefectDisplay, AddressOf Update_Defect
        Dim objDefect As UserControl = Page.LoadControl("DefectSave.ascx")
        CType(objDefect, DefectSave).ID = "DefectDisplay"
        CType(objDefect, DefectSave).LinacName = LinacName
        PlaceHolder3.Controls.Add(objDefect)
        AddHandler CType(objDefect, DefectSave).UpDateDefect, AddressOf Update_Today
        AddHandler CType(objDefect, DefectSave).UpdateViewFault, AddressOf Update_ViewOpenFaults

        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        wctrl.LinacName = LinacName
        Dim Vctrl As ViewCommentsuc = CType(FindControl("ViewCommentsuc1"), ViewCommentsuc)
        Vctrl.LinacName = LinacName

        'The solution of how to pass parameter to dynamically loaded user control is from here:
        'http://weblogs.asp.net/aghausman/archive/2009/04/15/how-to-pass-parameters-to-the-dynamically-added-user-control.aspx

        PlaceHolder2.Visible = True
        Dim Textboxcomment As TextBox = FindControl("CommentBox")

        If Not IsPostBack Then

            BindGridview2()
            BindComments()


            'removes engineer comments from display in grid
            GridView2.Columns(13).Visible = False

            Select Case LinacName
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

            If (Not HttpContext.Current.Application(BoxChanged) Is Nothing) Then
                Textboxcomment.Text = Application(BoxChanged).ToString
            Else
                'Textboxcomment.Text = comment
            End If
            Application(faultviewstate) = 1


            '2/12/16
            SetImaging()

        End If

    End Sub
    Private Sub BindComments()
        Dim SqlDateSourceComment As New SqlDataSource()
        Dim query As String = "select e.comment from handoverenergies e  where e.handoverid = (Select Max(handoverid) as mancount from [handoverenergies] where linac=@linac)"
        SqlDateSourceComment = QuerySqlConnection(LinacName, query)
        GridViewComments.DataSource = SqlDateSourceComment
        GridViewComments.DataBind()

    End Sub

    Private Sub BindGridview2()
        Dim SqlDateSourceGridView As New SqlDataSource()
        Dim query As String = "select HandoverId, MV6,ISNULL(MV6FFF, 0) as ""MV6FFF"",MV10,ISNULL(MV10FFF, 0) as ""MV10FFF"",ISNULL(MeV4,0) as ""MeV4"", MEV6, MEV8, MeV10, MeV12, MeV15, MeV18, MeV20, Comment, LogOutName, LogOutDate, linac, LogInDate, Duration, LogInStatusID, Approved, LogInName, LogOutStatusID From handoverenergies Where handoverid = (Select Max(handoverid) As mancount from [handoverenergies] where linac=@linac)"
        SqlDateSourceGridView = QuerySqlConnection(LinacName, query)
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
        comm = New SqlCommand("Select  MV6, MV6FFF, MV10, MV10FFF, MeV4, MeV6, MeV8, MeV10, MeV12, MeV15, MeV18, MeV20 from HandoverEnergies where HandoverID  = (Select max(HandoverID) As lastrecord from HandoverEnergies where linac=@linac)", conn)
        comm.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
        comm.Parameters("@linac").Value = LinacName
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

        Dim counter As Integer = 0

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
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "FocusScript", "setTimeout(function(){$get('" +
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
        SqlDataSource1.SelectParameters.Add("linac", LinacName)

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
        comm.Parameters("@linac").Value = LinacName
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

End Class

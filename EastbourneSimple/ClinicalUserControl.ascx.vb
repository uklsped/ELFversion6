Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.Page
Imports System.Drawing
Imports AjaxControlToolkit
Imports System.Text



Partial Class ClinicalUserControl
    Inherits System.Web.UI.UserControl
    Private MachineName As String
    Private mpContentPlaceHolder As ContentPlaceHolder
    Private CurrentCID As Integer
    Private colourstart As Color = Color.FromArgb(255, 204, 0)
    Private colourstop As Color = Color.FromArgb(102, 153, 153)
    Private StateLabel As Label
    Private ActivityLabel As Label
    Private appstate As String
    Private suspstate As String
    Private actionstate As String
    Private failstate As String
    Private repairstate As String
    Private faultviewstate As String
    Private treatmentstate As String
    Private clinicalstate As String
    Private returnclinical As String
    Private LinacFlag As String
    Private objconToday As TodayClosedFault
    Private Todaydefect As DefectSave
    Dim BoxChanged As String
    Dim accontrol As AcceptLinac
    Private tabstate As String




    Public Property LinacName() As String
        Get
            Return MachineName
        End Get
        Set(ByVal value As String)
            MachineName = value
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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'The method of finding acceptlinac3 started from here http://forums.asp.net/t/1380670.aspx?Access+Master+page+properties+from+User+Control

        'Dim accontrol As AcceptLinac
        Dim tabcontainer1 As TabContainer
        Page = Me.Page
        mpContentPlaceHolder = _
        CType(Page.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        If Not mpContentPlaceHolder Is Nothing Then
            tabcontainer1 = CType(mpContentPlaceHolder. _
                FindControl("tcl"), TabContainer)
            If Not tabcontainer1 Is Nothing Then
                Dim panelcontrol As TabPanel = tabcontainer1.FindControl("TabPanel3")
                accontrol = panelcontrol.FindControl("AcceptLinac3")
                AddHandler accontrol.ClinicalApproved, AddressOf ClinicalApprovedEvent
            End If
        End If
        AddHandler WriteDatauc1.UserApproved, AddressOf UserApprovedEvent
        AddHandler WriteDatauc2.UserApproved, AddressOf UserApprovedEvent


        If Not mpContentPlaceHolder Is Nothing Then
            StateLabel = CType(mpContentPlaceHolder. _
                FindControl("StateLabel"), Label)
            ActivityLabel = CType(mpContentPlaceHolder.FindControl("ActivityLabel"), Label)
        End If

        appstate = "LogOn" + MachineName
        actionstate = "ActionState" + MachineName
        suspstate = "Suspended" + MachineName
        failstate = "FailState" + MachineName
        repairstate = "rppTab" + MachineName
        faultviewstate = "Faultsee" + MachineName
        clinicalstate = "ClinicalOn" + MachineName
        treatmentstate = "Treatment" + MachineName
        returnclinical = "ReturnClinical" + MachineName
        LinacFlag = "State" + MachineName
        BoxChanged = "ClinBoxChanged" + MachineName
        tabstate = "ActTab" + MachineName
    End Sub

    Public Sub ClinicalApprovedEvent()
        'This is the point that the gridviews are displayed and Clinical Table Data is written
        BindEnergyData()
        WriteClinicalTable()
        Application(suspstate) = Nothing
        'DavesCode.Reuse.ReturnApplicationState("3")
        Dim Textboxcomment As TextBox = FindControl("CommentBox")
        'This looks to see if BoxChanged has a value. if it has the comment has not been saved.
        If (Not HttpContext.Current.Application(BoxChanged) Is Nothing) Then
            'Need to save and then delete app state
            WriteClinicalTableComment(True)
            'Textboxcomment.Text = Application(BoxChanged).ToString
        Else
            'Textboxcomment.Text = comment
        End If
    End Sub

    Protected Sub UserApprovedEvent(ByVal tabused As String, ByVal Userinfo As String)
        'This is called now if the machine is handed over ie the second part. Previously there was a signature
        'for starting and stopping but that isn't used now so the first part is redundant
        Dim Action As String = Application(actionstate)
        Dim machinelabel As String = MachineName & "Page.aspx"
        Dim username As String = Userinfo
        Dim linacstatusid As String = HiddenFieldLinacState.Value
        If tabused = "3" Then
            'DavesCode.Reuse.ReturnApplicationState(tabused)
            If Action = "Confirm" Then
                Application(LinacFlag) = "Clinical"
                Dim Textboxcomment As TextBox = FindControl("CommentBox")
                Dim strScript As String = "<script>"
                'Application("ClinicalGo") = Nothing
                If Application(treatmentstate) = "Yes" Then
                    DavesCode.Reuse.SetStatus(username, "Clinical - Treating", 3, 8, MachineName, 8)
                    Tstart.Text = "Stop Treatment"
                    Tstart.BackColor = Drawing.Color.Yellow
                    Application(treatmentstate) = "No"
                    LogOffButton.Visible = True
                ElseIf Application(treatmentstate) = "No" Then
                    DavesCode.Reuse.SetStatus(username, "Clinical - Not Treating", 3, 3, MachineName, 3)
                    Tstart.Text = "Start Treatment"
                    Tstart.BackColor = Drawing.Color.AntiqueWhite
                    Application(treatmentstate) = "Yes"

                    LogOffButton.Visible = True
                End If
                'DavesCode.Reuse.ReturnApplicationState(tabused)
            End If
            HttpContext.Current.Application(BoxChanged) = Nothing
            WriteDatauc1.Visible = False
        ElseIf tabused = "handover" Then
            If Action = "Confirm" Then

                Dim activity As Integer = 7 'This will always be log off?
                If Application(treatmentstate) = "No" Then
                    activity = 8
                ElseIf Application(treatmentstate) = "Yes" Then
                    activity = 3
                Else
                    'This has gone wrong
                End If
                'DavesCode.Reuse.SetStatus(username, "Suspended", 5, 7, MachineName, activity)

                DavesCode.Reuse.CommitClinical(MachineName, username, False)
                Application(treatmentstate) = "Yes"
                Application(appstate) = Nothing
                Application(suspstate) = 1
                Application(repairstate) = Nothing
                StateLabel.Text = "Suspended"
                ActivityLabel.Text = "Logged Off"
                Application(LinacFlag) = "Suspended"
                'DavesCode.Reuse.ReturnApplicationState(tabused)
                Response.Redirect(machinelabel)

            End If
            Application(tabstate) = String.Empty
            HttpContext.Current.Application(BoxChanged) = Nothing
            WriteDatauc2.Visible = False
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SaveText.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(SaveText, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
        WaitButtons("Rad")
        objconToday = Page.LoadControl("TodayClosedFault.ascx")
        objconToday.ID = "Todaysfaults"
        objconToday.LinacName = MachineName
        PlaceHolder5.Controls.Add(objconToday)
        'added 16march
        'Dim objImage As UserControl = Page.LoadControl("UpdateImaginguc.ascx")
        'CType(objImage, UpdateImaginguc).LinacName = MachineName
        'PlaceHolder6.Controls.Add(objImage)

        Dim conn As SqlConnection
        Dim connectionString1 As String = ConfigurationManager.ConnectionStrings( _
        "connectionstring").ConnectionString
        conn = New SqlConnection(connectionString1)
        Dim lastState As String
        Dim objCon As UserControl = Page.LoadControl("ViewOpenFaults.ascx")
        CType(objCon, ViewOpenFaults).LinacName = MachineName
        PlaceHolder1.Controls.Add(objCon)
        PlaceHolder2.Visible = True
        AddHandler CType(objCon, ViewOpenFaults).UpDateDefect, AddressOf Update_Today
        AddHandler CType(objCon, ViewOpenFaults).UpDateDefectDisplay, AddressOf Update_Defect
        Dim objDefect As UserControl = Page.LoadControl("DefectSave.ascx")
        CType(objDefect, DefectSave).ID = "DefectDisplay"
        CType(objDefect, DefectSave).LinacName = MachineName
        PlaceHolder3.Controls.Add(objDefect)
        'BindEnergyData()

        BindComments()
        Dim Vctrl As ViewCommentsuc = CType(FindControl("ViewCommentsuc1"), ViewCommentsuc)
        Vctrl.LinacName = MachineName
        Dim wctrl1 As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        wctrl1.LinacName = MachineName
        Dim wctrl2 As WriteDatauc = CType(FindControl("Writedatauc2"), WriteDatauc)
        wctrl2.LinacName = MachineName


        Dim Textboxcomment As TextBox = FindControl("CommentBox")

        If Not IsPostBack Then

            Dim treatval As String = Application(treatmentstate)

            Application(faultviewstate) = 1
            lastState = DavesCode.Reuse.GetLastState(MachineName, 0)

            If Application(returnclinical) = 1 Then
                ClinicalApprovedEvent()

                Select Case Application(treatmentstate)
                    Case "Yes"
                        Tstart.Text = "Start Treatment"
                        Tstart.BackColor = colourstart
                        ActivityLabel.Text = "Clinical - Not Treating"
                    Case "No"
                        Tstart.Text = "Stop Treatment"
                        Tstart.BackColor = colourstop
                        LogOffButton.Visible = True
                        ActivityLabel.Text = "Clinical - Treating"
                End Select
                Application(returnclinical) = Nothing
            Else
                SetButtonText()
            End If
            If Application(appstate) = 1 Then
                'If (Not HttpContext.Current.Application(BoxChanged) Is Nothing) Then
                '    Textboxcomment.Text = Application(BoxChanged).ToString
                'Else
                '    'Textboxcomment.Text = comment
                'End If
            End If
            'HttpContext.Current.Application(BoxChanged) = Nothing
            'DavesCode.Reuse.ReturnApplicationState("CUC load")
        End If

    End Sub
    Protected Sub EnergyGridView_DataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GridView2.RowDataBound

        Dim headerRow As GridViewRow = e.Row
        Dim energy As Integer
        Dim query As String
        Dim SqlDataSource1 As New SqlDataSource()
        'Query modified 14/12/16 to fix bug 36
        'query = "Select  MV6, MV10, MeV6, MeV8, " & _
        '                          "MeV10, MeV12, MeV15, MeV18, MeV20, iView, XVI from HandoverEnergies e  left outer join clinicalhandover r on e.handoverid=r.ehandid where e.HandoverID  = (Select max(HandoverID) as lastrecord from HandoverEnergies where linac=@linac)"

        'query = "Select  MV6, MV10, MeV6, MeV8, MeV10, MeV12, MeV15, MeV18, MeV20, iView, XVI, r.CHandID from HandoverEnergies e  left outer join clinicalhandover r on e.handoverid=r.ehandid where e.HandoverID  = (Select max(HandoverID) as lastrecord from HandoverEnergies where linac=@linac)" & _
        '"and r.CHandID = (Select MAX(CHandID) from ClinicalHandover where linac=@linac)"
        query = "Select distinct MV6,MV6FFF, MV10,MV10FFF,MeV4, MeV6, MeV8, " & _
                  "MeV10, MeV12, MeV15, MeV18, MeV20, iView, XVI from HandoverEnergies e  left outer join clinicalhandover r on e.handoverid=r.ehandid where r.CHandID  = (Select max(CHandID) as lastrecord from ClinicalHandover where linac=@linac)"

        SqlDataSource1 = QuerySqlConnection(MachineName, query)

        If headerRow.RowType = DataControlRowType.Header Then
            Try
                Dim dv As DataView
                dv = CType(SqlDataSource1.Select(DataSourceSelectArguments.Empty), DataView)
                'added 16march
                Dim colnum As Integer = dv.Table.Columns.Count - 1
                For count As Integer = 0 To dv.Table.Columns.Count - 1
                    energy = CType(dv.Table.Rows(0)(count), Integer)
                    Select Case energy
                        Case -1
                            headerRow.Cells(count).BackColor = System.Drawing.Color.Green
                        Case 0
                            headerRow.Cells(count).BackColor = System.Drawing.Color.Red
                    End Select
                    'added 16march



                Next
                'This is how it used to be done
                'Dim conn As SqlConnection
                'Dim comm As SqlCommand
                'Dim reader As SqlDataReader
                'Dim count As Integer
                'Dim connectionString1 As String = ConfigurationManager.ConnectionStrings( _
                '"connectionstring").ConnectionString
                'conn = New SqlConnection(connectionString1)
                'comm = New SqlCommand("Select  MV6, MV10, MeV6, MeV8, " & _
                '                          "MeV10, MeV12, MeV15, MeV18, MeV20 from HandoverEnergies where HandoverID  = (Select max(HandoverID) as lastrecord from HandoverEnergies where linac=@linac)", conn)
                'comm.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
                'comm.Parameters("@linac").Value = MachineName

                'conn.Open()
                'reader = comm.ExecuteReader()
                'While reader.Read()
                '    For count = 0 To reader.FieldCount - 1
                '        If Not reader.GetValue(count) Is System.DBNull.Value Then
                '            energy = reader.GetValue(count)
                '            Select Case energy
                '                Case -1
                '                    headerRow.Cells(count).BackColor = System.Drawing.Color.Green
                '                Case 0
                '                    headerRow.Cells(count).BackColor = System.Drawing.Color.Red
                '            End Select

                '        End If
                '    Next
                'End While

                'reader.Close()
            Finally
                'conn.Close()
                'GridView2.DataBind()
            End Try
        End If


    End Sub

    Private Sub BindComments()
        Dim SqlDateSourceComment As New SqlDataSource()
        Dim query As String = "select e.comment, r.Ccomment, c.ClinComment from handoverenergies e left outer join clinicalhandover r on e.handoverid=r.ehandid " & _
        "Left outer join ClinicalTable c on c.PreClinID = r.CHandID where e.handoverid = (Select Max(handoverid) as mancount from [handoverenergies] where linac=@linac) and " & _
        "c.PreClinID = (Select Max(CHandID) as mancount from [ClinicalHandover] where linac=@linac) and " & _
        "c.ClinicalID = (Select Max(ClinicalID) as mancount from [ClinicalTable] where linac = @linac)"
        SqlDateSourceComment = QuerySqlConnection(MachineName, query)
        GridViewComments.DataSource = SqlDateSourceComment
        GridViewComments.DataBind()

    End Sub



    Private Sub BindEnergyData()
        Dim SqlDataSource1 As New SqlDataSource()
        'the distinct takes care of when suspended returns via pre-clinical because then there are two pre ids for one runup id
        'Dim query As String = "Select distinct handoverID, MV6, MV10, MeV6, MeV8, " & _
        '                          "MeV10, MeV12, MeV15, MeV18, MeV20, iView, XVI from HandoverEnergies e  left outer join clinicalhandover r on e.handoverid=r.ehandid where e.HandoverID  = (Select max(HandoverID) as lastrecord from HandoverEnergies where linac=@linac)"

        Dim query As String = "Select distinct handoverID, MV6, MV6FFF, MV10,MV10FFF, MeV4, MeV6, MeV8, " & _
                          "MeV10, MeV12, MeV15, MeV18, MeV20, iView, XVI from HandoverEnergies e  left outer join clinicalhandover r on e.handoverid=r.ehandid where r.CHandID  = (Select max(CHandID) as lastrecord from ClinicalHandover where linac=@linac)"

        SqlDataSource1 = QuerySqlConnection(MachineName, query)
        GridView2.DataSource = SqlDataSource1
        GridView2.DataBind()

        GridView2.Columns(13).Visible = False
        Select Case MachineName
            Case "LA1"
                For index As Integer = 1 To 4
                    GridView2.Columns(index).Visible = False
                Next

            Case "LA2", "LA3"
                For index As Integer = 1 To 4
                    GridView2.Columns(index).Visible = False
                Next
                GridView2.Columns(2).Visible = True
            Case "LA4"

                For index As Integer = 1 To 11
                    GridView2.Columns(index).Visible = False
                Next
                GridView2.Columns(2).Visible = True
                GridView2.Columns(13).Visible = True
            Case "E1", "E2"
                GridView2.Columns(13).Visible = True
                For index As Integer = 10 To 11
                    GridView2.Columns(index).Visible = False
                Next
            Case Else
                'All columns are valid and are displayed

        End Select
        'Select Case MachineName
        '    Case "LA1"
        '        GridView2.Columns(1).Visible = False

        '    Case "LA4"
        '        GridView2.Columns(10).Visible = True
        '        For index As Integer = 2 To 8
        '            GridView2.Columns(index).Visible = False
        '        Next
        '    Case Else
        '        'All columns are valid and are displayed
        'End Select
    End Sub
    Private Sub SetButtonText()

        Dim treatment As String = Application(treatmentstate)
        Select Case treatment
            Case "No"
                Tstart.Text = "Stop Treatment"
                Application(treatmentstate) = "No"

            Case "Yes"
                Tstart.Text = "Start Treatment"
                Application(treatmentstate) = "Yes"

            Case Else
                Tstart.Text = "Start Treatment"
                Application(treatmentstate) = "Yes"
        End Select
        'DavesCode.Reuse.ReturnApplicationState("Set treatment")

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

    Protected Sub LinacHandover_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LogOffButton.Click
        'This hands over linac to enable repair, planned maintenance or Physics QA to take place
        'But it needs to allow existing engineering and pre-clinical run up to be retained
        'It needs a log out as well+
        'WriteClinicalTable()
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc2"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
        wcbutton.Text = "Log off Linac"
        Application(actionstate) = "Confirm"
        WriteDatauc2.Visible = True
        ForceFocus(wctext)

    End Sub
    Private Sub ForceFocus(ByVal ctrl As Control)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "FocusScript", "setTimeout(function(){$get('" + _
        ctrl.ClientID + "').focus();}, 100);", True)
    End Sub
    Protected Sub SaveText_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveText.Click
        WriteClinicalTableComment(False)

    End Sub

    Protected Sub Tstart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Tstart.Click
        Dim linacstatusid As String
        'http://www.javascripter.net/faq/hextorgb.htm to convert from hex to argb


        'Dim tstate As String
        'tstate = Application("treatment")
        'Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        'Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        'If tstate = "No" Then
        '    wcbutton.Text = "Stop Treatment"
        'Else
        '    wcbutton.Text = "Start Treatment"
        'End If
        'Application("ActionState") = "Confirm"
        'WriteDatauc1.Visible = True

        'this is run if writedatauc1 is disabled above
        'Dim statelabel As Label
        'Page = Me.Page
        'mpContentPlaceHolder = _
        'CType(Page.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        'If Not mpContentPlaceHolder Is Nothing Then
        '    statelabel = CType(mpContentPlaceHolder. _
        '        FindControl("StateLabel"), Label)
        'DavesCode.Reuse.ReturnApplicationState("before set treatment")
        If Not StateLabel Is Nothing Then
            linacstatusid = HiddenFieldLinacState.Value
            Dim tval As String = Application(treatmentstate)

            If tval = "Yes" Then
                '30 october
                'DavesCode.Reuse.SetStatus("User", "Clinical - Treating", 3, 8, MachineName, 3)
                DavesCode.Reuse.SetTreatment("Treating", MachineName, linacstatusid)
                Tstart.Text = "Stop Treatment"
                Tstart.BackColor = colourstop
                Application(treatmentstate) = "No"
                LogOffButton.Visible = True
                ActivityLabel.Text = "Clinical - Treating"
                BindEnergyData()
            Else
                'DavesCode.Reuse.SetStatus("User", "Clinical - Not Treating", 3, 3, MachineName, 8)
                DavesCode.Reuse.SetTreatment("Not Treating", MachineName, linacstatusid)
                Tstart.Text = "Start Treatment"
                Tstart.BackColor = colourstart
                Application(treatmentstate) = "Yes"
                LogOffButton.Visible = True
                ActivityLabel.Text = "Clinical - Not Treating"
            End If
            StateLabel.Text = "Clinical"
        End If
        'DavesCode.Reuse.ReturnApplicationState("after set treatment")
        'End If
    End Sub
    Protected Sub WriteClinicalTableComment(ByVal recovered As Boolean)
        'Dim time As DateTime
        'time = Now()
        Dim builder As New StringBuilder
        Dim conn As SqlConnection
        Dim comment As String
        Dim connectionString As String = ConfigurationManager.ConnectionStrings( _
        "connectionstring").ConnectionString
        Dim commCAuthID As SqlCommand
        'Dim theDate As DateTime = System.DateTime.Now
        Dim theTime As String
        theTime = DateTime.Now.ToString("h:mm tt")
        'if recovered then Boxchanged has a value so that should be written to the table and then displayed. If not take the comment that has been entered and write to table
        If recovered Then
            comment = Application(BoxChanged).ToString
        Else
            comment = CommentBox.Text
        End If

        ' Append the time to the stringBuilder
        builder.Append(theTime)
        builder.Append(" <br/>")
        ' Append the comment to the StringBuilder
        builder.Append(comment)
        ' Append a line break
        builder.Append(" <br/>")


        ' Get internal String value from StringBuilder
        Dim ClinicalComment As String = builder.ToString

        conn = New SqlConnection(connectionString)
        commCAuthID = New SqlCommand("Update ClinicalTable Set ClinComment = ClinComment + @Clincomment where ClinicalID  = (Select max(ClinicalID) as lastrecord from ClinicalTable where linac=@linac)", conn)
        'This should check for the length of the text already stored
        commCAuthID.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
        commCAuthID.Parameters("@linac").Value = MachineName
        commCAuthID.Parameters.Add("@clinComment", System.Data.SqlDbType.NVarChar, 250)
        commCAuthID.Parameters("@clinComment").Value = ClinicalComment
        Try
            conn.Open()
            commCAuthID.ExecuteNonQuery()
        Catch SQLExec As SqlException
            Dim err As String = SQLExec.ToString()
            ScriptManager.RegisterStartupScript(Page, Me.GetType(), "Alertmessagebox", "alert('Comment length exceeded');", True)

        Finally
            'Once table entry is written then set text to Nothing
            CommentBox.Text = Nothing
            Application(BoxChanged) = Nothing
            GridViewComments.DataBind()
            conn.Close()

        End Try

    End Sub
    'This happens on log on to create the comments table. Create TreatmentTable as well
    Protected Sub WriteClinicalTable()
        Dim time As DateTime
        time = Now()
        Dim reader As SqlDataReader
        Dim CID As Integer
        Dim CTID As Integer
        Dim ClinD As Integer
        Dim StatusID As Integer
        Dim UserName As String
        Dim conn As SqlConnection
        Dim Clinicalcomment As String
        Dim connectionString As String = ConfigurationManager.ConnectionStrings( _
        "connectionstring").ConnectionString
        Dim commCAuthID As SqlCommand
        Dim commclin As SqlCommand


        Clinicalcomment = CommentBox.Text
        conn = New SqlConnection(connectionString)

        commCAuthID = New SqlCommand("Select CHandID, LogOutStatusID from ClinicalHandover where CHandID  = (Select max(CHandID) as lastrecord from ClinicalHandover where linac=@linac)", conn)
        commCAuthID.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
        commCAuthID.Parameters("@linac").Value = MachineName
        conn.Open()
        CID = 0
        reader = commCAuthID.ExecuteReader()
        If reader.Read() Then
            CID = reader.Item("CHandID")
            CTID = reader.Item("LogOutStatusID")
            reader.Close()
            conn.Close()
        End If
        If Not CID = 0 Then
            commCAuthID = New SqlCommand("Select ClinicalID from ClinicalTable where PreClinID = @CID", conn)
            commCAuthID.Parameters.Add("@CID", System.Data.SqlDbType.NVarChar, 10)
            commCAuthID.Parameters("@CID").Value = CID
            conn.Open()
            reader = commCAuthID.ExecuteReader()

            If reader.Read() Then
                ClinD = reader.Item("clinicalID")
                reader.Close()
                conn.Close()
            Else
                conn.Close()


            End If


            commCAuthID = New SqlCommand("Select ClinComment from ClinicalTable where PreClinID = @CID and ClinicalID = (Select max(ClinicalID) from ClinicalTable where linac=@linac)", conn)
            commCAuthID.Parameters.Add("@CID", System.Data.SqlDbType.NVarChar, 10)
            commCAuthID.Parameters("@CID").Value = CID
            commCAuthID.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
            commCAuthID.Parameters("@linac").Value = MachineName
            conn.Open()
            reader = commCAuthID.ExecuteReader()

            If reader.Read() Then
                Clinicalcomment = reader.Item("ClinComment")
                reader.Close()
                conn.Close()
            Else
                conn.Close()

            End If
            commCAuthID = New SqlCommand("Select StateID, Username from LinacStatus where StateID  = (Select max(StateID) as lastrecord from LinacStatus where linac=@linac)", conn)
            commCAuthID.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
            commCAuthID.Parameters("@linac").Value = MachineName
            conn.Open()
            reader = commCAuthID.ExecuteReader()
            If reader.Read() Then
                StatusID = reader.Item("StateID")
                UserName = reader.Item("Username")
                reader.Close()
                conn.Close()
            End If

            commclin = New SqlCommand("INSERT INTO ClinicalTable (PreClinID,LinacStatusID, clinComment,linac, DateTime, Username) " & _
                                "VALUES ( @PreclinID, @LinacStatusID,@clincomment, @linac, @DateTime, @UserName)", conn)

            commclin.Parameters.Add("@PreClinID", System.Data.SqlDbType.Int)
            commclin.Parameters("@PreclinID").Value = CID
            commclin.Parameters.Add("@LinacStatusID", System.Data.SqlDbType.Int)
            commclin.Parameters("@LinacStatusID").Value = StatusID
            commclin.Parameters.Add("@clinComment", System.Data.SqlDbType.NVarChar, 250)
            commclin.Parameters("@clinComment").Value = Clinicalcomment
            commclin.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
            commclin.Parameters("@linac").Value = MachineName
            commclin.Parameters.Add("@DateTime", System.Data.SqlDbType.DateTime)
            commclin.Parameters("@DateTime").Value = time
            commclin.Parameters.Add("@UserName", System.Data.SqlDbType.NVarChar, 25)
            commclin.Parameters("@UserName").Value = UserName

            Try
                conn.Open()
                commclin.ExecuteNonQuery()

            Finally
                CommentBox.Text = Nothing
                conn.Close()

            End Try

        End If
        HiddenFieldLinacState.Value = StatusID
        'End If
        GridViewComments.DataBind()


        'from http://msdn.microsoft.com/en-us/library/system.string.isnullorempty(v=vs.110).aspx
        'If Not String.IsNullOrEmpty(CTID) Then
        'End If


    End Sub
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
                Dim LogOff As Button = FindControl("LogOffButton")
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

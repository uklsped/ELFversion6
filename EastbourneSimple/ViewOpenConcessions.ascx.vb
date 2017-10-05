﻿'this disappears
Imports System.Data.SqlClient
Imports System.Data
Partial Class ViewOpenConcessions

    Inherits System.Web.UI.UserControl
    Private MachineName As String
    Private TabNo As String
    Private FaultStatus As String
    Private ClinicalButton As Button
    Private suspstate As String
    Private failstate As String
    Private actionstate As String
    Private repairstate As String
    Private laststate As String
    Private faultviewstate As String

    Public Property TabName() As String
        Get
            Return TabNo
        End Get
        Set(ByVal value As String)
            TabNo = value
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
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        AddHandler WriteDatauc1.UserApproved, AddressOf UserApprovedEvent
        AddHandler WriteDatauc3.UserApproved, AddressOf UserApprovedEvent

        'AddHandler FaultGriduc1.ShowFault, AddressOf NewFaultEvent
        suspstate = "Suspended" + MachineName
        failstate = "FailState" + MachineName
        actionstate = "ActionState" + MachineName
        repairstate = "rppTab" + MachineName
        laststate = "previousstate" + MachineName
        faultviewstate = "Faultsee" + MachineName
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not IsPostBack Then
            AddEnergyItem()
        End If

        'This could have come from a valid page or opened up again because fault wasn't closed. In that case
        'The option on the parent page would still be to have LOOF
        'This wouldn't be altered until concession or closed event

        bindGridView()


    End Sub
    Protected Sub SetFaultGrid(ByVal incident As String)
        'Dim faultgrid1 As ManyFaultGriduc = CType(FindControl("manyFaultGriduc1"), ManyFaultGriduc)
        'Dim gridviewfault As GridView = CType(FindControl("Gridview4"), GridView)
        'faultgrid1.Incident = incident

        'Dim row As GridViewRow
        'Dim counter As Integer = 0
        'For Each gvr As GridViewRow In gridviewfault.Rows

        '    counter = counter + 1

        'Next

        'Dim CommandCell As TableCell = row.Cells(9)
        'CommandCell.Visible = True
        ''from http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.buttonfield.commandname%28v=vs.110%29.aspx?cs-save-lang=1&cs-lang=vb#code-snippet-2
    End Sub
    'Protected Sub NewFaultEvent(ByVal incident As String) 'Triggers if new fault and is selected via faultgriduc1
    '    Dim IncidentID As String = incident
    '    SetupStatusTech(IncidentID)
    '    UpdatePanel5.Visible = True
    '    MultiView2.SetActiveView(statustech)
    '    MultiView1.SetActiveView(View1)
    '    'FaultGriduc1.Visible = False
    '    ManyFaultGriduc.Visible = False
    'End Sub

    Protected Sub UserApprovedEvent(ByVal Tabused As String, ByVal Userinfo As String)
        Dim Action As String = Application(actionstate)
        Dim Energy As String
        Dim incidentID As String

        SetViewFault(True)
        If Tabused = "Updatefault" Then

            incidentID = Label2.Text
            Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
            wctrl.Visible = False


            If Action = "Confirm" Then
                GridView1.Enabled = True
                Dim time As DateTime
                time = Now

                Energy = DropDownListEnergy.SelectedItem.Text
                If Energy = "Select" Then
                    Energy = ""
                End If



                Dim conn As SqlConnection

                Dim connectionString As String = ConfigurationManager.ConnectionStrings( _
                "connectionstring").ConnectionString
                Dim incidentfault As SqlCommand
                Dim commfault As SqlCommand

                conn = New SqlConnection(connectionString)

                commfault = New SqlCommand("INSERT INTO ReportFault (Description, ReportedBy, DateReported, Area, Energy, GantryAngle, CollimatorAngle,Linac, IncidentID, BSUHID) " & _
                                           "VALUES (@Description, @ReportedBy, @DateReported, @Area, @Energy,@GantryAngle,@CollimatorAngle, @Linac, @IncidentID, @BSUHID )", conn)
                commfault.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar, 250)
                commfault.Parameters("@Description").Value = TextBox4.Text
                commfault.Parameters.Add("@ReportedBy", System.Data.SqlDbType.NVarChar, 50)
                commfault.Parameters("@ReportedBy").Value = Userinfo
                commfault.Parameters.Add("@DateReported", System.Data.SqlDbType.DateTime)
                commfault.Parameters("@DateReported").Value = time
                commfault.Parameters.Add("@Area", System.Data.SqlDbType.NVarChar, 20)
                commfault.Parameters("@Area").Value = AreaBox.Text
                commfault.Parameters.Add("@Energy", System.Data.SqlDbType.NVarChar, 6)
                commfault.Parameters("@Energy").Value = Energy
                commfault.Parameters.Add("@GantryAngle", System.Data.SqlDbType.NVarChar, 3)
                commfault.Parameters("@GantryAngle").Value = TextBox2.Text
                commfault.Parameters.Add("@CollimatorAngle", System.Data.SqlDbType.NVarChar, 3)
                commfault.Parameters("@CollimatorAngle").Value = TextBox3.Text
                commfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                commfault.Parameters("@Linac").Value = MachineName
                commfault.Parameters.Add("@IncidentID", System.Data.SqlDbType.Int)
                commfault.Parameters("@IncidentID").Value = incidentID
                commfault.Parameters.Add("@BSUHID", System.Data.SqlDbType.VarChar, 7)
                commfault.Parameters("@BSUHID").Value = PatientIDBox.Text
                Try
                    conn.Open()
                    commfault.ExecuteNonQuery()
                    conn.Close()
                Finally
                    conn.Close()

                End Try


            End If
            bindGridView()
            UpdatePanelRepeatFault.Visible = False
            UpdatePanel4.Visible = True
        ElseIf Tabused = "incident" Then
            incidentID = Label4.Text
            'This stops it popping up again when it shouldn't
            Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc3"), WriteDatauc)
            wctrl.Visible = False


            If Action = "Confirm" Then
                Dim username As String = Userinfo
                Dim strScript As String = "<script>"
                Dim selecttext As String
                Dim assignuser As String
                Dim time As DateTime
                time = Now()
                'Dim faultNumber As Integer
                Dim ConcessionNumber As String
                Dim ConcessionDescription As String
                Dim ConcessionActive As Integer
                Dim ConcessionAction As String

                Dim FaultStatus As String
                Dim exists As Integer

                'This should probably happen at the end

                'ConcessiondescriptionBox.ReadOnly = True
                'ConcessionNumberBox.ReadOnly = True
                Dim conn As SqlConnection
                Dim connectionString1 As String = ConfigurationManager.ConnectionStrings( _
                "connectionstring").ConnectionString
                conn = New SqlConnection(connectionString1)
                Try
                    'check that this is successful what happens if it isn't

                    selecttext = DropDownList1.SelectedItem.Text
                    'ConcessionNumber = ConcessionNumberBox.Text
                    ConcessionDescription = ConcessiondescriptionBox.Text
                    assignuser = DropDownList2.SelectedItem.Text
                    FaultStatus = StatusLabel1.Text
                    ConcessionAction = ActionBox.Text
                    If assignuser = "Select" Then
                        assignuser = "Unassigned"
                    End If
                    If selecttext = "Concession" Then
                        ConcessionActive = 1
                    Else
                        ConcessionActive = 0
                    End If

                    'should check that one of the fields has been altered?
                Finally
                    '    conn.Close()

                End Try
                If incidentID <> 0 Then
                    Dim commtrack As SqlCommand
                    commtrack = New SqlCommand("Insert into FaultTracking (Trackingcomment, AssignedTo, Status, LastupdatedBy, Lastupdatedon,   linac, action, incidentID) " & _
                                               "VALUES (@Trackingcomment, @AssignedTo, @Status, @LastupdatedBy, @Lastupdatedon,  @linac, @action, @IncidentID)", conn)
                    commtrack.Parameters.Add("@Trackingcomment", System.Data.SqlDbType.NVarChar, 250)
                    commtrack.Parameters("@Trackingcomment").Value = CommentBox1.Text
                    commtrack.Parameters.Add("@AssignedTo", Data.SqlDbType.NVarChar, 50)
                    commtrack.Parameters("@AssignedTo").Value = assignuser
                    commtrack.Parameters.Add("@Status", Data.SqlDbType.NVarChar, 50)
                    commtrack.Parameters("@Status").Value = DropDownList1.SelectedItem.Text
                    commtrack.Parameters.Add("@LastupdatedBy", System.Data.SqlDbType.NVarChar, 50)
                    commtrack.Parameters("@LastupdatedBy").Value = Userinfo
                    commtrack.Parameters.Add("@Lastupdatedon", System.Data.SqlDbType.DateTime)
                    commtrack.Parameters("@Lastupdatedon").Value = time

                    commtrack.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
                    commtrack.Parameters("@linac").Value = MachineName
                    commtrack.Parameters.Add("@action", System.Data.SqlDbType.NVarChar, 250)
                    commtrack.Parameters("@action").Value = ConcessionAction
                    commtrack.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    commtrack.Parameters("@incidentID").Value = incidentID
                    'This updates the original fault table as well so faultstatus is in two places as is concession number now


                    Dim incidentfault As SqlCommand
                    incidentfault = New SqlCommand("Update FaultIDTable SET Status=@Status WHERE IncidentID=@incidentID", conn)
                    incidentfault.Parameters.Add("@Status", Data.SqlDbType.NVarChar, 50)
                    incidentfault.Parameters("@Status").Value = DropDownList1.SelectedItem.Text
                    incidentfault.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    incidentfault.Parameters("@incidentID").Value = incidentID

                    Dim updateconcession As SqlCommand
                    updateconcession = New SqlCommand("Update ConcessionTable Set Action=@Action where IncidentID=@incidentID", conn)
                    updateconcession.Parameters.Add("@Action", Data.SqlDbType.NVarChar, 250)
                    updateconcession.Parameters("@Action").Value = ConcessionAction
                    updateconcession.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    updateconcession.Parameters("@incidentID").Value = incidentID

                    Dim commconcess As SqlCommand
                    'commconcess = New SqlCommand("Insert into ConcessionTable (ConcessionNumber, ConcessionDescription, IncidentID, Linac, ConcessionActive, Action) " & _
                    '"VALUES (@ConcessionNumber, @ConcessionDescription, @IncidentID, @Linac, @ConcessionActive, @Action) Update FaultIDTable SET ConcessionNumber=@ConcessionNumber WHERE IncidentID=@incidentID ", conn)
                    'commconcess.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 10)
                    'commconcess.Parameters("@ConcessionNumber").Value = ConcessionNumber
                    commconcess = New SqlCommand("Insert into ConcessionTable (PreFix, ConcessionDescription, IncidentID, Linac, ConcessionActive, Action) " & _
                    "VALUES (@PreFix, @ConcessionDescription, @IncidentID, @Linac, @ConcessionActive, @Action) SELECT SCOPE_IDENTITY()", conn)
                    commconcess.Parameters.Add("@PreFix", System.Data.SqlDbType.NVarChar, 10)
                    commconcess.Parameters("@PreFix").Value = "ELF"
                    commconcess.Parameters.Add("@ConcessionDescription", System.Data.SqlDbType.NVarChar, 20)
                    commconcess.Parameters("@ConcessionDescription").Value = ConcessionDescription
                    commconcess.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    commconcess.Parameters("@incidentID").Value = incidentID
                    commconcess.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                    commconcess.Parameters("@Linac").Value = MachineName
                    commconcess.Parameters.Add("@ConcessionActive", System.Data.SqlDbType.Bit)
                    commconcess.Parameters("@ConcessionActive").Value = ConcessionActive
                    commconcess.Parameters.Add("@Action", System.Data.SqlDbType.NVarChar, 250)
                    commconcess.Parameters("@Action").Value = ConcessionAction
                    Dim bcommand = New SqlCommand("select count(*) from Concessiontable where incidentID=@incidentID", conn)
                    bcommand.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    bcommand.Parameters("@incidentID").Value = incidentID
                    Try
                        conn.Open()
                        commtrack.ExecuteNonQuery()

                        'commfault.ExecuteNonQuery()

                        If selecttext = "Concession" Then
                            exists = -1

                            exists = bcommand.ExecuteScalar()

                            If exists = 0 Then
                                'commconcess.ExecuteNonQuery()
                                'from http://www.dotnetperls.com/string-format-vbnet
                                Dim obj As Object = commconcess.ExecuteScalar()
                                Dim value As Integer
                                value = CInt(obj)
                                Dim concessionnum As String = value.ToString("0000")
                                Dim builder As New StringBuilder
                                Dim Prefix As String = "ELF"
                                builder.Append(Prefix)
                                builder.Append(concessionnum)
                                Dim s As String = builder.ToString
                                commconcess = New SqlCommand("Update FaultIDTable SET ConcessionNumber=@ConcessionNumber WHERE IncidentID=@incidentID ", conn)
                                commconcess.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 10)
                                commconcess.Parameters("@ConcessionNumber").Value = s
                                commconcess.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                                commconcess.Parameters("@incidentID").Value = incidentID
                                commconcess.ExecuteNonQuery()
                                conn.Close()



                            Else
                                updateconcession.ExecuteNonQuery()
                                conn.Close()
                            End If
                        End If
                        conn.Close()
                        conn.Open()
                        incidentfault.ExecuteNonQuery()
                        'conn.Close()
                        'conn.Open()
                        'commconcess.ExecuteNonQuery()
                    Finally
                        conn.Close()

                    End Try
                    'This for when there is a closed statement
                    If selecttext = "Closed" Then

                        incidentfault = New SqlCommand("Update FaultIDTable SET DateClosed=@DateClosed WHERE IncidentID=@incidentID", conn)
                        incidentfault.Parameters.Add("@DateClosed", System.Data.SqlDbType.DateTime)
                        incidentfault.Parameters("@DateClosed").Value = time
                        'incidentfault.Parameters("@Status").Value = DropDownList1.SelectedItem.Text
                        incidentfault.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                        incidentfault.Parameters("@incidentID").Value = incidentID
                        updateconcession = New SqlCommand("Update ConcessionTable Set ConcessionActive=@ConcessionActive where IncidentID=@incidentID", conn)
                        updateconcession.Parameters.Add("@ConcessionActive", Data.SqlDbType.Bit)
                        updateconcession.Parameters("@ConcessionActive").Value = 0
                        updateconcession.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                        updateconcession.Parameters("@incidentID").Value = incidentID


                        Try
                            conn.Open()
                            incidentfault.ExecuteNonQuery()
                            updateconcession.ExecuteNonQuery()
                            conn.Close()
                        Finally
                            conn.Close()

                        End Try

                    End If
                    'This  puts  up the grid with all the faults but for now disable it and make the decision in the next bit
                    'GridView1.Enabled = True
                    'bindGridView()
                    'This is where what happens depends on which control it's on
                    'if we've got to here then reset buttons on master so that open fault is now not available or selected and relevant buttons are available

                    If TabName = "Tech" Then

                        Dim NumOpen As Integer
                        Dim Repairlist As RadioButtonList
                        Dim StateBox As TextBox
                        Dim comm As SqlCommand
                        Dim reader As SqlDataReader
                        comm = New SqlCommand("select Count(*) as Numopen from FaultIDTable where Status in ('New','Open') and linac=@linac", conn)
                        comm.Parameters.AddWithValue("@linac", MachineName)

                        conn.Open()
                        reader = comm.ExecuteReader()
                        If Not Me.Parent.FindControl("RadioButtonlist1") Is Nothing And Not Me.Parent.FindControl("StateTextBox") Is Nothing Then
                            Repairlist = Me.Parent.FindControl("RadioButtonlist1")
                            StateBox = Me.Parent.FindControl("StateTextBox")
                            If reader.Read() Then
                                NumOpen = reader.Item("NumOpen")
                                If NumOpen <> 0 Then 'there are open faults

                                    Repairlist.Items(4).Enabled = True
                                    Repairlist.Items(4).Selected = True

                                Else
                                    'But how does it know here to do this?
                                    'Dim lastState As String
                                    'Dim fState As String
                                    GridView1.Enabled = True
                                    bindGridView()
                                    Dim logbutton As Button
                                    Repairlist.Items(0).Enabled = True
                                    Repairlist.Items(3).Enabled = True
                                    Repairlist.Items(4).Enabled = False
                                    Repairlist.Items(4).Selected = False

                                    If Application(suspstate) = 1 Then
                                        Repairlist.Items(1).Enabled = True
                                        Repairlist.Items(2).Enabled = True
                                        StateBox.Text = "Suspended"
                                    End If

                                    Select Case Application(failstate)
                                        Case 2
                                            Repairlist.Items(1).Enabled = True
                                            StateBox.Text = "Engineering Approved"
                                        Case 3
                                            Repairlist.Items(1).Enabled = True
                                            Repairlist.Items(2).Enabled = True
                                            StateBox.Text = "Clinical - Not Treating"
                                        Case Else

                                    End Select
                                    Dim rtab As String = Application(repairstate)
                                    If Application(repairstate) = 1 Then
                                        Repairlist.Items(1).Enabled = True
                                        StateBox.Text = "Engineering Approved"
                                    End If

                                    If Not Me.Parent.FindControl("LogOffButton") Is Nothing Then
                                        logbutton = Me.Parent.FindControl("LogOffButton")
                                        logbutton.Enabled = False
                                    End If
                                End If
                            End If
                        End If
                    Else
                        GridView1.Enabled = True
                        bindGridView()
                    End If

                Else
                    'Linac status now set by repair or other parent container

                    strScript += "alert('Please select a fault or cancel action');"
                    strScript += "</script>"
                    ScriptManager.RegisterStartupScript(SaveAFault, Me.GetType(), "JSCR", strScript.ToString(), False)
                End If


                UpdatePanel5.Visible = False
                UpdatePanel4.Visible = True
            Else
                BindTrackingGridTech(incidentID)
            End If
        End If



    End Sub

    Protected Sub CancelButton_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        GridView1.Enabled = True
        UpdatePanel5.Visible = False
        UpdatePanel4.Visible = True

        ConcessiondescriptionBox.ReadOnly = True
        ConcessionNumberBox.ReadOnly = True

        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc3"), WriteDatauc)
        wctrl.Visible = False
        'FaultGriduc1.Visible = True

        SetViewFault(True)
        Page_Load(Page, e)
    End Sub

    Protected Sub BindOpenfault()

        'This is called when the new fault is changed to Open or Concession
        DropDownList1.SelectedIndex = -1
        DropDownList2.SelectedIndex = -1
        CommentBox1.Text = ""
        Dim SqlDataSource2 As New SqlDataSource()
        SqlDataSource2.ID = "SqlDataSource2"
        SqlDataSource2.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        'SqlDataSource2.SelectCommand = "select * from reportfault where linac=@linac and FaultStatus in('Open', 'Concession') order by faultid desc"
        SqlDataSource2.SelectCommand = "select distinct r.FaultID, r.Description, r.ReportedBy, r.DateReported, f.Status, f.ConcessionNumber, f.linac, f.incidentid " & _
            "from reportfault r left outer join FaultTracking t on r.incidentID=t.incidentID left outer join FaultIDTable f on f.incidentid=r.incidentid where f.status=t.status and f.Status in('Open', 'Concession') order by f.incidentid desc"
        SqlDataSource2.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        SqlDataSource2.SelectParameters.Add("linac", MachineName)

        'UpdatePanel1.Visible = False

    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging

        GridView1.PageIndex = e.NewPageIndex
        bindGridView()

    End Sub

    Sub FaultGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        Dim IncidentID As String
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim row As GridViewRow = GridView1.Rows(index)
        Dim Region As String
        Region = ""
        IncidentID = Server.HtmlDecode(row.Cells(0).Text)
        SetViewFault(False)
        ' If multiple buttons are used in a GridView control, use the
        ' CommandName property to determine which button was clicked.
        Select Case e.CommandName
            Case "View"
                'faultID = CInt(GridView3.SelectedDataKey.Values("FaultID"))


                'Panel2_ModalPopupExtender.Show()


                ' Convert the row index stored in the CommandArgument
                ' property to an Integer.
                'Dim index As Integer = Convert.ToInt32(e.CommandArgument)

                ' Retrieve the row that contains the button clicked 
                ' by the user from the Rows collection.
                'Dim row As GridViewRow = GridView3.Rows(index)
                'faultID = Server.HtmlDecode(row.Cells(0).Text)
                'incidentID = Server.HtmlDecode(row.Cells(0).Text)



                Select Case TabNo
                    Case "1", "4", "5", "Tech"
                        SetupStatusTech(IncidentID)
                        UpdatePanel5.Visible = True
                        MultiView2.SetActiveView(statustech)

                    Case Else
                        BindTrackingGrid(IncidentID)
                        Hidefaults.Visible = True
                        UpdatePanel1.Visible = True
                        MultiView2.SetActiveView(statusother)
                End Select


                MultiView1.SetActiveView(View1)

                GridView1.Enabled = False
                UpdatePanel4.Visible = False



                '        '' Create a new ListItem object for the contact in the row.     
                '        'Dim item As New ListItem()
                '        'item.Text = Server.HtmlDecode(row.Cells(2).Text) & " " & _
                '        '  Server.HtmlDecode(row.Cells(3).Text)

                '        '' If the contact is not already in the ListBox, add the ListItem 
                '        '' object to the Items collection of the ListBox control. 
                '        'If Not ContactsListBox.Items.Contains(item) Then

                '        '    ContactsListBox.Items.Add(item)

                '        'End If
            Case "Log Fault"
                UpdatePanelRepeatFault.Visible = True
                MultiView1.SetActiveView(UpdatefaultView)

                GridView1.Enabled = False
                'incidentID = Server.HtmlDecode(row.Cells(0).Text)
                Label2.Text = IncidentID
                AreaBox.Text = "Machine"

                TextBox2.Text = ""
                TextBox3.Text = ""
                TextBox4.Text = ""

                Dim conn1 As SqlConnection
                Dim comm1 As SqlCommand

                Dim connectionString As String = ConfigurationManager.ConnectionStrings( _
                "connectionstring").ConnectionString


                conn1 = New SqlConnection(connectionString)
                'from http://www.sqlservercentral.com/Forums/Topic1416029-1292-1.aspx
                comm1 = New SqlCommand("SELECT Area from ReportFault where incidentID=@incidentID", conn1)
                comm1.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                comm1.Parameters("@incidentID").Value = IncidentID
                conn1.Open()
                Region = comm1.ExecuteScalar()
                AreaBox.Text = Region
                conn1.Close()
                UpdatePanel4.Visible = False
            Case "Faults"
                'Dim objCon As UserControl = Page.LoadControl("FaultGriduc.ascx")
                'CType(objCon, FaultGriduc).Incident = IncidentID
                Dim objCon As UserControl = Page.LoadControl("ManyFaultGriduc.ascx")
                CType(objCon, ManyFaultGriduc).NewFault = False
                CType(objCon, ManyFaultGriduc).IncidentID = IncidentID
                PlaceHolder1.Controls.Add(objCon)
                UpdatePanel2.Visible = True
                MultiView1.SetActiveView(View2)
                GridView1.Enabled = False
                Hidefaults.Visible = True
                UpdatePanel4.Visible = False
        End Select
        'UpdatePanel4.Visible = False
    End Sub

    Protected Sub SaveAFault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveAFault.Click

        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc3"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)

        If TabName = "Clin" Then
            wctrl.UserReason = "12"
        End If
        Dim faultstatus As String
        Dim ConcessionNumber As String
        Dim ConcessionDescription As String
        Dim ConcessionAction As String
        Dim strScript As String = "<script>"
        Dim incidentid As String
        Dim UniqueC As Boolean

        faultstatus = DropDownList1.SelectedItem.Text
        ConcessionNumber = ConcessionNumberBox.Text
        ConcessionDescription = ConcessiondescriptionBox.Text
        ConcessionAction = ActionBox.Text
        UniqueC = CheckUniqueConcession(ConcessionNumber)
        If faultstatus = "Select" Then
            strScript += "alert('Please select a fault status or cancel action');"
            strScript += "</script>"
            ScriptManager.RegisterStartupScript(SaveAFault, Me.GetType(), "JSCR", strScript.ToString(), False)
            'ElseIf faultstatus = "Concession" And ConcessionNumber = "" Then
            '    strScript += "alert('Please Enter a Concession Number');"
            '    strScript += "</script>"
            '    ScriptManager.RegisterStartupScript(SaveAFault, Me.GetType(), "JSCR", strScript.ToString(), False)
        ElseIf faultstatus = "Concession" And ConcessionDescription = "" Then
            strScript += "alert('Please Enter a Concession Description');"
            strScript += "</script>"
            ScriptManager.RegisterStartupScript(SaveAFault, Me.GetType(), "JSCR", strScript.ToString(), False)
        ElseIf faultstatus = "Concession" And ConcessionAction = "" Then
            strScript += "alert('Please Enter a Concession Action');"
            strScript += "</script>"
            ScriptManager.RegisterStartupScript(SaveAFault, Me.GetType(), "JSCR", strScript.ToString(), False)
        ElseIf faultstatus = "Concession" And Not ConcessionNumberBox.ReadOnly And Not UniqueC Then
            strScript += "alert('That Concession Number has already been used');"
            strScript += "</script>"
            ScriptManager.RegisterStartupScript(SaveAFault, Me.GetType(), "JSCR", strScript.ToString(), False)
        Else
            wcbutton.Text = "Saving Fault Status"
            Application("ActionState") = "Confirm"
            wctrl.Visible = True
            incidentid = Label4.Text
            BindTrackingGridTech(incidentid)
        End If
    End Sub
    Function CheckUniqueConcession(ByVal ConcessionNumber As String) As Boolean
        Dim NewNumber As String = ConcessionNumber
        Dim ncount As Integer = 1
        Dim unique As Boolean = False
        Dim conn As SqlConnection
        Dim comm As SqlCommand
        Dim reader1 As SqlDataReader
        Dim connectionString As String = ConfigurationManager.ConnectionStrings( _
        "connectionstring").ConnectionString
        conn = New SqlConnection(connectionString)
        comm = New SqlCommand("select Count(ConcessionNumber) as 'Count' from ConcessionTable where linac=@linac and ConcessionNumber = @ConcessionNumber", conn)
        comm.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        comm.Parameters("@linac").Value = MachineName
        comm.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar)
        comm.Parameters("@ConcessionNumber").Value = NewNumber

        Try
            conn.Open()
            reader1 = comm.ExecuteReader()
            If reader1.Read() Then
                ncount = reader1.Item("Count")
                reader1.Close()
            End If
        Finally
            conn.Close()
        End Try
        If ncount = 0 Then
            unique = True
        End If
        Return unique
    End Function
    Protected Sub SetupStatusTech(ByVal incidentID As String)
        Dim FaultDescription As New List(Of String)
        'Dim objCon As UserControl = Page.LoadControl("FaultGriduc.ascx")
        'CType(objCon, FaultGriduc).Incident = incidentID
        ''PlaceHolderDisplayfault.Controls.Add(objCon)
        'Dim objCon As UserControl = Page.LoadControl("ManyFaultGriduc.ascx")
        'CType(objCon, ManyFaultGriduc).MachineName = MachineName
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc3"), WriteDatauc)
        'wctrl.Visible = True
        'If Not IsPostBack Then
        BindTrackingGridTech(incidentID)
        DropDownList1.SelectedIndex = -1
        DropDownList2.SelectedIndex = -1
        CommentBox1.Text = String.Empty
        Label4.Text = incidentID
        Dim conn As SqlConnection
        Dim comm As SqlCommand
        Dim connectionString1 As String = ConfigurationManager.ConnectionStrings( _
        "connectionstring").ConnectionString
        conn = New SqlConnection(connectionString1)
        comm = New SqlCommand("select distinct f.status, ISNULL(c.ConcessionNumber, '') as ConcessionNumber , ISNULL(c.concessiondescription, '') as ConcessionDescription, ISNULL(c.action, '') as Action, f.IncidentID " & _
        "from FaultIDTable f left outer join ConcessionTable c on f.ConcessionNumber=c.ConcessionNumber where f.incidentID = @incidentID", conn)
        comm.Parameters.AddWithValue("@incidentID", incidentID)
        Try
            conn.Open()
            Dim da As New SqlDataAdapter(comm)
            Dim dt As New DataTable()
            Dim nconc As String = ""
            Dim ncond As String = ""
            Dim nincid As String
            Dim naction As String = ""

            da.Fill(dt)
            If dt.Rows.Count > 0 Then

                For Each dataRow As DataRow In dt.Rows

                    FaultStatus = dataRow("Status")
                    nconc = dataRow("ConcessionNumber")
                    ncond = dataRow("ConcessionDescription")
                    nincid = dataRow("incidentID")
                    naction = dataRow("action")

                Next
            End If

            StatusLabel1.Text = FaultStatus

            ConcessionNumberBox.Text = nconc
            ConcessiondescriptionBox.Text = ncond
            ActionBox.Text = naction

            If FaultStatus = "Concession" Then
                DropDownList1.Items.FindByValue("Open").Enabled = False
                ConcessiondescriptionBox.ReadOnly = True
                ConcessionNumberBox.ReadOnly = True
            End If

        Finally
            conn.Close()

        End Try




        'comm = New SqlCommand("select FaultStatus  from reportfault where linac=@linac and FaultID = @FaultID", conn)
        'comm = New SqlCommand("select distinct r.FaultID, r.Description, r.ReportedBy, r.DateReported, r.FaultStatus, r.Area, r.Energy, r.GantryAngle, r.CollimatorAngle,t.ConcessionNumber, t.concessiondescription, r.linac, t.IncidentID " & _
        '    "from reportfault r left outer join faulttracking t on r.FaultID=t.FaultID left outer join faultidtable f on f.incidentid=t.incidentid where r.linac=@linac and f.status=t.status and r.FaultID = @FaultID", conn)
        comm = New SqlCommand("select distinct r.FaultID, r.Description, r.ReportedBy, r.DateReported, f.Status, r.Area, r.Energy, r.GantryAngle, r.CollimatorAngle, r.BSUHID,ISNULL(c.ConcessionNumber, '') as ConcessionNumber , ISNULL(c.concessiondescription, '') as ConcessionDescription, f.linac, f.IncidentID " & _
        "from reportfault r left outer join FaultIDTable f on f.OriginalFaultID = r.FaultID left outer join ConcessionTable c on f.ConcessionNumber=c.ConcessionNumber where f.linac=@linac and f.incidentID = @incidentID", conn)
        comm.Parameters.AddWithValue("@linac", MachineName)
        comm.Parameters.AddWithValue("@incidentID", incidentID)
        Try
            conn.Open()
            Dim da As New SqlDataAdapter(comm)
            Dim dt As New DataTable()
            Dim nfaultid As String
            Dim ndescription As String
            Dim nrep As String
            Dim ndate As String
            Dim nen As String
            Dim nga As String
            Dim nca As String
            Dim nlin As String
            Dim narea As String
            Dim nconc As String
            Dim ncond As String
            Dim nincid As String
            Dim nbsuhid As String


            da.Fill(dt)
            If dt.Rows.Count > 0 Then

                For Each dataRow As DataRow In dt.Rows
                    nfaultid = dataRow("FaultId")
                    ndescription = dataRow("Description")
                    nrep = dataRow("ReportedBy")
                    ndate = dataRow("DateReported")
                    FaultStatus = dataRow("Status")
                    narea = dataRow("Area")
                    nen = dataRow("Energy")
                    nga = dataRow("GantryAngle")
                    nca = dataRow("CollimatorAngle")
                    nconc = dataRow("ConcessionNumber")
                    ncond = dataRow("ConcessionDescription")
                    nlin = dataRow("Linac")
                    nincid = dataRow("incidentID")
                    nbsuhid = dataRow("BSUHID")

                Next
            End If

            OriginalDescriptionBox.Text = ndescription
            OriginalAreaBox.Text = narea
            OriginalEnergyBox.Text = nen
            OriginalGantryBox.Text = nga
            OriginalCollBox.Text = nca
            OriginalReportedBox.Text = nrep
            OriginalOpenDateBox.Text = ndate
            StatusLabel1.Text = FaultStatus
            OriginalPatientIDBox.Text = nbsuhid

        Finally
            conn.Close()
        End Try

    End Sub
    Protected Sub NewbindGridView()

        Dim SqlDataSource2 As New SqlDataSource()
        SqlDataSource2.ID = "SqlDataSource2"
        SqlDataSource2.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        SqlDataSource2.SelectCommand = "select distinct f.incidentID,  f.Dateinserted, c.ConcessionDescription, c.ConcessionNumber, c.Action, f.linac " & _
           "from FaultIDTable f left outer join ConcessionTable c on c.ConcessionNumber = f.ConcessionNumber where f.linac=@linac and f.Status in('New') order by incidentid desc"
        'Open was added to allow use with singlemachinefaultuc it will only be appropriate for the repair page
        SqlDataSource2.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        SqlDataSource2.SelectParameters.Add("linac", MachineName)
        GridView1.DataSource = SqlDataSource2
        GridView1.DataBind()

    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataBind()
    End Sub
    Protected Sub bindGridView()
        Dim SqlDataSource2 As New SqlDataSource()
        SqlDataSource2.ID = "SqlDataSource2"
        SqlDataSource2.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        SqlDataSource2.SelectCommand = "select distinct f.incidentID,  f.Dateinserted, c.ConcessionDescription, c.ConcessionNumber, c.Action, f.linac " & _
           "from FaultIDTable f left outer join ConcessionTable c on c.ConcessionNumber = f.ConcessionNumber where f.linac=@linac and f.Status in('Open','Concession') order by incidentid desc"
        'Open was added to allow use with singlemachinefaultuc it will only be appropriate for the repair page

        'SqlDataSource2.SelectCommand = "select r.FaultID, r.Description, r.ReportedBy, r.DateReported, r.FaultStatus, t.ConcessionNumber, r.linac " & _
        '   "from reportfault r left outer join Faulttracking t on r.FaultID=t.FaultID where r.linac=@linac and r.faultstatus=t.status and FaultStatus in('Concession') order by faultid desc"
        'SqlDataSource2.SelectCommand = "select * from reportfault where linac=@linac and FaultStatus in('Concession')"
        SqlDataSource2.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        SqlDataSource2.SelectParameters.Add("linac", MachineName)
        GridView1.DataSource = SqlDataSource2
        GridView1.DataBind()
        CheckEmptyGrid(GridView1)

    End Sub
    Protected Sub BindTrackingGrid(ByVal incidentID As String)
        Dim incidentNumber As String = incidentID
        'faultNumber = CInt(GridView3.SelectedDataKey.Values("FaultID"))

        Dim SqlDataSource3 As New SqlDataSource()
        SqlDataSource3.ID = "SqlDataSource3"
        SqlDataSource3.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        'SqlDataSource3.SelectCommand = "select * from FaultTracking where linac=@linac and FaultID=@FaultID"
        SqlDataSource3.SelectCommand = "select t.TrackingID, t.trackingcomment, t.AssignedTo, t.Status, t.LastUpDatedBy, t.LastUpDatedOn, c.ConcessionNumber, t.linac, t.incidentID " & _
            "from FaultTracking t left outer join ConcessionTable c on c.incidentID=t.incidentID where t.linac=@linac and t.incidentID=@incidentID order by trackingid asc"
        SqlDataSource3.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        SqlDataSource3.SelectParameters.Add("linac", MachineName)
        SqlDataSource3.SelectParameters.Add("@incidentID", System.Data.SqlDbType.Int)
        SqlDataSource3.SelectParameters.Add("incidentID", incidentNumber)
        GridView2.DataSource = SqlDataSource3
        GridView2.DataBind()
    End Sub
    Protected Sub BindTrackingGridTech(ByVal incidentID As String)
        Dim SqlDataSource3 As New SqlDataSource()
        SqlDataSource3.ID = "SqlDataSource3"
        SqlDataSource3.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        SqlDataSource3.SelectCommand = "select t.TrackingID, t.trackingcomment, t.AssignedTo, t.Status, t.LastUpDatedBy, t.LastUpDatedOn, t.linac, t.action " & _
            "from FaultTracking t  where t.linac=@linac and t.incidentID=@incidentID"
        SqlDataSource3.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        SqlDataSource3.SelectParameters.Add("linac", MachineName)
        SqlDataSource3.SelectParameters.Add("@incidentID", System.Data.SqlDbType.Int)
        SqlDataSource3.SelectParameters.Add("incidentID", incidentID)
        GridView5.DataSource = SqlDataSource3
        GridView5.DataBind()
    End Sub

    Protected Sub Hidefaults_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Hidefaults.Click
        GridView1.Visible = True
        GridView1.Enabled = True
        GridView1.SelectedIndex = -1
        UpdatePanel2.Visible = False
        Hidefaults.Visible = False
        UpdatePanel4.Visible = True
        SetViewFault(True)
    End Sub

    Protected Sub confirmfault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles confirmfault.Click
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        Dim strScript As String = "<script>"

        wcbutton.Text = "Saving Repeat Fault"
        Application(actionstate) = "Confirm"
        wctrl.Visible = True

    End Sub
    Protected Sub AddEnergyItem()
        'from http://www.aspsnippets.com/Articles/Programmatically-add-items-to-DropDownList-on-Button-Click-in-ASPNet-using-C-and-VBNet.aspx
        'and http://www.yaldex.com/asp_tutorial/0596004877_progaspdotnet2-chp-5-sect-7.html

        'This is a mad way of doing it but I don't know how to dim the energy list without constructing it at the same time

        Select Case MachineName
            Case "LA1"
                Dim Energy1() As String = {"Select", "6 MV", "6 MeV", "8 MeV", "10 MeV", "12 MeV", "15 MeV", "18 MeV", "20 MeV"}
                ConstructEnergylist(Energy1)
            Case "LA2", "LA3"
                Dim Energy1() As String = {"Select", "6 MV", "10 MV", "6 MeV", "8 MeV", "10 MeV", "12 MeV", "15 MeV", "18 MeV", "20 MeV"}
                ConstructEnergylist(Energy1)
            Case "LA4"
                Dim Energy1() As String = {"Select", "6 MV", "10 MV"}
                ConstructEnergylist(Energy1)
            Case Else
                'Don't show any energies
        End Select

    End Sub
    Protected Sub ConstructEnergylist(ByVal Energylist() As String)
        Dim energy() As String = Energylist
        Dim i As Integer
        For i = 0 To energy.GetLength(0) - 1
            DropDownListEnergy.Items.Add(New ListItem(energy(i)))
        Next
        DropDownListEnergy.SelectedIndex = -1
    End Sub
    Protected Sub Cancel_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancel.Click
        'GridView1.Visible = True
        'GridView1.SelectedIndex = -1
        GridView1.Enabled = True
        UpdatePanelRepeatFault.Visible = False
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        wctrl.Visible = False
        UpdatePanel4.Visible = True
        SetViewFault(True)
        'If TabName = "Clin" Then
        '    ClinicalButton.Enabled = True
        '    LogoffButton.Enabled = True
        'End If
        Page_Load(Page, e)
    End Sub

    Protected Sub ViewExistingFaults_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewExistingFaults.Click
        Dim incidentID As String
        incidentID = Label2.Text
        bindGridViewVEF(incidentID)
    End Sub

    Protected Sub bindGridViewVEF(ByVal IncidentID As String)
        Dim incidentNumber As String = IncidentID
        'faultNumber = CInt(GridView3.SelectedDataKey.Values("FaultID"))

        Dim SqlDataSource4 As New SqlDataSource()
        SqlDataSource4.ID = "SqlDataSource3"
        SqlDataSource4.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        SqlDataSource4.SelectCommand = "select FaultID, Description, ReportedBy,DateReported,Area,Energy,GantryAngle,CollimatorAngle,Linac from ReportFault where incidentID=@incidentID order by FaultID desc"
        SqlDataSource4.SelectParameters.Add("@incidentID", System.Data.SqlDbType.Int)
        SqlDataSource4.SelectParameters.Add("incidentID", incidentNumber)
        GridView4.DataSource = SqlDataSource4
        GridView4.DataBind()
    End Sub
    Protected Sub Dropdownlist1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
        Dim updateFaultStatus As String
        Dim incidentid As String
        FaultStatus = StatusLabel1.Text
        If FaultStatus = "Concession" Then
            ConcessionNumberBox.ReadOnly = True
            ConcessiondescriptionBox.ReadOnly = True
            ActionBox.ReadOnly = False
        Else
            updateFaultStatus = DropDownList1.SelectedItem.Text
            If updateFaultStatus = "Concession" Then
                'ConcessionNumberBox.ReadOnly = False
                ConcessiondescriptionBox.ReadOnly = False
                ActionBox.ReadOnly = False
            ElseIf updateFaultStatus = "Open" Then
                ConcessionNumberBox.Text = ""
                ConcessiondescriptionBox.Text = ""
                ActionBox.Text = ""
                ConcessionNumberBox.ReadOnly = True
                ConcessiondescriptionBox.ReadOnly = True
                ActionBox.ReadOnly = True
            ElseIf updateFaultStatus = "Closed" Then
                ConcessionNumberBox.ReadOnly = True
                ConcessiondescriptionBox.ReadOnly = True
                ActionBox.ReadOnly = True
            End If
        End If
        incidentid = Label4.Text
        BindTrackingGridTech(incidentid)
    End Sub
    Public Sub CheckEmptyGrid(ByVal grid As WebControls.GridView)
        'If grid.Rows.Count = 0 And Not grid.DataSource Is Nothing Then
        '    Dim dt As Object = Nothing
        '    If grid.DataSource.GetType Is GetType(Data.DataSet) Then
        '        dt = New System.Data.DataSet
        '        dt = CType(grid.DataSource, System.Data.DataSet).Tables(0).Clone()
        '    ElseIf grid.DataSource.GetType Is GetType(Data.DataTable) Then
        '        dt = New System.Data.DataTable
        '        dt = CType(grid.DataSource, System.Data.DataTable).Clone()
        '    ElseIf grid.DataSource.GetType Is GetType(Data.DataView) Then
        '        dt = New System.Data.DataView
        '        dt = CType(grid.DataSource, System.Data.DataView).Table.Clone
        '    End If
        '    dt.Rows.Add(dt.NewRow())
        '    grid.DataSource = dt
        '    grid.DataBind()
        '    Dim columnsCount As Integer
        '    Dim tCell As New TableCell()
        '    columnsCount = grid.Columns.Count
        '    grid.Rows(0).Cells.Clear()
        '    grid.Rows(0).Cells.Add(tCell)
        '    grid.Rows(0).Cells(0).ColumnSpan = columnsCount


        '    grid.Rows(0).Cells(0).Text = "No Records To Display"
        '    grid.Rows(0).Cells(0).HorizontalAlign = HorizontalAlign.Center
        '    grid.Rows(0).Cells(0).ForeColor = Drawing.Color.Black
        '    grid.Rows(0).Cells(0).Font.Bold = True




        'End If
    End Sub

    Public Sub SetViewFault(ByVal OnOff As Boolean)
        If Not Me.Parent.FindControl("FaultpanelButton") Is Nothing Then
            Dim Fbutton As Button = Me.Parent.FindControl("FaultPanelButton")
            Fbutton.Enabled = OnOff
        End If
    End Sub


End Class

Imports System.Data.SqlClient
Partial Class faultPage

    Inherits System.Web.UI.Page
    Private tabIndex As String
    Private appstate As String
    Private suspstate As String
    Private actionstate As String
    Private failstate As String
    Private repairstate As String
    Private treatmentstate As String
    Private clinicalstate As String
    Private returnclinical As String
    Private machinename As String
    Public Event ClinicalApprovedEvent()

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        AddHandler WriteDataucfr.UserApproved, AddressOf UserApprovedEvent

    End Sub

    Protected Sub UserApprovedEvent(ByVal Tabused As String, ByVal Userinfo As String)
        'Alter magic number later
        Dim faulttab As String = 666
        machinename = MachineLabel.Text
        appstate = "LogOn" + machinename
        actionstate = "ActionState" + machinename
        suspstate = "Suspended" + machinename
        failstate = "FailState" + machinename
        repairstate = "rppTab" + machinename
        clinicalstate = "ClinicalOn" + machinename
        treatmentstate = "Treatment" + machinename
        returnclinical = "ReturnClincal" + machinename
        Dim actionflag As String = actionstate
        Dim Action As String = Application(actionstate)
        Dim EnergyPicked As String
        If Tabused = "Report" Then

            Dim mpContentPlaceHolder As ContentPlaceHolder
            Dim grdview As GridView

            mpContentPlaceHolder = _
            CType(Me.Master.FindControl("ContentPlaceHolder1"),  _
            ContentPlaceHolder)
            If Not mpContentPlaceHolder Is Nothing Then
                grdview = CType(mpContentPlaceHolder.FindControl("DummyGridview"), GridView)
            End If


            If Action = "Confirm" Then
                Dim time As DateTime
                Dim LinacID As String = MachineLabel.Text
                Dim LastFault As Integer
                Dim LastIncident As Integer
                'tabby is the tab from where the fault was reported
                Dim tabby As String = HiddenField1.Value
                Dim comment As String = DummyCommentBox.Value 'want it here for commit run ups
                Dim breakdown As Boolean = True
                Dim LinacStatusID As String = ""
                time = Now()
                'This has the effect of logging off the linac

                Select Case tabby
                    Case 0
                        'Modified from 5,4 after 27th may tests
                        LinacStatusID = DavesCode.Reuse.SetStatus(Userinfo, "Fault", 5, 5, LinacID, 5)
                    Case 1, 7
                        DavesCode.Reuse.CommitRunup(grdview, LinacID, faulttab, Userinfo, comment, False, True)
                    Case 2
                        DavesCode.Reuse.CommitPreClin(LinacID, Userinfo, comment, False, False, False, True)
                    Case 3
                        DavesCode.Reuse.CommitClinical(LinacID, Userinfo, True)
                    Case 4, 5, 6
                        'LinacStatusID = DavesCode.Reuse.SetStatus(Userinfo, "Fault", 5, 4, LinacID, 5)
                        'DavesCode.Reuse.Writepm(LinacID, Userinfo, comment, LinacStatusID)
                        DavesCode.Reuse.WriteAuxTables(LinacID, Userinfo, comment, -1, tabby, True)
                        'Case 5
                        '    'LinacStatusID = DavesCode.Reuse.SetStatus(Userinfo, "Fault", 5, 4, LinacID, 4)
                        '    DavesCode.Reuse.WriteAuxTables(LinacID, Userinfo, comment, -1, 5, True)
                        'Case 6
                        '    'LinacStatusID = DavesCode.Reuse.SetStatus(Userinfo, "Fault", 5, 4, LinacID, 6)
                        '    DavesCode.Reuse.WriteAuxTables(LinacID, Userinfo, comment, -1, 6, True)
                    Case Else
                        'LinacStatusID = DavesCode.Reuse.SetStatus(Userinfo, "Fault", 5, 4, LinacID, 5)
                        DavesCode.Reuse.WriteAuxTables(LinacID, Userinfo, comment, -1, 5, True)
                End Select

                'Need to write to comments records of various sorts here.
                'DavesCode.Reuse.CommitComment(LinacID, 1, Userinfo, comment)
                'DavesCode.Reuse.SetStatus(Userinfo, "Fault", 5, 4, LinacID, 8)


                Dim conn As SqlConnection

                Dim connectionString As String = ConfigurationManager.ConnectionStrings( _
                "connectionstring").ConnectionString
                Dim incidentfault As SqlCommand
                Dim commfault As SqlCommand
                conn = New SqlConnection(connectionString)


                incidentfault = New SqlCommand("INSERT INTO FaultIDTable (DateInserted, Linac, Status, originalFaultID, ConcessionNumber) VALUES (@DateInserted, @Linac, @Status, @originalFaultID, @ConcessionNumber)", conn)
                incidentfault.Parameters.Add("@DateInserted", System.Data.SqlDbType.DateTime)
                incidentfault.Parameters("@DateInserted").Value = time
                incidentfault.Parameters.Add("@Status", System.Data.SqlDbType.NVarChar, 20)
                incidentfault.Parameters("@Status").Value = "New"
                incidentfault.Parameters.Add("@originalFaultID", System.Data.SqlDbType.Int)
                incidentfault.Parameters("@originalFaultID").Value = 0
                incidentfault.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 10)
                incidentfault.Parameters("@ConcessionNumber").Value = ""
                incidentfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                incidentfault.Parameters("@Linac").Value = LinacID

                conn.Open()
                incidentfault.ExecuteNonQuery()
                conn.Close()

                Dim comm1 As SqlCommand
                Dim reader1 As SqlDataReader

                comm1 = New SqlCommand("Select IncidentID from FaultIDTable where IncidentId = (select max(IncidentId) from FaultIDTable where linac = @linac)", conn)
                comm1.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                comm1.Parameters("@Linac").Value = LinacID
                conn.Open()
                reader1 = comm1.ExecuteReader()
                If reader1.Read() Then
                    LastIncident = reader1.Item("IncidentID")
                    reader1.Close()
                    conn.Close()
                End If

                EnergyPicked = DropDownListEnergy.SelectedItem.Text
                If EnergyPicked = "Select" Then
                    EnergyPicked = ""
                End If
                commfault = New SqlCommand("INSERT INTO ReportFault (Description, ReportedBy, DateReported, Area, Energy, GantryAngle, CollimatorAngle,Linac, IncidentID) " & _
                                           "VALUES (@Description, @ReportedBy, @DateReported, @Area, @Energy,@GantryAngle,@CollimatorAngle, @Linac, @IncidentID )", conn)
                commfault.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar, 250)
                commfault.Parameters("@Description").Value = TextBox4.Text
                commfault.Parameters.Add("@ReportedBy", System.Data.SqlDbType.NVarChar, 50)
                commfault.Parameters("@ReportedBy").Value = Userinfo
                commfault.Parameters.Add("@DateReported", System.Data.SqlDbType.DateTime)
                commfault.Parameters("@DateReported").Value = time
                commfault.Parameters.Add("@Area", System.Data.SqlDbType.NVarChar, 20)
                commfault.Parameters("@Area").Value = DropDownListArea.SelectedItem.Text
                commfault.Parameters.Add("@Energy", System.Data.SqlDbType.NVarChar, 6)
                'commfault.Parameters("@Energy").Value = TextBox1.Text
                commfault.Parameters("@Energy").Value = EnergyPicked
                'DropDownListEnergy.SelectedItem.Text
                commfault.Parameters.Add("@GantryAngle", System.Data.SqlDbType.NVarChar, 3)
                commfault.Parameters("@GantryAngle").Value = TextBox2.Text
                commfault.Parameters.Add("@CollimatorAngle", System.Data.SqlDbType.NVarChar, 3)
                commfault.Parameters("@CollimatorAngle").Value = TextBox3.Text
                commfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                commfault.Parameters("@Linac").Value = LinacID
                commfault.Parameters.Add("@IncidentID", System.Data.SqlDbType.Int)
                commfault.Parameters("@IncidentID").Value = LastIncident
                Try
                    conn.Open()
                    commfault.ExecuteNonQuery()
                    conn.Close()
                    'This reads the number of the newly created fault to put into the fault tracking database
                    'Dim comm1 As SqlCommand
                    'Dim reader1 As SqlDataReader
                    'This will need to be modified to put  in which linac has the fault on it
                    comm1 = New SqlCommand("Select FaultID from ReportFault where FaultId = (select max(FaultId) from ReportFault where linac = @linac)", conn)
                    comm1.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                    comm1.Parameters("@Linac").Value = LinacID
                    conn.Open()
                    reader1 = comm1.ExecuteReader()
                    If reader1.Read() Then
                        LastFault = reader1.Item("FaultID")
                        reader1.Close()
                        conn.Close()
                    End If

                Finally
                    conn.Close()

                End Try

                Dim commtrack As SqlCommand
                commtrack = New SqlCommand("Insert into FaultTracking (Trackingcomment, AssignedTo, Status, LastupdatedBy, Lastupdatedon, Linac, IncidentID) " & _
                                           "VALUES (@Trackingcomment, @AssignedTo, @Status, @LastupdatedBy, @Lastupdatedon, @Linac, @IncidentID)", conn)
                commtrack.Parameters.Add("@Trackingcomment", System.Data.SqlDbType.NVarChar, 250)
                commtrack.Parameters("@Trackingcomment").Value = TextBox4.Text
                commtrack.Parameters.Add("@AssignedTo", Data.SqlDbType.NVarChar, 50)
                commtrack.Parameters("@AssignedTo").Value = "Unassigned"
                commtrack.Parameters.Add("@Status", Data.SqlDbType.NVarChar, 50)
                commtrack.Parameters("@Status").Value = "New"
                commtrack.Parameters.Add("@LastupdatedBy", System.Data.SqlDbType.NVarChar, 50)
                commtrack.Parameters("@LastupdatedBy").Value = Userinfo
                commtrack.Parameters.Add("@Lastupdatedon", System.Data.SqlDbType.DateTime)
                commtrack.Parameters("@Lastupdatedon").Value = time
                'commtrack.Parameters.Add("@FaultID", System.Data.SqlDbType.Int)
                'commtrack.Parameters("@FaultID").Value = LastFault
                commtrack.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                commtrack.Parameters("@Linac").Value = MachineLabel.Text
                'commtrack.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 10)
                'commtrack.Parameters("@ConcessionNumber").Value = DummyConcessionNumber.Value
                'commtrack.Parameters.Add("@ConcessionDescription", System.Data.SqlDbType.NVarChar, 20)
                'commtrack.Parameters("@ConcessionDescription").Value = DummyConcessionDescription.Value
                'commtrack.Parameters.Add("@ConcessionActive", System.Data.SqlDbType.TinyInt)
                'commtrack.Parameters("@ConcessionActive").Value = 0
                commtrack.Parameters.Add("@IncidentID", System.Data.SqlDbType.Int)
                commtrack.Parameters("@IncidentID").Value = LastIncident
                Try
                    conn.Open()
                    commtrack.ExecuteNonQuery()


                Finally
                    conn.Close()

                End Try

                'update incident table with fault id then don't need it in fault tracking table?

                incidentfault = New SqlCommand("Update FaultIDTable SET originalFaultID=@originalFaultID where incidentID=@incidentID", conn)
                incidentfault.Parameters.Add("@originalFaultID", Data.SqlDbType.Int)
                incidentfault.Parameters("@originalFaultID").Value = LastFault
                incidentfault.Parameters.Add("@incidentID", Data.SqlDbType.Int)
                incidentfault.Parameters("@incidentID").Value = LastIncident
                conn.Open()
                incidentfault.ExecuteNonQuery()
                conn.Close()




                'Dim commconcess As SqlCommand
                'Dim readerc As SqlDataReader
                'Dim lasttrack As Integer
                'commconcess = New SqlCommand("Select FaultID from ReportFault where FaultId = (select max(FaultId) from ReportFault where linac = @linac)", conn)
                'commconcess.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                'commconcess.Parameters("@Linac").Value = LinacID
                'conn.Open()
                'readerc = commconcess.ExecuteReader()
                'If readerc.Read() Then
                '    lasttrack = readerc.Item("FaultID")
                '    readerc.Close()
                '    conn.Close()
                'End If

                'commconcess = New SqlCommand("Insert into ConcessionTable (ConcessionNumber, ConcessionDescription, TrackingID, Linac, ConcessionActive) " & _
                '"VALUES (@ConcessionNumber, @ConcessionDescription, @trackingID, @Linac, @ConcessionActive)", conn)
                'commconcess.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 10)
                'commconcess.Parameters("@ConcessionNumber").Value = DummyConcessionNumber.Value
                'commconcess.Parameters.Add("@ConcessionDescription", System.Data.SqlDbType.NVarChar, 20)
                'commconcess.Parameters("@ConcessionDescription").Value = DummyConcessionDescription.Value
                'commconcess.Parameters.Add("@trackingID", System.Data.SqlDbType.Int)
                'commconcess.Parameters("@trackingID").Value = lasttrack
                'commconcess.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                'commconcess.Parameters("@Linac").Value = MachineLabel.Text
                'commconcess.Parameters.Add("@ConcessionActive", System.Data.SqlDbType.Bit)
                'commconcess.Parameters("@ConcessionActive").Value = False
                'Try
                '    conn.Open()
                '    commconcess.ExecuteNonQuery()

                'Finally
                '    conn.Close()

                'End Try

                Application(appstate) = Nothing
                Application(suspstate) = Nothing
                Application(repairstate) = Nothing
                Application(treatmentstate) = "Yes"
                'FailState is the flag that tells repair and view open faults which tab the fault was reported from
                Application(failstate) = HiddenField1.Value
                Dim tab As String
                'This sets the return tab as repair - important that it is always tab 5!
                tab = "5"
                'don't want to return saved value of comment here because this sets the fault page. Want empty string
                comment = ""
                Dim returnstring As String = LinacID + "page.aspx?pageref=Fault&Tabindex="
                Response.Redirect(returnstring & tab & "&comment=" & comment)
                'Response.Redirect("LA1page.aspx?pageref=Fault&Tabindex=" & tab)
            End If
            '    Dim username As String = Userinfo
            '    Dim strScript As String = "<script>"
            '    Dim alertstring As String
            '    Dim time As DateTime
            '    time = Now()
            '    Dim conn As SqlConnection
            '    Dim connectionString As String = ConfigurationManager.ConnectionStrings( _
            '    "connectionstring").ConnectionString
            '    conn = New SqlConnection(connectionString)
            '    Dim commupdate As New SqlCommand("update PhysicsEnergies set Approved=@Approved, ApprovedBy=@ApprovedBy, DateApproved=@DateApproved, Comment=@Comment, linac=@linac where EnergyID=@EnergyID", conn)
            '    Dim row As Integer = Application("EnergyQA")
            '    Dim erow As GridViewRow
            '    erow = GridView1.Rows(row)

            '    Dim tenergy As String
            '    tenergy = erow.Cells(2).ToString

            '    Dim cb As CheckBox = erow.Cells(3).Controls(0)
            '    Dim txtComment As TextBox = erow.Cells(6).Controls(0)
            '    Dim keyfieldvalue As Integer = Application("keyfield")
            '    conn.Open()
            '    commupdate.Parameters.Add("@EnergyID", System.Data.SqlDbType.Int).Value = keyfieldvalue
            '    commupdate.Parameters.Add("@Approved", SqlDbType.Bit).Value = cb.Checked
            '    commupdate.Parameters.Add("@ApprovedBy", SqlDbType.VarChar).Value = username
            '    commupdate.Parameters.Add("@DateApproved", SqlDbType.DateTime).Value = Now
            '    commupdate.Parameters.Add("@Comment", SqlDbType.VarChar).Value = txtComment.Text
            '    commupdate.Parameters.Add("@linac", SqlDbType.NVarChar).Value = MachineName
            '    commupdate.ExecuteNonQuery()
            '    'Message.Text = txtApprovedby.Text
            '    conn.Close()
            '    GridView1.EditIndex = -1
            '    BindGridData()



            '    alertstring = "alert('Energy Updated');"
            '    strScript += alertstring
            '    strScript += "</script>"
            '    ScriptManager.RegisterStartupScript(EditEnergies, Me.GetType(), "JSCR", strScript.ToString(), False)
            'Else
            '    '    'DavesCode.Reuse.SetStatus("Engineering Approved", 5, 7, MachineName, 3)
            '    '    Application("Someoneisloggedin") = Nothing
            '    '    Dim strScript As String = "<script>"
            '    '    strScript += "alert('No Pre-clinical RunUp Logging Off');"
            '    '    strScript += "window.location='la1page.aspx';"
            '    '    strScript += "</script>"
            '    '    ScriptManager.RegisterStartupScript(LogOffButton, Me.GetType(), "JSCR", strScript.ToString(), False)
            'End If
        End If


    End Sub
    Protected Sub Faultconfirmed_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles confirmfault.Click
        'have to have the contentplaceholder step because the control is on a main page
        ' - this wouldn't be necessary if this was a user control
        Dim mpContentPlaceHolder As ContentPlaceHolder
        Dim wctrl As WriteDatauc
        Dim wcbutton As Button
        Dim Areastatus As String
        Dim strScript As String = "<script>"
        Dim actionstate As String

        machinename = MachineLabel.Text
        actionstate = "ActionState" + machinename
        Areastatus = DropDownListArea.SelectedItem.Text
        If Areastatus = "Select" Then
            strScript += "alert('Please select an Area');"
            strScript += "</script>"
            ScriptManager.RegisterStartupScript(confirmfault, Me.GetType(), "JSCR", strScript.ToString(), False)
        Else
            mpContentPlaceHolder = _
            CType(Me.Master.FindControl("ContentPlaceHolder1"),  _
            ContentPlaceHolder)
            If Not mpContentPlaceHolder Is Nothing Then
                wctrl = CType(mpContentPlaceHolder.FindControl("Writedataucfr"), WriteDatauc)
                wcbutton = CType(wctrl.FindControl("AcceptOK"), Button)
                wcbutton.Text = "Saving Fault"
                Application(actionstate) = "Confirm"
                wctrl.Visible = True
            End If
        End If

    End Sub

    Protected Sub Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancel.Click
        'This solution came via http://yasserzaid.wordpress.com/2009/01/02/how-to-go-back-to-the-previous-page-in-aspnet/
        'Dim refUrl As Object = ViewState("RefUrl")
        'If (Not (refUrl) Is Nothing) Then
        '    Response.Redirect(CType(refUrl, String))
        'End If
        'Application("Someoneisloggedin") = 1
        Dim LinacID As String = MachineLabel.Text
        failstate = "FailState" + LinacID
        returnclinical = "ClinicalOn" + LinacID
        Dim tab As String
        tab = HiddenField1.Value
        Dim comment As String = DummyCommentBox.Value
        'if the cancel originated from the repair, maintenance or physics tabs then want to remember original tab. Otherwise set to nothing
        If tab = "5" Or tab = "4" Or tab = "6" Then
            'don't alter failstate
        Else
            Application(failstate) = Nothing
        End If

        'Response.Redirect("LA1page.aspx?pageref=Fault&Tabindex=" & tab & "&comment=" & comment)
        If tab = "3" Then
            Application(returnclinical) = 1
        End If
        Dim returnstring As String = LinacID + "page.aspx?pageref=Fault&Tabindex="
        Response.Redirect(returnstring & tab & "&comment=" & comment)
        'Response.Redirect("LA1page.aspx?pageref=Fault")
        'Dim tab As String
        'tab = Request.QueryString("Tabindex").ToString
        'tab = Request.QueryString("tabIndex").ToString
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'from http://blog.davidsz.nl/2012/03/04/previous-page-in-asp-net/
        'Cancel.Attributes.Add("onClick", "javascript:history.back();   return false;")
        'leave  cancel_click empty if you use this


        If Not IsPostBack Then
            ViewState("RefUrl") = Request.UrlReferrer.ToString()
            Dim viewrefurl As String = ViewState("RefUrl")
            machinename = Request.QueryString("val")
            HiddenField1.Value = Request.QueryString("Tabindex")
            Dim commentbox As String = Request.QueryString("commentbox")
            DummyCommentBox.Value = commentbox
            MachineLabel.Text = machinename
            appstate = "LogOn" + machinename
            actionstate = "ActionState" + machinename
            suspstate = "Suspended" + machinename
            failstate = "FailState" + machinename
            repairstate = "rppTab" + machinename
            clinicalstate = "ClinicalOn" + machinename
            treatmentstate = "Treatment" + machinename
            returnclinical = "ReturnClincal" + machinename
            AddEnergyItem()
            
            'Some error handling for if a problem and fault already open
            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings( _
            "connectionstring").ConnectionString
            Dim existingfault As SqlCommand

            Dim reader As SqlDataReader
            conn = New SqlConnection(connectionString)

            existingfault = New SqlCommand("SELECT IncidentID FROM FaultIDTable where Linac = @linac and Status in ('New', 'Open')", conn)
            existingfault.Parameters.AddWithValue("@linac", machinename)
            conn.Open()
            reader = existingfault.ExecuteReader()

            If reader.HasRows() Then
                Dim strScript As String = "<script>"
                strScript += "alert('An open fault already exists');"
                'strScript += "window.location='la3page.aspx';"
                strScript += "</script>"
                ScriptManager.RegisterStartupScript(confirmfault, Me.GetType(), "JSCR", strScript.ToString(), True)
                Dim tab As String = "5"
                Dim comment As String = ""

                'don't want to return saved value of comment here because this sets the fault page. Want empty string
                Dim returnstring As String = machinename + "page.aspx?pageref=Fault&Tabindex="
                Response.Redirect(returnstring & tab & "&comment=" & comment)
            End If
            conn.Close()
        End If
    End Sub
    Protected Sub AddEnergyItem() 'this is also in ViewOpenFaults.ascx.vb
        'from http://www.aspsnippets.com/Articles/Programmatically-add-items-to-DropDownList-on-Button-Click-in-ASPNet-using-C-and-VBNet.aspx
        'and http://www.yaldex.com/asp_tutorial/0596004877_progaspdotnet2-chp-5-sect-7.html

        'This is a mad way of doing it but I don't know how to dim the energy list without constructing it at the same time
        Dim MachineName As String = MachineLabel.Text
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
End Class


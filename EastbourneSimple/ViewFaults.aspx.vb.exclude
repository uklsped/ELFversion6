Imports System.Globalization
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Drawing
Imports System.Data
Partial Class ViewFaults
    Inherits System.Web.UI.Page
    Private machinename As String



    Protected Sub ViewFaultsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewFaultsButton.Click
        'Also have to select linac
        Dim machineSelected As String
        Dim faultType As String
        Dim BeginDate As String
        Dim StopDate As String

        'If RequiredFieldValidatorstart.IsValid And RequiredFieldValidatorstop.IsValid Then
        machineSelected = dropLinac.SelectedValue
        faultType = RadioButtonFault.SelectedValue
        BeginDate = Request.Form(StartDate.UniqueID)
        StopDate = Request.Form(EndDate.UniqueID)


        'Date selection from http://aspsnippets.com/Articles/ASP.Net-AJAX-CalendarExtender---Get-selected-date-from-ReadOnly-TextBox.aspx
        GetFaultSelection(machineSelected, faultType, BeginDate, StopDate)
        'End If
    End Sub
    Protected Sub GetFaultSelection(ByVal machineSelected As String, ByVal faultType As String, ByVal BeginDate As String, ByVal StopDate As String)
        Dim MyCultureInfo As CultureInfo = New CultureInfo("en-GB")
        Dim MachineName As String = machineSelected
        Dim Fault As String = faultType
        Dim StartDate As DateTime
        Dim EndDate As DateTime
        Dim returntable As DataTable = New DataTable()
        ' the checking of the date is very problematic if view button is selected without radiobutton. So decided to control view button
        'this is a magic number

        If Fault > 4 Then
            If Not BeginDate Is Nothing Then
                StartDate = DateTime.Parse(BeginDate, MyCultureInfo)
                StartDate_CalendarExtender.SelectedDate = StartDate
            End If

            If Not StopDate Is Nothing Then
                EndDate = DateTime.Parse(StopDate, MyCultureInfo)
                EndDate_CalendarExtender.SelectedDate = EndDate
                EndDate = EndDate.AddDays(1)
            End If
        End If
        'This resets details tables between activations of view fault buttons
        resetTables()

        'Dim SqlDataSource1 As New SqlDataSource()
        Dim fconc, fclosed As String
        Dim elfconcession As Boolean = False
        Select Case Fault
            Case "1"
                fconc = "concession"
                fclosed = "closed"
            Case "2"
                fconc = "closed"
                fclosed = "closed"
            Case "3"
                fconc = "concession"
                fclosed = "concession"
            Case "4"
                elfconcession = True
            Case Else
        End Select


        returntable = SetGrid(MachineName, Fault, StartDate, EndDate)
        ViewState("FaultsDataTable") = returntable
        GridView1.DataSource = returntable
        'have to set page index before binding data
        GridView1.PageIndex = 0
        GridView1.DataBind()

    End Sub

    Function GetconcessionData(ByVal machineselected As String) As DataTable
        Dim dt As DataTable = New DataTable()
        Dim conn As SqlConnection
        Dim connectionstring As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        Dim querystring As String
        Dim adapter As SqlDataAdapter

        conn = New SqlConnection(connectionstring)
        querystring = "select distinct f.incidentID,  f.Dateinserted, f.DateClosed, f.status, r.description ,c.ConcessionNumber, c.ConcessionDescription,  f.linac " & _
        "from FaultIDTable f left outer join ConcessionTable c on c.ConcessionNumber = f.ConcessionNumber left outer join reportfault r on r.faultid = f.OriginalFaultID where f.linac = @linac and c.ConcessionNumber != '' order by c.concessionnumber"
        adapter = New SqlDataAdapter()
        Dim command As SqlCommand = New SqlCommand(querystring, conn)
        adapter.SelectCommand = command

        command.Parameters.AddWithValue("@linac", SqlDbType.NVarChar).Value = machineselected


        Try
            ' Connect to the database and run the query.
            ' Fill the DataSet.
            adapter.Fill(dt)

        Catch ex As Exception

            ' The connection failed. Display an error message.

        End Try

        Return dt

    End Function

    Protected Sub ExitButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExitButton.Click

        ClientScript.RegisterClientScriptBlock(Page.GetType(), "script", "window.close();", True)

    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            machinename = Request.QueryString("id")
            'If String.IsNullOrEmpty(Request.QueryString("id")) Then
            '    machinename = "1"
            'End If
            dropLinac.Items.FindByValue(machinename).Selected = True
            If Not machinename = "Select" Then
                UpdatePanel2.Visible = True
            End If
        End If
    End Sub

    Sub Listfaults(ByVal IncidentID As String)

        Dim IncidentNumber As String = IncidentID
        Dim conn As SqlConnection
        Dim comm As SqlCommand
        Dim reader As SqlDataReader
        Dim connectionstring As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        conn = New SqlConnection(connectionstring)
        comm = New SqlCommand("Select FaultID, Description, ReportedBy, DateReported, Area, Energy, GantryAngle,CollimatorAngle, BSUHID from ReportFault where IncidentID = @IncidentID order by faultid desc", conn)
        comm.Parameters.Add("IncidentID", System.Data.SqlDbType.Int)
        comm.Parameters.Item("IncidentID").Value = Convert.ToInt16(IncidentNumber)
        Try
            conn.Open()
            reader = comm.ExecuteReader()
            DatalistFaults.DataSource = reader
            DatalistFaults.DataBind()
            reader.Close()
        Finally
            conn.Close()
        End Try

    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Dim IncidentID As String
        Dim selectedRowIndex As Integer
        Dim oldselectedRowIndex As Integer

        selectedRowIndex = GridView1.SelectedIndex
        Dim row As GridViewRow
        If (Session("SelectedIndex") IsNot Nothing) Then
            oldselectedRowIndex = Convert.ToInt32(Session("SelectedIndex"))
            row = GridView1.Rows(oldselectedRowIndex)
            row.BackColor = (Session("selectedcolour"))

        End If
        row = GridView1.Rows(selectedRowIndex)
        IncidentID = row.Cells(0).Text
        Listfaults(IncidentID)
        ListHistory(IncidentID)
        Session("selectedcolour") = row.BackColor
        row.BackColor = ColorTranslator.FromHtml("#A1DCF2")
        Session("selectedindex") = GridView1.SelectedIndex
        GridView1.SelectedIndex = -1
    End Sub

    Sub ListHistory(ByVal IncidentID As String)
        Dim IncidentNumber As String = IncidentID
        Dim conn As SqlConnection
        Dim comm As SqlCommand
        Dim reader As SqlDataReader
        Dim connectionstring As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        conn = New SqlConnection(connectionstring)
        comm = New SqlCommand("select t.TrackingID, t.trackingcomment, t.AssignedTo, t.Status, t.LastUpDatedBy, t.LastUpDatedOn, t.linac, t.action " & _
            "from FaultTracking t  where t.incidentID=@incidentID order by t.trackingid desc", conn)
        comm.Parameters.Add("IncidentID", System.Data.SqlDbType.Int)
        comm.Parameters.Item("IncidentID").Value = Convert.ToInt16(IncidentNumber)
        
        Try
            conn.Open()
            reader = comm.ExecuteReader()
            trackingHistory.DataSource = reader
            trackingHistory.DataBind()
            reader.Close()
        Finally
            conn.Close()
        End Try
    End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        Dim dt As New DataTable
        resetTables()
        dt = ViewState("FaultsDataTable")
        Session("SelectedIndex") = Nothing
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataSource = dt
        GridView1.DataBind()
    End Sub

    Function GetData(ByVal linac As String, ByVal concession As String, closed As String, ByVal StartDate As DateTime, ByVal EndDate As DateTime) As DataTable
        Dim dt As DataTable = New DataTable()
        Dim conn As SqlConnection
        Dim connectionstring As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        Dim querystring As String
        Dim adapter As SqlDataAdapter

        conn = New SqlConnection(connectionstring)
        adapter = New SqlDataAdapter()
        Dim command As SqlCommand

        querystring = "select distinct f.incidentID,  f.Dateinserted, f.DateClosed, f.status, r.description ,c.ConcessionNumber, c.ConcessionDescription,  f.linac " & _
        "from FaultIDTable f left outer join ConcessionTable c on c.ConcessionNumber = f.ConcessionNumber left outer join reportfault r on r.faultid = f.OriginalFaultID where f.linac=@linac and f.Status in (@closed, @concession) and f.dateinserted between @StartDate and @EndDate order by Dateinserted desc"

        command = New SqlCommand(querystring, conn)
        adapter.SelectCommand = command

        command.Parameters.AddWithValue("@linac", SqlDbType.NVarChar).Value = linac
        command.Parameters.AddWithValue("@closed", System.Data.SqlDbType.NVarChar).Value = closed
        command.Parameters.AddWithValue("@concession", System.Data.SqlDbType.NVarChar).Value = concession
        command.Parameters.Add("@StartDate", System.Data.SqlDbType.DateTime).Value = StartDate
        command.Parameters.Add("@EndDate", System.Data.SqlDbType.DateTime).Value = EndDate

        Try
            ' Connect to the database and run the query.
            ' Fill the DataSet.
            adapter.Fill(dt)

        Catch ex As Exception

            ' The connection failed. Display an error message.

        End Try

        Return dt
    End Function

    Function SetGrid(ByVal machineselected As String, ByVal radioselection As Integer, StartDate As DateTime, EndDate As DateTime) As DataTable
        Dim dt As DataTable = New DataTable()
        Dim linac As String = machineselected
        Dim conn As SqlConnection
        Dim connectionstring As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        Dim querystring As String
        Dim adapter As SqlDataAdapter
        Dim closed As String
        Dim concession As String

        conn = New SqlConnection(connectionstring)
        adapter = New SqlDataAdapter()
        Dim command As SqlCommand

        Select Case radioselection
            Case 1, 2, 4
                If radioselection = 1 Then
                    closed = "closed"
                    concession = "concession"
                ElseIf radioselection = 2 Then
                    closed = "closed"
                    concession = "closed"
                Else
                    closed = "concession"
                    concession = "concession"
                End If
                querystring = "select distinct f.incidentID,  f.Dateinserted, f.DateClosed, f.status, r.description ,c.ConcessionNumber, c.ConcessionDescription,  f.linac " & _
                "from FaultIDTable f left outer join ConcessionTable c on c.ConcessionNumber = f.ConcessionNumber left outer join reportfault r on r.faultid = f.OriginalFaultID where f.linac=@linac and f.Status in (@closed, @concession) order by Dateinserted desc"

                command = New SqlCommand(querystring, conn)
                adapter.SelectCommand = command
                command.Parameters.AddWithValue("@linac", SqlDbType.NVarChar).Value = linac
                command.Parameters.AddWithValue("@closed", System.Data.SqlDbType.NVarChar).Value = closed
                command.Parameters.AddWithValue("@concession", System.Data.SqlDbType.NVarChar).Value = concession
            Case 3
                querystring = "select distinct f.incidentID,  f.Dateinserted, f.DateClosed, f.status, r.description ,c.ConcessionNumber, c.ConcessionDescription,  f.linac " & _
                "from FaultIDTable f left outer join ConcessionTable c on c.ConcessionNumber = f.ConcessionNumber left outer join reportfault r on r.faultid = f.OriginalFaultID where f.linac=@linac and c.ConcessionNumber like 'ELF%'  order by Dateinserted desc"
                command = New SqlCommand(querystring, conn)
                adapter.SelectCommand = command
                command.Parameters.AddWithValue("@linac", SqlDbType.NVarChar).Value = linac
            Case 5, 6
               
                If radioselection = 5 Then
                    querystring = "select distinct f.incidentID,  f.Dateinserted, f.DateClosed, f.status, r.description ,c.ConcessionNumber, c.ConcessionDescription,  f.linac " & _
                    "from FaultIDTable f left outer join ConcessionTable c on c.ConcessionNumber = f.ConcessionNumber left outer join reportfault r on r.faultid = f.OriginalFaultID where f.linac=@linac and f.dateinserted between @StartDate and @EndDate order by Dateinserted desc"
                Else
                    querystring = "select distinct f.incidentID,  f.Dateinserted, f.DateClosed, f.status, r.description ,c.ConcessionNumber, c.ConcessionDescription,  f.linac " & _
                    "from FaultIDTable f left outer join ConcessionTable c on c.ConcessionNumber = f.ConcessionNumber left outer join reportfault r on r.faultid = f.OriginalFaultID where f.linac=@linac and f.dateclosed between @StartDate and @EndDate order by Dateinserted desc"
                End If
                command = New SqlCommand(querystring, conn)
                adapter.SelectCommand = command
                command.Parameters.AddWithValue("@linac", SqlDbType.NVarChar).Value = linac
                command.Parameters.Add("@StartDate", System.Data.SqlDbType.DateTime).Value = StartDate
                command.Parameters.Add("@EndDate", System.Data.SqlDbType.DateTime).Value = EndDate
        End Select
        Try
            ' Connect to the database and run the query.
            ' Fill the DataSet.
            adapter.Fill(dt)

        Catch ex As Exception

            ' The connection failed. Display an error message.

        End Try

        Return dt


    End Function
    Sub resetTables()
        Listfaults("0")
        ListHistory("0")
    End Sub

    Protected Sub RadioButtonFault_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RadioButtonFault.SelectedIndexChanged
        Dim time As DateTime
        time = Now()
        Session("SelectedIndex") = Nothing

        If RadioButtonFault.SelectedValue <= 4 Then
            StartDate.Enabled = False
            EndDate.Enabled = False
            RequiredFieldValidatorstart.Enabled = False
            RequiredFieldValidatorstop.Enabled = False
            'StartDate_CalendarExtender.SelectedDate = time
            'EndDate_CalendarExtender.SelectedDate = time
            'StartDate.Text = time
            'EndDate.Text = time
        Else
            StartDate_CalendarExtender.SelectedDate = Nothing
            EndDate_CalendarExtender.SelectedDate = Nothing
            StartDate.Enabled = True
            EndDate.Enabled = True
            RequiredFieldValidatorstart.Enabled = True
            RequiredFieldValidatorstop.Enabled = True
            'StartDate.Text = String.Empty
            'EndDate.Text = String.Empty

        End If

        ViewFaultsButton.Enabled = True

    End Sub

    Protected Sub dropLinac_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles dropLinac.SelectedIndexChanged
        If Not dropLinac.SelectedValue = "Select" Then
            UpdatePanel2.Visible = True
        Else
            UpdatePanel2.Visible = False
        End If
    End Sub
End Class

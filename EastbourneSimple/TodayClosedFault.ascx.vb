Imports System.Data.SqlClient
Imports System.Data
Partial Class TodayClosedFault
    Inherits UserControl

    Public Property LinacName() As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            SetGrid()
        End If
    End Sub

    Public Sub SetGrid()
        Dim today As Date = DateTime.Today
        Dim todayplusone As DateTime = today.AddDays(1)
        Dim dt As DataTable = New DataTable()
        Dim conn As SqlConnection
        Dim connectionstring As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        Dim querystring As String
        Dim adapter As SqlDataAdapter


        conn = New SqlConnection(connectionstring)
        querystring = "select distinct f.incidentID,  f.Dateinserted, f.DateClosed, r.description ,c.ConcessionNumber, c.ConcessionDescription,  f.linac " &
        "from FaultIDTable f left outer join ConcessionTable c on c.ConcessionNumber = f.ConcessionNumber left outer join reportfault r on r.faultid = f.OriginalFaultID where f.linac=@linac and f.dateclosed between @StartDate and @EndDate order by Dateclosed desc"
        adapter = New SqlDataAdapter()
        Dim command As SqlCommand = New SqlCommand(querystring, conn)
        adapter.SelectCommand = command

        command.Parameters.AddWithValue("@linac", SqlDbType.NVarChar).Value = LinacName
        command.Parameters.Add("@StartDate", System.Data.SqlDbType.DateTime).Value = today
        command.Parameters.Add("@EndDate", System.Data.SqlDbType.DateTime).Value = todayplusone

        Try
            ' Connect to the database and run the query.
            ' Fill the DataSet.
            adapter.Fill(dt)


            Dim incidentID As String = ""
            Dim description As String = ""

            If dt.Rows.Count > 0 Then

                For Each dataRow As DataRow In dt.Rows

                    incidentID = dataRow("incidentID")

                    description = dataRow("Description")

                Next
            End If

        Catch ex As Exception

            ' The connection failed. Display an error message.

        End Try

        ViewState("FaultsDataTable") = dt
        GridView1.DataSource = dt
        GridView1.DataBind()
        CheckEmptyGrid(GridView1)
    End Sub
    Public Sub CheckEmptyGrid(ByVal grid As WebControls.GridView)
        If grid.Rows.Count = 0 And Not grid.DataSource Is Nothing Then
            Dim dt As Object = Nothing
            If grid.DataSource.GetType Is GetType(Data.DataSet) Then
                dt = New System.Data.DataSet
                dt = CType(grid.DataSource, System.Data.DataSet).Tables(0).Clone()
            ElseIf grid.DataSource.GetType Is GetType(Data.DataTable) Then
                dt = New System.Data.DataTable
                dt = CType(grid.DataSource, System.Data.DataTable).Clone()
            ElseIf grid.DataSource.GetType Is GetType(Data.DataView) Then
                dt = New System.Data.DataView
                dt = CType(grid.DataSource, System.Data.DataView).Table.Clone
            End If
            dt.Rows.Add(dt.NewRow())
            grid.DataSource = dt
            grid.DataBind()
            Dim columnsCount As Integer
            Dim tCell As New TableCell()
            columnsCount = grid.Columns.Count
            grid.Rows(0).Cells.Clear()
            grid.Rows(0).Cells.Add(tCell)
            grid.Rows(0).Cells(0).ColumnSpan = columnsCount


            grid.Rows(0).Cells(0).Text = "No Records To Display"
            grid.Rows(0).Cells(0).HorizontalAlign = HorizontalAlign.Center
            grid.Rows(0).Cells(0).ForeColor = Drawing.Color.Black
            grid.Rows(0).Cells(0).Font.Bold = True

        End If
    End Sub
    'Sub Listfaults(ByVal IncidentID As String)

    '    'Dim IncidentNumber As String = IncidentID
    '    'Dim conn As SqlConnection
    '    'Dim comm As SqlCommand
    '    'Dim reader As SqlDataReader
    '    'Dim connectionstring As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
    '    'conn = New SqlConnection(connectionstring)
    '    'comm = New SqlCommand("Select FaultID, Description, ReportedBy, DateReported, Area, Energy, GantryAngle,CollimatorAngle, BSUHID from ReportFault where IncidentID = @IncidentID", conn)
    '    'comm.Parameters.Add("IncidentID", System.Data.SqlDbType.Int)
    '    'comm.Parameters.Item("IncidentID").Value = Convert.ToInt16(IncidentNumber)
    '    'Try
    '    '    conn.Open()
    '    '    reader = comm.ExecuteReader()
    '    '    DatalistFaults.DataSource = reader
    '    '    DatalistFaults.DataBind()
    '    '    reader.Close()
    '    'Finally
    '    '    conn.Close()
    '    'End Try

    'End Sub

    'Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles GridView1.SelectedIndexChanged
    '    Dim IncidentID As String
    '    Dim selectedRowIndex As Integer
    '    Dim oldselectedRowIndex As Integer

    '    selectedRowIndex = GridView1.SelectedIndex
    '    Dim row As GridViewRow
    '    If (Session("SelectedIndex") IsNot Nothing) Then
    '        oldselectedRowIndex = Convert.ToInt32(Session("SelectedIndex"))
    '        row = GridView1.Rows(oldselectedRowIndex)
    '        row.BackColor = (Session("selectedcolour"))

    '    End If
    '    row = GridView1.Rows(selectedRowIndex)
    '    IncidentID = row.Cells(0).Text
    '    Listfaults(IncidentID)
    '    ListHistory(IncidentID)
    '    Session("selectedcolour") = row.BackColor
    '    row.BackColor = ColorTranslator.FromHtml("#A1DCF2")
    '    Session("selectedindex") = GridView1.SelectedIndex
    '    GridView1.SelectedIndex = -1
    'End Sub

    'Sub ListHistory(ByVal IncidentID As String)
    '    'Dim IncidentNumber As String = IncidentID
    '    'Dim conn As SqlConnection
    '    'Dim comm As SqlCommand
    '    'Dim reader As SqlDataReader
    '    'Dim connectionstring As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
    '    'conn = New SqlConnection(connectionstring)
    '    'comm = New SqlCommand("select t.TrackingID, t.trackingcomment, t.AssignedTo, t.Status, t.LastUpDatedBy, t.LastUpDatedOn, t.linac, t.action " & _
    '    '    "from FaultTracking t  where t.incidentID=@incidentID", conn)
    '    'comm.Parameters.Add("IncidentID", System.Data.SqlDbType.Int)
    '    'comm.Parameters.Item("IncidentID").Value = Convert.ToInt16(IncidentNumber)

    '    'Try
    '    '    conn.Open()
    '    '    reader = comm.ExecuteReader()
    '    '    trackingHistory.DataSource = reader
    '    '    trackingHistory.DataBind()
    '    '    reader.Close()
    '    'Finally
    '    '    conn.Close()
    '    'End Try
    'End Sub
    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        Dim dt As New DataTable
        'resetTables()
        dt = ViewState("FaultsDataTable")
        Session("SelectedIndex") = Nothing
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataSource = dt
        GridView1.DataBind()
    End Sub

    'Function GetData() As DataTable
    '    Dim dt As DataTable = New DataTable()
    '    Dim conn As SqlConnection
    '    Dim connectionstring As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
    '    Dim querystring As String
    '    Dim adapter As SqlDataAdapter


    '    conn = New SqlConnection(connectionstring)
    '    querystring = "select distinct f.incidentID,  f.Dateinserted, f.DateClosed, f.status, r.description ,c.ConcessionNumber, c.ConcessionDescription,  f.linac " & _
    '    "from FaultIDTable f left outer join ConcessionTable c on c.ConcessionNumber = f.ConcessionNumber left outer join reportfault r on r.faultid = f.OriginalFaultID where f.linac=@linac and f.dateclosed between '2015-09-07' and '2015-09-08' order by Dateclosed desc"
    '    adapter = New SqlDataAdapter()
    '    Dim command As SqlCommand = New SqlCommand(querystring, conn)
    '    adapter.SelectCommand = command

    '    command.Parameters.AddWithValue("@linac", SqlDbType.NVarChar).Value = machinename
    '    'command.Parameters.Add("@StartDate", System.Data.SqlDbType.DateTime).Value = StartDate
    '    'command.Parameters.Add("@EndDate", System.Data.SqlDbType.DateTime).Value = EndDate

    '    Try
    '        ' Connect to the database and run the query.
    '        ' Fill the DataSet.
    '        adapter.Fill(dt)

    '    Catch ex As Exception

    '        ' The connection failed. Display an error message.

    '    End Try

    '    Return dt
    'End Function
    'Sub resetTables()
    '    Listfaults("0")
    '    ListHistory("0")
    'End Sub

End Class



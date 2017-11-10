Imports System.Data
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Data.SqlClient


Partial Class AtlasEnergyViewuc
    Inherits System.Web.UI.UserControl
    Private MachineName As String


    Public Property LinacName() As String
        Get
            Return MachineName
        End Get
        Set(ByVal value As String)
            MachineName = value
        End Set
    End Property

    'Protected Sub ViewAtlasEnergies_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewAtlasEnergies.Click
    '    UpdatePanel3.Visible = True
    '    ViewAtlasEnergies.Visible = False
    'End Sub

    'Protected Sub HideAtlasEnergies_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HideAtlas.Click
    '    UpdatePanel3.Visible = False
    '    ViewAtlasEnergies.Visible = True
    'End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim machine As String
        ' Inserted this extra step because LA3 licence has been re-allocated to E1. 10/7/17
        'Needs to change forf E2 and B1
        If MachineName = "E1" Then
            machine = 3
        Else
            'This next step strips out the machine number because that's what atlas uses beware that the energies database needs the full machine name
            machine = MachineName.Last
        End If

        'machine = 2
        'Dim SqlDataSourceAtlas As New SqlDataSource()
        'SqlDataSourceAtlas.ID = "SqlDataSourceAtlas"
        'SqlDataSourceAtlas.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("AtlasConnectionString").ConnectionString
        'SqlDataSourceAtlas.SelectCommand = "select m.machinename,cast(e.energyvalue as varchar(10)) + ' ' + e.units as Beam,  mt.machinetestname as 'Test Name', b.result, cast(b.created as datetime) as 'Test Date', b.comment, b.accountname from atlas.dbo.[BeamTestResult] b " & _
        '"left outer join machinetemplate mt on b.machinetemplateid=mt.machinetemplateid " & _
        '"left outer join machine m on m.machineid=mt.machineid " & _
        '"left outer join energy e on e.energyid=mt.energyid " & _
        '"where substring((Cast(b.created as char)),1,11) = substring((Cast(getdate() as char)),1,11) " & _
        '"and mt.machineid= @machineid and beamtestresultid= " & _
        '"(SELECT MAX(btr.beamtestresultid) FROM beamtestresult as btr where btr.machinetemplateid=b.machinetemplateid) " & _
        '"order by e.energyid, mt.machinetestname"
        'SqlDataSourceAtlas.SelectParameters.Add("@machineid", System.Data.SqlDbType.NVarChar)
        'SqlDataSourceAtlas.SelectParameters.Add("machineid", machine)
        'GridView4.DataSource = SqlDataSourceAtlas
        'GridView4.DataBind()
        'Added energyid less than 29 becuase imaging modalities have been added
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("AtlasConnectionString").ConnectionString
        Dim con As New SqlConnection(connectionString)
        con.Open()
        Dim cmd As New SqlCommand("select m.machinename,cast(e.energyvalue as varchar(10)) + ' ' + e.units as Beam,  mt.machinetestname as 'Test Name', b.result, cast(b.created as datetime) as 'Test Date', b.comment, b.accountname from atlas.dbo.[BeamTestResult] b " & _
        "left outer join machinetemplate mt on b.machinetemplateid=mt.machinetemplateid " & _
        "left outer join machine m on m.machineid=mt.machineid " & _
        "left outer join energy e on e.energyid=mt.energyid " & _
        "where substring((Cast(b.created as char)),1,11) = substring((Cast(getdate() as char)),1,11) " & _
        "and mt.machineid= @machineid and beamtestresultid= " & _
        "(SELECT MAX(btr.beamtestresultid) FROM beamtestresult as btr where btr.machinetemplateid=b.machinetemplateid) " & _
        "order by e.energyid, mt.machinetestname", con)
        cmd.Parameters.AddWithValue("@machineid", machine)
        Dim da As New SqlDataAdapter(cmd)
        Dim dt As New DataTable()
        da.Fill(dt)
        GridView4.DataSource = dt
        GridView4.DataBind()
        CheckEmptyGrid(GridView4)

        'This sets the current run up gridview1
        'Dim SqlDataSourceRunup As New SqlDataSource()
        'SqlDataSourceRunup.ID = "SqlDataSourceRunup"
        'SqlDataSourceRunup.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        'SqlDataSourceRunup.SelectCommand = "SELECT p.Energy FROM [PhysicsEnergies] p where p.linac= @linac"

        'Dim SqlDataSource1 As New SqlDataSource()
        'Dim query As String = "SELECT * FROM [HandoverEnergies] where handoverid = (Select Max(handoverid) as mancount from [handoverenergies] where linac=@linac)"
        'SqlDataSource1 = QuerySqlConnection(MachineName, query)
        'GridView2.DataSource = SqlDataSource1
        'GridView2.DataBind()

        Dim time As String
        time = Now.ToString("d")



        '"Select * from HandoverEnergies where HandoverID  = (Select max(HandoverID) as lastrecord from HandoverEnergies where linac=@linac)"
        'SqlDataSourceRunup.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        'SqlDataSourceRunup.SelectParameters.Add("linac", MachineName)
        'GridView1.DataSource = SqlDataSourceRunup
        'GridView1.DataBind()



        'Dim SqlDataSourceSet As New SqlDataSource()
        'SqlDataSourceSet.ID = "SqlDataSourceSet"
        'SqlDataSourceSet.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        'SqlDataSourceSet.SelectCommand = "Select DateApproved, MV6, MeV6, MeV8, " & _
        '                          "MeV10, MeV12, MeV15, MeV18, MeV20 from HandoverEnergies where HandoverID  = (Select max(HandoverID) as lastrecord from HandoverEnergies where linac=@linac)"
        'SqlDataSourceSet.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
        'SqlDataSourceSet.SelectParameters.Add("linac", MachineName)
        'Dim dv As DataView
        'Dim energy As Integer
        'dv = CType(SqlDataSourceSet.Select(DataSourceSelectArguments.Empty), DataView)
        'Dim count As Integer = 1
        'Dim DateApproved As String = CType(dv.Table.Rows(0)(0), DateTime).ToShortDateString
        'If DateApproved = time Then
        '    For Each grv As GridViewRow In GridView1.Rows
        '        energy = CType(dv.Table.Rows(0)(count), Integer)
        '        Dim cb As CheckBox = CType(grv.FindControl("RowlevelCheckBox"), CheckBox)
        '        If energy = -1 Then
        '            cb.Checked = True
        '            cb.Enabled = False
        '        Else
        '            cb.Checked = False
        '            cb.Enabled = False
        '        End If
        '        count = count + 1
        '    Next
        'Else
        '    For Each grv As GridViewRow In GridView1.Rows
        '        Dim cb As CheckBox = CType(grv.FindControl("RowlevelCheckBox"), CheckBox)
        '        cb.Checked = False
        '        cb.Enabled = False
        '    Next
        'End If


        'Select Case MachineName
        '    Case "LA1"
        '        GridView1.Rows(1).Visible = False
        '    Case "LA4"
        '        For index As Integer = 2 To 8
        '            GridView1.Rows(index).Visible = False
        '        Next
        '    Case Else
        '        'All columns are valid and are displayed
        'End Select
        'Dim count As Integer = 0
        'Dim cb As CheckBox = CType(GridView1.Rows(count).FindControl("RowLevelCheckBox"), CheckBox)
        'For count = 0 To 8
        '    cb.Enabled = False
        '    cb.Visible = True
        'Next
        'MV6, MV10, MeV6, MeV8, " & _
        '"MeV10, MeV12, MeV15, MeV18, MeV20 
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


            'grid.Rows(0).Visible = False

        End If
    End Sub
End Class

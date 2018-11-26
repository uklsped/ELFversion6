Imports System.Data.SqlClient
Imports System.Data
Imports AjaxControlToolkit
Imports System.Web.UI.Page
Partial Class Emergencyrunupuc
    Inherits System.Web.UI.UserControl
    Private MachineName As String
    Private WriteName As String

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim objConAtlas As UserControl = Page.LoadControl("AtlasEnergyViewuc.ascx")
        CType(objConAtlas, AtlasEnergyViewuc).LinacName = MachineName
        PlaceHolder2.Controls.Add(objConAtlas)

        Dim objCon As UserControl = Page.LoadControl("ViewOpenFaults.ascx")
        CType(objCon, ViewOpenFaults).LinacName = MachineName
        PlaceHolder3.Controls.Add(objCon)

        Dim ControlName As String = WriteName

        If Not IsPostBack Then
            Application("EFaultsee") = 1
            Application("EAtlassee") = 1
            'This sets up the gridview with all of the available energies
            Dim SqlDataSource1 As New SqlDataSource()
            SqlDataSource1.ID = "SqlDataSource1"
            SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            SqlDataSource1.SelectCommand = "SELECT * FROM [PhysicsEnergies] where linac= @linac and EnergyID in (1,2,10,11,19,27,28)"
            SqlDataSource1.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
            SqlDataSource1.SelectParameters.Add("linac", MachineName)
            GridView1.DataSource = SqlDataSource1
            GridView1.DataBind()

            'This makes visible checkboxes for those energies that are approved
            Dim conn As SqlConnection
            Dim comm As SqlCommand
            Dim reader As SqlDataReader
            Dim count As Integer = 0
            Dim connectionString1 As String = ConfigurationManager.ConnectionStrings( _
            "connectionstring").ConnectionString
            conn = New SqlConnection(connectionString1)
            comm = New SqlCommand("SELECT EnergyID, Approved FROM physicsenergies where linac=@linac and EnergyID in (1,2,10,11,19,27,28)", conn)
            comm.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
            comm.Parameters("@linac").Value = MachineName
            Try
                conn.Open()
                reader = comm.ExecuteReader()
                While reader.Read()
                    'This will fall over if approved is null so needs error handling
                    If Not reader.Item("Approved") Then
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

        End If

    End Sub
End Class


Partial Class ManyFaultGriduc
    Inherits System.Web.UI.UserControl
    Private incidentNumber As String
    Private IsNewFault As Boolean
    Private linac As String
    Private newtech As Boolean
    Private technicalstate As String

    Public Event ShowFault(ByVal Incident As String)
    Public Property settech() As Boolean
        Get
            Return newtech
        End Get
        Set(value As Boolean)
            newtech = value
        End Set
    End Property
    Public Property MachineName() As String
        Get
            Return linac
        End Get
        Set(ByVal value As String)
            linac = value
        End Set
    End Property
    Public Property NewFault() As Boolean
        Get
            Return IsNewFault
        End Get
        Set(ByVal value As Boolean)
            IsNewFault = value
        End Set
    End Property
    Public Property IncidentID() As String
        Get
            Return incidentNumber
        End Get
        Set(ByVal value As String)
            incidentNumber = value
        End Set
    End Property

    Protected Sub page_init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        technicalstate = "techstate" + linac

    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        technicalstate = "techstate" + linac
        bindGridViewVEF()
    End Sub
    Public Sub bindGridViewVEF()

        'faultNumber = CInt(GridView3.SelectedDataKey.Values("FaultID"))
        'Dim row As GridViewRow = GridView4.Rows(index)
        Dim SqlDataSource4 As New SqlDataSource()
        Dim state As String
        SqlDataSource4.ID = "SqlDataSource3"
        SqlDataSource4.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        If IsNewFault Then
            SqlDataSource4.SelectCommand = "select IncidentID, Description, ReportedBy,DateReported,Area,Energy,GantryAngle,CollimatorAngle,Linac from ReportFault where incidentID in (select IncidentID from FaultIDTable where linac=@linac and Status in ('New', 'Open')) order by IncidentID desc"
            SqlDataSource4.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
            SqlDataSource4.SelectParameters.Add("linac", linac)
        Else
            SqlDataSource4.SelectCommand = "select IncidentID, Description, ReportedBy,DateReported,Area,Energy,GantryAngle,CollimatorAngle,Linac from ReportFault where incidentID = @IncidentID order by IncidentID desc"
            SqlDataSource4.SelectParameters.Add("@incidentID", System.Data.SqlDbType.NVarChar)
            SqlDataSource4.SelectParameters.Add("incidentID", incidentNumber)
        End If
        GridView4.DataSource = SqlDataSource4
        GridView4.DataBind()

        If IsNewFault Then
            If Application(technicalstate) = True Then
                GridView4.Columns(9).Visible = False
            Else
                GridView4.Columns(9).Visible = True
            End If

        End If


    End Sub




    Sub NewFaultGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        Dim IncidentID As String
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim row As GridViewRow = GridView4.Rows(index)


        IncidentID = Server.HtmlDecode(row.Cells(0).Text)

        ' If multiple buttons are used in a GridView control, use the
        ' CommandName property to determine which button was clicked.
        Select Case e.CommandName
            Case "View"
                'RaiseEvent ShowFault(incidentNumber)
                'added 05April2016
                Application(technicalstate) = Nothing
                RaiseEvent ShowFault(IncidentID)
        End Select

    End Sub


End Class

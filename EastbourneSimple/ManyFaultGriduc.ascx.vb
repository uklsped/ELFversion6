
Partial Class ManyFaultGriduc
    Inherits System.Web.UI.UserControl

    Private technicalstate As String

    Public Event ShowFault(ByVal Incident As String)
    Public Property Settech() As Boolean
    Public Property MachineName() As String
    Public Property NewFault() As Boolean
    Public Property IncidentID() As String

    Protected Sub Page_init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        technicalstate = "techstate" + MachineName

    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        technicalstate = "techstate" + MachineName
        BindGridViewManyFault()
        If MachineName Like "T#" Then
            MultiView1.SetActiveView(Tomo)
        Else
            MultiView1.SetActiveView(Linacs)
        End If

    End Sub
    Public Sub BindGridViewManyFault()
        If MachineName Like "T#" Then
            BindGridViewTomoVEF()
        Else
            BindGridViewVEF()
        End If
    End Sub
    Protected Sub BindGridViewVEF()

        Dim SqlDataSource4 As New SqlDataSource With {
            .ID = "SqlDataSource3",
            .ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        }
        If NewFault Then
            SqlDataSource4.SelectCommand = "select IncidentID, FaultID, Description, ReportedBy,DateReported,Area,Energy,GantryAngle,CollimatorAngle,Linac from ReportFault where incidentID in (select IncidentID from FaultIDTable where linac=@linac and Status in ('New', 'Open')) order by IncidentID desc"
            SqlDataSource4.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
            SqlDataSource4.SelectParameters.Add("linac", MachineName)
        Else
            SqlDataSource4.SelectCommand = "select IncidentID, FaultID, Description, ReportedBy,DateReported,Area,Energy,GantryAngle,CollimatorAngle,Linac from ReportFault where incidentID = @IncidentID order by DateReported desc"
            SqlDataSource4.SelectParameters.Add("@incidentID", System.Data.SqlDbType.NVarChar)
            SqlDataSource4.SelectParameters.Add("incidentID", IncidentID)
        End If
        GridView4.DataSource = SqlDataSource4
        GridView4.DataBind()

        If NewFault Then
            If Application(technicalstate) = True Then
                GridView4.Columns(10).Visible = False
            Else
                GridView4.Columns(10).Visible = True
            End If

        End If


    End Sub

    Protected Sub BindGridViewTomoVEF()

        Dim SqlDataSource4 As New SqlDataSource With {
            .ID = "SqlDataSource3",
            .ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        }
        If NewFault Then
            SqlDataSource4.SelectCommand = "select IncidentID, FaultID, Description, ReportedBy,DateReported,Area,Energy, Linac from ReportFault where incidentID in (select IncidentID from FaultIDTable where linac=@linac and Status in ('New', 'Open')) order by IncidentID desc"
            SqlDataSource4.SelectParameters.Add("@linac", System.Data.SqlDbType.NVarChar)
            SqlDataSource4.SelectParameters.Add("linac", MachineName)
        Else
            SqlDataSource4.SelectCommand = "select IncidentID, FaultID, Description, ReportedBy,DateReported,Area,Energy, Linac from ReportFault where incidentID = @IncidentID order by DateReported desc"
            SqlDataSource4.SelectParameters.Add("@incidentID", System.Data.SqlDbType.NVarChar)
            SqlDataSource4.SelectParameters.Add("incidentID", IncidentID)
        End If
        GridViewTomo.DataSource = SqlDataSource4
        GridViewTomo.DataBind()

        If NewFault Then
            If Application(technicalstate) = True Then
                GridViewTomo.Columns(8).Visible = False
            Else
                GridViewTomo.Columns(8).Visible = True
            End If

        End If


    End Sub


    Sub NewFaultGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        Dim IncidentID As String
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim row As GridViewRow
        If MachineName Like "T#" Then
            row = GridViewTomo.Rows(index)
        Else
            row = GridView4.Rows(index)
        End If

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

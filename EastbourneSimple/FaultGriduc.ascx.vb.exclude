
Partial Class FaultGriduc
    Inherits System.Web.UI.UserControl
    Private incidentNumber As String
    Private IsNewFault As Boolean
    Public Event ShowFault(ByVal Incident As String)
    Public Property Incident() As String
        Get
            Return incidentNumber
        End Get
        Set(ByVal value As String)
            incidentNumber = value
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


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        bindGridViewVEF()
    End Sub
    Protected Sub bindGridViewVEF()

        'faultNumber = CInt(GridView3.SelectedDataKey.Values("FaultID"))
        'Dim row As GridViewRow = GridView4.Rows(index)
        Dim SqlDataSource4 As New SqlDataSource()
        SqlDataSource4.ID = "SqlDataSource3"
        SqlDataSource4.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        SqlDataSource4.SelectCommand = "select FaultID, Description, ReportedBy,DateReported,Area,Energy,GantryAngle,CollimatorAngle,Linac from ReportFault where incidentID=@incidentID order by FaultID desc"
        SqlDataSource4.SelectParameters.Add("@incidentID", System.Data.SqlDbType.Int)
        SqlDataSource4.SelectParameters.Add("incidentID", incidentNumber)
        GridView4.DataSource = SqlDataSource4
        GridView4.DataBind()
        If IsNewFault Then
            GridView4.Columns(9).Visible = True
        End If


    End Sub

    


    Sub NewFaultGridView_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        Dim FaultID As String
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim row As GridViewRow = GridView4.Rows(index)


        FaultID = Server.HtmlDecode(row.Cells(0).Text)

        ' If multiple buttons are used in a GridView control, use the
        ' CommandName property to determine which button was clicked.
        Select Case e.CommandName
            Case "View"
                RaiseEvent ShowFault(incidentNumber)
        End Select

    End Sub


End Class

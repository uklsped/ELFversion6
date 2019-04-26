
Partial Class controls_ConcessionHistoryuc
    Inherits System.Web.UI.UserControl

    Public Sub BindConcessionHistoryGrid(ByVal incidentID As String)
        Dim SqlDataSource1 As New SqlDataSource With {
            .ID = "SqlDataSource1",
            .ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString,
            .SelectCommand = "select t.TrackingID, t.action, t.trackingcomment, t.AssignedTo, t.Status, t.LastUpDatedBy, t.LastUpDatedOn  " _
        & "from FaultTracking t  where t.incidentID=@incidentID order by t.TrackingID desc"
        }

        SqlDataSource1.SelectParameters.Add("@incidentID", System.Data.SqlDbType.Int)
        SqlDataSource1.SelectParameters.Add("incidentID", incidentID)
        ConcessionHistoryGridView.DataSource = SqlDataSource1
        ConcessionHistoryGridView.DataBind()
    End Sub
End Class

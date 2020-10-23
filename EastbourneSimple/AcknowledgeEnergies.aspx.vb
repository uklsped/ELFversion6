
Partial Class AcknowledgeEnergies
    Inherits System.Web.UI.Page
    Private MachineName As String = "LA3"

    Protected Sub AcceptOK_Click(sender As Object, e As System.EventArgs) Handles AcceptOK.Click

        Dim returnstring As String = MachineName + "page.aspx?tabref=3"
        Response.Redirect(returnstring)
    End Sub
End Class

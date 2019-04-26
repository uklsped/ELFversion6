
Partial Class testpopup
    Inherits System.Web.UI.Page
    Protected Sub ButtonShow_Click(ByVal sender As Object, ByVal e As EventArgs)

        'Show ModalPopup               

        mpeThePopup.Show()
    End Sub


    Protected Sub ButtonClose_Click(ByVal sender As Object, ByVal e As EventArgs)

        'Hide ModalPopup

        mpeThePopup.Hide()
    End Sub

End Class

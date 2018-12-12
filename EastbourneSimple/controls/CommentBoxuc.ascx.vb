
Partial Class controls_CommentBoxuc
    Inherits UserControl
    Public Property Currentcomment() As String
    'Public Property LinacName() As String
    Public Property BoxChanged() As String
    Const MAXCHARCOUNT = 250


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If (Not HttpContext.Current.Application(BoxChanged) Is Nothing) Then
                Currentcomment = HttpContext.Current.Application(BoxChanged).ToString
                CommentBox.Text = Currentcomment
                'CommentWordCount.Text = MAXCHARCOUNT - Currentcomment.Length
            End If
        End If
        Currentcomment = CommentBox.Text
        CommentWordCount.Text = MAXCHARCOUNT - Currentcomment.Length
    End Sub

    Protected Sub CommentBox_TextChanged(sender As Object, e As EventArgs) Handles CommentBox.TextChanged
        Currentcomment = CommentBox.Text
        Application(BoxChanged) = Currentcomment
        'DavesCode.Reuse.ReturnApplicationState(BoxChanged)
    End Sub
    Public Sub ResetCommentBox()
        Currentcomment = String.Empty
        Application(BoxChanged) = Currentcomment
        CommentBox.Text = Currentcomment
        CommentWordCount.Text = MAXCHARCOUNT - Currentcomment.Length

    End Sub
End Class

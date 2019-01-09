
Partial Class controls_CommentBoxuc
    Inherits UserControl
    Public Property Currentcomment() As String
    Public Property BoxChanged() As String

    Const MAXCHARCOUNT = 250

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        TextBox.Width = Unit.Pixel(355)
        TextBox.Height = Unit.Pixel(120)
        'https://docs.microsoft.com/en-us/dotnet/api/system.web.ui.clientscriptmanager?view=netframework-4.7.2
        'https://www.codeproject.com/Questions/125501/asp-net-object-Attributes-add-Method
        TextBox.Attributes.Add("onKeyUp", String.Format("Count(""{0}"", ""{1}"")", TextBox.ClientID, CommentWordCount.ClientID))

        Dim csname2 As String = "TextBoxScript"
        Dim cstype As Type = Me.GetType()
        'Get a ClientScriptManager reference from the Page class.
        Dim cs As ClientScriptManager = Page.ClientScript
        If Not cs.IsClientScriptBlockRegistered(cstype, csname2) Then

            Dim cstext2 As StringBuilder = New StringBuilder()
            cstext2.Append("function Count(TextID, CountID) {")
            cstext2.Append("var txt = document.getElementById(TextID);")
            cstext2.Append("var i = txt.value.length;")
            cstext2.Append("if (i < 251) {document.getElementById(CountID).innerHTML = 250 - i;} ")
            cstext2.Append("else {document.getElementById(CountID).innerHTML = ""You have exceeded the max text please delete some characters"";} ")
            cstext2.Append("return;}")
            cs.RegisterClientScriptBlock(cstype, csname2, cstext2.ToString(), True)
        End If
        If Not IsPostBack Then

            If Not HttpContext.Current.Application(BoxChanged) Is Nothing Then
                Currentcomment = HttpContext.Current.Application(BoxChanged).ToString
                TextBox.Text = Currentcomment

            End If
        End If
        Currentcomment = TextBox.Text
        CommentWordCount.Text = MAXCHARCOUNT - Currentcomment.Length
    End Sub

    Protected Sub TextBox_TextChanged(sender As Object, e As EventArgs) Handles TextBox.TextChanged
        Currentcomment = TextBox.Text
        Application(BoxChanged) = Currentcomment
        'DavesCode.Reuse.ReturnApplicationState(BoxChanged)
    End Sub
    Public Sub ResetCommentBox()
        Currentcomment = String.Empty
        Application(BoxChanged) = Nothing
        TextBox.Text = Currentcomment
        CommentWordCount.Text = MAXCHARCOUNT - Currentcomment.Length

    End Sub
    Public Sub SetValidation(ByVal ValidateGroup As String, ByVal ErrorMessage As String)
        CommentValidation.ValidationGroup = ValidateGroup
        CommentValidation.ErrorMessage = ErrorMessage
    End Sub

    Public Sub SetReadOnly(ByVal ReadOnlyValue As Boolean)
        TextBox.ReadOnly = ReadOnlyValue
    End Sub

End Class

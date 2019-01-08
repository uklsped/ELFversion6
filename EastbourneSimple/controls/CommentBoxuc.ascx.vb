
Partial Class controls_CommentBoxuc
    Inherits UserControl
    Public Property Currentcomment() As String
    Public Property BoxChanged() As String
    'Public Property ValidateGroup As String
    'Public Property ErrorMessage As String
    Const MAXCHARCOUNT = 250

    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        'Dim csname2 As String = "ButtonClickScript"
        'Dim cstype As Type = Me.GetType()
        ''Get a ClientScriptManager reference from the Page class.
        'Dim cs As ClientScriptManager = Page.ClientScript
        'If Not cs.IsClientScriptBlockRegistered(cstype, csname2) Then

        '    Dim cstext2 As StringBuilder = New StringBuilder()
        '    cstext2.Append("function Count() {alert(""Fuck Off""); return} ")
        '    'cstext2.Append("<script type=\"text/javascript\"> function DoClick() {")
        '    'cstext2.Append(" </")
        '    'cstext2.Append("script>")
        '    cs.RegisterClientScriptBlock(cstype, csname2, cstext2.ToString(), True)
        'End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        TextBox.Width = Unit.Pixel(355)
        TextBox.Height = Unit.Pixel(120)
        'Dim csname2 As String = "ButtonClickScript"
        'Dim cstype As Type = Me.GetType()
        ''Get a ClientScriptManager reference from the Page class.
        'Dim cs As ClientScriptManager = Page.ClientScript
        'If Not cs.IsClientScriptBlockRegistered(cstype, csname2) Then

        '    Dim cstext2 As StringBuilder = New StringBuilder()
        '    cstext2.Append("function Count() {Alert(""Fuck Off""}; Return ")
        '    'cstext2.Append("<script type=\"text/javascript\"> function DoClick() {")
        '    'cstext2.Append(" </")
        '    'cstext2.Append("script>")
        '    cs.RegisterClientScriptBlock(cstype, csname2, cstext2.ToString(), True)
        'End If




        ' CommentWordCount.Text = TextBox.MaxLength.ToString()

        ' TextBox.Attributes.Add("onKeyUp",
        '"javascript:document.getElementById('" + CommentWordCount.ClientID +
        '"').setAttribute('value', (" + TextBox.MaxLength +
        '" - parseInt(document.getElementById('" + TextBox.ClientID +
        '"').getAttribute('value').length)));")




        'Dim cs As ClientScriptManager
        'cs = Page.ClientScript
        'Dim csname2 As String = "JSCR"
        'Dim cstype As Type = Me.GetType()
        'Dim cstext2 As StringBuilder = New StringBuilder()
        'Dim strScript As String = "<script>"
        'strScript += "function Count() {}"
        'strScript += "</script>"
        'cstext2.Append("<script type=\'text/javascript\'> function Count() {} ")
        ''cstext2.Append("Form1.Message.value='Text from client script.'} </")
        'cstext2.Append("/script>")
        'cs.RegisterClientScriptBlock(cstype, csname2, strScript.ToString(), True)

        'ClientScriptManager.clientscript
        'ScriptManager.RegisterStartupScript(Page, Me.GetType(), "JSCR", "Function Count()", True)
        'Dim scriptString As String = "<script type='text/javascript'>Function Count(){Alert('What the fuck')}</script>"
        'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ClientScriptFunction", scriptString)
        'TextBox.Attributes.Add("onKeyUp", scriptString10
        TextBox.Attributes.Add("onKeyUp", String.Format("Count(""{0}"", ""{1}"")", TextBox.ClientID, CommentWordCount.ClientID))

        Dim csname2 As String = "ButtonClickScript"
        Dim cstype As Type = Me.GetType()
        'Get a ClientScriptManager reference from the Page class.
        Dim cs As ClientScriptManager = Page.ClientScript
        If Not cs.IsClientScriptBlockRegistered(cstype, csname2) Then

            Dim cstext2 As StringBuilder = New StringBuilder()
            cstext2.Append("function Count(TextID, CountID) {")
            cstext2.Append("var txt = document.getElementById(TextID);")
            cstext2.Append("var i = txt.value.length;")
            cstext2.Append("if (i < 251); {document.getElementById(CountID).innerHTML = 250 - i;}")
            'cstext2.Append("Else document.getElementById(""<%=CommentWordCount.ClientID%>').innerHTML = ""You have exceeded the max text please delete some characters"";)")
            cstext2.Append("return;}")
            cs.RegisterClientScriptBlock(cstype, csname2, cstext2.ToString(), True)
        End If
        If Not IsPostBack Then

            If Not HttpContext.Current.Application(BoxChanged) Is Nothing Then
                Currentcomment = HttpContext.Current.Application(BoxChanged).ToString
                TextBox.Text = Currentcomment

                'CommentWordCount.Text = MAXCHARCOUNT - Currentcomment.Length
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

End Class

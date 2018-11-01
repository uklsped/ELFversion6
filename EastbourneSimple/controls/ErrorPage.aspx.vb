
Partial Class controls_ErrorPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If (Not IsPostBack) Then

                ' 2. Grab the exception information from the session
                Dim errorMsg As String =
                CStr(Session("ErrorMsg"))
                Dim pageErrorOccured As String =
                CStr(Session("PageErrorOccured"))
                Dim exceptionType As String =
                CStr(Session("ExceptionType"))
                Dim stackTrace As String =
                CStr(Session("StackTrace"))

                ' 3. Clear the values from the session
                Session("ErrorMsg") = Nothing
                Session("PageErrorOccured") = Nothing
                Session("ExceptionType") = Nothing
                Session("StackTrace") = Nothing

                ' 4. Display a generic error message to the user
                lblMessage.Text = "We're sorry, but an unhandled " &
                "error has occurred.<br/><br/>"

                lblMessage.Text =
                String.Format("{0}To try again, " &
                "click <a href='{1}' class='linkgreen'> here</a>.<br/><br/>",
                lblMessage.Text, pageErrorOccured)

                ' 5. Add specific error information as 
                'HTML comments for you
                ' to view during development. 
                'You could also log the error to 
                ' the Windows event log here.
                lblMessage.Text = lblMessage.Text & "<!--" & Chr(10) &
                "Error Message: " & errorMsg & Chr(10) &
                "Page Error Occurred: " & pageErrorOccured & Chr(10) &
                "ExceptionType: " & exceptionType & Chr(10) &
                "Stack Trace: " & stackTrace & Chr(10) &
                "-->"

            End If
        Catch ex As Exception
            ' If an exception is thrown in the 
            ' above code output the message
            ' and stack trace to the screen
            lblMessage.Text = ex.Message & " " & ex.StackTrace
        End Try
    End Sub
End Class

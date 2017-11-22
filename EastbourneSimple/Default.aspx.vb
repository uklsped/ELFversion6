Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Net.Mail


Partial Class _Default
    Inherits System.Web.UI.Page




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim userIP As String = Request.UserHostAddress
        'Select Case userIP
        '    Case "127.0.0.1"
        '        Response.Redirect("LA1page.aspx")
        '    Case Else
        '        Response.Redirect("LA4page.aspx")
        'End Select
        Application("Lpage") = 0
        'If String.IsNullOrEmpty(Request.QueryString("key")) Then
        '    Response.Redirect("NonMachinepage.aspx")
        '    Application("LA1loaded") = True
        '    Dim view As Boolean = CBool(Application("LA1loaded"))
        '    Response.Redirect("LA1page.aspx?refkey=LA1")

        'Else
        If String.IsNullOrEmpty(Request.QueryString("machinekey")) Then
            'Application("LA1loaded") = True
            Response.Redirect("NonMachinepage.aspx")
        Else
            Dim machinepage As String = Request.QueryString("machinekey")
            Dim returnstring As String = machinepage + "page.aspx?loadup=1"
            'Dim loadedstring As String = machinepage + "loaded"
            'Dim answer As Integer = CInt(Application(loadedstring))
            'If CInt(Application(loadedstring)) <> 1 Then
            Select Case machinepage
                Case "LA1", "LA2", "LA3", "LA4", "E1", "E2", "B1"
                    'Application(loadedstring) = 1

                    'Dim smtpClient As SmtpClient = New SmtpClient()
                    'Dim message As MailMessage = New MailMessage()

                    'Dim fromAddress As New MailAddress("VISIRSERVER@VISIRSERVER.bsuh.nhs.uk", "ELF")
                    'Dim toAddress As New MailAddress("david.spendley@bsuh.nhs.uk")
                    'message.From = fromAddress
                    'message.To.Add(toAddress)
                    'message.Subject = "ELF IP address"
                    'message.Body = userIP + " " + machinepage
                    'smtpClient.Host = "10.216.8.19"
                    'smtpClient.Send(message)
                    Response.Redirect(returnstring)
                Case Else
                    Response.Redirect("NonMachinepage.aspx")
            End Select
            '    Else
            '    'Application(loadedstring) = 0
            '    Response.Redirect("NonMachinepage.aspx")
            'End If
        End If
        'Dim Tabloaded As Integer
        'Dim userIP As String = DavesCode.Reuse.GetIPAddress()
        'Response.Redirect("NonMachinepage.aspx")

        'Select Case userIP
        '    'Case "10.179.84.161"
        '    'Case "10.179.83.80"
        '    'Case "10.179.82.179"
        '    'Response.Redirect("LA1page.aspx")
        '    'Response.Redirect("LA2page.aspx")
        '    'Case "10.179.82.146"
        '    'Response.Redirect("Nonmachinepage.aspx")
        '    'Response.Redirect("LA3page.aspx")
        '    'Case "10.179.82.117"
        '    'Case "10.179.85.164"
        '    '    Response.Redirect("LA3page.aspx")
        '    '    'Case "10.179.82.184"
        '    '    'Response.Redirect("LA4page.aspx")
        '    'Case Else
        '    '    'Response.Redirect("LA3page.aspx")
        '    '    'Response.Redirect("Nonmachinepage.aspx")
        '    '    Tabloaded = DavesCode.Reuse.BrowserLoaded("LA2")
        '    '    If Tabloaded = False Then
        '    '        DavesCode.Reuse.ToggleBrowser("LA2", True)
        '    '        Response.Redirect("LA2page.aspx")
        '    '    Else
        '    '        Response.Redirect("LA3page.aspx")
        '    '    End If
        'End Select

    End Sub

End Class

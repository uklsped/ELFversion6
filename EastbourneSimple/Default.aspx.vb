Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Net.Mail


Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim userIP As String = Request.UserHostAddress

        If String.IsNullOrEmpty(Request.QueryString("machinekey")) Then

            Response.Redirect("NonMachinepage.aspx")
        Else
            Dim machinepage As String = Request.QueryString("machinekey")
            Dim returnstring As String = machinepage + "page.aspx?loadup=1"
            Dim loadedstring As String = machinepage + "loaded"

            Select Case machinepage
                Case "LA1", "LA2", "LA3", "LA4", "E1", "E2", "B1"

                    Response.Redirect(returnstring)
                Case Else
                    Response.Redirect("NonMachinepage.aspx")
            End Select

        End If

    End Sub

End Class

﻿
Partial Class Startpage
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Response.Redirect("default.aspx?machinekey=LA4")
        'Response.Redirect("default.aspx?machinekey=T1")
        Response.Redirect("default.aspx?machinekey=E2")
    End Sub

End Class

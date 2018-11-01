
Partial Class test_repeatfault
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Dim objcon As Object
        'objcon = Page.LoadControl("~/controls/DeviceRepeatFaultuc.ascx")
        'CType(objcon, controls_DeviceRepeatFaultuc).IncidentID = 1459
        'CType(objcon, controls_DeviceRepeatFaultuc).Device = "LA1"

        'PlaceHolderRepeatFault.Controls.Add(objcon)
        Dim objcon As Object
        objcon = Page.LoadControl("~/controls/ErrorTextuc.ascx")


        PlaceHolderRepeatFault.Controls.Add(objcon)
    End Sub
End Class

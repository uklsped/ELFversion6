Imports System.Data.SqlClient
Imports System.Configuration
Partial Class LinacStatusuc
    Inherits System.Web.UI.UserControl
    Private MachineName As String
    Private appstate As String
    Private actionstate As String
    Private suspstate As String
    Private repairstate As String
    Private failstate As String
    Private clinicalstate As String
    Private treatmentstate As String
    Private technicalstate As String
    Private Objfault As ViewFaultsuc
    Public Event Resetstatus()
    Private RegistrationState As String
    Private tabstate As String



    Public Property LinacName() As String
        Get
            Return MachineName
        End Get
        Set(ByVal value As String)
            MachineName = value
        End Set
    End Property
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        AddHandler WriteDatauc1.UserApproved, AddressOf UserApprovedEvent
        AddHandler RegisterUseruc1.ReloadTab, AddressOf PageLoad
        'Added handler so viewfaultsuc1 is reset to cope with nonmachinepage
        AddHandler ViewFaultsuc1.ReloadFaultsTab, AddressOf PageLoad
       
        appstate = "LogOn" + MachineName
        suspstate = "Suspended" + MachineName
        actionstate = "ActionState" + MachineName
        failstate = "FailState" + MachineName
        clinicalstate = "ClinicalOn" + MachineName
        repairstate = "rppTab" + MachineName
        treatmentstate = "Treatment" + MachineName
        technicalstate = "techstate" + MachineName
        RegistrationState = "regstate" + MachineName
        tabstate = "ActTab" + MachineName

    End Sub
    Protected Sub page_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim button As Button = CType(FindControl("Reset"), Button)
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        wctrl.LinacName = MachineName
        Dim fltctrl As ViewFaultsuc = CType(FindControl("ViewFaultsuc1"), ViewFaultsuc)
        fltctrl.LinacName = MachineName
        WaitButtons()
        'BindStatusData()
        'Dim userIP As String = Request.UserHostAddress
        'Dim userIP As String = DavesCode.Reuse.GetIPAddress()
        'TextBox1.Text = userIP
        Dim adminctrl As Administrationuc = CType(FindControl("Administrationuc1"), Administrationuc)
        adminctrl.LinacName = MachineName
        'PlaceHolder2.Controls.Add(adminctrl)
        'If Not IsPostBack Then
        '    Dim objreg = Page.LoadControl("RegisterUseruc.ascx")
        '    PlaceHolder2.Controls.Add(objreg)
        '    'Dim regctrl As RegisterUseruc CType(FindControl("R
        '    'AddHandler objreg.ResetTab, AddressOf PageLoad
        'End If
        Dim value As Boolean
        value = HiddenField2.Value
        If MachineName = "nonmachine" Then
            button.Visible = False
        End If


    End Sub
    Protected Sub PageLoad()
        Dim returnstring As String
        Application("RegistrationState") = True
        HiddenField2.Value = True
        Dim tab As String = Application(tabstate)
        If Application(appstate) = 1 Then
            returnstring = MachineName + "page.aspx?tabclicked=" + tab
        Else
            returnstring = MachineName + "Page.aspx"
        End If

        Response.Redirect(returnstring)

        'Dim button As Button = CType(FindControl("Reset"), Button)
        'Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        'wctrl.LinacName = MachineName
        'Dim fltctrl As ViewFaultsuc = CType(FindControl("ViewFaultsuc1"), ViewFaultsuc)
        'fltctrl.LinacName = MachineName
        'WaitButtons()
        ''BindStatusData()
        ''Dim userIP As String = Request.UserHostAddress
        ''Dim userIP As String = DavesCode.Reuse.GetIPAddress()
        ''TextBox1.Text = userIP
        'Dim adminctrl As Administrationuc = CType(FindControl("Administrationuc1"), Administrationuc)
        'adminctrl.LinacName = MachineName
        ''PlaceHolder2.Controls.Add(adminctrl)
        'If Not IsPostBack Then
        '    Dim objreg = Page.LoadControl("RegisterUseruc.ascx")
        '    PlaceHolder2.Controls.Add(objreg)
        'End If
        'Dim Multiviewlist As MultiView

        'If Not Me.Parent.FindControl("Multiview1") Is Nothing Then
        '    Multiviewlist = Me.Parent.FindControl("Multiview1")
        '    Multiviewlist.ActiveViewIndex = 0
        'End If
        'If MachineName = "nonmachine" Then
        '    button.Visible = False
        'End If
    End Sub
   
    Protected Sub UserApprovedEvent(ByVal Tabused As String, ByVal Userinfo As String)
        Dim tab As String = Tabused
        Dim reader As SqlDataReader
        Dim Status As String = ""
        Dim conn As SqlConnection
        Dim conActivity As SqlCommand
        Dim connectionString As String = ConfigurationManager.ConnectionStrings( _
        "connectionstring").ConnectionString
        Dim returnstring As String

        If tab = "0" Then
            conn = New SqlConnection(connectionString)

            conActivity = New SqlCommand("SELECT state FROM [LinacStatus] where stateID = (Select max(stateID) as lastrecord from [LinacStatus] where linac=@linac)", conn)

            conActivity.Parameters.AddWithValue("@linac", MachineName)
            conn.Open()
            reader = conActivity.ExecuteReader()

            If reader.Read() Then
                Status = reader.Item("State")
            End If
            reader.Close()
            conn.Close()

            If Status = "Fault" Then
                DavesCode.Reuse.SetStatus(Userinfo, "Fault", 5, 5, MachineName, 5)
                returnstring = MachineName + "page.aspx?pageref=Fault&Tabindex=0&comment=Nothing"
            Else
                DavesCode.Reuse.SetStatus(Userinfo, "Linac Unauthorised", 5, 7, MachineName, 0)
                returnstring = MachineName + "page.aspx"
            End If
            Application(suspstate) = Nothing
            Application(appstate) = Nothing
            Application(failstate) = Nothing
            Application(clinicalstate) = Nothing
            Application(repairstate) = Nothing
            Application(treatmentstate) = "Yes"
            Application(technicalstate) = Nothing
            Response.Redirect(returnstring)
           
            Response.Redirect(returnstring)

        End If
    End Sub

    Protected Sub Reset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Reset.Click
        Dim wctrl As WriteDatauc = CType(FindControl("Writedatauc1"), WriteDatauc)
        Dim wcbutton As Button = CType(wctrl.FindControl("AcceptOK"), Button)
        Dim wctext As TextBox = CType(wctrl.FindControl("txtchkUserName"), TextBox)
        wcbutton.Text = "Confirm Reset"
        Application(actionstate) = "Confirm"
        WriteDatauc1.Visible = True
        ForceFocus(wctext)

    End Sub

    Private Sub ForceFocus(ByVal ctrl As Control)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "FocusScript", "setTimeout(function(){$get('" + _
        ctrl.ClientID + "').focus();}, 100);", True)
    End Sub

    'Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
    '    MultiView1.ActiveViewIndex = DropDownList1.SelectedValue
    'End Sub
    Sub Button1_Click(s As Object, e As EventArgs)
        Dim faultcon As ViewFaultsuc
        faultcon = MultiView1.FindControl("ViewFaultsuc1")
        faultcon.Update()
        HiddenField1.Value = False
        MultiView1.Visible = True
        MultiView1.SetActiveView(View1)

    End Sub

    Sub Button2_Click(s As Object, e As EventArgs)
        Button1.Visible = False
        Button2.Visible = False
        Button3.Visible = False
        HiddenField1.Value = False
        MultiView1.Visible = True
        MultiView1.SetActiveView(View2)
        'Dim objreg = Page.LoadControl("RegisterUseruc.ascx")
        'PlaceHolder2.Controls.Add(objreg)
    End Sub

    Sub Button3_Click(s As Object, e As EventArgs)
        'MultiView1.SetActiveView(View3)
        HiddenField1.Value = True
        MultiView1.Visible = True
        MultiView1.SetActiveView(View3)
    End Sub
    Sub WaitButtons()
        Dim B1 As Button = FindControl("Button1")
        Dim B2 As Button = FindControl("Button2")
        Dim B3 As Button = FindControl("Button3")

        If Not B1 Is Nothing Then
            B1.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(B1, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
        End If
        If Not B2 Is Nothing Then
            B2.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(B2, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
        End If
        If Not B3 Is Nothing Then
            B3.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(B3, "") + ";this.value='Wait...';this.disabled = true; this.style.display='block';")
        End If
    End Sub

End Class

Imports System.Data
Imports System.Data.SqlClient
Partial Class controls_DeviceRepeatFaultuc
    Inherits System.Web.UI.UserControl
    Public Property IncidentID() As String
    Public Property Device() As String
    Public Property ConcessionN() As String
    Private conn As SqlConnection
    Private comm As SqlCommand
    Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
    Dim commfault As SqlCommand
    'Private ConcessionNumber As String
    Const RepeatFault As String = "Repeatfault"
    Const CancelFaultReturn As String = "Cancel"
    Const EMPTYSTRING As String = ""
    'Public Event UpDateDefectDisplay(ByVal EquipmentName As String)
    Public Event UpdateRepeatFault(ByVal Tab As String, ByVal User As String)
    Public Event UpDateDefectDisplay(ByVal EquipmentName As String)
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim connectionString1 As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
        conn = New SqlConnection(connectionString1)

        If Device Like "T?" Then
                TomoLoad()
                MultiView1.SetActiveView(Tomo)
            Else
                LinacLoad()
                MultiView1.SetActiveView(Linac)
            End If

    End Sub
    Protected Sub LinacLoad()
        AreaBox.Text = String.Empty
        GantryAngleBox.Text = String.Empty
        CollimatorAngleBox.Text = String.Empty
        DescriptionBox.Text = String.Empty
        PatientIDBox.Text = String.Empty
        RadioIncident.SelectedIndex = -1
        conn = New SqlConnection(connectionString)
        'from http://www.sqlservercentral.com/Forums/Topic1416029-1292-1.aspx
        comm = New SqlCommand("SELECT Area from ReportFault where incidentID=@incidentID", conn)
        comm.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
        comm.Parameters("@incidentID").Value = IncidentID
        conn.Open()
        AreaBox.Text = comm.ExecuteScalar()

        conn.Close()

        AddEnergyItem()

    End Sub
    Protected Sub TomoLoad()
        DescriptionBoxT.Text = String.Empty
        PatientIDBoxT.Text = String.Empty
        ErrorTextBox.Text = String.Empty
        Accuray.Text = String.Empty
        RadAct.Text = String.Empty
        RadioIncident.SelectedIndex = -1
        conn = New SqlConnection(connectionString)
        'from http://www.sqlservercentral.com/Forums/Topic1416029-1292-1.aspx
        comm = New SqlCommand("SELECT Action FROM ConcessionTable where incidentID=@incidentID", conn) 'Corrective action
        comm.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
        comm.Parameters("@incidentID").Value = IncidentID
        conn.Open()
        Dim sqlresult1 As Object = comm.ExecuteScalar()
        RadAct.Text = sqlresult1.ToString
        conn.Close()

    End Sub

    Protected Sub AddEnergyItem()
        'from http://www.aspsnippets.com/Articles/Programmatically-add-items-to-DropDownList-on-Button-Click-in-ASPNet-using-C-and-VBNet.aspx
        'and http://www.yaldex.com/asp_tutorial/0596004877_progaspdotnet2-chp-5-sect-7.html
        DropDownListEnergy.Items.Clear()
        'This is a mad way of doing it but I don't know how to dim the energy list without constructing it at the same time
        Select Case Device
            Case "LA1"
                Dim Energy1() As String = {"Select", "6 MV", "6 MeV", "8 MeV", "10 MeV", "12 MeV", "15 MeV", "18 MeV", "20 MeV"}
                ConstructEnergylist(Energy1)
            Case "LA2", "LA3"
                Dim Energy1() As String = {"Select", "6 MV", "10 MV", "6 MeV", "8 MeV", "10 MeV", "12 MeV", "15 MeV", "18 MeV", "20 MeV"}
                ConstructEnergylist(Energy1)
            Case "LA4"
                Dim Energy1() As String = {"Select", "6 MV", "10 MV"}
                ConstructEnergylist(Energy1)
            Case "E1", "E2", "B1"
                Dim Energy1() As String = {"Select", "6 MV", "6 MV FFF", "10 MV", "10 MV FFF", "4 MeV", "6 MeV", "8 MeV", "10 MeV", "12 MeV", "15 MeV"}
                ConstructEnergylist(Energy1)
            Case Else
                'Don't show any energies
        End Select

    End Sub
    Protected Sub ConstructEnergylist(ByVal Energylist() As String)
        Dim energy() As String = Energylist
        Dim i As Integer
        For i = 0 To energy.GetLength(0) - 1
            DropDownListEnergy.Items.Add(New ListItem(energy(i)))
        Next
        DropDownListEnergy.SelectedIndex = -1
    End Sub
    Private Sub SaveRepeatFault_Click1(sender As Object, e As EventArgs) Handles SaveRepeatFault.Click

        Page.Validate("Incident")
        If Page.IsValid Then
            SaveRepeatFaultbutton()
        End If
    End Sub
    Public Sub SaveRepeatFaultbutton()
        Dim faultid As Integer = -1
        Dim Energy As String = String.Empty
        Dim Description As String = String.Empty
        Dim ReportedBy As String = String.Empty
        Dim DateReported As DateTime = Now()
        Dim Area As String = String.Empty
        Dim CollimatorAngle As String = String.Empty
        Dim GantryAngle As String = String.Empty
        Dim PatientId As String = String.Empty
        Dim RadioIncidentSelected As String
        If Device Like "T?" Then
            Area = ErrorTextBox.Text
            Description = DescriptionBoxT.Text
            PatientId = PatientIDBoxT.Text

        Else
            Energy = DropDownListEnergy.SelectedItem.Text
            If Energy = "Select" Then
                Energy = String.Empty
            End If
            Area = AreaBox.Text
            GantryAngle = GantryAngleBox.Text
            CollimatorAngle = CollimatorAngleBox.Text
            Description = DescriptionBox.Text
            PatientId = PatientIDBox.Text

        End If
        RadioIncidentSelected = RadioIncident.SelectedItem.Value

        faultid = DavesCode.NewFaultHandling.InsertRepeatFault(Description, ReportedBy, DateReported, Area, Energy, GantryAngle, CollimatorAngle, Device, IncidentID, PatientId, ConcessionN, RadioIncidentSelected)
        If Not faultid = -1 Then
            RaiseEvent UpdateRepeatFault(RepeatFault, ReportedBy)
        Else
            RaiseError()
        End If
    End Sub
    Protected Sub RaiseError()
        Dim strScript As String = "<script>"

        strScript += "alert('Problem Updating Fault. Please call Engineering');"
        strScript += "</script>"
        ScriptManager.RegisterStartupScript(SaveRepeatFault, Me.GetType(), "JSCR", strScript.ToString(), False)
    End Sub
    Protected Sub CancelFault_Click(sender As Object, e As EventArgs) Handles CancelFault.Click
        RaiseEvent UpdateRepeatFault(CancelFaultReturn, String.Empty)
    End Sub
End Class

﻿Imports System.Data
Imports System.Data.SqlClient
Partial Class controls_DeviceRepeatFaultuc
    Inherits System.Web.UI.UserControl
    Public Property IncidentID() As String
    Public Property Device() As String
    Private conn As SqlConnection
    Private comm As SqlCommand
    Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
    Dim commfault As SqlCommand
    Dim ConcessionNumber As String
    Const RepeatFault As String = "Updatefault"
    Const CancelFaultReturn As String = "Cancel"
    'Public Event UpDateDefectDisplay(ByVal EquipmentName As String)
    Public Event UpdateRepeatFault(ByVal Tab As String, ByVal User As String)

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
        PatientIDBox.Text = String.Empty

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
    End Sub

    Protected Sub AddEnergyItem()
        'from http://www.aspsnippets.com/Articles/Programmatically-add-items-to-DropDownList-on-Button-Click-in-ASPNet-using-C-and-VBNet.aspx
        'and http://www.yaldex.com/asp_tutorial/0596004877_progaspdotnet2-chp-5-sect-7.html

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
        SaveRepeatFaultbutton()
    End Sub
    Public Sub SaveRepeatFaultbutton()

        Dim Energy As String = String.Empty
        Dim Description As String = String.Empty
        Dim ReportedBy As String = String.Empty
        Dim DateReported As DateTime = Now()
        Dim Area As String = String.Empty
        Dim CollimatorAngle As String = String.Empty
        Dim GantryAngle As String = String.Empty
        Dim PatientId As String = String.Empty


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


        conn = New SqlConnection(connectionString)

        'this gets the relevant concession number and creates the entry for the report fault table that is used subsequently to create the defect table entries
        'comm = New SqlCommand("SELECT ConcessionNumber + ' ' + ConcessionDescription As Fault FROM [ConcessionTable] where incidentID = @incidentID", conn)
        comm = New SqlCommand("SELECT ConcessionNumber FROM [ConcessionTable] where incidentID = @incidentID", conn)
        comm.Parameters.AddWithValue("@incidentID", IncidentID)
        conn.Open()
        'commstatus.ExecuteNonQuery()

        Dim obj As Object = comm.ExecuteScalar()
        'Dim LinacStatusIDs As String = obj.ToString()
        ConcessionNumber = CStr(obj)
        conn.Close()


        commfault = New SqlCommand("INSERT INTO ReportFault (Description, ReportedBy, DateReported, Area, Energy, GantryAngle, CollimatorAngle,Linac, IncidentID, BSUHID, ConcessionNumber) " _
                                  & "VALUES (@Description, @ReportedBy, @DateReported, @Area, @Energy,@GantryAngle,@CollimatorAngle, @Linac, @IncidentID, @BSUHID, @ConcessionNumber )", conn)
        commfault.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar, 250)
        commfault.Parameters("@Description").Value = Description
        commfault.Parameters.Add("@ReportedBy", System.Data.SqlDbType.NVarChar, 50)
        commfault.Parameters("@ReportedBy").Value = ReportedBy
        commfault.Parameters.Add("@DateReported", System.Data.SqlDbType.DateTime)
        commfault.Parameters("@DateReported").Value = DateReported
        commfault.Parameters.Add("@Area", System.Data.SqlDbType.NVarChar, 20)
        commfault.Parameters("@Area").Value = Area
        commfault.Parameters.Add("@Energy", System.Data.SqlDbType.NVarChar, 10)
        commfault.Parameters("@Energy").Value = Energy
        commfault.Parameters.Add("@GantryAngle", System.Data.SqlDbType.NVarChar, 3)
        commfault.Parameters("@GantryAngle").Value = GantryAngle
        commfault.Parameters.Add("@CollimatorAngle", System.Data.SqlDbType.NVarChar, 3)
        commfault.Parameters("@CollimatorAngle").Value = CollimatorAngle
        commfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
        commfault.Parameters("@Linac").Value = Device
        commfault.Parameters.Add("@IncidentID", System.Data.SqlDbType.Int)
        commfault.Parameters("@IncidentID").Value = IncidentID
        commfault.Parameters.Add("@BSUHID", System.Data.SqlDbType.VarChar, 7)
        commfault.Parameters("@BSUHID").Value = PatientId
        commfault.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 25)
        commfault.Parameters("@ConcessionNumber").Value = ConcessionNumber


        Try
            conn.Open()
            commfault.ExecuteNonQuery()
            conn.Close()

        Finally
            conn.Close()

        End Try
        'RaiseEvent UpDateDefectDisplay(Device)
        RaiseEvent UpdateRepeatFault(RepeatFault, ReportedBy)
    End Sub

    Protected Sub CancelFault_Click(sender As Object, e As EventArgs) Handles CancelFault.Click
        RaiseEvent UpdateRepeatFault(CancelFaultReturn, String.Empty)
    End Sub
End Class

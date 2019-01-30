Imports System.Data
Imports System.Data.SqlClient

Partial Class controls_DeviceReportedfaultuc
    Inherits UserControl
    Public Property IncidentID() As String
    Public Property Device() As String
    Public Property ReportFaultID() As String
    Private conn As SqlConnection
    Private comm As SqlCommand

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

        comm = New SqlCommand("select distinct r.FaultID, r.Description, r.ReportedBy, r.DateReported, f.Status, r.RadiationIncident, r.Area, r.Energy, r.GantryAngle, r.CollimatorAngle, ISNULL (r.BSUHID, '') as BSUHID,ISNULL(c.ConcessionNumber, '') as ConcessionNumber , ISNULL(c.concessiondescription, '') as ConcessionDescription, f.linac, f.IncidentID " _
       & "from reportfault r left outer join FaultIDTable f on f.OriginalFaultID = r.FaultID left outer join ConcessionTable c on f.ConcessionNumber=c.ConcessionNumber where f.incidentID = @incidentID", conn)

        comm.Parameters.AddWithValue("@incidentID", IncidentID)
        Try
            conn.Open()
            Dim da As New SqlDataAdapter(comm)
            Dim dt As New DataTable()
            'Dim nfaultid As String
            'Dim ndescription As String
            'Dim nrep As String
            'Dim ndate As String
            'Dim nen As String
            'Dim nga As String
            'Dim nca As String
            'Dim nlin As String
            'Dim narea As String
            'Dim nconc As String
            'Dim ncond As String
            'Dim nincid As String
            'Dim nbsuhid As String
            'Dim FaultStatus As String
            'Dim Radioincident As Boolean


            da.Fill(dt)
            If dt.Rows.Count > 0 Then

                For Each dataRow As DataRow In dt.Rows
                    'nfaultid = dataRow("FaultId")
                    OriginalDescriptionBoxL.Text = dataRow("Description")
                    OriginalReportedBoxL.Text = dataRow("ReportedBy")
                    OriginalOpenDateBoxL.Text = dataRow("DateReported")
                    'FaultStatus = dataRow("Status")
                    OriginalAreaBox.Text = dataRow("Area")
                    OriginalEnergyBox.Text = dataRow("Energy")
                    OriginalGantryBox.Text = dataRow("GantryAngle")
                    OriginalCollBox.Text = dataRow("CollimatorAngle")
                    'nconc = dataRow("ConcessionNumber")
                    'ncond = dataRow("ConcessionDescription")
                    'nlin = dataRow("Linac")
                    'nincid = dataRow("incidentID")
                    OriginalPatientIDBoxL.Text = dataRow("BSUHID")
                    If dataRow.IsNull("RadiationIncident") Then
                        OriginalRadioIncident.Visible = False
                        RadiationIncidentLabel.Visible = False
                    Else
                        OriginalRadioIncident.SelectedValue = dataRow("RadiationIncident")
                    End If


                Next
            End If

            'OriginalDescriptionBoxL.Text = ndescription
            'OriginalAreaBox.Text = narea
            'OriginalEnergyBox.Text = nen
            'OriginalGantryBox.Text = nga
            'OriginalCollBox.Text = nca
            'OriginalReportedBoxL.Text = nrep
            'OriginalOpenDateBoxL.Text = ndate
            'OriginalPatientIDBoxL.Text = nbsuhid
            'OriginalRadioIncident.SelectedValue = Radioincident

        Finally
            conn.Close()
        End Try
    End Sub

    Protected Sub TomoLoad()
        comm = New SqlCommand("select distinct r.FaultID, r.Description, r.ReportedBy, r.DateReported, f.Status, r.Area, r.Energy, ISNULL (r.BSUHID, '') as BSUHID,ISNULL(c.ConcessionNumber, '') as ConcessionNumber , ISNULL(c.concessiondescription, '') as ConcessionDescription, f.linac, f.IncidentID " _
      & "from reportfault r left outer join FaultIDTable f on f.OriginalFaultID = r.FaultID left outer join ConcessionTable c on f.ConcessionNumber=c.ConcessionNumber where f.incidentID = @incidentID", conn)

        comm.Parameters.AddWithValue("@incidentID", IncidentID)
        Try
            conn.Open()
            Dim da As New SqlDataAdapter(comm)
            Dim dt As New DataTable()
            Dim nfaultid As String
            Dim ndescription As String
            Dim nrep As String
            Dim ndate As String
            Dim nen As String
            Dim nlin As String
            Dim narea As String
            Dim nconc As String
            Dim ncond As String
            Dim nincid As String
            Dim nbsuhid As String
            Dim FaultStatus As String


            da.Fill(dt)
            If dt.Rows.Count > 0 Then

                For Each dataRow As DataRow In dt.Rows
                    nfaultid = dataRow("FaultId")
                    ndescription = dataRow("Description")
                    nrep = dataRow("ReportedBy")
                    ndate = dataRow("DateReported")
                    FaultStatus = dataRow("Status")
                    narea = dataRow("Area")
                    nen = dataRow("Energy")
                    nconc = dataRow("ConcessionNumber")
                    ncond = dataRow("ConcessionDescription")
                    nlin = dataRow("Linac")
                    nincid = dataRow("incidentID")
                    nbsuhid = dataRow("BSUHID")

                Next
            End If

            OriginalDescriptionBoxT.Text = ndescription
            AccurayTextBox.Text = narea
            ErrorTextBox.Text = nen
            OriginalReportedBoxT.Text = nrep
            OriginalOpenDateBoxT.Text = ndate
            OriginalPatientIDBoxT.Text = nbsuhid

        Finally
            conn.Close()
        End Try
    End Sub
End Class

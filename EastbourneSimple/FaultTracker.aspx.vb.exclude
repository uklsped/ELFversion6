Imports System.Data.SqlClient
Partial Class FaultTracker
    Inherits System.Web.UI.Page

    'Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    Dim conn As SqlConnection
    '    Dim comm As SqlCommand
    '    Dim reader As SqlDataReader
    '    Dim faultnumber As Integer
    '    Dim connectionstring As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
    '    conn = New SqlConnection(connectionstring)
    '    comm = New SqlCommand("Select FaultID from ReportFault where FaultID = @FaultId", conn)
    '    comm.Parameters.Add("@FaultID", Data.SqlDbType.Int)
    '    comm.Parameters("@FaultID").Value = TextBox1.Text
    '    conn.Open()
    '    reader = comm.ExecuteReader()

    '    If reader.Read() Then
    '        userLabel.Text = "FaultID: " & reader.Item("FaultID")
    '    Else
    '        userLabel.Text = "FaultID: No such fault"
    '    End If
    '    reader.Close()
    '    conn.Close()

    'End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim time As DateTime
        Dim LastFault As Integer
        time = Now()
        Dim ID As Integer = CInt(GridView1.SelectedDataKey.Values("FaultID"))

        Dim conn As SqlConnection

        Dim connectionString As String = ConfigurationManager.ConnectionStrings( _
        "connectionstring").ConnectionString

        'Dim commstatus As SqlCommand
        conn = New SqlConnection(connectionString)
        'commstatus = New SqlCommand("INSERT INTO LinacStatus ( State, DateTime) " & _
        '                            "VALUES ( @State, @Datetime)", conn)
        'commstatus.Parameters.Add("@State", System.Data.SqlDbType.NVarChar, 50)
        'commstatus.Parameters("@State").Value = "Fault"
        'commstatus.Parameters.Add("@DateTime", System.Data.SqlDbType.DateTime)
        'commstatus.Parameters("@DateTime").Value = time



        'Try
        '    conn.Open()
        '    commstatus.ExecuteNonQuery()

        'Finally
        '    conn.Close()
        'End Try



        Dim commtrack As SqlCommand
        commtrack = New SqlCommand("Insert into FaultTracking (Trackingcomment, AssignedTo, Status, LastupdatedBy, Lastupdatedon, FaultID) " & _
                                   "VALUES (@Trackingcomment, @AssignedTo, @Status, @LastupdatedBy, @Lastupdatedon, @FaultID)", conn)
        commtrack.Parameters.Add("@Trackingcomment", System.Data.SqlDbType.NVarChar, 250)
        commtrack.Parameters("@Trackingcomment").Value = TextBox2.Text
        commtrack.Parameters.Add("@AssignedTo", Data.SqlDbType.NVarChar, 50)
        commtrack.Parameters("@AssignedTo").Value = DropDownList2.SelectedItem.Text
        commtrack.Parameters.Add("@Status", Data.SqlDbType.NVarChar, 50)
        commtrack.Parameters("@Status").Value = DropDownList1.SelectedItem.Text
        commtrack.Parameters.Add("@LastupdatedBy", System.Data.SqlDbType.NVarChar, 50)
        commtrack.Parameters("@LastupdatedBy").Value = "DS"
        commtrack.Parameters.Add("@Lastupdatedon", System.Data.SqlDbType.DateTime)
        commtrack.Parameters("@Lastupdatedon").Value = time
        commtrack.Parameters.Add("@FaultID", System.Data.SqlDbType.Int)
        commtrack.Parameters("@FaultID").Value = ID

        Dim commfault As SqlCommand
        commfault = New SqlCommand("Update ReportFault SET FaultStatus=@FaultStatus WHERE FaultID=@FaultID", conn)
        commfault.Parameters.Add("@FaultStatus", Data.SqlDbType.NVarChar, 50)
        commfault.Parameters("@FaultStatus").Value = DropDownList1.SelectedItem.Text
        commfault.Parameters.Add("@FaultID", System.Data.SqlDbType.Int)
        commfault.Parameters("@FaultID").Value = ID
        Try
            conn.Open()
            commtrack.ExecuteNonQuery()
            conn.Close()
            conn.Open()
            commfault.ExecuteNonQuery()
        Finally
            conn.Close()

        End Try
        Dim secstatus As String = DropDownList1.SelectedItem.Text

        Select Case secstatus
            Case "Open"

            Case "Closed"
                Dim commsecond As SqlCommand
                commsecond = New SqlCommand("INSERT INTO SecondaryState ( SState, DateTime) " & _
                                            "VALUES ( @SState, @Datetime)", conn)
                commsecond.Parameters.Add("@SState", System.Data.SqlDbType.NVarChar, 50)
                commsecond.Parameters("@SState").Value = ""
                commsecond.Parameters.Add("@DateTime", System.Data.SqlDbType.DateTime)
                commsecond.Parameters("@DateTime").Value = time
                Try
                    conn.Open()
                    commsecond.ExecuteNonQuery()
                    conn.Close()

                Finally
                    conn.Close()

                End Try
            Case "Concession"
                Dim commsecond As SqlCommand
                commsecond = New SqlCommand("INSERT INTO SecondaryState ( SState, DateTime) " & _
                                            "VALUES ( @SState, @Datetime)", conn)
                commsecond.Parameters.Add("@SState", System.Data.SqlDbType.NVarChar, 50)
                commsecond.Parameters("@SState").Value = ""
                commsecond.Parameters.Add("@DateTime", System.Data.SqlDbType.DateTime)
                commsecond.Parameters("@DateTime").Value = time
                Try
                    conn.Open()
                    commsecond.ExecuteNonQuery()
                    conn.Close()

                Finally
                    conn.Close()

                End Try
            Case "Physics"
            Case "New"


        End Select





    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
End Class

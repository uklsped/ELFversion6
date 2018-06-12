Imports System.Data
Imports System.Data.SqlClient
Namespace DavesCode
    Public Class ReusePC
        Public Shared Function CommitRunup(ByVal LinacName As String, ByVal tabby As String, ByVal LogOffName As String, ByVal TextBoxc As String, ByVal Valid As Boolean, ByVal Fault As Boolean, ByVal lock As Boolean) As String
            Dim time As DateTime
            time = Now()
            Dim commitusername As String = LogOffName
            Dim LogInName As String = ""
            Dim conn As SqlConnection
            Dim comm As SqlCommand
            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString
            Dim TextBox As String = TextBoxc
            Dim machinename As String = LinacName
            Dim tablabel As Integer = tabby 'this is inconsistent
            Dim reader As SqlDataReader
            Dim StartTime As DateTime
            Dim LogOffStateID As String
            Dim LogOnStateID As String = ""
            Dim Approved As Boolean = Valid
            Dim breakdown As Boolean = Fault
            Dim contime As SqlCommand
            'Dim Activity As Integer = 2

            conn = New SqlConnection(connectionString)

            contime = New SqlCommand("SELECT DateTime, UserName, stateID FROM [LinacStatus] where stateID = (Select max(stateID) as lastrecord from [LinacStatus] where linac=@linac)", conn)

            contime.Parameters.AddWithValue("@linac", LinacName)
            conn.Open()
            reader = contime.ExecuteReader()

            If reader.Read() Then
                StartTime = reader.Item("DateTime")
                LogInName = reader.Item("UserName")
                LogOnStateID = reader.Item("stateID")
            End If
            reader.Close()
            conn.Close()
            If Fault Then
                'If it's a fault it will go straight to fault page so user will be whichever user group logged on ie engineering or physics.
                '2nd june 2016 changed userreason to 103 and activity to 0 - this stops duration being written for fault
                LogOffStateID = Reuse.SetStatus(commitusername, "Fault", 5, 103, machinename, tablabel)
            Else
                If Approved Then
                    If tablabel = 1 Then
                        LogOffStateID = Reuse.SetStatus(commitusername, "Engineering Approved", 5, 7, machinename, 1)
                    Else
                        LogOffStateID = Reuse.SetStatus(commitusername, "Radiographer Approved", 5, 7, machinename, 1)
                    End If
                Else
                    'added 5/6/16 to set reason to end of day for midnight reset
                    If commitusername = "System" Then
                        LogOffStateID = Reuse.SetStatus(commitusername, "Linac Unauthorised", 5, 102, machinename, 1)
                    Else
                        LogOffStateID = Reuse.SetStatus(commitusername, "Linac Unauthorised", 5, 7, machinename, 1)
                    End If

                End If

            End If
            If Not lock Then
                Reuse.UpdateActivityTable(machinename, LogOffStateID)
            End If

            Dim minutesDuration As Decimal
            Dim duration As TimeSpan = time - StartTime
            minutesDuration = Decimal.Round(duration.TotalMinutes, 2, MidpointRounding.ToEven)
            'Had to amend this to insert extra energies for E1 4/7/17
            'new table for handoverenergies
            comm = New SqlCommand("INSERT INTO HandoverEnergies ( MV6, MV6FFF, MV10, MV10FFF,MeV4, MeV6, MeV8, MeV10, MeV12, MeV15, MeV18, " &
                                  "MeV20, Comment, LogOutName, LogOutDate, linac, LogInDate, Duration,LogInStatusID, LogOutStatusID, Approved, LogInName) " &
                                  "VALUES ( @MV6, @MV6FFF, @MV10,@MV10FFF, @MeV4, @MeV6, @MeV8, @MeV10, @MeV12, @MeV15, @MeV18, @MeV20,  @Comment, @LogOutName, " &
                                  "@LogOutDate, @linac, @LogInDate, @Duration,@LogInStatusID, @LogOutStatusID, @Approved, @LogInName)", conn)

            Select Case tablabel
                Case 1
                    Select Case machinename
                        Case "T1"
                            comm.Parameters.Add("@MV6", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV6").Value = False
                            comm.Parameters.Add("@MV6FFF", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV6FFF").Value = False
                            comm.Parameters.Add("@MV10", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV10").Value = False
                            comm.Parameters.Add("@MV10FFF", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV10FFF").Value = False
                            comm.Parameters.Add("@MeV4", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV4").Value = False
                            comm.Parameters.Add("@MeV6", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV6").Value = False
                            comm.Parameters.Add("@MeV8", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV8").Value = False
                            comm.Parameters.Add("@MeV10", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV10").Value = False
                            comm.Parameters.Add("@MeV12", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV12").Value = False
                            comm.Parameters.Add("@MeV15", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV15").Value = False
                            comm.Parameters.Add("@MeV18", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV18").Value = False
                            comm.Parameters.Add("@MeV20", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV20").Value = False

                        Case Else
                    End Select

                Case 7
                    Select Case machinename
                        Case "T1"
                            comm.Parameters.Add("@MV6", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV6").Value = False
                            comm.Parameters.Add("@MV6FFF", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV6FFF").Value = False
                            comm.Parameters.Add("@MV10", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV10").Value = False
                            comm.Parameters.Add("@MV10FFF", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV10FFF").Value = False
                            comm.Parameters.Add("@MeV4", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV4").Value = False
                            comm.Parameters.Add("@MeV6", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV6").Value = False
                            comm.Parameters.Add("@MeV8", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV8").Value = False
                            comm.Parameters.Add("@MeV10", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV10").Value = False
                            comm.Parameters.Add("@MeV12", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV12").Value = False
                            comm.Parameters.Add("@MeV15", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV15").Value = False
                            comm.Parameters.Add("@MeV18", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV18").Value = False
                            comm.Parameters.Add("@MeV20", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV20").Value = False

                        Case Else
                    End Select
                Case Else
                    'needed to add eastbourne energies here to cater for fault condition
                    comm.Parameters.Add("@MV6", System.Data.SqlDbType.Bit)
                    comm.Parameters("@MV6").Value = False
                    comm.Parameters.Add("@MV6FFF", System.Data.SqlDbType.Bit)
                    comm.Parameters("@MV6FFF").Value = False
                    comm.Parameters.Add("@MV10", System.Data.SqlDbType.Bit)
                    comm.Parameters("@MV10").Value = False
                    comm.Parameters.Add("@MV10FFF", System.Data.SqlDbType.Bit)
                    comm.Parameters("@MV10FFF").Value = False
                    comm.Parameters.Add("@MeV4", System.Data.SqlDbType.Bit)
                    comm.Parameters("@MeV4").Value = False
                    comm.Parameters.Add("@MeV6", System.Data.SqlDbType.Bit)
                    comm.Parameters("@MeV6").Value = False
                    comm.Parameters.Add("@MeV8", System.Data.SqlDbType.Bit)
                    comm.Parameters("@MeV8").Value = False
                    comm.Parameters.Add("@MeV10", System.Data.SqlDbType.Bit)
                    comm.Parameters("@MeV10").Value = False
                    comm.Parameters.Add("@MeV12", System.Data.SqlDbType.Bit)
                    comm.Parameters("@MeV12").Value = False
                    comm.Parameters.Add("@MeV15", System.Data.SqlDbType.Bit)
                    comm.Parameters("@MeV15").Value = False
                    comm.Parameters.Add("@MeV18", System.Data.SqlDbType.Bit)
                    comm.Parameters("@MeV18").Value = False
                    comm.Parameters.Add("@MeV20", System.Data.SqlDbType.Bit)
                    comm.Parameters("@MeV20").Value = False

            End Select


            comm.Parameters.Add("@Comment", System.Data.SqlDbType.NVarChar, 250)
            comm.Parameters("@Comment").Value = TextBox
            comm.Parameters.Add("@LogOutName", System.Data.SqlDbType.NVarChar, 10)
            comm.Parameters("@LogOutName").Value = commitusername 'This will have to find real user name
            comm.Parameters.Add("@LogOutDate", System.Data.SqlDbType.DateTime)
            comm.Parameters("@LogOutDate").Value = time
            comm.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
            comm.Parameters("@linac").Value = machinename
            comm.Parameters.Add("@LogInDate", System.Data.SqlDbType.DateTime)
            comm.Parameters("@LogInDate").Value = StartTime
            comm.Parameters.Add("@Duration", System.Data.SqlDbType.Decimal)
            comm.Parameters("@Duration").Value = minutesDuration
            comm.Parameters.Add("@LogInStatusID", System.Data.SqlDbType.NVarChar, 10)
            comm.Parameters("@LogInStatusID").Value = LogOnStateID
            comm.Parameters.Add("@LogOutStatusID", System.Data.SqlDbType.NVarChar, 10)
            comm.Parameters("@LogOutStatusID").Value = LogOffStateID
            comm.Parameters.Add("@Approved", System.Data.SqlDbType.Bit)
            comm.Parameters("@Approved").Value = Approved
            comm.Parameters.Add("LogInName", SqlDbType.NVarChar, 50)
            comm.Parameters("LogInName").Value = LogInName
            Try
                conn.Open()
                comm.ExecuteNonQuery()

            Finally
                conn.Close()

            End Try
            Return LogOffStateID

        End Function
    End Class
End Namespace

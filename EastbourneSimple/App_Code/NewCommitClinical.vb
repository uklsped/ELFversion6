Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Namespace DavesCode
    Public Class NewCommitClinical
        Public Shared Function CommitClinical(ByVal LinacN As String, ByVal username As String, ByVal Fault As Boolean) As String
            Dim LinacStatusID As Integer
            Dim logInStatusID As Integer
            Dim time As DateTime
            Dim LinacName As String = LinacN
            Dim logOutName As String = username
            Dim logInName As String = ""
            time = Now()
            Dim StartTime As DateTime
            Dim reader As SqlDataReader
            Dim CHID As Integer
            Dim breakdown As Boolean = Fault
            'Dim textbox As String = TextBoxc
            Dim conn As SqlConnection
            Dim SqlDataSource1 As New SqlDataSource()

            Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
            Dim commHAuthID As SqlCommand
            Dim contime As SqlCommand
            Try

                Using myscope As TransactionScope = New TransactionScope()
                    conn = New SqlConnection(connectionString)
                    'This will get the time the linac was accepted for clinical
                    contime = New SqlCommand("SELECT stateid, DateTime, UserName FROM [LinacStatus] where stateID = (Select max(stateID) as lastrecord from [LinacStatus] where linac=@linac And state='clinical')", conn)
                    contime.Parameters.AddWithValue("@linac", LinacName)
                    conn.Open()
                    reader = contime.ExecuteReader()

                    If reader.Read() Then
                        logInStatusID = reader.Item("stateid")
                        StartTime = reader.Item("DateTime")
                        logInName = reader.Item("UserName")
                    End If
                    reader.Close()
                    conn.Close()
                    'This calculates the time between logging on and now - but why here?
                    Dim minutesDuration As Decimal
                    Dim duration As TimeSpan = time - StartTime

                    minutesDuration = Decimal.Round(duration.TotalMinutes, 2, MidpointRounding.ToEven)

                    'this gets the ID of the associated engineering handover
                    commHAuthID = New SqlCommand("Select CHandID from ClinicalHandover where CHandID  = (Select max(CHandID) as lastrecord from ClinicalHandover where linac=@linac)", conn)
                    commHAuthID.Parameters.Add("@linac", SqlDbType.NVarChar, 10)
                    commHAuthID.Parameters("@linac").Value = LinacName

                    conn.Open()
                    reader = commHAuthID.ExecuteReader()
                    If reader.Read() Then
                        CHID = reader.Item("CHandId")
                        reader.Close()
                        conn.Close()
                    End If
                    'Moved If Breakdown after this section, checking and writing treatment table to here because it needs to be checked both for breakdown and if recover button operated.
                    'It's only if there is a breakdown or recover that there will be a null entry for treatmentstoptime Not true 22/11/17

                    commHAuthID = New SqlCommand("select treatmentid, treatmentstoptime from treatmenttable where treatmentid = (select max(treatmentID) from treatmentTable where linac=@linac)", conn)
                    commHAuthID.Parameters.Add("@linac", SqlDbType.NVarChar, 10)
                    commHAuthID.Parameters("@linac").Value = LinacName
                    conn.Open()
                    reader = commHAuthID.ExecuteReader()
                    If reader.Read() Then 'this means there is an entry so
                        'checking for null from http://www.triconsole.com/dotnet/sqldatareader_class.php#isdbnull
                        If reader.IsDBNull(1) Then
                            SetTreatment("Not Treating", LinacName, logInStatusID, connectionString)
                        End If
                        reader.Close()

                    End If
                    conn.Close()
                    'This is here because this sub is also called from the fault page in order to write the linacstatus and to write clinicalhandover table
                    If breakdown Then

                        LinacStatusID = DavesCode.Reuse.SetStatusNew(logOutName, "Fault", 5, 103, LinacName, -1, connectionString)
                    Else 'not a breakdown so if approved set clinical - not treating or back to engineering approved
                        If logOutName = "System" Then
                            LinacStatusID = DavesCode.Reuse.SetStatusNew(logOutName, "Linac Unauthorised", 5, 102, LinacName, 3, connectionString)
                        Else
                            LinacStatusID = DavesCode.Reuse.SetStatusNew(logOutName, "Suspended", 5, 7, LinacName, 3, connectionString)
                        End If

                    End If
                    'http://www.mikesdotnetting.com/Article/53/Saving-a-user%27s-CheckBoxList-selection-and-re-populating-the-CheckBoxList-from-saved-data - used for imaging
                    'This writes the clinicalstatus table

                    Dim commaccept As SqlCommand
                    commaccept = New SqlCommand("INSERT INTO ClinicalStatus ( PClinID, LogInDate, LogOutDate, linac, Duration, LogInName, LogOutName,LogOutStatusID, logInStatusID) " &
                                                "VALUES (@PClinID, @LogInDate, @LogOutDate, @linac, @Duration,@LogInName, @LogOutName, @LogOutStatusID, @logInStatusID)", conn)

                    commaccept.Parameters.Add("@PClinID", Data.SqlDbType.Int)
                    commaccept.Parameters("@PClinID").Value = CHID
                    commaccept.Parameters.Add("@LogInDate", System.Data.SqlDbType.DateTime)
                    commaccept.Parameters("@LogInDate").Value = StartTime
                    commaccept.Parameters.Add("@LogOutDate", System.Data.SqlDbType.DateTime)
                    commaccept.Parameters("@LogOutDate").Value = time
                    commaccept.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
                    commaccept.Parameters("@linac").Value = LinacName
                    commaccept.Parameters.Add("@Duration", System.Data.SqlDbType.Decimal)
                    commaccept.Parameters("@Duration").Value = minutesDuration
                    commaccept.Parameters.Add("@LogInName", System.Data.SqlDbType.NVarChar, 50)
                    commaccept.Parameters("@LogInName").Value = logInName
                    commaccept.Parameters.Add("@LogOutName", System.Data.SqlDbType.NVarChar, 50)
                    commaccept.Parameters("@LogOutName").Value = logOutName
                    commaccept.Parameters.Add("@LogOutStatusID", Data.SqlDbType.Int)
                    commaccept.Parameters("@LogOutStatusID").Value = LinacStatusID
                    commaccept.Parameters.Add("@logInStatusID", Data.SqlDbType.Int)
                    commaccept.Parameters("@logInStatusID").Value = logInStatusID
                    conn.Open()
                    commaccept.ExecuteNonQuery()

                    conn.Close()
                    DavesCode.Reuse.UpdateActivityTable(LinacName, LinacStatusID, connectionString)
                    myscope.Complete()
                End Using
            Catch ex As Exception
                DavesCode.ReusePC.LogError(ex)
            End Try

            Return LinacStatusID
        End Function
        Public Shared Sub SetTreatment(ByVal State As String, ByVal linacid As String, ByVal linacstate As String, ByVal connectionString As String)
            Dim time As DateTime
            Dim MachineType As String = linacid
            Dim StateType As String = State
            Dim Linacstatusid As String = linacstate
            Dim conn As SqlConnection
            'Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            '"connectionstring").ConnectionString
            Dim commstatus As SqlCommand
            time = Now()
            conn = New SqlConnection(connectionString)
            If StateType = "Treating" Then
                commstatus = New SqlCommand("INSERT INTO TreatmentTable ( TreatmentStartTime, LinacStatusId,linac) " &
                                            "VALUES ( @TreatmentStartTime, @LinacStatusID, @linac)", conn)

                commstatus.Parameters.Add("@TreatmentStartTime", System.Data.SqlDbType.DateTime)
                commstatus.Parameters("@TreatmentStartTime").Value = time
                commstatus.Parameters.Add("@LinacStatusID", System.Data.SqlDbType.NVarChar, 10)
                commstatus.Parameters("@LinacStatusId").Value = Linacstatusid
                commstatus.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
                commstatus.Parameters("@linac").Value = MachineType
            Else
                'Update command changed 12 April because it should update last row not all rows with same LinacStatusId
                'commstatus = New SqlCommand("UPDATE  TreatmentTable SET TreatmentStopTime = @TreatmentStopTime where LinacStatusid = @LinacStatusID", conn)
                commstatus = New SqlCommand("UPDATE  TreatmentTable SET TreatmentStopTime = @TreatmentStopTime where TreatmentID  = (Select max(treatmentID) as lastrecord from treatmenttable where linac=@linac)", conn)
                commstatus.Parameters.Add("@TreatmentStopTime", System.Data.SqlDbType.DateTime)
                commstatus.Parameters("@TreatmentStopTime").Value = time
                commstatus.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
                commstatus.Parameters("@linac").Value = MachineType

            End If
            'Try
            conn.Open()
            commstatus.ExecuteNonQuery()

            'Finally
            conn.Close()
            'End Try

        End Sub

    End Class
End Namespace

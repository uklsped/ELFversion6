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

        Public Shared Function InsertReportFault(ByVal Description As String, ByVal ReportedBy As String, ByVal DateReported As DateTime, ByVal Area As String, ByVal Energy As String, ByVal GantryAngle As String, ByVal CollimatorAngle As String, ByVal Device As String, ByVal IncidentID As Integer, ByVal PatientID As String, ByVal ConcessionNumber As String) As Integer
            Dim LastFault As Integer
            Dim conn As SqlConnection
            Dim commfault As SqlCommand
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
            conn = New SqlConnection(connectionString)

            commfault = New SqlCommand("INSERT INTO ReportFault (Description, ReportedBy, DateReported, Area, Energy, GantryAngle, CollimatorAngle,Linac, IncidentID, BSUHID, ConcessionNumber) " _
                                 & "VALUES (@Description, @ReportedBy, @DateReported, @Area, @Energy,@GantryAngle,@CollimatorAngle, @Linac, @IncidentID, @BSUHID, @ConcessionNumber) Select SCOPE_IDENTITY()", conn)
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
            commfault.Parameters("@BSUHID").Value = PatientID
            commfault.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 25)
            commfault.Parameters("@ConcessionNumber").Value = ConcessionNumber


            Try
                conn.Open()
                'commfault.ExecuteNonQuery()
                Dim obj As Object = commfault.ExecuteScalar()

                LastFault = CInt(obj)
                conn.Close()

            Finally
                conn.Close()

            End Try
            Return LastFault
        End Function

        Public Shared Function InsertNewConcession(ByVal Description As String, ByVal Device As String, ByVal IncidentID As Integer, ByVal ConcessionAction As String) As String
            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
            conn = New SqlConnection(connectionString)
            Dim commconcess As SqlCommand

            Dim bcommand = New SqlCommand("select count(*) from Concessiontable where incidentID=@incidentID", conn)
            bcommand.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
            bcommand.Parameters("@incidentID").Value = IncidentID

            conn.Open()

            Dim exists As Integer
            exists = bcommand.ExecuteScalar()
            conn.Close()
            If exists = 0 Then
                'commconcess.ExecuteNonQuery()
                'from http://www.dotnetperls.com/string-format-vbnet
                Dim objTransaction As SqlTransaction
                objTransaction = Nothing
                Try
                    conn.Open()
                    'objTransaction = conn.BeginTransaction()

                    commconcess = New SqlCommand("Insert into ConcessionTable (PreFix, ConcessionDescription, IncidentID, Linac, ConcessionActive, Action) VALUES (@PreFix, @ConcessionDescription, @IncidentID, @Linac, @ConcessionActive, @Action) SELECT SCOPE_IDENTITY()", conn)
                    commconcess.Parameters.Add("@PreFix", System.Data.SqlDbType.NVarChar, 10)
                    commconcess.Parameters("@PreFix").Value = "ELF"
                    commconcess.Parameters.Add("@ConcessionDescription", System.Data.SqlDbType.NVarChar, 25)
                    commconcess.Parameters("@ConcessionDescription").Value = Description
                    commconcess.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    commconcess.Parameters("@incidentID").Value = IncidentID
                    commconcess.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                    commconcess.Parameters("@Linac").Value = Device
                    commconcess.Parameters.Add("@ConcessionActive", System.Data.SqlDbType.Bit)
                    commconcess.Parameters("@ConcessionActive").Value = 1
                    commconcess.Parameters.Add("@Action", System.Data.SqlDbType.NVarChar, 250)
                    commconcess.Parameters("@Action").Value = ConcessionAction
                    Dim obj As Object = commconcess.ExecuteScalar()



                    'conn.Close()
                    Dim value As Integer
                    value = CInt(obj)
                    Dim concessionnum As String = value.ToString("0000")
                    Dim builder As New StringBuilder
                    Dim Prefix As String = "ELF"
                    builder.Append(Prefix)
                    builder.Append(concessionnum)
                    Dim s As String = builder.ToString
                    commconcess = New SqlCommand("Update FaultIDTable SET ConcessionNumber=@ConcessionNumber WHERE IncidentID=@incidentID ", conn)
                    commconcess.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 10)
                    commconcess.Parameters("@ConcessionNumber").Value = s
                    commconcess.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    commconcess.Parameters("@incidentID").Value = IncidentID
                    'conn.Open()
                    commconcess.ExecuteNonQuery()
                    'objTransaction.Commit()
                    'Catch ex As Exception
                    '    objTransaction.Rollback()
                    '    Dim message As String = String.Format("Message: {0}\n\n", ex.Message)

                    '    message &= String.Format("StackTrace: {0}\n\n", ex.StackTrace.Replace(Environment.NewLine, String.Empty))

                    '    message &= String.Format("Source: {0}\n\n", ex.Source.Replace(Environment.NewLine, String.Empty))

                    '    message &= String.Format("TargetSite: {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, String.Empty))


                Finally
                    conn.Close()
                End Try



            End If

            Return exists
        End Function

        Public Shared Function UpdateTracking(ByVal TrackingComment As String, ByVal Assigned As String, ByVal Status As String, ByVal UserInfo As String, ByVal LinacName As String, ByVal Action As String, ByVal IncidentID As Integer) As String
            Dim time As DateTime = Now()
            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
            conn = New SqlConnection(connectionString)
            Dim trackingID As Integer = 0
            Dim commtrack As SqlCommand
            commtrack = New SqlCommand("Insert into FaultTracking (Trackingcomment, AssignedTo, Status, LastupdatedBy, Lastupdatedon,   linac, action, incidentID) " _
                                      & "VALUES (@Trackingcomment, @AssignedTo, @Status, @LastupdatedBy, @Lastupdatedon,  @linac, @action, @IncidentID) SELECT SCOPE_IDENTITY()", conn)
            commtrack.Parameters.Add("@Trackingcomment", System.Data.SqlDbType.NVarChar, 250)
            commtrack.Parameters("@Trackingcomment").Value = TrackingComment
            commtrack.Parameters.Add("@AssignedTo", Data.SqlDbType.NVarChar, 50)
            commtrack.Parameters("@AssignedTo").Value = Assigned
            commtrack.Parameters.Add("@Status", Data.SqlDbType.NVarChar, 50)
            commtrack.Parameters("@Status").Value = Status
            commtrack.Parameters.Add("@LastupdatedBy", System.Data.SqlDbType.NVarChar, 50)
            commtrack.Parameters("@LastupdatedBy").Value = UserInfo
            commtrack.Parameters.Add("@Lastupdatedon", System.Data.SqlDbType.DateTime)
            commtrack.Parameters("@Lastupdatedon").Value = time

            commtrack.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
            commtrack.Parameters("@linac").Value = LinacName
            commtrack.Parameters.Add("@action", System.Data.SqlDbType.NVarChar, 250)
            commtrack.Parameters("@action").Value = Action
            commtrack.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
            commtrack.Parameters("@incidentID").Value = IncidentID

            conn.Open()
            Dim tobj As Object = commtrack.ExecuteScalar()
            trackingID = CInt(tobj)
            conn.Close()
            Return trackingID
        End Function

        Public Shared Function InsertNewFault(ByVal LinacName As String, ByVal DateInserted As DateTime) As Integer

            Dim IncidentID As Integer
            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
            conn = New SqlConnection(connectionString)
            Dim incidentfault As SqlCommand
            Dim ConcessionNumber = ""
            Dim logInStatusID As Integer = 0
            Dim constateid As SqlCommand
            constateid = New SqlCommand("SELECT stateid FROM [LinacStatus] where stateID = (Select max(stateID) as lastrecord from [LinacStatus] where linac=@linac)", conn)
            constateid.Parameters.AddWithValue("@linac", LinacName)
            Dim readers As SqlDataReader
            conn.Open()
            readers = constateid.ExecuteReader()

            If readers.Read() Then
                logInStatusID = readers.Item("stateid")
            End If
            readers.Close()
            conn.Close()
            incidentfault = New SqlCommand("INSERT INTO FaultIDTable (DateInserted, Linac, Status, originalFaultID, ConcessionNumber, StatusID) VALUES (@DateInserted, @Linac, @Status, @originalFaultID, @ConcessionNumber, @StatusID) SELECT SCOPE_IDENTITY()", conn)
            incidentfault.Parameters.Add("@DateInserted", System.Data.SqlDbType.DateTime)
            incidentfault.Parameters("@DateInserted").Value = DateInserted
            'incidentfault.Parameters("@DateInserted").Value = time
            incidentfault.Parameters.Add("@Status", System.Data.SqlDbType.NVarChar, 20)
            incidentfault.Parameters("@Status").Value = "New"
            incidentfault.Parameters.Add("@originalFaultID", System.Data.SqlDbType.Int)
            incidentfault.Parameters("@originalFaultID").Value = 0
            incidentfault.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 10)
            incidentfault.Parameters("@ConcessionNumber").Value = ConcessionNumber
            incidentfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
            incidentfault.Parameters("@Linac").Value = LinacName
            incidentfault.Parameters.Add("@StatusID", System.Data.SqlDbType.Int)
            incidentfault.Parameters("@StatusID").Value = logInStatusID
            conn.Open()

            Dim NewIncident As Object = incidentfault.ExecuteScalar()
            IncidentID = CInt(NewIncident)
            conn.Close()

            Return IncidentID
        End Function

        Public Overloads Shared Sub UpdateFaultIDTable(ByVal IncidentId As Integer, ByVal LastFault As Integer)

            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
            conn = New SqlConnection(connectionString)
            Dim incidentfault As SqlCommand
            incidentfault = New SqlCommand("Update FaultIDTable SET originalFaultID=@originalFaultID where incidentID=@incidentID", conn)
            incidentfault.Parameters.Add("@originalFaultID", Data.SqlDbType.Int)
            incidentfault.Parameters("@originalFaultID").Value = LastFault
            incidentfault.Parameters.Add("@incidentID", Data.SqlDbType.Int)
            incidentfault.Parameters("@incidentID").Value = IncidentID
            conn.Open()
            incidentfault.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Overloads Shared Sub UpdateFaultIDTable(ByVal IncidentID As Integer, ByVal Status As String, ByVal LinacName As String)

            Dim ObjTransaction As SqlTransaction
            ObjTransaction = Nothing
            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
            conn = New SqlConnection(connectionString)
            Dim updatefault As SqlCommand
            'If Status = "Concession" Then
            'updatefault = New SqlCommand("UPDATE  FaultIDTable SET ReportClosed = @ReportClosed, Status = 'Concession' where IncidentID=@incidentID", conn, ObjTransaction)
            'Else
            '    updatefault = New SqlCommand("UPDATE  FaultIDTable SET ReportClosed = @ReportClosed, Status = 'Closed', DateClosed = @DateClosed where IncidentID=@incidentID", conn)
            '    updatefault.Parameters.Add("@DateClosed", System.Data.SqlDbType.DateTime)
            '    updatefault.Parameters("@DateClosed").Value = Now()
            'End If
            'updatefault.Parameters.Add("@ReportClosed", System.Data.SqlDbType.DateTime)
            'updatefault.Parameters("@ReportClosed").Value = Now()
            'updatefault.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
            'updatefault.Parameters("@linac").Value = LinacName
            'updatefault.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
            'updatefault.Parameters("@incidentID").Value = IncidentID
            Try
                conn.Open()
                ObjTransaction = conn.BeginTransaction()
                updatefault = New SqlCommand("UPDATE  FaultIDTable SET ReportClosed = @ReportClosed, Status = 'Concession' where IncidentID=@incidentID", conn, ObjTransaction)
                updatefault.Parameters.Add("@ReportClosed", System.Data.SqlDbType.DateTime)
                updatefault.Parameters("@ReportClosed").Value = Now()
                updatefault.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
                updatefault.Parameters("@linac").Value = LinacName
                updatefault.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                updatefault.Parameters("@incidentID").Value = IncidentID
                updatefault.ExecuteNonQuery()
                ObjTransaction.Commit()
            Catch ex As Exception
                objTransaction.Rollback()
                Dim message As String = String.Format("Message: {0}\n\n", ex.Message)

                message &= String.Format("StackTrace: {0}\n\n", ex.StackTrace.Replace(Environment.NewLine, String.Empty))

                message &= String.Format("Source: {0}\n\n", ex.Source.Replace(Environment.NewLine, String.Empty))

                message &= String.Format("TargetSite: {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, String.Empty))
            Finally
                conn.Close()
            End Try
        End Sub

        Public Shared Sub WriteRadAckFault(ByVal IncidentID As Integer, ByVal Ack As Boolean)
            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
            conn = New SqlConnection(connectionString)
            Dim commack As SqlCommand
            commack = New SqlCommand("Insert into RadAckFault (IncidentID,TrackingID, Acknowledge) VALUES (@IncidentID,@TrackingID,@Acknowledge)", conn)
            commack.Parameters.Add("@IncidentID", Data.SqlDbType.Int)
            commack.Parameters("@IncidentID").Value = IncidentID
            commack.Parameters.Add("@TrackingID", System.Data.SqlDbType.Int)
            commack.Parameters("@TrackingID").Value = 0
            commack.Parameters.Add("@Acknowledge", Data.SqlDbType.Bit)
            commack.Parameters("@Acknowledge").Value = Ack
            Try
                conn.Open()
                commack.ExecuteNonQuery()
            Finally
                conn.Close()

            End Try
        End Sub
    End Class
End Namespace

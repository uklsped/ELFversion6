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
            Dim LastFault As Integer = IncidentID
            Dim conn As SqlConnection
            'Dim incidentfault As SqlCommand
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
            conn = New SqlConnection(connectionString)

            Using (conn)

                Dim incidentfault As New SqlCommand With {
                    .Connection = conn
                }

                Dim ObjTransaction As SqlTransaction
                ObjTransaction = Nothing

                Try
                    conn.Open()
                    ObjTransaction = conn.BeginTransaction()
                    incidentfault.CommandText = "usp_ReportFault"
                    incidentfault.CommandType = CommandType.StoredProcedure
                    incidentfault.Transaction = ObjTransaction
                    incidentfault.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar, 250)
                    incidentfault.Parameters("@Description").Value = Description
                    incidentfault.Parameters.Add("@ReportedBy", System.Data.SqlDbType.NVarChar, 50)
                    incidentfault.Parameters("@ReportedBy").Value = ReportedBy
                    incidentfault.Parameters.Add("@DateReported", System.Data.SqlDbType.DateTime)
                    incidentfault.Parameters("@DateReported").Value = DateReported
                    incidentfault.Parameters.Add("@Area", System.Data.SqlDbType.NVarChar, 20)
                    incidentfault.Parameters("@Area").Value = Area
                    incidentfault.Parameters.Add("@Energy", System.Data.SqlDbType.NVarChar, 10)
                    incidentfault.Parameters("@Energy").Value = Energy
                    incidentfault.Parameters.Add("@GantryAngle", System.Data.SqlDbType.NVarChar, 3)
                    incidentfault.Parameters("@GantryAngle").Value = GantryAngle
                    incidentfault.Parameters.Add("@CollimatorAngle", System.Data.SqlDbType.NVarChar, 3)
                    incidentfault.Parameters("@CollimatorAngle").Value = CollimatorAngle
                    incidentfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                    incidentfault.Parameters("@Linac").Value = Device
                    incidentfault.Parameters.Add("@IncidentID", System.Data.SqlDbType.Int)
                    incidentfault.Parameters("@IncidentID").Value = IncidentID
                    incidentfault.Parameters.Add("@BSUHID", System.Data.SqlDbType.VarChar, 7)
                    incidentfault.Parameters("@BSUHID").Value = PatientID
                    incidentfault.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 25)
                    incidentfault.Parameters("@ConcessionNumber").Value = ConcessionNumber
                    incidentfault.Parameters.Add("@OriginalFaultID", System.Data.SqlDbType.Int)
                    incidentfault.Parameters("@OriginalFaultID").Value = -1
                    incidentfault.ExecuteNonQuery()
                    incidentfault.Parameters.Clear()

                    ObjTransaction.Commit()

                Catch ex As Exception

                    ObjTransaction.Rollback()
                    Dim message As String = String.Format("Message: {0}\n\n", ex.Message)

                    message &= String.Format("StackTrace: {0}\n\n", ex.StackTrace.Replace(Environment.NewLine, String.Empty))

                    message &= String.Format("Source: {0}\n\n", ex.Source.Replace(Environment.NewLine, String.Empty))

                    message &= String.Format("TargetSite: {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, String.Empty))

                Finally
                    incidentfault.Parameters.Clear()
                    conn.Close()

                End Try
            End Using
            Return LastFault
        End Function

        Public Shared Function InsertNewConcession(ByVal ConcessionDescription As String, ByVal LinacName As String, ByVal IncidentID As Integer, ByVal ReportedBy As String, ByVal ConcessionAction As String) As String

            Dim time As DateTime = Now()
            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
            conn = New SqlConnection(connectionString)

            Dim bcommand = New SqlCommand("select count(*) from Concessiontable where incidentID=@incidentID", conn)
            bcommand.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
            bcommand.Parameters("@incidentID").Value = IncidentID

            conn.Open()

            Dim exists As Integer
            exists = bcommand.ExecuteScalar()
            conn.Close()
            Using (conn)
                Dim incidentfault As New SqlCommand With {
                    .Connection = conn
                }

                Dim ObjTransaction As SqlTransaction
                ObjTransaction = Nothing
                Try
                    conn.Open()
                    ObjTransaction = conn.BeginTransaction()
                    If exists = 0 Then
                        'commconcess.ExecuteNonQuery()
                        'from http://www.dotnetperls.com/string-format-vbnet

                        incidentfault.CommandText = "usp_CreateNewConcession"
                        incidentfault.CommandType = CommandType.StoredProcedure
                        incidentfault.Parameters.Add("@ConcessionDescription", System.Data.SqlDbType.NVarChar, 25)
                        incidentfault.Parameters("@ConcessionDescription").Value = ConcessionDescription
                        incidentfault.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                        incidentfault.Parameters("@incidentID").Value = IncidentID
                        incidentfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                        incidentfault.Parameters("@Linac").Value = LinacName
                        incidentfault.Parameters.Add("@ConcessionActive", System.Data.SqlDbType.Bit)
                        incidentfault.Parameters("@ConcessionActive").Value = 1
                        incidentfault.Parameters.Add("@Action", System.Data.SqlDbType.NVarChar, 250)
                        incidentfault.Parameters("@Action").Value = ConcessionAction
                        incidentfault.Parameters.Add("@ReportedBy", System.Data.SqlDbType.NVarChar, 250)
                        incidentfault.Parameters("@ReportedBy").Value = ReportedBy
                        incidentfault.Parameters.Add("@Lastupdatedon", System.Data.SqlDbType.DateTime)
                        incidentfault.Parameters("@Lastupdatedon").Value = time
                        Dim outPutParameter = New SqlParameter With {
                        .ParameterName = "@TrackingNum",
                        .SqlDbType = System.Data.SqlDbType.Int,
                        .Direction = System.Data.ParameterDirection.Output
                        }
                        incidentfault.Parameters.Add(outPutParameter)
                        incidentfault.ExecuteNonQuery()
                        ObjTransaction.Commit()
                        incidentfault.Parameters.Clear()

                    End If
                Catch ex As Exception
                    ObjTransaction.Rollback()
                    Dim message As String = String.Format("Message: {0}\n\n", ex.Message)

                    message &= String.Format("StackTrace: {0}\n\n", ex.StackTrace.Replace(Environment.NewLine, String.Empty))

                    message &= String.Format("Source: {0}\n\n", ex.Source.Replace(Environment.NewLine, String.Empty))

                    message &= String.Format("TargetSite: {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, String.Empty))


                Finally
                    conn.Close()
                End Try
            End Using

            Return exists
        End Function

        Public Shared Function UpdateTracking(ByVal TrackingComment As String, ByVal Assigned As String, ByVal Status As String, ByVal ReportedBy As String, ByVal LinacName As String, ByVal Action As String, ByVal IncidentID As Integer, ByVal concess As Integer) As String
            Dim time As DateTime = Now()
            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
            conn = New SqlConnection(connectionString)
            Dim trackingID As Integer = 0
            Using (conn)

                Dim incidentfault As New SqlCommand With {
                    .Connection = conn
                }

                Dim ObjTransaction As SqlTransaction
                ObjTransaction = Nothing

                Try
                    conn.Open()
                    ObjTransaction = conn.BeginTransaction()
                    incidentfault.CommandText = "usp_ClassicFaultTracking"
                    incidentfault.CommandType = CommandType.StoredProcedure
                    incidentfault.Transaction = ObjTransaction
                    incidentfault.Parameters.Add("@Trackingcomment", System.Data.SqlDbType.NVarChar, 250)
                    incidentfault.Parameters("@Trackingcomment").Value = TrackingComment
                    incidentfault.Parameters.Add("@AssignedTo", Data.SqlDbType.NVarChar, 50)
                    incidentfault.Parameters("@AssignedTo").Value = Assigned
                    incidentfault.Parameters.Add("@Status", Data.SqlDbType.NVarChar, 50)
                    incidentfault.Parameters("@Status").Value = Status
                    incidentfault.Parameters.Add("@LastupdatedBy", System.Data.SqlDbType.NVarChar, 50)
                    incidentfault.Parameters("@LastupdatedBy").Value = ReportedBy
                    incidentfault.Parameters.Add("@Lastupdatedon", System.Data.SqlDbType.DateTime)
                    incidentfault.Parameters("@Lastupdatedon").Value = time
                    incidentfault.Parameters.Add("@linacName", System.Data.SqlDbType.NVarChar, 10)
                    incidentfault.Parameters("@linacName").Value = LinacName
                    incidentfault.Parameters.Add("@action", System.Data.SqlDbType.NVarChar, 250)
                    incidentfault.Parameters("@action").Value = Action
                    incidentfault.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    incidentfault.Parameters("@incidentID").Value = IncidentID
                    incidentfault.Parameters.Add("@concess", System.Data.SqlDbType.Int)
                    incidentfault.Parameters("@concess").Value = concess
                    Dim outPutParameter = New SqlParameter With {
                        .ParameterName = "@TrackingID",
                        .SqlDbType = System.Data.SqlDbType.Int,
                        .Direction = System.Data.ParameterDirection.Output
                        }
                    incidentfault.Parameters.Add(outPutParameter)
                    incidentfault.ExecuteNonQuery()
                    Dim NewTrackID As String = outPutParameter.Value.ToString
                    trackingID = CInt(NewTrackID)
                    incidentfault.Parameters.Clear()
                    outPutParameter.ParameterName = Nothing

                    incidentfault.CommandText = "usp_SetRadAckFault"
                    incidentfault.CommandType = CommandType.StoredProcedure
                    incidentfault.Transaction = ObjTransaction
                    incidentfault.Parameters.Add("@IncidentID", Data.SqlDbType.Int)
                    incidentfault.Parameters("@IncidentID").Value = IncidentID
                    incidentfault.Parameters.Add("@TrackingID", System.Data.SqlDbType.Int)
                    incidentfault.Parameters("@TrackingID").Value = trackingID
                    incidentfault.Parameters.Add("@Acknowledge", Data.SqlDbType.Bit)
                    incidentfault.Parameters("@Acknowledge").Value = True
                    incidentfault.ExecuteNonQuery()
                    incidentfault.Parameters.Clear()



                    ObjTransaction.Commit()


                Catch ex As Exception

                    ObjTransaction.Rollback()
                    Dim message As String = String.Format("Message: {0}\n\n", ex.Message)

                    message &= String.Format("StackTrace: {0}\n\n", ex.StackTrace.Replace(Environment.NewLine, String.Empty))

                    message &= String.Format("Source: {0}\n\n", ex.Source.Replace(Environment.NewLine, String.Empty))

                    message &= String.Format("TargetSite: {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, String.Empty))

                Finally
                    conn.Close()
                End Try

            End Using


            Return trackingID
        End Function

        Public Shared Function InsertNewFault(ByVal State As String, ByVal LinacName As String, ByVal DateInserted As DateTime, ByVal Description As String, ByVal ReportedBy As String, ByVal Area As String, ByVal Energy As String, ByVal GantryAngle As String, ByVal CollimatorAngle As String, ByVal PatientID As String, ByVal ConcessionDescription As String, ConcessionAction As String) As Integer
            Dim time As DateTime = Now()
            Dim IncidentID As Integer = 0
            Dim LastFault As Integer = 0
            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
            conn = New SqlConnection(connectionString)
            'Dim incidentfault As SqlCommand
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

            Using (conn)

                Dim incidentfault As New SqlCommand With {
                    .Connection = conn
                }

                Dim ObjTransaction As SqlTransaction
                ObjTransaction = Nothing

                Try
                    conn.Open()
                    ObjTransaction = conn.BeginTransaction()
                    incidentfault.CommandText = "usp_InsertNewFault"
                    incidentfault.CommandType = CommandType.StoredProcedure
                    incidentfault.Transaction = ObjTransaction

                    incidentfault.Parameters.Add("@DateInserted", System.Data.SqlDbType.DateTime)
                    incidentfault.Parameters("@DateInserted").Value = DateInserted
                    incidentfault.Parameters.Add("@Status", System.Data.SqlDbType.NVarChar, 20)
                    incidentfault.Parameters("@Status").Value = State
                    incidentfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                    incidentfault.Parameters("@Linac").Value = LinacName
                    incidentfault.Parameters.Add("@StateID", System.Data.SqlDbType.Int)
                    incidentfault.Parameters("@StateID").Value = logInStatusID

                    Dim outPutParameter = New SqlParameter With {
                        .ParameterName = "@IncidentID",
                        .SqlDbType = System.Data.SqlDbType.Int,
                        .Direction = System.Data.ParameterDirection.Output
                    }
                    incidentfault.Parameters.Add(outPutParameter)

                    incidentfault.ExecuteNonQuery()
                    Dim NewIncident As String = outPutParameter.Value.ToString
                    IncidentID = CInt(NewIncident)

                    incidentfault.Parameters.Clear()
                    outPutParameter.ParameterName = Nothing

                    incidentfault.CommandText = "usp_ReportFault"
                    incidentfault.CommandType = CommandType.StoredProcedure
                    incidentfault.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar, 250)
                    incidentfault.Parameters("@Description").Value = Description
                    incidentfault.Parameters.Add("@ReportedBy", System.Data.SqlDbType.NVarChar, 50)
                    incidentfault.Parameters("@ReportedBy").Value = ReportedBy
                    incidentfault.Parameters.Add("@DateReported", System.Data.SqlDbType.DateTime)
                    incidentfault.Parameters("@DateReported").Value = DateInserted
                    incidentfault.Parameters.Add("@Area", System.Data.SqlDbType.NVarChar, 20)
                    incidentfault.Parameters("@Area").Value = Area
                    incidentfault.Parameters.Add("@Energy", System.Data.SqlDbType.NVarChar, 10)
                    incidentfault.Parameters("@Energy").Value = Energy
                    incidentfault.Parameters.Add("@GantryAngle", System.Data.SqlDbType.NVarChar, 3)
                    incidentfault.Parameters("@GantryAngle").Value = GantryAngle
                    incidentfault.Parameters.Add("@CollimatorAngle", System.Data.SqlDbType.NVarChar, 3)
                    incidentfault.Parameters("@CollimatorAngle").Value = CollimatorAngle
                    incidentfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                    incidentfault.Parameters("@Linac").Value = LinacName
                    incidentfault.Parameters.Add("@IncidentID", System.Data.SqlDbType.Int)
                    incidentfault.Parameters("@IncidentID").Value = IncidentID
                    incidentfault.Parameters.Add("@BSUHID", System.Data.SqlDbType.VarChar, 7)
                    incidentfault.Parameters("@BSUHID").Value = PatientID
                    incidentfault.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 25)
                    incidentfault.Parameters("@ConcessionNumber").Value = ConcessionNumber
                    incidentfault.Parameters.Add("@OriginalFaultID", System.Data.SqlDbType.Int)
                    incidentfault.Parameters("@OriginalFaultID").Value = 0

                    incidentfault.ExecuteNonQuery()

                    incidentfault.Parameters.Clear()

                    incidentfault.CommandText = "usp_NewFaultTracking"
                    incidentfault.CommandType = CommandType.StoredProcedure
                    'incidentfault.Parameters.Add("@Trackingcomment", System.Data.SqlDbType.NVarChar, 250)
                    'incidentfault.Parameters("@Trackingcomment").Value = TrackingComment
                    'incidentfault.Parameters.Add("@AssignedTo", Data.SqlDbType.NVarChar, 50)
                    'incidentfault.Parameters("@AssignedTo").Value = Assigned
                    incidentfault.Parameters.Add("@Status", Data.SqlDbType.NVarChar, 50)
                    incidentfault.Parameters("@Status").Value = "New"
                    incidentfault.Parameters.Add("@LastupdatedBy", System.Data.SqlDbType.NVarChar, 50)
                    incidentfault.Parameters("@LastupdatedBy").Value = ReportedBy
                    incidentfault.Parameters.Add("@Lastupdatedon", System.Data.SqlDbType.DateTime)
                    incidentfault.Parameters("@Lastupdatedon").Value = DateInserted
                    incidentfault.Parameters.Add("@linacName", System.Data.SqlDbType.NVarChar, 10)
                    incidentfault.Parameters("@linacName").Value = LinacName
                    'incidentfault.Parameters.Add("@action", System.Data.SqlDbType.NVarChar, 250)
                    'incidentfault.Parameters("@action").Value = Action
                    incidentfault.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    incidentfault.Parameters("@incidentID").Value = IncidentID
                    incidentfault.ExecuteNonQuery()
                    incidentfault.Parameters.Clear()

                    Select Case State
                        Case "New"
                            'No further action except to commit transaction
                        Case "Concession"
                            incidentfault.CommandText = "usp_CreateNewConcession"
                            incidentfault.CommandType = CommandType.StoredProcedure
                            incidentfault.Parameters.Add("@ConcessionDescription", System.Data.SqlDbType.NVarChar, 25)
                            incidentfault.Parameters("@ConcessionDescription").Value = ConcessionDescription
                            incidentfault.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                            incidentfault.Parameters("@incidentID").Value = IncidentID
                            incidentfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                            incidentfault.Parameters("@Linac").Value = LinacName
                            incidentfault.Parameters.Add("@ConcessionActive", System.Data.SqlDbType.Bit)
                            incidentfault.Parameters("@ConcessionActive").Value = 1
                            incidentfault.Parameters.Add("@Action", System.Data.SqlDbType.NVarChar, 250)
                            incidentfault.Parameters("@Action").Value = ConcessionAction
                            incidentfault.Parameters.Add("@ReportedBy", System.Data.SqlDbType.NVarChar, 250)
                            incidentfault.Parameters("@ReportedBy").Value = ReportedBy
                            incidentfault.Parameters.Add("@Lastupdatedon", System.Data.SqlDbType.DateTime)
                            incidentfault.Parameters("@Lastupdatedon").Value = time
                            outPutParameter.ParameterName = "@TrackingNum"
                            outPutParameter.SqlDbType = System.Data.SqlDbType.Int
                            outPutParameter.Direction = System.Data.ParameterDirection.Output
                            incidentfault.Parameters.Add(outPutParameter)
                            incidentfault.ExecuteNonQuery()

                            incidentfault.Parameters.Clear()
                            incidentfault.CommandText = "usp_SetRadAckFault"
                            incidentfault.Parameters.Add("@IncidentID", Data.SqlDbType.Int)
                            incidentfault.Parameters("@IncidentID").Value = IncidentID
                            incidentfault.Parameters.Add("@TrackingID", System.Data.SqlDbType.Int)
                            incidentfault.Parameters("@TrackingID").Value = 0
                            incidentfault.Parameters.Add("@Acknowledge", Data.SqlDbType.Bit)
                            incidentfault.Parameters("@Acknowledge").Value = False
                            incidentfault.ExecuteNonQuery()
                            incidentfault.Parameters.Clear()
                        Case "Closed"
                            incidentfault.CommandText = "usp_NewFaultTracking"
                            incidentfault.CommandType = CommandType.StoredProcedure
                            incidentfault.Parameters.Add("@Status", Data.SqlDbType.NVarChar, 50)
                            incidentfault.Parameters("@Status").Value = "Closed"
                            incidentfault.Parameters.Add("@LastupdatedBy", System.Data.SqlDbType.NVarChar, 50)
                            incidentfault.Parameters("@LastupdatedBy").Value = ReportedBy
                            incidentfault.Parameters.Add("@Lastupdatedon", System.Data.SqlDbType.DateTime)
                            incidentfault.Parameters("@Lastupdatedon").Value = time
                            incidentfault.Parameters.Add("@linacName", System.Data.SqlDbType.NVarChar, 10)
                            incidentfault.Parameters("@linacName").Value = LinacName
                            incidentfault.Parameters.Add("@action", System.Data.SqlDbType.NVarChar, 250)
                            incidentfault.Parameters("@action").Value = ConcessionAction
                            incidentfault.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                            incidentfault.Parameters("@incidentID").Value = IncidentID
                            incidentfault.ExecuteNonQuery()
                            incidentfault.Parameters.Clear()

                    End Select

                    incidentfault.Parameters.Clear()
                    ObjTransaction.Commit()

                Catch ex As Exception

                    ObjTransaction.Rollback()
                    Dim message As String = String.Format("Message: {0}\n\n", ex.Message)

                    message &= String.Format("StackTrace: {0}\n\n", ex.StackTrace.Replace(Environment.NewLine, String.Empty))

                    message &= String.Format("Source: {0}\n\n", ex.Source.Replace(Environment.NewLine, String.Empty))

                    message &= String.Format("TargetSite: {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, String.Empty))

                Finally
                    conn.Close()
                End Try

            End Using
            Return IncidentID
        End Function


    End Class


End Namespace

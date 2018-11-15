Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.IO
Imports System.Net.Mail

Namespace DavesCode

    Public Class NewFaultHandling
        'used in Viewopenfaults, DeviceRepeatfault, DeviceSave,DeviceSavePark
        Public Shared Function InsertRepeatFault(ByVal Description As String, ByVal ReportedBy As String, ByVal DateReported As DateTime, ByVal Area As String, ByVal Energy As String, ByVal GantryAngle As String, ByVal CollimatorAngle As String, ByVal Device As String, ByVal IncidentID As Integer, ByVal PatientID As String, ByVal ConcessionNumber As String, ByVal Reportable As Boolean) As Integer
            Dim LastFault As Integer = IncidentID
            Dim conn As SqlConnection
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
                    incidentfault.Parameters.Add("@RadiationIncident", System.Data.SqlDbType.Bit)
                    incidentfault.Parameters("@RadiationIncident").Value = Reportable
                    incidentfault.ExecuteNonQuery()
                    incidentfault.Parameters.Clear()

                    ObjTransaction.Commit()


                Catch ex As Exception

                    ObjTransaction.Rollback()
                    LogError(ex)
                    LastFault = -1
                Finally
                    incidentfault.Parameters.Clear()
                    conn.Close()

                End Try
            End Using
            Return LastFault
        End Function
        'Used in ViewOpenFaults to change to concession or to update concession
        Public Shared Function InsertNewConcession(ByVal ConcessionDescription As String, ByVal LinacName As String, ByVal IncidentID As Integer, ByVal ReportedBy As String, ByVal ConcessionAction As String) As Integer
            Dim Countcommandtext As String = "select count(*) from Concessiontable where incidentID=@incidentID"
            Dim Getfaultstatus As String = "select Status From FaultIDTable where incidentID = @incidentID"
            Dim Status As String
            Dim exists As Integer
            Dim time As DateTime = Now()
            Dim conn As SqlConnection
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
                    incidentfault.CommandText = Getfaultstatus
                    incidentfault.CommandType = CommandType.Text
                    incidentfault.Transaction = ObjTransaction
                    incidentfault.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    incidentfault.Parameters("@incidentID").Value = IncidentID
                    Status = CType(incidentfault.ExecuteScalar(), String)
                    incidentfault.Parameters.Clear()

                    incidentfault.CommandText = Countcommandtext
                    incidentfault.CommandType = CommandType.Text
                    incidentfault.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                    incidentfault.Parameters("@incidentID").Value = IncidentID
                    exists = CInt(incidentfault.ExecuteScalar())
                    incidentfault.Parameters.Clear()

                    If (exists = 0) And ((Status = "Open") Or (Status = "New")) Then
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

                    ElseIf (((exists = 0) And (Status = "Concession")) Or ((exists = 1) And (Not Status = "Concession"))) Then
                        exists = -1
                        ObjTransaction.Rollback()
                        LogAnomaly(LinacName, "InsertNewConcession", "Error exists = 0 but Status = Concession")
                    End If
                Catch ex As Exception
                    ObjTransaction.Rollback()
                    exists = -1
                    LogError(ex)

                Finally
                    conn.Close()
                End Try
            End Using

            Return exists
        End Function
        'Used to update tracking in ViewOpenFaults
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
                    LogError(ex)
                    trackingID = -1
                Finally
                    conn.Close()
                End Try

            End Using


            Return trackingID
        End Function
        'Public Shared Function InsertNewFault(ByVal State As String, ByVal LinacName As String, ByVal DateInserted As DateTime, ByVal Description As String, ByVal ReportedBy As String, ByVal Area As String, ByVal Energy As String, ByVal GantryAngle As String, ByVal CollimatorAngle As String, ByVal PatientID As String, ByVal ConcessionDescription As String, ByVal ConcessionAction As String, ByVal RadiationIncident As Boolean) As Integer
        Public Shared Function InsertNewFault(ByVal State As String, ByVal FaultP As DavesCode.FaultParameters) As Integer
            Dim time As DateTime = Now()
            Dim IncidentID As Integer = 0
            Dim LastFault As Integer = 0
            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
            conn = New SqlConnection(connectionString)
            Dim ConcessionNumber = ""
            Dim logInStatusID As Integer = 0
            Dim constateid As SqlCommand
            constateid = New SqlCommand("SELECT stateid FROM [LinacStatus] where stateID = (Select max(stateID) as lastrecord from [LinacStatus] where linac=@linac)", conn)
            constateid.Parameters.AddWithValue("@linac", FaultP.Linac)
            Dim readers As SqlDataReader
            conn.Open()
            readers = constateid.ExecuteReader()

            If readers.Read() Then
                logInStatusID = CInt(readers.Item("stateid"))
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
                    incidentfault.Parameters("@DateInserted").Value = FaultP.DateInserted
                    incidentfault.Parameters.Add("@Status", System.Data.SqlDbType.NVarChar, 20)
                    incidentfault.Parameters("@Status").Value = State
                    incidentfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                    incidentfault.Parameters("@Linac").Value = FaultP.Linac
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
                    incidentfault.Parameters("@Description").Value = FaultP.FaultDescription
                    incidentfault.Parameters.Add("@ReportedBy", System.Data.SqlDbType.NVarChar, 50)
                    incidentfault.Parameters("@ReportedBy").Value = FaultP.UserInfo
                    incidentfault.Parameters.Add("@DateReported", System.Data.SqlDbType.DateTime)
                    incidentfault.Parameters("@DateReported").Value = FaultP.DateInserted
                    incidentfault.Parameters.Add("@Area", System.Data.SqlDbType.NVarChar, 20)
                    incidentfault.Parameters("@Area").Value = FaultP.Area
                    incidentfault.Parameters.Add("@Energy", System.Data.SqlDbType.NVarChar, 10)
                    incidentfault.Parameters("@Energy").Value = FaultP.Energy
                    incidentfault.Parameters.Add("@GantryAngle", System.Data.SqlDbType.NVarChar, 3)
                    incidentfault.Parameters("@GantryAngle").Value = FaultP.GantryAngle
                    incidentfault.Parameters.Add("@CollimatorAngle", System.Data.SqlDbType.NVarChar, 3)
                    incidentfault.Parameters("@CollimatorAngle").Value = FaultP.CollimatorAngle
                    incidentfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                    incidentfault.Parameters("@Linac").Value = FaultP.Linac
                    incidentfault.Parameters.Add("@IncidentID", System.Data.SqlDbType.Int)
                    incidentfault.Parameters("@IncidentID").Value = IncidentID
                    incidentfault.Parameters.Add("@BSUHID", System.Data.SqlDbType.VarChar, 7)
                    incidentfault.Parameters("@BSUHID").Value = FaultP.PatientID
                    incidentfault.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 25)
                    incidentfault.Parameters("@ConcessionNumber").Value = ConcessionNumber
                    incidentfault.Parameters.Add("@OriginalFaultID", System.Data.SqlDbType.Int)
                    incidentfault.Parameters("@OriginalFaultID").Value = 0
                    incidentfault.Parameters.Add("@RadiationIncident", System.Data.SqlDbType.Bit)
                    incidentfault.Parameters("@RadiationIncident").Value = FaultP.RadioIncident
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
                    incidentfault.Parameters("@LastupdatedBy").Value = FaultP.UserInfo
                    incidentfault.Parameters.Add("@Lastupdatedon", System.Data.SqlDbType.DateTime)
                    incidentfault.Parameters("@Lastupdatedon").Value = FaultP.DateInserted
                    incidentfault.Parameters.Add("@linacName", System.Data.SqlDbType.NVarChar, 10)
                    incidentfault.Parameters("@linacName").Value = FaultP.Linac
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
                            incidentfault.Parameters("@ConcessionDescription").Value = FaultP.FaultDescription
                            incidentfault.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                            incidentfault.Parameters("@incidentID").Value = IncidentID
                            incidentfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                            incidentfault.Parameters("@Linac").Value = FaultP.Linac
                            incidentfault.Parameters.Add("@ConcessionActive", System.Data.SqlDbType.Bit)
                            incidentfault.Parameters("@ConcessionActive").Value = 1
                            incidentfault.Parameters.Add("@Action", System.Data.SqlDbType.NVarChar, 250)
                            incidentfault.Parameters("@Action").Value = FaultP.RadAct
                            incidentfault.Parameters.Add("@ReportedBy", System.Data.SqlDbType.NVarChar, 250)
                            incidentfault.Parameters("@ReportedBy").Value = FaultP.UserInfo
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
                            incidentfault.Parameters("@LastupdatedBy").Value = FaultP.DateInserted
                            incidentfault.Parameters.Add("@Lastupdatedon", System.Data.SqlDbType.DateTime)
                            incidentfault.Parameters("@Lastupdatedon").Value = time
                            incidentfault.Parameters.Add("@linacName", System.Data.SqlDbType.NVarChar, 10)
                            incidentfault.Parameters("@linacName").Value = FaultP.Linac
                            incidentfault.Parameters.Add("@action", System.Data.SqlDbType.NVarChar, 250)
                            incidentfault.Parameters("@action").Value = FaultP.RadAct
                            incidentfault.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                            incidentfault.Parameters("@incidentID").Value = IncidentID
                            incidentfault.ExecuteNonQuery()
                            incidentfault.Parameters.Clear()

                    End Select

                    incidentfault.Parameters.Clear()
                    ObjTransaction.Commit()

                Catch ex As Exception

                    ObjTransaction.Rollback()
                    LogError(ex)
                Finally
                    conn.Close()
                End Try

            End Using
            Return IncidentID
        End Function

        Public Shared Function InsertMajorFault(FaultP As DavesCode.FaultParameters, ByVal connectionString As String) As Integer
            Const STATE = "New"
            Const ORIGINALFAULTID = 0
            Dim time As DateTime = Now()
            Dim IncidentID As Integer = 0
            Dim LastFault As Integer = 0
            Dim conn As SqlConnection
            'Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
            conn = New SqlConnection(connectionString)
            'Dim incidentfault As SqlCommand
            Const CONCESSIONNUMBER = ""
            Dim logInStatusID As Integer = 0
            Dim constateid As SqlCommand
            constateid = New SqlCommand("SELECT stateid FROM [LinacStatus] where stateID = (Select max(stateID) as lastrecord from [LinacStatus] where linac=@linac)", conn)
            constateid.Parameters.AddWithValue("@linac", FaultP.Linac)
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

                'Dim ObjTransaction As SqlTransaction
                'ObjTransaction = Nothing

                'Try
                conn.Open()
                'ObjTransaction = conn.BeginTransaction()
                incidentfault.CommandText = "usp_InsertNewFault"
                incidentfault.CommandType = CommandType.StoredProcedure
                'incidentfault.Transaction = ObjTransaction

                incidentfault.Parameters.Add("@DateInserted", System.Data.SqlDbType.DateTime)
                incidentfault.Parameters("@DateInserted").Value = FaultP.DateInserted
                incidentfault.Parameters.Add("@Status", System.Data.SqlDbType.NVarChar, 20)
                incidentfault.Parameters("@Status").Value = STATE
                incidentfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                incidentfault.Parameters("@Linac").Value = FaultP.Linac
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
                incidentfault.Parameters("@Description").Value = FaultP.FaultDescription
                incidentfault.Parameters.Add("@ReportedBy", System.Data.SqlDbType.NVarChar, 50)
                incidentfault.Parameters("@ReportedBy").Value = FaultP.UserInfo
                incidentfault.Parameters.Add("@DateReported", System.Data.SqlDbType.DateTime)
                incidentfault.Parameters("@DateReported").Value = FaultP.DateInserted
                incidentfault.Parameters.Add("@Area", System.Data.SqlDbType.NVarChar, 20)
                incidentfault.Parameters("@Area").Value = FaultP.Area
                incidentfault.Parameters.Add("@Energy", System.Data.SqlDbType.NVarChar, 10)
                incidentfault.Parameters("@Energy").Value = FaultP.Energy
                incidentfault.Parameters.Add("@GantryAngle", System.Data.SqlDbType.NVarChar, 3)
                incidentfault.Parameters("@GantryAngle").Value = FaultP.GantryAngle
                incidentfault.Parameters.Add("@CollimatorAngle", System.Data.SqlDbType.NVarChar, 3)
                incidentfault.Parameters("@CollimatorAngle").Value = FaultP.CollimatorAngle
                incidentfault.Parameters.Add("@Linac", System.Data.SqlDbType.NVarChar, 10)
                incidentfault.Parameters("@Linac").Value = FaultP.Linac
                incidentfault.Parameters.Add("@IncidentID", System.Data.SqlDbType.Int)
                incidentfault.Parameters("@IncidentID").Value = IncidentID
                incidentfault.Parameters.Add("@BSUHID", System.Data.SqlDbType.VarChar, 7)
                incidentfault.Parameters("@BSUHID").Value = FaultP.PatientID
                incidentfault.Parameters.Add("@ConcessionNumber", System.Data.SqlDbType.NVarChar, 25)
                incidentfault.Parameters("@ConcessionNumber").Value = CONCESSIONNUMBER
                incidentfault.Parameters.Add("@OriginalFaultID", System.Data.SqlDbType.Int)
                incidentfault.Parameters("@OriginalFaultID").Value = ORIGINALFAULTID
                incidentfault.Parameters.Add("@RadiationIncident", System.Data.SqlDbType.Bit)
                incidentfault.Parameters("@RadiationIncident").Value = FaultP.RadioIncident
                incidentfault.ExecuteNonQuery()

                incidentfault.Parameters.Clear()

                incidentfault.CommandText = "usp_NewFaultTracking"
                incidentfault.CommandType = CommandType.StoredProcedure
                'incidentfault.Parameters.Add("@Trackingcomment", System.Data.SqlDbType.NVarChar, 250)
                'incidentfault.Parameters("@Trackingcomment").Value = TrackingComment
                'incidentfault.Parameters.Add("@AssignedTo", Data.SqlDbType.NVarChar, 50)
                'incidentfault.Parameters("@AssignedTo").Value = Assigned
                incidentfault.Parameters.Add("@Status", Data.SqlDbType.NVarChar, 50)
                incidentfault.Parameters("@Status").Value = STATE
                incidentfault.Parameters.Add("@LastupdatedBy", System.Data.SqlDbType.NVarChar, 50)
                incidentfault.Parameters("@LastupdatedBy").Value = FaultP.UserInfo
                incidentfault.Parameters.Add("@Lastupdatedon", System.Data.SqlDbType.DateTime)
                incidentfault.Parameters("@Lastupdatedon").Value = FaultP.DateInserted
                incidentfault.Parameters.Add("@linacName", System.Data.SqlDbType.NVarChar, 10)
                incidentfault.Parameters("@linacName").Value = FaultP.Linac
                'incidentfault.Parameters.Add("@action", System.Data.SqlDbType.NVarChar, 250)
                'incidentfault.Parameters("@action").Value = Action
                incidentfault.Parameters.Add("@incidentID", System.Data.SqlDbType.Int)
                incidentfault.Parameters("@incidentID").Value = IncidentID
                incidentfault.ExecuteNonQuery()
                incidentfault.Parameters.Clear()



                incidentfault.Parameters.Clear()
                '    ObjTransaction.Commit()

                'Catch ex As Exception

                '    ObjTransaction.Rollback()
                '    LogError(ex)
                'Finally
                '    conn.Close()
                'End Try

            End Using
            Return IncidentID
        End Function

        Public Shared Sub LogError(ex As Exception)
            Dim message As String = String.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"))
            message += Environment.NewLine
            message += "-----------------------------------------------------------"
            message += Environment.NewLine
            message += String.Format("Message: {0}", ex.Message)
            message += Environment.NewLine
            message += String.Format("StackTrace: {0}", ex.StackTrace)
            message += Environment.NewLine
            message += String.Format("Source: {0}", ex.Source)
            message += Environment.NewLine
            message += String.Format("TargetSite: {0}", ex.TargetSite.ToString())
            message += Environment.NewLine
            message += "-----------------------------------------------------------"
            message += Environment.NewLine
            'Dim path As String = System.Web.HttpContext.Current.Server.MapPath("~/ErrorLog/ErrorLog.txt")
            Dim path As String = System.Web.HttpContext.Current.Server.MapPath("~/ErrorLog/")
            If (Not Directory.Exists(path)) Then
                Directory.CreateDirectory(path)
            End If
            Dim shortfilename As String = DateTime.Today.ToString("dd-MM-yy") + ".txt"
            path = path + shortfilename ' Text File Name
            If (Not File.Exists(path)) Then
                File.Create(path).Dispose()
            End If
            Using writer As New StreamWriter(path, True)
                writer.WriteLine(message)
                writer.Close()
            End Using
            'Send email
            'Dim smtpClient As SmtpClient = New SmtpClient()
            'Dim emessage As MailMessage = New MailMessage()

            'Dim fromAddress As New MailAddress("VISIRSERVER@VISIRSERVER.bsuh.nhs.uk", "ELF")
            'Dim toAddress As New MailAddress("david.spendley@bsuh.nhs.uk")
            'emessage.From = fromAddress
            'emessage.To.Add(toAddress)
            'emessage.Subject = "ELF system error"
            'emessage.Body = "Error file name: " + shortfilename
            'smtpClient.Host = "10.216.8.19"
            'smtpClient.Send(emessage)

        End Sub

        Public Shared Sub LogAnomaly(ByVal LinacName As String, ByVal Procedure As String, ByVal Anomaly As String)
            Dim message As String = String.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"))
            message += Environment.NewLine
            message += "-----------------------------------------------------------"
            message += Environment.NewLine
            message += String.Format("Linac: {0}", LinacName)
            message += Environment.NewLine
            message += String.Format("Calling Procedure: {0}", Procedure)
            message += Environment.NewLine
            message += String.Format("Anomaly: {0}", Anomaly)
            message += Environment.NewLine
            message += String.Format("Anomaly occurred on page: {0}", System.Web.HttpContext.Current.Request.Url.ToString)
            message += Environment.NewLine
            message += "-----------------------------------------------------------"
            message += Environment.NewLine
            'Dim path As String = System.Web.HttpContext.Current.Server.MapPath("~/ErrorLog/ErrorLog.txt")
            Dim path As String = System.Web.HttpContext.Current.Server.MapPath("~/ErrorLog/")
            If (Not Directory.Exists(path)) Then
                Directory.CreateDirectory(path)
            End If
            path = path + DateTime.Today.ToString("dd-MM-yy") + ".txt" ' Text File Name
            If (Not File.Exists(path)) Then
                File.Create(path).Dispose()
            End If
            Using writer As New StreamWriter(path, True)
                writer.WriteLine(message)
                writer.Close()
            End Using
        End Sub

    End Class

End Namespace

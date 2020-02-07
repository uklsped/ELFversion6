Imports System.Data.SqlClient
Imports System.Transactions
Namespace DavesCode
    Public Class NewWriteAux
        Public Shared Function WriteAuxTables(ByVal LinacName As String, ByVal LogOffName As String, ByVal comment As String, ByVal Radioselect As Integer, ByVal Tabused As Integer, ByVal NewFault As Boolean, ByVal suspstate As String, ByVal RunUpBoolean As String, ByVal lock As Boolean, ByVal FaultParams As DavesCode.FaultParameters) As Boolean
            Dim Successful As Boolean = False
            Dim NewIncidentID As Integer = 0
            Dim FaultParamsApplication As String
            FaultParamsApplication = "FaultParams" + LinacName
            Try
                Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
                Using myscope As TransactionScope = New TransactionScope()
                    WriteAuxTablesNew(LinacName, LogOffName, comment, Radioselect, Tabused, NewFault, suspstate, RunUpBoolean, lock, connectionString)
                    If NewFault Then
                        NewIncidentID = NewFaultHandling.InsertMajorFault(FaultParams, connectionString)
                        FaultParams.SelectedIncident = NewIncidentID
                        HttpContext.Current.Application(FaultParamsApplication) = FaultParams
                    End If
                    myscope.Complete()
                    Successful = True
                End Using
            Catch ex As Exception
                DavesCode.NewFaultHandling.LogError(ex)
            End Try
            Return Successful
        End Function
        Public Shared Function WriteAuxTablesNew(ByVal LinacName As String, ByVal LogOffName As String, ByVal comment As String, ByVal Radioselect As Integer, ByVal Tabused As Integer, ByVal Fault As Boolean, ByVal suspstate As String, ByVal RunUpBoolean As String, ByVal lock As Boolean, ByVal connectionString As String) As String
            'writes the aux tables depending on the options picked. And writes the linac status table first.
            'When tidying this up look at whether radio is the same as tab used - no it's not
            Dim state As String = "Linac Unauthorised" 'most common value
            Dim time As DateTime
            time = Now()
            Dim LoginStatusID As String = ""
            Dim conn As SqlConnection
            Dim commpm As SqlCommand
            Dim StartTime As DateTime
            Dim LoginName As String = ""
            Dim LogOutStatusID As String = ""
            Dim contime As SqlCommand
            Dim reader As SqlDataReader
            Dim Activity As String = ""
            Dim userreason As Integer
            conn = New SqlConnection(connectionString)


            'Try
            '    Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
            '    Using myscope As TransactionScope = New TransactionScope()

            'had to change this because Fault was getting confused with new fault with new popup.
            contime = New SqlCommand("SELECT stateID,State,DateTime, UserName FROM [LinacStatus] where stateID = (Select max(stateID) as lastrecord from [LinacStatus] where linac=@linac)", conn)
            contime.Parameters.AddWithValue("@linac", LinacName)
            conn.Open()
            reader = contime.ExecuteReader()



            If reader.Read() Then
                StartTime = reader.Item("DateTime")
                LoginName = reader.Item("UserName")
                LoginStatusID = reader.Item("stateID")
                'state = reader.Item("State")
            End If
            reader.Close()
            conn.Close()
            'If Fault Then
            '    state = "Fault"
            'End If



            Select Case Radioselect
                Case 101, 102, 103
                    userreason = Radioselect
                Case Else
                    userreason = 7
            End Select
            If Fault Then
                state = "Fault"
                userreason = 103
            Else
                'Radioselect determines how to SetStatus as a result of which radiobutton selected
                Select Case Radioselect
                    Case 1

                    Case 4, 5, 6, 8, 101
                        If suspstate = 1 Then
                            state = "Suspended"
                        ElseIf RunUpBoolean = 1 Then
                            state = "Engineering Approved"
                        Else

                        End If

                    Case 2
                        state = "Engineering Approved"
                    Case 3
                        state = "Suspended"
                    Case 102


                End Select
            End If

            LogOutStatusID = DavesCode.Reuse.SetStatusNew(LogOffName, state, 5, userreason, LinacName, Tabused, connectionString)
            commpm = New SqlCommand("INSERT INTO AuxTable (Tab,LogInDate, LogOutDate, LogInName, LogOutName, Comment,linac, LogInStatusID, LogOutStatusID ) " &
                                               "VALUES (@Tab, @LogInDate, @LogOutDate, @LogInName, @LogOutName, @Comment,@linac, @LogInStatusID, @LogOutStatusID)", conn)

            commpm.Parameters.Add("@Tab", System.Data.SqlDbType.Int)
            commpm.Parameters("@Tab").Value = Tabused
            commpm.Parameters.Add("@LogInDate", System.Data.SqlDbType.DateTime)
            commpm.Parameters("@LogInDate").Value = StartTime
            commpm.Parameters.Add("@LogOutDate", System.Data.SqlDbType.DateTime)
            commpm.Parameters("@LogOutDate").Value = time
            commpm.Parameters.Add("@LogInName", System.Data.SqlDbType.NVarChar, 50)
            commpm.Parameters("@LogInName").Value = LoginName
            commpm.Parameters.Add("@LogOutName", System.Data.SqlDbType.NVarChar, 50)
            commpm.Parameters("@LogOutName").Value = LogOffName
            commpm.Parameters.Add("Comment", System.Data.SqlDbType.NVarChar, 250)
            commpm.Parameters("Comment").Value = comment
            commpm.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
            commpm.Parameters("@linac").Value = LinacName
            commpm.Parameters.Add("@LogInStatusID", System.Data.SqlDbType.NVarChar, 10)
            commpm.Parameters("@LogInStatusID").Value = LoginStatusID
            commpm.Parameters.Add("@LogOutStatusID", System.Data.SqlDbType.NVarChar, 10)
            commpm.Parameters("@LogOutStatusID").Value = LogOutStatusID

            conn.Open()
            commpm.ExecuteNonQuery()

            conn.Close()

            If Not lock Then
                DavesCode.Reuse.UpdateActivityTable(LinacName, LogOutStatusID, connectionString)
            End If
            'myscope.Complete()
            'End Using
            'Catch ex As Exception
            'DavesCode.NewFaultHandling.LogError(ex)
            ''This is for case where logoutstatusid is required
            'LogOutStatusID = "Unsuccessful"
            'End Try
            Return LogOutStatusID
        End Function

    End Class
End Namespace

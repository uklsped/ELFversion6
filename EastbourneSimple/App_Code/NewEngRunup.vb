Imports System.Data.SqlClient
Imports System.Transactions
Namespace DavesCode
    Public Class NewEngRunup
        Public Shared Function CommitRunup(ByVal GridviewE As GridView, ByVal grdviewI As GridView, ByVal LinacName As String, ByVal tabby As String, ByVal LogOffName As String, ByVal TextBoxc As String, ByVal Valid As Boolean, ByVal Fault As Boolean, ByVal lock As Boolean) As Boolean
            Dim usergroupselected As Integer = 2
            Dim Reason As Integer = 2
            Dim iView As Boolean = False
            Dim XVI As Boolean = False
            Dim Successful As Boolean = False
            Try
                Dim connectionString As String = ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString
                Using myscope As TransactionScope = New TransactionScope()
                    If Valid Then

                        If LinacName Like "LA?" Then
                            CommitRunupNew(GridviewE, LinacName, tabby, LogOffName, TextBoxc, Valid, False, False, connectionString)
                        Else
                            If LinacName Like "T1" Then
                                CommitRunupNew(GridviewE, LinacName, tabby, LogOffName, TextBoxc, Valid, False, False, connectionString)
                                Reuse.MachineStateNew(LogOffName, usergroupselected, LinacName, Reason, False, connectionString)
                            Else
                                CommitRunupNew(GridviewE, LinacName, tabby, LogOffName, TextBoxc, Valid, False, False, connectionString)
                                Reuse.MachineStateNew(LogOffName, usergroupselected, LinacName, Reason, False, connectionString)
                                Reuse.ReturnImaging(iView, XVI, grdviewI, LinacName)
                            End If

                            NewPreClinRunup.CommitPreClinNew(LinacName, LogOffName, "", iView, XVI, Valid, False, connectionString)

                        End If
                    Else
                        CommitRunupNew(GridviewE, LinacName, tabby, LogOffName, TextBoxc, Valid, False, False, connectionString)
                    End If
                    myscope.Complete()
                    Successful = True

                End Using
            Catch ex As Exception
                DavesCode.ReusePC.LogError(ex)
            End Try
            Return Successful
        End Function

        Public Shared Function CommitRunupNew(ByVal GridviewE As GridView, ByVal LinacName As String, ByVal tabby As String, ByVal LogOffName As String, ByVal TextBoxc As String, ByVal Valid As Boolean, ByVal Fault As Boolean, ByVal lock As Boolean, ByVal ConnectionString As String) As String
            Dim time As DateTime
            time = Now()
            Dim commitusername As String = LogOffName
            Dim LogInName As String = ""
            Dim cb As CheckBox
            Dim conn As SqlConnection
            Dim comm As SqlCommand
            Dim TextBox As String = TextBoxc
            Dim GridView1 As GridView = GridviewE
            Dim machinename As String = LinacName
            Dim tablabel As Integer = tabby 'this is inconsistent
            Dim reader As SqlDataReader
            Dim StartTime As DateTime
            Dim LogOffStateID As String
            Dim LogOnStateID As String = ""
            Dim Approved As Boolean = Valid
            Dim breakdown As Boolean = Fault
            Dim contime As SqlCommand
            Dim State As String = "Linac Unauthorised" 'default reason
            Dim UserReason As Integer = 7 'most common reason
            'Dim Activity As Integer = 2

            conn = New SqlConnection(ConnectionString)

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
                'LogOffStateID = DavesCode.Reuse.SetStatus(commitusername, "Fault", 5, 103, machinename, tablabel)
                State = "Fault"
                UserReason = 103
            Else
                If Approved Then
                    If tablabel = 1 Then
                        State = "Engineering Approved"
                    Else
                        State = "Radiographer Approved"
                    End If
                Else
                    'added 5/6/16 to set reason to end of day for midnight reset
                    If commitusername = "System" Then
                        UserReason = 102
                    End If

                End If

            End If
            LogOffStateID = Reuse.SetStatusNew(commitusername, State, 5, UserReason, machinename, 1, ConnectionString)
            If Not lock Then
                Reuse.UpdateActivityTable(machinename, LogOffStateID, ConnectionString)
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

                        Case "LA1"
                            cb = CType(GridView1.Rows(0).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MV6", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV6").Value = cb.Checked
                            comm.Parameters.Add("@MV6FFF", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV6FFF").Value = False
                            comm.Parameters.Add("@MV10", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV10").Value = False
                            comm.Parameters.Add("@MV10FFF", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV10FFF").Value = False
                            comm.Parameters.Add("@MeV4", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV4").Value = False
                            cb = CType(GridView1.Rows(1).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV6", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV6").Value = cb.Checked
                            cb = CType(GridView1.Rows(2).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV8", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV8").Value = cb.Checked
                            cb = CType(GridView1.Rows(3).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV10", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV10").Value = cb.Checked
                            cb = CType(GridView1.Rows(4).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV12", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV12").Value = cb.Checked
                            cb = CType(GridView1.Rows(5).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV15", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV15").Value = cb.Checked
                            cb = CType(GridView1.Rows(6).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV18", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV18").Value = cb.Checked
                            cb = CType(GridView1.Rows(7).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV20", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV20").Value = cb.Checked

                        Case "LA4"
                            cb = CType(GridView1.Rows(0).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MV6", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV6").Value = cb.Checked
                            comm.Parameters.Add("@MV6FFF", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV6FFF").Value = False
                            cb = CType(GridView1.Rows(1).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MV10", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV10").Value = cb.Checked
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

                        Case "LA2", "LA3"

                            cb = CType(GridView1.Rows(0).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MV6", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV6").Value = cb.Checked
                            comm.Parameters.Add("@MV6FFF", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV6FFF").Value = False
                            cb = CType(GridView1.Rows(1).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MV10", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV10").Value = cb.Checked
                            comm.Parameters.Add("@MV10FFF", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV10FFF").Value = False
                            comm.Parameters.Add("@MeV4", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV4").Value = False
                            cb = CType(GridView1.Rows(2).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV6", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV6").Value = cb.Checked
                            cb = CType(GridView1.Rows(3).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV8", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV8").Value = cb.Checked
                            cb = CType(GridView1.Rows(4).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV10", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV10").Value = cb.Checked
                            cb = CType(GridView1.Rows(5).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV12", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV12").Value = cb.Checked
                            cb = CType(GridView1.Rows(6).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV15", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV15").Value = cb.Checked
                            cb = CType(GridView1.Rows(7).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV18", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV18").Value = cb.Checked
                            cb = CType(GridView1.Rows(8).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV20", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV20").Value = cb.Checked
                        Case "E1", "E2", "B1"
                            cb = CType(GridView1.Rows(0).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MV6", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV6").Value = cb.Checked
                            cb = CType(GridView1.Rows(1).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MV6FFF", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV6FFF").Value = cb.Checked
                            cb = CType(GridView1.Rows(2).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MV10", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV10").Value = cb.Checked
                            cb = CType(GridView1.Rows(3).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MV10FFF", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV10FFF").Value = cb.Checked
                            cb = CType(GridView1.Rows(4).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV4", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV4").Value = cb.Checked
                            cb = CType(GridView1.Rows(5).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV6", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV6").Value = cb.Checked
                            cb = CType(GridView1.Rows(6).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV8", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV8").Value = cb.Checked
                            cb = CType(GridView1.Rows(7).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV10", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV10").Value = cb.Checked
                            cb = CType(GridView1.Rows(8).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV12", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV12").Value = cb.Checked
                            cb = CType(GridView1.Rows(9).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MeV15", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV15").Value = cb.Checked
                            comm.Parameters.Add("@MeV18", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV18").Value = False
                            comm.Parameters.Add("@MeV20", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MeV20").Value = False

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
                        Case "LA1"
                            cb = CType(GridView1.Rows(0).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MV6", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV6").Value = cb.Checked
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

                        Case "LA2", "LA3", "LA4"
                            cb = CType(GridView1.Rows(0).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MV6", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV6").Value = cb.Checked
                            comm.Parameters.Add("@MV6FFF", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV6FFF").Value = False
                            cb = CType(GridView1.Rows(1).FindControl("RowLevelCheckBox"), CheckBox)
                            comm.Parameters.Add("@MV10", System.Data.SqlDbType.Bit)
                            comm.Parameters("@MV10").Value = cb.Checked
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
            comm.Parameters.Add("LogInName", Data.SqlDbType.NVarChar, 50)
            comm.Parameters("LogInName").Value = LogInName
            'Try
            conn.Open()
            comm.ExecuteNonQuery()

            conn.Close()

            Return LogOffStateID


        End Function
    End Class
End Namespace

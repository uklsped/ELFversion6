Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.UI.Page
Imports AjaxControlToolkit
Imports System.Text
Imports System.IO



Namespace DavesCode

    Public Class Reuse

        Public Shared Function GetIPAddress() As String
            Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
            Dim sIPAddress As String = context.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
            If String.IsNullOrEmpty(sIPAddress) Then
                Return context.Request.ServerVariables("REMOTE_ADDR")
            Else
                Dim ipArray As String() = sIPAddress.Split(New [Char]() {","c})
                Return ipArray(0)
            End If
        End Function

        Public Shared Function ReturnActivity(ByVal activity As String) As String
            Dim Activitydictionary As New Dictionary(Of Integer, String)
            Activitydictionary.Add(1, "Engineering Run up")
            Activitydictionary.Add(2, "Pre-Clinical Run up")
            Activitydictionary.Add(3, "Clinical")
            Activitydictionary.Add(4, "Planned Maintenance")
            Activitydictionary.Add(5, "Repair")
            Activitydictionary.Add(6, "Physics QA")
            Activitydictionary.Add(7, "Logged Off")
            Activitydictionary.Add(8, "Development/Training")
            Activitydictionary.Add(9, "Emergency Run Up")
            Activitydictionary.Add(100, "Administration")
            Activitydictionary.Add(101, "Lock Elf")
            Activitydictionary.Add(102, "End of Day")
            Activitydictionary.Add(103, "Report Fault")


            Return Activitydictionary.Item(activity)

        End Function



        Public Shared Sub writeLogFile(ByVal useage As Integer, ByVal user As String, ByVal onoff As Boolean)
            'Instrumentation to write to log file

            Dim builder As New StringBuilder
            Dim Reason As Integer = useage
            Dim loginUsername As String = user
            Dim polarity As Boolean = onoff
            Dim statement As String
            If polarity Then
                statement = "linac logged on for "
            Else
                statement = "linac logged off for "
            End If
            ' Append the time to the stringBuilder
            builder.Append(statement)
            builder.Append(Reason)
            ' Append the comment to the StringBuilder
            builder.Append(" by ")
            ' Append a line break
            builder.Append(loginUsername)
            builder.Append(" at ")
            builder.Append(DateTime.Now.ToString("h:mm:ss"))
            'builder.Append(DateTime.Now.ToString("h:mm tt"))
            'see date patterns at http://www.geekzilla.co.uk/View00FF7904-B510-468C-A2C8-F859AA20581F.htm

            'Using streamWriter As StreamWriter = File.AppendText("C:\ELF\TestRun1.txt")
            '    streamWriter.WriteLine(builder)
            'End Using


            'this is the end of the file try


        End Sub


        Public Shared Function StringBuilder(ByVal faulttype As String, ByVal BeginDate As String, StopDate As String) As String
            Dim builder As New StringBuilder
            Dim statement As String
            Dim StartDate As String = BeginDate
            Dim EndDate As String = StopDate
            Dim ending As String
            Dim TypeofFault As String = ""
            Dim Dates As String = StartDate & " And " & EndDate

            ending = "</td></tr></table>"
            statement = "<table border='1' width='100%' height='40px' cellpadding='0' cellspacing='0' bgcolor='#66d9ff'><tr><td>"
            Select Case faulttype
                Case 1
                    TypeofFault = "All Faults"
                Case 2
                    TypeofFault = "All Closed Faults"
                Case 3
                    TypeofFault = "All Concessions"
                Case 4
                    TypeofFault = "All Open Concessions"
                Case 5
                    TypeofFault = "All Faults Opened Between: "
                Case 6
                    TypeofFault = "All Faults Closed Between: "
                Case 7
                    TypeofFault = "All User Closed Faults Between: "
            End Select

            builder.Append(statement)
            builder.Append(TypeofFault)

            Select Case faulttype
                Case 5, 6, 7
                    builder.Append(Dates)
            End Select

            builder.Append(ending)
            Return builder.ToString

        End Function

        Public Shared Function SetPageLoad(ByVal linac As String) As String
            'This is here to check day run up was done but will only work if page is reloaded
            Dim machine As String = linac
            Dim status As String
            Dim NowTime As DateTime
            NowTime = DateTime.Now
            Dim RunTime As DateTime
            Dim conn1 As SqlConnection
            Dim comm2 As SqlCommand
            Dim reader1 As SqlDataReader
            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString
            status = "Linac Unauthorised"

            conn1 = New SqlConnection(connectionString)
            'from http://www.sqlservercentral.com/Forums/Topic1416029-1292-1.aspx
            comm2 = New SqlCommand("SELECT state, RunTime FROM (select max(datetime) as RunTime from LinacStatus as a where state = 'Engineering Approved' and linac=@linac) t1, (select state as state from linacstatus as b where stateid = (select max(stateid) from linacstatus where linac=@linac)) t2", conn1)
            comm2.Parameters.AddWithValue("@linac", machine)
            'comm2 = New SqlCommand("Select state from linacstatus where stateid = (select max(stateid) from linacstatus where linac='LA1')", conn1)
            conn1.Open()
            reader1 = comm2.ExecuteReader()
            If reader1.HasRows() Then
                If reader1.Read() Then
                    status = reader1.Item("state")
                    If Not IsDBNull(reader1.Item("RunTime")) Then
                        RunTime = reader1.Item("RunTime")
                    End If

                End If
            Else
                'DavesCode.Reuse.SetStatus("No User", "Linac Unauthorised", 5, 7, machine, 1)
                MachineState("No User", 5, machine, 7, False)
                status = "Linac Unauthorised"
            End If
            reader1.Close()
            conn1.Close()
            'Select Case status
            '    Case "Fault", "Repair", "Linac Unauthorised"
            '        'do nothing
            '    Case Else
            '        If DateTime.Compare(NowTime.Date, RunTime.Date) > 0 Then
            '            DavesCode.Reuse.SetStatus("No User", "Linac Unauthorised", 5, 7, "LA1", 1)
            '            status = "Linac Unauthorised"
            '        End If
            'End Select

            Return status

        End Function

        Public Shared Function BrowserLoaded(ByVal linac As String) As Boolean
            Dim machine As String = linac
            Dim status As Boolean
            Dim conn1 As SqlConnection
            Dim comm2 As SqlCommand
            Dim reader1 As SqlDataReader
            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString
            status = False

            conn1 = New SqlConnection(connectionString)

            comm2 = New SqlCommand("SELECT browserOpen FROM BrowserState where id = (Select max(id) from BrowserState where linac=@linac)", conn1)
            comm2.Parameters.AddWithValue("@linac", machine)
            conn1.Open()
            reader1 = comm2.ExecuteReader()
            If reader1.HasRows() Then
                If reader1.Read() Then
                    status = reader1.Item("browserOpen")
                End If
            Else
                status = False
            End If
            reader1.Close()
            conn1.Close()
            Return status
        End Function

        Public Shared Sub ToggleBrowser(ByVal linac As String, ByVal toggle As Boolean)
            Dim conn1 As SqlConnection
            Dim comm2 As SqlCommand
            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString

            conn1 = New SqlConnection(connectionString)

            comm2 = New SqlCommand("Insert into BrowserState (linac, browserOpen) VALUES (@linac, @browserOpen)", conn1)
            comm2.Parameters.AddWithValue("@linac", linac)
            comm2.Parameters.AddWithValue("@browserOpen", toggle)
            conn1.Open()


            comm2.ExecuteNonQuery()

            conn1.Close()

        End Sub

        Public Shared Function RandomNumber(ByVal MaxNumber As Integer,
    Optional ByVal MinNumber As Integer = 0) As Integer

            'initialize random number generator
            Dim r As New Random(System.DateTime.Now.Millisecond)

            'if passed incorrect arguments, swap them
            'can also throw exception or return 0

            If MinNumber > MaxNumber Then
                Dim t As Integer = MinNumber
                MinNumber = MaxNumber
                MaxNumber = t
            End If

            Return r.Next(MinNumber, MaxNumber)

        End Function

        'Used
        Public Shared Function SuccessfulLogin(ByVal username As String, ByVal userpassword As String, ByVal LinacName As String, ByVal Need As Integer, ByVal Texbox As TextBox, ByVal pasword As TextBox, ByVal logerror As Label, ByVal modalp As ModalPopupExtender) As Integer
            'We need to determine if the user is authenticated and set e.Authenticated accordingly
            'Get the values entered by the user
            Dim loginUsername As String = username
            Dim loginPassword As String = userpassword
            Dim MachineName As String = LinacName
            Dim Reason As Integer = Need
            Dim textboxUser As TextBox = Texbox
            Dim textboxPass As TextBox = pasword
            Dim LoginErrorDetail As Label = logerror
            Dim modalpopupident As ModalPopupExtender = modalp
            'First check if user name and password are correct
            If Membership.ValidateUser(loginUsername, loginPassword) Then
                'Find out which user group user is in
                Dim usergroupselected As Integer = GetRole(loginUsername)
                If LogOn(usergroupselected, MachineName, Reason) Then
                    Return usergroupselected
                Else
                    'Tell them they don't have permission

                    If (Not textboxUser Is Nothing) Then
                        textboxUser.Text = String.Empty
                        LoginErrorDetail.Text = "You don't have permission to perform that action."
                        'modalpopupident.Show()

                    End If

                End If

            Else
                'See if this user exists in the database
                Dim userInfo As MembershipUser = Nothing
                Try
                    userInfo = Membership.GetUser(loginUsername)
                Catch e As ArgumentException
                    LoginErrorDetail.Text = "Your username is invalid."
                    'Return Nothing
                End Try
                If userInfo Is Nothing Then
                    'The user entered an invalid username...

                    If (Not textboxUser Is Nothing) Then
                        If textboxUser.Text = String.Empty Then
                            LoginErrorDetail.Text = "Please Enter Your User Name."
                        Else
                            textboxUser.Text = String.Empty
                            LoginErrorDetail.Text = "Your username is invalid."
                        End If

                    End If
                    'Return Nothing
                Else
                    'The password was incorrect (don't show anything, the Login control already describes the problem)

                    If (Not textboxPass Is Nothing) Then
                        If textboxPass.Text = String.Empty Then
                            LoginErrorDetail.Text = "Please Enter Your Password."
                        Else
                            textboxPass.Text = String.Empty
                            LoginErrorDetail.Text = "Your Password is invalid."
                            modalpopupident.Show()

                        End If

                    End If
                    'Return Nothing
                End If
            End If
        End Function
        'Used Finds out which user group user is in
        Public Shared Function GetRole(ByVal user As String) As Integer
            Dim loginUsername As String = user
            'This won't work if new users are added.
            Dim Roledictionary As New Dictionary(Of String, Integer)
            Roledictionary.Add("Administrator", 1)
            Roledictionary.Add("Engineer", 2)
            Roledictionary.Add("Radiographer", 3)
            Roledictionary.Add("Physicist", 4)
            Roledictionary.Add("None", 5)
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("userstring").ConnectionString
            Dim con As New SqlConnection(connectionString)
            con.Open()
            Dim cmd As New SqlCommand("select r.rolename from aspnet_Roles r " &
            "left outer join aspnet_UsersInRoles ur on ur.RoleId = r.RoleId " &
            "left outer join aspnet_Users u on u.UserId=ur.UserId " &
            "where u.UserName = @UserName", con)
            cmd.Parameters.AddWithValue("@UserName", loginUsername)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            Dim userrolename As String
            da.Fill(dt)
            If dt.Rows.Count > 0 Then

                For Each dataRow As DataRow In dt.Rows

                    userrolename = dataRow("RoleName")
                Next
            End If
            con.Close()
            Return Roledictionary.Item(userrolename)
        End Function
        'Used
        Public Shared Function LogOn(ByVal usergroup As Integer, ByVal linac As String, ByVal userreason As Integer) As Boolean
            Dim usergroupselected As Integer = usergroup
            Dim reason As Integer = userreason
            Dim linacName As String = linac
            Select Case reason
                Case 1, 4, 5, 101

                    If usergroupselected = 2 Then
                        'MachineState(usergroupselected, linacName, reason)
                        Return True
                    ElseIf usergroupselected = 4 Then
                        'MachineState(usergroupselected, linacName, reason)
                        Return True
                    Else
                        Return False

                    End If


                Case 2, 3, 9

                    If usergroupselected = 3 Then
                        'MachineState(usergroupselected, linacName, reason)
                        Return True
                    Else
                        Return False
                    End If

                Case 6
                    If usergroupselected = 4 Or usergroupselected = 2 Then
                        'MachineState(usergroupselected, linacName, reason)
                        Return True
                    Else
                        Return False
                    End If
                    '104 is recovery and at the moment let anyone do that
                    'change 11 to only allow rad to select RAD RESET in defect uc
                    'Added 12 to allow for tomo unrecoverable fault
                Case 8, 10, 12, 102, 103, 104
                    Return True

                Case 11
                    If usergroupselected = 3 Then
                        Return True
                    Else
                        Return False
                    End If

                Case 100
                    If usergroupselected = 1 Then
                        Return True
                    Else
                        Return False
                    End If

                Case Else
                    Return False
            End Select

        End Function

        'Used This sub never changes the state so line 433 to 457 can go - except for Clinical but need to check as I have done
        Public Shared Sub MachineState(ByVal loginuser As String, ByVal usergroup As Integer, ByVal linac As String, ByVal possessreason As Integer, ByVal unlock As Boolean)
            'Need Some Error Handling in this function
            Dim loginName As String = loginuser
            Dim time As DateTime
            time = Now()

            Dim LinacStatusID As Integer
            Dim reader As SqlDataReader
            Dim nowstatus As String
            Dim linacName As String = linac
            Dim conn As SqlConnection

            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString
            Dim Machinestatus As SqlCommand
            Dim StatusNow As SqlCommand
            conn = New SqlConnection(connectionString)
            'Added because state wasn't being changed from Suspended to Clinical 31 March 2016 Bug 7
            If possessreason = 3 Then
                nowstatus = "Clinical"
            Else
                StatusNow = New SqlCommand("SELECT state FROM [LinacStatus] where stateID = (Select max(stateID) as lastrecord from [LinacStatus] where linac=@linac)", conn)
                StatusNow.Parameters.AddWithValue("@linac", linacName)
                conn.Open()
                reader = StatusNow.ExecuteReader()

                If reader.Read() Then
                    nowstatus = reader.Item("state").ToString()
                Else
                    'this caters for the case where this is the first record for the linac
                    nowstatus = "Linac Unauthorised"
                End If
                reader.Close()
                conn.Close()
            End If
            'commented out code removed 15 April 2016

            Machinestatus = New SqlCommand("INSERT INTO LinacStatus (state, DateTime, usergroup,userreason,linac, UserName ) " &
                                        "VALUES (@state, @Datetime, @usergroup, @userreason, @linac, @UserName) SELECT SCOPE_IDENTITY()", conn)
            Machinestatus.Parameters.AddWithValue("@state", System.Data.SqlDbType.NVarChar)
            Machinestatus.Parameters("@state").Value = nowstatus
            Machinestatus.Parameters.Add("@DateTime", System.Data.SqlDbType.DateTime)
            Machinestatus.Parameters("@DateTime").Value = time
            Machinestatus.Parameters.Add("@usergroup", System.Data.SqlDbType.Int)
            Machinestatus.Parameters("@usergroup").Value = usergroup
            Machinestatus.Parameters.Add("@userreason", System.Data.SqlDbType.Int)
            Machinestatus.Parameters("@userreason").Value = possessreason
            Machinestatus.Parameters.AddWithValue("@linac", System.Data.SqlDbType.NVarChar)
            Machinestatus.Parameters("@linac").Value = linacName
            Machinestatus.Parameters.AddWithValue("@UserName", System.Data.SqlDbType.NVarChar)
            Machinestatus.Parameters("@UserName").Value = loginName


            Try
                'To get the identity of the record just inserted from
                'http://www.aspsnippets.com/Articles/Return-Identity-Auto-Increment-Column-value-after-record-insert-in-SQL-Server-Database-using-ADONet-with-C-and-VBNet.aspx
                conn.Open()
                'commstatus.ExecuteNonQuery()

                Dim obj As Object = Machinestatus.ExecuteScalar()
                'Dim LinacStatusIDs As String = obj.ToString()
                LinacStatusID = CInt(obj)
                conn.Close()
                'This creates in the Activity table the entry for the start of an activity so long as it is not as a result of switching the user.
                If Not unlock Then
                    WriteActivityTable(LinacStatusID, time, possessreason, linacName)
                End If

            Finally
                conn.Close()
            End Try


        End Sub
        Public Shared Sub WriteActivityTable(ByVal StateID As Integer, ByVal timestamp As DateTime, ByVal possessreason As Integer, ByVal linacname As String)
            Dim laststateid As Integer = 0
            Dim lastState As String = ""
            Dim Activestatus As SqlCommand
            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString

            laststateid = GetLastTech(linacname, 1, lastState, lastusername:="", lastusergroup:=0)


            conn = New SqlConnection(connectionString)

            Activestatus = New SqlCommand("INSERT INTO ActiveTime (userreason, StartID, StartTime, Linac, PreviousStateID ) " &
                                        "VALUES (@userreason,@StartID, @StartTime, @Linac, @PreviousStateID)", conn)
            Activestatus.Parameters.Add("@userreason", System.Data.SqlDbType.Int)
            Activestatus.Parameters("@userreason").Value = possessreason
            Activestatus.Parameters.AddWithValue("@StartID", System.Data.SqlDbType.Int)
            Activestatus.Parameters("@StartID").Value = StateID
            Activestatus.Parameters.Add("@StartTime", System.Data.SqlDbType.DateTime)
            Activestatus.Parameters("@StartTime").Value = timestamp
            Activestatus.Parameters.AddWithValue("@linac", System.Data.SqlDbType.NVarChar)
            Activestatus.Parameters("@linac").Value = linacname
            Activestatus.Parameters.AddWithValue("@PreviousStateID", System.Data.SqlDbType.Int)
            Activestatus.Parameters("@PreviousStateID").Value = laststateid

            Try
                conn.Open()

                Activestatus.ExecuteNonQuery()

            Finally
                conn.Close()
            End Try

        End Sub

        Public Shared Sub UpdateActivityTable(ByVal linac As String, ByVal StopID As String)

            Dim reader As SqlDataReader
            Dim ActID As String
            Dim linacName As String = linac
            Dim conn As SqlConnection

            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString

            Dim StatusNow As SqlCommand
            Dim UpdateNow As SqlCommand
            conn = New SqlConnection(connectionString)
            'Added because state wasn't being changed from Suspended to Clinical 31 March 2016 Bug 7

            StatusNow = New SqlCommand("SELECT ActID FROM [ActiveTime] where ActID = (Select max(ActID) as lastrecord from [ActiveTime] where linac=@linac)", conn)
            StatusNow.Parameters.AddWithValue("@linac", linacName)
            conn.Open()
            reader = StatusNow.ExecuteReader()

            If reader.Read() Then
                ActID = reader.Item("ActID").ToString
            End If
            reader.Close()
            conn.Close()

            UpdateNow = New SqlCommand("Update ActiveTime SET StopID=@StopID,StopTime=@StopTime WHERE ActID=@ActID", conn)
            UpdateNow.Parameters.Add("@StopID", System.Data.SqlDbType.Int)
            UpdateNow.Parameters("@StopID").Value = StopID
            UpdateNow.Parameters.Add("@StopTime", System.Data.SqlDbType.DateTime)
            UpdateNow.Parameters("@StopTime").Value = Now()
            UpdateNow.Parameters.Add("@ActID", System.Data.SqlDbType.Int)
            UpdateNow.Parameters("@ActID").Value = ActID


            conn.Open()
            UpdateNow.ExecuteNonQuery()
            conn.Close()
        End Sub


        'Used
        Public Shared Function CommitRunup(ByVal GridviewE As GridView, ByVal LinacName As String, ByVal tabby As String, ByVal LogOffName As String, ByVal TextBoxc As String, ByVal Valid As Boolean, ByVal Fault As Boolean, ByVal lock As Boolean) As String
            Dim time As DateTime
            time = Now()
            Dim commitusername As String = LogOffName
            Dim LogInName As String = ""
            Dim cb As CheckBox
            Dim conn As SqlConnection
            Dim comm As SqlCommand
            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString
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
                LogOffStateID = DavesCode.Reuse.SetStatus(commitusername, "Fault", 5, 103, machinename, tablabel)
            Else
                If Approved Then
                    If tablabel = 1 Then
                        LogOffStateID = DavesCode.Reuse.SetStatus(commitusername, "Engineering Approved", 5, 7, machinename, 1)
                    Else
                        LogOffStateID = DavesCode.Reuse.SetStatus(commitusername, "Radiographer Approved", 5, 7, machinename, 1)
                    End If
                Else
                    'added 5/6/16 to set reason to end of day for midnight reset
                    If commitusername = "System" Then
                        LogOffStateID = DavesCode.Reuse.SetStatus(commitusername, "Linac Unauthorised", 5, 102, machinename, 1)
                    Else
                        LogOffStateID = DavesCode.Reuse.SetStatus(commitusername, "Linac Unauthorised", 5, 7, machinename, 1)
                    End If

                End If

            End If
            If Not lock Then
                UpdateActivityTable(machinename, LogOffStateID)
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

        Public Shared Function WriteAuxTables(ByVal LinacID As String, ByVal username As String, ByVal comment As String, ByVal Radio As Integer, ByVal Tab As Integer, ByVal Fault As Boolean, ByVal suspendvalue As String, ByVal repairvalue As String, ByVal lock As Boolean) As String
            'writes the aux tables depending on the options picked. And writes the linac status table first.
            'When tidying this up look at whether radio is the same as tab used - no it's not

            Dim Breakdown As Boolean = Fault
            Dim time As DateTime
            time = Now()
            Dim Tabused As Integer = Tab
            Dim LoginStatusID As String = ""
            Dim conn As SqlConnection
            Dim localcomment As String = comment
            Dim MachineName As String = LinacID
            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString
            Dim commpm As SqlCommand
            Dim StartTime As DateTime
            Dim LoginName As String = ""
            Dim logOutName As String = username
            Dim LogOutStatusID As String = ""
            Dim contime As SqlCommand
            Dim reader As SqlDataReader
            Dim Radioselect As Integer = Radio
            Dim builder As New StringBuilder()
            Dim TableName As String = "AuxTable"
            Dim Activity As String = ""
            Dim InsertData As String = "(Tab,LogInDate, LogOutDate, LogInName, LogOutName, Comment,linac, LogInStatusID, LogOutStatusID ) " &
                                       "VALUES (@Tab, @LogInDate, @LogOutDate, @LogInName, @LogOutName, @Comment,@linac, @LogInStatusID, @LogOutStatusID)"
            Dim suspstate As String = suspendvalue
            Dim repstate As String = repairvalue
            Dim userreason As Integer

            conn = New SqlConnection(connectionString)

            contime = New SqlCommand("SELECT stateID,DateTime, UserName FROM [LinacStatus] where stateID = (Select max(stateID) as lastrecord from [LinacStatus] where linac=@linac)", conn)
            contime.Parameters.AddWithValue("@linac", MachineName)
            conn.Open()
            reader = contime.ExecuteReader()

            If reader.Read() Then
                StartTime = reader.Item("DateTime")
                LoginName = reader.Item("UserName")
                LoginStatusID = reader.Item("stateID")
            End If
            reader.Close()
            conn.Close()

            Select Case Tabused
                'created a new table AuxTable for all of the aux values so table name is redundant - don't know what activity is used for here
                Case 4
                    'TableName = "PMTable"
                    Activity = "Planned Maintenance"
                Case 5
                    'TableName = "RepairTable"
                    Activity = "Repair"
                Case 6
                    'TableName = "PQATable"
                    Activity = "Physics QA"
                Case 8
                    'TableName = "TrainingTable"
                    Activity = "Training"
                Case Else

            End Select

            builder.Append("INSERT INTO ")
            builder.Append(TableName)
            builder.Append(" ")
            builder.Append(InsertData)
            Dim output As String = builder.ToString

            Select Case Radioselect
                Case 101, 102, 103
                    userreason = Radioselect
                Case Else
                    userreason = 7
            End Select

            If Breakdown Then
                LogOutStatusID = DavesCode.Reuse.SetStatus(username, "Fault", 5, userreason, MachineName, Tabused)
            Else
                'Radioselect determines how to SetStatus as a result of which radiobutton selected
                Select Case Radioselect
                    Case 1
                        LogOutStatusID = DavesCode.Reuse.SetStatus(username, "Linac Unauthorised", 5, userreason, MachineName, Tabused)
                    Case 4, 5, 6, 8, 101
                        If suspstate = 1 Then
                            LogOutStatusID = DavesCode.Reuse.SetStatus(username, "Suspended", 5, userreason, MachineName, Tabused)
                        ElseIf repstate = 1 Then
                            LogOutStatusID = DavesCode.Reuse.SetStatus(username, "Engineering Approved", 5, userreason, MachineName, Tabused)
                        Else
                            LogOutStatusID = DavesCode.Reuse.SetStatus(username, "Linac Unauthorised", 5, userreason, MachineName, Tabused)
                        End If

                    Case 2
                        LogOutStatusID = DavesCode.Reuse.SetStatus(username, "Engineering Approved", 5, userreason, MachineName, Tabused)
                    Case 3
                        LogOutStatusID = DavesCode.Reuse.SetStatus(username, "Suspended", 5, userreason, MachineName, Tabused)
                        'Case 3
                        '    LogOutStatusID = DavesCode.Reuse.SetStatus(username, Activity, 5, 7, MachineName, Tabused)
                        'Case 101
                        '    LogOutStatusID = DavesCode.Reuse.SetStatus(username, "Fault", 5, userreason, MachineName, Tabused)
                    Case 102
                        LogOutStatusID = DavesCode.Reuse.SetStatus(username, "Linac Unauthorised", 5, userreason, MachineName, Tabused)
                        'Case 5
                        '    LogOutStatusID = DavesCode.Reuse.SetStatus(username, "Linac Unauthorised", 5, 7, MachineName, Tabused)
                End Select
            End If
            'commpm = New SqlCommand("INSERT INTO PQATable (LogInDate, LogOutDate, LogInName, LogOutName, pqaComment,linac, LogInStatusID, LogOutStatusID ) " & _
            '                           "VALUES ( @LogInDate, @LogOutDate, @LogInName, @LogOutName, @pqaComment,@linac, @LogInStatusID, @LogOutStatusID)", conn)

            commpm = New SqlCommand(builder.ToString, conn)
            commpm.Parameters.Add("@Tab", System.Data.SqlDbType.Int)
            commpm.Parameters("@Tab").Value = Tabused
            commpm.Parameters.Add("@LogInDate", System.Data.SqlDbType.DateTime)
            commpm.Parameters("@LogInDate").Value = StartTime
            commpm.Parameters.Add("@LogOutDate", System.Data.SqlDbType.DateTime)
            commpm.Parameters("@LogOutDate").Value = time
            commpm.Parameters.Add("@LogInName", System.Data.SqlDbType.NVarChar, 50)
            commpm.Parameters("@LogInName").Value = LoginName
            commpm.Parameters.Add("@LogOutName", System.Data.SqlDbType.NVarChar, 50)
            commpm.Parameters("@LogOutName").Value = logOutName
            commpm.Parameters.Add("Comment", System.Data.SqlDbType.NVarChar, 250)
            commpm.Parameters("Comment").Value = localcomment
            commpm.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
            commpm.Parameters("@linac").Value = MachineName
            commpm.Parameters.Add("@LogInStatusID", System.Data.SqlDbType.NVarChar, 10)
            commpm.Parameters("@LogInStatusID").Value = LoginStatusID
            commpm.Parameters.Add("@LogOutStatusID", System.Data.SqlDbType.NVarChar, 10)
            commpm.Parameters("@LogOutStatusID").Value = LogOutStatusID
            Try
                conn.Open()
                commpm.ExecuteNonQuery()
            Finally
                conn.Close()

            End Try
            If Not lock Then
                UpdateActivityTable(MachineName, LogOutStatusID)
            End If
            Return LogOutStatusID
        End Function
        'Used
        Public Shared Function CommitPreClin(ByVal LinacN As String, ByVal username As String, ByVal TextBoxp As String, ByVal Imgchk1 As Boolean, ByVal Imgchk2 As Boolean, ByVal Valid As Boolean, ByVal Fault As Boolean) As String
            'Public Shared Function CommitPreClin(ByVal LinacN As String, ByVal username As String, ByVal TextBoxp As String, ByVal GridViewI As GridView, ByVal Valid As Boolean, ByVal Fault As Boolean) As String
            Dim LogOutStatusID As String
            Dim LogInStatusID As String
            Dim time As DateTime
            Dim LinacName As String = LinacN
            Dim logOutName As String = username
            Dim logInName As String = ""
            time = Now()
            Dim StartTime As DateTime
            Dim reader As SqlDataReader
            Dim EHID As Integer
            Dim Approved As Boolean = Valid
            Dim breakdown As Boolean = Fault
            Dim iView As Boolean = Imgchk1
            Dim XVI As Boolean = Imgchk2
            Dim textbox As String = TextBoxp
            Dim conn As SqlConnection
            Dim SqlDataSource1 As New SqlDataSource()
            Dim cb As CheckBox
            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString
            Dim commHAuthID As SqlCommand
            Dim contime As SqlCommand

            conn = New SqlConnection(connectionString)
            'This will get the time the linac was accepted for the pre-clinical
            contime = New SqlCommand("SELECT DateTime, username, stateID FROM [LinacStatus] where stateID = (Select max(stateID) as lastrecord from [LinacStatus] where linac=@linac)", conn)

            contime.Parameters.AddWithValue("@linac", LinacName)
            conn.Open()
            reader = contime.ExecuteReader()

            If reader.Read() Then
                StartTime = reader.Item("DateTime")
                logInName = reader.Item("username")
                LogInStatusID = reader.Item("stateID")
            End If
            reader.Close()
            conn.Close()
            'This calculates the time between logging on and now - but why here?
            Dim minutesDuration As Decimal
            Dim duration As TimeSpan = time - StartTime

            minutesDuration = Decimal.Round(duration.TotalMinutes, 2, MidpointRounding.ToEven)


            'this gets the ID of the associated engineering handover
            commHAuthID = New SqlCommand("Select HandoverID from HandoverEnergies where HandoverID  = (Select max(HandoverID) as lastrecord from HandoverEnergies where linac=@linac)", conn)
            commHAuthID.Parameters.Add("@linac", SqlDbType.NVarChar, 10)
            commHAuthID.Parameters("@linac").Value = LinacName

            conn.Open()
            reader = commHAuthID.ExecuteReader()
            If reader.Read() Then
                EHID = reader.Item("HandoverId")
                reader.Close()
                conn.Close()
            End If
            'This is here because this sub is also called from the fault page in order to write the linacstatus and to write clinicalhandover table
            If breakdown Then
                'changed to user reason 103
                LogOutStatusID = DavesCode.Reuse.SetStatus(logOutName, "Fault", 5, 103, LinacName, -1)
                iView = False
                XVI = False
            Else 'not a breakdown so if approved set clinical - not treating or back to engineering approved
                If Approved Then
                    'LinacStatusID = DavesCode.Reuse.SetStatus(loginName, "Clinical - Not Treating", 5, 7, LinacName, 2)
                    'October change
                    'changed august 21 to allow going to other states
                    'changed clinical to suspended to allow for cancel on clinical.  31 March 2016
                    LogOutStatusID = DavesCode.Reuse.SetStatus(logOutName, "Suspended", 5, 7, LinacName, 2)
                Else
                    'added to make right for midnight check before it just left as engineering approved
                    If logOutName = "System" Then
                        LogOutStatusID = DavesCode.Reuse.SetStatus(logOutName, "Linac Unauthorised", 5, 102, LinacName, 2)
                    Else
                        'added for E1 and E2
                        If LinacName = "E1" Or LinacName = "E2" Or LinacName = "B1" Then
                            LogOutStatusID = DavesCode.Reuse.SetStatus(logOutName, "Linac Unauthorised", 5, 7, LinacName, 1)
                        Else
                            LogOutStatusID = DavesCode.Reuse.SetStatus(logOutName, "Engineering Approved", 5, 7, LinacName, 2)
                        End If

                    End If

                End If
            End If
            'http://www.mikesdotnetting.com/Article/53/Saving-a-user%27s-CheckBoxList-selection-and-re-populating-the-CheckBoxList-from-saved-data - used for imaging

            'This writes the clinicalhandover table - doesn't have the user in it

            Dim commaccept As SqlCommand
            commaccept = New SqlCommand("INSERT INTO ClinicalHandover ( CComment,Ehandid, LogOutDate, linac, LogInDate, Duration, iView, XVI, LogOutStatusID, Approved, LogInName, LogOutName, LogInStatusID) " &
                                        "VALUES (@CComment,@Ehandid, @LogOutDate, @linac, @LogInDate, @Duration, @iView, @XVI, @LogOutStatusID, @Approved, @LogInName, @LogOutName, @LogInStatusID)", conn)
            commaccept.Parameters.Add("@CComment", System.Data.SqlDbType.NVarChar, 250)
            commaccept.Parameters("@CComment").Value = textbox
            commaccept.Parameters.Add("@Ehandid", Data.SqlDbType.Int)
            commaccept.Parameters("@Ehandid").Value = EHID
            commaccept.Parameters.Add("@LogOutDate", System.Data.SqlDbType.DateTime)
            commaccept.Parameters("@LogOutDate").Value = time
            commaccept.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
            commaccept.Parameters("@linac").Value = LinacName
            commaccept.Parameters.Add("@LogInDate", System.Data.SqlDbType.DateTime)
            commaccept.Parameters("@LogInDate").Value = StartTime
            commaccept.Parameters.Add("@Duration", System.Data.SqlDbType.Decimal)
            commaccept.Parameters("@Duration").Value = minutesDuration
            commaccept.Parameters.Add("@iView", System.Data.SqlDbType.Bit)
            commaccept.Parameters("@iView").Value = iView
            commaccept.Parameters.Add("@XVI", System.Data.SqlDbType.Bit)
            commaccept.Parameters("@XVI").Value = XVI
            commaccept.Parameters.Add("@LogOutStatusID", Data.SqlDbType.Int)
            commaccept.Parameters("@LogOutStatusID").Value = LogOutStatusID
            commaccept.Parameters.Add("@Approved", Data.SqlDbType.Bit)
            commaccept.Parameters("@Approved").Value = Approved
            commaccept.Parameters.Add("@LogInName", System.Data.SqlDbType.NVarChar, 50)
            commaccept.Parameters("@LogInName").Value = logInName
            commaccept.Parameters.Add("@LogOutName", System.Data.SqlDbType.NVarChar, 50)
            commaccept.Parameters("@LogOutName").Value = logOutName
            commaccept.Parameters.Add("@LogInStatusID", Data.SqlDbType.Int)
            commaccept.Parameters("@LogInStatusID").Value = LogInStatusID


            Try
                conn.Open()
                'Altered 17 March
                'commaccept.ExecuteNonQuery()
                Dim CHANDID As Integer
                Dim obj As Object = commaccept.ExecuteScalar()
                'Dim LinacStatusIDs As String = obj.ToString()
                CHANDID = CInt(obj)

            Finally
                conn.Close()
                UpdateActivityTable(LinacName, LogOutStatusID)
            End Try
            Return LogOutStatusID

        End Function

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

            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString
            Dim commHAuthID As SqlCommand
            Dim contime As SqlCommand

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
                    SetTreatment("Not Treating", LinacName, logInStatusID)
                End If
                reader.Close()

            End If
            conn.Close()
            'This is here because this sub is also called from the fault page in order to write the linacstatus and to write clinicalhandover table
            If breakdown Then
                'need to call SetTreatment to write treatment table before anything else is updated


                'LogInName should have been LogOutName. Changed user reason to 7
                'LinacStatusID = DavesCode.Reuse.SetStatus(logInName, "Fault", 5, 5, LinacName, 3)
                LinacStatusID = DavesCode.Reuse.SetStatus(logOutName, "Fault", 5, 103, LinacName, -1)
            Else 'not a breakdown so if approved set clinical - not treating or back to engineering approved
                If logOutName = "System" Then
                    LinacStatusID = DavesCode.Reuse.SetStatus(logOutName, "Linac Unauthorised", 5, 102, LinacName, 3)
                Else
                    LinacStatusID = DavesCode.Reuse.SetStatus(logOutName, "Suspended", 5, 7, LinacName, 3)
                End If

                'LinacStatusID = DavesCode.Reuse.SetStatus(logInName, "Suspended", 5, 7, LinacName, 3)
            End If
            'http://www.mikesdotnetting.com/Article/53/Saving-a-user%27s-CheckBoxList-selection-and-re-populating-the-CheckBoxList-from-saved-data - used for imaging
            'This writes the clinicalstatus table

            Dim commaccept As SqlCommand
            commaccept = New SqlCommand("INSERT INTO ClinicalStatus ( PClinID, LogInDate, LogOutDate, linac, Duration, LogInName, LogOutName,LogOutStatusID, logInStatusID) " &
                                        "VALUES (@PClinID, @LogInDate, @LogOutDate, @linac, @Duration,@LogInName, @LogOutName, @LogOutStatusID, @logInStatusID)", conn)
            'commaccept.Parameters.Add("@CComment", System.Data.SqlDbType.NVarChar, 250)
            'commaccept.Parameters("@CComment").Value = TextBox
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
            Try
                conn.Open()
                commaccept.ExecuteNonQuery()

            Finally
                conn.Close()
                UpdateActivityTable(LinacName, LinacStatusID)
            End Try
            Return LinacStatusID
        End Function

        Public Shared Sub SetTreatment(ByVal State As String, ByVal linacid As String, ByVal linacstate As String)
            Dim time As DateTime
            Dim MachineType As String = linacid
            Dim StateType As String = State
            Dim Linacstatusid As String = linacstate
            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString
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
            Try
                conn.Open()
                commstatus.ExecuteNonQuery()

            Finally
                conn.Close()
            End Try

        End Sub
        'Used
        Public Shared Function SetStatus(ByVal loginName As String, ByVal State As String, ByVal UserGroup As Integer, ByVal Userreason As Integer, ByVal linacid As String, ByVal Activity As Integer) As Integer
            Dim time As DateTime
            Dim userName As String = loginName
            Dim StateType As String = State
            Dim UserType As Integer = UserGroup
            Dim ReasonType As Integer = Userreason
            Dim MachineType As String = linacid
            Dim minutesDuration As Decimal
            Dim StartTime As DateTime
            Dim LinacStatusID As String
            time = Now()
            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString
            Dim commstatus As SqlCommand

            If Activity <> 0 Then
                CalculateDuration(MachineType, time, StartTime, minutesDuration)
            End If

            conn = New SqlConnection(connectionString)


            commstatus = New SqlCommand("INSERT INTO LinacStatus ( State, DateTime, Usergroup, Userreason, linac, UserName) " &
                                        "VALUES ( @State, @Datetime, @Usergroup, @Userreason, @linac, @UserName) SELECT SCOPE_IDENTITY()", conn)
            commstatus.Parameters.Add("@State", System.Data.SqlDbType.NVarChar, 50)
            commstatus.Parameters("@State").Value = StateType
            commstatus.Parameters.Add("@DateTime", System.Data.SqlDbType.DateTime)
            commstatus.Parameters("@DateTime").Value = time
            commstatus.Parameters.Add("@usergroup", System.Data.SqlDbType.Int)
            commstatus.Parameters("@usergroup").Value = UserType
            commstatus.Parameters.Add("@userreason", System.Data.SqlDbType.Int)
            commstatus.Parameters("@userreason").Value = ReasonType
            commstatus.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
            commstatus.Parameters("@linac").Value = MachineType
            commstatus.Parameters.Add("@UserName", System.Data.SqlDbType.NVarChar, 50)
            commstatus.Parameters("@UserName").Value = userName

            Try
                'To get the identity of the record just inserted from
                'http://www.aspsnippets.com/Articles/Return-Identity-Auto-Increment-Column-value-after-record-insert-in-SQL-Server-Database-using-ADONet-with-C-and-VBNet.aspx
                conn.Open()
                'commstatus.ExecuteNonQuery()

                Dim obj As Object = commstatus.ExecuteScalar()
                'Dim LinacStatusIDs As String = obj.ToString()
                LinacStatusID = CInt(obj)
                conn.Close()
                If Activity > 0 Then
                    WriteDurationnew(MachineType, Activity, time, StartTime, minutesDuration, LinacStatusID)
                End If
            Finally
                conn.Close()
            End Try
            Return LinacStatusID
        End Function
        'Used
        Public Shared Sub WriteDurationnew(ByVal Linac As String, ByVal ActivityIn As Integer, ByVal EndTime As DateTime, ByVal StartingTime As DateTime, ByVal Duration As Decimal, ByVal LinacStatusID As Integer)
            Dim time As DateTime
            time = EndTime
            Dim StartTime As DateTime
            StartTime = StartingTime
            Dim LinacName As String = Linac
            Dim Activity As Integer = ActivityIn
            Dim minutesDuration As Decimal
            minutesDuration = Duration
            Dim LinacStateID As Integer
            LinacStateID = LinacStatusID
            Dim conn As SqlConnection
            Dim comm As SqlCommand

            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString

            conn = New SqlConnection(connectionString)
            comm = New SqlCommand("INSERT INTO WriteDuration (Linac, Activity, EndTime,  StartTime, Duration, StatusID) " &
                                        "VALUES (@linac, @Activity, @EndTime, @StartTime, @Duration, @StatusID)", conn)
            comm.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
            comm.Parameters("@linac").Value = LinacName
            comm.Parameters.Add("@Activity", System.Data.SqlDbType.Int)
            comm.Parameters("@Activity").Value = Activity
            comm.Parameters.Add("@EndTime", System.Data.SqlDbType.DateTime)
            comm.Parameters("@EndTime").Value = time
            comm.Parameters.Add("@StartTime", System.Data.SqlDbType.DateTime)
            comm.Parameters("@StartTime").Value = StartTime
            comm.Parameters.Add("@Duration", System.Data.SqlDbType.Decimal)
            comm.Parameters("@Duration").Value = minutesDuration
            comm.Parameters.Add("@StatusID", System.Data.SqlDbType.Int)
            comm.Parameters("@StatusID").Value = LinacStateID

            conn.Open()
            Try
                comm.ExecuteNonQuery()
            Catch ex As Exception
                'Continue
            End Try

            conn.Close()
        End Sub

        Public Shared Sub WriteDuration(ByVal Linac As String, ByVal EndTime As DateTime)
            Dim time As DateTime
            time = EndTime
            Dim reader As SqlDataReader
            Dim StartTime As DateTime
            Dim LinacName As String = Linac
            'Dim Activity As Integer = ActivityIn
            Dim Activity As Integer
            Dim conn As SqlConnection
            Dim comm As SqlCommand
            Dim contime As SqlCommand
            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString

            conn = New SqlConnection(connectionString)

            contime = New SqlCommand("SELECT DateTime, userreason FROM [LinacStatus] where stateID = (Select max(stateID) as lastrecord from [LinacStatus] where linac=@linac)", conn)

            contime.Parameters.AddWithValue("@linac", LinacName)
            conn.Open()
            reader = contime.ExecuteReader()

            If reader.Read() Then
                StartTime = reader.Item("DateTime")
                Activity = reader.Item("userreason")
            End If
            reader.Close()
            conn.Close()

            Dim minutesDuration As Decimal
            Dim duration As TimeSpan = time - StartTime

            minutesDuration = Decimal.Round(duration.TotalMinutes, 2, MidpointRounding.ToEven)


            comm = New SqlCommand("INSERT INTO WriteDuration (Linac, Activity, EndTime,  StartTime, Duration) " &
                                        "VALUES (@linac, @Activity, @EndTime, @StartTime, @Duration)", conn)
            comm.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
            comm.Parameters("@linac").Value = LinacName
            comm.Parameters.Add("@Activity", System.Data.SqlDbType.Int)
            comm.Parameters("@Activity").Value = Activity
            comm.Parameters.Add("@EndTime", System.Data.SqlDbType.DateTime)
            comm.Parameters("@EndTime").Value = time
            comm.Parameters.Add("@StartTime", System.Data.SqlDbType.DateTime)
            comm.Parameters("@StartTime").Value = StartTime
            comm.Parameters.Add("@Duration", System.Data.SqlDbType.Decimal)
            comm.Parameters("@Duration").Value = minutesDuration

            conn.Open()
            comm.ExecuteNonQuery()
            conn.Close()

        End Sub

        Public Shared Sub CalculateDuration(ByVal Linac As String, ByVal EndTime As DateTime, ByRef StartTime As DateTime, ByRef minutesDuration As Decimal)
            Dim time As DateTime
            time = EndTime
            Dim reader As SqlDataReader
            Dim LinacName As String = Linac
            Dim conn As SqlConnection
            Dim contime As SqlCommand
            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString

            conn = New SqlConnection(connectionString)

            contime = New SqlCommand("SELECT DateTime FROM [LinacStatus] where stateID = (Select max(stateID) as lastrecord from [LinacStatus] where linac=@linac)", conn)

            contime.Parameters.AddWithValue("@linac", LinacName)
            conn.Open()
            reader = contime.ExecuteReader()

            If reader.Read() Then
                StartTime = reader.Item("DateTime")
            End If
            reader.Close()
            conn.Close()

            Dim duration As TimeSpan = time - StartTime

            minutesDuration = Decimal.Round(duration.TotalMinutes, 2, MidpointRounding.ToEven)

        End Sub

        'Used
        Public Shared Function GetLastState(ByVal linac As String, ByVal index As Integer) As String
            Dim time As DateTime
            time = Now()
            Dim PreviousState As Integer = index
            Dim reader As SqlDataReader
            Dim nowstatus As String
            Dim linacName As String = linac
            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString
            'Dim Machinestatus As SqlCommand
            Dim StatusNow As SqlCommand
            conn = New SqlConnection(connectionString)

            If PreviousState = 0 Then
                StatusNow = New SqlCommand("SELECT state FROM [LinacStatus] where stateID = (Select max(stateID) as lastrecord from [LinacStatus] where linac=@linac)", conn)
            Else
                'This doesn't work it just gets penultimate record irrespective of linac
                'StatusNow = New SqlCommand("SELECT state FROM [LinacStatus] where stateID = (Select (max(stateID)-1) as penultimaterecord from [LinacStatus] where linac=@linac)", conn)
                'from http://stackoverflow.com/questions/8198962/taking-the-second-last-row-with-only-one-select-in-sql-server
                StatusNow = New SqlCommand("SELECT TOP 1 * From (select Top 2 * from (select * from [LinacStatus] where linac=@linac) as a ORDER BY a.stateid DESC)  as x ORDER BY x.stateid", conn)

            End If
            StatusNow.Parameters.AddWithValue("@linac", linacName)
            conn.Open()
            reader = StatusNow.ExecuteReader()

            If reader.Read() Then
                nowstatus = reader.Item("state").ToString()
            Else
                'added for problem of when new linac so no previous state 6/7/17
                nowstatus = Nothing
            End If
            reader.Close()
            conn.Close()
            Return nowstatus
        End Function

        Public Shared Function GetLastTime(ByVal linac As String, ByVal index As Integer) As String
            Dim time As DateTime
            time = Now()
            Dim PreviousState As Integer = index
            Dim reader As SqlDataReader
            Dim nowstatus As String = "Error"
            Dim linacName As String = linac
            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString
            'Dim Machinestatus As SqlCommand
            Dim StatusNow As SqlCommand
            Dim ResetDayCom As SqlCommand
            Dim oldtime As DateTime
            Dim activity As String = ""
            Dim StateID As String = ""
            Dim oldDayofyear As Integer
            Dim newDayofyear As Integer
            Dim Status As String = ""
            Dim AppState As Integer = 100 ' this is set to 100 to detect if Appstate is null later.
            Dim LogOn As String
            Dim LiveTab As String
            Dim SuspValue As String
            Dim RunupVal As String

            LogOn = "LogOn" + linacName
            LiveTab = "ActTab" + linacName
            SuspValue = "Suspended" + linacName
            RunupVal = "rppTab" + linacName

            conn = New SqlConnection(connectionString)



            If (Not HttpContext.Current.Application(LogOn) Is Nothing) Then
                AppState = CInt(HttpContext.Current.Application(LogOn))
            End If



            If PreviousState = 0 Then

                'StatusNow = New SqlCommand("SELECT dateadd(dd,0, datediff(dd,0,datetime)) FROM [LinacStatus] where stateID = (Select max(stateID) as lastrecord from [LinacStatus] where linac=@linac)", conn)

                'StatusNow = New SqlCommand("SELECT stateid, datetime, userreason FROM [LinacStatus] where stateID = (Select max(stateID) as lastrecord from [LinacStatus] where linac=@linac)", conn)
                StatusNow = New SqlCommand("SELECT TOP 1 stateid, state, datetime, userreason FROM [LinacStatus] where linac=@linac order by StateID desc", conn)
            Else
                'This doesn't work it just gets penultimate record irrespective of linac
                'StatusNow = New SqlCommand("SELECT state FROM [LinacStatus] where stateID = (Select (max(stateID)-1) as penultimaterecord from [LinacStatus] where linac=@linac)", conn)
                'from http://stackoverflow.com/questions/8198962/taking-the-second-last-row-with-only-one-select-in-sql-server
                StatusNow = New SqlCommand("SELECT TOP 1 * From (select Top 2 * from (select * from [LinacStatus] where linac=@linac) as a ORDER BY a.stateid DESC)  as x ORDER BY x.stateid", conn)

            End If
            StatusNow.Parameters.AddWithValue("@linac", linacName)
            conn.Open()
            reader = StatusNow.ExecuteReader()



            If reader.Read() Then
                oldtime = reader.Item("datetime")
                activity = reader.Item("userreason")
                StateID = reader.Item("StateID")
                Status = reader.Item("State")

                oldDayofyear = oldtime.DayOfYear
                newDayofyear = time.DayOfYear
                'oldtime = oldtime.Date.AddDays(-1) test line
                activity = reader.Item("userreason")
                If activity = "102" Then
                    nowstatus = "Ignore"
                ElseIf Not newDayofyear = oldDayofyear Then
                    nowstatus = "EndDay"
                ElseIf newDayofyear = oldDayofyear Then
                    nowstatus = "Ignore"
                End If
            Else
                nowstatus = "Error"
            End If
            reader.Close()
            conn.Close()
            'This line checks to see if Appstate was null, if it was it will still be 100 from the start of the sub.
            'If it is null then the application states are reset depending on the last entry in the database.
            If nowstatus IsNot "Error" Then
                If AppState = 100 Then
                    Select Case activity
                        Case 101, 102, 7, 103
                            HttpContext.Current.Application(LogOn) = 0
                        Case Else
                            HttpContext.Current.Application(LogOn) = 1
                            HttpContext.Current.Application(LiveTab) = activity

                    End Select
                    Select Case Status
                        Case "Suspended"
                            HttpContext.Current.Application(SuspValue) = 1
                        Case "Engineering Approved"
                            HttpContext.Current.Application(RunupVal) = 1

                    End Select
                End If


                'this an instrumentation table it could be removed at a later update.
                ResetDayCom = New SqlCommand("INSERT INTO ResetDay (DateCreated,StateID, State, ApplicationState, Activity, OldDayofYear, newDayofYear,nowstatus, Linac) VALUES (@DateCreated,@StateID, @State, @ApplicationState, @Activity, @OldDayofYear, @newDayofYear,@nowstatus, @Linac)", conn)
                ResetDayCom.Parameters.Add("@DateCreated", System.Data.SqlDbType.DateTime)
                ResetDayCom.Parameters("@DateCreated").Value = time
                ResetDayCom.Parameters.Add("@StateID", System.Data.SqlDbType.Int)
                ResetDayCom.Parameters("@StateID").Value = StateID
                ResetDayCom.Parameters.Add("@State", System.Data.SqlDbType.NVarChar, 50)
                ResetDayCom.Parameters("@State").Value = Status
                ResetDayCom.Parameters.Add("@ApplicationState", System.Data.SqlDbType.Int)
                ResetDayCom.Parameters("@ApplicationState").Value = AppState
                ResetDayCom.Parameters.Add("@Activity", System.Data.SqlDbType.Int)
                ResetDayCom.Parameters("@Activity").Value = activity
                ResetDayCom.Parameters.Add("@OldDayofYear", System.Data.SqlDbType.Int)
                ResetDayCom.Parameters("@OldDayofYear").Value = oldDayofyear
                ResetDayCom.Parameters.Add("@newDayofYear", System.Data.SqlDbType.Int)
                ResetDayCom.Parameters("@newDayofYear").Value = newDayofyear
                ResetDayCom.Parameters.Add("@nowstatus", System.Data.SqlDbType.NVarChar, 10)
                ResetDayCom.Parameters("@nowstatus").Value = nowstatus
                ResetDayCom.Parameters.Add("@linac", System.Data.SqlDbType.NVarChar, 10)
                ResetDayCom.Parameters("@linac").Value = linacName

                conn.Open()
                ResetDayCom.ExecuteNonQuery()
                conn.Close()
            End If
            Return nowstatus
        End Function

        Public Shared Function GetLastTech(ByVal linac As String, ByVal index As Integer, ByRef lastState As String, ByRef lastusername As String, ByRef lastusergroup As Integer) As Integer
            'Changed this to a function to return the linac state ID where necessary
            Dim laststateid As Integer = 0
            Dim PreviousState As Integer = index
            Dim reader As SqlDataReader
            Dim linacName As String = linac
            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString
            'Dim Machinestatus As SqlCommand
            Dim StatusNow As SqlCommand
            conn = New SqlConnection(connectionString)

            If PreviousState = 0 Then
                StatusNow = New SqlCommand("SELECT state, username, usergroup, stateID FROM [LinacStatus] where stateID = (Select max(stateID) as lastrecord from [LinacStatus] where linac=@linac)", conn)
            Else
                'This doesn't work it just gets penultimate record irrespective of linac
                'StatusNow = New SqlCommand("SELECT state, username, usergroup FROM [LinacStatus] where stateID = (Select (max(stateID)-1) as penultimaterecord from [LinacStatus] where linac=@linac)", conn)
                'from http://stackoverflow.com/questions/8198962/taking-the-second-last-row-with-only-one-select-in-sql-server
                StatusNow = New SqlCommand("SELECT TOP 1 * From (select Top 2 * from (select * from [LinacStatus] where linac=@linac) as a ORDER BY a.stateid DESC)  as x ORDER BY x.stateid", conn)

            End If
            StatusNow.Parameters.AddWithValue("@linac", linacName)
            conn.Open()
            reader = StatusNow.ExecuteReader()

            If reader.Read() Then
                lastState = reader.Item("state").ToString()
                lastusername = reader.Item("username").ToString()
                lastusergroup = reader.Item("usergroup")
                laststateid = reader.Item("stateID")
            End If
            reader.Close()
            conn.Close()

            Return laststateid
        End Function

        Public Shared Function CheckForOpenFault(ByVal machinename As String) As Boolean
            Dim openfault As Boolean = False
            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString
            Dim existingfault As SqlCommand
            Dim LinacStatusID As String = ""
            Dim reader As SqlDataReader
            conn = New SqlConnection(connectionString)

            existingfault = New SqlCommand("SELECT TOP(1) [IncidentID], [StatusID] FROM [FaultIDTable] where Linac = @linac and ReportClosed is Null and statusid is not NULL ORDER BY [IncidentID] DESC", conn)
            existingfault.Parameters.AddWithValue("@linac", machinename)
            conn.Open()
            reader = existingfault.ExecuteReader()
            If reader.HasRows() Then
                'Have to now actually read the rows
                reader.Read()
                LinacStatusID = reader.Item("StatusID").ToString()
                openfault = True
                'Application(appstate) = 1
            End If

            Return openfault
        End Function

        Public Shared Sub ArchiveEnergies(ByVal EnergyIndex As Integer)
            Dim conn As SqlConnection
            Dim connectionString As String = ConfigurationManager.ConnectionStrings(
            "connectionstring").ConnectionString
            Dim archive As SqlCommand

            conn = New SqlConnection(connectionString)

            archive = New SqlCommand("Insert INTO  [PhysicsEnergiesArchive] Select * from [PhysicsEnergies] where EnergyID = @EnergyID", conn)
            archive.Parameters.AddWithValue("@EnergyID", EnergyIndex)
            conn.Open()
            archive.ExecuteNonQuery()
            conn.Close()

        End Sub

        Public Shared Sub ReturnImaging(ByRef iView As Boolean, ByRef XVi As Boolean, ByVal GridViewImage As GridView, ByVal MachineName As String)
            Dim cb As CheckBox
            Select Case MachineName

                Case "LA1", "LA2", "LA3"
                    cb = CType(GridViewImage.Rows(0).FindControl("RowLevelCheckBoxImage"), CheckBox)
                    iView = cb.Checked
                    'added E1 and LA6 for eastbourne 4/7/17
                Case Else
                    cb = CType(GridViewImage.Rows(0).FindControl("RowLevelCheckBoxImage"), CheckBox)
                    iView = cb.Checked
                    cb = CType(GridViewImage.Rows(1).FindControl("RowLevelCheckBoxImage"), CheckBox)
                    XVi = cb.Checked
            End Select
        End Sub

    End Class
End Namespace
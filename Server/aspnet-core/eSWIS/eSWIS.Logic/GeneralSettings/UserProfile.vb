'Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General
Imports System.Data.SqlClient

Namespace UsrProfile
    Public NotInheritable Class UsrProfile
        Inherits Core2.CoreControl
        'Private USRPROFILEInfo As USRPROFILEInfo = New USRPROFILEInfo
        'Private Log As New SystemLog()

        Private connStr As String
        Private privateConn As SqlConnection

        Public Sub New(ByVal _connStr As String)
            connStr = _connStr
            ConnectionString = _connStr
            ConnectionSetup()
            privateConn = New SqlConnection(connStr)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        ' Private Function Save(ByVal USRPROFILECont As Container.USRPROFILE, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
        '     Dim strSQL As String = ""
        '     Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
        '     Dim rdr As System.Data.SqlClient.SqlDataReader
        '     Save = False
        '     Try
        '         If USRPROFILECont Is Nothing Then
        '             'Message return
        '         Else
        '             blnExec = False
        '             blnFound = False
        '             blnFlag = False
        '             If StartConnection(EnumIsoState.StateUpdatetable) = True Then
        '                 StartSQLControl()
        '                 With USRPROFILEInfo.MyInfo
        '                     strSQL = BuildSelect(.CheckFields, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, USRPROFILECont.UserID) & "'")
        '                 End With
        '                 rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
        '                 blnExec = True

        '                 If rdr Is Nothing = False Then
        '                     With rdr
        '                         If .Read Then
        '                             blnFound = True
        '                             If Convert.ToInt16(.Item("Flag")) = 0 Then
        '                                 'Found but deleted
        '                                 blnFlag = False
        '                             Else
        '                                 'Found and active
        '                                 blnFlag = True
        '                             End If
        '                         End If
        '                         .Close()
        '                     End With
        '                 End If
        '             End If

        '             If blnExec Then
        '                 If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
        '                     message = "Record already exist"
        '                     Return False
        '                 Else
        '                     StartSQLControl()
        '                     With objSQL
        '                         .TableName = "USRPROFILE WITH (ROWLOCK)"
        '                         .AddField("UserName", USRPROFILECont.UserName, SQLControl.EnumDataType.dtStringN)
        '                         .AddField("Password", USRPROFILECont.Password, SQLControl.EnumDataType.dtString)
        '                         .AddField("RefID", USRPROFILECont.RefID, SQLControl.EnumDataType.dtString)
        '                         .AddField("RefType", USRPROFILECont.RefType, SQLControl.EnumDataType.dtNumeric)
        '                         .AddField("ParentID", USRPROFILECont.ParentID, SQLControl.EnumDataType.dtString)
        '                         .AddField("Status", USRPROFILECont.Status, SQLControl.EnumDataType.dtNumeric)
        '                         .AddField("Logged", USRPROFILECont.Logged, SQLControl.EnumDataType.dtNumeric)
        '                         .AddField("LogStation", USRPROFILECont.LogStation, SQLControl.EnumDataType.dtString)
        '                         .AddField("LastLogin", USRPROFILECont.LastLogin, SQLControl.EnumDataType.dtDateTime)
        '                         .AddField("LastLogout", USRPROFILECont.LastLogout, SQLControl.EnumDataType.dtDateTime)
        '                         .AddField("CreateDate", USRPROFILECont.CreateDate, SQLControl.EnumDataType.dtDateTime)
        '                         .AddField("CreateBy", USRPROFILECont.CreateBy, SQLControl.EnumDataType.dtString)
        '                         .AddField("LastUpdate", USRPROFILECont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
        '                         .AddField("UpdateBy", USRPROFILECont.UpdateBy, SQLControl.EnumDataType.dtString)
        '                         .AddField("IsHost", USRPROFILECont.IsHost, SQLControl.EnumDataType.dtNumeric)

        '                         Select Case pType
        '                             Case SQLControl.EnumSQLType.stInsert
        '                                 If blnFound = True And blnFlag = False Then
        '                                     strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "Where UserID ='" & .ParseValue(SQLControl.EnumDataType.dtString, USRPROFILECont.UserID) & "'")
        '                                 Else
        '                                     If blnFound = False Then
        '                                         .AddField("UserID", USRPROFILECont.UserID, SQLControl.EnumDataType.dtString)
        '                                         strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
        '                                     End If
        '                                 End If
        '                             Case SQLControl.EnumSQLType.stUpdate
        '                                 strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE UserID='" & .ParseValue(SQLControl.EnumDataType.dtString, USRPROFILECont.UserID) & "'")
        '                         End Select
        '                     End With
        '                     Try
        '                         objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
        '                         Return True

        '                     Catch axExecute As Exception
        '                         If pType = SQLControl.EnumSQLType.stInsert Then
        '                             message = axExecute.Message.ToString()
        '                         Else
        '                             message = axExecute.Message.ToString()
        '                         End If
        '                         Log.Notifier.Notify(axExecute)
        '                         Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/UserProfile", axExecute.Message & strSQL, axExecute.StackTrace)
        '                         Return False

        '                     Finally
        '                         objSQL.Dispose()
        '                     End Try
        '                 End If

        '             End If
        '         End If
        '     Catch axAssign As ApplicationException
        '         message = axAssign.Message.ToString()
        '         Log.Notifier.Notify(axAssign)
        '         Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/UserProfile", axAssign.Message, axAssign.StackTrace)
        '         Return False
        '     Catch exAssign As SystemException
        '         message = exAssign.Message.ToString()
        '         Log.Notifier.Notify(exAssign)
        '         Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/UserProfile", exAssign.Message, exAssign.StackTrace)
        '         Return False
        '     Finally
        '         USRPROFILECont = Nothing
        '         rdr = Nothing
        '         EndSQLControl()
        '         EndConnection()
        '     End Try
        ' End Function

        ' 'ADD
        ' Public Function Insert(ByVal USRPROFILECont As Container.USRPROFILE, ByRef message As String) As Boolean
        '     Return Save(USRPROFILECont, SQLControl.EnumSQLType.stInsert, message)
        ' End Function

        ' 'AMEND
        ' Public Function Update(ByVal USRPROFILECont As Container.USRPROFILE, ByRef message As String) As Boolean
        '     Return Save(USRPROFILECont, SQLControl.EnumSQLType.stUpdate, message)
        ' End Function

        ' Public Function Delete(ByVal USRPROFILECont As Container.USRPROFILE, ByRef message As String) As Boolean
        '     Dim strSQL As String
        '     Dim blnFound As Boolean
        '     Dim blnInUse As Boolean
        '     Dim rdr As System.Data.SqlClient.SqlDataReader
        '     Delete = False
        '     blnFound = False
        '     blnInUse = False
        '     Try
        '         If USRPROFILECont Is Nothing Then
        '             'Error Message
        '         Else
        '             If StartConnection(EnumIsoState.StateUpdatetable) = True Then
        '                 StartSQLControl()
        '                 With USRPROFILEInfo.MyInfo
        '                     strSQL = BuildSelect(.CheckFields, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, USRPROFILECont.UserID) & "'")
        '                 End With
        '                 rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

        '                 If rdr Is Nothing = False Then
        '                     With rdr
        '                         If .Read Then
        '                             blnFound = True
        '                             If Convert.ToInt16(.Item("InUse")) = 1 Then
        '                                 blnInUse = True
        '                             End If
        '                         End If
        '                         .Close()
        '                     End With
        '                 End If

        '                 If blnFound = True And blnInUse = True Then
        '                     With objSQL
        '                         strSQL = BuildUpdate("USRPROFILE WITH (ROWLOCK)", " SET Flag = 0" & _
        '                         " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
        '                         objSQL.ParseValue(SQLControl.EnumDataType.dtString, USRPROFILECont.UpdateBy) & "'" & _
        '                         " WHERE UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, USRPROFILECont.UserID) & "'")
        '                     End With
        '                 End If

        '                 If blnFound = True And blnInUse = False Then
        '                     strSQL = BuildDelete("USRPROFILE WITH (ROWLOCK)", "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, USRPROFILECont.UserID) & "'")
        '                 End If

        '                 Try
        '                     objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
        '                     Return True
        '                 Catch exExecute As Exception
        '                     message = exExecute.Message.ToString()
        '                     Log.Notifier.Notify(exExecute)
        '                     Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/UserProfile", exExecute.Message & strSQL, exExecute.StackTrace)
        '                     Return False
        '                 End Try
        '             End If
        '         End If

        '     Catch axDelete As ApplicationException
        '         message = axDelete.Message.ToString()
        '         Log.Notifier.Notify(axDelete)
        '         Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/UserProfile", axDelete.Message, axDelete.StackTrace)
        '         Return False
        '     Catch exDelete As Exception
        '         message = exDelete.Message.ToString()
        '         Log.Notifier.Notify(exDelete)
        '         Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/UserProfile", exDelete.Message, exDelete.StackTrace)
        '         Return False
        '     Finally
        '         USRPROFILECont = Nothing
        '         rdr = Nothing
        '         EndSQLControl()
        '         EndConnection()
        '     End Try
        ' End Function

        Public Function Insert(ByVal cont As Container.USRPROFILE) As Boolean
            'Return Save(UsrverifyCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
            Dim affected = 0
            Try
                If StartConnection() = True Then
                    'With UsrverifyInfo.MyInfo
                    StartSQLControl()
                    Dim strSQL = "INSERT INTO USRPROFILE(UserID, UserName, Password, RefID, CreateDate, LastUpdate, Status) " &
                                 "VALUES(@userID, @userName, @password, @refID, @createDate, @lastUpdate, @status)"
                    Dim cmd = New SqlCommand(strSQL, Conn)
                    cmd.Parameters.AddWithValue("@userID", cont.UserID)
                    cmd.Parameters.AddWithValue("@userName", cont.UserName)
                    cmd.Parameters.AddWithValue("@password", cont.Password)
                    cmd.Parameters.AddWithValue("@refID", cont.RefID)
                    cmd.Parameters.AddWithValue("@createDate", DateTime.Now)
                    cmd.Parameters.AddWithValue("@lastUpdate", DateTime.Now)
                    cmd.Parameters.AddWithValue("@status", 1)
                    affected = cmd.ExecuteNonQuery()
                    If affected <= 0 Then
                        Return False
                    End If
                    strSQL = "INSERT INTO USRAPP(UserID, AppID, AccessCode, SyncCreate, SyncLastUpd) " &
                                 "VALUES(@userID, (SELECT G.AppID FROM UsrGroup G WHERE G.GroupCode=@accessCode), @accessCode, @syncCreate, @syncLastUpd)"
                    cmd = New SqlCommand(strSQL, Conn)
                    cmd.Parameters.AddWithValue("@userID", cont.UserID)
                    cmd.Parameters.AddWithValue("@accessCode", cont.AccessCode)
                    cmd.Parameters.AddWithValue("@syncCreate", DateTime.Now)
                    cmd.Parameters.AddWithValue("@syncLastUpd", DateTime.Now)
                    affected = cmd.ExecuteNonQuery()
                    If affected <= 0 Then
                        Return False
                    End If
                    Return True
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Return False
                'Log.Notifier.Notify(ex)
                'Gibraltar.Agent.Log.Error("UserSecurity/UsrVerify", ex.Message, ex.StackTrace)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function Update(ByVal cont As Container.USRPROFILE) As Boolean
            'Return Save(UsrverifyCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
            Dim affected = 0
            Try
                If StartConnection() = True Then
                    'With UsrverifyInfo.MyInfo
                    StartSQLControl()
                    Dim strSQL As String = ""
                    If cont.Password = "" Then
                        strSQL = "UPDATE USRPROFILE SET UserName = @userName, RefID = @refID, LastUpdate = @lastUpdate " &
                                 "WHERE UserID = @userID"
                    Else
                        strSQL = "UPDATE USRPROFILE SET UserName = @userName, RefID = @refID, LastUpdate = @lastUpdate,  Password = @password " &
                                 "WHERE UserID = @userID"
                    End If

                    Dim cmd = New SqlCommand(strSQL, Conn)
                    cmd.Parameters.AddWithValue("@userID", cont.UserID)
                    cmd.Parameters.AddWithValue("@userName", cont.UserName)
                    cmd.Parameters.AddWithValue("@refID", cont.RefID)
                    cmd.Parameters.AddWithValue("@lastUpdate", DateTime.Now)
                    If cont.Password <> "" Then
                        cmd.Parameters.AddWithValue("@password", cont.Password)
                    End If
                    affected = cmd.ExecuteNonQuery()
                    If affected <= 0 Then
                        Return False
                    End If
                    strSQL = "UPDATE USRAPP SET AppID = (SELECT G.AppID FROM UsrGroup G WHERE G.GroupCode=@accessCode), AccessCode = @accessCode, SyncLastUpd = @syncLastUpd " &
                            "WHERE UserID = @userID"
                    cmd = New SqlCommand(strSQL, Conn)
                    cmd.Parameters.AddWithValue("@userID", cont.UserID)
                    cmd.Parameters.AddWithValue("@accessCode", cont.AccessCode)
                    cmd.Parameters.AddWithValue("@syncLastUpd", DateTime.Now)
                    affected = cmd.ExecuteNonQuery()
                    If affected <= 0 Then
                        Return False
                    End If
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Return False
                'Log.Notifier.Notify(ex)
                'Gibraltar.Agent.Log.Error("UserSecurity/UsrVerify", ex.Message, ex.StackTrace)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function Delete(ByVal id As System.String) As Boolean
            'Return Save(UsrverifyCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
            Dim affected = 0
            Try
                If StartConnection() = True Then
                    'With UsrverifyInfo.MyInfo
                    StartSQLControl()
                    Dim strSQL = "DELETE FROM USRPROFILE " &
                                 "WHERE UserID = @userID"
                    Dim cmd = New SqlCommand(strSQL, Conn)
                    cmd.Parameters.AddWithValue("@userID", id)
                    affected = cmd.ExecuteNonQuery()
                    If affected <= 0 Then
                        Return False
                    End If
                    Return True
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Return False
                'Log.Notifier.Notify(ex)
                'Gibraltar.Agent.Log.Error("UserSecurity/UsrVerify", ex.Message, ex.StackTrace)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        ''' <summary>
        ''' List all items
        ''' </summary>
        Public Overloads Function GetUserProfile(Optional ByVal SkipCount As Int32 = -1, Optional ByVal Limit As Int32 = -1, Optional ByVal OrderBy As String = "", Optional ByVal Filter As String = "", Optional ByRef totalRows As Int32 = 0) As List(Of Container.USRPROFILE)
            Dim rUserProfile As Container.USRPROFILE = Nothing
            Dim listUserProfile As List(Of Container.USRPROFILE) = New List(Of Container.USRPROFILE)
            Dim dtTemp As DataTable = New DataTable()

            Dim strSQL As String = ""
            Try
                Dim column = ""

                column = "Select " &
                           "    ISNULL(U.UserID, '') AS UserID,  " &
                           "    ISNULL(U.UserName, '') AS UserName, " &
                           "    ISNULL(U.LastLogin, '') AS LastLogin, " &
                           "    ISNULL(U.RefID, '') AS RefID, " &
                           "    ISNULL(U.Status, '') AS Status, " &
                           "    ISNULL(CD.CodeDesc, '') AS StatusDesc, " &
                           "        ISNULL(UA.AccessCode, '') AS AccessCode, " &
                           "        ISNULL(UG.GroupName, '') AS AccessDesc "

                strSQL = "FROM  " &
                           "    USRPROFILE U  " &
                           "INNER JOIN USRAPP UA ON U.UserID = UA.UserID " &
                           "INNER JOIN USRGROUP UG ON UA.AccessCode = UG.GroupCode " &
                           "INNER JOIN CODEMASTER CD ON U.Status = CD.Code AND CD.CodeType = 'USR'  " &
                           "WHERE  " &
                           "    U.Status = 1 "

                Dim condition = ""

                If Not String.IsNullOrEmpty(Filter) Then
                    condition = String.Format("AND (U.UserID LIKE '%{0}%' OR U.UserName LIKE '%{0}%' OR U.RefID LIKE '%{0}%')", Filter)
                End If

                Dim sqlLimit = ""
                If SkipCount >= 0 And Limit > 0 Then
                    sqlLimit = String.Format("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", SkipCount, Limit)
                End If

                strSQL = String.Format("{0} {1}", strSQL, condition)

                Dim adapter As SqlDataAdapter
                If SkipCount >= 0 And Limit > 0 Then
                    Dim sqlCount = String.Format("SELECT COUNT(*) AS TotalRows {0}", strSQL)

                    StartConnection()
                    adapter = New SqlDataAdapter(sqlCount, Conn)

                    'dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
                    adapter.Fill(dtTemp)

                    If dtTemp Is Nothing = False Then
                        For Each drRow As DataRow In dtTemp.Rows
                            totalRows = drRow.Item("TotalRows")
                        Next
                    End If
                    EndConnection()
                Else

                End If

                OrderBy = String.Format("ORDER BY {0}", OrderBy)
                Dim dtActual As DataTable = New DataTable()
                StartConnection()
                Dim sqlSelect = String.Format("{0} {1} {2} {3}", column, strSQL, OrderBy, sqlLimit)
                adapter = New SqlDataAdapter(sqlSelect, Conn)

                'dtActual = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
                adapter.Fill(dtActual)

                If dtActual IsNot Nothing AndAlso dtActual.Rows.Count > 0 Then
                    listUserProfile = New List(Of Container.USRPROFILE)
                    For Each row As DataRow In dtActual.Rows
                        rUserProfile = New Container.USRPROFILE
                        With rUserProfile
                            rUserProfile = New Container.USRPROFILE
                            rUserProfile.UserID = row.Item("UserID")
                            rUserProfile.UserName = row.Item("UserName")
                            rUserProfile.LastLogin = row.Item("LastLogin")
                            rUserProfile.RefID = row.Item("RefID")
                            rUserProfile.Status = row.Item("Status")
                            rUserProfile.StatusDesc = row.Item("StatusDesc")
                            rUserProfile.AccessCode = row.Item("AccessCode")
                            rUserProfile.AccessDesc = row.Item("AccessDesc")
                            'rUserProfile.s = row.Item("RefID")
                        End With
                        listUserProfile.Add(rUserProfile)
                    Next
                End If
            Catch ex As Exception
                Dim temp As String = ex.ToString()
            Finally
                privateConn.Close()
            End Try

            privateConn.Close()
            Return listUserProfile
        End Function

        ''' <summary>
        ''' Return login credentials for authentication
        ''' </summary>
        Public Overloads Function GetUserProfile(ByVal UserName As String, Optional ByVal Password As String = "") As Container.USRPROFILE
            Dim rUserProfile As Container.USRPROFILE = Nothing
            Dim dtTemp As DataTable = New DataTable()

            privateConn.Open()
            Try
                Dim strSQL = "SELECT p.UserID AS UserID, p.UserName AS UserName, g.GroupCode AS GroupCode, g.GroupName AS GroupName FROM USRPROFILE p INNER JOIN USRAPP a ON p.UserID = a.UserID INNER JOIN USRGROUP g ON a.AccessCode = g.GroupCode WHERE (p.UserName = '" & UserName & "' OR p.UserID = '" & UserName & "')"
                If Not String.IsNullOrEmpty(Password) Then
                    strSQL = strSQL & " AND p.Password = '" & Password & "'"
                End If

                Dim adapter As SqlDataAdapter = New SqlDataAdapter(strSQL, privateConn)
                adapter.Fill(dtTemp)

                If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
                    Dim drRow = dtTemp.Rows(0)
                    rUserProfile = New Container.USRPROFILE
                    rUserProfile.UserID = drRow.Item("UserID")
                    rUserProfile.UserName = drRow.Item("UserName")
                    rUserProfile.GroupCode = drRow.Item("GroupCode")
                    rUserProfile.GroupName = drRow.Item("GroupName")
                End If
            Catch ex As Exception
                Dim temp As String = ex.ToString()
            Finally
                privateConn.Close()
            End Try

            privateConn.Close()
            Return rUserProfile
        End Function
#End Region
    End Class


    Namespace Container
#Region "Class Container"
        Public Class USRPROFILE
            Public fUserID As System.String = "UserID"
            Public fUserName As System.String = "UserName"
            Public fPassword As System.String = "Password"
            Public fRefID As System.String = "RefID"
            Public fRefType As System.String = "RefType"
            Public fParentID As System.String = "ParentID"
            Public fStatus As System.String = "Status"
            Public fLogged As System.String = "Logged"
            Public fLogStation As System.String = "LogStation"
            Public fLastLogin As System.String = "LastLogin"
            Public fLastLogout As System.String = "LastLogout"
            Public fCreateDate As System.String = "CreateDate"
            Public fCreateBy As System.String = "CreateBy"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fUpdateBy As System.String = "UpdateBy"
            Public frowguid As System.String = "rowguid"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fIsHost As System.String = "IsHost"
            Public fLastSyncBy As System.String = "LastSyncBy"
            Public fGroupCode As System.String = "GroupCode"
            Public fGroupName As System.String = "GroupName"

            Protected _UserID As System.String
            Private _UserName As System.String
            Private _Password As System.String
            Private _RefID As System.String
            Private _RefType As System.Byte
            Private _ParentID As System.String
            Private _Status As System.Byte
            Private _StatusDesc As System.String
            Private _AccessCode As System.String
            Private _AccessDesc As System.String
            Private _Logged As System.Byte
            Private _LogStation As System.String
            Private _LastLogin As System.DateTime
            Private _LastLogout As System.DateTime
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _IsHost As System.Byte
            Private _LastSyncBy As System.String
            Private _GroupCode As System.String
            Private _GroupName As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property UserID As System.String
                Get
                    Return _UserID
                End Get
                Set(ByVal Value As System.String)
                    _UserID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property UserName As System.String
                Get
                    Return _UserName
                End Get
                Set(ByVal Value As System.String)
                    _UserName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Password As System.String
                Get
                    Return _Password
                End Get
                Set(ByVal Value As System.String)
                    _Password = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RefID As System.String
                Get
                    Return _RefID
                End Get
                Set(ByVal Value As System.String)
                    _RefID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RefType As System.Byte
                Get
                    Return _RefType
                End Get
                Set(ByVal Value As System.Byte)
                    _RefType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ParentID As System.String
                Get
                    Return _ParentID
                End Get
                Set(ByVal Value As System.String)
                    _ParentID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Status As System.Byte
                Get
                    Return _Status
                End Get
                Set(ByVal Value As System.Byte)
                    _Status = Value
                End Set
            End Property

            Public Property StatusDesc As System.String
                Get
                    Return _StatusDesc
                End Get
                Set(ByVal Value As System.String)
                    _StatusDesc = Value
                End Set
            End Property

            Public Property AccessCode As System.String
                Get
                    Return _AccessCode
                End Get
                Set(ByVal Value As System.String)
                    _AccessCode = Value
                End Set
            End Property

            Public Property AccessDesc As System.String
                Get
                    Return _AccessDesc
                End Get
                Set(ByVal Value As System.String)
                    _AccessDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Logged As System.Byte
                Get
                    Return _Logged
                End Get
                Set(ByVal Value As System.Byte)
                    _Logged = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LogStation As System.String
                Get
                    Return _LogStation
                End Get
                Set(ByVal Value As System.String)
                    _LogStation = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property LastLogin As System.DateTime
                Get
                    Return _LastLogin
                End Get
                Set(ByVal Value As System.DateTime)
                    _LastLogin = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property LastLogout As System.DateTime
                Get
                    Return _LastLogout
                End Get
                Set(ByVal Value As System.DateTime)
                    _LastLogout = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property CreateDate As System.DateTime
                Get
                    Return _CreateDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _CreateDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CreateBy As System.String
                Get
                    Return _CreateBy
                End Get
                Set(ByVal Value As System.String)
                    _CreateBy = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property LastUpdate As System.DateTime
                Get
                    Return _LastUpdate
                End Get
                Set(ByVal Value As System.DateTime)
                    _LastUpdate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property UpdateBy As System.String
                Get
                    Return _UpdateBy
                End Get
                Set(ByVal Value As System.String)
                    _UpdateBy = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property rowguid As System.Guid
                Get
                    Return _rowguid
                End Get
                Set(ByVal Value As System.Guid)
                    _rowguid = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SyncCreate As System.DateTime
                Get
                    Return _SyncCreate
                End Get
                Set(ByVal Value As System.DateTime)
                    _SyncCreate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SyncLastUpd As System.DateTime
                Get
                    Return _SyncLastUpd
                End Get
                Set(ByVal Value As System.DateTime)
                    _SyncLastUpd = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsHost As System.Byte
                Get
                    Return _IsHost
                End Get
                Set(ByVal Value As System.Byte)
                    _IsHost = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LastSyncBy As System.String
                Get
                    Return _LastSyncBy
                End Get
                Set(ByVal Value As System.String)
                    _LastSyncBy = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property GroupCode As System.String
                Get
                    Return _GroupCode
                End Get
                Set(ByVal Value As System.String)
                    _GroupCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property GroupName As System.String
                Get
                    Return _GroupName
                End Get
                Set(ByVal Value As System.String)
                    _GroupName = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    ' Public Class USRPROFILEInfo
    '     Inherits Core.CoreBase
    '     Protected Overrides Sub InitializeClassInfo()
    '         With MyInfo
    '             .FieldsList = "UserID,UserName,Password,RefID,RefType,ParentID,Status,Logged,LogStation,LastLogin,LastLogout,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
    '             .CheckFields = "RefType,Status,Logged,IsHost"
    '             .TableName = "USRPROFILE WITH (NOLOCK)"
    '             .DefaultCond = Nothing
    '             .DefaultOrder = Nothing
    '             .Listing = "UserID,UserName,Password,RefID,RefType,ParentID,Status,Logged,LogStation,LastLogin,LastLogout,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
    '             .ListingCond = Nothing
    '             .ShortList = Nothing
    '             .ShortListCond = Nothing
    '         End With
    '     End Sub

    '     Public Function JoinTableField(ByVal Prefix As String, ByVal FieldList As String) As String
    '         Dim aFieldList As Array
    '         Dim strFieldList As String = Nothing
    '         aFieldList = FieldList.Split(",")
    '         If Not aFieldList Is Nothing Then
    '             For Each Str As String In aFieldList
    '                 If strFieldList Is Nothing Then
    '                     strFieldList = Prefix & "." & Str
    '                 Else
    '                     strFieldList &= "," & Prefix & "." & Str
    '                 End If
    '             Next
    '         End If
    '         aFieldList = Nothing

    '         Return strFieldList
    '     End Function
    ' End Class
#End Region

#Region "Scheme"
    ' Public Class UsrProfileScheme
    '     Inherits Core.SchemeBase
    '     Protected Overrides Sub InitializeInfo()

    '         With this
    '             .DataType = SQLControl.EnumDataType.dtString
    '             .FieldName = "UserID"
    '             .Length = 20
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = True
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(0, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtStringN
    '             .FieldName = "UserName"
    '             .Length = 50
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = True
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(1, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtString
    '             .FieldName = "Password"
    '             .Length = 20
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = True
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(2, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtString
    '             .FieldName = "RefID"
    '             .Length = 20
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = True
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(3, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtNumeric
    '             .FieldName = "RefType"
    '             .Length = 1
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = True
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(4, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtString
    '             .FieldName = "ParentID"
    '             .Length = 20
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = True
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(5, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtNumeric
    '             .FieldName = "Status"
    '             .Length = 1
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = True
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(6, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtNumeric
    '             .FieldName = "Logged"
    '             .Length = 1
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = True
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(7, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtString
    '             .FieldName = "LogStation"
    '             .Length = 10
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = True
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(8, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtDateTime
    '             .FieldName = "LastLogin"
    '             .Length = 8
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = False
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(9, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtDateTime
    '             .FieldName = "LastLogout"
    '             .Length = 8
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = False
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(10, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtDateTime
    '             .FieldName = "CreateDate"
    '             .Length = 8
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = False
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(11, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtString
    '             .FieldName = "CreateBy"
    '             .Length = 20
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = True
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(12, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtDateTime
    '             .FieldName = "LastUpdate"
    '             .Length = 8
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = False
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(13, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtString
    '             .FieldName = "UpdateBy"
    '             .Length = 20
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = True
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(14, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtString
    '             .FieldName = "rowguid"
    '             .Length = 16
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = True
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(15, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtDateTime
    '             .FieldName = "SyncCreate"
    '             .Length = 8
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = True
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(16, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtDateTime
    '             .FieldName = "SyncLastUpd"
    '             .Length = 8
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = True
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(17, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtNumeric
    '             .FieldName = "IsHost"
    '             .Length = 1
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = True
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(18, this)
    '         With this
    '             .DataType = SQLControl.EnumDataType.dtString
    '             .FieldName = "LastSyncBy"
    '             .Length = 10
    '             .DecPlace = Nothing
    '             .RegExp = String.Empty
    '             .IsMandatory = True
    '             .AllowNegative = False
    '         End With
    '         MyBase.AddItem(19, this)

    '     End Sub

    '     Public ReadOnly Property UserID As StrucElement
    '         Get
    '             Return MyBase.GetItem(0)
    '         End Get
    '     End Property

    '     Public ReadOnly Property UserName As StrucElement
    '         Get
    '             Return MyBase.GetItem(1)
    '         End Get
    '     End Property
    '     Public ReadOnly Property Password As StrucElement
    '         Get
    '             Return MyBase.GetItem(2)
    '         End Get
    '     End Property
    '     Public ReadOnly Property RefID As StrucElement
    '         Get
    '             Return MyBase.GetItem(3)
    '         End Get
    '     End Property
    '     Public ReadOnly Property RefType As StrucElement
    '         Get
    '             Return MyBase.GetItem(4)
    '         End Get
    '     End Property
    '     Public ReadOnly Property ParentID As StrucElement
    '         Get
    '             Return MyBase.GetItem(5)
    '         End Get
    '     End Property
    '     Public ReadOnly Property Status As StrucElement
    '         Get
    '             Return MyBase.GetItem(6)
    '         End Get
    '     End Property
    '     Public ReadOnly Property Logged As StrucElement
    '         Get
    '             Return MyBase.GetItem(7)
    '         End Get
    '     End Property
    '     Public ReadOnly Property LogStation As StrucElement
    '         Get
    '             Return MyBase.GetItem(8)
    '         End Get
    '     End Property
    '     Public ReadOnly Property LastLogin As StrucElement
    '         Get
    '             Return MyBase.GetItem(9)
    '         End Get
    '     End Property
    '     Public ReadOnly Property LastLogout As StrucElement
    '         Get
    '             Return MyBase.GetItem(10)
    '         End Get
    '     End Property
    '     Public ReadOnly Property CreateDate As StrucElement
    '         Get
    '             Return MyBase.GetItem(11)
    '         End Get
    '     End Property
    '     Public ReadOnly Property CreateBy As StrucElement
    '         Get
    '             Return MyBase.GetItem(12)
    '         End Get
    '     End Property
    '     Public ReadOnly Property LastUpdate As StrucElement
    '         Get
    '             Return MyBase.GetItem(13)
    '         End Get
    '     End Property
    '     Public ReadOnly Property UpdateBy As StrucElement
    '         Get
    '             Return MyBase.GetItem(14)
    '         End Get
    '     End Property
    '     Public ReadOnly Property rowguid As StrucElement
    '         Get
    '             Return MyBase.GetItem(15)
    '         End Get
    '     End Property
    '     Public ReadOnly Property SyncCreate As StrucElement
    '         Get
    '             Return MyBase.GetItem(16)
    '         End Get
    '     End Property
    '     Public ReadOnly Property SyncLastUpd As StrucElement
    '         Get
    '             Return MyBase.GetItem(17)
    '         End Get
    '     End Property
    '     Public ReadOnly Property IsHost As StrucElement
    '         Get
    '             Return MyBase.GetItem(18)
    '         End Get
    '     End Property
    '     Public ReadOnly Property LastSyncBy As StrucElement
    '         Get
    '             Return MyBase.GetItem(19)
    '         End Get
    '     End Property

    '     Public Function GetElement(ByVal Key As Integer) As StrucElement
    '         Return MyBase.GetItem(Key)
    '     End Function
    ' End Class
#End Region

End Namespace

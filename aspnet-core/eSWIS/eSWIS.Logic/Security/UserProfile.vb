Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace UserSecurity
    Public NotInheritable Class UserProfile
        Inherits Core.CoreControl
        Private UserProfileInfo As UserProfileInfo = New UserProfileInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal UsrprofileCont As Container.UserProfile, ByVal USRAPP As UsrApp, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim arrSQL As New ArrayList

            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If UsrprofileCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With UserProfileInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UserID) & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Status")) = 0 Then
                                        'Found but deleted
                                        blnFlag = False
                                    Else
                                        'Found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            message = "Record already exist"
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "USRPROFILE WITH (ROWLOCK)"
                                .AddField("UserName", UsrprofileCont.UserName, SQLControl.EnumDataType.dtStringN)
                                .AddField("Password", UsrprofileCont.Password, SQLControl.EnumDataType.dtString)
                                .AddField("RefID", UsrprofileCont.RefID, SQLControl.EnumDataType.dtString)
                                .AddField("RefType", UsrprofileCont.RefType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ParentID", UsrprofileCont.ParentID, SQLControl.EnumDataType.dtString)
                                .AddField("Status", UsrprofileCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Logged", UsrprofileCont.Logged, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LogStation", UsrprofileCont.LogStation, SQLControl.EnumDataType.dtString)
                                .AddField("LastLogin", UsrprofileCont.LastLogin, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastLogout", UsrprofileCont.LastLogout, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateDate", UsrprofileCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastUpdate", UsrprofileCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", UsrprofileCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("UpdateBy", UsrprofileCont.UpdateBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UserID) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("UserID", UsrprofileCont.UserID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UserID) & "'")
                                End Select
                                arrSQL.Add(strSQL)

                                If USRAPP IsNot Nothing Then 'Build  USR APP
                                    .TableName = "USRAPP WITH (ROWLOCK)"
                                    .AddField("AccessCode", USRAPP.AccessCode, SQLControl.EnumDataType.dtString)
                                    .AddField("IsInherit", USRAPP.IsInherit, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("AppID", USRAPP.AppID, SQLControl.EnumDataType.dtNumeric)

                                    Select Case pType
                                        Case SQLControl.EnumSQLType.stInsert
                                            'Add
                                            .AddField("UserID", USRAPP.UserID, SQLControl.EnumDataType.dtString)
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)

                                        Case SQLControl.EnumSQLType.stUpdate
                                            'Edit
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                           "WHERE UserID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, USRAPP.UserID) & "'")
                                    End Select

                                    arrSQL.Add(strSQL)
                                End If
                            End With
                            Try
                                'execute
                                objConn.BatchExecute(arrSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True

                            Catch axExecute As Exception
                                Dim sqlStatement As String = " "
                                If objConn.FailedSQLStatement.Count > 0 Then
                                    sqlStatement &= objConn.FailedSQLStatement.Item(0)
                                End If

                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                Else
                                    message = axExecute.Message.ToString()
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", axExecute.Message & sqlStatement, axExecute.StackTrace)
                                Return False
                            Finally
                                objSQL.Dispose()
                            End Try
                        End If

                    End If
                End If
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                UsrprofileCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function SaveChangePassword(ByVal UsrprofileCont As Container.UserProfile, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim arrSQL As New ArrayList

            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            SaveChangePassword = False
            Try
                If UsrprofileCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With UserProfileInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UserID) & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Status")) = 0 Then
                                        'Found but deleted
                                        blnFlag = False
                                    Else
                                        'Found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            message = "Record already exist"
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "USRPROFILE WITH (ROWLOCK)"
                                .AddField("UserName", UsrprofileCont.UserName, SQLControl.EnumDataType.dtStringN)
                                .AddField("Password", UsrprofileCont.Password, SQLControl.EnumDataType.dtString)
                                .AddField("RefID", UsrprofileCont.RefID, SQLControl.EnumDataType.dtString)
                                .AddField("RefType", UsrprofileCont.RefType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ParentID", UsrprofileCont.ParentID, SQLControl.EnumDataType.dtString)
                                .AddField("Status", UsrprofileCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Logged", UsrprofileCont.Logged, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LogStation", UsrprofileCont.LogStation, SQLControl.EnumDataType.dtString)
                                .AddField("LastLogin", UsrprofileCont.LastLogin, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastLogout", UsrprofileCont.LastLogout, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateDate", UsrprofileCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastUpdate", UsrprofileCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", UsrprofileCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("UpdateBy", UsrprofileCont.UpdateBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UserID) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("UserID", UsrprofileCont.UserID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UserID) & "'")
                                End Select
                                arrSQL.Add(strSQL)

                            End With
                            Try

                                objConn.BatchExecute(arrSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                Dim sqlStatement As String = " "
                                If objConn.FailedSQLStatement.Count > 0 Then
                                    sqlStatement &= objConn.FailedSQLStatement.Item(0)
                                End If

                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                Else
                                    message = axExecute.Message.ToString()
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", axExecute.Message & sqlStatement, axExecute.StackTrace)
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If

                    End If
                End If
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                UsrprofileCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal UsrprofileCont As Container.UserProfile, ByVal USRAPP As UsrApp, ByRef message As String) As Boolean
            Return Save(UsrprofileCont, USRAPP, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal UsrprofileCont As Container.UserProfile, ByVal USRAPP As UsrApp, ByRef message As String) As Boolean
            Return Save(UsrprofileCont, USRAPP, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function UpdateChangePassword(ByVal UsrprofileCont As Container.UserProfile, ByRef message As String) As Boolean
            Return SaveChangePassword(UsrprofileCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal UsrprofileCont As Container.UserProfile, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If UsrprofileCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With UserProfileInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UserID) & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True

                                End If
                                .Close()
                            End With
                        End If

                        If blnFound = True And blnInUse = True Then
                            With objSQL
                                strSQL = BuildUpdate("USRPROFILE WITH (ROWLOCK)", " SET Status = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UpdateBy) & "' WHERE" & _
                                "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UserID) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("USRPROFILE WITH (ROWLOCK)", "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrprofileCont.UserID) & "'")
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                UsrprofileCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'Add
        Public Function IsUserExist(ByVal UserID As String) As Boolean
            Dim strSQL As String

            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim dt As New DataTable
            IsUserExist = False

            Try

                If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()

                    strSQL = "SELECT UserID FROM USRPROFILE WITH (NOLOCK) WHERE USERID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UserID) & "' "
                    dt = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, "USRPROFILE"), Data.DataTable)

                    If dt.Rows.Count = 0 Then
                        IsUserExist = False
                    Else
                        IsUserExist = True
                    End If
                End If

            Catch exExecute As ApplicationException
                Log.Notifier.Notify(exExecute)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", exExecute.Message, exExecute.StackTrace)

            Catch exExecute As Exception
                Log.Notifier.Notify(exExecute)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", exExecute.Message, exExecute.StackTrace)

            Finally

                rdr = Nothing
                dt = Nothing
                EndSQLControl()
                EndConnection()
            End Try

            Return IsUserExist
        End Function

        'Add
        Public Function UpdatePassword(ByVal UserID As String, ByVal NewPassword As String, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            UpdatePassword = False
            blnFound = False
            blnInUse = False
            Try

                If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()

                    strSQL = "UPDATE USRPROFILE SET Password='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NewPassword) & "', LastUpdate=getdate(), UpdateBy='systemreset' WHERE USERID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UserID) & "'"

                    objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                End If

            Catch exExecute As ApplicationException
                message = exExecute.Message.ToString()
                Log.Notifier.Notify(exExecute)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", exExecute.Message, exExecute.StackTrace)
                Return False
            Catch exExecute As Exception
                message = exExecute.Message.ToString()
                Log.Notifier.Notify(exExecute)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", exExecute.Message, exExecute.StackTrace)
                Return False
            Finally

                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region

#Region "Data Selection"
        Public Overloads Function CheckUser(ByVal userid As String, ByVal pass As String) As DataTable
            Try
                If StartConnection() = True Then
                    With UserProfileInfo.MyInfo
                        StartSQLControl()

                        Dim filterByDeviceID As String = ""

                        strSQL = "SELECT UserID, UserName, Password FROM USRPROFILE where UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, userid) & "'" & _
                                 " AND Password = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, pass) & "'"

                        Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    End With
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", ex.Message & " " & strSQL, ex.StackTrace)
                'Throw ex
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function updtPass(ByVal userid As String, ByVal newPass As String) As String
            Dim objUserProfile As eSWIS.Logic.UserSecurity.UserProfile
            Dim strSQL As String = ""
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim dt As Data.DataTable = Nothing
            Dim ds As Data.DataSet = New Data.DataSet()

            Try
                If StartConnection() = True Then
                    strSQL = "UPDATE USRPROFILE SET PASSWORD = '" & newPass & "', Status = 2" & _
                             " WHERE USERID = '" & userid & "'"

                    rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

                    With UserProfileInfo.MyInfo
                        StartSQLControl()

                        objUserProfile = New eSWIS.Logic.UserSecurity.UserProfile("Data Source=121.121.17.73;Initial Catalog=ESWIS;Integrated Security=False;User ID=sa;Password=coll@393")
                        dt = objUserProfile.CheckUser(userid, newPass)

                        If dt Is Nothing = False Then
                            If dt.Rows.Count > 0 Then
                                Return "Password Successfully Updated"
                            Else
                                Return "No Record Found"
                            End If
                        Else
                            Return "No Record Found"
                        End If
                    End With
                Else
                    Return Nothing
                End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        'List Login Details
        Public Overloads Function GetLoginDetails(ByVal UserID As System.String, ByVal PassKey As System.String, ByVal DeviceID As System.String, ByVal DeviceType As String) As Container_UserProfile
            Dim dt As New Data.DataTable
            Dim ListLogin As List(Of Container_UserProfile) = New List(Of Container_UserProfile)

            Try
                If StartConnection() = True Then
                    With UserProfileInfo.MyInfo
                        StartSQLControl()

                        Dim filterByDeviceID As String = ""

                        If DeviceType = "2" Or DeviceType = "3" Then
                            filterByDeviceID = " AND uv.VeriKey = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, DeviceID) & "' "
                        End If

                        strSQL = "SELECT isnull(USRPROFILE.UserID, '') as UserID, isnull(USRPROFILE.UserName, '') as UserName, isnull(USRPROFILE.Password, '') as Password, " & _
                            " isnull(USRPROFILE.RefID, '') as RefID, isnull(USRPROFILE.RefType, '') as RefType, isnull(USRPROFILE.ParentID, '') as ParentID, " & _
                            " isnull(USRPROFILE.Status, '') as Status, isnull(USRAPP.AppID, '') as AppID, isnull(USRAPP.AccessCode, '') as AccessCode, (SELECT TOP 1 isnull(UP.UserName, '') FROM USRPROFILE UP WITH (NOLOCK) WHERE (UP.UserID = USRPROFILE.CreateBy)) AS CreateUser, isnull(USRPROFILE.CreateDate, '') as CreateDate, " & _
                            " isnull(g.GroupCode, '') As GroupCode, isnull(uv.Status, '') as VerifyStatus, isnull(uv.VeriKey, '') as VerifyKey, isnull(uv.Active, '') as mActive, isnull(uv.Remark, '') as mRemark  " & _
                            " FROM USRPROFILE WITH (NOLOCK) " & _
                            " INNER JOIN USRAPP WITH (NOLOCK) ON USRPROFILE.UserID = USRAPP.UserID " & _
                            " LEFT JOIN USRGROUP g WITH (NOLOCK) ON g.GroupCode = USRAPP.AccessCode AND g.AppID = USRAPP.APPID " & _
                            " LEFT JOIN CODEMASTER c WITH (NOLOCK) ON c.CodeType = 'USR' AND c.Code = USRPROFILE.Status " & _
                            " LEFT JOIN USRVERIFY uv with (nolock) on uv.Userid = usrprofile.userid " & filterByDeviceID & _
                            " WHERE USRPROFILE.status <> 0 AND USRPROFILE.UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UserID) & "' " & _
                            " AND USRPROFILE.Password = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, PassKey) & "'"

                        dt = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)

                        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                            Dim container As Container_UserProfile = New eSWIS.Logic.UserSecurity.Container_UserProfile
                            container.UserID = dt.Rows.Item(0)("UserID").ToString
                            container.UserName = dt.Rows.Item(0)("UserName").ToString
                            container.Password = dt.Rows.Item(0)("Password").ToString
                            container.RefID = dt.Rows.Item(0)("RefID").ToString
                            container.RefType = dt.Rows.Item(0)("RefType").ToString
                            container.ParentID = dt.Rows.Item(0)("ParentID").ToString
                            container.Status = dt.Rows.Item(0)("Status").ToString
                            container.AppID = dt.Rows.Item(0)("AppID").ToString
                            container.AccessCode = dt.Rows.Item(0)("AccessCode").ToString
                            container.CreateUser = dt.Rows.Item(0)("CreateUser").ToString
                            container.CreateDate = dt.Rows.Item(0)("CreateDate").ToString
                            container.GroupCode = dt.Rows.Item(0)("GroupCode").ToString
                            container.VerifyStatus = dt.Rows.Item(0)("VerifyStatus").ToString
                            container.VerifyKey = dt.Rows.Item(0)("VerifyKey").ToString
                            container.mActive = dt.Rows.Item(0)("mActive").ToString
                            container.mRemark = dt.Rows.Item(0)("mRemark").ToString

                            Return container
                        Else
                            Return Nothing
                        End If

                    End With
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", ex.Message & " " & strSQL, ex.StackTrace)
                Return Nothing
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'Check Old Password
        Public Function GetOldPassword(ByVal UserID As String) As Container.UserProfile

            Dim rUsrprofile As Container.UserProfile = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With UserProfileInfo.MyInfo
                        strSQL = "SELECT Password FROM USRPROFILE WITH (NOLOCK) WHERE UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UserID) & "'"
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rUsrprofile = New Container.UserProfile
                                rUsrprofile.Password = drRow.Item("Password")
                            Else
                                rUsrprofile = Nothing
                            End If
                        Else
                            rUsrprofile = Nothing
                        End If
                    End With
                End If
                Return rUsrprofile
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", ex.Message, ex.StackTrace)
            Finally
                rUsrprofile = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try

        End Function

        'Enquiry
        Public Function Enquiry(ByVal strSQLQuery As String) As DataTable
            Dim dtTemp As DataTable = Nothing
            Dim strCond As String = String.Empty

            Try
                If StartConnection() = True Then

                    dtTemp = objConn.Execute(strSQLQuery, CommandType.Text)

                    If dtTemp Is Nothing = False Then
                        Return dtTemp
                    Else
                        Return Nothing
                    End If
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try

        End Function

        Public Overloads Function GetUserProfile(ByVal UserID As System.String, Optional ByVal PermissionAPI As String = "01") As Container.UserProfile
            Dim rUsrprofile As Container.UserProfile = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With UserProfileInfo.MyInfo
                        strSQL = "SELECT ISNULL(bz.KKM,0) AS KKM, USRPROFILE.UserID, USRPROFILE.UserName, USRPROFILE.Password, " & _
                            " USRPROFILE.RefID, USRPROFILE.RefType, USRPROFILE.ParentID, " & _
                            " USRPROFILE.Status, USRAPP.AppID, USRAPP.AccessCode,USRAPP.PermissionAPI, (SELECT TOP 1 UP.UserName FROM USRPROFILE UP WITH (NOLOCK) WHERE (UP.UserID = USRPROFILE.CreateBy)) AS CreateUser, USRPROFILE.CreateDate, " & _
                            " g.GroupCode As GroupCode, c.CodeDesc AS STATUSDESC, bz.CompanyType " & _
                            " FROM USRPROFILE WITH (NOLOCK) " & _
                            " INNER JOIN USRAPP WITH (NOLOCK) ON USRPROFILE.UserID = USRAPP.UserID " & _
                            " LEFT JOIN USRGROUP g WITH (NOLOCK) ON g.GroupCode = USRAPP.AccessCode AND g.AppID = USRAPP.APPID " & _
                            " LEFT JOIN CODEMASTER c WITH (NOLOCK) ON c.CodeType = 'USR' AND c.Code = USRPROFILE.Status LEFT JOIN BIZENTITY bz WITH (NOLOCK) ON bz.BizRegID=USRPROFILE.ParentID " & _
                            " WHERE USRPROFILE.STATUS<>0 AND USRPROFILE.UserID= '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UserID) & "' AND USRAPP.PermissionAPI= '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, PermissionAPI) & "'"
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rUsrprofile = New Container.UserProfile
                                rUsrprofile.UserID = drRow.Item("UserID")
                                rUsrprofile.UserName = drRow.Item("UserName")
                                rUsrprofile.Password = drRow.Item("Password")
                                rUsrprofile.RefID = drRow.Item("RefID")
                                rUsrprofile.RefType = drRow.Item("RefType")
                                rUsrprofile.ParentID = drRow.Item("ParentID")
                                rUsrprofile.Status = drRow.Item("Status")
                                rUsrprofile.GroupCode = IIf(IsDBNull(drRow.Item("GroupCode")), "", drRow.Item("GroupCode"))
                                rUsrprofile.AppId = drRow.Item("AppId")
                                rUsrprofile.KKM = drRow.Item("KKM")
                                rUsrprofile.PermissionAPI = drRow.Item("PermissionAPI")
                            Else
                                rUsrprofile = Nothing
                            End If
                        Else
                            rUsrprofile = Nothing
                        End If
                    End With
                End If
                Return rUsrprofile
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", ex.Message, ex.StackTrace)
            Finally
                rUsrprofile = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetUserProfileByRefID(ByVal RefID As System.String) As Container.UserProfile
            Dim rUsrprofile As Container.UserProfile = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With UserProfileInfo.MyInfo
                        StartSQLControl()
                        strSQL = BuildSelect(.FieldsList, .TableName, "RefID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, RefID) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rUsrprofile = New Container.UserProfile
                                rUsrprofile.UserID = drRow.Item("UserID")
                                rUsrprofile.Password = drRow.Item("Password")
                            Else
                                rUsrprofile = Nothing
                            End If
                        Else
                            rUsrprofile = Nothing
                        End If
                    End With
                End If
                Return rUsrprofile
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", ex.Message, ex.StackTrace)
            Finally
                rUsrprofile = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetUserProfile(ByVal UserID As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.UserProfile)
            Dim rUsrprofile As Container.UserProfile = Nothing
            Dim lstUsrprofile As List(Of Container.UserProfile) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With UserProfileInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal UserID As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UserID) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rUsrprofile = New Container.UserProfile
                                rUsrprofile.UserID = drRow.Item("UserID")
                                rUsrprofile.UserName = drRow.Item("UserName")
                                rUsrprofile.Password = drRow.Item("Password")
                                rUsrprofile.RefID = drRow.Item("RefID")
                                rUsrprofile.RefType = drRow.Item("RefType")
                                rUsrprofile.ParentID = drRow.Item("ParentID")
                                rUsrprofile.Status = drRow.Item("Status")
                                rUsrprofile.Logged = drRow.Item("Logged")
                                rUsrprofile.LogStation = drRow.Item("LogStation")
                                rUsrprofile.rowguid = drRow.Item("rowguid")
                                rUsrprofile.CreateBy = drRow.Item("CreateBy")
                                rUsrprofile.UpdateBy = drRow.Item("UpdateBy")
                                rUsrprofile.SyncCreate = drRow.Item("SyncCreate")
                                rUsrprofile.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rUsrprofile.AppId = drRow.Item("AppId")

                            Next
                            lstUsrprofile.Add(rUsrprofile)
                        Else
                            rUsrprofile = Nothing
                        End If
                        Return lstUsrprofile
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", ex.Message, ex.StackTrace)
            Finally
                rUsrprofile = Nothing
                lstUsrprofile = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetUserProfileList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With UserProfileInfo.MyInfo
                    If SQL = Nothing Or SQL = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName, FieldCond)
                    Else
                        strSQL = SQL
                    End If
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetUserProfileListEntity(Optional ByVal Status As String = Nothing, Optional ByVal ParentID As String = Nothing) As Data.DataTable
            Dim strFilter As String = ""
            If StartConnection() = True Then
                StartSQLControl()
                With UserProfileInfo.MyInfo
                    strFilter = "Status " & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Status) &
                                " AND ParentID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ParentID) & "' "
                    strSQL = BuildSelect(.FieldsList, .TableName, strFilter)
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            End If
            EndSQLControl()
            EndConnection()
        End Function

#End Region
    End Class

    Public Class Container_UserProfile
        Private _userid As String
        Private _username As String
        Private _password As String
        Private _refid As String
        Private _reftype As String
        Private _parentid As String
        Private _status As String
        Private _appid As String
        Private _accesscode As String
        Private _createuser As String
        Private _createdate As String
        Private _groupcode As String
        Private _verifystatus As String
        Private _verifykey As String
        Private _mactive As String
        Private _mremark As String

        Public Property UserID() As String
            Get
                Return _userid
            End Get
            Set(ByVal value As String)
                _userid = value
            End Set
        End Property

        Public Property UserName() As String
            Get
                Return _username
            End Get
            Set(ByVal value As String)
                _username = value
            End Set
        End Property

        Public Property Password() As String
            Get
                Return _password
            End Get
            Set(ByVal value As String)
                _password = value
            End Set
        End Property

        Public Property RefID() As String
            Get
                Return _refid
            End Get
            Set(ByVal value As String)
                _refid = value
            End Set
        End Property

        Public Property RefType() As String
            Get
                Return _reftype
            End Get
            Set(ByVal value As String)
                _reftype = value
            End Set
        End Property

        Public Property ParentID() As String
            Get
                Return _parentid
            End Get
            Set(ByVal value As String)
                _parentid = value
            End Set
        End Property

        Public Property Status() As String
            Get
                Return _status
            End Get
            Set(ByVal value As String)
                _status = value
            End Set
        End Property

        Public Property AppID() As String
            Get
                Return _appid
            End Get
            Set(ByVal value As String)
                _appid = value
            End Set
        End Property

        Public Property AccessCode() As String
            Get
                Return _accesscode
            End Get
            Set(ByVal value As String)
                _accesscode = value
            End Set
        End Property

        Public Property CreateUser() As String
            Get
                Return _createuser
            End Get
            Set(ByVal value As String)
                _createuser = value
            End Set
        End Property

        Public Property CreateDate() As String
            Get
                Return _createdate
            End Get
            Set(ByVal value As String)
                _createdate = value
            End Set
        End Property

        Public Property GroupCode() As String
            Get
                Return _groupcode
            End Get
            Set(ByVal value As String)
                _groupcode = value
            End Set
        End Property

        Public Property VerifyStatus() As String
            Get
                Return _verifystatus
            End Get
            Set(ByVal value As String)
                _verifystatus = value
            End Set
        End Property

        Public Property VerifyKey() As String
            Get
                Return _verifykey
            End Get
            Set(ByVal value As String)
                _verifykey = value
            End Set
        End Property

        Public Property mActive() As String
            Get
                Return _mactive
            End Get
            Set(ByVal value As String)
                _mactive = value
            End Set
        End Property

        Public Property mRemark() As String
            Get
                Return _mremark
            End Get
            Set(ByVal value As String)
                _mremark = value
            End Set
        End Property
    End Class


    Namespace Container
#Region "Class Container"
        Public Class UserProfile
            Public fUserID As System.String = "UserID"
            Public fUserIDOld As System.String = "UserID"
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
            Public frowguid As System.String = "rowguid"
            Public fCreateDate As System.String = "CreateDate"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fCreateBy As System.String = "CreateBy"
            Public fUpdateBy As System.String = "UpdateBy"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fGroupCode As System.String = "GroupCode"
            Public fAppId As System.String = "AppId"


            Protected _UserID As System.String
            Protected _UserIDOld As System.String
            Private _UserName As System.String
            Private _Password As System.String
            Private _RefID As System.String
            Private _RefType As System.Byte
            Private _ParentID As System.String
            Private _Status As System.Byte
            Private _Logged As System.Byte
            Private _LogStation As System.String
            Private _LastLogin As System.DateTime
            Private _LastLogout As System.DateTime
            Private _rowguid As System.Guid
            Private _CreateDate As System.DateTime
            Private _LastUpdate As System.DateTime
            Private _CreateBy As System.String
            Private _UpdateBy As System.String
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _GroupCode As System.String
            Private _AppId As System.String
            Private _KKM As System.Byte
            Private _PermissionAPI As System.String
            Private _CompanyType As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property CompanyType As System.String
                Get
                    Return _CompanyType
                End Get
                Set(ByVal Value As System.String)
                    _CompanyType = Value
                End Set
            End Property
            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property PermissionAPI As System.String
                Get
                    Return _PermissionAPI
                End Get
                Set(ByVal Value As System.String)
                    _PermissionAPI = Value
                End Set
            End Property
            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property KKM As System.Byte
                Get
                    Return _KKM
                End Get
                Set(ByVal Value As System.Byte)
                    _KKM = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property AppId As System.String
                Get
                    Return _AppId
                End Get
                Set(ByVal Value As System.String)
                    _AppId = Value
                End Set
            End Property

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
            ''' Mandatory
            ''' </summary>
            Public Property UserIDOld As System.String
                Get
                    Return _UserIDOld
                End Get
                Set(ByVal Value As System.String)
                    _UserIDOld = Value
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
            Public Property CreateBy As System.String
                Get
                    Return _CreateBy
                End Get
                Set(ByVal Value As System.String)
                    _CreateBy = Value
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

            Public Property GroupCode As System.String
                Get
                    Return _GroupCode
                End Get
                Set(ByVal Value As System.String)
                    _GroupCode = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class UserProfileInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "UserID,UserName,Password,RefID,RefType,ParentID,Status,Logged,LogStation,LastLogin,LastLogout,rowguid,CreateDate,LastUpdate,CreateBy,UpdateBy,SyncCreate,SyncLastUpd"
                .CheckFields = "RefType,Status,Logged"
                .TableName = "USRPROFILE WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "UserID,UserName,Password,RefID,RefType,ParentID,Status,Logged,LogStation,LastLogin,LastLogout,rowguid,CreateDate,LastUpdate,CreateBy,UpdateBy,SyncCreate,SyncLastUpd"
                .ListingCond = Nothing
                .ShortList = Nothing
                .ShortListCond = Nothing
            End With
        End Sub

        Public Function JoinTableField(ByVal Prefix As String, ByVal FieldList As String) As String
            Dim aFieldList As Array
            Dim strFieldList As String = Nothing
            aFieldList = FieldList.Split(",")
            If Not aFieldList Is Nothing Then
                For Each Str As String In aFieldList
                    If strFieldList Is Nothing Then
                        strFieldList = Prefix & "." & Str
                    Else
                        strFieldList &= "," & Prefix & "." & Str
                    End If
                Next
            End If
            aFieldList = Nothing

            Return strFieldList
        End Function
    End Class
#End Region

#Region "Scheme"
    Public Class UserProfileScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UserID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "UserName"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Password"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "RefID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "RefType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ParentID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Logged"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LogStation"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastLogin"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastLogout"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)

        End Sub

        Public ReadOnly Property UserID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property UserName As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property Password As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property RefID As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property RefType As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property ParentID As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property Logged As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property LogStation As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property LastLogin As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property LastLogout As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

#Region "Property Object - USRAPP"
    Public Class UsrApp

#Region "Private Members"
        Private _userID As System.String
        Private _appID As System.Int32
        Private _accessCode As System.String
        Private _isInherit As System.Byte
        Private _rowguid As System.Guid
        Private _syncCreate As System.DateTime
        Private _syncLastUpd As System.DateTime
        'Private _customerID As System.String
#End Region

#Region "Constructors"
        ' initialization
        Public Sub New()
        End Sub

        Public Sub New(ByVal userID As System.String, ByVal appID As System.Int32, ByVal accessCode As System.String, ByVal isInherit As System.Byte, ByVal rowguid As System.Guid, ByVal syncCreate As System.DateTime, ByVal syncLastUpd As System.DateTime, ByVal customerID As String)
            Me.UserID = userID
            Me.AppID = appID
            Me.AccessCode = accessCode
            Me.IsInherit = isInherit
            Me.Rowguid = rowguid
            Me.SyncCreate = syncCreate
            Me.SyncLastUpd = syncLastUpd
            ' Me.CustomerID = customerID
        End Sub
#End Region

#Region "Public Properties"
        Public Property UserID() As System.String
            Get
                Return _userID
            End Get
            Set(ByVal value As System.String)
                _userID = value
            End Set
        End Property
        Public Property AppID() As System.Int32
            Get
                Return _appID
            End Get
            Set(ByVal value As System.Int32)
                _appID = value
            End Set
        End Property
        Public Property AccessCode() As System.String
            Get
                Return _accessCode
            End Get
            Set(ByVal value As System.String)
                _accessCode = value
            End Set
        End Property
        Public Property IsInherit() As System.Byte
            Get
                Return _isInherit
            End Get
            Set(ByVal value As System.Byte)
                _isInherit = value
            End Set
        End Property
        Public Property Rowguid() As System.Guid
            Get
                Return _rowguid
            End Get
            Set(ByVal value As System.Guid)
                _rowguid = value
            End Set
        End Property
        Public Property SyncCreate() As System.DateTime
            Get
                Return _syncCreate
            End Get
            Set(ByVal value As System.DateTime)
                _syncCreate = value
            End Set
        End Property
        Public Property SyncLastUpd() As System.DateTime
            Get
                Return _syncLastUpd
            End Get
            Set(ByVal value As System.DateTime)
                _syncLastUpd = value
            End Set
        End Property
        'Public Property CustomerID() As System.String
        '    Get
        '        Return _customerID
        '    End Get
        '    Set(ByVal value As System.String)
        '        _customerID = value
        '    End Set
        'End Property
#End Region


    End Class
#End Region


End Namespace
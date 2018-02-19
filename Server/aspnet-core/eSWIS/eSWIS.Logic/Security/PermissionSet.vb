'Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General
Imports System.Data.SqlClient

Namespace UserSecurity
    Public NotInheritable Class PermissionSet
        Inherits Core2.CoreControl
        'Private PermissionsetInfo As PermissionsetInfo = New PermissionsetInfo
        'Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        'Private Function Save(ByVal PermissionsetCont As Container.PermissionSet, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
        '    Dim strSQL As String = ""
        '    Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
        '    Dim rdr As System.Data.SqlClient.SqlDataReader
        '    Save = False
        '    Try
        '        If PermissionsetCont Is Nothing Then
        '            'Message return
        '        Else
        '            blnExec = False
        '            blnFound = False
        '            blnFlag = False
        '            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
        '                StartSQLControl()
        '                With PermissionsetInfo.MyInfo
        '                    strSQL = BuildSelect(.CheckFields, .TableName, "AccessCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, PermissionsetCont.AccessCode) & "' AND ModuleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, PermissionsetCont.ModuleID) & "' AND FunctionID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, PermissionsetCont.FunctionID) & "'")
        '                End With
        '                rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
        '                blnExec = True

        '                If rdr Is Nothing = False Then
        '                    With rdr
        '                        If .Read Then
        '                            blnFound = True
        '                            blnFlag = True

        '                        End If
        '                        .Close()
        '                    End With
        '                End If
        '            End If

        '            If blnExec Then
        '                If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
        '                    message = "Record already exist"
        '                    Return False
        '                Else
        '                    StartSQLControl()
        '                    With objSQL
        '                        .TableName = "PERMISSIONSET WITH (ROWLOCK)"
        '                        .AddField("AllowNew", PermissionsetCont.AllowNew, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("AllowEdit", PermissionsetCont.AllowEdit, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("AllowDel", PermissionsetCont.AllowDel, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("AllowPrt", PermissionsetCont.AllowPrt, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("AllowPro", PermissionsetCont.AllowPro, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("IsDeny", PermissionsetCont.IsDeny, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("AccessLevel", PermissionsetCont.AccessLevel, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("Status", PermissionsetCont.Status, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("Active", PermissionsetCont.Active, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("IsHost", PermissionsetCont.IsHost, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("Flag", PermissionsetCont.Flag, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("APPID", PermissionsetCont.AppIdUpdate, SQLControl.EnumDataType.dtNumeric)


        '                        Select Case pType
        '                            Case SQLControl.EnumSQLType.stInsert
        '                                If blnFound = True And blnFlag = False Then
        '                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "AccessCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, PermissionsetCont.AccessCode) & "' AND ModuleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, PermissionsetCont.ModuleID) & "' AND FunctionID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, PermissionsetCont.FunctionID) & "'")
        '                                Else
        '                                    If blnFound = False Then
        '                                        .AddField("AccessCode", PermissionsetCont.AccessCode, SQLControl.EnumDataType.dtString)
        '                                        .AddField("ModuleID", PermissionsetCont.ModuleID, SQLControl.EnumDataType.dtNumeric)
        '                                        .AddField("FunctionID", PermissionsetCont.FunctionID, SQLControl.EnumDataType.dtNumeric)
        '                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
        '                                    End If
        '                                End If
        '                            Case SQLControl.EnumSQLType.stUpdate
        '                                .AddField("AccessCode", PermissionsetCont.AccessCodeUpdate, SQLControl.EnumDataType.dtString)
        '                                .AddField("FunctionID", PermissionsetCont.FunctionIDUpdate, SQLControl.EnumDataType.dtNumeric)
        '                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "AccessCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, PermissionsetCont.AccessCode) & "' AND ModuleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, PermissionsetCont.ModuleID) & "' AND FunctionID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, PermissionsetCont.FunctionID) & "'")
        '                        End Select
        '                    End With
        '                    Try
        '                        objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
        '                        Return True

        '                    Catch axExecute As Exception
        '                        If pType = SQLControl.EnumSQLType.stInsert Then
        '                            message = axExecute.Message.ToString()
        '                        Else
        '                            message = axExecute.Message.ToString()
        '                        End If
        '                        Log.Notifier.Notify(axExecute)
        '                        Gibraltar.Agent.Log.Error("eSWISLogic/Security/PermissionSet", axExecute.Message & strSQL, axExecute.StackTrace)
        '                        Return False

        '                    Finally
        '                        objSQL.Dispose()
        '                    End Try
        '                End If

        '            End If
        '        End If
        '    Catch axAssign As ApplicationException
        '        message = axAssign.Message.ToString()
        '        Log.Notifier.Notify(axAssign)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Security/PermissionSet", axAssign.Message, axAssign.StackTrace)
        '        Return False
        '    Catch exAssign As SystemException
        '        message = exAssign.Message.ToString()
        '        Log.Notifier.Notify(exAssign)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Security/PermissionSet", exAssign.Message, exAssign.StackTrace)
        '        Return False
        '    Finally
        '        PermissionsetCont = Nothing
        '        rdr = Nothing
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function

        ''ADD
        'Public Function Insert(ByVal PermissionsetCont As Container.PermissionSet, ByRef message As String) As Boolean
        '    Return Save(PermissionsetCont, SQLControl.EnumSQLType.stInsert, message)
        'End Function

        ''AMEND
        'Public Function Update(ByVal PermissionsetCont As Container.PermissionSet, ByRef message As String) As Boolean
        '    Return Save(PermissionsetCont, SQLControl.EnumSQLType.stUpdate, message)
        'End Function

        'Public Function Delete(ByVal PermissionsetCont As Container.PermissionSet, ByRef message As String) As Boolean
        '    Dim strSQL As String
        '    Dim blnFound As Boolean
        '    Dim blnInUse As Boolean
        '    Dim rdr As System.Data.SqlClient.SqlDataReader
        '    Delete = False
        '    blnFound = False
        '    blnInUse = False
        '    Try
        '        If PermissionsetCont Is Nothing Then
        '            'Error Message
        '        Else
        '            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
        '                StartSQLControl()
        '                With PermissionsetInfo.MyInfo
        '                    strSQL = BuildSelect(.CheckFields, .TableName, "APPID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, PermissionsetCont.APPID) & "' AND AccessCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, PermissionsetCont.AccessCode) & "' AND ModuleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, PermissionsetCont.ModuleID) & "' AND FunctionID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, PermissionsetCont.FunctionID) & "'")
        '                End With
        '                rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

        '                If rdr Is Nothing = False Then
        '                    With rdr
        '                        If .Read Then
        '                            blnFound = True
        '                        End If
        '                        .Close()
        '                    End With
        '                End If

        '                If blnFound = True And blnInUse = False Then
        '                    strSQL = BuildDelete("PERMISSIONSET WITH (ROWLOCK)", "APPID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, PermissionsetCont.APPID) & "' AND AccessCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, PermissionsetCont.AccessCode) & "' AND ModuleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, PermissionsetCont.ModuleID) & "' AND FunctionID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, PermissionsetCont.FunctionID) & "'")
        '                End If

        '                Try
        '                    objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
        '                    Return True
        '                Catch exExecute As Exception
        '                    message = exExecute.Message.ToString()
        '                    Log.Notifier.Notify(exExecute)
        '                    Gibraltar.Agent.Log.Error("eSWISLogic/Security/PermissionSet", exExecute.Message & strSQL, exExecute.StackTrace)
        '                    Return False
        '                End Try
        '            End If
        '        End If

        '    Catch axDelete As ApplicationException
        '        message = axDelete.Message.ToString()
        '        Log.Notifier.Notify(axDelete)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Security/PermissionSet", axDelete.Message, axDelete.StackTrace)
        '        Return False
        '    Catch exDelete As Exception
        '        message = exDelete.Message.ToString()
        '        Log.Notifier.Notify(exDelete)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Security/PermissionSet", exDelete.Message, exDelete.StackTrace)
        '        Return False
        '    Finally
        '        PermissionsetCont = Nothing
        '        rdr = Nothing
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function
        Public Function Update(ByVal containers As IList(Of Container.PermissionSet)) As Boolean
            'Return Save(UsrverifyCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
            Try
                Dim strSQL = ""
                For Each r As Container.PermissionSet In containers
                    strSQL = strSQL & String.Format("Update PERMISSIONSET SET " &
                                                        "AllowNew = {0}, AllowEdit = {1}, AllowDel = {2}, AllowPrt = {3}, AllowPro = {4}, IsDeny = {5} " &
                                                    "WHERE AccessCode = {6} AND ModuleID = {7} AND FunctionID = {8} AND AppID = {9};",
                                                    r.AllowNew, r.AllowEdit, r.AllowDel, r.AllowPrt, r.AllowPro, r.IsDeny,
                                                    r.AccessCode, r.ModuleID, r.FunctionID, r.APPID)
                Next
                If StartConnection() = True Then
                    'With UsrverifyInfo.MyInfo
                    StartSQLControl()

                    Dim cmd = New SqlCommand(strSQL, Conn)
                    Dim affected = cmd.ExecuteNonQuery()
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                'Log.Notifier.Notify(ex)
                'Gibraltar.Agent.Log.Error("UserSecurity/UsrVerify", ex.Message, ex.StackTrace)
                Return False
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        'Public Overloads Function GetPermission(ByVal APPID As System.Int32, ByVal AccessCode As System.String, ByVal ModuleID As System.Int32, ByVal FunctionID As System.Int32) As Container.PermissionSet
        '    Dim rPermissionset As Container.PermissionSet = Nothing
        '    Dim dtTemp As DataTable = Nothing
        '    Dim lstField As New List(Of String)
        '    Dim strSQL As String = Nothing

        '    Try
        '        If StartConnection() = True Then
        '            StartSQLControl()
        '            With PermissionsetInfo.MyInfo
        '                strSQL = BuildSelect(.FieldsList, .TableName, "APPID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, APPID) & "' AND AccessCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, AccessCode) & "' AND ModuleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, ModuleID) & "' AND FunctionID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, FunctionID) & "'")
        '                dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '                Dim rowCount As Integer = 0
        '                If dtTemp Is Nothing = False Then
        '                    If dtTemp.Rows.Count > 0 Then
        '                        Dim drRow = dtTemp.Rows(0)
        '                        rPermissionset = New Container.PermissionSet
        '                        rPermissionset.APPID = drRow.Item("APPID")
        '                        rPermissionset.AccessCode = drRow.Item("AccessCode")
        '                        rPermissionset.ModuleID = drRow.Item("ModuleID")
        '                        rPermissionset.FunctionID = drRow.Item("FunctionID")
        '                        rPermissionset.AllowNew = drRow.Item("AllowNew")
        '                        rPermissionset.AllowEdit = drRow.Item("AllowEdit")
        '                        rPermissionset.AllowDel = drRow.Item("AllowDel")
        '                        rPermissionset.AllowPrt = drRow.Item("AllowPrt")
        '                        rPermissionset.AllowPro = drRow.Item("AllowPro")
        '                        rPermissionset.IsDeny = drRow.Item("IsDeny")
        '                        rPermissionset.AccessLevel = drRow.Item("AccessLevel")
        '                        rPermissionset.rowguid = drRow.Item("rowguid")
        '                        rPermissionset.SyncCreate = drRow.Item("SyncCreate")
        '                        rPermissionset.SyncLastUpd = drRow.Item("SyncLastUpd")
        '                        rPermissionset.IsHost = drRow.Item("IsHost")
        '                    Else
        '                        rPermissionset = Nothing
        '                    End If
        '                Else
        '                    rPermissionset = Nothing
        '                End If
        '            End With
        '        End If
        '        Return rPermissionset
        '    Catch ex As Exception
        '        Log.Notifier.Notify(ex)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Security/PermissionSet", ex.Message, ex.StackTrace)
        '    Finally
        '        rPermissionset = Nothing
        '        dtTemp = Nothing
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function

        Public Overloads Function GetPermissionBy(ByVal APPID As System.Int32, ByVal UserID As System.String, ByVal AccessCode As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.PermissionSet)
            Dim rPermissionset As Container.PermissionSet = Nothing
            Dim lstPermissionset As List(Of Container.PermissionSet) = New List(Of Container.PermissionSet)
            Dim dtTemp As DataTable = New DataTable()
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    'With PermissionsetInfo.MyInfo
                    'If DecendingOrder Then
                    '    strDesc = " Order by ByVal APPID As System.Int32, ByVal AccessCode As System.String, ByVal ModuleID As System.Int32, ByVal FunctionID As System.Int32 DESC"
                    'End If
                    strSQL = String.Format("SELECT " &
                                            "    ( " &
                                            "        CAST ( " &
                                            "            PS.APPID AS VARCHAR " &
                                            "        ) + ';' + CAST ( " &
                                            "            PS.AccessCode AS VARCHAR " &
                                            "        ) + ';' + CAST ( " &
                                            "            PS.ModuleID AS VARCHAR " &
                                            "        ) + ';' + CAST ( " &
                                            "            PS.FunctionID AS VARCHAR " &
                                            "        ) " &
                                            "    ) AS CompositeKey, " &
                                            "    PS.APPID, " &
                                            "    PS.AccessCode, " &
                                            "    UG.GroupName AS GroupName, " &
                                            "    PS.ModuleID, " &
                                            "    SM.ModuleName AS ModuleName, " &
                                            "    PS.FunctionID, " &
                                            "    SF.FunctionName AS FunctionName, " &
                                            "    PS.AllowNew, " &
                                            "    PS.AllowEdit, " &
                                            "    PS.AllowDel, " &
                                            "    PS.AllowPrt, " &
                                            "    PS.AllowPro, " &
                                            "    PS.IsDeny, " &
                                            "    PS.AccessLevel, " &
                                            "    PS.rowguid, " &
                                            "    PS.SyncCreate, " &
                                            "    PS.SyncLastUpd, " &
                                            "    PS.IsHost " &
                                            "FROM " &
                                            "    PERMISSIONSET PS " &
                                            "INNER JOIN UsrGroup UG ON UG.GroupCode = PS.AccessCode AND UG.AppID = PS.AppID " &
                                            "INNER JOIN SYSMODULE SM ON SM.ModuleID = PS.ModuleID " & 'AND SM.AppID = PS.AppID 
                                            "INNER JOIN SYSFUNCTION SF ON SF.FunctionID = PS.FunctionID AND UG.AppID = PS.AppID " &
                                            "{0} " &
                                            " " &
                                            "   {1} ORDER BY SM.ModuleName, SF.FunctionName",
                                           If(Not String.IsNullOrEmpty(UserID), "INNER JOIN USRAPP UA ON UA.AccessCode = PS.AccessCode AND UA.AppID = PS.AppID", ""),
                                           If(Not String.IsNullOrEmpty(UserID), "WHERE UA.UserID = '" & UserID & "' AND PS.IsDeny != 1", If(Not String.IsNullOrEmpty(AccessCode), "WHERE PS.AccessCode = '" & AccessCode & "'", "")))

                    'If Not String.IsNullOrEmpty(UserID) Then
                    '    strSQL = String.Format("{0} AND UA.UserID = '{1}'", strSQL, UserID)
                    'End If
                    'If Not String.IsNullOrEmpty(AccessCode) Then
                    '    strSQL = String.Format("{0} AND UA.AccessCode = '{1}'", strSQL, AccessCode)
                    'End If

                    'strSQL = BuildSelect(.FieldsList, .TableName, "APPID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, APPID) & "' AND AccessCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, AccessCode) & "' AND ModuleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, ModuleID) & "' AND FunctionID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, FunctionId) & "'" & strDesc)
                    'dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    Dim adapter As SqlDataAdapter = New SqlDataAdapter(strSQL, Conn)

                    'dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
                    adapter.Fill(dtTemp)
                    Dim rowCount As Integer = 0
                    If dtTemp Is Nothing = False Then
                        For Each drRow As DataRow In dtTemp.Rows
                            rPermissionset = New Container.PermissionSet
                            rPermissionset.APPID = drRow.Item("APPID")
                            rPermissionset.AccessCode = drRow.Item("AccessCode")
                            rPermissionset.ModuleID = drRow.Item("ModuleID")
                            rPermissionset.FunctionID = drRow.Item("FunctionID")
                            rPermissionset.ModuleName = drRow.Item("ModuleName")
                            rPermissionset.FunctionName = drRow.Item("FunctionName")
                            rPermissionset.AllowNew = drRow.Item("AllowNew")
                            rPermissionset.AllowEdit = drRow.Item("AllowEdit")
                            rPermissionset.AllowDel = drRow.Item("AllowDel")
                            rPermissionset.AllowPrt = drRow.Item("AllowPrt")
                            rPermissionset.AllowPro = drRow.Item("AllowPro")
                            rPermissionset.IsDeny = drRow.Item("IsDeny")
                            rPermissionset.AccessLevel = drRow.Item("AccessLevel")
                            rPermissionset.rowguid = drRow.Item("rowguid")
                            rPermissionset.SyncCreate = drRow.Item("SyncCreate")
                            rPermissionset.SyncLastUpd = drRow.Item("SyncLastUpd")
                            rPermissionset.IsHost = drRow.Item("IsHost")
                            lstPermissionset.Add(rPermissionset)
                        Next
                    Else
                        rPermissionset = Nothing
                    End If
                    Return lstPermissionset
                    'End With
                End If
            Catch ex As Exception
                'Log.Notifier.Notify(ex)
                'Gibraltar.Agent.Log.Error("eSWISLogic/Security/PermissionSet", ex.Message, ex.StackTrace)
            Finally
                rPermissionset = Nothing
                lstPermissionset = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'Public Overloads Function GetPermissionList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
        '    If StartConnection() = True Then
        '        With PermissionsetInfo.MyInfo
        '            If SQL = Nothing Or SQL = String.Empty Then
        '                strSQL = BuildSelect(.FieldsList, .TableName, FieldCond)
        '            Else
        '                strSQL = SQL
        '            End If
        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        'End Function

        'Public Overloads Function GetFunctionList(ByVal APPID As System.Int32) As DataTable
        '    Dim dtTemp As DataTable = Nothing
        '    Dim strSQL As String = Nothing

        '    Try
        '        If StartConnection() = True Then
        '            StartSQLControl()
        '            With PermissionsetInfo.MyInfo
        '                strSQL = "SELECT FunctionID, ISNULL((SELECT TOP 1 ModuleID FROM SYSMODULESET WITH (NOLOCK) WHERE FunctionID=SYSFUNCTION.FunctionID AND AppID=SYSFUNCTION.AppID),'') AS ModuleID FROM SYSFUNCTION WITH (NOLOCK) WHERE AppID=" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, APPID) & ""
        '                dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, "SYSFUNCTION"), Data.DataTable)
        '                Return dtTemp
        '            End With
        '        End If
        '        Return dtTemp
        '    Catch ex As Exception
        '        Log.Notifier.Notify(ex)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Security/PermissionSet", ex.Message, ex.StackTrace)
        '    Finally
        '        dtTemp = Nothing
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function

        ''Enquiry
        'Public Function Enquiry(ByVal strSQLQuery As String) As DataTable
        '    Dim dtTemp As DataTable = Nothing
        '    Dim strCond As String = String.Empty

        '    Try
        '        If StartConnection() = True Then

        '            dtTemp = objConn.Execute(strSQLQuery, CommandType.Text)

        '            If dtTemp Is Nothing = False Then
        '                Return dtTemp
        '            Else
        '                Return Nothing
        '            End If
        '        Else
        '            Return Nothing
        '        End If
        '    Catch ex As Exception
        '        Log.Notifier.Notify(ex)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Security/PermissionSet", ex.Message, ex.StackTrace)
        '    Finally
        '        dtTemp = Nothing
        '        EndSQLControl()
        '        EndConnection()
        '    End Try

        'End Function
#End Region
    End Class


    Namespace Container
#Region "Class Container"
        Public Class PermissionSet
            Public fCompositeKey As System.String = "CompositeKey"
            Public fAPPID As System.String = "APPID"
            Public fAccessCode As System.String = "AccessCode"
            Public fGroupName As System.String = "GroupName"
            Public fModuleID As System.String = "ModuleID"
            Public fModuleName As System.String = "ModuleName"
            Public fFunctionID As System.String = "FunctionID"
            Public fFunctionName As System.String = "FunctionName"
            Public fAllowNew As System.String = "AllowNew"
            Public fAllowEdit As System.String = "AllowEdit"
            Public fAllowDel As System.String = "AllowDel"
            Public fAllowPrt As System.String = "AllowPrt"
            Public fAllowPro As System.String = "AllowPro"
            Public fIsDeny As System.String = "IsDeny"
            Public fAccessLevel As System.String = "AccessLevel"
            Public frowguid As System.String = "rowguid"
            Public fFlag As System.String = "Flag"
            Public fStatus As System.String = "Status"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fLastSyncBy As System.String = "LastSyncBy"
            Public fActive As System.String = "Active"
            Public fIsHost As System.String = "IsHost"
            Public fFunctionIDUpdate As System.String = "FunctionID"

            Protected _CompositeKey As System.String
            Protected _APPID As System.Int32
            Protected _AccessCode As System.String
            Protected _GroupName As System.String
            Protected _ModuleID As System.Int32
            Protected _ModuleName As System.String
            Protected _FunctionID As System.Int32
            Protected _FunctionName As System.String
            Private _AllowNew As System.Byte
            Private _AllowEdit As System.Byte
            Private _AllowDel As System.Byte
            Private _AllowPrt As System.Byte
            Private _AllowPro As System.Byte
            Private _IsDeny As System.Byte
            Private _AccessLevel As System.Byte
            Private _rowguid As System.Guid
            Private _Status As System.Byte
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _LastSyncBy As System.String
            Private _Active As System.Byte
            Private _IsHost As System.Byte
            Private _Flag As System.Byte
            Protected _FunctionIDUpdate As System.Int32
            Protected _AccessCodeupdate As System.String
            Protected _AppIdUpdate As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property CompositeKey As System.String
                Get
                    Return _CompositeKey
                End Get
                Set(ByVal Value As System.String)
                    _CompositeKey = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property APPID As System.Int32
                Get
                    Return _APPID
                End Get
                Set(ByVal Value As System.Int32)
                    _APPID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property AccessCode As System.String
                Get
                    Return _AccessCode
                End Get
                Set(ByVal Value As System.String)
                    _AccessCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property GroupName As System.String
                Get
                    Return _GroupName
                End Get
                Set(ByVal Value As System.String)
                    _GroupName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ModuleID As System.Int32
                Get
                    Return _ModuleID
                End Get
                Set(ByVal Value As System.Int32)
                    _ModuleID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ModuleName As System.String
                Get
                    Return _ModuleName
                End Get
                Set(ByVal Value As System.String)
                    _ModuleName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property FunctionID As System.Int32
                Get
                    Return _FunctionID
                End Get
                Set(ByVal Value As System.Int32)
                    _FunctionID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property FunctionIDUpdate As System.Int32
                Get
                    Return _FunctionIDUpdate
                End Get
                Set(ByVal Value As System.Int32)
                    _FunctionIDUpdate = Value
                End Set
            End Property

            Public Property AppIdUpdate As System.String
                Get
                    Return _AppIdUpdate
                End Get
                Set(ByVal Value As System.String)
                    _AppIdUpdate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property AccessCodeUpdate As System.String
                Get
                    Return _AccessCodeupdate
                End Get
                Set(ByVal Value As System.String)
                    _AccessCodeupdate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property FunctionName As System.String
                Get
                    Return _FunctionName
                End Get
                Set(ByVal Value As System.String)
                    _FunctionName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property AllowNew As System.Byte
                Get
                    Return _AllowNew
                End Get
                Set(ByVal Value As System.Byte)
                    _AllowNew = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property AllowEdit As System.Byte
                Get
                    Return _AllowEdit
                End Get
                Set(ByVal Value As System.Byte)
                    _AllowEdit = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property AllowDel As System.Byte
                Get
                    Return _AllowDel
                End Get
                Set(ByVal Value As System.Byte)
                    _AllowDel = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property AllowPrt As System.Byte
                Get
                    Return _AllowPrt
                End Get
                Set(ByVal Value As System.Byte)
                    _AllowPrt = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property AllowPro As System.Byte
                Get
                    Return _AllowPro
                End Get
                Set(ByVal Value As System.Byte)
                    _AllowPro = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsDeny As System.Byte
                Get
                    Return _IsDeny
                End Get
                Set(ByVal Value As System.Byte)
                    _IsDeny = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property AccessLevel As System.Byte
                Get
                    Return _AccessLevel
                End Get
                Set(ByVal Value As System.Byte)
                    _AccessLevel = Value
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
            Public Property Active As System.Byte
                Get
                    Return _Active
                End Get
                Set(ByVal Value As System.Byte)
                    _Active = Value
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
            Public Property Flag As System.Byte
                Get
                    Return _Flag
                End Get
                Set(ByVal Value As System.Byte)
                    _Flag = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    'Public Class PermissionsetInfo
    '    Inherits Core.CoreBase
    '    Protected Overrides Sub InitializeClassInfo()
    '        With MyInfo
    '            .FieldsList = "(CAST(PERMISSIONSET.APPID AS VARCHAR) + ';' + CAST(PERMISSIONSET.AccessCode AS VARCHAR) + ';' + CAST(PERMISSIONSET.ModuleID AS VARCHAR) + ';' + CAST(PERMISSIONSET.FunctionID AS VARCHAR)) as CompositeKey,APPID,AccessCode,(SELECT GroupName FROM UsrGroup g WITH (NOLOCK) WHERE g.GroupCode = PermissionSet.AccessCode AND g.AppID = PermissionSet.APPID) AS GroupName,ModuleID,(SELECT ModuleName FROM SYSMODULE WITH (NOLOCK) WHERE ModuleID=PermissionSet.ModuleID) AS ModuleName,FunctionID,(SELECT FunctionName FROM SYSFUNCTION WITH (NOLOCK) WHERE FunctionID=PermissionSet.FunctionID and APPID =PERMISSIONSET.APPID) AS FunctionName,AllowNew,AllowEdit,AllowDel,AllowPrt,AllowPro,IsDeny,AccessLevel,rowguid,SyncCreate,SyncLastUpd,IsHost"
    '            .CheckFields = "AllowNew,AllowEdit,AllowDel,AllowPrt,AllowPro,IsDeny,AccessLevel,IsHost"
    '            .TableName = "PERMISSIONSET WITH (NOLOCK)"
    '            .DefaultCond = Nothing
    '            .DefaultOrder = Nothing
    '            .Listing = "APPID,AccessCode,GroupName,ModuleName,FunctionName,ModuleID,FunctionID,AllowNew,AllowEdit,AllowDel,AllowPrt,AllowPro,IsDeny,AccessLevel,rowguid,SyncCreate,SyncLastUpd,IsHost"
    '            .ListingCond = Nothing
    '            .ShortList = Nothing
    '            .ShortListCond = Nothing
    '        End With
    '    End Sub

    '    Public Function JoinTableField(ByVal Prefix As String, ByVal FieldList As String) As String
    '        Dim aFieldList As Array
    '        Dim strFieldList As String = Nothing
    '        aFieldList = FieldList.Split(",")
    '        If Not aFieldList Is Nothing Then
    '            For Each Str As String In aFieldList
    '                If strFieldList Is Nothing Then
    '                    strFieldList = Prefix & "." & Str
    '                Else
    '                    strFieldList &= "," & Prefix & "." & Str
    '                End If
    '            Next
    '        End If
    '        aFieldList = Nothing

    '        Return strFieldList
    '    End Function
    'End Class
#End Region

#Region "Scheme"
    'Public Class PermissionSetScheme
    '    Inherits Core.SchemeBase
    '    Protected Overrides Sub InitializeInfo()

    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "APPID"
    '            .Length = 4
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(0, this)

    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "AccessCode"
    '            .Length = 20
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(1, this)

    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "ModuleID"
    '            .Length = 4
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(2, this)

    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "FunctionID"
    '            .Length = 4
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(3, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "AllowNew"
    '            .Length = 1
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(4, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "AllowEdit"
    '            .Length = 1
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(5, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "AllowDel"
    '            .Length = 1
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(6, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "AllowPrt"
    '            .Length = 1
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(7, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "AllowPro"
    '            .Length = 1
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(8, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "IsDeny"
    '            .Length = 1
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(9, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "AccessLevel"
    '            .Length = 1
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(10, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "rowguid"
    '            .Length = 16
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(11, this)
    '        'With this
    '        '    .DataType = SQLControl.EnumDataType.dtNumeric
    '        '    .FieldName = "Flag"
    '        '    .Length = 1
    '        '    .DecPlace = Nothing
    '        '    .RegExp = String.Empty
    '        '    .IsMandatory = True
    '        '    .AllowNegative = False
    '        'End With
    '        'MyBase.AddItem(12, this)
    '        'With this
    '        '    .DataType = SQLControl.EnumDataType.dtNumeric
    '        '    .FieldName = "Status"
    '        '    .Length = 1
    '        '    .DecPlace = Nothing
    '        '    .RegExp = String.Empty
    '        '    .IsMandatory = True
    '        '    .AllowNegative = False
    '        'End With
    '        'MyBase.AddItem(13, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtDateTime
    '            .FieldName = "SyncCreate"
    '            .Length = 8
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(12, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtDateTime
    '            .FieldName = "SyncLastUpd"
    '            .Length = 8
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(13, this)
    '        'With this
    '        '    .DataType = SQLControl.EnumDataType.dtString
    '        '    .FieldName = "LastSyncBy"
    '        '    .Length = 10
    '        '    .DecPlace = Nothing
    '        '    .RegExp = String.Empty
    '        '    .IsMandatory = True
    '        '    .AllowNegative = False
    '        'End With
    '        'MyBase.AddItem(14, this)
    '        'With this
    '        '    .DataType = SQLControl.EnumDataType.dtNumeric
    '        '    .FieldName = "Active"
    '        '    .Length = 1
    '        '    .DecPlace = Nothing
    '        '    .RegExp = String.Empty
    '        '    .IsMandatory = True
    '        '    .AllowNegative = False
    '        'End With
    '        'MyBase.AddItem(17, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "IsHost"
    '            .Length = 1
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(14, this)

    '    End Sub

    '    Public ReadOnly Property APPID As StrucElement
    '        Get
    '            Return MyBase.GetItem(0)
    '        End Get
    '    End Property
    '    Public ReadOnly Property AccessCode As StrucElement
    '        Get
    '            Return MyBase.GetItem(1)
    '        End Get
    '    End Property
    '    Public ReadOnly Property ModuleID As StrucElement
    '        Get
    '            Return MyBase.GetItem(2)
    '        End Get
    '    End Property
    '    Public ReadOnly Property FunctionID As StrucElement
    '        Get
    '            Return MyBase.GetItem(3)
    '        End Get
    '    End Property

    '    Public ReadOnly Property AllowNew As StrucElement
    '        Get
    '            Return MyBase.GetItem(4)
    '        End Get
    '    End Property
    '    Public ReadOnly Property AllowEdit As StrucElement
    '        Get
    '            Return MyBase.GetItem(5)
    '        End Get
    '    End Property
    '    Public ReadOnly Property AllowDel As StrucElement
    '        Get
    '            Return MyBase.GetItem(6)
    '        End Get
    '    End Property
    '    Public ReadOnly Property AllowPrt As StrucElement
    '        Get
    '            Return MyBase.GetItem(7)
    '        End Get
    '    End Property
    '    Public ReadOnly Property AllowPro As StrucElement
    '        Get
    '            Return MyBase.GetItem(8)
    '        End Get
    '    End Property
    '    Public ReadOnly Property IsDeny As StrucElement
    '        Get
    '            Return MyBase.GetItem(9)
    '        End Get
    '    End Property
    '    Public ReadOnly Property AccessLevel As StrucElement
    '        Get
    '            Return MyBase.GetItem(10)
    '        End Get
    '    End Property
    '    Public ReadOnly Property rowguid As StrucElement
    '        Get
    '            Return MyBase.GetItem(11)
    '        End Get
    '    End Property
    '    'Public ReadOnly Property Flag As StrucElement
    '    '    Get
    '    '        Return MyBase.GetItem(12)
    '    '    End Get
    '    'End Property
    '    'Public ReadOnly Property Status As StrucElement
    '    '    Get
    '    '        Return MyBase.GetItem(13)
    '    '    End Get
    '    'End Property
    '    Public ReadOnly Property SyncCreate As StrucElement
    '        Get
    '            Return MyBase.GetItem(12)
    '        End Get
    '    End Property
    '    Public ReadOnly Property SyncLastUpd As StrucElement
    '        Get
    '            Return MyBase.GetItem(13)
    '        End Get
    '    End Property
    '    'Public ReadOnly Property LastSyncBy As StrucElement
    '    '    Get
    '    '        Return MyBase.GetItem(14)
    '    '    End Get
    '    'End Property
    '    'Public ReadOnly Property Active As StrucElement
    '    '    Get
    '    '        Return MyBase.GetItem(17)
    '    '    End Get
    '    'End Property
    '    Public ReadOnly Property IsHost As StrucElement
    '        Get
    '            Return MyBase.GetItem(14)
    '        End Get
    '    End Property

    '    Public Function GetElement(ByVal Key As Integer) As StrucElement
    '        Return MyBase.GetItem(Key)
    '    End Function
    'End Class
#End Region

End Namespace
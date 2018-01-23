Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace CodeMaster

    Public NotInheritable Class WasteType
        Inherits Core.CoreControl
        Private WasteTypeInfo As WasteTypeInfo = New WasteTypeInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub
#Region "Data Manipulation-Add,Edit,Del"

        Private Function Save(ByVal WasteTypeCont As Container.WasteType, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If WasteTypeCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With WasteTypeInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "Codetype = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasteTypeCont.CodeType) & "'" & _
                                                 " AND Code = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasteTypeCont.Code) & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("IsHost")) = 1 Then
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
                                .TableName = "CODEMASTER WITH (ROWLOCK)"
                                .AddField("CodeDesc", WasteTypeCont.CodeDesc, SQLControl.EnumDataType.dtString)
                                .AddField("SysCode", WasteTypeCont.SysCode, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsHost", WasteTypeCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastUpdateBy", WasteTypeCont.LastUpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", WasteTypeCont.Active, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE Codetype = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasteTypeCont.CodeType) & "'" & _
                                                 " AND Code = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasteTypeCont.Code) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("CodeType", WasteTypeCont.CodeType, SQLControl.EnumDataType.dtStringN)
                                                .AddField("Code", WasteTypeCont.Code, SQLControl.EnumDataType.dtString)
                                                .AddField("CodeSeq", WasteTypeCont.CodeSeq, SQLControl.EnumDataType.dtNumeric)

                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE Codetype = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasteTypeCont.CodeType) & "'" & _
                                                 " AND Code = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasteTypeCont.Code) & "'")
                                End Select
                            End With
                            Try
                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True

                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                Else
                                    message = axExecute.Message.ToString()
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/WasteType", axExecute.Message & strSQL, axExecute.StackTrace)
                                Return False
                            Finally
                                objSQL.Dispose()
                            End Try
                        End If

                    End If
                End If
            Catch axAssign As ApplicationException
                message = message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/WasteType", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/WasteType", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                WasteTypeCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function Insert(ByVal WasteTypeCont As Container.WasteType, ByRef message As String) As Boolean
            Return Save(WasteTypeCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        Public Function Update(ByVal WasteTypeCont As Container.WasteType, ByRef message As String) As Boolean
            Return Save(WasteTypeCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal WasteTypeCont As Container.WasteType, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If WasteTypeCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With WasteTypeInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, " Codetype = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteTypeCont.CodeType) & "'" & _
                                                " AND Code = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteTypeCont.Code) & "'")
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

                        If blnFound = True Then
                            With objSQL
                                .AddField("SyncLastUpd", WasteTypeCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                strSQL = BuildUpdate("CODEMASTER WITH (ROWLOCK)", " SET IsHost = 0" & _
                                                     " WHERE Codetype = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteTypeCont.CodeType) & "' AND Code = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteTypeCont.Code) & "'")
                            End With
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/WasteType", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/WasteType", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/WasteType", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                WasteTypeCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function


#End Region

#Region "Data Selection"
        Public Function CountCodeSeq(ByVal CodeType As String) As Integer
            Try
                StartConnection()
                StartSQLControl()

                Dim dtTmp As DataTable
                Dim intCodeSeq As Integer

                strSQL = "SELECT * FROM CODEMASTER WITH (NOLOCK) WHERE (CodeType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CodeType) & "')"

                dtTmp = objConn.Execute(strSQL, CommandType.Text)

                If dtTmp Is Nothing = False Then
                    If dtTmp.Rows.Count > 0 Then
                        intCodeSeq = dtTmp.Rows.Count
                    End If
                Else
                    intCodeSeq = 1
                End If

                Return intCodeSeq

            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/WasteType", ex.Message, ex.StackTrace)
            Finally
                EndSQLControl()
                EndConnection()

            End Try
        End Function

        Public Overloads Function GetWasteType(ByVal CodeType As System.String, ByVal Code As System.String) As Container.WasteType
            Dim rWasteType As Container.WasteType = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            StartSQLControl()
            Try
                If StartConnection() = True Then
                    With WasteTypeInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "CodeType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CodeType) & "' AND Code = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Code) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWasteType = New Container.WasteType
                                rWasteType.CodeType = drRow.Item("CodeType")
                                rWasteType.Code = drRow.Item("Code")
                                rWasteType.CodeDesc = drRow.Item("CodeDesc")
                                rWasteType.CodeSeq = drRow.Item("CodeSeq")
                                rWasteType.SysCode = drRow.Item("SysCode")
                                rWasteType.rowguid = drRow.Item("rowguid")
                                rWasteType.SyncCreate = drRow.Item("SyncCreate")
                                rWasteType.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWasteType.IsHost = drRow.Item("IsHost")
                                rWasteType.LastSyncBy = drRow.Item("LastSyncBy")
                                rWasteType.LastUpdateBy = drRow.Item("LastUpdateBy")
                                rWasteType.Active = drRow.Item("Active")
                            Else
                                rWasteType = Nothing
                            End If
                        Else
                            rWasteType = Nothing
                        End If
                    End With
                End If
                Return rWasteType
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/WasteType", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rWasteType = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWasteTypeByCodeDesc(ByVal CodeType As System.String, ByVal CodeDesc As System.String) As Container.WasteType
            Dim rWasteType As Container.WasteType = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            StartSQLControl()
            Try
                If StartConnection() = True Then
                    With WasteTypeInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "CodeType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CodeType) & "' AND CodeDesc = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CodeDesc) & "' AND Active=1")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWasteType = New Container.WasteType
                                rWasteType.CodeType = drRow.Item("CodeType")
                                rWasteType.Code = drRow.Item("Code")
                                rWasteType.CodeDesc = drRow.Item("CodeDesc")
                                rWasteType.CodeSeq = drRow.Item("CodeSeq")
                                rWasteType.SysCode = drRow.Item("SysCode")
                                rWasteType.rowguid = drRow.Item("rowguid")
                                rWasteType.SyncCreate = drRow.Item("SyncCreate")
                                rWasteType.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWasteType.IsHost = drRow.Item("IsHost")
                                rWasteType.LastSyncBy = drRow.Item("LastSyncBy")
                                rWasteType.LastUpdateBy = drRow.Item("LastUpdateBy")
                                rWasteType.Active = drRow.Item("Active")
                            Else
                                rWasteType = Nothing
                            End If
                        Else
                            rWasteType = Nothing
                        End If
                    End With
                End If
                Return rWasteType
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/WasteType", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rWasteType = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWasteTypeByType(ByVal CodeType As String, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With WasteTypeInfo.MyInfo
                    strSQL = "SELECT Code, CodeDesc, SysCode FROM CODEMASTER WITH(NOLOCK) WHERE CodeType='" & CodeType & "' and Active = 1 ORDER BY CodeSeq"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetWastebyCodeDesc(ByVal CodeType As System.String, ByVal CodeDesc As System.String) As Data.DataTable
            If StartConnection() = True Then
                With WasteTypeInfo.MyInfo
                    strSQL = "SELECT Code, CodeDesc FROM CODEMASTER WITH(NOLOCK) WHERE CodeType='" & CodeType & "' And codeDesc='" & CodeDesc & "' and Active = 1 ORDER BY CodeSeq"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetCodeSec(ByVal CodeType As System.String, ByVal Code As System.String) As Data.DataTable
            If StartConnection() = True Then
                With WasteTypeInfo.MyInfo
                    strSQL = "SELECT Code, CodeDesc, CodeSeq FROM CODEMASTER WITH(NOLOCK) WHERE CodeType='" & CodeType & "' And code='" & Code & "' and Active = 1 ORDER BY CodeSeq"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetGroupingTreatment(ByVal Code As System.String) As Data.DataTable
            If StartConnection() = True Then
                With WasteTypeInfo.MyInfo
                    strSQL = "select CD.* from CODEMASTER CM LEFT JOIN CODEMASTER CD on CM.SysCode = CD.Code WHERE CM.CodeType ='WTH' and CD.CodeType='GRP' and CM.Code = '" & Code & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetWasteTypeByCode(ByVal Code As String, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With WasteTypeInfo.MyInfo
                    strSQL = "SELECT Code, CodeDesc FROM CODEMASTER WITH(NOLOCK) WHERE Code='" & Code & "' and Active = 1 ORDER BY CodeSeq"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetWasteTypeByCodeWAC(ByVal Code As String, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With WasteTypeInfo.MyInfo
                    strSQL = "SELECT w.WasCode, c.Code, c.CodeDesc FROM WAC_WASTYPE w Inner Join CodeMaster c on c.Code=w.TypeCode WHERE c.CodeType='WTY' AND c.Active=1 AND w.WasCode='" & Code & "' ORDER BY c.CodeSeq"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetOperationType(ByVal CodeType As System.String, ByVal Code As System.String) As Container.WasteType
            Dim rWasteType As Container.WasteType = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            StartSQLControl()
            Try
                If StartConnection() = True Then
                    With WasteTypeInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "CodeType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CodeType) & "' AND Code = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Code) & "' AND Active=1")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWasteType = New Container.WasteType
                                rWasteType.CodeType = drRow.Item("CodeType")
                                rWasteType.Code = drRow.Item("Code")
                                rWasteType.CodeDesc = drRow.Item("CodeDesc")
                                rWasteType.CodeSeq = drRow.Item("CodeSeq")
                                rWasteType.SysCode = drRow.Item("SysCode")
                                rWasteType.rowguid = drRow.Item("rowguid")
                                rWasteType.SyncCreate = drRow.Item("SyncCreate")
                                rWasteType.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWasteType.IsHost = drRow.Item("IsHost")
                                rWasteType.LastSyncBy = drRow.Item("LastSyncBy")
                                rWasteType.LastUpdateBy = drRow.Item("LastUpdateBy")
                                rWasteType.Active = drRow.Item("Active")
                            Else
                                rWasteType = Nothing
                            End If
                        Else
                            rWasteType = Nothing
                        End If
                    End With
                End If
                Return rWasteType
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/WasteType", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rWasteType = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetOperationTypeAPI(ByVal CodeType As System.String, ByVal CodeNum As System.String) As Container.WasteType
            Dim rWasteType As Container.WasteType = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            StartSQLControl()
            Try
                If StartConnection() = True Then
                    With WasteTypeInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "CodeType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CodeType) & "' AND CodeNum = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CodeNum) & "' AND Active=1")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWasteType = New Container.WasteType
                                rWasteType.CodeType = drRow.Item("CodeType")
                                rWasteType.Code = drRow.Item("Code")
                                rWasteType.CodeDesc = drRow.Item("CodeDesc")
                                rWasteType.CodeSeq = drRow.Item("CodeSeq")
                                rWasteType.SysCode = drRow.Item("SysCode")
                                rWasteType.rowguid = drRow.Item("rowguid")
                                rWasteType.SyncCreate = drRow.Item("SyncCreate")
                                rWasteType.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWasteType.IsHost = drRow.Item("IsHost")
                                rWasteType.LastSyncBy = drRow.Item("LastSyncBy")
                                rWasteType.LastUpdateBy = drRow.Item("LastUpdateBy")
                                rWasteType.Active = drRow.Item("Active")
                            Else
                                rWasteType = Nothing
                            End If
                        Else
                            rWasteType = Nothing
                        End If
                    End With
                End If
                Return rWasteType
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/WasteType", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rWasteType = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWasteType(ByVal CodeType As System.String, ByVal Code As System.String, DecendingOrder As Boolean) As List(Of Container.WasteType)
            Dim rWasteType As Container.WasteType = Nothing
            Dim lstState As List(Of Container.WasteType) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With WasteTypeInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal Code As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "CodeType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CodeType) & "' AND Code = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Code) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rWasteType = New Container.WasteType
                                rWasteType.CodeType = drRow.Item("CodeType")
                                rWasteType.Code = drRow.Item("Code")
                                rWasteType.CodeDesc = drRow.Item("CodeDesc")
                                rWasteType.CodeSeq = drRow.Item("CodeSeq")
                                rWasteType.SysCode = drRow.Item("SysCode")
                                rWasteType.rowguid = drRow.Item("rowguid")
                                rWasteType.SyncCreate = drRow.Item("SyncCreate")
                                rWasteType.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWasteType.IsHost = drRow.Item("IsHost")
                                rWasteType.LastSyncBy = drRow.Item("LastSyncBy")
                                rWasteType.LastUpdateBy = drRow.Item("LastUpdateBy")
                                rWasteType.Active = drRow.Item("Active")
                            Next
                            lstState.Add(rWasteType)
                        Else
                            rWasteType = Nothing
                        End If
                        Return lstState
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/WasteType", ex.Message, ex.StackTrace)
            Finally
                rWasteType = Nothing
                lstState = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWasteTypeList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With WasteTypeInfo.MyInfo
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

        Public Overloads Function GetWasteTypesList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As List(Of Container.WasteType)
            Dim rWasteType As Container.WasteType = Nothing
            Dim lstState As List(Of Container.WasteType) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Try
                If StartConnection() = True Then
                    With WasteTypeInfo.MyInfo
                        If SQL = Nothing Or SQL = String.Empty Then
                            strSQL = BuildSelect(.FieldsList, .TableName, FieldCond)
                            strSQL &= "ORDER BY CodeSeq"
                        Else
                            strSQL = SQL
                        End If
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            lstState = New List(Of Container.WasteType)
                            For Each drRow As DataRow In dtTemp.Rows
                                rWasteType = New Container.WasteType
                                rWasteType.CodeType = drRow.Item("CodeType")
                                rWasteType.Code = drRow.Item("Code")
                                rWasteType.CodeDesc = drRow.Item("CodeDesc")
                                rWasteType.CodeNum = drRow.Item("CodeNum")
                                lstState.Add(rWasteType)
                            Next

                        Else
                            rWasteType = Nothing
                        End If
                        Return lstState
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/WasteType", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rWasteType = Nothing
                lstState = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWasteTransTypesListAPI(Optional ByVal TransType As String = "", Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As List(Of Container.WasteType)
            Dim rWasteType As Container.WasteType = Nothing
            Dim lstState As List(Of Container.WasteType) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With WasteTypeInfo.MyInfo
                        If SQL = Nothing Or SQL = String.Empty Then
                            strSQL = BuildSelect(.FieldsList, .TableName, FieldCond)
                            strSQL &= "WHERE codetype = 'CNT' and code='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransType) & "' and active = 1 ORDER BY CodeSeq"
                        Else
                            strSQL = SQL
                        End If
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            lstState = New List(Of Container.WasteType)
                            For Each drRow As DataRow In dtTemp.Rows
                                rWasteType = New Container.WasteType
                                rWasteType.CodeType = drRow.Item("CodeType")
                                rWasteType.Code = drRow.Item("Code")
                                rWasteType.CodeDesc = drRow.Item("CodeDesc")
                                rWasteType.CodeNum = drRow.Item("CodeNum")
                                lstState.Add(rWasteType)
                            Next

                        Else
                            rWasteType = Nothing
                        End If
                        Return lstState
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/WasteType", ex.Message, ex.StackTrace)
            Finally
                rWasteType = Nothing
                lstState = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWasteTypesListAPI(Optional ByVal CompanyType As String = "", Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As List(Of Container.WasteType)
            Dim rWasteType As Container.WasteType = Nothing
            Dim lstState As List(Of Container.WasteType) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With WasteTypeInfo.MyInfo
                        If SQL = Nothing Or SQL = String.Empty Then
                            strSQL = BuildSelect(.FieldsList, .TableName, FieldCond)
                        Else
                            strSQL = SQL
                        End If

                        If CompanyType <> "" AndAlso CompanyType IsNot Nothing Then
                            strSQL &= " WHERE codetype = 'FCT' and code='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompanyType) & "' and active = 1 ORDER BY CodeSeq"
                        End If
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            lstState = New List(Of Container.WasteType)
                            For Each drRow As DataRow In dtTemp.Rows
                                rWasteType = New Container.WasteType
                                rWasteType.CodeType = drRow.Item("CodeType")
                                rWasteType.Code = drRow.Item("Code")
                                rWasteType.CodeDesc = drRow.Item("CodeDesc")
                                rWasteType.CodeNum = drRow.Item("CodeNum")
                                lstState.Add(rWasteType)
                            Next

                        Else
                            rWasteType = Nothing
                        End If
                        Return lstState
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/WasteType", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rWasteType = Nothing
                lstState = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetStateShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With WasteTypeInfo.MyInfo
                    If ShortListing Then
                        strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Else
                        strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    End If
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

#End Region
    End Class

    Namespace Container
#Region "Class Container"
        Public Class WasteType
            Public fCodeType As System.String = "CodeType"
            Public fCode As System.String = "Code"
            Public fCodeDesc As System.String = "CodeDesc"
            Public fCodeNum As System.String = "CodeNum"
            Public fCodeSeq As System.String = "CodeSeq"
            Public fSysCode As System.String = "SysCode"
            Public frowguid As System.String = "rowguid"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fIsHost As System.String = "IsHost"
            Public fLastSyncBy As System.String = "LastSyncBy"
            Public fLastUpdateBy As System.String = "LastUpdateBy"
            Public fActive As System.String = "Active"



            Protected _CodeType As System.String
            Protected _Code As System.String
            Private _CodeDesc As System.String
            Private _CodeNum As System.String
            Private _CodeSeq As System.Byte
            Private _SysCode As System.Byte
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _IsHost As System.Byte
            Private _LastSyncBy As System.String
            Private _LastUpdateBy As System.String
            Private _Active As System.Byte

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property CodeType As System.String
                Get
                    Return _CodeType
                End Get
                Set(ByVal Value As System.String)
                    _CodeType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property Code As System.String
                Get
                    Return _Code
                End Get
                Set(ByVal Value As System.String)
                    _Code = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property CodeDesc As System.String
                Get
                    Return _CodeDesc
                End Get
                Set(ByVal Value As System.String)
                    _CodeDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property CodeNum As System.String
                Get
                    Return _CodeNum
                End Get
                Set(ByVal Value As System.String)
                    _CodeNum = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property CodeSeq As System.Byte
                Get
                    Return _CodeSeq
                End Get
                Set(ByVal Value As System.Byte)
                    _CodeSeq = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property SysCode As System.Byte
                Get
                    Return _SysCode
                End Get
                Set(ByVal Value As System.Byte)
                    _SysCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
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
            ''' Mandatory
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
            ''' Mandatory
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
            ''' Mandatory
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
            ''' Mandatory
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
            ''' Mandatory
            ''' </summary>
            Public Property LastUpdateBy As System.String
                Get
                    Return _LastUpdateBy
                End Get
                Set(ByVal Value As System.String)
                    _LastUpdateBy = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property Active As System.Byte
                Get
                    Return _Active

                End Get
                Set(ByVal Value As System.Byte)
                    _Active = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class WasteTypeInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "CodeType, Code, CodeDesc, CodeNum,  CodeSeq, SysCode, rowguid, SyncCreate , IsHost, LastSyncBy, SyncLastUpd, LastUpdateBy, Active"
                .CheckFields = "IsHost"
                .TableName = "CODEMASTER WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "CodeType, Code, CodeDesc, CodeSeq, SysCode, rowguid, SyncCreate, IsHost, LastSyncBy,SyncLastUpd, LastUpdateBy, Active"
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
    Public Class WasteTypeScheme
        Inherits Core.SchemeBase

        Protected Overrides Sub InitializeInfo()
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CodeType"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Code"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CodeDesc"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "CodeSeq"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "SysCode"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastUpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)

        End Sub

        Public ReadOnly Property CodeType As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property Code As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property

        Public ReadOnly Property CodeDesc As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property

        Public ReadOnly Property CodeSeq As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property

        Public ReadOnly Property SysCode As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property

        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property

        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property

        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property

        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property

        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property

        Public ReadOnly Property LastUpdateBy As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property

        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace




Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace Actions
#Region "WAC_WRLIST Class"
    Public NotInheritable Class WAC_WRLIST
        Inherits Core.CoreControl
        Private Wac_wrlistInfo As Wac_wrlistInfo = New Wac_wrlistInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal Wac_wrlistCont As Container.Wac_wrlist, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If Wac_wrlistCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Wac_wrlistInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "GeneratorID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.GeneratorID) & "' AND GeneratorLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.GeneratorLocID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_wrlistCont.WasType) & "' AND Allow3R = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_wrlistCont.Allow3R) & "' AND ReceiverID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.ReceiverID) & "' AND ReceiverLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.ReceiverLocID) & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
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
                                .TableName = "Wac_wrlist"
                                .AddField("ReqSupp", Wac_wrlistCont.ReqSupp, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Active", Wac_wrlistCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", Wac_wrlistCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("rowguid", Wac_wrlistCont.rowguid, SQLControl.EnumDataType.dtString)
                                .AddField("CreateDate", Wac_wrlistCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", Wac_wrlistCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", Wac_wrlistCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", Wac_wrlistCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("SyncCreate", Wac_wrlistCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", Wac_wrlistCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastSyncBy", Wac_wrlistCont.LastSyncBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "GeneratorID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.GeneratorID) & "' AND GeneratorLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.GeneratorLocID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_wrlistCont.WasType) & "' AND Allow3R = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_wrlistCont.Allow3R) & "' AND ReceiverID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.ReceiverID) & "' AND ReceiverLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.ReceiverLocID) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("GeneratorID", Wac_wrlistCont.GeneratorID, SQLControl.EnumDataType.dtString)
                                                .AddField("GeneratorLocID", Wac_wrlistCont.GeneratorLocID, SQLControl.EnumDataType.dtString)
                                                .AddField("WasCode", Wac_wrlistCont.WasCode, SQLControl.EnumDataType.dtString)
                                                .AddField("WasType", Wac_wrlistCont.WasType, SQLControl.EnumDataType.dtNumeric)
                                                .AddField("Allow3R", Wac_wrlistCont.Allow3R, SQLControl.EnumDataType.dtNumeric)
                                                .AddField("ReceiverID", Wac_wrlistCont.ReceiverID, SQLControl.EnumDataType.dtString)
                                                .AddField("ReceiverLocID", Wac_wrlistCont.ReceiverLocID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "GeneratorID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.GeneratorID) & "' AND GeneratorLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.GeneratorLocID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_wrlistCont.WasType) & "' AND Allow3R = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_wrlistCont.Allow3R) & "' AND ReceiverID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.ReceiverID) & "' AND ReceiverLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.ReceiverLocID) & "'")
                                End Select
                            End With
                            Try
                                If BatchExecute Then
                                    BatchList.Add(strSQL)
                                    If Commit Then
                                        objConn.BatchExecute(BatchList, CommandType.Text, True)
                                    End If
                                Else
                                    'execute
                                    objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                End If
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                    Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                                Else
                                    message = axExecute.Message.ToString()
                                    Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If

                    End If
                End If
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Return False
            Finally
                Wac_wrlistCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal Wac_wrlistCont As Container.Wac_wrlist, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Wac_wrlistCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal Wac_wrlistCont As Container.Wac_wrlist, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Wac_wrlistCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal Wac_wrlistCont As Container.Wac_wrlist, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Wac_wrlistCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Wac_wrlistInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "GeneratorID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.GeneratorID) & "' AND GeneratorLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.GeneratorLocID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_wrlistCont.WasType) & "' AND Allow3R = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_wrlistCont.Allow3R) & "' AND ReceiverID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.ReceiverID) & "' AND ReceiverLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.ReceiverLocID) & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("InUse")) = 1 Then
                                        blnInUse = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        If blnFound = True And blnInUse = True Then
                            With objSQL
                                strSQL = BuildUpdate(Wac_wrlistInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = GETDATE() , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.UpdateBy) & "' WHERE" & _
                                "GeneratorID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.GeneratorID) & "' AND GeneratorLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.GeneratorLocID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_wrlistCont.WasType) & "' AND Allow3R = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_wrlistCont.Allow3R) & "' AND ReceiverID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.ReceiverID) & "' AND ReceiverLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.ReceiverLocID) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Wac_wrlistInfo.MyInfo.TableName, "GeneratorID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.GeneratorID) & "' AND GeneratorLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.GeneratorLocID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_wrlistCont.WasType) & "' AND Allow3R = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_wrlistCont.Allow3R) & "' AND ReceiverID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.ReceiverID) & "' AND ReceiverLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wrlistCont.ReceiverLocID) & "'")
                        End If

                        Try

                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Return False
            Finally
                Wac_wrlistCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetWAC_WRLIST(ByVal GeneratorID As System.String, ByVal GeneratorLocID As System.String, ByVal WasCode As System.String, ByVal WasType As System.Byte, ByVal Allow3R As System.Byte, ByVal ReceiverID As System.String, ByVal ReceiverLocID As System.String) As Container.Wac_wrlist
            Dim rWac_wrlist As Container.Wac_wrlist = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With Wac_wrlistInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "GeneratorID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, GeneratorID) & "' AND GeneratorLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, GeneratorLocID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, WasType) & "' AND Allow3R = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Allow3R) & "' AND ReceiverID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ReceiverID) & "' AND ReceiverLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ReceiverLocID) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWac_wrlist = New Container.Wac_wrlist
                                rWac_wrlist.GeneratorID = drRow.Item("GeneratorID")
                                rWac_wrlist.GeneratorLocID = drRow.Item("GeneratorLocID")
                                rWac_wrlist.WasCode = drRow.Item("WasCode")
                                rWac_wrlist.WasType = drRow.Item("WasType")
                                rWac_wrlist.Allow3R = drRow.Item("Allow3R")
                                rWac_wrlist.ReceiverID = drRow.Item("ReceiverID")
                                rWac_wrlist.ReceiverLocID = drRow.Item("ReceiverLocID")
                                rWac_wrlist.ReqSupp = drRow.Item("ReqSupp")
                                rWac_wrlist.Active = drRow.Item("Active")
                                rWac_wrlist.Inuse = drRow.Item("Inuse")
                                rWac_wrlist.rowguid = drRow.Item("rowguid")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rWac_wrlist.CreateDate = drRow.Item("CreateDate")
                                End If
                                rWac_wrlist.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rWac_wrlist.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rWac_wrlist.UpdateBy = drRow.Item("UpdateBy")
                                If Not IsDBNull(drRow.Item("SyncCreate")) Then
                                    rWac_wrlist.SyncCreate = drRow.Item("SyncCreate")
                                End If
                                If Not IsDBNull(drRow.Item("SyncLastUpd")) Then
                                    rWac_wrlist.SyncLastUpd = drRow.Item("SyncLastUpd")
                                End If
                                rWac_wrlist.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rWac_wrlist = Nothing
                            End If
                        Else
                            rWac_wrlist = Nothing
                        End If
                    End With
                End If
                Return rWac_wrlist
            Catch ex As Exception
                Throw ex
            Finally
                rWac_wrlist = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_WRLIST(ByVal GeneratorID As System.String, ByVal GeneratorLocID As System.String, ByVal WasCode As System.String, ByVal WasType As System.Byte, ByVal Allow3R As System.Byte, ByVal ReceiverID As System.String, ByVal ReceiverLocID As System.String, DecendingOrder As Boolean) As List(Of Container.Wac_wrlist)
            Dim rWac_wrlist As Container.Wac_wrlist = Nothing
            Dim lstWac_wrlist As List(Of Container.Wac_wrlist) = New List(Of Container.Wac_wrlist)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With Wac_wrlistInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by GeneratorID, GeneratorLocID, WasCode, WasType, Allow3R, ReceiverID, ReceiverLocID DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "GeneratorID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, GeneratorID) & "' AND GeneratorLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, GeneratorLocID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, WasType) & "' AND Allow3R = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Allow3R) & "' AND ReceiverID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ReceiverID) & "' AND ReceiverLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ReceiverLocID) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rWac_wrlist = New Container.Wac_wrlist
                                rWac_wrlist.GeneratorID = drRow.Item("GeneratorID")
                                rWac_wrlist.GeneratorLocID = drRow.Item("GeneratorLocID")
                                rWac_wrlist.WasCode = drRow.Item("WasCode")
                                rWac_wrlist.WasType = drRow.Item("WasType")
                                rWac_wrlist.Allow3R = drRow.Item("Allow3R")
                                rWac_wrlist.ReceiverID = drRow.Item("ReceiverID")
                                rWac_wrlist.ReceiverLocID = drRow.Item("ReceiverLocID")
                                rWac_wrlist.ReqSupp = drRow.Item("ReqSupp")
                                rWac_wrlist.Active = drRow.Item("Active")
                                rWac_wrlist.Inuse = drRow.Item("Inuse")
                                rWac_wrlist.rowguid = drRow.Item("rowguid")
                                rWac_wrlist.CreateBy = drRow.Item("CreateBy")
                                rWac_wrlist.UpdateBy = drRow.Item("UpdateBy")
                                rWac_wrlist.LastSyncBy = drRow.Item("LastSyncBy")
                                lstWac_wrlist.Add(rWac_wrlist)
                            Next

                        Else
                            rWac_wrlist = Nothing
                        End If
                        Return lstWac_wrlist
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rWac_wrlist = Nothing
                lstWac_wrlist = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_WRLISTList(Optional ByVal WasteCode As String = "", Optional ByVal WasteType As String = "", Optional ByVal isQualified As String = "") As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_wrlistInfo.MyInfo
                    strSQL = "exec WAC_WRCompilation '','" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "'," & _
                        "'" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "'," & _
                        "'','','" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, isQualified) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GETWAC_WRListEligibleReceiver(ByVal LocID As String, ByVal WasteCode As String, ByVal WasteType As String, Optional ByVal HandlingGroup As String = "", Optional ByVal CompanyName As String = "", Optional ByVal State As String = "", Optional ByVal HandlingType As String = "", Optional ByVal isSelected As String = "") As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_wrlistInfo.MyInfo
                    strSQL = "exec WAC_QualifiedWR '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "', '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "','" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "', '" & HandlingType & "', '','" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, HandlingGroup) & "','" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompanyName) & "','" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, State) & "','" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, isSelected) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function
#End Region
    End Class
#End Region

#Region "Container"
    Namespace Container
#Region "Wac_wrlist Container"
        Public Class Wac_wrlist_FieldName
            Public GeneratorID As System.String = "GeneratorID"
            Public GeneratorLocID As System.String = "GeneratorLocID"
            Public WasCode As System.String = "WasCode"
            Public WasType As System.String = "WasType"
            Public Allow3R As System.String = "Allow3R"
            Public ReceiverID As System.String = "ReceiverID"
            Public ReceiverLocID As System.String = "ReceiverLocID"            
            Public ReqSupp As System.String = "ReqSupp"
            Public Active As System.String = "Active"
            Public Inuse As System.String = "Inuse"
            Public Flag As System.String = "Flag"
            Public rowguid As System.String = "rowguid"
            Public CreateDate As System.String = "CreateDate"
            Public CreateBy As System.String = "CreateBy"
            Public LastUpdate As System.String = "LastUpdate"
            Public UpdateBy As System.String = "UpdateBy"
            Public SyncCreate As System.String = "SyncCreate"
            Public SyncLastUpd As System.String = "SyncLastUpd"
            Public LastSyncBy As System.String = "LastSyncBy"
            Public IsSelected As System.String = "IsSelected"
            Public HandlingGroup As System.String = "HandlingGroup"
        End Class

        Public Class Wac_wrlist
            Protected _GeneratorID As System.String
            Protected _GeneratorLocID As System.String
            Protected _WasCode As System.String
            Protected _WasType As System.String
            Protected _Allow3R As System.Byte
            Protected _ReceiverID As System.String
            Protected _ReceiverLocID As System.String
            Private _ReqSupp As System.Byte
            Private _Active As System.Byte
            Private _Inuse As System.Byte
            Private _rowguid As System.Guid
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _LastSyncBy As System.String
            Private _IsSelected As System.Byte
            Private _HandlingGroup As System.Byte

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property GeneratorID As System.String
                Get
                    Return _GeneratorID
                End Get
                Set(ByVal Value As System.String)
                    _GeneratorID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property GeneratorLocID As System.String
                Get
                    Return _GeneratorLocID
                End Get
                Set(ByVal Value As System.String)
                    _GeneratorLocID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property WasCode As System.String
                Get
                    Return _WasCode
                End Get
                Set(ByVal Value As System.String)
                    _WasCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property WasType As System.String
                Get
                    Return _WasType
                End Get
                Set(ByVal Value As System.String)
                    _WasType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property Allow3R As System.Byte
                Get
                    Return _Allow3R
                End Get
                Set(ByVal Value As System.Byte)
                    _Allow3R = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ReceiverID As System.String
                Get
                    Return _ReceiverID
                End Get
                Set(ByVal Value As System.String)
                    _ReceiverID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ReceiverLocID As System.String
                Get
                    Return _ReceiverLocID
                End Get
                Set(ByVal Value As System.String)
                    _ReceiverLocID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ReqSupp As System.Byte
                Get
                    Return _ReqSupp
                End Get
                Set(ByVal Value As System.Byte)
                    _ReqSupp = Value
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
            Public Property Inuse As System.Byte
                Get
                    Return _Inuse
                End Get
                Set(ByVal Value As System.Byte)
                    _Inuse = Value
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
            ''' Non-Mandatory, Allow Null
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
            ''' Non-Mandatory, Allow Null
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

            Public Property IsSelected As System.Byte
                Get
                    Return _IsSelected
                End Get
                Set(ByVal Value As System.Byte)
                    _IsSelected = Value
                End Set
            End Property

            Public Property HandlingGroup As System.Byte
                Get
                    Return _HandlingGroup
                End Get
                Set(ByVal Value As System.Byte)
                    _HandlingGroup = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace
#End Region

#Region "Class Info"
#Region "Wac_wrlist Info"
    Public Class Wac_wrlistInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "GeneratorID,GeneratorLocID,WasCode,WasType,Allow3R,ReceiverID,ReceiverLocID,ReqSupp,Active,Inuse,Flag,rowguid,CreateDate,CreateBy,LastUpdate,UpdateBy,SyncCreate,SyncLastUpd,LastSyncBy"
                .CheckFields = "WasType,Allow3R,ReqSupp,Active,Inuse,Flag"
                .TableName = "Wac_wrlist"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "GeneratorID,GeneratorLocID,WasCode,WasType,Allow3R,ReceiverID,ReceiverLocID,ReqSupp,Active,Inuse,Flag,rowguid,CreateDate,CreateBy,LastUpdate,UpdateBy,SyncCreate,SyncLastUpd,LastSyncBy"
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
#End Region

#Region "Scheme"
#Region "WAC_WRLIST Scheme"
    Public Class WAC_WRLISTScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "GeneratorID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "GeneratorLocID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WasCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "WasType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Allow3R"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ReceiverID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ReceiverLocID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ReqSupp"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)

        End Sub

        Public ReadOnly Property GeneratorID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property GeneratorLocID As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property WasCode As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property WasType As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property Allow3R As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property ReceiverID As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property ReceiverLocID As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property

        Public ReadOnly Property ReqSupp As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
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
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
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
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace

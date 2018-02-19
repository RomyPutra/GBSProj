Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Actions
#Region "WAC_BASELABEL Class"
    Public NotInheritable Class WAC_BASELABEL
        Inherits Core.CoreControl
        Private Wac_baselabelInfo As Wac_baselabelInfo = New Wac_baselabelInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal Wac_baselabelCont As Container.Wac_baselabel, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If Wac_baselabelCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Wac_baselabelInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "LabelID = '" & Wac_baselabelCont.LabelID & "' AND LabelCode = '" & Wac_baselabelCont.LabelCode & "' AND LabelType = '" & Wac_baselabelCont.LabelType & "'")
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
                                .TableName = "Wac_baselabel"
                                .AddField("Description", Wac_baselabelCont.Description, SQLControl.EnumDataType.dtStringN)
                                .AddField("LabelMode", Wac_baselabelCont.LabelMode, SQLControl.EnumDataType.dtNumeric)
                                .AddField("TotalBind", Wac_baselabelCont.TotalBind, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LabelPath1", Wac_baselabelCont.LabelPath1, SQLControl.EnumDataType.dtStringN)
                                .AddField("LabelPath2", Wac_baselabelCont.LabelPath2, SQLControl.EnumDataType.dtStringN)
                                .AddField("LabelPath3", Wac_baselabelCont.LabelPath3, SQLControl.EnumDataType.dtStringN)
                                .AddField("Remark", Wac_baselabelCont.Remark, SQLControl.EnumDataType.dtStringN)
                                .AddField("CreateDate", Wac_baselabelCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", Wac_baselabelCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", Wac_baselabelCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", Wac_baselabelCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", Wac_baselabelCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", Wac_baselabelCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("SyncCreate", Wac_baselabelCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", Wac_baselabelCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastSyncBy", Wac_baselabelCont.LastSyncBy, SQLControl.EnumDataType.dtString)
                                .AddField("IsHost", Wac_baselabelCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LabelCode", Wac_baselabelCont.LabelCode, SQLControl.EnumDataType.dtString)
                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LabelID = '" & Wac_baselabelCont.LabelID & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("LabelID", Wac_baselabelCont.LabelID, SQLControl.EnumDataType.dtString)
                                                .AddField("LabelType", Wac_baselabelCont.LabelType, SQLControl.EnumDataType.dtNumeric)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LabelID = '" & Wac_baselabelCont.LabelID & "'")
                                End Select
                            End With
                            Try
                                If BatchExecute Then
                                    BatchList.Add(strSQL)
                                    If Commit Then
                                        objConn.BatchExecute(BatchList, CommandType.Text, True)
                                    End If
                                Else
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
                Wac_baselabelCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal Wac_baselabelCont As Container.Wac_baselabel, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Wac_baselabelCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal Wac_baselabelCont As Container.Wac_baselabel, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Wac_baselabelCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal Wac_baselabelCont As Container.Wac_baselabel, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Wac_baselabelCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Wac_baselabelInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "LabelID = '" & Wac_baselabelCont.LabelID & "' AND LabelCode = '" & Wac_baselabelCont.LabelCode & "' AND LabelType = '" & Wac_baselabelCont.LabelType & "'")
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
                                strSQL = BuildUpdate(Wac_baselabelInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = '" & Wac_baselabelCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselabelCont.UpdateBy) & "' WHERE" & _
                                "LabelID = '" & Wac_baselabelCont.LabelID & "' AND LabelCode = '" & Wac_baselabelCont.LabelCode & "' AND LabelType = '" & Wac_baselabelCont.LabelType & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Wac_baselabelInfo.MyInfo.TableName, "LabelID = '" & Wac_baselabelCont.LabelID & "' AND LabelCode = '" & Wac_baselabelCont.LabelCode & "' AND LabelType = '" & Wac_baselabelCont.LabelType & "'")
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
                Wac_baselabelCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetWAC_BASELABEL(ByVal LabelID As System.String, ByVal LabelCode As System.String, ByVal LabelType As System.Int32) As Container.Wac_baselabel
            Dim rWac_baselabel As Container.Wac_baselabel = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With Wac_baselabelInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "LabelID = '" & LabelID & "' AND LabelCode = '" & LabelCode & "' AND LabelType = '" & LabelType & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWac_baselabel = New Container.Wac_baselabel
                                rWac_baselabel.LabelID = drRow.Item("LabelID")
                                rWac_baselabel.LabelCode = drRow.Item("LabelCode")
                                rWac_baselabel.LabelType = drRow.Item("LabelType")
                                rWac_baselabel.Description = drRow.Item("Description")
                                rWac_baselabel.LabelMode = drRow.Item("LabelMode")
                                rWac_baselabel.TotalBind = drRow.Item("TotalBind")
                                rWac_baselabel.LabelPath1 = drRow.Item("LabelPath1")
                                rWac_baselabel.LabelPath2 = drRow.Item("LabelPath2")
                                rWac_baselabel.LabelPath3 = drRow.Item("LabelPath3")
                                rWac_baselabel.Remark = drRow.Item("Remark")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rWac_baselabel.CreateDate = drRow.Item("CreateDate")
                                End If
                                rWac_baselabel.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rWac_baselabel.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rWac_baselabel.UpdateBy = drRow.Item("UpdateBy")
                                rWac_baselabel.Active = drRow.Item("Active")
                                rWac_baselabel.Inuse = drRow.Item("Inuse")
                                rWac_baselabel.rowguid = drRow.Item("rowguid")
                                If Not IsDBNull(drRow.Item("SyncCreate")) Then
                                    rWac_baselabel.SyncCreate = drRow.Item("SyncCreate")
                                End If
                                If Not IsDBNull(drRow.Item("SyncLastUpd")) Then
                                    rWac_baselabel.SyncLastUpd = drRow.Item("SyncLastUpd")
                                End If
                                rWac_baselabel.LastSyncBy = drRow.Item("LastSyncBy")
                                rWac_baselabel.IsHost = drRow.Item("IsHost")
                            Else
                                rWac_baselabel = Nothing
                            End If
                        Else
                            rWac_baselabel = Nothing
                        End If
                    End With
                End If
                Return rWac_baselabel
            Catch ex As Exception
                Throw ex
            Finally
                rWac_baselabel = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_BASELABELByLabelCode(ByVal LabelCode As System.String) As Container.Wac_baselabel
            Dim rWac_baselabel As Container.Wac_baselabel = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With Wac_baselabelInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, " LabelCode = '" & LabelCode & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWac_baselabel = New Container.Wac_baselabel
                                rWac_baselabel.LabelID = drRow.Item("LabelID")
                                rWac_baselabel.LabelCode = drRow.Item("LabelCode")
                                rWac_baselabel.LabelType = drRow.Item("LabelType")
                                rWac_baselabel.Description = drRow.Item("Description")
                                rWac_baselabel.LabelMode = drRow.Item("LabelMode")
                                rWac_baselabel.TotalBind = drRow.Item("TotalBind")
                                rWac_baselabel.LabelPath1 = drRow.Item("LabelPath1")
                                rWac_baselabel.LabelPath2 = drRow.Item("LabelPath2")
                                rWac_baselabel.LabelPath3 = drRow.Item("LabelPath3")
                                rWac_baselabel.Remark = drRow.Item("Remark")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rWac_baselabel.CreateDate = drRow.Item("CreateDate")
                                End If
                                rWac_baselabel.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rWac_baselabel.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rWac_baselabel.UpdateBy = drRow.Item("UpdateBy")
                                rWac_baselabel.Active = drRow.Item("Active")
                                rWac_baselabel.Inuse = drRow.Item("Inuse")
                                If Not IsDBNull(drRow.Item("SyncCreate")) Then
                                    rWac_baselabel.SyncCreate = drRow.Item("SyncCreate")
                                End If
                                If Not IsDBNull(drRow.Item("SyncLastUpd")) Then
                                    rWac_baselabel.SyncLastUpd = drRow.Item("SyncLastUpd")
                                End If
                                rWac_baselabel.LastSyncBy = drRow.Item("LastSyncBy")
                                rWac_baselabel.IsHost = drRow.Item("IsHost")
                            Else
                                rWac_baselabel = Nothing
                            End If
                        Else
                            rWac_baselabel = Nothing
                        End If
                    End With
                End If
                Return rWac_baselabel
            Catch ex As Exception
                Throw ex
            Finally
                rWac_baselabel = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_BASELABEL(ByVal LabelID As System.String, ByVal LabelCode As System.String, ByVal LabelType As System.Int32, DecendingOrder As Boolean) As List(Of Container.Wac_baselabel)
            Dim rWac_baselabel As Container.Wac_baselabel = Nothing
            Dim lstWac_baselabel As List(Of Container.Wac_baselabel) = New List(Of Container.Wac_baselabel)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With Wac_baselabelInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by LabelID, LabelCode, LabelType DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "LabelID = '" & LabelID & "' AND LabelCode = '" & LabelCode & "' AND LabelType = '" & LabelType & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rWac_baselabel = New Container.Wac_baselabel
                                rWac_baselabel.LabelID = drRow.Item("LabelID")
                                rWac_baselabel.LabelCode = drRow.Item("LabelCode")
                                rWac_baselabel.LabelType = drRow.Item("LabelType")
                                rWac_baselabel.Description = drRow.Item("Description")
                                rWac_baselabel.LabelMode = drRow.Item("LabelMode")
                                rWac_baselabel.TotalBind = drRow.Item("TotalBind")
                                rWac_baselabel.LabelPath1 = drRow.Item("LabelPath1")
                                rWac_baselabel.LabelPath2 = drRow.Item("LabelPath2")
                                rWac_baselabel.LabelPath3 = drRow.Item("LabelPath3")
                                rWac_baselabel.Remark = drRow.Item("Remark")
                                rWac_baselabel.CreateBy = drRow.Item("CreateBy")
                                rWac_baselabel.UpdateBy = drRow.Item("UpdateBy")
                                rWac_baselabel.Active = drRow.Item("Active")
                                rWac_baselabel.Inuse = drRow.Item("Inuse")
                                rWac_baselabel.rowguid = drRow.Item("rowguid")
                                rWac_baselabel.LastSyncBy = drRow.Item("LastSyncBy")
                                rWac_baselabel.IsHost = drRow.Item("IsHost")
                                lstWac_baselabel.Add(rWac_baselabel)
                            Next

                        Else
                            rWac_baselabel = Nothing
                        End If
                        Return lstWac_baselabel
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rWac_baselabel = Nothing
                lstWac_baselabel = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_WASLABELBASE(ByVal WasCode As String, ByVal WasType As String, Optional ByVal strFilter As String = "") As Data.DataTable
            If StartConnection() = True Then
                With Wac_baselabelInfo.MyInfo
                    StartSQLControl()
                    strSQL = "Select BS.LabelCode, BS.Description, BS.LabelID, BS.LabelPath1 From WAC_BASELABEL BS WITH (NOLOCK) "
                    If WasCode <> "" AndAlso WasType <> "" Then
                        strSQL &= " LEFT JOIN WAC_WASLABEL WS WITH (NOLOCK) ON BS.LabelID=WS.LabelID "
                        strSQL &= " WHERE WS.WasCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasCode) & "' AND WS.WasType='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasType) & "'"
                    Else
                        strSQL &= " WHERE " & strFilter
                    End If
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetWAC_BASELABELList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Wac_baselabelInfo.MyInfo
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

        Public Overloads Function GetLabelURLByLabelID(ByVal LabelID As String) As String
            Dim LabelURL As String = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With Wac_baselabelInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "LabelID = '" & LabelID & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                LabelURL = drRow.Item("LabelPath1")
                            Else
                                LabelURL = Nothing
                            End If
                        Else
                            LabelURL = Nothing
                        End If
                    End With
                End If
                Return LabelURL
            Catch ex As Exception
                Throw ex
            Finally
                LabelURL = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function IsLabelNoExist(ByVal LabelNo As String, ByVal Command As String, ByVal LabelID As String, Optional ByVal type As String = "") As Boolean
            Dim strSQL As String

            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim dt As New DataTable
            IsLabelNoExist = False

            Try

                If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()

                    strSQL = "SELECT LabelCode, LabelID FROM WAC_BASELABEL WITH (NOLOCK) WHERE LabelCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, LabelNo) & "'"

                    'execute
                    dt = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, "WAC_BASELABEL"), Data.DataTable)

                    If dt.Rows.Count > 0 Then
                        If Command.Contains("Save") Then
                            IsLabelNoExist = False
                        Else
                            If dt.Rows(0)("LabelID") = LabelID Then
                                IsLabelNoExist = True
                            Else
                                IsLabelNoExist = False
                            End If
                        End If
                    Else
                        IsLabelNoExist = True
                    End If
                End If

            Catch exExecute As ApplicationException

            Catch exExecute As Exception

            Finally

                rdr = Nothing
                dt = Nothing
                EndSQLControl()
                EndConnection()
            End Try

            Return IsLabelNoExist
        End Function

#End Region
    End Class
#End Region

#Region "Container"
    Namespace Container
#Region "Wac_baselabel Container"
        Public Class Wac_baselabel_FieldName
            Public LabelID As System.String = "LabelID"
            Public LabelCode As System.String = "LabelCode"
            Public LabelType As System.String = "LabelType"
            Public Description As System.String = "Description"
            Public LabelMode As System.String = "LabelMode"
            Public TotalBind As System.String = "TotalBind"
            Public LabelPath1 As System.String = "LabelPath1"
            Public LabelPath2 As System.String = "LabelPath2"
            Public LabelPath3 As System.String = "LabelPath3"
            Public Remark As System.String = "Remark"
            Public CreateDate As System.String = "CreateDate"
            Public CreateBy As System.String = "CreateBy"
            Public LastUpdate As System.String = "LastUpdate"
            Public UpdateBy As System.String = "UpdateBy"
            Public Active As System.String = "Active"
            Public Inuse As System.String = "Inuse"
            Public Flag As System.String = "Flag"
            Public rowguid As System.String = "rowguid"
            Public SyncCreate As System.String = "SyncCreate"
            Public SyncLastUpd As System.String = "SyncLastUpd"
            Public LastSyncBy As System.String = "LastSyncBy"
            Public IsHost As System.String = "IsHost"
        End Class

        Public Class Wac_baselabel
            Protected _LabelID As System.String
            Protected _LabelCode As System.String
            Protected _LabelType As System.Int32
            Private _Description As System.String
            Private _LabelMode As System.Byte
            Private _TotalBind As System.Int32
            Private _LabelPath1 As System.String
            Private _LabelPath2 As System.String
            Private _LabelPath3 As System.String
            Private _Remark As System.String
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _Active As System.Byte
            Private _Inuse As System.Byte
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _LastSyncBy As System.String
            Private _IsHost As System.Byte

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property LabelID As System.String
                Get
                    Return _LabelID
                End Get
                Set(ByVal Value As System.String)
                    _LabelID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property LabelCode As System.String
                Get
                    Return _LabelCode
                End Get
                Set(ByVal Value As System.String)
                    _LabelCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property LabelType As System.Int32
                Get
                    Return _LabelType
                End Get
                Set(ByVal Value As System.Int32)
                    _LabelType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Description As System.String
                Get
                    Return _Description
                End Get
                Set(ByVal Value As System.String)
                    _Description = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LabelMode As System.Byte
                Get
                    Return _LabelMode
                End Get
                Set(ByVal Value As System.Byte)
                    _LabelMode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TotalBind As System.Int32
                Get
                    Return _TotalBind
                End Get
                Set(ByVal Value As System.Int32)
                    _TotalBind = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LabelPath1 As System.String
                Get
                    Return _LabelPath1
                End Get
                Set(ByVal Value As System.String)
                    _LabelPath1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LabelPath2 As System.String
                Get
                    Return _LabelPath2
                End Get
                Set(ByVal Value As System.String)
                    _LabelPath2 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LabelPath3 As System.String
                Get
                    Return _LabelPath3
                End Get
                Set(ByVal Value As System.String)
                    _LabelPath3 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Remark As System.String
                Get
                    Return _Remark
                End Get
                Set(ByVal Value As System.String)
                    _Remark = Value
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

        End Class
#End Region
    End Namespace
#End Region

#Region "Class Info"
#Region "Wac_baselabel Info"
    Public Class Wac_baselabelInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "LabelID,LabelCode,LabelType,Description,LabelMode,TotalBind,LabelPath1,LabelPath2,LabelPath3,Remark,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,LastSyncBy,IsHost"
                .CheckFields = "LabelMode,Active,Inuse,Flag,IsHost"
                .TableName = "Wac_baselabel"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "LabelID,LabelCode,LabelType,Description,LabelMode,TotalBind,LabelPath1,LabelPath2,LabelPath3,Remark,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,LastSyncBy,IsHost"
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
#Region "WAC_BASELABEL Scheme"
    Public Class WAC_BASELABELScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LabelID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LabelCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LabelType"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Description"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LabelMode"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "TotalBind"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "LabelPath1"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "LabelPath2"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "LabelPath3"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)

        End Sub

        Public ReadOnly Property LabelID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property LabelCode As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property LabelType As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property

        Public ReadOnly Property Description As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property LabelMode As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property TotalBind As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property LabelPath1 As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property LabelPath2 As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property LabelPath3 As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property Remark As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace
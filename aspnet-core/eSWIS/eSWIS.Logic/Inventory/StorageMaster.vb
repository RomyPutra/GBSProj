Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General


Namespace Inventory
    Public NotInheritable Class StorageMaster
        Inherits Core.CoreControl
        Private StorageMasterInfo As StorageMasterInfo = New StorageMasterInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function Save(ByVal StorageMasterCont As Container.StorageMaster, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If StorageMasterCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With StorageMasterInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.LocID) & "' AND StorageID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.StorageID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.ItemCode) & "'")

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
                                .TableName = "STORAGEMASTER WITH (ROWLOCK)"
                                .AddField("OperationType", StorageMasterCont.OperationType, SQLControl.EnumDataType.dtString)
                                .AddField("SchemeNo", StorageMasterCont.SchemeNo, SQLControl.EnumDataType.dtString)
                                .AddField("ZoneNo", StorageMasterCont.ZoneNo, SQLControl.EnumDataType.dtString)
                                .AddField("AreaCode", StorageMasterCont.AreaCode, SQLControl.EnumDataType.dtString)
                                .AddField("StreetCode", StorageMasterCont.StreetCode, SQLControl.EnumDataType.dtString)
                                .AddField("StorageAreaType", StorageMasterCont.StorageAreaType, SQLControl.EnumDataType.dtString)
                                .AddField("StorageAreaCode", StorageMasterCont.StorageAreaCode, SQLControl.EnumDataType.dtString)
                                .AddField("StorageBin", StorageMasterCont.StorageBin, SQLControl.EnumDataType.dtString)
                                .AddField("Status", StorageMasterCont.Status, SQLControl.EnumDataType.dtString)
                                .AddField("CreateDate", StorageMasterCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", StorageMasterCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", StorageMasterCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", StorageMasterCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", StorageMasterCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", StorageMasterCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsHost", StorageMasterCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", StorageMasterCont.Flag, SQLControl.EnumDataType.dtNumeric)


                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.LocID) & "' AND StorageID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.StorageID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.ItemCode) & "'")

                                        Else
                                            If blnFound = False Then
                                                .AddField("LocID", StorageMasterCont.LocID, SQLControl.EnumDataType.dtString)
                                                .AddField("StorageID", StorageMasterCont.StorageID, SQLControl.EnumDataType.dtString)
                                                .AddField("ItemCode", StorageMasterCont.ItemCode, SQLControl.EnumDataType.dtString)

                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.LocID) & "' AND StorageID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.StorageID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.ItemCode) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True

                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                Else
                                    message = axExecute.Message.ToString()
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/StorageMaster", axExecute.Message & strSQL, axExecute.StackTrace)
                                Return True

                            Finally
                                objSQL.Dispose()
                            End Try
                        End If

                    End If
                End If
            Catch axAssign As ApplicationException
                message = message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/StorageMaster", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/StorageMaster", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                StorageMasterCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function Insert(ByVal StorageMasterCont As Container.StorageMaster, ByRef message As String) As Boolean
            Return Save(StorageMasterCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        Public Function Update(ByVal StorageMasterCont As Container.StorageMaster, ByRef message As String) As Boolean
            Return Save(StorageMasterCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal StorageMasterCont As Container.StorageMaster, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If StorageMasterCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With StorageMasterInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.LocID) & "' AND StorageID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.StorageID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.ItemCode) & "'")
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
                                strSQL = BuildUpdate("STORAGEMASTER WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.UpdateBy) & "' WHERE " & _
                                " LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.LocID) & "' AND StorageID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.StorageID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.ItemCode) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("STORAGEMASTER WITH (ROWLOCK)", "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.LocID) & "' AND StorageID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.StorageID) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCont.ItemCode) & "'")
                        End If

                        Try
                            'execute
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/StorageMaster", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/StorageMaster", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/StorageMaster", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                StorageMasterCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetStorageMaster(ByVal StorageMasterCode As System.String, ByVal LocID As System.String, ByVal ItemCode As System.String) As Container.StorageMaster
            Dim rStorageMaster As Container.StorageMaster = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With StorageMasterInfo.MyInfo
                        StartSQLControl()
                        strSQL = BuildSelect(.FieldsList, .TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' AND StorageID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCode) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCode) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rStorageMaster = New Container.StorageMaster
                                rStorageMaster.LocID = drRow.Item("LocID")
                                rStorageMaster.StorageID = drRow.Item("StorageID")
                                rStorageMaster.ItemCode = drRow.Item("ItemCode")
                                rStorageMaster.OperationType = drRow.Item("OperationType")
                                rStorageMaster.SchemeNo = drRow.Item("SchemeNo")
                                rStorageMaster.ZoneNo = drRow.Item("ZoneNo")
                                rStorageMaster.AreaCode = drRow.Item("AreaCode")
                                rStorageMaster.StreetCode = drRow.Item("StreetCode")
                                rStorageMaster.StorageAreaType = drRow.Item("StorageAreaType")
                                rStorageMaster.StorageAreaCode = drRow.Item("StorageAreaCode")
                                rStorageMaster.StorageBin = drRow.Item("StorageBin")
                                rStorageMaster.Status = drRow.Item("Status")
                                rStorageMaster.CreateBy = drRow.Item("CreateBy")
                                rStorageMaster.UpdateBy = drRow.Item("UpdateBy")
                                rStorageMaster.Active = drRow.Item("Active")
                                rStorageMaster.Inuse = drRow.Item("Inuse")
                                rStorageMaster.rowguid = drRow.Item("rowguid")
                                rStorageMaster.SyncCreate = drRow.Item("SyncCreate")
                                rStorageMaster.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rStorageMaster.IsHost = drRow.Item("IsHost")
                                rStorageMaster.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rStorageMaster = Nothing
                            End If
                        Else
                            rStorageMaster = Nothing
                        End If
                    End With
                End If
                Return rStorageMaster
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/StorageMaster", ex.Message, ex.StackTrace)
            Finally
                rStorageMaster = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetStorageMaster(ByVal StorageMasterCode As System.String, ByVal LocID As System.String, ByVal ItemCode As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.StorageMaster)
            Dim rStorageMaster As Container.StorageMaster = Nothing
            Dim lstStorageMaster As List(Of Container.StorageMaster) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With StorageMasterInfo.MyInfo
                        StartSQLControl()
                        If DecendingOrder Then
                            strDesc = " Order by ByVal CatgCode As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' AND StorageID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StorageMasterCode) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCode) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rStorageMaster = New Container.StorageMaster
                                rStorageMaster.LocID = drRow.Item("LocID")
                                rStorageMaster.StorageID = drRow.Item("StorageID")
                                rStorageMaster.ItemCode = drRow.Item("ItemCode")
                                rStorageMaster.OperationType = drRow.Item("OperationType")
                                rStorageMaster.SchemeNo = drRow.Item("SchemeNo")
                                rStorageMaster.ZoneNo = drRow.Item("ZoneNo")
                                rStorageMaster.AreaCode = drRow.Item("AreaCode")
                                rStorageMaster.StreetCode = drRow.Item("StreetCode")
                                rStorageMaster.StorageAreaType = drRow.Item("StorageAreaType")
                                rStorageMaster.StorageAreaCode = drRow.Item("StorageAreaCode")
                                rStorageMaster.StorageBin = drRow.Item("StorageBin")
                                rStorageMaster.Status = drRow.Item("Status")
                                rStorageMaster.CreateBy = drRow.Item("CreateBy")
                                rStorageMaster.UpdateBy = drRow.Item("UpdateBy")
                                rStorageMaster.Active = drRow.Item("Active")
                                rStorageMaster.Inuse = drRow.Item("Inuse")
                                rStorageMaster.rowguid = drRow.Item("rowguid")
                                rStorageMaster.SyncCreate = drRow.Item("SyncCreate")
                                rStorageMaster.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rStorageMaster.IsHost = drRow.Item("IsHost")
                                rStorageMaster.LastSyncBy = drRow.Item("LastSyncBy")
                            Next
                            lstStorageMaster.Add(rStorageMaster)
                        Else
                            rStorageMaster = Nothing
                        End If
                        Return lstStorageMaster
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/StorageMaster", ex.Message, ex.StackTrace)
            Finally
                rStorageMaster = Nothing
                lstStorageMaster = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetStorageMasterList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With StorageMasterInfo.MyInfo
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

        Public Overloads Function GetStorageMasterList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With StorageMasterInfo.MyInfo
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

        'amended by diana 20140913, not directly filter by Location only
        Public Overloads Function GetWasteCodeList(Optional ByVal Condition As String = "") As Data.DataTable
            If StartConnection() = True Then
                With StorageMasterInfo.MyInfo
                    strSQL = "SELECT SM.LocID, SM.ItemCode, ISNULL(I.ItmDesc,'') AS ItemDesc, SM.StorageID, SM.StorageAreaCode, SM.StorageBin " & _
                        " FROM StorageMaster SM WITH (NOLOCK) " & _
                        " LEFT JOIN Item I WITH (NOLOCK) ON I.ItemCode=SM.ItemCode " & _
                        " WHERE SM.Flag=1 "

                    If Not Condition Is Nothing And Condition <> "" Then strSQL &= Condition

                    strSQL &= " ORDER BY ItemCode"
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
        Public Class StorageMaster

            Public fLocID As System.String = "LocID"
            Public fStorageID As System.String = "StorageID"
            Public fItemCode As System.String = "ItemCode"
            Public fOperationType As System.String = "OperationType"
            Public fSchemeNo As System.String = "SchemeNo"
            Public fZoneNo As System.String = "ZoneNo"
            Public fAreaCode As System.String = "AreaCode"
            Public fStreetCode As System.String = "StreetCode"
            Public fStorageAreaType As System.String = "StorageAreaType"
            Public fStorageAreaCode As System.String = "StorageAreaCode"
            Public fStorageBin As System.String = "StorageBin"
            Public fStatus As System.String = "Status"
            Public fCreateDate As System.String = "CreateDate"
            Public fCreateBy As System.String = "CreateBy"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fUpdateBy As System.String = "UpdateBy"
            Public fActive As System.String = "Active"
            Public fInuse As System.String = "Inuse"
            Public fFlag As System.String = "Flag"
            Public frowguid As System.String = "rowguid"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fIsHost As System.String = "IsHost"
            Public fLastSyncBy As System.String = "LastSyncBy"
            Public fLocationDesc As System.String = "LocationDesc"
            Public fHandlingdesc As System.String = "HandlingDesc"

            Protected _LocID As System.String
            Private _StorageID As System.String
            Private _ItemCode As System.String
            Private _OperationType As System.String
            Private _SchemeNo As System.String
            Private _ZoneNo As System.String
            Private _AreaCode As System.String
            Private _StreetCode As System.String
            Private _StorageAreaType As System.String
            Private _StorageAreaCode As System.String
            Private _StorageBin As System.String
            Private _Status As System.Byte
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _Active As System.Byte
            Private _Inuse As System.Byte
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _IsHost As System.Byte
            Private _LastSyncBy As System.String
            Private _Flag As System.Byte
            Private _LocationDesc As System.String
            Private _HandlingDesc As System.String


            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property LocationDesc As System.String
                Get
                    Return _LocationDesc
                End Get
                Set(ByVal Value As System.String)
                    _LocationDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property HandlingDesc As System.String
                Get
                    Return _HandlingDesc
                End Get
                Set(ByVal Value As System.String)
                    _HandlingDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property StreetCode As System.String
                Get
                    Return _StreetCode
                End Get
                Set(ByVal Value As System.String)
                    _StreetCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property LocID As System.String
                Get
                    Return _LocID
                End Get
                Set(ByVal Value As System.String)
                    _LocID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property StorageID As System.String
                Get
                    Return _StorageID
                End Get
                Set(ByVal Value As System.String)
                    _StorageID = Value
                End Set
            End Property
            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ItemCode As System.String
                Get
                    Return _ItemCode
                End Get
                Set(ByVal Value As System.String)
                    _ItemCode = Value
                End Set
            End Property
            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property OperationType As System.String
                Get
                    Return _OperationType
                End Get
                Set(ByVal Value As System.String)
                    _OperationType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property SchemeNo As System.String
                Get
                    Return _SchemeNo
                End Get
                Set(ByVal Value As System.String)
                    _SchemeNo = Value
                End Set
            End Property
            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ZoneNo As System.String
                Get
                    Return _ZoneNo
                End Get
                Set(ByVal Value As System.String)
                    _ZoneNo = Value
                End Set
            End Property
            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property AreaCode As System.String
                Get
                    Return _AreaCode
                End Get
                Set(ByVal Value As System.String)
                    _AreaCode = Value
                End Set
            End Property
            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property StorageAreaType As System.String
                Get
                    Return _StorageAreaType
                End Get
                Set(ByVal Value As System.String)
                    _StorageAreaType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property StorageAreaCode As System.String
                Get
                    Return _StorageAreaCode
                End Get
                Set(ByVal Value As System.String)
                    _StorageAreaCode = Value
                End Set
            End Property
            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property StorageBin As System.String
                Get
                    Return _StorageBin
                End Get
                Set(ByVal Value As System.String)
                    _StorageBin = Value
                End Set
            End Property
            ''' <summary>
            ''' Mandatory
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
    Public Class StorageMasterInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "LocID,StorageID,ItemCode,OperationType,SchemeNo,ZoneNo,AreaCode,StreetCode,StorageAreaType,StorageAreaCode,StorageBin,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy,(SELECT CityDesc FROM City WITH (NOLOCK) WHERE CityCode=StorageMaster.AreaCode) AS LocationDesc, ISNULL((SELECT CodeDesc FROM CodeMaster WITH (NOLOCK) WHERE Code=StorageMaster.OperationType AND CodeType='STG'),'') AS HandlingDesc"
                .CheckFields = "Active,Inuse,Flag"
                .TableName = "STORAGEMASTER WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "LocID,StorageID,ItemCode,OperationType,SchemeNo,ZoneNo,AreaCode,StreetCode,StorageAreaType,StorageAreaCode,StorageBin,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy,(SELECT CityDesc FROM City WITH (NOLOCK) WHERE CityCode=StorageMaster.AreaCode) AS LocationDesc,ISNULL((SELECT CodeDesc FROM CodeMaster WITH (NOLOCK) WHERE Code=StorageMaster.OperationType AND CodeType='STG'),'') AS HandlingDesc"
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
    Public Class StorageMasterScheme
        Inherits Core.SchemeBase

        Protected Overrides Sub InitializeInfo()
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LocID"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "StorageID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ItemCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "OperationType"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "SchemeNo"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ZoneNo"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "StreetCode"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "StorageAreaType"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "StorageAreaCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "StorageBin"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "AreaCode"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
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
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)


        End Sub

        Public ReadOnly Property LocID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property StorageID As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property ItemCode As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property OperationType As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property SchemeNo As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property ZoneNo As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property AreaCode As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property StreetCode As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property StorageAreaType As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property StorageAreaCode As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property StorageBin As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
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
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace





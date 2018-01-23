
Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Actions
    Public NotInheritable Class CompDict
        Inherits Core.CoreControl
        Private CompdictInfo As CompdictInfo = New CompdictInfo
        Private Log As New SystemLog()

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub


#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal CompdictCont As Container.Compdict, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If CompdictCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With CompdictInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DictNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompdictCont.DictNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompdictCont.ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompdictCont.ComponentCode) & "'")

                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        blnFlag = False
                                    Else
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
                                .TableName = "Compdict WITH (ROWLOCK)"
                                .AddField("ComponentDesc", CompdictCont.ComponentDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("Value", CompdictCont.Value, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Unit", CompdictCont.Unit, SQLControl.EnumDataType.dtString)
                                .AddField("Status", CompdictCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", CompdictCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", CompdictCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", CompdictCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", CompdictCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", CompdictCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", CompdictCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("rowguid", CompdictCont.rowguid, SQLControl.EnumDataType.dtString)
                                .AddField("IsHost", CompdictCont.IsHost, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DictNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompdictCont.DictNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompdictCont.ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompdictCont.ComponentCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("DictNo", CompdictCont.DictNo, SQLControl.EnumDataType.dtString)
                                                .AddField("ItemCode", CompdictCont.ItemCode, SQLControl.EnumDataType.dtString)
                                                .AddField("ComponentCode", CompdictCont.ComponentCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DictNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompdictCont.DictNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompdictCont.ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompdictCont.ComponentCode) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                    Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                                Else
                                    message = axExecute.Message.ToString()
                                    Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/CompDict", axExecute.Message & strSQL, axExecute.StackTrace)
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
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/CompDict", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/CompDict", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                CompdictCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal CompdictCont As Container.Compdict, ByRef message As String) As Boolean
            Return Save(CompdictCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal CompdictCont As Container.Compdict, ByRef message As String) As Boolean
            Return Save(CompdictCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal CompdictCont As Container.Compdict, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If CompdictCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With CompdictInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DictNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompdictCont.DictNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompdictCont.ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompdictCont.ComponentCode) & "'")
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
                                strSQL = BuildUpdate("Compdict WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtDateTime, CompdictCont.LastUpdate) & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompdictCont.UpdateBy) & "' WHERE" & _
                                "DictNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompdictCont.DictNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompdictCont.ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompdictCont.ComponentCode) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("Compdict WITH (ROWLOCK)", "DictNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompdictCont.DictNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompdictCont.ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompdictCont.ComponentCode) & "'")
                        End If

                        Try
                            'execute
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Actions/CompDict", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/CompDict", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/CompDict", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                CompdictCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region

#Region "Data Selection"
        Public Overloads Function GetCompDict(ByVal DictNo As System.String, ByVal ItemCode As System.String, ByVal ComponentCode As System.String) As Container.Compdict
            Dim rCompdict As Container.Compdict = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With CompdictInfo.MyInfo
                        StartSQLControl()
                        strSQL = BuildSelect(.FieldsList, .TableName, "DictNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, DictNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ComponentCode) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rCompdict = New Container.Compdict
                                rCompdict.DictNo = drRow.Item("DictNo")
                                rCompdict.ItemCode = drRow.Item("ItemCode")
                                rCompdict.ComponentCode = drRow.Item("ComponentCode")
                                rCompdict.ComponentDesc = drRow.Item("ComponentDesc")
                                rCompdict.Value = drRow.Item("Value")
                                rCompdict.Unit = drRow.Item("Unit")
                                rCompdict.Status = drRow.Item("Status")
                                rCompdict.CreateBy = drRow.Item("CreateBy")
                                rCompdict.UpdateBy = drRow.Item("UpdateBy")
                                rCompdict.Active = drRow.Item("Active")
                                rCompdict.Inuse = drRow.Item("Inuse")
                                rCompdict.rowguid = drRow.Item("rowguid")
                                rCompdict.SyncCreate = drRow.Item("SyncCreate")
                                rCompdict.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rCompdict.IsHost = drRow.Item("IsHost")
                                rCompdict.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rCompdict = Nothing
                            End If
                        Else
                            rCompdict = Nothing
                        End If
                    End With
                End If
                Return rCompdict
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/CompDict", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rCompdict = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetCompDict(ByVal DictNo As System.String, ByVal ItemCode As System.String, ByVal ComponentCode As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Compdict)
            Dim rCompdict As Container.Compdict = Nothing
            Dim lstCompdict As List(Of Container.Compdict) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With CompdictInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal DictNo As System.String, ByVal ItemCode As System.String, ByVal ComponentCode As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "DictNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, DictNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ComponentCode) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rCompdict = New Container.Compdict
                                rCompdict.DictNo = drRow.Item("DictNo")
                                rCompdict.ItemCode = drRow.Item("ItemCode")
                                rCompdict.ComponentCode = drRow.Item("ComponentCode")
                                rCompdict.ComponentDesc = drRow.Item("ComponentDesc")
                                rCompdict.Value = drRow.Item("Value")
                                rCompdict.Unit = drRow.Item("Unit")
                                rCompdict.Status = drRow.Item("Status")
                                rCompdict.CreateBy = drRow.Item("CreateBy")
                                rCompdict.UpdateBy = drRow.Item("UpdateBy")
                                rCompdict.Active = drRow.Item("Active")
                                rCompdict.Inuse = drRow.Item("Inuse")
                                rCompdict.rowguid = drRow.Item("rowguid")
                                rCompdict.SyncCreate = drRow.Item("SyncCreate")
                                rCompdict.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rCompdict.IsHost = drRow.Item("IsHost")
                                rCompdict.LastSyncBy = drRow.Item("LastSyncBy")
                            Next
                            lstCompdict.Add(rCompdict)
                        Else
                            rCompdict = Nothing
                        End If
                        Return lstCompdict
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/CompDict", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rCompdict = Nothing
                lstCompdict = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetCompDictList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With CompdictInfo.MyInfo
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

        Public Overloads Function GetCompDictShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With CompdictInfo.MyInfo
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

        'Add
        Public Overloads Function GetCompListCustomList(Optional ByVal Condition As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With CompdictInfo.MyInfo

                    strSQL = "SELECT *,ItemCode AS ItemCode FROM NOTIFYDTL"
                    If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetCompDictListCustomList(Optional ByVal Condition As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With CompdictInfo.MyInfo

                    strSQL = "SELECT *,ItemCode AS ItemCode FROM COMPDICT"
                    If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition

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
        Public Class Compdict
            Public fDictNo As System.String = "DictNo"
            Public fItemCode As System.String = "ItemCode"
            Public fComponentCode As System.String = "ComponentCode"
            Public fComponentDesc As System.String = "ComponentDesc"
            Public fValue As System.String = "Value"
            Public fUnit As System.String = "Unit"
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

            Protected _DictNo As System.String
            Protected _ItemCode As System.String
            Protected _ComponentCode As System.String
            Private _ComponentDesc As System.String
            Private _Value As System.Decimal
            Private _Unit As System.String
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

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property DictNo As System.String
                Get
                    Return _DictNo
                End Get
                Set(ByVal Value As System.String)
                    _DictNo = Value
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
            Public Property ComponentCode As System.String
                Get
                    Return _ComponentCode
                End Get
                Set(ByVal Value As System.String)
                    _ComponentCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ComponentDesc As System.String
                Get
                    Return _ComponentDesc
                End Get
                Set(ByVal Value As System.String)
                    _ComponentDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Value As System.Decimal
                Get
                    Return _Value
                End Get
                Set(ByVal Value As System.Decimal)
                    _Value = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Unit As System.String
                Get
                    Return _Unit
                End Get
                Set(ByVal Value As System.String)
                    _Unit = Value
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

        End Class
#End Region

    End Namespace

#Region "Class Info"
    Public Class CompdictInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "DictNo,ItemCode,ComponentCode,ComponentDesc,Value,Unit,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
                .CheckFields = "Status,Active,Inuse,Flag,IsHost"
                .TableName = "Compdict WITH (NOLOCK) "
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "DictNo,ItemCode,ComponentCode,ComponentDesc,Value,Unit,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
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
    Public Class CompDictScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "DictNo"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ItemCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ComponentCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ComponentDesc"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Value"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Unit"
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
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
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
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)

        End Sub

        Public ReadOnly Property DictNo As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property ItemCode As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property ComponentCode As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property

        Public ReadOnly Property ComponentDesc As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property Value As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property Unit As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
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

End Namespace

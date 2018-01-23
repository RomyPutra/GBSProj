
Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Actions
    Public NotInheritable Class ItemComponent
        Inherits Core.CoreControl
        Private ItemcomponentInfo As ItemcomponentInfo = New ItemcomponentInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal ItemcomponentCont As Container.Itemcomponent, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ItemcomponentCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItemcomponentInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemcomponentCont.ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemcomponentCont.ComponentCode) & "'")
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
                                .TableName = "Itemcomponent WITH (ROWLOCK)"
                                .AddField("ComponentDesc", ItemcomponentCont.ComponentDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("Value", ItemcomponentCont.Value, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Unit", ItemcomponentCont.Unit, SQLControl.EnumDataType.dtString)
                                .AddField("Status", ItemcomponentCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", ItemcomponentCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", ItemcomponentCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", ItemcomponentCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", ItemcomponentCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", ItemcomponentCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", ItemcomponentCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("rowguid", ItemcomponentCont.rowguid, SQLControl.EnumDataType.dtString)
                                .AddField("IsHost", ItemcomponentCont.IsHost, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemcomponentCont.ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemcomponentCont.ComponentCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("ItemCode", ItemcomponentCont.ItemCode, SQLControl.EnumDataType.dtString)
                                                .AddField("ComponentCode", ItemcomponentCont.ComponentCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemcomponentCont.ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemcomponentCont.ComponentCode) & "'")
                                End Select
                            End With
                            Try
                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                Else
                                    message = axExecute.Message.ToString()
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemComponent", axExecute.Message & strSQL, axExecute.StackTrace)
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
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemComponent", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemComponent", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ItemcomponentCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal ItemcomponentCont As Container.Itemcomponent, ByRef message As String) As Boolean
            Return Save(ItemcomponentCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal ItemcomponentCont As Container.Itemcomponent, ByRef message As String) As Boolean
            Return Save(ItemcomponentCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal ItemcomponentCont As Container.Itemcomponent, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If ItemcomponentCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItemcomponentInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemcomponentCont.ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemcomponentCont.ComponentCode) & "'")
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
                                strSQL = BuildUpdate("Itemcomponent WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemcomponentCont.UpdateBy) & "' WHERE" & _
                                "ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemcomponentCont.ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemcomponentCont.ComponentCode) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("Itemcomponent WITH (ROWLOCK)", "ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemcomponentCont.ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemcomponentCont.ComponentCode) & "'")
                        End If

                        Try
                            'execute
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemComponent", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemComponent", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemComponent", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                ItemcomponentCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetItemComponent(ByVal ItemCode As System.String, ByVal ComponentCode As System.String) As Container.Itemcomponent
            Dim rItemcomponent As Container.Itemcomponent = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With ItemcomponentInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ComponentCode) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rItemcomponent = New Container.Itemcomponent
                                rItemcomponent.ItemCode = drRow.Item("ItemCode")
                                rItemcomponent.ComponentCode = drRow.Item("ComponentCode")
                                rItemcomponent.ComponentDesc = drRow.Item("ComponentDesc")
                                rItemcomponent.Value = drRow.Item("Value")
                                rItemcomponent.Unit = drRow.Item("Unit")
                                rItemcomponent.Status = drRow.Item("Status")
                                rItemcomponent.CreateBy = drRow.Item("CreateBy")
                                rItemcomponent.UpdateBy = drRow.Item("UpdateBy")
                                rItemcomponent.Active = drRow.Item("Active")
                                rItemcomponent.Inuse = drRow.Item("Inuse")
                                rItemcomponent.rowguid = drRow.Item("rowguid")
                                rItemcomponent.SyncCreate = drRow.Item("SyncCreate")
                                rItemcomponent.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rItemcomponent.IsHost = drRow.Item("IsHost")
                                rItemcomponent.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rItemcomponent = Nothing
                            End If
                        Else
                            rItemcomponent = Nothing
                        End If
                    End With
                End If
                Return rItemcomponent
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemComponent", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rItemcomponent = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetItemComponent(ByVal ItemCode As System.String, ByVal ComponentCode As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Itemcomponent)
            Dim rItemcomponent As Container.Itemcomponent = Nothing
            Dim lstItemcomponent As List(Of Container.Itemcomponent) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With ItemcomponentInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal ItemCode As System.String, ByVal ComponentCode As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ComponentCode) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rItemcomponent = New Container.Itemcomponent
                                rItemcomponent.ItemCode = drRow.Item("ItemCode")
                                rItemcomponent.ComponentCode = drRow.Item("ComponentCode")
                                rItemcomponent.ComponentDesc = drRow.Item("ComponentDesc")
                                rItemcomponent.Value = drRow.Item("Value")
                                rItemcomponent.Unit = drRow.Item("Unit")
                                rItemcomponent.Status = drRow.Item("Status")
                                rItemcomponent.CreateBy = drRow.Item("CreateBy")
                                rItemcomponent.UpdateBy = drRow.Item("UpdateBy")
                                rItemcomponent.Active = drRow.Item("Active")
                                rItemcomponent.Inuse = drRow.Item("Inuse")
                                rItemcomponent.rowguid = drRow.Item("rowguid")
                                rItemcomponent.SyncCreate = drRow.Item("SyncCreate")
                                rItemcomponent.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rItemcomponent.IsHost = drRow.Item("IsHost")
                                rItemcomponent.LastSyncBy = drRow.Item("LastSyncBy")
                            Next
                            lstItemcomponent.Add(rItemcomponent)
                        Else
                            rItemcomponent = Nothing
                        End If
                        Return lstItemcomponent
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemComponent", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rItemcomponent = Nothing
                lstItemcomponent = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetItemComponentList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ItemcomponentInfo.MyInfo
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

        Public Overloads Function GetItemComponentShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With ItemcomponentInfo.MyInfo
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
        Public Class Itemcomponent
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
    Public Class ItemcomponentInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "ItemCode,ComponentCode,ComponentDesc,Value,Unit,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
                .CheckFields = "Status,Active,Inuse,Flag,IsHost"
                .TableName = "Itemcomponent WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "ItemCode,ComponentCode,ComponentDesc,Value,Unit,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
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
    Public Class ItemComponentScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ItemCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ComponentCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ComponentDesc"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Value"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Unit"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)

        End Sub

        Public ReadOnly Property ItemCode As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property ComponentCode As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property

        Public ReadOnly Property ComponentDesc As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property Value As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property Unit As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region


End Namespace





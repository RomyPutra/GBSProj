Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace GeneralSettings
    Public NotInheritable Class SysPreft
        Inherits Core.CoreControl
        Private SyspreftInfo As SyspreftInfo = New SyspreftInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal SyspreftCont As Container.Syspreft, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If SyspreftCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With SyspreftInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "AppID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtCustom, SyspreftCont.AppID) & "' AND GrpID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, SyspreftCont.GrpID) & "' AND SYSKey = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, SyspreftCont.SYSKey) & "'")
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
                                .TableName = "SYSPREFT WITH (ROWLOCK)"
                                .AddField("SYSValue", SyspreftCont.SYSValue, SQLControl.EnumDataType.dtStringN)
                                .AddField("SYSValueEx", SyspreftCont.SYSValueEx, SQLControl.EnumDataType.dtStringN)
                                .AddField("SYSSet", SyspreftCont.SYSSet, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsHost", SyspreftCont.IsHost, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "AppID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtCustom, SyspreftCont.AppID) & "' AND GrpID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, SyspreftCont.GrpID) & "' AND SYSKey = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, SyspreftCont.SYSKey) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("AppID", SyspreftCont.AppID, SQLControl.EnumDataType.dtCustom)
                                                .AddField("GrpID", SyspreftCont.GrpID, SQLControl.EnumDataType.dtString)
                                                .AddField("SYSKey", SyspreftCont.SYSKey, SQLControl.EnumDataType.dtStringN)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "AppID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtCustom, SyspreftCont.AppID) & "' AND GrpID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, SyspreftCont.GrpID) & "' AND SYSKey = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, SyspreftCont.SYSKey) & "'")
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
                                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/SysPreft", axExecute.Message & strSQL, axExecute.StackTrace)
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
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/SysPreft", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/SysPreft", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                SyspreftCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal SyspreftCont As Container.Syspreft, ByRef message As String) As Boolean
            Return Save(SyspreftCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal SyspreftCont As Container.Syspreft, ByRef message As String) As Boolean
            Return Save(SyspreftCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal SyspreftCont As Container.Syspreft, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If SyspreftCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With SyspreftInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "AppID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtCustom, SyspreftCont.AppID) & "' AND GrpID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, SyspreftCont.GrpID) & "' AND SYSKey = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, SyspreftCont.SYSKey) & "'")
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
                                strSQL = BuildUpdate("SYSPREFT WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , SyncLastUpd = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , LastSyncBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, SyspreftCont.LastSyncBy) & "' WHERE " & _
                                " AppID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtCustom, SyspreftCont.AppID) & "' AND GrpID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, SyspreftCont.GrpID) & "' AND SYSKey = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, SyspreftCont.SYSKey) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("SYSPREFT WITH (ROWLOCK)", "AppID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtCustom, SyspreftCont.AppID) & "' AND GrpID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, SyspreftCont.GrpID) & "' AND SYSKey = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, SyspreftCont.SYSKey) & "'")
                        End If

                        Try
                            'execute
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/SysPreft", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                            'Throw New ApplicationException("210006 " & exExecute.Message.ToString())
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/SysPreft", axDelete.Message, axDelete.StackTrace)
                Return False
                'Throw axDelete
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/SysPreft", exDelete.Message, exDelete.StackTrace)
                Return False
                'Throw exDelete
            Finally
                SyspreftCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetSysPreft(ByVal AppID As System.Int16, ByVal GrpID As System.String, ByVal SYSKey As System.String) As Container.Syspreft
            Dim rSyspreft As Container.Syspreft = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With SyspreftInfo.MyInfo
                        StartSQLControl()
                        strSQL = BuildSelect(.FieldsList, .TableName, "AppID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, AppID) & "' AND GrpID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, GrpID) & "' AND SYSKey = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, SYSKey) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rSyspreft = New Container.Syspreft
                                rSyspreft.AppID = drRow.Item("AppID")
                                rSyspreft.GrpID = drRow.Item("GrpID")
                                rSyspreft.SYSKey = drRow.Item("SYSKey")
                                rSyspreft.SYSValue = drRow.Item("SYSValue")
                                rSyspreft.SYSValueEx = drRow.Item("SYSValueEx")
                                rSyspreft.SYSSet = drRow.Item("SYSSet")
                                rSyspreft.rowguid = drRow.Item("rowguid")
                                rSyspreft.SyncCreate = drRow.Item("SyncCreate")
                                rSyspreft.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rSyspreft.IsHost = drRow.Item("IsHost")
                                rSyspreft.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rSyspreft = Nothing
                            End If
                        Else
                            rSyspreft = Nothing
                        End If
                    End With
                End If
                Return rSyspreft
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/SysPreft", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rSyspreft = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetSysPreft(ByVal AppID As System.Int16, ByVal GrpID As System.String, ByVal SYSKey As System.String, DecendingOrder As Boolean) As List(Of Container.Syspreft)
            Dim rSyspreft As Container.Syspreft = Nothing
            Dim lstSyspreft As List(Of Container.Syspreft) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With SyspreftInfo.MyInfo
                        StartSQLControl()
                        If DecendingOrder Then
                            strDesc = " Order by ByVal AppID As System.Int16, ByVal GrpID As System.String, ByVal SYSKey As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "AppID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, AppID) & "' AND GrpID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, GrpID) & "' AND SYSKey = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, SYSKey) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rSyspreft = New Container.Syspreft
                                rSyspreft.AppID = drRow.Item("AppID")
                                rSyspreft.GrpID = drRow.Item("GrpID")
                                rSyspreft.SYSKey = drRow.Item("SYSKey")
                                rSyspreft.SYSValue = drRow.Item("SYSValue")
                                rSyspreft.SYSValueEx = drRow.Item("SYSValueEx")
                                rSyspreft.SYSSet = drRow.Item("SYSSet")
                                rSyspreft.rowguid = drRow.Item("rowguid")
                                rSyspreft.SyncCreate = drRow.Item("SyncCreate")
                                rSyspreft.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rSyspreft.IsHost = drRow.Item("IsHost")
                                rSyspreft.LastSyncBy = drRow.Item("LastSyncBy")
                            Next
                            lstSyspreft.Add(rSyspreft)
                        Else
                            rSyspreft = Nothing
                        End If
                        Return lstSyspreft
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/SysPreft", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rSyspreft = Nothing
                lstSyspreft = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetSingleSysPreft(ByVal AppID As System.Int16, ByVal GrpID As System.String, ByVal SYSKey As System.String) As String
            Dim SysValue As String = ""
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With SyspreftInfo.MyInfo
                        StartSQLControl()
                        strSQL = BuildSelect(.FieldsList, .TableName, "AppID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, AppID) & "' AND GrpID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, GrpID) & "' AND SYSKey = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, SYSKey) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                SysValue = drRow.Item("SYSValue")
                            Else
                                SysValue = ""
                            End If
                        Else
                            SysValue = ""
                        End If
                    End With
                End If
                Return SysValue
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/SysPreft", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                SysValue = ""
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region
    End Class


    Namespace Container
#Region "Class Container"
        Public Class Syspreft
            Public fAppID As System.String = "AppID"
            Public fGrpID As System.String = "GrpID"
            Public fSYSKey As System.String = "SYSKey"
            Public fSYSValue As System.String = "SYSValue"
            Public fSYSValueEx As System.String = "SYSValueEx"
            Public fSYSSet As System.String = "SYSSet"
            Public frowguid As System.String = "rowguid"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fIsHost As System.String = "IsHost"
            Public fLastSyncBy As System.String = "LastSyncBy"

            Protected _AppID As System.Int16
            Protected _GrpID As System.String
            Protected _SYSKey As System.String
            Private _SYSValue As System.String
            Private _SYSValueEx As System.String
            Private _SYSSet As System.Byte
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _IsHost As System.Byte
            Private _LastSyncBy As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property AppID As System.Int16
                Get
                    Return _AppID
                End Get
                Set(ByVal Value As System.Int16)
                    _AppID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property GrpID As System.String
                Get
                    Return _GrpID
                End Get
                Set(ByVal Value As System.String)
                    _GrpID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property SYSKey As System.String
                Get
                    Return _SYSKey
                End Get
                Set(ByVal Value As System.String)
                    _SYSKey = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SYSValue As System.String
                Get
                    Return _SYSValue
                End Get
                Set(ByVal Value As System.String)
                    _SYSValue = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SYSValueEx As System.String
                Get
                    Return _SYSValueEx
                End Get
                Set(ByVal Value As System.String)
                    _SYSValueEx = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SYSSet As System.Byte
                Get
                    Return _SYSSet
                End Get
                Set(ByVal Value As System.Byte)
                    _SYSSet = Value
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
    Public Class SyspreftInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "AppID,GrpID,SYSKey,SYSValue,SYSValueEx,SYSSet,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
                .CheckFields = "SYSSet,IsHost"
                .TableName = "SYSPREFT WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "AppID,GrpID,SYSKey,SYSValue,SYSValueEx,SYSSet,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
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
    Public Class SysPreftScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "AppID"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "GrpID"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "SYSKey"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "SYSValue"
                .Length = 255
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "SYSValueEx"
                .Length = 255
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "SYSSet"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)

        End Sub

        Public ReadOnly Property AppID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property GrpID As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property SYSKey As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property

        Public ReadOnly Property SYSValue As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property SYSValueEx As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property SYSSet As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace
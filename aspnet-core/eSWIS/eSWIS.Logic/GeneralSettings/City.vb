Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General


Namespace GeneralSettings
    Public NotInheritable Class City
        Inherits Core.CoreControl
        Private CityInfo As CityInfo = New CityInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal CityCont As Container.City, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim strSQLCountry As String = ""
            Dim strSQLState As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If CityCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With CityInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "CountryCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.CountryCode) & "' AND StateCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.StateCode) & "' AND CityCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.CityCode) & "'")
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
                                .TableName = "CITY WITH (ROWLOCK)"
                                .AddField("CityDesc", CityCont.CityDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("Status", CityCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", CityCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", CityCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", CityCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", CityCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", CityCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", CityCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsHost", CityCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", CityCont.Flag, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "CountryCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.CountryCode) & "' AND StateCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.StateCode) & "' AND CityCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.CityCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("CountryCode", CityCont.CountryCode, SQLControl.EnumDataType.dtString)
                                                .AddField("StateCode", CityCont.StateCode, SQLControl.EnumDataType.dtString)
                                                .AddField("CityCode", CityCont.CityCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        .AddField("CountryCode", CityCont.CountryCodeUpdate, SQLControl.EnumDataType.dtString)
                                        .AddField("StateCode", CityCont.StateCodeUpdate, SQLControl.EnumDataType.dtString)
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "CountryCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.CountryCode) & "' AND StateCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.StateCode) & "' AND CityCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.CityCode) & "'")

                                End Select
                            End With
                            Try
                                With objSQL
                                    .TableName = "COUNTRY WITH (ROWLOCK)"
                                    .AddField("Inuse", 1, SQLControl.EnumDataType.dtNumeric)
                                    strSQLCountry = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "CountryCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.CountryCodeUpdate) & "'")
                                    .TableName = "STATE WITH (ROWLOCK)"
                                    .AddField("Inuse", 1, SQLControl.EnumDataType.dtNumeric)
                                    strSQLState = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "StateCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.StateCodeUpdate) & "'")
                                End With

                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                objConn.Execute(strSQLCountry, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                objConn.Execute(strSQLState, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True

                            Catch axExecute As Exception
                                Dim SQLStatement As String = ""
                                If objConn.FailedSQLStatement.Count > 0 Then
                                    SQLStatement &= objConn.FailedSQLStatement.Item(0)
                                End If

                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                Else
                                    message = axExecute.Message.ToString()
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/City", axExecute.Message & SQLStatement, axExecute.StackTrace)
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
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/City", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/City", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                CityCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal CityCont As Container.City, ByRef message As String) As Boolean
            Return Save(CityCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal CityCont As Container.City, ByRef message As String) As Boolean
            Return Save(CityCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal CityCont As Container.City, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If CityCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With CityInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "CountryCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.CountryCode) & "' AND StateCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.StateCode) & "' AND CityCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.CityCode) & "'")
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
                                strSQL = BuildUpdate("CITY WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = '" & CityCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.UpdateBy) & "' WHERE" & _
                                " CountryCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.CountryCode) & "' AND StateCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.StateCode) & "' AND CityCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.CityCode) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("CITY WITH (ROWLOCK)", "CountryCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.CountryCode) & "' AND StateCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.StateCode) & "' AND CityCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCont.CityCode) & "'")
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/City", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/City", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/City", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                CityCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region

#Region "Data Selection"
        Public Overloads Function GetCity(ByVal CountryCode As System.String, ByVal StateCode As System.String, ByVal CityCode As System.String) As Container.City
            Dim rCity As Container.City = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With CityInfo.MyInfo
                        StartSQLControl()
                        strSQL = BuildSelect(.FieldsList, .TableName, "CountryCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CountryCode) & "' AND StateCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StateCode) & "' AND CityCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCode) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rCity = New Container.City
                                rCity.CountryCode = drRow.Item("CountryCode")
                                rCity.CountryDesc = drRow.Item("CountryDesc")
                                rCity.StateCode = drRow.Item("StateCode")
                                rCity.StateDesc = drRow.Item("StateDesc")
                                rCity.CityCode = drRow.Item("CityCode")
                                rCity.CityDesc = drRow.Item("CityDesc")
                                rCity.Status = drRow.Item("Status")
                                rCity.CreateBy = drRow.Item("CreateBy")
                                rCity.UpdateBy = drRow.Item("UpdateBy")
                                rCity.Active = drRow.Item("Active")
                                rCity.Inuse = drRow.Item("Inuse")
                                rCity.rowguid = drRow.Item("rowguid")
                                rCity.SyncCreate = drRow.Item("SyncCreate")
                                rCity.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rCity.IsHost = drRow.Item("IsHost")
                                rCity.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rCity = Nothing
                            End If
                        Else
                            rCity = Nothing
                        End If
                    End With
                End If
                Return rCity
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/City", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rCity = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetCity(ByVal CountryCode As System.String, ByVal StateCode As System.String, ByVal CityCode As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.City)
            Dim rCity As Container.City = Nothing
            Dim lstCity As List(Of Container.City) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With CityInfo.MyInfo
                        StartSQLControl()
                        If DecendingOrder Then
                            strDesc = " Order by ByVal CountryCode As System.String, ByVal StateCode As System.String, ByVal CityCode As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "CountryCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CountryCode) & "' AND StateCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StateCode) & "' AND CityCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CityCode) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rCity = New Container.City
                                rCity.CountryCode = drRow.Item("CountryCode")
                                rCity.CountryDesc = drRow.Item("CountryDesc")
                                rCity.StateCode = drRow.Item("StateCode")
                                rCity.StateDesc = drRow.Item("StateDesc")
                                rCity.CityCode = drRow.Item("CityCode")
                                rCity.CityDesc = drRow.Item("CityDesc")
                                rCity.Status = drRow.Item("Status")
                                rCity.CreateBy = drRow.Item("CreateBy")
                                rCity.UpdateBy = drRow.Item("UpdateBy")
                                rCity.Active = drRow.Item("Active")
                                rCity.Inuse = drRow.Item("Inuse")
                                rCity.rowguid = drRow.Item("rowguid")
                                rCity.SyncCreate = drRow.Item("SyncCreate")
                                rCity.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rCity.IsHost = drRow.Item("IsHost")
                                rCity.LastSyncBy = drRow.Item("LastSyncBy")
                            Next
                            lstCity.Add(rCity)
                        Else
                            rCity = Nothing
                        End If
                        Return lstCity
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/City", ex.Message, ex.StackTrace)
            Finally
                rCity = Nothing
                lstCity = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetCityList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With CityInfo.MyInfo
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

#End Region
    End Class


    Namespace Container
#Region "Class Container"
        Public Class City
            Public fCountryCode As System.String = "CountryCode"
            Public fCountryDesc As System.String = "CountryDesc"
            Public fStateCode As System.String = "StateCode"
            Public fStateDesc As System.String = "StateDesc"
            Public fCityCode As System.String = "CityCode"
            Public fCityDesc As System.String = "CityDesc"
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
            Public fCountryCodeUpdate As System.String = "CountryCode"
            Private fStateCodeUpdate As System.String = "StateCode"

            Protected _CountryCode As System.String
            Protected _CountryDesc As System.String
            Protected _StateCode As System.String
            Protected _StateDesc As System.String
            Protected _CityCode As System.String
            Private _CityDesc As System.String
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
            Private _Flag As System.String
            Private _CountryCodeUpdate As System.String
            Private _StateCodeUpdate As System.String


            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property CountryCode As System.String
                Get
                    Return _CountryCode
                End Get
                Set(ByVal Value As System.String)
                    _CountryCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property CountryDesc As System.String
                Get
                    Return _CountryDesc
                End Get
                Set(ByVal Value As System.String)
                    _CountryDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property CountryCodeUpdate As System.String
                Get
                    Return _CountryCodeUpdate
                End Get
                Set(ByVal Value As System.String)
                    _CountryCodeUpdate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property StateCode As System.String
                Get
                    Return _StateCode
                End Get
                Set(ByVal Value As System.String)
                    _StateCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property StateDesc As System.String
                Get
                    Return _StateDesc
                End Get
                Set(ByVal Value As System.String)
                    _StateDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property StateCodeUpdate As System.String
                Get
                    Return _StateCodeUpdate
                End Get
                Set(ByVal Value As System.String)
                    _StateCodeUpdate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property CityCode As System.String
                Get
                    Return _CityCode
                End Get
                Set(ByVal Value As System.String)
                    _CityCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CityDesc As System.String
                Get
                    Return _CityDesc
                End Get
                Set(ByVal Value As System.String)
                    _CityDesc = Value
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

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property Flag As System.String
                Get
                    Return _Flag
                End Get
                Set(ByVal Value As System.String)
                    _Flag = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class CityInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "CountryCode, (SELECT CountryDesc FROM COUNTRY WITH (NOLOCK) WHERE CountryCode=City.CountryCode) AS CountryDesc, StateCode, (SELECT StateDesc FROM STATE WITH (NOLOCK) WHERE CountryCode=City.CountryCode AND StateCode=City.StateCode) AS StateDesc, CityCode,CityDesc,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
                .CheckFields = "Status,Active,Inuse,Flag,IsHost"
                .TableName = "CITY WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "CountryCode, CountryDesc, StateCode, StateDesc, CityCode,CityDesc,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
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
    Public Class CityScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CountryCode"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "StateCode"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CityCode"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "CityDesc"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)

        End Sub

        Public ReadOnly Property CountryCode As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property StateCode As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property CityCode As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property

        Public ReadOnly Property CityDesc As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace
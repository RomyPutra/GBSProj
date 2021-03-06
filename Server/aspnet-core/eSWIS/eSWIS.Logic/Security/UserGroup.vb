﻿Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace UserSecurity

    ''' <summary>
    ''' store data access of save, edit delete
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class UserGroup
        Inherits Core.CoreControl
        Private UserGroupInfo As UserGroupInfo = New UserGroupInfo
        Private Log As New SystemLog()


        Public Structure SearchData
            Public AppID As String
            Public GroupCode As String
            Public GroupName As String
            Public AccessLevel As String
            Public Status As String
        End Structure

        'Private objCommonFunction As New CommonFunction

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal UsrgroupCont As Container.UserGroup, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If UsrgroupCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With UserGroupInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "APPID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, UsrgroupCont.APPID) & "' AND GroupCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrgroupCont.GroupCode) & "'")
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
                                .TableName = "USRGROUP WITH (ROWLOCK)"
                                .AddField("GroupName", UsrgroupCont.GroupName, SQLControl.EnumDataType.dtString)
                                .AddField("AccessLevel", UsrgroupCont.AccessLevel, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Status", UsrgroupCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", UsrgroupCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", UsrgroupCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", UsrgroupCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", UsrgroupCont.UpdateBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "APPID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, UsrgroupCont.APPID) & "' AND GroupCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrgroupCont.GroupCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("AppID", UsrgroupCont.APPID, SQLControl.EnumDataType.dtNumeric)
                                                .AddField("GroupCode", UsrgroupCont.GroupCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        .AddField("AppID", UsrgroupCont.APPID, SQLControl.EnumDataType.dtNumeric)
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "GroupCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrgroupCont.GroupCode) & "'")
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
                                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserGroup", axExecute.Message & strSQL, axExecute.StackTrace)
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
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserGroup", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserGroup", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                UsrgroupCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal UsrgroupCont As Container.UserGroup, ByRef message As String) As Boolean
            Return Save(UsrgroupCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal UsrgroupCont As Container.UserGroup, ByRef message As String) As Boolean
            Return Save(UsrgroupCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal UsrgroupCont As Container.UserGroup, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If UsrgroupCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With UserGroupInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "GroupCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrgroupCont.GroupCode) & "'")
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
                                strSQL = BuildUpdate("USRGROUP WITH (ROWLOCK)", " SET Status = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrgroupCont.UpdateBy) & "' WHERE" & _
                                " GroupCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrgroupCont.GroupCode) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("USRGROUP WITH (ROWLOCK)", "GroupCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UsrgroupCont.GroupCode) & "'")
                        End If

                        Try
                            'execute
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserGroup", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                            'Throw New ApplicationException("210006 " & exExecute.Message.ToString())
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserGroup", axDelete.Message, axDelete.StackTrace)
                Return False
                'Throw axDelete
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserGroup", exDelete.Message, exDelete.StackTrace)
                Return False
                'Throw exDelete
            Finally
                UsrgroupCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        'Enquiry
        Public Function Enquiry(ByVal strSQLQuery As String) As DataTable
            Dim dtTemp As DataTable = Nothing
            Dim strCond As String = String.Empty

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
            EndConnection()
        End Function

        Public Overloads Function GetUserGroup(ByVal GroupCode As System.String) As Container.UserGroup
            Dim rUserGroup As Container.UserGroup = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With UserGroupInfo.MyInfo
                        strSQL = "SELECT APPID, GroupCode, GroupName, AccessLevel, Status, " & _
                            "CreateDate, CreateBy, LastUpdate, UpdateBy, SyncCreate, SyncLastUpd " & _
                            "FROM USRGROUP WITH (NOLOCK) WHERE GroupCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, GroupCode) & "'"
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rUserGroup = New Container.UserGroup
                                rUserGroup.GroupCode = drRow.Item("GroupCode")
                                rUserGroup.GroupName = drRow.Item("GroupName")
                                rUserGroup.CreateDate = If(drRow.Item("CreateDate") IsNot DBNull.Value, drRow.Item("CreateDate"), Nothing)
                                rUserGroup.CreateBy = If(drRow.Item("CreateBy") IsNot DBNull.Value, drRow.Item("CreateBy"), Nothing)
                                rUserGroup.LastUpdate = If(drRow.Item("LastUpdate") IsNot DBNull.Value, drRow.Item("LastUpdate"), Nothing)
                                rUserGroup.UpdateBy = If(drRow.Item("UpdateBy") IsNot DBNull.Value, drRow.Item("UpdateBy"), Nothing)
                                rUserGroup.Status = drRow.Item("Status")
                                rUserGroup.AccessLevel = drRow.Item("AccessLevel")
                                rUserGroup.APPID = drRow.Item("AppID")

                                'rUserGroup.StatusDesc = drRow.Item("StatusDesc")
                                'rUserGroup.Rowguid = drRow.Item("rowguid")
                                rUserGroup.SyncCreate = If(drRow.Item("SyncCreate") IsNot DBNull.Value, drRow.Item("SyncCreate"), Nothing)
                                rUserGroup.SyncLastUpd = If(drRow.Item("SyncLastUpd") IsNot DBNull.Value, drRow.Item("SyncLastUpd"), Nothing)
                            Else
                                rUserGroup = Nothing
                            End If
                        Else
                            rUserGroup = Nothing
                        End If
                    End With
                End If
                Return rUserGroup
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserGroup", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rUserGroup = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetUserGroup(ByVal SearchData As SearchData) As DataTable
            Try
                StartConnection()
                StartSQLControl()

                Dim dtTemp As DataTable = Nothing
                Dim strCond As String = String.Empty

                With SearchData

                    strSQL = "SELECT APPID, GroupCode, GroupName, AccessLevel, Status, c.CodeDesc AS STATUSDESC " &
                             " FROM USRGROUP WITH (NOLOCK) " &
                             " LEFT JOIN CODEMASTER c WITH (NOLOCK) ON c.CodeType = 'USR' AND c.Code = USRGROUP.Status " &
                             " WHERE APPID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, .AppID) & "'"

                    If .GroupCode <> String.Empty Then
                        strCond = String.Concat(" AND GroupCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, .GroupCode) & "' ")
                    End If

                    If .GroupName <> String.Empty Then
                        strCond = String.Concat(" AND GroupName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, .GroupName) & "' ")
                    End If

                    If .Status <> String.Empty Then
                        strCond = String.Concat(" AND Status = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, .Status) & "'")
                    End If

                    If strCond <> String.Empty Then
                        strSQL = String.Concat(strSQL, strCond)
                    End If

                End With

                If strSQL <> String.Empty Then
                    dtTemp = objConn.Execute(strSQL, CommandType.Text)
                End If

                If dtTemp Is Nothing = False Then
                    Return dtTemp
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserGroup", ex.Message, ex.StackTrace)
                'Throw New ApplicationException(ex.Message)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetUserGroupList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With UserGroupInfo.MyInfo
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

        Public Overloads Function GetUserGroupByUserId(ByVal UserID) As Container.UserGroup
            Dim rUserGroup As Container.UserGroup = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With UserGroupInfo.MyInfo
                        strSQL = "Select * from USRGROUP g WITH (NOLOCK) " &
                                 " INNER JOIN USRAPP a WITH (NOLOCK) ON g.APPID = a.AppID AND g.GroupCode = a.AccessCode" &
                                 " WHERE UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UserID) & "'"
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rUserGroup = New Container.UserGroup
                                rUserGroup.GroupCode = drRow.Item("GroupCode")
                                rUserGroup.GroupName = drRow.Item("GroupName")
                                rUserGroup.CreateDate = If(drRow.Item("CreateDate") IsNot DBNull.Value, drRow.Item("CreateDate"), Nothing)
                                rUserGroup.CreateBy = If(drRow.Item("CreateBy") IsNot DBNull.Value, drRow.Item("CreateBy"), Nothing)
                                rUserGroup.LastUpdate = If(drRow.Item("LastUpdate") IsNot DBNull.Value, drRow.Item("LastUpdate"), Nothing)
                                rUserGroup.UpdateBy = If(drRow.Item("UpdateBy") IsNot DBNull.Value, drRow.Item("UpdateBy"), Nothing)
                                rUserGroup.Status = drRow.Item("Status")
                                rUserGroup.AccessLevel = drRow.Item("AccessLevel")
                                rUserGroup.APPID = drRow.Item("AppID")
                                rUserGroup.SyncCreate = If(drRow.Item("SyncCreate") IsNot DBNull.Value, drRow.Item("SyncCreate"), Nothing)
                                rUserGroup.SyncLastUpd = If(drRow.Item("SyncLastUpd") IsNot DBNull.Value, drRow.Item("SyncLastUpd"), Nothing)
                            Else
                                rUserGroup = Nothing
                            End If
                        Else
                            rUserGroup = Nothing
                        End If
                    End With
                End If
                Return rUserGroup
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserGroup", ex.Message, ex.StackTrace)
            Finally
                rUserGroup = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function IsExist(ByVal GroupCode As String) As Boolean
            Dim dtTemp As DataTable
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    strSQL = "SELECT GroupCode FROM USRGROUP WITH (NOLOCK) WHERE APPID = 1 AND GroupCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, GroupCode) & "'"

                    dtTemp = objConn.Execute(strSQL, CommandType.Text)

                    If dtTemp Is Nothing = False Then
                        If dtTemp.Rows.Count > 0 Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserGroup", ex.Message, ex.StackTrace)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

    End Class

    Namespace Container
        ''' <summary>
        ''' store object
        ''' </summary>
        ''' <remarks></remarks>
#Region "Class Container"

        Public Class UserGroup

#Region "StaticFieldName"
            Public fAppID As System.String = "AppID"
            Public fGroupCode As System.String = "GroupCode"
            Public fGroupName As System.String = "GroupName"
            Public fAccessLevel As System.String = "AccessLevel"
            Public fStatus As System.String = "Status"
            Public fStatusDesc As System.String = "StatusDesc"
            Public fCreateby As System.String = "CreateBy"
            Public fCreateDate As System.String = "CreateDate"
            Public fUpdateBy As System.String = "UpdateBy"
            Public fLastUpdate As System.String = "LastUpdate"

#End Region

#Region "Protected Members"

            Private _aPPID As System.Int32
            Private _groupCode As System.String
            Private _groupName As System.String
            Private _accessLevel As System.Byte
            Private _status As System.Byte
            Private _statusDesc As System.String
            Private _createDate As System.DateTime
            Private _createBy As System.String
            Private _lastUpdate As System.DateTime
            Private _updateBy As System.String
            Private _rowguid As System.Guid
            Private _syncCreate As System.DateTime
            Private _syncLastUpd As System.DateTime

#End Region

#Region "Public Properties"
            Public Property APPID() As System.Int32
                Get
                    Return _aPPID
                End Get
                Set(ByVal value As System.Int32)
                    _aPPID = value
                End Set
            End Property
            Public Property GroupCode() As System.String
                Get
                    Return _groupCode
                End Get
                Set(ByVal value As System.String)
                    _groupCode = value
                End Set
            End Property
            Public Property GroupName() As System.String
                Get
                    Return _groupName
                End Get
                Set(ByVal value As System.String)
                    _groupName = value
                End Set
            End Property
            Public Property AccessLevel() As System.Byte
                Get
                    Return _accessLevel
                End Get
                Set(ByVal value As System.Byte)
                    _accessLevel = value
                End Set
            End Property
            Public Property Status() As System.Byte
                Get
                    Return _status
                End Get
                Set(ByVal value As System.Byte)
                    _status = value
                End Set
            End Property
            Public Property StatusDesc() As System.String
                Get
                    Return _statusDesc
                End Get
                Set(ByVal value As System.String)
                    _statusDesc = value
                End Set
            End Property
            Public Property CreateDate() As System.DateTime
                Get
                    Return _createDate
                End Get
                Set(ByVal value As System.DateTime)
                    _createDate = value
                End Set
            End Property
            Public Property CreateBy() As System.String
                Get
                    Return _createBy
                End Get
                Set(ByVal value As System.String)
                    _createBy = value
                End Set
            End Property
            Public Property LastUpdate() As System.DateTime
                Get
                    Return _lastUpdate
                End Get
                Set(ByVal value As System.DateTime)
                    _lastUpdate = value
                End Set
            End Property
            Public Property UpdateBy() As System.String
                Get
                    Return _updateBy
                End Get
                Set(ByVal value As System.String)
                    _updateBy = value
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
#End Region
        End Class
#End Region
    End Namespace


#Region "Class Info"
    Public Class UserGroupInfo
        Inherits Core.CoreBase

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "AppID,GroupCode,GroupName,AccessLevel,Status,Status AS StatusDesc,CreateDate,CreateBy,LastUpdate,UpdateBy,SyncCreate,SyncLastUpd"
                .CheckFields = "Status"
                .TableName = "USRGROUP WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "AppID,GroupCode,GroupName,AccessLevel,Status,StatusDesc, CreateDate,CreateBy,LastUpdate,UpdateBy,SyncCreate,SyncLastUpd"
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

    ''' <summary>
    ''' store field scheme
    ''' </summary>
    ''' <remarks></remarks>
#Region "Scheme"
    Public Class UserGroupScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "AppID"
                .Length = 11
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "GroupCode"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "GroupName"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "AccessLevel"
                .Length = 1
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
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)

        End Sub

        Public ReadOnly Property AppID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property GroupCode As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property GroupName As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property AccessLevel As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
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
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
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

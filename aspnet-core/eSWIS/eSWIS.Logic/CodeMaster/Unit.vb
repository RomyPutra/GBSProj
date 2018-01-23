﻿Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace CodeMaster
    Public NotInheritable Class Unit
        Inherits Core.CoreControl
        Private UnitInfo As UnitInfo = New UnitInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function Save(ByVal UnitCont As Container.Unit, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If UnitCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With UnitInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "UOMCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.UOMCode) & "'")
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
                                .TableName = "UOM WITH (ROWLOCK)"
                                .AddField("UOMDesc", UnitCont.UOMDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("UOMGroup", UnitCont.UOMGroup, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Measure", UnitCont.Measure, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", UnitCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastUpdate", UnitCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", UnitCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", UnitCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", UnitCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", UnitCont.Flag, SQLControl.EnumDataType.dtNumeric)
                                .AddField("SyncLastUpd", UnitCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("IsHost", UnitCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastSyncBy", UnitCont.LastSyncBy, SQLControl.EnumDataType.dtString)


                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "UOMCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.UOMCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("UOMCode", UnitCont.UOMCode, SQLControl.EnumDataType.dtString)
                                                .AddField("CreateBy", UnitCont.CreateBy, SQLControl.EnumDataType.dtString)

                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "UOMCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.UOMCode) & "'")
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
                                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/Unit", axExecute.Message & strSQL, axExecute.StackTrace)
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
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/Unit", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/Unit", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                UnitCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function Insert(ByVal UnitCont As Container.Unit, ByRef message As String) As Boolean
            Return Save(UnitCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        Public Function Update(ByVal UnitCont As Container.Unit, ByRef message As String) As Boolean
            Return Save(UnitCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal UnitCont As Container.Unit, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If UnitCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With UnitInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "UOMCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.UOMCode) & "'")
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
                                strSQL = BuildUpdate("UOM WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.UpdateBy) & "'" & _
                                " Where UOMCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.UOMCode) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("UOM WITH (ROWLOCK)", "UOMCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.UOMCode) & "'")
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/Unit", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False

                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/Unit", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/Unit", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                UnitCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region


#Region "Data Selection"
        Public Overloads Function GetUnit(ByVal UOMCode As System.String) As Container.Unit
            Dim rUnit As Container.Unit = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With UnitInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "UOMNumber = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, UOMCode) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rUnit = New Container.Unit
                                rUnit.UOMCode = drRow.Item("UOMCode")
                                rUnit.UOMDesc = drRow.Item("UOMDesc")
                                rUnit.UOMNumber = drRow.Item("UOMNumber")
                                rUnit.UOMGroup = drRow.Item("UOMGroup")
                                rUnit.Measure = drRow.Item("Measure")
                                rUnit.CreateDate = drRow.Item("CreateDate")
                                rUnit.LastUpdate = IIf(IsDBNull(drRow.Item("LastUpdate")), Now, drRow.Item("LastUpdate"))
                                rUnit.UpdateBy = drRow.Item("Updateby")
                                rUnit.Active = drRow.Item("Active")
                                rUnit.Inuse = drRow.Item("Inuse")
                                rUnit.Flag = drRow.Item("Flag")
                                rUnit.rowguid = drRow.Item("rowguid")
                                rUnit.SyncCreate = IIf(IsDBNull(drRow.Item("SyncCreate")), Now, drRow.Item("SyncCreate"))
                                rUnit.SyncLastUpd = IIf(IsDBNull(drRow.Item("SyncLastUpd")), Now, drRow.Item("SyncLastUpd"))
                                rUnit.IsHost = drRow.Item("IsHost")
                                rUnit.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rUnit = Nothing
                            End If
                        Else
                            rUnit = Nothing
                        End If
                    End With
                End If
                Return rUnit
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/Unit", ex.Message, ex.StackTrace)
            Finally
                rUnit = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetUnit() As List(Of Container.Unit)
            Dim rUnit As Container.Unit = Nothing
            Dim listUnit As List(Of Container.Unit) = Nothing
            Dim dtTemp As DataTable = Nothing
            If StartConnection() = True Then
                StartSQLControl()
                Try
                    With UnitInfo.MyInfo
                        strSQL = "SELECT UOMCode, UOMDesc, UOMNumber FROM UOM WHERE UOMGroup = 1"
                        dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
                        If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
                            listUnit = New List(Of Container.Unit)
                            For Each row As DataRow In dtTemp.Rows
                                rUnit = New Container.Unit
                                With rUnit
                                    rUnit = New Container.Unit
                                    rUnit.UOMCode = row.Item("UOMCode")
                                    rUnit.UOMDesc = row.Item("UOMDesc")
                                    rUnit.UOMNumber = row.Item("UOMNumber")
                                End With
                                listUnit.Add(rUnit)
                            Next
                        End If
                    End With
                Catch ex As Exception
                    Log.Notifier.Notify(ex)
                    Gibraltar.Agent.Log.Error("ISWIS_API/UOMManager", ex.Message & " " & strSQL, ex.StackTrace)
                Finally
                    EndSQLControl()
                End Try
            End If
            EndConnection()
            Return listUnit
        End Function

        Public Overloads Function GetUnit(ByVal UOMCode As System.String, DecendingOrder As Boolean) As List(Of Container.Unit)
            Dim rUnit As Container.Unit = Nothing
            Dim lstState As List(Of Container.Unit) = New List(Of Container.Unit)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With UnitInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal UOMCode As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "UOMCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, UOMCode) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rUnit = New Container.Unit
                                rUnit.UOMCode = drRow.Item("UOMCode")
                                rUnit.UOMDesc = drRow.Item("UOMDesc")
                                rUnit.UOMGroup = drRow.Item("UOMGroup")
                                rUnit.Measure = drRow.Item("Measure")
                                rUnit.CreateDate = drRow.Item("CreateDate")
                                rUnit.LastUpdate = IIf(IsDBNull(drRow.Item("LastUpdate")), Now, drRow.Item("LastUpdate"))
                                rUnit.UpdateBy = drRow.Item("Updateby")
                                rUnit.Active = drRow.Item("Active")
                                rUnit.Inuse = drRow.Item("Inuse")
                                rUnit.Flag = drRow.Item("Flag")
                                rUnit.rowguid = drRow.Item("rowguid")
                                rUnit.SyncCreate = drRow.Item("SyncCreate")
                                rUnit.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rUnit.IsHost = drRow.Item("IsHost")
                                rUnit.LastSyncBy = drRow.Item("LastSyncBy")
                            Next
                            lstState.Add(rUnit)
                        Else
                            rUnit = Nothing
                        End If
                        Return lstState
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/Unit", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rUnit = Nothing
                lstState = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetUnitList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With UnitInfo.MyInfo
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

        Public Overloads Function GetStateShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With UnitInfo.MyInfo
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

        Public Overloads Function GetUnitByUOMNumber(ByVal UOMNumber As System.String) As Container.Unit
            Dim rUnit As Container.Unit = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With UnitInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "UOMNumber = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, UOMNumber) & "' AND UOMGroup=1")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rUnit = New Container.Unit
                                rUnit.UOMCode = drRow.Item("UOMCode")
                                rUnit.UOMDesc = drRow.Item("UOMDesc")
                                rUnit.UOMNumber = drRow.Item("UOMNumber")
                                rUnit.UOMGroup = drRow.Item("UOMGroup")
                                rUnit.Measure = drRow.Item("Measure")
                                rUnit.CreateDate = IIf(IsDBNull(drRow.Item("CreateDate")), Now, drRow.Item("CreateDate"))
                                rUnit.LastUpdate = IIf(IsDBNull(drRow.Item("LastUpdate")), Now, drRow.Item("LastUpdate"))
                                rUnit.UpdateBy = drRow.Item("Updateby")
                                rUnit.Active = drRow.Item("Active")
                                rUnit.Inuse = drRow.Item("Inuse")
                                rUnit.Flag = drRow.Item("Flag")
                                rUnit.rowguid = drRow.Item("rowguid")
                                rUnit.SyncCreate = IIf(IsDBNull(drRow.Item("SyncCreate")), Now, drRow.Item("SyncCreate"))
                                rUnit.SyncLastUpd = IIf(IsDBNull(drRow.Item("SyncLastUpd")), Now, drRow.Item("SyncLastUpd"))
                                rUnit.IsHost = drRow.Item("IsHost")
                                rUnit.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rUnit = Nothing
                            End If
                        Else
                            rUnit = Nothing
                        End If
                    End With
                End If
                Return rUnit
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/CodeMaster/Unit", ex.Message, ex.StackTrace)
            Finally
                rUnit = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region

    End Class

    Namespace Container
#Region "Class Container"
        Public Class Unit
            Public fUOMCode As System.String = "UOMCode"
            Public fUOMDesc As System.String = "UOMDesc"
            Public fUOMNumber As System.String = "UOMNumber"
            Public fUOMGroup As System.String = "UOMGroup"
            Public fMeasure As System.String = "Measure"
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

            Protected _UOMCode As System.String
            Protected _UOMDesc As System.String
            Protected _UOMNumber As System.String
            Protected _UOMGroup As System.Byte
            Protected _Measure As System.Byte
            Protected _CreateDate As System.DateTime
            Protected _CreateBy As System.String
            Protected _LastUpdate As System.DateTime
            Protected _UpdateBy As System.String
            Protected _Active As System.Byte
            Protected _Inuse As System.Byte
            Protected _Flag As System.Byte
            Protected _rowguid As System.Guid
            Protected _SyncCreate As System.DateTime
            Protected _SyncLastUpd As System.DateTime
            Protected _IsHost As System.Byte
            Protected _LastSyncBy As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property UOMCode As System.String
                Get
                    Return _UOMCode
                End Get
                Set(ByVal Value As System.String)
                    _UOMCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property UOMDesc As System.String
                Get
                    Return _UOMDesc
                End Get
                Set(ByVal Value As System.String)
                    _UOMDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property UOMNumber As System.String
                Get
                    Return _UOMNumber
                End Get
                Set(ByVal Value As System.String)
                    _UOMNumber = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property UOMGroup As System.Byte
                Get
                    Return _UOMGroup
                End Get
                Set(ByVal Value As System.Byte)
                    _UOMGroup = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property Measure As System.Byte
                Get
                    Return _Measure
                End Get
                Set(ByVal Value As System.Byte)
                    _Measure = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
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
            ''' Mandatory
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
            ''' Mandatory
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
            ''' Mandatory
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

            ''' <summary>
            ''' Mandatory
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
            ''' Mandatory
            ''' </summary>
            Public Property Flag As System.Byte
                Get
                    Return _Flag
                End Get
                Set(ByVal Value As System.Byte)
                    _Flag = Value
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
        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class UnitInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "UOMCode, UOMDesc, UOMNumber, UOMGroup, Measure, CreateDate, CreateBy, LastUpdate, UpdateBy, Active, Inuse, Flag, rowguid, SyncCreate, SyncLastUpd, IsHost, LastSyncBy"
                .CheckFields = "Active,Inuse,Flag,IsHost"
                .TableName = "UOM WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "UOMCode, UOMDesc, UOMGroup, Measure, CreateDate, CreateBy, LastUpdate, UpdateBy, Active, Inuse, Flag, rowguid, SyncCreate, SyncLastUpd, IsHost, LastSyncBy"
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
    Public Class UnitScheme
        Inherits Core.SchemeBase

        Protected Overrides Sub InitializeInfo()
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UOMCode"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UOMDesc"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "UOMGroup"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Measure"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)

            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)

            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)

            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)

            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
        End Sub

        Public ReadOnly Property UOMCode As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property UOMDesc As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property

        Public ReadOnly Property UOMGroup As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property

        Public ReadOnly Property Measure As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property

        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property

        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property

        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property

        Public ReadOnly Property UpdateBy As StrucElement
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

        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property

        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property

        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property

        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
End Namespace


Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Profiles
    Public NotInheritable Class EmployeeBranch
        Inherits Core.CoreControl
        Private EmpbranchInfo As EmpbranchInfo = New EmpbranchInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal EmpbranchCont As Container.Empbranch, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If EmpbranchCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With EmpbranchInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmpbranchCont.EmployeeID) & "' AND LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmpbranchCont.LocID) & "'")
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
                                .TableName = "EMPBRANCH WITH (ROWLOCK)"
                                .AddField("InAppt", EmpbranchCont.InAppt, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsServer", EmpbranchCont.IsServer, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsHost", EmpbranchCont.IsHost, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmpbranchCont.EmployeeID) & "' AND LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmpbranchCont.LocID) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("EmployeeID", EmpbranchCont.EmployeeID, SQLControl.EnumDataType.dtString)
                                                .AddField("LocID", EmpbranchCont.LocID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmpbranchCont.EmployeeID) & "' AND LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmpbranchCont.LocID) & "'")
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
                                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/EmployeeBranch", axExecute.Message & strSQL, axExecute.StackTrace)
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
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/EmployeeBranch", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/EmployeeBranch", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                EmpbranchCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function BatchSave(ByVal ListContEmployeeBranch As List(Of Profiles.Container.Empbranch), ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()

            BatchSave = False
            Try
                If ListContEmployeeBranch Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()

                        If ListContEmployeeBranch.Count > 0 Then
                            strSQL = BuildDelete("EMPBRANCH WITH (ROWLOCK)", "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ListContEmployeeBranch(0).EmployeeID) & "'")
                            ListSQL.Add(strSQL)
                        End If

                    End If

                    For Each EmployeeBranchCont In ListContEmployeeBranch
                        StartSQLControl()
                        With objSQL
                            .TableName = "EMPBRANCH WITH (ROWLOCK)"
                            .AddField("InAppt", EmployeeBranchCont.InAppt, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsServer", EmployeeBranchCont.IsServer, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsHost", EmployeeBranchCont.IsHost, SQLControl.EnumDataType.dtNumeric)

                            Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    .AddField("EmployeeID", EmployeeBranchCont.EmployeeID, SQLControl.EnumDataType.dtString)
                                    .AddField("LocID", EmployeeBranchCont.LocID, SQLControl.EnumDataType.dtString)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                Case SQLControl.EnumSQLType.stUpdate
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeBranchCont.EmployeeID) & "' AND LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeBranchCont.LocID) & "'")
                            End Select
                        End With

                        ListSQL.Add(strSQL)
                    Next

                    Try
                        objConn.BatchExecute(ListSQL, CommandType.Text)
                    Catch axExecute As Exception
                        Dim sqlStatement As String = " "
                        If objConn.FailedSQLStatement.Count > 0 Then
                            sqlStatement &= objConn.FailedSQLStatement.Item(0)
                        End If

                        If pType = SQLControl.EnumSQLType.stInsert Then
                            message = axExecute.Message.ToString()
                        Else
                            message = axExecute.Message.ToString()
                        End If
                        Log.Notifier.Notify(axExecute)
                        Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/EmployeeBranch", axExecute.Message & sqlStatement, axExecute.StackTrace)
                    Finally
                        objSQL.Dispose()
                    End Try
                    Return True

                End If
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/EmployeeBranch", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/EmployeeBranch", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ListContEmployeeBranch = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal EmpbranchCont As Container.Empbranch, ByRef message As String) As Boolean
            Return Save(EmpbranchCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal EmpbranchCont As Container.Empbranch, ByRef message As String) As Boolean
            Return Save(EmpbranchCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        'BATCH ADD
        Public Function BatchInsert(ByVal ListContEmployeeBranch As List(Of Profiles.Container.Empbranch), ByRef message As String) As Boolean
            Return BatchSave(ListContEmployeeBranch, SQLControl.EnumSQLType.stInsert, message)
        End Function

        Public Function Delete(ByVal EmpbranchCont As Container.Empbranch, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If EmpbranchCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With EmpbranchInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmpbranchCont.EmployeeID) & "' AND LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmpbranchCont.LocID) & "'")
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


                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("EMPBRANCH WITH (ROWLOCK)", "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmpbranchCont.EmployeeID) & "' AND LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmpbranchCont.LocID) & "'")
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/EmployeeBranch", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/EmployeeBranch", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/EmployeeBranch", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                EmpbranchCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"

        Public Overloads Function GetAssignLocation(Optional ByVal Condition As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With EmpbranchInfo.MyInfo

                    strSQL = "SELECT distinct BranchName  "

                    If Not Condition Is Nothing And Condition <> "" Then strSQL &= Condition

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetEmployeeBranch(ByVal EmployeeID As System.String, ByVal LocID As System.String) As Container.Empbranch
            Dim rEmpbranch As Container.Empbranch = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With EmpbranchInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeID) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rEmpbranch = New Container.Empbranch
                                rEmpbranch.EmployeeID = drRow.Item("EmployeeID")
                                rEmpbranch.LocID = drRow.Item("LocID")
                                rEmpbranch.InAppt = drRow.Item("InAppt")
                                rEmpbranch.IsServer = drRow.Item("IsServer")
                                rEmpbranch.rowguid = drRow.Item("rowguid")
                                rEmpbranch.SyncCreate = drRow.Item("SyncCreate")
                                rEmpbranch.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rEmpbranch.IsHost = drRow.Item("IsHost")
                                rEmpbranch.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rEmpbranch = Nothing
                            End If
                        Else
                            rEmpbranch = Nothing
                        End If
                    End With
                End If
                Return rEmpbranch
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/EmployeeBranch", ex.Message, ex.StackTrace)
            Finally
                rEmpbranch = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetEmployeeBranch(ByVal EmployeeID As System.String, ByVal LocID As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Empbranch)
            Dim rEmpbranch As Container.Empbranch = Nothing
            Dim lstEmpbranch As List(Of Container.Empbranch) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With EmpbranchInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal EmployeeID As System.String, ByVal LocID As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeID) & "' AND LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rEmpbranch = New Container.Empbranch
                                rEmpbranch.EmployeeID = drRow.Item("EmployeeID")
                                rEmpbranch.LocID = drRow.Item("LocID")
                                rEmpbranch.InAppt = drRow.Item("InAppt")
                                rEmpbranch.IsServer = drRow.Item("IsServer")
                                rEmpbranch.rowguid = drRow.Item("rowguid")
                                rEmpbranch.SyncCreate = drRow.Item("SyncCreate")
                                rEmpbranch.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rEmpbranch.IsHost = drRow.Item("IsHost")
                                rEmpbranch.LastSyncBy = drRow.Item("LastSyncBy")
                            Next
                            lstEmpbranch.Add(rEmpbranch)
                        Else
                            rEmpbranch = Nothing
                        End If
                        Return lstEmpbranch
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/EmployeeBranch", ex.Message, ex.StackTrace)
            Finally
                rEmpbranch = Nothing
                lstEmpbranch = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        
#End Region
    End Class


    Namespace Container
#Region "Class Container"
        Public Class Empbranch
            Public fEmployeeID As System.String = "EmployeeID"
            Public fLocID As System.String = "LocID"
            Public fInAppt As System.String = "InAppt"
            Public fIsServer As System.String = "IsServer"
            Public frowguid As System.String = "rowguid"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fIsHost As System.String = "IsHost"
            Public fLastSyncBy As System.String = "LastSyncBy"

            Protected _EmployeeID As System.String
            Protected _LocID As System.String
            Private _InAppt As System.Byte
            Private _IsServer As System.Byte
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _IsHost As System.Byte
            Private _LastSyncBy As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property EmployeeID As System.String
                Get
                    Return _EmployeeID
                End Get
                Set(ByVal Value As System.String)
                    _EmployeeID = Value
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
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property InAppt As System.Byte
                Get
                    Return _InAppt
                End Get
                Set(ByVal Value As System.Byte)
                    _InAppt = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsServer As System.Byte
                Get
                    Return _IsServer
                End Get
                Set(ByVal Value As System.Byte)
                    _IsServer = Value
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
    Public Class EmpbranchInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "EmployeeID,LocID,InAppt,IsServer,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
                .CheckFields = "InAppt,IsServer,IsHost"
                .TableName = "EMPBRANCH WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "EmployeeID,LocID,InAppt,IsServer,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
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
    Public Class EmployeeBranchScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "EmployeeID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LocID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "InAppt"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsServer"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)

        End Sub

        Public ReadOnly Property EmployeeID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property LocID As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property

        Public ReadOnly Property InAppt As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property IsServer As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace




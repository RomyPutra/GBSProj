
Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Actions
    Public NotInheritable Class NotifyItmComp
        Inherits Core.CoreControl
        Private NotifyitmcompInfo As NotifyitmcompInfo = New NotifyitmcompInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function Save(ByVal NotifyitmcompCont As Container.Notifyitmcomp, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If NotifyitmcompCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With NotifyitmcompInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyitmcompCont.TransNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyitmcompCont.ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyitmcompCont.ComponentCode) & "'")
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
                                .TableName = "Notifyitmcomp WITH (ROWLOCK)"
                                .AddField("ComponentDesc", NotifyitmcompCont.ComponentDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("Value", NotifyitmcompCont.Value, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Unit", NotifyitmcompCont.Unit, SQLControl.EnumDataType.dtString)
                                .AddField("Status", NotifyitmcompCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", NotifyitmcompCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", NotifyitmcompCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", NotifyitmcompCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", NotifyitmcompCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", NotifyitmcompCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", NotifyitmcompCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsHost", NotifyitmcompCont.IsHost, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyitmcompCont.TransNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyitmcompCont.ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyitmcompCont.ComponentCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("TransNo", NotifyitmcompCont.TransNo, SQLControl.EnumDataType.dtString)
                                                .AddField("ItemCode", NotifyitmcompCont.ItemCode, SQLControl.EnumDataType.dtString)
                                                .AddField("ComponentCode", NotifyitmcompCont.ComponentCode, SQLControl.EnumDataType.dtString)
                                                .AddField("Status", NotifyitmcompCont.Status, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyitmcompCont.TransNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyitmcompCont.ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyitmcompCont.ComponentCode) & "'")
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
                                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotifyItmComp", axExecute.Message & strSQL, axExecute.StackTrace)
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
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotifyItmComp", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotifyItmComp", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                NotifyitmcompCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal NotifyitmcompCont As Container.Notifyitmcomp, ByRef message As String) As Boolean
            Return Save(NotifyitmcompCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal NotifyitmcompCont As Container.Notifyitmcomp, ByRef message As String) As Boolean
            Return Save(NotifyitmcompCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal NotifyitmcompCont As Container.Notifyitmcomp, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If NotifyitmcompCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With NotifyitmcompInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyitmcompCont.TransNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyitmcompCont.ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyitmcompCont.ComponentCode) & "'")
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
                                strSQL = BuildUpdate("Notifyitmcomp WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyitmcompCont.UpdateBy) & "' WHERE" & _
                                "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyitmcompCont.TransNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyitmcompCont.ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyitmcompCont.ComponentCode) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("Notifyitmcomp WITH (ROWLOCK)", "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyitmcompCont.TransNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyitmcompCont.ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyitmcompCont.ComponentCode) & "'")
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotifyItmComp", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotifyItmComp", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotifyItmComp", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                NotifyitmcompCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetNotifyItmComp(ByVal TransNo As System.String, ByVal ItemCode As System.String, ByVal ComponentCode As System.String) As Container.Notifyitmcomp
            Dim rNotifyitmcomp As Container.Notifyitmcomp = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With NotifyitmcompInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ComponentCode) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rNotifyitmcomp = New Container.Notifyitmcomp
                                rNotifyitmcomp.TransNo = drRow.Item("TransNo")
                                rNotifyitmcomp.ItemCode = drRow.Item("ItemCode")
                                rNotifyitmcomp.ComponentCode = drRow.Item("ComponentCode")
                                rNotifyitmcomp.ComponentDesc = drRow.Item("ComponentDesc")
                                rNotifyitmcomp.Value = drRow.Item("Value")
                                rNotifyitmcomp.Unit = drRow.Item("Unit")
                                rNotifyitmcomp.Status = drRow.Item("Status")
                                rNotifyitmcomp.CreateBy = drRow.Item("CreateBy")
                                rNotifyitmcomp.UpdateBy = drRow.Item("UpdateBy")
                                rNotifyitmcomp.Active = drRow.Item("Active")
                                rNotifyitmcomp.Inuse = drRow.Item("Inuse")
                                rNotifyitmcomp.rowguid = drRow.Item("rowguid")
                                rNotifyitmcomp.SyncCreate = drRow.Item("SyncCreate")
                                rNotifyitmcomp.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rNotifyitmcomp.IsHost = drRow.Item("IsHost")
                                rNotifyitmcomp.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rNotifyitmcomp = Nothing
                            End If
                        Else
                            rNotifyitmcomp = Nothing
                        End If
                    End With
                End If
                Return rNotifyitmcomp
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotifyItmComp", ex.Message, ex.StackTrace)
            Finally
                rNotifyitmcomp = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetNotifyItmComp(ByVal TransNo As System.String, ByVal ItemCode As System.String, ByVal ComponentCode As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Notifyitmcomp)
            Dim rNotifyitmcomp As Container.Notifyitmcomp = Nothing
            Dim lstNotifyitmcomp As List(Of Container.Notifyitmcomp) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With NotifyitmcompInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal TransNo As System.String, ByVal ItemCode As System.String, ByVal ComponentCode As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ComponentCode) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rNotifyitmcomp = New Container.Notifyitmcomp
                                rNotifyitmcomp.TransNo = drRow.Item("TransNo")
                                rNotifyitmcomp.ItemCode = drRow.Item("ItemCode")
                                rNotifyitmcomp.ComponentCode = drRow.Item("ComponentCode")
                                rNotifyitmcomp.ComponentDesc = drRow.Item("ComponentDesc")
                                rNotifyitmcomp.Value = drRow.Item("Value")
                                rNotifyitmcomp.Unit = drRow.Item("Unit")
                                rNotifyitmcomp.Status = drRow.Item("Status")
                                rNotifyitmcomp.CreateBy = drRow.Item("CreateBy")
                                rNotifyitmcomp.UpdateBy = drRow.Item("UpdateBy")
                                rNotifyitmcomp.Active = drRow.Item("Active")
                                rNotifyitmcomp.Inuse = drRow.Item("Inuse")
                                rNotifyitmcomp.rowguid = drRow.Item("rowguid")
                                rNotifyitmcomp.SyncCreate = drRow.Item("SyncCreate")
                                rNotifyitmcomp.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rNotifyitmcomp.IsHost = drRow.Item("IsHost")
                                rNotifyitmcomp.LastSyncBy = drRow.Item("LastSyncBy")
                            Next
                            lstNotifyitmcomp.Add(rNotifyitmcomp)
                        Else
                            rNotifyitmcomp = Nothing
                        End If
                        Return lstNotifyitmcomp
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotifyItmComp", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rNotifyitmcomp = Nothing
                lstNotifyitmcomp = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetNotifyItmCompList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With NotifyitmcompInfo.MyInfo
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

        Public Overloads Function GetNotifyItmCompShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With NotifyitmcompInfo.MyInfo
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

        'Add with no filter
        Public Overloads Function GetWasteComponentList(ByVal TransNo As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With NotifyitmcompInfo.MyInfo

                    strSQL = "select (ItemCode + ' ' + cm.CodeDesc) as ItemCodeType, c.*" & _
                        " from NOTIFYITMCOMP c" & _
                        " LEFT OUTER JOIN CODEMASTER cm on c.WasteType = cm.Code" & _
                        " where c.TransNo='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransNo) & "'"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        'Add with filter status = 1
        Public Overloads Function GetWasteComponent(ByVal TransNo As String, ByVal ItemCode As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With NotifyitmcompInfo.MyInfo

                    strSQL = "select * from NOTIFYITMCOMP where TransNo='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransNo) & "' and ItemCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "'" & _
                             " and status=1 order by ComponentDesc asc"

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


    Namespace Container
#Region "Class Container"
        Public Class Notifyitmcomp
            Public fTransNo As System.String = "TransNo"
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
            Public fWasteType As System.String = "WasteType"

            Protected _TransNo As System.String
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
            Private _WasteType As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property TransNo As System.String
                Get
                    Return _TransNo
                End Get
                Set(ByVal Value As System.String)
                    _TransNo = Value
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

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property WasteType As System.String
                Get
                    Return _WasteType
                End Get
                Set(ByVal Value As System.String)
                    _WasteType = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class NotifyitmcompInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "TransNo,ItemCode,ComponentCode,ComponentDesc,Value,Unit,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
                .CheckFields = "Status,Active,Inuse,Flag,IsHost"
                .TableName = "Notifyitmcomp WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "TransNo,ItemCode,ComponentCode,ComponentDesc,Value,Unit,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
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
    Public Class NotifyItmCompScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TransNo"
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

        Public ReadOnly Property TransNo As StrucElement
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


Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Contract
    Public NotInheritable Class LicenseItem
        Inherits Core.CoreControl
        Private LICENSEITEMInfo As LICENSEITEMInfo = New LICENSEITEMInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function BatchSave(ByVal ListContLICENSEITEM As List(Of Contract.Container.Unit), ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()

            BatchSave = False
            Try
                If ListContLICENSEITEM Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()

                        If ListContLICENSEITEM.Count > 0 Then
                            strSQL = BuildDelete(LICENSEITEMInfo.MyInfo.TableName, "ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ListContLICENSEITEM(0).ContractNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ListContLICENSEITEM(0).ItemCode) & "'")
                            ListSQL.Add(strSQL)
                        End If

                    End If

                    For Each UnitCont In ListContLICENSEITEM
                        With objSQL
                            .TableName = "LICENSEITEM WITH (ROWLOCK)"
                            .AddField("Qty", UnitCont.Qty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("PackQty", UnitCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("UOM", UnitCont.UOM, SQLControl.EnumDataType.dtString)
                            .AddField("CreateDate", UnitCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("LastUpdate", UnitCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("UpdateBy", UnitCont.UpdateBy, SQLControl.EnumDataType.dtString)
                            .AddField("CreateBy", UnitCont.CreateBy, SQLControl.EnumDataType.dtString)
                            .AddField("Active", UnitCont.Active, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Inuse", UnitCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Flag", UnitCont.Flag, SQLControl.EnumDataType.dtNumeric)
                            .AddField("SyncLastUpd", UnitCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)

                            Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    .AddField("ContractNo", UnitCont.ContractNo, SQLControl.EnumDataType.dtString)
                                    .AddField("ItemCode", UnitCont.ItemCode, SQLControl.EnumDataType.dtString)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                Case SQLControl.EnumSQLType.stUpdate
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ListContLICENSEITEM(0).ContractNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ListContLICENSEITEM(0).ItemCode) & "'")
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
                        Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseItem", axExecute.Message & sqlStatement, axExecute.StackTrace)
                    Finally
                        objSQL.Dispose()
                    End Try
                    Return True

                End If
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseItem", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseItem", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ListContLICENSEITEM = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

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
                        With LICENSEITEMInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.ContractNo) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.ItemCode) & "'")
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
                                .AddField("Qty", UnitCont.Qty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("PackQty", UnitCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("UOM", UnitCont.UOM, SQLControl.EnumDataType.dtString)
                                .AddField("CreateDate", UnitCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastUpdate", UnitCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", UnitCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", UnitCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", UnitCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", UnitCont.Flag, SQLControl.EnumDataType.dtNumeric)
                                .AddField("SyncLastUpd", UnitCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.ContractNo) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.ItemCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("ContractNo", UnitCont.ContractNo, SQLControl.EnumDataType.dtString)
                                                .AddField("ItemCode", UnitCont.ItemCode, SQLControl.EnumDataType.dtString)

                                                .AddField("CreateBy", UnitCont.CreateBy, SQLControl.EnumDataType.dtString)

                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.ContractNo) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.ItemCode) & "'")
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
                                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseItem", axExecute.Message & strSQL, axExecute.StackTrace)
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
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseItem", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseItem", exAssign.Message, exAssign.StackTrace)
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

        'BATCH ADD
        Public Function BatchInsert(ByVal ListContLICENSEITEM As List(Of Contract.Container.Unit), ByRef message As String) As Boolean
            Return BatchSave(ListContLICENSEITEM, SQLControl.EnumSQLType.stInsert, message)
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
                        With LICENSEITEMInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.ContractNo) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.ItemCode) & "'")

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
                                strSQL = BuildUpdate("LICENSEITEM WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = '" & UnitCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.UpdateBy) & "'" & _
                                " Where ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.ContractNo) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.ItemCode) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("LICENSEITEM WITH (ROWLOCK)", "ContractNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.ContractNo) & "' AND ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UnitCont.ItemCode) & "'")
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseItem", exExecute.Message & strSQL, exExecute.StackTrace)
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseItem", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseItem", exDelete.Message, exDelete.StackTrace)
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
        Public Overloads Function GetUnit(ByVal ContractNo As System.String, ByVal ItemCode As System.String) As Container.Unit
            Dim rUnit As Container.Unit = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With LICENSEITEMInfo.MyInfo
                        strSQL = BuildSelect(.CheckFields, .TableName, "ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ContractNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "'")

                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rUnit = New Container.Unit
                                rUnit.ContractNo = drRow.Item("ContractNo")
                                rUnit.ItemCode = drRow.Item("ItemCode")
                                rUnit.Qty = drRow.Item("Qty")
                                rUnit.PackQty = drRow.Item("PackQty")
                                rUnit.UOM = drRow.Item("UOM")
                                rUnit.CreateDate = drRow.Item("CreateDate")
                                rUnit.LastUpdate = drRow.Item("LastUpdate")
                                rUnit.UpdateBy = drRow.Item("Updateby")
                                rUnit.Active = drRow.Item("Active")
                                rUnit.Inuse = drRow.Item("Inuse")
                                rUnit.Flag = drRow.Item("Flag")
                                rUnit.rowguid = drRow.Item("rowguid")
                                rUnit.SyncCreate = drRow.Item("SyncCreate")
                                rUnit.SyncLastUpd = drRow.Item("SyncLastUpd")

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
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseItem", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rUnit = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetUnit(ByVal UOMCode As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Unit)
            Dim rUnit As Container.Unit = Nothing
            Dim lstState As List(Of Container.Unit) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With LICENSEITEMInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal UOMCode As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "UOMCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, UOMCode) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rUnit = New Container.Unit
                                rUnit = New Container.Unit
                                rUnit.ContractNo = drRow.Item("ContractNo")
                                rUnit.ItemCode = drRow.Item("ItemCode")
                                rUnit.Qty = drRow.Item("Qty")
                                rUnit.PackQty = drRow.Item("PackQty")
                                rUnit.UOM = drRow.Item("UOM")
                                rUnit.CreateDate = drRow.Item("CreateDate")
                                rUnit.LastUpdate = drRow.Item("LastUpdate")
                                rUnit.UpdateBy = drRow.Item("Updateby")
                                rUnit.Active = drRow.Item("Active")
                                rUnit.Inuse = drRow.Item("Inuse")
                                rUnit.Flag = drRow.Item("Flag")
                                rUnit.rowguid = drRow.Item("rowguid")
                                rUnit.SyncCreate = drRow.Item("SyncCreate")
                                rUnit.SyncLastUpd = drRow.Item("SyncLastUpd")
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
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseItem", ex.Message, ex.StackTrace)
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
                With LICENSEITEMInfo.MyInfo
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
                With LICENSEITEMInfo.MyInfo
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

        Public Overloads Function GetWasteInfo(ByVal LocID As System.String) As Data.DataTable
            If StartConnection() = True Then
                With LICENSEITEMInfo.MyInfo
                    StartSQLControl()

                    strSQL = " Select L.ContractNo, li.ItemCode, c.CodeDesc, c.Code, i.ShortDesc, l.CreateDate, li.Active, li.flag, '' As valueName " & _
                             "From CodeMaster c, license l, licenseitem li, item i " & _
                             "Where l.locid = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND c.CODETYPE = 'WTY' AND c.Active = 1 " & _
                             "AND l.ContractNo = li.ContractNo AND i.ItemCode = li.ItemCode"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetWasteInfoUpdate(ByVal LocID As System.String) As Data.DataTable
            If StartConnection() = True Then
                With LICENSEITEMInfo.MyInfo
                    StartSQLControl()

                    strSQL = "select distinct ItemCode, '' As itemType, '' As valueName from license l " & _
                             "inner join licenseitem li on l.ContractNo = li.ContractNo " & _
                             "where locid = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' "

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

#End Region

    End Class

    Namespace Container
#Region "Class Container"
        Public Class Unit
            Public fContractNo As System.String = "ContractNo"
            Public fItemCode As System.String = "ItemCode"
            Public fQty As System.String = "Qty"
            Public fPackQty As System.String = "PackQty"
            Public fUOM As System.String = "UOM"
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


            Protected _ContractNo As System.String
            Protected _Qty As System.Byte
            Protected _ItemCode As System.String
            Protected _UOM As System.String
            Protected _PackQty As System.Byte
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


            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ContractNo As System.String
                Get
                    Return _ContractNo
                End Get
                Set(ByVal Value As System.String)
                    _ContractNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property UOM As System.String
                Get
                    Return _UOM
                End Get
                Set(ByVal Value As System.String)
                    _UOM = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property Qty As System.Byte
                Get
                    Return _Qty
                End Get
                Set(ByVal Value As System.Byte)
                    _Qty = Value
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
            Public Property PackQty As System.Byte
                Get
                    Return _PackQty
                End Get
                Set(ByVal Value As System.Byte)
                    _PackQty = Value
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


        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class LICENSEITEMInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "ContractNo, ItemCode, Qty, PackQty,UOM, CreateDate, CreateBy, LastUpdate, UpdateBy, Active, Inuse, Flag, rowguid, SyncCreate, SyncLastUpd"
                .CheckFields = "Active,Inuse,Flag"
                .TableName = "LICENSEITEM WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "ContractNo, ItemCode, Qty, PackQty,UOM, CreateDate, CreateBy, LastUpdate, UpdateBy, Active, Inuse, Flag, rowguid, SyncCreate, SyncLastUpd"
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
                .FieldName = "ContractNo"
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
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Qty"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "PackQty"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UOM"
                .Length = 10
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
                .IsMandatory = True
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
                .IsMandatory = True
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


        End Sub

        Public ReadOnly Property ContractNo As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property ItemCode As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property

        Public ReadOnly Property Qty As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property

        Public ReadOnly Property PackQty As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property

        Public ReadOnly Property UOM As StrucElement
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

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
End Namespace


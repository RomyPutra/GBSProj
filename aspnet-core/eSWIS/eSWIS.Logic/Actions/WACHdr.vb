Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Actions
    Public NotInheritable Class WACHdr
        Inherits Core.CoreControl

        Private WachdrInfo As WachdrInfo = New WachdrInfo
        Private WacdtlInfo As WacdtlInfo = New WacdtlInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal connecn As String)
            ConnectionString = connecn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal WachdrCont As Container.Wachdr, ByVal ListWACDtlCont As List(Of Actions.Container.Wacdtl), ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            Save = False

            Try
                If WachdrCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With WachdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "WACNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WachdrCont.WACNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WachdrCont.ItemCode) & "' AND ItemType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WachdrCont.ItemType) & "' AND IndustryType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WachdrCont.IndustryType) & "'")
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
                                .TableName = "Wachdr WITH (ROWLOCK)"
                                .AddField("ItemDesc", WachdrCont.ItemDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("Remark", WachdrCont.Remark, SQLControl.EnumDataType.dtStringN)
                                .AddField("ItemType", WachdrCont.ItemType, SQLControl.EnumDataType.dtString)
                                .AddField("IndustryType", WachdrCont.IndustryType, SQLControl.EnumDataType.dtString)
                                .AddField("Classification", WachdrCont.Classification, SQLControl.EnumDataType.dtString)
                                .AddField("Handling", WachdrCont.Handling, SQLControl.EnumDataType.dtString)
                                .AddField("CreateDate", WachdrCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", WachdrCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", WachdrCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", WachdrCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Posted", WachdrCont.Posted, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Active", WachdrCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", WachdrCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", WachdrCont.Flag, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsHost", WachdrCont.IsHost, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "WACNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WachdrCont.WACNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WachdrCont.ItemCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("WACNo", WachdrCont.WACNo, SQLControl.EnumDataType.dtString)
                                                .AddField("ItemCode", WachdrCont.ItemCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "WACNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WachdrCont.WACNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WachdrCont.ItemCode) & "'")
                                End Select
                                ListSQL.Add(strSQL)
                            End With

                            If StartConnection(EnumIsoState.StateUpdatetable) = True Then 'if update then delete
                                StartSQLControl()
                                If ListWACDtlCont.Count > 0 Then
                                    strSQL = BuildDelete("Wachdr WITH (ROWLOCK)", "WACNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ListWACDtlCont(0).WACNo) & "'")
                                    ListSQL.Add(strSQL)
                                End If
                            End If

                            For Each WACDtlCont In ListWACDtlCont
                                With objSQL
                                    .TableName = "Wacdtl WITH (ROWLOCK)"
                                    .AddField("ComponentDesc", WACDtlCont.ComponentDesc, SQLControl.EnumDataType.dtStringN)
                                    .AddField("ValueMin", WACDtlCont.ValueMin, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ValueMax", WACDtlCont.ValueMax, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("AvgMin", WACDtlCont.AvgMin, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("AvgMax", WACDtlCont.AvgMax, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Unit", WACDtlCont.Unit, SQLControl.EnumDataType.dtString)
                                    .AddField("CreateDate", WACDtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateBy", WACDtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("LastUpdate", WACDtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("UpdateBy", WACDtlCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("Active", WACDtlCont.Active, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Inuse", WACDtlCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Flag", WACDtlCont.Flag, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("IsHost", WACDtlCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("WACNo", WACDtlCont.WACNo, SQLControl.EnumDataType.dtString)
                                    .AddField("ComponentCode", WACDtlCont.ComponentCode, SQLControl.EnumDataType.dtString)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                End With
                                ListSQL.Add(strSQL)
                            Next

                            Try
                                objConn.BatchExecute(ListSQL, CommandType.Text)
                                Return True

                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                Else
                                    message = axExecute.Message.ToString()
                                End If

                                Dim sqlStatement As String = " "
                                If objConn.FailedSQLStatement.Count > 0 Then
                                    sqlStatement &= objConn.FailedSQLStatement.Item(0)
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("WACHdr", axExecute.Message & sqlStatement, axExecute.StackTrace)
                                Return False
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If

                    End If
                End If
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Return False
            Finally
                WachdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal WachdrCont As Container.Wachdr, ByVal ListWACDtlCont As List(Of Actions.Container.Wacdtl), ByRef message As String) As Boolean
            Return Save(WachdrCont, ListWACDtlCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal WachdrCont As Container.Wachdr, ByVal ListWACDtlCont As List(Of Actions.Container.Wacdtl), ByRef message As String) As Boolean
            Return Save(WachdrCont, ListWACDtlCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal WachdrCont As Container.Wachdr, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If WachdrCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With WachdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "WACNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WachdrCont.WACNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WachdrCont.ItemCode) & "'")
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
                                strSQL = BuildUpdate("Wachdr WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, WachdrCont.UpdateBy) & "' WHERE" & _
                                "WACNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WachdrCont.WACNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WachdrCont.ItemCode) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("Wachdr WITH (ROWLOCK)", "WACNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WachdrCont.WACNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WachdrCont.ItemCode) & "'")
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Return False
            Finally
                WachdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"

        Public Overloads Function GetWACHdr(ByVal WACNo As System.String, ByVal ItemCode As System.String) As Container.Wachdr
            Dim rWachdr As Container.Wachdr = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With WachdrInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "WACNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WACNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWachdr = New Container.Wachdr
                                rWachdr.WACNo = drRow.Item("WACNo")
                                rWachdr.ItemCode = drRow.Item("ItemCode")
                                rWachdr.ItemDesc = drRow.Item("ItemDesc")
                                rWachdr.Remark = drRow.Item("Remark")
                                rWachdr.ItemType = drRow.Item("ItemType")
                                rWachdr.IndustryType = drRow.Item("IndustryType")
                                rWachdr.Classification = drRow.Item("Classification")
                                rWachdr.Handling = drRow.Item("Handling")
                                rWachdr.CreateBy = drRow.Item("CreateBy")
                                rWachdr.UpdateBy = drRow.Item("UpdateBy")
                                rWachdr.Active = drRow.Item("Active")
                                rWachdr.Inuse = drRow.Item("Inuse")
                                rWachdr.rowguid = drRow.Item("rowguid")
                                rWachdr.SyncCreate = drRow.Item("SyncCreate")
                                rWachdr.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWachdr.IsHost = drRow.Item("IsHost")
                                rWachdr.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rWachdr = Nothing
                            End If
                        Else
                            rWachdr = Nothing
                        End If
                    End With
                End If
                Return rWachdr
            Catch ex As Exception
                Throw ex
            Finally
                rWachdr = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWACHdr(ByVal WACNo As System.String, ByVal ItemCode As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Wachdr)
            Dim rWachdr As Container.Wachdr = Nothing
            Dim lstWachdr As List(Of Container.Wachdr) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With WachdrInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal WACNo As System.String, ByVal ItemCode As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "WACNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WACNo) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rWachdr = New Container.Wachdr
                                rWachdr.WACNo = drRow.Item("WACNo")
                                rWachdr.ItemCode = drRow.Item("ItemCode")
                                rWachdr.ItemDesc = drRow.Item("ItemDesc")
                                rWachdr.Remark = drRow.Item("Remark")
                                rWachdr.ItemType = drRow.Item("ItemType")
                                rWachdr.IndustryType = drRow.Item("IndustryType")
                                rWachdr.Classification = drRow.Item("Classification")
                                rWachdr.Handling = drRow.Item("Handling")
                                rWachdr.CreateBy = drRow.Item("CreateBy")
                                rWachdr.UpdateBy = drRow.Item("UpdateBy")
                                rWachdr.Active = drRow.Item("Active")
                                rWachdr.Inuse = drRow.Item("Inuse")
                                rWachdr.Flag = drRow.Item("Flag")
                                rWachdr.Posted = drRow.Item("Posted")
                                rWachdr.rowguid = drRow.Item("rowguid")
                                rWachdr.SyncCreate = drRow.Item("SyncCreate")
                                rWachdr.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWachdr.IsHost = drRow.Item("IsHost")
                                rWachdr.LastSyncBy = drRow.Item("LastSyncBy")
                            Next
                            lstWachdr.Add(rWachdr)
                        Else
                            rWachdr = Nothing
                        End If
                        Return lstWachdr
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rWachdr = Nothing
                lstWachdr = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWACHdrList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With WachdrInfo.MyInfo
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

        Public Overloads Function GetWACHdrShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With WachdrInfo.MyInfo
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

        Public Overloads Function GetWACHDRByCodeTypeIndustry(ByVal ItemCode As String, ByVal ItemType As String) As Container.Wachdr
            Dim rWachdr As Container.Wachdr = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With WachdrInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "' AND ItemType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemType) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWachdr = New Container.Wachdr
                                rWachdr.WACNo = drRow.Item("WACNo")
                                rWachdr.ItemCode = drRow.Item("ItemCode")
                                rWachdr.ItemDesc = drRow.Item("ItemDesc")
                                rWachdr.Remark = drRow.Item("Remark")
                                rWachdr.ItemType = drRow.Item("ItemType")
                                rWachdr.IndustryType = drRow.Item("IndustryType")
                                rWachdr.Classification = drRow.Item("Classification")
                                rWachdr.Handling = drRow.Item("Handling")
                                rWachdr.Flag = drRow.Item("Flag")
                                rWachdr.Posted = drRow.Item("Posted")
                                rWachdr.CreateBy = drRow.Item("CreateBy")
                                rWachdr.UpdateBy = drRow.Item("UpdateBy")
                                rWachdr.Active = drRow.Item("Active")
                                rWachdr.Inuse = drRow.Item("Inuse")
                                rWachdr.Flag = drRow.Item("Flag")
                                rWachdr.Posted = drRow.Item("Posted")
                                rWachdr.rowguid = drRow.Item("rowguid")
                                rWachdr.SyncCreate = drRow.Item("SyncCreate")
                                rWachdr.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWachdr.IsHost = drRow.Item("IsHost")
                                rWachdr.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rWachdr = Nothing
                            End If
                        Else
                            rWachdr = Nothing
                        End If
                    End With
                End If
                Return rWachdr
            Catch ex As Exception
                Throw ex
            Finally
                rWachdr = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWACHDRByCodeType(ByVal ItemCode As String, ByVal ItemType As String) As Container.Wachdr
            Dim rWachdr As Container.Wachdr = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With WachdrInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "' AND ItemType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemType) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWachdr = New Container.Wachdr
                                rWachdr.WACNo = drRow.Item("WACNo")
                                rWachdr.ItemCode = drRow.Item("ItemCode")
                                rWachdr.ItemDesc = drRow.Item("ItemDesc")
                                rWachdr.Remark = drRow.Item("Remark")
                                rWachdr.ItemType = drRow.Item("ItemType")
                                rWachdr.IndustryType = drRow.Item("IndustryType")
                                rWachdr.Classification = drRow.Item("Classification")
                                rWachdr.Handling = drRow.Item("Handling")
                                rWachdr.Flag = drRow.Item("Flag")
                                rWachdr.Posted = drRow.Item("Posted")
                                rWachdr.CreateBy = drRow.Item("CreateBy")
                                rWachdr.UpdateBy = drRow.Item("UpdateBy")
                                rWachdr.Active = drRow.Item("Active")
                                rWachdr.Inuse = drRow.Item("Inuse")
                                rWachdr.Flag = drRow.Item("Flag")
                                rWachdr.Posted = drRow.Item("Posted")
                                rWachdr.rowguid = drRow.Item("rowguid")
                                rWachdr.SyncCreate = drRow.Item("SyncCreate")
                                rWachdr.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWachdr.IsHost = drRow.Item("IsHost")
                                rWachdr.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rWachdr = Nothing
                            End If
                        Else
                            rWachdr = Nothing
                        End If
                    End With
                End If
                Return rWachdr
            Catch ex As Exception
                Throw ex
            Finally
                rWachdr = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWasteCodeByType(ByVal WasteType As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With WachdrInfo.MyInfo
                    strSQL = "SELECT DISTINCT ItemCode, ItemDesc FROM WACHDR WHERE ITEMTYPE = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "'"
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
        Public Class Wachdr
            Public fWACNo As System.String = "WACNo"
            Public fItemCode As System.String = "ItemCode"
            Public fItemDesc As System.String = "ItemDesc"
            Public fRemark As System.String = "Remark"
            Public fItemType As System.String = "ItemType"
            Public fIndustryType As System.String = "IndustryType"
            Public fClassification As System.String = "Classification"
            Public fHandling As System.String = "Handling"
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
            Public fPosted As System.String = "Posted"

            Protected _WACNo As System.String
            Protected _ItemCode As System.String
            Private _ItemDesc As System.String
            Private _Remark As System.String
            Private _ItemType As System.String
            Private _IndustryType As System.String
            Private _Classification As System.String
            Private _Handling As System.String
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
            Private _Posted As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property WACNo As System.String
                Get
                    Return _WACNo
                End Get
                Set(ByVal Value As System.String)
                    _WACNo = Value
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
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ItemDesc As System.String
                Get
                    Return _ItemDesc
                End Get
                Set(ByVal Value As System.String)
                    _ItemDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Remark As System.String
                Get
                    Return _Remark
                End Get
                Set(ByVal Value As System.String)
                    _Remark = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ItemType As System.String
                Get
                    Return _ItemType
                End Get
                Set(ByVal Value As System.String)
                    _ItemType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IndustryType As System.String
                Get
                    Return _IndustryType
                End Get
                Set(ByVal Value As System.String)
                    _IndustryType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Classification As System.String
                Get
                    Return _Classification
                End Get
                Set(ByVal Value As System.String)
                    _Classification = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Handling As System.String
                Get
                    Return _Handling
                End Get
                Set(ByVal Value As System.String)
                    _Handling = Value
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
            Public Property Flag As System.String
                Get
                    Return _Flag
                End Get
                Set(ByVal Value As System.String)
                    _Flag = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Posted As System.String
                Get
                    Return _Posted
                End Get
                Set(ByVal Value As System.String)
                    _Posted = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class WachdrInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "WACNo,ItemCode,ItemDesc,Remark,ItemType,IndustryType,Classification,Handling,CreateDate,CreateBy,LastUpdate,UpdateBy,Posted,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
                .CheckFields = "Active,Inuse,Flag,IsHost"
                .TableName = "Wachdr WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "WACNo,ItemCode,ItemDesc,Remark,ItemType,IndustryType,Classification,Handling,CreateDate,CreateBy,LastUpdate,UpdateBy,Posted,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
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
    Public Class WACHdrScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WACNo"
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
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ItemDesc"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ItemType"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "IndustryType"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Classification"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Handling"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
        End Sub

        Public ReadOnly Property WACNo As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property ItemCode As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property

        Public ReadOnly Property ItemDesc As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property Remark As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property ItemType As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property IndustryType As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property Classification As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property Handling As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property

        Public ReadOnly Property Posted As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace
Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace InventorySummary
#Region "ITEMSUMMARY Class"
    Public NotInheritable Class ITEMSUMMARY
        Inherits Core.CoreControl
        Private ItemsummaryInfo As ItemsummaryInfo = New ItemsummaryInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal ItemsummaryCont As Container.Itemsummary, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ItemsummaryCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItemsummaryInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "LocID = '" & ItemsummaryCont.LocID & "' AND ItemCode = '" & ItemsummaryCont.ItemCode & "' AND ItemName = '" & ItemsummaryCont.ItemName & "' AND YearCode = '" & ItemsummaryCont.YearCode & "' AND MthCode = '" & ItemsummaryCont.MthCode & "'")
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
                                .TableName = "Itemsummary"
                                .AddField("RefNo", ItemsummaryCont.RefNo, SQLControl.EnumDataType.dtString)
                                .AddField("StorageID", ItemsummaryCont.StorageID, SQLControl.EnumDataType.dtString)
                                .AddField("Opening", ItemsummaryCont.Opening, SQLControl.EnumDataType.dtNumeric)
                                .AddField("QtyIn", ItemsummaryCont.QtyIn, SQLControl.EnumDataType.dtNumeric)
                                .AddField("QtyHandling", ItemsummaryCont.QtyHandling, SQLControl.EnumDataType.dtNumeric)
                                .AddField("QtyOut", ItemsummaryCont.QtyOut, SQLControl.EnumDataType.dtNumeric)
                                .AddField("QtyReused", ItemsummaryCont.QtyReused, SQLControl.EnumDataType.dtNumeric)
                                .AddField("QtyAdj", ItemsummaryCont.QtyAdj, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Closing", ItemsummaryCont.Closing, SQLControl.EnumDataType.dtNumeric)
                                .AddField("YTDQty", ItemsummaryCont.YTDQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("MTDQty", ItemsummaryCont.MTDQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("FirstGenerated", ItemsummaryCont.FirstGenerated, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateDate", ItemsummaryCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", ItemsummaryCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", ItemsummaryCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", ItemsummaryCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("rowguid", ItemsummaryCont.rowguid, SQLControl.EnumDataType.dtString)
                                .AddField("SyncCreate", ItemsummaryCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", ItemsummaryCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("IsHost", ItemsummaryCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastSyncBy", ItemsummaryCont.LastSyncBy, SQLControl.EnumDataType.dtString)
                                .AddField("IsTrial", ItemsummaryCont.IsTrial, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsReqCus", ItemsummaryCont.IsReqCus, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ShelfLife", ItemsummaryCont.ShelfLife, SQLControl.EnumDataType.dtNumeric)
                                .AddField("WarrantyPeriod", ItemsummaryCont.WarrantyPeriod, SQLControl.EnumDataType.dtNumeric)
                                .AddField("WarrantyType", ItemsummaryCont.WarrantyType, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & ItemsummaryCont.LocID & "' AND ItemCode = '" & ItemsummaryCont.ItemCode & "' AND ItemName = '" & ItemsummaryCont.ItemName & "' AND YearCode = '" & ItemsummaryCont.YearCode & "' AND MthCode = '" & ItemsummaryCont.MthCode & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("LocID", ItemsummaryCont.LocID, SQLControl.EnumDataType.dtString)
                                                .AddField("ItemCode", ItemsummaryCont.ItemCode, SQLControl.EnumDataType.dtString)
                                                .AddField("ItemName", ItemsummaryCont.ItemName, SQLControl.EnumDataType.dtStringN)
                                                .AddField("YearCode", ItemsummaryCont.YearCode, SQLControl.EnumDataType.dtString)
                                                .AddField("MthCode", ItemsummaryCont.MthCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & ItemsummaryCont.LocID & "' AND ItemCode = '" & ItemsummaryCont.ItemCode & "' AND ItemName = '" & ItemsummaryCont.ItemName & "' AND YearCode = '" & ItemsummaryCont.YearCode & "' AND MthCode = '" & ItemsummaryCont.MthCode & "'")
                                End Select
                            End With
                            Try
                                If BatchExecute Then
                                    BatchList.Add(strSQL)
                                    If Commit Then
                                        objConn.BatchExecute(BatchList, CommandType.Text, True)
                                    End If
                                Else
                                    objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                End If

                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                    Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                                Else
                                    message = axExecute.Message.ToString()
                                    Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                                End If
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
                ItemsummaryCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal ItemsummaryCont As Container.Itemsummary, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(ItemsummaryCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal ItemsummaryCont As Container.Itemsummary, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(ItemsummaryCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal ItemsummaryCont As Container.Itemsummary, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If ItemsummaryCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItemsummaryInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "LocID = '" & ItemsummaryCont.LocID & "' AND ItemCode = '" & ItemsummaryCont.ItemCode & "' AND ItemName = '" & ItemsummaryCont.ItemName & "' AND YearCode = '" & ItemsummaryCont.YearCode & "' AND MthCode = '" & ItemsummaryCont.MthCode & "'")
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
                                strSQL = BuildUpdate(ItemsummaryInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = '" & ItemsummaryCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemsummaryCont.UpdateBy) & "' WHERE" & _
                                "LocID = '" & ItemsummaryCont.LocID & "' AND ItemCode = '" & ItemsummaryCont.ItemCode & "' AND ItemName = '" & ItemsummaryCont.ItemName & "' AND YearCode = '" & ItemsummaryCont.YearCode & "' AND MthCode = '" & ItemsummaryCont.MthCode & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(ItemsummaryInfo.MyInfo.TableName, "LocID = '" & ItemsummaryCont.LocID & "' AND ItemCode = '" & ItemsummaryCont.ItemCode & "' AND ItemName = '" & ItemsummaryCont.ItemName & "' AND YearCode = '" & ItemsummaryCont.YearCode & "' AND MthCode = '" & ItemsummaryCont.MthCode & "'")
                        End If

                        Try
                            'execute
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Return False
                            'Throw New ApplicationException("210006 " & exExecute.Message.ToString())
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Return False
                'Throw axDelete
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Return False
                'Throw exDelete
            Finally
                ItemsummaryCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetITEMSUMMARY(ByVal LocID As System.String, ByVal ItemCode As System.String, ByVal ItemName As System.String, ByVal YearCode As System.String, ByVal MthCode As System.String) As Container.Itemsummary
            Dim rItemsummary As Container.Itemsummary = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With ItemsummaryInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "LocID = '" & LocID & "' AND ItemCode = '" & ItemCode & "' AND ItemName = '" & ItemName & "' AND YearCode = '" & YearCode & "' AND MthCode = '" & MthCode & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rItemsummary = New Container.Itemsummary
                                rItemsummary.LocID = drRow.Item("LocID")
                                rItemsummary.ItemCode = drRow.Item("ItemCode")
                                rItemsummary.ItemName = drRow.Item("ItemName")
                                rItemsummary.YearCode = drRow.Item("YearCode")
                                rItemsummary.MthCode = drRow.Item("MthCode")
                                rItemsummary.RefNo = drRow.Item("RefNo")
                                rItemsummary.StorageID = drRow.Item("StorageID")
                                rItemsummary.Opening = drRow.Item("Opening")
                                rItemsummary.QtyIn = drRow.Item("QtyIn")
                                rItemsummary.QtyHandling = drRow.Item("QtyHandling")
                                rItemsummary.QtyOut = drRow.Item("QtyOut")
                                rItemsummary.QtyReused = drRow.Item("QtyReused")
                                rItemsummary.QtyAdj = drRow.Item("QtyAdj")
                                rItemsummary.Closing = drRow.Item("Closing")
                                rItemsummary.YTDQty = drRow.Item("YTDQty")
                                rItemsummary.MTDQty = drRow.Item("MTDQty")
                                If Not IsDBNull(drRow.Item("FirstGenerated")) Then
                                    rItemsummary.FirstGenerated = drRow.Item("FirstGenerated")
                                End If
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rItemsummary.CreateDate = drRow.Item("CreateDate")
                                End If
                                rItemsummary.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rItemsummary.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rItemsummary.UpdateBy = drRow.Item("UpdateBy")
                                rItemsummary.rowguid = drRow.Item("rowguid")
                                rItemsummary.SyncCreate = drRow.Item("SyncCreate")
                                rItemsummary.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rItemsummary.IsHost = drRow.Item("IsHost")
                                rItemsummary.LastSyncBy = drRow.Item("LastSyncBy")
                                rItemsummary.IsTrial = drRow.Item("IsTrial")
                                rItemsummary.IsReqCus = drRow.Item("IsReqCus")
                                rItemsummary.ShelfLife = drRow.Item("ShelfLife")
                                rItemsummary.WarrantyPeriod = drRow.Item("WarrantyPeriod")
                                rItemsummary.WarrantyType = drRow.Item("WarrantyType")
                            Else
                                rItemsummary = Nothing
                            End If
                        Else
                            rItemsummary = Nothing
                        End If
                    End With
                End If
                Return rItemsummary
            Catch ex As Exception
                Throw ex
            Finally
                rItemsummary = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetITEMSUMMARY(ByVal LocID As System.String, ByVal ItemCode As System.String, ByVal ItemName As System.String, ByVal YearCode As System.String, ByVal MthCode As System.String, DecendingOrder As Boolean) As List(Of Container.Itemsummary)
            Dim rItemsummary As Container.Itemsummary = Nothing
            Dim lstItemsummary As List(Of Container.Itemsummary) = New List(Of Container.Itemsummary)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With ItemsummaryInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by LocID, ItemCode, ItemName, YearCode, MthCode DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "LocID = '" & LocID & "' AND ItemCode = '" & ItemCode & "' AND ItemName = '" & ItemName & "' AND YearCode = '" & YearCode & "' AND MthCode = '" & MthCode & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rItemsummary = New Container.Itemsummary
                                rItemsummary.LocID = drRow.Item("LocID")
                                rItemsummary.ItemCode = drRow.Item("ItemCode")
                                rItemsummary.ItemName = drRow.Item("ItemName")
                                rItemsummary.YearCode = drRow.Item("YearCode")
                                rItemsummary.MthCode = drRow.Item("MthCode")
                                rItemsummary.RefNo = drRow.Item("RefNo")
                                rItemsummary.StorageID = drRow.Item("StorageID")
                                rItemsummary.Opening = drRow.Item("Opening")
                                rItemsummary.QtyIn = drRow.Item("QtyIn")
                                rItemsummary.QtyHandling = drRow.Item("QtyHandling")
                                rItemsummary.QtyOut = drRow.Item("QtyOut")
                                rItemsummary.QtyReused = drRow.Item("QtyReused")
                                rItemsummary.QtyAdj = drRow.Item("QtyAdj")
                                rItemsummary.Closing = drRow.Item("Closing")
                                rItemsummary.YTDQty = drRow.Item("YTDQty")
                                rItemsummary.MTDQty = drRow.Item("MTDQty")
                                rItemsummary.CreateBy = drRow.Item("CreateBy")
                                rItemsummary.UpdateBy = drRow.Item("UpdateBy")
                                rItemsummary.rowguid = drRow.Item("rowguid")
                                rItemsummary.SyncCreate = drRow.Item("SyncCreate")
                                rItemsummary.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rItemsummary.IsHost = drRow.Item("IsHost")
                                rItemsummary.LastSyncBy = drRow.Item("LastSyncBy")
                                rItemsummary.IsTrial = drRow.Item("IsTrial")
                                rItemsummary.IsReqCus = drRow.Item("IsReqCus")
                                rItemsummary.ShelfLife = drRow.Item("ShelfLife")
                                rItemsummary.WarrantyPeriod = drRow.Item("WarrantyPeriod")
                                rItemsummary.WarrantyType = drRow.Item("WarrantyType")
                                lstItemsummary.Add(rItemsummary)
                            Next

                        Else
                            rItemsummary = Nothing
                        End If
                        Return lstItemsummary
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rItemsummary = Nothing
                lstItemsummary = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetInventoryCompanyListed(Optional ByVal FieldCond As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With ItemsummaryInfo.MyInfo
                    If StartConnection() = True Then
                        StartSQLControl()
                        strSQL = "SELECT it.ItemCode, il.TypeCode ItemType, it.ItemName, il.ExpiredQty, il.ExpiryDate, (it.QtyAdj-isnull(R.RejectedQty,0)) as QtyAdjust, " & _
                                 "E.CompanyName as CompanyName, b.Address1 as CityCode, /*STORAGEMASTER.StorageAreaCode as StorageArea ,  STORAGEMASTER.storagebin as Bin , " & _
                                 "*/  '' as StorageArea ,  '' as Bin ,  isnull(H.HandlingQty,0) as OutgoingQty , it.Opening as LastIn,   it.Opening as QtyOnHand,  it.Closing  AS ClosingQty, " & _
                                 "it.Opening as LastOut, it.QtyIn as IncomingQty,it.QtyReused  ,  it.Opening+IncomingQty-(QtyReused+(it.QtyAdj-isnull(R.RejectedQty,0))+OutgoingQty) as Balance  FROM ITEMSUMMARY it WITH (NOLOCK) " & _
                                 "INNER JOIN ITEMLOC il WITH (NOLOCK) on it.LocID=il.LocID and it.ItemCode=il.ItemCode AND il.ItemName=it.ItemName /*LEFT JOIN STORAGEMASTER WITH (NOLOCK) on STORAGEMASTER.LocID=it.LocID AND it.ItemCode=STORAGEMASTER.ItemCode*/ " & _
                                 "LEFT JOIN  BIZLOCATE B WITH (NOLOCK) on b.BizLocID=it.LocID  LEFT JOIN  BIZENTITY E WITH (NOLOCK) on b.BizRegID=E.BizRegID  OUTER APPLY (SELECT SUM(d.QTY) HandlingQty FROM CONSIGNDTL d WITH (NOLOCK) " & _
                                 "LEFT JOIN CONSIGNHDR h WITH (NOLOCK) ON h.CONTRACTNO = d.CONTRACTNO AND h.FLAG = 1 AND h.STATUS <> 0 AND h.STATUS <> 2 AND h.STATUS <> 9 AND h.STATUS <> 12 AND h.STATUS <> 13 AND h.Status <> 14 AND h.STATUS <> 15 AND h.ISCONFIRM = 1 " & _
                                 "/*isconfirm=1 --> release (deduct)*/ LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODE = d.OPERATIONTYPE AND cm.CODETYPE='WTH' WHERE YEAR(h.TRANSDATE) = it.YearCode AND MONTH(h.TRANSDATE) = it.MthCode AND h.GENERATORLOCID = it.LocID " & _
                                 "AND d.WASTECODE = it.ItemCode	AND d.WASTEDESCRIPTION = it.ItemName) H OUTER APPLY (SELECT SUM(d.QTY) RejectedQty FROM CONSIGNDTL d WITH (NOLOCK) LEFT JOIN CONSIGNHDR h WITH (NOLOCK) ON h.CONTRACTNO = d.CONTRACTNO AND h.FLAG = 1 AND h.STATUS = 9 " & _
                                 "AND h.ISCONFIRM = 1 /*Rejected CN*/ LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODE = d.OPERATIONTYPE AND cm.CODETYPE='WTH' WHERE YEAR(h.TRANSDATE) = it.YearCode AND MONTH(h.TRANSDATE) = it.MthCode AND h.GENERATORLOCID = it.LocID AND d.WASTECODE = it.ItemCode " & _
                                 "AND d.WASTEDESCRIPTION = it.ItemName) R WHERE (it.IsHost=0 OR (it.IsHost=1 AND ((it.Closing<>0) OR (it.Closing=0 AND (it.QtyIn<>0 or isnull(H.HandlingQty,0)<>0 or it.QtyReused<>0))))) "

                        If FieldCond IsNot Nothing AndAlso FieldCond <> "" Then
                            strSQL &= FieldCond
                        End If
                        strSQL &= " ORDER BY it.ItemCode ASC"
                        Try
                            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Catch ex As Exception
                            Log.Notifier.Notify(ex)
                            Gibraltar.Agent.Log.Error("Actions/Inventory", ex.Message & " " & strSQL, ex.StackTrace)
                        End Try
                    End If
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function


#End Region
    End Class
#End Region

#Region "Container"
    Namespace Container
#Region "Itemsummary Container"
        Public Class Itemsummary_FieldName
            Public LocID As System.String = "LocID"
            Public ItemCode As System.String = "ItemCode"
            Public ItemName As System.String = "ItemName"
            Public YearCode As System.String = "YearCode"
            Public MthCode As System.String = "MthCode"
            Public RefNo As System.String = "RefNo"
            Public StorageID As System.String = "StorageID"
            Public Opening As System.String = "Opening"
            Public QtyIn As System.String = "QtyIn"
            Public QtyHandling As System.String = "QtyHandling"
            Public QtyOut As System.String = "QtyOut"
            Public QtyReused As System.String = "QtyReused"
            Public QtyAdj As System.String = "QtyAdj"
            Public Closing As System.String = "Closing"
            Public YTDQty As System.String = "YTDQty"
            Public MTDQty As System.String = "MTDQty"
            Public FirstGenerated As System.String = "FirstGenerated"
            Public CreateDate As System.String = "CreateDate"
            Public CreateBy As System.String = "CreateBy"
            Public LastUpdate As System.String = "LastUpdate"
            Public UpdateBy As System.String = "UpdateBy"
            Public rowguid As System.String = "rowguid"
            Public SyncCreate As System.String = "SyncCreate"
            Public SyncLastUpd As System.String = "SyncLastUpd"
            Public IsHost As System.String = "IsHost"
            Public LastSyncBy As System.String = "LastSyncBy"
            Public IsTrial As System.String = "IsTrial"
            Public IsReqCus As System.String = "IsReqCus"
            Public ShelfLife As System.String = "ShelfLife"
            Public WarrantyPeriod As System.String = "WarrantyPeriod"
            Public WarrantyType As System.String = "WarrantyType"
        End Class

        Public Class Itemsummary
            Protected _LocID As System.String
            Protected _ItemCode As System.String
            Protected _ItemName As System.String
            Protected _YearCode As System.String
            Protected _MthCode As System.String
            Private _RefNo As System.String
            Private _StorageID As System.String
            Private _Opening As System.Decimal
            Private _QtyIn As System.Decimal
            Private _QtyHandling As System.Decimal
            Private _QtyOut As System.Decimal
            Private _QtyReused As System.Decimal
            Private _QtyAdj As System.Decimal
            Private _Closing As System.Decimal
            Private _YTDQty As System.Decimal
            Private _MTDQty As System.Decimal
            Private _FirstGenerated As System.DateTime
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _IsHost As System.Byte
            Private _LastSyncBy As System.String
            Private _IsTrial As System.Byte
            Private _IsReqCus As System.Byte
            Private _ShelfLife As System.Decimal
            Private _WarrantyPeriod As System.Decimal
            Private _WarrantyType As System.Byte

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
            Public Property ItemName As System.String
                Get
                    Return _ItemName
                End Get
                Set(ByVal Value As System.String)
                    _ItemName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property YearCode As System.String
                Get
                    Return _YearCode
                End Get
                Set(ByVal Value As System.String)
                    _YearCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property MthCode As System.String
                Get
                    Return _MthCode
                End Get
                Set(ByVal Value As System.String)
                    _MthCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RefNo As System.String
                Get
                    Return _RefNo
                End Get
                Set(ByVal Value As System.String)
                    _RefNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property StorageID As System.String
                Get
                    Return _StorageID
                End Get
                Set(ByVal Value As System.String)
                    _StorageID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Opening As System.Decimal
                Get
                    Return _Opening
                End Get
                Set(ByVal Value As System.Decimal)
                    _Opening = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property QtyIn As System.Decimal
                Get
                    Return _QtyIn
                End Get
                Set(ByVal Value As System.Decimal)
                    _QtyIn = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property QtyHandling As System.Decimal
                Get
                    Return _QtyHandling
                End Get
                Set(ByVal Value As System.Decimal)
                    _QtyHandling = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property QtyOut As System.Decimal
                Get
                    Return _QtyOut
                End Get
                Set(ByVal Value As System.Decimal)
                    _QtyOut = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property QtyReused As System.Decimal
                Get
                    Return _QtyReused
                End Get
                Set(ByVal Value As System.Decimal)
                    _QtyReused = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property QtyAdj As System.Decimal
                Get
                    Return _QtyAdj
                End Get
                Set(ByVal Value As System.Decimal)
                    _QtyAdj = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Closing As System.Decimal
                Get
                    Return _Closing
                End Get
                Set(ByVal Value As System.Decimal)
                    _Closing = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property YTDQty As System.Decimal
                Get
                    Return _YTDQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _YTDQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property MTDQty As System.Decimal
                Get
                    Return _MTDQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _MTDQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property FirstGenerated As System.DateTime
                Get
                    Return _FirstGenerated
                End Get
                Set(ByVal Value As System.DateTime)
                    _FirstGenerated = Value
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
            Public Property IsTrial As System.Byte
                Get
                    Return _IsTrial
                End Get
                Set(ByVal Value As System.Byte)
                    _IsTrial = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsReqCus As System.Byte
                Get
                    Return _IsReqCus
                End Get
                Set(ByVal Value As System.Byte)
                    _IsReqCus = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ShelfLife As System.Decimal
                Get
                    Return _ShelfLife
                End Get
                Set(ByVal Value As System.Decimal)
                    _ShelfLife = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property WarrantyPeriod As System.Decimal
                Get
                    Return _WarrantyPeriod
                End Get
                Set(ByVal Value As System.Decimal)
                    _WarrantyPeriod = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property WarrantyType As System.Byte
                Get
                    Return _WarrantyType
                End Get
                Set(ByVal Value As System.Byte)
                    _WarrantyType = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace
#End Region

#Region "Class Info"
#Region "Itemsummary Info"
    Public Class ItemsummaryInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "LocID,ItemCode,ItemName,YearCode,MthCode,RefNo,StorageID,Opening,QtyIn,QtyHandling,QtyOut,QtyReused,QtyAdj,Closing,YTDQty,MTDQty,FirstGenerated,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy,IsTrial,IsReqCus,ShelfLife,WarrantyPeriod,WarrantyType"
                .CheckFields = "IsHost,IsTrial,IsReqCus,WarrantyType"
                .TableName = "Itemsummary"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "LocID,ItemCode,ItemName,YearCode,MthCode,RefNo,StorageID,Opening,QtyIn,QtyHandling,QtyOut,QtyReused,QtyAdj,Closing,YTDQty,MTDQty,FirstGenerated,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy,IsTrial,IsReqCus,ShelfLife,WarrantyPeriod,WarrantyType"
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
#End Region

#Region "Scheme"
#Region "ITEMSUMMARY Scheme"
    Public Class ITEMSUMMARYScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LocID"
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
                .FieldName = "ItemName"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "YearCode"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "MthCode"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "RefNo"
                .Length = 28
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "StorageID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Opening"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "QtyIn"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "QtyHandling"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "QtyOut"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "QtyReused"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "QtyAdj"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Closing"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "YTDQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "MTDQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "FirstGenerated"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsTrial"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsReqCus"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(27, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ShelfLife"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(28, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "WarrantyPeriod"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(29, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "WarrantyType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(30, this)

        End Sub

        Public ReadOnly Property LocID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property ItemCode As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property ItemName As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property YearCode As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property MthCode As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property

        Public ReadOnly Property RefNo As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property StorageID As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property Opening As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property QtyIn As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property QtyHandling As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property QtyOut As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property QtyReused As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property QtyAdj As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property Closing As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property YTDQty As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property MTDQty As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property FirstGenerated As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property IsTrial As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property IsReqCus As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property
        Public ReadOnly Property ShelfLife As StrucElement
            Get
                Return MyBase.GetItem(28)
            End Get
        End Property
        Public ReadOnly Property WarrantyPeriod As StrucElement
            Get
                Return MyBase.GetItem(29)
            End Get
        End Property
        Public ReadOnly Property WarrantyType As StrucElement
            Get
                Return MyBase.GetItem(30)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace
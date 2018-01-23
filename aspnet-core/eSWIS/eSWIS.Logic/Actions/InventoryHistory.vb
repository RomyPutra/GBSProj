Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Actions
    Public NotInheritable Class InventoryHistory
        Inherits Core.CoreControl
        Private InventoriInfo As InventoriInfo = New InventoriInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal connecn As String)
            ConnectionString = connecn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal InventoriCont As Container.Inventori, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If InventoriCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With InventoriInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, InventoriCont.ID) & "' AND KOD_IVT = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, InventoriCont.KOD_IVT) & "' AND ID_INVENTORI = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, InventoriCont.ID_INVENTORI) & "'")
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
                                .TableName = "Inventori WITH (ROWLOCK)"
                                .AddField("KOD_UI", InventoriCont.KOD_UI, SQLControl.EnumDataType.dtString)
                                .AddField("IDLOGIN", InventoriCont.IDLOGIN, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IDPREMIS", InventoriCont.IDPREMIS, SQLControl.EnumDataType.dtString)
                                .AddField("TARIKH_PENGELUAR", InventoriCont.TARIKH_PENGELUAR, SQLControl.EnumDataType.dtCustom)
                                .AddField("BULAN", InventoriCont.BULAN, SQLControl.EnumDataType.dtString)
                                .AddField("TAHUN", InventoriCont.TAHUN, SQLControl.EnumDataType.dtString)
                                .AddField("KOD_BT", InventoriCont.KOD_BT, SQLControl.EnumDataType.dtString)
                                .AddField("NAMA_BT", InventoriCont.NAMA_BT, SQLControl.EnumDataType.dtStringN)
                                .AddField("ID_NOTI", InventoriCont.ID_NOTI, SQLControl.EnumDataType.dtNumeric)
                                .AddField("KOMPONEN_BT", InventoriCont.KOMPONEN_BT, SQLControl.EnumDataType.dtStringN)
                                .AddField("ASAL_BT", InventoriCont.ASAL_BT, SQLControl.EnumDataType.dtStringN)
                                .AddField("KUANTITI_PAKEJ_BT", InventoriCont.KUANTITI_PAKEJ_BT, SQLControl.EnumDataType.dtCustom)
                                .AddField("KUANTITI_METRIK", InventoriCont.KUANTITI_METRIK, SQLControl.EnumDataType.dtNumeric)
                                .AddField("KM1", InventoriCont.KM1, SQLControl.EnumDataType.dtNumeric)
                                .AddField("KUANTITI_M3", InventoriCont.KUANTITI_M3, SQLControl.EnumDataType.dtCustom)
                                .AddField("UNIT", InventoriCont.UNIT, SQLControl.EnumDataType.dtString)
                                .AddField("WH_METHOD", InventoriCont.WH_METHOD, SQLControl.EnumDataType.dtStringN)
                                .AddField("WH_QUANTITY", InventoriCont.WH_QUANTITY, SQLControl.EnumDataType.dtNumeric)
                                .AddField("WH_PLACE", InventoriCont.WH_PLACE, SQLControl.EnumDataType.dtStringN)
                                .AddField("STORAN", InventoriCont.STORAN, SQLControl.EnumDataType.dtStringN)
                                .AddField("NEGERI", InventoriCont.NEGERI, SQLControl.EnumDataType.dtString)
                                .AddField("DAERAH", InventoriCont.DAERAH, SQLControl.EnumDataType.dtString)
                                .AddField("NAMAPREMIS", InventoriCont.NAMAPREMIS, SQLControl.EnumDataType.dtStringN)
                                .AddField("ALAMAT", InventoriCont.ALAMAT, SQLControl.EnumDataType.dtStringN)
                                .AddField("TARIKH_WUJUD", InventoriCont.TARIKH_WUJUD, SQLControl.EnumDataType.dtCustom)
                                .AddField("WUJUD_PENGGUNA", InventoriCont.WUJUD_PENGGUNA, SQLControl.EnumDataType.dtStringN)
                                .AddField("KEMASKINI_PENGGUNA", InventoriCont.KEMASKINI_PENGGUNA, SQLControl.EnumDataType.dtDateTime)
                                .AddField("STAT", InventoriCont.STAT, SQLControl.EnumDataType.dtString)
                                .AddField("STATUS", InventoriCont.STATUS, SQLControl.EnumDataType.dtString)
                                .AddField("SIC", InventoriCont.SIC, SQLControl.EnumDataType.dtString)
                                .AddField("B_F", InventoriCont.B_F, SQLControl.EnumDataType.dtNumeric)
                                .AddField("C_F", InventoriCont.C_F, SQLControl.EnumDataType.dtNumeric)
                                .AddField("TRAN", InventoriCont.TRAN, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, InventoriCont.ID) & "' AND KOD_IVT = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, InventoriCont.KOD_IVT) & "' AND ID_INVENTORI = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, InventoriCont.ID_INVENTORI) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("ID", InventoriCont.ID, SQLControl.EnumDataType.dtNumeric)
                                                .AddField("KOD_IVT", InventoriCont.KOD_IVT, SQLControl.EnumDataType.dtString)
                                                .AddField("ID_INVENTORI", InventoriCont.ID_INVENTORI, SQLControl.EnumDataType.dtNumeric)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, InventoriCont.ID) & "' AND KOD_IVT = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, InventoriCont.KOD_IVT) & "' AND ID_INVENTORI = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, InventoriCont.ID_INVENTORI) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                    Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                                Else
                                    message = axExecute.Message.ToString()
                                    Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/InventoryHistory", axExecute.Message & strSQL, axExecute.StackTrace)
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If

                    End If
                End If
            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/InventoryHistory", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/InventoryHistory", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                InventoriCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal InventoriCont As Container.Inventori, ByRef message As String) As Boolean
            Return Save(InventoriCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal InventoriCont As Container.Inventori, ByRef message As String) As Boolean
            Return Save(InventoriCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal InventoriCont As Container.Inventori, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If InventoriCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With InventoriInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, InventoriCont.ID) & "' AND KOD_IVT = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, InventoriCont.KOD_IVT) & "' AND ID_INVENTORI = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, InventoriCont.ID_INVENTORI) & "'")
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
                                strSQL = BuildUpdate("Inventori WITH (ROWLOCK)", " SET Flag = 0" & _
                                 "' WHERE" & _
                                "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, InventoriCont.ID) & "' AND KOD_IVT = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, InventoriCont.KOD_IVT) & "' AND ID_INVENTORI = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, InventoriCont.ID_INVENTORI) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("Inventori WITH (ROWLOCK)", "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, InventoriCont.ID) & "' AND KOD_IVT = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, InventoriCont.KOD_IVT) & "' AND ID_INVENTORI = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, InventoriCont.ID_INVENTORI) & "'")
                        End If

                        Try
                            'execute
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Actions/InventoryHistory", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/InventoryHistory", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/InventoryHistory", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                InventoriCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region

#Region "Data Selection"

        Public Overloads Function GetInventoryHistory(ByVal ID As System.Int32, ByVal KOD_IVT As System.String, ByVal ID_INVENTORI As System.Int32) As Container.Inventori
            Dim rInventori As Container.Inventori = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With InventoriInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ID) & "' AND KOD_IVT = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, KOD_IVT) & "' AND ID_INVENTORI = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ID_INVENTORI) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rInventori = New Container.Inventori
                                rInventori.ID = drRow.Item("ID")
                                rInventori.KOD_IVT = drRow.Item("KOD_IVT")
                                rInventori.ID_INVENTORI = drRow.Item("ID_INVENTORI")
                            Else
                                rInventori = Nothing
                            End If
                        Else
                            rInventori = Nothing
                        End If
                    End With
                End If
                Return rInventori
            Catch ex As Exception
                Throw ex
            Finally
                rInventori = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetInventoryHistory(ByVal ID As System.Int32, ByVal KOD_IVT As System.String, ByVal ID_INVENTORI As System.Int32, DecendingOrder As Boolean) As List(Of Container.Inventori)
            Dim rInventori As Container.Inventori = Nothing
            Dim lstInventori As List(Of Container.Inventori) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With InventoriInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal ID As System.Int32, ByVal KOD_IVT As System.String, ByVal ID_INVENTORI As System.Int32 DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "ID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ID) & "' AND KOD_IVT = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, KOD_IVT) & "' AND ID_INVENTORI = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ID_INVENTORI) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rInventori = New Container.Inventori
                                rInventori.ID = drRow.Item("ID")
                                rInventori.KOD_IVT = drRow.Item("KOD_IVT")
                                rInventori.ID_INVENTORI = drRow.Item("ID_INVENTORI")
                            Next
                            lstInventori.Add(rInventori)
                        Else
                            rInventori = Nothing
                        End If
                        Return lstInventori
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rInventori = Nothing
                lstInventori = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetInventoryHistoryList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With InventoriInfo.MyInfo
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

        Public Overloads Function GetInventoryHistoryShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With InventoriInfo.MyInfo
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

        Public Overloads Function strSelComSummaryInventory(ByRef str As String, ByVal val1 As String, ByVal val2 As String, ByVal opt As String)
            Select Case opt

                Case "expired"
                    str = "SELECT il.ItemName, il.ExpiredQty, il.ExpiryDate, (it.QtyAdj-isnull(R.RejectedQty,0)) as QtyAdjust, E.CompanyName as CompanyName, it.ItemCode, b.Address1 as CityCode," & _
                          " '' as StorageArea ,  '' as Bin , " & _
                          " isnull(H.HandlingQty,0) as OutgoingQty , it.Opening as LastIn,   it.Opening as QtyOnHand, " & _
                          " it.Closing AS ClosingQty, " & _
                          " it.Opening as LastOut, it.QtyIn as IncomingQty,it.QtyReused " & _
                          " ,  it.Opening+IncomingQty-(QtyReused+(it.QtyAdj-isnull(R.RejectedQty,0))+OutgoingQty) as Balance " & _
                          " FROM ITEMSUMMARY it WITH (NOLOCK) " & _
                          " LEFT JOIN  BIZLOCATE B WITH (NOLOCK) on b.BizLocID=it.LocID " & _
                          " LEFT JOIN  BIZENTITY E WITH (NOLOCK) on b.BizRegID=E.BizRegID " & _
                          " LEFT JOIN ITEMLOC IL WITH (NOLOCK) on it.LocID=il.LocID and it.ItemCode=il.ItemCode" & _
                          " OUTER APPLY (SELECT SUM(d.QTY) HandlingQty FROM CONSIGNDTL d WITH (NOLOCK) " & _
                          " LEFT JOIN CONSIGNHDR h WITH (NOLOCK) ON h.CONTRACTNO = d.CONTRACTNO AND h.FLAG = 1 AND h.STATUS <> 0 AND h.STATUS <> 2 AND h.STATUS <> 9 AND h.STATUS <> 12 AND h.STATUS <> 13 AND h.Status <> 14 AND h.STATUS <> 15 AND h.ISCONFIRM = 1 /*isconfirm=1 --> release (deduct)*/ LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODE = d.OPERATIONTYPE AND cm.CODETYPE='WTH' WHERE YEAR(h.TRANSDATE) = it.YearCode AND MONTH(h.TRANSDATE) = it.MthCode AND h.GENERATORLOCID = it.LocID AND d.WASTECODE = it.ItemCode	AND d.WASTEDESCRIPTION = it.ItemName) H" & _
                          " OUTER APPLY (SELECT SUM(d.QTY) RejectedQty FROM CONSIGNDTL d WITH (NOLOCK) LEFT JOIN CONSIGNHDR h WITH (NOLOCK) ON h.CONTRACTNO = d.CONTRACTNO AND h.FLAG = 1 AND h.STATUS = 9 AND h.ISCONFIRM = 1 /*Rejected CN*/ LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODE = d.OPERATIONTYPE AND cm.CODETYPE='WTH' WHERE YEAR(h.TRANSDATE) = it.YearCode AND MONTH(h.TRANSDATE) = it.MthCode AND h.GENERATORLOCID = it.LocID AND d.WASTECODE = it.ItemCode	AND d.WASTEDESCRIPTION = it.ItemName) R" & _
                          " WHERE " & val1 & " AND il.ItemName=it.ItemName AND it.IsHost=0 " & _
                          " And ExpiredQty > 0 And ExpiryDate < DateAdd(Day, -180, GETDATE())" & _
                          " Order by it.ItemCode ASC"

                Case "filter240day"
                    str = "SELECT il.ItemCode, il.ItemName, E.CompanyName as CompanyName,  b.Address1 as CityCode, case when il.QtyOnHand < D.ExpiredQty then il.QtyOnHand else D.ExpiredQty end as ExpiredQty, D.ExpiryDate " & _
                          " ,0 as QtyAdjust, '' as StorageArea , '' as Bin, 0 as OutgoingQty , 0 as LastIn, 0 as QtyOnHand, 0 AS ClosingQty, 0 as LastOut, 0 as IncomingQty,0 as QtyReused , 0 as Balance   " & _
                          " FROM ITEMLOC il  INNER JOIN  ITMTRANSHDR H WITH (NOLOCK) on il.LocID = H.LocID " & _
                          " INNER JOIN ITMTRANSDTL D WITH (NOLOCK) on H.DOCCODE = D.DocCode And D.ExpiredQty > 0  And il.QtyOnHand >0 And TransDate <= DateAdd(Day, -240, CONVERT(datetime, '" & val1 & "', 110)) and il.ItemCode=d.itemcode and il.ItemName=d.ItemName " & _
                          " LEFT JOIN  BIZLOCATE B WITH (NOLOCK) on b.BizLocID=il.LocID  " & _
                          " INNER JOIN  BIZENTITY E WITH (NOLOCK) on b.BizRegID=E.BizRegID    " & _
                          " WHERE " & val2 & _
                          " and h.posted=1 and h.status=1 and h.termid=0 and h.TransType=0 " & _
                          " ORDER BY il.ItemCode ASC  "

                Case "filter180day"
                    str = "SELECT il.ItemCode, il.ItemName, E.CompanyName as CompanyName,  b.Address1 as CityCode, case when il.QtyOnHand < D.ExpiredQty then il.QtyOnHand else D.ExpiredQty end as ExpiredQty, D.ExpiryDate " & _
                          " ,0 as QtyAdjust, '' as StorageArea , '' as Bin, 0 as OutgoingQty , 0 as LastIn, 0 as QtyOnHand, 0 AS ClosingQty, 0 as LastOut, 0 as IncomingQty,0 as QtyReused , 0 as Balance   " & _
                          " FROM ITEMLOC il " & _
                          " INNER JOIN  ITMTRANSHDR H WITH (NOLOCK) on il.LocID = H.LocID " & _
                          " INNER JOIN ITMTRANSDTL D WITH (NOLOCK) on H.DOCCODE = D.DocCode And D.ExpiredQty > 0  And il.QtyOnHand >0 And TransDate <= DateAdd(Day, -180, CONVERT(datetime, '" & val1 & "', 110)) And  TransDate >= DateAdd(Day, -240, CONVERT(datetime, '" & val1 & "', 110)) and il.ItemCode=d.itemcode and il.ItemName=d.ItemName " & _
                          " LEFT JOIN  BIZLOCATE B WITH (NOLOCK) on b.BizLocID=il.LocID  " & _
                          " INNER JOIN  BIZENTITY E WITH (NOLOCK) on b.BizRegID=E.BizRegID    " & _
                          " WHERE " & val2 & _
                          " and h.posted=1 and h.status=1 and h.termid=0 and h.TransType=0 " & _
                          " ORDER BY il.ItemCode ASC  "

                Case "filter20MT"
                    str = "SELECT it.ItemCode, it.ItemName, il.ExpiredQty, il.ExpiryDate, (it.QtyAdj-isnull(R.RejectedQty,0)) as QtyAdjust, E.CompanyName as CompanyName, b.Address1 as CityCode," & _
                          " '' as StorageArea ,  '' as Bin , " & _
                          " isnull(H.HandlingQty,0) as OutgoingQty , it.Opening as LastIn,   it.Opening as QtyOnHand, " & _
                          " it.Closing  AS ClosingQty, " & _
                          " it.Opening as LastOut, it.QtyIn as IncomingQty,it.QtyReused " & _
                          " ,  it.Opening+IncomingQty-(QtyReused+(it.QtyAdj-isnull(R.RejectedQty,0))+OutgoingQty) as Balance " & _
                          " FROM ITEMSUMMARY it WITH (NOLOCK) " & _
                          " INNER JOIN ITEMLOC il WITH (NOLOCK) on it.LocID=il.LocID and it.ItemCode=il.ItemCode AND il.ItemName=it.ItemName" & _
                          " LEFT JOIN  BIZLOCATE B WITH (NOLOCK) on b.BizLocID=it.LocID " & _
                          " LEFT JOIN  BIZENTITY E WITH (NOLOCK) on b.BizRegID=E.BizRegID " & _
                          " OUTER APPLY (SELECT SUM(d.QTY) HandlingQty FROM CONSIGNDTL d WITH (NOLOCK) LEFT JOIN CONSIGNHDR h WITH (NOLOCK) ON h.CONTRACTNO = d.CONTRACTNO AND h.FLAG = 1 AND h.STATUS <> 0 AND h.STATUS <> 2 AND h.STATUS <> 9 AND h.STATUS <> 12 AND h.STATUS <> 13 AND h.Status <> 14 AND h.STATUS <> 15 AND h.ISCONFIRM = 1 /*isconfirm=1 --> release (deduct)*/ LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODE = d.OPERATIONTYPE AND cm.CODETYPE='WTH' WHERE YEAR(h.TRANSDATE) = it.YearCode AND MONTH(h.TRANSDATE) = it.MthCode AND h.GENERATORLOCID = it.LocID AND d.WASTECODE = it.ItemCode	AND d.WASTEDESCRIPTION = it.ItemName) H" & _
                          " OUTER APPLY (SELECT SUM(d.QTY) RejectedQty FROM CONSIGNDTL d WITH (NOLOCK) LEFT JOIN CONSIGNHDR h WITH (NOLOCK) ON h.CONTRACTNO = d.CONTRACTNO AND h.FLAG = 1 AND h.STATUS = 9 AND h.ISCONFIRM = 1 /*Rejected CN*/ LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODE = d.OPERATIONTYPE AND cm.CODETYPE='WTH' WHERE YEAR(h.TRANSDATE) = it.YearCode AND MONTH(h.TRANSDATE) = it.MthCode AND h.GENERATORLOCID = it.LocID AND d.WASTECODE = it.ItemCode	AND d.WASTEDESCRIPTION = it.ItemName) R" & _
                          " WHERE " & val1 & _
                          " AND it.Closing >20 " & _
                          " ORDER BY it.ItemCode ASC "

                Case "NotExpired"
                    str = "SELECT it.ItemCode, it.ItemName, il.ExpiredQty, il.ExpiryDate, (it.QtyAdj-isnull(R.RejectedQty,0)) as QtyAdjust, E.CompanyName as CompanyName, b.Address1 as CityCode," &
                          " '' as StorageArea ,  '' as Bin , " &
                          " isnull(H.HandlingQty,0) as OutgoingQty , it.Opening as LastIn,   it.Opening as QtyOnHand, " &
                          " it.Closing  AS ClosingQty, " &
                          " it.Opening as LastOut, it.QtyIn as IncomingQty,it.QtyReused " &
                          " ,  it.Opening+IncomingQty-(QtyReused+(it.QtyAdj-isnull(R.RejectedQty,0))+OutgoingQty) as Balance " &
                          " FROM ITEMSUMMARY it WITH (NOLOCK) " &
                          " INNER JOIN ITEMLOC il WITH (NOLOCK) on it.LocID=il.LocID and it.ItemCode=il.ItemCode AND il.ItemName=it.ItemName" &
                          " LEFT JOIN  BIZLOCATE B WITH (NOLOCK) on b.BizLocID=it.LocID " &
                          " LEFT JOIN  BIZENTITY E WITH (NOLOCK) on b.BizRegID=E.BizRegID " &
                          " OUTER APPLY (SELECT SUM(d.QTY) HandlingQty FROM CONSIGNDTL d WITH (NOLOCK) LEFT JOIN CONSIGNHDR h WITH (NOLOCK) ON h.CONTRACTNO = d.CONTRACTNO AND h.FLAG = 1 AND h.STATUS <> 0 AND h.STATUS <> 2 AND h.STATUS <> 9 /*AND h.STATUS <> 12*/ AND h.STATUS <> 13 AND h.Status <> 14 AND h.STATUS <> 15 AND h.ISCONFIRM = 1 /*isconfirm=1 --> release (deduct)*/ LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODE = d.OPERATIONTYPE AND cm.CODETYPE='WTH' WHERE YEAR(h.TRANSDATE) = it.YearCode AND MONTH(h.TRANSDATE) = it.MthCode AND h.GENERATORLOCID = it.LocID AND d.WASTECODE = it.ItemCode	AND d.WASTEDESCRIPTION = it.ItemName) H" &
                          " OUTER APPLY (SELECT SUM(d.QTY) RejectedQty FROM CONSIGNDTL d WITH (NOLOCK) LEFT JOIN CONSIGNHDR h WITH (NOLOCK) ON h.CONTRACTNO = d.CONTRACTNO AND h.FLAG = 1 AND h.STATUS = 9 AND h.ISCONFIRM = 1 /*Rejected CN*/ LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODE = d.OPERATIONTYPE AND cm.CODETYPE='WTH' WHERE YEAR(h.TRANSDATE) = it.YearCode AND MONTH(h.TRANSDATE) = it.MthCode AND h.GENERATORLOCID = it.LocID AND d.WASTECODE = it.ItemCode	AND d.WASTEDESCRIPTION = it.ItemName) R" &
                          " WHERE " & val1 &
                          " ORDER BY it.ItemCode ASC "

            End Select

            Return str
        End Function

        Public Overloads Function filterSummaryInventory(ByVal UsrSett As Boolean, ByVal paramLocation As String, ByVal paramCity As String, _
                                                         ByVal paramWaste As String, ByVal ParamMonth As String, ByVal ParamYear As String, _
                                                         ByVal inactive As String, ByRef strFilter As String, ByRef strfilter180240 As String)

            'Dim strfilter180240 As String = ""

            'Dim strFilter As String = "(it.IsHost=0 OR (it.IsHost=1 AND ((it.Closing<>0) OR (it.Closing=0 AND (it.QtyIn<>0 or isnull(H.HandlingQty,0)<>0 or it.QtyReused<>0)))))"
            strFilter = "(it.IsHost=0 OR (it.IsHost=1 AND ((it.Closing<>0) OR (it.Closing=0 AND (it.QtyIn<>0 or isnull(H.HandlingQty,0)<>0 or it.QtyReused<>0)))))"
            'If Not MyUserSet Is Nothing AndAlso MyUserSet.AppID = "1" Then
            If UsrSett = True Then
                If Not paramLocation Is Nothing AndAlso paramLocation <> "" Then
                    strFilter &= " AND it.LocID in (select BizLocID from BIZLOCATE WITH (NOLOCK) where State='" & paramLocation & "')"
                    strfilter180240 &= " il.LocID in (select BizLocID from BIZLOCATE WITH (NOLOCK) where State='" & paramLocation & "')"
                    If paramCity <> Nothing AndAlso paramCity <> "All" Then
                        strFilter &= " AND it.LocID in (select BizLocID from BIZLOCATE WITH (NOLOCK) where State='" & paramLocation & "' AND City = '" & paramCity & "')"
                        strfilter180240 &= " AND il.LocID in (select BizLocID from BIZLOCATE WITH (NOLOCK) where State='" & paramLocation & "' AND City = '" & paramCity & "')"
                    End If
                End If
                If paramWaste IsNot Nothing AndAlso paramWaste <> "All" Then
                    strFilter &= " AND it.ItemCode='" & paramWaste & "' "
                    strfilter180240 &= " AND il.ItemCode='" & paramWaste & "' "
                End If
            Else
                strFilter &= " AND it.LocID='" & paramLocation & "'"
            End If

            If Not ParamMonth Is Nothing AndAlso ParamMonth <> "" Then
                strFilter &= " AND MthCode='" & ParamMonth & "'"
            End If
            If Not ParamYear Is Nothing AndAlso ParamYear <> "" Then
                strFilter &= " AND YearCode='" & ParamYear & "'"
            End If
            If inactive = False Then
                strFilter &= " AND il.Flag=1 "
            End If

            Return strFilter & strfilter180240

        End Function


#End Region

    End Class


    Namespace Container
#Region "Class Container"
        Public Class Inventori
            Public fID As System.String = "ID"
            Public fKOD_IVT As System.String = "KOD_IVT"
            Public fID_INVENTORI As System.String = "ID_INVENTORI"
            Public fKOD_UI As System.String = "KOD_UI"
            Public fIDLOGIN As System.String = "IDLOGIN"
            Public fIDPREMIS As System.String = "IDPREMIS"
            Public fTARIKH_PENGELUAR As System.String = "TARIKH_PENGELUAR"
            Public fBULAN As System.String = "BULAN"
            Public fTAHUN As System.String = "TAHUN"
            Public fKOD_BT As System.String = "KOD_BT"
            Public fNAMA_BT As System.String = "NAMA_BT"
            Public fID_NOTI As System.String = "ID_NOTI"
            Public fKOMPONEN_BT As System.String = "KOMPONEN_BT"
            Public fASAL_BT As System.String = "ASAL_BT"
            Public fKUANTITI_PAKEJ_BT As System.String = "KUANTITI_PAKEJ_BT"
            Public fKUANTITI_METRIK As System.String = "KUANTITI_METRIK"
            Public fKM1 As System.String = "KM1"
            Public fKUANTITI_M3 As System.String = "KUANTITI_M3"
            Public fUNIT As System.String = "UNIT"
            Public fWH_METHOD As System.String = "WH_METHOD"
            Public fWH_QUANTITY As System.String = "WH_QUANTITY"
            Public fWH_PLACE As System.String = "WH_PLACE"
            Public fSTORAN As System.String = "STORAN"
            Public fNEGERI As System.String = "NEGERI"
            Public fDAERAH As System.String = "DAERAH"
            Public fNAMAPREMIS As System.String = "NAMAPREMIS"
            Public fALAMAT As System.String = "ALAMAT"
            Public fTARIKH_WUJUD As System.String = "TARIKH_WUJUD"
            Public fWUJUD_PENGGUNA As System.String = "WUJUD_PENGGUNA"
            Public fKEMASKINI_PENGGUNA As System.String = "KEMASKINI_PENGGUNA"
            Public fSTAT As System.String = "STAT"
            Public fSTATUS As System.String = "STATUS"
            Public fSIC As System.String = "SIC"
            Public fB_F As System.String = "B_F"
            Public fC_F As System.String = "C_F"
            Public fTRAN As System.String = "TRAN"

            Protected _ID As System.Int32
            Protected _KOD_IVT As System.String
            Protected _ID_INVENTORI As System.Int32
            Private _KOD_UI As System.String
            Private _IDLOGIN As System.Int32
            Private _IDPREMIS As System.String
            Private _TARIKH_PENGELUAR As System.Object
            Private _BULAN As System.String
            Private _TAHUN As System.String
            Private _KOD_BT As System.String
            Private _NAMA_BT As System.String
            Private _ID_NOTI As System.Int32
            Private _KOMPONEN_BT As System.String
            Private _ASAL_BT As System.String
            Private _KUANTITI_PAKEJ_BT As System.Single
            Private _KUANTITI_METRIK As System.Decimal
            Private _KM1 As System.Decimal
            Private _KUANTITI_M3 As System.Single
            Private _UNIT As System.String
            Private _WH_METHOD As System.String
            Private _WH_QUANTITY As System.Decimal
            Private _WH_PLACE As System.String
            Private _STORAN As System.String
            Private _NEGERI As System.String
            Private _DAERAH As System.String
            Private _NAMAPREMIS As System.String
            Private _ALAMAT As System.String
            Private _TARIKH_WUJUD As System.Object
            Private _WUJUD_PENGGUNA As System.String
            Private _KEMASKINI_PENGGUNA As System.DateTime
            Private _STAT As System.String
            Private _STATUS As System.String
            Private _SIC As System.String
            Private _B_F As System.Decimal
            Private _C_F As System.Decimal
            Private _TRAN As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ID As System.Int32
                Get
                    Return _ID
                End Get
                Set(ByVal Value As System.Int32)
                    _ID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property KOD_IVT As System.String
                Get
                    Return _KOD_IVT
                End Get
                Set(ByVal Value As System.String)
                    _KOD_IVT = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ID_INVENTORI As System.Int32
                Get
                    Return _ID_INVENTORI
                End Get
                Set(ByVal Value As System.Int32)
                    _ID_INVENTORI = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property KOD_UI As System.String
                Get
                    Return _KOD_UI
                End Get
                Set(ByVal Value As System.String)
                    _KOD_UI = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property IDLOGIN As System.Int32
                Get
                    Return _IDLOGIN
                End Get
                Set(ByVal Value As System.Int32)
                    _IDLOGIN = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property IDPREMIS As System.String
                Get
                    Return _IDPREMIS
                End Get
                Set(ByVal Value As System.String)
                    _IDPREMIS = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property TARIKH_PENGELUAR As System.Object
                Get
                    Return _TARIKH_PENGELUAR
                End Get
                Set(ByVal Value As System.Object)
                    _TARIKH_PENGELUAR = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property BULAN As System.String
                Get
                    Return _BULAN
                End Get
                Set(ByVal Value As System.String)
                    _BULAN = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property TAHUN As System.String
                Get
                    Return _TAHUN
                End Get
                Set(ByVal Value As System.String)
                    _TAHUN = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property KOD_BT As System.String
                Get
                    Return _KOD_BT
                End Get
                Set(ByVal Value As System.String)
                    _KOD_BT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property NAMA_BT As System.String
                Get
                    Return _NAMA_BT
                End Get
                Set(ByVal Value As System.String)
                    _NAMA_BT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property ID_NOTI As System.Int32
                Get
                    Return _ID_NOTI
                End Get
                Set(ByVal Value As System.Int32)
                    _ID_NOTI = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property KOMPONEN_BT As System.String
                Get
                    Return _KOMPONEN_BT
                End Get
                Set(ByVal Value As System.String)
                    _KOMPONEN_BT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property ASAL_BT As System.String
                Get
                    Return _ASAL_BT
                End Get
                Set(ByVal Value As System.String)
                    _ASAL_BT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property KUANTITI_PAKEJ_BT As System.Single
                Get
                    Return _KUANTITI_PAKEJ_BT
                End Get
                Set(ByVal Value As System.Single)
                    _KUANTITI_PAKEJ_BT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property KUANTITI_METRIK As System.Decimal
                Get
                    Return _KUANTITI_METRIK
                End Get
                Set(ByVal Value As System.Decimal)
                    _KUANTITI_METRIK = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property KM1 As System.Decimal
                Get
                    Return _KM1
                End Get
                Set(ByVal Value As System.Decimal)
                    _KM1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property KUANTITI_M3 As System.Single
                Get
                    Return _KUANTITI_M3
                End Get
                Set(ByVal Value As System.Single)
                    _KUANTITI_M3 = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property UNIT As System.String
                Get
                    Return _UNIT
                End Get
                Set(ByVal Value As System.String)
                    _UNIT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property WH_METHOD As System.String
                Get
                    Return _WH_METHOD
                End Get
                Set(ByVal Value As System.String)
                    _WH_METHOD = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property WH_QUANTITY As System.Decimal
                Get
                    Return _WH_QUANTITY
                End Get
                Set(ByVal Value As System.Decimal)
                    _WH_QUANTITY = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property WH_PLACE As System.String
                Get
                    Return _WH_PLACE
                End Get
                Set(ByVal Value As System.String)
                    _WH_PLACE = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property STORAN As System.String
                Get
                    Return _STORAN
                End Get
                Set(ByVal Value As System.String)
                    _STORAN = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property NEGERI As System.String
                Get
                    Return _NEGERI
                End Get
                Set(ByVal Value As System.String)
                    _NEGERI = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property DAERAH As System.String
                Get
                    Return _DAERAH
                End Get
                Set(ByVal Value As System.String)
                    _DAERAH = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property NAMAPREMIS As System.String
                Get
                    Return _NAMAPREMIS
                End Get
                Set(ByVal Value As System.String)
                    _NAMAPREMIS = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property ALAMAT As System.String
                Get
                    Return _ALAMAT
                End Get
                Set(ByVal Value As System.String)
                    _ALAMAT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property TARIKH_WUJUD As System.Object
                Get
                    Return _TARIKH_WUJUD
                End Get
                Set(ByVal Value As System.Object)
                    _TARIKH_WUJUD = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property WUJUD_PENGGUNA As System.String
                Get
                    Return _WUJUD_PENGGUNA
                End Get
                Set(ByVal Value As System.String)
                    _WUJUD_PENGGUNA = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property KEMASKINI_PENGGUNA As System.DateTime
                Get
                    Return _KEMASKINI_PENGGUNA
                End Get
                Set(ByVal Value As System.DateTime)
                    _KEMASKINI_PENGGUNA = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property STAT As System.String
                Get
                    Return _STAT
                End Get
                Set(ByVal Value As System.String)
                    _STAT = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property STATUS As System.String
                Get
                    Return _STATUS
                End Get
                Set(ByVal Value As System.String)
                    _STATUS = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property SIC As System.String
                Get
                    Return _SIC
                End Get
                Set(ByVal Value As System.String)
                    _SIC = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property B_F As System.Decimal
                Get
                    Return _B_F
                End Get
                Set(ByVal Value As System.Decimal)
                    _B_F = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property C_F As System.Decimal
                Get
                    Return _C_F
                End Get
                Set(ByVal Value As System.Decimal)
                    _C_F = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property TRAN As System.String
                Get
                    Return _TRAN
                End Get
                Set(ByVal Value As System.String)
                    _TRAN = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class InventoriInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "ID,KOD_IVT,ID_INVENTORI,KOD_UI,IDLOGIN,IDPREMIS,TARIKH_PENGELUAR,BULAN,TAHUN,KOD_BT,NAMA_BT,ID_NOTI,KOMPONEN_BT,ASAL_BT,KUANTITI_PAKEJ_BT,KUANTITI_METRIK,KM1,KUANTITI_M3,UNIT,WH_METHOD,WH_QUANTITY,WH_PLACE,STORAN,NEGERI,DAERAH,NAMAPREMIS,ALAMAT,TARIKH_WUJUD,WUJUD_PENGGUNA,KEMASKINI_PENGGUNA,STAT,STATUS,SIC,B_F,C_F,TRAN"
                .CheckFields = ""
                .TableName = "Inventori WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "ID,KOD_IVT,ID_INVENTORI,KOD_UI,IDLOGIN,IDPREMIS,TARIKH_PENGELUAR,BULAN,TAHUN,KOD_BT,NAMA_BT,ID_NOTI,KOMPONEN_BT,ASAL_BT,KUANTITI_PAKEJ_BT,KUANTITI_METRIK,KM1,KUANTITI_M3,UNIT,WH_METHOD,WH_QUANTITY,WH_PLACE,STORAN,NEGERI,DAERAH,NAMAPREMIS,ALAMAT,TARIKH_WUJUD,WUJUD_PENGGUNA,KEMASKINI_PENGGUNA,STAT,STATUS,SIC,B_F,C_F,TRAN"
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
    Public Class InventoryHistoryScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ID"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "KOD_IVT"
                .Length = 30
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ID_INVENTORI"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "KOD_UI"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IDLOGIN"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "IDPREMIS"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "TARIKH_PENGELUAR"
                .Length = 6
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BULAN"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TAHUN"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "KOD_BT"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "NAMA_BT"
                .Length = 60
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ID_NOTI"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "KOMPONEN_BT"
                .Length = 60
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ASAL_BT"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "KUANTITI_PAKEJ_BT"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "KUANTITI_METRIK"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "KM1"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "KUANTITI_M3"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UNIT"
                .Length = 15
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "WH_METHOD"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "WH_QUANTITY"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "WH_PLACE"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "STORAN"
                .Length = 60
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "NEGERI"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "DAERAH"
                .Length = 25
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "NAMAPREMIS"
                .Length = 60
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ALAMAT"
                .Length = 100
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "TARIKH_WUJUD"
                .Length = 6
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(27, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "WUJUD_PENGGUNA"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(28, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "KEMASKINI_PENGGUNA"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(29, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "STAT"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(30, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "STATUS"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(31, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "SIC"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(32, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "B_F"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(33, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "C_F"
                .Length = 13
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(34, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TRAN"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(35, this)

        End Sub

        Public ReadOnly Property ID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property KOD_IVT As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property ID_INVENTORI As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property

        Public ReadOnly Property KOD_UI As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property IDLOGIN As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property IDPREMIS As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property TARIKH_PENGELUAR As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property BULAN As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property TAHUN As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property KOD_BT As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property NAMA_BT As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property ID_NOTI As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property KOMPONEN_BT As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property ASAL_BT As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property KUANTITI_PAKEJ_BT As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property KUANTITI_METRIK As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property KM1 As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property KUANTITI_M3 As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property UNIT As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property WH_METHOD As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property WH_QUANTITY As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property WH_PLACE As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property STORAN As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property NEGERI As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property DAERAH As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property NAMAPREMIS As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property ALAMAT As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property TARIKH_WUJUD As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property
        Public ReadOnly Property WUJUD_PENGGUNA As StrucElement
            Get
                Return MyBase.GetItem(28)
            End Get
        End Property
        Public ReadOnly Property KEMASKINI_PENGGUNA As StrucElement
            Get
                Return MyBase.GetItem(29)
            End Get
        End Property
        Public ReadOnly Property STAT As StrucElement
            Get
                Return MyBase.GetItem(30)
            End Get
        End Property
        Public ReadOnly Property STATUS As StrucElement
            Get
                Return MyBase.GetItem(31)
            End Get
        End Property
        Public ReadOnly Property SIC As StrucElement
            Get
                Return MyBase.GetItem(32)
            End Get
        End Property
        Public ReadOnly Property B_F As StrucElement
            Get
                Return MyBase.GetItem(33)
            End Get
        End Property
        Public ReadOnly Property C_F As StrucElement
            Get
                Return MyBase.GetItem(34)
            End Get
        End Property
        Public ReadOnly Property TRAN As StrucElement
            Get
                Return MyBase.GetItem(35)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace
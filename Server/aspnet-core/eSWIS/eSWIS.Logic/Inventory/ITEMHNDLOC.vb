Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared


Namespace Inventory
#Region "ITEMHNDLOC Class"
    Public NotInheritable Class ITEMHNDLOC
        Inherits Core.CoreControl
        Private ItemhndlocInfo As ItemhndlocInfo = New ItemhndlocInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal ItemhndlocCont As Container.Itemhndloc, ByVal TWGRESCont As TWG.Container.Twg_reshdr, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ItemhndlocCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItemhndlocInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "LocID = '" & ItemhndlocCont.LocID & "' AND ItemCode = '" & ItemhndlocCont.ItemCode & "' AND ItemName = '" & ItemhndlocCont.ItemName & "'")
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
                            'Throw New ApplicationException("210011: Record already exist")
                            'Item found & active
                            'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIconInformation,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
                            'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "Itemhndloc"
                                .AddField("StorageID", ItemhndlocCont.StorageID, SQLControl.EnumDataType.dtString)
                                .AddField("ItemDesc", ItemhndlocCont.ItemDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("ItemName", ItemhndlocCont.ItemName, SQLControl.EnumDataType.dtStringN)
                                .AddField("ShortDesc", ItemhndlocCont.ShortDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("ItemComponent", ItemhndlocCont.ItemComponent, SQLControl.EnumDataType.dtStringN)
                                .AddField("TariffCode", ItemhndlocCont.TariffCode, SQLControl.EnumDataType.dtStringN)
                                .AddField("OrgCountry", ItemhndlocCont.OrgCountry, SQLControl.EnumDataType.dtString)
                                .AddField("MATNo", ItemhndlocCont.MATNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("MarkNo", ItemhndlocCont.MarkNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("ItmSize2", ItemhndlocCont.ItmSize2, SQLControl.EnumDataType.dtString)
                                .AddField("ItmSize1", ItemhndlocCont.ItmSize1, SQLControl.EnumDataType.dtString)
                                .AddField("ItmSize", ItemhndlocCont.ItmSize, SQLControl.EnumDataType.dtString)
                                .AddField("ConSize", ItemhndlocCont.ConSize, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ConUOM", ItemhndlocCont.ConUOM, SQLControl.EnumDataType.dtString)
                                .AddField("DefUOM", ItemhndlocCont.DefUOM, SQLControl.EnumDataType.dtString)
                                .AddField("ClassType", ItemhndlocCont.ClassType, SQLControl.EnumDataType.dtString)
                                '.AddField("TypeCode", ItemhndlocCont.TypeCode, SQLControl.EnumDataType.dtString)
                                .AddField("BehvType", ItemhndlocCont.BehvType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("BehvShow", ItemhndlocCont.BehvShow, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ComboItem", ItemhndlocCont.ComboItem, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ItmCatgCode", ItemhndlocCont.ItmCatgCode, SQLControl.EnumDataType.dtString)
                                .AddField("ItmBrand", ItemhndlocCont.ItmBrand, SQLControl.EnumDataType.dtString)
                                .AddField("LooseUOM", ItemhndlocCont.LooseUOM, SQLControl.EnumDataType.dtString)
                                .AddField("PackUOM", ItemhndlocCont.PackUOM, SQLControl.EnumDataType.dtString)
                                .AddField("PackQty", ItemhndlocCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsSales", ItemhndlocCont.IsSales, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsEmpDisc", ItemhndlocCont.IsEmpDisc, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsRtnable", ItemhndlocCont.IsRtnable, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsDisc", ItemhndlocCont.IsDisc, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsFOC", ItemhndlocCont.IsFOC, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsTaxable", ItemhndlocCont.IsTaxable, SQLControl.EnumDataType.dtNumeric)
                                .AddField("AvgCost", ItemhndlocCont.AvgCost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("StdCost", ItemhndlocCont.StdCost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("StdMarkup", ItemhndlocCont.StdMarkup, SQLControl.EnumDataType.dtNumeric)
                                .AddField("StdSellPrice", ItemhndlocCont.StdSellPrice, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsSelected", ItemhndlocCont.IsSelected, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsBestBuy", ItemhndlocCont.IsBestBuy, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsPurchase", ItemhndlocCont.IsPurchase, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsWLength", ItemhndlocCont.IsWLength, SQLControl.EnumDataType.dtNumeric)
                                .AddField("TrackSerial", ItemhndlocCont.TrackSerial, SQLControl.EnumDataType.dtNumeric)
                                .AddField("MinQty", ItemhndlocCont.MinQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("MaxQty", ItemhndlocCont.MaxQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ImageName", ItemhndlocCont.ImageName, SQLControl.EnumDataType.dtStringN)
                                .AddField("IncomingQty", ItemhndlocCont.IncomingQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ReOrderLvl", ItemhndlocCont.ReOrderLvl, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ReOrderQty", ItemhndlocCont.ReOrderQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("QtyOnHand", ItemhndlocCont.QtyOnHand, SQLControl.EnumDataType.dtNumeric)
                                .AddField("QtySellable", ItemhndlocCont.QtySellable, SQLControl.EnumDataType.dtNumeric)
                                .AddField("QtyBalance", ItemhndlocCont.QtyBalance, SQLControl.EnumDataType.dtNumeric)
                                .AddField("POQty", ItemhndlocCont.POQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OutgoingQty", ItemhndlocCont.OutgoingQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("QtyAdj", ItemhndlocCont.QtyAdj, SQLControl.EnumDataType.dtNumeric)
                                .AddField("FirstIn", ItemhndlocCont.FirstIn, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastIn", ItemhndlocCont.LastIn, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastPO", ItemhndlocCont.LastPO, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastOut", ItemhndlocCont.LastOut, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastRV", ItemhndlocCont.LastRV, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CummQty", ItemhndlocCont.CummQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("DayQty", ItemhndlocCont.DayQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LDayQty", ItemhndlocCont.LDayQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("MthQty", ItemhndlocCont.MthQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LMthQty", ItemhndlocCont.LMthQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("MtdQty", ItemhndlocCont.MtdQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("YrQty", ItemhndlocCont.YrQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LYrQty", ItemhndlocCont.LYrQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("YtdQty", ItemhndlocCont.YtdQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ExpiredQty", ItemhndlocCont.ExpiredQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ExpiryDate", ItemhndlocCont.ExpiryDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ProcessDate", ItemhndlocCont.ProcessDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateDate", ItemhndlocCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", ItemhndlocCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", ItemhndlocCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", ItemhndlocCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", ItemhndlocCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", ItemhndlocCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                '.AddField("rowguid", ItemhndlocCont.rowguid, SQLControl.EnumDataType.dtString)
                                .AddField("SyncCreate", ItemhndlocCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", ItemhndlocCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("DailySync", ItemhndlocCont.DailySync, SQLControl.EnumDataType.dtDateTime)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & ItemhndlocCont.LocID & "' AND ItemCode = '" & ItemhndlocCont.ItemCode & "' AND TypeCode = '" & ItemhndlocCont.TypeCode & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("LocID", ItemhndlocCont.LocID, SQLControl.EnumDataType.dtString)
                                                .AddField("ItemCode", ItemhndlocCont.ItemCode, SQLControl.EnumDataType.dtString)
                                                '.AddField("ItemName", ItemhndlocCont.ItemName, SQLControl.EnumDataType.dtStringN)
                                                .AddField("TypeCode", ItemhndlocCont.TypeCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & ItemhndlocCont.LocID & "' AND ItemCode = '" & ItemhndlocCont.ItemCode & "' AND TypeCode = '" & ItemhndlocCont.TypeCode & "'")
                                End Select
                                BatchList.Add(strSQL)
                            End With

                            If TWGRESCont IsNot Nothing Then
                                With objSQL
                                    .TableName = "Twg_reshdr"
                                    .AddField("CreateDate", TWGRESCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateBy", TWGRESCont.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("LastUpdate", TWGRESCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("UpdateBy", TWGRESCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("Active", TWGRESCont.Active, SQLControl.EnumDataType.dtNumeric)
                                    '.AddField("WasteCode", TWGRESCont.WasteCode, SQLControl.EnumDataType.dtString)
                                    '.AddField("WasteType", TWGRESCont.WasteType, SQLControl.EnumDataType.dtString)
                                    .AddField("WasteName", TWGRESCont.WasteName, SQLControl.EnumDataType.dtString)

                                    Select Case pType
                                        'Case SQLControl.EnumSQLType.stInsert
                                        '    If blnFound = True And blnFlag = False Then
                                        '        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & Twg_reshdrCont.DocCode & "' AND ReceiverID = '" & Twg_reshdrCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_reshdrCont.ReceiverLocID & "'")
                                        '    Else
                                        '        If blnFound = False Then
                                        '            .AddField("DocCode", Twg_reshdrCont.DocCode, SQLControl.EnumDataType.dtString)
                                        '            .AddField("ReceiverID", Twg_reshdrCont.ReceiverID, SQLControl.EnumDataType.dtString)
                                        '            .AddField("ReceiverLocID", Twg_reshdrCont.ReceiverLocID, SQLControl.EnumDataType.dtString)
                                        '            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                        '        End If
                                        '    End If
                                        Case SQLControl.EnumSQLType.stUpdate
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ReceiverLocID = '" & TWGRESCont.ReceiverLocID & "' AND WasteCode = '" & ItemhndlocCont.ItemCode & "' AND WasteType = '" & TWGRESCont.WasteType & "'")
                                            BatchList.Add(strSQL)
                                    End Select
                                End With

                                With objSQL
                                    .TableName = "HandleSummary"
                                    .AddField("CreateDate", TWGRESCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateBy", TWGRESCont.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("LastUpdate", TWGRESCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("UpdateBy", TWGRESCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                    '.AddField("WasteCode", TWGRESCont.WasteCode, SQLControl.EnumDataType.dtString)
                                    '.AddField("WasteType", TWGRESCont.WasteType, SQLControl.EnumDataType.dtString)
                                    .AddField("ItemName", TWGRESCont.WasteName, SQLControl.EnumDataType.dtString)

                                    Select Case pType
                                        'Case SQLControl.EnumSQLType.stInsert
                                        '    If blnFound = True And blnFlag = False Then
                                        '        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & Twg_reshdrCont.DocCode & "' AND ReceiverID = '" & Twg_reshdrCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_reshdrCont.ReceiverLocID & "'")
                                        '    Else
                                        '        If blnFound = False Then
                                        '            .AddField("DocCode", Twg_reshdrCont.DocCode, SQLControl.EnumDataType.dtString)
                                        '            .AddField("ReceiverID", Twg_reshdrCont.ReceiverID, SQLControl.EnumDataType.dtString)
                                        '            .AddField("ReceiverLocID", Twg_reshdrCont.ReceiverLocID, SQLControl.EnumDataType.dtString)
                                        '            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                        '        End If
                                        '    End If
                                        Case SQLControl.EnumSQLType.stUpdate
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & TWGRESCont.ReceiverLocID & "' AND ItemCode = '" & ItemhndlocCont.ItemCode & "' AND ItemName = '" & TWGRESCont.OldWasteCode & "'")
                                            BatchList.Add(strSQL)
                                    End Select
                                End With
                            End If
                            
                            Try
                                If BatchExecute Then
                                    If Commit Then
                                        objConn.BatchExecute(BatchList, CommandType.Text, True)
                                    End If
                                Else
                                    'execute
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
                'Throw axAssign
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Return False
            Finally
                ItemhndlocCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal ItemhndlocCont As Container.Itemhndloc, ByVal TWGRESCont As TWG.Container.Twg_reshdr, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(ItemhndlocCont, TWGRESCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        Private Function SaveBatch(ByVal ListItemhndLoc As List(Of Container.Itemhndloc), ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False, Optional ByVal LocID As String = "") As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            SaveBatch = False
            Try
                If ListItemhndLoc Is Nothing OrElse ListItemhndLoc.Count = 0 Then
                    If LocID <> "" Then
                        StartSQLControl()

                        'Message return
                        With objSQL
                            .TableName = "Itemhndloc WITH (ROWLOCK)"
                            strSQL = BuildDelete(.TableName, "LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "'")
                            BatchList.Add(strSQL)
                        End With

                        Try
                            If BatchExecute Then
                                If Commit Then
                                    objConn.BatchExecute(BatchList, CommandType.Text, True)
                                End If
                            Else
                                'execute
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
                    End If


                    Return True
                Else
                    StartSQLControl()

                    With objSQL
                        .TableName = "Itemhndloc WITH (ROWLOCK)"
                        strSQL = BuildDelete(.TableName, "LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ListItemhndLoc(0).LocID) & "' AND TypeCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ListItemhndLoc(0).TypeCode) & "' AND ItemName = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ListItemhndLoc(0).ItemName) & "'")
                        BatchList.Add(strSQL)
                    End With
                    For Each BizItemhndLoc In ListItemhndLoc
                        With objSQL
                            .TableName = "Itemhndloc"
                            .AddField("StorageID", BizItemhndLoc.StorageID, SQLControl.EnumDataType.dtString)
                            .AddField("ItemDesc", BizItemhndLoc.ItemDesc, SQLControl.EnumDataType.dtStringN)
                            .AddField("ItemName", BizItemhndLoc.ItemName, SQLControl.EnumDataType.dtStringN)
                            .AddField("ShortDesc", BizItemhndLoc.ShortDesc, SQLControl.EnumDataType.dtStringN)
                            .AddField("ItemComponent", BizItemhndLoc.ItemComponent, SQLControl.EnumDataType.dtStringN)
                            .AddField("TariffCode", BizItemhndLoc.TariffCode, SQLControl.EnumDataType.dtStringN)
                            .AddField("OrgCountry", BizItemhndLoc.OrgCountry, SQLControl.EnumDataType.dtString)
                            .AddField("MATNo", BizItemhndLoc.MATNo, SQLControl.EnumDataType.dtStringN)
                            .AddField("MarkNo", BizItemhndLoc.MarkNo, SQLControl.EnumDataType.dtStringN)
                            .AddField("ItmSize2", BizItemhndLoc.ItmSize2, SQLControl.EnumDataType.dtString)
                            .AddField("ItmSize1", BizItemhndLoc.ItmSize1, SQLControl.EnumDataType.dtString)
                            .AddField("ItmSize", BizItemhndLoc.ItmSize, SQLControl.EnumDataType.dtString)
                            .AddField("ConSize", BizItemhndLoc.ConSize, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ConUOM", BizItemhndLoc.ConUOM, SQLControl.EnumDataType.dtString)
                            .AddField("DefUOM", BizItemhndLoc.DefUOM, SQLControl.EnumDataType.dtString)
                            .AddField("ClassType", BizItemhndLoc.ClassType, SQLControl.EnumDataType.dtString)
                            '.AddField("TypeCode", ItemhndlocCont.TypeCode, SQLControl.EnumDataType.dtString)
                            .AddField("BehvType", BizItemhndLoc.BehvType, SQLControl.EnumDataType.dtNumeric)
                            .AddField("BehvShow", BizItemhndLoc.BehvShow, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ComboItem", BizItemhndLoc.ComboItem, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ItmCatgCode", BizItemhndLoc.ItmCatgCode, SQLControl.EnumDataType.dtString)
                            .AddField("ItmBrand", BizItemhndLoc.ItmBrand, SQLControl.EnumDataType.dtString)
                            .AddField("LooseUOM", BizItemhndLoc.LooseUOM, SQLControl.EnumDataType.dtString)
                            .AddField("PackUOM", BizItemhndLoc.PackUOM, SQLControl.EnumDataType.dtString)
                            .AddField("PackQty", BizItemhndLoc.PackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsSales", BizItemhndLoc.IsSales, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsEmpDisc", BizItemhndLoc.IsEmpDisc, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsRtnable", BizItemhndLoc.IsRtnable, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsDisc", BizItemhndLoc.IsDisc, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsFOC", BizItemhndLoc.IsFOC, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsTaxable", BizItemhndLoc.IsTaxable, SQLControl.EnumDataType.dtNumeric)
                            .AddField("AvgCost", BizItemhndLoc.AvgCost, SQLControl.EnumDataType.dtNumeric)
                            .AddField("StdCost", BizItemhndLoc.StdCost, SQLControl.EnumDataType.dtNumeric)
                            .AddField("StdMarkup", BizItemhndLoc.StdMarkup, SQLControl.EnumDataType.dtNumeric)
                            .AddField("StdSellPrice", BizItemhndLoc.StdSellPrice, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsSelected", BizItemhndLoc.IsSelected, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsBestBuy", BizItemhndLoc.IsBestBuy, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsPurchase", BizItemhndLoc.IsPurchase, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsWLength", BizItemhndLoc.IsWLength, SQLControl.EnumDataType.dtNumeric)
                            .AddField("TrackSerial", BizItemhndLoc.TrackSerial, SQLControl.EnumDataType.dtNumeric)
                            .AddField("MinQty", BizItemhndLoc.MinQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("MaxQty", BizItemhndLoc.MaxQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ImageName", BizItemhndLoc.ImageName, SQLControl.EnumDataType.dtStringN)
                            .AddField("IncomingQty", BizItemhndLoc.IncomingQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ReOrderLvl", BizItemhndLoc.ReOrderLvl, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ReOrderQty", BizItemhndLoc.ReOrderQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("QtyOnHand", BizItemhndLoc.QtyOnHand, SQLControl.EnumDataType.dtNumeric)
                            .AddField("QtySellable", BizItemhndLoc.QtySellable, SQLControl.EnumDataType.dtNumeric)
                            .AddField("QtyBalance", BizItemhndLoc.QtyBalance, SQLControl.EnumDataType.dtNumeric)
                            .AddField("POQty", BizItemhndLoc.POQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OutgoingQty", BizItemhndLoc.OutgoingQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("QtyAdj", BizItemhndLoc.QtyAdj, SQLControl.EnumDataType.dtNumeric)
                            .AddField("FirstIn", BizItemhndLoc.FirstIn, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastIn", BizItemhndLoc.LastIn, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastPO", BizItemhndLoc.LastPO, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastOut", BizItemhndLoc.LastOut, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastRV", BizItemhndLoc.LastRV, SQLControl.EnumDataType.dtNumeric)
                            .AddField("CummQty", BizItemhndLoc.CummQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("DayQty", BizItemhndLoc.DayQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LDayQty", BizItemhndLoc.LDayQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("MthQty", BizItemhndLoc.MthQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LMthQty", BizItemhndLoc.LMthQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("MtdQty", BizItemhndLoc.MtdQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("YrQty", BizItemhndLoc.YrQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LYrQty", BizItemhndLoc.LYrQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("YtdQty", BizItemhndLoc.YtdQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ExpiredQty", BizItemhndLoc.ExpiredQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ExpiryDate", BizItemhndLoc.ExpiryDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("ProcessDate", BizItemhndLoc.ProcessDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateDate", BizItemhndLoc.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateBy", BizItemhndLoc.CreateBy, SQLControl.EnumDataType.dtString)
                            .AddField("LastUpdate", BizItemhndLoc.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("UpdateBy", BizItemhndLoc.UpdateBy, SQLControl.EnumDataType.dtString)
                            .AddField("Active", BizItemhndLoc.Active, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Inuse", BizItemhndLoc.Inuse, SQLControl.EnumDataType.dtNumeric)
                            '.AddField("rowguid", ItemhndlocCont.rowguid, SQLControl.EnumDataType.dtString)
                            .AddField("SyncCreate", BizItemhndLoc.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("SyncLastUpd", BizItemhndLoc.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                            .AddField("DailySync", BizItemhndLoc.DailySync, SQLControl.EnumDataType.dtDateTime)
                            'strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)

                            Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    If blnFound = True And blnFlag = False Then
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & BizItemhndLoc.LocID & "' AND ItemCode = '" & BizItemhndLoc.ItemCode & "' AND TypeCode = '" & BizItemhndLoc.TypeCode & "'")
                                    Else
                                        If blnFound = False Then
                                            .AddField("LocID", BizItemhndLoc.LocID, SQLControl.EnumDataType.dtString)
                                            .AddField("ItemCode", BizItemhndLoc.ItemCode, SQLControl.EnumDataType.dtString)
                                            .AddField("TypeCode", BizItemhndLoc.TypeCode, SQLControl.EnumDataType.dtString)
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                        End If
                                    End If
                                Case SQLControl.EnumSQLType.stUpdate
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & BizItemhndLoc.LocID & "' AND ItemCode = '" & BizItemhndLoc.ItemCode & "' AND TypeCode = '" & BizItemhndLoc.TypeCode & "'")
                            End Select
                        End With

                        BatchList.Add(strSQL)
                    Next

                    Try
                        If BatchExecute Then
                            If Commit Then
                                objConn.BatchExecute(BatchList, CommandType.Text, True)
                            End If
                        Else
                            'execute
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


            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Return False
            Finally
                ListItemhndLoc = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD WASTE WITH LIST
        Public Function InsertWaste(ByVal ListItemhndLoc As List(Of Container.Itemhndloc), ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False, Optional LocID As String = "") As Boolean
            Return SaveBatch(ListItemhndLoc, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit, LocID)
        End Function

        'AMEND
        Public Function Update(ByVal ItemhndlocCont As Container.Itemhndloc, ByVal TWGRESCont As TWG.Container.Twg_reshdr, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(ItemhndlocCont, TWGRESCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal ItemhndlocCont As Container.Itemhndloc, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If ItemhndlocCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItemhndlocInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "LocID = '" & ItemhndlocCont.LocID & "' AND ItemCode = '" & ItemhndlocCont.ItemCode & "' AND ItemName = '" & ItemhndlocCont.ItemName & "'")
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
                                strSQL = BuildUpdate(ItemhndlocInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = '" & ItemhndlocCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemhndlocCont.UpdateBy) & "' WHERE" & _
                                "LocID = '" & ItemhndlocCont.LocID & "' AND ItemCode = '" & ItemhndlocCont.ItemCode & "' AND ItemName = '" & ItemhndlocCont.ItemName & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(ItemhndlocInfo.MyInfo.TableName, "LocID = '" & ItemhndlocCont.LocID & "' AND ItemCode = '" & ItemhndlocCont.ItemCode & "' AND ItemName = '" & ItemhndlocCont.ItemName & "'")
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
                ItemhndlocCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetITEMHNDLOC(ByVal LocID As System.String, ByVal ItemCode As System.String, ByVal ItemType As System.String) As Container.Itemhndloc
            Dim rItemhndloc As Container.Itemhndloc = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With ItemhndlocInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "LocID = '" & LocID & "' AND ItemCode = '" & ItemCode & "' AND TypeCode = '" & ItemType & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rItemhndloc = New Container.Itemhndloc
                                rItemhndloc.LocID = drRow.Item("LocID")
                                rItemhndloc.ItemCode = drRow.Item("ItemCode")
                                rItemhndloc.ItemName = drRow.Item("ItemName")
                                rItemhndloc.StorageID = drRow.Item("StorageID")
                                rItemhndloc.ItemDesc = drRow.Item("ItemDesc")
                                rItemhndloc.ShortDesc = drRow.Item("ShortDesc")
                                rItemhndloc.ItemComponent = drRow.Item("ItemComponent")
                                rItemhndloc.TariffCode = drRow.Item("TariffCode")
                                rItemhndloc.OrgCountry = drRow.Item("OrgCountry")
                                rItemhndloc.MATNo = drRow.Item("MATNo")
                                rItemhndloc.MarkNo = drRow.Item("MarkNo")
                                rItemhndloc.ItmSize2 = drRow.Item("ItmSize2")
                                rItemhndloc.ItmSize1 = drRow.Item("ItmSize1")
                                rItemhndloc.ItmSize = drRow.Item("ItmSize")
                                rItemhndloc.ConSize = drRow.Item("ConSize")
                                rItemhndloc.ConUOM = drRow.Item("ConUOM")
                                rItemhndloc.DefUOM = drRow.Item("DefUOM")
                                rItemhndloc.ClassType = drRow.Item("ClassType")
                                rItemhndloc.TypeCode = drRow.Item("TypeCode")
                                rItemhndloc.BehvType = drRow.Item("BehvType")
                                rItemhndloc.BehvShow = drRow.Item("BehvShow")
                                rItemhndloc.ComboItem = drRow.Item("ComboItem")
                                rItemhndloc.ItmCatgCode = drRow.Item("ItmCatgCode")
                                rItemhndloc.ItmBrand = drRow.Item("ItmBrand")
                                rItemhndloc.LooseUOM = drRow.Item("LooseUOM")
                                rItemhndloc.PackUOM = drRow.Item("PackUOM")
                                rItemhndloc.PackQty = drRow.Item("PackQty")
                                rItemhndloc.IsSales = drRow.Item("IsSales")
                                rItemhndloc.IsEmpDisc = drRow.Item("IsEmpDisc")
                                rItemhndloc.IsRtnable = drRow.Item("IsRtnable")
                                rItemhndloc.IsDisc = drRow.Item("IsDisc")
                                rItemhndloc.IsFOC = drRow.Item("IsFOC")
                                rItemhndloc.IsTaxable = drRow.Item("IsTaxable")
                                rItemhndloc.AvgCost = drRow.Item("AvgCost")
                                rItemhndloc.StdCost = drRow.Item("StdCost")
                                rItemhndloc.StdMarkup = drRow.Item("StdMarkup")
                                rItemhndloc.StdSellPrice = drRow.Item("StdSellPrice")
                                rItemhndloc.IsSelected = drRow.Item("IsSelected")
                                rItemhndloc.IsBestBuy = drRow.Item("IsBestBuy")
                                rItemhndloc.IsPurchase = drRow.Item("IsPurchase")
                                rItemhndloc.IsWLength = drRow.Item("IsWLength")
                                rItemhndloc.TrackSerial = drRow.Item("TrackSerial")
                                rItemhndloc.MinQty = drRow.Item("MinQty")
                                rItemhndloc.MaxQty = drRow.Item("MaxQty")
                                rItemhndloc.ImageName = drRow.Item("ImageName")
                                rItemhndloc.IncomingQty = drRow.Item("IncomingQty")
                                rItemhndloc.ReOrderLvl = drRow.Item("ReOrderLvl")
                                rItemhndloc.ReOrderQty = drRow.Item("ReOrderQty")
                                rItemhndloc.QtyOnHand = drRow.Item("QtyOnHand")
                                rItemhndloc.QtySellable = drRow.Item("QtySellable")
                                rItemhndloc.QtyBalance = drRow.Item("QtyBalance")
                                rItemhndloc.POQty = drRow.Item("POQty")
                                rItemhndloc.OutgoingQty = drRow.Item("OutgoingQty")
                                rItemhndloc.QtyAdj = drRow.Item("QtyAdj")
                                rItemhndloc.FirstIn = drRow.Item("FirstIn")
                                rItemhndloc.LastIn = drRow.Item("LastIn")
                                rItemhndloc.LastPO = drRow.Item("LastPO")
                                rItemhndloc.LastOut = drRow.Item("LastOut")
                                rItemhndloc.LastRV = drRow.Item("LastRV")
                                rItemhndloc.CummQty = drRow.Item("CummQty")
                                rItemhndloc.DayQty = drRow.Item("DayQty")
                                rItemhndloc.LDayQty = drRow.Item("LDayQty")
                                rItemhndloc.MthQty = drRow.Item("MthQty")
                                rItemhndloc.LMthQty = drRow.Item("LMthQty")
                                rItemhndloc.MtdQty = drRow.Item("MtdQty")
                                rItemhndloc.YrQty = drRow.Item("YrQty")
                                rItemhndloc.LYrQty = drRow.Item("LYrQty")
                                rItemhndloc.YtdQty = drRow.Item("YtdQty")
                                rItemhndloc.ExpiredQty = drRow.Item("ExpiredQty")
                                If Not IsDBNull(drRow.Item("ExpiryDate")) Then
                                    rItemhndloc.ExpiryDate = drRow.Item("ExpiryDate")
                                End If
                                If Not IsDBNull(drRow.Item("ProcessDate")) Then
                                    rItemhndloc.ProcessDate = drRow.Item("ProcessDate")
                                End If
                                rItemhndloc.CreateDate = drRow.Item("CreateDate")
                                rItemhndloc.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rItemhndloc.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rItemhndloc.UpdateBy = drRow.Item("UpdateBy")
                                rItemhndloc.Active = drRow.Item("Active")
                                rItemhndloc.Inuse = drRow.Item("Inuse")
                                rItemhndloc.rowguid = drRow.Item("rowguid")
                                rItemhndloc.SyncCreate = drRow.Item("SyncCreate")
                                rItemhndloc.SyncLastUpd = drRow.Item("SyncLastUpd")
                                If Not IsDBNull(drRow.Item("DailySync")) Then
                                    rItemhndloc.DailySync = drRow.Item("DailySync")
                                End If
                            Else
                                rItemhndloc = Nothing
                            End If
                        Else
                            rItemhndloc = Nothing
                        End If
                    End With
                End If
                Return rItemhndloc
            Catch ex As Exception
                Throw ex
            Finally
                rItemhndloc = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetITEMHNDLOC(ByVal LocID As System.String, ByVal ItemCode As System.String, ByVal ItemName As System.String, DecendingOrder As Boolean) As List(Of Container.Itemhndloc)
            Dim rItemhndloc As Container.Itemhndloc = Nothing
            Dim lstItemhndloc As List(Of Container.Itemhndloc) = New List(Of Container.Itemhndloc)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With ItemhndlocInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by LocID, ItemCode, ItemName DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "LocID = '" & LocID & "' AND ItemCode = '" & ItemCode & "' AND ItemName = '" & ItemName & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rItemhndloc = New Container.Itemhndloc
                                rItemhndloc.LocID = drRow.Item("LocID")
                                rItemhndloc.ItemCode = drRow.Item("ItemCode")
                                rItemhndloc.ItemName = drRow.Item("ItemName")
                                rItemhndloc.StorageID = drRow.Item("StorageID")
                                rItemhndloc.ItemDesc = drRow.Item("ItemDesc")
                                rItemhndloc.ShortDesc = drRow.Item("ShortDesc")
                                rItemhndloc.ItemComponent = drRow.Item("ItemComponent")
                                rItemhndloc.TariffCode = drRow.Item("TariffCode")
                                rItemhndloc.OrgCountry = drRow.Item("OrgCountry")
                                rItemhndloc.MATNo = drRow.Item("MATNo")
                                rItemhndloc.MarkNo = drRow.Item("MarkNo")
                                rItemhndloc.ItmSize2 = drRow.Item("ItmSize2")
                                rItemhndloc.ItmSize1 = drRow.Item("ItmSize1")
                                rItemhndloc.ItmSize = drRow.Item("ItmSize")
                                rItemhndloc.ConSize = drRow.Item("ConSize")
                                rItemhndloc.ConUOM = drRow.Item("ConUOM")
                                rItemhndloc.DefUOM = drRow.Item("DefUOM")
                                rItemhndloc.ClassType = drRow.Item("ClassType")
                                rItemhndloc.TypeCode = drRow.Item("TypeCode")
                                rItemhndloc.BehvType = drRow.Item("BehvType")
                                rItemhndloc.BehvShow = drRow.Item("BehvShow")
                                rItemhndloc.ComboItem = drRow.Item("ComboItem")
                                rItemhndloc.ItmCatgCode = drRow.Item("ItmCatgCode")
                                rItemhndloc.ItmBrand = drRow.Item("ItmBrand")
                                rItemhndloc.LooseUOM = drRow.Item("LooseUOM")
                                rItemhndloc.PackUOM = drRow.Item("PackUOM")
                                rItemhndloc.PackQty = drRow.Item("PackQty")
                                rItemhndloc.IsSales = drRow.Item("IsSales")
                                rItemhndloc.IsEmpDisc = drRow.Item("IsEmpDisc")
                                rItemhndloc.IsRtnable = drRow.Item("IsRtnable")
                                rItemhndloc.IsDisc = drRow.Item("IsDisc")
                                rItemhndloc.IsFOC = drRow.Item("IsFOC")
                                rItemhndloc.IsTaxable = drRow.Item("IsTaxable")
                                rItemhndloc.AvgCost = drRow.Item("AvgCost")
                                rItemhndloc.StdCost = drRow.Item("StdCost")
                                rItemhndloc.StdMarkup = drRow.Item("StdMarkup")
                                rItemhndloc.StdSellPrice = drRow.Item("StdSellPrice")
                                rItemhndloc.IsSelected = drRow.Item("IsSelected")
                                rItemhndloc.IsBestBuy = drRow.Item("IsBestBuy")
                                rItemhndloc.IsPurchase = drRow.Item("IsPurchase")
                                rItemhndloc.IsWLength = drRow.Item("IsWLength")
                                rItemhndloc.TrackSerial = drRow.Item("TrackSerial")
                                rItemhndloc.MinQty = drRow.Item("MinQty")
                                rItemhndloc.MaxQty = drRow.Item("MaxQty")
                                rItemhndloc.ImageName = drRow.Item("ImageName")
                                rItemhndloc.IncomingQty = drRow.Item("IncomingQty")
                                rItemhndloc.ReOrderLvl = drRow.Item("ReOrderLvl")
                                rItemhndloc.ReOrderQty = drRow.Item("ReOrderQty")
                                rItemhndloc.QtyOnHand = drRow.Item("QtyOnHand")
                                rItemhndloc.QtySellable = drRow.Item("QtySellable")
                                rItemhndloc.QtyBalance = drRow.Item("QtyBalance")
                                rItemhndloc.POQty = drRow.Item("POQty")
                                rItemhndloc.OutgoingQty = drRow.Item("OutgoingQty")
                                rItemhndloc.QtyAdj = drRow.Item("QtyAdj")
                                rItemhndloc.FirstIn = drRow.Item("FirstIn")
                                rItemhndloc.LastIn = drRow.Item("LastIn")
                                rItemhndloc.LastPO = drRow.Item("LastPO")
                                rItemhndloc.LastOut = drRow.Item("LastOut")
                                rItemhndloc.LastRV = drRow.Item("LastRV")
                                rItemhndloc.CummQty = drRow.Item("CummQty")
                                rItemhndloc.DayQty = drRow.Item("DayQty")
                                rItemhndloc.LDayQty = drRow.Item("LDayQty")
                                rItemhndloc.MthQty = drRow.Item("MthQty")
                                rItemhndloc.LMthQty = drRow.Item("LMthQty")
                                rItemhndloc.MtdQty = drRow.Item("MtdQty")
                                rItemhndloc.YrQty = drRow.Item("YrQty")
                                rItemhndloc.LYrQty = drRow.Item("LYrQty")
                                rItemhndloc.YtdQty = drRow.Item("YtdQty")
                                rItemhndloc.ExpiredQty = drRow.Item("ExpiredQty")
                                rItemhndloc.CreateDate = drRow.Item("CreateDate")
                                rItemhndloc.CreateBy = drRow.Item("CreateBy")
                                rItemhndloc.UpdateBy = drRow.Item("UpdateBy")
                                rItemhndloc.Active = drRow.Item("Active")
                                rItemhndloc.Inuse = drRow.Item("Inuse")
                                rItemhndloc.rowguid = drRow.Item("rowguid")
                                rItemhndloc.SyncCreate = drRow.Item("SyncCreate")
                                rItemhndloc.SyncLastUpd = drRow.Item("SyncLastUpd")
                                lstItemhndloc.Add(rItemhndloc)
                            Next

                        Else
                            rItemhndloc = Nothing
                        End If
                        Return lstItemhndloc
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rItemhndloc = Nothing
                lstItemhndloc = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetITEMHNDLOCListByLocID(ByVal locid As String) As Data.DataTable
            If StartConnection() = True Then
                With ItemhndlocInfo.MyInfo
                    'If SQL = Nothing Or SQL = String.Empty Then
                    '    strSQL = BuildSelect(.FieldsList, .TableName, FieldCond)
                    'Else
                    '    strSQL = SQL
                    'End If
                    strSQL = "select ItemCode, ItemName, TypeCode,  CodeDesc as WasteType, CreateDate, CreateBy, LastUpdate, UpdateBy" &
                            " from itemhndloc i left join codemaster c on i.TypeCode = c.Code and CodeType = 'WTY'" &
                            " where locid = '" & locid & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetITEMHNDLOCListByGlobal(ByVal locid As String, ByVal ItemCode As String, ByVal ItemName As String, ByVal ItemType As String) As Data.DataTable
            If StartConnection() = True Then
                With ItemhndlocInfo.MyInfo
                    'If SQL = Nothing Or SQL = String.Empty Then
                    '    strSQL = BuildSelect(.FieldsList, .TableName, FieldCond)
                    'Else
                    '    strSQL = SQL
                    'End If
                    strSQL = "select ItemCode, ItemName, TypeCode,  CodeDesc as WasteType, CreateDate, CreateBy, LastUpdate, UpdateBy" &
                            " from itemhndloc i left join codemaster c on i.TypeCode = c.Code and CodeType = 'WTY'" &
                            " where locid = '" & locid & "' and ItemCode = '" & ItemCode & "' and ItemName = '" & ItemName & "' and TypeCode = '" & ItemType & "' "
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetITEMHNDLOCList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ItemhndlocInfo.MyInfo
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

        Public Overloads Function GetITEMHNDLOCShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With ItemhndlocInfo.MyInfo
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


#End Region
    End Class
#End Region

#Region "Container"
    Namespace Container
#Region "Itemhndloc Container"
        Public Class Itemhndloc_FieldName
            Public LocID As System.String = "LocID"
            Public ItemCode As System.String = "ItemCode"
            Public ItemName As System.String = "ItemName"
            Public StorageID As System.String = "StorageID"
            Public ItemDesc As System.String = "ItemDesc"
            Public ShortDesc As System.String = "ShortDesc"
            Public ItemComponent As System.String = "ItemComponent"
            Public TariffCode As System.String = "TariffCode"
            Public OrgCountry As System.String = "OrgCountry"
            Public MATNo As System.String = "MATNo"
            Public MarkNo As System.String = "MarkNo"
            Public ItmSize2 As System.String = "ItmSize2"
            Public ItmSize1 As System.String = "ItmSize1"
            Public ItmSize As System.String = "ItmSize"
            Public ConSize As System.String = "ConSize"
            Public ConUOM As System.String = "ConUOM"
            Public DefUOM As System.String = "DefUOM"
            Public ClassType As System.String = "ClassType"
            Public TypeCode As System.String = "TypeCode"
            Public BehvType As System.String = "BehvType"
            Public BehvShow As System.String = "BehvShow"
            Public ComboItem As System.String = "ComboItem"
            Public ItmCatgCode As System.String = "ItmCatgCode"
            Public ItmBrand As System.String = "ItmBrand"
            Public LooseUOM As System.String = "LooseUOM"
            Public PackUOM As System.String = "PackUOM"
            Public PackQty As System.String = "PackQty"
            Public IsSales As System.String = "IsSales"
            Public IsEmpDisc As System.String = "IsEmpDisc"
            Public IsRtnable As System.String = "IsRtnable"
            Public IsDisc As System.String = "IsDisc"
            Public IsFOC As System.String = "IsFOC"
            Public IsTaxable As System.String = "IsTaxable"
            Public AvgCost As System.String = "AvgCost"
            Public StdCost As System.String = "StdCost"
            Public StdMarkup As System.String = "StdMarkup"
            Public StdSellPrice As System.String = "StdSellPrice"
            Public IsSelected As System.String = "IsSelected"
            Public IsBestBuy As System.String = "IsBestBuy"
            Public IsPurchase As System.String = "IsPurchase"
            Public IsWLength As System.String = "IsWLength"
            Public TrackSerial As System.String = "TrackSerial"
            Public MinQty As System.String = "MinQty"
            Public MaxQty As System.String = "MaxQty"
            Public ImageName As System.String = "ImageName"
            Public IncomingQty As System.String = "IncomingQty"
            Public ReOrderLvl As System.String = "ReOrderLvl"
            Public ReOrderQty As System.String = "ReOrderQty"
            Public QtyOnHand As System.String = "QtyOnHand"
            Public QtySellable As System.String = "QtySellable"
            Public QtyBalance As System.String = "QtyBalance"
            Public POQty As System.String = "POQty"
            Public OutgoingQty As System.String = "OutgoingQty"
            Public QtyAdj As System.String = "QtyAdj"
            Public FirstIn As System.String = "FirstIn"
            Public LastIn As System.String = "LastIn"
            Public LastPO As System.String = "LastPO"
            Public LastOut As System.String = "LastOut"
            Public LastRV As System.String = "LastRV"
            Public CummQty As System.String = "CummQty"
            Public DayQty As System.String = "DayQty"
            Public LDayQty As System.String = "LDayQty"
            Public MthQty As System.String = "MthQty"
            Public LMthQty As System.String = "LMthQty"
            Public MtdQty As System.String = "MtdQty"
            Public YrQty As System.String = "YrQty"
            Public LYrQty As System.String = "LYrQty"
            Public YtdQty As System.String = "YtdQty"
            Public ExpiredQty As System.String = "ExpiredQty"
            Public ExpiryDate As System.String = "ExpiryDate"
            Public ProcessDate As System.String = "ProcessDate"
            Public CreateDate As System.String = "CreateDate"
            Public CreateBy As System.String = "CreateBy"
            Public LastUpdate As System.String = "LastUpdate"
            Public UpdateBy As System.String = "UpdateBy"
            Public Active As System.String = "Active"
            Public Inuse As System.String = "Inuse"
            Public Flag As System.String = "Flag"
            Public rowguid As System.String = "rowguid"
            Public SyncCreate As System.String = "SyncCreate"
            Public SyncLastUpd As System.String = "SyncLastUpd"
            Public DailySync As System.String = "DailySync"
        End Class

        Public Class Itemhndloc
            Protected _LocID As System.String
            Protected _ItemCode As System.String
            Protected _ItemName As System.String
            Private _StorageID As System.String
            Private _ItemDesc As System.String
            Private _ShortDesc As System.String
            Private _ItemComponent As System.String
            Private _TariffCode As System.String
            Private _OrgCountry As System.String
            Private _MATNo As System.String
            Private _MarkNo As System.String
            Private _ItmSize2 As System.String
            Private _ItmSize1 As System.String
            Private _ItmSize As System.String
            Private _ConSize As System.Decimal
            Private _ConUOM As System.String
            Private _DefUOM As System.String
            Private _ClassType As System.String
            Private _TypeCode As System.String
            Private _BehvType As System.Byte
            Private _BehvShow As System.Byte
            Private _ComboItem As System.Int32
            Private _ItmCatgCode As System.String
            Private _ItmBrand As System.String
            Private _LooseUOM As System.String
            Private _PackUOM As System.String
            Private _PackQty As System.Decimal
            Private _IsSales As System.Byte
            Private _IsEmpDisc As System.Byte
            Private _IsRtnable As System.Byte
            Private _IsDisc As System.Byte
            Private _IsFOC As System.Byte
            Private _IsTaxable As System.Byte
            Private _AvgCost As System.Decimal
            Private _StdCost As System.Decimal
            Private _StdMarkup As System.Decimal
            Private _StdSellPrice As System.Decimal
            Private _IsSelected As System.Byte
            Private _IsBestBuy As System.Byte
            Private _IsPurchase As System.Byte
            Private _IsWLength As System.Byte
            Private _TrackSerial As System.Byte
            Private _MinQty As System.Decimal
            Private _MaxQty As System.Decimal
            Private _ImageName As System.String
            Private _IncomingQty As System.Decimal
            Private _ReOrderLvl As System.Decimal
            Private _ReOrderQty As System.Decimal
            Private _QtyOnHand As System.Decimal
            Private _QtySellable As System.Decimal
            Private _QtyBalance As System.Decimal
            Private _POQty As System.Decimal
            Private _OutgoingQty As System.Decimal
            Private _QtyAdj As System.Decimal
            Private _FirstIn As System.Decimal
            Private _LastIn As System.Decimal
            Private _LastPO As System.Decimal
            Private _LastOut As System.Decimal
            Private _LastRV As System.Decimal
            Private _CummQty As System.Decimal
            Private _DayQty As System.Decimal
            Private _LDayQty As System.Decimal
            Private _MthQty As System.Decimal
            Private _LMthQty As System.Decimal
            Private _MtdQty As System.Decimal
            Private _YrQty As System.Decimal
            Private _LYrQty As System.Decimal
            Private _YtdQty As System.Decimal
            Private _ExpiredQty As System.Decimal
            Private _ExpiryDate As System.DateTime
            Private _ProcessDate As System.DateTime
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _Active As System.Byte
            Private _Inuse As System.Byte
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _DailySync As System.DateTime

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
            Public Property ShortDesc As System.String
                Get
                    Return _ShortDesc
                End Get
                Set(ByVal Value As System.String)
                    _ShortDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ItemComponent As System.String
                Get
                    Return _ItemComponent
                End Get
                Set(ByVal Value As System.String)
                    _ItemComponent = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TariffCode As System.String
                Get
                    Return _TariffCode
                End Get
                Set(ByVal Value As System.String)
                    _TariffCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OrgCountry As System.String
                Get
                    Return _OrgCountry
                End Get
                Set(ByVal Value As System.String)
                    _OrgCountry = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property MATNo As System.String
                Get
                    Return _MATNo
                End Get
                Set(ByVal Value As System.String)
                    _MATNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property MarkNo As System.String
                Get
                    Return _MarkNo
                End Get
                Set(ByVal Value As System.String)
                    _MarkNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ItmSize2 As System.String
                Get
                    Return _ItmSize2
                End Get
                Set(ByVal Value As System.String)
                    _ItmSize2 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ItmSize1 As System.String
                Get
                    Return _ItmSize1
                End Get
                Set(ByVal Value As System.String)
                    _ItmSize1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ItmSize As System.String
                Get
                    Return _ItmSize
                End Get
                Set(ByVal Value As System.String)
                    _ItmSize = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ConSize As System.Decimal
                Get
                    Return _ConSize
                End Get
                Set(ByVal Value As System.Decimal)
                    _ConSize = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ConUOM As System.String
                Get
                    Return _ConUOM
                End Get
                Set(ByVal Value As System.String)
                    _ConUOM = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property DefUOM As System.String
                Get
                    Return _DefUOM
                End Get
                Set(ByVal Value As System.String)
                    _DefUOM = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ClassType As System.String
                Get
                    Return _ClassType
                End Get
                Set(ByVal Value As System.String)
                    _ClassType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TypeCode As System.String
                Get
                    Return _TypeCode
                End Get
                Set(ByVal Value As System.String)
                    _TypeCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property BehvType As System.Byte
                Get
                    Return _BehvType
                End Get
                Set(ByVal Value As System.Byte)
                    _BehvType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property BehvShow As System.Byte
                Get
                    Return _BehvShow
                End Get
                Set(ByVal Value As System.Byte)
                    _BehvShow = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ComboItem As System.Int32
                Get
                    Return _ComboItem
                End Get
                Set(ByVal Value As System.Int32)
                    _ComboItem = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ItmCatgCode As System.String
                Get
                    Return _ItmCatgCode
                End Get
                Set(ByVal Value As System.String)
                    _ItmCatgCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ItmBrand As System.String
                Get
                    Return _ItmBrand
                End Get
                Set(ByVal Value As System.String)
                    _ItmBrand = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LooseUOM As System.String
                Get
                    Return _LooseUOM
                End Get
                Set(ByVal Value As System.String)
                    _LooseUOM = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PackUOM As System.String
                Get
                    Return _PackUOM
                End Get
                Set(ByVal Value As System.String)
                    _PackUOM = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PackQty As System.Decimal
                Get
                    Return _PackQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _PackQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsSales As System.Byte
                Get
                    Return _IsSales
                End Get
                Set(ByVal Value As System.Byte)
                    _IsSales = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsEmpDisc As System.Byte
                Get
                    Return _IsEmpDisc
                End Get
                Set(ByVal Value As System.Byte)
                    _IsEmpDisc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsRtnable As System.Byte
                Get
                    Return _IsRtnable
                End Get
                Set(ByVal Value As System.Byte)
                    _IsRtnable = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsDisc As System.Byte
                Get
                    Return _IsDisc
                End Get
                Set(ByVal Value As System.Byte)
                    _IsDisc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsFOC As System.Byte
                Get
                    Return _IsFOC
                End Get
                Set(ByVal Value As System.Byte)
                    _IsFOC = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsTaxable As System.Byte
                Get
                    Return _IsTaxable
                End Get
                Set(ByVal Value As System.Byte)
                    _IsTaxable = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property AvgCost As System.Decimal
                Get
                    Return _AvgCost
                End Get
                Set(ByVal Value As System.Decimal)
                    _AvgCost = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property StdCost As System.Decimal
                Get
                    Return _StdCost
                End Get
                Set(ByVal Value As System.Decimal)
                    _StdCost = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property StdMarkup As System.Decimal
                Get
                    Return _StdMarkup
                End Get
                Set(ByVal Value As System.Decimal)
                    _StdMarkup = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property StdSellPrice As System.Decimal
                Get
                    Return _StdSellPrice
                End Get
                Set(ByVal Value As System.Decimal)
                    _StdSellPrice = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsSelected As System.Byte
                Get
                    Return _IsSelected
                End Get
                Set(ByVal Value As System.Byte)
                    _IsSelected = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsBestBuy As System.Byte
                Get
                    Return _IsBestBuy
                End Get
                Set(ByVal Value As System.Byte)
                    _IsBestBuy = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsPurchase As System.Byte
                Get
                    Return _IsPurchase
                End Get
                Set(ByVal Value As System.Byte)
                    _IsPurchase = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsWLength As System.Byte
                Get
                    Return _IsWLength
                End Get
                Set(ByVal Value As System.Byte)
                    _IsWLength = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TrackSerial As System.Byte
                Get
                    Return _TrackSerial
                End Get
                Set(ByVal Value As System.Byte)
                    _TrackSerial = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property MinQty As System.Decimal
                Get
                    Return _MinQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _MinQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property MaxQty As System.Decimal
                Get
                    Return _MaxQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _MaxQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ImageName As System.String
                Get
                    Return _ImageName
                End Get
                Set(ByVal Value As System.String)
                    _ImageName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IncomingQty As System.Decimal
                Get
                    Return _IncomingQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _IncomingQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ReOrderLvl As System.Decimal
                Get
                    Return _ReOrderLvl
                End Get
                Set(ByVal Value As System.Decimal)
                    _ReOrderLvl = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ReOrderQty As System.Decimal
                Get
                    Return _ReOrderQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _ReOrderQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property QtyOnHand As System.Decimal
                Get
                    Return _QtyOnHand
                End Get
                Set(ByVal Value As System.Decimal)
                    _QtyOnHand = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property QtySellable As System.Decimal
                Get
                    Return _QtySellable
                End Get
                Set(ByVal Value As System.Decimal)
                    _QtySellable = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property QtyBalance As System.Decimal
                Get
                    Return _QtyBalance
                End Get
                Set(ByVal Value As System.Decimal)
                    _QtyBalance = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property POQty As System.Decimal
                Get
                    Return _POQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _POQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OutgoingQty As System.Decimal
                Get
                    Return _OutgoingQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _OutgoingQty = Value
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
            Public Property FirstIn As System.Decimal
                Get
                    Return _FirstIn
                End Get
                Set(ByVal Value As System.Decimal)
                    _FirstIn = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LastIn As System.Decimal
                Get
                    Return _LastIn
                End Get
                Set(ByVal Value As System.Decimal)
                    _LastIn = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LastPO As System.Decimal
                Get
                    Return _LastPO
                End Get
                Set(ByVal Value As System.Decimal)
                    _LastPO = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LastOut As System.Decimal
                Get
                    Return _LastOut
                End Get
                Set(ByVal Value As System.Decimal)
                    _LastOut = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LastRV As System.Decimal
                Get
                    Return _LastRV
                End Get
                Set(ByVal Value As System.Decimal)
                    _LastRV = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CummQty As System.Decimal
                Get
                    Return _CummQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _CummQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property DayQty As System.Decimal
                Get
                    Return _DayQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _DayQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LDayQty As System.Decimal
                Get
                    Return _LDayQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _LDayQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property MthQty As System.Decimal
                Get
                    Return _MthQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _MthQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LMthQty As System.Decimal
                Get
                    Return _LMthQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _LMthQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property MtdQty As System.Decimal
                Get
                    Return _MtdQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _MtdQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property YrQty As System.Decimal
                Get
                    Return _YrQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _YrQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LYrQty As System.Decimal
                Get
                    Return _LYrQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _LYrQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property YtdQty As System.Decimal
                Get
                    Return _YtdQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _YtdQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ExpiredQty As System.Decimal
                Get
                    Return _ExpiredQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _ExpiredQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property ExpiryDate As System.DateTime
                Get
                    Return _ExpiryDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _ExpiryDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property ProcessDate As System.DateTime
                Get
                    Return _ProcessDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _ProcessDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
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
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property DailySync As System.DateTime
                Get
                    Return _DailySync
                End Get
                Set(ByVal Value As System.DateTime)
                    _DailySync = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace
#End Region

#Region "Class Info"
#Region "Itemhndloc Info"
    Public Class ItemhndlocInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "LocID,ItemCode,ItemName,StorageID,ItemDesc,ShortDesc,ItemComponent,TariffCode,OrgCountry,MATNo,MarkNo,ItmSize2,ItmSize1,ItmSize,ConSize,ConUOM,DefUOM,ClassType,TypeCode,BehvType,BehvShow,ComboItem,ItmCatgCode,ItmBrand,LooseUOM,PackUOM,PackQty,IsSales,IsEmpDisc,IsRtnable,IsDisc,IsFOC,IsTaxable,AvgCost,StdCost,StdMarkup,StdSellPrice,IsSelected,IsBestBuy,IsPurchase,IsWLength,TrackSerial,MinQty,MaxQty,ImageName,IncomingQty,ReOrderLvl,ReOrderQty,QtyOnHand,QtySellable,QtyBalance,POQty,OutgoingQty,QtyAdj,FirstIn,LastIn,LastPO,LastOut,LastRV,CummQty,DayQty,LDayQty,MthQty,LMthQty,MtdQty,YrQty,LYrQty,YtdQty,ExpiredQty,ExpiryDate,ProcessDate,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,DailySync"
                .CheckFields = "BehvType,BehvShow,IsSales,IsEmpDisc,IsRtnable,IsDisc,IsFOC,IsTaxable,IsSelected,IsBestBuy,IsPurchase,IsWLength,TrackSerial,Active,Inuse,Flag"
                .TableName = "Itemhndloc"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "LocID,ItemCode,ItemName,StorageID,ItemDesc,ShortDesc,ItemComponent,TariffCode,OrgCountry,MATNo,MarkNo,ItmSize2,ItmSize1,ItmSize,ConSize,ConUOM,DefUOM,ClassType,TypeCode,BehvType,BehvShow,ComboItem,ItmCatgCode,ItmBrand,LooseUOM,PackUOM,PackQty,IsSales,IsEmpDisc,IsRtnable,IsDisc,IsFOC,IsTaxable,AvgCost,StdCost,StdMarkup,StdSellPrice,IsSelected,IsBestBuy,IsPurchase,IsWLength,TrackSerial,MinQty,MaxQty,ImageName,IncomingQty,ReOrderLvl,ReOrderQty,QtyOnHand,QtySellable,QtyBalance,POQty,OutgoingQty,QtyAdj,FirstIn,LastIn,LastPO,LastOut,LastRV,CummQty,DayQty,LDayQty,MthQty,LMthQty,MtdQty,YrQty,LYrQty,YtdQty,ExpiredQty,ExpiryDate,ProcessDate,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,DailySync"
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
#Region "ITEMHNDLOC Scheme"
    Public Class ITEMHNDLOCScheme
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
                .FieldName = "StorageID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ItemDesc"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ShortDesc"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ItemComponent"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "TariffCode"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "OrgCountry"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "MATNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "MarkNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ItmSize2"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ItmSize1"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ItmSize"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ConSize"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ConUOM"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "DefUOM"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ClassType"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TypeCode"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "BehvType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "BehvShow"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ComboItem"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ItmCatgCode"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ItmBrand"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LooseUOM"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PackUOM"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "PackQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsSales"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(27, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsEmpDisc"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(28, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsRtnable"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(29, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsDisc"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(30, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsFOC"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(31, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsTaxable"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(32, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "AvgCost"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(33, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "StdCost"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(34, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "StdMarkup"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(35, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "StdSellPrice"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(36, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsSelected"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(37, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsBestBuy"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(38, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsPurchase"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(39, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsWLength"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(40, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "TrackSerial"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(41, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "MinQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(42, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "MaxQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(43, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ImageName"
                .Length = 255
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(44, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IncomingQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(45, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ReOrderLvl"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(46, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ReOrderQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(47, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "QtyOnHand"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(48, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "QtySellable"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(49, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "QtyBalance"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(50, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "POQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(51, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OutgoingQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(52, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "QtyAdj"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(53, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "FirstIn"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(54, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LastIn"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(55, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LastPO"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(56, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LastOut"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(57, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LastRV"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(58, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "CummQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(59, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "DayQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(60, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LDayQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(61, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "MthQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(62, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LMthQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(63, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "MtdQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(64, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "YrQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(65, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LYrQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(66, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "YtdQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(67, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ExpiredQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(68, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "ExpiryDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(69, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "ProcessDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(70, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(71, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(72, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(73, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(74, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(75, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(76, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(77, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(78, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(79, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(80, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "DailySync"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(81, this)

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

        Public ReadOnly Property StorageID As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property ItemDesc As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property ShortDesc As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property ItemComponent As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property TariffCode As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property OrgCountry As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property MATNo As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property MarkNo As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property ItmSize2 As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property ItmSize1 As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property ItmSize As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property ConSize As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property ConUOM As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property DefUOM As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property ClassType As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property TypeCode As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property BehvType As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property BehvShow As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property ComboItem As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property ItmCatgCode As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property ItmBrand As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property LooseUOM As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property PackUOM As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property PackQty As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property IsSales As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property
        Public ReadOnly Property IsEmpDisc As StrucElement
            Get
                Return MyBase.GetItem(28)
            End Get
        End Property
        Public ReadOnly Property IsRtnable As StrucElement
            Get
                Return MyBase.GetItem(29)
            End Get
        End Property
        Public ReadOnly Property IsDisc As StrucElement
            Get
                Return MyBase.GetItem(30)
            End Get
        End Property
        Public ReadOnly Property IsFOC As StrucElement
            Get
                Return MyBase.GetItem(31)
            End Get
        End Property
        Public ReadOnly Property IsTaxable As StrucElement
            Get
                Return MyBase.GetItem(32)
            End Get
        End Property
        Public ReadOnly Property AvgCost As StrucElement
            Get
                Return MyBase.GetItem(33)
            End Get
        End Property
        Public ReadOnly Property StdCost As StrucElement
            Get
                Return MyBase.GetItem(34)
            End Get
        End Property
        Public ReadOnly Property StdMarkup As StrucElement
            Get
                Return MyBase.GetItem(35)
            End Get
        End Property
        Public ReadOnly Property StdSellPrice As StrucElement
            Get
                Return MyBase.GetItem(36)
            End Get
        End Property
        Public ReadOnly Property IsSelected As StrucElement
            Get
                Return MyBase.GetItem(37)
            End Get
        End Property
        Public ReadOnly Property IsBestBuy As StrucElement
            Get
                Return MyBase.GetItem(38)
            End Get
        End Property
        Public ReadOnly Property IsPurchase As StrucElement
            Get
                Return MyBase.GetItem(39)
            End Get
        End Property
        Public ReadOnly Property IsWLength As StrucElement
            Get
                Return MyBase.GetItem(40)
            End Get
        End Property
        Public ReadOnly Property TrackSerial As StrucElement
            Get
                Return MyBase.GetItem(41)
            End Get
        End Property
        Public ReadOnly Property MinQty As StrucElement
            Get
                Return MyBase.GetItem(42)
            End Get
        End Property
        Public ReadOnly Property MaxQty As StrucElement
            Get
                Return MyBase.GetItem(43)
            End Get
        End Property
        Public ReadOnly Property ImageName As StrucElement
            Get
                Return MyBase.GetItem(44)
            End Get
        End Property
        Public ReadOnly Property IncomingQty As StrucElement
            Get
                Return MyBase.GetItem(45)
            End Get
        End Property
        Public ReadOnly Property ReOrderLvl As StrucElement
            Get
                Return MyBase.GetItem(46)
            End Get
        End Property
        Public ReadOnly Property ReOrderQty As StrucElement
            Get
                Return MyBase.GetItem(47)
            End Get
        End Property
        Public ReadOnly Property QtyOnHand As StrucElement
            Get
                Return MyBase.GetItem(48)
            End Get
        End Property
        Public ReadOnly Property QtySellable As StrucElement
            Get
                Return MyBase.GetItem(49)
            End Get
        End Property
        Public ReadOnly Property QtyBalance As StrucElement
            Get
                Return MyBase.GetItem(50)
            End Get
        End Property
        Public ReadOnly Property POQty As StrucElement
            Get
                Return MyBase.GetItem(51)
            End Get
        End Property
        Public ReadOnly Property OutgoingQty As StrucElement
            Get
                Return MyBase.GetItem(52)
            End Get
        End Property
        Public ReadOnly Property QtyAdj As StrucElement
            Get
                Return MyBase.GetItem(53)
            End Get
        End Property
        Public ReadOnly Property FirstIn As StrucElement
            Get
                Return MyBase.GetItem(54)
            End Get
        End Property
        Public ReadOnly Property LastIn As StrucElement
            Get
                Return MyBase.GetItem(55)
            End Get
        End Property
        Public ReadOnly Property LastPO As StrucElement
            Get
                Return MyBase.GetItem(56)
            End Get
        End Property
        Public ReadOnly Property LastOut As StrucElement
            Get
                Return MyBase.GetItem(57)
            End Get
        End Property
        Public ReadOnly Property LastRV As StrucElement
            Get
                Return MyBase.GetItem(58)
            End Get
        End Property
        Public ReadOnly Property CummQty As StrucElement
            Get
                Return MyBase.GetItem(59)
            End Get
        End Property
        Public ReadOnly Property DayQty As StrucElement
            Get
                Return MyBase.GetItem(60)
            End Get
        End Property
        Public ReadOnly Property LDayQty As StrucElement
            Get
                Return MyBase.GetItem(61)
            End Get
        End Property
        Public ReadOnly Property MthQty As StrucElement
            Get
                Return MyBase.GetItem(62)
            End Get
        End Property
        Public ReadOnly Property LMthQty As StrucElement
            Get
                Return MyBase.GetItem(63)
            End Get
        End Property
        Public ReadOnly Property MtdQty As StrucElement
            Get
                Return MyBase.GetItem(64)
            End Get
        End Property
        Public ReadOnly Property YrQty As StrucElement
            Get
                Return MyBase.GetItem(65)
            End Get
        End Property
        Public ReadOnly Property LYrQty As StrucElement
            Get
                Return MyBase.GetItem(66)
            End Get
        End Property
        Public ReadOnly Property YtdQty As StrucElement
            Get
                Return MyBase.GetItem(67)
            End Get
        End Property
        Public ReadOnly Property ExpiredQty As StrucElement
            Get
                Return MyBase.GetItem(68)
            End Get
        End Property
        Public ReadOnly Property ExpiryDate As StrucElement
            Get
                Return MyBase.GetItem(69)
            End Get
        End Property
        Public ReadOnly Property ProcessDate As StrucElement
            Get
                Return MyBase.GetItem(70)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(71)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(72)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(73)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(74)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(75)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(76)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(77)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(78)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(79)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(80)
            End Get
        End Property
        Public ReadOnly Property DailySync As StrucElement
            Get
                Return MyBase.GetItem(81)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace
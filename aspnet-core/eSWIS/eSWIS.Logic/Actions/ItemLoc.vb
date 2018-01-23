Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General


Namespace Actions
    Public NotInheritable Class ItemLoc
        Inherits Core.CoreControl
        Private ItemlocInfo As ItemlocInfo = New ItemlocInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function Save(ByVal ItemlocCont As Container.Itemloc, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ItemlocCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItemlocInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemlocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemlocCont.ItemCode) & "'")
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
                            pType = SQLControl.EnumSQLType.stUpdate
                        End If
                        StartSQLControl()
                        With objSQL
                            .TableName = "Itemloc"
                            .AddField("ItemDesc", ItemlocCont.ItemDesc, SQLControl.EnumDataType.dtStringN)
                            .AddField("ItemName", ItemlocCont.ItemName, SQLControl.EnumDataType.dtStringN)
                            .AddField("ItemComponent", ItemlocCont.ItemComponent, SQLControl.EnumDataType.dtStringN)
                            .AddField("ShortDesc", ItemlocCont.ShortDesc, SQLControl.EnumDataType.dtStringN)
                            .AddField("TariffCode", ItemlocCont.TariffCode, SQLControl.EnumDataType.dtStringN)
                            .AddField("OrgCountry", ItemlocCont.OrgCountry, SQLControl.EnumDataType.dtString)
                            .AddField("MATNo", ItemlocCont.MATNo, SQLControl.EnumDataType.dtStringN)
                            .AddField("MarkNo", ItemlocCont.MarkNo, SQLControl.EnumDataType.dtStringN)
                            .AddField("ItmSize2", ItemlocCont.ItmSize2, SQLControl.EnumDataType.dtString)
                            .AddField("ItmSize1", ItemlocCont.ItmSize1, SQLControl.EnumDataType.dtString)
                            .AddField("ItmSize", ItemlocCont.ItmSize, SQLControl.EnumDataType.dtString)
                            .AddField("ConSize", ItemlocCont.ConSize, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ConUOM", ItemlocCont.ConUOM, SQLControl.EnumDataType.dtString)
                            .AddField("DefUOM", ItemlocCont.DefUOM, SQLControl.EnumDataType.dtString)
                            .AddField("ClassType", ItemlocCont.ClassType, SQLControl.EnumDataType.dtString)
                            .AddField("TypeCode", ItemlocCont.TypeCode, SQLControl.EnumDataType.dtString)
                            .AddField("BehvType", ItemlocCont.BehvType, SQLControl.EnumDataType.dtNumeric)
                            .AddField("BehvShow", ItemlocCont.BehvShow, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ComboItem", ItemlocCont.ComboItem, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ItmCatgCode", ItemlocCont.ItmCatgCode, SQLControl.EnumDataType.dtString)
                            .AddField("ItmBrand", ItemlocCont.ItmBrand, SQLControl.EnumDataType.dtString)
                            .AddField("LooseUOM", ItemlocCont.LooseUOM, SQLControl.EnumDataType.dtString)
                            .AddField("PackUOM", ItemlocCont.PackUOM, SQLControl.EnumDataType.dtString)
                            .AddField("PackQty", ItemlocCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsSales", ItemlocCont.IsSales, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsEmpDisc", ItemlocCont.IsEmpDisc, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsRtnable", ItemlocCont.IsRtnable, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsDisc", ItemlocCont.IsDisc, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsFOC", ItemlocCont.IsFOC, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsTaxable", ItemlocCont.IsTaxable, SQLControl.EnumDataType.dtNumeric)
                            .AddField("AvgCost", ItemlocCont.AvgCost, SQLControl.EnumDataType.dtNumeric)
                            .AddField("StdCost", ItemlocCont.StdCost, SQLControl.EnumDataType.dtNumeric)
                            .AddField("StdMarkup", ItemlocCont.StdMarkup, SQLControl.EnumDataType.dtNumeric)
                            .AddField("StdSellPrice", ItemlocCont.StdSellPrice, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsSelected", ItemlocCont.IsSelected, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsBestBuy", ItemlocCont.IsBestBuy, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsPurchase", ItemlocCont.IsPurchase, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsWLength", ItemlocCont.IsWLength, SQLControl.EnumDataType.dtNumeric)
                            .AddField("TrackSerial", ItemlocCont.TrackSerial, SQLControl.EnumDataType.dtNumeric)
                            .AddField("MinQty", ItemlocCont.MinQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("MaxQty", ItemlocCont.MaxQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ImageName", ItemlocCont.ImageName, SQLControl.EnumDataType.dtStringN)
                            .AddField("IncomingQty", ItemlocCont.IncomingQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ReOrderLvl", ItemlocCont.ReOrderLvl, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ReOrderQty", ItemlocCont.ReOrderQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("QtyOnHand", ItemlocCont.QtyOnHand, SQLControl.EnumDataType.dtNumeric)
                            .AddField("QtySellable", ItemlocCont.QtySellable, SQLControl.EnumDataType.dtNumeric)
                            .AddField("QtyBalance", ItemlocCont.QtyBalance, SQLControl.EnumDataType.dtNumeric)
                            .AddField("POQty", ItemlocCont.POQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OutgoingQty", ItemlocCont.OutgoingQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("FirstIn", ItemlocCont.FirstIn, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastIn", ItemlocCont.LastIn, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastPO", ItemlocCont.LastPO, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastOut", ItemlocCont.LastOut, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastRV", ItemlocCont.LastRV, SQLControl.EnumDataType.dtNumeric)
                            .AddField("CummQty", ItemlocCont.CummQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("DayQty", ItemlocCont.DayQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LDayQty", ItemlocCont.LDayQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("MthQty", ItemlocCont.MthQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LMthQty", ItemlocCont.LMthQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("MtdQty", ItemlocCont.MtdQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("YrQty", ItemlocCont.YrQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LYrQty", ItemlocCont.LYrQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("YtdQty", ItemlocCont.YtdQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ProcessDate", ItemlocCont.ProcessDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateDate", ItemlocCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateBy", ItemlocCont.CreateBy, SQLControl.EnumDataType.dtString)
                            .AddField("LastUpdate", ItemlocCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("UpdateBy", ItemlocCont.UpdateBy, SQLControl.EnumDataType.dtString)
                            .AddField("Active", ItemlocCont.Active, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Inuse", ItemlocCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                            '.AddField("rowguid", ItemlocCont.rowguid, SQLControl.EnumDataType.dtString)
                            .AddField("Flag", ItemlocCont.Flag, SQLControl.EnumDataType.dtNumeric)

                            .AddField("StorageID", ItemlocCont.StorageID, SQLControl.EnumDataType.dtString)

                            Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    If blnFound = True And blnFlag = False Then
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemlocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemlocCont.ItemCode) & "'")
                                    Else
                                        If blnFound = False Then
                                            .AddField("LocID", ItemlocCont.LocID, SQLControl.EnumDataType.dtString)
                                            .AddField("ItemCode", ItemlocCont.ItemCode, SQLControl.EnumDataType.dtString)
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                        End If
                                    End If
                                Case SQLControl.EnumSQLType.stUpdate
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemlocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemlocCont.ItemCode) & "'")
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
                            Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemLoc", axExecute.Message & strSQL, axExecute.StackTrace)
                            Return False

                        Finally
                            objSQL.Dispose()
                        End Try

                    End If
                End If
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemLoc", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemLoc", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ItemlocCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function BatchSave(ByVal ListItemLocCont As List(Of Container.Itemloc), ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()

            BatchSave = False
            Try
                If ListItemLocCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()

                        If ListItemLocCont.Count > 0 Then
                            strSQL = BuildDelete("Itemloc WITH (ROWLOCK)", "LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ListItemLocCont(0).LocID) & "' AND ItemCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ListItemLocCont(0).ItemCode) & "' AND StorageID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ListItemLocCont(0).StorageID) & "'")
                            ListSQL.Add(strSQL)
                        End If

                    End If

                    For Each ItemLocCont In ListItemLocCont

                        With objSQL
                            .TableName = "Itemloc WITH (ROWLOCK)"
                            .AddField("ItemDesc", ItemLocCont.ItemDesc, SQLControl.EnumDataType.dtStringN)
                            .AddField("ItemName", ItemLocCont.ItemName, SQLControl.EnumDataType.dtStringN)
                            .AddField("ItemComponent", ItemLocCont.ItemComponent, SQLControl.EnumDataType.dtStringN)
                            .AddField("ShortDesc", ItemLocCont.ShortDesc, SQLControl.EnumDataType.dtStringN)
                            .AddField("TariffCode", ItemLocCont.TariffCode, SQLControl.EnumDataType.dtStringN)
                            .AddField("OrgCountry", ItemLocCont.OrgCountry, SQLControl.EnumDataType.dtString)
                            .AddField("MATNo", ItemLocCont.MATNo, SQLControl.EnumDataType.dtStringN)
                            .AddField("MarkNo", ItemLocCont.MarkNo, SQLControl.EnumDataType.dtStringN)
                            .AddField("ItmSize2", ItemLocCont.ItmSize2, SQLControl.EnumDataType.dtString)
                            .AddField("ItmSize1", ItemLocCont.ItmSize1, SQLControl.EnumDataType.dtString)
                            .AddField("ItmSize", ItemLocCont.ItmSize, SQLControl.EnumDataType.dtString)
                            .AddField("ConSize", ItemLocCont.ConSize, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ConUOM", ItemLocCont.ConUOM, SQLControl.EnumDataType.dtString)
                            .AddField("DefUOM", ItemLocCont.DefUOM, SQLControl.EnumDataType.dtString)
                            .AddField("ClassType", ItemLocCont.ClassType, SQLControl.EnumDataType.dtString)
                            .AddField("TypeCode", ItemLocCont.TypeCode, SQLControl.EnumDataType.dtString)
                            .AddField("BehvType", ItemLocCont.BehvType, SQLControl.EnumDataType.dtNumeric)
                            .AddField("BehvShow", ItemLocCont.BehvShow, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ComboItem", ItemLocCont.ComboItem, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ItmCatgCode", ItemLocCont.ItmCatgCode, SQLControl.EnumDataType.dtString)
                            .AddField("ItmBrand", ItemLocCont.ItmBrand, SQLControl.EnumDataType.dtString)
                            .AddField("LooseUOM", ItemLocCont.LooseUOM, SQLControl.EnumDataType.dtString)
                            .AddField("PackUOM", ItemLocCont.PackUOM, SQLControl.EnumDataType.dtString)
                            .AddField("PackQty", ItemLocCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsSales", ItemLocCont.IsSales, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsEmpDisc", ItemLocCont.IsEmpDisc, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsRtnable", ItemLocCont.IsRtnable, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsDisc", ItemLocCont.IsDisc, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsFOC", ItemLocCont.IsFOC, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsTaxable", ItemLocCont.IsTaxable, SQLControl.EnumDataType.dtNumeric)
                            .AddField("AvgCost", ItemLocCont.AvgCost, SQLControl.EnumDataType.dtNumeric)
                            .AddField("StdCost", ItemLocCont.StdCost, SQLControl.EnumDataType.dtNumeric)
                            .AddField("StdMarkup", ItemLocCont.StdMarkup, SQLControl.EnumDataType.dtNumeric)
                            .AddField("StdSellPrice", ItemLocCont.StdSellPrice, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsSelected", ItemLocCont.IsSelected, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsBestBuy", ItemLocCont.IsBestBuy, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsPurchase", ItemLocCont.IsPurchase, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsWLength", ItemLocCont.IsWLength, SQLControl.EnumDataType.dtNumeric)
                            .AddField("TrackSerial", ItemLocCont.TrackSerial, SQLControl.EnumDataType.dtNumeric)
                            .AddField("MinQty", ItemLocCont.MinQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("MaxQty", ItemLocCont.MaxQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ImageName", ItemLocCont.ImageName, SQLControl.EnumDataType.dtStringN)
                            .AddField("IncomingQty", ItemLocCont.IncomingQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ReOrderLvl", ItemLocCont.ReOrderLvl, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ReOrderQty", ItemLocCont.ReOrderQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("QtyOnHand", ItemLocCont.QtyOnHand, SQLControl.EnumDataType.dtNumeric)
                            .AddField("QtySellable", ItemLocCont.QtySellable, SQLControl.EnumDataType.dtNumeric)
                            .AddField("QtyBalance", ItemLocCont.QtyBalance, SQLControl.EnumDataType.dtNumeric)
                            .AddField("POQty", ItemLocCont.POQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OutgoingQty", ItemLocCont.OutgoingQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("FirstIn", ItemLocCont.FirstIn, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastIn", ItemLocCont.LastIn, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastPO", ItemLocCont.LastPO, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastOut", ItemLocCont.LastOut, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastRV", ItemLocCont.LastRV, SQLControl.EnumDataType.dtNumeric)
                            .AddField("CummQty", ItemLocCont.CummQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("DayQty", ItemLocCont.DayQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LDayQty", ItemLocCont.LDayQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("MthQty", ItemLocCont.MthQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LMthQty", ItemLocCont.LMthQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("MtdQty", ItemLocCont.MtdQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("YrQty", ItemLocCont.YrQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LYrQty", ItemLocCont.LYrQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("YtdQty", ItemLocCont.YtdQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ProcessDate", ItemLocCont.ProcessDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateDate", ItemLocCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateBy", ItemLocCont.CreateBy, SQLControl.EnumDataType.dtString)
                            .AddField("LastUpdate", ItemLocCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("UpdateBy", ItemLocCont.UpdateBy, SQLControl.EnumDataType.dtString)
                            .AddField("Active", ItemLocCont.Active, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Inuse", ItemLocCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                            Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    .AddField("LocID", ItemLocCont.LocID, SQLControl.EnumDataType.dtString)
                                    '.AddField("StorageID", ItemLocCont.StorageID, SQLControl.EnumDataType.dtString)
                                    .AddField("ItemCode", ItemLocCont.ItemCode, SQLControl.EnumDataType.dtString)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                Case SQLControl.EnumSQLType.stUpdate
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "'")
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
                        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemLoc", axExecute.Message & sqlStatement, axExecute.StackTrace)
                    Finally
                        objSQL.Dispose()
                    End Try
                    Return True

                End If
            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemLoc", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemLoc", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ListItemLocCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal ItemlocCont As Container.Itemloc, ByRef message As String) As Boolean
            Return Save(ItemlocCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal ItemlocCont As Container.Itemloc, ByRef message As String) As Boolean
            Return Save(ItemlocCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        'BATCH ADD
        Public Function BatchInsert(ByVal ListItemLocCont As List(Of Container.Itemloc), ByRef message As String) As Boolean
            Return BatchSave(ListItemLocCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'BATCH AMEND
        Public Function BatchUpdate(ByVal ListItemLocCont As List(Of Container.Itemloc), ByRef message As String) As Boolean
            Return BatchSave(ListItemLocCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal ItemlocCont As Container.Itemloc, Optional ByVal message As String = Nothing) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If ItemlocCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()

                        With ItemlocInfo.MyInfo

                            strSQL = BuildSelect(.CheckFields, .TableName, "LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemlocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemlocCont.ItemCode) & "'")
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
                                strSQL = BuildUpdate("Itemloc WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemlocCont.UpdateBy) & "' WHERE" & _
                                " LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemlocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemlocCont.ItemCode) & "'")
                            End With

                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("Itemloc WITH (ROWLOCK)", "LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemlocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemlocCont.ItemCode) & "'")
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemLoc", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemLoc", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemLoc", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                ItemlocCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'Add
        Public Function GetSQLDelete(ByVal ItemlocCont As Container.Itemloc, Optional ByVal message As String = Nothing) As String
            Dim strSQL As String = ""
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader

            blnFound = False
            blnInUse = False
            Try
                If ItemlocCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()

                        With ItemlocInfo.MyInfo

                            strSQL = BuildSelect(.CheckFields, .TableName, "LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemlocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemlocCont.ItemCode) & "'")
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
                                strSQL = BuildUpdate("Itemloc WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemlocCont.UpdateBy) & "' WHERE" & _
                                " LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemlocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemlocCont.ItemCode) & "'")
                            End With

                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("Itemloc WITH (ROWLOCK)", "LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemlocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemlocCont.ItemCode) & "'")
                        End If

                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemLoc", axDelete.Message, axDelete.StackTrace)
                Return ""
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemLoc", exDelete.Message, exDelete.StackTrace)
                Return ""
            Finally
                ItemlocCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try

            Return strSQL

        End Function

        Private Function UpdateQtyLabel(ByVal ListItemLocCont As List(Of Container.Itemloc), ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()

            UpdateQtyLabel = False
            Try
                If ListItemLocCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                    End If

                    For Each ItemLocCont In ListItemLocCont

                        With objSQL
                            .TableName = "Itemloc WITH (ROWLOCK)"
                            .AddField("PackQty", ItemLocCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                            Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    .AddField("LocID", ItemLocCont.LocID, SQLControl.EnumDataType.dtString)
                                    .AddField("ItemCode", ItemLocCont.ItemCode, SQLControl.EnumDataType.dtString)
                                    .AddField("ItemName", ItemLocCont.ItemName, SQLControl.EnumDataType.dtString)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                Case SQLControl.EnumSQLType.stUpdate
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemLocCont.LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemLocCont.ItemCode) & "' AND ItemName='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemLocCont.ItemName) & "'")
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
                        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemLoc", axExecute.Message & sqlStatement, axExecute.StackTrace)
                    Finally
                        objSQL.Dispose()
                    End Try
                    Return True

                End If
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemLoc", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemLoc", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ListItemLocCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'BATCH AMEND
        Public Function UpdateLabel(ByVal ListItemLocCont As List(Of Container.Itemloc), ByRef message As String) As Boolean
            Return UpdateQtyLabel(ListItemLocCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

#End Region

#Region "Data Selection"
        Public Overloads Function GetItemLoc(ByVal LocID As System.String, ByVal ItemCode As System.String, Optional ByVal ItemName As System.String = Nothing) As Container.Itemloc
            Dim rItemloc As Container.Itemloc = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With ItemlocInfo.MyInfo

                        If Not ItemName Is Nothing Then
                            strSQL = BuildSelect(.FieldsList, .TableName, "LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "' AND ItemName = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemName) & "'")
                        Else
                            strSQL = BuildSelect(.FieldsList, .TableName, "LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "'")
                        End If

                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rItemloc = New Container.Itemloc
                                rItemloc.LocID = drRow.Item("LocID")
                                rItemloc.StorageID = drRow.Item("StorageID")
                                rItemloc.ItemCode = drRow.Item("ItemCode")
                                rItemloc.ItemDesc = drRow.Item("ItemDesc")
                                rItemloc.ItemName = drRow.Item("ItemName")
                                rItemloc.ItemComponent = drRow.Item("ItemComponent")
                                rItemloc.ShortDesc = drRow.Item("ShortDesc")
                                rItemloc.TariffCode = drRow.Item("TariffCode")
                                rItemloc.OrgCountry = drRow.Item("OrgCountry")
                                rItemloc.MATNo = drRow.Item("MATNo")
                                rItemloc.MarkNo = drRow.Item("MarkNo")
                                rItemloc.ItmSize2 = drRow.Item("ItmSize2")
                                rItemloc.ItmSize1 = drRow.Item("ItmSize1")
                                rItemloc.ItmSize = drRow.Item("ItmSize")
                                rItemloc.ConSize = drRow.Item("ConSize")
                                rItemloc.ConUOM = drRow.Item("ConUOM")
                                rItemloc.DefUOM = drRow.Item("DefUOM")
                                rItemloc.ClassType = drRow.Item("ClassType")
                                rItemloc.TypeCode = drRow.Item("TypeCode")
                                rItemloc.BehvType = drRow.Item("BehvType")
                                rItemloc.BehvShow = drRow.Item("BehvShow")
                                rItemloc.ComboItem = drRow.Item("ComboItem")
                                rItemloc.ItmCatgCode = drRow.Item("ItmCatgCode")
                                rItemloc.ItmBrand = drRow.Item("ItmBrand")
                                rItemloc.LooseUOM = drRow.Item("LooseUOM")
                                rItemloc.PackUOM = drRow.Item("PackUOM")
                                rItemloc.PackQty = drRow.Item("PackQty")
                                rItemloc.IsSales = drRow.Item("IsSales")
                                rItemloc.IsEmpDisc = drRow.Item("IsEmpDisc")
                                rItemloc.IsRtnable = drRow.Item("IsRtnable")
                                rItemloc.IsDisc = drRow.Item("IsDisc")
                                rItemloc.IsFOC = drRow.Item("IsFOC")
                                rItemloc.IsTaxable = drRow.Item("IsTaxable")
                                rItemloc.AvgCost = drRow.Item("AvgCost")
                                rItemloc.StdCost = drRow.Item("StdCost")
                                rItemloc.StdMarkup = drRow.Item("StdMarkup")
                                rItemloc.StdSellPrice = drRow.Item("StdSellPrice")
                                rItemloc.IsSelected = drRow.Item("IsSelected")
                                rItemloc.IsBestBuy = drRow.Item("IsBestBuy")
                                rItemloc.IsPurchase = drRow.Item("IsPurchase")
                                rItemloc.IsWLength = drRow.Item("IsWLength")
                                rItemloc.TrackSerial = drRow.Item("TrackSerial")
                                rItemloc.MinQty = drRow.Item("MinQty")
                                rItemloc.MaxQty = drRow.Item("MaxQty")
                                rItemloc.ImageName = drRow.Item("ImageName")
                                rItemloc.IncomingQty = drRow.Item("IncomingQty")
                                rItemloc.ReOrderLvl = drRow.Item("ReOrderLvl")
                                rItemloc.ReOrderQty = drRow.Item("ReOrderQty")
                                rItemloc.QtyOnHand = drRow.Item("QtyOnHand")
                                rItemloc.QtySellable = drRow.Item("QtySellable")
                                rItemloc.QtyBalance = drRow.Item("QtyBalance")
                                rItemloc.POQty = drRow.Item("POQty")
                                rItemloc.OutgoingQty = drRow.Item("OutgoingQty")
                                rItemloc.FirstIn = drRow.Item("FirstIn")
                                rItemloc.LastIn = drRow.Item("LastIn")
                                rItemloc.LastPO = drRow.Item("LastPO")
                                rItemloc.LastOut = drRow.Item("LastOut")
                                rItemloc.LastRV = drRow.Item("LastRV")
                                rItemloc.CummQty = drRow.Item("CummQty")
                                rItemloc.DayQty = drRow.Item("DayQty")
                                rItemloc.LDayQty = drRow.Item("LDayQty")
                                rItemloc.MthQty = drRow.Item("MthQty")
                                rItemloc.LMthQty = drRow.Item("LMthQty")
                                rItemloc.MtdQty = drRow.Item("MtdQty")
                                rItemloc.YrQty = drRow.Item("YrQty")
                                rItemloc.LYrQty = drRow.Item("LYrQty")
                                rItemloc.YtdQty = drRow.Item("YtdQty")
                                rItemloc.CreateBy = drRow.Item("CreateBy")
                                rItemloc.UpdateBy = drRow.Item("UpdateBy")
                                rItemloc.Active = drRow.Item("Active")
                                rItemloc.Inuse = drRow.Item("Inuse")
                                rItemloc.rowguid = drRow.Item("rowguid")
                                rItemloc.SyncCreate = drRow.Item("SyncCreate")
                                rItemloc.SyncLastUpd = IIf(IsDBNull(drRow.Item("SyncLastUpd")), Now, drRow.Item("SyncLastUpd"))
                            Else
                                rItemloc = Nothing
                            End If
                        Else
                            rItemloc = Nothing
                        End If
                    End With
                End If
                Return rItemloc
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemLoc", ex.Message, ex.StackTrace)
            Finally
                rItemloc = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetItemLoc(ByVal LocID As System.String, ByVal ItemCode As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Itemloc)
            Dim rItemloc As Container.Itemloc = Nothing
            Dim lstItemloc As List(Of Container.Itemloc) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With ItemlocInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal LocID As System.String, ByVal ItemCode As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rItemloc = New Container.Itemloc
                                rItemloc.LocID = drRow.Item("LocID")
                                rItemloc.StorageID = drRow.Item("StorageID")
                                rItemloc.ItemCode = drRow.Item("ItemCode")
                                rItemloc.ItemDesc = drRow.Item("ItemDesc")
                                rItemloc.ItemName = drRow.Item("ItemName")
                                rItemloc.ItemComponent = drRow.Item("ItemComponent")
                                rItemloc.ShortDesc = drRow.Item("ShortDesc")
                                rItemloc.TariffCode = drRow.Item("TariffCode")
                                rItemloc.OrgCountry = drRow.Item("OrgCountry")
                                rItemloc.MATNo = drRow.Item("MATNo")
                                rItemloc.MarkNo = drRow.Item("MarkNo")
                                rItemloc.ItmSize2 = drRow.Item("ItmSize2")
                                rItemloc.ItmSize1 = drRow.Item("ItmSize1")
                                rItemloc.ItmSize = drRow.Item("ItmSize")
                                rItemloc.ConSize = drRow.Item("ConSize")
                                rItemloc.ConUOM = drRow.Item("ConUOM")
                                rItemloc.DefUOM = drRow.Item("DefUOM")
                                rItemloc.ClassType = drRow.Item("ClassType")
                                rItemloc.TypeCode = drRow.Item("TypeCode")
                                rItemloc.BehvType = drRow.Item("BehvType")
                                rItemloc.BehvShow = drRow.Item("BehvShow")
                                rItemloc.ComboItem = drRow.Item("ComboItem")
                                rItemloc.ItmCatgCode = drRow.Item("ItmCatgCode")
                                rItemloc.ItmBrand = drRow.Item("ItmBrand")
                                rItemloc.LooseUOM = drRow.Item("LooseUOM")
                                rItemloc.PackUOM = drRow.Item("PackUOM")
                                rItemloc.PackQty = drRow.Item("PackQty")
                                rItemloc.IsSales = drRow.Item("IsSales")
                                rItemloc.IsEmpDisc = drRow.Item("IsEmpDisc")
                                rItemloc.IsRtnable = drRow.Item("IsRtnable")
                                rItemloc.IsDisc = drRow.Item("IsDisc")
                                rItemloc.IsFOC = drRow.Item("IsFOC")
                                rItemloc.IsTaxable = drRow.Item("IsTaxable")
                                rItemloc.AvgCost = drRow.Item("AvgCost")
                                rItemloc.StdCost = drRow.Item("StdCost")
                                rItemloc.StdMarkup = drRow.Item("StdMarkup")
                                rItemloc.StdSellPrice = drRow.Item("StdSellPrice")
                                rItemloc.IsSelected = drRow.Item("IsSelected")
                                rItemloc.IsBestBuy = drRow.Item("IsBestBuy")
                                rItemloc.IsPurchase = drRow.Item("IsPurchase")
                                rItemloc.IsWLength = drRow.Item("IsWLength")
                                rItemloc.TrackSerial = drRow.Item("TrackSerial")
                                rItemloc.MinQty = drRow.Item("MinQty")
                                rItemloc.MaxQty = drRow.Item("MaxQty")
                                rItemloc.ImageName = drRow.Item("ImageName")
                                rItemloc.IncomingQty = drRow.Item("IncomingQty")
                                rItemloc.ReOrderLvl = drRow.Item("ReOrderLvl")
                                rItemloc.ReOrderQty = drRow.Item("ReOrderQty")
                                rItemloc.QtyOnHand = drRow.Item("QtyOnHand")
                                rItemloc.QtySellable = drRow.Item("QtySellable")
                                rItemloc.QtyBalance = drRow.Item("QtyBalance")
                                rItemloc.POQty = drRow.Item("POQty")
                                rItemloc.OutgoingQty = drRow.Item("OutgoingQty")
                                rItemloc.FirstIn = drRow.Item("FirstIn")
                                rItemloc.LastIn = drRow.Item("LastIn")
                                rItemloc.LastPO = drRow.Item("LastPO")
                                rItemloc.LastOut = drRow.Item("LastOut")
                                rItemloc.LastRV = drRow.Item("LastRV")
                                rItemloc.CummQty = drRow.Item("CummQty")
                                rItemloc.DayQty = drRow.Item("DayQty")
                                rItemloc.LDayQty = drRow.Item("LDayQty")
                                rItemloc.MthQty = drRow.Item("MthQty")
                                rItemloc.LMthQty = drRow.Item("LMthQty")
                                rItemloc.MtdQty = drRow.Item("MtdQty")
                                rItemloc.YrQty = drRow.Item("YrQty")
                                rItemloc.LYrQty = drRow.Item("LYrQty")
                                rItemloc.YtdQty = drRow.Item("YtdQty")
                                rItemloc.CreateBy = drRow.Item("CreateBy")
                                rItemloc.UpdateBy = drRow.Item("UpdateBy")
                                rItemloc.Active = drRow.Item("Active")
                                rItemloc.Inuse = drRow.Item("Inuse")
                                rItemloc.rowguid = drRow.Item("rowguid")
                                rItemloc.SyncCreate = drRow.Item("SyncCreate")
                                rItemloc.SyncLastUpd = drRow.Item("SyncLastUpd")
                            Next
                            lstItemloc.Add(rItemloc)
                        Else
                            rItemloc = Nothing
                        End If
                        Return lstItemloc
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ItemLoc", ex.Message, ex.StackTrace)
            Finally
                rItemloc = Nothing
                lstItemloc = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetItemLocList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ItemlocInfo.MyInfo
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

        Public Overloads Function GetItemLocShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With ItemlocInfo.MyInfo
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

        'lily 20/05/2016 function to get Item for Inventory
        Public Overloads Function GetItemLocCustom(ByVal LocID As System.String, ByVal ItemCode As System.String, ByVal ItemName As System.String) As Container.Itemloc
            Dim rItemLoc As Container.Itemloc = Nothing
            Dim dtTemp As DataTable = Nothing

            If StartConnection() = True Then
                StartSQLControl()
                strSQL = "SELECT L.LocID, L.ItemCode, L.ItemName, L.QtyOnHand, L.LastIn, L.LastOut, L.ShortDesc as Remarks" & _
                         " from ITEMLOC L WITH (NOLOCK) " & _
                         " where L.Flag=1 AND L.IsSelected<>1 AND L.LocId='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & _
                         " ' and ItemCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & _
                         " ' and ItemName='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemName) & "'"
                Try
                    dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
                    If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count = 1 Then
                        rItemLoc = New Container.Itemloc
                        With rItemLoc
                            .LocID = dtTemp.Rows(0).Item("LocID").ToString
                            .ItemCode = dtTemp.Rows(0).Item("ItemCode").ToString
                            .ItemName = dtTemp.Rows(0).Item("ItemName").ToString
                            .QtyOnHand = dtTemp.Rows(0).Item("QtyOnHand").ToString
                            .LastIn = dtTemp.Rows(0).Item("LastIn").ToString
                            .LastOut = dtTemp.Rows(0).Item("LastOut").ToString
                            .ShortDesc = dtTemp.Rows(0).Item("Remarks").ToString
                        End With
                    End If
                Catch ex As Exception
                    Log.Notifier.Notify(ex)
                    Gibraltar.Agent.Log.Error("Actions/ItemLoc", ex.Message & " " & strSQL, ex.StackTrace)
                Finally
                    EndSQLControl()
                End Try
            End If
            EndConnection()
            Return rItemLoc
        End Function

        'Lily 24/05/2015 get itemList for inventory
        Public Overloads Function GetItemLocCustom(ByVal LocID As System.String) As List(Of Container.Itemloc)
            Dim ListItemLoc As List(Of Container.Itemloc) = Nothing
            Dim rItemLoc As Container.Itemloc = Nothing
            Dim dtTemp As DataTable = Nothing

            If StartConnection() = True Then
                StartSQLControl()

                strSQL = "select be.BizRegID, be.CompanyName, bl.BizLocID, bl.BranchName, l.ItemCode, l.ItemName, l.QtyOnHand, l.LastIn, l.LastOut, l.ShortDesc " & _
                    "from ITEMLOC l WITH (NOLOCK) " & _
                    "inner join BIZLOCATE bl WITH (NOLOCK) on l.LocID=bl.BizLocID " & _
                    "inner join BIZENTITY be WITH (NOLOCK) on bl.BizRegID=be.BizRegID " & _
                    "where l.Flag='1' AND L.IsSelected<>1 AND l.LocId='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "'"

                Try
                    dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
                    If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
                        ListItemLoc = New List(Of Container.Itemloc)
                        For Each row In dtTemp.Rows
                            rItemLoc = New Container.Itemloc
                            With rItemLoc
                                .CompanyID = row.Item("BizRegID").ToString
                                .CompanyName = row.Item("CompanyName").ToString
                                .LocID = row.Item("BizLocID").ToString
                                .BranchName = row.Item("BranchName").ToString
                                .ItemCode = row.Item("ItemCode").ToString
                                .ItemName = row.Item("ItemName").ToString
                                .QtyOnHand = row.Item("QtyOnHand").ToString
                                .LastIn = row.Item("LastIn").ToString
                                .LastOut = row.Item("LastOut").ToString
                                .ShortDesc = row.Item("ShortDesc").ToString
                            End With
                            ListItemLoc.Add(rItemLoc)
                        Next
                    End If
                Catch ex As Exception
                    Log.Notifier.Notify(ex)
                    Gibraltar.Agent.Log.Error("Actions/ItemLoc", ex.Message & " " & strSQL, ex.StackTrace)
                Finally
                    EndSQLControl()
                End Try
            End If
            EndConnection()
            Return ListItemLoc
        End Function

        'Add for PieChart
        Public Overloads Function GetPieChartCustomList(Optional ByVal LocID As String = Nothing, Optional ByVal Condition As String = Nothing, Optional ByVal Month As String = Nothing, Optional ByVal Year As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With ItemlocInfo.MyInfo

                    'make pie chart in inventory addition the same as summary
                    strSQL = "SELECT it.ItemCode,sum(it.Closing) as QtyOnHandPie, E.BizRegID as CompanyID" & _
                        " FROM itemsummary it WITH (NOLOCK) " & _
                        " Left Join bizlocate B WITH (NOLOCK) on b.BizLocID=it.LocID" & _
                        " Left Join BizEntity E WITH (NOLOCK) on b.BizRegID=E.BizRegID" & _
                        " Left join ItemLoc il WITH (NOLOCK) on it.LocID=il.LocID and it.ItemCode=il.ItemCode" & _
                        " Left join (SELECT dtl.WASTECODE, SUM(dtl.QTY) QTY FROM CONSIGNHDR hdr WITH (NOLOCK) LEFT JOIN CONSIGNDTL dtl WITH (NOLOCK) ON hdr.TRANSID = dtl.TRANSID  WHERE REJECTREMARK != '' AND hdr.GENERATORLOCID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' GROUP BY dtl.WASTECODE) CN ON  it.ITEMCODE = CN.WASTECODE" & _
                        " WHERE it.ishost = 0 AND it.locID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND MthCode='" & Month & "' AND YearCode='" & Year & "' AND il.ItemName=it.ItemName AND CompanyName not in ('') AND it.IsHost=0 group by it.ItemCode, E.BizRegID order by it.itemCode ASC"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetItemSummaryHDR(ByVal state As Boolean, Optional ByVal locID As String = Nothing, Optional ByVal MTHCODE As String = Nothing, Optional ByVal YEARCODE As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With ItemlocInfo.MyInfo

                    If Not state Then

                        strSQL = "SELECT DISTINCT l.ACCNO, it.REFNO, c.COMPANYNAME, l.BRANCHNAME, l.ADDRESS1, l.ADDRESS2, l.ADDRESS3, l.ADDRESS4, s.STATEDESC, 'SUBMIT' AS STATUS, UPPER(DATENAME(MONTH,DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,(it.MTHCODE+'/'+'01/'+it.YEARCODE))+1,0)))) AS MONTH, it.YEARCODE, l.CONTACTPERSON, cm.CODEDESC AS DESIGNATION " & _
                            "FROM ITEMSUMMARY it WITH (NOLOCK) " & _
                            "LEFT JOIN ITEM i WITH (NOLOCK) ON i.ItemCode=it.ItemCode " & _
                            "LEFT JOIN BIZLOCATE l WITH (NOLOCK) ON l.BIZLOCID=it.LOCID " & _
                            "LEFT JOIN BIZENTITY c WITH (NOLOCK) ON c.BIZREGID=l.BIZREGID " & _
                            "LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODE=l.CONTACTDESIGNATION AND cm.CODETYPE='DSN' " & _
                            "LEFT JOIN STATE s WITH (NOLOCK) ON s.STATECODE=l.STATE AND s.COUNTRYCODE=l.COUNTRY WHERE it.IsHost=0 AND it.QTYIN<>0 "

                        If Not locID Is Nothing And locID <> "" Then
                            strSQL &= " AND it.LOCID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, locID) & "' "
                        End If

                        If Not MTHCODE Is Nothing And MTHCODE <> "" Then
                            strSQL &= " AND it.MTHCODE='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, MTHCODE) & "' "
                        End If

                        If Not YEARCODE Is Nothing And YEARCODE <> "" Then
                            strSQL &= " AND it.YEARCODE='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, YEARCODE) & "' "
                        End If
                    ElseIf state Then

                        strSQL = "SELECT DISTINCT l.ACCNO, l.BIZLOCID+@MONTH+@YEAR as REFNO, c.COMPANYNAME, l.BRANCHNAME, l.ADDRESS1, l.ADDRESS2, l.ADDRESS3, l.ADDRESS4, s.STATEDESC, 'SUBMIT' AS STATUS, UPPER(DATENAME(MONTH,DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,(@MONTH+'/'+'01/'+@YEAR))+1,0)))) AS MONTH, @YEAR as YEARCODE, l.CONTACTPERSON, cm.CODEDESC AS DESIGNATION " & _
                            "FROM BIZLOCATE l WITH (NOLOCK) " & _
                            "LEFT JOIN BIZENTITY c WITH (NOLOCK) ON c.BIZREGID=l.BIZREGID " & _
                            "LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODE=l.CONTACTDESIGNATION AND cm.CODETYPE='DSN' " & _
                            "LEFT JOIN STATE s WITH (NOLOCK) ON s.STATECODE=l.STATE AND s.COUNTRYCODE=l.COUNTRY "

                        Dim strFilter As String
                        Dim strMonth As String
                        Dim strYear As String

                        If locID IsNot Nothing AndAlso locID <> "" Then
                            strFilter = "l.BIZLOCID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, locID) & "'"
                        End If

                        If Not MTHCODE Is Nothing And MTHCODE <> "" Then
                            strMonth = objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, MTHCODE)
                            If strMonth <> 0 Then
                                strSQL = strSQL.Replace("@MONTH", "'" & strMonth & "'")
                            Else
                                strSQL = strSQL.Replace("@MONTH", "'" & DateTime.Now.Month & "'")
                            End If
                        End If

                        If Not YEARCODE Is Nothing And YEARCODE <> "" Then
                            strYear = objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, YEARCODE)
                            If strYear <> 0 Then
                                strSQL = strSQL.Replace("@YEAR", "'" & strYear & "'")
                            Else
                                strSQL = strSQL.Replace("@YEAR", "'" & DateTime.Now.Year & "'")
                            End If
                        End If

                        If Not strFilter Is Nothing And strFilter <> "" Then strSQL &= " WHERE " & strFilter
                    End If

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetItemSummaryDTL(Optional ByVal locID As String = Nothing, Optional ByVal MTHCode As String = Nothing, Optional ByVal YearCode As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ItemlocInfo.MyInfo
                    StartSQLControl()

                    strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY it.MTHCODE, it.YEARCODE) AS [#], it.QtyAdj as QtyAdjust, " & _
                         " CASE when it.FIRSTGENERATED is null then (select min(h.transdate) from itmtranshdr h, itmtransdtl d where h.DocCode=d.doccode and h.locid=d.locid and h.termid=0 and d.transtype=0 and h.flag=1 and h.posted=1 and h.status=1 and h.locid=it.locid and d.itemcode=it.itemcode and d.itemname=it.itemname and MONTH(h.TransDate)=it.MTHCODE and YEAR(h.TransDate) = it.YEARCODE) else it.FIRSTGENERATED end  AS [DATE],  " & _
                        "it.ITEMCODE, i.ITMDESC,il.ITEMNAME, it.OPENING, it.QTYIN, H.HMethod AS CODEDESC, H.HPlace AS REMARK, H.HQty AS QTYHANDLING, it.Closing AS CLOSING " & _
                        "FROM ITEMSUMMARY it WITH (NOLOCK) " & _
                        "LEFT JOIN ITEM i WITH (NOLOCK) ON i.ItemCode=it.ItemCode " & _
                        "LEFT JOIN ITEMLOC il WITH (NOLOCK) on il.LocID = it.LocID and il.ItemCode=it.ItemCode AND il.ITEMNAME=it.ITEMNAME " & _
                        "LEFT JOIN ( SELECT HYear, HMonth, HLocID, HItemCode, HItemName, HMethod, HPlace, SUM(HQty) as HQty FROM ( " & _
                        "SELECT YEAR(h.TRANSDATE) AS HYear, MONTH(h.TRANSDATE) AS HMonth, h.GENERATORLOCID AS HLocID, d.WASTECODE AS HItemCode, d.WASTEDESCRIPTION AS HItemName, cm.CODEDESC AS HMethod, case h.STATUS when 8 then e.COMPANYNAME else '' end AS HPlace, SUM(d.QTY) AS HQty " & _
                        "FROM CONSIGNDTL d WITH (NOLOCK) " & _
                        "LEFT JOIN CONSIGNHDR h WITH (NOLOCK) ON h.CONTRACTNO = d.CONTRACTNO AND h.FLAG = 1 AND h.STATUS <> 0 AND h.STATUS <> 2 AND h.STATUS <> 9 AND h.STATUS <> 12 AND h.ISCONFIRM = 1 " & _
                        "LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODE = d.OPERATIONTYPE AND cm.CODETYPE='WTH' " & _
                        "LEFT JOIN BIZENTITY e WITH (NOLOCK) ON e.BIZREGID = h.RECEIVERID " & _
                        "GROUP BY YEAR(h.TRANSDATE), MONTH(h.TRANSDATE), h.GENERATORLOCID, d.WASTECODE, d.WASTEDESCRIPTION, cm.CODEDESC, e.COMPANYNAME, h.STATUS " & _
                        "UNION ALL " & _
                        "SELECT YEARCODE as HYear, MTHCODE AS HMonth, LOCID AS HLocID, ITEMCODE AS HItemCode, ITEMNAME AS HItemName, 'Adjustment' AS HMethod, '' AS HPlace, SUM(QTYADJ) AS HQty  " & _
                        "FROM ITEMSUMMARY WITH (NOLOCK) " & _
                        "WHERE QTYADJ <> 0 " & _
                        "GROUP BY YEARCODE, MTHCODE, LOCID, ITEMCODE, ITEMNAME " & _
                        "UNION ALL " & _
                        "SELECT YEARCODE as HYear, MTHCODE AS HMonth, LOCID AS HLocID, ITEMCODE AS HItemCode, ITEMNAME AS HItemName, 'Reused' AS HMethod, '' AS HPlace, SUM(QTYREUSED) AS HQty  " & _
                        "FROM ITEMSUMMARY WITH (NOLOCK) " & _
                        "WHERE QTYREUSED <> 0 " & _
                        "GROUP BY YEARCODE, MTHCODE, LOCID, ITEMCODE, ITEMNAME) as TH " & _
                        "GROUP BY HYear, HMonth, HLocID, HItemCode, HItemName, HMethod, HPlace) as H ON H.HYear=it.YEARCODE and H.HMonth=it.MTHCODE and H.HLocID=it.LocID and H.HItemCode=it.ItemCode and H.HItemName=it.ItemName " & _
                        "WHERE il.Flag=1 AND il.Active=1 "

                    If Not locID Is Nothing And locID <> "" Then
                        strSQL &= " AND it.LOCID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, locID) & "' "
                    End If

                    If Not MTHCode Is Nothing And MTHCode <> "" Then
                        strSQL &= " AND it.MTHCODE='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, MTHCode) & "'"
                    End If

                    If Not YearCode Is Nothing And YearCode <> "" Then
                        strSQL &= " AND it.YEARCODE='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, YearCode) & "'"
                    End If

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    EndSQLControl()
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetItemQtyOnHand(ByVal LocID As String) As Data.DataTable
            If StartConnection() = True Then
                With ItemlocInfo.MyInfo
                    StartSQLControl()
                    strSQL = "select it.ItemCode AS WasteCode, c.CodeDesc As WasteTypeDesc, s.QtyIn AS RcvQty, " &
                        " s.QtyHandling AS HandlingQty, s.QtyOut AS ResidueQty, s.Closing AS QtyOnHand from itemloc it WITH (NOLOCK) INNER " &
                        " JOIN itemsummary s WITH(NOLOCK) ON it.LocID=s.LocID AND it.ItemCode=s.ItemCode AND it.ItemName=s.ItemName INNER " &
                        " JOIN CodeMaster c WITH (NOLOCK) ON it.TypeCode=c.Code AND c.CodeType='WTY' where it.LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND " &
                        " s.MthCode=Month(GetDate()) AND YearCode=Year(GetDate())"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetItemQtyOnHand_ByCodeName(ByVal WasCode As String, ByVal WasName As String, ByVal LocID As String) As Data.DataTable
            If StartConnection() = True Then
                With ItemlocInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT * FROM ItemLoc WHERE ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasCode) & "' " &
                             "AND ItemName = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasName) & "'" &
                             "AND LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetItemClosingPrevMonth(ByVal LocID As String, ByVal YearCode As String, ByVal MthCode As String, ByVal ItemCode As String, ByVal ItemName As String) As Data.DataTable
            If StartConnection() = True Then
                With ItemlocInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT Closing FROM ITEMSUMMARY WHERE LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND YearCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, YearCode) & "' " &
                        "AND MthCode =  '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, MthCode) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "' AND ItemName = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemName) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetBalanceItemBySelectedDate(ByVal LocID As String, ByVal ItemCode As String, ByVal ItemName As String, ByVal SelectedDate As String) As Data.DataTable
            If StartConnection() = True Then
                With ItemlocInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT * FROM dbo.[GetListDateBalance] ('" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "', '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "', '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemName) & "', '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, SelectedDate) & "')"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetNotifiedList(ByVal LocID As String, ByVal Key As String, Optional ByVal Qty As Boolean = False) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With ItemlocInfo.MyInfo
                    strSQL = "SELECT D.ItemCode, D.ItemName,I.TransDate,"

                    If Qty Then
                        strSQL &= " L.PackQty AS QtyLabel "
                    Else
                        strSQL &= " 1 AS QtyLabel "
                    End If

                    strSQL &= " from ITMTRANSHDR I WITH (NOLOCK)  INNER JOIN ITMTRANSDTL D WITH (NOLOCK) ON I.DocCode=D.DOcCode INNER JOIN ITEMLOC L WITH (NOLOCK) ON L.LocID=I.LocID AND D.ItemCode=L.ItemCode AND D.ItemName=L.ItemName WHERE I.DocCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Key) & "' " & _
                        " AND L.Flag=1 AND I.LocId ='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' ORDER BY D.ItemCode ASC "

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
        Public Class Itemloc
            Public fLocID As System.String = "LocID"
            Public fStorageID As System.String = "StorageID"
            Public fItemCode As System.String = "ItemCode"
            Public fItemDesc As System.String = "ItemDesc"
            Public fItemName As System.String = "ItemName"
            Public fItemComponent As System.String = "ItemComponent"
            Public fShortDesc As System.String = "ShortDesc"
            Public fTariffCode As System.String = "TariffCode"
            Public fOrgCountry As System.String = "OrgCountry"
            Public fMATNo As System.String = "MATNo"
            Public fMarkNo As System.String = "MarkNo"
            Public fItmSize2 As System.String = "ItmSize2"
            Public fItmSize1 As System.String = "ItmSize1"
            Public fItmSize As System.String = "ItmSize"
            Public fConSize As System.String = "ConSize"
            Public fConUOM As System.String = "ConUOM"
            Public fDefUOM As System.String = "DefUOM"
            Public fClassType As System.String = "ClassType"
            Public fTypeCode As System.String = "TypeCode"
            Public fBehvType As System.String = "BehvType"
            Public fBehvShow As System.String = "BehvShow"
            Public fComboItem As System.String = "ComboItem"
            Public fItmCatgCode As System.String = "ItmCatgCode"
            Public fItmBrand As System.String = "ItmBrand"
            Public fLooseUOM As System.String = "LooseUOM"
            Public fPackUOM As System.String = "PackUOM"
            Public fPackQty As System.String = "PackQty"
            Public fIsSales As System.String = "IsSales"
            Public fIsEmpDisc As System.String = "IsEmpDisc"
            Public fIsRtnable As System.String = "IsRtnable"
            Public fIsDisc As System.String = "IsDisc"
            Public fIsFOC As System.String = "IsFOC"
            Public fIsTaxable As System.String = "IsTaxable"
            Public fAvgCost As System.String = "AvgCost"
            Public fStdCost As System.String = "StdCost"
            Public fStdMarkup As System.String = "StdMarkup"
            Public fStdSellPrice As System.String = "StdSellPrice"
            Public fIsSelected As System.String = "IsSelected"
            Public fIsBestBuy As System.String = "IsBestBuy"
            Public fIsPurchase As System.String = "IsPurchase"
            Public fIsWLength As System.String = "IsWLength"
            Public fTrackSerial As System.String = "TrackSerial"
            Public fMinQty As System.String = "MinQty"
            Public fMaxQty As System.String = "MaxQty"
            Public fImageName As System.String = "ImageName"
            Public fIncomingQty As System.String = "IncomingQty"
            Public fReOrderLvl As System.String = "ReOrderLvl"
            Public fReOrderQty As System.String = "ReOrderQty"
            Public fQtyOnHand As System.String = "QtyOnHand"
            Public fQtySellable As System.String = "QtySellable"
            Public fQtyBalance As System.String = "QtyBalance"
            Public fPOQty As System.String = "POQty"
            Public fOutgoingQty As System.String = "OutgoingQty"
            Public fFirstIn As System.String = "FirstIn"
            Public fLastIn As System.String = "LastIn"
            Public fLastPO As System.String = "LastPO"
            Public fLastOut As System.String = "LastOut"
            Public fLastRV As System.String = "LastRV"
            Public fCummQty As System.String = "CummQty"
            Public fDayQty As System.String = "DayQty"
            Public fLDayQty As System.String = "LDayQty"
            Public fMthQty As System.String = "MthQty"
            Public fLMthQty As System.String = "LMthQty"
            Public fMtdQty As System.String = "MtdQty"
            Public fYrQty As System.String = "YrQty"
            Public fLYrQty As System.String = "LYrQty"
            Public fYtdQty As System.String = "YtdQty"
            Public fProcessDate As System.String = "ProcessDate"
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

            Protected _LocID As System.String
            Protected _StorageID As System.String
            Protected _ItemCode As System.String
            Private _ItemDesc As System.String
            Private _ItemName As System.String
            Private _ItemComponent As System.String
            Private _ShortDesc As System.String
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
            Private _PackQty As System.Int32
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
            Private _Flag As System.Byte

            'Custom fields
            Protected _CompanyID As System.String
            Protected _CompanyName As System.String
            Protected _BranchName As System.String

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

            Public Property StorageID As System.String
                Get
                    Return _StorageID
                End Get
                Set(ByVal Value As System.String)
                    _StorageID = Value
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
            Public Property PackQty As System.Int32
                Get
                    Return _PackQty
                End Get
                Set(ByVal Value As System.Int32)
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
            Public Property Flag As System.Byte
                Get
                    Return _Flag
                End Get
                Set(ByVal Value As System.Byte)
                    _Flag = Value
                End Set
            End Property

            Public Property CompanyID As System.String
                Get
                    Return _CompanyID
                End Get
                Set(ByVal Value As System.String)
                    _CompanyID = Value
                End Set
            End Property
            Public Property CompanyName As System.String
                Get
                    Return _CompanyName
                End Get
                Set(ByVal Value As System.String)
                    _CompanyName = Value
                End Set
            End Property
            Public Property BranchName As System.String
                Get
                    Return _BranchName
                End Get
                Set(ByVal Value As System.String)
                    _BranchName = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class ItemlocInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "LocID,StorageID,ItemCode,ItemDesc,ItemName,ItemComponent,ShortDesc,TariffCode,OrgCountry,MATNo,MarkNo,ItmSize2,ItmSize1,ItmSize,ConSize,ConUOM,DefUOM,ClassType,TypeCode,BehvType,BehvShow,ComboItem,ItmCatgCode,ItmBrand,LooseUOM,PackUOM,PackQty,IsSales,IsEmpDisc,IsRtnable,IsDisc,IsFOC,IsTaxable,AvgCost,StdCost,StdMarkup,StdSellPrice,IsSelected,IsBestBuy,IsPurchase,IsWLength,TrackSerial,MinQty,MaxQty,ImageName,IncomingQty,ReOrderLvl,ReOrderQty,QtyOnHand,QtySellable,QtyBalance,POQty,OutgoingQty,FirstIn,LastIn,LastPO,LastOut,LastRV,CummQty,DayQty,LDayQty,MthQty,LMthQty,MtdQty,YrQty,LYrQty,YtdQty,ProcessDate,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd"
                .CheckFields = "BehvType,BehvShow,IsSales,IsEmpDisc,IsRtnable,IsDisc,IsFOC,IsTaxable,IsSelected,IsBestBuy,IsPurchase,IsWLength,TrackSerial,Active,Inuse,Flag"
                .TableName = "ITEMLOC WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "LocID,StorageID,ItemCode,ItemDesc,ItemName,ItemComponent,ShortDesc,TariffCode,OrgCountry,MATNo,MarkNo,ItmSize2,ItmSize1,ItmSize,ConSize,ConUOM,DefUOM,ClassType,TypeCode,BehvType,BehvShow,ComboItem,ItmCatgCode,ItmBrand,LooseUOM,PackUOM,PackQty,IsSales,IsEmpDisc,IsRtnable,IsDisc,IsFOC,IsTaxable,AvgCost,StdCost,StdMarkup,StdSellPrice,IsSelected,IsBestBuy,IsPurchase,IsWLength,TrackSerial,MinQty,MaxQty,ImageName,IncomingQty,ReOrderLvl,ReOrderQty,QtyOnHand,QtySellable,QtyBalance,POQty,OutgoingQty,FirstIn,LastIn,LastPO,LastOut,LastRV,CummQty,DayQty,LDayQty,MthQty,LMthQty,MtdQty,YrQty,LYrQty,YtdQty,ProcessDate,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd"
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
    Public Class ItemLocScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LocID"
                .Length = 10
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
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ShortDesc"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "TariffCode"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "OrgCountry"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "MATNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "MarkNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ItmSize2"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ItmSize1"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ItmSize"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ConSize"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ConUOM"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "DefUOM"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ClassType"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TypeCode"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "BehvType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "BehvShow"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ComboItem"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ItmCatgCode"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ItmBrand"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LooseUOM"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PackUOM"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "PackQty"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsSales"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsEmpDisc"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsRtnable"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsDisc"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(27, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsFOC"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(28, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsTaxable"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(29, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "AvgCost"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(30, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "StdCost"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(31, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "StdMarkup"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(32, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "StdSellPrice"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(33, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsSelected"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(34, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsBestBuy"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(35, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsPurchase"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(36, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsWLength"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(37, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "TrackSerial"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(38, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "MinQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(39, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "MaxQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(40, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ImageName"
                .Length = 255
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(41, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IncomingQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(42, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ReOrderLvl"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(43, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ReOrderQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(44, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "QtyOnHand"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(45, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "QtySellable"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(46, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "QtyBalance"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(47, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "POQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(48, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OutgoingQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(49, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "FirstIn"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(50, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LastIn"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(51, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LastPO"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(52, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LastOut"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(53, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LastRV"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(54, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "CummQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(55, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "DayQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(56, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LDayQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(57, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "MthQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(58, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LMthQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(59, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "MtdQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(60, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "YrQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(61, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LYrQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(62, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "YtdQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(63, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "ProcessDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(64, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(65, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(66, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(67, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(68, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(69, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(70, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(71, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(72, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(73, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(74, this)

            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ItemName"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(75, this)

            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ItemComponent"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(76, this)
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

        Public ReadOnly Property ItemDesc As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property ShortDesc As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property TariffCode As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property OrgCountry As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property MATNo As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property MarkNo As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property ItmSize2 As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property ItmSize1 As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property ItmSize As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property ConSize As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property ConUOM As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property DefUOM As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property ClassType As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property TypeCode As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property BehvType As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property BehvShow As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property ComboItem As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property ItmCatgCode As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property ItmBrand As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property LooseUOM As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property PackUOM As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property PackQty As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property IsSales As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property IsEmpDisc As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property IsRtnable As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property IsDisc As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property
        Public ReadOnly Property IsFOC As StrucElement
            Get
                Return MyBase.GetItem(28)
            End Get
        End Property
        Public ReadOnly Property IsTaxable As StrucElement
            Get
                Return MyBase.GetItem(29)
            End Get
        End Property
        Public ReadOnly Property AvgCost As StrucElement
            Get
                Return MyBase.GetItem(30)
            End Get
        End Property
        Public ReadOnly Property StdCost As StrucElement
            Get
                Return MyBase.GetItem(31)
            End Get
        End Property
        Public ReadOnly Property StdMarkup As StrucElement
            Get
                Return MyBase.GetItem(32)
            End Get
        End Property
        Public ReadOnly Property StdSellPrice As StrucElement
            Get
                Return MyBase.GetItem(33)
            End Get
        End Property
        Public ReadOnly Property IsSelected As StrucElement
            Get
                Return MyBase.GetItem(34)
            End Get
        End Property
        Public ReadOnly Property IsBestBuy As StrucElement
            Get
                Return MyBase.GetItem(35)
            End Get
        End Property
        Public ReadOnly Property IsPurchase As StrucElement
            Get
                Return MyBase.GetItem(36)
            End Get
        End Property
        Public ReadOnly Property IsWLength As StrucElement
            Get
                Return MyBase.GetItem(37)
            End Get
        End Property
        Public ReadOnly Property TrackSerial As StrucElement
            Get
                Return MyBase.GetItem(38)
            End Get
        End Property
        Public ReadOnly Property MinQty As StrucElement
            Get
                Return MyBase.GetItem(39)
            End Get
        End Property
        Public ReadOnly Property MaxQty As StrucElement
            Get
                Return MyBase.GetItem(40)
            End Get
        End Property
        Public ReadOnly Property ImageName As StrucElement
            Get
                Return MyBase.GetItem(41)
            End Get
        End Property
        Public ReadOnly Property IncomingQty As StrucElement
            Get
                Return MyBase.GetItem(42)
            End Get
        End Property
        Public ReadOnly Property ReOrderLvl As StrucElement
            Get
                Return MyBase.GetItem(43)
            End Get
        End Property
        Public ReadOnly Property ReOrderQty As StrucElement
            Get
                Return MyBase.GetItem(44)
            End Get
        End Property
        Public ReadOnly Property QtyOnHand As StrucElement
            Get
                Return MyBase.GetItem(45)
            End Get
        End Property
        Public ReadOnly Property QtySellable As StrucElement
            Get
                Return MyBase.GetItem(46)
            End Get
        End Property
        Public ReadOnly Property QtyBalance As StrucElement
            Get
                Return MyBase.GetItem(47)
            End Get
        End Property
        Public ReadOnly Property POQty As StrucElement
            Get
                Return MyBase.GetItem(48)
            End Get
        End Property
        Public ReadOnly Property OutgoingQty As StrucElement
            Get
                Return MyBase.GetItem(49)
            End Get
        End Property
        Public ReadOnly Property FirstIn As StrucElement
            Get
                Return MyBase.GetItem(50)
            End Get
        End Property
        Public ReadOnly Property LastIn As StrucElement
            Get
                Return MyBase.GetItem(51)
            End Get
        End Property
        Public ReadOnly Property LastPO As StrucElement
            Get
                Return MyBase.GetItem(52)
            End Get
        End Property
        Public ReadOnly Property LastOut As StrucElement
            Get
                Return MyBase.GetItem(53)
            End Get
        End Property
        Public ReadOnly Property LastRV As StrucElement
            Get
                Return MyBase.GetItem(54)
            End Get
        End Property
        Public ReadOnly Property CummQty As StrucElement
            Get
                Return MyBase.GetItem(55)
            End Get
        End Property
        Public ReadOnly Property DayQty As StrucElement
            Get
                Return MyBase.GetItem(56)
            End Get
        End Property
        Public ReadOnly Property LDayQty As StrucElement
            Get
                Return MyBase.GetItem(57)
            End Get
        End Property
        Public ReadOnly Property MthQty As StrucElement
            Get
                Return MyBase.GetItem(58)
            End Get
        End Property
        Public ReadOnly Property LMthQty As StrucElement
            Get
                Return MyBase.GetItem(59)
            End Get
        End Property
        Public ReadOnly Property MtdQty As StrucElement
            Get
                Return MyBase.GetItem(60)
            End Get
        End Property
        Public ReadOnly Property YrQty As StrucElement
            Get
                Return MyBase.GetItem(61)
            End Get
        End Property
        Public ReadOnly Property LYrQty As StrucElement
            Get
                Return MyBase.GetItem(62)
            End Get
        End Property
        Public ReadOnly Property YtdQty As StrucElement
            Get
                Return MyBase.GetItem(63)
            End Get
        End Property
        Public ReadOnly Property ProcessDate As StrucElement
            Get
                Return MyBase.GetItem(64)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(65)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(66)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(67)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(68)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(69)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(70)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(71)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(72)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(73)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(74)
            End Get
        End Property

        Public ReadOnly Property ItemName As StrucElement
            Get
                Return MyBase.GetItem(75)
            End Get
        End Property

        Public ReadOnly Property ItemComponent As StrucElement
            Get
                Return MyBase.GetItem(76)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace

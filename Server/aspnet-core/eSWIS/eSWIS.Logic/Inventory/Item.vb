Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Inventory
    Public NotInheritable Class Item
        Inherits Core.CoreControl
        Private ItemInfo As ItemInfo = New ItemInfo
        Private Log As New SystemLog()

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function Save(ByVal ItemCont As Container.Item, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim strSQLItemCategory As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ItemCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItemInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCont.ItemCode) & "' AND ItmCatgCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCont.ItmCatgCode) & "'")
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
                                .TableName = "ITEM WITH (ROWLOCK)"
                                .AddField("ItmDesc", ItemCont.ItmDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("ShortDesc", ItemCont.ShortDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("TariffCode", ItemCont.TariffCode, SQLControl.EnumDataType.dtStringN)
                                .AddField("OrgCountry", ItemCont.OrgCountry, SQLControl.EnumDataType.dtString)
                                .AddField("MATNo", ItemCont.MATNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("MarkNo", ItemCont.MarkNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("ItmSize2", ItemCont.ItmSize2, SQLControl.EnumDataType.dtStringN)
                                .AddField("ItmSize1", ItemCont.ItmSize1, SQLControl.EnumDataType.dtStringN)
                                .AddField("ItmSize", ItemCont.ItmSize, SQLControl.EnumDataType.dtStringN)
                                .AddField("ConSize", ItemCont.ConSize, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ConUOM", ItemCont.ConUOM, SQLControl.EnumDataType.dtString)
                                .AddField("DefUOM", ItemCont.DefUOM, SQLControl.EnumDataType.dtString)
                                .AddField("ClassType", ItemCont.ClassType, SQLControl.EnumDataType.dtString)
                                .AddField("TypeCode", ItemCont.TypeCode, SQLControl.EnumDataType.dtString)
                                .AddField("BehvType", ItemCont.BehvType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("BehvShow", ItemCont.BehvShow, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ComboItem", ItemCont.ComboItem, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ItmCatgCode", ItemCont.ItmCatgCode, SQLControl.EnumDataType.dtString)
                                .AddField("ItmBrand", ItemCont.ItmBrand, SQLControl.EnumDataType.dtStringN)
                                .AddField("LooseUOM", ItemCont.LooseUOM, SQLControl.EnumDataType.dtString)
                                .AddField("PackUOM", ItemCont.PackUOM, SQLControl.EnumDataType.dtString)
                                .AddField("PackQty", ItemCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsSales", ItemCont.ShortDesc, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsEmpDisc", ItemCont.IsEmpDisc, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsRtnable", ItemCont.IsRtnable, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsDisc", ItemCont.IsDisc, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsFOC", ItemCont.IsFOC, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsTaxable", ItemCont.IsTaxable, SQLControl.EnumDataType.dtNumeric)
                                .AddField("AvgCost", ItemCont.AvgCost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("StdCost", ItemCont.StdCost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("StdMarkup", ItemCont.StdMarkup, SQLControl.EnumDataType.dtNumeric)
                                .AddField("StdSellPrice", ItemCont.StdSellPrice, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsSelected", ItemCont.IsSelected, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsBestBuy", ItemCont.IsBestBuy, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsPurchase", ItemCont.IsPurchase, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsWLength", ItemCont.IsWLength, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", ItemCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", ItemCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", ItemCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", ItemCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", ItemCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", ItemCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", ItemCont.Flag, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCont.ItemCode) & "' AND ItmCatgCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCont.ItmCatgCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("ItemCode", ItemCont.ItemCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        .AddField("ItmCatgCode", ItemCont.ItmCatgCode, SQLControl.EnumDataType.dtString)
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCont.ItemCode) & "' AND ItmCatgCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCont.ItmCatgCode) & "'")

                                End Select
                            End With
                            Try
                                With objSQL
                                    .TableName = "ITEMCATEGORY WITH (ROWLOCK)"
                                    .AddField("Inuse", 1, SQLControl.EnumDataType.dtNumeric)
                                    strSQLItemCategory = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "CatgCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCont.ItmCatgCodeUpdate) & "'")
                                End With

                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                objConn.Execute(strSQLItemCategory, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True

                            Catch axExecute As Exception
                                Dim sqlStatement As String = " "
                                If objConn.FailedSQLStatement.Count > 0 Then
                                    sqlStatement &= objConn.FailedSQLStatement.Item(0)
                                End If

                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = message.ToString()
                                Else
                                    message = message.ToString()
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/Item", axExecute.Message & sqlStatement, axExecute.StackTrace)
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
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/Item", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/Item", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ItemCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function Insert(ByVal ItemCont As Container.Item, ByRef message As String) As Boolean
            Return Save(ItemCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        Public Function Update(ByVal ItemCont As Container.Item, ByRef message As String) As Boolean
            Return Save(ItemCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal ItemCont As Container.Item, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If ItemCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItemInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCont.ItemCode) & "' AND ItmCatgCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCont.ItmCatgCode) & "'")
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
                                strSQL = BuildUpdate("ITEM WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCont.UpdateBy) & "' WHERE" & _
                                " ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCont.ItemCode) & "' AND ItmCatgCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCont.ItmCatgCode) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("ITEM WITH (ROWLOCK)", "ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCont.ItemCode) & "' AND ItmCatgCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCont.ItmCatgCode) & "'")
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/Item", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/Item", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/Item", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                ItemCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetItem(ByVal ItemCode As System.String, ByVal ItemCategoryCode As System.String) As Container.Item
            Dim rItem As Container.Item = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With ItemInfo.MyInfo
                        StartSQLControl()
                        strSQL = BuildSelect(.FieldsList, .TableName, "ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCode) & "' AND ItmCatgCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCategoryCode) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rItem = New Container.Item
                                rItem.ItemCode = drRow.Item("ItemCode")
                                If Not IsDBNull(drRow.Item("ItmDesc")) Then
                                    rItem.ItmDesc = drRow.Item("ItmDesc")
                                End If
                                rItem.ShortDesc = drRow.Item("ShortDesc")
                                rItem.TariffCode = drRow.Item("TariffCode")
                                rItem.OrgCountry = drRow.Item("OrgCountry")
                                rItem.MATNo = drRow.Item("MATNo")
                                rItem.MarkNo = drRow.Item("MarkNo")
                                rItem.ItmSize2 = drRow.Item("ItmSize2")
                                rItem.ItmSize1 = drRow.Item("ItmSize1")
                                rItem.ItmSize = drRow.Item("ItmSize")
                                rItem.ConSize = drRow.Item("ConSize")
                                rItem.ConUOM = drRow.Item("ConUOM")
                                rItem.DefUOM = drRow.Item("DefUOM")
                                rItem.ClassType = drRow.Item("ClassType")
                                rItem.BehvType = drRow.Item("BehvType")
                                rItem.BehvShow = drRow.Item("BehvShow")
                                rItem.ComboItem = drRow.Item("ComboItem")
                                rItem.ItmCatgCode = drRow.Item("ItmCatgCode")
                                rItem.ItmBrand = drRow.Item("ItmBrand")
                                rItem.LooseUOM = drRow.Item("LooseUOM")
                                rItem.PackUOM = drRow.Item("PackUOM")
                                rItem.PackQty = drRow.Item("PackQty")
                                rItem.IsSales = drRow.Item("IsSales")
                                rItem.IsEmpDisc = drRow.Item("IsEmpDisc")
                                rItem.IsRtnable = drRow.Item("IsRtnable")
                                rItem.IsDisc = drRow.Item("IsDisc")
                                rItem.IsFOC = drRow.Item("IsFOC")
                                rItem.IsTaxable = drRow.Item("IsTaxable")
                                rItem.AvgCost = drRow.Item("AvgCost")
                                rItem.StdCost = drRow.Item("StdCost")
                                rItem.StdMarkup = drRow.Item("StdMarkup")
                                rItem.StdSellPrice = drRow.Item("StdSellPrice")
                                rItem.IsSelected = drRow.Item("IsSelected")
                                rItem.IsBestBuy = drRow.Item("IsBestBuy")
                                rItem.IsPurchase = drRow.Item("IsPurchase")
                                rItem.IsWLength = drRow.Item("IsWLength")
                                rItem.CreateDate = IIf(IsDBNull(drRow.Item("CreateDate")), Now(), drRow.Item("CreateDate"))
                                rItem.CreateBy = drRow.Item("CreateBy")
                                rItem.UpdateBy = drRow.Item("UpdateBy")
                                rItem.Active = drRow.Item("Active")
                                rItem.Inuse = drRow.Item("Inuse")
                                rItem.rowguid = drRow.Item("rowguid")
                                rItem.SyncCreate = IIf(IsDBNull(drRow.Item("SyncCreate")), Now(), drRow.Item("SyncCreate"))
                                rItem.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rItem.CategoryDesc = IIf(IsDBNull(drRow.Item("CategoryDesc")), "", drRow.Item("CategoryDesc"))

                            Else
                                rItem = Nothing
                            End If
                        Else
                            rItem = Nothing
                        End If
                    End With
                End If
                Return rItem
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/Item", ex.Message, ex.StackTrace)
            Finally
                rItem = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWaste(ByVal ItemCode As System.String) As Container.Item
            Dim rItem As Container.Item = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With ItemInfo.MyInfo
                        StartSQLControl()
                        strSQL = BuildSelect(.FieldsList, .TableName, "ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCode) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rItem = New Container.Item
                                rItem.ItemCode = drRow.Item("ItemCode")
                                If Not IsDBNull(drRow.Item("ItmDesc")) Then
                                    rItem.ItmDesc = drRow.Item("ItmDesc")
                                End If
                                If Not IsDBNull(drRow.Item("TypeCode")) Then
                                    rItem.TypeCode = drRow.Item("TypeCode")
                                Else
                                    rItem.TypeCode = ""
                                End If
                                rItem.ShortDesc = drRow.Item("ShortDesc")
                                rItem.TariffCode = drRow.Item("TariffCode")
                                rItem.OrgCountry = drRow.Item("OrgCountry")
                                rItem.MATNo = drRow.Item("MATNo")
                                rItem.MarkNo = drRow.Item("MarkNo")
                                rItem.ItmSize2 = drRow.Item("ItmSize2")
                                rItem.ItmSize1 = drRow.Item("ItmSize1")
                                rItem.ItmSize = drRow.Item("ItmSize")
                                rItem.ConSize = drRow.Item("ConSize")
                                rItem.ConUOM = drRow.Item("ConUOM")
                                rItem.DefUOM = drRow.Item("DefUOM")
                                rItem.ClassType = drRow.Item("ClassType")
                                rItem.BehvType = drRow.Item("BehvType")
                                rItem.BehvShow = drRow.Item("BehvShow")
                                rItem.ComboItem = drRow.Item("ComboItem")
                                rItem.ItmCatgCode = drRow.Item("ItmCatgCode")
                                rItem.ItmBrand = drRow.Item("ItmBrand")
                                rItem.LooseUOM = drRow.Item("LooseUOM")
                                rItem.PackUOM = drRow.Item("PackUOM")
                                rItem.PackQty = drRow.Item("PackQty")
                                rItem.IsSales = drRow.Item("IsSales")
                                rItem.IsEmpDisc = drRow.Item("IsEmpDisc")
                                rItem.IsRtnable = drRow.Item("IsRtnable")
                                rItem.IsDisc = drRow.Item("IsDisc")
                                rItem.IsFOC = drRow.Item("IsFOC")
                                rItem.IsTaxable = drRow.Item("IsTaxable")
                                rItem.AvgCost = drRow.Item("AvgCost")
                                rItem.StdCost = drRow.Item("StdCost")
                                rItem.StdMarkup = drRow.Item("StdMarkup")
                                rItem.StdSellPrice = drRow.Item("StdSellPrice")
                                rItem.IsSelected = drRow.Item("IsSelected")
                                rItem.IsBestBuy = drRow.Item("IsBestBuy")
                                rItem.IsPurchase = drRow.Item("IsPurchase")
                                rItem.IsWLength = drRow.Item("IsWLength")
                                rItem.CreateDate = IIf(IsDBNull(drRow.Item("CreateDate")), Now(), drRow.Item("CreateDate"))
                                rItem.CreateBy = drRow.Item("CreateBy")
                                rItem.UpdateBy = drRow.Item("UpdateBy")
                                rItem.Active = drRow.Item("Active")
                                rItem.Inuse = drRow.Item("Inuse")
                                rItem.rowguid = drRow.Item("rowguid")
                                rItem.SyncCreate = IIf(IsDBNull(drRow.Item("SyncCreate")), Now(), drRow.Item("SyncCreate"))
                                rItem.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rItem.CategoryDesc = IIf(IsDBNull(drRow.Item("CategoryDesc")), "", drRow.Item("CategoryDesc"))

                            Else
                                rItem = Nothing
                            End If
                        Else
                            rItem = Nothing
                        End If
                    End With
                End If
                Return rItem
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/Item", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rItem = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetItem(ByVal ItemCode As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Item)
            Dim rItem As Container.Item = Nothing
            Dim lstItem As List(Of Container.Item) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With ItemInfo.MyInfo
                        StartSQLControl()
                        If DecendingOrder Then
                            strDesc = " Order by ByVal ItemCode As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "ItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCode) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rItem = New Container.Item
                                rItem.ItemCode = drRow.Item("ItemCode")
                                rItem.ItmDesc = drRow.Item("ItmDesc")
                                rItem.ShortDesc = drRow.Item("ShortDesc")
                                rItem.TariffCode = drRow.Item("TariffCode")
                                rItem.OrgCountry = drRow.Item("OrgCountry")
                                rItem.MATNo = drRow.Item("MATNo")
                                rItem.MarkNo = drRow.Item("MarkNo")
                                rItem.ItmSize2 = drRow.Item("ItmSize2")
                                rItem.ItmSize1 = drRow.Item("ItmSize1")
                                rItem.ItmSize = drRow.Item("ItmSize")
                                rItem.ConSize = drRow.Item("ConSize")
                                rItem.ConUOM = drRow.Item("ConUOM")
                                rItem.DefUOM = drRow.Item("DefUOM")
                                rItem.ClassType = drRow.Item("ClassType")
                                rItem.BehvType = drRow.Item("BehvType")
                                rItem.BehvShow = drRow.Item("BehvShow")
                                rItem.ComboItem = drRow.Item("ComboItem")
                                rItem.ItmCatgCode = drRow.Item("ItmCatgCode")
                                rItem.ItmBrand = drRow.Item("ItmBrand")
                                rItem.LooseUOM = drRow.Item("LooseUOM")
                                rItem.PackUOM = drRow.Item("PackUOM")
                                rItem.PackQty = drRow.Item("PackQty")
                                rItem.IsSales = drRow.Item("IsSales")
                                rItem.IsEmpDisc = drRow.Item("IsEmpDisc")
                                rItem.IsRtnable = drRow.Item("IsRtnable")
                                rItem.IsDisc = drRow.Item("IsDisc")
                                rItem.IsFOC = drRow.Item("IsFOC")
                                rItem.IsTaxable = drRow.Item("IsTaxable")
                                rItem.AvgCost = drRow.Item("AvgCost")
                                rItem.StdCost = drRow.Item("StdCost")
                                rItem.StdMarkup = drRow.Item("StdMarkup")
                                rItem.StdSellPrice = drRow.Item("StdSellPrice")
                                rItem.IsSelected = drRow.Item("IsSelected")
                                rItem.IsBestBuy = drRow.Item("IsBestBuy")
                                rItem.IsPurchase = drRow.Item("IsPurchase")
                                rItem.IsWLength = drRow.Item("IsWLength")
                                rItem.CreateDate = drRow.Item("CreateDate")
                                rItem.CreateBy = drRow.Item("CreateBy")
                                rItem.UpdateBy = drRow.Item("UpdateBy")
                                rItem.Active = drRow.Item("Active")
                                rItem.Inuse = drRow.Item("Inuse")
                                rItem.rowguid = drRow.Item("rowguid")
                                rItem.SyncCreate = drRow.Item("SyncCreate")
                                rItem.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rItem.IsHost = drRow.Item("IsHost")
                                rItem.LastSyncBy = drRow.Item("LastSyncBy")
                                rItem.CategoryDesc = drRow.Item("CategoryDesc")
                            Next
                            lstItem.Add(rItem)
                        Else
                            rItem = Nothing
                        End If
                        Return lstItem
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/Item", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rItem = Nothing
                lstItem = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetItemList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ItemInfo.MyInfo
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

        Public Overloads Function GetItemList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With ItemInfo.MyInfo
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

        'Add
        Public Overloads Function GetItemCustomList(Optional ByVal WasteCode As String = "", Optional ByVal WasteType As String = "", Optional ByVal param As Boolean = True) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With ItemInfo.MyInfo

                    strSQL = "SELECT CM.Code, CM.CodeDesc as WasteType, CM.Code as WasType,  IT.* " & _
                            " FROM ITEM IT WITH (NOLOCK)  " & _
                            " INNER JOIN WAC_WASTYPE T ON T.WasCode=IT.ItemCode " & _
                            " LEFT JOIN CODEMASTER CM WITH (NOLOCK) ON CM.CodeType='WTY' AND CM.Code=T.TypeCode " & _
                            " LEFT JOIN WAC_BASELINE BL WITH (NOLOCK) ON BL.WasCode = IT.ItemCode AND BL.WasType = CM.Code AND BL.Flag = 1 AND BL.Active = 1 " & _
                            " WHERE IT.FLAG=1 "

                    If param Then
                        strSQL &= " AND BL.BASEID IS NULL"
                    Else
                        strSQL &= " AND IT.ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "' AND CM.Code='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "'"
                    End If

                    strSQL &= " ORDER BY ItemCode, Code"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        'amended by Richard 20140916, not directly filter by Location only
        Public Overloads Function GetWasteCodesList(Optional ByVal Condition As String = "") As Data.DataTable
            If StartConnection() = True Then
                With ItemInfo.MyInfo
                    strSQL = "SELECT SM.LocID, SM.ItemCode, ISNULL(I.ItmDesc,'') AS ItemDesc, SM.StorageID, SM.StorageAreaCode, SM.StorageBin " & _
                        " FROM StorageMaster SM WITH (NOLOCK) " & _
                        " LEFT JOIN Item I WITH (NOLOCK) ON I.ItemCode=SM.ItemCode " & _
                        " WHERE SM.Flag=1"

                    strSQL = "SELECT ItemCode as ItemCode, ItmDesc as ItemDesc FROM Item WITH (NOLOCK) WHERE ItemCode like 'SW%' and Flag=1"

                    If Not Condition Is Nothing And Condition <> "" Then strSQL &= Condition

                    strSQL &= " ORDER BY ItemCode"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetWasteCodesListLicense(ByVal LocID As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With ItemInfo.MyInfo
                    strSQL = "SELECT DISTINCT Item.ItemCode as ItemCode, Item.ItmDesc as ItemDesc FROM Item WITH (NOLOCK) INNER JOIN LICENSEITEM I ON Item.ItemCode=I.ItemCode INNER JOIN LICENSE L ON I.ContractNo=L.ContractNo AND L.Active=1 AND L.Flag=1 AND L.ValidityEnd>=CONVERT(date,GETDATE()) WHERE Item.ItemCode like 'SW%' and Item.Flag=1 AND L.LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "'"
                    strSQL &= " ORDER BY Item.ItemCode"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetWasteCodeList() As List(Of Container.Item)
            Dim dtTemp As DataTable = Nothing
            Dim rItem As Container.Item = Nothing
            Dim lstItem As List(Of Container.Item) = Nothing
            Try
                If StartConnection() = True Then
                    With ItemInfo.MyInfo

                        strSQL = "SELECT ItemCode as ItemCode, ItmDesc as ItemDesc FROM Item WITH (NOLOCK) WHERE ItemCode like 'SW%' and Flag=1"

                        strSQL &= " ORDER BY ItemCode"
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            lstItem = New List(Of Container.Item)
                            For Each drRow As DataRow In dtTemp.Rows
                                rItem = New Container.Item
                                rItem.ItemCode = drRow.Item("ItemCode")
                                rItem.ItmDesc = drRow.Item("ItemDesc")
                                lstItem.Add(rItem)
                            Next

                        Else
                            rItem = Nothing
                        End If
                        Return lstItem
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/Item", ex.Message, ex.StackTrace)
            Finally
                rItem = Nothing
                lstItem = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWasteCodeListByWGNotification(Optional ByVal LocID As String = "", Optional ByVal WasteCode As String = "", Optional ByVal WasteType As String = "", Optional ByVal param As Boolean = True) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With ItemInfo.MyInfo
                    strSQL = "select ROW_NUMBER() OVER (ORDER BY ItemCode) AS ItemNo, d.ItemCode as ItemCode, CodeDesc as WasteType, b.WasType, ItmDesc, ItmName, d.TransNo, ItmSource, h.TransDate" & _
                        " from Notifydtl d INNER JOIN CODEMASTER c ON d.TypeCode = c.Code AND c.CodeType = 'WTY'" & _
                        " INNER JOIN WAC_BASELINE b ON d.ItemCode = b.WasCode AND b.WasType = c.CodeSeq" & _
                        " INNER JOIN NOTIFYHDR h on d.TransNo = h.TransNo AND h.Active=1 AND h.Flag=1 AND h.Posted=1" & _
                        " LEFT JOIN WAC_CHARMASTER ch on d.ItemCode = ch.WasCode AND ch.WasType = c.CodeSeq and ch.LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' and ch.Flag=1" & _
                        " WHERE d.LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "'"

                    If param Then
                        strSQL &= " and ch.LocId IS NULL"
                    Else
                        strSQL &= " and ItemCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "'" & _
                            " AND b.WasType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "'"
                    End If

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWasteCodeListByLicense(Optional ByVal LocID As String = "", Optional ByVal CompanyID As String = "", Optional ByVal WasteCode As String = "", Optional ByVal WasteType As String = "", Optional ByVal param As Boolean = True) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With ItemInfo.MyInfo

                    strSQL = "SELECT DISTINCT CM.Code, CM.CodeDesc as WasteType, CM.Code as WasType, IT.*, Bc.TestingWaste, Bc.BaseID FROM LICENSEITEM LI WITH (NOLOCK)" & _
                        " INNER JOIN LICENSE LC ON LC.ContractNo = LI.ContractNo INNER JOIN ITEM IT ON IT.ItemCode = LI.ItemCode" & _
                        " INNER JOIN WAC_WASTYPE T ON T.WasCode=IT.ItemCode " & _
                        " LEFT JOIN CODEMASTER CM WITH (NOLOCK) ON CM.CodeType='WTY' AND CM.Code=T.TypeCode " & _
                        " INNER JOIN WAC_BASELINE BC WITH (NOLOCK) ON BC.WasCode = IT.ItemCode AND BC.WasType = CM.Code AND BC.FLAG=1 AND BC.ACTIVE=1 AND BC.Allow3R=1 " & _
                        " LEFT JOIN WAC_ACCMASTER AC WITH (NOLOCK) ON AC.WasCode = IT.ItemCode AND AC.WasType = CM.Code AND AC.LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND AC.FLAG=1 AND AC.ACTIVE=1" & _
                        " WHERE CM.CodeType='WTY'" & _
                        " AND LC.LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND LC.CompanyID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompanyID) & "'" & _
                        " AND LC.ValidityEnd >= Convert(Date, GETDATE()) AND ContType='R'"

                    If param Then
                        strSQL &= " and AC.LocId IS NULL"
                    Else
                        strSQL &= " and IT.ItemCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "'" & _
                            " AND CM.Code = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "'"
                    End If

                    strSQL &= " ORDER BY ItemCode, Code"
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
        Public Class Item
            Public fItemCode As System.String = "ItemCode"
            Public fItmDesc As System.String = "ItmDesc"
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
            Public fTypeCode As System.String = "TypeCode"
            Public fBehvType As System.String = "BehvType"
            Public fBehvShow As System.String = "BehvShow"
            Public fComboItem As System.String = "ComboItem"
            Public fItmCatgCode As System.String = "ItmCatgCode"
            Public fCategoryDesc As System.String = "CategoryDesc"
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
            Public fItmCatgCodeUpdate As System.String = "ItmCatgCode"


            Protected _ItemCode As System.String
            Private _ItmDesc As System.String
            Private _ShortDesc As System.String
            Private _TariffCode As System.String
            Private _OrgCountry As System.String
            Private _MATNo As System.String
            Private _MarkNo As System.String
            Private _ItmSize2 As System.String
            Private _ItmSize1 As System.String
            Private _ItmSize As System.String
            Private _ConSize As System.Byte
            Private _ConUOM As System.String
            Private _DefUOM As System.String
            Private _ClassType As System.String
            Private _TypeCode As System.String
            Private _BehvType As System.Byte
            Private _BehvShow As System.Byte
            Private _ComboItem As System.Byte
            Private _ItmCatgCode As System.String
            Private _ItmBrand As System.String
            Private _LooseUOM As System.String
            Private _PackUOM As System.String
            Private _PackQty As System.Byte
            Private _IsSales As System.Byte
            Private _IsEmpDisc As System.Byte
            Private _IsRtnable As System.Byte
            Private _IsDisc As System.Byte
            Private _IsFOC As System.Byte
            Private _IsTaxable As System.Byte
            Private _AvgCost As System.Byte
            Private _StdCost As System.Byte
            Private _StdMarkup As System.Byte
            Private _StdSellPrice As System.Byte
            Private _IsSelected As System.Byte
            Private _IsBestBuy As System.Byte
            Private _IsPurchase As System.Byte
            Private _IsWLength As System.Byte
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
            Private _Flag As System.Byte
            Private _ItmCatgCodeUpdate As System.String
            Protected _CategoryDesc As System.String


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
            ''' Allow Null
            ''' </summary>
            Public Property ItmDesc As System.String
                Get
                    Return _ItmDesc
                End Get
                Set(ByVal Value As System.String)
                    _ItmDesc = Value
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
            Public Property ConSize As System.Byte
                Get
                    Return _ConSize
                End Get
                Set(ByVal Value As System.Byte)
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
            Public Property ComboItem As System.Byte
                Get
                    Return _ComboItem
                End Get
                Set(ByVal Value As System.Byte)
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
            Public Property PackQty As System.Byte
                Get
                    Return _PackQty
                End Get
                Set(ByVal Value As System.Byte)
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
            Public Property AvgCost As System.Byte
                Get
                    Return _AvgCost
                End Get
                Set(ByVal Value As System.Byte)
                    _AvgCost = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property StdCost As System.Byte
                Get
                    Return _StdCost
                End Get
                Set(ByVal Value As System.Byte)
                    _StdCost = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property StdMarkup As System.Byte
                Get
                    Return _StdMarkup
                End Get
                Set(ByVal Value As System.Byte)
                    _StdMarkup = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property StdSellPrice As System.Byte
                Get
                    Return _StdSellPrice
                End Get
                Set(ByVal Value As System.Byte)
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
            Public Property Flag As System.Byte
                Get
                    Return _Flag
                End Get
                Set(ByVal Value As System.Byte)
                    _Flag = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ItmCatgCodeUpdate As System.String
                Get
                    Return _ItmCatgCodeUpdate
                End Get
                Set(ByVal Value As System.String)
                    _ItmCatgCodeUpdate = Value
                End Set
            End Property


            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CategoryDesc As System.String
                Get
                    Return _CategoryDesc
                End Get
                Set(ByVal Value As System.String)
                    _CategoryDesc = Value
                End Set
            End Property
        End Class
#End Region

    End Namespace

#Region "Class Info"
    Public Class ItemInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "ItemCode,ItmDesc,ShortDesc,TariffCode,OrgCountry,MATNo,MarkNo,ItmSize2,ItmSize1,ItmSize,ConSize,ConUOM,DefUOM,ClassType,TypeCode,BehvType,BehvShow,ComboItem,ItmCatgCode,(SELECT CatgDesc FROM ITEMCATEGORY WITH (NOLOCK) WHERE CatgCode=ITEM.ItmCatgCode) AS CategoryDesc,ItmBrand,LooseUOM,PackUOM,PackQty,IsSales,IsEmpDisc,IsRtnable,IsDisc,IsFOC,IsTaxable,AvgCost,StdCost,StdMarkup,StdSellPrice,IsSelected,IsBestBuy,IsPurchase,IsWLength,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd"
                .CheckFields = "Active,Inuse,Flag"
                .TableName = "ITEM WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "ItemCode,ItmDesc,ShortDesc,TariffCode,OrgCountry,MATNo,MarkNo,ItmSize2,ItmSize1,ItmSize,ConSize,ConUOM,DefUOM,ClassType,TypeCode,BehvType,BehvShow,ComboItem,ItmCatgCode,ItmBrand,LooseUOM,PackUOM,PackQty,IsSales,IsEmpDisc,IsRtnable,IsDisc,IsFOC,IsTaxable,AvgCost,StdCost,StdMarkup,StdSellPrice,IsSelected,IsBestBuy,IsPurchase,IsWLength,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd"
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
    Public Class ItemScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ItemCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ItmDesc"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ShortDesc"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "TariffCode"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "OrgCountry"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "MATNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "MarkNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ItmSize2"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ItmSize1"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ItmSize"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ConSize"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ConUOM"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "DefUOM"
                .Length = 3
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
                .Length = 5
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
                .DataType = SQLControl.EnumDataType.dtStringN
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
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsSales"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsEmpDisc"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsRtnable"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsRtnable"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(27, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsDisc"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(28, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsFOC"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(29, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsTaxable"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(30, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "AvgCost"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(31, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "StdCost"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(32, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "StdMarkup"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(33, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "StdSellPrice"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(34, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsSelected"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(35, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsBestBuy"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(36, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsPurchase"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(37, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsWLength"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(38, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(39, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(40, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(41, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(42, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(43, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(44, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(45, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(46, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(47, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(48, this)

        End Sub

        Public ReadOnly Property ItemCode As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property ItmDesc As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property ShortDesc As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property TariffCode As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property OrgCountry As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property MATNo As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property MarkNo As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property ItmSize2 As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property ItmSize1 As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property ItmSize As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property ConSize As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property

        Public ReadOnly Property ConUOM As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property DefUOM As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property ClassType As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property TypeCode As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property BehvType As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property BehvShow As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property ComboItem As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property ItmCatgCode As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property ItmBrand As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property LooseUOM As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property

        Public ReadOnly Property PackUOM As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property PackQty As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property IsSales As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property IsEmpDisc As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property IsRtnable As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property IsDisc As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property IsFOC As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property
        Public ReadOnly Property IsTaxable As StrucElement
            Get
                Return MyBase.GetItem(28)
            End Get
        End Property
        Public ReadOnly Property AvgCost As StrucElement
            Get
                Return MyBase.GetItem(29)
            End Get
        End Property
        Public ReadOnly Property StdCost As StrucElement
            Get
                Return MyBase.GetItem(30)
            End Get
        End Property

        Public ReadOnly Property StdMarkup As StrucElement
            Get
                Return MyBase.GetItem(31)
            End Get
        End Property
        Public ReadOnly Property StdSellPrice As StrucElement
            Get
                Return MyBase.GetItem(32)
            End Get
        End Property
        Public ReadOnly Property IsSelected As StrucElement
            Get
                Return MyBase.GetItem(33)
            End Get
        End Property
        Public ReadOnly Property IsBestBuy As StrucElement
            Get
                Return MyBase.GetItem(34)
            End Get
        End Property
        Public ReadOnly Property IsPurchase As StrucElement
            Get
                Return MyBase.GetItem(35)
            End Get
        End Property
        Public ReadOnly Property IsWLength As StrucElement
            Get
                Return MyBase.GetItem(36)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(37)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(38)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(39)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(40)
            End Get
        End Property

        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(41)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(42)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(43)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(44)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(45)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(46)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(47)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(48)
            End Get
        End Property
    End Class
#End Region


End Namespace


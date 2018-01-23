Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace GeneralSettings
    Public NotInheritable Class Branch
        Inherits Core.CoreControl
        Private SYSBRANCHInfo As SYSBRANCHInfo = New SYSBRANCHInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal SYSBRANCHCont As Container.SYSBRANCH, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If SYSBRANCHCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With SYSBRANCHInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, SYSBRANCHCont.BranchID) & "'")
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
                                .TableName = "SYSBRANCH"
                                .AddField("RevGroup", SYSBRANCHCont.RevGroup, SQLControl.EnumDataType.dtString)
                                .AddField("BranchName", SYSBRANCHCont.BranchName, SQLControl.EnumDataType.dtStringN)
                                .AddField("BranchCode", SYSBRANCHCont.BranchCode, SQLControl.EnumDataType.dtString)
                                .AddField("AccNo", SYSBRANCHCont.AccNo, SQLControl.EnumDataType.dtString)
                                .AddField("Address1", SYSBRANCHCont.Address1, SQLControl.EnumDataType.dtString)
                                .AddField("Address2", SYSBRANCHCont.Address2, SQLControl.EnumDataType.dtString)
                                .AddField("Address3", SYSBRANCHCont.Address3, SQLControl.EnumDataType.dtString)
                                .AddField("Address4", SYSBRANCHCont.Address4, SQLControl.EnumDataType.dtString)
                                .AddField("PostalCode", SYSBRANCHCont.PostalCode, SQLControl.EnumDataType.dtString)
                                .AddField("Contact1", SYSBRANCHCont.Contact1, SQLControl.EnumDataType.dtString)
                                .AddField("Contact2", SYSBRANCHCont.Contact2, SQLControl.EnumDataType.dtString)
                                .AddField("StoreType", SYSBRANCHCont.StoreType, SQLControl.EnumDataType.dtString)
                                .AddField("Email", SYSBRANCHCont.Email, SQLControl.EnumDataType.dtStringN)
                                .AddField("Tel", SYSBRANCHCont.Tel, SQLControl.EnumDataType.dtString)
                                .AddField("Fax", SYSBRANCHCont.Fax, SQLControl.EnumDataType.dtString)
                                .AddField("Region", SYSBRANCHCont.Region, SQLControl.EnumDataType.dtString)
                                .AddField("Country", SYSBRANCHCont.Country, SQLControl.EnumDataType.dtString)
                                .AddField("State", SYSBRANCHCont.State, SQLControl.EnumDataType.dtString)
                                .AddField("Currency", SYSBRANCHCont.Currency, SQLControl.EnumDataType.dtString)
                                .AddField("StoreStatus", SYSBRANCHCont.StoreStatus, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpPrevBook", SYSBRANCHCont.OpPrevBook, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpTimeStart", SYSBRANCHCont.OpTimeStart, SQLControl.EnumDataType.dtString)
                                .AddField("OpTimeEnd", SYSBRANCHCont.OpTimeEnd, SQLControl.EnumDataType.dtString)
                                .AddField("OpDay1", SYSBRANCHCont.OpDay1, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpDay2", SYSBRANCHCont.OpDay2, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpDay3", SYSBRANCHCont.OpDay3, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpDay4", SYSBRANCHCont.OpDay4, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpDay5", SYSBRANCHCont.OpDay5, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpDay6", SYSBRANCHCont.OpDay6, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpDay7", SYSBRANCHCont.OpDay7, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpBookAlwDY", SYSBRANCHCont.OpBookAlwDY, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpBookAlwHR", SYSBRANCHCont.OpBookAlwHR, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpBookFirst", SYSBRANCHCont.OpBookFirst, SQLControl.EnumDataType.dtString)
                                .AddField("OpBookLast", SYSBRANCHCont.OpBookLast, SQLControl.EnumDataType.dtString)
                                .AddField("OpBookIntv", SYSBRANCHCont.OpBookIntv, SQLControl.EnumDataType.dtNumeric)
                                .AddField("SalesItemType", SYSBRANCHCont.SalesItemType, SQLControl.EnumDataType.dtStringN)
                                .AddField("InSvcItemType", SYSBRANCHCont.InSvcItemType, SQLControl.EnumDataType.dtStringN)
                                .AddField("GenInSvcID", SYSBRANCHCont.GenInSvcID, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RcpHeader", SYSBRANCHCont.RcpHeader, SQLControl.EnumDataType.dtStringN)
                                .AddField("RcpFooter", SYSBRANCHCont.RcpFooter, SQLControl.EnumDataType.dtStringN)
                                .AddField("PriceLevel", SYSBRANCHCont.PriceLevel, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsStockTake", SYSBRANCHCont.IsStockTake, SQLControl.EnumDataType.dtNumeric)
                                .AddField("rowguid", SYSBRANCHCont.rowguid, SQLControl.EnumDataType.dtString)
                                .AddField("IsHost", SYSBRANCHCont.IsHost, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "Where BranchID ='" & .ParseValue(SQLControl.EnumDataType.dtString, SYSBRANCHCont.BranchID) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("BranchID", SYSBRANCHCont.BranchID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE BranchID='" & .ParseValue(SQLControl.EnumDataType.dtString, SYSBRANCHCont.BranchID) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                Else
                                    message = axExecute.Message.ToString()
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/Branch", axExecute.Message & strSQL, axExecute.StackTrace)
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
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/Branch", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/Branch", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                SYSBRANCHCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal SYSBRANCHCont As Container.SYSBRANCH, ByRef message As String) As Boolean
            Return Save(SYSBRANCHCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal SYSBRANCHCont As Container.SYSBRANCH, ByRef message As String) As Boolean
            Return Save(SYSBRANCHCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal SYSBRANCHCont As Container.SYSBRANCH, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If SYSBRANCHCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With SYSBRANCHInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, SYSBRANCHCont.BranchID) & "'")
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
                                strSQL = BuildUpdate(SYSBRANCHInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , SyncLastUpd = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , LastSyncBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, SYSBRANCHCont.LastSyncBy) & "'" & _
                                " WHERE BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, SYSBRANCHCont.BranchID) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(SYSBRANCHInfo.MyInfo.TableName, "BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, SYSBRANCHCont.BranchID) & "'")
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/Branch", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/Branch", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/Branch", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                SYSBRANCHCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetBranch(ByVal BranchID As System.String) As Container.SYSBRANCH
            Dim rSYSBRANCH As Container.SYSBRANCH = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With SYSBRANCHInfo.MyInfo
                        StartSQLControl()
                        strSQL = BuildSelect(.FieldsList, .TableName, "BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rSYSBRANCH = New Container.SYSBRANCH
                                rSYSBRANCH.BranchID = drRow.Item("BranchID")
                                rSYSBRANCH.RevGroup = drRow.Item("RevGroup")
                                rSYSBRANCH.BranchName = drRow.Item("BranchName")
                                rSYSBRANCH.BranchCode = drRow.Item("BranchCode")
                                rSYSBRANCH.AccNo = drRow.Item("AccNo")
                                rSYSBRANCH.Address1 = drRow.Item("Address1")
                                rSYSBRANCH.Address2 = drRow.Item("Address2")
                                rSYSBRANCH.Address3 = drRow.Item("Address3")
                                rSYSBRANCH.Address4 = drRow.Item("Address4")
                                rSYSBRANCH.PostalCode = drRow.Item("PostalCode")
                                rSYSBRANCH.Contact1 = drRow.Item("Contact1")
                                rSYSBRANCH.Contact2 = drRow.Item("Contact2")
                                rSYSBRANCH.StoreType = drRow.Item("StoreType")
                                rSYSBRANCH.Email = drRow.Item("Email")
                                rSYSBRANCH.Tel = drRow.Item("Tel")
                                rSYSBRANCH.Fax = drRow.Item("Fax")
                                rSYSBRANCH.Region = drRow.Item("Region")
                                rSYSBRANCH.Country = drRow.Item("Country")
                                rSYSBRANCH.State = drRow.Item("State")
                                rSYSBRANCH.Currency = drRow.Item("Currency")
                                rSYSBRANCH.StoreStatus = drRow.Item("StoreStatus")
                                rSYSBRANCH.OpPrevBook = drRow.Item("OpPrevBook")
                                rSYSBRANCH.OpTimeStart = drRow.Item("OpTimeStart")
                                rSYSBRANCH.OpTimeEnd = drRow.Item("OpTimeEnd")
                                rSYSBRANCH.OpDay1 = drRow.Item("OpDay1")
                                rSYSBRANCH.OpDay2 = drRow.Item("OpDay2")
                                rSYSBRANCH.OpDay3 = drRow.Item("OpDay3")
                                rSYSBRANCH.OpDay4 = drRow.Item("OpDay4")
                                rSYSBRANCH.OpDay5 = drRow.Item("OpDay5")
                                rSYSBRANCH.OpDay6 = drRow.Item("OpDay6")
                                rSYSBRANCH.OpDay7 = drRow.Item("OpDay7")
                                rSYSBRANCH.OpBookAlwDY = drRow.Item("OpBookAlwDY")
                                rSYSBRANCH.OpBookAlwHR = drRow.Item("OpBookAlwHR")
                                rSYSBRANCH.OpBookFirst = drRow.Item("OpBookFirst")
                                rSYSBRANCH.OpBookLast = drRow.Item("OpBookLast")
                                rSYSBRANCH.OpBookIntv = drRow.Item("OpBookIntv")
                                rSYSBRANCH.SalesItemType = drRow.Item("SalesItemType")
                                rSYSBRANCH.InSvcItemType = drRow.Item("InSvcItemType")
                                rSYSBRANCH.GenInSvcID = drRow.Item("GenInSvcID")
                                rSYSBRANCH.RcpHeader = drRow.Item("RcpHeader")
                                rSYSBRANCH.RcpFooter = drRow.Item("RcpFooter")
                                rSYSBRANCH.PriceLevel = drRow.Item("PriceLevel")
                                rSYSBRANCH.IsStockTake = drRow.Item("IsStockTake")
                                rSYSBRANCH.rowguid = drRow.Item("rowguid")
                                rSYSBRANCH.SyncCreate = drRow.Item("SyncCreate")
                                rSYSBRANCH.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rSYSBRANCH.IsHost = drRow.Item("IsHost")
                                rSYSBRANCH.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rSYSBRANCH = Nothing
                            End If
                        Else
                            rSYSBRANCH = Nothing
                        End If
                    End With
                End If
                Return rSYSBRANCH
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/Branch", ex.Message, ex.StackTrace)
            Finally
                rSYSBRANCH = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetBranch(ByVal BranchID As System.String, DecendingOrder As Boolean) As List(Of Container.SYSBRANCH)
            Dim rSYSBRANCH As Container.SYSBRANCH = Nothing
            Dim lstSYSBRANCH As List(Of Container.SYSBRANCH) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With SYSBRANCHInfo.MyInfo
                        StartSQLControl()
                        If DecendingOrder Then
                            strDesc = " Order by ByVal BranchID As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rSYSBRANCH = New Container.SYSBRANCH
                                rSYSBRANCH.BranchID = drRow.Item("BranchID")
                                rSYSBRANCH.RevGroup = drRow.Item("RevGroup")
                                rSYSBRANCH.BranchName = drRow.Item("BranchName")
                                rSYSBRANCH.BranchCode = drRow.Item("BranchCode")
                                rSYSBRANCH.AccNo = drRow.Item("AccNo")
                                rSYSBRANCH.Address1 = drRow.Item("Address1")
                                rSYSBRANCH.Address2 = drRow.Item("Address2")
                                rSYSBRANCH.Address3 = drRow.Item("Address3")
                                rSYSBRANCH.Address4 = drRow.Item("Address4")
                                rSYSBRANCH.PostalCode = drRow.Item("PostalCode")
                                rSYSBRANCH.Contact1 = drRow.Item("Contact1")
                                rSYSBRANCH.Contact2 = drRow.Item("Contact2")
                                rSYSBRANCH.StoreType = drRow.Item("StoreType")
                                rSYSBRANCH.Email = drRow.Item("Email")
                                rSYSBRANCH.Tel = drRow.Item("Tel")
                                rSYSBRANCH.Fax = drRow.Item("Fax")
                                rSYSBRANCH.Region = drRow.Item("Region")
                                rSYSBRANCH.Country = drRow.Item("Country")
                                rSYSBRANCH.State = drRow.Item("State")
                                rSYSBRANCH.Currency = drRow.Item("Currency")
                                rSYSBRANCH.StoreStatus = drRow.Item("StoreStatus")
                                rSYSBRANCH.OpPrevBook = drRow.Item("OpPrevBook")
                                rSYSBRANCH.OpTimeStart = drRow.Item("OpTimeStart")
                                rSYSBRANCH.OpTimeEnd = drRow.Item("OpTimeEnd")
                                rSYSBRANCH.OpDay1 = drRow.Item("OpDay1")
                                rSYSBRANCH.OpDay2 = drRow.Item("OpDay2")
                                rSYSBRANCH.OpDay3 = drRow.Item("OpDay3")
                                rSYSBRANCH.OpDay4 = drRow.Item("OpDay4")
                                rSYSBRANCH.OpDay5 = drRow.Item("OpDay5")
                                rSYSBRANCH.OpDay6 = drRow.Item("OpDay6")
                                rSYSBRANCH.OpDay7 = drRow.Item("OpDay7")
                                rSYSBRANCH.OpBookAlwDY = drRow.Item("OpBookAlwDY")
                                rSYSBRANCH.OpBookAlwHR = drRow.Item("OpBookAlwHR")
                                rSYSBRANCH.OpBookFirst = drRow.Item("OpBookFirst")
                                rSYSBRANCH.OpBookLast = drRow.Item("OpBookLast")
                                rSYSBRANCH.OpBookIntv = drRow.Item("OpBookIntv")
                                rSYSBRANCH.SalesItemType = drRow.Item("SalesItemType")
                                rSYSBRANCH.InSvcItemType = drRow.Item("InSvcItemType")
                                rSYSBRANCH.GenInSvcID = drRow.Item("GenInSvcID")
                                rSYSBRANCH.RcpHeader = drRow.Item("RcpHeader")
                                rSYSBRANCH.RcpFooter = drRow.Item("RcpFooter")
                                rSYSBRANCH.PriceLevel = drRow.Item("PriceLevel")
                                rSYSBRANCH.IsStockTake = drRow.Item("IsStockTake")
                                rSYSBRANCH.rowguid = drRow.Item("rowguid")
                                rSYSBRANCH.SyncCreate = drRow.Item("SyncCreate")
                                rSYSBRANCH.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rSYSBRANCH.IsHost = drRow.Item("IsHost")
                                rSYSBRANCH.LastSyncBy = drRow.Item("LastSyncBy")
                            Next
                            lstSYSBRANCH.Add(rSYSBRANCH)
                        Else
                            rSYSBRANCH = Nothing
                        End If
                        Return lstSYSBRANCH
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/Branch", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rSYSBRANCH = Nothing
                lstSYSBRANCH = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetBranchList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With SYSBRANCHInfo.MyInfo
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

#End Region
    End Class


    Namespace Container
#Region "Class Container"
        Public Class SYSBRANCH
            Public fBranchID As System.String = "BranchID"
            Public fRevGroup As System.String = "RevGroup"
            Public fBranchName As System.String = "BranchName"
            Public fBranchCode As System.String = "BranchCode"
            Public fAccNo As System.String = "AccNo"
            Public fAddress1 As System.String = "Address1"
            Public fAddress2 As System.String = "Address2"
            Public fAddress3 As System.String = "Address3"
            Public fAddress4 As System.String = "Address4"
            Public fPostalCode As System.String = "PostalCode"
            Public fContact1 As System.String = "Contact1"
            Public fContact2 As System.String = "Contact2"
            Public fStoreType As System.String = "StoreType"
            Public fEmail As System.String = "Email"
            Public fTel As System.String = "Tel"
            Public fFax As System.String = "Fax"
            Public fRegion As System.String = "Region"
            Public fCountry As System.String = "Country"
            Public fState As System.String = "State"
            Public fCurrency As System.String = "Currency"
            Public fStoreStatus As System.String = "StoreStatus"
            Public fOpPrevBook As System.String = "OpPrevBook"
            Public fOpTimeStart As System.String = "OpTimeStart"
            Public fOpTimeEnd As System.String = "OpTimeEnd"
            Public fOpDay1 As System.String = "OpDay1"
            Public fOpDay2 As System.String = "OpDay2"
            Public fOpDay3 As System.String = "OpDay3"
            Public fOpDay4 As System.String = "OpDay4"
            Public fOpDay5 As System.String = "OpDay5"
            Public fOpDay6 As System.String = "OpDay6"
            Public fOpDay7 As System.String = "OpDay7"
            Public fOpBookAlwDY As System.String = "OpBookAlwDY"
            Public fOpBookAlwHR As System.String = "OpBookAlwHR"
            Public fOpBookFirst As System.String = "OpBookFirst"
            Public fOpBookLast As System.String = "OpBookLast"
            Public fOpBookIntv As System.String = "OpBookIntv"
            Public fSalesItemType As System.String = "SalesItemType"
            Public fInSvcItemType As System.String = "InSvcItemType"
            Public fGenInSvcID As System.String = "GenInSvcID"
            Public fRcpHeader As System.String = "RcpHeader"
            Public fRcpFooter As System.String = "RcpFooter"
            Public fPriceLevel As System.String = "PriceLevel"
            Public fIsStockTake As System.String = "IsStockTake"
            Public fFlag As System.String = "Flag"
            Public frowguid As System.String = "rowguid"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fIsHost As System.String = "IsHost"
            Public fLastSyncBy As System.String = "LastSyncBy"

            Protected _BranchID As System.String
            Private _RevGroup As System.String
            Private _BranchName As System.String
            Private _BranchCode As System.String
            Private _AccNo As System.String
            Private _Address1 As System.String
            Private _Address2 As System.String
            Private _Address3 As System.String
            Private _Address4 As System.String
            Private _PostalCode As System.String
            Private _Contact1 As System.String
            Private _Contact2 As System.String
            Private _StoreType As System.String
            Private _Email As System.String
            Private _Tel As System.String
            Private _Fax As System.String
            Private _Region As System.String
            Private _Country As System.String
            Private _State As System.String
            Private _Currency As System.String
            Private _StoreStatus As System.Byte
            Private _OpPrevBook As System.Int32
            Private _OpTimeStart As System.String
            Private _OpTimeEnd As System.String
            Private _OpDay1 As System.Byte
            Private _OpDay2 As System.Byte
            Private _OpDay3 As System.Byte
            Private _OpDay4 As System.Byte
            Private _OpDay5 As System.Byte
            Private _OpDay6 As System.Byte
            Private _OpDay7 As System.Byte
            Private _OpBookAlwDY As System.Int32
            Private _OpBookAlwHR As System.Int32
            Private _OpBookFirst As System.String
            Private _OpBookLast As System.String
            Private _OpBookIntv As System.Int32
            Private _SalesItemType As System.String
            Private _InSvcItemType As System.String
            Private _GenInSvcID As System.Byte
            Private _RcpHeader As System.String
            Private _RcpFooter As System.String
            Private _PriceLevel As System.Byte
            Private _IsStockTake As System.Byte
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _IsHost As System.Byte
            Private _LastSyncBy As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property BranchID As System.String
                Get
                    Return _BranchID
                End Get
                Set(ByVal Value As System.String)
                    _BranchID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RevGroup As System.String
                Get
                    Return _RevGroup
                End Get
                Set(ByVal Value As System.String)
                    _RevGroup = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property BranchName As System.String
                Get
                    Return _BranchName
                End Get
                Set(ByVal Value As System.String)
                    _BranchName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property BranchCode As System.String
                Get
                    Return _BranchCode
                End Get
                Set(ByVal Value As System.String)
                    _BranchCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property AccNo As System.String
                Get
                    Return _AccNo
                End Get
                Set(ByVal Value As System.String)
                    _AccNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Address1 As System.String
                Get
                    Return _Address1
                End Get
                Set(ByVal Value As System.String)
                    _Address1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Address2 As System.String
                Get
                    Return _Address2
                End Get
                Set(ByVal Value As System.String)
                    _Address2 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Address3 As System.String
                Get
                    Return _Address3
                End Get
                Set(ByVal Value As System.String)
                    _Address3 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Address4 As System.String
                Get
                    Return _Address4
                End Get
                Set(ByVal Value As System.String)
                    _Address4 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PostalCode As System.String
                Get
                    Return _PostalCode
                End Get
                Set(ByVal Value As System.String)
                    _PostalCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Contact1 As System.String
                Get
                    Return _Contact1
                End Get
                Set(ByVal Value As System.String)
                    _Contact1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property Contact2 As System.String
                Get
                    Return _Contact2
                End Get
                Set(ByVal Value As System.String)
                    _Contact2 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property StoreType As System.String
                Get
                    Return _StoreType
                End Get
                Set(ByVal Value As System.String)
                    _StoreType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Email As System.String
                Get
                    Return _Email
                End Get
                Set(ByVal Value As System.String)
                    _Email = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Tel As System.String
                Get
                    Return _Tel
                End Get
                Set(ByVal Value As System.String)
                    _Tel = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Fax As System.String
                Get
                    Return _Fax
                End Get
                Set(ByVal Value As System.String)
                    _Fax = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Region As System.String
                Get
                    Return _Region
                End Get
                Set(ByVal Value As System.String)
                    _Region = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Country As System.String
                Get
                    Return _Country
                End Get
                Set(ByVal Value As System.String)
                    _Country = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property State As System.String
                Get
                    Return _State
                End Get
                Set(ByVal Value As System.String)
                    _State = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Currency As System.String
                Get
                    Return _Currency
                End Get
                Set(ByVal Value As System.String)
                    _Currency = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property StoreStatus As System.Byte
                Get
                    Return _StoreStatus
                End Get
                Set(ByVal Value As System.Byte)
                    _StoreStatus = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpPrevBook As System.Int32
                Get
                    Return _OpPrevBook
                End Get
                Set(ByVal Value As System.Int32)
                    _OpPrevBook = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpTimeStart As System.String
                Get
                    Return _OpTimeStart
                End Get
                Set(ByVal Value As System.String)
                    _OpTimeStart = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpTimeEnd As System.String
                Get
                    Return _OpTimeEnd
                End Get
                Set(ByVal Value As System.String)
                    _OpTimeEnd = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpDay1 As System.Byte
                Get
                    Return _OpDay1
                End Get
                Set(ByVal Value As System.Byte)
                    _OpDay1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpDay2 As System.Byte
                Get
                    Return _OpDay2
                End Get
                Set(ByVal Value As System.Byte)
                    _OpDay2 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpDay3 As System.Byte
                Get
                    Return _OpDay3
                End Get
                Set(ByVal Value As System.Byte)
                    _OpDay3 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpDay4 As System.Byte
                Get
                    Return _OpDay4
                End Get
                Set(ByVal Value As System.Byte)
                    _OpDay4 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpDay5 As System.Byte
                Get
                    Return _OpDay5
                End Get
                Set(ByVal Value As System.Byte)
                    _OpDay5 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpDay6 As System.Byte
                Get
                    Return _OpDay6
                End Get
                Set(ByVal Value As System.Byte)
                    _OpDay6 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpDay7 As System.Byte
                Get
                    Return _OpDay7
                End Get
                Set(ByVal Value As System.Byte)
                    _OpDay7 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpBookAlwDY As System.Int32
                Get
                    Return _OpBookAlwDY
                End Get
                Set(ByVal Value As System.Int32)
                    _OpBookAlwDY = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpBookAlwHR As System.Int32
                Get
                    Return _OpBookAlwHR
                End Get
                Set(ByVal Value As System.Int32)
                    _OpBookAlwHR = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpBookFirst As System.String
                Get
                    Return _OpBookFirst
                End Get
                Set(ByVal Value As System.String)
                    _OpBookFirst = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpBookLast As System.String
                Get
                    Return _OpBookLast
                End Get
                Set(ByVal Value As System.String)
                    _OpBookLast = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpBookIntv As System.Int32
                Get
                    Return _OpBookIntv
                End Get
                Set(ByVal Value As System.Int32)
                    _OpBookIntv = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SalesItemType As System.String
                Get
                    Return _SalesItemType
                End Get
                Set(ByVal Value As System.String)
                    _SalesItemType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property InSvcItemType As System.String
                Get
                    Return _InSvcItemType
                End Get
                Set(ByVal Value As System.String)
                    _InSvcItemType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property GenInSvcID As System.Byte
                Get
                    Return _GenInSvcID
                End Get
                Set(ByVal Value As System.Byte)
                    _GenInSvcID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RcpHeader As System.String
                Get
                    Return _RcpHeader
                End Get
                Set(ByVal Value As System.String)
                    _RcpHeader = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RcpFooter As System.String
                Get
                    Return _RcpFooter
                End Get
                Set(ByVal Value As System.String)
                    _RcpFooter = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PriceLevel As System.Byte
                Get
                    Return _PriceLevel
                End Get
                Set(ByVal Value As System.Byte)
                    _PriceLevel = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsStockTake As System.Byte
                Get
                    Return _IsStockTake
                End Get
                Set(ByVal Value As System.Byte)
                    _IsStockTake = Value
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
    Public Class SYSBRANCHInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "BranchID,RevGroup,BranchName,BranchCode,AccNo,Address1,Address2,Address3,Address4,PostalCode,Contact1,Contact2,StoreType,Email,Tel,Fax,Region,Country,State,Currency,StoreStatus,OpPrevBook,OpTimeStart,OpTimeEnd,OpDay1,OpDay2,OpDay3,OpDay4,OpDay5,OpDay6,OpDay7,OpBookAlwDY,OpBookAlwHR,OpBookFirst,OpBookLast,OpBookIntv,SalesItemType,InSvcItemType,GenInSvcID,RcpHeader,RcpFooter,PriceLevel,IsStockTake,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
                .CheckFields = "StoreStatus,OpDay1,OpDay2,OpDay3,OpDay4,OpDay5,OpDay6,OpDay7,GenInSvcID,PriceLevel,IsStockTake,Flag,IsHost"
                .TableName = "SYSBRANCH"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "BranchID,RevGroup,BranchName,BranchCode,AccNo,Address1,Address2,Address3,Address4,PostalCode,Contact1,Contact2,StoreType,Email,Tel,Fax,Region,Country,State,Currency,StoreStatus,OpPrevBook,OpTimeStart,OpTimeEnd,OpDay1,OpDay2,OpDay3,OpDay4,OpDay5,OpDay6,OpDay7,OpBookAlwDY,OpBookAlwHR,OpBookFirst,OpBookLast,OpBookIntv,SalesItemType,InSvcItemType,GenInSvcID,RcpHeader,RcpFooter,PriceLevel,IsStockTake,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
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
    Public Class BranchScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BranchID"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "RevGroup"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "BranchName"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BranchCode"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "AccNo"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address1"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address2"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address3"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address4"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PostalCode"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Contact1"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Contact2"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "StoreType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Email"
                .Length = 80
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Tel"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Fax"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Region"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Country"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "State"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Currency"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "StoreStatus"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpPrevBook"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "OpTimeStart"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "OpTimeEnd"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpDay1"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpDay2"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpDay3"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpDay4"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(27, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpDay5"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(28, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpDay6"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(29, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpDay7"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(30, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpBookAlwDY"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(31, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpBookAlwHR"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(32, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "OpBookFirst"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(33, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "OpBookLast"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(34, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpBookIntv"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(35, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "SalesItemType"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(36, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "InSvcItemType"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(37, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "GenInSvcID"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(38, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "RcpHeader"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(39, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "RcpFooter"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(40, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "PriceLevel"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(41, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsStockTake"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(42, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(43, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(44, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(45, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(46, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(47, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(48, this)

        End Sub

        Public ReadOnly Property BranchID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property RevGroup As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property BranchName As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property BranchCode As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property AccNo As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property Address1 As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property Address2 As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property Address3 As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property Address4 As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property PostalCode As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property Contact1 As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property Contact2 As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property StoreType As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property Email As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property Tel As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property Fax As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property Region As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property Country As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property State As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property Currency As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property StoreStatus As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property OpPrevBook As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property OpTimeStart As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property OpTimeEnd As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property OpDay1 As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property OpDay2 As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property OpDay3 As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property OpDay4 As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property
        Public ReadOnly Property OpDay5 As StrucElement
            Get
                Return MyBase.GetItem(28)
            End Get
        End Property
        Public ReadOnly Property OpDay6 As StrucElement
            Get
                Return MyBase.GetItem(29)
            End Get
        End Property
        Public ReadOnly Property OpDay7 As StrucElement
            Get
                Return MyBase.GetItem(30)
            End Get
        End Property
        Public ReadOnly Property OpBookAlwDY As StrucElement
            Get
                Return MyBase.GetItem(31)
            End Get
        End Property
        Public ReadOnly Property OpBookAlwHR As StrucElement
            Get
                Return MyBase.GetItem(32)
            End Get
        End Property
        Public ReadOnly Property OpBookFirst As StrucElement
            Get
                Return MyBase.GetItem(33)
            End Get
        End Property
        Public ReadOnly Property OpBookLast As StrucElement
            Get
                Return MyBase.GetItem(34)
            End Get
        End Property
        Public ReadOnly Property OpBookIntv As StrucElement
            Get
                Return MyBase.GetItem(35)
            End Get
        End Property
        Public ReadOnly Property SalesItemType As StrucElement
            Get
                Return MyBase.GetItem(36)
            End Get
        End Property
        Public ReadOnly Property InSvcItemType As StrucElement
            Get
                Return MyBase.GetItem(37)
            End Get
        End Property
        Public ReadOnly Property GenInSvcID As StrucElement
            Get
                Return MyBase.GetItem(38)
            End Get
        End Property
        Public ReadOnly Property RcpHeader As StrucElement
            Get
                Return MyBase.GetItem(39)
            End Get
        End Property
        Public ReadOnly Property RcpFooter As StrucElement
            Get
                Return MyBase.GetItem(40)
            End Get
        End Property
        Public ReadOnly Property PriceLevel As StrucElement
            Get
                Return MyBase.GetItem(41)
            End Get
        End Property
        Public ReadOnly Property IsStockTake As StrucElement
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

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace

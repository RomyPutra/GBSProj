Imports SEAL.Data
imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace Actions
#Region "ITMTRANSEXP Class"
    Public NotInheritable Class ITMTRANSEXP
        Inherits Core.CoreControl
        Private ItmtransexpInfo As ItmtransexpInfo = New ItmtransexpInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal ItmtransexpCont As Container.Itmtransexp, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False, Optional ByVal confirm As Boolean = False, Optional ByVal days As Integer = 0) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ItmtransexpCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItmtransexpInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & ItmtransexpCont.DocCode & "'")
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
                            Throw New ApplicationException("210011: Record already exist")
                            'Item found & active
                            'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIconInformation,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
                            'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "Itmtransexp"
                                .AddField("BatchCode", ItmtransexpCont.BatchCode, SQLControl.EnumDataType.dtString)
                                .AddField("CompanyID", ItmtransexpCont.CompanyID, SQLControl.EnumDataType.dtString)
                                .AddField("LocID", ItmtransexpCont.LocID, SQLControl.EnumDataType.dtString)
                                .AddField("CompanyName", ItmtransexpCont.CompanyName, SQLControl.EnumDataType.dtStringN)
                                .AddField("DOEFileNo", ItmtransexpCont.DOEFileNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("TransDate", ItmtransexpCont.TransDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ItemCode", ItmtransexpCont.ItemCode, SQLControl.EnumDataType.dtString)
                                .AddField("ItemType", ItmtransexpCont.ItemType, SQLControl.EnumDataType.dtString)
                                .AddField("QOH", ItmtransexpCont.QOH, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Remark", ItmtransexpCont.Remark, SQLControl.EnumDataType.dtStringN)
                                .AddField("Reason", ItmtransexpCont.Reason, SQLControl.EnumDataType.dtStringN)
                                .AddField("PostDate", ItmtransexpCont.PostDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Posted", ItmtransexpCont.Posted, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Status", ItmtransexpCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ExpiryDate", ItmtransexpCont.ExpiryDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ExtendDate", ItmtransexpCont.ExtendDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ExtendCode", ItmtransexpCont.ExtendCode, SQLControl.EnumDataType.dtString)
                                .AddField("AuthID", ItmtransexpCont.AuthID, SQLControl.EnumDataType.dtStringN)
                                .AddField("AuthPOS", ItmtransexpCont.AuthPOS, SQLControl.EnumDataType.dtStringN)
                                .AddField("UploadPath", ItmtransexpCont.UploadPath, SQLControl.EnumDataType.dtStringN)
                                .AddField("ApproveDate", ItmtransexpCont.ApproveDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ApproveBy", ItmtransexpCont.ApproveBy, SQLControl.EnumDataType.dtString)
                                .AddField("CreateDate", ItmtransexpCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", ItmtransexpCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", ItmtransexpCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", ItmtransexpCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", ItmtransexpCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", ItmtransexpCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("SyncCreate", ItmtransexpCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", ItmtransexpCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastSyncBy", ItmtransexpCont.LastSyncBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & ItmtransexpCont.DocCode & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("DocCode", ItmtransexpCont.DocCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & ItmtransexpCont.DocCode & "'")
                                End Select
                                BatchList.Add(strSQL)
                            End With

                            If confirm Then
                                strSQL = "update itmtransdtl set expirydate = DATEADD (day , " & days & " , expirydate)" &
                                    " where itemcode = '" & ItmtransexpCont.ItemCode & "' and locid = '" & ItmtransexpCont.LocID & "' and itemname in (select itemname from itemloc where itemcode = '" & ItmtransexpCont.ItemCode & "' and locid = '" & ItmtransexpCont.LocID & "' and typecode = '" & ItmtransexpCont.ItemType & "') and expirydate is not null"
                                BatchList.Add(strSQL)

                                strSQL = "update itemloc set expirydate = DATEADD (day , " & days & " , expirydate) where itemcode = '" & ItmtransexpCont.ItemCode & "' and locid = '" & ItmtransexpCont.LocID & "' and typecode = '" & ItmtransexpCont.ItemType & "' and expirydate is not null"
                                BatchList.Add(strSQL)
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
                ItmtransexpCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal ItmtransexpCont As Container.Itmtransexp, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False, Optional ByVal confirm As Boolean = False) As Boolean
            Return Save(ItmtransexpCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit, confirm)
        End Function

        'AMEND
        Public Function Update(ByVal ItmtransexpCont As Container.Itmtransexp, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False, Optional ByVal confirm As Boolean = False, Optional ByVal days As Integer = 0) As Boolean
            Return Save(ItmtransexpCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit, confirm, days)
        End Function

        Public Function Delete(ByVal ItmtransexpCont As Container.Itmtransexp, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If ItmtransexpCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItmtransexpInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & ItmtransexpCont.DocCode & "'")
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
                                strSQL = BuildUpdate(ItmtransexpInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = '" & ItmtransexpCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtransexpCont.UpdateBy) & "' WHERE" & _
                                "DocCode = '" & ItmtransexpCont.DocCode & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(ItmtransexpInfo.MyInfo.TableName, "DocCode = '" & ItmtransexpCont.DocCode & "'")
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
                ItmtransexpCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetITMTRANSEXP(ByVal DocCode As System.String) As Container.Itmtransexp
            Dim rItmtransexp As Container.Itmtransexp = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With ItmtransexpInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "DocCode = '" & DocCode & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rItmtransexp = New Container.Itmtransexp
                                rItmtransexp.DocCode = drRow.Item("DocCode")
                                rItmtransexp.BatchCode = drRow.Item("BatchCode")
                                rItmtransexp.CompanyID = drRow.Item("CompanyID")
                                rItmtransexp.LocID = drRow.Item("LocID")
                                rItmtransexp.CompanyName = drRow.Item("CompanyName")
                                rItmtransexp.DOEFileNo = drRow.Item("DOEFileNo")
                                rItmtransexp.TransDate = drRow.Item("TransDate")
                                rItmtransexp.ItemCode = drRow.Item("ItemCode")
                                rItmtransexp.ItemType = drRow.Item("ItemType")
                                rItmtransexp.QOH = drRow.Item("QOH")
                                rItmtransexp.Remark = drRow.Item("Remark")
                                rItmtransexp.Reason = drRow.Item("Reason")
                                If Not IsDBNull(drRow.Item("PostDate")) Then
                                    rItmtransexp.PostDate = drRow.Item("PostDate")
                                End If
                                rItmtransexp.Posted = drRow.Item("Posted")
                                rItmtransexp.Status = drRow.Item("Status")
                                If Not IsDBNull(drRow.Item("ExpiryDate")) Then
                                    rItmtransexp.ExpiryDate = drRow.Item("ExpiryDate")
                                End If
                                If Not IsDBNull(drRow.Item("ExtendDate")) Then
                                    rItmtransexp.ExtendDate = drRow.Item("ExtendDate")
                                End If
                                rItmtransexp.ExtendCode = drRow.Item("ExtendCode")
                                rItmtransexp.AuthID = drRow.Item("AuthID")
                                If Not IsDBNull(drRow.Item("AuthPOS")) Then
                                    rItmtransexp.AuthPOS = drRow.Item("AuthPOS")
                                End If
                                rItmtransexp.UploadPath = drRow.Item("UploadPath")
                                If Not IsDBNull(drRow.Item("ApproveDate")) Then
                                    rItmtransexp.ApproveDate = drRow.Item("ApproveDate")
                                End If
                                rItmtransexp.ApproveBy = drRow.Item("ApproveBy")
                                rItmtransexp.CreateDate = drRow.Item("CreateDate")
                                rItmtransexp.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rItmtransexp.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rItmtransexp.UpdateBy = drRow.Item("UpdateBy")
                                rItmtransexp.Active = drRow.Item("Active")
                                rItmtransexp.Inuse = drRow.Item("Inuse")
                                rItmtransexp.rowguid = drRow.Item("rowguid")
                                rItmtransexp.SyncCreate = drRow.Item("SyncCreate")
                                rItmtransexp.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rItmtransexp.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rItmtransexp = Nothing
                            End If
                        Else
                            rItmtransexp = Nothing
                        End If
                    End With
                End If
                Return rItmtransexp
            Catch ex As Exception
                Throw ex
            Finally
                rItmtransexp = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetITMTRANSEXP(ByVal DocCode As System.String, DecendingOrder As Boolean) As List(Of Container.Itmtransexp)
            Dim rItmtransexp As Container.Itmtransexp = Nothing
            Dim lstItmtransexp As List(Of Container.Itmtransexp) = New List(Of Container.Itmtransexp)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With ItmtransexpInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by DocCode DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "DocCode = '" & DocCode & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rItmtransexp = New Container.Itmtransexp
                                rItmtransexp.DocCode = drRow.Item("DocCode")
                                rItmtransexp.BatchCode = drRow.Item("BatchCode")
                                rItmtransexp.CompanyID = drRow.Item("CompanyID")
                                rItmtransexp.LocID = drRow.Item("LocID")
                                rItmtransexp.CompanyName = drRow.Item("CompanyName")
                                rItmtransexp.DOEFileNo = drRow.Item("DOEFileNo")
                                rItmtransexp.TransDate = drRow.Item("TransDate")
                                rItmtransexp.ItemCode = drRow.Item("ItemCode")
                                rItmtransexp.ItemType = drRow.Item("ItemType")
                                rItmtransexp.QOH = drRow.Item("QOH")
                                rItmtransexp.Remark = drRow.Item("Remark")
                                rItmtransexp.Reason = drRow.Item("Reason")
                                rItmtransexp.Posted = drRow.Item("Posted")
                                rItmtransexp.Status = drRow.Item("Status")
                                rItmtransexp.ExtendCode = drRow.Item("ExtendCode")
                                rItmtransexp.AuthID = drRow.Item("AuthID")
                                rItmtransexp.UploadPath = drRow.Item("UploadPath")
                                rItmtransexp.ApproveBy = drRow.Item("ApproveBy")
                                rItmtransexp.CreateDate = drRow.Item("CreateDate")
                                rItmtransexp.CreateBy = drRow.Item("CreateBy")
                                rItmtransexp.UpdateBy = drRow.Item("UpdateBy")
                                rItmtransexp.Active = drRow.Item("Active")
                                rItmtransexp.Inuse = drRow.Item("Inuse")
                                rItmtransexp.rowguid = drRow.Item("rowguid")
                                rItmtransexp.SyncCreate = drRow.Item("SyncCreate")
                                rItmtransexp.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rItmtransexp.LastSyncBy = drRow.Item("LastSyncBy")
                                lstItmtransexp.Add(rItmtransexp)
                            Next

                        Else
                            rItmtransexp = Nothing
                        End If
                        Return lstItmtransexp
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rItmtransexp = Nothing
                lstItmtransexp = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetITMTRANSEXPList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ItmtransexpInfo.MyInfo
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

        Public Overloads Function GetITMTRANSEXPListByStateGroup(Optional ByVal State As String = Nothing, Optional ByVal DocCode As String = Nothing, Optional ByVal LocID As String = Nothing, Optional ByVal posted As String = Nothing) As Data.DataTable

            If StartConnection() = True Then
                StartSQLControl()
                With ItmtransexpInfo.MyInfo
                    strSQL = "SELECT  I.CompanyName,ItemCode, I.ItemType, C.CodeDesc AS TypeDesc, I.LocID, SUM(I.QOH) AS OverdueQty " & vbCrLf & _
                            "FROM ITMTRANSEXP I INNER JOIN " & vbCrLf & _
                            "BIZLOCATE B ON I.LocID=B.BizLocID INNER JOIN " & vbCrLf & _
                            "CodeMaster C ON C.Code=I.ItemType AND CodeType='WTY' " & vbCrLf & _
                            "WHERE I.Active=1 AND I.Flag=1 AND B.State='05' " & vbCrLf & _
                            "GROUP BY I.ItemCode, I.ItemType, C.CodeDesc,I.CompanyName, I.LocID "
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()

        End Function

        Public Overloads Function GetITMTRANSEXPListByState(Optional ByVal State As String = Nothing, Optional ByVal DocCode As String = Nothing, Optional ByVal LocID As String = Nothing, Optional ByVal posted As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With ItmtransexpInfo.MyInfo
                    Dim strfilter As String = ""
                    If State IsNot Nothing AndAlso State <> "" Then
                        strfilter &= "AND B.State = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, State) & "'"
                    End If
                    If DocCode IsNot Nothing AndAlso DocCode <> "" Then
                        strfilter &= "AND I.DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, DocCode) & "'"
                    End If
                    If LocID IsNot Nothing AndAlso LocID <> "" Then
                        strfilter &= "AND I.LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, LocID) & "'"
                    End If
                    If posted IsNot Nothing AndAlso posted <> "" Then
                        strfilter &= "AND I.Posted = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, posted) & "'"
                    End If
                    strSQL = "select I.*, codedesc as TypeDesc, Convert(varchar(10),CONVERT(date,I.expirydate,106),103) ExpiryDateWG, ITM.OverdueQty AS SUMOverdueQty  from ITMTRANSEXP I INNER JOIN BIZLOCATE B on I.LocID = B.BizlocID Inner Join Codemaster c on c.Code=I.ItemType and codetype = 'WTY' INNER JOIN ( select SUM(l.ExpiredQty) AS OverdueQty, l.ItemCode,i.typecode, l.LocID from itmtransdtl l inner join itemloc i on l.locid = i.locid and l.itemcode = i.itemcode and l.itemname=i.itemname Group by l.itemcode, i.typecode,l.locid ) ITM on I.ItemType=ITM.TypeCode AND I.Itemcode=ITM.itemcode AND I.LocID=ITM.LocID  where I.active= 1 and I.flag = 1 " & strfilter
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetITMTRANSEXPShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With ItmtransexpInfo.MyInfo
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
#Region "Itmtransexp Container"
        Public Class Itmtransexp_FieldName
            Public DocCode As System.String = "DocCode"
            Public BatchCode As System.String = "BatchCode"
            Public CompanyID As System.String = "CompanyID"
            Public LocID As System.String = "LocID"
            Public CompanyName As System.String = "CompanyName"
            Public DOEFileNo As System.String = "DOEFileNo"
            Public TransDate As System.String = "TransDate"
            Public ItemCode As System.String = "ItemCode"
            Public ItemType As System.String = "ItemType"
            Public QOH As System.String = "QOH"
            Public Remark As System.String = "Remark"
            Public Reason As System.String = "Reason"
            Public PostDate As System.String = "PostDate"
            Public Posted As System.String = "Posted"
            Public Status As System.String = "Status"
            Public ExpiryDate As System.String = "ExpiryDate"
            Public ExtendDate As System.String = "ExtendDate"
            Public ExtendCode As System.String = "ExtendCode"
            Public AuthID As System.String = "AuthID"
            Public AuthPOS As System.String = "AuthPOS"
            Public UploadPath As System.String = "UploadPath"
            Public ApproveDate As System.String = "ApproveDate"
            Public ApproveBy As System.String = "ApproveBy"
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
            Public LastSyncBy As System.String = "LastSyncBy"
        End Class

        Public Class Itmtransexp
            Protected _DocCode As System.String
            Private _BatchCode As System.String
            Private _CompanyID As System.String
            Private _LocID As System.String
            Private _CompanyName As System.String
            Private _DOEFileNo As System.String
            Private _TransDate As System.DateTime
            Private _ItemCode As System.String
            Private _ItemType As System.String
            Private _QOH As System.Decimal
            Private _Remark As System.String
            Private _Reason As System.String
            Private _PostDate As System.DateTime
            Private _Posted As System.Byte
            Private _Status As System.Byte
            Private _ExpiryDate As System.DateTime
            Private _ExtendDate As System.DateTime
            Private _ExtendCode As System.String
            Private _AuthID As System.String
            Private _AuthPOS As System.String
            Private _UploadPath As System.String
            Private _ApproveDate As System.DateTime
            Private _ApproveBy As System.String
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _Active As System.Byte
            Private _Inuse As System.Byte
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _LastSyncBy As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property DocCode As System.String
                Get
                    Return _DocCode
                End Get
                Set(ByVal Value As System.String)
                    _DocCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property BatchCode As System.String
                Get
                    Return _BatchCode
                End Get
                Set(ByVal Value As System.String)
                    _BatchCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CompanyID As System.String
                Get
                    Return _CompanyID
                End Get
                Set(ByVal Value As System.String)
                    _CompanyID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
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
            Public Property CompanyName As System.String
                Get
                    Return _CompanyName
                End Get
                Set(ByVal Value As System.String)
                    _CompanyName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property DOEFileNo As System.String
                Get
                    Return _DOEFileNo
                End Get
                Set(ByVal Value As System.String)
                    _DOEFileNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransDate As System.DateTime
                Get
                    Return _TransDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _TransDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
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
            Public Property QOH As System.Decimal
                Get
                    Return _QOH
                End Get
                Set(ByVal Value As System.Decimal)
                    _QOH = Value
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
            Public Property Reason As System.String
                Get
                    Return _Reason
                End Get
                Set(ByVal Value As System.String)
                    _Reason = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property PostDate As System.DateTime
                Get
                    Return _PostDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _PostDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Posted As System.Byte
                Get
                    Return _Posted
                End Get
                Set(ByVal Value As System.Byte)
                    _Posted = Value
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
            Public Property ExtendDate As System.DateTime
                Get
                    Return _ExtendDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _ExtendDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ExtendCode As System.String
                Get
                    Return _ExtendCode
                End Get
                Set(ByVal Value As System.String)
                    _ExtendCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property AuthID As System.String
                Get
                    Return _AuthID
                End Get
                Set(ByVal Value As System.String)
                    _AuthID = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property AuthPOS As System.String
                Get
                    Return _AuthPOS
                End Get
                Set(ByVal Value As System.String)
                    _AuthPOS = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property UploadPath As System.String
                Get
                    Return _UploadPath
                End Get
                Set(ByVal Value As System.String)
                    _UploadPath = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property ApproveDate As System.DateTime
                Get
                    Return _ApproveDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _ApproveDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ApproveBy As System.String
                Get
                    Return _ApproveBy
                End Get
                Set(ByVal Value As System.String)
                    _ApproveBy = Value
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
#End Region

#Region "Class Info"
#Region "Itmtransexp Info"
    Public Class ItmtransexpInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "DocCode,BatchCode,CompanyID,LocID,CompanyName,DOEFileNo,TransDate,ItemCode,ItemType,QOH,Remark,Reason,PostDate,Posted,Status,ExpiryDate,ExtendDate,ExtendCode,AuthID,AuthPOS,UploadPath,ApproveDate,ApproveBy,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,LastSyncBy"
                .CheckFields = "Posted,Status,Active,Inuse,Flag"
                .TableName = "Itmtransexp"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "DocCode,BatchCode,CompanyID,LocID,CompanyName,DOEFileNo,TransDate,ItemCode,ItemType,QOH,Remark,Reason,PostDate,Posted,Status,ExpiryDate,ExtendDate,ExtendCode,AuthID,AuthPOS,UploadPath,ApproveDate,ApproveBy,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,LastSyncBy"
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
#Region "ITMTRANSEXP Scheme"
    Public Class ITMTRANSEXPScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "DocCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BatchCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CompanyID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LocID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "CompanyName"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "DOEFileNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "TransDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ItemCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ItemType"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "QOH"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Reason"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "PostDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Posted"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "ExpiryDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "ExtendDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ExtendCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "AuthID"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "AuthPOS"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "UploadPath"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "ApproveDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ApproveBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(27, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(28, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(29, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(30, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(31, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(32, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(33, this)

        End Sub

        Public ReadOnly Property DocCode As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property BatchCode As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property CompanyID As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property LocID As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property CompanyName As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property DOEFileNo As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property TransDate As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property ItemCode As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property ItemType As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property QOH As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property Remark As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property Reason As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property PostDate As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property Posted As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property ExpiryDate As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property ExtendDate As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property ExtendCode As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property AuthID As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property AuthPOS As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property UploadPath As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property ApproveDate As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property ApproveBy As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(28)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(29)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(30)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(31)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(32)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(33)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace
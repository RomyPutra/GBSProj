Imports SEAL.Data
imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace TWG
#Region "TWG_RESDTL Class"
    Public NotInheritable Class TWG_RESDTL
        Inherits Core.CoreControl
        Private Twg_resdtlInfo As Twg_resdtlInfo = New Twg_resdtlInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal Twg_resdtlCont As Container.Twg_resdtl, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If Twg_resdtlCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Twg_resdtlInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & Twg_resdtlCont.DocCode & "' AND WasteCode = '" & Twg_resdtlCont.WasteCode & "' AND WasteName = '" & Twg_resdtlCont.WasteName & "' AND WasteType = '" & Twg_resdtlCont.WasteType & "' AND RecType = '" & Twg_resdtlCont.RecType & "'")
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
                                .TableName = "Twg_resdtl"
                                .AddField("SeqNo", Twg_resdtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Qty", Twg_resdtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ExpectedQty", Twg_resdtlCont.ExpectedQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ResidueQty", Twg_resdtlCont.ResidueQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("HandlingQty1", Twg_resdtlCont.HandlingQty1, SQLControl.EnumDataType.dtNumeric)
                                .AddField("HandlingQty2", Twg_resdtlCont.HandlingQty2, SQLControl.EnumDataType.dtNumeric)
                                .AddField("HandlingQty3", Twg_resdtlCont.HandlingQty3, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Remark1", Twg_resdtlCont.Remark1, SQLControl.EnumDataType.dtStringN)
                                .AddField("Remark2", Twg_resdtlCont.Remark2, SQLControl.EnumDataType.dtStringN)
                                .AddField("Remark3", Twg_resdtlCont.Remark3, SQLControl.EnumDataType.dtStringN)
                                .AddField("CreateDate", Twg_resdtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", Twg_resdtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", Twg_resdtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", Twg_resdtlCont.UpdateBy, SQLControl.EnumDataType.dtDateTime)
                                .AddField("rowguid", Twg_resdtlCont.rowguid, SQLControl.EnumDataType.dtString)
                                .AddField("SyncCreate", Twg_resdtlCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", Twg_resdtlCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastSyncBy", Twg_resdtlCont.LastSyncBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & Twg_resdtlCont.DocCode & "' AND WasteCode = '" & Twg_resdtlCont.WasteCode & "' AND WasteName = '" & Twg_resdtlCont.WasteName & "' AND WasteType = '" & Twg_resdtlCont.WasteType & "' AND RecType = '" & Twg_resdtlCont.RecType & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("DocCode", Twg_resdtlCont.DocCode, SQLControl.EnumDataType.dtString)
                                                .AddField("WasteCode", Twg_resdtlCont.WasteCode, SQLControl.EnumDataType.dtString)
                                                .AddField("WasteName", Twg_resdtlCont.WasteName, SQLControl.EnumDataType.dtStringN)
                                                .AddField("WasteType", Twg_resdtlCont.WasteType, SQLControl.EnumDataType.dtString)
                                                .AddField("RecType", Twg_resdtlCont.RecType, SQLControl.EnumDataType.dtNumeric)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & Twg_resdtlCont.DocCode & "' AND WasteCode = '" & Twg_resdtlCont.WasteCode & "' AND WasteName = '" & Twg_resdtlCont.WasteName & "' AND WasteType = '" & Twg_resdtlCont.WasteType & "' AND RecType = '" & Twg_resdtlCont.RecType & "'")
                                End Select
                            End With
                            Try
                                If BatchExecute Then
                                    BatchList.Add(strSQL)
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
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Return False
            Finally
                Twg_resdtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal Twg_resdtlCont As Container.Twg_resdtl, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Twg_resdtlCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal Twg_resdtlCont As Container.Twg_resdtl, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Twg_resdtlCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal Twg_resdtlCont As Container.Twg_resdtl, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Twg_resdtlCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Twg_resdtlInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & Twg_resdtlCont.DocCode & "' AND WasteCode = '" & Twg_resdtlCont.WasteCode & "' AND WasteName = '" & Twg_resdtlCont.WasteName & "' AND WasteType = '" & Twg_resdtlCont.WasteType & "' AND RecType = '" & Twg_resdtlCont.RecType & "'")
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
                                strSQL = BuildUpdate(Twg_resdtlInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = '" & Twg_resdtlCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_resdtlCont.UpdateBy) & "' WHERE" & _
                                "DocCode = '" & Twg_resdtlCont.DocCode & "' AND WasteCode = '" & Twg_resdtlCont.WasteCode & "' AND WasteName = '" & Twg_resdtlCont.WasteName & "' AND WasteType = '" & Twg_resdtlCont.WasteType & "' AND RecType = '" & Twg_resdtlCont.RecType & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Twg_resdtlInfo.MyInfo.TableName, "DocCode = '" & Twg_resdtlCont.DocCode & "' AND WasteCode = '" & Twg_resdtlCont.WasteCode & "' AND WasteName = '" & Twg_resdtlCont.WasteName & "' AND WasteType = '" & Twg_resdtlCont.WasteType & "' AND RecType = '" & Twg_resdtlCont.RecType & "'")
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
                Twg_resdtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetTWG_RESDTL(ByVal DocCode As System.String, ByVal WasteCode As System.String, ByVal WasteName As System.String, ByVal WasteType As System.String, ByVal RecType As System.Byte) As Container.Twg_resdtl
            Dim rTwg_resdtl As Container.Twg_resdtl = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With Twg_resdtlInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "DocCode = '" & DocCode & "' AND WasteCode = '" & WasteCode & "' AND WasteName = '" & WasteName & "' AND WasteType = '" & WasteType & "' AND RecType = '" & RecType & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rTwg_resdtl = New Container.Twg_resdtl
                                rTwg_resdtl.DocCode = drRow.Item("DocCode")
                                rTwg_resdtl.WasteCode = drRow.Item("WasteCode")
                                rTwg_resdtl.WasteName = drRow.Item("WasteName")
                                rTwg_resdtl.WasteType = drRow.Item("WasteType")
                                rTwg_resdtl.RecType = drRow.Item("RecType")
                                rTwg_resdtl.Qty = drRow.Item("Qty")
                                rTwg_resdtl.ExpectedQty = drRow.Item("ExpectedQty")
                                rTwg_resdtl.ResidueQty = drRow.Item("ResidueQty")
                                rTwg_resdtl.HandlingQty1 = drRow.Item("HandlingQty1")
                                rTwg_resdtl.HandlingQty2 = drRow.Item("HandlingQty2")
                                rTwg_resdtl.HandlingQty3 = drRow.Item("HandlingQty3")
                                rTwg_resdtl.Remark1 = drRow.Item("Remark1")
                                rTwg_resdtl.Remark2 = drRow.Item("Remark2")
                                rTwg_resdtl.Remark3 = drRow.Item("Remark3")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rTwg_resdtl.CreateDate = drRow.Item("CreateDate")
                                End If
                                rTwg_resdtl.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rTwg_resdtl.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rTwg_resdtl.UpdateBy = drRow.Item("UpdateBy")
                                rTwg_resdtl.rowguid = drRow.Item("rowguid")
                                rTwg_resdtl.SyncCreate = drRow.Item("SyncCreate")
                                rTwg_resdtl.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rTwg_resdtl.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rTwg_resdtl = Nothing
                            End If
                        Else
                            rTwg_resdtl = Nothing
                        End If
                    End With
                End If
                Return rTwg_resdtl
            Catch ex As Exception
                Throw ex
            Finally
                rTwg_resdtl = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTWG_RESDTL(ByVal DocCode As System.String, ByVal WasteCode As System.String, ByVal WasteName As System.String, ByVal WasteType As System.String, ByVal RecType As System.Byte, DecendingOrder As Boolean) As List(Of Container.Twg_resdtl)
            Dim rTwg_resdtl As Container.Twg_resdtl = Nothing
            Dim lstTwg_resdtl As List(Of Container.Twg_resdtl) = New List(Of Container.Twg_resdtl)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With Twg_resdtlInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by DocCode, WasteCode, WasteName, WasteType, RecType DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "DocCode = '" & DocCode & "' AND WasteCode = '" & WasteCode & "' AND WasteName = '" & WasteName & "' AND WasteType = '" & WasteType & "' AND RecType = '" & RecType & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rTwg_resdtl = New Container.Twg_resdtl
                                rTwg_resdtl.DocCode = drRow.Item("DocCode")
                                rTwg_resdtl.WasteCode = drRow.Item("WasteCode")
                                rTwg_resdtl.WasteName = drRow.Item("WasteName")
                                rTwg_resdtl.WasteType = drRow.Item("WasteType")
                                rTwg_resdtl.RecType = drRow.Item("RecType")
                                rTwg_resdtl.Qty = drRow.Item("Qty")
                                rTwg_resdtl.ExpectedQty = drRow.Item("ExpectedQty")
                                rTwg_resdtl.ResidueQty = drRow.Item("ResidueQty")
                                rTwg_resdtl.HandlingQty1 = drRow.Item("HandlingQty1")
                                rTwg_resdtl.HandlingQty2 = drRow.Item("HandlingQty2")
                                rTwg_resdtl.HandlingQty3 = drRow.Item("HandlingQty3")
                                rTwg_resdtl.Remark1 = drRow.Item("Remark1")
                                rTwg_resdtl.Remark2 = drRow.Item("Remark2")
                                rTwg_resdtl.Remark3 = drRow.Item("Remark3")
                                rTwg_resdtl.CreateBy = drRow.Item("CreateBy")
                                rTwg_resdtl.UpdateBy = drRow.Item("UpdateBy")
                                rTwg_resdtl.rowguid = drRow.Item("rowguid")
                                rTwg_resdtl.SyncCreate = drRow.Item("SyncCreate")
                                rTwg_resdtl.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rTwg_resdtl.LastSyncBy = drRow.Item("LastSyncBy")
                                lstTwg_resdtl.Add(rTwg_resdtl)
                            Next

                        Else
                            rTwg_resdtl = Nothing
                        End If
                        Return lstTwg_resdtl
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rTwg_resdtl = Nothing
                lstTwg_resdtl = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetItem_ItemLoc(ByVal WasteCode As System.String, ByVal WasteName As System.String, Optional ByVal LocID As String = "")
            If StartConnection() = True Then
                With Twg_resdtlInfo.MyInfo
                    StartSQLControl()
                    If WasteCode = "" OrElse WasteCode IsNot Nothing Then
                        If WasteName = "" OrElse WasteName IsNot Nothing Then
                            strSQL = "select * from itemloc WHERE ItemCOde = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "' AND ItemName = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteName) & "'"
                            If LocID IsNot Nothing AndAlso LocID <> "" Then
                                strSQL &= " AND LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "'"
                            End If
                        End If
                    End If
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetTWG_RESDTLByDocCode(ByVal DocCode As System.String)
            If StartConnection() = True Then
                With Twg_resdtlInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT * FROM TWG_RESDTL WHERE DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, DocCode) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetTWG_RESDTLList(ByVal DocCode As System.String, ByVal WasteCode As System.String, ByVal WasteType As System.String, ByVal WasteName As String, ByVal ReceiverID As String, ByVal ReceiverLocID As System.String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_resdtlInfo.MyInfo
                    StartSQLControl()

                    If DocCode = "0" OrElse DocCode = "" OrElse DocCode = Nothing Then

                        strSQL = "SELECT ROW_NUMBER() OVER (order by d.SeqNo asc) AS No, '0' AS DocCode, d.WasteCode, d.WasteName, cd.CodeDesc AS WasteTypeDesc, '' AS HandlingType, 0.00 As QtyMT, '' as Remark, '' as RemainingMT, d.WasteType, 0 AS LastQty, 0 AS Qty,  getdate() AS SetQtyDate, CASE WHEN d.WasteCode = 'SW501' THEN 2 ELSE 0 END AS RecType," & _
                            " CASE WHEN d.WasteCode='SW501' THEN 'false' ELSE 'true' END AS 'DeleteStatus',  0.00 AS HandlingQty, 0.00 AS ResQty, 0 AS type, 1 AS Flag FROM TWG_TEMPLATE h WITH (NOLOCK) INNER JOIN TWG_TEMPLATEDTL d WITH (NOLOCK) ON h.TemplateID=d.TemplateID LEFT JOIN CODEMASTER cd WITH (NOLOCK) ON cd.Code=d.WasteType" & _
                            " AND cd.CodeType='WTY' WHERE h.ReceiverID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverID) & "'" & _
                            " AND h.ReceiverLocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "' AND h.WasteCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "' AND h.WasteType='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "' AND h.Flag='1' ORDER BY d.SeqNo"
                    Else
                        strSQL = "select ROW_NUMBER() OVER (order by d.SeqNo asc) as No, d.DocCode, d.WasteCode, d.WasteName, cd.CodeDesc AS WasteTypeDesc, d.HandlingType, Qty As QtyMT, Remark1 as Remark, '' as RemainingMT, d.WasteType, LastQty, Qty, SetQtyDate, RecType, " & _
                        " CASE WHEN h.Posted=1 then 'false' ELSE CASE WHEN d.RecType=2 THEN 'false' ELSE 'true' END END AS 'DeleteStatus', h.HandlingQty, h.ResQty, h.HandlingType AS type, h.Flag " & _
                        " from twg_resdtl d WITH (NOLOCK) inner join twg_reshdr h WITH (NOLOCK) on d.doccode = h.doccode" & _
                        " left join CODEMASTER cd WITH (NOLOCK) ON cd.Code=d.WasteType AND cd.CodeType='WTY'" & _
                        " WHERE h.DocCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, DocCode) & "' AND d.Flag=1" & _
                        " Order By SeqNo"
                    End If

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
#End Region

#Region "Container"
    Namespace Container
#Region "Twg_resdtl Container"
        Public Class Twg_resdtl_FieldName
            Public DocCode As System.String = "DocCode"
            Public WasteCode As System.String = "WasteCode"
            Public WasteName As System.String = "WasteName"
            Public WasteType As System.String = "WasteType"
            Public RecType As System.String = "RecType"
            Public SeqNo As System.String = "SeqNo"
            Public Qty As System.String = "Qty"
            Public ExpectedQty As System.String = "ExpectedQty"
            Public ResidueQty As System.String = "ResidueQty"
            Public HandlingQty1 As System.String = "HandlingQty1"
            Public HandlingQty2 As System.String = "HandlingQty2"
            Public HandlingQty3 As System.String = "HandlingQty3"
            Public HandlingType As System.String = "HandlingType"
            Public Remark1 As System.String = "Remark1"
            Public Remark2 As System.String = "Remark2"
            Public Remark3 As System.String = "Remark3"
            Public CreateDate As System.String = "CreateDate"
            Public CreateBy As System.String = "CreateBy"
            Public LastUpdate As System.String = "LastUpdate"
            Public UpdateBy As System.String = "UpdateBy"
            Public rowguid As System.String = "rowguid"
            Public SyncCreate As System.String = "SyncCreate"
            Public SyncLastUpd As System.String = "SyncLastUpd"
            Public LastSyncBy As System.String = "LastSyncBy"
        End Class

        Public Class Twg_resdtl
            Protected _DocCode As System.String
            Protected _WasteCode As System.String
            Protected _WasteName As System.String
            Protected _WasteType As System.String
            Protected _RecType As System.Byte
            Private _Qty As System.Decimal
            Private _LastQty As System.Decimal
            Private _ExpectedQty As System.Decimal
            Private _ResidueQty As System.Decimal
            Private _HandlingQty1 As System.Decimal
            Private _HandlingQty2 As System.Decimal
            Private _HandlingQty3 As System.Decimal
            Private _HandlingType As System.String
            Private _Remark1 As System.String
            Private _Remark2 As System.String
            Private _Remark3 As System.String
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.DateTime
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _LastSyncBy As System.String
            Private _SeqNo As System.Byte
            Private _SetQtyDate As System.DateTime
            Private _SetLastQtyDate As System.DateTime
            Private _Flag As System.Byte
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
            ''' Mandatory
            ''' </summary>
            Public Property WasteCode As System.String
                Get
                    Return _WasteCode
                End Get
                Set(ByVal Value As System.String)
                    _WasteCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property WasteName As System.String
                Get
                    Return _WasteName
                End Get
                Set(ByVal Value As System.String)
                    _WasteName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property WasteType As System.String
                Get
                    Return _WasteType
                End Get
                Set(ByVal Value As System.String)
                    _WasteType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property RecType As System.Byte
                Get
                    Return _RecType
                End Get
                Set(ByVal Value As System.Byte)
                    _RecType = Value
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
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Qty As System.Decimal
                Get
                    Return _Qty
                End Get
                Set(ByVal Value As System.Decimal)
                    _Qty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LastQty As System.Decimal
                Get
                    Return _LastQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _LastQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SetQtyDate As System.DateTime
                Get
                    Return _SetQtyDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _SetQtyDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SetLastQtyDate As System.DateTime
                Get
                    Return _SetLastQtyDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _SetLastQtyDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ExpectedQty As System.Decimal
                Get
                    Return _ExpectedQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _ExpectedQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ResidueQty As System.Decimal
                Get
                    Return _ResidueQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _ResidueQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property HandlingQty1 As System.Decimal
                Get
                    Return _HandlingQty1
                End Get
                Set(ByVal Value As System.Decimal)
                    _HandlingQty1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property HandlingQty2 As System.Decimal
                Get
                    Return _HandlingQty2
                End Get
                Set(ByVal Value As System.Decimal)
                    _HandlingQty2 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property HandlingQty3 As System.Decimal
                Get
                    Return _HandlingQty3
                End Get
                Set(ByVal Value As System.Decimal)
                    _HandlingQty3 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Remark1 As System.String
                Get
                    Return _Remark1
                End Get
                Set(ByVal Value As System.String)
                    _Remark1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Remark2 As System.String
                Get
                    Return _Remark2
                End Get
                Set(ByVal Value As System.String)
                    _Remark2 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Remark3 As System.String
                Get
                    Return _Remark3
                End Get
                Set(ByVal Value As System.String)
                    _Remark3 = Value
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
            Public Property UpdateBy As System.DateTime
                Get
                    Return _UpdateBy
                End Get
                Set(ByVal Value As System.DateTime)
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
            Public Property HandlingType As System.String
                Get
                    Return _HandlingType
                End Get
                Set(ByVal Value As System.String)
                    _HandlingType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SeqNo As System.Byte
                Get
                    Return _SeqNo
                End Get
                Set(ByVal Value As System.Byte)
                    _SeqNo = Value
                End Set
            End Property
        End Class
#End Region
    End Namespace
#End Region

#Region "Class Info"
#Region "Twg_resdtl Info"
    Public Class Twg_resdtlInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "DocCode,WasteCode,WasteName,WasteType,RecType,SeqNo,Qty,ExpectedQty,ResidueQty,HandlingQty1,HandlingQty2,HandlingQty3,HandlingType,Remark1,Remark2,Remark3,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid,SyncCreate,SyncLastUpd,LastSyncBy"
                .CheckFields = "RecType"
                .TableName = "Twg_resdtl"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "DocCode,WasteCode,WasteName,WasteType,RecType,SeqNo,Qty,ExpectedQty,ResidueQty,HandlingQty1,HandlingQty2,HandlingQty3,HandlingType,Remark1,Remark2,Remark3,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid,SyncCreate,SyncLastUpd,LastSyncBy"
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
#Region "TWG_RESDTL Scheme"
    Public Class TWG_RESDTLScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "DocCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WasteCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "WasteName"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WasteType"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "RecType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Qty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ExpectedQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ResidueQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "HandlingQty1"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "HandlingQty2"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "HandlingQty3"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark1"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark2"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark3"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "UpdateBy"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "HandlingType"
                .Length = 100
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
        End Sub

        Public ReadOnly Property DocCode As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property WasteCode As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property WasteName As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property WasteType As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property RecType As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property

        Public ReadOnly Property Qty As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property ExpectedQty As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property ResidueQty As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property HandlingQty1 As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property HandlingQty2 As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property HandlingQty3 As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property Remark1 As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property Remark2 As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property Remark3 As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property HandlingType As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace
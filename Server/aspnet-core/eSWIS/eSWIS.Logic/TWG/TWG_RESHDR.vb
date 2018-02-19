
Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace TWG

#Region "TWG_RESHDR Class"
    Public NotInheritable Class TWG_RESHDR
        Inherits Core.CoreControl
        Private Twg_reshdrInfo As Twg_reshdrInfo = New Twg_reshdrInfo
        Private Twg_resdtlInfo As Twg_resdtlInfo = New Twg_resdtlInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function Save(ByVal Twg_reshdrCont As Container.Twg_reshdr, ByVal ListContTWG_RESDtl As List(Of Container.Twg_resdtl), ByVal WasteCode As String, ByVal WasteType As String, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            BatchList = New ArrayList()
            Save = False
            Try
                If Twg_reshdrCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Twg_reshdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & Twg_reshdrCont.DocCode & "' AND ReceiverID = '" & Twg_reshdrCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_reshdrCont.ReceiverLocID & "'")
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
                                .TableName = "Twg_reshdr"
                                .AddField("TransDate", Twg_reshdrCont.TransDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Status", Twg_reshdrCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", Twg_reshdrCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", Twg_reshdrCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", Twg_reshdrCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", Twg_reshdrCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", Twg_reshdrCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", Twg_reshdrCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ReceiveDate", Twg_reshdrCont.ReceiveDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ReceivedQty", Twg_reshdrCont.ReceivedQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("HandlingQty", Twg_reshdrCont.HandlingQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ReceivedQty", Twg_reshdrCont.ReceivedQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LiveCal", Twg_reshdrCont.LiveCal, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RemainQty", Twg_reshdrCont.RemainQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ResQty", Twg_reshdrCont.ResQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("HandlingType", Twg_reshdrCont.HandlingType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastHandlingQty", Twg_reshdrCont.LastHandlingQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastResQty", Twg_reshdrCont.LastResQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastHandlingType", Twg_reshdrCont.LastHandlingType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ViewDate", Twg_reshdrCont.ViewDate, SQLControl.EnumDataType.dtNumeric)
                                .AddField("WasteCode", Twg_reshdrCont.WasteCode, SQLControl.EnumDataType.dtString)
                                .AddField("WasteType", Twg_reshdrCont.WasteType, SQLControl.EnumDataType.dtString)
                                .AddField("WasteName", Twg_reshdrCont.WasteName, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & Twg_reshdrCont.DocCode & "' AND ReceiverID = '" & Twg_reshdrCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_reshdrCont.ReceiverLocID & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("DocCode", Twg_reshdrCont.DocCode, SQLControl.EnumDataType.dtString)
                                                .AddField("ReceiverID", Twg_reshdrCont.ReceiverID, SQLControl.EnumDataType.dtString)
                                                .AddField("ReceiverLocID", Twg_reshdrCont.ReceiverLocID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & Twg_reshdrCont.DocCode & "' AND ReceiverID = '" & Twg_reshdrCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_reshdrCont.ReceiverLocID & "'")
                                End Select
                                BatchList.Add(strSQL)
                            End With

                            If ListContTWG_RESDtl Is Nothing OrElse ListContTWG_RESDtl.Count <= 0 Then
                                strSQL = BuildDelete(Twg_resdtlInfo.MyInfo.TableName, "DocCode = '" & Twg_reshdrCont.DocCode & "' AND Flag='1' ")
                                BatchList.Add(strSQL)
                            Else
                                blnExec = False
                                blnFound = False
                                blnFlag = False
                                If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                                    StartSQLControl()
                                    With Twg_resdtlInfo.MyInfo
                                        strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & ListContTWG_RESDtl(0).DocCode & "'")
                                    End With
                                    rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
                                    blnExec = True

                                    If rdr Is Nothing = False Then
                                        With rdr
                                            If .Read Then
                                                blnFound = True
                                            End If
                                            .Close()
                                        End With
                                    End If

                                    If blnExec Then
                                        If blnFound = True And pType = SQLControl.EnumSQLType.stUpdate Then
                                            strSQL = BuildDelete(Twg_resdtlInfo.MyInfo.TableName, "DocCode = '" & ListContTWG_RESDtl(0).DocCode & "' AND Flag='1'")
                                            BatchList.Add(strSQL)
                                        End If
                                    End If
                                End If

                                For Each Twg_resdtlCont In ListContTWG_RESDtl
                                    StartSQLControl()
                                    With objSQL
                                        .TableName = "Twg_resdtl"
                                        .AddField("Qty", Twg_resdtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("ExpectedQty", Twg_resdtlCont.ExpectedQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("ResidueQty", Twg_resdtlCont.ResidueQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("HandlingQty1", Twg_resdtlCont.HandlingQty1, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("HandlingQty2", Twg_resdtlCont.HandlingQty2, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("HandlingQty3", Twg_resdtlCont.HandlingQty3, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("HandlingType", Twg_resdtlCont.HandlingType, SQLControl.EnumDataType.dtString)
                                        .AddField("Remark1", Twg_resdtlCont.Remark1, SQLControl.EnumDataType.dtStringN)
                                        .AddField("Remark2", Twg_resdtlCont.Remark2, SQLControl.EnumDataType.dtStringN)
                                        .AddField("Remark3", Twg_resdtlCont.Remark3, SQLControl.EnumDataType.dtStringN)
                                        .AddField("CreateDate", Twg_resdtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                        .AddField("CreateBy", Twg_resdtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                                        .AddField("LastUpdate", Twg_resdtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                        .AddField("UpdateBy", Twg_resdtlCont.UpdateBy, SQLControl.EnumDataType.dtDateTime)

                                        .AddField("DocCode", Twg_resdtlCont.DocCode, SQLControl.EnumDataType.dtString)
                                        .AddField("WasteCode", Twg_resdtlCont.WasteCode, SQLControl.EnumDataType.dtString)
                                        .AddField("WasteName", Twg_resdtlCont.WasteName, SQLControl.EnumDataType.dtStringN)
                                        .AddField("WasteType", Twg_resdtlCont.WasteType, SQLControl.EnumDataType.dtString)
                                        .AddField("RecType", Twg_resdtlCont.RecType, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("SeqNo", Twg_resdtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("LastQty", Twg_resdtlCont.LastQty, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("SetQtyDate", Twg_resdtlCont.SetQtyDate, SQLControl.EnumDataType.dtDateTime)
                                        .AddField("SetLastQtyDate", Twg_resdtlCont.SetLastQtyDate, SQLControl.EnumDataType.dtDateTime)
                                        .AddField("Flag", Twg_resdtlCont.Flag, SQLControl.EnumDataType.dtNumeric)
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                    End With
                                    BatchList.Add(strSQL)
                                Next
                            End If

                            If Twg_reshdrCont IsNot Nothing And Twg_reshdrCont.DocCode IsNot Nothing Then
                                strSQL = "EXEC sp_LiveInvWREdit '" & Twg_reshdrCont.DocCode & "'"
                                BatchList.Add(strSQL)
                            Else
                                strSQL = "EXEC sp_LiveInvWREdit '" & ListContTWG_RESDtl(0).DocCode & "'"
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
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Return False
            Finally
                Twg_reshdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal Twg_reshdrCont As Container.Twg_reshdr, ByVal ListContTWG_RESDtl As List(Of Container.Twg_resdtl), ByVal WasteCode As String, ByVal WasteType As String, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Twg_reshdrCont, ListContTWG_RESDtl, WasteCode, WasteType, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal Twg_reshdrCont As Container.Twg_reshdr, ByVal ListContTWG_RESDtl As List(Of Container.Twg_resdtl), ByVal WasteCode As String, ByVal WasteType As String, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Twg_reshdrCont, ListContTWG_RESDtl, WasteCode, WasteType, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal Twg_reshdrCont As Container.Twg_reshdr, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Twg_reshdrCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Twg_reshdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & Twg_reshdrCont.DocCode & "' AND ReceiverID = '" & Twg_reshdrCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_reshdrCont.ReceiverLocID & "'")
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
                                strSQL = BuildUpdate(Twg_reshdrInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = '" & Twg_reshdrCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_reshdrCont.UpdateBy) & "' WHERE" & _
                                "DocCode = '" & Twg_reshdrCont.DocCode & "' AND ReceiverID = '" & Twg_reshdrCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_reshdrCont.ReceiverLocID & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Twg_reshdrInfo.MyInfo.TableName, "DocCode = '" & Twg_reshdrCont.DocCode & "' AND ReceiverID = '" & Twg_reshdrCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_reshdrCont.ReceiverLocID & "'")
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
                Twg_reshdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function DeleteActivityDate(ByVal Twg_reshdrCont As Container.Twg_reshdr, ByVal ListContTWG_RESDtl As List(Of Container.Twg_resdtl), ByRef message As String, Optional ByRef BatchList As ArrayList = Nothing) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            DeleteActivityDate = False
            blnFound = False
            blnInUse = False
            BatchList = New ArrayList()
            Try
                If Twg_reshdrCont Is Nothing OrElse ListContTWG_RESDtl Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Twg_reshdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_reshdrCont.DocCode) & "' AND ReceiverID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_reshdrCont.ReceiverID) & "' AND ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_reshdrCont.ReceiverLocID) & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("InUse")) = 0 Then
                                        blnInUse = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        If blnFound = True And blnInUse = True Then
                            With objSQL
                                strSQL = BuildUpdate(Twg_reshdrInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = GETDATE() , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_reshdrCont.UpdateBy) & "', HandlingQty=0," & _
                                " LastHandlingQty=" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Twg_reshdrCont.LastHandlingQty) & ", ResQty=0,LastResQty=" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_reshdrCont.LastResQty) & ", STATUS='5', LiveCal='1' WHERE " & _
                                "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_reshdrCont.DocCode) & "' AND ReceiverID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_reshdrCont.ReceiverID) & "' AND ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_reshdrCont.ReceiverLocID) & "'")
                            End With
                            BatchList.Add(strSQL)

                            strSQL = BuildDelete(Twg_resdtlInfo.MyInfo.TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_reshdrCont.DocCode) & "'")
                            BatchList.Add(strSQL)

                            For Each Twg_resdtlCont In ListContTWG_RESDtl
                                StartSQLControl()
                                With objSQL
                                    .TableName = "Twg_resdtl"
                                    .AddField("Qty", Twg_resdtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ExpectedQty", Twg_resdtlCont.ExpectedQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ResidueQty", Twg_resdtlCont.ResidueQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("HandlingQty1", Twg_resdtlCont.HandlingQty1, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("HandlingQty2", Twg_resdtlCont.HandlingQty2, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("HandlingQty3", Twg_resdtlCont.HandlingQty3, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("HandlingType", Twg_resdtlCont.HandlingType, SQLControl.EnumDataType.dtString)
                                    .AddField("Remark1", Twg_resdtlCont.Remark1, SQLControl.EnumDataType.dtStringN)
                                    .AddField("Remark2", Twg_resdtlCont.Remark2, SQLControl.EnumDataType.dtStringN)
                                    .AddField("Remark3", Twg_resdtlCont.Remark3, SQLControl.EnumDataType.dtStringN)
                                    .AddField("CreateDate", Twg_resdtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateBy", Twg_resdtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("LastUpdate", Twg_resdtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("UpdateBy", Twg_resdtlCont.UpdateBy, SQLControl.EnumDataType.dtDateTime)

                                    .AddField("DocCode", Twg_resdtlCont.DocCode, SQLControl.EnumDataType.dtString)
                                    .AddField("WasteCode", Twg_resdtlCont.WasteCode, SQLControl.EnumDataType.dtString)
                                    .AddField("WasteName", Twg_resdtlCont.WasteName, SQLControl.EnumDataType.dtStringN)
                                    .AddField("WasteType", Twg_resdtlCont.WasteType, SQLControl.EnumDataType.dtString)
                                    .AddField("RecType", Twg_resdtlCont.RecType, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("SeqNo", Twg_resdtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastQty", Twg_resdtlCont.LastQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("SetQtyDate", Twg_resdtlCont.SetQtyDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("SetLastQtyDate", Twg_resdtlCont.SetLastQtyDate, SQLControl.EnumDataType.dtDateTime)

                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                End With
                                BatchList.Add(strSQL)
                            Next
                        End If

                        strSQL = "EXEC sp_LiveInvWREdit '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_reshdrCont.DocCode) & "'"
                        BatchList.Add(strSQL)

                        Try
                            objConn.BatchExecute(BatchList, DataAccess.EnumRtnType.rtNone, CommandType.Text)
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
                Twg_reshdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function UpdateTemplate(ByVal DocCode As String, ByVal ReceiverID As String, ByVal ReceiverLocID As String, ByVal WasteCode As String, ByVal WasteType As String) As Boolean
            Dim BatchList As ArrayList = New ArrayList()

            Try
                If StartConnection() = True Then
                    With Twg_reshdrInfo.MyInfo
                        StartSQLControl()

                        strSQL = "Update H SET STATUS = 1 from TWG_RESHDR" & _
                            " WHERE ReceiverID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverID) & "'" & _
                            " AND ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "'" & _
                            " AND WasteCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "'" & _
                            " AND WasteType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "'"

                        BatchList.Add(strSQL)

                        strSQL = "Update TWG_RESHDR SET STATUS = 2 WHERE DOCCODE = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, DocCode) & "'"
                        BatchList.Add(strSQL)

                        objConn.BatchExecute(BatchList, CommandType.Text, True)

                        Return True
                    End With
                End If
            Catch axAssign As ApplicationException
                Return False
            Catch exAssign As SystemException
                Return False
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function UpdatePost(ByVal ReceiverID As String, ByVal ReceiverLocID As String, ByVal Month As String, ByVal Year As String) As Boolean
            Dim BatchList As ArrayList = New ArrayList()

            Try
                If StartConnection() = True Then
                    With Twg_reshdrInfo.MyInfo
                        StartSQLControl()

                        strSQL = "Update TWG_RESHDR SET Posted = 1 " & _
                            " WHERE ReceiverID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverID) & "'" & _
                            " AND ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "'" & _
                            " AND MONTH(ReceiveDate) = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Month) & "'" & _
                            " AND YEAR(ReceiveDate) = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Year) & "'"

                        BatchList.Add(strSQL)

                        strSQL = "EXEC sp_LiveInvWRPost '" & ReceiverLocID & "', '" & Month & "', '" & Year & "'"
                        BatchList.Add(strSQL)

                        objConn.BatchExecute(BatchList, CommandType.Text, True)

                        Return True
                    End With
                End If
            Catch axAssign As ApplicationException
                Return False
            Catch exAssign As SystemException
                Return False
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function isPrevMonthExist(ByVal ReceiverID As String, ByVal ReceiverLocID As String, ByVal Month As String, ByVal Year As String) As Boolean
            Dim BatchList As ArrayList = New ArrayList()
            Dim dt As New DataTable
            isPrevMonthExist = False
            Try
                If StartConnection() = True Then
                    With Twg_reshdrInfo.MyInfo
                        StartSQLControl()

                        strSQL = "Select * from TWG_RESHDR WITH(NOLOCK)" & _
                            " WHERE ReceiverID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverID) & "'" & _
                            " AND ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "'" & _
                            " AND MONTH(ReceiveDate) = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Month) & "'" & _
                            " AND YEAR(ReceiveDate) = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Year) & "'"

                        dt = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, "BIZLICENSE"), Data.DataTable)

                        If dt.Rows.Count > 0 Then
                            If dt.Rows(0)("Posted").ToString = "1" Then
                                isPrevMonthExist = True
                            Else
                                isPrevMonthExist = False
                            End If
                        Else
                            isPrevMonthExist = False
                        End If
                        Return isPrevMonthExist
                    End With
                End If
            Catch axAssign As ApplicationException
                Return False
            Catch exAssign As SystemException
                Return False
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region

#Region "Data Selection"
        Public Overloads Function GetTWG_RESHDR(ByVal DocCode As System.String, ByVal ReceiverID As System.String, ByVal ReceiverLocID As System.String) As Container.Twg_reshdr
            Dim rTwg_reshdr As Container.Twg_reshdr = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With Twg_reshdrInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "DocCode = '" & DocCode & "' AND ReceiverID = '" & ReceiverID & "' AND ReceiverLocID = '" & ReceiverLocID & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rTwg_reshdr = New Container.Twg_reshdr
                                rTwg_reshdr.DocCode = drRow.Item("DocCode")
                                rTwg_reshdr.ReceiverID = drRow.Item("ReceiverID")
                                rTwg_reshdr.ReceiverLocID = drRow.Item("ReceiverLocID")
                                rTwg_reshdr.TransDate = drRow.Item("TransDate")
                                rTwg_reshdr.Status = drRow.Item("Status")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rTwg_reshdr.CreateDate = drRow.Item("CreateDate")
                                End If
                                rTwg_reshdr.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rTwg_reshdr.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rTwg_reshdr.UpdateBy = drRow.Item("UpdateBy")
                                rTwg_reshdr.Active = drRow.Item("Active")
                                rTwg_reshdr.Inuse = drRow.Item("Inuse")
                                rTwg_reshdr.rowguid = drRow.Item("rowguid")
                                rTwg_reshdr.SyncCreate = drRow.Item("SyncCreate")
                                rTwg_reshdr.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rTwg_reshdr.LastSyncBy = drRow.Item("LastSyncBy")
                                rTwg_reshdr.HandlingQty = drRow.Item("HandlingQty")
                                rTwg_reshdr.ResQty = drRow.Item("ResQty")
                                rTwg_reshdr.HandlingType = drRow.Item("HandlingType")
                            Else
                                rTwg_reshdr = Nothing
                            End If
                        Else
                            rTwg_reshdr = Nothing
                        End If
                    End With
                End If
                Return rTwg_reshdr
            Catch ex As Exception
                Throw ex
            Finally
                rTwg_reshdr = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTWG_RESHDR(ByVal DocCode As System.String, ByVal ReceiverID As System.String, ByVal ReceiverLocID As System.String, DecendingOrder As Boolean) As List(Of Container.Twg_reshdr)
            Dim rTwg_reshdr As Container.Twg_reshdr = Nothing
            Dim lstTwg_reshdr As List(Of Container.Twg_reshdr) = New List(Of Container.Twg_reshdr)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With Twg_reshdrInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by DocCode, ReceiverID, ReceiverLocID DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "DocCode = '" & DocCode & "' AND ReceiverID = '" & ReceiverID & "' AND ReceiverLocID = '" & ReceiverLocID & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rTwg_reshdr = New Container.Twg_reshdr
                                rTwg_reshdr.DocCode = drRow.Item("DocCode")
                                rTwg_reshdr.ReceiverID = drRow.Item("ReceiverID")
                                rTwg_reshdr.ReceiverLocID = drRow.Item("ReceiverLocID")
                                rTwg_reshdr.TransDate = drRow.Item("TransDate")
                                rTwg_reshdr.Status = drRow.Item("Status")
                                rTwg_reshdr.CreateBy = drRow.Item("CreateBy")
                                rTwg_reshdr.UpdateBy = drRow.Item("UpdateBy")
                                rTwg_reshdr.Active = drRow.Item("Active")
                                rTwg_reshdr.Inuse = drRow.Item("Inuse")
                                rTwg_reshdr.rowguid = drRow.Item("rowguid")
                                rTwg_reshdr.SyncCreate = drRow.Item("SyncCreate")
                                rTwg_reshdr.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rTwg_reshdr.LastSyncBy = drRow.Item("LastSyncBy")
                                lstTwg_reshdr.Add(rTwg_reshdr)
                            Next

                        Else
                            rTwg_reshdr = Nothing
                        End If
                        Return lstTwg_reshdr
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rTwg_reshdr = Nothing
                lstTwg_reshdr = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTWG_RESHDRListHandling(ByVal ReceiverID As String, ByVal ReceiverLocID As String, ByVal Month As String, ByVal Year As String, ByVal WasteCode As String, ByVal WasteType As String, ByVal WasteName As String, Optional ByVal DocCode As String = Nothing, Optional ByVal DescOrder As Boolean = False) As DataTable
            Try
                If StartConnection() = True Then
                    With Twg_reshdrInfo.MyInfo
                        StartSQLControl()

                        strSQL = " Select * from GetWasteHandlingDetail('" & Year & "','" & Month & "','" & ReceiverID & "','" & ReceiverLocID & "','" & WasteCode & "','" & WasteType & "','" & WasteName & "')"

                        Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    End With
                Else
                    Return Nothing
                End If
            Catch ex As Exception
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Overloads Function GetTWG_RESHDRgvList(ByVal ReceiverID As String, ByVal ReceiverLocID As String, Optional ByVal Month As String = "", Optional ByVal Year As String = "GetD") As DataTable
            Try
                If StartConnection() = True Then
                    With Twg_reshdrInfo.MyInfo
                        StartSQLControl()

                        strSQL = "SELECT han.MthCode, han.YearCode, han.Opening AS 'OpeningQty', iloc.ItemCode AS WasteCode, iloc.ItemName, c.Code AS WasteType, c.CodeDesc AS WasteTypeDesc," &
                                 " Case iloc.IsSelected WHEN 1 THEN 0 Else ISNULL(han.QtyIn,0) END AS RcvQty, " &
                                 " Case iloc.IsSelected WHEN 1 THEN 0 Else ISNULL(han.QtyHandling,0) END AS HandlingQty, " &
                                 " Case iloc.IsSelected WHEN 1 THEN ISNULL(han.QtyIn,0) Else ISNULL(han.QtyReused,0) END AS ResidueQty, " &
                                 " Case iloc.IsSelected WHEN 1 THEN 0 Else ISNULL(han.Closing,0) END AS QtyOnHand, " &
                                 " Case iloc.IsSelected WHEN 1 THEN ISNULL(han.QtyHandling + han.QtyReused,0) Else 0 END AS HandlingResidueQty, " &
                                 " Case iloc.IsSelected WHEN 1 THEN ISNULL(han.Closing,0) Else 0 END AS QtyResidueOnHand, iloc.ItemDesc, " &
                                 " CASE iloc.IsSelected WHEN 1 THEN 'true' ELSE 'false' END AS IsResidue, max(TR.posted) posted, " &
                                 " Case iloc.IsSelected WHEN 1 THEN ISNULL(han.QtyAdj,0) Else 0 END AS QtyAdj, 'Generated Residue' as ComponentType," &
                                 " CASE WHEN MAX(Cx.EditDateMax) IS NOT NULL AND MAX(Cx.EditDateMax) > ISNULL(Hx.ActDateMax,0) THEN 1 ELSE 0 END IsEdited," &
                                 " CASE WHEN Hx.ActDateMax IS NOT NULL AND Hx.ActDateMax > ISNULL(MAX(Cx.EditDateMax),0) THEN 1 ELSE 0 END IsRecent, Hx.ActDateMax, MAX(Cx.EditDateMax) EditDateMax " &
                                 " FROM ITEMLOC iloc WITH(NOLOCK) LEFT JOIN CODEMASTER c WITH(NOLOCK) ON c.Code= iloc.TypeCode AND c.CodeType='WTY'" &
                                 " LEFT JOIN HANDLESUMMARY han WITH(NOLOCK) ON  han.LocID=iloc.LocID AND han.ItemCode=iloc.ItemCode AND han.ItemName=iloc.ItemName" &
                                 " LEFT JOIN TWG_RESHDR TR on TR.ReceiverLocID = iloc.LocID and TR.WasteCode = iloc.ItemCode AND MONTH(TR.ReceiveDate) = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Month) & "' AND YEAR(TR.ReceiveDate) = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Year) & "' AND TR.Flag = 1 AND TR.Status <> 0 " &
                                 " OUTER APPLY (SELECT MAX(TransDate) EditDateMax FROM CONSIGNHDR H WITH (NOLOCK) INNER JOIN CONSIGNDTL D WITH (NOLOCK) ON D.TransID = H.TransID AND D.WasteCode = iloc.ItemCode AND D.WasteDescription = iloc.ItemName WHERE h.FLAG = 1 AND h.STATUS <> 0 AND h.STATUS <> 2 AND h.STATUS <> 12 AND h.STATUS <> 13 AND h.Status <> 14 AND h.STATUS <> 15 AND h.ISCONFIRM = 1 AND H.GeneratorLocID = iloc.LocID AND CONVERT(varchar(6),H.TransDate,112) <= CONVERT(varchar(6),CONVERT(DATE,CONCAT(han.YearCode,'-',han.MthCode,'-1')),112) UNION SELECT MAX(TransDate) EditDateMax FROM ITMTRANSDTL d WITH (NOLOCK) INNER JOIN ITMTRANSHDR h WITH (NOLOCK) ON h.DOCCODE = d.DOCCODE AND h.TERMID = 1 AND h.TRANSTYPE = 1 AND h.STATUS = 1 AND h.ACTIVE = 1 AND h.FLAG = 1 WHERE d.TRANSTYPE = 1 AND CONVERT(varchar(6),H.TransDate,112) <= CONVERT(varchar(6),CONVERT(DATE,CONCAT(han.YearCode,'-',han.MthCode,'-1')),112) AND d.LOCID = iloc.LocID AND d.ITEMCODE = iloc.ItemCode AND d.ITEMNAME = iloc.ItemName) Cx " &
                                 "  OUTER APPLY (SELECT MAX(TransDate) ActDateMax FROM TWG_RESHDR H WITH (NOLOCK) INNER JOIN TWG_RESDTL D WITH (NOLOCK) ON D.DocCode = H.DocCode AND D.Flag = 1 WHERE H.Status <> 0 AND H.Flag = 1 AND H.ReceiverLocID = iloc.LocID AND D.WasteCode = iloc.ItemCode AND D.WasteType = iloc.TypeCode AND D.WasteName = iloc.ItemName AND CONVERT(varchar(6),H.ReceiveDate,112) <= CONVERT(varchar(6),CONVERT(DATE,CONCAT(han.YearCode,'-',han.MthCode,'-1')),112)) Hx " &
                                 " WHERE iloc.LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "' AND han.MthCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Month) & "'  AND han.YearCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Year) & "' AND iloc.Flag=1 and iloc.isselected = 1 " &
                                 " Group by han.MthCode, han.YearCode, han.Opening, iloc.ItemCode, iloc.ItemName, c.Code, c.CodeDesc, ISNULL(han.QtyIn,0), ISNULL(han.QtyHandling,0), ISNULL(han.QtyReused,0), ISNULL(han.Closing,0), iloc.ItemDesc, iloc.IsSelected, ISNULL(han.QtyHandling + han.QtyReused,0),ISNULL(han.QtyAdj,0),Hx.ActDateMax " &
                                 " UNION " &
                                 " SELECT han.MthCode, han.YearCode, han.Opening AS 'OpeningQty', iloc.ItemCode AS WasteCode, iloc.ItemName, c.Code AS WasteType, c.CodeDesc AS WasteTypeDesc," &
                                 " Case iloc.IsSelected WHEN 1 THEN 0 Else ISNULL(han.QtyIn,0) END AS RcvQty, " &
                                 " Case iloc.IsSelected WHEN 1 THEN 0 Else ISNULL(han.QtyHandling,0) END AS HandlingQty, " &
                                 " Case iloc.IsSelected WHEN 1 THEN ISNULL(han.QtyIn,0) Else 0 END AS ResidueQty, " &
                                 " Case iloc.IsSelected WHEN 1 THEN 0 Else ISNULL(han.Closing,0) END AS QtyOnHand, " &
                                 " Case iloc.IsSelected WHEN 1 THEN ISNULL(han.QtyHandling + han.QtyReused,0) Else 0 END AS HandlingResidueQty, " &
                                 " Case iloc.IsSelected WHEN 1 THEN ISNULL(han.Closing,0) Else 0 END AS QtyResidueOnHand, iloc.ItemDesc, " &
                                 " CASE iloc.IsSelected WHEN 1 THEN 'true' ELSE 'false' END AS IsResidue, max(TR.posted) posted, " &
                                 " Case iloc.IsSelected WHEN 1 THEN ISNULL(han.QtyAdj,0) Else 0 END AS QtyAdj, 'Received Waste' as ComponentType," &
                                 " CASE WHEN Hx.EditDateMax IS NOT NULL AND Hx.EditDateMax > ISNULL(Cx.ActDateMax,0) THEN 1 ELSE 0 END IsEdited, " &
                                 " CASE WHEN Cx.ActDateMax IS NOT NULL AND Cx.ActDateMax > ISNULL(Hx.EditDateMax,0) THEN 1 ELSE 0 END IsRecent , Cx.ActDateMax, Hx.EditDateMax" & _
                                 " FROM ITEMHNDLOC iloc WITH(NOLOCK) LEFT JOIN CODEMASTER c WITH(NOLOCK) ON c.Code= iloc.TypeCode AND c.CodeType='WTY'" &
                                 " LEFT JOIN HANDLESUMMARY han WITH(NOLOCK) ON  han.LocID=iloc.LocID AND han.ItemCode=iloc.ItemCode AND han.ItemName=iloc.ItemName" &
                                 " LEFT JOIN TWG_RESHDR TR on TR.ReceiverLocID = iloc.LocID and TR.WasteCode = iloc.ItemCode AND MONTH(TR.ReceiveDate) = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Month) & "'" &
                                 " AND YEAR(TR.ReceiveDate) = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Year) & "' AND TR.Flag = 1 AND TR.Status <> 0 " & _
                                 " OUTER APPLY (SELECT MAX(ReceiveDate) ActDateMax FROM CONSIGNHDR H WITH (NOLOCK) INNER JOIN CONSIGNDTL D WITH (NOLOCK) ON D.TransID = H.TransID AND D.WasteCode = iloc.ItemCode AND D.WasteType = iloc.TypeCode WHERE H.Status = 8 AND H.ReceiverLocID = iloc.LocID AND CONVERT(varchar(6),H.ReceiveDate,112) <= CONVERT(varchar(6),CONVERT(DATE,CONCAT(han.YearCode,'-',han.MthCode,'-1')),112)) Cx " &
                                 " OUTER APPLY (SELECT MAX(TransDate) EditDateMax FROM TWG_RESHDR H WITH (NOLOCK) WHERE H.Status <> 0 AND H.Flag = 1 AND H.ReceiverLocID = iloc.LocID AND H.WasteCode = iloc.ItemCode AND H.WasteType = iloc.TypeCode AND H.WasteName = iloc.ItemName AND CONVERT(varchar(6),H.ReceiveDate,112) <= CONVERT(varchar(6),CONVERT(DATE,CONCAT(han.YearCode,'-',han.MthCode,'-1')),112)) Hx" &
                                 " WHERE iloc.LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "' AND han.MthCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Month) & "'  AND han.YearCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Year) & "' AND iloc.Flag=1 " &
                                 " Group by  han.MthCode, han.YearCode, han.Opening, iloc.ItemCode, iloc.ItemName, c.Code, c.CodeDesc, ISNULL(han.QtyIn,0), ISNULL(han.QtyHandling,0), " &
                                 " ISNULL(han.QtyReused,0), ISNULL(han.Closing,0), iloc.ItemDesc, iloc.IsSelected, ISNULL(han.QtyHandling + han.QtyReused,0),ISNULL(han.QtyAdj,0), " &
                                 " Cx.ActDateMax,Hx.EditDateMax"

                        Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    End With
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("TWG/TWG_RESHDR.vb", ex.Message, ex.StackTrace)
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Overloads Function GetPremiseHaPrint(ByVal ReceiverID As String, ByVal ReceiverLocID As String, Optional ByVal Month As String = Nothing, Optional ByVal Year As String = Nothing, Optional ByVal DocCode As String = Nothing, Optional ByVal DescOrder As Boolean = False) As Data.DataTable
            If StartConnection() = True Then
                With Twg_reshdrInfo.MyInfo
                    StartSQLControl()

                    strSQL = "EXEC [rpt_HandlingTWGE] '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "', '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Month) & "', '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Year) & "'"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function MassBalanceList(ByVal Month As String, ByVal Year As String, Optional WasteCode As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Twg_reshdrInfo.MyInfo
                    StartSQLControl()

                    strSQL = "SELECT TOP 100 ROW_NUMBER() OVER (order by SUM(hdr.ResQty) DESC) as No, MAX(hdr.DocCode) AS DocCode, hdr.ReceiverID, biz.CompanyName, hdr.WasteCode, hdr.WasteType, hdr.WasteName, c.CodeDesc AS WasteTypeDesc," & _
                        " SUM(hdr.HandlingQty) AS HandlingQty, SUM(hdr.ResQty) AS ResidueQty, Max(CONVERT(VARCHAR(11), hdr.CreateDate, 103)) AS CreateDate from TWG_RESHDR hdr WITH (NOLOCK)" & _
                        " LEFT JOIN BIZENTITY biz WITH (NOLOCK) ON hdr.ReceiverID=biz.BizRegID" & _
                        " LEFT JOIN CODEMASTER c WITH(NOLOCK) ON c.Code= hdr.WasteType AND c.CodeType='WTY'" & _
                        " WHERE hdr.Posted=1 AND MONTH(hdr.ReceiveDate)='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Month) & "' AND YEAR(hdr.ReceiveDate)='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Year) & "'"

                    If WasteCode IsNot Nothing AndAlso WasteCode <> "" Then
                        strSQL &= " AND hdr.WasteCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "'"
                    End If

                    strSQL &= " GROUP BY hdr.ReceiverID, biz.CompanyName, hdr.WasteCode, hdr.WasteType, hdr.WasteName, c.CodeDesc ORDER BY SUM(hdr.ResQty) DESC"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetTWGListforMassBalance(ByVal DocCode As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_reshdrInfo.MyInfo
                    StartSQLControl()

                    strSQL = "SELECT hdr.DocCode, hdr.WasteCode, hdr.WasteType, cm.CodeDesc AS WasteTypeDesc, hdr.WasteName, ic.ItemDesc, MONTH(hdr.ReceiveDate) AS MonthCode, Year(hdr.ReceiveDate) AS YearCode, hdr.ReceiverID, hdr.ReceiverLocID FROM TWG_RESHDR hdr WITH (NOLOCK)" & _
                       " LEFT JOIN CODEMASTER cm WITH(NOLOCK) ON hdr.WasteType=cm.Code AND cm.CodeType='WTY'" & _
                       " LEFT JOIN ITEMHNDLOC ic WITH(NOLOCK) ON ic.LocID=hdr.ReceiverLocID AND hdr.WasteCode=ic.ItemCode AND" & _
                       " hdr.WasteName=ic.ItemName AND hdr.WasteType=ic.TypeCode WHERE hdr.DocCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, DocCode) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetHandleSummarybyLocID(ByVal LocID As String, ByVal ItemCode As String, ByVal ItemName As String, ByVal ItemType As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_reshdrInfo.MyInfo
                    StartSQLControl()
                    strSQL = "Select count(*) from handlesummary WHERE LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND ItemCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "'" &
                            " AND ItemName='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemName) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetTWGbyLocID(ByVal LocID As String, ByVal ItemCode As String, ByVal ItemName As String, ByVal ItemType As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_reshdrInfo.MyInfo
                    StartSQLControl()
                    strSQL = "Select count(DocCode) from TWG_ResHDR WHERE ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND WasteCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "'" &
                            " AND WasteName='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemName) & "' AND WasteType='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemType) & "'"
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
#Region "Twg_reshdr Container"
        Public Class Twg_reshdr_FieldName
            Public DocCode As System.String = "DocCode"
            Public ReceiverID As System.String = "ReceiverID"
            Public ReceiverLocID As System.String = "ReceiverLocID"
            Public TransDate As System.String = "TransDate"
            Public Status As System.String = "Status"
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
            Public Viewdate As System.String = "ViewDate"
        End Class

        Public Class Twg_reshdr
            Protected _DocCode As System.String
            Protected _ReceiverID As System.String
            Protected _ReceiverLocID As System.String
            Private _TransDate As System.DateTime
            Private _Status As System.Byte
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
            Private _ReceiveDate As System.DateTime
            Private _ReceivedQty As System.Decimal
            Private _HandlingQty As System.Decimal
            Private _LastHandlingQty As System.Decimal
            Private _LiveCal As System.Byte
            Private _RemainQty As System.Decimal
            Private _ResQty As System.Decimal
            Private _LastResQty As System.Decimal
            Private _HandlingType As System.Byte
            Private _LastHandlingType As System.Byte
            Private _ExcessQty As System.Decimal
            Protected _PrevDocCode As System.String
            Private _ViewDate As System.Byte
            Private _WasteCode As System.String
            Private _WasteType As System.String
            Private _WasteName As System.String
            Private _OldWasteName As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property OldWasteCode As System.String
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
            Public Property ExcessQty As System.Decimal
                Get
                    Return _ExcessQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _ExcessQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property LastResQty As System.Decimal
                Get
                    Return _LastResQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _LastResQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property LastHandlingQty As System.Decimal
                Get
                    Return _LastHandlingQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _LastHandlingQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property HandlingQty As System.Decimal
                Get
                    Return _HandlingQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _HandlingQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ReceivedQty As System.Decimal
                Get
                    Return _ReceivedQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _ReceivedQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property RemainQty As System.Decimal
                Get
                    Return _RemainQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _RemainQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ResQty As System.Decimal
                Get
                    Return _ResQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _ResQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property HandlingType As System.Byte
                Get
                    Return _HandlingType
                End Get
                Set(ByVal Value As System.Byte)
                    _HandlingType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property LastHandlingType As System.Byte
                Get
                    Return _LastHandlingType
                End Get
                Set(ByVal Value As System.Byte)
                    _LastHandlingType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property LiveCal As System.Byte
                Get
                    Return _LiveCal
                End Get
                Set(ByVal Value As System.Byte)
                    _LiveCal = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ReceiveDate As System.DateTime
                Get
                    Return _ReceiveDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _ReceiveDate = Value
                End Set
            End Property

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
            Public Property ReceiverID As System.String
                Get
                    Return _ReceiverID
                End Get
                Set(ByVal Value As System.String)
                    _ReceiverID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ReceiverLocID As System.String
                Get
                    Return _ReceiverLocID
                End Get
                Set(ByVal Value As System.String)
                    _ReceiverLocID = Value
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

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ViewDate As System.Byte
                Get
                    Return _ViewDate
                End Get
                Set(ByVal Value As System.Byte)
                    _ViewDate = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace
#End Region

#Region "Class Info"
#Region "Twg_reshdr Info"
    Public Class Twg_reshdrInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "DocCode,ReceiverID,ReceiverLocID,TransDate,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,LastSyncBy, HandlingQty, ResQty, HandlingType"
                .CheckFields = "Status,Active,Inuse,Flag"
                .TableName = "Twg_reshdr"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "DocCode,ReceiverID,ReceiverLocID,TransDate,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,LastSyncBy,HandlingQty, ResQty, HandlingType"
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
#Region "TWG_RESHDR Scheme"
    Public Class TWG_RESHDRScheme
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
                .FieldName = "ReceiverID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ReceiverLocID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "TransDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
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
                .IsMandatory = False
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
                .IsMandatory = False
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
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)

        End Sub

        Public ReadOnly Property DocCode As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property ReceiverID As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property ReceiverLocID As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property

        Public ReadOnly Property TransDate As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
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
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace


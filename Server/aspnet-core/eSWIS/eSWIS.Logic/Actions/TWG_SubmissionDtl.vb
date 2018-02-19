
Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace Actions
#Region "TWG_SUBMISSIONDTL Class"
    Public NotInheritable Class TWG_SUBMISSIONDTL
        Inherits Core.CoreControl
        Private Twg_submissiondtlInfo As Twg_submissiondtlInfo = New Twg_submissiondtlInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function UpdateQtys(ByVal ListContDel As List(Of Actions.Container.Twg_submissiondtl), ByVal ListContConsign As List(Of Actions.Container.Consigndtl), ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            UpdateQtys = False

            If ListContDel Is Nothing AndAlso ListContDel.Count > 0 Then
                'Message return
            Else
                StartSQLControl()

                For Each row As Actions.Container.Twg_submissiondtl In ListContDel
                    strSQL = "UPDATE TWG_SUBMISSIONDTL SET ExpectedQty = " & row.ExpectedQty & " , ExpectedQty1 = " & row.ExpectedQty1 & ", UpdateBy = '" & row.UpdateBy & "', LastUpdate = '" & row.LastUpdate & "' " & _
                        "WHERE SubmissionID='" & row.SubmissionID & "'"
                    ListSQL.Add(strSQL)
                Next
            End If
            If ListContConsign Is Nothing AndAlso ListContConsign.Count > 0 Then
                'Message return
            Else
                StartSQLControl()

                For Each row As Actions.Container.Consigndtl In ListContConsign
                    strSQL = "UPDATE CONSIGNDTL SET Qty = " & row.Qty & ", UpdateBy = '" & row.UpdateBy & "', LastUpdate = '" & row.LastUpdate & "'" & _
                        " WHERE TransID='" & row.TransID & "'"
                    ListSQL.Add(strSQL)
                Next
            End If

            Try
                objConn.BatchExecute(ListSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Return False
            Finally
                ListContDel = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
            Return True
        End Function

        Private Function Save(ByVal Twg_submissiondtlCont As Container.Twg_submissiondtl, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If Twg_submissiondtlCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Twg_submissiondtlInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "SubmissionID = '" & Twg_submissiondtlCont.SubmissionID & "' AND WasteCode = '" & Twg_submissiondtlCont.WasteCode & "' AND WasteDescription = '" & Twg_submissiondtlCont.WasteDescription & "' AND RecType = '" & Twg_submissiondtlCont.RecType & "'")
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
                                .TableName = "Twg_submissiondtl"
                                .AddField("Qty", Twg_submissiondtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ExpectedQty", Twg_submissiondtlCont.ExpectedQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ApprovedQty", Twg_submissiondtlCont.ApprovedQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("HandlingQty1", Twg_submissiondtlCont.HandlingQty1, SQLControl.EnumDataType.dtNumeric)
                                .AddField("HandlingQty2", Twg_submissiondtlCont.HandlingQty2, SQLControl.EnumDataType.dtNumeric)
                                .AddField("HandlingQty3", Twg_submissiondtlCont.HandlingQty3, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Remark1", Twg_submissiondtlCont.Remark1, SQLControl.EnumDataType.dtStringN)
                                .AddField("Remark2", Twg_submissiondtlCont.Remark2, SQLControl.EnumDataType.dtStringN)
                                .AddField("Remark3", Twg_submissiondtlCont.Remark3, SQLControl.EnumDataType.dtStringN)
                                .AddField("CreateDate", Twg_submissiondtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", Twg_submissiondtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", Twg_submissiondtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", Twg_submissiondtlCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("rowguid", Twg_submissiondtlCont.rowguid, SQLControl.EnumDataType.dtString)
                                .AddField("SyncCreate", Twg_submissiondtlCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", Twg_submissiondtlCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastSyncBy", Twg_submissiondtlCont.LastSyncBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "SubmissionID = '" & Twg_submissiondtlCont.SubmissionID & "' AND WasteCode = '" & Twg_submissiondtlCont.WasteCode & "' AND WasteDescription = '" & Twg_submissiondtlCont.WasteDescription & "' AND RecType = '" & Twg_submissiondtlCont.RecType & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("SubmissionID", Twg_submissiondtlCont.SubmissionID, SQLControl.EnumDataType.dtString)
                                                .AddField("WasteCode", Twg_submissiondtlCont.WasteCode, SQLControl.EnumDataType.dtString)
                                                .AddField("WasteDescription", Twg_submissiondtlCont.WasteDescription, SQLControl.EnumDataType.dtStringN)
                                                .AddField("RecType", Twg_submissiondtlCont.RecType, SQLControl.EnumDataType.dtNumeric)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "SubmissionID = '" & Twg_submissiondtlCont.SubmissionID & "' AND WasteCode = '" & Twg_submissiondtlCont.WasteCode & "' AND WasteDescription = '" & Twg_submissiondtlCont.WasteDescription & "' AND RecType = '" & Twg_submissiondtlCont.RecType & "'")
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
                'Throw axAssign
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Return False
            Finally
                Twg_submissiondtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal Twg_submissiondtlCont As Container.Twg_submissiondtl, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Twg_submissiondtlCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal Twg_submissiondtlCont As Container.Twg_submissiondtl, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Twg_submissiondtlCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function UpdateQty(ByVal ListContDTL As List(Of Actions.Container.Twg_submissiondtl), ByVal ListContConsign As List(Of Actions.Container.Consigndtl), ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return UpdateQtys(ListContDTL, ListContConsign, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal Twg_submissiondtlCont As Container.Twg_submissiondtl, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Twg_submissiondtlCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Twg_submissiondtlInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "SubmissionID = '" & Twg_submissiondtlCont.SubmissionID & "' AND WasteCode = '" & Twg_submissiondtlCont.WasteCode & "' AND WasteDescription = '" & Twg_submissiondtlCont.WasteDescription & "' AND RecType = '" & Twg_submissiondtlCont.RecType & "'")
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
                                strSQL = BuildUpdate(Twg_submissiondtlInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = '" & Twg_submissiondtlCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissiondtlCont.UpdateBy) & "' WHERE" & _
                                "SubmissionID = '" & Twg_submissiondtlCont.SubmissionID & "' AND WasteCode = '" & Twg_submissiondtlCont.WasteCode & "' AND WasteDescription = '" & Twg_submissiondtlCont.WasteDescription & "' AND RecType = '" & Twg_submissiondtlCont.RecType & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Twg_submissiondtlInfo.MyInfo.TableName, "SubmissionID = '" & Twg_submissiondtlCont.SubmissionID & "' AND WasteCode = '" & Twg_submissiondtlCont.WasteCode & "' AND WasteDescription = '" & Twg_submissiondtlCont.WasteDescription & "' AND RecType = '" & Twg_submissiondtlCont.RecType & "'")
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
                Twg_submissiondtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetTWG_SUBMISSIONDTL(ByVal SubmissionID As System.String, ByVal WasteCode As System.String, ByVal WasteDescription As System.String, ByVal RecType As System.Byte) As Container.Twg_submissiondtl
            Dim rTwg_submissiondtl As Container.Twg_submissiondtl = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With Twg_submissiondtlInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "SubmissionID = '" & SubmissionID & "' AND WasteCode = '" & WasteCode & "' AND WasteDescription = '" & WasteDescription & "' AND RecType = '" & RecType & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rTWG_SUBMISSIONDTL = New Container.TWG_SUBMISSIONDTL
                                rTWG_SUBMISSIONDTL.SubmissionID = drRow.Item("SubmissionID")
                                rTWG_SUBMISSIONDTL.WasteCode = drRow.Item("WasteCode")
                                rTWG_SUBMISSIONDTL.WasteDescription = drRow.Item("WasteDescription")
                                rTWG_SUBMISSIONDTL.RecType = drRow.Item("RecType")
                                rTwg_submissiondtl.Qty = drRow.Item("Qty")
                                rTwg_submissiondtl.ExpectedQty = drRow.Item("ExpectedQty")
                                rTwg_submissiondtl.ApprovedQty = drRow.Item("ApprovedQty")
                                rTwg_submissiondtl.HandlingQty1 = drRow.Item("HandlingQty1")
                                rTwg_submissiondtl.HandlingQty2 = drRow.Item("HandlingQty2")
                                rTwg_submissiondtl.HandlingQty3 = drRow.Item("HandlingQty3")
                                rTwg_submissiondtl.Remark1 = drRow.Item("Remark1")
                                rTwg_submissiondtl.Remark2 = drRow.Item("Remark2")
                                rTwg_submissiondtl.Remark3 = drRow.Item("Remark3")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rTwg_submissiondtl.CreateDate = drRow.Item("CreateDate")
                                End If
                                rTwg_submissiondtl.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rTwg_submissiondtl.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rTwg_submissiondtl.UpdateBy = drRow.Item("UpdateBy")
                                rTwg_submissiondtl.rowguid = drRow.Item("rowguid")
                                rTwg_submissiondtl.SyncCreate = drRow.Item("SyncCreate")
                                rTwg_submissiondtl.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rTwg_submissiondtl.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rTwg_submissiondtl = Nothing
                            End If
                        Else
                            rTwg_submissiondtl = Nothing
                        End If
                    End With
                End If
                Return rTwg_submissiondtl
            Catch ex As Exception
                Throw ex
            Finally
                rTwg_submissiondtl = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTWG_SUBMISSIONDTL(ByVal SubmissionID As System.String, ByVal WasteCode As System.String, ByVal WasteDescription As System.String, ByVal RecType As System.Byte, DecendingOrder As Boolean) As List(Of Container.Twg_submissiondtl)
            Dim rTwg_submissiondtl As Container.Twg_submissiondtl = Nothing
            Dim lstTwg_submissiondtl As List(Of Container.Twg_submissiondtl) = New List(Of Container.Twg_submissiondtl)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With Twg_submissiondtlInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by SubmissionID, WasteCode, WasteDescription, RecType DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "SubmissionID = '" & SubmissionID & "' AND WasteCode = '" & WasteCode & "' AND WasteDescription = '" & WasteDescription & "' AND RecType = '" & RecType & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rTWG_SUBMISSIONDTL = New Container.TWG_SUBMISSIONDTL
                                rTWG_SUBMISSIONDTL.SubmissionID = drRow.Item("SubmissionID")
                                rTWG_SUBMISSIONDTL.WasteCode = drRow.Item("WasteCode")
                                rTWG_SUBMISSIONDTL.WasteDescription = drRow.Item("WasteDescription")
                                rTWG_SUBMISSIONDTL.RecType = drRow.Item("RecType")
                                rTwg_submissiondtl.Qty = drRow.Item("Qty")
                                rTwg_submissiondtl.ExpectedQty = drRow.Item("ExpectedQty")
                                rTwg_submissiondtl.ApprovedQty = drRow.Item("ApprovedQty")
                                rTwg_submissiondtl.HandlingQty1 = drRow.Item("HandlingQty1")
                                rTwg_submissiondtl.HandlingQty2 = drRow.Item("HandlingQty2")
                                rTwg_submissiondtl.HandlingQty3 = drRow.Item("HandlingQty3")
                                rTwg_submissiondtl.Remark1 = drRow.Item("Remark1")
                                rTwg_submissiondtl.Remark2 = drRow.Item("Remark2")
                                rTwg_submissiondtl.Remark3 = drRow.Item("Remark3")
                                rTwg_submissiondtl.CreateBy = drRow.Item("CreateBy")
                                rTwg_submissiondtl.UpdateBy = drRow.Item("UpdateBy")
                                rTwg_submissiondtl.rowguid = drRow.Item("rowguid")
                                rTwg_submissiondtl.SyncCreate = drRow.Item("SyncCreate")
                                rTwg_submissiondtl.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rTwg_submissiondtl.LastSyncBy = drRow.Item("LastSyncBy")
                                lstTwg_submissiondtl.Add(rTwg_submissiondtl)
                            Next

                        Else
                            rTwg_submissiondtl = Nothing
                        End If
                        Return lstTwg_submissiondtl
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rTwg_submissiondtl = Nothing
                lstTwg_submissiondtl = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTWG_SUBMISSIONDTL(ByVal SubmissionID As System.String) As Container.Twg_submissiondtl
            Dim rTwg_submissiondtl As Container.Twg_submissiondtl = Nothing
            Dim lstTwg_submissiondtl As List(Of Container.Twg_submissiondtl) = New List(Of Container.Twg_submissiondtl)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With Twg_submissiondtlInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "SubmissionID = '" & SubmissionID & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rTwg_submissiondtl = New Container.Twg_submissiondtl
                                rTwg_submissiondtl.SubmissionID = drRow.Item("SubmissionID")
                                rTwg_submissiondtl.WasteCode = drRow.Item("WasteCode")
                                rTwg_submissiondtl.WasteDescription = drRow.Item("WasteDescription")
                                rTwg_submissiondtl.RecType = drRow.Item("RecType")
                                rTwg_submissiondtl.Qty = drRow.Item("Qty")
                                rTwg_submissiondtl.ExpectedQty = drRow.Item("ExpectedQty")
                                'rTwg_submissiondtl.ExpectedQty1 = drRow.Item("ExpectedQty1")
                                rTwg_submissiondtl.ApprovedQty = drRow.Item("ApprovedQty")
                                rTwg_submissiondtl.HandlingQty1 = drRow.Item("HandlingQty1")
                                rTwg_submissiondtl.HandlingQty2 = drRow.Item("HandlingQty2")
                                rTwg_submissiondtl.HandlingQty3 = drRow.Item("HandlingQty3")
                                rTwg_submissiondtl.Remark1 = drRow.Item("Remark1")
                                rTwg_submissiondtl.Remark2 = drRow.Item("Remark2")
                                rTwg_submissiondtl.Remark3 = drRow.Item("Remark3")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rTwg_submissiondtl.CreateDate = drRow.Item("CreateDate")
                                End If
                                rTwg_submissiondtl.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rTwg_submissiondtl.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rTwg_submissiondtl.UpdateBy = drRow.Item("UpdateBy")
                                rTwg_submissiondtl.rowguid = drRow.Item("rowguid")
                                rTwg_submissiondtl.SyncCreate = drRow.Item("SyncCreate")
                                rTwg_submissiondtl.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rTwg_submissiondtl.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rTwg_submissiondtl = Nothing
                            End If
                        Else
                            rTwg_submissiondtl = Nothing
                        End If
                    End With
                End If
                Return rTwg_submissiondtl
            Catch ex As Exception
                Throw ex
            Finally
                rTwg_submissiondtl = Nothing
                lstTwg_submissiondtl = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTWG_SUBMISSIONDTLList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissiondtlInfo.MyInfo
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

        Public Overloads Function GetTWG_SUBMISSIONDTLShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissiondtlInfo.MyInfo
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

        Public Overloads Function GetTWG_SUBMISSIONDTLBySubmissionID(ByVal SubmissionID As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissiondtlInfo.MyInfo
                    StartSQLControl()
                    strSQL = "select TW.ReceiverLocID, DT.WasteCode, DT.WasteType, TW.GeneratorLocID FROM TWG_SUBMISSIONHDR TW LEFT JOIN " & _
                        "TWG_SUBMISSIONDTL DT WITH (NOLOCK) ON TW.SubmissionID=DT.SubmissionID WHERE TW.SubmissionID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, SubmissionID) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetSumQty(ByVal SubmissionID As String, ByVal ReceiverID As String, ByVal WasteCode As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissiondtlInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT ISNULL(SUM(DT.ExpectedQty1),0) As SUM FROM TWG_SUBMISSIONHDR TW LEFT JOIN " & _
                        "TWG_SUBMISSIONDTL DT WITH (NOLOCK) ON TW.SubmissionID=DT.SubmissionID WHERE DT.SubmissionID <> '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, SubmissionID) & "' " & _
                        "AND TW.ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverID) & "' AND DT.WasteCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "'"
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
#Region "Twg_submissiondtl Container"
        Public Class Twg_submissiondtl_FieldName
            Public SubmissionID As System.String = "SubmissionID"
            Public WasteCode As System.String = "WasteCode"
            Public CodeSeq As System.String = "CodeSeq"
            Public WasteDescription As System.String = "WasteDescription"
            Public RecType As System.String = "RecType"
            Public Qty As System.String = "Qty"
            Public ExpectedQty As System.String = "ExpectedQty"
            Public ApprovedQty As System.String = "ApprovedQty"
            Public HandlingQty1 As System.String = "HandlingQty1"
            Public HandlingQty2 As System.String = "HandlingQty2"
            Public HandlingQty3 As System.String = "HandlingQty3"
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

        Public Class Twg_submissiondtl
            Protected _SubmissionID As System.String
            Protected _WasteCode As System.String
            Protected _WasteDescription As System.String
            Protected _RecType As System.Byte
            Private _Qty As System.Decimal
            Private _ExpectedQty As System.Decimal
            Private _ApprovedQty As System.Decimal
            Private _HandlingQty1 As System.Decimal
            Private _HandlingQty2 As System.Decimal
            Private _HandlingQty3 As System.Decimal
            Private _Remark1 As System.String
            Private _Remark2 As System.String
            Private _Remark3 As System.String
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _LastSyncBy As System.String
            Private _WasteType As System.String
            Private _CodeSeq As System.String
            Private _ExpectedQty1 As System.Decimal
            Private _ExpectedQty2 As System.Decimal
            Private _ExpSetDate As System.DateTime
            Private _ExpSetDate1 As System.DateTime
            Private _ExpSetDate2 As System.DateTime
            Private _NotifQty As System.Decimal
            Private _NewNotifQty As System.Decimal

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ExpSetDate1 As System.DateTime
                Get
                    Return _ExpSetDate1
                End Get
                Set(ByVal Value As System.DateTime)
                    _ExpSetDate1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ExpSetDate2 As System.DateTime
                Get
                    Return _ExpSetDate2
                End Get
                Set(ByVal Value As System.DateTime)
                    _ExpSetDate2 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ExpSetDate As System.DateTime
                Get
                    Return _ExpSetDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _ExpSetDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ExpectedQty1 As System.Decimal
                Get
                    Return _ExpectedQty1
                End Get
                Set(ByVal Value As System.Decimal)
                    _ExpectedQty1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ExpectedQty2 As System.Decimal
                Get
                    Return _ExpectedQty2
                End Get
                Set(ByVal Value As System.Decimal)
                    _ExpectedQty2 = Value
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

            Public Property CodeSeq As System.String
                Get
                    Return _CodeSeq
                End Get
                Set(ByVal Value As System.String)
                    _CodeSeq = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property SubmissionID As System.String
                Get
                    Return _SubmissionID
                End Get
                Set(ByVal Value As System.String)
                    _SubmissionID = Value
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
            Public Property WasteDescription As System.String
                Get
                    Return _WasteDescription
                End Get
                Set(ByVal Value As System.String)
                    _WasteDescription = Value
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
            Public Property ApprovedQty As System.Decimal
                Get
                    Return _ApprovedQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _ApprovedQty = Value
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
            Public Property LastSyncBy As System.String
                Get
                    Return _LastSyncBy
                End Get
                Set(ByVal Value As System.String)
                    _LastSyncBy = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property NotifQty As System.Decimal
                Get
                    Return _NotifQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _NotifQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property NewNotifQty As System.Decimal
                Get
                    Return _NewNotifQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _NewNotifQty = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace
#End Region

#Region "Class Info"
#Region "Twg_submissiondtl Info"
    Public Class Twg_submissiondtlInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "SubmissionID,WasteCode,WasteDescription,RecType,Qty,ExpectedQty,ApprovedQty,HandlingQty1,HandlingQty2,HandlingQty3,Remark1,Remark2,Remark3,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid,SyncCreate,SyncLastUpd,LastSyncBy"
                .CheckFields = "RecType"
                .TableName = "Twg_submissiondtl"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "SubmissionID,WasteCode,WasteDescription,RecType,Qty,ExpectedQty,ApprovedQty,HandlingQty1,HandlingQty2,HandlingQty3,Remark1,Remark2,Remark3,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid,SyncCreate,SyncLastUpd,LastSyncBy"
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
#Region "TWG_SUBMISSIONDTL Scheme"
    Public Class TWG_SUBMISSIONDTLScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "SubmissionID"
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
                .FieldName = "WasteDescription"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "RecType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Qty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ExpectedQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ApprovedQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "HandlingQty1"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "HandlingQty2"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "HandlingQty3"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark1"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark2"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark3"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)

        End Sub

        Public ReadOnly Property SubmissionID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property WasteCode As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property WasteDescription As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property RecType As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property

        Public ReadOnly Property Qty As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property ExpectedQty As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property ApprovedQty As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property HandlingQty1 As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property HandlingQty2 As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property HandlingQty3 As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property Remark1 As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property Remark2 As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property Remark3 As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace
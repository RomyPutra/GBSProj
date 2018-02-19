Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Actions
#Region "TWG_SUBMISSIONHDR Class"
    Public NotInheritable Class TWG_SUBMISSIONHDR
        Inherits Core.CoreControl
        Private Twg_submissionhdrInfo As Twg_submissionhdrInfo = New Twg_submissionhdrInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal Twg_submissionhdrCont As Container.Twg_submissionhdr, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If Twg_submissionhdrCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Twg_submissionhdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "SubmissionID = '" & Twg_submissionhdrCont.SubmissionID & "' AND ReceiverID = '" & Twg_submissionhdrCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_submissionhdrCont.ReceiverLocID & "' AND GeneratorID = '" & Twg_submissionhdrCont.GeneratorID & "' AND GeneratorLocID = '" & Twg_submissionhdrCont.GeneratorLocID & "'")
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
                                .TableName = "Twg_submissionhdr"
                                .AddField("SubmissionRemark", Twg_submissionhdrCont.SubmissionRemark, SQLControl.EnumDataType.dtStringN)
                                .AddField("RevisionRemark", Twg_submissionhdrCont.RevisionRemark, SQLControl.EnumDataType.dtStringN)
                                .AddField("DeclineRemark", Twg_submissionhdrCont.DeclineRemark, SQLControl.EnumDataType.dtStringN)
                                .AddField("ReasonRemark", Twg_submissionhdrCont.ReasonRemark, SQLControl.EnumDataType.dtStringN)
                                .AddField("RevSubmitRemark", Twg_submissionhdrCont.RevSubmitRemark, SQLControl.EnumDataType.dtStringN)
                                .AddField("SubStatus", Twg_submissionhdrCont.SubStatus, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ApprovalDate", Twg_submissionhdrCont.ApprovalDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Status", Twg_submissionhdrCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", Twg_submissionhdrCont.Flag, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", Twg_submissionhdrCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", Twg_submissionhdrCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", Twg_submissionhdrCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", Twg_submissionhdrCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("AckDate", Twg_submissionhdrCont.AckDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("StatusTWG", Twg_submissionhdrCont.StatusTWG, SQLControl.EnumDataType.dtNumeric)
                                '.AddField("rowguid", Twg_submissionhdrCont.rowguid,SQLControl.EnumDataType.dtString)
                                .AddField("SyncCreate", Twg_submissionhdrCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", Twg_submissionhdrCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastSyncBy", Twg_submissionhdrCont.LastSyncBy, SQLControl.EnumDataType.dtString)
                                .AddField("ApprovalBy", Twg_submissionhdrCont.ApprovalBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "SubmissionID = '" & Twg_submissionhdrCont.SubmissionID & "' AND ReceiverID = '" & Twg_submissionhdrCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_submissionhdrCont.ReceiverLocID & "' AND GeneratorID = '" & Twg_submissionhdrCont.GeneratorID & "' AND GeneratorLocID = '" & Twg_submissionhdrCont.GeneratorLocID & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("SubmissionID", Twg_submissionhdrCont.SubmissionID, SQLControl.EnumDataType.dtString)
                                                .AddField("ReceiverID", Twg_submissionhdrCont.ReceiverID, SQLControl.EnumDataType.dtString)
                                                .AddField("ReceiverLocID", Twg_submissionhdrCont.ReceiverLocID, SQLControl.EnumDataType.dtString)
                                                .AddField("GeneratorID", Twg_submissionhdrCont.GeneratorID, SQLControl.EnumDataType.dtString)
                                                .AddField("GeneratorLocID", Twg_submissionhdrCont.GeneratorLocID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "SubmissionID = '" & Twg_submissionhdrCont.SubmissionID & "' AND ReceiverID = '" & Twg_submissionhdrCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_submissionhdrCont.ReceiverLocID & "' AND GeneratorID = '" & Twg_submissionhdrCont.GeneratorID & "' AND GeneratorLocID = '" & Twg_submissionhdrCont.GeneratorLocID & "'")
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
                Twg_submissionhdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function UpdateIsNews(ByVal ListContDel As List(Of Actions.Container.Twg_submissionhdr), ByVal ListContConsign As List(Of Actions.Container.Consignhdr), ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            UpdateIsNews = False

            If ListContDel Is Nothing AndAlso ListContDel.Count > 0 Then
                'Message return
            Else
                StartSQLControl()

                For Each row As Actions.Container.Twg_submissionhdr In ListContDel
                    strSQL = "UPDATE TWG_SUBMISSIONHDR SET IsNew = 1" & _
                        " WHERE SubmissionID='" & row.SubmissionID & "'"
                    ListSQL.Add(strSQL)
                Next
            End If
            If ListContConsign Is Nothing AndAlso ListContConsign.Count > 0 Then
                'Message return
            Else
                StartSQLControl()

                For Each row As Actions.Container.Consignhdr In ListContConsign
                    strSQL = "UPDATE CONSIGnHDR SET IsNew = 1" & _
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

        Private Function DeleteTWG(ByVal ListContDel As List(Of Actions.Container.Twg_submissionhdr), ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            DeleteTWG = False

            If ListContDel Is Nothing AndAlso ListContDel.Count > 0 Then
                'Message return
            Else
                StartSQLControl()

                For Each row As Actions.Container.Twg_submissionhdr In ListContDel
                    strSQL = "UPDATE TWG_SUBMISSIONHDR SET Status='" & row.Status & "', UpdateBy='" & row.UpdateBy & "'," & _
                        " LastUpdate=getdate(), AckDate=getdate(), DeclineRemark='" & row.DeclineRemark & "'" & _
                        " WHERE SubmissionID='" & row.SubmissionID & "'"
                    ListSQL.Add(strSQL)
                Next

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
            End If
        End Function

        Private Function DeleteWG(ByVal Twg_submissionhdrList As Actions.Container.Twg_submissionhdr, ByVal Twg_submissionDTLList As Actions.Container.Twg_submissiondtl, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            DeleteWG = False
            Try
                If Twg_submissionhdrList Is Nothing Then
                    'Message return
                Else
                    StartSQLControl()
                    'For x As Integer = 0 To Twg_submissionhdrList.Count - 1
                    With Twg_submissionhdrInfo.MyInfo
                        strSQL = "SELECT a.ReceiverLocID, a.GeneratorLocID, a.Status, a.SubmissionID, b.WasteCode, b.WasteType " & _
                            "FROM TWG_SUBMISSIONHDR a WITH(NOLOCK) INNER JOIN TWG_SUBMISSIONDTL b ON a.SubmissionID=b.SubmissionID " & _
                            "WHERE a.SubmissionID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissionhdrList.SubmissionID) & "' "
                        '"AND GeneratorLocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissionhdrList.GeneratorLocID) & "' " & _
                        '"AND WasteCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissionDTLList.WasteCode) & "' " & _
                        '"AND WasteType='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissionDTLList.WasteType) & "'"
                    End With
                    rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

                    If rdr Is Nothing = False Then
                        With rdr
                            If .Read Then
                                strSQL = "Delete From TWG_SUBMISSIONHDR WHERE SUBMISSIONID='" & .Item("SubmissionID") & "'"
                                ListSQL.Add(strSQL)
                                strSQL = "Delete From TWG_SUBMISSIONDTL WHERE SUBMISSIONID='" & .Item("SubmissionID") & "'"
                                ListSQL.Add(strSQL)
                            End If
                            .Close()
                        End With
                    End If
                    'ListSQL.Add(strSQL)
                    'Next

                    Try
                        'execute
                        objConn.BatchExecute(ListSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Catch axExecute As Exception
                        Dim sqlStatement As String = " "
                        If objConn.FailedSQLStatement.Count > 0 Then
                            sqlStatement &= objConn.FailedSQLStatement.Item(0)
                        End If

                        Log.Notifier.Notify(axExecute)
                        Gibraltar.Agent.Log.Error("TWG_SUBMISSIONHDR", axExecute.Message & sqlStatement, axExecute.StackTrace)
                        Return False
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
                Twg_submissionhdrList = Nothing
                Twg_submissionDTLList = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function SaveBatchExecute(ByVal Twg_submissionhdrCont As Container.Twg_submissionhdr, ByVal Twg_submissiondtlCont As Container.Twg_submissiondtl, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            SaveBatchExecute = False
            Try
                If Twg_submissionhdrCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Twg_submissionhdrInfo.MyInfo
                            'strSQL = BuildSelect(.CheckFields, .TableName, "SubmissionID = '" & Twg_submissionhdrCont.SubmissionID & "'")
                            strSQL = "select * from twg_submissionhdr h, twg_submissiondtl d, CODEMASTER cm,bizentity bi" & _
                                    " where h.submissionid=d.submissionid and d.wastetype = cm.code and cm.codetype='WTY' and bi.bizregid=h.generatorid and h.status<>3 and YEAR(h.createdate)=YEAR(GETDATE()) " & _
                                    " and h.ReceiverLocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_submissionhdrCont.ReceiverLocID) & "' and wastecode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_submissiondtlCont.WasteCode) & "' and d.wastetype='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_submissiondtlCont.WasteType) & "' and h.generatorlocid='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_submissionhdrCont.GeneratorLocID) & "'"
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
                                .TableName = "Twg_submissionhdr"
                                .AddField("SubmissionRemark", Twg_submissionhdrCont.SubmissionRemark, SQLControl.EnumDataType.dtStringN)
                                .AddField("RevisionRemark", Twg_submissionhdrCont.RevisionRemark, SQLControl.EnumDataType.dtStringN)
                                .AddField("DeclineRemark", Twg_submissionhdrCont.DeclineRemark, SQLControl.EnumDataType.dtStringN)
                                .AddField("ReasonRemark", Twg_submissionhdrCont.ReasonRemark, SQLControl.EnumDataType.dtStringN)
                                .AddField("RevSubmitRemark", Twg_submissionhdrCont.RevSubmitRemark, SQLControl.EnumDataType.dtStringN)
                                .AddField("SubStatus", Twg_submissionhdrCont.SubStatus, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ApprovalDate", Twg_submissionhdrCont.ApprovalDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Status", Twg_submissionhdrCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", Twg_submissionhdrCont.Flag, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", Twg_submissionhdrCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", Twg_submissionhdrCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", Twg_submissionhdrCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", Twg_submissionhdrCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("AckDate", Twg_submissionhdrCont.AckDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("StatusTWG", Twg_submissionhdrCont.StatusTWG, SQLControl.EnumDataType.dtNumeric)
                                '.AddField("rowguid", Twg_submissionhdrCont.rowguid,SQLControl.EnumDataType.dtString)
                                .AddField("SyncCreate", Twg_submissionhdrCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", Twg_submissionhdrCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastSyncBy", Twg_submissionhdrCont.LastSyncBy, SQLControl.EnumDataType.dtString)
                                .AddField("ApprovalBy", Twg_submissionhdrCont.ApprovalBy, SQLControl.EnumDataType.dtString)
                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "SubmissionID = '" & Twg_submissionhdrCont.SubmissionID & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("SubmissionID", Twg_submissionhdrCont.SubmissionID, SQLControl.EnumDataType.dtString)
                                                .AddField("ReceiverID", Twg_submissionhdrCont.ReceiverID, SQLControl.EnumDataType.dtString)
                                                .AddField("ReceiverLocID", Twg_submissionhdrCont.ReceiverLocID, SQLControl.EnumDataType.dtString)
                                                .AddField("GeneratorID", Twg_submissionhdrCont.GeneratorID, SQLControl.EnumDataType.dtString)
                                                .AddField("GeneratorLocID", Twg_submissionhdrCont.GeneratorLocID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "SubmissionID = '" & Twg_submissionhdrCont.SubmissionID & "'")
                                End Select
                            End With
                            ListSQL.Add(strSQL)

                            If Twg_submissiondtlCont IsNot Nothing Then


                                With objSQL
                                    .TableName = "Twg_submissiondtl"
                                    .AddField("ExpectedQty", Twg_submissiondtlCont.ExpectedQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ExpectedQty1", Twg_submissiondtlCont.ExpectedQty1, SQLControl.EnumDataType.dtNumeric)
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
                                    '.AddField("rowguid", Twg_submissiondtlCont.rowguid, SQLControl.EnumDataType.dtString)
                                    .AddField("SyncCreate", Twg_submissiondtlCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("SyncLastUpd", Twg_submissiondtlCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("LastSyncBy", Twg_submissiondtlCont.LastSyncBy, SQLControl.EnumDataType.dtString)
                                    .AddField("WasteType", Twg_submissiondtlCont.WasteType, SQLControl.EnumDataType.dtString)
                                    .AddField("ExpectedQty1", Twg_submissiondtlCont.ExpectedQty1, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ExpectedQty2", Twg_submissiondtlCont.ExpectedQty2, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ExpSetDate", Twg_submissiondtlCont.ExpSetDate, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ExpSetDate1", Twg_submissiondtlCont.ExpSetDate1, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ExpSetDate2", Twg_submissiondtlCont.ExpSetDate2, SQLControl.EnumDataType.dtNumeric)
                                    Select Case pType
                                        Case SQLControl.EnumSQLType.stInsert
                                            If blnFound = True And blnFlag = False Then
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "SubmissionID = '" & Twg_submissiondtlCont.SubmissionID & "'")
                                            Else
                                                If blnFound = False Then
                                                    .AddField("SubmissionID", Twg_submissiondtlCont.SubmissionID, SQLControl.EnumDataType.dtString)
                                                    .AddField("WasteCode", Twg_submissiondtlCont.WasteCode, SQLControl.EnumDataType.dtString)
                                                    .AddField("WasteDescription", Twg_submissiondtlCont.WasteDescription, SQLControl.EnumDataType.dtStringN)
                                                    .AddField("RecType", Twg_submissiondtlCont.RecType, SQLControl.EnumDataType.dtNumeric)
                                                    .AddField("Qty", Twg_submissiondtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
                                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                                End If
                                            End If
                                        Case SQLControl.EnumSQLType.stUpdate
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "SubmissionID = '" & Twg_submissiondtlCont.SubmissionID & "'")
                                    End Select
                                End With
                                ListSQL.Add(strSQL)
                            End If
                            Try
                                'execute
                                objConn.BatchExecute(ListSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                    'Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                                Else
                                    message = axExecute.Message.ToString()
                                    'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                                End If

                                Dim sqlStatement As String = " "
                                If objConn.FailedSQLStatement.Count > 0 Then
                                    sqlStatement &= objConn.FailedSQLStatement.Item(0)
                                End If

                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("TWG_SUBMISSIONHDR", axExecute.Message & sqlStatement, axExecute.StackTrace)
                                Return False
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
                Twg_submissionhdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function SaveRequest(ByVal Twg_submissionhdrList As Actions.Container.Twg_submissionhdr, ByVal Twg_submissionDTLList As Actions.Container.Twg_submissiondtl, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            SaveRequest = False
            Try
                If Twg_submissionhdrList Is Nothing Then
                    'Message return
                Else
                    StartSQLControl()
                    'For x As Integer = 0 To Twg_submissionhdrList.Count - 1
                    With Twg_submissionhdrInfo.MyInfo
                        strSQL = "SELECT a.ReceiverLocID, a.GeneratorLocID, a.Status, a.SubmissionID, b.WasteCode, b.WasteType " & _
                            "FROM TWG_SUBMISSIONHDR a WITH(NOLOCK) INNER JOIN TWG_SUBMISSIONDTL b ON a.SubmissionID=b.SubmissionID " & _
                            "WHERE ReceiverLocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissionhdrList.ReceiverLocID) & "' " & _
                            "AND GeneratorLocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissionhdrList.GeneratorLocID) & "' " & _
                            "AND WasteCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissionDTLList.WasteCode) & "' " & _
                            "AND WasteType='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissionDTLList.WasteType) & "'"
                    End With
                    rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

                    If rdr Is Nothing = False Then
                        With rdr
                            If .Read Then
                                strSQL = "Delete From TWG_SUBMISSIONHDR WHERE SUBMISSIONID='" & .Item("SubmissionID") & "'"
                                ListSQL.Add(strSQL)
                                strSQL = "Delete From TWG_SUBMISSIONDTL WHERE SUBMISSIONID='" & .Item("SubmissionID") & "'"
                                ListSQL.Add(strSQL)
                            End If
                            .Close()
                        End With
                    End If

                    With Twg_submissionhdrInfo.MyInfo
                        strSQL = "SELECT Code, CodeType, CodeDesc FROM CODEMASTER WHERE CODETYPE='WTY' AND CodeSeq='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissionDTLList.CodeSeq) & "'"
                    End With
                    rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

                    If rdr Is Nothing = False Then
                        With rdr
                            If .Read Then
                                strSQL = "UPDATE NOTIFYDTL SET Qty=" & Twg_submissionDTLList.NewNotifQty & " WHERE CompanyID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissionhdrList.GeneratorID) & "'" & _
                                    " AND LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissionhdrList.GeneratorLocID) & "'" & _
                                    " AND ItemCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissionDTLList.WasteCode) & "'" & _
                                    " AND ItmName='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissionDTLList.WasteDescription) & "' AND TypeCode='" & .Item("Code") & "'"
                                ListSQL.Add(strSQL)
                            End If
                            .Close()
                        End With
                    End If

                    With objSQL
                        .TableName = "Twg_submissionhdr"
                        .AddField("SubmissionRemark", Twg_submissionhdrList.SubmissionRemark, SQLControl.EnumDataType.dtStringN)
                        .AddField("RevisionRemark", Twg_submissionhdrList.RevisionRemark, SQLControl.EnumDataType.dtStringN)
                        .AddField("DeclineRemark", Twg_submissionhdrList.DeclineRemark, SQLControl.EnumDataType.dtStringN)
                        .AddField("ReasonRemark", Twg_submissionhdrList.ReasonRemark, SQLControl.EnumDataType.dtStringN)
                        .AddField("RevSubmitRemark", Twg_submissionhdrList.RevSubmitRemark, SQLControl.EnumDataType.dtStringN)
                        .AddField("SubStatus", Twg_submissionhdrList.SubStatus, SQLControl.EnumDataType.dtNumeric)
                        .AddField("ApprovalDate", Twg_submissionhdrList.ApprovalDate, SQLControl.EnumDataType.dtDateTime)
                        .AddField("Status", Twg_submissionhdrList.Status, SQLControl.EnumDataType.dtNumeric)
                        .AddField("Flag", Twg_submissionhdrList.Flag, SQLControl.EnumDataType.dtNumeric)
                        .AddField("CreateDate", Twg_submissionhdrList.CreateDate, SQLControl.EnumDataType.dtDateTime)
                        .AddField("CreateBy", Twg_submissionhdrList.CreateBy, SQLControl.EnumDataType.dtString)
                        .AddField("LastUpdate", Twg_submissionhdrList.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                        .AddField("UpdateBy", Twg_submissionhdrList.UpdateBy, SQLControl.EnumDataType.dtString)
                        .AddField("AckDate", Twg_submissionhdrList.AckDate, SQLControl.EnumDataType.dtDateTime)
                        .AddField("StatusTWG", Twg_submissionhdrList.StatusTWG, SQLControl.EnumDataType.dtNumeric)
                        '.AddField("rowguid", Twg_submissionhdrCont.rowguid,SQLControl.EnumDataType.dtString)
                        .AddField("SyncCreate", Twg_submissionhdrList.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                        .AddField("SyncLastUpd", Twg_submissionhdrList.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                        .AddField("LastSyncBy", Twg_submissionhdrList.LastSyncBy, SQLControl.EnumDataType.dtString)
                        .AddField("ApprovalBy", Twg_submissionhdrList.ApprovalBy, SQLControl.EnumDataType.dtString)

                        .AddField("SubmissionID", Twg_submissionhdrList.SubmissionID, SQLControl.EnumDataType.dtString)
                        .AddField("ReceiverID", Twg_submissionhdrList.ReceiverID, SQLControl.EnumDataType.dtString)
                        .AddField("ReceiverLocID", Twg_submissionhdrList.ReceiverLocID, SQLControl.EnumDataType.dtString)
                        .AddField("GeneratorID", Twg_submissionhdrList.GeneratorID, SQLControl.EnumDataType.dtString)
                        .AddField("GeneratorLocID", Twg_submissionhdrList.GeneratorLocID, SQLControl.EnumDataType.dtString)
                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                    End With
                    ListSQL.Add(strSQL)

                    With objSQL
                        .TableName = "Twg_submissiondtl"
                        .AddField("ExpectedQty", Twg_submissionDTLList.ExpectedQty, SQLControl.EnumDataType.dtNumeric)
                        .AddField("ExpectedQty1", Twg_submissionDTLList.ExpectedQty1, SQLControl.EnumDataType.dtNumeric)
                        .AddField("ApprovedQty", Twg_submissionDTLList.ApprovedQty, SQLControl.EnumDataType.dtNumeric)
                        .AddField("HandlingQty1", Twg_submissionDTLList.HandlingQty1, SQLControl.EnumDataType.dtNumeric)
                        .AddField("HandlingQty2", Twg_submissionDTLList.HandlingQty2, SQLControl.EnumDataType.dtNumeric)
                        .AddField("HandlingQty3", Twg_submissionDTLList.HandlingQty3, SQLControl.EnumDataType.dtNumeric)
                        .AddField("Remark1", Twg_submissionDTLList.Remark1, SQLControl.EnumDataType.dtStringN)
                        .AddField("Remark2", Twg_submissionDTLList.Remark2, SQLControl.EnumDataType.dtStringN)
                        .AddField("Remark3", Twg_submissionDTLList.Remark3, SQLControl.EnumDataType.dtStringN)
                        .AddField("CreateDate", Twg_submissionDTLList.CreateDate, SQLControl.EnumDataType.dtDateTime)
                        .AddField("CreateBy", Twg_submissionDTLList.CreateBy, SQLControl.EnumDataType.dtString)
                        .AddField("LastUpdate", Twg_submissionDTLList.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                        .AddField("UpdateBy", Twg_submissionDTLList.UpdateBy, SQLControl.EnumDataType.dtString)
                        '.AddField("rowguid", Twg_submissiondtlCont.rowguid, SQLControl.EnumDataType.dtString)
                        .AddField("SyncCreate", Twg_submissionDTLList.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                        .AddField("SyncLastUpd", Twg_submissionDTLList.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                        .AddField("LastSyncBy", Twg_submissionDTLList.LastSyncBy, SQLControl.EnumDataType.dtString)
                        .AddField("WasteType", Twg_submissionDTLList.WasteType, SQLControl.EnumDataType.dtString)
                        .AddField("ExpectedQty1", Twg_submissionDTLList.ExpectedQty1, SQLControl.EnumDataType.dtNumeric)
                        .AddField("ExpectedQty2", Twg_submissionDTLList.ExpectedQty2, SQLControl.EnumDataType.dtNumeric)
                        .AddField("ExpSetDate", Twg_submissionDTLList.ExpSetDate, SQLControl.EnumDataType.dtNumeric)
                        .AddField("ExpSetDate1", Twg_submissionDTLList.ExpSetDate1, SQLControl.EnumDataType.dtNumeric)
                        .AddField("ExpSetDate2", Twg_submissionDTLList.ExpSetDate2, SQLControl.EnumDataType.dtNumeric)

                        .AddField("SubmissionID", Twg_submissionDTLList.SubmissionID, SQLControl.EnumDataType.dtString)
                        .AddField("WasteCode", Twg_submissionDTLList.WasteCode, SQLControl.EnumDataType.dtString)
                        .AddField("WasteDescription", Twg_submissionDTLList.WasteDescription, SQLControl.EnumDataType.dtStringN)
                        .AddField("RecType", Twg_submissionDTLList.RecType, SQLControl.EnumDataType.dtNumeric)
                        .AddField("Qty", Twg_submissionDTLList.Qty, SQLControl.EnumDataType.dtNumeric)
                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                    End With
                    ListSQL.Add(strSQL)
                    'Next

                    Try
                        'execute
                        objConn.BatchExecute(ListSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Catch axExecute As Exception
                        Dim sqlStatement As String = " "
                        If objConn.FailedSQLStatement.Count > 0 Then
                            sqlStatement &= objConn.FailedSQLStatement.Item(0)
                        End If

                        Log.Notifier.Notify(axExecute)
                        Gibraltar.Agent.Log.Error("TWG_SUBMISSIONHDR", axExecute.Message & sqlStatement, axExecute.StackTrace)
                        Return False
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
                Twg_submissionhdrList = Nothing
                Twg_submissionDTLList = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function SaveRequestList(ByVal ListContHDR As List(Of Actions.Container.Twg_submissionhdr), ByVal ListContDTL As List(Of Actions.Container.Twg_submissiondtl), ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            SaveRequestList = False
            Try
                If ListContHDR Is Nothing Then
                    'Message return
                Else
                    StartSQLControl()
                    'For x As Integer = 0 To Twg_submissionhdrList.Count - 1

                    For Each Twg_submissionhdrList As Actions.Container.Twg_submissionhdr In ListContHDR
                        With objSQL
                            .TableName = "Twg_submissionhdr"
                            .AddField("SubmissionRemark", Twg_submissionhdrList.SubmissionRemark, SQLControl.EnumDataType.dtStringN)
                            .AddField("RevisionRemark", Twg_submissionhdrList.RevisionRemark, SQLControl.EnumDataType.dtStringN)
                            .AddField("DeclineRemark", Twg_submissionhdrList.DeclineRemark, SQLControl.EnumDataType.dtStringN)
                            .AddField("ReasonRemark", Twg_submissionhdrList.ReasonRemark, SQLControl.EnumDataType.dtStringN)
                            .AddField("RevSubmitRemark", Twg_submissionhdrList.RevSubmitRemark, SQLControl.EnumDataType.dtStringN)
                            .AddField("SubStatus", Twg_submissionhdrList.SubStatus, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ApprovalDate", Twg_submissionhdrList.ApprovalDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("Status", Twg_submissionhdrList.Status, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Flag", Twg_submissionhdrList.Flag, SQLControl.EnumDataType.dtNumeric)
                            .AddField("CreateDate", Twg_submissionhdrList.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateBy", Twg_submissionhdrList.CreateBy, SQLControl.EnumDataType.dtString)
                            .AddField("LastUpdate", Twg_submissionhdrList.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("UpdateBy", Twg_submissionhdrList.UpdateBy, SQLControl.EnumDataType.dtString)
                            .AddField("AckDate", Twg_submissionhdrList.AckDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("StatusTWG", Twg_submissionhdrList.StatusTWG, SQLControl.EnumDataType.dtNumeric)
                            '.AddField("rowguid", Twg_submissionhdrCont.rowguid,SQLControl.EnumDataType.dtString)
                            .AddField("SyncCreate", Twg_submissionhdrList.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("SyncLastUpd", Twg_submissionhdrList.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                            .AddField("LastSyncBy", Twg_submissionhdrList.LastSyncBy, SQLControl.EnumDataType.dtString)
                            .AddField("ApprovalBy", Twg_submissionhdrList.ApprovalBy, SQLControl.EnumDataType.dtString)

                            .AddField("SubmissionID", Twg_submissionhdrList.SubmissionID, SQLControl.EnumDataType.dtString)
                            .AddField("ReceiverID", Twg_submissionhdrList.ReceiverID, SQLControl.EnumDataType.dtString)
                            .AddField("ReceiverLocID", Twg_submissionhdrList.ReceiverLocID, SQLControl.EnumDataType.dtString)
                            .AddField("GeneratorID", Twg_submissionhdrList.GeneratorID, SQLControl.EnumDataType.dtString)
                            .AddField("GeneratorLocID", Twg_submissionhdrList.GeneratorLocID, SQLControl.EnumDataType.dtString)
                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                        End With
                        ListSQL.Add(strSQL)
                    Next

                    For Each Twg_submissionDTLList As Actions.Container.Twg_submissiondtl In ListContDTL
                        With objSQL
                            .TableName = "Twg_submissiondtl"
                            .AddField("ExpectedQty", Twg_submissionDTLList.ExpectedQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ExpectedQty1", Twg_submissionDTLList.ExpectedQty1, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ApprovedQty", Twg_submissionDTLList.ApprovedQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("HandlingQty1", Twg_submissionDTLList.HandlingQty1, SQLControl.EnumDataType.dtNumeric)
                            .AddField("HandlingQty2", Twg_submissionDTLList.HandlingQty2, SQLControl.EnumDataType.dtNumeric)
                            .AddField("HandlingQty3", Twg_submissionDTLList.HandlingQty3, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Remark1", Twg_submissionDTLList.Remark1, SQLControl.EnumDataType.dtStringN)
                            .AddField("Remark2", Twg_submissionDTLList.Remark2, SQLControl.EnumDataType.dtStringN)
                            .AddField("Remark3", Twg_submissionDTLList.Remark3, SQLControl.EnumDataType.dtStringN)
                            .AddField("CreateDate", Twg_submissionDTLList.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateBy", Twg_submissionDTLList.CreateBy, SQLControl.EnumDataType.dtString)
                            .AddField("LastUpdate", Twg_submissionDTLList.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("UpdateBy", Twg_submissionDTLList.UpdateBy, SQLControl.EnumDataType.dtString)
                            '.AddField("rowguid", Twg_submissiondtlCont.rowguid, SQLControl.EnumDataType.dtString)
                            .AddField("SyncCreate", Twg_submissionDTLList.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("SyncLastUpd", Twg_submissionDTLList.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                            .AddField("LastSyncBy", Twg_submissionDTLList.LastSyncBy, SQLControl.EnumDataType.dtString)
                            .AddField("WasteType", Twg_submissionDTLList.WasteType, SQLControl.EnumDataType.dtString)
                            .AddField("ExpectedQty2", Twg_submissionDTLList.ExpectedQty2, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ExpSetDate", Twg_submissionDTLList.ExpSetDate, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ExpSetDate1", Twg_submissionDTLList.ExpSetDate1, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ExpSetDate2", Twg_submissionDTLList.ExpSetDate2, SQLControl.EnumDataType.dtNumeric)

                            .AddField("SubmissionID", Twg_submissionDTLList.SubmissionID, SQLControl.EnumDataType.dtString)
                            .AddField("WasteCode", Twg_submissionDTLList.WasteCode, SQLControl.EnumDataType.dtString)
                            .AddField("WasteDescription", Twg_submissionDTLList.WasteDescription, SQLControl.EnumDataType.dtStringN)
                            .AddField("RecType", Twg_submissionDTLList.RecType, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Qty", Twg_submissionDTLList.Qty, SQLControl.EnumDataType.dtNumeric)
                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                        End With
                        ListSQL.Add(strSQL)
                    Next

                    'Next


                End If
                Try
                    'execute
                    objConn.BatchExecute(ListSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                Catch axExecute As Exception
                    Dim sqlStatement As String = " "
                    If objConn.FailedSQLStatement.Count > 0 Then
                        sqlStatement &= objConn.FailedSQLStatement.Item(0)
                    End If

                    Log.Notifier.Notify(axExecute)
                    Gibraltar.Agent.Log.Error("TWG_SUBMISSIONHDR", axExecute.Message & sqlStatement, axExecute.StackTrace)
                    Return False
                Finally
                    objSQL.Dispose()
                End Try
                Return True
            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Return False
            Finally
                ListContHDR = Nothing
                ListContDTL = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function SaveApproval(ByVal Twg_submissionhdrCont As Container.Twg_submissionhdr, ByVal Twg_submissiondtlCont As Container.Twg_submissiondtl, ByVal Type As String, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim rdr As System.Data.SqlClient.SqlDataReader
            SaveApproval = False
            Dim ListSQL As ArrayList = New ArrayList()
            Try
                If Twg_submissionhdrCont Is Nothing Then
                    'Message return
                Else
                    StartSQLControl()
                    With objSQL
                        strSQL = "UPDATE TWG_SUBMISSIONHDR SET STATUSTWG='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_submissionhdrCont.StatusTWG) & "', " & _
                            "APPROVALDATE=getDate() " & _
                            ", APPROVALBY='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_submissionhdrCont.ApprovalBy) & "', "

                        If Twg_submissionhdrCont.SubStatus = "0" Then
                            strSQL &= "SubmissionRemark='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_submissionhdrCont.SubmissionRemark) & "' "
                        Else
                            If Twg_submissionhdrCont.StatusTWG = "2" Then
                                strSQL &= "Flag = " & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_submissionhdrCont.Flag) & ","
                            End If
                            strSQL &= "RevisionRemark='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_submissionhdrCont.RevisionRemark) & "' "
                        End If

                        strSQL &= "WHERE SubmissionID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_submissionhdrCont.SubmissionID) & "' " & _
                            "AND Status=3 AND SubStatus='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_submissionhdrCont.SubStatus) & "'"
                    End With
                    ListSQL.Add(strSQL)

                    If Twg_submissiondtlCont IsNot Nothing Then
                        With objSQL
                            strSQL = BuildUpdate("twg_submissiondtl", " SET " & _
                            " Qty=" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Twg_submissiondtlCont.Qty) & ", ExpectedQty1=" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Twg_submissiondtlCont.ExpectedQty1) & ", ExpectedQty=" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Twg_submissiondtlCont.ExpectedQty) & " , UpdateBy = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissiondtlCont.UpdateBy) & _
                            "', LastUpdate = getDate(), LastSyncBy = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissiondtlCont.UpdateBy) & "', Remark2 = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissiondtlCont.Remark2) & "' WHERE " & _
                            "SubmissionID = '" & Twg_submissiondtlCont.SubmissionID & "'")
                        End With
                        ListSQL.Add(strSQL)
                    End If
                    Try
                        objConn.BatchExecute(ListSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Catch axExecute As Exception
                        Log.Notifier.Notify(axExecute)
                        Gibraltar.Agent.Log.Error("TWG_SUBMISSIONHDR", axExecute.Message & strSQL, axExecute.StackTrace)
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
                Twg_submissionhdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function SubmitApproval(ByVal Twg_submissionhdrCont As Container.Twg_submissionhdr, ByVal SubmissionID As String, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim rdr As System.Data.SqlClient.SqlDataReader
            SubmitApproval = False
            Try
                If Twg_submissionhdrCont Is Nothing Then
                    'Message return
                Else
                    StartSQLControl()
                    With objSQL
                        strSQL = "UPDATE TWG_SUBMISSIONHDR SET STATUS='3', StatusTWG = " & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Twg_submissionhdrCont.StatusTWG) & ", Flag = 1," & _
                            "LASTUPDATE=GETDATE(), SubmissionRemark = '', " & _
                            "UPDATEBY='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Twg_submissionhdrCont.UpdateBy) & "' " & _
                            "WHERE SubmissionID IN (" & SubmissionID & ")"
                    End With
                    Try
                        objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Catch axExecute As Exception
                        Log.Notifier.Notify(axExecute)
                        Gibraltar.Agent.Log.Error("TWG_SUBMISSIONHDR", axExecute.Message & strSQL, axExecute.StackTrace)
                        Return False
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
                Twg_submissionhdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'SAVE APPROVAL
        Public Function SaveTWG(ByVal Twg_submissionhdrCont As Container.Twg_submissionhdr, ByVal Twg_submissiondtlCont As Container.Twg_submissiondtl, ByVal Type As String, ByRef message As String) As Boolean
            Return SaveApproval(Twg_submissionhdrCont, Twg_submissiondtlCont, Type, message)
        End Function

        'SAVE APPROVAL
        Public Function SubmitTWG(ByVal Twg_submissionhdrCont As Container.Twg_submissionhdr, ByVal SubmissionID As String, ByRef message As String) As Boolean
            Return SubmitApproval(Twg_submissionhdrCont, SubmissionID, message)
        End Function

        'ADD
        Public Function Insert(ByVal Twg_submissionhdrCont As Container.Twg_submissionhdr, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Twg_submissionhdrCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal Twg_submissionhdrCont As Container.Twg_submissionhdr, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Twg_submissionhdrCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function UpdateIsNew(ByVal ListContHDR As List(Of Actions.Container.Twg_submissionhdr), ByVal ListContConsign As List(Of Actions.Container.Consignhdr), ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return UpdateIsNews(ListContHDR, ListContConsign, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function DeleteSubmission(ByVal ListContDel As List(Of Actions.Container.Twg_submissionhdr), ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return DeleteTWG(ListContDel, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function DeleteWGSubmission(ByVal Twg_submissionhdrCont As Container.Twg_submissionhdr, ByVal Twg_submissiondtlCont As Container.Twg_submissiondtl, ByRef message As String) As Boolean
            Return DeleteWG(Twg_submissionhdrCont, Twg_submissiondtlCont, message)
        End Function

        'ADD BATCHEXECUTE
        Public Function InsertBatch(ByVal Twg_submissionhdrCont As Container.Twg_submissionhdr, ByVal Twg_submissiondtlCont As Container.Twg_submissiondtl, ByRef message As String) As Boolean
            Return SaveBatchExecute(Twg_submissionhdrCont, Twg_submissiondtlCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'ADD BATCHEXECUTE
        Public Function InsertRequest(ByVal Twg_submissionhdrList As Actions.Container.Twg_submissionhdr, ByVal Twg_submissionDTLList As Actions.Container.Twg_submissiondtl, ByRef message As String) As Boolean
            Return SaveRequest(Twg_submissionhdrList, Twg_submissionDTLList, message)
        End Function

        'ADD BATCHEXECUTE
        Public Function InsertRequestList(ByVal ListContSubHDR As List(Of Actions.Container.Twg_submissionhdr), ByVal ListContSubDTL As List(Of Actions.Container.Twg_submissiondtl), ByRef message As String) As Boolean
            Return SaveRequestList(ListContSubHDR, ListContSubDTL, message)
        End Function

        'AMEND
        Public Function UpdateBatch(ByVal Twg_submissionhdrCont As Container.Twg_submissionhdr, ByVal Twg_submissiondtlCont As Container.Twg_submissiondtl, ByRef message As String) As Boolean
            Return SaveBatchExecute(Twg_submissionhdrCont, Twg_submissiondtlCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal Twg_submissionhdrCont As Container.Twg_submissionhdr, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Twg_submissionhdrCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Twg_submissionhdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "SubmissionID = '" & Twg_submissionhdrCont.SubmissionID & "' AND ReceiverID = '" & Twg_submissionhdrCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_submissionhdrCont.ReceiverLocID & "' AND GeneratorID = '" & Twg_submissionhdrCont.GeneratorID & "' AND GeneratorLocID = '" & Twg_submissionhdrCont.GeneratorLocID & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 1 Then
                                        blnInUse = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        If blnFound = True And blnInUse = True Then
                            With objSQL
                                strSQL = BuildUpdate(Twg_submissionhdrInfo.MyInfo.TableName, " SET Status = 3" & _
                                " , LastUpdate = GETDATE() , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissionhdrCont.CreateBy) & "' WHERE " & _
                                "SubmissionID = '" & Twg_submissionhdrCont.SubmissionID & "' AND ReceiverID = '" & Twg_submissionhdrCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_submissionhdrCont.ReceiverLocID & "' AND GeneratorID = '" & Twg_submissionhdrCont.GeneratorID & "' AND GeneratorLocID = '" & Twg_submissionhdrCont.GeneratorLocID & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Twg_submissionhdrInfo.MyInfo.TableName, "SubmissionID = '" & Twg_submissionhdrCont.SubmissionID & "' AND ReceiverID = '" & Twg_submissionhdrCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_submissionhdrCont.ReceiverLocID & "' AND GeneratorID = '" & Twg_submissionhdrCont.GeneratorID & "' AND GeneratorLocID = '" & Twg_submissionhdrCont.GeneratorLocID & "'")
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
                Twg_submissionhdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function UpdateRequest(ByVal ListTwg_submissionhdrCont As List(Of Container.Twg_submissionhdr), ByVal ListTwg_submissiondtlCont As List(Of Container.Twg_submissiondtl), ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()

            blnFound = False
            blnInUse = False
            Try
                If ListTwg_submissionhdrCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        For Each Twg_submissionhdrCont In ListTwg_submissionhdrCont
                            With objSQL
                                strSQL = BuildUpdate(Twg_submissionhdrInfo.MyInfo.TableName, " SET Status = " & Twg_submissionhdrCont.Status & ", " & _
                                "ReasonRemark = '" & Twg_submissionhdrCont.ReasonRemark & "', RevSubmitRemark = '" & Twg_submissionhdrCont.RevSubmitRemark & "'," & _
                                "SubStatus = " & Twg_submissionhdrCont.SubStatus & ", StatusTWG = " & Twg_submissionhdrCont.StatusTWG & ", Flag = " & Twg_submissionhdrCont.Flag & _
                                " , LastUpdate = '" & Twg_submissionhdrCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissionhdrCont.UpdateBy) & "' WHERE " & _
                                "SubmissionID = '" & Twg_submissionhdrCont.SubmissionID & "'")
                            End With
                            ListSQL.Add(strSQL)
                        Next
                    End If
                End If

                If ListTwg_submissiondtlCont Is Nothing Then
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        For Each Twg_submissiondtlCont In ListTwg_submissiondtlCont
                            With objSQL
                                strSQL = BuildUpdate("twg_submissiondtl", " SET " & _
                                " Remark1 = '" & Twg_submissiondtlCont.Remark1 & "', Qty=" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Twg_submissiondtlCont.Qty) & ", UpdateBy = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissiondtlCont.UpdateBy) & _
                                "' WHERE " & _
                                "SubmissionID = '" & Twg_submissiondtlCont.SubmissionID & "'")
                            End With
                            ListSQL.Add(strSQL)
                        Next
                    End If
                End If

                Try
                    'execute                            
                    objConn.BatchExecute(ListSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Return True
                Catch exExecute As Exception
                    message = exExecute.Message.ToString()
                    Return False
                    'Throw New ApplicationException("210006 " & exExecute.Message.ToString())
                End Try

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Return False
                'Throw axDelete
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Return False
                'Throw exDelete
            Finally
                ListTwg_submissionhdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function


        Public Function Resubmit(ByVal Twg_submissionhdrCont As Container.Twg_submissionhdr, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Resubmit = False
            blnFound = False
            blnInUse = False
            Try
                If Twg_submissionhdrCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Twg_submissionhdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "SubmissionID = '" & Twg_submissionhdrCont.SubmissionID & "' AND ReceiverID = '" & Twg_submissionhdrCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_submissionhdrCont.ReceiverLocID & "' AND GeneratorID = '" & Twg_submissionhdrCont.GeneratorID & "' AND GeneratorLocID = '" & Twg_submissionhdrCont.GeneratorLocID & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 1 Then
                                        blnInUse = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        If blnFound = True And blnInUse = True Then
                            With objSQL
                                strSQL = BuildUpdate(Twg_submissionhdrInfo.MyInfo.TableName, " SET Status = 0" & _
                                " , LastUpdate = GETDATE() , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissionhdrCont.CreateBy) & "' WHERE " & _
                                "SubmissionID = '" & Twg_submissionhdrCont.SubmissionID & "' AND ReceiverID = '" & Twg_submissionhdrCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_submissionhdrCont.ReceiverLocID & "' AND GeneratorID = '" & Twg_submissionhdrCont.GeneratorID & "' AND GeneratorLocID = '" & Twg_submissionhdrCont.GeneratorLocID & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Twg_submissionhdrInfo.MyInfo.TableName, "SubmissionID = '" & Twg_submissionhdrCont.SubmissionID & "' AND ReceiverID = '" & Twg_submissionhdrCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_submissionhdrCont.ReceiverLocID & "' AND GeneratorID = '" & Twg_submissionhdrCont.GeneratorID & "' AND GeneratorLocID = '" & Twg_submissionhdrCont.GeneratorLocID & "'")
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
                Twg_submissionhdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function PostSubmit(ByVal Twg_submissionhdrCont As Container.Twg_submissionhdr, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            PostSubmit = False
            blnFound = False
            blnInUse = False
            Try
                If Twg_submissionhdrCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()

                        With objSQL
                            strSQL = BuildUpdate(Twg_submissionhdrInfo.MyInfo.TableName, " SET Status = 4" & _
                            " , LastUpdate = GETDATE() , UpdateBy = '" & _
                            objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_submissionhdrCont.CreateBy) & "' WHERE " & _
                            " ReceiverID = '" & Twg_submissionhdrCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_submissionhdrCont.ReceiverLocID & "' AND YEAR(CreateDate)=YEAR(GETDATE()) AND Status='0'")
                        End With

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
                Twg_submissionhdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region

#Region "Data Selection"
        Public Overloads Function GetTWG_SUBMISSIONHDR(ByVal SubmissionID As System.String, ByVal ReceiverID As System.String, ByVal ReceiverLocID As System.String, ByVal GeneratorID As System.String, ByVal GeneratorLocID As System.String) As Container.Twg_submissionhdr
            Dim rTwg_submissionhdr As Container.Twg_submissionhdr = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With Twg_submissionhdrInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "SubmissionID = '" & SubmissionID & "' AND ReceiverID = '" & ReceiverID & "' AND ReceiverLocID = '" & ReceiverLocID & "' AND GeneratorID = '" & GeneratorID & "' AND GeneratorLocID = '" & GeneratorLocID & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rTwg_submissionhdr = New Container.Twg_submissionhdr
                                rTwg_submissionhdr.SubmissionID = drRow.Item("SubmissionID")
                                rTwg_submissionhdr.ReceiverID = drRow.Item("ReceiverID")
                                rTwg_submissionhdr.ReceiverLocID = drRow.Item("ReceiverLocID")
                                rTwg_submissionhdr.GeneratorID = drRow.Item("GeneratorID")
                                rTwg_submissionhdr.GeneratorLocID = drRow.Item("GeneratorLocID")
                                rTwg_submissionhdr.SubmissionRemark = drRow.Item("SubmissionRemark")
                                rTwg_submissionhdr.RevisionRemark = drRow.Item("RevisionRemark")
                                rTwg_submissionhdr.DeclineRemark = drRow.Item("DeclineRemark")
                                rTwg_submissionhdr.ReasonRemark = drRow.Item("ReasonRemark")
                                rTwg_submissionhdr.RevSubmitRemark = drRow.Item("RevSubmitRemark")
                                rTwg_submissionhdr.SubStatus = drRow.Item("SubStatus")
                                If Not IsDBNull(drRow.Item("ApprovalDate")) Then
                                    rTwg_submissionhdr.ApprovalDate = drRow.Item("ApprovalDate")
                                End If
                                rTwg_submissionhdr.Status = drRow.Item("Status")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rTwg_submissionhdr.CreateDate = drRow.Item("CreateDate")
                                End If
                                rTwg_submissionhdr.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rTwg_submissionhdr.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rTwg_submissionhdr.UpdateBy = drRow.Item("UpdateBy")
                                If Not IsDBNull(drRow.Item("AckDate")) Then
                                    rTwg_submissionhdr.AckDate = drRow.Item("AckDate")
                                End If
                                rTwg_submissionhdr.StatusTWG = drRow.Item("StatusTWG")
                                rTwg_submissionhdr.rowguid = drRow.Item("rowguid")
                                rTwg_submissionhdr.SyncCreate = drRow.Item("SyncCreate")
                                rTwg_submissionhdr.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rTwg_submissionhdr.LastSyncBy = drRow.Item("LastSyncBy")
                                rTwg_submissionhdr.ApprovalBy = drRow.Item("ApprovalBy")
                            Else
                                rTwg_submissionhdr = Nothing
                            End If
                        Else
                            rTwg_submissionhdr = Nothing
                        End If
                    End With
                End If
                Return rTwg_submissionhdr
            Catch ex As Exception
                Throw ex
            Finally
                rTwg_submissionhdr = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWGList(Optional ByVal DOEFileNo As String = "", Optional ByVal BranchName As String = "") As Data.DataTable
            Try
                If StartConnection() = True Then
                    With Twg_submissionhdrInfo.MyInfo
                        StartSQLControl()
                        strSQL = "Select BIZLOCATE.BranchName, BIZLOCATE.BizRegID, BIZLOCATE.BizLocID, BIZLOCATE.ContactEmail from BIZLOCATE INNER JOIN BIZENTITY ON BIZLOCATE.BIZREGID=BIZENTITY.BIZREGID WHERE CompanyType IN(2,5,6,9) AND BIZLOCATE.ACTIVE=1 AND BIZLOCATE.FLAG=1"
                        If BranchName <> "" Then
                            strSQL &= " AND BIZLOCATE.BranchName like '%" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, BranchName) & "%'"
                            strSQL &= " AND BIZLOCATE.AccNo='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, DOEFileNo) & "'"
                        End If
                        Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    End With
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", ex.Message & " " & strSQL, ex.StackTrace)
                'Throw ex
            Finally
                EndSQLControl()
                EndConnection()
            End Try
            Return Nothing
        End Function

        Public Overloads Function GetSubmissionID(ByVal GeneratorLocID As String, ByVal WasteCode As String, ByVal ReceiverLocID As String) As Data.DataTable
            Try
                If StartConnection() = True Then
                    With Twg_submissionhdrInfo.MyInfo
                        StartSQLControl()
                        strSQL = " SELECT HDR.SubmissionID FROM TWG_SUBMISSIONHDR HDR INNER JOIN TWG_SUBMISSIONDTL DTL ON HDR.SubmissionID = DTL.SubmissionID " & _
                            "WHERE HDR.GeneratorLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorLocID) & "' AND " & _
                            "DTL.WasteCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "' And HDR.IsNew = 0 AND ((HDR.StatusTWG IN (1,0) AND " & _
                            "HDR.SubStatus= 0) OR (HDR.StatusTWG IN (1,0,2) AND HDR.SubStatus= 1)) AND HDR.ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "'"
                        Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    End With
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", ex.Message & " " & strSQL, ex.StackTrace)
                'Throw ex
            Finally
                EndSQLControl()
                EndConnection()
            End Try
            Return Nothing
        End Function

        Public Overloads Function GetTWG_SUBMISSIONHDRBySubmission(ByVal SubmissionID As System.String) As Container.Twg_submissionhdr
            Dim rTwg_submissionhdr As Container.Twg_submissionhdr = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With Twg_submissionhdrInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "SubmissionID = '" & SubmissionID & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rTwg_submissionhdr = New Container.Twg_submissionhdr
                                rTwg_submissionhdr.SubmissionID = drRow.Item("SubmissionID")
                                rTwg_submissionhdr.ReceiverID = drRow.Item("ReceiverID")
                                rTwg_submissionhdr.ReceiverLocID = drRow.Item("ReceiverLocID")
                                rTwg_submissionhdr.GeneratorID = drRow.Item("GeneratorID")
                                rTwg_submissionhdr.GeneratorLocID = drRow.Item("GeneratorLocID")
                                rTwg_submissionhdr.SubmissionRemark = drRow.Item("SubmissionRemark")
                                rTwg_submissionhdr.RevisionRemark = drRow.Item("RevisionRemark")
                                rTwg_submissionhdr.DeclineRemark = drRow.Item("DeclineRemark")
                                rTwg_submissionhdr.ReasonRemark = drRow.Item("ReasonRemark")
                                rTwg_submissionhdr.RevSubmitRemark = drRow.Item("RevSubmitRemark")
                                rTwg_submissionhdr.Flag = drRow.Item("Flag")
                                rTwg_submissionhdr.SubStatus = drRow.Item("SubStatus")
                                If Not IsDBNull(drRow.Item("ApprovalDate")) Then
                                    rTwg_submissionhdr.ApprovalDate = drRow.Item("ApprovalDate")
                                End If
                                rTwg_submissionhdr.Status = drRow.Item("Status")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rTwg_submissionhdr.CreateDate = drRow.Item("CreateDate")
                                End If
                                rTwg_submissionhdr.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rTwg_submissionhdr.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rTwg_submissionhdr.UpdateBy = drRow.Item("UpdateBy")
                                If Not IsDBNull(drRow.Item("AckDate")) Then
                                    rTwg_submissionhdr.AckDate = drRow.Item("AckDate")
                                End If
                                rTwg_submissionhdr.StatusTWG = drRow.Item("StatusTWG")
                                rTwg_submissionhdr.rowguid = drRow.Item("rowguid")
                                rTwg_submissionhdr.SyncCreate = drRow.Item("SyncCreate")
                                rTwg_submissionhdr.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rTwg_submissionhdr.LastSyncBy = drRow.Item("LastSyncBy")
                                rTwg_submissionhdr.ApprovalBy = drRow.Item("ApprovalBy")
                            Else
                                rTwg_submissionhdr = Nothing
                            End If
                        Else
                            rTwg_submissionhdr = Nothing
                        End If
                    End With
                End If
                Return rTwg_submissionhdr
            Catch ex As Exception
                Throw ex
            Finally
                rTwg_submissionhdr = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTWG_SUBMISSIONHDRByReceiverLocID(ByVal ReceiverLocID As System.String) As Container.Twg_submissionhdr
            Dim rTwg_submissionhdr As Container.Twg_submissionhdr = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With Twg_submissionhdrInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "ReceiverLocID = '" & ReceiverLocID & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rTwg_submissionhdr = New Container.Twg_submissionhdr
                                rTwg_submissionhdr.SubmissionID = drRow.Item("SubmissionID")
                                rTwg_submissionhdr.ReceiverID = drRow.Item("ReceiverID")
                                rTwg_submissionhdr.ReceiverLocID = drRow.Item("ReceiverLocID")
                                rTwg_submissionhdr.GeneratorID = drRow.Item("GeneratorID")
                                rTwg_submissionhdr.GeneratorLocID = drRow.Item("GeneratorLocID")
                                rTwg_submissionhdr.SubmissionRemark = drRow.Item("SubmissionRemark")
                                rTwg_submissionhdr.RevisionRemark = drRow.Item("RevisionRemark")
                                rTwg_submissionhdr.DeclineRemark = drRow.Item("DeclineRemark")
                                rTwg_submissionhdr.ReasonRemark = drRow.Item("ReasonRemark")
                                rTwg_submissionhdr.RevSubmitRemark = drRow.Item("RevSubmitRemark")
                                rTwg_submissionhdr.SubStatus = drRow.Item("SubStatus")
                                If Not IsDBNull(drRow.Item("ApprovalDate")) Then
                                    rTwg_submissionhdr.ApprovalDate = drRow.Item("ApprovalDate")
                                End If
                                rTwg_submissionhdr.Status = drRow.Item("Status")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rTwg_submissionhdr.CreateDate = drRow.Item("CreateDate")
                                End If
                                rTwg_submissionhdr.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rTwg_submissionhdr.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rTwg_submissionhdr.UpdateBy = drRow.Item("UpdateBy")
                                If Not IsDBNull(drRow.Item("AckDate")) Then
                                    rTwg_submissionhdr.AckDate = drRow.Item("AckDate")
                                End If
                                rTwg_submissionhdr.StatusTWG = drRow.Item("StatusTWG")
                                rTwg_submissionhdr.rowguid = drRow.Item("rowguid")
                                rTwg_submissionhdr.SyncCreate = drRow.Item("SyncCreate")
                                rTwg_submissionhdr.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rTwg_submissionhdr.LastSyncBy = drRow.Item("LastSyncBy")
                                rTwg_submissionhdr.ApprovalBy = drRow.Item("ApprovalBy")
                            Else
                                rTwg_submissionhdr = Nothing
                            End If
                        Else
                            rTwg_submissionhdr = Nothing
                        End If
                    End With
                End If
                Return rTwg_submissionhdr
            Catch ex As Exception
                Throw ex
            Finally
                rTwg_submissionhdr = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTWG_SUBMISSIONHDR(ByVal SubmissionID As System.String, ByVal ReceiverID As System.String, ByVal ReceiverLocID As System.String, ByVal GeneratorID As System.String, ByVal GeneratorLocID As System.String, DecendingOrder As Boolean) As List(Of Container.Twg_submissionhdr)
            Dim rTwg_submissionhdr As Container.Twg_submissionhdr = Nothing
            Dim lstTwg_submissionhdr As List(Of Container.Twg_submissionhdr) = New List(Of Container.Twg_submissionhdr)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With Twg_submissionhdrInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by SubmissionID, ReceiverID, ReceiverLocID, GeneratorID, GeneratorLocID DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "SubmissionID = '" & SubmissionID & "' AND ReceiverID = '" & ReceiverID & "' AND ReceiverLocID = '" & ReceiverLocID & "' AND GeneratorID = '" & GeneratorID & "' AND GeneratorLocID = '" & GeneratorLocID & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rTwg_submissionhdr = New Container.Twg_submissionhdr
                                rTwg_submissionhdr.SubmissionID = drRow.Item("SubmissionID")
                                rTwg_submissionhdr.ReceiverID = drRow.Item("ReceiverID")
                                rTwg_submissionhdr.ReceiverLocID = drRow.Item("ReceiverLocID")
                                rTwg_submissionhdr.GeneratorID = drRow.Item("GeneratorID")
                                rTwg_submissionhdr.GeneratorLocID = drRow.Item("GeneratorLocID")
                                rTwg_submissionhdr.SubmissionRemark = drRow.Item("SubmissionRemark")
                                rTwg_submissionhdr.RevisionRemark = drRow.Item("RevisionRemark")
                                rTwg_submissionhdr.DeclineRemark = drRow.Item("DeclineRemark")
                                rTwg_submissionhdr.ReasonRemark = drRow.Item("ReasonRemark")
                                rTwg_submissionhdr.RevSubmitRemark = drRow.Item("RevSubmitRemark")
                                rTwg_submissionhdr.SubStatus = drRow.Item("SubStatus")
                                If Not IsDBNull(drRow.Item("ApprovalDate")) Then
                                    rTwg_submissionhdr.ApprovalDate = drRow.Item("ApprovalDate")
                                End If
                                rTwg_submissionhdr.Status = drRow.Item("Status")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rTwg_submissionhdr.CreateDate = drRow.Item("CreateDate")
                                End If
                                rTwg_submissionhdr.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rTwg_submissionhdr.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rTwg_submissionhdr.UpdateBy = drRow.Item("UpdateBy")
                                If Not IsDBNull(drRow.Item("AckDate")) Then
                                    rTwg_submissionhdr.AckDate = drRow.Item("AckDate")
                                End If
                                rTwg_submissionhdr.StatusTWG = drRow.Item("StatusTWG")
                                rTwg_submissionhdr.rowguid = drRow.Item("rowguid")
                                rTwg_submissionhdr.SyncCreate = drRow.Item("SyncCreate")
                                rTwg_submissionhdr.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rTwg_submissionhdr.LastSyncBy = drRow.Item("LastSyncBy")
                                rTwg_submissionhdr.ApprovalBy = drRow.Item("ApprovalBy")
                                lstTwg_submissionhdr.Add(rTwg_submissionhdr)
                            Next

                        Else
                            rTwg_submissionhdr = Nothing
                        End If
                        Return lstTwg_submissionhdr
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rTwg_submissionhdr = Nothing
                lstTwg_submissionhdr = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTWG_SUBMISSIONHDRList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
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

        Public Overloads Function GetTWG_SUBMISSIONHDRShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
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

        Public Overloads Function GetPremiseSubmission(ByVal ReceiverLocID As String, Optional FieldCond As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()

                    strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY B.BRANCHNAME ASC) AS Bil,b.bizregid, YEAR(TW.createdate) AS YEAR, TW.ReceiverID, TW.ReceiverLocID, DT.WasteCode, DT.WasteDescription , TW.SubmissionID, B.BranchName, CONVERT(DECIMAL(12,4),DT.ExpectedQty) AS  ExpectedQty_estimateYear ,CONVERT(DECIMAL(12,4),DT.ExpectedQty1) AS ExpectedQty1_averageMonth ,CONVERT(DECIMAL(12,4),DT.ExpectedQty2) AS ExpectedQty2_ApproveYear,  " & _
                    "CM.CodeDesc, TW.CreateDate, CASE WHEN TW.Status=1 THEN 'Accepted' WHEN TW.Status=0 THEN 'Pending' WHEN TW.Status=3 THEN 'Submited' Else 'Decline' END AS Status, TW.Status AS StatusNo, TW.StatusTWG, TW.SubStatus,0 AS DeleteStatus," & _
                    "( SELECT isnull(SUM(RCVQTY),0) FROM CONSIGNDTL,CONSIGNHDR WHERE CONSIGNHDR.TransID =CONSIGNDTL .TransID AND GeneratorLocID=TW.GeneratorLocID AND wastecode=DT.WasteCode and WasteType=DT.WasteType  and year(TransDate)= year(getdate()) and CONSIGNHDR.status=8  AND ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "'   )as YearSumQty,   " & _
                    "( SELECT isnull(SUM(RCVQTY),0) FROM CONSIGNDTL,CONSIGNHDR WHERE CONSIGNHDR.TransID =CONSIGNDTL .TransID AND GeneratorLocID=TW.GeneratorLocID AND wastecode=DT.WasteCode and WasteType=DT.WasteType  and month(TransDate)= month(getdate()) and CONSIGNHDR.status=8  AND ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "'   )as MonthSumQty,   " & _
                    "CONVERT(DECIMAL(12,2),( SELECT isnull(SUM(RCVQTY),0) FROM CONSIGNDTL,CONSIGNHDR WHERE CONSIGNHDR.TransID =CONSIGNDTL .TransID AND GeneratorLocID=TW.GeneratorLocID AND wastecode=DT.WasteCode and WasteType=DT.WasteType  and year(TransDate)= year(getdate()) and CONSIGNHDR.status=8  AND ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "'   )-DT.ExpectedQty) AS VarianceQty,  '' as Qty, '' as Remark, " & _
                    "CASE WHEN TW.Status = 0 THEN 'Pending Acceptance' WHEN TW.Status = 1 THEN 'Accepted' WHEN TW.Status = 3 AND TW.StatusTWG = 0 AND TW.SubStatus = 0 THEN 'Submission Submitted' WHEN TW.Status = 3 AND TW.StatusTWG = 1 AND TW.SubStatus = 0 THEN 'Submission Approved' WHEN TW.Status = 3 AND TW.StatusTWG = 2 AND TW.SubStatus = 0 THEN 'Submission Rejected' WHEN TW.Status = 3 AND TW.StatusTWG = 0 AND TW.SubStatus = 1 THEN 'Revision Submitted' WHEN TW.Status = 3 " & _
                    "AND TW.StatusTWG = 1 AND TW.SubStatus = 1 THEN 'Revision Approved' WHEN TW.Status = 3 AND TW.StatusTWG = 2 AND TW.SubStatus = 1 THEN 'Revision Rejected' END As StatusSubmmission, " & _
                    "CASE WHEN TW.Status=1 THEN 1 WHEN TW.Status=3 AND TW.StatusTWG=2 AND TW.SubStatus=0 THEN 1 ELSE 0 END AS Remove, TW.RevisionRemark, TW.SubmissionRemark  " & _
                    "FROM TWG_SUBMISSIONHDR TW WITH (NOLOCK)  " & _
                    "LEFT JOIN TWG_SUBMISSIONDTL DT WITH (NOLOCK)  ON TW.SubmissionID=DT.SubmissionID  " & _
                    "LEFT JOIN CONSIGNHDR C WITH (NOLOCK) ON TW.GENERATORID=C.GENERATORID AND TW.RECEIVERID=C.RECEIVERID  " & _
                    "LEFT  JOIN CONSIGNDTL D ON C.TransID=D.TransID AND DT.WasteCode=D.WasteCode and c.status=8 and D.wastetype = DT.WasteType  " & _
                    "LEFT JOIN BIZLOCATE B WITH (NOLOCK)  ON TW.GeneratorID=B.BizRegID LEFT JOIN CODEMASTER CM WITH(NOLOCK) ON DT.WasteType=CM.Code AND CM.CodeType='WTY'  " & _
                    "WHERE TW.ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "' AND YEAR(TW.createdate)=YEAR(GETDATE()) AND ((TW.Flag = 0 AND TW.StatusTWG=0 AND TW.SubStatus = 1) OR (TW.Flag = 1 AND TW.StatusTWG=1 AND TW.SubStatus = 1) OR (TW.Flag = 1 AND TW.StatusTWG=0 AND TW.SubStatus = 0) OR (TW.Flag = 1 AND TW.StatusTWG=1 AND TW.SubStatus = 0) OR (TW.Flag = 1 AND TW.StatusTWG=0 AND TW.SubStatus = 1) OR (TW.Flag = 1 AND TW.StatusTWG=2 AND TW.SubStatus = 1) OR (TW.Flag = 1 AND TW.StatusTWG=2 AND TW.SubStatus = 0)) "

                    If FieldCond IsNot Nothing AndAlso FieldCond <> "" Then
                        strSQL &= FieldCond
                    End If

                    strSQL &= " GROUP BY YEAR(TW.createdate), b.bizregid,  DT.WasteCode, DT.WasteDescription, DT.WasteType, DT.Qty,  DT.ExpectedQty, TW.SubmissionID, B.BranchName, CM.CodeDesc,TW.CreateDate,DT.ExpectedQty1, DT.ExpectedQty2, TW.Status, TW.StatusTWG, TW.SubStatus, TW.GeneratorLocID, TW.RevisionRemark, TW.SubmissionRemark, TW.ReceiverID, TW.ReceiverLocID order by bil "

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetPremisePrint(ByVal ReceiverLocID As String, Optional FieldCond As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()

                    strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY B.BRANCHNAME ASC) AS Bil,b.bizregid,b.bizlocid, YEAR(TW.createdate) AS YEAR, TW.ReceiverID, TW.ReceiverLocID, DT.WasteCode, DT.WasteDescription , TW.SubmissionID, B.BranchName, CONVERT(DECIMAL(12,4),DT.ExpectedQty) AS  ExpectedQty_estimateYear ,CONVERT(DECIMAL(12,4),DT.ExpectedQty1) AS ExpectedQty1_averageMonth ,CONVERT(DECIMAL(12,4),DT.ExpectedQty2) AS ExpectedQty2_ApproveYear,  " & _
                    "CM.CodeDesc, TW.CreateDate, CASE WHEN TW.Status=1 THEN 'Accepted' WHEN TW.Status=0 THEN 'Pending' WHEN TW.Status=3 THEN 'Submited' Else 'Decline' END AS Status, TW.Status AS StatusNo, TW.StatusTWG, TW.SubStatus,0 AS DeleteStatus," & _
                    "( SELECT isnull(SUM(RCVQTY),0) FROM CONSIGNDTL,CONSIGNHDR WHERE CONSIGNHDR.TransID =CONSIGNDTL .TransID AND GeneratorLocID=TW.GeneratorLocID AND wastecode=DT.WasteCode and WasteType=DT.WasteType  and year(TransDate)= year(getdate()) and CONSIGNHDR.status=8  AND ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "'   )as YearSumQty,   " & _
                    "( SELECT isnull(SUM(RCVQTY),0) FROM CONSIGNDTL,CONSIGNHDR WHERE CONSIGNHDR.TransID =CONSIGNDTL .TransID AND GeneratorLocID=TW.GeneratorLocID AND wastecode=DT.WasteCode and WasteType=DT.WasteType  and month(TransDate)= month(getdate()) and CONSIGNHDR.status=8  AND ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "'   )as MonthSumQty,   " & _
                    "CONVERT(DECIMAL(12,2),( SELECT isnull(SUM(RCVQTY),0) FROM CONSIGNDTL,CONSIGNHDR WHERE CONSIGNHDR.TransID =CONSIGNDTL .TransID AND GeneratorLocID=TW.GeneratorLocID AND wastecode=DT.WasteCode and WasteType=DT.WasteType  and year(TransDate)= year(getdate()) and CONSIGNHDR.status=8  AND ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "'   )-DT.ExpectedQty) AS VarianceQty,  '' as Qty, '' as Remark, " & _
                    "CASE WHEN TW.Status = 0 THEN 'Pending Acceptance' WHEN TW.Status = 1 THEN 'Accepted' WHEN TW.Status = 3 AND TW.StatusTWG = 0 AND TW.SubStatus = 0 THEN 'Submission Submitted' WHEN TW.Status = 3 AND TW.StatusTWG = 1 AND TW.SubStatus = 0 THEN 'Submission Approved' WHEN TW.Status = 3 AND TW.StatusTWG = 2 AND TW.SubStatus = 0 THEN 'Submission Rejected' WHEN TW.Status = 3 AND TW.StatusTWG = 0 AND TW.SubStatus = 1 THEN 'Revision Submitted' WHEN TW.Status = 3 " & _
                    "AND TW.StatusTWG = 1 AND TW.SubStatus = 1 THEN 'Revision Approved' WHEN TW.Status = 3 AND TW.StatusTWG = 2 AND TW.SubStatus = 1 THEN 'Revision Rejected' END As StatusSubmmission, " & _
                    "CASE WHEN TW.Status=3 AND TW.StatusTWG=1 THEN 1 WHEN TW.Status=3 AND TW.StatusTWG=2 AND TW.SubStatus=1 THEN 1 ELSE 0 END AS Print992, TW.RevisionRemark  " & _
                    "FROM TWG_SUBMISSIONHDR TW WITH (NOLOCK)  " & _
                    "LEFT JOIN TWG_SUBMISSIONDTL DT WITH (NOLOCK)  ON TW.SubmissionID=DT.SubmissionID  " & _
                    "LEFT JOIN CONSIGNHDR C WITH (NOLOCK) ON TW.GENERATORID=C.GENERATORID AND TW.RECEIVERID=C.RECEIVERID  " & _
                    "LEFT  JOIN CONSIGNDTL D ON C.TransID=D.TransID AND DT.WasteCode=D.WasteCode and c.status=8 and D.wastetype = DT.WasteType  " & _
                    "LEFT JOIN BIZLOCATE B WITH (NOLOCK)  ON TW.GeneratorID=B.BizRegID LEFT JOIN CODEMASTER CM WITH(NOLOCK) ON DT.WasteType=CM.Code AND CM.CodeType='WTY'  " & _
                    "WHERE TW.ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "' AND YEAR(TW.createdate)=YEAR(GETDATE()) AND ((TW.Status=3 AND TW.StatusTWG=0 AND TW.SubStatus=1) OR (TW.Status=3 AND TW.StatusTWG=1 AND TW.Flag = 1) OR (TW.Status=3 AND TW.StatusTWG=2 AND TW.SubStatus=1)) "

                    If FieldCond IsNot Nothing AndAlso FieldCond <> "" Then
                        strSQL &= FieldCond
                    End If

                    strSQL &= "GROUP BY YEAR(TW.createdate), b.bizregid, b.bizlocid, DT.WasteCode, DT.WasteDescription, DT.WasteType, DT.Qty,  DT.ExpectedQty, TW.SubmissionID, B.BranchName, CM.CodeDesc,TW.CreateDate,DT.ExpectedQty1, DT.ExpectedQty2, TW.Status, TW.StatusTWG, TW.SubStatus, TW.GeneratorLocID, TW.RevisionRemark, TW.ReceiverID, TW.ReceiverLocID order by bil "

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetCapacityReceiver(ByVal ReceiverLocID As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()

                    strSQL = "SELECT BZ.BizLocID, CASE WHEN BZ.CapacityLevel = 0 THEN SIC.Capacitylevel ELSE BZ.CapacityLevel END AS CapacityLevel, ISNULL(SUM(DT.ExpectedQty1),0) AS UsedQty, " & _
                        "(CASE WHEN BZ.CapacityLevel = 0 THEN SIC.Capacitylevel ELSE BZ.CapacityLevel END-ISNULL(SUM(DT.ExpectedQty1),0)) AS LeftQty, BZ.BranchName " & _
                        "FROM BIZLOCATE BZ LEFT JOIN SIC WITH (NOLOCK) ON BZ.IndustryType = SIC.SICCode " & _
                        "LEFT JOIN TWG_SUBMISSIONHDR TW WITH (NOLOCK) ON TW.ReceiverLocID = BZ.BizLocID " & _
                        "LEFT JOIN TWG_SUBMISSIONDTL DT WITH (NOLOCK) ON TW.SubmissionID=DT.SubmissionID AND TW.STATUS <> 0 " & _
                        "WHERE BZ.BizLocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "' GROUP BY BZ.CapacityLevel, SIC.CapacityLevel, BZ.BranchName, BZ.BizLocID,TW.ReceiverLocID"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetCapacityLicense(ByVal ReceiverLocID As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()

                    strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY BL.BRANCHNAME ASC) AS Bil, LI.ItemCode, BL.BizRegID, BL.BizLocID, BL.ContactPerson, CM.CodeDesc, BL.BranchName, " & _
                        "CASE WHEN ISNULL(LC.CapacityLevel,0) = 0 THEN SUM(LI.Qty) ELSE LC.CapacityLevel END AS CapacityLevel " & _
                        "FROM BIZLOCATE BL WITH(NOLOCK) LEFT JOIN BIZENTITY BE WITH(NOLOCK) ON BL.BizRegID = BE.BizRegID LEFT JOIN CITY C " & _
                        "ON BL.City = C.CityCode LEFT JOIN STATE S ON BL.State = S.StateCode LEFT JOIN COUNTRY CN ON BL.Country = CN.CountryCode " & _
                        "INNER JOIN LICENSE LS WITH(NOLOCK) ON BL.BizLocID = LS.LocID AND BL.BizRegID = LS.CompanyID INNER JOIN LICENSEITEM LI " & _
                        "WITH(NOLOCK) ON LS.ContractNo = LI.ContractNo LEFT JOIN LICENSECAPACITY LC WITH(NOLOCK) ON LI.ItemCode = LC.ItemCode AND " & _
                        "LS.LocID = LC.ReceiverLocID LEFT JOIN WAC_ACCMASTER WB ON LI.ItemCode = WB.WasCode AND WB.CompanyID = LS.CompanyID AND " & _
                        "WB.LocID = LS.LocID  LEFT JOIN CODEMASTER CM WITH(NOLOCK) ON CM.Code = WB.WasType AND CM.CodeType = 'WTY' WHERE " & _
                        "LS.ContType = 'R' AND LS.LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "' AND LI.ItemCode = WB.WasCode GROUP BY LI.ItemCode, BL.BizRegID, " & _
                        "BL.ContactPerson, BL.BizLocID, CM.CodeDesc, BL.BranchName,LC.CapacityLevel"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetPremiseSubmission_Print(ByVal ReceiverLocID As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY B.BRANCHNAME ASC) AS 'No', B.BranchName as 'Waste Generator', " & _
                    "DT.WasteCode as 'Waste Code', CM.CodeDesc as 'Waste Type', DT.WasteDescription as 'Waste Name', CONVERT(DECIMAL(12,4),DT.ExpectedQty1) AS 'Average Monthly Qty (MT)', " & _
                    "( SELECT isnull(SUM(RCVQTY),0) FROM CONSIGNDTL,CONSIGNHDR WHERE CONSIGNHDR.TransID =CONSIGNDTL .TransID AND GeneratorLocID=TW.GeneratorLocID AND wastecode=DT.WasteCode and WasteType=DT.WasteType " & _
                    "and year(TransDate)= year(getdate()) and CONSIGNHDR.status=8  AND ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "'   )as 'Year-to-Year Qty (MT)', " & _
                    "CONVERT(DECIMAL(12,4),DT.ExpectedQty) AS  'Estimated Yearly Qty (MT)',  CONVERT(DECIMAL(12,4),DT.ExpectedQty-( SELECT isnull(SUM(RCVQTY),0) FROM CONSIGNDTL,CONSIGNHDR WHERE CONSIGNHDR.TransID =CONSIGNDTL .TransID AND " & _
                    "GeneratorLocID=TW.GeneratorLocID AND wastecode=DT.WasteCode and WasteType=DT.WasteType  and year(TransDate)= year(getdate()) and CONSIGNHDR.status=8  AND ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "'   )) AS 'Variance Year (MT)' , " & _
                    "TW.CreateDate as 'Create Date', ( SELECT isnull(SUM(RCVQTY),0) FROM CONSIGNDTL,CONSIGNHDR WHERE CONSIGNHDR.TransID =CONSIGNDTL .TransID AND GeneratorLocID=TW.GeneratorLocID AND wastecode=DT.WasteCode and WasteType=DT.WasteType  and year(TransDate)= year(getdate()) and CONSIGNHDR.status=8  AND ReceiverLocID = '0504N21446621'   )as YearSumQty  " & _
                    "FROM TWG_SUBMISSIONHDR TW WITH (NOLOCK)  " & _
                    "LEFT JOIN TWG_SUBMISSIONDTL DT WITH (NOLOCK)  ON TW.SubmissionID=DT.SubmissionID  " & _
                    "LEFT JOIN CONSIGNHDR C WITH (NOLOCK) ON TW.GENERATORID=C.GENERATORID AND TW.RECEIVERID=C.RECEIVERID  " & _
                    "LEFT  JOIN CONSIGNDTL D ON C.TransID=D.TransID AND DT.WasteCode=D.WasteCode and c.status=8 and D.wastetype = DT.WasteType  " & _
                    "LEFT JOIN BIZLOCATE B WITH (NOLOCK)  ON TW.GeneratorID=B.BizRegID   LEFT JOIN CODEMASTER CM WITH(NOLOCK) ON DT.WasteType=CM.Code AND CM.CodeType='WTY'  " & _
                    "WHERE TW.Flag = 1 AND TW.ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "' AND (TW.Status = 3 AND TW.StatusTWG = 1) OR (TW.Status = 3 AND TW.StatusTWG = 0 AND TW.SubStatus = 1) AND YEAR(TW.createdate)=YEAR(GETDATE())  " & _
                    "GROUP BY YEAR(TW.createdate),   DT.WasteCode, DT.WasteDescription, DT.WasteType, DT.Qty,  DT.ExpectedQty, TW.SubmissionID, B.BranchName, CM.CodeDesc,TW.CreateDate,DT.ExpectedQty1, DT.ExpectedQty2, TW.Status, TW.GeneratorLocID, TW.ReceiverLocID " & _
                    "order by 'No' "

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetSubmissionforGenerator(ByVal SubmissionID As String, ByVal GeneratorID As String, ByVal GeneratorLocID As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY B.BRANCHNAME ASC) AS Bil, YEAR(C.TransDate) AS YEAR,DT.WasteCode," & _
                        " DT.WasteDescription, CONVERT(DECIMAL(12,2),DT.Qty) AS MonthQty, CONVERT(DECIMAL(12,2),DT.ExpectedQty) AS  " & _
                        " ExpectedQty, TW.SubmissionID, CONVERT(DECIMAL(12,2),DT.Qty - DT.ExpectedQty) AS VarianceQty,  B.BranchName," & _
                        " CM.CodeDesc, TW.CreateDate,DT.ExpectedQty1,DT.ExpSetDate,DT.ExpSetDate1, ISNULL(SUM(CONVERT(DECIMAL(12,2),D.Qty)),0) AS Qty, " & _
                        " CASE WHEN TW.Status=1 THEN 'Approved' WHEN TW.Status=0 THEN 'Pending' Else 'Rejected' END AS Status " & _
                        " FROM TWG_SUBMISSIONHDR TW WITH (NOLOCK) LEFT JOIN TWG_SUBMISSIONDTL " & _
                        " DT WITH (NOLOCK)  ON TW.SubmissionID=DT.SubmissionID LEFT JOIN CONSIGNHDR C WITH (NOLOCK) ON " & _
                        " TW.GENERATORID=C.GENERATORID LEFT  JOIN CONSIGNDTL D ON C.TransID=D.TransID AND DT.WasteCode=D.WasteCode " & _
                        " LEFT JOIN BIZLOCATE B WITH (NOLOCK)  ON TW.GeneratorID=B.BizRegID  LEFT JOIN CODEMASTER CM WITH(NOLOCK) ON " & _
                        " DT.WasteType=CM.Code AND CM.CodeType='WTY' WHERE TW.SubmissionID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, SubmissionID) & "' " & _
                        " AND TW.GENERATORID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorID) & "' AND TW.GENERATORLOCID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorLocID) & "'" & _
                        " GROUP BY YEAR(C.TransDate),  " & _
                        " DT.WasteCode, DT.WasteDescription, DT.Qty,  DT.ExpectedQty, TW.SubmissionID, B.BranchName, DT.WasteType,CM.CodeDesc,TW.CreateDate,DT.ExpectedQty1,DT.ExpSetDate,DT.ExpSetDate1, TW.Status"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetPremiseSuPrint(ByVal ReceiverLocID As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()
                    'strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY B.BRANCHNAME ASC) AS Bil, YEAR(C.TransDate) AS YEAR,DT.WasteCode," & _
                    '    " DT.WasteDescription, CONVERT(DECIMAL(12,2),DT.Qty) AS MonthQty,B.BranchName, " & _
                    '    " CM.CodeDesc, TW.CreateDate,DT.ExpectedQty1,DT.ExpSetDate,DT.ExpSetDate1, ISNULL(SUM(CONVERT(DECIMAL(12,2),D.Qty)),0) AS Qty, " & _
                    '    " CASE WHEN TW.Status='1' THEN 'true' ELSE 'false' END AS StatusAck, CASE WHEN TW.Status='1' THEN TW.AckDate ELSE NULL END AS AckDate, TW.Status " & _
                    '    " FROM TWG_SUBMISSIONHDR TW WITH (NOLOCK) LEFT JOIN TWG_SUBMISSIONDTL " & _
                    '    " DT WITH (NOLOCK)  ON TW.SubmissionID=DT.SubmissionID LEFT JOIN CONSIGNHDR C WITH (NOLOCK) ON " & _
                    '    " TW.GENERATORID=C.GENERATORID LEFT  JOIN CONSIGNDTL D ON C.TransID=D.TransID AND DT.WasteCode=D.WasteCode " & _
                    '    " LEFT JOIN BIZLOCATE B WITH (NOLOCK)  ON TW.GeneratorID=B.BizRegID  LEFT JOIN CODEMASTER CM WITH(NOLOCK) ON " & _
                    '    " DT.WasteType=CM.Code AND CM.CodeType='WTY' WHERE TW.ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "' AND TW.Status <> '3' " & _
                    '    " GROUP BY YEAR(C.TransDate),  " & _
                    '    " DT.WasteCode, DT.WasteDescription, DT.Qty, TW.SubmissionID, B.BranchName, DT.WasteType,CM.CodeDesc,TW.CreateDate,DT.ExpectedQty1,DT.ExpSetDate,DT.ExpSetDate1, " & _
                    '    " TW.Status,TW.AckDate ORDER BY B.BranchName ASC"

                    strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY B.BRANCHNAME ASC) AS Bil, YEAR(TW.createdate) AS YEAR,DT.WasteCode," & _
                        " DT.WasteDescription , CONVERT(DECIMAL(12,4),DT.ExpectedQty) AS  " & _
                        " ExpectedQty, TW.SubmissionID, TW.ApprovalDate, E.NickName, CONVERT(DECIMAL(12,4),DT.Qty - DT.ExpectedQty) AS VarianceQty,  B.BranchName," & _
                        " CM.CodeDesc, TW.CreateDate,DT.ExpectedQty1,DT.ExpSetDate,DT.ExpSetDate1, " & _
                        " CASE WHEN TW.Status=1 THEN 'Approved' WHEN TW.Status=0 THEN 'Pending' Else 'Rejected' END AS Status, " & _
                        " CASE WHEN TW.Status='1' THEN 'true' ELSE 'false' END AS StatusAck, CASE WHEN TW.Status='1' THEN TW.AckDate ELSE NULL END AS AckDate, b.address1 as Address, CONVERT(DECIMAL(12,4),DT.ExpectedQty) AS  ExpectedQty_estimateYear ,CONVERT(DECIMAL(12,4),DT.ExpectedQty1) AS ExpectedQty1_averageMonth ,CONVERT(DECIMAL(12,4),DT.ExpectedQty2) AS ExpectedQty2_ApproveYear, " & _
                        " ( SELECT isnull(SUM(RCVQTY),0) FROM CONSIGNDTL,CONSIGNHDR WHERE CONSIGNHDR.TransID =CONSIGNDTL .TransID AND GeneratorLocID=TW.GeneratorLocID AND wastecode=DT.WasteCode and WasteType=DT.WasteType  and year(TransDate)= year(getdate()) and CONSIGNHDR.status=8  AND ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "'   )as YearSumQty, isnull(dt.remark1,'') as Remark " & _
                        " FROM TWG_SUBMISSIONHDR TW WITH (NOLOCK) " & _
                        " LEFT JOIN TWG_SUBMISSIONDTL DT WITH (NOLOCK)  ON TW.SubmissionID=DT.SubmissionID " & _
                        " LEFT JOIN CONSIGNHDR C WITH (NOLOCK) ON TW.GENERATORID=C.GENERATORID " & _
                        " LEFT  JOIN CONSIGNDTL D ON C.TransID=D.TransID AND DT.WasteCode=D.WasteCode and c.status=8 and D.wastetype = DT.WasteType " & _
                        " LEFT JOIN BIZLOCATE B WITH (NOLOCK)  ON TW.GeneratorID=B.BizRegID  " & _
                        " LEFT JOIN CODEMASTER CM WITH(NOLOCK) ON DT.WasteType=CM.Code AND CM.CodeType='WTY' " & _
                        " LEFT JOIN USRPROFILE U ON TW.ApprovalBy = U.UserID LEFT JOIN EMPLOYEE E ON U.RefID = E.EmployeeID " & _
                        " WHERE TW.ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "' AND ((TW.Status=3 AND TW.StatusTWG=0 AND TW.SubStatus=1) OR (TW.Status=3 AND TW.StatusTWG=1 AND TW.Flag = 1) OR (TW.Status=3 AND TW.StatusTWG=2 AND TW.SubStatus=1)) AND TW.Status <> 2 AND YEAR(TW.createdate)=YEAR(GETDATE())  " & _
                        " GROUP BY YEAR(TW.createdate),   DT.WasteCode, DT.WasteDescription, DT.WasteType, TW.ApprovalDate, E.NickName, DT.Qty,  DT.ExpectedQty, TW.SubmissionID, B.BranchName, CM.CodeDesc,TW.CreateDate,DT.ExpectedQty1,DT.ExpSetDate,DT.ExpSetDate1, TW.Status, TW.GeneratorLocID, TW.ReceiverLocID, TW.Status, AckDate, b.address1, ExpectedQty2, isnull(dt.remark1,'')  order by bil"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetWastecodeWG(ByVal ReceiverID As String, ByVal GeneratorID As String, Optional ByVal SubmissionID As String = "") As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()

                    strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY B.BRANCHNAME ASC) AS Bil, DT.WasteCode, DT.WasteDescription, CM.CodeDesc ,CAST(CONVERT(DECIMAL(12,2),DT.ExpectedQty1) AS VARCHAR(10)) + ' MT/bulan' AS ExpectedQty1_averageMonth " & _
                            "FROM TWG_SUBMISSIONHDR TW WITH (NOLOCK)   " & _
                            "LEFT JOIN TWG_SUBMISSIONDTL DT WITH (NOLOCK)  ON TW.SubmissionID=DT.SubmissionID   " & _
                            "LEFT JOIN CONSIGNHDR C WITH (NOLOCK) ON TW.GENERATORID=C.GENERATORID AND TW.RECEIVERID=C.RECEIVERID   " & _
                            "LEFT  JOIN CONSIGNDTL D ON C.TransID=D.TransID AND DT.WasteCode=D.WasteCode and c.status=8 and D.wastetype = DT.WasteType   " & _
                            "LEFT JOIN BIZLOCATE B WITH (NOLOCK)  ON TW.GeneratorID=B.BizRegID    " & _
                            "LEFT JOIN CODEMASTER CM WITH(NOLOCK) ON DT.WasteType=CM.Code AND CM.CodeType='WTY'   " & _
                            "WHERE TW.Flag = 1 AND YEAR(TW.createdate)=YEAR(GETDATE()) "

                    If SubmissionID IsNot Nothing OrElse SubmissionID <> "" Then
                        strSQL += " AND TW.SubmissionID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, SubmissionID) & "' "
                    End If

                    strSQL += "GROUP BY YEAR(TW.createdate), DT.WasteCode, DT.WasteDescription, DT.WasteType, CM.CodeDesc, DT.ExpectedQty1, B.BRANCHNAME order by bil "

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetApproval(ByVal SubmissionID As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()

                    strSQL = "  SELECT E.NickName, HD.ApprovalDate FROM TWG_SUBMISSIONHDR HD LEFT JOIN USRPROFILE U ON HD.ApprovalBy = U.UserID " & _
                        "LEFT JOIN EMPLOYEE E ON U.RefID = E.EmployeeID WHERE HD.SubmissionID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, SubmissionID) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetSubmissionforGeneratorApproved(ByVal GeneratorLocID As String, ByVal GeneratorID As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY B.BRANCHNAME ASC) AS Bil, YEAR(C.TransDate) AS YEAR,DT.WasteCode," & _
                        " DT.WasteDescription, DT.Qty AS MonthQty,B.BranchName," & _
                        " CM.CodeDesc, TW.CreateDate,DT.ExpectedQty1,DT.ExpSetDate,DT.ExpSetDate1, ISNULL(SUM(D.Qty),0) AS Qty FROM TWG_SUBMISSIONHDR TW WITH (NOLOCK) LEFT JOIN TWG_SUBMISSIONDTL " & _
                        " DT WITH (NOLOCK)  ON TW.SubmissionID=DT.SubmissionID LEFT JOIN CONSIGNHDR C WITH (NOLOCK) ON " & _
                        " TW.GENERATORID=C.GENERATORID LEFT  JOIN CONSIGNDTL D ON C.TransID=D.TransID AND DT.WasteCode=D.WasteCode " & _
                        " LEFT JOIN BIZLOCATE B WITH (NOLOCK)  ON TW.GeneratorID=B.BizRegID  LEFT JOIN CODEMASTER CM WITH(NOLOCK) ON " & _
                        " DT.WasteType=CM.Code AND CM.CodeType='WTY' WHERE TW.Status='1' AND TW.GeneratorLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorLocID) & "' " & _
                        " AND TW.GeneratorID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorID) & "' " &
                        " AND TW.Status='1' GROUP BY YEAR(C.TransDate),  " & _
                        " DT.WasteCode, DT.WasteDescription, DT.Qty, TW.SubmissionID, B.BranchName, DT.WasteType,CM.CodeDesc,TW.CreateDate,DT.ExpectedQty1,DT.ExpSetDate,DT.ExpSetDate1, " & _
                        " TW.Status ORDER BY B.BranchName ASC"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetSubmissionGeneratorApproved(ByVal GeneratorLocID As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()
                    strSQL = "select HDR.SubmissionID, ROW_NUMBER() OVER (ORDER BY BIZ.BRANCHNAME ASC) AS Bil, HDR.RejectRemark, HDR.UpdateBy, HDR.LastUpdate, BIZ.BranchName, HDR.ReceiverLocID, HDR.Status , HDR.GeneratorLocID, DT.WasteCode, CM.CodeDesc, dt.Qty" & _
                            " from TWG_SUBMISSIONHDR HDR WITH(NOLOCK) " & _
                            " INNER JOIN TWG_SUBMISSIONDTL DT WITH(NOLOCK) ON HDR.SubmissionID=DT.SubmissionID" & _
                            " INNER JOIN BIZLOCATE BIZ WITH(NOLOCK) ON HDR.ReceiverLocID=BIZ.BizLocID " & _
                            " LEFT JOIN CODEMASTER CM WITH(NOLOCK) ON DT.WasteType=CM.Code AND CM.CodeType='WTY' " & _
                            " WHERE HDR.Status=0 AND HDR.GeneratorLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorLocID) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetSubmissionGeneratorDecline(ByVal GeneratorLocID As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()
                    strSQL = "select TOP 1 HDR.SubmissionID, BIZ.BRANCHNAME, ROW_NUMBER() OVER (ORDER BY BIZ.BRANCHNAME ASC) AS Bil, HDR.DeclineRemark, HDR.UpdateBy, HDR.LastUpdate, HDR.AckDate, BIZ.BranchName, HDR.ReceiverLocID, HDR.Status , HDR.GeneratorLocID, DT.WasteCode, CM.CodeDesc, dt.Qty" & _
                            " from TWG_SUBMISSIONHDR HDR WITH(NOLOCK) " & _
                            " INNER JOIN TWG_SUBMISSIONDTL DT WITH(NOLOCK) ON HDR.SubmissionID=DT.SubmissionID" & _
                            " INNER JOIN BIZLOCATE BIZ WITH(NOLOCK) ON HDR.ReceiverLocID=BIZ.BizLocID " & _
                            " LEFT JOIN CODEMASTER CM WITH(NOLOCK) ON DT.WasteType=CM.Code AND CM.CodeType='WTY' " & _
                            " WHERE HDR.Status = 2 AND HDR.GeneratorLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorLocID) & "' " & _
                            " ORDER BY HDR.AckDate DESC"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetSubmissionReceiverRejected(ByVal ReceiverLocID As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()
                    strSQL = "select TOP 1 HDR.SubmissionID, BIZ.BRANCHNAME, CASE WHEN HDR.SubStatus = 1 THEN 'Revision' " & _
                        "WHEN HDR.SubStatus = 0 THEN 'Submission' END AS Note, CASE WHEN HDR.SubStatus = 1 THEN HDR.RevisionRemark " & _
                        "WHEN HDR.SubStatus = 0  THEN HDR.SubmissionRemark END AS Remark, HDR.UpdateBy, HDR.LastUpdate, HDR.AckDate, " & _
                        "BIZ.BranchName, HDR.ReceiverLocID, HDR.Status , HDR.GeneratorLocID, HDR.ApprovalDate, DT.WasteCode, CM.CodeDesc, dt.Qty " & _
                        "from TWG_SUBMISSIONHDR HDR WITH(NOLOCK) INNER JOIN TWG_SUBMISSIONDTL DT WITH(NOLOCK) " & _
                        "ON HDR.SubmissionID=DT.SubmissionID INNER JOIN BIZLOCATE BIZ WITH(NOLOCK) ON HDR.ReceiverLocID=BIZ.BizLocID " & _
                        "LEFT JOIN CODEMASTER CM WITH(NOLOCK) ON DT.WasteType=CM.Code AND CM.CodeType='WTY' WHERE HDR.StatusTWG = 2 " & _
                        "AND HDR.ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "' ORDER BY HDR.ApprovalDate DESC"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetSubmissionReceiverPending(ByVal LocID As String, Optional ByVal Status As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()
                    strSQL = "select HDR.SubmissionID, ROW_NUMBER() OVER (ORDER BY BIZ.BRANCHNAME ASC) AS Bil, HDR.DeclineRemark, HDR.UpdateBy, HDR.LastUpdate, HDR.AckDate, BIZ.BranchName, HDR.ReceiverLocID, HDR.Status , HDR.GeneratorLocID, DT.WasteCode, CM.CodeDesc, dt.Qty" & _
                            " from TWG_SUBMISSIONHDR HDR WITH(NOLOCK) " & _
                            " INNER JOIN TWG_SUBMISSIONDTL DT WITH(NOLOCK) ON HDR.SubmissionID=DT.SubmissionID" & _
                            " INNER JOIN BIZLOCATE BIZ WITH(NOLOCK) ON HDR.ReceiverLocID=BIZ.BizLocID " & _
                            " LEFT JOIN CODEMASTER CM WITH(NOLOCK) ON DT.WasteType=CM.Code AND CM.CodeType='WTY' " & _
                            " WHERE HDR.Status = 0 AND HDR.ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetSubmissionGeneratorApprovedByID(ByVal ReceiverLocID As String, ByVal Type As String, ByVal Status As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()
                    strSQL = "select HDR.SubmissionRemark, HDR.ReasonRemark, HDR.RevisionRemark,BIZ.AccNo, BIZ.BranchName, HDR.ReceiverLocID, HDR.StatusTWG, HDR.GeneratorLocID, DT.WasteCode, CM.CodeDesc, dt.Qty, HDR.LastUpdate, HDR.Approvaldate, HDR.ApprovalBy" & _
                            " from TWG_SUBMISSIONHDR HDR WITH(NOLOCK) " & _
                            " INNER JOIN TWG_SUBMISSIONDTL DT WITH(NOLOCK) ON HDR.SubmissionID=DT.SubmissionID" & _
                            " INNER JOIN BIZLOCATE BIZ WITH(NOLOCK) ON HDR.ReceiverLocID=BIZ.BizLocID " & _
                            " LEFT JOIN CODEMASTER CM WITH(NOLOCK) ON DT.WasteType=CM.Code AND CM.CodeType='WTY' " & _
                            " WHERE HDR.ReceiverLocID= '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "'" & _
                            " AND HDR.SubStatus='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Type) & "' AND HDR.Status='3' ORDER BY HDR.LastUpdate DESC"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetDashboardLatestNews(ByVal GeneratorLocID As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()

                    'strSQL = "SELECT TOP 3 * FROM " & _
                    '        "(SELECT TOP 3 BI.CompanyName AS ApproveBy, '' Generator, " & _
                    '        "CASE " & _
                    '        "WHEN (HDR.Status = 2 AND HDR.StatusTWG = 0 AND HDR.SubStatus = 0) THEN HDR.DeclineRemark " & _
                    '        "WHEN (HDR.Status = 1 AND HDR.StatusTWG = 0 AND HDR.SubStatus = 0) THEN HDR.DeclineRemark " & _
                    '        "WHEN (HDR.Status = 2 AND HDR.StatusTWG = 2 AND HDR.SubStatus = 0) THEN HDR.DeclineRemark " & _
                    '        "WHEN (HDR.Status = 3 AND HDR.StatusTWG = 1 AND HDR.SubStatus = 1 AND HDR.Flag = 1) THEN HDR.ReasonRemark " & _
                    '        "WHEN (HDR.Status = 3 AND HDR.StatusTWG = 1 AND HDR.SubStatus = 1 AND HDR.Flag = 0) THEN HDR.ReasonRemark " & _
                    '        "END AS Remark, " & _
                    '        "HDR.AckDate AS ApproveDate, '' AS Note, DT.WasteCode, CM.CodeDesc, " & _
                    '        "CASE " & _
                    '        "WHEN (HDR.Status = 2 AND HDR.StatusTWG = 0 AND HDR.SubStatus = 0) THEN 0 " & _
                    '        "WHEN (HDR.Status = 1 AND HDR.StatusTWG = 0 AND HDR.SubStatus = 0) THEN 1 " & _
                    '        "WHEN (HDR.Status = 2 AND HDR.StatusTWG = 2 AND HDR.SubStatus = 0) THEN 2 " & _
                    '        "WHEN (HDR.Status = 3 AND HDR.StatusTWG = 1 AND HDR.SubStatus = 1 AND HDR.Flag = 1) THEN 3 " & _
                    '        "WHEN (HDR.Status = 3 AND HDR.StatusTWG = 1 AND HDR.SubStatus = 1 AND HDR.Flag = 0) THEN 4 " & _
                    '        "END AS Type " & _
                    '        "FROM TWG_SUBMISSIONHDR HDR WITH(NOLOCK) INNER JOIN TWG_SUBMISSIONDTL DT WITH(NOLOCK) ON HDR.SubmissionID=DT.SubmissionID INNER JOIN BIZLOCATE BIZ WITH(NOLOCK) " & _
                    '        "ON HDR.ReceiverLocID=BIZ.BizLocID LEFT JOIN BIZENTITY BI WITH(NOLOCK) ON BIZ.BizRegID = BI.BizRegID LEFT JOIN CODEMASTER CM WITH(NOLOCK) ON DT.WasteType=CM.CodeSeq AND CM.CodeType='WTY' WHERE " & _
                    '        "(HDR.Status = 2 AND HDR.StatusTWG = 0 AND HDR.SubStatus = 0 AND HDR.GeneratorLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorLocID) & "') OR " & _
                    '        "(HDR.Status = 1 AND HDR.StatusTWG = 0 AND HDR.SubStatus = 0 AND HDR.GeneratorLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorLocID) & "') OR " & _
                    '        "(HDR.Status = 2 AND HDR.StatusTWG = 2 AND HDR.SubStatus = 0 AND HDR.GeneratorLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorLocID) & "') OR " & _
                    '        "(HDR.Status = 3 AND HDR.StatusTWG = 1 AND HDR.SubStatus = 1 AND HDR.Flag = 1 AND HDR.GeneratorLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorLocID) & "') OR " & _
                    '        "(HDR.Status = 3 AND HDR.StatusTWG = 1 AND HDR.SubStatus = 1 AND HDR.Flag = 0 AND HDR.GeneratorLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorLocID) & "') " & _
                    '        "ORDER BY HDR.AckDate DESC " & _
                    '        "UNION ALL " & _
                    '        "SELECT TOP 3 'DOE' AS ApproveBy, '' Generator, TransRemark  AS Remark, LastUpdate AS ApproveDate,'' AS Note, '' AS WasteCode, '' AS CodeDesc, 9 AS Type FROM NOTIFYHDR WITH(NOLOCK) " & _
                    '        "WHERE LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorLocID) & "' AND Flag = 2 AND Posted = 2 ORDER By ApproveDate DESC " & _
                    '        "UNION ALL " & _
                    '        "SELECT TOP 3 'DOE' AS ApproveBy, BI.CompanyName Generator, CASE WHEN HDR.SubStatus = 1 THEN HDR.RevisionRemark WHEN HDR.SubStatus = 0 THEN HDR.SubmissionRemark END AS Remark, ApprovalDate AS ApproveDate, " & _
                    '        "CASE WHEN HDR.SubStatus = 1 THEN 'Revision' WHEN HDR.SubStatus = 0 THEN 'Submission' END AS Note, DT.WasteCode, CM.CodeDesc, " & _
                    '        "CASE " & _
                    '        "WHEN (HDR.Status = 3 AND HDR.StatusTWG = 2 AND HDR.SubStatus = 0) THEN 5 " & _
                    '        "WHEN (HDR.Status = 3 AND HDR.StatusTWG = 1 AND HDR.SubStatus = 0) THEN 6 " & _
                    '        "WHEN (HDR.Status = 3 AND HDR.StatusTWG = 2 AND HDR.SubStatus = 1) THEN 7 " & _
                    '        "WHEN (HDR.Status = 3 AND HDR.StatusTWG = 1 AND HDR.SubStatus = 1) THEN 8 " & _
                    '        "END AS Type " & _
                    '        "FROM TWG_SUBMISSIONHDR HDR WITH(NOLOCK) INNER JOIN TWG_SUBMISSIONDTL DT WITH(NOLOCK) ON HDR.SubmissionID=DT.SubmissionID INNER JOIN BIZLOCATE BIZ WITH(NOLOCK) ON HDR.ReceiverLocID=BIZ.BizLocID LEFT JOIN BIZENTITY BI WITH(NOLOCK) " & _
                    '        "ON HDR.GeneratorID = BI.BizRegID LEFT JOIN CODEMASTER CM WITH(NOLOCK) ON DT.WasteType=CM.CodeSeq AND CM.CodeType='WTY' WHERE " & _
                    '        "(HDR.Status = 3 AND HDR.StatusTWG = 2 AND HDR.SubStatus = 0 AND HDR.ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorLocID) & "') OR " & _
                    '        "(HDR.Status = 3 AND HDR.StatusTWG = 1 AND HDR.SubStatus = 0 AND HDR.ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorLocID) & "') OR " & _
                    '        "(HDR.Status = 3 AND HDR.StatusTWG = 2 AND HDR.SubStatus = 1 AND HDR.ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorLocID) & "') OR " & _
                    '        "(HDR.Status = 3 AND HDR.StatusTWG = 1 AND HDR.SubStatus = 1 AND HDR.ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorLocID) & "') " & _
                    '        "ORDER BY HDR.ApprovalDate DESC) AS X ORDER BY ApproveDate DESC"

                    strSQL = "SELECT TOP 3 'DOE' AS ApproveBy, '' Generator, TransRemark  AS Remark, LastUpdate AS ApproveDate,'' AS Note, '' AS WasteCode, '' AS CodeDesc, 9 AS Type FROM NOTIFYHDR WITH(NOLOCK) " & _
                           "WHERE LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorLocID) & "' AND Flag = 2 AND Posted = 2 ORDER By ApproveDate DESC "

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetDistinctTWG(ByVal ReceiverLocID As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()
                    strSQL = "select DISTINCT DTL.WasteCode from TWG_SUBMISSIONHDR hdr WITH (NOLOCK) left join TWG_SUBMISSIONDTL dtl on hdr.SubmissionID=dtl.SubmissionID WHERE HDR.ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "' AND HDR.Status <> 3"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetStatusNo(ByVal ReceiverLocID As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()
                    strSQL = "select SubmissionID, LastUpdate from TWG_SUBMISSIONHDR where YEAR(ApprovalDate)=YEAR(GETDATE())-1 and StatusTWG=1 AND ReceiverLocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetApprovalSubmission(ByVal Type As Integer, ByVal History As Boolean, Optional ByVal LastUpdate As String = Nothing, Optional ByVal District As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()
                    strSQL = "select "
                    If Type = 3 Then
                        strSQL &= " TOP 50 "
                    End If
                    strSQL &= "CASE WHEN HDR.SubStatus = 0 THEN 'New Submission' ELSE 'Revision' END AS RequestType, MAX(hdr.Createdate) AS SubmissionDate,SUM(DTL.ExpectedQty1) AS Qty,Biz.CITY, ISNULL(ct.CityDesc,'') AS District, BIZ.BranchName, HDR.ReceiverLocID, " & _
                        "BIZ.BizRegID, MAX(HDR.ApprovalDate), HDR.ApprovalBy,HDR.UpdateBy, HDR.SubStatus, CASE WHEN Y.AppC = 1 then 'Completed' else 'Pending' end TWGStatus " & _
                        "from TWG_SUBMISSIONHDR HDR WITH(NOLOCK) JOIN TWG_SUBMISSIONDTL DTL ON HDR.SubmissionID=DTL.SubmissionID LEFT JOIN BIZLOCATE BIZ WITH(NOLOCK) ON HDR.ReceiverLocID=BIZ.BizLocID " & _
                        "LEFT JOIN CITY ct ON ct.CityCode=BIZ.CITY OUTER APPLY (SELECT (case HD.StatusTWG when 1 then 1 when 2 then 1 else 0 end) AppC FROM TWG_SUBMISSIONHDR HD LEFT JOIN TWG_SUBMISSIONDTL DT " & _
                        "ON HD.SubmissionID=DT.SubmissionID WHERE HD.ReceiverID = BIZ.BizRegID AND HD.Status = 3 "
                    If Type = 3 Then
                        strSQL &= " AND HD.SubStatus IN (0,1) "
                    Else
                        strSQL &= " AND HD.SubStatus='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Type) & "'"
                    End If
                    If History Then
                        strSQL &= " AND HD.StatusTWG IN(1,2)"
                    Else
                        strSQL &= " AND HD.StatusTWG = 0"
                    End If
                    strSQL &= ") Y WHERE HDR.Status = 3 "
                    If Type = 3 Then
                        strSQL &= " AND HDR.SubStatus IN (0,1) "
                    Else
                        strSQL &= " AND HDR.SubStatus='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Type) & "'"
                    End If

                    If History Then
                        strSQL &= " AND HDR.StatusTWG IN(1,2)"
                    Else
                        strSQL &= " AND HDR.StatusTWG = 0"
                    End If
                    If LastUpdate IsNot Nothing Then
                        strSQL &= LastUpdate
                    End If
                    If District IsNot Nothing AndAlso District <> "" AndAlso District <> "All" Then
                        strSQL &= " AND BIZ.CITY='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, District) & "'"
                    End If

                    strSQL &= " GROUP BY HDR.SubStatus, Y.AppC, BIZ.BranchName, HDR.ReceiverLocID, HDR.SubStatus,BIZ.BizRegID, BIZ.CITY, ct.CityDesc, HDR.ApprovalBy,HDR.UpdateBy order by branchname asc "
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetGeneratorList(ByVal ReceiverLocID As String, ByVal Type As String, ByVal History As Boolean, Optional ByVal Reason As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()
                    strSQL = "select b.SubmissionID, a.StatusTWG, CASE WHEN ISNULL(LC.CapacityLevel,0) = 0 THEN sum(LI.Qty) ELSE LC.CapacityLevel END " & _
                        "- SUM(b.ExpectedQty1) AS balancecapacity, CASE WHEN ISNULL(LC.CapacityLevel,0) = 0 THEN sum(LI.Qty) ELSE LC.CapacityLevel " & _
                        "END AS licensedcapacity, a.Flag, b.ExpectedQty AS estimateYear , b.Qty, b.Remark1, b.ExpectedQty1 AS averageMonth, WasteDescription, " & _
                        "ca.branchname, wastecode, wastetype, generatorlocid, codedesc, CASE WHEN a.Flag = 0 OR b.Remark2 = 'Removal of Generator' THEN 'Removal of Generator' WHEN b.Qty <> 0 OR b.Remark2 = 'Value Changed' THEN 'Value Changed' ELSE '' END AS Revision, " & _
                        "a.ApprovalDate from twg_submissionhdr a inner join twg_submissiondtl b on a.submissionid=b.submissionid " & _
                        "left join bizlocate c on a.receiverlocid=c.bizlocid  left join bizlocate ca on a.generatorlocid=ca.bizlocid left join codemaster d on b.wastetype=d.code and d.codetype='WTY' " & _
                        "INNER JOIN LICENSE LS WITH(NOLOCK) ON c.BizLocID = LS.LocID AND c.BizRegID =  LS.CompanyID INNER JOIN LICENSEITEM LI WITH(NOLOCK) " & _
                        "ON LS.ContractNo = LI.ContractNo LEFT JOIN LICENSECAPACITY LC WITH(NOLOCK) ON LI.ItemCode = LC.ItemCode AND LS.LocID = LC.ReceiverLocID " & _
                        "WHERE a.ReceiverLocID= '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "' AND a.Status=3 " & _
                        "AND a.SubStatus='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Type) & "' "
                    If History Then
                        strSQL &= " AND a.StatusTWG IN(1,2)"
                    Else
                        strSQL &= " AND a.StatusTWG = 0"
                    End If
                    strSQL &= " AND LI.ItemCode = b.wastecode " & _
                        "GROUP BY b.SubmissionID, wastecode, wastetype, b.qty, generatorlocid,ca.branchname,WasteDescription, b.ExpectedQty, b.ExpectedQty1, " & _
                        "codedesc, a.Flag, LC.CapacityLevel, a.StatusTWG, b.Remark1, b.Remark2, a.ApprovalDate"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetCapacityList(ByVal ReceiverLocID As String, ByVal Type As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_submissionhdrInfo.MyInfo
                    StartSQLControl()
                    strSQL = "select ISNULL(CASE WHEN ISNULL(LC.CapacityLevel,0) = 0 THEN LI.Qty ELSE LC.CapacityLevel END " & _
                        "- (SELECT SUM(ExpectedQty) FROM twg_submissiondtl dtl INNER JOIN twg_submissionhdr hdr ON " & _
                        "hdr.submissionid=dtl.submissionid WHERE dtl.wastecode = LI.ItemCode AND hdr.ReceiverLocID= '0504N21446621' AND hdr.Status = 3),0) " & _
                        "AS balancecapacity, CASE WHEN ISNULL(LC.CapacityLevel,0) = 0 THEN LI.Qty ELSE LC.CapacityLevel END AS licensedcapacity, " & _
                        "ISNULL((SELECT SUM(ExpectedQty) FROM twg_submissiondtl dtl INNER JOIN twg_submissionhdr hdr ON hdr.submissionid=dtl.submissionid " & _
                        "WHERE dtl.wastecode = LI.ItemCode AND hdr.ReceiverLocID= '0504N21446621' AND hdr.Status = 3),0) AS estimateYear, b.Qty, LI.ItemCode from twg_submissionhdr a " & _
                        "INNER join twg_submissiondtl b on a.submissionid=b.submissionid INNER join bizlocate c on a.receiverlocid=c.bizlocid INNER JOIN LICENSE LS " & _
                        "WITH(NOLOCK) ON c.BizLocID = LS.LocID AND c.BizRegID =  LS.CompanyID INNER JOIN LICENSEITEM LI WITH(NOLOCK) ON LS.ContractNo = LI.ContractNo " & _
                        "LEFT JOIN LICENSECAPACITY LC WITH(NOLOCK) ON LI.ItemCode = LC.ItemCode AND LS.LocID = LC.ReceiverLocID " & _
                        "WHERE a.ReceiverLocID= '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "' AND a.Status=3 GROUP BY LI.ItemCode, b.qty, a.Flag, LC.CapacityLevel, LI.Qty"
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
#Region "Twg_submissionhdr Container"
        Public Class Twg_submissionhdr_FieldName
            Public SubmissionID As System.String = "SubmissionID"
            Public ReceiverID As System.String = "ReceiverID"
            Public ReceiverLocID As System.String = "ReceiverLocID"
            Public GeneratorID As System.String = "GeneratorID"
            Public GeneratorLocID As System.String = "GeneratorLocID"
            Public SubmissionRemark As System.String = "SubmissionRemark"
            Public RevisionRemark As System.String = "RevisionRemark"
            Public DeclineRemark As System.String = "DeclineRemark"
            Public ReasonRemark As System.String = "ReasonRemark"
            Public RevSubmitRemark As System.String = "RevSubmitRemark"
            Public SubStatus As System.String = "SubStatus"
            Public ApprovalDate As System.String = "ApprovalDate"
            Public Status As System.String = "Status"
            Public Flag As System.String = "Flag"
            Public CreateDate As System.String = "CreateDate"
            Public CreateBy As System.String = "CreateBy"
            Public LastUpdate As System.String = "LastUpdate"
            Public UpdateBy As System.String = "UpdateBy"
            Public AckDate As System.String = "AckDate"
            Public StatusTWG As System.String = "StatusTWG"
            Public rowguid As System.String = "rowguid"
            Public SyncCreate As System.String = "SyncCreate"
            Public SyncLastUpd As System.String = "SyncLastUpd"
            Public LastSyncBy As System.String = "LastSyncBy"
            Public ApprovalBy As System.String = "ApprovalBy"
        End Class

        Public Class Twg_submissionhdr
            Protected _SubmissionID As System.String
            Protected _ReceiverID As System.String
            Protected _ReceiverLocID As System.String
            Protected _GeneratorID As System.String
            Protected _GeneratorLocID As System.String
            Private _SubmissionRemark As System.String
            Private _RevisionRemark As System.String
            Private _DeclineRemark As System.String
            Private _ReasonRemark As System.String
            Private _RevSubmitRemark As System.String
            Private _SubStatus As System.Byte
            Private _ApprovalDate As System.DateTime
            Private _Status As System.Byte
            Private _Flag As System.Byte
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _AckDate As System.DateTime
            Private _StatusTWG As System.Byte
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _LastSyncBy As System.String
            Private _ApprovalBy As System.String
            Private _WasteType As System.String
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
            ''' Mandatory
            ''' </summary>
            Public Property GeneratorID As System.String
                Get
                    Return _GeneratorID
                End Get
                Set(ByVal Value As System.String)
                    _GeneratorID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property GeneratorLocID As System.String
                Get
                    Return _GeneratorLocID
                End Get
                Set(ByVal Value As System.String)
                    _GeneratorLocID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SubmissionRemark As System.String
                Get
                    Return _SubmissionRemark
                End Get
                Set(ByVal Value As System.String)
                    _SubmissionRemark = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RevisionRemark As System.String
                Get
                    Return _RevisionRemark
                End Get
                Set(ByVal Value As System.String)
                    _RevisionRemark = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property DeclineRemark As System.String
                Get
                    Return _DeclineRemark
                End Get
                Set(ByVal Value As System.String)
                    _DeclineRemark = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ReasonRemark As System.String
                Get
                    Return _ReasonRemark
                End Get
                Set(ByVal Value As System.String)
                    _ReasonRemark = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RevSubmitRemark As System.String
                Get
                    Return _RevSubmitRemark
                End Get
                Set(ByVal Value As System.String)
                    _RevSubmitRemark = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SubStatus As System.Byte
                Get
                    Return _SubStatus
                End Get
                Set(ByVal Value As System.Byte)
                    _SubStatus = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property ApprovalDate As System.DateTime
                Get
                    Return _ApprovalDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _ApprovalDate = Value
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
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property AckDate As System.DateTime
                Get
                    Return _AckDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _AckDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property StatusTWG As System.Byte
                Get
                    Return _StatusTWG
                End Get
                Set(ByVal Value As System.Byte)
                    _StatusTWG = Value
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
            Public Property ApprovalBy As System.String
                Get
                    Return _ApprovalBy
                End Get
                Set(ByVal Value As System.String)
                    _ApprovalBy = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace
#End Region

#Region "Class Info"
#Region "Twg_submissionhdr Info"
    Public Class Twg_submissionhdrInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "SubmissionID,ReceiverID,ReceiverLocID,GeneratorID,GeneratorLocID,SubmissionRemark,RevisionRemark,DeclineRemark,ReasonRemark,RevSubmitRemark,SubStatus,ApprovalDate,Status,Flag,CreateDate,CreateBy,LastUpdate,UpdateBy,AckDate,StatusTWG,rowguid,SyncCreate,SyncLastUpd,LastSyncBy,ApprovalBy"
                .CheckFields = "Status,Flag"
                .TableName = "Twg_submissionhdr"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "SubmissionID,ReceiverID,ReceiverLocID,GeneratorID,GeneratorLocID,SubmissionRemark,RevisionRemark,DeclineRemark,ReasonRemark,RevSubmitRemark,SubStatus,ApprovalDate,Status,Flag,CreateDate,CreateBy,LastUpdate,UpdateBy,AckDate,StatusTWG,rowguid,SyncCreate,SyncLastUpd,LastSyncBy,ApprovalBy"
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
#Region "TWG_SUBMISSIONHDR Scheme"
    Public Class TWG_SUBMISSIONHDRScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "SubmissionID"
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
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "GeneratorID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "GeneratorLocID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "SubmissionRemark"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ApprovedRemark"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "RejectRemark"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)

        End Sub

        Public ReadOnly Property SubmissionID As StrucElement
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
        Public ReadOnly Property GeneratorID As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property GeneratorLocID As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property

        Public ReadOnly Property SubmissionRemark As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property ApprovedRemark As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property RejectRemark As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace
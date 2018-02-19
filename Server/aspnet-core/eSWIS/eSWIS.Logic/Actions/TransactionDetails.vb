Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Actions
    Public NotInheritable Class TransactionDetails
        Inherits Core.CoreControl
        Private ItmtransdtlInfo As ItmtransdtlInfo = New ItmtransdtlInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function Save(ByVal ItmtransdtlCont As Container.Itmtransdtl, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ItmtransdtlCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItmtransdtlInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.DocCode) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.ItemCode) & "' AND ItemName = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.ItemCode) & "' AND LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.LocID) & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    'If Convert.ToInt16(.Item("Flag")) = 0 Then
                                    '    'Found but deleted
                                    '    blnFlag = False
                                    'Else
                                    '    'Found and active
                                    '    blnFlag = True
                                    'End If
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
                                .TableName = "Itmtransdtl WITH (ROWLOCK)"
                                .AddField("StorageID", ItmtransdtlCont.StorageID, SQLControl.EnumDataType.dtString)
                                .AddField("TermID", ItmtransdtlCont.TermID, SQLControl.EnumDataType.dtCustom)
                                .AddField("SeqNo", ItmtransdtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Status", ItmtransdtlCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("TransType", ItmtransdtlCont.TransType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OperationType", ItmtransdtlCont.OperationType, SQLControl.EnumDataType.dtString)
                                .AddField("ItmPrice", ItmtransdtlCont.ItmPrice, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ReqQty", ItmtransdtlCont.ReqQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ReqPackQty", ItmtransdtlCont.ReqPackQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpeningQty", ItmtransdtlCont.OpeningQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpeningPackQty", ItmtransdtlCont.OpeningPackQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Qty", ItmtransdtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("PackQty", ItmtransdtlCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("HandlingQty", ItmtransdtlCont.HandlingQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("HandlingPackQty", ItmtransdtlCont.HandlingPackQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ClosingQty", ItmtransdtlCont.ClosingQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ClosingPackQty", ItmtransdtlCont.ClosingPackQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastInQty", ItmtransdtlCont.LastInQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastInPackQty", ItmtransdtlCont.LastInPackQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastOutQty", ItmtransdtlCont.LastOutQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastOutPackQty", ItmtransdtlCont.LastOutPackQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RtnPackQty", ItmtransdtlCont.RtnPackQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RtnQty", ItmtransdtlCont.RtnQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RecPackQty", ItmtransdtlCont.RecPackQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RecQty", ItmtransdtlCont.RecQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ExpiredQty", ItmtransdtlCont.ExpiredQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ExpiryDate", ItmtransdtlCont.ExpiryDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Remark", ItmtransdtlCont.Remark, SQLControl.EnumDataType.dtStringN)
                                .AddField("LotNo", ItmtransdtlCont.LotNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("CityCode", ItmtransdtlCont.CityCode, SQLControl.EnumDataType.dtStringN)
                                .AddField("SecCode", ItmtransdtlCont.SecCode, SQLControl.EnumDataType.dtStringN)
                                .AddField("BinCode", ItmtransdtlCont.BinCode, SQLControl.EnumDataType.dtStringN)
                                '.AddField("RowGuid", ItmtransdtlCont.RowGuid, SQLControl.EnumDataType.dtString)
                                .AddField("CreateDate", ItmtransdtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", ItmtransdtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", ItmtransdtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", ItmtransdtlCont.UpdateBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.DocCode) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.ItemCode) & "' AND ItemName = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.ItemCode) & "' AND LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.LocID) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("DocCode", ItmtransdtlCont.DocCode, SQLControl.EnumDataType.dtString)
                                                .AddField("ItemCode", ItmtransdtlCont.ItemCode, SQLControl.EnumDataType.dtString)
                                                .AddField("LocID", ItmtransdtlCont.LocID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.DocCode) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.ItemCode) & "' AND ItemName = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.ItemCode) & "' AND LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.LocID) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True

                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                    'Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                                Else
                                    message = axExecute.Message.ToString()
                                    'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDetails", axExecute.Message & strSQL, axExecute.StackTrace)
                                Return False

                            Finally
                                objSQL.Dispose()
                            End Try
                        End If

                    End If
                End If
            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDetails", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDetails", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ItmtransdtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function Save(ByVal ItmtransdtlContlist As List(Of Container.Itmtransdtl), ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ItmtransdtlContlist Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItmtransdtlInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlContlist(0).DocCode) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlContlist(0).ItemCode) & "' AND ItemName = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlContlist(0).ItemName) & "' AND LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlContlist(0).LocID) & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    'If Convert.ToInt16(.Item("Flag")) = 0 Then
                                    '    'Found but deleted
                                    '    blnFlag = False
                                    'Else
                                    '    'Found and active
                                    '    blnFlag = True
                                    'End If
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
                                For Each ItmtransdtlCont In ItmtransdtlContlist
                                    .TableName = "Itmtransdtl WITH (ROWLOCK)"
                                    .AddField("StorageID", ItmtransdtlCont.StorageID, SQLControl.EnumDataType.dtString)
                                    .AddField("TermID", ItmtransdtlCont.TermID, SQLControl.EnumDataType.dtCustom)
                                    .AddField("SeqNo", ItmtransdtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Status", ItmtransdtlCont.Status, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("TransType", ItmtransdtlCont.TransType, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OperationType", ItmtransdtlCont.OperationType, SQLControl.EnumDataType.dtString)
                                    .AddField("ItmPrice", ItmtransdtlCont.ItmPrice, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ReqQty", ItmtransdtlCont.ReqQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ReqPackQty", ItmtransdtlCont.ReqPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpeningQty", ItmtransdtlCont.OpeningQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("OpeningPackQty", ItmtransdtlCont.OpeningPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Qty", ItmtransdtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("PackQty", ItmtransdtlCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("HandlingQty", ItmtransdtlCont.HandlingQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("HandlingPackQty", ItmtransdtlCont.HandlingPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ClosingQty", ItmtransdtlCont.ClosingQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ClosingPackQty", ItmtransdtlCont.ClosingPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastInQty", ItmtransdtlCont.LastInQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastInPackQty", ItmtransdtlCont.LastInPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastOutQty", ItmtransdtlCont.LastOutQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastOutPackQty", ItmtransdtlCont.LastOutPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RtnPackQty", ItmtransdtlCont.RtnPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RtnQty", ItmtransdtlCont.RtnQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RecPackQty", ItmtransdtlCont.RecPackQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RecQty", ItmtransdtlCont.RecQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ExpiredQty", ItmtransdtlCont.ExpiredQty, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ExpiryDate", ItmtransdtlCont.ExpiryDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("Remark", ItmtransdtlCont.Remark, SQLControl.EnumDataType.dtStringN)
                                    .AddField("LotNo", ItmtransdtlCont.LotNo, SQLControl.EnumDataType.dtStringN)
                                    .AddField("CityCode", ItmtransdtlCont.CityCode, SQLControl.EnumDataType.dtStringN)
                                    .AddField("SecCode", ItmtransdtlCont.SecCode, SQLControl.EnumDataType.dtStringN)
                                    .AddField("BinCode", ItmtransdtlCont.BinCode, SQLControl.EnumDataType.dtStringN)
                                    '.AddField("RowGuid", ItmtransdtlCont.RowGuid, SQLControl.EnumDataType.dtString)
                                    .AddField("CreateDate", ItmtransdtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateBy", ItmtransdtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("LastUpdate", ItmtransdtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("UpdateBy", ItmtransdtlCont.UpdateBy, SQLControl.EnumDataType.dtString)

                                    Select Case pType
                                        Case SQLControl.EnumSQLType.stInsert
                                            If blnFound = True And blnFlag = False Then
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.DocCode) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.ItemCode) & "' AND ItemName = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.ItemCode) & "' AND LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.LocID) & "'")
                                            Else
                                                If blnFound = False Then
                                                    .AddField("DocCode", ItmtransdtlCont.DocCode, SQLControl.EnumDataType.dtString)
                                                    .AddField("ItemCode", ItmtransdtlCont.ItemCode, SQLControl.EnumDataType.dtString)
                                                    .AddField("LocID", ItmtransdtlCont.LocID, SQLControl.EnumDataType.dtString)
                                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                                End If
                                            End If
                                        Case SQLControl.EnumSQLType.stUpdate
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.DocCode) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.ItemCode) & "' AND ItemName = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.ItemCode) & "' AND LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.LocID) & "'")

                                    End Select
                                Next
                                
                            End With
                            Try
                                'execute
                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True

                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                    'Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                                Else
                                    message = axExecute.Message.ToString()
                                    'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDetails", axExecute.Message & strSQL, axExecute.StackTrace)
                                Return False

                            Finally
                                objSQL.Dispose()
                            End Try
                        End If

                    End If
                End If
            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDetails", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDetails", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ItmtransdtlContlist = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal ItmtransdtlCont As Container.Itmtransdtl, ByRef message As String) As Boolean
            Return Save(ItmtransdtlCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal ItmtransdtlCont As Container.Itmtransdtl, ByRef message As String) As Boolean
            Return Save(ItmtransdtlCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function UpdateList(ByVal ItmtransdtlCont As List(Of Container.Itmtransdtl), ByRef message As String) As Boolean
            Return Save(ItmtransdtlCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        'Ivan,30 July,Add BatchSave
        Private Function BatchSave(ByVal ListItmtransdtlCont As List(Of Container.Itmtransdtl), ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()

            BatchSave = False
            Try
                If ListItmtransdtlCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()

                        If ListItmtransdtlCont.Count > 0 Then

                            strSQL = BuildDelete("Itmtransdtl WITH (ROWLOCK)", "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ListItmtransdtlCont(0).DocCode) & "'")
                            ListSQL.Add(strSQL)
                        End If

                    End If

                    For Each ItmTransDtlCont In ListItmtransdtlCont
                        With objSQL

                            .TableName = "Itmtransdtl WITH (ROWLOCK)"
                            .AddField("StorageID", ItmTransDtlCont.StorageID, SQLControl.EnumDataType.dtString)
                            .AddField("TermID", ItmTransDtlCont.TermID, SQLControl.EnumDataType.dtCustom)
                            .AddField("SeqNo", ItmTransDtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Status", ItmTransDtlCont.Status, SQLControl.EnumDataType.dtNumeric)
                            .AddField("TransType", ItmTransDtlCont.TransType, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OperationType", ItmTransDtlCont.OperationType, SQLControl.EnumDataType.dtString)
                            .AddField("ItmPrice", ItmTransDtlCont.ItmPrice, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ReqQty", ItmTransDtlCont.ReqQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ReqPackQty", ItmTransDtlCont.ReqPackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OpeningQty", ItmTransDtlCont.OpeningQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("OpeningPackQty", ItmTransDtlCont.OpeningPackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Qty", ItmTransDtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("PackQty", ItmTransDtlCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("HandlingQty", ItmTransDtlCont.HandlingQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("HandlingPackQty", ItmTransDtlCont.HandlingPackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ClosingQty", ItmTransDtlCont.ClosingQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ClosingPackQty", ItmTransDtlCont.ClosingPackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastInQty", ItmTransDtlCont.LastInQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastInPackQty", ItmTransDtlCont.LastInPackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastOutQty", ItmTransDtlCont.LastOutQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastOutPackQty", ItmTransDtlCont.LastOutPackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("RtnPackQty", ItmTransDtlCont.RtnPackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("RtnQty", ItmTransDtlCont.RtnQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("RecPackQty", ItmTransDtlCont.RecPackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("RecQty", ItmTransDtlCont.RecQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ExpiredQty", ItmTransDtlCont.ExpiredQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ExpiryDate", ItmTransDtlCont.ExpiryDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("Remark", ItmTransDtlCont.Remark, SQLControl.EnumDataType.dtStringN)
                            .AddField("LotNo", ItmTransDtlCont.LotNo, SQLControl.EnumDataType.dtStringN)
                            .AddField("CityCode", ItmTransDtlCont.CityCode, SQLControl.EnumDataType.dtStringN)
                            .AddField("SecCode", ItmTransDtlCont.SecCode, SQLControl.EnumDataType.dtStringN)
                            .AddField("BinCode", ItmTransDtlCont.BinCode, SQLControl.EnumDataType.dtStringN)
                            '.AddField("RowGuid", ItmTransDtlCont.RowGuid, SQLControl.EnumDataType.dtString)
                            .AddField("CreateDate", ItmTransDtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateBy", ItmTransDtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                            .AddField("LastUpdate", ItmTransDtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("UpdateBy", ItmTransDtlCont.UpdateBy, SQLControl.EnumDataType.dtString)

                            .AddField("DocCode", ItmTransDtlCont.DocCode, SQLControl.EnumDataType.dtString)
                            .AddField("ItemCode", ItmTransDtlCont.ItemCode, SQLControl.EnumDataType.dtString)
                            .AddField("LocID", ItmTransDtlCont.LocID, SQLControl.EnumDataType.dtString)
                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)

                            'Select Case pType
                            '    Case SQLControl.EnumSQLType.stInsert
                            '        If blnFound = True And blnFlag = False Then
                            '            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.DocCode) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.ItemCode) & "' AND LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.LocID) & "'")
                            '        Else
                            '            If blnFound = False Then
                            '                .AddField("DocCode", ItmTransDtlCont.DocCode, SQLControl.EnumDataType.dtString)
                            '                .AddField("ItemCode", ItmTransDtlCont.ItemCode, SQLControl.EnumDataType.dtString)
                            '                .AddField("LocID", ItmTransDtlCont.LocID, SQLControl.EnumDataType.dtString)
                            '                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            '            End If
                            '        End If
                            '    Case SQLControl.EnumSQLType.stUpdate
                            '        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.DocCode) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.ItemCode) & "' AND LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.LocID) & "'")
                            'End Select

                        End With

                        ListSQL.Add(strSQL)
                    Next

                    Try
                        'batch execute
                        objConn.BatchExecute(ListSQL, CommandType.Text)
                        Return True

                    Catch axExecute As Exception
                        Dim sqlStatement As String = " "
                        If objConn.FailedSQLStatement.Count > 0 Then
                            sqlStatement &= objConn.FailedSQLStatement.Item(0)
                        End If

                        If pType = SQLControl.EnumSQLType.stInsert Then
                            message = axExecute.Message.ToString()
                            'Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                        Else
                            message = axExecute.Message.ToString()
                            'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                        End If
                        Log.Notifier.Notify(axExecute)
                        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDetails", axExecute.Message & sqlStatement, axExecute.StackTrace)
                        Return False

                    Finally
                        objSQL.Dispose()
                    End Try

                End If
            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDetails", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDetails", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ListItmtransdtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
        'Private Function BatchSaveAdjust(ByVal ListItmtransdtlCont As List(Of Container.Itmtransdtl), ByVal pType As SQLControl.EnumSQLType, ByRef message As String, ByVal TermIDVal As Integer) As Boolean
        '    Dim strSQL As String = ""
        '    Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
        '    Dim rdr As System.Data.SqlClient.SqlDataReader
        '    Dim ListSQL As ArrayList = New ArrayList()

        '    BatchSaveAdjust = False
        '    Try
        '        If ListItmtransdtlCont Is Nothing Then
        '            'Message return
        '        Else
        '            blnExec = False
        '            blnFound = False
        '            blnFlag = False
        '            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
        '                StartSQLControl()

        '                'If ListItmtransdtlCont.Count > 0 Then

        '                '    strSQL = BuildDelete(ItmtransdtlInfo.MyInfo.TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString,ListItmtransdtlCont(0).DocCode) & "'")
        '                '    ListSQL.Add(strSQL)
        '                'End If

        '            End If

        '            For Each ItmTransDtlCont In ListItmtransdtlCont

        '                With objSQL
        '                    .TableName = "Itmtransdtl"
        '                    .AddField("StorageID", ItmTransDtlCont.StorageID, SQLControl.EnumDataType.dtString)
        '                    .AddField("TermID", ItmTransDtlCont.TermID, SQLControl.EnumDataType.dtCustom)
        '                    .AddField("SeqNo", ItmTransDtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("Status", ItmTransDtlCont.Status, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("TransType", ItmTransDtlCont.TransType, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("OperationType", ItmTransDtlCont.OperationType, SQLControl.EnumDataType.dtString)
        '                    .AddField("ItmPrice", ItmTransDtlCont.ItmPrice, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("ReqQty", ItmTransDtlCont.ReqQty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("ReqPackQty", ItmTransDtlCont.ReqPackQty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("OpeningQty", ItmTransDtlCont.OpeningQty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("OpeningPackQty", ItmTransDtlCont.OpeningPackQty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("Qty", ItmTransDtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("PackQty", ItmTransDtlCont.PackQty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("HandlingQty", ItmTransDtlCont.HandlingQty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("HandlingPackQty", ItmTransDtlCont.HandlingPackQty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("ClosingQty", ItmTransDtlCont.ClosingQty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("ClosingPackQty", ItmTransDtlCont.ClosingPackQty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("LastInQty", ItmTransDtlCont.LastInQty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("LastInPackQty", ItmTransDtlCont.LastInPackQty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("LastOutQty", ItmTransDtlCont.LastOutQty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("LastOutPackQty", ItmTransDtlCont.LastOutPackQty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("RtnPackQty", ItmTransDtlCont.RtnPackQty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("RtnQty", ItmTransDtlCont.RtnQty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("RecPackQty", ItmTransDtlCont.RecPackQty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("RecQty", ItmTransDtlCont.RecQty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("ExpiredQty", ItmTransDtlCont.ExpiredQty, SQLControl.EnumDataType.dtNumeric)
        '                    .AddField("ExpiryDate", ItmTransDtlCont.ExpiryDate, SQLControl.EnumDataType.dtDateTime)
        '                    .AddField("Remark", ItmTransDtlCont.Remark, SQLControl.EnumDataType.dtStringN)
        '                    .AddField("LotNo", ItmTransDtlCont.LotNo, SQLControl.EnumDataType.dtStringN)
        '                    .AddField("CityCode", ItmTransDtlCont.CityCode, SQLControl.EnumDataType.dtStringN)
        '                    .AddField("SecCode", ItmTransDtlCont.SecCode, SQLControl.EnumDataType.dtStringN)
        '                    .AddField("BinCode", ItmTransDtlCont.BinCode, SQLControl.EnumDataType.dtStringN)
        '                    '.AddField("RowGuid", ItmTransDtlCont.RowGuid, SQLControl.EnumDataType.dtString)
        '                    .AddField("CreateDate", ItmTransDtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
        '                    .AddField("CreateBy", ItmTransDtlCont.CreateBy, SQLControl.EnumDataType.dtString)
        '                    .AddField("LastUpdate", ItmTransDtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
        '                    .AddField("UpdateBy", ItmTransDtlCont.UpdateBy, SQLControl.EnumDataType.dtString)
        '                    .AddField("SyncCreate", ItmTransDtlCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
        '                    .AddField("SyncLastUpd", ItmTransDtlCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)

        '                    Select Case pType
        '                        Case SQLControl.EnumSQLType.stInsert
        '                            .AddField("Qty", ItmTransDtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
        '                            If blnFound = True And blnFlag = False Then
        '                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.DocCode) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.ItemCode) & "' AND LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.LocID) & "'")
        '                            Else
        '                                If blnFound = False Then
        '                                    .AddField("DocCode", ItmTransDtlCont.DocCode, SQLControl.EnumDataType.dtString)
        '                                    .AddField("ItemCode", ItmTransDtlCont.ItemCode, SQLControl.EnumDataType.dtString)
        '                                    .AddField("LocID", ItmTransDtlCont.LocID, SQLControl.EnumDataType.dtString)
        '                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
        '                                End If
        '                            End If
        '                            If TermIDVal = 0 And ItmTransDtlCont.Qty >= 0 Then
        '                                ListSQL.Add(strSQL)
        '                            End If
        '                            If TermIDVal = 1 And ItmTransDtlCont.Qty < 0 Then
        '                                ListSQL.Add(strSQL)
        '                            End If
        '                        Case SQLControl.EnumSQLType.stUpdate
        '                            If TermIDVal = 0 And ItmTransDtlCont.Qty < 0 Then
        '                                .AddField("Qty", 0, SQLControl.EnumDataType.dtNumeric)
        '                            End If
        '                            If TermIDVal = 1 And ItmTransDtlCont.Qty >= 0 Then
        '                                .AddField("Qty", 0, SQLControl.EnumDataType.dtNumeric)
        '                            End If
        '                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.DocCode) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.ItemCode) & "' AND LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.LocID) & "'")
        '                            ListSQL.Add(strSQL)
        '                    End Select

        '                End With



        '            Next

        '            Try
        '                'batch execute
        '                objConn.BatchExecute(ListSQL, CommandType.Text)
        '            Catch axExecute As Exception
        '                If pType = SQLControl.EnumSQLType.stInsert Then
        '                    message = axExecute.Message.ToString()
        '                    Throw New ApplicationException("210002 " & axExecute.Message.ToString())
        '                Else
        '                    message = axExecute.Message.ToString()
        '                    Throw New ApplicationException("210004 " & axExecute.Message.ToString())
        '                End If
        '            Finally
        '                objSQL.Dispose()
        '            End Try
        '            Return True

        '        End If
        '    Catch axAssign As ApplicationException
        '        'Throw axAssign
        '        message = axAssign.Message.ToString()
        '        Return False
        '    Catch exAssign As SystemException
        '        'Throw exAssign
        '        message = exAssign.Message.ToString()
        '        Return False
        '    Finally
        '        ListItmtransdtlCont = Nothing
        '        rdr = Nothing
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function
        'Ivan,30 July,Add BatchInsert

        Public Function BatchInsert(ByVal ListItmtransdtlCont As List(Of Container.Itmtransdtl), ByRef message As String) As Boolean
            Return BatchSave(ListItmtransdtlCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'Public Function BatchInsertAdjust(ByVal ListItmtransdtlCont As List(Of Container.Itmtransdtl), ByRef message As String, ByVal TermIDVal As Integer) As Boolean
        '    Return BatchSaveAdjust(ListItmtransdtlCont, SQLControl.EnumSQLType.stInsert, message, TermIDVal)
        'End Function

        'Ivan,30 July,Add BatchUpdate
        Public Function BatchUpdate(ByVal ListItmtransdtlCont As List(Of Container.Itmtransdtl), ByRef message As String) As Boolean
            Return BatchSave(ListItmtransdtlCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        'Public Function BatchUpdateAdjust(ByVal ListItmtransdtlCont As List(Of Container.Itmtransdtl), ByRef message As String, ByVal TermIDVal As Integer) As Boolean
        '    Return BatchSaveAdjust(ListItmtransdtlCont, SQLControl.EnumSQLType.stUpdate, message, TermIDVal)
        'End Function

        Public Function Delete(ByVal ItmtransdtlCont As Container.Itmtransdtl, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If ItmtransdtlCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItmtransdtlInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.DocCode) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.ItemCode) & "'")
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
                                strSQL = BuildUpdate("Itmtransdtl WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtransdtlCont.UpdateBy) & "' WHERE" & _
                                "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.DocCode) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.ItemCode) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("Itmtransdtl WITH (ROWLOCK)", "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.DocCode) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtransdtlCont.ItemCode) & "'")
                        End If

                        Try
                            'execute
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDetails", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                            'Throw New ApplicationException("210006 " & exExecute.Message.ToString())
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDetails", axDelete.Message, axDelete.StackTrace)
                Return False
                'Throw axDelete
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDetails", exDelete.Message, exDelete.StackTrace)
                Return False
                'Throw exDelete
            Finally
                ItmtransdtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'Ivan,6 Agus 2014, UpdateStockItemLoc
        Public Function UpdateStockItemLoc(ByVal DocCode As String, ByRef message As String) As Boolean
            Dim strSQL As String = ""

            Try
                StartSQLControl()

                strSQL = "Exec sp_LiveInv '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, DocCode) & "'"

                'execute
                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                Return True

            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDetails", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDetails", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally

                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'Richard 2014-09-18 Adjustment
        Public Function UpdateStockItemLocAdjust(ByVal DocCode As String, ByRef message As String) As Boolean
            Dim strSQL As String = ""

            Try
                StartSQLControl()

                strSQL = "Exec sp_LiveInvAdj '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, DocCode) & "'"

                'execute
                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                Return True

            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDetails", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDetails", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally

                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'Lily 2016-05-20 assign calculation to ItmTransDTL container for addition
        Public Function CalculateAdditionDTL(ByRef ListItmTransDTL As List(Of Container.Itmtransdtl)) As Boolean
            If ListItmTransDTL IsNot Nothing AndAlso ListItmTransDTL.Count > 0 Then
                For Each TransDTL In ListItmTransDTL
                    If Not IsDBNull(TransDTL.Qty) And Val(TransDTL.Qty) > 0 Then
                        With TransDTL
                            .ClosingQty = .OpeningQty + .Qty
                            .ClosingPackQty = .ClosingQty * 1000
                        End With
                    End If
                Next
                Return True
            Else
                Return False
            End If
        End Function

        'Lily 2016-02-06 assign calculation to ItmTransDTL container for reuse
        Public Function CalculateReusedDTL(ByRef ListItmTransDTL As List(Of Container.Itmtransdtl)) As Boolean
            If ListItmTransDTL IsNot Nothing AndAlso ListItmTransDTL.Count > 0 Then
                For Each TransDTL In ListItmTransDTL
                    If Not IsDBNull(TransDTL.Qty) And Val(TransDTL.Qty) > 0 Then
                        With TransDTL
                            .ClosingQty = .OpeningQty - .Qty
                            .ClosingPackQty = .ClosingQty * 1000
                        End With
                    End If
                Next
                Return True
            Else
                Return False
            End If
        End Function

        'Lily 2016-05-20 assign calculation to Itmtransdtl container for adjustment
        Public Function CalculateAdjustDTL(ByRef ListItmTransDTL As List(Of Container.Itmtransdtl)) As Boolean
            Dim TempList As New List(Of Container.Itmtransdtl)

            If ListItmTransDTL IsNot Nothing AndAlso ListItmTransDTL.Count > 0 Then
                For Each TransDTL In ListItmTransDTL
                    'If Not IsDBNull(TransDTL.ClosingQty) And Val(TransDTL.ClosingQty) > 0 Then
                    With TransDTL
                        .Qty = .ClosingQty - .OpeningQty
                        .PackQty = .ClosingPackQty - (.OpeningQty * 1000)
                        If .Qty >= 0 Then
                            .TransType = 0
                        Else
                            .TransType = 1
                            .Qty = 0 - .Qty
                            .PackQty = 0 - .PackQty
                        End If
                    End With
                    'End If
                Next
                For Each TransDTL In ListItmTransDTL
                    If TransDTL.Qty <> 0 Then
                        TempList.Add(TransDTL)
                    End If
                Next
                ListItmTransDTL = TempList
                Return True
            Else
                Return False
            End If
        End Function

        ''Lily 2016-05-20 assign calculation and itemLoc info to ItmTransDTL container
        ' ''used if all assign move to logic
        'Public Function AssignQtyDTL(ByRef ListItmTransDTL As List(Of Container.Itmtransdtl)) As Boolean
        '    Dim seqNo As Integer = 1
        '    Dim objItemLoc As ItemLoc
        '    Dim ItemLoc As Container.Itemloc
        '    Dim objSysPreft As GeneralSettings.SysPreft
        '    Dim SysPreft As String

        '    For Each TransDTL In ListItmTransDTL
        '        If Not IsDBNull(TransDTL.Qty) And Val(TransDTL.Qty) > 0 Then
        '            With TransDTL
        '                'Get stock on ItemLoc
        '                objItemLoc = New Actions.ItemLoc(ConnectionString)
        '                ItemLoc = objItemLoc.GetItemLocCustom(.LocID, .ItemCode, .ItemName)
        '                If ItemLoc IsNot Nothing Then
        '                    .LastInQty = ItemLoc.LastIn
        '                    .OpeningQty = ItemLoc.QtyOnHand
        '                    .ExpiredQty = Val(.Qty)
        '                    'If TransDTL.Unit = "0" Then
        '                    '    .Qty = Val(TransDTL.Qty)
        '                    '    .PackQty = Val(TransDTL.Qty) * 1000
        '                    'ElseIf TransDTL.Unit = "1" Then
        '                    '    .Qty = Val(TransDTL.Qty) / 1000
        '                    '    .PackQty = Val(TransDTL.Qty)
        '                    'End If
        '                    .ClosingQty = .OpeningQty + .Qty
        '                    .ClosingPackQty = .ClosingQty * 1000

        '                End If

        '                .StorageID = ""
        '                .SeqNo = seqNo
        '                objSysPreft = New GeneralSettings.SysPreft(ConnectionString)
        '                SysPreft = objSysPreft.GetSingleSysPreft(1, "INV", "STORAGEDURATION")
        '                .ExpiryDate = Now.Date.AddDays(Integer.Parse(SysPreft)) 'to set expiry date from 180 days

        '                .Status = SaveType.Submit

        '                If .Qty >= 0 Then
        '                    .TransType = TransType.TransIn
        '                Else
        '                    .TransType = TransType.TransOut
        '                    .Qty = 0 - .Qty
        '                    .PackQty = 0 - .PackQty
        '                End If

        '                .CreateDate = Now
        '                .SyncCreate = Now
        '                '.CreateBy = UserID
        '            End With
        '            seqNo = seqNo + 1
        '        End If
        '    Next
        'End Function

        ''Lily 2016-05-20 assign calculation and itemLoc info to AdjustmentDTL container
        ' ''used if all assign move to logic
        'Public Function AssignQtyAdjustDTL(ByRef ListItmTransDTL As List(Of Container.Itmtransdtl)) As Boolean
        '    Dim seqNo As Integer = 1
        '    Dim objItemLoc As ItemLoc
        '    Dim ItemLoc As Container.Itemloc
        '    Dim objSysPreft As GeneralSettings.SysPreft
        '    Dim SysPreft As String

        '    For Each TransDTL In ListItmTransDTL
        '        With TransDTL
        '            'Get stock on ItemLoc
        '            objItemLoc = New Actions.ItemLoc(ConnectionString)
        '            ItemLoc = objItemLoc.GetItemLocCustom(.LocID, .ItemCode, .ItemName)
        '            If ItemLoc IsNot Nothing Then
        '                .LastInQty = ItemLoc.LastIn
        '                .LastOutQty = ItemLoc.LastOut
        '                .OpeningQty = ItemLoc.QtyOnHand
        '                'If TransDTL.Unit = "0" Then  'MT
        '                '    .ClosingQty = Val(TransDTL.Qty)
        '                '    .ClosingPackQty = Val(TransDTL.Qty) * 1000
        '                '    '.Qty = Val(TransDTL.Qty) - ItemLoc.QtyOnHand
        '                '    '.PackQty = (Val(TransDTL.Qty) - ItemLoc.QtyOnHand) * 1000
        '                'ElseIf TransDTL.Unit = "1" Then  'KG
        '                '    .ClosingQty = Val(TransDTL.Qty) / 1000
        '                '    .ClosingPackQty = Val(TransDTL.Qty)
        '                '    '.Qty = (Val(TransDTL.Qty) - ItemLoc.QtyOnHand) / 1000
        '                '    '.PackQty = Val(TransDTL.Qty) - ItemLoc.QtyOnHand
        '                'End If
        '                .Qty = .ClosingQty - .OpeningQty
        '                .PackQty = .ClosingPackQty - (.OpeningQty * 1000)
        '            End If
        '            .StorageID = ""
        '            .SeqNo = seqNo
        '            .Status = SaveType.Submit

        '            If .Qty >= 0 Then
        '                .TransType = TransType.TransIn
        '            Else
        '                .TransType = TransType.TransOut
        '                .Qty = 0 - .Qty
        '                .PackQty = 0 - .PackQty
        '            End If

        '            .CreateDate = Now
        '            .SyncCreate = Now
        '            '.CreateBy = UserID
        '        End With
        '        seqNo = seqNo + 1
        '    Next
        'End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetTransactionDetails(ByVal DocCode As System.String, ByVal ItemCode As System.String) As Container.Itmtransdtl
            Dim rItmtransdtl As Container.Itmtransdtl = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With ItmtransdtlInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, DocCode) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rItmtransdtl = New Container.Itmtransdtl
                                rItmtransdtl.DocCode = drRow.Item("DocCode")
                                rItmtransdtl.ItemCode = drRow.Item("ItemCode")
                                rItmtransdtl.LocID = drRow.Item("LocID")
                                rItmtransdtl.StorageID = drRow.Item("StorageID")
                                rItmtransdtl.TermID = drRow.Item("TermID")
                                rItmtransdtl.SeqNo = drRow.Item("SeqNo")
                                rItmtransdtl.Status = drRow.Item("Status")
                                rItmtransdtl.TransType = drRow.Item("TransType")
                                rItmtransdtl.OperationType = drRow.Item("OperationType")
                                rItmtransdtl.ItmPrice = drRow.Item("ItmPrice")
                                rItmtransdtl.ReqQty = drRow.Item("ReqQty")
                                rItmtransdtl.ReqPackQty = drRow.Item("ReqPackQty")
                                rItmtransdtl.OpeningQty = drRow.Item("OpeningQty")
                                rItmtransdtl.OpeningPackQty = drRow.Item("OpeningPackQty")
                                rItmtransdtl.Qty = drRow.Item("Qty")
                                rItmtransdtl.PackQty = drRow.Item("PackQty")
                                rItmtransdtl.HandlingQty = drRow.Item("HandlingQty")
                                rItmtransdtl.HandlingPackQty = drRow.Item("HandlingPackQty")
                                rItmtransdtl.ClosingQty = drRow.Item("ClosingQty")
                                rItmtransdtl.ClosingPackQty = drRow.Item("ClosingPackQty")
                                rItmtransdtl.LastInQty = drRow.Item("LastInQty")
                                rItmtransdtl.LastInPackQty = drRow.Item("LastInPackQty")
                                rItmtransdtl.LastOutQty = drRow.Item("LastOutQty")
                                rItmtransdtl.LastOutPackQty = drRow.Item("LastOutPackQty")
                                rItmtransdtl.RtnPackQty = drRow.Item("RtnPackQty")
                                rItmtransdtl.RtnQty = drRow.Item("RtnQty")
                                rItmtransdtl.RecPackQty = drRow.Item("RecPackQty")
                                rItmtransdtl.RecQty = drRow.Item("RecQty")
                                rItmtransdtl.ExpiredQty = drRow.Item("ExpiredQty")
                                rItmtransdtl.Remark = drRow.Item("Remark")
                                rItmtransdtl.LotNo = drRow.Item("LotNo")
                                rItmtransdtl.CityCode = drRow.Item("CityCode")
                                rItmtransdtl.SecCode = drRow.Item("SecCode")
                                rItmtransdtl.BinCode = drRow.Item("BinCode")
                                rItmtransdtl.RowGuid = drRow.Item("RowGuid")
                                rItmtransdtl.CreateDate = drRow.Item("CreateDate")
                                rItmtransdtl.CreateBy = drRow.Item("CreateBy")
                                rItmtransdtl.SyncCreate = drRow.Item("SyncCreate")
                                rItmtransdtl.ItemName = drRow.Item("ItemName")

                            Else
                                rItmtransdtl = Nothing
                            End If
                        Else
                            rItmtransdtl = Nothing
                        End If
                    End With
                End If
                Return rItmtransdtl
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDetails", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rItmtransdtl = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTransactionDetails(ByVal DocCode As System.String, ByVal ItemCode As System.String, DecendingOrder As Boolean) As List(Of Container.Itmtransdtl)
            Dim rItmtransdtl As Container.Itmtransdtl = Nothing
            Dim lstItmtransdtl As List(Of Container.Itmtransdtl) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With ItmtransdtlInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal DocCode As System.String, ByVal ItemCode As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, DocCode) & "' AND ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rItmtransdtl = New Container.Itmtransdtl
                                rItmtransdtl.DocCode = drRow.Item("DocCode")
                                rItmtransdtl.ItemCode = drRow.Item("ItemCode")
                                rItmtransdtl.LocID = drRow.Item("LocID")
                                rItmtransdtl.StorageID = drRow.Item("StorageID")
                                rItmtransdtl.TermID = drRow.Item("TermID")
                                rItmtransdtl.SeqNo = drRow.Item("SeqNo")
                                rItmtransdtl.Status = drRow.Item("Status")
                                rItmtransdtl.TransType = drRow.Item("TransType")
                                rItmtransdtl.OperationType = drRow.Item("OperationType")
                                rItmtransdtl.ItmPrice = drRow.Item("ItmPrice")
                                rItmtransdtl.ReqQty = drRow.Item("ReqQty")
                                rItmtransdtl.ReqPackQty = drRow.Item("ReqPackQty")
                                rItmtransdtl.OpeningQty = drRow.Item("OpeningQty")
                                rItmtransdtl.OpeningPackQty = drRow.Item("OpeningPackQty")
                                rItmtransdtl.Qty = drRow.Item("Qty")
                                rItmtransdtl.PackQty = drRow.Item("PackQty")
                                rItmtransdtl.HandlingQty = drRow.Item("HandlingQty")
                                rItmtransdtl.HandlingPackQty = drRow.Item("HandlingPackQty")
                                rItmtransdtl.ClosingQty = drRow.Item("ClosingQty")
                                rItmtransdtl.ClosingPackQty = drRow.Item("ClosingPackQty")
                                rItmtransdtl.LastInQty = drRow.Item("LastInQty")
                                rItmtransdtl.LastInPackQty = drRow.Item("LastInPackQty")
                                rItmtransdtl.LastOutQty = drRow.Item("LastOutQty")
                                rItmtransdtl.LastOutPackQty = drRow.Item("LastOutPackQty")
                                rItmtransdtl.RtnPackQty = drRow.Item("RtnPackQty")
                                rItmtransdtl.RtnQty = drRow.Item("RtnQty")
                                rItmtransdtl.RecPackQty = drRow.Item("RecPackQty")
                                rItmtransdtl.RecQty = drRow.Item("RecQty")
                                rItmtransdtl.ExpiredQty = drRow.Item("ExpiredQty")
                                rItmtransdtl.Remark = drRow.Item("Remark")
                                rItmtransdtl.LotNo = drRow.Item("LotNo")
                                rItmtransdtl.CityCode = drRow.Item("CityCode")
                                rItmtransdtl.SecCode = drRow.Item("SecCode")
                                rItmtransdtl.BinCode = drRow.Item("BinCode")
                                rItmtransdtl.RowGuid = drRow.Item("RowGuid")
                                rItmtransdtl.CreateDate = drRow.Item("CreateDate")
                                rItmtransdtl.CreateBy = drRow.Item("CreateBy")
                                rItmtransdtl.SyncCreate = drRow.Item("SyncCreate")
                                rItmtransdtl.ItemName = drRow.Item("ItemName")

                            Next
                            lstItmtransdtl.Add(rItmtransdtl)
                        Else
                            rItmtransdtl = Nothing
                        End If
                        Return lstItmtransdtl
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDetails", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rItmtransdtl = Nothing
                lstItmtransdtl = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTransactionListCustom(ByVal posted As String, ByVal Location As String, ByVal DocCode As String) As Data.DataTable
            If StartConnection() Then
                With ItmtransdtlInfo.MyInfo
                    If Not DocCode Is Nothing Then
                        If posted = "1" Then
                            strSQL = "SELECT I.Remark, ROW_NUMBER() OVER (ORDER BY L.ITEMCODE) as DocCode, ROW_NUMBER() OVER (ORDER BY L.ITEMCODE) as RecNo, " & _
                                     " I.LocID,I.ItemName, I.ItemCode AS ItemCode, CASE WHEN I.Status = 1 THEN I.OpeningQty ELSE L.QtyOnHand END AS QtyOnHand , " & _
                                     " I.Qty, I.PackQty, CASE WHEN I.Status=1 THEN I.ClosingQty ELSE (L.QtyOnHand-I.Qty) END AS ClosingQty, " & _
                                     " CASE WHEN I.Status=1 THEN I.LastInQty ELSE L.LastIn END AS LastIn, " & _
                                     " CASE WHEN I.Status=1 THEN I.LastOutQty ELSE L.LastOut END AS LastOut, " & _
                                     " /*S.StorageID*/ '' as StorageID, L.ShortDesc as Remarks, I.Status " & _
                                     " from ITEMLOC L WITH (NOLOCK) INNER JOIN " & _
                                     " ITMTRANSDTL I WITH (NOLOCK) ON I.LocID=L.LocID AND  I.ItemCode=L.ItemCode and I.ItemName=L.ItemName and  I.DocCode='" & DocCode & "' " & _
                                     " /*Left Join storagemaster S on S.LocID = I.LocID and S.ItemCode = I.ItemCode*/ " & _
                                     " ORDER BY L.ItemCode ASC"
                        Else
                            strSQL = "SELECT I.Remark, ROW_NUMBER() OVER (ORDER BY L.ITEMCODE) as DocCode, ROW_NUMBER() OVER (ORDER BY L.ITEMCODE) as RecNo, " & _
                                     " L.LocID,L.ItemName, L.ItemCode AS ItemCode, CASE WHEN I.Status = 1 THEN ISNULL(I.OpeningQty,0) ELSE L.QtyOnHand END AS QtyOnHand , " & _
                                     " I.Qty AS Qty, I.PackQty AS PackQty, CASE WHEN I.Status=1 THEN ISNULL(I.ClosingQty,0) ELSE ISNULL((L.QtyOnHand-I.Qty),0) END AS ClosingQty, " & _
                                     " CASE WHEN I.Status=1 THEN ISNULL(I.LastInQty,0) ELSE L.LastIn END AS LastIn, " & _
                                     " CASE WHEN I.Status=1 THEN ISNULL(I.LastOutQty,0) ELSE L.LastOut END AS LastOut, " & _
                                     " /*ISNULL(S.StorageID,'')*/ '' as StorageID, L.ShortDesc as Remarks, ISNULL(I.Status,0) AS Status " & _
                                     " from ITEMLOC L WITH (NOLOCK) LEFT JOIN " & _
                                     " ITMTRANSDTL I WITH (NOLOCK) ON I.LocID=L.LocID and I.ItemName=L.ItemName AND  I.ItemCode=L.ItemCode and  I.DocCode='" & DocCode & "'" & _
                                     " /*Left Join storagemaster S on S.LocID = I.LocID and S.ItemCode = I.ItemCode*/ " & _
                                     " WHERE L.Flag=1 AND L.LocId='" & Location & "' ORDER BY L.ItemCode ASC"
                        End If

                    Else
                        strSQL = "SELECT '' as Remark, ROW_NUMBER() OVER (ORDER BY L.ITEMCODE) as DocCode, ROW_NUMBER() OVER (ORDER BY L.ITEMCODE) as RecNo," & _
                                 " L.ItemCode,L.ItemName, L.QtyOnHand , CAST(NULL AS NUMERIC(10,4)) as Qty, CAST(NULL AS NUMERIC(10,4)) as PackQty, L.QtyOnHand as ClosingQty," & _
                                 " L.LastIn, L.LastOut, " & _
                                 " /*S.StorageID*/ '' as StorageID, L.ShortDesc as Remarks, 0 AS Posted " & _
                                 " from ITEMLOC L WITH (NOLOCK) " & _
                                 " /*LEFT JOIN STORAGEMASTER S on L.StorageID = S.StorageID AND L.LocID = S.LocID*/ " & _
                                 " where L.LocId='" & Location & "' AND L.Flag=1 ORDER BY L.ItemCode ASC"
                    End If
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetTransactionDetailsList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ItmtransdtlInfo.MyInfo
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

        Public Overloads Function GetTransactionDetailsShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With ItmtransdtlInfo.MyInfo
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

        Public Overloads Function GetTransactionDetailsAdjustmentList(Optional ByVal condition As String = Nothing, Optional ByVal docCodeVal As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ItmtransdtlInfo.MyInfo
                    strSQL = "select ITEMLOC.ItemCode as ItemCode, STORAGEMASTER.LocID as LocID,STORAGEMASTER.CityCode as Location, STORAGEMASTER.StorageAreaCode as StorageArea, STORAGEMASTER.StorageBin as Bin, ITEMLOC.IncomingQty as Qty, ITEMLOC.PackQty as PackQty,ITEMLOC.QtyOnHand as QtyOnHand, (ITEMLOC.IncomingQty - ITEMLOC.QtyOnHand) as Variance, BIZLOCATE.BizRegID as CompanyID, ITEMLOC.OutgoingQty as OutgoingQty from ITEMLOC, STORAGEMASTER LEFT JOIN BIZLOCATE ON BIZLOCATE.BizLocID = STORAGEMASTER.LocID where ITEMLOC.ItemCode = STORAGEMASTER.ItemCode AND STORAGEMASTER.LocID = ITEMLOC.LocID " & condition

                    If Not docCodeVal Is Nothing Then
                        strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY L.ITEMCODE) as DocCode,I.LocID as LocID, S.CityCode as Location, L.ItemCode,(SELECT Address1  FROM BizLocate WHERE bizlocid=s.LocID) AS CityCode,S.StorageAreaCode as StorageArea , " & _
                                                                    " S.storagebin as Bin, I.qty as Variance,I.qty as QtyBefore, I.Packqty as PackQty, L.QtyOnHand , L.OutgoingQty , " & _
                                                                    " L.IncomingQty as InQty, " & _
                                                                    " L.ShortDesc as Remarks,L.LastIn,L.IncomingQty, S.StorageID as StorageID, I.OperationType as Handling,(L.QtyOnHand+I.Qty) as Qty from ITEMLOC L, " & _
                                                                    " storagemaster S, ITMTRANSDTL I  where I.LocID=L.LocID AND I.ItemCode=L.ItemCode and L.StorageID = S.StorageID and L.Flag=1 AND  I.LocID=S.LocID AND I.DocCode='" & docCodeVal & "' order by QtyOnHand desc"
                    End If
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetTransactionDetailsOverdueList(Optional ByVal location As String = Nothing, Optional ByVal state As String = Nothing, Optional ByVal transDate As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With ItmtransdtlInfo.MyInfo
                    strSQL = "SELECT d.DocCode, d.LocID, d.ItemCode, d.ItemName, d.ExpiredQty, d.ExpiryDate, l.QtyOnHand As ClosingQty, d.CreateDate, 'SOLID' as WasteType FROM ITMTRANSDTL d " & _
                                " INNER JOIN (SELECT *, ROW_NUMBER() OVER(PARTITION BY DocCode ORDER BY DocCode) AS RecNo FROM ITMTRANSHDR) h on d.DocCode = h.DocCode" & _
                                " INNER JOIN ITEMLOC l on d.LocID = l.LocID AND d.ItemCode = l.ItemCode AND d.ItemName = l.ItemName" & _
                                " WHERE d.ExpiredQty > 0 And d.ExpiryDate <= GetDate() And h.FLAG = 1 And h.TermID = 0 And h.RecNo = 1 "

                    If location IsNot Nothing AndAlso location <> "ALL" Then
                        strSQL &= " AND d.LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, location) & "'"
                    ElseIf state IsNot Nothing Then
                        strSQL &= " AND d.Remark != '' AND COMPANYID IN (SELECT BIZREGID FROM BIZLOCATE WHERE STATE = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, state) & "')"
                    Else
                        strSQL &= " AND d.Remark != ''"
                    End If

                    If transDate IsNot Nothing AndAlso transDate <> "" Then
                        strSQL &= transDate
                    End If

                    strSQL &= " ORDER BY TransDate DESC"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetSumZeroWaste(ByVal DocCode As System.String) As Container.Itmtransdtl
            Dim rItmtransdtl As Container.Itmtransdtl = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With ItmtransdtlInfo.MyInfo
                        strSQL = "select sum(qty) AS QTY from itmtransdtl where doccode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, DocCode) & "'"
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rItmtransdtl = New Container.Itmtransdtl
                                rItmtransdtl.Qty = IIf(IsDBNull(drRow.Item("Qty")), "0", drRow.Item("Qty"))
                            Else
                                rItmtransdtl = Nothing
                            End If
                        Else
                            rItmtransdtl = Nothing
                        End If
                    End With
                End If
                Return rItmtransdtl
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/TransactionDTL", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rItmtransdtl = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function InventoryLoadGrid(ByRef sql As String, ByVal DocCodeDetail As String, ByVal UserLocation As String, ByVal opt As String, Optional ByVal FieldCond As String = Nothing)

            Select Case opt

                Case "posted"
                    sql = "SELECT ROW_NUMBER() OVER (ORDER BY L.ITEMCODE) as DocCode, ROW_NUMBER() OVER (ORDER BY L.ITEMCODE) as RecNo, " &
                          " I.LocID,L.ItemName, I.ItemCode AS ItemCode, CASE WHEN I.Status = 1 THEN I.OpeningQty ELSE L.QtyOnHand END AS QtyOnHand , " &
                          " I.Qty, I.PackQty, CASE WHEN I.Status=1 THEN I.ClosingQty ELSE (L.QtyOnHand+I.Qty) END AS ClosingQty, " &
                          " CASE WHEN I.Status=1 THEN I.LastInQty ELSE L.LastIn END AS LastIn, " &
                          " CASE WHEN I.Status=1 THEN I.LastOutQty ELSE L.LastOut END AS LastOut, " &
                          " /*S.StorageID*/ '' as StorageID, L.ShortDesc as Remarks, I.Status " &
                          " from ITEMLOC L WITH (NOLOCK) INNER JOIN " &
                          " ITMTRANSDTL I WITH (NOLOCK) ON I.LocID=L.LocID and I.ItemName=L.ItemName AND  I.ItemCode=L.ItemCode and  I.DocCode='" & DocCodeDetail & "' " &
                          " /*Left Join storagemaster S on S.LocID = I.LocID and S.ItemCode = I.ItemCode*/ " &
                          " WHERE L.Flag=1 AND L.IsSelected<>1 AND L.LocId='" & UserLocation & "' ORDER BY L.ItemCode ASC"

                Case "notposted"
                    sql = "SELECT ROW_NUMBER() OVER (ORDER BY L.ITEMCODE) as DocCode, ROW_NUMBER() OVER (ORDER BY L.ITEMCODE) as RecNo, " &
                          " L.LocID,L.ItemName, L.ItemCode AS ItemCode, CASE WHEN I.Status = 1 THEN ISNULL(I.OpeningQty,0) ELSE L.QtyOnHand END AS QtyOnHand , " &
                          " I.Qty AS Qty, I.PackQty AS PackQty, CASE WHEN I.Status=1 THEN ISNULL(I.ClosingQty,0) ELSE ISNULL((L.QtyOnHand+I.Qty),0) END AS ClosingQty, " &
                          " CASE WHEN I.Status=1 THEN ISNULL(I.LastInQty,0) ELSE L.LastIn END AS LastIn, " &
                          " CASE WHEN I.Status=1 THEN ISNULL(I.LastOutQty,0) ELSE L.LastOut END AS LastOut, " &
                          " /*ISNULL(S.StorageID,'')*/ '' as StorageID, L.ShortDesc as Remarks, ISNULL(I.Status,0) AS Status " &
                          " from ITEMLOC L WITH (NOLOCK) LEFT JOIN " &
                          " ITMTRANSDTL I WITH (NOLOCK) ON I.LocID=L.LocID and I.ItemName=L.ItemName AND  I.ItemCode=L.ItemCode and  I.DocCode='" & DocCodeDetail & "' " &
                          " /*Left Join storagemaster S on S.LocID = I.LocID and S.ItemCode = I.ItemCode*/ " &
                          " WHERE L.Flag=1 AND L.LocId='" & UserLocation & "' AND L.IsSelected<>1 ORDER BY L.ItemCode ASC"

                Case "DocCodeDetailFill"
                    sql = "SELECT ROW_NUMBER() OVER (ORDER BY L.ITEMCODE) as DocCode, ROW_NUMBER() OVER (ORDER BY L.ITEMCODE) as RecNo," &
                          " L.ItemCode,L.ItemName, L.QtyOnHand , CAST(NULL AS NUMERIC(10,4)) as Qty, CAST(NULL AS NUMERIC(10,4)) as PackQty, L.QtyOnHand as ClosingQty," &
                          " L.LastIn, L.LastOut, /*S.StorageID*/ '' as StorageID, L.ShortDesc as Remarks, 0 AS Posted " &
                          " from ITEMLOC L WITH (NOLOCK) " &
                          " /*LEFT JOIN STORAGEMASTER S on L.StorageID = S.StorageID AND L.LocID = S.LocID*/  where L.Flag=1 AND L.IsSelected<>1 AND L.LocId='" & UserLocation & "' ORDER BY L.ItemCode ASC"
            End Select

            If StartConnection() = True Then
                With ItmtransdtlInfo.MyInfo
                    If sql = Nothing Or sql = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName, FieldCond)
                    Else
                        strSQL = sql
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
        Public Class Itmtransdtl
            Public fDocCode As System.String = "DocCode"
            Public fItemCode As System.String = "ItemCode"
            Public fLocID As System.String = "LocID"
            Public fStorageID As System.String = "StorageID"
            Public fTermID As System.String = "TermID"
            Public fSeqNo As System.String = "SeqNo"
            Public fStatus As System.String = "Status"
            Public fTransType As System.String = "TransType"
            Public fOperationType As System.String = "OperationType"
            Public fItmPrice As System.String = "ItmPrice"
            Public fReqQty As System.String = "ReqQty"
            Public fReqPackQty As System.String = "ReqPackQty"
            Public fOpeningQty As System.String = "OpeningQty"
            Public fOpeningPackQty As System.String = "OpeningPackQty"
            Public fQty As System.String = "Qty"
            Public fPackQty As System.String = "PackQty"
            Public fHandlingQty As System.String = "HandlingQty"
            Public fHandlingPackQty As System.String = "HandlingPackQty"
            Public fClosingQty As System.String = "ClosingQty"
            Public fClosingPackQty As System.String = "ClosingPackQty"
            Public fLastInQty As System.String = "LastInQty"
            Public fLastInPackQty As System.String = "LastInPackQty"
            Public fLastOutQty As System.String = "LastOutQty"
            Public fLastOutPackQty As System.String = "LastOutPackQty"
            Public fRtnPackQty As System.String = "RtnPackQty"
            Public fRtnQty As System.String = "RtnQty"
            Public fRecPackQty As System.String = "RecPackQty"
            Public fRecQty As System.String = "RecQty"
            Public fExpiredQty As System.String = "ExpiredQty"
            Public fExpiryDate As System.String = "ExpiryDate"
            Public fRemark As System.String = "Remark"
            Public fLotNo As System.String = "LotNo"
            Public fCityCode As System.String = "CityCode"
            Public fSecCode As System.String = "SecCode"
            Public fBinCode As System.String = "BinCode"
            Public fRowGuid As System.String = "RowGuid"
            Public fCreateDate As System.String = "CreateDate"
            Public fCreateBy As System.String = "CreateBy"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fUpdateBy As System.String = "UpdateBy"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fItemName As System.String = "ItemName"

            Protected _DocCode As System.String
            Protected _ItemCode As System.String
            Protected _LocID As System.String
            Private _StorageID As System.String
            Private _TermID As System.Int16
            Private _SeqNo As System.Int32
            Private _Status As System.Byte
            Private _TransType As System.Byte
            Private _OperationType As System.String
            Private _ItmPrice As System.Decimal
            Private _ReqQty As System.Decimal
            Private _ReqPackQty As System.Decimal
            Private _OpeningQty As System.Decimal
            Private _OpeningPackQty As System.Decimal
            Private _Qty As System.Decimal
            Private _PackQty As System.Decimal
            Private _HandlingQty As System.Decimal
            Private _HandlingPackQty As System.Decimal
            Private _ClosingQty As System.Decimal
            Private _ClosingPackQty As System.Decimal
            Private _LastInQty As System.Decimal
            Private _LastInPackQty As System.Decimal
            Private _LastOutQty As System.Decimal
            Private _LastOutPackQty As System.Decimal
            Private _RtnPackQty As System.Decimal
            Private _RtnQty As System.Decimal
            Private _RecPackQty As System.Decimal
            Private _RecQty As System.Decimal
            Private _ExpiredQty As System.Decimal
            Private _ExpiryDate As System.DateTime
            Private _Remark As System.String
            Private _LotNo As System.String
            Private _CityCode As System.String
            Private _SecCode As System.String
            Private _BinCode As System.String
            Private _RowGuid As System.Guid
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _ItemName As System.String

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
            Public Property TermID As System.Int16
                Get
                    Return _TermID
                End Get
                Set(ByVal Value As System.Int16)
                    _TermID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SeqNo As System.Int32
                Get
                    Return _SeqNo
                End Get
                Set(ByVal Value As System.Int32)
                    _SeqNo = Value
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
            Public Property TransType As System.Byte
                Get
                    Return _TransType
                End Get
                Set(ByVal Value As System.Byte)
                    _TransType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OperationType As System.String
                Get
                    Return _OperationType
                End Get
                Set(ByVal Value As System.String)
                    _OperationType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ItmPrice As System.Decimal
                Get
                    Return _ItmPrice
                End Get
                Set(ByVal Value As System.Decimal)
                    _ItmPrice = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ReqQty As System.Decimal
                Get
                    Return _ReqQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _ReqQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ReqPackQty As System.Decimal
                Get
                    Return _ReqPackQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _ReqPackQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpeningQty As System.Decimal
                Get
                    Return _OpeningQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _OpeningQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpeningPackQty As System.Decimal
                Get
                    Return _OpeningPackQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _OpeningPackQty = Value
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
            Public Property HandlingQty As System.Decimal
                Get
                    Return _HandlingQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _HandlingQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property HandlingPackQty As System.Decimal
                Get
                    Return _HandlingPackQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _HandlingPackQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ClosingQty As System.Decimal
                Get
                    Return _ClosingQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _ClosingQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ClosingPackQty As System.Decimal
                Get
                    Return _ClosingPackQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _ClosingPackQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LastInQty As System.Decimal
                Get
                    Return _LastInQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _LastInQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LastInPackQty As System.Decimal
                Get
                    Return _LastInPackQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _LastInPackQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LastOutQty As System.Decimal
                Get
                    Return _LastOutQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _LastOutQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LastOutPackQty As System.Decimal
                Get
                    Return _LastOutPackQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _LastOutPackQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RtnPackQty As System.Decimal
                Get
                    Return _RtnPackQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _RtnPackQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RtnQty As System.Decimal
                Get
                    Return _RtnQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _RtnQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RecPackQty As System.Decimal
                Get
                    Return _RecPackQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _RecPackQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RecQty As System.Decimal
                Get
                    Return _RecQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _RecQty = Value
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
            Public Property LotNo As System.String
                Get
                    Return _LotNo
                End Get
                Set(ByVal Value As System.String)
                    _LotNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CityCode As System.String
                Get
                    Return _CityCode
                End Get
                Set(ByVal Value As System.String)
                    _CityCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SecCode As System.String
                Get
                    Return _SecCode
                End Get
                Set(ByVal Value As System.String)
                    _SecCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property BinCode As System.String
                Get
                    Return _BinCode
                End Get
                Set(ByVal Value As System.String)
                    _BinCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RowGuid As System.Guid
                Get
                    Return _RowGuid
                End Get
                Set(ByVal Value As System.Guid)
                    _RowGuid = Value
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
            ''' Non-Mandatory, Allow Null
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
            Public Property SyncCreate As System.DateTime
                Get
                    Return _SyncCreate
                End Get
                Set(ByVal Value As System.DateTime)
                    _SyncCreate = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property SyncLastUpd As System.DateTime
                Get
                    Return _SyncLastUpd
                End Get
                Set(ByVal Value As System.DateTime)
                    _SyncLastUpd = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class ItmtransdtlInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "DocCode,ItemCode,LocID,TermID,SeqNo,Status,ItmPrice,OperationType,TransType,ReqQty,ReqPackQty,Qty,PackQty,RtnPackQty,RtnQty,RecPackQty,RecQty,Remark,LotNo,CityCode,SecCode,BinCode,RowGuid,CreateDate,CreateBy,LastUpdate,UpdateBy,SyncCreate,SyncLastUpd,StorageID,ItemName"
                .CheckFields = "OperationType,TransType"
                .TableName = "Itmtransdtl WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "DocCode,ItemCode,LocID,TermID,SeqNo,Status,ItmPrice,OperationType,TransType,ReqQty,ReqPackQty,Qty,PackQty,RtnPackQty,RtnQty,RecPackQty,RecQty,Remark,LotNo,CityCode,SecCode,BinCode,RowGuid,CreateDate,CreateBy,LastUpdate,UpdateBy,SyncCreate,SyncLastUpd,StorageID,ItemName"
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
    Public Class TransactionDetailsScheme
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
                .FieldName = "ItemCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LocID"
                .Length = 10
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
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "TermID"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "SeqNo"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "TransType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "OperationType"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ItmPrice"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ReqQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ReqPackQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpeningQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpeningPackQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Qty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "PackQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "HandlingQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "HandlingPackQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ClosingQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ClosingPackQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LastInQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LastInPackQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LastOutQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LastOutPackQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "RtnPackQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "RtnQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "RecPackQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "RecQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(27, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ExpiredQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(28, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "ExpiryDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(29, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(30, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "LotNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(31, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "CityCode"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(32, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "SecCode"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(33, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "BinCode"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(34, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "RowGuid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(35, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(36, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(37, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(38, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(39, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(40, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(41, this)

        End Sub

        Public ReadOnly Property DocCode As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property ItemCode As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property LocID As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property

        Public ReadOnly Property StorageID As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property TermID As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property SeqNo As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property TransType As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property OperationType As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property ItmPrice As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property ReqQty As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property ReqPackQty As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property OpeningQty As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property OpeningPackQty As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property Qty As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property PackQty As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property HandlingQty As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property HandlingPackQty As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property ClosingQty As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property ClosingPackQty As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property LastInQty As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property LastInPackQty As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property LastOutQty As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property LastOutPackQty As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property RtnPackQty As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property RtnQty As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property RecPackQty As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property RecQty As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property
        Public ReadOnly Property ExpiredQty As StrucElement
            Get
                Return MyBase.GetItem(28)
            End Get
        End Property
        Public ReadOnly Property ExpiryDate As StrucElement
            Get
                Return MyBase.GetItem(29)
            End Get
        End Property
        Public ReadOnly Property Remark As StrucElement
            Get
                Return MyBase.GetItem(30)
            End Get
        End Property
        Public ReadOnly Property LotNo As StrucElement
            Get
                Return MyBase.GetItem(31)
            End Get
        End Property
        Public ReadOnly Property CityCode As StrucElement
            Get
                Return MyBase.GetItem(32)
            End Get
        End Property
        Public ReadOnly Property SecCode As StrucElement
            Get
                Return MyBase.GetItem(33)
            End Get
        End Property
        Public ReadOnly Property BinCode As StrucElement
            Get
                Return MyBase.GetItem(34)
            End Get
        End Property
        Public ReadOnly Property RowGuid As StrucElement
            Get
                Return MyBase.GetItem(35)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(36)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(37)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(38)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(39)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(40)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(41)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace

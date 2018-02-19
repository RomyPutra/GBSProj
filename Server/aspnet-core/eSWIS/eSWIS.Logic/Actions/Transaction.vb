Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Actions
    Public NotInheritable Class Transaction
        Inherits Core.CoreControl
        Private ItmtranshdrInfo As ItmtranshdrInfo = New ItmtranshdrInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal ItmtranshdrCont As Container.Itmtranshdr, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ItmtranshdrCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItmtranshdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
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
                                .TableName = "Itmtranshdr WITH (ROWLOCK)"
                                .AddField("LocID", ItmtranshdrCont.LocID, SQLControl.EnumDataType.dtString)
                                .AddField("RequestCode", ItmtranshdrCont.RequestCode, SQLControl.EnumDataType.dtString)
                                .AddField("BatchCode", ItmtranshdrCont.BatchCode, SQLControl.EnumDataType.dtString)
                                .AddField("TermID", ItmtranshdrCont.TermID, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RegistedSupp", ItmtranshdrCont.RegistedSupp, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CompanyID", ItmtranshdrCont.CompanyID, SQLControl.EnumDataType.dtString)
                                .AddField("CompanyName", ItmtranshdrCont.CompanyName, SQLControl.EnumDataType.dtStringN)
                                .AddField("CompanyType", ItmtranshdrCont.CompanyType, SQLControl.EnumDataType.dtString)
                                .AddField("TransType", ItmtranshdrCont.TransType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("TransDate", ItmtranshdrCont.TransDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("TransInit", ItmtranshdrCont.TransInit, SQLControl.EnumDataType.dtString)
                                .AddField("TransSrc", ItmtranshdrCont.TransSrc, SQLControl.EnumDataType.dtString)
                                .AddField("TransDest", ItmtranshdrCont.TransDest, SQLControl.EnumDataType.dtString)
                                .AddField("InterTrans", ItmtranshdrCont.InterTrans, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Remark", ItmtranshdrCont.Remark, SQLControl.EnumDataType.dtStringN)
                                .AddField("Reason", ItmtranshdrCont.Reason, SQLControl.EnumDataType.dtStringN)
                                .AddField("PostDate", ItmtranshdrCont.PostDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Posted", ItmtranshdrCont.Posted, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Status", ItmtranshdrCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ConfirmDate", ItmtranshdrCont.ConfirmDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("AuthID", ItmtranshdrCont.AuthID, SQLControl.EnumDataType.dtString)
                                .AddField("AuthPOS", ItmtranshdrCont.AuthPOS, SQLControl.EnumDataType.dtStringN)
                                .AddField("CreateDate", ItmtranshdrCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", ItmtranshdrCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("ApproveDate", ItmtranshdrCont.ApproveDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ApproveBy", ItmtranshdrCont.ApproveBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", ItmtranshdrCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", ItmtranshdrCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", ItmtranshdrCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", ItmtranshdrCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                
                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("DocCode", ItmtranshdrCont.DocCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
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
                                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Transaction", axExecute.Message & strSQL, axExecute.StackTrace)
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
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Transaction", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Transaction", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ItmtranshdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal ItmtranshdrCont As Container.Itmtranshdr, ByRef message As String) As Boolean
            Return Save(ItmtranshdrCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        Public Function UpdateOverdue(ByVal Transdate As Date, ByVal status As Integer, ByVal key As String, ByVal UserID As String, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim pType As SQLControl.EnumSQLType = SQLControl.EnumSQLType.stUpdate
            Try

                blnExec = False
                blnFound = False
                blnFlag = False
                If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
                    With ItmtranshdrInfo.MyInfo
                        strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, key) & "'")
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
                            .TableName = "Itmtranshdr WITH (ROWLOCK)"
                            .AddField("TransDate", Transdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("Status", status, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastUpdate", Transdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("UpdateBy", UserID, SQLControl.EnumDataType.dtString)
                            .AddField("SyncLastUpd", Now.Date, SQLControl.EnumDataType.dtDateTime)

                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, key) & "'")
                        End With
                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True

                        Catch axExecute As Exception
                            message = axExecute.Message.ToString()
                            Log.Notifier.Notify(axExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Transaction", axExecute.Message, axExecute.StackTrace)
                            Return False

                        Finally
                            objSQL.Dispose()
                        End Try
                    End If

                End If
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Transaction", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Transaction", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'AMEND
        Public Function Update(ByVal ItmtranshdrCont As Container.Itmtranshdr, ByRef message As String) As Boolean
            Return Save(ItmtranshdrCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function BatchInsert(ByVal ListItmtranshdrCont As List(Of Container.Itmtranshdr), ByRef message As String) As Boolean
            Return BatchSave(ListItmtranshdrCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        Public Function BatchUpdate(ByVal ListItmtranshdrCont As List(Of Container.Itmtranshdr), ByRef message As String) As Boolean
            Return BatchSave(ListItmtranshdrCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal ItmtranshdrCont As Container.Itmtranshdr, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If ItmtranshdrCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItmtranshdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
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
                                strSQL = BuildUpdate("Itmtranshdr WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = '" & ItmtranshdrCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtranshdrCont.UpdateBy) & "' WHERE" & _
                                "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
                            End With
                        End If

                        'Ivan,31 July 2014,Delete Details
                        Dim strSQL2 As String = "" ' For SQL delete details
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("Itmtranshdr WITH (ROWLOCK)", "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
                            strSQL2 = BuildDelete("ITMTRANSDTL WITH (ROWLOCK)", "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            objConn.Execute(strSQL2, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Dim sqlStatement As String = " "
                            If objConn.FailedSQLStatement.Count > 0 Then
                                sqlStatement &= objConn.FailedSQLStatement.Item(0)
                            End If

                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Transaction", exExecute.Message & sqlStatement, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Transaction", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Transaction", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                ItmtranshdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'Ivan, 6 Agus 2014, Build Funct Cancel
        Public Function Cancel(ByVal ItmtranshdrCont As Container.Itmtranshdr, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Cancel = False
            blnFound = False
            blnInUse = False
            Try
                If ItmtranshdrCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItmtranshdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
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

                        If blnFound = True Then
                            With objSQL
                                strSQL = BuildUpdate("Itmtranshdr WITH (ROWLOCK)", " SET Posted = 2, Status = 2, Flag = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtranshdrCont.UpdateBy) & "' WHERE " & _
                                "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")

                                strSQL &= BuildUpdate("ITMTRANSDTL WITH (ROWLOCK)", " SET Status = 2 " & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItmtranshdrCont.UpdateBy) & "' WHERE " & _
                                "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
                            End With
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Transaction", exExecute.Message, exExecute.StackTrace)
                            Return False

                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Transaction", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Transaction", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                ItmtranshdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function BatchSave(ByVal ListItmtranshdrCont As List(Of Container.Itmtranshdr), ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            Try
                If ListItmtranshdrCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False

                    StartSQLControl()
                    If Not ListItmtranshdrCont Is Nothing AndAlso ListItmtranshdrCont.Count > 0 Then
                        With objSQL
                            Select Case pType
                                Case SQLControl.EnumSQLType.stInsert

                                Case SQLControl.EnumSQLType.stUpdate
                                    strSQL = BuildDelete("Itmtranshdr WITH (ROWLOCK)", "DocCode = '" & ListItmtranshdrCont(0).DocCode & "'")
                                    ListSQL.Add(strSQL)
                            End Select
                        End With
                    End If

                    For Each ItmtranshdrCont In ListItmtranshdrCont
                        With objSQL
                            .TableName = "Itmtranshdr WITH (ROWLOCK)"
                            .AddField("LocID", ItmtranshdrCont.LocID, SQLControl.EnumDataType.dtString)
                            .AddField("RequestCode", ItmtranshdrCont.RequestCode, SQLControl.EnumDataType.dtString)
                            .AddField("BatchCode", ItmtranshdrCont.BatchCode, SQLControl.EnumDataType.dtString)
                            .AddField("TermID", ItmtranshdrCont.TermID, SQLControl.EnumDataType.dtNumeric)
                            .AddField("RegistedSupp", ItmtranshdrCont.RegistedSupp, SQLControl.EnumDataType.dtNumeric)
                            .AddField("CompanyID", ItmtranshdrCont.CompanyID, SQLControl.EnumDataType.dtString)
                            .AddField("CompanyName", ItmtranshdrCont.CompanyName, SQLControl.EnumDataType.dtStringN)
                            .AddField("CompanyType", ItmtranshdrCont.CompanyType, SQLControl.EnumDataType.dtString)
                            .AddField("TransType", ItmtranshdrCont.TransType, SQLControl.EnumDataType.dtNumeric)
                            .AddField("TransDate", ItmtranshdrCont.TransDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("TransInit", ItmtranshdrCont.TransInit, SQLControl.EnumDataType.dtString)
                            .AddField("TransSrc", ItmtranshdrCont.TransSrc, SQLControl.EnumDataType.dtString)
                            .AddField("TransDest", ItmtranshdrCont.TransDest, SQLControl.EnumDataType.dtString)
                            .AddField("InterTrans", ItmtranshdrCont.InterTrans, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Remark", ItmtranshdrCont.Remark, SQLControl.EnumDataType.dtStringN)
                            .AddField("Reason", ItmtranshdrCont.Reason, SQLControl.EnumDataType.dtStringN)
                            .AddField("PostDate", ItmtranshdrCont.PostDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("Posted", ItmtranshdrCont.Posted, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Status", ItmtranshdrCont.Status, SQLControl.EnumDataType.dtNumeric)
                            .AddField("ConfirmDate", ItmtranshdrCont.ConfirmDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("AuthID", ItmtranshdrCont.AuthID, SQLControl.EnumDataType.dtString)
                            .AddField("AuthPOS", ItmtranshdrCont.AuthPOS, SQLControl.EnumDataType.dtStringN)
                            .AddField("CreateDate", ItmtranshdrCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateBy", ItmtranshdrCont.CreateBy, SQLControl.EnumDataType.dtString)
                            .AddField("ApproveDate", ItmtranshdrCont.ApproveDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("ApproveBy", ItmtranshdrCont.ApproveBy, SQLControl.EnumDataType.dtString)
                            .AddField("LastUpdate", ItmtranshdrCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("UpdateBy", ItmtranshdrCont.UpdateBy, SQLControl.EnumDataType.dtString)
                            .AddField("Active", ItmtranshdrCont.Active, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Inuse", ItmtranshdrCont.Inuse, SQLControl.EnumDataType.dtNumeric)

                            Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    If blnFound = True And blnFlag = False Then
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItmtranshdrCont.DocCode) & "'")
                                    Else
                                        If blnFound = False Then
                                            .AddField("DocCode", ItmtranshdrCont.DocCode, SQLControl.EnumDataType.dtString)
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                        End If
                                    End If
                                Case SQLControl.EnumSQLType.stUpdate
                                    .AddField("DocCode", ItmtranshdrCont.DocCode, SQLControl.EnumDataType.dtString)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            End Select
                        End With
                        ListSQL.Add(strSQL)
                    Next

                    Try
                        objConn.BatchExecute(ListSQL, CommandType.Text)
                        Return True

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
                        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Transaction", axExecute.Message & sqlStatement, axExecute.StackTrace)
                        Return False

                    Finally
                        objSQL.Dispose()
                    End Try
                End If
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Transaction", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Transaction", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ListItmtranshdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region

#Region "Data Selection"
        Public Overloads Function GetInventoryByID(ByVal ID As String) As String
            Try
                If StartConnection() = True Then
                    With ItmtranshdrInfo.MyInfo
                        StartSQLControl()
                        strSQL = "SELECT distinct(cast(D.DocCode as varchar(6))+cast(L.itemcode as varchar(99))) as 'barcodeno'," & _
                        " D.DocCode as TransNo,L.ItemName,E.CompanyName, 'https://eswis.doe.gov.my/' + cast(L.itemcode as varchar(99)) AS 'link', " & _
                        " FLOOR(L.PackQty) AS PackQty, L.ItemCode as WasteCode ,  L.ItemCode + '-' + L.TypeCode as 'QRCode', E.BizRegID AS PremiseID " & _
                        " ,L.ItemCode + '|' + L.TypeCode + '|' + L.ItemName + '|' + L.ItemDesc + '|' + D.DocCode + '|' + E.CompanyName as NQR" & _
                        " FROM ITMTRANSDTL D WITH (NOLOCK) INNER JOIN ITMTRANSHDR H ON D.DocCode=H.DocCode INNER JOIN ITEMLOC L WITH (NOLOCK) ON D.LocID = L.LocID " & _
                            " INNER JOIN BIZLOCATE B ON B.BIZLOCID=D.LOCID INNER JOIN BIZENTITY E WITH (NOLOCK) ON B.BizregID=E.BizRegID INNER JOIN ITEM I " & _
                            " WITH (NOLOCK) ON I.ItemCode=D.ItemCode " & _
                                "" 
                        Dim dt = New DataTable()
                        dt = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        EndSQLControl()
                    End With
                Else
                    Return Nothing
                End If

            Catch ex As Exception

            Finally
                EndSQLControl()
                EndConnection()
            End Try
            Return ""
        End Function

        Public Overloads Function GetCNData(ByVal CNNo As String) As List(Of Container_Transaction)
            Dim dt As New Data.DataTable
            Dim ListCN As List(Of Container_Transaction) = New List(Of Container_Transaction)

            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo
                    StartSQLControl()

                    strSQL = "SELECT DISTINCT isnull(cm.codedesc, '-') as [Trans Type], isnull(D.contractno, '-') as [CN No], isnull(D.serialno, '-') as [Serial No], isnull(H.referid, '-') as [Ref No], isnull(H.Transdate, '-') as [Date], isnull(d.qty, '-') as [Total Qty] " & _
                             "FROM CONSIGNHDR H WITH (NOLOCK) " & _
                             "INNER JOIN CONSIGNDTL D WITH (NOLOCK) on H.ContractNo=D.ContractNo " & _
                             "LEFT JOIN ITEMLOC I WITH (NOLOCK) on H.GeneratorLocID=I.LocID and D.WasteCode=I.ItemCode AND D.WasteDescription=I.ItemName " & _
                             "LEFT JOIN StorageMaster S WITH (NOLOCK) on D.StorageID=S.StorageID and H.GeneratorLocID=S.LocID " & _
                             "left join codemaster cm on h.transtype = cm.code and cm.codetype = 'cnt' " & _
                             "WHERE H.ContractNo='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, CNNo) & "'"

                    dt = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)

                    If dt Is Nothing = False Then
                        For Each drRow As DataRow In dt.Rows
                            Dim container As Container_Transaction = New eSWIS.Logic.Actions.Container_Transaction
                            container.TransType = drRow.Item("Trans Type").ToString
                            container.CNNo = drRow.Item("CN No").ToString
                            container.SerialNo = drRow.Item("Serial No").ToString
                            container.RefNo = drRow.Item("Ref No").ToString
                            container.Dates = drRow.Item("Date").ToString
                            container.TotalQty = drRow.Item("Total Qty").ToString

                            ListCN.Add(container)
                        Next

                        Return ListCN
                    Else
                        Return Nothing
                    End If

                    EndSQLControl()
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetSummary(ByVal preID As String, ByVal itemCode As String, ByVal dateStart As String, ByVal dateEnd As String) As List(Of Container_Transaction)
            Dim dt As New Data.DataTable
            Dim ListSummary As List(Of Container_Transaction) = New List(Of Container_Transaction)

            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo
                    StartSQLControl()

                    strSQL = "SELECT CASE WHEN H.TermID=0 AND D.TransType=0 THEN 'Add' WHEN H.TermID=2 AND D.TransType=0 THEN 'Adjust (+)' WHEN H.TermID=2 AND D.TransType=1 THEN 'Adjust (-)' WHEN H.TermID=1 AND D.TransType=1 THEN 'Reused' END AS TransType, sum(D.Qty) as [SUM] " &
                             "FROM ITMTRANSHDR H WITH (NOLOCK) " &
                             "INNER JOIN ITMTRANSDTL D WITH (NOLOCK) ON D.DocCode=H.DocCode AND D.LocID=H.LocID " &
                             "INNER JOIN (SELECT u.UserID, e.BizRegID, l.LocID, e.CompanyName, l.ItemCode, l.ItemName, l.QtyOnHand, x.Closing FROM ITEMLOC l WITH (NOLOCK) " &
                             "INNER JOIN BIZLOCATE loc WITH (NOLOCK) ON loc.BizLocID=l.LocID " &
                             "INNER JOIN BIZENTITY e WITH (NOLOCK) ON e.BizRegID=loc.BizRegID CROSS APPLY (SELECT TOP 1 i.Closing FROM ITEMSUMMARY i WITH (NOLOCK) WHERE i.LocID=L.LocID AND i.ItemCode=l.ItemCode AND i.ItemName=l.ItemName ORDER BY CAST(i.YearCode AS INT) DESC, CAST(i.MthCode AS INT) DESC) x CROSS APPLY (SELECT TOP 1 u.UserID FROM USRPROFILE u WITH (NOLOCK) " &
                             "INNER JOIN USRAPP a WITH (NOLOCK) ON a.UserID=u.UserID AND CAST(a.AccessCode AS INT)%2=0 WHERE u.ParentID=e.BizRegID ORDER BY u.LastLogin DESC) u WHERE l.LocID is not null) X ON X.LocID=D.LocID AND X.ItemCode=D.ItemCode AND X.ItemName=D.ItemName " &
                             "WHERE H.Status = 1 And H.Flag = 1 and d.locid = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, preID) & "'"
                    strSQL &= "GROUP BY CASE WHEN H.TermID=0 AND D.TransType=0 THEN 'Add' WHEN H.TermID=2 AND D.TransType=0 THEN 'Adjust (+)' WHEN H.TermID=2 AND D.TransType=1 THEN 'Adjust (-)' WHEN H.TermID=1 AND D.TransType=1 THEN 'Reused' END " &
                              "union all " &
                              "SELECT CASE WHEN H.STATUS>12 THEN 'Backdate CN' WHEN CONVERT(DATE,H.TransDate)<CONVERT(DATE,H.CreateDate) THEN 'Backdate CN' ELSE 'CN' END, sum(D.Qty) as [SUM] FROM CONSIGNHDR H WITH (NOLOCK) " &
                              "INNER JOIN BIZENTITY E WITH (NOLOCK) ON E.BizRegID=H.GeneratorID " &
                              "INNER JOIN CONSIGNDTL D WITH (NOLOCK) ON D.TransID=H.TransID AND D.ContractNo=H.ContractNo " &
                              "INNER JOIN (SELECT u.UserID, e.BizRegID, l.LocID, e.CompanyName, l.ItemCode, l.ItemName, l.QtyOnHand, x.Closing FROM ITEMLOC l WITH (NOLOCK) " &
                              "INNER JOIN BIZLOCATE loc WITH (NOLOCK) ON loc.BizLocID=l.LocID " &
                              "INNER JOIN BIZENTITY e WITH (NOLOCK) ON e.BizRegID=loc.BizRegID " &
                              "CROSS APPLY (SELECT TOP 1 i.Closing FROM ITEMSUMMARY i WITH (NOLOCK) WHERE i.LocID=L.LocID AND i.ItemCode=l.ItemCode AND i.ItemName=l.ItemName ORDER BY CAST(i.YearCode AS INT) DESC, CAST(i.MthCode AS INT) DESC) x " &
                              "CROSS APPLY (SELECT TOP 1 u.UserID FROM USRPROFILE u WITH (NOLOCK) INNER JOIN USRAPP a WITH (NOLOCK) ON a.UserID=u.UserID AND CAST(a.AccessCode AS INT)%2=0 WHERE u.ParentID=e.BizRegID ORDER BY u.LastLogin DESC) u " &
                              "where loc.bizlocid = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, preID) & "'"
                    strSQL &= ") X ON X.LocID=h.GeneratorLocID AND X.ItemCode=D.WasteCode AND X.ItemName=D.WasteDescription " &
                              "WHERE h.FLAG = 1 And h.STATUS <> 0 And h.STATUS <> 2 And h.STATUS <> 12 And h.STATUS <> 13 And h.STATUS <> 15 And h.ISCONFIRM = 1 " &
                              "GROUP BY CASE WHEN H.STATUS>12 THEN 'Backdate CN' WHEN CONVERT(DATE,H.TransDate)<CONVERT(DATE,H.CreateDate) THEN 'Backdate CN' ELSE 'CN' END "


                    If (Not itemCode Is Nothing And itemCode <> "") Or (Not dateStart Is Nothing And dateStart <> "") Or (Not dateEnd Is Nothing And dateEnd <> "") Then
                        Dim strItemCode As String = ""
                        Dim strCondition As String = ""

                        If itemCode <> "" Then
                            strItemCode &= " AND d.itemcode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, itemCode) & "' "
                        End If

                        If dateStart <> "" Then
                            strCondition &= " AND h.transdate >= '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, dateStart) & "' "
                        End If

                        If dateEnd <> "" Then
                            strCondition &= " AND h.transdate <= '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, dateEnd) & "' "
                        End If

                        strSQL = "SELECT CASE WHEN H.TermID=0 AND D.TransType=0 THEN 'Add' WHEN H.TermID=2 AND D.TransType=0 THEN 'Adjust (+)' WHEN H.TermID=2 AND D.TransType=1 THEN 'Adjust (-)' WHEN H.TermID=1 AND D.TransType=1 THEN 'Reused' END AS TransType, sum(D.Qty) as [SUM] " &
                             "FROM ITMTRANSHDR H WITH (NOLOCK) " &
                             "INNER JOIN ITMTRANSDTL D WITH (NOLOCK) ON D.DocCode=H.DocCode AND D.LocID=H.LocID " &
                             "INNER JOIN (SELECT u.UserID, e.BizRegID, l.LocID, e.CompanyName, l.ItemCode, l.ItemName, l.QtyOnHand, x.Closing FROM ITEMLOC l WITH (NOLOCK) " &
                             "INNER JOIN BIZLOCATE loc WITH (NOLOCK) ON loc.BizLocID=l.LocID " &
                             "INNER JOIN BIZENTITY e WITH (NOLOCK) ON e.BizRegID=loc.BizRegID CROSS APPLY (SELECT TOP 1 i.Closing FROM ITEMSUMMARY i WITH (NOLOCK) WHERE i.LocID=L.LocID AND i.ItemCode=l.ItemCode AND i.ItemName=l.ItemName ORDER BY CAST(i.YearCode AS INT) DESC, CAST(i.MthCode AS INT) DESC) x CROSS APPLY (SELECT TOP 1 u.UserID FROM USRPROFILE u WITH (NOLOCK) " &
                             "INNER JOIN USRAPP a WITH (NOLOCK) ON a.UserID=u.UserID AND CAST(a.AccessCode AS INT)%2=0 WHERE u.ParentID=e.BizRegID ORDER BY u.LastLogin DESC) u WHERE l.LocID is not null) X ON X.LocID=D.LocID AND X.ItemCode=D.ItemCode AND X.ItemName=D.ItemName " &
                             "WHERE H.Status = 1 And H.Flag = 1 and d.locid = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, preID) & "' " & strItemCode & strCondition
                        strSQL &= "GROUP BY CASE WHEN H.TermID=0 AND D.TransType=0 THEN 'Add' WHEN H.TermID=2 AND D.TransType=0 THEN 'Adjust (+)' WHEN H.TermID=2 AND D.TransType=1 THEN 'Adjust (-)' WHEN H.TermID=1 AND D.TransType=1 THEN 'Reused' END " &
                                  "union all " &
                                  "SELECT CASE WHEN H.STATUS>12 THEN 'Backdate CN' WHEN CONVERT(DATE,H.TransDate)<CONVERT(DATE,H.CreateDate) THEN 'Backdate CN' ELSE 'CN' END, sum(D.Qty) as [SUM] FROM CONSIGNHDR H WITH (NOLOCK) " &
                                  "INNER JOIN BIZENTITY E WITH (NOLOCK) ON E.BizRegID=H.GeneratorID " &
                                  "INNER JOIN CONSIGNDTL D WITH (NOLOCK) ON D.TransID=H.TransID AND D.ContractNo=H.ContractNo " &
                                  "INNER JOIN (SELECT u.UserID, e.BizRegID, l.LocID, e.CompanyName, l.ItemCode, l.ItemName, l.QtyOnHand, x.Closing FROM ITEMLOC l WITH (NOLOCK) " &
                                  "INNER JOIN BIZLOCATE loc WITH (NOLOCK) ON loc.BizLocID=l.LocID " &
                                  "INNER JOIN BIZENTITY e WITH (NOLOCK) ON e.BizRegID=loc.BizRegID " &
                                  "CROSS APPLY (SELECT TOP 1 i.Closing FROM ITEMSUMMARY i WITH (NOLOCK) WHERE i.LocID=L.LocID AND i.ItemCode=l.ItemCode AND i.ItemName=l.ItemName ORDER BY CAST(i.YearCode AS INT) DESC, CAST(i.MthCode AS INT) DESC) x " &
                                  "CROSS APPLY (SELECT TOP 1 u.UserID FROM USRPROFILE u WITH (NOLOCK) INNER JOIN USRAPP a WITH (NOLOCK) ON a.UserID=u.UserID AND CAST(a.AccessCode AS INT)%2=0 WHERE u.ParentID=e.BizRegID ORDER BY u.LastLogin DESC) u " &
                                  "where loc.bizlocid = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, preID) & "'" & strItemCode.Replace("d.", "")
                        strSQL &= ") X ON X.LocID=h.GeneratorLocID AND X.ItemCode=D.WasteCode AND X.ItemName=D.WasteDescription " &
                                  "WHERE h.FLAG = 1 And h.STATUS <> 0 And h.STATUS <> 2 And h.STATUS <> 12 And h.STATUS <> 13 And h.STATUS <> 15 And h.ISCONFIRM = 1 " &
                                  "GROUP BY CASE WHEN H.STATUS>12 THEN 'Backdate CN' WHEN CONVERT(DATE,H.TransDate)<CONVERT(DATE,H.CreateDate) THEN 'Backdate CN' ELSE 'CN' END "
                    End If

                    dt = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)

                    If dt Is Nothing = False Then
                        For Each drRow As DataRow In dt.Rows
                            Dim container As Container_Transaction = New Container_Transaction
                            container.TransType = drRow.Item("TransType").ToString
                            container.SUM = drRow.Item("SUM").ToString
                            ListSummary.Add(container)
                        Next

                        Return ListSummary
                    Else
                        Return Nothing
                    End If

                    EndSQLControl()
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetLicenseCategory() As List(Of eSWIS.Logic.Actions.Container_Transaction)
            Dim dt As New Data.DataTable
            Dim ListLicenseCategory As List(Of eSWIS.Logic.Actions.Container_Transaction) = New List(Of Container_Transaction)

            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo
                    StartSQLControl()

                    strSQL = "SELECT LicenseDesc AS LicenseDesc, RTRIM(LTRIM(LicenseCode)) AS 'LicenseCode' FROM LicenseCategory  ORDER BY LicenseDesc ASC"

                    dt = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)

                    If dt Is Nothing = False Then
                        For Each drRow As DataRow In dt.Rows
                            Dim container As Container_Transaction = New eSWIS.Logic.Actions.Container_Transaction
                            container.LicenseDesc = drRow.Item("LicenseDesc").ToString
                            container.LicenseCode = drRow.Item("LicenseCode").ToString

                            ListLicenseCategory.Add(container)
                        Next

                        Return ListLicenseCategory
                    Else
                        Return Nothing
                    End If

                    EndSQLControl()
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetWasteCode() As List(Of Container_Transaction)
            Dim dt As New Data.DataTable
            Dim ListWasteCode As List(Of Container_Transaction) = New List(Of Container_Transaction)

            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo
                    StartSQLControl()

                    strSQL = "SELECT RTRIM(LTRIM(ItemCode)) AS 'ItemCode' FROM Item  where Item.ItemCode like 'SW%' and LEN(Item.ItemCode)=5  ORDER BY Item.ItemCode ASC"

                    dt = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)

                    If dt Is Nothing = False Then
                        For Each drRow As DataRow In dt.Rows
                            Dim container As Container_Transaction = New eSWIS.Logic.Actions.Container_Transaction
                            container.ItemCode = drRow.Item("ItemCode").ToString
                            ListWasteCode.Add(container)
                        Next
                        Return ListWasteCode
                    Else
                        Return Nothing
                    End If

                    EndSQLControl()
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetTypeofLicense() As List(Of Actions.Container_Transaction)
            Dim dt As New Data.DataTable
            Dim ListTypeOfLicense As List(Of Actions.Container_Transaction) = New List(Of Container_Transaction)

            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo
                    StartSQLControl()

                    strSQL = "SELECT CodeDesc AS CodeDesc, RTRIM(LTRIM(Code)) AS 'Code' FROM CodeMaster  Where CodeType='LTY'  ORDER BY CodeDesc ASC"

                    dt = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)

                    If dt Is Nothing = False Then
                        For Each drRow As DataRow In dt.Rows
                            Dim container As Container_Transaction = New Actions.Container_Transaction
                            container.CodeDesc = drRow.Item("CodeDesc").ToString
                            container.Code = drRow.Item("Code").ToString

                            ListTypeOfLicense.Add(container)
                        Next

                        Return ListTypeOfLicense
                    Else
                        Return Nothing
                    End If

                    EndSQLControl()
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetCity(ByVal stateCode As String) As List(Of Actions.Container_Transaction)
            Dim dt As New Data.DataTable
            Dim ListCity As List(Of Container_Transaction) = New List(Of Container_Transaction)

            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo
                    StartSQLControl()

                    strSQL = "SELECT CityCode,CityDesc FROM City WHERE CountryCode = 'MY' AND StateCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, stateCode) & "' " &
                             "AND Active = 1 AND Flag = 1 ORDER BY CityCode ASC"

                    dt = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)

                    If dt Is Nothing = False Then
                        For Each drRow As DataRow In dt.Rows
                            Dim container As Container_Transaction = New Actions.Container_Transaction
                            container.CityDesc = drRow.Item("CityDesc").ToString
                            container.CityCode = drRow.Item("CityCode").ToString
                            ListCity.Add(container)
                        Next

                        Return ListCity
                    Else
                        Return Nothing
                    End If

                    EndSQLControl()
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetArea(ByVal stateCode As String, ByVal cityCode As String) As List(Of Actions.Container_Transaction)
            Dim dt As New Data.DataTable
            Dim ListArea As List(Of Container_Transaction) = New List(Of Container_Transaction)

            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo
                    StartSQLControl()

                    strSQL = "SELECT AreaCode,AreaDesc FROM Area WHERE CountryCode = 'MY' AND StateCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, stateCode) & "' " &
                             "AND CityCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, cityCode) & "' AND Active = 1 AND Flag = 1 ORDER BY AreaCode ASC"

                    dt = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)

                    If dt Is Nothing = False Then
                        For Each drRow As DataRow In dt.Rows
                            Dim container As Container_Transaction = New Actions.Container_Transaction
                            container.AreaDesc = drRow.Item("AreaDesc").ToString
                            container.AreaCode = drRow.Item("AreaCode").ToString
                            ListArea.Add(container)
                        Next

                        Return ListArea
                    Else
                        Return Nothing
                    End If

                    EndSQLControl()
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetState() As List(Of Container_Transaction)
            Dim dt As New Data.DataTable
            Dim ListState As List(Of Container_Transaction) = New List(Of Container_Transaction)

            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo
                    StartSQLControl()

                    strSQL = "SELECT StateDesc AS StateDesc, RTRIM(LTRIM(StateCode)) AS 'StateCode' FROM State " &
                             "WHERE Flag = '1' AND Active = '1' AND COUNTRYCODE = 'MY'  ORDER BY StateDesc ASC"

                    dt = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)

                    If dt Is Nothing = False Then
                        For Each drRow As DataRow In dt.Rows
                            Dim container As Container_Transaction = New Actions.Container_Transaction
                            container.StateDesc = drRow.Item("StateDesc").ToString
                            container.StateCode = drRow.Item("StateCode").ToString
                            ListState.Add(container)
                        Next

                        Return ListState
                    Else
                        Return Nothing
                    End If

                    EndSQLControl()
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetInventoryByPremise(ByVal LocationID As String, Optional ByVal StartDate As String = Nothing, _
                                                 Optional ByVal EndDate As String = Nothing) As List(Of Container_Transaction)
            Dim rTrans As Container_Transaction = Nothing
            Dim listTrans As List(Of Container_Transaction) = Nothing
            Dim dtTemp As DataTable = Nothing

            If StartConnection() = True Then
                StartSQLControl()
                strSQL = "SELECT H.LocID, x.BizRegID, x.CompanyName,  X.BranchName, H.DocCode as TransID, H.Status, H.Posted, H.TransDate " & _
                     "FROM ITMTRANSHDR H WITH (NOLOCK) " & _
                     "INNER JOIN ITMTRANSDTL D WITH (NOLOCK) ON D.DocCode=H.DocCode AND D.LocID=H.LocID " & _
                     "INNER JOIN (SELECT u.UserID, e.BizRegID, l.LocID, e.CompanyName, loc.BranchName, l.ItemCode, l.ItemName, l.QtyOnHand, x.Closing FROM ITEMLOC l WITH (NOLOCK) " & _
                     "INNER JOIN BIZLOCATE loc WITH (NOLOCK) ON loc.BizLocID=l.LocID " & _
                     "INNER JOIN BIZENTITY e WITH (NOLOCK) ON e.BizRegID=loc.BizRegID " & _
                     "CROSS APPLY (SELECT TOP 1 i.Closing FROM ITEMSUMMARY i WITH (NOLOCK) WHERE i.LocID=L.LocID AND i.ItemCode=l.ItemCode AND i.ItemName=l.ItemName ORDER BY CAST(i.YearCode AS INT) DESC, CAST(i.MthCode AS INT) DESC) x " & _
                     "CROSS APPLY (SELECT TOP 1 u.UserID FROM USRPROFILE u WITH (NOLOCK) INNER JOIN USRAPP a WITH (NOLOCK) ON a.UserID=u.UserID AND CAST(a.AccessCode AS INT)%2=0 WHERE u.ParentID=e.BizRegID ORDER BY u.LastLogin DESC) u " & _
                     "WHERE l.LocID is not null) X ON X.LocID=D.LocID AND X.ItemCode=D.ItemCode AND X.ItemName=D.ItemName " & _
                     "WHERE H.Status = 1 And H.Flag = 1 AND H.locid = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocationID) & "'"


                If StartDate IsNot Nothing AndAlso StartDate <> "" AndAlso EndDate IsNot Nothing AndAlso EndDate <> "" Then
                    strSQL &= " And (convert(date,H.TransDate) >= convert(date, '" & StartDate & "', 103) and convert(date,H.TransDate) <= convert(date, '" & EndDate & "', 103))"
                ElseIf StartDate = "" AndAlso EndDate = "" Then
                    strSQL &= " And (convert(date,H.TransDate) >= convert(date,dateadd(dd,-day(getdate())+1,getdate())) and convert(date,H.TransDate) <= convert(date,getDate()))"
                ElseIf StartDate Is Nothing AndAlso EndDate Is Nothing Then
                    strSQL &= " And (convert(date,H.TransDate) >= convert(date,dateadd(dd,-day(getdate())+1,getdate())) and convert(date,H.TransDate) <= convert(date,getDate()))"
                ElseIf StartDate IsNot Nothing AndAlso StartDate <> "" AndAlso EndDate = "" Then
                    strSQL &= " And (convert(date,H.TransDate) >= convert(date, '" & StartDate & "', 103) and convert(date,H.TransDate) <= convert(date,getDate()))"
                ElseIf StartDate = "" AndAlso EndDate IsNot Nothing AndAlso EndDate <> "" Then
                    strSQL &= " And (convert(date,H.TransDate) >= convert(date,dateadd(dd,-day(getdate())+1,getdate())) and convert(date,H.TransDate) <= convert(date, '" & EndDate & "', 103))"
                ElseIf StartDate IsNot Nothing AndAlso StartDate <> "" AndAlso EndDate Is Nothing Then
                    strSQL &= " And (convert(date,H.TransDate) >= convert(date, '" & StartDate & "', 103) and convert(date,H.TransDate) <= convert(date,getDate()))"
                ElseIf StartDate Is Nothing AndAlso EndDate IsNot Nothing AndAlso EndDate <> "" Then
                    strSQL &= " And (convert(date,H.TransDate) >= convert(date,dateadd(dd,-day(getdate())+1,getdate())) and convert(date,H.TransDate) <= convert(date, '" & EndDate & "', 103))"
                End If

                strSQL &= " GROUP BY H.LocID, x.BizRegID, x.CompanyName, X.BranchName, H.DocCode, H.Status, H.Posted, H.TransDate ORDER BY h.transdate desc"

                Try
                    dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
                    If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
                        listTrans = New List(Of Container_Transaction)
                        For Each row As DataRow In dtTemp.Rows
                            rTrans = New Container_Transaction
                            With rTrans
                                .BizRegID = row.Item("BizRegID").ToString
                                .CompanyName = row.Item("CompanyName").ToString
                                .LocID = row.Item("LocID").ToString
                                .BranchName = row.Item("BranchName").ToString
                                .TransID = row.Item("TransID").ToString
                                .TransDate = row.Item("TransDate").ToString
                                .Status = row.Item("Status").ToString
                                .Posted = row.Item("Posted").ToString
                            End With
                            listTrans.Add(rTrans)
                        Next
                    End If
                Catch ex As Exception
                    Log.Notifier.Notify(ex)
                    Gibraltar.Agent.Log.Error("Actions/Notification", ex.Message & " " & strSQL, ex.StackTrace)
                Finally
                    EndSQLControl()
                End Try
            End If
            EndConnection()
            Return listTrans
        End Function

        Public Overloads Function GetInventoryDTL(ByVal TransNo As String) As List(Of Container_Transaction)
            Dim rTrans As Container_Transaction = Nothing
            Dim listTrans As List(Of Container_Transaction) = Nothing
            Dim dtTemp As DataTable = Nothing

            If StartConnection() = True Then
                StartSQLControl()
                strSQL = "SELECT CASE WHEN H.TermID=0 AND D.TransType=0 THEN 'Add' " & _
                                     "WHEN H.TermID=2 AND D.TransType=0 THEN 'Adjust (+)' " & _
                                     "WHEN H.TermID=2 AND D.TransType=1 THEN 'Adjust (-)' " & _
                                     "WHEN H.TermID=1 AND D.TransType=1 THEN 'Reused' END AS TransType, " & _
                        "D.ItemCode, D.ItemName, D.Qty, D.OpeningQty, D.ClosingQty " & _
                        "FROM ITMTRANSHDR H WITH (NOLOCK) INNER JOIN ITMTRANSDTL D WITH (NOLOCK) ON D.DocCode=H.DocCode AND D.LocID=H.LocID " & _
                        "INNER JOIN (SELECT u.UserID, e.BizRegID, l.LocID, e.CompanyName, l.ItemCode, l.ItemName, l.QtyOnHand, x.Closing FROM ITEMLOC l WITH (NOLOCK) " & _
                        "INNER JOIN BIZLOCATE loc WITH (NOLOCK) ON loc.BizLocID=l.LocID " & _
                        "INNER JOIN BIZENTITY e WITH (NOLOCK) ON e.BizRegID=loc.BizRegID " & _
                        "CROSS APPLY (SELECT TOP 1 i.Closing FROM ITEMSUMMARY i WITH (NOLOCK) WHERE i.LocID=L.LocID AND i.ItemCode=l.ItemCode AND i.ItemName=l.ItemName ORDER BY CAST(i.YearCode AS INT) DESC, CAST(i.MthCode AS INT) DESC) x " & _
                        "CROSS APPLY (SELECT TOP 1 u.UserID FROM USRPROFILE u WITH (NOLOCK) INNER JOIN USRAPP a WITH (NOLOCK) ON a.UserID=u.UserID AND CAST(a.AccessCode AS INT)%2=0 WHERE u.ParentID=e.BizRegID ORDER BY u.LastLogin DESC) u " & _
                        "WHERE l.LocID is not null) X ON X.LocID=D.LocID AND X.ItemCode=D.ItemCode AND X.ItemName=D.ItemName " & _
                        "WHERE H.Status = 1 And H.Flag = 1 AND H.DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransNo) & "' " & _
                        "GROUP BY H.LocID, H.CompanyName, H.DocCode, CASE WHEN H.TermID=0 AND D.TransType=0 THEN 'Add' WHEN H.TermID=2 AND D.TransType=0 THEN 'Adjust (+)' WHEN H.TermID=2 AND D.TransType=1 THEN 'Adjust (-)' " & _
                        "WHEN H.TermID=1 AND D.TransType=1 THEN 'Reused' END, H.Status, H.Posted, H.TransDate, D.ItemCode, D.ItemName, D.Qty, D.OpeningQty, D.ClosingQty ORDER BY h.transdate desc"
                Try
                    dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
                    If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
                        listTrans = New List(Of Container_Transaction)
                        For Each row As DataRow In dtTemp.Rows
                            rTrans = New Container_Transaction
                            With rTrans
                                .ItemCode = row.Item("ItemCode").ToString
                                .ItemName = row.Item("ItemName").ToString
                                .TransType = row.Item("TransType").ToString
                                .Qty = row.Item("Qty").ToString
                                .OpeningQty = row.Item("OpeningQty").ToString
                                .ClosingQty = row.Item("ClosingQty").ToString
                            End With
                            listTrans.Add(rTrans)
                        Next
                    End If
                Catch ex As Exception
                    Log.Notifier.Notify(ex)
                    Gibraltar.Agent.Log.Error("Actions/Notification", ex.Message & " " & strSQL, ex.StackTrace)
                Finally
                    EndSQLControl()
                End Try
            End If
            EndConnection()
            Return listTrans
        End Function

        Public Overloads Function GetTransactions(ByVal DocCode As System.String) As Container.Itmtranshdr
            Dim rItmtranshdr As Container.Itmtranshdr = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With ItmtranshdrInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, DocCode) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rItmtranshdr = New Container.Itmtranshdr
                                rItmtranshdr.DocCode = drRow.Item("DocCode")
                                rItmtranshdr.LocID = drRow.Item("LocID")
                                rItmtranshdr.RequestCode = drRow.Item("RequestCode")
                                rItmtranshdr.BatchCode = drRow.Item("BatchCode")
                                rItmtranshdr.TermID = drRow.Item("TermID")
                                rItmtranshdr.RegistedSupp = drRow.Item("RegistedSupp")
                                rItmtranshdr.CompanyID = drRow.Item("CompanyID")
                                rItmtranshdr.CompanyName = drRow.Item("CompanyName")
                                rItmtranshdr.CompanyType = drRow.Item("CompanyType")
                                rItmtranshdr.TransType = drRow.Item("TransType")
                                rItmtranshdr.TransDate = IIf(IsDBNull(drRow.Item("TransDate")), Now, drRow.Item("TransDate"))
                                rItmtranshdr.TransInit = drRow.Item("TransInit")
                                rItmtranshdr.TransSrc = drRow.Item("TransSrc")
                                rItmtranshdr.TransDest = drRow.Item("TransDest")
                                rItmtranshdr.InterTrans = drRow.Item("InterTrans")
                                rItmtranshdr.Remark = drRow.Item("Remark")
                                rItmtranshdr.Reason = drRow.Item("Reason")
                                rItmtranshdr.Posted = drRow.Item("Posted")
                                rItmtranshdr.PostDate = IIf(IsDBNull(drRow.Item("PostDate")), Now, drRow.Item("PostDate")) 'PostDate For Submission date
                                rItmtranshdr.Status = drRow.Item("Status")
                                rItmtranshdr.AuthID = drRow.Item("AuthID")
                                rItmtranshdr.CreateDate = IIf(IsDBNull(drRow.Item("CreateDate")), Now, drRow.Item("CreateDate"))
                                rItmtranshdr.CreateBy = drRow.Item("CreateBy")
                                rItmtranshdr.Active = drRow.Item("Active")
                                rItmtranshdr.Inuse = drRow.Item("Inuse")
                                rItmtranshdr.RowGuid = drRow.Item("RowGuid")
                                rItmtranshdr.SyncCreate = drRow.Item("SyncCreate")
                            Else
                                rItmtranshdr = Nothing
                            End If
                        Else
                            rItmtranshdr = Nothing
                        End If
                    End With
                End If
                Return rItmtranshdr
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Transaction", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rItmtranshdr = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTransactions(ByVal DocCode As System.String, DecendingOrder As Boolean) As List(Of Container.Itmtranshdr)
            Dim rItmtranshdr As Container.Itmtranshdr = Nothing
            Dim lstItmtranshdr As List(Of Container.Itmtranshdr) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With ItmtranshdrInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal DocCode As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "DocCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, DocCode) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rItmtranshdr = New Container.Itmtranshdr
                                rItmtranshdr.DocCode = drRow.Item("DocCode")
                                rItmtranshdr.LocID = drRow.Item("LocID")
                                rItmtranshdr.RequestCode = drRow.Item("RequestCode")
                                rItmtranshdr.BatchCode = drRow.Item("BatchCode")
                                rItmtranshdr.TermID = drRow.Item("TermID")
                                rItmtranshdr.RegistedSupp = drRow.Item("RegistedSupp")
                                rItmtranshdr.CompanyID = drRow.Item("CompanyID")
                                rItmtranshdr.CompanyName = drRow.Item("CompanyName")
                                rItmtranshdr.CompanyType = drRow.Item("CompanyType")
                                rItmtranshdr.TransType = drRow.Item("TransType")
                                rItmtranshdr.TransDate = drRow.Item("TransDate")
                                rItmtranshdr.TransInit = drRow.Item("TransInit")
                                rItmtranshdr.TransSrc = drRow.Item("TransSrc")
                                rItmtranshdr.TransDest = drRow.Item("TransDest")
                                rItmtranshdr.InterTrans = drRow.Item("InterTrans")
                                rItmtranshdr.Remark = drRow.Item("Remark")
                                rItmtranshdr.Reason = drRow.Item("Reason")
                                rItmtranshdr.Posted = drRow.Item("Posted")
                                rItmtranshdr.Status = drRow.Item("Status")
                                rItmtranshdr.AuthID = drRow.Item("AuthID")
                                rItmtranshdr.CreateDate = drRow.Item("CreateDate")
                                rItmtranshdr.CreateBy = drRow.Item("CreateBy")
                                rItmtranshdr.Active = drRow.Item("Active")
                                rItmtranshdr.Inuse = drRow.Item("Inuse")
                                rItmtranshdr.RowGuid = drRow.Item("RowGuid")
                                rItmtranshdr.SyncCreate = drRow.Item("SyncCreate")
                            Next
                            lstItmtranshdr.Add(rItmtranshdr)
                        Else
                            rItmtranshdr = Nothing
                        End If
                        Return lstItmtranshdr
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Transaction", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rItmtranshdr = Nothing
                lstItmtranshdr = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTransactionsList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo
                    If SQL = Nothing Or SQL = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, " (SELECT dh.* from itmtranshdr dh with (nolock) cross apply (select dl.LocID from itmtransdtl dl with (nolock) inner join itemloc il with (nolock) on dl.itemcode = il.itemcode and dl.itemname = il.itemname and dl.locid = il.locid where dh.DocCode = dl.DocCode group by dl.LocID) dtl) ITMTRANSHDR ", FieldCond)
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

        Public Overloads Function GetTransactionsListAdjustment(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo

                    strSQL = "SELECT distinct DocCode,LocID,RequestCode,BatchCode,TermID,RegistedSupp,CompanyID,CompanyName,CompanyType,CONVERT(varchar, TransDate, 101) as Transdate,TransInit,TransSrc,TransDest,InterTrans,Remark,Reason,Posted,Status,ConfirmDate,AuthID,AuthPOS,CreateBy,ApproveDate,ApproveBy,UpdateBy,Active,Inuse,Flag,SyncLastUpd,right(convert(varchar, TransDate, 106), 8) as TransDateMY,LiveCal " &
                            " FROM  (SELECT dh.* from itmtranshdr dh with (nolock) cross apply (select dl.LocID from itmtransdtl dl with (nolock) inner join itemloc il with (nolock) on dl.itemcode = il.itemcode and dl.itemname = il.itemname and dl.locid = il.locid where dh.DocCode = dl.DocCode group by dl.LocID) dtl) ITMTRANSHDR WHERE " & FieldCond
                    
            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function
        
        Public Overloads Function GetAdjustmentList(ByVal BeginDate As String, ByVal EndDate As String, Optional ByVal CompanyID As String = Nothing, Optional ByVal LocID As String = Nothing) As Data.DataTable
            Dim FieldCond As String = " FLAG = 1 AND TermID=2 "
            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo
                    FieldCond &= " AND CompanyID='" & CompanyID & "'"

                    FieldCond &= " AND LocID='" & LocID & "'"
                    FieldCond &= " And (TransDate >= '" & BeginDate & "' and TransDate <= '" & EndDate & "') "

                    strSQL = BuildSelect(.FieldsList, " (SELECT *, ROW_NUMBER() OVER(PARTITION BY DocCode ORDER BY DocCode) AS RecNo FROM ITMTRANSHDR) ITMTRANSHDR ", FieldCond)

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetInventoryList(ByVal BeginDate As String, ByVal EndDate As String, Optional ByVal CompanyID As String = Nothing, Optional ByVal LocID As String = Nothing) As Data.DataTable
            Dim FieldCond As String = " FLAG = 1 AND TermID!=2 "
            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo
                    FieldCond &= " AND CompanyID='" & CompanyID & "'"

                    FieldCond &= " AND LocID='" & LocID & "'"
                    FieldCond &= " And (TransDate >= '" & BeginDate & "' and TransDate <= '" & EndDate & "') "
                    strSQL = BuildSelect(.FieldsList, " (SELECT *, ROW_NUMBER() OVER(PARTITION BY DocCode ORDER BY DocCode) AS RecNo FROM ITMTRANSHDR) ITMTRANSHDR ", FieldCond)
                   
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        'View AdjustmentMonitoring
        Public Overloads Function GetAdjustmentMonitoring(Optional ByVal condition As String = Nothing, Optional ByVal condition2 As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo

                    strSQL = "select ROW_NUMBER() OVER (ORDER BY D.ItemCode,H.CompanyName,SUBSTRING(CONVERT(VARCHAR(11), max(TransDate), 113), 4, 8)) as RecNo, CONVERT(VARCHAR(11), max(TransDate), 103) as TransDate, sum(Qty) as Qty, " & _
                            "(select STATE.StateDesc from STATE WITH (NOLOCK) inner join BIZLOCATE WITH (NOLOCK) on STATE.StateCode = BIZLOCATE.State where BIZLOCATE.BizLocID=H.LocID) as State, D.ItemCode,D.ItemName,(SELECT SUBSTRING(CONVERT(VARCHAR(11), " & _
                            "max(TransDate), 113), 4, 8)) AS TransDateMY,H.CompanyName, (select top 1 BranchName from BIZLOCATE WITH (NOLOCK) where BIZLOCATE.BizLocID=H.LocID) as Location,(select top 1 BizLocID from BIZLOCATE WITH (NOLOCK) " & _
                            "where BIZLOCATE.BizLocID=H.LocID) as LocID,  COUNT (distinct D.DocCode) as AdjAttempt,(select top 1 Qty from ITMTRANSDTL left join ITMTRANSHDR on ITMTRANSDTL.DocCode=ITMTRANSHDR.DocCode where ITMTRANSDTL.ItemCode=D.ItemCode " & _
                            "AND ITMTRANSDTL.ItemName=D.ItemName AND ITMTRANSHDR.LocID=(select top 1 BizLocID from BIZLOCATE WITH (NOLOCK) where BIZLOCATE.BizLocID=H.LocID) AND " & condition2 & _
                            "order by TransDate desc) as LastQtyAdjust, I.CurrentBalance from ITMTRANSHDR H WITH (NOLOCK) right join ITMTRANSDTL D WITH (NOLOCK) " & _
                            "on H.DocCode=D.DocCode	 left join (SELECT ItemCode, ItemName, /*LocID,*/ MthCode, YearCode, Closing  AS CurrentBalance FROM ITEMSUMMARY WITH (NOLOCK)) I on D.ItemCode=I.ItemCode and D.ItemName=I.ItemName /*and H.LocID=I.LocID*/ and " & _
                            "I.MthCode=MONTH(TransDate) and YearCode=YEAR(Transdate) " & _
                            " WHERE " & condition & _
                            " GROUP BY D.ItemCode,D.ItemName,H.CompanyName,H.LocID,I.CurrentBalance, SUBSTRING(CONVERT(VARCHAR(11), TransDate, 113), 4, 8)" & _
                            " ORDER BY AdjAttempt desc"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        'View Export AdjustmentMonitoring
        Public Overloads Function GetExportAdjustmentMonitoring(Optional ByVal condition As String = Nothing, Optional ByVal condition2 As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo

                    strSQL = "select ROW_NUMBER() OVER (ORDER BY D.ItemCode,H.CompanyName,SUBSTRING(CONVERT(VARCHAR(11), TransDate, 113), 4, 8)) " & _
                            "as RecNo, H.DocCode, CONVERT(VARCHAR(11), TransDate, 103) as TransDate, Qty, H.Remark, AuthID, (select STATE.StateDesc from STATE WITH (NOLOCK) " & _
                            "inner join BIZLOCATE WITH (NOLOCK) on STATE.StateCode = BIZLOCATE.State where BIZLOCATE.BizLocID=H.LocID) as State, D.ItemCode,D.ItemName, " & _
                            "(SELECT SUBSTRING(CONVERT(VARCHAR(11), TransDate, 113), 4, 8)) AS TransDateMY,(select top 1 ContactPerson from BIZLOCATE WITH (NOLOCK) where BIZLOCATE.BizLocID=H.LocID) as OfficerName, " & _
                            "(select top 1 BranchName from BIZLOCATE WITH (NOLOCK) " & _
                            "where BIZLOCATE.BizLocID=H.LocID) as Location,(select top 1 BizLocID from BIZLOCATE WITH (NOLOCK) where BIZLOCATE.BizLocID=H.LocID) as LocID,  " & _
                            "COUNT (distinct D.DocCode) as AdjAttempt,(select top 1 Qty from ITMTRANSDTL left join ITMTRANSHDR on ITMTRANSDTL.DocCode=ITMTRANSHDR.DocCode " & _
                            "where ITMTRANSDTL.ItemCode=D.ItemCode AND ITMTRANSDTL.ItemName=D.ItemName AND ITMTRANSHDR.LocID=(select top 1 BizLocID from BIZLOCATE WITH (NOLOCK) " & _
                            "where BIZLOCATE.BizLocID=H.LocID) AND " & condition2 & _
                            "order by TransDate desc) as LastQtyAdjust, I.CurrentBalance from ITMTRANSHDR H WITH " & _
                            "(NOLOCK) right join ITMTRANSDTL D WITH (NOLOCK) on H.DocCode=D.DocCode	 left join (SELECT ItemCode, ItemName, LocID, MthCode, YearCode, Closing " & _
                            "AS CurrentBalance FROM ITEMSUMMARY WITH (NOLOCK)) I    on D.ItemCode=I.ItemCode and D.ItemName=I.ItemName and H.LocID=I.LocID and I.MthCode=MONTH(TransDate) and YearCode=YEAR(Transdate) " & _
                            " WHERE " & condition & _
                            " GROUP BY D.ItemCode,D.ItemName,H.CompanyName,SUBSTRING(CONVERT(VARCHAR(11), TransDate, 113), 4, 8),H.LocID,I.CurrentBalance, H.DocCode, CONVERT(VARCHAR(11), TransDate, 103), Qty, H.Remark, AuthID" & _
                            " ORDER BY AdjAttempt desc"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        'Load Posted Status
        Public Overloads Function GetPostedStatus(ByVal DocCode As String) As String
            If StartConnection() = True Then
                StartSQLControl()
                With ItmtranshdrInfo.MyInfo

                    strSQL = "SELECT Posted FROM ITMTRANSHDR WHERE DocCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, DocCode) & "'"

                    Dim dt As DataTable = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)

                    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                        Return dt.Rows(0)("Posted")
                    End If
                End With

            Else
                Return ""
            End If
            EndSQLControl()
            EndConnection()
            Return ""
        End Function

        Public Overloads Function GetInventoryDashBoard(Optional ByVal condition As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo

                    strSQL = "SELECT top 10 DocCode,LocID,(select branchname from BIZLOCATE where BizLocID=ITMTRANSHDR.LocID ) as LocationName,RequestCode,BatchCode,TermID,RegistedSupp,CompanyID,CompanyName,CompanyType,TransType,TransDate, " & _
                        " TransInit,TransSrc,TransDest,InterTrans,Remark,Reason,PostDate,Posted,Status,ConfirmDate,AuthID,AuthPOS,CreateDate,CreateBy,ApproveDate, " & _
                        " ApproveBy,LastUpdate,UpdateBy,Active,Inuse,Flag,RowGuid,SyncCreate,SyncLastUpd,right(convert(varchar, TransDate, 106), 8) as TransDateMY " & _
                        " FROM  (SELECT *, ROW_NUMBER() OVER(PARTITION BY DocCode ORDER BY DocCode) AS RecNo FROM ITMTRANSHDR) ITMTRANSHDR  " & _
                        " WHERE " & condition

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetTransDate(Optional ByVal condition As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo

                    strSQL = "SELECT distinct right(convert(varchar, TransDate, 106), 8) as TransDateMY " & _
                                " From ITMTRANSHDR  WHERE " & condition

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetTransDateAdjustmentMonitoring(Optional ByVal condition As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo

                    strSQL = "SELECT distinct right(convert(varchar, TransDate, 106), 8) as TransDateMY " & _
                                " From ITMTRANSHDR H WHERE " & condition

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetTransactionsShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo
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

        Public Overloads Function GetTransactionCustomList(Optional ByVal Condition As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ItmtranshdrInfo.MyInfo

                    'amended by diana 20140911, load correct details from location
                    strSQL = "SELECT DocCode, ITMTRANSHDR.Remark,ITMTRANSHDR.LocID, TransDate, AuthID, ISNULL(AuthPOS,'') AS AuthPos, CompanyID, Posted,  " & _
                        "ISNULL( (SELECT BranchName FROM BIZLOCATE WHERE BizLocID=ITMTRANSHDR.LocID),'') AS CompanyName, " & _
                        "ISNULL( (SELECT RegNo FROM BizEntity WHERE BizRegID=ITMTRANSHDR.CompanyID),'') AS RegNo, " & _
                        "ISNULL( (SELECT AccNo FROM BizLocate WHERE BizLocID=ITMTRANSHDR.LocID),'') AS AccNo, " & _
                        "BranchName, Address1, Address2, Address3, Address4, " & _
                        "ISNULL( (SELECT StateDesc FROM STATE WHERE StateCode=BIZLOCATE.State and CountryCode=BIZLOCATE.Country),'') AS StateDesc, " & _
                        "ISNULL( (SELECT CityDesc FROM CITY WHERE CityCode=BIZLOCATE.City and CountryCode=BIZLOCATE.Country and StateCode=BIZLOCATE.State),'') AS CityDesc, " & _
                        "ISNULL( (SELECT CountryDesc FROM COUNTRY WHERE CountryCode=BIZLOCATE.Country),'') AS CountryDesc " & _
                        "FROM ITMTRANSHDR INNER JOIN BIZLOCATE ON " & _
                        " ITMTRANSHDR.LocID=BIZLOCATE.BizLocID"

                    If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

#End Region
    End Class

    Public Class Container_Transaction
        Private _statedesc As String
        Private _statecode As String

        Private _citydesc As String
        Private _citycode As String

        Private _areadesc As String
        Private _areacode As String

        Private _licensedesc As String
        Private _licensecode As String

        Private _codedesc As String
        Private _code As String

        Private _itemcode As String

        Private _BizRegID As String
        Private _companyname As String
        Private _LocID As String

        Private _branchname As String
        Private _transid As String
        Private _transtype As String
        Private _status As String
        Private _posted As String
        Private _transdate As String
        Private _itemname As String
        Private _qty As String
        Private _openingqty As String
        Private _closingqty As String
        Private _sum As String

        Private _cnno As String
        Private _serialno As String
        Private _refno As String
        Private _date As String
        Private _totalqty As String

        Public Property StateDesc() As String
            Get
                Return _statedesc
            End Get
            Set(ByVal value As String)
                _statedesc = value
            End Set
        End Property

        Public Property StateCode() As String
            Get
                Return _statecode
            End Get
            Set(ByVal value As String)
                _statecode = value
            End Set
        End Property

        Public Property CityDesc() As String
            Get
                Return _citydesc
            End Get
            Set(ByVal value As String)
                _citydesc = value
            End Set
        End Property

        Public Property CityCode() As String
            Get
                Return _citycode
            End Get
            Set(ByVal value As String)
                _citycode = value
            End Set
        End Property

        Public Property AreaDesc() As String
            Get
                Return _areadesc
            End Get
            Set(ByVal value As String)
                _areadesc = value
            End Set
        End Property

        Public Property AreaCode() As String
            Get
                Return _areacode
            End Get
            Set(ByVal value As String)
                _areacode = value
            End Set
        End Property

        Public Property LicenseDesc() As String
            Get
                Return _licensedesc
            End Get
            Set(ByVal value As String)
                _licensedesc = value
            End Set
        End Property

        Public Property LicenseCode() As String
            Get
                Return _licensecode
            End Get
            Set(ByVal value As String)
                _licensecode = value
            End Set
        End Property

        Public Property CodeDesc() As String
            Get
                Return _codedesc
            End Get
            Set(ByVal value As String)
                _codedesc = value
            End Set
        End Property

        Public Property Code() As String
            Get
                Return _code
            End Get
            Set(ByVal value As String)
                _code = value
            End Set
        End Property

        Public Property ItemCode() As String
            Get
                Return _itemcode
            End Get
            Set(ByVal value As String)
                _itemcode = value
            End Set
        End Property

        Public Property BizRegID() As String
            Get
                Return _BizRegID
            End Get
            Set(ByVal value As String)
                _BizRegID = value
            End Set
        End Property

        Public Property CompanyName() As String
            Get
                Return _companyname
            End Get
            Set(ByVal value As String)
                _companyname = value
            End Set
        End Property

        Public Property LocID() As String
            Get
                Return _LocID
            End Get
            Set(ByVal value As String)
                _LocID = value
            End Set
        End Property

        Public Property BranchName() As String
            Get
                Return _branchname
            End Get
            Set(ByVal value As String)
                _branchname = value
            End Set
        End Property

        Public Property TransID() As String
            Get
                Return _transid
            End Get
            Set(ByVal value As String)
                _transid = value
            End Set
        End Property

        Public Property TransType() As String
            Get
                Return _transtype
            End Get
            Set(ByVal value As String)
                _transtype = value
            End Set
        End Property

        Public Property Status() As String
            Get
                Return _status
            End Get
            Set(ByVal value As String)
                _status = value
            End Set
        End Property

        Public Property Posted() As String
            Get
                Return _posted
            End Get
            Set(ByVal value As String)
                _posted = value
            End Set
        End Property

        Public Property TransDate() As String
            Get
                Return _transdate
            End Get
            Set(ByVal value As String)
                _transdate = value
            End Set
        End Property

        Public Property ItemName() As String
            Get
                Return _itemname
            End Get
            Set(ByVal value As String)
                _itemname = value
            End Set
        End Property

        Public Property Qty() As String
            Get
                Return _qty
            End Get
            Set(ByVal value As String)
                _qty = value
            End Set
        End Property

        Public Property OpeningQty() As String
            Get
                Return _openingqty
            End Get
            Set(ByVal value As String)
                _openingqty = value
            End Set
        End Property

        Public Property ClosingQty() As String
            Get
                Return _closingqty
            End Get
            Set(ByVal value As String)
                _closingqty = value
            End Set
        End Property

        Public Property SUM() As String
            Get
                Return _sum
            End Get
            Set(ByVal value As String)
                _sum = value
            End Set
        End Property

        Public Property CNNo() As String
            Get
                Return _cnno
            End Get
            Set(ByVal value As String)
                _cnno = value
            End Set
        End Property

        Public Property SerialNo() As String
            Get
                Return _serialno
            End Get
            Set(ByVal value As String)
                _serialno = value
            End Set
        End Property

        Public Property RefNo() As String
            Get
                Return _refno
            End Get
            Set(ByVal value As String)
                _refno = value
            End Set
        End Property

        Public Property Dates() As String
            Get
                Return _date
            End Get
            Set(ByVal value As String)
                _date = value
            End Set
        End Property

        Public Property TotalQty() As String
            Get
                Return _totalqty
            End Get
            Set(ByVal value As String)
                _totalqty = value
            End Set
        End Property
    End Class


    Namespace Container
#Region "Class Container"
        Public Class Itmtranshdr
            Public fDocCode As System.String = "DocCode"
            Public fLocID As System.String = "LocID"
            Public fRequestCode As System.String = "RequestCode"
            Public fBatchCode As System.String = "BatchCode"
            Public fTermID As System.String = "TermID"
            Public fRegistedSupp As System.String = "RegistedSupp"
            Public fCompanyID As System.String = "CompanyID"
            Public fCompanyName As System.String = "CompanyName"
            Public fCompanyType As System.String = "CompanyType"
            Public fTransType As System.String = "TransType"
            Public fTransDate As System.String = "TransDate"
            Public fTransInit As System.String = "TransInit"
            Public fTransSrc As System.String = "TransSrc"
            Public fTransDest As System.String = "TransDest"
            Public fInterTrans As System.String = "InterTrans"
            Public fRemark As System.String = "Remark"
            Public fReason As System.String = "Reason"
            Public fPostDate As System.String = "PostDate"
            Public fPosted As System.String = "Posted"
            Public fStatus As System.String = "Status"
            Public fConfirmDate As System.String = "ConfirmDate"
            'Public fExpiredDate As System.String = "ExpiredDate"
            Public fAuthID As System.String = "AuthID"
            Public fAuthPOS As System.String = "AuthPOS"
            Public fCreateDate As System.String = "CreateDate"
            Public fCreateBy As System.String = "CreateBy"
            Public fApproveDate As System.String = "ApproveDate"
            Public fApproveBy As System.String = "ApproveBy"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fUpdateBy As System.String = "UpdateBy"
            Public fActive As System.String = "Active"
            Public fInuse As System.String = "Inuse"
            Public fFlag As System.String = "Flag"
            Public fRowGuid As System.String = "RowGuid"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fTransDateMY As System.String = "TransDateMY"
            Public fLocationName As System.String = "LocationName"
            Public fLiveCal As System.String = "LiveCal"

            Protected _DocCode As System.String
            Private _LocID As System.String
            Private _RequestCode As System.String
            Private _BatchCode As System.String
            Private _TermID As System.Int16
            Private _RegistedSupp As System.Byte
            Private _CompanyID As System.String
            Private _CompanyName As System.String
            Private _CompanyType As System.String
            Private _TransType As System.Byte
            Private _TransDate As System.DateTime
            Private _TransInit As System.String
            Private _TransSrc As System.String
            Private _TransDest As System.String
            Private _InterTrans As System.Byte
            Private _Remark As System.String
            Private _Reason As System.String
            Private _PostDate As System.DateTime
            Private _Posted As System.Byte
            Private _Status As System.Byte
            Private _ConfirmDate As System.DateTime
            'Private _ExpiredDate As System.DateTime
            Private _AuthID As System.String
            Private _AuthPOS As System.String
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _ApproveDate As System.DateTime
            Private _ApproveBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _Active As System.Byte
            Private _Inuse As System.Byte
            Private _RowGuid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _TransDateMY As System.String
            Private _LocationName As System.String
            Private _LiveCal As System.Byte

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property LocationName As System.String
                Get
                    Return _LocationName
                End Get
                Set(ByVal Value As System.String)
                    _LocationName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property TransDateMY As System.String
                Get
                    Return _TransDateMY
                End Get
                Set(ByVal Value As System.String)
                    _TransDateMY = Value
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
            Public Property RequestCode As System.String
                Get
                    Return _RequestCode
                End Get
                Set(ByVal Value As System.String)
                    _RequestCode = Value
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
            Public Property RegistedSupp As System.Byte
                Get
                    Return _RegistedSupp
                End Get
                Set(ByVal Value As System.Byte)
                    _RegistedSupp = Value
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
            Public Property CompanyType As System.String
                Get
                    Return _CompanyType
                End Get
                Set(ByVal Value As System.String)
                    _CompanyType = Value
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
            Public Property TransInit As System.String
                Get
                    Return _TransInit
                End Get
                Set(ByVal Value As System.String)
                    _TransInit = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransSrc As System.String
                Get
                    Return _TransSrc
                End Get
                Set(ByVal Value As System.String)
                    _TransSrc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransDest As System.String
                Get
                    Return _TransDest
                End Get
                Set(ByVal Value As System.String)
                    _TransDest = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property InterTrans As System.Byte
                Get
                    Return _InterTrans
                End Get
                Set(ByVal Value As System.Byte)
                    _InterTrans = Value
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
            Public Property ConfirmDate As System.DateTime
                Get
                    Return _ConfirmDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _ConfirmDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            'Public Property ExpiredDate As System.DateTime
            '    Get
            '        Return _ExpiredDate
            '    End Get
            '    Set(ByVal Value As System.DateTime)
            '        _ExpiredDate = Value
            '    End Set
            'End Property

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
            Public Property ApproveDate As System.DateTime
                Get
                    Return _ApproveDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _ApproveDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
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

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LiveCal As System.Byte
                Get
                    Return _LiveCal
                End Get
                Set(ByVal Value As System.Byte)
                    _LiveCal = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class ItmtranshdrInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "DocCode,LocID,RequestCode,BatchCode,TermID,RegistedSupp,CompanyID,CompanyName,CompanyType,TransType,TransDate,TransInit,TransSrc,TransDest,InterTrans,Remark,Reason,PostDate,Posted,Status,ConfirmDate,AuthID,AuthPOS,CreateDate,CreateBy,ApproveDate,ApproveBy,LastUpdate,UpdateBy,Active,Inuse,Flag,RowGuid,SyncCreate,SyncLastUpd,right(convert(varchar, TransDate, 106), 8) as TransDateMY,LiveCal"
                .CheckFields = "RegistedSupp,TransType,InterTrans,Posted,Status,Active,Inuse,Flag,TermID"
                .TableName = "Itmtranshdr WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "DocCode,LocID,RequestCode,BatchCode,TermID,RegistedSupp,CompanyID,CompanyName,CompanyType,TransType,TransDate,TransInit,TransSrc,TransDest,InterTrans,Remark,Reason,PostDate,Posted,Status,ConfirmDate,AuthID,AuthPOS,CreateDate,CreateBy,ApproveDate,ApproveBy,LastUpdate,UpdateBy,Active,Inuse,Flag,RowGuid,SyncCreate,SyncLastUpd,right(convert(varchar, TransDate, 106), 8) as TransDateMY,LiveCal"
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
    Public Class TransactionsScheme
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
                .FieldName = "LocID"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "RequestCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BatchCode"
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
                .FieldName = "RegistedSupp"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CompanyID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "CompanyName"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CompanyType"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "TransType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "TransDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TransInit"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TransSrc"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TransDest"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "InterTrans"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Reason"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "PostDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Posted"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "ConfirmDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "AuthID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "AuthPOS"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "ApproveDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ApproveBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(27, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(28, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(29, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(30, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(31, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(32, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "RowGuid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(33, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(34, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(35, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LiveCal"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(36, this)
        End Sub

        Public ReadOnly Property DocCode As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property LocID As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property RequestCode As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property BatchCode As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property TermID As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property RegistedSupp As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property CompanyID As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property CompanyName As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property CompanyType As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property TransType As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property TransDate As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property TransInit As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property TransSrc As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property TransDest As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property InterTrans As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property Remark As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property Reason As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property PostDate As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property Posted As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property ConfirmDate As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property AuthID As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property AuthPOS As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property ApproveDate As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property ApproveBy As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(28)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(29)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(30)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(31)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(32)
            End Get
        End Property
        Public ReadOnly Property RowGuid As StrucElement
            Get
                Return MyBase.GetItem(33)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(34)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(35)
            End Get
        End Property
        Public ReadOnly Property LiveCal As StrucElement
            Get
                Return MyBase.GetItem(36)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace
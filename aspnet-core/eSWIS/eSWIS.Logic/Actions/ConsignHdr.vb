﻿
'Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General
Imports System.Data.SqlClient

Namespace Actions
    Public NotInheritable Class ConsignHDR
        Inherits Core2.CoreControl
        'Private ConsignhdrInfo As ConsignhdrInfo = New ConsignhdrInfo
        'Private Log As New SystemLog()

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub


#Region "Data Manipulation-Add,Edit,Del"
        'Private Function Save(ByVal ConsignhdrCont As Container.Consignhdr, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
        '    Dim strSQL As String = ""
        '    Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
        '    Dim rdr As System.Data.SqlClient.SqlDataReader
        '    Save = False
        '    Try
        '        If ConsignhdrCont Is Nothing Then
        '            'Message return
        '        Else
        '            blnExec = False
        '            blnFound = False
        '            blnFlag = False
        '            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
        '                StartSQLControl()
        '                With ConsignhdrInfo.MyInfo
        '                    strSQL = BuildSelect(.CheckFields, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignhdrCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignhdrCont.ContractNo) & "'")
        '                End With
        '                rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
        '                blnExec = True

        '                If rdr Is Nothing = False Then
        '                    With rdr
        '                        If .Read Then
        '                            blnFound = True
        '                            If Convert.ToInt16(.Item("Flag")) = 0 Then
        '                                'Found but deleted
        '                                blnFlag = False
        '                            Else
        '                                'Found and active
        '                                blnFlag = True
        '                            End If
        '                        End If
        '                        .Close()
        '                    End With
        '                End If
        '            End If

        '            If blnExec Then
        '                If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
        '                    message = "Record already exist"
        '                    'Throw New ApplicationException("210011: Record already exist")
        '                    'Item found & active
        '                    'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIconInformation,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
        '                    'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
        '                    Return False
        '                Else
        '                    StartSQLControl()
        '                    With objSQL
        '                        .TableName = "Consignhdr WITH (ROWLOCK)"
        '                        .AddField("ReferID", ConsignhdrCont.ReferID, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransType", ConsignhdrCont.TransType, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransDate", ConsignhdrCont.TransDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("Status", ConsignhdrCont.Status, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("TransportStatus", ConsignhdrCont.TransportStatus, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("ReceiveStatus", ConsignhdrCont.ReceiveStatus, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("GeneratorID", ConsignhdrCont.GeneratorID, SQLControl.EnumDataType.dtString)
        '                        .AddField("GeneratorLocID", ConsignhdrCont.GeneratorLocID, SQLControl.EnumDataType.dtString)
        '                        .AddField("GeneratorAddress", ConsignhdrCont.GeneratorAddress, SQLControl.EnumDataType.dtString)
        '                        .AddField("GeneratorPIC", ConsignhdrCont.GeneratorPIC, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("GeneratorPos", ConsignhdrCont.GeneratorPos, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("GeneratorTelNo", ConsignhdrCont.GeneratorTelNo, SQLControl.EnumDataType.dtString)
        '                        .AddField("GeneratorFaxNo", ConsignhdrCont.GeneratorFaxNo, SQLControl.EnumDataType.dtString)
        '                        .AddField("GeneratorEmailAddress", ConsignhdrCont.GeneratorEmailAddress, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("TransporterID", ConsignhdrCont.TransporterID, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransporterLocID", ConsignhdrCont.TransporterLocID, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransporterAddress", ConsignhdrCont.TransporterAddress, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransporterPIC", ConsignhdrCont.TransporterPIC, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("TransporterPos", ConsignhdrCont.TransporterPos, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("TransporterTelNo", ConsignhdrCont.TransporterTelNo, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransporterFaxNo", ConsignhdrCont.TransporterFaxNo, SQLControl.EnumDataType.dtString)
        '                        .AddField("TransporterEmailAddress", ConsignhdrCont.TransporterEmailAddress, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("VehicleID", ConsignhdrCont.VehicleID, SQLControl.EnumDataType.dtString)
        '                        .AddField("DriverID", ConsignhdrCont.DriverID, SQLControl.EnumDataType.dtString)
        '                        .AddField("DriverName", ConsignhdrCont.DriverName, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("ICNo", ConsignhdrCont.ICNo, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("ReceiverID", ConsignhdrCont.ReceiverID, SQLControl.EnumDataType.dtString)
        '                        .AddField("ReceiverLocID", ConsignhdrCont.ReceiverLocID, SQLControl.EnumDataType.dtString)
        '                        .AddField("ReceiverAddress", ConsignhdrCont.ReceiverAddress, SQLControl.EnumDataType.dtString)
        '                        .AddField("ReceiverPIC", ConsignhdrCont.ReceiverPIC, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("ReceiverPos", ConsignhdrCont.ReceiverPos, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("ReceiverTelNo", ConsignhdrCont.ReceiverTelNo, SQLControl.EnumDataType.dtString)
        '                        .AddField("ReceiverFaxNo", ConsignhdrCont.ReceiverFaxNo, SQLControl.EnumDataType.dtString)
        '                        .AddField("ReceiverEmailAddress", ConsignhdrCont.ReceiverEmailAddress, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("PremiseID", ConsignhdrCont.PremiseID, SQLControl.EnumDataType.dtString)
        '                        .AddField("TempStorage1", ConsignhdrCont.TempStorage1, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("TempStorage2", ConsignhdrCont.TempStorage2, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("DeliveryDate", ConsignhdrCont.DeliveryDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("TargetTransportDate", ConsignhdrCont.TargetTransportDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("TransportDate", ConsignhdrCont.TransportDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("TargetReceiveDate", ConsignhdrCont.TargetReceiveDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("ReceiveDate", ConsignhdrCont.ReceiveDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("DeliveryRemark", ConsignhdrCont.DeliveryRemark, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("ReceiveRemark", ConsignhdrCont.ReceiveRemark, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("RejectRemark", ConsignhdrCont.RejectRemark, SQLControl.EnumDataType.dtStringN)
        '                        .AddField("CreateDate", ConsignhdrCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("CreateBy", ConsignhdrCont.CreateBy, SQLControl.EnumDataType.dtString)
        '                        .AddField("LastUpdate", ConsignhdrCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
        '                        .AddField("UpdateBy", ConsignhdrCont.UpdateBy, SQLControl.EnumDataType.dtString)
        '                        '.AddField("rowguid", ConsignhdrCont.rowguid, SQLControl.EnumDataType.dtString)
        '                        .AddField("IsHost", ConsignhdrCont.IsHost, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("IsConfirm", ConsignhdrCont.IsConfirm, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("IsNew", ConsignhdrCont.IsNew, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("LastSyncBy", ConsignhdrCont.LastSyncBy, SQLControl.EnumDataType.dtString)
        '                        .AddField("Active", ConsignhdrCont.Active, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("Flag", ConsignhdrCont.Flag, SQLControl.EnumDataType.dtNumeric)
        '                        .AddField("CancelReason", ConsignhdrCont.CancelReason, SQLControl.EnumDataType.dtString)

        '                        Select Case pType
        '                            Case SQLControl.EnumSQLType.stInsert
        '                                If blnFound = True And blnFlag = False Then
        '                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignhdrCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignhdrCont.ContractNo) & "'")
        '                                Else
        '                                    If blnFound = False Then
        '                                        .AddField("TransID", ConsignhdrCont.TransID, SQLControl.EnumDataType.dtString)
        '                                        .AddField("ContractNo", ConsignhdrCont.ContractNo, SQLControl.EnumDataType.dtString)
        '                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
        '                                    End If
        '                                End If
        '                            Case SQLControl.EnumSQLType.stUpdate
        '                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignhdrCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignhdrCont.ContractNo) & "'")
        '                        End Select
        '                    End With
        '                    Try
        '                        'execute
        '                        objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
        '                        Return True

        '                    Catch axExecute As Exception
        '                        If pType = SQLControl.EnumSQLType.stInsert Then
        '                            message = axExecute.Message.ToString()
        '                            'Throw New ApplicationException("210002 " & axExecute.Message.ToString())
        '                        Else
        '                            message = axExecute.Message.ToString()
        '                            'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
        '                        End If
        '                        Log.Notifier.Notify(axExecute)
        '                        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignHDR", axExecute.Message & strSQL, axExecute.StackTrace)
        '                        Return False

        '                    Finally
        '                        objSQL.Dispose()
        '                    End Try
        '                End If

        '            End If
        '        End If
        '    Catch axAssign As ApplicationException
        '        'Throw axAssign
        '        message = axAssign.Message.ToString()
        '        Log.Notifier.Notify(axAssign)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignHDR", axAssign.Message, axAssign.StackTrace)
        '        Return False
        '    Catch exAssign As SystemException
        '        'Throw exAssign
        '        message = exAssign.Message.ToString()
        '        Log.Notifier.Notify(exAssign)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignHDR", exAssign.Message, exAssign.StackTrace)
        '        Return False
        '    Finally
        '        ConsignhdrCont = Nothing
        '        rdr = Nothing
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function

        ''ADD
        'Public Function Insert(ByVal ConsignhdrCont As Container.Consignhdr, ByRef message As String) As Boolean
        '    Return Save(ConsignhdrCont, SQLControl.EnumSQLType.stInsert, message)
        'End Function

        ''AMEND
        'Public Function Update(ByVal ConsignhdrCont As Container.Consignhdr, ByRef message As String) As Boolean
        '    Return Save(ConsignhdrCont, SQLControl.EnumSQLType.stUpdate, message)
        'End Function

        'Public Function Delete(ByVal ConsignhdrCont As Container.Consignhdr, ByRef message As String) As Boolean
        '    Dim strSQL As String
        '    Dim blnFound As Boolean
        '    Dim blnInUse As Boolean
        '    Dim rdr As System.Data.SqlClient.SqlDataReader
        '    Delete = False
        '    blnFound = False
        '    blnInUse = False
        '    Try
        '        If ConsignhdrCont Is Nothing Then
        '            'Error Message
        '        Else
        '            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
        '                StartSQLControl()
        '                With ConsignhdrInfo.MyInfo
        '                    strSQL = BuildSelect(.CheckFields, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignhdrCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignhdrCont.ContractNo) & "'")
        '                End With
        '                rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

        '                If rdr Is Nothing = False Then
        '                    With rdr
        '                        If .Read Then
        '                            blnFound = True
        '                            If Convert.ToInt16(.Item("InUse")) = 1 Then
        '                                blnInUse = True
        '                            End If
        '                        End If
        '                        .Close()
        '                    End With
        '                End If

        '                If blnFound = True And blnInUse = True Then
        '                    With objSQL
        '                        strSQL = BuildUpdate("Consignhdr WITH (ROWLOCK)", " SET Flag = 0" & _
        '                        " , LastUpdate = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtDateTime, ConsignhdrCont.LastUpdate) & "' , UpdateBy = '" & _
        '                        objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignhdrCont.UpdateBy) & "' WHERE " & _
        '                        " TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignhdrCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignhdrCont.ContractNo) & "'")
        '                    End With
        '                End If

        '                If blnFound = True And blnInUse = False Then
        '                    strSQL = BuildDelete("Consignhdr WITH (ROWLOCK)", "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignhdrCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignhdrCont.ContractNo) & "'")
        '                End If

        '                Try
        '                    'execute
        '                    objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
        '                    Return True
        '                Catch exExecute As Exception
        '                    message = exExecute.Message.ToString()
        '                    Log.Notifier.Notify(exExecute)
        '                    Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignHDR", exExecute.Message & strSQL, exExecute.StackTrace)
        '                    Return False
        '                    'Throw New ApplicationException("210006 " & exExecute.Message.ToString())
        '                End Try
        '            End If
        '        End If

        '    Catch axDelete As ApplicationException
        '        message = axDelete.Message.ToString()
        '        Log.Notifier.Notify(axDelete)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignHDR", axDelete.Message, axDelete.StackTrace)
        '        Return False
        '        'Throw axDelete
        '    Catch exDelete As Exception
        '        message = exDelete.Message.ToString()
        '        Log.Notifier.Notify(exDelete)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignHDR", exDelete.Message, exDelete.StackTrace)
        '        Return False
        '        'Throw exDelete
        '    Finally
        '        ConsignhdrCont = Nothing
        '        rdr = Nothing
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function

        ''Add Cancel
        'Public Function Cancel(ByVal CNhdrCont As Container.Consignhdr, ByVal UpdateBy As String, ByRef message As String) As Boolean
        '    Dim strSQL As String
        '    Dim blnFound As Boolean
        '    Dim blnInUse As Boolean
        '    Dim rdr As System.Data.SqlClient.SqlDataReader
        '    Cancel = False
        '    blnFound = False
        '    blnInUse = False
        '    Try
        '        If CNhdrCont Is Nothing Then
        '            'Error Message
        '        Else
        '            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
        '                StartSQLControl()
        '                With ConsignhdrInfo.MyInfo
        '                    strSQL = BuildSelect(.CheckFields, .TableName, "ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CNhdrCont.ContractNo) & "'")
        '                End With
        '                rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

        '                If rdr Is Nothing = False Then
        '                    With rdr
        '                        If .Read Then
        '                            blnFound = True
        '                            'If Convert.ToInt16(.Item("InUse")) = 1 Then
        '                            '    blnInUse = True
        '                            'End If
        '                        End If
        '                        .Close()
        '                    End With
        '                End If

        '                'If blnFound = True And blnInUse = True Then
        '                If blnFound = True Then
        '                    With objSQL
        '                        'If Cancel then set Status to 2 (Canceled)
        '                        strSQL = BuildUpdate("Consignhdr WITH (ROWLOCK)", " SET Status = 2, Flag = 0" & _
        '                        " , LastUpdate = '" & String.Format("{0:yyyy-MM-dd HH:mm:ss}", Now) & "' , UpdateBy = '" & _
        '                        objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, UpdateBy) & "' WHERE " & _
        '                        " ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CNhdrCont.ContractNo) & "'")
        '                    End With
        '                End If

        '                Try
        '                    'execute header
        '                    objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

        '                    Return True
        '                Catch exExecute As Exception
        '                    message = exExecute.Message.ToString()
        '                    Log.Notifier.Notify(exExecute)
        '                    Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignHDR", exExecute.Message, exExecute.StackTrace)
        '                    Return False

        '                End Try
        '            End If
        '        End If

        '    Catch axDelete As ApplicationException
        '        message = axDelete.Message.ToString()
        '        Log.Notifier.Notify(axDelete)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignHDR", axDelete.Message, axDelete.StackTrace)
        '        Return False
        '        'Throw axDelete
        '    Catch exDelete As Exception
        '        message = exDelete.Message.ToString()
        '        Log.Notifier.Notify(exDelete)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignHDR", exDelete.Message, exDelete.StackTrace)
        '        Return False
        '        'Throw exDelete
        '    Finally
        '        CNhdrCont = Nothing
        '        rdr = Nothing
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function

        'Public Function Cancel_BackDateCN(ByVal CNhdrCont As Container.Consignhdr, ByVal UpdateBy As String, ByRef message As String) As Boolean
        '    Dim strSQL As String
        '    Dim blnFound As Boolean
        '    Dim blnInUse As Boolean
        '    Dim rdr As System.Data.SqlClient.SqlDataReader
        '    Cancel_BackDateCN = False
        '    blnFound = False
        '    blnInUse = False
        '    Try
        '        If CNhdrCont Is Nothing Then
        '            'Error Message
        '        Else
        '            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
        '                StartSQLControl()
        '                With ConsignhdrInfo.MyInfo
        '                    strSQL = BuildSelect(.CheckFields, .TableName, "ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CNhdrCont.ContractNo) & "'")
        '                End With
        '                rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

        '                If rdr Is Nothing = False Then
        '                    With rdr
        '                        If .Read Then
        '                            blnFound = True
        '                            'If Convert.ToInt16(.Item("InUse")) = 1 Then
        '                            '    blnInUse = True
        '                            'End If
        '                        End If
        '                        .Close()
        '                    End With
        '                End If

        '                'If blnFound = True And blnInUse = True Then
        '                If blnFound = True Then
        '                    With objSQL
        '                        'If Cancel then set Status to 2 (Canceled)
        '                        strSQL = BuildUpdate("Consignhdr WITH (ROWLOCK)", " SET Status = 15, Flag = 0" & _
        '                        " , LastUpdate = '" & String.Format("{0:yyyy-MM-dd HH:mm:ss}", Now) & "' , UpdateBy = '" & _
        '                        objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, UpdateBy) & "' WHERE " & _
        '                        " ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CNhdrCont.ContractNo) & "'")
        '                    End With
        '                End If

        '                Try
        '                    'execute header
        '                    objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

        '                    Return True
        '                Catch exExecute As Exception
        '                    message = exExecute.Message.ToString()
        '                    Log.Notifier.Notify(exExecute)
        '                    Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignHDR", exExecute.Message, exExecute.StackTrace)
        '                    Return False

        '                End Try
        '            End If
        '        End If

        '    Catch axDelete As ApplicationException
        '        message = axDelete.Message.ToString()
        '        Log.Notifier.Notify(axDelete)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignHDR", axDelete.Message, axDelete.StackTrace)
        '        Return False
        '        'Throw axDelete
        '    Catch exDelete As Exception
        '        message = exDelete.Message.ToString()
        '        Log.Notifier.Notify(exDelete)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignHDR", exDelete.Message, exDelete.StackTrace)
        '        Return False
        '        'Throw exDelete
        '    Finally
        '        CNhdrCont = Nothing
        '        rdr = Nothing
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function

        ''Add Cancel Submit
        'Public Function CancelSubmit(ByVal CNhdrCont As Container.Consignhdr, ByVal UpdateBy As String, ByRef message As String) As Boolean
        '    Dim strSQL As String
        '    Dim blnFound As Boolean
        '    Dim blnInUse As Boolean
        '    Dim rdr As System.Data.SqlClient.SqlDataReader
        '    Dim ListSQL As ArrayList = New ArrayList()
        '    CancelSubmit = False
        '    blnFound = False
        '    blnInUse = False
        '    Try
        '        If CNhdrCont Is Nothing Then
        '            'Error Message
        '        Else
        '            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
        '                StartSQLControl()
        '                With ConsignhdrInfo.MyInfo
        '                    strSQL = BuildSelect(.CheckFields, .TableName, "ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CNhdrCont.ContractNo) & "'")
        '                End With
        '                rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

        '                If rdr Is Nothing = False Then
        '                    With rdr
        '                        If .Read Then
        '                            blnFound = True
        '                            'If Convert.ToInt16(.Item("InUse")) = 1 Then
        '                            '    blnInUse = True
        '                            'End If
        '                        End If
        '                        .Close()
        '                    End With
        '                End If

        '                'If blnFound = True And blnInUse = True Then
        '                If blnFound = True Then
        '                    With objSQL
        '                        'If Cancel then set Status to 12 (Cancel Submit)
        '                        strSQL = BuildUpdate("Consignhdr WITH (ROWLOCK)", " SET Status = 12" &
        '                        " , LastUpdate = '" & String.Format("{0:yyyy-MM-dd HH:mm:ss}", Now) & "' , UpdateBy = '" &
        '                        objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, UpdateBy) & "' WHERE " &
        '                        " ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CNhdrCont.ContractNo) & "'")
        '                    End With

        '                    ListSQL.Add(strSQL)

        '                End If

        '                'execute store proc
        '                'strSQL = "Exec sp_LiveInvDeductByCN '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CNhdrCont.ContractNo) & "'"
        '                'ListSQL.Add(strSQL)

        '                Try
        '                    'execute header
        '                    'objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
        '                    objConn.BatchExecute(ListSQL, CommandType.Text)

        '                    Return True
        '                Catch exExecute As Exception
        '                    message = exExecute.Message.ToString()
        '                    Log.Notifier.Notify(exExecute)
        '                    Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignHDR", exExecute.Message, exExecute.StackTrace)
        '                    Return False

        '                End Try
        '            End If
        '        End If

        '    Catch axDelete As ApplicationException
        '        message = axDelete.Message.ToString()
        '        Log.Notifier.Notify(axDelete)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignHDR", axDelete.Message, axDelete.StackTrace)
        '        Return False
        '        'Throw axDelete
        '    Catch exDelete As Exception
        '        message = exDelete.Message.ToString()
        '        Log.Notifier.Notify(exDelete)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignHDR", exDelete.Message, exDelete.StackTrace)
        '        Return False
        '        'Throw exDelete
        '    Finally
        '        CNhdrCont = Nothing
        '        rdr = Nothing
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function
#End Region

#Region "Data Selection"
        'Public Overloads Function GetConsignHDR(ByVal TransID As System.String, ByVal ContractNo As System.String) As Container.Consignhdr
        '    Dim rConsignhdr As Container.Consignhdr = Nothing
        '    Dim dtTemp As DataTable = Nothing
        '    Dim lstField As New List(Of String)
        '    Dim strSQL As String = Nothing

        '    Try
        '        If StartConnection() = True Then
        '            StartSQLControl()
        '            With ConsignhdrInfo.MyInfo
        '                strSQL = BuildSelect(.FieldsList, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ContractNo) & "'")
        '                dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '                Dim rowCount As Integer = 0
        '                If dtTemp Is Nothing = False Then
        '                    If dtTemp.Rows.Count > 0 Then
        '                        Dim drRow = dtTemp.Rows(0)
        '                        rConsignhdr = New Container.Consignhdr
        '                        rConsignhdr.TransID = drRow.Item("TransID")
        '                        rConsignhdr.ContractNo = drRow.Item("ContractNo")
        '                        rConsignhdr.ReferID = drRow.Item("ReferID")
        '                        rConsignhdr.TransType = drRow.Item("TransType")
        '                        rConsignhdr.TransDate = drRow.Item("TransDate")
        '                        rConsignhdr.TargetTransportDate = IIf(IsDBNull(drRow.Item("TargetTransportDate")), Now, drRow.Item("TargetTransportDate"))
        '                        rConsignhdr.TransportDate = IIf(IsDBNull(drRow.Item("TransportDate")), Now, drRow.Item("TransportDate"))
        '                        rConsignhdr.TargetReceiveDate = IIf(IsDBNull(drRow.Item("TargetReceiveDate")), Now, drRow.Item("TargetReceiveDate"))
        '                        rConsignhdr.ReceiveDate = IIf(IsDBNull(drRow.Item("ReceiveDate")), Now, drRow.Item("ReceiveDate"))
        '                        rConsignhdr.Status = drRow.Item("Status")
        '                        rConsignhdr.TransportStatus = drRow.Item("TransportStatus")
        '                        rConsignhdr.ReceiveStatus = drRow.Item("ReceiveStatus")
        '                        rConsignhdr.GeneratorID = drRow.Item("GeneratorID")
        '                        rConsignhdr.GeneratorLocID = drRow.Item("GeneratorLocID")
        '                        rConsignhdr.GeneratorAddress = drRow.Item("GeneratorAddress")
        '                        rConsignhdr.GeneratorPIC = drRow.Item("GeneratorPIC")
        '                        rConsignhdr.GeneratorPos = drRow.Item("GeneratorPos")
        '                        rConsignhdr.GeneratorTelNo = drRow.Item("GeneratorTelNo")
        '                        rConsignhdr.GeneratorFaxNo = drRow.Item("GeneratorFaxNo")
        '                        rConsignhdr.GeneratorEmailAddress = drRow.Item("GeneratorEmailAddress")
        '                        rConsignhdr.TransporterID = drRow.Item("TransporterID")
        '                        rConsignhdr.TransporterLocID = drRow.Item("TransporterLocID")
        '                        rConsignhdr.TransporterAddress = drRow.Item("TransporterAddress")
        '                        rConsignhdr.TransporterPIC = drRow.Item("TransporterPIC")
        '                        rConsignhdr.TransporterPos = drRow.Item("TransporterPos")
        '                        rConsignhdr.TransporterTelNo = drRow.Item("TransporterTelNo")
        '                        rConsignhdr.TransporterFaxNo = drRow.Item("TransporterFaxNo")
        '                        rConsignhdr.TransporterEmailAddress = drRow.Item("TransporterEmailAddress")
        '                        rConsignhdr.VehicleID = drRow.Item("VehicleID")
        '                        rConsignhdr.DriverID = drRow.Item("DriverID")
        '                        rConsignhdr.DriverName = drRow.Item("DriverName")
        '                        rConsignhdr.ICNo = drRow.Item("ICNo")
        '                        rConsignhdr.ReceiverID = drRow.Item("ReceiverID")
        '                        rConsignhdr.ReceiverLocID = drRow.Item("ReceiverLocID")
        '                        rConsignhdr.ReceiverAddress = drRow.Item("ReceiverAddress")
        '                        rConsignhdr.ReceiverPIC = drRow.Item("ReceiverPIC")
        '                        rConsignhdr.ReceiverPos = drRow.Item("ReceiverPos")
        '                        rConsignhdr.ReceiverTelNo = drRow.Item("ReceiverTelNo")
        '                        rConsignhdr.ReceiverFaxNo = drRow.Item("ReceiverFaxNo")
        '                        rConsignhdr.ReceiverEmailAddress = drRow.Item("ReceiverEmailAddress")
        '                        rConsignhdr.TempStorage1 = drRow.Item("TempStorage1")
        '                        rConsignhdr.TempStorage2 = drRow.Item("TempStorage2")
        '                        rConsignhdr.DeliveryRemark = drRow.Item("DeliveryRemark")
        '                        rConsignhdr.ReceiveRemark = drRow.Item("ReceiveRemark")
        '                        rConsignhdr.RejectRemark = drRow.Item("RejectRemark")
        '                        rConsignhdr.CreateBy = drRow.Item("CreateBy")
        '                        rConsignhdr.UpdateBy = drRow.Item("UpdateBy")
        '                        rConsignhdr.rowguid = drRow.Item("rowguid")
        '                        rConsignhdr.SyncCreate = drRow.Item("SyncCreate")
        '                        rConsignhdr.SyncLastUpd = drRow.Item("SyncLastUpd")
        '                        rConsignhdr.IsHost = drRow.Item("IsHost")
        '                        rConsignhdr.IsConfirm = drRow.Item("IsConfirm")
        '                        rConsignhdr.IsNew = drRow.Item("IsNew")
        '                        rConsignhdr.LastSyncBy = drRow.Item("LastSyncBy")
        '                        rConsignhdr.Flag = drRow.Item("Flag")
        '                        rConsignhdr.Active = drRow.Item("Active")
        '                        rConsignhdr.CancelReason = drRow.Item("CancelReason")
        '                        If Not IsDBNull(drRow.Item("SubmissionDate")) Then
        '                            rConsignhdr.SubmissionDate = drRow.Item("SubmissionDate")
        '                        End If
        '                    Else
        '                        rConsignhdr = Nothing
        '                    End If
        '                Else
        '                    rConsignhdr = Nothing
        '                End If
        '            End With
        '        End If
        '        Return rConsignhdr
        '    Catch ex As Exception
        '        Log.Notifier.Notify(ex)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignHDR", ex.Message, ex.StackTrace)
        '        'Throw ex
        '    Finally
        '        rConsignhdr = Nothing
        '        dtTemp = Nothing
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function

        'Public Overloads Function GetConsignHDR(ByVal TransID As System.String, ByVal ContractNo As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Consignhdr)
        '    Dim rConsignhdr As Container.Consignhdr = Nothing
        '    Dim lstConsignhdr As List(Of Container.Consignhdr) = Nothing
        '    Dim dtTemp As DataTable = Nothing
        '    Dim lstField As New List(Of String)
        '    Dim strSQL As String = Nothing
        '    Dim strDesc As String = ""
        '    Try
        '        If StartConnection() = True Then
        '            StartSQLControl()
        '            With ConsignhdrInfo.MyInfo
        '                If DecendingOrder Then
        '                    strDesc = " Order by ByVal TransID As System.String, ByVal ContractNo As System.String DESC"
        '                End If
        '                strSQL = BuildSelect(.FieldsList, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ContractNo) & "'" & strDesc)
        '                dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '                Dim rowCount As Integer = 0
        '                If dtTemp Is Nothing = False Then
        '                    For Each drRow As DataRow In dtTemp.Rows
        '                        rConsignhdr = New Container.Consignhdr
        '                        rConsignhdr.TransID = drRow.Item("TransID")
        '                        rConsignhdr.ContractNo = drRow.Item("ContractNo")
        '                        rConsignhdr.ReferID = drRow.Item("ReferID")
        '                        rConsignhdr.TransType = drRow.Item("TransType")
        '                        rConsignhdr.TransDate = drRow.Item("TransDate")
        '                        rConsignhdr.Status = drRow.Item("Status")
        '                        rConsignhdr.TransportStatus = drRow.Item("TransportStatus")
        '                        rConsignhdr.ReceiveStatus = drRow.Item("ReceiveStatus")
        '                        rConsignhdr.GeneratorID = drRow.Item("GeneratorID")
        '                        rConsignhdr.GeneratorLocID = drRow.Item("GeneratorLocID")
        '                        rConsignhdr.GeneratorAddress = drRow.Item("GeneratorAddress")
        '                        rConsignhdr.GeneratorPIC = drRow.Item("GeneratorPIC")
        '                        rConsignhdr.GeneratorPos = drRow.Item("GeneratorPos")
        '                        rConsignhdr.GeneratorTelNo = drRow.Item("GeneratorTelNo")
        '                        rConsignhdr.GeneratorFaxNo = drRow.Item("GeneratorFaxNo")
        '                        rConsignhdr.GeneratorEmailAddress = drRow.Item("GeneratorEmailAddress")
        '                        rConsignhdr.TransporterID = drRow.Item("TransporterID")
        '                        rConsignhdr.TransporterLocID = drRow.Item("TransporterLocID")
        '                        rConsignhdr.TransporterAddress = drRow.Item("TransporterAddress")
        '                        rConsignhdr.TransporterPIC = drRow.Item("TransporterPIC")
        '                        rConsignhdr.TransporterPos = drRow.Item("TransporterPos")
        '                        rConsignhdr.TransporterTelNo = drRow.Item("TransporterTelNo")
        '                        rConsignhdr.TransporterFaxNo = drRow.Item("TransporterFaxNo")
        '                        rConsignhdr.TransporterEmailAddress = drRow.Item("TransporterEmailAddress")
        '                        rConsignhdr.VehicleID = drRow.Item("VehicleID")
        '                        rConsignhdr.DriverID = drRow.Item("DriverID")
        '                        rConsignhdr.DriverName = drRow.Item("DriverName")
        '                        rConsignhdr.ICNo = drRow.Item("ICNo")
        '                        rConsignhdr.ReceiverID = drRow.Item("ReceiverID")
        '                        rConsignhdr.ReceiverLocID = drRow.Item("ReceiverLocID")
        '                        rConsignhdr.ReceiverAddress = drRow.Item("ReceiverAddress")
        '                        rConsignhdr.ReceiverPIC = drRow.Item("ReceiverPIC")
        '                        rConsignhdr.ReceiverPos = drRow.Item("ReceiverPos")
        '                        rConsignhdr.ReceiverTelNo = drRow.Item("ReceiverTelNo")
        '                        rConsignhdr.ReceiverFaxNo = drRow.Item("ReceiverFaxNo")
        '                        rConsignhdr.ReceiverEmailAddress = drRow.Item("ReceiverEmailAddress")
        '                        rConsignhdr.TempStorage1 = drRow.Item("TempStorage1")
        '                        rConsignhdr.TempStorage2 = drRow.Item("TempStorage2")
        '                        rConsignhdr.DeliveryRemark = drRow.Item("DeliveryRemark")
        '                        rConsignhdr.ReceiveRemark = drRow.Item("ReceiveRemark")
        '                        rConsignhdr.RejectRemark = drRow.Item("RejectRemark")
        '                        rConsignhdr.CreateBy = drRow.Item("CreateBy")
        '                        rConsignhdr.UpdateBy = drRow.Item("UpdateBy")
        '                        rConsignhdr.rowguid = drRow.Item("rowguid")
        '                        rConsignhdr.SyncCreate = drRow.Item("SyncCreate")
        '                        rConsignhdr.SyncLastUpd = drRow.Item("SyncLastUpd")
        '                        rConsignhdr.IsHost = drRow.Item("IsHost")
        '                        rConsignhdr.IsConfirm = drRow.Item("IsConfirm")
        '                        rConsignhdr.IsNew = drRow.Item("IsNew")
        '                        rConsignhdr.LastSyncBy = drRow.Item("LastSyncBy")
        '                        rConsignhdr.Flag = drRow.Item("Flag")
        '                        rConsignhdr.Active = drRow.Item("Active")
        '                    Next
        '                    lstConsignhdr.Add(rConsignhdr)
        '                Else
        '                    rConsignhdr = Nothing
        '                End If
        '                Return lstConsignhdr
        '            End With
        '        End If
        '    Catch ex As Exception
        '        Log.Notifier.Notify(ex)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignHDR", ex.Message, ex.StackTrace)
        '        'Throw ex
        '    Finally
        '        rConsignhdr = Nothing
        '        lstConsignhdr = Nothing
        '        lstField = Nothing
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function

        ''Get ConsignHDR filtered by GeneratorID
        'Public Function GetConsignmentByLocation(ByVal LocationID As String, ByVal Type As String, _
        '                                         Optional ByVal StartDate As String = Nothing, _
        '                                         Optional ByVal EndDate As String = Nothing) As List(Of Actions.Container.Consignhdr)
        '    Dim rConsignHDR As Container.Consignhdr = Nothing
        '    Dim listConsignHDR As List(Of Container.Consignhdr) = Nothing
        '    Dim dtTemp As DataTable = Nothing

        '    If StartConnection() = True Then
        '        StartSQLControl()
        '        strSQL = "select h.GeneratorID, be.BizRegID, be.CompanyName, h.ContractNo, h.TransDate, h.TransType, d.SerialNo, h.ReferID," & _
        '                 " h.GeneratorLocID, g.BranchName as GeneratorName, h.GeneratorAddress, h.GeneratorPIC, h.GeneratorPos, cg.CodeDesc as GeneratorPosDesc, h.GeneratorTelNo, h.GeneratorFaxNo, g.ContactMobile as GeneratorMobile, h.GeneratorEmailAddress," & _
        '                 " h.TransporterLocID, t.BranchName as TransporterName, h.TransporterAddress, h.TransporterPIC, h.TransporterPos, ct.CodeDesc as TransporterPosDesc, h.TransporterTelNo, h.TransporterFaxNo, t.ContactMobile as TransporterMobile, h.TransporterEmailAddress," & _
        '                 " h.ReceiverLocID, r.BranchName as ReceiverName, h.ReceiverAddress, h.ReceiverPIC, h.ReceiverPos, cr.CodeDesc as ReceiverPosDesc, h.ReceiverTelNo, h.ReceiverFaxNo, r.ContactMobile as ReceiverMobile, h.ReceiverEmailAddress, h.DriverName, h.ICNo, h.DriverID, v.RegNo," & _
        '                 " h.DeliveryRemark, h.ReceiveRemark, h.TempStorage1, h.Status, Case when h.status = 8 then 'Received' when h.status = 9 then 'Rejected' else '' end as StatusDesc, h.ReceiveDate " & _
        '                 " from CONSIGNHDR h with (nolock) left join CONSIGNDTL d with (nolock) on h.ContractNo=d.ContractNo" & _
        '                 " left join BIZENTITY be with (nolock) on h.Generatorid=be.BizRegID" & _
        '                 " left join BIZLOCATE g with (nolock) on h.GeneratorID=g.BizRegID and h.GeneratorLocID=g.BizLocID " & _
        '                 " left join BIZLOCATE t with (nolock) on h.TransporterID=t.BizRegID and h.TransporterLocID=t.BizLocID " & _
        '                 " left join BIZLOCATE r with (nolock) on h.ReceiverID=r.BizRegID and h.ReceiverLocID=r.BizLocID " & _
        '                 " left join CODEMASTER cr with (nolock) on h.ReceiverPos=cr.Code and cr.CodeType='DSN'" &
        '                 " left join CODEMASTER cg with (nolock) on h.GeneratorPos=cg.Code and cg.CodeType='DSN'" &
        '                 " left join CODEMASTER ct with (nolock) on h.TransporterPos=ct.Code and ct.CodeType='DSN'" &
        '                 " left join Vehicle v with (nolock) on v.VehicleID=h.VehicleID"

        '        If Type = "WG" Then
        '            strSQL &= " where GeneratorLocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocationID) & "' AND h.Status not in (2,8,9,11,12,13,14,15)"
        '        ElseIf Type = "WR" Then
        '            strSQL &= " where ReceiverLocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocationID) & "' AND h.Status IN('1','4','6')"
        '        ElseIf Type = "WG_API" Then
        '            strSQL &= " where GeneratorLocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocationID) & "' AND h.Status not in (2,11,12,13,14,15)"
        '        End If

        '        If StartDate IsNot Nothing AndAlso StartDate <> "" AndAlso EndDate IsNot Nothing AndAlso EndDate <> "" Then
        '            strSQL &= " And (TransDate >= convert(datetime, '" & StartDate & "', 103) and TransDate <= convert(datetime, '" & EndDate & "', 103))"
        '        Else
        '            strSQL &= " And (TransDate >= dateadd(dd,-day(getdate())+1,getdate()) and TransDate <= getDate())"
        '        End If

        '        strSQL &= " ORDER BY TransDate DESC"

        '        Try
        '            dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
        '            If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
        '                listConsignHDR = New List(Of Container.Consignhdr)
        '                For Each rowHDR As DataRow In dtTemp.Rows
        '                    rConsignHDR = New Container.Consignhdr
        '                    With rConsignHDR
        '                        .GeneratorID = rowHDR.Item("GeneratorID").ToString
        '                        .CompanyName = rowHDR.Item("CompanyName").ToString
        '                        .ContractNo = rowHDR.Item("ContractNo").ToString
        '                        .TransDate = rowHDR.Item("TransDate").ToString
        '                        .TransType = rowHDR.Item("TransType").ToString
        '                        .ReferID = rowHDR.Item("ReferID").ToString

        '                        .GeneratorLocID = rowHDR.Item("GeneratorLocID").ToString
        '                        .GeneratorName = rowHDR.Item("GeneratorName").ToString
        '                        .GeneratorAddress = rowHDR.Item("GeneratorAddress").ToString
        '                        .GeneratorPIC = rowHDR.Item("GeneratorPIC").ToString
        '                        .GeneratorPos = rowHDR.Item("GeneratorPos").ToString
        '                        .GeneratorPosDesc = rowHDR.Item("GeneratorPosDesc").ToString
        '                        .GeneratorTelNo = rowHDR.Item("GeneratorTelNo").ToString
        '                        .GeneratorFaxNo = rowHDR.Item("GeneratorFaxNo").ToString
        '                        .GeneratorMobile = rowHDR.Item("GeneratorMobile").ToString
        '                        .GeneratorEmailAddress = rowHDR.Item("GeneratorEmailAddress").ToString

        '                        .TransporterLocID = rowHDR.Item("TransporterLocID").ToString
        '                        .TransporterName = rowHDR.Item("TransporterName").ToString
        '                        .TransporterAddress = rowHDR.Item("TransporterAddress").ToString
        '                        .TransporterPIC = rowHDR.Item("TransporterPIC").ToString
        '                        .TransporterPos = rowHDR.Item("TransporterPos").ToString
        '                        .TransporterPosDesc = rowHDR.Item("TransporterPosDesc").ToString
        '                        .TransporterTelNo = rowHDR.Item("TransporterTelNo").ToString
        '                        .TransporterFaxNo = rowHDR.Item("TransporterFaxNo").ToString
        '                        .TransporterMobile = rowHDR.Item("TransporterMobile").ToString
        '                        .TransporterEmailAddress = rowHDR.Item("TransporterEmailAddress").ToString

        '                        .ReceiverLocID = rowHDR.Item("ReceiverLocID").ToString
        '                        .ReceiverName = rowHDR.Item("ReceiverName").ToString
        '                        .ReceiverAddress = rowHDR.Item("ReceiverAddress").ToString
        '                        .ReceiverPIC = rowHDR.Item("ReceiverPIC").ToString
        '                        .ReceiverPos = rowHDR.Item("ReceiverPos").ToString
        '                        .ReceiverPosDesc = rowHDR.Item("ReceiverPosDesc").ToString
        '                        .ReceiverTelNo = rowHDR.Item("ReceiverTelNo").ToString
        '                        .ReceiverFaxNo = rowHDR.Item("ReceiverFaxNo").ToString
        '                        .ReceiverMobile = rowHDR.Item("ReceiverMobile").ToString
        '                        .ReceiverEmailAddress = rowHDR.Item("ReceiverEmailAddress").ToString
        '                        .ReceiveDate = IIf(IsDBNull(rowHDR.Item("ReceiveDate")) = False, rowHDR.Item("ReceiveDate"), Convert.ToDateTime("01/01/0001"))

        '                        .DriverName = rowHDR.Item("DriverName").ToString
        '                        .DriverID = rowHDR.Item("DriverID").ToString
        '                        .VehicleID = rowHDR.Item("RegNo").ToString
        '                        .ICNo = rowHDR.Item("ICNo").ToString
        '                        .DeliveryRemark = rowHDR.Item("DeliveryRemark").ToString
        '                        .ReceiveRemark = rowHDR.Item("ReceiveRemark").ToString
        '                        .TempStorage1 = rowHDR.Item("TempStorage1").ToString
        '                        .Status = rowHDR.Item("Status").ToString
        '                        .StatusDesc = rowHDR.Item("StatusDesc").ToString
        '                    End With
        '                    listConsignHDR.Add(rConsignHDR)
        '                Next
        '            End If
        '        Catch ex As Exception
        '            Log.Notifier.Notify(ex)
        '            Gibraltar.Agent.Log.Error("Actions/ConsignHDR", ex.Message & " " & strSQL, ex.StackTrace)
        '        Finally
        '            EndSQLControl()
        '        End Try
        '    End If
        '    EndConnection()
        '    Return listConsignHDR
        'End Function

        'Public Function GetConsignmentBySerialNo(ByVal serialNo As String, ByVal LocID As String, Optional ByVal StartDate As String = Nothing, Optional ByVal EndDate As String = Nothing) As List(Of Actions.Container.Consignhdr)
        '    Dim rConsignHDR As Container.Consignhdr = Nothing
        '    Dim listConsignHDR As List(Of Container.Consignhdr) = Nothing
        '    Dim dtTemp As DataTable = Nothing

        '    If StartConnection() = True Then
        '        StartSQLControl()
        '        strSQL = "select h.GeneratorID, be.BizRegID, be.CompanyName, h.ContractNo, h.TransDate, h.TransType, d.SerialNo, h.ReferID," &
        '                 " h.GeneratorLocID, g.BranchName as GeneratorName, h.GeneratorAddress, h.GeneratorPIC, h.GeneratorPos, h.GeneratorTelNo, h.GeneratorFaxNo, g.ContactMobile as GeneratorMobile, h.GeneratorEmailAddress," &
        '                 " h.TransporterLocID, t.BranchName as TransporterName, h.TransporterAddress, h.TransporterPIC, h.TransporterPos, h.TransporterTelNo, h.TransporterFaxNo, t.ContactMobile as TransporterMobile, h.TransporterEmailAddress," &
        '                 " h.ReceiverLocID, r.BranchName as ReceiverName, h.ReceiverAddress, h.ReceiverPIC, h.ReceiverPos, h.ReceiverTelNo, h.ReceiverFaxNo, r.ContactMobile as ReceiverMobile, h.ReceiverEmailAddress, h.DriverName, h.ICNo, h.DriverID, h.VehicleID," &
        '                 " h.DeliveryRemark, h.ReceiveRemark, h.TempStorage1, h.Status" &
        '                 " from CONSIGNHDR h with (nolock) left join CONSIGNDTL d with (nolock) on h.ContractNo=d.ContractNo" &
        '                 " left join BIZENTITY be with (nolock) on h.Generatorid=be.BizRegID" &
        '                 " left join BIZLOCATE g with (nolock) on h.GeneratorID=g.BizRegID and h.GeneratorLocID=g.BizLocID " &
        '                 " left join BIZLOCATE t with (nolock) on h.TransporterID=t.BizRegID and h.TransporterLocID=t.BizLocID " &
        '                 " left join BIZLOCATE r with (nolock) on h.ReceiverID=r.BizRegID and h.ReceiverLocID=r.BizLocID " &
        '                 " WHERE SerialNo='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, serialNo) & "' AND ReceiverLocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' "

        '        If StartDate IsNot Nothing AndAlso StartDate <> "" Then
        '            strSQL &= " And (TransDate >= convert(datetime, '" & StartDate & "', 103)"
        '        End If

        '        If EndDate IsNot Nothing AndAlso EndDate <> "" Then
        '            strSQL &= " and TransDate <= convert(datetime, '" & EndDate & "', 103))"
        '        End If

        '        strSQL &= " ORDER BY TransDate DESC"

        '        Try
        '            dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
        '            If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
        '                listConsignHDR = New List(Of Container.Consignhdr)
        '                For Each rowHDR As DataRow In dtTemp.Rows
        '                    rConsignHDR = New Container.Consignhdr
        '                    With rConsignHDR
        '                        .GeneratorID = rowHDR.Item("GeneratorID").ToString
        '                        .CompanyName = rowHDR.Item("CompanyName").ToString
        '                        .ContractNo = rowHDR.Item("ContractNo").ToString
        '                        .TransDate = rowHDR.Item("TransDate").ToString
        '                        .TransType = rowHDR.Item("TransType").ToString
        '                        .ReferID = rowHDR.Item("ReferID").ToString

        '                        .GeneratorLocID = rowHDR.Item("GeneratorLocID").ToString
        '                        .GeneratorName = rowHDR.Item("GeneratorName").ToString
        '                        .GeneratorAddress = rowHDR.Item("GeneratorAddress").ToString
        '                        .GeneratorPIC = rowHDR.Item("GeneratorPIC").ToString
        '                        .GeneratorPos = rowHDR.Item("GeneratorPos").ToString
        '                        .GeneratorTelNo = rowHDR.Item("GeneratorTelNo").ToString
        '                        .GeneratorFaxNo = rowHDR.Item("GeneratorFaxNo").ToString
        '                        .GeneratorMobile = rowHDR.Item("GeneratorMobile").ToString
        '                        .GeneratorEmailAddress = rowHDR.Item("GeneratorEmailAddress").ToString

        '                        .TransporterLocID = rowHDR.Item("TransporterLocID").ToString
        '                        .TransporterName = rowHDR.Item("TransporterName").ToString
        '                        .TransporterAddress = rowHDR.Item("TransporterAddress").ToString
        '                        .TransporterPIC = rowHDR.Item("TransporterPIC").ToString
        '                        .TransporterPos = rowHDR.Item("TransporterPos").ToString
        '                        .TransporterTelNo = rowHDR.Item("TransporterTelNo").ToString
        '                        .TransporterFaxNo = rowHDR.Item("TransporterFaxNo").ToString
        '                        .TransporterMobile = rowHDR.Item("TransporterMobile").ToString
        '                        .TransporterEmailAddress = rowHDR.Item("TransporterEmailAddress").ToString

        '                        .ReceiverLocID = rowHDR.Item("ReceiverLocID").ToString
        '                        .ReceiverName = rowHDR.Item("ReceiverName").ToString
        '                        .ReceiverAddress = rowHDR.Item("ReceiverAddress").ToString
        '                        .ReceiverPIC = rowHDR.Item("ReceiverPIC").ToString
        '                        .ReceiverPos = rowHDR.Item("ReceiverPos").ToString
        '                        .ReceiverTelNo = rowHDR.Item("ReceiverTelNo").ToString
        '                        .ReceiverFaxNo = rowHDR.Item("ReceiverFaxNo").ToString
        '                        .ReceiverMobile = rowHDR.Item("ReceiverMobile").ToString
        '                        .ReceiverEmailAddress = rowHDR.Item("ReceiverEmailAddress").ToString

        '                        .DriverName = rowHDR.Item("DriverName").ToString
        '                        .DriverID = rowHDR.Item("DriverID").ToString
        '                        .VehicleID = rowHDR.Item("VehicleID").ToString
        '                        .ICNo = rowHDR.Item("ICNo").ToString
        '                        .DeliveryRemark = rowHDR.Item("DeliveryRemark").ToString
        '                        .ReceiveRemark = rowHDR.Item("ReceiveRemark").ToString
        '                        .TempStorage1 = rowHDR.Item("TempStorage1").ToString
        '                        .Status = rowHDR.Item("Status").ToString
        '                    End With
        '                    listConsignHDR.Add(rConsignHDR)
        '                Next
        '            End If
        '        Catch ex As Exception
        '            Log.Notifier.Notify(ex)
        '            Gibraltar.Agent.Log.Error("Actions/ConsignHDR", ex.Message & " " & strSQL, ex.StackTrace)
        '        Finally
        '            EndSQLControl()
        '        End Try
        '    End If
        '    EndConnection()
        '    Return listConsignHDR
        'End Function

        'Public Function GetConsignmentByCNNo(ByVal CNNo As String, ByVal LocID As String, Optional ByVal StartDate As String = Nothing, Optional ByVal EndDate As String = Nothing) As List(Of Actions.Container.Consignhdr)
        '    Dim rConsignHDR As Container.Consignhdr = Nothing
        '    Dim listConsignHDR As List(Of Container.Consignhdr) = Nothing
        '    Dim dtTemp As DataTable = Nothing

        '    If StartConnection() = True Then
        '        StartSQLControl()
        '        strSQL = "select h.GeneratorID, be.BizRegID, be.CompanyName, h.ContractNo, h.TransDate, h.TransType, d.SerialNo, h.ReferID," &
        '                 " h.GeneratorLocID, g.BranchName as GeneratorName, h.GeneratorAddress, h.GeneratorPIC, h.GeneratorPos, h.GeneratorTelNo, h.GeneratorFaxNo, g.ContactMobile as GeneratorMobile, h.GeneratorEmailAddress," &
        '                 " h.TransporterLocID, t.BranchName as TransporterName, h.TransporterAddress, h.TransporterPIC, h.TransporterPos, h.TransporterTelNo, h.TransporterFaxNo, t.ContactMobile as TransporterMobile, h.TransporterEmailAddress," &
        '                 " h.ReceiverLocID, r.BranchName as ReceiverName, h.ReceiverAddress, h.ReceiverPIC, h.ReceiverPos, h.ReceiverTelNo, h.ReceiverFaxNo, r.ContactMobile as ReceiverMobile, h.ReceiverEmailAddress, h.DriverName, h.ICNo, h.DriverID, h.VehicleID," &
        '                 " h.DeliveryRemark, h.ReceiveRemark, h.TempStorage1, h.Status, h.ReceiveDate" &
        '                 " from CONSIGNHDR h with (nolock) left join CONSIGNDTL d with (nolock) on h.ContractNo=d.ContractNo" &
        '                 " left join BIZENTITY be with (nolock) on h.Generatorid=be.BizRegID" &
        '                 " left join BIZLOCATE g with (nolock) on h.GeneratorID=g.BizRegID and h.GeneratorLocID=g.BizLocID " &
        '                 " left join BIZLOCATE t with (nolock) on h.TransporterID=t.BizRegID and h.TransporterLocID=t.BizLocID " &
        '                 " left join BIZLOCATE r with (nolock) on h.ReceiverID=r.BizRegID and h.ReceiverLocID=r.BizLocID " &
        '                 " WHERE h.TransID='" & CNNo & "' AND ReceiverLocID='" & LocID & "' "

        '        If StartDate IsNot Nothing AndAlso StartDate <> "" Then
        '            strSQL &= " And (TransDate >= convert(datetime, '" & StartDate & "', 103)"
        '        End If

        '        If EndDate IsNot Nothing AndAlso EndDate <> "" Then
        '            strSQL &= " and TransDate <= convert(datetime, '" & EndDate & "', 103))"
        '        End If

        '        strSQL &= " ORDER BY TransDate DESC"

        '        Try
        '            dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
        '            If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
        '                listConsignHDR = New List(Of Container.Consignhdr)
        '                For Each rowHDR As DataRow In dtTemp.Rows
        '                    rConsignHDR = New Container.Consignhdr
        '                    With rConsignHDR
        '                        .GeneratorID = rowHDR.Item("GeneratorID").ToString
        '                        .CompanyName = rowHDR.Item("CompanyName").ToString
        '                        .ContractNo = rowHDR.Item("ContractNo").ToString
        '                        .TransDate = rowHDR.Item("TransDate").ToString
        '                        .TransType = rowHDR.Item("TransType").ToString
        '                        .ReferID = rowHDR.Item("ReferID").ToString

        '                        .GeneratorLocID = rowHDR.Item("GeneratorLocID").ToString
        '                        .GeneratorName = rowHDR.Item("GeneratorName").ToString
        '                        .GeneratorAddress = rowHDR.Item("GeneratorAddress").ToString
        '                        .GeneratorPIC = rowHDR.Item("GeneratorPIC").ToString
        '                        .GeneratorPos = rowHDR.Item("GeneratorPos").ToString
        '                        .GeneratorTelNo = rowHDR.Item("GeneratorTelNo").ToString
        '                        .GeneratorFaxNo = rowHDR.Item("GeneratorFaxNo").ToString
        '                        .GeneratorMobile = rowHDR.Item("GeneratorMobile").ToString
        '                        .GeneratorEmailAddress = rowHDR.Item("GeneratorEmailAddress").ToString

        '                        .TransporterLocID = rowHDR.Item("TransporterLocID").ToString
        '                        .TransporterName = rowHDR.Item("TransporterName").ToString
        '                        .TransporterAddress = rowHDR.Item("TransporterAddress").ToString
        '                        .TransporterPIC = rowHDR.Item("TransporterPIC").ToString
        '                        .TransporterPos = rowHDR.Item("TransporterPos").ToString
        '                        .TransporterTelNo = rowHDR.Item("TransporterTelNo").ToString
        '                        .TransporterFaxNo = rowHDR.Item("TransporterFaxNo").ToString
        '                        .TransporterMobile = rowHDR.Item("TransporterMobile").ToString
        '                        .TransporterEmailAddress = rowHDR.Item("TransporterEmailAddress").ToString

        '                        .ReceiverLocID = rowHDR.Item("ReceiverLocID").ToString
        '                        .ReceiverName = rowHDR.Item("ReceiverName").ToString
        '                        .ReceiverAddress = rowHDR.Item("ReceiverAddress").ToString
        '                        .ReceiverPIC = rowHDR.Item("ReceiverPIC").ToString
        '                        .ReceiverPos = rowHDR.Item("ReceiverPos").ToString
        '                        .ReceiverTelNo = rowHDR.Item("ReceiverTelNo").ToString
        '                        .ReceiverFaxNo = rowHDR.Item("ReceiverFaxNo").ToString
        '                        .ReceiverMobile = rowHDR.Item("ReceiverMobile").ToString
        '                        .ReceiverEmailAddress = rowHDR.Item("ReceiverEmailAddress").ToString
        '                        .ReceiveDate = rowHDR.Item("ReceiveDate").ToString

        '                        .DriverName = rowHDR.Item("DriverName").ToString
        '                        .DriverID = rowHDR.Item("DriverID").ToString
        '                        .VehicleID = rowHDR.Item("VehicleID").ToString
        '                        .ICNo = rowHDR.Item("ICNo").ToString
        '                        .DeliveryRemark = rowHDR.Item("DeliveryRemark").ToString
        '                        .ReceiveRemark = rowHDR.Item("ReceiveRemark").ToString
        '                        .TempStorage1 = rowHDR.Item("TempStorage1").ToString
        '                        .Status = rowHDR.Item("Status").ToString
        '                    End With
        '                    listConsignHDR.Add(rConsignHDR)
        '                Next
        '            End If
        '        Catch ex As Exception
        '            Log.Notifier.Notify(ex)
        '            Gibraltar.Agent.Log.Error("Actions/ConsignHDR", ex.Message & " " & strSQL, ex.StackTrace)
        '        Finally
        '            EndSQLControl()
        '        End Try
        '    End If
        '    EndConnection()
        '    Return listConsignHDR
        'End Function

        'Public Overloads Function GetConsignmentNoteCustomList(Optional ByVal print As String = Nothing) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo
        '            StartSQLControl()
        '            strSQL = "SELECT VH.RegNo AS REGNO,  case  when EM.NRICNO is null then H.ICNo when EM.NRICNO='' then H.ICNo when EM.NRICNO='-' then H.ICNo else EM.NRICNO end AS NRICNO, h.ContractNo, h.TransDate, h.Status,h.ReferID," & _
        '                "h.GeneratorID, h.GeneratorLocID, h.GeneratorAddress, h.GeneratorPIC, h.GeneratorPos, h.GeneratorTelNo, h.GeneratorFaxNo, h.GeneratorEmailAddress, cg.CompanyName as WG, lg.BranchName as BranchWG, sg.StateDesc as StateWG, ctg.CityDesc as CityWG, pbg.PBTDesc as PBTWG, cng.CountryDesc as CountryWG, " & _
        '                "h.TransporterID, h.TransporterLocID, h.TransporterAddress, h.TransporterPIC, h.TransporterPos,(select codeDesc from codemaster where codetype='DSN' and code=h.TransporterPos) as TransporterPosDesc, h.TransporterTelNo, h.TransporterFaxNo, h.TransporterEmailAddress, ct.CompanyName as WT, lt.BranchName as BranchWT, st.StateDesc as StateWT, ctt.CityDesc as CityWT, pbt.PBTDesc as PBTWT, cnt.CountryDesc as CountryWT, " & _
        '                "h.ReceiverID, h.ReceiverLocID, h.ReceiverAddress, h.ReceiverPIC, h.ReceiverPos, h.ReceiverTelNo, h.ReceiverFaxNo, h.ReceiverEmailAddress, cr.CompanyName as WR, lr.BranchName as BranchWR, sr.StateDesc as StateWR, ctr.CityDesc as CityWR, pbr.PBTDesc as PBTWR, cnr.CountryDesc as CountryWR," & _
        '                "d.SerialNo, d.OriginCode, d.OriginDescription, u.UOMDesc as WastePackaging, cmt.CodeDesc as WasteType, d.WasteComponent, d.TreatmentCost, d.PackagingQty, (d.WasteCode+' - '+i.ItmDesc) as WasteCode, d.WasteDescription as WasteName, d.Qty, h.DriverID, h.DriverName, h.ICNo, h.VehicleID, cmh.CodeDesc as OperationType, d.OperationDesc, d.HandlingQty, d.RcvQty, h.TempStorage1, h.TempStorage2, h.ReceiveDate, h.RejectDate, h.ReceiveRemark " & _
        '                " FROM CONSIGNHDR h WITH (NOLOCK) " & _
        '                " LEFT JOIN CONSIGNDTL d WITH (NOLOCK) ON d.ContractNo=h.ContractNo " & _
        '                " LEFT JOIN ITEM i WITH (NOLOCK) ON i.ItemCode=d.WASTECODE " & _
        '                " LEFT JOIN UOM u WITH (NOLOCK) ON u.UOMCode=d.WastePackage " & _
        '                " LEFT JOIN CODEMASTER cmt WITH (NOLOCK) ON cmt.CodeType='WTY' and cmt.Code=d.WasteType " & _
        '                " LEFT JOIN CODEMASTER cmh WITH (NOLOCK) ON cmh.CodeType='WTH' and cmh.Code=d.OperationType " & _
        '                " LEFT JOIN BIZLOCATE lg WITH (NOLOCK) ON lg.BizLocID=h.GeneratorLocID " & _
        '                " LEFT JOIN BIZENTITY cg WITH (NOLOCK) on cg.BizRegID=h.GeneratorID " & _
        '                " LEFT JOIN STATE sg WITH (NOLOCK) ON sg.StateCode=lg.State and sg.CountryCode=lg.Country " & _
        '                " LEFT JOIN PBT pbg WITH (NOLOCK) ON pbg.PBTCode=lg.PBT and pbg.CountryCode=lg.Country and pbg.StateCode=lg.State " & _
        '                " LEFT JOIN CITY ctg WITH (NOLOCK) ON ctg.CityCode=lg.City and ctg.CountryCode=lg.Country and ctg.StateCode=lg.State " & _
        '                " LEFT JOIN COUNTRY cng WITH (NOLOCK) ON cng.CountryCode=lg.Country " & _
        '                " LEFT JOIN BIZLOCATE lt WITH (NOLOCK) ON lt.BizLocID=h.TransporterLocID " & _
        '                " LEFT JOIN BIZENTITY ct WITH (NOLOCK) on ct.BizRegID=h.TransporterID " & _
        '                " LEFT JOIN STATE st WITH (NOLOCK) ON st.StateCode=lt.State and st.CountryCode=lt.Country " & _
        '                " LEFT JOIN PBT pbt WITH (NOLOCK) ON pbt.PBTCode=lt.PBT and pbt.CountryCode=lt.Country and pbt.StateCode=lt.State " & _
        '                " LEFT JOIN CITY ctt WITH (NOLOCK) ON ctt.CityCode=lt.City and ctt.CountryCode=lt.Country and ctt.StateCode=lt.State " & _
        '                " LEFT JOIN COUNTRY cnt WITH (NOLOCK) ON cnt.CountryCode=lt.Country " & _
        '                " LEFT JOIN BIZLOCATE lr WITH (NOLOCK) ON lr.BizLocID=h.ReceiverLocID " & _
        '                " LEFT JOIN BIZENTITY cr WITH (NOLOCK) on cr.BizRegID=h.ReceiverID " & _
        '                " LEFT JOIN STATE sr WITH (NOLOCK) ON sr.StateCode=lr.State and sr.CountryCode=lr.Country " & _
        '                " LEFT JOIN PBT pbr WITH (NOLOCK) ON pbr.PBTCode=lr.PBT and pbr.CountryCode=lr.Country and pbr.StateCode=lr.State " & _
        '                " LEFT JOIN CITY ctr WITH (NOLOCK) ON ctr.CityCode=lr.City and ctr.CountryCode=lr.Country and ctr.StateCode=lr.State " & _
        '                " LEFT JOIN COUNTRY cnr WITH (NOLOCK) ON cnr.CountryCode=lr.Country " & _
        '                " LEFT JOIN EMPLOYEE EM WITH (NOLOCK) ON H.DriverID =EM.EmployeeID " & _
        '                " LEFT JOIN VEHICLE VH WITH (NOLOCK) ON H.VehicleID =VH.VehicleID "
        '            'If Not Condition Is Nothing And Condition <> "" Then
        '            '    strSQL &= " WHERE " & Condition

        '            'End If
        '            If print IsNot Nothing AndAlso print <> "" Then
        '                strSQL &= " WHERE h.Flag=1 and h.ContractNo='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, print) & "'"
        '            End If

        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndSQLControl()
        '    EndConnection()
        'End Function

        'Public Overloads Function GetListCompanyListConsignmentNote(ByVal CompanyType As Integer, Optional ByVal Condition As String = Nothing, Optional ByVal Condition1 As String = Nothing, Optional ByVal OrderField As String = Nothing) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo
        '            Dim strStatus As String = "CASE CONSIGNHDR.Status WHEN 0 Then 'Draft' WHEN 1 THEN " & _
        '                                      "CASE CONSIGNHDR.isConfirm WHEN 0 Then 'Submitted' WHEN 1 Then 'Submitted' END " & _
        '                                      "WHEN 2 THEN 'Cancelled' " & _
        '                "WHEN 3 THEN 'Acknowledged' WHEN 4 THEN 'Acknowledged' WHEN 5 THEN 'Cancelled Transfer' " & _
        '                "WHEN 6 THEN 'On Hold' WHEN 7 THEN 'On Hold' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '                "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' WHEN 12 THEN 'Cancelled' END as Status"

        '            If CompanyType = 2 Then
        '                strStatus = "CASE CONSIGNHDR.Status WHEN 0 Then 'Draft' WHEN 1 THEN " & _
        '                                      "CASE CONSIGNHDR.isConfirm WHEN 0 Then 'Submitted' WHEN 1 Then 'Submitted' END " & _
        '                                      "WHEN 2 THEN 'Cancelled' " & _
        '                "WHEN 3 THEN 'Acknowledged' WHEN 4 THEN 'Acknowledged' WHEN 5 THEN 'Cancelled Transfer' " & _
        '                "WHEN 6 THEN 'On Hold' WHEN 7 THEN 'On Hold' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '                "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' WHEN 12 THEN 'Cancelled' " & _
        '                "WHEN 13 THEN 'Draft' WHEN 14 THEN 'Submitted / Received' WHEN 15 THEN 'Cancelled' END as Status"
        '            ElseIf CompanyType = 3 Then
        '                strStatus = "CASE CONSIGNHDR.Status WHEN 0 Then 'Draft' WHEN 1 THEN " & _
        '                                      "CASE CONSIGNHDR.isConfirm WHEN 0 Then 'Submitted' WHEN 1 Then 'Submitted' END " & _
        '                                      "WHEN 2 THEN 'Cancelled' " & _
        '                "WHEN 3 THEN 'Acknowledged' WHEN 4 THEN 'Acknowledged' WHEN 5 THEN 'Cancelled Transfer' " & _
        '                "WHEN 6 THEN 'On Hold' WHEN 7 THEN 'On Hold' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '                "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' WHEN 12 THEN 'Cancelled' END as Status"
        '            ElseIf CompanyType = 4 Then
        '                strStatus = "CASE CONSIGNHDR.Status WHEN 0 Then 'Draft' WHEN 1 THEN " & _
        '                                      "CASE CONSIGNHDR.isConfirm WHEN 0 Then 'Submitted' WHEN 1 Then 'Submitted' END " & _
        '                                      "WHEN 2 THEN 'Cancelled' " & _
        '                    "WHEN 3 THEN 'Acknowledged' WHEN 4 THEN 'Acknowledged' WHEN 5 THEN 'Cancelled Transfer' " & _
        '                    "WHEN 6 THEN 'On Hold' WHEN 7 THEN 'On Hold' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '                    "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' WHEN 12 THEN 'Cancelled' END as Status "
        '            End If

        '            strSQL = "SELECT CONSIGNHDR.ContractNo, CONSIGNHDR.ReferID, CONSIGNDTL.WasteCode AS WasteCode, CONSIGNDTL.WasteType AS WasteType, CONSIGNDTL.Qty AS Qty, " &
        '                " CONSIGNDTL.RcvQty AS RcvQty, (CONSIGNDTL.Qty-CONSIGNDTL.RcvQty) AS VarianceQty, " &
        '                " CG.CompanyName as GeneratorName, " &
        '                " CT.CompanyName as TransporterName, " &
        '                " CR.CompanyName as ReceiverName," &
        '                " CONSIGNHDR.TransDate, CONSIGNHDR.TransportDate, CONSIGNHDR.TargetTransportDate, CONSIGNHDR.TargetReceiveDate, CONSIGNHDR.ReceiveDate, CONSIGNHDR.Status AS SubmitStatus, " & strStatus &
        '                " , CASE WHEN ReceiveStatus=1 THEN 'Pending' WHEN ReceiveStatus=2 THEN 'Approve' WHEN ReceiveStatus=3 THEN 'Decline' END As ApprovalStatus " &
        '                " FROM CONSIGNHDR WITH (NOLOCK) " &
        '                " LEFT JOIN CONSIGNDTL WITH (NOLOCK) ON CONSIGNDTL.TransID=CONSIGNHDR.TransID " &
        '                " LEFT JOIN BIZENTITY CG WITH (NOLOCK) ON CG.BizRegID=CONSIGNHDR.GeneratorID " &
        '                " LEFT JOIN BIZENTITY CT WITH (NOLOCK) ON CT.BizRegID=CONSIGNHDR.TransporterID " &
        '                " LEFT JOIN BIZENTITY CR WITH (NOLOCK) ON CR.BizRegID=CONSIGNHDR.ReceiverID "

        '            If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition & Condition1

        '            strSQL &= " ORDER BY "

        '            If Not OrderField Is Nothing AndAlso OrderField <> "" Then
        '                strSQL &= OrderField
        '            Else
        '                strSQL &= "CONSIGNHDR.Status DESC, CONSIGNHDR.TargetReceiveDate DESC, CONSIGNHDR.TransDate DESC" 'added by diana, sorting
        '            End If


        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        'End Function

        'Public Overloads Function GetListCustomConsignmentNote(ByVal CompanyType As Integer, Optional ByVal Condition As String = Nothing, Optional ByVal OrderField As String = Nothing) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo
        '            'Dim strStatus As String = "CASE Status WHEN 0 Then 'Draft' WHEN 1 THEN 'Pending Transfer' WHEN 2 THEN 'Cancelled' " & _
        '            '    "WHEN 3 THEN 'Pending Transfer' WHEN 4 THEN 'Pending Received' WHEN 5 THEN 'Cancelled Transfer' " & _
        '            '    "WHEN 6 THEN 'Pending Receiving' WHEN 7 THEN 'Pending Receiving' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '            '    "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' END as Status"

        '            'If CompanyType = 2 Then
        '            '    strStatus = "CASE Status WHEN 0 Then 'Draft' WHEN 1 THEN 'Pending Transfer' WHEN 2 THEN 'Cancelled' " & _
        '            '    "WHEN 3 THEN 'Pending Transfer' WHEN 4 THEN 'Pending Received' WHEN 5 THEN 'Cancelled Transfer' " & _
        '            '    "WHEN 6 THEN 'Pending Receiving' WHEN 7 THEN 'Pending Receiving' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '            '    "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' END as Status"
        '            'ElseIf CompanyType = 3 Then
        '            '    strStatus = "CASE Status WHEN 0 Then 'Draft' WHEN 1 THEN 'Pending Transfer' WHEN 2 THEN 'Cancelled' " & _
        '            '    "WHEN 3 THEN 'Draft Transfer' WHEN 4 THEN 'Pending Received' WHEN 5 THEN 'Cancelled Transfer' " & _
        '            '    "WHEN 6 THEN 'Pending Receiving' WHEN 7 THEN 'Pending Receiving' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '            '    "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' END as Status"
        '            'ElseIf CompanyType = 4 Then
        '            '    strStatus = "CASE Status WHEN 0 Then 'Draft' WHEN 1 THEN 'Pending Transfer' WHEN 2 THEN 'Cancelled' " & _
        '            '        "WHEN 3 THEN 'Pending Transfer' WHEN 4 THEN 'Pending Received' WHEN 5 THEN 'Cancelled Transfer' " & _
        '            '        "WHEN 6 THEN 'Draft Receiving' WHEN 7 THEN 'Draft Receiving' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '            '        "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' END as Status"
        '            'End If

        '            'Dim strStatus As String = "CASE Status WHEN 0 Then 'Draft' WHEN 1 THEN 'Submitted' WHEN 2 THEN 'Cancelled' " & _
        '            '    "WHEN 3 THEN 'Pending Transfer' WHEN 4 THEN 'Pending Received' WHEN 5 THEN 'Cancelled Transfer' " & _
        '            '    "WHEN 6 THEN 'Pending Receiving' WHEN 7 THEN 'Pending Receiving' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '            '    "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' END as Status"

        '            'If CompanyType = 2 Then
        '            '    strStatus = "CASE Status WHEN 0 Then 'Draft' WHEN 1 THEN 'Submitted' WHEN 2 THEN 'Cancelled' " & _
        '            '    "WHEN 3 THEN 'Pending Transfer' WHEN 4 THEN 'Pending Received' WHEN 5 THEN 'Cancelled Transfer' " & _
        '            '    "WHEN 6 THEN 'Pending Receiving' WHEN 7 THEN 'Pending Receiving' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '            '    "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' END as Status"
        '            'ElseIf CompanyType = 3 Then
        '            '    strStatus = "CASE Status WHEN 0 Then 'Draft' WHEN 1 THEN 'Submitted' WHEN 2 THEN 'Cancelled' " & _
        '            '    "WHEN 3 THEN 'Draft Transfer' WHEN 4 THEN 'Pending Received' WHEN 5 THEN 'Cancelled Transfer' " & _
        '            '    "WHEN 6 THEN 'Pending Receiving' WHEN 7 THEN 'Pending Receiving' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '            '    "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' END as Status"
        '            'ElseIf CompanyType = 4 Then
        '            '    strStatus = "CASE Status WHEN 0 Then 'Draft' WHEN 1 THEN 'Submitted' WHEN 2 THEN 'Cancelled' " & _
        '            '        "WHEN 3 THEN 'Pending Transfer' WHEN 4 THEN 'Pending Received' WHEN 5 THEN 'Cancelled Transfer' " & _
        '            '        "WHEN 6 THEN 'Draft Receiving' WHEN 7 THEN 'Draft Receiving' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '            '        "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' END as Status"
        '            'End If

        '            Dim strStatus As String = "CASE CONSIGNHDR.Status WHEN 0 Then 'Draft' WHEN 1 THEN " & _
        '                                      "CASE CONSIGNHDR.isConfirm WHEN 0 Then 'Submitted' WHEN 1 Then 'Submitted' END " & _
        '                                      "WHEN 2 THEN 'Cancelled' " & _
        '                "WHEN 3 THEN 'Acknowledged' WHEN 4 THEN 'Acknowledged' WHEN 5 THEN 'Cancelled Transfer' " & _
        '                "WHEN 6 THEN 'On Hold' WHEN 7 THEN 'On Hold' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '                "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' WHEN 12 THEN 'Cancelled' END as Status"

        '            If CompanyType = 2 Then
        '                strStatus = "CASE CONSIGNHDR.Status WHEN 0 Then 'Draft' WHEN 1 THEN " & _
        '                                      "CASE CONSIGNHDR.isConfirm WHEN 0 Then 'Submitted' WHEN 1 Then 'Submitted' END " & _
        '                                      "WHEN 2 THEN 'Cancelled' " & _
        '                "WHEN 3 THEN 'Acknowledged' WHEN 4 THEN 'Acknowledged' WHEN 5 THEN 'Cancelled Transfer' " & _
        '                "WHEN 6 THEN 'On Hold' WHEN 7 THEN 'On Hold' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '                "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' WHEN 12 THEN 'Cancelled' " & _
        '                "WHEN 13 THEN 'Draft' WHEN 14 THEN 'Submitted / Received' WHEN 15 THEN 'Cancelled' END as Status"
        '            ElseIf CompanyType = 3 Then
        '                strStatus = "CASE CONSIGNHDR.Status WHEN 0 Then 'Draft' WHEN 1 THEN " & _
        '                                      "CASE CONSIGNHDR.isConfirm WHEN 0 Then 'Submitted' WHEN 1 Then 'Submitted' END " & _
        '                                      "WHEN 2 THEN 'Cancelled' " & _
        '                "WHEN 3 THEN 'Acknowledged' WHEN 4 THEN 'Acknowledged' WHEN 5 THEN 'Cancelled Transfer' " & _
        '                "WHEN 6 THEN 'On Hold' WHEN 7 THEN 'On Hold' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '                "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' WHEN 12 THEN 'Cancelled' END as Status"
        '            ElseIf CompanyType = 4 Then
        '                strStatus = "CASE CONSIGNHDR.Status WHEN 0 Then 'Draft' WHEN 1 THEN " & _
        '                                      "CASE CONSIGNHDR.isConfirm WHEN 0 Then 'Submitted' WHEN 1 Then 'Submitted' END " & _
        '                                      "WHEN 2 THEN 'Cancelled' " & _
        '                    "WHEN 3 THEN 'Acknowledged' WHEN 4 THEN 'Acknowledged' WHEN 5 THEN 'Cancelled Transfer' " & _
        '                    "WHEN 6 THEN 'On Hold' WHEN 7 THEN 'On Hold' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '                    "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' WHEN 12 THEN 'Cancelled' END as Status "
        '            End If

        '            strSQL = "SELECT CONSIGNHDR.ContractNo, CONSIGNHDR.ReferID, CONSIGNDTL.WasteCode AS WasteCode, CONSIGNDTL.WasteType AS WasteType, CONSIGNDTL.Qty AS Qty, " &
        '                " CONSIGNDTL.RcvQty AS RcvQty, (CONSIGNDTL.Qty-CONSIGNDTL.RcvQty) AS VarianceQty, " &
        '                " CG.CompanyName as GeneratorName, " &
        '                " CT.CompanyName as TransporterName, " &
        '                " CR.CompanyName as ReceiverName," &
        '                " CONSIGNHDR.TransDate, CONSIGNHDR.TransportDate, CONSIGNHDR.TargetTransportDate, CONSIGNHDR.TargetReceiveDate, CONSIGNHDR.ReceiveDate, CONSIGNHDR.Status AS SubmitStatus, " & strStatus &
        '                " , CASE WHEN ReceiveStatus=1 THEN 'Pending' WHEN ReceiveStatus=2 THEN 'Approve' WHEN ReceiveStatus=3 THEN 'Decline' END As ApprovalStatus " &
        '                " FROM CONSIGNHDR WITH (NOLOCK) " &
        '                " LEFT JOIN CONSIGNDTL WITH (NOLOCK) ON CONSIGNDTL.TransID=CONSIGNHDR.TransID " &
        '                " LEFT JOIN BIZENTITY CG WITH (NOLOCK) ON CG.BizRegID=CONSIGNHDR.GeneratorID " &
        '                " LEFT JOIN BIZENTITY CT WITH (NOLOCK) ON CT.BizRegID=CONSIGNHDR.TransporterID " &
        '                " LEFT JOIN BIZENTITY CR WITH (NOLOCK) ON CR.BizRegID=CONSIGNHDR.ReceiverID "
        '            '" INNER JOIN ITEMHNDLOC IL WITH (NOLOCK) ON IL.LocID = CONSIGNHDR.ReceiverLocID AND IL.ItemCode = CONSIGNDTL.WasteCode and IL.TypeCode = CONSIGNDTL.WasteType "

        '            If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition

        '            strSQL &= " ORDER BY "

        '            If Not OrderField Is Nothing AndAlso OrderField <> "" Then
        '                strSQL &= OrderField
        '            Else
        '                strSQL &= "CONSIGNHDR.Status DESC, CONSIGNHDR.TargetReceiveDate DESC, CONSIGNHDR.TransDate DESC" 'added by diana, sorting
        '            End If


        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        'End Function

        'Public Overloads Function GetListCustomConsignmentNoteByHighMT(ByVal CompanyType As Integer, Optional ByVal Condition As String = Nothing, Optional ByVal OrderField As String = Nothing) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo
        '            strSQL = "SELECT TOP 50 SUM(CONSIGNDTL.RcvQty) OVER(PARTITION BY CR.CompanyName) as Total, CONSIGNHDR.ContractNo, " & _
        '                " CONSIGNHDR.ReferID, CONSIGNDTL.WasteCode AS WasteCode, CONSIGNDTL.Qty AS Qty, CONSIGNDTL.RcvQty AS RcvQty, " & _
        '                " (CONSIGNDTL.Qty-CONSIGNDTL.RcvQty) AS VarianceQty,  CG.CompanyName as GeneratorName,  CT.CompanyName as " & _
        '                " TransporterName,  CR.CompanyName as ReceiverName, CONSIGNHDR.TransDate, CONSIGNHDR.TransportDate, " & _
        '                " CONSIGNHDR.TargetTransportDate, CONSIGNHDR.TargetReceiveDate,CONSIGNHDR.ReceiveDate, CONSIGNHDR.Status AS " & _
        '                " SubmitStatus, CASE CONSIGNHDR.Status WHEN 0 Then 'Draft' WHEN 1 THEN CASE CONSIGNHDR.isConfirm WHEN 0 Then " & _
        '                " 'Submitted' WHEN 1 Then 'Submitted' END WHEN 2 THEN 'Cancelled' WHEN 3 THEN 'Acknowledged' WHEN 4 THEN " & _
        '                " 'Acknowledged' WHEN 5 THEN 'Cancelled Transfer' WHEN 6 THEN 'On Hold' WHEN 7 THEN 'On Hold' WHEN 8 THEN " & _
        '                " 'Received' WHEN 9 THEN 'Rejected' WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' WHEN 12 THEN 'Cancelled' " & _
        '                " END as Status  FROM CONSIGNHDR WITH (NOLOCK)  LEFT JOIN CONSIGNDTL WITH (NOLOCK) ON CONSIGNDTL.TransID=" & _
        '                " CONSIGNHDR.TransID  LEFT JOIN BIZENTITY CG WITH (NOLOCK) ON CG.BizRegID=CONSIGNHDR.GeneratorID  LEFT JOIN " & _
        '                " BIZENTITY CT WITH (NOLOCK) ON CT.BizRegID=CONSIGNHDR.TransporterID  LEFT JOIN BIZENTITY CR WITH (NOLOCK) " & _
        '                " ON CR.BizRegID=CONSIGNHDR.ReceiverID where CONSIGNHDR.FLAG = 1 AND CONSIGNHDR.Status =8 AND " & _
        '                " MONTH(CONSIGNHDR.TransDate)=MONTH(GETDATE()) AND YEAR(CONSIGNHDR.TransDate)=YEAR(GETDATE()) ORDER BY Total Desc"

        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        'End Function

        'Load gvList ConsignWG
        Public Function GetListConsign(Optional ByVal Condition As String = Nothing, Optional ByVal SkipCount As Int32 = -1, Optional ByVal Limit As Int32 = -1, Optional ByRef totalRows As Int32 = 0) As List(Of Container.Consignhdr)
            Dim row As Container.Consignhdr = Nothing
            Dim list As List(Of Container.Consignhdr) = New List(Of Container.Consignhdr)
            Dim dtTemp As DataTable = New DataTable()
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Dim res As New Dictionary(Of Int32, List(Of Container.Consignhdr))
            Try
                'With ConsignhdrInfo.MyInfo

                Dim column = ""
                Dim strStatus = "CASE CONSIGNHDR.Status WHEN 0 Then 'Draft' WHEN 1 THEN 'Pending Transfer' WHEN 2 THEN 'Cancelled' " &
                    "WHEN 3 THEN 'Draft Transfer' WHEN 4 THEN 'Pending Received' WHEN 5 THEN 'Cancelled Transfer' " &
                    "WHEN 6 THEN 'Draft Receiving' WHEN 7 THEN 'Draft Receiving' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " &
                    "WHEN 10 THEN 'Cancelled' ELSE '' END as Status"

                column = "SELECT ISNULL(CONSIGNHDR.ContractNo, '') AS ContractNo, ISNULL(CG.CompanyName, '') AS GeneratorName, " &
                " ISNULL(CT.CompanyName, '') AS TransporterName, " &
                " ISNULL(CR.CompanyName, '') AS ReceiverName," &
                " ISNULL(CONSIGNHDR.TransDate, '') AS TransDate, ISNULL(CONSIGNHDR.TransportDate, '') AS TransportDate, ISNULL(CONSIGNHDR.TargetTransportDate, '') AS TargetTransportDate, ISNULL(CONSIGNHDR.TargetReceiveDate, '') AS TargetReceiveDate, ISNULL(CONSIGNHDR.ReceiveDate, '') AS ReceiveDate, " & strStatus

                strSQL = " FROM CONSIGNHDR WITH (NOLOCK) " &
                " LEFT JOIN BIZENTITY CG WITH (NOLOCK) ON CG.BizRegID=CONSIGNHDR.GeneratorID " &
                " LEFT JOIN BIZENTITY CT WITH (NOLOCK) ON CT.BizRegID=CONSIGNHDR.TransporterID " &
                " LEFT JOIN BIZENTITY CR WITH (NOLOCK) ON CR.BizRegID=CONSIGNHDR.ReceiverID "

                If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition

                Dim orderBy = " ORDER BY CONSIGNHDR.Status, CONSIGNHDR.TransDate DESC"

                Dim sqlLimit = ""
                If SkipCount >= 0 And Limit > 0 Then
                    sqlLimit = String.Format("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", SkipCount, Limit)
                Else
                    sqlLimit = String.Format("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", 0, 1000)
                End If

                Dim adapter As SqlDataAdapter
                If SkipCount >= 0 And Limit > 0 Then
                    Dim sqlCount = String.Format("SELECT COUNT(*) AS TotalRows {0}", strSQL)

                    StartConnection()
                    adapter = New SqlDataAdapter(sqlCount, Conn)

                    'dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
                    adapter.Fill(dtTemp)

                    If dtTemp Is Nothing = False Then
                        For Each drRow As DataRow In dtTemp.Rows
                            totalRows = drRow.Item("TotalRows")
                        Next
                    End If
                    EndConnection()
                Else

                End If

                Dim dtActual As DataTable = New DataTable()
                StartConnection()
                Dim sqlSelect = String.Format("{0} {1} {2} {3}", column, strSQL, orderBy, sqlLimit)
                adapter = New SqlDataAdapter(sqlSelect, Conn)

                'dtActual = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
                adapter.Fill(dtActual)
                Dim rowCount As Integer = 0
                If dtActual Is Nothing = False Then
                    For Each drRow As DataRow In dtActual.Rows
                        row = New Container.Consignhdr
                        row.ContractNo = drRow.Item("ContractNo")
                        row.GeneratorName = drRow.Item("GeneratorName")
                        row.TransporterName = drRow.Item("TransporterName")
                        row.ReceiverName = drRow.Item("ReceiverName")
                        row.TransDate = drRow.Item("TransDate")
                        row.TransportDate = drRow.Item("TransportDate")
                        row.TargetTransportDate = drRow.Item("TargetTransportDate")
                        row.TargetReceiveDate = drRow.Item("TargetReceiveDate")
                        row.ReceiveDate = drRow.Item("ReceiveDate")
                        row.StatusDesc = drRow.Item("Status")
                        list.Add(row)
                    Next
                Else
                    row = Nothing
                End If

                If totalRows <= 0 Then
                    totalRows = list.Count
                End If
                'End With

            Catch ex As Exception
                Dim temp = ex.ToString()
                'Log.Notifier.Notify(ex)
                'Gibraltar.Agent.Log.Error("eSWISLogic/Security/PermissionSet", ex.Message, ex.StackTrace)
            Finally
                row = Nothing
                EndSQLControl()
                EndConnection()
            End Try
            'res.Add(totalRows, list)
            Return list
        End Function

        ''Load gvList ConsignWG
        'Public Overloads Function GetListConsignWG(Optional ByVal Condition As String = Nothing) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo

        '            strSQL = "SELECT CONSIGNHDR.ContractNo, " & _
        '                " CT.CompanyName as TransporterName, " & _
        '                " CR.CompanyName as ReceiverName, " & _
        '                " CONSIGNHDR.TransDate, CONSIGNHDR.TargetTransportDate, CONSIGNHDR.TargetReceiveDate, CONSIGNHDR.ReceiveDate, CASE CONSIGNHDR.Status WHEN '0' Then 'Draft' ELSE 'Submitted' END as Status " & _
        '                " FROM CONSIGNHDR WITH (NOLOCK) " & _
        '                " LEFT JOIN BIZENTITY CT WITH (NOLOCK) ON CT.BizRegID=CONSIGNHDR.TransporterID " & _
        '                " LEFT JOIN BIZENTITY CR WITH (NOLOCK) ON CR.BizRegID=CONSIGNHDR.ReceiverID "

        '            If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition

        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        'End Function

        ''Load gvList ConsignWT
        'Public Overloads Function GetListConsignWT(Optional ByVal Condition As String = Nothing) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo

        '            strSQL = "SELECT CONSIGNHDR.ContractNo, " & _
        '                " CG.CompanyName as GeneratorName, " & _
        '                " CR.CompanyName as ReceiverName," & _
        '                " CONSIGNHDR.TransportDate, CONSIGNHDR.TargetTransportDate, CONSIGNHDR.TargetReceiveDate, CASE CONSIGNHDR.TransportStatus WHEN '1' Then 'Pending' WHEN '2' Then 'Draft' ELSE 'Submitted' END as Status " & _
        '                " FROM CONSIGNHDR WITH (NOLOCK) " & _
        '                " LEFT JOIN BIZENTITY CG WITH (NOLOCK) ON CG.BizRegID=CONSIGNHDR.GeneratorID " & _
        '                " LEFT JOIN BIZENTITY CR WITH (NOLOCK) ON CR.BizRegID=CONSIGNHDR.ReceiverID "

        '            If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition
        '            strSQL &= " ORDER BY CONSIGNHDR.TargetReceiveDate DESC"

        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        'End Function


        ''Load gvList ConsignWR
        'Public Overloads Function GetListConsignWR(Optional ByVal Condition As String = Nothing) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo

        '            strSQL = "SELECT CONSIGNHDR.ContractNo, " & _
        '                " CG.CompanyName as GeneratorName, " & _
        '                " CT.CompanyName as TransporterName," & _
        '                " CONSIGNHDR.ReceiveDate, CONSIGNHDR.TargetTransportDate, CONSIGNHDR.TargetReceiveDate, CASE CONSIGNHDR.Status WHEN '8' Then 'Completed' WHEN '9' Then 'Rejected' else 'Pending' END as Status " & _
        '                " FROM CONSIGNHDR WITH (NOLOCK) " & _
        '                " LEFT JOIN BIZENTITY CG WITH (NOLOCK) ON CG.BizRegID=CONSIGNHDR.GeneratorID " & _
        '                " LEFT JOIN BIZENTITY CT WITH (NOLOCK) ON CT.BizRegID=CONSIGNHDR.TransporterID "

        '            If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition
        '            strSQL &= " ORDER BY CONSIGNHDR.TargetReceiveDate DESC "

        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        'End Function

        ''Add for load Transporter in ConsignmentNote
        'Public Overloads Function GetTransporterCustomList(Optional ByVal Condition As String = Nothing) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo

        '            'strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY B.CompanyName asc) as RecNo, H.* , L.AccNo, " & _
        '            '    " B.CompanyName, " & _
        '            '    " ISNULL(SIC.SICDesc,'') as Industry, " & _
        '            '    " H.TransporterAddress,H.TransporterPIC,TransporterPos ,H.TransporterTelNo,B.ContactPersonMobile as TransporterMobile,H.TransporterFaxNo,H.TransporterEmailAddress," & _
        '            '    " B.ContactPersonMobile,B.ContactPersonEmail,B.BizRegID as CompanyID, " & _
        '            '    " ISNULL(S.StateDesc,'') as StateDesc, " & _
        '            '    " ISNULL(C.CountryDesc,'') as CountryDesc, " & _
        '            '    " ISNULL(PBT.PBTDesc,'') as PBTDesc, " & _
        '            '    " ISNULL(CITY.CityDesc,'') as CityDesc, " & _
        '            '    " VehicleID, DriverID, DriverName, ICNo, " & _
        '            '    " ISNULL(CM.CodeDesc,'') as TransporterPosDesc " & _
        '            '    " FROM CONSIGNHDR H WITH (NOLOCK) " & _
        '            '    " LEFT JOIN BIZENTITY B WITH (NOLOCK) on H.TransporterID=B.BizRegID " & _
        '            '    " LEFT JOIN BIZLOCATE L WITH (NOLOCK) on B.BizRegID = L.BizRegID " & _
        '            '    " LEFT JOIN SIC WITH (NOLOCK) ON SIC.SICCode=B.IndustryType " & _
        '            '    " LEFT JOIN STATE S WITH (NOLOCK) ON S.StateCode=B.State and S.CountryCode=B.Country " & _
        '            '    " LEFT JOIN COUNTRY C WITH (NOLOCK) ON C.CountryCode=B.Country " & _
        '            '    " LEFT JOIN PBT WITH (NOLOCK) ON PBT.PBTCode=B.PBT and PBT.CountryCode=B.Country and PBT.StateCode=B.State " & _
        '            '    " LEFT JOIN CITY WITH (NOLOCK) ON CITY.CityCode=B.City and CITY.CountryCode=B.Country and CITY.StateCode=B.State " & _
        '            '    " LEFT JOIN CODEMASTER CM WITH (NOLOCK) ON CM.CodeType='DSN' AND CM.Code=H.TransporterPos "

        '            ' modified by Lily (18062015): add ISNULL function to the query
        '            strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY B.CompanyName asc) as RecNo, " & _
        '                " H.TransID, H.ContractNo, H.ReferID, H.TransType, H.TransDate, H.Status, " & _
        '                " H.TransportStatus, H.ReceiveStatus, H.GeneratorID, H.GeneratorLocID, H.GeneratorAddress, " & _
        '                " H.GeneratorPIC, H.GeneratorPos, H.GeneratorTelNo, H.GeneratorFaxNo, H.GeneratorEmailAddress, " & _
        '                " H.TransporterID, H.TransporterLocID, " & _
        '                " H.VehicleID, H.DriverID, H.DriverName, " & _
        '                " case  when EM.NRICNO is null then H.DriverID when EM.NRICNO='' then H.DriverID when EM.NRICNO='-' then H.DriverID else EM.NRICNO end AS NRICNO,  " & _
        '                " case when VH.RegNo is null then H.VehicleID when VH.regno='' then H.VehicleID else VH.RegNo end AS REGNO, " & _
        '                " H.ICNo, H.ReceiverID, H.ReceiverLocID, H.ReceiverAddress, H.ReceiverPIC, H.ReceiverPos," & _
        '                " H.ReceiverTelNo, H.ReceiverFaxNo, H.ReceiverEmailAddress, ISNULL(H.PremiseID,'') as PremiseID, H.TempStorage1," & _
        '                " H.TempStorage2, H.DeliveryDate, H.TargetTransportDate, H.TransportDate, H.TargetReceiveDate," & _
        '                " ISNULL(H.ReceiveDate,'') as ReceiveDate, ISNULL(H.RejectDate,'') as RejectDate, H.DeliveryRemark, H.ReceiveRemark, H.RejectRemark, H.CreateDate, H.CreateBy," & _
        '                " H.LastUpdate, H.UpdateBy, H.Active, H.Inuse, H.Flag, H.rowguid," & _
        '                " H.SyncCreate, H.SyncLastUpd, H.IsHost, H.IsConfirm, H.IsNew, H.LastSyncBy," & _
        '                " ISNULL(L.AccNo,'') as AccNo, " & _
        '                " ISNULL(L.BranchName,'') as CompanyName, " & _
        '                " ISNULL(SIC.SICDesc,'') as Industry, " & _
        '                " H.TransporterAddress,H.TransporterPIC,TransporterPos ,H.TransporterTelNo, " & _
        '                " ISNULL(B.ContactPersonMobile,'') as TransporterMobile,H.TransporterFaxNo, H.TransporterEmailAddress," & _
        '                " ISNULL(B.ContactPersonMobile,'') as ContactPersonMobile, ISNULL(B.ContactPersonEmail,'') as ContactPersonEmail, ISNULL(B.BizRegID,'') as CompanyID, " & _
        '                " ISNULL(S.StateDesc,'') as StateDesc, " & _
        '                " ISNULL(C.CountryDesc,'') as CountryDesc, " & _
        '                " ISNULL(PBT.PBTDesc,'') as PBTDesc, " & _
        '                " ISNULL(CITY.CityDesc,'') as CityDesc, " & _
        '                " H.VehicleID, DriverID, DriverName, ICNo, " & _
        '                " ISNULL(CM.CodeDesc,'') as TransporterPosDesc " & _
        '                " FROM CONSIGNHDR H WITH (NOLOCK) " & _
        '                " LEFT JOIN BIZENTITY B WITH (NOLOCK) on H.TransporterID=B.BizRegID " & _
        '                " LEFT JOIN BIZLOCATE L WITH (NOLOCK) on B.BizRegID = L.BizRegID " & _
        '                " LEFT JOIN SIC WITH (NOLOCK) ON SIC.SICCode=B.IndustryType " & _
        '                " LEFT JOIN STATE S WITH (NOLOCK) ON S.StateCode=B.State and S.CountryCode=B.Country " & _
        '                " LEFT JOIN COUNTRY C WITH (NOLOCK) ON C.CountryCode=B.Country " & _
        '                " LEFT JOIN PBT WITH (NOLOCK) ON PBT.PBTCode=B.PBT and PBT.CountryCode=B.Country and PBT.StateCode=B.State " & _
        '                " LEFT JOIN CITY WITH (NOLOCK) ON CITY.CityCode=B.City and CITY.CountryCode=B.Country and CITY.StateCode=B.State " & _
        '                 " LEFT JOIN CODEMASTER CM WITH (NOLOCK) ON CM.CodeType='DSN' AND CM.Code=H.TransporterPos " & _
        '                " LEFT JOIN EMPLOYEE EM WITH (NOLOCK) ON H.DriverID =EM.EmployeeID " & _
        '                " LEFT JOIN VEHICLE VH WITH (NOLOCK) ON H.VehicleID =VH.VehicleID  "

        '            If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition

        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        'End Function

        ''function for ISWIS_API
        'Public Overloads Function GetTransporterCustomList(ByVal ValidityEndDate As System.DateTime) As List(Of Profiles.Container.Bizlocate)
        '    Dim rTransporter As Profiles.Container.Bizlocate = Nothing
        '    Dim listTransporter As List(Of Profiles.Container.Bizlocate) = Nothing
        '    Dim dtTemp As DataTable = Nothing

        '    If StartConnection() = True Then
        '        StartSQLControl()
        '        strSQL = "SELECT * FROM (SELECT DISTINCT L.AccNo, B.CompanyName,  ISNULL(SIC.SICDesc,'') as Industry, L.Address1, L.Address2, L.Address3, L.Address4," & _
        '                                " L.ContactPerson, L.ContactDesignation, L.ContactTelNo,  L.Fax," & _
        '                                " L.ContactMobile, L.ContactEmail, B.BizRegID, L.BizLocID, L.BranchName" & _
        '                                " FROM BIZENTITY B WITH (NOLOCK)  INNER JOIN BIZLOCATE L WITH (NOLOCK) on B.BizRegID = L.BizRegID" & _
        '                                " LEFT JOIN LICENSE LI WITH (NOLOCK) on L.BizLocID = LI.LocID and L.BizRegID=LI.CompanyID" & _
        '                                " LEFT JOIN SIC WITH (NOLOCK) ON SIC.SICCode=B.IndustryType" & _
        '                 " WHERE LI.conttype='T' AND L.Flag=1 AND  LI.ValidityEnd >= " & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtDateTime, ValidityEndDate) & _
        '                 " AND CompanyType IN ('3','5','7','9')) as x" & _
        '                 " ORDER BY CompanyName ASC"
        '        Try
        '            dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
        '            If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
        '                listTransporter = New List(Of Profiles.Container.Bizlocate)
        '                For Each row As DataRow In dtTemp.Rows
        '                    rTransporter = New Profiles.Container.Bizlocate
        '                    With rTransporter
        '                        .BizRegID = row.Item("BizRegID").ToString
        '                        .CompanyName = row.Item("CompanyName").ToString
        '                        .IndustryType = row.Item("Industry").ToString

        '                        .BizLocID = row.Item("BizLocID").ToString
        '                        .BranchName = row.Item("BranchName").ToString
        '                        .Address1 = row.Item("Address1").ToString
        '                        .Address2 = row.Item("Address2").ToString
        '                        .Address3 = row.Item("Address3").ToString
        '                        .Address4 = row.Item("Address4").ToString
        '                        .AccNo = row.Item("AccNo").ToString
        '                        .ContactPerson = row.Item("ContactPerson").ToString
        '                        .ContactDesignation = row.Item("ContactDesignation").ToString
        '                        .ContactTelNo = row.Item("ContactTelNo").ToString
        '                        .Fax = row.Item("Fax").ToString
        '                        .ContactMobile = row.Item("ContactMobile").ToString
        '                        .ContactEmail = row.Item("ContactEmail").ToString

        '                    End With
        '                    listTransporter.Add(rTransporter)
        '                Next
        '            End If
        '        Catch ex As Exception
        '            Log.Notifier.Notify(ex)
        '            Gibraltar.Agent.Log.Error("Actions/ConsignHDR", ex.Message & " " & strSQL, ex.StackTrace)
        '        Finally
        '            EndSQLControl()
        '        End Try
        '    End If
        '    EndConnection()
        '    Return listTransporter
        'End Function

        ''Add for Load all Location at one Company
        'Public Overloads Function GetLocationListByCompany(Optional ByVal Condition As String = Nothing) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo 'xxxx

        '            'strSQL = "select ROW_NUMBER() OVER (ORDER BY B.CompanyName asc) as RecNo,L.AccNo,B.CompanyName, ISNULL((select TOP 1 SICDesc from SIC where SICCode=B.IndustryType),'') as Industry" & _
        '            '                                ",L.Address1,L.ContactPerson,L.ContactDesignation,L.ContactTelNo as ContactPersonTelNo,L.Fax as ContactPersonFaxNo," & _
        '            '                                "L.ContactMobile as ContactPersonMobile,L.ContactEmail as ContactPersonEmail,B.BizRegID as CompanyID,L.BizLocID,L.BranchName" & _
        '            '                                " from BIZENTITY B inner join BIZLOCATE L on B.BizRegID = L.BizRegID"

        '            strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY x.CompanyName asc) as RecNo, * " &
        '                " FROM ( " &
        '                " SELECT DISTINCT L.AccNo,B.CompanyName, " &
        '                " ISNULL(SIC.SICDesc,'') as Industry, (L.Address1 + ' ' + L.Address2 + ' ' + L.Address3 + ' ' + L.Address4) As Address1, L.ContactPerson, L.ContactDesignation, L.ContactTelNo as ContactPersonTelNo, " &
        '                " L.Fax as ContactPersonFaxNo, L.ContactMobile as ContactPersonMobile, L.ContactEmail as ContactPersonEmail, B.BizRegID as CompanyID, L.BizLocID, L.BranchName, LI.ContType, LI.ContCategory, LC.LicenseDesc  " &
        '                " FROM BIZENTITY B WITH (NOLOCK) " &
        '                " INNER JOIN BIZLOCATE L WITH (NOLOCK) on B.BizRegID = L.BizRegID " &
        '                " LEFT JOIN LICENSE LI WITH (NOLOCK) on L.BizLocID = LI.LocID and L.BizRegID=LI.CompanyID  " &
        '                " LEFT JOIN LICENSECATEGORY LC WITH (NOLOCK) on LI.ContType = LC.LicenseType AND LI.ContCategory = LC.LicenseCode " &
        '                " LEFT JOIN SIC WITH (NOLOCK) ON SIC.SICCode=B.IndustryType " &
        '                " WHERE LI.conttype='T' AND L.Flag=1 AND " & Condition & ") as x " &
        '                " ORDER BY CompanyName ASC"

        '            'If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE L.Flag=1 AND " & Condition

        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        'End Function

        ''GetCompanyReceiver
        'Public Overloads Function GetLocationListByCompanyReceiver(Optional ByVal Condition As String = Nothing, Optional ByVal TransType As String = Nothing, Optional ByVal IsOverdue As Boolean = False) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo

        '            'Roslyn, 04/02/15, for Trans Type Special Management Area in new CN, removed linked to License
        '            If TransType = "CN3" Then
        '                strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY B.CompanyName asc) as RecNo, L.AccNo, B.CompanyName, " &
        '                    " ISNULL(SIC.SICDesc,'') as Industry, " &
        '                    " (L.Address1 + ' ' + L.Address2 + ' ' + L.Address3 + ' ' + L.Address4) As Address1, L.ContactPerson, L.ContactDesignation, L.ContactTelNo as ContactPersonTelNo, L.Fax as ContactPersonFaxNo, " &
        '                    " L.ContactMobile as ContactPersonMobile, L.ContactEmail as ContactPersonEmail, B.BizRegID as CompanyID, L.BizLocID, L.BranchName " &
        '                    " FROM BIZENTITY B WITH (NOLOCK) " &
        '                    " INNER JOIN BIZLOCATE L WITH (NOLOCK) on B.BizRegID = L.BizRegID " &
        '                    " LEFT JOIN SIC WITH (NOLOCK) ON SIC.SICCode=B.IndustryType " &
        '                    " WHERE B.Active=1 AND L.Active=1 AND L.Flag=1 " &
        '                    " ORDER BY CompanyName ASC "
        '            Else

        '                strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY x.CompanyName asc) as RecNo, * " &
        '                    " FROM ( " &
        '                    " SELECT DISTINCT L.AccNo,B.CompanyName, " &
        '                    " ISNULL(SIC.SICDesc,'') as Industry, " &
        '                    " (L.Address1 + ' ' + L.Address2 + ' ' + L.Address3 + ' ' + L.Address4) As Address1, " &
        '                    " L.ContactPerson,L.ContactDesignation,L.ContactTelNo as ContactPersonTelNo, " &
        '                    " L.Fax as ContactPersonFaxNo,L.ContactMobile as ContactPersonMobile,L.ContactEmail as ContactPersonEmail,B.BizRegID as CompanyID,L.BizLocID,L.BranchName, LI.ContType, LI.ContCategory, LC.LicenseDesc " &
        '                    " FROM BIZENTITY B WITH (NOLOCK) " &
        '                    " INNER JOIN BIZLOCATE L WITH (NOLOCK) on B.BizRegID = L.BizRegID " &
        '                    " LEFT JOIN LICENSE LI WITH (NOLOCK) on L.BizLocID = LI.LocID and L.BizRegID=LI.CompanyID  " &
        '                    " LEFT JOIN LICENSECATEGORY LC WITH (NOLOCK) on LI.ContType = LC.LicenseType AND LI.ContCategory = LC.LicenseCode " &
        '                    " LEFT JOIN SIC WITH (NOLOCK) ON SIC.SICCode=B.IndustryType " &
        '                    " WHERE LI.conttype='R' AND B.Active=1 AND L.Active=1 AND L.Flag=1 AND " & Condition & " AND LI.InUse = 1) as x  " &
        '                    " ORDER BY CompanyName ASC "
        '            End If

        '            If IsOverdue = True Then
        '                strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY x.CompanyName asc) as RecNo, * " &
        '              " FROM ( " &
        '              " SELECT DISTINCT L.AccNo,B.CompanyName, " &
        '              " ISNULL(SIC.SICDesc,'') as Industry, " &
        '              " (L.Address1 + ' ' + L.Address2 + ' ' + L.Address3 + ' ' + L.Address4) As Address1, " &
        '              " L.ContactPerson,L.ContactDesignation,L.ContactTelNo as ContactPersonTelNo, " &
        '              " L.Fax as ContactPersonFaxNo,L.ContactMobile as ContactPersonMobile,L.ContactEmail as ContactPersonEmail,B.BizRegID as CompanyID,L.BizLocID,L.BranchName, LI.ContType" &
        '              " FROM BIZENTITY B WITH (NOLOCK) " &
        '              " INNER JOIN BIZLOCATE L WITH (NOLOCK) on B.BizRegID = L.BizRegID " &
        '              " LEFT JOIN LICENSE LI WITH (NOLOCK) on L.BizLocID = LI.LocID  " &
        '              " LEFT JOIN SIC WITH (NOLOCK) ON SIC.SICCode=B.IndustryType " &
        '              " WHERE LI.conttype='R' AND B.Active=1 AND L.Active=1 AND L.Flag=1 AND " & Condition & " AND LI.InUse = 1) as x  " &
        '              " ORDER BY CompanyName ASC "
        '            End If

        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        'End Function

        ''Add for Load all waste
        'Public Overloads Function GetWasteList(ByVal LocID As String) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo
        '            strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY LEN(I.ItemCode), I.ItemCode) as RecNo,I.ItemName,I.ItemComponent,I.PackUOM, " &
        '                " (select TOP 1 UOMDesc from UOM WITH (NOLOCK) where UOMCode=I.PackUOM) as PackUOMDesc, " &
        '                " I.ItemCode,I.ItemDesc, " &
        '                " 0 as Qty, 0 as PackQty,0 as QtyPerMonth, 0 as PackQtyPerMonth, I.QtyOnHand, " &
        '                " (select TOP 1 CodeDesc from CODEMASTER WITH (NOLOCK) where Code=I.TypeCode AND CodeType='WTY') as WasteType, " &
        '                " (select convert(varchar(10),MAX(AuthorisedDate),103) from CONSIGNDTL WITH (NOLOCK) where WasteCode=I.ItemCode) as LastTransDate, " &
        '                " I.QtyOnHand*1000 as QtyOnHandKg " &
        '                " FROM ITEMLOC I WITH (NOLOCK) " &
        '                " LEFT JOIN ITEM W WITH (NOLOCK) on I.ItemCode=W.ItemCode " &
        '                " WHERE I.LocID='" & LocID & "' AND I.Flag=1" &
        '                " ORDER BY LEN(I.ItemCode), I.ItemCode "

        '            'strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY I.QtyOnHand desc) as RecNo,I.ItemCode,I.ItemDesc," & _
        '            '                     " S.StorageID,S.StorageAreaCode,S.StorageBin," & _
        '            '                    "0 as Qty, 0 as PackQty,0 as QtyPerMonth, 0 as PackQtyPerMonth, I.QtyOnHand" & _
        '            '                    " ,(select CodeDesc from CODEMASTER where Code=W.TypeCode) as WasteType" & _
        '            '                    ", (select convert(varchar(10),MAX(AuthorisedDate),103) from CONSIGNDTL where WasteCode=I.ItemCode) as LastTransDate " & _
        '            '                    ",I.QtyOnHand*1000 as QtyOnHandKg" & _
        '            '                    " FROM ITEMLOC I LEFT JOIN StorageMaster S on I.StorageID=S.StorageID" & _
        '            '                    " AND I.LocID=S.LocID" & _
        '            '                    " LEFT JOIN ITEM W on I.ItemCode=W.ItemCode" & _
        '            '                    " WHERE I.Flag=1 and S.LocID='" & LocID & "'" & _
        '            '                    " ORDER BY LEN(I.ItemCode), I.ItemCode"
        '            '" ORDER BY QtyOnHand desc"

        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        'End Function

        ''Add
        'Public Overloads Function GetReceiverCustomList(Optional ByVal Condition As String = Nothing) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo
        '            strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY B.CompanyName asc) as RecNo,H.*,L.AccNo, " &
        '                " ISNULL(L.BranchName,'') as CompanyName, " &
        '                " ISNULL(SIC.SICDesc,'') as Industry, " &
        '                " H.ReceiverAddress,H.ReceiverPIC,ReceiverPos ,H.ReceiverTelNo,B.ContactPersonMobile as ReceiverMobile,H.ReceiverFaxNo,H.ReceiverEmailAddress, " &
        '                " B.ContactPersonMobile,B.ContactPersonEmail,B.BizRegID as CompanyID, " &
        '                " ISNULL(S.StateDesc,'') as StateDesc, " &
        '                " ISNULL(CO.CountryDesc,'') as CountryDesc, " &
        '                " ISNULL(PBT.PBTDesc,'') as PBTDesc, " &
        '                " ISNULL(C.CityDesc,'') as CityDesc, " &
        '                " ISNULL(CM.CodeDesc,'') as ReceiverPosDesc " &
        '                " FROM CONSIGNHDR H WITH (NOLOCK) " &
        '                " LEFT JOIN BIZENTITY B WITH (NOLOCK) on H.ReceiverID=B.BizRegID " &
        '                " LEFT JOIN BIZLOCATE L WITH (NOLOCK) on B.BizRegID = L.BizRegID " &
        '                " LEFT JOIN SIC WITH (NOLOCK) ON SIC.SICCode=B.IndustryType " &
        '                " LEFT JOIN STATE S WITH (NOLOCK) ON S.StateCode=B.State and S.CountryCode=B.Country " &
        '                " LEFT JOIN COUNTRY CO WITH (NOLOCK) ON CO.CountryCode=B.Country " &
        '                " LEFT JOIN PBT WITH (NOLOCK) ON PBT.PBTCode=B.PBT and PBT.CountryCode=B.Country and PBT.StateCode=B.State " &
        '                " LEFT JOIN CITY C WITH (NOLOCK) ON C.CityCode=B.City and C.CountryCode=B.Country and C.StateCode=B.State " &
        '                " LEFT JOIN CODEMASTER CM WITH (NOLOCK) ON CM.CodeType='DSN' AND CM.Code=H.ReceiverPos "


        '            If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition

        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        'End Function

        ''function for ISWIS_API
        ''Public Overloads Function GetReceiverCustomList(ByVal ValidityEndDate As System.DateTime) As List(Of Profiles.Container.Bizlocate)
        ''    Dim rReceiver As Profiles.Container.Bizlocate = Nothing
        ''    Dim listReceiver As List(Of Profiles.Container.Bizlocate) = Nothing
        ''    Dim dtTemp As DataTable = Nothing

        ''    If StartConnection() = True Then
        ''        StartSQLControl()
        ''        strSQL = "SELECT * " & _
        ''                 " FROM (SELECT DISTINCT L.AccNo, B.CompanyName, ISNULL(SIC.SICDesc,'') as Industry, L.Address1, L.Address2, L.Address3, L.Address4, " & _
        ''                            " L.ContactPerson, L.ContactDesignation, L.ContactTelNo, " & _
        ''                            " L.Fax, L.ContactMobile, L.ContactEmail, B.BizRegID, L.BizLocID, L.BranchName " & _
        ''                        " FROM BIZENTITY B WITH (NOLOCK) " & _
        ''                        " INNER JOIN BIZLOCATE L WITH (NOLOCK) on B.BizRegID = L.BizRegID " & _
        ''                        " LEFT JOIN LICENSE LI WITH (NOLOCK) on L.BizLocID = LI.LocID and L.BizRegID=LI.CompanyID  " & _
        ''                        " LEFT JOIN SIC WITH (NOLOCK) ON SIC.SICCode=B.IndustryType " & _
        ''                        " WHERE LI.conttype='R' AND B.StoreStatus=1 AND B.Active=1 AND L.Active=1 AND L.Flag=1 AND LI.ValidityEnd >= " & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtDateTime, ValidityEndDate) & _
        ''                        " AND CompanyType IN ('4','6','7','9') ) as x " & _
        ''                 " ORDER BY CompanyName ASC "

        ''        Try
        ''            dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
        ''            If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
        ''                listReceiver = New List(Of Profiles.Container.Bizlocate)
        ''                For Each row As DataRow In dtTemp.Rows
        ''                    rReceiver = New Profiles.Container.Bizlocate
        ''                    With rReceiver
        ''                        .BizRegID = row.Item("BizRegID").ToString
        ''                        .CompanyName = row.Item("CompanyName").ToString
        ''                        .IndustryType = row.Item("Industry").ToString

        ''                        .BizLocID = row.Item("BizLocID").ToString
        ''                        .BranchName = row.Item("BranchName").ToString
        ''                        .Address1 = row.Item("Address1").ToString
        ''                        .Address2 = row.Item("Address2").ToString
        ''                        .Address3 = row.Item("Address3").ToString
        ''                        .Address4 = row.Item("Address4").ToString
        ''                        .AccNo = row.Item("AccNo").ToString
        ''                        .ContactPerson = row.Item("ContactPerson").ToString
        ''                        .ContactDesignation = row.Item("ContactDesignation").ToString
        ''                        .ContactTelNo = row.Item("ContactTelNo").ToString
        ''                        .Fax = row.Item("Fax").ToString
        ''                        .ContactMobile = row.Item("ContactMobile").ToString
        ''                        .ContactEmail = row.Item("ContactEmail").ToString

        ''                    End With
        ''                    listReceiver.Add(rReceiver)
        ''                Next
        ''            End If
        ''        Catch ex As Exception
        ''            Log.Notifier.Notify(ex)
        ''            Gibraltar.Agent.Log.Error("Actions/ConsignHDR", ex.Message & " " & strSQL, ex.StackTrace)
        ''        Finally
        ''            EndSQLControl()
        ''        End Try
        ''    End If
        ''    EndConnection()
        ''    Return listReceiver
        ''End Function

        ''Add
        'Public Overloads Function GetTransID(ByVal GeneratorLocID As String, ByVal WasteCode As String, ByVal ReceiverLocID As String) As Data.DataTable
        '    Try
        '        If StartConnection() = True Then
        '            With ConsignhdrInfo.MyInfo
        '                StartSQLControl()
        '                strSQL = " SELECT HDR.TransID FROM CONSIGNHDR HDR INNER JOIN CONSIGNDTL DTL ON HDR.TransID = DTL.TransID " &
        '                    "WHERE HDR.GeneratorLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorLocID) & "' AND " &
        '                    "DTL.WasteCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "' And HDR.IsNew = 0 AND HDR.Status = 8 AND HDR.ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "'"
        '                Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '            End With
        '        Else
        '            Return Nothing
        '        End If
        '    Catch ex As Exception
        '        Log.Notifier.Notify(ex)
        '        Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", ex.Message & " " & strSQL, ex.StackTrace)
        '        'Throw ex
        '    Finally
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        '    Return Nothing
        'End Function

        'Public Overloads Function GetGeneratorCustomList(Optional ByVal Condition As String = Nothing) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo
        '            'strSQL = "select ROW_NUMBER() OVER (ORDER BY B.CompanyName asc) as RecNo,H.*,L.AccNo,B.CompanyName,(select CodeDesc from CODEMASTER where Code=B.IndustryType) as Industry" & _
        '            '                        ",B.Address1,B.ContactPerson,B.ContactDesignation,B.ContactPersonTelNo,B.ContactPersonFaxNo," & _
        '            '                        "B.ContactPersonMobile,B.ContactPersonEmail,B.BizRegID as CompanyID" & _
        '            '                        ",(select StateDesc from STATE where StateCode=B.State) as StateDesc,(select CountryDesc from COUNTRY where CountryCode=B.Country) as CountryDesc,(select CityDesc from CITY where CityCode=B.City) as CityDesc" & _
        '            '                        " from CONSIGNHDR H left join BIZENTITY B on H.GeneratorID=B.BizRegID left join BIZLOCATE L on B.BizRegID = L.BizRegID"

        '            strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY B.CompanyName asc) as RecNo,H.*,L.AccNo, " &
        '                " ISNULL((select CompanyName from BIZENTITY where BizRegID=H.GeneratorID),'') as CompanyName, " &
        '                " ISNULL((select TOP 1 SICDesc FROM SIC WHERE SICCode=B.IndustryType),'') as Industry, " &
        '                " H.GeneratorAddress,H.GeneratorPIC,GeneratorPos ,H.GeneratorTelNo, " &
        '                " ISNULL((select EmerContactNo FROM EMPLOYEE WHERE EmployeeID=H.CreateBy),ISNULL(L.ContactTelNo,ISNULL(B.ContactPersonTelNo,''))) as GeneratorMobile, " &
        '                " H.GeneratorFaxNo,H.GeneratorEmailAddress," &
        '                " B.ContactPersonMobile,B.ContactPersonEmail,B.BizRegID as CompanyID, " &
        '                " ISNULL((select TOP 1 StateDesc from STATE where StateCode=B.State and CountryCode=B.Country),'') as StateDesc, " &
        '                " ISNULL((select TOP 1 CountryDesc from COUNTRY where CountryCode=B.Country),'') as CountryDesc, " &
        '                " ISNULL((select TOP 1 PBTDesc from PBT where PBTCode=B.PBT and CountryCode=B.Country and StateCode=B.State),'') as PBTDesc, " &
        '                " ISNULL((select CityDesc from CITY where CityCode=B.City and CountryCode=B.Country and StateCode=B.State),'') as CityDesc" &
        '                " FROM CONSIGNHDR H WITH (NOLOCK) " &
        '                " LEFT JOIN BIZENTITY B WITH (NOLOCK) on H.GeneratorID=B.BizRegID " &
        '                " left join BIZLOCATE L WITH (NOLOCK) on B.BizRegID = L.BizRegID "

        '            If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition

        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        'End Function

        'Public Function GetFacilityList(ByVal TransType As String, ByVal CompanyType As String, ByVal CompanyName As String) As List(Of Profiles.Container.Bizlocate)
        '    Dim rBizLocate As Profiles.Container.Bizlocate = Nothing
        '    Dim listBizLocate As List(Of Profiles.Container.Bizlocate) = Nothing
        '    Dim dtTemp As DataTable = Nothing
        '    Dim Condition As String = ""
        '    If StartConnection() = True Then
        '        StartSQLControl()
        '        If CompanyType = "T" Then 'Transporter
        '            If TransType = "CN1" Then
        '                Condition = "LI.ValidityEnd >= convert(date,GETDATE()) AND CompanyType IN ('3','5','7','9') AND Replace(B.CompanyName, ' ','') =  Replace('" & CompanyName & "',' ','') "
        '            ElseIf TransType = "CN2" Then
        '                Condition = "LI.ValidityEnd >= convert(date,GETDATE()) AND CompanyType IN ('3','5','7','9') AND B.State IN ('12', '13') AND Replace(B.CompanyName, ' ','') =  Replace('" & CompanyName & "',' ','') "
        '            End If
        '            strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY x.CompanyName asc) as RecNo, * " &
        '                     " FROM ( " &
        '                     " SELECT DISTINCT L.AccNo, B.BizRegID, B.CompanyName, " &
        '                     " ISNULL(SIC.SICDesc,'') as Industry, L.BranchType, (L.Address1 + ' ' + L.Address2 + ' ' + L.Address3 + ' ' + L.Address4) As Address1, L.ContactPerson, L.ContactDesignation, L.ContactTelNo as ContactPersonTelNo, " &
        '                     " L.Fax as ContactPersonFaxNo, L.ContactMobile as ContactPersonMobile, L.ContactEmail as ContactPersonEmail, B.BizRegID as CompanyID, L.BizLocID, L.BranchName, LC.LicenseDesc   " &
        '                     " FROM BIZENTITY B WITH (NOLOCK) " &
        '                     " INNER JOIN BIZLOCATE L WITH (NOLOCK) on B.BizRegID = L.BizRegID " &
        '                     " LEFT JOIN LICENSE LI WITH (NOLOCK) on L.BizLocID = LI.LocID and L.BizRegID=LI.CompanyID  " &
        '                     " LEFT JOIN LICENSECATEGORY LC WITH (NOLOCK) on LI.ContType = LC.LicenseType AND LI.ContCategory = LC.LicenseCode " &
        '                     " LEFT JOIN SIC WITH (NOLOCK) ON SIC.SICCode=B.IndustryType " &
        '                     " WHERE LI.conttype='T' AND L.Flag=1 AND " & Condition & ") as x " &
        '                     " ORDER BY CompanyName ASC"
        '        ElseIf CompanyType = "R" Then 'Receiver
        '            If TransType = "CN1" Then
        '                Condition = "LI.ValidityEnd >= convert(date,GETDATE()) AND CompanyType IN ('4','6','7','9') AND Replace(B.CompanyName, ' ','') =  Replace('" & CompanyName & "',' ','') "
        '            ElseIf TransType = "CN2" Then
        '                Condition = "LI.ValidityEnd >= convert(date,GETDATE()) AND B.State NOT IN ('12', '13') AND Replace(B.CompanyName, ' ','') =  Replace('" & CompanyName & "',' ','') "
        '            End If
        '            strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY x.CompanyName asc) as RecNo, * " &
        '                     " FROM ( " &
        '                     " SELECT DISTINCT L.AccNo, B.BizRegID, B.CompanyName, " &
        '                     " ISNULL(SIC.SICDesc,'') as Industry, L.BranchType, " &
        '                     " (L.Address1 + ' ' + L.Address2 + ' ' + L.Address3 + ' ' + L.Address4) As Address1, " &
        '                     " L.ContactPerson,L.ContactDesignation,L.ContactTelNo as ContactPersonTelNo, " &
        '                     " L.Fax as ContactPersonFaxNo,L.ContactMobile as ContactPersonMobile,L.ContactEmail as ContactPersonEmail,B.BizRegID as CompanyID,L.BizLocID,L.BranchName, LC.LicenseDesc   " &
        '                     " FROM BIZENTITY B WITH (NOLOCK) " &
        '                     " INNER JOIN BIZLOCATE L WITH (NOLOCK) on B.BizRegID = L.BizRegID " &
        '                     " LEFT JOIN LICENSE LI WITH (NOLOCK) on L.BizLocID = LI.LocID and L.BizRegID=LI.CompanyID  " &
        '                     " LEFT JOIN LICENSECATEGORY LC WITH (NOLOCK) on LI.ContType = LC.LicenseType AND LI.ContCategory = LC.LicenseCode " &
        '                     " LEFT JOIN SIC WITH (NOLOCK) ON SIC.SICCode=B.IndustryType " &
        '                     " WHERE LI.conttype='R' AND B.StoreStatus=1 AND B.Active=1 AND L.Active=1 AND L.Flag=1 AND " & Condition & ") as x " &
        '                     " ORDER BY CompanyName ASC "
        '        End If
        '        Try
        '            dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
        '            If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
        '                listBizLocate = New List(Of Profiles.Container.Bizlocate)
        '                For Each row As DataRow In dtTemp.Rows
        '                    rBizLocate = New Profiles.Container.Bizlocate
        '                    With rBizLocate
        '                        .BizRegID = row.Item("BizRegID").ToString
        '                        .CompanyName = row.Item("CompanyName").ToString
        '                        .IndustrytypeDesc = row.Item("LicenseDesc").ToString
        '                        .BizLocID = row.Item("BizLocID").ToString
        '                        .BranchName = row.Item("BranchName").ToString
        '                        .BranchType = row.Item("BranchType").ToString
        '                        .Address1 = row.Item("Address1").ToString
        '                        .AccNo = row.Item("AccNo").ToString
        '                        .ContactPerson = row.Item("ContactPerson").ToString
        '                        .ContactTelNo = row.Item("ContactPersonTelNo").ToString
        '                        .Fax = row.Item("ContactPersonFaxNo").ToString
        '                        .ContactMobile = row.Item("ContactPersonMobile").ToString
        '                        .ContactEmail = row.Item("ContactPersonEmail").ToString
        '                    End With
        '                    listBizLocate.Add(rBizLocate)
        '                Next
        '            End If
        '        Catch ex As Exception
        '            Log.Notifier.Notify(ex)
        '            Gibraltar.Agent.Log.Error("Actions/Notification", ex.Message & " " & strSQL, ex.StackTrace)
        '        Finally
        '            EndSQLControl()
        '        End Try
        '    End If
        '    EndConnection()
        '    Return listBizLocate
        'End Function

        ''Add
        'Public Overloads Function GetTop10TransporterTransaction(ByVal CNNO As String, ByVal TransporterID As String, ByVal GeneratorID As String) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo

        '            strSQL = "SELECT TOP 10 H.ContractNo, D.WasteCode, D.WasteDescription, H.TransportDate, " &
        '                " ISNULL(B.CompanyName,'') as Destination, SUM(D.Qty) as Qty " &
        '                " FROM CONSIGNHDR H WITH (NOLOCK) " &
        '                " INNER JOIN CONSIGNDTL D WITH (NOLOCK) on H.ContractNo=D.ContractNo " &
        '                " LEFT JOIN BIZENTITY B WITH (NOLOCK) ON B.BizRegID=H.ReceiverID " &
        '                " WHERE H.ContractNo <> '" & CNNO & "' AND H.TransporterID='" & TransporterID & "' AND H.GeneratorID='" & GeneratorID & "' and H.TransportDate is not null" &
        '                " GROUP BY H.ContractNo, D.WasteCode, D.WasteDescription, H.TransportDate, B.CompanyName, H.ReceiverID " &
        '                " ORDER BY H.TransportDate DESC "

        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        'End Function

        'Public Overloads Function GetConsignHDRList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo
        '            If SQL = Nothing Or SQL = String.Empty Then
        '                strSQL = BuildSelect(.FieldsList, .TableName, FieldCond)
        '            Else
        '                strSQL = SQL
        '            End If
        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        'End Function

        'Public Overloads Function GetConsignHDRShortList(ByVal ShortListing As Boolean) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo
        '            If ShortListing Then
        '                strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
        '            Else
        '                strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
        '            End If
        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With
        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        'End Function


        'Public Overloads Function GetListCustomConsignmentNoteWS(ByVal CompanyType As Integer, ByVal CompanyID As String, ByVal LocationID As String, ByVal CN As String, Optional ByVal OrderField As String = Nothing) As Data.DataTable
        '    Dim Condition As String = Nothing
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo
        '            'Dim strStatus As String = "CASE Status WHEN 0 Then 'Draft' WHEN 1 THEN 'Pending Transfer' WHEN 2 THEN 'Cancelled' " & _
        '            '    "WHEN 3 THEN 'Pending Transfer' WHEN 4 THEN 'Pending Received' WHEN 5 THEN 'Cancelled Transfer' " & _
        '            '    "WHEN 6 THEN 'Pending Receiving' WHEN 7 THEN 'Pending Receiving' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '            '    "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' END as Status"

        '            'If CompanyType = 2 Then
        '            '    strStatus = "CASE Status WHEN 0 Then 'Draft' WHEN 1 THEN 'Pending Transfer' WHEN 2 THEN 'Cancelled' " & _
        '            '    "WHEN 3 THEN 'Pending Transfer' WHEN 4 THEN 'Pending Received' WHEN 5 THEN 'Cancelled Transfer' " & _
        '            '    "WHEN 6 THEN 'Pending Receiving' WHEN 7 THEN 'Pending Receiving' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '            '    "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' END as Status"
        '            'ElseIf CompanyType = 3 Then
        '            '    strStatus = "CASE Status WHEN 0 Then 'Draft' WHEN 1 THEN 'Pending Transfer' WHEN 2 THEN 'Cancelled' " & _
        '            '    "WHEN 3 THEN 'Draft Transfer' WHEN 4 THEN 'Pending Received' WHEN 5 THEN 'Cancelled Transfer' " & _
        '            '    "WHEN 6 THEN 'Pending Receiving' WHEN 7 THEN 'Pending Receiving' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '            '    "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' END as Status"
        '            'ElseIf CompanyType = 4 Then
        '            '    strStatus = "CASE Status WHEN 0 Then 'Draft' WHEN 1 THEN 'Pending Transfer' WHEN 2 THEN 'Cancelled' " & _
        '            '        "WHEN 3 THEN 'Pending Transfer' WHEN 4 THEN 'Pending Received' WHEN 5 THEN 'Cancelled Transfer' " & _
        '            '        "WHEN 6 THEN 'Draft Receiving' WHEN 7 THEN 'Draft Receiving' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '            '        "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' END as Status"
        '            'End If

        '            'Dim strStatus As String = "CASE Status WHEN 0 Then 'Draft' WHEN 1 THEN 'Submitted' WHEN 2 THEN 'Cancelled' " & _
        '            '    "WHEN 3 THEN 'Pending Transfer' WHEN 4 THEN 'Pending Received' WHEN 5 THEN 'Cancelled Transfer' " & _
        '            '    "WHEN 6 THEN 'Pending Receiving' WHEN 7 THEN 'Pending Receiving' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '            '    "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' END as Status"

        '            'If CompanyType = 2 Then
        '            '    strStatus = "CASE Status WHEN 0 Then 'Draft' WHEN 1 THEN 'Submitted' WHEN 2 THEN 'Cancelled' " & _
        '            '    "WHEN 3 THEN 'Pending Transfer' WHEN 4 THEN 'Pending Received' WHEN 5 THEN 'Cancelled Transfer' " & _
        '            '    "WHEN 6 THEN 'Pending Receiving' WHEN 7 THEN 'Pending Receiving' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '            '    "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' END as Status"
        '            'ElseIf CompanyType = 3 Then
        '            '    strStatus = "CASE Status WHEN 0 Then 'Draft' WHEN 1 THEN 'Submitted' WHEN 2 THEN 'Cancelled' " & _
        '            '    "WHEN 3 THEN 'Draft Transfer' WHEN 4 THEN 'Pending Received' WHEN 5 THEN 'Cancelled Transfer' " & _
        '            '    "WHEN 6 THEN 'Pending Receiving' WHEN 7 THEN 'Pending Receiving' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '            '    "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' END as Status"
        '            'ElseIf CompanyType = 4 Then
        '            '    strStatus = "CASE Status WHEN 0 Then 'Draft' WHEN 1 THEN 'Submitted' WHEN 2 THEN 'Cancelled' " & _
        '            '        "WHEN 3 THEN 'Pending Transfer' WHEN 4 THEN 'Pending Received' WHEN 5 THEN 'Cancelled Transfer' " & _
        '            '        "WHEN 6 THEN 'Draft Receiving' WHEN 7 THEN 'Draft Receiving' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " & _
        '            '        "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' END as Status"
        '            'End If

        '            Dim strStatus As String = "CASE CONSIGNHDR.Status WHEN 0 Then 'Draft' WHEN 1 THEN " &
        '                                      "CASE CONSIGNHDR.isConfirm WHEN 0 Then 'Submitted' WHEN 1 Then 'Submitted' END " &
        '                                      "WHEN 2 THEN 'Cancelled' " &
        '                "WHEN 3 THEN 'Acknowledged' WHEN 4 THEN 'Acknowledged' WHEN 5 THEN 'Cancelled Transfer' " &
        '                "WHEN 6 THEN 'On Hold' WHEN 7 THEN 'On Hold' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " &
        '                "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' WHEN 12 THEN 'Cancelled' END as Status"

        '            If CompanyType = 2 Then
        '                strStatus = "CASE CONSIGNHDR.Status WHEN 0 Then 'Draft' WHEN 1 THEN " &
        '                                      "CASE CONSIGNHDR.isConfirm WHEN 0 Then 'Submitted' WHEN 1 Then 'Submitted' END " &
        '                                      "WHEN 2 THEN 'Cancelled' " &
        '                "WHEN 3 THEN 'Acknowledged' WHEN 4 THEN 'Acknowledged' WHEN 5 THEN 'Cancelled Transfer' " &
        '                "WHEN 6 THEN 'On Hold' WHEN 7 THEN 'On Hold' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " &
        '                "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' WHEN 12 THEN 'Cancelled' " &
        '                "WHEN 13 THEN 'Draft' WHEN 14 THEN 'Submitted' WHEN 15 THEN 'Cancelled' END as Status"
        '            ElseIf CompanyType = 3 Then
        '                strStatus = "CASE CONSIGNHDR.Status WHEN 0 Then 'Draft' WHEN 1 THEN " &
        '                                      "CASE CONSIGNHDR.isConfirm WHEN 0 Then 'Submitted' WHEN 1 Then 'Submitted' END " &
        '                                      "WHEN 2 THEN 'Cancelled' " &
        '                "WHEN 3 THEN 'Acknowledged' WHEN 4 THEN 'Acknowledged' WHEN 5 THEN 'Cancelled Transfer' " &
        '                "WHEN 6 THEN 'On Hold' WHEN 7 THEN 'On Hold' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " &
        '                "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' WHEN 12 THEN 'Cancelled' END as Status"
        '            ElseIf CompanyType = 4 Then
        '                strStatus = "CASE CONSIGNHDR.Status WHEN 0 Then 'Draft' WHEN 1 THEN " &
        '                                      "CASE CONSIGNHDR.isConfirm WHEN 0 Then 'Submitted' WHEN 1 Then 'Submitted' END " &
        '                                      "WHEN 2 THEN 'Cancelled' " &
        '                    "WHEN 3 THEN 'Acknowledged' WHEN 4 THEN 'Acknowledged' WHEN 5 THEN 'Cancelled Transfer' " &
        '                    "WHEN 6 THEN 'On Hold' WHEN 7 THEN 'On Hold' WHEN 8 THEN 'Received' WHEN 9 THEN 'Rejected' " &
        '                    "WHEN 10 THEN 'On Hold' WHEN 11 THEN 'Cancelled' WHEN 12 THEN 'Cancelled' END as Status"
        '            End If

        '            strSQL = "SELECT CONSIGNHDR.ContractNo, CONSIGNHDR.ReferID, CONSIGNDTL.WasteCode AS WasteCode, CONSIGNDTL.Qty AS Qty, " &
        '                " CONSIGNDTL.RcvQty AS RcvQty, (CONSIGNDTL.Qty-CONSIGNDTL.RcvQty) AS VarianceQty, " &
        '                " CG.CompanyName as GeneratorName, " &
        '                " CT.CompanyName as TransporterName, " &
        '                " CR.CompanyName as ReceiverName," &
        '                " CONSIGNHDR.TransDate, CONSIGNHDR.TransportDate, CONSIGNHDR.TargetTransportDate, CONSIGNHDR.TargetReceiveDate, CONSIGNHDR.ReceiveDate, CONSIGNHDR.Status AS SubmitStatus, " & strStatus &
        '                " FROM CONSIGNHDR WITH (NOLOCK) " &
        '                " LEFT JOIN CONSIGNDTL WITH (NOLOCK) ON CONSIGNDTL.TransID=CONSIGNHDR.TransID " &
        '                " LEFT JOIN BIZENTITY CG WITH (NOLOCK) ON CG.BizRegID=CONSIGNHDR.GeneratorID " &
        '                " LEFT JOIN BIZENTITY CT WITH (NOLOCK) ON CT.BizRegID=CONSIGNHDR.TransporterID " &
        '                " LEFT JOIN BIZENTITY CR WITH (NOLOCK) ON CR.BizRegID=CONSIGNHDR.ReceiverID "
        '            Condition = "((CONSIGNHDR.FLAG = 0) or (CONSIGNHDR.FLAG = 1))"
        '            If ((CN = "Transporter") Or (CN = "Generator")) Then
        '                'strFilter &= " AND " & Person & "ID='" & Session("UserCompany") & "'"
        '                Condition &= " AND " & CN & "ID='" & CompanyID & "'"
        '                Condition &= " AND " & CN & "LocID='" & LocationID & "'"

        '            End If
        '            Condition &= " AND (CONSIGNHDR.Status >= 1 and CONSIGNHDR.Status not in (2,5,8,9,11,13,14,15))"
        '            If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition

        '            strSQL &= " ORDER BY "

        '            If Not OrderField Is Nothing AndAlso OrderField <> "" Then
        '                strSQL &= OrderField
        '            Else
        '                strSQL &= "CONSIGNHDR.Status DESC, CONSIGNHDR.TargetReceiveDate DESC, CONSIGNHDR.TransDate DESC" 'added by diana, sorting
        '            End If


        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        'End Function

        ''Add by Richard 2014-09-11,Edited by Ivan 2014-10-15
        'Public Overloads Function GetCNDashboard(Optional ByVal condition As String = Nothing) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo
        '            StartSQLControl()
        '            strSQL = "SELECT top 10 L.Bizlocid, H.ContractNo, ISNULL(CT.CompanyName,'') as TransporterName, " &
        '                " ISNULL(CR.CompanyName,'') as ReceiverName, " &
        '                " H.TransDate, H.TargetTransportDate, H.TargetReceiveDate, " &
        '                " CASE H.Status WHEN '0' Then 'Draft' WHEN '1' Then " &
        '                " CASE H.IsConfirm WHEN 0 Then 'Submitted' WHEN 1 Then 'Submitted' END " &
        '                " WHEN '2' Then 'void' WHEN '3' Then 'Submitted' WHEN '4' Then 'Acknowledged' WHEN '5' Then 'Void Transfer' WHEN '6' Then 'On Hold' END as Status " &
        '                " FROM CONSIGNHDR H WITH (NOLOCK) " &
        '                " LEFT JOIN BizLocate L WITH (NOLOCK) ON L.BizRegID = H.GeneratorID AND L.BizLocID=H.GeneratorLocID " &
        '                " LEFT JOIN BIZENTITY CT WITH (NOLOCK) ON CT.BizRegID=H.TransporterID " &
        '                " LEFT JOIN BIZENTITY CR WITH (NOLOCK) ON CR.BizRegID=H.ReceiverID " &
        '                " WHERE " & condition

        '            strSQL &= " ORDER BY H.TargetReceiveDate DESC"

        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)


        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        '    EndSQLControl()
        'End Function

        'Public Overloads Function GetViewReceiver(ByVal State As String, ByVal Month As String, ByVal Year As String) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo
        '            StartSQLControl()
        '            strSQL = "SELECT ROW_NUMBER() OVER (order by SUM(RcvQty) DESC) as No, SUM(RcvQty) AS Total, COUNT(CH.ContractNo) AS " &
        '                " TotalCN, CR.CompanyName AS ReceiverName, CASE WHEN PBTDesc is null THEN ISNULL(StateDesc,'') ELSE " &
        '                " ISNULL(StateDesc,'') + ' | ' + ISNULL(PBTDesc,'') END AS StateDesc, AccNo AS DOENO, MAX(CH2.TRANSDATE)" &
        '                " as LastDate,(select top 1 b.CompanyName from CONSIGNHDR c, bizentity b where c.generatorid=b.bizregid and " &
        '                " c.receiverid=CH.receiverid AND c.TransDate=MAX(CH2.TRANSDATE) and c.Status=8 order by c.SyncCreate desc)" &
        '                " AS GeneratorName, CR.CompanyName AS CompanyName FROM ConsignHDR CH WITH (NOLOCK) INNER JOIN CONSIGNHDR CH2 WITH (NOLOCK) ON CH2.TransID=CH.TransID " &
        '                " INNER JOIN CONSIGNDTL WITH (NOLOCK) ON CONSIGNDTL.TransID=CH.TransID " &
        '                " LEFT JOIN BIZENTITY CG WITH (NOLOCK) ON CG.BizRegID=CH.GeneratorID " &
        '                " LEFT JOIN BIZLOCATE BZ WITH (NOLOCK) ON BZ.BizLocID=CH.ReceiverLocID " &
        '                " LEFT JOIN BIZENTITY CR WITH (NOLOCK) ON CR.BizRegID=CH.ReceiverID " &
        '                " LEFT JOIN STATE WITH (NOLOCK) ON CR.State=STATE.StateCode " &
        '                " LEFT JOIN PBT WITH (NOLOCK) ON CR.PBT=PBT.PBTCode WHERE MONTH(CH.TransDate)='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Month) & "' AND YEAR(CH.TransDate)='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Year) & "'" &
        '                " AND CH.Status =8 AND CR.State='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, State) & "' GROUP BY CR.CompanyName, StateDesc, PBTDesc, AccNo,CH.receiverid ORDER BY SUM(RcvQty) DESC"
        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With
        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        '    EndSQLControl()
        'End Function

        'Public Overloads Function GetViewReceiverDtl(ByVal CompanyName As String, ByVal Month As String, ByVal Year As String, ByVal State As String) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo
        '            StartSQLControl()
        '            strSQL = "select ROW_NUMBER() OVER (order by h.TransID DESC) as No, h.TransID AS TransID, RcvQty AS Qty, cr.companyname AS ReceiverName, cg.companyname AS GeneratorName, h.transdate AS TransDate " &
        '                " from Consignhdr h inner join consigndtl d on h.transid=d.transid " &
        '                " LEFT JOIN BIZENTITY CG WITH (NOLOCK) ON CG.BizRegID=h.GeneratorID " &
        '                " LEFT JOIN BIZENTITY CR WITH (NOLOCK) ON CR.BizRegID=h.ReceiverID " &
        '                " WHERE MONTH(h.TransDate)='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Month) & "' AND YEAR(h.TransDate)='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Year) & "'" &
        '                " AND cr.CompanyName='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompanyName) & "' AND h.Status =8 AND CR.State='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, State) & "' order by h.transdate desc"
        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With
        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        '    EndSQLControl()
        'End Function

        'Public Overloads Function GetViewConsignmentDtl(ByVal CompanyName As String, ByVal Month As String, ByVal Year As String, ByVal State As String) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo
        '            StartSQLControl()
        '            strSQL = "select ROW_NUMBER() OVER (order by h.TransID DESC) as No, h.TransID AS TransID, Qty AS Qty, cr.companyname AS ReceiverName, cg.companyname AS GeneratorName, h.transdate AS TransDate " &
        '                " from Consignhdr h inner join consigndtl d on h.transid=d.transid " &
        '                " LEFT JOIN BIZENTITY CG WITH (NOLOCK) ON CG.BizRegID=h.GeneratorID " &
        '                " LEFT JOIN BIZENTITY CR WITH (NOLOCK) ON CR.BizRegID=h.ReceiverID " &
        '                " WHERE MONTH(h.TransDate)='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Month) & "' AND YEAR(h.TransDate)='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Year) & "'" &
        '                " AND cg.CompanyName='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompanyName) & "' AND h.Status > 0 AND h.Status not in (9,11,13,14,15) AND CG.State='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, State) & "' order by h.transdate desc"
        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With
        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        '    EndSQLControl()
        'End Function

        'Public Overloads Function GetViewConsignment(ByVal State As String, ByVal Month As String, ByVal Year As String) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo
        '            StartSQLControl()

        '            strSQL = "select TOP 50 ROW_NUMBER() OVER (order by SUM(Qty) DESC) as No, SUM(Qty) AS Total, COUNT(CH.ContractNo) " &
        '                " AS TotalCN, CG.CompanyName AS GeneratorName, CASE WHEN PBTDesc is null THEN ISNULL(StateDesc,'') ELSE " &
        '                " ISNULL(StateDesc,'') + ' | ' + ISNULL(PBTDesc,'') END AS StateDesc, AccNo AS DOENO, MAX(CH2.TRANSDATE) as LastDate, " &
        '                " (select top 1 b.CompanyName from CONSIGNHDR c, bizentity b where c.receiverid=b.bizregid and c.GeneratorID=CH.GeneratorID " &
        '                " AND c.TransDate=MAX(CH2.TRANSDATE) and c.Status > 0 AND c.Status not in (9,11,13,14,15) order by c.SyncCreate desc) AS ReceiverName, CG.CompanyName AS CompanyName " &
        '                " FROM ConsignHDR CH WITH (NOLOCK) INNER JOIN CONSIGNHDR CH2 WITH (NOLOCK) ON CH2.TransID=CH.TransID " &
        '                " INNER JOIN CONSIGNDTL WITH (NOLOCK) ON CONSIGNDTL.TransID=CH.TransID LEFT JOIN BIZENTITY CG WITH (NOLOCK) " &
        '                " ON CG.BizRegID=CH.GeneratorID   LEFT JOIN BIZLOCATE BZ WITH (NOLOCK) ON BZ.BizLocID=CH.GeneratorLocID " &
        '                "LEFT JOIN BIZENTITY CR WITH (NOLOCK) ON CR.BizRegID=CH.ReceiverID " &
        '                " LEFT JOIN STATE WITH (NOLOCK) ON CG.State=STATE.StateCode " &
        '                " LEFT JOIN PBT WITH (NOLOCK) ON CG.PBT=PBT.PBTCode " &
        '                " WHERE MONTH(CH.TransDate)='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Month) & "'" &
        '                " AND YEAR(CH.TransDate)='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Year) & "' AND CH.Status > 0 AND CH.Status not in (9,11,13,14,15) AND CG.State='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, State) & "'" &
        '                " GROUP BY CG.CompanyName,StateDesc,PBTDesc, AccNo,CH.GENERATORID ORDER BY SUM(Qty) DESC"

        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)


        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        '    EndSQLControl()
        'End Function

        'Public Sub UpdateEmailNotifWR(ByVal TransID As String, ByVal Inuse As Integer)
        '    Dim dt As DataTable = Nothing
        '    'Dim strCond As String = String.Empty
        '    Dim strSQL As String

        '    strSQL = "UPDATE CONSIGNHDR WITH (ROWLOCK) SET inUse = " & Inuse & " , SyncLastUpd =GETDATE() WHERE TransID= '" & TransID & "'"
        '    dt = objConn.Execute(strSQL, CommandType.Text)
        'End Sub

        'Public Overloads Function GetPremiseConsign(ByVal ReceiverLocID As String, ByVal WasteCode As String, ByVal WasteDesc As String, ByVal GeneratorLocID As String) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo
        '            StartSQLControl()
        '            strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY B.BRANCHNAME ASC) AS Bil, MONTH(C.TransDate) AS MONTH, C.GeneratorID,  " &
        '                " C.GeneratorLocID, D.WasteDescription,B.BranchName, SUM(D.Qty) AS Qty FROM CONSIGNHDR C WITH (NOLOCK) INNER  " &
        '                " JOIN CONSIGNDTL D WITH (NOLOCK) ON C.TransID=D.TransID INNER JOIN BIZLOCATE B WITH (NOLOCK) ON C.ReceiverID=B.BizRegID  " &
        '                " AND  ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "' " &
        '                " AND D.WasteCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "' AND " &
        '                " D.WasteDescription='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteDesc) & "' AND " &
        '                " GeneratorLocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, GeneratorLocID) & "' GROUP BY  MONTH(C.TransDate), " &
        '                " C.GeneratorID, C.GeneratorLocID,  D.WasteDescription,B.BranchName"
        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        '    EndSQLControl()
        'End Function

        'Public Overloads Function GetClosingMonthQty(ByVal LocID As String, ByVal WasteCode As String, ByVal WasteName As String, ByVal Month As String, ByVal Year As String) As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo
        '            StartSQLControl()
        '            strSQL = "SELECT Closing FROM ITEMSUMMARY WITH (NOLOCK) " &
        '                " where ItemCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "'" &
        '                " AND LocId='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "'" &
        '                " AND ItemName='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteName) & "'" &
        '                " AND MthCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Month) & "'" &
        '                " AND YearCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Year) & "'"
        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        '    EndSQLControl()
        'End Function

        'Public Overloads Function GetCNApprovalList(ByVal State As String, ByVal CNNo As String, Optional fieldCond As String = "") As Data.DataTable
        '    If StartConnection() = True Then
        '        With ConsignhdrInfo.MyInfo
        '            StartSQLControl()
        '            Dim strFilter As String = ""
        '            If State <> "" Then
        '                strFilter = "  BL.State='" & State & "' "
        '            Else
        '                strFilter = "  CH.TransID='" & CNNo & "' "
        '            End If
        '            strSQL = "SELECT CH.TransID, ReceiverLocID,WasteCode, WasteType, RcvQty, OperationType, BranchName, CM.CodeDesc as WasteTypeDesc, CT.CodeDesc as WasteHandlingDesc, ReceiveDate, ReceiveDate, Qty, RcvPackQty, TransDate, SerialNo   " &
        '                    " FROM CONSIGNHDR CH With(NOLOCK) " &
        '                    " INNER JOIN CONSIGNDTL CD WITH(NOLOCK) ON CH.TransID=CD.TransID " &
        '                    " INNER JOIN BIZLOCATE BL WITH(NOLOCK) ON CH.ReceiverLocID=BL.BizLocID " &
        '                    " INNER JOIN CODEMASTER CM WITH(NOLOCK) ON CM.CODE=CD.WasteType AND CM.Codetype='WTY' " &
        '                    " INNER JOIN CODEMASTER CT WITH(NOLOCK) ON CT.CODE=CD.OperationType And CT.Codetype='WTH' " &
        '                    " WHERE " & strFilter & fieldCond
        '            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
        '        End With

        '    Else
        '        Return Nothing
        '    End If
        '    EndConnection()
        '    EndSQLControl()
        'End Function

#End Region
    End Class


    Namespace Container
#Region "Class Container"
        Public Class Consignhdr
            Public fTransID As System.String = "TransID"
            Public fContractNo As System.String = "ContractNo"
            Public fReferID As System.String = "ReferID"
            Public fTransType As System.String = "TransType"
            Public fTransDate As System.String = "TransDate"
            Public fStatus As System.String = "Status"
            Public fTransportStatus As System.String = "TransportStatus"
            Public fReceiveStatus As System.String = "ReceiveStatus"
            Public fGeneratorID As System.String = "GeneratorID"
            Public fGeneratorLocID As System.String = "GeneratorLocID"
            Public fGeneratorAddress As System.String = "GeneratorAddress"
            Public fGeneratorPIC As System.String = "GeneratorPIC"
            Public fGeneratorPos As System.String = "GeneratorPos"
            Public fGeneratorTelNo As System.String = "GeneratorTelNo"
            Public fGeneratorFaxNo As System.String = "GeneratorFaxNo"
            Public fGeneratorEmailAddress As System.String = "GeneratorEmailAddress"
            Public fTransporterID As System.String = "TransporterID"
            Public fTransporterLocID As System.String = "TransporterLocID"
            Public fTransporterAddress As System.String = "TransporterAddress"
            Public fTransporterPIC As System.String = "TransporterPIC"
            Public fTransporterPos As System.String = "TransporterPos"
            Public fTransporterTelNo As System.String = "TransporterTelNo"
            Public fTransporterFaxNo As System.String = "TransporterFaxNo"
            Public fTransporterEmailAddress As System.String = "TransporterEmailAddress"
            Public fVehicleID As System.String = "VehicleID"
            Public fDriverID As System.String = "DriverID"
            Public fDriverName As System.String = "DriverName"
            Public fICNo As System.String = "ICNo"
            Public fReceiverID As System.String = "ReceiverID"
            Public fReceiverLocID As System.String = "ReceiverLocID"
            Public fReceiverAddress As System.String = "ReceiverAddress"
            Public fReceiverPIC As System.String = "ReceiverPIC"
            Public fReceiverPos As System.String = "ReceiverPos"
            Public fReceiverTelNo As System.String = "ReceiverTelNo"
            Public fReceiverFaxNo As System.String = "ReceiverFaxNo"
            Public fReceiverEmailAddress As System.String = "ReceiverEmailAddress"
            Public fPremiseID As System.String = "PremiseID"
            Public fTempStorage1 As System.String = "TempStorage1"
            Public fTempStorage2 As System.String = "TempStorage2"
            Public fDeliveryDate As System.String = "DeliveryDate"
            Public fTargetTransportDate As System.String = "TargetTransportDate"
            Public fTransportDate As System.String = "TransportDate"
            Public fTargetReceiveDate As System.String = "TargetReceiveDate"
            Public fReceiveDate As System.String = "ReceiveDate"
            Public fDeliveryRemark As System.String = "DeliveryRemark"
            Public fReceiveRemark As System.String = "ReceiveRemark"
            Public fRejectRemark As System.String = "RejectRemark"
            Public fCreateDate As System.String = "CreateDate"
            Public fCreateBy As System.String = "CreateBy"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fUpdateBy As System.String = "UpdateBy"
            Public frowguid As System.String = "rowguid"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fIsHost As System.String = "IsHost"
            Public fIsConfirm As System.String = "IsConfirm"
            Public fIsNew As System.String = "IsNew"
            Public fLastSyncBy As System.String = "LastSyncBy"
            Public fActive As System.String = "Active"
            Public fFlag As System.String = "Flag"
            Public fBil As System.String = "Bil"
            Public fLiveCal As System.String = "LiveCal"

            Protected _TransID As System.String
            Protected _ContractNo As System.String
            Private _ReferID As System.String
            Private _TransType As System.String
            Private _TransDate As System.DateTime
            Private _Status As System.Byte
            Private _StatusDesc As System.String
            Private _TransportStatus As System.Byte
            Private _ReceiveStatus As System.Byte
            Private _GeneratorID As System.String
            Private _GeneratorLocID As System.String
            Private _GeneratorAddress As System.String
            Private _GeneratorPIC As System.String
            Private _GeneratorPos As System.String
            Private _GeneratorTelNo As System.String
            Private _GeneratorFaxNo As System.String
            Private _GeneratorEmailAddress As System.String
            Private _TransporterID As System.String
            Private _TransporterLocID As System.String
            Private _TransporterAddress As System.String
            Private _TransporterPIC As System.String
            Private _TransporterPos As System.String
            Private _TransporterTelNo As System.String
            Private _TransporterFaxNo As System.String
            Private _TransporterEmailAddress As System.String
            Private _VehicleID As System.String
            Private _DriverID As System.String
            Private _DriverName As System.String
            Private _ICNo As System.String
            Private _ReceiverID As System.String
            Private _ReceiverLocID As System.String
            Private _ReceiverAddress As System.String
            Private _ReceiverPIC As System.String
            Private _ReceiverPos As System.String
            Private _ReceiverPosDesc As System.String
            Private _GeneratorPosDesc As System.String
            Private _TransporterPosDesc As System.String
            Private _ReceiverTelNo As System.String
            Private _ReceiverFaxNo As System.String
            Private _ReceiverEmailAddress As System.String
            Private _PremiseID As System.String
            Private _TempStorage1 As System.String
            Private _TempStorage2 As System.String
            Private _DeliveryDate As System.DateTime
            Private _TargetTransportDate As System.DateTime
            Private _TransportDate As System.DateTime
            Private _TargetReceiveDate As System.DateTime
            Private _ReceiveDate As System.DateTime
            Private _DeliveryRemark As System.String
            Private _ReceiveRemark As System.String
            Private _RejectRemark As System.String
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _IsHost As System.Byte
            Private _IsConfirm As System.Byte
            Private _IsNew As System.Byte
            Private _LastSyncBy As System.String
            Private _Active As System.Byte
            Private _Flag As System.Byte
            Private _LiveCal As System.Byte
            Private _CancelReason As System.String
            Private _SubmissionDate As System.DateTime

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property CancelReason As System.String
                Get
                    Return _CancelReason
                End Get
                Set(ByVal Value As System.String)
                    _CancelReason = Value
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

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property TransID As System.String
                Get
                    Return _TransID
                End Get
                Set(ByVal Value As System.String)
                    _TransID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ContractNo As System.String
                Get
                    Return _ContractNo
                End Get
                Set(ByVal Value As System.String)
                    _ContractNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransType As System.String
                Get
                    Return _TransType
                End Get
                Set(ByVal Value As System.String)
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
            Public Property StatusDesc As System.String
                Get
                    Return _StatusDesc
                End Get
                Set(ByVal Value As System.String)
                    _StatusDesc = Value
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
            Public Property TransportStatus As System.Byte
                Get
                    Return _TransportStatus
                End Get
                Set(ByVal Value As System.Byte)
                    _TransportStatus = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ReceiveStatus As System.Byte
                Get
                    Return _ReceiveStatus
                End Get
                Set(ByVal Value As System.Byte)
                    _ReceiveStatus = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
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
            ''' Mandatory, Not Allow Null
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
            Public Property GeneratorAddress As System.String
                Get
                    Return _GeneratorAddress
                End Get
                Set(ByVal Value As System.String)
                    _GeneratorAddress = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property GeneratorPIC As System.String
                Get
                    Return _GeneratorPIC
                End Get
                Set(ByVal Value As System.String)
                    _GeneratorPIC = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property GeneratorPos As System.String
                Get
                    Return _GeneratorPos
                End Get
                Set(ByVal Value As System.String)
                    _GeneratorPos = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property GeneratorTelNo As System.String
                Get
                    Return _GeneratorTelNo
                End Get
                Set(ByVal Value As System.String)
                    _GeneratorTelNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property GeneratorFaxNo As System.String
                Get
                    Return _GeneratorFaxNo
                End Get
                Set(ByVal Value As System.String)
                    _GeneratorFaxNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property GeneratorEmailAddress As System.String
                Get
                    Return _GeneratorEmailAddress
                End Get
                Set(ByVal Value As System.String)
                    _GeneratorEmailAddress = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransporterID As System.String
                Get
                    Return _TransporterID
                End Get
                Set(ByVal Value As System.String)
                    _TransporterID = Value
                End Set
            End Property

            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransporterLocID As System.String
                Get
                    Return _TransporterLocID
                End Get
                Set(ByVal Value As System.String)
                    _TransporterLocID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransporterAddress As System.String
                Get
                    Return _TransporterAddress
                End Get
                Set(ByVal Value As System.String)
                    _TransporterAddress = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransporterPIC As System.String
                Get
                    Return _TransporterPIC
                End Get
                Set(ByVal Value As System.String)
                    _TransporterPIC = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransporterPos As System.String
                Get
                    Return _TransporterPos
                End Get
                Set(ByVal Value As System.String)
                    _TransporterPos = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransporterTelNo As System.String
                Get
                    Return _TransporterTelNo
                End Get
                Set(ByVal Value As System.String)
                    _TransporterTelNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransporterFaxNo As System.String
                Get
                    Return _TransporterFaxNo
                End Get
                Set(ByVal Value As System.String)
                    _TransporterFaxNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransporterEmailAddress As System.String
                Get
                    Return _TransporterEmailAddress
                End Get
                Set(ByVal Value As System.String)
                    _TransporterEmailAddress = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property VehicleID As System.String
                Get
                    Return _VehicleID
                End Get
                Set(ByVal Value As System.String)
                    _VehicleID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property DriverID As System.String
                Get
                    Return _DriverID
                End Get
                Set(ByVal Value As System.String)
                    _DriverID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property DriverName As System.String
                Get
                    Return _DriverName
                End Get
                Set(ByVal Value As System.String)
                    _DriverName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ICNo As System.String
                Get
                    Return _ICNo
                End Get
                Set(ByVal Value As System.String)
                    _ICNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
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
            ''' Mandatory, Not Allow Null
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
            Public Property ReceiverAddress As System.String
                Get
                    Return _ReceiverAddress
                End Get
                Set(ByVal Value As System.String)
                    _ReceiverAddress = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ReceiverPIC As System.String
                Get
                    Return _ReceiverPIC
                End Get
                Set(ByVal Value As System.String)
                    _ReceiverPIC = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ReceiverPos As System.String
                Get
                    Return _ReceiverPos
                End Get
                Set(ByVal Value As System.String)
                    _ReceiverPos = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ReceiverPosDesc As System.String
                Get
                    Return _ReceiverPosDesc
                End Get
                Set(ByVal Value As System.String)
                    _ReceiverPosDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransporterPosDesc As System.String
                Get
                    Return _TransporterPosDesc
                End Get
                Set(ByVal Value As System.String)
                    _TransporterPosDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property GeneratorPosDesc As System.String
                Get
                    Return _GeneratorPosDesc
                End Get
                Set(ByVal Value As System.String)
                    _GeneratorPosDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ReceiverTelNo As System.String
                Get
                    Return _ReceiverTelNo
                End Get
                Set(ByVal Value As System.String)
                    _ReceiverTelNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ReceiverFaxNo As System.String
                Get
                    Return _ReceiverFaxNo
                End Get
                Set(ByVal Value As System.String)
                    _ReceiverFaxNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ReceiverEmailAddress As System.String
                Get
                    Return _ReceiverEmailAddress
                End Get
                Set(ByVal Value As System.String)
                    _ReceiverEmailAddress = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property PremiseID As System.String
                Get
                    Return _PremiseID
                End Get
                Set(ByVal Value As System.String)
                    _PremiseID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TempStorage1 As System.String
                Get
                    Return _TempStorage1
                End Get
                Set(ByVal Value As System.String)
                    _TempStorage1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TempStorage2 As System.String
                Get
                    Return _TempStorage2
                End Get
                Set(ByVal Value As System.String)
                    _TempStorage2 = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property DeliveryDate As System.DateTime
                Get
                    Return _DeliveryDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _DeliveryDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property TargetTransportDate As System.DateTime
                Get
                    Return _TargetTransportDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _TargetTransportDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property TransportDate As System.DateTime
                Get
                    Return _TransportDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _TransportDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property SubmissionDate As System.DateTime
                Get
                    Return _SubmissionDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _SubmissionDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property TargetReceiveDate As System.DateTime
                Get
                    Return _TargetReceiveDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _TargetReceiveDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
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
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property DeliveryRemark As System.String
                Get
                    Return _DeliveryRemark
                End Get
                Set(ByVal Value As System.String)
                    _DeliveryRemark = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ReceiveRemark As System.String
                Get
                    Return _ReceiveRemark
                End Get
                Set(ByVal Value As System.String)
                    _ReceiveRemark = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RejectRemark As System.String
                Get
                    Return _RejectRemark
                End Get
                Set(ByVal Value As System.String)
                    _RejectRemark = Value
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
            Public Property IsConfirm As System.Byte
                Get
                    Return _IsConfirm
                End Get
                Set(ByVal Value As System.Byte)
                    _IsConfirm = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsNew As System.Byte
                Get
                    Return _IsNew
                End Get
                Set(ByVal Value As System.Byte)
                    _IsNew = Value
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
            Public Property ReferID As System.String
                Get
                    Return _ReferID
                End Get
                Set(ByVal Value As System.String)
                    _ReferID = Value
                End Set
            End Property

            'Custom field
            Private _CompanyName As System.String
            Private _GeneratorName As System.String
            Private _GeneratorMobile As System.String
            Private _TransporterName As System.String
            Private _TransporterMobile As System.String
            Private _ReceiverName As System.String
            Private _ReceiverMobile As System.String
            Private _VehicleNo As System.String

            Public Property CompanyName As System.String
                Get
                    Return _CompanyName
                End Get
                Set(ByVal Value As System.String)
                    _CompanyName = Value
                End Set
            End Property

            Public Property GeneratorName As System.String
                Get
                    Return _GeneratorName
                End Get
                Set(ByVal Value As System.String)
                    _GeneratorName = Value
                End Set
            End Property

            Public Property GeneratorMobile As System.String
                Get
                    Return _GeneratorMobile
                End Get
                Set(ByVal Value As System.String)
                    _GeneratorMobile = Value
                End Set
            End Property

            Public Property TransporterName As System.String
                Get
                    Return _TransporterName
                End Get
                Set(ByVal Value As System.String)
                    _TransporterName = Value
                End Set
            End Property

            Public Property TransporterMobile As System.String
                Get
                    Return _TransporterMobile
                End Get
                Set(ByVal Value As System.String)
                    _TransporterMobile = Value
                End Set
            End Property

            Public Property ReceiverName As System.String
                Get
                    Return _ReceiverName
                End Get
                Set(ByVal Value As System.String)
                    _ReceiverName = Value
                End Set
            End Property

            Public Property ReceiverMobile As System.String
                Get
                    Return _ReceiverMobile
                End Get
                Set(ByVal Value As System.String)
                    _ReceiverMobile = Value
                End Set
            End Property

            Public Property VehicleNo As System.String
                Get
                    Return _VehicleNo
                End Get
                Set(ByVal Value As System.String)
                    _VehicleNo = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    'Public Class ConsignhdrInfo
    '    Inherits Core.CoreBase
    '    Protected Overrides Sub InitializeClassInfo()
    '        With MyInfo
    '            .FieldsList = "TransID,ContractNo,ReferID,TransType,TransDate,Status,Flag,Active,TransportStatus,ReceiveStatus,GeneratorID,GeneratorLocID,GeneratorAddress,GeneratorPIC,GeneratorPos,GeneratorTelNo,GeneratorFaxNo,GeneratorEmailAddress,TransporterID,TransporterLocID,TransporterAddress,TransporterPIC,TransporterPos,TransporterTelNo,TransporterFaxNo,TransporterEmailAddress,VehicleID,DriverID,DriverName,ICNo,ReceiverID,ReceiverLocID,ReceiverAddress,ReceiverPIC,ReceiverPos,ReceiverTelNo,ReceiverFaxNo,ReceiverEmailAddress,PremiseID,TempStorage1,TempStorage2,DeliveryDate,TargetTransportDate,TransportDate,TargetReceiveDate,ReceiveDate,DeliveryRemark,ReceiveRemark,RejectRemark,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid,SyncCreate,SyncLastUpd,IsHost,IsConfirm,IsNew,LastSyncBy ,LiveCal,CancelReason, SubmissionDate"
    '            .CheckFields = "TransType,Status,ReceiveStatus,IsHost,IsConfirm,IsNew,Flag,ReceiveRemark"
    '            .TableName = "Consignhdr WITH (NOLOCK)"
    '            .DefaultCond = Nothing
    '            .DefaultOrder = Nothing
    '            .Listing = "TransID,ContractNo,ReferID,TransType,TransDate,Status,Flag,Active,TransportStatus,ReceiveStatus,GeneratorID,GeneratorLocID,GeneratorAddress,GeneratorPIC,GeneratorPos,GeneratorTelNo,GeneratorFaxNo,GeneratorEmailAddress,TransporterID,TransporterLocID,TransporterAddress,TransporterPIC,TransporterPos,TransporterTelNo,TransporterFaxNo,TransporterEmailAddress,VehicleID,DriverID,DriverName,ICNo,ReceiverID,ReceiverLocID,ReceiverAddress,ReceiverPIC,ReceiverPos,ReceiverTelNo,ReceiverFaxNo,ReceiverEmailAddress,PremiseID,TempStorage1,TempStorage2,DeliveryDate,TargetTransportDate,TransportDate,TargetReceiveDate,ReceiveDate,DeliveryRemark,ReceiveRemark,RejectRemark,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid,SyncCreate,SyncLastUpd,IsHost,IsConfirm,IsNew,LastSyncBy ,LiveCal,CancelReason, SubmissionDate"
    '            .ListingCond = Nothing
    '            .ShortList = Nothing
    '            .ShortListCond = Nothing
    '        End With
    '    End Sub

    '    Public Function JoinTableField(ByVal Prefix As String, ByVal FieldList As String) As String
    '        Dim aFieldList As Array
    '        Dim strFieldList As String = Nothing
    '        aFieldList = FieldList.Split(",")
    '        If Not aFieldList Is Nothing Then
    '            For Each Str As String In aFieldList
    '                If strFieldList Is Nothing Then
    '                    strFieldList = Prefix & "." & Str
    '                Else
    '                    strFieldList &= "," & Prefix & "." & Str
    '                End If
    '            Next
    '        End If
    '        aFieldList = Nothing

    '        Return strFieldList
    '    End Function
    'End Class
#End Region

#Region "Scheme"
    'Public Class ConsignHDRScheme
    '    Inherits Core.SchemeBase
    '    Protected Overrides Sub InitializeInfo()

    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "TransID"
    '            .Length = 20
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(0, this)

    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "ContractNo"
    '            .Length = 20
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(1, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "TransType"
    '            .Length = 20
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(2, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtDateTime
    '            .FieldName = "TransDate"
    '            .Length = 8
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(3, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "Status"
    '            .Length = 1
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(4, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "ReceiveStatus"
    '            .Length = 1
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(5, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "GeneratorID"
    '            .Length = 20
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(6, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "GeneratorAddress"
    '            .Length = 40
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(7, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtStringN
    '            .FieldName = "GeneratorPIC"
    '            .Length = 50
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(8, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtStringN
    '            .FieldName = "GeneratorPos"
    '            .Length = 50
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(9, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "GeneratorTelNo"
    '            .Length = 16
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(10, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "GeneratorFaxNo"
    '            .Length = 16
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(11, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtStringN
    '            .FieldName = "GeneratorEmailAddress"
    '            .Length = 80
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(12, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "TransporterID"
    '            .Length = 20
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(13, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "TransporterAddress"
    '            .Length = 255
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(14, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtStringN
    '            .FieldName = "TransporterPIC"
    '            .Length = 50
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(15, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtStringN
    '            .FieldName = "TransporterPos"
    '            .Length = 50
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(16, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "TransporterTelNo"
    '            .Length = 16
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(17, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "TransporterFaxNo"
    '            .Length = 16
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(18, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtStringN
    '            .FieldName = "TransporterEmailAddress"
    '            .Length = 80
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(19, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "VehicleID"
    '            .Length = 20
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(20, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "DriverID"
    '            .Length = 20
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(21, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtStringN
    '            .FieldName = "DriverName"
    '            .Length = 50
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(22, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtStringN
    '            .FieldName = "ICNo"
    '            .Length = 50
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(23, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "ReceiverID"
    '            .Length = 20
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(24, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "ReceiverAddress"
    '            .Length = 255
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(25, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtStringN
    '            .FieldName = "ReceiverPIC"
    '            .Length = 50
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(26, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtStringN
    '            .FieldName = "ReceiverPos"
    '            .Length = 50
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(27, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "ReceiverTelNo"
    '            .Length = 16
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(28, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "ReceiverFaxNo"
    '            .Length = 16
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(29, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtStringN
    '            .FieldName = "ReceiverEmailAddress"
    '            .Length = 80
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(30, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "PremiseID"
    '            .Length = 10
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = False
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(31, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtStringN
    '            .FieldName = "TempStorage1"
    '            .Length = 50
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(32, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtStringN
    '            .FieldName = "TempStorage2"
    '            .Length = 50
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(33, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtDateTime
    '            .FieldName = "DeliveryDate"
    '            .Length = 8
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = False
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(34, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtDateTime
    '            .FieldName = "TargetTransportDate"
    '            .Length = 8
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = False
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(35, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtDateTime
    '            .FieldName = "TransportDate"
    '            .Length = 8
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = False
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(36, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtDateTime
    '            .FieldName = "TargetReceiveDate"
    '            .Length = 8
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = False
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(37, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtDateTime
    '            .FieldName = "ReceiveDate"
    '            .Length = 8
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = False
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(38, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtStringN
    '            .FieldName = "DeliveryRemark"
    '            .Length = 50
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(39, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtStringN
    '            .FieldName = "ReceiveRemark"
    '            .Length = 50
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(40, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtDateTime
    '            .FieldName = "CreateDate"
    '            .Length = 8
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = False
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(41, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "CreateBy"
    '            .Length = 20
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(42, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtDateTime
    '            .FieldName = "LastUpdate"
    '            .Length = 8
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = False
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(43, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "UpdateBy"
    '            .Length = 20
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(44, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "rowguid"
    '            .Length = 16
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(45, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtDateTime
    '            .FieldName = "SyncCreate"
    '            .Length = 8
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(46, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtDateTime
    '            .FieldName = "SyncLastUpd"
    '            .Length = 8
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(47, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "IsHost"
    '            .Length = 1
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(48, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "IsConfirm"
    '            .Length = 1
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(49, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "IsNew"
    '            .Length = 1
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(50, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "LastSyncBy"
    '            .Length = 20
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(51, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "TransportStatus"
    '            .Length = 1
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(53, this)

    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "Flag"
    '            .Length = 1
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(54, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "Active"
    '            .Length = 1
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(55, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "GeneratorLocID"
    '            .Length = 20
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(66, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "ReceiverLocID"
    '            .Length = 20
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(57, this)
    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "TransporterLocID"
    '            .Length = 20
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = True
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(58, this)

    '        With this
    '            .DataType = SQLControl.EnumDataType.dtString
    '            .FieldName = "ReferID"
    '            .Length = 20
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = False
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(59, this)

    '        With this
    '            .DataType = SQLControl.EnumDataType.dtNumeric
    '            .FieldName = "LiveCal"
    '            .Length = 1
    '            .DecPlace = Nothing
    '            .RegExp = String.Empty
    '            .IsMandatory = False
    '            .AllowNegative = False
    '        End With
    '        MyBase.AddItem(60, this)
    '    End Sub


    '    Public ReadOnly Property TransID As StrucElement
    '        Get
    '            Return MyBase.GetItem(0)
    '        End Get
    '    End Property
    '    Public ReadOnly Property ContractNo As StrucElement
    '        Get
    '            Return MyBase.GetItem(1)
    '        End Get
    '    End Property

    '    Public ReadOnly Property TransType As StrucElement
    '        Get
    '            Return MyBase.GetItem(2)
    '        End Get
    '    End Property
    '    Public ReadOnly Property TransDate As StrucElement
    '        Get
    '            Return MyBase.GetItem(3)
    '        End Get
    '    End Property
    '    Public ReadOnly Property Status As StrucElement
    '        Get
    '            Return MyBase.GetItem(4)
    '        End Get
    '    End Property
    '    Public ReadOnly Property ReceiveStatus As StrucElement
    '        Get
    '            Return MyBase.GetItem(5)
    '        End Get
    '    End Property
    '    Public ReadOnly Property GeneratorID As StrucElement
    '        Get
    '            Return MyBase.GetItem(6)
    '        End Get
    '    End Property
    '    Public ReadOnly Property GeneratorAddress As StrucElement
    '        Get
    '            Return MyBase.GetItem(7)
    '        End Get
    '    End Property
    '    Public ReadOnly Property GeneratorPIC As StrucElement
    '        Get
    '            Return MyBase.GetItem(8)
    '        End Get
    '    End Property
    '    Public ReadOnly Property GeneratorPos As StrucElement
    '        Get
    '            Return MyBase.GetItem(9)
    '        End Get
    '    End Property
    '    Public ReadOnly Property GeneratorTelNo As StrucElement
    '        Get
    '            Return MyBase.GetItem(10)
    '        End Get
    '    End Property
    '    Public ReadOnly Property GeneratorFaxNo As StrucElement
    '        Get
    '            Return MyBase.GetItem(11)
    '        End Get
    '    End Property
    '    Public ReadOnly Property GeneratorEmailAddress As StrucElement
    '        Get
    '            Return MyBase.GetItem(12)
    '        End Get
    '    End Property
    '    Public ReadOnly Property TransporterID As StrucElement
    '        Get
    '            Return MyBase.GetItem(13)
    '        End Get
    '    End Property
    '    Public ReadOnly Property TransporterAddress As StrucElement
    '        Get
    '            Return MyBase.GetItem(14)
    '        End Get
    '    End Property
    '    Public ReadOnly Property TransporterPIC As StrucElement
    '        Get
    '            Return MyBase.GetItem(15)
    '        End Get
    '    End Property
    '    Public ReadOnly Property TransporterPos As StrucElement
    '        Get
    '            Return MyBase.GetItem(16)
    '        End Get
    '    End Property
    '    Public ReadOnly Property TransporterTelNo As StrucElement
    '        Get
    '            Return MyBase.GetItem(17)
    '        End Get
    '    End Property
    '    Public ReadOnly Property TransporterFaxNo As StrucElement
    '        Get
    '            Return MyBase.GetItem(18)
    '        End Get
    '    End Property
    '    Public ReadOnly Property TransporterEmailAddress As StrucElement
    '        Get
    '            Return MyBase.GetItem(19)
    '        End Get
    '    End Property
    '    Public ReadOnly Property VehicleID As StrucElement
    '        Get
    '            Return MyBase.GetItem(20)
    '        End Get
    '    End Property
    '    Public ReadOnly Property DriverID As StrucElement
    '        Get
    '            Return MyBase.GetItem(21)
    '        End Get
    '    End Property
    '    Public ReadOnly Property DriverName As StrucElement
    '        Get
    '            Return MyBase.GetItem(22)
    '        End Get
    '    End Property
    '    Public ReadOnly Property ICNo As StrucElement
    '        Get
    '            Return MyBase.GetItem(23)
    '        End Get
    '    End Property
    '    Public ReadOnly Property ReceiverID As StrucElement
    '        Get
    '            Return MyBase.GetItem(24)
    '        End Get
    '    End Property
    '    Public ReadOnly Property ReceiverAddress As StrucElement
    '        Get
    '            Return MyBase.GetItem(25)
    '        End Get
    '    End Property
    '    Public ReadOnly Property ReceiverPIC As StrucElement
    '        Get
    '            Return MyBase.GetItem(26)
    '        End Get
    '    End Property
    '    Public ReadOnly Property ReceiverPos As StrucElement
    '        Get
    '            Return MyBase.GetItem(27)
    '        End Get
    '    End Property
    '    Public ReadOnly Property ReceiverTelNo As StrucElement
    '        Get
    '            Return MyBase.GetItem(28)
    '        End Get
    '    End Property
    '    Public ReadOnly Property ReceiverFaxNo As StrucElement
    '        Get
    '            Return MyBase.GetItem(29)
    '        End Get
    '    End Property
    '    Public ReadOnly Property ReceiverEmailAddress As StrucElement
    '        Get
    '            Return MyBase.GetItem(30)
    '        End Get
    '    End Property
    '    Public ReadOnly Property PremiseID As StrucElement
    '        Get
    '            Return MyBase.GetItem(31)
    '        End Get
    '    End Property
    '    Public ReadOnly Property TempStorage1 As StrucElement
    '        Get
    '            Return MyBase.GetItem(32)
    '        End Get
    '    End Property
    '    Public ReadOnly Property TempStorage2 As StrucElement
    '        Get
    '            Return MyBase.GetItem(33)
    '        End Get
    '    End Property
    '    Public ReadOnly Property DeliveryDate As StrucElement
    '        Get
    '            Return MyBase.GetItem(34)
    '        End Get
    '    End Property
    '    Public ReadOnly Property TargetTransportDate As StrucElement
    '        Get
    '            Return MyBase.GetItem(35)
    '        End Get
    '    End Property
    '    Public ReadOnly Property TransportDate As StrucElement
    '        Get
    '            Return MyBase.GetItem(36)
    '        End Get
    '    End Property
    '    Public ReadOnly Property TargetReceiveDate As StrucElement
    '        Get
    '            Return MyBase.GetItem(37)
    '        End Get
    '    End Property
    '    Public ReadOnly Property ReceiveDate As StrucElement
    '        Get
    '            Return MyBase.GetItem(38)
    '        End Get
    '    End Property
    '    Public ReadOnly Property DeliveryRemark As StrucElement
    '        Get
    '            Return MyBase.GetItem(39)
    '        End Get
    '    End Property
    '    Public ReadOnly Property ReceiveRemark As StrucElement
    '        Get
    '            Return MyBase.GetItem(40)
    '        End Get
    '    End Property
    '    Public ReadOnly Property CreateDate As StrucElement
    '        Get
    '            Return MyBase.GetItem(41)
    '        End Get
    '    End Property
    '    Public ReadOnly Property CreateBy As StrucElement
    '        Get
    '            Return MyBase.GetItem(42)
    '        End Get
    '    End Property
    '    Public ReadOnly Property LastUpdate As StrucElement
    '        Get
    '            Return MyBase.GetItem(43)
    '        End Get
    '    End Property
    '    Public ReadOnly Property UpdateBy As StrucElement
    '        Get
    '            Return MyBase.GetItem(44)
    '        End Get
    '    End Property
    '    Public ReadOnly Property rowguid As StrucElement
    '        Get
    '            Return MyBase.GetItem(45)
    '        End Get
    '    End Property
    '    Public ReadOnly Property SyncCreate As StrucElement
    '        Get
    '            Return MyBase.GetItem(46)
    '        End Get
    '    End Property
    '    Public ReadOnly Property SyncLastUpd As StrucElement
    '        Get
    '            Return MyBase.GetItem(47)
    '        End Get
    '    End Property
    '    Public ReadOnly Property IsHost As StrucElement
    '        Get
    '            Return MyBase.GetItem(48)
    '        End Get
    '    End Property
    '    Public ReadOnly Property IsConfirm As StrucElement
    '        Get
    '            Return MyBase.GetItem(49)
    '        End Get
    '    End Property
    '    Public ReadOnly Property IsNew As StrucElement
    '        Get
    '            Return MyBase.GetItem(50)
    '        End Get
    '    End Property
    '    Public ReadOnly Property LastSyncBy As StrucElement
    '        Get
    '            Return MyBase.GetItem(51)
    '        End Get
    '    End Property
    '    Public ReadOnly Property TransportStatus As StrucElement
    '        Get
    '            Return MyBase.GetItem(52)
    '        End Get
    '    End Property
    '    Public ReadOnly Property Flag As StrucElement
    '        Get
    '            Return MyBase.GetItem(53)
    '        End Get
    '    End Property
    '    Public ReadOnly Property Active As StrucElement
    '        Get
    '            Return MyBase.GetItem(54)
    '        End Get
    '    End Property
    '    Public ReadOnly Property GeneratorLocID As StrucElement
    '        Get
    '            Return MyBase.GetItem(55)
    '        End Get
    '    End Property
    '    Public ReadOnly Property ReceiverLocID As StrucElement
    '        Get
    '            Return MyBase.GetItem(56)
    '        End Get
    '    End Property
    '    Public ReadOnly Property TransporterLocID As StrucElement
    '        Get
    '            Return MyBase.GetItem(57)
    '        End Get
    '    End Property
    '    Public ReadOnly Property ReferID As StrucElement
    '        Get
    '            Return MyBase.GetItem(58)
    '        End Get
    '    End Property
    '    Public ReadOnly Property LiveCal As StrucElement
    '        Get
    '            Return MyBase.GetItem(59)
    '        End Get
    '    End Property
    '    Public Function GetElement(ByVal Key As Integer) As StrucElement
    '        Return MyBase.GetItem(Key)
    '    End Function


    'End Class
#End Region

End Namespace



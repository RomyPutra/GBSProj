
Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Actions
    Public NotInheritable Class ConsignDTL
        Inherits Core.CoreControl
        Private ConsigndtlInfo As ConsigndtlInfo = New ConsigndtlInfo
        Private Log As New SystemLog()

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub


#Region "Data Manipulation-Add,Edit,Del"

        'Add
        Public Function BatchInsert(ByVal ListCNdtlCont As List(Of Container.Consigndtl), ByRef message As String) As Boolean
            Return BatchSave(ListCNdtlCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'Add
        Public Function BatchUpdate(ByVal ListCNdtlCont As List(Of Container.Consigndtl), ByRef message As String) As Boolean
            Return BatchSave(ListCNdtlCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        ' Add BatchSave
        Private Function BatchSave(ByVal ListCNdtlCont As List(Of Container.Consigndtl), ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()

            BatchSave = False
            Try
                If ListCNdtlCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                    End If

                    For Each CNDtlCont In ListCNdtlCont
                        With objSQL
                            .TableName = "CONSIGNDTL WITH (ROWLOCK)"

                            .AddField("WasteCode", CNDtlCont.WasteCode, SQLControl.EnumDataType.dtString)
                            .AddField("WasteDescription", CNDtlCont.WasteDescription, SQLControl.EnumDataType.dtString)
                            .AddField("WasteComponent", CNDtlCont.WasteComponent, SQLControl.EnumDataType.dtString)
                            .AddField("WasteType", CNDtlCont.WasteType, SQLControl.EnumDataType.dtString)
                            .AddField("WastePackage", CNDtlCont.WastePackage, SQLControl.EnumDataType.dtString)
                            .AddField("OriginCode", CNDtlCont.OriginCode, SQLControl.EnumDataType.dtString)
                            .AddField("OriginDescription", CNDtlCont.OriginDescription, SQLControl.EnumDataType.dtString)
                            .AddField("SerialNo", CNDtlCont.SerialNo, SQLControl.EnumDataType.dtString)
                            .AddField("Qty", CNDtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("PackQty", CNDtlCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("StorageID", CNDtlCont.StorageID, SQLControl.EnumDataType.dtString)
                            .AddField("CountryCode", CNDtlCont.CountryCode, SQLControl.EnumDataType.dtString)
                            .AddField("PackagingQty", CNDtlCont.PackagingQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("PackagingQtyKg", CNDtlCont.PackagingQtyKg, SQLControl.EnumDataType.dtNumeric)
                            .AddField("RcvQty", CNDtlCont.RcvQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("RcvPackQty", CNDtlCont.RcvPackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("RecPackQty", CNDtlCont.RecPackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("TreatmentCost", CNDtlCont.TreatmentCost, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Remark", CNDtlCont.Remark, SQLControl.EnumDataType.dtString)
                            .AddField("OperationType", CNDtlCont.OperationType, SQLControl.EnumDataType.dtString)
                            .AddField("OperationDesc", CNDtlCont.OperationDesc, SQLControl.EnumDataType.dtString)

                            .AddField("CreateDate", CNDtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateBy", CNDtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                            .AddField("AuthorisedBy", CNDtlCont.AuthorisedBy, SQLControl.EnumDataType.dtString)
                            .AddField("AuthorisedDate", CNDtlCont.AuthorisedDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("LastUpdate", CNDtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("UpdateBy", CNDtlCont.UpdateBy, SQLControl.EnumDataType.dtString)

                            .AddField("ReceivedDate", CNDtlCont.ReceivedDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("ReceivedBy", CNDtlCont.ReceivedBy, SQLControl.EnumDataType.dtString)

                            Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    If blnFound = True And blnFlag = False Then
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CNDtlCont.ContractNo) & "' AND SeqNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, CNDtlCont.SeqNo) & "'")
                                    Else
                                        If blnFound = False Then
                                            .AddField("TransID", CNDtlCont.TransID, SQLControl.EnumDataType.dtString)
                                            .AddField("ContractNo", CNDtlCont.ContractNo, SQLControl.EnumDataType.dtString)
                                            .AddField("SeqNo", CNDtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                        End If
                                    End If
                                Case SQLControl.EnumSQLType.stUpdate
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CNDtlCont.ContractNo) & "' AND SeqNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, CNDtlCont.SeqNo) & "'")
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
                        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignDTL", axExecute.Message & sqlStatement, axExecute.StackTrace)
                        Return False

                    Finally
                        objSQL.Dispose()
                    End Try
                End If

            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignDTL", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignDTL", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ListCNdtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'Add
        Public Function UpdateStockConsignmentNote(ByVal ContractNo As String, ByRef message As String) As Boolean
            Dim strSQL As String = ""

            Try
                StartSQLControl()
                strSQL = "Exec sp_LiveInvDeductByCN '" & ContractNo & "'"

                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                Return True

            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignDTL", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignDTL", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally

                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function Save(ByVal ConsigndtlCont As Container.Consigndtl, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ConsigndtlCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ConsigndtlInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsigndtlCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsigndtlCont.ContractNo) & "' AND SeqNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, ConsigndtlCont.SeqNo) & "' AND StorageID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsigndtlCont.StorageID) & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        blnFlag = False
                                    Else
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
                                .TableName = "Consigndtl WITH (ROWLOCK)"
                                .AddField("ERPMATNo", ConsigndtlCont.ERPMATNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("SerialNo", ConsigndtlCont.SerialNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("WasteCode", ConsigndtlCont.WasteCode, SQLControl.EnumDataType.dtString)
                                .AddField("WasteDescription", ConsigndtlCont.WasteDescription, SQLControl.EnumDataType.dtStringN)
                                .AddField("WasteType", ConsigndtlCont.WasteType, SQLControl.EnumDataType.dtString)
                                .AddField("WastePackage", ConsigndtlCont.WastePackage, SQLControl.EnumDataType.dtString)
                                .AddField("OperationType", ConsigndtlCont.OperationType, SQLControl.EnumDataType.dtString)
                                .AddField("TransInit", ConsigndtlCont.TransInit, SQLControl.EnumDataType.dtString)
                                .AddField("TermID", ConsigndtlCont.TermID, SQLControl.EnumDataType.dtCustom)
                                .AddField("WasteComponent", ConsigndtlCont.WasteComponent, SQLControl.EnumDataType.dtStringN)
                                .AddField("OriginCode", ConsigndtlCont.OriginCode, SQLControl.EnumDataType.dtStringN)
                                .AddField("OriginDescription", ConsigndtlCont.OriginDescription, SQLControl.EnumDataType.dtStringN)
                                .AddField("TariffCode", ConsigndtlCont.TariffCode, SQLControl.EnumDataType.dtStringN)
                                .AddField("CountryCode", ConsigndtlCont.CountryCode, SQLControl.EnumDataType.dtString)
                                .AddField("PackagingQty", ConsigndtlCont.PackagingQty, SQLControl.EnumDataType.dtString)
                                .AddField("PackagingQtyKg", ConsigndtlCont.PackagingQtyKg, SQLControl.EnumDataType.dtString)
                                .AddField("PackagingWeight", ConsigndtlCont.PackagingWeight, SQLControl.EnumDataType.dtString)
                                .AddField("TreatmentCost", ConsigndtlCont.TreatmentCost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RegistedSupp", ConsigndtlCont.RegistedSupp, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Qty", ConsigndtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("PackQty", ConsigndtlCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RcvQty", ConsigndtlCont.RcvQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RcvPackQty", ConsigndtlCont.RcvPackQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("PalletQty", ConsigndtlCont.PalletQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RecPackQty", ConsigndtlCont.RecPackQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("AttnNote", ConsigndtlCont.AttnNote, SQLControl.EnumDataType.dtStringN)
                                .AddField("Remark", ConsigndtlCont.Remark, SQLControl.EnumDataType.dtStringN)
                                .AddField("AuthorisedDate", ConsigndtlCont.AuthorisedDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("AuthorisedBy", ConsigndtlCont.AuthorisedBy, SQLControl.EnumDataType.dtString)
                                .AddField("IssuedDate", ConsigndtlCont.IssuedDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("IssuedBy", ConsigndtlCont.IssuedBy, SQLControl.EnumDataType.dtString)
                                .AddField("CreatedDate", ConsigndtlCont.IssuedDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", ConsigndtlCont.IssuedBy, SQLControl.EnumDataType.dtString)
                                .AddField("ReceivedDate", ConsigndtlCont.ReceivedDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ReceivedBy", ConsigndtlCont.ReceivedBy, SQLControl.EnumDataType.dtString)
                                .AddField("IsHost", ConsigndtlCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastUpdate", ConsigndtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", ConsigndtlCont.UpdateBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsigndtlCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsigndtlCont.ContractNo) & "' AND SeqNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, ConsigndtlCont.SeqNo) & "' AND StorageID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsigndtlCont.StorageID) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("TransID", ConsigndtlCont.TransID, SQLControl.EnumDataType.dtString)
                                                .AddField("ContractNo", ConsigndtlCont.ContractNo, SQLControl.EnumDataType.dtString)
                                                .AddField("SeqNo", ConsigndtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                                .AddField("StorageID", ConsigndtlCont.StorageID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsigndtlCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsigndtlCont.ContractNo) & "' AND SeqNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, ConsigndtlCont.SeqNo) & "' AND StorageID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsigndtlCont.StorageID) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True

                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                Else
                                    message = axExecute.Message.ToString()
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignDTL", axExecute.Message & strSQL, axExecute.StackTrace)
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
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignDTL", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignDTL", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ConsigndtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal ConsigndtlCont As Container.Consigndtl, ByRef message As String) As Boolean
            Return Save(ConsigndtlCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal ConsigndtlCont As Container.Consigndtl, ByRef message As String) As Boolean
            Return Save(ConsigndtlCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal ConsigndtlCont As Container.Consigndtl, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If ConsigndtlCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ConsigndtlInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsigndtlCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsigndtlCont.ContractNo) & "' AND SeqNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, ConsigndtlCont.SeqNo) & "' AND StorageID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsigndtlCont.StorageID) & "'")
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
                                strSQL = BuildUpdate("Consigndtl WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtDateTime, ConsigndtlCont.LastUpdate) & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsigndtlCont.UpdateBy) & "' WHERE " & _
                                " TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsigndtlCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsigndtlCont.ContractNo) & "' AND SeqNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, ConsigndtlCont.SeqNo) & "' AND StorageID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsigndtlCont.StorageID) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("Consigndtl WITH (ROWLOCK)", "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsigndtlCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsigndtlCont.ContractNo) & "' AND SeqNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, ConsigndtlCont.SeqNo) & "' AND StorageID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, ConsigndtlCont.StorageID) & "'")
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignDTL", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignDTL", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignDTL", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                ConsigndtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region

#Region "Data Selection"

        Public Overloads Function GetConsignDTLTransID(ByVal TransID As System.String) As Container.Consigndtl
            Dim rConsigndtl As Container.Consigndtl = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With ConsigndtlInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransID) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rConsigndtl = New Container.Consigndtl
                                rConsigndtl.TransID = drRow.Item("TransID")
                                rConsigndtl.ContractNo = drRow.Item("ContractNo")
                                rConsigndtl.SeqNo = drRow.Item("SeqNo")
                                rConsigndtl.StorageID = drRow.Item("StorageID")
                                rConsigndtl.ERPMATNo = drRow.Item("ERPMATNo")
                                rConsigndtl.SerialNo = drRow.Item("SerialNo")
                                rConsigndtl.WasteCode = drRow.Item("WasteCode")
                                rConsigndtl.WasteDescription = drRow.Item("WasteDescription")
                                rConsigndtl.WasteType = drRow.Item("WasteType")
                                rConsigndtl.WastePackage = drRow.Item("WastePackage")
                                rConsigndtl.OperationType = drRow.Item("OperationType")
                                rConsigndtl.TermID = drRow.Item("TermID")
                                rConsigndtl.WasteComponent = drRow.Item("WasteComponent")
                                rConsigndtl.OriginCode = drRow.Item("OriginCode")
                                rConsigndtl.OriginDescription = drRow.Item("OriginDescription")
                                rConsigndtl.TariffCode = drRow.Item("TariffCode")
                                rConsigndtl.CountryCode = drRow.Item("CountryCode")
                                rConsigndtl.PackagingQty = drRow.Item("PackagingQty")
                                rConsigndtl.PackagingQtyKg = drRow.Item("PackagingQtyKg")
                                rConsigndtl.PackagingWeight = drRow.Item("PackagingWeight")
                                rConsigndtl.TreatmentCost = drRow.Item("TreatmentCost")
                                rConsigndtl.RegistedSupp = drRow.Item("RegistedSupp")
                                rConsigndtl.Qty = drRow.Item("Qty")
                                rConsigndtl.PackQty = drRow.Item("PackQty")
                                rConsigndtl.RcvQty = drRow.Item("RcvQty")
                                rConsigndtl.RcvPackQty = drRow.Item("RcvPackQty")
                                rConsigndtl.PalletQty = drRow.Item("PalletQty")
                                rConsigndtl.RecPackQty = drRow.Item("RecPackQty")
                                rConsigndtl.AttnNote = drRow.Item("AttnNote")
                                rConsigndtl.Remark = drRow.Item("Remark")
                                rConsigndtl.AuthorisedBy = drRow.Item("AuthorisedBy")
                                rConsigndtl.IssuedBy = drRow.Item("IssuedBy")
                                rConsigndtl.ReceivedBy = drRow.Item("ReceivedBy")
                                rConsigndtl.rowguid = drRow.Item("rowguid")
                                rConsigndtl.SyncCreate = drRow.Item("SyncCreate")
                                rConsigndtl.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rConsigndtl.IsHost = drRow.Item("IsHost")
                                rConsigndtl.LastSyncBy = drRow.Item("LastSyncBy")
                                rConsigndtl.CreateBy = drRow.Item("CreateBy")
                            Else
                                rConsigndtl = Nothing
                            End If
                        Else
                            rConsigndtl = Nothing
                        End If
                    End With
                End If
                Return rConsigndtl
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignDTL", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rConsigndtl = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetConsignDTL(ByVal TransID As System.String, ByVal ContractNo As System.String) As Container.Consigndtl
            Dim rConsigndtl As Container.Consigndtl = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With ConsigndtlInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ContractNo) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rConsigndtl = New Container.Consigndtl
                                rConsigndtl.TransID = drRow.Item("TransID")
                                rConsigndtl.ContractNo = drRow.Item("ContractNo")
                                rConsigndtl.SeqNo = drRow.Item("SeqNo")
                                rConsigndtl.StorageID = drRow.Item("StorageID")
                                rConsigndtl.ERPMATNo = drRow.Item("ERPMATNo")
                                rConsigndtl.SerialNo = drRow.Item("SerialNo")
                                rConsigndtl.WasteCode = drRow.Item("WasteCode")
                                rConsigndtl.WasteDescription = drRow.Item("WasteDescription")
                                rConsigndtl.WasteType = drRow.Item("WasteType")
                                rConsigndtl.WastePackage = drRow.Item("WastePackage")
                                rConsigndtl.OperationType = drRow.Item("OperationType")
                                rConsigndtl.TermID = drRow.Item("TermID")
                                rConsigndtl.WasteComponent = drRow.Item("WasteComponent")
                                rConsigndtl.OriginCode = drRow.Item("OriginCode")
                                rConsigndtl.OriginDescription = drRow.Item("OriginDescription")
                                rConsigndtl.TariffCode = drRow.Item("TariffCode")
                                rConsigndtl.CountryCode = drRow.Item("CountryCode")
                                rConsigndtl.PackagingQty = drRow.Item("PackagingQty")
                                rConsigndtl.PackagingQtyKg = drRow.Item("PackagingQtyKg")
                                rConsigndtl.PackagingWeight = drRow.Item("PackagingWeight")
                                rConsigndtl.TreatmentCost = drRow.Item("TreatmentCost")
                                rConsigndtl.RegistedSupp = drRow.Item("RegistedSupp")
                                rConsigndtl.Qty = drRow.Item("Qty")
                                rConsigndtl.PackQty = drRow.Item("PackQty")
                                rConsigndtl.RcvQty = drRow.Item("RcvQty")
                                rConsigndtl.RcvPackQty = drRow.Item("RcvPackQty")
                                rConsigndtl.PalletQty = drRow.Item("PalletQty")
                                rConsigndtl.RecPackQty = drRow.Item("RecPackQty")
                                rConsigndtl.AttnNote = drRow.Item("AttnNote")
                                rConsigndtl.Remark = drRow.Item("Remark")
                                rConsigndtl.AuthorisedBy = drRow.Item("AuthorisedBy")
                                rConsigndtl.IssuedBy = drRow.Item("IssuedBy")
                                rConsigndtl.ReceivedBy = drRow.Item("ReceivedBy")
                                rConsigndtl.rowguid = drRow.Item("rowguid")
                                rConsigndtl.SyncCreate = drRow.Item("SyncCreate")
                                rConsigndtl.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rConsigndtl.IsHost = drRow.Item("IsHost")
                                rConsigndtl.LastSyncBy = drRow.Item("LastSyncBy")
                                rConsigndtl.CreateBy = drRow.Item("CreateBy")
                            Else
                                rConsigndtl = Nothing
                            End If
                        Else
                            rConsigndtl = Nothing
                        End If
                    End With
                End If
                Return rConsigndtl
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignDTL", ex.Message, ex.StackTrace)
            Finally
                rConsigndtl = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetConsignDTL(ByVal TransID As System.String, ByVal ContractNo As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Consigndtl)
            Dim rConsigndtl As Container.Consigndtl = Nothing
            Dim lstConsigndtl As List(Of Container.Consigndtl) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With ConsigndtlInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal TransID As System.String, ByVal ContractNo As System.String, ByVal SeqNo As System.Int32, ByVal StorageID As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ContractNo) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rConsigndtl = New Container.Consigndtl
                                rConsigndtl.TransID = drRow.Item("TransID")
                                rConsigndtl.ContractNo = drRow.Item("ContractNo")
                                rConsigndtl.SeqNo = drRow.Item("SeqNo")
                                rConsigndtl.StorageID = drRow.Item("StorageID")
                                rConsigndtl.ERPMATNo = drRow.Item("ERPMATNo")
                                rConsigndtl.SerialNo = drRow.Item("SerialNo")
                                rConsigndtl.WasteCode = drRow.Item("WasteCode")
                                rConsigndtl.WasteDescription = drRow.Item("WasteDescription")
                                rConsigndtl.WasteType = drRow.Item("WasteType")
                                rConsigndtl.WastePackage = drRow.Item("WastePackage")
                                rConsigndtl.OperationType = drRow.Item("OperationType")
                                rConsigndtl.TermID = drRow.Item("TermID")
                                rConsigndtl.WasteComponent = drRow.Item("WasteComponent")
                                rConsigndtl.OriginCode = drRow.Item("OriginCode")
                                rConsigndtl.OriginDescription = drRow.Item("OriginDescription")
                                rConsigndtl.TariffCode = drRow.Item("TariffCode")
                                rConsigndtl.CountryCode = drRow.Item("CountryCode")
                                rConsigndtl.PackagingQty = drRow.Item("PackagingQty")
                                rConsigndtl.PackagingQtyKg = drRow.Item("PackagingQtyKg")
                                rConsigndtl.PackagingWeight = drRow.Item("PackagingWeight")
                                rConsigndtl.TreatmentCost = drRow.Item("TreatmentCost")
                                rConsigndtl.RegistedSupp = drRow.Item("RegistedSupp")
                                rConsigndtl.Qty = drRow.Item("Qty")
                                rConsigndtl.PackQty = drRow.Item("PackQty")
                                rConsigndtl.RcvQty = drRow.Item("RcvQty")
                                rConsigndtl.RcvPackQty = drRow.Item("RcvPackQty")
                                rConsigndtl.PalletQty = drRow.Item("PalletQty")
                                rConsigndtl.RecPackQty = drRow.Item("RecPackQty")
                                rConsigndtl.AttnNote = drRow.Item("AttnNote")
                                rConsigndtl.Remark = drRow.Item("Remark")
                                rConsigndtl.AuthorisedBy = drRow.Item("AuthorisedBy")
                                rConsigndtl.IssuedBy = drRow.Item("IssuedBy")
                                rConsigndtl.ReceivedBy = drRow.Item("ReceivedBy")
                                rConsigndtl.rowguid = drRow.Item("rowguid")
                                rConsigndtl.SyncCreate = drRow.Item("SyncCreate")
                                rConsigndtl.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rConsigndtl.IsHost = drRow.Item("IsHost")
                                rConsigndtl.LastSyncBy = drRow.Item("LastSyncBy")
                                rConsigndtl.CreateBy = drRow.Item("CreateBy")
                                rConsigndtl.CreateDate = drRow.Item("CreateDate")
                            Next
                            lstConsigndtl.Add(rConsigndtl)
                        Else
                            rConsigndtl = Nothing
                        End If
                        Return lstConsigndtl
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignDTL", ex.Message, ex.StackTrace)
            Finally
                rConsigndtl = Nothing
                lstConsigndtl = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetConsignDTL(ByVal TransNo As String) As Container.Consigndtl
            Dim rConsignDTL As Container.Consigndtl = Nothing
            Dim dtTemp As DataTable = Nothing
            If StartConnection() = True Then
                StartSQLControl()
                strSQL = "SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY I.QtyOnHand desc) as RecNo,I.ItemCode," & _
                            "I.ItemName, I.ItemComponent, I.PackUOM, I.ItemDesc,CM.CodeDesc as OperationDesc," & _
                            "S.StorageID, S.StorageAreaCode, S.StorageBin," & _
                            "D.*, I.QtyOnHand,I.QtyOnHand*1000 as QtyOnHandKg" & _
                            ",ISNULL((select TOP 1 CodeDesc from CODEMASTER WITH (NOLOCK) where Code=D.WasteType AND CodeType='WTY'),'') as WasteTypeDesc" & _
                            ", (select MAX(AuthorisedDate) as LastTransDate from CONSIGNDTL WITH (NOLOCK) where WasteCode=I.ItemCode) as LastTransDate" & _
                            " FROM CONSIGNHDR H WITH (NOLOCK) INNER JOIN CONSIGNDTL D WITH (NOLOCK) on H.ContractNo=D.ContractNo" & _
                            " LEFT JOIN ITEMLOC I WITH (NOLOCK) on H.GeneratorLocID=I.LocID and D.WasteCode=I.ItemCode AND D.WasteDescription=I.ItemName" & _
                            " LEFT JOIN StorageMaster S WITH (NOLOCK) on D.StorageID=S.StorageID and H.GeneratorLocID=S.LocID" & _
                            " LEFT JOIN CodeMaster CM WITH (NOLOCK) on D.OperationType=CM.Code and CM.CodeType = 'WTH'" & _
                            " WHERE H.ContractNo='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransNo) & "'"
                Try
                    dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
                    If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
                        rConsignDTL = New Container.Consigndtl
                        With rConsignDTL
                            .SerialNo = dtTemp.Rows(0).Item("SerialNo").ToString
                            .WasteCode = dtTemp.Rows(0).Item("WasteCode").ToString
                            .WasteName = dtTemp.Rows(0).Item("ItemName").ToString
                            .OriginCode = dtTemp.Rows(0).Item("OriginCode").ToString
                            .OriginDescription = dtTemp.Rows(0).Item("OriginDescription").ToString
                            .WasteComponent = dtTemp.Rows(0).Item("WasteComponent").ToString
                            .Remark = dtTemp.Rows(0).Item("Remark").ToString
                            .WasteType = dtTemp.Rows(0).Item("WasteTypeDesc").ToString
                            .Qty = dtTemp.Rows(0).Item("Qty").ToString
                            .PackQty = dtTemp.Rows(0).Item("PackQty").ToString
                            .TreatmentCost = dtTemp.Rows(0).Item("TreatmentCost").ToString
                            .PackageType = dtTemp.Rows(0).Item("PackUOM").ToString
                            .PackagingQty = dtTemp.Rows(0).Item("PackagingQty").ToString
                            .OperationDesc = dtTemp.Rows(0).Item("OperationDesc").ToString
                            .RcvQty = dtTemp.Rows(0).Item("RcvQty").ToString
                            .RcvPackQty = dtTemp.Rows(0).Item("RcvPackQty").ToString
                        End With
                    End If
                Catch ex As Exception
                    Log.Notifier.Notify(ex)
                    Gibraltar.Agent.Log.Error("Actions/Notification", ex.Message & " " & strSQL, ex.StackTrace)
                Finally
                    EndSQLControl()
                End Try
            End If
            EndConnection()
            Return rConsignDTL
        End Function

        Public Overloads Function GetConsignDTLList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ConsigndtlInfo.MyInfo
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

        'Add
        Public Overloads Function GetBarcode(ByVal SQL As String) As Data.DataTable
            If StartConnection() = True Then
                With ConsigndtlInfo.MyInfo
                    strSQL = SQL


                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        'Add
        Public Overloads Function GetWasteCustomList(Optional ByVal Condition As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ConsigndtlInfo.MyInfo
                    strSQL = "SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY I.QtyOnHand desc) as RecNo,I.ItemCode, " &
                                "I.ItemName, I.ItemComponent, I.PackUOM, I.ItemDesc," &
                                "S.StorageID, S.StorageAreaCode, S.StorageBin," &
                                "D.*, I.QtyOnHand,I.QtyOnHand*1000 as QtyOnHandKg" &
                                ",ISNULL((select TOP 1 CodeDesc from CODEMASTER WITH (NOLOCK) where Code=D.WasteType AND CodeType='WTY'),'') as WasteTypeDesc" &
                                ", (select MAX(AuthorisedDate) as LastTransDate from CONSIGNDTL WITH (NOLOCK) where WasteCode=I.ItemCode) as LastTransDate" &
                                " FROM CONSIGNHDR H WITH (NOLOCK) INNER JOIN CONSIGNDTL D WITH (NOLOCK) on H.ContractNo=D.ContractNo" &
                                " LEFT JOIN ITEMLOC I WITH (NOLOCK) on H.GeneratorLocID=I.LocID and D.WasteCode=I.ItemCode AND D.WasteDescription=I.ItemName" &
                                " LEFT JOIN StorageMaster S WITH (NOLOCK) on D.StorageID=S.StorageID" &
                                " and H.GeneratorLocID=S.LocID"

                    If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetSubmittedConsignDTL(Optional ByVal Condition As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ConsigndtlInfo.MyInfo
                    strSQL = "SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY I.QtyOnHand desc) as RecNo,I.ItemCode, " &
                                "I.ItemName, I.ItemComponent, I.PackUOM, I.ItemDesc," &
                                "S.StorageID, S.StorageAreaCode, S.StorageBin," &
                                "D.*, I.QtyOnHand,I.QtyOnHand*1000 as QtyOnHandKg" &
                                ",ISNULL((select TOP 1 CodeDesc from CODEMASTER WITH (NOLOCK) where Code=D.WasteType AND CodeType='WTY'),'') as WasteTypeDesc" &
                                ", (select MAX(AuthorisedDate) as LastTransDate from CONSIGNDTL WITH (NOLOCK) where WasteCode=I.ItemCode) as LastTransDate" &
                                " FROM CONSIGNHDR H WITH (NOLOCK) INNER JOIN CONSIGNDTL D WITH (NOLOCK) on H.ContractNo=D.ContractNo" &
                                " LEFT JOIN ITEMLOC I WITH (NOLOCK) on H.GeneratorLocID=I.LocID and D.WasteCode=I.ItemCode AND D.WasteDescription=I.ItemName" &
                                " LEFT JOIN StorageMaster S WITH (NOLOCK) on D.StorageID=S.StorageID" &
                                " and H.GeneratorLocID=S.LocID"

                    Condition &= " AND (H.Status >= 1 and H.Status not in (2,5,8,9,11,13,14,15))"
                    If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetConsignDTLShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With ConsigndtlInfo.MyInfo
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

        Public Overloads Function GetLampiranA(ByVal LocID As System.String) As Data.DataTable
            If StartConnection() = True Then
                With ConsigndtlInfo.MyInfo
                    StartSQLControl()

                    strSQL = "  SELECT ROW_NUMBER() OVER(ORDER BY BranchName) AS ID, BranchName, Address1,BizRegID, BizLocID, WasteCode, IsNew, sum(value) value, froms FROM ( " &
                                "SELECT 0 as ID, BL.BranchName,BL.Address1, BL.BizRegID, BL.BizLocID, CD.WasteCode, CH.IsNew, 0 As Value, 1 As froms FROM CONSIGNHDR CH WITH (NOLOCK) " &
                                "INNER JOIN CONSIGNDTL CD WITH (NOLOCK) ON CH.TransID = CD.TransID LEFT JOIN BIZLOCATE BL WITH (NOLOCK) ON CH.GeneratorLocID = BL.BizLocID " &
                                "LEFT JOIN CODEMASTER CM WITH (NOLOCK) ON CD.WasteType = CM.Code LEFT JOIN LICENSEITEM LI WITH (NOLOCK) ON CD.WasteCode = LI.ItemCode INNER JOIN LICENSE L WITH (NOLOCK) ON " &
                                "L.ContractNo = LI.ContractNo AND L.LocId = CH.ReceiverLocID WHERE NOT EXISTS (SELECT 1 FROM TWG_SUBMISSIONHDR HDR INNER JOIN TWG_SUBMISSIONDTL " &
                                "DTL ON HDR.SubmissionID = DTL.SubmissionID WHERE HDR.GeneratorLocID = CH.GeneratorLocID AND DTL.WasteCode = CD.WasteCode And HDR.IsNew = 0 AND " &
                                "((HDR.StatusTWG IN (1,0) AND HDR.SubStatus= 0) OR (HDR.StatusTWG IN (1,0,2) AND HDR.SubStatus= 1)) AND HDR.ReceiverLocID = CH.ReceiverLocID) AND " &
                                "CM.CodeType = 'WTY' AND CH.Status = 8 AND L.Active=1 AND L.Flag=1 AND L.ValidityEnd>=CONVERT(date,GETDATE()) AND CH.ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND CH.IsNew = 0 AND BL.Active = 1 GROUP BY BL.BranchName, BL.BizRegID, BL.BizLocID, " &
                                "CD.WasteCode, CH.IsNew, BL.Address1 " &
                                "UNION " &
                                "SELECT 0 As ID, BL.BranchName,BL.Address1, BL.BizRegID, BL.BizLocID, DT.WasteCode, HD.IsNew, DT.ExpectedQty Value, 2 as froms FROM TWG_SUBMISSIONHDR HD WITH (NOLOCK) " &
                                "INNER JOIN TWG_SUBMISSIONDTL DT WITH (NOLOCK) ON HD.SubmissionID = DT.SubmissionID LEFT JOIN BIZLOCATE BL WITH (NOLOCK) ON HD.GeneratorLocID = BL.BizLocID " &
                                "LEFT JOIN CODEMASTER CM WITH (NOLOCK) ON DT.WasteType = CM.CodeSeq WHERE CM.CodeType = 'WTY' AND HD.Status = 1 " &
                                "AND HD.ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND HD.IsNew = 0 AND BL.Active = 1 GROUP BY BL.BranchName,BL.Address1, BL.BizRegID, BL.BizLocID, " &
                                "DT.WasteCode, DT.ExpectedQty, HD.IsNew) AS X GROUP BY BranchName,Address1, BizRegID, BizLocID, WasteCode, IsNew, froms ORDER BY BranchName ASC"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetLampiranAQty(ByVal LocID As System.String, ByVal WasteCode As System.String) As Data.DataTable
            If StartConnection() = True Then
                With ConsigndtlInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT Qty FROM (" &
                            " select LI.Qty FROM CONSIGNHDR CH WITH (NOLOCK) " &
                            " INNER JOIN CONSIGNDTL CD WITH (NOLOCK) ON CH.TransID = CD.TransID " &
                            " LEFT JOIN BIZLOCATE BL WITH (NOLOCK) ON CH.GeneratorLocID = BL.BizLocID " &
                            " LEFT JOIN CODEMASTER CM WITH (NOLOCK) ON CD.WasteType = CM.Code " &
                            " LEFT JOIN LICENSEITEM LI WITH (NOLOCK) ON CD.WasteCode = LI.ItemCode " &
                            " INNER JOIN LICENSE L WITH (NOLOCK) ON L.ContractNo = LI.ContractNo AND L.LocId = CH.ReceiverLocID" &
                            " WHERE NOT EXISTS (SELECT 1 FROM TWG_SUBMISSIONHDR HDR INNER JOIN TWG_SUBMISSIONDTL DTL ON HDR.SubmissionID = DTL.SubmissionID WHERE HDR.GeneratorLocID = CH.GeneratorLocID AND DTL.WasteCode = CD.WasteCode And HDR.IsNew = 0 AND ((HDR.StatusTWG IN (1,0) AND HDR.SubStatus= 0) OR (HDR.StatusTWG IN (1,0,2) AND HDR.SubStatus= 1)) " &
                            " AND HDR.ReceiverLocID = CH.ReceiverLocID) " &
                            " AND CM.CodeType = 'WTY' AND CH.Status = 8 " &
                            " AND L.Active=1 AND L.Flag=1 " &
                            " AND L.ValidityEnd>=CONVERT(date,GETDATE())   " &
                            " AND CH.IsNew = 0 " &
                            " AND CH.ReceiverLocID = '" & LocID & "'" &
                            " AND CD.WasteCode = '" & WasteCode & "'" &
                            " UNION" &
                            " Select LI.Qty" &
                            " FROM TWG_SUBMISSIONHDR HD WITH (NOLOCK) " &
                            " INNER JOIN TWG_SUBMISSIONDTL DT WITH (NOLOCK) ON HD.SubmissionID = DT.SubmissionID " &
                            " LEFT JOIN BIZLOCATE BL WITH (NOLOCK) ON HD.GeneratorLocID = BL.BizLocID " &
                            " LEFT JOIN CODEMASTER CM WITH (NOLOCK) ON DT.WasteType = CM.Code " &
                            " LEFT JOIN LICENSEITEM LI WITH (NOLOCK) ON DT.WasteCode = LI.ItemCode " &
                            " INNER JOIN LICENSE L WITH (NOLOCK) ON L.ContractNo = LI.ContractNo AND L.LocId = HD.ReceiverLocID " &
                            " WHERE CM.CodeType = 'WTY' AND HD.Status = 1 AND HD.ReceiverLocID = '" & LocID & "' AND HD.IsNew = 0 " &
                            " AND DT.WasteCode = '" & WasteCode & "') AS X"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetLampiranAList(Optional ByVal strFilter As String = Nothing, Optional ByVal District As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ConsigndtlInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT ID, BranchName, WasteCode, CodeDesc, WasteDescription, SUM(Value) TotalQty, State, District FROM ( " &
                            "SELECT BL.BizLocID as ID, BL.BranchName, CD.WasteCode, CM.CodeDesc, CD.WasteDescription, SUM(CD.Qty) As Value, S.StateDesc As State, C.CityDesc As District " &
                            "FROM CONSIGNHDR CH WITH (NOLOCK) INNER JOIN CONSIGNDTL CD WITH (NOLOCK) ON CH.TransID = CD.TransID LEFT JOIN BIZLOCATE BL WITH (NOLOCK) " &
                            "ON CH.ReceiverLocID = BL.BizLocID LEFT JOIN CODEMASTER CM WITH (NOLOCK) ON CD.WasteType = CM.Code LEFT JOIN CITY C WITH (NOLOCK) ON BL.City = C.CityCode LEFT JOIN State S WITH (NOLOCK) ON BL.State = S.StateCode WHERE CM.CodeType = 'WTY' " &
                            "AND CH.Status = 8 AND IsNew = 0 " &
                            "GROUP BY BL.BizLocID, BL.BranchName, CD.WasteCode, CM.CodeDesc, CD.WasteDescription, S.StateDesc, C.CityDesc " &
                            "UNION ALL " &
                            "SELECT BL.BizLocID As ID, BL.BranchName, DT.WasteCode, CM.CodeDesc, DT.WasteDescription, SUM(DT.ExpectedQty) As Value, S.StateDesc As State, C.CityDesc As District " &
                            "FROM TWG_SUBMISSIONHDR HD WITH (NOLOCK) INNER JOIN TWG_SUBMISSIONDTL DT WITH (NOLOCK) ON HD.SubmissionID = DT.SubmissionID " &
                            "LEFT JOIN BIZLOCATE BL WITH (NOLOCK) ON HD.ReceiverLocID = BL.BizLocID LEFT JOIN CODEMASTER CM WITH (NOLOCK) ON " &
                            "DT.WasteType = CM.CodeSeq LEFT JOIN CITY C WITH (NOLOCK) ON BL.City = C.CityCode LEFT JOIN State S WITH (NOLOCK) ON BL.State = S.StateCode  WHERE CM.CodeType = 'WTY' AND ((HD.StatusTWG IN (1,0) AND HD.SubStatus= 0) OR (HD.StatusTWG IN (1,0,2) AND HD.SubStatus= 1)) And IsNew = 0 " &
                            "GROUP BY BL.BizLocID, BL.BranchName, DT.WasteCode, CM.CodeDesc, DT.WasteDescription, S.StateDesc, C.CityDesc) AS X "
                    If Not strFilter Is Nothing AndAlso strFilter <> "" Then
                        strSQL &= strFilter
                    End If
                    If Not District Is Nothing AndAlso District <> "" Then
                        strSQL &= " AND District='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, District) & "' "
                    End If

                    strSQL &= "GROUP BY ID, BranchName, WasteCode, CodeDesc, WasteDescription, State, District "

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetListLampiranA(Optional ByVal strFilter As String = Nothing, Optional ByVal District As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ConsigndtlInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT BL.BizLocID As ID, BL.BranchName, B.BranchName As GeneratorName, B.BizLocID As GeneratorLocID, DT.WasteCode, " &
                        "SUM(DT.ExpectedQty) As Value, S.StateDesc As State, C.CityDesc As District, MAX(HD.LastUpdate) LastUpdate, HD.UpdateBy FROM TWG_SUBMISSIONHDR HD WITH (NOLOCK) " &
                        "INNER JOIN TWG_SUBMISSIONDTL DT WITH (NOLOCK) ON HD.SubmissionID = DT.SubmissionID LEFT JOIN BIZLOCATE BL WITH (NOLOCK) " &
                        "ON HD.ReceiverLocID = BL.BizLocID LEFT JOIN BIZLOCATE B WITH (NOLOCK) ON HD.GeneratorLocID = B.BizLocID LEFT JOIN " &
                        "CODEMASTER CM WITH (NOLOCK) ON DT.WasteType = CM.Code LEFT JOIN CITY C WITH (NOLOCK) ON BL.City = C.CityCode LEFT JOIN " &
                        "State S WITH (NOLOCK) ON BL.State = S.StateCode WHERE CM.CodeType = 'WTY' AND ((HD.StatusTWG IN (1,0) AND HD.SubStatus= 0) " &
                        "OR (HD.StatusTWG IN (1,0,2) AND HD.SubStatus= 1)) And IsNew = 0 "
                    If Not strFilter Is Nothing AndAlso strFilter <> "" Then
                        strSQL &= strFilter
                    End If
                    If Not District Is Nothing AndAlso District <> "" Then
                        strSQL &= " AND C.CityDesc ='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, District) & "' "
                    End If

                    strSQL &= "GROUP BY BL.BizLocID, BL.BranchName, B.BranchName, B.BizLocID, DT.WasteCode, S.StateDesc, C.CityDesc, HD.UpdateBy"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetListLampiranACount(Optional ByVal strFilter As String = Nothing, Optional ByVal District As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ConsigndtlInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT BL.BizLocID As ID, BL.BranchName, COUNT(DISTINCT B.BranchName) As GeneratorName, COUNT(B.BizLocID) As GeneratorLocID, COUNT(DISTINCT DT.WasteCode) WasteCode, " &
                        "SUM(DT.ExpectedQty) As Value, S.StateDesc As State, C.CityDesc As District, MAX(HD.LastUpdate) LastUpdate, HD.UpdateBy FROM TWG_SUBMISSIONHDR HD WITH (NOLOCK) " &
                        "INNER JOIN TWG_SUBMISSIONDTL DT WITH (NOLOCK) ON HD.SubmissionID = DT.SubmissionID LEFT JOIN BIZLOCATE BL WITH (NOLOCK) " &
                        "ON HD.ReceiverLocID = BL.BizLocID LEFT JOIN BIZLOCATE B WITH (NOLOCK) ON HD.GeneratorLocID = B.BizLocID LEFT JOIN " &
                        "CODEMASTER CM WITH (NOLOCK) ON DT.WasteType = CM.Code LEFT JOIN CITY C WITH (NOLOCK) ON BL.City = C.CityCode LEFT JOIN " &
                        "State S WITH (NOLOCK) ON BL.State = S.StateCode WHERE CM.CodeType = 'WTY' AND ((HD.StatusTWG IN (1,0) AND HD.SubStatus= 0) " &
                        "OR (HD.StatusTWG IN (1,0,2) AND HD.SubStatus= 1)) And IsNew = 0 "
                    If Not strFilter Is Nothing AndAlso strFilter <> "" Then
                        strSQL &= strFilter
                    End If
                    If Not District Is Nothing AndAlso District <> "" Then
                        strSQL &= " AND C.CityDesc ='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, District) & "' "
                    End If

                    strSQL &= "GROUP BY BL.BizLocID, BL.BranchName, S.StateDesc, C.CityDesc, HD.UpdateBy"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetLampiranAListLocId(ByVal BizLocID As String) As Data.DataTable
            If StartConnection() = True Then
                With ConsigndtlInfo.MyInfo
                    StartSQLControl()
                    
                    strSQL = "SELECT B.BizLocID, BL.BranchName, B.BranchName As GeneratorName, BL.AccNo, BL.Tel, BL.Fax, BL.ContactPerson, " &
                        "B.Address1, BL.Address2, BL.Address3, BL.Address4, C.CityDesc, S.StateDesc, CN.CountryDesc, DT.WasteCode, " &
                        "SUM(DT.ExpectedQty) As Value FROM TWG_SUBMISSIONHDR HD WITH (NOLOCK) INNER JOIN TWG_SUBMISSIONDTL DT WITH (NOLOCK) " &
                        "ON HD.SubmissionID = DT.SubmissionID LEFT JOIN BIZLOCATE BL WITH (NOLOCK) ON HD.ReceiverLocID = BL.BizLocID LEFT JOIN " &
                        "BIZLOCATE B WITH (NOLOCK) ON HD.GeneratorLocID = B.BizLocID LEFT JOIN CITY C ON BL.City = C.CityCode LEFT JOIN State S " &
                        "ON BL.State = S.StateCode LEFT JOIN COUNTRY CN ON BL.Country = CN.CountryCode WHERE HD.Status<>1 AND HD.Status<>0 " &
                        "AND HD.IsNew = 0 AND HD.ReceiverLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, BizLocID) & "' " &
                        "GROUP BY B.BizLocID, BL.BranchName, B.BranchName, BL.AccNo, BL.Tel, BL.Fax, BL.ContactPerson, B.Address1, BL.Address2, " &
                        "BL.Address3, BL.Address4, C.CityDesc, S.StateDesc, CN.CountryDesc, DT.WasteCode"

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


    Namespace Container

#Region "Class Container"
        Public Class Consigndtl
            Public fTransID As System.String = "TransID"
            Public fContractNo As System.String = "ContractNo"
            Public fSeqNo As System.String = "SeqNo"
            Public fStorageID As System.String = "StorageID"
            Public fERPMATNo As System.String = "ERPMATNo"
            Public fSerialNo As System.String = "SerialNo"
            Public fWasteCode As System.String = "WasteCode"
            Public fWasteDescription As System.String = "WasteDescription"
            Public fWasteType As System.String = "WasteType"
            Public fWastePackage As System.String = "WastePackage"
            Public fOperationType As System.String = "OperationType"
            Public fOperationDesc As System.String = "OperationDesc"
            Public fTransInit As System.String = "TransInit"
            Public fTermID As System.String = "TermID"
            Public fWasteComponent As System.String = "WasteComponent"
            Public fOriginCode As System.String = "OriginCode"
            Public fOriginDescription As System.String = "OriginDescription"
            Public fTariffCode As System.String = "TariffCode"
            Public fCountryCode As System.String = "CountryCode"
            Public fPackagingQty As System.String = "PackagingQty"
            Public fPackagingQtyKg As System.String = "PackagingQtyKg"
            Public fPackagingWeight As System.String = "PackagingWeight"
            Public fTreatmentCost As System.String = "TreatmentCost"
            Public fRegistedSupp As System.String = "RegistedSupp"
            Public fQty As System.String = "Qty"
            Public fPackQty As System.String = "PackQty"
            Public fRcvQty As System.String = "RcvQty"
            Public fRcvPackQty As System.String = "RcvPackQty"
            Public fPalletQty As System.String = "PalletQty"
            Public fRecPackQty As System.String = "RecPackQty"
            Public fAttnNote As System.String = "AttnNote"
            Public fRemark As System.String = "Remark"
            Public fAuthorisedDate As System.String = "AuthorisedDate"
            Public fAuthorisedBy As System.String = "AuthorisedBy"
            Public fIssuedDate As System.String = "IssuedDate"
            Public fIssuedBy As System.String = "IssuedBy"
            Public fReceivedDate As System.String = "ReceivedDate"
            Public fReceivedBy As System.String = "ReceivedBy"
            Public frowguid As System.String = "rowguid"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fIsHost As System.String = "IsHost"
            Public fLastSyncBy As System.String = "LastSyncBy"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fUpdateBy As System.String = "UpdateBy"
            Public fCreateDate As System.String = "CreateDate"
            Public fCreateBy As System.String = "CreateBy"

            Protected _TransID As System.String
            Protected _ContractNo As System.String
            Protected _SeqNo As System.Int32
            Protected _StorageID As System.String
            Private _ERPMATNo As System.String
            Private _SerialNo As System.String
            Private _WasteCode As System.String
            Private _WasteDescription As System.String
            Private _WasteType As System.String
            Private _WastePackage As System.String
            Private _OperationType As System.String
            Private _OperationDesc As System.String
            Private _TransInit As System.String
            Private _TermID As System.Int16
            Private _WasteComponent As System.String
            Private _OriginCode As System.String
            Private _OriginDescription As System.String
            Private _TariffCode As System.String
            Private _CountryCode As System.String
            Private _PackagingQty As System.String
            Private _PackagingQtyKg As System.String
            Private _PackagingWeight As System.String
            Private _TreatmentCost As System.Decimal
            Private _RegistedSupp As System.Byte
            Private _Qty As System.Decimal
            Private _PackQty As System.Decimal
            Private _RcvQty As System.Decimal
            Private _RcvPackQty As System.Decimal
            Private _PalletQty As System.Decimal
            Private _RecPackQty As System.Decimal
            Private _AttnNote As System.String
            Private _Remark As System.String
            Private _AuthorisedDate As System.DateTime
            Private _AuthorisedBy As System.String
            Private _IssuedDate As System.DateTime
            Private _IssuedBy As System.String
            Private _ReceivedDate As System.DateTime
            Private _ReceivedBy As System.String
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _IsHost As System.Byte
            Private _LastSyncBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
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
            ''' Mandatory
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
            ''' Mandatory
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
            Public Property ERPMATNo As System.String
                Get
                    Return _ERPMATNo
                End Get
                Set(ByVal Value As System.String)
                    _ERPMATNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SerialNo As System.String
                Get
                    Return _SerialNo
                End Get
                Set(ByVal Value As System.String)
                    _SerialNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
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
            ''' Mandatory, Not Allow Null
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
            ''' Mandatory, Not Allow Null
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
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property WastePackage As System.String
                Get
                    Return _WastePackage
                End Get
                Set(ByVal Value As System.String)
                    _WastePackage = Value
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
            Public Property OperationDesc As System.String
                Get
                    Return _OperationDesc
                End Get
                Set(ByVal Value As System.String)
                    _OperationDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
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
            Public Property WasteComponent As System.String
                Get
                    Return _WasteComponent
                End Get
                Set(ByVal Value As System.String)
                    _WasteComponent = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OriginCode As System.String
                Get
                    Return _OriginCode
                End Get
                Set(ByVal Value As System.String)
                    _OriginCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OriginDescription As System.String
                Get
                    Return _OriginDescription
                End Get
                Set(ByVal Value As System.String)
                    _OriginDescription = Value
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
            Public Property CountryCode As System.String
                Get
                    Return _CountryCode
                End Get
                Set(ByVal Value As System.String)
                    _CountryCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PackagingQty As System.String
                Get
                    Return _PackagingQty
                End Get
                Set(ByVal Value As System.String)
                    _PackagingQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PackagingQtyKg As System.String
                Get
                    Return _PackagingQtyKg
                End Get
                Set(ByVal Value As System.String)
                    _PackagingQtyKg = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PackagingWeight As System.String
                Get
                    Return _PackagingWeight
                End Get
                Set(ByVal Value As System.String)
                    _PackagingWeight = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TreatmentCost As System.Decimal
                Get
                    Return _TreatmentCost
                End Get
                Set(ByVal Value As System.Decimal)
                    _TreatmentCost = Value
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
            Public Property RcvQty As System.Decimal
                Get
                    Return _RcvQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _RcvQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RcvPackQty As System.Decimal
                Get
                    Return _RcvPackQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _RcvPackQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PalletQty As System.Decimal
                Get
                    Return _PalletQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _PalletQty = Value
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
            Public Property AttnNote As System.String
                Get
                    Return _AttnNote
                End Get
                Set(ByVal Value As System.String)
                    _AttnNote = Value
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
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property AuthorisedDate As System.DateTime
                Get
                    Return _AuthorisedDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _AuthorisedDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property AuthorisedBy As System.String
                Get
                    Return _AuthorisedBy
                End Get
                Set(ByVal Value As System.String)
                    _AuthorisedBy = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property IssuedDate As System.DateTime
                Get
                    Return _IssuedDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _IssuedDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IssuedBy As System.String
                Get
                    Return _IssuedBy
                End Get
                Set(ByVal Value As System.String)
                    _IssuedBy = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property ReceivedDate As System.DateTime
                Get
                    Return _ReceivedDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _ReceivedDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ReceivedBy As System.String
                Get
                    Return _ReceivedBy
                End Get
                Set(ByVal Value As System.String)
                    _ReceivedBy = Value
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
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property CreateBy As System.String
                Get
                    Return _CreateBy
                End Get
                Set(ByVal Value As System.String)
                    _CreateBy = Value
                End Set
            End Property

            'Custom field
            Private _WasteName As System.String
            Private _PackageType As System.String

            Public Property WasteName As System.String
                Get
                    Return _WasteName
                End Get
                Set(ByVal Value As System.String)
                    _WasteName = Value
                End Set
            End Property

            Public Property PackageType As System.String
                Get
                    Return _PackageType
                End Get
                Set(ByVal Value As System.String)
                    _PackageType = Value
                End Set
            End Property

        End Class
#End Region

    End Namespace

#Region "Class Info"
    Public Class ConsigndtlInfo
        Inherits Core.CoreBase

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "TransID,ContractNo,SeqNo,StorageID,ERPMATNo,SerialNo,WasteCode,WasteDescription,WasteType,WastePackage,OperationType,TransInit,TermID,WasteComponent,OriginCode,OriginDescription,TariffCode,CountryCode,PackagingQty,PackagingQtyKg,PackagingWeight,TreatmentCost,RegistedSupp,Qty,PackQty,RcvQty,RcvPackQty,PalletQty,RecPackQty,AttnNote,Remark,AuthorisedDate,AuthorisedBy,IssuedDate,IssuedBy,ReceivedDate,ReceivedBy,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy,LastUpdate,UpdateBy,CreateDate,CreateBy"
                .CheckFields = "RegistedSupp,IsHost"
                .TableName = "Consigndtl WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "TransID,ContractNo,SeqNo,StorageID,ERPMATNo,SerialNo,WasteCode,WasteDescription,WasteType,WastePackage,OperationType,TransInit,TermID,WasteComponent,OriginCode,OriginDescription,TariffCode,CountryCode,PackagingQty,PackagingQtyKg,PackagingWeight,TreatmentCost,RegistedSupp,Qty,PackQty,RcvQty,RcvPackQty,PalletQty,RecPackQty,AttnNote,Remark,AuthorisedDate,AuthorisedBy,IssuedDate,IssuedBy,ReceivedDate,ReceivedBy,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy,LastUpdate,UpdateBy,CreateDate,CreateBy"
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
    Public Class ConsignDTLScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TransID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ContractNo"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "SeqNo"
                .Length = 4
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
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ERPMATNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "SerialNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WasteCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "WasteDescription"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WasteType"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WastePackage"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "OperationType"
                .Length = 3
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
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "TermID"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "WasteComponent"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "OriginCode"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "OriginDescription"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "TariffCode"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CountryCode"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PackagingQty"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PackagingQtyKg"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PackagingWeight"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "TreatmentCost"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "RegistedSupp"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Qty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "PackQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "RcvQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "RcvPackQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "PalletQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(27, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "RecPackQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(28, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "AttnNote"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
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
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "AuthorisedDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(31, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "AuthorisedBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(32, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "IssuedDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(33, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "IssuedBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(34, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "ReceivedDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(35, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ReceivedBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(36, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(37, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(38, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(39, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(40, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(41, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(42, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(43, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(44, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(45, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "OperationDesc"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(46, this)
        End Sub

        Public ReadOnly Property TransID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property ContractNo As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property

        Public ReadOnly Property SeqNo As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property

        Public ReadOnly Property StorageID As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property

        Public ReadOnly Property ERPMATNo As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property

        Public ReadOnly Property SerialNo As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property WasteCode As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property WasteDescription As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property WasteType As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property WastePackage As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property OperationType As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property TransInit As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property TermID As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property WasteComponent As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property OriginCode As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property OriginDescription As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property TariffCode As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property CountryCode As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property PackagingQty As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property PackagingQtyKg As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property PackagingWeight As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property TreatmentCost As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property RegistedSupp As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property Qty As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property PackQty As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property RcvQty As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property RcvPackQty As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property PalletQty As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property
        Public ReadOnly Property RecPackQty As StrucElement
            Get
                Return MyBase.GetItem(28)
            End Get
        End Property
        Public ReadOnly Property AttnNote As StrucElement
            Get
                Return MyBase.GetItem(29)
            End Get
        End Property
        Public ReadOnly Property Remark As StrucElement
            Get
                Return MyBase.GetItem(30)
            End Get
        End Property
        Public ReadOnly Property AuthorisedDate As StrucElement
            Get
                Return MyBase.GetItem(31)
            End Get
        End Property
        Public ReadOnly Property AuthorisedBy As StrucElement
            Get
                Return MyBase.GetItem(32)
            End Get
        End Property
        Public ReadOnly Property IssuedDate As StrucElement
            Get
                Return MyBase.GetItem(33)
            End Get
        End Property
        Public ReadOnly Property IssuedBy As StrucElement
            Get
                Return MyBase.GetItem(34)
            End Get
        End Property
        Public ReadOnly Property ReceivedDate As StrucElement
            Get
                Return MyBase.GetItem(35)
            End Get
        End Property
        Public ReadOnly Property ReceivedBy As StrucElement
            Get
                Return MyBase.GetItem(36)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(37)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(38)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(39)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(40)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(41)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(42)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(43)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(44)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(45)
            End Get
        End Property
        Public ReadOnly Property OperationDesc As StrucElement
            Get
                Return MyBase.GetItem(46)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace





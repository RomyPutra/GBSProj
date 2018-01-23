Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Actions
    Public NotInheritable Class ConsignLBL
        Inherits Core.CoreControl
        Private ConsignlabelInfo As ConsignlabelInfo = New ConsignlabelInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal ConsignlabelCont As Container.Consignlabel, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ConsignlabelCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ConsignlabelInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.ContractNo) & "' AND ActivedDate = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.ActivedDate) & "' AND LabelID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.LabelID) & "' AND LabelType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.LabelType) & "' AND AuthCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.AuthCode) & "'")
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
                                .TableName = "Consignlabel WITH (ROWLOCK)"
                                .AddField("GeneratorID", ConsignlabelCont.GeneratorID, SQLControl.EnumDataType.dtString)
                                .AddField("ReceiverID", ConsignlabelCont.ReceiverID, SQLControl.EnumDataType.dtString)
                                .AddField("SeqNo", ConsignlabelCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                .AddField("AuthCode", ConsignlabelCont.AuthCode, SQLControl.EnumDataType.dtString)
                                .AddField("TransferDate", ConsignlabelCont.TransferDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ExpiryDate", ConsignlabelCont.ExpiryDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ValidateDate", ConsignlabelCont.ValidateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ValidateBy", ConsignlabelCont.ValidateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Validated", ConsignlabelCont.Validated, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ShortNote", ConsignlabelCont.ShortNote, SQLControl.EnumDataType.dtString)
                                .AddField("Active", ConsignlabelCont.Active, SQLControl.EnumDataType.dtNumeric)
                                '.AddField("rowguid", ConsignlabelCont.rowguid, SQLControl.EnumDataType.dtString)
                                .AddField("IsHost", ConsignlabelCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", ConsignlabelCont.Flag, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.ContractNo) & "' AND ActivedDate = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtDateTime, ConsignlabelCont.ActivedDate) & "' AND LabelID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.LabelID) & "' AND LabelType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.LabelType) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("TransID", ConsignlabelCont.TransID, SQLControl.EnumDataType.dtString)
                                                .AddField("ContractNo", ConsignlabelCont.ContractNo, SQLControl.EnumDataType.dtString)
                                                .AddField("ActivedDate", ConsignlabelCont.ActivedDate, SQLControl.EnumDataType.dtString)
                                                .AddField("LabelID", ConsignlabelCont.LabelID, SQLControl.EnumDataType.dtString)
                                                .AddField("LabelType", ConsignlabelCont.LabelType, SQLControl.EnumDataType.dtNumeric)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.ContractNo) & "' AND ActivedDate = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.ActivedDate) & "' AND LabelID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.LabelID) & "' AND LabelType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.LabelType) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                    Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                                Else
                                    message = axExecute.Message.ToString()
                                    Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                                End If

                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignLBL", axExecute.Message & strSQL, axExecute.StackTrace)

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
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignLBL", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignLBL", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ConsignlabelCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal ConsignlabelCont As Container.Consignlabel, ByRef message As String) As Boolean
            Return Save(ConsignlabelCont, SQLControl.EnumSQLType.stInsert, message)
        End Function
        'Add List
        Public Function Insert(ByVal ListCNLblCont As List(Of Actions.Container.Consignlabel), ByRef message As String) As Boolean
            For Each CNLblCont In ListCNLblCont
                If Save(CNLblCont, SQLControl.EnumSQLType.stInsert, message) = False Then
                    Return False
                    Exit For
                End If
            Next
            Return True
        End Function

        'AMEND
        Public Function Update(ByVal ConsignlabelCont As Container.Consignlabel, ByRef message As String) As Boolean
            Return Save(ConsignlabelCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal ConsignlabelCont As Container.Consignlabel, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If ConsignlabelCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ConsignlabelInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.ContractNo) & "' AND ActivedDate = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtDateTime, ConsignlabelCont.ActivedDate) & "' AND LabelID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.LabelID) & "' AND LabelType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.LabelID) & "'")
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
                                strSQL = BuildUpdate("Consignlabel WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtDateTime, ConsignlabelCont.SyncLastUpd) & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignlabelCont.LastSyncBy) & "' WHERE" & _
                                "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.ContractNo) & "' AND ActivedDate = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtDateTime, ConsignlabelCont.ActivedDate) & "' AND LabelID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.LabelID) & "' AND LabelType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.LabelType) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("Consignlabel WITH (ROWLOCK)", "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.ContractNo) & "' AND ActivedDate = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtDateTime, ConsignlabelCont.ActivedDate) & "' AND LabelID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.LabelID) & "' AND LabelType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ConsignlabelCont.LabelType) & "'")
                        End If

                        Try
                            'execute
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignLBL", exExecute.Message & strSQL, exExecute.StackTrace)

                            Return False
                            'Throw New ApplicationException("210006 " & exExecute.Message.ToString())
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignLBL", axDelete.Message, axDelete.StackTrace)
                Return False
                'Throw axDelete
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignLBL", exDelete.Message, exDelete.StackTrace)
                Return False
                'Throw exDelete
            Finally
                ConsignlabelCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetConsignLBL(ByVal TransID As System.String, ByVal ContractNo As System.String, ByVal LabelID As System.String, ByVal PassKey As System.String) As Container.Consignlabel
            Dim rConsignlabel As Container.Consignlabel = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With ConsignlabelInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ContractNo) & "' AND LabelID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LabelID) & "' AND AuthCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, PassKey) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rConsignlabel = New Container.Consignlabel
                                rConsignlabel.TransID = drRow.Item("TransID")
                                rConsignlabel.ContractNo = drRow.Item("ContractNo")
                                rConsignlabel.ActivedDate = drRow.Item("ActivedDate")
                                rConsignlabel.LabelID = drRow.Item("LabelID")
                                rConsignlabel.LabelType = drRow.Item("LabelType")
                                rConsignlabel.GeneratorID = drRow.Item("GeneratorID")
                                rConsignlabel.ReceiverID = drRow.Item("ReceiverID")
                                rConsignlabel.SeqNo = drRow.Item("SeqNo")
                                rConsignlabel.AuthCode = drRow.Item("AuthCode")
                                rConsignlabel.ValidateBy = drRow.Item("ValidateBy")
                                rConsignlabel.Validated = drRow.Item("Validated")
                                rConsignlabel.ShortNote = drRow.Item("ShortNote")
                                rConsignlabel.Active = drRow.Item("Active")
                                rConsignlabel.rowguid = drRow.Item("rowguid")
                                rConsignlabel.SyncCreate = drRow.Item("SyncCreate")
                                rConsignlabel.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rConsignlabel.IsHost = drRow.Item("IsHost")
                                rConsignlabel.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rConsignlabel = Nothing
                            End If
                        Else
                            rConsignlabel = Nothing
                        End If
                    End With
                End If
                Return rConsignlabel
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignLBL", ex.Message & strSQL, ex.StackTrace)
                Throw ex
            Finally
                rConsignlabel = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetConsignLBL(ByVal TransID As System.String, ByVal ContractNo As System.String, ByVal ActivedDate As System.DateTime, ByVal LabelID As System.String, ByVal LabelType As System.Byte, ByVal DecendingOrder As Boolean) As List(Of Container.Consignlabel)
            Dim rConsignlabel As Container.Consignlabel = Nothing
            Dim lstConsignlabel As List(Of Container.Consignlabel) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With ConsignlabelInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal TransID As System.String, ByVal ContractNo As System.String, ByVal ActivedDate As System.DateTime, ByVal LabelID As System.String, ByVal LabelType As System.Byte DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "TransID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransID) & "' AND ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ContractNo) & "' AND ActivedDate = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtDateTime, ActivedDate) & "' AND LabelID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LabelID) & "' AND LabelType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LabelType) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rConsignlabel = New Container.Consignlabel
                                rConsignlabel.TransID = drRow.Item("TransID")
                                rConsignlabel.ContractNo = drRow.Item("ContractNo")
                                rConsignlabel.ActivedDate = drRow.Item("ActivedDate")
                                rConsignlabel.LabelID = drRow.Item("LabelID")
                                rConsignlabel.LabelType = drRow.Item("LabelType")
                                rConsignlabel.GeneratorID = drRow.Item("GeneratorID")
                                rConsignlabel.ReceiverID = drRow.Item("ReceiverID")
                                rConsignlabel.SeqNo = drRow.Item("SeqNo")
                                rConsignlabel.AuthCode = drRow.Item("AuthCode")
                                rConsignlabel.ValidateBy = drRow.Item("ValidateBy")
                                rConsignlabel.Validated = drRow.Item("Validated")
                                rConsignlabel.ShortNote = drRow.Item("ShortNote")
                                rConsignlabel.Active = drRow.Item("Active")
                                rConsignlabel.rowguid = drRow.Item("rowguid")
                                rConsignlabel.SyncCreate = drRow.Item("SyncCreate")
                                rConsignlabel.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rConsignlabel.IsHost = drRow.Item("IsHost")
                                rConsignlabel.LastSyncBy = drRow.Item("LastSyncBy")
                            Next
                            lstConsignlabel.Add(rConsignlabel)
                        Else
                            rConsignlabel = Nothing
                        End If
                        Return lstConsignlabel
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignLBL", ex.Message & strSQL, ex.StackTrace)

                Throw ex
            Finally
                rConsignlabel = Nothing
                lstConsignlabel = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetConsignLBLList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ConsignlabelInfo.MyInfo
                    If SQL = Nothing Or SQL = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName, FieldCond)
                    Else
                        strSQL = SQL
                    End If
                    Try
                        Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    Catch ex As Exception
                        Log.Notifier.Notify(ex)
                        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignLBL", ex.Message & strSQL, ex.StackTrace)
                        Throw
                    End Try
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetConsignLBLShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With ConsignlabelInfo.MyInfo
                    If ShortListing Then
                        strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Else
                        strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    End If
                    Try
                        Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    Catch ex As Exception
                        Log.Notifier.Notify(ex)
                        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignLBL", ex.Message & strSQL, ex.StackTrace)
                        Throw
                    End Try
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function DbFilterBackDateCN(ByRef sql As String, ByVal val As String, ByVal opt As String, Optional ByVal FieldCond As String = Nothing)

            Select Case opt
                Case "ValidateAmount"
                    sql = "SELECT ROW_NUMBER() OVER (ORDER BY LabelID asc) As NO, LabelID, CAST(CASE WHEN Validated = '0' THEN 'Not Validated' ELSE 'Validated' END AS char) As Validation " & _
                         "FROM CONSIGNLABEL WHERE CONTRACTNO = '" & val & "'"

                Case "UpdateScanBarcode"
                    sql = "SELECT ROW_NUMBER() OVER (ORDER BY LabelID asc) As NO, LabelID, CAST(CASE WHEN Validated = '0' THEN 'Not Validated' ELSE 'Validated' END AS varchar) As Validation " & _
                          "FROM CONSIGNLABEL WHERE CONTRACTNO = '" & val & "'"

                Case "LoadScanBarcode"
                    sql = "SELECT ROW_NUMBER() OVER (ORDER BY LabelID asc) As NO, LabelID, CAST(CASE WHEN Validated = '0' THEN 'Not Validated' ELSE 'Validated' END AS varchar) As Validation " & _
                          "FROM CONSIGNLABEL WHERE CONTRACTNO = '" & val & "' "

                Case "ValidatedSummary"
                    sql = "SELECT ROW_NUMBER() OVER (ORDER BY LabelID asc) As NO, LabelID, CAST(CASE WHEN Validated = '0' THEN 'Not Validated' ELSE 'Validated' END AS char) As Validation " & _
                          "FROM CONSIGNLABEL WHERE CONTRACTNO = '" & val & "'"


            End Select


            If StartConnection() = True Then
                With ConsignlabelInfo.MyInfo
                    If sql = Nothing Or sql = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName, FieldCond)
                    Else
                        strSQL = sql
                    End If
                    Try
                        Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    Catch ex As Exception
                        Log.Notifier.Notify(ex)
                        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/ConsignLBL", ex.Message & strSQL, ex.StackTrace)
                        Throw
                    End Try
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
        Public Class Consignlabel
            Public fTransID As System.String = "TransID"
            Public fContractNo As System.String = "ContractNo"
            Public fActivedDate As System.String = "ActivedDate"
            Public fLabelID As System.String = "LabelID"
            Public fLabelType As System.String = "LabelType"
            Public fGeneratorID As System.String = "GeneratorID"
            Public fReceiverID As System.String = "ReceiverID"
            Public fSeqNo As System.String = "SeqNo"
            Public fAuthCode As System.String = "AuthCode"
            Public fTransferDate As System.String = "TransferDate"
            Public fExpiryDate As System.String = "ExpiryDate"
            Public fValidateDate As System.String = "ValidateDate"
            Public fValidateBy As System.String = "ValidateBy"
            Public fValidated As System.String = "Validated"
            Public fShortNote As System.String = "ShortNote"
            Public fActive As System.String = "Active"
            Public fFlag As System.String = "Flag"
            Public frowguid As System.String = "rowguid"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fIsHost As System.String = "IsHost"
            Public fLastSyncBy As System.String = "LastSyncBy"

            Protected _TransID As System.String
            Protected _ContractNo As System.String
            Protected _ActivedDate As System.DateTime
            Protected _LabelID As System.String
            Protected _LabelType As System.Byte
            Private _GeneratorID As System.String
            Private _ReceiverID As System.String
            Private _SeqNo As System.Int32
            Private _AuthCode As System.String
            Private _TransferDate As System.DateTime
            Private _ExpiryDate As System.DateTime
            Private _ValidateDate As System.DateTime
            Private _ValidateBy As System.String
            Private _Validated As System.Byte
            Private _ShortNote As System.String
            Private _Active As System.Byte
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _IsHost As System.Byte
            Private _LastSyncBy As System.String
            Private _Flag As System.Byte

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
            Public Property ActivedDate As System.DateTime
                Get
                    Return _ActivedDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _ActivedDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property LabelID As System.String
                Get
                    Return _LabelID
                End Get
                Set(ByVal Value As System.String)
                    _LabelID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property LabelType As System.Byte
                Get
                    Return _LabelType
                End Get
                Set(ByVal Value As System.Byte)
                    _LabelType = Value
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
            Public Property AuthCode As System.String
                Get
                    Return _AuthCode
                End Get
                Set(ByVal Value As System.String)
                    _AuthCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property TransferDate As System.DateTime
                Get
                    Return _TransferDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _TransferDate = Value
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
            Public Property ValidateDate As System.DateTime
                Get
                    Return _ValidateDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _ValidateDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ValidateBy As System.String
                Get
                    Return _ValidateBy
                End Get
                Set(ByVal Value As System.String)
                    _ValidateBy = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Validated As System.Byte
                Get
                    Return _Validated
                End Get
                Set(ByVal Value As System.Byte)
                    _Validated = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ShortNote As System.String
                Get
                    Return _ShortNote
                End Get
                Set(ByVal Value As System.String)
                    _ShortNote = Value
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
        End Class
#End Region

    End Namespace

#Region "Class Info"
    Public Class ConsignlabelInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "TransID,ContractNo,ActivedDate,LabelID,LabelType,GeneratorID,ReceiverID,SeqNo,AuthCode,TransferDate,ExpiryDate,ValidateDate,ValidateBy,Validated,ShortNote,Active,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
                .CheckFields = "LabelType,Validated,Active,Flag,IsHost"
                .TableName = "Consignlabel WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "TransID,ContractNo,ActivedDate,LabelID,LabelType,GeneratorID,ReceiverID,SeqNo,AuthCode,TransferDate,ExpiryDate,ValidateDate,ValidateBy,Validated,ShortNote,Active,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
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
    Public Class ConsignLBLScheme
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
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "ActivedDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LabelID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LabelType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "GeneratorID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ReceiverID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "SeqNo"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "AuthCode"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "TransferDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "ExpiryDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "ValidateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ValidateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Validated"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ShortNote"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)

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
        Public ReadOnly Property ActivedDate As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property LabelID As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property LabelType As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property

        Public ReadOnly Property GeneratorID As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property ReceiverID As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property SeqNo As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property AuthCode As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property TransferDate As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property ExpiryDate As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property ValidateDate As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property ValidateBy As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property Validated As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property ShortNote As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
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
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace
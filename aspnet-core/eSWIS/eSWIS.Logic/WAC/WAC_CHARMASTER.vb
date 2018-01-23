
Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General


Namespace Actions
    Public NotInheritable Class WAC_CHARMASTER
        Inherits Core.CoreControl
        Private Wac_charmasterInfo As Wac_charmasterInfo = New Wac_charmasterInfo
        Private Wac_charscopeInfo As Wac_charscopeInfo = New Wac_charscopeInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal connecn As String)
            ConnectionString = connecn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal ListContWACHdr As List(Of Container.Wac_charmaster), ByVal ListContWACDtl As List(Of Container.Wac_charscope), ByVal ListContWACReceiver As List(Of Container.Wac_wrlist), ByVal pType As SQLControl.EnumSQLType, ByRef message As String, ByVal usedeleted As Boolean, Optional wastecode As String = "", Optional wastetype As String = "", Optional LocIDparam As String = "") As Boolean
            Dim strSQL As String = ""
            Dim ListSQL As ArrayList = New ArrayList()
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Dim BaseID As String = ""
            Dim WasCode As String = ""
            Dim WasType As String = ""
            Dim LocID As String = ""
            Try
                StartSQLControl()
                If ListContWACHdr IsNot Nothing AndAlso ListContWACHdr.Count > 0 Then
                    For Each Wac_charmasterCont In ListContWACHdr
                        blnExec = False
                        blnFound = False
                        blnFlag = False
                        If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                            StartSQLControl()
                            With Wac_charmasterInfo.MyInfo
                                strSQL = BuildSelect(.CheckFields, .TableName, "CompanyID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.CompanyID) & "' AND LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.LocID) & "' AND BaseID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Wac_charmasterCont.HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.DictionaryNo) & "'")
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
                                    Else
                                        blnFound = False
                                    End If
                                    .Close()
                                End With
                            End If
                        End If

                        If ListContWACHdr(0).Posted = 1 Then
                            strSQL = "Update Wac_charmaster set Flag = 0 WHERE BASEID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ListContWACHdr(0).BaseID) & "' AND CompanyID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ListContWACHdr(0).CompanyID) & "' AND LOCID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ListContWACHdr(0).LocID) & "' AND WasCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ListContWACHdr(0).WasCode) & "' AND WASTYPE='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ListContWACHdr(0).WasType) & "'"
                            ListSQL.Add(strSQL)
                        End If

                        If blnExec Then
                            StartSQLControl()
                            With objSQL
                                .TableName = "Wac_charmaster"
                                .AddField("HandlingName", Wac_charmasterCont.HandlingName, SQLControl.EnumDataType.dtStringN)
                                .AddField("DictionaryName", Wac_charmasterCont.DictionaryName, SQLControl.EnumDataType.dtStringN)
                                .AddField("State", Wac_charmasterCont.State, SQLControl.EnumDataType.dtString)
                                .AddField("PercentProduct", Wac_charmasterCont.PercentProduct, SQLControl.EnumDataType.dtNumeric)
                                .AddField("TotalComp", Wac_charmasterCont.TotalComp, SQLControl.EnumDataType.dtNumeric)
                                .AddField("PercentResidue", Wac_charmasterCont.PercentResidue, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Experiment", Wac_charmasterCont.Experiment, SQLControl.EnumDataType.dtNumeric)
                                .AddField("DataGroupNo", Wac_charmasterCont.DataGroupNo, SQLControl.EnumDataType.dtString)
                                .AddField("SpecNo", Wac_charmasterCont.SpecNo, SQLControl.EnumDataType.dtString, SEAL.Data.SQLControl.EnumValidate.cEmpty)
                                .AddField("DMSet", Wac_charmasterCont.DMSet, SQLControl.EnumDataType.dtString)
                                .AddField("DMOption", Wac_charmasterCont.DMOption, SQLControl.EnumDataType.dtString)
                                .AddField("Remark", Wac_charmasterCont.Remark, SQLControl.EnumDataType.dtString, SEAL.Data.SQLControl.EnumValidate.cEmpty)
                                .AddField("PeriodicComply", Wac_charmasterCont.PeriodicComply, SQLControl.EnumDataType.dtNumeric)
                                .AddField("PeriodicMonths", Wac_charmasterCont.PeriodicMonths, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastComplyDate", Wac_charmasterCont.LastComplyDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("PostDate", Wac_charmasterCont.PostDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Posted", Wac_charmasterCont.Posted, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Active", Wac_charmasterCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", Wac_charmasterCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", Wac_charmasterCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", Wac_charmasterCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", Wac_charmasterCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Path", Wac_charmasterCont.Path, SQLControl.EnumDataType.dtString)

                                BaseID = Wac_charmasterCont.BaseID
                                .AddField("CompanyID", Wac_charmasterCont.CompanyID, SQLControl.EnumDataType.dtString)
                                .AddField("LocID", Wac_charmasterCont.LocID, SQLControl.EnumDataType.dtString)
                                .AddField("BaseID", Wac_charmasterCont.BaseID, SQLControl.EnumDataType.dtString)
                                .AddField("WasCode", Wac_charmasterCont.WasCode, SQLControl.EnumDataType.dtString)
                                .AddField("WasType", Wac_charmasterCont.WasType, SQLControl.EnumDataType.dtString)
                                .AddField("HandlingType", Wac_charmasterCont.HandlingType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("HandlingCode", Wac_charmasterCont.HandlingCode, SQLControl.EnumDataType.dtString)
                                .AddField("DictionaryNo", Wac_charmasterCont.DictionaryNo, SQLControl.EnumDataType.dtString)
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            End With
                            'End If
                        End If
                        ListSQL.Add(strSQL)
                    Next
                End If
                If ListContWACDtl IsNot Nothing AndAlso ListContWACDtl.Count > 0 Then
                    If usedeleted = True Then
                        strSQL = "DELETE wac_charscope WITH (ROWLOCK) WHERE BASEID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ListContWACDtl(0).BaseID) & "' AND WasCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ListContWACDtl(0).WasCode) & "' AND WASTYPE='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ListContWACDtl(0).WasType) & "'"
                        ListSQL.Add(strSQL)
                    End If

                    For Each Wac_charscopeCont In ListContWACDtl
                        If ListContWACDtl Is Nothing Or ListContWACDtl.Count <= 0 Then
                            'Message return
                        Else
                            blnExec = False
                            blnFound = False
                            blnFlag = False
                            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                                StartSQLControl()
                                
                                With objSQL
                                    .TableName = "Wac_charscope"
                                    .AddField("SeqNo", Wac_charscopeCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPPROP", Wac_charscopeCont.CPPROP, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPBool", Wac_charscopeCont.ReactivityValue, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPFACTOR", Wac_charscopeCont.CPFACTOR, SQLControl.EnumDataType.dtString)
                                    .AddField("CPSGM", Wac_charscopeCont.CPSGM, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPMIN", Wac_charscopeCont.CPMIN, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPSGX", Wac_charscopeCont.CPSGX, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPMAX", Wac_charscopeCont.CPMAX, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPSMD", Wac_charscopeCont.CPSMD, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPMED", Wac_charscopeCont.CPMED, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPSAV", Wac_charscopeCont.CPSAV, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPAVG", Wac_charscopeCont.CPAVG, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("MethodNo", Wac_charscopeCont.MethodNo, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("UnitNo", Wac_charscopeCont.UnitNo, SQLControl.EnumDataType.dtString)
                                    .AddField("DeletedAttempt", Wac_charscopeCont.DeletedAttempt, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("DeletedReason", Wac_charscopeCont.DeletedReason, SQLControl.EnumDataType.dtStringN)
                                    .AddField("CHStep", Wac_charscopeCont.CHStep, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Dependency", Wac_charscopeCont.Dependency, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Mode", Wac_charscopeCont.Mode, SQLControl.EnumDataType.dtString)
                                    .AddField("ATMin", Wac_charscopeCont.ATMin, SQLControl.EnumDataType.dtString)
                                    .AddField("ATMax", Wac_charscopeCont.ATMax, SQLControl.EnumDataType.dtString)
                                    .AddField("ATNX", Wac_charscopeCont.ATNX, SQLControl.EnumDataType.dtString)
                                    .AddField("ATTR", Wac_charscopeCont.ATTR, SQLControl.EnumDataType.dtString)
                                    .AddField("DDGroup", Wac_charscopeCont.DDGroup, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Active", Wac_charscopeCont.Active, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CreateDate", Wac_charscopeCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateBy", Wac_charscopeCont.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("LastUpdate", Wac_charscopeCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("UpdateBy", Wac_charscopeCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("ConvertMin", Wac_charscopeCont.ConvertMin, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ConvertMax", Wac_charscopeCont.ConvertMax, SQLControl.EnumDataType.dtNumeric)
                                    
                                    If blnFound = False Then
                                        .AddField("LocID", Wac_charscopeCont.LocID, SQLControl.EnumDataType.dtString)
                                        .AddField("BaseID", Wac_charscopeCont.BaseID, SQLControl.EnumDataType.dtString)
                                        .AddField("WasCode", Wac_charscopeCont.WasCode, SQLControl.EnumDataType.dtString)
                                        .AddField("WasType", Wac_charscopeCont.WasType, SQLControl.EnumDataType.dtString)
                                        .AddField("HandlingType", Wac_charscopeCont.HandlingType, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("HandlingCode", Wac_charscopeCont.HandlingCode, SQLControl.EnumDataType.dtString)
                                        .AddField("DictionaryNo", Wac_charscopeCont.DictionaryNo, SQLControl.EnumDataType.dtString)
                                        .AddField("COMPCode", Wac_charscopeCont.COMPCode, SQLControl.EnumDataType.dtNumeric)
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                    End If
                                    
                                End With
                            End If
                        End If
                        ListSQL.Add(strSQL)
                    Next
                End If


                If Not ListContWACReceiver Is Nothing AndAlso ListContWACReceiver.Count > 0 Then
                    For i As Integer = 0 To ListContWACReceiver.Count - 1
                        strSQL = "UPDATE WAC_WRLIST SET ACTIVE=1, FLAG=1, ISSELECTED='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ListContWACReceiver(i).IsSelected) & "' WHERE RECEIVERLOCID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ListContWACReceiver(i).ReceiverLocID) & "' AND WASCODE='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ListContWACReceiver(i).WasCode) & "' AND WASTYPE='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ListContWACReceiver(i).WasType) & "' AND GENERATORLOCID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ListContWACReceiver(i).GeneratorLocID) & "'"
                        ListSQL.Add(strSQL)
                    Next
                End If

                If wastecode <> "" AndAlso wastetype <> "" Then
                    LocID = LocIDparam
                    WasCode = wastecode
                    WasType = wastetype
                Else
                    LocID = ListContWACHdr(0).LocID
                    WasCode = ListContWACHdr(0).WasCode
                    WasType = ListContWACHdr(0).WasType
                End If

                If ListContWACHdr IsNot Nothing AndAlso ListContWACHdr.Count > 0 Then
                    If ListContWACHdr(0).Posted = 1 Then
                        strSQL = "EXEC [WAC_QualifiedWR_WG] '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, LocID) & "','','" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, WasCode) & "','" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, WasType) & "','',''"
                        ListSQL.Add(strSQL)
                    End If
                Else
                    strSQL = "EXEC [WAC_QualifiedWR_WG] '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, LocID) & "','','" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, WasCode) & "','" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, WasType) & "','',''"
                    ListSQL.Add(strSQL)
                End If


                Try
                    'execute
                    objConn.BatchExecute(ListSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                Catch axExecute As Exception
                    If pType = SQLControl.EnumSQLType.stInsert Then
                        message = axExecute.Message.ToString()
                    Else
                        message = axExecute.Message.ToString()
                    End If

                    Dim sqlStatement As String = " "
                    If objConn.FailedSQLStatement.Count > 0 Then
                        sqlStatement &= objConn.FailedSQLStatement.Item(0)
                    End If

                    Log.Notifier.Notify(axExecute)
                    Gibraltar.Agent.Log.Error("WAC_CHARMASTER", axExecute.Message & sqlStatement, axExecute.StackTrace)
                    Return False
                Finally
                    objSQL.Dispose()
                End Try
                Return True
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Return False
            Finally
                ListContWACHdr = Nothing
                ListContWACDtl = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal ListContWACHdr As List(Of Container.Wac_charmaster), ByVal ListContWACDtl As List(Of Container.Wac_charscope), ByVal ListContWACReceiver As List(Of Container.Wac_wrlist), ByRef message As String, Optional wastecode As String = "", Optional wastetype As String = "", Optional locID As String = "", Optional ByVal usedeleted As Boolean = True) As Boolean
            Return Save(ListContWACHdr, ListContWACDtl, Nothing, SQLControl.EnumSQLType.stInsert, message, usedeleted, wastecode, wastetype, locID)
        End Function

        'AMEND
        Public Function Update(ByVal ListContWACHdr As List(Of Container.Wac_charmaster), ByVal ListContWACDtl As List(Of Container.Wac_charscope), ByVal ListContWACReceiver As List(Of Container.Wac_wrlist), ByRef message As String, Optional wastecode As String = "", Optional wastetype As String = "", Optional locID As String = "", Optional ByVal usedeleted As Boolean = True) As Boolean
            Return Save(ListContWACHdr, ListContWACDtl, ListContWACReceiver, SQLControl.EnumSQLType.stUpdate, message, usedeleted, wastecode, wastetype, locID)
        End Function

        Public Function Delete(ByVal Wac_charmasterCont As Container.Wac_charmaster, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Wac_charmasterCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Wac_charmasterInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "CompanyID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.CompanyID) & "' AND LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.LocID) & "' AND BaseID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Wac_charmasterCont.HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.DictionaryNo) & "'")
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
                                strSQL = BuildUpdate(Wac_charmasterInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = GETDATE() , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_charmasterCont.UpdateBy) & "' WHERE " & _
                                "CompanyID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.CompanyID) & "' AND LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.LocID) & "' AND BaseID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Wac_charmasterCont.HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.DictionaryNo) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Wac_charmasterInfo.MyInfo.TableName, "CompanyID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.CompanyID) & "' AND LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.LocID) & "' AND BaseID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Wac_charmasterCont.HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Wac_charmasterCont.DictionaryNo) & "'")
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
                Wac_charmasterCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetWAC_CHARMASTERByDictionaryNo(ByVal DictionaryNo As System.String) As Container.Wac_charmaster
            Dim rWac_charmaster As Container.Wac_charmaster = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With Wac_charmasterInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "DictionaryNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, DictionaryNo) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWac_charmaster = New Container.Wac_charmaster
                                rWac_charmaster.CompanyID = drRow.Item("CompanyID")
                                rWac_charmaster.LocID = drRow.Item("LocID")
                                rWac_charmaster.BaseID = drRow.Item("BaseID")
                                rWac_charmaster.WasCode = drRow.Item("WasCode")
                                rWac_charmaster.WasType = drRow.Item("WasType")
                                rWac_charmaster.HandlingType = drRow.Item("HandlingType")
                                rWac_charmaster.HandlingCode = drRow.Item("HandlingCode")
                                rWac_charmaster.DictionaryNo = drRow.Item("DictionaryNo")
                                rWac_charmaster.HandlingName = drRow.Item("HandlingName")
                                rWac_charmaster.DictionaryName = drRow.Item("DictionaryName")
                                rWac_charmaster.State = drRow.Item("State")
                                rWac_charmaster.PercentProduct = drRow.Item("PercentProduct")
                                rWac_charmaster.PercentResidue = drRow.Item("PercentResidue")
                                rWac_charmaster.Experiment = drRow.Item("Experiment")
                                rWac_charmaster.DataGroupNo = drRow.Item("DataGroupNo")
                                rWac_charmaster.SpecNo = drRow.Item("SpecNo")
                                rWac_charmaster.DMSet = drRow.Item("DMSet")
                                rWac_charmaster.DMOption = drRow.Item("DMOption")
                                rWac_charmaster.Remark = drRow.Item("Remark")
                                rWac_charmaster.PeriodicComply = drRow.Item("PeriodicComply")
                                rWac_charmaster.PeriodicMonths = drRow.Item("PeriodicMonths")
                                rWac_charmaster.Posted = drRow.Item("Posted")
                                rWac_charmaster.Active = drRow.Item("Active")
                                rWac_charmaster.CreateBy = drRow.Item("CreateBy")
                                rWac_charmaster.UpdateBy = drRow.Item("UpdateBy")
                                rWac_charmaster.Path = drRow.Item("Path")

                            Else
                                rWac_charmaster = Nothing
                            End If
                        Else
                            rWac_charmaster = Nothing
                        End If
                    End With
                End If
                Return rWac_charmaster
            Catch ex As Exception
                Throw ex
            Finally
                rWac_charmaster = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_CHARMASTER(ByVal CompanyID As System.String, ByVal LocID As System.String, ByVal BaseID As System.String, ByVal WasCode As System.String, ByVal WasType As System.String, ByVal HandlingType As System.Int32, ByVal HandlingCode As System.String, ByVal DictionaryNo As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Wac_charmaster)
            Dim rWac_charmaster As Container.Wac_charmaster = Nothing
            Dim lstWac_charmaster As List(Of Container.Wac_charmaster) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With Wac_charmasterInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal CompanyID As System.String, ByVal LocID As System.String, ByVal BaseID As System.String, ByVal WasCode As System.String, ByVal WasType As System.String, ByVal HandlingType As System.Int32, ByVal HandlingCode As System.String, ByVal DictionaryNo As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "CompanyID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompanyID) & "' AND LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND BaseID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasCode) & "' AND WasType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, DictionaryNo) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rWac_charmaster = New Container.Wac_charmaster
                                rWac_charmaster.CompanyID = drRow.Item("CompanyID")
                                rWac_charmaster.LocID = drRow.Item("LocID")
                                rWac_charmaster.BaseID = drRow.Item("BaseID")
                                rWac_charmaster.WasCode = drRow.Item("WasCode")
                                rWac_charmaster.WasType = drRow.Item("WasType")
                                rWac_charmaster.HandlingType = drRow.Item("HandlingType")
                                rWac_charmaster.HandlingCode = drRow.Item("HandlingCode")
                                rWac_charmaster.DictionaryNo = drRow.Item("DictionaryNo")
                                rWac_charmaster.HandlingName = drRow.Item("HandlingName")
                                rWac_charmaster.DictionaryName = drRow.Item("DictionaryName")
                                rWac_charmaster.State = drRow.Item("State")
                                rWac_charmaster.PercentProduct = drRow.Item("PercentProduct")
                                rWac_charmaster.PercentResidue = drRow.Item("PercentResidue")
                                rWac_charmaster.Experiment = drRow.Item("Experiment")
                                rWac_charmaster.DataGroupNo = drRow.Item("DataGroupNo")
                                rWac_charmaster.SpecNo = drRow.Item("SpecNo")
                                rWac_charmaster.DMSet = drRow.Item("DMSet")
                                rWac_charmaster.DMOption = drRow.Item("DMOption")
                                rWac_charmaster.Remark = drRow.Item("Remark")
                                rWac_charmaster.PeriodicComply = drRow.Item("PeriodicComply")
                                rWac_charmaster.PeriodicMonths = drRow.Item("PeriodicMonths")
                                rWac_charmaster.Posted = drRow.Item("Posted")
                                rWac_charmaster.Active = drRow.Item("Active")
                                rWac_charmaster.CreateBy = drRow.Item("CreateBy")
                                rWac_charmaster.UpdateBy = drRow.Item("UpdateBy")
                                rWac_charmaster.Path = drRow.Item("Path")

                            Next
                            lstWac_charmaster.Add(rWac_charmaster)
                        Else
                            rWac_charmaster = Nothing
                        End If
                        Return lstWac_charmaster
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rWac_charmaster = Nothing
                lstWac_charmaster = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_CHARMASTERByWasteCode(ByVal WasteCode As System.String, ByVal WasteType As System.String) As List(Of Container.Wac_charmaster)
            Dim rWac_charmaster As Container.Wac_charmaster = Nothing
            Dim lstWac_charmaster As List(Of Container.Wac_charmaster) = New List(Of Container.Wac_charmaster)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With Wac_charmasterInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "WasCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "' AND WasType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rWac_charmaster = New Container.Wac_charmaster
                                rWac_charmaster.CompanyID = drRow.Item("CompanyID")
                                rWac_charmaster.LocID = drRow.Item("LocID")
                                rWac_charmaster.BaseID = drRow.Item("BaseID")
                                rWac_charmaster.WasCode = drRow.Item("WasCode")
                                rWac_charmaster.WasType = drRow.Item("WasType")
                                rWac_charmaster.HandlingType = drRow.Item("HandlingType")
                                rWac_charmaster.HandlingCode = drRow.Item("HandlingCode")
                                rWac_charmaster.DictionaryNo = drRow.Item("DictionaryNo")
                                rWac_charmaster.HandlingName = drRow.Item("HandlingName")
                                rWac_charmaster.DictionaryName = drRow.Item("DictionaryName")
                                rWac_charmaster.State = drRow.Item("State")
                                rWac_charmaster.PercentProduct = drRow.Item("PercentProduct")
                                rWac_charmaster.PercentResidue = drRow.Item("PercentResidue")
                                rWac_charmaster.Experiment = drRow.Item("Experiment")
                                rWac_charmaster.DataGroupNo = drRow.Item("DataGroupNo")
                                rWac_charmaster.SpecNo = drRow.Item("SpecNo")
                                rWac_charmaster.DMSet = drRow.Item("DMSet")
                                rWac_charmaster.DMOption = drRow.Item("DMOption")
                                rWac_charmaster.Remark = drRow.Item("Remark")
                                rWac_charmaster.PeriodicComply = drRow.Item("PeriodicComply")
                                rWac_charmaster.PeriodicMonths = drRow.Item("PeriodicMonths")
                                rWac_charmaster.Posted = drRow.Item("Posted")
                                rWac_charmaster.Active = drRow.Item("Active")
                                rWac_charmaster.CreateBy = drRow.Item("CreateBy")
                                rWac_charmaster.UpdateBy = drRow.Item("UpdateBy")
                                rWac_charmaster.Path = drRow.Item("Path")
                                lstWac_charmaster.Add(rWac_charmaster)
                            Next
                        Else
                            rWac_charmaster = Nothing
                        End If
                        Return lstWac_charmaster
                    End With
                End If
            Catch ex As Exception
                Throw ex
                Return Nothing
            Finally
                rWac_charmaster = Nothing
                lstWac_charmaster = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetACCMASTERByWasteCode(ByVal WasCode As System.String, ByVal WasType As System.String) As DataTable
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With Wac_charmasterInfo.MyInfo
                        strSQL = "select distinct CompanyID,LocID,state,DictionaryNo from WAC_CHARMASTER" &
                                " where WasCode = '" & WasCode & "' and WasType = '" & WasType & "'"
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    End With
                End If
                Return dtTemp
            Catch ex As Exception
                Throw ex
            Finally
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_CHARMASTERList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Wac_charmasterInfo.MyInfo
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

        Public Overloads Function GetWAC_CHARMASTERCustomList(Optional ByVal LocID As String = "") As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_charmasterInfo.MyInfo
                    strSQL = "select distinct ISNULL(ch.DictionaryNo,'') As DictionaryNo, max(h.TransNo) as TransNo, d.ItemCode As WasCode, c.Code As WasType, c.CodeDesc As WasTypeDesc, ch.SpecNo, '' As DictionaryName, ch.CreateBy, ch.CreateDate as PostDate, CASE WHEN ch.CreateBy IS NULL THEN 'Pending Assign' WHEN tb.cnt >= 1 THEN 'Assigned' ELSE 'Pending Assign' end ComponentStatus, b.BaseID, case when h.Active=1 AND h.Flag=1 AND h.Posted=1 then 'Active' else 'InActive' end as status " & _
                        " from Notifydtl d  WITH (NOLOCK) INNER JOIN CODEMASTER c ON c.code = d.TypeCode AND c.CodeType = 'WTY'" & _
                        " INNER JOIN WAC_BASELINE b  WITH (NOLOCK) ON d.ItemCode = b.WasCode AND b.WasType = c.Code" & _
                        " INNER JOIN NOTIFYHDR h  WITH (NOLOCK) on d.TransNo = h.TransNo AND h.Active=1 AND h.Flag=1 AND h.Posted=1" & _
                        " OUTER APPLY (SELECT TOP 1 * FROM WAC_CHARMASTER ch  WITH (NOLOCK) WHERE d.ItemCode = ch.WasCode AND ch.WasType = c.Code and ch.Flag=1 and ch.LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' ORDER BY PostDate DESC) ch" & _
                        " OUTER APPLY (SELECT COUNT(Posted) cnt FROM WAC_CHARMASTER cm where cm.locid = ch.locid and cm.baseid = ch.baseID and cm.wascode = ch.wascode and cm.wastype = ch.wastype and cm.Posted = 1) tb " & _
                        " WHERE d.LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "'" & _
                        " group by ch.DictionaryNo, d.ItemCode, c.Code, c.CodeDesc, ch.SpecNo, ch.CreateBy, ch.CreateDate,b.BaseID,ch.posted ,TB.CNT, h.Active,h.Flag,h.Posted "
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWAC_CHARMASTERByLoc(Optional ByVal CompanyID As String = "") As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_charmasterInfo.MyInfo
                    strSQL = "SELECT TOP 1 CompanyID " & _
                        " FROM WAC_CHARMASTER wa WITH (NOLOCK)" & _
                        " WHERE wa.Flag=1 AND wa.CompanyID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompanyID) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWac_CHARMASTERListByDictionaryNo(ByVal DictNo As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_charmasterInfo.MyInfo
                    strSQL = "Select ch.*, d.itmName, wb.TestingWaste, CodeDesc, case when ch.Posted = 0 and  tb.cnt>= 1 then 2 else ch.Posted end as postedstatus, IIF(DMSet = 0 ,'Other', wp.ProductDesc) as ProductDesc FROM wac_charmaster ch WITH (NOLOCK) LEFT JOIN NotifyDTL d WITH (NOLOCK) on ch.CompanyID = d.CompanyID AND ch.LocID = d.LocID AND ch.WasCode = d.ItemCode Left Join WAC_BASELINE wb on ch.BaseID = wb.BaseID and ch.WasCode=wb.WasCode and ch.WasType = wb.WasType " &
                        " Left join CODEMASTER wc WITH (NOLOCK) on ch.HandlingType = wc.Code and CodeType='WHY'" &
                        " Left join WAC_WASPRODUCT wp WITH (NOLOCK) on ch.DMSet = wp.ProductCode and ch.WasCode=wp.WasCode AND ch.WasType=wp.WasType " &
                        " CROSS APPLY (SELECT COUNT(Posted) cnt FROM WAC_CHARMASTER cm where cm.locid = ch.locid and cm.baseid = ch.baseID and cm.wascode = ch.wascode and cm.wastype = ch.wastype and cm.Posted = 1) tb  " &
                        " WHERE DictionaryNo='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, DictNo) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetSummaryCharacteristicList(Optional ByVal WasteCode As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_charmasterInfo.MyInfo
                    strSQL = "SELECT a.WasCode, a.WasType, b.CodeDesc AS WasTypeDesc, ISNULL(c.BranchName,'') AS PremiseName, count(Distinct a.DictionaryNo) AS AttempNo, ISNULL(max(a.LastUpdate),max(a.CreateDate)) AS LastUpdated, a.LocID " & _
                             "FROM wac_charmaster a WITH (NOLOCK) INNER JOIN CODEMASTER b WITH (NOLOCK) ON a.WasType=b.Code AND b.CodeType='WTY' LEFT JOIN BIZLOCATE c WITH(NOLOCK) ON a.LocID = c.Bizlocid "
                    If Not WasteCode Is Nothing Then
                        strSQL &= " WHERE a.WasCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "' and a.posted = 1"
                    Else
                        strSQL &= " WHERE a.posted = 1 "
                    End If
                    strSQL &= " GROUP BY a.LocID, a.WasCode, a.WasType, b.CodeDesc, c.BranchName"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

#End Region
    End Class


    Namespace Container
#Region "Class Container"
        Public Class Wac_charmaster
            Public fCompanyID As System.String = "CompanyID"
            Public fLocID As System.String = "LocID"
            Public fBaseID As System.String = "BaseID"
            Public fWasCode As System.String = "WasCode"
            Public fWasType As System.String = "WasType"
            Public fHandlingType As System.String = "HandlingType"
            Public fHandlingCode As System.String = "HandlingCode"
            Public fDictionaryNo As System.String = "DictionaryNo"
            Public fHandlingName As System.String = "HandlingName"
            Public fDictionaryName As System.String = "DictionaryName"
            Public fState As System.String = "State"
            Public fPercentProduct As System.String = "PercentProduct"
            Public fPercentResidue As System.String = "PercentResidue"
            Public fExperiment As System.String = "Experiment"
            Public fDataGroupNo As System.String = "DataGroupNo"
            Public fSpecNo As System.String = "SpecNo"
            Public fDMSet As System.String = "DMSet"
            Public fDMOption As System.String = "DMOption"
            Public fRemark As System.String = "Remark"
            Public fPeriodicComply As System.String = "PeriodicComply"
            Public fPeriodicMonths As System.String = "PeriodicMonths"
            Public fLastComplyDate As System.String = "LastComplyDate"
            Public fPostDate As System.String = "PostDate"
            Public fPosted As System.String = "Posted"
            Public fActive As System.String = "Active"
            Public fFlag As System.String = "Flag"
            Public fCreateDate As System.String = "CreateDate"
            Public fCreateBy As System.String = "CreateBy"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fUpdateBy As System.String = "UpdateBy"
            Public fTreatmentName As System.String = "TreatmentName"
            Public fWasteTypeDesc As System.String = "WasteType"
            Public fPath As System.String = "Path"
            Public fTotalComp As System.String = "TotalComp"


            Protected _CompanyID As System.String
            Protected _LocID As System.String
            Protected _BaseID As System.String
            Protected _WasCode As System.String
            Protected _WasType As System.String
            Protected _HandlingType As System.Int32
            Protected _HandlingCode As System.String
            Protected _DictionaryNo As System.String
            Private _HandlingName As System.String
            Private _DictionaryName As System.String
            Private _State As System.String
            Public _Flag As System.String
            Private _PercentProduct As System.Int32
            Private _PercentResidue As System.Int32
            Private _Experiment As System.Byte
            Private _DataGroupNo As System.String
            Private _SpecNo As System.String
            Private _DMSet As System.String
            Private _DMOption As System.String
            Private _Remark As System.String
            Private _PeriodicComply As System.Byte
            Private _PeriodicMonths As System.Int32
            Private _LastComplyDate As System.DateTime
            Private _PostDate As System.DateTime
            Private _Posted As System.Byte
            Private _Active As System.Byte
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _TreatmentName As System.String
            Public _WasteTypeDesc As System.String
            Public _Code As System.String
            Public _Path As System.String
            Public _TotalComp As System.Int32


            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public ReadOnly Property WasteTypeDesc As System.String
                Get
                    Return _WasteTypeDesc
                End Get
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public ReadOnly Property TreatmentName As System.String
                Get
                    Return _TreatmentName
                End Get
            End Property
            ''' <summary>
            ''' Mandatory
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
            ''' Mandatory
            ''' </summary>
            Public Property TotalComp As System.Int32
                Get
                    Return _TotalComp
                End Get
                Set(ByVal Value As System.Int32)
                    _TotalComp = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property Code As System.String
                Get
                    Return _Code
                End Get
                Set(ByVal Value As System.String)
                    _Code = Value
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
            ''' Mandatory
            ''' </summary>
            Public Property BaseID As System.String
                Get
                    Return _BaseID
                End Get
                Set(ByVal Value As System.String)
                    _BaseID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property WasCode As System.String
                Get
                    Return _WasCode
                End Get
                Set(ByVal Value As System.String)
                    _WasCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property WasType As System.String
                Get
                    Return _WasType
                End Get
                Set(ByVal Value As System.String)
                    _WasType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property Flag As System.String
                Get
                    Return _Flag
                End Get
                Set(ByVal Value As System.String)
                    _Flag = Value
                End Set
            End Property


            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property HandlingType As System.Int32
                Get
                    Return _HandlingType
                End Get
                Set(ByVal Value As System.Int32)
                    _HandlingType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property HandlingCode As System.String
                Get
                    Return _HandlingCode
                End Get
                Set(ByVal Value As System.String)
                    _HandlingCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property DictionaryNo As System.String
                Get
                    Return _DictionaryNo
                End Get
                Set(ByVal Value As System.String)
                    _DictionaryNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property Path As System.String
                Get
                    Return _Path
                End Get
                Set(ByVal Value As System.String)
                    _Path = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property HandlingName As System.String
                Get
                    Return _HandlingName
                End Get
                Set(ByVal Value As System.String)
                    _HandlingName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property DictionaryName As System.String
                Get
                    Return _DictionaryName
                End Get
                Set(ByVal Value As System.String)
                    _DictionaryName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property State As System.String
                Get
                    Return _State
                End Get
                Set(ByVal Value As System.String)
                    _State = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PercentProduct As System.Int32
                Get
                    Return _PercentProduct
                End Get
                Set(ByVal Value As System.Int32)
                    _PercentProduct = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PercentResidue As System.Int32
                Get
                    Return _PercentResidue
                End Get
                Set(ByVal Value As System.Int32)
                    _PercentResidue = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Experiment As System.Byte
                Get
                    Return _Experiment
                End Get
                Set(ByVal Value As System.Byte)
                    _Experiment = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property DataGroupNo As System.String
                Get
                    Return _DataGroupNo
                End Get
                Set(ByVal Value As System.String)
                    _DataGroupNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SpecNo As System.String
                Get
                    Return _SpecNo
                End Get
                Set(ByVal Value As System.String)
                    _SpecNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property DMSet As System.String
                Get
                    Return _DMSet
                End Get
                Set(ByVal Value As System.String)
                    _DMSet = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property DMOption As System.String
                Get
                    Return _DMOption
                End Get
                Set(ByVal Value As System.String)
                    _DMOption = Value
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
            Public Property PeriodicComply As System.Byte
                Get
                    Return _PeriodicComply
                End Get
                Set(ByVal Value As System.Byte)
                    _PeriodicComply = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PeriodicMonths As System.Int32
                Get
                    Return _PeriodicMonths
                End Get
                Set(ByVal Value As System.Int32)
                    _PeriodicMonths = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property LastComplyDate As System.DateTime
                Get
                    Return _LastComplyDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _LastComplyDate = Value
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
            Public Property Active As System.Byte
                Get
                    Return _Active
                End Get
                Set(ByVal Value As System.Byte)
                    _Active = Value
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

        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class Wac_charmasterInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "CompanyID,LocID,BaseID,WasCode,WasType,HandlingType,HandlingCode,DictionaryNo,HandlingName,DictionaryName,State,PercentProduct,PercentResidue,Experiment,DataGroupNo,SpecNo,DMSet,DMOption,Remark,PeriodicComply,PeriodicMonths,LastComplyDate,PostDate,Posted,Active,Flag,CreateDate,CreateBy,LastUpdate,UpdateBy,Path"
                .CheckFields = "Experiment,PeriodicComply,Posted,Active,Flag"
                .TableName = "Wac_charmaster"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "CompanyID,LocID,BaseID,WasCode,WasType,HandlingType,HandlingCode,DictionaryNo,HandlingName,DictionaryName,State,PercentProduct,PercentResidue,Experiment,DataGroupNo,SpecNo,DMSet,DMOption,Remark,PeriodicComply,PeriodicMonths,LastComplyDate,PostDate,Posted,Active,Flag,CreateDate,CreateBy,LastUpdate,UpdateBy,Path"
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
    Public Class WAC_CHARMASTERScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CompanyID"
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
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BaseID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WasCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WasType"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "HandlingType"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "HandlingCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "DictionaryNo"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "HandlingName"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "DictionaryName"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "State"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "PercentProduct"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "PercentResidue"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Experiment"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "DataGroupNo"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "SpecNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "DMSet"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "DMOption"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "PeriodicComply"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "PeriodicMonths"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastComplyDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "PostDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Posted"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
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
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(29, this)

        End Sub

        Public ReadOnly Property CompanyID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property LocID As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property BaseID As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property WasCode As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property WasType As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property HandlingType As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property HandlingCode As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property DictionaryNo As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property

        Public ReadOnly Property HandlingName As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property DictionaryName As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property State As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property PercentProduct As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property PercentResidue As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property Experiment As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property DataGroupNo As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property SpecNo As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property DMSet As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property DMOption As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property Remark As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property PeriodicComply As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property PeriodicMonths As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property LastComplyDate As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property PostDate As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property Posted As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
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

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace


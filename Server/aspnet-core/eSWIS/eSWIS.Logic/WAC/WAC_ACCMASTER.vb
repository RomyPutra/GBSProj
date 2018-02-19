Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Actions
    Public NotInheritable Class WAC_ACCMASTER
        Inherits Core.CoreControl
        Private Wac_accmasterInfo As Wac_accmasterInfo = New Wac_accmasterInfo
        Private Wac_accscopeInfo As Wac_accscopeInfo = New Wac_accscopeInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal connecn As String)
            ConnectionString = connecn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal ListContWACHdr As List(Of Container.Wac_accmaster), ByVal ListContWACDtl As List(Of Container.Wac_accscope), ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional updateBaseline As Boolean = False, Optional usedeleted As Boolean = True, Optional wastecode As String = "", Optional wastetype As String = "", Optional locIDparam As String = "") As Boolean
            Dim strSQL As String = ""
            Dim ListSQL As ArrayList = New ArrayList()
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Dim DictionaryNo As String = ""
            Dim WasCode As String = ""
            Dim WasType As String = ""
            Dim LocID As String = ""
            Dim CompanyID As String = ""
            Try
                StartSQLControl()
                If ListContWACHdr Is Nothing OrElse ListContWACHdr.Count <= 0 Then
                    strSQL = "Delete from Wac_accmaster WHERE LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, locIDparam) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, wastecode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, wastetype) & "'"
                    ListSQL.Add(strSQL)

                Else
                    StartSQLControl()
                    With Wac_accmasterInfo.MyInfo
                        If ListContWACHdr(0).Posted <> 0 Then
                            strSQL = "Update Wac_accmaster SET Flag = 0 WHERE CompanyID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ListContWACHdr(0).CompanyID) & "' AND LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ListContWACHdr(0).LocID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ListContWACHdr(0).WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ListContWACHdr(0).WasType) & "'"
                            ListSQL.Add(strSQL)
                        End If
                    End With

                    For Each Wac_accmasterCont In ListContWACHdr
                        blnExec = False
                        blnFound = False
                        blnFlag = False
                        If StartConnection(EnumIsoState.StateUpdatetable) = True Then

                            With Wac_accmasterInfo.MyInfo
                                strSQL = BuildSelect(.CheckFields, .TableName, "CompanyID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.CompanyID) & "' AND LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.LocID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.WasType) & "' AND HandlingCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.HandlingCode) & "'")
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

                        If blnExec Then
                            StartSQLControl()
                            With objSQL
                                .TableName = "Wac_accmaster"
                                .AddField("HandlingName", Wac_accmasterCont.HandlingName, SQLControl.EnumDataType.dtStringN)
                                .AddField("DictionaryName", Wac_accmasterCont.DictionaryName, SQLControl.EnumDataType.dtStringN)
                                .AddField("State", Wac_accmasterCont.State, SQLControl.EnumDataType.dtString)
                                .AddField("PercentProduct", Wac_accmasterCont.PercentProduct, SQLControl.EnumDataType.dtNumeric)
                                .AddField("PercentResidue", Wac_accmasterCont.PercentResidue, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Experiment", Wac_accmasterCont.Experiment, SQLControl.EnumDataType.dtNumeric)
                                .AddField("DataGroupNo", Wac_accmasterCont.DataGroupNo, SQLControl.EnumDataType.dtString)
                                .AddField("SpecNo", Wac_accmasterCont.SpecNo, SQLControl.EnumDataType.dtString, SEAL.Data.SQLControl.EnumValidate.cEmpty)
                                .AddField("DMSet", Wac_accmasterCont.DMSet, SQLControl.EnumDataType.dtString)
                                .AddField("DMOption", Wac_accmasterCont.DMOption, SQLControl.EnumDataType.dtString)
                                .AddField("Remark", Wac_accmasterCont.Remark, SQLControl.EnumDataType.dtString, SEAL.Data.SQLControl.EnumValidate.cEmpty)
                                .AddField("PostDate", Wac_accmasterCont.PostDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Posted", Wac_accmasterCont.Posted, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Active", Wac_accmasterCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", Wac_accmasterCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", Wac_accmasterCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", Wac_accmasterCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", Wac_accmasterCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Flag", Wac_accmasterCont.Flag, SQLControl.EnumDataType.dtNumeric)
                                .AddField("TotalComp", Wac_accmasterCont.TotalComp, SQLControl.EnumDataType.dtNumeric)
                                .AddField("DictionaryNo", Wac_accmasterCont.DictionaryNo, SQLControl.EnumDataType.dtString)
                                DictionaryNo = Wac_accmasterCont.DictionaryNo
                                WasCode = Wac_accmasterCont.WasCode
                                WasType = Wac_accmasterCont.WasType
                                LocID = Wac_accmasterCont.LocID
                                CompanyID = Wac_accmasterCont.CompanyID
                                
                                .AddField("CompanyID", Wac_accmasterCont.CompanyID, SQLControl.EnumDataType.dtString)
                                .AddField("LocID", Wac_accmasterCont.LocID, SQLControl.EnumDataType.dtString)
                                .AddField("BaseID", Wac_accmasterCont.BaseID, SQLControl.EnumDataType.dtString)
                                .AddField("WasCode", Wac_accmasterCont.WasCode, SQLControl.EnumDataType.dtString)
                                .AddField("WasType", Wac_accmasterCont.WasType, SQLControl.EnumDataType.dtString)
                                .AddField("HandlingType", Wac_accmasterCont.HandlingType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("HandlingCode", Wac_accmasterCont.HandlingCode, SQLControl.EnumDataType.dtString)
                                .AddField("HandlingGroup", Wac_accmasterCont.HandlingGroup, SQLControl.EnumDataType.dtStringN)
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                ListSQL.Add(strSQL)
                                
                            End With

                        End If
                    Next

                End If


                If ListContWACDtl Is Nothing OrElse ListContWACDtl.Count <= 0 Then
                    strSQL = "DELETE Wac_accscope WITH (ROWLOCK) WHERE LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, locIDparam) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, wastecode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, wastetype) & "'"
                    ListSQL.Add(strSQL)
                Else
                    If usedeleted = True Then
                        strSQL = "DELETE Wac_accscope WITH (ROWLOCK) WHERE WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ListContWACDtl(0).WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ListContWACDtl(0).WasType) & "'"
                        ListSQL.Add(strSQL)
                    End If
                    For Each Wac_accscopeCont In ListContWACDtl
                        blnExec = False
                        blnFound = False
                        If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                            StartSQLControl()
                            
                            With objSQL
                                .TableName = "Wac_accscope"
                                .AddField("SeqNo", Wac_accscopeCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPPROP", Wac_accscopeCont.CPPROP, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPFACTOR", Wac_accscopeCont.CPFACTOR, SQLControl.EnumDataType.dtString)
                                .AddField("CPSGM", Wac_accscopeCont.CPSGM, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPBool", Wac_accscopeCont.ReactivityValue, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPMIN", Wac_accscopeCont.CPMIN, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPSGX", Wac_accscopeCont.CPSGX, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPMAX", Wac_accscopeCont.CPMAX, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPSMD", Wac_accscopeCont.CPSMD, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPMED", Wac_accscopeCont.CPMED, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPSAV", Wac_accscopeCont.CPSAV, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPAVG", Wac_accscopeCont.CPAVG, SQLControl.EnumDataType.dtNumeric)
                                .AddField("MethodNo", Wac_accscopeCont.MethodNo, SQLControl.EnumDataType.dtNumeric)
                                .AddField("UnitNo", Wac_accscopeCont.UnitNo, SQLControl.EnumDataType.dtString)
                                .AddField("DeletedAttempt", Wac_accscopeCont.DeletedAttempt, SQLControl.EnumDataType.dtNumeric)
                                .AddField("DeletedReason", Wac_accscopeCont.DeletedReason, SQLControl.EnumDataType.dtStringN)
                                .AddField("ACStep", Wac_accscopeCont.ACStep, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Dependency", Wac_accscopeCont.Dependency, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Mode", Wac_accscopeCont.Mode, SQLControl.EnumDataType.dtString)
                                .AddField("ATMin", Wac_accscopeCont.ATMin, SQLControl.EnumDataType.dtString)
                                .AddField("ATMax", Wac_accscopeCont.ATMax, SQLControl.EnumDataType.dtString)
                                .AddField("ATNX", Wac_accscopeCont.ATNX, SQLControl.EnumDataType.dtString)
                                .AddField("ATTR", Wac_accscopeCont.ATTR, SQLControl.EnumDataType.dtString)
                                .AddField("DDGroup", Wac_accscopeCont.DDGroup, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Active", Wac_accscopeCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", Wac_accscopeCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", Wac_accscopeCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", Wac_accscopeCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", Wac_accscopeCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("ConvertMin", Wac_accscopeCont.ConvertMin, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ConvertMax", Wac_accscopeCont.ConvertMax, SQLControl.EnumDataType.dtNumeric)
                                .AddField("DictionaryNo", Wac_accscopeCont.DictionaryNo, SQLControl.EnumDataType.dtString)

                                If blnFound = False Then
                                    .AddField("LocID", Wac_accscopeCont.LocID, SQLControl.EnumDataType.dtString)
                                    .AddField("BaseID", Wac_accscopeCont.BaseID, SQLControl.EnumDataType.dtString)
                                    .AddField("WasCode", Wac_accscopeCont.WasCode, SQLControl.EnumDataType.dtString)
                                    .AddField("WasType", Wac_accscopeCont.WasType, SQLControl.EnumDataType.dtString)
                                    .AddField("HandlingType", Wac_accscopeCont.HandlingType, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("HandlingCode", Wac_accscopeCont.HandlingCode, SQLControl.EnumDataType.dtString)
                                    .AddField("COMPCode", Wac_accscopeCont.COMPCode, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("HandlingGroup", Wac_accscopeCont.HandlingGroup, SQLControl.EnumDataType.dtNumeric)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                End If
                                
                            End With
                        End If
                        ListSQL.Add(strSQL)
                    Next
                End If
                If wastecode <> "" AndAlso wastetype <> "" Then
                    LocID = locIDparam
                    WasCode = wastecode
                    WasType = wastetype
                Else
                    LocID = ListContWACDtl(0).LocID
                    WasCode = ListContWACDtl(0).WasCode
                    WasType = ListContWACDtl(0).WasType
                End If
                If ListContWACHdr IsNot Nothing AndAlso ListContWACHdr.Count > 0 Then
                    If ListContWACHdr(0).Posted = 1 Then
                        strSQL = "EXEC [WAC_QualifiedWR_DOE] '" & LocID & "','" & WasCode & "','" & WasType & "','',''"
                        ListSQL.Add(strSQL)

                        strSQL = "EXEC [WAC_QualifiedWR_WG] '','" & LocID & "','" & WasCode & "','" & WasType & "','',''"
                        ListSQL.Add(strSQL)
                    End If
                Else
                    strSQL = "EXEC [WAC_QualifiedWR_DOE] '" & LocID & "','" & WasCode & "','" & WasType & "','',''"
                    ListSQL.Add(strSQL)

                    strSQL = "EXEC [WAC_QualifiedWR_WG] '','" & LocID & "','" & WasCode & "','" & WasType & "','',''"
                    ListSQL.Add(strSQL)
                End If


                Try

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
                    Gibraltar.Agent.Log.Error("WAC_ACCMASTER", axExecute.Message & sqlStatement, axExecute.StackTrace)
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

        Public Function UpdateAllWasteCode(ByVal wastecode As String, ByVal wastetype As String, ByVal totalcomp As String, ByVal HandlingType As String, ByVal BaseID As String, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim ListSQL As ArrayList = New ArrayList()
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim DictionaryNo As String = ""
            Dim WasCode As String = ""
            Dim WasType As String = ""
            Dim LocID As String = ""
            Dim CompanyID As String = ""
            Try

                StartSQLControl()
                With Wac_accmasterInfo.MyInfo
                    strSQL = "update WAC_ACCMASTER set BaseID = '" & BaseID & "', HandlingType = '" & HandlingType & "' where WasCode = '" & wastecode & "' and WasType = '" & wastetype & "'"
                    ListSQL.Add(strSQL)
                    strSQL = "update WAC_ACCSCOPE set BaseID = '" & BaseID & "', HandlingType = '" & HandlingType & "', Active = 1 where WasCode = '" & wastecode & "' and WasType = '" & wastetype & "'"
                    ListSQL.Add(strSQL)
                    strSQL = "update WAC_ACCMASTER set TotalComp = '" & totalcomp & "' where WasCode = '" & wastecode & "' and WasType = '" & wastetype & "' and HandlingCode <> '-'"
                    ListSQL.Add(strSQL)
                    strSQL = "update WAC_CHARMASTER set BaseID = '" & BaseID & "', HandlingType = '" & HandlingType & "',TotalComp = '" & totalcomp & "' where WasCode = '" & wastecode & "' and WasType = '" & wastetype & "'"
                    ListSQL.Add(strSQL)
                    strSQL = "update WAC_CHARSCOPE set BaseID = '" & BaseID & "', HandlingType = '" & HandlingType & "', Active = 1 where WasCode = '" & wastecode & "' and WasType = '" & wastetype & "'"
                    ListSQL.Add(strSQL)
                    strSQL = "update WAC_BASEQUEST set BaseID = '" & BaseID & "' where WasCode = '" & wastecode & "' and WasType = '" & wastetype & "'"
                    ListSQL.Add(strSQL)
                End With

                Try
                    objConn.BatchExecute(ListSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                Catch axExecute As Exception
                    message = axExecute.Message.ToString()
                    Dim sqlStatement As String = " "
                    If objConn.FailedSQLStatement.Count > 0 Then
                        sqlStatement &= objConn.FailedSQLStatement.Item(0)
                    End If

                    Log.Notifier.Notify(axExecute)
                    Gibraltar.Agent.Log.Error("WAC_ACCMASTER", axExecute.Message & sqlStatement, axExecute.StackTrace)
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
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function DeleteByCompCodeWasteCode(ByVal wastecode As String, ByVal wastetype As String, ByVal compcode As String, ByVal handlingcode As String, ByVal totalcomp As String, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim ListSQL As ArrayList = New ArrayList()
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim DictionaryNo As String = ""
            Dim WasCode As String = ""
            Dim WasType As String = ""
            Dim LocID As String = ""
            Dim CompanyID As String = ""
            Try

                StartSQLControl()
                With Wac_accmasterInfo.MyInfo
                    If compcode <> "" Then
                        strSQL = "delete WAC_ACCSCOPE where COMPCode in (" & compcode & ") and WasCode = '" & wastecode & "' and WasType = '" & wastetype & "'"
                        ListSQL.Add(strSQL)
                        strSQL = "delete WAC_CHARSCOPE where COMPCode in (" & compcode & ") and WasCode = '" & wastecode & "' and WasType = '" & wastetype & "'"
                        ListSQL.Add(strSQL)
                    ElseIf handlingcode <> "" Then
                        strSQL = "delete WAC_ACCSCOPE where HandlingCode in (" & handlingcode & ") and WasCode = '" & wastecode & "' and WasType = '" & wastetype & "'"
                        ListSQL.Add(strSQL)
                        strSQL = "delete WAC_ACCMASTER where HandlingCode in (" & handlingcode & ") and WasCode = '" & wastecode & "' and WasType = '" & wastetype & "'"
                        ListSQL.Add(strSQL)
                        strSQL = "delete WAC_ACCMASTER where HandlingCode in (" & handlingcode & ") and WasCode = '" & wastecode & "' and WasType = '" & wastetype & "'"
                        ListSQL.Add(strSQL)
                    End If
                End With

                Try
                    objConn.BatchExecute(ListSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                Catch axExecute As Exception
                    message = axExecute.Message.ToString()
                    Dim sqlStatement As String = " "
                    If objConn.FailedSQLStatement.Count > 0 Then
                        sqlStatement &= objConn.FailedSQLStatement.Item(0)
                    End If
                    Log.Notifier.Notify(axExecute)
                    Gibraltar.Agent.Log.Error("WAC_ACCMASTER", axExecute.Message & sqlStatement, axExecute.StackTrace)
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
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal ListContWACHdr As List(Of Container.Wac_accmaster), ByVal ListContWACDtl As List(Of Container.Wac_accscope), ByRef message As String, Optional baseline As Boolean = False, Optional wastecode As String = "", Optional wastetype As String = "", Optional LocID As String = "", Optional ByVal usedeleted As Boolean = True) As Boolean
            Return Save(ListContWACHdr, ListContWACDtl, SQLControl.EnumSQLType.stInsert, message, baseline, usedeleted, wastecode, wastetype, LocID)
        End Function

        'AMEND
        Public Function Update(ByVal ListContWACHdr As List(Of Container.Wac_accmaster), ByVal ListContWACDtl As List(Of Container.Wac_accscope), ByRef message As String, Optional baseline As Boolean = False, Optional wastecode As String = "", Optional wastetype As String = "", Optional LocID As String = "", Optional ByVal usedeleted As Boolean = True) As Boolean
            Return Save(ListContWACHdr, ListContWACDtl, SQLControl.EnumSQLType.stUpdate, message, baseline, usedeleted, wastecode, wastetype, LocID)
        End Function

        Public Function Delete(ByVal ListContWACHdr As List(Of Container.Wac_accmaster), ByRef message As String) As Boolean
            Dim ListSQL As ArrayList = New ArrayList()
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False

            Try
                If ListContWACHdr Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        For Each Wac_accmasterCont In ListContWACHdr
                            With Wac_accmasterInfo.MyInfo
                                strSQL = BuildSelect(.CheckFields, .TableName, "CompanyID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.CompanyID) & "' AND LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.LocID) & "' AND BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.DictionaryNo) & "'")
                            End With
                            rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

                            If rdr Is Nothing = False Then
                                With rdr
                                    If .Read Then
                                        blnFound = True
                                       
                                    End If
                                    .Close()
                                End With
                            End If

                            If blnFound = True Then
                                With objSQL
                                    strSQL = BuildUpdate(Wac_accmasterInfo.MyInfo.TableName, " SET Flag = 0" & _
                                    " , LastUpdate = GETDATE() , UpdateBy = '" & _
                                    objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.UpdateBy) & "' WHERE " & _
                                    "CompanyID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.CompanyID) & "' AND LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.LocID) & "' AND BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accmasterCont.DictionaryNo) & "'")
                                End With
                            End If

                            ListSQL.Add(strSQL)
                        Next

                        Try
                            objConn.BatchExecute(ListSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
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
                ListContWACHdr = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region


#Region "Data Selection"

        Public Overloads Function GetWAC_ACCMASTERByWasteCode(ByVal WasCode As System.String, ByVal WasType As System.String) As List(Of Container.Wac_accmaster)
            Dim rWac_accmaster As Container.Wac_accmaster = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim listContWAC_AccMaster As List(Of Container.Wac_accmaster) = New List(Of Container.Wac_accmaster)

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With Wac_accmasterInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "WasCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasCode) & "' AND WasType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasType) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                For Each drrow In dtTemp.Rows

                                    rWac_accmaster = New Container.Wac_accmaster
                                    rWac_accmaster.CompanyID = drrow.Item("CompanyID")
                                    rWac_accmaster.LocID = drrow.Item("LocID")
                                    rWac_accmaster.BaseID = drrow.Item("BaseID")
                                    rWac_accmaster.WasCode = drrow.Item("WasCode")
                                    rWac_accmaster.WasType = drrow.Item("WasType")
                                    rWac_accmaster.HandlingType = drrow.Item("HandlingType")
                                    rWac_accmaster.HandlingCode = drrow.Item("HandlingCode")
                                    rWac_accmaster.DictionaryNo = drrow.Item("DictionaryNo")
                                    rWac_accmaster.HandlingName = drrow.Item("HandlingName")
                                    rWac_accmaster.DictionaryName = drrow.Item("DictionaryName")
                                    rWac_accmaster.State = drrow.Item("State")
                                    rWac_accmaster.PercentProduct = drrow.Item("PercentProduct")
                                    rWac_accmaster.PercentResidue = drrow.Item("PercentResidue")
                                    rWac_accmaster.Experiment = drrow.Item("Experiment")
                                    rWac_accmaster.DataGroupNo = drrow.Item("DataGroupNo")
                                    rWac_accmaster.SpecNo = drrow.Item("SpecNo")
                                    rWac_accmaster.DMSet = drrow.Item("DMSet")
                                    rWac_accmaster.DMOption = drrow.Item("DMOption")
                                    rWac_accmaster.Remark = drrow.Item("Remark")
                                    rWac_accmaster.Posted = drrow.Item("Posted")
                                    rWac_accmaster.Active = drrow.Item("Active")
                                    rWac_accmaster.CreateBy = drrow.Item("CreateBy")
                                    rWac_accmaster.UpdateBy = drrow.Item("UpdateBy")
                                    listContWAC_AccMaster.Add(rWac_accmaster)
                                Next

                            Else
                                rWac_accmaster = Nothing
                            End If
                        Else
                            rWac_accmaster = Nothing
                        End If
                    End With
                End If
                Return listContWAC_AccMaster
            Catch ex As Exception
                Throw ex
            Finally
                rWac_accmaster = Nothing
                dtTemp = Nothing
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
                    With Wac_accmasterInfo.MyInfo
                        strSQL = "select distinct CompanyID,LocID,state,DictionaryNo from WAC_ACCMASTER" &
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

        Public Overloads Function GetWAC_ACCMASTERListByDictNo(ByVal DictionaryNo As System.String) As List(Of Container.Wac_accmaster)
            Dim rWac_accmaster As Container.Wac_accmaster = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim ListContWACHdr As List(Of Container.Wac_accmaster) = New List(Of Container.Wac_accmaster)

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With Wac_accmasterInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "DictionaryNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, DictionaryNo) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0

                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                For Each drRow In dtTemp.Rows
                                    rWac_accmaster = New Container.Wac_accmaster
                                    rWac_accmaster.CompanyID = drRow.Item("CompanyID")
                                    rWac_accmaster.LocID = drRow.Item("LocID")
                                    rWac_accmaster.BaseID = drRow.Item("BaseID")
                                    rWac_accmaster.WasCode = drRow.Item("WasCode")
                                    rWac_accmaster.WasType = drRow.Item("WasType")
                                    rWac_accmaster.HandlingType = drRow.Item("HandlingType")
                                    rWac_accmaster.HandlingCode = drRow.Item("HandlingCode")
                                    rWac_accmaster.DictionaryNo = drRow.Item("DictionaryNo")
                                    rWac_accmaster.HandlingName = drRow.Item("HandlingName")
                                    rWac_accmaster.DictionaryName = drRow.Item("DictionaryName")
                                    rWac_accmaster.State = drRow.Item("State")
                                    rWac_accmaster.PercentProduct = drRow.Item("PercentProduct")
                                    rWac_accmaster.PercentResidue = drRow.Item("PercentResidue")
                                    rWac_accmaster.Experiment = drRow.Item("Experiment")
                                    rWac_accmaster.DataGroupNo = drRow.Item("DataGroupNo")
                                    rWac_accmaster.SpecNo = drRow.Item("SpecNo")
                                    rWac_accmaster.DMSet = drRow.Item("DMSet")
                                    rWac_accmaster.DMOption = drRow.Item("DMOption")
                                    rWac_accmaster.Remark = drRow.Item("Remark")
                                    rWac_accmaster.Posted = drRow.Item("Posted")
                                    rWac_accmaster.Active = drRow.Item("Active")
                                    rWac_accmaster.CreateBy = drRow.Item("CreateBy")
                                    rWac_accmaster.UpdateBy = drRow.Item("UpdateBy")

                                    ListContWACHdr.Add(rWac_accmaster)
                                Next
                            Else
                                ListContWACHdr = Nothing
                            End If
                        Else
                            ListContWACHdr = Nothing
                        End If

                    End With
                End If
                Return ListContWACHdr
            Catch ex As Exception
                Throw ex
            Finally
                rWac_accmaster = Nothing
                ListContWACHdr = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_ACCMASTER(ByVal CompanyID As System.String, ByVal LocID As System.String, ByVal BaseID As System.String, ByVal WasCode As System.String, ByVal WasType As System.String, ByVal HandlingType As System.Int32, ByVal HandlingCode As System.String, ByVal DictionaryNo As System.String) As Container.Wac_accmaster
            Dim rWac_accmaster As Container.Wac_accmaster = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With Wac_accmasterInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "CompanyID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyID) & "' AND LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' AND BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, DictionaryNo) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWac_accmaster = New Container.Wac_accmaster
                                rWac_accmaster.CompanyID = drRow.Item("CompanyID")
                                rWac_accmaster.LocID = drRow.Item("LocID")
                                rWac_accmaster.BaseID = drRow.Item("BaseID")
                                rWac_accmaster.WasCode = drRow.Item("WasCode")
                                rWac_accmaster.WasType = drRow.Item("WasType")
                                rWac_accmaster.HandlingType = drRow.Item("HandlingType")
                                rWac_accmaster.HandlingCode = drRow.Item("HandlingCode")
                                rWac_accmaster.DictionaryNo = drRow.Item("DictionaryNo")
                                rWac_accmaster.HandlingName = drRow.Item("HandlingName")
                                rWac_accmaster.DictionaryName = drRow.Item("DictionaryName")
                                rWac_accmaster.State = drRow.Item("State")
                                rWac_accmaster.PercentProduct = drRow.Item("PercentProduct")
                                rWac_accmaster.PercentResidue = drRow.Item("PercentResidue")
                                rWac_accmaster.Experiment = drRow.Item("Experiment")
                                rWac_accmaster.DataGroupNo = drRow.Item("DataGroupNo")
                                rWac_accmaster.SpecNo = drRow.Item("SpecNo")
                                rWac_accmaster.DMSet = drRow.Item("DMSet")
                                rWac_accmaster.DMOption = drRow.Item("DMOption")
                                rWac_accmaster.Remark = drRow.Item("Remark")
                                rWac_accmaster.Posted = drRow.Item("Posted")
                                rWac_accmaster.Active = drRow.Item("Active")
                                rWac_accmaster.CreateBy = drRow.Item("CreateBy")
                                rWac_accmaster.UpdateBy = drRow.Item("UpdateBy")
                            Else
                                rWac_accmaster = Nothing
                            End If
                        Else
                            rWac_accmaster = Nothing
                        End If
                    End With
                End If
                Return rWac_accmaster
            Catch ex As Exception
                Throw ex
            Finally
                rWac_accmaster = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_ACCMASTER(ByVal CompanyID As System.String, ByVal LocID As System.String, ByVal BaseID As System.String, ByVal WasCode As System.String, ByVal WasType As System.String, ByVal HandlingType As System.Int32, ByVal HandlingCode As System.String, ByVal DictionaryNo As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Wac_accmaster)
            Dim rWac_accmaster As Container.Wac_accmaster = Nothing
            Dim lstWac_accmaster As List(Of Container.Wac_accmaster) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With Wac_accmasterInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal CompanyID As System.String, ByVal LocID As System.String, ByVal BaseID As System.String, ByVal WasCode As System.String, ByVal WasType As System.String, ByVal HandlingType As System.Int32, ByVal HandlingCode As System.String, ByVal DictionaryNo As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "CompanyID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyID) & "' AND LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' AND BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, DictionaryNo) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rWac_accmaster = New Container.Wac_accmaster
                                rWac_accmaster.CompanyID = drRow.Item("CompanyID")
                                rWac_accmaster.LocID = drRow.Item("LocID")
                                rWac_accmaster.BaseID = drRow.Item("BaseID")
                                rWac_accmaster.WasCode = drRow.Item("WasCode")
                                rWac_accmaster.WasType = drRow.Item("WasType")
                                rWac_accmaster.HandlingType = drRow.Item("HandlingType")
                                rWac_accmaster.HandlingCode = drRow.Item("HandlingCode")
                                rWac_accmaster.DictionaryNo = drRow.Item("DictionaryNo")
                                rWac_accmaster.HandlingName = drRow.Item("HandlingName")
                                rWac_accmaster.DictionaryName = drRow.Item("DictionaryName")
                                rWac_accmaster.State = drRow.Item("State")
                                rWac_accmaster.PercentProduct = drRow.Item("PercentProduct")
                                rWac_accmaster.PercentResidue = drRow.Item("PercentResidue")
                                rWac_accmaster.Experiment = drRow.Item("Experiment")
                                rWac_accmaster.DataGroupNo = drRow.Item("DataGroupNo")
                                rWac_accmaster.SpecNo = drRow.Item("SpecNo")
                                rWac_accmaster.DMSet = drRow.Item("DMSet")
                                rWac_accmaster.DMOption = drRow.Item("DMOption")
                                rWac_accmaster.Remark = drRow.Item("Remark")
                                rWac_accmaster.Posted = drRow.Item("Posted")
                                rWac_accmaster.Active = drRow.Item("Active")
                                rWac_accmaster.CreateBy = drRow.Item("CreateBy")
                                rWac_accmaster.UpdateBy = drRow.Item("UpdateBy")
                            Next
                            lstWac_accmaster.Add(rWac_accmaster)
                        Else
                            rWac_accmaster = Nothing
                        End If
                        Return lstWac_accmaster
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rWac_accmaster = Nothing
                lstWac_accmaster = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_ACCMASTERListByDictionaryNo(ByVal DictNo As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_accmasterInfo.MyInfo
                    strSQL = " Select TOP 1 wac.*, cm.CodeDesc AS WasteDesc, wab.allowreuse, wab.TestingWaste,  case when wac.Posted = 0 and  tb.cnt>= 1 then 2 else wac.Posted end as postedstatus " &
                             " FROM Wac_accmaster wac WITH (NOLOCK) " &
                             " LEFT JOIN WAC_BASELINE wab WITH (NOLOCK) on wac.BaseID =  wab.BaseID and wac.wascode = wab.wascode " &
                             " Left JOIN CodeMaster cm With (NOLOCK) On wac.WasType=cm.Code And cm.CodeType='WTY' " &
                             "  CROSS APPLY (SELECT COUNT(Posted) cnt FROM Wac_accmaster cm where cm.locid = wac.locid and cm.baseid = wac.baseID and cm.wascode = wac.wascode and cm.wastype = wac.wastype and cm.Posted = 1) tb   " &
                             " WHERE wac.DictionaryNo='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, DictNo) & "' ORDER BY PostDate DESC"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWAC_ACCMASTERListByBaseID(ByVal BaseID As String, ByVal LocID As String, ByVal WasteType As String, Optional ByVal HandlingCode As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_accmasterInfo.MyInfo
                    strSQL = "Select TOP 1 wac.*, cm.CodeDesc AS WasteDesc, wab.allowreuse, wab.TestingWaste,  case when wac.Posted = 0 and  tb.cnt>= 1 then 2 else wac.Posted end as postedstatus FROM Wac_accmaster wac WITH (NOLOCK) LEFT JOIN WAC_BASELINE wab WITH (NOLOCK) on wac.BaseID =  wab.BaseID and wac.wascode = wab.wascode LEFT JOIN CodeMaster cm WITH (NOLOCK) ON wac.WasType=cm.Code AND cm.CodeType='WTY' " & _
                        " CROSS APPLY (SELECT COUNT(Posted) cnt FROM Wac_accmaster cm where cm.locid = wac.locid and cm.baseid = wac.baseID and cm.wascode = wac.wascode and cm.wastype = wac.wastype and cm.Posted = 1) tb " &
                        " WHERE wac.BaseID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, BaseID) & "' AND wac.LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND wac.WasType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "'  and wac.Flag = 1  "
                    If HandlingCode IsNot Nothing Then
                        strSQL &= " AND HandlingCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, HandlingCode) & "'"
                    End If
                    strSQL &= " ORDER BY PostDate DESC"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWAC_ACCMASTERCustomList(Optional ByVal LocID As String = "") As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_accmasterInfo.MyInfo
                    strSQL = "SELECT DISTINCT CONVERT(date,am.PostDate) AS PostDate, am.DictionaryNo, am.LocID, am.BaseID, am.WasCode, am.WasType, i.ItmDesc, am.TotalComp, am.SpecNo, am.CreateBy, STUFF((SELECT DISTINCT '-' + CAST(md.HandlingType AS VARCHAR(MAX)) FROM WAC_ACCMASTER md WITH (NOLOCK) WHERE md.LocID=am.LocID AND md.WasCode=am.WasCode AND md.WasType=am.WasType FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') AS HandlingType, STUFF((SELECT DISTINCT '-' + md.HandlingCode FROM WAC_ACCMASTER md WITH (NOLOCK) WHERE md.LocID=am.LocID AND md.WasCode=am.WasCode and md.DictionaryNo = am.DictionaryNo AND md.WasType=am.WasType FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') AS HandlingCode, case WHEN am.Posted = 0 THEN 'Draft' ELSE 'Submitted' END As Status, " & _
                        " (select TOP 1 case when ValidityStart < Getdate() and Validityend > Getdate() Then 'Active' ELSE 'InActive' END as Status from license l  INNER JOIN LICENSEITEM li WITH (NOLOCK) ON l.ContractNo = li.ContractNo where l.locid = am.locid and li.itemcode = am.WasCode and l.active = 1 and l.Flag= 1 and li.Active = 1 and li.Flag = 1 ) as statusactive" & _
                        " FROM WAC_ACCMASTER wa OUTER APPLY (SELECT TOP 1 * FROM WAC_ACCMASTER ch  WITH (NOLOCK) WHERE ch.Flag=1 and ch.LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' and ch.wascode = wa.wascode and ch.wastype=wa.wastype ORDER BY PostDate DESC) am LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON am.WasType = cm.Code AND cm.CodeType = 'WTY'" & _
                        " LEFT JOIN ITEM i WITH (NOLOCK) ON i.ItemCode=am.WasCode" & _
                        " WHERE am.Flag=1 AND am.LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "'" & _
                        " GROUP BY am.LocID, am.BaseID, am.WasCode, am.WasType, i.ItmDesc, am.TotalComp, am.PostDate, am.DictionaryNo, am.SpecNo, am.CreateBy, am.posted"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWAC_ACCMASTERByLOCID(Optional ByVal CompanyID As String = "") As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_accmasterInfo.MyInfo
                    strSQL = "SELECT TOP 1 CompanyID " & _
                        " FROM WAC_ACCMASTER wa WITH (NOLOCK)" & _
                        " WHERE wa.Flag=1 AND wa.CompanyID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompanyID) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetSummaryCriteria(Optional ByVal WasteCode As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_accmasterInfo.MyInfo
                    strSQL = "SELECT a.WasCode, a.WasType, b.CodeDesc AS WasTypeDesc, ISNULL(c.BranchName,'') AS PremiseName, count(Distinct a.DictionaryNo) AS AttempNo, ISNULL(max(a.LastUpdate),max(a.CreateDate)) AS LastUpdated, a.LocID " &
                             "FROM WAC_ACCMASTER a WITH (NOLOCK) INNER JOIN CODEMASTER b WITH (NOLOCK) ON a.WasType=b.Code AND b.CodeType='WTY' LEFT JOIN BIZLOCATE c WITH(NOLOCK) ON a.LocID = c.Bizlocid "
                    If Not WasteCode Is Nothing Then
                        strSQL &= " WHERE a.WasCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "'"
                    Else
                        strSQL &= " WHERE a.posted = 1"
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

        Public Overloads Function GetWAC_ACCMASTERTreatmentList(ByVal BaseID As String, ByVal LocID As String, Optional HandlingType As String = "") As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_accmasterInfo.MyInfo
                    strSQL = "SELECT distinct b.Code AS HandlingCode,b.CodeDesc AS TreatmentDesc, a.HandlingGroup" &
                        " FROM WAC_ACCMASTER a INNER JOIN codemaster b  ON b.Code = a.HandlingCode AND  b.CodeType='WTH'" &
                        " outer APPLY (SELECT TOP 1 * FROM WAC_ACCMASTER e  WITH (NOLOCK) WHERE e.Flag=1 and e.LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' and e.BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BaseID) & "' ORDER BY PostDate DESC) e " &
                        " Where a.BaseID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BaseID) & "' AND a.DictionaryNo = e.DictionaryNo AND a.HandlingCode <> '-'" &
                        " AND a.LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' "
                    If HandlingType <> "" Then
                        strSQL &= " AND a.HandlingType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, HandlingType) & "'"
                    End If
                    strSQL &= " AND a.Flag=1 and a.Active=1"
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
        Public Class Wac_accmaster
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
            Public fPostDate As System.String = "PostDate"
            Public fPosted As System.String = "Posted"
            Public fActive As System.String = "Active"
            Public fFlag As System.String = "Flag"
            Public fCreateDate As System.String = "CreateDate"
            Public fCreateBy As System.String = "CreateBy"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fUpdateBy As System.String = "UpdateBy"
            Public fTreatmentName As System.String = "TreatmentName"
            Public fTotalComp As System.String = "TotalComp"

            Protected _CompanyID As System.String
            Protected _LocID As System.String
            Protected _BaseID As System.String
            Protected _WasCode As System.String
            Protected _WasType As System.String
            Protected _HandlingType As System.Int32
            Protected _HandlingGroup As System.Int32
            Protected _HandlingCode As System.String
            Protected _DictionaryNo As System.String
            Private _HandlingName As System.String
            Private _DictionaryName As System.String
            Private _State As System.String
            Private _PercentProduct As System.Int32
            Private _PercentResidue As System.Int32
            Private _Experiment As System.Byte
            Private _DataGroupNo As System.String
            Private _SpecNo As System.String
            Private _DMSet As System.String
            Private _DMOption As System.String
            Private _Remark As System.String
            Private _PostDate As System.DateTime
            Private _Posted As System.Byte
            Private _Active As System.Byte
            Private _Flag As System.Byte
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _TreatmentName As System.String
            Private _TotalComp As System.Int32

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
            Public Property HandlingGroup As System.Int32
                Get
                    Return _HandlingGroup
                End Get
                Set(ByVal Value As System.Int32)
                    _HandlingGroup = Value
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
    Public Class Wac_accmasterInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "CompanyID,LocID,BaseID,WasCode,WasType,HandlingType,HandlingCode,DictionaryNo,HandlingName,DictionaryName,State,PercentProduct,PercentResidue,Experiment,DataGroupNo,SpecNo,DMSet,DMOption,Remark,PostDate,Posted,Active,Flag,CreateDate,CreateBy,LastUpdate,UpdateBy"
                .CheckFields = "Experiment,Posted,Active,Flag"
                .TableName = "Wac_accmaster"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "CompanyID,LocID,BaseID,WasCode,WasType,HandlingType,HandlingCode,DictionaryNo,HandlingName,DictionaryName,State,PercentProduct,PercentResidue,Experiment,DataGroupNo,SpecNo,DMSet,DMOption,Remark,PostDate,Posted,Active,Flag,CreateDate,CreateBy,LastUpdate,UpdateBy"
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
    Public Class WAC_ACCMASTERScheme
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
                .Length = 10
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
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "PostDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Posted"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)

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
        Public ReadOnly Property PostDate As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property Posted As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace
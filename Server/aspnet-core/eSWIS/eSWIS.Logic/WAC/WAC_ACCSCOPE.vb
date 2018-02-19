

Imports SEAL.Data
imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace Actions
Public NotInheritable Class WAC_ACCSCOPE
	Inherits Core.CoreControl
	Private Wac_accscopeInfo As Wac_accscopeInfo = New Wac_accscopeInfo
	
	Public Sub New(ByVal connecn As String)
        ConnectionString = connecn
        ConnectionSetup()
    End Sub
		
	#Region "Data Manipulation-Add,Edit,Del"
	Private Function Save(ByVal Wac_accscopeCont As Container.Wac_accscope, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
		Dim strSQL As String = ""
        Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
        Dim rdr As System.Data.SqlClient.SqlDataReader
        Save = False
        Try
			If Wac_accscopeCont Is Nothing Then
                'Message return
            Else
				blnExec = False
                blnFound = False
                blnFlag = False
				If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
					With Wac_accscopeInfo.MyInfo
                        strSQL = BuildSelect(.CheckFields, .TableName, "LocID = '" & Wac_accscopeCont.LocID & "' AND BaseID = '" & Wac_accscopeCont.BaseID & "' AND WasCode = '" & Wac_accscopeCont.WasCode & "' AND WasType = '" & Wac_accscopeCont.WasType & "' AND HandlingType = '" & Wac_accscopeCont.HandlingType & "' AND HandlingCode = '" & Wac_accscopeCont.HandlingCode & "' AND DictionaryNo = '" & Wac_accscopeCont.DictionaryNo & "' AND COMPCode = '" & Wac_accscopeCont.COMPCode & "'")
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
							.TableName = "Wac_accscope"
							.AddField("SeqNo", Wac_accscopeCont.SeqNo,SQLControl.EnumDataType.dtNumeric)
							.AddField("CPPROP", Wac_accscopeCont.CPPROP,SQLControl.EnumDataType.dtNumeric)
							.AddField("CPFACTOR", Wac_accscopeCont.CPFACTOR,SQLControl.EnumDataType.dtString)
							.AddField("CPSGM", Wac_accscopeCont.CPSGM,SQLControl.EnumDataType.dtNumeric)
							.AddField("CPMIN", Wac_accscopeCont.CPMIN,SQLControl.EnumDataType.dtNumeric)
							.AddField("CPSGX", Wac_accscopeCont.CPSGX,SQLControl.EnumDataType.dtNumeric)
							.AddField("CPMAX", Wac_accscopeCont.CPMAX,SQLControl.EnumDataType.dtNumeric)
							.AddField("CPSMD", Wac_accscopeCont.CPSMD,SQLControl.EnumDataType.dtNumeric)
							.AddField("CPMED", Wac_accscopeCont.CPMED,SQLControl.EnumDataType.dtNumeric)
							.AddField("CPSAV", Wac_accscopeCont.CPSAV,SQLControl.EnumDataType.dtNumeric)
							.AddField("CPAVG", Wac_accscopeCont.CPAVG,SQLControl.EnumDataType.dtNumeric)
							.AddField("MethodNo", Wac_accscopeCont.MethodNo,SQLControl.EnumDataType.dtNumeric)
							.AddField("UnitNo", Wac_accscopeCont.UnitNo,SQLControl.EnumDataType.dtString)
							.AddField("DeletedAttempt", Wac_accscopeCont.DeletedAttempt,SQLControl.EnumDataType.dtNumeric)
							.AddField("DeletedReason", Wac_accscopeCont.DeletedReason,SQLControl.EnumDataType.dtStringN)
                                .AddField("ACStep", Wac_accscopeCont.ACStep, SQLControl.EnumDataType.dtNumeric)
							.AddField("Dependency", Wac_accscopeCont.Dependency,SQLControl.EnumDataType.dtNumeric)
							.AddField("Mode", Wac_accscopeCont.Mode,SQLControl.EnumDataType.dtString)
							.AddField("ATMin", Wac_accscopeCont.ATMin,SQLControl.EnumDataType.dtString)
							.AddField("ATMax", Wac_accscopeCont.ATMax,SQLControl.EnumDataType.dtString)
							.AddField("ATNX", Wac_accscopeCont.ATNX,SQLControl.EnumDataType.dtString)
							.AddField("ATTR", Wac_accscopeCont.ATTR,SQLControl.EnumDataType.dtString)
							.AddField("DDGroup", Wac_accscopeCont.DDGroup,SQLControl.EnumDataType.dtNumeric)
							.AddField("Active", Wac_accscopeCont.Active,SQLControl.EnumDataType.dtNumeric)
							.AddField("CreateDate", Wac_accscopeCont.CreateDate,SQLControl.EnumDataType.dtDateTime)
							.AddField("CreateBy", Wac_accscopeCont.CreateBy,SQLControl.EnumDataType.dtString)
							.AddField("LastUpdate", Wac_accscopeCont.LastUpdate,SQLControl.EnumDataType.dtDateTime)
							.AddField("UpdateBy", Wac_accscopeCont.UpdateBy,SQLControl.EnumDataType.dtString)
							
							Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    If blnFound = True And blnFlag = False Then
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & Wac_accscopeCont.LocID & "' AND BaseID = '" & Wac_accscopeCont.BaseID & "' AND WasCode = '" & Wac_accscopeCont.WasCode & "' AND WasType = '" & Wac_accscopeCont.WasType & "' AND HandlingType = '" & Wac_accscopeCont.HandlingType & "' AND HandlingCode = '" & Wac_accscopeCont.HandlingCode & "' AND DictionaryNo = '" & Wac_accscopeCont.DictionaryNo & "' AND COMPCode = '" & Wac_accscopeCont.COMPCode & "'")
									Else
                                        If blnFound = False Then		
											.AddField("LocID", Wac_accscopeCont.LocID, SQLControl.EnumDataType.dtString)
											.AddField("BaseID", Wac_accscopeCont.BaseID, SQLControl.EnumDataType.dtString)
											.AddField("WasCode", Wac_accscopeCont.WasCode, SQLControl.EnumDataType.dtString)
											.AddField("WasType", Wac_accscopeCont.WasType, SQLControl.EnumDataType.dtNumeric)
											.AddField("HandlingType", Wac_accscopeCont.HandlingType, SQLControl.EnumDataType.dtNumeric)
											.AddField("HandlingCode", Wac_accscopeCont.HandlingCode, SQLControl.EnumDataType.dtString)
											.AddField("DictionaryNo", Wac_accscopeCont.DictionaryNo, SQLControl.EnumDataType.dtString)
											.AddField("COMPCode", Wac_accscopeCont.COMPCode, SQLControl.EnumDataType.dtNumeric)
											strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
										End If
									End If
								Case SQLControl.EnumSQLType.stUpdate
									strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LocID = '" & Wac_accscopeCont.LocID & "' AND BaseID = '" & Wac_accscopeCont.BaseID & "' AND WasCode = '" & Wac_accscopeCont.WasCode & "' AND WasType = '" & Wac_accscopeCont.WasType & "' AND HandlingType = '" & Wac_accscopeCont.HandlingType & "' AND HandlingCode = '" & Wac_accscopeCont.HandlingCode & "' AND DictionaryNo = '" & Wac_accscopeCont.DictionaryNo & "' AND COMPCode = '" & Wac_accscopeCont.COMPCode & "'")
							End Select
						End With
						Try
                             objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
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
			Wac_accscopeCont = Nothing
			rdr = Nothing
            EndSQLControl()
            EndConnection()
        End Try
	End Function
	
	'ADD
    Public Function Insert(ByVal Wac_accscopeCont As Container.Wac_accscope, ByRef message As String) As Boolean
        Return Save(Wac_accscopeCont, SQLControl.EnumSQLType.stInsert, message)
    End Function
	
	'AMEND
    Public Function Update(ByVal Wac_accscopeCont As Container.Wac_accscope, ByRef message As String) As Boolean
        Return Save(Wac_accscopeCont, SQLControl.EnumSQLType.stUpdate, message)
    End Function
	
	Public Function Delete(ByVal Wac_accscopeCont As Container.Wac_accscope, ByRef message As String) As Boolean
        Dim strSQL As String
        Dim blnFound As Boolean
        Dim blnInUse As Boolean
        Dim rdr As System.Data.SqlClient.SqlDataReader
        Delete = False
        blnFound = False
        blnInUse = False
        Try
            If Wac_accscopeCont  Is Nothing Then
                'Error Message
            Else
				If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
					With Wac_accscopeInfo.MyInfo
                        strSQL = BuildSelect(.CheckFields, .TableName, "LocID = '" & Wac_accscopeCont.LocID & "' AND BaseID = '" & Wac_accscopeCont.BaseID & "' AND WasCode = '" & Wac_accscopeCont.WasCode & "' AND WasType = '" & Wac_accscopeCont.WasType & "' AND HandlingType = '" & Wac_accscopeCont.HandlingType & "' AND HandlingCode = '" & Wac_accscopeCont.HandlingCode & "' AND DictionaryNo = '" & Wac_accscopeCont.DictionaryNo & "' AND COMPCode = '" & Wac_accscopeCont.COMPCode & "'")
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
							strSQL = BuildUpdate(Wac_accscopeInfo.MyInfo.TableName, " SET Flag = 0" & _
							" , LastUpdate = '" & Wac_accscopeCont.LastUpdate & "' , UpdateBy = '" & _
							objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_accscopeCont.UpdateBy) & "' WHERE" & _
							"LocID = '" & Wac_accscopeCont.LocID & "' AND BaseID = '" & Wac_accscopeCont.BaseID & "' AND WasCode = '" & Wac_accscopeCont.WasCode & "' AND WasType = '" & Wac_accscopeCont.WasType & "' AND HandlingType = '" & Wac_accscopeCont.HandlingType & "' AND HandlingCode = '" & Wac_accscopeCont.HandlingCode & "' AND DictionaryNo = '" & Wac_accscopeCont.DictionaryNo & "' AND COMPCode = '" & Wac_accscopeCont.COMPCode & "'")
						End With
					End if
					
					If blnFound = True And blnInUse = False Then
                        strSQL = BuildDelete(Wac_accscopeInfo.MyInfo.TableName, "LocID = '" & Wac_accscopeCont.LocID & "' AND BaseID = '" & Wac_accscopeCont.BaseID & "' AND WasCode = '" & Wac_accscopeCont.WasCode & "' AND WasType = '" & Wac_accscopeCont.WasType & "' AND HandlingType = '" & Wac_accscopeCont.HandlingType & "' AND HandlingCode = '" & Wac_accscopeCont.HandlingCode & "' AND DictionaryNo = '" & Wac_accscopeCont.DictionaryNo & "' AND COMPCode = '" & Wac_accscopeCont.COMPCode & "'")
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
            Wac_accscopeCont = Nothing
            rdr = Nothing
            EndSQLControl()
            EndConnection()
        End Try	
	End Function
#End Region
	
	#Region "Data Selection"
	Public Overloads Function GetWAC_ACCSCOPE(ByVal LocID As System.String, ByVal BaseID As System.String, ByVal WasCode As System.String, ByVal WasType As System.Int32, ByVal HandlingType As System.Int32, ByVal HandlingCode As System.String, ByVal DictionaryNo As System.String, ByVal COMPCode As System.Int32) As Container.Wac_accscope
		Dim rWac_accscope As Container.Wac_accscope = Nothing
		Dim dtTemp As DataTable = Nothing
		Dim lstField As New List(Of String)
		Dim strSQL As String = Nothing
		
		Try
			If StartConnection() = True Then
				With Wac_accscopeInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' AND BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, DictionaryNo) & "' AND COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, COMPCode) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWAC_ACCSCOPE = New Container.WAC_ACCSCOPE
                                rWAC_ACCSCOPE.LocID = drRow.Item("LocID")
                                rWAC_ACCSCOPE.BaseID = drRow.Item("BaseID")
                                rWAC_ACCSCOPE.WasCode = drRow.Item("WasCode")
                                rWAC_ACCSCOPE.WasType = drRow.Item("WasType")
                                rWAC_ACCSCOPE.HandlingType = drRow.Item("HandlingType")
                                rWAC_ACCSCOPE.HandlingCode = drRow.Item("HandlingCode")
                                rWAC_ACCSCOPE.DictionaryNo = drRow.Item("DictionaryNo")
                                rWAC_ACCSCOPE.COMPCode = drRow.Item("COMPCode")
                                rWac_accscope.SeqNo = drRow.Item("SeqNo")
                                rWac_accscope.CPPROP = drRow.Item("CPPROP")
                                rWac_accscope.CPFACTOR = drRow.Item("CPFACTOR")
                                rWac_accscope.CPSGM = drRow.Item("CPSGM")
                                rWac_accscope.CPMIN = drRow.Item("CPMIN")
                                rWac_accscope.CPSGX = drRow.Item("CPSGX")
                                rWac_accscope.CPMAX = drRow.Item("CPMAX")
                                rWac_accscope.CPSMD = drRow.Item("CPSMD")
                                rWac_accscope.CPMED = drRow.Item("CPMED")
                                rWac_accscope.CPSAV = drRow.Item("CPSAV")
                                rWac_accscope.CPAVG = drRow.Item("CPAVG")
                                rWac_accscope.MethodNo = drRow.Item("MethodNo")
                                rWac_accscope.UnitNo = drRow.Item("UnitNo")
                                rWac_accscope.DeletedAttempt = drRow.Item("DeletedAttempt")
                                rWac_accscope.DeletedReason = drRow.Item("DeletedReason")
                                rWac_accscope.ACStep = drRow.Item("ACStep")
                                rWac_accscope.Dependency = drRow.Item("Dependency")
                                rWac_accscope.Mode = drRow.Item("Mode")
                                rWac_accscope.ATMin = drRow.Item("ATMin")
                                rWac_accscope.ATMax = drRow.Item("ATMax")
                                rWac_accscope.ATNX = drRow.Item("ATNX")
                                rWac_accscope.ATTR = drRow.Item("ATTR")
                                rWac_accscope.DDGroup = drRow.Item("DDGroup")
                                rWac_accscope.Active = drRow.Item("Active")
                                rWac_accscope.CreateBy = drRow.Item("CreateBy")
                                rWac_accscope.UpdateBy = drRow.Item("UpdateBy")
                            Else
                                rWac_accscope = Nothing
                            End If
                        Else
                            rWac_accscope = Nothing
                        End If
                    End With
                End If
                Return rWac_accscope
            Catch ex As Exception
                Throw ex
            Finally
                rWac_accscope = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_ACCSCOPE(ByVal LocID As System.String, ByVal BaseID As System.String, ByVal WasCode As System.String, ByVal WasType As System.Int32, ByVal HandlingType As System.Int32, ByVal HandlingCode As System.String, ByVal DictionaryNo As System.String, ByVal COMPCode As System.Int32, ByVal DecendingOrder As Boolean) As List(Of Container.Wac_accscope)
            Dim rWac_accscope As Container.Wac_accscope = Nothing
            Dim lstWac_accscope As List(Of Container.Wac_accscope) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With Wac_accscopeInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal LocID As System.String, ByVal BaseID As System.String, ByVal WasCode As System.String, ByVal WasType As System.Int32, ByVal HandlingType As System.Int32, ByVal HandlingCode As System.String, ByVal DictionaryNo As System.String, ByVal COMPCode As System.Int32 DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' AND BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, DictionaryNo) & "' AND COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, COMPCode) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rWAC_ACCSCOPE = New Container.WAC_ACCSCOPE
                                rWAC_ACCSCOPE.LocID = drRow.Item("LocID")
                                rWAC_ACCSCOPE.BaseID = drRow.Item("BaseID")
                                rWAC_ACCSCOPE.WasCode = drRow.Item("WasCode")
                                rWAC_ACCSCOPE.WasType = drRow.Item("WasType")
                                rWAC_ACCSCOPE.HandlingType = drRow.Item("HandlingType")
                                rWAC_ACCSCOPE.HandlingCode = drRow.Item("HandlingCode")
                                rWAC_ACCSCOPE.DictionaryNo = drRow.Item("DictionaryNo")
                                rWAC_ACCSCOPE.COMPCode = drRow.Item("COMPCode")
                                rWac_accscope.SeqNo = drRow.Item("SeqNo")
                                rWac_accscope.CPPROP = drRow.Item("CPPROP")
                                rWac_accscope.CPFACTOR = drRow.Item("CPFACTOR")
                                rWac_accscope.CPSGM = drRow.Item("CPSGM")
                                rWac_accscope.CPMIN = drRow.Item("CPMIN")
                                rWac_accscope.CPSGX = drRow.Item("CPSGX")
                                rWac_accscope.CPMAX = drRow.Item("CPMAX")
                                rWac_accscope.CPSMD = drRow.Item("CPSMD")
                                rWac_accscope.CPMED = drRow.Item("CPMED")
                                rWac_accscope.CPSAV = drRow.Item("CPSAV")
                                rWac_accscope.CPAVG = drRow.Item("CPAVG")
                                rWac_accscope.MethodNo = drRow.Item("MethodNo")
                                rWac_accscope.UnitNo = drRow.Item("UnitNo")
                                rWac_accscope.DeletedAttempt = drRow.Item("DeletedAttempt")
                                rWac_accscope.DeletedReason = drRow.Item("DeletedReason")
                                rWac_accscope.ACStep = drRow.Item("ACStep")
                                rWac_accscope.Dependency = drRow.Item("Dependency")
                                rWac_accscope.Mode = drRow.Item("Mode")
                                rWac_accscope.ATMin = drRow.Item("ATMin")
                                rWac_accscope.ATMax = drRow.Item("ATMax")
                                rWac_accscope.ATNX = drRow.Item("ATNX")
                                rWac_accscope.ATTR = drRow.Item("ATTR")
                                rWac_accscope.DDGroup = drRow.Item("DDGroup")
                                rWac_accscope.Active = drRow.Item("Active")
                                rWac_accscope.CreateBy = drRow.Item("CreateBy")
                                rWac_accscope.UpdateBy = drRow.Item("UpdateBy")
                            Next
                            lstWac_accscope.Add(rWac_accscope)
                        Else
                            rWac_accscope = Nothing
                        End If
                        Return lstWac_accscope
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rWac_accscope = Nothing
                lstWac_accscope = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_ACCSCOPE(ByVal WasCode As System.String, ByVal WasType As System.String) As List(Of Container.Wac_accscope)
            Dim rWac_accscope As Container.Wac_accscope = Nothing
            Dim lstWac_accscope As List(Of Container.Wac_accscope) = New List(Of Container.Wac_accscope)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With Wac_accscopeInfo.MyInfo
                        strSQL = "Select WA.* from WAC_ACCSCOPE WA WITH(NOLOCK)" &
                                " INNER JOIN WAC_BASECOMP WB WITH(NOLOCK) on WA.WasCode = WB.WasCode AND WA.WasType = WB.WasType" &
                                " AND WA.HandlingCode = WB.HandlingCode AND WA.COMPCode = WB.COMPCode" &
                                " where WA.WasCode = '" & WasCode & "' AND WasType = '" & WasType & "'"
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rWac_accscope = New Container.Wac_accscope
                                rWac_accscope.LocID = drRow.Item("LocID")
                                rWac_accscope.WasCode = drRow.Item("WasCode")
                                rWac_accscope.WasType = drRow.Item("WasType")
                                rWac_accscope.HandlingCode = drRow.Item("HandlingCode")
                                rWac_accscope.DictionaryNo = drRow.Item("DictionaryNo")
                                rWac_accscope.COMPCode = drRow.Item("COMPCode")
                                rWac_accscope.SeqNo = drRow.Item("SeqNo")
                                rWac_accscope.CPPROP = drRow.Item("CPPROP")
                                rWac_accscope.CPFACTOR = drRow.Item("CPFACTOR")
                                rWac_accscope.CPSGM = drRow.Item("CPSGM")
                                rWac_accscope.CPMIN = drRow.Item("CPMIN")
                                rWac_accscope.CPSGX = drRow.Item("CPSGX")
                                rWac_accscope.CPMAX = drRow.Item("CPMAX")
                                rWac_accscope.CPSMD = drRow.Item("CPSMD")
                                rWac_accscope.CPMED = drRow.Item("CPMED")
                                rWac_accscope.CPSAV = drRow.Item("CPSAV")
                                rWac_accscope.CPAVG = drRow.Item("CPAVG")
                                rWac_accscope.MethodNo = drRow.Item("MethodNo")
                                rWac_accscope.UnitNo = drRow.Item("UnitNo")
                                rWac_accscope.DeletedAttempt = drRow.Item("DeletedAttempt")
                                rWac_accscope.DeletedReason = drRow.Item("DeletedReason")
                                rWac_accscope.ACStep = drRow.Item("ACStep")
                                rWac_accscope.Dependency = drRow.Item("Dependency")
                                rWac_accscope.Mode = drRow.Item("Mode")
                                rWac_accscope.ATMin = drRow.Item("ATMin")
                                rWac_accscope.ATMax = drRow.Item("ATMax")
                                rWac_accscope.ATNX = drRow.Item("ATNX")
                                rWac_accscope.ATTR = drRow.Item("ATTR")
                                rWac_accscope.DDGroup = drRow.Item("DDGroup")
                                rWac_accscope.Active = drRow.Item("Active")
                                lstWac_accscope.Add(rWac_accscope)
                            Next
                        Else
                            rWac_accscope = Nothing
                        End If
                        Return lstWac_accscope
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rWac_accscope = Nothing
                lstWac_accscope = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTreatmentCriteria(ByVal WasteCode As String, ByVal WasteType As String, Optional ByVal strFilter As String = "", Optional baseid As String = "", Optional locid As String = "", Optional dispose As Boolean = False) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_accscopeInfo.MyInfo
                    strSQL = "select distinct cm.Code, cm.CodeDesc from wac_basecomp wac INNER JOIN Codemaster cm ON cm.Code IN (SELECT VALUE FROM fn_Split(wac.HandlingCode,'-')) " & _
                        " Where wac.WasType='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "' AND wac.WasCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "' AND cm.CodeType='WTH' AND cm.Active = 1 "
                    If dispose = True Then
                        strSQL += " and CM.SysCode = '1'"
                    Else
                        strSQL += " and CM.SysCode = '0'"
                    End If
                    If baseid <> "" AndAlso locid <> "" Then
                        strSQL &= " AND wac.HandlingCode not in (select a.handlingcode from wac_accmaster a where a.BaseID='" & baseid & "' and a.WasCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "' and a.WasType='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "' AND a.HandlingCode <> '-' AND a.LocID='" & locid & "'  AND a.Flag=1 and a.Active=1) "
                    End If
                    If strFilter <> "" Then
                        strSQL &= strFilter
                    End If
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetComponetByWasteCode(ByVal WasteCode As String, ByVal WasteType As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_accscopeInfo.MyInfo
                    strSQL = "select wb.* from WAC_BASECOMP WB" &
                            " where Compcode not in (select COMPCode from WAC_ACCSCOPE WHERE WasCode = '" & WasteCode & "' and WasType = '" & WasteType & "') and handlingCode in(select handlingcode from wac_accmaster  WHERE WasCode = '" & WasteCode & "' and WasType = '" & WasteType & "') and WasCode = '" & WasteCode & "' and WasType = '" & WasteType & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        'Add By Dery 2017-06-16, Add GetWAC_HistoryList Query
        Public Overloads Function GetWAC_HistoryList(ByVal WasteCode As String, ByVal WasteType As String, ByVal userlocation As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_accscopeInfo.MyInfo
                    strSQL = "SELECT LocID, x.CreateDate, CreateBy, TotalComp, DictionaryNo " &
                             "FROM WAC_ACCMASTER w " &
                             "Outer Apply (select top 1 createdate from WAC_ACCMASTER wa where wa.DictionaryNo = w.DictionaryNo) x " &
                             "WHERE WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasteCode) & "' " &
                             "AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasteType) & "' " &
                             "AND LocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, userlocation) & "' and Posted = 1" &
                             "GROUP BY LocID, x.CreateDate, CreateBy, TotalComp, DictionaryNo ORDER BY x.CreateDate desc"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWAC_ACCSCOPEComponentCriteriaListDis(ByVal BaseID As String, ByVal LocID As String, Optional ByVal HandlingCode As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_accscopeInfo.MyInfo
                    strSQL = "Select *, WAC_ACCMASTER.Experiment AS ProResidue, 'false' AS RecStatus From WAC_ACCMASTER wac left join WAC_BASELINE wab on wac.BaseID = wab.BaseID and wac.WasCode = wab.WasCode and wac.WasType = wab.WasType WHERE wac.BaseID='" & BaseID & "' AND wac.LocId='" & LocID & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWAC_ACCSCOPEComponentCriteriaList(ByVal BaseID As String, ByVal RecLocID As String, ByVal LocID As String, Optional ByVal HandlingCode As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_accscopeInfo.MyInfo
                    strSQL = "Select bs.Mode,CASE WHEN f.ReceiverLocID is null THEN 'false' ELSE 'true' END AS RecStatus, bs.HandlingCode AS TreatCode, ISNULL(e.PercentProduct,'') AS PercentProduct, ISNULL(e.PercentResidue,'') AS PercentResidue, e.Experiment AS ProResidue, ISNULL(d.CodeDesc,'') AS TreatDesc, bs.COMPCode, bs.CPProp, bs.UnitNo AS DEFUOM, CASE bs.Mode WHEN 'NN' THEN b.COMPDescExt+ '*' ELSE b.COMPDescExt End AS COMPDesc, c.UOMDesc AS UOMDesc, bs.HandlingType, bs.CPFactor, bs.BaseID,CASE WHEN a.CPMIN=0 AND a.CPMAX=0 THEN 0 ELSE 1 END AS NIL,isnull(a.ConvertMin,0) as ConvertMin, isnull(a.ConvertMax,0) as ConvertMax, " &
                        " CASE WHEN ISNULL(a.CPMIN,0)=0 THEN '' ELSE cm.CodeDesc + CASE WHEN a.ConvertMin=0 then left(ltrim(str(a.CPMIN,20,10)),charindex('.',ltrim(str(a.CPMIN,20,10)))+iif(unv.Precision=0,-1,isnull(unv.Precision,iif(a.CPMIN<1,4,2)))) else left(ltrim(str(a.CPMIN/" & convert_.percent & ",20,10)),charindex('.',ltrim(str(a.CPMIN/" & convert_.percent & ",20,10)))+iif(unv.Precision=0,-1,isnull(unv.Precision,iif(a.CPMIN/" & convert_.percent & "<1,4,2)))) end END AS ValueMin, " &
                        " CASE WHEN ISNULL(a.CPMAX,0)=0 THEN '' ELSE cx.CodeDesc + CASE WHEN a.ConvertMax=0 then left(ltrim(str(a.CPMAX,20,10)),charindex('.',ltrim(str(a.CPMAX,20,10)))+iif(unv.Precision=0,-1,isnull(unv.Precision,iif(a.CPMAX<1,4,2)))) else left(ltrim(str(a.CPMAX/" & convert_.percent & ",20,10)),charindex('.',ltrim(str(a.CPMAX/" & convert_.percent & ",20,10)))+iif(unv.Precision=0,-1,isnull(unv.Precision,iif(a.CPMAX/" & convert_.percent & "<1,4,2)))) end END AS ValueMax, " &
                        " CASE WHEN unv.COMPCode is null then 'false' else 'true' END AS DisplayCheck, l.AllowDisposal, b.COMPType,'1' as HandlingGroup , '1' as CPBool, X.PrevMinValue, X.PrevMaxValue, bs.AllowDec, bs.Precision  " &
                        " from WAC_ACCMASTER wa " &
                        " outer APPLY (SELECT TOP 1 * FROM WAC_ACCMASTER e  WITH (NOLOCK) WHERE e.Flag=1 and e.LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, RecLocID) & "' and e.BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BaseID) & "' ORDER BY PostDate DESC) e " &
                        " left join WAC_BASECOMP bs on e.HandlingCode = bs.HandlingCode and wa.BaseID = bs.BaseID and wa.WasCode = bs.WasCode and wa.WasType = bs.WasType " &
                        " LEFT JOIN WAC_ACCSCOPE a WITH (NOLOCK) ON e.BaseID=a.BaseID AND a.HandlingCode=e.HandlingCode AND a.HandlingType=e.HandlingType  AND e.Active=1 AND bs.COMPCode=a.COMPCode AND a.LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, RecLocID) & "' and  a.DictionaryNo = e.DictionaryNo " &
                        " LEFT JOIN WAC_COMPCHART b WITH (NOLOCK) ON bs.COMPCode=b.COMPCode " &
                        " LEFT JOIN UOM c WITH (NOLOCK) ON b.DEFUOM = c.UOMCode " &
                        " LEFT JOIN CODEMASTER d WITH (NOLOCK) ON d.Code=bs.HandlingCode AND d.CodeType='WTH' " &
                        " LEFT JOIN CODEMASTER cm WITH(NOLOCK) ON cm.CodeType='CON' AND cm.Code=CAST(a.CPSGM AS VARCHAR) " &
                        " LEFT JOIN CODEMASTER cx WITH(NOLOCK) ON cx.CodeType='CON' AND cx.Code=CAST(a.CPSGX AS VARCHAR) " &
                        " LEFT JOIN WAC_WRLIST f WITH(NOLOCK) ON f.WasCode=a.WasCode AND f.WasType=a.WasType AND GeneratorID='0000' AND GeneratorLocID='0000' AND ReceiverID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' AND ReceiverLocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, RecLocID) & "' " &
                        " LEFT JOIN WAC_UNIVCOMP UNV WITH (NOLOCK) ON UNV.COMPCode = a.CompCode " &
                        " LEFT JOIN WAC_BASELINE l WITH (NOLOCK) ON bs.BaseID = l.BaseID and bs.WasCode=l.WasCode" &
                        " OUTER APPLY (SELECT CASE WHEN ISNULL(X.CPMIN,0)=0 THEN '' ELSE cmX.CodeDesc + CASE WHEN X.ConvertMin=0 then left(ltrim(str(X.CPMIN,20,10)),charindex('.',ltrim(str(X.CPMIN,20,10)))+iif(unv.Precision=0,-1,isnull(unv.Precision,iif(X.CPMIN<1,4,2)))) else left(ltrim(str(X.CPMIN/" & convert_.percent & ",20,10)),charindex('.',ltrim(str(X.CPMIN/" & convert_.percent & ",20,10)))+iif(unv.Precision=0,-1,isnull(unv.Precision,iif(X.CPMIN/" & convert_.percent & "<1,4,2)))) end END PrevMinValue," &
                        " CASE WHEN ISNULL(X.CPMAX,0)=0 THEN '' ELSE cxX.CodeDesc + CASE WHEN X.ConvertMax=0 then left(ltrim(str(X.CPMAX,20,10)),charindex('.',ltrim(str(X.CPMAX,20,10)))+iif(unv.Precision=0,-1,isnull(unv.Precision,iif(X.CPMAX<1,4,2)))) else left(ltrim(str(X.CPMAX/" & convert_.percent & ",20,10)),charindex('.',ltrim(str(X.CPMAX/" & convert_.percent & ",20,10)))+iif(unv.Precision=0,-1,isnull(unv.Precision,iif(X.CPMAX/" & convert_.percent & "<1,4,2)))) end END AS PrevMaxValue " &
                        " FROM WAC_ACCSCOPE X WITH (NOLOCK) INNER JOIN WAC_ACCMASTER Y ON Y.LOCID=X.LOCID AND Y.BASEID=X.BASEID AND Y.WASCODE=X.WASCODE AND Y.WASTYPE=X.WASTYPE AND Y.DICTIONARYNO=X.DICTIONARYNO " &
                        " LEFT JOIN CODEMASTER cmX WITH(NOLOCK) ON cmX.CodeType='CON' AND cmX.Code=CAST(X.CPSGM AS VARCHAR) " &
                        " LEFT JOIN CODEMASTER cxX WITH(NOLOCK) ON cxX.CodeType='CON' AND cxX.Code=CAST(X.CPSGX AS VARCHAR) " &
                        " WHERE Y.LOCID=a.LOCID AND Y.WASCODE=a.WASCODE AND Y.WASTYPE=a.WASTYPE AND Y.BASEID=a.BASEID AND Y.POSTED=1 AND Y.FLAG=1 AND X.COMPCODE=A.COMPCODE) X " &
                        " Where bs.BaseID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BaseID) & "' and e.Flag = 1 /*AND a.HandlingCode is not null*/ "
                    '" UNION " &
                    '" Select bs.Mode,CASE WHEN f.ReceiverLocID is null THEN 'false' ELSE 'true' END AS RecStatus, '' AS TreatCode, ISNULL(e.PercentProduct,'') AS PercentProduct, ISNULL(e.PercentResidue,'') AS PercentResidue, " &
                    '" 0 AS ProResidue, '' AS TreatDesc, '', bs.CPProp, bs.UnitNo AS DEFUOM, CASE bs.Mode WHEN 'NN' THEN b.COMPDescExt+' ('+b.CompDesc+')' + '*' ELSE b.COMPDescExt+' ('+b.CompDesc+')' End AS COMPDesc, c.UOMDesc AS UOMDesc," &
                    '" bs.HandlingType, bs.CPFactor, bs.BaseID,CASE WHEN a.CPMIN=0 AND a.CPMAX=0 THEN 0 ELSE 1 END AS NIL,isnull(a.ConvertMin,0) as ConvertMin, isnull(a.ConvertMax,0) as ConvertMax,  '0' as ValueMin,'0' ValueMax, " &
                    '" CASE WHEN unv.COMPCode is null then 'false' else 'true' END AS DisplayCheck, l.AllowDisposal, b.COMPType,a.HandlingGroup, '1' as CPBool  " &
                    '" from WAC_BASECOMP bs " &
                    '" LEFT JOIN WAC_ACCSCOPE a WITH (NOLOCK) ON bs.BaseID=a.BaseID AND a.HandlingCode=bs.HandlingCode AND a.HandlingType=bs.HandlingType  AND bs.Active=1 AND bs.COMPCode=a.COMPCode AND LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, RecLocID) & "' " &
                    '" LEFT JOIN WAC_COMPCHART b WITH (NOLOCK) ON B.CompType = 1 " &
                    '" LEFT JOIN UOM c WITH (NOLOCK) ON b.DEFUOM = c.UOMCode " &
                    '" LEFT JOIN CODEMASTER d WITH (NOLOCK) ON d.Code=bs.HandlingCode AND d.CodeType='WTH' " &
                    '" LEFT JOIN CODEMASTER cm WITH(NOLOCK) ON cm.CodeType='CON' AND cm.Code=CAST(a.CPSGM AS VARCHAR) " &
                    '" LEFT JOIN CODEMASTER cx WITH(NOLOCK) ON cx.CodeType='CON' AND cx.Code=CAST(a.CPSGX AS VARCHAR) " &
                    '" LEFT JOIN WAC_ACCMASTER e WITH (NOLOCK) ON a.BaseID=e.BaseID AND a.HandlingCode = e.HandlingCode AND e.BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BaseID) & "' AND e.LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, RecLocID) & "' " &
                    '" LEFT JOIN WAC_WRLIST f WITH(NOLOCK) ON f.WasCode=a.WasCode AND f.WasType=a.WasType AND GeneratorID='0000' AND GeneratorLocID='0000' AND ReceiverID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' AND ReceiverLocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, RecLocID) & "' " &
                    '" LEFT JOIN WAC_UNIVCOMP UNV WITH (NOLOCK) ON UNV.COMPCode = a.CompCode " &
                    '" LEFT JOIN WAC_BASELINE l WITH (NOLOCK) ON bs.BaseID = l.BaseID and bs.WasCode=l.WasCode" &
                    '" Where bs.BaseID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BaseID) & "' /*AND a.HandlingCode is not null*/ "
                   
                    If HandlingCode IsNot Nothing Then
                        strSQL &= " AND HandlingCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, HandlingCode) & "'"
                    End If
                    strSQL &= " GROUP BY bs.Mode,f.ReceiverLocID, bs.HandlingCode, e.PercentProduct, e.PercentResidue, e.Experiment, d.CodeDesc, bs.COMPCode, bs.CPProp, bs.UnitNo, b.COMPDescExt, b.CompDesc, c.UOMDesc, bs.HandlingType, bs.CPFactor, bs.BaseID,a.CPMIN, a.CPMAX, a.ConvertMin, a.ConvertMax, cm.CodeDesc,cx.CodeDesc,unv.COMPCode, l.AllowDisposal, b.COMPType, e.Posted, X.PrevMinValue, X.PrevMaxValue, bs.AllowDec, bs.Precision, unv.Precision  "
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWAC_ReactivityList(ByVal LocID As String, ByVal BaseID As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_accscopeInfo.MyInfo
                    strSQL = " select * from WAC_ACCSCOPE where BaseID = '" & BaseID & "' AND LocID = '" & LocID & "' and COMPCode = 0 "
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWAC_ACCSCOPEComponentCriteriaListRec(ByVal BaseID As String, ByVal LocID As String, ByVal BizID As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_accscopeInfo.MyInfo
                    strSQL = " Select bs.Mode,CASE WHEN e.ReceiverLocID is null THEN 'false' ELSE 'true' END AS RecStatus,bs.COMPCode, bs.CPPRop, bs.UnitNo AS DEFUOM, CASE bs.Mode WHEN 'NN' THEN b.COMPDescExt + '*' ELSE b.COMPDescExt End AS COMPDesc, c.UOMDesc AS UOMDesc, bs.HandlingType, bs.CPFactor, bs.BaseID, CASE WHEN a.CPMIN=0 AND a.CPMAX=0 THEN 0 ELSE 1 END AS NIL, isnull(a.ConvertMin,0) as ConvertMin, isnull(a.ConvertMax,0) as ConvertMax, " & _
                        " CASE WHEN ISNULL(a.CPMIN,0)=0 THEN '' ELSE cm.CodeDesc +  CASE WHEN a.ConvertMin=0 then convert(varchar,convert(numeric(16,2),a.cpmin)) else convert(varchar,convert(numeric(16,2),a.cpmin/" & convert_.percent & "))  end END AS ValueMin, CASE WHEN ISNULL(a.CPMAX,0)=0 THEN '' ELSE cx.CodeDesc + CASE WHEN a.ConvertMax=0 then convert(varchar,convert(numeric(16,2),a.cpmax)) else convert(varchar,convert(numeric(16,2),a.cpmax/" & convert_.percent & "))  end   END AS ValueMax, hdr.ProResidue, " & _
                        " CASE WHEN unv.COMPCode is null then 'false' else 'true' END AS DisplayCheck " & _
                        "from WAC_BASECOMP bs WITH (NOLOCK) " & _
                        " LEFT JOIN WAC_BASELINE hdr WITH(NOLOCK) ON bs.BaseID=hdr.BaseID" & _
                        " LEFT JOIN WAC_ACCSCOPE  a WITH (NOLOCK) ON bs.BaseID=a.BaseID AND a.HandlingCode=bs.HandlingCode AND a.HandlingType=bs.HandlingType  AND bs.Active=1 AND bs.COMPCode=a.COMPCode AND LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' " & _
                        " LEFT JOIN WAC_COMPCHART b WITH (NOLOCK) ON bs.COMPCode=b.COMPCode " & _
                        " LEFT JOIN UOM c WITH (NOLOCK) ON b.DEFUOM = c.UOMCode " & _
                        " LEFT JOIN CODEMASTER cm WITH(NOLOCK) ON cm.CodeType='CON' AND cm.Code=CAST(a.CPSGM AS VARCHAR) " & _
                        " LEFT JOIN CODEMASTER cx WITH(NOLOCK) ON cx.CodeType='CON' AND cx.Code=CAST(a.CPSGX AS VARCHAR) " & _
                        " LEFT JOIN WAC_WRLIST e WITH(NOLOCK) ON e.WasCode=a.WasCode AND e.WasType=a.WasType AND GeneratorID='0000' AND GeneratorLocID='0000' AND ReceiverID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizID) & "' AND ReceiverLocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "'" & _
                        " LEFT JOIN WAC_UNIVCOMP UNV WITH (NOLOCK) ON UNV.COMPCode = a.CompCode" & _
                        " Where bs.BaseID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BaseID) & "' AND a.HandlingCode is not null "
                    'strSQL = "Select DISTINCT a.Mode,CASE WHEN e.ReceiverLocID is null THEN 'false' ELSE 'true' END AS RecStatus,a.COMPCode, a.CPPRop, UnitNo AS DEFUOM, CASE a.Mode	WHEN 'NN' THEN b.COMPDesc + '*' ELSE b.COMPDesc End AS COMPDesc, c.UOMDesc AS UOMDesc, " & _
                    '    "a.HandlingType, a.CPFactor, a.BaseID,CASE WHEN CPMIN=0 THEN '' WHEN " & _
                    '    "CPSGM=0 THEN REPLACE(CAST(CPMIN AS VARCHAR),'.0000','') WHEN  CPSGM = 1 THEN '=' + " & _
                    '    "REPLACE(CAST(CPMIN AS VARCHAR),'.0000','') WHEN CPSGM=2 THEN '>' + REPLACE(CAST(CPMIN AS VARCHAR),'.0000','') " & _
                    '    "WHEN CPSGM=3 THEN '>=' + REPLACE(CAST(CPMIN AS VARCHAR),'.0000','') WHEN CPSGM =4 THEN '<' + " & _
                    '    "REPLACE(CAST(CPMIN AS VARCHAR),'.0000','') WHEN CPSGM=5 THEN '<=' + REPLACE(CAST(CPMIN AS VARCHAR),'.0000','') " & _
                    '    "END AS ValueMin, CASE WHEN CPMAX=0 THEN '' WHEN CPSGX=0 THEN REPLACE(CAST(CPMAX AS VARCHAR),'.0000','') WHEN  CPSGX = 1 THEN '=' + " & _
                    '    "REPLACE(CAST(CPMAX AS VARCHAR),'.0000','') WHEN CPSGX=2 THEN '>' + REPLACE(CAST(CPMAX AS VARCHAR),'.0000','') " & _
                    '    "WHEN CPSGX=3 THEN '>=' + REPLACE(CAST(CPMAX AS VARCHAR),'.0000','') WHEN CPSGX =4 THEN '<' + " & _
                    '    "REPLACE(CAST(CPMAX AS VARCHAR),'.0000','') WHEN CPSGX=5 THEN '<=' + REPLACE(CAST(CPMAX AS VARCHAR),'.0000','') " & _
                    '    "END AS ValueMax, CASE WHEN CPMIN=0 AND CPMAX=0 THEN 0 ELSE 1 END AS NIL from WAC_COMPCHART b WITH (NOLOCK) LEFT JOIN WAC_ACCSCOPE  a WITH (NOLOCK) ON a.COMPCode=b.COMPCode " & _
                    '    "LEFT JOIN UOM c WITH (NOLOCK) ON b.DEFUOM = c.UOMCode " & _
                    '    " LEFT JOIN WAC_WRLIST e WITH(NOLOCK) ON e.WasCode=a.WasCode AND e.WasType=a.WasType AND GeneratorID='0000' AND GeneratorLocID='0000' AND ReceiverID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizID) & "' AND ReceiverLocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "' " & _
                    '    "Where BaseID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BaseID) & "' AND LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "'"
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
Public Class Wac_accscope
	Public fLocID As System.String = "LocID"
	Public fBaseID As System.String = "BaseID"
	Public fWasCode As System.String = "WasCode"
	Public fWasType As System.String = "WasType"
	Public fHandlingType As System.String = "HandlingType"
	Public fHandlingCode As System.String = "HandlingCode"
	Public fDictionaryNo As System.String = "DictionaryNo"
	Public fCOMPCode As System.String = "COMPCode"
	Public fSeqNo As System.String = "SeqNo"
	Public fCPPROP As System.String = "CPPROP"
	Public fCPFACTOR As System.String = "CPFACTOR"
	Public fCPSGM As System.String = "CPSGM"
	Public fCPMIN As System.String = "CPMIN"
	Public fCPSGX As System.String = "CPSGX"
	Public fCPMAX As System.String = "CPMAX"
	Public fCPSMD As System.String = "CPSMD"
	Public fCPMED As System.String = "CPMED"
	Public fCPSAV As System.String = "CPSAV"
	Public fCPAVG As System.String = "CPAVG"
	Public fMethodNo As System.String = "MethodNo"
	Public fUnitNo As System.String = "UnitNo"
	Public fDeletedAttempt As System.String = "DeletedAttempt"
	Public fDeletedReason As System.String = "DeletedReason"
            Public fACStep As System.String = "ACStep"
	Public fDependency As System.String = "Dependency"
	Public fMode As System.String = "Mode"
	Public fATMin As System.String = "ATMin"
	Public fATMax As System.String = "ATMax"
	Public fATNX As System.String = "ATNX"
	Public fATTR As System.String = "ATTR"
	Public fDDGroup As System.String = "DDGroup"
	Public fActive As System.String = "Active"
	Public fCreateDate As System.String = "CreateDate"
	Public fCreateBy As System.String = "CreateBy"
	Public fLastUpdate As System.String = "LastUpdate"
            Public fUpdateBy As System.String = "UpdateBy"
            Public fConvertMin As System.String = "ConvertMin"
            Public fConvertMax As System.String = "ConvertMax"

	Protected _LocID As System.String
	Protected _BaseID As System.String
	Protected _WasCode As System.String
            Protected _WasType As System.String
            Protected _HandlingType As System.Int32
            Protected _HandlingGroup As System.Int32
            Protected _ReactivityValue As System.Int32
	Protected _HandlingCode As System.String
	Protected _DictionaryNo As System.String
	Protected _COMPCode As System.Int32
	Private _SeqNo As System.Int32
	Private _CPPROP As System.Byte
	Private _CPFACTOR As System.String
	Private _CPSGM As System.Byte
	Private _CPMIN As System.Decimal
	Private _CPSGX As System.Byte
	Private _CPMAX As System.Decimal
	Private _CPSMD As System.Byte
	Private _CPMED As System.Decimal
	Private _CPSAV As System.Byte
	Private _CPAVG As System.Decimal
	Private _MethodNo As System.Int32
	Private _UnitNo As System.String
	Private _DeletedAttempt As System.Int32
	Private _DeletedReason As System.String
            Private _ACStep As System.Int32
	Private _Dependency As System.Int32
	Private _Mode As System.String
	Private _ATMin As System.String
	Private _ATMax As System.String
	Private _ATNX As System.String
	Private _ATTR As System.String
	Private _DDGroup As System.Int32
	Private _Active As System.Byte
	Private _CreateDate As System.DateTime
	Private _CreateBy As System.String
	Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _ConvertMin As System.Int32
            Private _ConvertMax As System.Int32

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
            Public Property ReactivityValue As System.Int32
                Get
                    Return _ReactivityValue
                End Get
                Set(ByVal Value As System.Int32)
                    _ReactivityValue = Value
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
	Public Property COMPCode As System.Int32
		Get 
			Return _COMPCode
		End Get
		Set(ByVal Value As System.Int32)
			_COMPCode = Value
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
	Public Property CPPROP As System.Byte
		Get 
			Return _CPPROP
		End Get
		Set(ByVal Value As System.Byte)
			_CPPROP = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property CPFACTOR As System.String
		Get 
			Return _CPFACTOR
		End Get
		Set(ByVal Value As System.String)
			_CPFACTOR = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property CPSGM As System.Byte
		Get 
			Return _CPSGM
		End Get
		Set(ByVal Value As System.Byte)
			_CPSGM = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property CPMIN As System.Decimal
		Get 
			Return _CPMIN
		End Get
		Set(ByVal Value As System.Decimal)
			_CPMIN = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property CPSGX As System.Byte
		Get 
			Return _CPSGX
		End Get
		Set(ByVal Value As System.Byte)
			_CPSGX = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property CPMAX As System.Decimal
		Get 
			Return _CPMAX
		End Get
		Set(ByVal Value As System.Decimal)
			_CPMAX = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property CPSMD As System.Byte
		Get 
			Return _CPSMD
		End Get
		Set(ByVal Value As System.Byte)
			_CPSMD = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property CPMED As System.Decimal
		Get 
			Return _CPMED
		End Get
		Set(ByVal Value As System.Decimal)
			_CPMED = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property CPSAV As System.Byte
		Get 
			Return _CPSAV
		End Get
		Set(ByVal Value As System.Byte)
			_CPSAV = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property CPAVG As System.Decimal
		Get 
			Return _CPAVG
		End Get
		Set(ByVal Value As System.Decimal)
			_CPAVG = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property MethodNo As System.Int32
		Get 
			Return _MethodNo
		End Get
		Set(ByVal Value As System.Int32)
			_MethodNo = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property UnitNo As System.String
		Get 
			Return _UnitNo
		End Get
		Set(ByVal Value As System.String)
			_UnitNo = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property DeletedAttempt As System.Int32
		Get 
			Return _DeletedAttempt
		End Get
		Set(ByVal Value As System.Int32)
			_DeletedAttempt = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property DeletedReason As System.String
		Get 
			Return _DeletedReason
		End Get
		Set(ByVal Value As System.String)
			_DeletedReason = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
            Public Property ACStep As System.Int32
                Get
                    Return _ACStep
                End Get
                Set(ByVal Value As System.Int32)
                    _ACStep = Value
                End Set
            End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property Dependency As System.Int32
		Get 
			Return _Dependency
		End Get
		Set(ByVal Value As System.Int32)
			_Dependency = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property Mode As System.String
		Get 
			Return _Mode
		End Get
		Set(ByVal Value As System.String)
			_Mode = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property ATMin As System.String
		Get 
			Return _ATMin
		End Get
		Set(ByVal Value As System.String)
			_ATMin = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property ATMax As System.String
		Get 
			Return _ATMax
		End Get
		Set(ByVal Value As System.String)
			_ATMax = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property ATNX As System.String
		Get 
			Return _ATNX
		End Get
		Set(ByVal Value As System.String)
			_ATNX = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property ATTR As System.String
		Get 
			Return _ATTR
		End Get
		Set(ByVal Value As System.String)
			_ATTR = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property DDGroup As System.Int32
		Get 
			Return _DDGroup
		End Get
		Set(ByVal Value As System.Int32)
			_DDGroup = Value
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

            Public Property ConvertMin As System.Int32
                Get
                    Return _ConvertMin
                End Get
                Set(ByVal Value As System.Int32)
                    _ConvertMin = Value
                End Set
            End Property

            Public Property ConvertMax As System.Int32
                Get
                    Return _ConvertMax
                End Get
                Set(ByVal Value As System.Int32)
                    _ConvertMax = Value
                End Set
            End Property
	
End Class
#End Region
End Namespace

#Region "Class Info"
public Class Wac_accscopeInfo
		Inherits Core.CoreBase
	Protected Overrides Sub InitializeClassInfo()
		With MyInfo
                .FieldsList = "LocID,BaseID,WasCode,WasType,HandlingType,HandlingCode,DictionaryNo,COMPCode,SeqNo,CPPROP,CPFACTOR,CPSGM,CPMIN,CPSGX,CPMAX,CPSMD,CPMED,CPSAV,CPAVG,MethodNo,UnitNo,DeletedAttempt,DeletedReason,ACStep,Dependency,Mode,ATMin,ATMax,ATNX,ATTR,DDGroup,Active,CreateDate,CreateBy,LastUpdate,UpdateBy"
			.CheckFields = "CPPROP,CPSGM,CPSGX,CPSMD,CPSAV,Active"
			.TableName = "Wac_accscope"
            .DefaultCond = Nothing
            .DefaultOrder = Nothing
                .Listing = "LocID,BaseID,WasCode,WasType,HandlingType,HandlingCode,DictionaryNo,COMPCode,SeqNo,CPPROP,CPFACTOR,CPSGM,CPMIN,CPSGX,CPMAX,CPSMD,CPMED,CPSAV,CPAVG,MethodNo,UnitNo,DeletedAttempt,DeletedReason,ACStep,Dependency,Mode,ATMin,ATMax,ATNX,ATTR,DDGroup,Active,CreateDate,CreateBy,LastUpdate,UpdateBy"
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
Public Class WAC_ACCSCOPEScheme
Inherits Core.SchemeBase
	Protected Overrides Sub InitializeInfo()
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "LocID"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(0,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "BaseID"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(1,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "WasCode"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(2,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "WasType"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(3,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "HandlingType"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(4,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "HandlingCode"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(5,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "DictionaryNo"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(6,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "COMPCode"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(7,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "SeqNo"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(8,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CPPROP"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(9,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "CPFACTOR"
				.Length = 10
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(10,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CPSGM"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(11,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CPMIN"
				.Length = 9
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(12,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CPSGX"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(13,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CPMAX"
				.Length = 9
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(14,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CPSMD"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(15,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CPMED"
				.Length = 9
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(16,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CPSAV"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(17,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CPAVG"
				.Length = 9
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(18,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "MethodNo"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(19,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "UnitNo"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(20,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "DeletedAttempt"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(21,this)
			With this
				.DataType = SQLControl.EnumDataType.dtStringN
				.FieldName = "DeletedReason"
				.Length = 50
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(22,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ACStep"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(23,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Dependency"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(24,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "Mode"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(25,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "ATMin"
				.Length = 30
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(26,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "ATMax"
				.Length = 30
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(27,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "ATNX"
				.Length = 30
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(28,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "ATTR"
				.Length = 30
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(29,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "DDGroup"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(30,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Active"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(31,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "CreateDate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = False
				.AllowNegative = False
			End With
			MyBase.AddItem(32,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "CreateBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(33,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "LastUpdate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = False
				.AllowNegative = False
			End With
			MyBase.AddItem(34,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "UpdateBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(35,this)
		
	End Sub
	
		Public ReadOnly Property LocID As StrucElement
			Get
                Return MyBase.GetItem(0)
            End Get
        End Property
		Public ReadOnly Property BaseID As StrucElement
			Get
                Return MyBase.GetItem(1)
            End Get
        End Property
		Public ReadOnly Property WasCode As StrucElement
			Get
                Return MyBase.GetItem(2)
            End Get
        End Property
		Public ReadOnly Property WasType As StrucElement
			Get
                Return MyBase.GetItem(3)
            End Get
        End Property
		Public ReadOnly Property HandlingType As StrucElement
			Get
                Return MyBase.GetItem(4)
            End Get
        End Property
		Public ReadOnly Property HandlingCode As StrucElement
			Get
                Return MyBase.GetItem(5)
            End Get
        End Property
		Public ReadOnly Property DictionaryNo As StrucElement
			Get
                Return MyBase.GetItem(6)
            End Get
        End Property
		Public ReadOnly Property COMPCode As StrucElement
			Get
                Return MyBase.GetItem(7)
            End Get
        End Property
	
		Public ReadOnly Property SeqNo As StrucElement
			Get
                Return MyBase.GetItem(8)
            End Get
        End Property
		Public ReadOnly Property CPPROP As StrucElement
			Get
                Return MyBase.GetItem(9)
            End Get
        End Property
		Public ReadOnly Property CPFACTOR As StrucElement
			Get
                Return MyBase.GetItem(10)
            End Get
        End Property
		Public ReadOnly Property CPSGM As StrucElement
			Get
                Return MyBase.GetItem(11)
            End Get
        End Property
		Public ReadOnly Property CPMIN As StrucElement
			Get
                Return MyBase.GetItem(12)
            End Get
        End Property
		Public ReadOnly Property CPSGX As StrucElement
			Get
                Return MyBase.GetItem(13)
            End Get
        End Property
		Public ReadOnly Property CPMAX As StrucElement
			Get
                Return MyBase.GetItem(14)
            End Get
        End Property
		Public ReadOnly Property CPSMD As StrucElement
			Get
                Return MyBase.GetItem(15)
            End Get
        End Property
		Public ReadOnly Property CPMED As StrucElement
			Get
                Return MyBase.GetItem(16)
            End Get
        End Property
		Public ReadOnly Property CPSAV As StrucElement
			Get
                Return MyBase.GetItem(17)
            End Get
        End Property
		Public ReadOnly Property CPAVG As StrucElement
			Get
                Return MyBase.GetItem(18)
            End Get
        End Property
		Public ReadOnly Property MethodNo As StrucElement
			Get
                Return MyBase.GetItem(19)
            End Get
        End Property
		Public ReadOnly Property UnitNo As StrucElement
			Get
                Return MyBase.GetItem(20)
            End Get
        End Property
		Public ReadOnly Property DeletedAttempt As StrucElement
			Get
                Return MyBase.GetItem(21)
            End Get
        End Property
		Public ReadOnly Property DeletedReason As StrucElement
			Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property ACStep As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
		Public ReadOnly Property Dependency As StrucElement
			Get
                Return MyBase.GetItem(24)
            End Get
        End Property
		Public ReadOnly Property Mode As StrucElement
			Get
                Return MyBase.GetItem(25)
            End Get
        End Property
		Public ReadOnly Property ATMin As StrucElement
			Get
                Return MyBase.GetItem(26)
            End Get
        End Property
		Public ReadOnly Property ATMax As StrucElement
			Get
                Return MyBase.GetItem(27)
            End Get
        End Property
		Public ReadOnly Property ATNX As StrucElement
			Get
                Return MyBase.GetItem(28)
            End Get
        End Property
		Public ReadOnly Property ATTR As StrucElement
			Get
                Return MyBase.GetItem(29)
            End Get
        End Property
		Public ReadOnly Property DDGroup As StrucElement
			Get
                Return MyBase.GetItem(30)
            End Get
        End Property
		Public ReadOnly Property Active As StrucElement
			Get
                Return MyBase.GetItem(31)
            End Get
        End Property
		Public ReadOnly Property CreateDate As StrucElement
			Get
                Return MyBase.GetItem(32)
            End Get
        End Property
		Public ReadOnly Property CreateBy As StrucElement
			Get
                Return MyBase.GetItem(33)
            End Get
        End Property
		Public ReadOnly Property LastUpdate As StrucElement
			Get
                Return MyBase.GetItem(34)
            End Get
        End Property
		Public ReadOnly Property UpdateBy As StrucElement
			Get
                Return MyBase.GetItem(35)
            End Get
        End Property

    Public Function GetElement(ByVal Key As Integer) As StrucElement
        Return MyBase.GetItem(Key)
    End Function
End Class
#End Region

End Namespace

Imports SEAL.Data
imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace Actions
Public NotInheritable Class WAC_BASECOMP
	Inherits Core.CoreControl
	Private Wac_basecompInfo As Wac_basecompInfo = New Wac_basecompInfo
	
	Public Sub New(ByVal connecn As String)
        ConnectionString = connecn
        ConnectionSetup()
    End Sub
		
	#Region "Data Manipulation-Add,Edit,Del"
	Private Function Save(ByVal Wac_basecompCont As Container.Wac_basecomp, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
		Dim strSQL As String = ""
        Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
        Dim rdr As System.Data.SqlClient.SqlDataReader
        Save = False
        Try
			If Wac_basecompCont Is Nothing Then
                'Message return
            Else
				blnExec = False
                blnFound = False
                blnFlag = False
				If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
					With Wac_basecompInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_basecompCont.HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.DictionaryNo) & "' AND COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_basecompCont.COMPCode) & "'")
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
                                .TableName = "Wac_basecomp"
                                .AddField("SeqNo", Wac_basecompCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPPROP", Wac_basecompCont.CPPROP, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPFACTOR", Wac_basecompCont.CPFACTOR, SQLControl.EnumDataType.dtString)
                                .AddField("CPSGM", Wac_basecompCont.CPSGM, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPMIN", Wac_basecompCont.CPMIN, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPSGX", Wac_basecompCont.CPSGX, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPMAX", Wac_basecompCont.CPMAX, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPSMD", Wac_basecompCont.CPSMD, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPMED", Wac_basecompCont.CPMED, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPSAV", Wac_basecompCont.CPSAV, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPAVG", Wac_basecompCont.CPAVG, SQLControl.EnumDataType.dtNumeric)
                                .AddField("MethodNo", Wac_basecompCont.MethodNo, SQLControl.EnumDataType.dtNumeric)
                                .AddField("UnitNo", Wac_basecompCont.UnitNo, SQLControl.EnumDataType.dtString)
                                .AddField("DeletedAttempt", Wac_basecompCont.DeletedAttempt, SQLControl.EnumDataType.dtNumeric)
                                .AddField("DeletedReason", Wac_basecompCont.DeletedReason, SQLControl.EnumDataType.dtStringN)
                                .AddField("BCStep", Wac_basecompCont.BCStep, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Dependency", Wac_basecompCont.Dependency, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Mode", Wac_basecompCont.Mode, SQLControl.EnumDataType.dtString)
                                .AddField("ATMin", Wac_basecompCont.ATMin, SQLControl.EnumDataType.dtString)
                                .AddField("ATMax", Wac_basecompCont.ATMax, SQLControl.EnumDataType.dtString)
                                .AddField("ATNX", Wac_basecompCont.ATNX, SQLControl.EnumDataType.dtString)
                                .AddField("ATTR", Wac_basecompCont.ATTR, SQLControl.EnumDataType.dtString)
                                .AddField("DDGroup", Wac_basecompCont.DDGroup, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Active", Wac_basecompCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", Wac_basecompCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", Wac_basecompCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", Wac_basecompCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", Wac_basecompCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("ConvertMin", Wac_basecompCont.ConvertMin, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ConvertMax", Wac_basecompCont.ConvertMax, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_basecompCont.HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.DictionaryNo) & "' AND COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_basecompCont.COMPCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("BaseID", Wac_basecompCont.BaseID, SQLControl.EnumDataType.dtString)
                                                .AddField("WasCode", Wac_basecompCont.WasCode, SQLControl.EnumDataType.dtString)
                                                .AddField("WasType", Wac_basecompCont.WasType, SQLControl.EnumDataType.dtString)
                                                .AddField("HandlingType", Wac_basecompCont.HandlingType, SQLControl.EnumDataType.dtNumeric)
                                                .AddField("HandlingCode", Wac_basecompCont.HandlingCode, SQLControl.EnumDataType.dtString)
                                                .AddField("DictionaryNo", Wac_basecompCont.DictionaryNo, SQLControl.EnumDataType.dtString)
                                                .AddField("COMPCode", Wac_basecompCont.COMPCode, SQLControl.EnumDataType.dtNumeric)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_basecompCont.HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.DictionaryNo) & "' AND COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_basecompCont.COMPCode) & "'")
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
                Wac_basecompCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal Wac_basecompCont As Container.Wac_basecomp, ByRef message As String) As Boolean
            Return Save(Wac_basecompCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal Wac_basecompCont As Container.Wac_basecomp, ByRef message As String) As Boolean
            Return Save(Wac_basecompCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal Wac_basecompCont As Container.Wac_basecomp, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Wac_basecompCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Wac_basecompInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_basecompCont.HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.DictionaryNo) & "' AND COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_basecompCont.COMPCode) & "'")
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
                                strSQL = BuildUpdate(Wac_basecompInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Wac_basecompCont.LastUpdate) & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.UpdateBy) & "' WHERE" & _
                                "BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_basecompCont.HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.DictionaryNo) & "' AND COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_basecompCont.COMPCode) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Wac_basecompInfo.MyInfo.TableName, "BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_basecompCont.HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basecompCont.DictionaryNo) & "' AND COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Wac_basecompCont.COMPCode) & "'")
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
                Wac_basecompCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region
	
	#Region "Data Selection"
        Public Overloads Function GetWAC_BASECOMP(ByVal BaseID As System.String, ByVal WasCode As System.String, ByVal WasType As System.String, ByVal HandlingType As System.Int32, ByVal HandlingCode As System.String, ByVal DictionaryNo As System.String, ByVal COMPCode As System.Int32) As Container.Wac_basecomp
            Dim rWac_basecomp As Container.Wac_basecomp = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With Wac_basecompInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, HandlingCode) & "' AND COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, COMPCode) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWac_basecomp = New Container.Wac_basecomp
                                rWac_basecomp.BaseID = drRow.Item("BaseID")
                                rWac_basecomp.WasCode = drRow.Item("WasCode")
                                rWac_basecomp.WasType = drRow.Item("WasType")
                                rWac_basecomp.HandlingType = drRow.Item("HandlingType")
                                rWac_basecomp.HandlingCode = drRow.Item("HandlingCode")
                                rWac_basecomp.DictionaryNo = drRow.Item("DictionaryNo")
                                rWac_basecomp.COMPCode = drRow.Item("COMPCode")
                                rWac_basecomp.SeqNo = drRow.Item("SeqNo")
                                rWac_basecomp.CPPROP = drRow.Item("CPPROP")
                                rWac_basecomp.CPFACTOR = drRow.Item("CPFACTOR")
                                rWac_basecomp.CPSGM = drRow.Item("CPSGM")
                                rWac_basecomp.CPMIN = drRow.Item("CPMIN")
                                rWac_basecomp.CPSGX = drRow.Item("CPSGX")
                                rWac_basecomp.CPMAX = drRow.Item("CPMAX")
                                rWac_basecomp.CPSMD = drRow.Item("CPSMD")
                                rWac_basecomp.CPMED = drRow.Item("CPMED")
                                rWac_basecomp.CPSAV = drRow.Item("CPSAV")
                                rWac_basecomp.CPAVG = drRow.Item("CPAVG")
                                rWac_basecomp.MethodNo = drRow.Item("MethodNo")
                                rWac_basecomp.UnitNo = drRow.Item("UnitNo")
                                rWac_basecomp.DeletedAttempt = drRow.Item("DeletedAttempt")
                                rWac_basecomp.DeletedReason = drRow.Item("DeletedReason")
                                rWac_basecomp.BCStep = drRow.Item("BCStep")
                                rWac_basecomp.Dependency = drRow.Item("Dependency")
                                rWac_basecomp.Mode = drRow.Item("Mode")
                                rWac_basecomp.ATMin = drRow.Item("ATMin")
                                rWac_basecomp.ATMax = drRow.Item("ATMax")
                                rWac_basecomp.ATNX = drRow.Item("ATNX")
                                rWac_basecomp.ATTR = drRow.Item("ATTR")
                                rWac_basecomp.DDGroup = drRow.Item("DDGroup")
                                rWac_basecomp.Active = drRow.Item("Active")
                                rWac_basecomp.CreateBy = drRow.Item("CreateBy")
                                rWac_basecomp.UpdateBy = drRow.Item("UpdateBy")
                                rWac_basecomp.ConvertMin = drRow.Item("ConvertMin")
                                rWac_basecomp.ConvertMax = drRow.Item("ConvertMax")
                            Else
                                rWac_basecomp = Nothing
                            End If
                        Else
                            rWac_basecomp = Nothing
                        End If
                    End With
                End If
                Return rWac_basecomp
            Catch ex As Exception
                Throw ex
            Finally
                rWac_basecomp = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_BASECOMP(ByVal BaseID As System.String, ByVal WasCode As System.String, ByVal WasType As System.String, ByVal HandlingType As System.Int32, ByVal HandlingCode As System.String, ByVal DictionaryNo As System.String, ByVal COMPCode As System.Int32, ByVal DecendingOrder As Boolean) As List(Of Container.Wac_basecomp)
            Dim rWac_basecomp As Container.Wac_basecomp = Nothing
            Dim lstWac_basecomp As List(Of Container.Wac_basecomp) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With Wac_basecompInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal BaseID As System.String, ByVal WasCode As System.String, ByVal WasType As System.Int32, ByVal HandlingType As System.Int32, ByVal HandlingCode As System.String, ByVal DictionaryNo As System.String, ByVal COMPCode As System.Int32 DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasType) & "' AND HandlingType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, HandlingType) & "' AND HandlingCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, HandlingCode) & "' AND DictionaryNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, HandlingCode) & "' AND COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, COMPCode) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rWac_basecomp = New Container.Wac_basecomp
                                rWac_basecomp.BaseID = drRow.Item("BaseID")
                                rWac_basecomp.WasCode = drRow.Item("WasCode")
                                rWac_basecomp.WasType = drRow.Item("WasType")
                                rWac_basecomp.HandlingType = drRow.Item("HandlingType")
                                rWac_basecomp.HandlingCode = drRow.Item("HandlingCode")
                                rWac_basecomp.DictionaryNo = drRow.Item("DictionaryNo")
                                rWac_basecomp.COMPCode = drRow.Item("COMPCode")
                                rWac_basecomp.SeqNo = drRow.Item("SeqNo")
                                rWac_basecomp.CPPROP = drRow.Item("CPPROP")
                                rWac_basecomp.CPFACTOR = drRow.Item("CPFACTOR")
                                rWac_basecomp.CPSGM = drRow.Item("CPSGM")
                                rWac_basecomp.CPMIN = drRow.Item("CPMIN")
                                rWac_basecomp.CPSGX = drRow.Item("CPSGX")
                                rWac_basecomp.CPMAX = drRow.Item("CPMAX")
                                rWac_basecomp.CPSMD = drRow.Item("CPSMD")
                                rWac_basecomp.CPMED = drRow.Item("CPMED")
                                rWac_basecomp.CPSAV = drRow.Item("CPSAV")
                                rWac_basecomp.CPAVG = drRow.Item("CPAVG")
                                rWac_basecomp.MethodNo = drRow.Item("MethodNo")
                                rWac_basecomp.UnitNo = drRow.Item("UnitNo")
                                rWac_basecomp.DeletedAttempt = drRow.Item("DeletedAttempt")
                                rWac_basecomp.DeletedReason = drRow.Item("DeletedReason")
                                rWac_basecomp.BCStep = drRow.Item("BCStep")
                                rWac_basecomp.Dependency = drRow.Item("Dependency")
                                rWac_basecomp.Mode = drRow.Item("Mode")
                                rWac_basecomp.ATMin = drRow.Item("ATMin")
                                rWac_basecomp.ATMax = drRow.Item("ATMax")
                                rWac_basecomp.ATNX = drRow.Item("ATNX")
                                rWac_basecomp.ATTR = drRow.Item("ATTR")
                                rWac_basecomp.DDGroup = drRow.Item("DDGroup")
                                rWac_basecomp.Active = drRow.Item("Active")
                                rWac_basecomp.CreateBy = drRow.Item("CreateBy")
                                rWac_basecomp.UpdateBy = drRow.Item("UpdateBy")
                                rWac_basecomp.ConvertMin = drRow.Item("ConvertMin")
                                rWac_basecomp.ConvertMax = drRow.Item("ConvertMax")
                            Next
                            lstWac_basecomp.Add(rWac_basecomp)
                        Else
                            rWac_basecomp = Nothing
                        End If
                        Return lstWac_basecomp
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rWac_basecomp = Nothing
                lstWac_basecomp = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_BASECOMPTreatmentList(ByVal WasteType As String, ByVal HandlingType As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_basecompInfo.MyInfo
                    strSQL = "select DISTINCT BC.HandlingCode, CM.CodeDesc" & _
                        " from WAC_BASECOMP BC INNER JOIN CODEMASTER CM ON BC.HandlingCode = CM.Code" & _
                        " WHERE CM.CodeType='WTH' AND BC.WasType='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "'" & _
                        " AND BC.HandlingType='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, HandlingType) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function getQuestion(ByVal WasteType As String, ByVal WasteCode As String, ByVal BaseID As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_basecompInfo.MyInfo
                    strSQL = "select B.QuestID, B.QuestDesc, REPLACE(B.RefValue,'|',',') as RefValue ,C.CodeDesc As Category,B.ControlType, CM.CodeDesc as RoleDesc , B.Role, B.Active, iif(B.Active = 1,'Active', 'Not Active') As Status from WAC_BASEQUEST B WITH (NOLOCK) INNER JOIN CODEMASTER C WITH (NOLOCK) ON B.ControlType = C.Code AND C.CodeType= 'SYS' INNER JOIN CODEMASTER CM WITH (NOLOCK) ON B.Role = CM.Code AND CM.CodeType= 'QRL' " & _
                        " WHERE WasType='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "'" & _
                        " AND WasCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "'" & _
                        " AND BaseID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, BaseID) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWAC_BASECOMPTreatmentList(ByVal BaseID As String, ByVal WasteType As String, ByVal WasteCode As String, ByVal HandlingType As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_basecompInfo.MyInfo
                    strSQL = "SELECT DISTINCT a.HandlingCode AS HandlingCode,b.CodeDesc AS TreatmentDesc, cm.CodeDesc as Groups FROM Wac_basecomp a INNER JOIN codemaster b ON a.HandlingCode = b.Code AND b.CodeType='WTH'  INNER JOIN Codemaster cm ON cm.Code = b.syscode and cm.CodeType = 'GRP' " &
                            " Where BaseID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BaseID) & "' AND WasType='" & WasteType & "' AND WasCode='" & WasteCode & "' AND HandlingCode <> '-' /*AND a.HandlingType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, HandlingType) & "'*/"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetReactivityValueChar(ByVal BaseID As String, ByVal WasteType As String, ByVal WasteCode As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_basecompInfo.MyInfo
                    strSQL = "SELECT CPBool FROM Wac_Charscope Where BaseID = '" & BaseID & "' AND WasCode = '" & WasteCode & "' AND WasType ='" & WasteType & "' AND CompCode = 0"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWAC_BASECOMPComponentList(ByVal BaseID As String, ByVal CPFactor As String, ByVal CPCrop As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_basecompInfo.MyInfo
                    
                    strSQL = "Select Distinct a.COMPCode, UnitNo AS DEFUOM, b.COMPDescExt+' ('+b.CompDesc+')' AS COMPDesc, c.UOMDesc AS UOMDesc, a.Mode, a.ConvertMin, a.ConvertMax, " & _
                            " CASE WHEN ISNULL(a.CPMIN,0)=0 THEN '' ELSE cm.CodeDesc + CASE WHEN a.ConvertMin=0 then left(ltrim(str(a.CPMIN,20,10)),charindex('.',ltrim(str(a.CPMIN,20,10)))+iif(unv.Precision=0,-1,isnull(unv.Precision,iif(a.CPMIN<1,4,2)))) else left(ltrim(str(a.CPMIN/" & convert_.percent & ",20,10)),charindex('.',ltrim(str(a.CPMIN/" & convert_.percent & ",20,10)))+iif(unv.Precision=0,-1,isnull(unv.Precision,iif(a.CPMIN/" & convert_.percent & "<1,4,2)))) end END AS ValueMin, " & _
                            " CASE WHEN ISNULL(a.CPMAX,0)=0 THEN '' ELSE cx.CodeDesc + CASE WHEN a.ConvertMax=0 then left(ltrim(str(a.CPMAX,20,10)),charindex('.',ltrim(str(a.CPMAX,20,10)))+iif(unv.Precision=0,-1,isnull(unv.Precision,iif(a.CPMAX<1,4,2)))) else left(ltrim(str(a.CPMAX/" & convert_.percent & ",20,10)),charindex('.',ltrim(str(a.CPMAX/" & convert_.percent & ",20,10)))+iif(unv.Precision=0,-1,isnull(unv.Precision,iif(a.CPMAX/" & convert_.percent & "<1,4,2)))) end END AS ValueMax, " & _
                            " a.DDGroup, CASE WHEN unv.COMPCode IS null THEN 'false' ELSE 'true' END AS DisplayCheck, a.AllowDec, a.Precision " & _
                            " FROM WAC_COMPCHART b WITH (NOLOCK) LEFT JOIN wac_basecomp a WITH (NOLOCK) ON a.COMPCode=b.COMPCode " & _
                            " LEFT JOIN CODEMASTER cm WITH(NOLOCK) ON cm.CodeType='CON' AND cm.Code=CAST(a.CPSGM AS VARCHAR) " & _
                            " LEFT JOIN CODEMASTER cx WITH(NOLOCK) ON cx.CodeType='CON' AND cx.Code=CAST(a.CPSGX AS VARCHAR) " & _
                            " LEFT JOIN UOM c WITH (NOLOCK) ON b.DEFUOM = c.UOMCode " & _
                            " LEFT JOIN WAC_UNIVCOMP unv WITH (NOLOCK) ON unv.COMPCode = b.COMPCode "

                    strSQL &= " WHERE BaseID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BaseID) & "' AND CPFACTOR='" & CPFactor & "' AND CPPROP='" & CPCrop & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWAC_BASECOMPComponentCriteriaList(ByVal WasteCode As String, ByVal WasteType As String, ByVal RecLocID As String, ByVal RecID As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_basecompInfo.MyInfo
                    strSQL = "Select Distinct CASE WHEN e.ReceiverLocID is null THEN 'false' ELSE 'true' END AS RecStatus, a.COMPCode, ISNULL(d.ProResidue,0) AS ProResidue, UnitNo AS DEFUOM, a.Mode AS Mode, CASE a.Mode	WHEN 'NN' THEN b.COMPDescExt + '*' ELSE b.COMPDescExt End AS COMPDesc, c.UOMDesc AS UOMDesc, a.HandlingType, a.CPFactor, a.CPPROP, a.BaseID, " & _
                        "'' AS ValueMin, '' AS ValueMax, '' AS TreatCode, '' AS TreatDesc, 1 AS NIL, 0 as ConvertMin, 0 as ConvertMax," & _
                        " CASE WHEN unv.COMPCode is null then 'false' else 'true' END AS DisplayCheck, l.AllowDisposal, a.AllowDec, a.Precision,'' as PrevMaxValue, '' as PrevMinValue " & _
                        " from WAC_COMPCHART b WITH (NOLOCK) LEFT JOIN wac_basecomp  a WITH (NOLOCK) ON a.COMPCode=b.COMPCode " & _
                        "LEFT JOIN UOM c WITH (NOLOCK) ON b.DEFUOM = c.UOMCode LEFT JOIN WAC_BASELINE d WITH(NOLOCK) ON a.BaseID=d.BaseID" & _
                        " LEFT JOIN WAC_WRLIST e WITH(NOLOCK) ON e.WasCode=a.WasCode AND e.WasType=a.WasType AND GeneratorID='0000' AND GeneratorLocID='0000' AND e.Active=1 AND e.Flag=1 AND ReceiverID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, RecID) & "' AND ReceiverLocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, RecLocID) & "'" & _
                        " LEFT JOIN WAC_UNIVCOMP UNV WITH (NOLOCK) ON UNV.COMPCode = a.CompCode LEFT JOIN WAC_BASELINE l WITH (NOLOCK) ON d.BaseID = l.BaseID and d.WasCode=l.WasCode "
                    strSQL &= " Where a.WasCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasteCode) & "' AND a.WasType='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasteType) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWAC_BASECOMPComponentCriteriaList_char(ByVal WasteCode As String, ByVal WasteType As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_basecompInfo.MyInfo
                    strSQL = "Select CASE MAX(CASE a.Mode WHEN '-' THEN 0 WHEN 'NN' THEN 1 WHEN 'D' THEN 2 END) WHEN 0 THEN '-' WHEN 1 THEN 'NN' WHEN 2 THEN 'D' END AS Mode,a.COMPCode, d.ProResidue, UnitNo AS DEFUOM," & _
                        " CASE max(a.Mode) WHEN 'NN' THEN b.COMPDescExt + '*' ELSE b.COMPDescExt End AS COMPDesc, ISNULL(c.UOMDesc,'') AS UOMDesc, a.HandlingType, a.CPFactor, a.BaseID, " & _
                        "'' AS ValueMin, '' AS ValueMax, '' AS TreatCode, '' AS TreatDesc, 1 AS NIL, 0 as ConvertMin, 0 as ConvertMax, CASE WHEN unv.COMPCode is null then 'false' else 'true' END AS DisplayCheck, a.AllowDec, a.Precision,'' as PrevValue " & _
                        " from WAC_COMPCHART b WITH (NOLOCK) LEFT JOIN wac_basecomp  a WITH (NOLOCK) ON a.COMPCode=b.COMPCode " & _
                        "LEFT JOIN UOM c WITH (NOLOCK) ON b.DEFUOM = c.UOMCode LEFT JOIN WAC_BASELINE d WITH(NOLOCK) ON a.BaseID=d.BaseID" & _
                        " LEFT JOIN WAC_UNIVCOMP UNV WITH (NOLOCK) ON UNV.COMPCode = a.CompCode"
                    strSQL &= " Where a.WasCode='" & WasteCode & "' AND a.WasType='" & WasteType & "'"
                    strSQL &= " group by a.COMPCode, d.ProResidue, UnitNo,b.COMPDescExt, b.CompDesc,c.UOMDesc, a.HandlingType, a.CPFactor, a.BaseID, unv.COMPCode, a.AllowDec, a.Precision"
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
        Public Class Wac_basecomp
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
            Public fBCStep As System.String = "BCStep"
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

            Protected _BaseID As System.String
            Protected _WasCode As System.String
            Protected _WasType As System.String
            Protected _HandlingType As System.Int32
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
            Private _BCStep As System.Int32
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
            Private _AllowDec As System.Int32
            Private _Precision As System.Int32

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property AllowDec As System.Int32
                Get
                    Return _AllowDec
                End Get
                Set(ByVal Value As System.Int32)
                    _AllowDec = Value
                End Set
            End Property
            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property Precision As System.Int32
                Get
                    Return _Precision
                End Get
                Set(ByVal Value As System.Int32)
                    _Precision = Value
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
            Public Property BCStep As System.Int32
                Get
                    Return _BCStep
                End Get
                Set(ByVal Value As System.Int32)
                    _BCStep = Value
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
public Class Wac_basecompInfo
		Inherits Core.CoreBase
	Protected Overrides Sub InitializeClassInfo()
		With MyInfo
                .FieldsList = "BaseID,WasCode,WasType,HandlingType,HandlingCode,DictionaryNo,COMPCode,SeqNo,CPPROP,CPFACTOR,CPSGM,CPMIN,CPSGX,CPMAX,CPSMD,CPMED,CPSAV,CPAVG,MethodNo,UnitNo,DeletedAttempt,DeletedReason,BCStep,Dependency,Mode,ATMin,ATMax,ATNX,ATTR,DDGroup,Active,CreateDate,CreateBy,LastUpdate,UpdateBy, ConvertMin, ConvertMax"
			.CheckFields = "CPPROP,CPSGM,CPSGX,CPSMD,CPSAV,Active"
			.TableName = "Wac_basecomp"
            .DefaultCond = Nothing
            .DefaultOrder = Nothing
                .Listing = "BaseID,WasCode,WasType,HandlingType,HandlingCode,DictionaryNo,COMPCode,SeqNo,CPPROP,CPFACTOR,CPSGM,CPMIN,CPSGX,CPMAX,CPSMD,CPMED,CPSAV,CPAVG,MethodNo,UnitNo,DeletedAttempt,DeletedReason,BCStep,Dependency,Mode,ATMin,ATMax,ATNX,ATTR,DDGroup,Active,CreateDate,CreateBy,LastUpdate,UpdateBy, ConvertMin, ConvertMax"
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
Public Class WAC_BASECOMPScheme
Inherits Core.SchemeBase
	Protected Overrides Sub InitializeInfo()
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "BaseID"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(0,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "WasCode"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(1,this)
		
			With this
                .DataType = SQLControl.EnumDataType.dtString
				.FieldName = "WasType"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(2,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "HandlingType"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(3,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "HandlingCode"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(4,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "DictionaryNo"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(5,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "COMPCode"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(6,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "SeqNo"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(7,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CPPROP"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(8,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "CPFACTOR"
				.Length = 10
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(9,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CPSGM"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(10,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CPMIN"
				.Length = 9
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(11,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CPSGX"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(12,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CPMAX"
				.Length = 9
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(13,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CPSMD"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(14,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CPMED"
				.Length = 9
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(15,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CPSAV"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(16,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CPAVG"
				.Length = 9
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(17,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "MethodNo"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(18,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "UnitNo"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(19,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "DeletedAttempt"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(20,this)
			With this
				.DataType = SQLControl.EnumDataType.dtStringN
				.FieldName = "DeletedReason"
				.Length = 50
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(21,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "BCStep"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(22,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Dependency"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(23,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "Mode"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(24,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "ATMin"
				.Length = 30
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(25,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "ATMax"
				.Length = 30
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(26,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "ATNX"
				.Length = 30
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(27,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "ATTR"
				.Length = 30
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(28,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "DDGroup"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(29,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Active"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(30,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "CreateDate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = False
				.AllowNegative = False
			End With
			MyBase.AddItem(31,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "CreateBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(32,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "LastUpdate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = False
				.AllowNegative = False
			End With
			MyBase.AddItem(33,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "UpdateBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
            MyBase.AddItem(34, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ConvertMin"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(35, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ConvertMax"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(36, this)
		
	End Sub
	
		Public ReadOnly Property BaseID As StrucElement
			Get
                Return MyBase.GetItem(0)
            End Get
        End Property
		Public ReadOnly Property WasCode As StrucElement
			Get
                Return MyBase.GetItem(1)
            End Get
        End Property
		Public ReadOnly Property WasType As StrucElement
			Get
                Return MyBase.GetItem(2)
            End Get
        End Property
		Public ReadOnly Property HandlingType As StrucElement
			Get
                Return MyBase.GetItem(3)
            End Get
        End Property
		Public ReadOnly Property HandlingCode As StrucElement
			Get
                Return MyBase.GetItem(4)
            End Get
        End Property
		Public ReadOnly Property DictionaryNo As StrucElement
			Get
                Return MyBase.GetItem(5)
            End Get
        End Property
		Public ReadOnly Property COMPCode As StrucElement
			Get
                Return MyBase.GetItem(6)
            End Get
        End Property
	
		Public ReadOnly Property SeqNo As StrucElement
			Get
                Return MyBase.GetItem(7)
            End Get
        End Property
		Public ReadOnly Property CPPROP As StrucElement
			Get
                Return MyBase.GetItem(8)
            End Get
        End Property
		Public ReadOnly Property CPFACTOR As StrucElement
			Get
                Return MyBase.GetItem(9)
            End Get
        End Property
		Public ReadOnly Property CPSGM As StrucElement
			Get
                Return MyBase.GetItem(10)
            End Get
        End Property
		Public ReadOnly Property CPMIN As StrucElement
			Get
                Return MyBase.GetItem(11)
            End Get
        End Property
		Public ReadOnly Property CPSGX As StrucElement
			Get
                Return MyBase.GetItem(12)
            End Get
        End Property
		Public ReadOnly Property CPMAX As StrucElement
			Get
                Return MyBase.GetItem(13)
            End Get
        End Property
		Public ReadOnly Property CPSMD As StrucElement
			Get
                Return MyBase.GetItem(14)
            End Get
        End Property
		Public ReadOnly Property CPMED As StrucElement
			Get
                Return MyBase.GetItem(15)
            End Get
        End Property
		Public ReadOnly Property CPSAV As StrucElement
			Get
                Return MyBase.GetItem(16)
            End Get
        End Property
		Public ReadOnly Property CPAVG As StrucElement
			Get
                Return MyBase.GetItem(17)
            End Get
        End Property
		Public ReadOnly Property MethodNo As StrucElement
			Get
                Return MyBase.GetItem(18)
            End Get
        End Property
		Public ReadOnly Property UnitNo As StrucElement
			Get
                Return MyBase.GetItem(19)
            End Get
        End Property
		Public ReadOnly Property DeletedAttempt As StrucElement
			Get
                Return MyBase.GetItem(20)
            End Get
        End Property
		Public ReadOnly Property DeletedReason As StrucElement
			Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property BCStep As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
		Public ReadOnly Property Dependency As StrucElement
			Get
                Return MyBase.GetItem(23)
            End Get
        End Property
		Public ReadOnly Property Mode As StrucElement
			Get
                Return MyBase.GetItem(24)
            End Get
        End Property
		Public ReadOnly Property ATMin As StrucElement
			Get
                Return MyBase.GetItem(25)
            End Get
        End Property
		Public ReadOnly Property ATMax As StrucElement
			Get
                Return MyBase.GetItem(26)
            End Get
        End Property
		Public ReadOnly Property ATNX As StrucElement
			Get
                Return MyBase.GetItem(27)
            End Get
        End Property
		Public ReadOnly Property ATTR As StrucElement
			Get
                Return MyBase.GetItem(28)
            End Get
        End Property
		Public ReadOnly Property DDGroup As StrucElement
			Get
                Return MyBase.GetItem(29)
            End Get
        End Property
		Public ReadOnly Property Active As StrucElement
			Get
                Return MyBase.GetItem(30)
            End Get
        End Property
		Public ReadOnly Property CreateDate As StrucElement
			Get
                Return MyBase.GetItem(31)
            End Get
        End Property
		Public ReadOnly Property CreateBy As StrucElement
			Get
                Return MyBase.GetItem(32)
            End Get
        End Property
		Public ReadOnly Property LastUpdate As StrucElement
			Get
                Return MyBase.GetItem(33)
            End Get
        End Property
		Public ReadOnly Property UpdateBy As StrucElement
			Get
                Return MyBase.GetItem(34)
            End Get
        End Property
        Public ReadOnly Property ConvertMin As StrucElement
            Get
                Return MyBase.GetItem(35)
            End Get
        End Property
        Public ReadOnly Property ConvertMax As StrucElement
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

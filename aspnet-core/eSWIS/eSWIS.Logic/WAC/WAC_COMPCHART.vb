Imports SEAL.Data
imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace Actions
Public NotInheritable Class WAC_COMPCHART
	Inherits Core.CoreControl
	Private Wac_compchartInfo As Wac_compchartInfo = New Wac_compchartInfo
	
	Public Sub New(ByVal connecn As String)
        ConnectionString = connecn
        ConnectionSetup()
    End Sub
		
	#Region "Data Manipulation-Add,Edit,Del"
	Private Function Save(ByVal Wac_compchartCont As Container.Wac_compchart, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
        Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
        Dim rdr As System.Data.SqlClient.SqlDataReader
        Save = False
        Try
			If Wac_compchartCont Is Nothing Then
                'Message return
            Else
				blnExec = False
                blnFound = False
                blnFlag = False
				If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
					With Wac_compchartInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.COMPCode) & "' AND COMPDesc = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.COMPDesc) & "' AND DEFUOM = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.DEFUOM) & "'")
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
							.TableName = "Wac_compchart"
							.AddField("COMPGroup", Wac_compchartCont.COMPGroup,SQLControl.EnumDataType.dtNumeric)
							.AddField("SeqNo", Wac_compchartCont.SeqNo,SQLControl.EnumDataType.dtNumeric)
							.AddField("Measure", Wac_compchartCont.Measure,SQLControl.EnumDataType.dtNumeric)
							.AddField("CreateDate", Wac_compchartCont.CreateDate,SQLControl.EnumDataType.dtDateTime)
							.AddField("CreateBy", Wac_compchartCont.CreateBy,SQLControl.EnumDataType.dtString)
							.AddField("LastUpdate", Wac_compchartCont.LastUpdate,SQLControl.EnumDataType.dtDateTime)
							.AddField("UpdateBy", Wac_compchartCont.UpdateBy,SQLControl.EnumDataType.dtString)
							.AddField("Active", Wac_compchartCont.Active,SQLControl.EnumDataType.dtNumeric)
							.AddField("Inuse", Wac_compchartCont.Inuse,SQLControl.EnumDataType.dtNumeric)
							.AddField("rowguid", Wac_compchartCont.rowguid,SQLControl.EnumDataType.dtString)
							.AddField("SyncCreate", Wac_compchartCont.SyncCreate,SQLControl.EnumDataType.dtDateTime)
							.AddField("SyncLastUpd", Wac_compchartCont.SyncLastUpd,SQLControl.EnumDataType.dtDateTime)
							.AddField("IsHost", Wac_compchartCont.IsHost,SQLControl.EnumDataType.dtNumeric)
							.AddField("LastSyncBy", Wac_compchartCont.LastSyncBy,SQLControl.EnumDataType.dtString)
							
							Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.COMPCode) & "' AND COMPDesc = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.COMPDesc) & "' AND DEFUOM = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.DEFUOM) & "'")
									Else
                                        If blnFound = False Then		
											.AddField("COMPCode", Wac_compchartCont.COMPCode, SQLControl.EnumDataType.dtNumeric)
											.AddField("COMPDesc", Wac_compchartCont.COMPDesc, SQLControl.EnumDataType.dtString)
											.AddField("DEFUOM", Wac_compchartCont.DEFUOM, SQLControl.EnumDataType.dtString)
											strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
										End If
									End If
								Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.COMPCode) & "' AND COMPDesc = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.COMPDesc) & "' AND DEFUOM = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.DEFUOM) & "'")
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
			Wac_compchartCont = Nothing
			rdr = Nothing
            EndSQLControl()
            EndConnection()
        End Try
	End Function
	
	'ADD
    Public Function Insert(ByVal Wac_compchartCont As Container.Wac_compchart, ByRef message As String) As Boolean
        Return Save(Wac_compchartCont, SQLControl.EnumSQLType.stInsert, message)
    End Function
	
	'AMEND
    Public Function Update(ByVal Wac_compchartCont As Container.Wac_compchart, ByRef message As String) As Boolean
        Return Save(Wac_compchartCont, SQLControl.EnumSQLType.stUpdate, message)
    End Function
	
	Public Function Delete(ByVal Wac_compchartCont As Container.Wac_compchart, ByRef message As String) As Boolean
        Dim strSQL As String
        Dim blnFound As Boolean
        Dim blnInUse As Boolean
        Dim rdr As System.Data.SqlClient.SqlDataReader
        Delete = False
        blnFound = False
        blnInUse = False
        Try
            If Wac_compchartCont  Is Nothing Then
                'Error Message
            Else
				If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
					With Wac_compchartInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.COMPCode) & "' AND COMPDesc = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.COMPDesc) & "' AND DEFUOM = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.DEFUOM) & "'")
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
                                strSQL = BuildUpdate(Wac_compchartInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = GETDATE() , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.UpdateBy) & "' WHERE" & _
                                "COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.COMPCode) & "' AND COMPDesc = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.COMPDesc) & "' AND DEFUOM = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.DEFUOM) & "'")
						End With
					End if
					
					If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Wac_compchartInfo.MyInfo.TableName, "COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.COMPCode) & "' AND COMPDesc = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.COMPDesc) & "' AND DEFUOM = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_compchartCont.DEFUOM) & "'")
                    End If
				
					Try
                        'execute
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
            Wac_compchartCont = Nothing
            rdr = Nothing
            EndSQLControl()
            EndConnection()
        End Try	
	End Function
#End Region
	
	#Region "Data Selection"
	Public Overloads Function GetWAC_COMPCHART(ByVal COMPCode As System.Int32, ByVal COMPDesc As System.String, ByVal DEFUOM As System.String) As Container.Wac_compchart
		Dim rWac_compchart As Container.Wac_compchart = Nothing
		Dim dtTemp As DataTable = Nothing
		Dim lstField As New List(Of String)
		Dim strSQL As String = Nothing
		
		Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With Wac_compchartInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, COMPCode) & "' AND COMPDesc = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, COMPDesc) & "' AND DEFUOM = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, DEFUOM) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWac_compchart = New Container.Wac_compchart
                                rWac_compchart.COMPCode = drRow.Item("COMPCode")
                                rWac_compchart.COMPDesc = drRow.Item("COMPDesc")
                                rWac_compchart.DEFUOM = drRow.Item("DEFUOM")
                                rWac_compchart.COMPGroup = drRow.Item("COMPGroup")
                                rWac_compchart.SeqNo = drRow.Item("SeqNo")
                                rWac_compchart.Measure = drRow.Item("Measure")
                                rWac_compchart.CreateBy = drRow.Item("CreateBy")
                                rWac_compchart.UpdateBy = drRow.Item("UpdateBy")
                                rWac_compchart.Active = drRow.Item("Active")
                                rWac_compchart.Inuse = drRow.Item("Inuse")
                                rWac_compchart.rowguid = drRow.Item("rowguid")
                                rWac_compchart.SyncCreate = drRow.Item("SyncCreate")
                                rWac_compchart.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWac_compchart.IsHost = drRow.Item("IsHost")
                                rWac_compchart.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rWac_compchart = Nothing
                            End If
                        Else
                            rWac_compchart = Nothing
                        End If
                    End With
                End If
			Return rWac_compchart
		Catch ex As Exception
			throw ex
		Finally
			rWac_compchart = Nothing
			dtTemp = Nothing
			EndSQLControl()
            EndConnection()
		End Try
	End Function
	
	Public Overloads Function GetWAC_COMPCHART(ByVal COMPCode As System.Int32, ByVal COMPDesc As System.String, ByVal DEFUOM As System.String, DecendingOrder as Boolean) As List(Of Container.Wac_compchart)
		Dim rWac_compchart As Container.Wac_compchart = Nothing
		dim lstWac_compchart As List(Of Container.Wac_compchart) = Nothing
		Dim dtTemp As DataTable = Nothing
		Dim lstField As New List(Of String)
		Dim strSQL As String = Nothing
		Dim strDesc As String = ""
		Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With Wac_compchartInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal COMPCode As System.Int32, ByVal COMPDesc As System.String, ByVal DEFUOM As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, COMPCode) & "' AND COMPDesc = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, COMPDesc) & "' AND DEFUOM = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, DEFUOM) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rWac_compchart = New Container.Wac_compchart
                                rWac_compchart.COMPCode = drRow.Item("COMPCode")
                                rWac_compchart.COMPDesc = drRow.Item("COMPDesc")
                                rWac_compchart.DEFUOM = drRow.Item("DEFUOM")
                                rWac_compchart.COMPGroup = drRow.Item("COMPGroup")
                                rWac_compchart.SeqNo = drRow.Item("SeqNo")
                                rWac_compchart.Measure = drRow.Item("Measure")
                                rWac_compchart.CreateBy = drRow.Item("CreateBy")
                                rWac_compchart.UpdateBy = drRow.Item("UpdateBy")
                                rWac_compchart.Active = drRow.Item("Active")
                                rWac_compchart.Inuse = drRow.Item("Inuse")
                                rWac_compchart.rowguid = drRow.Item("rowguid")
                                rWac_compchart.SyncCreate = drRow.Item("SyncCreate")
                                rWac_compchart.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWac_compchart.IsHost = drRow.Item("IsHost")
                                rWac_compchart.LastSyncBy = drRow.Item("LastSyncBy")
                            Next
                            lstWac_compchart.Add(rWac_compchart)
                        Else
                            rWac_compchart = Nothing
                        End If
                        Return lstWac_compchart
                    End With
                End If
		Catch ex As Exception
			Throw ex
		Finally
			rWac_compchart = Nothing
			lstWac_compchart = Nothing
			lstField = Nothing
			EndSQLControl()
            EndConnection()
		End Try
	End Function

    Public Overloads Function GetWAC_COMPCHARTUOM(ByVal CompCode As Integer, ByVal CompDesc As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_compchartInfo.MyInfo
                    'amended by Suprobo, 20170517, Change strSQL
                    strSQL = "SELECT TOP 1 cc.DEFUOM as DEFUOM, um.UOMDesc as UOMDesc, CASE WHEN unv.COMPCode is null then 'false' else 'true' END AS DisplayCheck " & _
                        " FROM WAC_COMPCHART cc LEFT JOIN UOM um ON cc.DEFUOM = um.UOMCode" & _
                        " LEFT JOIN Wac_univcomp unv on cc.COMPCode=unv.COMPCode " & _
                        " WHERE cc.COMPCode=" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, CompCode)

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
Public Class Wac_compchart
	Public fCOMPCode As System.String = "COMPCode"
	Public fCOMPDesc As System.String = "COMPDesc"
	Public fDEFUOM As System.String = "DEFUOM"
	Public fCOMPGroup As System.String = "COMPGroup"
	Public fSeqNo As System.String = "SeqNo"
	Public fMeasure As System.String = "Measure"
	Public fCreateDate As System.String = "CreateDate"
	Public fCreateBy As System.String = "CreateBy"
	Public fLastUpdate As System.String = "LastUpdate"
	Public fUpdateBy As System.String = "UpdateBy"
	Public fActive As System.String = "Active"
	Public fInuse As System.String = "Inuse"
	Public fFlag As System.String = "Flag"
	Public frowguid As System.String = "rowguid"
	Public fSyncCreate As System.String = "SyncCreate"
	Public fSyncLastUpd As System.String = "SyncLastUpd"
	Public fIsHost As System.String = "IsHost"
	Public fLastSyncBy As System.String = "LastSyncBy"

	Protected _COMPCode As System.Int32
	Protected _COMPDesc As System.String
	Protected _DEFUOM As System.String
	Private _COMPGroup As System.Byte
	Private _SeqNo As System.Int32
	Private _Measure As System.Decimal
	Private _CreateDate As System.DateTime
	Private _CreateBy As System.String
	Private _LastUpdate As System.DateTime
	Private _UpdateBy As System.String
	Private _Active As System.Byte
	Private _Inuse As System.Byte
	Private _rowguid As System.Guid
	Private _SyncCreate As System.DateTime
	Private _SyncLastUpd As System.DateTime
	Private _IsHost As System.Byte
	Private _LastSyncBy As System.String

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
	''' Mandatory
    ''' </summary>
	Public Property COMPDesc As System.String
		Get 
			Return _COMPDesc
		End Get
		Set(ByVal Value As System.String)
			_COMPDesc = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory
    ''' </summary>
	Public Property DEFUOM As System.String
		Get 
			Return _DEFUOM
		End Get
		Set(ByVal Value As System.String)
			_DEFUOM = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property COMPGroup As System.Byte
		Get 
			Return _COMPGroup
		End Get
		Set(ByVal Value As System.Byte)
			_COMPGroup = Value
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
	Public Property Measure As System.Decimal
		Get 
			Return _Measure
		End Get
		Set(ByVal Value As System.Decimal)
			_Measure = Value
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
	
End Class
#End Region
End Namespace

#Region "Class Info"
public Class Wac_compchartInfo
		Inherits Core.CoreBase
	Protected Overrides Sub InitializeClassInfo()
		With MyInfo
			.FieldsList = "COMPCode,COMPDesc,DEFUOM,COMPGroup,SeqNo,Measure,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
			.CheckFields = "COMPGroup,Active,Inuse,Flag,IsHost"
			.TableName = "Wac_compchart"
            .DefaultCond = Nothing
            .DefaultOrder = Nothing
            .Listing = "COMPCode,COMPDesc,DEFUOM,COMPGroup,SeqNo,Measure,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
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
Public Class WAC_COMPCHARTScheme
Inherits Core.SchemeBase
	Protected Overrides Sub InitializeInfo()
		
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "COMPCode"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(0,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "COMPDesc"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(1,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "DEFUOM"
				.Length = 10
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(2,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "COMPGroup"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(3,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "SeqNo"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(4,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Measure"
				.Length = 13
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(5,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "CreateDate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = False
				.AllowNegative = False
			End With
			MyBase.AddItem(6,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "CreateBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(7,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "LastUpdate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = False
				.AllowNegative = False
			End With
			MyBase.AddItem(8,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "UpdateBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(9,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Active"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(10,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Inuse"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(11,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Flag"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(12,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "rowguid"
				.Length = 16
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(13,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "SyncCreate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(14,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "SyncLastUpd"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(15,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "IsHost"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(16,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "LastSyncBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(17,this)
		
	End Sub
	
		Public ReadOnly Property COMPCode As StrucElement
			Get
                Return MyBase.GetItem(0)
            End Get
        End Property
		Public ReadOnly Property COMPDesc As StrucElement
			Get
                Return MyBase.GetItem(1)
            End Get
        End Property
		Public ReadOnly Property DEFUOM As StrucElement
			Get
                Return MyBase.GetItem(2)
            End Get
        End Property
	
		Public ReadOnly Property COMPGroup As StrucElement
			Get
                Return MyBase.GetItem(3)
            End Get
        End Property
		Public ReadOnly Property SeqNo As StrucElement
			Get
                Return MyBase.GetItem(4)
            End Get
        End Property
		Public ReadOnly Property Measure As StrucElement
			Get
                Return MyBase.GetItem(5)
            End Get
        End Property
		Public ReadOnly Property CreateDate As StrucElement
			Get
                Return MyBase.GetItem(6)
            End Get
        End Property
		Public ReadOnly Property CreateBy As StrucElement
			Get
                Return MyBase.GetItem(7)
            End Get
        End Property
		Public ReadOnly Property LastUpdate As StrucElement
			Get
                Return MyBase.GetItem(8)
            End Get
        End Property
		Public ReadOnly Property UpdateBy As StrucElement
			Get
                Return MyBase.GetItem(9)
            End Get
        End Property
		Public ReadOnly Property Active As StrucElement
			Get
                Return MyBase.GetItem(10)
            End Get
        End Property
		Public ReadOnly Property Inuse As StrucElement
			Get
                Return MyBase.GetItem(11)
            End Get
        End Property
		Public ReadOnly Property Flag As StrucElement
			Get
                Return MyBase.GetItem(12)
            End Get
        End Property
		Public ReadOnly Property rowguid As StrucElement
			Get
                Return MyBase.GetItem(13)
            End Get
        End Property
		Public ReadOnly Property SyncCreate As StrucElement
			Get
                Return MyBase.GetItem(14)
            End Get
        End Property
		Public ReadOnly Property SyncLastUpd As StrucElement
			Get
                Return MyBase.GetItem(15)
            End Get
        End Property
		Public ReadOnly Property IsHost As StrucElement
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

End Namespace


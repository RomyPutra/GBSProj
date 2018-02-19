Imports SEAL.Data
imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace WAC
#Region "WAC_GENCONVERT Class"
Public NotInheritable Class WAC_GENCONVERT
	Inherits Core.CoreControl
	Private Wac_genconvertInfo As Wac_genconvertInfo = New Wac_genconvertInfo
	
	Public Sub New(ByVal Conn As String)
        ConnectionString = Conn
        ConnectionSetup()
    End Sub
		
	#Region "Data Manipulation-Add,Edit,Del"
	Private Function Save(ByVal Wac_genconvertCont As Container.Wac_genconvert, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
		Dim strSQL As String = ""
        Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
        Dim rdr As System.Data.SqlClient.SqlDataReader
        Save = False
        Try
			If Wac_genconvertCont Is Nothing Then
                    'Message returna
            Else
				blnExec = False
                blnFound = False
                blnFlag = False
				If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
					With Wac_genconvertInfo.MyInfo
                        strSQL = BuildSelect(.CheckFields, .TableName, "ConvertCode = '" & Wac_genconvertCont.ConvertCode & "'")
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
                                .TableName = "Wac_genconvert"
                                .AddField("ConvertGroup", Wac_genconvertCont.ConvertGroup, SQLControl.EnumDataType.dtString)
                                .AddField("ConvertDesc1", Wac_genconvertCont.ConvertDesc1, SQLControl.EnumDataType.dtString)
                                .AddField("ConvertDesc2", Wac_genconvertCont.ConvertDesc2, SQLControl.EnumDataType.dtString)
                                .AddField("Measure", Wac_genconvertCont.Measure, SQLControl.EnumDataType.dtString)
                                .AddField("SeqNo", Wac_genconvertCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Active", Wac_genconvertCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("SyncCreate", Wac_genconvertCont.SyncCreate, SQLControl.EnumDataType.dtStringN)
                                .AddField("SyncLastUpd", Wac_genconvertCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastSyncBy", Wac_genconvertCont.LastSyncBy, SQLControl.EnumDataType.dtString)
							
							Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    If blnFound = True And blnFlag = False Then
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ConvertCode = '" & Wac_genconvertCont.ConvertCode & "'")
									Else
                                        If blnFound = False Then		
											.AddField("ConvertCode", Wac_genconvertCont.ConvertCode, SQLControl.EnumDataType.dtString)
											strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
										End If
									End If
								Case SQLControl.EnumSQLType.stUpdate
									strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ConvertCode = '" & Wac_genconvertCont.ConvertCode & "'")
							End Select
						End With
						Try
                            If BatchExecute Then
                                BatchList.Add(strSQL)
                                If Commit Then
                                    objConn.BatchExecute(BatchList, CommandType.Text, True)
                                End If
                            Else

                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            End If
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
			Wac_genconvertCont = Nothing
			rdr = Nothing
            EndSQLControl()
            EndConnection()
        End Try
	End Function
	
	'ADD
    Public Function Insert(ByVal Wac_genconvertCont As Container.Wac_genconvert, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
        Return Save(Wac_genconvertCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
    End Function
	
	'AMEND
    Public Function Update(ByVal Wac_genconvertCont As Container.Wac_genconvert, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
        Return Save(Wac_genconvertCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
    End Function
	
	Public Function Delete(ByVal Wac_genconvertCont As Container.Wac_genconvert, ByRef message As String) As Boolean
        Dim strSQL As String
        Dim blnFound As Boolean
        Dim blnInUse As Boolean
        Dim rdr As System.Data.SqlClient.SqlDataReader
        Delete = False
        blnFound = False
        blnInUse = False
        Try
            If Wac_genconvertCont  Is Nothing Then
                'Error Message
            Else
				If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
					With Wac_genconvertInfo.MyInfo
                        strSQL = BuildSelect(.CheckFields, .TableName, "ConvertCode = '" & Wac_genconvertCont.ConvertCode & "'")
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
                                strSQL = BuildUpdate(Wac_genconvertInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " WHERE " & _
                                "ConvertCode = '" & Wac_genconvertCont.ConvertCode & "'")
						End With
					End if
					
					If blnFound = True And blnInUse = False Then
                        strSQL = BuildDelete(Wac_genconvertInfo.MyInfo.TableName, "ConvertCode = '" & Wac_genconvertCont.ConvertCode & "'")
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
            Wac_genconvertCont = Nothing
            rdr = Nothing
            EndSQLControl()
            EndConnection()
        End Try	
	End Function
#End Region
	
	#Region "Data Selection"
	Public Overloads Function GetWAC_GENCONVERT(ByVal ConvertCode As System.String) As Container.Wac_genconvert
		Dim rWac_genconvert As Container.Wac_genconvert = Nothing
		Dim dtTemp As DataTable = Nothing
		Dim lstField As New List(Of String)
		Dim strSQL As String = Nothing
		
		Try
			If StartConnection() = True Then
				With Wac_genconvertInfo.MyInfo
					strSQL = BuildSelect(.FieldsList, .TableName, "ConvertCode = '" & ConvertCode & "'")
					dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
					Dim rowCount As Integer = 0
					If dtTemp Is Nothing = False Then
						If dtTemp.Rows.Count > 0 Then
							Dim drRow = dtTemp.Rows(0)
							rWAC_GENCONVERT = New Container.WAC_GENCONVERT
							rWAC_GENCONVERT.ConvertCode = drRow.Item("ConvertCode")
							rWac_genconvert.ConvertGroup = drRow.Item("ConvertGroup")
							rWac_genconvert.ConvertDesc1 = drRow.Item("ConvertDesc1")
							rWac_genconvert.ConvertDesc2 = drRow.Item("ConvertDesc2")
							rWac_genconvert.Measure = drRow.Item("Measure")
							rWac_genconvert.SeqNo = drRow.Item("SeqNo")
							rWac_genconvert.Active = drRow.Item("Active")
							rWac_genconvert.rowguid = drRow.Item("rowguid")
							rWac_genconvert.SyncCreate = drRow.Item("SyncCreate")
							If Not IsDBNull(drRow.Item("SyncLastUpd")) Then
								rWac_genconvert.SyncLastUpd = drRow.Item("SyncLastUpd")
							End IF
							rWac_genconvert.LastSyncBy = drRow.Item("LastSyncBy")
						Else
							rWac_genconvert = Nothing
						End If
					Else
						rWac_genconvert = Nothing
					End If
				End With
			End If
			Return rWac_genconvert
		Catch ex As Exception
			throw ex
		Finally
			rWac_genconvert = Nothing
			dtTemp = Nothing
			EndSQLControl()
            EndConnection()
		End Try
	End Function
	
	Public Overloads Function GetWAC_GENCONVERT(ByVal ConvertCode As System.String, DecendingOrder as Boolean) As List(Of Container.Wac_genconvert)
		Dim rWac_genconvert As Container.Wac_genconvert = Nothing
		dim lstWac_genconvert As List(Of Container.Wac_genconvert) = New List(Of Container.Wac_genconvert)
		Dim dtTemp As DataTable = Nothing
		Dim lstField As New List(Of String)
		Dim strSQL As String = Nothing
		Dim strDesc As String = ""
		Try
			If StartConnection() = True Then
				With Wac_genconvertInfo.MyInfo
					If DecendingOrder Then
						strDesc = " Order by ConvertCode DESC"
					End If
					strSQL = BuildSelect(.FieldsList, .TableName, "ConvertCode = '" & ConvertCode & "'" & strDesc)
					dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
					Dim rowCount As Integer = 0
					If dtTemp Is Nothing = False Then
						For Each drRow As DataRow In dtTemp.Rows
							rWAC_GENCONVERT = New Container.WAC_GENCONVERT
							rWAC_GENCONVERT.ConvertCode = drRow.Item("ConvertCode")
							rWac_genconvert.ConvertGroup = drRow.Item("ConvertGroup")
							rWac_genconvert.ConvertDesc1 = drRow.Item("ConvertDesc1")
							rWac_genconvert.ConvertDesc2 = drRow.Item("ConvertDesc2")
							rWac_genconvert.Measure = drRow.Item("Measure")
							rWac_genconvert.SeqNo = drRow.Item("SeqNo")
							rWac_genconvert.Active = drRow.Item("Active")
							rWac_genconvert.rowguid = drRow.Item("rowguid")
							rWac_genconvert.SyncCreate = drRow.Item("SyncCreate")
							rWac_genconvert.LastSyncBy = drRow.Item("LastSyncBy")
							lstWac_genconvert.Add(rWac_genconvert)
						Next
						
					Else
						rWac_genconvert = Nothing
					End If
					Return lstWac_genconvert
				End With
			End If
		Catch ex As Exception
			Throw ex
		Finally
			rWac_genconvert = Nothing
			lstWac_genconvert = Nothing
			lstField = Nothing
			EndSQLControl()
            EndConnection()
		End Try
	End Function

#End Region
End Class
#End Region

#Region "Container"
Namespace Container
#Region "Wac_genconvert Container"
public Class Wac_genconvert_FieldName
	Public ConvertCode As System.String = "ConvertCode"
	Public ConvertGroup As System.String = "ConvertGroup"
	Public ConvertDesc1 As System.String = "ConvertDesc1"
	Public ConvertDesc2 As System.String = "ConvertDesc2"
	Public Measure As System.String = "Measure"
	Public SeqNo As System.String = "SeqNo"
	Public Active As System.String = "Active"
	Public Flag As System.String = "Flag"
	Public rowguid As System.String = "rowguid"
	Public SyncCreate As System.String = "SyncCreate"
	Public SyncLastUpd As System.String = "SyncLastUpd"
	Public LastSyncBy As System.String = "LastSyncBy"
End Class

Public Class Wac_genconvert
	Protected _ConvertCode As System.String
	Private _ConvertGroup As System.String
	Private _ConvertDesc1 As System.String
	Private _ConvertDesc2 As System.String
            Private _Measure As System.String
	Private _SeqNo As System.Int32
	Private _Active As System.Byte
	Private _rowguid As System.Guid
	Private _SyncCreate As System.String
	Private _SyncLastUpd As System.DateTime
	Private _LastSyncBy As System.String

	''' <summary>
	''' Mandatory
    ''' </summary>
	Public Property ConvertCode As System.String
		Get 
			Return _ConvertCode
		End Get
		Set(ByVal Value As System.String)
			_ConvertCode = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property ConvertGroup As System.String
		Get 
			Return _ConvertGroup
		End Get
		Set(ByVal Value As System.String)
			_ConvertGroup = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property ConvertDesc1 As System.String
		Get 
			Return _ConvertDesc1
		End Get
		Set(ByVal Value As System.String)
			_ConvertDesc1 = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property ConvertDesc2 As System.String
		Get 
			Return _ConvertDesc2
		End Get
		Set(ByVal Value As System.String)
			_ConvertDesc2 = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
            Public Property Measure As System.String
                Get
                    Return _Measure
                End Get
                Set(ByVal Value As System.String)
                    _Measure = Value
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
	Public Property SyncCreate As System.String
		Get 
			Return _SyncCreate
		End Get
		Set(ByVal Value As System.String)
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
#End Region

#Region "Class Info"
#Region "Wac_genconvert Info"
public Class Wac_genconvertInfo
		Inherits Core.CoreBase
	Protected Overrides Sub InitializeClassInfo()
		With MyInfo
			.FieldsList = "ConvertCode,ConvertGroup,ConvertDesc1,ConvertDesc2,Measure,SeqNo,Active,Flag,rowguid,SyncCreate,SyncLastUpd,LastSyncBy"
			.CheckFields = "Active,Flag"
			.TableName = "Wac_genconvert"
            .DefaultCond = Nothing
            .DefaultOrder = Nothing
            .Listing = "ConvertCode,ConvertGroup,ConvertDesc1,ConvertDesc2,Measure,SeqNo,Active,Flag,rowguid,SyncCreate,SyncLastUpd,LastSyncBy"
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
#End Region
	
#Region "Scheme"
#Region "WAC_GENCONVERT Scheme"
Public Class WAC_GENCONVERTScheme
Inherits Core.SchemeBase
	Protected Overrides Sub InitializeInfo()
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "ConvertCode"
				.Length = 10
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(0,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "ConvertGroup"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(1,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "ConvertDesc1"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(2,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "ConvertDesc2"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(3,this)
			With this
                .DataType = SQLControl.EnumDataType.dtString
				.FieldName = "Measure"
				.Length = 13
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(4,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "SeqNo"
				.Length = 4
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(5,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Active"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(6,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Flag"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(7,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "rowguid"
				.Length = 16
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(8,this)
			With this
				.DataType = SQLControl.EnumDataType.dtStringN
				.FieldName = "SyncCreate"
				.Length = 100
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(9,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "SyncLastUpd"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = False
				.AllowNegative = False
			End With
			MyBase.AddItem(10,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "LastSyncBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(11,this)
		
	End Sub
	
		Public ReadOnly Property ConvertCode As StrucElement
			Get
                Return MyBase.GetItem(0)
            End Get
        End Property
	
		Public ReadOnly Property ConvertGroup As StrucElement
			Get
                Return MyBase.GetItem(1)
            End Get
        End Property
		Public ReadOnly Property ConvertDesc1 As StrucElement
			Get
                Return MyBase.GetItem(2)
            End Get
        End Property
		Public ReadOnly Property ConvertDesc2 As StrucElement
			Get
                Return MyBase.GetItem(3)
            End Get
        End Property
		Public ReadOnly Property Measure As StrucElement
			Get
                Return MyBase.GetItem(4)
            End Get
        End Property
		Public ReadOnly Property SeqNo As StrucElement
			Get
                Return MyBase.GetItem(5)
            End Get
        End Property
		Public ReadOnly Property Active As StrucElement
			Get
                Return MyBase.GetItem(6)
            End Get
        End Property
		Public ReadOnly Property Flag As StrucElement
			Get
                Return MyBase.GetItem(7)
            End Get
        End Property
		Public ReadOnly Property rowguid As StrucElement
			Get
                Return MyBase.GetItem(8)
            End Get
        End Property
		Public ReadOnly Property SyncCreate As StrucElement
			Get
                Return MyBase.GetItem(9)
            End Get
        End Property
		Public ReadOnly Property SyncLastUpd As StrucElement
			Get
                Return MyBase.GetItem(10)
            End Get
        End Property
		Public ReadOnly Property LastSyncBy As StrucElement
			Get
                Return MyBase.GetItem(11)
            End Get
        End Property

    Public Function GetElement(ByVal Key As Integer) As StrucElement
        Return MyBase.GetItem(Key)
    End Function
End Class
#End Region
#End Region

End Namespace
Imports SEAL.Data
imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared


Namespace Actions
#Region "WAC_BASEQUEST Class"
Public NotInheritable Class WAC_BASEQUEST
	Inherits Core.CoreControl
	Private Wac_basequestInfo As Wac_basequestInfo = New Wac_basequestInfo
	
	Public Sub New(ByVal Conn As String)
        ConnectionString = Conn
        ConnectionSetup()
    End Sub
		
	#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal ListWac_basequestCont As List(Of Container.Wac_basequest), ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False, Optional ByVal baseid As String = "", Optional ByVal wascode As String = "", Optional ByVal wastype As String = "") As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ListWac_basequestCont Is Nothing OrElse ListWac_basequestCont.Count <= 0 Then
                    'Message return

                    StartSQLControl()
                    With Wac_basequestInfo.MyInfo
                        strSQL = BuildDelete(.TableName, "WasCode = '" & wascode & "' AND WasType = '" & wastype & "'")
                        BatchList.Add(strSQL)
                    End With
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Wac_basequestInfo.MyInfo
                            strSQL = BuildDelete(.TableName, "WasCode = '" & ListWac_basequestCont(0).WasCode & "' AND WasType = '" & ListWac_basequestCont(0).WasType & "'")
                            BatchList.Add(strSQL)
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

                            For Each Wac_basequestCont In ListWac_basequestCont
                                With objSQL
                                    .TableName = "Wac_basequest"
                                    .AddField("QuestDesc", Wac_basequestCont.QuestDesc, SQLControl.EnumDataType.dtStringN)
                                    .AddField("SubText", Wac_basequestCont.SubText, SQLControl.EnumDataType.dtStringN)
                                    .AddField("ControlType", Wac_basequestCont.ControlType, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RefType", Wac_basequestCont.RefType, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("RefValue", Wac_basequestCont.RefValue, SQLControl.EnumDataType.dtStringN)
                                    .AddField("IsCheck", Wac_basequestCont.IsCheck, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CheckType", Wac_basequestCont.CheckType, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CheckValue", Wac_basequestCont.CheckValue, SQLControl.EnumDataType.dtStringN)
                                    .AddField("CreateDate", Wac_basequestCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateBy", Wac_basequestCont.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("LastUpdate", Wac_basequestCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("UpdateBy", Wac_basequestCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("Status", Wac_basequestCont.Status, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Active", Wac_basequestCont.Active, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("SyncCreate", Wac_basequestCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("SyncLastUpd", Wac_basequestCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("LastSyncBy", Wac_basequestCont.LastSyncBy, SQLControl.EnumDataType.dtString)
                                    .AddField("Role", Wac_basequestCont.Role, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("BaseID", Wac_basequestCont.BaseID, SQLControl.EnumDataType.dtString)
                                    .AddField("WasCode", Wac_basequestCont.WasCode, SQLControl.EnumDataType.dtString)
                                    .AddField("WasType", Wac_basequestCont.WasType, SQLControl.EnumDataType.dtString)
                                    .AddField("QuestID", Wac_basequestCont.QuestID, SQLControl.EnumDataType.dtString)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                End With
                                BatchList.Add(strSQL)

                            Next
                        End If
                    End If
                End If
                Try
                    If BatchExecute Then
                        If Commit Then
                            objConn.BatchExecute(BatchList, CommandType.Text, True)
                        End If
                    Else
                        'execute
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
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Return False
            Finally
                ListWac_basequestCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
	
	'ADD
        Public Function Insert(ByVal Wac_basequestCont As List(Of Container.Wac_basequest), ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Wac_basequestCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function
	
	'AMEND
        Public Function Update(ByVal Wac_basequestCont As List(Of Container.Wac_basequest), ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False, Optional ByVal BaseID As String = "", Optional ByVal wascode As String = "", Optional ByVal wastype As String = "") As Boolean
            Return Save(Wac_basequestCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit, BaseID, wascode, wastype)
        End Function
	
	Public Function Delete(ByVal Wac_basequestCont As Container.Wac_basequest, ByRef message As String) As Boolean
        Dim strSQL As String
        Dim blnFound As Boolean
        Dim blnInUse As Boolean
        Dim rdr As System.Data.SqlClient.SqlDataReader
        Delete = False
        blnFound = False
        blnInUse = False
        Try
            If Wac_basequestCont  Is Nothing Then
                'Error Message
            Else
				If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
					With Wac_basequestInfo.MyInfo
                        strSQL = BuildSelect(.CheckFields, .TableName, "BaseID = '" & Wac_basequestCont.BaseID & "' AND WasCode = '" & Wac_basequestCont.WasCode & "' AND WasType = '" & Wac_basequestCont.WasType & "' AND QuestID = '" & Wac_basequestCont.QuestID & "'")
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
							strSQL = BuildUpdate(Wac_basequestInfo.MyInfo.TableName, " SET Flag = 0" & _
							" , LastUpdate = '" & Wac_basequestCont.LastUpdate & "' , UpdateBy = '" & _
							objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_basequestCont.UpdateBy) & "' WHERE" & _
							"BaseID = '" & Wac_basequestCont.BaseID & "' AND WasCode = '" & Wac_basequestCont.WasCode & "' AND WasType = '" & Wac_basequestCont.WasType & "' AND QuestID = '" & Wac_basequestCont.QuestID & "'")
						End With
					End if
					
					If blnFound = True And blnInUse = False Then
                        strSQL = BuildDelete(Wac_basequestInfo.MyInfo.TableName, "BaseID = '" & Wac_basequestCont.BaseID & "' AND WasCode = '" & Wac_basequestCont.WasCode & "' AND WasType = '" & Wac_basequestCont.WasType & "' AND QuestID = '" & Wac_basequestCont.QuestID & "'")
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
            Wac_basequestCont = Nothing
            rdr = Nothing
            EndSQLControl()
            EndConnection()
        End Try	
	End Function
#End Region
	
	#Region "Data Selection"

        Public Overloads Function GetBaseQuest(ByVal WasteCode As String, ByVal WasteType As String, ByVal QuestID As String, ByVal ControlType As Integer, ByVal UserRole As Integer) As Data.DataTable
            If StartConnection() = True Then
                With Wac_basequestInfo.MyInfo
                    strSQL = "SELECT value FROM WAC_BASEQUEST H " & _
                        "CROSS APPLY(SELECT * FROM DBO.fn_Split(H.REFVALUE,'|') ) X " & _
                        "WHERE WasCode='" & WasteCode & "' And ControlType=" & ControlType & " AND WasType='" & WasteType & "' AND QuestID='" & QuestID & "' and Role = " & UserRole & " " & _
                        "ORDER BY Value"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

	#End Region
End Class
#End Region

#Region "Container"
Namespace Container
#Region "Wac_basequest Container"
public Class Wac_basequest_FieldName
	Public BaseID As System.String = "BaseID"
	Public WasCode As System.String = "WasCode"
	Public WasType As System.String = "WasType"
	Public QuestID As System.String = "QuestID"
	Public QuestDesc As System.String = "QuestDesc"
	Public SubText As System.String = "SubText"
	Public ControlType As System.String = "ControlType"
	Public RefType As System.String = "RefType"
	Public RefValue As System.String = "RefValue"
	Public IsCheck As System.String = "IsCheck"
	Public CheckType As System.String = "CheckType"
	Public CheckValue As System.String = "CheckValue"
	Public rowguid As System.String = "rowguid"
	Public CreateDate As System.String = "CreateDate"
	Public CreateBy As System.String = "CreateBy"
	Public LastUpdate As System.String = "LastUpdate"
	Public UpdateBy As System.String = "UpdateBy"
	Public Status As System.String = "Status"
	Public Active As System.String = "Active"
	Public Flag As System.String = "Flag"
	Public SyncCreate As System.String = "SyncCreate"
	Public SyncLastUpd As System.String = "SyncLastUpd"
	Public LastSyncBy As System.String = "LastSyncBy"
End Class

Public Class Wac_basequest
	Protected _BaseID As System.String
	Protected _WasCode As System.String
	Protected _WasType As System.String
	Protected _QuestID As System.String
	Private _QuestDesc As System.String
	Private _SubText As System.String
	Private _ControlType As System.Byte
	Private _RefType As System.Byte
	Private _RefValue As System.String
	Private _IsCheck As System.Byte
	Private _CheckType As System.Byte
	Private _CheckValue As System.String
	Private _rowguid As System.Guid
	Private _CreateDate As System.DateTime
	Private _CreateBy As System.String
	Private _LastUpdate As System.DateTime
	Private _UpdateBy As System.String
	Private _Status As System.Byte
	Private _Active As System.Byte
	Private _SyncCreate As System.DateTime
	Private _SyncLastUpd As System.DateTime
            Private _LastSyncBy As System.String
            Private _Role As System.Byte

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property Role As System.Byte
                Get
                    Return _Role
                End Get
                Set(ByVal Value As System.Byte)
                    _Role = Value
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
	Public Property QuestID As System.String
		Get 
			Return _QuestID
		End Get
		Set(ByVal Value As System.String)
			_QuestID = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property QuestDesc As System.String
		Get 
			Return _QuestDesc
		End Get
		Set(ByVal Value As System.String)
			_QuestDesc = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property SubText As System.String
		Get 
			Return _SubText
		End Get
		Set(ByVal Value As System.String)
			_SubText = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property ControlType As System.Byte
		Get 
			Return _ControlType
		End Get
		Set(ByVal Value As System.Byte)
			_ControlType = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property RefType As System.Byte
		Get 
			Return _RefType
		End Get
		Set(ByVal Value As System.Byte)
			_RefType = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property RefValue As System.String
		Get 
			Return _RefValue
		End Get
		Set(ByVal Value As System.String)
			_RefValue = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property IsCheck As System.Byte
		Get 
			Return _IsCheck
		End Get
		Set(ByVal Value As System.Byte)
			_IsCheck = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property CheckType As System.Byte
		Get 
			Return _CheckType
		End Get
		Set(ByVal Value As System.Byte)
			_CheckType = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property CheckValue As System.String
		Get 
			Return _CheckValue
		End Get
		Set(ByVal Value As System.String)
			_CheckValue = Value
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
#Region "Wac_basequest Info"
public Class Wac_basequestInfo
		Inherits Core.CoreBase
	Protected Overrides Sub InitializeClassInfo()
		With MyInfo
			.FieldsList = "BaseID,WasCode,WasType,QuestID,QuestDesc,SubText,ControlType,RefType,RefValue,IsCheck,CheckType,CheckValue,rowguid,CreateDate,CreateBy,LastUpdate,UpdateBy,Status,Active,Flag,SyncCreate,SyncLastUpd,LastSyncBy"
			.CheckFields = "ControlType,RefType,IsCheck,CheckType,Status,Active,Flag"
			.TableName = "Wac_basequest"
            .DefaultCond = Nothing
            .DefaultOrder = Nothing
            .Listing = "BaseID,WasCode,WasType,QuestID,QuestDesc,SubText,ControlType,RefType,RefValue,IsCheck,CheckType,CheckValue,rowguid,CreateDate,CreateBy,LastUpdate,UpdateBy,Status,Active,Flag,SyncCreate,SyncLastUpd,LastSyncBy"
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
#Region "WAC_BASEQUEST Scheme"
Public Class WAC_BASEQUESTScheme
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
				.Length = 3
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(2,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "QuestID"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(3,this)
			With this
				.DataType = SQLControl.EnumDataType.dtStringN
				.FieldName = "QuestDesc"
				.Length = 4000
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(4,this)
			With this
				.DataType = SQLControl.EnumDataType.dtStringN
				.FieldName = "SubText"
				.Length = 4000
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(5,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "ControlType"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(6,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "RefType"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(7,this)
			With this
				.DataType = SQLControl.EnumDataType.dtStringN
				.FieldName = "RefValue"
				.Length = 4000
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(8,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "IsCheck"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(9,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "CheckType"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(10,this)
			With this
				.DataType = SQLControl.EnumDataType.dtStringN
				.FieldName = "CheckValue"
				.Length = 4000
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(11,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "rowguid"
				.Length = 16
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(12,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "CreateDate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = False
				.AllowNegative = False
			End With
			MyBase.AddItem(13,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "CreateBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(14,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "LastUpdate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = False
				.AllowNegative = False
			End With
			MyBase.AddItem(15,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "UpdateBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(16,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Status"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(17,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Active"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(18,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Flag"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(19,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "SyncCreate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(20,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "SyncLastUpd"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(21,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "LastSyncBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(22,this)
		
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
		Public ReadOnly Property QuestID As StrucElement
			Get
                Return MyBase.GetItem(3)
            End Get
        End Property
	
		Public ReadOnly Property QuestDesc As StrucElement
			Get
                Return MyBase.GetItem(4)
            End Get
        End Property
		Public ReadOnly Property SubText As StrucElement
			Get
                Return MyBase.GetItem(5)
            End Get
        End Property
		Public ReadOnly Property ControlType As StrucElement
			Get
                Return MyBase.GetItem(6)
            End Get
        End Property
		Public ReadOnly Property RefType As StrucElement
			Get
                Return MyBase.GetItem(7)
            End Get
        End Property
		Public ReadOnly Property RefValue As StrucElement
			Get
                Return MyBase.GetItem(8)
            End Get
        End Property
		Public ReadOnly Property IsCheck As StrucElement
			Get
                Return MyBase.GetItem(9)
            End Get
        End Property
		Public ReadOnly Property CheckType As StrucElement
			Get
                Return MyBase.GetItem(10)
            End Get
        End Property
		Public ReadOnly Property CheckValue As StrucElement
			Get
                Return MyBase.GetItem(11)
            End Get
        End Property
		Public ReadOnly Property rowguid As StrucElement
			Get
                Return MyBase.GetItem(12)
            End Get
        End Property
		Public ReadOnly Property CreateDate As StrucElement
			Get
                Return MyBase.GetItem(13)
            End Get
        End Property
		Public ReadOnly Property CreateBy As StrucElement
			Get
                Return MyBase.GetItem(14)
            End Get
        End Property
		Public ReadOnly Property LastUpdate As StrucElement
			Get
                Return MyBase.GetItem(15)
            End Get
        End Property
		Public ReadOnly Property UpdateBy As StrucElement
			Get
                Return MyBase.GetItem(16)
            End Get
        End Property
		Public ReadOnly Property Status As StrucElement
			Get
                Return MyBase.GetItem(17)
            End Get
        End Property
		Public ReadOnly Property Active As StrucElement
			Get
                Return MyBase.GetItem(18)
            End Get
        End Property
		Public ReadOnly Property Flag As StrucElement
			Get
                Return MyBase.GetItem(19)
            End Get
        End Property
		Public ReadOnly Property SyncCreate As StrucElement
			Get
                Return MyBase.GetItem(20)
            End Get
        End Property
		Public ReadOnly Property SyncLastUpd As StrucElement
			Get
                Return MyBase.GetItem(21)
            End Get
        End Property
		Public ReadOnly Property LastSyncBy As StrucElement
			Get
                Return MyBase.GetItem(22)
            End Get
        End Property

    Public Function GetElement(ByVal Key As Integer) As StrucElement
        Return MyBase.GetItem(Key)
    End Function
End Class
#End Region
#End Region

End Namespace
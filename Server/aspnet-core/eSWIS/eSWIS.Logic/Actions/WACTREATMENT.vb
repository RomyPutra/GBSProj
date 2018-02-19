Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace Actions
Public NotInheritable Class WACTREATMENT
	Inherits Core.CoreControl
	Private WactreatmentInfo As WactreatmentInfo = New WactreatmentInfo
	
	Public Sub New(ByVal connecn As String)
        ConnectionString = connecn
        ConnectionSetup()
    End Sub
		
	#Region "Data Manipulation-Add,Edit,Del"
	Private Function Save(ByVal WactreatmentCont As Container.Wactreatment, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
		Dim strSQL As String = ""
        Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
        Dim rdr As System.Data.SqlClient.SqlDataReader
        Save = False
        Try
			If WactreatmentCont Is Nothing Then
                'Message return
            Else
				blnExec = False
                blnFound = False
                blnFlag = False
				If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
					With WactreatmentInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WactreatmentCont.ItemCode) & "' AND WasteType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WactreatmentCont.WasteType) & "' AND TreatmentID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WactreatmentCont.TreatmentID) & "'")
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
                                .TableName = "Wactreatment WITH (ROWLOCK)"
                                .AddField("TreatmentDesc", WactreatmentCont.TreatmentDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("Active", WactreatmentCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", WactreatmentCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", WactreatmentCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", WactreatmentCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", WactreatmentCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("rowguid", WactreatmentCont.rowguid, SQLControl.EnumDataType.dtString)
                                .AddField("IsHost", WactreatmentCont.IsHost, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WactreatmentCont.ItemCode) & "' AND WasteType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WactreatmentCont.WasteType) & "' AND TreatmentID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WactreatmentCont.TreatmentID) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("ItemCode", WactreatmentCont.ItemCode, SQLControl.EnumDataType.dtString)
                                                .AddField("WasteType", WactreatmentCont.WasteType, SQLControl.EnumDataType.dtString)
                                                .AddField("TreatmentID", WactreatmentCont.TreatmentID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WactreatmentCont.ItemCode) & "' AND WasteType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WactreatmentCont.WasteType) & "' AND TreatmentID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WactreatmentCont.TreatmentID) & "'")
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
                'Throw axAssign
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Return False
            Finally
                WactreatmentCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal WactreatmentCont As Container.Wactreatment, ByRef message As String) As Boolean
            Return Save(WactreatmentCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal WactreatmentCont As Container.Wactreatment, ByRef message As String) As Boolean
            Return Save(WactreatmentCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal WactreatmentCont As Container.Wactreatment, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If WactreatmentCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With WactreatmentInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WactreatmentCont.ItemCode) & "' AND WasteType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WactreatmentCont.WasteType) & "' AND TreatmentID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WactreatmentCont.TreatmentID) & "'")
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
                                strSQL = BuildUpdate("Wactreatment WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtDateTime, WactreatmentCont.LastUpdate) & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, WactreatmentCont.UpdateBy) & "' WHERE" & _
                                "ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WactreatmentCont.ItemCode) & "' AND WasteType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WactreatmentCont.WasteType) & "' AND TreatmentID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WactreatmentCont.TreatmentID) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("Wactreatment WITH (ROWLOCK)", "ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WactreatmentCont.ItemCode) & "' AND WasteType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WactreatmentCont.WasteType) & "' AND TreatmentID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WactreatmentCont.TreatmentID) & "'")
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
                WactreatmentCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region
	
	#Region "Data Selection"
	Public Overloads Function GetWACTREATMENT(ByVal ItemCode As System.String, ByVal WasteType As System.String, ByVal TreatmentID As System.String) As Container.Wactreatment
		Dim rWactreatment As Container.Wactreatment = Nothing
		Dim dtTemp As DataTable = Nothing
		Dim lstField As New List(Of String)
		Dim strSQL As String = Nothing
		
		Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With WactreatmentInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "' AND WasteType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "' AND TreatmentID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TreatmentID) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWactreatment = New Container.Wactreatment
                                rWactreatment.ItemCode = drRow.Item("ItemCode")
                                rWactreatment.WasteType = drRow.Item("WasteType")
                                rWactreatment.TreatmentID = drRow.Item("TreatmentID")
                                rWactreatment.TreatmentDesc = drRow.Item("TreatmentDesc")
                                rWactreatment.Active = drRow.Item("Active")
                                rWactreatment.CreateBy = drRow.Item("CreateBy")
                                rWactreatment.UpdateBy = drRow.Item("UpdateBy")
                                rWactreatment.rowguid = drRow.Item("rowguid")
                                rWactreatment.IsHost = drRow.Item("IsHost")
                            Else
                                rWactreatment = Nothing
                            End If
                        Else
                            rWactreatment = Nothing
                        End If
                    End With
                End If
                Return rWactreatment
            Catch ex As Exception
                Throw ex
            Finally
                rWactreatment = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWACTREATMENT(ByVal ItemCode As System.String, ByVal WasteType As System.String, ByVal TreatmentID As System.String, DecendingOrder As Boolean) As List(Of Container.Wactreatment)
            Dim rWactreatment As Container.Wactreatment = Nothing
            Dim lstWactreatment As List(Of Container.Wactreatment) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With WactreatmentInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal ItemCode As System.String, ByVal WasteType As System.String, ByVal TreatmentID As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "' AND WasteType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "' AND TreatmentID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TreatmentID) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rWactreatment = New Container.Wactreatment
                                rWactreatment.ItemCode = drRow.Item("ItemCode")
                                rWactreatment.WasteType = drRow.Item("WasteType")
                                rWactreatment.TreatmentID = drRow.Item("TreatmentID")
                                rWactreatment.TreatmentDesc = drRow.Item("TreatmentDesc")
                                rWactreatment.Active = drRow.Item("Active")
                                rWactreatment.CreateBy = drRow.Item("CreateBy")
                                rWactreatment.UpdateBy = drRow.Item("UpdateBy")
                                rWactreatment.rowguid = drRow.Item("rowguid")
                                rWactreatment.IsHost = drRow.Item("IsHost")
                            Next
                            lstWactreatment.Add(rWactreatment)
                        Else
                            rWactreatment = Nothing
                        End If
                        Return lstWactreatment
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rWactreatment = Nothing
                lstWactreatment = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
		
	Public Overloads Function GetWACTREATMENTList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
        If StartConnection() = True Then
            With WactreatmentInfo.MyInfo
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
		
	Public Overloads Function GetWACTREATMENTShortList(ByVal ShortListing As Boolean) As Data.DataTable
        If StartConnection() = True Then
            With WactreatmentInfo.MyInfo
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

        Public Overloads Function GetDistinctWACTreatmentItemCode(Optional ByVal FieldCond As String = Nothing)
            If StartConnection() = True Then
                With WactreatmentInfo.MyInfo
                    If Not FieldCond Is Nothing Then
                        strSQL = "SELECT DISTINCT ItemCode FROM WACTREATMENT WHERE " & FieldCond
                    Else
                        strSQL = "SELECT DISTINCT ItemCode FROM WACTREATMENT"
                    End If

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetDistinctWACTreatment(Optional ByVal FieldCond As String = Nothing)
            If StartConnection() = True Then
                With WactreatmentInfo.MyInfo
                    If Not FieldCond Is Nothing Then
                        strSQL = "SELECT DISTINCT TreatmentID, TreatmentDesc FROM WACTREATMENT WHERE " & FieldCond
                    Else
                        strSQL = "SELECT DISTINCT TreatmentID, TreatmentDesc FROM WACTREATMENT"
                    End If

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
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
Public Class Wactreatment
	Public fItemCode As System.String = "ItemCode"
	Public fWasteType As System.String = "WasteType"
	Public fTreatmentID As System.String = "TreatmentID"
	Public fTreatmentDesc As System.String = "TreatmentDesc"
	Public fActive As System.String = "Active"
	Public fCreateDate As System.String = "CreateDate"
	Public fCreateBy As System.String = "CreateBy"
	Public fLastUpdate As System.String = "LastUpdate"
	Public fUpdateBy As System.String = "UpdateBy"
	Public frowguid As System.String = "rowguid"
	Public fIsHost As System.String = "IsHost"

	Protected _ItemCode As System.String
	Protected _WasteType As System.String
	Protected _TreatmentID As System.String
	Private _TreatmentDesc As System.String
	Private _Active As System.Byte
	Private _CreateDate As System.DateTime
	Private _CreateBy As System.String
	Private _LastUpdate As System.DateTime
	Private _UpdateBy As System.String
	Private _rowguid As System.Guid
	Private _IsHost As System.Byte

	''' <summary>
	''' Mandatory
    ''' </summary>
	Public Property ItemCode As System.String
		Get 
			Return _ItemCode
		End Get
		Set(ByVal Value As System.String)
			_ItemCode = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory
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
	''' Mandatory
    ''' </summary>
	Public Property TreatmentID As System.String
		Get 
			Return _TreatmentID
		End Get
		Set(ByVal Value As System.String)
			_TreatmentID = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property TreatmentDesc As System.String
		Get 
			Return _TreatmentDesc
		End Get
		Set(ByVal Value As System.String)
			_TreatmentDesc = Value
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
	Public Property IsHost As System.Byte
		Get 
			Return _IsHost
		End Get
		Set(ByVal Value As System.Byte)
			_IsHost = Value
		End Set
	End Property
	
End Class
#End Region
End Namespace

#Region "Class Info"
public Class WactreatmentInfo
		Inherits Core.CoreBase
	Protected Overrides Sub InitializeClassInfo()
		With MyInfo
                .FieldsList = "ItemCode,WasteType,TreatmentID,TreatmentDesc,TreatmentType,Active,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid,IsHost"
			.CheckFields = "Active,IsHost"
                .TableName = "Wactreatment WITH (NOLOCK)"
            .DefaultCond = Nothing
            .DefaultOrder = Nothing
                .Listing = "ItemCode,WasteType,TreatmentID,TreatmentDesc,TreatmentType,Active,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid,IsHost"
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
Public Class WACTREATMENTScheme
Inherits Core.SchemeBase
	Protected Overrides Sub InitializeInfo()
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "ItemCode"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(0,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "WasteType"
				.Length = 3
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(1,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "TreatmentID"
				.Length = 3
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(2,this)
			With this
				.DataType = SQLControl.EnumDataType.dtStringN
				.FieldName = "TreatmentDesc"
				.Length = 100
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(3,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Active"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(4,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "CreateDate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = False
				.AllowNegative = False
			End With
			MyBase.AddItem(5,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "CreateBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(6,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "LastUpdate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = False
				.AllowNegative = False
			End With
			MyBase.AddItem(7,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "UpdateBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(8,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "rowguid"
				.Length = 16
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(9,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "IsHost"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(10,this)
		
	End Sub
	
		Public ReadOnly Property ItemCode As StrucElement
			Get
                Return MyBase.GetItem(0)
            End Get
        End Property
		Public ReadOnly Property WasteType As StrucElement
			Get
                Return MyBase.GetItem(1)
            End Get
        End Property
		Public ReadOnly Property TreatmentID As StrucElement
			Get
                Return MyBase.GetItem(2)
            End Get
        End Property
	
		Public ReadOnly Property TreatmentDesc As StrucElement
			Get
                Return MyBase.GetItem(3)
            End Get
        End Property
		Public ReadOnly Property Active As StrucElement
			Get
                Return MyBase.GetItem(4)
            End Get
        End Property
		Public ReadOnly Property CreateDate As StrucElement
			Get
                Return MyBase.GetItem(5)
            End Get
        End Property
		Public ReadOnly Property CreateBy As StrucElement
			Get
                Return MyBase.GetItem(6)
            End Get
        End Property
		Public ReadOnly Property LastUpdate As StrucElement
			Get
                Return MyBase.GetItem(7)
            End Get
        End Property
		Public ReadOnly Property UpdateBy As StrucElement
			Get
                Return MyBase.GetItem(8)
            End Get
        End Property
		Public ReadOnly Property rowguid As StrucElement
			Get
                Return MyBase.GetItem(9)
            End Get
        End Property
		Public ReadOnly Property IsHost As StrucElement
			Get
                Return MyBase.GetItem(10)
            End Get
        End Property

    Public Function GetElement(ByVal Key As Integer) As StrucElement
        Return MyBase.GetItem(Key)
    End Function
End Class
#End Region

End Namespace
Imports SEAL.Data
imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace Actions
Public NotInheritable Class WACDtl
	Inherits Core.CoreControl
	Private WacdtlInfo As WacdtlInfo = New WacdtlInfo
	
	Public Sub New(ByVal connecn As String)
        ConnectionString = connecn
        ConnectionSetup()
    End Sub
		
#Region "Data Manipulation-Add,Edit,Del"

        Private Function Save(ByVal WacdtlCont As Container.Wacdtl, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If WacdtlCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With WacdtlInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "WACNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WacdtlCont.WACNo) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WacdtlCont.ComponentCode) & "'")
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
                                .TableName = "Wacdtl WITH (ROWLOCK)"
                                .AddField("ComponentDesc", WacdtlCont.ComponentDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("ValueMin", WacdtlCont.ValueMin, SQLControl.EnumDataType.dtNumeric)
                                .AddField("ValueMax", WacdtlCont.ValueMax, SQLControl.EnumDataType.dtNumeric)
                                .AddField("AvgMin", WacdtlCont.AvgMin, SQLControl.EnumDataType.dtNumeric)
                                .AddField("AvgMax", WacdtlCont.AvgMax, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Unit", WacdtlCont.Unit, SQLControl.EnumDataType.dtString)
                                .AddField("CreateDate", WacdtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", WacdtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", WacdtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", WacdtlCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", WacdtlCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", WacdtlCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsHost", WacdtlCont.IsHost, SQLControl.EnumDataType.dtNumeric)

                                .AddField("WACNo", WacdtlCont.WACNo, SQLControl.EnumDataType.dtString)
                                .AddField("ComponentCode", WacdtlCont.ComponentCode, SQLControl.EnumDataType.dtString)
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
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
                WacdtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal WacdtlCont As Container.Wacdtl, ByRef message As String) As Boolean
            Return Save(WacdtlCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal WacdtlCont As Container.Wacdtl, ByRef message As String) As Boolean
            Return Save(WacdtlCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal WacdtlCont As Container.Wacdtl, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If WacdtlCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With WacdtlInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "WACNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WacdtlCont.WACNo) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WacdtlCont.ComponentCode) & "'")
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
                                strSQL = BuildUpdate("Wacdtl WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, WacdtlCont.UpdateBy) & "' WHERE" & _
                                "WACNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WacdtlCont.WACNo) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WacdtlCont.ComponentCode) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("Wacdtl WITH (ROWLOCK)", "WACNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WacdtlCont.WACNo) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WacdtlCont.ComponentCode) & "'")
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
                WacdtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region
	
#Region "Data Selection"

        Public Overloads Function GetWACDtl(ByVal WACNo As System.String, ByVal ComponentCode As System.String) As Container.Wacdtl
            Dim rWacdtl As Container.Wacdtl = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With WacdtlInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "WACNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WACNo) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ComponentCode) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWacdtl = New Container.Wacdtl
                                rWacdtl.WACNo = drRow.Item("WACNo")
                                rWacdtl.ComponentCode = drRow.Item("ComponentCode")
                                rWacdtl.ComponentDesc = drRow.Item("ComponentDesc")
                                rWacdtl.ValueMin = drRow.Item("ValueMin")
                                rWacdtl.ValueMax = drRow.Item("ValueMax")
                                rWacdtl.AvgMin = drRow.Item("AvgMin")
                                rWacdtl.AvgMax = drRow.Item("AvgMax")
                                rWacdtl.Unit = drRow.Item("Unit")
                                rWacdtl.CreateBy = drRow.Item("CreateBy")
                                rWacdtl.UpdateBy = drRow.Item("UpdateBy")
                                rWacdtl.Active = drRow.Item("Active")
                                rWacdtl.Inuse = drRow.Item("Inuse")
                                rWacdtl.rowguid = drRow.Item("rowguid")
                                rWacdtl.SyncCreate = drRow.Item("SyncCreate")
                                rWacdtl.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWacdtl.IsHost = drRow.Item("IsHost")
                                rWacdtl.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rWacdtl = Nothing
                            End If
                        Else
                            rWacdtl = Nothing
                        End If
                    End With
                End If
                Return rWacdtl
            Catch ex As Exception
                Throw ex
            Finally
                rWacdtl = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWACDtl(ByVal WACNo As System.String, ByVal ComponentCode As System.String, DecendingOrder As Boolean) As List(Of Container.Wacdtl)
            Dim rWacdtl As Container.Wacdtl = Nothing
            Dim lstWacdtl As List(Of Container.Wacdtl) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With WacdtlInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal WACNo As System.String, ByVal ComponentCode As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "WACNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WACNo) & "' AND ComponentCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ComponentCode) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rWacdtl = New Container.Wacdtl
                                rWacdtl.WACNo = drRow.Item("WACNo")
                                rWacdtl.ComponentCode = drRow.Item("ComponentCode")
                                rWacdtl.ComponentDesc = drRow.Item("ComponentDesc")
                                rWacdtl.ValueMin = drRow.Item("ValueMin")
                                rWacdtl.ValueMax = drRow.Item("ValueMax")
                                rWacdtl.AvgMin = drRow.Item("AvgMin")
                                rWacdtl.AvgMax = drRow.Item("AvgMax")
                                rWacdtl.Unit = drRow.Item("Unit")
                                rWacdtl.CreateBy = drRow.Item("CreateBy")
                                rWacdtl.UpdateBy = drRow.Item("UpdateBy")
                                rWacdtl.Active = drRow.Item("Active")
                                rWacdtl.Inuse = drRow.Item("Inuse")
                                rWacdtl.rowguid = drRow.Item("rowguid")
                                rWacdtl.SyncCreate = drRow.Item("SyncCreate")
                                rWacdtl.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWacdtl.IsHost = drRow.Item("IsHost")
                                rWacdtl.LastSyncBy = drRow.Item("LastSyncBy")
                            Next
                            lstWacdtl.Add(rWacdtl)
                        Else
                            rWacdtl = Nothing
                        End If
                        Return lstWacdtl
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rWacdtl = Nothing
                lstWacdtl = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWACDtlList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With WacdtlInfo.MyInfo
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

        Public Overloads Function GetWACDtlShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With WacdtlInfo.MyInfo
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

        Public Overloads Function GetWACDtlListByWACNo(Optional ByVal WACNo As String = Nothing, Optional ByVal SQL As Boolean = False) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With WacdtlInfo.MyInfo
                    If Not SQL Then
                        strSQL = BuildSelect(.FieldsList, .TableName, "WACNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WACNo) & "'")
                    Else
                        strSQL = "SELECT (wh.ItemCode + ' ' + cm.CodeDesc) As ItemCodeType, wd.*" & _
                            " FROM Wacdtl wd" & _
                            " LEFT OUTER JOIN Wachdr wh ON wd.WACNo = wh.WACNo" & _
                            " LEFT OUTER JOIN CODEMASTER cm ON wh.ItemType = cm.Code" & _
                            " WHERE wd.WACNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WACNo) & "'"
                    End If
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWACDtlListByCriteria(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With WacdtlInfo.MyInfo
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


#End Region

    End Class


Namespace Container
#Region "Class Container"
Public Class Wacdtl
	Public fWACNo As System.String = "WACNo"
	Public fComponentCode As System.String = "ComponentCode"
	Public fComponentDesc As System.String = "ComponentDesc"
	Public fValueMin As System.String = "ValueMin"
	Public fValueMax As System.String = "ValueMax"
	Public fAvgMin As System.String = "AvgMin"
	Public fAvgMax As System.String = "AvgMax"
	Public fUnit As System.String = "Unit"
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

	Protected _WACNo As System.String
	Protected _ComponentCode As System.String
	Private _ComponentDesc As System.String
	Private _ValueMin As System.Decimal
	Private _ValueMax As System.Decimal
	Private _AvgMin As System.Decimal
	Private _AvgMax As System.Decimal
	Private _Unit As System.String
	Private _CreateDate As System.DateTime
	Private _CreateBy As System.String
	Private _LastUpdate As System.DateTime
	Private _UpdateBy As System.String
	Private _Active As System.Byte
            Private _Inuse As System.Byte
            Private _Flag As System.Byte
	Private _rowguid As System.Guid
	Private _SyncCreate As System.DateTime
	Private _SyncLastUpd As System.DateTime
	Private _IsHost As System.Byte
	Private _LastSyncBy As System.String

	''' <summary>
	''' Mandatory
    ''' </summary>
	Public Property WACNo As System.String
		Get 
			Return _WACNo
		End Get
		Set(ByVal Value As System.String)
			_WACNo = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory
    ''' </summary>
	Public Property ComponentCode As System.String
		Get 
			Return _ComponentCode
		End Get
		Set(ByVal Value As System.String)
			_ComponentCode = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property ComponentDesc As System.String
		Get 
			Return _ComponentDesc
		End Get
		Set(ByVal Value As System.String)
			_ComponentDesc = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property ValueMin As System.Decimal
		Get 
			Return _ValueMin
		End Get
		Set(ByVal Value As System.Decimal)
			_ValueMin = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property ValueMax As System.Decimal
		Get 
			Return _ValueMax
		End Get
		Set(ByVal Value As System.Decimal)
			_ValueMax = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property AvgMin As System.Decimal
		Get 
			Return _AvgMin
		End Get
		Set(ByVal Value As System.Decimal)
			_AvgMin = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property AvgMax As System.Decimal
		Get 
			Return _AvgMax
		End Get
		Set(ByVal Value As System.Decimal)
			_AvgMax = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property Unit As System.String
		Get 
			Return _Unit
		End Get
		Set(ByVal Value As System.String)
			_Unit = Value
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
            Public Property Flag As System.Byte
                Get
                    Return _Flag
                End Get
                Set(ByVal Value As System.Byte)
                    _Flag = Value
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
public Class WacdtlInfo
		Inherits Core.CoreBase
	Protected Overrides Sub InitializeClassInfo()
		With MyInfo
			.FieldsList = "WACNo,ComponentCode,ComponentDesc,ValueMin,ValueMax,AvgMin,AvgMax,Unit,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
			.CheckFields = "Active,Inuse,Flag,IsHost"
                .TableName = "Wacdtl WITH (NOLOCK)"
            .DefaultCond = Nothing
            .DefaultOrder = Nothing
            .Listing = "WACNo,ComponentCode,ComponentDesc,ValueMin,ValueMax,AvgMin,AvgMax,Unit,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
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
Public Class WACDtlScheme
Inherits Core.SchemeBase
	Protected Overrides Sub InitializeInfo()
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "WACNo"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(0,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "ComponentCode"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(1,this)
			With this
				.DataType = SQLControl.EnumDataType.dtStringN
				.FieldName = "ComponentDesc"
				.Length = 50
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(2,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "ValueMin"
				.Length = 9
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(3,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "ValueMax"
				.Length = 9
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(4,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "AvgMin"
				.Length = 9
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(5,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "AvgMax"
				.Length = 9
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(6,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "Unit"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(7,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "CreateDate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = False
				.AllowNegative = False
			End With
			MyBase.AddItem(8,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "CreateBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(9,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "LastUpdate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = False
				.AllowNegative = False
			End With
			MyBase.AddItem(10,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "UpdateBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(11,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Active"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(12,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Inuse"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(13,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Flag"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(14,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "rowguid"
				.Length = 16
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(15,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "SyncCreate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(16,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "SyncLastUpd"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(17,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "IsHost"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(18,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "LastSyncBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(19,this)
		
	End Sub
	
		Public ReadOnly Property WACNo As StrucElement
			Get
                Return MyBase.GetItem(0)
            End Get
        End Property
		Public ReadOnly Property ComponentCode As StrucElement
			Get
                Return MyBase.GetItem(1)
            End Get
        End Property
	
		Public ReadOnly Property ComponentDesc As StrucElement
			Get
                Return MyBase.GetItem(2)
            End Get
        End Property
		Public ReadOnly Property ValueMin As StrucElement
			Get
                Return MyBase.GetItem(3)
            End Get
        End Property
		Public ReadOnly Property ValueMax As StrucElement
			Get
                Return MyBase.GetItem(4)
            End Get
        End Property
		Public ReadOnly Property AvgMin As StrucElement
			Get
                Return MyBase.GetItem(5)
            End Get
        End Property
		Public ReadOnly Property AvgMax As StrucElement
			Get
                Return MyBase.GetItem(6)
            End Get
        End Property
		Public ReadOnly Property Unit As StrucElement
			Get
                Return MyBase.GetItem(7)
            End Get
        End Property
		Public ReadOnly Property CreateDate As StrucElement
			Get
                Return MyBase.GetItem(8)
            End Get
        End Property
		Public ReadOnly Property CreateBy As StrucElement
			Get
                Return MyBase.GetItem(9)
            End Get
        End Property
		Public ReadOnly Property LastUpdate As StrucElement
			Get
                Return MyBase.GetItem(10)
            End Get
        End Property
		Public ReadOnly Property UpdateBy As StrucElement
			Get
                Return MyBase.GetItem(11)
            End Get
        End Property
		Public ReadOnly Property Active As StrucElement
			Get
                Return MyBase.GetItem(12)
            End Get
        End Property
		Public ReadOnly Property Inuse As StrucElement
			Get
                Return MyBase.GetItem(13)
            End Get
        End Property
		Public ReadOnly Property Flag As StrucElement
			Get
                Return MyBase.GetItem(14)
            End Get
        End Property
		Public ReadOnly Property rowguid As StrucElement
			Get
                Return MyBase.GetItem(15)
            End Get
        End Property
		Public ReadOnly Property SyncCreate As StrucElement
			Get
                Return MyBase.GetItem(16)
            End Get
        End Property
		Public ReadOnly Property SyncLastUpd As StrucElement
			Get
                Return MyBase.GetItem(17)
            End Get
        End Property
		Public ReadOnly Property IsHost As StrucElement
			Get
                Return MyBase.GetItem(18)
            End Get
        End Property
		Public ReadOnly Property LastSyncBy As StrucElement
			Get
                Return MyBase.GetItem(19)
            End Get
        End Property

    Public Function GetElement(ByVal Key As Integer) As StrucElement
        Return MyBase.GetItem(Key)
    End Function
End Class
#End Region

End Namespace
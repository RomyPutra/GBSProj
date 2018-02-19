Imports SEAL.Data
imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace Actions
#Region "CONSIGNLOG Class"
Public NotInheritable Class CONSIGNLOG
	Inherits Core.CoreControl
	Private ConsignlogInfo As ConsignlogInfo = New ConsignlogInfo
	
	Public Sub New(ByVal Conn As String)
        ConnectionString = Conn
        ConnectionSetup()
    End Sub
		
	#Region "Data Manipulation-Add,Edit,Del"
	Private Function Save(ByVal ConsignlogCont As Container.Consignlog, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
		Dim strSQL As String = ""
        Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
        Dim rdr As System.Data.SqlClient.SqlDataReader
        Save = False
        Try
			If ConsignlogCont Is Nothing Then
                'Message return
            Else
				blnExec = False
                blnFound = False
                blnFlag = False
				If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
					With ConsignlogInfo.MyInfo
                        strSQL = BuildSelect(.CheckFields, .TableName, "TransID = '" & ConsignlogCont.TransID & "' AND ContractNo = '" & ConsignlogCont.ContractNo & "' AND GeneratorID = '" & ConsignlogCont.GeneratorID & "' AND GeneratorLocID = '" & ConsignlogCont.GeneratorLocID & "'")
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
							.TableName = "Consignlog"
							.AddField("ReferID", ConsignlogCont.ReferID,SQLControl.EnumDataType.dtString)
							.AddField("TransType", ConsignlogCont.TransType,SQLControl.EnumDataType.dtString)
							.AddField("TransDate", ConsignlogCont.TransDate,SQLControl.EnumDataType.dtDateTime)
							.AddField("Status", ConsignlogCont.Status,SQLControl.EnumDataType.dtNumeric)
							.AddField("CreateDate", ConsignlogCont.CreateDate,SQLControl.EnumDataType.dtDateTime)
							.AddField("CreateBy", ConsignlogCont.CreateBy,SQLControl.EnumDataType.dtString)
							.AddField("LastUpdate", ConsignlogCont.LastUpdate,SQLControl.EnumDataType.dtDateTime)
							.AddField("UpdateBy", ConsignlogCont.UpdateBy,SQLControl.EnumDataType.dtString)
							.AddField("Active", ConsignlogCont.Active,SQLControl.EnumDataType.dtNumeric)
							.AddField("Inuse", ConsignlogCont.Inuse,SQLControl.EnumDataType.dtNumeric)
							.AddField("rowguid", ConsignlogCont.rowguid,SQLControl.EnumDataType.dtString)
							.AddField("SyncCreate", ConsignlogCont.SyncCreate,SQLControl.EnumDataType.dtDateTime)
							.AddField("SyncLastUpd", ConsignlogCont.SyncLastUpd,SQLControl.EnumDataType.dtDateTime)
							.AddField("IsHost", ConsignlogCont.IsHost,SQLControl.EnumDataType.dtNumeric)
							.AddField("IsConfirm", ConsignlogCont.IsConfirm,SQLControl.EnumDataType.dtNumeric)
							.AddField("IsNew", ConsignlogCont.IsNew,SQLControl.EnumDataType.dtNumeric)
							.AddField("LastSyncBy", ConsignlogCont.LastSyncBy,SQLControl.EnumDataType.dtString)
							
							Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    If blnFound = True And blnFlag = False Then
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransID = '" & ConsignlogCont.TransID & "' AND ContractNo = '" & ConsignlogCont.ContractNo & "' AND GeneratorID = '" & ConsignlogCont.GeneratorID & "' AND GeneratorLocID = '" & ConsignlogCont.GeneratorLocID & "'")
									Else
                                        If blnFound = False Then		
											.AddField("TransID", ConsignlogCont.TransID, SQLControl.EnumDataType.dtString)
											.AddField("ContractNo", ConsignlogCont.ContractNo, SQLControl.EnumDataType.dtString)
											.AddField("GeneratorID", ConsignlogCont.GeneratorID, SQLControl.EnumDataType.dtString)
											.AddField("GeneratorLocID", ConsignlogCont.GeneratorLocID, SQLControl.EnumDataType.dtString)
											strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
										End If
									End If
								Case SQLControl.EnumSQLType.stUpdate
									strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransID = '" & ConsignlogCont.TransID & "' AND ContractNo = '" & ConsignlogCont.ContractNo & "' AND GeneratorID = '" & ConsignlogCont.GeneratorID & "' AND GeneratorLocID = '" & ConsignlogCont.GeneratorLocID & "'")
							End Select
						End With
						Try
                            If BatchExecute Then
                                BatchList.Add(strSQL)
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
			ConsignlogCont = Nothing
			rdr = Nothing
            EndSQLControl()
            EndConnection()
        End Try
	End Function
	
	'ADD
    Public Function Insert(ByVal ConsignlogCont As Container.Consignlog, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
        Return Save(ConsignlogCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
    End Function
	
	'AMEND
    Public Function Update(ByVal ConsignlogCont As Container.Consignlog, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
        Return Save(ConsignlogCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
    End Function
	
	Public Function Delete(ByVal ConsignlogCont As Container.Consignlog, ByRef message As String) As Boolean
        Dim strSQL As String
        Dim blnFound As Boolean
        Dim blnInUse As Boolean
        Dim rdr As System.Data.SqlClient.SqlDataReader
        Delete = False
        blnFound = False
        blnInUse = False
        Try
            If ConsignlogCont  Is Nothing Then
                'Error Message
            Else
				If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
					With ConsignlogInfo.MyInfo
                        strSQL = BuildSelect(.CheckFields, .TableName, "TransID = '" & ConsignlogCont.TransID & "' AND ContractNo = '" & ConsignlogCont.ContractNo & "' AND GeneratorID = '" & ConsignlogCont.GeneratorID & "' AND GeneratorLocID = '" & ConsignlogCont.GeneratorLocID & "'")
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
							strSQL = BuildUpdate(ConsignlogInfo.MyInfo.TableName, " SET Flag = 0" & _
							" , LastUpdate = '" & ConsignlogCont.LastUpdate & "' , UpdateBy = '" & _
							objSQL.ParseValue(SQLControl.EnumDataType.dtString, ConsignlogCont.UpdateBy) & "' WHERE" & _
							"TransID = '" & ConsignlogCont.TransID & "' AND ContractNo = '" & ConsignlogCont.ContractNo & "' AND GeneratorID = '" & ConsignlogCont.GeneratorID & "' AND GeneratorLocID = '" & ConsignlogCont.GeneratorLocID & "'")
						End With
					End if
					
					If blnFound = True And blnInUse = False Then
                        strSQL = BuildDelete(ConsignlogInfo.MyInfo.TableName, "TransID = '" & ConsignlogCont.TransID & "' AND ContractNo = '" & ConsignlogCont.ContractNo & "' AND GeneratorID = '" & ConsignlogCont.GeneratorID & "' AND GeneratorLocID = '" & ConsignlogCont.GeneratorLocID & "'")
                    End If
				
					Try
                        'execute
                        objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                        Return True
                    Catch exExecute As Exception
						message = exExecute.Message.ToString()
						Return False
                        'Throw New ApplicationException("210006 " & exExecute.Message.ToString())
                    End Try
				End If
			End If
			
		Catch axDelete As ApplicationException
			message = axDelete.Message.ToString()
			Return False
            'Throw axDelete
        Catch exDelete As Exception
			message = exDelete.Message.ToString()
			Return False
            'Throw exDelete
        Finally
            ConsignlogCont = Nothing
            rdr = Nothing
            EndSQLControl()
            EndConnection()
        End Try	
	End Function
#End Region
	
	#Region "Data Selection"
	Public Overloads Function GetCONSIGNLOG(ByVal TransID As System.String, ByVal ContractNo As System.String, ByVal GeneratorID As System.String, ByVal GeneratorLocID As System.String) As Container.Consignlog
		Dim rConsignlog As Container.Consignlog = Nothing
		Dim dtTemp As DataTable = Nothing
		Dim lstField As New List(Of String)
		Dim strSQL As String = Nothing
		
		Try
			If StartConnection() = True Then
				With ConsignlogInfo.MyInfo
					strSQL = BuildSelect(.FieldsList, .TableName, "TransID = '" & TransID & "' AND ContractNo = '" & ContractNo & "' AND GeneratorID = '" & GeneratorID & "' AND GeneratorLocID = '" & GeneratorLocID & "'")
					dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
					Dim rowCount As Integer = 0
					If dtTemp Is Nothing = False Then
						If dtTemp.Rows.Count > 0 Then
							Dim drRow = dtTemp.Rows(0)
							rCONSIGNLOG = New Container.CONSIGNLOG
							rCONSIGNLOG.TransID = drRow.Item("TransID")
							rCONSIGNLOG.ContractNo = drRow.Item("ContractNo")
							rCONSIGNLOG.GeneratorID = drRow.Item("GeneratorID")
							rCONSIGNLOG.GeneratorLocID = drRow.Item("GeneratorLocID")
							rConsignlog.ReferID = drRow.Item("ReferID")
							rConsignlog.TransType = drRow.Item("TransType")
							rConsignlog.TransDate = drRow.Item("TransDate")
							rConsignlog.Status = drRow.Item("Status")
							If Not IsDBNull(drRow.Item("CreateDate")) Then
								rConsignlog.CreateDate = drRow.Item("CreateDate")
							End IF
							rConsignlog.CreateBy = drRow.Item("CreateBy")
							If Not IsDBNull(drRow.Item("LastUpdate")) Then
								rConsignlog.LastUpdate = drRow.Item("LastUpdate")
							End IF
							rConsignlog.UpdateBy = drRow.Item("UpdateBy")
							rConsignlog.Active = drRow.Item("Active")
							rConsignlog.Inuse = drRow.Item("Inuse")
							rConsignlog.rowguid = drRow.Item("rowguid")
							rConsignlog.SyncCreate = drRow.Item("SyncCreate")
							rConsignlog.SyncLastUpd = drRow.Item("SyncLastUpd")
							rConsignlog.IsHost = drRow.Item("IsHost")
							rConsignlog.IsConfirm = drRow.Item("IsConfirm")
							rConsignlog.IsNew = drRow.Item("IsNew")
							rConsignlog.LastSyncBy = drRow.Item("LastSyncBy")
						Else
							rConsignlog = Nothing
						End If
					Else
						rConsignlog = Nothing
					End If
				End With
			End If
			Return rConsignlog
		Catch ex As Exception
			throw ex
		Finally
			rConsignlog = Nothing
			dtTemp = Nothing
			EndSQLControl()
            EndConnection()
		End Try
	End Function
	
	Public Overloads Function GetCONSIGNLOG(ByVal TransID As System.String, ByVal ContractNo As System.String, ByVal GeneratorID As System.String, ByVal GeneratorLocID As System.String, DecendingOrder as Boolean) As List(Of Container.Consignlog)
		Dim rConsignlog As Container.Consignlog = Nothing
		dim lstConsignlog As List(Of Container.Consignlog) = New List(Of Container.Consignlog)
		Dim dtTemp As DataTable = Nothing
		Dim lstField As New List(Of String)
		Dim strSQL As String = Nothing
		Dim strDesc As String = ""
		Try
			If StartConnection() = True Then
				With ConsignlogInfo.MyInfo
					If DecendingOrder Then
						strDesc = " Order by TransID, ContractNo, GeneratorID, GeneratorLocID DESC"
					End If
					strSQL = BuildSelect(.FieldsList, .TableName, "TransID = '" & TransID & "' AND ContractNo = '" & ContractNo & "' AND GeneratorID = '" & GeneratorID & "' AND GeneratorLocID = '" & GeneratorLocID & "'" & strDesc)
					dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
					Dim rowCount As Integer = 0
					If dtTemp Is Nothing = False Then
						For Each drRow As DataRow In dtTemp.Rows
							rCONSIGNLOG = New Container.CONSIGNLOG
							rCONSIGNLOG.TransID = drRow.Item("TransID")
							rCONSIGNLOG.ContractNo = drRow.Item("ContractNo")
							rCONSIGNLOG.GeneratorID = drRow.Item("GeneratorID")
							rCONSIGNLOG.GeneratorLocID = drRow.Item("GeneratorLocID")
							rConsignlog.ReferID = drRow.Item("ReferID")
							rConsignlog.TransType = drRow.Item("TransType")
							rConsignlog.TransDate = drRow.Item("TransDate")
							rConsignlog.Status = drRow.Item("Status")
							rConsignlog.CreateBy = drRow.Item("CreateBy")
							rConsignlog.UpdateBy = drRow.Item("UpdateBy")
							rConsignlog.Active = drRow.Item("Active")
							rConsignlog.Inuse = drRow.Item("Inuse")
							rConsignlog.rowguid = drRow.Item("rowguid")
							rConsignlog.SyncCreate = drRow.Item("SyncCreate")
							rConsignlog.SyncLastUpd = drRow.Item("SyncLastUpd")
							rConsignlog.IsHost = drRow.Item("IsHost")
							rConsignlog.IsConfirm = drRow.Item("IsConfirm")
							rConsignlog.IsNew = drRow.Item("IsNew")
							rConsignlog.LastSyncBy = drRow.Item("LastSyncBy")
							lstConsignlog.Add(rConsignlog)
						Next
						
					Else
						rConsignlog = Nothing
					End If
					Return lstConsignlog
				End With
			End If
		Catch ex As Exception
			Throw ex
		Finally
			rConsignlog = Nothing
			lstConsignlog = Nothing
			lstField = Nothing
			EndSQLControl()
            EndConnection()
		End Try
	End Function
		
	Public Overloads Function GetCONSIGNLOGList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
        If StartConnection() = True Then
            With ConsignlogInfo.MyInfo
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
		
	Public Overloads Function GetCONSIGNLOGShortList(ByVal ShortListing As Boolean) As Data.DataTable
        If StartConnection() = True Then
            With ConsignlogInfo.MyInfo
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
	

	#End Region
End Class
#End Region

#Region "Container"
Namespace Container
#Region "Consignlog Container"
public Class Consignlog_FieldName
	Public TransID As System.String = "TransID"
	Public ContractNo As System.String = "ContractNo"
	Public GeneratorID As System.String = "GeneratorID"
	Public GeneratorLocID As System.String = "GeneratorLocID"
	Public ReferID As System.String = "ReferID"
	Public TransType As System.String = "TransType"
	Public TransDate As System.String = "TransDate"
	Public Status As System.String = "Status"
	Public CreateDate As System.String = "CreateDate"
	Public CreateBy As System.String = "CreateBy"
	Public LastUpdate As System.String = "LastUpdate"
	Public UpdateBy As System.String = "UpdateBy"
	Public Active As System.String = "Active"
	Public Inuse As System.String = "Inuse"
	Public Flag As System.String = "Flag"
	Public rowguid As System.String = "rowguid"
	Public SyncCreate As System.String = "SyncCreate"
	Public SyncLastUpd As System.String = "SyncLastUpd"
	Public IsHost As System.String = "IsHost"
	Public IsConfirm As System.String = "IsConfirm"
	Public IsNew As System.String = "IsNew"
	Public LastSyncBy As System.String = "LastSyncBy"
End Class

Public Class Consignlog
	Protected _TransID As System.String
	Protected _ContractNo As System.String
	Protected _GeneratorID As System.String
	Protected _GeneratorLocID As System.String
	Private _ReferID As System.String
	Private _TransType As System.String
	Private _TransDate As System.DateTime
	Private _Status As System.Byte
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
	Private _IsConfirm As System.Byte
	Private _IsNew As System.Byte
	Private _LastSyncBy As System.String

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
	Public Property GeneratorID As System.String
		Get 
			Return _GeneratorID
		End Get
		Set(ByVal Value As System.String)
			_GeneratorID = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory
    ''' </summary>
	Public Property GeneratorLocID As System.String
		Get 
			Return _GeneratorLocID
		End Get
		Set(ByVal Value As System.String)
			_GeneratorLocID = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property ReferID As System.String
		Get 
			Return _ReferID
		End Get
		Set(ByVal Value As System.String)
			_ReferID = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property TransType As System.String
		Get 
			Return _TransType
		End Get
		Set(ByVal Value As System.String)
			_TransType = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property TransDate As System.DateTime
		Get 
			Return _TransDate
		End Get
		Set(ByVal Value As System.DateTime)
			_TransDate = Value
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
	Public Property IsConfirm As System.Byte
		Get 
			Return _IsConfirm
		End Get
		Set(ByVal Value As System.Byte)
			_IsConfirm = Value
		End Set
	End Property
	
	''' <summary>
	''' Mandatory, Not Allow Null
    ''' </summary>
	Public Property IsNew As System.Byte
		Get 
			Return _IsNew
		End Get
		Set(ByVal Value As System.Byte)
			_IsNew = Value
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
#Region "Consignlog Info"
public Class ConsignlogInfo
		Inherits Core.CoreBase
	Protected Overrides Sub InitializeClassInfo()
		With MyInfo
			.FieldsList = "TransID,ContractNo,GeneratorID,GeneratorLocID,ReferID,TransType,TransDate,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,IsConfirm,IsNew,LastSyncBy"
			.CheckFields = "Status,Active,Inuse,Flag,IsHost,IsConfirm,IsNew"
			.TableName = "Consignlog"
            .DefaultCond = Nothing
            .DefaultOrder = Nothing
            .Listing = "TransID,ContractNo,GeneratorID,GeneratorLocID,ReferID,TransType,TransDate,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,IsConfirm,IsNew,LastSyncBy"
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
#Region "CONSIGNLOG Scheme"
Public Class CONSIGNLOGScheme
Inherits Core.SchemeBase
	Protected Overrides Sub InitializeInfo()
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "TransID"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(0,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "ContractNo"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(1,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "GeneratorID"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(2,this)
		
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "GeneratorLocID"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(3,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "ReferID"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(4,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "TransType"
				.Length = 10
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(5,this)
			With this
				.DataType = SQLControl.EnumDataType.dtDateTime
				.FieldName = "TransDate"
				.Length = 8
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(6,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "Status"
				.Length = 1
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
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "IsConfirm"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(19,this)
			With this
				.DataType = SQLControl.EnumDataType.dtNumeric
				.FieldName = "IsNew"
				.Length = 1
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(20,this)
			With this
				.DataType = SQLControl.EnumDataType.dtString
				.FieldName = "LastSyncBy"
				.Length = 20
				.DecPlace = Nothing
				.RegExp = String.Empty
				.isMandatory = True
				.AllowNegative = False
			End With
			MyBase.AddItem(21,this)
		
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
		Public ReadOnly Property GeneratorID As StrucElement
			Get
                Return MyBase.GetItem(2)
            End Get
        End Property
		Public ReadOnly Property GeneratorLocID As StrucElement
			Get
                Return MyBase.GetItem(3)
            End Get
        End Property
	
		Public ReadOnly Property ReferID As StrucElement
			Get
                Return MyBase.GetItem(4)
            End Get
        End Property
		Public ReadOnly Property TransType As StrucElement
			Get
                Return MyBase.GetItem(5)
            End Get
        End Property
		Public ReadOnly Property TransDate As StrucElement
			Get
                Return MyBase.GetItem(6)
            End Get
        End Property
		Public ReadOnly Property Status As StrucElement
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
		Public ReadOnly Property IsConfirm As StrucElement
			Get
                Return MyBase.GetItem(19)
            End Get
        End Property
		Public ReadOnly Property IsNew As StrucElement
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
#End Region

End Namespace
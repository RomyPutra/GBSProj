
Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace TWG
#Region "TWG_TEMPLATEDTL Class"
    Public NotInheritable Class TWG_TEMPLATEDTL
        Inherits Core.CoreControl
        Private Twg_templatedtlInfo As Twg_templatedtlInfo = New Twg_templatedtlInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal Twg_templatedtlCont As Container.Twg_templatedtl, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If Twg_templatedtlCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Twg_templatedtlInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "TemplateID = '" & Twg_templatedtlCont.TemplateID & "' AND WasteCode = '" & Twg_templatedtlCont.WasteCode & "' AND WasteType = '" & Twg_templatedtlCont.WasteType & "' AND WasteName = '" & Twg_templatedtlCont.WasteName & "'")
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
                                .TableName = "Twg_templatedtl"
                                .AddField("SeqNo", Twg_templatedtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", Twg_templatedtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", Twg_templatedtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", Twg_templatedtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", Twg_templatedtlCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", Twg_templatedtlCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", Twg_templatedtlCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("rowguid", Twg_templatedtlCont.rowguid, SQLControl.EnumDataType.dtString)
                                .AddField("SyncCreate", Twg_templatedtlCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", Twg_templatedtlCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastSyncBy", Twg_templatedtlCont.LastSyncBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TemplateID = '" & Twg_templatedtlCont.TemplateID & "' AND WasteCode = '" & Twg_templatedtlCont.WasteCode & "' AND WasteType = '" & Twg_templatedtlCont.WasteType & "' AND WasteName = '" & Twg_templatedtlCont.WasteName & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("TemplateID", Twg_templatedtlCont.TemplateID, SQLControl.EnumDataType.dtString)
                                                .AddField("WasteCode", Twg_templatedtlCont.WasteCode, SQLControl.EnumDataType.dtString)
                                                .AddField("WasteType", Twg_templatedtlCont.WasteType, SQLControl.EnumDataType.dtString)
                                                .AddField("WasteName", Twg_templatedtlCont.WasteName, SQLControl.EnumDataType.dtStringN)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TemplateID = '" & Twg_templatedtlCont.TemplateID & "' AND WasteCode = '" & Twg_templatedtlCont.WasteCode & "' AND WasteType = '" & Twg_templatedtlCont.WasteType & "' AND WasteName = '" & Twg_templatedtlCont.WasteName & "'")
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
                Twg_templatedtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal Twg_templatedtlCont As Container.Twg_templatedtl, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Twg_templatedtlCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal Twg_templatedtlCont As Container.Twg_templatedtl, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Twg_templatedtlCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal Twg_templatedtlCont As Container.Twg_templatedtl, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Twg_templatedtlCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Twg_templatedtlInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "TemplateID = '" & Twg_templatedtlCont.TemplateID & "' AND WasteCode = '" & Twg_templatedtlCont.WasteCode & "' AND WasteType = '" & Twg_templatedtlCont.WasteType & "' AND WasteName = '" & Twg_templatedtlCont.WasteName & "'")
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
                                strSQL = BuildUpdate(Twg_templatedtlInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = '" & Twg_templatedtlCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_templatedtlCont.UpdateBy) & "' WHERE" & _
                                "TemplateID = '" & Twg_templatedtlCont.TemplateID & "' AND WasteCode = '" & Twg_templatedtlCont.WasteCode & "' AND WasteType = '" & Twg_templatedtlCont.WasteType & "' AND WasteName = '" & Twg_templatedtlCont.WasteName & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Twg_templatedtlInfo.MyInfo.TableName, "TemplateID = '" & Twg_templatedtlCont.TemplateID & "' AND WasteCode = '" & Twg_templatedtlCont.WasteCode & "' AND WasteType = '" & Twg_templatedtlCont.WasteType & "' AND WasteName = '" & Twg_templatedtlCont.WasteName & "'")
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
                Twg_templatedtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"


#End Region
    End Class
#End Region

#Region "Container"
    Namespace Container
#Region "Twg_templatedtl Container"
        Public Class Twg_templatedtl_FieldName
            Public TemplateID As System.String = "TemplateID"
            Public WasteCode As System.String = "WasteCode"
            Public WasteType As System.String = "WasteType"
            Public WasteName As System.String = "WasteName"
            Public SeqNo As System.String = "SeqNo"
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
            Public LastSyncBy As System.String = "LastSyncBy"
        End Class

        Public Class Twg_templatedtl
            Protected _TemplateID As System.String
            Protected _WasteCode As System.String
            Protected _WasteType As System.String
            Protected _WasteName As System.String
            Private _SeqNo As System.Int32
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _Active As System.Byte
            Private _Inuse As System.Byte
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _LastSyncBy As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property TemplateID As System.String
                Get
                    Return _TemplateID
                End Get
                Set(ByVal Value As System.String)
                    _TemplateID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property WasteCode As System.String
                Get
                    Return _WasteCode
                End Get
                Set(ByVal Value As System.String)
                    _WasteCode = Value
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
            Public Property WasteName As System.String
                Get
                    Return _WasteName
                End Get
                Set(ByVal Value As System.String)
                    _WasteName = Value
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
            ''' Non-Mandatory, Allow Null
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
#Region "Twg_templatedtl Info"
    Public Class Twg_templatedtlInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "TemplateID,WasteCode,WasteType,WasteName,SeqNo,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,LastSyncBy"
                .CheckFields = "Active,Inuse,Flag"
                .TableName = "Twg_templatedtl"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "TemplateID,WasteCode,WasteType,WasteName,SeqNo,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,LastSyncBy"
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
#Region "TWG_TEMPLATEDTL Scheme"
    Public Class TWG_TEMPLATEDTLScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TemplateID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WasteCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WasteType"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)

            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "WasteName"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "SeqNo"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)

        End Sub

        Public ReadOnly Property TemplateID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property WasteCode As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property WasteType As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property WasteName As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property

        Public ReadOnly Property SeqNo As StrucElement
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
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace


Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Log
#Region "MOB_ACTLOG Class"
    Public NotInheritable Class MobileActivityLog
        Inherits Core.CoreControl
        Private Mob_actlogInfo As Mob_actlogInfo = New Mob_actlogInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal Mob_actlogCont As Container.MobileActivityLog, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If Mob_actlogCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Mob_actlogInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "LogID = '" & Mob_actlogCont.LogID & "'")
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
                                .TableName = "Mob_actlog"
                                .AddField("SessionID", Mob_actlogCont.SessionID, SQLControl.EnumDataType.dtString)
                                .AddField("UserID", Mob_actlogCont.UserID, SQLControl.EnumDataType.dtString)
                                .AddField("DeviceID", Mob_actlogCont.DeviceID, SQLControl.EnumDataType.dtStringN)
                                .AddField("Longitude", Mob_actlogCont.Longitude, SQLControl.EnumDataType.dtStringN)
                                .AddField("Latitude", Mob_actlogCont.Latitude, SQLControl.EnumDataType.dtStringN)
                                .AddField("Flag", Mob_actlogCont.Flag, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Status", Mob_actlogCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", Mob_actlogCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LogDate", Mob_actlogCont.LogDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Active", Mob_actlogCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Remark", Mob_actlogCont.Remark, SQLControl.EnumDataType.dtStringN)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LogID = '" & Mob_actlogCont.LogID & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("LogID", Mob_actlogCont.LogID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LogID = '" & Mob_actlogCont.LogID & "'")
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
                Mob_actlogCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal Mob_actlogCont As Container.MobileActivityLog, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Mob_actlogCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal Mob_actlogCont As Container.MobileActivityLog, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Mob_actlogCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal Mob_actlogCont As Container.MobileActivityLog, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Mob_actlogCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Mob_actlogInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "LogID = '" & Mob_actlogCont.LogID & "'")
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

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Mob_actlogInfo.MyInfo.TableName, "LogID = '" & Mob_actlogCont.LogID & "'")
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
                Mob_actlogCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"

        Public Overloads Function SaveActivities(logID As String, userID As String, ByVal logDate As DateTime, ByVal DeviceID As String, ByVal lat As String, ByVal lng As String, ByVal ActivityLog As String, ByVal SaveType As SQLControl.EnumSQLType) As Boolean
            Try
                If StartConnection() = True Then
                    With Mob_actlogInfo.MyInfo
                        StartSQLControl()

                        Dim cont As Container.MobileActivityLog
                        cont = New Container.MobileActivityLog
                        cont.LogID = logID
                        cont.SessionID = logDate.ToString("ddMMyyyy") + userID
                        cont.UserID = userID
                        cont.DeviceID = DeviceID
                        cont.Latitude = lat
                        cont.Longitude = lng
                        cont.Flag = 1
                        cont.Status = 1
                        cont.CreateDate = System.DateTime.Now
                        cont.LogDate = logDate
                        cont.Active = 1
                        cont.Remark = ActivityLog

                        Return Save(cont, SaveType, "", False, Nothing, False)
                    End With
                Else
                    Return False
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eCNLogic/Security/UsrVerify", ex.Message & " " & strSQL, ex.StackTrace)
                'Throw ex
                Return False
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region
    End Class
#End Region

#Region "Container"
    Namespace Container
        Public Class MobileActivityLog_FieldName
            Public LogID As System.String = "LogID"
            Public SessionID As System.String = "SessionID"
            Public UserID As System.String = "UserID"
            Public DeviceID As System.String = "DeviceID"
            Public Longitude As System.String = "Longitude"
            Public Latitude As System.String = "Latitude"
            Public rowguid As System.String = "rowguid"
            Public Flag As System.String = "Flag"
            Public Status As System.String = "Status"
            Public CreateDate As System.String = "CreateDate"
            Public LogDate As System.String = "LogDate"
            Public Active As System.String = "Active"
            Public Remark As System.String = "Remark"
        End Class

        Public Class MobileActivityLog
            Protected _LogID As System.String
            Private _SessionID As System.String
            Private _UserID As System.String
            Private _DeviceID As System.String
            Private _Longitude As System.String
            Private _Latitude As System.String
            Private _rowguid As System.Guid
            Private _flag As System.Byte
            Private _Status As System.Byte
            Private _CreateDate As System.DateTime
            Private _LogDate As System.DateTime
            Private _Active As System.Byte
            Private _Remark As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property LogID As System.String
                Get
                    Return _LogID
                End Get
                Set(ByVal Value As System.String)
                    _LogID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SessionID As System.String
                Get
                    Return _SessionID
                End Get
                Set(ByVal Value As System.String)
                    _SessionID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property UserID As System.String
                Get
                    Return _UserID
                End Get
                Set(ByVal Value As System.String)
                    _UserID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property DeviceID As System.String
                Get
                    Return _DeviceID
                End Get
                Set(ByVal Value As System.String)
                    _DeviceID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Longitude As System.String
                Get
                    Return _Longitude
                End Get
                Set(ByVal Value As System.String)
                    _Longitude = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Latitude As System.String
                Get
                    Return _Latitude
                End Get
                Set(ByVal Value As System.String)
                    _Latitude = Value
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
            Public Property Flag As System.Byte
                Get
                    Return _flag
                End Get
                Set(ByVal Value As System.Byte)
                    _flag = Value
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
            Public Property LogDate As System.DateTime
                Get
                    Return _LogDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _LogDate = Value
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
            Public Property Remark As System.String
                Get
                    Return _Remark
                End Get
                Set(ByVal Value As System.String)
                    _Remark = Value
                End Set
            End Property

        End Class
    End Namespace
#End Region

#Region "Class Info"
#Region "Mob_actlog Info"
    Public Class Mob_actlogInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "LogID,SessionID,UserID,DeviceID,Longitude,Latitude,rowguid,Flag,Status,CreateDate,LogDate,Active,Remark"
                .CheckFields = "Flag,Status,Active"
                .TableName = "Mob_actlog"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "LogID,SessionID,UserID,DeviceID,Longitude,Latitude,rowguid,Flag,Status,CreateDate,LogDate,Active,Remark"
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
#Region "MOB_ACTLOG Scheme"
    Public Class MOB_ACTLOGScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LogID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "SessionID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UserID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "DeviceID"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Longitude"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Latitude"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LogDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark"
                .Length = 255
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)

        End Sub

        Public ReadOnly Property LogID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property SessionID As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property UserID As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property DeviceID As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property Longitude As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property Latitude As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property LogDate As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property Remark As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace
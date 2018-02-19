Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Inventory
    Public NotInheritable Class ItemComponent
        Inherits Core.CoreControl
        Private ItemcomponentInfo As ItemcomponentInfo = New ItemcomponentInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal connecn As String)
            ConnectionString = connecn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal ItemcomponentCont As Container.Itemcomponent, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ItemcomponentCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItemcomponentInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "CMAutoNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, ItemcomponentCont.CMAutoNo) & "' AND CMComponentName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, ItemcomponentCont.CMComponentName) & "'")
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
                                .TableName = "Itemcomponent"
                                .AddField("CMSymbol", ItemcomponentCont.CMSymbol, SQLControl.EnumDataType.dtStringN)
                                .AddField("UMUnitName", ItemcomponentCont.UMUnitName, SQLControl.EnumDataType.dtString)
                                .AddField("CMUserStamp", ItemcomponentCont.CMUserStamp, SQLControl.EnumDataType.dtString)
                                .AddField("CMTimeStamp", ItemcomponentCont.CMTimeStamp, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CMMethodNo", ItemcomponentCont.CMMethodNo, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CMUnitNo", ItemcomponentCont.CMUnitNo, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Active", ItemcomponentCont.Active, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "CMAutoNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, ItemcomponentCont.CMAutoNo) & "' AND CMComponentName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, ItemcomponentCont.CMComponentName) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("CMAutoNo", ItemcomponentCont.CMAutoNo, SQLControl.EnumDataType.dtNumeric)
                                                .AddField("CMComponentName", ItemcomponentCont.CMComponentName, SQLControl.EnumDataType.dtStringN)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "CMAutoNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, ItemcomponentCont.CMAutoNo) & "' AND CMComponentName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, ItemcomponentCont.CMComponentName) & "'")
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
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("WasteComponent", axExecute.Message & strSQL, axExecute.StackTrace)
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
                ItemcomponentCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal ItemcomponentCont As Container.Itemcomponent, ByRef message As String) As Boolean
            Return Save(ItemcomponentCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal ItemcomponentCont As Container.Itemcomponent, ByRef message As String) As Boolean
            Return Save(ItemcomponentCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal ItemcomponentCont As Container.Itemcomponent, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If ItemcomponentCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItemcomponentInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "CMAutoNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, ItemcomponentCont.CMAutoNo) & "' AND CMComponentName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, ItemcomponentCont.CMComponentName) & "'")
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
                                strSQL = BuildUpdate(ItemcomponentInfo.MyInfo.TableName, " SET Flag = 0 WHERE CMAutoNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, ItemcomponentCont.CMAutoNo) & "' AND CMComponentName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, ItemcomponentCont.CMComponentName) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(ItemcomponentInfo.MyInfo.TableName, "CMAutoNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, ItemcomponentCont.CMAutoNo) & "' AND CMComponentName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, ItemcomponentCont.CMComponentName) & "'")
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("WasteComponent", exExecute.Message & strSQL, exExecute.StackTrace)

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
                ItemcomponentCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetItemComponent(ByVal CMAutoNo As System.Int32, ByVal CMComponentName As System.String) As Container.Itemcomponent
            Dim rItemcomponent As Container.Itemcomponent = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With ItemcomponentInfo.MyInfo
                        StartSQLControl()
                        strSQL = BuildSelect(.FieldsList, .TableName, "CMAutoNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, CMAutoNo) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rItemcomponent = New Container.Itemcomponent
                                rItemcomponent.CMAutoNo = drRow.Item("CMAutoNo")
                                rItemcomponent.CMComponentName = drRow.Item("CMComponentName")
                                rItemcomponent.CMSymbol = drRow.Item("CMSymbol")
                                rItemcomponent.UMUnitName = drRow.Item("UMUnitName")
                                rItemcomponent.CMUserStamp = drRow.Item("CMUserStamp")
                                rItemcomponent.CMMethodNo = drRow.Item("CMMethodNo")
                                rItemcomponent.CMUnitNo = drRow.Item("CMUnitNo")
                                rItemcomponent.Active = drRow.Item("Active")
                            Else
                                rItemcomponent = Nothing
                            End If
                        Else
                            rItemcomponent = Nothing
                        End If
                    End With
                End If
                Return rItemcomponent
            Catch ex As Exception
                Throw ex
            Finally
                rItemcomponent = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetItemComponent(ByVal CMAutoNo As System.Int32, ByVal CMComponentName As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Itemcomponent)
            Dim rItemcomponent As Container.Itemcomponent = Nothing
            Dim lstItemcomponent As List(Of Container.Itemcomponent) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With ItemcomponentInfo.MyInfo
                        StartSQLControl()
                        If DecendingOrder Then
                            strDesc = " Order by ByVal CMAutoNo As System.Int32, ByVal CMComponentName As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "CMAutoNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, CMAutoNo) & "' AND CMComponentName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, CMComponentName) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rItemcomponent = New Container.Itemcomponent
                                rItemcomponent.CMAutoNo = drRow.Item("CMAutoNo")
                                rItemcomponent.CMComponentName = drRow.Item("CMComponentName")
                                rItemcomponent.CMSymbol = drRow.Item("CMSymbol")
                                rItemcomponent.UMUnitName = drRow.Item("UMUnitName")
                                rItemcomponent.CMUserStamp = drRow.Item("CMUserStamp")
                                rItemcomponent.CMMethodNo = drRow.Item("CMMethodNo")
                                rItemcomponent.CMUnitNo = drRow.Item("CMUnitNo")
                                rItemcomponent.Active = drRow.Item("Active")
                            Next
                            lstItemcomponent.Add(rItemcomponent)
                        Else
                            rItemcomponent = Nothing
                        End If
                        Return lstItemcomponent
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rItemcomponent = Nothing
                lstItemcomponent = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetItemComponentList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ItemcomponentInfo.MyInfo
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

        Public Overloads Function GetItemComponentShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With ItemcomponentInfo.MyInfo
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


    Namespace Container
#Region "Class Container"
        Public Class Itemcomponent
            Public fCMAutoNo As System.String = "CMAutoNo"
            Public fCMComponentName As System.String = "CMComponentName"
            Public fCMSymbol As System.String = "CMSymbol"
            Public fUMUnitName As System.String = "UMUnitName"
            Public fCMUserStamp As System.String = "CMUserStamp"
            Public fCMTimeStamp As System.String = "CMTimeStamp"
            Public fCMMethodNo As System.String = "CMMethodNo"
            Public fCMUnitNo As System.String = "CMUnitNo"
            Public fActive As System.String = "Active"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"

            Protected _CMAutoNo As System.Int32
            Protected _CMComponentName As System.String
            Private _CMSymbol As System.String
            Private _UMUnitName As System.String
            Private _CMUserStamp As System.String
            Private _CMTimeStamp As System.DateTime
            Private _CMMethodNo As System.Int32
            Private _CMUnitNo As System.Int32
            Private _Active As System.Byte
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property CMAutoNo As System.Int32
                Get
                    Return _CMAutoNo
                End Get
                Set(ByVal Value As System.Int32)
                    _CMAutoNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property CMComponentName As System.String
                Get
                    Return _CMComponentName
                End Get
                Set(ByVal Value As System.String)
                    _CMComponentName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CMSymbol As System.String
                Get
                    Return _CMSymbol
                End Get
                Set(ByVal Value As System.String)
                    _CMSymbol = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property UMUnitName As System.String
                Get
                    Return _UMUnitName
                End Get
                Set(ByVal Value As System.String)
                    _UMUnitName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CMUserStamp As System.String
                Get
                    Return _CMUserStamp
                End Get
                Set(ByVal Value As System.String)
                    _CMUserStamp = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property CMTimeStamp As System.DateTime
                Get
                    Return _CMTimeStamp
                End Get
                Set(ByVal Value As System.DateTime)
                    _CMTimeStamp = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CMMethodNo As System.Int32
                Get
                    Return _CMMethodNo
                End Get
                Set(ByVal Value As System.Int32)
                    _CMMethodNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CMUnitNo As System.Int32
                Get
                    Return _CMUnitNo
                End Get
                Set(ByVal Value As System.Int32)
                    _CMUnitNo = Value
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

        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class ItemcomponentInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "CMAutoNo,CMComponentName,CMSymbol,UMUnitName,CMUserStamp,CMTimeStamp,CMMethodNo,CMUnitNo,Active,SyncCreate,SyncLastUpd"
                .CheckFields = "Active"
                .TableName = "Itemcomponent"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "CMAutoNo,CMComponentName,CMSymbol,UMUnitName,CMUserStamp,CMTimeStamp,CMMethodNo,CMUnitNo,Active,SyncCreate,SyncLastUpd"
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
    Public Class ItemComponentScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "CMAutoNo"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "CMComponentName"
                .Length = 255
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "CMSymbol"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UMUnitName"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CMUserStamp"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CMTimeStamp"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "CMMethodNo"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "CMUnitNo"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)

        End Sub

        Public ReadOnly Property CMAutoNo As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property CMComponentName As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property

        Public ReadOnly Property CMSymbol As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property UMUnitName As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property CMUserStamp As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property CMTimeStamp As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property CMMethodNo As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property CMUnitNo As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
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

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace
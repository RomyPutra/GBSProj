
Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared


Namespace HELPS
#Region "HELPHDR Class"
    Public NotInheritable Class HELPHDR
        Inherits Core.CoreControl
        Private HelphdrInfo As HelphdrInfo = New HelphdrInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal HelphdrCont As Container.Helphdr, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If HelphdrCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With HelphdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "HelpID = '" & HelphdrCont.HelpID & "'")
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
                                .TableName = "Helphdr"
                                .AddField("Title", HelphdrCont.Title, SQLControl.EnumDataType.dtStringN)
                                .AddField("Description", HelphdrCont.Description, SQLControl.EnumDataType.dtStringN)
                                .AddField("CreateDate", HelphdrCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", HelphdrCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", HelphdrCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", HelphdrCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("rowguid", HelphdrCont.rowguid, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "HelpID = '" & HelphdrCont.HelpID & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("HelpID", HelphdrCont.HelpID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "HelpID = '" & HelphdrCont.HelpID & "'")
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
                HelphdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal HelphdrCont As Container.Helphdr, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(HelphdrCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal HelphdrCont As Container.Helphdr, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(HelphdrCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal HelphdrCont As Container.Helphdr, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If HelphdrCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With HelphdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "HelpID = '" & HelphdrCont.HelpID & "'")
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
                                strSQL = BuildUpdate(HelphdrInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = '" & HelphdrCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, HelphdrCont.UpdateBy) & "' WHERE" & _
                                "HelpID = '" & HelphdrCont.HelpID & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(HelphdrInfo.MyInfo.TableName, "HelpID = '" & HelphdrCont.HelpID & "'")
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
                HelphdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetHELPHDR(ByVal HelpID As System.String) As Container.Helphdr
            Dim rHelphdr As Container.Helphdr = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With HelphdrInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "HelpID = '" & HelpID & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rHELPHDR = New Container.HELPHDR
                                rHELPHDR.HelpID = drRow.Item("HelpID")
                                rHelphdr.Title = drRow.Item("Title")
                                rHelphdr.Description = drRow.Item("Description")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rHelphdr.CreateDate = drRow.Item("CreateDate")
                                End If
                                rHelphdr.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rHelphdr.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rHelphdr.UpdateBy = drRow.Item("UpdateBy")
                                rHelphdr.rowguid = drRow.Item("rowguid")
                            Else
                                rHelphdr = Nothing
                            End If
                        Else
                            rHelphdr = Nothing
                        End If
                    End With
                End If
                Return rHelphdr
            Catch ex As Exception
                Throw ex
            Finally
                rHelphdr = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetHELPHDR(ByVal HelpID As System.String, DecendingOrder As Boolean) As List(Of Container.Helphdr)
            Dim rHelphdr As Container.Helphdr = Nothing
            Dim lstHelphdr As List(Of Container.Helphdr) = New List(Of Container.Helphdr)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With HelphdrInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by HelpID DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "HelpID = '" & HelpID & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rHELPHDR = New Container.HELPHDR
                                rHELPHDR.HelpID = drRow.Item("HelpID")
                                rHelphdr.Title = drRow.Item("Title")
                                rHelphdr.Description = drRow.Item("Description")
                                rHelphdr.CreateBy = drRow.Item("CreateBy")
                                rHelphdr.UpdateBy = drRow.Item("UpdateBy")
                                rHelphdr.rowguid = drRow.Item("rowguid")
                                lstHelphdr.Add(rHelphdr)
                            Next

                        Else
                            rHelphdr = Nothing
                        End If
                        Return lstHelphdr
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rHelphdr = Nothing
                lstHelphdr = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetHELPHDRList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With HelphdrInfo.MyInfo
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

        Public Overloads Function GetHELPHDRShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With HelphdrInfo.MyInfo
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
#Region "Helphdr Container"
        Public Class Helphdr_FieldName
            Public HelpID As System.String = "HelpID"
            Public Title As System.String = "Title"
            Public Description As System.String = "Description"
            Public CreateDate As System.String = "CreateDate"
            Public CreateBy As System.String = "CreateBy"
            Public LastUpdate As System.String = "LastUpdate"
            Public UpdateBy As System.String = "UpdateBy"
            Public rowguid As System.String = "rowguid"
        End Class

        Public Class Helphdr
            Protected _HelpID As System.String
            Private _Title As System.String
            Private _Description As System.String
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _rowguid As System.Guid

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property HelpID As System.String
                Get
                    Return _HelpID
                End Get
                Set(ByVal Value As System.String)
                    _HelpID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Title As System.String
                Get
                    Return _Title
                End Get
                Set(ByVal Value As System.String)
                    _Title = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Description As System.String
                Get
                    Return _Description
                End Get
                Set(ByVal Value As System.String)
                    _Description = Value
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

        End Class
#End Region
    End Namespace
#End Region

#Region "Class Info"
#Region "Helphdr Info"
    Public Class HelphdrInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "HelpID,Title,Description,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid"
                .CheckFields = ""
                .TableName = "Helphdr"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "HelpID,Title,Description,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid"
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
#Region "HELPHDR Scheme"
    Public Class HELPHDRScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "HelpID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Title"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Description"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)

        End Sub

        Public ReadOnly Property HelpID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property Title As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property Description As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace
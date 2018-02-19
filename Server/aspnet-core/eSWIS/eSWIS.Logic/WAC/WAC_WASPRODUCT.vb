Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace WAC
#Region "WAC_WASPRODUCT Class"
    Public NotInheritable Class WAC_WASPRODUCT
        Inherits Core.CoreControl
        Private Wac_wasproductInfo As Wac_wasproductInfo = New Wac_wasproductInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal Wac_wasproductCont As Container.Wac_wasproduct, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If Wac_wasproductCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Wac_wasproductInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "WasCode = '" & Wac_wasproductCont.WasCode & "' AND WasType = '" & Wac_wasproductCont.WasType & "' AND TypeCode = '" & Wac_wasproductCont.TypeCode & "' AND ProductCode = '" & Wac_wasproductCont.ProductCode & "'")
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
                                .TableName = "Wac_wasproduct"
                                .AddField("ProductDesc", Wac_wasproductCont.ProductDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("Status", Wac_wasproductCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", Wac_wasproductCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", Wac_wasproductCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", Wac_wasproductCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", Wac_wasproductCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", Wac_wasproductCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", Wac_wasproductCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("SyncCreate", Wac_wasproductCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", Wac_wasproductCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastSyncBy", Wac_wasproductCont.LastSyncBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "WasCode = '" & Wac_wasproductCont.WasCode & "' AND WasType = '" & Wac_wasproductCont.WasType & "' AND TypeCode = '" & Wac_wasproductCont.TypeCode & "' AND ProductCode = '" & Wac_wasproductCont.ProductCode & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("WasCode", Wac_wasproductCont.WasCode, SQLControl.EnumDataType.dtString)
                                                .AddField("WasType", Wac_wasproductCont.WasType, SQLControl.EnumDataType.dtString)
                                                .AddField("TypeCode", Wac_wasproductCont.TypeCode, SQLControl.EnumDataType.dtString)
                                                .AddField("ProductCode", Wac_wasproductCont.ProductCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "WasCode = '" & Wac_wasproductCont.WasCode & "' AND WasType = '" & Wac_wasproductCont.WasType & "' AND TypeCode = '" & Wac_wasproductCont.TypeCode & "' AND ProductCode = '" & Wac_wasproductCont.ProductCode & "'")
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
                Wac_wasproductCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function Save(ByVal Wac_wasproductCont As List(Of Container.Wac_wasproduct), ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False, Optional ByVal WasCode As String = Nothing, Optional ByVal WasType As String = Nothing) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If Wac_wasproductCont Is Nothing OrElse Wac_wasproductCont.Count <= 0 Then
                    StartSQLControl()
                    strSQL = BuildDelete(Wac_wasproductInfo.MyInfo.TableName, "WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasType) & "'")
                    BatchList.Add(strSQL)
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Wac_wasproductInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "WasCode = '" & Wac_wasproductCont(0).WasCode & "' AND WasType = '" & Wac_wasproductCont(0).WasType & "' AND ProductCode = '" & Wac_wasproductCont(0).ProductCode & "'")
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
                            strSQL = BuildDelete(Wac_wasproductInfo.MyInfo.TableName, "WasCode = '" & Wac_wasproductCont(0).WasCode & "' AND WasType = '" & Wac_wasproductCont(0).WasType & "'")
                            BatchList.Add(strSQL)
                        End If

                        StartSQLControl()
                        For Each wac_product In Wac_wasproductCont
                            With objSQL
                                .TableName = "Wac_wasproduct"
                                .AddField("ProductDesc", wac_product.ProductDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("Status", wac_product.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", wac_product.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", wac_product.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", wac_product.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", wac_product.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", wac_product.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", wac_product.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("SyncCreate", wac_product.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", wac_product.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastSyncBy", wac_product.LastSyncBy, SQLControl.EnumDataType.dtString)

                                .AddField("WasCode", wac_product.WasCode, SQLControl.EnumDataType.dtString)
                                .AddField("WasType", wac_product.WasType, SQLControl.EnumDataType.dtString)
                                .AddField("TypeCode", wac_product.TypeCode, SQLControl.EnumDataType.dtString)
                                .AddField("ProductCode", wac_product.ProductCode, SQLControl.EnumDataType.dtString)
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                
                            End With
                            BatchList.Add(strSQL)
                        Next
                    End If
                End If
                Try
                    If BatchExecute Then
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
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Return False
            Finally
                Wac_wasproductCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal Wac_wasproductCont As Container.Wac_wasproduct, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Wac_wasproductCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'ADD
        Public Function Insert(ByVal Wac_wasproductCont As List(Of Container.Wac_wasproduct), ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False, Optional ByVal WasCode As String = Nothing, Optional ByVal WasType As String = Nothing) As Boolean
            Return Save(Wac_wasproductCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit, WasCode, WasType)
        End Function

        'AMEND
        Public Function Update(ByVal Wac_wasproductCont As Container.Wac_wasproduct, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Wac_wasproductCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal Wac_wasproductCont As Container.Wac_wasproduct, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Wac_wasproductCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Wac_wasproductInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "WasCode = '" & Wac_wasproductCont.WasCode & "' AND WasType = '" & Wac_wasproductCont.WasType & "' AND TypeCode = '" & Wac_wasproductCont.TypeCode & "' AND ProductCode = '" & Wac_wasproductCont.ProductCode & "'")
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
                                strSQL = BuildUpdate(Wac_wasproductInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = '" & Wac_wasproductCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_wasproductCont.UpdateBy) & "' WHERE" & _
                                "WasCode = '" & Wac_wasproductCont.WasCode & "' AND WasType = '" & Wac_wasproductCont.WasType & "' AND TypeCode = '" & Wac_wasproductCont.TypeCode & "' AND ProductCode = '" & Wac_wasproductCont.ProductCode & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Wac_wasproductInfo.MyInfo.TableName, "WasCode = '" & Wac_wasproductCont.WasCode & "' AND WasType = '" & Wac_wasproductCont.WasType & "' AND TypeCode = '" & Wac_wasproductCont.TypeCode & "' AND ProductCode = '" & Wac_wasproductCont.ProductCode & "'")
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
                Wac_wasproductCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetWAC_WASPRODUCT(ByVal WasCode As System.String, ByVal WasType As System.String, ByVal TypeCode As System.String, ByVal ProductCode As System.String) As Container.Wac_wasproduct
            Dim rWac_wasproduct As Container.Wac_wasproduct = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With Wac_wasproductInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "WasCode = '" & WasCode & "' AND WasType = '" & WasType & "' AND TypeCode = '" & TypeCode & "' AND ProductCode = '" & ProductCode & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWac_wasproduct = New Container.Wac_wasproduct
                                rWac_wasproduct.WasCode = drRow.Item("WasCode")
                                rWac_wasproduct.WasType = drRow.Item("WasType")
                                rWac_wasproduct.TypeCode = drRow.Item("TypeCode")
                                rWac_wasproduct.ProductCode = drRow.Item("ProductCode")
                                rWac_wasproduct.ProductDesc = drRow.Item("ProductDesc")
                                rWac_wasproduct.Status = drRow.Item("Status")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rWac_wasproduct.CreateDate = drRow.Item("CreateDate")
                                End If
                                rWac_wasproduct.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rWac_wasproduct.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rWac_wasproduct.UpdateBy = drRow.Item("UpdateBy")
                                rWac_wasproduct.Active = drRow.Item("Active")
                                rWac_wasproduct.Inuse = drRow.Item("Inuse")
                                rWac_wasproduct.rowguid = drRow.Item("rowguid")
                                rWac_wasproduct.SyncCreate = drRow.Item("SyncCreate")
                                rWac_wasproduct.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWac_wasproduct.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rWac_wasproduct = Nothing
                            End If
                        Else
                            rWac_wasproduct = Nothing
                        End If
                    End With
                End If
                Return rWac_wasproduct
            Catch ex As Exception
                Throw ex
            Finally
                rWac_wasproduct = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_WASPRODUCT(ByVal WasCode As System.String, ByVal WasType As System.String, ByVal TypeCode As System.String, ByVal ProductCode As System.String, DecendingOrder As Boolean) As List(Of Container.Wac_wasproduct)
            Dim rWac_wasproduct As Container.Wac_wasproduct = Nothing
            Dim lstWac_wasproduct As List(Of Container.Wac_wasproduct) = New List(Of Container.Wac_wasproduct)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With Wac_wasproductInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by WasCode, WasType, TypeCode, ProductCode DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "WasCode = '" & WasCode & "' AND WasType = '" & WasType & "' AND TypeCode = '" & TypeCode & "' AND ProductCode = '" & ProductCode & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rWac_wasproduct = New Container.Wac_wasproduct
                                rWac_wasproduct.WasCode = drRow.Item("WasCode")
                                rWac_wasproduct.WasType = drRow.Item("WasType")
                                rWac_wasproduct.TypeCode = drRow.Item("TypeCode")
                                rWac_wasproduct.ProductCode = drRow.Item("ProductCode")
                                rWac_wasproduct.ProductDesc = drRow.Item("ProductDesc")
                                rWac_wasproduct.Status = drRow.Item("Status")
                                rWac_wasproduct.CreateBy = drRow.Item("CreateBy")
                                rWac_wasproduct.UpdateBy = drRow.Item("UpdateBy")
                                rWac_wasproduct.Active = drRow.Item("Active")
                                rWac_wasproduct.Inuse = drRow.Item("Inuse")
                                rWac_wasproduct.rowguid = drRow.Item("rowguid")
                                rWac_wasproduct.SyncCreate = drRow.Item("SyncCreate")
                                rWac_wasproduct.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWac_wasproduct.LastSyncBy = drRow.Item("LastSyncBy")
                                lstWac_wasproduct.Add(rWac_wasproduct)
                            Next

                        Else
                            rWac_wasproduct = Nothing
                        End If
                        Return lstWac_wasproduct
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rWac_wasproduct = Nothing
                lstWac_wasproduct = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_WASPRODUCTList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Wac_wasproductInfo.MyInfo
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

        Public Overloads Function GetWAC_WASPRODUCTListByWASCODE(Optional ByVal WasCode As String = Nothing, Optional ByVal WasType As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_wasproductInfo.MyInfo
                    strSQL = "Select ProductCode, ProductDesc, Status, IIF(Status = '0', 'No', 'Yes') as StatusDesc  from WAC_WASPRODUCT WHERE WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasType) & "'"
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
#End Region

#Region "Container"
    Namespace Container
#Region "Wac_wasproduct Container"
        Public Class Wac_wasproduct_FieldName
            Public WasCode As System.String = "WasCode"
            Public WasType As System.String = "WasType"
            Public TypeCode As System.String = "TypeCode"
            Public ProductCode As System.String = "ProductCode"
            Public ProductDesc As System.String = "ProductDesc"
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
            Public LastSyncBy As System.String = "LastSyncBy"
        End Class

        Public Class Wac_wasproduct
            Protected _WasCode As System.String
            Protected _WasType As System.String
            Protected _TypeCode As System.String
            Protected _ProductCode As System.String
            Private _ProductDesc As System.String
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
            Private _LastSyncBy As System.String

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
            Public Property TypeCode As System.String
                Get
                    Return _TypeCode
                End Get
                Set(ByVal Value As System.String)
                    _TypeCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ProductCode As System.String
                Get
                    Return _ProductCode
                End Get
                Set(ByVal Value As System.String)
                    _ProductCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ProductDesc As System.String
                Get
                    Return _ProductDesc
                End Get
                Set(ByVal Value As System.String)
                    _ProductDesc = Value
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
#Region "Wac_wasproduct Info"
    Public Class Wac_wasproductInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "WasCode,WasType,TypeCode,ProductCode,ProductDesc,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,LastSyncBy"
                .CheckFields = "Status,Active,Inuse,Flag"
                .TableName = "Wac_wasproduct"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "WasCode,WasType,TypeCode,ProductCode,ProductDesc,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,LastSyncBy"
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
#Region "WAC_WASPRODUCT Scheme"
    Public Class WAC_WASPRODUCTScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WasCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WasType"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TypeCode"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ProductCode"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ProductDesc"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)

        End Sub

        Public ReadOnly Property WasCode As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property WasType As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property TypeCode As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property ProductCode As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property

        Public ReadOnly Property ProductDesc As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
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
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace
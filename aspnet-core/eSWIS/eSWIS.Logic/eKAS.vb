Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Logic
Imports eSWIS.Shared.General

Namespace eKAS
    Public NotInheritable Class DOEEntity
        Inherits Core.CoreControl
        Private DoefilenoregInfo As DoefilenoregInfo = New DoefilenoregInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal DoefilenoregCont As Container.Doefilenoreg, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If DoefilenoregCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With DoefilenoregInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DOEFileNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, DoefilenoregCont.DOEFileNo) & "'")
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
                                .TableName = "Doefilenoreg"
                                .AddField("ROCNo", DoefilenoregCont.ROCNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("CompanyName", DoefilenoregCont.CompanyName, SQLControl.EnumDataType.dtStringN)
                                .AddField("IndustryType", DoefilenoregCont.IndustryType, SQLControl.EnumDataType.dtString)
                                .AddField("BusinessType", DoefilenoregCont.BusinessType, SQLControl.EnumDataType.dtString)
                                .AddField("Address1", DoefilenoregCont.Address1, SQLControl.EnumDataType.dtStringN)
                                .AddField("Address2", DoefilenoregCont.Address2, SQLControl.EnumDataType.dtStringN)
                                .AddField("Address3", DoefilenoregCont.Address3, SQLControl.EnumDataType.dtString)
                                .AddField("Address4", DoefilenoregCont.Address4, SQLControl.EnumDataType.dtString)
                                .AddField("PostalCode", DoefilenoregCont.PostalCode, SQLControl.EnumDataType.dtString)
                                .AddField("State", DoefilenoregCont.State, SQLControl.EnumDataType.dtString)
                                .AddField("Country", DoefilenoregCont.Country, SQLControl.EnumDataType.dtString)
                                .AddField("PBT", DoefilenoregCont.PBT, SQLControl.EnumDataType.dtString)
                                .AddField("City", DoefilenoregCont.City, SQLControl.EnumDataType.dtString)
                                .AddField("Area", DoefilenoregCont.Area, SQLControl.EnumDataType.dtString)
                                .AddField("TelNo", DoefilenoregCont.TelNo, SQLControl.EnumDataType.dtString)
                                .AddField("FaxNo", DoefilenoregCont.FaxNo, SQLControl.EnumDataType.dtString)
                                .AddField("rowguid", DoefilenoregCont.rowguid, SQLControl.EnumDataType.dtString)
                                .AddField("CreateDate", DoefilenoregCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", DoefilenoregCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", DoefilenoregCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastUpdateBy", DoefilenoregCont.LastUpdateBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DOEFileNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, DoefilenoregCont.DOEFileNo) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("DOEFileNo", DoefilenoregCont.DOEFileNo, SQLControl.EnumDataType.dtStringN)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "DOEFileNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, DoefilenoregCont.DOEFileNo) & "'")
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
                DoefilenoregCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal DoefilenoregCont As Container.Doefilenoreg, ByRef message As String) As Boolean
            Return Save(DoefilenoregCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal DoefilenoregCont As Container.Doefilenoreg, ByRef message As String) As Boolean
            Return Save(DoefilenoregCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal DoefilenoregCont As Container.Doefilenoreg, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If DoefilenoregCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With DoefilenoregInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DOEFileNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, DoefilenoregCont.DOEFileNo) & "'")
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
                                strSQL = BuildUpdate(DoefilenoregInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, DoefilenoregCont.LastUpdateBy) & "' WHERE" & _
                                "DOEFileNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, DoefilenoregCont.DOEFileNo) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(DoefilenoregInfo.MyInfo.TableName, "DOEFileNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, DoefilenoregCont.DOEFileNo) & "'")
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
                DoefilenoregCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetDOEEntity(ByVal DOEFileNo As System.String) As Container.Doefilenoreg
            Dim rDoefilenoreg As Container.Doefilenoreg = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With DoefilenoregInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "DOEFileNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, DOEFileNo) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rDoefilenoreg = New Container.Doefilenoreg
                                rDoefilenoreg.DOEFileNo = drRow.Item("DOEFileNo")
                                rDoefilenoreg.ROCNo = drRow.Item("ROCNo")
                                rDoefilenoreg.CompanyName = drRow.Item("CompanyName")
                                rDoefilenoreg.IndustryType = drRow.Item("IndustryType")
                                rDoefilenoreg.BusinessType = drRow.Item("BusinessType")
                                rDoefilenoreg.Address1 = drRow.Item("Address1")
                                rDoefilenoreg.Address2 = drRow.Item("Address2")
                                rDoefilenoreg.Address3 = drRow.Item("Address3")
                                rDoefilenoreg.Address4 = drRow.Item("Address4")
                                rDoefilenoreg.PostalCode = drRow.Item("PostalCode")
                                rDoefilenoreg.State = drRow.Item("State")
                                rDoefilenoreg.Country = drRow.Item("Country")
                                rDoefilenoreg.PBT = drRow.Item("PBT")
                                rDoefilenoreg.City = drRow.Item("City")
                                rDoefilenoreg.Area = drRow.Item("Area")
                                rDoefilenoreg.TelNo = drRow.Item("TelNo")
                                rDoefilenoreg.FaxNo = drRow.Item("FaxNo")
                                rDoefilenoreg.rowguid = drRow.Item("rowguid")
                                rDoefilenoreg.CreateDate = drRow.Item("CreateDate")
                                rDoefilenoreg.CreateBy = drRow.Item("CreateBy")
                                rDoefilenoreg.LastUpdate = drRow.Item("LastUpdate")
                                rDoefilenoreg.LastUpdateBy = drRow.Item("LastUpdateBy")
                            Else
                                rDoefilenoreg = Nothing
                            End If
                        Else
                            rDoefilenoreg = Nothing
                        End If
                    End With
                End If
                Return rDoefilenoreg
            Catch ex As Exception
                Throw ex
            Finally
                rDoefilenoreg = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetDOEEntity(ByVal DOEFileNo As System.String, DecendingOrder As Boolean) As List(Of Container.Doefilenoreg)
            Dim rDoefilenoreg As Container.Doefilenoreg = Nothing
            Dim lstDoefilenoreg As List(Of Container.Doefilenoreg) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With DoefilenoregInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal DOEFileNo As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "DOEFileNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, DOEFileNo) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rDoefilenoreg = New Container.Doefilenoreg
                                rDoefilenoreg.DOEFileNo = drRow.Item("DOEFileNo")
                                rDoefilenoreg.ROCNo = drRow.Item("ROCNo")
                                rDoefilenoreg.CompanyName = drRow.Item("CompanyName")
                                rDoefilenoreg.IndustryType = drRow.Item("IndustryType")
                                rDoefilenoreg.BusinessType = drRow.Item("BusinessType")
                                rDoefilenoreg.Address1 = drRow.Item("Address1")
                                rDoefilenoreg.Address2 = drRow.Item("Address2")
                                rDoefilenoreg.Address3 = drRow.Item("Address3")
                                rDoefilenoreg.Address4 = drRow.Item("Address4")
                                rDoefilenoreg.PostalCode = drRow.Item("PostalCode")
                                rDoefilenoreg.State = drRow.Item("State")
                                rDoefilenoreg.Country = drRow.Item("Country")
                                rDoefilenoreg.PBT = drRow.Item("PBT")
                                rDoefilenoreg.City = drRow.Item("City")
                                rDoefilenoreg.Area = drRow.Item("Area")
                                rDoefilenoreg.TelNo = drRow.Item("TelNo")
                                rDoefilenoreg.FaxNo = drRow.Item("FaxNo")
                                rDoefilenoreg.rowguid = drRow.Item("rowguid")
                                rDoefilenoreg.CreateDate = drRow.Item("CreateDate")
                                rDoefilenoreg.CreateBy = drRow.Item("CreateBy")
                                rDoefilenoreg.LastUpdate = drRow.Item("LastUpdate")
                                rDoefilenoreg.LastUpdateBy = drRow.Item("LastUpdateBy")
                            Next
                            lstDoefilenoreg.Add(rDoefilenoreg)
                        Else
                            rDoefilenoreg = Nothing
                        End If
                        Return lstDoefilenoreg
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rDoefilenoreg = Nothing
                lstDoefilenoreg = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetDOEEntity(ByVal DOEFileNo As System.String, ByVal CompanyName As System.String) As Container.Doefilenoreg
            Dim rDoefilenoreg As Container.Doefilenoreg = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With DoefilenoregInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "DOEFileNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, DOEFileNo) & "' AND COMPANYNAME = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyName) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rDoefilenoreg = New Container.Doefilenoreg
                                rDoefilenoreg.DOEFileNo = drRow.Item("DOEFileNo")
                                rDoefilenoreg.ROCNo = drRow.Item("ROCNo")
                                rDoefilenoreg.CompanyName = drRow.Item("CompanyName")
                                rDoefilenoreg.IndustryType = drRow.Item("IndustryType")
                                rDoefilenoreg.BusinessType = drRow.Item("BusinessType")
                                rDoefilenoreg.Address1 = drRow.Item("Address1")
                                rDoefilenoreg.Address2 = drRow.Item("Address2")
                                rDoefilenoreg.Address3 = drRow.Item("Address3")
                                rDoefilenoreg.Address4 = drRow.Item("Address4")
                                rDoefilenoreg.PostalCode = drRow.Item("PostalCode")
                                rDoefilenoreg.State = drRow.Item("State")
                                rDoefilenoreg.Country = drRow.Item("Country")
                                rDoefilenoreg.PBT = drRow.Item("PBT")
                                rDoefilenoreg.City = drRow.Item("City")
                                rDoefilenoreg.Area = drRow.Item("Area")
                                rDoefilenoreg.TelNo = drRow.Item("TelNo")
                                rDoefilenoreg.FaxNo = drRow.Item("FaxNo")
                                rDoefilenoreg.rowguid = drRow.Item("rowguid")
                                rDoefilenoreg.CreateDate = drRow.Item("CreateDate")
                                rDoefilenoreg.CreateBy = drRow.Item("CreateBy")
                                rDoefilenoreg.LastUpdate = drRow.Item("LastUpdate")
                                rDoefilenoreg.LastUpdateBy = drRow.Item("LastUpdateBy")
                            Else
                                rDoefilenoreg = Nothing
                            End If
                        Else
                            rDoefilenoreg = Nothing
                        End If
                    End With
                End If
                Return rDoefilenoreg
            Catch ex As Exception
                Throw ex
            Finally
                rDoefilenoreg = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function


        Public Overloads Function GetDOEEntityList(Optional ByVal Flag As String = Nothing, Optional ByVal DOEFileNo As String = Nothing, Optional ByVal CompanyName As String = Nothing, Optional ByVal isPartial As Boolean = False) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With DoefilenoregInfo.MyInfo
                    strSQL = BuildSelect(.FieldsList, .TableName, " FLAG=" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Flag) & " AND DOEFileNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, DOEFileNo) & "'")
                    If isPartial Then
                        strSQL &= " AND CompanyName LIKE '%" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyName) & "%'"
                    Else
                        strSQL &= " AND CompanyName='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyName) & "'"
                    End If
                    Try
                        Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    Catch ex As Exception
                        Log.Notifier.Notify(ex)
                        Gibraltar.Agent.Log.Error("eKAS", ex.Message & " " & strSQL, ex.StackTrace)
                        Return Nothing
                    End Try
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetDOEEntityShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With DoefilenoregInfo.MyInfo
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
        Public Class Doefilenoreg
            Public fDOEFileNo As System.String = "DOEFileNo"
            Public fROCNo As System.String = "ROCNo"
            Public fCompanyName As System.String = "CompanyName"
            'add by dery, 20170517, Add Variable IsWG
            Public fIsWG As System.String = "IsWG"
            Public fIndustryType As System.String = "IndustryType"
            Public fBusinessType As System.String = "BusinessType"
            Public fAddress1 As System.String = "Address1"
            Public fAddress2 As System.String = "Address2"
            Public fAddress3 As System.String = "Address3"
            Public fAddress4 As System.String = "Address4"
            Public fPostalCode As System.String = "PostalCode"
            Public fState As System.String = "State"
            Public fCountry As System.String = "Country"
            Public fPBT As System.String = "PBT"
            Public fCity As System.String = "City"
            Public fArea As System.String = "Area"
            Public fTelNo As System.String = "TelNo"
            Public fFaxNo As System.String = "FaxNo"
            Public frowguid As System.String = "rowguid"
            Public fCreateDate As System.String = "CreateDate"
            Public fCreateBy As System.String = "CreateBy"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fLastUpdateBy As System.String = "LastUpdateBy"

            Protected _DOEFileNo As System.String
            Private _ROCNo As System.String
            Private _CompanyName As System.String
            'add by dery, 20170517, Add Variable IsWG
            Private _IsWG As System.String
            Private _IndustryType As System.String
            Private _BusinessType As System.String
            Private _Address1 As System.String
            Private _Address2 As System.String
            Private _Address3 As System.String
            Private _Address4 As System.String
            Private _PostalCode As System.String
            Private _State As System.String
            Private _Country As System.String
            Private _PBT As System.String
            Private _City As System.String
            Private _Area As System.String
            Private _TelNo As System.String
            Private _FaxNo As System.String
            Private _rowguid As System.Guid
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _LastUpdateBy As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property DOEFileNo As System.String
                Get
                    Return _DOEFileNo
                End Get
                Set(ByVal Value As System.String)
                    _DOEFileNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ROCNo As System.String
                Get
                    Return _ROCNo
                End Get
                Set(ByVal Value As System.String)
                    _ROCNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CompanyName As System.String
                Get
                    Return _CompanyName
                End Get
                Set(ByVal Value As System.String)
                    _CompanyName = Value
                End Set
            End Property

            'add by dery, 20170517, Add IsWG Property
            Public Property IsWG As System.String
                Get
                    Return _IsWG
                End Get
                Set(ByVal Value As System.String)
                    _IsWG = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IndustryType As System.String
                Get
                    Return _IndustryType
                End Get
                Set(ByVal Value As System.String)
                    _IndustryType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property BusinessType As System.String
                Get
                    Return _BusinessType
                End Get
                Set(ByVal Value As System.String)
                    _BusinessType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Address1 As System.String
                Get
                    Return _Address1
                End Get
                Set(ByVal Value As System.String)
                    _Address1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Address2 As System.String
                Get
                    Return _Address2
                End Get
                Set(ByVal Value As System.String)
                    _Address2 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Address3 As System.String
                Get
                    Return _Address3
                End Get
                Set(ByVal Value As System.String)
                    _Address3 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Address4 As System.String
                Get
                    Return _Address4
                End Get
                Set(ByVal Value As System.String)
                    _Address4 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PostalCode As System.String
                Get
                    Return _PostalCode
                End Get
                Set(ByVal Value As System.String)
                    _PostalCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property State As System.String
                Get
                    Return _State
                End Get
                Set(ByVal Value As System.String)
                    _State = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Country As System.String
                Get
                    Return _Country
                End Get
                Set(ByVal Value As System.String)
                    _Country = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PBT As System.String
                Get
                    Return _PBT
                End Get
                Set(ByVal Value As System.String)
                    _PBT = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property City As System.String
                Get
                    Return _City
                End Get
                Set(ByVal Value As System.String)
                    _City = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Area As System.String
                Get
                    Return _Area
                End Get
                Set(ByVal Value As System.String)
                    _Area = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TelNo As System.String
                Get
                    Return _TelNo
                End Get
                Set(ByVal Value As System.String)
                    _TelNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property FaxNo As System.String
                Get
                    Return _FaxNo
                End Get
                Set(ByVal Value As System.String)
                    _FaxNo = Value
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
            ''' Mandatory, Not Allow Null
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
            Public Property LastUpdateBy As System.String
                Get
                    Return _LastUpdateBy
                End Get
                Set(ByVal Value As System.String)
                    _LastUpdateBy = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class DoefilenoregInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                'add by dery, 20170517, Add IsWG Field
                .FieldsList = "ID,DOEFileNo,ROCNo,CompanyName,IndustryType,BusinessType,Address1,Address2,Address3,Address4,PostalCode,State,Country,PBT,City,Area,TelNo,FaxNo,rowguid,ISNULL(CreateDate,'') AS CreateDate,CreateBy,LastUpdate,LastUpdateBy,IsWG"
                .CheckFields = ""
                .TableName = "Doefilenoreg"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "ID,DOEFileNo,ROCNo,CompanyName,IndustryType,BusinessType,Address1,Address2,Address3,Address4,PostalCode,State,Country,PBT,City,Area,TelNo,FaxNo,rowguid,ISNULL(CreateDate,'') AS CreateDate,CreateBy,LastUpdate,LastUpdateBy"
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
    Public Class DOEEntityScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "DOEFileNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ROCNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "CompanyName"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "IndustryType"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BusinessType"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Address1"
                .Length = 255
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Address2"
                .Length = 100
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address3"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address4"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PostalCode"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "State"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Country"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PBT"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "City"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Area"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TelNo"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "FaxNo"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastUpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)

        End Sub

        Public ReadOnly Property DOEFileNo As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property ROCNo As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property CompanyName As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property IndustryType As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property BusinessType As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property Address1 As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property Address2 As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property Address3 As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property Address4 As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property PostalCode As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property State As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property Country As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property PBT As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property City As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property Area As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property TelNo As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property FaxNo As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property LastUpdateBy As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace
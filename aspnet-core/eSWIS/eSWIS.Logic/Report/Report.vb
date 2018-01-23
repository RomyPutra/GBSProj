Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared


Namespace Reports
    Public NotInheritable Class ReportList
        Inherits Core.CoreControl

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Report"

        Private Shared Function GetSqlFields(ByVal Fields As List(Of String)) As String
            Dim strFields As String = String.Empty
            If Fields IsNot Nothing Then
                For Each sField As String In Fields
                    If strFields = String.Empty Then
                        strFields = sField
                    Else
                        strFields += ", " & sField
                    End If
                Next
            End If
            Return strFields
        End Function

        Public Function GetSingleSYSREPORT(ByVal pRPTCode As String) As SysReport
            Dim objSYSREPORT_Info As SysReport
            Dim dt As DataTable
            Dim dateValue As DateTime
            Dim strSQL As String = String.Empty
            Dim strFields As String = String.Empty
            Dim strFilter As String = String.Empty
            Dim lstFields As New List(Of String)()

            Try
                StartSQLControl()

                lstFields.Add("SYSREPORT.APPID")
                lstFields.Add("SYSREPORT.RPTCode")
                lstFields.Add("SYSREPORT.RPTName")
                lstFields.Add("SYSREPORT.RPTDesc")
                lstFields.Add("SYSREPORT.RPTQuery")
                lstFields.Add("SYSREPORT.RPTType")
                lstFields.Add("SYSREPORT.RPTSection")
                lstFields.Add("SYSRPTTYPE.TypeDesc")
                lstFields.Add("SYSREPORT.rowguid")
                lstFields.Add("SYSREPORT.SyncCreate")
                lstFields.Add("SYSREPORT.SyncLastUpd")
                lstFields.Add("SYSREPORT.IsHost")
                lstFields.Add("SYSREPORT.LastSyncBy")
                lstFields.Add("SYSREPORT.QueryType") 'add by Ivan

                strFields = GetSqlFields(lstFields)
                strFilter = "WHERE SYSREPORT.RPTCode='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, pRPTCode) & "'"
                strSQL = "SELECT " & strFields & " FROM SYSREPORT WITH (NOLOCK) " & " INNER JOIN SYSRPTTYPE WITH (NOLOCK) ON SYSREPORT.RPTType = SYSRPTTYPE.RPTType " & strFilter

                dt = objConn.Execute(strSQL, CommandType.Text, False)

                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    Dim drRow As DataRow = dt.Rows(0)

                    objSYSREPORT_Info = New SysReport()
                    objSYSREPORT_Info.APPID = CInt(drRow("APPID"))
                    objSYSREPORT_Info.RPTCode = DirectCast(drRow("RPTCode"), String)
                    objSYSREPORT_Info.RPTName = DirectCast(drRow("RPTName"), String)
                    objSYSREPORT_Info.RPTDesc = DirectCast(drRow("RPTDesc"), String)
                    objSYSREPORT_Info.RPTQuery = DirectCast(drRow("RPTQuery"), String)
                    objSYSREPORT_Info.RPTTypeName = DirectCast(drRow("TypeDesc"), String)
                    objSYSREPORT_Info.RPTType = CByte(drRow("RPTType"))
                    objSYSREPORT_Info.RPTSection = CByte(drRow("RPTSection"))
                    objSYSREPORT_Info.rowguid = DirectCast(drRow("rowguid"), Guid)
                    If DateTime.TryParse(drRow("SyncCreate").ToString(), dateValue) Then
                        objSYSREPORT_Info.SyncCreate = DirectCast(drRow("SyncCreate"), DateTime)
                    End If
                    If DateTime.TryParse(drRow("SyncLastUpd").ToString(), dateValue) Then
                        objSYSREPORT_Info.SyncLastUpd = DirectCast(drRow("SyncLastUpd"), DateTime)
                    End If
                    objSYSREPORT_Info.IsHost = CByte(drRow("IsHost"))
                    objSYSREPORT_Info.LastSyncBy = CByte(drRow("LastSyncBy"))
                    objSYSREPORT_Info.QueryType = CInt(drRow("QueryType")) 'add by Ivan
                    Return objSYSREPORT_Info
                Else
                    Return Nothing
                    Throw New ApplicationException("SYSREPORT does not exist.")
                End If
            Catch ex As Exception
                Return Nothing
            Finally
                EndSQLControl()
            End Try
        End Function

        Public Function GetAllSYSRPTFIELD(ByVal RptCode As String, Optional ByVal FilterRpt As String = Nothing) As List(Of SysReportField)
            Dim objSYSRPTFIELD_Info As SysReportField
            Dim objListSYSRPTFIELD_Info As New List(Of SysReportField)()
            Dim dt As DataTable
            Dim dateValue As DateTime
            Dim strSQL As String = String.Empty

            Try
                StartConnection()
                StartSQLControl()

                strSQL = "SELECT * FROM SYSRPTFIELD WITH (NOLOCK) WHERE RPTCode='" & RptCode & "' " & FilterRpt & " ORDER BY SeqNo"

                dt = objConn.Execute(strSQL, CommandType.Text, False)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    For Each drRow As DataRow In dt.Rows
                        objSYSRPTFIELD_Info = New SysReportField()
                        objSYSRPTFIELD_Info.APPID = CInt(drRow("APPID"))
                        objSYSRPTFIELD_Info.RPTCode = DirectCast(drRow("RPTCode"), String)
                        objSYSRPTFIELD_Info.RptDBField = DirectCast(drRow("RptDBField"), String)
                        objSYSRPTFIELD_Info.RPTField = DirectCast(drRow("RPTField"), String)
                        objSYSRPTFIELD_Info.FieldType = CByte(drRow("FieldType"))
                        objSYSRPTFIELD_Info.FieldDef = DirectCast(drRow("FieldDef"), String)
                        objSYSRPTFIELD_Info.FieldAttb = DirectCast(drRow("FieldAttb"), String)
                        objSYSRPTFIELD_Info.IsReq = CInt(drRow("IsReq"))
                        objSYSRPTFIELD_Info.rowguid = DirectCast(drRow("rowguid"), Guid)
                        If DateTime.TryParse(drRow("SyncCreate").ToString(), dateValue) Then
                            objSYSRPTFIELD_Info.SyncCreate = DirectCast(drRow("SyncCreate"), DateTime)
                        End If
                        If DateTime.TryParse(drRow("SyncLastUpd").ToString(), dateValue) Then
                            objSYSRPTFIELD_Info.SyncLastUpd = DirectCast(drRow("SyncLastUpd"), DateTime)
                        End If
                        objSYSRPTFIELD_Info.IsHost = CByte(drRow("IsHost"))
                        objSYSRPTFIELD_Info.LastSyncBy = CByte(drRow("LastSyncBy"))
                        objListSYSRPTFIELD_Info.Add(objSYSRPTFIELD_Info)
                    Next
                    Return objListSYSRPTFIELD_Info
                Else
                    Return Nothing
                    Throw New ApplicationException("SYSRPTFIELD does not exist.")
                End If
            Catch ex As Exception
                Return Nothing
            Finally
                
            End Try
        End Function

        Public Function GetAllSYSREPORT(ByVal MODULE_REPORT As String, ByVal GroupCode As String) As DataTable
            Dim objListSYSREPORT_Info As New List(Of SysReport)()
            Dim dt As DataTable
            Dim dtReport As DataTable = Nothing
            Dim strSQL As String = String.Empty

            Try
                StartConnection()
                StartSQLControl()

                strSQL = "SELECT RptCode, ROW_NUMBER() OVER(ORDER BY seqno) AS Number, RptName" & _
                        " FROM PERMISSIONSET P WITH (NOLOCK) LEFT JOIN SYSFUNCTION F WITH (NOLOCK) on P.FunctionID=F.FunctionID and P.APPID = F.APPID" & _
                        " LEFT JOIN SYSREPORT R WITH (NOLOCK) on F.FunctionCode=R.RPTCode" & _
                        " WHERE P.moduleID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, MODULE_REPORT) & "' and P.accessCode in (" & GroupCode & ")" & _
                        " and isDeny = 0 and functionCode like 'GR%' order by seqno"
                dt = objConn.Execute(strSQL, CommandType.Text, False)
                
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    dtReport = dt.Clone
                    For Each drRow As DataRow In dt.Rows
                        
                        dtReport.ImportRow(drRow)
                        
                    Next drRow
                End If


                If dtReport IsNot Nothing AndAlso dtReport.Rows.Count > 0 Then
                    Return dtReport
                Else
                    Return Nothing
                    Throw New ApplicationException("SYSREPORT does not exist.")
                End If
            Catch ex As Exception
                Return Nothing
            Finally
                
            End Try
        End Function

        Private Shared Function GetFilterFields(ByVal Fields As List(Of String)) As String
            Dim strFields As String = String.Empty
            If Fields IsNot Nothing Then
                For Each sField As String In Fields
                    If strFields = String.Empty Then
                        strFields = " WHERE " & sField
                    Else
                        strFields += " AND " & sField
                    End If
                Next
            End If
            Return strFields
        End Function



#End Region

    End Class
    Public Class SysReport
        Private _aPPID As Integer
        Private _rPTCode As String = String.Empty
        Private _rPTName As String = String.Empty
        Private _rPTDesc As String = String.Empty
        Private _rPTTypeName As String = String.Empty
        Private _rPTQuery As String = String.Empty
        Private _rPTType As Byte
        Private _rPTSection As Byte
        Private _rowguid As Guid = Guid.Empty
        Private _syncCreate As DateTime
        Private _syncLastUpd As DateTime
        Private _isHost As Byte
        Private _lastSyncBy As Byte
        Private _queryType As Integer 'add by Ivan

#Region "Public Properties"

        Public Property APPID() As Integer
            Get
                Return _aPPID
            End Get
            Set(ByVal value As Integer)
                _aPPID = value
            End Set
        End Property
        Public Property RPTCode() As String
            Get
                Return _rPTCode
            End Get
            Set(ByVal value As String)
                _rPTCode = value
            End Set
        End Property
        Public Property RPTName() As String
            Get
                Return _rPTName
            End Get
            Set(ByVal value As String)
                _rPTName = value
            End Set
        End Property

        Public Property RPTDesc() As String
            Get
                Return _rPTDesc
            End Get
            Set(ByVal value As String)
                _rPTDesc = value
            End Set
        End Property

        Public Property RPTQuery() As String
            Get
                Return _rPTTypeName
            End Get
            Set(ByVal value As String)
                _rPTTypeName = value
            End Set
        End Property

        Public Property RPTTypeName() As String
            Get
                Return _rPTQuery
            End Get
            Set(ByVal value As String)
                _rPTQuery = value
            End Set
        End Property

        Public Property RPTType() As Byte
            Get
                Return _rPTType
            End Get
            Set(ByVal value As Byte)
                _rPTType = value
            End Set
        End Property

        Public Property RPTSection() As Byte
            Get
                Return _rPTSection
            End Get
            Set(ByVal value As Byte)
                _rPTSection = value
            End Set
        End Property

        Public Property rowguid() As Guid
            Get
                Return _rowguid
            End Get
            Set(ByVal value As Guid)
                _rowguid = value
            End Set
        End Property

        Public Property SyncCreate() As DateTime
            Get
                Return _syncCreate
            End Get
            Set(ByVal value As DateTime)
                _syncCreate = value
            End Set
        End Property

        Public Property SyncLastUpd() As DateTime
            Get
                Return _syncLastUpd
            End Get
            Set(ByVal value As DateTime)
                _syncLastUpd = value
            End Set
        End Property

        Public Property IsHost() As Byte
            Get
                Return _isHost
            End Get
            Set(ByVal value As Byte)
                _isHost = value
            End Set
        End Property

        Public Property LastSyncBy() As Byte
            Get
                Return _lastSyncBy
            End Get
            Set(ByVal value As Byte)
                _lastSyncBy = value
            End Set
        End Property

        'add by Ivan
        Public Property QueryType() As Integer
            Get
                Return _queryType
            End Get
            Set(ByVal value As Integer)
                _queryType = value
            End Set
        End Property

#End Region
    End Class

#Region "SysReportField"
    ''' <summary>
    ''' This object represents the properties and methods of a SYSRPTFIELD.
    ''' </summary>
    Public Class SysReportField
        Private _aPPID As Integer
        Private _rPTCode As String = String.Empty
        Private _rptDBField As String = String.Empty

        Private _rPTField As String = String.Empty
        Private _fieldType As Byte
        Private _fieldDef As String = String.Empty
        Private _fieldAttb As String = String.Empty
        Private _IsReq As Integer
        Private _rowguid As Guid = Guid.Empty
        Private _syncCreate As DateTime
        Private _syncLastUpd As DateTime
        Private _isHost As Byte
        Private _lastSyncBy As Byte

#Region "Public Properties"
        Public Property APPID() As Integer
            Get
                Return _aPPID
            End Get
            Set(ByVal value As Integer)
                _aPPID = value
            End Set
        End Property
        Public Property RPTCode() As String
            Get
                Return _rPTCode
            End Get
            Set(ByVal value As String)
                _rPTCode = value
            End Set
        End Property
        Public Property RptDBField() As String
            Get
                Return _rptDBField
            End Get
            Set(ByVal value As String)
                _rptDBField = value
            End Set
        End Property
        Public Property RPTField() As String
            Get
                Return _rPTField
            End Get
            Set(ByVal value As String)
                _rPTField = value
            End Set
        End Property

        Public Property FieldType() As Byte
            Get
                Return _fieldType
            End Get
            Set(ByVal value As Byte)
                _fieldType = value
            End Set
        End Property

        Public Property FieldDef() As String
            Get
                Return _fieldDef
            End Get
            Set(ByVal value As String)
                _fieldDef = value
            End Set
        End Property

        Public Property FieldAttb() As String
            Get
                Return _fieldAttb
            End Get
            Set(ByVal value As String)
                _fieldAttb = value
            End Set
        End Property

        Public Property IsReq() As Integer
            Get
                Return _isReq
            End Get
            Set(ByVal value As Integer)
                _isReq = value
            End Set
        End Property

        Public Property rowguid() As Guid
            Get
                Return _rowguid
            End Get
            Set(ByVal value As Guid)
                _rowguid = value
            End Set
        End Property

        Public Property SyncCreate() As DateTime
            Get
                Return _syncCreate
            End Get
            Set(ByVal value As DateTime)
                _syncCreate = value
            End Set
        End Property

        Public Property SyncLastUpd() As DateTime
            Get
                Return _syncLastUpd
            End Get
            Set(ByVal value As DateTime)
                _syncLastUpd = value
            End Set
        End Property

        Public Property IsHost() As Byte
            Get
                Return _isHost
            End Get
            Set(ByVal value As Byte)
                _isHost = value
            End Set
        End Property

        Public Property LastSyncBy() As Byte
            Get
                Return _lastSyncBy
            End Get
            Set(ByVal value As Byte)
                _lastSyncBy = value
            End Set
        End Property
#End Region

    End Class

#End Region


End Namespace

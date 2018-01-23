'Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General
Imports System.Data.SqlClient

Namespace UserSecurity
#Region "UsrVerify Class"
    Public NotInheritable Class UsrVerify

        Private connStr As String
        Private conn As SqlConnection

        Public Sub New(ByVal _connStr As String)
            connStr = _connStr
            conn = New SqlConnection(connStr)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"


        Public Overloads Function UpdateApproval(ByVal UserID As String, ByVal Status As String, ByVal RejectRemark As String) As Container.UserVerify
            Dim rUser As Container.UserVerify = Nothing
            If True = True Then

                Try
                    Dim strSQL = "UPDATE USRVERIFY Set Status ='" & Status & "', RejectRemark='" & RejectRemark & "' Where UserID='" & UserID & "'"

                    Dim cmd As SqlCommand = New SqlCommand(strSQL, conn)

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    rUser = New Container.UserVerify
                    With rUser
                        .UserID = UserID
                    End With

                Catch ex As Exception
                    Dim temp As String = ex.ToString()
                Finally
                    conn.Close()

                End Try

            End If
            conn.Close()
            Return rUser
        End Function


#End Region

#Region "Data Selection"


        'Public Overloads Function GetUsrAccessList(Optional ByVal FieldCond As String = Nothing) As List(Of Container.UserVerify)
        '    Dim rUser As Container.UserVerify = Nothing
        '    Dim listUser As List(Of Container.UserVerify) = Nothing
        '    Dim dtTemp As DataTable = New DataTable()

        '    If True = True Then
        '        conn.Open()
        '        Try
        '            Dim strSQL = "SELECT up.UserID, bz.BizRegID, bz.CompanyName, bz.RegNo, uv.RequestDate1, uv.RequestDate2, uv.VeriKey, uv.SyncCreate, uv.SyncLastUpd, uv.LastSyncBy, uv.Status," &
        '               " CASE WHEN uv.Status='0' THEN 'Pending' WHEN uv.Status='1' THEN 'Approved' ELSE 'Rejected' END AS StatusDesc" &
        '               " FROM USRVERIFY uv WITH (NOLOCK)" &
        '               " LEFT JOIN USRPROFILE up WITH (NOLOCK) ON uv.UserID=up.UserID" &
        '               " LEFT JOIN BIZENTITY bz WITH (NOLOCK) ON up.ParentID = bz.BizRegID" &
        '               " WHERE uv.UserType=2" & FieldCond & "ORDER BY uv.RequestDate1 DESC"

        '            Dim adapter As SqlDataAdapter = New SqlDataAdapter(strSQL, conn)
        '            adapter.Fill(dtTemp)

        '            If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
        '                listUser = New List(Of Container.UserVerify)
        '                For Each row As DataRow In dtTemp.Rows
        '                    Dim i As Integer
        '                    i += 1
        '                    Dim index As String = i.ToString
        '                    rUser = New Container.UserVerify
        '                    With rUser
        '                        '''rUser = New Container.UserVerify
        '                        .RowID = index
        '                        .UserID = row.Item("UserID")
        '                        .BizRegID = row.Item("BizRegID")
        '                        .RegNo = row.Item("RegNo")
        '                        .CompanyName = row.Item("CompanyName")
        '                        .StatusDesc = row.Item("StatusDesc")
        '                        .VeriKey = row.Item("VeriKey")
        '                        .Status = row.Item("Status")
        '                        .SyncCreate = row.Item("SyncCreate")
        '                        .SyncLastUpd = row.Item("SyncLastUpd")
        '                        .LastSyncBy = row.Item("LastSyncBy")
        '                        .RequestDate1 = row.Item("RequestDate1")
        '                        .RequestDate2 = row.Item("RequestDate2")
        '                    End With
        '                    listUser.Add(rUser)
        '                Next
        '            End If


        '        Catch ex As Exception
        '            Dim temp As String = ex.ToString()
        '        Finally
        '            conn.Close()
        '        End Try
        '    End If
        '    Return listUser

        'End Function

        Public Overloads Function GetUsrAccessListAll() As List(Of Container.UserVerify)
            Dim rUser As Container.UserVerify = Nothing
            Dim listUser As List(Of Container.UserVerify) = Nothing
            Dim dtTemp As DataTable = New DataTable()

            If True = True Then
                conn.Open()
                Try
                    Dim strSQL = "SELECT up.UserID, bz.BizRegID, bz.CompanyName, bz.RegNo, uv.RequestDate1, uv.RequestDate2, uv.VeriKey, uv.SyncCreate, uv.SyncLastUpd, uv.LastSyncBy, uv.Remark, bl.AccNo," &
                            " CASE WHEN uv.Status='0' THEN 'Pending' WHEN uv.Status='1' THEN 'Approved' ELSE 'Rejected' END AS StatusDesc, uv.Status, uv.RejectRemark " &
                            " FROM USRVERIFY uv WITH (NOLOCK)" &
                            " LEFT JOIN USRPROFILE up WITH (NOLOCK) ON uv.UserID=up.UserID" &
                            " LEFT JOIN BIZENTITY bz WITH (NOLOCK) ON up.ParentID = bz.BizRegID" &
                            " LEFT JOIN BIZLOCATE bl WITH (NOLOCK) ON up.ParentID = bl.BizRegID AND bl.BizLocID = up.RefID" &
                            " WHERE uv.UserType=2"

                    Dim adapter As SqlDataAdapter = New SqlDataAdapter(strSQL, conn)
                    adapter.Fill(dtTemp)

                    If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
                        listUser = New List(Of Container.UserVerify)
                        For Each row As DataRow In dtTemp.Rows
                            rUser = New Container.UserVerify
                            Dim i As Integer
                            i += 1
                            Dim index As String = i.ToString
                            With rUser
                                .RowID = index
                                .UserID = row.Item("UserID")
                                .BizRegID = row.Item("BizRegID")
                                .RegNo = row.Item("RegNo")
                                .CompanyName = row.Item("CompanyName")
                                .RequestDate1 = row.Item("RequestDate1")
                                .RequestDate2 = row.Item("RequestDate2")
                                .VeriKey = row.Item("VeriKey")
                                .SyncCreate = row.Item("SyncCreate")
                                .SyncLastUpd = row.Item("SyncLastUpd")
                                .LastSyncBy = row.Item("LastSyncBy")
                                .Remark = row.Item("Remark")
                                .AccNo = row.Item("AccNo")
                                .Status = row.Item("Status")
                                .StatusDesc = row.Item("StatusDesc")
                                .RejectRemark = row.Item("RejectRemark")
                            End With
                            listUser.Add(rUser)
                        Next
                    End If


                Catch ex As Exception
                    Dim temp As String = ex.ToString()
                Finally
                    conn.Close()
                End Try
            End If
            Return listUser

        End Function

        Public Overloads Function GetApprovalEdit(ByVal UserID As String) As Container.UserVerify
            Dim rUser As Container.UserVerify = Nothing
            Dim dtTemp As DataTable = New DataTable()

            If True = True Then
                conn.Open()
                Try
                    Dim strSQL = "SELECT up.UserID, bz.BizRegID, bz.CompanyName, bz.RegNo, uv.RequestDate1, uv.RequestDate2, uv.VeriKey, uv.SyncCreate, uv.SyncLastUpd, uv.LastSyncBy, uv.Remark, bl.AccNo," &
                            " CASE WHEN uv.Status='0' THEN 'Pending' WHEN uv.Status='1' THEN 'Approved' ELSE 'Rejected' END AS StatusDesc, uv.Status, uv.RejectRemark " &
                            " FROM USRVERIFY uv WITH (NOLOCK)" &
                            " LEFT JOIN USRPROFILE up WITH (NOLOCK) ON uv.UserID=up.UserID" &
                            " LEFT JOIN BIZENTITY bz WITH (NOLOCK) ON up.ParentID = bz.BizRegID" &
                            " LEFT JOIN BIZLOCATE bl WITH (NOLOCK) ON up.ParentID = bl.BizRegID AND bl.BizLocID = up.RefID" &
                            " WHERE uv.UserType=2 AND uv.UserID='" & UserID & "'"

                    Dim adapter As SqlDataAdapter = New SqlDataAdapter(strSQL, conn)
                    adapter.Fill(dtTemp)

                    If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then

                        rUser = New Container.UserVerify
                        With rUser
                            .UserID = dtTemp.Rows(0).Item("UserID")
                            .BizRegID = dtTemp.Rows(0).Item("BizRegID")
                            .RegNo = dtTemp.Rows(0).Item("RegNo")
                            .CompanyName = dtTemp.Rows(0).Item("CompanyName")
                            .RequestDate1 = dtTemp.Rows(0).Item("RequestDate1")
                            .RequestDate2 = dtTemp.Rows(0).Item("RequestDate2")
                            .VeriKey = dtTemp.Rows(0).Item("VeriKey")
                            .SyncCreate = dtTemp.Rows(0).Item("SyncCreate")
                            .SyncLastUpd = dtTemp.Rows(0).Item("SyncLastUpd")
                            .LastSyncBy = dtTemp.Rows(0).Item("LastSyncBy")
                            .Remark = dtTemp.Rows(0).Item("Remark")
                            .AccNo = dtTemp.Rows(0).Item("AccNo")
                            .Status = dtTemp.Rows(0).Item("Status")
                            .StatusDesc = dtTemp.Rows(0).Item("StatusDesc")
                            .RejectRemark = dtTemp.Rows(0).Item("RejectRemark")
                        End With
                    End If


                Catch ex As Exception
                    Dim temp As String = ex.ToString()
                Finally

                    conn.Close()

                End Try

            End If
            conn.Close()
            Return rUser
        End Function


#End Region
    End Class
#End Region

#Region "Container"
    Namespace Container
#Region "Usrverify Container"
        Public Class UserVerify
            Public fRowID As System.String = "RowID"
            Public fUserID As System.String = "UserID"
            Public fUserType As System.String = "UserType"
            Public fKeyIndex As System.String = "KeyIndex"
            Public fVeriKey As System.String = "VeriKey"
            Public fVeriCode As System.String = "VeriCode"
            Public VferiType As System.String = "VeriType"
            Public frowguid As System.String = "rowguid"
            Public fFlag As System.String = "Flag"
            Public fStatus As System.String = "Status"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fLastSyncBy As System.String = "LastSyncBy"
            Public fRequestDate1 As System.String = "RequestDate1"
            Public fRequestDate2 As System.String = "RequestDate2"
            Public fActive As System.String = "Active"
            Public fRemark As System.String = "Remark"
            Public fDeviceBrand As System.String = "DeviceBrand"
            Public fRejectRemark As System.String = "RejectRemark"
            Public fCompanyName As System.String = "CompanyName"
            Public fBizRegID As System.String = "BizRegID"
            Public fStatusDesc As System.String = "StatusDesc"
            Public fRegNo As System.String = "RegNo"
            Public fAccNo As System.String = "AccNo"

            Private _RowID As System.Double
            Protected _UserID As System.String
            Private _UserType As System.Byte
            Private _KeyIndex As System.Byte
            Private _VeriKey As System.String
            Private _VeriCode As System.Byte
            Private _VeriType As System.Byte
            Private _rowguid As System.Guid
            Private _Status As System.Byte
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _LastSyncBy As System.String
            Private _RequestDate1 As System.DateTime
            Private _RequestDate2 As System.DateTime
            Private _Active As System.Byte
            Private _Remark As System.String
            Private _DeviceBrand As System.String
            Private _RejectRemark As System.String
            Private _CompanyName As System.String
            Private _BizRegID As System.String
            Private _StatusDesc As System.String
            Private _RegNo As System.String
            Private _AccNo As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property RowID As System.Double
                Get
                    Return _RowID
                End Get
                Set(ByVal Value As System.Double)
                    _RowID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
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
            Public Property UserType As System.Byte
                Get
                    Return _UserType
                End Get
                Set(ByVal Value As System.Byte)
                    _UserType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property KeyIndex As System.Byte
                Get
                    Return _KeyIndex
                End Get
                Set(ByVal Value As System.Byte)
                    _KeyIndex = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property VeriKey As System.String
                Get
                    Return _VeriKey
                End Get
                Set(ByVal Value As System.String)
                    _VeriKey = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property VeriCode As System.Byte
                Get
                    Return _VeriCode
                End Get
                Set(ByVal Value As System.Byte)
                    _VeriCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property VeriType As System.Byte
                Get
                    Return _VeriType
                End Get
                Set(ByVal Value As System.Byte)
                    _VeriType = Value
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

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RequestDate1 As System.DateTime
                Get
                    Return _RequestDate1
                End Get
                Set(ByVal Value As System.DateTime)
                    _RequestDate1 = Value
                End Set
            End Property
            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RequestDate2 As System.DateTime
                Get
                    Return _RequestDate2
                End Get
                Set(ByVal Value As System.DateTime)
                    _RequestDate2 = Value
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

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property DeviceBrand As System.String
                Get
                    Return _DeviceBrand
                End Get
                Set(ByVal Value As System.String)
                    _DeviceBrand = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RejectRemark As System.String
                Get
                    Return _RejectRemark
                End Get
                Set(ByVal Value As System.String)
                    _RejectRemark = Value
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

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property BizRegID As System.String
                Get
                    Return _BizRegID
                End Get
                Set(ByVal Value As System.String)
                    _BizRegID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property StatusDesc As System.String
                Get
                    Return _StatusDesc
                End Get
                Set(ByVal Value As System.String)
                    _StatusDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RegNo As System.String
                Get
                    Return _RegNo
                End Get
                Set(ByVal Value As System.String)
                    _RegNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property AccNo As System.String
                Get
                    Return _AccNo
                End Get
                Set(ByVal Value As System.String)
                    _AccNo = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace
#End Region

#Region "Class Info"
#Region "Usrverify Info"
    Public Class UsrverifyInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "UserID,UserType,KeyIndex,VeriKey,VeriCode,VeriType,rowguid,Flag,Status,SyncCreate,SyncLastUpd,LastSyncBy,RequestDate1,RequestDate2,Active,Remark"
                .CheckFields = "UserType,KeyIndex,VeriCode,VeriType,Flag,Status,Active"
                .TableName = "Usrverify"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "UserID,UserType,KeyIndex,VeriKey,VeriCode,VeriType,rowguid,Flag,Status,SyncCreate,SyncLastUpd,LastSyncBy,RequestDate1,RequestDate2,Active,Remark"
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
#Region "UsrVerify Scheme"
    Public Class UsrVerifyScheme
        Inherits Core.SchemeBase


        Public ReadOnly Property UserID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property UserType As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property KeyIndex As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property VeriKey As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property VeriCode As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property VeriType As StrucElement
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
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property Remark As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace
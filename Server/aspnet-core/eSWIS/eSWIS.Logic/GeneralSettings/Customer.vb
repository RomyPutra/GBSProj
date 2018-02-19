Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Customer
    Public NotInheritable Class Customer
        Inherits Core.CoreControl
        Private CUSTOMERInfo As CUSTOMERInfo = New CUSTOMERInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal CUSTOMERCont As Container.CUSTOMER, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If CUSTOMERCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With CUSTOMERInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "CustomerID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CUSTOMERCont.CustomerID) & "'")
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
                                .TableName = "CUSTOMER"
                                .AddField("BranchID", CUSTOMERCont.BranchID, SQLControl.EnumDataType.dtString)
                                .AddField("CategoryID", CUSTOMERCont.CategoryID, SQLControl.EnumDataType.dtString)
                                .AddField("PrivilegeCode", CUSTOMERCont.PrivilegeCode, SQLControl.EnumDataType.dtString)
                                .AddField("Salutation", CUSTOMERCont.Salutation, SQLControl.EnumDataType.dtString)
                                .AddField("Surname", CUSTOMERCont.Surname, SQLControl.EnumDataType.dtStringN)
                                .AddField("FirstName", CUSTOMERCont.FirstName, SQLControl.EnumDataType.dtStringN)
                                .AddField("NRICNo", CUSTOMERCont.NRICNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("Address1", CUSTOMERCont.Address1, SQLControl.EnumDataType.dtString)
                                .AddField("Address2", CUSTOMERCont.Address2, SQLControl.EnumDataType.dtString)
                                .AddField("Address3", CUSTOMERCont.Address3, SQLControl.EnumDataType.dtString)
                                .AddField("Address4", CUSTOMERCont.Address4, SQLControl.EnumDataType.dtString)
                                .AddField("PostalCode", CUSTOMERCont.PostalCode, SQLControl.EnumDataType.dtString)
                                .AddField("State", CUSTOMERCont.State, SQLControl.EnumDataType.dtString)
                                .AddField("Country", CUSTOMERCont.Country, SQLControl.EnumDataType.dtString)
                                .AddField("DOB", CUSTOMERCont.DOB, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Sex", CUSTOMERCont.Sex, SQLControl.EnumDataType.dtString)
                                .AddField("Race", CUSTOMERCont.Race, SQLControl.EnumDataType.dtString)
                                .AddField("Religion", CUSTOMERCont.Religion, SQLControl.EnumDataType.dtString)
                                .AddField("Nationality", CUSTOMERCont.Nationality, SQLControl.EnumDataType.dtString)
                                .AddField("Marital", CUSTOMERCont.Marital, SQLControl.EnumDataType.dtString)
                                .AddField("Occupation", CUSTOMERCont.Occupation, SQLControl.EnumDataType.dtStringN)
                                .AddField("HomeTel", CUSTOMERCont.HomeTel, SQLControl.EnumDataType.dtString)
                                .AddField("OfficeTel", CUSTOMERCont.OfficeTel, SQLControl.EnumDataType.dtString)
                                .AddField("FaxNo", CUSTOMERCont.FaxNo, SQLControl.EnumDataType.dtString)
                                .AddField("MobileNo", CUSTOMERCont.MobileNo, SQLControl.EnumDataType.dtString)
                                .AddField("Email", CUSTOMERCont.Email, SQLControl.EnumDataType.dtStringN)
                                .AddField("LoyaltyMember", CUSTOMERCont.LoyaltyMember, SQLControl.EnumDataType.dtNumeric)
                                .AddField("MemberID", CUSTOMERCont.MemberID, SQLControl.EnumDataType.dtString)
                                .AddField("AccumPoints", CUSTOMERCont.AccumPoints, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RefID", CUSTOMERCont.RefID, SQLControl.EnumDataType.dtString)
                                .AddField("Remarks", CUSTOMERCont.Remarks, SQLControl.EnumDataType.dtStringN)
                                .AddField("Status", CUSTOMERCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CommID", CUSTOMERCont.CommID, SQLControl.EnumDataType.dtString)
                                .AddField("IsEmployee", CUSTOMERCont.IsEmployee, SQLControl.EnumDataType.dtNumeric)
                                .AddField("EmployeeID", CUSTOMERCont.EmployeeID, SQLControl.EnumDataType.dtString)
                                .AddField("CrAccNo", CUSTOMERCont.CrAccNo, SQLControl.EnumDataType.dtString)
                                .AddField("FeedBack", CUSTOMERCont.FeedBack, SQLControl.EnumDataType.dtStringN)
                                .AddField("Referral", CUSTOMERCont.Referral, SQLControl.EnumDataType.dtString)
                                .AddField("JoinDate", CUSTOMERCont.JoinDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateDate", CUSTOMERCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", CUSTOMERCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", CUSTOMERCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", CUSTOMERCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Inuse", CUSTOMERCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsHost", CUSTOMERCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RefInfo", CUSTOMERCont.RefInfo, SQLControl.EnumDataType.dtString)
                                .AddField("IMAGE", CUSTOMERCont.IMAGE, SQLControl.EnumDataType.dtStringN)
                                .AddField("Expiry", CUSTOMERCont.Expiry, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CustType", CUSTOMERCont.CustType, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "Where CustomerID ='" & .ParseValue(SQLControl.EnumDataType.dtString, CUSTOMERCont.CustomerID) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("CustomerID", CUSTOMERCont.CustomerID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE CustomerID='" & .ParseValue(SQLControl.EnumDataType.dtString, CUSTOMERCont.CustomerID) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                Else
                                    message = axExecute.Message.ToString()
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/Customer", axExecute.Message & strSQL, axExecute.StackTrace)
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If

                    End If
                End If
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/Customer", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/Customer", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                CUSTOMERCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal CUSTOMERCont As Container.CUSTOMER, ByRef message As String) As Boolean
            Return Save(CUSTOMERCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal CUSTOMERCont As Container.CUSTOMER, ByRef message As String) As Boolean
            Return Save(CUSTOMERCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal CUSTOMERCont As Container.CUSTOMER, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If CUSTOMERCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With CUSTOMERInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "CustomerID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CUSTOMERCont.CustomerID) & "'")
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
                                strSQL = BuildUpdate(CUSTOMERInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, CUSTOMERCont.UpdateBy) & "'" & _
                                " WHERE CustomerID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CUSTOMERCont.CustomerID) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(CUSTOMERInfo.MyInfo.TableName, "CustomerID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CUSTOMERCont.CustomerID) & "'")
                        End If

                        Try
                            'execute
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/Customer", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/Customer", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/GeneralSettings/Customer", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                CUSTOMERCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        
#End Region
    End Class


    Namespace Container
#Region "Class Container"
        Public Class CUSTOMER
            Public fCustomerID As System.String = "CustomerID"
            Public fBranchID As System.String = "BranchID"
            Public fCategoryID As System.String = "CategoryID"
            Public fPrivilegeCode As System.String = "PrivilegeCode"
            Public fSalutation As System.String = "Salutation"
            Public fSurname As System.String = "Surname"
            Public fFirstName As System.String = "FirstName"
            Public fNRICNo As System.String = "NRICNo"
            Public fAddress1 As System.String = "Address1"
            Public fAddress2 As System.String = "Address2"
            Public fAddress3 As System.String = "Address3"
            Public fAddress4 As System.String = "Address4"
            Public fPostalCode As System.String = "PostalCode"
            Public fState As System.String = "State"
            Public fCountry As System.String = "Country"
            Public fDOB As System.String = "DOB"
            Public fSex As System.String = "Sex"
            Public fRace As System.String = "Race"
            Public fReligion As System.String = "Religion"
            Public fNationality As System.String = "Nationality"
            Public fMarital As System.String = "Marital"
            Public fOccupation As System.String = "Occupation"
            Public fHomeTel As System.String = "HomeTel"
            Public fOfficeTel As System.String = "OfficeTel"
            Public fFaxNo As System.String = "FaxNo"
            Public fMobileNo As System.String = "MobileNo"
            Public fEmail As System.String = "Email"
            Public fLoyaltyMember As System.String = "LoyaltyMember"
            Public fMemberID As System.String = "MemberID"
            Public fAccumPoints As System.String = "AccumPoints"
            Public fRefID As System.String = "RefID"
            Public fRemarks As System.String = "Remarks"
            Public fStatus As System.String = "Status"
            Public fCommID As System.String = "CommID"
            Public fIsEmployee As System.String = "IsEmployee"
            Public fEmployeeID As System.String = "EmployeeID"
            Public fCrAccNo As System.String = "CrAccNo"
            Public fFeedBack As System.String = "FeedBack"
            Public fReferral As System.String = "Referral"
            Public fJoinDate As System.String = "JoinDate"
            Public fCreateDate As System.String = "CreateDate"
            Public fCreateBy As System.String = "CreateBy"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fUpdateBy As System.String = "UpdateBy"
            Public fFlag As System.String = "Flag"
            Public fInuse As System.String = "Inuse"
            Public frowguid As System.String = "rowguid"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fIsHost As System.String = "IsHost"
            Public fLastSyncBy As System.String = "LastSyncBy"
            Public fSyncCreateBy As System.String = "SyncCreateBy"
            Public fRefInfo As System.String = "RefInfo"
            Public fIMAGE As System.String = "IMAGE"
            Public fExpiry As System.String = "Expiry"
            Public fCustType As System.String = "CustType"

            Protected _CustomerID As System.String
            Private _BranchID As System.String
            Private _CategoryID As System.String
            Private _PrivilegeCode As System.String
            Private _Salutation As System.String
            Private _Surname As System.String
            Private _FirstName As System.String
            Private _NRICNo As System.String
            Private _Address1 As System.String
            Private _Address2 As System.String
            Private _Address3 As System.String
            Private _Address4 As System.String
            Private _PostalCode As System.String
            Private _State As System.String
            Private _Country As System.String
            Private _DOB As System.DateTime
            Private _Sex As System.String
            Private _Race As System.String
            Private _Religion As System.String
            Private _Nationality As System.String
            Private _Marital As System.String
            Private _Occupation As System.String
            Private _HomeTel As System.String
            Private _OfficeTel As System.String
            Private _FaxNo As System.String
            Private _MobileNo As System.String
            Private _Email As System.String
            Private _LoyaltyMember As System.Byte
            Private _MemberID As System.String
            Private _AccumPoints As System.Decimal
            Private _RefID As System.String
            Private _Remarks As System.String
            Private _Status As System.Byte
            Private _CommID As System.String
            Private _IsEmployee As System.Byte
            Private _EmployeeID As System.String
            Private _CrAccNo As System.String
            Private _FeedBack As System.String
            Private _Referral As System.String
            Private _JoinDate As System.DateTime
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _Inuse As System.Byte
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _IsHost As System.Byte
            Private _LastSyncBy As System.String
            Private _SyncCreateBy As System.String
            Private _RefInfo As System.String
            Private _IMAGE As System.String
            Private _Expiry As System.Byte
            Private _CustType As System.Byte

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property CustomerID As System.String
                Get
                    Return _CustomerID
                End Get
                Set(ByVal Value As System.String)
                    _CustomerID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property BranchID As System.String
                Get
                    Return _BranchID
                End Get
                Set(ByVal Value As System.String)
                    _BranchID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CategoryID As System.String
                Get
                    Return _CategoryID
                End Get
                Set(ByVal Value As System.String)
                    _CategoryID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PrivilegeCode As System.String
                Get
                    Return _PrivilegeCode
                End Get
                Set(ByVal Value As System.String)
                    _PrivilegeCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Salutation As System.String
                Get
                    Return _Salutation
                End Get
                Set(ByVal Value As System.String)
                    _Salutation = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Surname As System.String
                Get
                    Return _Surname
                End Get
                Set(ByVal Value As System.String)
                    _Surname = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property FirstName As System.String
                Get
                    Return _FirstName
                End Get
                Set(ByVal Value As System.String)
                    _FirstName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property NRICNo As System.String
                Get
                    Return _NRICNo
                End Get
                Set(ByVal Value As System.String)
                    _NRICNo = Value
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
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property DOB As System.DateTime
                Get
                    Return _DOB
                End Get
                Set(ByVal Value As System.DateTime)
                    _DOB = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Sex As System.String
                Get
                    Return _Sex
                End Get
                Set(ByVal Value As System.String)
                    _Sex = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Race As System.String
                Get
                    Return _Race
                End Get
                Set(ByVal Value As System.String)
                    _Race = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Religion As System.String
                Get
                    Return _Religion
                End Get
                Set(ByVal Value As System.String)
                    _Religion = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Nationality As System.String
                Get
                    Return _Nationality
                End Get
                Set(ByVal Value As System.String)
                    _Nationality = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Marital As System.String
                Get
                    Return _Marital
                End Get
                Set(ByVal Value As System.String)
                    _Marital = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Occupation As System.String
                Get
                    Return _Occupation
                End Get
                Set(ByVal Value As System.String)
                    _Occupation = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property HomeTel As System.String
                Get
                    Return _HomeTel
                End Get
                Set(ByVal Value As System.String)
                    _HomeTel = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OfficeTel As System.String
                Get
                    Return _OfficeTel
                End Get
                Set(ByVal Value As System.String)
                    _OfficeTel = Value
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
            Public Property MobileNo As System.String
                Get
                    Return _MobileNo
                End Get
                Set(ByVal Value As System.String)
                    _MobileNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Email As System.String
                Get
                    Return _Email
                End Get
                Set(ByVal Value As System.String)
                    _Email = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LoyaltyMember As System.Byte
                Get
                    Return _LoyaltyMember
                End Get
                Set(ByVal Value As System.Byte)
                    _LoyaltyMember = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property MemberID As System.String
                Get
                    Return _MemberID
                End Get
                Set(ByVal Value As System.String)
                    _MemberID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property AccumPoints As System.Decimal
                Get
                    Return _AccumPoints
                End Get
                Set(ByVal Value As System.Decimal)
                    _AccumPoints = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RefID As System.String
                Get
                    Return _RefID
                End Get
                Set(ByVal Value As System.String)
                    _RefID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Remarks As System.String
                Get
                    Return _Remarks
                End Get
                Set(ByVal Value As System.String)
                    _Remarks = Value
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
            Public Property CommID As System.String
                Get
                    Return _CommID
                End Get
                Set(ByVal Value As System.String)
                    _CommID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsEmployee As System.Byte
                Get
                    Return _IsEmployee
                End Get
                Set(ByVal Value As System.Byte)
                    _IsEmployee = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property EmployeeID As System.String
                Get
                    Return _EmployeeID
                End Get
                Set(ByVal Value As System.String)
                    _EmployeeID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CrAccNo As System.String
                Get
                    Return _CrAccNo
                End Get
                Set(ByVal Value As System.String)
                    _CrAccNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property FeedBack As System.String
                Get
                    Return _FeedBack
                End Get
                Set(ByVal Value As System.String)
                    _FeedBack = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Referral As System.String
                Get
                    Return _Referral
                End Get
                Set(ByVal Value As System.String)
                    _Referral = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property JoinDate As System.DateTime
                Get
                    Return _JoinDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _JoinDate = Value
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
            Public Property SyncCreateBy As System.String
                Get
                    Return _SyncCreateBy
                End Get
                Set(ByVal Value As System.String)
                    _SyncCreateBy = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RefInfo As System.String
                Get
                    Return _RefInfo
                End Get
                Set(ByVal Value As System.String)
                    _RefInfo = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property IMAGE As System.String
                Get
                    Return _IMAGE
                End Get
                Set(ByVal Value As System.String)
                    _IMAGE = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Expiry As System.Byte
                Get
                    Return _Expiry
                End Get
                Set(ByVal Value As System.Byte)
                    _Expiry = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CustType As System.Byte
                Get
                    Return _CustType
                End Get
                Set(ByVal Value As System.Byte)
                    _CustType = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class CUSTOMERInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "CustomerID,BranchID,CategoryID,PrivilegeCode,Salutation,Surname,FirstName,NRICNo,Address1,Address2,Address3,Address4,PostalCode,State,Country,DOB,Sex,Race,Religion,Nationality,Marital,Occupation,HomeTel,OfficeTel,FaxNo,MobileNo,Email,LoyaltyMember,MemberID,AccumPoints,RefID,Remarks,Status,CommID,IsEmployee,EmployeeID,CrAccNo,FeedBack,Referral,JoinDate,CreateDate,CreateBy,LastUpdate,UpdateBy,Flag,Inuse,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy,SyncCreateBy,RefInfo,IMAGE,Expiry,CustType"
                .CheckFields = "LoyaltyMember,Status,IsEmployee,Flag,Inuse,IsHost,Expiry,CustType"
                .TableName = "CUSTOMER"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "CustomerID,BranchID,CategoryID,PrivilegeCode,Salutation,Surname,FirstName,NRICNo,Address1,Address2,Address3,Address4,PostalCode,State,Country,DOB,Sex,Race,Religion,Nationality,Marital,Occupation,HomeTel,OfficeTel,FaxNo,MobileNo,Email,LoyaltyMember,MemberID,AccumPoints,RefID,Remarks,Status,CommID,IsEmployee,EmployeeID,CrAccNo,FeedBack,Referral,JoinDate,CreateDate,CreateBy,LastUpdate,UpdateBy,Flag,Inuse,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy,SyncCreateBy,RefInfo,IMAGE,Expiry,CustType"
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
    Public Class CustomerScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CustomerID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BranchID"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CategoryID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PrivilegeCode"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Salutation"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Surname"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "FirstName"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "NRICNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address1"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address2"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address3"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address4"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PostalCode"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "State"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Country"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "DOB"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Sex"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Race"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Religion"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Nationality"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Marital"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Occupation"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "HomeTel"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "OfficeTel"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "FaxNo"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "MobileNo"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Email"
                .Length = 80
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "LoyaltyMember"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(27, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "MemberID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(28, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "AccumPoints"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(29, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "RefID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(30, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remarks"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(31, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(32, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CommID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(33, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsEmployee"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(34, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "EmployeeID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(35, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CrAccNo"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(36, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "FeedBack"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(37, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Referral"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(38, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "JoinDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(39, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(40, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(41, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(42, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(43, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(44, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(45, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(46, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(47, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(48, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(49, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(50, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "SyncCreateBy"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(51, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "RefInfo"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(52, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "IMAGE"
                .Length = 255
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(53, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Expiry"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(54, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "CustType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(55, this)

        End Sub

        Public ReadOnly Property CustomerID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property BranchID As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property CategoryID As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property PrivilegeCode As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property Salutation As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property Surname As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property FirstName As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property NRICNo As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property Address1 As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property Address2 As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property Address3 As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property Address4 As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property PostalCode As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property State As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property Country As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property DOB As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property Sex As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property Race As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property Religion As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property Nationality As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property Marital As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property Occupation As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property HomeTel As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property OfficeTel As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property FaxNo As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property MobileNo As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property Email As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property LoyaltyMember As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property
        Public ReadOnly Property MemberID As StrucElement
            Get
                Return MyBase.GetItem(28)
            End Get
        End Property
        Public ReadOnly Property AccumPoints As StrucElement
            Get
                Return MyBase.GetItem(29)
            End Get
        End Property
        Public ReadOnly Property RefID As StrucElement
            Get
                Return MyBase.GetItem(30)
            End Get
        End Property
        Public ReadOnly Property Remarks As StrucElement
            Get
                Return MyBase.GetItem(31)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(32)
            End Get
        End Property
        Public ReadOnly Property CommID As StrucElement
            Get
                Return MyBase.GetItem(33)
            End Get
        End Property
        Public ReadOnly Property IsEmployee As StrucElement
            Get
                Return MyBase.GetItem(34)
            End Get
        End Property
        Public ReadOnly Property EmployeeID As StrucElement
            Get
                Return MyBase.GetItem(35)
            End Get
        End Property
        Public ReadOnly Property CrAccNo As StrucElement
            Get
                Return MyBase.GetItem(36)
            End Get
        End Property
        Public ReadOnly Property FeedBack As StrucElement
            Get
                Return MyBase.GetItem(37)
            End Get
        End Property
        Public ReadOnly Property Referral As StrucElement
            Get
                Return MyBase.GetItem(38)
            End Get
        End Property
        Public ReadOnly Property JoinDate As StrucElement
            Get
                Return MyBase.GetItem(39)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(40)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(41)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(42)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(43)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(44)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(45)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(46)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(47)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(48)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(49)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(50)
            End Get
        End Property
        Public ReadOnly Property SyncCreateBy As StrucElement
            Get
                Return MyBase.GetItem(51)
            End Get
        End Property
        Public ReadOnly Property RefInfo As StrucElement
            Get
                Return MyBase.GetItem(52)
            End Get
        End Property
        Public ReadOnly Property IMAGE As StrucElement
            Get
                Return MyBase.GetItem(53)
            End Get
        End Property
        Public ReadOnly Property Expiry As StrucElement
            Get
                Return MyBase.GetItem(54)
            End Get
        End Property
        Public ReadOnly Property CustType As StrucElement
            Get
                Return MyBase.GetItem(55)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace
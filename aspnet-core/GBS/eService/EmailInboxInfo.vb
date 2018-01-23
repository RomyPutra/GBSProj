Public Class EmailInboxInfo
#Region "Private Members"
    Private _EmailAddress As System.String
    Private _Subject As System.String
    Private _Message As System.String
    Private _BookingID As System.Int64
    Private _EmailType As System.Byte
    Private _UserName As System.String
    Private _CarrierCode As System.String
    Private _FlightNumber As System.String
    Private _ArrivalStation As System.String
    Private _DepartureStation As System.String
    Private _STA As System.DateTime
    Private _STD As System.DateTime
    Private _MobileNo As System.String
    Private _Status As System.Byte
    Private _CreateDate As System.DateTime
    Private _CreateBy As System.String
    Private _ReceivedDate As System.DateTime
    Private _DeletedDate As System.DateTime
    Private _SentDate As System.DateTime
    Private _Flag As System.Byte
    Private _Attempt As System.Byte
    Private _ReaderAttempt As System.Byte
    Private _Title As System.String
    Private _FirstName As System.String
    Private _LastName As System.String
    Private _AccessToken As System.String
    Private _RecordLocator As System.String
    Private _EmailFailedMsg As System.String
    Private _MsgID As System.String
    Private _ConditionText As System.String
    Private _HelpLinePassword As System.String
    Private _EmailTemplate As System.String
    Private _EmailBody As System.String
    Private _QueueCode As System.String
    Private _IsConnecting As System.Byte
    Private _CarrierCode2 As System.String
    Private _FlightNumber2 As System.String
    Private _ArrivalStation2 As System.String
    Private _DepartureStation2 As System.String
    Private _STA2 As System.DateTime
    Private _STD2 As System.DateTime
    Private _ArrivalStationName As System.String
    Private _DepartureStationName As System.String
    Private _ArrivalStationName2 As System.String
    Private _DepartureStationName2 As System.String
    Private _JourneySellKey As System.String
    Private _BatchNo As System.String
    Private _CreatedDate As System.DateTime
    Private _ExpiryDate As System.DateTime
    Private _AccountNumber As System.String
    Private _AgentName As System.String
    Private _DepartmentCode As System.String
    Private _LocationCode As System.String
    Private _EmailPassword As System.String
    Private _NextReminderDate As System.DateTime
    Private _CollectedAmt As System.Double
    Private _TransTotalAmt As System.Double
    Private _Currency As System.String
    Private _BalanceDue As System.Double
    Private _PaymentAmt As System.Double
    Private _AgentID As System.String
    Private _NextPaymentAmt As System.Double
#End Region

#Region "Public Properties"
    Public Property EmailAddress() As System.String
        Get
            Return _EmailAddress
        End Get
        Set(ByVal Value As System.String)
            _EmailAddress = Value
        End Set
    End Property

    Public Property Subject() As System.String
        Get
            Return _Subject
        End Get
        Set(ByVal Value As System.String)
            _Subject = Value
        End Set
    End Property

    Public Property Message() As System.String
        Get
            Return _Message
        End Get
        Set(ByVal Value As System.String)
            _Message = Value
        End Set
    End Property
    Public Property BookingID() As System.String
        Get
            Return _BookingID
        End Get
        Set(ByVal Value As System.String)
            _BookingID = Value
        End Set
    End Property
    Public Property EmailType() As System.Byte
        Get
            Return _EmailType
        End Get
        Set(ByVal Value As System.Byte)
            _EmailType = Value
        End Set
    End Property
    Public Property UserName() As System.String
        Get
            Return _UserName
        End Get
        Set(ByVal Value As System.String)
            _UserName = Value
        End Set
    End Property
    Public Property CarrierCode() As System.String
        Get
            Return _CarrierCode
        End Get
        Set(ByVal Value As System.String)
            _CarrierCode = Value
        End Set
    End Property
    Public Property RecordLocator() As System.String
        Get
            Return _RecordLocator
        End Get
        Set(ByVal Value As System.String)
            _RecordLocator = Value
        End Set
    End Property
    Public Property AccessToken() As System.String
        Get
            Return _AccessToken
        End Get
        Set(ByVal Value As System.String)
            _AccessToken = Value
        End Set
    End Property
    Public Property MobileNo() As System.String
        Get
            Return _MobileNo
        End Get
        Set(ByVal Value As System.String)
            _MobileNo = Value
        End Set
    End Property

    Public Property FlightNumber() As System.String
        Get
            Return _FlightNumber
        End Get
        Set(ByVal Value As System.String)
            _FlightNumber = Value
        End Set
    End Property
    Public Property ArrivalStation() As System.String
        Get
            Return _ArrivalStation
        End Get
        Set(ByVal Value As System.String)
            _ArrivalStation = Value
        End Set
    End Property
    Public Property DepartureStation() As System.String
        Get
            Return _DepartureStation
        End Get
        Set(ByVal Value As System.String)
            _DepartureStation = Value
        End Set
    End Property
    Public Property STA() As System.DateTime
        Get
            Return _STA
        End Get
        Set(ByVal Value As System.DateTime)
            _STA = Value
        End Set
    End Property
    Public Property STD() As System.DateTime
        Get
            Return _STD
        End Get
        Set(ByVal Value As System.DateTime)
            _STD = Value
        End Set
    End Property
    Public Property Status() As System.Byte
        Get
            Return _Status
        End Get
        Set(ByVal Value As System.Byte)
            _Status = Value
        End Set
    End Property
    Public Property CreateDate() As System.DateTime
        Get
            Return _CreateDate
        End Get
        Set(ByVal Value As System.DateTime)
            _CreateDate = Value
        End Set
    End Property
    Public Property CreateBy() As System.String
        Get
            Return _CreateBy
        End Get
        Set(ByVal Value As System.String)
            _CreateBy = Value
        End Set
    End Property
    Public Property ReceivedDate() As System.DateTime
        Get
            Return _ReceivedDate
        End Get
        Set(ByVal Value As System.DateTime)
            _ReceivedDate = Value
        End Set
    End Property
    Public Property DeletedDate() As System.DateTime
        Get
            Return _DeletedDate
        End Get
        Set(ByVal Value As System.DateTime)
            _DeletedDate = Value
        End Set
    End Property
    Public Property SentDate() As System.DateTime
        Get
            Return _SentDate
        End Get
        Set(ByVal Value As System.DateTime)
            _SentDate = Value
        End Set
    End Property

    Public Property Flag() As System.Byte
        Get
            Return _Flag
        End Get
        Set(ByVal Value As System.Byte)
            _Flag = Value
        End Set
    End Property

    Public Property Attempt() As System.Byte
        Get
            Return _Attempt
        End Get
        Set(ByVal Value As System.Byte)
            _Attempt = Value
        End Set
    End Property
    Public Property ReaderAttempt() As System.Byte
        Get
            Return _ReaderAttempt
        End Get
        Set(ByVal Value As System.Byte)
            _ReaderAttempt = Value
        End Set
    End Property
    Public Property FirstName() As System.String
        Get
            Return _FirstName
        End Get
        Set(ByVal Value As System.String)
            _FirstName = Value
        End Set
    End Property
    Public Property LastName() As System.String
        Get
            Return _LastName
        End Get
        Set(ByVal Value As System.String)
            _LastName = Value
        End Set
    End Property
    Public Property Title() As System.String
        Get
            Return _Title
        End Get
        Set(ByVal Value As System.String)
            _Title = Value
        End Set
    End Property
    Public Property EmailFailedMsg() As System.String
        Get
            Return _EmailFailedMsg
        End Get
        Set(ByVal Value As System.String)
            _EmailFailedMsg = Value
        End Set
    End Property
    Public Property MsgID() As System.String
        Get
            Return _MsgID
        End Get
        Set(ByVal Value As System.String)
            _MsgID = Value
        End Set
    End Property
    Public Property ConditionText() As System.String
        Get
            Return _ConditionText
        End Get
        Set(ByVal Value As System.String)
            _ConditionText = Value
        End Set
    End Property
    Public Property HelpLinePassword() As System.String
        Get
            Return _HelpLinePassword
        End Get
        Set(ByVal Value As System.String)
            _HelpLinePassword = Value
        End Set
    End Property
    Public Property EmailTemplate() As System.String
        Get
            Return _EmailTemplate
        End Get
        Set(ByVal Value As System.String)
            _EmailTemplate = Value
        End Set
    End Property
    Public Property EmailBody() As System.String
        Get
            Return _EmailBody
        End Get
        Set(ByVal Value As System.String)
            _EmailBody = Value
        End Set
    End Property
    Public Property QueueCode() As System.String
        Get
            Return _QueueCode
        End Get
        Set(ByVal Value As System.String)
            _QueueCode = Value
        End Set
    End Property
    Public Property IsConnecting() As System.Byte
        Get
            Return _IsConnecting
        End Get
        Set(ByVal Value As System.Byte)
            _IsConnecting = Value
        End Set
    End Property
    Public Property CarrierCode2() As System.String
        Get
            Return _CarrierCode2
        End Get
        Set(ByVal Value As System.String)
            _CarrierCode2 = Value
        End Set
    End Property
    Public Property FlightNumber2() As System.String
        Get
            Return _FlightNumber2
        End Get
        Set(ByVal Value As System.String)
            _FlightNumber2 = Value
        End Set
    End Property
    Public Property ArrivalStation2() As System.String
        Get
            Return _ArrivalStation2
        End Get
        Set(ByVal Value As System.String)
            _ArrivalStation2 = Value
        End Set
    End Property
    Public Property DepartureStation2() As System.String
        Get
            Return _DepartureStation2
        End Get
        Set(ByVal Value As System.String)
            _DepartureStation2 = Value
        End Set
    End Property
    Public Property STA2() As System.DateTime
        Get
            Return _STA2
        End Get
        Set(ByVal Value As System.DateTime)
            _STA2 = Value
        End Set
    End Property
    Public Property STD2() As System.DateTime
        Get
            Return _STD2
        End Get
        Set(ByVal Value As System.DateTime)
            _STD2 = Value
        End Set
    End Property
    Public Property ArrivalStationName() As System.String
        Get
            Return _ArrivalStationName
        End Get
        Set(ByVal Value As System.String)
            _ArrivalStationName = Value
        End Set
    End Property
    Public Property DepartureStationName() As System.String
        Get
            Return _DepartureStationName
        End Get
        Set(ByVal Value As System.String)
            _DepartureStationName = Value
        End Set
    End Property
    Public Property ArrivalStationName2() As System.String
        Get
            Return _ArrivalStationName2
        End Get
        Set(ByVal Value As System.String)
            _ArrivalStationName2 = Value
        End Set
    End Property
    Public Property DepartureStationName2() As System.String
        Get
            Return _DepartureStationName2
        End Get
        Set(ByVal Value As System.String)
            _DepartureStationName2 = Value
        End Set
    End Property
    Public Property JourneySellKey() As System.String
        Get
            Return _JourneySellKey
        End Get
        Set(ByVal Value As System.String)
            _JourneySellKey = Value
        End Set
    End Property
    Public Property BatchNo() As System.String
        Get
            Return _BatchNo
        End Get
        Set(ByVal Value As System.String)
            _BatchNo = Value
        End Set
    End Property
    Public Property CreatedDate() As System.DateTime
        Get
            Return _CreatedDate
        End Get
        Set(ByVal Value As System.DateTime)
            _CreatedDate = Value
        End Set
    End Property
    Public Property ExpiryDate() As System.DateTime
        Get
            Return _ExpiryDate
        End Get
        Set(ByVal Value As System.DateTime)
            _ExpiryDate = Value
        End Set
    End Property
    Public Property AccountNumber() As System.String
        Get
            Return _AccountNumber
        End Get
        Set(ByVal Value As System.String)
            _AccountNumber = Value
        End Set
    End Property
    Public Property AgentName() As System.String
        Get
            Return _AgentName
        End Get
        Set(ByVal Value As System.String)
            _AgentName = Value
        End Set
    End Property
    Public Property DepartmentCode() As System.String
        Get
            Return _DepartmentCode
        End Get
        Set(ByVal Value As System.String)
            _DepartmentCode = Value
        End Set
    End Property
    Public Property LocationCode() As System.String
        Get
            Return _LocationCode
        End Get
        Set(ByVal Value As System.String)
            _LocationCode = Value
        End Set
    End Property
    Public Property EmailPassword() As System.String
        Get
            Return _EmailPassword
        End Get
        Set(ByVal Value As System.String)
            _EmailPassword = Value
        End Set
    End Property
    Public Property NextReminderDate() As System.DateTime
        Get
            Return _NextReminderDate
        End Get
        Set(ByVal Value As System.DateTime)
            _NextReminderDate = Value
        End Set
    End Property
    Public Property CollectedAmt() As System.Double
        Get
            Return _CollectedAmt
        End Get
        Set(ByVal Value As System.Double)
            _CollectedAmt = Value
        End Set
    End Property
    Public Property TransTotalAmt() As System.Double
        Get
            Return _TransTotalAmt
        End Get
        Set(ByVal Value As System.Double)
            _TransTotalAmt = Value
        End Set
    End Property
    Public Property Currency() As System.String
        Get
            Return _Currency
        End Get
        Set(ByVal Value As System.String)
            _Currency = Value
        End Set
    End Property
    Public Property BalanceDue() As System.Double
        Get
            Return _BalanceDue
        End Get
        Set(ByVal Value As System.Double)
            _BalanceDue = Value
        End Set
    End Property
    Public Property PaymentAmt() As System.Double
        Get
            Return _PaymentAmt
        End Get
        Set(ByVal Value As System.Double)
            _PaymentAmt = Value
        End Set
    End Property
    Public Property AgentID() As System.String
        Get
            Return _AgentID
        End Get
        Set(ByVal Value As System.String)
            _AgentID = Value
        End Set
    End Property
    Public Property NextPaymentAmt() As System.Double
        Get
            Return _NextPaymentAmt
        End Get
        Set(ByVal Value As System.Double)
            _NextPaymentAmt = Value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal MsgID As System.String, ByVal EmailAddress As System.String, ByVal Subject As System.String, ByVal Message As System.String, ByVal ContactName As System.String, ByVal Status As System.Byte, ByVal ModemID As System.String, ByVal SimNo As System.String)

        'Me.EmailAddress = EmailAddress
        'Me.Subject = Subject
        'Me.Message = Message
        'Me.ContactName = ContactName
        'Me.Status = Status
        'Me.ModemID = ModemID
        'Me.SimNo = SimNo
        'Me.CreateDate = CreateDate
        'Me.CreateBy = CreateBy
        'Me.ReceivedDate = ReceivedDate
        'Me.DeletedDate = DeletedDate
        'Me.SentDate = SentDate
        'Me.IsDeleted = IsDeleted
        'Me.Flag = Flag
        'Me.Active = Active
        'Me.Inuse = Inuse

    End Sub

#End Region
End Class



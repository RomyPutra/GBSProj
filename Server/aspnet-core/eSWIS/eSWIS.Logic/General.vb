Imports Sharpbrake.Client

Public Class General
#Region "AirBrake"
    Public Class SystemLog
        Protected _Notifier As AirbrakeNotifier

        ''' <summary>
        ''' Mandatory
        ''' </summary>
        Public Property Notifier As AirbrakeNotifier
            Get
                Return _Notifier
            End Get
            Set(ByVal Value As AirbrakeNotifier)
                _Notifier = Value
            End Set
        End Property

        Public Sub New()
            '_connectionString = ConfigurationManager.ConnectionStrings("conn").ConnectionString
            '_BranchID = CStr(ConfigurationManager.AppSettings("BranchID"))
            '_TermID = CInt(ConfigurationManager.AppSettings("TermID"))
            Config()
        End Sub

        Public Overloads Sub Config()
            Dim settings As Generic.Dictionary(Of String, String) = Nothing
            settings.Add("Airbrake.ProjectId", "144184")
            settings.Add("Airbrake.ProjectKey", "e13263c7568a2142e0ef46a3f9ead6d3")
            settings.Add("Airbrake.Environment", "Production")
            settings.Add("Airbrake.AppVersion", "1.0.1")
            Dim airbrakeConfiguration = AirbrakeConfig.Load(settings)
            Notifier = New AirbrakeNotifier(airbrakeConfiguration)
        End Sub
    End Class

#End Region
End Class

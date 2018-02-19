Imports System

'Namespace Sharpbrake.Client
Public Class AirbrakeNotifier
        'Public Sub New(config As AirbrakeConfig, Optional logger As ILogger = Nothing, Optional httpRequestHandler As IHttpRequestHandler = Nothing)

        'End Sub

        'Public Event NotifyCompleted As NotifyCompletedEventHandler

        'Public Sub AddFilter(filter As Func(Of Notice, Notice))

        'End Sub
        'Public Sub Notify(exception As Exception, Optional context As IHttpContext = Nothing, Optional severity As Severity = Severity.Error)

        'End Sub
        'Public Sub NotifyAsync(exception As Exception, Optional context As IHttpContext = Nothing, Optional severity As Severity = Severity.Error)

        'End Sub
        'Protected Overridable Sub OnNotifyCompleted(eventArgs As NotifyCompletedEventArgs)

        'End Sub
        Public Sub New(config As AirbrakeConfig)

        End Sub

        'Public Event NotifyCompleted As NotifyCompletedEventHandler

        'Public Sub AddFilter(filter As Func(Of Notice, Notice))

        'End Sub
        Public Sub Notify(exception As Exception)

        End Sub
        Public Sub NotifyAsync(exception As Exception)

        End Sub
        'Protected Overridable Sub OnNotifyCompleted(eventArgs As NotifyCompletedEventArgs)

        'End Sub
    End Class

    Public Class AirbrakeConfig
    Public Sub New()

    End Sub

    Public Property Environment As String
        Public Property AppVersion As String
        Public Property ProjectKey As String
        Public Property ProjectId As String
        Public Property Host As String
        Public Property LogFile As String
        Public Property ProxyUri As String
        Public Property ProxyUsername As String
        Public Property ProxyPassword As String
        Public Property IgnoreEnvironments As IList(Of String)
        Public Property WhitelistKeys As IList(Of String)
        Public Property BlacklistKeys As IList(Of String)

        Public Shared Function Load(settings As IDictionary(Of String, String)) As AirbrakeConfig
            Return New AirbrakeConfig()
        End Function
    End Class

'Public Class NotifyCompletedEventArgs
'    Inherits AsyncCompletedEventArgs

'    Public Sub New(response As AirbrakeResponse, [error] As Exception, cancelled As Boolean, state As Object)

'    Public ReadOnly Property Result As AirbrakeResponse
'End Class
'End Namespace

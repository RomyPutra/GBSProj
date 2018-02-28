﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     //
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NavitaireSessionManger
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LogonRequestData", Namespace="http://schemas.navitaire.com/WebServices/DataContracts/Session")]
    public partial class LogonRequestData : object
    {
        
        private string DomainCodeField;
        
        private string AgentNameField;
        
        private string PasswordField;
        
        private string LocationCodeField;
        
        private string RoleCodeField;
        
        private string TerminalInfoField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DomainCode
        {
            get
            {
                return this.DomainCodeField;
            }
            set
            {
                this.DomainCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public string AgentName
        {
            get
            {
                return this.AgentNameField;
            }
            set
            {
                this.AgentNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public string Password
        {
            get
            {
                return this.PasswordField;
            }
            set
            {
                this.PasswordField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public string LocationCode
        {
            get
            {
                return this.LocationCodeField;
            }
            set
            {
                this.LocationCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=4)]
        public string RoleCode
        {
            get
            {
                return this.RoleCodeField;
            }
            set
            {
                this.RoleCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=5)]
        public string TerminalInfo
        {
            get
            {
                return this.TerminalInfoField;
            }
            set
            {
                this.TerminalInfoField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TokenRequest", Namespace="http://schemas.navitaire.com/WebServices/DataContracts/Session")]
    public partial class TokenRequest : object
    {
        
        private string TokenField;
        
        private string TerminalInfoField;
        
        private NavitaireSessionManger.ChannelType ChannelTypeField;
        
        private NavitaireSessionManger.SystemType SystemTypeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Token
        {
            get
            {
                return this.TokenField;
            }
            set
            {
                this.TokenField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public string TerminalInfo
        {
            get
            {
                return this.TerminalInfoField;
            }
            set
            {
                this.TerminalInfoField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public NavitaireSessionManger.ChannelType ChannelType
        {
            get
            {
                return this.ChannelTypeField;
            }
            set
            {
                this.ChannelTypeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public NavitaireSessionManger.SystemType SystemType
        {
            get
            {
                return this.SystemTypeField;
            }
            set
            {
                this.SystemTypeField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ChannelType", Namespace="http://schemas.navitaire.com/WebServices/DataContracts/Common/Enumerations")]
    public enum ChannelType : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Default = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Direct = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Web = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        GDS = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        API = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Unmapped = -1,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SystemType", Namespace="http://schemas.navitaire.com/WebServices/DataContracts/Common/Enumerations")]
    public enum SystemType : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Default = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        WinRez = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FareManager = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ScheduleManager = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        WinManager = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ConsoleRez = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        WebRez = 6,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        WebServicesAPI = 7,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        WebServicesESC = 8,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        InternalService = 9,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        WebReporting = 10,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        TaxAndFeeManager = 11,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DCS = 12,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Unmapped = -1,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TransferSessionResponseData", Namespace="http://schemas.navitaire.com/WebServices/DataContracts/Session")]
    public partial class TransferSessionResponseData : object
    {
        
        private string SignatureField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Signature
        {
            get
            {
                return this.SignatureField;
            }
            set
            {
                this.SignatureField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://schemas.navitaire.com/WebServices", ConfigurationName="NavitaireSessionManger.ISessionManager")]
    public interface ISessionManager
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.navitaire.com/WebServices/ISessionManager/ChangePassword", ReplyAction="http://schemas.navitaire.com/WebServices/ISessionManager/ChangePasswordResponse")]
        System.Threading.Tasks.Task<NavitaireSessionManger.ChangePasswordResponse> ChangePasswordAsync(NavitaireSessionManger.ChangePasswordRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.navitaire.com/WebServices/ISessionManager/Logon", ReplyAction="http://schemas.navitaire.com/WebServices/ISessionManager/LogonResponse")]
        System.Threading.Tasks.Task<NavitaireSessionManger.LogonResponse> LogonAsync(NavitaireSessionManger.LogonRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.navitaire.com/WebServices/ISessionManager/Logout", ReplyAction="http://schemas.navitaire.com/WebServices/ISessionManager/LogoutResponse")]
        System.Threading.Tasks.Task<NavitaireSessionManger.LogoutResponse> LogoutAsync(NavitaireSessionManger.LogoutRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.navitaire.com/WebServices/ISessionManager/TransferSession", ReplyAction="http://schemas.navitaire.com/WebServices/ISessionManager/TransferSessionResponse")]
        System.Threading.Tasks.Task<NavitaireSessionManger.TransferSessionResponse> TransferSessionAsync(NavitaireSessionManger.TransferSessionRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.navitaire.com/WebServices/ISessionManager/KeepAlive", ReplyAction="http://schemas.navitaire.com/WebServices/ISessionManager/KeepAliveResponse")]
        System.Threading.Tasks.Task<NavitaireSessionManger.KeepAliveResponse> KeepAliveAsync(NavitaireSessionManger.KeepAliveRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ChangePasswordRequest", WrapperNamespace="http://schemas.navitaire.com/WebServices/ServiceContracts/SessionService", IsWrapped=true)]
    public partial class ChangePasswordRequest
    {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://schemas.navitaire.com/WebServices")]
        public int ContractVersion;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://schemas.navitaire.com/WebServices/ServiceContracts/SessionService", Order=0)]
        public NavitaireSessionManger.LogonRequestData logonRequestData;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://schemas.navitaire.com/WebServices/ServiceContracts/SessionService", Order=1)]
        public string newPassword;
        
        public ChangePasswordRequest()
        {
        }
        
        public ChangePasswordRequest(int ContractVersion, NavitaireSessionManger.LogonRequestData logonRequestData, string newPassword)
        {
            this.ContractVersion = ContractVersion;
            this.logonRequestData = logonRequestData;
            this.newPassword = newPassword;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ChangePasswordResponse
    {
        
        public ChangePasswordResponse()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="LogonRequest", WrapperNamespace="http://schemas.navitaire.com/WebServices/ServiceContracts/SessionService", IsWrapped=true)]
    public partial class LogonRequest
    {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://schemas.navitaire.com/WebServices")]
        public int ContractVersion;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://schemas.navitaire.com/WebServices/ServiceContracts/SessionService", Order=0)]
        public NavitaireSessionManger.LogonRequestData logonRequestData;
        
        public LogonRequest()
        {
        }
        
        public LogonRequest(int ContractVersion, NavitaireSessionManger.LogonRequestData logonRequestData)
        {
            this.ContractVersion = ContractVersion;
            this.logonRequestData = logonRequestData;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="LogonResponse", WrapperNamespace="http://schemas.navitaire.com/WebServices", IsWrapped=true)]
    public partial class LogonResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://schemas.navitaire.com/WebServices", Order=0)]
        public string Signature;
        
        public LogonResponse()
        {
        }
        
        public LogonResponse(string Signature)
        {
            this.Signature = Signature;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="LogoutRequest", WrapperNamespace="http://schemas.navitaire.com/WebServices/ServiceContracts/SessionService", IsWrapped=true)]
    public partial class LogoutRequest
    {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://schemas.navitaire.com/WebServices")]
        public int ContractVersion;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://schemas.navitaire.com/WebServices")]
        public string Signature;
        
        public LogoutRequest()
        {
        }
        
        public LogoutRequest(int ContractVersion, string Signature)
        {
            this.ContractVersion = ContractVersion;
            this.Signature = Signature;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class LogoutResponse
    {
        
        public LogoutResponse()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="TransferSessionRequest", WrapperNamespace="http://schemas.navitaire.com/WebServices/ServiceContracts/SessionService", IsWrapped=true)]
    public partial class TransferSessionRequest
    {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://schemas.navitaire.com/WebServices")]
        public int ContractVersion;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://schemas.navitaire.com/WebServices/ServiceContracts/SessionService", Order=0)]
        public NavitaireSessionManger.TokenRequest tokenRequest;
        
        public TransferSessionRequest()
        {
        }
        
        public TransferSessionRequest(int ContractVersion, NavitaireSessionManger.TokenRequest tokenRequest)
        {
            this.ContractVersion = ContractVersion;
            this.tokenRequest = tokenRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="TransferSessionResponse", WrapperNamespace="http://schemas.navitaire.com/WebServices/ServiceContracts/SessionService", IsWrapped=true)]
    public partial class TransferSessionResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://schemas.navitaire.com/WebServices/ServiceContracts/SessionService", Order=0)]
        public NavitaireSessionManger.TransferSessionResponseData TransferSessionResponseData;
        
        public TransferSessionResponse()
        {
        }
        
        public TransferSessionResponse(NavitaireSessionManger.TransferSessionResponseData TransferSessionResponseData)
        {
            this.TransferSessionResponseData = TransferSessionResponseData;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="KeepAliveRequest", WrapperNamespace="http://schemas.navitaire.com/WebServices/ServiceContracts/SessionService", IsWrapped=true)]
    public partial class KeepAliveRequest
    {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://schemas.navitaire.com/WebServices")]
        public int ContractVersion;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://schemas.navitaire.com/WebServices")]
        public string Signature;
        
        public KeepAliveRequest()
        {
        }
        
        public KeepAliveRequest(int ContractVersion, string Signature)
        {
            this.ContractVersion = ContractVersion;
            this.Signature = Signature;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class KeepAliveResponse
    {
        
        public KeepAliveResponse()
        {
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public interface ISessionManagerChannel : NavitaireSessionManger.ISessionManager, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public partial class SessionManagerClient : System.ServiceModel.ClientBase<NavitaireSessionManger.ISessionManager>, NavitaireSessionManger.ISessionManager
    {
        
    /// <summary>
    /// Implement this partial method to configure the service endpoint.
    /// </summary>
    /// <param name="serviceEndpoint">The endpoint to configure</param>
    /// <param name="clientCredentials">The client credentials</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public SessionManagerClient() : 
                base(SessionManagerClient.GetDefaultBinding(), SessionManagerClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_ISessionManager.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SessionManagerClient(EndpointConfiguration endpointConfiguration) : 
                base(SessionManagerClient.GetBindingForEndpoint(endpointConfiguration), SessionManagerClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SessionManagerClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(SessionManagerClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SessionManagerClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(SessionManagerClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SessionManagerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<NavitaireSessionManger.ChangePasswordResponse> NavitaireSessionManger.ISessionManager.ChangePasswordAsync(NavitaireSessionManger.ChangePasswordRequest request)
        {
            return base.Channel.ChangePasswordAsync(request);
        }
        
        public System.Threading.Tasks.Task<NavitaireSessionManger.ChangePasswordResponse> ChangePasswordAsync(int ContractVersion, NavitaireSessionManger.LogonRequestData logonRequestData, string newPassword)
        {
            NavitaireSessionManger.ChangePasswordRequest inValue = new NavitaireSessionManger.ChangePasswordRequest();
            inValue.ContractVersion = ContractVersion;
            inValue.logonRequestData = logonRequestData;
            inValue.newPassword = newPassword;
            return ((NavitaireSessionManger.ISessionManager)(this)).ChangePasswordAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<NavitaireSessionManger.LogonResponse> NavitaireSessionManger.ISessionManager.LogonAsync(NavitaireSessionManger.LogonRequest request)
        {
            return base.Channel.LogonAsync(request);
        }
        
        public System.Threading.Tasks.Task<NavitaireSessionManger.LogonResponse> LogonAsync(int ContractVersion, NavitaireSessionManger.LogonRequestData logonRequestData)
        {
            NavitaireSessionManger.LogonRequest inValue = new NavitaireSessionManger.LogonRequest();
            inValue.ContractVersion = ContractVersion;
            inValue.logonRequestData = logonRequestData;
            return ((NavitaireSessionManger.ISessionManager)(this)).LogonAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<NavitaireSessionManger.LogoutResponse> NavitaireSessionManger.ISessionManager.LogoutAsync(NavitaireSessionManger.LogoutRequest request)
        {
            return base.Channel.LogoutAsync(request);
        }
        
        public System.Threading.Tasks.Task<NavitaireSessionManger.LogoutResponse> LogoutAsync(int ContractVersion, string Signature)
        {
            NavitaireSessionManger.LogoutRequest inValue = new NavitaireSessionManger.LogoutRequest();
            inValue.ContractVersion = ContractVersion;
            inValue.Signature = Signature;
            return ((NavitaireSessionManger.ISessionManager)(this)).LogoutAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<NavitaireSessionManger.TransferSessionResponse> NavitaireSessionManger.ISessionManager.TransferSessionAsync(NavitaireSessionManger.TransferSessionRequest request)
        {
            return base.Channel.TransferSessionAsync(request);
        }
        
        public System.Threading.Tasks.Task<NavitaireSessionManger.TransferSessionResponse> TransferSessionAsync(int ContractVersion, NavitaireSessionManger.TokenRequest tokenRequest)
        {
            NavitaireSessionManger.TransferSessionRequest inValue = new NavitaireSessionManger.TransferSessionRequest();
            inValue.ContractVersion = ContractVersion;
            inValue.tokenRequest = tokenRequest;
            return ((NavitaireSessionManger.ISessionManager)(this)).TransferSessionAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<NavitaireSessionManger.KeepAliveResponse> NavitaireSessionManger.ISessionManager.KeepAliveAsync(NavitaireSessionManger.KeepAliveRequest request)
        {
            return base.Channel.KeepAliveAsync(request);
        }
        
        public System.Threading.Tasks.Task<NavitaireSessionManger.KeepAliveResponse> KeepAliveAsync(int ContractVersion, string Signature)
        {
            NavitaireSessionManger.KeepAliveRequest inValue = new NavitaireSessionManger.KeepAliveRequest();
            inValue.ContractVersion = ContractVersion;
            inValue.Signature = Signature;
            return ((NavitaireSessionManger.ISessionManager)(this)).KeepAliveAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_ISessionManager))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                result.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_ISessionManager))
            {
                return new System.ServiceModel.EndpointAddress("https://aktest2r3xapi.navitaire.com/SessionManager.svc");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return SessionManagerClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_ISessionManager);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return SessionManagerClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_ISessionManager);
        }
        
        public enum EndpointConfiguration
        {
            
            BasicHttpBinding_ISessionManager,
        }
    }
}
﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BeamsCheckBalance
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://balance.bds", ConfigurationName="BeamsCheckBalance.GetBalancePortType")]
    public interface GetBalancePortType
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:getBalance", ReplyAction="urn:getBalanceResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<BeamsCheckBalance.getBalanceResponse> getBalanceAsync(BeamsCheckBalance.getBalanceRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://balance.bds/xsd")]
    public partial class BalanceData
    {
        
        private string currMonthBalanceField;
        
        private string currMonthBudgetField;
        
        private string currMonthExpField;
        
        private string currTimeStampField;
        
        private string distributedFlagField;
        
        private string negativeExpField;
        
        private string stateBalanceField;
        
        private string stateBudgetField;
        
        private string stateExpField;
        
        private string statusCodeField;
        
        private string totalBalanceField;
        
        private string totalBudgetField;
        
        private string totalExpField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public string currMonthBalance
        {
            get
            {
                return this.currMonthBalanceField;
            }
            set
            {
                this.currMonthBalanceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=1)]
        public string currMonthBudget
        {
            get
            {
                return this.currMonthBudgetField;
            }
            set
            {
                this.currMonthBudgetField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=2)]
        public string currMonthExp
        {
            get
            {
                return this.currMonthExpField;
            }
            set
            {
                this.currMonthExpField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=3)]
        public string currTimeStamp
        {
            get
            {
                return this.currTimeStampField;
            }
            set
            {
                this.currTimeStampField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=4)]
        public string distributedFlag
        {
            get
            {
                return this.distributedFlagField;
            }
            set
            {
                this.distributedFlagField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=5)]
        public string negativeExp
        {
            get
            {
                return this.negativeExpField;
            }
            set
            {
                this.negativeExpField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=6)]
        public string stateBalance
        {
            get
            {
                return this.stateBalanceField;
            }
            set
            {
                this.stateBalanceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=7)]
        public string stateBudget
        {
            get
            {
                return this.stateBudgetField;
            }
            set
            {
                this.stateBudgetField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=8)]
        public string stateExp
        {
            get
            {
                return this.stateExpField;
            }
            set
            {
                this.stateExpField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=9)]
        public string statusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=10)]
        public string totalBalance
        {
            get
            {
                return this.totalBalanceField;
            }
            set
            {
                this.totalBalanceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=11)]
        public string totalBudget
        {
            get
            {
                return this.totalBudgetField;
            }
            set
            {
                this.totalBudgetField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=12)]
        public string totalExp
        {
            get
            {
                return this.totalExpField;
            }
            set
            {
                this.totalExpField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getBalance", WrapperNamespace="http://balance.bds", IsWrapped=true)]
    public partial class getBalanceRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://balance.bds", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("params", IsNullable=true)]
        public string[] @params;
        
        public getBalanceRequest()
        {
        }
        
        public getBalanceRequest(string[] @params)
        {
            this.@params = @params;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getBalanceResponse", WrapperNamespace="http://balance.bds", IsWrapped=true)]
    public partial class getBalanceResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://balance.bds", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public BeamsCheckBalance.BalanceData @return;
        
        public getBalanceResponse()
        {
        }
        
        public getBalanceResponse(BeamsCheckBalance.BalanceData @return)
        {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    public interface GetBalancePortTypeChannel : BeamsCheckBalance.GetBalancePortType, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    public partial class GetBalancePortTypeClient : System.ServiceModel.ClientBase<BeamsCheckBalance.GetBalancePortType>, BeamsCheckBalance.GetBalancePortType
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public GetBalancePortTypeClient(EndpointConfiguration endpointConfiguration) : 
                base(GetBalancePortTypeClient.GetBindingForEndpoint(endpointConfiguration), GetBalancePortTypeClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public GetBalancePortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(GetBalancePortTypeClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public GetBalancePortTypeClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(GetBalancePortTypeClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public GetBalancePortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<BeamsCheckBalance.getBalanceResponse> BeamsCheckBalance.GetBalancePortType.getBalanceAsync(BeamsCheckBalance.getBalanceRequest request)
        {
            return base.Channel.getBalanceAsync(request);
        }
        
        public System.Threading.Tasks.Task<BeamsCheckBalance.getBalanceResponse> getBalanceAsync(string[] @params)
        {
            BeamsCheckBalance.getBalanceRequest inValue = new BeamsCheckBalance.getBalanceRequest();
            inValue.@params = @params;
            return ((BeamsCheckBalance.GetBalancePortType)(this)).getBalanceAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.GetBalanceHttpSoap11Endpoint))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.GetBalanceHttpsSoap11Endpoint))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.GetBalanceHttpsSoap12Endpoint))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.GetBalanceHttpSoap12Endpoint))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.GetBalanceHttpSoap11Endpoint))
            {
                return new System.ServiceModel.EndpointAddress("http://testbeams.mahaitgov.in:8080/BeamsWS1/services/GetBalance.GetBalanceHttpSoa" +
                        "p11Endpoint/");
            }
            if ((endpointConfiguration == EndpointConfiguration.GetBalanceHttpsSoap11Endpoint))
            {
                return new System.ServiceModel.EndpointAddress("http://testbeams.mahaitgov.in:8443/BeamsWS1/services/GetBalance.GetBalanceHttpsSo" +
                        "ap11Endpoint/");
            }
            if ((endpointConfiguration == EndpointConfiguration.GetBalanceHttpsSoap12Endpoint))
            {
                return new System.ServiceModel.EndpointAddress("http://testbeams.mahaitgov.in:8443/BeamsWS1/services/GetBalance.GetBalanceHttpsSo" +
                        "ap12Endpoint/");
            }
            if ((endpointConfiguration == EndpointConfiguration.GetBalanceHttpSoap12Endpoint))
            {
                return new System.ServiceModel.EndpointAddress("http://testbeams.mahaitgov.in:8080/BeamsWS1/services/GetBalance.GetBalanceHttpSoa" +
                        "p12Endpoint/");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            GetBalanceHttpSoap11Endpoint,
            
            GetBalanceHttpsSoap11Endpoint,
            
            GetBalanceHttpsSoap12Endpoint,
            
            GetBalanceHttpSoap12Endpoint,
        }
    }
}

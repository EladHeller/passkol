﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model.PDFService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PDFService.PDFServiceSoap")]
    public interface PDFServiceSoap {
        
        // CODEGEN: Generating message contract since element name UserId from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/createPDF", ReplyAction="*")]
        Model.PDFService.createPDFResponse createPDF(Model.PDFService.createPDFRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/createPDF", ReplyAction="*")]
        System.Threading.Tasks.Task<Model.PDFService.createPDFResponse> createPDFAsync(Model.PDFService.createPDFRequest request);
        
        // CODEGEN: Generating message contract since element name name from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ArtistPermissionAgreement", ReplyAction="*")]
        Model.PDFService.ArtistPermissionAgreementResponse ArtistPermissionAgreement(Model.PDFService.ArtistPermissionAgreementRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ArtistPermissionAgreement", ReplyAction="*")]
        System.Threading.Tasks.Task<Model.PDFService.ArtistPermissionAgreementResponse> ArtistPermissionAgreementAsync(Model.PDFService.ArtistPermissionAgreementRequest request);
        
        // CODEGEN: Generating message contract since element name MusicName from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ArtistAddMusic", ReplyAction="*")]
        Model.PDFService.ArtistAddMusicResponse ArtistAddMusic(Model.PDFService.ArtistAddMusicRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ArtistAddMusic", ReplyAction="*")]
        System.Threading.Tasks.Task<Model.PDFService.ArtistAddMusicResponse> ArtistAddMusicAsync(Model.PDFService.ArtistAddMusicRequest request);
        
        // CODEGEN: Generating message contract since element name customerName from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetTempPurchaseHtml", ReplyAction="*")]
        Model.PDFService.GetTempPurchaseHtmlResponse GetTempPurchaseHtml(Model.PDFService.GetTempPurchaseHtmlRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetTempPurchaseHtml", ReplyAction="*")]
        System.Threading.Tasks.Task<Model.PDFService.GetTempPurchaseHtmlResponse> GetTempPurchaseHtmlAsync(Model.PDFService.GetTempPurchaseHtmlRequest request);
        
        // CODEGEN: Generating message contract since element name BaseRoute from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/PurchaseAgreement", ReplyAction="*")]
        Model.PDFService.PurchaseAgreementResponse PurchaseAgreement(Model.PDFService.PurchaseAgreementRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/PurchaseAgreement", ReplyAction="*")]
        System.Threading.Tasks.Task<Model.PDFService.PurchaseAgreementResponse> PurchaseAgreementAsync(Model.PDFService.PurchaseAgreementRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class createPDFRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="createPDF", Namespace="http://tempuri.org/", Order=0)]
        public Model.PDFService.createPDFRequestBody Body;
        
        public createPDFRequest() {
        }
        
        public createPDFRequest(Model.PDFService.createPDFRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class createPDFRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string UserId;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string BaseRoute;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string text;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string fileName;
        
        public createPDFRequestBody() {
        }
        
        public createPDFRequestBody(string UserId, string BaseRoute, string text, string fileName) {
            this.UserId = UserId;
            this.BaseRoute = BaseRoute;
            this.text = text;
            this.fileName = fileName;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class createPDFResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="createPDFResponse", Namespace="http://tempuri.org/", Order=0)]
        public Model.PDFService.createPDFResponseBody Body;
        
        public createPDFResponse() {
        }
        
        public createPDFResponse(Model.PDFService.createPDFResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class createPDFResponseBody {
        
        public createPDFResponseBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ArtistPermissionAgreementRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ArtistPermissionAgreement", Namespace="http://tempuri.org/", Order=0)]
        public Model.PDFService.ArtistPermissionAgreementRequestBody Body;
        
        public ArtistPermissionAgreementRequest() {
        }
        
        public ArtistPermissionAgreementRequest(Model.PDFService.ArtistPermissionAgreementRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class ArtistPermissionAgreementRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public System.DateTime Time;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string name;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string email;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string UserId;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string BaseRoute;
        
        public ArtistPermissionAgreementRequestBody() {
        }
        
        public ArtistPermissionAgreementRequestBody(System.DateTime Time, string name, string email, string UserId, string BaseRoute) {
            this.Time = Time;
            this.name = name;
            this.email = email;
            this.UserId = UserId;
            this.BaseRoute = BaseRoute;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ArtistPermissionAgreementResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ArtistPermissionAgreementResponse", Namespace="http://tempuri.org/", Order=0)]
        public Model.PDFService.ArtistPermissionAgreementResponseBody Body;
        
        public ArtistPermissionAgreementResponse() {
        }
        
        public ArtistPermissionAgreementResponse(Model.PDFService.ArtistPermissionAgreementResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class ArtistPermissionAgreementResponseBody {
        
        public ArtistPermissionAgreementResponseBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ArtistAddMusicRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ArtistAddMusic", Namespace="http://tempuri.org/", Order=0)]
        public Model.PDFService.ArtistAddMusicRequestBody Body;
        
        public ArtistAddMusicRequest() {
        }
        
        public ArtistAddMusicRequest(Model.PDFService.ArtistAddMusicRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class ArtistAddMusicRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public System.DateTime Time;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string MusicName;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string Composer;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string Singer;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string Exeptions;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string Writer;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string UserId;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string BaseRoute;
        
        public ArtistAddMusicRequestBody() {
        }
        
        public ArtistAddMusicRequestBody(System.DateTime Time, string MusicName, string Composer, string Singer, string Exeptions, string Writer, string UserId, string BaseRoute) {
            this.Time = Time;
            this.MusicName = MusicName;
            this.Composer = Composer;
            this.Singer = Singer;
            this.Exeptions = Exeptions;
            this.Writer = Writer;
            this.UserId = UserId;
            this.BaseRoute = BaseRoute;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ArtistAddMusicResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ArtistAddMusicResponse", Namespace="http://tempuri.org/", Order=0)]
        public Model.PDFService.ArtistAddMusicResponseBody Body;
        
        public ArtistAddMusicResponse() {
        }
        
        public ArtistAddMusicResponse(Model.PDFService.ArtistAddMusicResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class ArtistAddMusicResponseBody {
        
        public ArtistAddMusicResponseBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetTempPurchaseHtmlRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetTempPurchaseHtml", Namespace="http://tempuri.org/", Order=0)]
        public Model.PDFService.GetTempPurchaseHtmlRequestBody Body;
        
        public GetTempPurchaseHtmlRequest() {
        }
        
        public GetTempPurchaseHtmlRequest(Model.PDFService.GetTempPurchaseHtmlRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetTempPurchaseHtmlRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public System.DateTime time;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string customerName;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string email;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string musicName;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string musicWriter;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string musicComposer;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string musicPerformer;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string permission;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=8)]
        public double cost;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=9)]
        public string reference;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=10)]
        public string exceptions;
        
        public GetTempPurchaseHtmlRequestBody() {
        }
        
        public GetTempPurchaseHtmlRequestBody(System.DateTime time, string customerName, string email, string musicName, string musicWriter, string musicComposer, string musicPerformer, string permission, double cost, string reference, string exceptions) {
            this.time = time;
            this.customerName = customerName;
            this.email = email;
            this.musicName = musicName;
            this.musicWriter = musicWriter;
            this.musicComposer = musicComposer;
            this.musicPerformer = musicPerformer;
            this.permission = permission;
            this.cost = cost;
            this.reference = reference;
            this.exceptions = exceptions;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetTempPurchaseHtmlResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetTempPurchaseHtmlResponse", Namespace="http://tempuri.org/", Order=0)]
        public Model.PDFService.GetTempPurchaseHtmlResponseBody Body;
        
        public GetTempPurchaseHtmlResponse() {
        }
        
        public GetTempPurchaseHtmlResponse(Model.PDFService.GetTempPurchaseHtmlResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetTempPurchaseHtmlResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetTempPurchaseHtmlResult;
        
        public GetTempPurchaseHtmlResponseBody() {
        }
        
        public GetTempPurchaseHtmlResponseBody(string GetTempPurchaseHtmlResult) {
            this.GetTempPurchaseHtmlResult = GetTempPurchaseHtmlResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PurchaseAgreementRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PurchaseAgreement", Namespace="http://tempuri.org/", Order=0)]
        public Model.PDFService.PurchaseAgreementRequestBody Body;
        
        public PurchaseAgreementRequest() {
        }
        
        public PurchaseAgreementRequest(Model.PDFService.PurchaseAgreementRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class PurchaseAgreementRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string BaseRoute;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string FileId;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string UserId;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public System.DateTime time;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string customerName;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string email;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string musicName;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string musicWriter;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=8)]
        public string musicComposer;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=9)]
        public string musicPerformer;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=10)]
        public string permission;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=11)]
        public double cost;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=12)]
        public string reference;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=13)]
        public string exceptions;
        
        public PurchaseAgreementRequestBody() {
        }
        
        public PurchaseAgreementRequestBody(string BaseRoute, string FileId, string UserId, System.DateTime time, string customerName, string email, string musicName, string musicWriter, string musicComposer, string musicPerformer, string permission, double cost, string reference, string exceptions) {
            this.BaseRoute = BaseRoute;
            this.FileId = FileId;
            this.UserId = UserId;
            this.time = time;
            this.customerName = customerName;
            this.email = email;
            this.musicName = musicName;
            this.musicWriter = musicWriter;
            this.musicComposer = musicComposer;
            this.musicPerformer = musicPerformer;
            this.permission = permission;
            this.cost = cost;
            this.reference = reference;
            this.exceptions = exceptions;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PurchaseAgreementResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PurchaseAgreementResponse", Namespace="http://tempuri.org/", Order=0)]
        public Model.PDFService.PurchaseAgreementResponseBody Body;
        
        public PurchaseAgreementResponse() {
        }
        
        public PurchaseAgreementResponse(Model.PDFService.PurchaseAgreementResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class PurchaseAgreementResponseBody {
        
        public PurchaseAgreementResponseBody() {
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface PDFServiceSoapChannel : Model.PDFService.PDFServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PDFServiceSoapClient : System.ServiceModel.ClientBase<Model.PDFService.PDFServiceSoap>, Model.PDFService.PDFServiceSoap {
        
        public PDFServiceSoapClient() {
        }
        
        public PDFServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PDFServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PDFServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PDFServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Model.PDFService.createPDFResponse Model.PDFService.PDFServiceSoap.createPDF(Model.PDFService.createPDFRequest request) {
            return base.Channel.createPDF(request);
        }
        
        public void createPDF(string UserId, string BaseRoute, string text, string fileName) {
            Model.PDFService.createPDFRequest inValue = new Model.PDFService.createPDFRequest();
            inValue.Body = new Model.PDFService.createPDFRequestBody();
            inValue.Body.UserId = UserId;
            inValue.Body.BaseRoute = BaseRoute;
            inValue.Body.text = text;
            inValue.Body.fileName = fileName;
            Model.PDFService.createPDFResponse retVal = ((Model.PDFService.PDFServiceSoap)(this)).createPDF(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Model.PDFService.createPDFResponse> Model.PDFService.PDFServiceSoap.createPDFAsync(Model.PDFService.createPDFRequest request) {
            return base.Channel.createPDFAsync(request);
        }
        
        public System.Threading.Tasks.Task<Model.PDFService.createPDFResponse> createPDFAsync(string UserId, string BaseRoute, string text, string fileName) {
            Model.PDFService.createPDFRequest inValue = new Model.PDFService.createPDFRequest();
            inValue.Body = new Model.PDFService.createPDFRequestBody();
            inValue.Body.UserId = UserId;
            inValue.Body.BaseRoute = BaseRoute;
            inValue.Body.text = text;
            inValue.Body.fileName = fileName;
            return ((Model.PDFService.PDFServiceSoap)(this)).createPDFAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Model.PDFService.ArtistPermissionAgreementResponse Model.PDFService.PDFServiceSoap.ArtistPermissionAgreement(Model.PDFService.ArtistPermissionAgreementRequest request) {
            return base.Channel.ArtistPermissionAgreement(request);
        }
        
        public void ArtistPermissionAgreement(System.DateTime Time, string name, string email, string UserId, string BaseRoute) {
            Model.PDFService.ArtistPermissionAgreementRequest inValue = new Model.PDFService.ArtistPermissionAgreementRequest();
            inValue.Body = new Model.PDFService.ArtistPermissionAgreementRequestBody();
            inValue.Body.Time = Time;
            inValue.Body.name = name;
            inValue.Body.email = email;
            inValue.Body.UserId = UserId;
            inValue.Body.BaseRoute = BaseRoute;
            Model.PDFService.ArtistPermissionAgreementResponse retVal = ((Model.PDFService.PDFServiceSoap)(this)).ArtistPermissionAgreement(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Model.PDFService.ArtistPermissionAgreementResponse> Model.PDFService.PDFServiceSoap.ArtistPermissionAgreementAsync(Model.PDFService.ArtistPermissionAgreementRequest request) {
            return base.Channel.ArtistPermissionAgreementAsync(request);
        }
        
        public System.Threading.Tasks.Task<Model.PDFService.ArtistPermissionAgreementResponse> ArtistPermissionAgreementAsync(System.DateTime Time, string name, string email, string UserId, string BaseRoute) {
            Model.PDFService.ArtistPermissionAgreementRequest inValue = new Model.PDFService.ArtistPermissionAgreementRequest();
            inValue.Body = new Model.PDFService.ArtistPermissionAgreementRequestBody();
            inValue.Body.Time = Time;
            inValue.Body.name = name;
            inValue.Body.email = email;
            inValue.Body.UserId = UserId;
            inValue.Body.BaseRoute = BaseRoute;
            return ((Model.PDFService.PDFServiceSoap)(this)).ArtistPermissionAgreementAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Model.PDFService.ArtistAddMusicResponse Model.PDFService.PDFServiceSoap.ArtistAddMusic(Model.PDFService.ArtistAddMusicRequest request) {
            return base.Channel.ArtistAddMusic(request);
        }
        
        public void ArtistAddMusic(System.DateTime Time, string MusicName, string Composer, string Singer, string Exeptions, string Writer, string UserId, string BaseRoute) {
            Model.PDFService.ArtistAddMusicRequest inValue = new Model.PDFService.ArtistAddMusicRequest();
            inValue.Body = new Model.PDFService.ArtistAddMusicRequestBody();
            inValue.Body.Time = Time;
            inValue.Body.MusicName = MusicName;
            inValue.Body.Composer = Composer;
            inValue.Body.Singer = Singer;
            inValue.Body.Exeptions = Exeptions;
            inValue.Body.Writer = Writer;
            inValue.Body.UserId = UserId;
            inValue.Body.BaseRoute = BaseRoute;
            Model.PDFService.ArtistAddMusicResponse retVal = ((Model.PDFService.PDFServiceSoap)(this)).ArtistAddMusic(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Model.PDFService.ArtistAddMusicResponse> Model.PDFService.PDFServiceSoap.ArtistAddMusicAsync(Model.PDFService.ArtistAddMusicRequest request) {
            return base.Channel.ArtistAddMusicAsync(request);
        }
        
        public System.Threading.Tasks.Task<Model.PDFService.ArtistAddMusicResponse> ArtistAddMusicAsync(System.DateTime Time, string MusicName, string Composer, string Singer, string Exeptions, string Writer, string UserId, string BaseRoute) {
            Model.PDFService.ArtistAddMusicRequest inValue = new Model.PDFService.ArtistAddMusicRequest();
            inValue.Body = new Model.PDFService.ArtistAddMusicRequestBody();
            inValue.Body.Time = Time;
            inValue.Body.MusicName = MusicName;
            inValue.Body.Composer = Composer;
            inValue.Body.Singer = Singer;
            inValue.Body.Exeptions = Exeptions;
            inValue.Body.Writer = Writer;
            inValue.Body.UserId = UserId;
            inValue.Body.BaseRoute = BaseRoute;
            return ((Model.PDFService.PDFServiceSoap)(this)).ArtistAddMusicAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Model.PDFService.GetTempPurchaseHtmlResponse Model.PDFService.PDFServiceSoap.GetTempPurchaseHtml(Model.PDFService.GetTempPurchaseHtmlRequest request) {
            return base.Channel.GetTempPurchaseHtml(request);
        }
        
        public string GetTempPurchaseHtml(System.DateTime time, string customerName, string email, string musicName, string musicWriter, string musicComposer, string musicPerformer, string permission, double cost, string reference, string exceptions) {
            Model.PDFService.GetTempPurchaseHtmlRequest inValue = new Model.PDFService.GetTempPurchaseHtmlRequest();
            inValue.Body = new Model.PDFService.GetTempPurchaseHtmlRequestBody();
            inValue.Body.time = time;
            inValue.Body.customerName = customerName;
            inValue.Body.email = email;
            inValue.Body.musicName = musicName;
            inValue.Body.musicWriter = musicWriter;
            inValue.Body.musicComposer = musicComposer;
            inValue.Body.musicPerformer = musicPerformer;
            inValue.Body.permission = permission;
            inValue.Body.cost = cost;
            inValue.Body.reference = reference;
            inValue.Body.exceptions = exceptions;
            Model.PDFService.GetTempPurchaseHtmlResponse retVal = ((Model.PDFService.PDFServiceSoap)(this)).GetTempPurchaseHtml(inValue);
            return retVal.Body.GetTempPurchaseHtmlResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Model.PDFService.GetTempPurchaseHtmlResponse> Model.PDFService.PDFServiceSoap.GetTempPurchaseHtmlAsync(Model.PDFService.GetTempPurchaseHtmlRequest request) {
            return base.Channel.GetTempPurchaseHtmlAsync(request);
        }
        
        public System.Threading.Tasks.Task<Model.PDFService.GetTempPurchaseHtmlResponse> GetTempPurchaseHtmlAsync(System.DateTime time, string customerName, string email, string musicName, string musicWriter, string musicComposer, string musicPerformer, string permission, double cost, string reference, string exceptions) {
            Model.PDFService.GetTempPurchaseHtmlRequest inValue = new Model.PDFService.GetTempPurchaseHtmlRequest();
            inValue.Body = new Model.PDFService.GetTempPurchaseHtmlRequestBody();
            inValue.Body.time = time;
            inValue.Body.customerName = customerName;
            inValue.Body.email = email;
            inValue.Body.musicName = musicName;
            inValue.Body.musicWriter = musicWriter;
            inValue.Body.musicComposer = musicComposer;
            inValue.Body.musicPerformer = musicPerformer;
            inValue.Body.permission = permission;
            inValue.Body.cost = cost;
            inValue.Body.reference = reference;
            inValue.Body.exceptions = exceptions;
            return ((Model.PDFService.PDFServiceSoap)(this)).GetTempPurchaseHtmlAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Model.PDFService.PurchaseAgreementResponse Model.PDFService.PDFServiceSoap.PurchaseAgreement(Model.PDFService.PurchaseAgreementRequest request) {
            return base.Channel.PurchaseAgreement(request);
        }
        
        public void PurchaseAgreement(string BaseRoute, string FileId, string UserId, System.DateTime time, string customerName, string email, string musicName, string musicWriter, string musicComposer, string musicPerformer, string permission, double cost, string reference, string exceptions) {
            Model.PDFService.PurchaseAgreementRequest inValue = new Model.PDFService.PurchaseAgreementRequest();
            inValue.Body = new Model.PDFService.PurchaseAgreementRequestBody();
            inValue.Body.BaseRoute = BaseRoute;
            inValue.Body.FileId = FileId;
            inValue.Body.UserId = UserId;
            inValue.Body.time = time;
            inValue.Body.customerName = customerName;
            inValue.Body.email = email;
            inValue.Body.musicName = musicName;
            inValue.Body.musicWriter = musicWriter;
            inValue.Body.musicComposer = musicComposer;
            inValue.Body.musicPerformer = musicPerformer;
            inValue.Body.permission = permission;
            inValue.Body.cost = cost;
            inValue.Body.reference = reference;
            inValue.Body.exceptions = exceptions;
            Model.PDFService.PurchaseAgreementResponse retVal = ((Model.PDFService.PDFServiceSoap)(this)).PurchaseAgreement(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Model.PDFService.PurchaseAgreementResponse> Model.PDFService.PDFServiceSoap.PurchaseAgreementAsync(Model.PDFService.PurchaseAgreementRequest request) {
            return base.Channel.PurchaseAgreementAsync(request);
        }
        
        public System.Threading.Tasks.Task<Model.PDFService.PurchaseAgreementResponse> PurchaseAgreementAsync(string BaseRoute, string FileId, string UserId, System.DateTime time, string customerName, string email, string musicName, string musicWriter, string musicComposer, string musicPerformer, string permission, double cost, string reference, string exceptions) {
            Model.PDFService.PurchaseAgreementRequest inValue = new Model.PDFService.PurchaseAgreementRequest();
            inValue.Body = new Model.PDFService.PurchaseAgreementRequestBody();
            inValue.Body.BaseRoute = BaseRoute;
            inValue.Body.FileId = FileId;
            inValue.Body.UserId = UserId;
            inValue.Body.time = time;
            inValue.Body.customerName = customerName;
            inValue.Body.email = email;
            inValue.Body.musicName = musicName;
            inValue.Body.musicWriter = musicWriter;
            inValue.Body.musicComposer = musicComposer;
            inValue.Body.musicPerformer = musicPerformer;
            inValue.Body.permission = permission;
            inValue.Body.cost = cost;
            inValue.Body.reference = reference;
            inValue.Body.exceptions = exceptions;
            return ((Model.PDFService.PDFServiceSoap)(this)).PurchaseAgreementAsync(inValue);
        }
    }
}

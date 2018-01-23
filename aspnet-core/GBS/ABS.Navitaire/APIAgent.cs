using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ABS.Navitaire.AgentManager;
using ABS.Navitaire.PersonManager;

namespace ABS.Navitaire
{
    public partial class APIAgent : APIBase
    {
        public APIAgent()
        {
            InitializeComponent();
        }

        public APIAgent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public FindAgentsResponse NewFindingAgent(string agentName, string SessionID)
        {//OK
            IAgentManager agentAPI = new AgentManagerClient();
            //Create a FindAgentsRequest and 
            // SearchString object to define search parameters
            FindAgentsRequest findAgentRequest = new FindAgentsRequest();
            FindAgentRequestData findData = new FindAgentRequestData();
            findData.AgentName = new Navitaire.AgentManager.SearchString();
            findData.AgentName.SearchType = Navitaire.AgentManager.SearchType.StartsWith;
            findData.DomainCode = "ext";
            findData.AgentName.Value = agentName;// "VANCELINFO_ADMIN";
            findData.Status = AgentStatus.Active;
            findData.ActiveOnly = true;
            findAgentRequest.FindAgentRequestData = findData;
            findAgentRequest.Signature = SessionID;// SessionManager._signature;
            //Find agents by calling FindAgents and passing the FindAgentsRequest as a parameter.
            FindAgentsResponse response = agentAPI.FindAgents(findAgentRequest);
            return response;
        }
        public Agent GetAgentByID(string agentName, string SessionID , string AgentIDVerify)
        {
            //FindAgentsResponse FAResponse = new FindAgentsResponse();
            IAgentManager agentApi = new AgentManagerClient();
            //FAResponse = NewFindingAgent(agentName, SessionID);
            //long agentId = 0;
            //if (FAResponse != null)
            //{
            //    for (int i = 0; i < FAResponse.FindAgentResponseData.FindAgentList.Length; i++)
            //    {
            //        if (FAResponse.FindAgentResponseData.FindAgentList[i].AgentID.ToString().Trim() == AgentIDVerify.Trim())
            //        {
            //            agentId = FAResponse.FindAgentResponseData.FindAgentList[i].AgentID;
            //        }
            //    }
            //}
            //else
            //{
            //    return null;
            //}
            //long agentId = NewFindingAgent(agentName, SessionID).FindAgentResponseData.FindAgentList[0].AgentID;// 7754026;
            GetAgentRequest request = new GetAgentRequest();
            request.GetAgentReqData = new GetAgentRequestData();
            request.GetAgentReqData.AgentID = Convert.ToInt64(AgentIDVerify);
            request.Signature = SessionID;// SessionManager._signature;
            Agent agent;
            GetAgentResponse getResponse = agentApi.GetAgent(request);
            //string xml = GetXMLString(getResponse);
            return agent = getResponse.Agent;
        }

        public string GetOrganizationName(string agentName, string SessionID, string AgentID, string OrganizationCode)
        {
            //IAgentManager agentApi = new AgentManagerClient();
            //Agent getagent = GetAgentByID(agentName, SessionID, AgentID);

            //if (getagent != null)
            //{
            //    if (getagent.AgentIdentifier != null)
            //    {
            //        GetOrganizationRequest request = new GetOrganizationRequest();
            //        request.ContractVersion = this.ContractVersion;
            //        request.Signature = SessionID;
            //        request.GetOrganizationReqData = new GetOrganizationRequestData();
            //        request.GetOrganizationReqData.OrganizationCode = getagent.AgentIdentifier.OrganizationCode;
            //        request.GetOrganizationReqData.GetDetails = true;
            //        GetOrganizationResponse resp = agentApi.GetOrganization(request);
            //        if (resp != null && resp.Organization != null)
            //        {
            //            return resp.Organization.OrganizationName;
            //        }
            //    }
            //}
            //return "";

            IAgentManager agentApi = new AgentManagerClient();
            if (OrganizationCode != "")
            {
                GetOrganizationRequest request = new GetOrganizationRequest();
                request.ContractVersion = this.ContractVersion;
                request.Signature = SessionID;
                request.GetOrganizationReqData = new GetOrganizationRequestData();
                request.GetOrganizationReqData.OrganizationCode = OrganizationCode;
                request.GetOrganizationReqData.GetDetails = false;
                GetOrganizationResponse resp = agentApi.GetOrganization(request);
                if (resp != null && resp.Organization != null)
                {
                    return resp.Organization.OrganizationName;
                }
            }
            return "";
        }

        public GetOrganizationResponse GetOrganization(string agentName, string SessionID, string AgentID, string OrganizationCode)
        {
            IAgentManager agentApi = new AgentManagerClient();
            if (OrganizationCode != "")
            {
                GetOrganizationRequest request = new GetOrganizationRequest();
                request.ContractVersion = this.ContractVersion;
                request.Signature = SessionID;
                request.GetOrganizationReqData = new GetOrganizationRequestData();
                request.GetOrganizationReqData.OrganizationCode = OrganizationCode;
                request.GetOrganizationReqData.GetDetails = false;
                GetOrganizationResponse resp = agentApi.GetOrganization(request);
                if (resp != null && resp.Organization != null)
                {
                    return resp;
                }
            }
            return null;
        }

        public GetPersonResponse GetPersonByID(string agentName, string SessionID, string AgentID)
        {
            IPersonManager personManager = new PersonManagerClient();
            GetPersonRequest PersonRequest = new GetPersonRequest();
            PersonRequest.GetPersonRequestData = new GetPersonRequestData();
            long PersonID = GetAgentByID(agentName, SessionID, AgentID).PersonID;// 7750946;
            PersonRequest.GetPersonRequestData.GetPersonBy = new GetPersonBy();
            PersonRequest.GetPersonRequestData.GetPersonByPersonID = new GetPersonByPersonID();
            PersonRequest.GetPersonRequestData.GetPersonByPersonID.PersonID = PersonID;
            PersonRequest.Signature = SessionID;// SessionManager._signature;
            PersonRequest.ContractVersion = ContractVersion;
            //string xml = GetXMLString(PersonRequest);
            GetPersonResponse getResponse = personManager.GetPerson(PersonRequest);
            return getResponse;
            //APIAgentService.Person person = getResponse.Person;
        }

        //public GroupBooking.Model.Entity.Agent SetAgentInformation(string agentName, string SessionID)
        //{
        //    GroupBooking.Model.Entity.Agent agent = new Model.Entity.Agent();
        //    GetPersonResponse Response = GetPersonByID(agentName, SessionID);

        //    agent.AgentAddress = Response.Person.PersonAddressList[0].Address.AddressLine1;
        //    agent.AgentAddress1 = Response.Person.PersonAddressList[0].Address.AddressLine2;
        //    agent.AgentAddress2 = Response.Person.PersonAddressList[0].Address.AddressLine3;
        //    //agent.AgentCarrierCode=
        //    agent.AgentCity = Response.Person.PersonAddressList[0].Address.City;
        //    agent.AgentCompany = "";// Response.Person.PersonAddressList[0].Address.AddressLine3;
        //    agent.AgentCountry = Response.Person.PersonAddressList[0].Address.CountryCode;
        //    agent.AgentEmail = Response.Person.PersonEMailList[0].EMailAddress;
        //    agent.AgentTel = Response.Person.PersonPhoneList[0].Number;
        //    agent.AgentFirstName = Response.Person.PersonNameList[0].Name.FirstName;
        //    agent.AgentLastName = Response.Person.PersonNameList[0].Name.LastName;
        //    agent.AgentTitle = Response.Person.PersonNameList[0].Name.Title;
        //    agent.AgentMobilePhone = Response.Person.PersonPhoneList[0].PhoneCode;
        //    agent.AgentPostalCode = Response.Person.PersonAddressList[0].Address.PostalCode;
        //    return agent;
        //}
    }
}

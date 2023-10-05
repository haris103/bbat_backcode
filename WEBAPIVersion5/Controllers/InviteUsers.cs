using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using Microsoft.Graph;
using Microsoft.OpenApi.Any;
using WEBAPIVersion5.Models;
using Microsoft.Graph.Models.Security;
using Microsoft.AspNetCore.Authorization;
using Azure.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Cosmos;
using System.Text.Json;
using Newtonsoft.Json;
using Azure;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using System;

namespace WEBAPIVersion5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InviteUsers : ControllerBase
    {

        private readonly GraphServiceClient _graphServiceClient;

        public InviteUsers(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

       // [Authorize]
        [HttpPost("sendInvitations")]
        public async Task<ActionResult> sendInvitations([FromBody] EmailList emailList)
        {
            try
            {
                // Cosmos Region
                var endpoint = "https://batcosmosnebeedev01.documents.azure.com:443/";
                var masterkey = "GCRz7PyagZryStb2CZbzvSV26Xwng36nSAH6HuiJLrGtwaDzaj5ig1FUWngibtn1n8ZjwfcNRyNvACDbldP7rg==";
                var appData = new List<dynamic>();

                using (CosmosClient client = new CosmosClient(endpoint, masterkey))
                {
                    var container = client.GetContainer("batcosmosnebeedev01", "AD_App_Data");
                    
                    string getById = "SELECT * FROM c WHERE c.ApplicationID = '" + emailList.ClientId + "'";
                    string getAll = "SELECT * FROM c";

                    var iterator = container.GetItemQueryIterator<dynamic>(getById);
                    var page = await iterator.ReadNextAsync();

                    foreach (var item in page.Resource)
                    {
                        string _applicationId = item.ApplicationID;
                        string _appName = item.AppData.AppName;
                        string _tenantId = item.AppData.TenentID;
                        string _objectId = item.AppData.ObjectID;
                        string _clientSecretId = item.AppData.ClientSecretID;
                        string _clientSecretValue = item.AppData.ClientSecretValue;

                        appData.Add(new
                        {
                            ApplicationID = _applicationId,
                            AppName = _appName,
                            TenentID = _tenantId,
                            ObjectID = _objectId,
                            ClientSecretID = _clientSecretId,
                            ClientSecretValue = _clientSecretValue
                        });
                    }

                }


                // Invite Region
                var results = new List<Invitation>();

                foreach (var emailInfo in emailList.Emails)
                {
                    string tenantId = appData[0].TenentID;
                    string clientId = appData[0].ApplicationID;
                     string clientSecretId = appData[0].ClientSecretValue;
                    var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecretId);
                    var graph = new GraphServiceClient(clientSecretCredential);

                    var requestBody = new Invitation
                    {
                        InvitedUserEmailAddress = emailInfo.Email,
                        InvitedUserDisplayName= emailInfo.Name,
                       InviteRedirectUrl = "https://myapplications.microsoft.com/?tenantid=" + appData[0].TenentID,

                        InvitedUserMessageInfo = new InvitedUserMessageInfo
                        {
                            CustomizedMessageBody = emailInfo.Message + " " + emailInfo.Name
                        },
                        SendInvitationMessage = true,
                    };

                    var result = await graph.Invitations.PostAsync(requestBody);
                    results.Add(result);
                }

                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("getData")]
        public async Task<ActionResult> getData()
        {
            var endpoint = "https://batcosmosnebeedev01.documents.azure.com:443/";
            var masterkey = "GCRz7PyagZryStb2CZbzvSV26Xwng36nSAH6HuiJLrGtwaDzaj5ig1FUWngibtn1n8ZjwfcNRyNvACDbldP7rg==";
            var results1 = new List<dynamic>();

            using (CosmosClient client = new CosmosClient(endpoint, masterkey))
            {
                var container = client.GetContainer("batcosmosnebeedev01", "AD_App_Data");
               // var getById = "SELECT * FROM c WHERE c.ApplicationID = \"ca3aee63-a99b-4623-9f62-20216966d2c6\"";
                //string applicationId = "ca3aee63-a99b-4623-9f62-20216966d2c6";
                // string getById = "SELECT * FROM c WHERE c.ApplicationID = '" + emailList.ClientId + "'";
                var getAll = "SELECT * FROM c";
                var iterator = container.GetItemQueryIterator<dynamic>(getAll);
                var page = await iterator.ReadNextAsync();


                foreach (var item in page.Resource)
                {
                    string _applicationId = item.ApplicationID;
                    string _appName = item.AppData.AppName;
                    string _tenantId = item.AppData.TenentID;
                    string _objectId = item.AppData.ObjectID;
                    string _clientSecretId = item.AppData.ClientSecretID;
                    string _clientSecretValue = item.AppData.ClientSecretValue;

                    results1.Add(new
                    {
                        ApplicationID = _applicationId,
                        AppName = _appName,
                        TenentID = _tenantId,
                        ObjectID = _objectId,
                        ClientSecretID = _clientSecretId,
                        ClientSecretValue = _clientSecretValue
                    });
                }

            }
             return Ok(results1);

            
        }


        [Authorize]
        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser(CreateUser create_user)
        {
            try
            {
                var requestBody = new User
                {
                    AccountEnabled = true,
                    DisplayName = "hamid Khan 3",
                    MailNickname = "hamid3",
                    UserPrincipalName = "hamid.khan3@harismahmood203gmail.onmicrosoft.com",
                    JobTitle = "Developer 3",
                    UserType = "Guest",
                    CompanyName = "Dummy Company",
                    Department = "Dummy Department",
                    OfficeLocation = "dummy location",
                    City = "dummy city",
                    Mail = "hamid.khan2@gmail.com",
                    AccountEnabled = true,
                    DisplayName = create_user.DisplayName,
                    MailNickname = create_user.DisplayName,
                    UserPrincipalName = $"{create_user.firstName}.{create_user.lastName}203@harismahmood203gmail.onmicrosoft.com",
                    JobTitle = create_user.JobTitle,
                    UserType = create_user.UserType,
                    CompanyName = create_user.CompanyName,
                    Department = create_user.Department,
                    OfficeLocation = create_user.OfficeLocation,
                    City = create_user.City,
                    Mail = create_user.Mail,
                    PasswordProfile = new PasswordProfile
                    {
                        ForceChangePasswordNextSignIn = true,
                        Password = "xWwvJ]6NMw+bWH-d",
                    },
                };

                var result = await _graphServiceClient.Users.PostAsync(requestBody);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }
    }
}

//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace WEBAPIVersion5.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class Employee : ControllerBase
//    {
//    }
//}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Azure.Identity;
using Microsoft.Graph;
using WEBAPIVersion5.DAL;
using WEBAPIVersion5.Models;
using System.Security.Claims;
using Microsoft.OpenApi.Any;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using Microsoft.Graph.Models.TermStore;
using Microsoft.Kiota.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Abstractions.Authentication;

namespace WEBAPIVersion5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:scopes")]

    public class Employee : ControllerBase
    {

       // private readonly UserDBContext _context;
        private readonly BusinessUnitContext _businessUnitcontext;
        private readonly GraphServiceClient _graphServiceClient;

        public Employee(
            
            BusinessUnitContext businessUnitcontext,  GraphServiceClient graphServiceClient)
        {
          //  _context = context;
            _graphServiceClient = graphServiceClient;
            _businessUnitcontext = businessUnitcontext;
        }

        [Authorize]
        [HttpGet("employees")] 
        public IActionResult GetEmployees()
        {
            var users =  _graphServiceClient.Users.GetAsync().Result;
            return Ok(users);
        }

        [Authorize]
        [HttpGet("total-employees")]
        public IActionResult TotalEmployees()
        {
            var users = _graphServiceClient.Users.GetAsync().Result;
            return Ok(users?.Value?.Count);

        }

        //[HttpPost]
        //public IActionResult submitUserData(Userdata data)
        //{
        //    try
        //    {
        //     //   _context.Add(data);
        //     //   _context.SaveChanges();
        //        return Ok("Submitted");

        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(ex.Message);
        //    }
        //}
        [Authorize]
        [HttpGet("getbusinessunit/{employeeId}/{tenantId}")]
        public IActionResult GetBusinessUnit(string employeeId, string tenantId)
        {
            try
            {
                //string employeeId = "ca2344f1-22f3-45b4-8b94-38caa52d2628";
                //string tenantId = "06b0bc3b-8b4e-467f-a6d5-0d2db90af7dd";

                var businessUnit = _businessUnitcontext.BusinessUnit
                   .FirstOrDefault(e => e.employee_id == employeeId && e.tenant_id == tenantId);

                if(businessUnit == null)
                {
                    return NotFound("business unit not found");
                }
                    var team = _businessUnitcontext.Teams
                         .FirstOrDefault(e => e.employee_id == employeeId && e.businessunit_id == businessUnit.id);
                if (team == null)
                {
                    return NotFound("team not found");
                }
                var security = _businessUnitcontext.Security
                         .FirstOrDefault(e => e.employee_id == employeeId && e.businessunit_id == businessUnit.id && e.team_id == team.id);
                if (security == null)
                {
                    return NotFound("security role not found");
                }
                return Ok(security);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [Authorize]
        [HttpGet("listAppRoleAssignments/{employeeId}")]
        public async Task<ActionResult> listAppRoleAssignments(string employeeId)
        {
           // to get the collective
           var result = await  _graphServiceClient.Users[employeeId].AppRoleAssignments.GetAsync();

            // to get the specific
            //var result = await _graphServiceClient.Users["ca2344f1-22f3-45b4-8b94-38caa52d2628"].AppRoleAssignments.GetAsync((requestConfiguration) =>
            //{
            //    requestConfiguration.QueryParameters.Filter = "resourceId eq 8fb8b487-cd8b-4136-83ad-5b734e5d2574";
            //});
            return Ok(result);
        }

        [Authorize]
        [HttpPost("grantAppRoleAssignment/{user_id}/{resource_id}/{role_id}")]
        public async Task<ActionResult> grantAppRoleAssignment(string user_id, string resource_id, string role_id)
        {
            try
            {
            var requestBody = new AppRoleAssignment
                {
                    // PrincipalId = Guid.Parse("5c086019-0cc9-49ab-a917-49408482f64a"), // user-id
                    // ResourceId = Guid.Parse("8fb8b487-cd8b-4136-83ad-5b734e5d2574"), //  application id/ business id to target
                    // AppRoleId = Guid.Parse("a4ffff26-75c0-48a9-8f03-aa2fd54c5c58"), // role-id

                PrincipalId = Guid.Parse(user_id), // user-id
                ResourceId = Guid.Parse(resource_id), //  application id/ business id to target
                AppRoleId = Guid.Parse(role_id), // role-id
            };
                var result = await _graphServiceClient.Users[user_id].AppRoleAssignments.PostAsync(requestBody);
                return Ok(result);
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
    }
    }

        // [HttpGet("getdetails")]
        //public IActionResult getDetails()
        //{
        //    try
        //    {
        //        var identity = User.Identity;

        //        // Getting claim of type 'aud' only
        //        var audClaim = User.Claims.FirstOrDefault(c => c.Type == "aud");

        //        if (audClaim != null)
        //        {
        //            var businessUnitResponse = new
        //            {
        //                Type = audClaim.Type,
        //                Value = audClaim.Value
        //            };

        //            // Finding business object
        //            var systemUnit = _businessUnitcontext.Usersystem
        //              .FirstOrDefault(e => e.businessunit_id == businessUnitResponse.Value);

        //            if (systemUnit == null)
        //            {
        //                return NotFound("System Unit not found");
        //            }

        //            // Finding Team object
        //            var team = _businessUnitcontext.Teamsystem
        //            .FirstOrDefault(e => e.systemunit_id == systemUnit.id);

        //            if (team == null)
        //            {
        //                return NotFound("Team Unit not found");
        //            }
        //            return Ok(team);
        //        }
        //        else
        //        {
        //            return NotFound("Claim of type 'aud' not found.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(ex.Message);

        //    }
        //}
    }
    }
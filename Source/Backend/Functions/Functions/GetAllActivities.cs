using database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Shared.Services;
using System.Net;

namespace Functions.Functions;

public class GetAllActivities
{
    private readonly ILogger<GetAllActivities> _logger;
    private readonly IActivityService _activityService;

    public GetAllActivities(ILogger<GetAllActivities> logger, IActivityService activityService)
    {
        _logger = logger;
        _activityService = activityService;
    }

    [Function("GetAllActivitiesV1")]
    [OpenApiOperation(operationId: "GetAllActivitiesV1", Summary = "Retrieves all activities for a specific user", Description = "This API retrieves all activities associated with a given user GUID.")]
    [OpenApiParameter(name: "userGuid", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The unique identifier for the user.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<ActivityModel>), Description = "A list of activities for the specified user.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid GUID format or missing required parameter.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "No activities found for the specified user GUID.")]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/user/{userGuid}/activities")] HttpRequest req, [FromRoute] Guid userGuid)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var res = await _activityService.GetAll(userGuid);
        if (res == null || !res.Any())
            return new NoContentResult();
        return new OkObjectResult(res);
    }
}

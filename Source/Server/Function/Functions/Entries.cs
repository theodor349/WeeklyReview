using Function.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Shared.Models;
using Shared.Services;
using System.Net;
using System.Text.Json;

namespace Function.Functions
{
    public class Entries(ILogger<Entries> logger, IEntryService entryService)
    {
        [Function(nameof(Enter))]
        [OpenApiOperation(operationId: nameof(Enter), Summary = "Adds a new entry", Description = "This API adds a new entry with activities for a specified date and user GUID.")]
        [OpenApiRequestBody("application/json", typeof(AddEntryDto), Description = "The entry data containing the date, activities, and user GUID.", Required = true)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "Entry added successfully.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid request payload or missing required fields.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.InternalServerError, Description = "An error occurred while adding the entry.")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        public async Task<IActionResult> Enter([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/entry")] HttpRequest req)
        {
            try
            {
                var entry = await JsonSerializer.DeserializeAsync<AddEntryDto>(req.Body);

                if (entry == null)
                {
                    logger.LogWarning("Deserialized entry is null.");
                    return new BadRequestObjectResult("Invalid input. Entry data is required.");
                }

                logger.LogInformation($"Entering {entry.Entries.Count} activities at {entry.Date:yyyy-MM-dd HH:mm}");
                await entryService.Create(new EnterEntryModel { Entries = entry.Entries, Date = entry.Date }, entry.UserGuid);
                return new OkResult();
            }
            catch (JsonException jsonEx)
            {
                logger.LogError(jsonEx, "JSON deserialization failed.");
                return new BadRequestObjectResult("Invalid JSON format.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the request.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

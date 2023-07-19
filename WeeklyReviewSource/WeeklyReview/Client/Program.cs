using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WeeklyReview.Client;
using Syncfusion.Blazor;
using WeeklyReview.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("WeeklyReview.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("WeeklyReview.ServerAPI"));
builder.Services.AddSingleton<IWeeklyReviewService, WeeklyReviewService>();

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add(builder.Configuration.GetSection("ServerApi")["Scopes"]);
});

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt+QHJqVk1hXk5Hd0BLVGpAblJ3T2ZQdVt5ZDU7a15RRnVfRF1iSXpSckBkWnlddw==;Mgo+DSMBPh8sVXJ1S0R+X1pFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF5jT35SdkFiXXpZdXdVQQ==;ORg4AjUWIQA/Gnt2VFhiQlJPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXhTd0VnWHtbd3JcQ2c=;MjI3OTc0N0AzMjMxMmUzMDJlMzBHSW9lQ2ZJeUFFZWZmSmRncjA5VXpoWmxicmlEVWlXZXRBMDBOcnh1WG9VPQ==;MjI3OTc0OEAzMjMxMmUzMDJlMzBBTHRla2VSN0d0SDF1Mk5vR1M5bmhuSXVYcXhnRUF2RStJRUdFeTUzcTN3PQ==;NRAiBiAaIQQuGjN/V0d+Xk9HfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5VdkRjXX5adXxQRmhb;MjI3OTc1MEAzMjMxMmUzMDJlMzBkaUdHMXUxb2htaVdKdWdTYVJqZGR1bGxpaCtITitzVE1VL0RKaDNxSFJNPQ==;MjI3OTc1MUAzMjMxMmUzMDJlMzBVSjJHc2lOWkI0bG1jWVJhYXpVU3NlQnNrZDhpVXVxKzVtSEhYRVUyNGNrPQ==;Mgo+DSMBMAY9C3t2VFhiQlJPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXhTd0VnWHtbeHxTRGU=;MjI3OTc1M0AzMjMxMmUzMDJlMzBlemtDclVYUFJTVFlzVU04SVdWcjkzdWl4WlB2cG0yZFhQdG94OGZLZTIwPQ==;MjI3OTc1NEAzMjMxMmUzMDJlMzBuM1dCY010RDdmaXdKck5EaGJyTmRxeml0Y2lVM0hhVFloc003NnRqbUJrPQ==;MjI3OTc1NUAzMjMxMmUzMDJlMzBkaUdHMXUxb2htaVdKdWdTYVJqZGR1bGxpaCtITitzVE1VL0RKaDNxSFJNPQ==");
builder.Services.AddSyncfusionBlazor();
builder.Services.AddSingleton<WeeklyReviewService>();

await builder.Build().RunAsync();

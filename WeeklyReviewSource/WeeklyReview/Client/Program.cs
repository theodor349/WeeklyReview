using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;
using WeeklyReview.Client;
using WeeklyReview.Client.Http;
using WeeklyReview.Client.Persistance;
using WeeklyReview.Client.Services;
using WeeklyReview.Database.Persitance;
using WeeklyReview.Shared;
using WeeklyReview.Shared.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("WeeklyReview.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
    //.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("WeeklyReview.ServerAPI"));
builder.Services.AddDbContext<WeeklyReviewBlazorClientDbContext>(options =>
{
    //options.UseLazyLoadingProxies();
    options.UseInMemoryDatabase(databaseName: "WeeklyReview"); 
});
builder.Services.AddScoped<WeeklyReviewDbContext, WeeklyReviewBlazorClientDbContext>();
builder.Services.AddSharedServices();
builder.Services.AddTransient<IClientWeeklyReviewService, ClientWeeklyReviewService>();
builder.Services.AddTransient<WeeklyReviewApiClient>();
builder.Services.AddTransient<IActivityChangeService, ActivityChangeService>();
builder.Services.AddTransient<IActivityService, ActivityService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IEntryService, EntryService>();
builder.Services.AddTransient<IApiWeeklyReviewService, ApiWeeklyReviewService>();

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add(builder.Configuration.GetSection("ServerApi")["Scopes"]);
});

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cWWdCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWH5fdXVWRmBdUEN+XUY=");
builder.Services.AddScoped<SfDialogService>();
builder.Services.AddSyncfusionBlazor();

await builder.Build().RunAsync();

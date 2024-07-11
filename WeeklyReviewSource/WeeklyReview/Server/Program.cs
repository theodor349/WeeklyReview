using Microsoft.AspNetCore.Mvc.ApiExplorer;
using WeeklyReview.Server.Setup;
using WeeklyReview.Shared;

var allowedOrigins = "_allowOrigins";


var builder = WebApplication.CreateBuilder(args);
builder.AddConfigurationProviders();
builder.AddAuthentication();
builder.AddCors(allowedOrigins);
builder.AddDatabases();
builder.AddEndpoints();
builder.Services.AddSharedServices();

var app = builder.Build();
var db = builder.Configuration.GetConnectionString("WeeklyReview");
Console.WriteLine("DB Connection: " + db.Substring(0, 60));

app.UseSwagger();
var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
app.UseSwaggerUI(options =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
        "/swagger/" + description.GroupName + "/swagger.json",
        description.GroupName.ToUpperInvariant()
        );
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseCors(allowedOrigins);
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

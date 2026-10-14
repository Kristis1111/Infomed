using Infomed.API.Middleware;
using Infomed.Core.Caching;
using Infomed.Data.Autocomplete;
using Infomed.Data.Doctors;
using Infomed.Data.DropDowns;
using Infomed.Domain.AutoMapper.Profiles;
using Infomed.Services.Autocomplete;
using Infomed.Services.Doctors;
using Infomed.Services.DropDowns;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(configuration =>
{
    configuration.AddProfile(new DoctorMappingProfile());
    configuration.AddProfile(new ContactRequestMappingProfile());
    configuration.AddProfile(new AutocompleteMappingProfile());
});

// Should be Redis in prod
builder.Services.AddMemoryCache();

// Should be managed on Azure
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IDoctorDataService, DoctorDataJsonService>();
builder.Services.AddScoped<IAutocompleteService, AutocompleteService>();
builder.Services.AddScoped<IAutocompleteDataService, AutocompleteDataJsonService>();
builder.Services.AddScoped<IDropDownService, DropDownService>();
builder.Services.AddScoped<IDropDownDataService, DropDownDataService>();
// Singleton on purpose - so all requests share the same file-write lock
builder.Services.AddSingleton<IContactRequestDataService, ContactRequestJsonService>();

//  Should be Redis
builder.Services.AddSingleton<IApplicationCache, InMemoryApplicationCache>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestInterceptorMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
// Request validation middleware should be added
// Sanitization middleware should be added
// Response headers middleware should be added

app.UseHttpsRedirection();

// Should be managed on Azure
app.UseCors("AngularClient");


//app.UseAuthentication();
//app.UseAuthorization();

//app.UseRateLimiter();

app.MapControllers();

//app.MapHealthChecks("/health");

app.Run();

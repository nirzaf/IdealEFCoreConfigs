using IdealEFCoreConfigs.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Identity.Web;


var builder = WebApplication.CreateBuilder(args);
bool isDevelopment = builder.Configuration.GetValue<bool>("IsDevelopment");
// Add services to the container.
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPooledDbContextFactory<AppDbContext>(opt => 
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnectionString"), options =>
    {
        options.EnableRetryOnFailure(
            maxRetryCount:4,
            maxRetryDelay:TimeSpan.FromSeconds(2),
            errorNumbersToAdd:new int[]{}
        );
        opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        if (isDevelopment)
        {
            opt.EnableDetailedErrors();
            opt.EnableSensitiveDataLogging();
            opt.ConfigureWarnings(warnings =>
            {
                warnings.Log( new EventId[]
                {
                    CoreEventId.FirstWithoutOrderByAndFilterWarning,
                    CoreEventId.RowLimitingOperationWithoutOrderByWarning
                });
            });
        }
    }));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();
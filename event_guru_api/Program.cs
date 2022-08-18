using System.Text;
using System.Text.Json.Serialization;
using event_guru_api.models;
using event_guru_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

builder.Services
    .AddControllers(options => options.UseDateOnlyTimeOnlyStringConverters())
    .AddJsonOptions(options =>
    {
        options.UseDateOnlyTimeOnlyStringConverters();
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen(c => c.UseDateOnlyTimeOnlyStringConverters());

//1. Add the database context to the application
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    string connStr;
    if (env == "Development")
    {
        connStr = builder.Configuration.GetConnectionString("DefaultConnection");
    }
    else
    {
        //Use connection string provided by runtime by heroku
        //the connection-
        var connUrl = Environment.GetEnvironmentVariable("CLEARDB_DATABASE_URL");
        connUrl = connUrl.Replace("mysql://", String.Empty);
        var userPassSide = connUrl.Split("@")[0];
        var hostSide = connUrl.Split("@")[1];

        var connUser = userPassSide.Split(":")[0];
        var connPass = userPassSide.Split(":")[1];
        var connHost = hostSide.Split("/")[0];
        var connDb = hostSide.Split("/")[1].Split("?")[0];
        connStr = $"Server={connHost};Database={connDb};Uid={connUser};Pwd={connPass};SSL Mode=None";
    }

    options.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
});
//2.Adding the identity by dependency injection
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
        .WithOrigins("https://localhost:7027;http://localhost:5084")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})


//Adding Jwt Bearer
 .AddJwtBearer(options =>
 {
     options.SaveToken = true;
     options.RequireHttpsMetadata = false;
     options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidAudience = configuration["JWT:ValidAudience"],
         ValidIssuer = configuration["JWT:ValidIssuer"],
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
     };
 });

builder.Services.AddScoped<IEventBudgetService, EventBudgetService>();
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<ISMSSenderService, SMSSenderService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
//Authentication and Authorization
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

